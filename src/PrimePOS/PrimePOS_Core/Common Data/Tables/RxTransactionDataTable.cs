using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_Core.CommonData.Rows;
using System.Data;

namespace POS_Core.CommonData.Tables
{
    public class RxTransactionDataTable : DataTable, System.Collections.IEnumerable
    {

        private DataColumn colRxTransNo;
        private DataColumn colPatientID;
        private DataColumn colRxNo;
        private DataColumn colNrefill;
        private DataColumn colPickedUp;
        private DataColumn colPickUpPOS;
        private DataColumn colTransID;
        private DataColumn colIsDelivery;
        private DataColumn colIsRxSync;
        private DataColumn colTransAmount;
        private DataColumn colStationID;
        private DataColumn colUserID;
        private DataColumn colTransDate;
        private DataColumn colConsentTextID;
        private DataColumn colConsentTypeID;
        private DataColumn colConsentStatusID;
        private DataColumn colConsentCaptureDate;
        private DataColumn colConsentEffectiveDate;
        private DataColumn colConsentEndDate;
        private DataColumn colPatConsentRelationID;
        private DataColumn colSigneeName;
        private DataColumn colSignatureData;
        private DataColumn colPickUpDate;
        private DataColumn colCopayPaid;
        private DataColumn colPackDATESIGNED;
        private DataColumn colPackPATACCEPT;
        private DataColumn colPackPRIVACYTEXT;
        private DataColumn colPackPRIVACYSIG;
        private DataColumn colPackSIGTYPE;
        private DataColumn colPackBinarySign;
        private DataColumn colPackEventType;
        private DataColumn colDeliveryStatus;
        private DataColumn colConsentSourceID;//PRIMEPOS-2866,PRIMEPOS-2871
        private DataColumn colIsConsentSkip;//PRIMEPOS-2866,PRIMEPOS-2871
        private DataColumn colPartialFillNo;

        #region Constructors 
        internal RxTransactionDataTable() : base(clsPOSDBConstants.RxTransactionData_tbl) { this.InitClass(); }
        internal RxTransactionDataTable(DataTable table) : base(table.TableName) { }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public RxTransactionDataRow this[int index]
        {
            get
            {
                return ((RxTransactionDataRow)(this.Rows[index]));
            }
        }

        public DataColumn RxTransNo
        {
            get
            {
                return this.colRxTransNo;
            }
        }

        public DataColumn PatientID
        {
            get
            {
                return this.colPatientID;
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

        public DataColumn PartialFillNo
        {
            get
            {
                return this.colPartialFillNo;
            }
        }

        public DataColumn PickedUp
        {
            get
            {
                return this.colPickedUp;
            }
        }

        public DataColumn PickUpPOS
        {
            get
            {
                return this.colPickUpPOS;
            }
        }

        public DataColumn TransID
        {
            get
            {
                return this.colTransID;
            }
        }
        public DataColumn IsDelivery
        {
            get { return this.colIsDelivery; }
        }

        public DataColumn IsRxSync
        {
            get
            {
                return this.colIsRxSync;
            }
        }

        public DataColumn TransAmount
        {
            get
            {
                return this.colTransAmount;
            }
        }
        public DataColumn StationID
        {
            get { return this.colStationID; }
        }

        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
        }

        public DataColumn TransDate
        {
            get
            {
                return this.colTransDate;
            }
        }
        public DataColumn ConsentTextID
        {
            get
            {
                return this.colConsentTextID;
            }
        }
        public DataColumn ConsentTypeID
        {
            get
            {
                return this.colConsentTypeID;
            }
        }
        public DataColumn ConsentStatusID
        {
            get
            {
                return this.colConsentStatusID;
            }
        }
        public DataColumn ConsentCaptureDate
        {
            get
            {
                return this.colConsentCaptureDate;
            }
        }
        public DataColumn ConsentEffectiveDate
        {
            get
            {
                return this.colConsentEffectiveDate;
            }
        }
        public DataColumn ConsentEndDate
        {
            get
            {
                return this.colConsentEndDate;
            }
        }
        public DataColumn PatConsentRelationID
        {
            get
            {
                return this.colPatConsentRelationID;
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

        public DataColumn PickUpDate
        {
            get
            {
                return this.colPickUpDate;
            }
        }

        public DataColumn CopayPaid
        {
            get
            {
                return this.colCopayPaid;
            }
        }

        public DataColumn PackDATESIGNED
        {
            get
            {
                return this.colPackDATESIGNED;
            }
        }
        public DataColumn PackPATACCEPT
        {
            get
            {
                return this.colPackPATACCEPT;
            }
        }
        public DataColumn PackPRIVACYTEXT
        {
            get
            {
                return this.colPackPRIVACYTEXT;
            }
        }
        public DataColumn PackPRIVACYSIG
        {
            get
            {
                return this.colPackPRIVACYSIG;
            }
        }
        public DataColumn PackSIGTYPE
        {
            get
            {
                return this.colPackSIGTYPE;
            }
        }
        public DataColumn PackBinarySign
        {
            get
            {
                return this.colPackBinarySign;
            }
        }
        public DataColumn PackEventType
        {
            get
            {
                return this.colPackEventType;
            }
        }
        public DataColumn DeliveryStatus
        {
            get
            {
                return this.colDeliveryStatus;
            }
        }
        public DataColumn IsConsentSkip//PRIMEPOS-2866,PRIMEPOS-2871
        {
            get { return this.colIsConsentSkip; }
        }
        #endregion //Properties

        #region Add and Get Methods 

        public void AddRow(RxTransactionDataRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(RxTransactionDataRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.RxTransNo) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public RxTransactionDataRow GetRowByID(System.Int32 RxTransID)
        {
            return (RxTransactionDataRow)this.Rows.Find(new object[] { RxTransID });
        }


        public RxTransactionDataRow NewRxTransactionDataRow()
        {
            int RxTransNo = 1;
            foreach (RxTransactionDataRow oRow in this.Rows)
            {
                if (oRow.RxTransNo >= RxTransNo)
                {
                    RxTransNo = oRow.RxTransNo + 1;
                }
            }
            RxTransactionDataRow oNewRow = (RxTransactionDataRow)this.NewRow();
            oNewRow.RxTransNo = RxTransNo;
            return oNewRow;

        }

        public RxTransactionDataRow AddRow(
            System.String RxTransNo
            ,
            System.Int32 PatientID
                                        , System.Int64 RxNo
                                        , System.Int16 Nrefill
                                        , System.String PickedUp
                                        , System.String PickUpPOS
                                        , System.Int32 TransID
                                        , System.Boolean IsDelivery
                                        , System.Boolean IsRxSync
                                        , System.String TransAmount
                                        , System.String StationID
                                        , System.String UserID
                                        , System.DateTime TransDate
                                        , System.Int32 colConsentTextID
                                        , System.Int32 colConsentTypeID
                                        , System.Int32 colConsentStatusID
                                        , System.DateTime colConsentCaptureDate
                                        , System.DateTime colConsentEffectiveDate
                                        , System.DateTime colConsentEndDate
                                        , System.Int32 colPatConsentRelationID
                                        , System.String colSigneeName
                                        , System.Byte[] colSignatureData
                                        , System.DateTime colPickUpDate
                                        , System.Boolean colCopayPaid
                                        , System.Int16 PartialFillNo
            )
        {
            RxTransactionDataRow row = (RxTransactionDataRow)this.NewRow();
            row.ItemArray = new object[] {
                RxTransNo,
            PatientID,
            RxNo,
            Nrefill,
            PickedUp,
            PickUpPOS,
            TransID,
            IsDelivery,
            IsRxSync,
            TransAmount,
            StationID,
            UserID,
            TransDate,
            colConsentTextID,
            colConsentTypeID,
            colConsentStatusID,
            colConsentCaptureDate,
            colConsentEffectiveDate,
            colConsentEndDate,
            colPatConsentRelationID,
            colSigneeName,
            colSignatureData,
            colPickUpDate,
            colCopayPaid,
            PartialFillNo
            };
            this.Rows.Add(row);
            return row;
        }

        public void MergeTable(DataTable dt)
        {
            RxTransactionDataRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (RxTransactionDataRow)this.NewRow();
                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo] = Convert.ToInt32((dr[clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PatientID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PatientID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PatientID] = Convert.ToInt32((dr[clsPOSDBConstants.RxTransmissionData_Fld_PatientID].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_PatientID].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_RxNo] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_RxNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_RxNo] = Convert.ToInt64((dr[clsPOSDBConstants.RxTransmissionData_Fld_RxNo].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_RxNo].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_Nrefill] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_Nrefill] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_Nrefill] = Convert.ToInt32((dr[clsPOSDBConstants.RxTransmissionData_Fld_Nrefill].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_Nrefill].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PickedUp] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PickedUp] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PickedUp] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_PickedUp].ToString() == "") ? "" : dr[clsPOSDBConstants.RxTransmissionData_Fld_PickedUp].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS].ToString() == "") ? "" : dr[clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_TransID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_TransID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_TransID] = Convert.ToDecimal((dr[clsPOSDBConstants.RxTransmissionData_Fld_TransID].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_TransID].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery] = Convert.ToBoolean((dr[clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync] = Convert.ToBoolean((dr[clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync].ToString());

                //if (dr[clsPOSDBConstants.RxTransmissionData_Fld_TransAmount] == DBNull.Value)
                //    row[clsPOSDBConstants.RxTransmissionData_Fld_TransAmount] = DBNull.Value;
                //else
                //    row[clsPOSDBConstants.RxTransmissionData_Fld_TransAmount] = Convert.ToDecimal((dr[clsPOSDBConstants.RxTransmissionData_Fld_TransAmount].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_TransAmount].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_StationID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_StationID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_StationID] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_StationID].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_StationID].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_UserID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_UserID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_UserID] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_UserID].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_UserID].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime] = DBNull.Value;
                else
                    if (dr[clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime].ToString().Trim() == "")
                    row[clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime] = Convert.ToDateTime(dr[clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime].ToString());


                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID].ToString());


                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate] = DBNull.Value;
                else
                   if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate].ToString().Trim() == "")
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate] = Convert.ToDateTime(dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate] = DBNull.Value;
                else
                  if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate].ToString().Trim() == "")
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate] = Convert.ToDateTime(dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate] = DBNull.Value;
                else
                 if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate].ToString().Trim() == "")
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate] = Convert.ToDateTime(dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate].ToString());



                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID].ToString());


                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_SigneeName] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_SigneeName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_SigneeName] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_SigneeName].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_SigneeName].ToString());


                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_SignatureData] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_SignatureData] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_SignatureData] = dr[clsPOSDBConstants.RxTransmissionData_Fld_SignatureData];


                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate] = DBNull.Value;
                else
                  if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate].ToString().Trim() == "")
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate] = Convert.ToDateTime(dr[clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid] = Convert.ToString((dr[clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid].ToString());


                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED] = dr[clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED];

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT] = dr[clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT];

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYTEXT] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYTEXT] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYTEXT] = dr[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYTEXT];

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG] = dr[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG];

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE] =DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE] = dr[clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE];

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign] = dr[clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign];

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PackEventType] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackEventType] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PackEventType] = dr[clsPOSDBConstants.RxTransmissionData_Fld_PackEventType];

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus] = dr[clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus];
                //PRIMEPOS-2866,PRIMEPOS-2871
                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentSourceID] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentSourceID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_ConsentSourceID] = dr[clsPOSDBConstants.RxTransmissionData_Fld_ConsentSourceID];                
                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip] = Convert.ToBoolean((dr[clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip].ToString());

                if (dr[clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo] == DBNull.Value)
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo] = Convert.ToInt32((dr[clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo].ToString() == "") ? "0" : dr[clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo].ToString());

                this.AddRow(row);
            }
        }

        #endregion
        public override DataTable Clone()
        {
            RxTransactionDataTable cln = (RxTransactionDataTable)base.Clone();
            cln.InitVars();
            return cln;
        }
        protected override DataTable CreateInstance()
        {
            return new TransDetailTaxTable();
        }

        internal void InitVars()
        {
            this.colRxTransNo = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo];
            this.colPatientID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PatientID];
            this.colRxNo = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_RxNo];
            this.colNrefill = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_Nrefill];
            this.colPickedUp = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PickedUp];
            this.colPickUpPOS = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS];
            this.colTransID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_TransID];
            this.colIsDelivery = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery];
            this.colIsRxSync = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync];
            this.colTransAmount = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_TransAmount];
            this.colStationID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_StationID];
            this.colUserID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_UserID];
            this.colTransDate = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime];
            this.colConsentTextID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID];
            this.colConsentTypeID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID];
            this.colConsentStatusID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID];
            this.colConsentCaptureDate = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate];
            this.colConsentEffectiveDate = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate];
            this.colConsentEndDate = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate];
            this.colPatConsentRelationID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID];
            this.colSigneeName = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_SigneeName];
            this.colSignatureData = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_SignatureData];
            this.colPickUpDate = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate];
            this.colCopayPaid = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid];

            this.colPackDATESIGNED = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED];
            this.colPackPATACCEPT = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT];
            this.colPackPRIVACYTEXT = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG];
            this.colPackPRIVACYSIG = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE];
            this.colPackSIGTYPE = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE];
            this.colPackBinarySign = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign];
            this.colPackEventType = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PackEventType];
            this.colDeliveryStatus = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus];
            this.colConsentSourceID = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_ConsentSourceID];//PRIMEPOS-2866,PRIMEPOS-2871
            this.colIsConsentSkip = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip];//PRIMEPOS-2866,PRIMEPOS-2871
            this.colPartialFillNo = this.Columns[clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colRxTransNo = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_RxTransNo, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRxTransNo);
            this.colRxTransNo.AllowDBNull = true;

            this.colPatientID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PatientID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPatientID);
            this.colPatientID.AllowDBNull = true;

            this.colRxNo = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_RxNo, typeof(long), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRxNo);
            this.colRxNo.AllowDBNull = true;

            this.colNrefill = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_Nrefill, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colNrefill);
            this.colNrefill.AllowDBNull = true;

            this.colPickedUp = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PickedUp, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPickedUp);
            this.colPickedUp.AllowDBNull = true;

            this.colPickUpPOS = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PickUpPOS, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPickUpPOS);
            this.colPickUpPOS.AllowDBNull = true;

            this.colTransID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_TransID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransID);
            this.colTransID.AllowDBNull = true;

            this.colIsDelivery = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_IsDelivery, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsDelivery);
            this.colIsDelivery.AllowDBNull = true;

            this.colIsRxSync = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_IsRxSync, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsRxSync);
            this.colIsRxSync.AllowDBNull = true;

            this.colTransAmount = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_TransAmount, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransAmount);
            this.colTransAmount.AllowDBNull = true;

            this.colStationID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_StationID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStationID);
            this.colStationID.AllowDBNull = true;

            this.colUserID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            this.colUserID.AllowDBNull = true;

            this.colTransDate = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_TransDateTime, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colTransDate);
            this.colTransDate.AllowDBNull = true;

            this.colConsentTextID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_ConsentTextID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentTextID);
            this.colConsentTextID.AllowDBNull = true;

            this.colConsentTypeID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_ConsentTypeID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentTypeID);
            this.colConsentTypeID.AllowDBNull = true;


            this.colConsentStatusID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_ConsentStatusID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentStatusID);
            this.colConsentStatusID.AllowDBNull = true;

            this.colConsentCaptureDate = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_ConsentCaptureDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentCaptureDate);
            this.colConsentCaptureDate.AllowDBNull = true;


            this.colConsentEffectiveDate = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_ConsentEffectiveDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentEffectiveDate);
            this.colConsentEffectiveDate.AllowDBNull = true;

            this.colConsentEndDate = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_ConsentEndDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentEndDate);
            this.colConsentEndDate.AllowDBNull = true;

            this.colPatConsentRelationID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PatConsentRelationID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPatConsentRelationID);
            this.colPatConsentRelationID.AllowDBNull = true;

            this.colSigneeName = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_SigneeName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSigneeName);
            this.colSigneeName.AllowDBNull = true;

            this.colSignatureData = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_SignatureData, typeof(System.Byte[]), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSignatureData);
            this.colSignatureData.AllowDBNull = true;

            this.colPickUpDate = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PickUpDate, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPickUpDate);
            this.colPickUpDate.AllowDBNull = true;

            this.colCopayPaid = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_CopayPaid, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colCopayPaid);
            this.colCopayPaid.AllowDBNull = true;

            this.colPackDATESIGNED = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PackDATESIGNED, typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPackDATESIGNED);
            this.colPackDATESIGNED.AllowDBNull = true;

            this.colPackPATACCEPT = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PackPATACCEPT, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPackPATACCEPT);
            this.colPackPATACCEPT.AllowDBNull = true;

            this.colPackPRIVACYTEXT = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYTEXT, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPackPRIVACYTEXT);
            this.colPackPRIVACYTEXT.AllowDBNull = true;

            this.colPackPRIVACYSIG = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PackPRIVACYSIG, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPackPRIVACYSIG);
            this.colPackPRIVACYSIG.AllowDBNull = true;

            this.colPackSIGTYPE = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PackSIGTYPE, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPackSIGTYPE);
            this.colPackSIGTYPE.AllowDBNull = true;

            this.colPackBinarySign = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PackBinarySign, typeof(System.Byte[]), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPackBinarySign);
            this.colPackBinarySign.AllowDBNull = true;

            this.colPackEventType = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PackEventType, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPackEventType);
            this.colPackEventType.AllowDBNull = true;

            this.colDeliveryStatus = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_DeliveryStatus, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDeliveryStatus);
            this.colDeliveryStatus.AllowDBNull = true;
            //PRIMEPOS-2866,PRIMEPOS-2871
            this.colConsentSourceID = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_ConsentSourceID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colConsentSourceID);
            this.colConsentSourceID.AllowDBNull = true;
            //PRIMEPOS-2866,PRIMEPOS-2871
            this.colIsConsentSkip = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_IsConsentSkip, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsConsentSkip);
            this.colIsConsentSkip.AllowDBNull = true;

            this.colPartialFillNo = new DataColumn(clsPOSDBConstants.RxTransmissionData_Fld_PartialFillNo, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPartialFillNo);
            this.colPartialFillNo.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colRxTransNo };
        }

        public RxTransactionDataRow NewTransDetailTaxRow()
        {
            return (RxTransactionDataRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new RxTransactionDataRow(builder);
        }

        public IEnumerable<RxTransactionDataRow> AsEnumerable()
        {
            foreach (RxTransactionDataRow row in this.Rows)
            {
                yield return row;
            }
        }

    }
}
