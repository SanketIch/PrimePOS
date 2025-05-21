using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace POS_Core_UI
{
    public partial class frmNewLaneTripos : Form
    {
        string serialLane = "SERIAL LANE";
        string IPLane = "IP LANE";
        public bool IsClosed = false;
        public string ConfigFilePath = string.Empty;
        public string strConfigFileName = string.Empty;
        DataSet lanesDataSet = new DataSet();
        public frmNewLaneTripos()
        {
            InitializeComponent();            
        }

        private void LoadSettings()
        {
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            valueListItem1.DataValue = "ValueListItem0";
            valueListItem1.DisplayText = "VERIFONE MX915";
            valueListItem2.DataValue = "ValueListItem1";
            valueListItem2.DisplayText = "VERIFONE MX925";
            valueListItem3.DataValue = "ValueListItem2";
            valueListItem3.DisplayText = "INGENICO ISMP";

            this.cmbDevice.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,valueListItem2,valueListItem3});


            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            valueListItem4.DataValue = "ValueListItem0";
            valueListItem4.DisplayText = serialLane;
            valueListItem5.DataValue = "ValueListItem1";
            valueListItem5.DisplayText = IPLane;

            this.cmbLaneType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem4,valueListItem5});

            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();//PRIMEPOS-3063
            valueListItem6.DataValue = "ValueListItem0";
            valueListItem6.DisplayText = "VeriFoneXpi";
            valueListItem7.DataValue = "ValueListItem1";
            valueListItem7.DisplayText = "VeriFoneCXpi";
            valueListItem8.DataValue = "ValueListItem2";
            valueListItem8.DisplayText = "VerifoneFormAgentXpi";
            valueListItem9.DataValue = "ValueListItem3";
            valueListItem9.DisplayText = "IngenicoRba";
            valueListItem10.DataValue = "IngenicoUpp";//PRIMEPOS-3063
            valueListItem10.DisplayText = "IngenicoUpp";//PRIMEPOS-3063

            this.cmbDriver.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem6,valueListItem7,valueListItem8,valueListItem9,valueListItem10});//PRIMEPOS-3063
        }

        private void cmbLaneType_SelectionChanged(object sender, EventArgs e)
        {
            if (this.cmbLaneType.Text.Equals(serialLane))
            {
                txtIPAddress.Enabled = false;
                txtPort.Enabled = false;
                txtComPort.Enabled = true;
                txtDataBits.Enabled = true;
                txtHandshake.Enabled = true;
                txtParity.Enabled = true;
                txtStopBits.Enabled = true;
                numBaudRate.Enabled = true;
            }
            else
            {
                txtIPAddress.Enabled = true;
                txtPort.Enabled = true;
                txtComPort.Enabled = false;
                txtDataBits.Enabled = false;
                txtHandshake.Enabled = false;
                txtParity.Enabled = false;
                txtStopBits.Enabled = false;
                numBaudRate.Enabled = false;
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbDriver.Text) || string.IsNullOrWhiteSpace(cmbDevice.Text))
            {
                clsUIHelper.ShowErrorMsg("Select Lane and Device type");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtLaneId.Text))
            {
                clsUIHelper.ShowErrorMsg("Enter Lane ID");
                return;
            }
            else
            {
                var laneFound  = lanesDataSet.Tables[0].AsEnumerable().Where(a => a.Field<string>("LaneID") == txtLaneId.Text).ToList();
                if (laneFound.Count > 0)
                {
                    clsUIHelper.ShowErrorMsg("Lane Already in use. Please use other lane id");
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(ConfigFilePath) && ConfigFilePath.Contains(".config"))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(@ConfigFilePath);

                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("tripos");
                if (cmbLaneType.Text == serialLane)
                {
                    foreach (XmlNode xmlnodes in xmlNodeList)
                    {
                        XmlAttribute attribute1 = xmlDocument.CreateAttribute("description");
                        attribute1.Value = txtDescription.Text;
                        XmlAttribute attribute2 = xmlDocument.CreateAttribute("laneId");
                        attribute2.Value = txtLaneId.Text;
                        XmlNode nodeserialLane = xmlDocument.CreateElement("serialLane");
                        XmlNode nodePinpad = xmlDocument.CreateElement("pinpad");
                        XmlNode nodehost = xmlDocument.CreateElement("host");
                        XmlNode terminalType = xmlDocument.CreateElement("terminalType");
                        terminalType.InnerText = "PointOfSale";
                        XmlNode nodestore = xmlDocument.CreateElement("store");
                        XmlNode transactionAmountLimit = xmlDocument.CreateElement("transactionAmountLimit");
                        transactionAmountLimit.InnerText = txtTransLimit.Text;
                        nodestore.AppendChild(transactionAmountLimit);
                        XmlNode terminalID = xmlDocument.CreateElement("terminalId"); //PRIMEPOS-3513
                        terminalID.InnerText = txtTerminalID.Text;
                        nodehost.AppendChild(terminalID);

                        XmlNode driver = xmlDocument.CreateElement("driver");
                        driver.InnerText = cmbDriver.Text;
                        XmlNode ManualEntry = xmlDocument.CreateElement("isManualEntryAllowed");
                        ManualEntry.InnerText = Configuration.convertNullToString(chkManualEntry.Checked).ToLower();
                        XmlNode NearCommunication = xmlDocument.CreateElement("isContactlessEmvEntryAllowed");
                        NearCommunication.InnerText = Configuration.convertNullToString(chkNearCommunication.Checked).ToLower();
                        XmlNode NearCommunicationMSD = xmlDocument.CreateElement("isContactlessMsdEntryAllowed");
                        NearCommunicationMSD.InnerText = "false";
                        XmlNode comPort = xmlDocument.CreateElement("comPort");
                        comPort.InnerText = txtComPort.Text;
                        XmlNode dataBits = xmlDocument.CreateElement("dataBits");
                        dataBits.InnerText = txtDataBits.Text;
                        XmlNode parity = xmlDocument.CreateElement("parity");
                        parity.InnerText = txtParity.Text;
                        XmlNode stopBits = xmlDocument.CreateElement("stopBits");
                        stopBits.InnerText = txtStopBits.Text;
                        XmlNode handshake = xmlDocument.CreateElement("handshake");
                        handshake.InnerText = txtHandshake.Text;
                        XmlNode baudRate = xmlDocument.CreateElement("baudRate");
                        baudRate.InnerText = numBaudRate.Text;
                        XmlNode idleScreen = xmlDocument.CreateElement("idleScreen");
                        idleScreen.InnerText = txtMessage.Text;

                        xmlnodes["lanes"].AppendChild(nodeserialLane);
                        nodeserialLane.AppendChild(nodePinpad);
                        nodeserialLane.AppendChild(nodehost);
                        nodePinpad.AppendChild(terminalType);
                        nodePinpad.AppendChild(driver);
                        nodePinpad.AppendChild(NearCommunication);
                        nodePinpad.AppendChild(NearCommunicationMSD);
                        nodePinpad.AppendChild(ManualEntry);
                        nodePinpad.AppendChild(comPort);
                        nodePinpad.AppendChild(dataBits);
                        nodePinpad.AppendChild(parity);
                        nodePinpad.AppendChild(stopBits);
                        nodePinpad.AppendChild(handshake);
                        nodePinpad.AppendChild(baudRate);
                        nodePinpad.AppendChild(idleScreen);

                        nodeserialLane.Attributes.Append(attribute1);
                        nodeserialLane.Attributes.Append(attribute2);

                    }
                }
                else
                {
                    foreach (XmlNode xmlnodes in xmlNodeList)
                    {
                        XmlAttribute attribute1 = xmlDocument.CreateAttribute("description");
                        attribute1.Value = txtDescription.Text;
                        XmlAttribute attribute2 = xmlDocument.CreateAttribute("laneId");
                        attribute2.Value = txtLaneId.Text;
                        XmlNode nodeIPlLane = xmlDocument.CreateElement("ipLane");
                        XmlNode nodePinpad = xmlDocument.CreateElement("pinpad");
                        XmlNode nodehost = xmlDocument.CreateElement("host");
                        XmlNode terminalType = xmlDocument.CreateElement("terminalType");
                        terminalType.InnerText = "PointOfSale";
                        XmlNode nodestore = xmlDocument.CreateElement("store");
                        XmlNode transactionAmountLimit = xmlDocument.CreateElement("transactionAmountLimit");
                        transactionAmountLimit.InnerText = txtTransLimit.Text;
                        nodestore.AppendChild(transactionAmountLimit);
                        XmlNode terminalID = xmlDocument.CreateElement("terminalId"); //PRIMEPOS-3513
                        terminalID.InnerText = txtTerminalID.Text;
                        nodehost.AppendChild(terminalID);

                        XmlNode driver = xmlDocument.CreateElement("driver");
                        driver.InnerText = cmbDriver.Text;
                        XmlNode ipAddress = xmlDocument.CreateElement("ipAddress");
                        ipAddress.InnerText = txtIPAddress.Text;
                        XmlNode ipPort = xmlDocument.CreateElement("ipPort");
                        ipPort.InnerText = txtPort.Text;
                        //XmlNode parity = xmlDocument.CreateElement("parity");
                        //parity.InnerText = txtParity.Text;
                        //XmlNode stopBits = xmlDocument.CreateElement("stopBits");
                        //stopBits.InnerText = txtStopBits.Text;
                        //XmlNode handshake = xmlDocument.CreateElement("handshake");
                        //handshake.InnerText = txtHandshake.Text;
                        //XmlNode baudRate = xmlDocument.CreateElement("baudRate");
                        //baudRate.InnerText = numBaudRate.Text;
                        XmlNode ManualEntry = xmlDocument.CreateElement("isManualEntryAllowed");
                        ManualEntry.InnerText = Configuration.convertNullToString(chkManualEntry.Checked).ToLower();
                        XmlNode NearCommunication = xmlDocument.CreateElement("isContactlessEmvEntryAllowed");
                        NearCommunication.InnerText = Configuration.convertNullToString(chkNearCommunication.Checked).ToLower();
                        XmlNode NearCommunicationMSD = xmlDocument.CreateElement("isContactlessMsdEntryAllowed");
                        NearCommunicationMSD.InnerText = "false";
                        XmlNode idleScreen = xmlDocument.CreateElement("idleScreen");
                        //idleScreen.InnerText = txtMessage.Text;
                        XmlNode message = xmlDocument.CreateElement("message");
                        message.InnerText = txtMessage.Text;
                        idleScreen.AppendChild(message);

                        xmlnodes["lanes"].AppendChild(nodeIPlLane);
                        nodeIPlLane.AppendChild(nodePinpad);
                        nodeIPlLane.AppendChild(nodehost);
                        nodePinpad.AppendChild(terminalType);
                        nodePinpad.AppendChild(driver);
                        nodePinpad.AppendChild(NearCommunication);
                        nodePinpad.AppendChild(NearCommunicationMSD);
                        nodePinpad.AppendChild(ManualEntry);
                        nodePinpad.AppendChild(ipAddress);
                        nodePinpad.AppendChild(ipPort);
                        nodePinpad.AppendChild(idleScreen);

                        nodeIPlLane.Attributes.Append(attribute1);
                        nodeIPlLane.Attributes.Append(attribute2);

                    }
                }

                //copy file
                string sourceDir, backupDir;
                sourceDir = backupDir = Path.GetDirectoryName(ConfigFilePath);

                string destFileName = "triPOS" + DateTime.Now.ToString("MMddyyyy HHmm") + ".config";
                File.Copy(Path.Combine(sourceDir, strConfigFileName), Path.Combine(backupDir, destFileName), true);

                xmlDocument.Save(ConfigFilePath);
                clsUIHelper.ShowOKMsg("Record Saved Successfully");
            }
            else
            {
                clsUIHelper.ShowErrorMsg("Could not find Tripos.config file");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            IsClosed = true;
            this.Close();
        }

        private void frmNewLaneTripos_Load(object sender, EventArgs e)
        {
            LoadSettings();            
            DataTable sampleDataTable = lanesDataSet.Tables.Add("Lanes");

            sampleDataTable.Columns.Add("LaneID", typeof(string));
            sampleDataTable.Columns.Add("Description", typeof(string));
            DataRow sampleDataRow;
            //for (int i = 1; i <= 49; i++)
            //{
                //sampleDataRow = sampleDataTable.NewRow();
                //sampleDataRow["FirstColumn"] = "Cell1: " + i.ToString(CultureInfo.CurrentCulture);
                //sampleDataRow["SecondColumn"] = "Cell2: " + i.ToString(CultureInfo.CurrentCulture);
                //sampleDataTable.Rows.Add(sampleDataRow);
            //}
            Dictionary<string, string> lanes = new Dictionary<string, string>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@ConfigFilePath);

            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/serialLane");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                {
                    //lanes.Add(Configuration.convertNullToString(xmlNode.Attributes["laneId"].Value), Configuration.convertNullToString(xmlNode.Attributes["description"].Value));
                    sampleDataRow = sampleDataTable.NewRow();
                    sampleDataRow["LaneID"] = Configuration.convertNullToString(xmlNode.Attributes["laneId"].InnerText);
                    sampleDataRow["Description"] = Configuration.convertNullToString(xmlNode.Attributes["description"].InnerText);
                    sampleDataTable.Rows.Add(sampleDataRow);
                }
            }
            xmlNodeList = xmlDocument.SelectNodes("tripos/lanes/ipLane");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                {
                    sampleDataRow = sampleDataTable.NewRow();
                    sampleDataRow["LaneID"] = Configuration.convertNullToString(xmlNode.Attributes["laneId"].InnerText);
                    sampleDataRow["Description"] = Configuration.convertNullToString(xmlNode.Attributes["description"].InnerText);
                    sampleDataTable.Rows.Add(sampleDataRow);
                }
            }

            //var data = lanes.Select(x => new List<object> { x.Key, x.Value });

            grdLaneGrid.DataSource = lanesDataSet;
            grdLaneGrid.DataBind();
            
        }

        private void grdLaneGrid_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            this.grdLaneGrid.ActiveRow.Cells["LaneID"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grdLaneGrid.ActiveRow.Cells["Description"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

        }

        private void grdLaneGrid_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            e.Row.Cells["Description"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            e.Row.Cells["LaneID"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
        }
    }
}
