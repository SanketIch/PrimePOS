using POS_Core.CommonData.Rows;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.CommonData.Tables
{
    public class ConsentTransmissionDataTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colConsentLogId;
        private DataColumn colConsentSourceID;
        private DataColumn colConsentTypeId;
        private DataColumn colConsentTextId;
        private DataColumn colConsentStatusId;
        private DataColumn colConsentRelationId;
        private DataColumn colPatientNo;
        private DataColumn colRxNo;
        private DataColumn colNrefill;
        private DataColumn colConsentCaptureDate;
        private DataColumn colConsentExpiryDate;
        private DataColumn colSigneeName;
        private DataColumn colSignatureData;

        #region Constructors 
        internal ConsentTransmissionDataTable() : base(clsPOSDBConstants.ConsentTransmissionData_tbl) { this.InitClass(); }
        internal ConsentTransmissionDataTable(DataTable table) : base(table.TableName) { }
        #endregion
        #region Properties

        public ConsentTransmissionDataRow this[int index]
        {
            get
            {
                return ((ConsentTransmissionDataRow)(this.Rows[index]));
            }
        }

        public DataColumn ConsentLogId
        {
            get
            {
                return this.colConsentLogId;
            }
        }

        public DataColumn ConsentSourceID
        {
            get
            {
                return this.colConsentSourceID;
            }
        }

        public DataColumn ConsentTypeId
        {
            get
            {
                return this.colConsentTypeId;
            }
        }

        public DataColumn ConsentTextId
        {
            get
            {
                return this.colConsentTextId;
            }
        }

        public DataColumn ConsentStatusId
        {
            get
            {
                return this.colConsentStatusId;
            }
        }

        public DataColumn ConsentRelationId
        {
            get
            {
                return this.colConsentRelationId;
            }
        }

        public DataColumn PatientNo
        {
            get
            {
                return this.colPatientNo;
            }
        }

        public DataColumn RxNo
        {
            get
            {
                return this.colRxNo;
            }
        }
        public DataColumn Nrefill
        {
            get
            {
                return this.colNrefill;
            }
        }

        public DataColumn ConsentCaptureDate
        {
            get
            {
                return this.colConsentCaptureDate;
            }
        }
        public DataColumn ConsentExpiryDate
        {
            get
            {
                return this.colConsentExpiryDate;
            }
        }

        public DataColumn SigneeName
        {
            get
            {
                return this.colSigneeName;
            }
        }

        public DataColumn SignatureData
        {
            get
            {
                return this.colSignatureData;
            }
        }
        #endregion
        #region Add and Get Methods 

        public void AddRow(ConsentTransmissionDataRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(ConsentTransmissionDataRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.ConsentLogId) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public ConsentTransmissionDataRow GetRowByID(int consentLogId)
        {
            return (ConsentTransmissionDataRow)this.Rows.Find(new object[] { consentLogId });
        }


        public ConsentTransmissionDataRow NewConsentTransmissionDataRow()
        {
            int consentLogId = 1;
            foreach (ConsentTransmissionDataRow oRow in this.Rows)
            {
                if (oRow.ConsentLogId >= consentLogId)
                {
                    consentLogId = oRow.ConsentLogId + 1;
                }
            }
            ConsentTransmissionDataRow oNewRow = (ConsentTransmissionDataRow)this.NewRow();
            oNewRow.ConsentLogId = consentLogId;
            return oNewRow;

        }

        public ConsentTransmissionDataRow AddRow(int ConsentLogId, int ConsentSourceID, int ConsentTypeId, int ConsentTextId, int ConsentStatusId, int ConsentRelationId, int PatientNo, DateTime ConsentCaptureDate, DateTime ConsentExpiryDate, string SigneeName, byte[] SignatureData)
        {
            ConsentTransmissionDataRow row = (ConsentTransmissionDataRow)this.NewRow();
            row.ItemArray = new object[] {
                ConsentLogId, ConsentSourceID, ConsentTypeId, ConsentTextId, ConsentStatusId, ConsentRelationId, PatientNo, ConsentCaptureDate, ConsentExpiryDate, SigneeName, SignatureData
            };
            this.Rows.Add(row);
            return row;
        }
        public void MergeTable(DataTable dt)
        {
            ConsentTransmissionDataRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (ConsentTransmissionDataRow)this.NewRow();
                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId] = Convert.ToInt32((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId].ToString() == "") ? "0" : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId].ToString());

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID] = Convert.ToInt32((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID].ToString() == "") ? "0" : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID].ToString());

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId] = Convert.ToInt32((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId].ToString() == "") ? "0" : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId].ToString());

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId] = Convert.ToInt32((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId].ToString() == "") ? 0 : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId]);

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId] = Convert.ToInt32((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId].ToString() == "") ? 0 : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId]);

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId] = Convert.ToInt32((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId].ToString() == "") ? 0 : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId]);

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo] = Convert.ToInt32((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo].ToString() == "") ? 0 : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo]);

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo] = Convert.ToInt64((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo].ToString() == "") ? 0 : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo]);

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill] = Convert.ToInt32((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill].ToString() == "") ? 0 : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill]);

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate] = Convert.ToDateTime((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate].ToString() == "") ? DateTime.MinValue : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate]);

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate] = Convert.ToDateTime(
                        (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate].ToString() == "") ? DateTime.MinValue : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate]);

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName] = Convert.ToString((dr[clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName].ToString() == "") ? string.Empty : dr[clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName].ToString());

                if (dr[clsPOSDBConstants.ConsentTransmissionData_Fld_SignatureData] == DBNull.Value)
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_SignatureData] = DBNull.Value;
                else
                    row[clsPOSDBConstants.ConsentTransmissionData_Fld_SignatureData] = dr[clsPOSDBConstants.ConsentTransmissionData_Fld_SignatureData];

                this.AddRow(row);
            }
        }
        #endregion
        #region Common Methods
        public override DataTable Clone()
        {
            ConsentTransmissionDataTable cln = (ConsentTransmissionDataTable)base.Clone();
            cln.InitVars();
            return cln;
        }
        internal void InitVars()
        {
            this.colConsentLogId = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId];
            this.colConsentSourceID = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID];
            this.colConsentTypeId = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId];
            this.colConsentTextId = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId];
            this.colConsentStatusId = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId];
            this.colConsentRelationId = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId];
            this.colPatientNo = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo];
            this.colRxNo = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo];
            this.colNrefill = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill];
            this.colConsentCaptureDate = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate];
            this.colConsentExpiryDate = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate];
            this.colSigneeName = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName];
            this.colSignatureData = this.Columns[clsPOSDBConstants.ConsentTransmissionData_Fld_SignatureData];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colConsentLogId = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentLogId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentLogId);
            this.colConsentLogId.AllowDBNull = true;

            this.colConsentSourceID = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentSourceID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentSourceID);
            this.colConsentSourceID.AllowDBNull = true;

            this.colConsentTypeId = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTypeId, typeof(Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentTypeId);
            this.colConsentTypeId.AllowDBNull = true;

            this.colConsentTextId = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentTextId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentTextId);
            this.colConsentTextId.AllowDBNull = true;

            this.colConsentStatusId = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentStatusId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentStatusId);
            this.colConsentStatusId.AllowDBNull = true;

            this.colConsentRelationId = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentRelationId, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentRelationId);
            this.colConsentRelationId.AllowDBNull = true;

            this.colPatientNo = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_PatientNo, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPatientNo);
            this.colPatientNo.AllowDBNull = true;

            this.colRxNo = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_RxNo, typeof(System.Int64), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRxNo);
            this.colRxNo.AllowDBNull = true;

            this.colNrefill = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_Nrefill, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNrefill);
            this.colNrefill.AllowDBNull = true;

            this.colConsentCaptureDate = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentCaptureDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentCaptureDate);
            this.colConsentCaptureDate.AllowDBNull = true;

            this.colConsentExpiryDate = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_ConsentExpiryDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentExpiryDate);
            this.colConsentExpiryDate.AllowDBNull = true;

            this.colSigneeName = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_SigneeName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSigneeName);
            this.colSigneeName.AllowDBNull = true;

            this.colSignatureData = new DataColumn(clsPOSDBConstants.ConsentTransmissionData_Fld_SignatureData, typeof(System.Byte[]), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSignatureData);
            this.colSignatureData.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colConsentLogId };
        }

        public ConsentTransmissionDataRow NewTransDetailTaxRow()
        {
            return (ConsentTransmissionDataRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ConsentTransmissionDataRow(builder);
        }

        public IEnumerable<ConsentTransmissionDataRow> AsEnumerable()
        {
            foreach (ConsentTransmissionDataRow row in this.Rows)
            {
                yield return row;
            }
        }
        #endregion
    }
}
