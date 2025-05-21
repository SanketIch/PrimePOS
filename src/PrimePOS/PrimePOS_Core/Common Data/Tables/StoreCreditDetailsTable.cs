using POS_Core.CommonData.Rows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.CommonData.Tables {
    // StoreCredit - PRIMEPOS-2747 - NileshJ
    public class StoreCreditDetailsTable : DataTable, System.Collections.IEnumerable {


        private DataColumn colStoreCreditDetailsID;
        private DataColumn colStoreCreditID;
        private DataColumn colCustomerID;
        private DataColumn colTransactionID;

        private DataColumn colCreditAmt;
        private DataColumn colUpdatedOn;
        private DataColumn colUpdatedBy;
        private DataColumn colRemainingAmount;

        #region Constants
        private const String _TableName = "StoreCreditDetails";
        #endregion

        #region Constructors 
        internal StoreCreditDetailsTable() : base(_TableName) { this.InitClass(); }
        internal StoreCreditDetailsTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        public int Count {
            get {
                return this.Rows.Count;
            }
        }

        public StoreCreditDetailsRow this[int index] {
            get {
                return ((StoreCreditDetailsRow)(this.Rows[index]));
            }
        }

        public DataColumn StoreCreditDetailsID {
            get {
                return this.colStoreCreditDetailsID;
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

        public DataColumn TransactionID {
            get {
                return this.colTransactionID;
            }
        }

        public DataColumn CreditAmt {
            get {
                return this.colCreditAmt;
            }
        }

        public DataColumn UpdatedOn {
            get {
                return this.colUpdatedOn;
            }
        }

        public DataColumn UpdatedBy {
            get {
                return this.colUpdatedBy;
            }
        }

        public DataColumn RemainingAmount
        {
            get
            {
                return this.colRemainingAmount;
            }
        }

        #endregion //Properties

        #region Add and Get Methods 

        public void AddRow(StoreCreditDetailsRow row) {
            AddRow(row, false);
        }

        public void AddRow(StoreCreditDetailsRow row, bool preserveChanges) {
            if (this.GetRowByID(row.StoreCreditDetailsID) == null) {
                this.Rows.Add(row);
                if (!preserveChanges) {
                    row.AcceptChanges();
                }
            }
        }

        public StoreCreditDetailsRow AddRow(System.Int32 ID
                                        , System.Int32 StoreCreditID
                                        , System.Int32 CustomerID
                                        , System.Int32 TransactionID
                                        , System.Decimal CreditAmt
                                        , System.DateTime UpdatedOn
                                        , System.String UpdatedBy) {
            StoreCreditDetailsRow row = (StoreCreditDetailsRow)this.NewRow();

            row.StoreCreditDetailsID = ID;
            row.StoreCreditID = StoreCreditID;
            row.CustomerID = CustomerID;
            row.TransactionID = TransactionID;
            row.CreditAmt = CreditAmt;
            row.UpdatedOn = UpdatedOn;
            row.UpdatedBy = UpdatedBy;
            this.Rows.Add(row);
            return row;
        }

        public StoreCreditDetailsRow GetRowByID(System.Int32 iID) {
            return (StoreCreditDetailsRow)this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt) {

            StoreCreditDetailsRow row;
            foreach (DataRow dr in dt.Rows) {
                row = (StoreCreditDetailsRow)this.NewRow();

                if (dr[clsPOSDBConstants.StoreCreditDetails_ID] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCreditDetails_ID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCreditDetails_ID] = Convert.ToInt32((dr[clsPOSDBConstants.StoreCreditDetails_ID].ToString() == "") ? "0" : dr[0].ToString());


                if (dr[clsPOSDBConstants.StoreCreditDetails_StoreCreditID] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCreditDetails_StoreCreditID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCreditDetails_StoreCreditID] = Convert.ToInt32((dr[clsPOSDBConstants.StoreCreditDetails_StoreCreditID].ToString() == "") ? "0" : dr[clsPOSDBConstants.StoreCreditDetails_StoreCreditID].ToString());

                if (dr[clsPOSDBConstants.StoreCreditDetails_CustomerID] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCreditDetails_CustomerID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCreditDetails_CustomerID] = Convert.ToInt32((dr[clsPOSDBConstants.StoreCreditDetails_CustomerID].ToString() == "") ? "0" : dr[clsPOSDBConstants.StoreCreditDetails_CustomerID].ToString());

                if (dr[clsPOSDBConstants.StoreCreditDetails_TransactionID] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCreditDetails_TransactionID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCreditDetails_TransactionID] = Convert.ToInt32((dr[clsPOSDBConstants.StoreCreditDetails_TransactionID].ToString() == "") ? "0" : dr[clsPOSDBConstants.StoreCreditDetails_TransactionID].ToString());



                if (dr[clsPOSDBConstants.StoreCreditDetails_CreditAmt] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCreditDetails_CreditAmt] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCreditDetails_CreditAmt] = Convert.ToDecimal((dr[clsPOSDBConstants.StoreCreditDetails_CreditAmt].ToString() == "") ? "0" : dr[clsPOSDBConstants.StoreCreditDetails_CreditAmt].ToString());

                if (dr[clsPOSDBConstants.StoreCreditDetails_UpdatedOn] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCreditDetails_UpdatedOn] = DBNull.Value;
                else
                    if (dr[clsPOSDBConstants.StoreCreditDetails_UpdatedOn].ToString().Trim() == "")
                    row[clsPOSDBConstants.StoreCreditDetails_UpdatedOn] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.StoreCreditDetails_UpdatedOn] = Convert.ToDateTime(dr[clsPOSDBConstants.StoreCreditDetails_UpdatedOn].ToString());


                string strField = clsPOSDBConstants.StoreCreditDetails_UpdatedBy;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                if (dr[clsPOSDBConstants.StoreCreditDetails_RemainingAmount] == DBNull.Value)
                    row[clsPOSDBConstants.StoreCreditDetails_RemainingAmount] = DBNull.Value;
                else
                    row[clsPOSDBConstants.StoreCreditDetails_RemainingAmount] = Convert.ToDecimal((dr[clsPOSDBConstants.StoreCreditDetails_RemainingAmount].ToString() == "") ? "0" : dr[clsPOSDBConstants.StoreCreditDetails_RemainingAmount].ToString());



                this.AddRow(row);
            }
        }

        #endregion

        public override DataTable Clone() {
            StoreCreditDetailsTable cln = (StoreCreditDetailsTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance() {
            return new StoreCreditDetailsTable();
        }

        internal void InitVars() {
            this.colStoreCreditDetailsID = this.Columns[clsPOSDBConstants.StoreCreditDetails_ID];
            this.colStoreCreditID = this.Columns[clsPOSDBConstants.StoreCreditDetails_StoreCreditID];
            this.colCustomerID = this.Columns[clsPOSDBConstants.StoreCreditDetails_CustomerID];
            this.colTransactionID = this.Columns[clsPOSDBConstants.StoreCreditDetails_TransactionID];
            this.colCreditAmt = this.Columns[clsPOSDBConstants.StoreCreditDetails_CreditAmt];
            this.colUpdatedOn = this.Columns[clsPOSDBConstants.StoreCreditDetails_UpdatedOn];
            this.colUpdatedBy = this.Columns[clsPOSDBConstants.StoreCreditDetails_UpdatedBy];
            this.colRemainingAmount = this.Columns[clsPOSDBConstants.StoreCreditDetails_RemainingAmount];
        }

        public System.Collections.IEnumerator GetEnumerator() {
            return this.Rows.GetEnumerator();
        }

        private void InitClass() {
            
            this.colStoreCreditDetailsID = new DataColumn("StoreCreditDetailsID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStoreCreditDetailsID);
            this.colStoreCreditDetailsID.AllowDBNull = true;

            this.colStoreCreditID = new DataColumn("StoreCreditID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStoreCreditID);
            this.colStoreCreditID.AllowDBNull = true;

            this.colCustomerID = new DataColumn("CustomerID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCustomerID);
            this.colCustomerID.AllowDBNull = true;

            this.colTransactionID = new DataColumn("TransactionID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransactionID);
            this.colTransactionID.AllowDBNull = true;

            this.colCreditAmt = new DataColumn("CreditAmt", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreditAmt);
            this.colCreditAmt.AllowDBNull = true;

            this.colUpdatedOn = new DataColumn("UpdatedOn", typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatedOn);
            this.colUpdatedOn.AllowDBNull = true;

            this.colUpdatedBy = new DataColumn("UpdatedBy", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatedBy);
            this.colUpdatedBy.AllowDBNull = true;

            this.colRemainingAmount = new DataColumn("RemainingAmount", typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRemainingAmount);
            this.colRemainingAmount.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colStoreCreditDetailsID };
        }
        public StoreCreditDetailsRow NewStoreCreditDetailsRow() {
            return (StoreCreditDetailsRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
            return new StoreCreditDetailsRow(builder);
        }

        protected override System.Type GetRowType() {
            return typeof(StoreCreditDetailsRow);
        }
    }

}

