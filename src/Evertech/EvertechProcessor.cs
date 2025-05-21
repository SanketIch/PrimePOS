using Evertech.Common;
using Evertech.Data;
using Evertech.Implementation;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NLog;
using PossqlData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Evertech
{
    public class EvertechProcessor
    {
        #region variables

        long RefNumber;
        bool isSignatureReceived = false;
        String SettleIndicator = "S";
        public int LRC;
        private String IPAddress = Constant.DefaultIPAddress;//constant
        private int Port = 0;
        DeviceComm device = null;
        private PmtTxnResponse evertecDevResponse = null;
        private SettleTxnResponse evertecSettleResponse = null;
        private static EvertechProcessor evertecProcessor = null;
        public bool isLoggedOn = false;
        private const int MAX_RETRIES = 3;

        #endregion
        #region properties


        #endregion
        ILogger logger = LogManager.GetCurrentClassLogger();

        private EvertechProcessor(String IPaddress, int Port)
        {
            device = new DeviceComm();
            device.IPaddress = IPaddress;
            device.Port = Port;
        }

        public static EvertechProcessor getInstance(String IPAddress, int Port)
        {
            if (evertecProcessor == null)
            {
                evertecProcessor = new EvertechProcessor(IPAddress, Port);
                evertecProcessor.Port = Port;
                evertecProcessor.IPAddress = IPAddress;
            }
            return evertecProcessor;
        }

        //public string BuildRequest(Enum txnType, string strAmount)
        //{       
        //    return "";
        //}

        public PmtTxnResponse Sale(Dictionary<String, String> fields)
        {
            PmtTxnResponse EvertecSaleResponse = new PmtTxnResponse();
            try
            {
                if (isLoggedOn == false)
                {
                    MessageBox.Show("The Device is not connected ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return EvertecSaleResponse;
                }
                logger.Trace("ENTERED THE SALE METHOD IN EVERTEC");

                String[] RespData;
                int MsgLength;
                char ResponseLRC;
                string DuplicateTxn = (fields["ALLOWDUP"] == "False") ? "0" : "1";
                string manualEntry = "0";
                string cityTaxAmount = string.Empty;
                string stateTaxAmount = string.Empty;
                string reduceStateTaxAmount = string.Empty;
                string reduceCityTaxAmount = string.Empty;//PRIMEPOS-3099
                string reduceStateTaxPercent = string.Empty;//PRIMEPOS-3099


                DataTable dt = (DataTable)JsonConvert.DeserializeObject(fields["TAX_DETAILS"], typeof(DataTable));
                string totalAmount = string.Empty;
                string totalTaxAmount = string.Empty;

                totalAmount = dt.AsEnumerable().Select(a => a.Field<string>("TotalAmount")).FirstOrDefault();
                totalTaxAmount = dt.AsEnumerable().Select(a => a.Field<string>("TotalTaxAmount")).FirstOrDefault();
                stateTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                cityTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.Municipality && a.Field<string>("TaxCode").ToUpper().Contains("PRM")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                reduceStateTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                reduceStateTaxPercent = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxPercent")).SingleOrDefault();//PRIMEPOS-3099 

                CalculateTaxAmount(dt, fields, totalAmount, totalTaxAmount, reduceStateTaxPercent, ref cityTaxAmount, ref stateTaxAmount, ref reduceStateTaxAmount, ref reduceCityTaxAmount);//PRIMEPOS-3099



                if (fields.ContainsKey("ISMANUAL"))
                {
                    manualEntry = (fields["ISMANUAL"] == "false") ? "0" : "1";
                }
                string basicMessageFormat = string.Empty;

                string prosCode = "000000";
                string cashbackAmount = "000000000000";
                if (fields.Keys.Contains("ISALLOWDUP"))
                {
                    if (fields.Keys.Contains("CASHBACK_AMOUNT"))
                    {
                        prosCode = "090000";
                        cashbackAmount = Strings.Format(Conversion.Val(fields["CASHBACK_AMOUNT"]) * 100, "000000000000");
                    }
                    else if (fields["ISALLOWDUP"].ToString().ToLower() == "true")
                        prosCode = "009100";
                    else if (fields["ISALLOWDUP"].ToString().ToLower() == "true" && fields.Keys.Contains("CASHBACK_AMOUNT"))
                    {
                        prosCode = "099100";
                        cashbackAmount = Strings.Format(Conversion.Val(fields["CASHBACK_AMOUNT"]) * 100, "000000000000");
                    }
                }
                //ReadRefNumber();
                //Format specific data before sending out
                long ReferenceNumber = GetNextRefNumber();
                String EcrNumber = Strings.Format(ReferenceNumber, "000000");

                //fields["AMOUNT"] = (Convert.ToDecimal(fields["AMOUNT"]) + Convert.ToDecimal(cityTaxAmount)+ Convert.ToDecimal(stateTaxAmount)+ Convert.ToDecimal(reduceStateTaxAmount)).ToString();

                String strAmount = Strings.Format(Conversion.Val(fields["AMOUNT"]) * 100, "000000000000");
                cityTaxAmount = Strings.Format(Conversion.Val(cityTaxAmount) * 100, "000000000000");
                stateTaxAmount = Strings.Format(Conversion.Val(stateTaxAmount) * 100, "000000000000");
                reduceStateTaxAmount = Strings.Format(Conversion.Val(reduceStateTaxAmount) * 100, "000000000000");
                reduceCityTaxAmount = Strings.Format(Conversion.Val(reduceCityTaxAmount) * 100, "000000000000");//PRIMEPOS-3099               
                
                //cityTaxAmount = Strings.Format(Conversion.Val(cityTaxAmount) / 100, "000000000000");
                //Build request 
                if (fields["TRANSACTIONTYPE"].Contains("CREDIT"))
                {
                    //Credit
                    basicMessageFormat = String.Format(Constant.FormatID + Constant.Sale + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + "/////" + stateTaxAmount + "/" + cityTaxAmount + "//" + reduceStateTaxAmount + "/" + reduceCityTaxAmount + "//////" + "{0}", Strings.ChrW(28));//PRIMEPOS-3099
                    //basicMessageFormat = String.Format(Constant.FormatID + Constant.Sale + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + "/////" + stateTaxAmount + "/" + cityTaxAmount + "////////" + "{0}", Strings.ChrW(28));
                }
                else
                {
                    //Debit
                    basicMessageFormat = String.Format(Constant.FormatID + Constant.Sale + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + "/////" + stateTaxAmount + "/" + cityTaxAmount + "//" + reduceStateTaxAmount + "/" + reduceCityTaxAmount + "//////" + "{0}", Strings.ChrW(28));//PRIMEPOS-3099
                }
                //calculate LRC
                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                //Send Request
                string request = SendRequest(Constant.Transaction, basicMessageFormat, LRC);

                logger.Trace("The Request for SALE is " + request);

                string response = string.Empty;
                //response = "318^SALE.333332330001000103206300/APPROVED/0021402300000000000000065943320000000000221430150223AM0000010000391454910872025         //ATH/A//000000000059/000000000006//000000000034/000000000000/114904/////ATH MOVIL/NONE///NOSIGN/ /D/M062CONTROL: 21D98-L7PS9ET                                        ^v ";
                //response = "378^SALE.333332330001000103182000/APPROVED/0012368300000000000000235000360000000006581533150128VC0000160006340454910872025         //ATH/A//000000000210/000000000020/////////VISA/HATHECRP///NOSIGN/ /C/C062CONTROL: 3D6PW-CCP1JET                                        Visa Credit/AID: A0000000031010/AC: B57E00BC0A40AFF3/UN: D80E2FBE/TVR: 0080008000/TSI: F800^";
                //Receive Response
                do
                {
                    device.ReceiveMessage(out response);
                    if (response != string.Empty)
                    {
                        RespData = response.Split(Strings.ChrW(0x5E));
                        MsgLength = Strings.Len(RespData[1]);
                        LRC = CalculateLRC(MsgLength, RespData[1]);
                        ResponseLRC = response.ElementAt(response.Length - 1);
                        if (Strings.Chr(LRC) == ResponseLRC)
                        {
                            EvertecSaleResponse.ParseEvertechResponse(response);
                        }
                        else
                        {
                            logger.Error("INVALID LRC IN SALE EVERTECPROCESSOR");
                            EvertecSaleResponse.ParseEvertechResponse(response);
                        }
                    }
                } while (EvertecSaleResponse.isStatusMessage);

                logger.Trace("The Response for SALE is " + response);

                EvertecSaleResponse.request = request;//Request for Saving in CC_Transmissionlog

                SaveRefNumber(ReferenceNumber); //Setting the Response and Saving it 

                if (EvertecSaleResponse.EmvReceipt != null)
                    EvertecSaleResponse.EmvReceipt.ReferenceNumber = EcrNumber;

                device.Disconnect();//Disconnecting the Socket

                //Checking if the Response Contains SIGN and Getting the Signature if it contains SIGN
                if (EvertecSaleResponse.isSignature == true)
                {
                    EvertecSaleResponse.SignatureString = GetSignature().SignatureData;
                    EvertecSaleResponse.EmvReceipt.IsEvertecSign = true;//PRIMEPOS-2831
                }

                if (EvertecSaleResponse.ResultDescription == "SUCCESS" && prosCode == "009100")
                {
                    EvertecSaleResponse.EmvReceipt.IsEvertecForceTransaction = true;
                }
                if (EvertecSaleResponse.ResultDescription == "SUCCESS" && (prosCode == "090000" || prosCode == "099100"))
                {
                    EvertecSaleResponse.CashBack = (Convert.ToDecimal(cashbackAmount) / 100).ToString();
                }
                if (EvertecSaleResponse.ResultDescription == "SUCCESS")
                {
                    EvertecSaleResponse.EmvReceipt.EvertecTaxBreakdown = (Convert.ToDecimal(cityTaxAmount) / 100).ToString() + "|" + (Convert.ToDecimal(stateTaxAmount) / 100).ToString() + "|" + (Convert.ToDecimal(reduceStateTaxAmount) / 100).ToString();//PRIMEPOS-3099
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return EvertecSaleResponse;
        }

        public PmtTxnResponse Refund(Dictionary<String, String> fields)
        {
            PmtTxnResponse EvertecRefundResponse = new Data.PmtTxnResponse();
            try
            {
                if (isLoggedOn == false)
                {
                    MessageBox.Show("The Device is not connected ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return EvertecRefundResponse;
                }

                logger.Trace("ENTERED THE REFUND METHOD IN EVERTEC");

                String[] RespData;
                int MsgLength;
                char ResponseLRC;


                //ReadRefNumber();
                //Format specific data before sending out
                long ReferenceNumber = GetNextRefNumber();
                string EcrNumber = Strings.Format(ReferenceNumber, "000000");
                string cityTaxAmount = string.Empty;
                string stateTaxAmount = string.Empty;
                string reduceStateTaxAmount = string.Empty;
                string reduceCityTaxAmount = string.Empty;//PRIMEPOS-3099
                string reduceStateTaxPercent = string.Empty;//PRIMEPOS-3099

                DataTable dt = (DataTable)JsonConvert.DeserializeObject(fields["TAX_DETAILS"], typeof(DataTable));
                string totalAmount = string.Empty;
                string totalTaxAmount = string.Empty;

                totalAmount = dt.AsEnumerable().Select(a => a.Field<string>("TotalAmount")).FirstOrDefault();
                totalTaxAmount = dt.AsEnumerable().Select(a => a.Field<string>("TotalTaxAmount")).FirstOrDefault();
                stateTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                cityTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.Municipality && a.Field<string>("TaxCode").ToUpper().Contains("PRM")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                reduceStateTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                reduceStateTaxPercent = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxPercent")).SingleOrDefault();//PRIMEPOS-3099

                CalculateTaxAmount(dt, fields, totalAmount, totalTaxAmount, reduceStateTaxPercent, ref cityTaxAmount, ref stateTaxAmount, ref reduceStateTaxAmount, ref reduceCityTaxAmount);//PRIMEPOS-3099


                string strAmount = Strings.Format(Conversion.Val(fields["AMOUNT"]) * 100, "000000000000");
                cityTaxAmount = Strings.Format(Conversion.Val(cityTaxAmount) * 100, "000000000000");
                stateTaxAmount = Strings.Format(Conversion.Val(stateTaxAmount) * 100, "000000000000");
                reduceStateTaxAmount = Strings.Format(Conversion.Val(reduceStateTaxAmount) * 100, "000000000000");
                reduceCityTaxAmount = Strings.Format(Conversion.Val(reduceCityTaxAmount) * 100, "000000000000");//PRIMEPOS-3099
                //BuildRequest
                if (strAmount.Contains("-"))
                {
                    strAmount = strAmount.Substring(1);//Did this because of the negative Amount for Return Transaction as EVERTEC doesn't support negative Amount 
                }
                string manualEntry = "0";
                if (fields.ContainsKey("ISMANUAL"))
                {
                    manualEntry = (fields["ISMANUAL"] == "false") ? "0" : "1";
                }

                string duplicatetrans = "000000";
                if (fields.Keys.Contains("ISALLOWDUP"))
                {
                    if (fields["ISALLOWDUP"].ToString().ToLower() == "true")
                        duplicatetrans = "009100";
                }
                string basicMessageFormat = String.Format(Constant.FormatID + Constant.Refund + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + duplicatetrans + "{0}" + strAmount + "{0}" + "000000000000" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + "/////" + stateTaxAmount + "/" + cityTaxAmount + "//" + reduceStateTaxAmount + "//" + reduceCityTaxAmount + "////" + "{0}", Strings.ChrW(28));//PRIMEPOS-3099

                //Calculate LRC
                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                //Send Message
                string request = SendRequest(Constant.Transaction, basicMessageFormat, LRC);

                logger.Trace("The Request for REFUND is " + request);

                string response = string.Empty;
                //Receive Message
                do
                {
                    device.ReceiveMessage(out response);
                    if (response != string.Empty)
                    {
                        RespData = response.Split(Strings.ChrW(0x5E));
                        MsgLength = Strings.Len(RespData[1]);
                        LRC = CalculateLRC(MsgLength, RespData[1]);
                        ResponseLRC = response.ElementAt(response.Length - 1);
                        if (Strings.Chr(LRC) == ResponseLRC)
                        {
                            EvertecRefundResponse.ParseEvertechResponse(response);
                        }
                        else
                        {
                            logger.Error("INVALID LRC AT REFUND EVERTECPROCESSOR");
                            EvertecRefundResponse.ParseEvertechResponse(response);
                        }
                    }
                } while (EvertecRefundResponse.isStatusMessage);

                logger.Trace("The Response for REFUND is " + response);

                EvertecRefundResponse.request = request;//Request for Saving in CC_Transmissionlog

                SaveRefNumber(ReferenceNumber); //Setting the Response and Saving it 

                if (EvertecRefundResponse.EmvReceipt != null)
                    EvertecRefundResponse.EmvReceipt.ReferenceNumber = EcrNumber;

                device.Disconnect();

                if (EvertecRefundResponse.isSignature == true)
                {
                    EvertecRefundResponse.SignatureString = GetSignature().SignatureData;
                    EvertecRefundResponse.EmvReceipt.IsEvertecSign = true;//PRIMEPOS-2831
                }
                //PRIMEPOS-2831
                if (EvertecRefundResponse.ResultDescription == "SUCCESS" && duplicatetrans == "009100")
                {
                    EvertecRefundResponse.EmvReceipt.IsEvertecForceTransaction = true;
                }
                if (EvertecRefundResponse.ResultDescription == "SUCCESS")
                {
                    EvertecRefundResponse.EmvReceipt.EvertecTaxBreakdown = (Convert.ToDecimal(cityTaxAmount) / 100).ToString() + "|" + (Convert.ToDecimal(stateTaxAmount) / 100).ToString() + "|" + (Convert.ToDecimal(reduceStateTaxAmount) / 100).ToString();//PRIMEPOS-3099
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return EvertecRefundResponse;
        }

        public PmtTxnResponse EBT(Dictionary<String, String> fields)
        {
            evertecDevResponse = new PmtTxnResponse();
            try
            {
                if (isLoggedOn == false)
                {
                    MessageBox.Show("The Device is not connected ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return evertecDevResponse;
                }
                logger.Trace("ENTERED THE EBT METHOD IN EVERTEC");

                String[] aRespData;
                int iMsgLength;
                char cResponseLRC;
                string manualEntry = "0";
                if (fields.ContainsKey("ISMANUAL"))
                {
                    manualEntry = (fields["ISMANUAL"] == "false") ? "0" : "1";
                }


                //ReadRefNumber();
                //Format specific data before sending out
                long ReferenceNumber = GetNextRefNumber();
                String EcrNumber = Strings.Format(ReferenceNumber, "000000");
                string EBTTransactionOption = string.Empty;
                string cityTaxAmount = string.Empty;
                string stateTaxAmount = string.Empty;
                string reduceStateTaxAmount = string.Empty;
                string reduceCityTaxAmount = string.Empty;//PRIMEPOS-3099
                string reduceStateTaxPercent = string.Empty;//PRIMEPOS-3099

                if (fields["AMOUNT"].Contains("-"))
                {
                    fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                }

                DataTable dt = (DataTable)JsonConvert.DeserializeObject(fields["TAX_DETAILS"], typeof(DataTable));
                string totalAmount = string.Empty;
                string totalTaxAmount = string.Empty;

                totalAmount = dt.AsEnumerable().Select(a => a.Field<string>("TotalAmount")).FirstOrDefault();
                totalTaxAmount = dt.AsEnumerable().Select(a => a.Field<string>("TotalTaxAmount")).FirstOrDefault();
                stateTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                cityTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.Municipality && a.Field<string>("TaxCode").ToUpper().Contains("PRM")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                reduceStateTaxAmount = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxAmount")).SingleOrDefault();
                reduceStateTaxPercent = dt.AsEnumerable().Where(a => !string.IsNullOrWhiteSpace(a.Field<string>("Description")) && Convert.ToInt32(a.Field<string>("TaxType")) == (int)TaxTypes.State && a.Field<string>("TaxCode").ToUpper().Contains("PRRS")).Select(s => s.Field<string>("TaxPercent")).SingleOrDefault();//PRIMEPOS-3099

                //fields["AMOUNT"] = (Convert.ToDecimal(fields["AMOUNT"]) + Convert.ToDecimal(cityTaxAmount) + Convert.ToDecimal(stateTaxAmount)).ToString();

                CalculateTaxAmount(dt, fields, totalAmount, totalTaxAmount, reduceStateTaxPercent, ref cityTaxAmount, ref stateTaxAmount, ref reduceStateTaxAmount, ref reduceCityTaxAmount);

                string prosCode = "000000";
                string cashbackAmount = "000000000000";
                if (fields.Keys.Contains("ISALLOWDUP"))
                {
                    if (fields["ISALLOWDUP"].ToString().ToLower() == "true")
                    {
                        if (fields.Keys.Contains("CASHBACK_AMOUNT"))
                        {
                            prosCode = "099100";
                            cashbackAmount = Strings.Format(Conversion.Val(fields["CASHBACK_AMOUNT"]) * 100, "000000000000");
                        }
                        else
                        {
                            prosCode = "009100";
                        }
                    }
                    else if (fields.Keys.Contains("CASHBACK_AMOUNT"))
                    {
                        prosCode = "090000";
                        cashbackAmount = Strings.Format(Conversion.Val(fields["CASHBACK_AMOUNT"]) * 100, "000000000000");
                    }
                }

                if (fields.Keys.Contains("EBTFOODSTAMP"))
                {
                    if (fields["EBTFOODSTAMP"].ToString().ToLower() == "true")
                    {
                        EBTTransactionOption = "EBT/FOODPURCH";
                    }
                    else
                    {
                        if (fields.Keys.Contains("CASHBACK_AMOUNT"))
                        {
                            EBTTransactionOption = "EBT/CASHBACK";
                        }
                        else
                        {
                            EBTTransactionOption = "EBT/CASHPURCH";
                        }
                    }
                }

                String strAmount = Strings.Format(Conversion.Val(fields["AMOUNT"]) * 100, "000000000000");
                cityTaxAmount = Strings.Format(Conversion.Val(cityTaxAmount) * 100, "000000000000");
                stateTaxAmount = Strings.Format(Conversion.Val(stateTaxAmount) * 100, "000000000000");
                reduceStateTaxAmount = Strings.Format(Conversion.Val(reduceStateTaxAmount) * 100, "000000000000");
                reduceCityTaxAmount = Strings.Format(Conversion.Val(reduceCityTaxAmount) * 100, "000000000000");

                string basicMessageFormat = String.Empty;
                //BuildRequest

                if (fields["TRANSACTIONTYPE"].ToUpper().Contains("SALE"))
                {
                    if (fields.ContainsKey("FONDOUNICA") && fields["FONDOUNICA"].ToString().ToLower() == "true")
                    {
                        basicMessageFormat = String.Format(Constant.FormatID + Constant.EBT + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "98" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + EBTTransactionOption + "////" + stateTaxAmount + "/" + cityTaxAmount + "//" + reduceStateTaxAmount + "//" + reduceCityTaxAmount + "/////" + "{0}", Strings.ChrW(28));//PRIMEPOS-3099
                        //basicMessageFormat = String.Format(Constant.FormatID + Constant.EBT + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "98" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + EBTTransactionOption + "////" + stateTaxAmount + "/" + cityTaxAmount + "//" + reduceStateTaxAmount + "//////" + "{0}", Strings.ChrW(28));
                    }
                    else
                        basicMessageFormat = String.Format(Constant.FormatID + Constant.EBT + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "98" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + EBTTransactionOption + "////" + "000000000000" + "/" + "000000000000" + "//" + "000000000000" + "//" + "000000000000" + "/////" + "{0}", Strings.ChrW(28));//PRIMEPOS-3099
                }
                //basicMessageFormat = String.Format(Constant.FormatID + Constant.EBT + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "98" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + EBTTransactionOption + "////" + "000000000000" + "/" + "000000000000" + "////////" + "{0}", Strings.ChrW(28));
                else
                {
                    if (fields.ContainsKey("FONDOUNICA") && fields["FONDOUNICA"].ToString().ToLower() == "true")
                    {
                        basicMessageFormat = String.Format(Constant.FormatID + Constant.EBT + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "98" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + "EBT/REFUND" + "////" + stateTaxAmount + "/" + cityTaxAmount + "//" + reduceStateTaxAmount + "//" + reduceCityTaxAmount + "/////" + "{0}", Strings.ChrW(28));//PRIMEPOS-3099
                    }
                    else
                        basicMessageFormat = String.Format(Constant.FormatID + Constant.EBT + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + prosCode + "{0}" + strAmount + "{0}" + cashbackAmount + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "98" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + manualEntry + "{0}" + "{0}" + "EBT/REFUND" + "////" + "000000000000" + "/" + "000000000000" + "//" + "000000000000" + "//" + "000000000000" + "/////" + "{0}", Strings.ChrW(28));//PRIMEPOS-3099
                }
                //Calculate LRC
                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                //Send Message
                string request = SendRequest(Constant.Transaction, basicMessageFormat, LRC);

                logger.Trace("The Request for EBT is " + request);

                string response = string.Empty;
                //response = "377^EBT.333332330001000103198900/APPROVED/0000000700000000000000117553480000000000071153370209$19930.94+$19951.73+EBAPPROVED                                0000010000070454910872025         EBT/CASHPURCH/ATH/A//000000000000/000000000000//000000000000/000000000000//////EBT/NONE///NOSIGN/ /D/S062CONTROL: 33F6U-SX1WKET                                        ^O ";
                //Receive Message
                do
                {
                    device.ReceiveMessage(out response);
                    if (response != string.Empty)
                    {
                        aRespData = response.Split(Strings.ChrW(0x5E));
                        iMsgLength = Strings.Len(aRespData[1]);
                        LRC = CalculateLRC(iMsgLength, aRespData[1]);
                        cResponseLRC = response.ElementAt(response.Length - 1);
                        if (Strings.Chr(LRC) == cResponseLRC)
                        {
                            evertecDevResponse.ParseEvertechResponse(response);
                        }
                        else
                        {
                            logger.Error("INVALID LRC AT EBT EVERTECPROCESSOR");
                            evertecDevResponse.ParseEvertechResponse(response);
                        }
                    }
                } while (evertecDevResponse.isStatusMessage);

                logger.Trace("The Response for EBT is " + response);

                evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

                if (evertecDevResponse.EmvReceipt != null)
                    evertecDevResponse.EmvReceipt.ReferenceNumber = EcrNumber;

                SaveRefNumber(ReferenceNumber);

                device.Disconnect();

                if (evertecDevResponse.ResultDescription == "SUCCESS" && prosCode == "009100")
                {
                    evertecDevResponse.EmvReceipt.IsEvertecForceTransaction = true;
                }
                if (evertecDevResponse.ResultDescription == "SUCCESS" && (prosCode == "090000" || prosCode == "099100"))
                {
                    evertecDevResponse.CashBack = (Convert.ToDecimal(cashbackAmount) / 100).ToString();
                }
                if (evertecDevResponse.ResultDescription == "SUCCESS")
                {
                    evertecDevResponse.EmvReceipt.EvertecTaxBreakdown = (Convert.ToDecimal(cityTaxAmount) / 100).ToString() + "|" + (Convert.ToDecimal(stateTaxAmount) / 100).ToString() + "|" + (Convert.ToDecimal(reduceStateTaxAmount) / 100).ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return evertecDevResponse;
        }

        public PmtTxnResponse IVUCash(Dictionary<String, String> fields)
        {

            String[] aRespData;
            int iMsgLength;
            char cResponseLRC;
            string Amount = string.Empty;

            if (fields["AMOUNT"].Contains("-"))
            {
                Amount = fields["AMOUNT"].Remove(0, 1);
            }
            else
            {
                Amount = fields["AMOUNT"];
            }

            long ReferenceNumber = GetNextRefNumber();
            String ecrNumber = Strings.Format(ReferenceNumber, "000000");

            String strAmount = Strings.Format(Conversion.Val(Amount) * 100, "000000000000");
            string basicMessageFormat = string.Empty;

            basicMessageFormat = Constant.FormatID + Constant.IVUCASH + PmtTxnResponse.SessionID + ecrNumber + Strings.ChrW(28) +
                Strings.ChrW(28) + Strings.ChrW(28) + "000000" + Strings.ChrW(28) + strAmount + Strings.ChrW(28) + Strings.ChrW(28) +
                Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28) +
                Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28) +
                Strings.ChrW(28) + Strings.ChrW(28) + Strings.ChrW(28);

            string citytaxAmount = fields["CITYTAX"];
            string statetaxAmount = fields["STATETAX"];
            string reduceStatetaxAmount = fields["REDUCESTATETAX"];
            string reduceCitytaxAmount = fields["REDUCECITYTAX"];//PRIMEPOS-3099

            if (statetaxAmount.Contains("-"))
            {
                statetaxAmount = statetaxAmount.Remove(0, 1);
            }
            if (citytaxAmount.Contains("-"))
            {
                citytaxAmount = citytaxAmount.Remove(0, 1);
            }
            if (reduceStatetaxAmount.Contains("-"))
            {
                reduceStatetaxAmount = reduceStatetaxAmount.Remove(0, 1);
            }
            if (reduceCitytaxAmount.Contains("-"))//PRIMEPOS-3099
            {
                reduceCitytaxAmount = reduceCitytaxAmount.Remove(0, 1);
            }

            citytaxAmount = Strings.Format(Conversion.Val(citytaxAmount) * 100, "000000000000");
            statetaxAmount = Strings.Format(Conversion.Val(statetaxAmount) * 100, "000000000000");
            reduceStatetaxAmount = Strings.Format(Conversion.Val(reduceStatetaxAmount) * 100, "000000000000");
            reduceCitytaxAmount = Strings.Format(Conversion.Val(reduceCitytaxAmount) * 100, "000000000000");//PRIMEPOS-3099

            //if (Convert.ToDecimal(totaltaxAmount) != 0)
            //{
            //    decimal currentTotalTax = Math.Round((Convert.ToDecimal(Fields["AMOUNT"]) * Convert.ToDecimal(totalTaxAmount)) / Convert.ToDecimal(totalAmount), 2);
            //    stateTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(stateTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
            //    cityTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(cityTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
            //    reduceStateTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(reduceStateTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
            //}

            if (!fields["AMOUNT"].Contains("-"))
            {
                basicMessageFormat += "IVUCASH/CASH////" + statetaxAmount + "/" + citytaxAmount + "//" + reduceStatetaxAmount + "//" + reduceCitytaxAmount + "//////" + Strings.ChrW(28);//PRIMEPOS-3099
                //basicMessageFormat += "IVUCASH/CASH////" + statetaxAmount + "/" + citytaxAmount + "////////" + Strings.ChrW(28);
            }
            else
            {
                basicMessageFormat += "IVUCASH/REFUND////" + statetaxAmount + "/" + citytaxAmount + "//" + reduceStatetaxAmount + "//" + reduceCitytaxAmount + "//////" + Strings.ChrW(28);//PRIMEPOS-3099
                //basicMessageFormat += "IVUCASH/REFUND////" + statetaxAmount + "/" + citytaxAmount + "////////" + Strings.ChrW(28);
            }

            LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

            string request = SendRequest(Constant.Transaction, basicMessageFormat, LRC);

            logger.Trace("The request message" + request);

            evertecDevResponse = new PmtTxnResponse();
            string response = string.Empty;
            do
            {
                device.ReceiveMessage(out response);
                if (response != string.Empty)
                {
                    aRespData = response.Split(Strings.ChrW(0x5E));
                    iMsgLength = Strings.Len(aRespData[1]);
                    LRC = CalculateLRC(iMsgLength, aRespData[1]);
                    cResponseLRC = response.ElementAt(response.Length - 1);
                    if (Strings.Chr(LRC) == cResponseLRC)
                    {
                        evertecDevResponse.ParseEvertechResponse(response);
                    }
                    else
                    {
                        MessageBox.Show("INVALID LRC", "", 0, MessageBoxIcon.Question);
                        evertecDevResponse.ParseEvertechResponse(response);
                    }
                }
            } while (evertecDevResponse.isStatusMessage);

            logger.Trace("The Response for SETTLE is " + response);

            evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

            SaveRefNumber(ReferenceNumber);

            // IncrementRefNumber();
            device.Disconnect();

            return evertecDevResponse;
        }

        public SettleTxnResponse Settle()
        {
            evertecSettleResponse = new SettleTxnResponse();
            try
            {
                //ReadRefNumber();
                //Format specific data before sending out
                long refNum = GetNextRefNumber();
                String EcrNumber = Strings.Format(refNum, "000000");

                //BuildRequest
                string basisMessageFromat = Constant.FormatID + Constant.Settle + PmtTxnResponse.SessionID + EcrNumber + SettleIndicator;

                //Calculate LRC
                LRC = CalculateLRC(basisMessageFromat.Length, basisMessageFromat);

                //Send Request
                string request = SendRequest(Constant.Transaction, basisMessageFromat, LRC);

                logger.Trace("The Request for SETTLE is " + request);

                string response = string.Empty;

                do
                {

                    device.ReceiveMessage(out response);

                    evertecSettleResponse.SetResponse(response);
                } while (string.IsNullOrWhiteSpace(evertecSettleResponse.Message));

                logger.Trace("The Response for SETTLE is " + response);

                //evertecSettleResponse.request = request;//Request for Saving in CC_Transmissionlog

                SaveRefNumber(refNum);

                device.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return evertecSettleResponse;
        }

        public PmtTxnResponse GetSignature()
        {
            evertecDevResponse = new Data.PmtTxnResponse();
            try
            {
                if (isLoggedOn == false)
                {
                    MessageBox.Show("The Device is not connected ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return evertecDevResponse;
                }
                logger.Trace("ENTERED THE GETSIGNATURE METHOD IN EVERTEC");

                String[] aRespData;
                int iMsgLength;
                char cResponseLRC;


                //ReadRefNumber();
                //Format specific data before sending out
                long refNum = GetNextRefNumber();
                String ecrNumber = Strings.Format(refNum, "000000");

                //BuildRequest
                string basicMessageFormat = String.Format(Constant.FormatID + Constant.GetSignature + PmtTxnResponse.SessionID + ecrNumber + "{0}" + "{0}", Strings.ChrW(28));

                //Calculate LRC
                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                string request = SendRequest(Constant.Transaction, basicMessageFormat, LRC);

                logger.Trace("The Request for GETSIGNATURE is " + request);

                device.blExpectingImg = true;
                device.sImgData = "";

                //Receive Response
                string response = string.Empty;
                device.ReceiveMessage(out response);

                if (response != string.Empty)
                {
                    aRespData = response.Split(Strings.ChrW(0x5E));
                    iMsgLength = Strings.Len(aRespData[1]);
                    LRC = CalculateLRC(iMsgLength, aRespData[1]);
                    cResponseLRC = response.ElementAt(response.Length - 1);
                    if (Strings.Chr(LRC) == cResponseLRC)
                    {
                        evertecDevResponse.ParseEvertechResponse(response);
                    }
                    else
                    {
                        logger.Error("INVALID LRC AT GETSIGNATURE EVERTECPROCESSOR");
                        evertecDevResponse.ParseEvertechResponse(response);
                    }
                }

                logger.Trace("The Response for GETSIGNATURE is " + response);

                evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

                if (evertecDevResponse.isTransApproved == true)
                {
                    isSignatureReceived = true;
                    evertecDevResponse.isSignatureSigned = true;
                    evertecDevResponse.ProcessImageSent(device.sImgData);
                }
                else
                {
                    evertecDevResponse.isSignatureSigned = false;
                    isSignatureReceived = false;
                }
                SaveRefNumber(refNum);

                device.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return evertecDevResponse;

        }

        #region PRIMEPOS-3209
        public string CaptureConfirmation(string strMsg, string button1caption, string button2caption)
        {
            string selectedButton = string.Empty;
            evertecDevResponse = new Data.PmtTxnResponse();
            try
            {
                if (isLoggedOn == false)
                {
                    MessageBox.Show("The Device is not connected ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return selectedButton;
                }
                logger.Trace("ENTERED THE CAPTURECONFIRMATION METHOD IN EVERTEC");

                String[] RespData;
                String[] RespCode;
                String selectedResp;


                //ReadRefNumber();
                //Format specific data before sending out
                long refNum = GetNextRefNumber();
                String ecrNumber = Strings.Format(refNum, "000000");

                //Build Request
                string strShowText = String.Format(Constant.FormatID + Constant.CONFDATA2 + PmtTxnResponse.SessionID + ecrNumber + "{0}" + strMsg + "{0}" + button1caption + "/" + button2caption, Strings.ChrW(28));

                //Calculate LRC
                LRC = CalculateLRC(strShowText.Length, strShowText);

                //Send Request
                string request = SendRequest(Constant.Transaction, strShowText, LRC);

                logger.Trace("The Request for CAPTURECONFIRMATION is " + request);

                string response = string.Empty;
                //Receive Message
                do
                {
                    device.ReceiveMessage(out response);
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        evertecDevResponse.ParseEvertechResponse(response);//PRIMEPOS-3442
                        RespData = response.Split(Strings.ChrW(28));
                        RespCode = RespData[1].Split(Convert.ToChar("/"));
                        selectedResp = Convert.ToString(RespCode[1]);
                        if (!string.IsNullOrEmpty(selectedResp))
                        {
                            if (selectedResp.ToUpper() == button1caption.ToUpper())
                            {
                                selectedButton = "1";
                            }
                            else if (selectedResp.ToUpper() == button2caption.ToUpper())
                            {
                                selectedButton = "2";
                            }
                        }
                    }
                } while (evertecDevResponse.isStatusMessage);

                logger.Trace("The Response for CAPTURECONFIRMATION is " + response);

                evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

                SaveRefNumber(refNum);

                device.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "EvertechProcessor==>CaptureConfirmation(): An Exception Occured");
            }

            return selectedButton;
        }
        #endregion

        public PmtTxnResponse CaptureSignature(string message)
        {
            evertecDevResponse = new Data.PmtTxnResponse();
            try
            {
                if (isLoggedOn == false)
                {
                    MessageBox.Show("The Device is not connected ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return evertecDevResponse;
                }
                logger.Trace("ENTERED THE CAPTURE METHOD IN EVERTEC");

                String[] RespData;
                int MsgLength;
                char ResponseLRC;


                //ReadRefNumber();
                //Format specific data before sending out
                long refNum = GetNextRefNumber();
                String ecrNumber = Strings.Format(refNum, "000000");

                //Build Request
                string strSignature = String.Format(Constant.FormatID + Constant.CapSignature + PmtTxnResponse.SessionID + ecrNumber + "{0}" + "{0}" + "{0}" + message + "{0}", Strings.ChrW(28));

                //Calculate LRC
                LRC = CalculateLRC(strSignature.Length, strSignature);

                //Send Request
                string request = SendRequest(Constant.Transaction, strSignature, LRC);

                logger.Trace("The Request for CAPTURESIGNATURE is " + request);

                string response = string.Empty;
                //Receive Message
                do
                {
                    device.ReceiveMessage(out response);
                    if (response != string.Empty)
                    {
                        RespData = response.Split(Strings.ChrW(0x5E));
                        MsgLength = Strings.Len(RespData[1]);
                        LRC = CalculateLRC(MsgLength, RespData[1]);
                        ResponseLRC = response.ElementAt(response.Length - 1);
                        if (Strings.Chr(LRC) == ResponseLRC)
                        {
                            evertecDevResponse.ParseEvertechResponse(response);
                        }
                        else
                        {
                            logger.Error("INVALID LRC AT CAPTURESIGNATURE EVERTECPROCESSOR");
                            evertecDevResponse.ParseEvertechResponse(response);
                        }
                    }
                } while (evertecDevResponse.isStatusMessage);

                logger.Trace("The Response for CAPTURESIGNATURE is " + response);

                evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

                SaveRefNumber(refNum);

                device.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return evertecDevResponse;
        }

        public PmtTxnResponse Void(Dictionary<String, String> fields) //Adjust Delete
        {
            evertecDevResponse = new PmtTxnResponse();
            try
            {
                if (isLoggedOn == false)
                {
                    MessageBox.Show("The Device is not connected ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return evertecDevResponse;
                }
                logger.Trace("ENTERED THE VOID METHOD IN EVERTEC");


                //ReadRefNumber();
                //Format specific data before sending out
                long refNum = GetNextRefNumber();
                String EcrNumber = Strings.Format(refNum, "000000");

                String basicMessageFormat = fields["TRANSACTIONID"] + "\\";

                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);
                String request = SendRequest(Constant.VoidJournal, basicMessageFormat, LRC);

                string response = string.Empty;
                device.ReceiveMessage(out response);

                device.Disconnect();

                String basicAdjRequest = String.Format(evertecDevResponse.AdjustDelete(response), Constant.AdjDelete, EcrNumber);

                //Calculate LRC
                LRC = CalculateLRC(basicAdjRequest.Length, basicAdjRequest);
                String adjRequest = SendRequest(Constant.Transaction, basicAdjRequest, LRC);

                logger.Trace("The Request for VOID is " + adjRequest);

                string adjResponse = string.Empty;
                device.ReceiveMessage(out adjResponse);
                evertecDevResponse.ParseEvertechResponse(adjResponse);

                logger.Trace("The Response for VOID is " + response);

                evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

                device.Disconnect();

                SaveRefNumber(refNum);

                if (evertecDevResponse.Result == "00")
                {
                    evertecDevResponse.Result = "SUCCESS";
                    ReceiptDataForVoid("ADJDELETE", out evertecDevResponse.StateTax, out evertecDevResponse.ReduceTax, out evertecDevResponse.MunicipalTax, out evertecDevResponse.TotalAmount, out evertecDevResponse.BaseAmount);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return evertecDevResponse;
        }

        public PmtTxnResponse Logon(string terminalID, string stationID, string cashierID)
        {
            evertecDevResponse = new PmtTxnResponse();
            try
            {
                logger.Trace("ENTERED THE LOGON METHOD IN EVERTEC");

                String[] RespData;
                int MsgLength;
                int stationNo;
                int cashierNo;
                char ResponseLRC;


                //ReadRefNumber();
                //Format specific data before sending out
                long refNum = GetNextRefNumber();
                String ecrNumber = Strings.Format(refNum, "000000");

                //Parsing StationID
                int.TryParse(stationID, out stationNo);
                stationID = Strings.Format(stationNo, "0000");

                //Parsing CashierID
                int.TryParse(cashierID, out cashierNo);
                cashierID = Strings.Format(cashierNo, "0000");

                if (isLoggedOn)
                    return evertecDevResponse;
                else
                {
                    //Build Request
                    string basicMessageFormat = String.Format(Constant.FormatID + Constant.Login + terminalID + stationID + cashierID + ecrNumber + "{0}" + "{0}", Strings.ChrW(28));

                    //Calculate LRC
                    LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                    string request = SendRequest(Constant.Transaction, basicMessageFormat, LRC);

                    logger.Trace("The Request for LOGON is " + request);

                    //Receive Message
                    string response = string.Empty;
                    do
                    {
                        device.ReceiveMessage(out response);
                        if (response != string.Empty)
                        {
                            RespData = response.Split(Strings.ChrW(0x5E));
                            MsgLength = Strings.Len(RespData[1]);
                            LRC = CalculateLRC(MsgLength, RespData[1]);
                            ResponseLRC = response.ElementAt(response.Length - 1);
                            if (Strings.Chr(LRC) == ResponseLRC)
                            {
                                evertecDevResponse.ParseEvertechResponse(response);
                            }
                            else
                            {
                                MessageBox.Show("INVALID LRC", "", 0, MessageBoxIcon.Question);
                                evertecDevResponse.ParseEvertechResponse(response);
                            }
                        }
                    } while (evertecDevResponse.isStatusMessage);

                    logger.Trace("The Response for LOGON is " + response);

                    evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

                    SaveRefNumber(refNum);

                    isLoggedOn = true;
                    //logon structure and send it to DeviceComm
                    device.Disconnect();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                isLoggedOn = false;
                throw ex;
            }

            return evertecDevResponse;
        }

        public PmtTxnResponse Logoff()
        {
            evertecDevResponse = new PmtTxnResponse();
            try
            {
                if (isLoggedOn)
                {
                    //ReadRefNumber();
                    //Format specific data before sending out
                    long ReferenceNumber = GetNextRefNumber();
                    String EcrNumber = Strings.Format(ReferenceNumber, "000000");

                    string basicMessageFormat = Constant.FormatID + Constant.Logoff + PmtTxnResponse.SessionID + EcrNumber + Strings.ChrW(28) + Strings.ChrW(28);
                    LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                    //Send Message
                    string request = SendRequest(Constant.Transaction, basicMessageFormat, LRC);

                    logger.Trace("The Request for LOGOFF is " + request);

                    evertecDevResponse = new PmtTxnResponse();

                    string response = string.Empty;
                    do
                    {
                        device.ReceiveMessage(out response);
                        evertecDevResponse.ParseEvertechResponse(response);
                    } while (evertecDevResponse.isStatusMessage);

                    logger.Trace("The response for LOGOFF is " + response);

                    SaveRefNumber(ReferenceNumber);

                    isLoggedOn = false;

                    evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

                    device.Disconnect();

                    return evertecDevResponse;
                    //logOff structure and send it to DeviceComm
                }
                else
                {
                    //do nothing
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return evertecDevResponse;
        }

        private int CalculateLRC(int length, string strpout)
        {
            int PrevChar;
            int initialNumber;
            int lrcCalculate = 0;
            for (initialNumber = 1; initialNumber <= length; initialNumber++)                         // Calculate LRC
            {
                var midstr = Strings.Mid(strpout, initialNumber, 1);
                byte[] charByte = Encoding.ASCII.GetBytes(midstr.ToString());
                PrevChar = charByte[0];
                lrcCalculate = (lrcCalculate ^ PrevChar);
            }
            return lrcCalculate;
        }

        //private long GetNextRefNumber()
        //{
        //    try
        //    {
        //        FileStream fs = new FileStream("config.txt", FileMode.Open, FileAccess.Read);
        //        StreamReader d = new StreamReader(fs);
        //        d.BaseStream.Seek(0, SeekOrigin.Begin);
        //        while (d.Peek() > -1)
        //        {
        //            RefNumber = Convert.ToInt32(d.ReadLine());
        //        }
        //        //Increment Refnumber
        //        RefNumber = RefNumber + 1;
        //        if (RefNumber > 999999)
        //        {
        //            RefNumber = 1;
        //        }
        //        d.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Trace("ENTERED IN GET NEXT REFERENCE NUMBER");
        //        throw ex;
        //    }
        //    return RefNumber;
        //}

        //private void SaveRefNumber(long ReferenceNumber)
        //{
        //    try
        //    {
        //        FileStream fs = new FileStream("config.txt", FileMode.Create, FileAccess.Write);
        //        StreamWriter s = new StreamWriter(fs);

        //        s.BaseStream.Seek(0, SeekOrigin.End);
        //        s.WriteLine(ReferenceNumber);
        //        s.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Trace("ENTERED IN SAVE REFERENCE NUMBER");
        //        throw ex;
        //    }
        //}

        private string SendRequest(String TransactionName, String BasicRequest, int LRC)
        {
            string request = string.Empty;
            bool isConnected = false;
            try
            {
                request = TransactionName + BasicRequest + Strings.ChrW(LRC);
                int len = request.Length;
                isConnected = device.Connect();
                //if (isConnected)
                {
                    device.SendMessage(Encoding.ASCII.GetBytes(request));
                }
            }
            catch (SocketException ex)
            {
                logger.Error(ex.ToString());
                //check connectivity exception
                //if (isConnected)
                //{
                //    device.Disconnect();
                //}
                //if (retry < MAX_RETRIES)
                //{
                //    retry++;
                //    return SendRequest(TransactionName, BasicRequest, LRC, retry);
                //}
                //else
                //{
                //    if (MessageBox.Show("The Connection failed do you want to Retry??", "Confirm", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) == DialogResult.Retry)
                //    {
                //        retry = 0;
                //        SendRequest(TransactionName, BasicRequest, LRC, retry);
                //    }
                //    else
                //    {
                //        return request;
                //    }
                //    //Yes
                //    //else
                //}
                throw ex;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }

            return request;
        }

        private long GetNextRefNumber()
        {
            TransactionReference tr = new TransactionReference();
            try
            {
                var db = new Possql();
                tr = db.TransactionReferences.Where(w => w.Processor == "EVERTEC").SingleOrDefault();

                tr.LastTransaction = (Convert.ToInt32(tr.LastTransaction) + 1).ToString();
                if (Convert.ToInt32(tr.LastTransaction) > 999999)
                {
                    tr.LastTransaction = "1";
                    db.TransactionReferences.Attach(tr);
                    db.Entry(tr).Property(p => p.LastTransaction).IsModified = true;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return Convert.ToInt64(tr.LastTransaction);
        }

        private void SaveRefNumber(long ReferenceNumber)
        {
            var db = new Possql();
            TransactionReference tr = new TransactionReference();
            tr = db.TransactionReferences.Where(w => w.Processor == "EVERTEC").SingleOrDefault();
            tr.LastTransaction = ReferenceNumber.ToString();
            db.TransactionReferences.Attach(tr);
            db.Entry(tr).Property(p => p.LastTransaction).IsModified = true;
            db.SaveChanges();
        }

        public string ReceiptData(string transType)
        {
            string controlNumber = string.Empty;
            try
            {
                string basicMessageFormat = transType + "\\1\\1\\1\\";

                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                string request = SendRequest("RECEIPTDATA\\", basicMessageFormat, LRC);

                logger.Trace("The Request for LOGON is " + request);

                //Receive Message
                string response = string.Empty;

                device.ReceiveMessage(out response);
                //response = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF - 8'/><title> MERCHANT RECEIPT</title><script type='text / javascript'></script> </head><body><div id='caption'><img src='ATHLogo.png' alt='Caption'> </div><div id='StoreInfo'>MICRO MERCH SEMI-INTEGRADO<br>CUPEY CENTER<br>SAN JUAN<br></div><div id='line1'><div id='Labels1'><div class='label'>DATE</div><div class='centered'>TIME</div><div class='content'>HOST</div></div></div><div id='lineData'><div id='LDATA'><div class='label'>Jan 28,21</div><div class='centered'>120640</div><div class='content'>ATH</div></div></div><div id='line3'><div id='Labels3'><div class='label'>BATCH</div><div class='centered'>TERMINAL ID</div><div class='content'>MERCHANT ID</div></div></div><div id='lineData2'><div id='LDATA2'><div class='label'>000016</div><div class='centered'>33333233</div><div class='content'>454910872025         </div></div></div><div id='TrxType2'><div id='TRXName2'><div class='centered'>IVUCASH &nbsp CASH &nbsp </div></div></div><div id='Account'><div id='ACC'><div class='centered'>ACCT.</div></div></div><div id='lineData3'><div id='LDATA3'><div class='label'>IVUCASH</div><div class='centered'>9805</div></div></div><div id='AutCode'><div id='ACODE'><div class='label'>AUTH. CODE: 957922</div><div class='content'>INVOICE: 000651</div></div></div><div id='trace'><div id='TRACE'><div class='label'>TRACE: 000627</div><div class='content'>REFERENCE: 031809</div></div></div><div id='totals'><div id='AMOUNT'><div class='label'>AMOUNT:</div><div class='content'>$   42.40</div></div><br><div id='STATE TAX'><div class='label'>STATE TAX:</div><div class='content'>$   4.20</div></div><br><div id='MUN. TAX'><div class='label'>CITY TAX:</div><div class='content'>$   0.40</div></div><br><div id='LineT' class='content'>-------------</div><br><div id='TOTAL'><div class='label'>TOTAL:</div><div class='content'>$   47.00</div></div><br><br><div style='font - size:14px' id='IVULOTTO'>CONTROL: 2M2EZ-3BJBSET                  </div><br><br> <div id='RSPMSG'>APPROVED<br> </div><br><br><div id='SignLine'> NO SIGNATURE REQUIRED </div></center></div><div id='custom'></div><br><div id = 'Disclosure' style='font - size:11px 'class='content'>CARDHOLDER ACKNOWLEDGES RECEIPT OF GOODS AND/OR SERVICE IN THE AMOUNT OF THE TOTAL SHOWN HEREON AND AGREES TO PERFORM THE OBLIGATIONS SET FORTH IN THE CARDHOLDER'S AGRREMENT WITH THE ISSUER</div><br><br><div id='footer'><br><br>** CUSTOMER COPY **<br></div></html>    ";
                if (response != string.Empty)
                {
                    string pattern = @"<div id=""IVULOTTO"">(.*?)</div";
                    logger.Trace("response is :" + response);
                    MatchCollection collection = Regex.Matches(response, pattern);
                    foreach (Match match in collection)
                    {
                        controlNumber += Convert.ToString(match.Groups[1]);
                    }
                    logger.Trace("ControlNumber : " + controlNumber);
                }

                device.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            //RECEIPTDATA
            return controlNumber;
        }
        public void ReceiptDataForVoid(string transType, out string StateTax, out string ReduceTax, out string cityTax, out string TotalTax, out string BaseAmount)
        {
            string controlNumber = string.Empty;
            StateTax = string.Empty;
            ReduceTax = string.Empty;
            cityTax = string.Empty;
            TotalTax = string.Empty;
            BaseAmount = string.Empty;
            try
            {
                string basicMessageFormat = transType + "\\1\\1\\1\\";

                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                string request = SendRequest("RECEIPTDATA\\", basicMessageFormat, LRC);

                logger.Trace("The Request for LOGON is " + request);

                //Receive Message
                string response = string.Empty;

                device.ReceiveMessage(out response);
                //response = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF - 8'/><title> MERCHANT RECEIPT</title><script type='text / javascript'></script> </head><body><div id='caption'><img src='ATHLogo.png' alt='Caption'> </div><div id='StoreInfo'>MICRO MERCH SEMI-INTEGRADO<br>CUPEY CENTER<br>SAN JUAN<br></div><div id='line1'><div id='Labels1'><div class='label'>DATE</div><div class='centered'>TIME</div><div class='content'>HOST</div></div></div><div id='lineData'><div id='LDATA'><div class='label'>Jan 28,21</div><div class='centered'>120640</div><div class='content'>ATH</div></div></div><div id='line3'><div id='Labels3'><div class='label'>BATCH</div><div class='centered'>TERMINAL ID</div><div class='content'>MERCHANT ID</div></div></div><div id='lineData2'><div id='LDATA2'><div class='label'>000016</div><div class='centered'>33333233</div><div class='content'>454910872025         </div></div></div><div id='TrxType2'><div id='TRXName2'><div class='centered'>IVUCASH &nbsp CASH &nbsp </div></div></div><div id='Account'><div id='ACC'><div class='centered'>ACCT.</div></div></div><div id='lineData3'><div id='LDATA3'><div class='label'>IVUCASH</div><div class='centered'>9805</div></div></div><div id='AutCode'><div id='ACODE'><div class='label'>AUTH. CODE: 957922</div><div class='content'>INVOICE: 000651</div></div></div><div id='trace'><div id='TRACE'><div class='label'>TRACE: 000627</div><div class='content'>REFERENCE: 031809</div></div></div><div id='totals'><div id='AMOUNT'><div class='label'>AMOUNT:</div><div class='content'>$   42.40</div></div><br><div id='STATE TAX'><div class='label'>STATE TAX:</div><div class='content'>$   4.20</div></div><br><div id='MUN. TAX'><div class='label'>CITY TAX:</div><div class='content'>$   0.40</div></div><br><div id='LineT' class='content'>-------------</div><br><div id='TOTAL'><div class='label'>TOTAL:</div><div class='content'>$   47.00</div></div><br><br><div style='font - size:14px' id='IVULOTTO'>CONTROL: 2M2EZ-3BJBSET                  </div><br><br> <div id='RSPMSG'>APPROVED<br> </div><br><br><div id='SignLine'> NO SIGNATURE REQUIRED </div></center></div><div id='custom'></div><br><div id = 'Disclosure' style='font - size:11px 'class='content'>CARDHOLDER ACKNOWLEDGES RECEIPT OF GOODS AND/OR SERVICE IN THE AMOUNT OF THE TOTAL SHOWN HEREON AND AGREES TO PERFORM THE OBLIGATIONS SET FORTH IN THE CARDHOLDER'S AGRREMENT WITH THE ISSUER</div><br><br><div id='footer'><br><br>** CUSTOMER COPY **<br></div></html>    ";
                if (response != string.Empty)
                {
                    logger.Trace("response is :" + response);
                    HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(response);

                    HtmlNode State = html.GetElementbyId("STATE_TAX");
                    HtmlNode Municipal = html.GetElementbyId("CITY_TAX");
                    HtmlNode Reduce = html.GetElementbyId("STATE_TAX2");
                    HtmlNode Total = html.GetElementbyId("TOTAL");
                    HtmlNode BaseTotal = html.GetElementbyId("AMOUNT");

                    StateTax = State.InnerText;
                    ReduceTax = Reduce.InnerText;
                    cityTax = Municipal.InnerText;
                    TotalTax = Total.InnerText;
                    BaseAmount = BaseTotal.InnerText;
                    //string pattern = @"<div id=""STATE_TAX"">(.*?)</div";
                    //MatchCollection collection = Regex.Matches(response, pattern);
                    //foreach (Match match in collection)
                    //{
                    //    controlNumber += Convert.ToString(match.Groups[1]);
                    //}
                    logger.Trace("ControlNumber : " + controlNumber);
                }

                device.Disconnect();

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            //RECEIPTDATA            
        }
        public PmtTxnResponse GetEBTBalance()
        {
            evertecDevResponse = new PmtTxnResponse();
            try
            {
                if (isLoggedOn == false)
                {
                    MessageBox.Show("The Device is not connected ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return evertecDevResponse;
                }
                logger.Trace("ENTERED THE GetEBTBalance METHOD IN EVERTEC");

                String[] aRespData;
                int iMsgLength;
                char cResponseLRC;
                //string DuplicateTxn = (fields["ALLOWDUP"] == "False") ? "0" : "1";
                //string manualEntry = (fields["MANUAL_FLAG"] == "0") ? "0" : "1";


                //ReadRefNumber();
                //Format specific data before sending out
                long ReferenceNumber = GetNextRefNumber();
                String EcrNumber = Strings.Format(ReferenceNumber, "000000");

                //if (fields["AMOUNT"].Contains("-"))
                //{
                //    fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                //}

                //String strAmount = Strings.Format(Conversion.Val(fields["AMOUNT"]) * 100, "000000000000");

                string basicMessageFormat = String.Empty;
                //BuildRequest

                basicMessageFormat = String.Format(Constant.FormatID + Constant.EBT + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + "310000" + "{0}" + "000000000000" + "{0}" + "000000000000" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "00" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "0" + "{0}" + "{0}" + "EBT/BALANCE" + "/////////////" + "{0}", Strings.ChrW(28));
                //else
                //basicMessageFormat = String.Format(Constant.FormatID + Constant.EBT + PmtTxnResponse.SessionID + EcrNumber + "{0}" + "{0}" + "{0}" + "310000" + "{0}" + strAmount + "{0}" + "000000000000" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "98" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "{0}" + "0" + "{0}" + "{0}" + "EBT/REFUND" + "/////////////" + "{0}", Strings.ChrW(28));
                //Calculate LRC
                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                //Send Message
                string request = SendRequest(Constant.Transaction, basicMessageFormat, LRC);

                logger.Trace("The Request for GetEBTBalance is " + request);

                string response = string.Empty;
                //Receive Message
                do
                {
                    device.ReceiveMessage(out response);
                    if (response != string.Empty)
                    {
                        aRespData = response.Split(Strings.ChrW(0x5E));
                        iMsgLength = Strings.Len(aRespData[1]);
                        LRC = CalculateLRC(iMsgLength, aRespData[1]);
                        cResponseLRC = response.ElementAt(response.Length - 1);
                        if (Strings.Chr(LRC) == cResponseLRC)
                        {
                            evertecDevResponse.ParseEvertechResponse(response);
                        }
                        else
                        {
                            logger.Error("INVALID LRC AT EBT EVERTECPROCESSOR");
                            evertecDevResponse.ParseEvertechResponse(response);
                        }
                    }
                } while (evertecDevResponse.isStatusMessage);

                logger.Trace("The Response for GetEBTBalance is " + response);

                evertecDevResponse.request = request;//Request for Saving in CC_Transmissionlog

                SaveRefNumber(ReferenceNumber);

                device.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return evertecDevResponse;
        }
        private void CalculateTaxAmount(DataTable dt, Dictionary<string, string> Fields, string totalAmount, string totalTaxAmount, string reduceStateTaxPercent, ref string cityTaxAmount, ref string stateTaxAmount, ref string reduceStateTaxAmount, ref string reduceCityTaxAmount)
        {
            try
            {
                if (Convert.ToDecimal(totalTaxAmount) != 0)
                {
                    decimal currentTotalTax = Math.Round((Convert.ToDecimal(Fields["AMOUNT"]) * Convert.ToDecimal(totalTaxAmount)) / Convert.ToDecimal(totalAmount), 2);
                    stateTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(stateTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
                    cityTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(cityTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
                    reduceStateTaxAmount = Math.Round((currentTotalTax * Convert.ToDecimal(reduceStateTaxAmount)) / Convert.ToDecimal(totalTaxAmount), 2).ToString();
                    if (Convert.ToDecimal(reduceStateTaxPercent) == 0)
                    {
                        reduceCityTaxAmount = "0.00";
                    }
                    else
                    {
                        reduceCityTaxAmount = Math.Round((Convert.ToDecimal(reduceStateTaxAmount) * 100) / Convert.ToDecimal(reduceStateTaxPercent)).ToString();//PRIMEPOS-3099
                    }
                    if (stateTaxAmount.Contains("-"))
                    {
                        stateTaxAmount = stateTaxAmount.Remove(0, 1);
                    }
                    if (cityTaxAmount.Contains("-"))
                    {
                        cityTaxAmount = cityTaxAmount.Remove(0, 1);
                    }
                    if (reduceStateTaxAmount.Contains("-"))
                    {
                        reduceStateTaxAmount = reduceStateTaxAmount.Remove(0, 1);
                    }
                    if (reduceCityTaxAmount.Contains("-"))//PRIMEPOS-3099
                    {
                        reduceCityTaxAmount = reduceCityTaxAmount.Remove(0, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
        public string EmvTag()//PRIMEPOS-3000
        {
            string emvTag = string.Empty;
            try
            {
                string basicMessageFormat = "SALE" + "\\1\\1\\1\\";

                LRC = CalculateLRC(basicMessageFormat.Length, basicMessageFormat);

                string request = SendRequest("RECEIPTDATA\\", basicMessageFormat, LRC);

                logger.Trace("The Request for LOGON is " + request);

                //Receive Message
                string response = string.Empty;

                device.ReceiveMessage(out response);
                logger.Trace("EMV TAG RESPONSE IS : " + response);
                if (response != string.Empty)
                {
                    //IWebElement table = browser.FindElement(By.Id("column2"));

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(response);
                    emvTag = doc.GetElementbyId("EMV1")?.InnerHtml;
                    if (string.IsNullOrWhiteSpace(emvTag))
                        emvTag = string.Empty;
                }

                device.Disconnect();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            //RECEIPTDATA
            return emvTag;
        }
        public enum TaxTypes
        {
            State = 1,
            Municipality,
            Federal,
            City,
            Local,
            County
        }
    }
}
