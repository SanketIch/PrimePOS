
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class POSTransPaymentTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colTransPayId;
		private DataColumn colTransTypeCode;
		private DataColumn colTransTypeDesc;
		private DataColumn colHC_Posted;

		private DataColumn colTransDate;
		private DataColumn colExpiryDate;//2943
        private DataColumn colTransID;
		private DataColumn colAmount;
		private DataColumn colRefNo;
		private DataColumn colAuthNo;
		private DataColumn colCCName;
		private DataColumn colCCTransNo;
        private DataColumn colCustomerSign;
        private DataColumn colBinarySign;
        private DataColumn colSigType;
        private DataColumn colIsIIASPayment;
        //Added By SRT(Gaurav) Date : 21-Jul-2009
        private DataColumn colPaymentProcessor;
        //End Of Added By SRT(Gaurav)
        private DataColumn colCLCouponID;
        private DataColumn colProcessorTransID;//Added By Shitaljit on 19 july 2012 to store Processor TransID
        private DataColumn colIsManual; //Sprint-19 - 2139 06-Jan-2015 JY Added

        private DataColumn colCashBack;
        private DataColumn colAid;
        private DataColumn colAidName;
        private DataColumn colCryptogram;
        private DataColumn colTransCounter;
        private DataColumn colTerminalTvr;
        private DataColumn colTransStatusInfo;
        private DataColumn colAuthorizationCode;
        private DataColumn colTransRefNum;
        private DataColumn colValidateCode;
        private DataColumn colMerchantID;
        private DataColumn colRTransactionID;
        private DataColumn colEntryLegend;
        private DataColumn colEntryMethod;
        private DataColumn colProfiledID;
        private DataColumn colCardType;
        private DataColumn colProcTransType;
        private DataColumn colVerbiage;
        private DataColumn colApprovalCode;

        //Added By Rohit Nair on Sept 08 2016 for WP EMV
        private DataColumn colIssuerAppData;
        private DataColumn colCardVerificationMethod;
        private DataColumn colS3TransID; //  Added for Solutran - PRIMEPOS-2663 - NileshJ

        //Added by Arvind on July 18 2019 for EvertecReceipt PRIMEPOS-2664
        private DataColumn colTraceNumber;
        private DataColumn colBatchNumber;
        private DataColumn colInvoiceNumber;
        private DataColumn colControlNumber;
        private DataColumn colEbtBalance;
        //Added by Arvind PRIMEPOS-2636
        private DataColumn colTerminalID;
        private DataColumn colReferenceNumber;
        private DataColumn colTransactionID;
        private DataColumn colResponseCode;
        #region PRIMEPOS-2793
        private DataColumn colApplicationLabel;
        private DataColumn colPinVerified;
        private DataColumn colLaneID;
        private DataColumn colCardLogo;
        #endregion
        #region PRIMEPOS-2761
        private DataColumn colTicketNumber;
        #endregion
        #region PRIMEPOS-2915
        private DataColumn colPrimeRxPayTransID;
        private DataColumn colStatus;
        private DataColumn colApprovedAmount;
        private DataColumn colEmail;
        private DataColumn colMobile;
        private DataColumn colTransactionProcessingMode;
        #endregion

        private DataColumn colATHMovil;//2664
        private DataColumn colEvertecTaxBreakdown;//2664
        #region primepos-2831
        private DataColumn colIsEvertecForceTransaction;
        private DataColumn colIsEvertecSign;
        #endregion
        private DataColumn colOverrideHousechargeLimitUser; //PRIMEPOS-2402 09-Jul-2021 JY Added
        private DataColumn colMaxTenderedAmountOverrideUser;    //PRIMEPOS-2402 20-Jul-2021 JY Added

        private DataColumn colIsFsaCard;//2990
        private DataColumn colTokenID;//3009
        private DataColumn colNBSTransId; //PRIMEPOS-3375
        private DataColumn colNBSTransUid; //PRIMEPOS-3375
        private DataColumn colNBSPaymentType; //PRIMEPOS-3375
        private DataColumn colTransFeeAmt;  //PRIMEPOS-3117 11-Jul-2022 JY Added
        private DataColumn colTokenize; //PRIMEPOS-3145 28-Sep-2022 JY Added
        private DataColumn colTenderedAmount; //PRIMEPOS-3428

        #region Constants
        private const String _TableName = "POSTransPayment";
		#endregion
		#region Constructors 
		internal POSTransPaymentTable() : base(_TableName) { this.InitClass(); }
		internal POSTransPaymentTable(DataTable table) : base(table.TableName) {}
		#endregion
		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public POSTransPaymentRow this[int index] 
		{
			get 
			{
				return ((POSTransPaymentRow)(this.Rows[index]));
			}
		}

		public DataColumn TransPayID
		{
			get 
			{
				return this.colTransPayId;
			}
		}

        public DataColumn CLCouponID
        {
            get
            {
                return this.colCLCouponID;
            }
        }

		public DataColumn TransTypeCode
		{
			get 
			{
				return this.colTransTypeCode;
			}
		}

		public DataColumn TransTypeDesc 
		{
			get 
			{
				return this.colTransTypeDesc;
			}
		}

		public DataColumn Amount 
		{
			get 
			{
				return this.colAmount;
			}
		}

		public DataColumn HC_Posted 
		{
			get 
			{
				return this.colHC_Posted;
			}
		}

		public DataColumn TransDate 
		{
			get 
			{
				return this.colTransDate;
			}
		}
        public DataColumn ExpiryDate//2943
        {
            get
            {
                return this.colExpiryDate;
            }
        }

        public DataColumn TransID 
		{
			get 
			{
				return this.colTransID;
			}
		}

		public DataColumn RefNo 
		{
			get 
			{
				return this.colRefNo;
			}
		}

		public DataColumn AuthNo 
		{
			get 
			{
				return this.colAuthNo;
			}
		}

		public DataColumn CCName
		{
			get 
			{
				return this.colCCName;
			}
		}

		public DataColumn CCTransNo 
		{
			get 
			{
				return this.colCCTransNo;
			}
		}

        public DataColumn CustomerSign
        {
            get
            {
                return this.colCustomerSign;
            }
        }

        public DataColumn BinarySign
        {
            get
            {
                return this.colBinarySign;
            }
        }

        public DataColumn SigType
        {
            get
            {
                return this.colSigType;
            }
        }

        public DataColumn IsIIASPayment
        {
            get
            {
                return this.colIsIIASPayment;
            }
        }

        //Added By SRT(Gaurav) Date : 21-Jul-2009
        //This column will hold the data regarding payment processor for the current transaction.
        public DataColumn PaymentProcessor
        {
            get
            {
                return this.colPaymentProcessor;
            }
        }
        //End OF Added By SRT(Gaurav)

        //Added By Shitaljit on 19 july 2012 to store Processor TransID
        public DataColumn ProcessorTransID
        {
            get
            {
                return this.colProcessorTransID;
            }
        }

        #region Sprint-19 - 2139 06-Jan-2015 JY Added
        public DataColumn IsManual
        {
            get
            {
                return this.colIsManual;
            }
        }
        #endregion
        public DataColumn CashBack
        {
            get { return this.colCashBack; }
        }
        public DataColumn Aid
        {
            get { return this.colAid; }
        }
        public DataColumn AidName
        {
            get { return this.colAidName; }
        }
        public DataColumn Cryptogram_AC
        {
            get { return this.colCryptogram; }
        }
        public DataColumn TransactionCounter_ATC
        {
            get { return this.colTransCounter; }
        }
        public DataColumn Terminal_Tvr
        {
            get { return this.colTerminalTvr; }
        }
        public DataColumn TransStatusInfo_Tsi
        {
            get { return this.colTransStatusInfo; }
        }
        public DataColumn AuthorizationRespCode_CD
        {
            get { return this.colAuthorizationCode; }
        }
        public DataColumn TransRefNum_Trn
        {
            get { return this.colTransRefNum; }
        }
        public DataColumn ValidateCode_Vc
        {
            get { return this.colValidateCode; }
        }
        public DataColumn MerchantID
        {
            get { return this.colMerchantID; }
        }
        public DataColumn RTransID
        {
            get { return this.colRTransactionID; }
        }
        public DataColumn EntryLegend
        {
            get { return this.colEntryLegend; }
        }
        public DataColumn EntryMethod
        {
            get { return this.colEntryMethod; }
        }
        public DataColumn ProfiledID
        {
            get { return this.colProfiledID; }
        }
        public DataColumn CardType_Ct
        {
            get { return this.colCardType; }
        }
        public DataColumn ProcTransType
        {
            get { return this.colProcTransType; }
        }
        public DataColumn Verbiage
        {
            get { return this.colVerbiage; }
        }

        //added By Rohit Nair On Sept 08 2016 for WP EMV

        public DataColumn IssuerAppData
        {
            get { return this.colIssuerAppData; }
        }


        public DataColumn CardVerificationMethod
        {
            get { return this.colCardVerificationMethod; }
        }

        #region  Added for Solutran - PRIMEPOS-2663 - NileshJ
        public DataColumn S3TransID
        {
            get { return this.colS3TransID; }
        }
        #endregion

        //Added by Arvind on july 18 2019 for EvertecReceipt PRIMEPOS-2664
        public DataColumn InvoiceNumber
        {
            get
            {
                return this.colInvoiceNumber;
            }
        }
        public DataColumn TraceNumber
        {
            get
            {
                return this.colTraceNumber;
            }
        }
        public DataColumn BatchNumber
        {
            get
            {
                return this.colBatchNumber;
            }
        }
        //Added by Arvind PRIMEPOS-2636
        public DataColumn TerminalID
        {
            get
            {
                return this.colTerminalID;
            }
        }
        public DataColumn ReferenceNumber
        {
            get
            {
                return this.colReferenceNumber;
            }
        }
        public DataColumn TransactionID
        {
            get
            {
                return this.colTransactionID;
            }
        }
        public DataColumn ResponseCode
        {
            get
            {
                return this.colResponseCode;
            }
        }
        public DataColumn ApprovalCode
        {
            get
            {
                return this.colApprovalCode;
            }
        }

        #region PRIMEPOS-2793
        public DataColumn ApplicationLabel
        {
            get
            {
                return this.colApplicationLabel;
            }
        }
        public DataColumn PinVerified
        {
            get
            {
                return this.colPinVerified;
            }
        }
        public DataColumn LaneID
        {
            get
            {
                return this.colLaneID;
            }
        }
        public DataColumn CardLogo
        {
            get
            {
                return this.colCardLogo;
            }
        }
        #endregion
        #region PRIMEPOS-2761
        public DataColumn TicketNumber
        {
            get { return this.colTicketNumber; }
        }
        #endregion
        #region PRIMEPOS-2664 ADDED BY ARVIND 
        public DataColumn ControlNumber
        {
            get
            {
                return this.colControlNumber;
            }
        }
        public DataColumn EbtBalance
        {
            get
            {
                return this.colEbtBalance;
            }
        }
        #endregion
        #region PRIMEPOS-2915
        public DataColumn PrimeRxPayTransID
        {
            get
            {
                return this.colPrimeRxPayTransID;
            }
        }

        public DataColumn ApprovedAmount
        {
            get
            {
                return this.colApprovedAmount;
            }
        }

        public DataColumn Status
        {
            get
            {
                return this.colStatus;
            }
        }
        public DataColumn Email
        {
            get
            {
                return this.colEmail;
            }
        }
        public DataColumn Mobile
        {
            get
            {
                return this.colMobile;
            }
        }
        public DataColumn TransactionProcessingMode
        {
            get
            {
                return this.colTransactionProcessingMode;
            }
        }

        #region primepos-2831
        public DataColumn IsEvertecForceTransaction
        {
            get
            {
                return this.colIsEvertecForceTransaction;
            }
        }
        public DataColumn IsEvertecSign
        {
            get
            {
                return this.colIsEvertecSign;
            }
        }
        #endregion
        //PRIMEPOS-2857
        public DataColumn EvertecTaxBreakdown
        {
            get
            {
                return this.colEvertecTaxBreakdown;
            }
        }
        public DataColumn ATHMovil
        {
            get
            {
                return this.colATHMovil;
            }
        }
        #endregion

        #region PRIMEPOS-2402 09-Jul-2021 JY Added
        public DataColumn OverrideHousechargeLimitUser
        {
            get
            {
                return this.colOverrideHousechargeLimitUser;
            }
        }
        public DataColumn MaxTenderedAmountOverrideUser
        {
            get
            {
                return this.colMaxTenderedAmountOverrideUser;
            }
        }
        #endregion
        public DataColumn IsFsaCard//2990
        {
            get
            {
                return this.colIsFsaCard;
            }
        }
        public DataColumn TokenID//3009
        {
            get
            {
                return this.colTokenID;
            }
        }

        public DataColumn NBSTransId//PRIMEPOS-3375
        {
            get
            {
                return this.colNBSTransId;
            }
        }

        public DataColumn NBSTransUid//PRIMEPOS-3375
        {
            get
            {
                return this.colNBSTransUid;
            }
        }

        public DataColumn NBSPaymentType//PRIMEPOS-3375
        {
            get
            {
                return this.colNBSPaymentType;
            }
        }

        public DataColumn TransFeeAmt   //PRIMEPOS-3117 11-Jul-2022 JY Added
        {
            get
            {
                return this.colTransFeeAmt;
            }
        }

        public DataColumn Tokenize  //PRIMEPOS-3145 28-Sep-2022 JY Added
        {
            get
            {
                return this.colTokenize;
            }
        }

        public DataColumn TenderedAmount//PRIMEPOS-3428
        {
            get
            {
                return this.colTenderedAmount;
            }
        }
        #endregion //Properties

        #region Add and Get Methods 
        public void AddRow(POSTransPaymentRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(POSTransPaymentRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.TransPayID) == null) 
			{
				this.Rows.Add(row);
				if(preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

        //Updated By SRT(Gaurav) Date : 21-Jul-2009
        //New parameter introduced strPaymentProcessor. This will assign to 
		public POSTransPaymentRow AddRow( System.Int32 TransPayID
										, System.String TransTypeCode
										, System.String TransTypeDesc
										, System.String RefNo
										, System.Decimal Amount
										, System.Boolean HC_Posted 
										, System.DateTime TransDate 										
                                        , System.Int32  TransID 
										, System.String AuthNo
										, System.String CCName
										, System.String CCTransNo
                                        , System.String CustomerSign
                                        , System.String strPaymentProcessor
                                        , System.Byte[] BinarySign
                                        , System.String SigType
                                        , System.String IsManual
                                        , System.String Aid
                                        , System.String AidName
                                        , System.String Cryptogram
                                        , System.String TransCounter
                                        , System.String TerminalTvr
                                        , System.String TransStatusinfo
                                        , System.String AuthorizeCode
                                        , System.String TransRefNum
                                        , System.String ValidateCode
                                        , System.String MerchantID
                                        , System.String EntryMethod
                                        , System.String EntryLegend
                                        , System.String ProfiledID
                                        , System.String CardType
                                        , System.String ProcTransType
                                        , System.String Verbiage
                                        , System.String RTransID
                                        //PRIMEPOS-2664 FROM HERE ADDED BY ARVIND 
                                        , System.String InvoiceNumber
                                        , System.String BatchNumber
                                        , System.String TraceNumber
                                        //ADDED BY ARVIND PRIMEPOS-2636
                                        , System.String TerminalID
                                        , System.String ReferenceNumber
                                        , System.String TransactionID
                                        , System.String ResponseCode
                                        , System.String ApprovalCode
            //TILL HERE
            //PRIMEPOS-2664 ADDED BY ARVIND
            , System.String ControlNumber
            //PRIMEPOS-2786 EVERTEC EBTBALANCE
            , System.String EbtBalance
             //
             //PRIMEPOS-2793 VANTIV
             , System.String ApplicationLabel
             , System.Boolean PinVerified
            , System.String LaneID
            , System.String CardLogo
            , System.DateTime ExpDate//2943
            //
            //, System.String IssuerAppData
            //, System.String CardVerificationMethod

            , System.Boolean IsFsaCard//2990
            , System.Decimal TransFeeAmt    //PRIMEPOS-3117 11-Jul-2022 JY Added
            )
        {
			POSTransPaymentRow row = (POSTransPaymentRow)this.NewRow();
			row.TransPayID=TransPayID;
			row.TransTypeDesc=TransTypeDesc;
			row.TransTypeCode=TransTypeCode;
			row.RefNo=RefNo;
			row.Amount=Amount;
			row.HC_Posted=HC_Posted;
			row.TransDate=TransDate;			
			row.TransID=TransID;
			row.AuthNo=AuthNo;
			row.CCName=CCName;
			row.CCTransNo=CCTransNo;
            row.CustomerSign = CustomerSign;
            row.PaymentProcessor = strPaymentProcessor;
            row.BinarySign = BinarySign;
            row.SigType = SigType;
            row.CLCouponID = 0;
            row.IsManual = IsManual;    //Sprint-19 - 2139 06-Jan-2015 JY Added
            row.Aid = Aid;
            row.AidName = AidName;
            row.Cryptogram = Cryptogram;
            row.TransCounter = TransCounter;
            row.TerminalTvr = TerminalTvr;
            row.TransStatusInfo = TransStatusinfo;
            row.AuthorizeRespCode = AuthorizeCode;
            row.TransRefNum = TransRefNum;
            row.ValidateCode = ValidateCode;
            row.MerchantID = MerchantID;
            row.EntryMethod = EntryMethod;
            row.EntryLegend = EntryLegend;
            row.ProfiledID = ProfiledID;
            row.CardType = CardType;
            row.ProcTransType = ProcTransType;
            row.Verbiage = Verbiage;
            row.RTransID = RTransID;
            //
            //PRIMEPOS-2664 ADDED BY ARVIND FROM HERE
            row.InvoiceNumber = InvoiceNumber;
            row.BatchNumber = BatchNumber;
            row.TraceNumber = TraceNumber;
            //Added by Arvind PRIMEPOS-2636        
            row.TerminalID = TerminalID;
            row.ReferenceNumber = ReferenceNumber;
            row.TransactionID = TransactionID;
            row.ResponseCode = ResponseCode;
            row.ApprovalCode = ApprovalCode;
            //
            row.ControlNumber = ControlNumber;
            //TILL HERE
            #region PRIMEPOS-2786
            row.EbtBalance = EbtBalance;
            #endregion
            // row.IssuerAppData = IssuerAppData;
            //row.CardVerificationMethod = CardVerificationMethod;

            //PRIMEPOS-2793
            row.ApplicaionLabel = ApplicationLabel;
            row.PinVerified = PinVerified;
            row.LaneID = LaneID;
            row.CardLogo = CardLogo;
            //
            row.ExpiryDate = ExpDate;//2943
            row.IsFsaCard = IsFsaCard;//2990
            row.TransFeeAmt = TransFeeAmt;  //PRIMEPOS-3117 11-Jul-2022 JY Added
            this.Rows.Add(row);
			return row;
		}

		public POSTransPaymentRow GetRowByID(System.Int32 TransPayID) 
		{
			return (POSTransPaymentRow)this.Rows.Find(new object[] {TransPayID});
		}

		public  void MergeTable(DataTable dt) 
		{
      
            POSTransPaymentRow row;

			foreach(DataRow dr in dt.Rows) 
            {
                row = (POSTransPaymentRow)this.NewRow();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransPayID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransPayID] = DBNull.Value;
                else
					row[clsPOSDBConstants.POSTransPayment_Fld_TransPayID] = Convert.ToInt32((dr[clsPOSDBConstants.POSTransPayment_Fld_TransPayID].ToString()=="")?"0":dr[clsPOSDBConstants.POSTransPayment_Fld_TransPayID].ToString());

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode] = Convert.ToString(dr[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString());

                if (dr[clsPOSDBConstants.PayType_Fld_PayTypeDescription] == DBNull.Value)
                    row[clsPOSDBConstants.PayType_Fld_PayTypeDescription] = DBNull.Value;
                else
                    row[clsPOSDBConstants.PayType_Fld_PayTypeDescription] = Convert.ToString(dr[clsPOSDBConstants.PayType_Fld_PayTypeDescription].ToString());

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_Amount] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_Amount] = DBNull.Value;
                else
					row[clsPOSDBConstants.POSTransPayment_Fld_Amount] = Convert.ToDecimal((dr[clsPOSDBConstants.POSTransPayment_Fld_Amount].ToString()=="")? "0":dr[clsPOSDBConstants.POSTransPayment_Fld_Amount].ToString());

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_HC_Posted] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_HC_Posted] = DBNull.Value;
                else
					row[clsPOSDBConstants.POSTransPayment_Fld_HC_Posted] = Convert.ToBoolean((dr[clsPOSDBConstants.POSTransPayment_Fld_HC_Posted].ToString()=="")? "false":dr[clsPOSDBConstants.POSTransPayment_Fld_HC_Posted].ToString());

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransDate] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransDate] = DBNull.Value;
                else
					if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransDate].ToString().Trim()=="") 
						row[clsPOSDBConstants.POSTransPayment_Fld_TransDate]= Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
						row[clsPOSDBConstants.POSTransPayment_Fld_TransDate]= Convert.ToDateTime(dr[clsPOSDBConstants.POSTransPayment_Fld_TransDate].ToString());

				string strField=clsPOSDBConstants.POSTransPayment_Fld_TransID	;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
					row[strField] = Convert.ToInt32((dr[strField].ToString()=="")? "0":dr[strField].ToString());

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_RefNo] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_RefNo] = DBNull.Value;
                else
					row[clsPOSDBConstants.POSTransPayment_Fld_RefNo] = ((dr[clsPOSDBConstants.POSTransPayment_Fld_RefNo].ToString()=="")? "":dr[clsPOSDBConstants.POSTransPayment_Fld_RefNo].ToString());

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_CCName] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_CCName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_CCName] = dr[clsPOSDBConstants.POSTransPayment_Fld_CCName].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo] = dr[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo].ToString();

				strField=clsPOSDBConstants.POSTransPayment_Fld_AuthNo;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.POSTransPayment_Fld_CustomerSign;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                //Added By SRT(Gaurav) Date : 21-07-2009
                //here null value for payment processor is handeled with noprocessor of clsposdbconstant.
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor] = clsPOSDBConstants.NOPROCESSOR;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor] = dr[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor].ToString();

                strField = clsPOSDBConstants.POSTransPayment_Fld_BinarySign;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = (byte[])dr[strField];

                strField = clsPOSDBConstants.POSTransPayment_Fld_SigType;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();
                //End Of Added By SRT(Gaurav)

                strField = clsPOSDBConstants.POSTransPayment_Fld_CLCouponID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                #region Sprint-19 - 2139 06-Jan-2015 JY Added
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_IsManual] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_IsManual] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_IsManual] = dr[clsPOSDBConstants.POSTransPayment_Fld_IsManual].ToString();
                #endregion

                #region EMV
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_AID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_AID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_AID] = dr[clsPOSDBConstants.POSTransPayment_Fld_AID].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_AIDNAME] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_AIDNAME] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_AIDNAME] = dr[clsPOSDBConstants.POSTransPayment_Fld_AIDNAME].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_CRYTOGRAM] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_CRYTOGRAM] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_CRYTOGRAM] = dr[clsPOSDBConstants.POSTransPayment_Fld_CRYTOGRAM].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransactionCounter] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransactionCounter] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransactionCounter] = dr[clsPOSDBConstants.POSTransPayment_Fld_TransactionCounter].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TerminalTVR] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TerminalTVR] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TerminalTVR] = dr[clsPOSDBConstants.POSTransPayment_Fld_TerminalTVR].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransStatusInfo] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransStatusInfo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransStatusInfo] = dr[clsPOSDBConstants.POSTransPayment_Fld_TransStatusInfo].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_AuthorizationResponse] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_AuthorizationResponse] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_AuthorizationResponse] = dr[clsPOSDBConstants.POSTransPayment_Fld_AuthorizationResponse].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransRefNum] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransRefNum] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransRefNum] = dr[clsPOSDBConstants.POSTransPayment_Fld_TransRefNum].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ValidateCode] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ValidateCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ValidateCode] = dr[clsPOSDBConstants.POSTransPayment_Fld_ValidateCode].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_MerchantID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_MerchantID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_MerchantID] = dr[clsPOSDBConstants.POSTransPayment_Fld_MerchantID].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_EntryLegend] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_EntryLegend] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_EntryLegend] = dr[clsPOSDBConstants.POSTransPayment_Fld_EntryLegend].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_EntryMethod] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_EntryMethod] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_EntryMethod] = dr[clsPOSDBConstants.POSTransPayment_Fld_EntryMethod].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ProfiledID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ProfiledID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ProfiledID] = dr[clsPOSDBConstants.POSTransPayment_Fld_ProfiledID].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_CardType] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_CardType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_CardType] = dr[clsPOSDBConstants.POSTransPayment_Fld_CardType].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ProcTransType] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ProcTransType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ProcTransType] = dr[clsPOSDBConstants.POSTransPayment_Fld_ProcTransType].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_Verbiage] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_Verbiage] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_Verbiage] = dr[clsPOSDBConstants.POSTransPayment_Fld_Verbiage].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_RTransactionID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_RTransactionID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_RTransactionID] = dr[clsPOSDBConstants.POSTransPayment_Fld_RTransactionID].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_IssuerAppData] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_IssuerAppData] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_IssuerAppData] = dr[clsPOSDBConstants.POSTransPayment_Fld_IssuerAppData].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_CardVerificationMethod] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_CardVerificationMethod] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_CardVerificationMethod] = dr[clsPOSDBConstants.POSTransPayment_Fld_CardVerificationMethod].ToString();

                #endregion EMV

                //Added by Arvind PRIMEPOS-2664
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_InvoiceNumber] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_InvoiceNumber] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_InvoiceNumber] = dr[clsPOSDBConstants.POSTransPayment_Fld_InvoiceNumber].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID] = dr[clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_BatchNumber] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_BatchNumber] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_BatchNumber] = dr[clsPOSDBConstants.POSTransPayment_Fld_BatchNumber].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TraceNumber] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TraceNumber] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TraceNumber] = dr[clsPOSDBConstants.POSTransPayment_Fld_TraceNumber].ToString();
                //
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ControlNumber] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ControlNumber] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ControlNumber] = dr[clsPOSDBConstants.POSTransPayment_Fld_ControlNumber].ToString();
                //
                #region PRIMEPOS-2786 EVERTEC EBTBALANCE
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_EbtBalance] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_EbtBalance] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_EbtBalance] = dr[clsPOSDBConstants.POSTransPayment_Fld_EbtBalance].ToString();
                #endregion
                //ADDED BY ARVIND PRIMEPOS-2636
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TerminalID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TerminalID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TerminalID] = dr[clsPOSDBConstants.POSTransPayment_Fld_TerminalID].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ReferenceNumber] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ReferenceNumber] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ReferenceNumber] = dr[clsPOSDBConstants.POSTransPayment_Fld_ReferenceNumber].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransactionID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransactionID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransactionID] = dr[clsPOSDBConstants.POSTransPayment_Fld_TransactionID].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ResponseCode] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ResponseCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ResponseCode] = dr[clsPOSDBConstants.POSTransPayment_Fld_ResponseCode].ToString();

                #region PRIMEPOS-2793
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ApplicationLabel] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ApplicationLabel] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ApplicationLabel] = dr[clsPOSDBConstants.POSTransPayment_Fld_ApplicationLabel].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ApprovalCode] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ApprovalCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ApprovalCode] = dr[clsPOSDBConstants.POSTransPayment_Fld_ApprovalCode].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_PinVerified] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_PinVerified] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_PinVerified] = dr[clsPOSDBConstants.POSTransPayment_Fld_PinVerified].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_LaneID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_LaneID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_LaneID] = dr[clsPOSDBConstants.POSTransPayment_Fld_LaneID].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_CardLogo] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_CardLogo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_CardLogo] = dr[clsPOSDBConstants.POSTransPayment_Fld_CardLogo].ToString();
                #endregion

                #region PRIMEPOS-2892 24-Sep-2020 JY Added
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TicketNumber] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TicketNumber] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TicketNumber] = dr[clsPOSDBConstants.POSTransPayment_Fld_TicketNumber].ToString();
                #endregion

                #region PRIMEPOS-2915
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID] = dr[clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount] = dr[clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_Status] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_Status] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_Status] = dr[clsPOSDBConstants.POSTransPayment_Fld_Status].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_Mobile] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_Mobile] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_Mobile] = dr[clsPOSDBConstants.POSTransPayment_Fld_Mobile].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_Email] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_Email] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_Email] = dr[clsPOSDBConstants.POSTransPayment_Fld_Email].ToString();
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode] = dr[clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode].ToString();
                #endregion

                #region PRIMEPOS-2857
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_EvertecTaxBreakdown] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_EvertecTaxBreakdown] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_EvertecTaxBreakdown] = dr[clsPOSDBConstants.POSTransPayment_Fld_EvertecTaxBreakdown].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_CashBack] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_CashBack] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_CashBack] = dr[clsPOSDBConstants.POSTransPayment_Fld_CashBack].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_IsEvertecForceTransaction] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_IsEvertecForceTransaction] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_IsEvertecForceTransaction] = dr[clsPOSDBConstants.POSTransPayment_Fld_IsEvertecForceTransaction].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_IsEvertecSign] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_IsEvertecSign] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_IsEvertecSign] = dr[clsPOSDBConstants.POSTransPayment_Fld_IsEvertecSign].ToString();

                #endregion

                //2664
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_ATHMovil] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_ATHMovil] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_ATHMovil] = dr[clsPOSDBConstants.POSTransPayment_Fld_ATHMovil].ToString();

                //PRIMEPOS-3117 11-Jul-2022 JY Added
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt] = Convert.ToDecimal((dr[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt].ToString() == "") ? "0" : dr[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt].ToString());

                //PRIMEPOS-3145 28-Sep-2022 JY Added
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_Tokenize] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_Tokenize] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_Tokenize] = dr[clsPOSDBConstants.POSTransPayment_Fld_Tokenize].ToString();

                #region PRIMEPOS-3375
                if (dr[clsPOSDBConstants.POSTransPayment_Fld_NBSTransId] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_NBSTransId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_NBSTransId] = dr[clsPOSDBConstants.POSTransPayment_Fld_NBSTransId].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid] = dr[clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid].ToString();

                if (dr[clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType] == DBNull.Value)
                    row[clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType] = dr[clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType].ToString();
                #endregion

                #region PRIMEPOS-3428
                if (dt.Columns.Contains(clsPOSDBConstants.TransHeader_Fld_TenderedAmount))
                {
                    if (dr[clsPOSDBConstants.TransHeader_Fld_TenderedAmount] == DBNull.Value)
                        row[clsPOSDBConstants.TransHeader_Fld_TenderedAmount] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransHeader_Fld_TenderedAmount] = Convert.ToString(dr[clsPOSDBConstants.TransHeader_Fld_TenderedAmount].ToString()); 
                }
                #endregion

                this.AddRow(row);
            }
        }
		
		#endregion 
		public override DataTable Clone() 
		{
			POSTransPaymentTable cln = (POSTransPaymentTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new POSTransPaymentTable();
		}

		internal void InitVars() 
		{
			this.colTransPayId = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransPayID];
			this.colTransTypeCode = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode];
			this.colTransTypeDesc = this.Columns[clsPOSDBConstants.PayType_Fld_PayTypeDescription];
			this.colAmount = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_Amount];
			this.colHC_Posted = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_HC_Posted];
			this.colTransDate = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransDate];
			this.colExpiryDate = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_ExpDate];//2943
            this.colTransID= this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransID];
			this.colRefNo= this.Columns[clsPOSDBConstants.POSTransPayment_Fld_RefNo];
			this.colAuthNo= this.Columns[clsPOSDBConstants.POSTransPayment_Fld_AuthNo];
			this.colCCName= this.Columns[clsPOSDBConstants.POSTransPayment_Fld_CCName];
			this.colCCTransNo= this.Columns[clsPOSDBConstants.POSTransPayment_Fld_CCTransNo];
            this.colCustomerSign= this.Columns[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign];
            this.colIsIIASPayment = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment];

            this.colBinarySign = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_BinarySign];
            this.colSigType = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_SigType];
            //Added By SRT(Gaurav) Date : 21-Jul-2009
            this.colPaymentProcessor = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor];
            //End Of Added By SRT(Gaurav)
            //Added By Shitaljit on 19 july 2012 to store Processor TransID
            this.colProcessorTransID = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor];
            this.colCLCouponID = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_CLCouponID];
            this.colIsManual = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_IsManual];    //Sprint-19 - 2139 06-Jan-2015 JY Added
            this.colCashBack = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_CashBack];
            this.colS3TransID = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_S3TransID]; // Solutran - PRIMEPOS-2663
            #region Emv
            this.colAid = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_AID];
            this.colAidName = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_AIDNAME];
            this.colCryptogram = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_CRYTOGRAM];
            this.colTransCounter = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransactionCounter];
            this.colTerminalTvr = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TerminalTVR];
            this.colTransStatusInfo = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransStatusInfo];
            this.colAuthorizationCode = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_AuthorizationResponse];
            this.colTransRefNum = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransRefNum];
            this.colValidateCode = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_ValidateCode]; 
            this.colMerchantID = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_MerchantID]; 
            this.colEntryLegend = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_EntryLegend];
            this.colEntryMethod = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_EntryMethod];
            this.colProfiledID = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_ProfiledID];
            this.colCardType = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_CardType];
            this.colProcTransType = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_ProcTransType];
            this.colVerbiage = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_ValidateCode];
            this.colRTransactionID = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_RTransactionID];

            this.colIssuerAppData = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_IssuerAppData];
            this.colCardVerificationMethod = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_CardVerificationMethod];

            #region Added by Arvind for Evertec Receipt  PRIMEPOS-2664

            this.colBatchNumber = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_BatchNumber];
            this.colTraceNumber = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TraceNumber];
            this.colInvoiceNumber = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_InvoiceNumber];

            #endregion
            #endregion Emv
            #region PRIMEPOS-2761
            this.colTicketNumber = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TicketNumber];    //PRIMEPOS-2892 24-Sep-2020 JY modified
            #endregion

            this.colPrimeRxPayTransID = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID];//2915
            this.colApprovedAmount = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount];//2915
            this.colStatus = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_Status];//2915
            this.colEmail = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_Email];//2915
            this.colMobile = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_Mobile];//2915
            this.colTransactionProcessingMode = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode];//2915
            this.colOverrideHousechargeLimitUser = this.Columns[clsPOSDBConstants.PosTransPayment_Status_OverrideHousechargeLimitUser];    //PRIMEPOS-2402 09-Jul-2021 JY Added
            this.colMaxTenderedAmountOverrideUser = this.Columns[clsPOSDBConstants.PosTransPayment_Status_MaxTenderedAmountOverrideUser];    //PRIMEPOS-2402 20-Jul-2021 JY Added

            this.colIsFsaCard = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_IsFsaCard];//2990
            this.colTokenID = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TokenID];//3009
            this.colTransFeeAmt = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt];  //PRIMEPOS-3117 11-Jul-2022 JY Added
            this.colTokenize = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_Tokenize];    //PRIMEPOS-2402 20-Jul-2021 JY Added
            this.colNBSTransId = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_NBSTransId];//PRIMEPOS-3375
            this.colNBSTransUid = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid];//PRIMEPOS-3375
            this.colNBSPaymentType = this.Columns[clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType];//PRIMEPOS-3375
            this.colTenderedAmount = this.Columns[clsPOSDBConstants.TransHeader_Fld_TenderedAmount]; //PRIMEPOS-3428
        }

        public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colTransPayId = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransPayID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransPayId);
			this.colTransPayId.AllowDBNull = true;

			this.colTransTypeCode = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransTypeCode);
			this.colTransTypeCode.AllowDBNull = false;

			this.colTransTypeDesc = new DataColumn(clsPOSDBConstants.PayType_Fld_PayTypeDescription, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransTypeDesc);
			this.colTransTypeDesc.AllowDBNull = true;

			this.colAmount = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_Amount, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colAmount);
			this.colAmount.AllowDBNull = true;

			this.colHC_Posted = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_HC_Posted, typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colHC_Posted);
			this.colHC_Posted.AllowDBNull = true;

			this.colTransDate = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransDate,typeof(System.DateTime), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransDate);
			this.colTransDate.AllowDBNull = true;

            //2943
            this.colExpiryDate = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ExpDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExpiryDate);
            this.colExpiryDate.AllowDBNull = true;

            this.colTransID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransID,typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransID);
			this.colTransID.AllowDBNull = true;

			this.colRefNo = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_RefNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRefNo);
			this.colRefNo.AllowDBNull = true;

			this.colAuthNo = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_AuthNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colAuthNo);
			this.colAuthNo.AllowDBNull = true;

			this.colCCName = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CCName, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCCName);
			this.colCCName.AllowDBNull = true;

			this.colCCTransNo = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CCTransNo, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colCCTransNo);
			this.colCCTransNo.AllowDBNull = true;

            this.colCustomerSign = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CustomerSign, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerSign);
            this.CustomerSign.AllowDBNull = true;

            this.colIsIIASPayment = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment , typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsIIASPayment);
            this.colIsIIASPayment.AllowDBNull = true;

            //Added By SRT(Gaurav) Date : 21-Jul-2009
            this.colPaymentProcessor = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPaymentProcessor);
            this.colPaymentProcessor.AllowDBNull = true;

            this.colBinarySign = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_BinarySign, typeof(System.Byte[]), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colBinarySign);
            this.BinarySign.AllowDBNull = true;

            this.colSigType = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_SigType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSigType);
            this.SigType.AllowDBNull = true;
            //End Of Added By SRT(Gaurav)
            //Added By Shitaljit on 19 july 2012 to store Processor TransID
            this.colProcessorTransID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProcessorTransID);
            this.SigType.AllowDBNull = true;

            this.colCLCouponID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CLCouponID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCLCouponID);
            this.colCLCouponID.AllowDBNull = true;

            #region Sprint-19 - 2139 06-Jan-2015 JY Added
            this.colIsManual = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_IsManual, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsManual);
            this.colIsManual.AllowDBNull = true;
            #endregion
            this.colCashBack = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CashBack, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCashBack);
            this.colCashBack.AllowDBNull = true;

            //Solutran - PRIMEPOS-2663
            this.colS3TransID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_S3TransID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3TransID);
            this.colS3TransID.AllowDBNull = true;

            #region EMV
            this.colAid = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_AID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAid);
            this.colAid.AllowDBNull = true;

            this.colAidName = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_AIDNAME, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAidName);
            this.colAidName.AllowDBNull = true;

            this.colCryptogram = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CRYTOGRAM, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCryptogram);
            this.colCryptogram.AllowDBNull = true;

            this.colTransCounter = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransactionCounter, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransCounter);
            this.colTransCounter.AllowDBNull = true;

            this.colTerminalTvr = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TerminalTVR, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTerminalTvr);
            this.colTerminalTvr.AllowDBNull = true;

            this.colTransStatusInfo = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransStatusInfo, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransStatusInfo);
            this.colTransStatusInfo.AllowDBNull = true;

            this.colAuthorizationCode = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_AuthorizationResponse, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAuthorizationCode);
            this.colAuthorizationCode.AllowDBNull = true;

            this.colTransRefNum = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransRefNum, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransRefNum);
            this.colTransRefNum.AllowDBNull = true;

            this.colValidateCode = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ValidateCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colValidateCode);
            this.colValidateCode.AllowDBNull = true;

            this.colMerchantID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_MerchantID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMerchantID);
            this.colMerchantID.AllowDBNull = true;

            this.colEntryLegend = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_EntryLegend, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEntryLegend);
            this.colEntryLegend.AllowDBNull = true;

            this.colEntryMethod = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_EntryMethod, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEntryMethod);
            this.colEntryMethod.AllowDBNull = true;

            this.colProfiledID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ProfiledID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProfiledID);
            this.colProfiledID.AllowDBNull = true;

            this.colCardType = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CardType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCardType);
            this.colCardType.AllowDBNull = true;

            this.colProcTransType = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ProcTransType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProcTransType);
            this.colProcTransType.AllowDBNull = true;

            this.colVerbiage = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_Verbiage, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVerbiage);
            this.colVerbiage.AllowDBNull = true;

            this.colRTransactionID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_RTransactionID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRTransactionID);
            this.colRTransactionID.AllowDBNull = true;



            this.colIssuerAppData = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_IssuerAppData, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIssuerAppData);
            this.colIssuerAppData.AllowDBNull = true;

            this.colCardVerificationMethod = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CardVerificationMethod, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCardVerificationMethod);
            this.colCardVerificationMethod.AllowDBNull = true;



            #endregion EMV
            #region Added by Arvind for Evertec Receipt PRIMEPOS-2664

            this.colBatchNumber = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_BatchNumber, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colBatchNumber);
            this.colBatchNumber.AllowDBNull = true;

            this.colTraceNumber = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TraceNumber, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTraceNumber);
            this.colTraceNumber.AllowDBNull = true;

            this.colInvoiceNumber = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_InvoiceNumber, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colInvoiceNumber);
            this.colInvoiceNumber.AllowDBNull = true;

            #endregion
            #region Added by Arvind for VANTIV Receipt PRIMEPOS-2636

            this.colTerminalID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TerminalID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTerminalID);
            this.SigType.AllowDBNull = true;

            this.colReferenceNumber = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ReferenceNumber, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colReferenceNumber);
            this.SigType.AllowDBNull = true;

            this.colTransactionID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransactionID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransactionID);
            this.SigType.AllowDBNull = true;

            this.colResponseCode = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ResponseCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colResponseCode);
            this.SigType.AllowDBNull = true;

            this.colApprovalCode = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ApprovalCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colApprovalCode);
            this.SigType.AllowDBNull = true;

            #endregion

            #region  PRIMEPOS-2761
            this.colTicketNumber = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TicketNumber, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTicketNumber);
            this.colTicketNumber.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-2664 AND PRIMEPOS-2786
            this.colControlNumber = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ControlNumber, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colControlNumber);
            this.SigType.AllowDBNull = true;

            this.colEbtBalance = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_EbtBalance, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEbtBalance);
            this.SigType.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-2793
            this.colApplicationLabel = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ApplicationLabel, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colApplicationLabel);
            this.SigType.AllowDBNull = true;

            this.colPinVerified = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_PinVerified, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPinVerified);
            this.SigType.AllowDBNull = true;

            this.colLaneID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_LaneID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLaneID);
            this.SigType.AllowDBNull = true;

            this.colCardLogo = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_CardLogo, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCardLogo);
            this.SigType.AllowDBNull = true;
            #endregion

            this.colPrimeRxPayTransID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID, typeof(System.String), null, System.Data.MappingType.Element);//2915
            this.Columns.Add(this.colPrimeRxPayTransID);
            this.colPrimeRxPayTransID.AllowDBNull = true;

            this.colApprovedAmount = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);//2915
            this.Columns.Add(this.colApprovedAmount);
            this.colApprovedAmount.AllowDBNull = true;

            this.colStatus = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_Status, typeof(System.String), null, System.Data.MappingType.Element);//2915
            this.Columns.Add(this.colStatus);
            this.colStatus.AllowDBNull = true;

            this.colEmail = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_Email, typeof(System.String), null, System.Data.MappingType.Element);//2915
            this.Columns.Add(this.colEmail);
            this.colEmail.AllowDBNull = true;

            this.colMobile = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_Mobile, typeof(System.String), null, System.Data.MappingType.Element);//2915
            this.Columns.Add(this.colMobile);
            this.colMobile.AllowDBNull = true;

            this.colTransactionProcessingMode = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode, typeof(System.Int32), null, System.Data.MappingType.Element);//2915
            this.Columns.Add(this.colTransactionProcessingMode);
            this.colTransactionProcessingMode.AllowDBNull = true;

            #region primepos-2831
            this.colIsEvertecForceTransaction = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_IsEvertecForceTransaction, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsEvertecForceTransaction);
            this.colIsEvertecForceTransaction.AllowDBNull = true;

            this.colIsEvertecSign = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_IsEvertecSign, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsEvertecSign);
            this.colIsEvertecSign.AllowDBNull = true;
            #endregion
            this.colEvertecTaxBreakdown = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_EvertecTaxBreakdown, typeof(System.String), null, System.Data.MappingType.Element);//PRIMEPOS-2857
            this.Columns.Add(this.colEvertecTaxBreakdown);
            this.colIsEvertecSign.AllowDBNull = true;

            //2664
            this.colATHMovil = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_ATHMovil, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colATHMovil);
            this.colATHMovil.AllowDBNull = true;

            #region PRIMEPOS-2402 08-Jul-2021 JY Added
            this.colOverrideHousechargeLimitUser = new DataColumn(clsPOSDBConstants.PosTransPayment_Status_OverrideHousechargeLimitUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOverrideHousechargeLimitUser);
            this.colOverrideHousechargeLimitUser.AllowDBNull = true;
            this.colMaxTenderedAmountOverrideUser = new DataColumn(clsPOSDBConstants.PosTransPayment_Status_MaxTenderedAmountOverrideUser, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMaxTenderedAmountOverrideUser);
            this.colMaxTenderedAmountOverrideUser.AllowDBNull = true;
            #endregion

            //2990
            this.colIsFsaCard = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_IsFsaCard, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsFsaCard);
            this.colIsFsaCard.AllowDBNull = true;

            //3009
            this.colTokenID = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TokenID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTokenID);
            this.colTokenID.AllowDBNull = true;

            //PRIMEPOS-3117 11-Jul-2022 JY Added
            this.colTransFeeAmt = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransFeeAmt);
            this.colTransFeeAmt.AllowDBNull = true;

            //PRIMEPOS-3145 28-Sep-2022 JY Added
            this.colTokenize = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_Tokenize, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTokenize);
            this.colTokenize.AllowDBNull = true;

            //PRIMEPOS-3375
            this.colNBSTransId = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_NBSTransId, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNBSTransId);
            this.colNBSTransId.AllowDBNull = true;

            //PRIMEPOS-3375
            this.colNBSTransUid = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNBSTransUid);
            this.colNBSTransUid.AllowDBNull = true;

            //PRIMEPOS-3375
            this.colNBSPaymentType = new DataColumn(clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNBSPaymentType);
            this.colNBSPaymentType.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] {this.colTransPayId};

            //PRIMEPOS-3428
            this.colTenderedAmount = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TenderedAmount, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTenderedAmount);
            this.colTenderedAmount.AllowDBNull = true;
        }

		public  POSTransPaymentRow NewPOSTransPaymentRow() 
		{
            int TransPayID = 1;
            foreach (POSTransPaymentRow oRow in this.Rows)
            {
                if (oRow.TransPayID >= TransPayID)
                {
                    TransPayID = oRow.TransPayID + 1;
                }
            }
			POSTransPaymentRow oNewRow =(POSTransPaymentRow)this.NewRow();
            oNewRow.TransPayID = TransPayID;
            return oNewRow;

		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new POSTransPaymentRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(POSTransPaymentRow);
		}

        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
        }

        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
        }
	}
}
