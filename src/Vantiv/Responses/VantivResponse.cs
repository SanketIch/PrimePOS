using MMS.PROCESSOR;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using Vantiv.ResponseModels;

namespace Vantiv.Responses
{
    public class VantivResponse : PaymentResponse
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        public string deviceRequest = string.Empty;
        public string deviceResponse = string.Empty;
        public string SignatureData = string.Empty;
        public string buttonNumber;

        #region PRIMEPOS-2796
        public string ExpressResponseCode = string.Empty;
        public string HostResponseMessage = string.Empty;
        #endregion
        public override int ParseResponse(string xmlResponse, string FilterNode)
        {
            return 0;
        }

        public void ParseSaleResponse(string response)
        {
            logger.Trace("ENTERED IN THE PARSESALERESPONSE() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var saleResponse = JsonConvert.DeserializeObject<SaleResponse>(response);
                base.EmvReceipt = new EmvReceiptTags();
                if (saleResponse?.emv != null)
                {
                    base.EmvReceipt.AppIndentifer = saleResponse?.emv?.applicationIdentifier;
                    base.EmvReceipt.AppCrytogram = saleResponse?.emv?.cryptogram;
                    base.EmvReceipt.AppPreferedName = saleResponse?.emv?.applicationPreferredName == string.Empty ? saleResponse?.CardLogo : saleResponse?.emv?.applicationPreferredName;
                    base.EmvReceipt.ApplicationLabel = saleResponse?.emv?.applicationLabel;
                    base.EmvReceipt.MerchantID = saleResponse?.merchantId;
                    base.EmvReceipt.TerminalID = saleResponse?.TerminalId;
                    base.EmvReceipt.PinVerified = saleResponse.pinVerified;//PRIMEPOS-2793 ARVIND
                }
                CreditCardSaleResponse ccResponse;
                EBTSaleResponse ebtResponse;
                DebitCardSaleResponse dbResponse;
                if (saleResponse?.ProcessorResponse != null)
                {
                    //XElement xmlDocumentWithoutNs = XmlHelper.RemoveAllNamespaces(XElement.Parse(saleResponse.ProcessorResponse.ProcessorRawResponse));
                    //saleResponse.ProcessorResponse.ProcessorRawResponse = xmlDocumentWithoutNs.ToString();

                    if (saleResponse?.ProcessorResponse?.ProcessorRawResponse != null)
                    {
                        if (saleResponse.ProcessorResponse.ProcessorRawResponse.Contains("DebitCardSaleResponse"))
                        {
                            dbResponse = XmlHelper.Deserialize<DebitCardSaleResponse>(saleResponse.ProcessorResponse?.ProcessorRawResponse);//For XML part
                            base.AuthNo = base.EmvReceipt.ApprovalCode = dbResponse?.Response?.Transaction?.ApprovalNumber;//3006
                            base.EmvReceipt.ResponseCode = dbResponse?.Response?.HostResponseCode + "/" + dbResponse?.Response?.ExpressResponseMessage;
                            base.Result = dbResponse?.Response?.Transaction?.TransactionStatus;
                            base.EmvReceipt.ReferenceNumber = dbResponse?.Response?.Transaction?.ReferenceNumber;
                            base.EmvReceipt.IsFsaCard = saleResponse?.FsaCard.ToUpper() == "YES" ? true : false;  //PRIMEPOS-3545
                            base.NBSPayType = "DEBIT";
                            base.PaymentType = "DEBIT"; //PRIMEPOS-3526 //PRIMEPOS-3504
                        }
                        else if (saleResponse.ProcessorResponse.ProcessorRawResponse.Contains("CreditCardSaleResponse"))
                        {
                            ccResponse = XmlHelper.Deserialize<CreditCardSaleResponse>(saleResponse.ProcessorResponse?.ProcessorRawResponse);//For XML part
                            base.AuthNo = base.EmvReceipt.ApprovalCode = ccResponse?.Response?.Transaction?.ApprovalNumber;//3006
                            base.EmvReceipt.ResponseCode = ccResponse?.Response?.HostResponseCode + "/" + ccResponse?.Response?.ExpressResponseMessage;
                            base.Result = ccResponse?.Response?.Transaction?.TransactionStatus;
                            base.EmvReceipt.ReferenceNumber = ccResponse?.Response?.Transaction?.ReferenceNumber;
                            base.EmvReceipt.IsFsaCard = saleResponse?.FsaCard.ToUpper() == "YES" ? true : false;  //PRIMEPOS-3545
                            base.NBSPayType = "CREDIT";
                            base.PaymentType = "CREDIT"; //PRIMEPOS-3526 //PRIMEPOS-3504
                        }
                        else if (saleResponse.ProcessorResponse.ProcessorRawResponse.Contains("EBTSaleResponse"))
                        {
                            ebtResponse = XmlHelper.Deserialize<EBTSaleResponse>(saleResponse.ProcessorResponse?.ProcessorRawResponse);//For XML part
                            base.AuthNo = base.EmvReceipt.ApprovalCode = ebtResponse?.Response?.Transaction?.ApprovalNumber;//3006
                            base.Result = ebtResponse?.Response?.Transaction?.TransactionStatus;
                            base.EmvReceipt.ResponseCode = ebtResponse?.Response?.HostResponseCode + "/" + ebtResponse?.Response?.ExpressResponseMessage;
                            base.EmvReceipt.ReferenceNumber = ebtResponse?.Response?.Transaction?.ReferenceNumber;
                            base.EmvReceipt.IsFsaCard = saleResponse?.FsaCard.ToUpper() == "YES" ? true : false;  //PRIMEPOS-3545
                            base.PaymentType = "EBT"; //PRIMEPOS-3526 //PRIMEPOS-3504
                        }
                    }
                }
                base.EmvReceipt.TransactionID = saleResponse?.TransactionId;
                base.EmvReceipt.TransID = saleResponse?.TransactionId;
                base.EntryMethod = saleResponse?.EntryMode;
                base.EmvReceipt.EntryLegend = saleResponse?.EntryMode;
                #region PRIMEPOS-2793
                if (saleResponse != null && saleResponse.PaymentType != null && (saleResponse.PaymentType.ToLower() == "debit" || saleResponse.PaymentType.ToLower() == "ebt"))
                {
                    base.EmvReceipt.CardLogo = saleResponse?.CardLogo + " " + saleResponse?.PaymentType;
                }
                if (saleResponse != null && saleResponse.PaymentType != null && saleResponse.PaymentType.ToLower() == "debit" && base.EmvReceipt.PinVerified == false)
                {
                    base.EmvReceipt.PinVerified = saleResponse.pinVerified;
                }
                #endregion
                base.MaskedCardNo = saleResponse?.AccountNumber;
                if (saleResponse?.Signature != null && saleResponse?.Signature?.SignatureData != null)
                    base.SignatureString = Encoding.Default.GetString(saleResponse.Signature?.SignatureData);
                base.CardType = saleResponse?.CardLogo == null ? String.Empty : saleResponse?.CardLogo;
                base.EmvReceipt.AccountType = saleResponse?.CardLogo == null ? String.Empty : saleResponse?.CardLogo;
                base.IsFSATransaction = saleResponse?.FsaCard.ToUpper() == "YES" ? "T" : String.Empty;//PRIMEPOS-3545
                base.AccountNo = saleResponse?.AccountNumber;
                base.AmountApproved = saleResponse?.ApprovedAmount.ToString();
                base.ResultDescription = saleResponse?.StatusCode.ToUpper() == Constant.Approved ? Constant.Success : saleResponse?.StatusCode; //PRIMEPOS-3156
                if (String.IsNullOrWhiteSpace(base.Result))
                    base.Result = saleResponse?.StatusCode.ToUpper() == Constant.Approved ? Constant.Success : saleResponse?.StatusCode; //PRIMEPOS-3156
                //else //PRIMEPOS-3156
                //{
                //    base.Result = base.Result.Insert(0, "Transaction Status :");
                //}
                base.TransactionNo = saleResponse?.TransactionId;
                base.ticketNum = saleResponse?.TransactionId;
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN PARSESALERESPONSE METHOD : ", ex.ToString());
                throw ex;
            }
        }

        #region PRIMEPOS-3372
        public void ParsePreReadResponse(string response)
        {
            logger.Trace("VantivResponses==>ParsePreReadResponse()==> ENTERED IN THE ParsePreReadResponse() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var saleResponse = JsonConvert.DeserializeObject<SaleResponse>(response);
                base.PaymentType = saleResponse?.PaymentType; //PRIMEPOS-3526 //PRIMEPOS-3504
                base.AccountNo = saleResponse?.AccountNumber;
                base.BinValue = saleResponse?.BinValue;
                base.AccountHolderName = saleResponse?.CardHolderName;
                base.PreReadId = saleResponse?.preReadId;
                base.ResultDescription = saleResponse?.StatusCode.ToUpper() == Constant.Approved ? Constant.Success : saleResponse?.StatusCode; //PRIMEPOS-3156
                if (String.IsNullOrWhiteSpace(base.Result))
                    base.Result = saleResponse?.StatusCode.ToUpper() == Constant.Approved ? Constant.Success : saleResponse?.StatusCode; //PRIMEPOS-3156
            }
            catch (Exception ex)
            {
                logger.Error("VantivResponses==>ParsePreReadResponse()==> ERROR IN PARSESALERESPONSE METHOD : ", ex.ToString());
                throw ex;
            }
        }

        public void ParseCancelResponse(string response)
        {
            logger.Trace("VantivResponses==>ParseCancelResponse()==> ENTERED IN THE CancelResponse() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                string temp = response.Replace(@"\", "");
                var cancelResponse = JsonConvert.DeserializeObject<SaleResponse>(temp);

                base.ResultDescription = !cancelResponse.HasErrors ? Constant.Success : cancelResponse?.StatusCode;
                if (String.IsNullOrWhiteSpace(base.Result))
                    base.Result = !cancelResponse.HasErrors ? Constant.Success : cancelResponse?.StatusCode;
            }
            catch (Exception ex)
            {
                logger.Error("VantivResponses==>ParseCancelResponse()==> ERROR IN PARSECANCELRESPONSE METHOD : ", ex.ToString());
                throw ex;
            }
        }
        #endregion

        public void ParseRefundResponse(string response)
        {
            logger.Trace(" ENTERED IN THE PARSEREFUNDRESPONSE() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var refundResponse = JsonConvert.DeserializeObject<RefundResponse>(response);
                base.EmvReceipt = new EmvReceiptTags();

                if (refundResponse?.emv != null)
                {
                    base.EmvReceipt.AppIndentifer = refundResponse?.emv?.applicationIdentifier;
                    base.EmvReceipt.AppCrytogram = refundResponse?.emv?.cryptogram;
                    base.EmvReceipt.AppPreferedName = refundResponse?.emv?.applicationPreferredName == string.Empty ? refundResponse?.cardLogo : refundResponse?.emv?.applicationPreferredName;
                    base.EmvReceipt.ApplicationLabel = refundResponse?.emv?.applicationLabel;
                    base.EmvReceipt.MerchantID = refundResponse?.merchantId;
                    base.EmvReceipt.TerminalID = refundResponse?.terminalId;
                    base.EmvReceipt.PinVerified = refundResponse.pinVerified;//PRIMEPOS-2793 ARVIND
                }

                CreditCardCreditResponse ccResponse;
                EBTSaleResponse ebtResponse;
                DebitCardReturnResponse dbResponse;

                if (refundResponse?._processor?.ProcessorRawResponse != null)
                {
                    if (refundResponse._processor.ProcessorRawResponse.Contains("DebitCardReturnResponse")) //PRIMEPOS-3504
                    {
                        dbResponse = XmlHelper.Deserialize<DebitCardReturnResponse>(refundResponse?._processor?.ProcessorRawResponse);//For XML part
                        base.AuthNo = base.EmvReceipt.ApprovalCode = dbResponse?.Response?.Transaction?.ApprovalNumber;//3006
                        base.EmvReceipt.ResponseCode = dbResponse?.Response?.HostResponseCode + "/" + dbResponse?.Response?.ExpressResponseMessage;
                        base.Result = dbResponse?.Response?.Transaction?.TransactionStatus;
                        base.EmvReceipt.ReferenceNumber = dbResponse?.Response?.Transaction?.ReferenceNumber;
                        base.NBSPayType = "DEBIT";
                        base.PaymentType = "DEBIT"; //PRIMEPOS-3526 //PRIMEPOS-3504
                    }
                    else if (refundResponse._processor.ProcessorRawResponse.Contains("CreditCardCreditResponse"))
                    {
                        ccResponse = XmlHelper.Deserialize<CreditCardCreditResponse>(refundResponse._processor?.ProcessorRawResponse);//For XML part
                        base.AuthNo = base.EmvReceipt.ApprovalCode = ccResponse?.Response?.Transaction?.ApprovalNumber;//3006
                        base.EmvReceipt.ResponseCode = ccResponse?.Response?.HostResponseCode + "/" + ccResponse?.Response?.ExpressResponseMessage;
                        base.Result = ccResponse?.Response?.Transaction?.TransactionStatus;
                        base.EmvReceipt.ReferenceNumber = ccResponse?.Response?.Transaction?.ReferenceNumber;
                        base.NBSPayType = "CREDIT";
                        base.PaymentType = "CREDIT"; //PRIMEPOS-3526 //PRIMEPOS-3504
                    }
                    else if (refundResponse._processor.ProcessorRawResponse.Contains("EBTSaleResponse"))
                    {
                        ebtResponse = XmlHelper.Deserialize<EBTSaleResponse>(refundResponse._processor?.ProcessorRawResponse);//For XML part
                        base.AuthNo = base.EmvReceipt.ApprovalCode = ebtResponse?.Response?.Transaction?.ApprovalNumber;//3006
                        base.Result = ebtResponse?.Response?.Transaction?.TransactionStatus;
                        base.EmvReceipt.ResponseCode = ebtResponse?.Response?.HostResponseCode + "/" + ebtResponse?.Response?.ExpressResponseMessage;
                        base.EmvReceipt.ReferenceNumber = ebtResponse?.Response?.Transaction?.ReferenceNumber;
                    }
                }
                base.MaskedCardNo = refundResponse?.accountNumber;
                #region PRIMEPOS-2793
                if (refundResponse != null && refundResponse.paymentType != null && (refundResponse.paymentType.ToLower() == "debit" || refundResponse.paymentType.ToLower() == "ebt"))
                {
                    base.EmvReceipt.CardLogo = refundResponse?.cardLogo + " " + refundResponse?.paymentType;
                }
                if (refundResponse != null && refundResponse.paymentType != null && refundResponse.paymentType.ToLower() == "debit" && base.EmvReceipt.PinVerified == false)
                {
                    base.EmvReceipt.PinVerified = refundResponse.pinVerified;
                }
                #endregion
                base.EntryMethod = refundResponse?.entryMode;
                if (refundResponse?.signature != null && refundResponse?.signature?.SignatureData != null)
                    base.SignatureString = Encoding.Default.GetString(refundResponse?.signature?.SignatureData);//Get Signature to Payment Response
                base.CardType = refundResponse?.cardLogo;
                base.EmvReceipt.AccountType = refundResponse?.cardLogo;
                base.AccountNo = refundResponse?.accountNumber;
                base.AmountApproved = refundResponse?.totalAmount.ToString();
                base.ResultDescription = refundResponse?.statusCode?.ToUpper() == "APPROVED" ? "SUCCESS" : refundResponse?.statusCode;
                base.Result = refundResponse?.statusCode?.ToUpper() == "APPROVED" ? "SUCCESS" : refundResponse?.statusCode;
                base.TransactionNo = refundResponse?.transactionId;
                base.ticketNum = refundResponse?.transactionId;
                base.EmvReceipt.TransID = refundResponse?.transactionId;
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN PARSEREFUNDRESPONSE METHOD : ", ex.ToString());
                throw ex;
            }
        }
        public void ParseVoidResponse(string response)
        {
            logger.Trace("ENTERED IN THE PARSEVOIDRESPONSE METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var voidResponse = JsonConvert.DeserializeObject<VoidResponse>(response);
                base.EmvReceipt = new EmvReceiptTags();
                base.MaskedCardNo = voidResponse?.accountNumber;
                base.CardType = voidResponse?.cardLogo;
                base.EmvReceipt.AccountType = voidResponse?.cardLogo;
                base.AccountNo = voidResponse.accountNumber;
                //base.AmountApproved = saleResponse..ToString();
                base.ResultDescription = voidResponse.statusCode.ToUpper() == "APPROVED" ? "SUCCESS" : voidResponse.statusCode;
                base.Result = voidResponse.statusCode.ToUpper() == "APPROVED" ? "SUCCESS" : voidResponse.statusCode;
                base.TransactionNo = voidResponse.transactionId;
                base.ticketNum = voidResponse.transactionId;
                base.EmvReceipt.TransID = voidResponse.transactionId;
                base.AuthNo = base.EmvReceipt.ApprovalCode = voidResponse?.approvalNumber;//3006
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN PARSEVOIDRESPONSE METHOD : ", ex.ToString());
                throw ex;
            }
        }
        public void ParseEBTResponse(string response)
        {
            logger.Trace("ENTERED IN PARSEEBTRESPONSE METHOD");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var ebtResponse = JsonConvert.DeserializeObject<EBTResponse>(response);
                base.EmvReceipt = new EmvReceiptTags();
                base.MaskedCardNo = ebtResponse.accountNumber;
                //base.CardType = saleResponse.cardLogo;
                base.AccountNo = ebtResponse.accountNumber;
                base.AmountApproved = ebtResponse.totalAmount.ToString();
                base.ResultDescription = ebtResponse.statusCode.ToUpper() == "APPROVED" ? "SUCCESS" : ebtResponse.statusCode;
                base.Result = ebtResponse.statusCode.ToUpper() == "APPROVED" ? "SUCCESS" : ebtResponse.statusCode;
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN PARSEEBTRESPONSE METHOD : ", ex.ToString());
                throw ex;
            }
        }
        public void ParseSignatureResponse(string response)
        {
            logger.Trace("ENTERED IN THE PARSESALERESPONSE() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var signatureResponse = JsonConvert.DeserializeObject<SignatureResponse>(response);
                base.EmvReceipt = new EmvReceiptTags();
                if (signatureResponse?.signature?.SignatureData != null)
                    base.SignatureString = Encoding.Default.GetString(signatureResponse.signature.SignatureData);
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN PARSESALERESPONSE METHOD : ", ex.ToString());
                throw ex;
            }
        }

        public void ParsePaymentCreateResponse(string response)
        {
            logger.Trace("ENTERED IN THE ParsePaymentCreateResponse() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                //XElement xmlDocumentWithoutNs = XmlHelper.RemoveAllNamespaces(XElement.Parse(response));
                //response = xmlDocumentWithoutNs.ToString();

                var paymentResponse = XmlHelper.Deserialize<PaymentAccountCreateWithTransIDResponse>(response);
                if (paymentResponse?.Response?.response == null)
                {
                    base.ProfiledID = paymentResponse?.Response?.PaymentAccount?.PaymentAccountID;
                    if (paymentResponse?.Response?.Card != null)
                        base.Expiration = paymentResponse?.Response?.Card?.ExpirationMonth + paymentResponse?.Response?.Card?.ExpirationYear;
                }
                else
                {
                    base.ProfiledID = paymentResponse?.Response?.response?.PaymentAccount?.PaymentAccountID;
                    if (paymentResponse?.Response?.response?.Card != null)
                        base.Expiration = paymentResponse?.Response?.response?.Card?.ExpirationMonth + paymentResponse?.Response?.response?.Card?.ExpirationYear;
                }
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN ParsePaymentCreateResponse METHOD : ", ex.ToString());
                throw ex;
            }
        }
        public void ParseCreditCardResponse(string response)
        {
            logger.Trace("ENTERED IN THE ParsePaymentCreateResponse() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var creditResponse = XmlHelper.Deserialize<CreditCardSaleResponse>(response);

                base.EmvReceipt = new EmvReceiptTags();
                base.EmvReceipt.TransID = creditResponse?.Response?.Transaction?.TransactionID;
                base.Result = creditResponse?.Response?.Transaction?.TransactionStatus?.ToUpper() == "APPROVED" || creditResponse?.Response?.Transaction?.TransactionStatus?.ToUpper() == "PARTIALAPPROVED" ? "SUCCESS" : creditResponse?.Response?.Transaction?.TransactionStatus;
                base.ResultDescription = creditResponse?.Response?.Transaction?.TransactionStatus?.ToUpper() == "APPROVED" || creditResponse?.Response?.Transaction?.TransactionStatus?.ToUpper() == "PARTIALAPPROVED" ? "SUCCESS" : creditResponse?.Response?.Transaction?.TransactionStatus;
                base.EmvReceipt.ResponseCode = creditResponse?.Response?.HostResponseCode + "/" + creditResponse?.Response?.ExpressResponseMessage;//PRIMEPOS-2793
                base.CardType = creditResponse?.Response?.Card?.CardLogo;
                base.MaskedCardNo = creditResponse?.Response?.Card?.CardNumberMasked;
                base.Expiration = creditResponse?.Response?.Card?.ExpirationMonth + creditResponse?.Response?.Card?.ExpirationYear;
                base.TransactionNo = creditResponse?.Response?.Transaction?.TransactionID;
                base.AmountApproved = creditResponse?.Response?.Transaction?.ApprovedAmount;
                base.AuthNo = base.EmvReceipt.ApprovalCode = creditResponse?.Response?.Transaction?.ApprovalNumber;//3006
                base.ticketNum = creditResponse?.Response?.Transaction?.TransactionID;
                base.EmvReceipt.AccountType = creditResponse?.Response?.Card?.CardLogo;

                #region PRIMEPOS-2796
                this.ExpressResponseCode = creditResponse?.Response?.ExpressResponseCode;
                this.HostResponseMessage = creditResponse?.Response?.HostResponseMessage;
                #endregion
                #region PRIMEPOS-3081
                if (creditResponse?.Response?.Transaction?.NetworkTransactionID != null)
                    base.ProfiledID = creditResponse?.Response?.PaymentAccount?.PaymentAccountID + "|" + creditResponse?.Response?.Transaction?.NetworkTransactionID;
                else
                    base.ProfiledID = creditResponse?.Response?.PaymentAccount?.PaymentAccountID;
                #endregion

            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN ParsePaymentCreateResponse METHOD : ", ex.ToString());
                throw ex;
            }
        }
        public void ParseReturnResponse(string response)
        {
            logger.Trace(" ENTERED IN THE ParseReturnResponse() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var returnResponse = JsonConvert.DeserializeObject<StrictReturnResponse>(response);
                base.EmvReceipt = new EmvReceiptTags();
                base.MaskedCardNo = returnResponse?.accountNumber;
                //base.EntryMethod = returnResponse.entryMode;
                if (returnResponse?.signature != null && returnResponse.signature?.SignatureData != null)
                    base.SignatureString = Encoding.Default.GetString(returnResponse?.signature?.SignatureData);//Get Signature to Payment Response
                //base.CardType = returnResponse.cardLogo;
                base.AccountNo = returnResponse?.accountNumber;
                base.AmountApproved = returnResponse?.totalAmount.ToString();
                base.ResultDescription = returnResponse?.statusCode?.ToUpper() == "APPROVED" ? "SUCCESS" : returnResponse?.statusCode;
                base.Result = returnResponse?.statusCode?.ToUpper() == "APPROVED" ? "SUCCESS" : returnResponse?.statusCode;
                base.TransactionNo = returnResponse?.transactionId;
                base.ticketNum = returnResponse?.transactionId;
                base.EmvReceipt.TransID = returnResponse?.transactionId;
                base.AuthNo = base.EmvReceipt.ApprovalCode = returnResponse?.approvalNumber;
                base.EmvReceipt.AccountType = returnResponse?.cardLogo; //PRIMEPOS-3504
                //3006
                ////PRIMEPOS-3156 commented and added new code below
                //#region PRIMEPOS-3081
                //if (!string.IsNullOrWhiteSpace(returnResponse?._processor?.ProcessorRawResponse))
                //{
                //    var creditCardReturnResponse = XmlHelper.Deserialize<CreditCardReturnResponse>(returnResponse._processor.ProcessorRawResponse);
                //    if (creditCardReturnResponse != null)
                //    {
                //        if (!string.IsNullOrWhiteSpace(creditCardReturnResponse?.Response?.Card?.CardLogo))
                //        {
                //            base.CardType = creditCardReturnResponse?.Response?.Card?.CardLogo;
                //        }
                //    }
                //}
                //#endregion

                #region PRIMEPOS-3156
                if (returnResponse?._processor?.ProcessorRawResponse != null)
                {
                    if (returnResponse._processor.ProcessorRawResponse.ToLower().Contains("debitcardreturnresponse"))
                    {
                        var creditCardReturnResponse = XmlHelper.Deserialize<DebitCardCreditResponse>(returnResponse?._processor?.ProcessorRawResponse);//For XML part
                        base.CardType = creditCardReturnResponse?.Response?.Card?.CardLogo;
                        if (!string.IsNullOrWhiteSpace(creditCardReturnResponse?.Response?.ExpressResponseMessage))
                        {
                            this.ResultDescription += $", {creditCardReturnResponse?.Response?.ExpressResponseMessage?.ToLower()}";
                        }
                        base.NBSPayType = "DEBIT";  //PRIMEPOS-3427
                        base.PaymentType = "DEBIT"; //PRIMEPOS-3526 //PRIMEPOS-3504
                    }
                    else if (returnResponse._processor.ProcessorRawResponse.ToLower().Contains("creditcardreturnresponse"))
                    {
                        var debitCardReturnResponse = XmlHelper.Deserialize<CreditCardReturnResponse>(returnResponse._processor?.ProcessorRawResponse);//For XML part
                        base.CardType = debitCardReturnResponse?.Response?.Card?.CardLogo;
                        if (!string.IsNullOrWhiteSpace(debitCardReturnResponse?.Response?.ExpressResponseMessage))
                        {
                            this.ResultDescription += $", {debitCardReturnResponse?.Response?.ExpressResponseMessage?.ToLower()}";
                        }
                        base.NBSPayType = "CREDIT";  //PRIMEPOS-3427
                        base.PaymentType = "CREDIT"; //PRIMEPOS-3526 //PRIEMPOS-3504
                    }
                    else if (returnResponse._processor.ProcessorRawResponse.ToLower().Contains("ebtreversalresponse"))
                    {
                        var ebtReturnResponse = XmlHelper.Deserialize<EBTReversalResponse>(returnResponse._processor?.ProcessorRawResponse);//For XML part
                        base.CardType = ebtReturnResponse?.Response?.Card?.CardLogo;
                        if (!string.IsNullOrWhiteSpace(ebtReturnResponse?.Response?.ExpressResponseMessage))
                        {
                            this.ResultDescription += $", {ebtReturnResponse?.Response?.ExpressResponseMessage?.ToLower()}";
                        }
                        base.NBSPayType = "EBT";  //PRIMEPOS-3427
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN ParseReturnResponse METHOD : ", ex.ToString());
                throw ex;
            }
        }
        public void ParseReversalResponse(string response)
        {
            logger.Trace("ENTERED IN THE ParseReversalResponse METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var voidResponse = JsonConvert.DeserializeObject<ReversalResponse>(response);
                base.EmvReceipt = new EmvReceiptTags();
                base.MaskedCardNo = voidResponse.accountNumber;
                base.AccountNo = voidResponse.accountNumber;
                //base.AmountApproved = saleResponse..ToString();
                base.ResultDescription = voidResponse.statusCode.ToUpper() == "APPROVED" ? "SUCCESS" : voidResponse.statusCode;
                base.Result = voidResponse.statusCode.ToUpper() == "APPROVED" ? "SUCCESS" : voidResponse.statusCode;
                base.TransactionNo = voidResponse.transactionId;
                base.ticketNum = voidResponse.transactionId;
                base.EmvReceipt.TransID = voidResponse.transactionId;
                base.AuthNo = base.EmvReceipt.ApprovalCode = voidResponse?.approvalNumber;//3006
                #region PRIMEPOS-3081
                //if (!string.IsNullOrWhiteSpace(voidResponse?._processor?.ProcessorRawResponse))
                //{
                //    var creditCardReversalResponse = XmlHelper.Deserialize<CreditCardReversalResponse>(voidResponse._processor.ProcessorRawResponse);
                //    if (creditCardReversalResponse != null)
                //    {
                //        if (!string.IsNullOrWhiteSpace(creditCardReversalResponse?.Response?.Card?.CardLogo))
                //        {
                //            base.CardType = creditCardReversalResponse?.Response?.Card?.CardLogo;
                //        }
                //    }
                //}
                if (voidResponse.paymentType != null && voidResponse.paymentType.ToUpper() == "CREDIT") //PRIMEPOS-3283
                {
                    if (!string.IsNullOrWhiteSpace(voidResponse?._processor?.ProcessorRawResponse))
                    {
                        var creditCardReversalResponse = XmlHelper.Deserialize<CreditCardReversalResponse>(voidResponse._processor.ProcessorRawResponse);//PRIMEPOS-3283
                        //var creditCardReversalResponse = XmlHelper.Deserialize<DebitCardReversalResponse>(voidResponse._processor.ProcessorRawResponse);//PRIMEPOS-3283
                        if (creditCardReversalResponse != null)
                        {
                            if (!string.IsNullOrWhiteSpace(creditCardReversalResponse?.Response?.Card?.CardLogo))
                            {
                                base.CardType = creditCardReversalResponse?.Response?.Card?.CardLogo;
                            }
                        }
                    }
                }
                else if (voidResponse.paymentType != null && voidResponse.paymentType.ToUpper() == "DEBIT")
                {
                    if (!string.IsNullOrWhiteSpace(voidResponse?._processor?.ProcessorRawResponse))
                    {
                        /*var creditCardReversalResponse = XmlHelper.Deserialize<CreditCardReversalResponse>(voidResponse._processor.ProcessorRawResponse);//PRIMEPOS-3283*/
                        var debitCardReversalResponse = XmlHelper.Deserialize<DebitCardReversalResponse>(voidResponse._processor.ProcessorRawResponse);//PRIMEPOS-3283
                        if (debitCardReversalResponse != null)
                        {
                            if (!string.IsNullOrWhiteSpace(debitCardReversalResponse?.Response?.Card?.CardLogo))
                            {
                                base.CardType = debitCardReversalResponse?.Response?.Card?.CardLogo;
                            }
                        }
                    }
                }
                else if (voidResponse.paymentType != null && voidResponse.paymentType.ToUpper() == "EBT") //PRIMEPOS-3283
                {
                    if (!string.IsNullOrWhiteSpace(voidResponse?._processor?.ProcessorRawResponse))
                    {
                        var ebtCardReversalResponse = XmlHelper.Deserialize<EBTReversalResponse>(voidResponse._processor.ProcessorRawResponse);//PRIMEPOS-3283
                        if (ebtCardReversalResponse != null)
                        {
                            if (!string.IsNullOrWhiteSpace(ebtCardReversalResponse?.Response?.Card?.CardLogo))
                            {
                                base.CardType = ebtCardReversalResponse?.Response?.Card?.CardLogo;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN ParseReversalResponse METHOD : ", ex.ToString());
                throw ex;
            }
        }
        #region PRIMEPOS-2769
        public void ParseCreditCardReversalResponse(string response)
        {
            logger.Trace("ENTERED IN THE ParsePaymentCreateResponse() METHOD ");

            deviceResponse = response;//Saving the response for saving it in the CC_Transmission_Log

            try
            {
                var creditResponse = XmlHelper.Deserialize<CreditCardReversalResponse>(response);

                base.EmvReceipt = new EmvReceiptTags();
                base.EmvReceipt.TransID = creditResponse?.Response?.Transaction?.TransactionID;
                base.Result = creditResponse?.Response?.Transaction?.TransactionStatus?.ToUpper() == "APPROVED" ? "SUCCESS" : creditResponse?.Response?.Transaction?.TransactionStatus;
                base.ResultDescription = creditResponse?.Response?.Transaction?.TransactionStatus?.ToUpper() == "APPROVED" ? "SUCCESS" : creditResponse?.Response?.Transaction?.TransactionStatus;
                base.CardType = creditResponse?.Response?.Card?.CardLogo;
                base.MaskedCardNo = creditResponse?.Response?.Card?.CardNumberMasked;
                base.Expiration = creditResponse?.Response?.Card?.ExpirationMonth + creditResponse?.Response?.Card?.ExpirationYear;
                base.TransactionNo = creditResponse?.Response?.Transaction?.TransactionID;
                base.AmountApproved = creditResponse?.Response?.Transaction?.ApprovedAmount;
                base.ticketNum = creditResponse?.Response?.Transaction?.TransactionID;
                base.EmvReceipt.AccountType = creditResponse?.Response?.Card?.CardLogo;

                base.AuthNo = base.EmvReceipt.ApprovalCode = creditResponse?.Response?.Transaction?.ApprovalNumber;//3006

                #region PRIMEPOS-2796
                this.ExpressResponseCode = creditResponse?.Response?.ExpressResponseCode;
                this.HostResponseMessage = creditResponse?.Response?.HostResponseMessage;
                #endregion

                if (creditResponse?.Response?.Transaction?.NetworkTransactionID != null)
                    base.ProfiledID = creditResponse?.Response?.PaymentAccount?.PaymentAccountID + "|" + creditResponse?.Response?.Transaction?.NetworkTransactionID;
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN ParsePaymentCreateResponse METHOD : ", ex.ToString());
                throw ex;
            }
        }
        #endregion

        #region PRIMEPOS-3156
        public void ParseLocalDetailReportResponse(string response)
        {
            logger.Trace("ENTERED IN THE ParseLocalDetailReportResponse() METHOD ");

            deviceResponse = response;

            try
            {
                var localReportResponse = XmlHelper.Deserialize<TransactionQueryResponse>(response);
                TransactionQueryResponse transactionQueryResponse = new TransactionQueryResponse();

                List<Item> ReportingItemData = new List<Item>();

                base.EmvReceipt = new EmvReceiptTags();
                if (localReportResponse?.response != null)
                {
                    base.EmvReceipt.AppIndentifer = localReportResponse?.response?.ReportingData?.Items?.Item?.AcceptorID;
                    //base.EmvReceipt.AppCrytogram = localReportResponse?.response?.ReportingData?.Items?.Item?.emv?.cryptogram;
                    base.EmvReceipt.AppPreferedName = localReportResponse?.response?.ReportingData?.Items?.Item?.CardLogo;
                    //base.EmvReceipt.ApplicationLabel = localReportResponse?.response?.ReportingData?.Items?.Item?.applicationLabel;
                    //base.EmvReceipt.MerchantID = localReportResponse?.response?.ReportingData?.Items?.Item?.merchantId;
                    base.EmvReceipt.TerminalID = localReportResponse?.response?.ReportingData?.Items?.Item?.TerminalID;
                    //base.EmvReceipt.PinVerified = localReportResponse?.response?.ReportingData?.Items?.Item?.pinVerified; 
                }
                if (localReportResponse?.response?.ReportingData?.Items?.Item != null)
                {
                    if (localReportResponse.response.ReportingData.Items.Item.TransactionType.Contains("DebitCardSale"))
                    {
                        base.AuthNo = base.EmvReceipt.ApprovalCode = localReportResponse?.response?.ReportingData?.Items?.Item?.ApprovalNumber;
                        base.EmvReceipt.ResponseCode = localReportResponse?.response?.ReportingData?.Items?.Item?.HostResponseCode + "/" + localReportResponse?.response?.ReportingData?.Items?.Item?.ExpressResponseMessage;
                        base.Result = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionStatus;
                        base.EmvReceipt.ReferenceNumber = localReportResponse?.response?.ReportingData?.Items?.Item?.ReferenceNumber;
                        base.CardType = localReportResponse?.response?.ReportingData?.Items?.Item?.CardType;
                    }
                    else if (localReportResponse.response.ReportingData.Items.Item.TransactionType.Contains("CreditCardSale"))
                    {
                        //dbResponse = XmlHelper.Deserialize<DebitCardSaleResponse>(saleResponse.ProcessorResponse?.ProcessorRawResponse);//For XML part

                        base.AuthNo = base.EmvReceipt.ApprovalCode = localReportResponse?.response?.ReportingData?.Items?.Item?.ApprovalNumber;
                        base.EmvReceipt.ResponseCode = localReportResponse?.response?.ReportingData?.Items?.Item?.HostResponseCode + "/" + localReportResponse?.response?.ReportingData?.Items?.Item?.ExpressResponseMessage;
                        base.Result = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionStatus;
                        base.EmvReceipt.ReferenceNumber = localReportResponse?.response?.ReportingData?.Items?.Item?.ReferenceNumber;
                        base.CardType = localReportResponse?.response?.ReportingData?.Items?.Item?.CardType;
                    }
                    else if (localReportResponse.response.ReportingData.Items.Item.TransactionType.Contains("EBTSale"))
                    {
                        //ebtResponse = XmlHelper.Deserialize<EBTSaleResponse>(saleResponse.ProcessorResponse?.ProcessorRawResponse);//For XML part

                        base.AuthNo = base.EmvReceipt.ApprovalCode = localReportResponse?.response?.ReportingData?.Items?.Item?.ApprovalNumber;
                        base.EmvReceipt.ResponseCode = localReportResponse?.response?.ReportingData?.Items?.Item?.HostResponseCode + "/" + localReportResponse?.response?.ReportingData?.Items?.Item?.ExpressResponseMessage;
                        base.Result = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionStatus;
                        base.EmvReceipt.ReferenceNumber = localReportResponse?.response?.ReportingData?.Items?.Item?.ReferenceNumber;
                        base.CardType = localReportResponse?.response?.ReportingData?.Items?.Item?.CardType;
                    }
                }
                else
                {
                    base.Result = localReportResponse?.response?.ExpressResponseCode;
                    base.ResultDescription = localReportResponse?.response?.ExpressResponseMessage;
                }
                base.EmvReceipt.TransactionID = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionID;
                base.EmvReceipt.TransID = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionID;

                //this two fields not found in the response
                //base.EntryMethod = saleResponse?.EntryMode;
                //base.EmvReceipt.EntryLegend = saleResponse?.EntryMode;

                #region PRIMEPOS-2793
                //if (localReportResponse != null && localReportResponse?.response?.ReportingData?.Items?.Item? != null && (localReportResponse?.response?.ReportingData?.Items?.Item?.typ.ToLower() == "debit" || saleResponse.PaymentType.ToLower() == "ebt"))
                //{
                //    base.EmvReceipt.CardLogo = saleResponse?.CardLogo + " " + saleResponse?.PaymentType;
                //}

                //if (saleResponse != null && saleResponse.PaymentType != null && saleResponse.PaymentType.ToLower() == "debit" && base.EmvReceipt.PinVerified == false)
                //{
                //    base.EmvReceipt.PinVerified = saleResponse.pinVerified;
                //}
                #endregion


                base.MaskedCardNo = localReportResponse?.response?.ReportingData?.Items?.Item?.CardNumberMasked;//check it with amit


                base.CardType = localReportResponse?.response?.ReportingData?.Items?.Item?.CardLogo == null ? String.Empty : localReportResponse?.response?.ReportingData?.Items?.Item?.CardLogo;

                //this field is not present
                //base.EmvReceipt.AccountType = localReportResponse?.response?.ReportingData?.Items?.Item?.ac == null ? String.Empty : saleResponse?.CardLogo;

                //base.AccountNo = localReportResponse?.response?.ReportingData?.Items?.Item?.AccountID;

                base.AmountApproved = localReportResponse?.response?.ReportingData?.Items?.Item?.ApprovedAmount?.ToString();

                if (String.IsNullOrWhiteSpace(base.ResultDescription))
                    base.ResultDescription = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionStatusCode.ToUpper() == Constant.Approved ? Constant.Success : localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionStatusCode;

                if (String.IsNullOrWhiteSpace(base.Result))
                    //base.Result = saleResponse?.StatusCode.ToUpper() == Constant.Approved ? Constant.Success : "Status Code :" + saleResponse?.StatusCode;//PRIMEPOS-3156
                    base.Result = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionStatusCode.ToUpper() == Constant.Approved ? Constant.Success : localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionStatusCode;
                //else
                //{
                //    base.Result = base.Result.Insert(0, "Transaction Status :");
                //}
                base.TransactionNo = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionID;
                base.ticketNum = localReportResponse?.response?.ReportingData?.Items?.Item?.TransactionID;


            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN ParseLocalDetailReportResponse METHOD : ", ex.ToString());
                throw ex;
            }
        }
        #endregion PRIMEPOS-3156 till here

    }
}
