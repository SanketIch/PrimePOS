using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FirstMile.Helpers;
using NLog;

namespace FirstMile
{
    public class TransactionResult
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public string Result { set; get; }

        public string Account { set; get; }
        public decimal Amount { set; get; }
        public string Expiration { set; get; }
        public string ApprovalCode { set; get; }
        public string Name { set; get; }
        public string TransactionID { set; get; }
        public string OrderID { set; get; }
        public string TransDetail { set; get; }
        public string AccountType { set; get; }
        public bool PartialApproval { set; get; }
        public string TerminalID { set; get; }
        public string MerchantID { set; get; }
        public decimal Balance { set; get; }
        public string Terms { set; get; }
        public string Token { set; get; }
        public string Last4 { set; get; }
        public string EntryMethod { set; get; }

        public bool IsFSA { set; get; }

        public string SignatureString { set; get; }
        public RecieptResponse ResponseTags { set; get; }

        //private List<ResponseTag> taglist = new List<ResponseTag>();


        public TransactionResult()
        {
            logger.Debug("In Constructor TransactionResult()");
            this.Amount = 0.0m;
            this.Balance = 0.0m;
        }

        public void LoadResponse(string strResponseresult)
        {
            logger.Debug("Loading and Parsing Response String");
            try
            {
                using (StringReader reader = new StringReader(strResponseresult))
                {
                    string tag = string.Empty;
                    string message = string.Empty;
                    while ((tag = reader.ReadLine()) != null)
                    {
                        try
                        {
                            using (ResponseTag oresponsetag = new ResponseTag(tag, "="))
                            {
                                switch (oresponsetag.Tag)
                                {
                                    case ResponseStrings.RESULT:
                                        message += tag + "  ";
                                        this.Result = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.ACCOUNT:
                                        this.Account = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.AMOUNT:
                                        message += tag + "  ";
                                        this.Amount = GlobalHelpers.StringToDecimal(oresponsetag.Value);
                                        break;
                                    case ResponseStrings.EXPIRATION:
                                        this.Expiration = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.APPROVAL_CODE:
                                        message += tag + "  ";
                                        this.ApprovalCode = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.NAME:
                                        this.Name = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.TRANSACTION_ID:
                                        message += tag + "  ";
                                        this.TransactionID = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.ORDER_ID:
                                        message += tag + "  ";
                                        this.OrderID = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.TRANS_DETAIL:
                                        this.TransDetail = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.ACCOUNT_TYPE:
                                        this.AccountType = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.PARTIAL_APPROVAL:
                                        message += tag + "  ";
                                        this.PartialApproval = GlobalHelpers.StringToBool(oresponsetag.Value);
                                        break;
                                    case ResponseStrings.TERM_ID:
                                        this.TerminalID = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.MERCHANT_ID:
                                        this.MerchantID = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.BALANCE:
                                        this.Balance = GlobalHelpers.StringToDecimal(oresponsetag.Value);
                                        break;
                                    case ResponseStrings.TERMS:
                                        this.Terms = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.TOKEN:
                                        this.Token = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.LAST4:
                                        this.Last4 = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.ENTRY_METHOD:
                                        this.EntryMethod = Helpers.GlobalHelpers.DecodeEntryMethod(oresponsetag.Value);
                                        break;
                                    case ResponseStrings.SIGNATURE:
                                        this.SignatureString = oresponsetag.Value;
                                        break;
                                    case ResponseStrings.RECEIPT_GROUP_1:
                                        if (this.ResponseTags == null)
                                        {
                                            this.ResponseTags = new RecieptResponse();
                                        }

                                        this.ResponseTags.LoadResponse(oresponsetag.Value);
                                        break;
                                    case ResponseStrings.RECEIPT_GROUP_2:
                                        if (this.ResponseTags == null)
                                        {
                                            this.ResponseTags = new RecieptResponse();
                                        }
                                        this.ResponseTags.LoadEmvResponse(oresponsetag.Value);
                                        break;

                                    default:
                                        break;

                                }
                            }
                        }
                        catch(Exception expn)
                        {
                            logger.Error(expn, "An Error Occured while processing the Transaction Response Tag " + tag + ": " + expn.Message);
                        }
                        

                    }

                    logger.Trace(message);
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An Error Occured while processing the Transaction Response " + ex.Message);
            }
           

        }

        

    }
}
