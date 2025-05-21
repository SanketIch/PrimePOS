using MMS.GlobalPayments.Api.CommonData;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.PaymentMethods;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMS.GlobalPayments.Api.Terminals.PAX
{
    public class PaxInterface : DeviceInterface<PaxController>, IDeviceInterface
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static int DEFAULT_TIMEOUT = 300000; // NileshJ Disconnection Timeout Issue - 3-July-2019        
        public static int Default_ReconnectWaitTime = 250; //PRIMEPOS-3087  Default 250 MiliSecond
        internal PaxInterface(PaxController controller) : base(controller)
        {
        }

        #region Administration Messages
        public override PaxTerminalResponse Initialize()//NG SDKUPDATE 20/9/2022
        {

            byte[] response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A00_INITIALIZE));

            var res = new InitializeResponse(response);
            logger.Info("Response = " + res);
            return res;
        }

        public override ISignatureResponse GetSignatureFile()
        {
            var response = _controller.Send(TerminalUtilities.BuildRequest(
                PAX_MSG_ID.A08_GET_SIGNATURE,
                0,
                ControlCodes.FS
            ));
            return new SignatureResponse(response, _controller.DeviceType.Value);
        }

        public override PaxTerminalResponse Cancel() //NG SDKUPDATE 20/9/2022
        {
            PaxTerminalResponse deviceResponse = null;
            logger.Info("Cancel() -" + clsPOSDBConstants.Log_Entering);
            if (_controller.ConnectionMode == ConnectionModes.HTTP)
            {
                logger.Error("The cancel command is not available in HTTP mode");
                throw new MessageException("The cancel command is not available in HTTP mode");
            }

            try
            {
                _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A14_CANCEL));
                var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A14_CANCEL));
                deviceResponse = new PaxTerminalResponse(response, PAX_MSG_ID.A15_RSP_CANCEL);
                logger.Info("Cancel() - response=" + deviceResponse);

            }
            catch (MessageException exc)
            {
                if (!exc.Message.Equals("Terminal returned EOT for the current message."))
                {
                    logger.Error("Terminal returned EOT for the current message.");
                    throw;
                }
            }
            return deviceResponse;
        }

        public override PaxTerminalResponse Reset() //NG SDKUPDATE 20/9/2022
        {
            //logger.Trace("Reset() " + clsPOSDBConstants.Log_Entering);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A16_RESET));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A17_RSP_RESET);
            //logger.Info("Reset() - response=" + res);
            return res;
        }

        public override ISignatureResponse PromptForSignature(string transactionId = null)
        {
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A20_DO_SIGNATURE,
                (transactionId != null) ? 1 : 0,
                ControlCodes.FS,
                transactionId ?? string.Empty,
                ControlCodes.FS,
                (transactionId != null) ? "00" : "",
                ControlCodes.FS,
                300
            ));
            var signatureResponse = new SignatureResponse(response);
            if (signatureResponse.DeviceResponseCode == "000000")
                return GetSignatureFile();
            logger.Info("response= " + signatureResponse);
            return signatureResponse;
        }

        public override PaxTerminalResponse Reboot() //NG SDKUPDATE 20/9/2022
        {
            //logger.Trace("Reboot() -" + clsPOSDBConstants.Log_Entering);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A26_REBOOT));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A27_RSP_REBOOT);
            logger.Info("Reboot() - response=" + res);
            return res;
        }

        public override IDeviceResponse DisableHostResponseBeep()
        {
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A04_SET_VARIABLE,
                "01",
                ControlCodes.FS,
                "hostRspBeep",
                ControlCodes.FS,
                "N",
                ControlCodes.FS,
                ControlCodes.FS,
                ControlCodes.FS,
                ControlCodes.FS,
                ControlCodes.FS,
                ControlCodes.FS,
                ControlCodes.FS,
                ControlCodes.FS,
                ControlCodes.FS
            ));
            return new PaxTerminalResponse(response, PAX_MSG_ID.A05_RSP_SET_VARIABLE);
        }

        public override string SendCustomMessage(DeviceMessage message)
        {
            var response = _controller.Send(message);
            return Encoding.UTF8.GetString(response);
        }
        #endregion

        #region Reporting Messages
        //public override TerminalReportBuilder LocalDetailReport()
        //{
        //    return new TerminalReportBuilder(TerminalReportType.LocalDetailReport);
        //}
        #endregion

        #region Batch Commands
        public override IBatchCloseResponse BatchClose()
        {
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.B00_BATCH_CLOSE, DateTime.Now.ToString("YYYYMMDDhhmmss")));
            var res = new BatchCloseResponse(response);
            logger.Info("BatchClose - response=" + res);
            return res;

        }
        #endregion

        #region Credit Methods
        //public TerminalAuthBuilder CreditAuth(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.Auth, PaymentMethodType.Credit).WithAmount(amount);
        //}

        //public TerminalManageBuilder CreditCapture(decimal? amount = null) {
        //    return new TerminalManageBuilder(TransactionType.Capture, PaymentMethodType.Credit).WithAmount(amount);
        //}

        ////public TerminalManageBuilder CreditEdit(decimal? amount = null) {
        ////    return new TerminalManageBuilder(TransactionType.Edit).WithAmount(amount);
        ////}

        //public TerminalAuthBuilder CreditRefund(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.Refund, PaymentMethodType.Credit).WithAmount(amount);
        //}

        //public TerminalAuthBuilder CreditSale(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.Sale, PaymentMethodType.Credit).WithAmount(amount);
        //}

        //public TerminalAuthBuilder CreditVerify() {
        //    return new TerminalAuthBuilder(TransactionType.Verify, PaymentMethodType.Credit);
        //}

        //public TerminalManageBuilder CreditVoid() {
        //    return new TerminalManageBuilder(TransactionType.Void, PaymentMethodType.Credit);
        //}
        #endregion

        #region Debit Methods
        //public TerminalAuthBuilder DebitRefund(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.Refund, PaymentMethodType.Debit).WithAmount(amount);
        //}

        //public TerminalAuthBuilder DebitSale(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.Sale, PaymentMethodType.Debit).WithAmount(amount);
        //}
        #endregion

        #region EBT Methods
        //public TerminalAuthBuilder EbtBalance() {
        //    return new TerminalAuthBuilder(TransactionType.Balance, PaymentMethodType.EBT);
        //}

        //public TerminalAuthBuilder EbtPurchase(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.Sale, PaymentMethodType.EBT).WithAmount(amount);
        //}

        //public TerminalAuthBuilder EbtRefund(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.Refund, PaymentMethodType.EBT).WithAmount(amount);
        //}

        //public TerminalAuthBuilder EbtWithdrawl(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.BenefitWithdrawal, PaymentMethodType.EBT).WithAmount(amount);
        //}
        #endregion

        #region Gift Methods
        //public TerminalAuthBuilder GiftSale(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.Sale, PaymentMethodType.Gift)
        //        .WithAmount(amount)
        //        .WithCurrency(CurrencyType.CURRENCY);
        //}

        //public TerminalAuthBuilder GiftAddValue(decimal? amount = null) {
        //    return new TerminalAuthBuilder(TransactionType.AddValue, PaymentMethodType.Gift)                
        //        .WithCurrency(CurrencyType.CURRENCY)
        //        .WithAmount(amount);
        //}

        //public TerminalManageBuilder GiftVoid() {
        //    return new TerminalManageBuilder(TransactionType.Void, PaymentMethodType.Gift).WithCurrency(CurrencyType.CURRENCY);
        //}

        //public TerminalAuthBuilder GiftBalance() {
        //    return new TerminalAuthBuilder(TransactionType.Balance, PaymentMethodType.Gift).WithCurrency(CurrencyType.CURRENCY);
        //}
        #endregion


        //NG SDKUPDATE 20/9/2022
        #region Transaction Commands
        internal byte[] DoSend(string messageId, params object[] elements)
        {
            return _controller.Send(TerminalUtilities.BuildRequest(messageId, elements));
        }

        internal byte[] DoTransaction(string messageId, string txnType, params IRequestSubGroup[] subGroups)
        {
            //logger.Info("DoTransaction() : MessageID=" + messageId + "; TransactionType=" + txnType + "; SubGroup" + subGroups);
            logger.Info($"DoTransaction() : MessageID={messageId}; TransactionType={txnType }; SubGroup={subGroups}");
            var commands = new ArrayList() { txnType, ControlCodes.FS };
            if (subGroups.Length > 0)
            {
                commands.Add(subGroups[0]);
                for (int i = 1; i < subGroups.Length; i++)
                {
                    commands.Add(ControlCodes.FS);
                    commands.Add(subGroups[i]);
                }
            }

            var message = TerminalUtilities.BuildRequest(messageId, commands.ToArray());
            //Console.WriteLine("PAX message " + message);
            //logger.Info("DoTransaction(): Message = " + message);
            logger.Info($"DoTransaction(): Message = {message}");
            byte[] response = _controller.Send(message);
            return response;
        }

        public CreditResponse DoCredit(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, AvsRequest avs,
            CashierSubGroup cashier, CommercialRequest commercial, EcomSubGroup ecom, ExtDataSubGroup extData)
        {
            //string test = "[STX]0[FS]T01[FS]1.40[FS]100001[FS]TIMEOUT[ETX]&";
            //test = test.Replace("[STX]", "~");
            //test = test.Replace('~', ((char)ControlCodes.STX));
            //test = test.Replace("[FS]", "~");
            //test = test.Replace('~', ((char)ControlCodes.FS));
            //test = test.Replace("[US]", "~");
            //test = test.Replace('~', ((char)ControlCodes.US));
            //test = test.Replace("[ETX]", "~");
            //test = test.Replace('~', ((char)ControlCodes.ETX));
            //byte[] vs = Encoding.UTF8.GetBytes(test ?? "");
            //var result =  new CreditResponse(vs);

            //return result;

            //logger.Info("PaxDevice.DoCredit() : TransactionType=" + txnType + ";Amount=" + amounts.TransactionAmount + ";Account=" + accounts + ";Trace=" + trace.ReferenceNumber + ";AvsRequest" + avs + ";CashierSubGroup=" + cashier + ";CommercialRequest=" + commercial + ";EcomSubGroup=" + ecom + ";ExtDataSubGroup=" + extData);
            logger.Info($"PaxDevice.DoCredit()=>Request: TransactionType={ txnType };AmountRequest={JsonConvert.SerializeObject(amounts)};" +
                $"AccountRequest={JsonConvert.SerializeObject(accounts)};" +
                $"TraceRequest={JsonConvert.SerializeObject(trace)};" +
                $"AvsRequest={JsonConvert.SerializeObject(avs)};" +
                $"CashierSubGroup={JsonConvert.SerializeObject(cashier)};" +
                $"CommercialRequest={JsonConvert.SerializeObject(commercial)};" +
                $"EcomSubGroup={JsonConvert.SerializeObject(ecom)};" +
                $"ExtDataSubGroup={JsonConvert.SerializeObject(extData)}");
            var response = DoTransaction(PAX_MSG_ID.T00_DO_CREDIT, txnType, amounts, accounts, trace, avs, cashier, commercial, ecom, extData);
            var res = new CreditResponse(response);
            logger.Info($"PaxDevice.DoCredit()=>Response:{ res}");
            return res;
        }
        public PaxTerminalResponse DoCreditToken(string Token, string Amount, Dictionary<string, string> Fields)
        {
            //if(Fields.ContainsKey)
            try
            {
                ServicesContainer.ConfigureService(new PorticoConfig
                {
                    SiteId = Convert.ToInt32(Fields["SITEID"]),
                    LicenseId = Convert.ToInt32(Fields["LICENSEID"]),
                    DeviceId = Convert.ToInt32(Fields["DEVICEID"]),
                    Username = Fields["USERNAME"],
                    Password = Fields["PASSWORD"]
                    //DeveloperId = "000000",
                    //VersionNumber = "0000"
                });
                AutoSubstantiation autoSubstantiation = new AutoSubstantiation();
                autoSubstantiation.PrescriptionSubTotal = Convert.ToDecimal(Amount);
                CreditCardData creditcardData = new CreditCardData(Token);
                Transaction response = creditcardData.Charge(Convert.ToDecimal(Amount)).WithCurrency("USD")
                    .WithAllowDuplicates(true)
                    .WithAutoSubstantiation(autoSubstantiation).Execute();

                PaxTerminalResponse terminalRes = new PaxTerminalResponse(new byte[1], "");
                System.IO.Stream stream = new System.IO.MemoryStream();
                System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
                terminalRes.AccountResponse = new AccountResponse(br);
                terminalRes.AmountResponse = new AmountResponse(br);
                terminalRes.TraceResponse = new TraceResponse(br);
                terminalRes.HostResponse = new HostResponse(br);
                terminalRes.CommercialResponse = new CommercialResponse(br);
                terminalRes.ExtDataResponse = new ExtDataSubGroup(br);
                terminalRes.EcomResponse = new EcomSubGroup(br);

                TerminalCardType cardtype;

                logger.Info(" Cardtype = : " + response.CardType);
                response.CardType = SetActualCardType(response.CardType);

                Enum.TryParse(response.CardType.ToUpper(), out cardtype);
                terminalRes.AccountResponse.CardType = cardtype;
                terminalRes.DeviceResponseText = response.ResponseMessage;
                if (response.ResponseMessage.ToUpper() == "APPROVAL")
                {
                    terminalRes.AccountResponse.AccountNumber = Fields["LASTFOUR"];
                    //res.AccountResponse.AccountNumber = response.;
                    terminalRes.MaskedCardNumber = Fields["LASTFOUR"];
                    terminalRes.DeviceResponseCode = "000000";
                    terminalRes.AmountResponse.ApprovedAmount = Convert.ToDecimal(Amount);
                }
                terminalRes.AuthorizationCode = response.AuthorizationCode;
                terminalRes.TraceResponse.TransactionNumber = response.TransactionId;
                terminalRes.TraceResponse.ReferenceNumber = response.ReferenceNumber;
                terminalRes.TerminalRefNumber = response.TransactionId;
                terminalRes.HostResponse.HostRefereceNumber = response.ReferenceNumber;
                terminalRes.ReferenceNumber = Fields["TICKETNO"];

                logger.Info($" DoCreditToken() Response: {JsonConvert.SerializeObject(terminalRes)}");
                return terminalRes;
            }
            catch (Exception ex)
            {
                //logger.Error("Error in DoCreditToken" + ex.Message);
                logger.Error($"Exception occured on PaxInterface=>DoCreditToken():{JsonConvert.SerializeObject(ex)}");
                return null;
            }
        }
        public PaxTerminalResponse DoVoidToken(Dictionary<string, string> Fields, string Transid)//2990
        {
            PaxTerminalResponse terminalRes = new PaxTerminalResponse(new byte[1], "");
            try
            {
                ServicesContainer.ConfigureService(new PorticoConfig
                {
                    SiteId = Convert.ToInt32(Fields["SITEID"]),
                    LicenseId = Convert.ToInt32(Fields["LICENSEID"]),
                    DeviceId = Convert.ToInt32(Fields["DEVICEID"]),
                    Username = Fields["USERNAME"],
                    Password = Fields["PASSWORD"]
                });

                var responsevoid = Transaction.FromId(Transid).Void().Execute();

                System.IO.Stream stream = new System.IO.MemoryStream();
                System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
                terminalRes.AccountResponse = new AccountResponse(br);
                terminalRes.AmountResponse = new AmountResponse(br);
                terminalRes.TraceResponse = new TraceResponse(br);
                terminalRes.HostResponse = new HostResponse(br);
                terminalRes.CommercialResponse = new CommercialResponse(br);
                terminalRes.ExtDataResponse = new ExtDataSubGroup(br);
                terminalRes.EcomResponse = new EcomSubGroup(br);

                if (responsevoid.ResponseMessage.ToUpper() == "SUCCESS")
                {
                    terminalRes.DeviceResponseCode = "000000";
                    terminalRes.TransactionType = "VOID";
                    //terminalRes.AmountResponse.ApprovedAmount = Convert.ToDecimal(Amount);
                }

                logger.Info($" DoVoidToken() Response: {JsonConvert.SerializeObject(terminalRes)}");
            }
            catch (Exception ex)
            {
                logger.Error($"Exception occured on PaxInterface=>DoVoidToken():{JsonConvert.SerializeObject(ex)}");
            }
            return terminalRes;
        }
        public PaxTerminalResponse DoReverseToken(Dictionary<string, string> Fields, string Transid)//2990
        {
            PaxTerminalResponse terminalRes = new PaxTerminalResponse(new byte[1], "");
            try
            {
                ServicesContainer.ConfigureService(new PorticoConfig
                {
                    SiteId = Convert.ToInt32(Fields["SITEID"]),
                    LicenseId = Convert.ToInt32(Fields["LICENSEID"]),
                    DeviceId = Convert.ToInt32(Fields["DEVICEID"]),
                    Username = Fields["USERNAME"],
                    Password = Fields["PASSWORD"]
                });

                var responsevoid = Transaction.FromId(Transid).Reverse().Execute();


                System.IO.Stream stream = new System.IO.MemoryStream();
                System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
                terminalRes.AccountResponse = new AccountResponse(br);
                terminalRes.AmountResponse = new AmountResponse(br);
                terminalRes.TraceResponse = new TraceResponse(br);
                terminalRes.HostResponse = new HostResponse(br);
                terminalRes.CommercialResponse = new CommercialResponse(br);
                terminalRes.ExtDataResponse = new ExtDataSubGroup(br);
                terminalRes.EcomResponse = new EcomSubGroup(br);

                TerminalCardType cardtype;

                logger.Info(" Cardtype = : " + responsevoid.CardType);
                responsevoid.CardType = SetActualCardType(responsevoid.CardType);

                Enum.TryParse(responsevoid.CardType.ToUpper(), out cardtype);
                terminalRes.AccountResponse.CardType = cardtype;
                terminalRes.DeviceResponseText = responsevoid.ResponseMessage;
                if (responsevoid.ResponseMessage.ToUpper() == "APPROVAL")
                {
                    terminalRes.AccountResponse.AccountNumber = Fields["LASTFOUR"];
                    terminalRes.MaskedCardNumber = Fields["LASTFOUR"];
                    terminalRes.DeviceResponseCode = "000000";
                    terminalRes.AmountResponse.ApprovedAmount = Convert.ToDecimal(Fields["AMOUNT"]);
                }
                terminalRes.AuthorizationCode = responsevoid.AuthorizationCode;
                terminalRes.TraceResponse.TransactionNumber = responsevoid.TransactionId;
                terminalRes.TraceResponse.ReferenceNumber = responsevoid.ReferenceNumber;
                terminalRes.TerminalRefNumber = responsevoid.TransactionId;
                terminalRes.HostResponse.HostRefereceNumber = responsevoid.ReferenceNumber;
                terminalRes.ReferenceNumber = responsevoid.ReferenceNumber;
                logger.Info($" DoReverseToken() Response: {JsonConvert.SerializeObject(terminalRes)}");
                return terminalRes;
            }
            catch (Exception ex)
            {
                logger.Error($"Exception occured on PaxInterface=>DoReverseToken():{JsonConvert.SerializeObject(ex)}");
            }
            return terminalRes;
        }

        public DebitResponse DoDebit(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData)
        {
            #region Custome Method for Debit LocalDetails Report Note DoNot Uncomment its for testing purpose
            //string test = "[STX]0[FS]R03[FS]1.44[FS]000000[FS]OK[FS]1[FS]0[FS]00[US]APPROVAL[US]145759[US]206826963337[US][US][US][FS]02[FS]01[FS][FS]11382[US]0[US]0[US]0[US]0[US]0[US][US][FS]1725[US]4[US]0522[US][US][US][US]01[US]LUCAS/JOE W               [US][US][US]0[FS]7[US]033619842[US]20220309115113[US][FS][US][FS][FS][FS]CARDBIN=471743[US]HRef=1122474233[US]SN=70050066[US]PINSTATUSNUM=1[US]GLOBALUID=70050066202203091151136342[US]TC=E836791A671C563B[US]TVR=8000048000[US]AID=A0000000980840[US]TSI=6800[US]ATC=0295[US]APPLAB=US DEBIT[US]APPPN=US DEBIT        [US]IAD=06010A03218000[US]ARC=Z3[US]CID=00[US]CVM=2[ETX]#";
            //test = test.Replace("[STX]", "~");
            //test = test.Replace('~', ((char)ControlCodes.STX));
            //test = test.Replace("[FS]", "~");
            //test = test.Replace('~', ((char)ControlCodes.FS));
            //test = test.Replace("[US]", "~");
            //test = test.Replace('~', ((char)ControlCodes.US));
            //test = test.Replace("[ETX]", "~");
            //test = test.Replace('~', ((char)ControlCodes.ETX));
            //byte[] vs = Encoding.UTF8.GetBytes(test ?? "");
            //var result = new PaxTerminalResponse(vs, PAX_MSG_ID.R03_RSP_LOCAL_DETAIL_REPORT);
            #endregion

            //logger.Info("DoDebit() : TransactionType=" + txnType + "; Amount=" + amounts + "; Account=" + accounts + "; Trace=" + trace + "; CashierSubGroup=" + cashier + "; ExtDataSubGroup=" + extData);
            logger.Info($"PaxDevice.DoDebit()=>Request: TransactionType={txnType};" +
                $"AmountRequest={JsonConvert.SerializeObject(amounts)};" +
                $"AccountRequest={JsonConvert.SerializeObject(accounts)};" +
                $"TraceRequest={JsonConvert.SerializeObject(trace)};" +
                $"CashierSubGroup={JsonConvert.SerializeObject(cashier)};" +
                $"ExtDataSubGroup={JsonConvert.SerializeObject(extData)}");
            var response = DoTransaction(PAX_MSG_ID.T02_DO_DEBIT, txnType, amounts, accounts, trace, cashier, extData);
            var res = new DebitResponse(response);

            logger.Info($"PaxDevice.DoDebit()=>Response: {res}");
            return res;
        }



        public EbtResponse DoEBT(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier)
        {
            //logger.Info("DoEBT() : TransactionType=" + txnType + "; Amount=" + amounts + "; Account=" + accounts + "; Trace=" + trace + "; CashierSubGroup=" + cashier);
            logger.Info($"PaxDevice.DoEBT()=>Request: TransactionType={ txnType };AmountRequest={JsonConvert.SerializeObject(amounts)};" +
                $"AccountRequest={JsonConvert.SerializeObject(accounts)};" +
                $"TraceRequest={JsonConvert.SerializeObject(trace)};" +
                $"CashierSubGroup={JsonConvert.SerializeObject(cashier)};");
            var response = DoTransaction(PAX_MSG_ID.T04_DO_EBT, txnType, amounts, accounts, trace, cashier, new ExtDataSubGroup());
            var res = new EbtResponse(response);
            logger.Info($"PaxDevice.DoEBT()=>Response:{ res}");
            return res;
        }

        public GiftResponse DoGift(string messageId, string txnType, AmountRequest amounts, AccountRequest accounts,
            TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData = null)
        {
            //logger.Info("DoGift() : MessageID=" + messageId + "TransactionType=" + txnType + "; Amount=" + amounts + "; Account=" + accounts + "; Trace=" + trace + "; CashierSubGroup=" + cashier + "; ExtDataSubGroup=" + extData);
            logger.Info($"PaxDevice.DoGift()=>Request: MessageID={messageId};" +
                $"TransactionType={ txnType };" +
                $"AmountRequest={JsonConvert.SerializeObject(amounts)};" +
                $"AccountRequest={JsonConvert.SerializeObject(accounts)};" +
                $"TraceRequest={JsonConvert.SerializeObject(trace)};" +
                $"CashierSubGroup={JsonConvert.SerializeObject(cashier)};" +
                $"ExtDataSubGroup={JsonConvert.SerializeObject(extData)}");
            var response = DoTransaction(messageId, txnType, amounts, accounts, trace, cashier, extData);
            var res = new GiftResponse(response);
            logger.Info($"DoGift()=>Response:{ res}");
            return res;
        }

        public CashResponse DoCash(string txnType, AmountRequest amounts, TraceRequest trace, CashierSubGroup cashier)
        {
            //logger.Info("DoCash() : TransactionType=" + txnType + "; Amount=" + amounts + "; Trace=" + trace + "; CashierSubGroup=" + cashier);
            logger.Info($"PaxDevice.DoCash()=>Request:" +
                $"TransactionType={ txnType };" +
                $"AmountRequest={JsonConvert.SerializeObject(amounts)};" +
                $"TraceRequest={JsonConvert.SerializeObject(trace)};" +
                $"CashierSubGroup={JsonConvert.SerializeObject(cashier)};");
            var response = DoTransaction(PAX_MSG_ID.T10_DO_CASH, txnType, amounts, trace, cashier, new ExtDataSubGroup());
            var res = new CashResponse(response);
            logger.Info($"DoCash()=>Response: { res}");
            return res;
        }

        public CheckSubResponse DoCheck(string txnType, AmountRequest amounts, CheckSubGroup check, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData)
        {
            //logger.Info("DoCheck() : TransactionType=" + txnType + "; Amount=" + amounts + "; CheckSubGroup=" + check + "; Trace=" + trace + "; CashierSubGroup=" + cashier + "; ExtDataSubGroup=" + extData);
            logger.Info($"PaxDevice.DoCheck()=>Request:" +
              $"TransactionType={ txnType };" +
              $"AmountRequest={JsonConvert.SerializeObject(amounts)};" +
              $"CheckSubGroup={JsonConvert.SerializeObject(check)};" +
              $"TraceRequest={JsonConvert.SerializeObject(trace)};" +
              $"CashierSubGroup={JsonConvert.SerializeObject(cashier)};" +
              $"ExtDataSubGroup={JsonConvert.SerializeObject(extData)}");
            var response = DoTransaction(PAX_MSG_ID.T12_DO_CHECK, txnType, amounts, check, trace, cashier, extData);
            var res = new CheckSubResponse(response);
            logger.Info($"DoCheck()=>Response:{ res}");
            return res;
        }
        #endregion

        #region Custome Method 
        public override void ShowItems(object[] dataList, string action, DeviceType deviceType)
        {
            logger.Trace("ShowItems() -" + clsPOSDBConstants.Log_Entering);
            byte[] response = null;
            string title;
            //PRIMEPOS - 2555  adjust coloumn header - NileshJ
            if (deviceType == DeviceType.PAX_D200)
            {
                //title = String.Format("{0,-32} {1,-12} {2,-15} {3,-15} {4,-10}", "Description", "Qty", "Unit-Price", "Discount", "Total");//Amit
                title = String.Format("{0,-32} {1,-12} {2,-18} {3,-18} {4,-35}", "Description", "Qty", "Unit-Price", "Discount", "Total");//Amit
            }
            else if (deviceType == DeviceType.PAX_A920)
            {
                title = String.Format("{0,-32} {1,-12} {2,-18} {3,-18} {4,-35}", "Description", "Qty", "Unit-Price", "Discount", "Total");
            }
            else
            {
                title = String.Format("{0,11} {1,6} {2,15} {3,6} {4,8}", "Description", "Qty", "Unit-Price", "Discount", "Total");//Amit
            }
            if (action == "A")
            {
                if (deviceType == DeviceType.PAX_D200)
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + dataList[dataList.Length - 1].ToString(), "\\B\\L" + title, "", "", "", "", "", ""));  //  N - Shows Items in line except //NileshJ - Remove Space Between Items - 20-Dec-2018
                }
                else if (deviceType == DeviceType.PAX_A920)
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + dataList[dataList.Length - 1].ToString(), "\\B\\L" + title, "", "", "", "", "", ""));  //  N - Shows Items in line except //NileshJ - Remove Space Between Items - 20-Dec-2018
                }
                else
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, @"\1" + dataList[dataList.Length - 1].ToString(), @"\1" + title, "", "N", "", "", "", ""));  //  N - Shows Items in line except //NileshJ - Remove Space Between Items - 20-Dec-2018
                }
                var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                logger.Trace("ShowItems() - Column Header=" + title + "; Response:" + res);
            }
            else
            {
                ClearMessageScreen();
                if (dataList != null)
                {
                    foreach (object item in dataList)
                    {
                        if (deviceType == DeviceType.PAX_D200)
                        {
                            response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + item.ToString(), "\\B\\L" + title, "", "", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order //NileshJ - Remove Space Between Items - 20-Dec-2018
                        }
                        if (deviceType == DeviceType.PAX_A920)
                        {
                            response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + item.ToString(), "\\B\\L" + title, "", "", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order //NileshJ - Remove Space Between Items - 20-Dec-2018
                        }
                        else
                        {
                            response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, @"\1" + item.ToString(), @"\1" + title, "", "N", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order //NileshJ - Remove Space Between Items - 20-Dec-2018
                        }
                        var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                        //Console.WriteLine("Show Item --------------------" + res);
                        logger.Trace($"ShowItems() - Response:{ res}");
                    }
                }
            }
        }
        //public override void ShowItems(object[] dataList, string action, DeviceType deviceType)
        //{
        //    logger.Trace("ShowItems() -" + clsPOSDBConstants.Log_Entering);
        //    string title = string.Empty;
        //    byte[] response = null;
        //    //PRIMEPOS - 2555  adjust coloumn header - NileshJ
        //    if (deviceType == DeviceType.PAX_D200)
        //    {
        //        //title = String.Format("{0,-32} {1,-12} {2,-15} {3,-15} {4,-10}", "Description", "Qty", "Unit-Price", "Discount", "Total");//Amit
        //        title = String.Format("{0,-32} {1,-12} {2,-18} {3,-18} {4,-12}", "Description", "Qty", "Unit-Price", "Discount", "Total");//Amit
        //        ClearMessageScreen();
        //        if (dataList != null)
        //        {
        //            foreach (object item in dataList.Reverse())
        //            {
        //                if (deviceType == DeviceType.PAX_D200)
        //                {
        //                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + item.ToString(), "\\B\\L" + title, "", "", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order //NileshJ - Remove Space Between Items - 20-Dec-2018
        //                }
        //                else
        //                {
        //                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, @"\1" + item.ToString(), @"\1" + title, "", "N", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order //NileshJ - Remove Space Between Items - 20-Dec-2018
        //                }
        //                var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
        //                Console.WriteLine("Show Item --------------------" + res);
        //                logger.Trace("ShowItems() - res=" + res);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        title = String.Format("{0,11} {1,6} {2,15} {3,6} {4,8}", "Description", "Qty", "Unit-Price", "Discount", "Total");//Amit

        //        if (action == "A")
        //        {
        //            //if (deviceType == DeviceType.PAX_D200)
        //            //{
        //            //    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + dataList[dataList.Length - 1].ToString(), "\\B\\L" + title, "", "", "", "", "", ""));  //  N - Shows Items in line except //NileshJ - Remove Space Between Items - 20-Dec-2018
        //            //}
        //            //else
        //            //{
        //            response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, @"\1" + dataList[dataList.Length - 1].ToString(), @"\1" + title, "", "N", "", "", "", ""));  //  N - Shows Items in line except //NileshJ - Remove Space Between Items - 20-Dec-2018
        //            //}
        //            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
        //            logger.Trace("ShowItems() - Column Header=" + title + "; Response=" + res);
        //        }
        //        else
        //        {
        //            ClearMessageScreen();
        //            if (dataList != null)
        //            {
        //                foreach (object item in dataList.Reverse())
        //                {
        //                    //if (deviceType == DeviceType.PAX_D200)
        //                    //{
        //                    //    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + item.ToString(), "\\B\\L" + title, "", "", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order //NileshJ - Remove Space Between Items - 20-Dec-2018
        //                    //}
        //                    //else
        //                    //{
        //                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, @"\1" + item.ToString(), @"\1" + title, "", "N", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order //NileshJ - Remove Space Between Items - 20-Dec-2018
        //                    //}
        //                    var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
        //                    Console.WriteLine("Show Item --------------------" + res);
        //                    logger.Trace("ShowItems() - res=" + res);
        //                }
        //            }
        //        }
        //    }
        //}

        public override void ShowItemsDisplay(object[] dataList, string action, DeviceType deviceType, int index)
        {
            //logger.Trace("ShowItemsDisplay() -" + clsPOSDBConstants.Log_Entering);
            string title = string.Empty;
            if (deviceType == DeviceType.PAX_A920)
            {
                //title = string.Format("{0,-24}{1,-14}{2,-18}", "Description", "Qty", "Total");
                title = string.Format("{0,-24}{1,-14}{2,-18}", "Description", "Qty", "Total");
            }
            else
            {
                title = string.Format(" {0,-32} {1,-12} {2,-18} {3,-18} {4,-45}", "Description", "Qty", "Unit-Price", "Discount", "Total");
            }
            byte[] response;
            if (action == "A")
            {
                if (deviceType == DeviceType.PAX_A920)
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\1\\L" + dataList[index].ToString(), "\\1\\B\\L" + title, "", "N", "", "", "", "", "0", index));
                }
                else
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + dataList[index].ToString(), "\\B\\L" + title, "", "N", "", "", "", "", "0", index));
                }
                var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                logger.Trace($"ShowItemsDisplay()=>Add - Header={title}; Response: {res}");
            }
            else if (action == "U")
            {
                if (deviceType == DeviceType.PAX_A920)
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\1" + dataList[index].ToString(), "\\1\\B\\L" + title, "", "N", "", "", "", "", "1", index));
                }
                else
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + dataList[index].ToString(), "\\B\\L" + title, "", "N", "", "", "", "", "1", index));
                }
                var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                logger.Trace($"ShowItemsDisplay()=>Update - Response:{ res}");
            }
            else if (action == "D")
            {
                System.Threading.Thread.Sleep(200);
                if (deviceType == DeviceType.PAX_A920)
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\1\\L" + dataList[index].ToString(), "\\1\\B\\L" + title, "", "N", "", "", "", "", "2", index));
                    System.Threading.Thread.Sleep(200);
                }
                else
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + dataList[index].ToString(), "", "", "N", "", "", "", "", "2", index));
                }
                var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                logger.Trace($"ShowItemsDisplay()=>Delete - Response:{res}");
            }
            else //For Hold Items
            {
                if (deviceType == DeviceType.PAX_A920)
                {
                    for (int i = 0; i < dataList.Length; i++)
                    {
                        response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\1\\L" + dataList[i].ToString(), "\\1\\B\\L" + title, "", "N", "", "", "", "", "", i));
                        var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                        logger.Trace($"ShowItemsDisplay()=>Hold Items - Response:{ res}");
                        System.Threading.Thread.Sleep(200);
                    }
                }
                else
                {
                    for (int i = 0; i < dataList.Length; i++)
                    {
                        response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, "\\L" + dataList[i].ToString(), "\\B\\L" + title, "", "N", "", "", "", "", "", i));
                        var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                        logger.Trace($"ShowItemsDisplay()=>Hold Items - Response:{ res}");
                    }
                }
            }
        }

        public override IDeviceResponse ClearMessageScreen()
        {
            //logger.Trace("ClearMessageScreen() -" + clsPOSDBConstants.Log_Entering);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A12_CLEAR_MESSAGE));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A13_RSP_CLEAR_MESSAGE);
            logger.Trace($"ClearMessageScreen() -Response:{ res}");
            return res;
        }

        // A24 - WELCOME
        public override PaxTerminalResponse ShowWelcomeMessage(string title, string displayMessage1, string displayMessage2, string timeout)
        {
            //logger.Trace("ShowWelcomeMessage() -" + clsPOSDBConstants.Log_Entering);
            logger.Trace("ShowWelcomeMessage() - Title=" + title + "; DisplayMessage1=" + displayMessage1 + "; DisplayMessage2=" + displayMessage2 + "; TimeOut=" + timeout);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A24_SHOW_MESSAGE_CENTER_ALIGNED, new string[] { title, displayMessage1, displayMessage2, timeout }));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A25_RSP_SHOW_MESSAGE_CENTER_ALIGNED);
            logger.Trace($"ShowWelcomeMessage() -Response:{ res}");
            return res;
        }

        // A24 - Thank you message
        public override PaxTerminalResponse ShowThanksMessage(string title, string displayMessage1, string displayMessage2, string timeout)
        {
            //logger.Trace("ShowThanksMessage() -" + clsPOSDBConstants.Log_Entering);
            logger.Trace("ShowThanksMessage() - Title=" + title + "; DisplayMessage1=" + displayMessage1 + "; DisplayMessage2=" + displayMessage2 + "; TimeOut=" + timeout);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A24_SHOW_MESSAGE_CENTER_ALIGNED, new string[] { title, displayMessage1, displayMessage2, timeout }));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A25_RSP_SHOW_MESSAGE_CENTER_ALIGNED);
            logger.Trace($"ShowThanksMessage() -Response: { response}");
            return res;
        }

        // A24 - Thank you message
        public override IDeviceResponse ShowMessageCenterAligned(string title, string displayMessage1, string timeout)
        {
            //logger.Trace("ShowMessageCenterAligned() -" + clsPOSDBConstants.Log_Entering);
            logger.Trace("ShowMessageCenterAligned() - Title=" + title + "; DisplayMessage1=" + displayMessage1 + "; TimeOut=" + timeout);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A24_SHOW_MESSAGE_CENTER_ALIGNED, new string[] { title, displayMessage1, "", timeout }));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A25_RSP_SHOW_MESSAGE_CENTER_ALIGNED);
            logger.Trace($"ShowMessageCenterAligned() -Response:{ res}");
            return res;
        }

        // A24 - SHOW DIALOG
        public override PaxTerminalResponse ShowDialog(string title, string button1, string button2, string button3, string button4)
        {
            //logger.Trace("ShowDialog() -" + clsPOSDBConstants.Log_Entering);
            logger.Trace("ShowDialog() - Title=" + title + "; Button1=" + button1 + "; Button2=" + button2 + "; Button3=" + button3 + "; Button4" + button4);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A06_SHOW_DIALOG, title, new string[] { button1, button2, button3, button4 }));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A07_RSP_SHOW_DIALOG);
            logger.Trace($"ShowDialog() -Response:{ res}");
            return res;
        }

        // PRIMEPOS - 2555  NILESHJ Added ShowDialogForm method for Radio Button
        // A68 - SHOW DIALOG Form - 
        public override PaxTerminalResponse ShowDialogForm(string title, string Label1, int Label1Property, string Label2, int Label2Property, string Label3, int Label3Property, string Label4, int Label4Property, int buttonType)
        {
            //logger.Trace("ShowDialogForm() -" + clsPOSDBConstants.Log_Entering);
            logger.Trace("ShowDialogForm() - Title=" + title + "; Label1=" + Label1 + "; Label1Property=" + Label1Property + "; Label2=" + Label2 + "; Label2Property" + Label2Property + "; Label3" + Label3 + "; Label3Property" + Label3Property + "; Label4" + Label4 + "; Label4Property" + Label4Property);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A68_SHOW_DIALOG_FORM, title, Label1, Label2Property, Label2, Label3Property, Label3, Label3Property, Label4, Label4Property, buttonType, ""));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A69_RSP_SHOW_DIALOG_FORM);
            logger.Trace($"ShowDialogForm() -Response:{ res}");
            return res;
        }

        // A24 - SHOW TEXTBOX
        public override PaxTerminalResponse ShowTextBox(string title, string message, string button1, string button1Color, string button2, string button2Color, string button3, string button3Color)
        {
            //logger.Trace("ShowTextBox() -" + clsPOSDBConstants.Log_Entering);
            logger.Trace("ShowTextBox() - Title=" + title + "; Message=" + message + "; Button1=" + button1 + "; button1Color" + button1Color + "; Button2=" + button2 + "; button2Color" + button2Color + "; Button3=" + button3 + "; button3Color" + button3Color);

            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A56_SHOW_TEXTBOX, title, message, button1, button1Color, button2, button2Color, button3, button3Color, "", "", "", "", "0", ""));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A57_RSP_SHOW_TEXTBOX);
            logger.Trace($"ShowTextBox() -Response:{ res}");
            return res;
        }

        // A10 - SHOW Message
        public override void ShowMessage(string title, string message)
        {
            //logger.Trace("ShowMessage() -" + clsPOSDBConstants.Log_Entering);
            logger.Trace("ShowMessage() -Title" + title + "|" + "Message" + message);
            ClearMessageScreen();
            var centerAlignedMessage = "\\B\\C" + message;
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, String.Format("{0,40}", " "), title, centerAlignedMessage, "", ""));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
            logger.Trace($"ShowMessage() -Response:{ res}");
        }
        // A24 - Thank you message
        public override void ShowTwoLineMessage(string title, string displayMessage1, string displayMessage2)
        {
            //logger.Trace("ShowThanksMessage() -" + clsPOSDBConstants.Log_Entering);
            logger.Trace("ShowTwoLineMessage() - Title=" + title + "; DisplayMessage1=" + displayMessage1 + "; DisplayMessage2=" + displayMessage2);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A24_SHOW_MESSAGE_CENTER_ALIGNED, new string[] { title, displayMessage1, displayMessage2, "60" }));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A25_RSP_SHOW_MESSAGE_CENTER_ALIGNED);
            logger.Trace($"ShowTwoLineMessage() -Response: { response}");

        }
        // A24 - DoSignature
        public override PaxTerminalResponse DoSignature()
        {
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A20_DO_SIGNATURE, "0", "", "01", null)); // PRIMEPOS - 2555 Change timeout to null - NileshJ
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A21_RSP_DO_SIGNATURE);
            logger.Trace($"DoSignature() -Response:{ res}");
            return res;
        }

        // A24 - GetSignature
        //public override PaxTerminalResponse GetSignature()
        //{
        //    var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A08_GET_SIGNATURE, "0", ""));
        //    var res = new PaxTerminalResponse(response, PAX_MSG_ID.A09_RSP_GET_SIGNATURE);
        //    logger.Trace("GetSignature() -response=" + res);
        //    return res;
        //}
        public override PaxTerminalResponse GetSignature()
        {
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A08_GET_SIGNATURE, 0, "99999"));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A09_RSP_GET_SIGNATURE);
            logger.Trace($"GetSignature() -Response:{ res}");
            return res;
        }

        public override IDeviceResponse ResetMSR()
        {
            logger.Info("ResetMSR() -" + clsPOSDBConstants.Log_Entering);
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A16_RESET));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.A17_RSP_RESET);
            logger.Info("ResetMSR() - Reponse : A16_RESET = " + response + "; Res : A17_RSP_RESET=" + res);
            //Console.WriteLine("--------------------" + res);
            //Console.WriteLine("--------------------");
            return null;
        }

        public override IDeviceResponse UpdateResourceImage(byte[] file)
        {
            //logger.Trace("UpdateResourceImage(byte[] file) -" + clsPOSDBConstants.Log_Entering);
            if (file != null && file.Length > 1)
            {
                string byteString = Encoding.UTF8.GetString(file, 0, file.Length);
                string base64File = Convert.ToBase64String(file);
                logger.Info("UpdateResourceImage(byte[] file) - byteString=" + byteString + ";base64File=" + base64File);
                byte[] response;
                IDeviceResponse res = null;
                var tempCheck = "";
                if (base64File.Length > 4000)
                {
                    double offset = 0;
                    var fileArray = Split(base64File, 4000);
                    for (int i = 0; i < fileArray.Count(); i++)
                    {
                        string segment = fileArray.ElementAt(i);
                        var segmentBytes = Convert.FromBase64String(segment);
                        offset = offset + segmentBytes.Length;
                        if (i == fileArray.Count() - 1)
                        {
                            response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, offset, segment, "0"));
                            res = new PaxTerminalResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                            Console.WriteLine(i + "--------------------" + res);
                            tempCheck += segment;
                        }
                        else
                        {
                            response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, offset, segment, "1"));
                            res = new PaxTerminalResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                            Console.WriteLine(i + "--------------------" + res);
                            tempCheck += segment;
                        }
                    }
                }
                else
                {
                    response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, "0", file, "0"));
                    res = new PaxTerminalResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                    Console.WriteLine("--------------------" + res);
                }

                var isIntegrity = tempCheck.Length == file.Length;
                //Console.WriteLine("integrity of string --------------------" + isIntegrity);
                logger.Info("UpdateResourceImage(byte[] file) : integrity of string : " + isIntegrity);
                logger.Info($"UpdateResourceImage() Response:{ res}");
                return res;
            }
            else
            {
                return null;
            }
        }

        public override PaxTerminalResponse UpdateResourceImageNew(byte[] file)
        {
            //logger.Trace("UpdateResourceImageNew(byte[] file) :  " + clsPOSDBConstants.Log_Entering);
            if (file != null && file.Length > 1)
            {
                byte[] response;
                PaxTerminalResponse res = null;
                int offset = 0;

                for (int i = 0; i < file.Length; i += 3000)
                {
                    if (file.Length - i >= 3000)
                    {
                        byte[] copiedByts = new byte[3000];
                        Array.Copy(file, i, copiedByts, 0, 3000);
                        string base64File = Convert.ToBase64String(copiedByts);
                        response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, offset, base64File, "1"));
                        res = new PaxTerminalResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                        Console.WriteLine(offset + "--------------------" + res);
                        offset += 3000;
                    }
                    else
                    {
                        byte[] copiedByts = new byte[file.Length - i];
                        Array.Copy(file, i, copiedByts, 0, file.Length - i);
                        string base64File = Convert.ToBase64String(copiedByts);
                        response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, offset, base64File, "0"));
                        res = new PaxTerminalResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                        Console.WriteLine(offset + "--------------------" + res);
                    }
                }
                return res;
            }
            else
            {
                return null;
            }
        }

        static IEnumerable<string> Split(string str, int chunkLength)
        {
            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }

        //Nilesh,Sajid add new method PRIMEPOS-2854
        public override string GetDeviceRequestMessage(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, AvsRequest avs, CashierSubGroup cashier, CommercialRequest commercial, EcomSubGroup ecom, ExtDataSubGroup extData)
        {
            IRequestSubGroup[] subGroups = { amounts, accounts, trace, avs, cashier, commercial, ecom, extData };
            logger.Info("DoTransaction() : MessageID= T00 " + ": TXN_TYPE_MAP[txnType] = " + txnType + "; SubGroup" + subGroups);

            var commands = new ArrayList() { txnType, ControlCodes.FS };
            if (subGroups.Length > 0)
            {
                commands.Add(subGroups[0]);
                for (int i = 1; i < subGroups.Length; i++)
                {
                    commands.Add(ControlCodes.FS);
                    commands.Add(subGroups[i]);
                }
            }

            var message = TerminalUtilities.BuildRequest("T00", commands.ToArray());
            Console.WriteLine("PAX message " + message);
            return message.ToString();
        }

        // SAJID LOCAL DETAILS REPORT PRIMEPOS-2862
        public override PaxTerminalResponse LocalDetailReport(string refNumber)
        {
            #region Custome Method for Debit LocalDetails Report Note DoNot Uncomment its for testing purpose
            //string test = "[STX]0[FS]R03[FS]1.44[FS]000000[FS]OK[FS]1[FS]0[FS]00[US]APPROVAL[US]145759[US]206826963337[US][US][US][FS]02[FS]01[FS][FS]11382[US]0[US]0[US]0[US]0[US]0[US][US][FS]1725[US]4[US]0522[US][US][US][US]01[US]LUCAS/JOE W               [US][US][US]0[FS]7[US]033619842[US]20220309115113[US][FS][US][FS][FS][FS]CARDBIN=471743[US]HRef=1122474233[US]SN=70050066[US]PINSTATUSNUM=1[US]GLOBALUID=70050066202203091151136342[US]TC=E836791A671C563B[US]TVR=8000048000[US]AID=A0000000980840[US]TSI=6800[US]ATC=0295[US]APPLAB=US DEBIT[US]APPPN=US DEBIT        [US]IAD=06010A03218000[US]ARC=Z3[US]CID=00[US]CVM=2[ETX]#";
            //test = test.Replace("[STX]", "~");
            //test = test.Replace('~', ((char)ControlCodes.STX));
            //test = test.Replace("[FS]", "~");
            //test = test.Replace('~', ((char)ControlCodes.FS));
            //test = test.Replace("[US]", "~");
            //test = test.Replace('~', ((char)ControlCodes.US));
            //test = test.Replace("[ETX]", "~");
            //test = test.Replace('~', ((char)ControlCodes.ETX));

            //byte[] vs = Encoding.UTF8.GetBytes(test ?? "");

            //var result = new PaxTerminalResponse(vs, PAX_MSG_ID.R03_RSP_LOCAL_DETAIL_REPORT);

            //return result;
            #endregion
            logger.Info($"LocalDetailReport() - RefNumber :{ refNumber } - {clsPOSDBConstants.Log_Entering}");
            var response = _controller.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.R02_LOCAL_DETAIL_REPORT, "00", "", "", "", "", "", refNumber, ""));
            var res = new PaxTerminalResponse(response, PAX_MSG_ID.R03_RSP_LOCAL_DETAIL_REPORT);
            logger.Info($"LocalDetailReport() -Response: {res}");
            return res;
        }
        public string SetActualCardType(string cardType)
        {
            string newCardType = string.Empty;
            //ADDED BY ARVIND TO HANDLE CARDTYPE PRIMEPOS-2636 
            if (cardType == null)
            {
                cardType = string.Empty;
            }
            //
            switch (cardType.ToUpper().Trim())
            {
                case "MASTERCARD":
                case "MASTER":
                case "MASTER CARD":
                case "MC":
                    newCardType = "MasterCard";
                    break;
                case "VISACARD":
                case "VISA":
                case "VISA CARD":
                    newCardType = "Visa";
                    break;
                case "AMERICANEXPRESSCARD":
                case "AMERICANEXPRESS":
                case "AMERICAN EXPRESS":
                case "AMEX":
                    newCardType = "Amex";
                    break;
                case "DISCOVERCARD":
                case "DISCOVER CARD":
                case "DISCOVER":
                case "DISC":
                    newCardType = "Discover";
                    break;
                //PRIMEPOS-3087
                default:
                    newCardType = cardType.ToUpper().Trim();
                    break;
            }
            return newCardType;
        }
        //public override void Dispose()
        //{
        //    _controller.Disconnect();
        //}
        #endregion
    }
}
