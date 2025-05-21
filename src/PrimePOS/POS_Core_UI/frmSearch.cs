using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
//using POS_Core.DataAccess;
using POS_Core.DataAccess;
using MMSBClass;
using MMSChargeAccount;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmVendorSearch.
	/// </summary>
	public class frmSearch : System.Windows.Forms.Form
	{
		private string SearchTable;
        
		public bool IsCanceled  = true;
		public string DefaultCode="";
        public int AdditionalParameter=-1;
		public int ActiveOnly=0;
		private Search oBLSearch = new Search();

		private DataSet oDataSet = new DataSet();
        private DataSet oDataSetDefaultVendor = new DataSet();

        private string ParamValue = string.Empty;
        private bool fromPurchaseOrder = false;

        private bool fromBestVendorPrice = false;
        private bool allowMultiRowSelect = false;
        private string selectedRowsData = string.Empty;

        //Added By SRT(Gaurav) Date: 30-Oct-2008
        private string  PrgFlag ,DataQuery;
        //End Of Added Vy SRT(Gaurav)

        ////Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 06-04-2011
        public bool searchInConstructor = false;
        private bool AllowedDelete = false;//Added On 3 Oct 2011
        //End Of Added By Shitaljit(QuicSolv)

        //Added By SRT(Gaurav) Date: 30-Oct-2008
        //private string vendorTextFromAutoPO; 
        //End Of Added Vy SRT(Gaurav)

        //Added By shitaljit 0n 17 June 0213 for PRIMEPOS-1189 Advance Search Result list is not retained when you come back to it after editing an item
        private bool IsAdvSearchDone = false;
        //End
        //private string vendorId = string.Empty;
        //private string vendorItemId = string.Empty;
        private string sqlQueryForView = "SELECT dbo.Item.ItemID, dbo.Vendor.VendorName, dbo.ItemVendor.VendorID, dbo.ItemVendor.VendorItemID, dbo.Item.Description, dbo.Item.Unit," +
                                  "dbo.Vendor.VendorID AS Expr1, dbo.Item.Location, dbo.Item.QtyInStock AS Qty, dbo.Item.SellingPrice AS Cost, dbo.Item.Discount," +
                                  "dbo.Item.ReOrderLevel, dbo.Item.ExptDeliveryDate, dbo.Item.SaleEndDate, dbo.Item.SaleStartDate, dbo.Item.ProductCode, dbo.Vendor.VendorCode," +
                                  "dbo.ItemVendor.LastOrderDate, dbo.Item.LastVendor,dbo.Item.PreferredVendor, dbo.ItemVendor.IsDeleted As Discontinued FROM  dbo.Item INNER JOIN " +
                                  "dbo.ItemVendor ON dbo.Item.ItemID = dbo.ItemVendor.ItemID AND dbo.Item.LastVendor = dbo.ItemVendor.VendorID INNER JOIN" +
                                  " dbo.Vendor ON dbo.ItemVendor.VendorID = dbo.Vendor.VendorID";

        //private string queryToFetchItem = "Select Item.ItemID,Item.Description,Item.QtyInStock,Item.PreferredVendor,Item.SellingPrice,ItemVendor.VendorItemId,Item.ReOrderLevel,Item.LastVendor,Vendor.VendorID" +
        //                       " from Item ,ItemVendor,Vendor where Item.ItemID =ItemVendor.ItemID AND ItemVendor.VendorID = Vendor.VendorID";

        //private string queryToFetchItem = "Select Item.ItemID,Item.Description,Item.QtyInStock,Item.PreferredVendor,Item.SellingPrice , " +
        //                                  " ItemVendor.VendorItemId,Item.ReOrderLevel,Vendor.VendorID,Vendor.VendorName as LastVendorName,ItemVendor.VendorCostPrice from  Item ,ItemVendor,Vendor where Item.ItemID =ItemVendor.ItemID AND " +
        //                                  " ItemVendor.VendorID = Vendor.VendorID and ItemVendor.VendorID in (Select DISTINCT Item.LastVendor from Vendor,Item where Vendor.VendorID=Item.LastVendor)";
        //Modified By Amit Date 14 May 2011
        //private string queryToFetchItem = "Select Item.ItemID,Item.Description,Vendor.VendorName as VendorName,Item.QtyInStock,Item.PreferredVendor,Item.SellingPrice , " +
        //                                  " ItemVendor.VendorItemId,ItemVendor.VendorCostPrice,Item.ReOrderLevel,Vendor.VendorID from  Item ,ItemVendor,Vendor where Item.ItemID =ItemVendor.ItemID AND " +
        //                                  " ItemVendor.VendorID = Vendor.VendorID";
        private string queryToFetchItem = "Select Item.ItemID,Item.Description,Vendor.VendorName as VendorName,Item.QtyInStock,Item.QtyOnOrder,Item.MinOrdQty,Item.PreferredVendor,Item.SellingPrice , " +
                                            " ItemVendor.VendorItemId,ItemVendor.VendorCostPrice,Item.ReOrderLevel,Vendor.VendorID, ISNULL(ItemVendor.PCKQTY,'') AS PCKQTY, ISNULL(ItemVendor.PCKSIZE,'') AS PCKSIZE, ISNULL(ItemVendor.PCKUNIT,'') AS PCKUNIT " +
                                            " from  Item, ItemVendor, Vendor where Item.ItemID =ItemVendor.ItemID AND " +
                                            " ItemVendor.VendorID = Vendor.VendorID";   //Sprint-25 - PRIMEPOS-1041 03-Apr-2017 JY Added ItemVendor.PCKQTY, ItemVendor.PCKSIZE, ItemVendor.PCKUNIT 

        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.Misc.UltraButton btnSearch;
		private Infragistics.Win.Misc.UltraButton btnOk;
		private Infragistics.Win.Misc.UltraButton btnCancel;
		private Infragistics.Win.Misc.UltraLabel lbl1;
		private Infragistics.Win.Misc.UltraLabel lbl2;
        internal Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private RadioButton radioVendItemId;
        private RadioButton radioItemId;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTxtEditorNoOfItems;
        private Infragistics.Win.Misc.UltraLabel ultraLblNoOfItems;
        private IContainer components;

        #region 
        string strItemID = string.Empty;
        string strVendorItemID = string.Empty;
        string strDescription = string.Empty;
        string strBestVendorName = string.Empty;
        string strVendorCostPrice = string.Empty;
        private Infragistics.Win.Misc.UltraButton ultraBtnBestPrice;
        private CheckBox chkSelectAll;
        public Infragistics.Win.Misc.UltraButton btnAdd;
        public Infragistics.Win.Misc.UltraButton btnEdit;
        public Infragistics.Win.Misc.UltraButton btnDelete;
       //Commented by krishna-----------------------
        // bool searchInConstructor = false;
        public string SelectedRowCode;
        private Infragistics.Win.Misc.UltraButton btnAdvSearch;
        private TextBox txtName;
        public TextBox txtCode;
        private TextBox txtMasterSearchVal;
        private Infragistics.Win.Misc.UltraLabel lbl3;
        private Infragistics.Win.Misc.UltraButton btnCopy;

        //------------------------------------
        //Added by Amit Date 2 Nov 2011
        public string strSetSelected = string.Empty;
        //End
        public string sCalledFrom = string.Empty;   //Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
        #endregion 

        internal bool AllowMultiRowSelect
        {
            get { return allowMultiRowSelect; }
            set { allowMultiRowSelect = value; }
        }

        internal string SelectedRowsData
        {
            get { return selectedRowsData; }
            set { selectedRowsData = value; }
        }

        /// <summary>
        /// Author : Gaurav
        /// Date : 11-Jul-2009
        /// Returns The Data count of searched data.
        /// </summary>
        public Int32 SearchDataRowsCount
        {
            get
            {
                return (oDataSet != null &&oDataSet.Tables.Count>0 ? oDataSet.Tables[0].Rows.Count : 0);
            }
        }

        //Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 06-04-2011
        //Will search the data at constructor level if property 'SearchInConstructor' set to true.
        public bool SearchInConstructor
        {
            get
            {
                return (searchInConstructor);
            }
            set
            {
                searchInConstructor = value;
                
                if (searchInConstructor)
                {
                    Search();
                }
                //End OfAdded By Shitaljit(QuicSolv)
            }
        }

        public frmSearch(string Table)
        {
            InitializeComponent();
            //vendorTextFromAutoPO = string.Empty;
            try
            {
                grdSearch.DataSource = oDataSet;
                grdSearch.Refresh();
                SearchTable = Table;
                resizeColumns();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public frmSearch(string Table, string searchValue)
        {
            InitializeComponent();
            //vendorTextFromAutoPO = string.Empty;
            try
            {
                grdSearch.DataSource = oDataSet;
                grdSearch.Refresh();
                ParamValue = searchValue;
                PrgFlag = Table;
                GetDataForPreferedVendor();
                //Added By SRT(Ritesh Parekh) Date : 22-Jul-2009
                //assigned search type as search table to identify the caller at time of getting the vendoritemcode on post selection.
                SearchTable = Table;
                if (searchValue != "")
                    Search();
                resizeColumns();
                //End Of Added By SRT(Ritesh Parekh)
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public frmSearch(string Table, string sCode, string sDescription)
        {
            InitializeComponent();
            //vendorTextFromAutoPO = string.Empty;
            try
            {
                //this.txtCode.Text = sCode; ORIGINAL commected By Shitaljit(QuicSolv)
                //Modified By Shitaljit(QuicSolv) on 3 June 2011
                if (SearchSvr.SubDeptIDFlag)
                {
                    this.txtCode.Text = "";
                    DefaultCode = sCode;
                }
                else
                {
                    this.txtCode.Text = sCode;
                }
                //Till here Added By Shitaljit
                this.txtName.Text = sDescription;
                grdSearch.DataSource = oDataSet;
                grdSearch.Refresh();
                SearchTable = Table;
                resizeColumns();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public frmSearch(string Table, string tmpPrgFlag, string tmpParamName, string tmpParamValue)
        {
            InitializeComponent();
            //vendorTextFromAutoPO = string.Empty;
            try
            {
                grdSearch.DataSource = oDataSet;
                grdSearch.Refresh();
                // ParamName = tmpParamName;
                ParamValue = tmpParamValue;
                SearchTable = Table;
                PrgFlag = tmpPrgFlag;
                //DataQuery = "select * from " + Table + " where " + tmpParamName + "='" + tmpParamValue + "'"; //commented as not in use
                resizeColumns();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
                     
        private void GetDataForPreferedVendor()
        {
            string queryForDefaultVendor = string.Empty;
            string defaultVendor = Configuration.CPrimeEDISetting.DefaultVendor;    //PRIMEPOS-3167 07-Nov-2022 JY Modified
            DataSet vendor = new DataSet();

            try
            {
                string query = "Select VendorID from Vendor where VendorCode='" + defaultVendor + "'";
                vendor = oBLSearch.SearchData(query);

                if (vendor.Tables[0].Rows.Count == 1)
                {
                    queryForDefaultVendor = "Select itm.ItemID,itm.Description,itm.QtyInStock,itm.PreferredVendor,itm.ReOrderLevel,itm.LastVendor ," +
                           "itmVen.VendorCostPrice from Item as itm,ItemVendor as itmVen ,Vendor as vend where itm.ItemID =itmVen.ItemID AND " +
                           "itmVen.VendorID = vend.VendorID AND itmVen.VendorID =" + Convert.ToInt32(vendor.Tables[0].Rows[0]["VendorId"].ToString());
                    oDataSetDefaultVendor = oBLSearch.SearchData(queryForDefaultVendor);
                }


            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }   
        }  


        //End Of Added By SRT(Gaurav)

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearch));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Column Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Type");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Criteria Value");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.txtMasterSearchVal = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lbl3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.btnAdvSearch = new Infragistics.Win.Misc.UltraButton();
            this.radioVendItemId = new System.Windows.Forms.RadioButton();
            this.radioItemId = new System.Windows.Forms.RadioButton();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraTxtEditorNoOfItems = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLblNoOfItems = new Infragistics.Win.Misc.UltraLabel();
            this.ultraBtnBestPrice = new Infragistics.Win.Misc.UltraButton();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnCopy = new Infragistics.Win.Misc.UltraButton();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtEditorNoOfItems)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.txtMasterSearchVal);
            this.ultraTabPageControl1.Controls.Add(this.txtCode);
            this.ultraTabPageControl1.Controls.Add(this.lbl3);
            this.ultraTabPageControl1.Controls.Add(this.txtName);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Controls.Add(this.btnAdvSearch);
            this.ultraTabPageControl1.Controls.Add(this.radioVendItemId);
            this.ultraTabPageControl1.Controls.Add(this.radioItemId);
            this.ultraTabPageControl1.Controls.Add(this.lbl2);
            this.ultraTabPageControl1.Controls.Add(this.lbl1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 22);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(877, 63);
            // 
            // txtMasterSearchVal
            // 
            this.txtMasterSearchVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMasterSearchVal.Location = new System.Drawing.Point(92, 3);
            this.txtMasterSearchVal.Name = "txtMasterSearchVal";
            this.txtMasterSearchVal.Size = new System.Drawing.Size(474, 22);
            this.txtMasterSearchVal.TabIndex = 93;
            this.txtMasterSearchVal.Visible = false;
            this.txtMasterSearchVal.TextChanged += new System.EventHandler(this.txtMasterSearchVal_TextChanged);
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.Location = new System.Drawing.Point(92, 13);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(202, 22);
            this.txtCode.TabIndex = 1;
            this.txtCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxKeyup);
            // 
            // lbl3
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.lbl3.Appearance = appearance1;
            this.lbl3.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl3.Location = new System.Drawing.Point(3, 3);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(75, 19);
            this.lbl3.TabIndex = 94;
            this.lbl3.Text = "&Look For";
            this.lbl3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lbl3.Visible = false;
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Location = new System.Drawing.Point(364, 13);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(202, 22);
            this.txtName.TabIndex = 2;
            this.txtName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBoxKeyup);
            // 
            // btnSearch
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnSearch.Appearance = appearance2;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(640, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(124, 28);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "&Search (F4)";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnAdvSearch
            // 
            this.btnAdvSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.btnAdvSearch.Appearance = appearance3;
            this.btnAdvSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnAdvSearch.HotTrackAppearance = appearance4;
            this.btnAdvSearch.Location = new System.Drawing.Point(706, 26);
            this.btnAdvSearch.Name = "btnAdvSearch";
            this.btnAdvSearch.Size = new System.Drawing.Size(154, 28);
            this.btnAdvSearch.TabIndex = 4;
            this.btnAdvSearch.Text = "&Adv. Search (F11)";
            this.btnAdvSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnAdvSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAdvSearch.Visible = false;
            this.btnAdvSearch.Click += new System.EventHandler(this.btnAdvSearch_Click);
            // 
            // radioVendItemId
            // 
            this.radioVendItemId.AutoSize = true;
            this.radioVendItemId.BackColor = System.Drawing.Color.Silver;
            this.radioVendItemId.Location = new System.Drawing.Point(187, 41);
            this.radioVendItemId.Name = "radioVendItemId";
            this.radioVendItemId.Size = new System.Drawing.Size(111, 18);
            this.radioVendItemId.TabIndex = 15;
            this.radioVendItemId.Text = "VendorItemId";
            this.radioVendItemId.UseVisualStyleBackColor = false;
            this.radioVendItemId.Visible = false;
            // 
            // radioItemId
            // 
            this.radioItemId.AutoSize = true;
            this.radioItemId.BackColor = System.Drawing.Color.Silver;
            this.radioItemId.Checked = true;
            this.radioItemId.Location = new System.Drawing.Point(96, 41);
            this.radioItemId.Name = "radioItemId";
            this.radioItemId.Size = new System.Drawing.Size(67, 18);
            this.radioItemId.TabIndex = 14;
            this.radioItemId.TabStop = true;
            this.radioItemId.Text = "ItemId";
            this.radioItemId.UseVisualStyleBackColor = false;
            this.radioItemId.Visible = false;
            // 
            // lbl2
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            this.lbl2.Appearance = appearance5;
            this.lbl2.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl2.Location = new System.Drawing.Point(308, 16);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(86, 19);
            this.lbl2.TabIndex = 3;
            this.lbl2.Text = "&Name";
            this.lbl2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lbl1
            // 
            appearance6.ForeColor = System.Drawing.Color.White;
            this.lbl1.Appearance = appearance6;
            this.lbl1.BackColorInternal = System.Drawing.Color.Transparent;
            this.lbl1.Location = new System.Drawing.Point(43, 16);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(86, 19);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "&Code";
            // 
            // btnClear
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance7.FontData.BoldAsString = "True";
            appearance7.ForeColor = System.Drawing.Color.White;
            this.btnClear.Appearance = appearance7;
            this.btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClear.Location = new System.Drawing.Point(187, 297);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 28);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click_1);
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3});
            // 
            // grdSearch
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.White;
            appearance8.BackColorDisabled = System.Drawing.Color.White;
            appearance8.BackColorDisabled2 = System.Drawing.Color.White;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance8;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand1.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            ultraGridBand1.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdSearch.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            appearance11.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance11;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BackColorDisabled = System.Drawing.Color.White;
            appearance13.BackColorDisabled2 = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance14.BorderColor = System.Drawing.Color.Gray;
            appearance14.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            appearance14.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance14.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance16;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.White;
            appearance18.BackColorDisabled = System.Drawing.Color.White;
            appearance18.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance19.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.FontData.BoldAsString = "True";
            appearance20.FontData.SizeInPoints = 10F;
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.TextHAlignAsString = "Left";
            appearance20.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance20;
            this.grdSearch.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance21.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance22.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance22;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance24;
            this.grdSearch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance25.BackColor = System.Drawing.Color.Navy;
            appearance25.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.Navy;
            appearance26.BackColorDisabled = System.Drawing.Color.Navy;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance26.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            appearance26.ForeColor = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance26;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance27.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance27;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance28.BackColor2 = System.Drawing.Color.White;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance28.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance28.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance28;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.Location = new System.Drawing.Point(14, 99);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(879, 386);
            this.grdSearch.TabIndex = 4;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            this.grdSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.grdSearch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdSearch_MouseClick);
            // 
            // ultraTabControl1
            // 
            appearance29.FontData.BoldAsString = "True";
            this.ultraTabControl1.ActiveTabAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ultraTabControl1.Appearance = appearance30;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Location = new System.Drawing.Point(12, 8);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(881, 87);
            this.ultraTabControl1.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.ultraTabControl1.TabIndex = 0;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance31.FontData.BoldAsString = "True";
            ultraTab1.Appearance = appearance31;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            ultraTab1.ClientAreaAppearance = appearance32;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Criteria";
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.ultraTabControl1.TabStop = false;
            this.ultraTabControl1.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2003;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(877, 63);
            // 
            // btnOk
            // 
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance33.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance33.FontData.BoldAsString = "True";
            appearance33.ForeColor = System.Drawing.Color.White;
            appearance33.Image = ((object)(resources.GetObject("appearance33.Image")));
            this.btnOk.Appearance = appearance33;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.Location = new System.Drawing.Point(670, 498);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(108, 28);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance34.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance34.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance34.FontData.BoldAsString = "True";
            appearance34.ForeColor = System.Drawing.Color.White;
            appearance34.Image = ((object)(resources.GetObject("appearance34.Image")));
            this.btnCancel.Appearance = appearance34;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(785, 498);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ultraTxtEditorNoOfItems
            // 
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance35.ForeColor = System.Drawing.Color.White;
            this.ultraTxtEditorNoOfItems.Appearance = appearance35;
            this.ultraTxtEditorNoOfItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ultraTxtEditorNoOfItems.Enabled = false;
            this.ultraTxtEditorNoOfItems.Location = new System.Drawing.Point(102, 500);
            this.ultraTxtEditorNoOfItems.Name = "ultraTxtEditorNoOfItems";
            this.ultraTxtEditorNoOfItems.ReadOnly = true;
            this.ultraTxtEditorNoOfItems.Size = new System.Drawing.Size(100, 24);
            this.ultraTxtEditorNoOfItems.TabIndex = 85;
            // 
            // ultraLblNoOfItems
            // 
            appearance36.ForeColor = System.Drawing.Color.White;
            this.ultraLblNoOfItems.Appearance = appearance36;
            this.ultraLblNoOfItems.AutoSize = true;
            this.ultraLblNoOfItems.Location = new System.Drawing.Point(14, 504);
            this.ultraLblNoOfItems.Name = "ultraLblNoOfItems";
            this.ultraLblNoOfItems.Size = new System.Drawing.Size(84, 17);
            this.ultraLblNoOfItems.TabIndex = 84;
            this.ultraLblNoOfItems.Text = "No. Of Items";
            // 
            // ultraBtnBestPrice
            // 
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance37.FontData.BoldAsString = "True";
            appearance37.ForeColor = System.Drawing.Color.White;
            appearance37.Image = ((object)(resources.GetObject("appearance37.Image")));
            this.ultraBtnBestPrice.Appearance = appearance37;
            this.ultraBtnBestPrice.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.ultraBtnBestPrice.Location = new System.Drawing.Point(548, 498);
            this.ultraBtnBestPrice.Name = "ultraBtnBestPrice";
            this.ultraBtnBestPrice.Size = new System.Drawing.Size(113, 28);
            this.ultraBtnBestPrice.TabIndex = 86;
            this.ultraBtnBestPrice.Text = "&Best Price";
            this.ultraBtnBestPrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraBtnBestPrice.Visible = false;
            this.ultraBtnBestPrice.Click += new System.EventHandler(this.ultraBtnBestPrice_Click);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(19, 106);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(15, 14);
            this.chkSelectAll.TabIndex = 85;
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.SystemColors.Control;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.FontData.BoldAsString = "True";
            appearance38.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Appearance = appearance38;
            this.btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance39.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnAdd.HotTrackAppearance = appearance39;
            this.btnAdd.Location = new System.Drawing.Point(275, 498);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(77, 28);
            this.btnAdd.TabIndex = 87;
            this.btnAdd.Text = "&Add (F2)";
            this.btnAdd.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnAdd.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.bttnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance40.BackColor = System.Drawing.Color.White;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance40.FontData.BoldAsString = "True";
            appearance40.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Appearance = appearance40;
            this.btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEdit.HotTrackAppearance = appearance41;
            this.btnEdit.Location = new System.Drawing.Point(357, 498);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(77, 28);
            this.btnEdit.TabIndex = 88;
            this.btnEdit.Text = "&Edit (F3)";
            this.btnEdit.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.SystemColors.Control;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance42.FontData.BoldAsString = "True";
            appearance42.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Appearance = appearance42;
            this.btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnDelete.HotTrackAppearance = appearance43;
            this.btnDelete.Location = new System.Drawing.Point(440, 498);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(102, 28);
            this.btnDelete.TabIndex = 89;
            this.btnDelete.Text = "&Delete(F10)";
            this.btnDelete.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCopy
            // 
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance44.FontData.BoldAsString = "True";
            appearance44.ForeColor = System.Drawing.Color.White;
            appearance44.Image = ((object)(resources.GetObject("appearance44.Image")));
            this.btnCopy.Appearance = appearance44;
            this.btnCopy.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCopy.Location = new System.Drawing.Point(669, 498);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(108, 28);
            this.btnCopy.TabIndex = 90;
            this.btnCopy.TabStop = false;
            this.btnCopy.Text = "C&opy";
            this.btnCopy.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCopy.Visible = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // frmSearch
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(905, 538);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.ultraBtnBestPrice);
            this.Controls.Add(this.ultraTxtEditorNoOfItems);
            this.Controls.Add(this.ultraLblNoOfItems);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCopy);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this.frmSearch_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyUp);
            this.Leave += new System.EventHandler(this.frmSearch_Leave);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTxtEditorNoOfItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}


		#endregion

        //public string VendorId
        //{
        //    set { this.vendorId = value; }
        //    get { return this.vendorId; }
        //}

        //public string VendorItemId
        //{
        //    set { this.vendorItemId = value; }
        //    get { return this.vendorItemId; }
        //}

        public bool IsFromPurchaseOrder
        {
            set { fromPurchaseOrder = value; }
            get { return fromPurchaseOrder; }
        }

        //public string VendorTextFromAutoPO
        //{
        //    set { vendorTextFromAutoPO = value; }
        //    get { return vendorTextFromAutoPO; }
        //}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			Search();
		}
        private string GetHouseChargeSearchQuery()
        {
            string sSQL = string.Empty;
            sSQL = @"SELECT ACCT_NO,ACCT_NAME,ADDRESS1,ADDRESS2,CITY ,STATE,ZIP,PHONE_NO,STATUS,APPFINCHRG,RECURRINGB,CCTYPE,CCNUMBER,CCEXPDATE,CREDIT_LMT
                ,BALANCE,DISCOUNT,MTD_CHARGE,YTD_CHARGE,LY_CHARGE,MTD_PAYM,YTD_PAYM,LY_PAYM,LAST_SBAL, (CONVERT(VARCHAR(10),LAST_SDATE , 126)) as 'LAST_SDATE',COMMENT,ACCEPTCHK,MOBILENO,NAMEONCC,IE FROM ACCOUNT WHERE 1=1 ";
            string sWhrClause = string.Empty;
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {
                this.txtMasterSearchVal.Text = this.txtMasterSearchVal.Text.Trim();
                sWhrClause = @" AND ACCT_NO  " + " LIKE (" + "'" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "ACCT_NAME" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "ADDRESS1" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "ADDRESS2" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "CITY" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "STATE" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "ZIP" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "PHONE_NO" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "STATUS" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "APPFINCHRG" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "RECURRINGB" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "CCTYPE" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "CCNUMBER" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "CCEXPDATE" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "CREDIT_LMT" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "BALANCE" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "DISCOUNT" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "MTD_CHARGE" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "YTD_CHARGE" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "LY_CHARGE" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "MTD_PAYM" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "YTD_PAYM" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "LY_PAYM" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "LAST_SBAL" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + " CONVERT(VARCHAR(25), LAST_SDATE , 126)" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "COMMENT" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "ACCEPTCHK" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "MOBILENO" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "NAMEONCC" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                          + "IE " + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')   ";
            }
            else
            {
                sWhrClause = @" AND ACCT_NO  " + " LIKE (" + "'" + this.txtCode.Text + "%')OR   "
                            + "ACCT_NAME" + " LIKE (" + "'" + this.txtName.Text + "%')  ";
            }
            return sSQL = sSQL + sWhrClause;
        }

		private void Search()
		{
            this.grdSearch.DisplayLayout.Bands[0].SortedColumns.Clear();    //19-Jun-2015 JY Added 

            this.Cursor = Cursors.WaitCursor;
            //Added By shitaljit on 12 Aug 2013 to allow special character "'" in specially for customer Name while searching for P
            //RIMEPOS-1308 customer name is a special character cannot be searched in POS 
            if (string.IsNullOrEmpty(this.txtName.Text) == false)
            {
                this.txtName.Text = this.txtName.Text.Replace("'", "''");
            }
            if (string.IsNullOrEmpty(this.txtCode.Text) == false)
            {
                this.txtCode.Text = this.txtCode.Text.Replace("'", "''");
            }
            //End 
			if (DefaultCode.Trim()=="")
				DefaultCode=this.txtCode.Text.Trim();
            if(PrgFlag == clsPOSDBConstants.ItemId)            
            {              
                string StrQuery = string.Empty;                
                if (this.txtName.Text.Trim() != string.Empty || this.txtCode.Text.Trim() != string.Empty)
                    StrQuery = queryToFetchItem + "  AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + this.txtName.Text.Trim() + "%' AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + this.txtCode.Text.Trim() + "%'";
                else
                    StrQuery = queryToFetchItem + "  AND " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + ParamValue + "%'";

                FetchItem(StrQuery);
            }
            else if (PrgFlag == clsPOSDBConstants.VendorItemCodeWise)
            {
                string StrQuery = string.Empty;
                
                if (this.txtName.Text.Trim() != string.Empty || this.txtCode.Text.Trim() != string.Empty)
                    StrQuery = queryToFetchItem + "  AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + this.txtName.Text.Trim() + "%' AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + this.txtCode.Text.Trim() + "%'";
                else
                    StrQuery = queryToFetchItem + "  AND " + clsPOSDBConstants.ItemVendor_tbl + "." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID + " like '" + ParamValue + "%'";
                
                FetchItem(StrQuery);
            }
            else if (PrgFlag == clsPOSDBConstants.DescriptionWise)
            {
                string StrQuery = string.Empty;
                if (this.txtName.Text.Trim() != string.Empty || this.txtCode.Text.Trim() != string.Empty)
                    StrQuery = queryToFetchItem + "  AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + this.txtName.Text.Trim() + "%' AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + this.txtCode.Text.Trim() + "%'";
                else 
                    StrQuery = queryToFetchItem + "  AND " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + ParamValue + "%'";
                
                FetchItem(StrQuery);
            }
            else if (IsFromPurchaseOrder == true && PrgFlag == "")
            {
                string StrQuery = string.Empty;

                if (this.txtName.Text.Trim() != string.Empty || this.txtCode.Text.Trim() != string.Empty)
                    StrQuery = queryToFetchItem + "  AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + this.txtName.Text.Trim() + "%' AND  " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " like '" + this.txtCode.Text.Trim() + "%'";
                else
                    StrQuery = queryToFetchItem;  //+ "  AND " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + " like '" + ParamValue + "%'";

                FetchItem(StrQuery);
            }
            else if (IsFromPurchaseOrder == false)
            {
                //if (vendorTextFromAutoPO != string.Empty)
                //{
                //    this.txtName.Text = vendorTextFromAutoPO;
                //}
                #region ROA record search Filter.
                ContAccount oSearch = new ContAccount();
                if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface && (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false ||(string.IsNullOrEmpty(this.txtName.Text) == false && string.IsNullOrEmpty(this.txtCode.Text) == false)))
                {
                    if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
                    {
                        string sSQL = GetHouseChargeSearchQuery();
                        oSearch.GetRecs(sSQL, out oDataSet);
                    }
                }
                #endregion
                else
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, DefaultCode, this.txtName.Text.Trim(), ActiveOnly, AdditionalParameter);
                    if (oDataSet != null && oDataSet.Tables[0].Rows.Count == 0 && clsUIHelper.isNumeric(this.txtCode.Text.Trim()))  //PRIMEPOS-3106 13-Jul-2022 JY Modified
                    {
                        string sqlQuery = "SELECT ITEMID AS [Item Code], DESCRIPTION as [Item Description], SELLINGPRICE as [Unit Price], " +
                                          "QTYINSTOCK as [Qty In Stock], Unit, EXPTDELIVERYDATE as [Delivery Date], REORDERLEVEL as [Reorder Level], " +
                                          "SALEENDDATE as [Sale End Date], SALESTARTDATE as [Sale Start Date], Discount, PRODUCTCODE as [SKU Code], " +
                                          "Location, Remarks FROM ITEM WHERE PRODUCTCODE = '" + this.txtCode.Text.Trim() + "'";
                        oDataSet = oBLSearch.SearchData(sqlQuery);
                    }
                    DefaultCode = string.Empty;
                }
            }
            
            grdSearch.DataSource = oDataSet;
            if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface)
            {
                try
                {
                    grdSearch.DisplayLayout.Bands[0].Columns["ACCT_NO"].Header.Caption = "Account#";
                    grdSearch.DisplayLayout.Bands[0].Columns["ACCT_NAME"].Header.Caption = "Name";
                    grdSearch.DisplayLayout.Bands[0].Columns["ADDRESS1"].Header.Caption = "Address1";
                    grdSearch.DisplayLayout.Bands[0].Columns["ADDRESS2"].Header.Caption = "Address2";
                    grdSearch.DisplayLayout.Bands[0].Columns["CITY"].Header.Caption = "City";
                    grdSearch.DisplayLayout.Bands[0].Columns["STATE"].Header.Caption = "State";
                    grdSearch.DisplayLayout.Bands[0].Columns["ZIP"].Header.Caption  = "Zip";
                    grdSearch.DisplayLayout.Bands[0].Columns["PHONE_NO"].Header.Caption = "Phone";
                    grdSearch.DisplayLayout.Bands[0].Columns["STATUS"].Header.Caption = "Status";
                    grdSearch.DisplayLayout.Bands[0].Columns["APPFINCHRG"].Header.Caption = "App Fin Charge";
                    grdSearch.DisplayLayout.Bands[0].Columns["RECURRINGB"].Header.Caption = "Reccuring GB";
                    grdSearch.DisplayLayout.Bands[0].Columns["CCTYPE"].Header.Caption = "CC Type";
                    grdSearch.DisplayLayout.Bands[0].Columns["CCNUMBER"].Header.Caption = "CC#";
                    grdSearch.DisplayLayout.Bands[0].Columns["CCEXPDATE"].Header.Caption = "CC Exp. Date";
                    grdSearch.DisplayLayout.Bands[0].Columns["CREDIT_LMT"].Header.Caption = "Credit Limit";
                    grdSearch.DisplayLayout.Bands[0].Columns["BALANCE"].Header.Caption = "Balance";
                    grdSearch.DisplayLayout.Bands[0].Columns["DISCOUNT"].Header.Caption = "Discount";
                    grdSearch.DisplayLayout.Bands[0].Columns["MTD_CHARGE"].Header.Caption = "MDT Charge";
                    grdSearch.DisplayLayout.Bands[0].Columns["YTD_CHARGE"].Header.Caption = "YDT Charge";
                    grdSearch.DisplayLayout.Bands[0].Columns["LY_CHARGE"].Header.Caption = "LY Charge";
                    grdSearch.DisplayLayout.Bands[0].Columns["MTD_PAYM"].Header.Caption = "MDT Payment";
                    grdSearch.DisplayLayout.Bands[0].Columns["YTD_PAYM"].Header.Caption = "YDT Payment";
                    grdSearch.DisplayLayout.Bands[0].Columns["LY_PAYM"].Header.Caption = "LY Payment";
                    grdSearch.DisplayLayout.Bands[0].Columns["LAST_SBAL"].Header.Caption = "Last Bal.";
                    grdSearch.DisplayLayout.Bands[0].Columns["LAST_SDATE"].Header.Caption = "Last Date";
                    grdSearch.DisplayLayout.Bands[0].Columns["COMMENT"].Header.Caption = "Comment";
                    grdSearch.DisplayLayout.Bands[0].Columns["MOBILENO"].Header.Caption = "Mobile";
                    grdSearch.DisplayLayout.Bands[0].Columns["NAMEONCC"].Header.Caption = "Name On CC";
                    grdSearch.DisplayLayout.Bands[0].Columns["IE"].Header.Caption = "IE";
                }
                catch (Exception)
                {
                    
                }
            }
            if (oDataSet != null)
                ultraTxtEditorNoOfItems.Text = oDataSet.Tables[0].Rows.Count.ToString();

            if (this.AllowMultiRowSelect == true)
            {

                if (this.grdSearch.DisplayLayout.Bands[0].Columns.Exists("CHECK") == false)
                {
                    this.grdSearch.DisplayLayout.Bands[0].Columns.Add("CHECK");
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Header.Caption = "";

                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].DataType = typeof(bool);
                    this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                }

                this.grdSearch.DisplayLayout.Bands[0].Columns["Check"].Header.VisiblePosition = 0;
                this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Width = 50;
            }

            if (IsFromPurchaseOrder == true)
            {
                grdSearch.DisplayLayout.Bands[0].Columns["SellingPrice"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns["VendorID"].Hidden = true;
                //Start : Added By Amit Date 17 June 2011
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyInStock].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_QtyOnOrder].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ReOrderLevel].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_MinOrdQty].Hidden = true;
                //End
                foreach (UltraGridRow row in grdSearch.Rows)
                {
                    if (row.Cells[clsPOSDBConstants.ItemVendor_Fld_VendorCostPrice].Value.ToString() != row.Cells["BestPrice"].Value.ToString())
                    {
                        grdSearch.DisplayLayout.Bands[0].Columns["BestPrice"].CellAppearance.ForeColor = Color.Red;
                        grdSearch.DisplayLayout.Bands[0].Columns["BestVendor"].CellAppearance.ForeColor = Color.Red;
                    }
                }
            }
            if (grdSearch.DisplayLayout.Bands[0].Columns.Contains("id"))
            {
                grdSearch.DisplayLayout.Bands[0].Columns["id"].Hidden = true;
            }
            if (SearchTable == clsPOSDBConstants.Department_tbl)
            {
                if (grdSearch.DisplayLayout.Bands[0].Columns.Contains("id"))    //Sprint-24 - PRIMEPOS-2273 27-Oct-2016 JY Added to fix one exception which occur when we provide wrong dept code in search criteria
                    this.grdSearch.DisplayLayout.Bands[0].Columns["id"].Hidden = true;
                if (grdSearch.DisplayLayout.Bands[0].Columns.Contains("Name"))
                    this.grdSearch.DisplayLayout.Bands[0].Columns["Name"].SortIndicator = SortIndicator.Ascending;  //Sprint-19 - 2146 24-Dec-2014 JY Added to sort grid departmentwise by default 
            }
            #region Sprint-19 - 2146 24-Dec-2014 JY Added to sort grid sub-departmentwise when it populated first time
            if (SearchTable == clsPOSDBConstants.SubDepartment_tbl)
            {
                this.grdSearch.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.Ascending;
            }
            #endregion
            //Added By shitaljit on 12 Aug 2013 to allow special character "'" in specially for customer Name while searching for P
            //RIMEPOS-1308 customer name is a special character cannot be searched in POS 
            if (string.IsNullOrEmpty(this.txtName.Text) == false)
            {
                this.txtName.Text = this.txtName.Text.Replace("''", "'");
            }
            if (string.IsNullOrEmpty(this.txtCode.Text) == false)
            {
                this.txtCode.Text = this.txtCode.Text.Replace("''", "'");
            }
            //End 
            //this.grdSearch.DisplayLayout.Bands[0].Columns["CHECK"].Width = 50;
            grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
			grdSearch.Refresh();
            resizeColumns();
            this.Cursor = Cursors.Default;
		}
        private void FetchItem(string queryTofetchItems)
        {
            try
            {
                oDataSet = oBLSearch.SearchData(queryTofetchItems);
                SetLastVendor();
                //SetDefaultVendorValues();
                AddBestVendPriceCoulomn();
                SetBestPriceBestVendorValues();
            }
            catch(Exception ex)
            {
              POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }        
        }

        private void SetLastVendor()
        {
            DataSet lastVendorName = new DataSet();

            try
            {
                oDataSet.Tables[0].Columns.Add("LastVendorName");
                //Updated By SRT(Gaurav) Date: 15-Jul-2009
                //Updated query to avoid error in converting data of wrong type / (here is Int -> String)
                string queryforLastVendorName = "Select Item.ItemID,Item.LastVendor ,Vendor.VendorName as LastVendorName from  Item ,Vendor where CAST(Item.LastVendor as VARCHAR(50))  = CAST(Vendor.VendorCode as VARCHAR(50))";
                lastVendorName = oBLSearch.SearchData(queryforLastVendorName);
                
                foreach(DataRow row in oDataSet.Tables[0].Rows)
                {
                    int count = 0;
                    DataRow[] rowLastVendoName = lastVendorName.Tables[0].Select(" ItemID ='" + row["ItemID"].ToString()+"'");
                    for(count = 0; count < rowLastVendoName.Length; count++)
                    {
                      row["LastVendorName"] = rowLastVendoName[count].ItemArray[2].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
              POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
        }

        private void AddBestVendPriceCoulomn()
        {
            try
            {
                oDataSet.Tables[0].Columns.Add("BestVendor");
                oDataSet.Tables[0].Columns.Add("BestPrice");
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
        }

        private void SetBestPriceBestVendorValues()
        {
            
            DataSet dsBestVendPrice = null;
          
            try
            {
                foreach(DataRow row in oDataSet.Tables[0].Rows)
                {
                    dsBestVendPrice = new DataSet();
                    string search = "Select VendorCostPrice,Vendor.VendorName from ItemVendor, Vendor where Vendor.VendorID=ItemVendor.VendorID AND ItemVendor.VendorCostPrice= (Select MIN(ItemVendor.VendorCostPrice) from ItemVendor where ItemID = '" + row ["ItemID"].ToString()+ "')" +
                                     " AND ItemID = '" + row["ItemID"].ToString() + "'";  
                    //dsBestVendPrice = oBLSearch.SearchData("Select VendorCostPrice,Vendor.VendorName from ItemVendor, Vendor Where Vendor.VendorID=ItemVendor.VendorID AND ItemID = '"+ row[0].ToString() + "' order by VendorCostPrice ASC"); // AND ItemID='" + row[0].ToString() + "'");
                    dsBestVendPrice = oBLSearch.SearchData(search);
                    if (dsBestVendPrice.Tables.Count > 0)
                    {
                        if (dsBestVendPrice.Tables[0].Rows.Count > 0)
                        {
                            row["BestPrice"] = dsBestVendPrice.Tables[0].Rows[0]["VendorCostPrice"];
                            row["BestVendor"] = dsBestVendPrice.Tables[0].Rows[0]["VendorName"];
                        }
                    }
                    dsBestVendPrice = null;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //private void SetDefaultVendorValues()
        //{
        //    try
        //    {
        //        if (oDataSetDefaultVendor.Tables.Count == 0)
        //        {
        //            return;
        //        }
        //        foreach (DataRow row in oDataSet.Tables[0].Select(" PreferredVendor is null"))
        //        {
        //            row[4] = Configuration.CPOSSet.DefaultVendor;
        //            DataRow[] defaultVendRow = oDataSetDefaultVendor.Tables[0].Select(" ItemID='" + row[0].ToString() + "'");
        //            if (defaultVendRow.Length > 0)
        //                row[5] = Convert.ToDecimal(defaultVendRow[0].ItemArray[6].ToString());
        //        }
        //        grdSearch.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsUIHelper.ShowErrorMsg(ex.Message);
        //    }
        //    grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
        //    grdSearch.Refresh();
        //}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			IsCanceled = false;
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			IsCanceled = true;
			this.Close();
		}

		private void grdSearch_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				Point point = System.Windows.Forms.Cursor.Position;
				point = this.grdSearch.PointToClient(point);
				Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point);
				if ( oUI == null )
					return;

				while ( oUI != null )
				{
					if ( oUI.GetType() == typeof( Infragistics.Win.UltraWinGrid.RowUIElement ) )
					{
						if ( grdSearch.Rows.Count == 0)
							return;
						IsCanceled = false;
						this.Close();

					}
					oUI = oUI.Parent;
				}
			}
			catch (Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message);}
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Clear();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void Clear()
		{
			txtCode.Text = "";
			txtName.Text= "";

			if (oDataSet.Tables.Count !=0) oDataSet.Tables[0].Clear();
			grdSearch.Refresh();
		}
        /// <summary>
        ///      Contains variable "SelectedRowCode" to Get Code of Selected row 
        /// </summary>
        /// <returns></returns>
		public string SelectedRowID()
		{
			if(!fromBestVendorPrice)
            {
                if (grdSearch.ActiveRow!=null)
                    if (grdSearch.ActiveRow.Cells.Count > 0)
                    {
                        //if-else is added by shitaljit to resolve the issues with vender search filter as vendor code is in cell [0] not in cell [1]
                        if (this.SearchTable == clsPOSDBConstants.Vendor_tbl)
                        {
                            SelectedRowCode = grdSearch.ActiveRow.Cells[0].Value.ToString();
                        }
                        else
                        {
                            SelectedRowCode = grdSearch.ActiveRow.Cells[1].Value.ToString();//this line added by Krishna on 3 May 2011
                        }
                        return grdSearch.ActiveRow.Cells[0].Text;
                    }
                    else
                        return "";
			    else
				    return "";
            }
            else
            {
                return strItemID;
            }
        }     

        //public string SelectedCode()
        //{
        //    if (!fromBestVendorPrice)
        //    {
        //        if (grdSearch.ActiveRow != null)
        //            if (grdSearch.ActiveRow.Cells.Count > 0)
        //                //Updated By SRT(Ritesh Parekh) Date : 22-Jul-2009
        //                //
        //                if (SearchTable == "VendorItemCodeWise")
        //                {
        //                    return grdSearch.ActiveRow.Cells["VendorItemId"].Text;
        //                }
        //                else if (SearchTable == "ItemID")
        //                {
        //                    return grdSearch.ActiveRow.Cells["ItemID"].Text;
        //                }
        //                else
        //                {
        //                    return grdSearch.ActiveRow.Cells["Code"].Text;
        //                }
        //            else
        //                return "";
        //        else
        //            return "";
        //    }
        //    else
        //    {
        //        return strVendorItemID;
        //    }
        //}

        //Added by SRT(Abhishek) Date : 21 Aug 2009
        //Added this method to return value of the VendorItemId colomn

        public string SelectedVendorItemCode(String columnName)
        {
            if (grdSearch.ActiveRow != null)
            {
                if (grdSearch.ActiveRow.Cells.Count > 0)
                {
                    if (grdSearch.ActiveRow.Cells.Exists(columnName))
                    {
                        return grdSearch.ActiveRow.Cells[columnName].Text;
                    }
                    else
                        return "";
                }
                else
                    return "";
            }
            else
                return "";
        }
        //End of Added by SRT(Abhishek) Date : 21 Aug 2009

        public string SelectedCode()
        {
            if (!fromBestVendorPrice)
            {
                if (grdSearch.ActiveRow != null)
                    if (grdSearch.ActiveRow.Cells.Count > 0)
                        //Updated By SRT(Ritesh Parekh) Date : 22-Jul-2009             
                        if (SearchTable == "VendorItemCodeWise")
                        {
                            return grdSearch.ActiveRow.Cells["VendorItemId"].Text;
                        }
                        else if (SearchTable == "ItemID")
                        {
                            return grdSearch.ActiveRow.Cells["VendorItemId"].Text;
                        }
                        else
                        {
                            return grdSearch.ActiveRow.Cells["Code"].Text;
                        }
                    else
                        return "";
                else
                    return "";
            }
            else
            {
                return strVendorItemID;
            }
        }

        public string SelectedVendorID()
        {
            if (grdSearch.ActiveRow != null)
                if (grdSearch.ActiveRow.Cells.Count > 0)
                    return grdSearch.ActiveRow.Cells["VendorId"].Text;
                else
                    return "";
            else
                return "";
        }

        public string SelectedBestVendor()
        {
            if (!fromBestVendorPrice)
            {
                if(grdSearch.ActiveRow != null)
                {
                    if (grdSearch.ActiveRow.Cells.Count > 0)
                        return grdSearch.ActiveRow.Cells["BestVendor"].Text;
                    else
                        return "";
                }
                else
                    return "";
            }
            else
            {
                return strBestVendorName;          
            } 
        }

        public string SelectedVendorCostPrice()
        {
            if (grdSearch.ActiveRow != null)
                if (grdSearch.ActiveRow.Cells.Count > 0)
                    return grdSearch.ActiveRow.Cells["VendorCostPrice"].Text;
                else
                    return "";
            else
                return "";
        }

        //public string SelectedPrefVendor()
        //{
        //    if (grdSearch.ActiveRow != null  )
        //    {
        //        if (grdSearch.ActiveRow.Cells.Count > 0)
        //            return grdSearch.ActiveRow.Cells["PreferredVendor"].Text;
        //        else
        //            return "";
        //    }
        //    else
        //        return "";
        //}

        public string SelectedBestVendorPrice()
        {
            if (!fromBestVendorPrice)
            {
                if (grdSearch.ActiveRow != null)
                {
                    if (grdSearch.ActiveRow.Cells.Count > 0)
                        return grdSearch.ActiveRow.Cells["BestPrice"].Text;
                    else
                        return "";
                }
                else
                    return "";
            }
            else
            {
                return strVendorCostPrice;
            }
        }

        private void btnClear_Click_1(object sender, System.EventArgs e)
		{
			try
			{
				this.Clear();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void frmSearch_Load(object sender, System.EventArgs e)
		{
			try
			{
                #region Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
                if (sCalledFrom.Trim().ToUpper() == "frmItems".ToUpper())
                {
                    btnOk.Visible = false;
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCopy.Visible = true;
                }
                else
                {
                    //btnOk.Visible = true;
                    //btnAdd.Visible = true;
                    //btnEdit.Visible = true;
                    //btnDelete.Visible = true;
                    btnCopy.Visible = false;
                }
                #endregion

                clsUIHelper.SetAppearance(this.grdSearch);
                this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeCell = SelectType.None;
                if (allowMultiRowSelect == false)
                {
                    this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Single;
                }
                else
                {
                    this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Extended;
                }

                this.chkSelectAll.Visible = this.allowMultiRowSelect;
                this.grdSearch.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;

                #region commented
                //Changed By SRT(Gaurav) Date:30-OCT-2008
                //if (PrgFlag == clsPOSDBConstants.VendorWise)
                //{
                //    this.radioItemId.Visible = true;
                //    this.radioVendItemId.Visible = true;
                //    //oDataSet = oBLSearch.SearchData(DataQuery);
                //    //PrgFlag = String.Empty;
                //}
                //else if (PrgFlag == clsPOSDBConstants.DescriptionWise)
                //{
                //    this.radioItemId.Visible = true;
                //    this.radioVendItemId.Visible = true;
                //    //oDataSet = oBLSearch.SearchData(DataQuery);
                //    //PrgFlag = String.Empty;
                //}
                //else if (PrgFlag == clsPOSDBConstants.VendorItemCodeWise)
                //{
                //    this.radioItemId.Visible = true;
                //    this.radioVendItemId.Visible = true;
                //    //oDataSet = oBLSearch.SearchData(DataQuery);
                //    //PrgFlag = String.Empty;
                //}
                //else
                //{
                //    oDataSet = oBLSearch.SearchData(SearchTable, "----------------Invalid--------------", "----------------Invalid----------------", ActiveOnly);
                //}
                //End Of Changed By SRT(Gaurav)
                // oDataSet = oBLSearch.SearchData(SearchTable,"----------------Invalid--------------","----------------Invalid----------------",ActiveOnly);
                #endregion

               // grdSearch.DataSource = oDataSet;
                //Added By Shitaljit to ignore System MEmeory exception in case there are many item in the Item Table.
                if (this.allowMultiRowSelect == true && this.SearchTable == clsPOSDBConstants.Item_tbl)
                {
                    oDataSet = oBLSearch.SearchData(SearchTable, "----------------Invalid--------------", "----------------Invalid----------------", ActiveOnly, -1);
                    grdSearch.DataSource = oDataSet;
                }
                resizeColumns();
                grdSearch.Refresh();

                this.txtName.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtName.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtMasterSearchVal.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtMasterSearchVal.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                //Added By Shitaljit(QuicSolv) on 3 June 2011
                if (SearchSvr.SubDeptIDFlag)
                {
                    this.ultraTabControl1.Enabled = false;
                }
                //Till here Added By Shitaljit.
                //Added by STR Gaurav
                //Commented By shitaljit
                ////if (!searchInConstructor)
                ////{

                ////    Search();

                ////}

                //Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 06-04-2011
               // Search will invoked if Property SearchInConstructor is set to True.
                if (searchStatus() || searchInConstructor)
                {
                    Search();
                }
                //Sprint-24 - 13-Jan-2017 JY Commented to avoid repetative call
                //if (searchInConstructor)
                //{
                //    Search();
                //}
                //End OfAdded By Shitaljit(QuicSolv)

                if (txtCode.Text == "--##--")
                {
                    txtCode.Text = "";
                }
               
				DefaultCode="";
				setTitle();
				if (this.SearchTable==clsPOSDBConstants.VendorCostPrice_View)
				{
					this.ultraTabControl1.Visible=false;
					this.grdSearch.Top=this.ultraTabControl1.Top;
					this.grdSearch.Height+=this.ultraTabControl1.Height;
					this.btnOk.Visible=false;
					this.btnCancel.Text="&Close";
				}
                //Added By Shitaljit(QuicSolv) on 1 August 2011
                if (this.SearchTable == clsPOSDBConstants.Item_tbl)
                {
                    //if (sCalledFrom.Trim().ToUpper() != "frmItems".ToUpper())
                    //{
                    //    AllowedDelete = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteItem.ID, 0);
                    //    this.btnAdd.Visible = true;
                    //    this.btnEdit.Visible = true;
                    //    this.btnDelete.Visible = AllowedDelete;
                    //}

                    #region PRIMEPOS-2733 17-Sep-2019 JY Added 
                    if (sCalledFrom.Trim().ToUpper() == "frmItems".ToUpper())
                    {
                        this.btnAdd.Visible = this.btnEdit.Visible = this.btnDelete.Visible = btnOk.Visible = false;
                    }
                    else
                    {
                        this.btnAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -999);
                        this.btnEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998);
                        this.btnDelete.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.DeleteItem.ID, 0);
                    }
                    #endregion

                    //Following add by Krishna on 16 August 2012
                    this.btnSearch.Location = new Point(580, 9);
                    this.btnAdvSearch.Location = new Point(715, 9);
                    this.btnAdvSearch.Visible = true;
                }
                //End of added by shitaljit.
                clsUIHelper.setColorSchecme(this);
                if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface || SearchTable == clsPOSDBConstants.PrimeRX_PatientInterface)
                {
                    this.txtMasterSearchVal.Visible = true;
                    this.lbl3.Visible = true;
                    this.txtMasterSearchVal.Visible = true;
                    this.lbl1.Location = new Point(3, 36);
                    this.lbl2.Location = new Point(308, 36);
                    this.txtCode.Location = new Point(92, 36);
                    this.txtName.Location = new Point(364, 36);
                    this.btnSearch.Location = new Point(640, 36);
                    this.txtMasterSearchVal.TabIndex = 0;
                    if (Configuration.CInfo.ShowTextPrediction == true)
                    {
                        string sSql =  GetHouseChargeSearchQuery();
                        ContAccount oSearch = new ContAccount();
                        oSearch.GetRecs(sSql, out oDataSet);
                        AutoCompleteStringCollection ItemsCollection = new AutoCompleteStringCollection();
                        foreach (DataRow oRow in oDataSet.Tables[0].Rows)
                        {
                            for (int index = 0; index < oRow.ItemArray.Length; index++)
                            {
                                ItemsCollection.Add(oRow.ItemArray[index].ToString());
                            }
                        }
                        this.txtMasterSearchVal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        this.txtMasterSearchVal.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        this.txtMasterSearchVal.AutoCompleteCustomSource = ItemsCollection;
                    }
                    this.txtMasterSearchVal.Focus();
                    //txtName.Focus();
                }
                else
                {
                    if (grdSearch.Rows.Count > 0)
                    {
                        grdSearch.Focus();
                        this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                    }
                    else
                    {
                        txtCode.Focus();
                    }
                }

                chkSelectAll.BackColor = Color.Transparent;

                if (selectedRowsData.Trim().Length > 0)
                {
                    string[] IDs = selectedRowsData.Split(new char[] {','});
                    for (int i = 0; i < IDs.Length; i++)    
                    {
                        string sID = IDs[i].Substring(1, IDs[i].Length - 2);
                        foreach (UltraGridRow gridRow in grdSearch.Rows)
                        {
                            if (gridRow.Cells.Count>0 && gridRow.Cells[0].Text == sID)
                            {
                                gridRow.Cells["CHECK"].Value= true;
                                gridRow.Update();
                            }
                        }
                    }
                    
                    grdSearch.DisplayLayout.Bands[0].SortedColumns.Add("CHECK", true);
                }
                //Added By Amit Date 2 Nov 2011
                if (SearchTable == "Department" && grdSearch.Rows.Count > 0 && strSetSelected != "")
                {
                    int count = 0;
                    foreach (UltraGridRow row in grdSearch.Rows)
                    {
                        if (row.Cells[0].Value.ToString() == strSetSelected)
                        {
                            grdSearch.ActiveRow = grdSearch.Rows[count];
                            break;
                        }

                        count++;
                    }
                }
                //End
                resizeColumns();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void setTitle()
		{
			string strCaption="";
		
            switch (SearchTable)
			{
				case clsPOSDBConstants.Customer_tbl:
					strCaption="Customer";
					break;
				case clsPOSDBConstants.Department_tbl:
					strCaption="Department";
					break;
				case clsPOSDBConstants.FunctionKeys_tbl:
					strCaption="Function Keys";
					break;
				case clsPOSDBConstants.InvRecvHeader_tbl:
					strCaption= "Inventory Received";   //PRIMEPOS-2824 25-Mar-2020 JY modified  
                    break;
				case clsPOSDBConstants.Item_tbl:
					strCaption="Item File";
					break;
				case clsPOSDBConstants.PayOut_tbl:
					strCaption="Payout";
					break;
				case clsPOSDBConstants.PayType_tbl:
					strCaption="Payment Type";
					break;
				case clsPOSDBConstants.PhysicalInv_tbl:
					strCaption="Physical Inventory";
					break;
				case clsPOSDBConstants.POHeader_tbl:
					strCaption="Purchase Order";
					break;
				case clsPOSDBConstants.TaxCodes_tbl:
					strCaption="Tax Table";
					break;
				case clsPOSDBConstants.TransHeader_tbl:
					strCaption="POS Transaction";
					break;
				case clsPOSDBConstants.Users_tbl:
					strCaption="Users Information";
					break;
				case clsPOSDBConstants.Vendor_tbl:
					strCaption="Vendors";
					break;
				case clsPOSDBConstants.VendorCostPrice_View:
					strCaption="Vendor Cost Price";
					break;
                case clsPOSDBConstants.PrimeRX_PatientInterface:
                    strCaption = "PrimeRX Patient";
                    break;
                case clsPOSDBConstants.PrimeRX_HouseChargeInterface:
                    strCaption = "House Charge Account";
                    break;
			}
			this.Text="Search " + strCaption;
		}

		private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            try {
                #region PRIMEPOS-2473 **Modified by Rohit Nair
                switch (e.KeyData) {
                    case Keys.Enter: {
                            if (this.grdSearch.ContainsFocus == true) {
                                if (this.grdSearch.Rows.Count > 0) {
                                    IsCanceled = false;
                                    this.Close();
                                }
                            }
                            if (this.txtMasterSearchVal.ContainsFocus == true) {
                                Search();
                            } else {
                                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                            }
                        }
                        break;

                    //case Keys.F5:
                    //    {
                    //        btnCopy_Click(null, null);
                    //    }
                    //    break;
                    default:
                        break;

                }
                #endregion
            } catch (Exception exp) {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }


        //Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 06-04-2011
        private bool searchStatus()
        {
            if (txtCode.Text !=""|| txtName.Text!="")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		private void frmSearch_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

		private void TextBoxKeyup(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyData==Keys.Down)
			{
				if (this.grdSearch.Rows.Count>0)
				{
					this.grdSearch.Focus();
					this.grdSearch.ActiveRow=this.grdSearch.Rows[0];
				}
			}				
		}

		private void resizeColumns()
		{
		   foreach(UltraGridColumn oCol in grdSearch.DisplayLayout.Bands[0].Columns)
			{
                oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
				if ( oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
				{
					oCol.CellAppearance.TextHAlign=Infragistics.Win.HAlign.Right;
				}
            }
		}
		
        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{       
		
		}

        private void ultraBtnBestPrice_Click(object sender, EventArgs e)
        {
            try
            {
                frmComparePrices compPrices = new frmComparePrices();
                string itemID = SelectedRowID();
                compPrices.CompareVendorPrices(itemID);
                //Updated By SRT(Gaurav) Date: 08-Jul-2009
                //Validated Best Price / Vndor Comparison for more than 1 vendors.
                if (compPrices.OtherVendorsCount > 1)
                {

                    compPrices.ShowDialog();

                    if (compPrices.IsIncluded)
                    {
                        fromBestVendorPrice = true;
                        strItemID = compPrices.SelectedItemID();
                        strVendorItemID = compPrices.SelectedVendItemId();
                        strDescription = compPrices.SelectedDescription();
                        strBestVendorName = compPrices.SelectedVendorName();
                        strVendorCostPrice = compPrices.SelectedCostPrice();
                        IsCanceled = false;
                        this.Close();
                    }
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Other Vendors Are Not Available For Item #" + itemID);
                }
                //End Of Updated By SRT(Gaurav)
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        public bool IsBestVendorPrice
        {
            get { return fromBestVendorPrice; }        
        }



        //Added By Shitaljit(QuicSolv) Date(dd/mm/yy): 06-04-2011
        //Modifeid By Shitaljit on 1 August 2011
        private void frmSearch_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.KeyData)
            {
                case Keys.F4:
                case Keys.Enter:
                    if (searchStatus())
                    {
                        Search();
                    }
                  break;
                case Keys.F2:
                    if (btnAdd.Visible == true) Add();
                    break;
                case Keys.F3:
                     if (grdSearch.Rows.Count < 1)
                        return;
                    if (btnEdit.Visible == true) Edit();
                    break;
                    //Added By shitaljit for Advance Search
                case Keys.F11:
                    if (this.SearchTable == clsPOSDBConstants.Item_tbl)
                    {
                        btnAdvSearch_Click(null, null);
                    }
                    break;
                    #region PRIMEPOS-2473 **Commented out by Rohit nair

                    //case Keys.F5:
                    //    btnCopy_Click(null, null);
                    //    break;
                    #endregion
            }
        }
       //End of Added By Shitaljit.
        void grdSearch_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.allowMultiRowSelect == true)
            {

                Point point = System.Windows.Forms.Cursor.Position;
                point = this.grdSearch.PointToClient(point);
                Infragistics.Win.UIElement oUI = this.grdSearch.DisplayLayout.UIElement.ElementFromPoint(point, Infragistics.Win.UIElementInputType.MouseClick);
                if (oUI == null)
                    return;

                while (oUI != null)
                {
                    if (oUI.GetType() == typeof(Infragistics.Win.UltraWinGrid.CellUIElement))
                    {
                        Infragistics.Win.UltraWinGrid.CellUIElement cellUIElement = (Infragistics.Win.UltraWinGrid.CellUIElement)oUI;
                        if (cellUIElement.Column.Key.ToUpper() == "CHECK")
                        {
                            CheckUncheckGridRow(cellUIElement.Cell);
                        }
                        break;
                    }
                    oUI = oUI.Parent;
                }
            }
        }

        private void CheckUncheckGridRow(UltraGridCell oCell)
        {
            if ((bool)oCell.Value == false)
            {
                oCell.Value = true;
            }
            else
            {
                oCell.Value = false;
            }
            oCell.Row.Update();
        }


        void grdSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (grdSearch.ActiveRow != null)
                {
                    CheckUncheckGridRow(this.grdSearch.ActiveRow.Cells["check"]);
                }
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            grdSearch.BeginUpdate();
            foreach (UltraGridRow oRow in grdSearch.Rows)
            {
                oRow.Cells["check"].Value = chkSelectAll.Checked;
                oRow.Update();
            }
            grdSearch.EndUpdate();
        }

        private void frmSearch_Leave(object sender, EventArgs e)
        {
            string Deptname= grdSearch.ActiveRow.Cells[1].ToString();
        }

        //Added By Shitaljit(QuicSolv) on 1 August 2011
        //Added Logic to Add, Edit and Delete Items.
        private void AddItem()
		{
			try
			{
				frmItems oItems = new frmItems();
				oItems.Initialize();
				oItems.ShowDialog(this);
				if (!oItems.IsCanceled)
					Search();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}
    
        private void Add()
        {
            try
            {
                if (btnOk.Visible == false) return;
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.RemoveParent();
                switch (SearchTable)
                {
                    case clsPOSDBConstants.Item_tbl:
                        AddItem();
                        break;
                    case clsPOSDBConstants.Vendor_tbl:
                        AddVendor();
                        break;

                }
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) 
                    frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
            }
            catch (Exception exp)
            {
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) 
                    frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditItem()
        {
            if (grdSearch.Rows.Count <= 0) return;

            try
            {
                frmItems oItems = new frmItems();
                oItems.Edit(grdSearch.ActiveRow.Cells[0].Text.Trim());
                oItems.ShowDialog(this);
                if (!oItems.IsCanceled && IsAdvSearchDone == false)
                {
                    Search();
                }
                //Added By shitaljit 0n 17 June 0213 for PRIMEPOS-1189 Advance Search Result list is not retained when you come back to it after editing an item
                else if (oItems.IsCanceled == true || IsAdvSearchDone == true)
                {
                    bool isVendRequired = false;
                    bool isItemVendorRequired = false;
                    DataSet itemData = new DataSet();
                    ItemSvr itemSvr = new ItemSvr();
                    string WhereClause = " WHERE ItemID = '" + grdSearch.ActiveRow.Cells[0].Text.Trim() + "'";
                    string WhereClause1 = string.Empty; //19-Jun-2015 JY Added WhereClause1 
                    itemData = itemSvr.PopulateAdvSearch(WhereClause, ref isVendRequired, ref isItemVendorRequired, WhereClause1);  //19-Jun-2015 JY Added WhereClause1 
                    if (Configuration.isNullOrEmptyDataSet(itemData) == false)
                    {
                        for (int Index = 0; Index < itemData.Tables[0].Columns.Count; Index++)
                        {
                            grdSearch.ActiveRow.Cells[Index].Value = Configuration.convertNullToString(itemData.Tables[0].Rows[0][Index]);

                        }
                        grdSearch.UpdateData();
                    }
                }
                //End
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void AddVendor()
        {
            //if (grdSearch.Rows.Count <=0) return;

            try
            {
                frmVendor oVendor = new frmVendor();
                oVendor.Initialize();
                oVendor.ShowDialog(this);
                if (!oVendor.IsCanceled)
                    Search();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Edit()
        {
            try
            {
                if (btnOk.Visible == false) return;
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed)) frmMain.oPublicNumericPad.RemoveParent();
                switch (SearchTable)
                {
                    case clsPOSDBConstants.Item_tbl:
                        EditItem();
                        break;
                    case clsPOSDBConstants.Vendor_tbl:
                        EditVendor();
                        break;

                }
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed))
                    frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
            }
            catch (Exception exp)
            {
                if (frmMain.oPublicNumericPad != null && (!frmMain.oPublicNumericPad.IsDisposed))
                    frmMain.oPublicNumericPad.AttachParent(new IntPtr(0));
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void EditVendor()
        {
            if (grdSearch.Rows.Count <= 0) return;
            try
            {
                frmVendor oVendor = new frmVendor();
                oVendor.Edit(grdSearch.ActiveRow.Cells[0].Text.Trim());
                oVendor.ShowDialog(this);
                if (!oVendor.IsCanceled)
                    Search();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void DeleteItem()
        {
            Item objItem = new Item();
            string itemID = string.Empty;
            try
            {
                if (grdSearch.ActiveRow != null)
                {
                    itemID = grdSearch.ActiveRow.Cells["Item Code"].Value.ToString();
                    if (Resources.Message.Display("Do you want to Delete Item # " + itemID + "?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (objItem.DeleteRow(itemID))
                            Search();
                        else
                            Resources.Message.Display("You cannot delete selected Item.\nItem# " + itemID + "  is used in transaction(s).");
                    }
                }
            }
            catch (Exception ex)
            {
                Resources.Message.Display("You cannot delete selected Item.\nItem# " + itemID + "  is used in transaction(s).");
            }
        }
        private void Delete()
        {
            try
            {
                switch (SearchTable)
                {
                    case clsPOSDBConstants.Item_tbl:
                        DeleteItem();
                        break;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void bttnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count <1)
				return;
            try
            {
                Delete();
            }
            catch (Exception ex)
            { 
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdSearch.Rows.Count < 1)
                return;
            try
            {
                Edit();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            try
            {
                frmItemAdvSearch FrmAdvSearch = new frmItemAdvSearch();
                FrmAdvSearch.ShowDialog();
                if (!FrmAdvSearch.IsCanceled)
                {
                    this.grdSearch.DisplayLayout.Bands[0].SortedColumns.Clear();    //19-Jun-2015 JY Added 
                    this.grdSearch.DataSource = FrmAdvSearch.itemData;
                    ultraTxtEditorNoOfItems.Text = FrmAdvSearch.itemData.Tables[0].Rows.Count.ToString();
                    IsAdvSearchDone = true;
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void radioFor7Days_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioFlagged_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region Sprint-26 - PRIMEPOS-2417 06-Jul-2017 JY Added
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (SelectedRowID() == string.Empty)
            {
                Resources.Message.Display("Please select any record","Copy Item...");
                return;
            }
            IsCanceled = false;
            this.Close();
        }
        #endregion

        private void txtMasterSearchVal_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {
                this.txtMasterSearchVal.Focus();
                this.txtMasterSearchVal.SelectionStart = Configuration.convertNullToInt(this.txtMasterSearchVal.Text.Length);
                this.txtCode.Enabled = false;
                this.txtName.Enabled = false;
            }
            else
            {
                this.txtCode.Enabled = true;
                this.txtName.Enabled = true;
            }
        }
        //End of Added By Shitaljit.
	}
}
