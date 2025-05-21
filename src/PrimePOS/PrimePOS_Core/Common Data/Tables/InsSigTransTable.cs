//----------------------------------------------------------------------------------------------------
//PRIMEPOS-2339 03-Oct-2016 JY Added to maintain item InsSigTrans
//----------------------------------------------------------------------------------------------------

namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class InsSigTransTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colID;
        private DataColumn colTransID;
        private DataColumn colPatientNo;
        private DataColumn colInsType;
        private DataColumn colTransData;
        private DataColumn colTransSigData;
        private DataColumn colCounselingReq;
        private DataColumn colSigType;
        private DataColumn colBinarySign;
        private DataColumn colPrivacyPatAccept;
        private DataColumn colPrivacyText;
        private DataColumn colPrivacySig;
        private DataColumn colPrivacySigType;
        private DataColumn colPrivacyBinarySign;

        #region Constants
        private const String _TableName = "InsSigTrans";
        #endregion

        #region Constructors
        internal InsSigTransTable() : base(_TableName) { this.InitClass(); }
        internal InsSigTransTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public InsSigTransRow this[int index]
        {
            get
            {
                return ((InsSigTransRow)(this.Rows[index]));
            }
        }

        public DataColumn ID
        {
            get
            {
                return this.colID;
            }
        }

        public DataColumn TransID
        {
            get
            {
                return this.colTransID;
            }
        }

        public DataColumn PatientNo
        {
            get
            {
                return this.colPatientNo;
            }
        }

        public DataColumn InsType
        {
            get
            {
                return this.colInsType;
            }
        }

        public DataColumn TransData
        {
            get
            {
                return this.colTransData;
            }
        }

        public DataColumn TransSigData
        {
            get
            {
                return this.colTransSigData;
            }
        }

        public DataColumn CounselingReq
        {
            get
            {
                return this.colCounselingReq;
            }
        }

        public DataColumn SigType
        {
            get
            {
                return this.colSigType;
            }
        }

        public DataColumn BinarySign
        {
            get
            {
                return this.colBinarySign;
            }
        }

        public DataColumn PrivacyPatAccept
        {
            get
            {
                return this.colPrivacyPatAccept;
            }
        }

        public DataColumn PrivacyText
        {
            get
            {
                return this.colPrivacyText;
            }
        }

        public DataColumn PrivacySig
        {
            get
            {
                return this.colPrivacySig;
            }
        }

        public DataColumn PrivacySigType
        {
            get
            {
                return this.colPrivacySigType;
            }
        }

        public DataColumn PrivacyBinarySign
        {
            get
            {
                return this.colPrivacyBinarySign;
            }
        }
        #endregion

        #region Add and Get Methods
        public void AddRow(InsSigTransRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(InsSigTransRow row, bool preserveChanges)
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
        public InsSigTransRow AddRow(System.Int32 iID,
                                        System.Int32 iTransID,
                                        System.Int32 iPatientNo,
                                        System.String sInsType,
                                        System.String sTransData,
                                        System.String sTransSigData,
                                        System.String sCounselingReq,
                                        System.String sSigType,
                                        System.Byte[] BinarySign
                                        )
        {
            InsSigTransRow row = (InsSigTransRow)this.NewRow();

            row.ID = iID;
            row.TransID = iTransID;
            row.PatientNo = iPatientNo;
            row.InsType = sInsType;
            row.TransData = sTransData;
            row.TransSigData = sTransSigData;
            row.CounselingReq = sCounselingReq;
            row.SigType = sSigType;
            row.BinarySign = BinarySign;
            this.Rows.Add(row);
            return row;
        }

        public InsSigTransRow AddRow(System.Int32 iID,
                                        System.Int32 iTransID,
                                        System.Int32 iPatientNo,
                                        System.String sInsType,
                                        System.String sTransData,
                                        System.String sTransSigData,
                                        System.String sCounselingReq,
                                        System.String sSigType,
                                        System.Byte[] BinarySign,
                                        System.String sPrivacyPatAccept,
                                        System.String sPrivacyText,
                                        System.String sPrivacySig,
                                        System.String sPrivacySigType,
                                        System.Byte[] PrivacyBinarySign
                                        )
        {
            InsSigTransRow row = (InsSigTransRow)this.NewRow();

            row.ID = iID;
            row.TransID = iTransID;
            row.PatientNo = iPatientNo;
            row.InsType = sInsType;
            row.TransData = sTransData;
            row.TransSigData = sTransSigData;
            row.CounselingReq = sCounselingReq;
            row.SigType = sSigType;
            row.BinarySign = BinarySign;
            row.PrivacyPatAccept = sPrivacyPatAccept;
            row.PrivacyText = sPrivacyText;
            row.PrivacySig = sPrivacySig;
            row.PrivacySigType = sPrivacySigType;
            row.PrivacyBinarySign = PrivacyBinarySign;
            this.Rows.Add(row);
            return row;
        }

        public InsSigTransRow GetRowByID(System.Int64 iID)
        {
            return (InsSigTransRow)this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt)
        {
            InsSigTransRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (InsSigTransRow)this.NewRow();

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_Id] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_Id] = 0;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_Id] = Convert.ToInt32((dr[clsPOSDBConstants.InsSigTrans_Fld_Id].ToString() == "") ? "0" : dr[clsPOSDBConstants.InsSigTrans_Fld_Id].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_TransID] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_TransID] = 0;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_TransID] = Convert.ToInt32((dr[clsPOSDBConstants.InsSigTrans_Fld_TransID].ToString() == "") ? "0" : dr[clsPOSDBConstants.InsSigTrans_Fld_TransID].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_PatientNo] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_PatientNo] = 0;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_PatientNo] = Convert.ToInt32((dr[clsPOSDBConstants.InsSigTrans_Fld_PatientNo].ToString() == "") ? "0" : dr[clsPOSDBConstants.InsSigTrans_Fld_PatientNo].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_InsType] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_InsType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_InsType] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_InsType].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_InsType].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_TransData] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_TransData] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_TransData] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_TransData].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_TransData].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_TransSigData] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_TransSigData] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_TransSigData] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_TransSigData].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_TransSigData].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_CounselingReq] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_CounselingReq] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_CounselingReq] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_CounselingReq].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_CounselingReq].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_SigType] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_SigType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_SigType] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_SigType].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_SigType].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_BinarySign] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_BinarySign] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_BinarySign] = (byte[])dr[clsPOSDBConstants.InsSigTrans_Fld_BinarySign];

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacyPatAccept] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacyPatAccept] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacyPatAccept] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacyPatAccept].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacyPatAccept].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacyText] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacyText] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacyText] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacyText].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacyText].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacySig] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacySig] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacySig] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacySig].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacySig].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacySigType] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacySigType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacySigType] = Convert.ToString((dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacySigType].ToString() == "") ? "" : dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacySigType].ToString());

                if (dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacyBinarySign] == DBNull.Value)
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacyBinarySign] = DBNull.Value;
                else
                    row[clsPOSDBConstants.InsSigTrans_Fld_PrivacyBinarySign] = (byte[])dr[clsPOSDBConstants.InsSigTrans_Fld_PrivacyBinarySign];

                this.AddRow(row);
            }
        }
        #endregion

        public override DataTable Clone()
        {
            InsSigTransTable cln = (InsSigTransTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new InsSigTransTable();
        }

        internal void InitVars()
        {
            this.colID = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_Id];
            this.colTransID = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_TransID];
            this.colPatientNo = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_PatientNo];
            this.colInsType = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_InsType];
            this.colTransData = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_TransData];
            this.colTransSigData = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_TransSigData];
            this.colCounselingReq = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_CounselingReq];
            this.colSigType = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_SigType];
            this.colBinarySign = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_BinarySign];
            this.colPrivacyPatAccept = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_PrivacyPatAccept];
            this.colPrivacyText = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_PrivacyText];
            this.colPrivacySig = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_PrivacySig];
            this.colPrivacySigType = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_PrivacySigType];
            this.colPrivacyBinarySign = this.Columns[clsPOSDBConstants.InsSigTrans_Fld_PrivacyBinarySign];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colID = new DataColumn("ID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colID);
            this.colID.AllowDBNull = true;

            this.colTransID = new DataColumn("TransID", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransID);
            this.colTransID.AllowDBNull = true;

            this.colPatientNo = new DataColumn("PatientNo", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPatientNo);
            this.colPatientNo.AllowDBNull = true;

            this.colInsType = new DataColumn("InsType", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colInsType);
            this.colInsType.AllowDBNull = true;

            this.colTransData = new DataColumn("TransData", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransData);
            this.colTransData.AllowDBNull = true;

            this.colTransSigData = new DataColumn("TransSigData", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransSigData);
            this.colTransSigData.AllowDBNull = true;

            this.colCounselingReq = new DataColumn("CounselingReq", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCounselingReq);
            this.colCounselingReq.AllowDBNull = true;

            this.colSigType = new DataColumn("SigType", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSigType);
            this.colSigType.AllowDBNull = true;

            this.colBinarySign = new DataColumn("BinarySign", typeof(System.Byte[]), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colBinarySign);
            this.colBinarySign.AllowDBNull = true;

            this.colPrivacyPatAccept = new DataColumn("PrivacyPatAccept", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPrivacyPatAccept);
            this.colPrivacyPatAccept.AllowDBNull = true;

            this.colPrivacyText = new DataColumn("PrivacyText", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPrivacyText);
            this.colPrivacyText.AllowDBNull = true;

            this.colPrivacySig = new DataColumn("PrivacySig", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPrivacySig);
            this.colPrivacySig.AllowDBNull = true;

            this.colPrivacySigType = new DataColumn("PrivacySigType", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPrivacySigType);
            this.colPrivacySigType.AllowDBNull = true;

            this.colPrivacyBinarySign = new DataColumn("PrivacyBinarySign", typeof(System.Byte[]), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPrivacyBinarySign);
            this.colPrivacyBinarySign.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colID };
        }

        public InsSigTransRow NewInsSigTransRow()
        {
            return (InsSigTransRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new InsSigTransRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(InsSigTransRow);
        }
    }
}
