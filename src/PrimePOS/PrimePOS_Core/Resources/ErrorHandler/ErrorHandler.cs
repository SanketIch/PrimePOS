// ----------------------------------------------------------------
// Library: Error Handler
// Author: Adeel Shehzad.
// Company: D-P-S. (www.d-p-s.com)
//
// ----------------------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using POS_Core.Resources;
using POS_Core.CommonData;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Resources;
//using Resources;

namespace POS_Core.ErrorLogging
{
	public class ErrorHandler
	{
        public static void logException(Exception ex, string optitem1, string optitem2)
        {
            try
            {
                SaveExceptionToDataBase(ex, optitem1, optitem2);
                //SaveExceptionToDataBase(ex, optitem1, optitem2);  //Sprint-22 04-Nov-2015 JY Commented the duplicate
            }
            catch(Exception ex1)
            {
            }
        }

		public static void throwException(Exception ex, string optitem1 , string optitem2 )
		{
			SaveExceptionToDataBase(ex,optitem1,optitem2);
			throw(new OtherExceptions(ex));
		}

		public static void throwCustomError(POS_Core.CommonData.POSErrorENUM  errNumber)
		{
			string errMessage = "";
			errMessage = ErrorHandler.getCustomMessageFromDb((long)errNumber);

			if (errMessage.Trim() != "")
			{
				throw (new POSExceptions(errMessage, (long) errNumber));
			}
			else
			{
				errMessage = ErrorHandler.getCustomMessageFromXML((long) errNumber);
				if (errMessage.Trim() != "")
				{
					throw (new POSExceptions(errMessage, (long) errNumber));
				}
				else if (errNumber == POSErrorENUM.TaxCode_DuplicateTaxCode)
				{
					throw (new POSExceptions("The Tax Code already exist, please select another Tax Code.", (long) errNumber));
                }
                else if (errNumber == POSErrorENUM.PSE_Items_DuplicateProductId)
                {
                    throw (new POSExceptions("Item Code already exist, please select another Item Code.", (long)errNumber));
                }
                else if (errNumber == POSErrorENUM.PSE_Items_ProductIdCanNotBeNULL)
                {
                    throw (new POSExceptions("Item Code can not be null.", (long)errNumber));
                }
                else if (errNumber == POSErrorENUM.PSE_Items_ProductNameCanNotBeNULL)
                {
                    throw (new POSExceptions("Item Description can not be null.", (long)errNumber));
                }
                else if (errNumber == POSErrorENUM.PSE_Items_ProductGramsCanNotBeNULL)
                {
                    throw (new POSExceptions("Product grams should be greater than 0.", (long)errNumber));
                }
                else
				{
					throw (new POSExceptions("Custom message not found.", (long) errNumber));
				}
			}
		}
        /// <summary>
        /// Added By Amit Date 19 May 2011
        /// Append time remaining to release lock of login in Message
        /// </summary>
        /// <param name="errNumber"></param>
        public static void throwCustomError(POS_Core.CommonData.POSErrorENUM errNumber,int pMinutes)
        {
            string errMessage = "";
            errMessage = ErrorHandler.getCustomMessageFromDb((long)errNumber);

            if (errMessage.Trim() != "")
            {
                errMessage = errMessage.Replace("30", pMinutes.ToString());
                throw (new POSExceptions(errMessage, (long)errNumber));
            }
            else
            {
                errMessage = ErrorHandler.getCustomMessageFromXML((long)errNumber);
                if (errMessage.Trim() != "")
                {
                    throw (new POSExceptions(errMessage, (long)errNumber));
                }
                else
                {
                    throw (new POSExceptions("Custom message not found.", (long)errNumber));
                }
            }
        }


		private static void SaveExceptionToDataBase(System.Exception ex, string optitem1 , string optitem2 )
		{
			IDbTransaction tx;
			string sSQL = "";
			string ErrType = "";
			string ErrorDateTime = "";
			string ErrorDescription = "";
			string ErrorSource = "";
			string ErrorStackTrace = "";
			string ErrorTargetSite = "";
			string ErrorHelpLink = "";
			string OptionalText1 = "";
			string OptionalText2 = "";

			if (ex.ToString() != "") ErrType = ex.ToString().Replace("'","");
			if (System.DateTime.Now.ToString() != "") ErrorDateTime = System.DateTime.Now.ToString().Replace("'","");
			if (ex.Message != "") ErrorDescription = ex.Message.Replace("'","");
			if (ex.Source != "") ErrorSource = ex.Source.Replace("'","");
            if (ex.StackTrace != "")
            {
                ErrorStackTrace = ex.StackTrace.Replace("'", "");
                if (ErrorStackTrace.Length > 2000)
                {
                    ErrorStackTrace = ErrorStackTrace.Substring(0, 2000);
                }
            }

            if (ErrType != "")
            {
                if (ErrType.Length > 2000)
                {
                    ErrType= ErrType.Substring(0, 2000);
                }
            }

			if (ex.TargetSite.ToString() != "") ErrorTargetSite = ex.TargetSite.ToString().Replace("'","");
			if (ex.HelpLink != null) ErrorHelpLink = ex.HelpLink.Replace("'","");
			if (optitem1 != "") OptionalText1 = optitem1.Replace("'","");
			if (optitem2 != "") OptionalText2 = optitem2.Replace("'","");

			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
			conn.Open();
			tx = conn.BeginTransaction();

			try 
			{
				sSQL = String.Concat( "INSERT INTO " 
										, clsPOSDBConstants.ErrorLog_tbl
											,"( " ,  clsPOSDBConstants.ErrorLog_Fld_ErrorType 
											,  " , " , clsPOSDBConstants.ErrorLog_Fld_ErrorDateTime 
											, " , " , clsPOSDBConstants.ErrorLog_Fld_ErrorDescription
											, " , " , clsPOSDBConstants.ErrorLog_Fld_ErrorSource  
											, " , " , clsPOSDBConstants.ErrorLog_Fld_ErrorStackTrace
											, " , " ,  clsPOSDBConstants.ErrorLog_Fld_ErrorCausedByMethod
											, " , " ,  clsPOSDBConstants.ErrorLog_Fld_HelpLink
											, ", " , clsPOSDBConstants.ErrorLog_Fld_OptionalText1 
											, ", " , clsPOSDBConstants.ErrorLog_Fld_OptionalText2 
									," ) VALUES ( '" , 
												ErrType 
												, "' , '" ,  ErrorDateTime
												, "' , '" , ErrorDescription
												, "' , '" , ErrorSource
												, "' , '" , ErrorStackTrace
												, "' , '" , ErrorTargetSite
												, "' , '" , ErrorHelpLink
												, "' , '" , optitem1
												, "' , '" , optitem2
												, "')");

				DataHelper.ExecuteNonQuery(tx,CommandType.Text,sSQL,null);

				tx.Commit();
			} 
			catch(Exception exp) 
			{
				tx.Rollback();
				throw(exp);
			}
		}


		public static string getCustomMessageFromDb(long ErrNumber)
		{
			IDbCommand cmd = DataFactory.CreateCommand();
			string sSQL = "";
			string sErrorMassage = "";
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
		    conn.Open();

            try 
			{
				sSQL = String.Concat( "SELECT " 
											, clsPOSDBConstants.CustomError_Fld_ErrorMessage 
										," FROM " 
											, clsPOSDBConstants.CustomError_tbl
										," WHERE " 
											, clsPOSDBConstants.CustomError_Fld_ErrorID , " = " ,  ErrNumber.ToString());

				cmd.CommandType = CommandType.Text;
				cmd.CommandText = sSQL;
				cmd.Connection = conn;

				sErrorMassage = Convert.ToString( cmd.ExecuteScalar());
				conn.Close();
				return sErrorMassage;
			} 
			catch(Exception exp) 
			{
				sErrorMassage = exp.Message;
				conn.Close();
				throw(exp);
			}
		}


        private static string getCustomMessageFromXML(long ErrNumber)
        {
            string retValue;

            using (System.IO.TextReader oReader = new System.IO.StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("POS.ErrorHandler.AlertMessages.xml")))
            {

                System.Xml.XmlDataDocument oDoc = new System.Xml.XmlDataDocument();
                oDoc.Load(oReader);

                System.Xml.XmlNodeList xmlnode = oDoc.SelectNodes("//AlertMessages/AlertMessage[@ID=" + ErrNumber.ToString() + "]");
                if (xmlnode.Count > 0)
                {
                    retValue = xmlnode[0].ChildNodes[0].InnerText;
                }
                else
                {
                    retValue = string.Empty;
                }
            }

            return retValue;

        }

        public static void SaveLog(int iEventID, string sUserID, string sResult, string sMessage)
        {
            IDbTransaction tx;
            string UserID = string.Empty;
            string Result = string.Empty;
            string Message = string.Empty;
            string LogDate = string.Empty;
            string sSQL = string.Empty;

            if (System.DateTime.Now.ToString() != "") LogDate = System.DateTime.Now.ToString().Replace("'", "");
            if (sResult != "") Result = sResult.Replace("'", "");
            if (sMessage != "") Message = sMessage.Replace("'", "");
            if (sUserID != "") UserID = sUserID.Replace("'", "");


            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            conn.Open();
            tx = conn.BeginTransaction();

            try
            {
                sSQL = String.Concat("INSERT INTO "
                                        , clsPOSDBConstants.Log_tbl
                                            , "( ", clsPOSDBConstants.Log_Event
                                            , " , ", clsPOSDBConstants.Log_LogDate
                                            , " , ", clsPOSDBConstants.Log_UserID
                                            , " , ", clsPOSDBConstants.Log_LogResult
                                            , " , ", clsPOSDBConstants.Log_LogMessage
                                    , " ) VALUES ( '",
                                                iEventID
                                                , "' , '", LogDate
                                                , "' , '", UserID
                                                , "' , '", Result
                                                , "' , '", Message
                                                , "')");

                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, null);
                tx.Commit();
            }
            catch (Exception exp)
            {
                tx.Rollback();
                throw (exp);
            }
        }

        public static void UpdateUserLoginAttempt(string sUserID, int noOfAttempt, bool isToLock)
        {
            int ilock = isToLock ? 1 : 0;

            if (isToLock)
                noOfAttempt = 0;

            DateTime currTime = DateTime.Now;
            IDbTransaction tx;

            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            conn.Open();
            tx = conn.BeginTransaction();
                           try
                {
                    string sSQL= "Update " + clsPOSDBConstants.Users_tbl + " Set LastLoginAttempt='" + currTime.ToString() + "', NoOfAttempt=" + noOfAttempt + ", IsLocked=" + ilock + " Where UserId ='" + sUserID + "'";
                    
                    DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, null);
                    tx.Commit();
                }
                catch (Exception exp)
                {
                    tx.Rollback();
                    throw (exp);
                }            
        }
	}
}
