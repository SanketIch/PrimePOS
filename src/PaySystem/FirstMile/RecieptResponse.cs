using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMile.Helpers;
using NLog;

namespace FirstMile
{
    public class RecieptResponse
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public string TransactionType { set; get; }
        public string TransactionResult { set; get; }
        public string TimeStamp { set; get; }
        public string MerchantOrderNumber { set; get; }
        public string MerchantID { set; get; }
        public string TerminalID { set; get; }
        public decimal Amount { set; get; }
        public decimal CashBack { set; get; }
        public string EntryMethod { set; get; }
        public string Name { set; get; }
        public string AccountType { set; get; }
        public string Account { set; get; }
        public string OrderID { set; get; }
        public string TranstactionID { set; get; }
        public string AuthCode { set; get; }
        public string DeclineCode { set; get; }
        public string DeclineMessage { set; get; }
        public string BatchNumber { set; get; }
        public string AVSResult { set; get; }
        public string CVV2Result { set; get; }
        public decimal Balance { set; get; }


        // EMV Tags

        public string AppName { set; get; }
        public string AppID { set; get; }
        public string TermVerifResult { set; get; }
        public string IssuerAppData { set; get; }
        public string TransStatusIndicator { set; get; }
        public string AppCryptoGram { set; get; }
        public string AuthRespCode { set; get; }
        public string CardVerifMethod { set; get; }

        public RecieptResponse()
        {
            this.CashBack = 0.0m;
            this.Balance = 0.0m;
            this.Amount = 0.0M;
        }

        public void LoadEmvResponse(string strResponse)
        {
            logger.Debug("Loading and Parsing EMV Response String");
            try
            {
                string[] responsetagList = strResponse.Split('|').Select(sValue => sValue.Trim()).ToArray();
                foreach (string tag in responsetagList)
                {
                    try
                    {
                        using (ResponseTag oresponsetag = new ResponseTag(tag, ":"))
                        {
                            switch (oresponsetag.Tag)
                            {
                                case EMVResponseStrings.APP_LABEL:
                                    this.AppName = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.AID:
                                    this.AppID = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.TVR:
                                    this.TermVerifResult = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.IAD:
                                    this.IssuerAppData = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.TSI:
                                    this.TransStatusIndicator = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.ARQC:
                                    this.AppCryptoGram = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.ARC:
                                    this.AuthRespCode = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.CVM:
                                    this.CardVerifMethod = Helpers.GlobalHelpers.DecodeCVM(oresponsetag.Value);
                                    break;
                                default:
                                    break;


                            }
                        }
                    }
                    catch(Exception expn)
                    {
                        logger.Error(expn,"An Error Occured while processing the EMV Response Tag "+tag+": "+ expn.Message);
                    }
                    
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An Error Occured while processing the EMV Response " + ex.Message);
            }
            
        }

        public void LoadResponse(string strResponse)
        {
            logger.Debug("Loading and Parsing Reciept Response String");

            try
            {
                string[] responsetagList = strResponse.Split('|').Select(sValue => sValue.Trim()).ToArray();

                foreach (string tag in responsetagList)
                {
                    try
                    {
                        using (ResponseTag oresponsetag = new ResponseTag(tag, ":"))
                        {
                            switch (oresponsetag.Tag)
                            {
                                case EMVResponseStrings.TRANSACTION_TYPE:
                                    this.TransactionType = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.TRANSACTION_RESULT:
                                    this.TransactionResult = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.TIME_STAMP:
                                    this.TimeStamp = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.MERCHANT_ORDER_NUMBER:
                                    this.MerchantOrderNumber = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.MERCHANT_ID:
                                    this.MerchantID = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.TERM_ID:
                                    this.TerminalID = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.AMOUNT:
                                    this.Amount = GlobalHelpers.StringToDecimal(oresponsetag.Value);
                                    break;
                                case EMVResponseStrings.CASH_BACK:
                                    this.CashBack = GlobalHelpers.StringToDecimal(oresponsetag.Value);
                                    break;
                                case EMVResponseStrings.ENTRY_METHOD:
                                    this.EntryMethod = Helpers.GlobalHelpers.DecodeEntryMethod(oresponsetag.Value);
                                    break;
                                case EMVResponseStrings.NAME:
                                    this.Name = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.ACCOUNT_TYPE:
                                    this.AccountType = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.ACCOUNT:
                                    this.Account = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.ORDER_ID:
                                    this.OrderID = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.TRANSACTION_ID:
                                    this.TranstactionID = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.AUTH_CODE:
                                    this.AuthCode = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.DECLINE_CODE:
                                    this.DeclineCode = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.DECLINE_MESSAGE:
                                    this.DeclineMessage = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.BATCH_NUMBER:
                                    this.BatchNumber = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.AVS_RESULT:
                                    this.AVSResult = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.CVV2_RESULT:
                                    this.CVV2Result = oresponsetag.Value;
                                    break;
                                case EMVResponseStrings.BALANCE:
                                    this.Balance = GlobalHelpers.StringToDecimal(oresponsetag.Value);
                                    break;
                            }
                        }

                    }
                    catch(Exception expn)
                    {
                        logger.Error(expn, "An Error Occured while processing the Reciept Response Tag " + tag + ": " + expn.Message);
                    }
                    
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An Error Occured while processing the Reciept Response "+ ex.Message);
            }

            

        }


    }
}
