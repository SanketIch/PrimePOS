using Evertech.Common;
using Evertech.Implementation;
using Microsoft.VisualBasic;
using MMS.PROCESSOR;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Evertech.Data
{
    public class PmtTxnResponse : PaymentResponse
    {
        #region VARIABLES
        public string Duplicate;
        string ImageData;
        string PaymentHost;
        public String request = String.Empty;
        public string response = string.Empty;
        public string SignatureData = string.Empty;
        public string ResultSignature = string.Empty;
        string[] Responsetxn;
        string AmountLimitWarning;
        string ResponseCode = string.Empty;
        string AuthorizationCode = string.Empty;
        string ProcessingCode = string.Empty;
        string TransactionAmount = string.Empty;
        string AdditionalAmount = string.Empty;
        string PanCardNumber = string.Empty;
        string RetrievalReferenceNumber = string.Empty;
        string TransactionTime = string.Empty;
        string TransactionDate = string.Empty;
        string SpecialAmount = string.Empty;
        string LicenseNumber = string.Empty;
        string ManualEntry = string.Empty;
        string ExpirationDate = string.Empty;
        private bool AmountLimit = false;
        private bool isEMVCardDenied = false;
        private bool isError = false;
        public bool isSignatureSigned = false;
        string[] ResponseData;
        public bool isTransApproved = false;
        public static string SessionID = string.Empty;
        public string TransType = String.Empty;
        string ResponseResult;
        string[] TxnNameRequest;
        string[] RespCode;
        string ResponseType;
        public string[] ExtendedField;
        private bool isInsufFunds = false;
        public bool isStatusMessage = false;
        public bool isSignature = false;
        DeviceComm device = new DeviceComm();

        #endregion

        #region SettleMent Report

        public string InvoiceNumber = String.Empty;
        public string AuthCode = String.Empty;
        public string BatchNumber = String.Empty;
        public string LastFourDigitsOfAcct = string.Empty;
        public string MerchantID = string.Empty;
        public string TraceNumber = string.Empty;

        string Card = "&nbsp";
        string AdjustData = "&nbsp";
        string TransTypeDetails = "&nbsp";
        string CityTaxDetails = "&nbsp";
        string StateTaxDetails = "&nbsp";
        string TipOrCashback = "&nbsp";
        public string foodBalance = string.Empty;
        public string cashBalance = string.Empty;

        #endregion

        ILogger logger = LogManager.GetCurrentClassLogger();

        public PmtTxnResponse()
        {

        }

        public override int ParseResponse(string xmlResponse, string FilterNode)
        {
            throw new NotImplementedException();
        }

        public override void ParseEvertechResponse(string DeviceResponse)
        {
            string ResponseCode = String.Empty;
            string ResponseCodeSign = String.Empty;
            string[] ResponseStrSign = null;
            string[] ExtendedFieldCodes;
            string[] ExtendAID;
            string AID;
            string[] ExtendedAC;
            string AC;
            string[] ExtendedUN;
            string UN;
            string[] ExtendedTVR;
            string TVR;
            string[] ExtendedTSI;
            string TSI;
            string ResponseCodeNative = string.Empty;
            String Amount;

            try
            {
                if (DeviceResponse != string.Empty)
                {
                    response = DeviceResponse.ToString();
                    ResponseData = DeviceResponse.Split(Strings.Chr(0x5E));
                    base.EmvReceipt = new EmvReceiptTags();
                    if (DeviceResponse.Contains(Strings.Chr(28)))
                    {
                        if (ResponseData.Length == 13)
                        {
                            Responsetxn = ResponseData[11].Split(Strings.Chr(28));
                            Amount = Responsetxn[4];
                            String Amt = Amount.Substring(0, 10);
                            base.AmountApproved = Conversion.Val(Amt).ToString();
                        }
                        else if (ResponseData.Length == 11)
                        {
                            Responsetxn = ResponseData[9].Split(Strings.Chr(28));
                        }
                        else
                        {
                            Responsetxn = DeviceResponse.Split(Strings.Chr(28));

                            if (Responsetxn.Length >= 5)
                            {
                                Amount = Responsetxn[4];
                                double Num;
                                bool isNum = double.TryParse(Amount, out Num);
                                if (isNum)
                                {
                                    if (!String.IsNullOrWhiteSpace(Amount))
                                    {
                                        String Amt = Amount.Substring(0, 12);
                                        Amt = String.Format("{0:N2}", Int32.Parse(Amt) / 100.0);
                                        base.AmountApproved = Convert.ToDouble(Amt).ToString();
                                    }
                                }
                                if (Responsetxn.Length >= 20)
                                {
                                    if (!string.IsNullOrWhiteSpace(Responsetxn[19]))
                                    {
                                        if (Responsetxn[19].Trim() == "1")
                                            base.EntryMethod = "MANUAL";
                                    }
                                }
                                if (Responsetxn.Length >= 23)
                                {
                                    if (!string.IsNullOrWhiteSpace(Responsetxn[22]))
                                    {
                                        if (Responsetxn[22].ToUpper().Contains("CONTROL"))
                                        {
                                            logger.Trace("CONTROLNUMBER :" + Responsetxn[22]);
                                            string toBeSearched = "CONTROL:";
                                            int index = Responsetxn[22].IndexOf(toBeSearched);

                                            if (index != -1)
                                            {
                                                string code = Responsetxn[22].Substring(index + toBeSearched.Length);
                                                logger.Trace("Split String of control number :" + code);
                                                if (!string.IsNullOrWhiteSpace(code))
                                                {
                                                    code = "CONTROL:" + code.Trim();
                                                    base.EmvReceipt.ControlNumber = code;
                                                }
                                                // do something here
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (Responsetxn[0].Contains("."))
                        {
                            base.EmvReceipt.TerminalID = Responsetxn[0].Split('.')[1].Substring(0, 7);
                            base.EmvReceipt.ReferenceNumber = Responsetxn[0].Split('.')[1].Substring(Responsetxn[0].Split('.')[1].Length - 6);
                        }
                    }
                    if (Responsetxn != null)
                    {
                        if (Responsetxn.Length >= 2)
                        {
                            if (Responsetxn.Length >= 3 && Responsetxn[2].Contains("APPROVED"))
                            {
                                RespCode = Responsetxn[2].Split(Convert.ToChar("/"));
                            }
                            else
                            {
                                if (Responsetxn[1].Contains("/"))
                                    RespCode = Responsetxn[1].Split(Convert.ToChar("/"));
                                if (Responsetxn.Length >= 3)
                                {
                                    if (Responsetxn[2].Contains("/"))
                                    {
                                        ResponseStrSign = Responsetxn[2].Split('/');
                                        if (ResponseStrSign != null && ResponseStrSign.Length > 1 && ResponseStrSign[1] == "REQUEST FORMAT ERROR")
                                        {
                                            base.Result = ResponseStrSign[1];
                                            base.ResultDescription = ResponseStrSign[1];
                                            return;
                                        }
                                    }

                                }
                            }
                        }

                        if (ResponseStrSign != null && ResponseStrSign[1].Contains("APPROVED"))
                        {
                            ResponseCodeSign = ResponseStrSign[1];
                        }
                        if (RespCode != null)
                        {
                            if (RespCode.Length >= 1)
                                ResponseCodeNative = RespCode[0];
                            if (RespCode.Length >= 2)
                            {
                                ResponseResult = RespCode[1];
                                base.Result = (ResponseResult == "APPROVED") ? "SUCCESS" : ResponseResult;
                                base.ResultDescription = (ResponseResult == "APPROVED") ? "SUCCESS" : ResponseResult;
                            }

                            if (RespCode.Length >= 3)
                            {
                                ResponseCode = RespCode[2];
                                if (RespCode[2].Contains('^'))
                                {
                                    ResponseCode = RespCode[2].Split('^')[0];
                                }
                            }

                            if (Responsetxn.Length > 0)
                            {
                                if (Responsetxn[0].Contains("^"))
                                    TxnNameRequest = Responsetxn[0].Split(Convert.ToChar("^"));
                            }
                            if (TxnNameRequest.Length > 1)
                            {
                                if (TxnNameRequest[1].Contains("."))
                                    TxnNameRequest = TxnNameRequest[1].Split(Convert.ToChar("."));

                                TransType = TxnNameRequest[0];

                                if (TxnNameRequest[0] != "LOGON")
                                {
                                    String TransID = TxnNameRequest[1].Substring(16, 6);
                                    base.EmvReceipt.TransID = TransID;
                                    base.ticketNum = TransID;
                                    base.TransactionNo = TransID;
                                }

                            }
                        }
                        if (TxnNameRequest != null)
                        {
                            if (TxnNameRequest.Length > 0)
                                ResponseType = TxnNameRequest[0];
                        }

                        if (TransType == "SALE" | TransType == "EBT" | TransType == "CASH" | TransType == "REFUND" | TransType == "ADJDELETE" | TransType == "IVUCASH VOID" | TransType == "CASH REFUND" | TransType == "TIPADJUST" | TransType == "CASH REFUND" | TransType == "IVUCASH")
                        {
                            if (Responsetxn.Length >= 3)
                            {
                                AuthCode = Responsetxn[2];
                                base.AuthNo = AuthCode;
                            }
                            if (Responsetxn.Length >= 7)
                            {
                                LastFourDigitsOfAcct = Responsetxn[6];
                                if (LastFourDigitsOfAcct.Length == 4)
                                {
                                    //base.AccountNo = sLastFourDigitsOfAcct;
                                    base.MaskedCardNo = LastFourDigitsOfAcct;
                                }
                            }
                            if (Responsetxn.Length >= 8)
                            {
                                InvoiceNumber = Responsetxn[7];

                                if (!string.IsNullOrEmpty(InvoiceNumber))
                                {
                                    if (InvoiceNumber.Length >= 12)
                                    {
                                        InvoiceNumber = InvoiceNumber.Substring(6, 6);
                                        base.EmvReceipt.InvoiceNumber = InvoiceNumber;
                                    }
                                }
                            }
                            if (Responsetxn.Length >= 17)
                                BatchNumber = Responsetxn[16];
                            base.EmvReceipt.BatchNumber = BatchNumber;
                            if (Responsetxn.Length >= 18)
                                TraceNumber = Responsetxn[17];
                            base.EmvReceipt.TraceNumber = TraceNumber;
                            if (Responsetxn.Length >= 21)
                                MerchantID = Responsetxn[20];
                            base.EmvReceipt.MerchantID = MerchantID;
                            if (Responsetxn.Length >= 22)
                            {
                                if (Responsetxn[21].Contains("/"))
                                    ExtendedField = Responsetxn[21].Split('/');
                                if (ExtendedField != null)
                                {
                                    if (ExtendedField.Length >= 2)
                                        TransTypeDetails = ExtendedField[1];
                                    if (ExtendedField.Length >= 4)
                                        PaymentHost = ExtendedField[2];
                                    if (ExtendedField.Length >= 6)
                                        StateTaxDetails = Strings.Format(Conversion.Val(ExtendedField[5]) / (double)100, "c2");
                                    if (ExtendedField.Length >= 7)
                                        CityTaxDetails = Strings.Format(Conversion.Val(ExtendedField[6]) / (double)100, "c2");
                                    if (ExtendedField.Length >= 8)
                                        AdjustData = ExtendedField[7];
                                    if (ExtendedField.Length >= 16)
                                    {
                                        Card = ExtendedField[15];
                                        base.CardType = Card;
                                    }
                                    if (ExtendedField.Length >= 21)
                                    {
                                        TipOrCashback = ExtendedField[20];
                                        base.CashBack = TipOrCashback;
                                    }
                                    if (ExtendedField.Length >= 11)
                                    {
                                        if (!string.IsNullOrWhiteSpace(ExtendedField[10]))
                                        {
                                            base.EmvReceipt.ATHMovil = ExtendedField[10];
                                        }
                                    }
                                    if (ExtendedField[19] == "SIGN")
                                    {
                                        isSignature = true;
                                    }
                                    else
                                    {
                                        isSignature = false;
                                    }
                                }
                                if (Responsetxn.Length >= 24 && (ResponseResult == "APPROVED" || ResponseResult == "DENIED BY CARD SECOND GEN"))
                                {
                                    ExtendedFieldCodes = Responsetxn[23].Split('/');
                                    if (ExtendedFieldCodes.Length >= 2)
                                    {
                                        ExtendAID = ExtendedFieldCodes[1].Split(':');
                                        AID = ExtendAID[1];
                                        base.EmvReceipt.AppIndentifer = "AID:" + AID;
                                        ExtendedAC = ExtendedFieldCodes[2].Split(':');
                                        AC = ExtendedAC[1];
                                        base.EmvReceipt.AppIndentifer += "  AC:" + AC;
                                        ExtendedUN = ExtendedFieldCodes[3].Split(':');
                                        UN = ExtendedUN[1];
                                        base.EmvReceipt.AppIndentifer += "\n  UN:" + UN;
                                        ExtendedTVR = ExtendedFieldCodes[4].Split(':');
                                        TVR = ExtendedTVR[1];
                                        base.EmvReceipt.AppIndentifer += "  TVR:" + TVR;
                                        ExtendedTSI = ExtendedFieldCodes[5].Split(':');
                                        TSI = ExtendedTSI[1];
                                        base.EmvReceipt.AppIndentifer += "  TSI:" + TSI;
                                    }
                                }
                            }
                            RespCode = null;
                        }
                    }
                    if (Responsetxn.Length >= 3)
                    {
                        if (Responsetxn[2].Contains("/"))
                        {
                            if (Responsetxn.Length >= 22)
                            {
                                String[] SignatureArray = Responsetxn[21].Split('/');
                                String Signature = SignatureArray[19];
                                if (Signature == "SIGN")
                                {
                                    base.SignatureString = device.sImgData;
                                }
                            }

                        }
                        if (Responsetxn.Length >= 22)
                        {
                            foodBalance = Responsetxn[10];
                            cashBalance = Responsetxn[11];
                            if (Responsetxn.Length > 12)
                            {
                                base.EmvReceipt.ResponseCode = Responsetxn[13];
                            }
                            if (base.EmvReceipt != null)
                                base.EmvReceipt.EbtBalance = foodBalance + "|" + cashBalance;
                        }
                    }

                    if (TransType == "LOGON")
                    {
                        if (Responsetxn[0].Length >= 23)
                        {
                            SessionID = Responsetxn[0].Substring(21, 23);
                        }
                        else
                        {
                            logger.Trace("Lenght MisMatch");
                        }
                    }
                    isStatusMessage = false;
                    isInsufFunds = false;

                    try
                    {
                        ResponseCode = ResponseCode.Trim(Convert.ToChar("^"));
                        ResponseCode = ResponseCode.Split('^')[0];
                    }
                    catch (Exception ex)
                    {
                        logger.Trace(ex.Message + " " + ex.ToString());
                    }
                    if ((ResponseType == "SIGNREQ") && (ResponseResult == "APPROVED" || ResponseCodeSign == "APPROVED"))
                    {
                        ResultSignature = "OK";
                    }
                    switch (ResponseCode)
                    {
                        case "00":
                            isTransApproved = true;
                            if (ResponseCodeNative == "51")
                            {
                                if (TransType == "EBT")
                                    isInsufFunds = true;
                            }
                            isStatusMessage = false;
                            break;
                        case "ZX": //Transactional declined response.
                            isTransApproved = false;
                            if (ResponseCodeNative == "51")
                            {
                                if (TransType == "EBT")
                                    isInsufFunds = true;
                            }
                            isStatusMessage = false;
                            break;
                        case "ZY": //Operational exception response such as TC - Transaction cancelled , TO - TimeOut plus others of such nature.
                            isTransApproved = false;
                            isStatusMessage = false;
                            if (ResponseCodeNative == "AD")
                            {
                            }
                            else if (ResponseCodeNative == "DP")
                            {
                                Duplicate = ResponseResult;
                                base.Result = "DUP TRANSACTION";
                                base.ResultDescription = "DUP TRANSACTION";
                            }
                            else if (ResponseCodeNative == "AL")
                            {
                                AmountLimit = true;
                                AmountLimitWarning = ResponseResult;
                            }
                            else if (ResponseCodeNative == "FN")
                                isSignatureSigned = false;
                            else if (ResponseCodeNative == "TC")
                                logger.Trace("ProcessTransactionResponse");
                            else
                                isError = true;
                            break;
                        case "ZW":
                            isStatusMessage = false;
                            isEMVCardDenied = true;   // E1 fails first certificate E2 fails second certificate
                            if (ResponseCodeNative == "51")
                            {
                                if (TransType == "EBT")
                                    isInsufFunds = true;
                            }
                            break;
                        case "ST":                  //'Only status messages have "ST" as response code So we se bIsStatusMessage to true so we can continue listening for responsessMessage 
                            isStatusMessage = true;
                            break;
                    }
                }
                else
                {
                    isStatusMessage = false;
                }
                base.Result = ResponseCode;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Payment Process" + ex.ToString());
            }
        }

        public void ProcessImageSent(string ImageFile)
        {
            string[] Buffer;
            int FileLength;

            byte[] bytes = new byte[8001];
            string SignatureFileName = "SignatureFile.bmp";

            if (File.Exists(SignatureFileName))
            {
                try
                {
                    string Path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), SignatureFileName);
                    File.Delete(Path);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            var sw = new StreamWriter(SignatureFileName, true, Encoding.GetEncoding(1252));  // Windows-1252 or CP-1252 (code page – 1252)

            SignatureData = ImageFile;//To store the data in variable at retreive it in frmviewSignature Screen

            //I have to get the data from the image and put it into szImageData sECRResponse
            if (String.IsNullOrWhiteSpace(ImageFile))
            {
                isSignatureSigned = false;
                return;
            }
            else
            {
                Buffer = Strings.Split(ImageFile, "^");
                FileLength = System.Convert.ToInt32(Buffer[0]);
                isSignatureSigned = true;
            }

            try
            {
                FileLength -= 46; // the length of the text will rest 46 of the header to get the total file
                // infoLbl.Text = "this is the length of the file " & inFileLength & "THIS IS WHAT REMAINS: " & sECRResponse
                ImageData = ImageFile.Substring(53, FileLength); // I add the header 47 and 6 throughout the file so that it is where it should be                

                base.SignatureString = ImageData;   //To retrieve it in the frmViewSignature File      
                this.SignatureData = ImageData;

                sw.WriteLine(ImageData);
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AdjustDelete(string Adjresponse)
        {
            string[] AdjData;
            string Adjrequest;
            string ManualTrans;
            string ExtendedField;
            string OrigAmount = "0";
            string AdjustAmt;
            string TransTypeNum;
            string[] AdjustData;
            bool isFound;
            string[] ExtendedFieldArray = null;
            string TotalAmt = "0";
            string ProcessCode = "220000";//HardCoded Because in their Local Demo they have Applied the condition for Checkbox and Duplicate transaction ;
            string AddiontalAmt;
            string StateTax = "0";
            string CityTax = "0";
            string TransSettleDate;

            // sDataEntry = ""
            isTransApproved = false;
            isFound = false;
            TransTypeNum = "";
            ManualTrans = "";
            AddiontalAmt = "0";
            TransType = "ADJUST";

            AdjData = Adjresponse.Split(Strings.ChrW(28));

            try
            {
                if (AdjData.Length >= 1)
                {
                    if (AdjData[0] != "NOT FOUND")
                    {
                        try
                        {
                            if (AdjData.Length >= 4)
                            {
                                if (AdjData[3] != "SALE" & AdjData[3] != "REFUND")
                                {
                                    logger.Trace("UNABLE TO ADJUST");

                                }
                            }
                            else
                            {
                                logger.Trace("UNABLE TO ADJUST");
                            }
                            if (AdjData.Length >= 38)
                            {
                                if (AdjData[37] != "")
                                {
                                    AdjustData = Strings.Split(AdjData[37], " ");
                                    if (AdjustData[0] == "ADJUST" & AdjustData[1] == "COMPLETED:")
                                    {
                                        logger.Trace("TRANS ADJUSTED ON PURCHASE");
                                    }
                                    else if (AdjustData[0] == "ADJUSTED")
                                    {
                                        logger.Trace("TRANSACTION ADJUSTED " + AdjustData[2] + " " + AdjustData[3]);
                                    }
                                    else
                                        isFound = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Fatal(ex.ToString());
                            //throw ex;
                        }
                    }
                    else
                    {
                        logger.Trace("NOT FOUND");
                    }
                }
                else
                {
                    logger.Trace("NOT FOUND");
                }

                if (AdjData.Length >= 17)
                {
                    if (AdjData[17] != "")
                        AddiontalAmt = Strings.Format(Conversion.Val(Convert.ToInt64(AdjData[17]) * 100), "000000000000");
                    else
                        AddiontalAmt = Strings.Format(Conversion.Val(Convert.ToInt64(AddiontalAmt) * 100), "000000000000");
                }
                if (AdjData.Length >= 15)
                {
                    TotalAmt = (Convert.ToInt64(Conversion.Val(AdjData[14])) * 100).ToString();
                }
                if (AdjData.Length >= 15)
                {
                    OrigAmount = Strings.Format(Conversion.Val(Convert.ToDouble(AdjData[14]) * 100), "000000000000");
                }
                AdjustAmt = Strings.Mid(Strings.Format(Conversion.Val(TotalAmt), "000000000000"), 2);
                AdjustAmt = Strings.Format(Conversion.Val(AdjustAmt), "000000000000"); // Reformat to length 12
                if (AdjData.Length >= 4)
                {
                    if (AdjData[3] == "SALE")
                    {
                        TransTypeNum = "0200";
                    }
                    else if (AdjData[3] == "REFUND")
                    {
                        TransTypeNum = "0220";
                    }
                }
                if (AdjData.Length >= 33)
                {
                    if (AdjData[32] == "M")
                    {
                        ManualTrans = "1";
                        AdjData[19] = "/" + AdjData[19] + "/";
                        AdjData[19] = Strings.Format(AdjData[19].Length, "000") + AdjData[19];
                    }
                    else
                        ManualTrans = "0";
                    logger.Trace("MANUAL FLAG:" + ManualTrans);
                }
                if (AdjData.Length >= 35)
                {
                    ExtendedFieldArray = Strings.Split(AdjData[34], "/");
                }
                if (AdjData.Length >= 17)
                {
                    if (AdjData[15] == "" & AdjData[16] == "")
                    {
                        StateTax = "";
                        CityTax = "";
                    }
                    else
                    {
                        StateTax = Strings.Format(Conversion.Val(Convert.ToDouble(AdjData[15]) * 100), "000000000000");
                        CityTax = Strings.Format(Conversion.Val(Convert.ToDouble(AdjData[16]) * 100), "000000000000");
                    }
                }
                TransSettleDate = Strings.Format(Strings.Len(AdjData[28]), "000") + AdjData[28];

                ExtendedField = AdjData[2] + "/" + "/" + AdjData[5] + "/" + AdjData[6] + "/" + "/" + StateTax + "/" + CityTax + "/" + AdjustAmt + "00" + AdjData[10] + TransTypeNum + AdjData[20] + AdjData[22] + AdjData[21] + "00" + AdjData[36] + "/" + ExtendedFieldArray[17] + "/" + ExtendedFieldArray[22] + "/" + "/" + "/" + "/" + "/";

                Adjrequest = Constant.FormatID + "{0}" /*TRANSACTIONNAME*/ + PmtTxnResponse.SessionID + "{1}" /*REFERENCENUMBER*/ + Strings.Chr(28)
                + Strings.Chr(28) + AdjData[12] + Strings.Chr(28) + ProcessCode + Strings.Chr(28) + OrigAmount + Strings.Chr(28)
                + AddiontalAmt + Strings.Chr(28) + AdjData[19] + Strings.Chr(28) + AdjData[20] + Strings.Chr(28) + Strings.Chr(28)
                + Strings.Chr(28) + Strings.Chr(28) + Strings.Chr(28) + AdjData[25] + Strings.Chr(28) + Strings.Chr(28) + Strings.Chr(28) + TransSettleDate + Strings.Chr(28)
                + Strings.Chr(28) + Strings.Chr(28) + AdjData[31] + Strings.Chr(28) + ManualTrans + Strings.Chr(28) + Strings.Chr(28) + ExtendedField + Strings.Chr(28);

            }
            catch (Exception ex)
            {
                logger.Fatal(ex.ToString());
                throw ex;
            }

            return Adjrequest;
        }
    }
}
