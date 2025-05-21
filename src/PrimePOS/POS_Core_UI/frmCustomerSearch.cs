using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.UserManagement;
//using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmVendorSearch.
	/// </summary>
	public class frmCustomerSearch : System.Windows.Forms.Form
	{
		private string SearchTable;
		public bool IsCanceled  = true;
		public string DefaultCode="";
		public int ActiveOnly=0;
		private Search oBLSearch = new Search();
		private DataSet oDataSet = new DataSet();
        private string ParamValue = string.Empty;
        private bool includeCPLCardInfo = false;
        private bool bAutoSelectSingleRow = false;
        private bool bOnlyCLPCardCustomers = false;

        //Added by Shitaljit 
        MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
        CustomerData oCustomerData = new CustomerData();
        public bool bSearchExactCustomer = false;//Added By Shitaljit to search the exact customer.
        private bool bAllowCustEdit = true;
        #region StoreCredit  PRIMEPOS-2747 11-Nov-2019 - NileshJ
        public bool IsStoreCredit = false;
        #endregion
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.Misc.UltraButton btnSearch;
		private Infragistics.Win.Misc.UltraButton btnOk;
		private Infragistics.Win.Misc.UltraButton btnCancel;
		private Infragistics.Win.Misc.UltraLabel ultraLabel25;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSearch;
		private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        public Infragistics.Win.Misc.UltraButton btnAdd;
        public Infragistics.Win.Misc.UltraButton btnEdit;
        public CheckBox chkIncludeRXCust;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar sbMain;
        public Infragistics.Win.UltraWinEditors.UltraComboEditor txtContactNumber;
        private TextBox txtCode;
        private TextBox txtName;
        private TextBox txtMasterSearchVal;
        private IContainer components;

        public frmCustomerSearch(string sInitCriteria)
            : this(sInitCriteria, false, false)
        {

        }

        public frmCustomerSearch(string sInitCriteria, bool autoSelectSingleRow)
            : this(sInitCriteria, false, autoSelectSingleRow)
        {
            
        }

        public frmCustomerSearch(string sInitCriteria, bool includeCLPCard, bool autoSelectSingleRow)
            :this(sInitCriteria,includeCLPCard,autoSelectSingleRow,false,false)
        {
        }

        public frmCustomerSearch(string sInitCriteria, bool includeCLPCard, bool autoSelectSingleRow, bool onlyCLPCardCustomers)
            :this(sInitCriteria,includeCLPCard,autoSelectSingleRow,onlyCLPCardCustomers,false)
        {
        }

        public frmCustomerSearch(string sInitCriteria, bool includeCLPCard, bool autoSelectSingleRow, bool onlyCLPCardCustomers, bool bSearchExactCustomer)
		{
            this.bSearchExactCustomer = bSearchExactCustomer;

			InitializeComponent();
           
            try
			{
                bool searchContact = false;
                if (sInitCriteria.EndsWith("/") && Configuration.convertNullToDouble(sInitCriteria.Substring(0,sInitCriteria.Length-1))>0)
                {
                    searchContact = true;
                }

                if (searchContact == true)
                {
                    txtContactNumber.Text = sInitCriteria.Substring(0, sInitCriteria.Length - 1);
                    sInitCriteria = string.Empty;
                }
                else if (Configuration.convertNullToDouble(sInitCriteria) !=0)
                {
                    txtCode.Text = sInitCriteria;
                }
                else
                {
                    txtName.Text = sInitCriteria;
                }

                bAutoSelectSingleRow = autoSelectSingleRow;
                includeCPLCardInfo = includeCLPCard;
                bOnlyCLPCardCustomers = onlyCLPCardCustomers;
                #region Master search code added by shitaljit on 29Apr2013
                if (bSearchExactCustomer == false)
                {
                    this.txtMasterSearchVal.Text = sInitCriteria;
                }
                #endregion
				
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomerSearch));
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
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
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
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtMasterSearchVal = new System.Windows.Forms.TextBox();
            this.txtContactNumber = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.chkIncludeRXCust = new System.Windows.Forms.CheckBox();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel25 = new Infragistics.Win.Misc.UltraLabel();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.grdSearch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnAdd = new Infragistics.Win.Misc.UltraButton();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.sbMain = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.txtCode);
            this.ultraTabPageControl1.Controls.Add(this.txtName);
            this.ultraTabPageControl1.Controls.Add(this.txtMasterSearchVal);
            this.ultraTabPageControl1.Controls.Add(this.txtContactNumber);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel4);
            this.ultraTabPageControl1.Controls.Add(this.chkIncludeRXCust);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel3);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel2);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel1);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel25);
            this.ultraTabPageControl1.Controls.Add(this.btnSearch);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(2, 23);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(976, 106);
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.Location = new System.Drawing.Point(472, 49);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(226, 24);
            this.txtCode.TabIndex = 2;
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxKeyup);
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Location = new System.Drawing.Point(114, 50);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(194, 24);
            this.txtName.TabIndex = 1;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxKeyup);
            // 
            // txtMasterSearchVal
            // 
            this.txtMasterSearchVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMasterSearchVal.Location = new System.Drawing.Point(114, 21);
            this.txtMasterSearchVal.Name = "txtMasterSearchVal";
            this.txtMasterSearchVal.Size = new System.Drawing.Size(584, 24);
            this.txtMasterSearchVal.TabIndex = 0;
            this.txtMasterSearchVal.TextChanged += new System.EventHandler(this.txtMasterSearchVal_TextChanged);
            this.txtMasterSearchVal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMasterSearchVal_KeyDown);
            // 
            // txtContactNumber
            // 
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance1.BorderColor3DBase = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.ItalicAsString = "False";
            appearance1.FontData.StrikeoutAsString = "False";
            appearance1.FontData.UnderlineAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtContactNumber.Appearance = appearance1;
            this.txtContactNumber.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            this.txtContactNumber.ButtonAppearance = appearance2;
            this.txtContactNumber.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.txtContactNumber.Location = new System.Drawing.Point(114, 78);
            this.txtContactNumber.MaxLength = 20;
            this.txtContactNumber.Name = "txtContactNumber";
            this.txtContactNumber.Size = new System.Drawing.Size(194, 23);
            this.txtContactNumber.TabIndex = 3;
            // 
            // ultraLabel4
            // 
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel4.Appearance = appearance3;
            this.ultraLabel4.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel4.Location = new System.Drawing.Point(8, 20);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(100, 19);
            this.ultraLabel4.TabIndex = 16;
            this.ultraLabel4.Text = "&Look For";
            this.ultraLabel4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkIncludeRXCust
            // 
            this.chkIncludeRXCust.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIncludeRXCust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkIncludeRXCust.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIncludeRXCust.ForeColor = System.Drawing.Color.Black;
            this.chkIncludeRXCust.Location = new System.Drawing.Point(472, 78);
            this.chkIncludeRXCust.Name = "chkIncludeRXCust";
            this.chkIncludeRXCust.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkIncludeRXCust.Size = new System.Drawing.Size(226, 21);
            this.chkIncludeRXCust.TabIndex = 4;
            this.chkIncludeRXCust.Text = "Include PrimeRX Customers";
            this.chkIncludeRXCust.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.chkIncludeRXCust.CheckedChanged += new System.EventHandler(this.chkIncludeRXCust_CheckedChanged);
            // 
            // ultraLabel3
            // 
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel3.Appearance = appearance4;
            this.ultraLabel3.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel3.Location = new System.Drawing.Point(8, 78);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(86, 19);
            this.ultraLabel3.TabIndex = 13;
            this.ultraLabel3.Text = "Contact #";
            // 
            // ultraLabel2
            // 
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.Name = "Arial";
            appearance5.FontData.SizeInPoints = 14F;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance5;
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(326, 50);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(48, 24);
            this.ultraLabel2.TabIndex = 11;
            this.ultraLabel2.Text = "AND";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraLabel2.Visible = false;
            // 
            // ultraLabel1
            // 
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance6;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Location = new System.Drawing.Point(8, 49);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(77, 19);
            this.ultraLabel1.TabIndex = 3;
            this.ultraLabel1.Text = "&Name";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel25
            // 
            appearance7.ForeColor = System.Drawing.Color.Black;
            this.ultraLabel25.Appearance = appearance7;
            this.ultraLabel25.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel25.Location = new System.Drawing.Point(380, 51);
            this.ultraLabel25.Name = "ultraLabel25";
            this.ultraLabel25.Size = new System.Drawing.Size(86, 19);
            this.ultraLabel25.TabIndex = 1;
            this.ultraLabel25.Text = "Account #";
            // 
            // btnSearch
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnSearch.Appearance = appearance8;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Location = new System.Drawing.Point(765, 64);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(125, 33);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "&Search(F4)";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            this.btnClear.Appearance = appearance9;
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
            // grdSearch
            // 
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.White;
            appearance10.BackColorDisabled = System.Drawing.Color.White;
            appearance10.BackColorDisabled2 = System.Drawing.Color.White;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdSearch.DisplayLayout.Appearance = appearance10;
            this.grdSearch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSearch.DisplayLayout.InterBandSpacing = 10;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BackColor2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.White;
            appearance12.BackColor2 = System.Drawing.Color.White;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.ActiveRowAppearance = appearance12;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.AddRowAppearance = appearance13;
            this.grdSearch.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdSearch.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.grdSearch.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackColorDisabled = System.Drawing.Color.White;
            appearance15.BackColorDisabled2 = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.Color.Black;
            appearance15.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdSearch.DisplayLayout.Override.CellAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.White;
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance16.BorderColor = System.Drawing.Color.Gray;
            appearance16.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
            appearance16.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance16.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdSearch.DisplayLayout.Override.CellButtonAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdSearch.DisplayLayout.Override.EditCellAppearance = appearance17;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredInRowAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.FilteredOutRowAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.White;
            appearance20.BackColorDisabled = System.Drawing.Color.White;
            appearance20.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.FixedCellAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance21.BackColor2 = System.Drawing.Color.Beige;
            this.grdSearch.DisplayLayout.Override.FixedHeaderAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance22.FontData.BoldAsString = "True";
            appearance22.FontData.SizeInPoints = 10F;
            appearance22.ForeColor = System.Drawing.Color.White;
            appearance22.TextHAlignAsString = "Left";
            appearance22.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdSearch.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.grdSearch.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAlternateAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.White;
            appearance24.BackColor2 = System.Drawing.Color.White;
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance24.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance24.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowPreviewAppearance = appearance25;
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance26.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.RowSelectorAppearance = appearance26;
            this.grdSearch.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdSearch.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance27.BackColor = System.Drawing.Color.Navy;
            appearance27.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdSearch.DisplayLayout.Override.SelectedCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.Navy;
            appearance28.BackColorDisabled = System.Drawing.Color.Navy;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance28.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance28.BorderColor = System.Drawing.Color.Gray;
            appearance28.ForeColor = System.Drawing.Color.White;
            this.grdSearch.DisplayLayout.Override.SelectedRowAppearance = appearance28;
            this.grdSearch.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdSearch.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance29.BorderColor = System.Drawing.Color.Gray;
            this.grdSearch.DisplayLayout.Override.TemplateAddRowAppearance = appearance29;
            this.grdSearch.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdSearch.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            appearance30.BackColor2 = System.Drawing.Color.White;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance30.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance30;
            this.grdSearch.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdSearch.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.grdSearch.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdSearch.Location = new System.Drawing.Point(12, 145);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.Size = new System.Drawing.Size(980, 340);
            this.grdSearch.TabIndex = 4;
            this.grdSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdSearch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSearch_InitializeLayout);
            this.grdSearch.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSearch_InitializeRow);
            this.grdSearch.DoubleClick += new System.EventHandler(this.grdSearch_DoubleClick);
            this.grdSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            // 
            // ultraTabControl1
            // 
            appearance31.FontData.BoldAsString = "True";
            this.ultraTabControl1.ActiveTabAppearance = appearance31;
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ultraTabControl1.Appearance = appearance32;
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Location = new System.Drawing.Point(12, 8);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(980, 131);
            this.ultraTabControl1.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            this.ultraTabControl1.TabIndex = 1;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance33.FontData.BoldAsString = "True";
            ultraTab1.Appearance = appearance33;
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            ultraTab1.ClientAreaAppearance = appearance34;
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
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(976, 106);
            // 
            // btnOk
            // 
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance35.FontData.BoldAsString = "True";
            appearance35.ForeColor = System.Drawing.Color.White;
            appearance35.Image = ((object)(resources.GetObject("appearance35.Image")));
            this.btnOk.Appearance = appearance35;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.Location = new System.Drawing.Point(793, 496);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 28);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance36.FontData.BoldAsString = "True";
            appearance36.ForeColor = System.Drawing.Color.White;
            appearance36.Image = ((object)(resources.GetObject("appearance36.Image")));
            this.btnCancel.Appearance = appearance36;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(897, 496);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 28);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance37.FontData.BoldAsString = "True";
            appearance37.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Appearance = appearance37;
            this.btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance38.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnAdd.HotTrackAppearance = appearance38;
            this.btnAdd.Location = new System.Drawing.Point(24, 497);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(101, 30);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "&Add (F2)";
            this.btnAdd.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnAdd.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.SystemColors.Control;
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance39.FontData.BoldAsString = "True";
            appearance39.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Appearance = appearance39;
            this.btnEdit.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEdit.HotTrackAppearance = appearance40;
            this.btnEdit.Location = new System.Drawing.Point(146, 497);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(101, 30);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "&Edit (F3)";
            this.btnEdit.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEdit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // sbMain
            // 
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.SystemColors.Control;
            appearance41.BorderColor = System.Drawing.Color.Black;
            appearance41.FontData.Name = "Verdana";
            appearance41.FontData.SizeInPoints = 10F;
            appearance41.ForeColor = System.Drawing.Color.White;
            this.sbMain.Appearance = appearance41;
            this.sbMain.Location = new System.Drawing.Point(0, 530);
            this.sbMain.Name = "sbMain";
            appearance42.BorderColor = System.Drawing.Color.Black;
            appearance42.BorderColor3DBase = System.Drawing.Color.Black;
            appearance42.ForeColor = System.Drawing.Color.Black;
            this.sbMain.PanelAppearance = appearance42;
            appearance43.BorderColor = System.Drawing.Color.White;
            ultraStatusPanel1.Appearance = appearance43;
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            ultraStatusPanel1.Width = 200;
            this.sbMain.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1});
            this.sbMain.Size = new System.Drawing.Size(994, 21);
            this.sbMain.TabIndex = 18;
            this.sbMain.ViewStyle = Infragistics.Win.UltraWinStatusBar.ViewStyle.VisualStudio2005;
            // 
            // frmCustomerSearch
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(994, 551);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.sbMain);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.ultraTabControl1);
            this.Controls.Add(this.btnClear);
            this.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomerSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this.frmSearch_Activated);
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearch_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmCustomerSearch_KeyUp);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sbMain)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
        
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			Search();
		}

        private string GetQuery()
        {
            //MOdified the logic by shitaljit to do exact match first OR give preference to account# in the like query
            string sSQL = string.Empty;
            string sAcctNoClause = string.Empty;
            string strNewRxCust = @"'PrimePOS'" + " as [Cust. Source]";
            string sSQL1 = string.Empty;
            //Added By shitaljit on 12 Aug 2013 to allow special character "'" in specially for customer Name while searching for P
            //RIMEPOS-1308 customer name is a special character cannot be searched in POS 
            if (string.IsNullOrEmpty(this.txtName.Text) == false)
            {
                this.txtName.Text = this.txtName.Text.Replace("'","''");
            }
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {
                this.txtMasterSearchVal.Text = this.txtMasterSearchVal.Text.Replace("'", "''");
            }
            if (string.IsNullOrEmpty(this.txtCode.Text) == false)
            {
                this.txtCode.Text = this.txtCode.Text.Replace("'", "''");
            }
            //End
            if (includeCPLCardInfo == true)
            {
                sSQL = " Select "
                + "Customer." + clsPOSDBConstants.Customer_Fld_CustomerId + ", "
                + "Customer." + clsPOSDBConstants.Customer_Fld_CustomerCode + ", "//added by shitaljit Quicsolv on 2 Nov 2011
                + clsPOSDBConstants.Customer_Fld_AcctNumber + " as Account#," +
                clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+ IsNull(" + clsPOSDBConstants.Customer_Fld_FirstName + ",'') as Name," +
                "clCard." + clsPOSDBConstants.CLCards_Fld_CLCardID + " as [CLP Card ID], " +
                "clCard." + clsPOSDBConstants.CLCards_Fld_RegisterDate + " as [CLP Registered Date]," +
                clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                clsPOSDBConstants.Customer_Fld_City + " as City," +
                clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                clsPOSDBConstants.Customer_Fld_Email + " as Email , " +
                clsPOSDBConstants.Customer_tbl+"."+ clsPOSDBConstants.Customer_Fld_IsActive + " as IsActive ," +//added by shitaljit Quicsolv on 20 Feb 2012
                strNewRxCust + " , " +//added by shitaljit Quicsolv on 26 May 2012
                clsPOSDBConstants.Customer_Fld_Zip + " as Zip ," +
                clsPOSDBConstants.Customer_Fld_FaxNo +" as Fax# , "+
                clsPOSDBConstants.Customer_Fld_DriveLicNo +" as DL# , "+
                clsPOSDBConstants.Customer_Fld_DriveLicState + " as [DL State], " +
                clsPOSDBConstants.Customer_Fld_DateOfBirth + " as DOB , " +
                clsPOSDBConstants.Customer_Fld_PatientNo +" as Patient# ,"+
                clsPOSDBConstants.Customer_Fld_Email +" as Email, "+
                clsPOSDBConstants.Customer_Fld_Comments+
                
                " From Customer Left Join " + clsPOSDBConstants.CLCards_tbl + " as clCard On Customer.CustomerID=clCard.CustomerID " +
                " where 1=1 AND  clCard.isActive=1 ";
                #region Master Search Code Added By shitaljit for JIRA-375 on 29Apr13
                if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
                {
                    //Added By shitaljit to give preference to Acct# in search Query.
                    if (clsUIHelper.isNumeric(this.txtMasterSearchVal.Text) == true)
                    {
                        sSQL1 += sSQL + " AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + " LIKE (" + "'" + this.txtMasterSearchVal.Text + "%') ";
                        
                        sAcctNoClause = "AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + " NOT LIKE (" + "'" + this.txtMasterSearchVal.Text + "%') AND  (";
                        if (ActiveOnly == 1)
                        {
                            sSQL1 += " And Customer.isActive=1 ";
                        }

                        if (bOnlyCLPCardCustomers)
                        {
                            sSQL1 += " And clCard." + clsPOSDBConstants.CLCards_Fld_CLCardID + " Is Not Null ";
                        }
                        sSQL1 += " UNION "; //Sprint-25 - PRIMEPOS-2411 27-Apr-2017 JY changed "Union all" to "Union" to avoid duplicates 
                    }
                    else
                    {
                        sAcctNoClause = "AND (" + clsPOSDBConstants.Customer_Fld_AcctNumber + "  LIKE (" + "'" + this.txtMasterSearchVal.Text + "%')OR   ";
                    }
                    

                    sSQL += sAcctNoClause
                     + clsPOSDBConstants.Customer_Fld_CustomerName + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_FirstName + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Address1 + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Address2 + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                      + clsPOSDBConstants.Customer_Fld_Zip + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_City + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_CellNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PhoneOffice + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Comments + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_FaxNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_DriveLicNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     +" CONVERT(VARCHAR(25),"+ clsPOSDBConstants.Customer_Fld_DateOfBirth+", 126)" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_DriveLicState + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PatientNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PhoneHome + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Email + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')  ";

                }
                #endregion
                else
                {
                    if (txtCode.Text.Trim() != "")
                    {
                        Int32 iID = POS_Core.Resources.Configuration.convertNullToInt(txtCode.Text);
                        if (bSearchExactCustomer == true)
                        {
                            sSQL += " And Customer.AcctNumber='"+ iID.ToString() + "' ";
                        }
                        else
                        {
                            sSQL += " And AcctNumber LIKE (" + "'" + iID.ToString() + "%')"; //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE 
                        }
                    }

                    if (txtName.Text.Trim() != "")
                    {
                        sSQL += " And CustomerName+','+FirstName like '" + txtName.Text.Replace("'", "''").Replace(",", "%,") + "%'";
                    }
                    //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE 
                    if (txtContactNumber.Text.Trim() != "")
                    {
                        sSQL += " And ( CellNo LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                        sSQL += " Or  PhoneOff LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                        sSQL += " Or  PhoneHome LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%'))";
                    }
                }
                if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false  && (ActiveOnly == 1 || bOnlyCLPCardCustomers == true))
                {
                    sSQL += ")";
                }
                if (ActiveOnly == 1)
                {
                    sSQL += " And Customer.isActive=1 ";
                }

                if (bOnlyCLPCardCustomers)
                {
                    sSQL += " And clCard." + clsPOSDBConstants.CLCards_Fld_CLCardID + " Is Not Null ";
                }
                if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false && clsUIHelper.isNumeric(this.txtMasterSearchVal.Text) == true && ActiveOnly == 0 && bOnlyCLPCardCustomers == false)
                {
                     sSQL += ")";
                }
            }
            else
            {
                sSQL = " Select "
                    + clsPOSDBConstants.Customer_Fld_CustomerId + ", "
                    + "Customer." + clsPOSDBConstants.Customer_Fld_CustomerCode + ", "//added by shitaljit Quicsolv on 2 Nov 2011
                    + clsPOSDBConstants.Customer_Fld_AcctNumber + " as Account#," +
                    clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+ IsNull(" + clsPOSDBConstants.Customer_Fld_FirstName + ",'') as Name," +
                    clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                    clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                    clsPOSDBConstants.Customer_Fld_City + " as City," +
                    clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                    clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                    clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                    clsPOSDBConstants.Customer_Fld_Email + " as Email ," +
                    clsPOSDBConstants.Customer_tbl+"."+clsPOSDBConstants.Customer_Fld_IsActive + " as IsActive ," +//added by shitaljit Quicsolv on 20 Feb 2012
                    strNewRxCust +" , "+//added by shitaljit Quicsolv on 26 May 2012
                    clsPOSDBConstants.Customer_Fld_Zip + " as Zip ," +
                    clsPOSDBConstants.Customer_Fld_FaxNo + " as Fax# , " +
                    clsPOSDBConstants.Customer_Fld_DriveLicNo + " as DL# , " +
                    clsPOSDBConstants.Customer_Fld_DriveLicState + " as [DL State], " +
                    clsPOSDBConstants.Customer_Fld_DateOfBirth + " as DOB , " +
                    clsPOSDBConstants.Customer_Fld_PatientNo + " as Patient# ," +
                    clsPOSDBConstants.Customer_Fld_Email + " as Email, " +
                    clsPOSDBConstants.Customer_Fld_Comments+
                    " From Customer  Where 1=1";

                Int32 iID = POS_Core.Resources.Configuration.convertNullToInt(txtCode.Text);
                #region Master Search Code Added By shitaljit for JIRA-375 on 29Apr13
                if (string.IsNullOrEmpty(txtCode.Text.Trim()) == false && bSearchExactCustomer == true)
                {
                    sSQL += " And AcctNumber = '" + iID.ToString() + "'";
                    return sSQL;
                }

                if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
                {
                    //Added By shitaljit to give preference to Acct# in search Query.
                    if (clsUIHelper.isNumeric(this.txtMasterSearchVal.Text) == true)
                    {
                        sSQL1 += sSQL + " AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + " LIKE (" + "'" + this.txtMasterSearchVal.Text + "%') UNION ";    //Sprint-25 - PRIMEPOS-2411 27-Apr-2017 JY changed "Union all" to "Union" to avoid duplicates 
                        sAcctNoClause = " AND ";
                    }
                    else
                    {
                        sAcctNoClause = "AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + "  LIKE (" + "'" + this.txtMasterSearchVal.Text + "%') OR   ";
                    }

                    sSQL += sAcctNoClause
                     + clsPOSDBConstants.Customer_Fld_CustomerName + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_FirstName + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Address1 + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Address2 + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Zip + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_City + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_CellNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PhoneOffice + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Comments + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_FaxNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_DriveLicNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                    + " CONVERT(VARCHAR(25)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126)" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_DriveLicState + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PatientNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_PhoneHome + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                     + clsPOSDBConstants.Customer_Fld_Email + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')  ";

                }
                #endregion
                else
                {
                    if (string.IsNullOrEmpty(txtCode.Text.Trim()) == false && bSearchExactCustomer == false)
                    {
                        sSQL += " And AcctNumber LIKE (" + "'" + iID.ToString() + "%')";  //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE 
                    }

                    if (string.IsNullOrEmpty(txtName.Text.Trim()) == false)
                    {//from here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                        string lName = "";
                        string fName = "";
                        if (txtName.Text.Trim().Contains(","))
                        {
                            char[] seperater = { ',' };
                            string[] lFname = txtName.Text.Split(seperater);
                            lName = lFname[0].Trim();
                            if (lFname.Length > 1)
                                fName = lFname[1].Trim();
                        }
                        else
                        {
                            lName = txtName.Text.Trim();
                        }
                        //Till here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                        // commented by Ravindra(Quicsolv) to Search Customer by   last and first name
                        //sSQL += " And CustomerName+','+FirstName like '" + txtName.Text.Replace("'", "''").Replace(",", "%,") + "%'";
                        sSQL += " And CustomerName like '" + lName + "%' and +FirstName like '" + fName + "%'";
                    }
                    //Modified by Shitaljit on 29 May 2012 make '=' TO LIKE 
                    if (txtContactNumber.Text.Trim() != "")
                    {
                        sSQL += " And ( CellNo LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                        sSQL += " Or  PhoneOff LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                        sSQL += " Or  PhoneHome LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%'))";
                    }

                    if (ActiveOnly == 1)
                    {
                        sSQL += " And Customer.isActive=1 ";
                    }
                }
            }
            //Added By shitaljit on 12 Aug 2013 to allow special character "'" in specially for customer Name while searching for 
            //PRIMEPOS-1308 customer name is a special character cannot be searched in POS 
            if (string.IsNullOrEmpty(this.txtName.Text) == false)
            {
                this.txtName.Text = this.txtName.Text.Replace("''", "'");
            }
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {
                this.txtMasterSearchVal.Text = this.txtMasterSearchVal.Text.Replace("''", "'");
            }
            if (string.IsNullOrEmpty(this.txtCode.Text) == false)
            {
                this.txtCode.Text = this.txtCode.Text.Replace("''", "'");
            }
            //End 
            if (string.IsNullOrEmpty(sSQL1) == false)
            {
                return (sSQL1 + sSQL);
            }
            return sSQL;
        }

        private void Search()
        {
            try
            {

                if (DefaultCode.Trim() == "")
                    DefaultCode = this.txtCode.Text.Trim();

                //  string sSQL = GetQuery(); // Commented By Nileshj for StoreCredit PRIMEPOS-2747
                #region StoreCredit PRIMEPOS-2747 -NileshJ
                string sSQL = string.Empty;
                if (!IsStoreCredit)
                {
                    sSQL = GetQuery();
                }
                else
                {
                    sSQL = GetStoreCreditQuery();
                }
                #endregion
                oDataSet = oBLSearch.SearchData(sSQL);
                //Added By shitaljit to display new RX customers.
                #region code to display new RX customers.

                if ((this.txtName.Text != "" || this.txtContactNumber.Text != "" || string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
                    && this.chkIncludeRXCust.Checked == true && bSearchExactCustomer == false)
                {
                    DataSet dsRxPatient = null;

                    string sSQLPat = string.Empty;
                    sSQLPat = "SELECT * FROM Patient ";


                    string strSelectClause = " WHERE 1=1 ";
                    if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
                    {
                        strSelectClause += "AND LNAME LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "FNAME LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "ADDRSTR LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "ADDRCT LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "ADDRST LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "ADDRZP LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "PHONE LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "EMAIL LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "WORKNO LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                            + "MOBILENO LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')   ";
                    }
                    else
                    {
                        if (txtName.Text.Trim() != "")
                        {//from here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                            string lName = "";
                            string fName = "";
                            if (txtName.Text.Trim().Contains(","))
                            {
                                char[] seperater = { ',' };
                                string[] lFname = txtName.Text.Split(seperater);
                                lName = lFname[0].Trim();
                                if (lFname.Length > 1)
                                    fName = lFname[1].Trim();
                            }
                            else
                            {
                                lName = txtName.Text.Trim();
                            }
                            //Till here AddCustomer  by Ravindra(Quicsolv) to Search Customer by   last and first name
                            // commented by Ravindra(Quicsolv) to Search Customer by   last and first name
                            // strSelectClause += " AND  LNAME +','+ FNAME LIKE '" + txtName.Text.Replace("'", "''").Replace(",", "%,") + "%'";
                            //sSQL += " And CustomerName like '" + lName + "%' and +FirstName like '" + fName + "%'";

                            strSelectClause += "AND   LNAME like '" + lName + "%'AND FNAME like '" + fName + "%'";
                            //strSelectClause += " AND  LNAME +','+ FNAME LIKE '" + txtName.Text.Replace("'", "''").Replace(",", "%,") + "%'";
                        }

                        else if (txtContactNumber.Text.Trim() != "")
                        {
                            strSelectClause += " AND  MOBILENO LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                            strSelectClause += " OR  WORKNO LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                            strSelectClause += " OR  PHONE LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                        }
                    }
                    sSQLPat = sSQLPat + strSelectClause;
                    oAcct.GetRecs(sSQLPat, out dsRxPatient);

                    if (Configuration.isNullOrEmptyDataSet(dsRxPatient) == false)
                    {
                        Customer oCustomer = new Customer();
                        DataSet dsRxCustomers = new DataSet();
                        DataTable dt = new DataTable();
                        dt = dsRxPatient.Tables[0].Clone();
                        foreach (DataRow dr in dsRxPatient.Tables[0].Rows)
                        {
                            dt.ImportRow(dr);
                        }
                        dsRxCustomers.Tables.Add(dt);
                        oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(dsRxCustomers, true);
                        oCustomerData.Tables[0].Columns.Add("Name", Type.GetType("System.String"));
                        oCustomerData.Tables[0].Columns.Add("Cell No.", Type.GetType("System.String"));
                        oCustomerData.Tables[0].Columns.Add("Phone Office", Type.GetType("System.String"));
                        oCustomerData.Tables[0].Columns.Add("Phone Home", Type.GetType("System.String"));
                        oCustomerData.Tables[0].Columns.Add("Cust. Source", Type.GetType("System.String"));
                        oDataSet.Tables[0].TableName = "POSCustomers";
                        int RowIndex = 0;
                        foreach (CustomerRow oRow in oCustomerData.Tables[0].Rows)
                        {
                            oCustomerData.Tables[0].Rows[RowIndex]["Name"] = oRow.CustomerFullName;
                            oCustomerData.Tables[0].Rows[RowIndex]["Cell No."] = oRow.CellNo;
                            oCustomerData.Tables[0].Rows[RowIndex]["Phone Office"] = oRow.PhoneOffice;
                            oCustomerData.Tables[0].Rows[RowIndex]["Phone Home"] = oRow.PhoneHome;
                            oCustomerData.Tables[0].Rows[RowIndex]["Cust. Source"] = "PrimeRx";
                            oDataSet.Tables[0].ImportRow(oRow);
                            RowIndex++;
                        }
                    }
                }

                #endregion

                grdSearch.DataSource = oDataSet;
                grdSearch.DisplayLayout.Bands[0].Columns["CustomerID"].Hidden = true;
                grdSearch.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Customer_Fld_CustomerCode].Hidden = true;
                grdSearch.DisplayLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.None;

                this.grdSearch.DisplayLayout.Bands[0].Columns["Name"].Width = 150;
                this.grdSearch.DisplayLayout.Bands[0].Columns["Address1"].Width = 150;
                if (grdSearch.Rows.Count > 0)
                {
                    this.grdSearch.ActiveRow = this.grdSearch.Rows[0];
                    grdSearch.Focus();
                }
                else
                {
                    txtMasterSearchVal.Focus();
                }
                resizeColumns();
                sbMain.Panels[0].Text = "Record(s) Count = " + grdSearch.Rows.Count.ToString();
                grdSearch.PerformAction(UltraGridAction.FirstRowInGrid);
                grdSearch.Refresh();
                bSearchExactCustomer = false;
            }

            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

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
            txtContactNumber.Text = "";
			if (oDataSet.Tables.Count !=0) oDataSet.Tables[0].Clear();

			grdSearch.Refresh();
		}

		public string SelectedRowID()
		{
			if (grdSearch.ActiveRow!=null)
				if (grdSearch.ActiveRow.Cells.Count>0)
					return grdSearch.ActiveRow.Cells[0].Text;
				else
					return "";
			else
				return "";
		}

        public string SelectedCLPCardID()
        {
            if (grdSearch.ActiveRow != null)
                if (grdSearch.ActiveRow.Cells.Count > 0)
                    return grdSearch.ActiveRow.Cells["CLP Card ID"].Text;
                else
                    return "";
            else
                return "";
        }

		public string SelectedCode()
		{
			if (grdSearch.ActiveRow!=null)
				if (grdSearch.ActiveRow.Cells.Count>0)
                    return grdSearch.ActiveRow.Cells[clsPOSDBConstants.Customer_Fld_CustomerCode].Text;
				else
					return "";
			else
				return "";
		}
        public string SelectedAcctNo()
        {
            if (grdSearch.ActiveRow != null)
                if (grdSearch.ActiveRow.Cells.Count > 0)
                    return grdSearch.ActiveRow.Cells["Account#"].Text;
                else
                    return "";
            else
                return "";
        }
        public CustomerRow SelectedRow()
        {
            CustomerRow oCustRow = null;
            if (grdSearch.ActiveRow != null)
            {
                if (grdSearch.ActiveRow.Cells.Count > 0 && this.oCustomerData.Tables[0].Rows.Count > 0 && this.chkIncludeRXCust.Checked == true)
                {
                    foreach (CustomerRow oRow in oCustomerData.Tables[0].Rows)
                    {
                        if (oRow.CustomerId == POS_Core.Resources.Configuration.convertNullToInt(this.SelectedRowID()))
                        {
                            oCustRow = oRow;
                            break;
                        }
                    }
                }
            }
            return oCustRow;
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
                clsUIHelper.SetAppearance(this.grdSearch);
                this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeCell = SelectType.None;
                this.grdSearch.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Single;
                this.grdSearch.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;

                this.txtName.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtName.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.txtMasterSearchVal.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtMasterSearchVal.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                this.txtCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                clsUIHelper.setColorSchecme(this);
                this.bAllowCustEdit = false;

                Search();

                setTitle();

                this.btnEdit.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -998);
                this.btnAdd.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -999);

                if (bAutoSelectSingleRow == true && grdSearch.Rows.Count == 1)
                {
                    IsCanceled = false;
                    this.Close();
                }
                #region Fetching Item Descriptions for showing Inteligence to users
                if (Configuration.CInfo.ShowTextPrediction == true)
                {
                    Search oSearch = new Search();

                    AutoCompleteStringCollection ItemDescCollection = new AutoCompleteStringCollection();
                    ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Customer_tbl, clsPOSDBConstants.Customer_Fld_CustomerName + "+','+" + clsPOSDBConstants.Customer_Fld_FirstName);
                    this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                    this.txtName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.txtName.AutoCompleteCustomSource = ItemDescCollection;

                    string sSQL = oSearch.GetSearchEngineQuery(clsPOSDBConstants.Customer_tbl);
                    ItemDescCollection = oSearch.GetAutoCompleteCollectionData(clsPOSDBConstants.Customer_tbl, sSQL);
                    this.txtMasterSearchVal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                    this.txtMasterSearchVal.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.txtMasterSearchVal.AutoCompleteCustomSource = ItemDescCollection;
                }
                this.txtMasterSearchVal.Focus();
                #endregion
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

		private void setTitle()
		{
            this.Text = "Search Customer";
		}

		private void frmSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            if (bAllowCustEdit == false)
            {
                bAllowCustEdit = true;
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
			foreach (UltraGridColumn oCol in grdSearch.DisplayLayout.Bands[0].Columns)
			{
				oCol.Width =oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows,true);
				if ( oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
				{
					oCol.CellAppearance.TextHAlign=Infragistics.Win.HAlign.Right;
				}
            }
		}
		
        private void grdSearch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            if (grdSearch.DisplayLayout.Bands.Count > 0)
            {
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Home"].MaskInput = "(###) ###-####";
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Home"].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Home"].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Home"].MaskDisplayMode= Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                grdSearch.DisplayLayout.Bands[0].Columns["Phone Office"].MaskInput = "(###) ###-####";
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Office"].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Office"].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth; 
                grdSearch.DisplayLayout.Bands[0].Columns["Phone Office"].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;

                grdSearch.DisplayLayout.Bands[0].Columns["Cell No."].MaskInput = "(###) ###-####";
                grdSearch.DisplayLayout.Bands[0].Columns["Cell No."].MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
                grdSearch.DisplayLayout.Bands[0].Columns["Cell No."].MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
                grdSearch.DisplayLayout.Bands[0].Columns["Cell No."].MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeBoth;
            }
		}

        private void grdSearch_InitializeRow(object sender, InitializeRowEventArgs e)
        {
        }

        private void AddCustomer()
        {
            try
            {
                if (btnAdd.Visible == false) return;
                frmCustomers oCustomer = new frmCustomers();
                //oCustomer.Initialize();   //28-Nov-2017 JY No need to initiate 
                oCustomer.Owner = this;
                oCustomer.ShowDialog(this);
                if (!oCustomer.IsCanceled)
                {
                    
                    Search();
                    if (grdSearch.Rows.Count > 0)
                    {
                        this.grdSearch.BeginUpdate();
                        this.grdSearch.Selected.Rows.Clear();
                        this.grdSearch.ActiveRow = this.grdSearch.Rows[this.grdSearch.Rows.Count - 1];
                        this.grdSearch.EndUpdate();
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            bAllowCustEdit = true;
            EditCustomer();
        }

        private void EditCustomer()
        {
            if (btnEdit.Visible == false) return;

            if (grdSearch.Rows.Count <= 0) return;
            if (bAllowCustEdit == false) return;

            Customer ogetCustomer = new Customer();
            CustomerData oCustdata = new CustomerData();
            CustomerRow oCustRow = null;
            try
            {
                string strCode = SelectedRowID();
                oCustdata = ogetCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));
                if (oCustdata.Tables[0].Rows.Count == 0)
                {
                    oCustRow = SelectedRow();
                    if (oCustRow != null)
                    {
                        if (oCustRow.Address1.ToString().Trim() == "")
                        {
                            oCustRow.Address1 = "-";
                        }
                        if (oCustRow.State.ToString().Trim() == "")
                        {
                            oCustRow.State = "-";
                        }
                        oCustdata.Tables[0].ImportRow(oCustRow);
                        ogetCustomer.Persist(oCustdata);
                        oCustdata = ogetCustomer.GetCustomerByPatientNo(oCustRow.PatientNo);
                        if (oCustdata.Tables[0].Rows.Count > 0)
                        {
                            oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                            strCode = oCustRow.CustomerId.ToString();
                        }
                    }
                }
                else
                {
                    strCode = grdSearch.ActiveRow.Cells[0].Text;
                }
                frmCustomers oCustomer = new frmCustomers();
                oCustomer.Edit(strCode);
                oCustomer.ShowDialog(this);
                if (!oCustomer.IsCanceled)
                    Search();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddCustomer();
        }

        private void frmCustomerSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter && bAllowCustEdit == true)
                {
                    if (this.grdSearch.ContainsFocus == true)
                    {
                        if (this.grdSearch.Rows.Count > 0)
                        {
                            IsCanceled = false;
                            this.Close();
                        }
                    }
                    else
                    {
                        btnSearch_Click(btnSearch, new EventArgs());
                        //this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    }
                }
                else if (e.KeyData == Keys.F2)
                {
                    AddCustomer();
                }
                else if (e.KeyData == Keys.F3)
                {
                    EditCustomer();
                    bAllowCustEdit = true;
                }
                else if (e.KeyData == Keys.Escape)
                {
                    this.btnCancel_Click(btnCancel, new EventArgs());
                    e.Handled = true;
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    Search();
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F8 && bAllowCustEdit == false)
                {
                    Search();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void chkIncludeRXCust_CheckedChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void txtMasterSearchVal_KeyDown(object sender, KeyEventArgs e)
        {

            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false && e.KeyData == Keys.Enter)
            {
                Search();
                this.txtMasterSearchVal.Focus();
                bAllowCustEdit = false;
                this.txtMasterSearchVal.SelectionStart = Configuration.convertNullToInt(this.txtMasterSearchVal.Text.Length);
                this.txtCode.Enabled = false;
                this.txtName.Enabled = false;
                this.txtContactNumber.Enabled = false;
            }
            else
            {
                this.txtCode.Enabled = true;
                this.txtName.Enabled = true;
                this.txtContactNumber.Enabled = true;
            }

        }

        private void txtMasterSearchVal_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {
                this.txtMasterSearchVal.Focus();
                this.txtMasterSearchVal.SelectionStart = Configuration.convertNullToInt(this.txtMasterSearchVal.Text.Length);
                this.txtCode.Enabled = false;
                this.txtName.Enabled = false;
                this.txtContactNumber.Enabled = false;
            }
            else
            {
                this.txtCode.Enabled = true;
                this.txtName.Enabled = true;
                this.txtContactNumber.Enabled = true;
            }
        }

        #region StoreCredit PRIMEPOS-2747 -NileshJ - 11-Nov-2019
        private string GetStoreCreditQuery()
        {

            string sSQL = string.Empty;
            string sAcctNoClause = string.Empty;
            string strNewRxCust = @"'PrimePOS'" + " as [Cust. Source]";
            string sSQL1 = string.Empty;
            if (string.IsNullOrEmpty(this.txtName.Text) == false)
            {
                this.txtName.Text = this.txtName.Text.Replace("'", "''");
            }
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {
                this.txtMasterSearchVal.Text = this.txtMasterSearchVal.Text.Replace("'", "''");
            }

            if (string.IsNullOrEmpty(this.txtCode.Text) == false)
            {
                this.txtCode.Text = this.txtCode.Text.Replace("'", "''");
            }

            sSQL = " Select "
                + "Customer." + clsPOSDBConstants.Customer_Fld_CustomerId + ", "
                + "Customer." + clsPOSDBConstants.Customer_Fld_CustomerCode + ", "
                + clsPOSDBConstants.Customer_Fld_AcctNumber + " as Account#," +
                clsPOSDBConstants.Customer_Fld_CustomerName + "+', '+ IsNull(" + clsPOSDBConstants.Customer_Fld_FirstName + ",'') as Name," +
                "sCard.CreditAmt as [Credit Amount] , " +
                clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                clsPOSDBConstants.Customer_Fld_City + " as City," +
                clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                clsPOSDBConstants.Customer_Fld_Email + " as Email ," +
                clsPOSDBConstants.Customer_tbl + "." + clsPOSDBConstants.Customer_Fld_IsActive + " as IsActive ," +
                strNewRxCust + " , " +
                clsPOSDBConstants.Customer_Fld_Zip + " as Zip ," +
                clsPOSDBConstants.Customer_Fld_FaxNo + " as Fax# , " +
                clsPOSDBConstants.Customer_Fld_DriveLicNo + " as DL# , " +
                clsPOSDBConstants.Customer_Fld_DriveLicState + " as [DL State], " +
                clsPOSDBConstants.Customer_Fld_DateOfBirth + " as DOB , " +
                clsPOSDBConstants.Customer_Fld_PatientNo + " as Patient# ," +
                clsPOSDBConstants.Customer_Fld_Email + " as Email, " +
                clsPOSDBConstants.Customer_Fld_Comments +
                " From Customer Left Join " + clsPOSDBConstants.CLCards_tbl + " as clCard On Customer.CustomerID=clCard.CustomerID " +
                " inner Join StoreCredit as sCard On Customer.CustomerID=sCard.CustomerID " +
                " Where 1=1";

            Int32 iID = POS_Core.Resources.Configuration.convertNullToInt(txtCode.Text);

            if (string.IsNullOrEmpty(txtCode.Text.Trim()) == false && bSearchExactCustomer == true)
            {
                sSQL += " And AcctNumber = '" + iID.ToString() + "'";
                return sSQL;
            }

            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {

                if (clsUIHelper.isNumeric(this.txtMasterSearchVal.Text) == true)
                {
                    sSQL1 += sSQL + " AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + " LIKE (" + "'" + this.txtMasterSearchVal.Text + "%') UNION ";
                    sAcctNoClause = " AND ";
                }
                else
                {
                    sAcctNoClause = "AND " + clsPOSDBConstants.Customer_Fld_AcctNumber + "  LIKE (" + "'" + this.txtMasterSearchVal.Text + "%') OR   ";
                }

                sSQL += sAcctNoClause
                 + clsPOSDBConstants.Customer_Fld_CustomerName + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_FirstName + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Address1 + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Address2 + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Zip + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_City + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_CellNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_PhoneOffice + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Comments + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_FaxNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_DriveLicNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                //+ " CONVERT(VARCHAR(25)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126)" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_DriveLicState + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_PatientNo + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_PhoneHome + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')OR   "
                 + clsPOSDBConstants.Customer_Fld_Email + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%')  ";


                if (this.txtMasterSearchVal.Text != "-1")
                {
                    sSQL += "  OR CONVERT(VARCHAR(25)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126)" + " LIKE (" + "'%" + this.txtMasterSearchVal.Text + "%') ";
                }
                else
                {
                    sSQL += "  OR CONVERT(VARCHAR(25)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126)" + " LIKE (" + "'%%') ";
                }

            }
            else
            {
                if (string.IsNullOrEmpty(txtCode.Text.Trim()) == false && bSearchExactCustomer == false)
                {
                    sSQL += " And AcctNumber LIKE (" + "'" + iID.ToString() + "%')";
                }

                if (string.IsNullOrEmpty(txtName.Text.Trim()) == false)
                {
                    string lName = "";
                    string fName = "";
                    if (txtName.Text.Trim().Contains(","))
                    {
                        char[] seperater = { ',' };
                        string[] lFname = txtName.Text.Split(seperater);
                        lName = lFname[0].Trim();
                        if (lFname.Length > 1)
                            fName = lFname[1].Trim();
                    }
                    else
                    {
                        lName = txtName.Text.Trim();
                    }

                    sSQL += " And CustomerName like '" + lName + "%' and +FirstName like '" + fName + "%'";
                }
                if (txtContactNumber.Text.Trim() != "")
                {
                    sSQL += " And ( CellNo LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                    sSQL += " Or  PhoneOff LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%')";
                    sSQL += " Or  PhoneHome LIKE ('" + txtContactNumber.Text.Replace("'", "''") + "%'))";
                }

                if (ActiveOnly == 1)
                {
                    sSQL += " And Customer.isActive=1 ";
                }
            }


            if (string.IsNullOrEmpty(this.txtName.Text) == false)
            {
                this.txtName.Text = this.txtName.Text.Replace("''", "'");
            }
            if (string.IsNullOrEmpty(this.txtMasterSearchVal.Text) == false)
            {
                this.txtMasterSearchVal.Text = this.txtMasterSearchVal.Text.Replace("''", "'");
            }
            if (string.IsNullOrEmpty(this.txtCode.Text) == false)
            {
                this.txtCode.Text = this.txtCode.Text.Replace("''", "'");
            }
            //End 
            if (string.IsNullOrEmpty(sSQL1) == false)
            {
                return (sSQL1 + sSQL);
            }
            return sSQL;
        }
        #endregion
    }
}
