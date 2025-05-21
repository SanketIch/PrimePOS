
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class TransDetailRXData : DataSet 
	{

		private TransDetailRXTable _TransDetailRXTable;

		#region Constructors
		public TransDetailRXData() 
		{
			this.InitClass();
		}

		#endregion


		public TransDetailRXTable TransDetailRX 
		{
			get 
			{
				return this._TransDetailRXTable;
			}
			set 
			{
				this._TransDetailRXTable = value;
			}
		}

		public override DataSet Clone() 
		{
			TransDetailRXData cln = (TransDetailRXData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_TransDetailRXTable = (TransDetailRXTable)this.Tables[clsPOSDBConstants.TransDetailRX_tbl];
			if (_TransDetailRXTable != null) 
			{
				_TransDetailRXTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.TransDetailRX_tbl;
			this.Prefix = "";
			_TransDetailRXTable = new TransDetailRXTable();
			this.Tables.Add(this.TransDetailRX);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.TransDetailRX_tbl] != null) 
			{
				this.Tables.Add(new TransDetailRXTable(ds.Tables[clsPOSDBConstants.TransDetailRX_tbl]));
			}

			this.DataSetName = ds.DataSetName;
			this.Prefix = ds.Prefix;
			this.Namespace = ds.Namespace;
			this.Locale = ds.Locale;
			this.CaseSensitive = ds.CaseSensitive;
			this.EnforceConstraints = ds.EnforceConstraints;
			this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
			this.InitVars();
		}

		#endregion

		private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) 
		{
			if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) 
			{
				this.InitVars();
			}
		}


        //Shifted from DirectPrint.cs by Shitaljit(QuicSolv) on May 16 2011
        // this routine looks up the oRxDetailData and returns all the patients in the transaction in the Patient table.
        // added by akbar
        //added by akbar
        public PharmData.PharmBL PharmData()
        {
            if (mPharmData == null)
                mPharmData = new PharmData.PharmBL();

            return mPharmData;
        }
        private DataTable PatientTbl(long Patientno)
        {
            bool bGetpatient = false;
            if (tblPatient == null)
                bGetpatient = true;
            else
            {
                if (tblPatient.Rows.Count > 0 && MMSUtil.UtilFunc.ValorZeroL(tblPatient.Rows[0]["Patientno"].ToString()) == Patientno)
                    bGetpatient = false;
                else
                    bGetpatient = true;
            }
            if (bGetpatient)
            {
                tblPatient = this.PharmData().GetPatient(Patientno.ToString());
            }
            return tblPatient;
        }
       
        //Following function is move from DirectPrint.cs By shitaljit(QuicSolv) on 17 May 2011
        //added by akbar
        private PharmData.PharmBL mPharmData;
        private DataTable tblPatient = null;
        //
        public DataTable PatientInfo(TransDetailRXData oTRxDetailData)
        {
            if (this.tblPatient != null)
                return this.tblPatient;

            DataTable dtPat = new DataTable();
            if (oTRxDetailData.TransDetailRX.Rows.Count > 0)
            {
                string sPatno;
                DataTable tPat;
                bool bPatFound = false;

                foreach (DataRow dr in oTRxDetailData.TransDetailRX)
                {
                    sPatno = dr["Patientno"].ToString();

                    if (dtPat.Rows.Count > 0)
                    {
                        if (dtPat.Select("Patientno='" + sPatno + "'").Length == 0)
                            bPatFound = false;
                        else
                            bPatFound = true;
                    }
                    else
                        bPatFound = false;

                    if (!bPatFound)
                    {
                        tPat = this.PatientTbl(MMSUtil.UtilFunc.ValorZeroL(sPatno));
                        if (dtPat == null || dtPat.Rows.Count == 0)
                            dtPat = tPat.Clone();
                        if (tPat != null && tPat.Rows.Count > 0)
                        {
                            DataRow drp = tPat.Rows[0];
                            dtPat.ImportRow(drp);
                        }
                    }
                }
            }
            this.tblPatient = dtPat;
            return this.tblPatient;
        }
	}



}
