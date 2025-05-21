
namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class ColorSchemeForViewPOSTransTable : DataTable, System.Collections.IEnumerable
    {

        private DataColumn colId;
        private DataColumn colFromAmount;
        private DataColumn colToAmount;
        private DataColumn colBackColor;
        private DataColumn colForeColor;

        #region Constructors
        public ColorSchemeForViewPOSTransTable() : base(clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl) { this.InitClass(); }
        internal ColorSchemeForViewPOSTransTable(DataTable table) : base(table.TableName) { }
        #endregion
        #region Properties
        /// 
        /// Public Property for get all Rows in Table
        /// 
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public ColorSchemeForViewPOSTransRow this[int index]
        {
            get
            {
                return ((ColorSchemeForViewPOSTransRow)(this.Rows[index]));
            }
        }

        public DataColumn Id
        {
            get
            {
                return this.colId;
            }
        }


        public DataColumn BackColor
        {
            get
            {
                return this.colBackColor;
            }
        }

        public DataColumn ForeColor
        {
            get
            {
                return this.colForeColor;
            }
        }

        public DataColumn FromAmount
        {
            get
            {
                return this.colFromAmount;
            }
        }

        public DataColumn ToAmount
        {
            get
            {
                return this.colToAmount;
            }
        }
        #endregion //Properties
        #region Add and Get Methods

        public void AddRow(ColorSchemeForViewPOSTransRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(ColorSchemeForViewPOSTransRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public ColorSchemeForViewPOSTransRow AddRow(System.Int32 Id, System.Decimal FromAmount, System.Decimal ToAmount, System.String BackColor, System.String ForeColor)
        {
            ColorSchemeForViewPOSTransRow row = (ColorSchemeForViewPOSTransRow)this.NewRow();
            row.ItemArray = new object[] { Id, FromAmount,ToAmount, BackColor, ForeColor };
            this.Rows.Add(row);
            return row;
        }

        public ColorSchemeForViewPOSTransRow GetRowByID(System.Int32 Id)
        {
            return (ColorSchemeForViewPOSTransRow)this.Rows.Find(new object[] { Id });
        }

        public void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            ColorSchemeForViewPOSTransRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (ColorSchemeForViewPOSTransRow)this.NewRow();

                if (dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID] == DBNull.Value)
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID] = 0;
                else
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID] = Convert.ToInt32(dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID].ToString());
                
                if (dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount] == DBNull.Value)
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount] = Convert.ToDecimal(dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount].ToString());
               
                if (dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount] == DBNull.Value)
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount] = Convert.ToDecimal(dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount].ToString());

                if (dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor] == DBNull.Value)
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor] = Convert.ToString(dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor].ToString());

                if (dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor] == DBNull.Value)
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor] = Convert.ToString(dr[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor].ToString());

                this.AddRow(row);
            }
        }

        #endregion //Add and Get Methods
        public override DataTable Clone()
        {
            ColorSchemeForViewPOSTransTable cln = (ColorSchemeForViewPOSTransTable)base.Clone();
            cln.InitVars();
            return cln;
        }
        protected override DataTable CreateInstance()
        {
            return new ColorSchemeForViewPOSTransTable();
        }

        internal void InitVars()
        {
            this.colId = this.Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID];
            this.colFromAmount = this.Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount];
            this.colToAmount = this.Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount];
            this.colBackColor = this.Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor];
            this.colForeColor = this.Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor];
        }
        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }
        /// <summary>
        /// Initialise the DataTable
        /// </summary>
        /// <remarks>
        ///	 Add strongly typed rows with constraints
        /// </remarks>
        private void InitClass()
        {
            this.colId = new DataColumn(clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colId);
            this.colId.AllowDBNull = false;
            this.colId.AutoIncrement = true;

            this.colFromAmount = new DataColumn(clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colFromAmount);
            this.colFromAmount.AllowDBNull = true;

            this.colToAmount = new DataColumn(clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colToAmount);
            this.colFromAmount.AllowDBNull = true;

            this.colBackColor = new DataColumn(clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colBackColor);
            this.colBackColor.AllowDBNull = true;

            this.colForeColor = new DataColumn(clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colForeColor);
            this.colForeColor.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.Id };
        }
        public ColorSchemeForViewPOSTransRow NewColorSchemeForViewPOSTransRow()
        {
            return (ColorSchemeForViewPOSTransRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ColorSchemeForViewPOSTransRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(ColorSchemeForViewPOSTransRow);
        }

        #region Event Handlers

        public delegate void ColorSchemeForViewPOSTransRowChangeEventHandler(object sender, ColorSchemeForViewPOSTransRowChangeEvent e);

        public event ColorSchemeForViewPOSTransRowChangeEventHandler ColorSchemeForViewPOSTransRowChanged;
        public event ColorSchemeForViewPOSTransRowChangeEventHandler ColorSchemeForViewPOSTransRowChanging;
        public event ColorSchemeForViewPOSTransRowChangeEventHandler ColorSchemeForViewPOSTransRowDeleted;
        public event ColorSchemeForViewPOSTransRowChangeEventHandler ColorSchemeForViewPOSTransRowDeleting;

        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
            if (this.ColorSchemeForViewPOSTransRowChanged != null)
            {
                this.ColorSchemeForViewPOSTransRowChanged(this, new ColorSchemeForViewPOSTransRowChangeEvent((ColorSchemeForViewPOSTransRow)e.Row, e.Action));
            }
        }

        protected override void OnRowChanging(DataRowChangeEventArgs e)
        {
            base.OnRowChanging(e);
            if (this.ColorSchemeForViewPOSTransRowChanging != null)
            {
                this.ColorSchemeForViewPOSTransRowChanging(this, new ColorSchemeForViewPOSTransRowChangeEvent((ColorSchemeForViewPOSTransRow)e.Row, e.Action));
            }
        }

        protected override void OnRowDeleted(DataRowChangeEventArgs e)
        {
            base.OnRowDeleted(e);
            if (this.ColorSchemeForViewPOSTransRowDeleted != null)
            {
                this.ColorSchemeForViewPOSTransRowDeleted(this, new ColorSchemeForViewPOSTransRowChangeEvent((ColorSchemeForViewPOSTransRow)e.Row, e.Action));
            }
        }

        protected override void OnRowDeleting(DataRowChangeEventArgs e)
        {
            base.OnRowDeleting(e);
            if (this.ColorSchemeForViewPOSTransRowDeleting != null)
            {
                this.ColorSchemeForViewPOSTransRowDeleting(this, new ColorSchemeForViewPOSTransRowChangeEvent((ColorSchemeForViewPOSTransRow)e.Row, e.Action));
            }
        }

        #endregion
    }
}
