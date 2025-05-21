using System;
using System.Data;
using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData.Tables
{
    public class PSEItemTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colId;
        private DataColumn colProductId;
        private DataColumn colProductName;
        private DataColumn colProductNDC;
        private DataColumn colProductGrams;
        private DataColumn colProductPillCnt;
        private DataColumn colCreatedBy;
        private DataColumn colCreatedOn;
        private DataColumn colUpdatedBy;
        private DataColumn colUpdatedOn;
        private DataColumn colIsActive;
        private DataColumn colRecordStatus;

        #region Constants
        private const String _TableName = "PSE_Items";
        #endregion

        #region Constructors 
        internal PSEItemTable() : base(_TableName)
        { this.InitClass(); }
        internal PSEItemTable(DataTable table) : base(table.TableName)
        { }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }
        
        public PSEItemRow this[int index]
        {
            get
            {
                return ((PSEItemRow)(this.Rows[index]));
            }
        }

        public DataColumn id
        {
            get
            {
                return this.colId;
            }
        }

        public DataColumn ProductId
        {
            get
            {
                return this.colProductId;
            }
        }

        public DataColumn ProductName
        {
            get
            {
                return this.colProductName;
            }
        }

        public DataColumn ProductNDC
        {
            get
            {
                return this.colProductNDC;
            }
        }

        public DataColumn ProductGrams
        {
            get
            {
                return this.colProductGrams;
            }
        }

        public DataColumn ProductPillCnt
        {
            get
            {
                return this.colProductPillCnt;
            }
        }

        public DataColumn CreatedBy
        {
            get
            {
                return this.colCreatedBy;
            }
        }

        public DataColumn CreatedOn
        {
            get
            {
                return this.colCreatedOn;
            }
        }

        public DataColumn UpdatedBy
        {
            get
            {
                return this.colUpdatedBy;
            }
        }

        public DataColumn UpdatedOn
        {
            get
            {
                return this.colUpdatedOn;
            }
        }

        public DataColumn IsActive
        {
            get
            {
                return this.colIsActive;
            }
        }
        public DataColumn RecordStatus
        {
            get
            {
                return this.colRecordStatus;
            }
        }
        #endregion //Properties

        #region Add and Get Methods 
        public void AddRow(PSEItemRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(PSEItemRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ProductId) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public PSEItemRow AddRow(System.Int32 Id, System.String ProductId, System.String ProductName, System.String ProductNDC, System.Decimal ProductGrams, System.Int32 ProductPillCnt, System.Boolean IsActive, System.String sRecordStatus)
        {
            PSEItemRow row = (PSEItemRow)this.NewRow();
            //row.ItemArray = new object[] { Id,ProductId,ProductName,ProductNDC,ProductGrams,ProductPillCnt,IsActive};
            row.Id = Id;
            row.ProductId = ProductId;
            row.ProductName = ProductName;
            row.ProductNDC = ProductNDC;
            row.ProductGrams = ProductGrams;
            row.ProductPillCnt = ProductPillCnt;
            row.IsActive = IsActive;
            row.RecordStatus = sRecordStatus;
            this.Rows.Add(row);
            return row;
        }

        public PSEItemRow GetRowByID(System.String ProductId)
        {
            return (PSEItemRow)this.Rows.Find(new object[] { ProductId });
        }

        public void MergeTable(DataTable dt)
        {
            PSEItemRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (PSEItemRow)this.NewRow();

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_Id))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_Id] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_Id] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_Id] = Convert.ToInt32((dr[clsPOSDBConstants.PSE_Items_Fld_Id].ToString() == "") ? "0" : dr[clsPOSDBConstants.PSE_Items_Fld_Id].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_ProductId))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_ProductId] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductId] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductId] = Convert.ToString(dr[clsPOSDBConstants.PSE_Items_Fld_ProductId].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_ProductName))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_ProductName] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductName] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductName] = Convert.ToString(dr[clsPOSDBConstants.PSE_Items_Fld_ProductName].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_ProductNDC))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_ProductNDC] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductNDC] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductNDC] = Convert.ToString(dr[clsPOSDBConstants.PSE_Items_Fld_ProductNDC].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_ProductGrams))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_ProductGrams] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductGrams] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductGrams] = Convert.ToDecimal((dr[clsPOSDBConstants.PSE_Items_Fld_ProductGrams].ToString() == "") ? "0" : dr[clsPOSDBConstants.PSE_Items_Fld_ProductGrams].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt] = Convert.ToInt32((dr[clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt].ToString() == "") ? "0" : dr[clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_CreatedBy))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_CreatedBy] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_CreatedBy] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_CreatedBy] = Convert.ToString(dr[clsPOSDBConstants.PSE_Items_Fld_CreatedBy].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_CreatedOn))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_CreatedOn] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_CreatedOn] = DBNull.Value;
                    else
                        if (dr[clsPOSDBConstants.PSE_Items_Fld_CreatedOn].ToString().Trim() == "")
                        row[clsPOSDBConstants.PSE_Items_Fld_CreatedOn] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_CreatedOn] = Convert.ToDateTime(dr[clsPOSDBConstants.PSE_Items_Fld_CreatedOn].ToString());
                }
                
                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_UpdatedBy))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_UpdatedBy] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_UpdatedBy] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_UpdatedBy] = Convert.ToString(dr[clsPOSDBConstants.PSE_Items_Fld_UpdatedBy].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_UpdatedOn))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_UpdatedOn] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_UpdatedOn] = DBNull.Value;
                    else
                        if (dr[clsPOSDBConstants.PSE_Items_Fld_UpdatedOn].ToString().Trim() == "")
                        row[clsPOSDBConstants.PSE_Items_Fld_UpdatedOn] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_UpdatedOn] = Convert.ToDateTime(dr[clsPOSDBConstants.PSE_Items_Fld_UpdatedOn].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_IsActive))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_IsActive].ToString().Trim() == "")
                        row[clsPOSDBConstants.PSE_Items_Fld_IsActive] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_IsActive] = Convert.ToBoolean(dr[clsPOSDBConstants.PSE_Items_Fld_IsActive].ToString());
                }

                if (dr.Table.Columns.Contains(clsPOSDBConstants.PSE_Items_Fld_RecordStatus))
                {
                    if (dr[clsPOSDBConstants.PSE_Items_Fld_RecordStatus] == DBNull.Value)
                        row[clsPOSDBConstants.PSE_Items_Fld_RecordStatus] = DBNull.Value;
                    else
                        row[clsPOSDBConstants.PSE_Items_Fld_RecordStatus] = Convert.ToString(dr[clsPOSDBConstants.PSE_Items_Fld_RecordStatus].ToString());
                }

                this.AddRow(row);
            }
        }
        #endregion

        public override DataTable Clone()
        {
            PSEItemTable cln = (PSEItemTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new PSEItemTable();
        }

        internal void InitVars()
        {
            this.colId = this.Columns[clsPOSDBConstants.PSE_Items_Fld_Id];
            this.colProductId = this.Columns[clsPOSDBConstants.PSE_Items_Fld_ProductId];
            this.colProductName = this.Columns[clsPOSDBConstants.PSE_Items_Fld_ProductName];
            this.colProductNDC = this.Columns[clsPOSDBConstants.PSE_Items_Fld_ProductNDC];
            this.colProductGrams = this.Columns[clsPOSDBConstants.PSE_Items_Fld_ProductGrams];
            this.colProductPillCnt = this.Columns[clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt];
            this.colCreatedBy = this.Columns[clsPOSDBConstants.PSE_Items_Fld_CreatedBy];
            this.colUpdatedBy = this.Columns[clsPOSDBConstants.PSE_Items_Fld_UpdatedBy];
            this.colCreatedOn = this.Columns[clsPOSDBConstants.PSE_Items_Fld_CreatedOn];
            this.colUpdatedOn = this.Columns[clsPOSDBConstants.PSE_Items_Fld_UpdatedOn];
            this.colIsActive = this.Columns[clsPOSDBConstants.PSE_Items_Fld_IsActive];
            this.colRecordStatus = this.Columns[clsPOSDBConstants.PSE_Items_Fld_RecordStatus];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colId = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_Id, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colId);
            this.colId.AllowDBNull = false;

            this.colProductId = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_ProductId, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProductId);
            this.colProductId.AllowDBNull = false;

            this.colProductName = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_ProductName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProductName);
            this.colProductName.AllowDBNull = true;

            this.colProductNDC = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_ProductNDC, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProductNDC);
            this.colProductNDC.AllowDBNull = true;

            this.colProductGrams = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_ProductGrams, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProductGrams);
            this.colProductGrams.AllowDBNull = true;

            this.colProductPillCnt = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_ProductPillCnt, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProductPillCnt);
            this.colProductPillCnt.AllowDBNull = true;

            this.colCreatedBy = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_CreatedBy, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreatedBy);
            this.colCreatedBy.AllowDBNull = true;

            this.colCreatedOn = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_CreatedOn, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCreatedOn);
            this.colCreatedOn.AllowDBNull = true;

            this.colUpdatedBy = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_UpdatedBy, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatedBy);
            this.colUpdatedBy.AllowDBNull = true;

            this.colUpdatedOn = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_UpdatedOn, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatedOn);
            this.colUpdatedOn.AllowDBNull = true;

            this.colIsActive = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = true;

            this.colRecordStatus = new DataColumn(clsPOSDBConstants.PSE_Items_Fld_RecordStatus, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRecordStatus);
            this.colRecordStatus.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.ProductId };
        }

        public PSEItemRow NewPSEItemRow()
        {
            return (PSEItemRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new PSEItemRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(PSEItemRow);
        }
    }
}
