using Evertech.Implementation;
using MMS.Device.Global;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Evertech;
using Evertech.Data;
using Newtonsoft.Json;

namespace MMS.Device.Evertec
{
    public class EvertecVx820
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        #region Variable and Properties
        EvertechProcessor device = null;
        private static EvertecVx820 dDefObj = null;
        static Thread DeviceThread = null;
        private readonly object DeviceLock = new object();
        delegate void CommandHandler();
        event CommandHandler CommandEvent;
        Hashtable PData;
        delegate void EVERTECErrorEventLog();
        static event EVERTECErrorEventLog ErrorLog;
        private List<Order> deviceData = null;
        Queue<CommandStation> CommandQueue = new Queue<CommandStation>();
        private PmtTxnResponse deviceResponse;
        CultureInfo CI = CultureInfo.CurrentCulture;

        public const string BTN_YES = "1";
        public const string BTN_NO = "2";

        // - Handling Thread Exception
        public object exceptionObj = new object();
        public bool isException = false;


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
        public bool? shouldMoveNext = null;  
        private int ConsentApplies = -1;
        private int ConsentStatus = -1;



        #endregion
        public static EvertecVx820 DefaultInstance(string _posSettings,string _terminalID,string _stationID,string id)
        {            

            if (dDefObj == null)
            {
                dDefObj = new EvertecVx820(_posSettings,_terminalID,_stationID,id);
                dDefObj.deviceData = new List<Order>();
            }
            return dDefObj;
        }

        private EvertecVx820(string _posSettings,string _terminalID,string _stationID,string _ID)
        {

            string hostAddress = _posSettings.Split(':')[0];
            string hostPort = _posSettings.Split(':')[1].Split('/')[0];

            device = EvertechProcessor.getInstance(hostAddress,Convert.ToInt32(hostPort));      
            
            Logon(_terminalID,_stationID,_ID);
            
        }
                
        #region Device Public method to send commands
        
        public void Logon(string terminalID,string stationID,string cashierID)
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "LOGON");
            fields.Add("TERMINALID", terminalID);
            fields.Add("STATIONID", stationID);
            fields.Add("CASHIERID", cashierID);
            DeviceCommand("LOGON", fields);
        }
        

        public bool CaptureRxSignature()
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "CAPTURESIGNATURE");
            DeviceCommand("CAPTURESIGNATURE", fields);
            return true;
        }       
       
                         

        public bool GetSignature()
        {
            DeviceCommand("GETSIGNATURE", null);
            return true;
        }
        

        
        public void CaptureOTCItemSignature()
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "CAPTURESIGNATURE");
            DeviceCommand("CAPTURESIGNATURE", fields);
        }

        #region PRIMEPOS-3209

        public void CapturePatConsel(string sCurrentTransID, string totalRx, string patientName, string patientCounceling, DataTable rxList, bool bHidePatCounseling = false) //PRIMEPOS-3442
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

                RXList = rxList.Copy();
                logger.Debug("Total Rx's: " + RXList.Rows.Count);
                Constant.Counsel = patientCounceling; //orignial Patient counseling from POS

                Hashtable RXPatConsel = new Hashtable();
                RXPatConsel.Add("COMMAND", "PATCONSEL");
                RXPatConsel.Add("PATIENTNAME", patientName);
                RXPatConsel.Add("RXCOUNT", totalRx);
                RXPatConsel.Add("ISPATCONSELHIDE", bHidePatCounseling); //PRIMEPOS-3442
                DeviceCommand("PATCONSEL", RXPatConsel);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "EvertecVx820==>CapturePatConsel(): An Exception Occured");
                throw ex;
            }
        }

        public void CaptureNOPP()
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "CAPTURENOPP");
            DeviceCommand("CAPTURENOPP", fields);
        }
        #endregion

        public void CaptureOTCItemSignatureAknowledgement(ArrayList otcList)
        {
            Hashtable fields = new Hashtable();
            fields.Add("COMMAND", "CAPTURESIGNATURE");
            DeviceCommand("CAPTURESIGNATURE", fields);            
        }
        
        #endregion

        #region EVERTECDeviceCommandQueue 
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
                    // - Handling Thread Exception
                    bool isException = false;
                    lock (exceptionObj)
                    {
                        isException = this.isException;
                    }
                    //
                    if (isException || (DeviceThread == null ? true : !DeviceThread.IsAlive))// - Handling Thread Exception - Add isException
                    {
                        // if exception and thread.isalive = true; kill the thread
                        if (DeviceThread != null)
                        {
                            if (!isException)// - 13-Dec-2018
                            {
                                DeviceThread.Abort();
                            }

                            DeviceThread = null;
                            //if (isException)
                            //{
                            //    if (DataP["COMMAND"].Equals("RELOADITEMLIST"))
                            //    {
                            //        Hashtable fields = new Hashtable();
                            //        fields.Add("COMMAND", "RELOADITEMLIST");
                            //        //DeviceCommand("SHOWMESSAGE", fields);
                            //        CommandQueue = new Queue<CommandStation>();
                            //        CommandQueue.Enqueue(new CommandStation("SHOWMESSAGE", fields));
                            //    }
                            //}
                        }

                        lock (exceptionObj) { isException = false; this.isException = false; } //  - Lock Exception

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
            CommandStation command = null; //  - Add 
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
                            //CommandStation command = null; //  - Commented
                            command = null; // 
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
                                ErrorLog += EVERTEC_ErrorLog; //Event for Error. This will call the Thread again //  - Commented
                                ErrorLog.Invoke(); //  Commented

                                lock (exceptionObj) { isException = true; } //  - Lock Exception
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

                            logger.Debug("\t\tEntering EVERTEC ExecuteCommand ");


                            switch (cScreen.ToUpper().Trim())
                            {
                                case "LOGON":
                                    logger.Debug("\t\t\t[LOGON] Begin");
                                    string terminalID = (string)PData["TERMINALID"];
                                    string stationID = (string)PData["STATIONID"];
                                    string cashierID = (string)PData["CASHIERID"];
                                    deviceResponse = device.Logon(terminalID,stationID,cashierID);
                                    logger.Debug("\t\t\t[LOGON] Finish");
                                    break;
                                case "LOGOFF":
                                    deviceResponse = device.Logoff();
                                    break;
                                case "GETSIGNATURE":
                                    deviceResponse = device.GetSignature();
                                    if (deviceResponse.SignatureData != null && deviceResponse.SignatureData.Length > 0)
                                    {
                                        GetSignatureString = deviceResponse.SignatureData;
                                    }
                                    break;
                                case "CAPTURESIGNATURE":
                                    deviceResponse = device.CaptureSignature("SIGN HERE");

                                    if (deviceResponse.ResultSignature == "OK")
                                    {
                                        IsSignValid = true;
                                    }
                                    else
                                    {
                                        IsSignValid = false;
                                    }
                                    break;
                                case "PATCOUNSIL":
                                    logger.Debug("\t\t\t[PATCOUNSIL] Begin");
                                    deviceResponse = device.CaptureSignature("SIGN HERE");
                                    if (deviceResponse.ResultSignature == "OK")
                                    {
                                        IsSignValid = true;
                                    } else
                                    {
                                        IsSignValid = false;
                                    }
                                    break;
                                case "REBOOT":
                                    logger.Debug("\t\t\t[REBOOT] Begin");                                    
                                    logger.Debug("\t\t\t[REBOOT] Finish");
                                    break;
                                case "SHOWDIALOG":
                                case "CAPTURENOPP": //PRIMEPOS-3209
                                    string altMsg = "HIPAA ACKNOWLEDGEMENT/Proceed?///";
                                    string btn1cap = "YES";
                                    string btn2cap = "NO";
                                    string selectedResp = device.CaptureConfirmation(altMsg, btn1cap, btn2cap);
                                    if (!string.IsNullOrEmpty(selectedResp))
                                    {
                                        HIPPAAckResultId = selectedResp;
                                    }
                                    else
                                    {
                                        HIPPAAckResultId = "2";
                                    }
                                    break;
                                case "PATCONSEL":
                                    string RXCount = (string)PData["RXCOUNT"];
                                    string patName = (string)PData["PATIENTNAME"];
                                    bool IsPatConselHide = (bool)PData["ISPATCONSELHIDE"]; //PRIMEPOS-3442
                                    string alteredMsg = string.Empty; //PRIMEPOS-3442
                                    string selectedResponse = string.Empty; //PRIMEPOS-3442
                                    var patientName = patName;
                                    if (patName.Length > 21)
                                    {
                                        patientName = patName.Substring(0, 20);
                                    }
                                    if (IsPatConselHide) //PRIMEPOS-3442
                                    {
                                        alteredMsg = "Rx Pickup Ack./" + patientName.Trim() + "/RxCount : " + RXCount + "//";
                                        selectedResponse = device.CaptureConfirmation(alteredMsg, "YES", "NO");
                                    }
                                    else
                                    {
                                        alteredMsg = "Rx Pickup Ack./" + patientName.Trim() + "/RxCount : " + RXCount + "/Patient Counseling?/";
                                        selectedResponse = device.CaptureConfirmation(alteredMsg, "YES", "NO");
                                    }
                                    if (IsPatConselHide) //PRIMEPOS-3442 If Hide Patient Counseling is true then always 
                                    {
                                        PatCounsel = Constant.PatientCounselYes;
                                    }
                                    else
                                    {
                                        PatCounsel = selectedResponse == "1" ? Constant.PatientCounselYes : Constant.PatientCounselNo;
                                    }
                                    break;
                                case "SHOWMESSAGE":
                                case "CLEARMESSAGE":
                                case "RESET":
                                case "UPDATERESOURCEFILE":
                                case "DELETEIMAGE":
                                case "GETPINBLOCK":
                                case "INPUTACCOUNT":
                                case "RESETMSR":
                                case "INPUTTEXT":
                                case "CHECKFILE":
                                case "SHOWTEXTBOX":
                                case "AUTHORIZECARD":
                                case "COMPLETEONLINEEMV":
                                case "REMOVECARD":
                                case "GETEMVTLVDATA":
                                case "SETEMVTLVDATA":
                                case "INPUTACCOUNTWITHEMV":
                                case "COMPLETECONTACTLESSEMV":
                                case "SETSAFPARAMETERS":
                                
                                case "REPRINT":
                                case "PRINTER":
                                case "SHOWITEM":
                                case "CARDINSERTDETECTION":
                                case "TOKENADMINISTRATIVE":
                                case "SHOWDIALOGFORM": 
                                    break;
                                default:
                                    break;

                            }
                            logger.Debug("\t\t ----> EVERTEC ExecuteCommand finish setting");

                            Constant.IsInMethod = false;
                        }
                    }
                    Constant.IsStillWrite = false; //Now allow to start a new thread.
                }
            }
            catch (UnauthorizedAccessException uA)
            {
                logger.Error("\t\tError in EVERTEC ExecuteCommand  \n" + uA.ToString());
                Constant.IsStillWrite = false;
                Constant.IsInMethod = false;
                ErrorLog += EVERTEC_ErrorLog;
                ErrorLog.Invoke();

            }
            catch (Exception ex)
            {
                logger.Debug("\t\tError in EVERTEC ExecuteCommand  \n" + ex.ToString());
                Constant.IsStillWrite = false;
                Constant.IsInMethod = false;
                ErrorLog += EVERTEC_ErrorLog;
                ErrorLog.Invoke();
                //  - Exception Handler
                lock (exceptionObj) { isException = true; }
                if (command != null)
                {
                    Hashtable CatchData = command.DeviceData as Hashtable;

                    if (command.DeviceScreen == "SHOWMESSAGE")
                    {
                        //if (CatchData.Count != 0)
                        //{
                        CatchData["ACTION"] = "";
                        Hashtable fields = new Hashtable();
                        fields.Add("COMMAND", "RELOADITEMLIST");
                        DeviceCommand("SHOWMESSAGE", fields);
                        //}
                    }
                    if (command.DeviceScreen == "DOSIGNATURE" || command.DeviceScreen == "GETSIGNATURE")
                    {

                        IsSignValid = true;
                        GetSignatureString = " ";
                    }

                }
            }
        }

        private void EVERTEC_ErrorLog()
        {
            logger.Debug("\t\tError activated from EVERTEC ExecuteCommand ");
            if (CommandQueue.Count > 0 && !Constant.IsStillWrite)
            {
                CommandEvent += EVERTEC_CommandEvent;
                CommandEvent.Invoke();
            }
        }

        private void EVERTEC_CommandEvent()
        {
            CommandEvent -= EVERTEC_CommandEvent; //deactivate the event
            if (DeviceThread == null ? true : !DeviceThread.IsAlive)
            {
                //If the thread is not alive start a new thread.
                DeviceThread = new Thread(ExecuteDeviceCommnads);
                DeviceThread.Start();
            }
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

        //  
        public override string ToString()
        {
            return String.Format("{0,0} {1,0} {2,13} {3,17} {4}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString(), this.total.ToString());
            //return String.Format("{0} {1} {2} {3}", this.name.ToString(), this.quantity.ToString(), this.price.ToString(), this.discount.ToString());
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
            if (contentText.Length < finalLength)
            {
                int padSize = finalLength - contentText.Length;
                if (padSize > 0)
                {
                    paddedString = contentText.PadRight(contentText.Length + padSize);
                    paddedString = paddedString.PadLeft(paddedString.Length + padSize);
                }
            }
            if (contentText.Length > finalLength)
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
            return String.Format("{0,0} {1,10} {2,12}", this.RXNo.ToString(), this.drugName.ToString(), this.date.ToString()); 
        }
    }
    #endregion
}
