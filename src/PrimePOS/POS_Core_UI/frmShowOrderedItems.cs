using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinSchedule;
using POS_Core.ErrorLogging;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using POS_Core.DataAccess;
using POS_Core.Resources;
using NLog; //PRIMEPOS-2652 06-Dec-2019 JY Added

namespace POS_Core_UI
{
    public partial class frmShowOrderedItems : Form
    {
        private DataSet dataSet = null;
        private DataTable dataTable = null;
        public bool IsCanceled = true;
        private int rowCount = 0;
        private bool AnyOneUnchecked = true;

	    private TimeSpan _defaultStartDateEndDateDifference = TimeSpan.FromDays(30);

	    private const string fetchItemQueryTemplate =
		    "Select POD.PODetailId,POD.ItemID as [Item Id],Itm.Description,POD.VendorItemCode as [Vendor ItemCode],PO.OrderID as [Order Id],PO.VendorID as [Vendor Id],vend.VendorName as [Vendor Name] ,OrderDate as [Order Date]," +
		    "case Status when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled'" +
		    "when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 10  then 'PartiallyAcknowledge' when 11 then 'PartiallyAck-Reorder' when 12 then 'Error' when 13 then 'Overdue' when 14 then 'SubmittedManually' when 15 then 'DirectAcknowledge' when 17 then 'DeliveryReceived' end as [PO Status]" +
		    //Change By SRT (Sachin) Date : 19 Feb 2010
		    "From PurchaseOrder as PO ,PurchaseOrderDetail as POD ,Item as Itm,Vendor as vend where Itm.ItemID = POD.ItemID AND PO.OrderID = POD.OrderID AND PO.VendorID=vend.VendorID " +
		    " And OrderDate >= convert(datetime, cast('{0} 00:00:00' as datetime), 113) And OrderDate <= convert(datetime, cast('{1} 23:59:59' as datetime) ,113)";

        private ILogger logger = LogManager.GetCurrentClassLogger();    //PRIMEPOS-2652 06-Dec-2019 JY Added

        public frmShowOrderedItems()
        {
			InitializeComponent();

			dtpEndDate.Value = DateTime.Now;
			dtpStartDate.Value = DateTime.Now.AddDays(-_defaultStartDateEndDateDifference.TotalDays);
			dataSet = new DataSet();
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool Edit()
        {
            bool EditResult = false;
            Search search = new Search();

            try
            {
                //string fetchItemQuery = GetItemQuery(GetStartDate(), GetEndDate());   //PRIMEPOS-2652 17-Feb-2020 JY Commented
                string fetchItemQuery = Search();   //PRIMEPOS-2652 17-Feb-2020 JY Added

                dataSet = search.SearchData(fetchItemQuery);

                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    this.gridItemDetails.DataSource = dataSet;
                    rowCount = dataSet.Tables[0].Rows.Count;
                    ultraTxtEditorNoOfOrdItems.Text = rowCount.ToString();
                    
					AddColoumnInGridView();

	                // Commented by Shrikant Mali on 06/02/2014 so as to make it reusable in updated item details method.
					//foreach (var uRow in gridItemDetails.Rows)
					//	uRow.Cells["Select"].Value = false;
                    UpdateSetSelectAllRows(gridItemDetails.Rows, false);

                    EditResult = true;
                }
                else
                {
                    EditResult = false;
                }
            }
            catch (Exception ex)
            {
                EditResult = false;
                //ErrorHandler.logException(ex, "", "");
                logger.Fatal(ex, "Edit()");
            }
            return (EditResult);
        }

	    private void AddColoumnInGridView()
	    {
		    this.gridItemDetails.DisplayLayout.Bands[0].Columns.Add("Select", "");
		    this.gridItemDetails.DisplayLayout.Bands[0].Columns["Select"].Style =
			    Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
		    this.gridItemDetails.DisplayLayout.Bands[0].Columns["Select"].Header.VisiblePosition = 0;
		    this.gridItemDetails.DisplayLayout.Bands[0].Columns["Select"].Width = 10;
		    this.gridItemDetails.DisplayLayout.Bands[0].Columns["PODetailId"].Hidden = true;
	    }

		/// <summary>
		/// Auther : Shrikant Mali
		/// Date : 11/02/2014
		/// Gets the date selected as end date.
		/// </summary>
		/// <returns></returns>
	    private DateTime GetEndDate()
	    {
		    return (DateTime) dtpEndDate.Value;
	    }

		/// <summary>
		/// Auther : Shrikant Mali
		/// Date : 11/02/2014
		/// Gets the date selected as start date.
		/// </summary>
		/// <returns></returns>
	    private DateTime GetStartDate()
	    {
		    return (DateTime)dtpStartDate.Value;
	    }

	    private static void UpdateSetSelectAllRows(IEnumerable<UltraGridRow> rowsCollection, bool selectStatus)
	    {
		    foreach (var uRow in rowsCollection)
			    uRow.Cells["Select"].Value = selectStatus;
	    }

	    private void UpdateItemDetails()
	    {
		    try
		    {
			    var isToAddColoumns = gridItemDetails.Rows.Count == 0;

				//var fetchItemQuery = GetItemQuery((DateTime)dtpStartDate.Value, (DateTime)dtpEndDate.Value);    //PRIMEPOS-2652 17-Feb-2020 JY Commented
                string fetchItemQuery = Search();   //PRIMEPOS-2652 17-Feb-2020 JY Added

                using (var search = new Search())
					dataSet = search.SearchData(fetchItemQuery);

				gridItemDetails.DataSource = dataSet;

				if(isToAddColoumns)
					AddColoumnInGridView();

				UpdateSetSelectAllRows(gridItemDetails.Rows, false);

				ultraTxtEditorNoOfOrdItems.Text = gridItemDetails.Rows.Count.ToString(CultureInfo.InvariantCulture);
		    }
		    catch (Exception ex)
		    {
				//ErrorHandler.logException(ex, "", "");
                logger.Fatal(ex, "UpdateItemDetails()");
            }
			
	    }

		/// <summary>
		/// Auther : Shrikant Mali
		/// Date : 13/02/2014
		/// Gets the query to retrieve ordered items withing the date range.
		/// </summary>
		/// <param name="startDate">The start date of date range..</param>
		/// <param name="endDate">The end date of date range.</param>
		/// <returns></returns>
	    private static string GetItemQuery(DateTime startDate, DateTime endDate)
	    {
		    return string.Format(fetchItemQueryTemplate, startDate.ToShortDateString(), endDate.ToShortDateString());
	    }

	    private void frmShowOrderedItems_Load(object sender, EventArgs e)
        {
            //Added By SRT(Gaurav) Date: 10-Jul-2009
            clsUIHelper.setColorSchecme(this);
            //End OF Added By SRT(Gaurav)
            
        }

        private void btnIncludeSelectedItem_Click(object sender, EventArgs e)
        {
            try
            {
                IsCanceled = false;             
                this.Close();
            }
            catch(Exception exp)
            { 
                clsUIHelper.ShowErrorMsg(exp.Message); 
            }
        }

        public string SelectedRowID()
        {
            if (gridItemDetails.ActiveRow != null)
                if (gridItemDetails.ActiveRow.Cells.Count > 0)
                    return gridItemDetails.ActiveRow.Cells[0].Text;
                else
                    return "";
            else
                return "";
        }

        public PODetailData SelectedItems
        {
           get
            {
                PODetailData dsPoDetails = new PODetailData();
                PODetailSvr po = new PODetailSvr();
                List<string> poDetailIds = new List<string>();

                foreach (UltraGridRow uRow in gridItemDetails.Rows)
                {
                  if (Configuration.convertNullToBoolean(uRow.Cells["Select"].Value))
                   {
                         poDetailIds.Add(uRow.Cells["PODetailId"].Value.ToString());
                   }
                }
                if(poDetailIds.Count >0)
                  dsPoDetails = po.Populate(poDetailIds);
                return (dsPoDetails);
           }           
        }

        private void btnExcludeSelectedItem_Click(object sender, EventArgs e)
        {
            ExcludeFromData();             
        }

        private void ExcludeFromData()
        {
           try
            {
                if (gridItemDetails.Selected.Rows.Count > 0 && this.gridItemDetails.ActiveRow != null)
                {
                   for(int rCount = 0; rCount <= gridItemDetails.Selected.Rows.Count; rCount++)
                   {
                     gridItemDetails.Selected.Rows[0].Delete(false);
                     rowCount--;
                   }
                   this.gridItemDetails.UpdateData();               
                   this.ultraTxtEditorNoOfOrdItems.Text = rowCount.ToString();
                }
                else
                    clsUIHelper.ShowErrorMsg("Plese Select Row First");
            }
            catch (Exception ex)
            {
               clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void gridItemDetails_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Point point = System.Windows.Forms.Cursor.Position;
                point = this.gridItemDetails.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.gridItemDetails.DisplayLayout.UIElement.ElementFromPoint(point);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.RowUIElement))
                    {
                        if (gridItemDetails.Rows.Count == 0)
                            return;
                        IsCanceled = false;
                        this.Close();
                    }
                    oUI = oUI.Parent;
                }
            }
            catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        private void gridItemDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnIncludeSelectedItem_Click(this, new EventArgs());
            }
            else if (e.KeyChar == (char)Keys.Space)
            {
                gridItemDetails.ActiveRow.Cells["Select"].Value = !Configuration.convertNullToBoolean(gridItemDetails.ActiveRow.Cells["Select"].Value);
                if (!Configuration.convertNullToBoolean(gridItemDetails.ActiveRow.Cells["Select"].Value))
                {
                //    chkSelectAll.Checked = false;
                    AnyOneUnchecked = true;
                }
                //gridItemDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextRow);
            }
            else
            {
                e.Handled = true;
            }
            
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                foreach (UltraGridRow uRow in gridItemDetails.Rows)
                {
                    uRow.Cells["Select"].Value = chkSelectAll.Checked;
                }
                AnyOneUnchecked = false;
            }     
        }

        private void chkSelectAll_Click(object sender, EventArgs e)
        {

            if (!AnyOneUnchecked && !chkSelectAll.Checked)
            {
				UpdateSetSelectAllRows(gridItemDetails.Rows, false);
				//Commented by Shrikant so as to use the above fuction.
				//foreach (UltraGridRow uRow in gridItemDetails.Rows)
				//{
				//	uRow.Cells["Select"].Value = false;
				//}
            }

        }

        private void gridItemDetails_Click(object sender, EventArgs e)
        {
			// Commented by Shrikant Mali on 11/02/2014 so as to avoid the null reference execption.
			//if (Configuration.convertNullToBoolean(gridItemDetails.ActiveRow.Cells[9].Value) == true)
			if (gridItemDetails.ActiveRow != null && Configuration.convertNullToBoolean(gridItemDetails.ActiveRow.Cells[9].Value) == true)
			{
				//chkSelectAll.Checked = false;
				AnyOneUnchecked = true;
			}
        }

	    private void btnSearch_Click(object sender, EventArgs e)
	    {            
            UpdateItemDetails();
        }

		private void dtpStartDate_Leave(object sender, EventArgs e)
		{
			if (GetStartDate() > GetEndDate())
			{
				DisplayWarning("Start Date cannot be greater than the end date");
				dtpStartDate.Value = GetEndDate().AddDays(-1);
			}
		}

	    private void dtpEndDate_Leave(object sender, EventArgs e)
		{
			if (GetEndDate() < GetStartDate())
			{
				DisplayWarning("End Date cannot be less than the start date.");
				dtpEndDate.Value = GetStartDate().AddDays(1);
			}
			
		}

		/// <summary>
		/// Auther : Shrikant Mali
		/// Date : 10/02/2014
		/// Displays warning message box.
		/// </summary>
		/// <param name="message">The Message to be show.</param>
		private void DisplayWarning(string message)
		{
			Resources.Message.Display(this, message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

        #region PRIMEPOS-2652 06-Dec-2019 JY Added
        private string Search()
        {
            string strSQL = string.Empty;
            try
            {
                if (chkShowAllOrderedItems.Checked)
                {
                    strSQL = "SELECT POD.PODetailId, POD.ItemID AS [Item Id], Itm.Description, POD.VendorItemCode AS [Vendor ItemCode], PO.OrderID AS [Order Id], PO.VendorID AS [Vendor Id], vend.VendorName AS [Vendor Name], OrderDate AS [Order Date]," +
                        " CASE PO.Status WHEN 0 THEN 'Incomplete' WHEN 1 THEN 'Pending' WHEN 2 THEN 'Queued' WHEN 3 THEN 'Submitted' WHEN 4 THEN 'Canceled' WHEN 5 THEN 'Acknowledge'" +
                        " WHEN 6 THEN 'AcknowledgeManually' WHEN 7 THEN 'MaxAttempt' WHEN 8 THEN 'Processed' WHEN 9 THEN 'Expired' WHEN 10 THEN 'PartiallyAcknowledge'" +
                        " WHEN 11 THEN 'PartiallyAck-Reorder' WHEN 12 THEN 'Error' WHEN 13 THEN 'Overdue' WHEN 14 THEN 'SubmittedManually' WHEN 15 THEN 'DirectAcknowledge' WHEN 17 THEN 'DeliveryReceived' END AS[PO Status]" +
                        " FROM PurchaseOrder as PO INNER JOIN PurchaseOrderDetail as POD ON PO.OrderID = POD.OrderID" +
                        " INNER JOIN Item as Itm ON Itm.ItemID = POD.ItemID" +
                        " INNER JOIN Vendor as vend ON PO.VendorID = vend.VendorID" +
                        " WHERE OrderDate >= convert(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime), 113) AND OrderDate <= convert(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime), 113)";
                }
                else
                {
                    strSQL = "SELECT RANK() OVER(PARTITION BY POD.ItemID ORDER BY POD.PODetailId DESC) AS rnk, POD.PODetailId, POD.ItemID AS [Item Id], Itm.Description, POD.VendorItemCode AS [Vendor ItemCode], PO.OrderID AS [Order Id], PO.VendorID AS [Vendor Id], vend.VendorName AS [Vendor Name], OrderDate AS [Order Date]," +
                        " CASE PO.Status WHEN 0 THEN 'Incomplete' WHEN 1 THEN 'Pending' WHEN 2 THEN 'Queued' WHEN 3 THEN 'Submitted' WHEN 4 THEN 'Canceled' WHEN 5 THEN 'Acknowledge'" +
                        " WHEN 6 THEN 'AcknowledgeManually' WHEN 7 THEN 'MaxAttempt' WHEN 8 THEN 'Processed' WHEN 9 THEN 'Expired' WHEN 10 THEN 'PartiallyAcknowledge'" +
                        " WHEN 11 THEN 'PartiallyAck-Reorder' WHEN 12 THEN 'Error' WHEN 13 THEN 'Overdue' WHEN 14 THEN 'SubmittedManually' WHEN 15 THEN 'DirectAcknowledge' WHEN 17 THEN 'DeliveryReceived' END AS[PO Status]" +
                        " FROM PurchaseOrder as PO INNER JOIN PurchaseOrderDetail as POD ON PO.OrderID = POD.OrderID" +
                        " INNER JOIN Item as Itm ON Itm.ItemID = POD.ItemID" +
                        " INNER JOIN Vendor as vend ON PO.VendorID = vend.VendorID" +
                        " WHERE OrderDate >= convert(datetime, cast('" + this.dtpStartDate.Text + " 00:00:00' as datetime), 113) AND OrderDate <= convert(datetime, cast('" + this.dtpEndDate.Text + " 23:59:59' as datetime), 113)";
                }

                if(txtItemCode.Text.Trim() != "")
                {
                    strSQL += "AND Itm.ItemID LIKE '%" + txtItemCode.Text.Trim().Replace("'", "''") + "%'";
                }

                if (txtItemName.Text.Trim() != "")
                {
                    strSQL += "AND Itm.Description LIKE '%" + txtItemName.Text.Trim().Replace("'", "''") + "%'";
                }

                if (!chkShowAllOrderedItems.Checked)
                {
                    strSQL = "SELECT PODetailId, [Item Id], [Description], [Vendor ItemCode],  [Order Id], [Vendor Id], [Vendor Name], [Order Date], [PO Status] FROM (" + strSQL + ") x WHERE rnk = 1 ORDER BY PODetailId";
                }
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "Search()");
                return "";
            }
            return strSQL;
        }
        #endregion
    }
}