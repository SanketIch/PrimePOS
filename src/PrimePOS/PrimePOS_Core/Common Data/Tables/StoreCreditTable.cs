using System;
using System.Data;
using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData.Tables {
    // StoreCredit - PRIMEPOS-2747 - NileshJ
    public class StoreCreditTable : DataTable, System.Collections.IEnumerable
    {

        private DataColumn colStoreCreditID;
        private DataColumn colCustomerID;

        private DataColumn colCreditAmt;
        private DataColumn colLastUpdated;
        private DataColumn colLastUpdatedBy;
        private DataColumn colExpiresOn;

        #region Constants
        private const String _TableName = "StoreCredit";
        #endregion

        #region Constructors 
        internal StoreCreditTable() : base(_TableName) { this.InitClass(); }
        internal StoreCreditTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        public int Count {
            get {
                return this.Rows.Count;
            }
        }

        public StoreCreditRow this[int index] {
            get {
                return ((StoreCreditRow)(this.Rows[index]));
            }
        }

        public DataColumn StoreCreditID {
            get {
                return this.colStoreCreditID;
            }
        }

        public DataColumn CustomerID {
            get {
                return this.colCustomerID;
            }
        }

        public DataColumn CreditAmt {
            get {
                return this.colCreditAmt;
            }
        }

        public DataColumn LastUpdated {
            get {
                return this.colLastUpdated;
            }
        }

        public DataColumn LastUpdatedBy {
            get {
                return this.colLastUpdatedBy;
            }
        }


        public DataColumn ExpiresOn {
            get {
                return this.colExpiresOn;
            }
        }

        #endregion //Properties

        #region Add and Get Methods 

        public void AddRow(StoreCreditRow row) {
            AddRow(row, false);
        }

        public void AddRow(StoreCreditRow row, bool preserveChanges) {
            if (this.GetRowByID(row.StoreCreditID) == null) {
                this.Rows.Add(row);
                if (!preserveChanges) {
                    row.AcceptChanges();
                }
            }
        }

        public StoreCreditRow AddRow(System.Int32 ID
                                        , System.Int32 CustomerID
                                        , System.Decimal CreditAmt
                                        , System.DateTime LastUpdated
                                        , System.String LastUpdatedBy
                                        , System.Int32 ExpiresOn) {
            StoreCreditRow row = (StoreCreditRow)this.NewRow();
            
            row.StoreCreditID = ID;
            row.CustomerID = CustomerID;
            row.CreditAmt = CreditAmt;
            row.LastUpdated = LastUpdated;
            row.LastUpdatedBy = LastUpdatedBy;
            row.ExpiresOn = ExpiresOn;
            this.Rows.Add(row);
            return row;
        }

        public StoreCreditRow GetRowByID(System.Int64 iID) {
            return (StoreCreditRow)this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt) {

            StoreCreditRow row;
            foreach (DataRow dr in dt.Rows) {
                row = (StoreCreditRow)this.NewRow();

                if (dr[clsPOSDBConstants.StoreCredit_ID] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCredit_ID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCredit_ID] = Convert.ToInt32((dr[clsPOSDBConstants.StoreCredit_ID].ToString() == "") ? "0" : dr[0].ToString());

                if (dr[clsPOSDBConstants.StoreCredit_CustomerID] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCredit_CustomerID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCredit_CustomerID] = Convert.ToInt32((dr[clsPOSDBConstants.StoreCredit_CustomerID].ToString() == "") ? "0" : dr[clsPOSDBConstants.StoreCredit_CustomerID].ToString());


                if (dr[clsPOSDBConstants.StoreCredit_CreditAmt] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCredit_CreditAmt] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCredit_CreditAmt] = Convert.ToDecimal((dr[clsPOSDBConstants.StoreCredit_CreditAmt].ToString() == "") ? "0" : dr[clsPOSDBConstants.StoreCredit_CreditAmt].ToString());

                if (dr[clsPOSDBConstants.StoreCredit_LastUpdated] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCredit_LastUpdated] = DBNull.Value;
                else
                    if (dr[clsPOSDBConstants.StoreCredit_LastUpdated].ToString().Trim() == "")
                    row[clsPOSDBConstants.StoreCredit_LastUpdated] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.StoreCredit_LastUpdated] = Convert.ToDateTime(dr[clsPOSDBConstants.StoreCredit_LastUpdated].ToString());


                string strField = clsPOSDBConstants.StoreCredit_LastUpdatedBy;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.StoreCredit_ExpiresOn;
                if (dr[strField] == DBNull.Value)
                    row[strField] = 0;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "") ? "0" : dr[strField].ToString());


                this.AddRow(row);
            }
        }

        #endregion

        public override DataTable Clone() {
            StoreCreditTable cln = (StoreCreditTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance() {
            return new StoreCreditTable();
        }

        internal void InitVars() {
            this.colStoreCreditID = this.Columns[clsPOSDBConstants.StoreCredit_ID];
            this.colCustomerID = this.Columns[clsPOSDBConstants.StoreCredit_CustomerID];
            this.colCreditAmt = this.Columns[clsPOSDBConstants.StoreCredit_CreditAmt];
            this.colLastUpdated = this.Columns[clsPOSDBConstants.StoreCredit_LastUpdated];
            this.colLastUpdatedBy = this.Columns[clsPOSDBConstants.StoreCredit_LastUpdatedBy];
            this.colExpiresOn = this.Columns[clsPOSDBConstants.StoreCredit_ExpiresOn];

        }

        public System.Collections.IEnumerator GetEnumerator() {
            return this.Rows.GetEnumerator();
        }

        private void InitClass() {
            this.colStoreCreditID = new DataColumn("StoreCreditID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStoreCreditID);
            this.colStoreCreditID.AllowDBNull = true;

            this.colCustomerID = new DataColumn("CustomerID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerID);
            this.colCustomerID.AllowDBNull = true;

            this.colCreditAmt = new DataColumn("CreditAmt", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreditAmt);
            this.colCreditAmt.AllowDBNull = true;

            this.colLastUpdated = new DataColumn("LastUpdated", typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastUpdated);
            this.colLastUpdated.AllowDBNull = true;

            this.colLastUpdatedBy = new DataColumn("LastUpdatedBy", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastUpdatedBy);
            this.colLastUpdatedBy.AllowDBNull = true;

            this.colExpiresOn = new DataColumn("ExpiresOn", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colExpiresOn);
            this.colExpiresOn.AllowDBNull = true;
            this.PrimaryKey = new DataColumn[] { this.colStoreCreditID };
        }
        public StoreCreditRow NewStoreCreditRow() {
            return (StoreCreditRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
            return new StoreCreditRow(builder);
        }

        protected override System.Type GetRowType() {
            return typeof(StoreCreditRow);
        }
    }
}
