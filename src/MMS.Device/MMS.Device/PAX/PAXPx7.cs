using MMS.Device.Global;
using MMS.GlobalPayments.Api.Entities;
//using SecureSubmit.Terminals;//Commented by Amit Gupta on 02 Dec 2020//PRIMEPOS-2931
//using SecureSubmit.Terminals.PAX;//Commented by Amit Gupta on 02 Dec 2020//PRIMEPOS-2931
using MMS.GlobalPayments.Api.Services;//Added by Amit Gupta on 02 Dec 2020//PRIMEPOS-2931
using MMS.GlobalPayments.Api.Terminals;//Added by Amit Gupta on 02 Dec 2020//PRIMEPOS-2931
using MMS.GlobalPayments.Api.Terminals.PAX;//Added by Amit Gupta on 02 Dec 2020//PRIMEPOS-2931
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MMS.Device.PAX
{


    public class PAXPx7
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region Variable and Properties
        //PaxDevice device = null;//Commented by Amit on 02 Dec 2020
        IDeviceInterface device;//Added by Amit Gupta on 02 Dec 2020
        private static PAXPx7 dDefObj = null;
        static Thread DeviceThread = null;
        private readonly object DeviceLock = new object();
        delegate void CommandHandler();
        event CommandHandler CommandEvent;
        Hashtable PData;
        delegate void PAXErrorEventLog();
        static event PAXErrorEventLog ErrorLog;
        private List<Order> deviceData = null;
        private List<Order> deviceDataTemp = null; //Aries8 Keep when Payment screen open and use after Back screen.
        Queue<CommandStation> CommandQueue = new Queue<CommandStation>();
        //private PaxDeviceResponse deviceResponse;//Commented by Amit Gupta on 02 Dec 2020
        private PaxTerminalResponse deviceResponse;//Added by Amit Gupta on 02 Dec 2020
        CultureInfo CI = CultureInfo.CurrentCulture;

        public const string BTN_YES = "1";
        public const string BTN_NO = "2";

        //Nileshj - Handling Thread Exception - 20-Dec-2018
        public object exceptionObj = new object();
        public bool isException = false;
        //Aries8
        public DeviceType _deviceType = DeviceType.PAX_PX7;//Set Default for Old Device
        private string PAX_DEVICE_ARIES8 = "HPSPAX_ARIES8"; // Amit HPSPAX_ARIES8 Add 
        private string PAX_DEVICE_A920 = "HPSPAX_A920"; //PRIMEPOS-3146
        public string HIPPAAckResultId
        {
            get;
            set;
        }

        public string PatCounsel
        {
            get;
            set;
        }

        public string GetSignatureString
        {
            get;
            set;
        }

        public bool? IsSignValid
        {
            get;
            set;
        }

        public bool? IsResourceUpdated
        {
            get;
            set;
        }

        public PatientConsent PatConsent
        {
            get;
            set;
        }

        private DataTable RXList = null;
        private bool bSkipConsent = false;
        private PatientConsent tmpConsent = null;
        private int PattypeSelection = -1;
        public bool? shouldMoveNext = null;  //PRIMEPOS:2544 - Change Private To Public
        private int ConsentApplies = -1;
        private int ConsentStatus = -1;



        #endregion
        public static PAXPx7 DefaultInstance(string _posSettings, string _posPinPad)
        {
            if (dDefObj == null)
            {
                dDefObj = new PAXPx7(_posSettings, _posPinPad);
                dDefObj.deviceData = new List<Order>();
            }
            return dDefObj;
        }

        private PAXPx7(string _posSettings, string _posPinPad)
        {
            int timeOut = PaxInterface.DEFAULT_TIMEOUT;
            int reconnectWaitTime = PaxInterface.Default_ReconnectWaitTime;//PRIMEPOS-3087
            ConnectionModes ConMode = ConnectionModes.TCP_IP;
            string hostAddress = _posSettings.Split(':')[0];
            string hostPort = _posSettings.Split(':')[1].Split('/')[0];

            #region  NileshJ Disconnection Timeout Issue - 3-July-2019 - PRIMEPOS-2706            
            try
            {
                if (_posSettings.Contains("|"))
                {
                    timeOut = Convert.ToInt32(_posSettings.Split('|')[1]);
                }
            }
            catch (Exception)
            {
                timeOut = PaxInterface.DEFAULT_TIMEOUT;//Added by Amit Gupta on 02 Dec 2020
            }

            #endregion
            #region PRIMEPOS-2877
            try
            {
                if (_posSettings.Contains("~"))
                {
                    ConMode = (ConnectionModes)Enum.Parse(typeof(ConnectionModes), _posSettings.Split('~')[1], true);
                }
                else
                {
                    ConMode = ConnectionModes.TCP_IP;
                }
            }
            catch (Exception)
            {
                ConMode = ConnectionModes.TCP_IP;
            }
            #endregion

            #region PRIMEPOS-3087
            try
            {
                if (_posSettings.Contains("$"))
                {
                    reconnectWaitTime = PaxInterface.Default_ReconnectWaitTime + Convert.ToInt32(_posSettings.Split('$')[1]);//Milisecond //PRIMEPOS-3087
                }
            }
            catch (Exception)
            {
            }
            logger.Trace($"HPSPAX Device Reconnect Wait Time: {reconnectWaitTime} (Milisecond)");
            #endregion
            //device = new PaxDevice(new ConnectionConfig()
            //{
            //    BaudRate = BaudRate.r19200,
            //    ConnectionMode = ConMode,// ConnectionModes.TCP_IP, // PRIMEPOS-2877 - Commented TCP
            //    IpAddress = hostAddress,
            //    Port = hostPort,
            //    TimeOut = timeOut //  PRIMEPOS-2706
            //});//Commented by Amit on 02 Dec 2020

            //Aries8
            if (_posPinPad.Trim().ToUpper() == PAX_DEVICE_ARIES8)
            {
                _deviceType = DeviceType.PAX_D200;
            }
            else if (_posPinPad.Trim().ToUpper() == PAX_DEVICE_A920)//PRIMEPOS-3146
            {
                _deviceType = DeviceType.PAX_A920;
            }
            else
            {
                _deviceType = DeviceType.PAX_PX7;
            }

            device = DeviceService.Create(new ConnectionConfig
            {
                ConnectionMode = ConMode,// ConnectionModes.HTTP,//ConnectionModes.TCP_IP Amit
                IpAddress = hostAddress,
                Port = hostPort,
                BaudRate = BaudRate.r19200,
                Timeout = timeOut,
                ReconnectWaitTime = reconnectWaitTime
            });

            DeviceCommand("INIT", new Hashtable());
            Console.WriteLine(deviceResponse);
            DeviceCommand("WELCOME", new Hashtable());
        }

        // NileshJ -  Call Reinit() when First Item add
        public void ReInit()
        {
            DeviceCommand("INIT", new Hashtable());
            Console.WriteLine(deviceResponse);
        }



        #region Device Public method to send commands
        //Add Other public methods here

        //14-Dec-2018 - Added for Override Rx - NileshJ
        public void ClearItems()
        {
            deviceData.Clear();
            DeviceCommand("CLEARMESSAGE", new Hashtable());
        }

        public void AddItem(Hashtable fields)
        {

            fields.Add("ACTION", "A");
            fields.Add("COMMAND", "SHOWRXLIST");
            DeviceCommand("SHOWMESSAGE", fields);
        }

        public void UpdateItem(Hashtable fields)
        {

            fields.Add("ACTION", "U");
            fields.Add("COMMAND", "SHOWRXLIST");
            DeviceCommand("SHOWMESSAGE", fields);
        }

        public void DeleteItem(Hashtable fields)
        {

            fields.Add("ACTION", "D");
            fields.Add("COMMAND", "DELETEITEM");
            DeviceCommand("SHOWMESSAGE", fields);
        }

        public void ResendAllItem()
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "RELOADITEMLIST");
            DeviceCommand("SHOWMESSAGE", fields);
        }

        public void SendOnHoldItems(DataTable dt, string ShowRxDesc)
        {
            logger.Debug(string.Format("In DisplayOnHoldItem()"));
            bool isDisplayOnHold = false;
            ArrayList OnHoldItem = new ArrayList();
            Hashtable data = new Hashtable();
            try
            {
                logger.Debug("Entering DisplayOnHoldItem()");
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Constant.dataStore.Count >= 0)
                    {
                        Constant.dataStore.Clear();
                    }
                    else
                    {
                        Constant.dataStore = new ArrayList();
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i]; //rows
                        Hashtable fields = new Hashtable();
                        fields.Add("ItemName", dr["ItemDescription"].ToString());
                        fields.Add("ItemQty", dr["QTY"].ToString());
                        fields.Add("UnitPrice", Convert.ToDouble(dr["Price"].ToString()).ToString("F", CI).PadRight(2, '0'));
                        fields.Add("TotalPrice", Convert.ToDouble(dr["Discount"].ToString()).ToString("F", CI).PadRight(2, '0'));
                        fields.Add("ISRX", dr["ItemID"].ToString().ToUpper() == "RX" ? 1 : 0);
                        fields.Add("SHOWRXDESCRIPTION", ShowRxDesc);
                        fields.Add("DISCOUNT", Convert.ToDouble(dr["Discount"].ToString()).ToString("F", CI).PadRight(2, '0'));
                        OnHoldItem.Add(fields);
                        Constant.dataStore.Add(fields);

                    }
                    data.Add("OnHoldItems", OnHoldItem); //add the arraylist to the Hashtable for passing to pad
                    data.Add("COMMAND", "HOLDITEMS");
                    DeviceCommand("SHOWMESSAGE", data);
                }
                logger.Debug("Exiting DisplayOnHoldItem(): " + isDisplayOnHold);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error In Comm DisplayOnHoldItem(): \n" + ex.Message);
                throw new Exception(ex.ToString());
            }

        }


        public void DisplayScreen(string sCurrentTransID, string screen, string strData)
        {

            Hashtable customMessage = new Hashtable();
            customMessage.Add("COMMAND", screen /*Show RX INFO Remaining*/);
            customMessage.Add("TITLE", "Message");
            customMessage.Add("MESSAGE", strData);
            DeviceCommand("SHOWMESSAGE", customMessage);
        }

        public void CaptureNOPP(string sCurrentTransID, string sPatientName, string sPatientAddress, string privacyText)
        {
            //reset to null for newer button click id

            HIPPAAckResultId = null;
            Hashtable NOPPCommandData = new Hashtable();
            NOPPCommandData.Add("COMMAND", "HIPPAACK");
            //Aries8
            if (_deviceType == DeviceType.PAX_D200)
            {
                NOPPCommandData.Add("TITLE", @"HIPAA Acknowledgement"); //PRIMEPOS-3212
            }
            else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
            {
                NOPPCommandData.Add("TITLE", @"\1HIPAA Acknowledgement"); //PRIMEPOS-3212
            }
            else
            {
                NOPPCommandData.Add("TITLE", @"\2 HIPAA Acknowledgement"); //PRIMEPOS - 2555 Add @"\2" for Medium Font text to display on device - NileshJ //PRIMEPOS-3212
            }
            NOPPCommandData.Add("PATIENTNAME", sPatientName);
            NOPPCommandData.Add("PATIENTADDRESS", sPatientAddress);
            NOPPCommandData.Add("MESSAGE", privacyText);
            DeviceCommand("SHOWTEXTBOX", NOPPCommandData);
        }



        public void CapturePatConsel(string sCurrentTransID, string totalRx, string patientName, string patientCounceling, DataTable rxList, bool bHidePatCounseling = false) //PRIMEPOS-3245 added bHidePatCounseling
        {
            //reset to null for newer button click id

            try
            {
                if (RXList != null && RXList.Rows.Count > 0)
                {
                    RXList.Clear();
                }
                else if (RXList == null)
                {
                    RXList = new DataTable();
                }

                RXList = rxList.Copy();//addrx() line 801
                logger.Debug("In Comm RxSign Total Rx's: " + RXList.Rows.Count);
                Constant.Counsel = patientCounceling; //orignial Patient counseling from POS

                //Append RXinfo To Device Message
                string RXData = prepareRXData();

                Hashtable RXPatConsel = new Hashtable();
                RXPatConsel.Add("COMMAND", "RXHEADERINFO" /*Show RX INFO Remaining*/);
                //Aries8
                if (_deviceType == DeviceType.PAX_D200)
                {
                    RXPatConsel.Add("TITLE", @"Rx Pickup Acknowledgement");
                }
                else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                {
                    RXPatConsel.Add("TITLE", @"\1Rx Pickup Acknowledgement");
                }
                else
                {
                    RXPatConsel.Add("TITLE", @"\2 Rx Pickup Acknowledgement");  //PRIMEPOS - 2555  Add @"\2" for Medium Font text to display on device - NileshJ
                }
                RXPatConsel.Add("PATIENTNAME", patientName);
                RXPatConsel.Add("RXCOUNT", totalRx);
                RXPatConsel.Add("RXDATA", RXData);
                RXPatConsel.Add("ISPATCONSELHIDE", bHidePatCounseling); //PRIMEPOS-3245
                DeviceCommand("SHOWTEXTBOX", RXPatConsel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public void CapturePatConsent(string patConsentProvider, string pharmacy, DataSet dsConsentDetails = null) // PRIMEPOS-2869 - Added  DataSet dsConsentDetails = null
        {
            //if (patConsentProvider.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_HEALTHIX.ToUpper()) //Commented by Amit Gupta on 02 Dec 2020
            if (patConsentProvider.ToUpper() == Global.Constants.CONSENT_SOURCE_HEALTHIX.ToUpper())// PRIMEPOS-2869 - Added Healthix Condition//added by Amit Gupta on 02 Dec 2020
            {
                logger.Debug("In PerformConsentCaptureforHealthix");
                tmpConsent = new PatientConsent();
                string PharmacyName = pharmacy;

                GetPatientType(Constants.CONSENT_SOURCE_HEALTHIX);

                if (!bSkipConsent)
                {
                    ShowHealtixForm1();
                }
                if (!bSkipConsent)
                {
                    ShowHealtixForm2(PharmacyName);
                }
                if (!bSkipConsent)
                {
                    ShowHealtixForm3();
                }

                PatConsent = tmpConsent;
                tmpConsent = null;
            }
            else // PRIMEPOS-2869 - Added else for other Consent
            {
                tmpConsent = new PatientConsent();
                if (dsConsentDetails.Tables.Count > 0)
                {
                    SelectPatientType(dsConsentDetails);
                    if (PattypeSelection > -1)
                    {
                        ShowPatConsentText(dsConsentDetails);
                    }
                    if (ConsentApplies > -1)
                    {
                        if (ConsentApplies == 3)
                        {
                            tmpConsent = new PatientConsent();
                            tmpConsent.IsConsentSkip = true;
                        }

                        PatConsent = tmpConsent;
                    }
                }
                tmpConsent = null;
            }
        }
        #region Autorefill PRIMEPOS-2869

        private void ShowPatConsentText(DataSet ds)
        {
            try
            {
                logger.Debug("In ShowPatConsentText()=> Command  = SHOWDYNAMICFORM  ");
                DataTable dtPatient = new DataTable();
                dtPatient = ds.Tables["PATIENT"];

                DataTable dtConsentStatus = new DataTable();
                dtConsentStatus = ds.Tables["Consent_Status"];

                string alteredMsg = "_________________________________________________" + System.Environment.NewLine +
                                     "\\BPatient : " + dtPatient.Rows[0]["FNAME"].ToString() + " " + dtPatient.Rows[0]["LNAME"].ToString() + System.Environment.NewLine +
                                     "\\BPatientAddress :" + dtPatient.Rows[0]["ADDRSTR"].ToString() + " ," + dtPatient.Rows[0]["ADDRCT"].ToString() + " ," + dtPatient.Rows[0]["ADDRST"].ToString() + " ," + dtPatient.Rows[0]["ADDRZP"].ToString() + System.Environment.NewLine +
                                     "_________________________________________________" + System.Environment.NewLine + System.Environment.NewLine;

                DataTable dt = new DataTable();
                dt = ds.Tables["ConsentTextVersion"];
                Hashtable _PATCONSENTTEXT = new Hashtable();
                _PATCONSENTTEXT.Add("COMMAND", "PATCONSENTTEXT");
                _PATCONSENTTEXT.Add("TITLE", @"\2" + dt.Rows[0]["ConsentTextTitle"].ToString());
                _PATCONSENTTEXT.Add("MESSAGE", @"\1" + alteredMsg + dt.Rows[0]["ConsentText"].ToString());
                int i = 1;
                foreach (DataRow dr in dtConsentStatus.Rows)
                {
                    _PATCONSENTTEXT.Add("Button" + i, @"\1" + dr["Description"].ToString());
                    i++;
                }

                DeviceCommand("SHOWDYNAMICFORM", _PATCONSENTTEXT);
                ConsentApplies = -1;
                while (ConsentApplies == -1)
                {
                    Application.DoEvents(); //PRIMEPOS-3263
                    Thread.Sleep(100);
                }
                if (ConsentApplies > -1)
                {
                    if (ConsentApplies != 3)
                    {
                        tmpConsent.ConsentStatusCode = dtConsentStatus.Rows[ConsentApplies - 1]["Code"].ToString();
                        tmpConsent.ConsentStatusID = Convert.ToInt32(dtConsentStatus.Rows[ConsentApplies - 1]["ID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug("\t\tError  \n" + ex.ToString());
            }
        }

        private void SelectPatientType(DataSet ds)
        {
            try
            {
                DataTable dtnew = new DataTable();
                logger.Debug("In selectPatientType - Entered ");
                DataTable dt = new DataTable();
                dt = ds.Tables["ConsentTextVersion"];
                bSkipConsent = false;
                PattypeSelection = -1;
                Hashtable PatientPickingUpRX = new Hashtable();
                PatientPickingUpRX.Add("TITLE", @"\2" + dt.Rows[0]["ConsentTextTitle"].ToString());
                DataTable dtRec = new DataTable();
                dtRec.Clear();
                dtRec = ds.Tables["Consent_Relationship"];
                int i = 1;
                foreach (DataRow dr in dtRec.Rows)
                {
                    PatientPickingUpRX.Add("LABEL" + i, @"\1" + dr["Relation"].ToString());
                    i++;
                }
                PatientPickingUpRX.Add("BUTTONTYPE", 1);
                PatientPickingUpRX.Add("COMMAND", "PATIENTTYPE");
                DeviceCommand("SHOWDYNAMICFORM", PatientPickingUpRX);
                while (PattypeSelection == -1)
                {
                    Application.DoEvents(); //PRIMEPOS-3263
                    Thread.Sleep(100);
                }
                if (PattypeSelection > -1)
                {
                    tmpConsent.PatConsentRelationID = Convert.ToInt32(dtRec.Rows[PattypeSelection - 1]["ID"]);
                    tmpConsent.PatConsentRelationShipDescription = dtRec.Rows[PattypeSelection - 1]["Relation"].ToString();
                    tmpConsent.ConsentTextID = Convert.ToInt32(dt.Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            {
                logger.Debug("\t\tError  \n" + ex.ToString());
            }
        }
        #endregion

        public bool CaptureRXSig()
        {
            Hashtable RXSignatureData = new Hashtable();
            DeviceCommand("DOSIGNATURE", null);
            return true;
        }

        public bool GetSignature()
        {
            DeviceCommand("GETSIGNATURE", null);
            return true;
        }


        public void ShowDialog(string v, Hashtable fields)
        {
            DeviceCommand(v, fields);
        }

        public void EndTxn(string amount)
        {
            logger.Debug("In EndTxn()");
            int rtCode = 1;
            try
            {
                logger.Debug("Entering Comm EndTxn ");
                if (Constant.CCDataSwipe == null)
                {
                    Constant.CCDataSwipe = new ArrayList();
                }
                else
                {
                    Constant.CCDataSwipe.Clear();
                }

                //Add rx list 
                //rxListTable = null;
                Hashtable commandData = new Hashtable();
                commandData.Add("AMT", amount);
                DeviceCommand("SHOWTHANKYOU", commandData);
                deviceData.Clear();
                Constant.dataStore = null;
                rtCode = 0;
                logger.Debug("Exiting Comm EndTxn");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in Comm EndTxn: \n" + ex.Message);
            }
        }

        public void ResetToIdle()
        {
            Hashtable RXSignatureData = new Hashtable();
            DeviceCommand("RESET", null);
        }

        private string prepareRXData()
        {
            string RXData = "";
            for (int i = 0; i < RXList.Rows.Count; i++)
            {
                RXInfo drugInfo = new RXInfo();

                drugInfo.RXNo = string.IsNullOrEmpty(RXList.Rows[i]["RXNO"].ToString().Trim()) ? "000000" : RXList.Rows[i]["RXNO"].ToString();
                logger.Debug("In Comm RxSign: " + (string.IsNullOrEmpty(RXList.Rows[i]["RXNO"].ToString().Trim()) ? "000000" : RXList.Rows[i]["RXNO"].ToString()));

                drugInfo.drugName = string.Empty;
                if (Convert.ToBoolean(RXList.Rows[i]["SHOWRXDESCRIPTION"].ToString()) == false)
                {
                    drugInfo.drugName = "****";
                }
                else
                {
                    drugInfo.drugName = string.IsNullOrEmpty(RXList.Rows[i]["DRUGNAME"].ToString().Trim()) ? "Not Available" : RXList.Rows[i]["DRUGNAME"].ToString();
                }
                drugInfo.date = string.IsNullOrEmpty(RXList.Rows[i]["RXDATE"].ToString().Trim()) ? "00000000" : RXList.Rows[i]["RXDATE"].ToString();

                //Append Drug to Final RXData
                if (RXData.Length > 0)
                    RXData += System.Environment.NewLine + drugInfo.ToString();
                else
                    RXData += drugInfo.ToString();
            }
            return RXData;
        }


        public void UpdateResourceImage(byte[] file)
        {

            Hashtable resourceFileParam = new Hashtable();
            resourceFileParam.Add("FILE", file);
            DeviceCommand("UPDATERESOURCEFILE", resourceFileParam);
        }

        public void CapturePatConsent(string patConsentProvider, string pharmacy)
        {
            //Hashtable resourceFileParam = new Hashtable();
            //resourceFileParam.Add("FILE", file);
            //DeviceCommand("UPDATERESOURCEFILE", resourceFileParam);



            logger.Debug("In PerformConsentCaptureforHealthix");
            tmpConsent = new PatientConsent();
            string PharmacyName = pharmacy;


            GetPatientType(Constants.CONSENT_SOURCE_HEALTHIX);

            if (!bSkipConsent)
            {
                ShowHealtixForm1();
            }
            if (!bSkipConsent)
            {
                ShowHealtixForm2(PharmacyName);
            }
            if (!bSkipConsent)
            {
                ShowHealtixForm3();
            }

            PatConsent = tmpConsent;
            tmpConsent = null;
        }


        private void GetPatientType(string Consentsource)
        {
            logger.Debug("In GetPatientType()=> Getting Patient Type  ");
            bSkipConsent = false;
            PattypeSelection = -1;
            Hashtable PatientPickingUpRX = new Hashtable();
            //PRIMEPOS - 2555 - OLD Start
            //PatientPickingUpRX.Add("TITLE", @"\2Please select the person picking up the Prescription");
            //PatientPickingUpRX.Add("BUTTON1", @"\1Patient");
            //PatientPickingUpRX.Add("BUTTON2", @"\1Legal Representative");
            //PatientPickingUpRX.Add("BUTTON3", @"\1Both");
            //PatientPickingUpRX.Add("BUTTON4", @"\1Other");
            //PatientPickingUpRX.Add("COMMAND", "PATCONSENTSELECTPAT");
            //DeviceCommand("SHOWDIALOG", PatientPickingUpRX);
            //PRIMEPOS - 2555 - OLD End

            //PRIMEPOS - 2555  Add Parameter for Radio Button And Add @"\1" for Small Font text and @"\2" for Medium Font text to display on device -  NileshJ Start

            //Aries8
            if (_deviceType == DeviceType.PAX_D200)
            {
                PatientPickingUpRX.Add("TITLE", @"Please select the person picking up the Prescription");
                PatientPickingUpRX.Add("LABEL1", @"Patient");
                PatientPickingUpRX.Add("LABEL2", @"Legal Representative");
                PatientPickingUpRX.Add("LABEL3", @"Both");
                PatientPickingUpRX.Add("LABEL4", @"Other");
            }
            else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
            {
                PatientPickingUpRX.Add("TITLE", @"\1Select person picking up the Rx(s)");
                PatientPickingUpRX.Add("LABEL1", @"Patient");
                PatientPickingUpRX.Add("LABEL2", @"Legal Representative");
                PatientPickingUpRX.Add("LABEL3", @"Both");
                PatientPickingUpRX.Add("LABEL4", @"Other");
            }
            else
            {
                PatientPickingUpRX.Add("TITLE", @"\2Please select the person picking up the Prescription");
                PatientPickingUpRX.Add("LABEL1", @"\1Patient");
                PatientPickingUpRX.Add("LABEL2", @"\1Legal Representative");
                PatientPickingUpRX.Add("LABEL3", @"\1Both");
                PatientPickingUpRX.Add("LABEL4", @"\1Other");
            }

            PatientPickingUpRX.Add("BUTTONTYPE", 1);
            PatientPickingUpRX.Add("COMMAND", "PATCONSENTSELECTPAT");
            DeviceCommand("SHOWDIALOGFORM", PatientPickingUpRX);
            //PRIMEPOS - 2555  Add Parameter for Radio Button -  NileshJ End

            while (PattypeSelection == -1)
            {
                Application.DoEvents(); //PRIMEPOS-3263
                Thread.Sleep(100);
            }

            if (PattypeSelection > 0 && PattypeSelection <= 4)//PRIMEPOS-3146
            {
                tmpConsent.ConsentSourceName = Consentsource;
                tmpConsent.PatConsentRelationShipDescription = PatientConsent.GetRelationshipCodeForHealthix(PattypeSelection);
                if (PattypeSelection > 3)
                {
                    bSkipConsent = true;
                    tmpConsent.IsConsentSkip = true;
                }
            }
            else
            {
                bSkipConsent = true;
                tmpConsent.IsConsentSkip = true;
            }
        }

        private void ShowHealtixForm1()
        {
            logger.Debug("In ShowHelthixForm1()=>  ");

            Hashtable _HEALTHIXFRM1 = new Hashtable();
            _HEALTHIXFRM1.Add("COMMAND", "HEALTHIXFRM1");
            //PRIMEPOS - 2555 Add @"\1" for Small Font text and @"\2" for Medium Font text to display on device - NileshJ
            //Aries8
            if (_deviceType == DeviceType.PAX_D200)
            {
                _HEALTHIXFRM1.Add("TITLE", @"Authorization for Release of Patient Information");
            }
            else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
            {
                _HEALTHIXFRM1.Add("TITLE", @"\1Auth. for Release of Patient Info.");
            }
            else
            {
                _HEALTHIXFRM1.Add("TITLE", @"\2Authorization for Release of Patient Information");
            }

            _HEALTHIXFRM1.Add("MESSAGE", @"\1I request that health information regarding my care and treatment be accessed as "
                + "set forth on this form. I can choose whether or not to allow my health care providers and health plans to "
                + "obtain access to my medical records through the health information exchange organization called Healthix. "
                + "If I give consent, my medical records from the different places where I get health care can be accessed using"
                + " a statewide computer network. I can fill out this form once and allow all Healthix Participants (including their agents) "
                + "who provide me with treatment or care management services to electronically access my information. Healthix is a"
                + " not-for-profit organization that securely shares information about people's health electronically to improve the quality"
                + " of healthcare and meets the privacy and security standards of HIPAA and New York State Law. "
                + "To learn more visit Healthix's website at www.healthix.org");

            DeviceCommand("SHOWTEXTBOX", _HEALTHIXFRM1);

            while (shouldMoveNext == null)
            {
                Application.DoEvents(); //PRIMEPOS-3263
                Thread.Sleep(100);
            }

        }

        private void ShowHealtixForm2(string PharmacyName)
        {
            logger.Debug("In ShowHelthixForm1()=>  ");

            Hashtable _HEALTHIXFRM2 = new Hashtable();
            _HEALTHIXFRM2.Add("COMMAND", "HEALTHIXFRM2");
            //PRIMEPOS - 2555 Add @"\1" for Small Font text and @"\2" for Medium Font text to display on device - NileshJ
            //Aries8
            if (_deviceType == DeviceType.PAX_D200)
            {
                _HEALTHIXFRM2.Add("TITLE", "Authorization for Release of Patient Information");
            }
            else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
            {
                _HEALTHIXFRM2.Add("TITLE", @"\1Auth. for Release of Patient Info.");
            }
            else
            {
                _HEALTHIXFRM2.Add("TITLE", @"\2Authorization for Release of Patient Information");
            }

            _HEALTHIXFRM2.Add("MESSAGE", @"\1The choice I make in this form will NOT affect my ability to get medical care."
                + " The choice I make in this form does NOT allow health insurers to have access to my information for the"
                + " purpose of deciding whether to provide me with health insurance coverage or pay my medical bills.");
            _HEALTHIXFRM2.Add("PHARMACYNAME", string.IsNullOrEmpty(PharmacyName) ? "" : PharmacyName);
            DeviceCommand("SHOWTEXTBOX", _HEALTHIXFRM2);
            ConsentApplies = -1;
            while (ConsentApplies == -1)
            {
                Application.DoEvents(); //PRIMEPOS-3263
                Thread.Sleep(100);
            }
            if (!bSkipConsent)
            {
                tmpConsent.ConsentTypeCode = PatientConsent.GetConsentTypeCodeForHealthix(ConsentApplies);
            }
            else
            {
                tmpConsent.IsConsentSkip = true;
            }
        }

        private void ShowHealtixForm3()
        {
            logger.Debug("In ShowHelthixForm1()=>  ");

            Hashtable _HEALTHIXFRM3 = new Hashtable();
            _HEALTHIXFRM3.Add("COMMAND", "HEALTHIXFRM3");
            //Aries8
            if (_deviceType == DeviceType.PAX_D200)
            {
                _HEALTHIXFRM3.Add("TITLE", @"Authorization for Release of Patient Information");  //PRIMEPOS - 2555 Add @"\2" for Medium Font text to display on device - NileshJ
            }
            else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
            {
                _HEALTHIXFRM3.Add("TITLE", @"\1Auth. for Release of Patient Info."); //PRIMEPOS-3146
            }
            else
            {
                _HEALTHIXFRM3.Add("TITLE", @"\2Authorization for Release of Patient Information");  //PRIMEPOS - 2555 Add @"\2" for Medium Font text to display on device - NileshJ
            }

            DeviceCommand("SHOWTEXTBOX", _HEALTHIXFRM3);

            ConsentStatus = -1;
            while (ConsentStatus == -1)
            {
                Application.DoEvents(); //PRIMEPOS-3263
                Thread.Sleep(100);
            }
            if (!bSkipConsent)
            {
                tmpConsent.ConsentStatusCode = PatientConsent.GetStatusCodeForHealthix(ConsentStatus);
            }
            else
            {
                tmpConsent.IsConsentSkip = true;
            }

        }

        #region Added by Arvind PRIMEPOS-2734
        public bool IsStillWrite
        {
            get
            {
                return Constant.IsStillWrite;
            }
        }
        #endregion

        //PRIMEPOS:2544 - Waiting for OTC acknowledgement screen needs to be added for Item Monitoring Acknowledgement screen - NileshJ Start [14/06/2018]
        public void CaptureOTCItemSignature()
        {

            IsSignValid = null;
            DeviceCommand("DOSIGNATURE", null);
        }
        //PRIMEPOS:2544 Create New Method for Signature Aknowledgment - NileshJ
        public void CaptureOTCItemSignatureAknowledgement(ArrayList otcList)
        {

            Hashtable OTCItemsData = new Hashtable();
            OTCItemsData.Add("COMMAND", "OTCITEMDESC" /*Show RX INFO Remaining*/);
            //Aries8
            if (_deviceType == DeviceType.PAX_D200)
            {
                OTCItemsData.Add("TITLE", @"OTC Items Acknowledgement");
            }
            else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
            {
                OTCItemsData.Add("TITLE", @"\1OTC Items Acknowledgement");
            }
            else
            {
                OTCItemsData.Add("TITLE", @"\2OTC Items Acknowledgement");  //PRIMEPOS - 2555 Add @"\1" for Small Font text and @"\2" for Medium Font text to display on device - NileshJ
            }
            OTCItemsData.Add("MESSAGE", @"\1" + otcList[0]);

            ArrayList OTCItemList = new ArrayList();
            for (int i = 1; i < otcList.Count; i++)
            {
                OTCItemList.Add(otcList[i]);
            }
            OTCItemsData.Add("OTCITEMS", OTCItemList);
            DeviceCommand("SHOWTEXTBOX", OTCItemsData);


        }
        //PRIMEPOS:2544 -End Added for OTC NileshJ End [14/06/2018]


        #region PAXDeviceCommandQueue 
        /// <summary>
        /// This function Queue all Commands for the Device.
        /// Each command MUST be queued in order for the device not to crash.
        /// </summary>
        /// <param n0ame="cScreenP"></param>
        /// <param name="DataP"></param>
        private void DeviceCommand(string MessageID, Hashtable DataP)
        {
            lock (CommandQueue)
            {
                logger.Debug("\t\tQueue command");
                CommandStation CommandData = new CommandStation(MessageID, DataP);
                CommandQueue.Enqueue(CommandData); //Queue the command for the device
                logger.Debug("\t\tFinish Queue command: " + CommandQueue.Count);

                if (CommandQueue.Count > 0)
                {
                    //NileshJ - Handling Thread Exception - 20-Dec-2018
                    bool isException = false;
                    lock (exceptionObj)
                    {
                        isException = this.isException;
                    }
                    //
                    if (isException || (DeviceThread == null ? true : !DeviceThread.IsAlive))//NileshJ - Handling Thread Exception - Add isException - 20-Dec-2018
                    {
                        if (DeviceThread != null)
                        {
                            if (!isException)//NileshJ - 13-Dec-2018
                            {
                                DeviceThread.Abort();
                            }

                            DeviceThread = null;
                        }
                        lock (exceptionObj)
                        {
                            isException = false;
                            this.isException = false;
                        } // NileshJ - Lock Exception - 20-Dec-2018

                        /* If the thread is not alive start a new thread. */
                        DeviceThread = new Thread(ExecuteDeviceCommnads);
                        DeviceThread.Start();
                    }
                }
                logger.Debug("\t\tExiting DeviceCommand");
            }
        }

        private void ExecuteDeviceCommnads()
        {
            logger.Debug("\t\t==>About to Set Data on Device");
            CommandStation command = null; // NileshJ - Add -20-Dec-2018
            try
            {
                //This is to make sure no other data is being pass from any other method while in here
                lock (DeviceLock)
                {
                    while (CommandQueue.Count > 0)
                    {
                        if (!Constant.IsInMethod)
                        {
                            Constant.IsInMethod = true; //set condition so it wait until everything finish before going in the loop again
                            Constant.IsStillWrite = true; //make sure this is set so POS can know that device is still writing to the device
                            bool isCommand = false;
                            // CommandStation command = null; // NileshJ - Commented - 20-Dec-2018
                            command = null; // NileshJ  20-Dec-2018
                            logger.Debug("\t\tAbout to dequeue command from the Queue to the pad, Count: " + CommandQueue.Count);
                            try
                            {
                                command = CommandQueue.Dequeue(); //Deueue the Queue data                             
                                isCommand = true;
                            }
                            catch
                            {
                                isCommand = false; // if dequeue fail set this to false.
                            }

                            if (!isCommand)
                            {
                                Constant.IsInMethod = false;
                                Constant.IsStillWrite = false;
                                ErrorLog += PAX_ErrorLog; //Event for Error. This will call the Thread again
                                ErrorLog.Invoke();

                                lock (exceptionObj)
                                {
                                    isException = true;
                                } // NileshJ - Lock Exception - 20-Dec-2018
                                return;
                            }
                            string cScreen = string.Empty;

                            if (PData != null)
                                PData.Clear();
                            else
                                PData = new Hashtable();

                            PData = command.DeviceData as Hashtable; //data that was dequeue
                            cScreen = command.DeviceScreen;//data that was dequeue

                            if (!string.IsNullOrEmpty(cScreen) && PData != null)
                            {
                                Constant.CurrentDequeueScreen = string.Empty;
                                Constant.CurrentDequeueData = new Hashtable();
                                Constant.CurrentDequeueScreen = cScreen;
                                Constant.CurrentDequeueData = PData;
                            }

                            logger.Debug("\t\tEntering PAX ExecuteCommand ");


                            switch (cScreen.ToUpper().Trim())
                            {
                                case "INIT":
                                    logger.Debug("\t\t\t[Init] Begin");
                                    deviceResponse = device.Initialize();
                                    logger.Trace($"Device Details:{JsonConvert.SerializeObject(deviceResponse)}");//PRIMEPOS-3090
                                    logger.Debug("\t\t\t[Init] Finish");
                                    break;

                                case "WELCOME":
                                    logger.Debug("\t\t\t[WELCOME] Begin");
                                    string title = string.Empty;
                                    string displayMessage1 = string.Empty;
                                    string displayMessage2 = string.Empty;
                                    string timeout = "30";
                                    //Aries8
                                    if (_deviceType == DeviceType.PAX_D200 || _deviceType == DeviceType.PAX_A920) //PRIMEPOS-3146
                                    {
                                        title = "Hello there ";
                                        displayMessage1 = "Welcome to Test Pharmacy";
                                        //displayMessage2 = @"This is test Welcome Message";
                                    }
                                    else
                                    {
                                        title = @"\2Hello there ";
                                        displayMessage1 = @"\2Welcome to Test Pharmacy";
                                        //displayMessage2 = @"\2This is test Welcome Message";
                                    }
                                    deviceResponse = device.ShowWelcomeMessage(title: title,
                                        displayMessage1: displayMessage1,
                                        displayMessage2: displayMessage2,
                                        timeout: timeout);
                                    logger.Debug("\t\t\t[WELCOME] Finish");
                                    break;
                                case "GETVAR":
                                    logger.Debug("\t\t\t[GETVAR] Begin");
                                    logger.Debug("\t\t\t[GETVAR] Finish");
                                    break;
                                case "SETVAR":
                                    break;
                                case "SHOWDIALOG":
                                    string titleDialog = (string)PData["TITLE"];
                                    string button1Dialog = (string)PData["BUTTON1"];
                                    string button2Dialog = (string)PData["BUTTON2"];
                                    string button3Dialog = (string)PData["BUTTON3"];
                                    string button4Dialog = (string)PData["BUTTON4"];
                                    string screenCommandDialog = (string)PData["COMMAND"];

                                    deviceResponse = device.ShowDialog(title: titleDialog, button1: button1Dialog, button2: button2Dialog, button3: button3Dialog, button4: button4Dialog);

                                    if (screenCommandDialog == "PATCONSENTSELECTPAT" && deviceResponse.ButtonNumber != null && deviceResponse.ButtonNumber != "")
                                    {
                                        PattypeSelection = int.Parse(deviceResponse.ButtonNumber);
                                    }
                                    else
                                    {
                                        PattypeSelection = 0;
                                    }
                                    break;
                                case "GETSIGNATURE":
                                    deviceResponse = device.GetSignature();
                                    //if (Convert.ToInt64(deviceResponse.ResponseLength) > 0 && deviceResponse.SignatureData != null && deviceResponse.SignatureData.Length > 0)//Commented by Amit on 02 Dec 2020
                                    if (Convert.ToInt64(deviceResponse.ResponseLength) > 0 && deviceResponse.Signature_Data != null && deviceResponse.Signature_Data.Length > 0)
                                    {
                                        //GetSignatureString = deviceResponse.SignatureData;//Commented by Amit on 02 Dec 2020
                                        GetSignatureString = Convert.ToString(deviceResponse.Signature_Data);//Added by Amit on 02 Dec 2020
                                    }
                                    break;
                                case "SHOWMESSAGE":

                                    logger.Debug("\t\t\t[SHOWMESSAGE] Begin");

                                    string screenCommand = (string)PData["COMMAND"];
                                    string itemAction = (string)PData["ACTION"];
                                    if (screenCommand == "SHOWRXLIST")
                                    {
                                        if (!PData.ContainsKey("ItemName"))
                                        {
                                            device.ShowItems(null, "", _deviceType);
                                        }
                                        else
                                        {
                                            string itemName = ((string)PData["ItemName"]).Replace(@"\1", ""); //Amit //PRIMEPOS-2931                                            
                                            string placeHolderName = ((string)PData["ItemName"]).Replace(@"\1", ""); //Amit//PRIMEPOS-2931
                                            string showRXDesc = (string)PData["SHOWRXDESCRIPTION"];
                                            string isRX = (string)PData["ISRX"];
                                            string itemOty = (string)PData["ItemQty"];
                                            string unitPrice = (string)PData["UnitPrice"];
                                            string totalPrice = (string)PData["TotalPrice"];
                                            string withDiscAmout = (string)PData["WITHDISCAMOUNT"];
                                            string discount = (string)PData["DISCOUNT"];
                                            string index = (string)PData["INDEX"];//PRIMEPOS-2931
                                            //SURAJ ShowRXDESc
                                            if (isRX == "1")
                                            {
                                                string[] strArray = itemName.Split('-');
                                                if (strArray.Length > 0 && Convert.ToBoolean(showRXDesc) == false)
                                                {
                                                    placeHolderName = string.Format("{0,-10}", strArray[0] + "-****");
                                                }
                                                else
                                                {
                                                    placeHolderName = string.Format("{0,-10}", string.IsNullOrEmpty(PData["ItemName"].ToString().Trim()) ? "Not Available" : itemName);
                                                }
                                            }
                                            else
                                            {
                                                placeHolderName = string.Format("{0,-10}", string.IsNullOrEmpty(PData["ItemName"].ToString().Trim()) ? "Not Available" : itemName);
                                            }

                                            Order mOrder = new Order();
                                            mOrder.deviceType = _deviceType;//Aries8

                                            if (_deviceType == DeviceType.PAX_D200)
                                            {
                                                //PRIMEPOS-2931
                                                if (itemAction == "A")
                                                {
                                                    var item = deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()
                                                                                && p.quantity.contentText.ToString().Trim() == itemOty.ToString().Trim()
                                                                                && p.price.contentText.ToString().Trim() == unitPrice.ToString().Trim()
                                                                                && p.discount.contentText.ToString().Trim() == discount.ToString().Trim()
                                                                                ).SingleOrDefault();//Amit
                                                    if (deviceData.Contains(item))
                                                    {
                                                        //update item
                                                        mOrder = deviceData[deviceData.IndexOf(item)];
                                                        mOrder.name.contentText = placeHolderName;
                                                        mOrder.quantity.contentText = itemOty;
                                                        mOrder.price.contentText = unitPrice;
                                                        mOrder.discount.contentText = discount;
                                                        mOrder.total.contentText = totalPrice;
                                                        index = Convert.ToString(mOrder.Index);
                                                    }
                                                    else
                                                    {
                                                        //Add item
                                                        mOrder.name = new Descreption(placeHolderName);
                                                        mOrder.quantity = new Quantity(itemOty);
                                                        mOrder.price = new UnitPrice(unitPrice);
                                                        mOrder.discount = new Discount(discount);
                                                        mOrder.total = new Total(totalPrice);
                                                        mOrder.Index = Convert.ToInt32(deviceData.Count());
                                                        deviceData.Add(mOrder);
                                                        index = Convert.ToString(mOrder.Index);
                                                    }
                                                }
                                                else if (itemAction == "U")
                                                {
                                                    Order items = null;
                                                    if (deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()).Count() == 1)
                                                    {
                                                        items = deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()).SingleOrDefault();//Amit
                                                    }
                                                    else
                                                    {
                                                        items = deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()
                                                        && p.Index.ToString().Trim() == index.ToString().Trim()).FirstOrDefault();//Amit
                                                    }
                                                    if (deviceData.Contains(items))
                                                    {
                                                        //update item
                                                        mOrder = deviceData[deviceData.IndexOf(items)];
                                                        mOrder.name.contentText = placeHolderName;
                                                        mOrder.quantity.contentText = itemOty;
                                                        mOrder.price.contentText = unitPrice;
                                                        mOrder.discount.contentText = discount;
                                                        mOrder.total.contentText = totalPrice;
                                                        index = Convert.ToString(mOrder.Index);
                                                    }

                                                }

                                                device.ShowItemsDisplay(deviceData.ToArray(), itemAction, _deviceType, Convert.ToInt32(index));
                                                logger.Debug("\t\t\t[SHOWMESSAGE] Finish");
                                            }
                                            #region PRIMEPOS-3146
                                            else if (_deviceType == DeviceType.PAX_A920)
                                            {
                                                if (itemAction == "A")
                                                {
                                                    var item = deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()
                                                                                && p.quantity.contentText.ToString().Trim() == itemOty.ToString().Trim()
                                                                                && p.price.contentText.ToString().Trim() == unitPrice.ToString().Trim()
                                                                                && p.discount.contentText.ToString().Trim() == discount.ToString().Trim()
                                                                                ).SingleOrDefault();//Amit
                                                    if (deviceData.Contains(item))
                                                    {
                                                        //update item
                                                        mOrder = deviceData[deviceData.IndexOf(item)];
                                                        mOrder.name.contentText = placeHolderName;
                                                        mOrder.quantity.contentText = itemOty;
                                                        mOrder.price.contentText = unitPrice;
                                                        mOrder.discount.contentText = discount;
                                                        mOrder.total.contentText = totalPrice;
                                                        index = Convert.ToString(mOrder.Index);
                                                    }
                                                    else
                                                    {
                                                        //Add item
                                                        mOrder.name = new Descreption(placeHolderName);
                                                        mOrder.quantity = new Quantity(itemOty);
                                                        mOrder.price = new UnitPrice(unitPrice);
                                                        mOrder.discount = new Discount(discount);
                                                        mOrder.total = new Total(totalPrice);
                                                        mOrder.Index = Convert.ToInt32(deviceData.Count());
                                                        deviceData.Add(mOrder);
                                                        index = Convert.ToString(mOrder.Index);
                                                    }
                                                }
                                                else if (itemAction == "U")
                                                {
                                                    Order items = null;
                                                    //if (deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()).Count() == 1)
                                                    //{
                                                    //    items = deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()).SingleOrDefault();
                                                    //}
                                                    //else
                                                    //{
                                                    //    items = deviceData.Where(p => p.name.contentText.ToString().Trim() == itemName.ToString().Trim()
                                                    //    && p.Index.ToString().Trim() == index.ToString().Trim()).FirstOrDefault();//Amit
                                                    //}
                                                    items = deviceData[Convert.ToInt32(index)];
                                                    if (deviceData.Contains(items))
                                                    {
                                                        //update item
                                                        mOrder = deviceData[deviceData.IndexOf(items)];
                                                        mOrder.name.contentText = placeHolderName;
                                                        mOrder.quantity.contentText = itemOty;
                                                        mOrder.price.contentText = unitPrice;
                                                        mOrder.discount.contentText = discount;
                                                        mOrder.total.contentText = totalPrice;
                                                        index = Convert.ToString(mOrder.Index);
                                                    }

                                                }
                                                device.ShowItemsDisplay(deviceData.ToArray(), itemAction, _deviceType, Convert.ToInt32(index));
                                                logger.Debug("\t\t\t[SHOWMESSAGE] Finish");
                                            }
                                            #endregion 
                                            else
                                            {
                                                //PRIMEPOS-2931
                                                if (itemAction == "A")
                                                {
                                                    var item = deviceData.Where(p => p.name.contentText == itemName.ToString().Trim()
                                                                                && p.quantity.contentText == itemOty.ToString().Trim()
                                                                                && p.price.contentText == unitPrice.ToString().Trim()
                                                                                && p.discount.contentText == discount.ToString().Trim()
                                                                                ).SingleOrDefault();//Amit
                                                    if (deviceData.Contains(item))
                                                    {
                                                        //update item
                                                        mOrder = deviceData[deviceData.IndexOf(item)];
                                                        mOrder.name.contentText = placeHolderName;
                                                        mOrder.quantity.contentText = itemOty;
                                                        mOrder.price.contentText = unitPrice;
                                                        mOrder.discount.contentText = discount;
                                                        mOrder.total.contentText = totalPrice;
                                                    }
                                                    else
                                                    {
                                                        //Add item
                                                        mOrder.name = new Descreption(placeHolderName);
                                                        mOrder.quantity = new Quantity(itemOty);
                                                        mOrder.price = new UnitPrice(unitPrice);
                                                        mOrder.discount = new Discount(discount);
                                                        mOrder.total = new Total(totalPrice);
                                                        deviceData.Add(mOrder);
                                                    }
                                                }
                                                else if (itemAction == "U")
                                                {

                                                    var items = deviceData[Convert.ToInt32(index)];
                                                    if (deviceData.Contains(items))
                                                    {
                                                        //update item
                                                        mOrder = deviceData[deviceData.IndexOf(items)];
                                                        mOrder.name.contentText = placeHolderName;
                                                        mOrder.quantity.contentText = itemOty;
                                                        mOrder.price.contentText = unitPrice;
                                                        mOrder.discount.contentText = discount;
                                                        mOrder.total.contentText = totalPrice;
                                                    }
                                                }
                                                device.ShowItems(deviceData.ToArray(), itemAction, _deviceType);
                                                logger.Debug("\t\t\t[SHOWMESSAGE] Finish");
                                            }
                                        }
                                        deviceDataTemp = deviceData;//PRIMEPOS-3146
                                    }
                                    else if (screenCommand == "DELETEITEM")
                                    {
                                        logger.Debug("\t\t\t[DELETEITEM] Begin");
                                        if (deviceData.Count > 0)
                                        {
                                            Order itemNameString = null;
                                            if (_deviceType == DeviceType.PAX_D200)
                                            {
                                                string ItemDescription = (string)PData["ItemName"];
                                                string quantity = (string)PData["ItemQty"];
                                                string price = (string)PData["UnitPrice"];
                                                string discount = (string)PData["Discount"];
                                                Order mOrder = null;
                                                try
                                                {
                                                    itemNameString = deviceData.Where(p => p.name.contentText.ToString().Trim() == ItemDescription.ToString().Trim()
                                                                                && p.quantity.contentText.ToString().Trim() == quantity.ToString().Trim()
                                                                                && p.price.contentText.ToString().Trim() == price.ToString().Trim()
                                                                                && p.discount.contentText.ToString().Trim() == discount.ToString().Trim()
                                                        ).Single();
                                                }
                                                catch (Exception ex)
                                                {
                                                    itemNameString = null;
                                                }
                                                if (itemNameString != null)
                                                {
                                                    int index = deviceData.IndexOf(itemNameString);
                                                    if (deviceData.Contains(itemNameString))
                                                    {
                                                        //update item
                                                        mOrder = deviceData[deviceData.IndexOf(itemNameString)];
                                                    }
                                                    device.ShowItemsDisplay(deviceData.ToArray(), "D", _deviceType, index);
                                                }
                                            }
                                            #region PRIMEPOS-3146
                                            else if (_deviceType == DeviceType.PAX_A920)
                                            {
                                                string ItemDescription = (string)PData["ItemName"];
                                                string quantity = (string)PData["ItemQty"];
                                                string price = (string)PData["UnitPrice"];
                                                string discount = (string)PData["Discount"];
                                                Order mOrder = null;
                                                try
                                                {
                                                    itemNameString = deviceData.Where(p => p.name.contentText.ToString().Trim() == ItemDescription.ToString().Trim()
                                                                                && p.quantity.contentText.ToString().Trim() == quantity.ToString().Trim()
                                                                                && p.price.contentText.ToString().Trim() == price.ToString().Trim()
                                                                                && p.discount.contentText.ToString().Trim() == discount.ToString().Trim()
                                                        ).Single();
                                                }
                                                catch (Exception)
                                                {
                                                    itemNameString = null;
                                                }
                                                if (itemNameString != null)
                                                {
                                                    int index = deviceData.IndexOf(itemNameString);
                                                    if (deviceData.Contains(itemNameString))
                                                    {
                                                        //update item
                                                        mOrder = deviceData[deviceData.IndexOf(itemNameString)];
                                                    }
                                                    device.ShowItemsDisplay(deviceData.ToArray(), "D", _deviceType, index);
                                                }
                                            }
                                            #endregion
                                            else
                                            {
                                                string sCurrentTransID = (string)PData["CurrentTransID"];

                                                int nRowID = (int)PData["RowID"];
                                                Order dOrder = deviceData[nRowID];

                                                try
                                                {
                                                    itemNameString = deviceData.Where(p => p.name.contentText == dOrder.name.contentText
                                                                            && p.quantity.contentText == dOrder.quantity.ToString().Trim()
                                                                            && p.price.contentText == dOrder.price.ToString().Trim()
                                                                            && p.discount.contentText == dOrder.discount.ToString().Trim()
                                                    ).Single();//PRIMEPOS-2931
                                                }
                                                catch (Exception ex)
                                                {
                                                    itemNameString = null;
                                                }

                                                bool isremovedListItem = deviceData.Remove(itemNameString);
                                                logger.Debug("\t\t\t[DELETEITEM]  " + isremovedListItem);
                                                device.ShowItems(deviceData.ToArray(), itemAction, _deviceType);
                                            }

                                        }
                                        deviceDataTemp = deviceData;//PRIMEPOS-3146
                                        logger.Debug("\t\t\t[DELETEITEM] Finish");
                                    }
                                    else if (screenCommand == "RELOADITEMLIST")
                                    {
                                        if (_deviceType == DeviceType.PAX_D200 || _deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                        {
                                            device.ClearMessageScreen();
                                            device.ShowItemsDisplay(deviceDataTemp.ToArray(), "", _deviceType, 0);
                                        }
                                        else if (_deviceType == DeviceType.PAX_PX7)
                                        {
                                            device.ClearMessageScreen();
                                            device.ShowItems(deviceDataTemp.ToArray(), "", _deviceType);
                                        }
                                        else
                                        {
                                            device.ClearMessageScreen();
                                            device.ShowItems(deviceData.ToArray(), "", _deviceType);
                                        }

                                    }
                                    else if (screenCommand == "HOLDITEMS")
                                    {
                                        ArrayList onholdItems = (ArrayList)PData["OnHoldItems"];
                                        for (int i = 0; i < onholdItems.Count; i++)
                                        {
                                            Hashtable item = (Hashtable)onholdItems[i];
                                            string itemName = ((string)item["ItemName"]).Replace(@"\1", ""); //Amit;//PRIMEPOS-2931
                                            string placeHolderName = ((string)item["ItemName"]).Replace(@"\1", ""); //Amit;//PRIMEPOS-2931
                                            string showRXDesc = (string)item["SHOWRXDESCRIPTION"];
                                            string isRX = ((int)item["ISRX"] > 0).ToString();
                                            string itemOty = (string)item["ItemQty"];
                                            string unitPrice = (string)item["UnitPrice"];
                                            string totalPrice = (string)item["TotalPrice"];
                                            string withDiscAmout = (string)item["WITHDISCAMOUNT"];
                                            string discount = (string)item["DISCOUNT"];


                                            //SURAJ ShowRXDESc
                                            if (isRX == "True")
                                            {
                                                string[] strArray = itemName.Split('-');
                                                if (strArray.Length > 0 && Convert.ToBoolean(showRXDesc) == false)
                                                {
                                                    placeHolderName = string.Format("{0,-10}", strArray[0] + "-****");
                                                }
                                                else
                                                {
                                                    placeHolderName = string.Format("{0,-10}", string.IsNullOrEmpty(item["ItemName"].ToString().Trim()) ? "Not Available" : itemName);
                                                }
                                            }
                                            else
                                            {
                                                placeHolderName = string.Format("{0,-10}", string.IsNullOrEmpty(item["ItemName"].ToString().Trim()) ? "Not Available" : itemName);
                                            }


                                            Order mOrder = new Order();
                                            mOrder.deviceType = _deviceType;//Aries8
                                            mOrder.name = new Descreption(placeHolderName);
                                            mOrder.quantity = new Quantity(itemOty);
                                            mOrder.price = new UnitPrice(unitPrice);
                                            mOrder.discount = new Discount(discount);
                                            mOrder.total = new Total(totalPrice);
                                            deviceData.Add(mOrder);
                                        }
                                        if (_deviceType == DeviceType.PAX_D200 || _deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                        {
                                            device.ShowItemsDisplay(deviceData.ToArray(), "", _deviceType, 0);
                                        }
                                        else
                                        {
                                            device.ShowItems(deviceData.ToArray(), "", _deviceType);
                                        }
                                        deviceDataTemp = deviceData;//PRIMEPOS-3146
                                        logger.Debug("\t\t\t[SHOWMESSAGE] Finish");
                                    }
                                    else
                                    {
                                        string _CustomTitle = (string)PData["TITLE"];
                                        string _CustomMessage = (string)PData["MESSAGE"];
                                        //Aries8
                                        if (_deviceType == DeviceType.PAX_D200)
                                        {
                                            device.ShowMessage(_CustomTitle, _CustomMessage);
                                            Thread.Sleep(100);//PRIMEPOS-3239
                                        }
                                        else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                        {
                                            string firstPart = string.Empty;
                                            string secondPart = string.Empty;
                                            int firstCommaIndex = 0;
                                            if (_CustomMessage.Contains("Total"))
                                            {
                                                firstCommaIndex = _CustomMessage.IndexOf("\n");
                                                firstPart = _CustomMessage.Substring(0, firstCommaIndex);
                                                secondPart = "Processing Payment...";
                                            }
                                            else
                                            {
                                                firstCommaIndex = _CustomMessage.IndexOf('.');
                                                firstPart = _CustomMessage.Substring(0, firstCommaIndex);
                                                secondPart = _CustomMessage.Substring(firstCommaIndex + 1);
                                            }
                                            device.ShowTwoLineMessage(@"\1\L" + _CustomTitle, @"\1\L" + firstPart, @"\1\L" + secondPart);
                                            //device.ShowMessage(_CustomTitle, _CustomMessage);
                                        }
                                        else
                                        {
                                            device.ShowMessage(@"\2" + _CustomTitle, @"\1" + _CustomMessage);    // PRIMEPOS - 2555 Add @"\1" for Small Font text and @"\2" for Medium Font text to display on device - NileshJ
                                        }
                                        logger.Debug("\t\t\t[SHOWMESSAGE] Finish");
                                    }
                                    break;
                                case "CLEARMESSAGE":
                                    device.ClearMessageScreen();
                                    break;
                                case "RESET":
                                    deviceData.Clear();
                                    deviceResponse = device.Reset();
                                    break;
                                case "UPDATERESOURCEFILE":
                                    byte[] file = (byte[])PData["FILE"];
                                    deviceResponse = device.UpdateResourceImageNew(file);
                                    if (deviceResponse.DeviceResponseCode == "000000")
                                    {
                                        IsResourceUpdated = true;
                                    }

                                    break;
                                case "DOSIGNATURE":
                                    deviceResponse = device.DoSignature();
                                    if (deviceResponse.DeviceResponseText == "OK")
                                    {
                                        IsSignValid = true;
                                    }
                                    else
                                    {
                                        IsSignValid = false;
                                    }
                                    break;
                                case "DELETEIMAGE":
                                    break;
                                case "SHOWTHANKYOU":
                                    // PRIMEPOS - 2555 Add  @"\1" for Small Font text and @"\2" for Medium Font text to display on device - NileshJ
                                    string thanksTitle = string.Empty;
                                    string thanksMessage1 = string.Empty;
                                    string thanksMessage2 = string.Empty;
                                    string thanksTimeout = "50";
                                    //Aries8
                                    if (_deviceType == DeviceType.PAX_D200)
                                    {
                                        thanksTitle = "Thank You";
                                        thanksMessage1 = "Payment of " + PData["AMT"] + "$ Successfully done";
                                        thanksMessage2 = "--";
                                    }
                                    else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                    {
                                        thanksTitle = @"\1" + "Thank You";
                                        thanksMessage1 = @"\1" + "Payment of " + PData["AMT"] + "$";
                                        thanksMessage2 = @"\1" + " Successfully done";
                                    }
                                    else
                                    {
                                        thanksTitle = @"\2" + "Thank You";
                                        thanksMessage1 = @"\1" + "Payment of " + PData["AMT"] + "$ Successfully done";
                                        thanksMessage2 = @"\1" + "--";
                                    }
                                    deviceResponse = device.ShowThanksMessage(title: thanksTitle,
                                        displayMessage1: thanksMessage1,
                                        displayMessage2: thanksMessage2,
                                        timeout: thanksTimeout);
                                    break;
                                case "REBOOT":
                                    deviceResponse = device.Reboot();
                                    break;
                                case "GETPINBLOCK":
                                    break;
                                case "INPUTACCOUNT":
                                    break;
                                case "RESETMSR":
                                    break;
                                case "INPUTTEXT":
                                    break;
                                case "CHECKFILE":
                                    break;
                                case "AUTHORIZECARD":
                                    break;
                                case "COMPLETEONLINEEMV":
                                    break;
                                case "REMOVECARD":
                                    break;
                                case "GETEMVTLVDATA":
                                    break;
                                case "SETEMVTLVDATA":
                                    break;
                                case "INPUTACCOUNTWITHEMV":
                                    break;
                                case "COMPLETECONTACTLESSEMV":
                                    break;
                                case "SETSAFPARAMETERS":
                                    break;
                                case "SHOWTEXTBOX":
                                    //Commnand
                                    logger.Debug("\t\t\t[SHOWTEXTBOX] Begin");
                                    string screenDisplay = (string)PData["COMMAND"];
                                    string titleText = (string)PData["TITLE"];

                                    string patName = (string)PData["PATIENTNAME"];
                                    if (screenDisplay == "HIPPAACK")
                                    {
                                        string message = (string)PData["MESSAGE"];
                                        string patAdd = (string)PData["PATIENTADDRESS"];
                                        string alteredMsg = String.Empty;
                                        string button1 = "YES";
                                        string button2 = "NO";
                                        //Aries8
                                        if (_deviceType == DeviceType.PAX_D200)//PRIMEPOS-3146
                                        {
                                            titleText = titleText.Replace("\\2", "").Replace(@"\2", "");
                                            alteredMsg = message + System.Environment.NewLine + System.Environment.NewLine + "\\C\\BProceed?";
                                        }
                                        else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                        {
                                            titleText = titleText.Replace("\\2", "").Replace(@"\2", "");
                                            alteredMsg = message + System.Environment.NewLine + System.Environment.NewLine + "\\C\\BProceed?";
                                            button1 = "\\1" + button1;
                                            button2 = "\\1" + button2;
                                        }
                                        else
                                        {
                                            alteredMsg = @"\1" + message + System.Environment.NewLine + System.Environment.NewLine + "\\C\\BProceed?"; // PRIMEPOS - 2555 Add  @"\1" for Small Font text to display on device - NileshJ
                                        }
                                        deviceResponse = device.ShowTextBox(titleText, alteredMsg, button1, "00ff00", button2, "ff0000", null, null);
                                        HIPPAAckResultId = deviceResponse.ButtonNumber;
                                    }
                                    else if (screenDisplay == "RXHEADERINFO")
                                    {
                                        string RXCount = (string)PData["RXCOUNT"];
                                        string RXData = (string)PData["RXDATA"];
                                        bool IsPatConselHide = (bool)PData["ISPATCONSELHIDE"]; //PRIMEPOS-3245
                                        var patientName = patName;
                                        string alteredMsg = string.Empty; //PRIMEPOS-3245
                                        if (patName.Length > 35)
                                        {
                                            patientName = patName.Substring(0, 34);
                                        }
                                        if (!IsPatConselHide) //PRIMEPOS-3245
                                        {
                                            alteredMsg = "\\BPatient : " + patientName.Trim() +
                                         "\\R\\BRxCount : " + RXCount + System.Environment.NewLine +
                                         "________________________________________" + System.Environment.NewLine +
                                       "\\BRxInfo :" + System.Environment.NewLine + RXData + System.Environment.NewLine +
                                        "________________________________________" +
                                         System.Environment.NewLine + System.Environment.NewLine +
                                         "\\C\\BPatient Counseling ? ";//Append original msg to altered one
                                        }
                                        else
                                        {
                                            alteredMsg = "\\BPatient : " + patientName.Trim() +
                                             "\\R\\BRxCount : " + RXCount + System.Environment.NewLine +
                                             "________________________________________" + System.Environment.NewLine +
                                           "\\BRxInfo :" + System.Environment.NewLine + RXData + System.Environment.NewLine +
                                            "________________________________________"; //PRIMEPOS-3245
                                        }
                                        //Aries8
                                        if (_deviceType == DeviceType.PAX_D200)
                                        {
                                            if (!IsPatConselHide)//PRIMEPOS-3245
                                            {
                                                deviceResponse = device.ShowTextBox(titleText.Replace("\\2", ""), alteredMsg, "YES", "00ff00", "NO", "ff0000", null, null); //PRIMEPOS - 2555 Add  @"\1" for Small Font text to display on device - NileshJ
                                            }
                                            else
                                            {
                                                deviceResponse = device.ShowTextBox(titleText.Replace("\\2", ""), alteredMsg, "PROCEED", "00ff00", null, null, null, null); //PRIMEPOS-3245
                                            }
                                        }
                                        else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                        {
                                            if (!IsPatConselHide)//PRIMEPOS-3245
                                            {
                                                deviceResponse = device.ShowTextBox(titleText.Replace("\\2", "\\1"), @"\1" + alteredMsg, "\\1\\CYES", "00ff00", "\\1NO", "ff0000", null, null);
                                            }
                                            else
                                            {
                                                deviceResponse = device.ShowTextBox(titleText.Replace("\\2", "\\1"), @"\1" + alteredMsg, "\\1\\CPROCEED", "00ff00", null, null, null, null);
                                            }
                                        }
                                        else
                                        {
                                            if (!IsPatConselHide)//PRIMEPOS-3245
                                            {
                                                deviceResponse = device.ShowTextBox(titleText, @"\1" + alteredMsg, "YES", "00ff00", "NO", "ff0000", null, null); //PRIMEPOS - 2555 Add  @"\1" for Small Font text to display on device - NileshJ
                                            }
                                            else
                                            {
                                                deviceResponse = device.ShowTextBox(titleText, @"\1" + alteredMsg, "PROCEED", "00ff00", null, null, null, null);
                                            }
                                        }
                                        PatCounsel = deviceResponse.ButtonNumber == "1" ? Constant.PatientCounselYes : Constant.PatientCounselNo;
                                    }
                                    else if (screenDisplay == "HEALTHIXFRM1")
                                    {
                                        string message = (string)PData["MESSAGE"];
                                        if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                        {
                                            deviceResponse = device.ShowTextBox(titleText, message, "\\1Next", "f8f606", "", "", null, null);
                                        }
                                        else
                                        {
                                            deviceResponse = device.ShowTextBox(titleText, message, "Next", "f8f606", "", "", null, null);
                                        }
                                        if (!string.IsNullOrEmpty(deviceResponse.ButtonNumber))
                                        {
                                            shouldMoveNext = true;
                                        }
                                        else
                                        {
                                            shouldMoveNext = false;
                                        }
                                    }
                                    else if (screenDisplay == "HEALTHIXFRM2")
                                    {
                                        string pharmacy = (string)PData["PHARMACYNAME"];
                                        string message = (string)PData["MESSAGE"];
                                        //Aries8
                                        if (_deviceType == DeviceType.PAX_D200)
                                        {
                                            deviceResponse = device.ShowTextBox(titleText, message, @"All Healthix Participants", "00ff00", pharmacy, "f8f606", "Skip", "ff0000"); //PRIMEPOS - 2555 Add  @"\1" for Small Font text to display on device -
                                        }
                                        else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                        {
                                            //deviceResponse = device.ShowTextBox(titleText, message, @"\1All Healthix Participants", "00ff00", @"\1" + pharmacy, "f8f606", @"\1Skip", "ff0000");
                                            deviceResponse = device.ShowTextBox(titleText, message, @"\1Next", "00ff00", "", "", "", "");

                                            if (!string.IsNullOrEmpty(deviceResponse.ButtonNumber))
                                            {
                                                deviceResponse = device.ShowDialogForm(title: titleText, Label1: "All Healthix Participants", Label1Property: 0, Label2: pharmacy, Label2Property: 0, Label3: "Skip", Label3Property: 0, Label4: "", Label4Property: 0, buttonType: 1);
                                            }

                                        }
                                        else
                                        {
                                            deviceResponse = device.ShowTextBox(titleText, message, @"\1All Healthix Participants", "00ff00", @"\1" + pharmacy, "f8f606", @"\1Skip", "ff0000"); //PRIMEPOS - 2555 Add  @"\1" for Small Font text to display on device -
                                        }

                                        if (!string.IsNullOrEmpty(deviceResponse.ButtonNumber))
                                        {
                                            ConsentApplies = int.Parse(deviceResponse.ButtonNumber);
                                            if (ConsentApplies == 3)//Arvind & Nilesh added for Skip Healthix part
                                            {
                                                bSkipConsent = true;
                                                tmpConsent.IsConsentSkip = true;
                                            }
                                        }
                                        else
                                        {
                                            ConsentApplies = 2;
                                        }
                                    }
                                    else if (screenDisplay == "HEALTHIXFRM3")
                                    {
                                        if (_deviceType == DeviceType.PAX_A920)
                                        {


                                            deviceResponse = device.ShowDialogForm(title: titleText, Label1: "I Give Consent", Label1Property: 0, Label2: "I Deny Consent Unless in a Medical Emergency", Label2Property: 0, Label3: "I Deny Consent", Label3Property: 0, Label4: "", Label4Property: 0, buttonType: 1);


                                            //deviceResponse = device.ShowTextBox(titleText, "", @"\1I Give Consent", "00ff00", @"\1I Deny Consent Unless in a Medical Emergency", "f8f606", @"\1I Deny Consent", "ff0000");
                                        }
                                        else
                                        {
                                            deviceResponse = device.ShowTextBox(titleText, "", "I Give Consent", "00ff00", "I Deny Consent Unless in a Medical Emergency", "f8f606", "I Deny Consent", "ff0000");
                                        }
                                        if (!string.IsNullOrEmpty(deviceResponse.ButtonNumber))
                                        {
                                            ConsentStatus = int.Parse(deviceResponse.ButtonNumber);
                                        }
                                        else
                                        {
                                            ConsentStatus = 2;
                                        }
                                    }
                                    else if (screenDisplay == "OTCITEMDESC")
                                    {
                                        string message = (string)PData["MESSAGE"];
                                        ArrayList OTCItems = (ArrayList)PData["OTCITEMS"];
                                        string OTCItemsStr = "";
                                        for (int i = 0; i < OTCItems.Count; i++)
                                        {
                                            OTCItemsStr += OTCItems[i] + System.Environment.NewLine;
                                        }
                                        string alteredMsg = System.Environment.NewLine + message + System.Environment.NewLine + System.Environment.NewLine +
                                            System.Environment.NewLine + System.Environment.NewLine +
                                       "________________________________________" + System.Environment.NewLine +
                                     "\\BItems :" + System.Environment.NewLine + OTCItemsStr + System.Environment.NewLine +
                                      "________________________________________";
                                        if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
                                        {
                                            deviceResponse = device.ShowTextBox(titleText, alteredMsg, "\\1Proceed", "00ff00", "", "", "", "");
                                        }
                                        else
                                        {
                                            deviceResponse = device.ShowTextBox(titleText, alteredMsg, "Proceed", "00ff00", "", "", "", "");
                                        }
                                        if (!string.IsNullOrEmpty(deviceResponse.ButtonNumber))
                                        {
                                            shouldMoveNext = true;
                                        }
                                        else
                                        {
                                            shouldMoveNext = false;
                                        }
                                    }
                                    logger.Debug("\t\t\t[SHOWTEXTBOX] Finish");
                                    break;
                                case "REPRINT":
                                    break;
                                case "PRINTER":
                                    break;
                                case "SHOWITEM":
                                    break;
                                case "CARDINSERTDETECTION":
                                    break;
                                case "TOKENADMINISTRATIVE":
                                    break;
                                case "SHOWDIALOGFORM": //PRIMEPOS - 2555  Added for Radio Button NileshJ
                                    string titleDialogFORM = (string)PData["TITLE"];
                                    string Label1Dialog = (string)PData["LABEL1"];
                                    string Label2Dialog = (string)PData["LABEL2"];
                                    string Label3Dialog = (string)PData["LABEL3"];
                                    string Label4Dialog = (string)PData["LABEL4"];
                                    int ButtonType = (int)PData["BUTTONTYPE"];
                                    string screenCommandDialogForm = (string)PData["COMMAND"];

                                    deviceResponse = device.ShowDialogForm(title: titleDialogFORM, Label1: Label1Dialog, Label1Property: 0, Label2: Label2Dialog, Label2Property: 0, Label3: Label3Dialog, Label3Property: 0, Label4: Label4Dialog, Label4Property: 0, buttonType: ButtonType);

                                    if (screenCommandDialogForm == "PATCONSENTSELECTPAT" && deviceResponse.ButtonNumber != null && deviceResponse.ButtonNumber != "")
                                    {
                                        PattypeSelection = int.Parse(deviceResponse.ButtonNumber);
                                    }
                                    else
                                    {
                                        PattypeSelection = 0;
                                    }
                                    break;

                                case "SHOWDYNAMICFORM": // PRIMEPOS-2869 AutoRefill
                                    string screenDisplayName = (string)PData["COMMAND"];
                                    if (screenDisplayName == "PATCONSENTTEXT")
                                    {
                                        string Title = (string)PData["TITLE"];
                                        string BodyMessage = (string)PData["MESSAGE"];
                                        string Button1Text = null;
                                        string Button1TextColor = null;
                                        string Button2Text = null;
                                        string Button2TextColor = null;
                                        string Button3Text = null;
                                        string Button3TextColor = null;

                                        if (PData.ContainsKey("Button1"))
                                        {
                                            Button1Text = (string)PData["Button1"];
                                            Button1TextColor = "00ff00";
                                        }
                                        if (PData.ContainsKey("Button2"))
                                        {
                                            Button2Text = (string)PData["Button2"];
                                            Button2TextColor = "f8f606";
                                        }
                                        //Button3Text = "Skip";
                                        //Button3TextColor = "ff0000"; //PRIMEPOS-3192
                                        //Aries8
                                        if (_deviceType == DeviceType.PAX_D200)
                                        {
                                            Title = Title.Replace("\\2", "");
                                            Button1Text = Button1Text.Replace("\\1", "");
                                            Button2Text = Button2Text.Replace("\\1", "");
                                        }
                                        else if (_deviceType == DeviceType.PAX_A920)
                                        {
                                            Title = Title.Replace("\\2", "\\1");
                                            //Button3Text = "\\1" + Button3Text; //PRIMEPOS-3192
                                        }

                                        deviceResponse = device.ShowTextBox(Title, BodyMessage, Button1Text, Button1TextColor, Button2Text, Button2TextColor, Button3Text, Button3TextColor);
                                        if (!string.IsNullOrEmpty(deviceResponse.ButtonNumber))
                                        {
                                            ConsentApplies = int.Parse(deviceResponse.ButtonNumber);
                                        }
                                        else
                                        {
                                            ConsentApplies = 2;
                                        }
                                    }
                                    else if (screenDisplayName == "PATIENTTYPE")
                                    {
                                        string Title = (string)PData["TITLE"];
                                        string Option1 = null;
                                        string Option2 = null;
                                        string Option3 = null;
                                        string Option4 = null;
                                        if (PData.ContainsKey("LABEL1"))
                                        {
                                            Option1 = (string)PData["LABEL1"];
                                        }
                                        if (PData.ContainsKey("LABEL2"))
                                        {
                                            Option2 = (string)PData["LABEL2"];
                                        }
                                        if (PData.ContainsKey("LABEL3"))
                                        {
                                            Option3 = (string)PData["LABEL3"];
                                        }
                                        if (PData.ContainsKey("LABEL4"))
                                        {
                                            Option4 = (string)PData["LABEL4"];
                                        }
                                        int RadioButtonType = (int)PData["BUTTONTYPE"];
                                        //Aries8
                                        if (_deviceType == DeviceType.PAX_D200) //PRIMEPOS-3192 Added ? to options
                                        {
                                            Title = Title.Replace("\\2", "");
                                            Option1 = Option1?.Replace("\\1", "");
                                            Option2 = Option2?.Replace("\\1", "");
                                            Option3 = Option3?.Replace("\\1", "");
                                            Option4 = Option4?.Replace("\\1", "");
                                        }
                                        else if (_deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146 //PRIMEPOS-3192 Added ? to options
                                        {
                                            Title = Title.Replace("\\2", "\\1");
                                            Option1 = Option1?.Replace("\\1", "");
                                            Option2 = Option2?.Replace("\\1", "");
                                            Option3 = Option3?.Replace("\\1", "");
                                            Option4 = Option4?.Replace("\\1", "");
                                        }
                                        deviceResponse = device.ShowDialogForm(title: Title, Label1: Option1, Label1Property: 0, Label2: Option2, Label2Property: 0, Label3: Option3, Label3Property: 0, Label4: Option4, Label4Property: 0, buttonType: RadioButtonType);

                                        if (screenDisplayName == "PATIENTTYPE" && deviceResponse.ButtonNumber != null && deviceResponse.ButtonNumber != "")
                                        {
                                            PattypeSelection = int.Parse(deviceResponse.ButtonNumber);
                                        }
                                        else
                                        {
                                            PattypeSelection = 0;
                                        }
                                    }
                                    break;
                            }
                            logger.Debug("\t\t ----> PAX ExecuteCommand finish setting");

                            Constant.IsInMethod = false;
                        }
                    }
                    Constant.IsStillWrite = false; //Now allow to start a new thread.
                }
            }
            catch (UnauthorizedAccessException uA)
            {
                logger.Error("\t\tError in PAX ExecuteCommand  \n" + uA.ToString());
                Constant.IsStillWrite = false;
                ErrorLog += PAX_ErrorLog;
                ErrorLog.Invoke();
            }
            catch (Exception ex)
            {
                logger.Debug("\t\tError in PAX ExecuteCommand  \n" + ex.ToString());
                Constant.IsStillWrite = false;
                Constant.IsInMethod = false;
                PData = null;// Arvind - 16-Sept-2019 - PRIMEPOS-2735
                ErrorLog += PAX_ErrorLog;
                ErrorLog.Invoke();
                // NileshJ - Exception Handler - 20-Dec-2018
                lock (exceptionObj)
                {
                    isException = true;
                }
                if (command != null)
                {
                    Hashtable CatchData = command.DeviceData as Hashtable;
                    #region Arvind - Exception issue - 16-Sept-2019 PRIMEPOS-2735
                    if (command.DeviceScreen == "SHOWMESSAGE")
                    {
                        Hashtable fields = new Hashtable();
                        if (CatchData["COMMAND"].ToString() == "SHOWRXLIST")
                        {
                            fields.Add("COMMAND", "RELOADITEMLIST");
                        }
                        else
                        {
                            fields = CatchData;
                        }
                        DeviceCommand("SHOWMESSAGE", fields);
                    }
                    #endregion
                    #region Commented by Arvind - 16-Sept-2019 -PRIMEPOS-2735
                    //if (command.DeviceScreen == "SHOWMESSAGE")
                    //{
                    //    //if (CatchData.Count != 0)
                    //    //{
                    //    CatchData["ACTION"] = "";
                    //    Hashtable fields = new Hashtable();
                    //    fields.Add("COMMAND", "RELOADITEMLIST");
                    //    DeviceCommand("SHOWMESSAGE", fields);
                    //    //}
                    //}
                    #endregion

                    // NileshJ- 27-Feb-2019 - Signature Issue - PRIMEPOS-2656
                    if (command.DeviceScreen == "DOSIGNATURE" || command.DeviceScreen == "GETSIGNATURE")
                    {
                        IsSignValid = true;
                        GetSignatureString = " ";
                    }
                }
            }
        }

        private void PAX_ErrorLog()
        {
            logger.Debug("\t\tError activated from PAX ExecuteCommand ");
            if (CommandQueue.Count > 0 && !Constant.IsStillWrite)
            {
                CommandEvent += PAX_CommandEvent;
                CommandEvent.Invoke();
            }
        }

        private void PAX_CommandEvent()
        {
            CommandEvent -= PAX_CommandEvent; //deactivate the event
            if (DeviceThread == null ? true : !DeviceThread.IsAlive)
            {
                //If the thread is not alive start a new thread.
                DeviceThread = new Thread(ExecuteDeviceCommnads);
                DeviceThread.Start();
            }
        }

        #endregion

        #region PRIMEPOS-2730 - NileshJ
        public void ClearDeviceQueue()
        {
            lock (CommandQueue)
            {
                CommandQueue.Clear();
                //deviceData.Clear();//PRIMEPOS-3146
            }
        }
        public bool isDevQueueEmpty()
        {
            bool isQueueEmpty = false;
            lock (CommandQueue)
            {
                if (CommandQueue.Count == 0)
                {
                    isQueueEmpty = true;
                }
            }
            return isQueueEmpty;
        }
        #endregion
    }

    #region data structure to communicate with device
    //override .TOString() in all variable of order to format & pad them accordingly
    // const int finalLength = 15; in all Order variables classes is the lenght which is given on device screen text

    public class Order
    {

        //DisplayMessage1 
        public Descreption name
        {
            get; set;
        }

        //DisplyMessage2
        public Quantity quantity
        {
            get; set;
        }
        public UnitPrice price
        {
            get; set;
        }

        public Discount discount
        {
            get; set;
        }

        public Total total
        {
            get; set;
        }

        public DeviceType deviceType
        {
            get; set;
        }
        public int Index
        {
            get; set;
        }

        //  
        public override string ToString()
        {
            //string total = string.Empty;
            //Aries8
            if (deviceType == DeviceType.PAX_D200)
            {
                //t = String.Format("{0,-15} {1,-12} {2,-12} {3,-10} {4,-10}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());//Amit PRIMEPOS-2931
                //return String.Format("{0,-15} {1,-7} {2,12} {3,10} {4,-5}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());//Amit PRIMEPOS-2931
                //t = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}", "" + this.name.ToString().Trim(), ",,", this.total.ToString().Trim() + ",", "0" + ",", this.price.ToString().Trim(), ",,", this.quantity.ToString().Trim(), ",," + "");
                return String.Format("{0,-15} {1,-11} {2,-13} {3,-9} {4,-9}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());

            }
            else if (deviceType == DeviceType.PAX_A920)//PRIMEPOS-3146
            {
                return String.Format("{0}{1,-10}{2,1}", GetFirstString(Convert.ToString(this.name), 12), this.quantity.ToString(), this.total.ToString());
                //return String.Format("{0}{1,-10}{2,1}", GetFirstString(Convert.ToString(this.name), 12), this.quantity.ToString(), this.total.ToString()); 
            }
            else
            {
                return String.Format("{0,0} {1,1} {2,12} {3,12} {4}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());//Amit PRIMEPOS-2931
            }
        }

        #region //PRIMEPOS-3146
        public string GetLastString(string source, int tailLength)
        {
            if (tailLength >= source.Length)
                return source;
            return source.Substring(source.Length - tailLength);
        }
        public static string GetFirstString(string source, int tailLength)
        {
            if (tailLength >= source.Length)
                return source;
            return source.Substring(0, tailLength);
        }
        #endregion

        public string convert()
        {
            string total = string.Empty;
            //Aries8
            if (deviceType == DeviceType.PAX_D200)
            {
                //return String.Format("{0,-15} {1,-12} {2,-12} {3,-10} {4,-10}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());//Amit PRIMEPOS-2931
                //return String.Format("{0,-15} {1,-7} {2,12} {3,10} {4,-5}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());//Amit PRIMEPOS-2931
                //t = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}", "" + this.name.ToString().Trim(), ",,", this.total.ToString().Trim() + ",", "0" + ",", this.price.ToString().Trim(), ",,", this.quantity.ToString().Trim(), ",," + "");
                return total = this.total.ToString();
            }
            else
            {
                return String.Format("{0,0} {1,1} {2,12} {3,12} {4}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());//Amit PRIMEPOS-2931
            }
        }
    }



    public class Descreption
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 15;
        public Descreption(string content)
        {
            contentText = content;
            this.length = content.Length;
        }
        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadRight(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }

    public class Quantity
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 7;
        public Quantity(string content)
        {
            contentText = content;
            this.length = content.Length;
        }

        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadLeft(contentText.Length + padSize);
                    paddedString = paddedString.PadRight(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }

    public class UnitPrice
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 7;

        public UnitPrice(string content)
        {
            contentText = content;
            this.length = content.Length;
        }

        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadLeft(contentText.Length + padSize);
                    paddedString = paddedString.PadRight(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }

    public class Discount
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 5;
        public Discount(string content)
        {
            contentText = content;
            this.length = content.Length;
        }

        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadLeft(contentText.Length + padSize);
                    paddedString = paddedString.PadRight(contentText.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }

    public class Total
    {
        public string contentText = "";
        public int length = 0;
        const int finalLength = 7;
        public Total(string content)
        {
            contentText = content;
            this.length = content.Length;
        }

        public override string ToString()
        {
            string paddedString = "";
            if (contentText.Length <= finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadRight(contentText.Length + padSize);
                    paddedString = paddedString.PadLeft(paddedString.Length + padSize);
                }
            }
            if (contentText.Length >= finalLength)
            {
                paddedString = contentText.Substring(0, finalLength);
            }
            return paddedString;
        }
    }
    #endregion



    #region data structure to Show RxDetails
    public class RXInfo
    {

        //DisplayMessage1 
        public string RXNo
        {
            get; set;
        }

        //DisplyMessage2
        public string drugName
        {
            get; set;
        }
        public string date
        {
            get; set;
        }


        //  
        public override string ToString()
        {
            //return String.Format("{0,0} {1,0} {2,13} {3,17} {4}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());
            return String.Format("{0,0} {1,10} {2,12}", this.RXNo.ToString(), this.drugName.ToString(), this.date.ToString()); // PRIMEPOS - 2555 change string format - NileshJ
        }
    }
    #endregion
}
