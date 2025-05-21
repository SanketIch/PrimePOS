using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using NLog;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;
using PossqlData;

namespace Solutran
{
    public class SoluTranProcessor
    {
        #region Variable Declaration
        private ILogger logger = LogManager.GetCurrentClassLogger();
        public S3Transaction objTrans = null;
        public S3Transaction getRespone = new S3Transaction();
        public RestClientHelper restClientHelper = null;
        //DataSet dsReturn = null;
        public string Result = string.Empty;
        string SerializeResult = string.Empty;
        public static Random oRandom = new Random();
        DateTime date = DateTime.Now;
        string localDatetime = string.Empty;
        string busTime = string.Empty;
        string termID = string.Empty;
        string TerminalID = string.Empty;
        long lRefNumber;


        /*
         01 - Authorization Request
         02 - Cancel 
         03 - Confirmation
         07 - Balance Inqury
         08- Void 
         18 - Refund
          */

        #endregion

        public SoluTranProcessor()
        {
            localDatetime = date.ToString("yyMMddHHmmss");
            busTime = date.ToString("yyMMdd");
            termID = date.ToString("yyyyMMdd");
        }

        public static int GetRandomNo()
        {
            return oRandom.Next(100000, 9999999);
        }

        //private void ReadRefNumber()
        //{
        //    FileStream fs = new FileStream("TerminalID.txt", FileMode.Open, FileAccess.Read);
        //    StreamReader d = new StreamReader(fs);
        //    d.BaseStream.Seek(0, SeekOrigin.Begin);
        //    while (d.Peek() > -1)
        //        lRefNumber = Convert.ToInt32(d.ReadLine());

        //    d.Close();
        //}

        //private void SaveRefNumber()
        //{
        //    FileStream fs = new FileStream("TerminalID.txt", FileMode.Create, FileAccess.Write);
        //    StreamWriter s = new StreamWriter(fs);

        //    s.BaseStream.Seek(0, SeekOrigin.End);
        //    s.WriteLine(lRefNumber);
        //    s.Close();
        //}
        private void ReadRefNumber()
        {
            //FileStream fs = new FileStream("TerminalID.txt", FileMode.Open, FileAccess.Read);
            //StreamReader d = new StreamReader(fs);
            //d.BaseStream.Seek(0, SeekOrigin.Begin);
            //while (d.Peek() > -1)
            //    lRefNumber = Convert.ToInt32(d.ReadLine());

            //d.Close();
            var db = new Possql();
            TransactionReference tr = new TransactionReference();
            tr = db.TransactionReferences.Where(w => w.Processor.ToUpper() == "SOLUTRAN").SingleOrDefault();
            lRefNumber = Convert.ToInt64(tr.LastTransaction);
        }

        private void SaveRefNumber()
        {
            //FileStream fs = new FileStream("TerminalID.txt", FileMode.Create, FileAccess.Write);
            //StreamWriter s = new StreamWriter(fs);

            //s.BaseStream.Seek(0, SeekOrigin.End);
            //s.WriteLine(lRefNumber);
            //s.Close();
            var db = new Possql();
            TransactionReference tr = new TransactionReference();
            tr = db.TransactionReferences.Where(w => w.Processor.ToUpper() == "SOLUTRAN").SingleOrDefault();
            tr.LastTransaction = Convert.ToString(lRefNumber);
            db.TransactionReferences.Attach(tr);
            db.Entry(tr).Property(p => p.LastTransaction).IsModified = true;
            db.SaveChanges();

        }

        public void getTerminalNumber()
        {
            ReadRefNumber();
            lRefNumber = lRefNumber + 1;
            if (lRefNumber > 999999)
            {
                lRefNumber = 1;
            }
            TerminalID = Strings.Format(lRefNumber, "0000000");
            SaveRefNumber();
        }

        #region API methods

        public DataSet AuthorizeDiscount(DataSet dsTransData, string url, string key, ref string s3TransID, ref string actionCode, string stationID, string s3Merchant, string storedID)
        {
            try
            {
                string logResult= string.Empty;
                string logResResult = string.Empty;
                getTerminalNumber();

                logger.Trace("AuthorizeDiscount() - Entered");
                int ProdCount = dsTransData.Tables["POSTransactionDetail"].Rows.Count;
                objTrans = new S3Transaction();
                objTrans.VerType = SolutranProperties.VersionType;
                objTrans.MessType = MessageType.AuthMessType;
                objTrans.LocalDateTime = Convert.ToInt64(localDatetime);// 190327182111;
                objTrans.BusDate = Convert.ToInt32(busTime);// 190327;
                objTrans.S3MerchID = s3Merchant;// "0078";
                objTrans.RetaiLoc = storedID;
                objTrans.TermID = stationID;
                objTrans.TermTranID = termID + stationID + TerminalID;//"2019050700000006";// GetRandomNo().ToString();// "0224201910392856";// Auto Generate 
                objTrans.OrderID = GetRandomNo().ToString();// "01234567890"; // Auto Generate 
                objTrans.APLVer = "0"; // OPtional
                objTrans.CurrCde = SolutranProperties.CurrencyCode;
                objTrans.S3CardCt = dsTransData.Tables["Card Details"].Rows.Count.ToString();
                objTrans.S3RecMess = ""; // X
                objTrans.S3ProdCt = ProdCount.ToString();

                decimal TotalPurAmt = 0;
                decimal TotalTranAmt = 0;
                decimal TotalTaxAmt = 0;
                decimal TotalDiscount = 0;
                for (int i = 0; ProdCount > i; i++)
                {
                    TotalPurAmt += Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString());
                    TotalTranAmt += (Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString()) + Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"].ToString())) - Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["Discount"].ToString());
                    TotalTaxAmt += Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"].ToString());
                    TotalDiscount += Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["Discount"].ToString());
                }
                objTrans.S3PurAmt = TotalPurAmt.ToString();// Total Purchase Amount
                objTrans.TotalTranAmt = TotalTranAmt.ToString();// "0.73";// Total POS Transaction Amount include TAX and POS Discount
                objTrans.TotalTaxAmt = TotalTaxAmt.ToString(); // Total POS Transaction Tax Amount
                objTrans.TotalTranDisc = "0.00";
                //objTrans.S3TranID = getRespone.S3TranID;
                objTrans.ActionCode = "0";

                #region


                if (dsTransData.Tables.Count > 0 && dsTransData.Tables["POSTransactionDetail"].Rows.Count > 0)
                {
                    int cnt = dsTransData.Tables["POSTransactionDetail"].Rows.Count;
                    objTrans.ProductInfo = new ProductInfo[cnt];
                    for (int i = 0; i < cnt; i++)
                    {
                        objTrans.ProductInfo[i] = new ProductInfo();
                        objTrans.ProductInfo[i].ProdCode = dsTransData.Tables["POSTransactionDetail"].Rows[i]["ItemID"].ToString();
                        objTrans.ProductInfo[i].PurchAmt = dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString();// TotalPurchase.ToString();
                        objTrans.ProductInfo[i].NonS3DiscAmt = dsTransData.Tables["POSTransactionDetail"].Rows[i]["Discount"].ToString();
                        objTrans.ProductInfo[i].TaxAmt = dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"].ToString();
                        objTrans.ProductInfo[i].QuantityType = "U";
                        objTrans.ProductInfo[i].PurQuantity = dsTransData.Tables["POSTransactionDetail"].Rows[i]["Qty"].ToString();
                        objTrans.ProductInfo[i].Depart = "1";
                        objTrans.ProductInfo[i].ProdLevel = "0";
                        objTrans.ProductInfo[i].OrdinalNum = (i + 1).ToString();
                    }
                }
                #endregion


                if (dsTransData.Tables.Count > 0 && dsTransData.Tables["Card Details"].Rows.Count > 0)
                {
                    int cnt = dsTransData.Tables["Card Details"].Rows.Count;
                    objTrans.CardInfo = new CardInfo[cnt];
                    for (int i = 0; i < cnt; i++)
                    {
                        objTrans.CardInfo[i] = new CardInfo();
                        objTrans.CardInfo[i].BarcodeData = dsTransData.Tables["Card Details"].Rows[i]["CardNumber"].ToString();
                        objTrans.CardInfo[i].POSDataCode = dsTransData.Tables["Card Details"].Rows[i]["POSDataCode"].ToString();//SolutranProperties.POSDataCode;// dsTransData.Tables[0].Rows[i]["POSDataCode"].ToString(); //PRIMEPOS-3488
                        objTrans.CardInfo[i].CVV = dsTransData.Tables["Card Details"].Rows[i]["CVV"].ToString();// dsTransData.Tables["Card Details"].Rows[i]["CVV"].ToString(); //PRIMEPOS-3488
                    }
                }

                #region Commented
                //objTrans.CardInfo = new CardInfo[1];
                //objTrans.CardInfo[0] = new CardInfo();
                //objTrans.CardInfo[0].BarcodeData = "63681102000000141";
                //objTrans.CardInfo[0].POSDataCode = "11";
                //objTrans.CardInfo[0].CVV = "1234";

                //objTrans.CardInfo[1] = new CardInfo();
                //objTrans.CardInfo[1].BarcodeData = 000555555001000382060;
                //objTrans.CardInfo[1].POSDataCode = "12";
                //objTrans.CardInfo[1].CVV = "01234";

                //objTrans.ProductInfo = new ProductInfo[2];
                //objTrans.ProductInfo[0] = new ProductInfo();
                //objTrans.ProductInfo[0].ProdCode = "00089299200247";
                //objTrans.ProductInfo[0].Depart = "1";
                //// APLTypeCode
                //objTrans.ProductInfo[0].OrdinalNum = "0";
                //objTrans.ProductInfo[0].PurchAmt = "0.50";
                //objTrans.ProductInfo[0].NonS3DiscAmt = "0.00";
                //objTrans.ProductInfo[0].TaxAmt = "0.00";
                //objTrans.ProductInfo[0].QuantityType = "U";
                //objTrans.ProductInfo[0].PurQuantity = "1";
                //objTrans.ProductInfo[1] = new ProductInfo();
                //objTrans.ProductInfo[1].ProdCode = "00004116705360";

                //objTrans.ProductInfo[1].Depart = "2";
                ////APLTypeCode
                //objTrans.ProductInfo[1].OrdinalNum = "1";
                //objTrans.ProductInfo[1].PurchAmt = "0.25";
                //objTrans.ProductInfo[1].NonS3DiscAmt = "0.00";
                //objTrans.ProductInfo[1].TaxAmt = "0.00";
                //objTrans.ProductInfo[1].QuantityType = "U";
                //objTrans.ProductInfo[1].PurQuantity = "1";
                #endregion
                SerializeResult = JsonConvert.SerializeObject(objTrans, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                #region PRIMEPOS-3487 adding logic to mask the cvv number
                try
                {
                    var logTrans = JsonConvert.DeserializeObject<S3Transaction>(JsonConvert.SerializeObject(objTrans));
                    if (logTrans.CardInfo != null && logTrans.CardInfo.Length > 0)
                    {
                        foreach (var card in logTrans.CardInfo)
                        {
                            if (!string.IsNullOrEmpty(card.CVV))
                            {
                                card.CVV = "***";
                            }
                        }
                    }
                    logResult = JsonConvert.SerializeObject(logTrans, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    logger.Trace("AuthorizeDiscount() - SerializeData:" + "{TransInfo :" + logResult + "}");
                    //logger.Trace("AuthorizeDiscount() - SerializeData:" + "{TransInfo :" + SerializeResult + "}"); //PRIMEPOS-3488 we might needed to hide cvv in request
                }
                catch (Exception ex)
                {
                    logger.Trace("AuthorizeDiscount() - An exception occurred while attempting to mask CVV during data serialization : " + ex.Message.ToString());
                    logResult = string.Empty;
                }
                #endregion
                restClientHelper = new RestClientHelper();
                logger.Trace("restClientHelper.MakeRequest() - Entered");
                Result = restClientHelper.MakeRequest(url, "/Process", JsonServiceInfo.PostMethod, "{\n\"TransInfo\" :" + SerializeResult + "\n}", key, "ascii");
                #region PRIMEPOS-3510 adding logic to mask the cvv number
                //logger.Trace("AuthorizeDiscount() - DeserializeData:" + Result);
                try
                {
                    JObject jsonobj = JsonConvert.DeserializeObject<JObject>(Result);
                    var logResTrans = JsonConvert.DeserializeObject<S3Transaction>(jsonobj.SelectToken("TransInfo").ToString());
                    if (logResTrans.CardInfo != null && logResTrans.CardInfo.Length > 0)
                    {
                        foreach (var card in logResTrans.CardInfo)
                        {
                            if (!string.IsNullOrEmpty(card.CVV))
                            {
                                card.CVV = "***";
                            }
                        }
                    }
                    logResResult = JsonConvert.SerializeObject(logResTrans, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    logger.Trace("AuthorizeDiscount() - DeserializeData:" + logResResult);
                }
                catch (Exception ex)
                {
                    logger.Trace($"AuthorizeDiscount() - An exception occurred while attempting to mask CVV during data deserialization: {ex.Message}");
                    logResResult = string.Empty;
                }
                #endregion

                JObject jobj = JsonConvert.DeserializeObject<JObject>(Result);
                getRespone = JsonConvert.DeserializeObject<S3Transaction>(jobj.SelectToken("TransInfo").ToString());

                s3TransID = getRespone.S3TranID;
                actionCode = getRespone.ActionCode;

                for (int i = 0; i <= dsTransData.Tables["POSTransactionDetail"].Rows.Count - 1; i++)
                {
                    dsTransData.Tables["POSTransactionDetail"].Rows[i]["S3DiscountAmount"] = 0.00;
                }

                UpdateDiscountAtItemLevel(dsTransData, getRespone, TotalTranAmt);

                logger.Trace("AuthorizeDiscount() - End");
                Save_CCTransnmissionLog(Convert.ToDecimal(objTrans.TotalTranAmt), "{\n\"TransInfo\" :" + logResult + "\n}", Result);
            }
            catch (Exception ex)
            {
                logger.Trace("AuthorizeDiscount() - Error : " + ex.Message.ToString());
            }

            return dsTransData;
        }

        public string Void(DataSet dsTransData, string url, string key, string S3TransID, string stationID, string S3Merchant, string storedID)
        {
            try
            {
                getTerminalNumber();

                logger.Trace("Void() - Entered");
                int ProdCount = dsTransData.Tables["POSTransactionDetail"].Rows.Count;
                objTrans = new S3Transaction();
                objTrans.VerType = SolutranProperties.VersionType;
                objTrans.MessType = MessageType.VoidMessType;
                objTrans.LocalDateTime = Convert.ToInt64(localDatetime);// 190327182111;
                objTrans.BusDate = Convert.ToInt32(busTime);// 190327;
                objTrans.S3MerchID = S3Merchant;
                objTrans.RetaiLoc = storedID;
                objTrans.TermID = stationID;
                objTrans.TermTranID = termID + stationID + TerminalID;
                objTrans.OrderID = GetRandomNo().ToString();
                objTrans.CurrCde = SolutranProperties.CurrencyCode;
                objTrans.S3CardCt = "1";
                objTrans.S3RecMess = "";
                objTrans.S3ProdCt = ProdCount.ToString();
                objTrans.S3PurAmt = "0";
                objTrans.TotalTranAmt = "0.00";
                objTrans.TotalTaxAmt = "0.00";
                objTrans.TotalTranDisc = "0.00";
                objTrans.S3TranID = S3TransID;
                objTrans.ActionCode = "";

                SerializeResult = JsonConvert.SerializeObject(objTrans, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                logger.Trace("Void() - SerializeData:" + "{TransInfo :" + SerializeResult + "}");
                restClientHelper = new RestClientHelper();
                logger.Trace("restClientHelper.MakeRequest() - Entered");
                Result = restClientHelper.MakeRequest(url, "/Process", JsonServiceInfo.PostMethod, "{\n\"TransInfo\" :" + SerializeResult + "\n}", key, "ascii");
                logger.Trace("Void() - DeserializeData:" + Result);
                getRespone = new S3Transaction();
                JObject jobj = JsonConvert.DeserializeObject<JObject>(Result);
                getRespone = JsonConvert.DeserializeObject<S3Transaction>(jobj.SelectToken("TransInfo").ToString());
                logger.Trace("Void() - End");
            }
            catch (Exception ex)
            {
                logger.Trace("Void() - Error : " + ex.Message.ToString());
            }
            return getRespone.ActionCode;
        }

        public DataSet Refund(DataSet dsOrgTransData, string url, string key, string s3TransID, string CardNumber, ref string actionCode, string stationID, string s3Merchant, string storedID)
        {

            try
            {
                DataSet dsTransData = new DataSet();
                DataTable dtS3 = new DataTable();
                dtS3 = dsOrgTransData.Tables["POSTransactionDetail"].AsEnumerable().Where(r => r.Field<Int64>("S3TransID") != 0).CopyToDataTable();
                dtS3.TableName = "POSTransactionDetail";
                dsTransData.Tables.Add(dtS3);

                getTerminalNumber();

                logger.Trace("Refund() - Entered");
                int ProdCount = dsTransData.Tables["POSTransactionDetail"].Rows.Count;
                objTrans = new S3Transaction();
                objTrans.VerType = SolutranProperties.VersionType;
                objTrans.MessType = MessageType.RefundMessType;
                objTrans.LocalDateTime = Convert.ToInt64(localDatetime);// 190327182111;
                objTrans.BusDate = Convert.ToInt32(busTime);// 190327;
                objTrans.S3MerchID = s3Merchant;// "0078";
                objTrans.RetaiLoc = storedID;
                objTrans.TermID = stationID;
                objTrans.TermTranID = termID + stationID + TerminalID;
                objTrans.OrderID = GetRandomNo().ToString();
                objTrans.APLVer = "0";
                objTrans.CurrCde = SolutranProperties.CurrencyCode;
                objTrans.S3CardCt = "01";
                objTrans.S3RecMess = "0";
                objTrans.S3ProdCt = ProdCount.ToString();
                objTrans.S3TranID = s3TransID;
                //objTrans.S3PurAmt = "0";
                //objTrans.TotalTranAmt = "0.79";
                //objTrans.TotalTaxAmt = "0.00";
                //objTrans.TotalTranDisc = "0.00";
                //objTrans.S3TranID = "200152408";
                //objTrans.ActionCode = "0";

                decimal TotalPurAmt = 0;
                decimal TotalTranAmt = 0;
                decimal TotalTaxAmt = 0;
                decimal TotalDiscount = 0;
                for (int i = 0; ProdCount > i; i++)
                {
                    if (dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString().Contains("-"))
                    {
                        dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"] = dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString().Substring(1);
                    }
                    if (dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"].ToString().Contains("-"))
                    {
                        dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"] = dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"].ToString().Substring(1);
                    }
                    TotalPurAmt += Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString());
                    TotalTranAmt += (Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString()) + Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"].ToString())) - Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["Discount"].ToString());
                    TotalTaxAmt += Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"]);  //PRIMEPOS-3390
                    TotalDiscount += Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["Discount"].ToString());
                }
                objTrans.S3PurAmt = TotalPurAmt.ToString();// Total Purchase Amount
                objTrans.TotalTranAmt = TotalTranAmt.ToString();// "0.73";// Total POS Transaction Amount include TAX and POS Discount
                objTrans.TotalTaxAmt = TotalTaxAmt.ToString(); // PRIMEPOS-3390 Total POS Transaction Tax Amount
                objTrans.TotalTranDisc = "0";//Added
                //objTrans.S3TranID = getRespone.S3TranID;
                objTrans.ActionCode = "0";


                if (dsTransData.Tables.Count > 0 && dsTransData.Tables["POSTransactionDetail"].Rows.Count > 0)
                {
                    int cnt = dsTransData.Tables["POSTransactionDetail"].Rows.Count;
                    objTrans.ProductInfo = new ProductInfo[cnt];
                    for (int i = 0; i < cnt; i++)
                    {
                        objTrans.ProductInfo[i] = new ProductInfo();
                        objTrans.ProductInfo[i].ProdCode = dsTransData.Tables["POSTransactionDetail"].Rows[i]["ItemID"].ToString();

                        objTrans.ProductInfo[i].PurchAmt = dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString();// TotalPurchase.ToString();
                        objTrans.ProductInfo[i].NonS3DiscAmt = dsTransData.Tables["POSTransactionDetail"].Rows[i]["Discount"].ToString();
                        objTrans.ProductInfo[i].TaxAmt = dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"].ToString(); //PRIMEPOS-3390
                        objTrans.ProductInfo[i].QuantityType = "U";
                        if (dsTransData.Tables["POSTransactionDetail"].Rows[i]["Qty"].ToString().Contains("-"))
                        {
                            dsTransData.Tables["POSTransactionDetail"].Rows[i]["Qty"] = dsTransData.Tables["POSTransactionDetail"].Rows[i]["Qty"].ToString().Substring(1);
                        }
                        objTrans.ProductInfo[i].PurQuantity = dsTransData.Tables[0].Rows[i]["Qty"].ToString();
                    }
                }

                string[] arr = CardNumber.Split('|');
                objTrans.CardInfo = new CardInfo[arr.Count()];

                for (int i = 0; i < arr.Count(); i++)
                {
                    objTrans.CardInfo[i] = new CardInfo();
                    objTrans.CardInfo[i].BarcodeData = arr[i].ToString();
                    objTrans.CardInfo[i].POSDataCode = SolutranProperties.POSDataCode;
                    objTrans.CardInfo[i].CVV = ""; //EP-3495 removed the harcoded cvv
                }

                //objTrans.CardInfo[0] = new CardInfo();
                //objTrans.CardInfo[0].BarcodeData = CardNumber;
                //objTrans.CardInfo[0].POSDataCode = SolutranProperties.POSDataCode;
                //objTrans.CardInfo[0].CVV = "1234";
                #region Commented
                //objTrans.ProductInfo = new ProductInfo[2];

                //objTrans.ProductInfo[0] = new ProductInfo();//1
                //objTrans.ProductInfo[0].ProdCode = "00065006600171";
                //objTrans.ProductInfo[0].ProdLevel = "1";
                //objTrans.ProductInfo[0].Depart = "";
                //objTrans.ProductInfo[0].OrdinalNum = "0001";
                //objTrans.ProductInfo[0].PurchAmt = "15.65";
                //objTrans.ProductInfo[0].NonS3DiscAmt = "0";
                //objTrans.ProductInfo[0].TaxAmt = "0";
                //objTrans.ProductInfo[0].QuantityType = "U";
                //objTrans.ProductInfo[0].PurQuantity = "1";
                //objTrans.ProductInfo[1] = new ProductInfo();//2
                //objTrans.ProductInfo[1].ProdCode = "00065006600172";
                //objTrans.ProductInfo[1].ProdLevel = "1";
                //objTrans.ProductInfo[1].Depart = "";
                //objTrans.ProductInfo[1].OrdinalNum = "0001";
                //objTrans.ProductInfo[1].PurchAmt = "15.65";
                //objTrans.ProductInfo[1].NonS3DiscAmt = "0";
                //objTrans.ProductInfo[1].TaxAmt = "0";
                //objTrans.ProductInfo[1].QuantityType = "U";
                //objTrans.ProductInfo[1].PurQuantity = "1";
                #endregion

                SerializeResult = JsonConvert.SerializeObject(objTrans, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                logger.Trace("Refund() - SerializeData:" + "{TransInfo :" + SerializeResult + "}");
                restClientHelper = new RestClientHelper();
                logger.Trace("restClientHelper.MakeRequest() - Entered");
                Result = restClientHelper.MakeRequest(url, "/Process", JsonServiceInfo.PostMethod, "{\n\"TransInfo\" :" + SerializeResult + "\n}", key, "ascii");
                logger.Trace("Refund() - DeserializeData:" + Result);
                getRespone = new S3Transaction();
                JObject jobj = JsonConvert.DeserializeObject<JObject>(Result);
                getRespone = JsonConvert.DeserializeObject<S3Transaction>(jobj.SelectToken("TransInfo").ToString());
                s3TransID = getRespone.S3TranID;
                actionCode = getRespone.ActionCode;

                for (int i = 0; i <= dsTransData.Tables[0].Rows.Count - 1; i++)
                {
                    dsTransData.Tables["POSTransactionDetail"].Rows[i]["S3TransID"] = getRespone.S3TranID;
                    if (getRespone.DiscountInfo.Length > 0)
                    {
                        dsTransData.Tables["POSTransactionDetail"].Rows[i]["S3DiscountAmount"] = Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"]) + Convert.ToDecimal(dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"]);  //PRIMEPOS-3390
                        dsTransData.Tables["POSTransactionDetail"].Rows[i]["S3PurAmount"] = dsTransData.Tables["POSTransactionDetail"].Rows[i]["ExtendedPrice"].ToString();
                        dsTransData.Tables["POSTransactionDetail"].Rows[i]["S3TaxAmount"] = dsTransData.Tables["POSTransactionDetail"].Rows[i]["TaxAmount"].ToString();  //PRIMEPOS-3390
                    }
                    else
                    {
                        dsTransData.Tables["POSTransactionDetail"].Rows[i]["S3DiscountAmount"] = 0.00;
                        dsTransData.Tables["POSTransactionDetail"].Rows[i]["S3PurAmount"] = 0.00;
                        dsTransData.Tables["POSTransactionDetail"].Rows[i]["S3TaxAmount"] = 0.00;  //PRIMEPOS-3390
                    }
                }


                for (int i = 0; i < dsOrgTransData.Tables["POSTransactionDetail"].Rows.Count; i++)
                {
                    for (int c = 0; c < dsTransData.Tables["POSTransactionDetail"].Rows.Count; c++)
                    {
                        if (Equals(dsOrgTransData.Tables["POSTransactionDetail"].Rows[i]["ItemID"], dsTransData.Tables["POSTransactionDetail"].Rows[c]["ItemID"]))
                        {
                            dsOrgTransData.Tables["POSTransactionDetail"].Rows[i]["S3TransID"] = dsTransData.Tables["POSTransactionDetail"].Rows[c]["S3TransID"];
                            dsOrgTransData.Tables["POSTransactionDetail"].Rows[i]["S3PurAmount"] = dsTransData.Tables["POSTransactionDetail"].Rows[c]["S3PurAmount"];
                            dsOrgTransData.Tables["POSTransactionDetail"].Rows[i]["S3DiscountAmount"] = dsTransData.Tables["POSTransactionDetail"].Rows[c]["S3DiscountAmount"];
                            dsOrgTransData.Tables["POSTransactionDetail"].Rows[i]["S3TaxAmount"] = dsTransData.Tables["POSTransactionDetail"].Rows[c]["S3TaxAmount"];  //PRIMEPOS-3390
                        }

                    }
                }


                logger.Trace("Refund() - End");
            }
            catch (Exception ex)
            {
                logger.Trace("Refund() - Error : " + ex.Message.ToString());
            }

            return dsOrgTransData;
        }

        #endregion

        public DataSet UpdateDiscountAtItemLevel(DataSet updateDiscountDataset, S3Transaction response, decimal TotalTranAmt)
        {
            decimal TotalDiscount = 0m;
            try
            {
                for (int i = 0; i <= response.DiscountInfo.Length - 1; i++)
                {
                    TotalDiscount += Convert.ToDecimal(response.DiscountInfo[i].AppDiscAmt);
                }
                for (int i = 0; i < response.DiscItems.Count(); i++)
                {
                    int ordinalNumber = Convert.ToInt32(response.DiscItems[i].DiscOrdinalNum) - 1;
                    updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["S3TransID"] = response.S3TranID;
                    if (TotalDiscount >= (Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["ExtendedPrice"]) + Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["TaxAmount"])))
                    {
                        updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["S3DiscountAmount"] = (Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["ExtendedPrice"].ToString()) + Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["TaxAmount"].ToString()));
                        updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["S3PurAmount"] = Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["ExtendedPrice"].ToString());
                        updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["S3TaxAmount"] = Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["TaxAmount"]);  //PRIMEPOS-3390
                    }
                    else
                    {
                        updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["S3DiscountAmount"] = TotalDiscount;
                        updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["S3PurAmount"] = Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["ExtendedPrice"].ToString());
                        updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["S3TaxAmount"] = Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["TaxAmount"]);  //PRIMEPOS-3390
                        break;
                    }

                    TotalDiscount -= (Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["ExtendedPrice"].ToString()) + Convert.ToDecimal(updateDiscountDataset.Tables["POSTransactionDetail"].Rows[ordinalNumber]["TaxAmount"].ToString()));
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in UpdateDiscountAtItemLevel" + ex.ToString());
                throw ex;
            }
            return updateDiscountDataset;
        }

        public void Save_CCTransnmissionLog(decimal amount, string RequestMessage, string DeviceResponse)
        {
            try
            {
                using (var db = new Possql())
                {
                    CCTransmission_Log cclog = new CCTransmission_Log();
                    cclog.TransDateTime = DateTime.Now;
                    cclog.TransAmount = amount;
                    cclog.TransDataStr = RequestMessage;
                    cclog.RecDataStr = DeviceResponse;
                    db.CCTransmission_Logs.Add(cclog);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }


        #region Commented
        //public DataSet CancelAuthorization(DataSet dsTransData, string url, string key)
        //{
        //    try
        //    {
        //        logger.Trace("CancelAuthorization() - Entered");
        //        dsReturn = null;
        //        //string result = JsonConvert.SerializeObject(dsTransData Formatting.Indented);
        //        objTrans = new S3Transaction();
        //        objTrans.VerType = "V04";
        //        objTrans.MessType = MessageType.CancelMessType;
        //        objTrans.LocalDateTime = 190325181011;
        //        objTrans.BusDate = 190325;
        //        objTrans.S3MerchID = "0078";
        //        objTrans.RetaiLoc = "100";
        //        objTrans.TermID = "0001";
        //        objTrans.TermTranID = "1234123412341234"; //c
        //        objTrans.OrderID = "01234567890";
        //        //objTrans.APLVer = "0";
        //        objTrans.CurrCde = "USD";
        //        objTrans.S3CardCt = "01";
        //        objTrans.S3RecMess = "";
        //        objTrans.S3ProdCt = "002";
        //        objTrans.S3PurAmt = "0";
        //        objTrans.TotalTranAmt = "0.77";
        //        objTrans.TotalTranDisc = "0.00";
        //        objTrans.TotalTaxAmt = "0.00";
        //        objTrans.S3TranID = "200152406";
        //        objTrans.ActionCode = "0";

        //        SerializeResult = JsonConvert.SerializeObject(objTrans, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        //        logger.Trace("CancelAuthorization() - SerializeData:" + "{TransInfo :" + SerializeResult + "}");
        //        restClientHelper = new RestClientHelper();
        //        logger.Trace("restClientHelper.MakeRequest() - Entered");
        //        Result = restClientHelper.MakeRequest(url, "/Process", JsonServiceInfo.PostMethod, "{\n\"TransInfo\" :" + SerializeResult + "\n}", key, "ascii");
        //        logger.Trace("CancelAuthorization() - DeserializeData:" + Result);
        //        getRespone = new S3Transaction();
        //        JObject jobj = JsonConvert.DeserializeObject<JObject>(Result);
        //        getRespone = JsonConvert.DeserializeObject<S3Transaction>(jobj.SelectToken("TransInfo").ToString());


        //        dsReturn = null;

        //        logger.Trace("CancelAuthorization() - End");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Trace("CancelAuthorization() - Error : " + ex.Message.ToString());
        //    }
        //    return dsReturn;
        //}

        //public DataSet ConfirmAuthorization(DataSet dsTransData, string url, string key)
        //{
        //    try
        //    {
        //        logger.Trace("ConfirmAuthorization() - Entered");
        //        dsReturn = null;
        //        //string result = JsonConvert.SerializeObject(dsTransData Formatting.Indented);
        //        objTrans = new S3Transaction();
        //        objTrans.VerType = "V04";
        //        objTrans.MessType = MessageType.ConfirmMessType;
        //        objTrans.LocalDateTime = 190325181011;
        //        objTrans.BusDate = 190325;
        //        objTrans.S3MerchID = "0078";
        //        objTrans.RetaiLoc = "100";
        //        objTrans.TermID = "0001";
        //        objTrans.TermTranID = "1234123412341234";//c
        //        objTrans.OrderID = "01234567890";
        //        //objTrans.APLVer = "0";
        //        objTrans.CurrCde = "USD";
        //        objTrans.S3CardCt = "01";
        //        objTrans.S3RecMess = "";
        //        objTrans.S3ProdCt = "002";
        //        objTrans.S3PurAmt = "0";
        //        objTrans.TotalTranAmt = "0.77";
        //        objTrans.TotalTranDisc = "0.00";
        //        objTrans.TotalTaxAmt = "0.00";
        //        objTrans.S3TranID = "200152406";
        //        objTrans.ActionCode = "";

        //        SerializeResult = JsonConvert.SerializeObject(objTrans, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        //        logger.Trace("ConfirmAuthorization() - SerializeData:" + "{TransInfo :" + SerializeResult + "}");
        //        restClientHelper = new RestClientHelper();
        //        logger.Trace("restClientHelper.MakeRequest() - Entered");
        //        Result = restClientHelper.MakeRequest(url, "/Process", JsonServiceInfo.PostMethod, "{\n\"TransInfo\" :" + SerializeResult + "\n}", key, "ascii");
        //        logger.Trace("ConfirmAuthorization() - DeserializeData:" + Result);
        //        getRespone = new S3Transaction();
        //        JObject jobj = JsonConvert.DeserializeObject<JObject>(Result);
        //        getRespone = JsonConvert.DeserializeObject<S3Transaction>(jobj.SelectToken("TransInfo").ToString());

        //        //  https://support.solutran.com/POST/MessagingServiceV2/S3Auth.svc

        //        dsReturn = null;

        //        logger.Trace("ConfirmAuthorization() - End");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Trace("ConfirmAuthorization() - Error : " + ex.Message.ToString());
        //    }

        //    return dsReturn;
        //}
        #endregion
    }
}
