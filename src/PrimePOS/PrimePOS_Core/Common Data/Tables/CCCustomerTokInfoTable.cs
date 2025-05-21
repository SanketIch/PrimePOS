using System;
using System.Data;
using System.Collections;
using System.Linq;
using System.Text;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData.Tables
{
    public class CCCustomerTokInfoTable : DataTable, IEnumerable
    {
        #region "Private DataColumn"

        private DataColumn colEntryID;
        private DataColumn colCustomerID;
        private DataColumn colCardType;
        private DataColumn colLast4;
        private DataColumn colProfiledID;
        private DataColumn colProcessor;
        private DataColumn colEntryType;
        private DataColumn colTokenDate;   //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
        private DataColumn colExpDate;  //PRIMEPOS-2612 04-Dec-2018 JY Added
        private DataColumn colCardAlias;    //PRIMEPOS-2634 30-Jan-2019 JY Added
        private DataColumn colPreferenceId; //PRIMEPOS-2635 31-Jan-2019 JY Added

        private DataColumn colIsFsaCard; //PRIMEPOS-2990
        #endregion


        #region Constants
        private const String _TableName = "CCCustomerTokinfo";
        #endregion


        #region "Constructors"

        internal CCCustomerTokInfoTable() : base(_TableName) { this.InitClass(); }

        internal CCCustomerTokInfoTable(DataTable table) : base(table.TableName) { }

        #endregion

        #region Properties

        public int Count
        {
            get
            {
                return this.Rows.Count;
            }
        }

        public CCCustomerTokInfoRow this[int index]
        {
            get
            {
                return ((CCCustomerTokInfoRow)(this.Rows[index]));
            }
        }
        


        public DataColumn EntryID
        {
            get
            {
                return this.colEntryID;
            }
        }

        public DataColumn CustomerID
        {
            get
            {
                return this.colCustomerID;
            }
        }

        public DataColumn CardType
        {
            get
            {
                return this.colCardType;
            }
        }

        public DataColumn Last4
        {
            get
            {
                return this.colLast4;
            }
        }

        public DataColumn ProfiledID
        {
            get
            {
                return this.colProfiledID;
            }
        }

        public DataColumn Processor
        {
            get
            {
                return this.colProcessor;
            }
        }

        public DataColumn EntryType
        {
            get
            {
                return this.colEntryType;
            }
        }

        //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
        public DataColumn TokenDate
        {
            get
            {
                return this.colTokenDate;
            }
        }

        //PRIMEPOS-2612 04-Dec-2018 JY Added
        public DataColumn ExpDate
        {
            get
            {
                return this.colExpDate;
            }
        }

        //PRIMEPOS-2634 30-Jan-2019 JY Added
        public DataColumn CardAlias
        {
            get
            {
                return this.colCardAlias;
            }
        }

        //PRIMEPOS-2635 31-Jan-2019 JY Added
        public DataColumn PreferenceId
        {
            get
            {
                return this.colPreferenceId;
            }
        }

        public DataColumn IsFsaCard //2990
        {
            get
            {
                return this.colIsFsaCard;
            }
        }
        #endregion


        #region "Add and Get Methods"
        public void AddRow(CCCustomerTokInfoRow row)
        {
            AddRow(row, false);
        }

        public void AddRow(CCCustomerTokInfoRow row,bool preserveChanges)
        {
            if (this.GetRowByID(row.EntryID) == null)
            {
                this.Rows.Add(row);
                if (!preserveChanges)
                {
                    row.AcceptChanges();
                }
            }
        }

        //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added TokenDate
        public CCCustomerTokInfoRow AddRow(Int32 iEntryID,
                                            Int32 iCustomerID,
                                            String sCardType,
                                            String sLast4,
                                            String sProfiledID,
                                            String sProcessor,
                                            String sEntryType,
                                            DateTime TokenDate,
                                            DateTime ExpDate,
                                            String CardAlias,
                                            int PreferenceId,
                                            bool IsFsaCard)//2990
        {
            CCCustomerTokInfoRow row = (CCCustomerTokInfoRow)this.NewRow();

            row.EntryID = iEntryID;
            row.CustomerID = iCustomerID;
            row.CardType = sCardType;
            row.Last4 = sLast4;
            row.ProfiledID = sProfiledID;
            row.Processor = sProcessor;
            row.EntryType = sEntryType;
            row.TokenDate = TokenDate;
            row.ExpDate = ExpDate;
            row.CardAlias = CardAlias;
            row.PreferenceId = PreferenceId;
            row.IsFsaCard = IsFsaCard;//2990
            this.Rows.Add(row);
            return row;
        }

        public void MergeTable(DataTable dt)
        {

            CCCustomerTokInfoRow row;
            foreach(DataRow dr in dt.Rows)
            {
                string strField = string.Empty;

                row = (CCCustomerTokInfoRow)this.NewRow();

                //EntryID
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString()=="0")?"0": dr[strField]);

                //CustomerID
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "0") ? "0" : dr[strField]);

                //Card Type
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                //Last4
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                //ProfiledID
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                //Processor
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                //Entry Type
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
                if (dr[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate] == DBNull.Value)
                    row[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate] = DBNull.Value;
                else
                    if (dr[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate].ToString().Trim() == "")
                    row[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate] = Convert.ToDateTime(dr[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate].ToString());

                if (dr[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate] == DBNull.Value)
                    row[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate] = DBNull.Value;
                else if (dr[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate].ToString().Trim() == "")
                    row[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate] = Convert.ToDateTime(System.DateTime.MinValue.ToString());
                else
                    row[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate] = Convert.ToDateTime(dr[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate].ToString());

                //PRIMEPOS-2634 30-Jan-2019 JY Added for Card Alias
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_CardAlias;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = dr[strField].ToString();

                //PRIMEPOS-2635 31-Jan-2019 JY Added
                strField = clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId;
                if (dr[strField] == DBNull.Value)
                    row[strField] = DBNull.Value;
                else
                    row[strField] = Convert.ToInt32((dr[strField].ToString() == "0") ? "0" : dr[strField]);

                if (dr[clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard] == DBNull.Value)//2990
                    row[clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard] = false;
                else
                    row[clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard] = dr[clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard].ToString();

                this.AddRow(row);
            }

        }


        public CCCustomerTokInfoRow GetRowByID(Int64 iID)
        {
            return (CCCustomerTokInfoRow)this.Rows.Find(new object[] { iID });
        }



        #endregion

        public override DataTable Clone()
        {
            CCCustomerTokInfoTable cln = (CCCustomerTokInfoTable)base.Clone();

            return cln;
        }

        protected override DataTable CreateInstance()
        {
            return new CCCustomerTokInfoTable();
        }
        
        internal void InitVars()
        {
            this.colEntryID = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryID];
            this.colCustomerID = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID];
            this.colCardType = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType];
            this.colLast4= this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4];
            this.colProfiledID = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID];
            this.colProcessor = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor];
            this.colEntryType= this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType];
            this.colTokenDate = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate];   //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
            this.colExpDate = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate];
            this.colCardAlias = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_CardAlias]; //PRIMEPOS-2634 30-Jan-2019 JY Added
            this.colPreferenceId = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId]; //PRIMEPOS-2635 31-Jan-2019 JY Added
            this.colIsFsaCard = this.Columns[clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard];//2990
        }


        private void InitClass()
        {
            this.colEntryID = new DataColumn("EntryID", typeof(System.Int32), null, MappingType.Element);
            this.Columns.Add(this.colEntryID);
            this.colEntryID.AllowDBNull = true;

            this.colCustomerID = new DataColumn("CustomerID", typeof(System.Int32), null, MappingType.Element);
            this.Columns.Add(this.colCustomerID);
            this.colCustomerID.AllowDBNull = true;

            this.colCardType = new DataColumn("CardType", typeof(System.String), null, MappingType.Element);
            this.Columns.Add(this.colCardType);
            this.colCardType.AllowDBNull = true;

            this.colLast4 = new DataColumn("Last4", typeof(System.String), null, MappingType.Element);
            this.Columns.Add(this.colLast4);
            this.colLast4.AllowDBNull = true;

            this.colProfiledID = new DataColumn("ProfiledID", typeof(System.String), null, MappingType.Element);
            this.Columns.Add(this.colProfiledID);
            this.colProfiledID.AllowDBNull = true;

            this.colProcessor = new DataColumn("Processor", typeof(System.String), null, MappingType.Element);
            this.Columns.Add(this.colProcessor);
            this.colProcessor.AllowDBNull = true;

            this.colEntryType = new DataColumn("EntryType", typeof(System.String), null, MappingType.Element);
            this.Columns.Add(this.colEntryType);
            this.colEntryType.AllowDBNull = true;

            //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
            this.colTokenDate = new DataColumn("TokenDate", typeof(System.DateTime), null, MappingType.Element);
            this.Columns.Add(this.colTokenDate);
            this.colTokenDate.AllowDBNull = true;

            this.colExpDate = new DataColumn("ExpDate", typeof(System.DateTime), null, MappingType.Element);
            this.Columns.Add(this.colExpDate);
            this.colExpDate.AllowDBNull = true;

            //PRIMEPOS-2634 30-Jan-2019 JY Added
            this.colCardAlias = new DataColumn("CardAlias", typeof(System.String), null, MappingType.Element);
            this.Columns.Add(this.colCardAlias);
            this.colCardAlias.AllowDBNull = true;

            //PRIMEPOS-2635 31-Jan-2019 JY Added
            this.colPreferenceId = new DataColumn("PreferenceId", typeof(System.Int32), null, MappingType.Element);
            this.Columns.Add(this.colPreferenceId);
            this.colPreferenceId.AllowDBNull = true;

            //2990
            this.colIsFsaCard = new DataColumn("IsFsaCard", typeof(System.Boolean), null, MappingType.Element);
            this.Columns.Add(this.colIsFsaCard);
            this.colIsFsaCard.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colEntryID };
        }

        public CCCustomerTokInfoRow NewCCCustomerTokInfoRow()
        {
            return (CCCustomerTokInfoRow)this.NewRow();
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new CCCustomerTokInfoRow(builder);
        }


        protected override Type GetRowType()
        {
            return typeof(CCCustomerTokInfoRow);
        }


        #region "Ienumerable Methods"
        public IEnumerator GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        #endregion
    }
}
 