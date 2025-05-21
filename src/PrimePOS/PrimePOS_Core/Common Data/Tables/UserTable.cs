namespace POS_Core.CommonData.Tables
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using Resources.DelegateHandler;

    //         This class is used to define the shape of UserTable.
    public class UserTable : DataTable
    {

        private DataColumn colId;
        private DataColumn colFirstName;
        private DataColumn colLastName;
        private DataColumn colPassword;
        private DataColumn colUserID;
        private DataColumn colIsActive;
        private DataColumn colDrawNo;
        private DataColumn colMaxDiscountLimit;

        private DataColumn colMaxTransactionLimit;//Column Added By Ravindra(QuicSolv) 24 Jan 2013//
        private DataColumn colUserImage;//Column Added By Ravindra(QuicSolv) 11 Oct 2013//
        #region Constructors
        internal UserTable() : base(clsPOSDBConstants.Users_tbl) { this.InitClass(); }
        internal UserTable(DataTable table) : base(table.TableName) { }
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

        public UserRow this[int index]
        {
            get
            {
                return ((UserRow)(this.Rows[index]));
            }
        }

        public DataColumn Id
        {
            get
            {
                return this.colId;
            }
        }

        public DataColumn DrawNo
        {
            get
            {
                return this.colDrawNo;
            }
        }

        public DataColumn FirstName
        {
            get
            {
                return this.colFirstName;
            }
        }

        public DataColumn LastName
        {
            get
            {
                return this.colLastName;
            }
        }

        public DataColumn Password
        {
            get
            {
                return this.colPassword;
            }
        }

        public DataColumn UserID
        {
            get
            {
                return this.colUserID;
            }
        }

        public DataColumn IsActive
        {
            get
            {
                return this.colIsActive;
            }
        }
        public DataColumn MaxDiscountLimit
        {
            get
            {
                return this.colMaxDiscountLimit;
            }
        }
        //Following Column Added By Ravindra
        public DataColumn MaxTransactionLimit
        {
            get
            {
                return this.colMaxTransactionLimit;
            }
        }
        public DataColumn UserImage
        {
            get
            {
                return this.colUserImage;
            }
        }
        //Till Here Added By Ravindra


        #endregion //Properties

        #region Add and Get Methods

        public void AddRow(UserRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(UserRow row, bool preserveChanges)
        {
            if (this.GetRowByID(row.Id.ToString()) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        public UserRow AddRow(System.String sUserId, System.String sFirstName, System.String sLastName, System.String sPassword,
            System.Boolean bIsActive, System.Int32 iDrawNo, System.Decimal dMaxDiscountLimit, System.Decimal dMaxTransactionLimit,System.Byte[] UserImage)
        {

            UserRow row = (UserRow)this.NewRow();
            row.ItemArray = new object[] { sUserId, sFirstName, sLastName, sPassword, bIsActive, iDrawNo, dMaxDiscountLimit, dMaxTransactionLimit, UserImage };
            this.Rows.Add(row);
            return row;
        }

        public UserRow GetRowByID(System.String UserID)
        {
            return (UserRow)this.Rows.Find(new object[] { UserID });
        }

        public void MergeTable(DataTable dt)
        {
            //add any rows in the DataTable 
            UserRow row;
            try
            {

            
            foreach (DataRow dr in dt.Rows)
            {
                row = (UserRow)this.NewRow();
                if (POS_Core.Resources.Configuration.convertNullToString(dr[clsPOSDBConstants.Users_Fld_UserType]).Equals("G"))
                {
                    continue;  
                }
                if (dr[clsPOSDBConstants.Users_Fld_ID] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_ID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Users_Fld_ID] = Convert.ToInt32(dr[clsPOSDBConstants.Users_Fld_ID].ToString());

                if (dr[clsPOSDBConstants.Users_Fld_UserID] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_UserID] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Users_Fld_UserID] = Convert.ToString(dr[clsPOSDBConstants.Users_Fld_UserID].ToString());

                if (dr[clsPOSDBConstants.Users_Fld_fName] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_fName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Users_Fld_fName] = Convert.ToString(dr[clsPOSDBConstants.Users_Fld_fName].ToString());

                if (dr[clsPOSDBConstants.Users_Fld_lName] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_lName] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Users_Fld_lName] = Convert.ToString(dr[clsPOSDBConstants.Users_Fld_lName].ToString());

                if (dr[clsPOSDBConstants.Users_Fld_Password] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_Password] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Users_Fld_Password] = Convert.ToString(dr[clsPOSDBConstants.Users_Fld_Password].ToString());

                if (dr[clsPOSDBConstants.Users_Fld_IsActive] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_IsActive] = false;
                else
                    row[clsPOSDBConstants.Users_Fld_IsActive] = Convert.ToBoolean(dr[clsPOSDBConstants.Users_Fld_IsActive].ToString());

                if (dr[clsPOSDBConstants.Users_Fld_DrawNo] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_DrawNo] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Users_Fld_DrawNo] = Convert.ToInt32(dr[clsPOSDBConstants.Users_Fld_DrawNo].ToString());

                if (dr[clsPOSDBConstants.Users_Fld_UserImage] == DBNull.Value)
                    row[clsPOSDBConstants.Users_Fld_UserImage] = DBNull.Value;
                else
                    row[clsPOSDBConstants.Users_Fld_UserImage] = (byte[])dr[clsPOSDBConstants.Users_Fld_UserImage];

                this.AddRow(row);
            }
            }
            catch (Exception Ex)
            {

                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #endregion //Add and Get Methods

        protected override DataTable CreateInstance()
        {
            return new UserTable();
        }

        internal void InitVars()
        {
            try
            {

                this.colId = this.Columns[clsPOSDBConstants.Users_Fld_ID];
                this.colUserID = this.Columns[clsPOSDBConstants.Users_Fld_UserID];
                this.colFirstName = this.Columns[clsPOSDBConstants.Users_Fld_fName];
                this.colLastName = this.Columns[clsPOSDBConstants.Users_Fld_lName];
                this.colPassword = this.Columns[clsPOSDBConstants.Users_Fld_Password];
                this.colIsActive = this.Columns[clsPOSDBConstants.Users_Fld_IsActive];
                this.colDrawNo = this.Columns[clsPOSDBConstants.Users_Fld_DrawNo];
                this.colMaxDiscountLimit = this.Columns[clsPOSDBConstants.Users_Fld_MaxDiscountLimit];
                this.colMaxTransactionLimit = this.Columns[clsPOSDBConstants.Users_Fld_MaxTransactionLimit];//Added By Ravindra(QuicSolv)
                this.colUserImage = this.Columns[clsPOSDBConstants.Users_Fld_UserImage];//Added By Ravindra(QuicSolv)

            }
            catch (Exception exp)
            {
                throw (exp);
            }
        }

        private void InitClass()
        {
            this.colId = new DataColumn(clsPOSDBConstants.Users_Fld_ID, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colId);
            this.colId.AllowDBNull = false;

            this.colUserID = new DataColumn(clsPOSDBConstants.Users_Fld_UserID, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserID);
            this.colUserID.AllowDBNull = true;

            this.colFirstName = new DataColumn(clsPOSDBConstants.Users_Fld_fName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colFirstName);
            this.colFirstName.AllowDBNull = true;

            this.colLastName = new DataColumn(clsPOSDBConstants.Users_Fld_lName, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colLastName);
            this.colLastName.AllowDBNull = true;

            this.colPassword = new DataColumn(clsPOSDBConstants.Users_Fld_Password, typeof(System.String), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colPassword);
            this.colPassword.AllowDBNull = true;

            this.colIsActive = new DataColumn(clsPOSDBConstants.Users_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = true;

            this.colDrawNo = new DataColumn(clsPOSDBConstants.Users_Fld_DrawNo, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colDrawNo);
            this.colDrawNo.AllowDBNull = true;

            this.colMaxDiscountLimit = new DataColumn(clsPOSDBConstants.Users_Fld_MaxDiscountLimit, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMaxDiscountLimit);
            this.colMaxDiscountLimit.AllowDBNull = true;

            //Following Code Added BY Ravindra(QuicSolv)
            this.colMaxTransactionLimit = new DataColumn(clsPOSDBConstants.Users_Fld_MaxTransactionLimit, typeof(System.Decimal), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMaxTransactionLimit);
            this.colMaxDiscountLimit.AllowDBNull = true;
            //Till Here Added By Ravindra
           
            //Following Code Added BY Ravindra(QuicSolv)
            this.colUserImage = new DataColumn(clsPOSDBConstants.Users_Fld_UserImage, typeof(System.Byte[]), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colUserImage);
            this.colUserImage.AllowDBNull = true;
            //Till Here Added By Ravindra

            this.PrimaryKey = new DataColumn[] { this.UserID };
        }

        public UserRow NewUserRow()
        {
            return (UserRow)this.NewRow();
        }

        public override DataTable Clone()
        {
            UserTable cln = (UserTable)base.Clone();
            cln.InitVars();
            return cln;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new UserRow(builder);
        }

        protected override System.Type GetRowType()
        {
            return typeof(UserRow);
        }
    }
}
