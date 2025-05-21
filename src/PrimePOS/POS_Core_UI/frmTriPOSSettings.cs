using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using POS_Core.Resources;
using System.ServiceProcess;
using System.Management;
using System.IO;
using POS_Core.BusinessRules;
using NLog;
using POS_Core.CommonData;

namespace POS_Core_UI
{
    public partial class frmTriPOSSettings : Form
    {
        #region local variables
        private const string strServiceName = "TriPosService";
        private const string strConfigFileName = "triPOS.config";
        TriPOSSettings oTriPOSSettings = new TriPOSSettings();
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        #endregion

        public frmTriPOSSettings()
        {
            InitializeComponent();
        }

        private void frmTriPOSSettings_Load(object sender, EventArgs e)
        {
            logger.Trace("frmTriPOSSettings_Load() - " + clsPOSDBConstants.Log_Entering);
            clsUIHelper.setColorSchecme(this);
            Configuration.AddIPLaneToConfig();    //PRIMEPOS-3024 08-Nov-2021 JY Added

            #region service 
            if (!serviceExists(strServiceName))
            {
                lblErrMsg.Text = "TriPOS service is not installed";
                lblErrMsg.Appearance.ForeColor = Color.Red;
            }
            else
            {
                try
                {
                    using (ServiceController sc = new ServiceController())
                    {
                        sc.ServiceName = strServiceName;
                        //lblErrMsg.Text = string.Format("The {0} status is currently set to {1}", strServiceName, sc.Status.ToString());
                        if (sc.Status == ServiceControllerStatus.Running)
                        {
                            lblErrMsg.Text = "TriPOS service is running";
                            lblErrMsg.Appearance.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblErrMsg.Text = "TriPOS service is stopped";
                            lblErrMsg.Appearance.ForeColor = Color.Red;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    lblErrMsg.Text = "TriPOS service does not exists, please contact your system administrator";
                }
            }
            #endregion

            LoadDrivers(); //PRIMEPOS-3024 08-Nov-2021 JY Added

            string strConfigFilePath = string.Empty;
            Prefrences oPrefrences = new Prefrences();
            DataTable dt = oPrefrences.GetTriPOSSettings();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Configuration.convertNullToString(dt.Rows[0]["TriPOSConfigFilePath"]).Trim() != "")
                    strConfigFilePath = Configuration.convertNullToString(dt.Rows[0]["TriPOSConfigFilePath"]).Trim();
            }

            if (strConfigFilePath == "")
            {
                strConfigFilePath = Configuration.GetConfigFilePath();
                if (strConfigFilePath != string.Empty)
                {
                    txtTriPOSConfigFilePath.Text = string.Concat(strConfigFilePath, @"\", strConfigFileName);
                    LoadConfig(txtTriPOSConfigFilePath.Text);
                }
                else
                {
                    Resources.Message.Display("Couldnt find TriPOS config file path, please select it", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnTriPOSConfigFilePath.Focus();
                }
            }
            else
            {
                txtTriPOSConfigFilePath.Text = strConfigFilePath;
                LoadConfig(txtTriPOSConfigFilePath.Text);
            }

            logger.Trace("frmTriPOSSettings_Load() - " + clsPOSDBConstants.Log_Exiting);
        }

        #region PRIMEPOS-3024 08-Nov-2021 JY Added
        private void LoadDrivers()
        {
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();//PRIMEPOS-3063
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();//PRIMEPOS-3500
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "VeriFoneXpi";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "VeriFoneCXpi";
            valueListItem3.DataValue = "ValueListItem2";
            valueListItem3.DisplayText = "VerifoneFormAgentXpi";
            valueListItem4.DataValue = "ValueListItem3";
            valueListItem4.DisplayText = "IngenicoRba";
            valueListItem11.DataValue = "IngenicoUpp";//PRIMEPOS-3063
            valueListItem11.DisplayText = "IngenicoUpp";//PRIMEPOS-3063
            valueListItem12.DataValue = "Null";//PRIMEPOS-3500
            valueListItem12.DisplayText = "Null";//PRIMEPOS-3500

            this.cmbSLDriver.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem12,valueListItem1,valueListItem2,valueListItem3,valueListItem4,valueListItem11});//PRIMEPOS-3063

            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();//PRIMEPOS-3063
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();//PRIMEPOS-3500
            valueListItem5.DataValue = "ValueListItem0";
            valueListItem5.DisplayText = "VeriFoneXpi";
            valueListItem6.DataValue = "ValueListItem1";
            valueListItem6.DisplayText = "VeriFoneCXpi";
            valueListItem7.DataValue = "ValueListItem2";
            valueListItem7.DisplayText = "VerifoneFormAgentXpi";
            valueListItem8.DataValue = "ValueListItem3";
            valueListItem8.DisplayText = "IngenicoRba";
            valueListItem9.DataValue = "IngenicoUpp";//PRIMEPOS-3063
            valueListItem9.DisplayText = "IngenicoUpp";//PRIMEPOS-3063
            valueListItem13.DataValue = "Null";//PRIMEPOS-3500
            valueListItem13.DisplayText = "Null";//PRIMEPOS-3500

            this.cmbIPLDriver.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem13,valueListItem5,valueListItem6,valueListItem7,valueListItem8,valueListItem9});//PRIMEPOS-3063
        }
        #endregion

        public bool serviceExists(string ServiceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(ServiceName));
        }

        private async void btnStartService_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnStartService_Click() - " + clsPOSDBConstants.Log_Entering);
                using (ServiceController sc = new ServiceController())
                {
                    sc.ServiceName = strServiceName;
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        btnStartService.Enabled = false;
                        pbService.Visible = true;
                        pbService.Value = 0;
                        await Task.Run(() => startService(sc));
                        pbService.Visible = false;
                        btnStartService.Enabled = true;
                    }
                    else
                    {
                        //lblErrMsg.Text = string.Format("Service {0} already running.", strServiceName);
                        lblErrMsg.Text = "TriPOS service is running";
                        lblErrMsg.Appearance.ForeColor = Color.Green;
                    }
                }
                logger.Trace("btnStartService_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private async void btnStopService_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnStopService_Click() - " + clsPOSDBConstants.Log_Entering);
                using (ServiceController sc = new ServiceController())
                {
                    sc.ServiceName = strServiceName;

                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        btnStopService.Enabled = false;
                        pbService.Visible = true;
                        pbService.Value = 0;
                        await Task.Run(() => stopService(sc));
                        pbService.Visible = false;
                        btnStopService.Enabled = true;
                    }
                    else
                    {
                        //lblErrMsg.Text = string.Format("Service {0} already stopped.", strServiceName);
                        lblErrMsg.Text = "TriPOS service is stopped";
                        lblErrMsg.Appearance.ForeColor = Color.Red;
                    }
                }
                logger.Trace("btnStopService_Click() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        public void startService(ServiceController sc)
        {
            logger.Trace("startService() - " + clsPOSDBConstants.Log_Entering);
            try
            {
                // Start the service, and wait until its status is "Running".
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);

                // Display the current service status.
                //lblErrMsg.Text = string.Format("The {0} service status is now set to {1}.", strServiceName, sc.Status.ToString());

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    lblErrMsg.Text = "TriPOS service is running";
                    lblErrMsg.Appearance.ForeColor = Color.Green;
                }
                else
                {
                    lblErrMsg.Text = "TriPOS service is stopped";
                    lblErrMsg.Appearance.ForeColor = Color.Red;
                }

                logger.Trace("startService() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (InvalidOperationException e)
            {
                Resources.Message.Display(string.Format("Could not start the {0} service.", strServiceName), Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void stopService(ServiceController sc)
        {
            logger.Trace("stopService() - " + clsPOSDBConstants.Log_Entering);
            try
            {
                // Start the service, and wait until its status is "Running".
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);

                // Display the current service status.
                //lblErrMsg.Text = string.Format("The {0} service status is now set to {1}.",strServiceName, sc.Status.ToString());

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    lblErrMsg.Text = "TriPOS service is running";
                    lblErrMsg.Appearance.ForeColor = Color.Green;
                }
                else
                {
                    lblErrMsg.Text = "TriPOS service is stopped";
                    lblErrMsg.Appearance.ForeColor = Color.Red;
                }

                logger.Trace("stopService() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (InvalidOperationException e)
            {
                Resources.Message.Display(string.Format("Could not stop the {0} service.", strServiceName), Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Console.WriteLine(e.Message);
            }
        }

        private void LoadConfig(string strConfigFilePath)
        {
            logger.Trace("LoadConfig() - " + clsPOSDBConstants.Log_Entering);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(strConfigFilePath);
            XmlNodeList xmlNodeList;

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/host");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    numAcceptorID.Value = Configuration.convertNullToInt64(xmlNode["acceptorId"].InnerText);
                    numAccountID.Value = Configuration.convertNullToInt64(xmlNode["accountId"].InnerText);
                    txtAccountToken.Text = Configuration.convertNullToString(xmlNode["accountToken"].InnerText);
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                    {
                        txtSLLaneDesc.Text = Configuration.convertNullToString(xmlNode.Attributes["description"].Value);
                        txtSLLaneID.Value = Configuration.convertNullToInt(xmlNode.Attributes["laneId"].Value);
                    }
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane/host");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    txtSLTerminalID.Text = Configuration.convertNullToString(xmlNode["terminalId"].InnerText);
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/application");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    chkTestMode.Checked = Configuration.convertNullToBoolean(xmlNode["testMode"].InnerText);
                    txtPinPadIdelMessage.Text = Configuration.convertNullToString(xmlNode["pinPadIdleMessage"].InnerText);
                    chkquickChip.Checked = Configuration.convertNullToBoolean(xmlNode["quickChip"].InnerText); //PRIMEPOS-3500
                    chkcheckForPreReadId.Checked = Configuration.convertNullToBoolean(xmlNode["checkForPreReadId"].InnerText); //PRIMEPOS-3500
                    chkReturnResponseBeforeCardRemoval.Checked = Configuration.convertNullToBoolean(xmlNode["returnResponseBeforeCardRemoval"].InnerText); //PRIMEPOS-3535
                    txtQuickChipDataLifetime.Text = Configuration.convertNullToString(xmlNode["quickChipDataLifetime"].InnerText); //PRIMEPOS-3500
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/transaction");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    chkAllowPartialApprovals.Checked = Configuration.convertNullToBoolean(xmlNode["allowPartialApprovals"].InnerText);
                    chkConfirmOriginalAmount.Checked = Configuration.convertNullToBoolean(xmlNode["confirmOriginalAmount"].InnerText);
                    txtCreditAvsEntryCondition.Text = Configuration.convertNullToString(xmlNode["creditAvsEntryCondition"].InnerText);
                    txtThresholdAmount.Text = Configuration.convertNullToString(xmlNode["creditSaleSignatureThresholdAmount"].InnerText);//PRIMEPOS-3500
                    chkCheckForDupCCTrans.Checked = Configuration.convertNullToBoolean(xmlNode["checkForDuplicateTransactions"].InnerText);
                    chkVantivsCashBackFeature.Checked = Configuration.convertNullToBoolean(xmlNode["isCashbackAllowed"].InnerText);
                    chkPromptForCCCVVNumbeForKeyedCardTrans.Checked = Configuration.convertNullToBoolean(xmlNode["isCscSupported"].InnerText);
                    chkEnableDebitSale.Checked = Configuration.convertNullToBoolean(xmlNode["isDebitSupported"].InnerText);
                    chkEnableDebitRefunds.Checked = Configuration.convertNullToBoolean(xmlNode["isDebitRefundSupported"].InnerText);
                    chkEnableEBTRefunds.Checked = Configuration.convertNullToBoolean(xmlNode["isEbtRefundSupported"].InnerText);
                    chkEnableGiftCards.Checked = Configuration.convertNullToBoolean(xmlNode["isGiftSupported"].InnerText);
                    chkEnableEBTFoodStamp.Checked = Configuration.convertNullToBoolean(xmlNode["isEbtFoodStampSupported"].InnerText);
                    chkEnableEBTCashBenefit.Checked = Configuration.convertNullToBoolean(xmlNode["isEbtCashBenefitSupported"].InnerText);
                    chkEnableEMVChipProcessing.Checked = Configuration.convertNullToBoolean(xmlNode["isEmvSupported"].InnerText);
                    chkFSAHRACardProcessing.Checked = Configuration.convertNullToBoolean(xmlNode["isHealthcareSupported"].InnerText);
                    chkTippingDoesNotApplyToPharmacyBusiness.Checked = Configuration.convertNullToBoolean(xmlNode["isTipAllowed"].InnerText);
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane/pinpad");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    txtSLTerminalType.Text = Configuration.convertNullToString(xmlNode["terminalType"].InnerText);
                    //txtDriver.Text = Configuration.convertNullToString(xmlNode["driver"].InnerText);
                    cmbSLDriver.Text = Configuration.convertNullToString(xmlNode["driver"].InnerText);
                    txtSLComPort.Text = Configuration.convertNullToString(xmlNode["comPort"].InnerText);
                    txtSLDataBits.Text = Configuration.convertNullToString(xmlNode["dataBits"].InnerText);
                    txtSLParity.Text = Configuration.convertNullToString(xmlNode["parity"].InnerText);
                    txtSLStopBits.Text = Configuration.convertNullToString(xmlNode["stopBits"].InnerText);
                    txtSLHandShake.Text = Configuration.convertNullToString(xmlNode["handshake"].InnerText);
                    txtSLBaudRate.Value = Configuration.convertNullToString(xmlNode["baudRate"].InnerText);
                    chkSLManualEntryAllowed.Checked = Configuration.convertNullToBoolean(xmlNode["isManualEntryAllowed"].InnerText);
                    chkSLContactlessEmvEntryAllowed.Checked = Configuration.convertNullToBoolean(xmlNode["isContactlessEmvEntryAllowed"].InnerText);
                    chkSLContactlessMsdEntryAllowed.Checked = Configuration.convertNullToBoolean(xmlNode["isContactlessMsdEntryAllowed"].InnerText); //PRIMEPOS-3500
                    chkSLDisplayCustomAidScreen.Checked = Configuration.convertNullToBoolean(xmlNode["isDisplayCustomAidScreen"].InnerText); //PRIMEPOS-3500
                    //txtSLMessage.Value = Configuration.convertNullToString(xmlNode["idleScreen"].InnerText);
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane/pinpad/idleScreen");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    txtSLMessage.Text = Configuration.convertNullToString(xmlNode["message"].InnerText);
                }
            }
            catch (Exception Ex)
            {
            }


            #region PRIMEPOS-3024 08-Nov-2021 JY Added
            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                    {
                        txtIPLaneDesc.Text = Configuration.convertNullToString(xmlNode.Attributes["description"].Value);
                        numIPLaneID.Value = Configuration.convertNullToInt(xmlNode.Attributes["laneId"].Value);
                    }
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane/pinpad");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    txtIPLTerminalType.Text = Configuration.convertNullToString(xmlNode["terminalType"].InnerText); //PRIMEPOS-3500
                    cmbIPLDriver.Text = Configuration.convertNullToString(xmlNode["driver"].InnerText);
                    txtIPLIPAddress.Text = Configuration.convertNullToString(xmlNode["ipAddress"].InnerText);
                    txtIPLPort.Text = Configuration.convertNullToString(xmlNode["ipPort"].InnerText);
                    chkIPLManualEntry.Checked = Configuration.convertNullToBoolean(xmlNode["isManualEntryAllowed"].InnerText);
                    chkIPLContactlessEmvEntryAllowed.Checked = Configuration.convertNullToBoolean(xmlNode["isContactlessEmvEntryAllowed"].InnerText);
                    chkIPLContactlessMsdEntryAllowed.Checked = Configuration.convertNullToBoolean(xmlNode["isContactlessMsdEntryAllowed"].InnerText);//PRIMEPOS-3266 //PRIMEPOS-3455
                    if (xmlNode["isDisplayCustomAidScreen"] != null)
                    {
                        chkIPLDisplayCustomAidScreen.Checked = Configuration.convertNullToBoolean(xmlNode["isDisplayCustomAidScreen"].InnerText);//PRIMEPOS-3266
                    }
                    else
                    {
                        chkIPLDisplayCustomAidScreen.Checked = false;
                    }
                    if (xmlNode["isConfirmTotalAmountScreenDisplayed"] != null)
                    {
                        chkIPLConfirmTotalAmountScreenDisplayed.Checked = Configuration.convertNullToBoolean(xmlNode["isConfirmTotalAmountScreenDisplayed"].InnerText);//PRIMEPOS-3266
                    }
                    else
                    {
                        chkIPLConfirmTotalAmountScreenDisplayed.Checked = false;
                    }
                    if (xmlNode["isConfirmCreditSurchargeScreenDisplayed"] != null)
                    {
                        chkIPLConfirmCreditSurchargeScreenDisplayed.Checked = Configuration.convertNullToBoolean(xmlNode["isConfirmCreditSurchargeScreenDisplayed"].InnerText);//PRIMEPOS-3266
                    }
                    else
                    {
                        chkIPLConfirmCreditSurchargeScreenDisplayed.Checked = false;
                    }
                    if (xmlNode["isUnattended"] != null)
                    {
                        chkIPLUnattended.Checked = Configuration.convertNullToBoolean(xmlNode["isUnattended"].InnerText);//PRIMEPOS-3500
                    }
                    else
                    {
                        chkIPLUnattended.Checked = false;
                    }
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane/pinpad/idleScreen");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    txtIPLMessage.Text = Configuration.convertNullToString(xmlNode["message"].InnerText);
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane/host");
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    txtIPLTerminalID.Text = Configuration.convertNullToString(xmlNode["terminalId"].InnerText); //PRIMEPOS-3513
                }
            }
            catch (Exception Ex)
            {
            }
            #endregion

            //Prefrences oPrefrences = new Prefrences();
            //oPrefrences.UpdateTriPOSConfigFilePath(strConfigFilePath);

            LoadDataFromUtil_POSSET();
            PreserveOriginalValues();
            logger.Trace("LoadConfig() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void PreserveOriginalValues()
        {
            try
            {
                logger.Trace("PreserveOriginalValues() - " + clsPOSDBConstants.Log_Entering);
                oTriPOSSettings.TriPOSAcceptorID = Configuration.convertNullToInt64(numAcceptorID.Value);
                oTriPOSSettings.TriPOSAccountID = Configuration.convertNullToInt64(numAccountID.Value);
                oTriPOSSettings.TriPOSAccountToken = txtAccountToken.Text.Trim();
                oTriPOSSettings.TriPOSLaneID = Configuration.convertNullToInt(txtSLLaneID.Value);
                oTriPOSSettings.TriPOSLaneDesc = txtSLLaneDesc.Text.Trim();
                oTriPOSSettings.TriPOSTerminalId = txtSLTerminalID.Text.Trim();
                oTriPOSSettings.TriPOSConfigFilePath = txtTriPOSConfigFilePath.Text.Trim(); //PRIMEPOS-2953 15-Apr-2021 JY Added

                oTriPOSSettings.TPWelcomeMessage = txtPinPadIdelMessage.Text.Trim();
                oTriPOSSettings.TPMessage = txtSLMessage.Text.Trim();
                oTriPOSSettings.ThresholdAmount = txtThresholdAmount.Text.Trim(); //PRIMEPOS-3500
                oTriPOSSettings.EnablePromptForZipCode = txtCreditAvsEntryCondition.Text.Trim();
                oTriPOSSettings.RequiredDriverForVerifonePads = txtSLTerminalType.Text.Trim();
                //oTriPOSSettings.Driver = txtDriver.Text.Trim();
                oTriPOSSettings.Driver = cmbSLDriver.Text.Trim();
                oTriPOSSettings.ComPort = txtSLComPort.Text.Trim();
                oTriPOSSettings.DataBits = Configuration.convertNullToInt(txtSLDataBits.Value);
                oTriPOSSettings.Parity = txtSLParity.Text.Trim();
                oTriPOSSettings.StopBits = txtSLStopBits.Text.Trim();
                oTriPOSSettings.Handshake = txtSLHandShake.Text.Trim();
                oTriPOSSettings.BaudRate = Configuration.convertNullToInt(txtSLBaudRate.Value);

                oTriPOSSettings.TestMode = chkTestMode.Checked;
                oTriPOSSettings.QuickChip = chkquickChip.Checked;//PRIMEPOS-3500
                oTriPOSSettings.ReturnResponseBeforeCardRemoval = chkReturnResponseBeforeCardRemoval.Checked;//PRIMEPOS-3535
                oTriPOSSettings.CheckForPreReadId = chkcheckForPreReadId.Checked;//PRIMEPOS-3500
                oTriPOSSettings.AllowPartialApprovals = chkAllowPartialApprovals.Checked;
                oTriPOSSettings.ConfirmOriginalAmount = chkConfirmOriginalAmount.Checked;
                oTriPOSSettings.CheckForDuplicateCreditCardTransactions = chkCheckForDupCCTrans.Checked;
                oTriPOSSettings.VantivsCashBackFeature = chkVantivsCashBackFeature.Checked;
                oTriPOSSettings.PromptForCreditCardCVVNumberForKeyedCardTransactions = chkPromptForCCCVVNumbeForKeyedCardTrans.Checked;
                oTriPOSSettings.EnableDebitSale = chkEnableDebitSale.Checked;
                oTriPOSSettings.EnableDebitRefunds = chkEnableDebitRefunds.Checked;
                oTriPOSSettings.EnableEBTRefunds = chkEnableEBTRefunds.Checked;
                oTriPOSSettings.EnableGiftCards = chkEnableGiftCards.Checked;
                oTriPOSSettings.EnableEBTFoodStamp = chkEnableEBTFoodStamp.Checked;
                oTriPOSSettings.EnableEBTCashBenefit = chkEnableEBTCashBenefit.Checked;
                oTriPOSSettings.EnableEMVProcessing = chkEnableEMVChipProcessing.Checked;
                oTriPOSSettings.FSAHRACardProcessing = chkFSAHRACardProcessing.Checked;
                oTriPOSSettings.TippingDoesNotApplyToPharmacyBusiness = chkTippingDoesNotApplyToPharmacyBusiness.Checked;
                oTriPOSSettings.ManualCreditCardEntry = chkSLManualEntryAllowed.Checked;
                oTriPOSSettings.NearFieldCommunication = chkSLContactlessEmvEntryAllowed.Checked;
                oTriPOSSettings.ContactlessMsdEntryAllowed = chkSLContactlessMsdEntryAllowed.Checked; //PRIMEPOS-3500
                oTriPOSSettings.DisplayCustomAidScreen = chkSLDisplayCustomAidScreen.Checked; //PRIMEPOS-3500

                #region PRIMEPOS-3024 08-Nov-2021 JY Added
                oTriPOSSettings.TriPOSIPLaneID = Configuration.convertNullToInt(numIPLaneID.Value);
                oTriPOSSettings.TriPOSIPLaneDesc = txtIPLaneDesc.Text.Trim();
                oTriPOSSettings.TriPOSIPTerminalId = txtIPLTerminalID.Text.Trim();
                oTriPOSSettings.IPAddress = txtIPLIPAddress.Text;
                oTriPOSSettings.IPPort = txtIPLPort.Text;
                oTriPOSSettings.IPDriver = cmbIPLDriver.Text;
                oTriPOSSettings.IPMessage = txtIPLMessage.Text.Trim();
                oTriPOSSettings.ManualEntry = chkIPLManualEntry.Checked;
                oTriPOSSettings.NearCommunication = chkIPLContactlessEmvEntryAllowed.Checked;
                oTriPOSSettings.isContactlessMsdEntryAllowed = chkIPLContactlessMsdEntryAllowed.Checked; //PRIMEPOS-3266
                //oTriPOSSettings.isContactlessEmvEntryAllowed = chkContactlessEmvEntryAllowed.Checked; //PRIMEPOS-3266-EMV
                oTriPOSSettings.isDisplayCustomAidScreen = chkIPLDisplayCustomAidScreen.Checked; //PRIMEPOS-3266
                oTriPOSSettings.isConfirmTotalAmountScreenDisplayed = chkIPLConfirmTotalAmountScreenDisplayed.Checked; //PRIMEPOS-3266
                oTriPOSSettings.isConfirmCreditSurchargeScreenDisplayed = chkIPLConfirmCreditSurchargeScreenDisplayed.Checked; //PRIMEPOS-3266
                oTriPOSSettings.Unattended = chkIPLUnattended.Checked; //PRIMEPOS-3500
                #endregion
                logger.Trace("PreserveOriginalValues() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnSetDefaultValues_Click(object sender, EventArgs e)
        {
            logger.Trace("btnSetDefaultValues_Click() - " + clsPOSDBConstants.Log_Entering);
            if(tabLaneData.ActiveTab.Key == "CommonSettings") //PRIMEPOS-3500
            {
                txtPinPadIdelMessage.Text = Configuration.convertNullToString(Configuration.CSetting.TPWelcomeMessage);
                txtCreditAvsEntryCondition.Text = Configuration.convertNullToString(Configuration.CSetting.EnablePromptForZipCode);
                txtThresholdAmount.Text = Configuration.convertNullToString(Configuration.CSetting.ThresholdAmount);
                txtQuickChipDataLifetime.Text = Configuration.convertNullToString(Configuration.CSetting.QuickChipDataLifetime);
                chkTestMode.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.TestMode);
                chkAllowPartialApprovals.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.AllowPartialApprovals);
                chkConfirmOriginalAmount.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.ConfirmOriginalAmount);
                chkCheckForDupCCTrans.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.CheckForDuplicateCreditCardTransactions);
                chkVantivsCashBackFeature.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.VantivsCashBackFeature);
                chkPromptForCCCVVNumbeForKeyedCardTrans.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.PromptForCreditCardCVVNumberForKeyedCardTransactions);
                chkEnableDebitSale.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.EnableDebitSale);
                chkEnableDebitRefunds.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.EnableDebitRefunds);
                chkEnableEBTRefunds.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.EnableEBTRefunds);
                chkEnableGiftCards.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.EnableGiftCards);
                chkEnableEBTFoodStamp.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.EnableEBTFoodStamp);
                chkEnableEBTCashBenefit.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.EnableEBTCashBenefit);
                chkEnableEMVChipProcessing.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.EnableEMVProcessing);
                chkFSAHRACardProcessing.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.FSAHRACardProcessing);
                chkTippingDoesNotApplyToPharmacyBusiness.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.TippingDoesNotApplyToPharmacyBusiness);
                chkquickChip.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.QuickChip);
                chkReturnResponseBeforeCardRemoval.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.ReturnResponseBeforeCardRemoval); //PRIMEPOS-3535
                chkcheckForPreReadId.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.CheckForPreReadId);
            }
            else if (tabLaneData.ActiveTab.Key == "SerialLane")
            {
                txtSLTerminalType.Text = Configuration.convertNullToString(Configuration.CSetting.RequiredDriverForVerifonePads);
                //txtDriver.Text = Configuration.convertNullToString(Configuration.CSetting.Driver);
                cmbSLDriver.Text = Configuration.convertNullToString(Configuration.CSetting.Driver);
                txtSLComPort.Text = Configuration.convertNullToString(Configuration.CSetting.ComPort);
                txtSLDataBits.Text = Configuration.convertNullToString(Configuration.CSetting.DataBits);
                txtSLParity.Text = Configuration.convertNullToString(Configuration.CSetting.Parity);
                txtSLStopBits.Text = Configuration.convertNullToString(Configuration.CSetting.StopBits);
                txtSLHandShake.Text = Configuration.convertNullToString(Configuration.CSetting.Handshake);
                txtSLBaudRate.Value = Configuration.convertNullToInt(Configuration.CSetting.BaudRate);
                txtSLMessage.Text = Configuration.convertNullToString(Configuration.CSetting.TPMessage);

                chkSLManualEntryAllowed.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.ManualCreditCardEntry);
                chkSLContactlessEmvEntryAllowed.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.NearFieldCommunication);
                chkSLContactlessMsdEntryAllowed.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.ContactlessMsdEntryAllowed);
                chkSLDisplayCustomAidScreen.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.DisplayCustomAidScreen);
            }
            else if (tabLaneData.ActiveTab.Key == "IPLane")
            {
                txtIPLTerminalID.Text = Configuration.convertNullToString(Configuration.CSetting.IPTerminalId);
                txtIPLTerminalType.Text = Configuration.convertNullToString(Configuration.CSetting.IPTerminalType);//PRIMEPOS-3500
                txtIPLPort.Text = Configuration.convertNullToString(Configuration.CSetting.IPPort);
                cmbIPLDriver.Text = Configuration.convertNullToString(Configuration.CSetting.IPDriver);
                txtIPLMessage.Text = Configuration.convertNullToString(Configuration.CSetting.IPMessage);
                chkIPLManualEntry.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.IPisManualEntryAllowed);
                chkIPLContactlessEmvEntryAllowed.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.IPisContactlessEmvEntryAllowed);
                chkIPLContactlessMsdEntryAllowed.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.IPisContactlessMsdEntryAllowed);//PRIMEPOS-3266
                //chkContactlessEmvEntryAllowed.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.IPisContactlessEmvEntryAllowed);//PRIMEPOS-3266-EMV
                chkIPLDisplayCustomAidScreen.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.IPisDisplayCustomAidScreen);//PRIMEPOS-3266
                chkIPLConfirmCreditSurchargeScreenDisplayed.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.IPisConfirmTotalAmountScreenDisplayed);//PRIMEPOS-3266
                chkIPLConfirmCreditSurchargeScreenDisplayed.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.IPisConfirmCreditSurchargeScreenDisplayed);//PRIMEPOS-3266
                chkIPLUnattended.Checked = Configuration.convertNullToBoolean(Configuration.CSetting.Unattended);//PRIMEPOS-3500
            }
            logger.Trace("btnSetDefaultValues_Click() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void LoadDataFromUtil_POSSET()
        {
            try
            {
                logger.Trace("LoadDataFromUtil_POSSET() - " + clsPOSDBConstants.Log_Entering);
                Prefrences oPrefrences = new Prefrences();
                DataTable dt = oPrefrences.GetTriPOSSettings();
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Configuration.convertNullToInt(dt.Rows[0]["TriPOSLaneID"]) == 0)
                    {
                        int nLaneId = Configuration.convertNullToInt(Configuration.StationID);
                        string strLaneDesc = "Lane " + Configuration.convertNullToInt(Configuration.StationID).ToString();

                        oPrefrences.UpdateTriPOSSettings(Configuration.convertNullToInt64(numAcceptorID.Value), Configuration.convertNullToInt64(numAccountID.Value), txtAccountToken.Text, nLaneId, strLaneDesc, txtTriPOSConfigFilePath.Text, txtSLTerminalID.Text);
                        numDbAcceptorID.Value = Configuration.convertNullToInt64(numAcceptorID.Value);
                        numDBAccountID.Value = Configuration.convertNullToInt64(numAccountID.Value);
                        txtDBAccountToken.Text = txtAccountToken.Text;
                        txtDBSLLaneID.Value = nLaneId;
                        txtDBSLLaneDesc.Text = strLaneDesc;
                        txtDBSLTerminalID.Text = txtSLTerminalID.Text;
                        UpdateLaneIdInToConfigFile();
                    }
                    else
                    {
                        numDbAcceptorID.Value = Configuration.convertNullToInt64(dt.Rows[0]["TriPOSAcceptorID"]);
                        numDBAccountID.Value = Configuration.convertNullToInt64(dt.Rows[0]["TriPOSAccountID"]);
                        txtDBAccountToken.Text = Configuration.convertNullToString(dt.Rows[0]["TriPOSAccountToken"]);
                        //txtTriPOSConfigFilePath.Text = Configuration.convertNullToString(dt.Rows[0]["TriPOSConfigFilePath"]);
                        txtDBSLLaneID.Value = Configuration.convertNullToInt(dt.Rows[0]["TriPOSLaneID"]);
                        txtDBSLLaneDesc.Text = Configuration.convertNullToString(dt.Rows[0]["TriPOSLaneDesc"]);
                        txtDBSLTerminalID.Text = Configuration.convertNullToString(dt.Rows[0]["TriPOSTerminalId"]);
                    }

                    #region PRIMEPOS-3024 08-Nov-2021 JY Added
                    if (Configuration.convertNullToInt(dt.Rows[0]["TriPOSIPLaneID"]) == 0)
                    {
                        int nLaneId = Configuration.convertNullToInt(Configuration.StationID) + 1000;
                        string strLaneDesc = "Lane " + nLaneId.ToString();
                        oPrefrences.UpdateTriPOSIPLane(nLaneId, strLaneDesc, txtIPLTerminalID.Text);
                        numDBIPLaneID.Value = nLaneId;
                        txtDBIPLaneDesc.Text = strLaneDesc;
                        txtDBIPTerminalId.Text = txtIPLTerminalID.Text;
                        UpdateIPLaneIdInToConfigFile();
                    }
                    else
                    {
                        numDBIPLaneID.Value = Configuration.convertNullToInt(dt.Rows[0]["TriPOSIPLaneID"]);
                        txtDBIPLaneDesc.Text = Configuration.convertNullToString(dt.Rows[0]["TriPOSIPLaneDesc"]);
                        txtDBIPTerminalId.Text = Configuration.convertNullToString(dt.Rows[0]["TriPOSIPTerminalId"]);
                    }
                    #endregion
                }
                logger.Trace("LoadDataFromUtil_POSSET() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
            }
            VerifySettingsWithDb();
        }

        private void UpdateLaneIdInToConfigFile()
        {
            try
            {
                logger.Trace("UpdateLaneIdInToConfigFile() - " + clsPOSDBConstants.Log_Entering);
                string strConfigFilePath = txtTriPOSConfigFilePath.Text.ToString();
                bool bUpdateStatus = false;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(strConfigFilePath);
                XmlNodeList xmlNodeList;

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                        {
                            if (Configuration.convertNullToInt(txtDBSLLaneID.Text) != Configuration.convertNullToInt(txtSLLaneID.Value))
                            {
                                xmlNode.Attributes["laneId"].Value = Configuration.convertNullToInt(txtDBSLLaneID.Value).ToString();
                                bUpdateStatus = true;
                            }
                            if (txtDBSLLaneDesc.Text.Trim().ToUpper() != txtSLLaneDesc.Text.Trim().ToUpper())
                            {
                                xmlNode.Attributes["description"].Value = txtDBSLLaneDesc.Text.Trim();
                                bUpdateStatus = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    if (bUpdateStatus)
                    {
                        //copy file
                        string sourceDir, backupDir;
                        sourceDir = backupDir = Path.GetDirectoryName(strConfigFilePath);

                        string destFileName = "triPOS" + DateTime.Now.ToString("MMddyyyy HHmm") + ".config";
                        File.Copy(Path.Combine(sourceDir, strConfigFileName), Path.Combine(backupDir, destFileName), true);
                    }
                }
                catch (Exception Ex)
                {
                }
                xmlDocument.Save(strConfigFilePath);
                txtSLLaneID.Value = Configuration.convertNullToInt(txtDBSLLaneID.Value);
                txtSLLaneDesc.Text = txtDBSLLaneDesc.Text.Trim();
                if (oTriPOSSettings != null)
                {
                    oTriPOSSettings.TriPOSLaneID = Configuration.convertNullToInt(txtSLLaneID.Value);
                    oTriPOSSettings.TriPOSLaneDesc = txtSLLaneDesc.Text.Trim();
                }
                logger.Trace("UpdateLaneIdInToConfigFile() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #region PRIMEPOS-3024 08-Nov-2021 JY Added  
        private void UpdateIPLaneIdInToConfigFile()
        {
            try
            {
                logger.Trace("UpdateIPLaneIdInToConfigFile() - " + clsPOSDBConstants.Log_Entering);
                string strConfigFilePath = txtTriPOSConfigFilePath.Text.ToString();
                bool bUpdateStatus = false;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(strConfigFilePath);
                XmlNodeList xmlNodeList;

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                        {
                            if (Configuration.convertNullToInt(numDBIPLaneID.Text) != Configuration.convertNullToInt(numIPLaneID.Value))
                            {
                                xmlNode.Attributes["laneId"].Value = Configuration.convertNullToInt(numDBIPLaneID.Value).ToString();
                                bUpdateStatus = true;
                            }
                            if (txtDBIPLaneDesc.Text.Trim().ToUpper() != txtIPLaneDesc.Text.Trim().ToUpper())
                            {
                                xmlNode.Attributes["description"].Value = txtDBIPLaneDesc.Text.Trim();
                                bUpdateStatus = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    if (bUpdateStatus)
                    {
                        //copy file
                        string sourceDir, backupDir;
                        sourceDir = backupDir = Path.GetDirectoryName(strConfigFilePath);

                        string destFileName = "triPOS" + DateTime.Now.ToString("MMddyyyy HHmm") + ".config";
                        File.Copy(Path.Combine(sourceDir, strConfigFileName), Path.Combine(backupDir, destFileName), true);
                    }
                }
                catch (Exception Ex)
                {
                }
                xmlDocument.Save(strConfigFilePath);
                numIPLaneID.Value = Configuration.convertNullToInt(numDBIPLaneID.Value);
                txtIPLaneDesc.Text = txtDBIPLaneDesc.Text.Trim();
                if (oTriPOSSettings != null)
                {
                    oTriPOSSettings.TriPOSIPLaneID = Configuration.convertNullToInt(numIPLaneID.Value);
                    oTriPOSSettings.TriPOSIPLaneDesc = txtIPLaneDesc.Text.Trim();
                }
                logger.Trace("UpdateLaneIdInToConfigFile() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        #endregion

        private void VerifySettingsWithDb()
        {
            logger.Trace("VerifySettingsWithDb() - " + clsPOSDBConstants.Log_Entering);
            if (Configuration.convertNullToInt64(numAcceptorID.Value) != Configuration.convertNullToInt64(numDbAcceptorID.Value))
                numAcceptorID.Appearance.BackColor = Color.SkyBlue;
            if (Configuration.convertNullToInt64(numAccountID.Value) != Configuration.convertNullToInt64(numDBAccountID.Value))
                numAccountID.Appearance.BackColor = Color.SkyBlue;
            if (txtAccountToken.Text.Trim().ToUpper() != txtDBAccountToken.Text.Trim().ToUpper())
                txtAccountToken.Appearance.BackColor = Color.SkyBlue;
            if (Configuration.convertNullToInt(txtSLLaneID.Value) != Configuration.convertNullToInt(txtDBSLLaneID.Value))
                txtSLLaneID.Appearance.BackColor = Color.SkyBlue;
            if (txtSLLaneDesc.Text.Trim().ToUpper() != txtDBSLLaneDesc.Text.Trim().ToUpper())
                txtSLLaneDesc.Appearance.BackColor = Color.SkyBlue;
            if (txtSLTerminalID.Text.Trim().ToUpper() != txtDBSLTerminalID.Text.Trim().ToUpper())
                txtSLTerminalID.Appearance.BackColor = Color.SkyBlue;
            logger.Trace("VerifySettingsWithDb() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void btnTriPOSConfigFilePath_Click(object sender, EventArgs e)
        {
            logger.Trace("btnTriPOSConfigFilePath_Click() - " + clsPOSDBConstants.Log_Entering);
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\Program Files (x86)\Vantiv\triPOS Service",
                Title = "Browse TriPOS config file",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "config",
                Filter = "config files(*.config) | *.config",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = false,
                FileName = "triPOS.config"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtTriPOSConfigFilePath.Text = openFileDialog1.FileName;
                LoadConfig(txtTriPOSConfigFilePath.Text);
            }
            logger.Trace("btnTriPOSConfigFilePath_Click() - " + clsPOSDBConstants.Log_Exiting);
        }

        //private string GetConfigFilePath()
        //{
        //    logger.Trace("GetConfigFilePath() - " + clsPOSDBConstants.Log_Entering);
        //    string strConfigFilePath = string.Empty;

        //    using (ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + strServiceName + "'"))
        //    {
        //        wmiService.Get();
        //        string currentserviceExePath = wmiService["PathName"].ToString();
        //        var cleanPath = string.Join("", currentserviceExePath.Split(Path.GetInvalidPathChars()));
        //        if (cleanPath.Length > 0 && cleanPath.EndsWith(@"\"))
        //            cleanPath = cleanPath.Substring(0, cleanPath.Length - 1);

        //        strConfigFilePath = Path.GetDirectoryName(cleanPath);
        //    }
        //    logger.Trace("GetConfigFilePath() - " + clsPOSDBConstants.Log_Exiting);
        //    return strConfigFilePath;
        //}

        private void btnAcceptorID_Click(object sender, EventArgs e)
        {
            if (numAcceptorID.Value != numDbAcceptorID.Value)
                numAcceptorID.Value = numDbAcceptorID.Value;
        }

        private void btnAccountID_Click(object sender, EventArgs e)
        {
            if (numAccountID.Value != numDBAccountID.Value)
                numAccountID.Value = numDBAccountID.Value;
        }

        private void btnAccountToken_Click(object sender, EventArgs e)
        {
            if (txtAccountToken.Text.Trim().ToUpper() != txtDBAccountToken.Text.Trim().ToUpper())
                txtAccountToken.Text = txtDBAccountToken.Text;
        }

        private void btnLaneID_Click(object sender, EventArgs e)
        {
            if (txtSLLaneID.Value != txtDBSLLaneID.Value)
                txtSLLaneID.Value = txtDBSLLaneID.Value;
        }

        private void btnLaneDesc_Click(object sender, EventArgs e)
        {
            if (txtSLLaneDesc.Text.Trim().ToUpper() != txtDBSLLaneDesc.Text.Trim().ToUpper())
                txtSLLaneDesc.Text = txtDBSLLaneDesc.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                logger.Trace("btnSave_Click() - UpdateTriPOSSettings - " + clsPOSDBConstants.Log_Entering);
                Prefrences oPrefrences = new Prefrences();
                oPrefrences.UpdateTriPOSSettings(Configuration.convertNullToInt64(numAcceptorID.Value), Configuration.convertNullToInt64(numAccountID.Value), txtAccountToken.Text, Configuration.convertNullToInt(txtSLLaneID.Value), txtSLLaneDesc.Text, txtTriPOSConfigFilePath.Text, txtSLTerminalID.Text);
                LoadDataFromUtil_POSSET();
                logger.Trace("btnSave_Click() - UpdateTriPOSSettings - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
            }

            #region save config            
            try
            {
                logger.Trace("btnSave_Click() - Save config - " + clsPOSDBConstants.Log_Entering);
                string strConfigFilePath = txtTriPOSConfigFilePath.Text.ToString();
                bool bUpdateStatus = false;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(strConfigFilePath);
                XmlNodeList xmlNodeList;

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/host");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (oTriPOSSettings.TriPOSAcceptorID != Configuration.convertNullToInt64(numAcceptorID.Value))
                        {
                            xmlNode["acceptorId"].InnerText = Configuration.convertNullToInt64(numAcceptorID.Value).ToString();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.TriPOSAccountID != Configuration.convertNullToInt64(numAccountID.Value))
                        {
                            xmlNode["accountId"].InnerText = Configuration.convertNullToInt64(numAccountID.Value).ToString();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.TriPOSAccountToken != txtAccountToken.Text.Trim())
                        {
                            xmlNode["accountToken"].InnerText = txtAccountToken.Text.Trim();
                            bUpdateStatus = true;
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                        {
                            if (oTriPOSSettings.TriPOSLaneID != Configuration.convertNullToInt(txtSLLaneID.Value))
                            {
                                xmlNode.Attributes["laneId"].Value = Configuration.convertNullToInt(txtSLLaneID.Value).ToString();
                                bUpdateStatus = true;
                            }
                            if (oTriPOSSettings.TriPOSLaneDesc != txtSLLaneDesc.Text.Trim())
                            {
                                xmlNode.Attributes["description"].Value = txtSLLaneDesc.Text.Trim();
                                bUpdateStatus = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane/host");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        txtSLTerminalID.Text = Configuration.convertNullToString(xmlNode["terminalId"].InnerText);
                        if (oTriPOSSettings.TriPOSTerminalId != txtSLTerminalID.Text.Trim())
                        {
                            xmlNode["terminalId"].InnerText = txtSLTerminalID.Text.Trim();
                            bUpdateStatus = true;
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    if (bUpdateStatus)
                    {
                        //copy file
                        string sourceDir, backupDir;
                        sourceDir = backupDir = Path.GetDirectoryName(strConfigFilePath);

                        string destFileName = "triPOS" + DateTime.Now.ToString("MMddyyyy HHmm") + ".config";
                        File.Copy(Path.Combine(sourceDir, strConfigFileName), Path.Combine(backupDir, destFileName), true);
                    }
                }
                catch (Exception Ex)
                {
                }

                xmlDocument.Save(strConfigFilePath);
                if (oTriPOSSettings != null)
                {
                    oTriPOSSettings.TriPOSAcceptorID = Configuration.convertNullToInt64(numAcceptorID.Value);
                    oTriPOSSettings.TriPOSAccountID = Configuration.convertNullToInt64(numAccountID.Value);
                    oTriPOSSettings.TriPOSAccountToken = txtAccountToken.Text.Trim();
                    oTriPOSSettings.TriPOSLaneID = Configuration.convertNullToInt(txtSLLaneID.Value);
                    oTriPOSSettings.TriPOSLaneDesc = txtSLLaneDesc.Text.Trim();
                    oTriPOSSettings.TriPOSTerminalId = txtSLTerminalID.Text.Trim();
                    oTriPOSSettings.TriPOSConfigFilePath = txtTriPOSConfigFilePath.Text.Trim();   //PRIMEPOS-2953 15-Apr-2021 JY Added
                }

                logger.Trace("btnSave_Click() - Save config - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            #endregion
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            #region PRIMEPOS-2953 15-Apr-2021 JY Added
            try
            {
                Prefrences oPrefrences = new Prefrences();
                oPrefrences.UpdateTriPOSSettings(Configuration.convertNullToInt64(numAcceptorID.Value), Configuration.convertNullToInt64(numAccountID.Value), txtAccountToken.Text.Trim(), Configuration.convertNullToInt(txtSLLaneID.Value), txtSLLaneDesc.Text.Trim(), txtTriPOSConfigFilePath.Text.Trim(), txtSLTerminalID.Text.Trim());
                oPrefrences.UpdateTriPOSIPLane(Configuration.convertNullToInt(numIPLaneID.Value), txtIPLaneDesc.Text.Trim(), txtIPLTerminalID.Text.Trim());   //PRIMEPOS-3024 08-Nov-2021 JY Added
                LoadDataFromUtil_POSSET();
            }
            catch (Exception Ex)
            {
            }
            #endregion

            bool bStatus = SaveConfig(txtTriPOSConfigFilePath.Text);

            if (bStatus)
                Resources.Message.Display("Record saved successfully.", Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool SaveConfig(string strConfigFilePath)
        {
            bool bReturn = true;
            try
            {
                logger.Trace("SaveConfig() - " + clsPOSDBConstants.Log_Entering);
                bool bUpdateStatus = false;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(strConfigFilePath);
                XmlNodeList xmlNodeList;

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/host");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (oTriPOSSettings.TriPOSAcceptorID != Configuration.convertNullToInt64(numAcceptorID.Value))
                        {
                            xmlNode["acceptorId"].InnerText = Configuration.convertNullToInt64(numAcceptorID.Value).ToString();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.TriPOSAccountID != Configuration.convertNullToInt64(numAccountID.Value))
                        {
                            xmlNode["accountId"].InnerText = Configuration.convertNullToInt64(numAccountID.Value).ToString();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.TriPOSAccountToken != txtAccountToken.Text.Trim())
                        {
                            xmlNode["accountToken"].InnerText = txtAccountToken.Text.Trim();
                            bUpdateStatus = true;
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                        {
                            if (oTriPOSSettings.TriPOSLaneID != Configuration.convertNullToInt(txtSLLaneID.Value))
                            {
                                xmlNode.Attributes["laneId"].Value = Configuration.convertNullToInt(txtSLLaneID.Value).ToString();
                                bUpdateStatus = true;
                            }
                            if (oTriPOSSettings.TriPOSLaneDesc != txtSLLaneDesc.Text.Trim())
                            {
                                xmlNode.Attributes["description"].Value = txtSLLaneDesc.Text.Trim();
                                bUpdateStatus = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane/host");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (oTriPOSSettings.TriPOSTerminalId != txtSLTerminalID.Text.Trim())
                        {
                            xmlNode["terminalId"].InnerText = txtSLTerminalID.Text.Trim();
                            bUpdateStatus = true;
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/application");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (oTriPOSSettings.TestMode != chkTestMode.Checked)
                        {
                            xmlNode["testMode"].InnerText = chkTestMode.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.TPWelcomeMessage != txtPinPadIdelMessage.Text.Trim())
                        {
                            xmlNode["pinPadIdleMessage"].InnerText = txtPinPadIdelMessage.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.QuickChip != chkquickChip.Checked)
                        {
                            xmlNode["quickChip"].InnerText = chkquickChip.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.CheckForPreReadId != chkcheckForPreReadId.Checked)
                        {
                            xmlNode["checkForPreReadId"].InnerText = chkcheckForPreReadId.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.QuickChipDataLifetime != txtQuickChipDataLifetime.Text.Trim())
                        {
                            xmlNode["quickChipDataLifetime"].InnerText = txtQuickChipDataLifetime.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.ReturnResponseBeforeCardRemoval != chkReturnResponseBeforeCardRemoval.Checked)//PRIMEPOS-3535
                        {
                            xmlNode["returnResponseBeforeCardRemoval"].InnerText = chkReturnResponseBeforeCardRemoval.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/transaction");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (oTriPOSSettings.AllowPartialApprovals != chkAllowPartialApprovals.Checked)
                        {
                            xmlNode["allowPartialApprovals"].InnerText = chkAllowPartialApprovals.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.ConfirmOriginalAmount != chkConfirmOriginalAmount.Checked)
                        {
                            xmlNode["confirmOriginalAmount"].InnerText = chkConfirmOriginalAmount.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.EnablePromptForZipCode != txtCreditAvsEntryCondition.Text.Trim())
                        {
                            xmlNode["creditAvsEntryCondition"].InnerText = txtCreditAvsEntryCondition.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.CheckForDuplicateCreditCardTransactions != chkCheckForDupCCTrans.Checked)
                        {
                            xmlNode["checkForDuplicateTransactions"].InnerText = chkCheckForDupCCTrans.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.ThresholdAmount != txtThresholdAmount.Text.Trim()) //PRIMEPOS-3500
                        {
                            xmlNode["creditSaleSignatureThresholdAmount"].InnerText = txtThresholdAmount.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.VantivsCashBackFeature != chkVantivsCashBackFeature.Checked)
                        {
                            xmlNode["isCashbackAllowed"].InnerText = chkVantivsCashBackFeature.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.PromptForCreditCardCVVNumberForKeyedCardTransactions != chkPromptForCCCVVNumbeForKeyedCardTrans.Checked)
                        {
                            xmlNode["isCscSupported"].InnerText = chkPromptForCCCVVNumbeForKeyedCardTrans.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.EnableDebitSale != chkEnableDebitSale.Checked)
                        {
                            xmlNode["isDebitSupported"].InnerText = chkEnableDebitSale.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.EnableDebitRefunds != chkEnableDebitRefunds.Checked)
                        {
                            xmlNode["isDebitRefundSupported"].InnerText = chkEnableDebitRefunds.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.EnableEBTRefunds != chkEnableEBTRefunds.Checked)
                        {
                            xmlNode["isEbtRefundSupported"].InnerText = chkEnableEBTRefunds.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.EnableGiftCards != chkEnableGiftCards.Checked)
                        {
                            xmlNode["isGiftSupported"].InnerText = chkEnableGiftCards.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.EnableEBTFoodStamp != chkEnableEBTFoodStamp.Checked)
                        {
                            xmlNode["isEbtFoodStampSupported"].InnerText = chkEnableEBTFoodStamp.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.EnableEBTCashBenefit != chkEnableEBTCashBenefit.Checked)
                        {
                            xmlNode["isEbtCashBenefitSupported"].InnerText = chkEnableEBTCashBenefit.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.EnableEMVProcessing != chkEnableEMVChipProcessing.Checked)
                        {
                            xmlNode["isEmvSupported"].InnerText = chkEnableEMVChipProcessing.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.FSAHRACardProcessing != chkFSAHRACardProcessing.Checked)
                        {
                            xmlNode["isHealthcareSupported"].InnerText = chkFSAHRACardProcessing.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.TippingDoesNotApplyToPharmacyBusiness != chkTippingDoesNotApplyToPharmacyBusiness.Checked)
                        {
                            xmlNode["isTipAllowed"].InnerText = chkTippingDoesNotApplyToPharmacyBusiness.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane/pinpad");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (oTriPOSSettings.RequiredDriverForVerifonePads != txtSLTerminalType.Text.Trim())
                        {
                            xmlNode["terminalType"].InnerText = txtSLTerminalType.Text.Trim();
                            bUpdateStatus = true;
                        }
                        //if (oTriPOSSettings.Driver != txtDriver.Text.Trim())
                        //{
                        //    xmlNode["driver"].InnerText = txtDriver.Text.Trim();
                        //    bUpdateStatus = true;
                        //}
                        if (oTriPOSSettings.Driver != cmbSLDriver.Text.Trim())
                        {
                            xmlNode["driver"].InnerText = cmbSLDriver.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.ComPort != txtSLComPort.Text.Trim())
                        {
                            xmlNode["comPort"].InnerText = txtSLComPort.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.DataBits != Configuration.convertNullToInt(txtSLDataBits.Value))
                        {
                            xmlNode["dataBits"].InnerText = Configuration.convertNullToInt(txtSLDataBits.Value).ToString();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.Parity != txtSLParity.Text.Trim())
                        {
                            xmlNode["parity"].InnerText = txtSLParity.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.StopBits != txtSLStopBits.Text.Trim())
                        {
                            xmlNode["stopBits"].InnerText = txtSLStopBits.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.Handshake != txtSLHandShake.Text.Trim())
                        {
                            xmlNode["handshake"].InnerText = txtSLHandShake.Text.Trim();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.BaudRate != Configuration.convertNullToInt(txtSLBaudRate.Value))
                        {
                            xmlNode["baudRate"].InnerText = Configuration.convertNullToInt(txtSLBaudRate.Value).ToString();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.ManualCreditCardEntry != chkSLManualEntryAllowed.Checked)
                        {
                            xmlNode["isManualEntryAllowed"].InnerText = chkSLManualEntryAllowed.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.NearFieldCommunication != chkSLContactlessEmvEntryAllowed.Checked)
                        {
                            xmlNode["isContactlessEmvEntryAllowed"].InnerText = chkSLContactlessEmvEntryAllowed.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.ContactlessMsdEntryAllowed != chkSLContactlessMsdEntryAllowed.Checked) //PRIMEPOS-3500
                        {
                            xmlNode["isContactlessMsdEntryAllowed"].InnerText = chkSLContactlessMsdEntryAllowed.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.DisplayCustomAidScreen != chkSLDisplayCustomAidScreen.Checked) //PRIMEPOS-3500
                        {
                            xmlNode["isDisplayCustomAidScreen"].InnerText = chkSLDisplayCustomAidScreen.Checked.ToString().ToLower();
                            bUpdateStatus = true;
                        }
                        if (oTriPOSSettings.TPMessage != txtSLMessage.Text.Trim()) //PRIMEPOS-3500
                        {
                            //xmlNode["idleScreen"].InnerText = txtSLMessage.Text.Trim();
                            //bUpdateStatus = true;
                            XmlNode idleScreenNode = xmlNode["idleScreen"];
                            if (idleScreenNode != null)
                            {
                                XmlNode messageNode = idleScreenNode["message"];
                                if (messageNode != null)
                                {
                                    if (oTriPOSSettings.TPMessage != txtSLMessage.Text.Trim())
                                    {
                                        messageNode.InnerText = txtSLMessage.Text.Trim();
                                        bUpdateStatus = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                #region PRIMEPOS-3024 08-Nov-2021 JY Added
                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                        {
                            if (oTriPOSSettings.TriPOSIPLaneID != Configuration.convertNullToInt(numIPLaneID.Value))
                            {
                                xmlNode.Attributes["laneId"].Value = Configuration.convertNullToInt(numIPLaneID.Value).ToString();
                                bUpdateStatus = true;
                            }
                            if (oTriPOSSettings.TriPOSIPLaneDesc != txtIPLaneDesc.Text.Trim())
                            {
                                xmlNode.Attributes["description"].Value = txtIPLaneDesc.Text.Trim();
                                bUpdateStatus = true;
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                }

                #region PRIMEPOS-3500
                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane/host");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (oTriPOSSettings.TriPOSIPTerminalId != txtIPLTerminalID.Text.Trim())
                        {
                            xmlNode["terminalId"].InnerText = txtIPLTerminalID.Text.Trim(); //PRIMEPOS-3513
                            bUpdateStatus = true;
                        }
                    }
                }
                catch (Exception Ex)
                {
                }
                #endregion

                try
                {
                    xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane");
                    foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                        {
                            for (int i = 0; i < xmlNode["pinpad"].ChildNodes.Count; i++)
                            {
                                if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "terminalType".ToUpper()) //PRIMEPOS-3500
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(txtIPLTerminalType.Text);
                                }
                                if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "driver".ToUpper())
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(cmbIPLDriver.Text);
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "ipAddress".ToUpper())
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(txtIPLIPAddress.Text);
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "ipPort".ToUpper())
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(txtIPLPort.Text);
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "isManualEntryAllowed".ToUpper())
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(chkIPLManualEntry.Checked).ToLower();
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "isContactlessEmvEntryAllowed".ToUpper())
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(chkIPLContactlessEmvEntryAllowed.Checked).ToLower();
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "isContactlessMsdEntryAllowed".ToUpper()) //PRIMEPOS-3266
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(chkIPLContactlessMsdEntryAllowed.Checked).ToLower();
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "isDisplayCustomAidScreen".ToUpper()) //PRIMEPOS-3266
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(chkIPLDisplayCustomAidScreen.Checked).ToLower();
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "isConfirmTotalAmountScreenDisplayed".ToUpper()) //PRIMEPOS-3266
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(chkIPLConfirmTotalAmountScreenDisplayed.Checked).ToLower();
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "isConfirmCreditSurchargeScreenDisplayed".ToUpper()) //PRIMEPOS-3266
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(chkIPLConfirmCreditSurchargeScreenDisplayed.Checked).ToLower();
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "isUnattended".ToUpper()) //PRIMEPOS-3500
                                {
                                    xmlNode["pinpad"].ChildNodes[i].InnerText = Configuration.convertNullToString(chkIPLUnattended.Checked).ToLower();
                                }
                                else if (xmlNode["pinpad"].ChildNodes[i].Name.Trim().ToUpper() == "idleScreen".ToUpper())
                                {
                                    for (int j = 0; j < xmlNode["pinpad"].ChildNodes[i].ChildNodes?.Count; j++)
                                    {
                                        if (xmlNode["pinpad"].ChildNodes[i].ChildNodes[j].Name.Trim().ToUpper() == "message".ToUpper())
                                        {
                                            xmlNode["pinpad"].ChildNodes[i].ChildNodes[j].InnerText = txtIPLMessage.Text;
                                        }
                                    }
                                }
                            }
                            if (xmlNode["host"] != null)
                            {
                                for (int i = 0; i < xmlNode["host"].ChildNodes.Count; i++)
                                {
                                    if (xmlNode["host"].ChildNodes[i].Name.Trim().ToUpper() == "terminalId".ToUpper()) //PRIMEPOS-3513
                                    {
                                        xmlNode["host"].ChildNodes[i].InnerText = Configuration.convertNullToString(txtIPLTerminalID.Text);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                }
                #endregion

                try
                {
                    if (bUpdateStatus)
                    {
                        //copy file
                        string sourceDir, backupDir;
                        sourceDir = backupDir = Path.GetDirectoryName(strConfigFilePath);

                        string destFileName = "triPOS" + DateTime.Now.ToString("MMddyyyy HHmm") + ".config";
                        File.Copy(Path.Combine(sourceDir, strConfigFileName), Path.Combine(backupDir, destFileName), true);
                    }
                }
                catch (Exception Ex)
                {
                }

                xmlDocument.Save(strConfigFilePath);
                PreserveOriginalValues();
                logger.Trace("SaveConfig() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                bReturn = false;
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            return bReturn;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTerminalId_Click(object sender, EventArgs e)
        {
            if (txtSLTerminalID.Text.Trim().ToUpper() != txtDBSLTerminalID.Text.Trim().ToUpper())
                txtSLTerminalID.Text = txtDBSLTerminalID.Text;
        }

        private void frmTriPOSSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                btnClose_Click(btnClose, new System.EventArgs());
                e.Handled = true;
            }
        }

        private void btnIPLaneID_Click(object sender, EventArgs e)
        {
            if (numIPLaneID.Value != numDBIPLaneID.Value)
                numIPLaneID.Value = numDBIPLaneID.Value;
        }

        private void btnIPLaneDesc_Click(object sender, EventArgs e)
        {
            if (txtIPLaneDesc.Text.Trim().ToUpper() != txtDBIPLaneDesc.Text.Trim().ToUpper())
                txtIPLaneDesc.Text = txtDBIPLaneDesc.Text;
        }

        private void btnIPTerminalId_Click(object sender, EventArgs e)
        {
            if (txtIPLTerminalID.Text.Trim().ToUpper() != txtDBIPTerminalId.Text.Trim().ToUpper())
                txtIPLTerminalID.Text = txtDBIPTerminalId.Text;
        }
    }

    public class TriPOSSettings
    {
        public long TriPOSAcceptorID;
        public long TriPOSAccountID;
        public string TriPOSAccountToken;
        public int TriPOSLaneID;
        public string TriPOSLaneDesc;
        public string TriPOSTerminalId;
        public string TriPOSConfigFilePath; //PRIMEPOS-2953 15-Apr-2021 JY Added
        public bool QuickChip;//PRIMEPOS-3500
        public bool CheckForPreReadId;//PRIMEPOS-3500
        public string QuickChipDataLifetime; //PRIMEPOS-3500
        public string ThresholdAmount;//PRIMEPOS-3500
        public bool ContactlessMsdEntryAllowed;//PRIMEPOS-3500
        public bool ReturnResponseBeforeCardRemoval; //PRIMEPOS-3535
        public bool DisplayCustomAidScreen;//PRIMEPOS-3500
        public bool Unattended;//PRIMEPOS-3500
        public string IPTerminalType;//PRIMEPOS-3500

        public string TPWelcomeMessage;
        public string TPMessage;
        public string EnablePromptForZipCode;
        public string RequiredDriverForVerifonePads;
        public string Driver;
        public string ComPort;
        public int DataBits;
        public string Parity;
        public string StopBits;
        public string Handshake;
        public int BaudRate;

        public bool TestMode;
        public bool AllowPartialApprovals;
        public bool ConfirmOriginalAmount;
        public bool CheckForDuplicateCreditCardTransactions;
        public bool VantivsCashBackFeature;
        public bool PromptForCreditCardCVVNumberForKeyedCardTransactions;
        public bool EnableDebitSale;
        public bool EnableDebitRefunds;
        public bool EnableEBTRefunds;
        public bool EnableGiftCards;
        public bool EnableEBTFoodStamp;
        public bool EnableEBTCashBenefit;
        public bool EnableEMVProcessing;
        public bool FSAHRACardProcessing;
        public bool TippingDoesNotApplyToPharmacyBusiness;
        public bool ManualCreditCardEntry;
        public bool NearFieldCommunication;

        #region PRIMEPOS-3024 08-Nov-2021 JY Added
        public int TriPOSIPLaneID;
        public string TriPOSIPLaneDesc;
        public string TriPOSIPTerminalId;
        public string IPAddress;
        public string IPPort;
        public string IPDriver;
        public string IPMessage;
        public bool ManualEntry;
        public bool NearCommunication;
        public bool isContactlessMsdEntryAllowed;//PRIMEPOS-3266
        public bool isContactlessEmvEntryAllowed;//PRIMEPOS-3266
        public bool isDisplayCustomAidScreen;//PRIMEPOS-3266
        public bool isConfirmTotalAmountScreenDisplayed;//PRIMEPOS-3266
        public bool isConfirmCreditSurchargeScreenDisplayed;//PRIMEPOS-3266
        #endregion
    }
}