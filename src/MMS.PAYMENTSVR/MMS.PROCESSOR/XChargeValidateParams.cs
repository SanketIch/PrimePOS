using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using NLog;

namespace MMS.PROCESSOR
{
    public class XChargeValidateParams
    {
        ILogger logger = LogManager.GetCurrentClassLogger(); 
        DataTable xCreditSale; 
        private static XChargeValidateParams objDef;
        public XChargeValidateParams()
        {
            logger.Trace("In XChargeValidateParams()");
            xCreditSale = new DataTable("CreditSale");
            FillCreditSale();
        }
        public static XChargeValidateParams DefaultInstance
        {
            get
            {
                if (objDef == null)
                {
                    objDef = new XChargeValidateParams();
                }
                return objDef;
            }
        }

        private void FillCreditSale()
        {
            logger.Trace("In FillCreditSale()");
            DataColumn colFieldName = new DataColumn("FieldName", Type.GetType("System.String" , true, true));
            DataColumn colDataType = new DataColumn("DataType", Type.GetType("System.String", true, true));
            DataColumn colMinLength = new DataColumn("MinLength", Type.GetType("System.Int32", true, true));
            DataColumn colMaxLength = new DataColumn("MaxLength", Type.GetType("System.Int32", true, true));
            DataColumn colRequired = new DataColumn("Required", Type.GetType("System.String", true, true));
            xCreditSale.Columns.Add(colFieldName);
            xCreditSale.Columns.Add(colDataType);
            xCreditSale.Columns.Add(colMinLength);
            xCreditSale.Columns.Add(colMaxLength);
            xCreditSale.Columns.Add(colRequired);

            DataRow row =xCreditSale.NewRow();            
            row[0] = "TRANSACTIONTYPE";
            row[1] = "string";
            row[2] = DBNull.Value ;
            row[3] = DBNull.Value;
            row[4] = "Y";
            xCreditSale.Rows.Add(row);

            row = xCreditSale.NewRow(); 
            row[0] = "CLERK";
            row[1] = "string";
            row[2] = 0;
            row[3] = 20;
            row[4] = "N";
            xCreditSale.Rows.Add(row);

            row = xCreditSale.NewRow();

            row[0] = "RECEIPT";
            row[1] = "string";
            row[2] = 0;
            row[3] = 18;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();


            row[0] = "ACCOUNT";
            row[1] = "string";
            row[2] = 6;
            row[3] = 19;
            row[4] = "Y";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "EXPIRATION";
            row[1] = "int";
            row[2] = 4;
            row[3] = 4;
            row[4] = "Y";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "TRACK";
            row[1] = "string";
            row[2] = 1;
            row[3] = 200 ;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "AMOUNT";
            row[1] = "double";
            row[2] = 1;
            row[3] = 13;
            row[4] = "Y";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "ZIP";
            row[1] = "int";
            row[2] = 5;
            row[3] = 9;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "ADDRESS";
            row[1] = "string";
            row[2] = 0;
            row[3] = 20;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "CV";
            row[1] = "int";
            row[2] = 0;
            row[3] = 4;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "MERCHANTID";
            row[1] = "string";
            row[2] = 0;
            row[3] = DBNull.Value ;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "MARKETTYPE";
            row[1] = "string";
            row[2] = 0;
            row[3] = DBNull.Value ;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "REFERENCE";
            row[1] = "string";
            row[2] = 0;
            row[3] = DBNull.Value ;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "TOTALAMOUNT";
            row[1] = "double";
            row[2] = 1;
            row[3] = 13;
            row[4] = "Y";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "DEBITPIN";
            row[1] = "string";
            row[2] = 16;
            row[3] = 16;
            row[4] = "Y";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "DEBITKEY";
            row[1] = "string";
            row[2] = 16;
            row[3] = 20;
            row[4] = "Y";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

            row[0] = "CASHBACKAMOUNT";
            row[1] = "Int64";
            row[2] = 1;
            row[3] = 13;
            row[4] = "N";
            xCreditSale.Rows.Add(row);
            row = xCreditSale.NewRow();

        }
        public bool CheckXcardParams(MMSDictionary<string,string> dict,out string sError)
        {
            bool isValid = true;
            string tempError = string.Empty;
            DataRow []drow=null;
            foreach (KeyValuePair<string, string> kvp in dict)
            {
               drow=xCreditSale.Select("FieldName='"+kvp.Key+"'");                 
               if (drow[0][4].ToString() == "Y" && drow[0][3].ToString()!= string.Empty)
               {
                   int mLen = kvp.Value.Length;
                   int minLen = Convert.ToInt32(drow[0][2].ToString());
                   int maxLen = Convert.ToInt32(drow[0][3].ToString());
                   if (mLen < minLen || mLen > maxLen)
                   {
                                isValid = false;
                                tempError = "Invalid Value Length";
                                break;
                   }
                            
                    string expirationError = string.Empty;
                    //if (kvp.Key == "EXPIRATION" )//&& CheckCardExpiration(kvp.Value, out expirationError) == false)
                    //{
                    //    isValid = false;
                    //    tempError = expirationError;
                    //    break;
                    //}

                    if (kvp.Key == "AMOUNT" || kvp.Key == "TOTALAMOUNT")
                    {
                        
                        if (CheckCardAmount(kvp.Value, out expirationError) == false)
                        {
                            isValid = false;
                            tempError = expirationError;
                            break;
                        }
                        
                    }

                    if (kvp.Key == "ACCOUNT")
                    {
                        if (CheckAccountNum(kvp.Value, out expirationError) == false)
                        {
                            isValid = false;
                            tempError = expirationError;
                            break;
                        }                                
                    }

                    if (kvp.Key == "DEBITPIN" || kvp.Key == "DEBITKEY")
                    {
                        if (CheckDebitPIN_KEY(kvp.Value, out expirationError) == false)
                        {
                            isValid = false;
                            tempError = expirationError;
                            break;
                        }
                    }                  

                }
                else
                {
                    isValid = true;
                }  
            }
            sError = tempError;
            return isValid;
        }
        private bool CheckCardExpiration(string strYYMMOfExpiration,out string strCheckError)
        {
            bool isValid = false;
            bool isConverted = false;
            Int32 newYYMM = 0;
            string tempCheckError = string.Empty;
            isConverted = Int32.TryParse(strYYMMOfExpiration, out newYYMM);
            if (isConverted == true)
            {

                if (Convert.ToInt32(newYYMM.ToString().Substring(2, 2)) < 0 || Convert.ToInt32(newYYMM.ToString().Substring(2, 2)) > 12)
                {
                   tempCheckError = "Invalid Expiration Month";
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }

            }
            else
            {
                isValid = false;
                tempCheckError = "Invalid Expiration Year & Month";
            }
            strCheckError = tempCheckError;
            return isValid;
        }
        private bool CheckCardAmount(string cardAmount, out string sAmountError)
        {
            bool isValid = false;
            bool isConverted = false;
            string tempAmountError = string.Empty;
            double  cardAmountValue = 0D;
            isConverted = Double.TryParse(cardAmount,out cardAmountValue);
            if (isConverted == true)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
                tempAmountError = "Invalid Characters In AMOUNT/TOTALAMOUNT";
            }

            sAmountError = tempAmountError;
            return isValid;           
        }
        private bool CheckAccountNum(string strAccountNum, out string sActNumErr)
        {
            bool isValid = false;           
            bool isConverted = true;
            int newNum = 0;
            int foundIndex = -1;
            
            string numError = string.Empty;
            for (int numIndex = 0; numIndex < strAccountNum.Trim().Length; numIndex++)
            {
                isConverted = Int32.TryParse(strAccountNum.Substring(numIndex, 1), out newNum);
                if (isConverted == false)
                {
                    isValid = false;
                    numError = "Invalid Number In Card Number";
                    break;
                }
                else
                {
                    numError = "";
                    isValid = true;
                }
            }

            sActNumErr = numError;
            return isValid;
        }
        private bool CheckDebitPIN_KEY(string strPinOrKeyNo, out string sPinOrKeyNumErr)
        {
            bool isValid = false;
            bool isConverted = true;
            Int64 newNum = 0;            
            string numError = string.Empty;
            int[] tempNumArray = new int [36];
            int numIndex =48;
            //Filling ASCII values for Alphabates & Numbers in an array
            for (int mainIndex = 0; mainIndex < 36; mainIndex++)
            {

                if (mainIndex == 10)
                {
                    numIndex = numIndex + 7;
                    tempNumArray[mainIndex] = numIndex;
                }
                else
                {
                    tempNumArray[mainIndex] = numIndex;
                }
                numIndex+= 1;

            }     
           

            //There's a change in logic since pin pad is set to demo mode
            foreach (char chr in strPinOrKeyNo.ToUpper())
            {
                int tmpval = (int)chr;
                bool isSearch = false;
                for (int xIndex = 0; xIndex < tempNumArray.Length; xIndex++)
                {
                    
                    if (tmpval!= tempNumArray[xIndex])
                    {
                        isSearch = false;
                    }
                    else
                    {
                        isSearch = true;                        
                        numError = "";
                        isValid = true;
                        break;
                    }
                }
                if(isValid ==false)
                {
                        numError = "Invalid Number In PIN Or Key Serial Number";
                       break;
                }
            }
            //If it's fixed that Pin Number Or Key Serial No. will be puerly numeric,
            // then comment/Remove the above for & foreach block, and uncomment the lines from next to this statement.
            //    isConverted = Int64.TryParse(strPinOrKeyNo, out newNum);
            //    if (isConverted == false)
            //    {
            //        isValid = false;
            //        numError = "Invalid Number In PIN Or Key Serial Number";                   
            //    }
            //    else
            //    {
            //        numError = "";
            //        isValid = true;
            //    }
           
            sPinOrKeyNumErr = numError;
            return isValid;
        }

    }
}
