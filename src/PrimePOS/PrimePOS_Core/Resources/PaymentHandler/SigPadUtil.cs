using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
//Added by SRT (Dharmendra)
using System.Globalization;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Timers;
using System.Windows.Forms;
using MMS.HOSTSRV;
using POS_Core.CommonData;
using VFDevice = MMS.Device.Communication;
using NLog;
using PAXPx7Device = MMS.Device.PAX.PAXPx7;
using EvertecVx820Device = MMS.Device.Evertec.EvertecVx820;
using VerifoneMx925Device = MMS.Device.Verifone.VerifoneMx925;//PRIMEPOS-2636
using ElavonLink8000 = MMS.Device.Elavon.Elavon8000;//PRIMEPOS-2943
using MMS.Device;
using POS_Core.Resources;
using POS_Core.Resources.DelegateHandler;
using POS_Core.CommonData.Rows;
namespace POS_Core.Resources.PaymentHandler
{
    /// <summary>
    /// Summary description for SigPadUtil.
    /// </summary>
    ///

    public class SigPadUtil : Form
    {
        private TcpChannel tcpChanel;
        private IHost MMSHost;
        public VFDevice VF = null;
        //Suraj
        public PAXPx7Device PD = null;
        //
        public EvertecVx820Device EV = null;//PRIMEPOS-2664
        public VerifoneMx925Device VFD = null;//PRIMEPOS-2636
        public ElavonLink8000 ELV = null;//PRIMEPOS-2943
        private string sCurrentTransID = "";
        private static SigPadUtil oDefObj;
        public SigPadCardData oSigPadCardData;
        private readonly object DatafromPad = new object();
        //Modified By Dharmendra(SRT), The orginal delimeter was #
        // because POS sales item also contains # this causes invalid spliting of
        //item data when passed by AddItem method of MMS.Host
        private const string DELIM_CHARACTER = "%";
        private string strSignature;
        private string defvalue;
        public string strsigtype;
        private byte[] binSignature;
        private string sigType;
        private const string DECLINED_SCREEN = "DECLINED_SCREEN";
        private const string CARDSWIPECANCEL = "6000";
        private const string NOPPCAPTURECANCEL = "6002";
        private const string NOPPUSERCANCEL = "6008";
        private const string RXCAPTURECANCEL = "6007";
        private const string SIGNATURECAPTURECANCEL = "6001";
        private const string SWIPEATTEMPTEXCEED = "6003";
        private const string INITIATESIGNATURE = "6005";
        private const string NOPPUSERCONTINUE = "6009";
        private const string OTCCAPTURECANCEL = "6011";

        private POSCallbackClass mcs = null;
        //Added By Dharmendra on 29-08-08
        public string cardType = string.Empty;
        public string zipCode = string.Empty;
        public string customerAddress = string.Empty;
        private string paymentType;
        private const string ITEMSCREEN = "padItemList";
        // Added By SRT Dharmendra
        private const string ITEMNAME = "ItemName";
        private const string UNITPRICE = "UnitPrice";
        private const string ITEMQTY = "ItemQty";
        private const string TOTALPRICE = "TotalPrice";
        private const string DISCOUNT = "Discount"; //PRIMEPOS-2952
        //Added By SRT(Ritesh Parekh)  Date: 29-Jul-2009
        //This constant is used to add filed along with items in add item method to validate whether it was rx or not.
        private const string ISRX = "ISRX";
        private const string ISRXTRUE = "1";
        private const string ISRXFALSE = "0";
        public const string SHOWRXDESCRIPTION = "SHOWRXDESCRIPTION";
        //End Of Added By SRT(Ritesh Parekh)
        public const string WITHDISCAMOUNT = "WITHDISCAMOUNT"; // added by sachin 01-nov-2010
        private bool isItemOnHold = false; //added by Manoj 10/12/2011
        private bool isPosStartWithPad = false; //added by Manoj 3/11/2013
        private bool isAlreadyOn = false;
        private bool isConnectedToServer = false;
        private bool isVF = false;
        public bool isISC = false;
        //Suraj 
        public bool isPAX = false;
        //
        //Arvind - PRIMEPOS-2636
        public bool isVantiv = false;

        //PRIMEPOS-2664
        public bool isEvertec = false;
        //
        //PRIMEPOS-2943
        public bool isElavon = false;
        //
        public bool isWP = (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "WORLDPAY");
        //( //if (CPOSSet.PaymentProcessor.ToUpper() == "WORLDPAY")
        //{
        //    POS.Resources.SigPadUtil.DefaultInstance.isISC = true;
        //})
        private bool isInDevice = false;
        bool deviceWorkin = false;
        private int iiCount;
        private int sigCount;
        public CultureInfo ci = CultureInfo.CurrentCulture;
        DataTable rxList = new DataTable();
        DataColumn rxNo = new DataColumn("RXNO");
        DataColumn drugName = new DataColumn("DRUGNAME");
        DataColumn rxDate = new DataColumn("RXDATE");
        //Added By SRT(Ritesh Parekh) Date : 29-Jul-2009
        DataColumn rxToDisplay = new DataColumn("SHOWRXDESCRIPTION");
        //End Of Added By SRT(Ritesh Parekh)

        //End Added by SRT Dharmendra
        //Added By SRT (Gaurav) for HeartBeatTimer Declearation and Timer Interval Initialisation.
        System.Timers.Timer heartBeatTimer;
        string heartBeats;        
        int heartBeatMSec;        
        //End Added by SRT (Gaurav)
        string heartBeatsElavonHeartbeat;//PRIMEPOS-3260
        System.Timers.Timer heartBeatTimerElavonHeartbeat; //PRIMEPOS-3260
        int heartBeatMSecElavonHeartbeat;//PRIMEPOS-3260
        private int itemonholdID;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void itemonHoldID(int val)
        {
            itemonholdID = val;
        }

        public void iSItemOnHold(bool value)
        {
            isItemOnHold = value;
        }

        public int iCount
        {
            get { return iiCount; }
            set { iiCount = value; }
        }

        public bool IsVF
        {
            get { return isVF; }
        }

        public bool InMethod
        {
            get
            {
                if (isVF)
                    return VF.IsStillWrite;
                else
                    return false;
            }
        }

        public bool MsrCancel
        {
            set
            {
                if (value)
                {
                    VF.isMsrCancel = true;
                }
            }
        }

        public bool ISCCancel { get; set; }

        public bool isFinishDevice
        {
            get;
            set;
        }

        public string CashBack { get; set; }
        public SigPadCardData SigPadCardInfo
        {
            get { return oSigPadCardData; }
            set { oSigPadCardData = value; }
        }

        public string CustomerSignature
        {
            get { return strSignature; }
            set
            {
                if (value == null)
                {
                    strSignature = "";
                }
                else
                {
                    strSignature = value;
                }
            }
        }

        public string SigType
        {
            get { return sigType; }
            set
            {
                if (value == null)
                {
                    sigType = "";
                }
                else
                {
                    sigType = value;
                }
            }
        }

        public byte[] BinarySignature
        {
            get { return binSignature; }
            set
            {
                if (value == null)
                {
                    binSignature = null;
                }
                else
                {
                    binSignature = value;
                }
            }
        }

        public string DefaultValue
        {
            get { return strSignature; }
            set
            {
                if (value == null)
                {
                    defvalue = "";
                }
                else
                {
                    defvalue = value;
                }
            }
        }

        public static SigPadUtil DefaultInstance
        {
            get
            {
                if (oDefObj == null)
                {
                    oDefObj = new SigPadUtil();
                }
                /*else if (oDefObj.MMSHost == null)
                {
                    oDefObj = new SigPadUtil();
                }*/

                return oDefObj;
            }
        }

        public SigPadUtil()
        {
            //
            // TODO: Add constructor logic here
            //
            //Added by SRT for optimization of HHP code.
            rxList.Columns.Add(rxNo);
            rxList.Columns.Add(drugName);
            rxList.Columns.Add(rxDate);
            //Added By SRT(Ritesh Parekh) Date: 29-Jul-2009
            rxList.Columns.Add(rxToDisplay);
            //End Of Added By SRT(Ritesh Parekh)
            //Added till here by SRT.

            try
            {
                if (Configuration.CPOSSet.SigPadHostAddr.Trim().Length == 0)
                {
                    clsCoreUIHelper.ShowErrorMsg("Please provide valid SigPad Host Address.");
                    Configuration.CPOSSet.UseSigPad = false;
                }
                else
                {
                    this.ConnectServer();
                    if (Configuration.CPOSSet.UseSigPad && !this.isVF && Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("PRIMEPOSHOSTSRV"))
                    {
                        try
                        {
                            MMSHost.InitalizePrimeHost();
                        }
                        catch (Exception Ex)
                        {
                            clsCoreUIHelper.ShowErrorMsg("Signature Pad is not Connected");
                            logger.Fatal(Ex, "SigPadUtil() - Signature Pad is not Connected");
                        }
                        mcs = new POSCallbackClass(this);
                        RegisterEvent();
                        //Added by SRT (Gaurav)
                        InitiateTimers();
                        heartBeatTimer.Enabled = true;
                        heartBeatTimer.Start();
                        //End Of Added by SRT (Gaurav)
                    }
                    #region PRIMEPOS-3260
                    if (Configuration.CPOSSet.UseSigPad && this.isElavon)
                    {
                        InitiateElavonHeartBeatTimers();
                        heartBeatTimerElavonHeartbeat.Enabled = true;
                        heartBeatTimerElavonHeartbeat.Start();
                    }
                    #endregion
                }
            }
            catch (Exception Ex)
            {

                if (!this.isVF && !Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("VERIFONEPOSHOSTSRV"))
                {
                    MMSHost = null;
                }
                clsCoreUIHelper.ShowErrorMsg("Signature Pad Failed to Connect.");
                logger.Fatal(Ex, "SigPadUtil() - Signature Pad Failed to Connect.");
            }
        }
        public void CloseCOnnection()
        {
            if (this.IsVF)
            {
                VF.Disconnect();
            }
        }
        #region PRIMEPOS-3260
        private void InitiateElavonHeartBeatTimers()
        {
            //Here HeartBeat Frequency Is Converted In Milliseconds.
            heartBeatsElavonHeartbeat = Configuration.CPOSSet.HeartBeatTime;
            if (heartBeatsElavonHeartbeat.Trim().Equals(string.Empty) 
                || heartBeatsElavonHeartbeat.Trim() == null 
                || heartBeatsElavonHeartbeat.Trim() == "0"
                || Convert.ToInt32(heartBeatsElavonHeartbeat) < 300)
            {
                heartBeatsElavonHeartbeat = "300";//In second
            }
            
            //End of Addition
            heartBeatMSecElavonHeartbeat = Convert.ToInt32(heartBeatsElavonHeartbeat) * 1000;
            //Initialisation
            heartBeatTimerElavonHeartbeat = new System.Timers.Timer
            {
                Interval = heartBeatMSecElavonHeartbeat,
                Enabled = false
            };
            heartBeatTimerElavonHeartbeat.Elapsed += new ElapsedEventHandler(OnTimedEventElavonHeartbeat);
        }
        private void OnTimedEventElavonHeartbeat(object source, ElapsedEventArgs e)
        {
            try
            {
                if (this.isElavon && isConnected())
                {
                    heartBeatTimerElavonHeartbeat.Enabled = false;
                    ElavonLink8000.HeartbeatMessage(Configuration.CPOSSet.SigPadHostAddr);
                    heartBeatTimerElavonHeartbeat.Enabled = true;
                    heartBeatTimerElavonHeartbeat.Start();
                }
            }
            catch (System.Net.Sockets.SocketException soex)
            {
                logger.Error($"SocketException occured on SigPadUtil=>OnTimedEventElavonHeartbeat=>:{soex}");
                heartBeatTimerElavonHeartbeat.Enabled = false;
                ElavonLink8000.HeartbeatMessage(Configuration.CPOSSet.SigPadHostAddr);
                heartBeatTimerElavonHeartbeat.Enabled = true;
                heartBeatTimerElavonHeartbeat.Start();
            }
            catch (Exception ex)
            {
                logger.Error($"Exception occured on SigPadUtil=>OnTimedEventElavonHeartbeat=>:{ex}");
            }
        }
        #endregion
        public bool StartTransaction(string TransID, string LastTransID)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "StartTransaction()", clsPOSDBConstants.Log_Entering);
            logger.Trace("StartTransaction() - Sig Pad - " + clsPOSDBConstants.Log_Entering);
            int reset = 0;
            iCount = 0;
            bool retVal = false;
            itemonHoldID(reset);
            if (!this.isVF && MMSHost == null && !this.isPAX && !this.isVantiv && !this.isElavon) //PRIMEPOS-2528 (Suraj) 1-June-18 Added  !isPAx check to log error if no device is found PRIMEPOS-2636 ADDED BY ARVIND//2943 
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "StartTransaction() - MMSHost == Null", clsPOSDBConstants.Log_Exiting);
                logger.Trace("StartTransaction() - MMSHost | HPSPAX == Null - " + clsPOSDBConstants.Log_Exiting);
                return false;
            }
            /*if (isConnected()==false)
            {
                oDefObj = new SigPadUtil();
            }*/

            //if (isConnected())
            //{
            string CurrID;
            if (this.isVF) {
                CurrID = TransID;
            } else if (this.isPAX) {
                PD.ResetToIdle();
                return true;
            }
            else if (this.isEvertec)//PrimePOS-2664
            {
                return true;
            }
            else if (this.isVantiv)//Arvind PRIMEPOS-2636
            {
                VFD.ResetToIdle();
                return true;
            }
            else if (this.isElavon)//Arvind PRIMEPOS-2943
            {
                ELV.ResetToIdle();
                return true;
            }
            else {
                MMSHost.GetCurrentTxnId(out CurrID);
            }

            if (CurrID == LastTransID && CurrID != "")
            {
                if (this.isVF)
                {
                    ShowCustomScreen("Transaction Aborted.");
                }
                else
                {
                    MMSHost.AbortTxn(LastTransID);
                }
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "StartTransaction()", " Abort Transaction");
                logger.Trace("StartTransaction() - Abort Transaction - " + clsPOSDBConstants.Log_Exiting);
            }
            int retValue = 0;
            //DateTime dt = DateTime.Now;
            if (this.isVF)
            {
                deviceWorkin = true;
                retValue = VF.StartTxn(TransID);
                deviceWorkin = false;

            }
            else
            {
                retValue = MMSHost.StartTxn(TransID);
            }
            sCurrentTransID = TransID;
            oSigPadCardData = null;

            if (retValue == 0)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "StartTransaction()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("StartTransaction() - Sig Pad - " + clsPOSDBConstants.Log_Exiting);
                return true;
            }
            //}
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "StartTransaction()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("StartTransaction() - Sig Pad - " + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        public void DisplayCashPmtOnDevice(string GrossAmount, string TotTaxAmount, string NetAmount, string PaidAmount, string ChangeDueAmount)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "DisplayCashPmtOnDevice()", clsPOSDBConstants.Log_Entering);
            logger.Trace("DisplayCashPmtOnDevice() - Sig Pad - " + clsPOSDBConstants.Log_Entering);
            if (isConnected())
            {
                if (this.isVF) {
                    deviceWorkin = true;
                    VF.ProcessCashTxn(this.sCurrentTransID.Trim(), GrossAmount, TotTaxAmount, NetAmount, PaidAmount, ChangeDueAmount);
                    deviceWorkin = false;
                } else if (this.isPAX) {
                    //Suraj Show payment successful for amount.... already exist in CompleteTXN
                }
                //PRIMEPOS-2664
                else if (this.isEvertec)
                {
                    //Empty because device doesn't support this
                }
                //PRIMEPOS-2636 ADDED BY ARVIND
                else if (this.isVantiv)
                {
                    //Empty
                }
                //
                //PRIMEPOS-2943
                else if (this.isElavon)
                {
                    //Empty
                }
                //
                else
                {
                    MMSHost.ProcessCashTxn(this.sCurrentTransID.Trim(), GrossAmount, TotTaxAmount, NetAmount, PaidAmount, ChangeDueAmount);
                }
                GetCurrentScreen();
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "DisplayCashPmtOnDevice()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("DisplayCashPmtOnDevice() - Sig Pad - " + clsPOSDBConstants.Log_Exiting);
        }

        public void AddItem(CommonData.Rows.TransDetailRow oNewRow, int itemIndexNum) // Added additional column by SRT for HHP optimization.
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "AddItem()", clsPOSDBConstants.Log_Entering);
            logger.Trace("AddItem() - Sig Pad - " + clsPOSDBConstants.Log_Entering);
            //Added by SRT (Dharmendra) for optimization.
            try
            {
                if (this.isVF)
                {
                    VF.AddItems(sCurrentTransID, oNewRow, itemIndexNum, Configuration.CPOSSet.PrintRXDescription.ToString());
                }
                else if (isConnected() && !this.isVF)
                {
                    //Added by SRT (Dharmendra) for optimization.
                    //AddedBy Shitaljit for code optimazation and make sure no null or empty string is passesd to PAD.
                    Hashtable fields = new Hashtable(); //Used HashTable to avoid string formation and splitting time.
                    oNewRow.ItemDescription = string.IsNullOrEmpty(oNewRow.ItemDescription.Trim()) ? "Item Description Not Set" : oNewRow.ItemDescription.Trim();
                    oNewRow.QTY = Configuration.convertNullToInt(string.IsNullOrEmpty(oNewRow.QTY.ToString().Trim()) ? "0" : oNewRow.QTY.ToString().Trim().Trim());
                    oNewRow.Price = Configuration.convertNullToDecimal(string.IsNullOrEmpty(oNewRow.Price.ToString().Trim()) ? "Item Description Not Set" : oNewRow.Price.ToString().Trim());
                    oNewRow.ExtendedPrice = Configuration.convertNullToDecimal(string.IsNullOrEmpty(oNewRow.ExtendedPrice.ToString().Trim()) ? "Item Description Not Set" : oNewRow.ExtendedPrice.ToString().Trim());
                    //End of added by shitaljit.

                    fields.Add(ITEMNAME, @"\1" + oNewRow.ItemDescription.Trim()); // PRIMEPOS - 2555  Add @"\1" for Small Font text to display ItemList on device - NileshJ
                    fields.Add(ITEMQTY, oNewRow.QTY.ToString());
                    fields.Add(UNITPRICE, oNewRow.Price.ToString("F", ci).PadRight(2, '0'));
                    fields.Add(TOTALPRICE, (oNewRow.ExtendedPrice - oNewRow.Discount).ToString("F", ci).PadRight(2, '0'));//PrimePOS-3188
                    //Added By SRT(Ritesh Parekh) Date: 29-Jul-2009
                    //ISRX-This parameter will carry bool value for type of item RX-ISRXTRUE /OTC-ISRXFALSE
                    fields.Add(ISRX, oNewRow.ItemID.ToString() == "RX" ? ISRXTRUE : ISRXFALSE);
                    fields.Add(SHOWRXDESCRIPTION, Configuration.CPOSSet.PrintRXDescription.ToString());
                    //End Of Added By SRT(Ritesh Parekh)

                    Decimal TotalAmountWithDis = oNewRow.ExtendedPrice - oNewRow.Discount;
                    fields.Add(WITHDISCAMOUNT, TotalAmountWithDis.ToString("F", ci).PadRight(2, '0')); //change by SRT (Sachin) Dated : 1st Nov 2010

                    string currentHHScr = string.Empty; //To store the value of current screen on HHP Device
                    if (!isPAX && !isEvertec && !isVantiv && !isElavon)//PRIMEPOS-2664 ADDED BY ARVIND EVERTEC //PRIMEPOS-2636 ADDED BY ARVIND VANTIV
                    {
                        int errorCode = MMSHost.GetCurrentScreen(sCurrentTransID, out currentHHScr);
                        if (currentHHScr.ToUpper() != "PADITEMLIST")
                        {
                            MMSHost.DisplayScreen(sCurrentTransID, "ITEMLIST_SCREEN", "");
                        }
                        //End Added
                        //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "AddItem()", "CurrentTransID " + sCurrentTransID + " ItemId: " + oNewRow.ItemID + " ItemDescription: " + oNewRow.ItemDescription + " TransDetailID: " + oNewRow.TransDetailID);
                        logger.Trace("AddItem() - CurrentTransID " + sCurrentTransID + " ItemId: " + oNewRow.ItemID + " ItemDescription: " + oNewRow.ItemDescription + " TransDetailID: " + oNewRow.TransDetailID);
                        errorCode = MMSHost.AddItem(sCurrentTransID, itemIndexNum, fields); //The Interface has changed in MMS.HOST interface.
                                                                                            //POS_Core.ErrorLogging.Logs.Logger("Index Send " + itemIndexNum + "Error Code: " + errorCode);
                        logger.Trace("AddItem() - Index Send " + itemIndexNum + "Error Code: " + errorCode);
                    }
                    else if (isEvertec)//PRIMEPOS-2664
                    {
                        //Evertech does not support item preview on sigpad
                    }
                    else if (isVantiv && Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_LINK_2500") //PRIMEPOS-2636 ADDED BY ARVIND //PRIMEPOS-3231 Added Link2500 does not support. 
                    {
                        if (fields["ItemName"].ToString().Contains(@"\1"))
                        {
                            fields["ItemName"] = fields["ItemName"].ToString().Remove(0, 2);
                        }
                        fields.Add("TAX", "" + oNewRow.TaxAmount);
                        fields.Add("DISCOUNT", "" + oNewRow.Discount);
                        VFD.AddItem(fields);
                    }
                    else if (isElavon) //PRIMEPOS-2943
                    {
                        if (fields["ItemName"].ToString().Contains(@"\1"))
                        {
                            fields["ItemName"] = fields["ItemName"].ToString().Remove(0, 2);
                        }
                        fields.Add("TAX", "" + oNewRow.TaxAmount);
                        fields.Add("DISCOUNT", "" + oNewRow.Discount);
                        ELV.AddItem(fields);
                    }
                    else if (Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_LINK_2500") //PRIMEPOS-3231
                    {
                        //Added For HPSPAX To ShowOnly Discount
                        fields.Add("DISCOUNT", ""+oNewRow.Discount);
                        PD.AddItem(fields);
                    }
                    
                }
                else if (Configuration.CPOSSet.UseSigPad && !isConnected())
                {
                    if (Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper())    //Sprint-23 - PRIMEPOS-2321 29-Jul-2016 JY Added if condition//3002
                    {
                        clsCoreUIHelper.ShowErrorMsg("\tSignature pad is not connected. Please check \nthe pad connection or disable it from preference.");
                        //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "AddItem()", " ----->> Signature pad is NOT connected");
                        logger.Trace("AddItem() - ----->> Signature pad is NOT connected");
                    }
                }
            }
            catch (Exception Ex)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "AddItem()", clsPOSDBConstants.Log_Exception_Occured + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "SigPadUtil() - Error while sending items details to signature pad.");
                clsCoreUIHelper.ShowErrorMsg("Error while sending items details to signature pad.");
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "AddItem()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("AddItem() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        public void CompleteTransaction(decimal amount)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CompleteTransaction()", clsPOSDBConstants.Log_Entering);
            logger.Trace("CompleteTransaction() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            if (isConnected())
            {
                ArrayList msgList = new ArrayList();
                if (this.isVF)
                {
                    VF.EndTxn(this.sCurrentTransID.Trim());
                } else if (this.isPAX) {
                    PD.EndTxn(""+amount);
                }
                else if (this.isEvertec)
                {
                    //Empty PRIMEPOS-2664
                }
                //PRIMEPOS-2636
                else if (this.isVantiv)
                {
                    VFD.EndTxn("" + amount);
                }
                //
                else if (this.isElavon)//2943
                {
                    ELV.EndTxn("" + amount);
                }
                else
                {
                    MMSHost.EndTxn(this.sCurrentTransID.Trim());
                }
                this.sCurrentTransID = "";
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CompleteTransaction()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CompleteTransaction() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        private void DisposeObjects()
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "DisposeObjects()", clsPOSDBConstants.Log_Entering);
            logger.Trace("DisposeObjects() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            int retVal = 0;
            if (!this.isVF && Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("PRIMEPOSHOSTSRV"))
            {
                if (MMSHost != null)
                {
                    MMSHost.IsWelcomeScreen(Configuration.IsWelComeScreen);
                    retVal = MMSHost.AbortTxn(this.sCurrentTransID.Trim());

                    MMSHost.ClosePort(this.sCurrentTransID);
                    this.UnRegisterEvent();
                }

                if (tcpChanel != null)
                {
                    foreach (System.Runtime.Remoting.Channels.IChannel oChannel in System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels)
                    {
                        if (oChannel.GetType() == typeof(System.Runtime.Remoting.Channels.Tcp.TcpChannel))
                        {
                            if (oChannel.ChannelName == "")
                            {
                                System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(tcpChanel);
                                break;
                            }
                        }
                    }
                }

                this.sCurrentTransID = "";

                oDefObj = null;
                tcpChanel = null;
                MMSHost = null;

                //Added By SRT (Gaurav) Date : 15 Oct 2008
                heartBeatTimer.Stop();
                heartBeatTimer.Enabled = false;
                heartBeatTimer = null;
                //End Of Added By SRT (Gaurav)
            }
            else
            {
                this.sCurrentTransID = "";
            }
            if (VF != null)
            {
                VF.ClosePort();
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "DisposeObjects()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("DisposeObjects() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        public void AbortTransaction()
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "AbortTransaction()", clsPOSDBConstants.Log_Entering);
            logger.Trace("AbortTransaction() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            if (isConnected())
            {
                int retVal = 0;
                if (this.isVF)
                {
                    ShowCustomScreen("Transaction Aborted");
                } else if (this.isPAX) {
                    ShowCustomScreen("Transaction Aborted");
                }
                else if (this.isEvertec)//PRIMEPOS-2664
                {
                    ShowCustomScreen("Transaction Aborted");
                }
                //PRIMEPOS-2636
                else if (this.isVantiv)
                {
                    ShowCustomScreen("Transaction Aborted");
                }
                //
                //PRIMEPOS-2943
                else if (this.isElavon)
                {
                    ShowCustomScreen("Transaction Aborted");
                }
                else
                {
                    retVal = MMSHost.AbortTxn(this.sCurrentTransID.Trim());
                }
                this.sCurrentTransID = "";
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "AbortTransaction()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("AbortTransaction() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        /*~SigPadUtil()
        {
            DisposeObjects();
        }*/

        public static void CloseDefaultInstace()
        {
            if (oDefObj != null)
            {
                oDefObj.DisposeObjects();
            }
        }

        private string GetCurrentScreen()
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "GetCurrentScreen()", clsPOSDBConstants.Log_Entering);
            logger.Trace("GetCurrentScreen() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            string curentScreen = string.Empty;
            try
            {
                if (isConnected() && !isISC)
                {
                    if (this.isVF)
                    {
                        deviceWorkin = true;
                        VF.GetCurrentScreen(this.sCurrentTransID.Trim(), out curentScreen);
                        deviceWorkin = false;
                    }
                    else if(this.isPAX)
                    {
                        // NIleshj - sCurrentTransID is not applicable for PAX handled
                    }
                    else if (this.isEvertec)
                    {
                        // PRIMEPOS-2664 - Device doesn't support this 
                    }
                    //PRIMEPOS-2636
                    else if (this.isVantiv)
                    {

                    }
                    //
                    else if (this.isElavon)//2943
                    {

                    }
                    else
                    {
                        MMSHost.GetCurrentScreen(this.sCurrentTransID.Trim(), out curentScreen);
                    }
                }
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "GetCurrentScreen()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("GetCurrentScreen() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "GetCurrentScreen()", clsPOSDBConstants.Log_Exception_Occured + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "GetCurrentScreen() - Sig Pad");
            }
            return curentScreen;
        }

        public void UpdateItem(CommonData.Rows.TransDetailRow oUpdateRow, int Index)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UpdateItem()", clsPOSDBConstants.Log_Entering);
            logger.Trace("UpdateItem() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            //Added by SRT (Dharmendra) for optimization.
            //Added by SRT till here.
            try
            {
                if (this.isVF)
                {
                    VF.UpdateItem(this.sCurrentTransID, oUpdateRow, Index, Configuration.CPOSSet.PrintRXDescription.ToString());
                }

                else if (isConnected() && !this.isVF)
                {
                    //Modified by SRT (Dharmendra)
                    //Modified the string to HashTable to increase performance.
                    //Added by shitaljit to make sure any field value is not null
                    Hashtable fields = new Hashtable(); //Added to avoid string formation and splitting time.
                    string ItemDescription = string.IsNullOrEmpty(oUpdateRow.ItemDescription.ToString().Trim()) ? "No Item Description" : oUpdateRow.ItemDescription.ToString();
                    string QTY = string.IsNullOrEmpty(oUpdateRow.QTY.ToString().Trim()) ? "0" : oUpdateRow.QTY.ToString();
                    string Price = string.IsNullOrEmpty(oUpdateRow.Price.ToString().Trim()) ? "0.00" : oUpdateRow.Price.ToString();
                    string ExtendedPrice = string.IsNullOrEmpty(oUpdateRow.ExtendedPrice.ToString().Trim()) ? "0.00" : oUpdateRow.ExtendedPrice.ToString();

                    oUpdateRow.ItemDescription = ItemDescription;
                    oUpdateRow.QTY = Configuration.convertNullToInt(QTY);
                    oUpdateRow.Price = Configuration.convertNullToDecimal(Price);
                    oUpdateRow.ExtendedPrice = Configuration.convertNullToDecimal(ExtendedPrice);

                    fields.Add(ITEMNAME, oUpdateRow.ItemDescription.ToString());
                    fields.Add(ITEMQTY, oUpdateRow.QTY.ToString());
                    fields.Add(UNITPRICE, oUpdateRow.Price.ToString("F", ci).PadRight(2, '0'));
                    fields.Add(TOTALPRICE, (oUpdateRow.ExtendedPrice - oUpdateRow.Discount).ToString("F", ci).PadRight(2, '0'));
                    //ADDED PRASHANT 12 AUG 2010
                    fields.Add(ISRX, oUpdateRow.IsRxItem ? "1" : "0");
                    fields.Add(SHOWRXDESCRIPTION, Configuration.CPOSSet.PrintRXDescription.ToString());
                    Decimal TotalAmountWithDis = oUpdateRow.ExtendedPrice - oUpdateRow.Discount;
                    fields.Add(WITHDISCAMOUNT, TotalAmountWithDis.ToString("F", ci).PadRight(2, '0')); //change by SRT (Sachin) Dated : 1st Nov 2010
                    //END ADDED PRASHANT 12 AUG 2010
                    //MMSHost.UpdateItem(this.sCurrentTransID.Trim(), oUpdateRow.TransDetailID-1, fields);

                    // Added by Manoj 10/12/2011 This is to return the correct index of the on hold and regular items
                    // Otherwise when you change the price of an item of an on hold item it will change the wrong item.
                    // Bug FIX - 10/12/2011
                    //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UpdateItem()", "CurrentTransID " + sCurrentTransID + " ItemId: " + oUpdateRow.ItemID + " ItemDescription: " + ItemDescription + " TransDetailID: " + oUpdateRow.TransDetailID);
                    logger.Trace("UpdateItem() - Sig Pad - CurrentTransID " + sCurrentTransID + " ItemId: " + oUpdateRow.ItemID + " ItemDescription: " + ItemDescription + " TransDetailID: " + oUpdateRow.TransDetailID);

                    if (!isPAX && !isEvertec && !isVantiv && !isElavon)//Added by Arvind PRIMEPOS-2636//PRIMEPOS-2664//2943
                    {
                        if (itemonholdID > 0 || isItemOnHold)
                        {
                            MMSHost.UpdateItem(this.sCurrentTransID.Trim(), Index, fields); // on hold Items
                        }
                        else
                        {
                            MMSHost.UpdateItem(this.sCurrentTransID.Trim(), Index, fields); // Regular items
                        }
                    }
                    else if (isEvertec)//PRIMEPOS-2664
                    {
                        //Added For EVERTEC 
                        //It does not Contain this Functionlity
                    }
                    //PRIMEPOS-2636 VANTIV
                    else if (isVantiv && Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_LINK_2500") //PRIMEPOS-3231
                    {
                        fields.Add("TAX", "" + oUpdateRow.TaxAmount);
                        fields.Add("DISCOUNT", "" + oUpdateRow.Discount);
                        fields.Add("INDEX", "" + Index);//PRIMEPOS-3126
                        VFD.UpdateItem(fields);
                    }
                    //
                    //PRIMEPOS-2943
                    else if (isElavon)
                    {
                        fields.Add("TAX", "" + oUpdateRow.TaxAmount);
                        fields.Add("DISCOUNT", "" + oUpdateRow.Discount);
                        ELV.UpdateItem(fields);
                    }
                    //
                    else if (Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_LINK_2500") //PRIMEPOS-3231
                    {
                        //Added For HPSPAX To ShowOnly Discount
                        fields.Add("DISCOUNT", "" + oUpdateRow.Discount);
                        fields.Add("INDEX", "" + Index);//PRIMEPOS-2931
                        PD.UpdateItem(fields);
                    }
                    //Modified by SRT (Dharmendra) till here.
                }
            }
            catch (Exception Ex)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\t", "UpdateItem()", clsPOSDBConstants.Log_Exception_Occured + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "UpdateItem() - Sig Pad");
                clsCoreUIHelper.ShowErrorMsg("Error while sending items details to signature pad.");
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UpdateItem()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("UpdateItem() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        public void DeleteItemHPSPaxAX(int nRowID, TransDetailRow oTDRow) //PRIMEPOS-2952
        {
            logger.Trace("DeleteItemHPSPaxAX() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            if (isConnected())
            {
                Hashtable PData = new Hashtable();
                PData.Add("CurrentTransID", this.sCurrentTransID.Trim());
                PData.Add("RowID", nRowID);
                //oNewRow.ItemDescription = string.IsNullOrEmpty(oNewRow.ItemDescription.Trim()) ? "Item Description Not Set" : oNewRow.ItemDescription.Trim();
                //CommonData.Rows.TransDetailRow row = null;
                PData.Add(ITEMNAME, oTDRow.ItemDescription.ToString().Trim());
                PData.Add(ITEMQTY, oTDRow.QTY.ToString().Trim());
                PData.Add(UNITPRICE, oTDRow.Price.ToString().Trim());
                PData.Add(DISCOUNT, oTDRow.Discount.ToString().Trim());
                PD.DeleteItem(PData);
            }
            logger.Trace("DeleteItemHPSPaxAX() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        public void DeleteItemHPSPax7(int nRowID, TransDetailRow oTDRow) //PRIMEPOS-2952
        {
            logger.Trace("DeleteItemHPSPaxAX() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            if (isConnected())
            {
                Hashtable PData = new Hashtable();
                PData.Add("CurrentTransID", this.sCurrentTransID.Trim());
                PData.Add("RowID", nRowID);
                //oNewRow.ItemDescription = string.IsNullOrEmpty(oNewRow.ItemDescription.Trim()) ? "Item Description Not Set" : oNewRow.ItemDescription.Trim();
                //CommonData.Rows.TransDetailRow row = null;
                PData.Add(ITEMNAME, oTDRow.ItemDescription.ToString().Trim());
                PData.Add(ITEMQTY, oTDRow.QTY.ToString().Trim());
                PData.Add(UNITPRICE, oTDRow.Price.ToString().Trim());
                PData.Add(DISCOUNT, oTDRow.Discount.ToString().Trim());
                PD.DeleteItem(PData);
            }
            logger.Trace("DeleteItemHPSPax7() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        public void DeleteItem(int nRowID) //Modified by SRT (Dharmendra) parameter from String to int.
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "DeleteItem() Enter", clsPOSDBConstants.Log_Entering);
            logger.Trace("DeleteItem() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            if (isConnected())
            {
                if (this.isVF)
                {
                    deviceWorkin = true;
                    VF.DeleteItem(this.sCurrentTransID.Trim(), nRowID);
                    deviceWorkin = false;
                }
                else if (this.isPAX)
                {
                    Hashtable PData = new Hashtable();
                    PData.Add("CurrentTransID", this.sCurrentTransID.Trim());
                    PData.Add("RowID", nRowID);
                    PD.DeleteItem(PData);
                }
                else if (this.isEvertec)
                {
                    //Empty PRIMEPOS-2664
                }
                //PRIMEPOS-2636 ADDED BY ARVIND 
                else if (this.isVantiv && Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_LINK_2500") //PRIMEPOS-3231
                {
                    Hashtable PData = new Hashtable();
                    PData.Add("CurrentTransID", this.sCurrentTransID.Trim());
                    PData.Add("RowID", nRowID);
                    VFD.DeleteItem(PData);
                }
                //
                //PRIMEPOS-2943 ADDED BY ARVIND 
                else if (this.isElavon)
                {
                    Hashtable PData = new Hashtable();
                    PData.Add("CurrentTransID", this.sCurrentTransID.Trim());
                    PData.Add("RowID", nRowID);
                    ELV.DeleteItem(PData);
                }
                //
                else if (Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_LINK_2500") //PRIMEPOS-3231
                {
                    MMSHost.DeleteItem(this.sCurrentTransID.Trim(), nRowID);  //Changed the MMS.HOST interface method parameter type by SRT.
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "DeleteItem() Exit", clsPOSDBConstants.Log_Exiting);
            logger.Trace("DeleteItem() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        public void UpdateTransSummary(string GrossAmount, string DiscAmount, string TaxAmount, string NetAmount, string itemCount) //string itemCount Added By Dharmendra on Oct- 09-08
        {
            try
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UpdateTransSummary() Enter " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond, clsPOSDBConstants.Log_Entering);
                logger.Trace("UpdateTransSummary() - Sig Pad - " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + " " + clsPOSDBConstants.Log_Entering);
                if (isConnected())
                {
                    //Added By Shitaljit to make sure no null value is pass to PAD
                    string itemSumRow = string.Empty;
                    GrossAmount = string.IsNullOrEmpty(GrossAmount.Trim()) ? "0.00" : GrossAmount.Trim();
                    DiscAmount = string.IsNullOrEmpty(DiscAmount.Trim()) ? "0.00" : DiscAmount.Trim();
                    TaxAmount = string.IsNullOrEmpty(TaxAmount.Trim()) ? "0.00" : TaxAmount.Trim();
                    NetAmount = string.IsNullOrEmpty(NetAmount.Trim()) ? "0.00" : NetAmount.Trim();
                    itemCount = string.IsNullOrEmpty(itemCount.Trim()) ? "0" : itemCount.Trim();

                    itemSumRow = GrossAmount.Trim() + DELIM_CHARACTER + DiscAmount.Trim();
                    itemSumRow += DELIM_CHARACTER + TaxAmount.Trim() + DELIM_CHARACTER + NetAmount.Trim() + DELIM_CHARACTER + itemCount; //Added additioanl itemCount in the message by SRT.

                    if (this.isVF)
                    {
                        deviceWorkin = true;
                        VF.UpdateSum(this.sCurrentTransID.Trim(), itemSumRow);
                        deviceWorkin = false;
                    } else if (this.isPAX) {
                        //Does nothing whole tax row is updated every time
                    }
                    else if (this.isEvertec)
                    {
                        //Does nothing whole tax row is updated every time PRIMEPOS-2664
                    }
                    //PRIMEPOS-2636
                    else if (this.isVantiv)
                    {
                        //Empty needs to be filled
                    }
                    //  
                    //PRIMEPOS-2943
                    else if (this.isElavon)
                    {
                        //Empty needs to be filled
                    }
                    //  
                    else
                    {
                        MMSHost.UpdateSum(this.sCurrentTransID.Trim(), itemSumRow);
                    }
                }
            }
            catch (Exception Ex)
            {
                deviceWorkin = false;
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UpdateTransSummary() \n>>>>Error: ", clsPOSDBConstants.Log_Exception_Occured + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "UpdateTransSummary() - Sig Pad");
                clsCoreUIHelper.ShowErrorMsg("Error while sending transaction summary to signature pad.");
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UpdateTransSummary() Exit " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond, clsPOSDBConstants.Log_Exiting);
            logger.Trace("UpdateTransSummary() - Sig Pad - " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond + " " + clsPOSDBConstants.Log_Exiting);
        }

        //Added by Manoj to check if the Pad is already running if not start it.
        private bool CheckIfPadStart()
        {
            bool isStart = false;
            bool start = false;

            try
            {
                if (Configuration.CPOSSet.UseSigPad)
                {
                    bool Pfound = false;
                    /* Check if the process is running */
                    Process[] PArray = Process.GetProcesses();
                    foreach (Process p in PArray)
                    {
                        string s = p.ProcessName;
                        if (s.CompareTo("MMS.HOSTSRV") == 0 || s.CompareTo("MMS.HOSTSRV.vshost") == 0)
                        {
                            Pfound = true; // process is running
                        }
                    }

                    if (!Pfound)
                    {
                        string FileName = "C:\\MMS.HOSTSRVPATH.ini";
                        isPosStartWithPad = false;
                        if (File.Exists(FileName))
                        {
                            string[] path = null;
                            using (var reader = new StreamReader(FileName))
                            {
                                string line;
                                while ((line = reader.ReadLine()) != null)
                                {
                                    path = line.Split('=');
                                }
                            }
                            if (path != null && path.Length > 1)
                            {
                                Process.Start(path[1]);
                                isStart = true;
                                if (isAlreadyOn)
                                {
                                    clsCoreUIHelper.ShowErrorMsg("If you have any item in the Transaction screen \nplease press CLEAR ALL and redo the transaction.");
                                }
                            }
                        }
                    }
                    else
                    {
                        isAlreadyOn = true;
                    }
                }
                if (!isAlreadyOn && isStart)
                    start = isStart;
                else if (isAlreadyOn && !isStart)
                    start = isAlreadyOn;
            }
            catch (Exception Ex)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CheckIfPadStart() \n>>>>Error", clsPOSDBConstants.Log_Exception_Occured + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "CheckIfPadStart() - Sig Pad");
            }
            return isStart;
        }

        //Added by Manoj to restart the Pad. 3/11/2013
        private bool ReStart()
        {
            bool isReStart = false;
            try
            {
                if (CheckIfPadStart())
                {
                    ConnectServer();
                    mcs = new POSCallbackClass(this);
                    if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("PRIMEPOSHOSTSRV"))
                    {
                        UnRegisterEvent();
                        RegisterEvent();
                    }
                    InitiateTimers();
                    isReStart = true;
                    isPosStartWithPad = false;
                }
                else if (isAlreadyOn && isConnectedToServer)
                {
                    isReStart = true;
                }
                else if (isAlreadyOn && !isConnectedToServer)
                {
                    ConnectServer();
                    mcs = new POSCallbackClass(this);
                    if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("PRIMEPOSHOSTSRV"))
                    {
                        UnRegisterEvent();
                        RegisterEvent();
                    }
                    InitiateTimers();
                    isReStart = true;
                    isPosStartWithPad = false;
                }
            }
            catch (Exception Exp)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "ReStart() \n>>>>Error: ", clsPOSDBConstants.Log_Exception_Occured + Exp.StackTrace.ToString());
                logger.Fatal(Exp, "ReStart() - Sig Pad");
            }

            return isReStart;
        }

        public bool isConnected()
        {
            try
            {
                if (this.isVF)
                {
                    return true;
                }

                if (this.isPAX)
                {
                    return true;
                }
                if (this.isEvertec)//PRIMEPOS-2664
                {
                    return true;
                }
                if (this.isVantiv)//ARVIND PRIMEPOS-2636
                {
                    return true;
                }
                if (this.isElavon)//ARVIND PRIMEPOS-2943
                {
                    return true;
                }
                if (MMSHost == null)
                {
                    if (ReStart())
                    {
                        //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "Pad Restarted: ", "true, MMSHost was null");
                        logger.Trace("isConnected() - Sig Pad - Pad Restarted: true, MMSHost was null");
                        return true;
                    }
                    else
                    {
                        //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "Pad is OFF, false ", " MMSHost == null");
                        logger.Trace("isConnected() - Sig Pad - Pad is OFF, false MMSHost == null");
                        return false;
                    }
                }
                else if (MMSHost.PadIsConnect(this.sCurrentTransID))
                {
                    CheckIfPadStart();
                    return true;
                }
                else
                {
                    if (ReStart())
                    {
                        //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "Pad Restarted: ", "true, MMSHost != null");
                        logger.Trace("isConnected() - Sig Pad - Pad Restarted: true, MMSHost != null");
                        return true;
                    }
                    else
                    {
                        //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "Pad is OFF, false ", " MMSHost != null");
                        logger.Trace("isConnected() - Sig Pad - Pad is OFF, false MMSHost != null");
                        return false;
                    }
                }
            }
            catch (Exception Ex)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "isConnected() \n>>>>Error: ", clsPOSDBConstants.Log_Exception_Occured + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "isConnected() - Sig Pad");
                if (ReStart())
                {
                    return true;
                }
                else return false;
            }
        }

        private void ConnectServer()
        {
            //string hostPath = String.Empty;
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "ConnectServer()", clsPOSDBConstants.Log_Entering);
            logger.Trace("ConnectServer() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            isISC = false;
            if (Configuration.CPOSSet.UseSigPad)
            {
                if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("VERIFONEPOSHOSTSRV"))
                {
                    VF = new MMS.Device.Communication();
                    if (VF != null)
                    {
                        isVF = true;
                        VF.DeviceName = Configuration.CPOSSet.PinPadModel;
                        VF.ReturnedErrorEvents += VF_ReturnedErrorEvents;
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("Signature Pad Failed to start");
                    }
                }
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("ISCPOSHOSTSRV")) // Add Device Name
                {
                    VF = new VFDevice(Configuration.CPOSSet.PinPadModel, Convert.ToInt32(Configuration.CPOSSet.PinPadPortNo));
                    if (VF != null)
                    {
                        isVF = true;
                        VF.DeviceName = Configuration.CPOSSet.PinPadModel;
                        SigType = "M";
                        isISC = true;
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("Signature Pad Failed to start");
                    }
                }
                //Suraj
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("HPSPAX")) // Add Device Name
                {
                    PD = PAXPx7Device.DefaultInstance(Configuration.CPOSSet.SigPadHostAddr, Configuration.CPOSSet.PinPadModel); //Aries8
                    if (PD != null)
                    {
                        isPAX = true;
                        SigType = clsPOSDBConstants.BINARYIMAGE;
                        isVF = false;
                        isISC = false;
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("Signature Pad Failed to start");
                    }
                }
                //
                //PRIMEPOS-2664
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("EVERTEC")) // Add Device Name
                {
                    EV = EvertecVx820Device.DefaultInstance(Configuration.CPOSSet.SigPadHostAddr, Configuration.CPOSSet.TerminalID, Configuration.StationID, Configuration.CashierID);
                    if (EV != null)
                    {
                        isEvertec = true;
                        SigType = clsPOSDBConstants.BINARYIMAGE;
                        isVF = false;
                        isISC = false;
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("Signature Pad Failed to start");
                    }
                }
                //Arvind PRIMEPOS-2636
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("V1")) // Add Device Name
                {
                    VFD = VerifoneMx925Device.DefaultInstance(Configuration.CPOSSet.SigPadHostAddr, Configuration.ApplicationName, Configuration.StationID,Configuration.CSetting.VantivDelayInSecond, Configuration.GetConfigFilePath());//2895 Arvind
                    if (VFD != null)
                    {
                        isVantiv = true;
                        SigType = clsPOSDBConstants.BINARYIMAGE;
                        isVF = false;
                        isISC = false;
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("Signature Pad Failed to start");
                    }
                }
                //
                //Arvind PRIMEPOS-2943
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("ELAVON")) // Add Device Name
                {
                    ELV = ElavonLink8000.DefaultInstance(Configuration.CPOSSet.SigPadHostAddr);
                    if (ELV != null)
                    {
                        isElavon = true;
                        SigType = clsPOSDBConstants.BINARYIMAGE;
                        isVF = false;
                        isISC = false;
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("Signature Pad Failed to start");
                    }
                }
                //
                else
                {
                    if (!CheckIfPadStart() || !isAlreadyOn)
                    {
                        isPosStartWithPad = true;
                        isConnectedToServer = true;
                        TcpChannel tcpChanel = null;
                        string hostPath = String.Empty;
                        Hashtable channelProperties = new Hashtable();
                        channelProperties["name"] = "TcpHHP";
                        channelProperties["port"] = 0;
                        channelProperties["typeFilterLevel"] = "Full";

                        tcpChanel = new TcpChannel(channelProperties,
                            new BinaryClientFormatterSinkProvider(),
                            new BinaryServerFormatterSinkProvider());
                        if (RemotingConfiguration.ApplicationName != "MMSClient")
                            RemotingConfiguration.ApplicationName = "MMSClient";
                        foreach (System.Runtime.Remoting.Channels.IChannel oChannel in System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels)
                        {
                            if (oChannel.GetType() == typeof(System.Runtime.Remoting.Channels.Tcp.TcpChannel))
                            {//if (oChannel.ChannelName == "tcp")
                                if (oChannel.ChannelName == "TcpHHP")
                                {
                                    System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(oChannel);
                                    break;
                                }
                            }
                        }
                        if (ChannelServices.GetChannel(tcpChanel.ToString()) == null)
                            ChannelServices.RegisterChannel(tcpChanel, false);
                        //hostPath = "tcp://localhost:8080/PrimePosHostSrv";
                        MMSHost = (IHost)Activator.GetObject(typeof(IHost), Configuration.CPOSSet.SigPadHostAddr); // Added By Prashant(SRT) 23-sep-2010
                    }
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "ConnectServer()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("ConnectServer() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        void VF_ReturnedErrorEvents(bool b, int i, string d)
        {
            VF.ReturnedErrorEvents -= VF_ReturnedErrorEvents;
            if (b)
            {
                if (VF.DisplayVFReturnCode)
                {
                   clsCoreUIHelper.ShowErrorMsg("Verifone MX Error: " + i + " - " + d);
                }
                VF.ReInitialize();
                VF.ReturnedErrorEvents += VF_ReturnedErrorEvents;
            }
        }

        public void RegisterEvent()
        {
            try
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "RegisterEvent()", clsPOSDBConstants.Log_Entering);
                logger.Trace("RegisterEvent() - Sig Pad" + clsPOSDBConstants.Log_Entering);

                MMSHost.SignEvent -= new EventSignHandler(mcs.MMSHost_Sign);
                MMSHost.SignEvent += new EventSignHandler(mcs.MMSHost_Sign);
                MMSHost.CreditCardEvent -= new EventCCHandler(mcs.MMSHost_CC);
                MMSHost.CreditCardEvent += new EventCCHandler(mcs.MMSHost_CC);
                MMSHost.NoppEvent -= new EventNoppHandler(mcs.MMSHost_NOPP);
                MMSHost.NoppEvent += new EventNoppHandler(mcs.MMSHost_NOPP);
                MMSHost.RxApproveEvent -= new EventRxApproveHandler(mcs.MMSHost_RXSIGN);
                MMSHost.RxApproveEvent += new EventRxApproveHandler(mcs.MMSHost_RXSIGN);
                MMSHost.HeartBeatEvent -= new EventHeartBeatHandler(mcs.MMSHost_HeartBeat); // Added By Dharmendra (SRT) on Nov - 11-08 to unregister event
                MMSHost.HeartBeatEvent += new EventHeartBeatHandler(mcs.MMSHost_HeartBeat); // Added By Dharmendra (SRT) on Nov - 11-08 to register event

                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "RegisterEvent()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("RegisterEvent() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Exp)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "RegisterEvent()", clsPOSDBConstants.Log_Exception_Occured + Exp.StackTrace.ToString());
                logger.Fatal(Exp, "RegisterEvent() - Sig Pad");
            }
        }

        /// <summary>
        /// This is use in place of Remoting events. Added by Manoj
        /// </summary>
        public void PaDResp()
        {
            try
            {
                lock (DatafromPad)
                {
                    if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("VERIFONEPOSHOSTSRV") || this.isVF)
                    {
                        if (VF.IsPadAck && !isInDevice) //PadHost ack that it has a signature
                        {
                            isInDevice = true;
                            //if (IsDialogClose)
                            //{
                            //    IsDialogClose = false;
                            //    VF.isMsrCancel = true;
                            //    VF.IsPosAck = false;

                            //    frmWaitScreen ofrmWait = new frmWaitScreen("Credit Card Process", "Please Wait..", WaitFor.CreditCardProcess);
                            //    ofrmWait.SetMsgDetails("Capturing credit card information");
                            //    return;
                            //}

                            Hashtable padData = new Hashtable();
                            padData = VF.PadData(); //Signature is being pass in a hashtable
                            if (padData.Count != 0 && !string.IsNullOrEmpty(padData["DataEventType"].ToString())) //make sure its not null or empty
                            {
                                switch (padData["DataEventType"].ToString().ToUpper().Trim())
                                {
                                    /* Pad events */
                                    case "EVENTOTC":
                                    case "EVENTSIGN":
                                    case "EVENTNOPP":
                                    case "EVENTRXAPPROVE":
                                        {
                                            if (!string.IsNullOrEmpty(padData["Sign"].ToString()) || !string.IsNullOrEmpty(padData["SigType"].ToString())) //not null or empty
                                            {
                                                strSignature = padData["Sign"].ToString(); //Signature data
                                                this.sigType = padData["SigType"].ToString().Trim();

                                                if (strSignature != "" && this.strsigtype != "")
                                                    VF.IsPosAck = true; //ack that POS received the signature
                                                else
                                                    VF.IsPosAck = false;
                                            }
                                            else
                                            {
                                                VF.IsPosAck = false;
                                            }
                                        }
                                        break;
                                    case "EVENTCC": //Credit & Debit card
                                        {
                                            if (padData["CCInfo"] != null) // not null
                                            {
                                                ArrayList CC = new ArrayList();
                                                CC = (ArrayList)padData["CCInfo"];

                                                if (CC != null && CC.Count > 0) //Make sure CC is not null
                                                {
                                                    MMSHost_CreditCardEvent(ref CC);
                                                    VF.IsPosAck = true;
                                                }
                                                else
                                                {
                                                    VF.IsPosAck = false;
                                                }
                                            }
                                            else
                                            {
                                                VF.IsPosAck = false;
                                            }
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                VF.IsPosAck = false;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "PaDResp()", clsPOSDBConstants.Log_Exception_Occured + Ex.StackTrace);
                logger.Fatal(Ex, "PaDResp() - Sig Pad");
                VF.IsPosAck = false;
            }
            finally
            {
                isInDevice = false;
            }
        }

        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method is the heart beat event handler which is raised by Hostserver
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : Nov - 11 - 08
        /// </summary>
        /// <param name="sHeartBeatCalled"></param>
        public void MMSHost_HeartBeatEvent(ref string sHeartBeatCalled)
        {
            // This code block is intentinally left blank
            // Do not remove this code block
        }

        public void MMSHost_RxApproveEvent(ref string Signature, string sigtype)
        {
            try
            {
                strSignature = Signature;
                this.sigType = sigtype;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "MMSHost_RxApproveEvent()");
            }
        }

        private void UnRegisterEvent()
        {
            try
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UnRegisterEvent()", clsPOSDBConstants.Log_Entering);
                logger.Trace("UnRegisterEvent() - Sig Pad" + clsPOSDBConstants.Log_Entering);

                MMSHost.SignEvent -= new EventSignHandler(mcs.MMSHost_Sign);
                MMSHost.CreditCardEvent -= new EventCCHandler(mcs.MMSHost_CC);
                MMSHost.NoppEvent -= new EventNoppHandler(mcs.MMSHost_NOPP);
                MMSHost.RxApproveEvent -= new EventRxApproveHandler(mcs.MMSHost_RXSIGN);
                //Added By SRT (Gaurav) Date : 15 Oct 2008
                heartBeatTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);
                //End Of Added By SRT (Gaurav)
                MMSHost.HeartBeatEvent -= new EventHeartBeatHandler(mcs.MMSHost_HeartBeat); // Added By Dharmendra (SRT) on Nov - 11-08 to unregister event

                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UnRegisterEvent()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("UnRegisterEvent() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "UnRegisterEvent()", clsPOSDBConstants.Log_Exception_Occured + Ex.StackTrace.ToString());
                logger.Fatal(Ex, "UnRegisterEvent() - Sig Pad");
            }
        }

        public void MMSHost_SignEvent(ref string Data, string signOpt, string sigType /*Added by RiteshMx*/)
        {
            try  //Added by prashant 17-sep-2010
            {
                strSignature = Data;
                this.sigType = sigType;/*Added by RiteshMx*/
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "MMSHost_SignEvent()");
            } //Added by prashant 17-sep-2010
        }

        public void MMSHost_NoppEvent(ref string Data, string sigType /*Added by RiteshMx*/)
        {
            try
            {
                strSignature = Data;
                this.sigType = sigType;/*Added by RiteshMx*/
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "MMSHost_NoppEvent()");
            }
        }

        //Added by Manoj.
        private string PinBlock = string.Empty;

        public string PINBLOCK
        {
            get { return PinBlock; }
            set { PinBlock = value; }
        }

        public void MMSHost_CreditCardEvent(ref ArrayList Data)
        {
            //Added by Manoj
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "MMSHost_CreditCardEvent()", clsPOSDBConstants.Log_Entering);
            logger.Trace("MMSHost_CreditCardEvent() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            if (Data.Count.Equals(2) && Data[0].ToString().Equals("PINBLOCK"))
            {
                PINBLOCK = Data[1].ToString();
            }

            SigPadCardData oData = new SigPadCardData();
            if (Data.Count.Equals(1) && Data[0].ToString().Equals(SWIPEATTEMPTEXCEED))
            {
                oData.errorNumber = Data[0].ToString();
                oData.errorMessage = "Error:" + Data[0].ToString() + "\r\nSwipe Attempt Exceeded";
                oData.IsValidData = false;
                // Add Ended
            }
            if (Data.Count.Equals(7))
            {
                if (Data[0].ToString() == "\0")
                {
                    //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "MMSHost_CreditCardEvent()", clsPOSDBConstants.Log_Exiting);
                    logger.Trace("MMSHost_CreditCardEvent() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
                    return;
                }
                oData.CardNo = Data[0].ToString().Trim();
                oData.ExpireOn = Data[1].ToString().Trim();
                oData.FirstName = Data[2].ToString().Trim();
                oData.LastName = Data[3].ToString().Trim();
                //oData.Track1 = Data[4].ToString().Trim();
                //if(Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().EndsWith("VerifonePosHostSrv"))
                if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().EndsWith("VERIFONEPOSHOSTSRV"))
                {
                    oData.Completetrack = "%" + Data[4].ToString().Trim() + "?;" + Data[5].ToString().Trim() + "?";
                }
                else if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().EndsWith("PRIMEPOSHOSTSRV")) //else if(Configuration.CPOSSet.SigPadHostAddr.EndsWith("PrimePosHostSrv"))
                {
                    oData.Completetrack = Data[4].ToString().Trim();
                }
                //Added By SRT
                string trackInfo = "";
                char[] sep = { ';' };
                string[] trackData = Data[5].ToString().Split(sep);
                if (trackData.Length > 1)
                    trackInfo = trackData[1].ToString().Replace('?', ' ').Trim();
                else
                    trackInfo = trackData[0].ToString().Replace('?', ' ').Trim();
                oData.Track2 = trackInfo;

                //Added By SRT
                oData.Track3 = Data[6].ToString().Trim();
            }

            if (oData.ExpireOn != null && oData.ExpireOn.Length == 4)
            {
                oData.ExpireOn = oData.ExpireOn.Substring(2, 2) + oData.ExpireOn.Substring(0, 2);
            }

            oSigPadCardData = oData;
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "MMSHost_CreditCardEvent()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("MMSHost_CreditCardEvent() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
        }

        public void ShowCustomScreen(string strData)
        {
            try
            {
                if (isConnected() == true)
                {
                    if (this.isVF) {
                        deviceWorkin = true;
                        VF.DisplayScreen(this.sCurrentTransID, "TXTMESSAGE_SCREEN", strData);
                        deviceWorkin = false;
                    } else if (this.isPAX) {
                        PD.DisplayScreen(this.sCurrentTransID, "TXTMESSAGE_SCREEN", strData);
                    }
                    else if (this.isEvertec)
                    {
                        //PRIMEPOS-2664
                        //Do nothing as the Evertec Device does not contain the Functionality for displaying
                        //EV.DisplayScreen(this.sCurrentTransID, "TXTMESSAGE_SCREEN", strData);
                    }
                    else if (this.isVantiv)//Arvind PRIMEPOS-2636
                    {
                        VFD.DisplayScreen(this.sCurrentTransID, "TXTMESSAGE_SCREEN", strData);
                    }
                    else if (this.isElavon)//2943
                    {
                        ELV.DisplayScreen(this.sCurrentTransID, "TXTMESSAGE_SCREEN", strData);
                    }
                    else
                    {
                        MMSHost.DisplayScreen(this.sCurrentTransID, "TXTMESSAGE_SCREEN", strData);
                    }
                }
            }
            catch (Exception Ex)
            {
                if (this.isVF)
                    deviceWorkin = false;
                logger.Fatal(Ex, "ShowCustomScreen()");
            }
        }

        /* Add to Set PAd for XLINK by Manoj 8/25/2011*/

        public void XlinkOnOff(string OnOff)
        {
            try
            {
                if (this.isVF)
                {
                    if (isISC && OnOff.ToUpper() == "ON")
                    {
                        VF.Disconnect();
                    }
                    else if (isISC && OnOff.ToUpper() == "OFF")
                    {
                        ConnectServer();
                    }
                    else
                    {
                        deviceWorkin = true;
                        VF.DisplayScreen(this.sCurrentTransID, "XLINK", OnOff);
                        deviceWorkin = false;
                    }
                }
                else
                {
                    MMSHost.DisplayScreen(this.sCurrentTransID, "XLINK", OnOff);
                }
            }
            catch (Exception Ex)
            {
                if (this.isVF)
                    deviceWorkin = false;

                logger.Fatal(Ex, "XlinkOnOff()");
            }
        }

        public void ShowItemsScreen()
        {
            try
            {
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "ShowItemsScreen()", clsPOSDBConstants.Log_Entering);
                logger.Trace("ShowItemsScreen() - Sig Pad" + clsPOSDBConstants.Log_Entering);
                if (isConnected() == true)
                {
                    string scrName = string.Empty;
                    scrName = GetCurrentScreen();
                    if (!scrName.ToUpper().Equals(ITEMSCREEN.ToUpper()))
                    {
                        if (this.isVF) {
                            deviceWorkin = true;
                            VF.ResendAllItem(this.sCurrentTransID, "ITEMLIST_SCREEN");
                            deviceWorkin = false;
                        } else if (this.isPAX) {
                            PD.ResendAllItem();
                        }
                        else if (this.isEvertec)
                        {
                            //Does not Implement this Functionality PRIMEPOS-2664
                            //EV.ResendAllItem();
                        }
                        //PRIMEPOS-2636
                        else if (this.isVantiv && Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_LINK_2500") //PRIMEPOS-3231
                        {
                            VFD.ResendAllItem();
                        }
                        else if (this.isElavon)//2943
                        {
                            ELV.ResendAllItem();
                        }
                        else if (Configuration.CPOSSet.PinPadModel.Trim().ToUpper() != "VANTIV_LINK_2500") //PRIMEPOS-3231
                        {
                            MMSHost.ResendAllItems(this.sCurrentTransID, "ITEMLIST_SCREEN");
                        }
                    }
                }
                //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "ShowItemsScreen()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("ShowItemsScreen() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ShowItemsScreen()");
            }
            finally
            {
                if (this.isVF)
                    deviceWorkin = false;
            }
        }

        /// <summary>
        /// Author: Manoj Kumar
        /// Description: PIN from mx870
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>

        public bool CaptureDebitPinBlock(string CardNum) // Add to capture the PINBLOCK for HPS
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureDebitPinBlock()", clsPOSDBConstants.Log_Entering);
            logger.Trace("CaptureDebitPinBlock() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            PINBLOCK = string.Empty;
            if (isConnected())
            {
                ArrayList msglist = new ArrayList();
                msglist.Add("PinEntry");
                msglist.Add(CardNum);
                if (this.isVF)
                {
                    deviceWorkin = true;
                    VF.SetMessage(this.sCurrentTransID.Trim(), "PIN_PAD", msglist);
                    deviceWorkin = false;
                }
                else
                {
                    MMSHost.SetMessage(this.sCurrentTransID.Trim(), "PIN_PAD", msglist);
                }
                frmWaitScreen ofrmWait = new frmWaitScreen("Debit Card Pin", "Please Wait..", WaitFor.CapturePinBlock);
                ofrmWait.SetMsgDetails("Capturing Debit Card Pin Number");
                if (ofrmWait.ShowDialog(this) == DialogResult.Cancel)
                {
                    this.oSigPadCardData = null;
                    if (this.isVF)
                    {
                        deviceWorkin = true;
                        VF.DisablePinPad();
                        deviceWorkin = false;
                    }
                    ShowCustomScreen("Card Processing Cancelled.Please wait...");
                    oSigPadCardData = new SigPadCardData();
                    //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureDebitPinBlock()", clsPOSDBConstants.Log_Exiting);
                    logger.Trace("CaptureDebitPinBlock() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
                    return false;
                }
                else
                {
                    if (this.PINBLOCK != string.Empty)
                    {
                        ShowCustomScreen("Processing Payment. Please wait...");
                        //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureDebitPinBlock()", clsPOSDBConstants.Log_Exiting);
                        logger.Trace("CaptureDebitPinBlock() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureDebitPinBlock()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CaptureDebitPinBlock() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            return true;
        }

        public bool CaptureCreditCardInfo(string sAmount, string paymentTypeSelected)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureCreditCardInfo()", clsPOSDBConstants.Log_Entering);
            logger.Trace("CaptureCreditCardInfo() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            bool retValue = false;
            this.paymentType = paymentTypeSelected;
            SigPadUtil.DefaultInstance.SigPadCardInfo = null;
            if (isConnected() == true)
            {
                ArrayList msgList = new ArrayList();
                string cardOption = Convert.ToString(Card.CC);
                msgList.Add("Swipe");
                msgList.Add(sAmount);
                msgList.Add(string.Empty);

                if (this.isVF)
                {
                    deviceWorkin = true;
                    VF.SetMessage(this.sCurrentTransID.Trim(), "SWIPE_SCREEN", msgList);
                    deviceWorkin = false;
                }
                else
                {
                    MMSHost.SetMessage(this.sCurrentTransID.Trim(), "SWIPE_SCREEN", msgList);
                    MMSHost.ReadCardDetails(this.sCurrentTransID.Trim(), cardOption);
                }

                frmWaitScreen ofrmWait = new frmWaitScreen("Credit Card Process", "Please Wait..", WaitFor.CreditCardProcess);
                ofrmWait.SetMsgDetails("Capturing credit card information");
                if (ofrmWait.ShowDialog(this) == DialogResult.Cancel)
                {
                    this.oSigPadCardData = null;
                    if (!this.isVF)
                        MMSHost.CancelOperation(this.sCurrentTransID, CARDSWIPECANCEL);

                    ShowCustomScreen("Card Processing Cancelled.Please wait..."); // Modified by SRT the message to be displayed.
                    oSigPadCardData = new SigPadCardData();
                    retValue = false; // 29-09-08 Added By Dharmendra (SRT)
                }
                else
                {
                    if (this.oSigPadCardData != null)
                    {
                        //Modified By Dharmendra(SRT) to ensure the value of IsValiData
                        if (this.oSigPadCardData.IsValidData)
                        {
                            if (paymentTypeSelected != "DB" && paymentTypeSelected != "BT")
                            {
                                ShowCustomScreen("Processing Payment. Please wait...");
                            }
                            retValue = true;
                        }
                        else
                        {
                            if (this.oSigPadCardData.errorNumber.Equals(SWIPEATTEMPTEXCEED))
                            {
                                clsCoreUIHelper.ShowBtnErrorMsg(this.oSigPadCardData.errorMessage, "Card Swipe Failure", MessageBoxButtons.OK);
                                ShowCustomScreen("Processing Card Payment. Please wait...");
                            }
                            retValue = false;
                        }
                        //End Modified
                    }
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureCreditCardInfo()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CaptureCreditCardInfo() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            return retValue;
        }

        public bool WPCaptureSignature(WPResponse WpResp)
        {
            bool isSign = false;
            if (WpResp.Status.ToUpper() == "APPROVED")
            {
                ArrayList msgList = new ArrayList();
                msgList.Add(WpResp.TotalAmt);
                if (this.isVF)
                {
                    deviceWorkin = true;

                    VF.SetMessage(this.sCurrentTransID, "SIGN_SCREEN", msgList);

                    deviceWorkin = false;
                }

                string msg = string.Empty;
                if (WpResp.PartialApproval == "1")
                {
                    msg = "Capturing Signature for Partial Amount: $" + WpResp.TotalAmt;
                }
                else
                {
                    msg = "Capturing Signature.  Please wait...";
                }

                frmWaitScreen ofrmwait = new frmWaitScreen(true, "Capture Signature", msg);
                ofrmwait.Show();
                while (VF.IsSignValid == null)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    if (ofrmwait.IsDisposed)
                    {
                        break;
                    }
                }
                ofrmwait.Close();
                if (VF.IsSignValid == true)
                {
                    this.strSignature = VF.GetSignatureString;
                    VF.GetSignatureString = string.Empty;
                    isSign = true;
                }
                else if (VF.IsSignValid == false)
                {
                    this.strSignature = string.Empty;
                    ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                }
                VF.IsSignValid = null;

                if (WpResp.PartialApproval == "1")
                {
                    ShowCustomScreen("Partial Approved Amount: $" + WpResp.TotalAmt + " Please provide another source of payment.");
                }
            }
            else if (WpResp.Status.ToUpper() == "DECLINED")
            {
                ShowCustomScreen("Payment Method: " + WpResp.PayType + " is Declined");
            }
            return isSign;
        }

        public bool CaptureSignature(string strMessage)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureSignature()", clsPOSDBConstants.Log_Entering);
            logger.Trace("CaptureSignature() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            strSignature = "";
            bool retValue = false;
            if (isConnected() == true)
            {
                ArrayList msgList = new ArrayList();
                msgList.Add(strMessage);

                if (this.isVF) {
                    deviceWorkin = true;
                    VF.SetMessage(this.sCurrentTransID, "SIGN_SCREEN", msgList);
                    deviceWorkin = false;
                } else if (this.isPAX) {
                    PD.CaptureRXSig();
                }
                else if (this.isEvertec)
                {//PRIMEPOS-2664
                    EV.CaptureRxSignature();
                }
                //PRIMEPOS-2636 
                else if (this.isVantiv)
                {//
                    VFD.CaptureRxSignature();
                }
                else if (this.isElavon)//2943
                {//
                    ELV.CaptureRxSignature();
                }
                else {
                    MMSHost.SetMessage(this.sCurrentTransID, "SIGN_SCREEN", msgList);
                    MMSHost.InitiateCaptureSignature(this.sCurrentTransID, INITIATESIGNATURE);
                }

                if (isISC) {
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                    ofrmWait.Show();
                    while (VF.IsSignValid == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed) {
                            if (this.isVF) {
                                deviceWorkin = true;
                                VF.SetMessage(this.sCurrentTransID, "SIGN_SCREEN", msgList);
                                deviceWorkin = false;
                            }
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                        }
                    }
                    ofrmWait.Close();
                    if (VF.IsSignValid == true) {
                        this.strSignature = VF.GetSignatureString;
                        VF.GetSignatureString = string.Empty;
                        retValue = true;
                    }
                } else if(this.isPAX){
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                    ofrmWait.Show();
                    while (PD.IsSignValid == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);

                        if (ofrmWait.IsDisposed) {
                            if (this.isPAX) {
                                PD.CaptureRXSig();
                            }
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                        }
                    }
                    if (PD.IsSignValid == true) {
                        PD.GetSignature();
                        while (PD.GetSignatureString == null) {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed) {
                                this.strSignature = "";
                                break;
                            }
                        }
                        this.strSignature = PD.GetSignatureString;
                        retValue = true;
                    } else if (PD.IsSignValid == false) {
                        this.strSignature = string.Empty;
                        retValue = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    ofrmWait.Close();
                    PD.GetSignatureString = null;
                    PD.IsSignValid = null;
                }
                //PRIMEPOS-2664
                else if (this.isEvertec)
                {
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                    ofrmWait.Show();
                    while (EV.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);

                        if (ofrmWait.IsDisposed)
                        {
                            if (this.isEvertec)
                            {
                                EV.CaptureRxSignature();
                            }
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                        }
                    }
                    if (EV.IsSignValid == true)
                    {
                        EV.GetSignature();
                        while (EV.GetSignatureString == null)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed)
                            {
                                this.strSignature = "";
                                break;
                            }
                        }
                        this.strSignature = EV.GetSignatureString;
                        retValue = true;
                    }
                    else if (EV.IsSignValid == false)
                    {
                        this.strSignature = string.Empty;
                        retValue = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    ofrmWait.Close();
                    EV.GetSignatureString = null;
                    EV.IsSignValid = null;
                }
                //PRIMEPOS-2636 ADDED BY ARVIND
                else if (this.isVantiv)
                {
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                    ofrmWait.Show();
                    while (VFD.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);

                        if (ofrmWait.IsDisposed)
                        {
                            if (this.isVantiv)
                            {
                                VFD.CaptureRxSignature();
                            }
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                        }
                    }
                    if (VFD.IsSignValid == true)
                    {
                        //VFD.GetSignature();
                        //while (VFD.GetSignatureString == null)
                        //{
                        //    Application.DoEvents();
                        //    System.Threading.Thread.Sleep(100);
                        //    if (ofrmWait.IsDisposed)
                        //    {
                        //        this.strSignature = "";
                        //        break;
                        //    }
                        //}
                        this.strSignature = VFD.GetSignatureString;
                        retValue = true;
                    }
                    else if (VFD.IsSignValid == false)
                    {
                        this.strSignature = string.Empty;
                        retValue = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    ofrmWait.Close();
                    VFD.GetSignatureString = null;
                    VFD.IsSignValid = null;
                }
                else if (this.isElavon)//2943
                {
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                    ofrmWait.Show();
                    while (ELV.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);

                        if (ofrmWait.IsDisposed)
                        {
                            if (this.isElavon)
                            {
                                ELV.CaptureRxSignature();
                            }
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(false, "Capture Signature", "Please wait...");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                        }
                    }
                    if (ELV.IsSignValid == true)
                    {
                        //VFD.GetSignature();
                        //while (VFD.GetSignatureString == null)
                        //{
                        //    Application.DoEvents();
                        //    System.Threading.Thread.Sleep(100);
                        //    if (ofrmWait.IsDisposed)
                        //    {
                        //        this.strSignature = "";
                        //        break;
                        //    }
                        //}
                        this.strSignature = VFD.GetSignatureString;
                        retValue = true;
                    }
                    else if (ELV.IsSignValid == false)
                    {
                        this.strSignature = string.Empty;
                        retValue = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    ofrmWait.Close();
                    ELV.GetSignatureString = null;
                    ELV.IsSignValid = null;
                }
                else
                {
                    frmWaitScreen ofrmWait = new frmWaitScreen("Capture Signature", "Please wait...", WaitFor.CaptureCCSignature);
                    ofrmWait.SetMsgDetails("Approval Signature");
                    if (ofrmWait.ShowDialog(this) == DialogResult.Cancel)
                    {
                        this.strSignature = "";

                        if (!this.isVF)
                            MMSHost.CancelOperation(this.sCurrentTransID, SIGNATURECAPTURECANCEL);
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                        retValue = false; // Added By Dharmendra (SRT) 29-09-08
                        return retValue;
                    }
                    else
                    {
                        if (this.strSignature.Trim() != "")
                        {
                            retValue = true;
                        }
                    }
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureSignature()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CaptureSignature() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            return retValue;
        }

        /// <summary>
        /// Author: Manoj.
        /// Function: Capture OTC Signature for Pseudoephedrine.
        /// Start date: 5/10/2012
        /// </summary>
        /// <param name="strMessage"></param>
        /// <returns>True or False</returns>
        public bool CaptureOTcItemsSignature(ArrayList strOTCItemDescriptions, bool isPSEItem)//PRIMEPOS-3109
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureOTcItemsSignature()", clsPOSDBConstants.Log_Entering);
            logger.Trace("CaptureOTcItemsSignature() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            strSignature = "";
            bool retValue = false;
            if (isConnected())
            {
                string OTC_Notice = string.Empty;
                OTC_Notice = isPSEItem ? Configuration.CSetting.PseudoephedDisclaimer : Configuration.CInfo.OtcPrivacyNotice;//PRIMEPOS-3109
                ArrayList otcList = new ArrayList();
                otcList.Insert(0, OTC_Notice);
                int Index = 1;
                foreach (string ItemDesc in strOTCItemDescriptions)
                {
                    otcList.Insert(Index, ItemDesc);
                    Index++;
                }

                if (this.isVF) {
                    deviceWorkin = true;
                    VF.SetMessage(this.sCurrentTransID, "OTC_SIGN_SCREEN", otcList);
                    deviceWorkin = false;
                } else if (this.isPAX) {
                    //PRIMEPOS:2544 - Added following code for OTC Signature Waiting PopUp Message box  - NileshJ Start [14/06/2018]
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    ofrmWait.Show();
                    PD.CaptureOTCItemSignatureAknowledgement(otcList);
                     while (PD.shouldMoveNext == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                    PD.shouldMoveNext = null;
                    ofrmWait.Close();
                    //PRIMEPOS:2544  End [14/06/2018]

                }
                //PRIMEPOS-2636 ADDED BY ARVIND
                else if (this.isVantiv)
                {
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    ofrmWait.Show();
                    VFD.CaptureOTCItemSignatureAknowledgement(otcList);
                    while (VFD.shouldMoveNext == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                    VFD.shouldMoveNext = null;
                    ofrmWait.Close();

                }
                else if (this.isElavon)//2943
                {
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    ofrmWait.Show();
                    ELV.CaptureOTCItemSignatureAknowledgement(otcList);
                    while (ELV.shouldMoveNext == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                    ELV.shouldMoveNext = null;
                    ofrmWait.Close();

                }
                else if (this.isEvertec)
                {
                    //ToDo currently don't know this Functionality

                    //frmWaitScreen ofrmWait = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    //ofrmWait.Show();
                    //EV.CaptureOTCItemSignature();
                    //while (EV.shouldMoveNext == null)
                    //{
                    //    Application.DoEvents();
                    //    System.Threading.Thread.Sleep(100);
                    //}
                    //EV.shouldMoveNext = null;
                    //ofrmWait.Close();

                }
                else
                    MMSHost.SetMessage(this.sCurrentTransID, "OTC_SIGN_SCREEN", otcList);

                if (isISC) {
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    ofrmWait.Show();
                    VF.IsSignValid = null;
                    while (VF.IsSignValid == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed && sigCount <= 3) {
                            deviceWorkin = true;
                            VF.SetMessage(this.sCurrentTransID, "OTC_SIGN_SCREEN", otcList);
                            deviceWorkin = false;
                            ofrmWait.Close();
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(false, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                            sigCount++;
                        } else if (ofrmWait.IsDisposed && sigCount > 3) {
                            retValue = false;
                            break;
                        }
                    }
                    ofrmWait.Close();
                    if (VF.IsSignValid == true) {
                        this.strSignature = VF.GetSignatureString;
                        VF.GetSignature = null;
                        VF.GetSignatureString = null;
                        retValue = true;
                    }
                } else if (this.isPAX) {
                    PD.CaptureOTCItemSignature(); //PRIMEPOS:2544  NileshJ -  Pass method DOSIGNATURE()
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    ofrmWait.Show();
                    PD.IsSignValid = null;
                    while (PD.IsSignValid == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed && sigCount <= 3) {
                            PD.CaptureOTCItemSignature();
                            ofrmWait.Close();
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                            sigCount++;
                        } else if (ofrmWait.IsDisposed && sigCount > 3) {
                            retValue = false;
                            break;
                        }
                    }

                    if (PD.IsSignValid == true) {
                        PD.GetSignature();
                        while (PD.GetSignatureString == null) {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed) {
                                this.strSignature = "";
                                retValue = false;
                                break;
                            }
                        }
                        ofrmWait.Close();
                        this.strSignature = PD.GetSignatureString;
                        PD.GetSignatureString = null;
                        retValue = true;
                        PD.IsSignValid = null;
                    } else {
                        ofrmWait.Close();
                        this.strSignature = null;
                        PD.GetSignatureString = null;
                        retValue = false;
                        PD.IsSignValid = null;
                    }
                    ofrmWait.Close();
                }

                else if (this.isEvertec)//PRIMEPOS-2664
                {
                    EV.CaptureOTCItemSignature();
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    ofrmWait.Show();
                    EV.IsSignValid = null;
                    while (EV.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed && sigCount <= 3)
                        {
                            EV.CaptureOTCItemSignature();
                            ofrmWait.Close();
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                            sigCount++;
                        }
                        else if (ofrmWait.IsDisposed && sigCount > 3)
                        {
                            retValue = false;
                            break;
                        }
                    }

                    if (EV.IsSignValid == true)
                    {
                        EV.GetSignature();
                        while (EV.GetSignatureString == null)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed)
                            {
                                this.strSignature = "";
                                retValue = false;
                                break;
                            }
                        }
                        ofrmWait.Close();
                        this.strSignature = EV.GetSignatureString;
                        EV.GetSignatureString = null;
                        retValue = true;
                        EV.IsSignValid = null;
                    }
                    else
                    {
                        ofrmWait.Close();
                        this.strSignature = null;
                        EV.GetSignatureString = null;
                        retValue = false;
                        EV.IsSignValid = null;
                    }
                    ofrmWait.Close();
                }
                //PRIMEPOS-2636 ADDED BY ARVIND
                else if (this.isVantiv)
                {
                    VFD.CaptureOTCItemSignature();
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    ofrmWait.Show();
                    VFD.IsSignValid = null;
                    while (VFD.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed && sigCount <= 3)
                        {
                            VFD.CaptureOTCItemSignature();
                            ofrmWait.Close();
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                            sigCount++;
                        }
                        else if (ofrmWait.IsDisposed && sigCount > 3)
                        {
                            retValue = false;
                            break;
                        }
                    }

                    if (VFD.IsSignValid == true)
                    {
                        //VFD.GetSignature();
                        while (VFD.GetSignatureString == null)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed)
                            {
                                this.strSignature = "";
                                retValue = false;
                                break;
                            }
                        }
                        ofrmWait.Close();
                        this.strSignature = VFD.GetSignatureString;
                        VFD.GetSignatureString = null;
                        retValue = true;
                        VFD.IsSignValid = null;
                    }
                    else
                    {
                        ofrmWait.Close();
                        this.strSignature = null;
                        VFD.GetSignatureString = null;
                        retValue = false;
                        VFD.IsSignValid = null;
                    }
                    ofrmWait.Close();
                }
                else if (this.isElavon)//2943
                {
                    ELV.CaptureOTCItemSignature();
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                    ofrmWait.Show();
                    ELV.IsSignValid = null;
                    while (ELV.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed && sigCount <= 3)
                        {
                            ELV.CaptureOTCItemSignature();
                            ofrmWait.Close();
                            frmWaitScreen ofrmWaitd = new frmWaitScreen(true, "OTC Signature", "Capturing OTC Signature.  Please wait....");
                            ofrmWait = ofrmWaitd;
                            ofrmWait.Show();
                            sigCount++;
                        }
                        else if (ofrmWait.IsDisposed && sigCount > 3)
                        {
                            retValue = false;
                            break;
                        }
                    }

                    if (ELV.IsSignValid == true)
                    {
                        //VFD.GetSignature();
                        while (ELV.GetSignatureString == null)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed)
                            {
                                this.strSignature = "";
                                retValue = false;
                                break;
                            }
                        }
                        ofrmWait.Close();
                        this.strSignature = ELV.GetSignatureString;
                        ELV.GetSignatureString = null;
                        retValue = true;
                        ELV.IsSignValid = null;
                    }
                    else
                    {
                        ofrmWait.Close();
                        this.strSignature = null;
                        VFD.GetSignatureString = null;
                        retValue = false;
                        VFD.IsSignValid = null;
                    }
                    ofrmWait.Close();
                }
                else {
                    frmWaitScreen ofrmWait = new frmWaitScreen("Capture Signature for OTC", "Please wait...", WaitFor.CaptureOTCSignature);
                    ofrmWait.SetMsgDetails("Approval Signature");
                    if (ofrmWait.ShowDialog(this) == DialogResult.Cancel) {
                        this.strSignature = "";
                        if (!this.isVF)
                            MMSHost.CancelOperation(this.sCurrentTransID, OTCCAPTURECANCEL);

                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                        return retValue;
                    } else {
                        if (this.strSignature.Trim() != "") {
                            retValue = true;
                        }
                    }
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureOTcItemsSignature()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CaptureOTcItemsSignature() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            return retValue;
        }

        public bool? CaptureNOPP(string sPatientName, string sPatientAddress)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureNOPP()", clsPOSDBConstants.Log_Entering);
            logger.Trace("CaptureNOPP() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            strSignature = "";
            bool? retValue = true;
            if (isConnected() == true)
            {
                String privacyText = String.Empty;
                privacyText = Configuration.CInfo.PrivacyText;
                if (this.isVF) {
                    deviceWorkin = true;
                    VF.InitiateNoppSign(this.sCurrentTransID, sPatientName, sPatientAddress, privacyText);
                    deviceWorkin = false;
                } else if (this.isPAX) {
                    
                    PD.CaptureNOPP(this.sCurrentTransID, sPatientName, sPatientAddress, privacyText);
                }
                else if (this.isEvertec)
                {
                    //PRIMEPOS-2664
                    EV.CaptureNOPP();//PRIMEPOS-3209
                }
                //PRIMEPOS-2636 ADDED BY ARVIND
                else if (this.isVantiv)
                {
                    VFD.CaptureNOPP(this.sCurrentTransID, sPatientName, sPatientAddress, privacyText);
                }
                else if (this.isElavon)//2943
                {
                    ELV.CaptureNOPP(this.sCurrentTransID, sPatientName, sPatientAddress, privacyText);
                }
                else
                    MMSHost.InitiateNoppSign(this.sCurrentTransID, sPatientName, sPatientAddress, privacyText);

                if (isISC) {
                    //PRIMEPOS-2442 MODIFIED BY ROHIT NAIR
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "HIPAA Acknowledgement", "Capturing HIPAA Acknowledgement.  Please wait....");
                    ofrmWait.Show();
                    VF.ButtonClickID = null;

                    //VF.IsSignValid = null;
                    while (VF.ButtonClickID == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed) {
                            retValue = null;
                            break;
                        }
                    }
                    ofrmWait.Close();

                    if (!string.IsNullOrEmpty(VF.ButtonClickID) && (VF.ButtonClickID == "ENTER")) {
                        retValue = true;
                    } else if (!string.IsNullOrEmpty(VF.ButtonClickID) && (VF.ButtonClickID == "R")) {
                        retValue = false;
                    } else {
                        retValue = null;
                    }


                    VF.ButtonClickID = null;
                } else if (this.isPAX) {
                    

                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "HIPAA Acknowledgement", "Capturing HIPAA Acknowledgement.  Please wait....");
                    ofrmWait.Show();
                    while (PD.HIPPAAckResultId == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed) {
                            retValue = null;
                            break;
                        }
                    }
                    ofrmWait.Close();

                    if (!string.IsNullOrEmpty(PD.HIPPAAckResultId) && (PD.HIPPAAckResultId == PAXPx7Device.BTN_YES)) {
                        retValue = true;
                    } else if (!string.IsNullOrEmpty(PD.HIPPAAckResultId) && (PD.HIPPAAckResultId == PAXPx7Device.BTN_NO)) {
                        retValue = false;
                    } else {
                        retValue = null;
                    }


                    PD.HIPPAAckResultId = null;

                }
                else if (this.isEvertec)
                {
                    //NO NOPP
                    //PRIMEPOS-2664
                    #region PRIMEPOS-3209
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "HIPAA Acknowledgement", "Capturing HIPAA Acknowledgement.  Please wait....");
                    ofrmWait.Show();
                    while (EV.HIPPAAckResultId == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retValue = null;
                            break;
                        }
                    }
                    ofrmWait.Close();

                    if (!string.IsNullOrEmpty(EV.HIPPAAckResultId) && (EV.HIPPAAckResultId == EvertecVx820Device.BTN_YES))
                    {
                        retValue = true;
                    }
                    else if (!string.IsNullOrEmpty(EV.HIPPAAckResultId) && (EV.HIPPAAckResultId == EvertecVx820Device.BTN_NO))
                    {
                        retValue = false;
                    }
                    else
                    {
                        retValue = null;
                    }
                    EV.HIPPAAckResultId = null;
                    #endregion
                }
                //PRIMEPOS-2636
                else if (this.isVantiv)
                {


                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "HIPAA Acknowledgement", "Capturing HIPAA Acknowledgement.  Please wait....");
                    ofrmWait.Show();
                    while (VFD.HIPPAAckResultId == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retValue = null;
                            break;
                        }
                    }
                    ofrmWait.Close();

                    if (!string.IsNullOrEmpty(VFD.HIPPAAckResultId) && (VFD.HIPPAAckResultId == VerifoneMx925Device.BTN_YES))
                    {
                        retValue = true;
                    }
                    else if (!string.IsNullOrEmpty(VFD.HIPPAAckResultId) && (VFD.HIPPAAckResultId == VerifoneMx925Device.BTN_NO))
                    {
                        retValue = false;
                    }
                    else
                    {
                        retValue = null;
                    }


                    VFD.HIPPAAckResultId = null;

                }
                else if (this.isElavon)//2943
                {


                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "HIPAA Acknowledgement", "Capturing HIPAA Acknowledgement.  Please wait...."); //PRIMEPOS-3212
                    ofrmWait.Show();
                    while (ELV.HIPPAAckResultId == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retValue = null;
                            break;
                        }
                    }
                    ofrmWait.Close();

                    if (!string.IsNullOrEmpty(ELV.HIPPAAckResultId) && (ELV.HIPPAAckResultId == VerifoneMx925Device.BTN_YES))
                    {
                        retValue = true;
                    }
                    else if (!string.IsNullOrEmpty(ELV.HIPPAAckResultId) && (ELV.HIPPAAckResultId == VerifoneMx925Device.BTN_NO))
                    {
                        retValue = false;
                    }
                    else
                    {
                        retValue = null;
                    }


                    ELV.HIPPAAckResultId = null;

                }
                else {
                    frmWaitScreen ofrmWait = new frmWaitScreen("NOPP Signature", "Please Wait..", WaitFor.CaptureNOPPSignature);
                    ofrmWait.SetMsgDetails("Capture Signature for Privacy Ack");
                    if (ofrmWait.ShowDialog(this) == DialogResult.Cancel)
                    {
                        this.strSignature = "";
                        if (!this.isVF)
                            MMSHost.CancelOperation(this.sCurrentTransID, NOPPCAPTURECANCEL); //6002

                        retValue = false; // Added By Dharmendra (SRT) 29-09-08
                        return retValue;
                    }
                    else if (clsCoreUIHelper.DefaultValue == NOPPUSERCANCEL)
                    {
                        // if (this.strSignature.Trim() != "")
                        //{
                        retValue = false;
                        //}
                    }
                    else if (clsCoreUIHelper.DefaultValue == NOPPUSERCONTINUE)
                    {
                        retValue = null;
                    }
                    else if (clsCoreUIHelper.DefaultValue == "")
                    {
                        if (this.strSignature.Trim() != "")
                        {
                            retValue = true;
                        }
                    }
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureNOPP()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CaptureNOPP() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            return retValue;
        }

        #region PRIMEPOS-2442 ADDED BY ROHIT NAIR
        public bool? CapturePatientConsent(string consentName, out PatientConsent oConsent, DataSet dsActiveConsentDetails = null)
        {
            logger.Trace("CapturePatientSignature() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            bool retVal = false;
            int NoOfDays = 0; //PRIMEPOS - CONSENT SAJID DHUKKA // PRIMEPOS-2866,PRIMEPOS-2871
            oConsent = null;
            if (isConnected())
            {
                if (isISC)
                {
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, consentName, "Capturing Patient Consent. Please wait...."); //PRIMEPOS - CONSENT SAJID DHUKKA // PRIMEPOS-2866,PRIMEPOS-2871
                    ofrmWait.Show();
                    VF.PatConsent = null;
                    retVal = VF.DisplayScreen(this.sCurrentTransID, consentName, Configuration.CInfo.StoreName, dsActiveConsentDetails);
                    while (VF.PatConsent == null)
                    {
                        //ofrmWait.Dispose();
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retVal = false;
                            VF.CancelCapture = true;
                            break;
                        }
                    }
                    ofrmWait.Close();
                    //retVal = VF.PatConsent.ConsentTextID == 0; // Condition check whether its reject or select                    
                    oConsent = VF.PatConsent;
                }
                else if (isPAX)
                {
                    //frmWaitScreen ofrmWait = new frmWaitScreen(true, "Healthix", "Capturing Patient Consent. Please wait....");
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, consentName, "Capturing Patient Consent. Please wait...."); //PRIMEPOS - CONSENT SAJID DHUKKA // PRIMEPOS-2866,PRIMEPOS-2871
                    ofrmWait.Show();
                    PD.PatConsent = null;
                    retVal = true;
                    PD.CapturePatConsent(consentName, Configuration.CInfo.StoreName, dsActiveConsentDetails);
                    while (PD.PatConsent == null)
                    {
                        ofrmWait.Dispose();
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retVal = false;
                            break;
                        }
                    }
                    ofrmWait.Close();
                    oConsent = PD.PatConsent;
                }
                else if (isEvertec)
                {
                    //PRIMEPOS-2664                    
                    retVal = true;
                }
                //PRIMEPOS-2636
                else if (isVantiv && Configuration.CPOSSet.PinPadModel == "VANTIV")//3002
                {
                    //frmWaitScreen ofrmWait = new frmWaitScreen(true, "Healthix", "Capturing Patient Consent. Please wait....");
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, consentName, "Capturing Patient Consent. Please wait...."); //PRIMEPOS - CONSENT SAJID DHUKKA // PRIMEPOS-2866,PRIMEPOS-2871
                    ofrmWait.Show();
                    retVal = true;
                    VFD.PatConsent = null;
                    VFD.CapturePatConsent(consentName, Configuration.CInfo.StoreName, dsActiveConsentDetails);
                    while (VFD.PatConsent == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retVal = false;
                            VF.CancelCapture = true;
                            break;
                        }
                    }
                    ofrmWait.Close();

                    oConsent = VFD.PatConsent;
                }
                //PRIMEPOS-2867 Arvind  Consent
                else if (IsVF)
                {
                    try
                    {
                        if (consentName.ToUpper() != MMS.Device.Global.Constants.CONSENT_SOURCE_HEALTHIX.ToUpper())
                        {
                            frmWaitScreen ofrmWait = new frmWaitScreen(true, consentName, "Capturing Patient Consent. Please wait....");
                            ofrmWait.Show();
                            VF.PtConent = null;
                            VF.DisplayScreen(this.sCurrentTransID, consentName, Configuration.CInfo.StoreName, dsActiveConsentDetails);
                            while (VF.PtConent == null)
                            {
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(100);
                                if (ofrmWait.IsDisposed)
                                {
                                    retVal = false;
                                    VF.CancelCapture = true;
                                    break;
                                }
                            }
                            ofrmWait.Close();

                            oConsent = VF.PtConent;
                            retVal = true;
                            if (retVal)
                            {
                                //PRIMEPOS - CONSENT SAJID DHUKKA // PRIMEPOS-2866,PRIMEPOS-2871
                                int.TryParse(dsActiveConsentDetails.Tables["ConsentTextVersion"].Rows[0]["ConsentValidForDays"].ToString(), out NoOfDays);
                                oConsent.ConsentEndDate = DateTime.Now.AddDays(NoOfDays);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsCoreUIHelper.ShowErrorMsg("Error in CapturePatientConsent : " + ex.ToString());
                    }
                }
                else if (isElavon)//2943
                {
                    //frmWaitScreen ofrmWait = new frmWaitScreen(true, "Healthix", "Capturing Patient Consent. Please wait....");
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, consentName, "Capturing Patient Consent. Please wait...."); //PRIMEPOS - CONSENT SAJID DHUKKA // PRIMEPOS-2866,PRIMEPOS-2871
                    ofrmWait.Show();
                    retVal = true;
                    ELV.PatConsent = null;
                    ELV.CapturePatConsent(consentName, Configuration.CInfo.StoreName, dsActiveConsentDetails);
                    while (ELV.PatConsent == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retVal = false;
                            VF.CancelCapture = true;
                            break;
                        }
                    }
                    ofrmWait.Close();

                    oConsent = ELV.PatConsent;
                }
                if (oConsent != null)
                {
                    oConsent.ConsentSourceName = consentName;
                    if (!oConsent.IsConsentSkip)
                    {
                        if (dsActiveConsentDetails != null)
                        {
                            string code = oConsent.ConsentStatusCode;
                            //int sourceId = oConsent.ConsentSourceID;
                            var selectedConsentStatus = (from consentStatus in dsActiveConsentDetails.Tables["Consent_Status"].AsEnumerable() where consentStatus.Field<string>("Code") == code select consentStatus).CopyToDataTable();

                            int.TryParse(selectedConsentStatus.Rows[0]["ValidityPeriod"].ToString(), out NoOfDays);
                            // oConsent.ConsentEndDate = DateTime.Now.AddDays(NoOfDays); //NileshJ - COmmented
                        }
                    }
                }
            }
            logger.Trace("CapturePatientSignature() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        public bool CapturePatientSignature()
        {
            logger.Trace("CapturePatientSignature() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            this.strSignature = "";
            bool retVal = false;
            string sFormName = "PADPATSIGN";
            if (isConnected() == true)
            {
                if (isISC) {


                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "Rx Signature", "Capturing Rx Signature.  Please wait....");
                    ofrmWait.Show();
                    VF.IsSignValid = null;
                    retVal = VF.DisplayScreen(this.sCurrentTransID, sFormName, string.Empty);
                    while (VF.IsSignValid == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed) {
                            retVal = false;
                            break;
                        }
                    }
                    ofrmWait.Close();
                    if (VF.IsSignValid == true) {
                        this.strSignature = VF.GetSignatureString;
                        VF.GetSignature = null;
                        retVal = true;
                    } else if (VF.IsSignValid == false) {
                        this.strSignature = string.Empty;
                        retVal = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    VF.IsSignValid = null;
                } else if(this.isPAX) {

                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "Rx Signature", "Capturing Rx Signature.  Please wait....");
                    ofrmWait.Show();
                    PD.GetSignatureString = null;

                    retVal = PD.CaptureRXSig();

                    while (PD.IsSignValid == null) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed) {
                            retVal = false;
                            break;
                        }
                    }
                   

                    if (PD.IsSignValid == true) {
                        retVal = PD.GetSignature();
                        while (PD.GetSignatureString == null) {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed) {
                                retVal = false;
                                break;
                            }
                        }
                        

                        this.strSignature = PD.GetSignatureString;
                        PD.GetSignatureString = null;
                        retVal = true;
                    } else if (PD.IsSignValid == false) {
                        this.strSignature = string.Empty;
                        retVal = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    ofrmWait.Close();
                    PD.IsSignValid = null;
                }
                else if (this.isEvertec)//PRIMEPOS-2664
                {

                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "Rx Signature", "Capturing Rx Signature.  Please wait....");
                    ofrmWait.Show();
                    EV.GetSignatureString = null;

                    retVal = EV.CaptureRxSignature();

                    while (EV.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retVal = false;
                            break;
                        }
                    }


                    if (EV.IsSignValid == true)
                    {
                        retVal = EV.GetSignature();
                        while (EV.GetSignatureString == null)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed)
                            {
                                retVal = false;
                                break;
                            }
                        }


                        this.strSignature = EV.GetSignatureString;
                        EV.GetSignatureString = null;
                        retVal = true;
                    }
                    else if (EV.IsSignValid == false)
                    {
                        this.strSignature = string.Empty;
                        retVal = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    ofrmWait.Close();
                    EV.IsSignValid = null;
                }
                //PRIMEPOS-2636
                else if (this.isVantiv && Configuration.CPOSSet.PinPadModel == "VANTIV")//3002
                {

                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "Rx Signature", "Capturing Rx Signature.  Please wait....");
                    ofrmWait.Show();
                    VFD.GetSignatureString = null;

                    retVal = VFD.CaptureRXSig();

                    while (VFD.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retVal = false;
                            break;
                        }
                    }


                    if (VFD.IsSignValid == true)
                    {
                        //retVal = VFD.GetSignature();
                        while (VFD.GetSignatureString == null)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed)
                            {
                                retVal = false;
                                break;
                            }
                        }


                        this.strSignature = VFD.GetSignatureString;
                        VFD.GetSignatureString = null;
                        retVal = true;
                    }
                    else if (VFD.IsSignValid == false)
                    {
                        this.strSignature = string.Empty;
                        retVal = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    ofrmWait.Close();
                    VFD.IsSignValid = null;
                }
                else if (this.isVantiv && (Configuration.CPOSSet.PinPadModel == "VANTIV_ISMP_WITHTOUCHSCREEN" || Configuration.CPOSSet.PinPadModel == "VANTIV_ISMP_WITHOUTTOUCHSCREEN" || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500"))//3002 //PRIMEPOS-3231
                {
                    //Kept empty because we dont want to display anything here
                }
                else if (this.isElavon)//2943
                {

                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "Rx Signature", "Capturing Rx Signature.  Please wait....");
                    ofrmWait.Show();
                    ELV.GetSignatureString = null;

                    retVal = ELV.CaptureRXSig();

                    while (ELV.IsSignValid == null)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            retVal = false;
                            break;
                        }
                    }


                    if (ELV.IsSignValid == true)
                    {
                        //retVal = VFD.GetSignature();
                        while (ELV.GetSignatureString == null)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);
                            if (ofrmWait.IsDisposed)
                            {
                                retVal = false;
                                break;
                            }
                        }


                        this.strSignature = ELV.GetSignatureString;
                        ELV.GetSignatureString = null;
                        retVal = true;
                    }
                    else if (ELV.IsSignValid == false)
                    {
                        this.strSignature = string.Empty;
                        retVal = false;
                        ShowCustomScreen("Signature Capture Cancelled. Please wait...");
                    }
                    ofrmWait.Close();
                    ELV.IsSignValid = null;
                }
            }
          
            logger.Trace("CapturePatientSignature() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        #endregion
        public bool CaptureRXSignature(BusinessRules.RXHeader oRXHeader, out string PatientCounceling)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureRXSignature()", clsPOSDBConstants.Log_Entering);
            logger.Trace("CaptureRXSignature() - Sig Pad" + clsPOSDBConstants.Log_Entering);
            PatientCounceling = "";
            this.strSignature = "";
            bool retVal = false;
            if (isConnected() == true)
            {
                ArrayList newRxList = new ArrayList();
                string userSignature = string.Empty;
                string curentScreen = string.Empty;
                //Added by SRT (Dharmendra) to improve the performance.
                DataRow rxRows;
                if (rxList.Rows.Count > 0)
                {
                    rxList.Rows.Clear();
                }
                //Added by SRT till here.

                foreach (BusinessRules.RXDetail oRX in oRXHeader.RXDetails)
                {
                    //Modified by SRT to use DataRow instead of String to improve performance.
                    rxRows = rxList.NewRow();
                    rxRows[0] = oRX.RXNo.ToString() + "-" + oRX.RefillNo.ToString();
                    rxRows[1] = oRX.DrugName.Trim();
                    rxRows[2] = oRX.RxDate;
                    //Added By SRT(Ritesh Parekh) Date: 29-Jul-2009
                    //
                    rxRows[3] = oRX.ShowRxDescription;
                    //End Of SRT(Ritesh Parekh)
                    rxList.Rows.Add(rxRows);
                    //Modified by SRT till here.
                }


                if (this.isVF) {
                    deviceWorkin = true;
                    VF.RxSign(sCurrentTransID, oRXHeader.RXDetails.Count.ToString(), oRXHeader.PatientName, Configuration.CInfo.PatientCounceling, rxList, Configuration.CInfo.HidePatientCounseling);
                    deviceWorkin = false;
                } else if (this.isPAX) {
                    PD.PatCounsel = string.Empty;
                    PD.CapturePatConsel(sCurrentTransID, oRXHeader.RXDetails.Count.ToString(), oRXHeader.PatientName, Configuration.CInfo.PatientCounceling, rxList, Configuration.CInfo.HidePatientCounseling);
                }
                else if (this.isEvertec)
                {
                    EV.PatCounsel = string.Empty;//PRIMEPOS-3209
                    EV.CapturePatConsel(sCurrentTransID, oRXHeader.RXDetails.Count.ToString(), oRXHeader.PatientName, Configuration.CInfo.PatientCounceling, rxList, Configuration.CInfo.HidePatientCounseling);//PRIMEPOS-3209 //PRIEMPOS-3442 added HidePatientCounseling
                }
                //PRIMEPOS-2636
                else if (this.isVantiv && Configuration.CPOSSet.PinPadModel == "VANTIV")//3002
                {
                    VFD.PatCounsel = string.Empty;
                    VFD.CapturePatConsel(sCurrentTransID, oRXHeader.RXDetails.Count.ToString(), oRXHeader.PatientName, Configuration.CInfo.PatientCounceling, rxList, Configuration.CInfo.HidePatientCounseling); //PRIMEPOS-3302-added bHidePatCounseling
                }
                else if (this.isElavon)//2943
                {
                    ELV.PatCounsel = string.Empty;
                    ELV.CapturePatConsel(sCurrentTransID, oRXHeader.RXDetails.Count.ToString(), oRXHeader.PatientName, Configuration.CInfo.PatientCounceling, rxList);
                }
                else if (this.isVantiv && (Configuration.CPOSSet.PinPadModel == "VANTIV_ISMP_WITHTOUCHSCREEN" || Configuration.CPOSSet.PinPadModel == "VANTIV_ISMP_WITHOUTTOUCHSCREEN" || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500"))//3002 //PRIMEPOS-3231
                {
                    //Kept empty because we dont want to display anything here
                }
                else
                {
                    MMSHost.UpdateRxHeaders(sCurrentTransID, oRXHeader.RXDetails.Count.ToString(), oRXHeader.PatientName, Configuration.CInfo.PatientCounceling);
                    MMSHost.AddRx(sCurrentTransID, rxList);//Modified by SRT the parameter to be of type DataTable.
                }

                if (isISC)
                {
                    //PRIMEPOS-2442 MODIFIED BY ROHIT NAIR
                   
                    VF.PatCounsel = string.Empty;
                    frmWaitScreen ofrmWait = new frmWaitScreen(false, "Patient Counseling", "Capturing Confirmation.  Please wait....");
                    //new frmWaitScreen(false, "Rx Signature", "Capturing Rx Signature.  Please wait....");
                    ofrmWait.Show();
                    while (string.IsNullOrEmpty(VF.PatCounsel))
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            break;
                        }
                    }

                    ofrmWait.Close();

                    PatientCounceling = VF.PatCounsel;
                    VF.IsSignValid = null;
                    retVal = true;
                } else if (this.isPAX) {

                    frmWaitScreen ofrmWait = new frmWaitScreen(true, Configuration.CInfo.HidePatientCounseling ? "Rx Pickup Acknowledgement" : "Patient Counseling", "Capturing Confirmation.  Please wait....");
                    ofrmWait.Show();
                    while (string.IsNullOrEmpty(PD.PatCounsel)) {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed) {
                            break;
                        }
                    }

                    ofrmWait.Close();
                
                    PatientCounceling = PD.PatCounsel;
                    retVal = true;
                }
                else if (this.isEvertec)
                {
                    #region PRIMEPOS-3209
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, Configuration.CInfo.HidePatientCounseling ? "Rx Pickup Acknowledgement" : "Patient Counseling", "Capturing Confirmation.  Please wait...."); //PRIMEPOS-3442
                    ofrmWait.Show();
                    while (string.IsNullOrEmpty(EV.PatCounsel))
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            break;
                        }
                    }

                    ofrmWait.Close();
                    PatientCounceling = EV.PatCounsel;
                    #endregion
                    retVal = true;
                    //Doesn't have this feature of Patient Counseling PRIMEPOS-2664                    
                }
                //PRIMEPOS-2636 ADDED BY ARVIND
                else if (this.isVantiv && Configuration.CPOSSet.PinPadModel == "VANTIV")//3002
                {
                    frmWaitScreen ofrmWait = new frmWaitScreen(true, Configuration.CInfo.HidePatientCounseling ? "Rx Pickup Acknowledgement" : "Patient Counseling", "Capturing Confirmation.  Please wait...."); //PRIMEPOS-3302
                    //frmWaitScreen ofrmWait = new frmWaitScreen(true, "Patient Counseling", "Capturing Confirmation.  Please wait...."); //PRIMEPOS-3302
                    ofrmWait.Show();
                    while (string.IsNullOrEmpty(VFD.PatCounsel))
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            break;
                        }
                    }

                    ofrmWait.Close();

                    PatientCounceling = VFD.PatCounsel;
                    retVal = true;
                }
                else if (this.isElavon)//2943
                {

                    frmWaitScreen ofrmWait = new frmWaitScreen(true, "Patient Counseling", "Capturing Confirmation.  Please wait....");
                    ofrmWait.Show();
                    while (string.IsNullOrEmpty(ELV.PatCounsel))
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        if (ofrmWait.IsDisposed)
                        {
                            break;
                        }
                    }

                    ofrmWait.Close();

                    PatientCounceling = ELV.PatCounsel;
                    retVal = true;
                }
                else if (this.isVantiv && (Configuration.CPOSSet.PinPadModel == "VANTIV_ISMP_WITHTOUCHSCREEN" || Configuration.CPOSSet.PinPadModel == "VANTIV_ISMP_WITHOUTTOUCHSCREEN" || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500"))//3002 //PRIMEPOS-3231
                {
                    retVal = true;
                }
                else {
                    frmWaitScreen ofrmWait = new frmWaitScreen("RX Signature", "Please wait...", WaitFor.CaptureRXSignature);
                    ofrmWait.SetMsgDetails("Capture Signature for RX Approval");
                    if (ofrmWait.ShowDialog(this) == DialogResult.Cancel)
                    {
                        this.strSignature = "";
                        if (!this.isVF)
                            MMSHost.CancelOperation(this.sCurrentTransID, RXCAPTURECANCEL);

                        retVal = false; // Added By Dharmendra (SRT) 29-09-08
                    }
                    else
                    {
                        string tempPatientName = String.Empty;
                        string tempTotalRx = String.Empty;
                        if (this.isVF)
                        {
                            deviceWorkin = true;
                            PatientCounceling = VF.PatCounsel;
                            deviceWorkin = false;
                        }
                        else
                            MMSHost.GetRxHeaders(sCurrentTransID, out tempPatientName, out tempTotalRx, out PatientCounceling);

                        if (this.strSignature.Trim() != "")
                        {
                            retVal = true;
                        }
                    }
                }
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "CaptureRXSignature()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("CaptureRXSignature() - Sig Pad" + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        //Added By Dharmendra
        public string CardType
        {
            get { return cardType; }
            set { this.cardType = value; }
        }

        public string ZipCode
        {
            get { return zipCode; }
            set { this.zipCode = value; }
        }

        public string CustomerAddress
        {
            get { return customerAddress; }
            set { this.customerAddress = value; }
        }

        //Added By Dharmendra(SRT) to assign sample data if it is configured Y on 08-29-08
        private void AssignSampleCardData(SigPadCardData oData, string paymentType)
        {
            if (paymentType == PayTypes.CreditCard)
            {
                oData.CardNo = "F4012000033330026";
                oData.CardType = "VISA";
                oData.ExpireOn = "1208";
                oData.FirstName = "ATUL";
                oData.LastName = "JOSHI";
                oData.Track1 = "4012000033330026=08121011000001234567";
                oData.Track2 = "";
                oData.Track3 = "";
                this.CustomerAddress = "8320 Main Street";
                this.ZipCode = "85284";
            }
            else if (paymentType == PayTypes.DebitCard)
            {
                oData.CardNo = "4003000123456781";
                //oData.CardNo = "401200003333026";
                oData.CardType = "VISA";
                oData.ExpireOn = "0809";
                oData.FirstName = "visa test card";
                oData.LastName = "";
                oData.Track1 = "";
                //oData.Track2 = "%B4012000033330026^VERIFONE TEST 3^08121011000 1111A123456789012?;401200003333026=08121011000001234567?";
                //oData.Track2 = "401200003333026=08121011000001234567";
                oData.Track2 = "4003000123456781=09085025432198712345";
                oData.Track3 = "";
                this.CustomerAddress = "";
                this.ZipCode = "";
            }
        }

        //This method clears the card related informations : Added on 02-09-08
        public void ClearCardInfo()
        {
            oSigPadCardData.ClearCardInfo();
            this.CardType = "";
            this.CustomerAddress = "";
            this.ZipCode = "";
        }

        //Added By SRT (Gaurav) Date : 15 Oct 2008
        /// <summary>
        /// Author : Gaurav
        /// Functionality Description : This private methode initialises all requiries of Timer Control
        /// Known Bugs : NONE
        /// Start Date : 15 Oct 2008
        /// </summary>
        private void InitiateTimers()
        {
            //Here HeartBeat Frequency Is Converted In Milliseconds.
            //Modified By Dharmendra (SRT)on Nov-13-08 removed heartbeat time dependency from app.config file System.Configuration.ConfigurationManager.AppSettings["HEART_BEAT_MILLISEC"];
            heartBeats = Configuration.CPOSSet.HeartBeatTime;
            //Changed till here.
            //Added By Dharmendra (SRT) on Nov-17-08 to set the default value
            if (heartBeats.Trim().Equals(string.Empty) || heartBeats.Trim() == null)
            {
                heartBeats = "10";
            }
            //End of Addition
            heartBeatMSec = Convert.ToInt32(heartBeats) * 1000;
            //Initialisation
            heartBeatTimer = new System.Timers.Timer();
            heartBeatTimer.Interval = heartBeatMSec;
            heartBeatTimer.Enabled = false;
            heartBeatTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        /// <summary>
        /// Author : Gaurav
        /// Functionality Description : Event Named OnTimedEvent for HeartBeatTimer
        /// Known Bugs : NONE
        /// Start Date : 15 Oct 2008
        /// </summary>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                heartBeatTimer.Enabled = false;
                if (!this.isVF && isConnected())
                {
                    if (MMSHost.HeartBeat(sCurrentTransID))
                    {
                        heartBeatTimer.Enabled = true;
                    }
                    else
                    {
                        MMSHost = null;
                    }
                }
                else
                {
                    MMSHost = null;
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                MMSHost = null;
                heartBeatTimer.Enabled = false;
                this.ConnectServer();

                mcs = new POSCallbackClass(this);
                if (Configuration.CPOSSet.SigPadHostAddr.ToUpper().Trim().Contains("PRIMEPOSHOSTSRV"))
                {
                    UnRegisterEvent();
                    RegisterEvent();
                }
                heartBeatTimer.Enabled = true;
                heartBeatTimer.Start();
            }
        }

        /// <summary>
        /// Use to send ON HOLD ITEMS FOR VERIFONE
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ID"></param>
        public void SendOnHoldItems(DataTable dt)
        {
            try
            {
                if (this.isVF) {
                    //POS_Core.ErrorLogging.Logs.Logger("\t\tEntering SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    logger.Trace("SendOnHoldItems() - Entering SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    deviceWorkin = true;
                    VF.DisplayOnHoldItem(dt, Configuration.CPOSSet.PrintRXDescription.ToString());
                    deviceWorkin = false;
                    //POS_Core.ErrorLogging.Logs.Logger("\t\tExiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    logger.Trace("SendOnHoldItems() - Exiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                } else if (this.isPAX){
                    logger.Trace("SendOnHoldItems() - Entering SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    PD.SendOnHoldItems(dt, Configuration.CPOSSet.PrintRXDescription.ToString());
                    //POS_Core.ErrorLogging.Logs.Logger("\t\tExiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    logger.Trace("SendOnHoldItems() - Exiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                }
                else if (this.isEvertec)//PRIMEPOS-2664
                {
                    //Not there for Evertec
                    logger.Trace("SendOnHoldItems() - Entering SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    //PD.SendOnHoldItems(dt, Configuration.CPOSSet.PrintRXDescription.ToString());
                    //POS.ErrorLogging.Logs.Logger("\t\tExiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    logger.Trace("SendOnHoldItems() - Exiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                }
                //PRIMEPOS-2636 ADDED BY ARVIND
                else if (this.isVantiv)
                {
                    logger.Trace("SendOnHoldItems() - Entering SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    VFD.SendOnHoldItems(dt, Configuration.CPOSSet.PrintRXDescription.ToString());
                    //POS.ErrorLogging.Logs.Logger("\t\tExiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    logger.Trace("SendOnHoldItems() - Exiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                }
                //
                else if (this.isElavon)//2943
                {
                    logger.Trace("SendOnHoldItems() - Entering SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    ELV.SendOnHoldItems(dt, Configuration.CPOSSet.PrintRXDescription.ToString());
                    //POS.ErrorLogging.Logs.Logger("\t\tExiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                    logger.Trace("SendOnHoldItems() - Exiting SigPad SendOnHoldItems() at: " + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond);
                }
            }
            catch (Exception ex)
            {
                //POS_Core.ErrorLogging.Logs.Logger("\t-->>>> SigPad SendOnHoldItems() Error: \n" + ex.ToString());
                logger.Fatal(ex, "SendOnHoldItems() - SigPad");
            }
        }

        //End Of Added By SRT (Gaurav)
        //Added By Dharmendra (SRT) on Oct-18-08
        /// <summary>
        /// Author : Dharmendra (SRT)
        /// Functionality Description : This method sends onHold Item List on HHP Device
        /// Known Bugs None :
        /// External Functions : None
        /// Start Date : Oct-18-08
        /// </summary>
        /// <param name="oTransDData"></param>
        /// <param name="onHoldTransID"></param>
        public void SendOnHoldItemListOnHHP(TransDetailData oTransDData, int onHoldTransID)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "SendOnHoldItemListOnHHP()", clsPOSDBConstants.Log_Entering);
            logger.Trace("SendOnHoldItemListOnHHP() - Sig Pad - " + clsPOSDBConstants.Log_Entering);
            Hashtable fields = new Hashtable();
            fields.Add(ITEMNAME, "");
            fields.Add(ITEMQTY, "");
            fields.Add(UNITPRICE, "");
            fields.Add(TOTALPRICE, "");
            //added by atul 01-feb-2011
            fields.Add(ISRX, "");
            fields.Add(SHOWRXDESCRIPTION, "");
            //End of added by atul 01-feb-2011
            string currentHHScr = string.Empty; //To store the value of current screen on HHP Device
            int errorCode = 0;
            if (sCurrentTransID != String.Empty) //Change by SRT (Sachin) Date : 06 March 2010
            {
                errorCode = MMSHost.GetCurrentScreen(sCurrentTransID, out currentHHScr);
            }

            if (currentHHScr.ToUpper() != "PADITEMLIST")
            {
                if (sCurrentTransID != String.Empty)
                {
                    MMSHost.DisplayScreen(sCurrentTransID, "ITEMLIST_SCREEN", "");
                }
            }
            foreach (DataRow rw in oTransDData.Tables[0].Rows)
            {
                fields[ITEMNAME] = rw["ItemDescription"].ToString();
                fields[ITEMQTY] = rw["QTY"].ToString();
                fields[UNITPRICE] = Convert.ToDouble(rw["Price"].ToString()).ToString("F", ci).PadRight(2, '0');
                fields[TOTALPRICE] = Convert.ToDouble(rw["ExtendedPrice"].ToString()).ToString("F", ci).PadRight(2, '0');
                fields[ISRX] = rw["ItemID"].ToString().Trim().ToUpper() == "RX" ? ISRXTRUE : ISRXFALSE;
                fields[SHOWRXDESCRIPTION] = Configuration.CPOSSet.PrintRXDescription.ToString();

                double TotalAmountWithDis = Convert.ToDouble(rw["ExtendedPrice"].ToString()) - Convert.ToDouble(rw["Discount"].ToString());
                fields[WITHDISCAMOUNT] = TotalAmountWithDis.ToString("F", ci).PadRight(2, '0');//change by SRT (Sachin) Dated : 1st Nov 2010

                if (sCurrentTransID != String.Empty)
                {
                    //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "SendOnHoldItemListOnHHP()", "CurrentTransID " + sCurrentTransID + " ItemId: " + rw["ItemID"].ToString() + " ItemDescription: " + rw["ItemDescription"].ToString() + " TransDetailID: " + rw["TransDetailID"].ToString());
                    logger.Trace("SendOnHoldItemListOnHHP() - Sig Pad - CurrentTransID " + sCurrentTransID + " ItemId: " + rw["ItemID"].ToString() + " ItemDescription: " + rw["ItemDescription"].ToString() + " TransDetailID: " + rw["TransDetailID"].ToString());
                    errorCode = MMSHost.AddItem(onHoldTransID.ToString(), rw.Table.Rows.IndexOf(rw), fields); //The Interface has changed in MMS.HOST interface.
                    //POS_Core.ErrorLogging.Logs.Logger("Index Send " + rw.Table.Rows.IndexOf(rw) + "Error Code: " + errorCode);
                    logger.Trace("SendOnHoldItemListOnHHP() - Sig Pad - Index Send " + rw.Table.Rows.IndexOf(rw) + "Error Code: " + errorCode);
                }
                fields[ITEMNAME] = "";
                fields[ITEMQTY] = "";
                fields[UNITPRICE] = "";
                fields[TOTALPRICE] = "";
            }
            //POS_Core.ErrorLogging.Logs.Logger("\tSig Pad", "SendOnHoldItemListOnHHP()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("SendOnHoldItemListOnHHP() - Sig Pad - " + clsPOSDBConstants.Log_Exiting);
        }

        //Add Ended

        //only for HPSPAX to update standby image on device
        public void UpdateStandbyImage(byte[] file) {
            
            frmWaitScreen ofrmWait = new frmWaitScreen(true, "Please Wait", "Updating Standby Image of Device...");
            ofrmWait.Show();
            PD.UpdateResourceImage(file);
            while (PD.IsResourceUpdated== null) {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
            PD.IsResourceUpdated = null;
            ofrmWait.Close();
        }
        #region PRIMEPOS-2730 - NileshJ
        public void ClearDeviceQueue()
        {
            //PRIMEPOS-2636
            if (isVantiv)
            {
                VFD.ClearDeviceQueue();
                bool isQueueEmpty = false;
                do
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    isQueueEmpty = SigPadUtil.DefaultInstance.isDevQueueEmpty();
                }
                while (!isQueueEmpty);
            }
            //
            if (isPAX)
            {
                PD.ClearDeviceQueue();
                bool isQueueEmpty = false;
                do
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    isQueueEmpty = SigPadUtil.DefaultInstance.isDevQueueEmpty();
                }
                while (!isQueueEmpty);
            }
            if (isISC || isVF)
            {
                VF.ClearDeviceQueue();
                bool isQueueEmpty = false;
                do
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    isQueueEmpty = SigPadUtil.DefaultInstance.isDevQueueEmpty();
                }
                while (!isQueueEmpty);
            }
            if (isElavon)//2943
            {
                ELV.ClearDeviceQueue();
                bool isQueueEmpty = false;
                do
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    isQueueEmpty = SigPadUtil.DefaultInstance.isDevQueueEmpty();
                }
                while (!isQueueEmpty);
            }
        }

        public bool isDevQueueEmpty()
        {
            bool isQueueEmpty = true;
            if (isPAX)
            {
                isQueueEmpty = PD.isDevQueueEmpty();
            }
            if (isVantiv)
            {
                isQueueEmpty = VFD.isDevQueueEmpty();
            }
            if (isISC || isVF)
            {
                isQueueEmpty = VF.isDevQueueEmpty();
            }
            if (isElavon)//2943
            {
                isQueueEmpty = ELV.isDevQueueEmpty();
            }
            return isQueueEmpty;
        }
        #endregion
    }

    public enum Card
    {
        CC,
        DC
    }

    public class SigPadCardData
    {
        public string cardNo;
        public string cardType;
        public string FirstName;
        public string LastName;
        public string pinNumber; //Modified By SRT (Changed PinCode to PinNumber)
        public string ExpireOn;
        //public string Track1;
        public string Completetrack;
        public string Track2;
        //Added By Dharmendra (SRT) to get more flexibility in frmProcessCC.cs & frmPOSPayTypeList.cs
        //classes
        public string Track3;
        public string posTransAmount;
        public bool IsValidData;
        public string errorMessage;
        public string errorNumber;
        public string paymentType;
        public string keySerialNumber;

        public string CardNo
        {
            get { return cardNo; }
            set
            {
                this.cardNo = value;
                this.cardType = GetCardType(value);
            }
        }

        public string Track1
        {
            get { return Completetrack; }
            set { Completetrack = value; }
        }

        public string CardType
        {
            get { return cardType; }
            set { this.cardType = value; }
        }

        public string PinNumber
        {
            get { return pinNumber; }
            set { this.pinNumber = value; }
        }

        public string KeySerialNumber
        {
            get { return keySerialNumber; }
            set { this.keySerialNumber = value; }
        }

        private string GetCardType(string CardNumber)
        {
            int iCardType = Convert.ToInt32(CardNumber.Substring(0, 4));

            string sCardType = string.Empty;
            if ((iCardType >= 2014 && iCardType <= 2015) || (iCardType >= 2140 && iCardType <= 2150))
            {
                sCardType = "ENRT";
            }
            else if ((iCardType >= 3000 && iCardType <= 3059) || (iCardType >= 3800 && iCardType <= 3899))
            {
                sCardType = "DCCB";
            }
            else if ((iCardType >= 3600 && iCardType <= 3699) || (iCardType >= 5100 && iCardType <= 5599))
            {
                sCardType = "MC";
            }
            else if ((iCardType >= 3400 && iCardType <= 3499) || (iCardType >= 3700 && iCardType <= 3799))
            {
                sCardType = "AMEX";
            }
            else if (iCardType >= 4000 && iCardType <= 4999)
            {
                sCardType = "VISA";
            }
            else
            {
                sCardType = "VISA";
            }
            return sCardType;
        }

        public SigPadCardData()
        {
            cardNo = "";
            cardType = "VISA";
            FirstName = "";
            LastName = "";
            pinNumber = "";
            ExpireOn = "";
            //Track1 = "";
            Completetrack = "";
            Track2 = "";
            //Added By SRT
            Track3 = "";
            IsValidData = true;
            errorMessage = "";
            errorNumber = "";
            paymentType = "";
            keySerialNumber = "";
        }

        public void ClearCardInfo()
        {
            cardNo = "";
            cardType = "";
            FirstName = "";
            LastName = "";
            pinNumber = "";
            ExpireOn = "";
            Track1 = "";
            Track2 = "";
            //Added By SRT
            Track3 = "";
            IsValidData = true;
            errorMessage = "";
            errorNumber = "";
            paymentType = "";
            keySerialNumber = "";
            this.PinNumber = "";
            this.KeySerialNumber = "";
        }
    }

    public class POSCallbackClass : MMSHostDelegate
    {
        //Holds the POS Client Reference

        SigPadUtil mmsObj;

        public POSCallbackClass(SigPadUtil mms)
        {
            this.mmsObj = mms;
        }

        protected override void CCCallback(ref ArrayList Data)
        {
            //Call renamed functions which are made public and intended for handling the
            //CC Processing code.
            mmsObj.MMSHost_CreditCardEvent(ref Data);
        }

        protected override void NoppCallback(ref string Signature, string sigtype)
        {
            //Call renamed functions which are made public and intended for handling the
            //NOPP Signature related code.
            mmsObj.MMSHost_NoppEvent(ref Signature, sigtype);
        }

        protected override void SignCallback(ref string Signature, string signOpt, string sigType /*Added by RiteshMx*/)
        {
            //Call renamed functions which are made public and intended for handling the
            // Signature Capture for Rx Approval related code.
            mmsObj.MMSHost_SignEvent(ref Signature, signOpt, sigType); /*Added by RiteshMx*/
        }

        protected override void RxSignCallback(ref string Signature, String sigType/*Added by RiteshMx*/)
        {
            //Call renamed functions which are made public and intended for handling the
            // Signature Capture for Rx Approval related code.
            mmsObj.MMSHost_RxApproveEvent(ref Signature, sigType); /*Added by RiteshMx*/
        }

        //Do not delete this function as this is related to Lease Expiration and needs to
        // be kept as it is. This is the most important function.

        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Author Dharmendra
        /// Functionality Description : This method gets the heartbeat from the Hostserver
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : Nov - 11 - 08
        /// </summary>
        /// <param name="sHeartBeatOk"></param>
        protected override void HeartBeatCallBack(ref string sHeartBeatOk)
        {
            mmsObj.MMSHost_HeartBeatEvent(ref sHeartBeatOk);
        }

        protected override void DeviceReconnectionCallBack(ref string sDeviceReconnectionAttempt)
        {
            // mmsObj.MMSHost_DeviceReconnectEvent(ref sDeviceReconnectionAttempt);
        }
    }
}