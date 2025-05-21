using POS_Core.CommonData;
using POS_Core.Resources;
//using POS.Resources;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_Core.ErrorLogging;
using NLog; //PRIMEPOS-2764 10-Dec-2019 JY Added

namespace POS_Core.BusinessRules
{
    public class POSTransPayment_CCLog
    {
        int iID;
        string sPayTypeCode;
        string sRefNo;
        decimal dAmount;
        string sAuthNo;
        string sHC_Posted;
        string sCCName;
        string sCCTransNO;
        DateTime dtTransDate;
        string sCustomerSign;
        byte[] sBinarySign;
        string sSigType;
        string sStationID;
        int iTransType;
        string sUserID;
        string sTransID;
        bool bIsIIASTrans;
        //Added By SRT(Ritesh Parekh) Date : 23-07-2009
        string paymentProcessor;
        //End Of Added By SRT(Gaurav)
        PccCardInfo oPccCardInfo;
        POSTransPayment_CCLog_Status iStatus;
        string strIsManual;  //Sprint-19 - 2139 06-Jan-2015 JY Added

        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public POSTransPayment_CCLog()
        {
            iID = 0;
            sPayTypeCode = "";
            sRefNo = "";
            dAmount = 0;
            sAuthNo = "";
            sHC_Posted = "";
            sCCName = "";
            sCCTransNO = "";
            dtTransDate = DateTime.Now;
            sCustomerSign = "";
            sSigType = "";
            sStationID = Configuration.StationID;
            iTransType = 0;
            sUserID = Configuration.UserName;
            iStatus = 0;
            sTransID = "";
            bIsIIASTrans = false;
            oPccCardInfo = null;
            //Added By SRT(Ritesh Parekh) Date : 23-07-2009
            paymentProcessor = clsPOSDBConstants.NOPROCESSOR;
            //End Of Added By SRT(Gaurav)
            strIsManual = "";    //Sprint-19 - 2139 06-Jan-2015 JY Added
        }

        public int ID
        {
            get { return iID; }
            set { iID = value; }
        }

        public string PayTypeCode
        {
            get { return sPayTypeCode; }
            set { sPayTypeCode = value; }
        }

        public string RefNo
        {
            get { return sRefNo; }
            set { sRefNo = value; }
        }

        public decimal Amount
        {
            get { return dAmount; }
            set { dAmount = value; }
        }

        public string AuthNo
        {
            get { return sAuthNo; }
            set { sAuthNo = value; }
        }

        public string HC_Posted
        {
            get { return sHC_Posted; }
            set { sHC_Posted = value; }
        }

        public string CCName
        {
            get { return sCCName; }
            set { sCCName = value; }
        }

        public string CustomerSign
        {
            get { return sCustomerSign; }
            set { sCustomerSign = value; }
        }

        public byte[] BinarySign
        {
            get { return sBinarySign; }
            set { sBinarySign = value; }
        }

        public string SigType
        {
            get { return sSigType; }
            set { sSigType = value; }
        }

        public int TransType
        {
            get { return iTransType; }
            set { iTransType = value; }
        }

        public POSTransPayment_CCLog_Status Status
        {
            get { return iStatus; }
            set { iStatus = value; }
        }

        public string TransID
        {
            get { return sTransID; }
            set { sTransID = value; }
        }

        public bool IsIIASTrans
        {
            get { return bIsIIASTrans; }
            set { bIsIIASTrans = value; }
        }

        public PccCardInfo PccCardInfo
        {
            set { oPccCardInfo = value; }
            get { return oPccCardInfo; }
        }

        //Added By SRT(Ritesh Parekh) Date : 23-07-2009
        public string PaymentProcessor
        {
            set
            {
                if (value != null && value.ToString().Trim().Length > 0)
                {
                    paymentProcessor = value;
                }
                else
                {
                    paymentProcessor = clsPOSDBConstants.NOPROCESSOR;
                }
            }
            get
            {
                if (paymentProcessor != null && paymentProcessor.ToString().Trim().Length > 0)
                {
                    return paymentProcessor;
                }
                else
                {
                    return clsPOSDBConstants.NOPROCESSOR;
                }
            }
        }

        //End Of Added By SRT(Gaurav)

        #region Sprint-19 - 2139 06-Jan-2015 JY Added
        public string IsManual
        {
            get { return strIsManual; }
            set { strIsManual = value; }
        }
        #endregion

        public void Add()
        {
            try
            {
                logger.Trace("Add() - " + clsPOSDBConstants.Log_Entering);
                IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(18);    //Sprint-19 - 2139 06-Jan-2015 JY changed 17 to 18

                sqlParams[0] = DataFactory.CreateParameter("PayTypeCode", System.Data.DbType.String);
                sqlParams[0].Value = sPayTypeCode;

                sqlParams[1] = DataFactory.CreateParameter("RefNo", System.Data.DbType.String);
                sqlParams[1].Value = sRefNo;

                sqlParams[2] = DataFactory.CreateParameter("Amount", System.Data.DbType.Decimal);
                sqlParams[2].Value = dAmount;

                sqlParams[3] = DataFactory.CreateParameter("AuthNo", System.Data.DbType.String);
                sqlParams[3].Value = sAuthNo;

                sqlParams[4] = DataFactory.CreateParameter("HC_Posted", System.Data.DbType.String);
                sqlParams[4].Value = sHC_Posted;

                sqlParams[5] = DataFactory.CreateParameter("CCName", System.Data.DbType.String);
                sqlParams[5].Value = sCCName;

                sqlParams[6] = DataFactory.CreateParameter("CustomerSign", System.Data.DbType.String);
                sqlParams[6].Value = sCustomerSign;

                sqlParams[15] = DataFactory.CreateParameter("BinarySign", System.Data.DbType.Binary);
                if (sBinarySign == null)
                    sqlParams[15].Value = System.DBNull.Value;
                else
                    sqlParams[15].Value = sBinarySign;

                sqlParams[16] = DataFactory.CreateParameter("SigType", System.Data.DbType.String);
                sqlParams[16].Value = sSigType;

                sqlParams[7] = DataFactory.CreateParameter("TransType", System.Data.DbType.Int32);
                sqlParams[7].Value = iTransType;

                sqlParams[8] = DataFactory.CreateParameter("Status", System.Data.DbType.Int32);
                sqlParams[8].Value = (int)iStatus;

                sqlParams[9] = DataFactory.CreateParameter("StationID", System.Data.DbType.String);
                sqlParams[9].Value = sStationID;

                sqlParams[10] = DataFactory.CreateParameter("UserID", System.Data.DbType.String);
                sqlParams[10].Value = sUserID;

                sqlParams[11] = DataFactory.CreateParameter("TransDate", System.Data.DbType.DateTime);
                sqlParams[11].Value = DateTime.Now;

                sqlParams[12] = DataFactory.CreateParameter("TransID", System.Data.DbType.String);
                sqlParams[12].Value = sTransID;

                sqlParams[13] = DataFactory.CreateParameter("IsIIASTrans", System.Data.DbType.Boolean);
                sqlParams[13].Value = bIsIIASTrans;

                sqlParams[14] = DataFactory.CreateParameter("PaymentProcessor", System.Data.DbType.String);
                sqlParams[14].Value = paymentProcessor;

                #region Sprint-19 - 2139 06-Jan-2015 JY Added
                sqlParams[17] = DataFactory.CreateParameter("IsManual", System.Data.DbType.String);
                sqlParams[17].Value = strIsManual;
                #endregion

                string sSQL = BuildInsertSQL(sqlParams);
                //DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL, sqlParams);
                
                //sSQL += ";SELECT @@IDENTITY;";    //PRIMEPOS-2764 22-Nov-2019 JY Commented
                sSQL += ";select SCOPE_IDENTITY();";    //PRIMEPOS-2764 22-Nov-2019 JY Added

                try
                {
                    object objID = DataHelper.ExecuteScalar(Configuration.ConnectionString, CommandType.Text, sSQL, sqlParams);
                    iID = Configuration.convertNullToInt(objID.ToString()); //this is trying to convert "NULL" to "ToString"
                    //iID = Configuration.convertNullToInt(objID);    //PRIMEPOS-2764 22-Nov-2019 JY modified to avoid null reference issue //tempararily commented to reproduce and catch the issue into errorlog table
                }
                catch (Exception Exp)
                {
                    iID = GetPOSTransPayment_CCLogId();    //PRIMEPOS-2764 10-Dec-2019 JY Added logic to fetch "Id" from POSTransPayment_CCLog table
                    logger.Fatal(Exp, "Add() - Inner Catch");
                    //ErrorHandler.logException(Exp, "", "");
                }
                logger.Trace("Add() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Add() - Outer Catch");
                //ErrorHandler.logException(Ex, "", "");
            }
        }

        #region PRIMEPOS-2764 10-Dec-2019 JY Added
        private int GetPOSTransPayment_CCLogId()
        {
            int nId = 0;
            try
            {
                logger.Trace("GetPOSTransPayment_CCLogId() - " + clsPOSDBConstants.Log_Entering);
                string strSQL = string.Empty;
                if (Configuration.convertNullToString(sTransID).Trim() != "")
                {
                    strSQL = "SELECT ID FROM POSTransPayment_CCLog WHERE LTRIM(RTRIM(TransID)) = '" + Configuration.convertNullToString(sTransID).Trim().Replace("'", "''") + "'";
                }
                else
                {
                    strSQL = "SELECT ID FROM POSTransPayment_CCLog WHERE LTRIM(RTRIM(RefNo)) = '" + Configuration.convertNullToString(sRefNo).Trim() + "' AND Amount = " + Configuration.convertNullToDecimal(dAmount).ToString() + " AND LTRIM(RTRIM(AuthNo)) = '" + Configuration.convertNullToString(sAuthNo).Trim().Replace("'", "''") + "' AND LTRIM(RTRIM(StationID)) = '" + Configuration.convertNullToString(sStationID).Trim() + "'";
                }

                DataTable dt = DataHelper.ExecuteDataTable(DataFactory.CreateConnection(Configuration.ConnectionString), CommandType.Text, strSQL);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        throw (new Exception("We couldn't return Id as no matching record found in POSTransPayment_CCLog table"));
                    }
                    if (dt.Rows.Count > 1)
                    {
                        throw (new Exception("We couldn't return Id as found duplicates in POSTransPayment_CCLog table"));
                    }
                    else
                    {
                        nId = Configuration.convertNullToInt(dt.Rows[0][0]);
                    }
                    logger.Trace("GetPOSTransPayment_CCLogId() - " + clsPOSDBConstants.Log_Exiting);

                }
                else
                {
                    throw (new Exception("Returned null for DataTable, contact system administrator"));
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetPOSTransPayment_CCLogId()");
                //ErrorHandler.logException(Ex, "", "");
                nId = 0;
            }
            return nId;
        }
        #endregion
        
        public void UpdateStatus(POSTransPayment_CCLog_Status enumStatus)
        {
            if (iID > 0)
            {
                string sSQL = " Update POSTransPayment_CCLog set Status=" + ((int)enumStatus).ToString() + " where ID=" + iID.ToString();
                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                this.iStatus = enumStatus;
            }
        }

        public void Delete()
        {
            if (iID > 0)
            {
                string sSQL = " Delete from POSTransPayment_CCLog  where ID=" + iID.ToString();
                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
            }
        }

        private string BuildInsertSQL(IDbDataParameter[] delParam)
        {
            string sInsertSQL = "INSERT INTO POSTransPayment_CCLog ( ";
            // build where clause
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                //sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
                if (delParam[i].SourceColumn.ToString() == "BinarySign")
                {
                    sInsertSQL = sInsertSQL + " , cast(" + delParam[i].ParameterName + " as varbinary(MAX))";
                }
                else
                {
                    sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
                }
            }
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }
    }


    public class POSTransPayment_CCLogList : System.Collections.ObjectModel.Collection<POSTransPayment_CCLog>
    {
        protected override void RemoveItem(int index)
        {
            this[index].Delete();
            base.RemoveItem(index);
        }

        public void RemoveAll()
        {
            foreach (POSTransPayment_CCLog oCCLog in this.Items)
            {
                oCCLog.Delete();
                //base.Remove(oCCLog);
            }
        }
    }


    public enum POSTransPayment_CCLog_Status
    {
        Initiated = 0,
        Approved = 1,
        RemoveInitiated = 2
    }


}
