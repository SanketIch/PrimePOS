/*
 * Author: Manoj Kumar
 * Description: Get the information from the ID Card
 *              If another ID is needed; Add another class for that ID Type
 * Date: 4/12/2013
 */

using System;
using System.Collections.Generic;
using NLog;

namespace POS_Core.Business_Tier
{
    public class DL
    {
        #region variable

        private string DLInfo;
        Dictionary<string,string> DLDictionary = null;
        # endregion

        #region Properities

        private string _DBA;

        /// <summary>
        /// Get the Driver License Expiration Date
        /// </summary>
        public string DBA //Expiration Date
        {
            get { return _DBA; }
        }

        private string _DCS;

        /// <summary>
        /// Get the Driver License holder Last Name
        /// </summary>
        public string DCS //Last Name
        {
            get { return _DCS; }
        }

        private string _DCT;

        /// <summary>
        /// Get the Driver License holder First Name
        /// </summary>
        public string DCT
        {
            get { return _DCT; }
        }

        private string _DBD;

        /// <summary>
        /// Get the Driver License issue date
        /// </summary>
        public string DBD
        {
            get { return _DBD; }
        }

        private string _DBB;

        /// <summary>
        /// Get the Driver License holder DOB
        /// </summary>
        public string DBB
        {
            get { return _DBB; }
        }

        private string _DBC;

        /// <summary>
        /// Get the Driver License holder SEX
        /// </summary>
        public string DBC
        {
            get { return _DBC; }
        }

        private string _DAG;

        /// <summary>
        /// Get the Driver License holder street Address
        /// </summary>
        public string DAG
        {
            get { return _DAG; }
        }

        private string _DAI;

        /// <summary>
        /// Get the Driver License holder City
        /// </summary>
        public string DAI
        {
            get { return _DAI; }
        }

        private string _DAJ;

        /// <summary>
        /// Get the Driver License holder State
        /// </summary>
        public string DAJ
        {
            get { return _DAJ; }
        }

        private string _DAK;

        /// <summary>
        /// Get the Driver License holder ZIPCODE
        /// </summary>
        public string DAK
        {
            get { return _DAK; }
        }

        private string _DAQ;

        /// <summary>
        /// Get the Driver License Number
        /// </summary>
        public string DAQ
        {
            get { return _DAQ; }
        }

        private string _DCG;

        /// <summary>
        /// Get the Driver License holder Country
        /// </summary>
        public string DCG
        {
            get { return DCG; }
        }

        private bool _isValidated;

        public bool isValidated
        {
            get { return _isValidated; }
        }

        #endregion Properities

        public DL(string DLData)
        {
            this.DLInfo = DLData; //scan data
            if(ProcessDL())
            {
                DLSegments();
            }
        }

        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Segments
        /// <summary>
        /// This function assign the value to each tag(properity)
        /// </summary>
        private void DLSegments()
        {            
            _isValidated = false;
            try
            {
                if(DLDictionary != null && DLDictionary.Count > 1)
                {
                    logger.Trace("DLSegments() - " + DLDictionary.Count);   //PRIMEPOS-3162 18-Nov-2022 JY Added
                    foreach (var element in DLDictionary.Keys)
                    {
                        switch(element)
                        {
                            case "DBA": //Expiration Date
                                this._DBA = DLDictionary[element].Trim();
                                break;
                            case "DCS": //Last Name
                                this._DCS = DLDictionary[element].Trim();
                                break;
                            case "DCT": //First Name
                            case "DAC": // For NJ First name
                                this._DCT = DLDictionary[element].Trim();
                                break;
                            case "DBD": //Issue Date
                                this._DBD = DLDictionary[element].Trim();
                                break;
                            case "DBB": //DOB
                                this._DBB = AgeValidate(DLDictionary[element].Trim());
                                break;
                            case "DBC": //Sex - Male = 1
                                this._DBC = DLDictionary[element].Trim() != "1" ? "Female" : "Male";
                                break;
                            case "DAG": //Street Address
                                this._DAG = DLDictionary[element].Trim();
                                break;
                            case "DAI": //City
                                this._DAI = DLDictionary[element].Trim();
                                break;
                            case "DAJ": //State
                                this._DAJ = DLDictionary[element].Trim();
                                break;
                            case "DAK": //ZipCode
                                this._DAK = DLDictionary[element].Trim();
                                break;
                            case "DAQ": //Lic #
                                this._DAQ = DLDictionary[element].Trim();
                                break;
                            case "DCG": //Country
                                this._DCG = DLDictionary[element].Trim();
                                break;
                        }
                    }
                    _isValidated = true;
                }
            }
            catch(Exception ex)
            {
                logger.Fatal(ex, "DLSegments()");
                throw ex;
            }
            finally
            {
                DLDictionary.Clear();
                DLInfo = string.Empty;
            }
        }

        #endregion Segments

        #region Age Validate

        /// <summary>
        /// Old Driver Lic has the Date of Birth in the Format YYYYMMDD
        /// New ID has it MMDDYYYY. So this will convert if old to new format
        /// MMDDYYYY for all driver lic
        /// </summary>
        /// <param name="Age"></param>
        /// <returns></returns>
        private string AgeValidate(string DBB)
        {
            string aMonth, aDay, aYear;
            string Dob = string.Empty;
            try
            {
                if(!string.IsNullOrEmpty(DBB))
                {
                    string dobcheck = DBB.Substring(0, 2); //Check if the Year is first

                    if(Convert.ToInt32(dobcheck) < 13)
                    {
                        Dob = DBB.ToString(); //MMDDYYYY format
                    }
                    else
                    {
                        aYear = DBB.Substring(0, 4); //Year
                        aMonth = DBB.Substring(4, 2); //Month
                        aDay = DBB.Substring(6, 2); //Day

                        Dob = string.Format("{0:00}", aMonth) + string.Format("{0:00}", aDay) + aYear; //MMDDYYYY
                    }
                }
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "AgeValidate()");
                throw new Exception("Invaild Date Of Birth format");
            }
            return Dob;
        }

        #endregion Age Validate

        #region Parsing of the Driver ScanData

        /// <summary>
        /// This will process(parse) the ID Card
        /// </summary>
        private bool ProcessDL()
        {
            string[] DLTEMP;
            string Key = string.Empty; // store the Keys
            string Val = string.Empty; // store the values
            string tmpVal;
            string[] splitVal = null;
            bool isFinish = false;
            bool isDAA = false;

            DLDictionary = new Dictionary<string, string>(); //dictionary to store the split data
            Dictionary<string,string> TmpDict = new Dictionary<string, string>(); //tmp dictionary
            try
            {
                DLTEMP = this.DLInfo.Replace("\r", "").Split('\n'); //Cleaning up the scanned data

                foreach(var s in DLTEMP)
                {
                    /* variable to reuse */
                    int index = 0;
                    tmpVal = string.Empty;
                    splitVal = null;

                    if(s.Length > 3)
                    {
                        /* Searching for the tags and values */
                        if(s.Length > 4 && s.Substring(0, 4).ToUpper() == "AAMV" && s.ToUpper().Contains("DAA")) //check the type of DL
                        {
                            index = s.ToUpper().IndexOf("DAA"); //Contain the full name seperated by @
                            if(index > 0)
                            {
                                tmpVal = s.Substring(index + 3);
                                splitVal = tmpVal.Split('@');
                                if(splitVal.Length > 0)
                                {
                                    Key = "DCS"; //Last Name
                                    Val = splitVal[0];
                                }
                                TmpDict.Add(Key, Val); //temp dictionary

                                if(splitVal.Length > 1)
                                {
                                    Key = "DCT"; //First Name
                                    Val = splitVal[1];
                                }
                                TmpDict.Add(Key, Val);//temp dictionary
                                isDAA = true;
                            }
                        }
                        else if(s.Length > 3 && s.Substring(0, 4).ToUpper() == "ANSI" && s.ToUpper().Contains("DAQ")) //Type of DL
                        {
                            index = s.ToUpper().IndexOf("DAQ"); //Lic#
                            if(index > 0)
                            {
                                Key = "DAQ";
                                Val = s.Substring(index + 3);
                            }
                        }
                        else if(!isDAA && s.Length > 3 && s.Substring(0, 3).ToUpper() == "DAA") //Older DL card
                        {
                            tmpVal = s.Substring(3);
                            splitVal = tmpVal.Split('@');
                            if(splitVal.Length > 0)
                            {
                                Key = "DCS"; //Last Name
                                Val = splitVal[0];
                            }
                            TmpDict.Add(Key, Val);
                            if(splitVal.Length > 1)
                            {
                                Key = "DCT"; //First Name
                                Val = splitVal[1];
                            }
                            TmpDict.Add(Key, Val);
                            isDAA = true;
                        }
                        else // All other keys
                        {
                            if(s.Length > 3 && s.Substring(0, 4).ToUpper() == "ANSI")
                            {
                                Key = s.Substring(0, 4).ToUpper(); // the ANSI String
                                Val = s.Substring(4);
                            }
                            else
                            {
                                Key = s.Substring(0, 3).ToUpper();
                                Val = s.Substring(3);
                            }
                        }
                        if(!string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(Val))
                        {
                            if(!isDAA)
                            {
                                DLDictionary.Add(Key, Val); //create a dictionary with all the new string
                            }
                            else
                            {
                                foreach(var k in TmpDict.Keys)
                                {
                                    Key = k;
                                    Val = TmpDict[k];
                                    DLDictionary.Add(Key, Val);
                                }
                                isDAA = false;
                            }
                        }
                    }
                }
                isFinish = true;
            }
            catch(Exception ex)
            {
                logger.Fatal(ex, "ProcessDL()");
                throw ex;
            }
            finally
            {
                /* Clearing all data used */
                TmpDict.Clear();
                splitVal = null;
                tmpVal = string.Empty;
                Key = string.Empty;
                Val = string.Empty;
                DLTEMP = null;
            }
            return isFinish;
        }

        #endregion Parsing of the Driver ScanData
    }
}