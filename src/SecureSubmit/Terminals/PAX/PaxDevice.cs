using System;
using System.Collections;
using Hps.Exchange.PosGateway.Client;
using SecureSubmit.Entities;
using SecureSubmit.Fluent;
using SecureSubmit.Fluent.Services;
using SecureSubmit.Infrastructure;
using SecureSubmit.Services;
using SecureSubmit.Terminals.Abstractions;
using SecureSubmit.Terminals.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using SecureSubmit.CommonData;

namespace SecureSubmit.Terminals.PAX {
    public delegate void MessageSentEventHandler(string message);

    public class PaxDevice : IDisposable {
        ITerminalConfiguration _settings;
        IDeviceCommInterface _interface;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public event MessageSentEventHandler OnMessageSent;
        public static int DEFAULT_TIMEOUT = 300000; // NileshJ Disconnection Timeout Issue - 3-July-2019
        public PaxDevice(ITerminalConfiguration settings) {
            settings.Validate();
            this._settings = settings;

            switch (_settings.ConnectionMode) {
                case ConnectionModes.TCP_IP:
                    logger.Debug("ConnectionModes.TCP_IP : ConnectionMode=" + _settings.ConnectionMode + "; IPAddress= " + _settings.IpAddress + "; Port = " + _settings.Port + "; Timeout=" + _settings.TimeOut);
                    _interface = new PaxTcpInterface(settings);
                    logger.Debug("ConnectionModes.TCP_IP -END");
                    break;
                case ConnectionModes.HTTP:
                    logger.Debug("PaxDevice.ConnectionModes.HTTP ; settings=" + settings);
                    _interface = new PaxHttpInterface(settings);
                    logger.Debug("PaxDevice.ConnectionModes.HTTP ; _interface=" + _interface);
                    break;
                case ConnectionModes.SERIAL:
                case ConnectionModes.SSL_TCP:
                    throw new NotImplementedException();
            }

            _interface.OnMessageSent += (message) => {
                if (this.OnMessageSent != null)
                    OnMessageSent(message);
            };
        }

        #region Administration Messages
        // A00 - INITIALIZE
        public InitializeResponse Initialize()
        {
            logger.Debug("Initialize() -" + clsPOSDBConstants.Log_Entering);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A00_INITIALIZE));
            var res = new InitializeResponse(response);
            logger.Debug("Initialize() - response=" + res);
            return res;
        }

        //// A14 - CANCEL
        //public void Cancel() {
        //    if (_settings.ConnectionMode == ConnectionModes.HTTP)
        //        throw new HpsMessageException("The cancel command is not available in HTTP mode");

        //    _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A14_CANCEL));
        //}

        // A14 - CANCEL -- Modified Suraj
        public PaxDeviceResponse Cancel()
        {
            logger.Debug("Cancel() -" + clsPOSDBConstants.Log_Entering);
            if (_settings.ConnectionMode == ConnectionModes.HTTP)
                throw new HpsMessageException("The cancel command is not available in HTTP mode");

            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A14_CANCEL));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A15_RSP_CANCEL);
            logger.Debug("Cancel() - response=" + res);
            return res;
        }

        // A16 - RESET
        public PaxDeviceResponse Reset()
        {
            logger.Debug("Reset() " + clsPOSDBConstants.Log_Entering);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A16_RESET));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A17_RSP_RESET);
            logger.Debug("Reset() - response=" + res);
            return res;
        }

        // A26 - REBOOT
        public PaxDeviceResponse Reboot()
        {
            logger.Debug("Reboot() -" + clsPOSDBConstants.Log_Entering);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A26_REBOOT));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A27_RSP_REBOOT);
            logger.Debug("Reboot() - response=" + res);
            return res;
        }

        // A30 - INPUT ACCOUNT
        public InputAccountBuilder InputAccount() {
            logger.Debug("InputAccount() -" + clsPOSDBConstants.Log_Entering);
            return new InputAccountBuilder(this);
        }


        // A24 - WELCOME
        public PaxDeviceResponse ShowWelcomeMessage(string title, string displayMessage1, string displayMessage2, string timeout)
        {
            logger.Debug("ShowWelcomeMessage() -" + clsPOSDBConstants.Log_Entering);
            logger.Debug("ShowWelcomeMessage() - Title=" + title + "; DisplayMessage1=" + displayMessage1 + "; DisplayMessage2=" + displayMessage2 + "; TimeOut=" + timeout);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A24_SHOW_MESSAGE_CENTER_ALIGNED, new string[] { title, displayMessage1, displayMessage2, timeout }));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A25_RSP_SHOW_MESSAGE_CENTER_ALIGNED);
            logger.Debug("ShowWelcomeMessage() -" + res);
            return res;
        }

        // A24 - Thank you message
        public PaxDeviceResponse ShowThanksMessage(string title, string displayMessage1, string displayMessage2, string timeout)
        {
            logger.Debug("ShowThanksMessage() -" + clsPOSDBConstants.Log_Entering);
            logger.Debug("ShowThanksMessage() - Title=" + title + "; DisplayMessage1=" + displayMessage1 + "; DisplayMessage2=" + displayMessage2 + "; TimeOut=" + timeout);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A24_SHOW_MESSAGE_CENTER_ALIGNED, new string[] { title, displayMessage1, displayMessage2, timeout }));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A25_RSP_SHOW_MESSAGE_CENTER_ALIGNED);
            logger.Debug("ShowThanksMessage() -" + response);
            return res;
        }

        // A24 - Thank you message
        public PaxDeviceResponse ShowMessageCenterAligned(string title, string displayMessage1, string timeout)
        {
            logger.Debug("ShowMessageCenterAligned() -" + clsPOSDBConstants.Log_Entering);
            logger.Debug("ShowMessageCenterAligned() - Title=" + title + "; DisplayMessage1=" + displayMessage1 + "; TimeOut=" + timeout);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A24_SHOW_MESSAGE_CENTER_ALIGNED, new string[] { title, displayMessage1, "", timeout }));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A25_RSP_SHOW_MESSAGE_CENTER_ALIGNED);
            logger.Debug("ShowMessageCenterAligned() -" + res);
            return res;
        }

        // A12- CLEAR MESSAGE
        public PaxDeviceResponse ClearMessageScreen()
        {
            logger.Debug("ClearMessageScreen() -" + clsPOSDBConstants.Log_Entering);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A12_CLEAR_MESSAGE));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A13_RSP_CLEAR_MESSAGE);
            logger.Debug("ClearMessageScreen() -Response=" + res);
            return res;
        }


        // A24 - SHOW DIALOG
        public PaxDeviceResponse ShowDialog(string title, string button1, string button2, string button3, string button4)
        {
            logger.Debug("ShowDialog() -" + clsPOSDBConstants.Log_Entering);
            logger.Debug("ShowDialog() - Title=" + title + "; Button1=" + button1 + "; Button2=" + button2 + "; Button3=" + button3 + "; Button4" + button4);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A06_SHOW_DIALOG, title, new string[] { button1, button2, button3, button4 }));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A07_RSP_SHOW_DIALOG);
            logger.Debug("ShowDialog() -" + res);
            return res;
        }

        // PRIMEPOS - 2555  NILESHJ Added ShowDialogForm method for Radio Button
        // A68 - SHOW DIALOG Form - 
        public PaxDeviceResponse ShowDialogForm(string title, string Label1, int Label1Property, string Label2, int Label2Property, string Label3, int Label3Property, string Label4, int Label4Property, int buttonType)
        {
            logger.Debug("ShowDialogForm() -" + clsPOSDBConstants.Log_Entering);
            logger.Debug("ShowDialogForm() - Title=" + title + "; Label1=" + Label1 + "; Label1Property=" + Label1Property + "; Label2=" + Label2 + "; Label2Property" + Label2Property + "; Label3" + Label3 + "; Label3Property" + Label3Property + "; Label4" + Label4 + "; Label4Property" + Label4Property);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A68_SHOW_DIALOG_Form, title, Label1, Label2Property, Label2, Label3Property, Label3, Label3Property, Label4, Label4Property, buttonType, ""));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A69_RSP_SHOW_DIALOG_Form);
            logger.Debug("ShowDialogForm() -" + res);
            return res;
        }

        // A24 - SHOW TEXTBOX
        public PaxDeviceResponse ShowTextBox(string title, string message, string button1, string button1Color, string button2, string button2Color, string button3, string button3Color)
        {
            logger.Debug("ShowTextBox() -" + clsPOSDBConstants.Log_Entering);
            logger.Debug("ShowTextBox() - Title=" + title + "; Message=" + message + "; Button1=" + button1 + "; button1Color" + button1Color + "; Button2=" + button2 + "; button2Color" + button2Color + "; Button3=" + button3 + "; button3Color" + button3Color);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A56_SHOW_TEXTBOX, title, message, button1, button1Color, button2, button2Color, button3, button3Color, "", "", "", "", "0", ""));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A57_RSP_SHOW_TEXTBOX);
            logger.Debug("ShowTextBox() -" + res);
            return res;
        }


        // A10 - SHOW Message
        public void ShowItems(object[] dataList,string action) {
            logger.Debug("ShowItems() -" + clsPOSDBConstants.Log_Entering);
            if (action == "A") {
                //PRIMEPOS - 2555  adjust coloumn header - NileshJ
                var title = String.Format("{0,0} {1,8} {2,15} {3,13} {4,6}", "Description", "Qty", "Unit-Price", "Discount", "Total");
                //var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, String.Format("{0,40}", " "), @"\1" + title, @"\1" + dataList[dataList.Length - 1].ToString(), "N", "", "", "", ""));  //  N - Shows Items in line except"", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order 
                var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, @"\1" + dataList[dataList.Length - 1].ToString(), @"\1" + title, "", "N", "", "", "", ""));  //  N - Shows Items in line except //NileshJ - Remove Space Between Items - 20-Dec-2018
                var res = new PaxDeviceResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                logger.Debug("ShowItems() - Column Header=" + title + "; Response=" + res);
            } else {
                ClearMessageScreen();
                //PRIMEPOS - 2555  adjust coloumn header - NileshJ
                var title = String.Format("{0,0} {1,8} {2,15} {3,13} {4,6}", "Description", "Qty", "Unit-Price", "Discount", "Total");
               // var titleResponseBytes = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, String.Format("{0,40}", " "), @"\1" + title, "", "N", "", "", "", ""));
               // var titleResponse = new PaxDeviceResponse(titleResponseBytes, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
               // Console.WriteLine("Show Item --------------------" + titleResponse);
               // logger.Debug("ShowItems() - TitleResponse=" + titleResponse);
                if (dataList != null)
                    foreach (object item in dataList) {

                        //var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, String.Format("{0,40}", " "), @"\1" + title, item.ToString(), "N", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order 
                        var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, @"\1" + item.ToString(), @"\1" + title, "", "N", "", "", "", ""));  // N - Shows Items in line except first added  Y- Shows items in reverse order //NileshJ - Remove Space Between Items - 20-Dec-2018
                        var res = new PaxDeviceResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
                        Console.WriteLine("Show Item --------------------" + res);
                        logger.Debug("ShowItems() - res=" + res);
                    }
            }
            
        }


        // A10 - SHOW Message
        public void ShowMessage(string title, string message) {
            logger.Debug("ShowMessage() -" + clsPOSDBConstants.Log_Entering);
            logger.Debug("ShowMessage() -Title" + title +"|"+"Message"+ message);
            ClearMessageScreen();
            var centerAlignedMessage = "\\B\\C" + message ;
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A10_SHOW_MESSAGE, String.Format("{0,40}", " "), title, centerAlignedMessage, "", ""));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A11_RSP_SHOW_MESSAGE);
            logger.Debug("ShowMessage() -res=" + res);
        }





        // A24 - DoSignature
        public PaxDeviceResponse DoSignature()
        {
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A20_DO_SIGNATURE, "0", "", "01", null)); // PRIMEPOS - 2555 Change timeout to null - NileshJ
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A21_RSP_DO_SIGNATURE);
            logger.Debug("DoSignature() -res=" + res);
            return res;

        }


        // A24 - GetSignature
        public PaxDeviceResponse GetSignature()
        {
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A08_GET_SIGNATURE, "0", ""));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A09_RSP_GET_SIGNATURE);
            logger.Debug("GetSignature() -response=" + res);
            return res;
        }



        public PaxDeviceResponse ResetMSR() {
            logger.Debug("ResetMSR() -" + clsPOSDBConstants.Log_Entering);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A16_RESET));
            var res = new PaxDeviceResponse(response, PAX_MSG_ID.A17_RSP_RESET);
            logger.Debug("ResetMSR() - Reponse : A16_RESET = " + response + "; Res : A17_RSP_RESET=" + res);
            Console.WriteLine("--------------------" + res);
            Console.WriteLine("--------------------");
            return null;
        }


        public PaxDeviceResponse UpdateResourceImage(byte[] file) {
            logger.Debug("UpdateResourceImage(byte[] file) -" + clsPOSDBConstants.Log_Entering);
            if (file!=null && file.Length>1) {
                string byteString = Encoding.UTF8.GetString(file, 0, file.Length);
                string base64File= Convert.ToBase64String(file);
                logger.Debug("UpdateResourceImage(byte[] file) - byteString=" + byteString + ";base64File=" + base64File);
                byte[] response;
                PaxDeviceResponse res = null;
                var tempCheck = "";
                if (base64File.Length > 4000) {
                    var fileTemp = file;
                    int start = 0;
                    double offset = 0;
                    var fileArray = Split(base64File, 4000);
                    for (int i = 0; i < fileArray.Count(); i++) {
                        string segment = fileArray.ElementAt(i);
                        var segmentBytes = Convert.FromBase64String(segment);
                        //string segmentString = Encoding.UTF8.GetString(segmentBytes, 0, segmentBytes.Length);
                        offset = offset+segmentBytes.Length;
                        if (i == fileArray.Count() - 1) {
                            response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, offset, segment, "0"));
                            res = new PaxDeviceResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                            Console.WriteLine(i + "--------------------" + res);
                            tempCheck += segment;
                        } else {
                            response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, offset, segment, "1"));
                            res = new PaxDeviceResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                            Console.WriteLine(i + "--------------------" + res);
                            tempCheck += segment;
                        }
                    }
              

                } else {
                    response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, "0", file, "0"));
                    res = new PaxDeviceResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                    Console.WriteLine("--------------------" + res);
                }

                var isIntegrity = tempCheck.Length == file.Length;
                Console.WriteLine("integrity of string --------------------" + isIntegrity);
                logger.Debug("UpdateResourceImage(byte[] file) : integrity of string : " + isIntegrity);
                logger.Debug("res:" + res);
                return res;
            } else{
                return null;
            }
        }




        public PaxDeviceResponse UpdateResourceImageNew(byte[] file) {
            logger.Debug("UpdateResourceImageNew(byte[] file) :  " + clsPOSDBConstants.Log_Entering);
            if (file != null && file.Length > 1) {
                byte[] response;
                PaxDeviceResponse res = null;
                int offset = 0;
               
                for (int i = 0; i < file.Length;i+=3000) {
                    if (file.Length - i >= 3000) {
                        byte[] copiedByts = new byte[3000];
                        Array.Copy(file, i, copiedByts, 0, 3000);
                        string base64File = Convert.ToBase64String(copiedByts);
                        response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, offset, base64File, "1"));
                        res = new PaxDeviceResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                        Console.WriteLine(offset + "--------------------" + res);
                        offset +=3000;
                    } else {
                        byte[] copiedByts = new byte[file.Length -i];
                        Array.Copy(file, i, copiedByts, 0, file.Length - i);
                        string base64File = Convert.ToBase64String(copiedByts);
                        response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.A18_UPDATE_RESOURCE_FILE, offset, base64File, "0"));
                        res = new PaxDeviceResponse(response, PAX_MSG_ID.A19_RSP_UPDATE_RESOURCE_FILE);
                        Console.WriteLine(offset + "--------------------" + res);
                    }
                }
                return res;
            } else {
                return null;
            }
        }



        static IEnumerable<string> Split(string str, int chunkLength) {
            for (int i = 0; i < str.Length; i += chunkLength) {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }


        #endregion

        #region Transaction Commands
        internal byte[] DoSend(string messageId, params object[] elements)
        {
            return _interface.Send(TerminalUtilities.BuildRequest(messageId, elements));
        }

        internal byte[] DoTransaction(string messageId, string txnType, params IRequestSubGroup[] subGroups)
        {
            logger.Debug("DoTransaction() : MessageID=" + messageId + "; TransactionType=" + txnType + "; SubGroup" + subGroups);
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
            Console.WriteLine("PAX message " + message);
            logger.Debug("DoTransaction(): MEssage = " + message);
            return _interface.Send(message);
        }

        public CreditResponse DoCredit(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, AvsRequest avs, CashierSubGroup cashier, CommercialRequest commercial, EcomSubGroup ecom, ExtDataSubGroup extData)
        {
            logger.Debug("PaxDevice.DoCredit() : TransactionType=" + txnType + ";Amount=" + amounts + ";Account=" + accounts + ";Trace=" + trace + ";AvsRequest" + avs + ";CashierSubGroup=" + cashier + ";CommercialRequest=" + commercial + ";EcomSubGroup=" + ecom + ";ExtDataSubGroup=" + extData);
            var response = DoTransaction(PAX_MSG_ID.T00_DO_CREDIT, txnType, amounts, accounts, trace, avs, cashier, commercial, ecom, extData);
            var res = new CreditResponse(response);
            logger.Debug("DoCredit() : Response=" + res);
            return res;
        }

        public DebitResponse DoDebit(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData)
        {
            logger.Debug("DoDebit() : TransactionType=" + txnType + "; Amount=" + amounts + "; Account=" + accounts + "; Trace=" + trace + "; CashierSubGroup=" + cashier + "; ExtDataSubGroup=" + extData);
            var response = DoTransaction(PAX_MSG_ID.T02_DO_DEBIT, txnType, amounts, accounts, trace, cashier, extData);
            var res = new DebitResponse(response);
            logger.Debug("DoDebit() : Response=" + res);
            return res;
        }

        public EbtResponse DoEBT(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier)
        {
            logger.Debug("DoEBT() : TransactionType=" + txnType + "; Amount=" + amounts + "; Account=" + accounts + "; Trace=" + trace + "; CashierSubGroup=" + cashier);
            var response = DoTransaction(PAX_MSG_ID.T04_DO_EBT, txnType, amounts, accounts, trace, cashier, new ExtDataSubGroup());
            var res = new EbtResponse(response);
            logger.Debug("DoEBT() : Response=" + res);
            return res;
        }

        public GiftResponse DoGift(string messageId, string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData = null)
        {
            logger.Debug("DoGift() : MessageID=" + messageId + "TransactionType=" + txnType + "; Amount=" + amounts + "; Account=" + accounts + "; Trace=" + trace + "; CashierSubGroup=" + cashier + "; ExtDataSubGroup=" + extData);
            var response = DoTransaction(messageId, txnType, amounts, accounts, trace, cashier, extData);
            var res = new GiftResponse(response);
            logger.Debug("DoGift() : Response=" + res);
            return res;
        }

        public CashResponse DoCash(string txnType, AmountRequest amounts, TraceRequest trace, CashierSubGroup cashier)
        {
            logger.Debug("DoCash() : TransactionType=" + txnType + "; Amount=" + amounts + "; Trace=" + trace + "; CashierSubGroup=" + cashier);
            var response = DoTransaction(PAX_MSG_ID.T10_DO_CASH, txnType, amounts, trace, cashier, new ExtDataSubGroup());
            var res = new CashResponse(response);
            logger.Debug("DoCash() : Response=" + res);
            return res;
        }

        public CheckSubResponse DoCheck(string txnType, AmountRequest amounts, CheckSubGroup check, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData)
        {
            logger.Debug("DoCheck() : TransactionType=" + txnType + "; Amount=" + amounts + "; CheckSubGroup=" + check + "; Trace=" + trace + "; CashierSubGroup=" + cashier + "; ExtDataSubGroup=" + extData);
            var response = DoTransaction(PAX_MSG_ID.T12_DO_CHECK, txnType, amounts, check, trace, cashier, extData);
            var res = new CheckSubResponse(response);
            logger.Debug("DoCheck() : Response=" + res);
            return res;
        }
        #endregion

        #region Credit Methods
        public CreditAuthBuilder CreditAuth(int referenceNumber, decimal? amount = null)
        {
            logger.Debug("CreditAuth() : ReferenceNumber=" + referenceNumber + "; Amount=" + amount);
            return new CreditAuthBuilder(this).WithReferenceNumber(referenceNumber).WithAmount(amount);
        }

        public CreditCaptureBuilder CreditCapture(int referenceNumber, decimal? amount = null)
        {
            logger.Debug("CreditCapture(): ReferenceNumber=" + referenceNumber + "; Amount=" + amount);
            return new CreditCaptureBuilder(this).WithReferenceNumber(referenceNumber).WithAmount(amount);
        }

        public CreditEditBuilder CreditEdit(decimal? amount = null) {
            if (!_settings.DeviceId.HasValue || !_settings.SiteId.HasValue || !_settings.LicenseId.HasValue || string.IsNullOrEmpty(_settings.UserName) || string.IsNullOrEmpty(_settings.Password))
                throw new HpsConfigurationException("Device is not configured properly for Credit Edit. Please provide the device credentials in the ConnectionConfig.");

            var service = new HpsFluentCreditService(new HpsServicesConfig() {
                DeviceId = _settings.DeviceId.Value,
                SiteId = _settings.SiteId.Value,
                LicenseId = _settings.LicenseId.Value,
                UserName = _settings.UserName,
                Password = _settings.Password,
                ServiceUrl = _settings.Url
            });
            logger.Debug("CreditEdit(): Service=" + _settings.DeviceId.Value + "; DeviceID=" + _settings.DeviceId.Value + "; SiteId=" + _settings.SiteId.Value + "; LicenseId=" + _settings.LicenseId.Value + "; UserName=" + _settings.UserName + "; Password=" + _settings.Password + "; ServiceUrl" + _settings.Url);
            return new CreditEditBuilder(service).WithAmount(amount);
        }

        public CreditReturnBuilder CreditReturn(int referenceNumber, decimal? amount = null)
        {
            logger.Debug("CreditReturn(): ReferenceNumber=" + referenceNumber + "; Amount=" + amount);
            return new CreditReturnBuilder(this).WithReferenceNumber(referenceNumber).WithAmount(amount);
        }

        public CreditSaleBuilder CreditSale(int referenceNumber, decimal? amount = null)
        {
            logger.Debug("CreditSale(): ReferenceNumber=" + referenceNumber + "; Amount=" + amount);
            return new CreditSaleBuilder(this).WithReferenceNumber(referenceNumber).WithAmount(amount);
        }

        public CreditVerifyBuilder CreditVerify(int referenceNumber)
        {
            logger.Debug("CreditVerify(): ReferenceNumber=" + referenceNumber);
            return new CreditVerifyBuilder(this).WithReferenceNumber(referenceNumber);
        }

        public CreditVoidBuilder CreditVoid(int referenceNumber)
        {
            logger.Debug("CreditVoid(): ReferenceNumber=" + referenceNumber);
            return new CreditVoidBuilder(this).WithReferenceNumber(referenceNumber);
        }
        #endregion

        #region Debit Methods
        public DebitReturnBuilder DebitReturn(int referenceNumber, decimal? amount = null)
        {
            logger.Debug("DebitReturn(): ReferenceNumber=" + referenceNumber + "; Amount=" + amount);
            return new DebitReturnBuilder(this).WithReferenceNumber(referenceNumber).WithAmount(amount);
        }

        public DebitSaleBuilder DebitSale(int referenceNumber, decimal? amount = null)
        {
            logger.Debug("DebitSale(): ReferenceNumber=" + referenceNumber + "; Amount=" + amount);
            return new DebitSaleBuilder(this).WithReferenceNumber(referenceNumber).WithAmount(amount);
        }
        #endregion

        #region EBT Methods
        #endregion

        #region Gift Methods
        public GiftSaleBuilder GiftSale(int referenceNumber, decimal? amount = null)
        {
            logger.Debug("GiftSale(): ReferenceNumber=" + referenceNumber + "; Amount=" + amount);
            return new GiftSaleBuilder(this).WithReferenceNumber(referenceNumber).WithAmount(amount);
        }

        public GiftAddValueBuilder GiftAddValue(int referenceNumber)
        {
            logger.Debug("GiftAddValue(): ReferenceNumber=" + referenceNumber);
            return new GiftAddValueBuilder(this).WithReferenceNumber(referenceNumber);
        }

        public GiftVoidBuilder GiftVoid(int referenceNumber)
        {
            logger.Debug("GiftVoid(): ReferenceNumber=" + referenceNumber);
            return new GiftVoidBuilder(this).WithReferenceNumber(referenceNumber);
        }

        public GiftBalanceBuilder GiftBalance(int referenceNumber)
        {
            logger.Debug("GiftBalance(): ReferenceNumber=" + referenceNumber);
            return new GiftBalanceBuilder(this).WithReferenceNumber(referenceNumber);
        }
        #endregion

        #region Cash Methods
        #endregion

        #region Check Methods
        #endregion

        #region Batch Commands
        public BatchCloseResponse BatchClose()
        {
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.B00_BATCH_CLOSE, DateTime.Now.ToString("YYYYMMDDhhmmss")));
            var res = new BatchCloseResponse(response);
            logger.Debug("BatchClose(): Response=" + res);
            return res;
        }
        #endregion

        #region Reporting Commands
        #endregion
        //Nilesh,Sajid add new method PRIMEPOS-2854
        public string GetDeviceRequestMessage(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, AvsRequest avs, CashierSubGroup cashier, CommercialRequest commercial, EcomSubGroup ecom, ExtDataSubGroup extData)
        {
            IRequestSubGroup[] subGroups = { amounts, accounts, trace, avs, cashier, commercial, ecom, extData };
            logger.Debug("DoTransaction() : MessageID= T00 " + ": TXN_TYPE_MAP[txnType] = " + txnType + "; SubGroup" + subGroups);

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
        public PaxDeviceResponse LocalDetailReport(string refNumber)
        {
            logger.Debug("LocalDetailReport() - RefNumber :" + refNumber + " - " + clsPOSDBConstants.Log_Entering);
            var response = _interface.Send(TerminalUtilities.BuildRequest(PAX_MSG_ID.R02_LOCAL_DETAIL_REPORT, "00", "", "", "", "", "", refNumber, ""));
            var res = new ReportResponse(response);
            logger.Debug("LocalDetailReport() -" + res);
            return res;
        }
        public void Dispose() {
            _interface.Disconnect();
        }
    }
}
