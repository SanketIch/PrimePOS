 

namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Text;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;

    public class PayOutCatTable :DataTable
    {
        //private DataColumn colID;
        private DataColumn colPayoutCatType;
        private DataColumn colPayoutcatID;
        private DataColumn colUserId;
        private DataColumn colDefaultDescription;

        #region Constructors 
		internal PayOutCatTable() : base(clsPOSDBConstants.PayOutCat_tbl) { this.InitClass(); }
		internal PayOutCatTable (DataTable table) : base(table.TableName) {}
		#endregion


        #region Properties
        // Public Property for get all Rows in Table
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public PayOutCatRow this[int index]
        {
            get
            {
                return ((PayOutCatRow)(this.Rows[index]));
            }
        }

        // Public Property DataColumn PayOutcode
        public DataColumn Id
        {
            get
            {
                return this.colPayoutcatID;
            }
        }
        public DataColumn DefaultDescription
        {
            get
            {
                return this.colDefaultDescription;
            }
        }
        public DataColumn PayoutCatType
        {
            get
            {
                return this.colPayoutCatType;
            }
        }



        public DataColumn UserID
        {
            get
            {
                return this.colUserId;
            }
        }


        #endregion //Properties
        #region Add and Get Methods

        public void AddRow(PayOutCatRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(PayOutCatRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ID.ToString()) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }
        public PayOutCatRow GetRowByID(System.String ID)
            
        {
            
            return (PayOutCatRow)this.Rows.Find(new object[] {ID});
            
        }
        public PayOutCatRow AddRow(System.Int32 Id, System.String PayoutCatType)
        {

            PayOutCatRow row = (PayOutCatRow)this.NewRow();
            row.ItemArray = new object[] { Id, PayoutCatType };
            this.Rows.Add(row);
            return row;
        }


        public void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            PayOutCatRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (PayOutCatRow)this.NewRow();

                if (dr[clsPOSDBConstants.payoutCat_Fld_Id] == DBNull.Value)
                    row[clsPOSDBConstants.payoutCat_Fld_Id] = DBNull.Value;
                else
                    row[clsPOSDBConstants.payoutCat_Fld_Id] = Convert.ToInt32(dr[clsPOSDBConstants.payoutCat_Fld_Id].ToString());

                if (dr[clsPOSDBConstants.PayOutCat_Fld_PayoutType] == DBNull.Value)
                    row[clsPOSDBConstants.PayOutCat_Fld_PayoutType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.PayOutCat_Fld_PayoutType] = Convert.ToString(dr[clsPOSDBConstants.PayOutCat_Fld_PayoutType].ToString());

                if (dr[clsPOSDBConstants.PayOutCat_Fld_UserId] == DBNull.Value)
                    row[clsPOSDBConstants.PayOutCat_Fld_UserId] = String.Empty;
                else
                    row[clsPOSDBConstants.PayOutCat_Fld_UserId] = Convert.ToString(dr[clsPOSDBConstants.PayOutCat_Fld_UserId].ToString());

                if (dr[clsPOSDBConstants.PayOutCat_Fld_DefaultDescription] == DBNull.Value)
                    row[clsPOSDBConstants.PayOutCat_Fld_DefaultDescription] = String.Empty;
                else
                    row[clsPOSDBConstants.PayOutCat_Fld_DefaultDescription] = Convert.ToString(dr[clsPOSDBConstants.PayOutCat_Fld_DefaultDescription].ToString());

                //row[clsPOSDBConstants.PayOut_Fld_UserID] = Convert.ToString(dr[clsPOSDBConstants.PayOut_Fld_UserID].ToString());
                


                this.AddRow(row);
            }
        }

        #endregion //Add and Get Methods
        //protected override DataTable CreateInstance()
        //{
        //    return new PayOutCatTable();
        //}

        internal void InitVars()
        {
            try
            {

                this.colPayoutcatID = this.Columns[clsPOSDBConstants.payoutCat_Fld_Id];
                this.colPayoutCatType = this.Columns[clsPOSDBConstants.PayOutCat_Fld_PayoutType];
                this.colUserId = this.Columns[clsPOSDBConstants.PayOut_Fld_UserID];
                this.colDefaultDescription = this.Columns[clsPOSDBConstants.PayOutCat_Fld_DefaultDescription];

            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colPayoutcatID = new DataColumn(clsPOSDBConstants.payoutCat_Fld_Id, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPayoutcatID);
            this.colPayoutcatID.AllowDBNull = false;

            this.colPayoutCatType = new DataColumn(clsPOSDBConstants.PayOutCat_Fld_PayoutType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPayoutCatType);
            this.colPayoutCatType.AllowDBNull = true;

            this.colUserId = new DataColumn(clsPOSDBConstants.PayOutCat_Fld_UserId, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserId);
            this.colUserId.AllowDBNull = true;

            this.colDefaultDescription = new DataColumn(clsPOSDBConstants.PayOutCat_Fld_DefaultDescription, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDefaultDescription);
            this.colUserId.AllowDBNull = true;


            this.PrimaryKey = new DataColumn[] { this.colPayoutcatID };
        }
        public PayOutCatRow NewclsPayOutCatRow()
        {
            return (PayOutCatRow)this.NewRow();
        }

        public override DataTable Clone()
        {
            PayOutCatTable cln = (PayOutCatTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new PayOutCatRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(PayOutCatRow);
        }
    }
}