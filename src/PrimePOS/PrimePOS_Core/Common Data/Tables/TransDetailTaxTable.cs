
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
    using System.Collections.Generic;
    //using POS.Resources;

	public class TransDetailTaxTable : DataTable, System.Collections.IEnumerable 
	{
        private DataColumn colTransDetailTaxlID;
		private DataColumn colTransDetailID;
		private DataColumn colTransID;
		private DataColumn colTaxPercent;
		private DataColumn colTaxAmount;
		private DataColumn colItemID;
        private DataColumn colTaxID;
        private DataColumn colItemRow;

		#region Constructors 
		internal TransDetailTaxTable() : base(clsPOSDBConstants.TransDetailTax_tbl) { this.InitClass(); }
		internal TransDetailTaxTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public TransDetailTaxRow this[int index] 
		{
			get 
			{
				return ((TransDetailTaxRow)(this.Rows[index]));
			}
		}

        public DataColumn TransDetailTaxID
        {
            get
            {
                return this.colTransDetailTaxlID;
            }
        }

		public DataColumn TransDetailID 
		{
			get 
			{
				return this.colTransDetailID;
			}
		}

		public DataColumn TransID 
		{
			get 
			{
				return this.colTransID;
			}
		}


        public DataColumn TaxPercent
		{
			get 
			{
				return this.colTaxPercent;
			}
		}


		public DataColumn TaxAmount
		{
			get 
			{
				return this.colTaxAmount;
			}
		}

		public DataColumn ItemID
		{
			get 
			{
				return this.colItemID;
			}
		}

		public DataColumn TaxID
		{
			get 
			{
				return this.colTaxID;
			}
		}
        public DataColumn ItemRow
        {
            get { return this.colItemRow; }
        }
		#endregion //Properties

		#region Add and Get Methods 

		public  void AddRow(TransDetailTaxRow row) 
		{
			AddRow(row, false);
		}
		
		public  void AddRow(TransDetailTaxRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.TransDetailID) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}
		
		public TransDetailTaxRow GetRowByID(System.Int32 TransDetailID) 
		{
			return (TransDetailTaxRow)this.Rows.Find(new object[] {TransDetailID});
		}


		public TransDetailTaxRow AddRow(System.Int32 TransDetailTaxID
                                        , System.Int32 TransDetailID 
										,System.Int32 TransID 
										,System.Decimal TaxPercent
										,System.Decimal TaxAmount
                                        , System.String ItemID
										, System.Int32  TaxID
                                        , System.Int32 ItemRow)
		{
			TransDetailTaxRow row = (TransDetailTaxRow)this.NewRow();
            row.ItemArray = new object[] { TransDetailTaxID, TransDetailID, TransID, TaxPercent, TaxAmount,ItemID, TaxID, ItemRow };
			this.Rows.Add(row);
			return row;
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			TransDetailTaxRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (TransDetailTaxRow)this.NewRow();
                if (dr[clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID].ToString());

				if (dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_TransDetailID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_TransDetailID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID].ToString()=="")?"0":dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID].ToString());
				
				if (dr[clsPOSDBConstants.TransDetail_Fld_ItemID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_ItemID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_ItemID] = Convert.ToString((dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString()=="")? "":dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString());
				

				if (dr[clsPOSDBConstants.TransDetail_Fld_TransID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_TransID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_TransID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetail_Fld_TransID].ToString()=="")?"0":dr[clsPOSDBConstants.TransDetail_Fld_TransID].ToString());


				if (dr[clsPOSDBConstants.TransDetailTax_fld_TaxPercent] == DBNull.Value)
                    row[clsPOSDBConstants.TransDetailTax_fld_TaxPercent] = DBNull.Value;
				else
                    row[clsPOSDBConstants.TransDetailTax_fld_TaxPercent] = Convert.ToDecimal((dr[clsPOSDBConstants.TransDetailTax_fld_TaxPercent].ToString() == "") ? "0" : dr[clsPOSDBConstants.TransDetailTax_fld_TaxPercent].ToString());
				
                if (dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_TaxAmount] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_TaxAmount] = Convert.ToDecimal((dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString()=="")? "0":dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString());

				if (dr[clsPOSDBConstants.TransDetail_Fld_TaxID] == DBNull.Value) 
					row[clsPOSDBConstants.TransDetail_Fld_TaxID] = DBNull.Value;
				else
					row[clsPOSDBConstants.TransDetail_Fld_TaxID] = Convert.ToInt32((dr[clsPOSDBConstants.TransDetail_Fld_TaxID].ToString()=="")?"0":dr[clsPOSDBConstants.TransDetail_Fld_TaxID].ToString());


                this.AddRow(row);
			}
		}
		
		#endregion 
		public override DataTable Clone() 
		{
			TransDetailTaxTable cln = (TransDetailTaxTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new TransDetailTaxTable();
		}

        internal void InitVars()
        {
            this.colTransDetailTaxlID = this.Columns[clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID];
            this.colTransDetailID = this.Columns[clsPOSDBConstants.TransDetail_Fld_TransDetailID];
            this.colTransID = this.Columns[clsPOSDBConstants.TransDetail_Fld_TransID];
            this.colTaxPercent = this.Columns[clsPOSDBConstants.TransDetailTax_fld_TaxPercent];
            this.colTaxAmount = this.Columns[clsPOSDBConstants.TransDetail_Fld_TaxAmount];
            this.colItemID = this.Columns[clsPOSDBConstants.TransDetail_Fld_ItemID];
            this.colTaxID = this.Columns[clsPOSDBConstants.TransDetail_Fld_TaxID];
           

        }

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
            this.colTransDetailTaxlID = new DataColumn(clsPOSDBConstants.TransDetailTax_fld_TransDetailTaxID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransDetailTaxlID);
            this.colTransDetailTaxlID.AllowDBNull = true;

			this.colTransDetailID= new DataColumn(clsPOSDBConstants.TransDetail_Fld_TransDetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransDetailID);
			this.colTransDetailID.AllowDBNull = true;

			this.colTransID = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TransID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTransID);
			this.colTransID.AllowDBNull = true;

            this.colTaxPercent = new DataColumn(clsPOSDBConstants.TransDetailTax_fld_TaxPercent, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTaxPercent);
            this.colTaxPercent.AllowDBNull = true;

			this.colTaxAmount = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TaxAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTaxAmount);
			this.colTaxAmount.AllowDBNull = true;

			this.colItemID= new DataColumn(clsPOSDBConstants.TransDetail_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colItemID);
			this.colItemID.AllowDBNull = true;
            
			this.colTaxID = new DataColumn(clsPOSDBConstants.TransDetail_Fld_TaxID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colTaxID);
			this.colTaxID.AllowDBNull = true;

            this.colItemRow = new DataColumn(clsPOSDBConstants.TransDetail_Fld_ItemRow, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colItemRow);
            this.colItemRow.AllowDBNull = true;
            this.PrimaryKey = new DataColumn[] {this.colTransDetailTaxlID};
		}

		public  TransDetailTaxRow NewTransDetailTaxRow() 
		{
			return (TransDetailTaxRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new TransDetailTaxRow(builder);
		}

        public IEnumerable<TransDetailTaxRow> AsEnumerable()
        {
            foreach (TransDetailTaxRow row in this.Rows)
            {
                yield return row;
            }
        }

        public TransDetailTaxRow FindRow(String itemId)
        {
            foreach (TransDetailTaxRow row in this.Rows)
            {
                if (row.ItemID == itemId)
                    return row;
            }
            return null;
        }
	}
}
