using System;
//Collections
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using MMS.PROCESSOR;
//DataAccess
using POS_Core.DataAccess;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using System.Windows.Forms;

using System.Timers;

//For Remoting Purpose
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting;

//Interfaces that will be used for Communication with PrimePO
using Vendor.Interface.Item;
using Vendor.Interface.PurchaseOrder;
using Vendor.Interface.Vendor;
//Using Data Objects that will be used across.
using Vendor.DAO.Item;
using Vendor.DAO.Vendor;
using Vendor.DAO.PurchaseOrder;
using Vendor.DataTier.PurchaseOrder;
using Vendor.CommonData.PurchaseOrder;
using System.Data;
using POS_Core.ErrorLogging;
using System.IO;

using System.Diagnostics;
using System.Configuration;
using System.Threading;
//using PrimeSearchClient.Interface;
using Resources;
using Vendor.Helper;//PRIMEPOS-2679
using Vendor.DAO.EDI;
using System.Linq;

namespace POS_Core_UI.Resources
{

    public class PrimePOUtil : Form
    {
        //PRIMEPOS-2679
        enum EDICommunicationMode
        {
            REMOTING,
            RESTAPI
        }
        #region constants
        public const int SUCCESS = 1;
        public const int MANUAL = 2;
        public const int ERROR = 3;

        public const int PODISCOONECT = 4;

        public const int ZERO = 0;
        public const int ONE = 1;
        #endregion
        //public delegate void UpdateLogData();
        //public static event UpdateLogData UpdateLogStatus;
        #region privateVariables
        //Arrange the variables in private and public Ritesh

        private Dictionary<int, int> poPendingdict = new Dictionary<int, int>();
        private Dictionary<int, int> poQueueddict = new Dictionary<int, int>();
        private Dictionary<int, int> poAckdict = new Dictionary<int, int>();

        private const string CONNECTED = "CONNECTED";
        private const string DISCONNECTED = "DISCONNECTED";

        private string itemString = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|";
        private string itemStringToinsert = string.Empty;
        private const string LASTUPDATE = "_LastUpdate.txt";

        //The Remote object shall communicate using TCP IP.
        //Interfaces for the PrimePO
        static EDICommunicationMode eEDICommunicationMode = POS_Core.Resources.Configuration.CInfo.OldEDIComatibility.ToUpper() == "Y" ? EDICommunicationMode.REMOTING : EDICommunicationMode.RESTAPI;  //PRIMEPOS-2679
        private IPurchaseOrder purchaseOrderInt; //=null;
        private IItem itemInt; //= null;
        private IVendorInterface vendorInt; //= null;
        //private ArrayList valueArrayList = null;

        //This is done to ensure the instance is singleton.
        private static PrimePOUtil primePOUtil;
        public static bool isConnected = false;//Status for whether its connected with PrimePO

        //Timers
        private System.Timers.Timer conTimer = null; //Timer for connection retry.
        private System.Timers.Timer orderUpdateTimer = null; //Timer for sending accumulated Pending orders.          
        private System.Timers.Timer priceUpdateTimer = null; //Timer for sending accumulated Pending orders. 

        private System.Timers.Timer timeToOrder = null;

        //HeartBeat
        //private System.Timers.Timer heartBeatTimer; //Timer for heartbeat.    //PRIMEPOS-3167 07-Nov-2022 JY Commented
        private const int MAXATTEMPT = 5;
        private object lockObj;
        private int TotalItemInserted = 0;
        int TotalItemUpdated = 0;
        int error = 0;
        string usePrimePO = "N";
        #endregion

        //Added by Krishna on 13 July 2012
        public static Boolean IsPOConnected
        {
            get { return isConnected; }
        }
        //End of added by Krishna

        //IPrimeSearchClient objService;
        public MMSSearch.Service objService;
        private PrimePOUtil()
        {
            lockObj = new object();

            //InitializeHeartBeatTimer();   //PRIMEPOS-3167 07-Nov-2022 JY Commented as not in use
            InitializeConnectionTimer();
            if (POS_Core.Resources.Configuration.StationID == "01") //PRIMEPOS-3167 07-Nov-2022 JY Added if condition to allow price update only with station 01
            {
                InitializationTimeToOrder();    //This workflow is used to change PO status from Incomplete to Pending after users consent
                InitializeOrderTimer(); //most critical workflow  - a. Fetch Queued, Submitted, Acknowledge, Expired records from PrimePOS and send it to EDI, Collect information from EDI and send it back to POS, update status and other parameters and also update sync flag back to EDI b. to sync 'DirectAcknowledged','DirectDelivery' records from EDI
                InitializePriceUpdateTimer();
            }
            //valueArrayList = new ArrayList();
            //objService = new PrimeSearchClientdll.SearchClient(objService.Url.ToString());
            objService = new MMSSearch.Service();
            //if (Configuration.CPOSSet.RemoteURL != string.Empty)  //PRIMEPOS-2671 24-Apr-2019 JY Commented
            //    objService.Url = Configuration.CPOSSet.RemoteURL.ToString();
            if (POS_Core.Resources.Configuration.CInfo.PSServiceAddress != string.Empty)   //PRIMEPOS-2671 24-Apr-2019 JY Added
            {
                objService.Url = POS_Core.Resources.Configuration.CInfo.PSServiceAddress.Trim() + @"Prime.SearchService.asmx";
            }
        }

        private void InitializationTimeToOrder()
        {
            if (POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer == ZERO) //Sprint-22 04-Nov-2015 JY changed PriceUpdateTimer to PurchaseOrdTimer  //PRIMEPOS-3167 07-Nov-2022 JY Modified
            {
                POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer = 5; //Sprint-22 04-Nov-2015 JY changed PriceUpdateTimer to PurchaseOrdTimer and value 1 to 5 //PRIMEPOS-3167 07-Nov-2022 JY Modified
            }
            int timeToOrderMin = POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer * 1000 * 60;  //Check where this can be put in the POSSetUtil    //PRIMEPOS-3167 07-Nov-2022 JY Modified

            //Initialisation
            if (timeToOrder == null)
            {
                timeToOrder = new System.Timers.Timer();
                timeToOrder.Interval = timeToOrderMin;
                timeToOrder.Enabled = false;
                timeToOrder.Elapsed += new ElapsedEventHandler(OnTimeToOrderEvent);
            }
        }

        #region
        //private void InitializeHeartBeatTimer()
        //{
        //    string heartBeats;
        //    int heartBeatMSec;
        //    //Here HeartBeat Frequency Is Converted In Milliseconds.            
        //    heartBeats = POS_Core.Resources.Configuration.CPOSSet.HeartBeatTime;
        //    heartBeatMSec = POS_Core.Resources.Configuration.convertNullToInt(POS_Core.Resources.Configuration.CPOSSet.HeartBeatTime.ToString()) * 1000;  // Convert.ToInt32(heartBeats) * 1000;
        //    //Initialisation
        //    heartBeatTimer = new System.Timers.Timer();
        //    heartBeatTimer.Interval = heartBeatMSec;
        //    heartBeatTimer.Enabled = false;
        //    heartBeatTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        //}

        //private void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        //    if (POS_Core.Resources.Configuration.CPOSSet.USePrimePO == false)
        //        return;
        //    bool fromLock = false;
        //    try
        //    {
        //        fromLock = Monitor.TryEnter(lockObj, 1000);
        //        if (fromLock)
        //        {
        //            heartBeatTimer.Enabled = false;
        //            heartBeatTimer.Stop();
        //            try
        //            {
        //                if (eEDICommunicationMode == EDICommunicationMode.REMOTING)
        //                {
        //                    //Addedto Test the PrimeEDI using lots of CPU Memory resources on 19June13
        //                    if (isConnected == false)
        //                    {
        //                        primePOUtil.purchaseOrderInt.Initialize();
        //                        primePOUtil.itemInt.Initialize();
        //                        primePOUtil.vendorInt.Initialize();
        //                    }
        //                }
        //                heartBeatTimer.Enabled = true;
        //            }
        //            catch (RemotingException ex)
        //            {
        //                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
        //                HandlePrimePOConnectionFailure();
        //            }
        //            catch (SocketException ex)
        //            {
        //                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
        //                HandlePrimePOConnectionFailure();
        //            }
        //            catch (Exception ex)
        //            {
        //                //this.conTimer.Start();//Dont know what to do
        //                ErrorHandler.logException(ex, "", "");
        //            }
        //        }
        //    }
        //    catch (TimeoutException ex)
        //    {
        //        Monitor.Exit(lockObj);
        //    }
        //    finally
        //    {
        //        if (fromLock)
        //            Monitor.Exit(lockObj);
        //    }
        //}
        #endregion

        private void InitializeConnectionTimer()
        {
            if (POS_Core.Resources.Configuration.CPrimeEDISetting.ConnectionTimer == ZERO)   //PRIMEPOS-3167 07-Nov-2022 JY Modified
            {
                POS_Core.Resources.Configuration.CPrimeEDISetting.ConnectionTimer = ONE; //PRIMEPOS-3167 07-Nov-2022 JY Modified
            }
            //Modified By Dharmendra On Apr-25-09
            //Multiplied the values by 60 since 1 min = 60 sec
            int conTimerMSec = POS_Core.Resources.Configuration.CPrimeEDISetting.ConnectionTimer * 60 * 1000;  //Check where this can be put in the POSSetUtil   //PRIMEPOS-3167 07-Nov-2022 JY Modified
            //Modified Till Here Apr-25-09

            //Initialisation
            if (conTimer == null)
            {
                conTimer = new System.Timers.Timer();
                conTimer.Interval = conTimerMSec;
                conTimer.Enabled = false;
                conTimer.Elapsed += new ElapsedEventHandler(OnConnectionRetryEvent);
            }
        }

        private void InitializePriceUpdateTimer()
        {
            if (POS_Core.Resources.Configuration.CPrimeEDISetting.PriceUpdateTimer == ZERO) //PRIMEPOS-3167 07-Nov-2022 JY Modified
            {
                POS_Core.Resources.Configuration.CPrimeEDISetting.PriceUpdateTimer = ONE;   //PRIMEPOS-3167 07-Nov-2022 JY Modified
            }
            int priceUpdateTimerMSec = POS_Core.Resources.Configuration.CPrimeEDISetting.PriceUpdateTimer * 1000 * 60 * 60;  //Cpo;   //Check where this can be put in the POSSetUtil   //PRIMEPOS-3167 07-Nov-2022 JY Modified

            //Initialisation
            if (priceUpdateTimer == null)
            {
                priceUpdateTimer = new System.Timers.Timer();
                priceUpdateTimer.Interval = priceUpdateTimerMSec;
                priceUpdateTimer.Enabled = false;
                priceUpdateTimer.Elapsed += new ElapsedEventHandler(OnPriceUpdateEvent);
            }
        }

        void StartTimer(System.Timers.Timer timer)
        {
            if (timer != null)
            {
                timer.AutoReset = true;
                timer.Enabled = true;
            }
        }

        void StopTimer(System.Timers.Timer timer)
        {
            if (timer != null)
            {
                timer.AutoReset = false;
                timer.Enabled = false;
                timer.Stop();
            }
        }

        private void InitializeOrderTimer()
        {
            if (POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer == ZERO)  //PRIMEPOS-3167 07-Nov-2022 JY Modified
            {
                POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer = ONE;    //PRIMEPOS-3167 07-Nov-2022 JY Modified
            }
            int orderUpdateTimerMSec = POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer * 1000 * 60; //Check where this can be put in the POSSetUtil   //PRIMEPOS-3167 07-Nov-2022 JY Modified
            //Initialisation
            orderUpdateTimer = new System.Timers.Timer();
            orderUpdateTimer.Interval = orderUpdateTimerMSec;
            orderUpdateTimer.Enabled = false;
            orderUpdateTimer.Elapsed += new ElapsedEventHandler(OnUpdatePendingOrderStatusEvent);
        }

        public static PrimePOUtil DefaultInstance
        {
            get
            {
                if (primePOUtil == null)
                {
                    primePOUtil = new PrimePOUtil();
                    primePOUtil.Connect();
                }
                /*
                 if (PrimePOUtil.isConnected != true)
                 {
                     //if (!primePOUtil.ConnectServer())
                     //{
                     if(!primePOUtil.Connect())
                      {
                         primePOUtil = null;
                      }
                     //}
                 }
                 */
                return primePOUtil;
            }
        }

        private bool Connect()
        {
            bool result = false;
            bool fromLock = false;
            try
            {
                fromLock = Monitor.TryEnter(lockObj, 1000);
                if (fromLock)
                {
                    result = ConnectServer();
                }
            }
            catch (TimeoutException ex)
            {
                Monitor.Exit(lockObj);
            }
            finally
            {
                if (fromLock)
                    Monitor.Exit(lockObj);
            }
            return result;
        }

        private bool ConnectServer()
        {
            bool retVal = true;
            if (POS_Core.Resources.Configuration.CPrimeEDISetting.UsePrimePO == false)   //PRIMEPOS-3167 07-Nov-2022 JY Modified
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                return false;
            }

            try
            {
                if (isConnected == false)
                {
                    POS_Core.ErrorLogging.Logs.Logger("Connecting to PrimeEDI ");

                    if (eEDICommunicationMode == EDICommunicationMode.REMOTING)
                    {
                        TcpChannel tcpChanel = null;
                        string hostPath = String.Empty;
                        Hashtable channelProperties = new Hashtable();
                        channelProperties["name"] = "TcpPO";
                        channelProperties["port"] = 1;
                        channelProperties["typeFilterLevel"] = "Full";

                        tcpChanel = new TcpChannel(channelProperties,
                            new BinaryClientFormatterSinkProvider(),
                            new BinaryServerFormatterSinkProvider());


                        if (RemotingConfiguration.ApplicationName != "MMSClient")
                            RemotingConfiguration.ApplicationName = "MMSClient";

                        if (ChannelServices.GetChannel(tcpChanel.ToString()) == null)
                            ChannelServices.RegisterChannel(tcpChanel, false);
                    }
                    if (primePOUtil == null)
                    {
                        primePOUtil = new PrimePOUtil();
                    }

                    if (eEDICommunicationMode == EDICommunicationMode.REMOTING)
                    {
                        primePOUtil.purchaseOrderInt = (IPurchaseOrder)Activator.GetObject(typeof(IPurchaseOrder), POS_Core.Resources.Configuration.CPrimeEDISetting.HostAddress + "PurchaseOrder");    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        primePOUtil.itemInt = (IItem)Activator.GetObject(typeof(IItem), POS_Core.Resources.Configuration.CPrimeEDISetting.HostAddress + "Item");    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        primePOUtil.vendorInt = (IVendorInterface)Activator.GetObject(typeof(IVendorInterface), POS_Core.Resources.Configuration.CPrimeEDISetting.HostAddress + "Vendor");  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        //willl try to call the method on remote server side to ensure the connectivity.

                        primePOUtil.purchaseOrderInt.Initialize();
                        primePOUtil.itemInt.Initialize();
                        primePOUtil.vendorInt.Initialize();
                    }
                    else
                    {
                        PrimePOEDIHelper oPrimePOEDIHelper = new PrimePOEDIHelper();
                        purchaseOrderInt = new PrimePOUtilPurchaseOrder();
                        itemInt = new PrimePOUtilItem();
                        vendorInt = new PrimePOUtilVendor();

                        int iCount = 1;
                        do
                        {
                            retVal = oPrimePOEDIHelper.IsServerRunning();
                            Thread.Sleep(2000);//Retry after 2 seconds
                            iCount++;
                        } while (retVal != true && iCount < 4);

                        if (retVal != true)
                        {
                            throw new Exception("PO is Diconnected");
                        }
                    }
                    primePOUtil.itemInt.NDC = "N";
                    //long id = 2;
                    //itemInt.GetLatestItemData(ref id);

                    //Mark status as connected
                    PrimePOUtil.isConnected = true;
                    ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(CONNECTED);

                    //Stop the reconnection off till the time connection breaks off.
                    StopTimer(conTimer);

                    //this.conTimer.Enabled = false;
                    //Start the timer to send pending orders and status of Orders at PrimePO

                    if (POS_Core.Resources.Configuration.StationID == "01") //PRIMEPOS-3167 07-Nov-2022 JY Added if condition to allow price update only with station 01
                    {
                        if (orderUpdateTimer != null)
                            StartTimer(orderUpdateTimer);
                        else
                        {
                            InitializeOrderTimer();
                            StartTimer(orderUpdateTimer);
                        }

                        //Start Heartbeat timer
                        //check this condition if primpo is not available at 
                        //first time
                        //PRIMEPOS-3167 07-Nov-2022 JY Commented as not in use
                        //if (heartBeatTimer != null)
                        //    StartTimer(heartBeatTimer);
                        //else
                        //{
                        //    InitializeHeartBeatTimer();
                        //    StartTimer(heartBeatTimer);
                        //}

                        if (priceUpdateTimer != null)
                            StartTimer(priceUpdateTimer);
                        else
                        {
                            InitializePriceUpdateTimer();
                            StartTimer(priceUpdateTimer);
                        }

                        if (timeToOrder != null)
                            StartTimer(timeToOrder);
                        else
                        {
                            InitializationTimeToOrder();
                            StartTimer(timeToOrder);
                        }
                    }

                    POS_Core.ErrorLogging.Logs.Logger("Connected to PrimeEDI ");
                }
            }
            catch (RemotingException ex)
            {
                POS_Core.ErrorLogging.Logs.Logger(ex.Message + " ");
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                retVal = false;
            }
            catch (SocketException ex)
            {
                POS_Core.ErrorLogging.Logs.Logger(ex.Message + " ");
                //conTimer.Close(); 
                //conTimer.Dispose();
                //clsUIHelper.ShowErrorMsg("POSERVER IS NOT RUNNING");
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                retVal = false;
            }
            catch (Exception ex)
            {
                POS_Core.ErrorLogging.Logs.Logger(ex.Message + " ");

                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                retVal = false;
            }
            return retVal;
        }

        private void HandlePrimePOConnectionFailure()
        {
            //Clean the objects if any.
            DisposeObjects();
            //isConnected = false; //mark status to not connected.
            //Start the retry connection timer.
            //InitializeConnectionTimer();
            StartTimer(conTimer);
            //Stop the OrderUpdatorTimer
            StopTimer(orderUpdateTimer);

            //Stop the HeartBeat Timer.
            //StopTimer(heartBeatTimer);     //PRIMEPOS-3167 07-Nov-2022 JY Commented as not in use

            //Stop Price Update Timer 
            StopTimer(priceUpdateTimer);

            //Stop Time To Order Timer
            StopTimer(timeToOrder);
        }

        void OnTimeToOrderEvent(object sender, ElapsedEventArgs e)
        {
            bool fromLock = false;
            try
            {
                fromLock = Monitor.TryEnter(lockObj, 1000);
                if (fromLock)
                {

                    POHeaderSvr poHeaderSvr = new POHeaderSvr();
                    POHeaderData poHeaderData = poHeaderSvr.PopulateList(" AND status =" + ((int)PurchseOrdStatus.Incomplete).ToString()); //Select only the onces whihc are in Queued state.
                    PODetailData poDetailData = new PODetailData();
                    PODetailSvr poDetailSvr = new PODetailSvr();
                    PODetailData poDetaData = new PODetailData();

                    try
                    {
                        if (poHeaderData.POHeader.Rows.Count > 0)
                        {
                            PurchaseOrder poOrder = new PurchaseOrder();

                            foreach (POHeaderRow rows in poHeaderData.POHeader.Rows)
                            {
                                poDetaData = poDetailSvr.Populate(rows.OrderID);
                                poDetailData.Merge(poDetaData);
                            }

                            foreach (POHeaderRow rows in poHeaderData.POHeader.Rows)
                            {
                                POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
                                DateTime timeOfDay = new DateTime();
                                VendorData vendData = vendor.Populate(rows.VendorID);
                                if (vendData.Vendor[0].IsAutoClose == true)
                                {
                                    if (DateTime.TryParse(vendData.Vendor[0].TimeToOrder.ToString(), out timeOfDay))
                                    {
                                        if (DateTime.Now >= Convert.ToDateTime(vendData.Vendor[0].TimeToOrder.ToString()).Subtract(new TimeSpan(0, 2 * (POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer), 0))
                                         && DateTime.Now <= Convert.ToDateTime(vendData.Vendor[0].TimeToOrder.ToString()).Subtract(new TimeSpan(0, POS_Core.Resources.Configuration.CPrimeEDISetting.PurchaseOrderTimer, 0)))  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                        {
                                            if (Resources.Message.Display("Do You Want To Close Incomplete Order " + rows.OrderNo + " For Vendor " + vendData.Vendor[0].Vendorname + "?", "Time To Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                            {
                                                rows.Status = (int)PurchseOrdStatus.Pending;
                                                poOrder.Persist(poHeaderData, poDetailData);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (RemotingException ex)
                    {
                        ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                        HandlePrimePOConnectionFailure();
                    }
                    catch (SocketException ex)
                    {
                        ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                        HandlePrimePOConnectionFailure();
                    }
                    catch (Exception ex)
                    {
                        //Dont know what can be done?? Probably throw the exception
                        //or call he same HandlePrimePOCOnnectionFailure.
                        ErrorHandler.logException(ex, "", "");
                    }
                }
            }
            catch (TimeoutException ex)
            {
                Monitor.Exit(lockObj);
            }
            finally
            {
                if (fromLock)
                    Monitor.Exit(lockObj);
            }
        }

        void OnConnectionRetryEvent(object sender, ElapsedEventArgs e)
        {
            //This timer even will esnsure that the connection is retried
            //Disable connection timer.
            bool fromLock = false;
            this.conTimer.Enabled = false;
            this.conTimer.AutoReset = false;
            this.conTimer.Stop();
            if (POS_Core.Resources.Configuration.CPrimeEDISetting.UsePrimePO == false)   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                return;
            if (PrimePOUtil.isConnected != true)
            {
                try
                {
                    fromLock = Monitor.TryEnter(lockObj, 1000);
                    if (fromLock)
                    {

                        // ConnectServer(); //Need to see what can be done over here.                   
                        Connect();
                    }
                }
                catch (TimeoutException ex)
                {
                    Monitor.Exit(lockObj);
                }
                finally
                {
                    if (fromLock)
                        Monitor.Exit(lockObj);
                }
            }
        }

        public void OnUpdatePendingOrderStatusEvent(object sender, ElapsedEventArgs e)    //PRIMEPOS-3167 07-Nov-2022 JY Modified access level  
        {

            /*
            1) Update the local orders from the status of PrimePO order status. 
            */
            //bool fromLock = Monitor.TryEnter(lockObj, 1000);

            bool fromLock = false;
            try
            {
                fromLock = Monitor.TryEnter(lockObj, 1000);
                if (fromLock)
                {
                    POS_Core.ErrorLogging.Logs.Logger(" OnUpdatePendingOrderStatusEvent()");

                    if (POS_Core.Resources.Configuration.UpdateInProgress == true)
                        return;
                    else
                    {
                        POHeaderSvr poHeaderSvr = new POHeaderSvr();
                        POHeaderData poHeaderData = poHeaderSvr.PopulateList(" AND status IN (" + (int)PurchseOrdStatus.Queued + " , " + (int)PurchseOrdStatus.Submitted + " , " + (int)PurchseOrdStatus.Expired + " , " + (int)PurchseOrdStatus.Acknowledge + ")"); //Select only the ones which are in Queued,submitted,expired & Acknowledge state.
                        PODetailData poDetailData = new PODetailData();
                        PODetailSvr poDetailSvr = new PODetailSvr();
                        PODetailData poDetaData = new PODetailData();

                        try
                        {
                            // Add By SRT(Sachin) Data : 18 FEb 2010
                            //Add Direct PO in POS Database
                            long OrderID = 0;
                            int poIdCount = 0;
                            PurcahseOrderDO[] directPoData;//= new PurcahseOrderDO[poIdCount];
                            try
                            {
                                PurchaseOrderDetailsDO[] puchaseOrdDetDO;//= new PurchaseOrderDetailsDO();
                                PurchaseOrdDetailsData purchaseOrdDetData = new PurchaseOrdDetailsData();
                                PurchaseOrderDetaisSrv purchaseoOrdDetSrv = new PurchaseOrderDetaisSrv();
                                POS_Core.ErrorLogging.Logs.Logger(" Get Direct Purchase Order detail from PrimeEDI ");
                                directPoData = primePOUtil.purchaseOrderInt.GetDirectPODetails("POSs", ref poIdCount);

                                poIdCount = directPoData.Length;
                                if (poIdCount > 0)
                                {
                                    for (int poID = 0; poID < poIdCount; poID++)
                                    {
                                        try
                                        {
                                            //ErrorLogging.Logs.Logger("" + Environment.NewLine + "Prime EDI FileID "
                                            POHeaderData poHeaderData1 = new POHeaderData();
                                            POHeaderRow oPOHeaderRow = poHeaderData1.POHeader.NewPOHeaderRow();
                                            PODetailData poDetData = new PODetailData();
                                            PODetailData poDetOnHoldData = new PODetailData();
                                            POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
                                            string strVendorCode = directPoData[poID].VendorCode;
                                            VendorData vendorData = vendor.Populate(directPoData[poID].VendorCode);

                                            oPOHeaderRow.OrderID = poID + 1;
                                            OrderID = directPoData[poID].OrderId;
                                            //LogData.Write("** FTP Delete - Start Delete()", LogData.ThroughTheFlow);
                                            POS_Core.ErrorLogging.Logs.Logger(" **Start Purchase Order detail Update  PrimeEDI OrderID: " + directPoData[poID].OrderId);
                                            oPOHeaderRow.OrderNo = directPoData[poID].OrderNo;
                                            oPOHeaderRow.VendorID = (int)vendorData.Vendor[0].VendorId;//**
                                                                                                       //User
                                            oPOHeaderRow.OrderDate = directPoData[poID].OrderDate;
                                            oPOHeaderRow.ExptDelvDate = directPoData[poID].ExptDeliveryDate;
                                            //oPOHeaderRow.Status = directPoData[poID].Status;
                                            //oPOHeaderRow.Status = GetStatus("DirectAcknowledged");
                                            oPOHeaderRow.Status = GetStatus(directPoData[poID].Status.Trim());
                                            oPOHeaderRow.AckStatus = directPoData[poID].AckStatus;
                                            oPOHeaderRow.AckDate = directPoData[poID].AckDate;
                                            oPOHeaderRow.PrimePOrderId = directPoData[poID].OrderId;
                                            oPOHeaderRow.RefOrderNO = directPoData[poID].RefOrderID;//added by Ravindra(QuicSolv) 17 jan 2013
                                            oPOHeaderRow.AckFileType = directPoData[poID].AckFileType; //Added By shitaljit for strong file type JIRA-911
                                            oPOHeaderRow.TransTypeCode = directPoData[poID].TransTypeCode;  //PRIMEPOS-2901 05-Nov-2020 JY Added
                                            directPoData[poID].Synchronize = true;
                                            puchaseOrdDetDO = directPoData[poID].PurchaseOrdDetail;
                                            int rowCount = puchaseOrdDetDO.Length; //purchaseOrdDetData.PODetail.Rows.Count;
                                            string orderRefNo = directPoData[poID].RefOrderID;
                                            if (rowCount > 0)
                                            {
                                                POS_Core.ErrorLogging.Logs.Logger("Purchase Order Update for  DirectAcknowledged/DirectDelivery " + Environment.NewLine + " PrimeEDIOrderID " + oPOHeaderRow.PrimePOrderId + " Vendor Code " + strVendorCode);

                                                for (int count = 0; count < rowCount; count++)
                                                {
                                                    PODetailRow row = poDetData.PODetail.NewPODetailRow();
                                                    row.OrderID = puchaseOrdDetDO[count].OrderID;
                                                    row.ItemID = puchaseOrdDetDO[count].ItemCode;//Chk
                                                    if (directPoData[poID].AckFileType == "810")//added by shitaljit for 810 file
                                                    {
                                                        row.QTY = puchaseOrdDetDO[count].InvoiceQty;
                                                    }
                                                    else //855 file
                                                    {
                                                        row.QTY = puchaseOrdDetDO[count].Qty;
                                                    }
                                                    row.Cost = puchaseOrdDetDO[count].Cost;
                                                    row.AckQTY = puchaseOrdDetDO[count].AckQty;
                                                    row.AckStatus = puchaseOrdDetDO[count].AckStatus;
                                                    row.VendorItemCode = puchaseOrdDetDO[count].VendorItemCode;
                                                    row.ChangedProductQualifier = puchaseOrdDetDO[count].ChangedProductQualifier;
                                                    row.ChangedProductID = puchaseOrdDetDO[count].ChangedProductID;

                                                    getValidItemCode(ref row, puchaseOrdDetDO[count].Idescription);

                                                    //if - else if added By Shitaljit on 5/27/2014 to insert PO on hold in case there are items with invalid UPC/itemcode
                                                    //Check if itemID is not NA- Not Available item code
                                                    if (row.ItemID.ToUpper().ToString().Trim().Equals(clsPOSDBConstants.NotAvailableItemCode) == false)
                                                    {
                                                        poDetData.PODetail.AddRow(row, true);
                                                    }
                                                    else
                                                    {
                                                        poDetOnHoldData.PODetail.AddRow(row, true);
                                                    }
                                                }
                                            }

                                            bool s = directPoData[poID].Synchronize;
                                            string ss = directPoData[poID].Application;
                                            string sss = directPoData[poID].RefOrderID;
                                            poHeaderData1.POHeader.AddRow(oPOHeaderRow, true);
                                            PurchaseOrder purchaseOrder = new PurchaseOrder();
                                            //ValidItemCode(poDetData);
                                            if (ValidOrderNo(directPoData[poID].OrderNo))
                                            {
                                                string NewPONumber = poHeaderSvr.GetNextPONumber();
                                                int strONo = Convert.ToInt32(NewPONumber) + 1;
                                                //Following Line of code is commented by shitaljit on 29 august 2012
                                                //if PO OrderNo is exist in POS PurchaseOrder it increment the orderNo so it dint get synchronize in PO database 
                                                //infact it adds new record and keep incrementing bot POS and PO entries.
                                                //directPoData[poID].OrderNo = strONo.ToString();
                                                oPOHeaderRow.OrderNo = strONo.ToString();
                                            }
                                            purchaseOrder.Persist(poHeaderData1, poDetData);
                                            directPoData[poID].OrderId = OrderID;
                                            //Added By Shitaljit on 5/27/2014 to insert PO on hold in case there are items with invalid UPC/itemcode
                                            if (POS_Core.Resources.Configuration.isNullOrEmptyDataSet(poDetOnHoldData) == false)
                                            {
                                                purchaseOrder.PutOnHold(poHeaderData1, poDetOnHoldData);
                                                POS_Core.ErrorLogging.Logs.Logger("purchase Order instreated on Hold for PrimeEDIOrderID " + oPOHeaderRow.PrimePOrderId + " Order Status " + directPoData[poID].Status + " Vendor Code " + strVendorCode);
                                            }
                                            primePOUtil.purchaseOrderInt.UpdateSynchronize(directPoData[poID]);
                                            POS_Core.ErrorLogging.Logs.Logger("**End with purchase Order Update for PrimeEDIOrderID " + oPOHeaderRow.PrimePOrderId + " Order Status " + directPoData[poID].Status + " Vendor Code " + strVendorCode);
                                        }
                                        catch (Exception ex)
                                        {

                                            POS_Core.ErrorLogging.Logs.Logger("Erro in DirectAck OrderProcessing: " + ex.Message);
                                        }
                                    }
                                }

                            }
                            catch (Exception er)
                            {
                                POS_Core.ErrorLogging.Logs.Logger(er.Message);
                                throw new Exception(er.Message);
                            }

                            if (poHeaderData.POHeader.Rows.Count > 0)
                            {

                                Dictionary<long, string> dict = new Dictionary<long, string>();
                                //int count = 0;
                                POS_Core.ErrorLogging.Logs.Logger("**Start Update Purchase order in Queue");
                                foreach (POHeaderRow rows in poHeaderData.POHeader.Rows)
                                {
                                    if (rows.PrimePOrderId > 0 && !dict.ContainsKey(rows.PrimePOrderId))
                                    {
                                        dict.Add(rows.PrimePOrderId, "");
                                        poDetaData = poDetailSvr.Populate(rows.OrderID);

                                        poDetailData.Merge(poDetaData);
                                        POS_Core.ErrorLogging.Logs.Logger("**Merge PurchaseOrder Detail EDIOrderID:" + rows.PrimePOrderId);
                                    }
                                }
                                if (!UpdatePOStatus(ref dict, ref poHeaderData, ref poDetailData))
                                {
                                    throw new Exception("Could not update the local orderstatus");
                                }
                            }
                            /*
                            2) fetch the pending orders to be sent across to the PrimePO. 
                            */
                            if (!SendPendingOrders())
                            {
                                throw new Exception(" could not send PO ");
                            }
                        }
                        catch (RemotingException ex)
                        {
                            ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                            HandlePrimePOConnectionFailure();
                        }
                        catch (SocketException ex)
                        {
                            ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                            HandlePrimePOConnectionFailure();
                        }
                        catch (Exception ex)
                        {
                            //ErrorHandler.logException(ex, "", "");    //PRIMEPOS-2971 04-Jun-2021 JY Commented as no need to log it in errorlog
                        }
                    }
                }
            }
            catch (TimeoutException ex)
            {
                Monitor.Exit(lockObj);
            }
            finally
            {
                if (fromLock)
                    Monitor.Exit(lockObj);
            }
        }
        private ItemRow oItemRow;
        public ItemRow CurrentItem
        {
            get
            {
                return (oItemRow);
            }
        }
        //added by SRT (Sachin) Date : 18 Feb 2010
        public bool ValidOrderNo(string OrderNo)
        {
            POHeaderSvr poHeaderSvr = new POHeaderSvr();
            POHeaderData poHData = new POHeaderData();
            if (OrderNo != string.Empty)
            {
                poHData = poHeaderSvr.PopulateList(" and OrderNo = '" + OrderNo + "'");
            }
            if (poHData.POHeader.Rows.Count > 0)
                return true;
            else
                return false;

        }
        public void getValidItemCode(ref PODetailRow PODetRow, string Description)
        {
            try
            {
                ItemData itemData = new ItemData();
                ItemSvr itemSvr = new ItemSvr();
                ItemRow oItemRow = CurrentItem;

                VendorData vendorData = new VendorData();
                ItemVendorData oItemVendorData = new ItemVendorData();
                ItemVendor oItemVendor = new ItemVendor();

                //Item code NA then put item on hold do not add it into Purchase Order table or insert item to item table
                if (PODetRow.ItemID.ToUpper().ToString().Trim().Equals(clsPOSDBConstants.NotAvailableItemCode) == true)
                {
                    return;
                }

                if (PODetRow.ItemID.ToString().Trim().Length == 12)
                {
                    itemData = itemSvr.PopulateList(" Where ItemID = '" + PODetRow.ItemID.ToString() + "'"); //(po.PurchaseOrdDetail[count].ItemCode.ToString());
                    if (itemData.Item.Rows.Count <= 0)
                    {
                        itemData = itemSvr.PopulateList(" Where ItemID like '" + PODetRow.ItemID.Substring(0, PODetRow.ItemID.Length - 1) + "%' "); //(po.PurchaseOrdDetail[count].ItemCode.ToString());
                    }
                }
                else
                {
                    itemData = itemSvr.PopulateList(" Where ItemID like '" + PODetRow.ItemID.ToString() + "%' "); //(po.PurchaseOrdDetail[count].ItemCode.ToString());                
                }
                if (itemData.Item.Rows.Count > 0)
                {
                    PODetRow.ItemID = itemData.Item[0].ItemID;
                }
                else
                {
                    //POS_Core.ErrorLogging.//Logs.Logger("PrimeEDI Connection Initilization Started.");
                    ItemVendor itemVendor = new ItemVendor();
                    POS_Core.ErrorLogging.Logs.Logger("Adding new item from DirectAcknowledged/DirectDelivery " + Environment.NewLine + " OrderID " + PODetRow.OrderID);
                    ItemDO itemDo = new ItemDO();
                    Item item = new Item();
                    if ((PODetRow.ItemID.Length == 12) && (POS_Core.Resources.Configuration.CPrimeEDISetting.Insert11DigitItem == true))    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        PODetRow.ItemID = PODetRow.ItemID.Substring(0, PODetRow.ItemID.Length - 1);
                    else
                        PODetRow.ItemID = PODetRow.ItemID;

                    oItemRow = itemData.Item.AddRow("", 0, "", "", "", "", "", "", 0, 0, 0, 0, false, 0, false, 0, DBNull.Value
                                , DBNull.Value, 0, 0, "", 0, 0, 0, null
                                , "", "", System.DateTime.MinValue, System.DateTime.MinValue, "", false, false, false, false, true, 0, false, 0, "", true, true, false, 0
                                , 0, 0, 0, 0
                                );  //Sprint-21 - 2173 06-Jul-2015 JY Added "True" parameter for IsActive  //PRIMEPOS-2592 01-Nov-2018 JY Added "false" for IsNonRefundable// Added for Solutran: 0,0,0,0 - PRIMEPOS-2663 - NileshJ - 05-July-2019

                    //long fileID = item.GetMaxFileId();
                    oItemRow.ItemID = PODetRow.ItemID;
                    oItemRow.DepartmentID = POS_Core.Resources.Configuration.CInfo.DefaultDeptId;
                    oItemRow.SellingPrice = PODetRow.Price;
                    if (Description != null)
                        oItemRow.Description = Description;
                    POS_Core.Resources.Configuration.UpdatedBy = "E";
                    item.Persist(itemData);

                    POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                    VendorData oVendorData = new VendorData();
                    oVendorData = oVendor.Populate(PODetRow.VendorID);

                    IDbTransaction tr = null;
                    IDbConnection conn = DataFactory.CreateConnection();
                    IDbCommand cmd = DataFactory.CreateCommand();

                    string sSQL = "INSERT INTO ITEMVENDOR (ITEMID,VENDORITEMID,VENDORID,VENDORCOSTPRICE,USERID) VALUES"
                        + "('" + PODetRow.ItemID + "','" + PODetRow.VendorItemCode + "'," + PODetRow.VendorID
                        + "," + PODetRow.Cost + ",'" + POS_Core.Resources.Configuration.UserName + "')";
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    conn.Open();
                    tr = conn.BeginTransaction();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sSQL;
                    cmd.Transaction = tr;
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                    tr.Commit();
                    tr = null;
                    conn.Close();
                    POS_Core.ErrorLogging.Logs.Logger("New item added from DirectAcknowledged/DirectDelivery " + Environment.NewLine + " OrderID " + PODetRow.OrderID + "ItemCode" + PODetRow.ItemID);
                }
            }
            catch (Exception ex)
            { POS_Core.ErrorLogging.Logs.Logger(ex.Message + " " + DateTime.Now.ToString()); }
        }
        public bool ValidItemCode(PODetailData poDetailData)
        {
            bool isExist = false;
            int count = 0;
            try
            {
                for (count = 0; count < poDetailData.PODetail.Rows.Count; count++)
                {
                    ItemData itemData = new ItemData();
                    ItemSvr itemSvr = new ItemSvr();
                    ItemRow oItemRow = CurrentItem;

                    VendorData vendorData = new VendorData();
                    ItemVendorData oItemVendorData = new ItemVendorData();
                    ItemVendor oItemVendor = new ItemVendor();
                    if (poDetailData.PODetail[count].ItemID.ToString().Trim().Length == 12)
                    {
                        itemData = itemSvr.PopulateList(" Where ItemID = '" + poDetailData.PODetail[count].ItemID.ToString() + "'"); //(po.PurchaseOrdDetail[count].ItemCode.ToString());
                    }
                    else
                    {
                        itemData = itemSvr.PopulateList(" Where ItemID like '" + poDetailData.PODetail[count].ItemID.ToString() + "%' "); //(po.PurchaseOrdDetail[count].ItemCode.ToString());
                        if (itemData.Item.Rows.Count > 0)
                            poDetailData.PODetail[count].ItemID = itemData.Item[0].ItemID;
                    }
                    if (itemData.Item.Rows.Count > 0)
                    {
                        isExist = true;
                    }
                    else
                    {


                        // string tempVendId = string.Empty;
                        //if (vendorData != null && vendorData.Tables.Count > 0 && vendorData.Tables[0].Rows.Count > 0)
                        //{
                        //    tempVendId = vendorData.Tables[0].Rows[0][0].ToString();
                        //    oItemVendorData = oItemVendor.PopulateList(" WHERE ItemId='" + poDetailData.PODetail[count].ItemID + "' and VendorItemId = '1234' and ItemVendor.vendorid=" + tempVendId);
                        //}
                        //else
                        //{
                        //    oItemVendorData = oItemVendor.PopulateList(" WHERE ItemId='" + poDetailData.PODetail[count].ItemID + "' and VendorItemId = '1234'");
                        //}


                        ItemVendor itemVendor = new ItemVendor();

                        ItemDO itemDo = new ItemDO();
                        Item item = new Item();

                        //vendorData = vendor.Populate(poDetailData.PODetail[count].vendorCode);
                        //Added By Shitaljit(QuicSolv) on 1 July 2011
                        //Added last "false" value for isEBTItem field to set it false By default
                        oItemRow = itemData.Item.AddRow("", 0, "", "", "", "", "", "", 0, 0, 0, 0, false, 0, false, 0, DBNull.Value
                            , DBNull.Value, 0, 0, "", 0, 0, 0, null
                            , "", "", System.DateTime.MinValue, System.DateTime.MinValue, "", false, false, false, false, true, 0, false, 0, "", true, true, false, 0
                            , 0, 0, 0, 0
                            ); //Sprint-21 - 2173 06-Jul-2015 JY Added "True" parameter for IsActive   //PRIMEPOS-2592 01-Nov-2018 JY Added "false" for IsNonRefundable // Added for Solutran: 0,0,0,0 - PRIMEPOS-2663 - NileshJ - 05-July-2019

                        //long fileID = item.GetMaxFileId();
                        oItemRow.ItemID = poDetailData.PODetail[count].ItemID;
                        oItemRow.DepartmentID = POS_Core.Resources.Configuration.CInfo.DefaultDeptId;
                        oItemRow.SellingPrice = poDetailData.PODetail[count].Price;
                        POS_Core.Resources.Configuration.UpdatedBy = "E";
                        item.Persist(itemData);

                        POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                        VendorData oVendorData = new VendorData();
                        //oVendorData = oVendor.Populate(poDetailData.PODetail[count].VendorID);

                        IDbTransaction tr = null;
                        IDbConnection conn = DataFactory.CreateConnection();
                        IDbCommand cmd = DataFactory.CreateCommand();

                        string sSQL = "INSERT INTO ITEMVENDOR (ITEMID,VENDORITEMID,VENDORID,VENDORCOSTPRICE,USERID) VALUES"
                            + "('" + poDetailData.PODetail[count].ItemID + "','" + poDetailData.PODetail[count].VendorItemCode + "'," + poDetailData.PODetail[count].VendorID
                            + "," + poDetailData.PODetail[count].Cost + ",'" + POS_Core.Resources.Configuration.UserName + "')";
                        conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

                        conn.Open();
                        tr = conn.BeginTransaction();

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sSQL;
                        cmd.Transaction = tr;
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        tr = null;
                        conn.Close();



                    }
                }
            }
            catch (Exception ex)
            {
                ;
            }
            return isExist;
        }
        //End of Add by SRT(Sachin) Date : 18 Feb 2010

        private void OnPriceUpdateEvent(object sender, ElapsedEventArgs e)
        {
            long poFileID = 0;
            try
            {

                try
                {
                    //PRIMEPOS-2679
                    if (eEDICommunicationMode == EDICommunicationMode.REMOTING)
                    {
                        //primePOUtil.purchaseOrderInt.Initialize();
                        //primePOUtil.itemInt.Initialize();
                        primePOUtil.vendorInt.Initialize();
                    }
                }
                catch (Exception ex)
                {
                    POS_Core.ErrorLogging.Logs.Logger(ex.Message);
                }
                bool fromLock = false;
                try
                {
                    fromLock = Monitor.TryEnter(lockObj, 1000);
                    if (fromLock)
                    {
                        poFileID = GetMaxFileID();
                        FileHistory fileHistory = new FileHistory();
                        long posFileId = fileHistory.GetMaxFileID();

                        if (poFileID == 0)
                        {
                            return;
                        }
                        if (poFileID == posFileId)
                            return;
                        else
                        {
                            posFileId++;
                            UpdatePrice(posFileId);
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                    Monitor.Exit(lockObj);
                }
                finally
                {
                    if (fromLock)
                        Monitor.Exit(lockObj);
                }
            }
            catch (RemotingException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
            }
            catch (SocketException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
            }
            catch (Exception ex)
            {
                //Need to think what can be done over here.  
                ErrorHandler.logException(ex, "", "");
            }
        }

        private bool SendPendingOrders()
        {

            // try getting all the orders in PrimePOS whichh are in Pending state.
            //POSrv and PODetailSRV  Status = Pending --> SendPO()
            //send the orders one by one to primePO using SendPO

            bool retVal = true;
            POHeaderSvr poHeaderSvr = new POHeaderSvr();
            POHeaderData poHeaderData = poHeaderSvr.PopulateList(" AND status=" + ((int)PurchseOrdStatus.Pending).ToString()); //Only select the orders which are in pending status
            PODetailData poDetailData = new PODetailData();
            POS_Core.BusinessRules.PurchaseOrder purchaseOrder = new POS_Core.BusinessRules.PurchaseOrder();

            try
            {
                if (poHeaderData.POHeader.Rows.Count > 0)
                {
                    //create a dataset with the qualified records.
                    foreach (POHeaderRow rows in poHeaderData.POHeader.Rows)
                    {
                        PODetailSvr poDetailSvr = new PODetailSvr();
                        PODetailData poDetData = new PODetailData();
                        poDetData = poDetailSvr.Populate(rows.OrderID);
                        poDetailData.Merge(poDetData);
                    }
                    //for loop over here.
                    int count = 0;
                    if (poHeaderData.POHeader.Rows.Count > 0)
                    {
                        foreach (POHeaderRow poHeaderRow in poHeaderData.POHeader.Rows)
                        {
                            POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
                            VendorData vendorData = vendor.Populate(poHeaderRow.VendorCode);
                            DateTime timeOfDay = DateTime.Now;

                            if (vendorData.Vendor.Rows.Count > 0)
                            {
                                if (vendorData.Vendor[0].IsAutoClose == true)
                                {
                                    if (DateTime.TryParse(vendorData.Vendor[0].TimeToOrder.ToString(), out timeOfDay))
                                    {
                                        if (DateTime.Now <= Convert.ToDateTime(vendorData.Vendor[0].TimeToOrder.ToString()))
                                        {
                                            POHeaderTable poHeaderTable = new POHeaderTable();
                                            POHeaderData poData = new POHeaderData();

                                            PODetailData poDetData = new PODetailData();
                                            poHeaderTable.ImportRow(poHeaderRow);
                                            poData.Merge(poHeaderTable);

                                            DataRow[] row = poDetailData.PODetail.Select("OrderID =" + POS_Core.Resources.Configuration.convertNullToInt(poHeaderData.POHeader[count].OrderID.ToString()));

                                            foreach (DataRow rw in row)
                                            {
                                                poDetData.PODetail.ImportRow(rw);
                                            }

                                            int isSend = SendPO(poData, poDetData);
                                            switch (isSend)
                                            {
                                                case SUCCESS:
                                                    {
                                                        //poHeaderData.POHeader[count].Status = (int)PurchseOrdStatus.Queued; //2; //Set it to Queued. (Create an enum)
                                                        //poHeaderData.POHeader[count].PrimePOrderId = 
                                                        //purchaseOrder.Persist(poHeaderData, poDetailData);
                                                        ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + ",  Status - " + PurchseOrdStatus.Queued.ToString());
                                                        ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                                                        retVal = true;
                                                    }
                                                    break;
                                                case MANUAL:
                                                    {
                                                        //dont put for loop over herre
                                                        //Set it to Acknowledge without sending. (Create an enum)
                                                        poHeaderData.POHeader[count].Status = (int)PurchseOrdStatus.AcknowledgeManually; // 5;
                                                        purchaseOrder.Persist(poHeaderData, poDetailData);

                                                        ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + ", Status - " + PurchseOrdStatus.AcknowledgeManually.ToString());
                                                        ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                                                        retVal = true;
                                                    }
                                                    break;
                                                case ERROR:
                                                    throw new Exception("Could not send to PrimePO");
                                            }
                                            count++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (RemotingException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                retVal = false;
            }
            catch (SocketException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                retVal = false;
            }
            catch (Exception ex)
            {
                //Need to think what can be done over here.
                //ErrorHandler.throwException(ex, "", "");

                ErrorHandler.logException(ex, "", "");
                ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Could not send to PrimePO");
                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
            }
            return retVal;
        }

        public static void CloseDefaultInstace()
        {
            if (primePOUtil != null)
            {
                primePOUtil.DisposeObjects();
            }
            primePOUtil = null;
        }


        private void DisposeObjects()
        {
            try
            {
                if (eEDICommunicationMode == EDICommunicationMode.REMOTING)
                {
                    foreach (System.Runtime.Remoting.Channels.IChannel oChannel in System.Runtime.Remoting.Channels.ChannelServices.RegisteredChannels)
                    {
                        if (oChannel.GetType() == typeof(System.Runtime.Remoting.Channels.Tcp.TcpChannel))
                        {
                            if (oChannel.ToString() != null)
                            {
                                System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(oChannel);
                                break;
                            }
                        }
                    }
                }
            }

            catch (RemotingException ex)
            {
                // added to prevent the exception while unregistering the channel  
                // System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(tcpChanel);
                ErrorHandler.logException(ex, "", "");
            }
            finally
            {
                //tcpChanel = null;                
                if (eEDICommunicationMode == EDICommunicationMode.REMOTING) //PRIMEPOS-2679
                {
                    vendorInt = null;
                    itemInt = null;
                    purchaseOrderInt = null;
                }
                StopTimer(orderUpdateTimer);
                //StopTimer(heartBeatTimer);
                StopTimer(conTimer);
                orderUpdateTimer = null;
                //heartBeatTimer = null;
                //conTimer = null;
                PrimePOUtil.isConnected = false;
            }
        }

        public string GetVendorName(int vendorID)
        {
            if (!PrimePOUtil.isConnected)
                return null;

            VendorDO ovendorDo = null;
            string vendorName = string.Empty;
            try
            {
                bool fromLock = false;
                try
                {
                    fromLock = Monitor.TryEnter(lockObj, 1000);
                    if (fromLock)
                    {
                        ovendorDo = primePOUtil.vendorInt.FetchVendorData(vendorID);
                        if (ovendorDo != null)
                            vendorName = ovendorDo.VendorName;
                    }
                }
                catch (TimeoutException ex)
                {
                    Monitor.Exit(lockObj);
                }
                finally
                {
                    if (fromLock)
                        Monitor.Exit(lockObj);
                }
            }
            catch (RemotingException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                vendorName = null;
            }
            catch (SocketException ex)
            {

                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                vendorName = null;
            }
            catch (Exception ex)
            {
                //this.conTimer.Start();//Dont know what to do
                vendorName = null;
                ErrorHandler.logException(ex, "", "");
            }
            return vendorName;
        }

        public MMSDictionary<string, string> GetVendors()
        {
            if (!PrimePOUtil.isConnected)
                return null;

            MMSDictionary<string, string> dict = new MMSDictionary<string, string>();
            VendorDO[] ovendorDo = null;

            try
            {
                ovendorDo = primePOUtil.vendorInt.FetchVendorData();

                foreach (VendorDO vendorDo in ovendorDo)
                {
                    if (!dict.ContainsKey(vendorDo.VendorName))
                        dict.Add(vendorDo.VendorName, vendorDo.VendorCode);
                }
            }
            catch (RemotingException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                dict = null;
            }
            catch (SocketException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                dict = null;
            }
            catch (Exception ex)
            {
                //this.conTimer.Start();//Dont know what to do
                ErrorHandler.logException(ex, "", "");
                dict = null;
            }
            return dict;
        }

        public ItemVendorData GetItemVendorData(string ItemCode, string vendorCode)
        {
            if (!PrimePOUtil.isConnected)
                return null;

            ItemDO itemDO = null;
            ItemVendorData itemVendorData = new ItemVendorData();
            ItemVendorRow itemVendRow = itemVendorData.ItemVendor.NewItemVendorRow();

            try
            {
                itemDO = primePOUtil.itemInt.GetVendorItemCode(ItemCode, vendorCode);

                if (itemDO != null)
                {
                    POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
                    VendorData vendorData = new VendorData();
                    vendorData = vendor.PopulateList(" Where " + clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode + " = '" + itemDO.VendorCode + "'");

                    itemVendRow.AverageWholeSalePrice = itemDO.AverageWholesalePrice;
                    itemVendRow.CatalogPrice = itemDO.CatPrice;
                    itemVendRow.ContractPrice = itemDO.ConPrice;
                    itemVendRow.DealerAdjustedPrice = itemDO.DealerAdjustPrice;
                    itemVendRow.FedrelUpperLimitPrice = itemDO.FedUpperlimitPrice;

                    itemVendRow.NetItemPrice = itemDO.NetItemPrice;
                    itemVendRow.ProducersPrice = itemDO.ProducerPrice;

                    //Added by SRT(Abhishek) Date : 24/09/2009
                    itemVendRow.InVoiceBillingPrice = itemDO.InvoiceBillingPrice;
                    itemVendRow.UnitPriceBegQuantity = itemDO.UnitPriceBeginingQuantity;
                    itemVendRow.UnitCostPrice = itemDO.UnitCostPrice;////Added by Atul Joshi on 22-10-2010
                    itemVendRow.BaseCharge = itemDO.BaseChargePrice;
                    itemVendRow.Resale = itemDO.ResalePrice;
                    //End Of Added by SRT(Abhishek) Date : 24/09/2009

                    itemVendRow.VendorCode = itemDO.VendorCode;
                    itemVendRow.VendorID = vendorData.Vendor[0].VendorId;
                    itemVendRow.VendorName = vendorData.Vendor[0].Vendorname;
                    itemVendRow.ItemID = itemDO.ItemCode;
                    itemVendRow.VendorItemID = itemDO.VendorItemCode;
                    String priceQualifier = vendorData.Vendor[0].PriceQualifier;
                    itemVendRow.VenorCostPrice = GetPrice(itemDO, priceQualifier);

                    itemVendRow.ManufacturerSuggPrice = itemDO.ManufacturerSuggPrice;
                    itemVendRow.RetailPrice = itemDO.RetailPrice;

                    itemVendorData.ItemVendor.AddRow(itemVendRow, true);
                }
                return itemVendorData;
            }
            catch (RemotingException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                return null;
            }
            catch (SocketException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                return null;
            }
            catch (Exception ex)
            {
                //this.conTimer.Start();//Dont know what to do              
                if (itemDO.VendorCode == null)
                    ErrorHandler.throwCustomError(POSErrorENUM.Vendor_CodeCanNotBeNULL);
                else if (itemVendRow == null)
                    ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        private Decimal GetPrice(ItemDO itemDoItem, string priceQualifier)
        {
            Decimal price = Convert.ToDecimal(0.00);
            try
            {
                switch (priceQualifier)
                {
                    case clsPOSDBConstants.AWP:
                        price = itemDoItem.AverageWholesalePrice;
                        break;
                    case clsPOSDBConstants.CAT:
                        price = itemDoItem.CatPrice;
                        break;
                    case clsPOSDBConstants.CON:
                        price = itemDoItem.ConPrice;
                        break;
                    case clsPOSDBConstants.DAP:
                        price = itemDoItem.DealerAdjustPrice;
                        break;
                    case clsPOSDBConstants.FUL:
                        price = itemDoItem.FedUpperlimitPrice;
                        break;
                    case clsPOSDBConstants.MSR:
                        price = itemDoItem.ManufacturerSuggPrice;
                        break;
                    case clsPOSDBConstants.NET:
                        price = itemDoItem.NetItemPrice;
                        break;
                    case clsPOSDBConstants.PRO:
                        price = itemDoItem.ProducerPrice;
                        break;
                    case clsPOSDBConstants.RES:
                        price = itemDoItem.RetailPrice;
                        break;
                    //Added to handle RTL for Amiresource vendor Date : 01/09/2009
                    case clsPOSDBConstants.RTL:
                        price = itemDoItem.RetailPrice;
                        break;
                    //End of Added to handle RTL for Amiresource vendor 
                    //Added by SRT(Abhishek) Date : 24/09/2009   
                    case clsPOSDBConstants.BCH:
                        price = itemDoItem.BaseChargePrice;
                        break;
                    case clsPOSDBConstants.RESM:
                        price = itemDoItem.ResalePrice;
                        break;
                    case clsPOSDBConstants.INV:
                        price = itemDoItem.InvoiceBillingPrice;
                        break;
                    case clsPOSDBConstants.PBQ:
                        price = itemDoItem.UnitPriceBeginingQuantity;
                        break;
                    //End Of Added by SRT(Abhishek) Date : 24/09/2009
                    //Added by Atul Joshi on 22-10-2010
                    case clsPOSDBConstants.UCP:
                        price = itemDoItem.UnitCostPrice;
                        break;

                    #region Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added promotional qualifiers
                    case clsPOSDBConstants.RLT:
                    case clsPOSDBConstants.PRP:
                        price = itemDoItem.SalePrice;
                        break;
                    #endregion

                    case "":
                        price = Convert.ToDecimal(0.00);
                        break;
                }
                return price;
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
                return price;
            }
        }

        public int SendPO(POHeaderData poHeaderData, PODetailData poDetailData)
        {
            //Add a call to check 
            //1) Whether the vendor is allowed for electronic PO sending
            //2) Whether all the items in the PO are allowed for electornic purchase
            if (!PrimePOUtil.isConnected)
                return PODISCOONECT;

            int isSend = SUCCESS;
            bool isElectronic = true;
            PurchaseOrder oPurchaseOrder = new PurchaseOrder();
            POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();

            POHeaderRow oPOHeaderRow = poHeaderData.POHeader[0];
            POS_Core.CommonData.VendorData vendorData = vendor.Populate(oPOHeaderRow.VendorCode);

            if (vendorData.Vendor.Rows.Count <= 0)
            {
                return MANUAL;
            }
            //Check whether the Vendor is set for electronic mode
            isElectronic = vendorData.Vendor[0].USEVICForEPO;
            if (!isElectronic)
            {
                return MANUAL;
            }

            PurchaseOrder purchaseOder = new PurchaseOrder();
            PurcahseOrderDO purchseOrdDO = new PurcahseOrderDO();

            try
            {
                purchseOrdDO.OrderNo = poHeaderData.POHeader[0].OrderNo;
                purchseOrdDO.Status = POStatus.Queued.ToString();
                purchseOrdDO.VendorCode = vendorData.Vendor[0].PrimePOVendorCode;
                purchseOrdDO.OrderDate = poHeaderData.POHeader[0].OrderDate;
                purchseOrdDO.ExptDeliveryDate = poHeaderData.POHeader[0].ExptDelvDate;
                purchseOrdDO.Application = "POS";//Added By shitaljit to set Application name in PrimeEDI 

                int count = 0;
                if (poDetailData.PODetail.Rows.Count > 0)
                {
                    purchseOrdDO.PurchaseOrdDetail = new PurchaseOrderDetailsDO[poDetailData.PODetail.Rows.Count];

                    foreach (PODetailRow row in poDetailData.PODetail.Rows)
                    {

                        purchseOrdDO.PurchaseOrdDetail[count] = new PurchaseOrderDetailsDO();
                        purchseOrdDO.PurchaseOrdDetail[count].ItemCode = row.ItemID;
                        purchseOrdDO.PurchaseOrdDetail[count].Qty = row.QTY;
                        purchseOrdDO.PurchaseOrdDetail[count].Idescription = row.ItemDescription;   //PRIMEPO-159 02-May-2018 JY Added to send item description, in case of new item, it helps to update item description in EDI item table

                        if (vendorData.Vendor[0].SendVendCostPrice == true)
                            purchseOrdDO.PurchaseOrdDetail[count].Cost = row.Cost;
                        else
                            purchseOrdDO.PurchaseOrdDetail[count].Cost = 0.00M;

                        purchseOrdDO.PurchaseOrdDetail[count].VendorItemCode = row.VendorItemCode;
                        count++;
                    }
                }
                if (primePOUtil.purchaseOrderInt != null)
                {
                    bool isCreate = false;
                    bool fromLock = false;
                    try
                    {
                        fromLock = Monitor.TryEnter(primePOUtil.purchaseOrderInt, 1000);
                        if (fromLock)
                        {
                            isCreate = primePOUtil.purchaseOrderInt.CreatePO(ref purchseOrdDO);
                        }
                    }
                    catch (TimeoutException ex)
                    {
                        Monitor.Exit(primePOUtil.purchaseOrderInt);
                    }
                    finally
                    {
                        if (fromLock)
                            Monitor.Exit(primePOUtil.purchaseOrderInt);
                    }
                    //Added by SRT(Abhishek) Date :21 Aug 2009
                    //Added condition if PO is not created then it will update the status as pending
                    //if PO is created on PO side then it will update status as order is queued.
                    if (isCreate)
                    {
                        poHeaderData.POHeader[0].PrimePOrderId = purchseOrdDO.OrderId;
                        //Added By Ravindra (Quicsolv) 16 Jan 2013
                        poHeaderData.POHeader[0].RefOrderNO = purchseOrdDO.RefOrderID;


                        //End of Added By Ravindra (Quicsolv) 16 Jan 2013
                        poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Queued;

                        isSend = SUCCESS;
                    }
                    else
                    {
                        poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Pending;
                        isSend = ERROR;
                    }
                    //End of Added by SRT(Abhishek) Date :21 Aug 2009
                    oPurchaseOrder.Persist(poHeaderData, poDetailData);

                    ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + " , Status - " + Enum.GetName(typeof(PurchseOrdStatus), poHeaderData.POHeader[0].Status));
                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                }
            }
            catch (RemotingException ex)
            {
                isSend = ERROR;
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Pending;
                oPurchaseOrder.Persist(poHeaderData, poDetailData);
                ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + " , Status - " + PurchseOrdStatus.Pending);
                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                HandlePrimePOConnectionFailure();
            }
            catch (SocketException ex)
            {
                isSend = ERROR;
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Pending; //0;
                oPurchaseOrder.Persist(poHeaderData, poDetailData);
                ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + " , Status - " + PurchseOrdStatus.Pending);
                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                HandlePrimePOConnectionFailure();
            }
            catch (Exception ex)
            {
                //Log error atleast.
                ErrorHandler.logException(ex, "", "");
                isSend = ERROR;
                poHeaderData.POHeader[0].Status = (int)PurchseOrdStatus.Pending;//0;
                oPurchaseOrder.Persist(poHeaderData, poDetailData);
                ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order -" + poHeaderData.POHeader[0].OrderNo + " , Status - " + PurchseOrdStatus.Pending);
                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
            }
            return isSend;
        }


        public bool RetryPO(Dictionary<long, string> setStatus)
        {
            if (!PrimePOUtil.isConnected)
                return false;
            try
            {
                bool isQueued = primePOUtil.purchaseOrderInt.RetryPO(setStatus);

                if (isQueued == true)
                {
                    ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Max Retried Order is Placed");
                }
                else
                {
                    ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Failed To Placed Order ");
                }
                return isQueued;
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
                ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Failed To Placed Order ");
                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();

                return false;
            }
        }

        public bool ReSubmitPO(Dictionary<long, string> setStatus)
        {
            if (!PrimePOUtil.isConnected)
                return false;
            try
            {
                bool isQueued = primePOUtil.purchaseOrderInt.RetryPO(setStatus);

                if (isQueued == true)
                {
                    ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Purchase Order is Resubmitted");
                }
                else
                {
                    ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Failed To Placed Order ");
                }
                return isQueued;
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
                ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Failed To Placed Order ");
                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();

                return false;
            }
        }
        /* public bool AddDirectPO()
         {
             PurcahseOrderDO poDO = new PurcahseOrderDO();
             PurcahseOrderDO = primePOUtil.purchaseOrderInt.GetPOStatus(ref poStatus); 
         }*/


        public bool UpdatePOStatus(ref Dictionary<long, string> poStatus, ref POHeaderData poHeader, ref PODetailData poDetails)
        {
            //while adding items for creating a new PO, new item and selects an item already in the po item list then dont create a new row and give the focus on the item already there in the grid.
            //this will ensure no 2 items are same in a single PO.
            List<string> ParseIssuePOList = new List<string>();
            if (!PrimePOUtil.isConnected)
                return false;
            bool retVal = true;
            PurcahseOrderDO poDO = new PurcahseOrderDO();
            Dictionary<long, PurcahseOrderDO> poData = new Dictionary<long, PurcahseOrderDO>();
            string posItemId = string.Empty;
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            if (POS_Core.Resources.Configuration.UpdateInProgress == false)
            {
                POS_Core.Resources.Configuration.UpdateInProgress = true;
                try
                {
                    Application.DoEvents();
                    bool fromLock = false;
                    try
                    {
                        fromLock = Monitor.TryEnter(lockObj, 1000);
                        if (fromLock)
                        {
                            //get the status for the order id from the dictionary 
                            POS_Core.ErrorLogging.Logs.Logger("**Get PurchaseOrder Status from PrimeEDI");
                            poData = primePOUtil.purchaseOrderInt.GetPOStatus(ref poStatus);
                            {
                                #region HeaderRow
                                foreach (POHeaderRow row in poHeader.POHeader.Rows)
                                {
                                    if (!poData.ContainsKey(row.PrimePOrderId)) //Added by Ritesh on 12-Jul-09 for fixing unnecessary order issue
                                    {
                                        ParseIssuePOList.Add(row.OrderNo.ToString());
                                        continue;
                                    }
                                    POHeaderData poHeaderData = new POHeaderData();
                                    PODetailData poDetailData = new PODetailData();

                                    int previousStatus = 0;
                                    int currentstatus = 0;

                                    #region Qualified Records
                                    foreach (long poID in poData.Keys)
                                    {
                                        try
                                        {
                                            if (row.PrimePOrderId == poID)
                                            {
                                                //ErrorLogging.Logs.Logger("**Get PurchaseOrder Status from PrimeEDI");
                                                previousStatus = row.Status;
                                                // update status for the order id  from dictionary received as ref parameter
                                                //row[clsPOSDBConstants.POHeader_Fld_Status] = GetStatus(poStatus[poID]);
                                                //GetStatus(poStatus[poID]);
                                                currentstatus = GetStatus(poStatus[poID]);
                                                if (previousStatus != currentstatus)
                                                {//poData[1].
                                                    // update podata from the object received  for order id
                                                    POS_Core.ErrorLogging.Logs.Logger("Purchase order Update for Prime EDI OrderID" + row.PrimePOrderId.ToString() + " Previous Status:" + previousStatus + "Current Status" + currentstatus);
                                                    row[clsPOSDBConstants.POHeader_Fld_AckType] = poData[poID].AckType;
                                                    row[clsPOSDBConstants.POHeader_Fld_AckDate] = poData[poID].AckDate;
                                                    row[clsPOSDBConstants.POHeader_Fld_AckStatus] = poData[poID].AckStatus;
                                                    row[clsPOSDBConstants.POHeader_Fld_InvoiceDate] = poData[poID].InvoiceDate;
                                                    row[clsPOSDBConstants.POHeader_Fld_InvoiceNumber] = poData[poID].InvoiceNumber;//refOrderID
                                                    row[clsPOSDBConstants.POHeader_Fld_RefOrderNo] = poData[poID].RefOrderID;//Added By Ravindra(QuicSolv) 17 Jan 2013
                                                    row[clsPOSDBConstants.POHeader_Fld_AckFileType] = poData[poID].AckFileType;//Added By shitaljit on 17May13 to store file type JIRA- 911
                                                    row[clsPOSDBConstants.POHeader_Fld_TransTypeCode] = poData[poID].TransTypeCode; //PRIMEPOS-2901 05-Nov-2020 JY Added
                                                    // string t = row[clsPOSDBConstants.POHeader_Fld_RefOrderNo].ToString();
                                                    poDO = poData[poID];

                                                    int count = 0;
                                                    //string POitemDetail = "";

                                                    #region 29-Feb-2016 JY Added logic to improve performance and resolve status sync issue
                                                    List<PurchaseOrderDetailsDO> lstEDIPO = new List<PurchaseOrderDetailsDO>();
                                                    for (count = 0; count < poDO.PurchaseOrdDetail.Length; count++)
                                                    {
                                                        lstEDIPO.Add(poDO.PurchaseOrdDetail[count]);
                                                    }
                                                    #endregion

                                                    foreach (PODetailRow poDetailrow in poDetails.PODetail.Rows)
                                                    {
                                                        frmSearchPOAck.GetCurrentInstance().UpdateUI();

                                                        //check  whether podetail order id and the po order id  matches
                                                        if (row.OrderID == poDetailrow.OrderID)
                                                        {
                                                            //if  order id matches with one in the row from po table
                                                            #region 29-Feb-2016 JY Added logic to improve performance and resolve status sync issue
                                                            for (count = 0; count < lstEDIPO.Count; count++)
                                                            {
                                                                //if length of Item Code is 12 then here we will match for both item code of length 11 and 12
                                                                if (lstEDIPO[count].ItemCode.Length == 12)
                                                                    posItemId = lstEDIPO[count].ItemCode.Substring(0, lstEDIPO[count].ItemCode.Length - 1);
                                                                // as no of rows will be greater we will check itemId coulomn  
                                                                // as in PO there will be only one item at one time.
                                                                if (lstEDIPO[count].ItemCode == poDetailrow.ItemID || posItemId == poDetailrow.ItemID)
                                                                {
                                                                    posItemId = string.Empty;   //Sprint-22 23-Dec-2015 JY Added to reset it       
                                                                    poDetailrow[clsPOSDBConstants.PODetail_Fld_AckQTY] = lstEDIPO[count].AckQty;
                                                                    poDetailrow[clsPOSDBConstants.PODetail_Fld_AckStatus] = lstEDIPO[count].AckStatus;
                                                                    poDetailrow[clsPOSDBConstants.PODetail_Fld_ItemDescType] = lstEDIPO[count].ItemDescType;
                                                                    poDetailrow[clsPOSDBConstants.PODetail_Fld_Idescription] = lstEDIPO[count].Idescription;

                                                                    //To update cost price if seetings in POS, Vendor, Item settings.
                                                                    Item objItemSvr = new Item();
                                                                    ItemData ItemDataSet = new ItemData();
                                                                    ItemDataSet = objItemSvr.Populate(poDetailrow.ItemID);
                                                                    POS_Core.BusinessRules.Vendor objVendorSvr = new POS_Core.BusinessRules.Vendor();
                                                                    VendorData VendorDataSet = new VendorData();
                                                                    VendorDataSet = objVendorSvr.Populate(poDO.VendorCode);
                                                                    if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateVendorPrice == true) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                                    {
                                                                        if ((bool)(VendorDataSet.Vendor[0][clsPOSDBConstants.Vendor_Fld_UpdatePrice]) == true)
                                                                        {
                                                                            if ((bool)(ItemDataSet.Item[0][clsPOSDBConstants.Item_Fld_UpdatePrice]) == true)
                                                                            {
                                                                                string strcost = lstEDIPO[count].Cost.ToString();
                                                                                poDetailrow[clsPOSDBConstants.PODetail_Fld_Cost] = lstEDIPO[count].Cost;

                                                                            }
                                                                        }
                                                                    }

                                                                    if (currentstatus == (int)PurchseOrdStatus.Acknowledge || currentstatus == (int)PurchseOrdStatus.PartiallyAck || currentstatus == (int)PurchseOrdStatus.DeliveryReceived /*added by atul 25-oct-2010*/)
                                                                    {
                                                                        if (Convert.ToDecimal(poDetailrow[clsPOSDBConstants.PODetail_Fld_Cost]) != lstEDIPO[count].Cost)
                                                                        {
                                                                            try
                                                                            {
                                                                                UpdateItemPrice(lstEDIPO[count].ItemCode, lstEDIPO[count].VendorItemCode, lstEDIPO[count].Cost, row.VendorID);
                                                                            }
                                                                            catch (Exception ex)
                                                                            {; }
                                                                        }
                                                                    }

                                                                    poDetailrow[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier] = lstEDIPO[count].ChangedProductQualifier;
                                                                    poDetailrow[clsPOSDBConstants.PODetail_Fld_ChangedProductID] = lstEDIPO[count].ChangedProductID;
                                                                    lstEDIPO.RemoveAt(count);
                                                                    break;
                                                                }
                                                            }
                                                            #endregion
                                                            #region 29-Feb-2016 JY Commented to improve performance and resolve status sync issue
                                                            //for (count = 0; count < poDO.PurchaseOrdDetail.Length; count++)
                                                            //{
                                                            //    //if length of Item Code is 12 then here we will match for both item code of length 11 and 12
                                                            //    if (poDO.PurchaseOrdDetail[count].ItemCode.Length == 12)
                                                            //        posItemId = poDO.PurchaseOrdDetail[count].ItemCode.Substring(0, poDO.PurchaseOrdDetail[count].ItemCode.Length - 1);
                                                            //    // as no of rows will be greater we will check itemId coulomn  
                                                            //    // as in PO there will be only one item at one time.
                                                            //    if (poDO.PurchaseOrdDetail[count].ItemCode == poDetailrow.ItemID || posItemId == poDetailrow.ItemID)
                                                            //    {
                                                            //        posItemId = string.Empty;   //Sprint-22 23-Dec-2015 JY Added to reset it       
                                                            //        poDetailrow[clsPOSDBConstants.PODetail_Fld_AckQTY] = poDO.PurchaseOrdDetail[count].AckQty;
                                                            //        poDetailrow[clsPOSDBConstants.PODetail_Fld_AckStatus] = poDO.PurchaseOrdDetail[count].AckStatus;
                                                            //        poDetailrow[clsPOSDBConstants.PODetail_Fld_ItemDescType] = poDO.PurchaseOrdDetail[count].ItemDescType;
                                                            //        poDetailrow[clsPOSDBConstants.PODetail_Fld_Idescription] = poDO.PurchaseOrdDetail[count].Idescription;
                                                            //        //Added By Ravindra (QuicSolv) 17 Jan 2013
                                                            //        //poDetailrow[clsPOSDBConstants.PODetail_Fld_Idescription] = poDO.PurchaseOrdDetail[count].;
                                                            //        //End of Added BY Ravindra(QuicSolv) 17 Jan 2013

                                                            //        //poDetailrow[clsPOSDBConstants.PODetail_Fld_Cost] = poDO.PurchaseOrdDetail[count].Cost;
                                                            //        //Added By Shitaljit(QuicSolv) on 20 Jan 2012
                                                            //        //To update cost price if seetings in POS, Vendor, Item settings.
                                                            //        Item objItemSvr = new Item();
                                                            //        ItemData ItemDataSet = new ItemData();
                                                            //        ItemDataSet = objItemSvr.Populate(poDetailrow.ItemID);
                                                            //        POS.BusinessRules.Vendor objVendorSvr = new POS.BusinessRules.Vendor();
                                                            //        VendorData VendorDataSet = new VendorData();
                                                            //        VendorDataSet = objVendorSvr.Populate(poDO.VendorCode);
                                                            //        if (POS_Core.Resources.Configuration.CPOSSet.UpdateVendorPrice == true)
                                                            //        {
                                                            //            if ((bool)(VendorDataSet.Vendor[0][clsPOSDBConstants.Vendor_Fld_UpdatePrice]) == true)
                                                            //            {
                                                            //                if ((bool)(ItemDataSet.Item[0][clsPOSDBConstants.Item_Fld_UpdatePrice]) == true)
                                                            //                {
                                                            //                    string strcost = poDO.PurchaseOrdDetail[count].Cost.ToString();
                                                            //                    poDetailrow[clsPOSDBConstants.PODetail_Fld_Cost] = poDO.PurchaseOrdDetail[count].Cost;

                                                            //                }
                                                            //            }
                                                            //        }
                                                            //        //Change by SRT (Sachin) Date : 24 Nov 2009

                                                            //        //commented by atul 25-oct-2010
                                                            //        //// Added by atul 19-sep-2010
                                                            //        //VendorData objVendData = new VendorData();
                                                            //        //VendorSvr objVendorSvr = new VendorSvr();
                                                            //        //objVendData = objVendorSvr.Populate(poDO.VendorCode);
                                                            //        ////bool flag810 =Convert.ToBoolean(objVendData.Tables[0].Rows[0][clsPOSDBConstants.Vendor_Fld_Process810]);
                                                            //        //bool flag810 = objVendData.Vendor[0].Process810;

                                                            //        //if (flag810 == true)
                                                            //        //{
                                                            //        //    if (currentstatus == (int)PurchseOrdStatus.DeliveryReceived)
                                                            //        //    {
                                                            //        //        if (Convert.ToDecimal(poDetailrow[clsPOSDBConstants.PODetail_Fld_Cost]) != poDO.PurchaseOrdDetail[count].Cost)
                                                            //        //        {
                                                            //        //            try
                                                            //        //            {
                                                            //        //                UpdateItemPrice(poDO.PurchaseOrdDetail[count].ItemCode, poDO.PurchaseOrdDetail[count].VendorItemCode, poDO.PurchaseOrdDetail[count].Cost, row.VendorID);
                                                            //        //            }
                                                            //        //            catch (Exception ex)
                                                            //        //            { ;}
                                                            //        //        }
                                                            //        //    }
                                                            //        //}//end of Added by atul 19-sep-2010
                                                            //        //end of commented by atul 25-oct-2010

                                                            //        if (currentstatus == (int)PurchseOrdStatus.Acknowledge || currentstatus == (int)PurchseOrdStatus.PartiallyAck || currentstatus == (int)PurchseOrdStatus.DeliveryReceived /*added by atul 25-oct-2010*/)
                                                            //        {
                                                            //            if (Convert.ToDecimal(poDetailrow[clsPOSDBConstants.PODetail_Fld_Cost]) != poDO.PurchaseOrdDetail[count].Cost)
                                                            //            {
                                                            //                try
                                                            //                {
                                                            //                    UpdateItemPrice(poDO.PurchaseOrdDetail[count].ItemCode, poDO.PurchaseOrdDetail[count].VendorItemCode, poDO.PurchaseOrdDetail[count].Cost, row.VendorID);
                                                            //                }
                                                            //                catch (Exception ex)
                                                            //                { ;}
                                                            //            }
                                                            //        }

                                                            //        poDetailrow[clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier] = poDO.PurchaseOrdDetail[count].ChangedProductQualifier;
                                                            //        poDetailrow[clsPOSDBConstants.PODetail_Fld_ChangedProductID] = poDO.PurchaseOrdDetail[count].ChangedProductID;
                                                            //        count++;
                                                            //        try
                                                            //        {
                                                            //            POitemDetail = POitemDetail + "(Item Code:" + poDO.PurchaseOrdDetail[count].ItemCode + " Order Qty:" + poDO.PurchaseOrdDetail[count].Qty + " Ack Qty:" + poDO.PurchaseOrdDetail[count].AckQty + "),";
                                                            //        }
                                                            //        catch { }
                                                            //        break;
                                                            //    }
                                                            //}
                                                            #endregion
                                                            purchaseOrder = new PurchaseOrder();
                                                            poDetailData = new PODetailData();
                                                            DataRowState rowtstate = row.RowState;
                                                            poHeaderData.POHeader.LoadDataRow(row.ItemArray, true);
                                                            poHeaderData.POHeader[0].SetModified();
                                                            DataRow[] poDetailRow = poDetails.PODetail.Select(clsPOSDBConstants.POHeader_Fld_OrderID + "=" + row.OrderID + " AND " + clsPOSDBConstants.Item_Fld_ItemID + "='" + poDetailrow.ItemID + "'");
                                                            int poDetcount = 0;
                                                            foreach (DataRow poDetrow in poDetailRow)
                                                            {
                                                                poDetailData.PODetail.LoadDataRow(poDetrow.ItemArray, true);
                                                                poDetailData.PODetail[poDetcount].SetModified();
                                                                poDetcount++;
                                                            }
                                                            if (poHeaderData.POHeader.Rows.Count > 0 && poDetailData.PODetail.Rows.Count > 0)
                                                            {
                                                                purchaseOrder.Persist(poHeaderData, poDetailData);
                                                            }
                                                        }
                                                    }
                                                    if (previousStatus != currentstatus)
                                                    {
                                                        if (poHeaderData.POHeader.Count > 0)
                                                        {
                                                            poHeaderData.POHeader[0][clsPOSDBConstants.POHeader_Fld_Status] = GetStatus(poStatus[poID]);
                                                            purchaseOrder.Persist(poHeaderData, poDetailData);
                                                            row.Status = poHeaderData.POHeader[0].Status; // added by atul 26-oct-2010
                                                            ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Status For Purchase Order - " + row.OrderNo.ToString() + " is Updated");
                                                            ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Purchase Order - " + row.OrderNo.ToString() + ", Status -" + ((PurchseOrdStatus)row.Status).ToString());
                                                            try
                                                            {
                                                                VendorData objVendData = new VendorData();
                                                                VendorSvr objVendorSvr = new VendorSvr();
                                                                objVendData = objVendorSvr.Populate(poDO.VendorCode);
                                                                bool flag810 = objVendData.Vendor[0].Process810;

                                                                if (flag810 == true)
                                                                {
                                                                    if (currentstatus == (int)PurchseOrdStatus.DeliveryReceived)
                                                                    {
                                                                        poData[poID].Synchronize = true;
                                                                        primePOUtil.purchaseOrderInt.UpdateSynchronize(poData[poID]);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (currentstatus == (int)PurchseOrdStatus.PartiallyAck || currentstatus == (int)PurchseOrdStatus.Acknowledge)
                                                                    {
                                                                        poData[poID].Synchronize = true;
                                                                        primePOUtil.purchaseOrderInt.UpdateSynchronize(poData[poID]);
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception)
                                                            {

                                                                //throw;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("Status For Purchase Order - " + row.OrderNo.ToString() + " is Not Updated");
                                                    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    ParseIssuePOList.Add(row.OrderNo.ToString());
                                                    //}
                                                    POS_Core.ErrorLogging.Logs.Logger("**End of purchase order Update for PrimeEDIOrderID " + row.PrimePOrderId.ToString());
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            POS_Core.ErrorLogging.Logs.Logger(ex.Message);
                                            continue;
                                        }
                                    }
                                    #endregion
                                }

                                #endregion
                            }
                            retVal = true;
                            //Configuration.UpdateInProgress = false;
                        }
                    }
                    catch (TimeoutException ex)
                    {
                        POS_Core.ErrorLogging.Logs.Logger(ex.Message);
                        Monitor.Exit(lockObj);
                        POS_Core.Resources.Configuration.UpdateInProgress = false;
                    }
                    finally
                    {
                        if (fromLock)
                            Monitor.Exit(lockObj);
                    }
                    if (ParseIssuePOList.Count > 0)
                    {
                        ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet("PrimePOS can not update status for Order No ( " + string.Join(",", ParseIssuePOList.ToArray()) + " ).Please contact Administrator.");
                        //clsUIHelper.ShowErrorMsg("PrimePOS can not update status for following EPOs.\n" + string.Join(",", ParseIssuePOList.ToArray())+" Please contact Administrator.");
                    }
                    ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                }
                catch (RemotingException ex)
                {
                    POS_Core.ErrorLogging.Logs.Logger(ex.Message);
                    ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                    HandlePrimePOConnectionFailure();
                    retVal = false;
                    POS_Core.Resources.Configuration.UpdateInProgress = false;
                }
                catch (SocketException ex)
                {
                    POS_Core.ErrorLogging.Logs.Logger(ex.Message);
                    ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                    HandlePrimePOConnectionFailure();
                    retVal = false;
                    POS_Core.Resources.Configuration.UpdateInProgress = false;
                }
                catch (Exception ex)
                {
                    POS_Core.ErrorLogging.Logs.Logger(ex.Message);
                    //Log error atleast.
                    ErrorHandler.logException(ex, "", "");
                    retVal = false;
                    POS_Core.Resources.Configuration.UpdateInProgress = false;
                }
                POS_Core.Resources.Configuration.UpdateInProgress = false;
                return retVal;
            }
            else
            {
                return false;
            }
        }

        private int GetStatus(string status)
        {
            int poStatus = 0;
            //it will set the matching status in POS for status in PrimePO
            try
            {
                switch (status)
                {
                    case "Queued":
                        poStatus = (int)PurchseOrdStatus.Queued;  //  2;
                        break;
                    case "PartiallyAcknowledged":
                        poStatus = (int)PurchseOrdStatus.PartiallyAck; //4;
                        break;
                    case "Acknowledged":
                        poStatus = (int)PurchseOrdStatus.Acknowledge; //5;
                        break;
                    case "Confirmed":
                        poStatus = (int)PurchseOrdStatus.Processed; // 1;
                        break;
                    case "MaxAttemptsOver":
                        poStatus = (int)PurchseOrdStatus.MaxAttempt; //Processed;   //5;
                        break;
                    case "Submitted":
                        poStatus = (int)PurchseOrdStatus.Submitted; //Processed;   //5;
                        break;
                    case "Expired":
                        poStatus = (int)PurchseOrdStatus.Expired; //Processed;   //5;
                        break;
                    case "Error":
                        poStatus = (int)PurchseOrdStatus.Error; //Error;   //5;
                        break;
                    case "Overdue":
                        poStatus = (int)PurchseOrdStatus.Overdue; //Error;   //5;
                        break;
                    case "":
                        poStatus = (int)PurchseOrdStatus.Queued;
                        break;
                    //Add By SRT(Sachin) Date : 18 Feb 2010
                    case "DirectAcknowledged":
                        poStatus = (int)PurchseOrdStatus.DirectAcknowledge;
                        break;
                    //End of Add By SRT(Sachin) Date : 18 Feb 2010
                    //Added to handle  error status
                    case "DeliveryReceived": // added by atul 19-oct-2010
                        poStatus = (int)PurchseOrdStatus.DeliveryReceived;
                        break;
                    case "ERRORPOID":
                        poStatus = -1;
                        break;
                    case "DirectDelivery": //Added by shitaljit for 810 file
                        poStatus = (int)PurchseOrdStatus.DirectDelivery;
                        break;

                }
                return poStatus;
            }
            catch (Exception ex)
            {
                //ErrorHandler.throwException(ex,"","");
                ErrorHandler.logException(ex, "", "");
                return poStatus;
            }
        }

        public long GetMaxFileID()
        {
            if (!PrimePOUtil.isConnected)
                return 0L;
            long maxFileID = 0;

            //It will return the max file Id from  the   Prime PO side       

            try
            {
                bool fromLock = false;
                try
                {
                    fromLock = Monitor.TryEnter(lockObj, 1000);
                    if (fromLock)
                    {
                        maxFileID = primePOUtil.itemInt.GetMaxFileId();
                    }
                }
                catch (TimeoutException ex)
                {
                    Monitor.Exit(lockObj);
                }
                finally
                {
                    if (fromLock)
                        Monitor.Exit(lockObj);
                }
            }
            catch (RemotingException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                maxFileID = 0;
            }
            catch (SocketException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                maxFileID = 0;
            }
            catch (Exception ex)
            {
                //Log error atleast.
                ErrorHandler.logException(ex, "", "");
                maxFileID = 0;
            }
            return maxFileID;
        }

        #region Comments
        //will receive FileID as input 
        //call the method to get ITEMDO object array as output 
        //private ItemDO[] GetPriceDetailsForFile(long fileID)

        // 1) Is item exist  in item table if yes then  match the last vendor 
        // if last vendor matches with  vendor price file  then update the price in the item table with the
        // price in the file ,which is specified for that vendor in the vendor table .

        // 2) if item exist for that vendor in the itemvendor table then update the prices in table 
        //  else insert the values in the table.        

        // % ensure whether all item are updated or inserted 
        #endregion
        public int UpdatePrice(long POSfileID)
        {
            if (!PrimePOUtil.isConnected)
                return 0;

            //int percentCompleted = 0;
            long fileID = POSfileID;
            int numberOfPOItemRecords = 0;
            int numberOfRecordsUpdated = 0;
            int numberOfTrips = 0;
            int numberOfRemaining = 0;
            int StartIndex = 0;
            int EndIndex = 0;

            //Added by SRT(Abhishek) Date : 14/12/2009
            TotalItemInserted = 0;
            TotalItemUpdated = 0;
            //End Of  Added by SRT(Abhishek) Date : 14/12/2009
            error = 0;
            ItemDO[] itemDo = null;
            // will receive filled item DO object from server side.
            //Added By sRT(Gaurav) Date: 07-Jul-2009
            //This is a temp variable that holds price recieved temp.

            //End Of Added By SRT(Gaurav)
            FileInfo fileInfo = null;
            StreamWriter write = null;
            string vendorName = string.Empty;
            string sTotalFileID = string.Empty;
            string result = string.Empty;

            try
            {

                POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
                VendorData vendData = new VendorData();
                vendData = vendor.PopulateList(" Where " + clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode + "  != 'NULL'");

                if (vendData.Vendor.Rows.Count > 0)
                {
                    itemDo = null;

                    for (int Index = 0; Index < vendData.Vendor.Rows.Count; Index++)
                    {

                        fileID = POSfileID;         // Add by Ravindra to send oldPrimePOS fileID to PrimeEDI
                        string fileName = string.Empty;
                        numberOfPOItemRecords = 0;
                        numberOfRecordsUpdated = 0;
                        numberOfTrips = 0;
                        numberOfRemaining = 0;
                        //percentCompleted = 0;
                        StartIndex = 0;
                        EndIndex = 200;
                        Decimal percentCompleted = 0.00M;

                        fileInfo = new FileInfo(Application.StartupPath + "\\" + vendData.Vendor[Index].Vendorcode + LASTUPDATE);
                        numberOfPOItemRecords = primePOUtil.itemInt.CreateItemDataSet(ref fileID, ref fileName, vendData.Vendor[Index].PrimePOVendorCode);

                        POS_Core.ErrorLogging.Logs.Logger("Price update starred for Vendor : " + vendData.Vendor[Index].PrimePOVendorCode + Environment.NewLine + "FileID "
                            + fileID.ToString());
                        if (numberOfPOItemRecords > 0)
                        {
                            if (fileInfo.Exists)
                                fileInfo.Delete();

                            write = new StreamWriter(Application.StartupPath + "\\" + vendData.Vendor[Index].Vendorcode + LASTUPDATE);
                            vendorName = vendData.Vendor[Index].Vendorname.ToString();
                            numberOfTrips = numberOfPOItemRecords / 200;
                            numberOfRemaining = numberOfPOItemRecords % 200;
                            POS_Core.Resources.Configuration.UpdatedBy = "E";
                            for (int tripCount = 0; tripCount < numberOfTrips + 1; tripCount++)
                            {
                                POS_Core.ErrorLogging.Logs.Logger("tripCount : " + tripCount.ToString());
                                itemDo = primePOUtil.itemInt.GetLatestItemDataForRange(StartIndex, EndIndex);
                                numberOfRecordsUpdated += UpdatePosItemDatabase(itemDo, write, vendData.Vendor[Index].Vendorcode);
                                StartIndex = EndIndex + 1;
                                EndIndex = EndIndex + 200;

                                percentCompleted = Decimal.Divide(numberOfRecordsUpdated, numberOfPOItemRecords) * 100;

                                Application.DoEvents();

                                ClsUpdatePOStatus.GetMessage = " Update Price " + Convert.ToInt32(percentCompleted) + " %  for Vendor =" + vendData.Vendor[Index].Vendorname;
                                ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                            }
                            write.Close();
                            fileInfo = null;
                            ClsUpdatePOStatus.GetMessage = "Last File ID Receive " + fileID.ToString();
                            ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Price File Received. FileID =" + fileName.ToString() + " , Item Inserted =" + TotalItemInserted.ToString() + " , ItemUpdated =" + TotalItemUpdated.ToString() + " ,Error = " + error.ToString());//fileHistoryRowUpdate.FileID.ToString()
                            string sNPI = POS_Core.Resources.Configuration.GetNPI();
                            try
                            {
                                if (objService.Url != string.Empty || objService.Url != null)
                                    result = objService.AddPriceUpateHistroy(sNPI, DateTime.Now, TotalItemInserted, TotalItemUpdated, vendorName, fileName.ToString(), "PRIMEPOS");
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                        }

                        FileHistoryData fileHistoryDataUpdate = new FileHistoryData();
                        FileHistoryRow fileHistoryRowUpdate = fileHistoryDataUpdate.FileHistoryTable.NewFileHistoryRow();
                        FileHistory fileHistoryUpdate = new POS_Core.BusinessRules.FileHistory();

                        //set Coulomns from File history table.
                        fileHistoryRowUpdate.FileID = fileID; //GetMaxFileID();
                        fileHistoryRowUpdate.LastUpdateDate = DateTime.Now;


                        //Update File History Table 
                        //fileHistoryUpdate.Persist(fileHistoryDataUpdate);
                        // Set the message 

                        //ClsUpdatePOStatus.GetMessage = "Last File ID Receive " + fileID.ToString();
                        //ClsUpdatePOStatus.UpdateStatusInst.FillLogDataSet(" Price File Received. FileID =" + fileName.ToString() + " , Item Inserted =" + TotalItemInserted.ToString() + " , ItemUpdated =" + TotalItemUpdated.ToString() + " ,Error = " + error.ToString());//fileHistoryRowUpdate.FileID.ToString()
                        //string sNPI=Configuration.GetNPI();
                        //result = objService.AddPriceUpateHistroy(sNPI, DateTime.Now, TotalItemInserted, TotalItemUpdated, vendorName, fileName.ToString(), "PRIMEPOS");
                        //Update File History Table
                        if (numberOfPOItemRecords > 0)// added by atul 17-jan-2011
                        {
                            if (result == "success")
                            {
                                fileHistoryRowUpdate.SynchronizedCentrally = true;
                                fileHistoryDataUpdate.FileHistoryTable.AddRow(fileHistoryRowUpdate, true);
                                fileHistoryUpdate.Persist(fileHistoryDataUpdate);
                            }
                            else
                            {
                                fileHistoryRowUpdate.SynchronizedCentrally = false;
                                fileHistoryDataUpdate.FileHistoryTable.AddRow(fileHistoryRowUpdate, true);
                                fileHistoryUpdate.Persist(fileHistoryDataUpdate);
                            }
                        }
                        ClsUpdatePOStatus.UpdateStatusInst.UpdatePOCount();
                        primePOUtil.itemInt.PurgeItemDataSet();
                        POS_Core.ErrorLogging.Logs.Logger("Done with price update for Vendor : " + vendData.Vendor[Index].PrimePOVendorCode + Environment.NewLine + "FileID "
                            + fileID.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                POS_Core.ErrorLogging.Logs.Logger(ex.Message + Environment.NewLine + "Prime EDI FileID "
                              + fileID.ToString());
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            finally
            {
                if (write != null)
                {
                    write.Close();
                    write = null;
                }
                if (fileInfo != null)
                {
                    fileInfo = null;
                }
            }
            return 0;
        }

        private int UpdatePosItemDatabase(ItemDO[] poItemData, StreamWriter write, String varVendorCode)
        {
            decimal PriceRcvd = 0;
            itemStringToinsert = string.Empty;
            if (poItemData == null)
                return 0;

            //ItemData itemDataUpdated = new ItemData();    //Sprint-22 04-Nov-2015 JY Commented unwanted code
            //ItemVendorData itemVenodeDataUpdated = new ItemVendorData();  //Sprint-22 04-Nov-2015 JY Commented unwanted code

            using (ItemVendor itemVendor = new ItemVendor())
            {
                using (Item oItem = new Item())
                {
                    int subPercentageCompleted = 0;
                    int count = 0;
                    try
                    {
                        if (poItemData != null)
                        {
                            //Item oitem = new Item();
                            foreach (ItemDO oItemDoItem in poItemData)
                            {
                                using (ItemData oItemData = oItem.Populate(oItemDoItem.ItemCode, clsPOSDBConstants.UpdatePrice, true))
                                {
                                    //Item oitem = new Item();
                                    //POS.BusinessRules.Vendor vendor = new POS.BusinessRules.Vendor(); //Sprint-24 11-Jan-2016 JY Commented unwanted code
                                    ItemVendorData oItemVendorData = new ItemVendorData();
                                    //itemData = oitem.Populate(itemDoItem.ItemCode, clsPOSDBConstants.UpdatePrice, true);

                                    string[] itemvalues = new string[7];
                                    string priceQualifier = string.Empty;
                                    string costQualifier = string.Empty;
                                    string sellingPrice = string.Empty;
                                    string averagePrice = string.Empty;
                                    string vendorCostPrice = string.Empty;
                                    string orignalSellingPrice = string.Empty;
                                    string orignalAvgPrice = string.Empty;
                                    string orignalLastCostPrice = string.Empty;

                                    if (((oItemDoItem.PckUnit.ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (oItemDoItem.PckUnit.ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA)) && MMSUtil.UtilFunc.ValorZeroI(oItemDoItem.PckSize) > 0)    //Sprint-21 22-Feb-2016 JY Added CA for case item
                                    {
                                        decimal PriceRecived = 0;
                                        if (oItemDoItem.AverageWholesalePrice > 0)
                                        {
                                            PriceRcvd = Decimal.Round(Decimal.Divide(oItemDoItem.AverageWholesalePrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.AverageWholesalePrice = PriceRcvd;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.CatPrice > 0)
                                        {
                                            PriceRcvd = Decimal.Round(Decimal.Divide(oItemDoItem.CatPrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.CatPrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.ConPrice > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.ConPrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.ConPrice = PriceRcvd;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.DealerAdjustPrice > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.DealerAdjustPrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.DealerAdjustPrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.FedUpperlimitPrice > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.FedUpperlimitPrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.FedUpperlimitPrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.ManufacturerSuggPrice > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.ManufacturerSuggPrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.ManufacturerSuggPrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.NetItemPrice > 0)
                                        {
                                            string itemid = oItemDoItem.ItemCode;
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.NetItemPrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.NetItemPrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.ProducerPrice > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.ProducerPrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.ProducerPrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.ResalePrice > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.ResalePrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.ResalePrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.BaseChargePrice > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.BaseChargePrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.BaseChargePrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.InvoiceBillingPrice > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.InvoiceBillingPrice, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.InvoiceBillingPrice = PriceRecived;
                                            PriceRecived = 0;
                                        }
                                        if (oItemDoItem.UnitPriceBeginingQuantity > 0)
                                        {
                                            PriceRecived = Decimal.Round(Decimal.Divide(oItemDoItem.UnitPriceBeginingQuantity, MMSUtil.UtilFunc.ValorZeroDEC(oItemDoItem.PckSize)), 4);
                                            oItemDoItem.UnitPriceBeginingQuantity = PriceRecived;
                                        }
                                    }

                                    #region Update Item In Item Table
                                    if (oItemData.Item.Rows.Count > 0)
                                    {
                                        VendorData oVendorData = new VendorData();
                                        using (VendorSvr oVendorSvr = new VendorSvr())
                                        {
                                            orignalSellingPrice = oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice].ToString();
                                            orignalAvgPrice = oItemData.Item[0][clsPOSDBConstants.Item_Fld_AvgPrice].ToString();
                                            orignalLastCostPrice = oItemData.Item[0][clsPOSDBConstants.Item_Fld_LastCostPrice].ToString();
                                            if (orignalLastCostPrice == "")//Added by Atul Joshi on 22-10-2010
                                                orignalLastCostPrice = "0.0";
                                            //get vendor data for vendor code frpm the itemDO object 
                                            //vendorData = vendorSvr.Populate(POS_Core.Resources.Configuration.convertNullToInt(itemData.Item[0][clsPOSDBConstants.Item_Fld_LastVendor].ToString()));
                                            sellingPrice = oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice].ToString();
                                            //averagePrice = itemData.Item[0][clsPOSDBConstants.Item_Fld_AvgPrice].ToString();
                                            vendorCostPrice = oItemData.Item[0][clsPOSDBConstants.Item_Fld_LastCostPrice].ToString();
                                            //itemDoItem.SalePrice;
                                            oVendorData = oVendorSvr.Populate(POS_Core.Resources.Configuration.convertNullToString(oItemData.Item[0][clsPOSDBConstants.Item_Fld_PreferredVendor].ToString()));
                                            if (oVendorData.Vendor.Rows.Count > 0 && POS_Core.Resources.Configuration.CPrimeEDISetting.IgnoreVendorSequence == false)   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                            {
                                                //if last vendor matches with vendor from itemDo object 
                                                // then it will update the prices in the item table.
                                                if ((oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorCode].ToString().ToUpper()) == varVendorCode.ToUpper())
                                                {
                                                    try
                                                    {
                                                        if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateVendorPrice == true) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                        {
                                                            if ((bool)(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_UpdatePrice]) == true)
                                                            {
                                                                if ((bool)(oItemData.Item[0][clsPOSDBConstants.Item_Fld_UpdatePrice]) == true)
                                                                {
                                                                    #region added logic to set selling price
                                                                    priceQualifier = oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_PriceQualifier].ToString();
                                                                    //Updated By SRT(Gaurav) Date: 07-07-2009
                                                                    //Updated to handel the '0' price update issue.
                                                                    //itemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice] = GetPrice(itemDoItem, priceQualifier);
                                                                    PriceRcvd = 0;
                                                                    PriceRcvd = GetPrice(oItemDoItem, priceQualifier);
                                                                    if (PriceRcvd > 0)
                                                                    {
                                                                        if ((bool)(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice]) == false) //Sprint-21 - 2208 27-Jul-2015 JY Added if part - if ReduceSellingPrice is false then do not overwrite the selling price from 832 file if it is less than the db value
                                                                        {
                                                                            if (POS_Core.Resources.Configuration.convertNullToDecimal(oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice]) < PriceRcvd)
                                                                                oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice] = PriceRcvd.ToString();
                                                                        }
                                                                        else
                                                                            oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice] = PriceRcvd.ToString();
                                                                    }
                                                                    //End Of Updated By SRT(Gaurav)
                                                                    #endregion

                                                                    #region added logic to set cost price price
                                                                    //Changes By SRT(Sachin) Date : 15 Feb 2010
                                                                    costQualifier = oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_CostQualifier].ToString();
                                                                    //Updated By SRT(Gaurav) Date: 07-07-2009
                                                                    //Updated to handel the '0' price update issue.
                                                                    //itemData.Item[0][clsPOSDBConstants.Item_Fld_AvgPrice] = GetPrice(itemDoItem, costQualifier);
                                                                    PriceRcvd = 0;
                                                                    PriceRcvd = GetPrice(oItemDoItem, costQualifier);
                                                                    if (PriceRcvd > 0)
                                                                    {
                                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_LastCostPrice] = PriceRcvd;
                                                                    }
                                                                    #endregion

                                                                    if (oItemDoItem.AverageWholesalePrice > 0)
                                                                    {
                                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_AvgPrice] = oItemDoItem.AverageWholesalePrice;
                                                                    }
                                                                    //Changes by SRT(Sachin) Date : 15 Feb 2010
                                                                    if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateDescription == true)    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                                    {
                                                                        if (oItemDoItem.Description.ToString() != string.Empty)
                                                                        {
                                                                            oItemData.Item[0][clsPOSDBConstants.Item_Fld_Description] = oItemDoItem.Description.ToString();
                                                                        }
                                                                    }
                                                                    else if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateDescription == false)  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                                    {
                                                                        if (oItemData.Item[0][clsPOSDBConstants.Item_Fld_Description].ToString() == string.Empty)
                                                                        {
                                                                            oItemData.Item[0][clsPOSDBConstants.Item_Fld_Description] = oItemDoItem.Description.ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        oItemData.Item[0][clsPOSDBConstants.ItemVendor_Fld_PckQty] = oItemDoItem.PckQty;
                                                        oItemData.Item[0][clsPOSDBConstants.ItemVendor_Fld_PckSize] = oItemDoItem.PckSize;
                                                        oItemData.Item[0][clsPOSDBConstants.ItemVendor_Fld_PckUnit] = oItemDoItem.PckUnit;
                                                        //Added for PromoCode
                                                        if (POS_Core.Resources.Configuration.convertNullToBoolean(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_SalePriceUpdate].ToString()))
                                                        {
                                                            try
                                                            {
                                                                string SaleQualifier = POS_Core.Resources.Configuration.convertNullToString(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_SalePriceQualifier]);
                                                                decimal SalePriceRcvd = 0;
                                                                int? nSaleStartDateStatus = null, nSaleEndDateStatus = null;   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added 

                                                                SalePriceRcvd = GetPrice(oItemDoItem, SaleQualifier);
                                                                if (SalePriceRcvd > 0)
                                                                {
                                                                    oItemData.Item[0][clsPOSDBConstants.Item_Fld_OnSalePrice] = SalePriceRcvd;
                                                                }
                                                                if (oItemDoItem.SaleStartDate != null && oItemDoItem.SaleStartDate != DateTime.MinValue)
                                                                {
                                                                    string date = oItemDoItem.SaleStartDate.Year.ToString();//Added By Ravindra on 14 March 2012
                                                                    if (date.Trim() != "1900")//Added By Ravindra on 14 March 2012
                                                                    {
                                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleStartDate] = oItemDoItem.SaleStartDate;
                                                                        nSaleStartDateStatus = DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleStartDate]).ToString("MM/dd/yyyy")), Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")));   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added
                                                                    }
                                                                    //itemDoItem.
                                                                }
                                                                if (oItemDoItem.SaleEndDate != null && oItemDoItem.SaleEndDate != DateTime.MinValue)
                                                                {
                                                                    string date = oItemDoItem.SaleEndDate.Year.ToString();//Added By Ravindra on 14 March 2012
                                                                    if (date.Trim() != "1900")//Added By Ravindra on 14 March 2012
                                                                    {
                                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleEndDate] = oItemDoItem.SaleEndDate;
                                                                        nSaleEndDateStatus = DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleEndDate]).ToString("MM/dd/yyyy")), Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")));   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added
                                                                    }
                                                                }

                                                                #region Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added logic to turn on isOnSale flag
                                                                if (SalePriceRcvd > 0 && nSaleStartDateStatus != null && nSaleStartDateStatus <= 0 && nSaleEndDateStatus != null && nSaleEndDateStatus >= 0)
                                                                {
                                                                    oItemData.Item[0][clsPOSDBConstants.Item_Fld_isOnSale] = true;
                                                                }
                                                                #endregion
                                                            }
                                                            catch (Exception)
                                                            {
                                                                throw;
                                                            }
                                                        }
                                                        //try
                                                        //{
                                                        //    if (itemDoItem.SalePrice > 0)
                                                        //        itemData.Item[0][clsPOSDBConstants.Item_Fld_OnSalePrice] = itemDoItem.SalePrice;
                                                        //}
                                                        //catch (Exception ex)
                                                        //{
                                                        //    ///log
                                                        //   POS_Core.ErrorLogging.Logs.Logger("Error in SalePrice Update .Please use latest version for  Vendor.dll ");
                                                        //}
                                                        //End Of Updated By SRT(Gaurav)
                                                        ////Updated By SRT(Ritesh Parekh) Date : 27-Jul-2009
                                                        ////Last vendor updated in code format.
                                                        //POS.BusinessRules.Vendor oVendor = new POS.BusinessRules.Vendor();
                                                        //itemData.Item[0][clsPOSDBConstants.Item_Fld_LastVendor] = oVendor.GetVendorCode(vendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString());//vendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString()
                                                        //End Of Updated By SRT(Ritesh Parekh)

                                                        //itemVenodeData = UpdateItemVendor(vendorData.Vendor[0]["VendorID"].ToString(), itemDoItem, itemData);
                                                        TotalItemUpdated++;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ErrorHandler.logException(ex, "", "");
                                                        error++;
                                                    }
                                                }
                                                else
                                                {
                                                    //MessageBox.Show(vendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorCode].ToString().ToUpper() + "::" + itemDoItem.VendorCode.ToString());
                                                    //Else it will update the prices in Item vendor Table.
                                                    //itemVenodeData = UpdateItemVendor(vendorData.Vendor[0]["VendorID"].ToString(), itemDoItem, itemData);
                                                    TotalItemUpdated++;
                                                }
                                                if (oItemDoItem.IsDeleted == true)
                                                {
                                                    oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice] = orignalSellingPrice;
                                                    oItemData.Item[0][clsPOSDBConstants.Item_Fld_LastCostPrice] = orignalLastCostPrice;
                                                    oItemData.Item[0][clsPOSDBConstants.Item_Fld_AvgPrice] = orignalAvgPrice;
                                                }
                                                POS_Core.Resources.Configuration.UpdatedBy = "E";
                                                oItem.Persist(oItemData);
                                                //itemVendor.Persist(itemVenodeData);
                                                //Application.DoEvents();
                                                count++;
                                            }
                                            else
                                            {
                                                bool upDateAnyways = false;
                                                oVendorData = oVendorSvr.PopulateList(" Where PrimePOVendorCode ='" + oItemDoItem.VendorCode + "'");
                                                if (POS_Core.Resources.Configuration.CPrimeEDISetting.IgnoreVendorSequence == true) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                {
                                                    upDateAnyways = true;
                                                }
                                                else if (POS_Core.Resources.Configuration.CPrimeEDISetting.UseDefaultVendor == true)    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                {
                                                    if (POS_Core.Resources.Configuration.CPrimeEDISetting.DefaultVendor.Trim().ToUpper() == oVendorData.Vendor[0]["VendorCode"].ToString().Trim().ToUpper())    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                    {
                                                        upDateAnyways = true;
                                                    }
                                                }
                                                try
                                                {
                                                    //oVendorData = oVendorSvr.PopulateList(" Where PrimePOVendorCode ='" + oItemDoItem.VendorCode + "'");  //Sprint-24 - 12-Jan-2017 JY Commented as not in use
                                                    costQualifier = oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_CostQualifier].ToString();
                                                    priceQualifier = oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_PriceQualifier].ToString();

                                                    if (upDateAnyways == true)
                                                    {
                                                        if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateVendorPrice == true) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                        {
                                                            if ((bool)(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_UpdatePrice]) == true)
                                                            {
                                                                if ((bool)(oItemData.Item[0][clsPOSDBConstants.Item_Fld_UpdatePrice]) == true)
                                                                {
                                                                    //Updated By SRT(Gaurav) Date: 07-07-2009
                                                                    //Updated to handel the '0' price update issue.
                                                                    //itemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice] = GetPrice(itemDoItem, priceQualifier);
                                                                    PriceRcvd = 0;
                                                                    PriceRcvd = GetPrice(oItemDoItem, priceQualifier);
                                                                    //if ((oItemData.Tables[0].Rows[0]["PckUnit"].ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (oItemData.Tables[0].Rows[0]["PckUnit"].ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA)) //Sprint-21 22-Feb-2016 JY Added CA for case item
                                                                    //{
                                                                    //    ;
                                                                    //}

                                                                    if (PriceRcvd > 0)
                                                                    {
                                                                        if ((bool)(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice]) == false) //Sprint-21 - 2208 27-Jul-2015 JY Added if part - if ReduceSellingPrice is false then do not overwrite the selling price from 832 file if it is less than the db value
                                                                        {
                                                                            if (POS_Core.Resources.Configuration.convertNullToDecimal(oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice]) < PriceRcvd)
                                                                                oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice] = PriceRcvd;
                                                                        }
                                                                        else
                                                                            oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice] = PriceRcvd;
                                                                    }
                                                                    //End Of Updated By SRT(Gaurav)

                                                                    //change by SRT(Sachin) Data 15 Feb 2010
                                                                    //Updated By SRT(Gaurav) Date: 07-07-2009
                                                                    //Updated to handel the '0' price update issue.
                                                                    //itemData.Item[0][clsPOSDBConstants.Item_Fld_AvgPrice] = GetPrice(itemDoItem, costQualifier);
                                                                    PriceRcvd = 0;
                                                                    PriceRcvd = GetPrice(oItemDoItem, costQualifier);
                                                                    if (PriceRcvd > 0)
                                                                    {
                                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_LastCostPrice] = PriceRcvd;
                                                                    }
                                                                    if (oItemDoItem.AverageWholesalePrice > 0)
                                                                    {
                                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_AvgPrice] = oItemDoItem.AverageWholesalePrice;
                                                                    }

                                                                    //End Of Updated By SRT(Gaurav)
                                                                    //End of change by SRT(Sachin) Data 15 Feb 2010

                                                                    if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateDescription == true)    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                                    {
                                                                        if (oItemDoItem.Description.ToString() != string.Empty)
                                                                        {
                                                                            oItemData.Item[0][clsPOSDBConstants.Item_Fld_Description] = oItemDoItem.Description.ToString();
                                                                        }
                                                                    }
                                                                    else if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateDescription == false)  //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                                    {
                                                                        if (oItemData.Item[0][clsPOSDBConstants.Item_Fld_Description].ToString() == string.Empty)
                                                                        {
                                                                            oItemData.Item[0][clsPOSDBConstants.Item_Fld_Description] = oItemDoItem.Description.ToString();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    oItemData.Item[0][clsPOSDBConstants.ItemVendor_Fld_PckQty] = oItemDoItem.PckQty;
                                                    oItemData.Item[0][clsPOSDBConstants.ItemVendor_Fld_PckSize] = oItemDoItem.PckSize;
                                                    oItemData.Item[0][clsPOSDBConstants.ItemVendor_Fld_PckUnit] = oItemDoItem.PckUnit;
                                                    //Added for PromoCode
                                                    if (POS_Core.Resources.Configuration.convertNullToBoolean(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_SalePriceUpdate].ToString()))
                                                    {
                                                        string SaleQualifier = POS_Core.Resources.Configuration.convertNullToString(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_SalePriceQualifier]);//4-Nov-2014 Ravindra added For SalePriceQualifier;
                                                        decimal SalePriceRcvd = 0;
                                                        int? nSaleStartDateStatus = null, nSaleEndDateStatus = null;   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added 

                                                        SalePriceRcvd = GetPrice(oItemDoItem, SaleQualifier);
                                                        if (SalePriceRcvd > 0)
                                                        {
                                                            oItemData.Item[0][clsPOSDBConstants.Item_Fld_OnSalePrice] = SalePriceRcvd;
                                                        }
                                                        if (oItemDoItem.SaleStartDate != null && oItemDoItem.SaleStartDate != DateTime.MinValue)
                                                        {
                                                            string date = oItemDoItem.SaleStartDate.Year.ToString();//Added By Ravindra on 14 March 2012
                                                            if (date.Trim() != "1900")//Added By Ravindra on 14 March 2012
                                                            {
                                                                oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleStartDate] = oItemDoItem.SaleStartDate;
                                                                nSaleStartDateStatus = DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleStartDate]).ToString("MM/dd/yyyy")), Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")));   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added
                                                            }
                                                        }
                                                        if (oItemDoItem.SaleEndDate != null && oItemDoItem.SaleEndDate != DateTime.MinValue)
                                                        {
                                                            string date = oItemDoItem.SaleEndDate.Year.ToString();//Added By Ravindra on 14 March 2012
                                                            if (date.Trim() != "1900")//Added By Ravindra on 14 March 2012
                                                            {
                                                                oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleEndDate] = oItemDoItem.SaleEndDate;
                                                                nSaleEndDateStatus = DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleEndDate]).ToString("MM/dd/yyyy")), Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")));   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added
                                                            }
                                                        }

                                                        #region Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added logic to turn on isOnSale flag
                                                        if (SalePriceRcvd > 0 && nSaleStartDateStatus != null && nSaleStartDateStatus <= 0 && nSaleEndDateStatus != null && nSaleEndDateStatus >= 0)
                                                        {
                                                            oItemData.Item[0][clsPOSDBConstants.Item_Fld_isOnSale] = true;
                                                        }
                                                        #endregion
                                                    }

                                                    //itemDoItem.sa
                                                    //try
                                                    //{
                                                    //    if (itemDoItem.SalePrice > 0)
                                                    //        itemData.Item[0][clsPOSDBConstants.Item_Fld_OnSalePrice] = itemDoItem.SalePrice;
                                                    //}
                                                    //catch (Exception ex)
                                                    //{
                                                    //    ///log
                                                    //   POS_Core.ErrorLogging.Logs.Logger("Error in SalePrice Update .Please use latest version for  Vendor.dll ");
                                                    //}
                                                    //itemVenodeData = UpdateItemVendor(vendorData.Vendor[0]["VendorID"].ToString(), itemDoItem, itemData);
                                                    TotalItemUpdated++;
                                                    if (oItemDoItem.IsDeleted == true)
                                                    {
                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice] = orignalSellingPrice;
                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_LastCostPrice] = orignalLastCostPrice;
                                                        oItemData.Item[0][clsPOSDBConstants.Item_Fld_AvgPrice] = orignalAvgPrice;
                                                    }
                                                    POS_Core.Resources.Configuration.UpdatedBy = "E";
                                                    oItem.Persist(oItemData);
                                                    // itemVendor.Persist(itemVenodeData);
                                                    //Application.DoEvents();
                                                    count++;
                                                }
                                                catch (Exception ex)
                                                {
                                                    ErrorHandler.logException(ex, "", "");
                                                }
                                            }
                                            oVendorData = oVendorSvr.PopulateList(" Where PrimePOVendorCode ='" + oItemDoItem.VendorCode + "'");
                                            itemvalues[0] = oItemDoItem.ItemCode;
                                            itemvalues[1] = oItemDoItem.Description;
                                            //itemvalues[2] = averagePrice;
                                            itemvalues[2] = vendorCostPrice;
                                            //itemvalues[3] = averagePrice;
                                            itemvalues[3] = vendorCostPrice;
                                            itemvalues[5] = sellingPrice;
                                            priceQualifier = oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_PriceQualifier].ToString();
                                            if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateVendorPrice == true) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                            {
                                                if ((bool)(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_UpdatePrice]) == true)
                                                {
                                                    if ((bool)(oItemData.Item[0][clsPOSDBConstants.Item_Fld_UpdatePrice]) == true)
                                                    {
                                                        //Updated By SRT(Gaurav) Date: 07-07-2009
                                                        //Updated to handel the '0' price update issue.
                                                        //itemvalues[5] = GetPrice(itemDoItem, priceQualifier).ToString();
                                                        PriceRcvd = 0;
                                                        PriceRcvd = GetPrice(oItemDoItem, priceQualifier);
                                                        if (PriceRcvd > 0)
                                                        {
                                                            itemvalues[5] = PriceRcvd.ToString();
                                                        }
                                                        ////End Of Updated By SRT(Gaurav)
                                                        //Changes By SRT(Sachin) Date : 15 Feb 2010                                       
                                                        //Updated By SRT(Gaurav) Date: 07-07-2009
                                                        //Updated to handel the '0' price update issue.
                                                        //itemvalues[3] = GetPrice(itemDoItem, costQualifier).ToString();
                                                        PriceRcvd = 0;
                                                        PriceRcvd = GetPrice(oItemDoItem, costQualifier);
                                                        if (PriceRcvd > 0)
                                                        {
                                                            itemvalues[3] = PriceRcvd.ToString();
                                                        }
                                                        //End Of Updated By SRT(Gaurav)
                                                        itemvalues[4] = sellingPrice;
                                                        //End of Changes By SRT(Sachin) Date : 15 Feb 2010
                                                    }
                                                }
                                            }

                                            itemvalues[6] = oItemDoItem.IsDeleted.ToString();
                                            oItemDoItem.UnitCostPrice = oItemDoItem.UnitCostPrice;
                                            itemStringToinsert = String.Format(itemString, itemvalues);
                                            //call method to save ItemVendor data
                                            oItemVendorData = UpdateItemVendor(oVendorData.Vendor[0]["VendorID"].ToString(), oItemDoItem, oItemData);
                                            itemVendor.Persist(oItemVendorData);
                                            write.WriteLine(itemStringToinsert);
                                        }
                                    }
                                    #endregion
                                    #region Insert Item In Item Table
                                    else
                                    {
                                        //ItemVendor oitemVendor = new ItemVendor();    //Sprint-24 - 12-Jan-2017 JY Commented as not in use
                                        ItemRow itemRow = oItemData.Item.NewItemRow();

                                        VendorData oVendorData = new VendorData();
                                        VendorSvr oVendorSvr = new VendorSvr();

                                        //vendorData = vendorSvr.Populate(itemDoItem.VendorCode);
                                        oVendorData = oVendorSvr.PopulateList(" Where PrimePOVendorCode ='" + oItemDoItem.VendorCode + "'");
                                        try
                                        {
                                            //Added by SRT(Sachin) Date : 23 Feb 2010
                                            if ((oItemDoItem.ItemCode.Length == 12) && (POS_Core.Resources.Configuration.CPrimeEDISetting.Insert11DigitItem == true))   //PRIMEPOS-3167 07-Nov-2022 JY Modified
                                                itemRow.ItemID = oItemDoItem.ItemCode.Substring(0, oItemDoItem.ItemCode.Length - 1);
                                            else
                                                itemRow.ItemID = oItemDoItem.ItemCode;
                                            //End of Added by SRT(Sachin) Date : 23 Feb 2010
                                            ////Updated By SRT(Ritesh Parekh) Date : 27-Jul-2009
                                            ////Last vendor updated in code format.
                                            //POS.BusinessRules.Vendor oVendor = new POS.BusinessRules.Vendor();
                                            //itemRow.LastVendor = oVendor.GetVendorCode(vendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString());//vendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString();
                                            ////End Of Updated By SRT(Ritesh Parekh)

                                            //Commented by SRT(Abhishek) Date : 02/09/2009                               
                                            itemRow.DepartmentID = POS_Core.Resources.Configuration.CInfo.DefaultDeptId;
                                            //End of //Commented by SRT(Abhishek) Date : 02/09/2009

                                            itemRow.ReOrderLevel = 0;

                                            itemRow.MinOrdQty = 0;
                                            itemRow.QtyInStock = 0;
                                            itemRow.QtyOnOrder = 0;

                                            //Added to initailize the prices 
                                            //if prices are not initailize then it will 
                                            //throw exception for arithmetic oveflow. while inserting the Item.

                                            //Added by SRT(Abhishek)  Date : 07/23/2009
                                            itemRow.SellingPrice = 0;
                                            itemRow.LastCostPrice = 0;
                                            itemRow.AvgPrice = 0;
                                            //End of Added by SRT(Abhishek)

                                            itemRow.Description = oItemDoItem.Description;
                                            itemRow.Discount = oItemDoItem.Discount;

                                            //itemRow.AvgPrice = itemDoItem.AverageWholesalePrice;
                                            //itemRow.SellingPrice = itemDoItem.SellingPrice;

                                            priceQualifier = oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_PriceQualifier].ToString();
                                            //Why can this be in the Case structure. RItesh
                                            //Added Common function which will be called and will return price.
                                            //Updated By SRT(Gaurav) Date: 07-07-2009
                                            //Updated to handel the '0' price update issue.
                                            //itemRow.SellingPrice = GetPrice(itemDoItem, priceQualifier);

                                            PriceRcvd = 0;
                                            PriceRcvd = GetPrice(oItemDoItem, priceQualifier);
                                            if (PriceRcvd > 0)
                                            {
                                                itemRow.SellingPrice = PriceRcvd;
                                            }
                                            //End Of Updated By SRT(Gaurav)
                                            costQualifier = oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_CostQualifier].ToString();
                                            //itemRow.AvgPrice = GetPrice(itemDoItem, costQualifier);

                                            //Updated By SRT(Gaurav) Date: 07-07-2009
                                            //Updated to handel the '0' price update issue.
                                            PriceRcvd = 0;
                                            PriceRcvd = GetPrice(oItemDoItem, costQualifier);
                                            if (PriceRcvd > 0)
                                            {
                                                itemRow.LastCostPrice = PriceRcvd;
                                            }
                                            //End Of Updated By SRT(Gaurav)

                                            PriceRcvd = 0;
                                            PriceRcvd = GetPrice(oItemDoItem, clsPOSDBConstants.AWP);
                                            if (oItemDoItem.AverageWholesalePrice > 0)
                                            {
                                                itemRow.AvgPrice = oItemDoItem.AverageWholesalePrice;
                                            }
                                            itemRow.Freight = Convert.ToDecimal(0.0000);
                                            itemRow.isTaxable = oItemDoItem.IsTaxable;
                                            itemRow.isDiscountable = oItemDoItem.IsDiscountable;
                                            itemRow.Discount = oItemDoItem.Discount;
                                            itemRow.QtyInStock = 0;
                                            itemRow.isOnSale = false;
                                            itemRow.ExclFromAutoPO = false;
                                            itemRow.ExclFromRecpt = false;
                                            itemRow.isOTCItem = false;
                                            itemRow.UpdatePrice = true;

                                            itemRow.PckQty = oItemDoItem.PckQty;
                                            itemRow.PckSize = oItemDoItem.PckSize;
                                            itemRow.PckUnit = oItemDoItem.PckUnit;
                                            //Added for PromoCode
                                            if (POS_Core.Resources.Configuration.convertNullToBoolean(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_SalePriceUpdate].ToString()))
                                            {
                                                string SaleQualifier = POS_Core.Resources.Configuration.convertNullToString(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_SalePriceQualifier]);
                                                decimal SalePriceRcvd = 0;
                                                int? nSaleStartDateStatus = null, nSaleEndDateStatus = null;   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added 

                                                SalePriceRcvd = GetPrice(oItemDoItem, SaleQualifier);
                                                if (SalePriceRcvd > 0)
                                                {
                                                    oItemData.Item[0][clsPOSDBConstants.Item_Fld_OnSalePrice] = SalePriceRcvd;
                                                }
                                                if (oItemDoItem.SaleStartDate != null && oItemDoItem.SaleStartDate != DateTime.MinValue)
                                                {
                                                    string date = oItemDoItem.SaleStartDate.Year.ToString();//Added By Ravindra on 14 March 2012
                                                    if (date.Trim() != "1900")//Added By Ravindra on 14 March 2012
                                                    {
                                                        itemRow.SaleStartDate = oItemDoItem.SaleStartDate;
                                                        nSaleStartDateStatus = DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleStartDate]).ToString("MM/dd/yyyy")), Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")));   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added
                                                    }
                                                }
                                                if (oItemDoItem.SaleEndDate != null && oItemDoItem.SaleEndDate != DateTime.MinValue)
                                                {
                                                    string date = oItemDoItem.SaleEndDate.Year.ToString();//Added By Ravindra on 14 March 2012
                                                    if (date.Trim() != "1900")//Added By Ravindra on 14 March 2012
                                                    {
                                                        itemRow.SaleEndDate = oItemDoItem.SaleEndDate;
                                                        nSaleEndDateStatus = DateTime.Compare(Convert.ToDateTime(Convert.ToDateTime(oItemData.Item[0][clsPOSDBConstants.Item_Fld_SaleEndDate]).ToString("MM/dd/yyyy")), Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")));   //Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added
                                                    }
                                                }

                                                #region Sprint-25 - PRIMEPOS-294 02-Mar-2017 JY Added logic to turn on isOnSale flag
                                                if (SalePriceRcvd > 0 && nSaleStartDateStatus != null && nSaleStartDateStatus <= 0 && nSaleEndDateStatus != null && nSaleEndDateStatus >= 0)
                                                {
                                                    oItemData.Item[0][clsPOSDBConstants.Item_Fld_isOnSale] = true;
                                                }
                                                #endregion
                                            }
                                            //try
                                            //{
                                            //    if (itemDoItem.SalePrice > 0)
                                            //        itemData.Item[0][clsPOSDBConstants.Item_Fld_OnSalePrice] = itemDoItem.SalePrice;
                                            //}
                                            //catch (Exception ex)
                                            //{
                                            //    ///log
                                            //   POS_Core.ErrorLogging.Logs.Logger("Error in SalePrice Update .Please use latest version for  Vendor.dll ");
                                            //}
                                            oItemData.Item.AddRow(itemRow, true);
                                            //Change by SRT(Sachin) Date : 23 Feb 2010
                                            //itemVenodeData = GetItemVendorDataSet(itemDoItem, Configuration.convertNullToInt(vendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString()), itemDoItem.ItemCode);
                                            oItemVendorData = GetItemVendorDataSet(oItemDoItem, POS_Core.Resources.Configuration.convertNullToInt(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString()), itemRow.ItemID);
                                            PriceRcvd = 0;
                                            PriceRcvd = GetPrice(oItemDoItem, costQualifier);
                                            if (PriceRcvd > 0)
                                            {
                                                oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice] = PriceRcvd;
                                            }

                                            if (POS_Core.Resources.Configuration.convertNullToDecimal(oItemDoItem.SalePrice) >= 0)
                                            {
                                                oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice] = POS_Core.Resources.Configuration.convertNullToDecimal(oItemDoItem.SalePrice);
                                            }
                                            if (oItemDoItem.SaleStartDate != null && oItemDoItem.SaleStartDate != DateTime.MinValue)
                                            {
                                                string date = oItemDoItem.SaleStartDate.Year.ToString();
                                                if (date.Trim() != "1900")
                                                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_SaleStartDate] = oItemDoItem.SaleStartDate;
                                            }
                                            if (oItemDoItem.SaleEndDate != null && oItemDoItem.SaleEndDate != DateTime.MinValue)
                                            {
                                                string date = oItemDoItem.SaleEndDate.Year.ToString();
                                                if (date.Trim() != "1900")
                                                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_SaleEndDate] = oItemDoItem.SaleEndDate;
                                            }
                                            oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass] = POS_Core.Resources.Configuration.convertNullToString(oItemDoItem.HammacherDeptClass).Trim();

                                            POS_Core.Resources.Configuration.UpdatedBy = "E";
                                            oItem.Persist(oItemData);
                                            itemVendor.Persist(oItemVendorData);

                                            //Application.DoEvents();
                                            count++;
                                            TotalItemInserted++;

                                            itemvalues[0] = itemRow.ItemID;//itemDoItem.ItemCode; //Change by SRT(Sachin) Date : 23 Feb 2010
                                            itemvalues[1] = oItemDoItem.Description;
                                            itemvalues[2] = oItemData.Item[0].LastCostPrice.ToString();
                                            itemvalues[3] = oItemData.Item[0].LastCostPrice.ToString();
                                            itemvalues[4] = oItemData.Item[0].SellingPrice.ToString();
                                            itemvalues[5] = oItemData.Item[0].SellingPrice.ToString();

                                            itemvalues[6] = oItemDoItem.IsDeleted.ToString();
                                            itemStringToinsert = String.Format(itemString, itemvalues) + "\r\n";
                                            write.WriteLine(itemStringToinsert);
                                        }
                                        catch (Exception ex)
                                        {
                                            //What if an exception what you want to do. RItesh
                                            error++;
                                            POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        subPercentageCompleted = count;
                        return subPercentageCompleted;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.logException(ex, "", "");
                        // write.Close();
                        // fileInfo = null;              
                        return 0;
                    }
                }
            }
        }

        private ItemVendorData UpdateItemVendor(string vendorId, ItemDO itemDoItem, ItemData itemData)
        {
            ItemVendorData oItemVendorData = null;
            //ItemVendor oitemVendor = null;
            //ItemVendorData itemVenodeDataUpdate = null;   //Sprint-24 - 11-Jan-2017 JY Commented as not in use
            try
            {
                //itemVenodeDataUpdate = new ItemVendorData();  //Sprint-24 - 11-Jan-2017 JY Commented as not in use
                oItemVendorData = new ItemVendorData();
                using (ItemVendor oitemVendor = new ItemVendor())
                {
                    oItemVendorData = oitemVendor.PopulateList("  AND  ItemVendor.ItemID ='" + itemData.Item[0].ItemID.ToString().Trim() + "' AND ItemVendor.VendorItemID='" + itemDoItem.VendorItemCode.ToString() + "' AND ItemVendor.VendorID= " + POS_Core.Resources.Configuration.convertNullToInt(vendorId) + "");
                }

                //Added to check 
                VendorData oVendorData = new VendorData();
                using (VendorSvr oVendorSvr = new VendorSvr())
                {
                    oVendorData = oVendorSvr.Populate((POS_Core.Resources.Configuration.convertNullToInt(vendorId)));
                    //Added by Atul Joshi on 22-10-2010 if Error Comes and VedorData is null it again  Fetch
                    if (oVendorData == null || oVendorData.Vendor == null || oVendorData.Vendor.Rows.Count == 0)
                    {
                        oVendorData = oVendorSvr.Populate((POS_Core.Resources.Configuration.convertNullToInt(vendorId)));
                    }
                }

                if (oVendorData == null || oVendorData.Vendor == null || oVendorData.Vendor.Rows.Count == 0)
                    throw new Exception(" No Vendor Exist For VendorID =" + vendorId.ToString());

                string costQualifier = oVendorData.Vendor.Rows[0][clsPOSDBConstants.Vendor_Fld_CostQualifier].ToString();
                if (costQualifier == null || costQualifier == string.Empty)
                    throw new Exception(" No Cost Qualifier Set For Vendor=" + oVendorData.Vendor.Rows[0][clsPOSDBConstants.Vendor_Fld_VendorName].ToString());

                if (oItemVendorData.ItemVendor.Rows.Count > 0)
                {
                    decimal price = GetPrice(itemDoItem, costQualifier);

                    //Updated by (SRT)Sachin Date 19 Feb 2010 
                    if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateVendorPrice == true) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                    {
                        if ((bool)(oVendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_UpdatePrice]) == true)
                        {
                            if ((bool)(itemData.Item[0][clsPOSDBConstants.Item_Fld_UpdatePrice]) == true)
                            {
                                if (price > 0)
                                {
                                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice] = price;
                                }
                                if (itemDoItem.AverageWholesalePrice > 0)
                                {

                                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice] = itemDoItem.AverageWholesalePrice;
                                }
                            }
                        }
                    }
                    //End of Updated by (SRT)Sachin Date 19 Feb 2010
                    if (itemDoItem.CatPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_CatPrice] = itemDoItem.CatPrice;
                    }
                    if (itemDoItem.ConPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_ContractPrice] = itemDoItem.ConPrice;
                    }
                    if (itemDoItem.DealerAdjustPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice] = itemDoItem.DealerAdjustPrice;
                    }
                    if (itemDoItem.FedUpperlimitPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice] = itemDoItem.FedUpperlimitPrice;
                    }
                    if (itemDoItem.ManufacturerSuggPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice] = itemDoItem.ManufacturerSuggPrice;
                    }
                    if (itemDoItem.NetItemPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_NetItemPrice] = itemDoItem.NetItemPrice;
                    }
                    if (itemDoItem.ProducerPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_ProducerPrice] = itemDoItem.ProducerPrice;
                    }
                    if (itemDoItem.RetailPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_RetailPrice] = itemDoItem.RetailPrice;
                    }

                    //Added by SRT(Abhishek) Date : 24/09/2009
                    if (itemDoItem.InvoiceBillingPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice] = itemDoItem.InvoiceBillingPrice;
                    }
                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity] = itemDoItem.UnitPriceBeginingQuantity;

                    if (itemDoItem.BaseChargePrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_BaseCharge] = itemDoItem.BaseChargePrice;
                    }
                    if (itemDoItem.ResalePrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_ResalePrice] = itemDoItem.ResalePrice;
                    }
                    //Added by Atul Joshi on 22-10-2010
                    if (itemDoItem.UnitCostPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice] = itemDoItem.UnitCostPrice;
                    }
                    if (itemDoItem.SalePrice >= 0)//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorSalePrice] = POS_Core.Resources.Configuration.convertNullToDecimal(itemDoItem.SalePrice.ToString());
                    }

                    //End Of Added by SRT(Abhishek) Date : 24/09/2009 
                    if (itemDoItem.SaleStartDate != null && itemDoItem.SaleStartDate != DateTime.MinValue)
                    {
                        string date = itemDoItem.SaleStartDate.Year.ToString();
                        if (date.Trim() != "1900")
                            oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_SaleStartDate] = itemDoItem.SaleStartDate;
                    }
                    if (itemDoItem.SaleEndDate != null && itemDoItem.SaleEndDate != DateTime.MinValue)
                    {
                        string date = itemDoItem.SaleEndDate.Year.ToString();
                        if (date.Trim() != "1900")
                            oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_SaleEndDate] = itemDoItem.SaleEndDate;
                    }
                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_IsDeleted] = itemDoItem.IsDeleted;
                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_PckQty] = itemDoItem.PckQty;
                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_PckSize] = itemDoItem.PckSize;
                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_PckUnit] = itemDoItem.PckUnit;

                    oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_HammacherDeptClass] = POS_Core.Resources.Configuration.convertNullToString(itemDoItem.HammacherDeptClass).Trim();//Added by Ravindra PRIMEPOS-1628 EDI Promotional Pricing
                }
                else
                {
                    oItemVendorData = GetItemVendorDataSet(itemDoItem, POS_Core.Resources.Configuration.convertNullToInt(vendorId), itemData.Item[0].ItemID.ToString().Trim());
                    decimal price = GetPrice(itemDoItem, costQualifier);
                    if (price > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice] = price;
                    }
                    //Added by Atul Joshi on 22-10-2010
                    if (itemDoItem.UnitCostPrice > 0)
                    {
                        oItemVendorData.ItemVendor[0][clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice] = itemDoItem.UnitCostPrice;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
                return null;
            }
            return oItemVendorData;
        }
        #region commented
        //private ItemVendorData CreateNewEntryInItemVendor(ItemDO itemDoItem, ItemVendorData itemVenodeData, VendorData vendorData)
        //{
        //    ItemVendorRow oitemVendorRow = itemVenodeData.ItemVendor.NewItemVendorRow();

        //    try
        //    {
        //        #region
        //        oitemVendorRow.ItemID = itemDoItem.ItemCode;
        //        oitemVendorRow.VendorCode = itemDoItem.VendorCode;
        //        oitemVendorRow.VendorID = POS_Core.Resources.Configuration.convertNullToInt(vendorData.Vendor[0]["VendorID"].ToString());
        //        oitemVendorRow.VendorItemID = itemDoItem.VendorItemCode;

        //        oitemVendorRow.AverageWholeSalePrice = itemDoItem.AvgPrice;

        //        oitemVendorRow.VenorCostPrice = itemDoItem.SellingPrice;

        //        oitemVendorRow.AverageWholeSalePrice = itemDoItem.AverageWholesalePrice;
        //        oitemVendorRow.CatalogPrice = itemDoItem.CatPrice;
        //        oitemVendorRow.ContractPrice = itemDoItem.ConPrice;
        //        oitemVendorRow.DealerAdjustedPrice = itemDoItem.DealerAdjustPrice;

        //        oitemVendorRow.FedrelUpperLimitPrice = itemDoItem.FedUpperlimitPrice;
        //        oitemVendorRow.ManufacturerSuggPrice = itemDoItem.ManufacturerSuggPrice;
        //        oitemVendorRow.NetItemPrice = itemDoItem.NetItemPrice;
        //        oitemVendorRow.ProducersPrice = itemDoItem.ProducerPrice;
        //        oitemVendorRow.RetailPrice = itemDoItem.RetailPrice;

        //        oitemVendorRow.IsDeleted = itemDoItem.IsDeleted;

        //        itemVenodeData.ItemVendor.AddRow(oitemVendorRow, true);
        //        oitemVendorRow = null;
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        //error++;
        //        //Write what handling you are going to do. Ritesh
        //        if (oitemVendorRow.ItemID == null || itemDoItem.ItemCode == null)
        //            ErrorHandler.throwCustomError(POSErrorENUM.Item_CodeCanNotBeNULL);
        //        else if (oitemVendorRow.VendorCode == null || itemDoItem.VendorCode == null)
        //            ErrorHandler.throwCustomError(POSErrorENUM.Vendor_CodeCanNotBeNULL);
        //        else if (oitemVendorRow.VendorID == 0 || vendorData.Vendor[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString() == null)
        //            ErrorHandler.throwCustomError(POSErrorENUM.Vendor_PrimaryKeyVoilation);
        //        else if (oitemVendorRow.VendorItemID == null || itemDoItem.VendorItemCode == null)
        //            ErrorHandler.throwCustomError(POSErrorENUM.ItemVendor_VendorItemIDCanNotBeNull);
        //        else
        //            ErrorHandler.throwException(ex, "", "");
        //    }
        //    return itemVenodeData;
        //}
        #endregion

        private ItemVendorData GetItemVendorDataSet(ItemDO itemDoItem, int vendorID, string itemId)
        {
            try
            {
                ItemVendorData oItemVendorData = new ItemVendorData();
                ItemVendorRow oitemVendorRowInsert = oItemVendorData.ItemVendor.NewItemVendorRow();

                oitemVendorRowInsert.ItemID = itemId;
                oitemVendorRowInsert.VendorCode = itemDoItem.VendorCode;
                oitemVendorRowInsert.VendorID = vendorID;
                oitemVendorRowInsert.VendorItemID = itemDoItem.VendorItemCode;
                oitemVendorRowInsert.AverageWholeSalePrice = itemDoItem.AvgPrice;
                oitemVendorRowInsert.VenorCostPrice = POS_Core.Resources.Configuration.convertNullToDecimal(itemDoItem.LastCostPrice.ToString());  // Configuration.convertNullToDecimal(itemData.Item[0][clsPOSDBConstants.Item_Fld_SellingPrice].ToString());                  //itemDoItem.SellingPrice;

                oitemVendorRowInsert.AverageWholeSalePrice = itemDoItem.AverageWholesalePrice;
                oitemVendorRowInsert.CatalogPrice = itemDoItem.CatPrice;
                oitemVendorRowInsert.ContractPrice = itemDoItem.ConPrice;
                oitemVendorRowInsert.DealerAdjustedPrice = itemDoItem.DealerAdjustPrice;

                oitemVendorRowInsert.FedrelUpperLimitPrice = itemDoItem.FedUpperlimitPrice;
                oitemVendorRowInsert.ManufacturerSuggPrice = itemDoItem.ManufacturerSuggPrice;
                oitemVendorRowInsert.NetItemPrice = itemDoItem.NetItemPrice;
                oitemVendorRowInsert.ProducersPrice = itemDoItem.ProducerPrice;
                oitemVendorRowInsert.RetailPrice = itemDoItem.RetailPrice;

                //Added by SRT(Abhishek) Date : 24/09/2009
                oitemVendorRowInsert.InVoiceBillingPrice = itemDoItem.InvoiceBillingPrice;
                oitemVendorRowInsert.UnitPriceBegQuantity = itemDoItem.UnitPriceBeginingQuantity;
                //Added by Atul Joshi on 22-10-2010
                oitemVendorRowInsert.UnitCostPrice = itemDoItem.UnitCostPrice;
                oitemVendorRowInsert.BaseCharge = itemDoItem.BaseChargePrice;
                oitemVendorRowInsert.Resale = itemDoItem.ResalePrice;
                //End Of Added by SRT(Abhishek) Date : 24/09/2009

                oitemVendorRowInsert.IsDeleted = itemDoItem.IsDeleted;
                if (itemDoItem.SaleStartDate != null && itemDoItem.SaleStartDate != DateTime.MinValue)
                {
                    string date = itemDoItem.SaleStartDate.Year.ToString();
                    if (date.Trim() != "1900")
                        oitemVendorRowInsert.SaleStartDate = itemDoItem.SaleStartDate;
                }
                if (itemDoItem.SaleEndDate != null && itemDoItem.SaleEndDate != DateTime.MinValue)
                {
                    string date = itemDoItem.SaleEndDate.Year.ToString();
                    if (date.Trim() != "1900")
                        oitemVendorRowInsert.SaleEndDate = itemDoItem.SaleEndDate;
                }
                oitemVendorRowInsert.PckQty = itemDoItem.PckQty;
                oitemVendorRowInsert.PckSize = itemDoItem.PckSize;
                oitemVendorRowInsert.PckUnit = itemDoItem.PckUnit;
                oitemVendorRowInsert.HammacherDeptClass = itemDoItem.HammacherDeptClass.Trim();
                oitemVendorRowInsert.VendorSalePrice = itemDoItem.SalePrice;
                oItemVendorData.ItemVendor.AddRow(oitemVendorRowInsert, true);
                return oItemVendorData;
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
                return null;
            }
        }

        private ItemDO[] GetPriceDetailsForFile(long fileID)
        {
            ItemDO[] itemDO = null;
            try
            {
                bool fromLock = false;
                try
                {
                    fromLock = Monitor.TryEnter(lockObj, 1000);
                    if (fromLock)
                    {
                        itemDO = primePOUtil.itemInt.GetLatestItemData(ref fileID);
                    }
                }
                catch (TimeoutException ex)
                {
                    Monitor.Exit(lockObj);
                }
                finally
                {
                    if (fromLock)
                        Monitor.Exit(lockObj);
                }
            }
            catch (RemotingException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                itemDO = null;
            }
            catch (SocketException ex)
            {
                ClsUpdatePOStatus.UpdateStatusInst.UpdataConStatus(DISCONNECTED);
                HandlePrimePOConnectionFailure();
                itemDO = null;
            }
            catch (Exception ex)
            {
                //Log error atleast.
                ErrorHandler.logException(ex, "", "");
                itemDO = null;
            }
            return itemDO;
        }

        public void UpdateItemPrice(string ItemCode, string VendorItemCode, decimal Cost, int vendorID)
        {
            string VendorCode = "";
            ItemData ItemDataSet = new ItemData();
            ItemVendorData ItemVendorDataSet = new ItemVendorData();
            VendorData VendorDataSet = new VendorData();

            Item objItemSvr = new Item();
            ItemVendor objItemVendorSvr = new ItemVendor();
            POS_Core.BusinessRules.Vendor objVendorSvr = new POS_Core.BusinessRules.Vendor();

            ItemDataSet = objItemSvr.Populate(ItemCode);
            ItemVendorDataSet = objItemVendorSvr.PopulateList(" AND ItemID ='" + ItemCode + "' AND  VendorItemID = '" + VendorItemCode + "'");

            VendorDataSet = objVendorSvr.Populate(vendorID);
            if (VendorDataSet == null || VendorDataSet.Vendor == null || VendorDataSet.Vendor.Rows.Count == 0)
            {
                ErrorHandler.logException(new Exception("No Vendor Exist For Vendor VendorID =" + vendorID.ToString()), "", "");
                return;
            }

            VendorCode = VendorDataSet.Vendor.Rows[0][clsPOSDBConstants.Vendor_Fld_VendorCode].ToString();
            if (ItemDataSet.Item.Rows[0][clsPOSDBConstants.Item_Fld_PreferredVendor].ToString() == VendorCode)
            {
                if (Cost > 0)
                {


                    if ((ItemDataSet.Item.Rows[0][clsPOSDBConstants.ItemVendor_Fld_PckUnit].ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (ItemDataSet.Item.Rows[0][clsPOSDBConstants.ItemVendor_Fld_PckUnit].ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA))   //Sprint-21 22-Feb-2016 JY Added CA for case item
                    {
                        Cost = Decimal.Round(Decimal.Divide(Cost, MMSUtil.UtilFunc.ValorZeroDEC(ItemDataSet.Item.Rows[0][clsPOSDBConstants.ItemVendor_Fld_PckSize].ToString())), 2);
                        //Added By Shitaljit(QuicSolv) on 20 Jan 2012
                        //To update cost price if seetings in POS, Vendor, Item settings.
                        if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateVendorPrice == true) //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        {
                            if ((bool)(VendorDataSet.Vendor[0][clsPOSDBConstants.Vendor_Fld_UpdatePrice]) == true)
                            {
                                if ((bool)(ItemDataSet.Item[0][clsPOSDBConstants.Item_Fld_UpdatePrice]) == true)
                                {
                                    ItemDataSet.Item.Rows[0][clsPOSDBConstants.Item_Fld_LastCostPrice] = Cost;
                                    ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice] = Cost;
                                    SetPrice(VendorDataSet.Vendor.Rows[0][clsPOSDBConstants.Vendor_Fld_CostQualifier].ToString(), Cost, ref ItemVendorDataSet);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Added By Shitaljit(QuicSolv) on 20 Jan 2012
                        //To update cost price if seetings in POS, Vendor, Item settings.
                        if (POS_Core.Resources.Configuration.CPrimeEDISetting.UpdateVendorPrice == true)
                        {
                            if ((bool)(VendorDataSet.Vendor[0][clsPOSDBConstants.Vendor_Fld_UpdatePrice]) == true)
                            {
                                if ((bool)(ItemDataSet.Item[0][clsPOSDBConstants.Item_Fld_UpdatePrice]) == true)
                                {
                                    ItemDataSet.Item.Rows[0][clsPOSDBConstants.Item_Fld_LastCostPrice] = Cost;
                                    ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice] = Cost;
                                    SetPrice(VendorDataSet.Vendor.Rows[0][clsPOSDBConstants.Vendor_Fld_CostQualifier].ToString(), Cost, ref ItemVendorDataSet);
                                }
                            }
                        }
                    }
                }
            }
            POS_Core.Resources.Configuration.UpdatedBy = "E";
            objItemSvr.Persist(ItemDataSet);
            objItemVendorSvr.Persist(ItemVendorDataSet);
            objVendorSvr.Persist(VendorDataSet);
        }

        private void SetPrice(string CostQualifier, decimal Cost, ref ItemVendorData ItemVendorDataSet)
        {
            try
            {
                switch (CostQualifier)
                {

                    case clsPOSDBConstants.AWP:
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_AvgWholeSalePrice] = Cost;
                        break;
                    case clsPOSDBConstants.CAT:
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_CatPrice] = Cost;
                        break;
                    case clsPOSDBConstants.CON:
                        //price = itemDoItem.ConPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_ContractPrice] = Cost;
                        break;
                    case clsPOSDBConstants.DAP:
                        //price = itemDoItem.DealerAdjustPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_DealerAdjustPrice] = Cost;
                        break;
                    case clsPOSDBConstants.FUL:
                        //price = itemDoItem.FedUpperlimitPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_FederalUpperLimitPrice] = Cost;
                        break;
                    case clsPOSDBConstants.MSR:
                        //price = itemDoItem.ManufacturerSuggPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_ManufacturerSuggPrice] = Cost;
                        break;
                    case clsPOSDBConstants.NET:
                        // price = itemDoItem.NetItemPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_NetItemPrice] = Cost;
                        break;
                    case clsPOSDBConstants.PRO:
                        //price = itemDoItem.ProducerPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_ProducerPrice] = Cost;
                        break;
                    case clsPOSDBConstants.RES:
                        //price = itemDoItem.RetailPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_RetailPrice] = Cost;
                        break;
                    case clsPOSDBConstants.RTL:
                        //price = itemDoItem.RetailPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_RetailPrice] = Cost;
                        break;
                    case clsPOSDBConstants.BCH:
                        //price = itemDoItem.BaseChargePrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_BaseCharge] = Cost;
                        break;
                    case clsPOSDBConstants.RESM:
                        //price = itemDoItem.ResalePrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_ResalePrice] = Cost;
                        break;
                    case clsPOSDBConstants.INV:
                        //price = itemDoItem.InvoiceBillingPrice;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_InvBillingPrice] = Cost;
                        break;
                    case clsPOSDBConstants.PBQ:
                        //price = itemDoItem.UnitPriceBeginingQuantity;
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_UnitPriceBegQuantity] = Cost;
                        break;
                    //Added by Atul Joshi on 22-10-2010
                    case clsPOSDBConstants.UCP:
                        ItemVendorDataSet.ItemVendor.Rows[0][clsPOSDBConstants.ItemVendor_Fld_UnitCostPrice] = Cost;
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }
        }

        #region InitializeComponent

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PrimePOUtil
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "PrimePOUtil";
            this.Load += new System.EventHandler(this.PrimePOUtil_Load);
            this.ResumeLayout(false);

        }
        #endregion

        private void PrimePOUtil_Load(object sender, EventArgs e)
        {

        }



    }

    public class ClsUpdatePOStatus
    {

        #region public
        public delegate void UpdateConnStatus(string message);
        public static event UpdateConnStatus UpdateConnectionStatus;

        public delegate void UpdateLogData();
        public static event UpdateLogData UpdateLogStatus;

        public delegate void UpdateCount(int poIncomplete, int poAckCount, string msg, int poExpired, int poOverdue, int error, int maxAttempts, int deliveryReceived);
        public static event UpdateCount POCount;

        #endregion

        #region private Variables
        private static string logMessage = string.Empty;
        private static ClsUpdatePOStatus clsUpdateCount;
        private MessageLogData logDataSet = new MessageLogData();
        private MessageLogTable logDataTable = new MessageLogTable();

        private const string LOGDATE = "LogDate";
        private const string LOGMESSAGE = "LogMessage";
        private const string LOGTIME = "LogTime";
        private static int errorCount = 0;
        #endregion

        private ClsUpdatePOStatus()
        {
            //logDataSet = GetDataTable();
        }
        public static ClsUpdatePOStatus UpdateStatusInst
        {
            get
            {
                if (clsUpdateCount == null)
                {
                    clsUpdateCount = new ClsUpdatePOStatus();
                }
                return clsUpdateCount;
            }
        }


        public static string GetMessage
        {
            set { logMessage = value; }
            get { return logMessage; }
        }
        public MessageLogData GetLogData
        {
            set { clsUpdateCount.logDataSet = value; }
            get { return clsUpdateCount.logDataSet; }
        }
        public void FillLogDataSet(string message)
        {
            try
            {
                ClsUpdatePOStatus.GetMessage = message;

                MessageLogData currentMsg = new MessageLogData();
                MessageLogRow currentRow = (MessageLogRow)currentMsg.Tables[0].NewRow();

                MessageLogRow row = (MessageLogRow)logDataTable.NewRow();
                MessageLogTable logTable = new MessageLogTable();
                logTable = (MessageLogTable)logDataSet.Tables[0].Clone();

                row[LOGDATE] = DateTime.Now.Date;
                row[LOGTIME] = DateTime.Now.ToLongTimeString();
                row[LOGMESSAGE] = message;
                logDataTable.Rows.Add(row);

                //Current log Message
                currentRow[LOGDATE] = DateTime.Now.ToShortDateString();
                currentRow[LOGTIME] = DateTime.Now.ToLongTimeString();
                currentRow[LOGMESSAGE] = message;

                currentMsg.Tables[0].Rows.Add(currentRow);
                //logDataSet.Merge(logDataTable);
                //DataRow[] rowSorted = logDataSet.Tables[0].Select("","LOGDATE DESC");

                MessageLogRow[] rowSorted = (MessageLogRow[])logDataTable.Select("", "LOGDATE DESC");
                foreach (MessageLogRow rowsort in rowSorted)
                {
                    logTable.ImportRow(rowsort);
                }
                logDataSet.Tables[0].Clear();
                logDataSet.Merge(logTable);
                this.GetLogData = logDataSet;

                //Insert Current Log Message
                UpdateMessageLog(currentMsg);
                try
                {
                    UpdateLogStatus();
                }
                catch (Exception Ex)
                {
                    //PRIMEPOS-2765 29-Nov-2019 JY no need to capture exception, as the related event might got registered at later stage, which causes unwanted error logs
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }
        }
        private void UpdateMessageLog(MessageLogData logData)
        {
            MessageLog msgLog = null;
            try
            {
                msgLog = new MessageLog();
                msgLog.Persist(logData);
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }
        }
        private DataSet GetDataTable()
        {
            //DataColumn oCol1 = logDataTable.Columns.Add(LOGDATE);           
            //DataColumn oCol2 = logDataTable.Columns.Add(LOGMESSAGE);
            //logDataSet.Tables.Add(logDataTable);
            return logDataSet;
        }

        public void UpdataConStatus(string message)
        {
            //update connection status like  connected and disconnected          
            try
            {
                UpdateConnectionStatus(message);
            }
            catch (Exception ex)
            {
                //ErrorHandler.logException(ex, "", "");    //PRIMEPOS-2765 29-Nov-2019 JY Commented as no need to log it in errorlog 
            }
        }

        public void UpdatePOCount()
        {
            //update count for for queued ,pending ,Acknowledge orders
            // also update status message for 
            try
            {
                PurchaseOrder purchaseOrd = new PurchaseOrder();
                POHeaderData poHeaderData = new POHeaderData();

                //  int poSubmittedCount = 0;
                //  int poQueuedCount = 0;
                int poAcknowledgeCount = 0;
                int poIncompleteCount = 0;

                int poExpired = 0;
                int poOverdue = 0;
                int poError = 0;
                int poMaxAttempts = 0;
                int poDeliveryReceived = 0; //added by atul 25-oct-2010

                //poHeaderData = purchaseOrd.PopulateList(" AND Status IN ("+ (int)PurchseOrdStatus.Pending + ","+ (int)PurchseOrdStatus.Queued+") AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'");
                //poQueuedCount = poHeaderData.POHeader.Rows.Count;

                //poHeaderData = purchaseOrd.PopulateList(" AND Status=" + (int)PurchseOrdStatus.Submitted + " AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'");  // AND '" + DateTime.Today + "'");
                //poSubmittedCount = poHeaderData.POHeader.Rows.Count;

                poHeaderData = purchaseOrd.PopulateList(" AND Status IN (" + (int)PurchseOrdStatus.Acknowledge + " ," + (int)PurchseOrdStatus.AcknowledgeManually + ") AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'"); //AND '" + DateTime.Today + "'");
                poAcknowledgeCount = poHeaderData.POHeader.Rows.Count;

                poHeaderData = purchaseOrd.PopulateList(" AND Status =" + (int)PurchseOrdStatus.Expired + " AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'"); //AND '" + DateTime.Today + "'");
                poExpired = poHeaderData.POHeader.Rows.Count;

                poHeaderData = purchaseOrd.PopulateList(" AND Status =" + (int)PurchseOrdStatus.Overdue + " AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'"); //AND '" + DateTime.Today + "'");
                poOverdue = poHeaderData.POHeader.Rows.Count;

                poHeaderData = purchaseOrd.PopulateList(" AND Status =" + (int)PurchseOrdStatus.Error + " AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'"); //AND '" + DateTime.Today + "'");
                poError = poHeaderData.POHeader.Rows.Count;

                poHeaderData = purchaseOrd.PopulateList(" AND Status =" + (int)PurchseOrdStatus.MaxAttempt + " AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'"); //AND '" + DateTime.Today + "'");
                poMaxAttempts = poHeaderData.POHeader.Rows.Count;

                poHeaderData = purchaseOrd.PopulateList(" AND Status =" + (int)PurchseOrdStatus.Incomplete + " AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'"); //AND '" + DateTime.Today + "'");
                poIncompleteCount = poHeaderData.POHeader.Rows.Count;

                //added by atul 25-oct-2010
                poHeaderData = purchaseOrd.PopulateList(" AND Status =" + (int)PurchseOrdStatus.DeliveryReceived + " AND OrderDate Between '" + DateTime.Today.ToShortDateString() + " 00:00:00" + "' AND '" + DateTime.Today.ToShortDateString() + " 23:59:59" + "'"); //AND '" + DateTime.Today + "'");
                poDeliveryReceived = poHeaderData.POHeader.Rows.Count;
                //End of added by atul 25-oct-2010

                POCount(poIncompleteCount, poAcknowledgeCount, ClsUpdatePOStatus.GetMessage, poExpired, poOverdue, poError, poMaxAttempts, poDeliveryReceived);

            }
            catch (Exception ex)
            {
                //ErrorHandler.logException(ex, "", "");    //PRIMEPOS-2971 04-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }
    }

    #region PRIMEPOS-2679
    public class PrimePOUtilItem : IItem
    {
        public string NDC { get; set; }
        PrimeEDIHelper oPrimeEDIHelper = new PrimeEDIHelper(POS_Core.Resources.Configuration.CPrimeEDISetting.HostAddress);  //PRIMEPOS-3167 07-Nov-2022 JY Modified
        public ItemDO[] GetLatestItemData(ref long iFileId) { return oPrimeEDIHelper.GetLatestItemData(ref iFileId); }

        public long GetMaxFileId() { return oPrimeEDIHelper.GetMaxFileId(); }

        //public ItemDO[] FetchItemData(string itemCode, string vendorCode)
        //{
        //    return oPrimeEDIHelper.FetchItemData(itemCode, vendorCode);
        //}
        public ItemDO GetVendorItemCode(string upcCode, string vendorCode) { return oPrimeEDIHelper.GetVendorItemCode(upcCode, vendorCode); }

        public int CreateItemDataSet(ref long fileID, ref string fileName, string VendorCode) { return oPrimeEDIHelper.CreateItemDataSet(ref fileID, ref fileName, VendorCode); }

        public ItemDO[] GetLatestItemDataForRange(int StartIndex, int EndIndex) { return oPrimeEDIHelper.GetLatestItemDataForRange(StartIndex, EndIndex); }

        //public DataSet GetItemPriceData(string itemCode, string vendorCode, string fromDat, string toDate)
        //{
        //    return oPrimeEDIHelper.GetItemPriceData(itemCode, vendorCode, fromDat, toDate);
        //}

        public int PurgeItemDataSet() { return oPrimeEDIHelper.PurgeItemDataSet(); }

        #region Extra Interface methods 
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public bool AddItem(ItemDO objItemDo)
        {
            throw new NotImplementedException();
        }

        public bool UpdateItem(ItemDO objItemDo)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItem(ItemDO objItemDo)
        {
            throw new NotImplementedException();
        }

        public string GetMaxFileName()
        {
            throw new NotImplementedException();
        }

        public ItemDO[] FetchItemData(string ItemCode)
        {
            throw new NotImplementedException();
        }

        public string forcefunc(string VendorCode)
        {
            throw new NotImplementedException();
        }

        public DataSet GetDownloadDetailData(string whereclause)
        {
            throw new NotImplementedException();
        }

        public string[] GetVendorList()
        {
            throw new NotImplementedException();
        }

        public ItemDO[] FetchItemData(string itemCode, string vendorCode)
        {
            throw new NotImplementedException();
        }

        public DataSet GetItemPriceData(string itemCode, string vendorCode, string fromDat, string toDate)
        {
            throw new NotImplementedException();
        }

        public int CreateItemDataSet(ref long fileID, ref string fileName, ref string FileType, string VendorCode)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    public class PrimePOUtilPurchaseOrder : IPurchaseOrder
    {
        PrimeEDIHelper oPrimeEDIHelper = new PrimeEDIHelper(POS_Core.Resources.Configuration.CPrimeEDISetting.HostAddress);  //PRIMEPOS-3167 07-Nov-2022 JY Modified
        public bool RetryPO(Dictionary<long, string> dict) { return Convert.ToBoolean(oPrimeEDIHelper.RetryPO(dict)); }

        public PurcahseOrderDO[] GetDirectPODetails(string Application, ref int poIdCount) { return oPrimeEDIHelper.GetDirectPODetails(Application, ref poIdCount); }

        public bool CreatePO(ref PurcahseOrderDO purchaseOrder) { return Convert.ToBoolean(oPrimeEDIHelper.CreatePO(ref purchaseOrder)); }

        public Dictionary<long, PurcahseOrderDO> GetPOStatus(ref Dictionary<long, string> primePoStatus) { return oPrimeEDIHelper.GetPOStatus(ref primePoStatus); }

        #region Extra Interface methods 
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public PurcahseOrderDO[] GetPODetails(long[] poIds, POStatus poStatus)
        {
            throw new NotImplementedException();
        }

        public PurcahseOrderDO[] GetPODetails(DTO_PO poNumber, string VendorCode)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSynchronize(PurcahseOrderDO purchaseOrder)
        {
            return oPrimeEDIHelper.UpdateSynchronize(purchaseOrder);
        }

        public bool UpdateSynchronize(int PrimeEDIOrderID, bool syncStatus)
        {
            throw new NotImplementedException();
        }

        public PurcahseOrderDO[] GetPODetails(DateTime fromDate, POStatus poStatus)
        {
            throw new NotImplementedException();
        }

        public POStatus GetPOStatus(long poId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    public class PrimePOUtilVendor : IVendorInterface
    {
        PrimeEDIHelper oPrimeEDIHelper = new PrimeEDIHelper(POS_Core.Resources.Configuration.CPrimeEDISetting.HostAddress);  //PRIMEPOS-3167 07-Nov-2022 JY Modified

        public bool AddVendor(VendorDO objVendorDAO) { throw new NotImplementedException(); }

        public bool DeleteVendor(VendorDO objVendorDAO) { throw new NotImplementedException(); }

        public VendorDO[] FetchVendorData() { return oPrimeEDIHelper.FetchVendorData(); }

        public VendorDO FetchVendorData(string vendorCode) { throw new NotImplementedException(); }

        public VendorDO FetchVendorData(int vendorID) { return oPrimeEDIHelper.FetchVendorData(vendorID); }

        #region Extra Interface methods 
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public bool UpdateVendor(VendorDO objVendorDAO)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class PrimePOEDIHelper
    {
        PrimeEDIHelper oPrimeEDIHelper = new PrimeEDIHelper(POS_Core.Resources.Configuration.CPrimeEDISetting.HostAddress); //PRIMEPOS-3167 07-Nov-2022 JY Modified
        public bool IsServerRunning() { return oPrimeEDIHelper.IsServerRunning(); }
    }
    #endregion
}