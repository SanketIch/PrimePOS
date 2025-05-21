
namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class POSTransPaymentRow : DataRow
    {
        private POSTransPaymentTable table;

        internal POSTransPaymentRow(DataRowBuilder rb) : base(rb)
        {
            this.table = (POSTransPaymentTable)this.Table;
        }

        #region Public Properties
        public System.Int32 TransPayID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransPayID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransPayID] = value; }
        }

        public System.Int32 TransID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransID] = value; }
        }

        public System.String TransTypeCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransTypeCode];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.TransTypeCode] = value; }
        }

        public System.String TransTypeDesc
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransTypeDesc];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.TransTypeDesc] = value; }
        }

        public System.Decimal Amount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.Amount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.Amount] = value; }
        }

        public System.Boolean HC_Posted
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.HC_Posted];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.HC_Posted] = value; }
        }

        public System.DateTime TransDate
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.TransDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.TransDate] = value; }
        }

        public System.String RefNo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.RefNo];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.RefNo] = value; }
        }

        public System.String AuthNo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.AuthNo];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.AuthNo] = value; }
        }

        public System.String CCName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CCName];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.CCName] = value; }
        }

        public System.String CCTransNo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CCTransNo];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.CCTransNo] = value; }
        }

        public System.String CustomerSign
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CustomerSign];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.CustomerSign] = value; }
        }

        public System.Byte[] BinarySign
        {
            get
            {
                try
                {
                    return (System.Byte[])this[this.table.BinarySign];
                }
                catch
                {
                    return null;
                }
            }
            set { this[this.table.BinarySign] = value; }
        }

        public System.String SigType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.SigType];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.SigType] = value; }
        }

        public System.Boolean IsIIASPayment
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsIIASPayment];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsIIASPayment] = value; }
        }

        //Added By SRT(Gaurav) Date : 21-07-2009
        public System.String PaymentProcessor
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PaymentProcessor];
                }
                catch
                {
                    return clsPOSDBConstants.NOPROCESSOR;
                }
            }
            set { this[this.table.PaymentProcessor] = value; }
        }
        //End Of Added By SRT(Gaurav)

        public System.Int32 CLCouponID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.CLCouponID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.CLCouponID] = value; }
        }

        //Added By Shitaljit on 19 july 2012 to store Processor TransID
        public System.String ProcessorTransID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ProcessorTransID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ProcessorTransID] = value; }
        }

        #region Sprint-19 - 2139 06-Jan-2015 JY Added
        public System.String IsManual
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.IsManual];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.IsManual] = value; }
        }
        #endregion
        public System.String CashBack
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CashBack];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.CashBack] = value; }
        }

        #region   Added for Solutran - PRIMEPOS-2663 - NileshJ
        public System.String S3TransID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.S3TransID];
                }
                catch
                {
                    return "0";
                }
            }
            set { this[this.table.S3TransID] = value; }
        }

        #endregion

        #region Emv
        public System.String Aid
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Aid];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.Aid] = value; }
        }
        public string AidName
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.AidName];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.AidName] = value; }
        }
        public string Cryptogram
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Cryptogram_AC];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.Cryptogram_AC] = value; }
        }
        public string TransCounter
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransactionCounter_ATC];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TransactionCounter_ATC] = value; }
        }
        public string TerminalTvr
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Terminal_Tvr];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.Terminal_Tvr] = value; }
        }
        public string TransStatusInfo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransStatusInfo_Tsi];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TransStatusInfo_Tsi] = value; }
        }
        public string AuthorizeRespCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.AuthorizationRespCode_CD];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.AuthorizationRespCode_CD] = value; }
        }
        public string TransRefNum
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransRefNum_Trn];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TransRefNum_Trn] = value; }
        }
        public string ValidateCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ValidateCode_Vc];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ValidateCode_Vc] = value; }
        }
        //PRIMEPOS-2636 VANTIV
        public System.String TransactionID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TransactionID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TransactionID] = value; }
        }
        public System.String ResponseCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ResponseCode];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ResponseCode] = value; }
        }
        public System.String ApprovalCode
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ApprovalCode];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ApprovalCode] = value; }
        }
        public System.String TerminalID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TerminalID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TerminalID] = value; }
        }
        public System.String ReferenceNumber
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ReferenceNumber];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ReferenceNumber] = value; }
        }
        //
        public string MerchantID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.MerchantID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.MerchantID] = value; }
        }
        public string RTransID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.RTransID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.RTransID] = value; }
        }
        public string EntryMethod
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.EntryMethod];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.EntryMethod] = value; }
        }
        public string EntryLegend
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.EntryLegend];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.EntryLegend] = value; }
        }
        public string ProfiledID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ProfiledID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ProfiledID] = value; }
        }
        public string CardType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CardType_Ct];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.CardType_Ct] = value; }
        }
        public string ProcTransType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ProcTransType];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ProcTransType] = value; }
        }
        public string Verbiage
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Verbiage];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.Verbiage] = value; }
        }

        public string IssuerAppData
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.IssuerAppData];
                }
                catch
                {
                    return "";
                }

            }
            set
            {
                this[this.table.IssuerAppData] = value;
            }
        }

        public string CardVerificationMethod
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CardVerificationMethod];
                }
                catch
                {
                    return "";
                }

            }
            set
            {
                this[this.table.CardVerificationMethod] = value;
            }
        }


        //public string ApprovalCode
        //{
        //    get
        //    {
        //        try
        //        {
        //            return (System.String)this[this.table.ApprovalCode];
        //        }
        //        catch
        //        {
        //            return "";
        //        }
        //    }
        //    set { this[this.table.ApprovalCode] = value; }
        //}
        #endregion Emv
        #region PRIMEPOS-2664 ADDED BY ARVIND EVERTEC
        public System.String BatchNumber
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.BatchNumber];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.BatchNumber] = value; }
        }
        public System.String InvoiceNumber
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.InvoiceNumber];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.InvoiceNumber] = value; }
        }
        public System.String TraceNumber
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TraceNumber];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.TraceNumber] = value; }
        }
        #endregion

        #region  PRIMEPOS-2761 - NileshJ
        public System.String TicketNumber
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TicketNumber];
                }
                catch
                {
                    return "0";
                }
            }
            set { this[this.table.TicketNumber] = value; }
        }

        #endregion
        #region PRIMEPOS-2664 ADDED BY ARVIND EVERTEC
        public System.String ControlNumber
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ControlNumber];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ControlNumber] = value; }
        }
        public System.String EbtBalance
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.EbtBalance];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.EbtBalance] = value; }
        }
        #endregion
        #region PRIMEPOS-2793
        public System.String ApplicaionLabel
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ApplicationLabel];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ApplicationLabel] = value; }
        }
        public System.Boolean PinVerified
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.PinVerified];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.PinVerified] = value; }
        }
        public System.String LaneID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.LaneID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.LaneID] = value; }
        }
        public System.String CardLogo
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.CardLogo];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.CardLogo] = value; }
        }
        #endregion
        #region PRIMEPOS-2915 
        public System.String PrimeRxPayTransID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.PrimeRxPayTransID];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set { this[this.table.PrimeRxPayTransID] = value; }
        }

        public System.Decimal ApprovedAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.ApprovedAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.ApprovedAmount] = value; }
        }

        public System.String Status
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Status];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set { this[this.table.Status] = value; }
        }
        public System.String Email
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Email];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set { this[this.table.Email] = value; }
        }
        public System.String Mobile
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.Mobile];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set { this[this.table.Mobile] = value; }
        }
        public System.Int32 TransactionProcessingMode
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransactionProcessingMode];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransactionProcessingMode] = value; }
        }
        #endregion
        public System.String ATHMovil//2664
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ATHMovil];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ATHMovil] = value; }
        }
        //PRIMEPOS-2857
        public System.String EvertecTaxBreakdown
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.EvertecTaxBreakdown];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.EvertecTaxBreakdown] = value; }
        }
        #region PRIMEPOS-2831
        public System.Boolean IsEvertecForceTransaction
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsEvertecForceTransaction];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsEvertecForceTransaction] = value; }
        }
        public System.Boolean IsEvertecSign
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsEvertecSign];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsEvertecSign] = value; }
        }
        #endregion

        #region PRIMEPOS-2402 09-Jul-2021 JY Added
        //Override Housecharge Limit
        public System.String OverrideHousechargeLimitUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.OverrideHousechargeLimitUser];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.OverrideHousechargeLimitUser] = value; }
        }
        //Max. Tendered Amount
        public System.String MaxTenderedAmountOverrideUser
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.MaxTenderedAmountOverrideUser];
                }
                catch
                {
                    return System.String.Empty;
                }
            }
            set { this[this.table.MaxTenderedAmountOverrideUser] = value; }
        }
        #endregion

        public System.DateTime ExpiryDate//2943
        {
            get
            {
                try
                {
                    return (System.DateTime)this[this.table.ExpiryDate];
                }
                catch
                {
                    return System.DateTime.MinValue;
                }
            }
            set { this[this.table.ExpiryDate] = value; }
        }
        public System.Boolean IsFsaCard//2990
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsFsaCard];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsFsaCard] = value; }
        }
        public System.Int32 TokenID//3009
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TokenID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TokenID] = value; }
        }        

        #region PRIMEPOS-3117 11-Jul-2022 JY Added
        public System.Decimal TransFeeAmt
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.TransFeeAmt];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransFeeAmt] = value; }
        }
        #endregion

        #region PRIMEPOS-3145 28-Sep-2022 JY Added
        public System.Boolean Tokenize
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.Tokenize];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.Tokenize] = value; }
        }
        #endregion

        #region PRIMEPOS-3375
        public System.String NBSTransId
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.NBSTransId];
                }
                catch
                {
                    return "0";
                }
            }
            set { this[this.table.NBSTransId] = value; }
        }
        #endregion

        #region PRIMEPOS-3375
        public System.String NBSTransUid
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.NBSTransUid];
                }
                catch
                {
                    return "0";
                }
            }
            set { this[this.table.NBSTransUid] = value; }
        }
        #endregion

        #region PRIMEPOS-3375
        public System.String NBSPaymentType
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.NBSPaymentType];
                }
                catch
                {
                    return "0";
                }
            }
            set { this[this.table.NBSPaymentType] = value; }
        }
        #endregion
        #region PRIMEPOS-3428
        public System.String TenderedAmount
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.TenderedAmount];
                }
                catch
                {
                    return "0";
                }
            }
            set { this[this.table.TenderedAmount] = value; }
        }
        #endregion
        #endregion
    }
}