using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
using POS_Core.Data_Tier;
using POS_Core.Resources.DelegateHandler;
using POS_Core.TransType;
using PharmData;

namespace POS_Core.LabelHandler
{
    public enum EngineType
    {
        None,
        VBScript,
        JScript,
    };

    public class ScriptableLabel : IVsaSite, IDisposable, IJSVsaSite
    {
        private IJSVsaEngine _engine;
        public delegate void PrintL();
        public event PrintL PrintCustLabel;
        public delegate void PrintCC();
        public event PrintCC PrintCCReceipt;
        //Added By shitaljit 0n 12 march 2012 for dual receipt printing of  check  payment.
        public delegate void PrintCheck();
        public event PrintCheck PrintCheckReceipt;
        public object mRxLabel;
        private static string code;

        public ScriptableLabel()
        {
            //
            // TODO: Add constructor logic here
            return;
            //
        }

        public void PrintCLabel()
        {
            if (PrintCustLabel != null) PrintCustLabel();
        }

        public void PrintCCRec()
        {
            if (PrintCCReceipt != null) PrintCCReceipt();
        }

        //Added By shitaljit 0n 12 march 2012 for dual receipt printing of  check  payment.
        public void PrintCheckRec()
        {
            if (PrintCheckReceipt != null) PrintCheckReceipt();
        }

        public ScriptableLabel(object rxLabel, string scriptFileName)
        {
            this.mRxLabel = rxLabel;

            EngineType et = EngineType.None;
            string ext = Path.GetExtension(scriptFileName).ToLower();
            switch (ext)
            {
                case ".js":
                    et = EngineType.JScript;
                    break;

                case ".vb":
                    et = EngineType.VBScript;
                    break;

                default:
                    throw new ApplicationException("Unknown script file extension: " + ext);
            }

            // Load the file

            //			if (System.IO.File.GetLastWriteTime(scriptFileName) > LastWriteDate)
            //			{
            //				LastWriteDate = System.IO.File.GetLastWriteTime(scriptFileName);
            StreamReader reader = new StreamReader(scriptFileName);
            code = reader.ReadToEnd();
            reader.Close();
            //			}

            // Load the engine

            string st0 = DateTime.Now.Millisecond.ToString();

            switch (et)
            {
                case EngineType.JScript:
                    _engine = new Microsoft.JScript.Vsa.VsaEngine();
                    break;

                case EngineType.VBScript:
                    //_engine = new Microsoft.VisualBasic.Vsa.VsaEngine();
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            _engine.RootMoniker = "Phw://Label/";
            _engine.Site = this;
            _engine.InitNew();
            _engine.RootNamespace = "ScriptableLabel";

            // Turn on debugging
            _engine.GenerateDebugInfo = true;
            /* NOTE: This crashes under RC3
            if( et == EngineType.JScript )
            {
                Trace.WriteLine("JS debug directory= " + _engine.GetOption("DebugDirectory").ToString());
            }
            */

            // Load the core support assemblies
            IJSVsaItems items = _engine.Items;
            IJSVsaReferenceItem refItem;

            // system.dll
            refItem = (IJSVsaReferenceItem)items.CreateItem("system.dll", JSVsaItemType.Reference, JSVsaItemFlag.None);
            refItem.AssemblyName = "system.dll";

            // mscorlib.dll
            refItem = (IJSVsaReferenceItem)items.CreateItem("mscorlib.dll", JSVsaItemType.Reference, JSVsaItemFlag.None);
            refItem.AssemblyName = "mscorlib.dll";

            // Add System.Windows.Forms.dll
            refItem = (IJSVsaReferenceItem)items.CreateItem("WinForms",
                JSVsaItemType.Reference,
                JSVsaItemFlag.None);
            refItem.AssemblyName = "System.Windows.Forms.dll";

            // Add System.Drawing.dll
            refItem = (IJSVsaReferenceItem)items.CreateItem("SystemDrawing",
                JSVsaItemType.Reference,
                JSVsaItemFlag.None);
            refItem.AssemblyName = "System.Drawing.dll";

            // this assembly
            // NOTE: VB doesn't support EXEs as sources of types
            string assemName = Assembly.GetExecutingAssembly().Location;
            refItem = (IJSVsaReferenceItem)items.CreateItem(assemName,
                 JSVsaItemType.Reference,
                JSVsaItemFlag.None);
            refItem.AssemblyName = assemName;

            // Load the script code
            // NOTE: For VB, the name of the item, "Script", must match the name of the Module,
            //       or adding global items won't work <sigh>
            IJSVsaCodeItem codeItem = (IJSVsaCodeItem)items.CreateItem("Script",
                 JSVsaItemType.Code,
                 JSVsaItemFlag.None);
            codeItem.SourceText = code;

            // NOTE: JS doesn't support event sources
            if (et != EngineType.JScript)
            {
                // Add the "Clock" event source
                codeItem.AddEventSource("Label", "ClockBox.ScriptableLabel");
            }
            // NOTE: VB gets confused if we add an event source and a global object with the same name.
            // Luckily, you can access members of an event source in VB.
            else
            {
                // Add the global Label item
                IJSVsaGlobalItem lbl = (IJSVsaGlobalItem)items.CreateItem("Label",
                      JSVsaItemType.AppGlobal,
                    JSVsaItemFlag.None);
                lbl.TypeString = "POS_Core.LabelHandler.ScriptableLabel";

                IJSVsaGlobalItem rxL = (IJSVsaGlobalItem)items.CreateItem("RXL",
                      JSVsaItemType.AppGlobal,
                    JSVsaItemFlag.None);
                rxL.TypeString = "POS_Core.LabelHandler.RxLabel";

                string PhassemName = Assembly.GetExecutingAssembly().Location;

                IJSVsaGlobalItem Phw = (IJSVsaGlobalItem)items.CreateItem("Phw",
                      JSVsaItemType.AppGlobal,
                    JSVsaItemFlag.None);
                Phw.TypeString = PhassemName;
            }

            // Run the script

            string st1 = DateTime.Now.Millisecond.ToString();
            //_engine.Run();
            _engine.Compile();

            string st2 = DateTime.Now.Millisecond.ToString();
            //clsUIHelper.ShowErrorMsg(st0+"\n"+st1+"\n"+st2,"Compile");

            _engine.Run();
            // Call the VB entry point
            if (et == EngineType.VBScript)
            {
                // Execute ClockScript.Script.Main()
                Assembly assem = _engine.Assembly;
                Type type = assem.GetType("ClockScript.Script");
                MethodInfo method = type.GetMethod("Main");
                method.Invoke(null, null);
            }
        }

        // IVsaSite
        void IVsaSite.GetCompiledState(out Byte[] pe, out Byte[] debugInfo)
        {
            Trace.WriteLine("IVsaSite.GetCompiledState()");
            pe = debugInfo = null;
        }

        object IVsaSite.GetEventSourceInstance(string itemName, string eventSourceName)
        {
            Trace.WriteLine(string.Format("IVsaSite.GetEventSourceInstance('{0}', '{1}')", itemName, eventSourceName));
            switch (eventSourceName)
            {
                case "Label": return this;
                // TODO: Throw VsaError.EventSourceInvalid instead
                default: return null;
            }
        }

        object IVsaSite.GetGlobalInstance(string name)
        {
            Trace.WriteLine("IVsaSite.GetGlobalInstance('" + name + "')");
            switch (name)
            {
                case "Label": return this;
                // TODO: Thrown VsaError.GlobalInstanceInvalid instead
                case "RXL": return this.mRxLabel;
                default: return null;
            }
        }

        void IVsaSite.Notify(string notify, object info)
        {
        }

        bool IVsaSite.OnCompilerError(IVsaError e)
        {
            clsCoreUIHelper.ShowErrorMsg(String.Format("Error of severity {0} on line {1}: {2} \n {3}", e.Severity, e.Line, e.Description, e.LineText));
            return true; // Continue to report errors
        }

        // IDisposable
        void IDisposable.Dispose()
        {
            // Close the engine
            if (_engine != null) _engine.Close();
        }

        #region IJSVsaSite Members

        void IJSVsaSite.GetCompiledState(out byte[] pe, out byte[] debugInfo)
        {
            Trace.WriteLine("IVsaSite.GetCompiledState()");
            pe = debugInfo = null;
        }

        object IJSVsaSite.GetEventSourceInstance(string itemName, string eventSourceName)
        {
            Trace.WriteLine(string.Format("IVsaSite.GetEventSourceInstance('{0}', '{1}')", itemName, eventSourceName));
            switch (eventSourceName)
            {
                case "Label": return this;
                // TODO: Throw VsaError.EventSourceInvalid instead
                default: return null;
            }
        }

        object IJSVsaSite.GetGlobalInstance(string name)
        {
            Trace.WriteLine("IVsaSite.GetGlobalInstance('" + name + "')");
            switch (name)
            {
                case "Label": return this;
                // TODO: Thrown VsaError.GlobalInstanceInvalid instead
                case "RXL": return this.mRxLabel;
                default: return null;
            }
        }

        void IJSVsaSite.Notify(string notify, object info)
        {
        }

        public bool OnCompilerError(IJSVsaError error)
        {
            MessageBox.Show(String.Format("Error of severity {0} on line {1}: {2}", error.Severity, error.Line, error.Description));
            return true; // Continue to report errors
        }

        #endregion
    }
}

namespace POS_Core.LabelHandler.RxLabel
{
    /// <summary>
    /// Summary description for DirectPrint.
    /// </summary>

    public enum PrintDestination
    {
        Printer,
        Screen,
        File
    }
    public enum ReceiptType
    {
        SalesTransaction = 1,
        ReturnTransaction = 2,
        ReceiveOnAccount = 3,
        OnHoldTransction = 4,
        SalesTransactionReprint = 5,
        Void = 6
    }
    public class RxLabel
    {
        #region Variable Declaration
        //public DataTable dtTax;//2664
        public bool bPrintGiftReciept = false;//Added by shitaljit for printing gift receipt in the transaction.
        private bool isOnHoldTrans = false;
        protected TransHeaderData oTHeaderData;
        protected TransDetailData oTDetailData;
        protected POSTransPaymentData oPaymentData;
        //added by atul 07-jan-2011
        protected TransDetailRXData oTRxDetailData;
        //End of added by atul 07-jan-2011
        public PayOutRow oPayoutRow;
        private CustomerRow oCustomerRow = null;
        //Added By Shialjit(QuicSolv) 27 May 2011
        CLCardsRow oCLCardsRow = null;
        //Added on 4 oct 2011
        public string PatientName = "";
        public string PatientAddress = "";
        public string PatientPhoneNo = "";
        public DataTable PatientTable { get; set; }
        //END of Added on 4 oct 2011
        //Till here added by shitaljit

        public int lnX = 0;
        public int lnY = 0;

        public int PageWidth = 0;

        protected PrintDestination mPrintDest = PrintDestination.Printer;
        protected string PrinterName;
        protected string PaperSource;
        protected string CounsType = " ";
        protected bool InsertBlankLine = false;
        public System.Drawing.Font CFont;
        public System.Drawing.Brush CBrush;

        protected float CSize;
        protected PrintDocument mPD;

        public System.Drawing.Font Courier;
        public System.Drawing.Font Arial;
        public System.Drawing.Font MSSanSerif;
        public System.Drawing.Font Times;
        public System.Drawing.Font Verdana;
        public string CStoreName;
        public string CAddress;
        public string CCity;
        public string CState;
        public string CZip;
        public string CTelephone;
        public string CReceiptMSG;
        public string CMerchantNo;
        public string Barcode;

        public string TransNo;
        public string TransDate;
        public string TransTime;
        public string TransUserID;

        public string Qty;
        public string ItemName;
        public string ExAmount;

        public Decimal SubTotal;
        public Decimal Discount;
        public string Tax;
        public DateTime AmountDue;
        public string CLProgramName;
        public bool CLPrintCopoun;
        public bool PrintCLCouponSeparately;   //Sprint-18 - 2039 30-Oct-2014 JY CHanged CLPrintCopounWithReceipt to PrintCLCouponSeparately
        public bool PrintCLCouponOnlyIfTierIsReached;   //Sprint-18 - 2039 01-Dec-2014 JY Added to print CL coupon only if tier is reached
        public bool CLPrintMsgOnReceipt;
        public string CLMessage;

        protected bool PrintPHName = false;
        private string sLabelFile;
        public PrintPageEventArgs PR;

        private int mCopies = 1;
        private System.IntPtr lhPrinter;
        private POSTransPaymentRow oCCPaymentRow = null;
        private POSTransPaymentRow oCBPaymentRow = null;
        private POSTransPaymentRow oHCPaymentRow = null;
        private POSTransPaymentRow oCouponPaymentRow = null;
        private POSTransPaymentRow oEBTPaymentRow = null;
        private POSTransPaymentRow oCheckPaymentRow = null;
        private POSTransPaymentRow oCashPaymentRow = null;
        private POSTransPaymentRow oAthPaymentRow = null;//2785
        private bool isFSATranaction = false;
        private decimal totalFSAAmount = 0.0M;
        public string AccCode = "";
        public string AccName = "";
        public string HCReference = ""; //PRIMEPOS-3471
        public Decimal AccCurrBalance;
        public Decimal AccAmount;
        public bool MergeCCWithRecpt = false;
        public bool isCheckPayment = false;
        public bool isCLTierreached = false;
        public decimal CLCouponValue = 0;  //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value 

        public DataTable dtTax = new DataTable();//2664
        public Image CouponBarCode = null;
        public CLCouponsRow CouponRow = null;

        //added by akbar
        private PharmData.PharmBL mPharmData;
        private DataTable tblPatient = null;
        //
        private bool isCashPayment = false;
        private static int SignatureIndex = 0;
        private static int nTransPayID = 0;  //PRIMEPOS-2939 03-Mar-2021 JY Added

        //IVU Lotto Objects
        POS_Core.IVULottoService.ivuLotoData oivuLotoData = new POS_Core.IVULottoService.ivuLotoData();
        POS_Core.IVULottoService.txInfoRequest otxInfoRequest = new POS_Core.IVULottoService.txInfoRequest();
        POS_Core.IVULottoService.TxServerService oTxServerService = new POS_Core.IVULottoService.TxServerService();
        POS_Core.IVULottoService.transaction oivuLottotrans = new POS_Core.IVULottoService.transaction();
        Decimal StateTax = 0;
        Decimal LocalTax = 0;
        Decimal FederalTax = 0;
        Decimal CountyTax = 0;
        Decimal CityTax = 0;
        Decimal MunicipalityTax = 0;
        //End
        public TransDetailTaxData _oTDTaxData = new TransDetailTaxData();
        ReceiptType oReceiptType = new ReceiptType();
        public bool PrintReceiptInMultipleLanguage;   //Sprint-21 - 1272 28-Aug-2015 JY Added
        public DataRow OtherLanguageDesc = null;  //Sprint-21 - 1272 28-Aug-2015 JY Added
        public int OtherLanguageDescRowCount = 0;   //Sprint-21 - 1272 31-Aug-2015 JY Added
        public DataTable dtTransDetailTax;  //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added new parameter to collect tax and did related changes in code
        public bool bPrintDuplicateReceipt = false;    //PRIMEPOS-2647 20-Mar-2019 JY Added

        #region Evertec - Arvind - 16-july-2019 PRIMEPOS-2664
        public string PaymentProcessor = string.Empty;
        public string TerminalID = string.Empty;
        public string Batch = string.Empty;
        public string MerchantID = string.Empty;
        public string AuthNo = string.Empty;
        public string Trace = string.Empty;
        public string ReferenceNumber = string.Empty;
        public string InvoiceNumber = string.Empty;
        public string ControlNumber = string.Empty;//PRIMEPOS-2664
        public bool isEBTBalance = false;//PRIMEPOS-2786
        public bool isDenialReceipt = false;//PRIMEPOS-2785
        public string FoodBalance = string.Empty;//PRIMEPOS-2786
        public string CashBalance = string.Empty;//PRIMEPOS-2786

        #region PRIMEPOS-2785 EVERTEC
        public string ResultDescription = string.Empty;
        public DateTime EvertecDenialDate;
        public string Amount = string.Empty;
        public string TransactionID = string.Empty;
        #endregion

        #endregion
        
        #region VANTIV - Arvind - 16-july-2019  PRIMEPOS-2636      
        public string ResponseCode = string.Empty;
        public string Aid = string.Empty;
        public string Cryptogram = string.Empty;
        public string EntryMethod = string.Empty;
        public string ApprovalCode = string.Empty;
        #endregion
        #region PRIMEPOS-2793 
        public string ApplicationLabel = string.Empty;
        public string TC = string.Empty;//2943
        public string AAC = string.Empty;//2943
        public string IAD = string.Empty;
        public bool PinVerified = false;
        public string LaneID = string.Empty;
        public string CardLogo = string.Empty;
        #endregion

        public string TVR = string.Empty;

        private string tmpPaymentProcessor = string.Empty;  //PRIMEPOS-2876 27-Jul-2020 JY Added
        public string TicketNumber = string.Empty;  //PRIMEPOS-2892 24-Sep-2020 JY Added
        
        #region primepos-2831
        public bool IsEvertecForceTransaction = false;
        public bool IsEvertecSign = false;
        public string EmvTag = string.Empty;//PRIMEPOS-2857
        public string EvertecCityTax = string.Empty;//PRIMEPOS-2857
        public string EvertecStateTax = string.Empty;//PRIMEPOS-2857
        public string EvertecCashback = string.Empty;//PRIMEPOS-2857        
        public string ATHMovil = string.Empty;//2664
        #endregion                
        public bool isVoidReceipt = false;//PRIMEPOS-2664
        public string CardName = string.Empty;//PRIMEPOS-2664
        public string State = string.Empty;//PRIMEPOS-2664
        public string City = string.Empty;//PRIMEPOS-2664
        public string ReduceState = string.Empty;//PRIMEPOS-2664
        public string Municipal = string.Empty;//PRIMEPOS-2664
        public string TotalAmt = string.Empty;//PRIMEPOS-2664
        public string BaseAmt = string.Empty;//PRIMEPOS-2664
        public string CardNumber = string.Empty;//PRIMEPOS-2664        
        #endregion

        public PharmData.PharmBL PharmData()
        {
            if (mPharmData == null)
                mPharmData = new PharmData.PharmBL();

            return mPharmData;
        }




        private DataTable PatientTbl(long Patientno)
        {
            bool bGetpatient = false;
            if (tblPatient == null)
                bGetpatient = true;
            else
            {
                if (tblPatient.Rows.Count > 0 && MMSUtil.UtilFunc.ValorZeroL(tblPatient.Rows[0]["Patientno"].ToString()) == Patientno)
                    bGetpatient = false;
                else
                    bGetpatient = true;
            }
            if (bGetpatient)
            {
                tblPatient = this.PharmData().GetPatient(Patientno.ToString());
            }
            return tblPatient;
        }

        //

        public string GetStationName(string StationID)
        {
            return Configuration.GetStationName(StationID);
        }

        public string Replicate(string Data, int Len)
        {
            string str = Data;
            for (int i = 1; i <= Len; i++)
                str += Data;
            return str;
        }

        public HouseChargeAccount HouseChargeInfo;

        public string UserName
        {
            get { return Configuration.UserName; }
        }

        public TransDetailTaxData oTDTaxData
        {
            get { return _oTDTaxData; }
        }
        public string FLen(string Data, int lLen)
        {
            /*string str;
            str=Data;
            for(int i=1; i<=(Len-Data.Length);i++)
                str+=" ";
            return str;
            This is old logic
            */

            //New logic as per primepos
            if (Data == null)
            {
                return Space(lLen);
            } else
            {
                if (Data.Length >= lLen)
                    return Data.Substring(0, lLen);
                else
                {
                    string sD = Data + Space(lLen);
                    return sD.Substring(0, lLen);
                }
            }
        }

        public bool IsNullOrWhiteSpace(string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public string Space(int Spaces)
        {
            string str = "";
            for (int i = 1; i <= Spaces; i++)
                str += " ";
            return str;
        }

        public bool IsOnHoldTrans
        {
            get
            {
                return this.isOnHoldTrans;
            }
        }

        public bool IsDuplicatePrint
        {
            get
            {
                if (oReceiptType == ReceiptType.SalesTransactionReprint)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        //Added By shitaljit for printing gift receipt
        public bool PrintGiftReciept
        {
            get
            {
                return this.bPrintGiftReciept;
            }
        }

        public decimal TotalFSAAmount
        {
            get
            {
                return this.totalFSAAmount;
            }
        }

        public bool IsFSATransaction
        {
            get
            {
                return this.isFSATranaction;
            }
        }


        public RxLabel(TransHeaderData oHData, TransDetailData ODData, POSTransPaymentData oPData, ReceiptType oRcptType, DataTable dtTransDetailTax, bool bPrintDupReceipt = false)
            : this(oHData, ODData, oPData, false, oRcptType, dtTransDetailTax)
        {
            this.oReceiptType = oRcptType;
            this.bPrintDuplicateReceipt = bPrintDupReceipt;
            if (bPrintDupReceipt) tmpPaymentProcessor = GetPaymentProcessor(this.oPaymentData);    //PRIMEPOS-2876 27-Jul-2020 JY Added
        }

        public RxLabel()
        {
            InitClass();
        }
        /// <summary>
        /// Author:Shitaljit
        /// Aded By Shitaljit to get current receipt that is printing.
        /// </summary>
        public ReceiptType TrnansReceiptType
        {
            get
            {
                try
                { return oReceiptType; }

                catch (Exception)
                {

                    return ReceiptType.Void;
                }
            }
        }
        public RxLabel(TransHeaderData oHData, TransDetailData ODData, POSTransPaymentData oPData, bool isOnHTrans, ReceiptType oRcptType, DataTable dtTransDetailTax)
        {
            this.oTHeaderData = oHData;
            this.oTDetailData = ODData;
            UpdateItemDescInOL();   //Sprint-21 - 1272 17-Aug-2015 JY Added
            this.oPaymentData = oPData;
            this.isOnHoldTrans = isOnHTrans;
            this.oReceiptType = oRcptType;
            this.dtTransDetailTax = dtTransDetailTax;   //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
            //
            // TODO: Add constructor logic here
            //
            InitClass();
        }

        #region Sprint-21 - 1272 17-Aug-2015 JY Added
        private void UpdateItemDescInOL()
        {
            try
            {
                if (oTDetailData.Tables.Count > 0 && oTDetailData.Tables[0].Rows.Count > 0)
                {
                    string strItemIds = string.Empty;
                    for (int j = 0; j < oTDetailData.Tables[0].Rows.Count; j++)
                    {
                        if (strItemIds == string.Empty)
                            strItemIds = "'" + oTDetailData.Tables[0].Rows[j][clsPOSDBConstants.TransDetail_Fld_ItemID].ToString().Replace("'", "''").Trim() + "'";
                        else
                            strItemIds += ",'" + oTDetailData.Tables[0].Rows[j][clsPOSDBConstants.TransDetail_Fld_ItemID].ToString().Replace("'", "''").Trim() + "'";
                    }

                    DataSet ds = null;
                    using (ItemDescriptionSvr dao = new ItemDescriptionSvr())
                    {
                        ds = dao.GetItemDescriptionInOL(strItemIds, Configuration.convertNullToInt(oTHeaderData.Tables[0].Rows[0][clsPOSDBConstants.TransHeader_Fld_CustomerID]));
                    }

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < oTDetailData.Tables[0].Rows.Count; j++)
                            {
                                if (oTDetailData.Tables[0].Rows[j][clsPOSDBConstants.TransDetail_Fld_ItemID].ToString().Trim().ToUpper() == ds.Tables[0].Rows[i][clsPOSDBConstants.TransDetail_Fld_ItemID].ToString().Trim().ToUpper())
                                {
                                    oTDetailData.Tables[0].Rows[j]["ItemDescriptionInOL"] = ds.Tables[0].Rows[i]["ItemDescriptionInOL"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            { 
            }
        }
        #endregion


        //added by atul 07-jan-2011
        public RxLabel(TransHeaderData oHData, TransDetailData ODData, POSTransPaymentData oPData, TransDetailRXData oDRxData, ReceiptType oRcptType, DataTable dtTransDetailTax, Boolean bPrintDupReceipt = false, DataTable patTable = null)
            : this(oHData, ODData, oPData, oDRxData, false, oRcptType, dtTransDetailTax, patTable)
        {
            this.PatientTable = patTable;
            this.oReceiptType = oRcptType;
            this.bPrintDuplicateReceipt = bPrintDupReceipt;
            if (bPrintDupReceipt)   tmpPaymentProcessor = GetPaymentProcessor(this.oPaymentData);    //PRIMEPOS-2876 27-Jul-2020 JY Added
        }
        //End of added by atul 07-jan-2011

        //added by atul 07-jan-2011
        public RxLabel(TransHeaderData oHData, TransDetailData ODData, POSTransPaymentData oPData, TransDetailRXData oDRxData, bool isOnHTrans, ReceiptType oRcptType, DataTable dtTransDetailTax, DataTable patTable=null)
        {
            this.oTHeaderData = oHData;
            this.oTDetailData = ODData;
            this.oPaymentData = oPData;
            this.oTRxDetailData = oDRxData;
            this.isOnHoldTrans = isOnHTrans;
            this.oReceiptType = oRcptType;
            this.dtTransDetailTax = dtTransDetailTax;   //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
            this.PatientTable = patTable;
            //
            // TODO: Add constructor logic here
            //
            InitClass();
        }

        //end of added by atul 07-jan-2011

        #region PRIMEPOS-2884 21-Aug-2020 JY Added
        public void SetDescForTransData(TransDetailData oTransDData)
        {
            foreach (TransDetailRow oRow in oTransDData.Tables[0].Rows)
            {
                string strCat = "";
                if (oRow.IsIIAS == true && Configuration.CInfo.TagFSA == true)
                {
                    strCat = "F";
                }
                //Added oRow.TaxAmount > 0 by shitaljit to Tag item as taxable only if there is Tax amount charged
                //Sprint-22 - PRIMEPOS-1704 04-Dec-2015 JY removed oRow.TaxID > 0 condition as this is PosTransactionDetail.TaxId and now we are not using it
                if (oRow.TaxAmount > 0 && Configuration.CInfo.TagTaxable == true)
                {
                    strCat += "T";
                }
                if (oRow.IsMonitored == true && Configuration.CInfo.TagMonitored == true)
                {
                    strCat += "M";
                }
                if (oRow.IsEBTItem == true && Configuration.CInfo.TagEBT == true)
                {
                    strCat += "E";
                }
                if (Configuration.CSetting.TagSolutran == true && Configuration.convertNullToInt(oRow.S3TransID) > 0)   //PRIMEPOS-2836 21-Apr-2020 JY Added
                {
                    strCat += "S";
                }

                if (strCat != "")
                    oRow.ItemDescription = "(" + strCat + ") " + oRow.ItemDescription;
            }
        }
        #endregion

        public RxLabel(PayOutRow oPayout)
        {
            this.oPayoutRow = oPayout;
            //
            // TODO: Add constructor logic here
            //
            InitClass();
        }

        public TransHeaderRow TransH(int row)
        {
            try
            {
                return (TransHeaderRow)this.oTHeaderData.TransHeader.Rows[row];
            }
            catch
            { return null; }
        }

        public TransDetailRow TransD(int row)
        {
            try
            {
                TransDetailRow oRow = (TransDetailRow)this.oTDetailData.TransDetail.Rows[row];
                if(Configuration.CPOSSet.PrintRXDescription == false)
                {
                    if (oRow.ItemID.Trim().ToUpper() == "RX")
                    {
                        if (!string.IsNullOrWhiteSpace(oRow.ItemDescription))
                        {
                            try
                            {
                                int index = oRow.ItemDescription.IndexOf('-', oRow.ItemDescription.IndexOf('-') + 1);
                                if (index > 0)
                                    oRow.ItemDescription = oRow.ItemDescription.Substring(0, index);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                return oRow;
            }
            catch
            { return null; }
        }

        #region Sprint-23 - PRIMEPOS-2319 23-Jun-2016 JY Added rxcount
        public TransDetailRXRow TransRxDetail(int row)
        {
            TransDetailRXRow oRow = null;
            try
            {
                if (this.oTRxDetailData != null && this.oTRxDetailData.Tables[0].Rows.Count > 0)
                    oRow = (TransDetailRXRow)this.oTRxDetailData.TransDetailRX.Rows[row];
                return oRow;
            }
            catch
            { return null; }
        }
        #endregion

        //added by atul 07-jan-2011
        public TransDetailRXRow TransRxd(int row)
        {
            try
            {
                TransDetailRow oRow = (TransDetailRow)this.oTDetailData.TransDetail.Rows[row];
                TransDetailRXRow oRowRx = (TransDetailRXRow)this.oTRxDetailData.TransDetailRX.Rows[row];

                if(oRow.ItemID.Trim().ToUpper() == "RX")
                {
                    return oRowRx;
                } else
                {
                    return null;
                }
            } catch
            { return null; }
        }

        //End of added by atul 07-jan-2011

        public int TransDCount
        {
            get { return this.oTDetailData.TransDetail.Rows.Count; }
        }

        public int TransDetailTaxCount
        {
            get
            {
                int nRowCnt = 0;
                if (this.dtTransDetailTax != null && this.dtTransDetailTax.Rows.Count > 0)
                    nRowCnt = this.dtTransDetailTax.Rows.Count;
                return nRowCnt;
            }
        }

        #region Sprint-23 - PRIMEPOS-2319 23-Jun-2016 JY Added rxcount
        public int RxDetailCount
        {
            get
            {
                if (this.oTRxDetailData != null && this.oTRxDetailData.Tables[0].Rows.Count > 0)
                    return this.oTRxDetailData.TransDetailRX.Rows.Count;
                else
                    return 0;
            }
        }
        #endregion

        public POSTransPaymentRow TransPaymentRow(int row)
        {
            try
            {
                return (POSTransPaymentRow)this.oPaymentData.POSTransPayment.Rows[row];
            } catch
            { return null; }
        }

        public POSTransPaymentRow CCPaymentRow
        {
            get
            {
                try
                {
                    return oCCPaymentRow;
                } catch
                { return null; }
            }
        }

        public POSTransPaymentRow CashPaymentRow
        {
            get
            {
                try
                {
                    return oCashPaymentRow;
                } catch
                { return null; }
            }
        }

        public POSTransPaymentRow CheckPaymentRow
        {
            get
            {
                try
                {
                    return oCheckPaymentRow;
                } catch
                { return null; }
            }
        }

        public POSTransPaymentRow CouponPaymentRow
        {
            get
            {
                try
                {
                    return oCouponPaymentRow;
                } catch
                { return null; }
            }
        }

        public POSTransPaymentRow EBTPaymentRow
        {
            get
            {
                try
                {
                    return oEBTPaymentRow;
                } catch
                { return null; }
            }
        }

        public POSTransPaymentRow HCPaymentRow
        {
            get
            {
                try
                {
                    return oHCPaymentRow;
                } catch
                { return null; }
            }
        }

        public POSTransPaymentRow CBPaymentRow
        {
            get
            {
                try
                {
                    return oCBPaymentRow;
                } catch
                { return null; }
            }
        }

        public POSTransPaymentRow ATHPaymentRow//2785
        {
            get
            {
                try
                {
                    return oAthPaymentRow;
                }
                catch
                { return null; }
            }
        }

        public int PaymentRowCount
        {
            get { return this.oPaymentData.POSTransPayment.Rows.Count; }
        }

        public int TotalQty
        {
            get
            {
                int qty = 0;
                if (this.oTDetailData != null)
                {
                    foreach (TransDetailRow orow in this.oTDetailData.TransDetail.Rows)
                    {
                        qty = qty + orow.QTY;
                    }
                }
                return qty;
            }
        }

        public PrintDestination PDestination
        {
            set { this.mPrintDest = value; }
            get { return mPrintDest; }
        }

        public int CopiesToPrint
        {
            set { this.mCopies = value; }
            get { return mCopies; }
        }

        public String TransactionType
        {
            get
            {
                if (oTHeaderData.TransHeader[0].TransType.ToString().Trim() == "1")
                {
                    return "Sales Transaction";
                } else if (oTHeaderData.TransHeader[0].TransType.ToString().Trim() == "2")
                {
                    return "Return Transaction";
                } else if (oTHeaderData.TransHeader[0].TransType.ToString().Trim() == "3")
                {
                    return "Received On Account";
                } else
                {
                    return "";
                }
            }
        }

        public int TransactionTypeCode
        {
            get
            {
                return oTHeaderData.TransHeader[0].TransType;
            }
        }

        public CustomerRow CustomerInfo
        {
            get { return oCustomerRow; }
        }

        //Added By Shitaljit(QuicSolv) on 27 May 2011
        public CLCardsRow CLCardInfo
        {
            get { return oCLCardsRow; }
        }

        //Added By Shitaljit(QuicSolv) on 15 Sept 2011      //Modify by Manoj 9/22/2011
        public string CardBalance
        {
            get { return PccPaymentSvr.CCBalance; }
        }

        //Till here added by Shitaljit

        public Image GetTransIDBarcode()
        {
            Image oImage = null;
            string sBarcode = string.Empty;
            try
            {
                string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), oTHeaderData.TransHeader[0].TransID.ToString() + DateTime.Now.ToString("_MMddyyyy_HHmmss") + ".bmp");

                Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";

                if (oTHeaderData.TransHeader[0].WasonHold)  //PRIMEPOS-3447
                    sBarcode = "~" + oTHeaderData.TransHeader[0].TransID.ToString();
                else
                    //added '?' in barcode by shitaljit so that users can directly dump it and call the transaction from POSTransaction screen.
                    sBarcode = "?" + oTHeaderData.TransHeader[0].TransID.ToString();

                if (System.IO.File.Exists(sImagePath) == true)
                {
                    System.IO.File.Delete(sImagePath);
                }

                Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);
                {
                    using (Stream stream = new FileStream(sImagePath, FileMode.Open))
                    {
                        oImage = Image.FromStream(stream);
                        stream.Close();
                    }
                }
            } catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            } finally
            {
                /*(if (oImage != null)
                {
                    oImage.Dispose();
                }*/
            }
            return oImage;
        }

        #region Sprint-27 - PRIMEPOS-2447 04-Sep-2017 JY Added
        public Image GetRxNoBarcode(string sRxNo)
        {
            Image oImage = null;
            if (this.oTRxDetailData != null && this.oTRxDetailData.Tables[0].Rows.Count > 0)
            {
                try
                {
                    string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), sRxNo + DateTime.Now.ToString("_MMddyyyy_HHmmss") + ".bmp");
                    Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
                    string sBarcode = sRxNo;
                    if (System.IO.File.Exists(sImagePath) == true)
                    {
                        System.IO.File.Delete(sImagePath);
                    }
                    Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);
                    {
                        using (Stream stream = new FileStream(sImagePath, FileMode.Open))
                        {
                            oImage = Image.FromStream(stream);
                            stream.Close();
                        }
                    }
                } catch (Exception exp)
                {
                    clsCoreUIHelper.ShowErrorMsg(exp.Message);
                }
            }
            return oImage;
        }

        public Image GetRxNoBarcode(string sRxNo, string separator, string NRefill)
        {
            Image oImage = null;
            if (this.oTRxDetailData != null && this.oTRxDetailData.Tables[0].Rows.Count > 0)
            {
                try
                {
                    string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), sRxNo + NRefill + DateTime.Now.ToString("_MMddyyyy_HHmmss") + ".bmp");
                    Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";
                    string sBarcode = string.Empty;

                    if (separator == "")
                        sBarcode = sRxNo + "-" + NRefill;
                    else
                        sBarcode = sRxNo + separator + NRefill;

                    if (System.IO.File.Exists(sImagePath) == true)
                    {
                        System.IO.File.Delete(sImagePath);
                    }
                    Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);
                    {
                        using (Stream stream = new FileStream(sImagePath, FileMode.Open))
                        {
                            oImage = Image.FromStream(stream);
                            stream.Close();
                        }
                    }
                } catch (Exception exp)
                {
                    clsCoreUIHelper.ShowErrorMsg(exp.Message);
                }
            }
            return oImage;
        }
        #endregion

        /// <summary>
        /// Added by Amit Date 7 july 2011
        /// method to get barcode of vendoritemcode
        /// </summary>
        /// <param name="sBarcode"></param>
        /// <returns></returns>
        public Image GetVendorItemCodeBarcode(string sBarcode)
        {
            Image oImage = null;
            try
            {
                string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), oTHeaderData.TransHeader[0].TransID.ToString() + DateTime.Now.ToString("_MMddyyyy_HHmmss") + ".bmp");

                Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";

                if (System.IO.File.Exists(sImagePath) == true)
                {
                    System.IO.File.Delete(sImagePath);
                }

                Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);
                {
                    using (Stream stream = new FileStream(sImagePath, FileMode.Open))
                    {
                        oImage = Image.FromStream(stream);
                        stream.Close();
                    }
                }
            } catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            }

            return oImage;
        }

        //Added by Atul Joshi on 16 feb 2011 for printing transactionId with char '?'
        public Image GetTransIDBarcodewithChar()
        {
            Image oImage = null;
            try
            {
                string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), oTHeaderData.TransHeader[0].TransID.ToString() + DateTime.Now.ToString("_MMddyyyy_HHmmss1") + ".bmp");

                Mabry.Windows.Forms.Barcode.Licenser.Key = "E1P8-HKELVF8R04Q0";

                string sBarcode = "?" + oTHeaderData.TransHeader[0].TransID.ToString();

                if (System.IO.File.Exists(sImagePath) == true)
                {
                    System.IO.File.Delete(sImagePath);
                }

                Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);

                using (Stream stream = new FileStream(sImagePath, FileMode.Open))
                {
                    oImage = Image.FromStream(stream);
                    stream.Close();
                }
            } catch (Exception exp)
            {
                clsCoreUIHelper.ShowErrorMsg(exp.Message);
            } finally
            {
                /*(if (oImage != null)
                {
                    oImage.Dispose();
                }*/
            }
            return oImage;
        }

        // this routine looks up the oRxDetailData and returns all the patients in the transaction in the Patient table.
        // added by akbar
        public DataTable PatientInfo()
        {
            if (this.tblPatient != null)
                return this.tblPatient;

            DataTable dtPat = new DataTable();
            POSTransaction oposTrans = new POSTransaction();
            if (oTRxDetailData != null)
            {
                dtPat = oposTrans.PatientInfo(oTRxDetailData);
                this.tblPatient = dtPat;
            }
            return this.tblPatient;

            //Following Code is Commented By Shitaljit(QuicSolv) on 16 May 2011
            //if (oTRxDetailData != null && oTRxDetailData.TransDetailRX.Rows.Count > 0)
            //{
            //    string sPatno;
            //    DataTable tPat;
            //    bool bPatFound=false;

            //    foreach (DataRow dr in oTRxDetailData.TransDetailRX)
            //    {
            //        sPatno = dr["Patientno"].ToString();

            //        if (dtPat.Rows.Count > 0)
            //        {
            //            if (dtPat.Select("Patientno='" + sPatno + "'").Length == 0)
            //                bPatFound = false;
            //            else
            //                bPatFound = true;
            //        }
            //        else
            //            bPatFound=false;

            //        if (!bPatFound)
            //        {
            //            tPat = this.PatientTbl(MMSUtil.UtilFunc.ValorZeroL(sPatno));
            //            if (dtPat == null || dtPat.Rows.Count == 0)
            //                dtPat=tPat.Clone();
            //            if (tPat != null && tPat.Rows.Count > 0)
            //            {
            //                DataRow drp = tPat.Rows[0];
            //                dtPat.ImportRow(drp);
            //            }
            //        }
            //    }
            //}
            //this.tblPatient = dtPat;
            //return this.tblPatient;
        }

        private void InitClass()
        {
            Courier = new System.Drawing.Font("Courier", 10, System.Drawing.FontStyle.Regular);
            Arial = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
            MSSanSerif = new System.Drawing.Font("MS San Serif", 10, System.Drawing.FontStyle.Regular);
            Times = new System.Drawing.Font("Times New Roman", 10, System.Drawing.FontStyle.Regular);
            Verdana = new System.Drawing.Font("Verdana", 10, System.Drawing.FontStyle.Regular);
            this.CFont = Courier;
            this.CSize = 10;

            this.CBrush = new System.Drawing.SolidBrush(Color.Black);

            mPD = new PrintDocument();
            mPD.PrintController = new StandardPrintController();

            mPD.PrintPage += new PrintPageEventHandler(PrintLabel);

            this.CStoreName = Configuration.CInfo.StoreName;
            this.CAddress = Configuration.CInfo.Address;
            this.CCity = Configuration.CInfo.City;
            this.CState = Configuration.CInfo.State;
            this.CZip = Configuration.CInfo.Zip;
            this.CTelephone = Configuration.CInfo.Telephone;
            this.CReceiptMSG = Configuration.CInfo.ReceiptMSG;
            this.CMerchantNo = Configuration.CInfo.MerchantNo;
            this.CLMessage = Configuration.CLoyaltyInfo.Message;
            this.CLPrintCopoun = Configuration.CLoyaltyInfo.PrintCoupon;
            this.PrintCLCouponSeparately = Configuration.CLoyaltyInfo.PrintCLCouponSeparately;   //Sprint-18 - 2039 30-Oct-2014 JY CHanged CLPrintCopounWithReceipt to PrintCLCouponSeparately
            this.PrintCLCouponOnlyIfTierIsReached = Configuration.CLoyaltyInfo.PrintCLCouponOnlyIfTierIsReached;   //Sprint-18 - 2039 01-Dec-2014 JY Added to print CL coupon only if tier is reached
            this.CLPrintMsgOnReceipt = Configuration.CLoyaltyInfo.PrintMsgOnReceipt;
            this.CLProgramName = Configuration.CLoyaltyInfo.ProgramName;
            this.PrintReceiptInMultipleLanguage = Configuration.CInfo.PrintReceiptInMultipleLanguage;   //Sprint-21 - 1272 28-Aug-2015 JY Added

            if (oTHeaderData != null && oTHeaderData.TransHeader.Rows.Count > 0)
            {
                CustomerSvr oCustomerSvr = new CustomerSvr();
                oCustomerRow = oCustomerSvr.PopulateList(" Where CustomerID=" + oTHeaderData.TransHeader[0].CustomerID.ToString()).Customer[0];

                #region Sprint-21 - 1272 28-Aug-2015 JY Added
                try
                {
                    if (oCustomerRow.LanguageId > 0)
                    {
                        using (OtherLanguageDescSvr dao = new OtherLanguageDescSvr())
                        {
                            DataSet ds = dao.PopulateOtherLanguageDesc(Convert.ToInt64(oCustomerRow.LanguageId));
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                OtherLanguageDescRowCount = ds.Tables[0].Rows.Count;
                                OtherLanguageDesc = ds.Tables[0].Rows[0];
                            }
                        }
                    }
                } catch
                { }
                #endregion

                CLCoupons oCLCoupons = new CLCoupons();
                CLCouponsData oCLCouponsData = oCLCoupons.PopulateList(" Where CreatedInTransID=" + oTHeaderData.TransHeader[0].TransID.ToString());
                if (oCLCouponsData.CLCoupons.Rows.Count > 0)
                {
                    this.CouponRow = oCLCouponsData.CLCoupons[0];
                    GenerateCouponBarcode();
                }

                //Added By Shitaljit(QuicSolv) on 27 May 2011
                //This will populate Customer Loyality Card information for the selected customer
                try
                {
                    if (oCustomerRow.UseForCustomerLoyalty == true)
                    {
                        CLCards oCLCards = new CLCards();
                        oCLCardsRow = oCLCards.GetActiveCardForCustomerID(oTHeaderData.TransHeader[0].CustomerID);
                    }
                } catch (Exception exp)
                {
                    clsCoreUIHelper.ShowErrorMsg(exp.Message);
                }
                //Till here added By shitaljit
            }
            //Added By Shitaljit(QuicSolv) on 4 oct 2011
            if (this.oTRxDetailData != null && this.oTRxDetailData.Tables[0].Rows.Count > 0)
            {
                setPatientInformation();
            }
            if (this.oPaymentData != null)
            {
                foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                {
                    //Cash
                    if (oRow.TransTypeCode.ToString().Trim() == "1")
                    {
                        if (oRow.Amount < 0)
                        {
                            oRow.Amount = oRow.Amount + (this.oTHeaderData.TransHeader[0].TenderedAmount - this.oTHeaderData.TransHeader[0].TotalPaid);
                        }
                        oCashPaymentRow = oRow;
                        isCashPayment = true;
                    }
                    //House charge
                    if (oRow.TransTypeCode.ToString().Trim().ToUpper() == "H")
                    {
                        AccCode = oRow.RefNo.Substring(0, oRow.RefNo.IndexOf("\\"));
                        AccAmount = decimal.Parse(oRow.Amount.ToString());
                        setAccountInformation(AccCode, false, this.oTHeaderData.TransHeader[0].TransID); //PRIMEPOS-3471 Added extra Parameter
                        oHCPaymentRow = oRow;
                    }
                    //Check
                    if (oRow.TransTypeCode.ToString().Trim() == "2")
                    {
                        oCheckPaymentRow = oRow;
                        this.isCheckPayment = true;
                    }
                    //EBT
                    if (oRow.TransTypeCode.ToString().Trim() == "E")
                    {
                        oEBTPaymentRow = oRow;
                    }
                    //Cash Back
                    if (oRow.TransTypeCode.ToString().Trim() == "B")
                    {
                        oCBPaymentRow = oRow;
                    }
                    //Coupon
                    if (oRow.TransTypeCode.ToString().Trim() == "C")
                    {
                        oCouponPaymentRow = oRow;
                    }

                    //2785
                    if (oRow.TransTypeCode.ToString().Trim() == "8")
                    {
                        oAthPaymentRow = oRow;
                    }

                    //CC
                    //else if ("34567".IndexOf(oRow.TransTypeCode.Trim()) > -1) //PRIMEPOS-2940 09-Mar-2021 JY Commented
                    else if (oRow.TransTypeCode.Trim() == "3" || oRow.TransTypeCode.Trim() == "4" || oRow.TransTypeCode.Trim() == "5" || 
                        oRow.TransTypeCode.Trim() == "6" || oRow.TransTypeCode.Trim() == "7")
                    {
                        oCCPaymentRow = oRow;
                        oRow.RefNo = oRow.RefNo.Trim();
                        this.MergeCCWithRecpt = Configuration.CPOSSet.MergeCCWithRcpt;
                        try
                        {
                            int index = oRow.RefNo.IndexOf("|") - 5;
                            if (index != -1)
                            {
                                string tempstring = oRow.RefNo = oRow.RefNo.Substring(index);
                                tempstring = tempstring.Replace("*", "");   //PRIMEPOS-2876 24-Jul-2020 JY in case of worldpay it returns a string with "*" so just need to replace it.
                                oRow.RefNo = "XXXX-XXXX-XXXX-" + tempstring.Remove(tempstring.IndexOf("|")).Trim();
                                //		oRow.RefNo="XXXX-XXXX-XXXX-" + oRow.RefNo.Substring(oRow.RefNo.IndexOf("|")-5);
                            }
                        }
                        catch (Exception) { }
                    }
                }
            }
            //added By shitalljit to set house charge acount details
            if (oHCPaymentRow == null && oCustomerRow != null && oCustomerRow.PatientNo > 0)
            {
                setAccountInformation(oCustomerRow.PatientNo.ToString(), true,0); //PRIMEPOS-3471 Added Extra Parameter
            }
            if (oTHeaderData != null)
            {
                if (oTHeaderData.Tables[0].Rows.Count > 0) // added by atul 06-jan-2011
                {
                    if (oTHeaderData.TransHeader[0].TransType.ToString().Trim() == "3")
                    {
                        AccCode = this.oTHeaderData.TransHeader[0].Acc_No.ToString().Trim();
                        AccAmount = Decimal.Parse(this.oTHeaderData.TransHeader[0].TotalPaid.ToString());
                        setAccountInformation(AccCode, false, this.oTHeaderData.TransHeader[0].TransID); //PRIMEPOS-3471
                    }
                }
            }

            if (this.oTDetailData != null && this.oTDetailData.Tables.Count > 0)  SetDescForTransData(this.oTDetailData);    //PRIMEPOS-2884 21-Aug-2020 JY Added
        }

        public void GenerateCouponBarcode() // Replace Private to public for POSLITE
        {
            string sImagePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), CouponRow.ID.ToString() + ".bmp");
            string sBarcode = "!" + CouponRow.ID + "!";

            Configuration.PrintBarcode(sBarcode, 0, 0, 20, 200, "CODE128", "H", sImagePath);

            using (Stream stream = new FileStream(sImagePath, FileMode.Open))
            {
                CouponBarCode = Image.FromStream(stream);
                stream.Close();
            }
        }

        public void setAccountInformation(string sCode, bool bSearchByPatient, long posTransId) // Replace Private to public for POSLITE //PRIMEPOS-3471
        {
            this.AccName = "";
            this.AccCurrBalance = 0;
            this.HouseChargeInfo = new HouseChargeAccount();

            try
            {
                if (bSearchByPatient == true)
                {
                    POS_Core.ErrorLogging.Logs.Logger("Direct Print", "setAccountInformation() - About to call PHARMSQL", "");
                    PharmData.PharmBL oPhBl = new PharmData.PharmBL();
                    DataTable dtPatient = null;
                    if (PatientTable != null && oPhBl.ConnectedTo_ePrimeRx())
                        dtPatient = PatientTable;
                    else
                        dtPatient = oPhBl.GetPatient(sCode);
                    POS_Core.ErrorLogging.Logs.Logger("Direct Print", "setAccountInformation() - Successful PHARMSQL Call", "");
                    if (Configuration.isNullOrEmptyDataTable(dtPatient) == false)
                    {
                        AccCode = Configuration.convertNullToInt(dtPatient.Rows[0]["ACCT_NO"]).ToString();  //PRIMEPOS-2777 10-Jan-2020 JY modified
                    }
                }
                //if (AccCode.Equals("0") == false) //PRIMEPOS-2777 10-Jan-2020 JY Commented
                if (Configuration.convertNullToInt(AccCode) != 0)   //PRIMEPOS-2777 10-Jan-2020 JY Added
                {
                    this.HouseChargeInfo = clsCoreHouseCharge.GetAccountInformation(AccCode);
                    this.AccName = Configuration.convertNullToString(this.HouseChargeInfo.AccountName); //PRIMEPOS-2777 10-Jan-2020 JY modified
                    this.AccCurrBalance = Configuration.convertNullToDecimal(this.HouseChargeInfo.CurrentBalance);  //PRIMEPOS-2777 10-Jan-2020 JY modified
                    if (posTransId != 0) //PRIMEPOS-3471
                    {
                        PharmBL oPhBl = new PharmBL();
                        DataTable dt = oPhBl.GetHCAccountDetailsByPosTransId(Configuration.convertNullToInt64(posTransId));
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            this.HCReference = Configuration.convertNullToString(dt.Rows[0]["REFERENCE"]);
                        }
                    }
                } else
                {
                    AccCode = string.Empty;
                }
            } 
            catch (Exception e)
            {
                if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 22-Jun-2022 JY Added
                    clsCoreUIHelper.ShowErrorMsg(e.Message); }
        }

        //Added By Shitaljit(QuicSolv) on 4 oct 2011
        public DataTable PatientInformation = null;

        public void setPatientInformation()// Replace Private to public for POSLITE
        {
            try
            {
                PharmBL oPhBl = new PharmBL();
                DataTable dtPatient = null;

                if (PatientTable != null && oPhBl.ConnectedTo_ePrimeRx())
                    dtPatient = PatientTable;
                else
                    dtPatient = PatientInfo();
                if (dtPatient != null && dtPatient.Rows.Count > 0)
                {
                    PatientInformation = dtPatient;
                    this.PatientName = PatientInformation.Rows[0]["LNAME"].ToString().Trim() + " ," + PatientInformation.Rows[0]["FNAME"].ToString().Trim();
                    this.PatientPhoneNo = PatientInformation.Rows[0]["PHONE"].ToString().Trim();
                    this.PatientAddress = PatientInformation.Rows[0]["ADDRSTR"].ToString().Trim() + " " + PatientInformation.Rows[0]["ADDRCT"].ToString().Trim() + " " + ", " + PatientInformation.Rows[0]["ADDRST"].ToString().Trim() + " " + PatientInformation.Rows[0]["ADDRZP"].ToString().Trim();
                }
            } catch (Exception e)
            {
                clsCoreUIHelper.ShowErrorMsg(e.Message);
            }
        }

        //End of Added By Shitaljit(QuicSolv) on 4 oct 2011

        #region IVU Loto



        /// <summary>
        /// To stroe all IVU lotto details.
        /// </summary>
        public POS_Core.IVULottoService.ivuLotoData IVULottoData
        {
            get
            {
                try
                {
                    return oivuLotoData;
                } catch
                { return null; }
            }
        }


        /// <summary>
        /// To get total state tax
        /// </summary>
        public Decimal TotalStateTax
        {
            get
            {
                return StateTax;
            }
        }

        /// <summary>
        /// Total Local Tax
        /// </summary>
        public Decimal TotalLocalTax
        {
            get
            {
                return LocalTax;
            }
        }

        /// To get total city tax
        /// </summary>
        public Decimal TotalCityTax
        {
            get
            {
                return CityTax;
            }
        }

        /// To get total federal tax
        /// </summary>
        public Decimal TotalFederalTax
        {
            get
            {
                return FederalTax;
            }
        }

        /// To get total municipality tax
        /// </summary>
        public Decimal TotalMunicipalityTax
        {
            get
            {
                return MunicipalityTax;
            }
        }

        /// To get total County tax
        /// </summary>
        public Decimal TotalCountyTax
        {
            get
            {
                return CountyTax;
            }
        }

        /// <summary>
        /// Author: shitaljit 
        /// Created Date: 3/21/2014
        /// To get IVU lotto details that is to be printed on trasnaction receipt.
        /// </summary>
        public void GetIVULottoDetails()
        {
            try
            {
                POSTransaction oPOSTrans = new POSTransaction();
                oPOSTrans.CalculateBreakDownForTax(oTDTaxData, out StateTax, out LocalTax, out FederalTax, out CountyTax, out CityTax, out MunicipalityTax);
                oivuLottotrans.merchantId = Configuration.CInfo.IVULottoMerchantID;
                oivuLottotrans.municipalTax = Math.Abs(MunicipalityTax);
                oivuLottotrans.municipalTaxSpecified = true;
                oivuLottotrans.stateTax = Math.Abs(StateTax);
                oivuLottotrans.stateTaxSpecified = true;
                oivuLottotrans.total = Math.Abs(Decimal.Parse(this.oTHeaderData.TransHeader[0].TotalPaid.ToString()));
                //Total Paid -Total Tax is the sub total
                oivuLottotrans.subTotal = Math.Abs(Decimal.Parse(this.oTHeaderData.TransHeader[0].TotalPaid.ToString()) -
                                         Decimal.Parse(this.oTHeaderData.TransHeader[0].TotalTaxAmount.ToString()));
                oivuLottotrans.totalSpecified = true;
                oivuLottotrans.subTotalSpecified = true;
                oivuLottotrans.txDate = this.oTHeaderData.TransHeader[0].TransDate;
                oivuLottotrans.txDateSpecified = true;
                if (this.TrnansReceiptType == ReceiptType.SalesTransaction)
                {
                    oivuLottotrans.txType = POS_Core.IVULottoService.txType.SALE;
                } else
                {
                    oivuLottotrans.txType = POS_Core.IVULottoService.txType.REFUND;
                }

                oivuLottotrans.txTypeSpecified = true;
                oivuLottotrans.tenderType = GetPaymentMode(oPaymentData);
                oivuLottotrans.tenderTypeSpecified = true;
                oivuLottotrans.terminalId = Configuration.CPOSSet.IVULottoTerminalID;
                oivuLottotrans.terminalPassword = Configuration.CPOSSet.IVULottoPassword;
                //To dynamitically change URL, it can be station or pharmacy specific depending upon the set up of IVU Lotto.
                oTxServerService.Url = Configuration.CPOSSet.IVULottoServerURL;
                oivuLotoData = oTxServerService.requestIVULoto(oivuLottotrans);
                if (string.IsNullOrEmpty(oivuLotoData.drawNumber) == true)
                {
                    oivuLotoData = null;
                }

            } catch (Exception ex)
            {
                POS_Core.ErrorLogging.Logs.Logger("Unable to get IVU lotto details " + ex.Message + "  " + ex.StackTrace.ToString());
                oivuLotoData = null;
            }
        }

        /// <summary>
        /// To get payment mode to be send to get loto information.
        /// The last payment mode use will be consider as payment mode.
        /// </summary>
        /// <param name="oPaymentData"></param>
        /// <returns></returns>
        public POS_Core.IVULottoService.tenderType GetPaymentMode(POSTransPaymentData oPaymentData)
        {
            try
            {
                if (Configuration.isNullOrEmptyDataSet(oPaymentData) == false)
                {
                    POSTransPaymentRow oRow = oPaymentData.POSTransPayment[oPaymentData.POSTransPayment.Rows.Count - 1];
                    //if ("3456".IndexOf(oRow.TransTypeCode.Trim()) > -1)   //PRIMEPOS-2940 09-Mar-2021 JY Commented
                    if (oRow.TransTypeCode.Trim() == "3" || oRow.TransTypeCode.Trim() == "4" || oRow.TransTypeCode.Trim() == "5" || oRow.TransTypeCode.Trim() == "6")
                    {
                        return POS_Core.IVULottoService.tenderType.CREDIT;
                    }
                    //else if ("7".IndexOf(oRow.TransTypeCode.Trim()) > -1) //PRIMEPOS-2940 09-Mar-2021 JY Commented
                    else if (oRow.TransTypeCode.Trim() == "7")
                    {
                        return POS_Core.IVULottoService.tenderType.DEBIT;
                    } 
                    else
                    {
                        return POS_Core.IVULottoService.tenderType.CASH;
                    }

                } 
                else
                {
                    return POS_Core.IVULottoService.tenderType.CASH;
                }
            } catch
            {

                return POS_Core.IVULottoService.tenderType.CASH;
            }
        }

        /// <summary>
        /// Added By Shitaljit to Print draw date in MMM/DD/YYYY formate
        /// </summary>
        public string IVULottoDrawDate
        {
            get
            {
                try
                {
                    string format = "{0:MMM/dd/yyyy}";
                    string iVULottoDrawDate = String.Format(format, IVULottoData.drawDate);
                    return iVULottoDrawDate;
                } catch
                {

                    return IVULottoData.drawDate.ToShortDateString();
                }
            }
        }

        public string TrasnctionType
        {
            get
            {
                try
                {
                    if (this.TrnansReceiptType != ReceiptType.SalesTransaction && PaymentProcessor == "EVERTEC")//PRIMEPOS-2785
                    {
                        return "REFUND";
                    }

                    if (this.TrnansReceiptType == ReceiptType.SalesTransaction)
                    {
                        return "SALE";
                    } else
                    {
                        return "RETURN";
                    }
                } catch
                {

                    return "";
                }
            }
        }
        #endregion

        public Boolean Print()
        {
            bool bReceiptsPrinted = false;  //PRIMEPOS-2647 12-Mar-2019 JY Added logic to return no of receipt printed

            try
            {
                #region PRIMEPOS-3381
                if (this.oPaymentData != null && this.oPaymentData.POSTransPayment != null)
                {
                    foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                    {
                        if (oRow.PaymentProcessor.ToUpper() == "NB_VANTIV") //PRIMEPOS-3482
                        {
                            oRow.TransTypeDesc = "NB CARD"; //PRIMEPOS-3482
                        }
                    }
                }
                #endregion
                #region PRIMEPOS-2786 EVERTEC
                //if (isEBTBalance && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                if (isEBTBalance && ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC") || (bPrintDuplicateReceipt && tmpPaymentProcessor == "EVERTEC")))   //PRIMEPOS-2876 27-Jul-2020 JY modified
                {
                    Logs.Logger("Printing EBT Balance receipt Print()");

                    PrintLabelL(false, "EBT");

                    return true;
                }
                #endregion
                #region PRIMEPOS-2785
                //else if (isDenialReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                else if (isDenialReceipt && ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC") || (bPrintDuplicateReceipt && tmpPaymentProcessor == "EVERTEC")))  //PRIMEPOS-2876 27-Jul-2020 JY modified
                {
                    Logs.Logger("Printing EBT Balance receipt Print()");

                    PrintLabelL(false, "DENIAL");

                    return true;
                }
                #endregion
                else if (isDenialReceipt && ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "ELAVON") || (bPrintDuplicateReceipt && tmpPaymentProcessor == "ELAVON"))) //2943
                {
                    Logs.Logger("Printing EBT Balance receipt Print()");

                    PrintLabelL(false, "DENIAL");

                    return true;
                }
                #region PRIMEPOS-2664
                else if (isVoidReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "EVERTEC")
                {
                    Logs.Logger("Printing Void receipt Print()");

                    PrintLabelL(false, "VOID");

                    return true ;
                }
                #endregion
                //Added By shitaljit on 21March2014 to print IVU lotto information on the receipt.
                Logs.Logger("DirectPrint Entering Print()");
                if (Configuration.CInfo.UseIVULottoProgram == true && this.TrnansReceiptType != null &&
                   (this.TrnansReceiptType == ReceiptType.SalesTransaction || this.TrnansReceiptType == ReceiptType.ReturnTransaction))
                {
                    GetIVULottoDetails();
                } else
                {
                    oivuLotoData = null;
                }
                //End

                SignatureIndex = 0;
                nTransPayID = 0;    //PRIMEPOS-2939 03-Mar-2021 JY Added
                if (this.MergeCCWithRecpt == false)
                {
                    if (Configuration.convertNullToInt(this.oTHeaderData.TransHeader[0].TransType.ToString()) == (int)POSTransactionType.ReceiveOnAccount)
                    {
                        //for(int i = 0; i < Configuration.CInfo.NoOfRARC; i++)
                        for (int i = 0; i < Configuration.CPOSSet.NoOfRARC; i++)// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                        {
                            Print(false);
                            bReceiptsPrinted = true;
                        }
                    } else if (this.oPaymentData.POSTransPayment.Rows.Count > 0 && this.oPaymentData.POSTransPayment[0].TransTypeCode.ToString().Trim().ToUpper() == "H")
                    {
                        //for(int i = 0; i < Configuration.CInfo.NoOfHCRC; i++)
                        //for (int i = 0; i < Configuration.CPOSSet.NoOfHCRC; i++)// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set                        
                        int ReceiptPrintCount = 0;
                        Configuration.dctPayTypeReceipts.TryGetValue("H", out ReceiptPrintCount);   //PRIMEPOS-2308 16-May-2018 JY Added
                        for (int i = 0; i < ReceiptPrintCount; i++)
                        {
                            Print(false);
                            bReceiptsPrinted = true;
                        }

                        #region PRIMEPOS-2647 20-Mar-2019 JY Added logic to print duplicate receipt when grid setting is 0 and even # of receipt is 0
                        if (bPrintDuplicateReceipt == true && bReceiptsPrinted == false)
                        {
                            bPrintDuplicateReceipt = false;
                            Print(false);
                        }
                        #endregion
                        bool bTempReceiptsPrinted = PrintGiftCoupon();  //Sprint-27 - PRIMEPOS-2160 11-Sep-2017 JY Added to print gift coupon
                        if (bReceiptsPrinted == false && bTempReceiptsPrinted == true) bReceiptsPrinted = true;
                    }
                    else
                    {
                        int noOfReceipts = 0;

                        if (IsOnHoldTrans == true)
                        {
                            //noOfReceipts = Configuration.CInfo.NoOfOnHoldTransReceipt;
                            noOfReceipts = Configuration.CPOSSet.NoOfOnHoldTransReceipt;// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                        } else
                        {
                            //noOfReceipts = Configuration.CInfo.NoOfReceipt;
                            noOfReceipts = Configuration.CPOSSet.NoOfReceipt;// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                        }
                        //if(isCashPayment == true && Configuration.CInfo.NoOfCashReceipts > 0)
                        //if (isCashPayment == true && Configuration.CPOSSet.NoOfCashReceipts > 0)// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                        //{

                        //    //noOfReceipts = Configuration.CInfo.NoOfCashReceipts;
                        //    noOfReceipts = Configuration.CPOSSet.NoOfCashReceipts;// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                        //}                     

                        #region PRIMEPOS-2308 17-May-2018 JY Added
                        int ReceiptPrintCount = 0;
                        if (isCashPayment == true)
                        {
                            Configuration.dctPayTypeReceipts.TryGetValue("1", out ReceiptPrintCount);
                        } else
                        {
                            ReceiptPrintCount = GetNoOfReceipts();
                        }
                        #endregion

                        #region PRIMEPOS-2623 21-Dec-2018 JY Added
                        //if (ReceiptPrintCount > 0)
                        //{
                        //    noOfReceipts = ReceiptPrintCount;
                        //}                        
                        if (!IsOnHoldTrans)
                        {
                            #region PRIMEPOS-2624 04-Jan-2019 JY Added
                            bool bIsCheckOrCC = false;
                            foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                            {
                                //if Check or CC paymemt
                                //if (oRow.TransTypeCode.Trim() == "2" || "34567E".IndexOf(oRow.TransTypeCode.Trim()) > -1) //PRIMEPOS-2940 09-Mar-2021 JY Commented
                                if (oRow.TransTypeCode.Trim() == "2" || oRow.TransTypeCode.Trim() == "3" || oRow.TransTypeCode.Trim() == "4" ||
                                    oRow.TransTypeCode.Trim() == "5" || oRow.TransTypeCode.Trim() == "6" || oRow.TransTypeCode.Trim() == "7" ||
                                    oRow.TransTypeCode.Trim() == "E")
                                {
                                    bIsCheckOrCC = true;
                                    break;
                                }
                            }

                            if (bIsCheckOrCC == false)
                                noOfReceipts = ReceiptPrintCount;
                            #endregion
                        }
                        #endregion

                        for (int i = 0; i < noOfReceipts; i++)
                        {
                            Print(false);
                            bReceiptsPrinted = true;
                        }
                        #region PRIMEPOS-2647 20-Mar-2019 JY Added logic to print duplicate receipt when grid setting is 0 and even # of receipt is 0
                        if (bPrintDuplicateReceipt == true && bReceiptsPrinted == false)
                        {
                            bPrintDuplicateReceipt = false;
                            Print(false);
                        }
                        #endregion

                        //Added By Shitaljit for printing gift receipt on 21 Jan 2013
                        //if(PrintGiftreceipt == true && Configuration.CInfo.NoOfGiftReceipt > 0)
                        bool bTempReceiptsPrinted = PrintGiftCoupon();  //moved logic inside function                       
                        if (bReceiptsPrinted == false && bTempReceiptsPrinted == true) bReceiptsPrinted = true;

                        #region Sprint-18 - 2039 31-Oct-2014 JY Added to print coupon receipt seperately
                        if (this.PrintCLCouponSeparately == true && oCustomerRow.UseForCustomerLoyalty == true && CLCouponValue > 0 && (this.PrintCLCouponOnlyIfTierIsReached == false || (this.PrintCLCouponOnlyIfTierIsReached == true && this.isCLTierreached == true))) //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value
                        {
                            bReceiptsPrinted = true;
                            if (Configuration.CPOSSet.ReceiptPrinterType == "L")
                            {
                                PrintLabelL(this.PrintCLCouponSeparately, "CR"); //Sprint-18 - 2039 31-Oct-2014 JY Added additional parameter to resolve the wrong label pring issue
                            } else
                            {
                                PrintSeparateCouponRC();
                            }
                        }
                        #endregion
                    }

                    foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                    {
                        #region FSA Tras Check
                        if (oRow.IsIIASPayment == true)
                        {
                            totalFSAAmount += oRow.Amount;
                            isFSATranaction = true;
                        }
                        #endregion

                        //if ("34567E".IndexOf(oRow.TransTypeCode.Trim()) > -1) //PRIMEPOS-2940 09-Mar-2021 JY Commented
                        if (oRow.TransTypeCode.Trim() == "3" || oRow.TransTypeCode.Trim() == "4" || oRow.TransTypeCode.Trim() == "5" || 
                            oRow.TransTypeCode.Trim() == "6" || oRow.TransTypeCode.Trim() == "7" || oRow.TransTypeCode.Trim() == "E")
                        {
                            //oCCPaymentRow = oRow; //PRIMEPOS-2939 03-Mar-2021 JY Commented
                            #region PRIMEPOS-2939 03-Mar-2021 JY Added
                            bool bCC = false;
                            if (oRow.TransTypeCode.ToString().Trim() == "E")
                            {
                                oEBTPaymentRow = oRow;
                                oCCPaymentRow = oRow;//PRIMEPOS-2952 added for solve receipt print Issue
                            }
                            else
                            {
                                bCC = true;
                                oCCPaymentRow = oRow;
                            }
                            SetParameters(oPaymentData, oRow, bCC);
                            #endregion

                            oRow.RefNo = oRow.RefNo.Trim();
                            try
                            {
                                //if (POS_Core.Resources.Configuration.CPOSSet.PaymentProcessor.Equals(clsPOSDBConstants.WORLDPAY))
                                if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.Equals(clsPOSDBConstants.WORLDPAY)) || (bPrintDuplicateReceipt && tmpPaymentProcessor == clsPOSDBConstants.WORLDPAY))   //PRIMEPOS-2876 27-Jul-2020 JY modified
                                {
                                    if (!oRow.RefNo.Contains("XXXX-XXXX-XXXX"))
                                    {
                                        oRow.RefNo = "XXXX-XXXX-XXXX-" + oRow.RefNo.Replace("*", "");
                                    }
                                    else
                                    {
                                        oRow.RefNo = oRow.RefNo.Replace("*", "");
                                    }
                                }
                                else
                                {
                                    int index = oRow.RefNo.IndexOf("|");
                                    if (index != -1)
                                    {
                                        oRow.RefNo = "XXXX-XXXX-XXXX-" + oRow.RefNo.Substring(index - 5);
                                        if (oRow.RefNo.EndsWith("|"))
                                            oRow.RefNo.Replace("|", "");
                                    }
                                }
                            }
                            catch (Exception ex) { }
                            //for(int i = 1; i <= Configuration.CInfo.NoOfCC; i++)
                            //for (int i = 1; i <= Configuration.CPOSSet.NoOfCC; i++)// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                            int ReceiptPrintCount = 0;
                            Configuration.dctPayTypeReceipts.TryGetValue("99", out ReceiptPrintCount);   //PRIMEPOS-2308 16-May-2018 JY Added
                            for (int i = 0; i < ReceiptPrintCount; i++)
                            {
                                if (i > 1 && SignatureIndex > 0)
                                {
                                    SignatureIndex--;
                                }
                                nTransPayID = oRow.TransPayID;   //PRIMEPOS-2939 03-Mar-2021 JY Added
                                Print(true);
                                bReceiptsPrinted = true;
                            }
                        }
                    }
                }
                else
                {
                    foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                    {
                        #region FSA Tras Check
                        if (oRow.IsIIASPayment == true) //Added by Manoj 7/31/2014 
                        {
                            totalFSAAmount += oRow.Amount;
                            isFSATranaction = true;
                        }
                        #endregion

                        #region PRIMEPOS-2939 02-Feb-2021 JY commented
                        //if ("34567E".IndexOf(oRow.TransTypeCode.Trim()) > -1)
                        //{
                        //    oCCPaymentRow = oRow;
                        //    oRow.RefNo = oRow.RefNo.Trim();
                        //    try
                        //    {
                        //        //if (POS_Core.Resources.Configuration.CPOSSet.PaymentProcessor.Equals(clsPOSDBConstants.WORLDPAY))
                        //        if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.Equals(clsPOSDBConstants.WORLDPAY)) || (bPrintDuplicateReceipt && tmpPaymentProcessor == clsPOSDBConstants.WORLDPAY))   //PRIMEPOS-2876 27-Jul-2020 JY modified
                        //        {
                        //            if (!oRow.RefNo.Contains("XXXX-XXXX-XXXX"))
                        //            {
                        //                oRow.RefNo = "XXXX-XXXX-XXXX-" + oRow.RefNo.Replace("*", "");
                        //            }
                        //            else
                        //            {
                        //                oRow.RefNo = oRow.RefNo.Replace("*", "");
                        //            }
                        //        }
                        //        else
                        //        {
                        //            int index = oRow.RefNo.IndexOf("|");
                        //            if (index != -1)
                        //            {
                        //                oRow.RefNo = "XXXX-XXXX-XXXX-" + oRow.RefNo.Substring(index - 5);

                        //                if (oRow.RefNo.EndsWith("|"))
                        //                    oRow.RefNo.Replace("|", "");
                        //            }
                        //        }
                        //    }
                        //    catch (Exception) { }
                        //    //  for(int i = 1; i <= Configuration.CInfo.NoOfCC; i++)
                        //    //for (int i = 1; i <= Configuration.CPOSSet.NoOfCC; i++)// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                        //    int ReceiptPrintCount = 0;
                        //    Configuration.dctPayTypeReceipts.TryGetValue("99", out ReceiptPrintCount);   //PRIMEPOS-2308 16-May-2018 JY Added
                        //    for (int i = 0; i < ReceiptPrintCount; i++)
                        //    {
                        //        if (i > 1 && SignatureIndex > 0)
                        //        {
                        //            SignatureIndex--;
                        //        }
                        //        Print(false);
                        //        if (Configuration.CPOSSet.ReceiptPrinterType != "L")
                        //        {
                        //            Print(true);
                        //        }
                        //        bReceiptsPrinted = true;
                        //    }
                        //}
                        #endregion
                    }

                    #region PRIMEPOS-2939 02-Feb-2021 JY Added
                    int noOfReceipts = 0;
                    if (IsOnHoldTrans == true)
                        noOfReceipts = Configuration.CPOSSet.NoOfOnHoldTransReceipt;
                    else
                        noOfReceipts = Configuration.CPOSSet.NoOfReceipt;

                    bool bIsCC = false;
                    foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                    {
                        if ("34567E".IndexOf(oRow.TransTypeCode.Trim()) > -1)
                        {
                            bIsCC = true;
                            break;
                        }
                    }

                    if (bIsCC)
                    {
                        int ReceiptPrintCount = 0;
                        Configuration.dctPayTypeReceipts.TryGetValue("99", out ReceiptPrintCount);
                        if (ReceiptPrintCount > 0)
                            noOfReceipts = ReceiptPrintCount;
                    }

                    for (int i = 0; i < noOfReceipts; i++)
                    {
                        SignatureIndex = 0;  //PRIMEPOS-3429
                        Print(false);
                        if (Configuration.CPOSSet.ReceiptPrinterType != "L")
                        {
                            Print(true);
                        }
                        bReceiptsPrinted = true;
                    }

                    if (bPrintDuplicateReceipt == true && bReceiptsPrinted == false)
                    {
                        bPrintDuplicateReceipt = false;
                        Print(false);
                    }
                    #endregion

                    bool bTempReceiptsPrinted = PrintGiftCoupon();   //Sprint-27 - PRIMEPOS-2160 11-Sep-2017 JY Added logic to print gift card for "CC" paytype                     
                    if (bReceiptsPrinted == false && bTempReceiptsPrinted == true) bReceiptsPrinted = true;
                }

                //Added By Shitaljit 0n 13 March 2012
                //For dual receipt for Check payment.
                foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                {
                    //TransTypeCode "2" is for Check Payment
                    if (oRow.TransTypeCode.Trim() == "2")
                    {
                        oCheckPaymentRow = oRow;
                        //for (int i = 1; i <= Configuration.CInfo.NoOfCheckRC; i++)
                        //for (int i = 1; i <= Configuration.CPOSSet.NoOfCheckRC; i++)// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                        int ReceiptPrintCount = 0;
                        Configuration.dctPayTypeReceipts.TryGetValue("2", out ReceiptPrintCount);   //PRIMEPOS-2308 16-May-2018 JY Added
                        for (int i = 0; i < ReceiptPrintCount; i++)
                        {
                            PrintCheckRC();
                            bReceiptsPrinted = true;
                        }
                    }
                }
                Logs.Logger("DirectPrint Exiting Print()");
                return bReceiptsPrinted;
            } catch (Exception exp)
            {
                //clsUIHelper.ShowErrorMsg(exp.Message);//Commented by Shitaljit(QuicSolv) on 12 Jan 2011 to change the error message if there is any exception while printing receipt.
                clsCoreUIHelper.ShowErrorMsg("Unable to print receipt, Please check your printer settings.");
                POS_Core.ErrorLogging.Logs.Logger("Unable to print receipt " + exp.Message + "  " + exp.StackTrace.ToString());
                return false;
            }
        }

        #region PRIMEPOS-2308 17-May-2018 JY Added logic to get receipt print count                       
        public int GetNoOfReceipts() // Replace private to public for POSLITE
        {
            int ReceiptPrintCount = 0;
            try
            {
                foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                {
                    //if ("34567E".IndexOf(oRow.TransTypeCode.Trim()) > -1) //PRIMEPOS-2940 09-Mar-2021 JY Commented
                    if (oRow.TransTypeCode.Trim() == "3" || oRow.TransTypeCode.Trim() == "4" || oRow.TransTypeCode.Trim() == "5" || 
                        oRow.TransTypeCode.Trim() == "6" || oRow.TransTypeCode.Trim() == "7" || oRow.TransTypeCode.Trim() == "E")
                    {
                        Configuration.dctPayTypeReceipts.TryGetValue("99", out ReceiptPrintCount);
                        break;
                    } else
                    {
                        Configuration.dctPayTypeReceipts.TryGetValue(oRow.TransTypeCode.Trim(), out ReceiptPrintCount);
                        break;
                    }
                }
            } catch (Exception exp)
            {
            }
            return ReceiptPrintCount;
        }
        #endregion

        #region Sprint-27 - PRIMEPOS-2160 11-Sep-2017 JY Just moved print gift coupon logic in saperate function
        public bool PrintGiftCoupon(int NoOfGiftReceipt = 0) // Replace private to public for POSLITE
        {
            bool bReceiptsPrinted = false;  //PRIMEPOS-2647 12-Mar-2019 JY Added
            int nCount = Configuration.CPOSSet.NoOfGiftReceipt == 0 ? NoOfGiftReceipt : Configuration.CPOSSet.NoOfGiftReceipt;  //PRIMEPOS-2677 29-Jun-2020 JY Added
            if (PrintGiftReciept == true && nCount > 0) // Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
            {
                for (int i = 0; i < nCount; i++)// Added by Ravindra PRIMEPOS-1538 Number of receipts printed to be a station set rather then a global set
                {
                    bReceiptsPrinted = true;
                    if (Configuration.CPOSSet.ReceiptPrinterType == "L")
                    {
                        PrintLabelL(PrintGiftReciept, "GR");  //Sprint-18 - 2039 31-Oct-2014 JY Added additional parameter to resolve the wrong label pring issue
                    } else
                    {
                        PrintGiftRC();
                    }
                }
            }
            return bReceiptsPrinted;
        }
        #endregion
        
        /// <summary>
        /// function to print check payment receipt
        /// </summary>
        public void PrintCheckRC() // Replace private to public for POSLITE
        {
            if (Configuration.CPOSSet.ReceiptPrinterType == "L")
            {
                PrintLabelL(isCheckPayment, "CPR");    //Sprint-18 - 2039 31-Oct-2014 JY Added additional parameter to resolve the wrong label pring issue
            } else
            {
                PrintCheck();
            }
        }

        public void Print(bool bPrintCC) // Replace private to public for POSLITE
        {
            Logs.Logger("DirectPrint Enter Print(Bool)" + bPrintCC);
            if (bPrintCC == false)
            {
                if (Configuration.CPOSSet.ReceiptPrinterType == "L")
                {
                    PrintLabelL(false, "R"); //Sprint-18 - 2039 31-Oct-2014 JY Added additional parameter to resolve the wrong label pring issue
                } else
                {
                    PrintLabelD();
                }
            } else
            {
                if (Configuration.CPOSSet.ReceiptPrinterType == "L")
                {
                    PrintLabelL(true, "R");  //Sprint-18 - 2039 31-Oct-2014 JY Added additional parameter to resolve the wrong label pring issue
                } else
                {
                    PrintCC();
                }
            }
            Logs.Logger("DirectPrint Exiting Print(bool bPrintCC)");
        }

        public void PrintLine(string LineToPrint)
        {
            int pcWritten = 0;
            PrintDirectAPIs.WritePrinter(lhPrinter, LineToPrint, LineToPrint.Length, ref pcWritten);
            return;
        }

        public void OpenDrawer(bool bCalledFromScheduler = false)   //PRIMEPOS-3039 16-Dec-2021 JY Added bCalledFromScheduler
        {
            OpenDrawer(Configuration.DrawNo, bCalledFromScheduler);
        }

        public void OpenDrawer(int DrawNo, bool bCalledFromScheduler = false)   //PRIMEPOS-3039 16-Dec-2021 JY Added bCalledFromScheduler
        {
            // this routine prints the dot matrix label
            try
            {
                if (string.IsNullOrEmpty(Configuration.CPOSSet.RP_Name.Trim()))
                    return;
                this.lhPrinter = new System.IntPtr();
                DOCINFO di = new DOCINFO();
                di.pDocName = "Label";
                di.pDataType = "RAW";
                if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
                {
                    if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                    {
                        if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                        {
                            DrawNo = DrawNo == 0 ? 1 : DrawNo;
                            if (DrawNo == 1)
                            {
                                //PrintLine("Welcome to pos\n\n\n");
                                PrintLine(Convert.ToChar(27) + Configuration.CPOSSet.CDP_CODE);
                            } else if (DrawNo == 2)
                            {
                                PrintLine(Convert.ToChar(27) + Configuration.CPOSSet.CDP_CODE2);
                            }
                        }
                    }
                }
                PrintDirectAPIs.EndPagePrinter(lhPrinter);
                PrintDirectAPIs.EndDocPrinter(lhPrinter);
                PrintDirectAPIs.ClosePrinter(lhPrinter);
            } 
            catch (Exception) 
            { 
                if (!bCalledFromScheduler) //PRIMEPOS-3039 16-Dec-2021 JY Added if condition
                    clsCoreUIHelper.ShowErrorMsg("Unable to open drawer"); 
            }
        }

        public void PrintLabelD() // Replace private to public for POSLITE
        {
            // this routine prints the dot matrix label
            sLabelFile = "PrRECPT.js";

            if (System.IO.File.Exists(sLabelFile))
            {
                this.lhPrinter = new System.IntPtr();

                DOCINFO di = new DOCINFO();
                di.pDocName = "Label";
                di.pDataType = "RAW";

                // the \x1b means an ascii escape character
                //st1="\x1b*c600a6b0P\f";
                //lhPrinter contains the handle for the printer opened
                //If lhPrinter is 0 then an error has occured
                bool bSuccess = false;
                if (Configuration.CPOSSet.RP_Name.Trim() == "")
                {
                    bSuccess = true;
                } else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
                {
                    if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                    {
                        if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                        {
                            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);

                            //try
                            //{
                            //for(int i=0; i < Configuration.CInfo.NoOfReceipt; i++)
                            //{
                            _lbl.PrintCLabel();   // this will actually send the label printing lines
                            //}
                            bSuccess = true;
                            //}
                            //catch(Exception e)
                            //{
                            //	Resources.Message.Display(e.Message.ToString(),"ERROR Printing Label",MessageBoxButtons.OK,MessageBoxIcon.Error);

                            //}
                        }
                    }
                }
                PrintDirectAPIs.EndPagePrinter(lhPrinter);
                PrintDirectAPIs.EndDocPrinter(lhPrinter);
                PrintDirectAPIs.ClosePrinter(lhPrinter);
                if (bSuccess == false)
                {
                    clsCoreUIHelper.ShowErrorMsg("Unable to connect to Printer");
                }
                return;
            } else
            {
                throw (new Exception("Label File : " + sLabelFile + " Not Found"));
            }
        }

        public void PrintLabelL(bool bPrintCC, string strReceiptType)  //Sprint-18 - 2039 31-Oct-2014 JY Added additional parameter strReceiptType to resolve the wrong label pring issue  // Replace private to public for POSLITE
        {
            mPD.PrinterSettings.PrinterName = Configuration.CPOSSet.RP_Name.Trim();
            #region PRIMEPOS-2996 23-Sep-2021 JY Added
            try
            {
                if (Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim() != "")
                {
                    System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                    foreach (System.Drawing.Printing.PaperSource ps in mPD.PrinterSettings.PaperSources)
                    {
                        if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim().ToUpper())
                        {
                            mPD.DefaultPageSettings.PaperSource = ps;
                            break;
                        }
                    }
                }
            }
            catch { }
            #endregion

            this.CFont = this.Times;
            if (bPrintCC == false && strReceiptType == "R")
            {
                this.mPD.DocumentName = "TRANS";
                sLabelFile = "PrRECPTL.js";
            }
            //Added by shitaljit on 13 March 2012 for check payment receipt printing.
            else if (isCheckPayment == true && strReceiptType == "CPR")
            {
                this.mPD.DocumentName = "CHECK";
                sLabelFile = "CheckRECPTL.js";
            }
            //Added by shitaljit on 21 jan 2013 for gift receipt printing.
            else if (PrintGiftReciept == true && strReceiptType == "GR")
            {
                this.mPD.DocumentName = "GIFT";
                sLabelFile = "GiftRECPTL.js";
            }
            //Sprint-18 - 2039 31-Oct-2014 JY Added to print coupon receipt seperately
            else if (this.PrintCLCouponSeparately == true && strReceiptType == "CR")
            {
                this.mPD.DocumentName = "CouponReceipt";
                sLabelFile = "PRCouponReceipt.js";
            }
            #region PRIMEPOS-2786 EVERTEC
            else if (strReceiptType == "EBT")
            {
                this.mPD.DocumentName = "EBTReceipt";
                sLabelFile = "EBTReceipt.js";
            }
            #endregion
            #region PRIMEPOS-2785 EVERTEC
            else if (strReceiptType == "DENIAL")
            {
                this.mPD.DocumentName = "EvertecDenialReceipt";
                sLabelFile = "EvertecDenialReceipt.js";
            }
            #endregion
            #region PRIMEPOS-2664 EVERTEC
            else if (strReceiptType == "VOID")
            {
                this.mPD.DocumentName = "EvertecVoidReceipt";
                sLabelFile = "EvertecVoidReceipt.js";
            }
            #endregion
            else
            {
                this.mPD.DocumentName = "CC";
                sLabelFile = "CCRECPTL.js";
            }
            this.mPD.Print();
        }

        public void PrintLabel(object sender, PrintPageEventArgs ev) // Replace private to public for POSLITE
        {
            // this routine actually prints the label
            //Application.DoEvents();

            this.PR = ev;

            ev.Graphics.PageUnit = GraphicsUnit.Document;
            this.CFont = this.Times;

            this.PageWidth = ev.MarginBounds.Width;
            String FileDirectoryPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath.ToString());
            String FilePath = System.IO.Path.Combine(FileDirectoryPath, sLabelFile);
            if (System.IO.File.Exists(FilePath))
            {
                ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
                if (sLabelFile == "CCRECPTL.js")
                {
                    _lbl.PrintCCRec();
                }
                //Added by shitaljit on 13 March 2012 for check payment receipt printing.
                else if (sLabelFile == "CheckRECPTL.js")
                {
                    _lbl.PrintCCRec();
                }
                //Added by shitaljit on 23 Jan 2013 for gift receipt printing.
                else if (sLabelFile == "GiftRECPTL.js")
                {
                    _lbl.PrintCCRec();
                }
                //Sprint-18 - 2039 31-Oct-2014 JY Added to print coupon receipt seperately
                else if (sLabelFile == "PRCouponReceipt.js")
                {
                    _lbl.PrintCCRec();
                } else
                {
                    _lbl.PrintCLabel();
                    if (this.MergeCCWithRecpt == true)
                    {
                        //FilePath = System.IO.Path.Combine(FileDirectoryPath, "CCRECPTL.js");
                        //_lbl = new ScriptableLabel(this, "CCRECPTL.js");
                        //_lbl.PrintCCRec();

                        #region PRIMEPOS-2939 02-Feb-2021 JY Added
                        foreach (POSTransPaymentRow oRow in this.oPaymentData.POSTransPayment.Rows)
                        {
                            //if ("34567E".IndexOf(oRow.TransTypeCode.Trim()) > -1) //PRIMEPOS-2940 09-Mar-2021 JY Commented
                            if (oRow.TransTypeCode.Trim() == "3" || oRow.TransTypeCode.Trim() == "4" || oRow.TransTypeCode.Trim() == "5" || 
                                oRow.TransTypeCode.Trim() == "6" || oRow.TransTypeCode.Trim() == "7" || oRow.TransTypeCode.Trim() == "E")
                            {
                                //oCCPaymentRow = oRow; //PRIMEPOS-2939 03-Mar-2021 JY Commented
                                #region PRIMEPOS-2939 03-Mar-2021 JY Added
                                bool bCC = false;
                                if (oRow.TransTypeCode.ToString().Trim() == "E")
                                {
                                    oEBTPaymentRow = oRow;
                                    oCCPaymentRow = oRow;//PRIMEPOS-2952 added for solve receipt print Issue
                                }
                                else
                                {
                                    bCC = true;
                                    oCCPaymentRow = oRow;
                                }
                                SetParameters(oPaymentData, oRow, bCC);
                                nTransPayID = oRow.TransPayID;
                                #endregion

                                oRow.RefNo = oRow.RefNo.Trim();
                                try
                                {
                                    if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.Equals(clsPOSDBConstants.WORLDPAY)) || (bPrintDuplicateReceipt && tmpPaymentProcessor == clsPOSDBConstants.WORLDPAY))   //PRIMEPOS-2876 27-Jul-2020 JY modified
                                    {
                                        if (!oRow.RefNo.Contains("XXXX-XXXX-XXXX"))
                                        {
                                            oRow.RefNo = "XXXX-XXXX-XXXX-" + oRow.RefNo.Replace("*", "");
                                        }
                                        else
                                        {
                                            oRow.RefNo = oRow.RefNo.Replace("*", "");
                                        }
                                    }
                                    else
                                    {
                                        int index = oRow.RefNo.IndexOf("|");
                                        if (index != -1)
                                        {
                                            oRow.RefNo = "XXXX-XXXX-XXXX-" + oRow.RefNo.Substring(index - 5);

                                            if (oRow.RefNo.EndsWith("|"))
                                                oRow.RefNo.Replace("|", "");
                                        }
                                    }
                                }
                                catch (Exception) { }

                                FilePath = System.IO.Path.Combine(FileDirectoryPath, "CCRECPTL.js");
                                _lbl = new ScriptableLabel(this, "CCRECPTL.js");
                                _lbl.PrintCCRec();
                            }
                        }
                        #endregion
                    }
                }
                ev.HasMorePages = false;
            } else
            {
                throw (new Exception("Label File : " + sLabelFile + " Not Found"));
            }
        }

        public void PrintLabelPayout()
        {
            // this routine prints the dot matrix label
            //				string sLabelFile;
            //				sLabelFile="rcptEOD.js";
            //
            //				if (System.IO.File.Exists(sLabelFile))
            //				{
            this.lhPrinter = new System.IntPtr();

            DOCINFO di = new DOCINFO();
            di.pDocName = "Label";
            di.pDataType = "RAW";

            // the \x1b means an ascii escape character
            //st1="\x1b*c600a6b0P\f";
            //lhPrinter contains the handle for the printer opened
            //If lhPrinter is 0 then an error has occured
            //bool bSuccess=false;
            if (Configuration.CPOSSet.RP_Name.Trim() == "")
            {
                //bSuccess=true;
            } 
            else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
            {
                if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                {
                    if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                    {
                        //ScriptableLabel _lbl = new ScriptableLabel(this,sLabelFile);

                        try
                        {
                            PrintPayout();
                            //for(int i=0; i < Configuration.CInfo.NoOfReceipt; i++)
                            //{
                            //_lbl.PrintCLabel();   // this will actually send the label printing lines
                            //}
                            //	bSuccess=true;
                        } catch (Exception e)
                        {
                            clsCoreUIHelper.ShowErrorMsg(e.Message);
                        }
                    }
                }
            }
            PrintDirectAPIs.EndPagePrinter(lhPrinter);
            PrintDirectAPIs.EndDocPrinter(lhPrinter);
            PrintDirectAPIs.ClosePrinter(lhPrinter);
            //					if (bSuccess == false)
            //					{
            //						clsUIHelper.ShowErrorMsg("Unable to connect to printer");
            //
            //					}
            //					return;
            //				}
        }

        public void PrintPayout()
        {
            PrintLine("\x1B");

            this.BlankLineBeforeHeading = true;

            PrintLine(Convert.ToChar(1).ToString());

            PrintLine("\x1B" + "!`" + CStoreName + "\x1B" + "!" + Convert.ToChar(1));
            PrintLine("\n" + CAddress);
            PrintLine("\n" + CCity + ", " + CState + ", " + CZip);
            PrintLine("\nTel: " + CTelephone);
            PrintLine("\n" + Replicate("-", 49));
            PrintLine("\n" + Convert.ToString("Station: " + oPayoutRow.StationID).PadRight(20) + " " + Convert.ToString("Date: " + Convert.ToDateTime(oPayoutRow.TransDate.ToString()).ToShortDateString()).PadLeft(20) + " ");
            PrintLine("\n" + Convert.ToString("Oper: " + oPayoutRow.UserID).PadRight(20) + Convert.ToString("Time: " + Convert.ToDateTime(oPayoutRow.TransDate.ToString()).ToShortTimeString()).PadLeft(19) + " ");
            PrintLine("\n" + "Drawer: " + oPayoutRow.DrawNo + " ");
            PrintLine("\n\n" + "Amount Paid Out: " + Convert.ToDecimal(oPayoutRow.Amount.ToString()).ToString(Configuration.CInfo.CurrencySymbol.ToString() + "######0.00"));
            PrintLine("\n" + "Reference: " + oPayoutRow.Description.PadRight(25) + " ");

            PrintLine("\n\n\n");
            PrintLine("\x1B" + "J" + Convert.ToChar(250));
            PrintLine("\x1B" + "i");
        }

        public void PrintGiftRC() // Replace private to public for POSLITE
        {
            // this routine prints the dot matrix label
            sLabelFile = "GiftRECPT.js";

            if (System.IO.File.Exists(sLabelFile))
            {
                this.lhPrinter = new System.IntPtr();

                DOCINFO di = new DOCINFO();
                di.pDocName = "Label";
                di.pDataType = "RAW";

                // the \x1b means an ascii escape character
                //st1="\x1b*c600a6b0P\f";
                //lhPrinter contains the handle for the printer opened
                //If lhPrinter is 0 then an error has occured
                bool bSuccess = false;
                if (Configuration.CPOSSet.RP_Name.Trim() == "")
                {
                    bSuccess = true;
                } 
                else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
                {
                    if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                    {
                        if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                        {
                            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
                            try
                            {
                                _lbl.PrintCCRec();   // this will actually send the label printing lines
                                bSuccess = true;
                            } catch (Exception e)
                            {
                                clsCoreUIHelper.ShowBtnErrorMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK);//, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                PrintDirectAPIs.EndPagePrinter(lhPrinter);
                PrintDirectAPIs.EndDocPrinter(lhPrinter);
                PrintDirectAPIs.ClosePrinter(lhPrinter);
                if (bSuccess == false)
                {
                    clsCoreUIHelper.ShowErrorMsg("Unable to connect to printer");
                }
                return;
            }
        }

        #region Sprint-18 - 2039 31-Oct-2014 JY Added to print coupon receipt seperately
        public void PrintSeparateCouponRC() // Replace private to public for POSLITE
        {
            // this routine prints the dot matrix label
            sLabelFile = "PRCouponReceipt.js";

            if (System.IO.File.Exists(sLabelFile))
            {
                this.lhPrinter = new System.IntPtr();

                DOCINFO di = new DOCINFO();
                di.pDocName = "Label";
                di.pDataType = "RAW";

                // the \x1b means an ascii escape character
                //st1="\x1b*c600a6b0P\f";
                //lhPrinter contains the handle for the printer opened
                //If lhPrinter is 0 then an error has occured
                bool bSuccess = false;
                if (Configuration.CPOSSet.RP_Name.Trim() == "")
                {
                    bSuccess = true;
                } 
                else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
                {
                    if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                    {
                        if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                        {
                            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
                            try
                            {
                                _lbl.PrintCCRec();   // this will actually send the label printing lines
                                bSuccess = true;
                            } catch (Exception e)
                            {
                                clsCoreUIHelper.ShowBtnIconMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                PrintDirectAPIs.EndPagePrinter(lhPrinter);
                PrintDirectAPIs.EndDocPrinter(lhPrinter);
                PrintDirectAPIs.ClosePrinter(lhPrinter);
                if (bSuccess == false)
                {
                    clsCoreUIHelper.ShowErrorMsg("Unable to connect to printer");
                }
                return;
            }
        }
        #endregion

        public void PrintCC() // Replace private to public for POSLITE
        {
            // this routine prints the dot matrix label
            sLabelFile = "CCRECPT.js";

            if (System.IO.File.Exists(sLabelFile))
            {
                this.lhPrinter = new System.IntPtr();

                DOCINFO di = new DOCINFO();
                di.pDocName = "Label";
                di.pDataType = "RAW";

                // the \x1b means an ascii escape character
                //st1="\x1b*c600a6b0P\f";
                //lhPrinter contains the handle for the printer opened
                //If lhPrinter is 0 then an error has occured
                bool bSuccess = false;
                if (Configuration.CPOSSet.RP_Name.Trim() == "")
                {
                    bSuccess = true;
                } 
                else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
                {
                    if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                    {
                        if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                        {
                            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);

                            try
                            {
                                //for(int i=0; i < Configuration.CInfo.NoOfReceipt; i++)
                                //{
                                _lbl.PrintCCRec();   // this will actually send the label printing lines

                                //}
                                bSuccess = true;
                            } catch (Exception e)
                            {
                                clsCoreUIHelper.ShowBtnErrorMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK);//, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                PrintDirectAPIs.EndPagePrinter(lhPrinter);
                PrintDirectAPIs.EndDocPrinter(lhPrinter);
                PrintDirectAPIs.ClosePrinter(lhPrinter);
                if (bSuccess == false)
                {
                    clsCoreUIHelper.ShowErrorMsg("Unable to connect to printer");
                }
                return;
            }
        }

        //Added by shitaljit on 13 March 2012 for check payment reciept printing.
        /// <summary>
        /// Following function will print Check Payment reciept
        /// </summary>
        public void PrintCheck() //NileshJ - Change Access Modifier Private to public for POSLITE
        {
            // this routine prints the dot matrix label
            sLabelFile = "CheckRECPT.js";

            if (System.IO.File.Exists(sLabelFile))
            {
                this.lhPrinter = new System.IntPtr();

                DOCINFO di = new DOCINFO();
                di.pDocName = "Label";
                di.pDataType = "RAW";

                // the \x1b means an ascii escape character
                //st1="\x1b*c600a6b0P\f";
                //lhPrinter contains the handle for the printer opened
                //If lhPrinter is 0 then an error has occured
                bool bSuccess = false;
                if (Configuration.CPOSSet.RP_Name.Trim() == "")
                {
                    bSuccess = true;
                } 
                else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
                {
                    if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                    {
                        if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                        {
                            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);

                            try
                            {
                                _lbl.PrintCCRec();   // this will actually send the label printing lines
                                bSuccess = true;
                            } catch (Exception e)
                            {
                                clsCoreUIHelper.ShowBtnIconMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                PrintDirectAPIs.EndPagePrinter(lhPrinter);
                PrintDirectAPIs.EndDocPrinter(lhPrinter);
                PrintDirectAPIs.ClosePrinter(lhPrinter);
                if (bSuccess == false)
                {
                    clsCoreUIHelper.ShowErrorMsg("Unable to connect to printer");
                }
                return;
            }
        }

        public bool SetPrinter() //NileshJ - Change Access Modifier Private to public for POSLITE
        {
            bool PrinterSet = true;
            if (Configuration.CPOSSet.RP_Name.Trim().Length > 0)
            {
                mPD.PrinterSettings.PrinterName = Configuration.CPOSSet.RP_Name.Trim();
                #region PRIMEPOS-2996 23-Sep-2021 JY Added
                try
                {
                    if (Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim() != "")
                    {
                        System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                        foreach (System.Drawing.Printing.PaperSource ps in mPD.PrinterSettings.PaperSources)
                        {
                            if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim().ToUpper())
                            {
                                mPD.DefaultPageSettings.PaperSource = ps;
                                break;
                            }
                        }
                    }
                }
                catch { }
                #endregion
                /*
                if (!mPD.PrinterSettings.IsValid)
                {
                    if (PrinterSettings.InstalledPrinters.Count > 0)
                        mPD.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[0];
                }
                if (!mPD.PrinterSettings.IsValid)
                    PrinterSet=false;
                */
            } 
            else
                PrinterSet = false;
            return PrinterSet;
        }

        public bool BlankLineBeforeHeading
        {
            set { this.InsertBlankLine = value; }
            get { return this.InsertBlankLine; }
        }

        #region Laser Printing methods



        public void PrintS(string Text, long X, long Y)
        {
            PrintS(Text, X, Y, "H");
        }

        public void PrintS(string Text, long X, long Y, string PrintDirection, long lAngle) // Replace private to public for POSLITE
        {
            if (PrintDirection == "V")
                this.PrintVertical(Text, X, Y, lAngle);
            else
                this.PR.Graphics.DrawString(Text, CFont, CBrush, X, Y);
        }

        public void PrintS(string Text, long X, long Y, string PrintDirection) // Replace private to public for POSLITE
        {
            PrintS(Text, X, Y, PrintDirection, HorizontalAlignment.Left);
        }

        public void PrintS(string Text, long X, long Y, string PrintDirection, HorizontalAlignment oHorizontalAlignment) // Replace private to public for POSLITE
        {
            if (PrintDirection == "V")
                this.PrintVertical(Text, X, Y);
            else
            {
                if (oHorizontalAlignment == HorizontalAlignment.Center)
                {
                    System.Drawing.SizeF iTextSize = this.PR.Graphics.MeasureString(Text, CFont);
                    if (PageWidth > iTextSize.Width)
                    {
                        X = (long)((PageWidth - iTextSize.Width) / 2);
                    } else
                    {
                        X = 0;
                    }
                } else if (oHorizontalAlignment == HorizontalAlignment.Right)
                {
                    System.Drawing.SizeF iTextSize = this.PR.Graphics.MeasureString(Text, CFont);
                    if (PageWidth > iTextSize.Width)
                    {
                        X = (long)(PageWidth - iTextSize.Width);
                    } else
                    {
                        X = 0;
                    }
                }

                this.PR.Graphics.DrawString(Text, CFont, CBrush, X, Y);
            }
        }

        public void PrintSig(string Sig, Font font, Brush brush, long sX, long sY, long eX, long eY)
        {
            this.PR.Graphics.DrawString(Sig, font, brush, new RectangleF(sX, sY, eX, eY));
        }

        public void PrintSig(string Sig, Font font, Brush brush, long sX, long sY, long eX, long eY, string Direction)
        {
            StringFormat sf = new StringFormat();
            if (Direction.IndexOf("R") >= 0) // right to left
                sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            if (Direction.IndexOf("V") >= 0)
                this.PrintVerticalRect(Sig, sX, sY, eX, eY, 270, sf);
            else
                this.PR.Graphics.DrawString(Sig, font, brush, new RectangleF(sX, sY, eX, eY), sf);
        }

        public void PS(string Text, long X, long Y)
        {
            PrintS(Text, X, Y);
        }

        public void PS(string Text, long X, long Y, string PrintDirection)
        {
            PrintS(Text, X, Y, PrintDirection);
        }

        public void PS(string Text, long X, long Y, HorizontalAlignment oHorizontalAlignment)
        {
            PrintS(Text, X, Y, "H", oHorizontalAlignment);
        }

        public void PrintS(string Text, long X, long Y, long Size) // Replace private to public for POSLITE
        {
            CFont = new System.Drawing.Font(CFont.Name, Size);
            PrintS(Text, X, Y);
        }

        public void PSSize(string Text, long X, long Y, long Size)
        {
            PrintS(Text, X, Y, Size);
        }

        public void PrintS(string Text, long X, long Y, long Size, FontStyle Fs) // Replace private to public for POSLITE
        {
            CFont = new System.Drawing.Font(CFont.Name, Size, Fs);

            PrintS(Text, X, Y);
        }

        public void PSSS(string Text, long X, long Y, long Size, string Style)
        {
            PSSS(Text, X, Y, Size, Style, "H");
            /*
                        switch(Style)
                        {
                            case "B" :
                                PrintS(Text,X,Y,Size,FontStyle.Bold);
                                break;
                            case "R" :
                                PrintS(Text,X,Y,Size,FontStyle.Regular);
                                break;
                            case "I" :
                                PrintS(Text,X,Y,Size,FontStyle.Italic);
                                break;
                            case "U" :
                                PrintS(Text,X,Y,Size,FontStyle.Underline);
                                break;
                            case "S" :
                                PrintS(Text,X,Y,Size,FontStyle.Underline);
                                break;
                            default :
                                PrintS(Text,X,Y,Size,FontStyle.Regular);
                                break;
                        }
            */
        }

        public void PSSS(string Text, long X, long Y, long Size, string Style, string PrintDirection, long lAngle)
        {
            FontStyle Fs;
            switch (Style)
            {
                case "B":
                    Fs = FontStyle.Bold;
                    break;
                case "R":
                    Fs = FontStyle.Regular;
                    break;
                case "I":
                    Fs = FontStyle.Italic;
                    break;
                case "U":
                    Fs = FontStyle.Underline;
                    break;
                case "S":
                    Fs = FontStyle.Underline;
                    break;
                default:
                    Fs = FontStyle.Regular;
                    break;
            }

            CFont = new System.Drawing.Font(CFont.Name, Size, Fs);

            PrintS(Text, X, Y, PrintDirection, lAngle);
        }

        public void PSSS(string Text, long X, long Y, long Size, string Style, string PrintDirection)
        {
            FontStyle Fs;
            switch (Style)
            {
                case "B":
                    Fs = FontStyle.Bold;
                    break;
                case "R":
                    Fs = FontStyle.Regular;
                    break;
                case "I":
                    Fs = FontStyle.Italic;
                    break;
                case "U":
                    Fs = FontStyle.Underline;
                    break;
                case "S":
                    Fs = FontStyle.Underline;
                    break;
                default:
                    Fs = FontStyle.Regular;
                    break;
            }

            CFont = new System.Drawing.Font(CFont.Name, Size, Fs);

            PrintS(Text, X, Y, PrintDirection);
        }

        public void PrintS(string Text, long X, long Y, FontStyle Fs) // Replace private to public for POSLITE
        {
            CFont = new System.Drawing.Font(CFont, Fs);
            PrintS(Text, X, Y);
        }

        public void PSStyle(string Text, long X, long Y, string Style)
        {
            switch (Style)
            {
                case "B":
                    PrintS(Text, X, Y, FontStyle.Bold);
                    break;
                case "R":
                    PrintS(Text, X, Y, FontStyle.Regular);
                    break;
                case "I":
                    PrintS(Text, X, Y, FontStyle.Italic);
                    break;
                case "U":
                    PrintS(Text, X, Y, FontStyle.Underline);
                    break;
                case "S":
                    PrintS(Text, X, Y, FontStyle.Underline);
                    break;
                default:
                    PrintS(Text, X, Y, FontStyle.Regular);
                    break;
            }
        }

        public void SetFont(System.Drawing.Font font, long Size, FontStyle Fs)
        {
            this.CFont = new System.Drawing.Font(font.Name, Size, Fs);
        }

        public void SetFont(System.Drawing.Font font, FontStyle Fs)
        {
            this.CFont = new System.Drawing.Font(font, Fs);
        }

        public void SF(System.Drawing.Font font, long Size, string Style)
        {
            switch (Style)
            {
                case "B":
                    SetFont(font, Size, FontStyle.Bold);
                    break;
                case "R":
                    SetFont(font, Size, FontStyle.Regular);
                    break;
                case "I":
                    SetFont(font, Size, FontStyle.Italic);
                    break;
                case "U":
                    SetFont(font, Size, FontStyle.Underline);
                    break;
                case "S":
                    SetFont(font, Size, FontStyle.Strikeout);
                    break;
                default:
                    SetFont(font, Size, FontStyle.Regular);
                    break;
            }
        }

        public void PrintVerticalRect(string Ptext, long lX, long lY, long lEndX, long lEndY, long Angle, StringFormat sf)
        {
            PR.Graphics.TranslateTransform(lX, lY);
            PR.Graphics.RotateTransform(Angle); //,System.Drawing.Drawing2D.MatrixOrder.Append);
            PR.Graphics.DrawString(Ptext, this.CFont, this.CBrush, new RectangleF(0, 0, lEndY, lEndX), sf);
            PR.Graphics.ResetTransform();

        }


        public void DrawBox(int lnX, int lnY, int Width, int Height)
        {
            try
            {
                Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Graphics gBmp = Graphics.FromImage(bmp);
                gBmp.CompositingMode = CompositingMode.SourceCopy;

                Pen p = new Pen(Color.Black, 3);
                //p.
                Rectangle rctang = new Rectangle(2, 2, Width - 5, Height - 5);
                //rctang.
                gBmp.DrawRectangle(p, rctang);

                PR.Graphics.DrawImage(bmp, lnX, lnY, Width + 5, Height + 5);

            } catch (Exception ex)
            {
                throw (ex);
            }

        }


        public void PrintVerticalRect(string Ptext, long lX, long lY, long lEndX, long lEndY, long Angle)
        {
            PR.Graphics.TranslateTransform(lX, lY);
            PR.Graphics.RotateTransform(Angle); //,System.Drawing.Drawing2D.MatrixOrder.Append);
            PR.Graphics.DrawString(Ptext, this.CFont, this.CBrush, new RectangleF(0, 0, lEndY, lEndX));
            PR.Graphics.ResetTransform();
        }

        public void PrintVertical(string Ptext, long lX, long lY, long Angle)
        {
            PR.Graphics.TranslateTransform(lX, lY);
            PR.Graphics.RotateTransform(Angle); //,System.Drawing.Drawing2D.MatrixOrder.Append);
            PR.Graphics.DrawString(Ptext, this.CFont, this.CBrush, 0, 0);
            PR.Graphics.ResetTransform();
        }

        public void PrintVertical(string Ptext, long lX, long lY)
        {
            // default angle for vertical printing is 270

            PrintVertical(Ptext, lX, lY, 270);
        }

        public void PrintVerticalImg(Image bci, long lnX, long lnY, int Width, int Height)
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawImage(bci, 0, 0, Width, Height);
            //b.Save("c:\\test.bmp");
            //PR.Graphics.TranslateTransform(lnX, lnY);
            //PR.Graphics.RotateTransform(270); //,System.Drawing.Drawing2D.MatrixOrder.Append);
            PR.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            PR.Graphics.DrawImage(bci, lnX, lnY, Width, Height);
        }

        //Following Function is added By Shitaljit(QuicSolv) on 24 May 2011
        /// <summary>
        /// Prints the customer signature by creating image from the data store in DB.
        /// </summary>
        /// <param name="transID"></param>
        /// <param name="lnX"></param>
        /// <param name="lnY"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        //public void PrintSignature(int transID, long lnX, long lnY, int Width, int Height) //Commented For Aries8
        //{
        //    try
        //    {
        //        bool SignFlag = false;
        //        Bitmap bitmap = new Bitmap(Width, Height);
        //        Graphics graphics = Graphics.FromImage(bitmap);
        //        Bitmap myBitmap = new Bitmap(Width, Height);

        //        #region PRIMEPOS-2939 03-Mar-2021 JY Commented
        //        //POSTransPaymentData dsTransPayData = new POSTransPaymentData();
        //        //POSTransPaymentSvr oPOSTransSvr = new POSTransPaymentSvr();
        //        //dsTransPayData = oPOSTransSvr.PopulateSignaturedata(transID);
        //        //if (Configuration.isNullOrEmptyDataSet(dsTransPayData) == true)
        //        //{
        //        //    return;
        //        //}                
        //        //if (SignatureIndex > 0 && dsTransPayData.POSTransPayment.Rows.Count == 1)
        //        //{
        //        //    SignatureIndex = 0;
        //        //}
        //        //for (int Index = SignatureIndex; Index < dsTransPayData.POSTransPayment.Rows.Count; Index++)
        //        //{
        //        //    POSTransPaymentRow oRow = (POSTransPaymentRow)dsTransPayData.POSTransPayment.Rows[SignatureIndex];
        //        //    SignFlag = false;
        //        //    if (oRow["SigType"].ToString() == "D")
        //        //    {
        //        //        string strSignData = string.Empty;
        //        //        strSignData = oRow["CustomerSign"].ToString();
        //        //        if (strSignData != "")
        //        //        {
        //        //            myBitmap = clsCoreUIHelper.GetSignature(strSignData, "D");
        //        //        }
        //        //        else
        //        //        {
        //        //            SignFlag = true;
        //        //        }
        //        //        SignatureIndex++;
        //        //        break;
        //        //    }
        //        //    else if (oRow["SigType"].ToString() == "M")
        //        //    {
        //        //        Byte[] strBinSign = null;
        //        //        if (oRow["BinarySign"] != null)
        //        //        {
        //        //            strBinSign = (byte[])oRow["BinarySign"];
        //        //            if (strBinSign != null)
        //        //            {
        //        //                MemoryStream ms = new MemoryStream(strBinSign);
        //        //                //PRIMEPOS-2636 ADDED BY ARVIND 
        //        //                //if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")
        //        //                if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") || (bPrintDuplicateReceipt && tmpPaymentProcessor == "VANTIV"))   //PRIMEPOS-2876 27-Jul-2020 JY modified
        //        //                {
        //        //                    try
        //        //                    {
        //        //                        myBitmap = clsCoreUIHelper.ConvertPoints(strBinSign);
        //        //                    }
        //        //                    catch { }
        //        //                }
        //        //                else
        //        //                    myBitmap = new Bitmap(ms);
        //        //                //
        //        //            }
        //        //            else
        //        //            {
        //        //                SignFlag = true;
        //        //            }
        //        //        }
        //        //        SignatureIndex++;
        //        //        break;
        //        //    }
        //        //    SignatureIndex++;
        //        //}
        //        #endregion

        //        #region PRIMEPOS-2939 03-Mar-2021 JY Added
        //        POSTransPaymentData oPOSTransPaymentData = new POSTransPaymentData();
        //        POSTransPaymentSvr oPOSTransPaymentSvr = new POSTransPaymentSvr();
        //        oPOSTransPaymentData = oPOSTransPaymentSvr.PopulateSignaturedata(transID, nTransPayID);
        //        if (oPOSTransPaymentData != null && oPOSTransPaymentData.Tables.Count > 0 && oPOSTransPaymentData.POSTransPayment.Rows.Count > 0)
        //        {
        //            POSTransPaymentRow oRow = (POSTransPaymentRow)oPOSTransPaymentData.POSTransPayment.Rows[0];
        //            SignFlag = false;
        //            if (oRow["SigType"].ToString() == "D")
        //            {
        //                string strSignData = string.Empty;
        //                strSignData = oRow["CustomerSign"].ToString();
        //                if (strSignData != "")
        //                {
        //                    myBitmap = clsCoreUIHelper.GetSignature(strSignData, "D");
        //                }
        //                else
        //                {
        //                    SignFlag = true;
        //                }
        //            }
        //            else if (oRow["SigType"].ToString() == "M")
        //            {
        //                Byte[] strBinSign = null;
        //                if (oRow["BinarySign"] != null)
        //                {
        //                    strBinSign = (byte[])oRow["BinarySign"];
        //                    if (strBinSign != null)
        //                    {
        //                        MemoryStream ms = new MemoryStream(strBinSign);
        //                        //PRIMEPOS-2636 ADDED BY ARVIND 
        //                        //if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")
        //                        if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") || (bPrintDuplicateReceipt && tmpPaymentProcessor == "VANTIV"))   //PRIMEPOS-2876 27-Jul-2020 JY modified
        //                        {
        //                            try
        //                            {
        //                                myBitmap = clsCoreUIHelper.ConvertPoints(strBinSign);
        //                            }
        //                            catch { }
        //                        }
        //                        else
        //                            myBitmap = new Bitmap(ms);
        //                    }
        //                    else
        //                    {
        //                        SignFlag = true;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }
        //        #endregion

        //        Bitmap SignImage = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format32bppArgb);
        //        SignImage = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format32bppArgb);
        //        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //        graphics.DrawImage(SignImage, 0, 0, Width, Height);
        //        PR.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //        PR.Graphics.DrawImage(SignImage, lnX, lnY, Width, Height);

        //        if (SignFlag)
        //        {
        //            clsCoreUIHelper.ShowWarningMsg2("No Signature Present For The Selected User");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsCoreUIHelper.ShowErrorMsg(ex.Message);
        //    }
        //}
        //Till here added By Shitaljit(QuicSolv)

        #region PRIMEPOS-2939 02-Mar-2021 JY Added but not in use
        //public void PrintSignature(int transID, int TransPayID, long lnX, long lnY, int Width, int Height)
        //{
        //    try
        //    {
        //        bool SignFlag = false;
        //        Bitmap bitmap = new Bitmap(Width, Height);
        //        Graphics graphics = Graphics.FromImage(bitmap);
        //        Bitmap myBitmap = new Bitmap(Width, Height);
        //        POSTransPaymentData oPOSTransPaymentData = new POSTransPaymentData();
        //        POSTransPaymentSvr oPOSTransPaymentSvr = new POSTransPaymentSvr();
        //        oPOSTransPaymentData = oPOSTransPaymentSvr.PopulateSignaturedata(transID, TransPayID);
        //        if (oPOSTransPaymentData != null && oPOSTransPaymentData.Tables.Count > 0 && oPOSTransPaymentData.POSTransPayment.Rows.Count > 0)
        //        {
        //            POSTransPaymentRow oRow = (POSTransPaymentRow)oPOSTransPaymentData.POSTransPayment.Rows[0];
        //            SignFlag = false;
        //            if (oRow["SigType"].ToString() == "D")
        //            {
        //                string strSignData = string.Empty;
        //                strSignData = oRow["CustomerSign"].ToString();
        //                if (strSignData != "")
        //                {
        //                    myBitmap = clsCoreUIHelper.GetSignature(strSignData, "D");
        //                }
        //                else
        //                {
        //                    SignFlag = true;
        //                }
        //            }
        //            else if (oRow["SigType"].ToString() == "M")
        //            {
        //                Byte[] strBinSign = null;
        //                if (oRow["BinarySign"] != null)
        //                {
        //                    strBinSign = (byte[])oRow["BinarySign"];
        //                    if (strBinSign != null)
        //                    {
        //                        MemoryStream ms = new MemoryStream(strBinSign);
        //                        //PRIMEPOS-2636 ADDED BY ARVIND 
        //                        //if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")
        //                        if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") || (bPrintDuplicateReceipt && tmpPaymentProcessor == "VANTIV"))   //PRIMEPOS-2876 27-Jul-2020 JY modified
        //                        {
        //                            try
        //                            {
        //                                myBitmap = clsCoreUIHelper.ConvertPoints(strBinSign);
        //                            }
        //                            catch { }
        //                        }
        //                        else
        //                            myBitmap = new Bitmap(ms);
        //                    }
        //                    else
        //                    {
        //                        SignFlag = true;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        Bitmap SignImage = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format32bppArgb);
        //        SignImage = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format32bppArgb);
        //        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //        graphics.DrawImage(SignImage, 0, 0, Width, Height);
        //        PR.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        //        PR.Graphics.DrawImage(SignImage, lnX, lnY, Width, Height);

        //        if (SignFlag)
        //        {
        //            clsCoreUIHelper.ShowWarningMsg2("No Signature Present For The Selected User");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsCoreUIHelper.ShowErrorMsg(ex.Message);
        //    }
        //}
        #endregion

        public void PrintSignature(int transID, long lnX, long lnY, int Width, int Height)  //PRIMEPOS-2952
        {
            try
            {
                bool SignFlag = false;
                Bitmap bitmap = new Bitmap(Width, Height);
                Graphics graphics = Graphics.FromImage(bitmap);
                Bitmap myBitmap = new Bitmap(Width, Height);
                POSTransPaymentData dsTransPayData = new POSTransPaymentData();
                POSTransPaymentSvr oPOSTransSvr = new POSTransPaymentSvr();
                dsTransPayData = oPOSTransSvr.PopulateSignaturedata(transID);
                if (Configuration.isNullOrEmptyDataSet(dsTransPayData) == true)
                {
                    return;
                }
                if (SignatureIndex > 0 && dsTransPayData.POSTransPayment.Rows.Count == 1)
                {
                    SignatureIndex = 0;
                }
                for (int Index = SignatureIndex; Index < dsTransPayData.POSTransPayment.Rows.Count; Index++)
                {
                    POSTransPaymentRow oRow = (POSTransPaymentRow)dsTransPayData.POSTransPayment.Rows[SignatureIndex];
                    SignFlag = false;
                    if (oRow["SigType"].ToString() == "D")
                    {
                        string strSignData = string.Empty;
                        strSignData = oRow["CustomerSign"].ToString();
                        if (strSignData != "")
                        {
                            myBitmap = clsCoreUIHelper.GetSignature(strSignData, "D");
                        }
                        else
                        {
                            SignFlag = true;
                        }
                        SignatureIndex++;
                        break;
                    }
                    else if (oRow["SigType"].ToString() == "M")
                    {
                        Byte[] strBinSign = null;
                        if (oRow["BinarySign"] != null)
                        {
                            strBinSign = (byte[])oRow["BinarySign"];
                            if (strBinSign != null)
                            {
                                string strsign = System.Text.Encoding.Default.GetString(strBinSign);
                                MemoryStream ms = new MemoryStream(strBinSign);
                                //PRIMEPOS-2636 ADDED BY ARVIND 
                                //if (Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV")
                                if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") || (bPrintDuplicateReceipt && tmpPaymentProcessor == "VANTIV")  || (bPrintDuplicateReceipt && tmpPaymentProcessor == "NB_VANTIV"))   //PRIMEPOS-2876 27-Jul-2020 JY modified //PRIMEPOS-3482
                                {
                                    try
                                    {
                                        myBitmap = clsCoreUIHelper.ConvertPoints(strBinSign);
                                    }
                                    catch { }
                                }
                                //else if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "HPSPAX") || (bPrintDuplicateReceipt && tmpPaymentProcessor == "HPSPAX"))   //PRIMEPOS-2876 27-Jul-2020 JY modified  //PRIMEPOS-2952 Commented for Signature Issue Aries8
                                //{
                                //    try  //PRIMEPOS - 2952
                                //    {
                                //        string oError = string.Empty;   
                                //        byte[] btarr = null;
                                //        myBitmap = clsCoreUIHelper.GetSignaturePAX(strsign, out oError, "M", out btarr);

                                //    }
                                //    catch
                                //    {

                                //    }

                                //}
                                else
                                    myBitmap = new Bitmap(ms);
                                //
                            }
                            else
                            {
                                SignFlag = true;
                            }
                        }
                        SignatureIndex++;
                        break;
                    }
                    SignatureIndex++;
                }
                Bitmap SignImage = new Bitmap(myBitmap.Width, myBitmap.Height, PixelFormat.Format32bppArgb);
                SignImage = (Bitmap)myBitmap.Clone(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), PixelFormat.Format32bppArgb);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.DrawImage(SignImage, 0, 0, Width, Height);
                PR.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                PR.Graphics.DrawImage(SignImage, lnX, lnY, Width, Height);

                if (SignFlag)
                {
                    clsCoreUIHelper.ShowWarningMsg2("No Signature Present For The Selected User");
                }

            }
            catch (Exception ex)
            {
                clsCoreUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        public void PrintImage(Image oImage, int iLeft, int iTop, int iWidth, int iHeight)
        {
            PR.Graphics.DrawImage(oImage, iLeft, iTop, iWidth, iHeight);
        }

        #endregion

        #region PRIMEPOS-2876 27-Jul-2020 JY Added
        public string GetPaymentProcessor(POSTransPaymentData oPaymentData)
        {
            string strPaymentProcessor = string.Empty;
            try
            {
                if (oPaymentData != null && oPaymentData.Tables.Count > 0 && oPaymentData.Tables[0].Rows.Count > 0)
                {
                    foreach (POSTransPaymentRow oRow in oPaymentData.POSTransPayment.Rows)
                    {
                        if (Configuration.convertNullToString(oRow.PaymentProcessor).Trim() != "" && oRow.PaymentProcessor.Trim() != "N/A")
                        {
                            strPaymentProcessor = oRow.PaymentProcessor.Trim().ToUpper();
                            break;
                        }
                    }
                    if (strPaymentProcessor == "") strPaymentProcessor = Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper();
                }
            }
            catch(Exception Ex)
            {
                return Configuration.CPOSSet.PaymentProcessor.Trim().ToUpper();
            }
            return strPaymentProcessor;
        }
        #endregion

        #region PRIMEPOS-2939 03-Mar-2021 JY Added
        private void SetParameters(POSTransPaymentData oTransPaymentData, POSTransPaymentRow oRow, Boolean bCC)
        {
            try
            {
                string tmpPaymentProcessor = GetPaymentProcessor(oTransPaymentData);
                PaymentProcessor = tmpPaymentProcessor;
                //Added by Arvind PRIMEPOS-2664
                if (bCC == true && oCCPaymentRow != null && tmpPaymentProcessor == "EVERTEC")
                {
                    AuthNo = oCCPaymentRow.AuthNo;
                    ReferenceNumber = oCCPaymentRow.ProcessorTransID;
                    Trace = oCCPaymentRow.TraceNumber;
                    Batch = oCCPaymentRow.BatchNumber;
                    MerchantID = oCCPaymentRow.MerchantID;
                    InvoiceNumber = oCCPaymentRow.InvoiceNumber;
                    PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                    TerminalID = Configuration.CPOSSet.TerminalID;
                    IsEvertecForceTransaction = oCCPaymentRow.IsEvertecForceTransaction; //primepos-2831
                    IsEvertecSign = oCCPaymentRow.IsEvertecSign; //primepos-2831
                    if (!string.IsNullOrWhiteSpace(oCCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                    {
                        EvertecStateTax = oCCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        EvertecCityTax = oCCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                    }
                    EvertecCashback = oCCPaymentRow.CashBack;//PRIMEPOS-2857
                    EntryMethod = oCCPaymentRow.EntryMethod;//PRIMEPOS-2857
                    ControlNumber = oCCPaymentRow.ControlNumber;
                    if (!string.IsNullOrWhiteSpace(oCCPaymentRow.ATHMovil))
                        ATHMovil = oCCPaymentRow.ATHMovil.Substring(2, oCCPaymentRow.ATHMovil.Length - 2);//2664
                }
                else if (bCC == false && oEBTPaymentRow != null && tmpPaymentProcessor == "EVERTEC")
                {
                    AuthNo = oEBTPaymentRow.AuthNo;
                    ReferenceNumber = oEBTPaymentRow.ProcessorTransID;
                    Trace = oEBTPaymentRow.TraceNumber;
                    Batch = oEBTPaymentRow.BatchNumber;
                    MerchantID = oEBTPaymentRow.MerchantID;
                    InvoiceNumber = oEBTPaymentRow.InvoiceNumber;
                    PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                    TerminalID = Configuration.CPOSSet.TerminalID;
                    #region PRIMEPOS-2664 EVERTEC EBTBALANCE
                    if (oEBTPaymentRow.EbtBalance.Length >= 3)
                    {
                        FoodBalance = oEBTPaymentRow.EbtBalance.Split('|')[0];
                        CashBalance = oEBTPaymentRow.EbtBalance.Split('|')[1];
                    }
                    #endregion
                    IsEvertecForceTransaction = oEBTPaymentRow.IsEvertecForceTransaction; //primepos-2831
                    IsEvertecSign = oEBTPaymentRow.IsEvertecSign; //primepos-2831
                    if (!string.IsNullOrWhiteSpace(oEBTPaymentRow.EvertecTaxBreakdown))//PRIMEPOS-2857
                    {
                        EvertecStateTax = oEBTPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        EvertecCityTax = oEBTPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                    }
                    EvertecCashback = oEBTPaymentRow.CashBack;//PRIMEPOS-2857
                    ControlNumber = oEBTPaymentRow.ControlNumber;//PRIMEPOS-2857
                    if (!string.IsNullOrWhiteSpace(oEBTPaymentRow.ATHMovil))
                        ATHMovil = oEBTPaymentRow.ATHMovil.Substring(2, oEBTPaymentRow.ATHMovil.Length - 2);//2664
                }                
                if(tmpPaymentProcessor == "EVERTEC")//2664
                {
                    if (CashPaymentRow != null)
                    {
                        AuthNo = CashPaymentRow.AuthNo;
                        ReferenceNumber = CashPaymentRow.ProcessorTransID;
                        Trace = CashPaymentRow.TraceNumber;
                        Batch = CashPaymentRow.BatchNumber;
                        MerchantID = CashPaymentRow.MerchantID;
                        InvoiceNumber = CashPaymentRow.InvoiceNumber;
                        PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                        TerminalID = Configuration.CPOSSet.TerminalID;
                        IsEvertecForceTransaction = CashPaymentRow.IsEvertecForceTransaction; //primepos-2831
                        IsEvertecSign = CashPaymentRow.IsEvertecSign; //primepos-2831
                        ControlNumber = CashPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(CashPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            EvertecStateTax = CashPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            EvertecCityTax = CashPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        EntryMethod = CashPaymentRow.EntryMethod;//PRIMEPOS-2857
                        ControlNumber = CashPaymentRow.ControlNumber;//PRIMEPOS-2857
                        EvertecCashback = CashPaymentRow.CashBack;//PRIMEPOS-2857
                        if (!string.IsNullOrWhiteSpace(CashPaymentRow.ATHMovil))
                            ATHMovil = CashPaymentRow.ATHMovil.Substring(2, CashPaymentRow.ATHMovil.Length - 2);//2664
                    }
                    else if (CheckPaymentRow != null)
                    {
                        ControlNumber = CheckPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(CheckPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            EvertecStateTax = CheckPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            EvertecCityTax = CheckPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        EntryMethod = CheckPaymentRow.EntryMethod;//PRIMEPOS-2857
                        ControlNumber = CheckPaymentRow.ControlNumber;//PRIMEPOS-2857
                        ATHMovil = CheckPaymentRow.ATHMovil;//2664
                    }
                    else if (CouponPaymentRow != null)
                    {
                        if (!string.IsNullOrWhiteSpace(CouponPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            EvertecStateTax = CouponPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            EvertecCityTax = CouponPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        EntryMethod = CouponPaymentRow.EntryMethod;//PRIMEPOS-2857
                        ControlNumber = CouponPaymentRow.ControlNumber;//PRIMEPOS-2857
                        ATHMovil = CouponPaymentRow.ATHMovil;//2664
                    }
                    else if (HCPaymentRow != null)
                    {
                        ControlNumber = HCPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(HCPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            EvertecStateTax = HCPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            EvertecCityTax = HCPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        EntryMethod = HCPaymentRow.EntryMethod;//PRIMEPOS-2857
                        ControlNumber = HCPaymentRow.ControlNumber;//PRIMEPOS-2857
                        ATHMovil = HCPaymentRow.ATHMovil;//2664
                    }
                    else if (CBPaymentRow != null)
                    {
                        ControlNumber = CBPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(CBPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            EvertecStateTax = CBPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                            EvertecCityTax = CBPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                        }
                        EntryMethod = CBPaymentRow.EntryMethod;//PRIMEPOS-2857
                        ControlNumber = CBPaymentRow.ControlNumber;//PRIMEPOS-2857
                        ATHMovil = CBPaymentRow.ATHMovil;//2664
                    }
                    if (ATHPaymentRow != null)
                    {
                        AuthNo = ATHPaymentRow.AuthNo;
                        ReferenceNumber = ATHPaymentRow.ProcessorTransID;
                        Trace = ATHPaymentRow.TraceNumber;
                        Batch = ATHPaymentRow.BatchNumber;
                        MerchantID = ATHPaymentRow.MerchantID;
                        InvoiceNumber = ATHPaymentRow.InvoiceNumber;
                        PaymentProcessor = Configuration.CPOSSet.PaymentProcessor;
                        TerminalID = Configuration.CPOSSet.TerminalID;
                        IsEvertecForceTransaction = ATHPaymentRow.IsEvertecForceTransaction; //primepos-2831
                        IsEvertecSign = ATHPaymentRow.IsEvertecSign; //primepos-2831
                        if (!string.IsNullOrWhiteSpace(ATHPaymentRow.EvertecTaxBreakdown))//PPRIMEPOS-2857
                        {
                            EvertecStateTax = ATHPaymentRow.EvertecTaxBreakdown.Split('|')[1];
                            EvertecCityTax = ATHPaymentRow.EvertecTaxBreakdown.Split('|')[0];
                        }
                        EvertecCashback = ATHPaymentRow.CashBack;//PRIMEPOS-2857
                        EntryMethod = ATHPaymentRow.EntryMethod;//PRIMEPOS-2857
                        ControlNumber = ATHPaymentRow.ControlNumber;
                        if (!string.IsNullOrWhiteSpace(ATHPaymentRow.ATHMovil))
                            ATHMovil = ATHPaymentRow.ATHMovil.Substring(2, ATHPaymentRow.ATHMovil.Length - 2);//2664
                    }
                }
                //Added by Arvind PRIMEPOS-2636
                else if (bCC == true && oCCPaymentRow != null && tmpPaymentProcessor == "VANTIV")
                {
                    ReferenceNumber = oCCPaymentRow.ReferenceNumber;
                    MerchantID = oCCPaymentRow.MerchantID;
                    PaymentProcessor = tmpPaymentProcessor;
                    TerminalID = oCCPaymentRow.TerminalID;
                    TransactionID = oCCPaymentRow.TransactionID;
                    ResponseCode = oCCPaymentRow.ResponseCode;
                    Aid = oCCPaymentRow.Aid;
                    Cryptogram = oCCPaymentRow.Cryptogram;
                    EntryMethod = oCCPaymentRow.EntryMethod;
                    ApprovalCode = oCCPaymentRow?.ApprovalCode;
                    #region PRIMEPOS-2793
                    ApplicationLabel = oCCPaymentRow?.ApplicaionLabel;
                    PinVerified = oCCPaymentRow.PinVerified;
                    LaneID = oCCPaymentRow.LaneID;
                    CardLogo = oCCPaymentRow.CardLogo;
                    #endregion
                }                
                else if (bCC == false && oEBTPaymentRow != null && tmpPaymentProcessor == "VANTIV")
                {
                    ReferenceNumber = oEBTPaymentRow.ReferenceNumber;
                    MerchantID = oEBTPaymentRow.MerchantID;
                    PaymentProcessor = tmpPaymentProcessor;
                    TerminalID = oEBTPaymentRow.TerminalID;
                    TransactionID = oEBTPaymentRow.TransactionID;
                    ResponseCode = oEBTPaymentRow.ResponseCode;
                    Aid = oEBTPaymentRow.Aid;
                    Cryptogram = oEBTPaymentRow.Cryptogram;
                    EntryMethod = oEBTPaymentRow.EntryMethod;
                    ApprovalCode = oEBTPaymentRow?.ApprovalCode;
                    #region PRIMEPOS-2793
                    ApplicationLabel = oEBTPaymentRow?.ApplicaionLabel;
                    PinVerified = oEBTPaymentRow.PinVerified;
                    LaneID = oEBTPaymentRow.LaneID;
                    CardLogo = oEBTPaymentRow.CardLogo;
                    #endregion
                }
                else if (oCashPaymentRow != null && tmpPaymentProcessor == "VANTIV")
                {
                    ReferenceNumber = oCashPaymentRow.ReferenceNumber;
                    MerchantID = oCashPaymentRow.MerchantID;
                    PaymentProcessor = tmpPaymentProcessor;
                    TerminalID = oCashPaymentRow.TerminalID;
                    TransactionID = oCashPaymentRow.TransactionID;
                    ResponseCode = oCashPaymentRow.ResponseCode;
                    Aid = oCashPaymentRow.Aid;
                    Cryptogram = oCashPaymentRow.Cryptogram;
                    EntryMethod = oCashPaymentRow.EntryMethod.ToUpper();
                    #region PRIMEPOS-2793
                    ApplicationLabel = oCashPaymentRow?.ApplicaionLabel;
                    PinVerified = oCashPaymentRow.PinVerified;
                    LaneID = oCashPaymentRow.LaneID;
                    CardLogo = oCashPaymentRow.CardLogo;
                    #endregion
                }
                else if (Configuration.CPOSSet.PaymentProcessor == "ELAVON")
                {
                    if (oCashPaymentRow != null)
                    {
                        Aid = oCashPaymentRow.Aid;
                        Cryptogram = oCashPaymentRow.Cryptogram;
                        EntryMethod = oCashPaymentRow.EntryMethod.ToUpper();
                        //ApplicationLabel = ocsh
                    }
                    if (oCCPaymentRow != null)
                    {
                        Aid = oCCPaymentRow.Aid;
                        Cryptogram = oCCPaymentRow.Cryptogram;
                        EntryMethod = oCCPaymentRow.EntryLegend;
                        if (oCCPaymentRow.ApplicaionLabel.Contains("|"))
                        {
                            ApplicationLabel = "AppLabel : " + oCCPaymentRow.ApplicaionLabel.Split('|')[0];
                            TC += "TC : " + oCCPaymentRow.ApplicaionLabel.Split('|')[1];
                            IAD += "IAD : " + oCCPaymentRow.ApplicaionLabel.Split('|')[2];
                            //ApplicationLabel = oCCPaymentRow.ApplicaionLabel;
                        }
                        TVR = oCCPaymentRow.TerminalTvr;
                        ApprovalCode = oCCPaymentRow.ApprovalCode;
                        ReferenceNumber = oCCPaymentRow.ReferenceNumber;
                        MerchantID = oCCPaymentRow.MerchantID;
                    }
                }

                try
                {
                    if (oRow != null)
                    {
                        if (Configuration.convertNullToString(oRow.PaymentProcessor).Trim() != "" && oRow.PaymentProcessor.Trim() != "N/A")
                        {
                            TicketNumber = Configuration.convertNullToString(oRow.TicketNumber).Trim();
                            if (ReferenceNumber.Trim() == "") ReferenceNumber = Configuration.convertNullToString(oRow.ReferenceNumber).Trim();
                            if (TransactionID.Trim() == "") TransactionID = Configuration.convertNullToString(oRow.TransactionID).Trim();
                            if (ControlNumber.Trim() == "") ControlNumber = Configuration.convertNullToString(oRow.ControlNumber).Trim();
                        }
                    }
                }
                catch { }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
    }

    //public class BarcodeLabel
    //{
    //    protected BarcodeLabelDataCollection oBarcodeDataColl;

    //    string sLabelFile = "ItemBarCodeLabel.js";
    //    private System.Drawing.Font CFont;
    //    private System.Drawing.Brush CBrush;

    //    private int iLabelWidth = 250;
    //    private int iLabelHeight = 200;

    //    private int loopCounter = 0;

    //    private System.Drawing.Printing.PrintDocument oPrintDocument;

    //    public int LabelWidth
    //    {
    //        set
    //        {
    //            iLabelWidth = value;
    //            oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
    //        }
    //    }

    //    public int LabelHeight
    //    {
    //        set
    //        {
    //            iLabelHeight = value;
    //            oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
    //        }
    //    }

    //    public PrintDocument PrintDocument
    //    {
    //        get { return oPrintDocument; }
    //    }

    //    public BarcodeLabel(BarcodeLabelDataCollection oBarcodeColl)
    //    {
    //        this.oBarcodeDataColl = oBarcodeColl;
    //        // TODO: Add constructor logic here
    //        //
    //        InitClass();
    //    }

    //    public BarcodeLabelData CurrentBarcodeLabelData
    //    {
    //        get
    //        {
    //            try
    //            {
    //                return this.oBarcodeDataColl[loopCounter];
    //            } catch
    //            { return null; }
    //        }
    //    }

    //    private Graphics printerHandle = null;

    //    private void InitClass()
    //    {
    //        this.CFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
    //        this.CBrush = new System.Drawing.SolidBrush(Color.Black);
    //    }

    //    public void Print()
    //    {
    //        // this routine prints the dot matrix label

    //        if (System.IO.File.Exists(sLabelFile))
    //        {
    //            bool bSuccess = false;
    //            if (Configuration.CPOSSet.LabelPrinter.Trim() == "")
    //            {
    //                return;
    //            }

    //            try
    //            {
    //                oPrintDocument = new PrintDocument();
    //                oPrintDocument.DefaultPageSettings.PrinterSettings.PrinterName = Configuration.CPOSSet.LabelPrinter;
    //                //oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
    //                //oPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
    //                oPrintDocument.PrintPage += new PrintPageEventHandler(oPrintDocument_PrintPage);

    //                /*PrintPreviewDialog pd = new PrintPreviewDialog();
    //                pd.Document = oPrintDocument;

    //                pd.ShowDialog();
    //                return;
    //                */

    //                loopCounter = 0;
    //                for (; loopCounter < oBarcodeDataColl.Count; loopCounter++)
    //                {
    //                    oPrintDocument.Print();
    //                }
    //                //}
    //                bSuccess = true;
    //            } catch (Exception e)
    //            {
    //                clsCoreUIHelper.ShowBtnIconMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            }
    //        }
    //    }

    //    private void oPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
    //    {
    //        printerHandle = e.Graphics;
    //        ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
    //        _lbl.PrintCLabel();   // this will actually send the label printing lines
    //    }

    //    public void SetFont(string fontName, long fontSize, bool bold)
    //    {
    //        if (bold == true)
    //        {
    //            this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Bold);
    //        } else
    //        {
    //            this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Regular);
    //        }
    //    }

    //    public void SetFont(string fontName, long fontSize)
    //    {
    //        SetFont(fontName, fontSize, false);
    //    }

    //    public void PrintLine(string printData, int iLeft, int iTop)
    //    {
    //        this.printerHandle.DrawString(printData, CFont, CBrush, iLeft, iTop, new StringFormat());
    //    }

    //    public void PrintImage(Image oImage, int iLeft, int iTop, int iWidth, int iHeight)
    //    {
    //        this.printerHandle.DrawImage(oImage, iLeft, iTop, iWidth, iHeight);
    //    }
    //}

    public class CustomerLabel
    {
        protected CustomerRow oCustomerRow;
        protected Image oImage;

        string sLabelFile = "CustomerBarCodeLabel.js";
        private System.Drawing.Font CFont;
        private System.Drawing.Brush CBrush;

        private int iLabelWidth = 250;
        private int iLabelHeight = 200;

        private int loopCounter = 0;

        private System.Drawing.Printing.PrintDocument oPrintDocument;

        public int LabelWidth
        {
            set
            {
                iLabelWidth = value;
                oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
            }
        }

        public int LabelHeight
        {
            set
            {
                iLabelHeight = value;
                oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
            }
        }

        public PrintDocument PrintDocument
        {
            get { return oPrintDocument; }
        }

        public CustomerLabel(CustomerRow oCRow, Image oCImage)
        {
            this.oCustomerRow = oCRow;
            this.oImage = oCImage;
            //
            // TODO: Add constructor logic here
            //
            InitClass();
        }

        public CustomerRow CurrentCustomer
        {
            get
            {
                try
                {
                    return this.oCustomerRow;
                }
                catch
                { return null; }
            }
        }

        public Image CustomerBarCode
        {
            get
            {
                return this.oImage;
            }
        }

        private Graphics printerHandle = null;

        private void InitClass()
        {
            this.CFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
            this.CBrush = new System.Drawing.SolidBrush(Color.Black);
        }

        public void Print()
        {
            // this routine prints the dot matrix label

            if(System.IO.File.Exists(sLabelFile))
            {
                bool bSuccess = false;
                if(Configuration.CPOSSet.LabelPrinter.Trim() == "")
                {
                    return;
                }

                try
                {
                    oPrintDocument = new PrintDocument();
                    oPrintDocument.DefaultPageSettings.PrinterSettings.PrinterName = Configuration.CPOSSet.LabelPrinter;
                    #region PRIMEPOS-2996 23-Sep-2021 JY Added
                    try
                    {
                        if (Configuration.CPOSSet.LabelPrinterPaperSource.Trim() != "")
                        {
                            System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                            foreach (System.Drawing.Printing.PaperSource ps in oPrintDocument.PrinterSettings.PaperSources)
                            {
                                if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.LabelPrinterPaperSource.Trim().ToUpper())
                                {
                                    oPrintDocument.DefaultPageSettings.PaperSource = ps;
                                    break;
                                }
                            }
                        }
                    }
                    catch { }
                    #endregion
                    //oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
                    oPrintDocument.PrintPage += new PrintPageEventHandler(oPrintDocument_PrintPage);

                    /*PrintPreviewDialog pd = new PrintPreviewDialog();
                    pd.Document = oPrintDocument;

                    pd.ShowDialog();
                    return;
                    */

                    oPrintDocument.Print();
                    //}
                    bSuccess = true;
                }
                catch(Exception e)
                {
                    clsCoreUIHelper.ShowBtnIconMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void oPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            printerHandle = e.Graphics;
            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
            _lbl.PrintCLabel();   // this will actually send the label printing lines
        }

        public void SetFont(string fontName, long fontSize, bool bold)
        {
            if(bold == true)
            {
                this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Bold);
            }
            else
            {
                this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Regular);
            }
        }

        public void SetFont(string fontName, long fontSize)
        {
            SetFont(fontName, fontSize, false);
        }

        public void PrintLine(string printData, int iLeft, int iTop)
        {
            this.printerHandle.DrawString(printData, CFont, CBrush, iLeft, iTop, new StringFormat());
        }

        public void PrintImage(Image oImage, int iLeft, int iTop, int iWidth, int iHeight)
        {
            this.printerHandle.DrawImage(oImage, iLeft, iTop, iWidth, iHeight);
        }
    }

    public class CustomerLoyaltyCard
    {
        protected CLCardsRow oCLCardsRow;
        private CustomerRow oCustomerRow;
        protected Image oImage;

        string sLabelFile = "CustomerLoyaltyCardLabel.js";
        private System.Drawing.Font CFont;
        private System.Drawing.Brush CBrush;

        private int iLabelWidth = 250;
        private int iLabelHeight = 200;

        private int loopCounter = 0;

        private System.Drawing.Printing.PrintDocument oPrintDocument;

        public int LabelWidth
        {
            set
            {
                iLabelWidth = value;
                oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
            }
        }

        public int LabelHeight
        {
            set
            {
                iLabelHeight = value;
                oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
            }
        }

        public CustomerLoyaltyCard(CLCardsRow oCLRow, Image oCImage)
        {
            this.oCLCardsRow = oCLRow;

            this.oImage = oCImage;
            //
            // TODO: Add constructor logic here
            //
            InitClass();

            Customer oCustomer = new Customer();
            CustomerData oCData= oCustomer.GetCustomerByID(oCLCardsRow.CustomerID);
            if(oCData.Customer.Rows.Count > 0)
            {
                oCustomerRow = oCData.Customer[0];
            }
        }

        public CustomerRow CLCardCustomer
        {
            get
            {
                try
                {
                    return this.oCustomerRow;
                }
                catch
                { return null; }
            }
        }

        public CLCardsRow CLCard
        {
            get
            {
                try
                {
                    return this.oCLCardsRow;
                }
                catch
                { return null; }
            }
        }

        public Image BarCode
        {
            get
            {
                return this.oImage;
            }
        }

        private Graphics printerHandle = null;

        private void InitClass()
        {
            this.CFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
            this.CBrush = new System.Drawing.SolidBrush(Color.Black);
        }

        public void PrintL()
        {
            // this routine prints the dot matrix label
            if(System.IO.File.Exists(sLabelFile))
            {
                //bool bSuccess = false;
                //if(Configuration.CPOSSet.LabelPrinter.Trim() == "")
                //{
                //    return;
                //}

                try
                {
                    oPrintDocument = new PrintDocument();
                    //oPrintDocument.DefaultPageSettings.PrinterSettings.PrinterName = Configuration.CPOSSet.LabelPrinter;
                    oPrintDocument.PrinterSettings.PrinterName = Configuration.CPOSSet.RP_Name.Trim();
                    #region PRIMEPOS-2996 23-Sep-2021 JY Added
                    try
                    {
                        if (Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim() != "")
                        {
                            System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                            foreach (System.Drawing.Printing.PaperSource ps in oPrintDocument.PrinterSettings.PaperSources)
                            {
                                if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim().ToUpper())
                                {
                                    oPrintDocument.DefaultPageSettings.PaperSource = ps;
                                    break;
                                }
                            }
                        }
                    }
                    catch { }
                    #endregion
                    oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
                    oPrintDocument.PrintPage += new PrintPageEventHandler(oPrintDocument_PrintPage);
                    
                    oPrintDocument.Print();
                    //bSuccess = true;
                }
                catch(Exception e)
                {
                    clsCoreUIHelper.ShowBtnIconMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void oPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            printerHandle = e.Graphics;
            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
            _lbl.PrintCLabel();   // this will actually send the label printing lines
        }

        public void SetFont(string fontName, long fontSize, bool bold)
        {
            if(bold == true)
            {
                this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Bold);
            }
            else
            {
                this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Regular);
            }
        }

        public void SetFont(string fontName, long fontSize)
        {
            SetFont(fontName, fontSize, false);
        }

        public void PrintLine(string printData, int iLeft, int iTop)
        {
            this.printerHandle.DrawString(printData, CFont, CBrush, iLeft, iTop, new StringFormat());
        }

        public void PrintImage(Image oImage, int iLeft, int iTop, int iWidth, int iHeight)
        {
            this.printerHandle.DrawImage(oImage, iLeft, iTop, iWidth, iHeight);
        }

        #region PRIMEPOS-2829 22-Apr-2020 JY Added
        private System.IntPtr lhPrinter;
        public void PrintD()
        {
            // this routine prints the dot matrix label
            if (System.IO.File.Exists(sLabelFile))
            {
                this.lhPrinter = new System.IntPtr();

                DOCINFO di = new DOCINFO();
                di.pDocName = "Label";
                di.pDataType = "RAW";

                bool bSuccess = false;
                if (Configuration.CPOSSet.RP_Name.Trim() == "")
                {
                    bSuccess = true;
                }
                else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
                {
                    if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                    {
                        if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                        {
                            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);

                            _lbl.PrintCCRec();   // this will actually send the label printing lines
                            bSuccess = true;
                        }
                    }
                }
                PrintDirectAPIs.EndPagePrinter(lhPrinter);
                PrintDirectAPIs.EndDocPrinter(lhPrinter);
                PrintDirectAPIs.ClosePrinter(lhPrinter);
                if (bSuccess == false)
                {
                    clsCoreUIHelper.ShowErrorMsg("Unable to connect to Printer");
                }
                return;
            }
            else
            {
                throw (new Exception("Label File : " + sLabelFile + " Not Found"));
            }
        }
        #endregion
    }

    public class CustomerLoyaltyCoupon : BaseLabel
    {
        private CLCouponsRow oCouponRow = null;
        protected Image oImage;
        string sLabelFile = "CustomerLoyaltyCoupon.js";

        #region 28-Apr-2015 JY Added to synchronous reprinting of coupon with the coupon printed with receipt
        public System.Drawing.Font CFont;   
        public System.Drawing.Font Times;   
        public int PageWidth = 0;
        public string strCustomerName = string.Empty;
        #endregion

        public string CLProgramName;    //PRIMEPOS-2829 09-Apr-2020 JY Added

        public CustomerLoyaltyCoupon(CLCouponsRow oCLRow, Image oCImage, string strCustomerName)   //28-Apr-2015 JY Added CustomerName
            : base()
        {
            this.oCouponRow = oCLRow;
            this.oImage = oCImage;
            this.strCustomerName = strCustomerName;    //28-Apr-2015 JY Added
            this.CLProgramName = Configuration.CLoyaltyInfo.ProgramName;    //PRIMEPOS-2829 09-Apr-2020 JY Added
        }

        public Image BarCode
        {
            get
            {
                return this.oImage;
            }
        }

        public CLCouponsRow CouponRow
        {
            get
            {
                return oCouponRow;
            }
        }

        public string CustomerName
        {
            get
            {
                return strCustomerName;
            }
        }

        public void PrintL()
        {
            // this routine prints the dot matrix label
            if (System.IO.File.Exists(sLabelFile))
            {
                //bool bSuccess = false;
                //if(Configuration.CPOSSet.LabelPrinter.Trim() == "")
                //{
                //    return;
                //}
                try
                {
                    oPrintDocument = new PrintDocument();
                    //oPrintDocument.DefaultPageSettings.PrinterSettings.PrinterName = Configuration.CPOSSet.LabelPrinter;
                    oPrintDocument.PrinterSettings.PrinterName = Configuration.CPOSSet.RP_Name.Trim();
                    #region PRIMEPOS-2996 23-Sep-2021 JY Added
                    try
                    {
                        if (Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim() != "")
                        {
                            System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                            foreach (System.Drawing.Printing.PaperSource ps in oPrintDocument.PrinterSettings.PaperSources)
                            {
                                if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.ReceiptPrinterPaperSource.Trim().ToUpper())
                                {
                                    oPrintDocument.DefaultPageSettings.PaperSource = ps;
                                    break;
                                }
                            }
                        }
                    }
                    catch { }
                    #endregion
                    //oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", LabelWidth, LabelHeight);   //28-Apr-2015 JY Added to synchronous reprinting of coupon with the coupon printed with receipt
                    oPrintDocument.PrintController = new StandardPrintController();
                    oPrintDocument.PrintPage += new PrintPageEventHandler(oPrintDocument_PrintPage);
                    oPrintDocument.Print();
                    //bSuccess = true;
                }
                catch(Exception e)
                {
                   clsCoreUIHelper.ShowBtnIconMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void oPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            this.PR = e;    //08-Apr-2015 JY Added for DrawBox control     
            
            #region 28-Apr-2015 JY Added to synchronous reprinting of coupon with the coupon printed with receipt
            e.Graphics.PageUnit = GraphicsUnit.Document;    
            this.CFont = this.Times;    
            this.PageWidth = e.MarginBounds.Width;
            this.lnX = e.MarginBounds.X;
            this.lnY = e.MarginBounds.Y;
            #endregion 

            printerHandle = e.Graphics;
            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
            _lbl.PrintCLabel();   // this will actually send the label printing lines
        }

        #region 08-Apr-2015 JY Added for DrawBox control
        public void DrawBox(int lnX, int lnY, int Width, int Height)
        {
            try
            {
                Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Graphics gBmp = Graphics.FromImage(bmp);
                gBmp.CompositingMode = CompositingMode.SourceCopy;

                Pen p = new Pen(Color.Black, 3);
                //p.
                Rectangle rctang = new Rectangle(2, 2, Width - 5, Height - 5);
                //rctang.
                gBmp.DrawRectangle(p, rctang);

                PR.Graphics.DrawImage(bmp, lnX, lnY, Width + 5, Height + 5);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region PRIMEPOS-2829 09-Apr-2020 JY Added
        private System.IntPtr lhPrinter;
        public void PrintD()
        {
            // this routine prints the dot matrix label
            if (System.IO.File.Exists(sLabelFile))
            {
                this.lhPrinter = new System.IntPtr();

                DOCINFO di = new DOCINFO();
                di.pDocName = "Label";
                di.pDataType = "RAW";
                
                bool bSuccess = false;
                if (Configuration.CPOSSet.RP_Name.Trim() == "")
                {
                    bSuccess = true;
                }
                else if (PrintDirectAPIs.OpenPrinter(Configuration.CPOSSet.RP_Name.Trim(), ref lhPrinter, 0))
                {
                    if (PrintDirectAPIs.StartDocPrinter(lhPrinter, 1, ref di))
                    {
                        if (PrintDirectAPIs.StartPagePrinter(lhPrinter))
                        {
                            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
                            
                            _lbl.PrintCCRec();   // this will actually send the label printing lines
                            bSuccess = true;
                        }
                    }
                }
                PrintDirectAPIs.EndPagePrinter(lhPrinter);
                PrintDirectAPIs.EndDocPrinter(lhPrinter);
                PrintDirectAPIs.ClosePrinter(lhPrinter);
                if (bSuccess == false)
                {
                    clsCoreUIHelper.ShowErrorMsg("Unable to connect to Printer");
                }
                return;
            }
            else
            {
                throw (new Exception("Label File : " + sLabelFile + " Not Found"));
            }
        }
        #endregion
    }

    public class BaseLabel
    {
        private System.Drawing.Font CFont;
        private System.Drawing.Brush CBrush;

        private int iLabelWidth = 250;
        private int iLabelHeight = 200;
        protected Graphics printerHandle = null;

        protected System.Drawing.Printing.PrintDocument oPrintDocument;
        
        #region 28-Apr-2015 JY Added to synchronous reprinting of coupon with the coupon printed with receipt
        public int lnX = 0;
        public int lnY = 0;
        public int PageWidth = 0;
        public string CStoreName;
        public PrintPageEventArgs PR;
        public string CLMessage;
        public bool CLPrintMsgOnReceipt;
        #endregion

        public int LabelWidth
        {
            set
            {
                iLabelWidth = value;
                oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
            }
            get
            {
                return iLabelWidth;
            }
        }

        public int LabelHeight
        {
            set
            {
                iLabelHeight = value;
                oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
            }
            get
            {
                return iLabelHeight;
            }
        }

        public BaseLabel()
        {
            InitClass();
        }

        protected void InitClass()
        {
            this.CFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
            this.CBrush = new System.Drawing.SolidBrush(Color.Black);
            #region 29-Apr-2015 JY Added
            this.CStoreName = Configuration.CInfo.StoreName;    
            this.CLMessage = Configuration.CLoyaltyInfo.Message;   
            this.CLPrintMsgOnReceipt = Configuration.CLoyaltyInfo.PrintMsgOnReceipt;
            #endregion
        }

        public void SetFont(string fontName, long fontSize, bool bold)
        {
            if(bold == true)
            {
                this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Bold);
            }
            else
            {
                this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Regular);
            }
        }

        public void SetFont(string fontName, long fontSize)
        {
            SetFont(fontName, fontSize, false);
        }

        public void PrintLine(string printData, int iLeft, int iTop)
        {
            this.printerHandle.DrawString(printData, CFont, CBrush, iLeft, iTop, new StringFormat());
        }

        public void PrintImage(Image oImage, int iLeft, int iTop, int iWidth, int iHeight)
        {
            this.printerHandle.DrawImage(oImage, iLeft, iTop, iWidth, iHeight);
        }

        #region 29-Apr-2015 JY Added 
        public void PS(string Text, long X, long Y)
        {
            PrintS(Text, X, Y);
        }

        public void PS(string Text, long X, long Y, string PrintDirection)
        {
            PrintS(Text, X, Y, PrintDirection);
        }

        public void PS(string Text, long X, long Y, HorizontalAlignment oHorizontalAlignment)
        {
            PrintS(Text, X, Y, "H", oHorizontalAlignment);
        }

        public void PrintS(string Text, long X, long Y)
        {
            PrintS(Text, X, Y, "H");
        }

        private void PrintS(string Text, long X, long Y, string PrintDirection)
        {
            PrintS(Text, X, Y, PrintDirection, HorizontalAlignment.Left);
        }

        private void PrintS(string Text, long X, long Y, string PrintDirection, HorizontalAlignment oHorizontalAlignment)
        {
            if (PrintDirection == "V")
                this.PrintVertical(Text, X, Y);
            else
            {
                if (oHorizontalAlignment == HorizontalAlignment.Center)
                {
                    System.Drawing.SizeF iTextSize = this.PR.Graphics.MeasureString(Text, CFont);
                    if (PageWidth > iTextSize.Width)
                    {
                        X = (long)((PageWidth - iTextSize.Width) / 2);
                    }
                    else
                    {
                        X = 0;
                    }
                }
                else if (oHorizontalAlignment == HorizontalAlignment.Right)
                {
                    System.Drawing.SizeF iTextSize = this.PR.Graphics.MeasureString(Text, CFont);
                    if (PageWidth > iTextSize.Width)
                    {
                        X = (long)(PageWidth - iTextSize.Width);
                    }
                    else
                    {
                        X = 0;
                    }
                }

                this.PR.Graphics.DrawString(Text, CFont, CBrush, X, Y);
            }
        }

        public void PrintVertical(string Ptext, long lX, long lY)
        {
            // default angle for vertical printing is 270

            PrintVertical(Ptext, lX, lY, 270);
        }

        public void PrintVertical(string Ptext, long lX, long lY, long Angle)
        {
            PR.Graphics.TranslateTransform(lX, lY);
            PR.Graphics.RotateTransform(Angle); //,System.Drawing.Drawing2D.MatrixOrder.Append);
            PR.Graphics.DrawString(Ptext, this.CFont, this.CBrush, 0, 0);
            PR.Graphics.ResetTransform();
        }
              
        #endregion
    }

    public struct DOCINFO
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDocName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pOutputFile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDataType;
    }

    public class PrintDirectAPIs
    {
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);

        [DllImport(
             "winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Ansi, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv"
             , CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport(
             "winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);
    }
}