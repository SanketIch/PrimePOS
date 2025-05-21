//Sprint-22 - PRIMEPOS-2245 15-Oct-2015 JY Added class
namespace POS_Core.CommonData.Tables 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;

    public class SystemInfoTable : DataTable, System.Collections.IEnumerable
    {
        private DataColumn colId;
        private DataColumn colPharmNo;
        private DataColumn colOSName;
        private DataColumn colVersion;
        private DataColumn colSystemName;
        private DataColumn colSystemManufacturer;
        private DataColumn colSystemModel;
        private DataColumn colSystemType;
        private DataColumn colProcessor;
        private DataColumn colRAM;
        private DataColumn colDriveInfo;
        private DataColumn colUpdatedOn;
        private DataColumn colStatus;

        #region Constants
        private const String _TableName = "SystemInfo";
        #endregion

        #region Constructors 
		internal SystemInfoTable() : base(_TableName) { this.InitClass(); }
        internal SystemInfoTable(DataTable table) : base(table.TableName) { }
		#endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public SystemInfoRow this[int index]
        {
            get
            {
                return ((SystemInfoRow)(this.Rows[index]));
            }
        }

        public DataColumn Id
        {
            get
            {
                return this.colId;
            }
        }

        public DataColumn PharmNo
        {
            get
            {
                return this.colPharmNo;
            }
        }
        
        public DataColumn OSName
        {
            get
            {
                return this.colOSName;
            }
        }

        public DataColumn Version
        {
            get
            {
                return this.colVersion;
            }
        }

        public DataColumn SystemName
        {
            get
            {
                return this.colSystemName;
            }
        }

        public DataColumn SystemManufacturer
        {
            get
            {
                return this.colSystemManufacturer;
            }
        }

        public DataColumn SystemModel
        {
            get
            {
                return this.colSystemModel;
            }
        }

        public DataColumn SystemType
        {
            get
            {
                return this.colSystemType;
            }
        }

        public DataColumn Processor
        {
            get
            {
                return this.colProcessor;
            }
        }

        public DataColumn RAM
        {
            get
            {
                return this.colRAM;
            }
        }

        public DataColumn DriveInfo
        {
            get
            {
                return this.colDriveInfo;
            }
        }

        public DataColumn UpdatedOn
        {
            get
            {
                return this.colUpdatedOn;
            }
        }

        public DataColumn Status
        {
            get
            {
                return this.colStatus;
            }
        }
        #endregion //Properties

        #region Add and Get Methods

        public void AddRow(SystemInfoRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(SystemInfoRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.Id) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public SystemInfoRow AddRow(System.Int32 Id
                                        , System.String sPharmNo
                                        , System.String sOSName
                                        , System.String sVersion
                                        , System.String sSystemName
                                        , System.String sSystemManufacturer
                                        , System.String sSystemModel
                                        , System.String sSystemType
                                        , System.String sProcessor
                                        , System.String sRAM
                                        , System.String sDriveInfo)
        {
            SystemInfoRow row = (SystemInfoRow)this.NewRow();
            row.Id = Id;
            row.PharmNo = sPharmNo;
            row.OSName = sOSName;
            row.Version = sVersion;
            row.SystemName = sSystemName;
            row.SystemManufacturer = sSystemManufacturer;
            row.SystemModel = sSystemModel;
            row.SystemType = sSystemType;
            row.Processor = sProcessor;
            row.RAM = sRAM;
            row.DriveInfo = sDriveInfo;
            row.UpdatedOn = DateTime.Now;
            row.Status = false;
            this.Rows.Add(row);
            return row;
        }

        public SystemInfoRow GetRowByID(System.Int64 iID)
        {
            return (SystemInfoRow)this.Rows.Find(new object[] { iID });
        }

        public void MergeTable(DataTable dt)
        {

            SystemInfoRow row;
            foreach (DataRow dr in dt.Rows)
            {
                row = (SystemInfoRow)this.NewRow();

                if (dr[clsPOSDBConstants.SystemInfo_Fld_Id] == DBNull.Value)
                    row[clsPOSDBConstants.SystemInfo_Fld_Id] = DBNull.Value;
                else
                    row[clsPOSDBConstants.SystemInfo_Fld_Id] = Convert.ToInt32((dr[clsPOSDBConstants.SystemInfo_Fld_Id].ToString() == "") ? "0" : dr[0].ToString());

                string strField = clsPOSDBConstants.SystemInfo_Fld_PharmNo;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_OSName;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_Version;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_SystemName;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_SystemManufacturer;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_SystemModel;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_SystemType;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_Processor;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_RAM;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                strField = clsPOSDBConstants.SystemInfo_Fld_DriveInfo;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                if (dr[clsPOSDBConstants.SystemInfo_Fld_UpdatedOn] == DBNull.Value)
                    row[clsPOSDBConstants.SystemInfo_Fld_UpdatedOn] = DBNull.Value;
                else
                    if (dr[clsPOSDBConstants.SystemInfo_Fld_UpdatedOn].ToString().Trim() == "")
                        row[clsPOSDBConstants.SystemInfo_Fld_UpdatedOn] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                    else
                        row[clsPOSDBConstants.SystemInfo_Fld_UpdatedOn] = Convert.ToDateTime(dr[clsPOSDBConstants.SystemInfo_Fld_UpdatedOn].ToString());

                if (dr[clsPOSDBConstants.SystemInfo_Fld_Status] == DBNull.Value)
                    row[clsPOSDBConstants.SystemInfo_Fld_Status] = DBNull.Value;
                else
                    row[clsPOSDBConstants.SystemInfo_Fld_Status] = Convert.ToBoolean((dr[clsPOSDBConstants.SystemInfo_Fld_Status].ToString() == "") ? "false" : dr[clsPOSDBConstants.SystemInfo_Fld_Status].ToString());

                this.AddRow(row);
            }
        }
        #endregion

        public override DataTable Clone()
        {
            SystemInfoTable cln = (SystemInfoTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new SystemInfoTable();
        }

        internal void InitVars()
        {
            this.colId = this.Columns[clsPOSDBConstants.SystemInfo_Fld_Id];
            this.colPharmNo = this.Columns[clsPOSDBConstants.SystemInfo_Fld_PharmNo];
            this.colOSName = this.Columns[clsPOSDBConstants.SystemInfo_Fld_OSName];
            this.colVersion = this.Columns[clsPOSDBConstants.SystemInfo_Fld_Version];
            this.colSystemName = this.Columns[clsPOSDBConstants.SystemInfo_Fld_SystemName];
            this.colSystemManufacturer = this.Columns[clsPOSDBConstants.SystemInfo_Fld_SystemManufacturer];
            this.colSystemModel = this.Columns[clsPOSDBConstants.SystemInfo_Fld_SystemModel];
            this.colSystemType = this.Columns[clsPOSDBConstants.SystemInfo_Fld_SystemType];
            this.colProcessor = this.Columns[clsPOSDBConstants.SystemInfo_Fld_Processor];
            this.colRAM = this.Columns[clsPOSDBConstants.SystemInfo_Fld_RAM];
            this.colDriveInfo = this.Columns[clsPOSDBConstants.SystemInfo_Fld_DriveInfo];
            this.colUpdatedOn = this.Columns[clsPOSDBConstants.SystemInfo_Fld_UpdatedOn];
            this.colStatus = this.Columns[clsPOSDBConstants.SystemInfo_Fld_Status];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        private void InitClass()
        {
            this.colId = new DataColumn("Id", typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colId);
            this.colId.AllowDBNull = true;

            this.colPharmNo = new DataColumn("PharmNo", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPharmNo);
            this.colPharmNo.AllowDBNull = true;

            this.colOSName = new DataColumn("OSName", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colOSName);
            this.colOSName.AllowDBNull = true;

            this.colVersion = new DataColumn("Version", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colVersion);
            this.colVersion.AllowDBNull = true;

            this.colSystemName = new DataColumn("SystemName", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSystemName);
            this.colSystemName.AllowDBNull = true;

            this.colSystemManufacturer = new DataColumn("SystemManufacturer", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSystemManufacturer);
            this.colSystemManufacturer.AllowDBNull = true;

            this.colSystemModel = new DataColumn("SystemModel", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSystemModel);
            this.colSystemModel.AllowDBNull = true;

            this.colSystemType = new DataColumn("SystemType", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colSystemType);
            this.colSystemType.AllowDBNull = true;

            this.colProcessor = new DataColumn("Processor", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colProcessor);
            this.colProcessor.AllowDBNull = true;

            this.colRAM = new DataColumn("RAM", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colRAM);
            this.colRAM.AllowDBNull = true;

            this.colDriveInfo = new DataColumn("DriveInfo", typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDriveInfo);
            this.colDriveInfo.AllowDBNull = true;

            this.colUpdatedOn = new DataColumn("UpdatedOn", typeof(System.DateTime), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUpdatedOn);
            this.colUpdatedOn.AllowDBNull = true;

            this.colStatus = new DataColumn("Status", typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colStatus);
            this.colStatus.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colId };
        }

        public SystemInfoRow NewSystemInfoRow()
        {
            return (SystemInfoRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new SystemInfoRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(SystemInfoRow);
        }
    }
}







