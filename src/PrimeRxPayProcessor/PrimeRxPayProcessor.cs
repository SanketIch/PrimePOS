using Newtonsoft.Json;
using NLog;
using PrimeRxPay.RequestModels;
using PrimeRxPay.ResponseModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Windows.Forms;

namespace PrimeRxPay
{
    public class PrimeRxPayProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        private static PrimeRxPayProcessor _PrimeRxPayProcessor = null;
        private PrimeRxPayResponse _PrimeRxPayResponse = null;

        private PrimeRxPayProcessor()
        {

        }

        public static PrimeRxPayProcessor GetInstance()
        {
            if (_PrimeRxPayProcessor == null)
            {
                _PrimeRxPayProcessor = new PrimeRxPayProcessor();
            }
            return _PrimeRxPayProcessor;
        }
        public PrimeRxPayResponse Sale(Dictionary<string, string> fields)
        {
            _PrimeRxPayResponse = new PrimeRxPayResponse();
            try
            {
                logger.Debug("Entered in Sale Method");
                SaleRequest saleRequest = new SaleRequest();
                saleRequest.Amount = fields["AMOUNT"];
                saleRequest.TransDate = DateTime.Now.ToString();//PRIMEPOS-3131
                if (fields.ContainsKey("IIASRXAMOUNT"))
                {
                    saleRequest.HealthcareAmount = fields["IIASRXAMOUNT"];
                }
                else if (fields.ContainsKey("IIASTRANSACTION") && fields["IIASTRANSACTION"].ToString().ToLower() == "true")
                {
                    saleRequest.HealthcareAmount = fields["IIASAUTHORIZEDAMOUNT"];
                }
                else
                {
                    saleRequest.HealthcareAmount = "0";
                }
                if (fields.ContainsKey("LINKEXPIRY"))//PRIMEPOS-3134
                    saleRequest.LinkExpiryInMinutes = fields["LINKEXPIRY"];
                else
                    saleRequest.LinkExpiryInMinutes = "120";
                saleRequest.TransactionProcessingMode = "0";
                saleRequest.PharmacyNpi = fields["PHARMACYNO"];
                saleRequest.ApplicationId = "2";
                saleRequest.PaymentProviderId = Convert.ToInt32(fields["PAYPROVIDERID"]);
                if (fields.ContainsKey("FSACARD"))
                {
                    if (Convert.ToBoolean(fields["FSACARD"]))
                        saleRequest.PaymentCardType = "0";
                    else
                        saleRequest.PaymentCardType = "1";
                }
                else
                    saleRequest.PaymentCardType = "1";

                saleRequest.ClientID = fields["APIKEY"];
                saleRequest.SecretKey = fields["PASSWORD"];
                saleRequest.URL = fields["URL"];
                string saleUrl = string.Concat(fields["URL"], Constant.SaleUrl);
                string saleClientID = fields["APIKEY"].ToString();
                string saleSecreteKey = fields["PASSWORD"].ToString();

                bool isTokenized = false;
                if (fields.ContainsKey("TOKENREQUEST"))
                {
                    isTokenized = Convert.ToBoolean(fields["TOKENREQUEST"]);
                }
                if (isTokenized)
                    saleRequest.TransactionSetupMethod = "2";
                else if (fields.ContainsKey("ONLYTOKEN"))//PRIMEPOS-2896 Arvind
                    saleRequest.TransactionSetupMethod = "1";
                else
                    saleRequest.TransactionSetupMethod = "0";

                #region PRIMEPOS-3455
                bool isSecureDevice = false;
                if (fields.ContainsKey("ISSECUREDEVICE"))
                {
                    isSecureDevice = Convert.ToBoolean(fields["ISSECUREDEVICE"]);
                }
                if(isSecureDevice)
                {
                    saleRequest.IsSecuredDevice = true;
                    if (fields.ContainsKey("TERMINALMODEL"))
                    {
                        saleRequest.TerminalModel = Convert.ToString(fields["TERMINALMODEL"]);
                    }
                    else
                    {
                        saleRequest.TerminalModel = "";
                    }
                    if (fields.ContainsKey("TERMINALSRNUMBER"))
                    {
                        saleRequest.TerminalSrNumber = Convert.ToString(fields["TERMINALSRNUMBER"]);
                    }
                    else
                    {
                        saleRequest.TerminalSrNumber = "";
                    }
                }
                #endregion
                string request = JsonConvert.SerializeObject(saleRequest);
                string actualResponse = Constant.SendRequestPost(request, saleUrl, saleClientID, saleSecreteKey);

                SaleResponse saleResponse = JsonConvert.DeserializeObject<SaleResponse>(actualResponse);
                string getApiRespone = string.Empty;

                if (saleResponse?.Code?.Trim() == "0")
                {
                    string Url = saleResponse.PaymentTransactionUrl + HttpUtility.UrlEncode(saleResponse.TransactionSetupId);
                    getApiRespone = OpenCardDetailsFormUI(Url, Convert.ToString(saleResponse.TransId), saleRequest.URL, saleClientID, saleSecreteKey, fields["PAYPROVIDERID"], Convert.ToString(saleResponse.TransactionSetupId) , fields["StationID"]); //PRIMEPOS-3540
                }
                else if (saleResponse != null && saleResponse.Message != null && !string.IsNullOrWhiteSpace(saleResponse.Message))
                {
                    MessageBox.Show(saleResponse.Message);
                }
                request = request.Replace(saleClientID, "*****");
                request = request.Replace(saleSecreteKey, "*****");
                _PrimeRxPayResponse.request = request;
                _PrimeRxPayResponse.response = actualResponse + "\n" + getApiRespone;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Sale Method : " + ex.ToString());
                throw ex;
            }
            return _PrimeRxPayResponse;
        }
        public PrimeRxPayResponse Void(Dictionary<string, string> fields)
        {
            _PrimeRxPayResponse = new PrimeRxPayResponse();

            try
            {
                logger.Debug("Entered in Void Method");
                VoidRequest voidRequest = new VoidRequest();
                voidRequest.PharmacyNpi = fields["PHARMACYNO"];
                voidRequest.PaymentProviderId = fields["PAYPROVIDERID"];
                if (fields.ContainsKey("ISVOIDCDRIVEN") && Convert.ToBoolean(fields["ISVOIDCDRIVEN"]))
                {
                    if (fields.ContainsKey("TRANSACTIONID"))
                        voidRequest.PrimerxPayTransId = Convert.ToInt32(fields["TRANSACTIONID"]);
                }
                else if (fields.ContainsKey("ISVOIDPAYCOMP") && Convert.ToBoolean(fields["ISVOIDPAYCOMP"]))
                {
                    if (fields.ContainsKey("TRANSACTIONID"))
                        voidRequest.PaymentProviderTransId = fields["TRANSACTIONID"];
                }
                else if (fields.ContainsKey("TRANSACTIONID"))
                {
                    voidRequest.PaymentProviderTransId = fields["TRANSACTIONID"];
                }

                voidRequest.ApplicationId = "2";
                string voidUrl = string.Concat(fields["URL"], Constant.VoidUrl);
                string voidClientID = fields["APIKEY"].ToString();
                string voidSecreteKey = fields["PASSWORD"].ToString();

                string request = JsonConvert.SerializeObject(voidRequest);

                string response = Constant.SendRequestPost(request, voidUrl, voidClientID, voidSecreteKey);

                _PrimeRxPayResponse.ParseVoidResponse(response);

                request = request.Replace(voidClientID, "*****");
                request = request.Replace(voidSecreteKey, "*****");
                _PrimeRxPayResponse.request = request;
                _PrimeRxPayResponse.response = response;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Void Method : " + ex.ToString());
                throw ex;
            }

            return _PrimeRxPayResponse;
        }
        public PrimeRxPayResponse Reversal(Dictionary<string, string> fields)
        {
            _PrimeRxPayResponse = new PrimeRxPayResponse();

            try
            {
                logger.Debug("Entered in Reversal Method");
                VoidRequest voidRequest = new VoidRequest();
                voidRequest.PharmacyNpi = fields["PHARMACYNO"];
                voidRequest.PaymentProviderId = fields["PAYPROVIDERID"];
                if (fields.ContainsKey("TRANSACTIONID"))
                {
                    voidRequest.PaymentProviderTransId = fields["TRANSACTIONID"];
                }
                voidRequest.ApplicationId = "2";
                string reversalUrl = string.Concat(fields["URL"], Constant.ReversalUrl);
                string reversalClientID = fields["APIKEY"].ToString();
                string reversalSecreteKey = fields["PASSWORD"].ToString();

                string request = JsonConvert.SerializeObject(voidRequest);

                string response = Constant.SendRequestPost(request, reversalUrl, reversalClientID, reversalSecreteKey);

                _PrimeRxPayResponse.ParseVoidResponse(response);

                request = request.Replace(reversalClientID, "*****");
                request = request.Replace(reversalSecreteKey, "*****");
                _PrimeRxPayResponse.request = request;
                _PrimeRxPayResponse.response = response;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Reversal Method : " + ex.ToString());
                throw ex;
            }

            return _PrimeRxPayResponse;
        }
        public PrimeRxPayResponse Return(Dictionary<string, string> fields)
        {
            _PrimeRxPayResponse = new PrimeRxPayResponse();

            try
            {
                logger.Debug("Entered in Return Method");

                VoidRequest voidRequest = new VoidRequest();
                voidRequest.PharmacyNpi = fields["PHARMACYNO"];
                voidRequest.PaymentProviderId = fields["PAYPROVIDERID"];
                if (fields.ContainsKey("TRANSACTIONID"))
                    voidRequest.PaymentProviderTransId = fields["TRANSACTIONID"];
                if (fields["AMOUNT"].Contains("-"))
                {
                    fields["AMOUNT"] = fields["AMOUNT"].Remove(0, 1);
                }
                voidRequest.Amount = fields["AMOUNT"];
                voidRequest.ApplicationId = "2";
                string returnUrl = string.Concat(fields["URL"], Constant.ReturnUrl);
                string returnClientID = fields["APIKEY"].ToString();
                string returnSecreteKey = fields["PASSWORD"].ToString();
                string request = JsonConvert.SerializeObject(voidRequest);

                string response = Constant.SendRequestPost(request, returnUrl, returnClientID, returnSecreteKey);

                _PrimeRxPayResponse.ParseReturnResponse(response);
                request = request.Replace(returnClientID, "*****");
                request = request.Replace(returnSecreteKey, "*****");
                _PrimeRxPayResponse.request = request;
                _PrimeRxPayResponse.response = response;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Return Method : " + ex.ToString());
                throw ex;
            }

            return _PrimeRxPayResponse;
        }
        private string OpenCardDetailsFormUI(string Url, string TransId, string PrimeRxPayURL, string ClientId, string SecretKey, string PayProviderID, string TransactionSetupID, string stationID) //PRIMEPOS-3540
        {
            string response = string.Empty;
            try
            {
                //CardDetails cardDetails = new CardDetails(Url, TransId, PrimeRxPayURL, ClientId, SecretKey, PayProviderID, TransactionSetupID);
                HostedPayView cardDetails = new HostedPayView(Url, TransId, PrimeRxPayURL, ClientId, SecretKey, PayProviderID, TransactionSetupID, stationID);//PRIMEPOS-3178 //PRIMEPOS-3540
                cardDetails.ShowDialog();
                _PrimeRxPayResponse.ParseSaleResponse(cardDetails.getTransactionResponse);
                response = cardDetails.getTransactionResponse;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw ex;
            }
            return response;
        }
        //PRIMEPOS-TOKENSALE
        public PrimeRxPayResponse TokenSale(Dictionary<string, string> fields)
        {
            _PrimeRxPayResponse = new PrimeRxPayResponse();

            try
            {
                logger.Debug("Entered in TokenSale Method");

                CreditCardSale creditCardSale = new CreditCardSale();

                creditCardSale.Amount = fields["AMOUNT"];
                creditCardSale.TransDate = DateTime.Now.ToString();//PRIMEPOS-3131
                if (fields.ContainsKey("IIASRXAMOUNT"))
                {
                    creditCardSale.HealthcareAmount = fields["IIASRXAMOUNT"];
                }
                else if (fields.ContainsKey("IIASTRANSACTION") && fields["IIASTRANSACTION"].ToString().ToLower() == "true")
                {
                    creditCardSale.HealthcareAmount = fields["IIASAUTHORIZEDAMOUNT"];
                }
                else
                {
                    creditCardSale.HealthcareAmount = "0";
                }
                if (fields.ContainsKey("LINKEXPIRY"))//PRIMEPOS-3134
                    creditCardSale.LinkExpiryInMinutes = fields["LINKEXPIRY"];
                else
                    creditCardSale.LinkExpiryInMinutes = "120";
                creditCardSale.TransactionProcessingMode = "0";
                creditCardSale.PharmacyNPI = fields["PHARMACYNO"];
                creditCardSale.ApplicationID = "2";
                creditCardSale.PaymentProviderID = Convert.ToInt32(fields["PAYPROVIDERID"]);
                creditCardSale.ReferenceNumber = fields["TICKETNUMBER"];
                creditCardSale.TicketNumber = fields["TICKETNUMBER"];
                if (fields["TOKEN"].Contains("|"))
                {
                    creditCardSale.PaymentAccountID = fields["TOKEN"].Split('|')[0];
                    if (fields["TOKEN"].Split('|')[1].Length < 2)//PRIMEPOS-2902
                        creditCardSale.HSAFSACard = fields["TOKEN"].Split('|')[1];
                }
                else
                {
                    creditCardSale.PaymentAccountID = fields["TOKEN"];
                }
                //creditCardSale.ExternalID = "123";

                string creditUrl = string.Concat(fields["URL"], Constant.CreditCardSaleUrl);
                string creditClientID = fields["APIKEY"].ToString();
                string voidSecreteKey = fields["PASSWORD"].ToString();

                string request = JsonConvert.SerializeObject(creditCardSale);

                string response = Constant.SendRequestPost(request, creditUrl, creditClientID, voidSecreteKey);

                _PrimeRxPayResponse.ParseTokenSaleResponse(response);

                request = request.Replace(creditClientID, "*****");
                request = request.Replace(voidSecreteKey, "*****");
                _PrimeRxPayResponse.request = request;
                _PrimeRxPayResponse.response = response;
            }
            catch (Exception ex)
            {
                logger.Error("Error in TokenSale Method : " + ex.ToString());
                throw ex;
            }

            return _PrimeRxPayResponse;
        }
        //PRIMEPOS-2915
        public PrimeRxPayResponse CustomerSale(Dictionary<string, string> fields)
        {
            _PrimeRxPayResponse = new PrimeRxPayResponse();
            try
            {
                logger.Debug("Entered in Sale Method");
                CustomerSaleRequest saleRequest = new CustomerSaleRequest();
                saleRequest.Amount = fields["AMOUNT"];
                saleRequest.TransDate = DateTime.Now.ToString();//PRIMEPOS-3131
                if (fields.ContainsKey("IIASRXAMOUNT"))
                {
                    saleRequest.HealthcareAmount = fields["IIASRXAMOUNT"];
                }
                else if (fields.ContainsKey("IIASTRANSACTION") && fields["IIASTRANSACTION"].ToString().ToLower() == "true")
                {
                    saleRequest.HealthcareAmount = fields["IIASAUTHORIZEDAMOUNT"];
                }
                else
                {
                    saleRequest.HealthcareAmount = "0";
                }
                if (fields.ContainsKey("LINKEXPIRY"))
                    saleRequest.LinkExpiryInMinutes = Convert.ToInt32(fields["LINKEXPIRY"]);
                else
                    saleRequest.LinkExpiryInMinutes = 120;
                if (Convert.ToBoolean(fields["ISCUSTOMERDRIVEN"]))
                {
                    saleRequest.PatientFirstName = fields["CUSTOMERNAME"].Split(',')[1];
                    saleRequest.PatientLastName = fields["CUSTOMERNAME"].Split(',')[0];
                    if (fields.ContainsKey("EMAIL"))
                        saleRequest.PatientEmail = fields["EMAIL"];
                    saleRequest.PatientMobileNo = fields["MOBILE"];
                    //saleRequest.PatientDOB = "01/06/2010";//To-Do
                    saleRequest.PatientDOB = Convert.ToDateTime(fields["DOB"]).ToString("MM/dd/yyyy");
                    if (Convert.ToBoolean(fields["ISEMAIL"]))
                        saleRequest.TransactionProcessingMode = 2;
                    else
                        saleRequest.TransactionProcessingMode = 1;
                }
                else
                    saleRequest.TransactionProcessingMode = 0;
                saleRequest.PharmacyNPI = fields["PHARMACYNO"];
                saleRequest.ApplicationID = 2;
                saleRequest.PaymentProviderID = Convert.ToInt32(fields["PAYPROVIDERID"]);
                saleRequest.ReferenceNumber = fields["TICKETNUMBER"];
                if (fields.ContainsKey("FSACARD"))
                {
                    if (Convert.ToBoolean(fields["FSACARD"]))
                        saleRequest.PaymentCardType = 0;
                    else
                        saleRequest.PaymentCardType = 1;
                }
                else
                    saleRequest.PaymentCardType = 1;
                bool isTokenized = false;
                if (fields.ContainsKey("TOKENREQUEST"))
                {
                    isTokenized = Convert.ToBoolean(fields["TOKENREQUEST"]);
                }
                if (isTokenized)//2915 Check Customer driven and set to 0 
                    saleRequest.TransactionSetupMethod = "2";
                else if (Convert.ToDouble(fields["AMOUNT"]) > 0)
                    saleRequest.TransactionSetupMethod = "0";//To-DO constant
                else
                    saleRequest.TransactionSetupMethod = "1";

                string saleUrl = string.Concat(fields["URL"], Constant.SaleUrl);

                string creditClientID = fields["APIKEY"].ToString();
                string voidSecreteKey = fields["PASSWORD"].ToString();

                string request = JsonConvert.SerializeObject(saleRequest);
                string actualResponse = Constant.SendRequestPost(request, saleUrl, creditClientID, voidSecreteKey);

                SaleResponse saleResponse = JsonConvert.DeserializeObject<SaleResponse>(actualResponse);
                string getApiRespone = string.Empty;

                if (saleResponse.Code?.Trim() == "0" && saleResponse.TransId > 0)//2915 Check for transid and dont check for message
                {
                    logger.Trace("TRANSACTION HAS BEEN APPROVED" + saleResponse.Message + saleResponse.TransId);
                    _PrimeRxPayResponse.Result = "APPROVED";
                    _PrimeRxPayResponse.ResultDescription = saleResponse?.Message;
                    _PrimeRxPayResponse.TransactionNo = saleResponse.TransId.ToString();
                }
                else
                {
                    _PrimeRxPayResponse.Result = _PrimeRxPayResponse.ResultDescription = saleResponse.Message;
                }
                _PrimeRxPayResponse.request = request;
                _PrimeRxPayResponse.response = actualResponse + "\n" + getApiRespone;
            }
            catch (Exception ex)
            {
                logger.Error("Error in Sale Method : " + ex.ToString());
                throw ex;
            }
            return _PrimeRxPayResponse;
        }

        public PrimeRxPayResponse GetTransactionDetail(string Url, string transId, string payProviderID, string ClientID, string Secretkey, int lookUpDays) //PRIMEPOS-3453 Added LookUpDays
        {
            try
            {
                _PrimeRxPayResponse = new PrimeRxPayResponse();
                string response = string.Empty;
                System.Net.HttpStatusCode httpStatusCode = System.Net.HttpStatusCode.ServiceUnavailable;

                string url = string.Concat(Url, Constant.GetTransactionStatusUrl);

                var uriBuilder = new UriBuilder(url);
                var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                query["primerxPayTransId"] = transId;
                query["applicationId"] = "2";
                //query["transSetupID"] = this._transSetupId;
                query["paymentProviderId"] = payProviderID;
                uriBuilder.Query = query.ToString();

                string getTransactionURL = uriBuilder.ToString();
                response = Constant.SendRequestGet(getTransactionURL, out httpStatusCode, ClientID, Secretkey);

                _PrimeRxPayResponse.ParseSaleResponse(response);
                _PrimeRxPayResponse.request = getTransactionURL;
                _PrimeRxPayResponse.response = response;
                return _PrimeRxPayResponse;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return null;
            }
        }

        #region PRIMEPOS-3333
        public List<PrimeRxPayResponse> GetMultipleTransactionDetail(string Url, List<long> transId, int payProviderID, string ClientID, string Secretkey, int lookUpDays) //PRIMEPOS-3453 Added LookUpDays
        {
            try
            {
                List<PrimeRxPayResponse> primeRxPayResponses = new List<PrimeRxPayResponse>();
                List<GetTransactionDetail> getTransDetails = new List<GetTransactionDetail>();
                string response = string.Empty;
                string url = string.Concat(Url, Constant.GetMultipleTransactionStatusUrl);
                MultipleTransDetailRequest mtDetail = new MultipleTransDetailRequest();

                mtDetail.PrimerxPayTransId = transId;
                mtDetail.ApplicationId = 2;
                mtDetail.PaymentProviderID = payProviderID;
                mtDetail.LookUpDays = lookUpDays; //PRIMEPOS-3453

                string request = JsonConvert.SerializeObject(mtDetail);
                string actualResponse = Constant.SendRequestPost(request, url, ClientID, Secretkey);

                if (actualResponse == null)
                {
                    return primeRxPayResponses;
                }

                getTransDetails = JsonConvert.DeserializeObject<List<GetTransactionDetail>>(actualResponse);

                if (getTransDetails == null)
                {
                    return primeRxPayResponses;
                }

                foreach (var getTransactionDetail in getTransDetails)
                {
                    if (getTransactionDetail == null)
                    {
                        continue;
                    }
                    string temp = JsonConvert.SerializeObject(getTransactionDetail);
                    _PrimeRxPayResponse = new PrimeRxPayResponse();
                    _PrimeRxPayResponse.ParseSaleResponse(temp);
                    _PrimeRxPayResponse.request = request;
                    _PrimeRxPayResponse.response = response;
                    primeRxPayResponses.Add(_PrimeRxPayResponse);
                }
                return primeRxPayResponses;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"PrimeRxPayProcessor===>GetMultipleTransactionDetail()===>Exception occured while getting transaction detail.");
                return null;
            }
        }
        #endregion

        public string ResendLink(Dictionary<string, string> fields)
        {
            try
            {
                string response = string.Empty;

                ResendLinkReq resendLinkReq = new ResendLinkReq();
                if (fields.ContainsKey("LINKEXPIRY"))//PRIMEPOS-3134
                    resendLinkReq.LinkExpiryInMinutes = Convert.ToInt32(fields["LINKEXPIRY"]);
                else
                    resendLinkReq.LinkExpiryInMinutes = 120;
                resendLinkReq.PatientEmail = fields["Email"];
                resendLinkReq.PatientMobileNo = fields["Mobile"];
                resendLinkReq.PatientName = fields["Name"];
                resendLinkReq.TransactionProcessingMode = Convert.ToInt32(fields["TransactionProcessingMode"]);
                resendLinkReq.PrimerxPayTransId = Convert.ToInt32(fields["TRANSID"]);
                resendLinkReq.PharmacyNPI = fields["PHARMACYNO"];//PRIMEPOS-3134

                string creditClientID = fields["APIKEY"].ToString();
                string voidSecreteKey = fields["PASSWORD"].ToString();

                string request = JsonConvert.SerializeObject(resendLinkReq);
                string actualResponse = Constant.SendRequestPost(request, string.Concat(fields["URL"], Constant.ResendPayment), creditClientID, voidSecreteKey);

                return _PrimeRxPayResponse.ParseResendLinkResponse(actualResponse);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return null;
            }
        }
    }
}
