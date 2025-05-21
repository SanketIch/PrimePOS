using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Terminals.Builders;
using MMS.GlobalPayments.Api.Terminals.Messaging;
using MMS.GlobalPayments.Api.Terminals.PAX;
using System.Collections.Generic;

namespace MMS.GlobalPayments.Api.Terminals 
{
    public abstract class DeviceInterface<T> : IDeviceInterface where T : DeviceController {
        protected T _controller;
        protected IRequestIdProvider _requestIdProvider;

        public event MessageSentEventHandler OnMessageSent;
        public string EcrId { get; set; }
        internal DeviceInterface(T controller) {
            _controller = controller;
            _controller.OnMessageSent += (message) => {
                OnMessageSent?.Invoke(message);
            };
            _requestIdProvider = _controller.RequestIdProvider;
        }

        #region Admin Methods
        public virtual PaxTerminalResponse Cancel() //NG SDKUPDATE 20/9/2022
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

     
        
        public virtual IDeviceResponse CloseLane() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual IDeviceResponse DisableHostResponseBeep() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual ISignatureResponse GetSignatureFile() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual PaxTerminalResponse Initialize() //NG SDKUPDATE 20/9/2022
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual IDeviceResponse LineItem(string leftText, string rightText = null, string runningLeftText = null, string runningRightText = null) {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual IDeviceResponse OpenLane() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual ISignatureResponse PromptForSignature(string transactionId = null) {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual PaxTerminalResponse Reboot() //NG SDKUPDATE 20/9/2022
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual PaxTerminalResponse Reset() //NG SDKUPDATE 20/9/2022
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual string SendCustomMessage(DeviceMessage message) {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual IDeviceResponse SendFile(SendFileType fileType, string filePath) {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual ISAFResponse SendStoreAndForward() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual IDeviceResponse SetStoreAndForwardMode(bool enabled) {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual IDeviceResponse StartCard(PaymentMethodType paymentMethodType) {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual IDeviceResponse SendSaf() {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Batching
        public virtual IBatchCloseResponse BatchClose() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual IEODResponse EndOfDay() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        #endregion

        #region Reporting Methods  //NG SDKUPDATE 20/9/2022
       /* public virtual TerminalReportBuilder LocalDetailReport() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual TerminalReportBuilder GetSAFReport() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual TerminalReportBuilder GetBatchReport() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public virtual TerminalReportBuilder GetOpenTabDetails() {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }*/
        #endregion

        #region Transactions
        public virtual TerminalAuthBuilder AddValue(decimal? amount = null) {
            return new TerminalAuthBuilder(TransactionType.AddValue, PaymentMethodType.Gift)
                .WithAmount(amount);
        }
        public virtual TerminalAuthBuilder Authorize(decimal? amount = null) {
            return new TerminalAuthBuilder(TransactionType.Auth, PaymentMethodType.Credit)
                .WithAmount(amount);
        }
        public virtual TerminalAuthBuilder Balance() {
            return new TerminalAuthBuilder(TransactionType.Balance, PaymentMethodType.Gift);
        }
        public virtual TerminalManageBuilder Capture(decimal? amount = null) {
            return new TerminalManageBuilder(TransactionType.Capture, PaymentMethodType.Credit)
                .WithAmount(amount);
        }
        public virtual TerminalAuthBuilder Refund(decimal? amount = null) {
            return new TerminalAuthBuilder(TransactionType.Refund, PaymentMethodType.Credit)
                .WithAmount(amount);
        }
        public virtual TerminalAuthBuilder Sale(decimal? amount = null) {
            return new TerminalAuthBuilder(TransactionType.Sale, PaymentMethodType.Credit)
                .WithAmount(amount);
        }
        public virtual TerminalAuthBuilder Verify() {
            return new TerminalAuthBuilder(TransactionType.Verify, PaymentMethodType.Credit);
        }
        public virtual TerminalManageBuilder Void() {
            return new TerminalManageBuilder(TransactionType.Void, PaymentMethodType.Credit);
        }
        public virtual TerminalAuthBuilder Withdrawal(decimal? amount = null) {
            return new TerminalAuthBuilder(TransactionType.BenefitWithdrawal, PaymentMethodType.EBT)
                .WithAmount(amount);
        }
        #endregion

        //NG SDKUPDATE 20/9/2022
        #region Custom Method 

        public virtual void ShowItems(object[] dataList, string action, DeviceType deviceType)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual void ShowItemsDisplay(object[] dataList, string action, DeviceType deviceType, int index)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual IDeviceResponse ClearMessageScreen()
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual PaxTerminalResponse ShowWelcomeMessage(string title, string displayMessage1, string displayMessage2, string timeout)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual PaxTerminalResponse ShowThanksMessage(string title, string displayMessage1, string displayMessage2, string timeout)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual IDeviceResponse ShowMessageCenterAligned(string title, string displayMessage1, string timeout)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual PaxTerminalResponse ShowDialog(string title, string button1, string button2, string button3, string button4)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual PaxTerminalResponse ShowDialogForm(string title, string Label1, int Label1Property, string Label2, int Label2Property, string Label3, int Label3Property, string Label4, int Label4Property, int buttonType)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual PaxTerminalResponse ShowTextBox(string title, string message, string button1, string button1Color, string button2, string button2Color, string button3, string button3Color)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual void ShowMessage(string title, string message)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual void ShowTwoLineMessage(string title, string displayMessage1, string displayMessage2)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual PaxTerminalResponse DoSignature()
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual PaxTerminalResponse GetSignature()
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual IDeviceResponse ResetMSR()
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual IDeviceResponse UpdateResourceImage(byte[] file)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual PaxTerminalResponse UpdateResourceImageNew(byte[] file)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public virtual string GetDeviceRequestMessage(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, AvsRequest avs, CashierSubGroup cashier, CommercialRequest commercial, EcomSubGroup ecom, ExtDataSubGroup extData)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        #region Reporting Methods
        public virtual PaxTerminalResponse LocalDetailReport(string refNumber)
        {
            {
                throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
            }
        }

        #endregion

        public CreditResponse DoCredit(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, AvsRequest avs, CashierSubGroup cashier, CommercialRequest commercial, EcomSubGroup ecom, ExtDataSubGroup extData)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public PaxTerminalResponse DoCreditToken(string Token, string Amount, Dictionary<string, string> Fields)//2990
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public PaxTerminalResponse DoVoidToken(Dictionary<string, string> Fields,string Transid)//2990
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public PaxTerminalResponse DoReverseToken(Dictionary<string, string> Fields, string Transid)//2990
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }
        public DebitResponse DoDebit(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public EbtResponse DoEBT(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public GiftResponse DoGift(string messageId, string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData = null)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public CashResponse DoCash(string txnType, AmountRequest amounts, TraceRequest trace, CashierSubGroup cashier)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }

        public CheckSubResponse DoCheck(string txnType, AmountRequest amounts, CheckSubGroup check, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData)
        {
            throw new UnsupportedTransactionException("This function is not supported by the currently configured device.");
        }


        #endregion

        public virtual TerminalAuthBuilder TipAdjust(decimal? amount) {
            throw new System.NotImplementedException();
        }
        public virtual TerminalAuthBuilder EodProcessing() {
            throw new System.NotImplementedException();
        }

        public virtual TerminalAuthBuilder Tokenize() {
            throw new System.NotImplementedException();
        }

        public virtual TerminalAuthBuilder AuthCompletion() {
            throw new System.NotImplementedException();
        }
        public virtual TerminalManageBuilder DeletePreAuth() {
            throw new System.NotImplementedException();
        }
       

        #region IDisposable
        public void Dispose() {
            _controller.Dispose();
        }
        #endregion
    }
}
