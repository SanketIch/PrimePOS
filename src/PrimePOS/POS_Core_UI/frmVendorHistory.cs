using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.ErrorLogging;



using POS_Core.DataAccess;namespace POS_Core_UI
{
    public partial class frmVendorHistory : Form
    {
        string vendorid = string.Empty;
        MMS.PROCESSOR.MMSDictionary<string, int> dict = null;
        DataSet datasetVendorID = new DataSet();
        DataSet oDataSet = null;
        private SearchSvr oSearchSvr= new SearchSvr();
        private bool  isValueChanged = false;
        private PurchseOrdStatus poStatus = PurchseOrdStatus.All;

        public frmVendorHistory(string vendid)
        {
            InitializeComponent();
            LoadVendorList();
            cboStatusList.SelectedIndex = (int)PurchseOrdStatus.All;
            vendorid = vendid;
            SetCombValue();           
        }

        private void SetCombValue()
        {
            try
            {
                cboVendorList.SelectedIndex = 0;
                if (datasetVendorID.Tables.Count > 0 && datasetVendorID.Tables[0].Rows.Count > 0)
                {
                    this.cboVendorList.Enabled = true;
                    DataRow[] row = datasetVendorID.Tables[0].Select(clsPOSDBConstants.Vendor_Fld_VendorId+"='" + vendorid.ToString() + "'");
                    if (row.Length > 0)
                    {
                        bool isExist = this.cboVendorList.IsItemInList(row[0][clsPOSDBConstants.Vendor_Fld_VendorName].ToString());
                        this.cboVendorList.Value = Convert.ToString(row[0][clsPOSDBConstants.Vendor_Fld_VendorName].ToString());
                    }
                }                 
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex,"","");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void LoadVendorList()
        {
            DataTable dataTable = new DataTable();
            DataColumn oCol1 = dataTable.Columns.Add(clsPOSDBConstants.Vendor_Fld_VendorId);
            DataColumn oCol2 = dataTable.Columns.Add(clsPOSDBConstants.Vendor_Fld_VendorName);
            POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor(); 
            int count = 0;

            try
            {
                cboVendorList.SelectedIndex = 0;
                VendorData vendorData = vendor.PopulateList(" Where IsActive=1");

                foreach (VendorRow vendorRow in vendorData.Vendor.Rows)
                {
                     DataRow row = dataTable.NewRow();
                     cboVendorList.Items.Add(vendorRow.Vendorname);
                     row[clsPOSDBConstants.Vendor_Fld_VendorName] = vendorRow.Vendorname;
                     row[clsPOSDBConstants.Vendor_Fld_VendorId] = vendorRow.VendorId;
                     dataTable.Rows.Add(row);
                     count++;
                }
                datasetVendorID.Tables.Add(dataTable);         
            }           
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        

        private void frmVendorHistory_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            Search();
        }

        private void Search()
        {

            try
            {
                oDataSet = new DataSet();
                string sSQL = GetString();
                oDataSet.Tables.Add(oSearchSvr.Search(sSQL).Tables[0].Copy());
                oDataSet.Tables[0].TableName = "Master";

                if (oDataSet.Tables[0].Rows.Count == 0)
                {
                    grdSearch.DataSource = oDataSet;
                    return;
                }

                DataSet poDetials = new DataSet();
                foreach (DataRow row in oDataSet.Tables[0].Rows)
                {

                    DataSet details = new DataSet();
                    sSQL = "Select "
                             + " POD." + clsPOSDBConstants.PODetail_Fld_OrderID + " As [OrderID] "
                             + " , POD." + clsPOSDBConstants.PODetail_Fld_ItemID
                             + " , itm." + clsPOSDBConstants.Item_Fld_Description + " as [Item Name] "
                             + " , " + clsPOSDBConstants.PODetail_Fld_QTY
                             + " , " + clsPOSDBConstants.PODetail_Fld_Cost
                             + " , " + clsPOSDBConstants.PODetail_Fld_Comments
                             + " , POD." + clsPOSDBConstants.PODetail_Fld_AckStatus + " as [Ack Status] "
                             + " , POD." + clsPOSDBConstants.PODetail_Fld_AckQTY + " as [Act Qty] "
                             + " FROM "
                             + clsPOSDBConstants.PODetail_tbl + " as POD "
                             + " , " + clsPOSDBConstants.POHeader_tbl + " as POH "
                             + " , " + clsPOSDBConstants.Item_tbl + " as itm "
                             + ", " + clsPOSDBConstants.Vendor_tbl + " as Cus "
                             + " where POH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Cus." + clsPOSDBConstants.Vendor_Fld_VendorId
                             + " and POH." + clsPOSDBConstants.POHeader_Fld_OrderID + " = POD." + clsPOSDBConstants.PODetail_Fld_OrderID
                             + " and POD." + clsPOSDBConstants.PODetail_Fld_ItemID + " = itm." + clsPOSDBConstants.PODetail_Fld_ItemID
                             + " and Cus." + clsPOSDBConstants.Vendor_Fld_VendorId + "=" + vendorid;

                    sSQL += " and POH." + clsPOSDBConstants.POHeader_Fld_OrderID + "=" + row[clsPOSDBConstants.POHeader_Fld_OrderID].ToString();
                    details = oSearchSvr.Search(sSQL);
                    poDetials.Merge(details);
                }

                oDataSet.Tables.Add(poDetials.Tables[0].Copy());
                oDataSet.Tables[1].TableName = "Detail";
                oDataSet.Relations.Add("MasterDetail", oDataSet.Tables[0].Columns["OrderID"], oDataSet.Tables[1].Columns["OrderID"]);
                grdSearch.DataSource = oDataSet;
                grdSearch.DisplayLayout.Bands[0].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.FontData.SizeInPoints = 12;
                grdSearch.DisplayLayout.Bands[0].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdSearch.DisplayLayout.Bands[0].Header.Caption = "Purchase Order";

                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_OrderID].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.POHeader_Fld_OrderID].Hidden = true;

                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_Status].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_isFTPUsed].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.POHeader_Fld_isInvRecieved].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["Exp. Delv. Date"].Hidden = true;

                grdSearch.DisplayLayout.Bands[0].Columns["Is Inv Received"].Hidden = true;  //PRIMEPOS-2824 25-Mar-2020 JY modified
                grdSearch.DisplayLayout.Bands[0].Columns["Host Status"].Hidden = true;

                grdSearch.DisplayLayout.Bands[1].Columns[clsPOSDBConstants.PODetail_Fld_Comments].Hidden = true;
                grdSearch.DisplayLayout.Bands[1].Expandable = true;
                grdSearch.DisplayLayout.Bands[1].HeaderVisible = true;
                grdSearch.DisplayLayout.Bands[1].Header.Caption = "Order Detail";
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.FontData.SizeInPoints = 12;
                grdSearch.DisplayLayout.Bands[1].Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
                
                grdSearch.Focus();
                this.grdSearch.Refresh();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }        
        }

        private string GetString()
        {
            string viewPurchaseOrd = string.Empty;  
            string sSQL = "Select "
             + " TH." + clsPOSDBConstants.POHeader_Fld_OrderID + " as [OrderID]"
             + " , TH." + clsPOSDBConstants.POHeader_Fld_OrderNo + " as [Order No]"             
             + " , TH." + clsPOSDBConstants.POHeader_Fld_Description + " as [Description]"
             + " , Cus." + clsPOSDBConstants.Vendor_Fld_VendorName + " as [Vendor]"
             + " , cast(" + clsPOSDBConstants.POHeader_Fld_OrderDate + " as varchar) as [Order Date]"
             + " , " + "case " + clsPOSDBConstants.POHeader_Fld_Status + " when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 10  then 'PartiallyAck' when 11  then 'PartiallyAck-Reorder' when 12 then 'Error' when 13 then 'Overdue' when 14 then 'SubmittedManually' when 15 then 'DirectAcknowledge' end as [PO Status]" //Change By SRT (Sachin) Date : 19 Feb 2010
             + " , cast(" + clsPOSDBConstants.POHeader_Fld_ExptDelvDate + " as varchar) as [Exp. Delv. Date]"
             + " , " + "case " + clsPOSDBConstants.POHeader_Fld_isFTPUsed + " when 0 then 'Not Used' when 1 then 'Ack Not Processed' when 2 then 'Ack Processed' end as [Host Status]"
             + " , " + "case isInvRecieved when 1 then 'Yes' else 'No' end  as [Is Inv Received]"
             + " , " + clsPOSDBConstants.POHeader_Fld_Status
             + " , " + clsPOSDBConstants.POHeader_Fld_isFTPUsed
             + " , IsNull(" + clsPOSDBConstants.POHeader_Fld_isInvRecieved + ",0) as " + clsPOSDBConstants.POHeader_Fld_isInvRecieved
             + " FROM "
             + clsPOSDBConstants.POHeader_tbl + " as TH "
             + ", " + clsPOSDBConstants.Vendor_tbl + " as Cus "
             + " where TH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Cus." + clsPOSDBConstants.Vendor_Fld_VendorId             
             +" and Cus." + clsPOSDBConstants.Vendor_Fld_VendorId + "="+vendorid;
            
            if (isValueChanged)
            {
                sSQL += " and convert(datetime,orderdate,109) between convert(datetime, cast('" + this.dtpFromDate.Text + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + this.dtpToDate.Text + " 23:59:59 ' as datetime) ,113) ";
            }
            else
            {
                sSQL += " and convert(datetime,orderdate,109) between convert(datetime, cast('" + DateTime.Now.Subtract(new TimeSpan(365, 0, 0, 0)).ToShortDateString() + " 00:00:00 ' as datetime) ,113) and convert(datetime, cast('" + DateTime.Now.ToShortDateString() + " 23:59:59 ' as datetime) ,113) ";
            }
            if (poStatus != PurchseOrdStatus.All)
            {
                sSQL += " and TH." + clsPOSDBConstants.POHeader_Fld_Status + " = " + (int)poStatus;
            }
            sSQL += " order by " + clsPOSDBConstants.POHeader_Fld_OrderDate + " desc ";
            return sSQL;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboVendorList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (datasetVendorID.Tables.Count > 0 && datasetVendorID.Tables[0].Rows.Count > 0)
                {
                    this.cboVendorList.Enabled = true;
                    DataRow[] row = datasetVendorID.Tables[0].Select(clsPOSDBConstants.Vendor_Fld_VendorName + "='" + this.cboVendorList.Value.ToString() + "'");
                    if (row.Length > 0)
                    {
                        vendorid = row[0][clsPOSDBConstants.Vendor_Fld_VendorId].ToString();
                    }
                }
               Search();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {                  
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }
                    return;
                }
                isValueChanged = true;
                
            }
            catch (Exception ex)
            {
            }
        }
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
                isValueChanged = true;
            }
            catch (Exception ex)
            {
            }
        }
        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                {
                    isValid = false;
                    fieldName = "DATE";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = field;
            return isValid;
        }
        private void cboStatusList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                    }

                    return;
                }
                poStatus = GetStatus(this.cboStatusList.Text.ToString());
                Search();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private PurchseOrdStatus GetStatus(string order)
        {
            PurchseOrdStatus poStatus = PurchseOrdStatus.All;
            switch (order)
            {

                case clsPOSDBConstants.Incomplete:
                    poStatus = PurchseOrdStatus.Incomplete;
                    break;
                case clsPOSDBConstants.Processed:
                    poStatus = PurchseOrdStatus.Processed;
                    break;
                case clsPOSDBConstants.Pending:
                    poStatus = PurchseOrdStatus.Pending;
                    break;
                case clsPOSDBConstants.Queued:
                    poStatus = PurchseOrdStatus.Queued;
                    break;
                case clsPOSDBConstants.Canceled:
                    poStatus = PurchseOrdStatus.Canceled;
                    break;
                case clsPOSDBConstants.Acknowledge:
                    poStatus = PurchseOrdStatus.Acknowledge;
                    break;
                case clsPOSDBConstants.AcknowledgeManually:
                    poStatus = PurchseOrdStatus.AcknowledgeManually;
                    break;
                case clsPOSDBConstants.Submitted:
                    poStatus = PurchseOrdStatus.Submitted;
                    break;
                case clsPOSDBConstants.MaxAttempt:
                    poStatus = PurchseOrdStatus.MaxAttempt;
                    break;
                case clsPOSDBConstants.Expired:
                    poStatus = PurchseOrdStatus.Expired;
                    break;
                case clsPOSDBConstants.PartiallyAcknowledge:
                    poStatus = PurchseOrdStatus.PartiallyAck;
                    break;
                case clsPOSDBConstants.PartiallyAckReorder:
                    poStatus = PurchseOrdStatus.PartiallyAckReorder;
                    break;
                case clsPOSDBConstants.Error:
                    poStatus = PurchseOrdStatus.Error;
                    break;
                case clsPOSDBConstants.All:
                    poStatus = PurchseOrdStatus.All;
                    break;
                case clsPOSDBConstants.Overdue:
                    poStatus = PurchseOrdStatus.Overdue;
                    break;
                case clsPOSDBConstants.SubmittedManually:
                    poStatus = PurchseOrdStatus.SubmittedManually;
                    break;
                    //Add by SRT(Sachn) Date : 18 Feb 2010
                case clsPOSDBConstants.DirectAcknowledge:
                    poStatus = PurchseOrdStatus.DirectAcknowledge;
                    break;
                    //End of Add by SRT(Sachn) Date : 18 Feb 2010
                    
            }
            return poStatus;
        }

        private void cboStatusList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                cboStatusList.DropDown(); 
            }
        }
    }
}