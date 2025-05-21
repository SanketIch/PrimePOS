using POS_Core.CommonData.Rows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.CommonData.Tables
{
    public class TransFeeTable : DataTable, System.Collections.IEnumerable
    {
        #region columns declaration
        private DataColumn colTransFeeID;
        private DataColumn colTransFeeDesc;
        private DataColumn colChargeTransFeeFor;
        private DataColumn colTransFeeMode;
        private DataColumn colTransFeeValue;
        private DataColumn colPayTypeID;
        private DataColumn colIsActive;
        #endregion

        #region Constants
        /// <value>The constant used for TransactionFee table. </value>
        private const String _TableName = "TransactionFee";
        #endregion

        #region Constructors
        internal TransFeeTable() : base(_TableName) { this.InitClass(); }
        internal TransFeeTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public TransFeeRow this[int index]
        {
            get
            {
                return ((TransFeeRow)(this.Rows[index]));
            }
        }

        public DataColumn transFeeID
        {
            get
            {
                return this.colTransFeeID;
            }
        }

        public DataColumn transFeeDesc
        {
            get
            {
                return this.colTransFeeDesc;
            }
        }

        public DataColumn chargeTransFeeFor
        {
            get
            {
                return this.colChargeTransFeeFor;
            }
        }

        public DataColumn transFeeMode
        {
            get
            {
                return this.colTransFeeMode;
            }
        }

        public DataColumn transFeeValue
        {
            get
            {
                return this.colTransFeeValue;
            }
        }

        public DataColumn payTypeID
        {
            get
            {
                return this.colPayTypeID;
            }
        }

        public DataColumn isActive
        {
            get
            {
                return this.colIsActive;
            }
        }
        #endregion

        #region Add and Get Methods
        public virtual void AddRow(TransFeeRow row)
        {
            AddRow(row, false);
        }

        public virtual void AddRow(TransFeeRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.TransFeeID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public TransFeeRow AddRow(System.Int32 TransFeeID, System.String TransFeeDesc, System.Int16 ChargeTransFeeFor, System.Boolean TransFeeMode, System.Decimal TransFeeValue, System.String PayTypeID, System.Boolean IsActive)
        {
            TransFeeRow row = (TransFeeRow)this.NewRow();
            row.TransFeeID = TransFeeID;
            row.TransFeeDesc = TransFeeDesc;
            row.ChargeTransFeeFor = ChargeTransFeeFor;
            row.TransFeeMode = TransFeeMode;
            row.TransFeeValue = TransFeeValue;
            row.PayTypeID = PayTypeID;
            row.IsActive = IsActive;
            this.Rows.Add(row);
            return row;
        }

        public TransFeeRow GetRowByID(System.Int32 TransFeeID)
        {
            return (TransFeeRow)this.Rows.Find(new object[] { TransFeeID });
        }

        public virtual void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            TransFeeRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (TransFeeRow)this.NewRow();

                if (dr.Table.Columns.Contains(clsPOSDBConstants.TransFee_Fld_TransFeeID))
                {
                    if (dr[clsPOSDBConstants.TransFee_Fld_TransFeeID] == DBNull.Value)
                        row[clsPOSDBConstants.TransFee_Fld_TransFeeID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransFee_Fld_TransFeeID] = Convert.ToInt32((dr[clsPOSDBConstants.TransFee_Fld_TransFeeID].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransFee_Fld_TransFeeID].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.TransFee_Fld_TransFeeDesc))
                {
                    if (dr[clsPOSDBConstants.TransFee_Fld_TransFeeDesc] == DBNull.Value)
                        row[clsPOSDBConstants.TransFee_Fld_TransFeeDesc] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransFee_Fld_TransFeeDesc] = Convert.ToString(dr[clsPOSDBConstants.TransFee_Fld_TransFeeDesc].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor))
                {
                    if (dr[clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor] == DBNull.Value)
                        row[clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor] = Convert.ToInt16((dr[clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.TransFee_Fld_TransFeeMode))
                {
                    if (dr[clsPOSDBConstants.TransFee_Fld_TransFeeMode].ToString().Trim() == "")
                        row[clsPOSDBConstants.TransFee_Fld_TransFeeMode] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransFee_Fld_TransFeeMode] = Convert.ToBoolean(dr[clsPOSDBConstants.TransFee_Fld_TransFeeMode].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.TransFee_Fld_TransFeeValue))
                {
                    if (dr[clsPOSDBConstants.TransFee_Fld_TransFeeValue] == DBNull.Value)
                        row[clsPOSDBConstants.TransFee_Fld_TransFeeValue] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransFee_Fld_TransFeeValue] = Convert.ToDecimal((dr[clsPOSDBConstants.TransFee_Fld_TransFeeValue].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransFee_Fld_TransFeeValue].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.TransFee_Fld_PayTypeID))
                {
                    if (dr[clsPOSDBConstants.TransFee_Fld_PayTypeID] == DBNull.Value)
                        row[clsPOSDBConstants.TransFee_Fld_PayTypeID] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransFee_Fld_PayTypeID] = Convert.ToString(dr[clsPOSDBConstants.TransFee_Fld_PayTypeID].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.TransFee_Fld_IsActive))
                {
                    if (dr[clsPOSDBConstants.TransFee_Fld_IsActive].ToString().Trim() == "")
                        row[clsPOSDBConstants.TransFee_Fld_IsActive] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.TransFee_Fld_IsActive] = Convert.ToBoolean(dr[clsPOSDBConstants.TransFee_Fld_IsActive].ToString());
                }

                this.AddRow(row);
            }
        }
        #endregion

        public override DataTable Clone()
        {
            TransFeeTable cln = (TransFeeTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new TransFeeTable();
        }

        internal void InitVars()
        {
            this.colTransFeeID = this.Columns[clsPOSDBConstants.TransFee_Fld_TransFeeID];
            this.colTransFeeDesc = this.Columns[clsPOSDBConstants.TransFee_Fld_TransFeeDesc];
            this.colChargeTransFeeFor = this.Columns[clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor];
            this.colTransFeeMode = this.Columns[clsPOSDBConstants.TransFee_Fld_TransFeeMode];
            this.colTransFeeValue = this.Columns[clsPOSDBConstants.TransFee_Fld_TransFeeValue];
            this.colPayTypeID = this.Columns[clsPOSDBConstants.TransFee_Fld_PayTypeID];
            this.colIsActive = this.Columns[clsPOSDBConstants.TransFee_Fld_IsActive];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colTransFeeID = new DataColumn(clsPOSDBConstants.TransFee_Fld_TransFeeID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransFeeID);
            this.colTransFeeID.AllowDBNull = false;            

            this.colTransFeeDesc = new DataColumn(clsPOSDBConstants.TransFee_Fld_TransFeeDesc, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransFeeDesc);
            this.colTransFeeDesc.AllowDBNull = false;

            this.colChargeTransFeeFor = new DataColumn(clsPOSDBConstants.TransFee_Fld_ChargeTransFeeFor, typeof(System.Int16), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colChargeTransFeeFor);
            this.colChargeTransFeeFor.AllowDBNull = true;

            this.colTransFeeMode = new DataColumn(clsPOSDBConstants.TransFee_Fld_TransFeeMode, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransFeeMode);
            this.colTransFeeMode.AllowDBNull = true;

            this.colTransFeeValue = new DataColumn(clsPOSDBConstants.TransFee_Fld_TransFeeValue, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransFeeValue);
            this.colTransFeeValue.AllowDBNull = true;

            this.colPayTypeID = new DataColumn(clsPOSDBConstants.TransFee_Fld_PayTypeID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPayTypeID);
            this.colPayTypeID.AllowDBNull = false;

            this.colIsActive = new DataColumn(clsPOSDBConstants.TransFee_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = true;
            
            this.PrimaryKey = new DataColumn[] { this.transFeeID };
        }

        public virtual TransFeeRow NewTransFeeRow()
        {
            return (TransFeeRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new TransFeeRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(TransFeeRow);
        }
    }
}
