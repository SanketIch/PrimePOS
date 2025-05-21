using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using EDevice;
using NLog;
using EDevice.IscForms;
using System.Threading;
using System.Threading.Tasks;
using MMS.Device.Global;

namespace MMS.Device.WPDevice
{
    public class WPDevice
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        private EDevice.IEDevice isc_480;
        CommSettings comm = new CommSettings();
        FormProperties.Properties formprop = new FormProperties.Properties();
        FormProperties.FormSettings formset = new FormProperties.FormSettings();
        DeviceAPI isc = null;

        static AutoResetEvent AutoEvent;



        /// <summary>
        /// Get the signature
        /// </summary>
        public byte[] GetSignature { get; set; }
        public string GetSignatureString { get; set; }
        /// <summary>
        /// Check if the signature is valid
        /// </summary>
        public bool? ValidSign { get; set; }
        /// <summary>
        /// Get the Result return
        /// </summary>
        public Dictionary<string, string> ReturnResult { get; set; }
        /// <summary>
        /// Check if there is a response
        /// </summary>
        public bool? IsWPResponse { get; set; }
        public string ButtonClickID { get; set; }
        /// <summary>
        /// Patient Counsel
        /// </summary>
        bool IsPatientCounsel;
        #region  PRIMEPOS-2868 - Patient Consent Added for Defualt Consent
        public DataSet dsAutoRefillData
        {
            get; set;
        }

        private int PadRefConstSelection = 0;

        #endregion
        private bool _CancelCapture;
        public bool CancelCapture
        {
            set
            {
                _CancelCapture = value;
                if (_CancelCapture)
                {
                    bSkipConsent = true;
                    isc_UserReset();
                }
            }
            get
            {
                return _CancelCapture;
            }
        }

        public PatientConsent PatConsent { set; get; }

        private PatientConsent tmpConsent = null;

        private int PattypeSelection = 0;

        private int ConsentApplies = 0;

        private int ConsentStatus = 0;

        private bool bSkipConsent = false;

        public void Connect(int port, string conType)
        {
            logger.Debug(string.Format("In Connect()"));

            try
            {
                InitForm();
                comm.TimeOut = 3000;
                comm.Device_Name = CommSettings.DeviceName.Ingenico_isc480;
                switch (conType)
                {
                    case "S":
                        {
                            logger.Debug(string.Format("Serial Selected : port {0}", port));

                            comm.SERIAL.ComPort = (uint)port;
                            comm.DeviceInterface = CommSettings.eInterface.SERIAL;
                            break;
                        }
                    case "U":
                        {
                            //Added By Rohit Nair On June 13 2016  for WorldPay Integration to POS
                            //This Piece of Code sets the Interface with the Ingenico Pad as USB 
                            //and Sets the Auto Detect Feature to on
                            logger.Debug(string.Format("USB Selected "));

                            comm.DeviceInterface = CommSettings.eInterface.USB;
                            comm.USBHID.AutoDetection = CommSettings.UsbHid.AutoDetect.AutoDetectedOn;
                            break;
                        }
                }

                if (isc == null)
                {
                    isc = new DeviceAPI(comm.Device_Name);
                    isc_480 = isc.ISCDEVICE(formset);
                    isc_480.Connect(comm);
                    formset.Signature.CropSignature = true;
                    formset.Signature.SignReturnFormat = SigFormat.InString;
                    isc_480.SignatureEvent += isc_480_SignatureEvent;
                    isc_480.ButtonEvent += isc_480_ButtonEvent;
                    isc_480.Result += isc480_Result;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.ToString());
            }

        }

        public static void isc_UserReset()
        {

            if (AutoEvent != null)
            {
                AutoEvent.Set();
            }
        }
        void isc_480_ButtonEvent(string arg1, string arg2, string arg3)
        {
            logger.Debug(string.Format("In isc480buttonEvent({0},{1},{2})", arg1, arg2, arg3));

            switch (arg1)
            {
                case "PADRXLIST":
                    {
                        if (arg2.ToUpper() == "D")
                        {
                            Shared.ShowRXDetail();
                        }
                        break;
                    }
                case "PADREMINDN":
                case "PADREMINDER":
                case "PADRXLISTY":
                case "PADRXLISTN":
                    {
                        IsPatientCounsel = false;
                        if (arg2.ToUpper() == "ENTER" && !string.IsNullOrEmpty(arg3))
                        {
                            Constant.Counsel = arg3 == "1" ? Constant.PatientCounselYes : Constant.PatientCounselNo;
                            IsPatientCounsel = true;
                        }
                        break;
                    }
                case "PADRXLISTNC":
                    if (arg2.ToUpper() == "ENTER")
                    {
                        Constant.Counsel = Constant.PatientCounselYes;
                        IsPatientCounsel = true;
                    }
                    break;
                case "PADNOPP":
                    {
                        ButtonClickID = string.Empty;
                        if (arg2.ToUpper() == "R")
                        {
                            ButtonClickID = "R";
                            //ValidSign = false;
                        }
                        else if (arg2.ToUpper() == "S")
                        {
                            ButtonClickID = "S";
                            //ValidSign = false;
                        }
                        else if (arg2.ToUpper() == "ENTER")
                        {
                            ButtonClickID = "ENTER";
                        }

                        //AutoEvent.Set();
                        break;
                    }
                case "PADPATSIGN":
                    {
                        ValidSign = true;
                    }
                    break;
                case "PADPATSEL":
                    #region Pad Pat Select
                    try
                    {
                        PattypeSelection = Convert.ToInt32(arg3);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Unable to convert Pattype selction check box value  {0} to integer: {1}", arg3, ex.Message);
                        logger.Error(ex, message);


                    }
                    if (PattypeSelection == 4)
                    {
                        bSkipConsent = true;

                    }

                    AutoEvent.Set();
                    #endregion
                    break;
                case "PADTERM1":
                    AutoEvent.Set();
                    break;
                case "PADTERM2":
                    #region PadTerm2
                    if (arg2.ToUpper() == "S")
                    {
                        bSkipConsent = true;
                    }
                    else
                    {
                        try
                        {
                            ConsentApplies = Convert.ToInt32(arg3);
                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("Unable to convert Consent Applies to selction check box value  {0} to integer: {1}", arg3, ex.Message);
                            logger.Error(ex, message);


                        }
                    }



                    AutoEvent.Set();

                    #endregion
                    break;
                case "PADTERM3":
                    #region Pad Term 3
                    try
                    {
                        ConsentStatus = Convert.ToInt32(arg3);
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Unable to convert Consent Type selction check box value  {0} to integer: {1}", arg3, ex.Message);
                        logger.Error(ex, message);


                    }
                    AutoEvent.Set();
                    #endregion
                    break;
                case "PADREFCONST": // PRIMEPOS-2868 Added for default consent
                case "PADREFCONS2":
                case "PADREFCONS3":
                    #region PRIMEPOS-2868 get click button event Id
                    try
                    {
                        ButtonClickID = string.Empty;
                        if (arg2.ToUpper() == "R")
                        {
                            ButtonClickID = "R";
                            PadRefConstSelection = 1;//PRIMEPOS-Arvind Ingenico
                        }
                        else if (arg2.ToUpper() == "S")
                        {
                            ButtonClickID = "S";
                        }
                        else if (arg2.ToUpper() == "ENTER")
                        {
                            ButtonClickID = "ENTER";
                            PadRefConstSelection = Convert.ToInt32(arg3);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Unable to convert Consent Type selction check box value  {0} to integer: {1}", arg3, ex.Message);
                        logger.Error(ex, message);
                    }
                    AutoEvent.Set();
                    #endregion
                    break;
            }
        }

        void isc_480_SignatureEvent(SigFormat arg1, SigStatus arg2, object arg3)
        {
            logger.Debug(string.Format("In isc_480_SignatureEvent()"));

            switch (arg2)
            {
                case SigStatus.IsAccepted:
                    {
                        switch (arg1)
                        {
                            case SigFormat.InString:
                                {
                                    ValidSign = true;
                                    byte[] signByte = Convert.FromBase64String((string)arg3);
                                    GetSignatureString = arg3 as string;
                                    GetSignature = signByte;
                                    break;
                                }
                        }
                        break;
                    }
                case SigStatus.IsCancel:
                    {
                        switch (arg1)
                        {
                            case SigFormat.InString:
                                {
                                    ValidSign = false;
                                    GetSignature = null;
                                    GetSignatureString = string.Empty;
                                    break;
                                }
                        }
                        break;
                    }
            }
            logger.Debug("Signature Event Fired: " + arg2.ToString() + " SigType: " + arg1.ToString());
        }


        private void InitForm()
        {
            formset.CustomForms.FormList = new List<Tuple<string, string[]>>();
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADWELCOME", new string[] { }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADITEMLIST", new string[] { "linedisplay", "IST", "DIS", "TAX", "ITOT", "lblLineItem" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADNOPP", new string[] { "lblHippa", "lblPatientName", "lblAddress", "SigBox" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADRXLIST", new string[] { "RxPatientName", "RxCount", "linedisplay", "SigBox" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADSIGN", new string[] { "SigBox", "SigAmt" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADMSG", new string[] { "lblMsg" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADREMINDER", new string[] { "lblConfirmation" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADOTC", new string[] { "lblOTC", "linedisplay", "SigBox" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADREMINDN", new string[] { "lblConfirmation" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADPATSIGN", new string[] { "" }));
            formset.CustomForms.FormList.Add(new Tuple<string, string[]>("PADREFCONST", new string[] { "lblTitle", "lblConsent", "lblPatientName", "lblAddress", "rbtnRep1", "rbtnRep2", "rbtnRep3", "btnAccept", "btnRefuse" })); // PRIMEPOS-2868 - Added for default consent
            formset.Signature.SignReturnFormat = SigFormat.InString;
            formset.Signature.CropSignature = true;
        }

        private void ShowConfirmation(string cScreen)
        {
            formprop.FormName = cScreen.ToUpper();
            formprop.FormElementID = formset.CustomForms.FormList[6].Item2[0];
            formprop.FormElementType = FormProperties.Properties.ElementType.Text;
            formprop.FormElementData = "Patient Counseling ?";
            isc_480.ShowForm(formprop);
        }

        private void Events()
        {
            if (isc_480 != null)
            {
                isc_480.SignatureEvent -= isc_480_SignatureEvent;
                isc_480.SignatureEvent += isc_480_SignatureEvent;
                isc_480.ButtonEvent -= isc_480_ButtonEvent;
                isc_480.ButtonEvent += isc_480_ButtonEvent;
            }
        }
        public bool ShowForm(Hashtable data, string formName)
        {
            ValidSign = null;
            IsWPResponse = null;
            var form = formset.CustomForms.FormList.Find(f => f.Item1.ToUpper() == formName.ToUpper());
            if (form != null)
            {
                GetSignature = null;
                formprop.FormName = form.Item1;
                formprop.FormElementType = FormProperties.Properties.ElementType.Text;

                switch (form.Item1)
                {
                    case "PADWELCOME":
                        {
                            isc_480.ShowForm(formprop);
                            break;
                        }
                    case "PADNOPP":
                        {
                            if (data != null && data.ContainsKey("pNopp") && data.ContainsKey("pName") && data.ContainsKey("pAddress"))
                            {
                                logger.Debug("\t\t\t[PADNOPP] Nopp, Name, address");
                                //AutoEvent = new AutoResetEvent(false);
                                PadNopp noppForm = new PadNopp();


                                foreach (DictionaryEntry d in data)
                                {
                                    if (d.Key.ToString().ToUpper() == "PNOPP")
                                    {
                                        string NoppMsg = string.IsNullOrEmpty(data["pNopp"].ToString().Trim()) ? "Please ask for HIPAA notice. Not available at this time" : data["pNopp"].ToString();
                                        noppForm.SetPatientHippaText(NoppMsg);
                                        /*formprop.FormElementID = form.Item2[0];
                                        formprop.FormElementData = NoppMsg;
                                        isc_480.ShowForm(formprop);*/
                                    }
                                    else if (d.Key.ToString().ToUpper() == "PNAME")
                                    {
                                        string PatName = string.IsNullOrEmpty(data["pName"].ToString().Trim()) ? "Patient Name" : data["pName"].ToString();
                                        noppForm.SetPatientName(PatName);
                                        /*ormprop.FormElementID = form.Item2[1];
                                         formprop.FormElementData = PatName;
                                         isc_480.ShowForm(formprop);*/
                                    }
                                    else if (d.Key.ToString().ToUpper() == "PADDRESS")
                                    {
                                        string PatAddress = string.IsNullOrEmpty(data["pAddress"].ToString().Trim()) ? "Patient Address" : data["pAddress"].ToString();
                                        noppForm.SetPatientAddress(PatAddress);
                                        /*rmprop.FormElementID = form.Item2[2];
                                        formprop.FormElementData = PatAddress;
                                        isc_480.ShowForm(formprop);*/
                                    }
                                }
                                Events();
                                isc_480.LoadForm(noppForm);
                                /*rmprop.FormElementID = form.Item2[3];
                                formprop.FormElementData = string.Empty;
                                isc_480.ShowForm(formprop);*/

                            }
                            break;
                        }
                    case "PADRXLIST":
                        {
                            //Modified by Roht Nair on 08/07/2017
                            logger.Debug("In " + form.Item1.ToString());
                            PadPatCouncel councellingForm;
                            #region Setting Default Councelling
                            string defaultCouncel = "Y";
                            if (data.ContainsKey("counselReq"))
                            {
                                foreach (DictionaryEntry d in data)
                                {
                                    if (d.Key.ToString().ToUpper() == "COUNSELREQ")
                                    {
                                        defaultCouncel = string.IsNullOrEmpty(d.Value.ToString().Trim()) ? "Y" : d.Value.ToString().ToUpper();
                                    }
                                }
                            }

                            if (defaultCouncel == "NC")
                            {
                                councellingForm = new PadPatCouncel(false, true);
                            }
                            else
                            {
                                if (defaultCouncel == "N")
                                {
                                    councellingForm = new PadPatCouncel(false);
                                }
                                else
                                {
                                    councellingForm = new PadPatCouncel(true);
                                }
                                councellingForm.SetCouncellingText("Patient Counseling?");
                            }

                            #endregion

                            #region Name and Total Rx
                            if (data.ContainsKey("pName") && data.ContainsKey("totalRx") && data.ContainsKey("counselReq"))
                            {
                                logger.Debug("\t\t\t[PADRXLIST] PName, TotalRx, CounselReq");
                                

                                foreach (DictionaryEntry d in data)
                                {
                                    if (d.Key.ToString().ToUpper() == "PNAME")
                                    {
                                        string PatName = string.IsNullOrEmpty(d.Value.ToString().Trim()) ? "Patient Name" : d.Value.ToString();
                                        councellingForm.SetPatientName(PatName);
                                        /*try
                                        {
                                            if (PatName.Contains(","))
                                            {
                                                string[] splitchar = new string[] { "," };
                                                PatName = PatName.Replace(" ", "");
                                                string[] names = PatName.Split(splitchar, 2, StringSplitOptions.RemoveEmptyEntries);

                                                if (names.Length > 1)
                                                {
                                                    PatName = (names[0].Length > 15 ? names[0].Substring(0, 15) : names[0]) + (names[1].Length > 0 ? " ," + names[1].Substring(0, 1) : "");
                                                }
                                                else
                                                {
                                                    PatName = names[0].Length > 15 ? names[0].Substring(0, 15) : names[0];
                                                }
                                            }
                                        }
                                        catch (Exception expn)
                                        {
                                            logger.Error(expn, "An error occureed while parsing patient name " + PatName);
                                            PatName = string.IsNullOrEmpty(d.Value.ToString().Trim()) ? "Patient Name" : d.Value.ToString();
                                        }

                                        formprop.FormElementID = form.Item2[0];
                                        formprop.FormElementData = PatName;
                                        isc_480.ShowForm(formprop);*/

                                    }
                                    else if (d.Key.ToString().ToUpper() == "TOTALRX")
                                    {
                                        string TotalRx = string.IsNullOrEmpty(d.Value.ToString().Trim()) ? "00" : " " + d.Value.ToString();
                                        councellingForm.SetRxCount(TotalRx);
                                        /*formprop.FormElementID = form.Item2[1];
                                        formprop.FormElementData = TotalRx;
                                        isc_480.ShowForm(formprop);*/
                                    }
                                }
                                //Events();
                                //formprop.FormElementID = form.Item2[3];
                                //formprop.FormElementData = string.Empty;
                                //isc_480.ShowForm(formprop);

                            }
                            #endregion Name and Address        

                            #region  Remove item
                            /*
                                if (data != null && data.ContainsKey("removeRxItem"))
                                {
                                    if (!string.IsNullOrEmpty(data["index"].ToString().Trim()))
                                    {
                                        int index = Convert.ToInt32(data["index"].ToString().Trim());
                                        isc_480.RemoveFromLineDisplay(index);
                                    }
                                }
                                */
                            #endregion  Remove item

                            #region ShowRxDetail
                            if (data != null && data.ContainsKey("ShowRx"))
                            {
                                //isc_480.ClearLineDisplay(form.Item1);
                                ArrayList rx = new ArrayList();
                                bool isRxAdd = false;
                                //Events();
                                while (!isRxAdd)
                                {
                                    try
                                    {
                                        rx = data["ShowRx"] as ArrayList;
                                        isRxAdd = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error(ex, ex.Message);
                                        isRxAdd = false;
                                    }
                                }
                                foreach (var Rxs in rx)
                                {
                                    string rxString = string.IsNullOrEmpty(Rxs.ToString().Trim()) ? "000000" : Rxs.ToString();
                                    councellingForm.AddToLineDisplay(rxString);
                                    /*formprop.FormElementID = form.Item2[2];
                                    formprop.FormElementData = rxString;
                                    isc_480.ShowForm(formprop);*/
                                }
                            }
                            #endregion ShowRxDetail

                            #region Add Rx
                            if (data["rxItem"] != null && data.ContainsKey("rxItem"))
                            {
                                logger.Debug("\t\t\t[PADRXLIST] rxItem");
                                ArrayList rx = new ArrayList();
                                bool isRxAdd = false;
                                //Events();
                                while (!isRxAdd)
                                {
                                    try
                                    {
                                        rx = data["rxItem"] as ArrayList;
                                        isRxAdd = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error(ex, ex.Message);
                                        isRxAdd = false;
                                    }
                                }
                                foreach (var Rxs in rx)
                                {
                                    string rxString = string.IsNullOrEmpty(Rxs.ToString().Trim()) ? "000000" : Rxs.ToString();
                                    councellingForm.AddToLineDisplay(rxString);
                                    /*formprop.FormElementID = form.Item2[2];
                                    formprop.FormElementData = rxString;
                                    isc_480.ShowForm(formprop);*/
                                }
                            }
                            #endregion Add Rx 

                            Events();
                            isc_480.LoadForm(councellingForm);

                            break;
                        }
                    case "PADOTC":
                        {
                            logger.Debug("\t\t\t[PADOTC] Begin");
                            if (data != null && data["oTcItems"] != null)
                            {
                                logger.Debug("\t\t\t[PADOTC] OtcItems");
                                ArrayList OtcItems = new ArrayList();
                                bool isOtc = false;
                                string OTCMsg = string.IsNullOrEmpty(data["oTcMsg"].ToString().Trim()) ? "Unknown" : data["oTcMsg"].ToString();
                                formprop.FormElementID = form.Item2[0];
                                formprop.FormElementData = OTCMsg;
                                isc_480.ShowForm(formprop);

                                while (!isOtc)
                                {
                                    try
                                    {
                                        OtcItems = data["oTcItems"] as ArrayList;
                                        isOtc = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error(ex, ex.Message);
                                        isOtc = false;
                                    }
                                }

                                foreach (var Otc in OtcItems)
                                {
                                    string otcItem = string.IsNullOrEmpty(Otc.ToString().Trim()) ? "Unknown" : Otc.ToString();
                                    formprop.FormElementID = form.Item2[1];
                                    formprop.FormElementData = otcItem;
                                    isc_480.ShowForm(formprop);
                                }
                                Events();
                                formprop.FormElementID = form.Item2[2];
                                formprop.FormElementData = string.Empty;
                                isc_480.ShowForm(formprop);
                            }
                            logger.Debug("\t\t\t[PADOTC] Finish");
                            break;
                        }
                    case "PADITEMLIST":
                        {
                            #region Counter Item
                            if (data != null && data.ContainsKey("counterItem"))
                            {
                                formprop.FormElementID = form.Item2[0];
                                formprop.FormElementData = data["counterItem"].ToString();
                                isc_480.ShowForm(formprop);
                            }
                            #endregion Counter Item 

                            #region Clear all items
                            if (data != null && data.ContainsKey("ClearItems"))
                            {
                                isc_480.ClearLineDisplay(form.Item1);
                                formprop.UpdateFormElement.Update = new List<Tuple<string[], string[]>>();
                                formprop.UpdateFormElement.Update.Add(new Tuple<string[], string[]>(new string[] { form.Item2[1], form.Item2[2], form.Item2[3], form.Item2[4], form.Item2[5] },
                                        new string[] { data["subTotal"].ToString(), data["Discount"].ToString(), data["Tax"].ToString(), data["totalAmount"].ToString(), "Line Item: " + isc_480.LineItemNumber }));
                                isc_480.UpdateFormElement(formprop.UpdateFormElement);
                                isc_480.CancelTransaction();
                            }
                            #endregion Clear all items

                            #region  Update Item
                            if (data != null && data.ContainsKey("UpdateItem") && data.ContainsKey("Index"))
                            {
                                int ind = Convert.ToInt32(data["Index"].ToString());
                                isc_480.UpdateLineDisplay(ind, data["UpdateItem"].ToString());
                            }
                            #endregion  Update Item

                            #region Remove Item
                            if (data != null && data.ContainsKey("removeCounterItem"))
                            {
                                int ind = Convert.ToInt32(data["removeCounterItem"].ToString());
                                isc_480.RemoveFromLineDisplay(ind);
                            }
                            #endregion Remove Item

                            #region On Hold Item
                            if (data != null && data.Contains("OnHoldItems"))
                            {
                                ArrayList oitem = new ArrayList();
                                bool isItem = false;
                                while (!isItem)
                                {
                                    try
                                    {
                                        oitem = data["OnHoldItems"] as ArrayList;
                                        isItem = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error(ex, ex.Message);
                                        isItem = false;
                                    }
                                }
                                foreach (var cItem in oitem)
                                {
                                    formprop.FormElementID = form.Item2[0];
                                    formprop.FormElementData = cItem.ToString();
                                    isc_480.ShowForm(formprop);
                                }
                                formprop.UpdateFormElement.Update = new List<Tuple<string[], string[]>>();
                                formprop.UpdateFormElement.Update.Add(new Tuple<string[], string[]>(new string[] { form.Item2[1], form.Item2[2], form.Item2[3], form.Item2[4], form.Item2[5] },
                                        new string[] { data["subTotal"].ToString(), data["Discount"].ToString(), data["Tax"].ToString(), data["totalAmount"].ToString(), "Line Item: " + isc_480.LineItemNumber }));
                                isc_480.UpdateFormElement(formprop.UpdateFormElement);
                            }
                            #endregion On Hold Item

                            #region  Resend items
                            if (data != null && data.ContainsKey("resendCounter"))
                            {
                                isc_480.ClearLineDisplay(form.Item1);
                                ArrayList oitem = new ArrayList();
                                bool isItem = false;

                                while (!isItem)
                                {
                                    try
                                    {
                                        oitem = data["resendCounter"] as ArrayList;
                                        isItem = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error(ex, ex.Message);
                                        isItem = false;
                                    }
                                }
                                foreach (var cItem in oitem)
                                {
                                    formprop.FormElementID = form.Item2[0];
                                    formprop.FormElementData = cItem.ToString();
                                    isc_480.ShowForm(formprop);
                                }
                                formprop.UpdateFormElement.Update = new List<Tuple<string[], string[]>>();
                                formprop.UpdateFormElement.Update.Add(new Tuple<string[], string[]>(new string[] { form.Item2[1], form.Item2[2], form.Item2[3], form.Item2[4], form.Item2[5] },
                                        new string[] { data["subTotal"].ToString(), data["Discount"].ToString(), data["Tax"].ToString(), data["totalAmount"].ToString(), "Line Item: " + isc_480.LineItemNumber }));
                                isc_480.UpdateFormElement(formprop.UpdateFormElement);
                            }
                            #endregion  Resend items

                            #region Amount Section
                            if (data != null && data.ContainsKey("subTotal") && data.ContainsKey("Discount") && data.ContainsKey("Tax") && data.ContainsKey("totalAmount"))
                            {
                                formprop.UpdateFormElement.Update = new List<Tuple<string[], string[]>>();
                                formprop.UpdateFormElement.Update.Add(new Tuple<string[], string[]>(new string[] { form.Item2[1], form.Item2[2], form.Item2[3], form.Item2[4], form.Item2[5] },
                                        new string[] { data["subTotal"].ToString(), data["Discount"].ToString(), data["Tax"].ToString(), data["totalAmount"].ToString(), "Line Item: " + isc_480.LineItemNumber }));
                                isc_480.UpdateFormElement(formprop.UpdateFormElement);
                            }
                            #endregion Amount Section
                            

                            break;
                        }
                    case "PADMSG":
                        {
                            #region  Message
                            if (data != null && data.ContainsKey("padMsg"))
                            {
                                formprop.FormElementID = form.Item2[0];
                                formprop.FormElementData = data["padMsg"].ToString();
                                isc_480.ShowForm(formprop);
                            }
                            #endregion  Message
                            break;
                        }
                    case "PADSIGN":
                        {
                            #region Signature
                            if (data != null && data.ContainsKey("aCharge"))
                            {
                                formprop.FormElementID = form.Item2[0];
                                isc_480.ShowForm(formprop);
                                //formprop.FormElementID = form.Item2[1];
                                //formprop.FormElementType = FormProperties.Properties.ElementType.Text;
                                //formprop.FormElementData = data["aCharge"].ToString();
                                isc_480.ShowForm(formprop);
                                formprop.UpdateFormElement.Update = new List<Tuple<string[], string[]>>();
                                formprop.UpdateFormElement.Update.Add(new Tuple<string[], string[]>(new string[] { "SigAmt" }, new string[] { data["aCharge"].ToString() }));
                            }
                            #endregion Signature    
                            break;
                        }
                    case "PADREMINDER":
                        {
                            ShowConfirmation("PADREMINDER");
                            break;
                        }
                    case "PADREMINDN":
                        {
                            ShowConfirmation("PADREMINDN");
                            break;
                        }
                    case "PADPATSIGN":
                        {
                            logger.Debug("In PadPatSign -->About capturePatient Signature");
                            PadPatSign signatureForm = new PadPatSign();
                            Events();
                            isc_480.LoadForm(signatureForm);
                            break;
                        }
                }

            }
            return true;
        }

        public void PerformConsentCaptureforHealthix(Hashtable data)
        {
            //PattypeSelection


            logger.Debug("In PerformConsentCaptureforHealthix");
            tmpConsent = new PatientConsent();
            string PharmacyName = "This Pharmacy ";
            if (data.ContainsKey("PharmacyName"))
            {
                foreach (DictionaryEntry d in data)
                {
                    if (d.Key.ToString().ToUpper() == "PHARMACYNAME")
                    {
                        PharmacyName = string.IsNullOrEmpty(data["PharmacyName"].ToString().Trim()) ? "This Pharmacy " : data["PharmacyName"].ToString();
                    }
                }
            }

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
            AutoEvent = new AutoResetEvent(false);

            PadPatSelect patselectForm = new PadPatSelect();

            isc_480.LoadForm(patselectForm);

            AutoEvent.WaitOne();

            if (PattypeSelection > 0 && PattypeSelection < 4)
            {
                tmpConsent.ConsentSourceName = Consentsource;
                tmpConsent.PatConsentRelationShipDescription = PatientConsent.GetRelationshipCodeForHealthix(PattypeSelection);
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

            AutoEvent = new AutoResetEvent(false);

            PadHealthix1 padform = new PadHealthix1();
            isc_480.LoadForm(padform);

            AutoEvent.WaitOne();
        }

        private void ShowHealtixForm2(string PharmacyName)
        {
            logger.Debug("In ShowHelthixForm1()=>  ");

            AutoEvent = new AutoResetEvent(false);

            PadHealthix2 padform = new PadHealthix2();
            padform.SetPharmacyName(PharmacyName);
            isc_480.LoadForm(padform);

            AutoEvent.WaitOne();
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

            AutoEvent = new AutoResetEvent(false);

            PadHealthix3 padform = new PadHealthix3();
            isc_480.LoadForm(padform);

            AutoEvent.WaitOne();
            if (!bSkipConsent)
            {
                tmpConsent.ConsentStatusCode = PatientConsent.GetStatusCodeForHealthix(ConsentStatus);
            }
            else
            {
                tmpConsent.IsConsentSkip = true;
            }

        }

        /// <summary>
        /// Cancel Transaction
        /// </summary>
        internal void CancelTransaction()
        {
            isc_480.CancelTransaction();
        }

        /// <summary>
        /// Disconnect Device
        /// </summary>
        /// <returns></returns>
        internal bool Disconnect()
        {
            return isc_480.Disconnect() == 0 ? true : false;
            //isc_480.Disconnect();//reverted to old version for testing Ingenico lane close issue.
            //return isc_480.ShutDown() == 0 ? true : false;
        }

        /// <summary>
        /// Shutdown Device
        /// </summary> PRIMEPOS-2534 - Lane CLose Issue Added By Suraj 28-08-2018
        /// <returns></returns>
        internal bool ShutDown()
        {
            //return isc_480.Disconnect() == 0 ? true : false;
            //isc_480.Disconnect();//reverted to old version for testing Ingenico lane close issue.
            return isc_480.ShutDown() == 0 ? true : false;
        }

        /// <summary>
        /// Process Payment 
        /// </summary>
        /// <param name="pay"></param>
        /// <param name="WPInfo"></param>
        internal void ProcessPayment(WPData.Payment pay, WPData.WPAccountInfo WPInfo)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            PaymentTags pt = new PaymentTags();
            GatewayParameters gtp = new GatewayParameters();

            switch (pay.TransType)
            {
                case WPData.TransTypes.Sale:
                    {
                        pt.TransactionType = PaymentTags.transactionType.Sale;
                        pt.Amount.TotalAmount = pay.Amount.TotalAmount;
                        if (pay.Amount.FSA.RxAmount > 0 || pay.Amount.FSA.TotalHealthCareAmount > 0)
                        {
                            pt.FSA.IsHealthCare = true;
                            pt.FSA.AutoSubstantiation.RxAmount = pay.Amount.FSA.RxAmount;
                            pt.FSA.AutoSubstantiation.TotalHealthCareAmount = pay.Amount.FSA.TotalHealthCareAmount;
                        }
                        switch (pay.PayType)
                        {
                            case WPData.PayTypes.Credit:
                                {
                                    pt.PaymentType = PaymentTags.payType.Credit;
                                    break;
                                }
                            case WPData.PayTypes.Debit:
                                {
                                    pt.PaymentType = PaymentTags.payType.Debit;
                                    break;
                                }
                            case WPData.PayTypes.EBT_CashBenifit_Sale:
                                {
                                    pt.PaymentType = PaymentTags.payType.EBT_Cash;
                                    break;
                                }
                        }
                        break;
                    }
                case WPData.TransTypes.Credit:
                    {
                        pt.Reversal.HistoryID = pay.Return.HistoryID;
                        pt.Reversal.OrderID = pay.Return.OrderID;
                        break;
                    }
                case WPData.TransTypes.Void:
                    {
                        pt.Void.HistoryID = pay.Void.HistoryID;
                        pt.Void.OrderID = pay.Void.OrderID;
                        break;
                    }
            }
            gtp.AccountID = WPInfo.AccountID;
            gtp.MerchantPIN = WPInfo.MerchantPIN;
            gtp.SubID = WPInfo.SubID;
            IsWPResponse = null;
            isc_480.ProcessTransaction(pt);
        }

        void isc480_Result(Dictionary<string, string> obj)
        {
            ReturnResult = new Dictionary<string, string>();
            if (ReturnResult != null)
            {
                ReturnResult = obj;
            }
            IsWPResponse = true;
        }
        #region PRIMEPOS-2868 - Added for default / other consent
        public void GetDefaultConsentSelection(Hashtable data)
        {
            AutoEvent = new AutoResetEvent(false);
            tmpConsent = new PatientConsent();
            if (data != null && data.ContainsKey("pData") && data.ContainsKey("pName") && data.ContainsKey("pAddress"))
            {
                logger.Debug("\t\t\t[GetAutoFillSelection] ConsentText, Name, address");

                int numOptions = GetNumOfConsentPersonOptions(data);
                PadRefConst oPadRefConst = new PadRefConst(numOptions);

                foreach (DictionaryEntry d in data)
                {
                    if (d.Key.ToString().ToUpper() == "PTITLE")
                    {
                        string ConsentMsg = string.IsNullOrEmpty(data["pTitle"].ToString().Trim()) ? "Consent" : data["pTitle"].ToString();
                        oPadRefConst.SetTitle(ConsentMsg);
                    }
                    else if (d.Key.ToString().ToUpper() == "PDATA")
                    {
                        #region PRIMEPOS-3192
                        //string ConsentMsg = string.IsNullOrEmpty(data["pData"].ToString().Trim()) ? " Not available at this time" : data["pData"].ToString();
                        //oPadRefConst.SetConsentText(ConsentMsg); PRIMEPOS-3192
                        string ConsentMsg = string.Empty;
                        string DrugNameMsg = string.Empty;
                        if (!string.IsNullOrEmpty(data["pData"].ToString().Trim()))
                        {
                            string tempConsentText = data["pData"].ToString();
                            string[] ConsentArray = tempConsentText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                            if (ConsentArray.Length > 0)
                            {
                                ConsentMsg = ConsentArray[0];
                            }
                            if (ConsentArray.Length > 1)
                            {
                                DrugNameMsg = ConsentArray[1];
                            }
                        }
                        else
                        {
                            ConsentMsg = " Not available at this time";
                        }
                        oPadRefConst.SetConsentText(ConsentMsg);
                        oPadRefConst.SetDrugNameText(DrugNameMsg);
                        #endregion
                    }
                    else if (d.Key.ToString().ToUpper() == "PNAME")
                    {
                        string PatName = string.IsNullOrEmpty(data["pName"].ToString().Trim()) ? "Patient Name" : data["pName"].ToString();
                        oPadRefConst.SetPatientName(PatName);
                    }
                    else if (d.Key.ToString().ToUpper() == "PADDRESS")
                    {
                        string PatAddress = string.IsNullOrEmpty(data["pAddress"].ToString().Trim()) ? "Patient Address" : data["pAddress"].ToString();
                        oPadRefConst.SetPatientAddress(PatAddress);
                    }

                    else if (d.Key.ToString().ToUpper() == "BUTTON1")
                    {
                        string Button1 = string.IsNullOrEmpty(data["Button1"].ToString().Trim()) ? "no text" : data["Button1"].ToString();
                        oPadRefConst.SetConsentButtonText1(Button1);
                    }
                    else if (d.Key.ToString().ToUpper() == "BUTTON2")
                    {
                        string Button2 = string.IsNullOrEmpty(data["Button2"].ToString().Trim()) ? "no text" : data["Button2"].ToString();
                        oPadRefConst.SetConsentButtonText2(Button2);
                    }
                    else if (d.Key.ToString().ToUpper() == "BUTTON3")
                    {
                        string Button3 = string.IsNullOrEmpty(data["Button3"].ToString().Trim()) ? "no text" : data["Button3"].ToString();
                        oPadRefConst.SetConsentButtonText3(Button3);
                    }
                    else if (d.Key.ToString().ToUpper() == "POPTIONS1")
                    {
                        string Option1 = string.IsNullOrEmpty(data["pOptions1"].ToString().Trim()) ? "no options" : data["pOptions1"].ToString();
                        oPadRefConst.SetConsentRadioButton1Text(Option1);
                    }

                    else if (d.Key.ToString().ToUpper() == "POPTIONS2")
                    {
                        string Option2 = string.IsNullOrEmpty(data["pOptions2"].ToString().Trim()) ? "no options" : data["pOptions2"].ToString();
                        oPadRefConst.SetConsentRadioButton2Text(Option2);
                    }
                    else if (d.Key.ToString().ToUpper() == "POPTIONS3")
                    {
                        string Option3 = string.IsNullOrEmpty(data["pOptions3"].ToString().Trim()) ? "no options" : data["pOptions3"].ToString();
                        oPadRefConst.SetConsentRadioButton3Text(Option3);
                    }
                    else if (d.Key.ToString().ToUpper() == "POPTIONS4")
                    {
                        string Option4 = string.IsNullOrEmpty(data["pOptions4"].ToString().Trim()) ? "no options" : data["pOptions4"].ToString();
                        oPadRefConst.SetConsentRadioButton4Text(Option4);
                    }
                }
                //Events();

                isc_480.LoadForm(oPadRefConst);
                //isc_480.LoadForm(oPadRefConst); //PRIMEPOS-3238
                AutoEvent.WaitOne();
            }

            DataTable dtbtn = new DataTable();
            dtbtn = dsAutoRefillData.Tables["Consent_Status"];
            DataTable dtRelation = dsAutoRefillData.Tables["Consent_Relationship"];//PRIMEPOS-Arvind Ingenico
            if (PadRefConstSelection > 0 && PadRefConstSelection <= dtRelation.Rows.Count) //PRIMEPOS-3192
            {
                if (ButtonClickID == "ENTER")
                {

                    tmpConsent.PatConsentRelationID = Convert.ToInt32(dtRelation.Rows[PadRefConstSelection - 1]["ID"]);
                    tmpConsent.PatConsentRelationShipDescription = dtRelation.Rows[PadRefConstSelection - 1]["Relation"].ToString();
                    tmpConsent.ConsentStatusID = Convert.ToInt32(dtbtn.Rows[0]["ID"].ToString());
                    tmpConsent.ConsentStatusCode = dtbtn.Rows[0]["Code"].ToString();
                }

            }
            if (ButtonClickID == "R")
            {
                tmpConsent.PatConsentRelationID = Convert.ToInt32(dtRelation.Rows[PadRefConstSelection - 1]["ID"]);//PRIMEPOS-Arvind Ingenico
                tmpConsent.ConsentStatusID = Convert.ToInt32(dtbtn.Rows[1]["ID"].ToString());
                tmpConsent.ConsentStatusCode = dtbtn.Rows[1]["Code"].ToString();
                //tmpConsent = null;
            }
            DataTable dtConsentText = dsAutoRefillData.Tables["ConsentTextVersion"];
            tmpConsent.ConsentTextID = Convert.ToInt32(dtConsentText.Rows[0]["ID"]);
            if (ButtonClickID == "S")
            {
                tmpConsent = new PatientConsent();
                tmpConsent.IsConsentSkip = true;
            }
            PatConsent = tmpConsent;
            tmpConsent = null;
        }

        private int GetNumOfConsentPersonOptions(Hashtable data)
        {
            int numOptions = 0;

            foreach (DictionaryEntry d in data)
            {
                if (d.Key.ToString().ToUpper().Contains("POPTIONS"))
                {
                    numOptions++;
                }
            }

            return numOptions;
        }
        #endregion
    }
}
