namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Rows;

    public class TransHeaderTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colTransID;
        private DataColumn colReturnTransID;
        private DataColumn colTransDate;
        //Following column Added by Krishna on 2 June 2011
        private DataColumn colTransactionStartDate;
        //Till here Added by Krishna on 2 June 2011

        private DataColumn colCustomerID;
        private DataColumn colCustomerCode;
        private DataColumn colStationID;
        private DataColumn colCustomerName;

        private DataColumn colTransType;
        private DataColumn colGrossTotal;
        private DataColumn colTotalDiscAmount;
        private DataColumn colTotalTaxAmount;
        private DataColumn colTenderedAmount;
        private DataColumn colTotalPaid;
        private DataColumn colisStationClosed;
        private DataColumn colisEOD;
        private DataColumn colDrawerNo;
        private DataColumn colStClosedID;
        private DataColumn colEODID;
        private DataColumn colUserID;
        private DataColumn colAcc_No;
        private DataColumn colIsDelivery;
        private DataColumn colLoyaltyPoints;
        private DataColumn colDeliveryAddress;
        private DataColumn colWasonHold;    //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
        private DataColumn colDeliverySigSkipped;   //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added

        //Added By Shitaljit(QuicSolv) on 23 August 2011
        private DataColumn colInvoiceDiscount;
        // Till Here Added By Shitaljit(QuicSolv) on 23 August 2011

        #region  Added for Solutran - PRIMEPOS-2663 - NileshJ
        private DataColumn colS3TransID;
        private DataColumn colS3PurAmount;
        private DataColumn colS3TaxAmount;
        private DataColumn colS3DiscountAmount;
        #endregion

        private DataColumn colAllowRxPicked;    //PRIMEPOS-2865 16-Jul-2020 JY Added
        private DataColumn colIsCustomerDriven;//PRIMEPOS-2915     
        private DataColumn colRxTaxPolicyID;    //PRIMEPOS-3053 08-Feb-2021 JY Added
        private DataColumn colTotalTransFeeAmt; //PRIMEPOS-3117 11-Jul-2022 JY Added

        #region Constructors

        internal TransHeaderTable() : base(clsPOSDBConstants.TransHeader_tbl) { this.InitClass(); }

        internal TransHeaderTable(DataTable table) : base(table.TableName) { }

        #endregion Constructors

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public TransHeaderRow this[int index]
        {
            get
            {
                return ((TransHeaderRow) (this.Rows[index]));
            }
        }

        public DataColumn TransID
        {
            get
            {
                return this.colTransID;
            }
            set
            {
                this.colTransID = value;
            }
        }

        public DataColumn ReturnTransID
        {
            get
            {
                return this.colReturnTransID;
            }
            set
            {
                this.colReturnTransID = value;
            }
        }

        public DataColumn TransDate
        {
            get
            {
                return this.colTransDate;
            }
        }

        //Following Code Added by Krishna on 2 June 2011
        public DataColumn TransactionStartDate
        {
            get
            {
                return this.colTransactionStartDate;
            }
        }

        //Till here Added by Krishna on 2 June 2011

        public DataColumn CustomerID
        {
            get
            {
                return this.colCustomerID;
            }
        }

        public DataColumn CustomerCode
        {
            get
            {
                return this.colCustomerCode;
            }
        }

        public DataColumn StationID
        {
            get
            {
                return this.colStationID;
            }
        }

        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
        }

        public DataColumn CustomerName
        {
            get
            {
                return this.colCustomerName;
            }
        }

        public DataColumn TransType
        {
            get
            {
                return this.colTransType;
            }
        }

        public DataColumn GrossTotal
        {
            get
            {
                return this.colGrossTotal;
            }
        }

        public DataColumn TotalDiscAmount
        {
            get
            {
                return this.colTotalDiscAmount;
            }
        }

        public DataColumn DrawerNo
        {
            get
            {
                return this.colDrawerNo;
            }
        }

        public DataColumn TotalTaxAmount
        {
            get
            {
                return this.colTotalTaxAmount;
            }
        }

        public DataColumn TenderedAmount
        {
            get
            {
                return this.colTenderedAmount;
            }
        }

        public DataColumn TotalPaid
        {
            get
            {
                return this.colTotalPaid;
            }
        }

        public DataColumn isStationClosed
        {
            get
            {
                return this.colisStationClosed;
            }
        }

        public DataColumn IsDelivery
        {
            get
            {
                return this.colIsDelivery;
            }
        }

        public DataColumn isEOD
        {
            get
            {
                return this.colisEOD;
            }
        }

        public DataColumn StClosedID
        {
            get
            {
                return this.colStClosedID;
            }
        }

        public DataColumn EODID
        {
            get
            {
                return this.colEODID;
            }
        }

        public DataColumn Acc_No
        {
            get
            {
                return this.colAcc_No;
            }
        }

        public DataColumn LoyaltyPoints
        {
            get
            {
                return this.colLoyaltyPoints;
            }
        }

        //Added By Shitaljit(QuicSolv) on 23 August 2011
        public DataColumn InvoiceDiscount
        {
            get
            {
                return this.colInvoiceDiscount;
            }
        }

        //Till Here Added By Shitaljit(QuicSolv) on 23 August 2011

        public DataColumn DeliveryAddress
        {
            get
            {
                return this.colDeliveryAddress;
            }
        }

        #region Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
        public DataColumn WasonHold
        {
            get
            {
                return this.colWasonHold;
            }
        }

        public DataColumn DeliverySigSkipped
        {
            get
            {
                return this.colDeliverySigSkipped;
            }
        }
        #endregion

        #region  Added for Solutran - PRIMEPOS-2663 - NileshJ
        public DataColumn S3TransID
        {
            get
            {
                return this.colS3TransID;
            }
        }

        public DataColumn S3PurAmount
        {
            get
            {
                return this.colS3PurAmount;
            }
        }

        public DataColumn S3TaxAmount
        {
            get
            {
                return this.colS3TaxAmount;
            }
        }

        public DataColumn S3DiscountAmount
        {
            get
            {
                return this.colS3DiscountAmount;
            }
        }
        #endregion

        #region PRIMEPOS-2865 16-Jul-2020 JY Added
        public DataColumn AllowRxPicked
        {
            get
            {
                return this.colAllowRxPicked;
            }
        }
        #endregion

        public DataColumn IsCustomerDriven//PRIMEPOS-2915
        {
            get
            {
                return this.colIsCustomerDriven;
            }
        }

        #region PRIMEPOS-3053 08-Feb-2021 JY Added
        public DataColumn RxTaxPolicyID
        {
            get
            {
                return this.colRxTaxPolicyID;
            }
        }
        #endregion

        //PRIMEPOS-3117 11-Jul-2022 JY Added
        public DataColumn TotalTransFeeAmt
        {
            get
            {
                return this.colTotalTransFeeAmt;
            }
        }
        #endregion Properties

        #region Add and Get Methods

        public void AddRow(TransHeaderRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(TransHeaderRow row, bool preserveChanges)
        {
            if(this.GetRowByID(row.TransID) == null)
            {
                this.Rows.Add(row);
                if(preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public TransHeaderRow AddRow(System.Int32 TransID
                                        , System.DateTime TransDate
                                        , System.Int32 CustomerID
                                        , System.Decimal GrossTotal
                                        , System.Decimal TotalDiscAmount
                                        , System.Decimal TotalTaxAmount
                                        , System.Decimal TenderedAmount
                                        , System.Decimal TotalPaid
                                        , System.Decimal isStationClosed
                                        , System.Decimal isEOD
                                        , System.Decimal StClosedID
                                        , System.Decimal EODID)
        {
            TransHeaderRow row = (TransHeaderRow) this.NewRow();
            //row.ItemArray = new object[] {DeptId,DeptCode,DeptName,Discount,IsTaxable , SaleStartDate , SaleEndDate ,TaxId , SalePrice};
            row.TransID = TransID;
            row.TransDate = TransDate;
            row.CustomerID = CustomerID;

            this.Rows.Add(row);
            return row;
        }

        public TransHeaderRow GetRowByID(System.Int32 TransID)
        {
            return (TransHeaderRow) this.Rows.Find(new object[] { TransID });
        }

        public void MergeTable(DataTable dt)
        {
            TransHeaderRow row;
            foreach(DataRow dr in dt.Rows)
            {
                row = (TransHeaderRow) this.NewRow();

                if(dr[clsPOSDBConstants.TransHeader_Fld_TransID] == DBNull.Value)
                    row[clsPOSDBConstants.TransHeader_Fld_TransID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.TransHeader_Fld_TransID] = Convert.ToInt32((dr[clsPOSDBConstants.TransHeader_Fld_TransID].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransHeader_Fld_TransID].ToString());

                if(dr[clsPOSDBConstants.TransHeader_Fld_ReturnTransID] == DBNull.Value)
                    row[clsPOSDBConstants.TransHeader_Fld_ReturnTransID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.TransHeader_Fld_ReturnTransID] = Convert.ToInt32((dr[clsPOSDBConstants.TransHeader_Fld_ReturnTransID].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransHeader_Fld_ReturnTransID].ToString());

                if(dr[clsPOSDBConstants.TransHeader_Fld_TransDate] == DBNull.Value)
                    row[clsPOSDBConstants.TransHeader_Fld_TransDate] = DBNull.Value;
                else
                    if(dr[clsPOSDBConstants.TransHeader_Fld_TransDate].ToString().Trim() == "")
                        row[clsPOSDBConstants.TransHeader_Fld_TransDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                        row[clsPOSDBConstants.TransHeader_Fld_TransDate] = Convert.ToDateTime(dr[clsPOSDBConstants.TransHeader_Fld_TransDate].ToString());

                string strField=clsPOSDBConstants.TransHeader_Fld_CustomerID;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                if(dr[clsPOSDBConstants.Customer_Fld_CustomerCode] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_CustomerCode] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Customer_Fld_CustomerCode] = ((dr[clsPOSDBConstants.Customer_Fld_CustomerCode].ToString() == "") ? "" : dr[clsPOSDBConstants.Customer_Fld_CustomerCode].ToString());

                if(dr[clsPOSDBConstants.Customer_Fld_CustomerName] == DBNull.Value)
                    row[clsPOSDBConstants.Customer_Fld_CustomerName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Customer_Fld_CustomerName] = dr[clsPOSDBConstants.Customer_Fld_CustomerName].ToString();

                strField = clsPOSDBConstants.TransHeader_Fld_TransType;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_GrossTotal;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_TenderedAmount;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_TotalPaid;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.Users_Fld_UserID;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.TransHeader_Fld_StClosedID;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_EODID;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_DrawerNo;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToString((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_Account_No;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToString((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_isStationClosed;
                if(dr[strField] == DBNull.Value)
                    row[strField] = false;
                else
                    row[strField] = (dr[strField].ToString() == "") ? false : true;

                strField = clsPOSDBConstants.TransHeader_Fld_isEOD;
                if(dr[strField] == DBNull.Value)
                    row[strField] = false;
                else
                    row[strField] = (dr[strField].ToString() == "") ? false : true;

                strField = clsPOSDBConstants.TransHeader_Fld_StationID;
                if(dr[strField] == DBNull.Value)
                    row[strField] = false;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.TransHeader_Fld_IsDelivery;
                if(dr[strField] == DBNull.Value)
                    row[strField] = false;
                else
                    row[strField] = (dr[strField].ToString() == "") ? false : Convert.ToBoolean(dr[strField].ToString()); //change by Manoj 6/19/2013
                //Above line was return true when the value was set to false. Change and working fine now
                //row[strField] = (dr[strField].ToString() == "") ? false : Convert.ToBoolean(dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_LoyaltyPoints;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                //Added By Shitaljit(QuicSolv) on 2 Sept 2011
                strField = clsPOSDBConstants.TransHeader_Fld_InvoiceDiscount;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());
                //End of Added By Shitaljit 2 Sept 2011
                //Added By Shitaljit(QuicSolv) on 31 Oct 2011
                strField = clsPOSDBConstants.TransHeader_Fld_TransactionStartDate;
                if(dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    if(dr[strField].ToString().Trim() == "")
                        row[strField] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                        row[strField] = Convert.ToDateTime(dr[strField].ToString());
                //END of Added By Shitaljit(QuicSolv) on 31 Oct 2011
                
                row[clsPOSDBConstants.TransHeader_Fld_DeliveryAddress] = dr[clsPOSDBConstants.TransHeader_Fld_DeliveryAddress].ToString();

                #region Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
                strField = clsPOSDBConstants.TransHeader_Fld_WasonHold;
                if (dr[strField] == DBNull.Value)
                    row[strField] = false;
                else
                    row[strField] = (dr[strField].ToString() == "") ? false : Convert.ToBoolean(dr[strField].ToString());

                strField = clsPOSDBConstants.TransHeader_Fld_DeliverySigSkipped;
                if (dr[strField] == DBNull.Value)
                    row[strField] = false;
                else
                    row[strField] = (dr[strField].ToString() == "") ? false : Convert.ToBoolean(dr[strField].ToString());
                #endregion

                #region PRIMEPOS-2865 16-Jul-2020 JY Added
                strField = clsPOSDBConstants.TransHeader_Fld_AllowRxPicked;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());
                #endregion
                #region PRIMEPOS-3053 08-Feb-2021 JY Added
                strField = clsPOSDBConstants.TransHeader_Fld_RxTaxPolicyID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());
                #endregion

                //PRIMEPOS-3117 11-Jul-2022 JY Added
                strField = clsPOSDBConstants.TransHeader_Fld_TotalTransFeeAmt;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToDecimal((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());

                this.AddRow(row);
            }
        }

        #endregion Add and Get Methods

        public override DataTable Clone()
        {
            TransHeaderTable cln = (TransHeaderTable) base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new TransHeaderTable();
        }

        internal void InitVars()
        {
            this.colTransID = this.Columns[clsPOSDBConstants.TransHeader_Fld_TransID];
            this.colTransDate = this.Columns[clsPOSDBConstants.TransHeader_Fld_TransDate];
            //Following Code Added by Krishna on 2 June 2011
            this.colTransactionStartDate = this.Columns[clsPOSDBConstants.TransHeader_Fld_TransDate];
            //Till here Added by KRishna on 2 June 2011

            this.colCustomerCode = this.Columns[clsPOSDBConstants.Customer_Fld_CustomerCode];
            this.colCustomerID = this.Columns[clsPOSDBConstants.TransHeader_Fld_CustomerID];
            this.colStationID = this.Columns[clsPOSDBConstants.TransHeader_Fld_StationID];
            this.colCustomerName = this.Columns[clsPOSDBConstants.Customer_Fld_CustomerName];

            this.colDrawerNo = this.Columns[clsPOSDBConstants.TransHeader_Fld_DrawerNo];
            this.colTransType = this.Columns[clsPOSDBConstants.TransHeader_Fld_TransType];

            this.colEODID = this.Columns[clsPOSDBConstants.TransHeader_Fld_EODID];
            this.colGrossTotal = this.Columns[clsPOSDBConstants.TransHeader_Fld_GrossTotal];
            this.colisEOD = this.Columns[clsPOSDBConstants.TransHeader_Fld_isEOD];
            this.colStClosedID = this.Columns[clsPOSDBConstants.TransHeader_Fld_StClosedID];
            this.colTenderedAmount = this.Columns[clsPOSDBConstants.TransHeader_Fld_TenderedAmount];
            this.colTotalDiscAmount = this.Columns[clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount];
            this.colTotalTaxAmount = this.Columns[clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount];
            this.colTotalPaid = this.Columns[clsPOSDBConstants.TransHeader_Fld_TotalPaid];
            this.colUserID = this.Columns[clsPOSDBConstants.Users_Fld_UserID];
            this.colAcc_No = this.Columns[clsPOSDBConstants.TransHeader_Fld_Account_No];
            this.colReturnTransID = this.Columns[clsPOSDBConstants.TransHeader_Fld_ReturnTransID];
            this.colIsDelivery = this.Columns[clsPOSDBConstants.TransHeader_Fld_IsDelivery];
            this.colLoyaltyPoints = this.Columns[clsPOSDBConstants.TransHeader_Fld_LoyaltyPoints];
            //Added By Shitaljit(QuicSolv) on 31 August 2011
            this.colInvoiceDiscount = this.Columns[clsPOSDBConstants.TransHeader_Fld_InvoiceDiscount];
            //Till Here Added By Shitaljit(QuicSolv) on 31 August 2011
            this.colDeliveryAddress = this.Columns[clsPOSDBConstants.TransHeader_Fld_DeliveryAddress];
            this.colWasonHold = this.Columns[clsPOSDBConstants.TransHeader_Fld_WasonHold];  //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
            this.colDeliverySigSkipped = this.Columns[clsPOSDBConstants.TransHeader_Fld_DeliverySigSkipped];    //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
            #region NileshJ Solutran PRIMEPOS-2663
            this.colS3TransID = this.Columns[clsPOSDBConstants.TransHeader_Fld_S3TransID];
            this.colS3DiscountAmount = this.Columns[clsPOSDBConstants.TransHeader_Fld_S3DiscountAmount];
            this.colS3PurAmount = this.Columns[clsPOSDBConstants.TransHeader_Fld_S3PurAmount];
            this.colS3TaxAmount = this.Columns[clsPOSDBConstants.TransHeader_Fld_S3TaxAmount];
            #endregion
            this.colAllowRxPicked = this.Columns[clsPOSDBConstants.TransHeader_Fld_AllowRxPicked];  //PRIMEPOS-2865 16-Jul-2020 JY Added
            this.colIsCustomerDriven = this.Columns[clsPOSDBConstants.TransHeader_Fld_IsCustomerDriven];//2915
            this.colRxTaxPolicyID = this.Columns[clsPOSDBConstants.TransHeader_Fld_RxTaxPolicyID];  //PRIMEPOS-3053 08-Feb-2021 JY Added
            this.colTotalTransFeeAmt = this.Columns[clsPOSDBConstants.TransHeader_Fld_TotalTransFeeAmt];    //PRIMEPOS-3117 11-Jul-2022 JY Added
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colTransID = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TransID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransID);
            this.colTransID.AllowDBNull = true;

            this.colTransDate = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TransDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransDate);
            this.colTransDate.AllowDBNull = true;
            //Follwing Code Added by Krishna on 2 June 2011
            this.colTransactionStartDate = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TransactionStartDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransactionStartDate);
            this.colTransactionStartDate.AllowDBNull = true;
            //Till here Added by Krishna on 2 June 2011

            this.colCustomerID = new DataColumn(clsPOSDBConstants.TransHeader_Fld_CustomerID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerID);
            this.colCustomerID.AllowDBNull = true;

            this.colCustomerCode = new DataColumn(clsPOSDBConstants.Customer_Fld_CustomerCode, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerCode);
            this.colCustomerCode.AllowDBNull = true;

            this.colCustomerName = new DataColumn(clsPOSDBConstants.Customer_Fld_CustomerName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerName);
            this.colCustomerName.AllowDBNull = true;

            this.colTransType = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TransType, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransType);
            this.colTransType.AllowDBNull = true;

            this.colGrossTotal = new DataColumn(clsPOSDBConstants.TransHeader_Fld_GrossTotal, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colGrossTotal);
            this.colGrossTotal.AllowDBNull = true;

            this.colTotalDiscAmount = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalDiscAmount);
            this.colTotalDiscAmount.AllowDBNull = true;

            this.colTotalTaxAmount = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalTaxAmount);
            this.colTotalTaxAmount.AllowDBNull = true;

            this.colTenderedAmount = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TenderedAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTenderedAmount);
            this.colTenderedAmount.AllowDBNull = true;

            this.colTotalPaid = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TotalPaid, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalPaid);
            this.colTotalPaid.AllowDBNull = true;

            this.colisStationClosed = new DataColumn(clsPOSDBConstants.TransHeader_Fld_isStationClosed, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colisStationClosed);
            this.colisStationClosed.AllowDBNull = true;

            this.colisEOD = new DataColumn(clsPOSDBConstants.TransHeader_Fld_isEOD, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colisEOD);
            this.colisEOD.AllowDBNull = true;

            this.colDrawerNo = new DataColumn(clsPOSDBConstants.TransHeader_Fld_DrawerNo, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDrawerNo);
            this.colDrawerNo.AllowDBNull = true;

            this.colStClosedID = new DataColumn(clsPOSDBConstants.TransHeader_Fld_StClosedID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStClosedID);
            this.colStClosedID.AllowDBNull = true;

            this.colEODID = new DataColumn(clsPOSDBConstants.TransHeader_Fld_EODID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colEODID);
            this.colEODID.AllowDBNull = true;

            this.colUserID = new DataColumn(clsPOSDBConstants.Users_Fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            this.colUserID.AllowDBNull = true;

            this.colAcc_No = new DataColumn(clsPOSDBConstants.TransHeader_Fld_Account_No, typeof(long), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAcc_No);
            this.colAcc_No.AllowDBNull = true;

            this.colStationID = new DataColumn(clsPOSDBConstants.TransHeader_Fld_StationID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStationID);
            this.colStationID.AllowDBNull = true;

            this.colReturnTransID = new DataColumn(clsPOSDBConstants.TransHeader_Fld_ReturnTransID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colReturnTransID);
            this.colReturnTransID.AllowDBNull = true;

            this.colIsDelivery = new DataColumn(clsPOSDBConstants.TransHeader_Fld_IsDelivery, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsDelivery);
            this.colIsDelivery.AllowDBNull = true;

            this.colLoyaltyPoints = new DataColumn(clsPOSDBConstants.TransHeader_Fld_LoyaltyPoints, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLoyaltyPoints);
            this.colLoyaltyPoints.AllowDBNull = true;

            //Added By Shitaljit(QuicSolv) on 31 August 2011
            this.colInvoiceDiscount = new DataColumn(clsPOSDBConstants.TransHeader_Fld_InvoiceDiscount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colInvoiceDiscount);
            this.colInvoiceDiscount.AllowDBNull = true;
            //Till Here Added By Shitaljit(QuicSolv) on 31 August 2011

            this.colDeliveryAddress = new DataColumn(clsPOSDBConstants.TransHeader_Fld_DeliveryAddress, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDeliveryAddress);
            this.colDeliveryAddress.AllowDBNull = true;

            #region Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
            this.colWasonHold = new DataColumn(clsPOSDBConstants.TransHeader_Fld_WasonHold, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colWasonHold);
            this.colWasonHold.AllowDBNull = true;

            this.colDeliverySigSkipped = new DataColumn(clsPOSDBConstants.TransHeader_Fld_DeliverySigSkipped, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDeliverySigSkipped);
            this.colDeliverySigSkipped.AllowDBNull = true;
            #endregion

            #region NileshJ Solutran
            this.colS3TransID = new DataColumn(clsPOSDBConstants.TransHeader_Fld_S3TransID, typeof(System.Int64), null, System.Data.MappingType.Element); //PRIMEPOS-3265
            this.Columns.Add(this.colS3TransID);
            this.S3TransID.AllowDBNull = true;

            this.colS3DiscountAmount = new DataColumn(clsPOSDBConstants.TransHeader_Fld_S3DiscountAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3DiscountAmount);
            this.colS3DiscountAmount.AllowDBNull = true;

            this.colS3PurAmount = new DataColumn(clsPOSDBConstants.TransHeader_Fld_S3PurAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3PurAmount);
            this.colS3PurAmount.AllowDBNull = true;

            this.colS3TaxAmount = new DataColumn(clsPOSDBConstants.TransHeader_Fld_S3TaxAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colS3TaxAmount);
            this.colS3TaxAmount.AllowDBNull = true;
            #endregion

            #region PRIMEPOS-2865 16-Jul-2020 JY Added
            this.colAllowRxPicked = new DataColumn(clsPOSDBConstants.TransHeader_Fld_AllowRxPicked, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colAllowRxPicked);
            this.colAllowRxPicked.AllowDBNull = true;
            #endregion

            //PRIMEPOS-2915
            this.colIsCustomerDriven = new DataColumn(clsPOSDBConstants.TransHeader_Fld_IsCustomerDriven, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsCustomerDriven);
            this.colIsCustomerDriven.AllowDBNull = true;

            #region PRIMEPOS-3053 08-Feb-2021 JY Added
            this.colRxTaxPolicyID = new DataColumn(clsPOSDBConstants.TransHeader_Fld_RxTaxPolicyID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRxTaxPolicyID);
            this.colRxTaxPolicyID.AllowDBNull = true;
            #endregion

            //PRIMEPOS-3117 11-Jul-2022 JY Added
            this.colTotalTransFeeAmt = new DataColumn(clsPOSDBConstants.TransHeader_Fld_TotalTransFeeAmt, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTotalTransFeeAmt);
            this.colTotalTransFeeAmt.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colTransID };
        }

        public TransHeaderRow NewTransHeaderRow()
        {
            return (TransHeaderRow) this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new TransHeaderRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(TransHeaderRow);
        }
    }
}