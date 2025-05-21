using MMS.PROCESSOR;
using Newtonsoft.Json;
using NLog;
using PrimeRxPay.ResponseModels;
using System;

namespace PrimeRxPay
{
    public class PrimeRxPayResponse : PaymentResponse
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        public string response;
        public string request;
        public override int ParseResponse(string xmlResponse, string FilterNode)
        {
            throw new NotImplementedException();
        }
        public void ParseSaleResponse(string response)
        {
            try
            {
                var transactionDetail = JsonConvert.DeserializeObject<GetTransactionDetail>(response);
                if (transactionDetail != null)
                {
                    if (transactionDetail.PayProviderResponseCode?.Trim() == "0" || transactionDetail.PayProviderResponseCode?.Trim() == "16")
                    {
                        base.AmountApproved = transactionDetail.ApprovedAmounut;
                        base.AccountNo = transactionDetail.LastFour;
                        base.CardType = transactionDetail.CardLogo;
                        base.TransactionNo = transactionDetail.PayProviderTransId;
                        base.MaskedCardNo = transactionDetail.LastFour;
                        base.TransactionAmount = transactionDetail.AmountDue; //PRIMEPOS-3344
                        base.TransactionID = transactionDetail.TransactionId; //PRIMEPOS-3333
                        base.PayProviderMessage = transactionDetail.PayProviderResponseMessage; //PRIMEPOS-3343
                        base.Result = transactionDetail.Message.ToUpper();
                        base.EmvReceipt = new EmvReceiptTags();
                        base.EmvReceipt.TransactionID = transactionDetail.PayProviderTransId;
                        base.EmvReceipt.TransID = transactionDetail.PayProviderTransId;
                        base.EmvReceipt.AccountType = transactionDetail.CardLogo;
                        if (!string.IsNullOrWhiteSpace(transactionDetail.PaymentAccountId))
                            base.ProfiledID = transactionDetail.PaymentAccountId + "|" + transactionDetail.HSAFSACard;
                        base.AuthNo = base.EmvReceipt.ApprovalCode = transactionDetail.ApprovalNumber;//3006
                        logger.Trace(transactionDetail.ExpirationDate);
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(transactionDetail.ExpirationDate)) && transactionDetail.ExpirationDate?.Year != 1900)
                            base.Expiration = transactionDetail.ExpirationDate?.ToString("MMyy");//PRIMEPOS-3140
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(transactionDetail.PayProviderResponseMessage))
                        {
                            base.Result = transactionDetail.PayProviderResponseMessage.ToUpper();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ParseVoidResponse(string response)
        {
            try
            {
                var voidResponse = JsonConvert.DeserializeObject<VoidResponse>(response);

                if (voidResponse != null)
                {
                    if (voidResponse?.Code?.Trim() == "0" && voidResponse?.Message?.ToUpper() == "SUCCESS")
                    {
                        if (voidResponse?.ExpressResponseCode?.ToUpper() == "0" || voidResponse?.ExpressResponseCode?.ToUpper() == "16" || voidResponse?.ExpressResponseCode?.ToUpper() == "5")
                        {
                            base.Result = base.ResultDescription = voidResponse.Message?.ToUpper();
                            base.TransactionNo = voidResponse.TransactionId;
                            base.CardType = voidResponse.CardLogo;
                            base.AccountNo = voidResponse.LastFour; //PRIMEPOS-3383
                        }
                    }
                    else if (voidResponse?.Code?.Trim() == "21")
                    {
                        base.Result = base.ResultDescription = "SUCCESS";
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(voidResponse?.Message, " Message ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ParseReturnResponse(string response)
        {
            try
            {
                var returnResponse = JsonConvert.DeserializeObject<VoidResponse>(response);

                if (returnResponse != null)
                {
                    if (returnResponse?.Code?.Trim() == "0" && returnResponse?.Message?.ToUpper() == "SUCCESS")
                    {
                        if (returnResponse.ExpressResponseCode?.ToUpper() == "0" || returnResponse.ExpressResponseCode?.ToUpper() == "16" || returnResponse.ExpressResponseCode?.ToUpper() == "5")
                        {
                            base.Result = base.ResultDescription = returnResponse.Message?.ToUpper();
                            base.TransactionNo = returnResponse.TransactionId;
                            base.CardType = returnResponse.CardLogo;
                            base.AmountApproved = returnResponse.ApprovedAmount;
                            base.EmvReceipt = new EmvReceiptTags();
                            base.EmvReceipt.ApprovedAmount = returnResponse.ApprovedAmount;
                            base.EmvReceipt.CardLogo = returnResponse.CardLogo;
                            base.EmvReceipt.TransactionID = returnResponse.TransactionId;
                            base.EmvReceipt.TransID = returnResponse.TransactionId;
                            base.AuthNo = base.EmvReceipt.ApprovalCode = returnResponse.ApprovalNumber;//3006
                            base.EmvReceipt.AccountType = returnResponse.CardLogo;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(returnResponse?.Message, " Message ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ParseTokenSaleResponse(string response)
        {
            try
            {
                var transactionDetail = JsonConvert.DeserializeObject<CreditCardSaleResponse>(response);
                if (transactionDetail != null)
                {
                    if ((transactionDetail.ExpressResponseCode?.Trim() == "0" || transactionDetail.ExpressResponseCode?.Trim() == "5" || transactionDetail.ExpressResponseCode?.Trim() == "16") && transactionDetail?.Message?.ToUpper() == "SUCCESS")
                    {
                        base.AmountApproved = transactionDetail.ApprovedAmount;
                        if (string.IsNullOrWhiteSpace(transactionDetail.LastFour))
                        {
                            base.AccountNo = "0004";
                            base.MaskedCardNo = "0004";
                        }
                        else
                        {
                            base.AccountNo = transactionDetail.LastFour;
                            base.MaskedCardNo = transactionDetail.LastFour;
                        }
                        base.CardType = transactionDetail.CardLogo;
                        base.TransactionNo = transactionDetail.TransactionID;
                        base.Result = transactionDetail.Message.ToUpper();
                        base.EmvReceipt = new EmvReceiptTags();
                        base.EmvReceipt.TransactionID = transactionDetail.TransactionID;
                        base.EmvReceipt.TransID = transactionDetail.TransactionID;
                        base.EmvReceipt.AccountType = transactionDetail.CardLogo;
                        base.AuthNo = base.EmvReceipt.ApprovalCode = transactionDetail.ApprovalNumber;//3006
                        //}
                        base.ProfiledID = transactionDetail.PaymentAccountID;
                    }
                    else
                        base.Result = base.ResultDescription = transactionDetail.ExpressResponseMessage;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ParseResendLinkResponse(string response)
        {
            try
            {
                var transactionDetail = JsonConvert.DeserializeObject<ResendLinkRes>(response);
                if (transactionDetail != null)
                {
                    return transactionDetail.Message;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
