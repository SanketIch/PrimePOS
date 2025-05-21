using System;
using System.Collections.Generic;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Terminals.Abstractions;
using MMS.GlobalPayments.Api.Terminals.Builders;
using MMS.GlobalPayments.Api.Terminals.Messaging;
using MMS.GlobalPayments.Api.Terminals.PAX;

namespace MMS.GlobalPayments.Api.Terminals {
    public interface IDeviceInterface : IDisposable {
        event MessageSentEventHandler OnMessageSent;
        string EcrId { get; set; }
        #region Admin Calls
        PaxTerminalResponse Cancel(); //NG SDKUPDATE 20/9/2022
        IDeviceResponse CloseLane();
        IDeviceResponse DisableHostResponseBeep();
        ISignatureResponse GetSignatureFile();
        PaxTerminalResponse Initialize(); //NG SDKUPDATE 20/9/2022
        IDeviceResponse LineItem(string leftText, string rightText = null, string runningLeftText = null, string runningRightText = null);
        IDeviceResponse OpenLane();
        ISignatureResponse PromptForSignature(string transactionId = null);
        PaxTerminalResponse Reboot(); //NG SDKUPDATE 20/9/2022
        PaxTerminalResponse Reset(); //NG SDKUPDATE 20/9/2022
        string SendCustomMessage(DeviceMessage message);
        IDeviceResponse SendFile(SendFileType fileType, string filePath);
        ISAFResponse SendStoreAndForward();
        IDeviceResponse SetStoreAndForwardMode(bool enabled);
        IDeviceResponse StartCard(PaymentMethodType paymentMethodType);
        #endregion

        #region reporting
        // TerminalReportBuilder LocalDetailReport();
        #endregion

        #region Batch Calls
        IBatchCloseResponse BatchClose();
        IEODResponse EndOfDay();
        #endregion

        #region Credit Calls
        //TerminalAuthBuilder CreditAuth(decimal? amount = null);
        //TerminalManageBuilder CreditCapture(decimal? amount = null);
        //TerminalAuthBuilder CreditRefund(decimal? amount = null);
        //TerminalAuthBuilder CreditSale(decimal? amount = null);
        //TerminalAuthBuilder CreditVerify();
        //TerminalManageBuilder CreditVoid();
        #endregion

        #region Debit Calls
        //TerminalAuthBuilder DebitSale(decimal? amount = null);
        //TerminalAuthBuilder DebitRefund(decimal? amount = null);
        #endregion

        #region Gift Calls
        //TerminalAuthBuilder GiftSale(decimal? amount = null);
        //TerminalAuthBuilder GiftAddValue(decimal? amount = null);
        //TerminalManageBuilder GiftVoid();
        //TerminalAuthBuilder GiftBalance();
        #endregion

        #region EBT Calls
        //TerminalAuthBuilder EbtBalance();
        //TerminalAuthBuilder EbtPurchase(decimal? amount = null);
        //TerminalAuthBuilder EbtRefund(decimal? amount = null);
        //TerminalAuthBuilder EbtWithdrawl(decimal? amount = null);
        #endregion

        #region Generic Calls
        TerminalAuthBuilder AddValue(decimal? amount = null);
        TerminalAuthBuilder Authorize(decimal? amount = null);
        TerminalAuthBuilder Balance();
        TerminalManageBuilder Capture(decimal? amount = null);
        TerminalAuthBuilder Refund(decimal? amount = null);
        TerminalAuthBuilder Sale(decimal? amount = null);
        TerminalAuthBuilder Verify();
        TerminalManageBuilder Void();
        TerminalAuthBuilder Withdrawal(decimal? amount = null);
        TerminalAuthBuilder TipAdjust(decimal? amount = null);
        TerminalAuthBuilder Tokenize();
        TerminalAuthBuilder AuthCompletion();
        TerminalManageBuilder DeletePreAuth();
		#region Transaction Commands //NG SDKUPDATE 20/9/2022
        CreditResponse DoCredit(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, AvsRequest avs, CashierSubGroup cashier, CommercialRequest commercial, EcomSubGroup ecom, ExtDataSubGroup extData);
        PaxTerminalResponse DoCreditToken(string Token, string Amount, Dictionary<string, string> Fields);//2990
        PaxTerminalResponse DoVoidToken(Dictionary<string, string> Fields, string Transid);//2990
        PaxTerminalResponse DoReverseToken(Dictionary<string, string> Fields, string Transid);//2990
        DebitResponse DoDebit(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData);
        EbtResponse DoEBT(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier);
        GiftResponse DoGift(string messageId, string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData = null);
        CashResponse DoCash(string txnType, AmountRequest amounts, TraceRequest trace, CashierSubGroup cashier);
        CheckSubResponse DoCheck(string txnType, AmountRequest amounts, CheckSubGroup check, TraceRequest trace, CashierSubGroup cashier, ExtDataSubGroup extData);

        #endregion
        #endregion
        //NG SDKUPDATE 20/9/2022
        #region Custome Method 
        void ShowItems(object[] dataList, string action, DeviceType deviceType);
        void ShowItemsDisplay(object[] dataList, string action, DeviceType deviceType, int index);
        IDeviceResponse ClearMessageScreen();
        PaxTerminalResponse ShowWelcomeMessage(string title, string displayMessage1, string displayMessage2, string timeout);
        PaxTerminalResponse ShowThanksMessage(string title, string displayMessage1, string displayMessage2, string timeout);
        IDeviceResponse ShowMessageCenterAligned(string title, string displayMessage1, string timeout);
        PaxTerminalResponse ShowDialog(string title, string button1, string button2, string button3, string button4);
        PaxTerminalResponse ShowDialogForm(string title, string Label1, int Label1Property, string Label2, int Label2Property, string Label3, int Label3Property, string Label4, int Label4Property, int buttonType);
        PaxTerminalResponse ShowTextBox(string title, string message, string button1, string button1Color, string button2, string button2Color, string button3, string button3Color);

        void ShowMessage(string title, string message);

        void ShowTwoLineMessage(string title, string displayMessage1, string displayMessage2);
        PaxTerminalResponse DoSignature();
        PaxTerminalResponse GetSignature();
        IDeviceResponse ResetMSR();
        IDeviceResponse UpdateResourceImage(byte[] file);
        PaxTerminalResponse UpdateResourceImageNew(byte[] file);
        string GetDeviceRequestMessage(string txnType, AmountRequest amounts, AccountRequest accounts, TraceRequest trace, AvsRequest avs, CashierSubGroup cashier, CommercialRequest commercial, EcomSubGroup ecom, ExtDataSubGroup extData);
        PaxTerminalResponse LocalDetailReport(string refNumber);

        void Dispose();
        #endregion
    }
}
