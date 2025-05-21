//Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to read parameters of Merchant infor from the MerchatConfig.xml file
//External functions:None   
//Known Bugs : None
//Start Date : 03 January 2008.
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using PossqlData;
using System.Linq;
using NLog;
using System.Data;

namespace MMS.PROCESSOR
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to read parameters of Merchant infor from the MerchatConfig.xml file
    //External functions:None   
    //Known Bugs : None
    //Start Date : 03 January 2008.
    public class MerchantInfo : IDisposable
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        #region constants
        private const String CONFIGFILE = "MerchantConfig.xml";
        private const String USER_ID = "USER_ID";
        private const String PAYMENT_SERVER = "PAYMENT_SERVER";
        private const String PORT_NO = "PORT_NO";
        private const String CARDINFO = "CARDINFO";
        private const String PAYMENTCLIENT = "PAYMENTCLIENT";
        private const String PAYMENTRESULTFILE = "PAYMENTRESULTFILE";
        private const String APPLICATION_NAME = "APPLICATIONNAME";
        private const String XCCLIENTUITITLE = "XCCLIENTUITITLE";
        private const String PRIMEPOS ="PrimePOS";
        private const String HEADER = "HEADER";
        private const String HPS = "HPS";
        private const String PAYMENTSERVER = "PAYMENTSERVER";
        private const String SERVER = "SERVER";
        private const String LICENSEID = "LICENSEID";
        private const String SITEID = "SITEID";
        private const String DEVICEID = "DEVICEID";
        private const String strVCBin = "VisaHealthcareBins.xml"; //PRIMEPOS-2732 11-Sep-2019 JY Added
        private const String strMCBin = "MCHealthcareBins.xml";   //PRIMEPOS-2732 11-Sep-2019 JY Added
        #endregion

        #region variables
        private XmlToKeys Merchant = null;
        public String User = String.Empty;
        public String IPAddress = String.Empty;
        public String ApplicationName = String.Empty;
        public String XchargeWindowTitle = String.Empty;
        public int PortNo = 0;
        MMSDictionary<String, ProcessorInfo> CardProcessors = null;
        MMSDictionary<String, ProcessorInfo> HPSHearders = null;
        public String[] ProcList = null;
        private Boolean Disposed = false;
        public String PaymentClientPath = String.Empty;
        public String PaymentResultPath = String.Empty;

        public MerchantConfig oMerchantInfo = null;
        #endregion
        /// <summary>
        /// Author : Prashant 
        /// Functionality Desciption : Theis is the Constructor of the MerchantInfo
        /// External functions:XmlToKeys.GetFields.
        /// Known Bugs : None
        /// Start Date : 03 Jan 2008.
        /// </summary>
        public MerchantInfo()
        {
            logger.Trace("IN Constructor MerchantInfo() ");
            CheckAndLoadMerchantConfig();
            /*MMSDictionary<String, String> keys = new MMSDictionary<String, String>();
            Merchant = new XmlToKeys();
            Merchant.GetFields(CONFIGFILE, "", ref keys, true);
            String value = String.Empty;
            String hpsTags=String.Empty;
            if (keys.TryGetValue(USER_ID, out value))
                User = value.Trim();
            if (keys.TryGetValue(PORT_NO, out value))
                PortNo = Convert.ToInt32(value.Trim());
            if (keys.TryGetValue(PAYMENT_SERVER, out value))
                IPAddress = value.Trim();
            if (keys.TryGetValue(PAYMENTCLIENT, out value))
                PaymentClientPath = value.Trim();
            if (keys.TryGetValue(PAYMENTRESULTFILE, out value))
                PaymentResultPath = value.Trim();

            if (keys.TryGetValue(CARDINFO, out value))
            {
                CardProcessors = new MMSDictionary<String, ProcessorInfo>();
                ParseCardList(value.Trim());
            }            
            if (keys.TryGetValue(PAYMENTSERVER, out hpsTags))
            {
                HPSHearders = new MMSDictionary<string, ProcessorInfo>();
                ParseHPSHeaders(hpsTags.Trim());
            }
            if (keys.TryGetValue(APPLICATION_NAME, out value))
            {
                ApplicationName = value.Trim();
            }
            else
            {
                ApplicationName = PRIMEPOS;
            }
            if (keys.TryGetValue(XCCLIENTUITITLE, out value))
            {
                XchargeWindowTitle = value;
            }
            else
            {
                XchargeWindowTitle = "Prime POS CC Processing";
            }
            keys.Clear();
            keys = null;*/
        }
        public void CheckAndLoadMerchantConfig()
        {
            logger.Trace("In CheckandLoadMerchantConfig()");
            bool exists = false;
            try
            {
                using (var db = new Possql())
                {
                    if (db.MerchantConfigs.Any())
                    {
                        exists = true;
                    }
                    else
                    {
                        exists = false;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error Occured While Checking MerchantConfig");
            }
            

            if (exists)
            {
                LoadMerchantConfig();
            }
            else
            {
                InsertToMerchantConfig();
            }

        }
        private void LoadMerchantConfig()
        {
            logger.Trace("In LoadMerchantConfig() LOading Merchant Config From DB");
            try
            {
                using (var db = new Possql())
                {
                    oMerchantInfo = (from config in db.MerchantConfigs select config).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, "An error Occured While Loading MerchantConfig");
            }
            
        }

        private void InsertToMerchantConfig()
        {
            logger.Trace("In InserttoMerchantConfig() ");
            string value = string.Empty;
            MMSDictionary<String, String> keys = new MMSDictionary<String, String>();
            Merchant = new XmlToKeys();
            Merchant.GetFields(CONFIGFILE, "", ref keys, true);

            oMerchantInfo = new MerchantConfig();
            DataTable dt = GetColumnDetails("MerchantConfig");

            #region User ID
            if (keys.TryGetValue(USER_ID, out value))
            {
                int nLen = GetColumnLength(dt,USER_ID);
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.User_ID = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.User_ID = value.Trim();
            }
            else
            {
                oMerchantInfo.User_ID = string.Empty;
            }
            #endregion

            #region Payment Server
            if (keys.TryGetValue(PAYMENT_SERVER, out value))
            {
                int nLen = GetColumnLength(dt, PAYMENT_SERVER);
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.Payment_Server = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.Payment_Server = value.Trim();
            }
            else
            {
                oMerchantInfo.Payment_Server = string.Empty;
            }
            #endregion

            #region Port no
            if (keys.TryGetValue(PORT_NO, out value))
            {
                int nLen = GetColumnLength(dt, PORT_NO);
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.Port_No = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.Port_No = value.Trim();
            }
            else
            {
                oMerchantInfo.Port_No = string.Empty;
            }
            #endregion

            #region Payment Server
            if (keys.TryGetValue(PAYMENT_SERVER, out value))
            {
                int nLen = GetColumnLength(dt, PAYMENT_SERVER);
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.Payment_Server = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.Payment_Server = value.Trim();
            }
            else
            {
                oMerchantInfo.Payment_Server = string.Empty;
            }
            #endregion
            
            #region Payment Client
            if (keys.TryGetValue(PAYMENTCLIENT, out value))
            {
                int nLen = GetColumnLength(dt, "Payment_Client");
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.Payment_Client = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.Payment_Client = value.Trim();
            }
            else
            {
                oMerchantInfo.Payment_Client = string.Empty;
            }
            #endregion

            #region Payment Result File
            if (keys.TryGetValue(PAYMENTRESULTFILE, out value))
            {
                int nLen = GetColumnLength(dt, "Payment_ResultFile");
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.Payment_ResultFile = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.Payment_ResultFile = value.Trim();
            }
            else
            {
                oMerchantInfo.Payment_ResultFile = string.Empty;
            }
            #endregion

            #region Application Name
            if (keys.TryGetValue(APPLICATION_NAME, out value))
            {
                int nLen = GetColumnLength(dt, "Application_Name");
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.Application_Name = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.Application_Name = value.Trim();
            }
            else
            {
                oMerchantInfo.Application_Name = string.Empty;
            }
            #endregion

            #region XC Client UI Title
            if (keys.TryGetValue(XCCLIENTUITITLE, out value))
            {
                int nLen = GetColumnLength(dt, XCCLIENTUITITLE);
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.XCClientUITitle = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.XCClientUITitle = value.Trim();
            }
            else
            {
                oMerchantInfo.XCClientUITitle = string.Empty;
            }
            #endregion

            #region License ID
            if (keys.TryGetValue(LICENSEID, out value))
            {
                int nLen = GetColumnLength(dt, LICENSEID);
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.LicenseID = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.LicenseID = value.Trim();
            }
            else
            {
                oMerchantInfo.LicenseID = string.Empty;
            }
            #endregion

            #region Site ID
            if (keys.TryGetValue(SITEID, out value))
            {
                int nLen = GetColumnLength(dt, SITEID);
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.SiteID = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.SiteID = value.Trim();
            }
            else
            {
                oMerchantInfo.SiteID = string.Empty;
            }
            #endregion

            #region Device ID
            if (keys.TryGetValue(DEVICEID, out value))
            {
                int nLen = GetColumnLength(dt, DEVICEID);
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.DeviceID = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.DeviceID = value.Trim();
            }
            else
            {
                oMerchantInfo.DeviceID = string.Empty;
            }
            #endregion

            #region URL
            if (keys.TryGetValue("URL", out value))
            {
                int nLen = GetColumnLength(dt, "URL");
                if (nLen != 0 && value.Trim().Length > nLen)
                    oMerchantInfo.URL = value.Trim().Substring(0, nLen);
                else
                    oMerchantInfo.URL = value.Trim();
            }
            else
            {
                oMerchantInfo.URL = string.Empty;
            }
            #endregion

            #region VC BIN
            oMerchantInfo.VCBin = strVCBin; //PRIMEPOS-2732 11-Sep-2019 JY Added
            //if (keys.TryGetValue("VCBIN", out value))
            //{
            //    oMerchantInfo.VCBin = value.Trim();
            //}
            //else
            //{
            //    oMerchantInfo.VCBin = string.Empty;
            //}
            #endregion

            #region MC BIN
            oMerchantInfo.MCBin = strMCBin; //PRIMEPOS-2732 11-Sep-2019 JY Added
            //if (keys.TryGetValue("MCBIN", out value))
            //{
            //    oMerchantInfo.MCBin = value.Trim();
            //}
            //else
            //{
            //    oMerchantInfo.MCBin = string.Empty;
            //}
            #endregion

            #region Cardinfo
            if (keys.TryGetValue(CARDINFO, out value))
            {
                CardProcessors = new MMSDictionary<String, ProcessorInfo>();
                ParseCardList(value.Trim());

                ProcessorInfo procInfo = null;
               
                foreach (KeyValuePair<String, ProcessorInfo> kvp in CardProcessors)
                {
                    procInfo = kvp.Value;
                    int nLen = GetColumnLength(dt, "Merchant");
                    if (nLen != 0 && procInfo.MERCHNUM.Trim().Length > nLen)
                        oMerchantInfo.Merchant = procInfo.MERCHNUM.Trim().Substring(0, nLen);
                    else
                        oMerchantInfo.Merchant = procInfo.MERCHNUM.Trim();

                    nLen = GetColumnLength(dt, "Processor_ID");
                    if (nLen != 0 && procInfo.PROCESSORID.Trim().Length > nLen)
                        oMerchantInfo.Processor_ID = procInfo.PROCESSORID.Trim().Substring(0, nLen);
                    else
                        oMerchantInfo.Processor_ID = procInfo.PROCESSORID.Trim();
                    break;
                }
            }
            #endregion

            try
            {
                using (var db = new Possql())
                {
                    #region PRIMEPOS-2732 11-Sep-2019 JY "MerchantConfig" should consist single record, but some times it got failed so to restrict the same aded below code block
                    try
                    {
                        if (db.MerchantConfigs.Any())
                        {
                            db.Database.ExecuteSqlCommand("TRUNCATE TABLE MerchantConfig");
                        }
                    }
                    catch { }
                    #endregion

                    db.MerchantConfigs.Add(oMerchantInfo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error Occured While Saving  MerchantConfig");
            }
        }

        #region PRIMEPOS-2732 11-Sep-2019 JY Added
        private DataTable GetColumnDetails(string TableName)
        {            
            using (var db = new Possql())
            {
                var dt = new DataTable();
                var conn = db.Database.Connection;
                var connectionState = conn.State;
                try
                {
                    if (connectionState != ConnectionState.Open) conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        string strSQL = "SELECT COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TableName + "'";
                        cmd.CommandText = strSQL;
                        cmd.CommandType = CommandType.Text;
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (connectionState != ConnectionState.Closed) conn.Close();
                }
                return dt;
            }
        }

        private int GetColumnLength(DataTable dt, string strColumn)
        {
            int nLen = 0;
            try
            {
                DataRow[] dr = dt.Select("COLUMN_NAME = '" + strColumn + "'");
                if (dr != null)
                {
                    nLen = Convert.ToInt32(dr[0]["CHARACTER_MAXIMUM_LENGTH"].ToString());
                }
                return nLen;
            }
            catch(Exception Ex)
            {
                return 0;
            }
        }
        #endregion

        public String[] GetValidCards()
        {
            logger.Trace("in GetValidCards()");
            String[] validCards = null;
            if (CardProcessors == null)
                return null;
            validCards = new String[CardProcessors.Count];
            int count = 0;
            foreach (KeyValuePair<String, ProcessorInfo> kvp in CardProcessors)
            {
                validCards[count] = kvp.Key.Trim();
                count++;
            }
            return validCards;
        }

        public bool GetMerchantInfo(String card, out String merchantNumber, out String processorId)
        {
            logger.Trace("In GetMerchantInfo()");
            ProcessorInfo procInfo = null;
            //Ritesh 6-Dec-08
            //Ritesh changed this to avoid multiple merchant information which is unnecessary. 
            //Instead of searching on a card it will always retrun the first cardinformation details
            //This will only give the value for the first Merchant and Processor
            foreach( KeyValuePair<String,ProcessorInfo> kvp in CardProcessors)
            {
                procInfo = kvp.Value; 
                merchantNumber = procInfo.MERCHNUM;
                processorId = procInfo.PROCESSORID;
                return true;
            }
            merchantNumber = null;
            processorId = null;
            return false;
        }

        public MMSDictionary<String, String> GetMerchantInfo()
        {
            logger.Trace("In GetMerchantInfo()");
            MMSDictionary<String, String> keys = new MMSDictionary<String, String>();
            Merchant = new XmlToKeys();
            Merchant.GetFields(CONFIGFILE, "", ref keys, true);
            return keys;
        }

        private void ParseCardList(String validCards)
        {
            logger.Trace("In ParseCardList()");

            XmlDocument msgDocument = new XmlDocument();
            XmlNodeList nodeList = null;
            try
            {
                msgDocument.LoadXml(validCards);
                nodeList = msgDocument.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    //if no filter is ther means all child from root elemet are required
                    ProcessorInfo procInfo = null;
                    if (node.Name == "CARD")
                    {
                        XmlNode attibute = node.Attributes.Item(0);
                        procInfo = new ProcessorInfo(node.OuterXml);
                        CardProcessors.Add(attibute.InnerText.Trim(), procInfo);
                    }
                }
                nodeList = null;
                msgDocument.RemoveAll();
                msgDocument = null;
            }
            catch (Exception ex)
            {
                //Logwrite ex
            }
            return;

        }



        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : Theis function is get HPS Header tags from Merchant.Config File
        /// Known Bugs : None
        /// Start Date : 03 Aug 2010.
        /// </summary>
        public bool GetHPSHeaderInfo(String header,out String LicenseID,out String siteID,out String deviceID)
        {
            logger.Trace("In GetHPSHeaderInfo()");
            ProcessorInfo procInfo = null;
            foreach (KeyValuePair<String, ProcessorInfo> kvp in HPSHearders)
            {
                procInfo = kvp.Value;
                LicenseID = procInfo.LICENSEID;
                siteID = procInfo.SITEID;
                deviceID = procInfo.DEVICEID;
                return true;
            }
            LicenseID = null;
            siteID = null;
            deviceID = null;
            return false;
        }



        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : Theis function is Parse HPS Header tags 
        /// Known Bugs : None
        /// Start Date : 03 Aug 2010.
        /// </summary>
        private void ParseHPSHeaders(String validNode)
        {
            logger.Trace("In ParseHPSHeaders()");
            XmlDocument msgDocument = new XmlDocument();
            XmlNodeList nodeList = null;
            try
            {
                msgDocument.LoadXml(validNode);
                nodeList = msgDocument.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    //if no filter is ther means all child from root elemet are required
                    ProcessorInfo procInfo = null;
                    if (node.Name == "HPS_SERVER")
                    {
                        XmlNode attibute = node.Attributes.Item(0);
                        procInfo = new ProcessorInfo(node.OuterXml);
                        HPSHearders.Add(attibute.InnerText.Trim(), procInfo);
                    }
                }
                nodeList = null;
                msgDocument.RemoveAll();
                msgDocument = null;

            }
            catch (Exception ex)
            { }
        }


        /// <summary>
        /// Author : Prashant 
        /// Functionality Desciption : Theis is the Destructor of the MerchantInfo
        /// External functions:XmlToKeys.GetFields.
        /// Known Bugs : None
        /// Start Date : 03 Jan 2008.
        /// </summary>
        ~MerchantInfo()
        {
            logger.Trace("MerchantInfo destructor\n");
            Dispose(false);
        }

        #region IDisposable Members
        private void Dispose(Boolean disposing)
        {
            logger.Trace("In Dispose()");
            if (Disposed)
                return;

            if (disposing)
            {
                if (CardProcessors != null)
                {
                    CardProcessors.Clear();
                    CardProcessors = null;
                }
                if (Merchant != null)
                    Merchant = null;
                User = null;
                ProcList = null;
                IPAddress = null;
                PortNo = 0;
            }

            // Unmanaged cleanup code here

            Disposed = true;

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
