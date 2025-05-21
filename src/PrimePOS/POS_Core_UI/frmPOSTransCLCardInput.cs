using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core.BusinessRules;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPOSChangeDue.
	/// </summary>
	public class frmPOSTransCLCardInput : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox grpCLCard;
		public System.Windows.Forms.TextBox txtCardID;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnIssueNewCard;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraButton btnSearchByCustomer;
        private GroupBox grpCLTransPoints;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor txtTransPoints;

        private Int32 clTransPoints= 0;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCLCoupons;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCLPoints;
        public Infragistics.Win.Misc.UltraButton btnEditCustomer;
        public Infragistics.Win.Misc.UltraButton btnEditCLCard;
        private Infragistics.Win.Misc.UltraButton btnSearch;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        private bool allowedCLCardEdit = false;
        private bool allowedCustomerEdit = false;
        private CLCardsRow selectedCard = null;
        public Infragistics.Win.Misc.UltraLabel lblCustomerName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private CustomerRow selectedCustomer = null;
        private bool inViewMode = false;
        Customer oCustomer = new Customer();
        int customerAcctNo;
        bool isCloseButtonClicked = false;  //Sprint-23 - PRIMEPOS-2275 13-Jun-2016 JY Added 
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmPOSTransCLCardInput():this(false)
        {}

        public frmPOSTransCLCardInput(bool isViewMode):this(isViewMode,0)
        {}

        public frmPOSTransCLCardInput(bool isViewMode, int customerAcctNo)
		{
            inViewMode = isViewMode;
            this.customerAcctNo = customerAcctNo;
            InitializeComponent();
		}

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

        public POS_Core.CommonData.Rows.CLCardsRow CLCardRow
        {
            get { return selectedCard; }
        }

        public POS_Core.CommonData.Rows.CustomerRow CLCustomerRow
        {
            get { return selectedCustomer; }
        }

        public Int32 CLTransPoints
        {
            get { return clTransPoints; }
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
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value");
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSTransCLCardInput));
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Column Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Criteria Value", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            this.grpCLCard = new System.Windows.Forms.GroupBox();
            this.lblCustomerName = new Infragistics.Win.Misc.UltraLabel();
            this.btnEditCustomer = new Infragistics.Win.Misc.UltraButton();
            this.grpCLTransPoints = new System.Windows.Forms.GroupBox();
            this.txtTransPoints = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.txtCardID = new System.Windows.Forms.TextBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnEditCLCard = new Infragistics.Win.Misc.UltraButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.btnIssueNewCard = new Infragistics.Win.Misc.UltraButton();
            this.btnSearchByCustomer = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.grdCLCoupons = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grdCLPoints = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpCLCard.SuspendLayout();
            this.grpCLTransPoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransPoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCLCoupons)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCLPoints)).BeginInit();
            this.SuspendLayout();
            // 
            // grpCLCard
            // 
            this.grpCLCard.Controls.Add(this.lblCustomerName);
            this.grpCLCard.Controls.Add(this.btnEditCustomer);
            this.grpCLCard.Controls.Add(this.grpCLTransPoints);
            this.grpCLCard.Controls.Add(this.ultraLabel19);
            this.grpCLCard.Controls.Add(this.txtCardID);
            this.grpCLCard.Controls.Add(this.btnClose);
            this.grpCLCard.Controls.Add(this.btnEditCLCard);
            this.grpCLCard.Controls.Add(this.btnSearch);
            this.grpCLCard.Controls.Add(this.btnIssueNewCard);
            this.grpCLCard.Controls.Add(this.btnSearchByCustomer);
            this.grpCLCard.Controls.Add(this.ultraLabel2);
            this.grpCLCard.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpCLCard.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.grpCLCard.Location = new System.Drawing.Point(11, 3);
            this.grpCLCard.Name = "grpCLCard";
            this.grpCLCard.Size = new System.Drawing.Size(815, 141);
            this.grpCLCard.TabIndex = 0;
            this.grpCLCard.TabStop = false;
            // 
            // lblCustomerName
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BackColor2 = System.Drawing.Color.Transparent;
            appearance1.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.BorderColor = System.Drawing.Color.Silver;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Arial";
            appearance1.FontData.SizeInPoints = 14F;
            appearance1.ForeColor = System.Drawing.Color.Blue;
            appearance1.TextHAlignAsString = "Left";
            this.lblCustomerName.Appearance = appearance1;
            this.lblCustomerName.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblCustomerName.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCustomerName.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblCustomerName.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(100, 56);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(299, 27);
            this.lblCustomerName.TabIndex = 91;
            // 
            // btnEditCustomer
            // 
            this.btnEditCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.SystemColors.Control;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnEditCustomer.Appearance = appearance2;
            this.btnEditCustomer.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEditCustomer.HotTrackAppearance = appearance3;
            this.btnEditCustomer.Location = new System.Drawing.Point(610, 54);
            this.btnEditCustomer.Name = "btnEditCustomer";
            this.btnEditCustomer.Size = new System.Drawing.Size(199, 29);
            this.btnEditCustomer.TabIndex = 7;
            this.btnEditCustomer.TabStop = false;
            this.btnEditCustomer.Text = "Edit Customer (F7)";
            this.btnEditCustomer.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEditCustomer.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEditCustomer.Click += new System.EventHandler(this.btnEditCustomer_Click);
            // 
            // grpCLTransPoints
            // 
            this.grpCLTransPoints.Controls.Add(this.txtTransPoints);
            this.grpCLTransPoints.Controls.Add(this.ultraLabel3);
            this.grpCLTransPoints.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpCLTransPoints.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.grpCLTransPoints.Location = new System.Drawing.Point(6, 78);
            this.grpCLTransPoints.Name = "grpCLTransPoints";
            this.grpCLTransPoints.Size = new System.Drawing.Size(393, 52);
            this.grpCLTransPoints.TabIndex = 1;
            this.grpCLTransPoints.TabStop = false;
            // 
            // txtTransPoints
            // 
            appearance4.FontData.BoldAsString = "False";
            this.txtTransPoints.Appearance = appearance4;
            this.txtTransPoints.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransPoints.ButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.txtTransPoints.FormatString = "###################";
            this.txtTransPoints.Location = new System.Drawing.Point(215, 19);
            this.txtTransPoints.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtTransPoints.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.txtTransPoints.MaskInput = "nnnnn";
            this.txtTransPoints.MaxValue = 99999;
            this.txtTransPoints.MinValue = 0;
            this.txtTransPoints.Name = "txtTransPoints";
            this.txtTransPoints.NullText = "0";
            this.txtTransPoints.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtTransPoints.Size = new System.Drawing.Size(173, 24);
            this.txtTransPoints.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.txtTransPoints.TabIndex = 0;
            this.txtTransPoints.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtTransPoints.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtTransPoints.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransPoints_KeyDown);
            // 
            // ultraLabel3
            // 
            appearance5.FontData.Name = "Arial";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance5;
            this.ultraLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel3.Location = new System.Drawing.Point(3, 14);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(211, 35);
            this.ultraLabel3.TabIndex = 6;
            this.ultraLabel3.Text = "Please enter PointsAcquired for this transaction";
            // 
            // ultraLabel19
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BackColor2 = System.Drawing.Color.Transparent;
            appearance6.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "Arial";
            appearance6.FontData.SizeInPoints = 11F;
            appearance6.ForeColor = System.Drawing.Color.Maroon;
            this.ultraLabel19.Appearance = appearance6;
            this.ultraLabel19.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel19.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel19.Location = new System.Drawing.Point(6, 59);
            this.ultraLabel19.Name = "ultraLabel19";
            this.ultraLabel19.Size = new System.Drawing.Size(105, 21);
            this.ultraLabel19.TabIndex = 90;
            this.ultraLabel19.Text = "Customer :";
            // 
            // txtCardID
            // 
            this.txtCardID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCardID.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardID.Location = new System.Drawing.Point(218, 21);
            this.txtCardID.Name = "txtCardID";
            this.txtCardID.Size = new System.Drawing.Size(181, 24);
            this.txtCardID.TabIndex = 0;
            this.txtCardID.TextChanged += new System.EventHandler(this.txtCardID_TextChanged);
            this.txtCardID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCardID_KeyDown);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            this.btnClose.Appearance = appearance7;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.Location = new System.Drawing.Point(610, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(199, 29);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Continue";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEditCLCard
            // 
            this.btnEditCLCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.SystemColors.Control;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.btnEditCLCard.Appearance = appearance8;
            this.btnEditCLCard.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnEditCLCard.HotTrackAppearance = appearance9;
            this.btnEditCLCard.Location = new System.Drawing.Point(611, 96);
            this.btnEditCLCard.Name = "btnEditCLCard";
            this.btnEditCLCard.Size = new System.Drawing.Size(199, 29);
            this.btnEditCLCard.TabIndex = 6;
            this.btnEditCLCard.TabStop = false;
            this.btnEditCLCard.Text = "Edit CL Card (F6)";
            this.btnEditCLCard.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnEditCLCard.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnEditCLCard.Click += new System.EventHandler(this.btnEditCLCard_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.SystemColors.Control;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance10.FontData.BoldAsString = "True";
            appearance10.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Appearance = appearance10;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            this.btnSearch.HotTrackAppearance = appearance11;
            this.btnSearch.Location = new System.Drawing.Point(405, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(199, 29);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.TabStop = false;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnIssueNewCard
            // 
            this.btnIssueNewCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            this.btnIssueNewCard.Appearance = appearance12;
            this.btnIssueNewCard.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnIssueNewCard.Location = new System.Drawing.Point(406, 96);
            this.btnIssueNewCard.Name = "btnIssueNewCard";
            this.btnIssueNewCard.Size = new System.Drawing.Size(199, 29);
            this.btnIssueNewCard.TabIndex = 5;
            this.btnIssueNewCard.TabStop = false;
            this.btnIssueNewCard.Text = "&Issue New Card (F2)";
            this.btnIssueNewCard.Click += new System.EventHandler(this.btnIssueNewCard_Click);
            // 
            // btnSearchByCustomer
            // 
            appearance13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            this.btnSearchByCustomer.Appearance = appearance13;
            this.btnSearchByCustomer.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearchByCustomer.Location = new System.Drawing.Point(405, 54);
            this.btnSearchByCustomer.Name = "btnSearchByCustomer";
            this.btnSearchByCustomer.Size = new System.Drawing.Size(199, 29);
            this.btnSearchByCustomer.TabIndex = 2;
            this.btnSearchByCustomer.TabStop = false;
            this.btnSearchByCustomer.Text = "&Search By Customer (F4)";
            this.btnSearchByCustomer.Click += new System.EventHandler(this.btnSearchByCustomer_Click);
            // 
            // ultraLabel2
            // 
            appearance14.FontData.Name = "Arial";
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance14;
            this.ultraLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(6, 16);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(194, 35);
            this.ultraLabel2.TabIndex = 6;
            this.ultraLabel2.Text = "Please provide Customer Loyalty Card number:";
            // 
            // grdCLCoupons
            // 
            this.grdCLCoupons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BackColor2 = System.Drawing.Color.White;
            appearance15.BackColorDisabled = System.Drawing.Color.White;
            appearance15.BackColorDisabled2 = System.Drawing.Color.White;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance15.TextHAlignAsString = "Left";
            this.grdCLCoupons.DisplayLayout.Appearance = appearance15;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            appearance16.FontData.BoldAsString = "True";
            ultraGridBand1.Header.Appearance = appearance16;
            this.grdCLCoupons.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCLCoupons.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.FontData.BoldAsString = "True";
            appearance17.FontData.SizeInPoints = 9F;
            appearance17.ForeColor = System.Drawing.Color.Black;
            this.grdCLCoupons.DisplayLayout.CaptionAppearance = appearance17;
            this.grdCLCoupons.DisplayLayout.InterBandSpacing = 10;
            this.grdCLCoupons.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCLCoupons.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.White;
            this.grdCLCoupons.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance18;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.White;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.ActiveRowAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.White;
            appearance20.BackColor2 = System.Drawing.Color.White;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.AddRowAppearance = appearance20;
            this.grdCLCoupons.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdCLCoupons.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCLCoupons.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance21.BackColor = System.Drawing.Color.Transparent;
            this.grdCLCoupons.DisplayLayout.Override.CardAreaAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.White;
            appearance22.BackColor2 = System.Drawing.Color.White;
            appearance22.BackColorDisabled = System.Drawing.Color.White;
            appearance22.BackColorDisabled2 = System.Drawing.Color.White;
            appearance22.BorderColor = System.Drawing.Color.Black;
            appearance22.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdCLCoupons.DisplayLayout.Override.CellAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance23.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            appearance23.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance23.Image = ((object)(resources.GetObject("appearance23.Image")));
            appearance23.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance23.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdCLCoupons.DisplayLayout.Override.CellButtonAppearance = appearance23;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdCLCoupons.DisplayLayout.Override.EditCellAppearance = appearance24;
            appearance25.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.FilteredInRowAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.FilteredOutRowAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BackColor2 = System.Drawing.Color.White;
            appearance27.BackColorDisabled = System.Drawing.Color.White;
            appearance27.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdCLCoupons.DisplayLayout.Override.FixedCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance28.BackColor2 = System.Drawing.Color.Beige;
            this.grdCLCoupons.DisplayLayout.Override.FixedHeaderAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.SystemColors.Control;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance29.FontData.BoldAsString = "True";
            appearance29.FontData.SizeInPoints = 9F;
            appearance29.ForeColor = System.Drawing.Color.Black;
            appearance29.TextHAlignAsString = "Left";
            appearance29.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdCLCoupons.DisplayLayout.Override.HeaderAppearance = appearance29;
            this.grdCLCoupons.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance30.BackColor = System.Drawing.Color.LightCyan;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.RowAlternateAppearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.White;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance31.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.RowAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.RowPreviewAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.SystemColors.Control;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.RowSelectorAppearance = appearance33;
            this.grdCLCoupons.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdCLCoupons.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance34.BackColor = System.Drawing.Color.Navy;
            appearance34.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdCLCoupons.DisplayLayout.Override.SelectedCellAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.Navy;
            appearance35.BackColorDisabled = System.Drawing.Color.Navy;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance35.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance35.BorderColor = System.Drawing.Color.Gray;
            appearance35.ForeColor = System.Drawing.Color.Black;
            this.grdCLCoupons.DisplayLayout.Override.SelectedRowAppearance = appearance35;
            this.grdCLCoupons.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCLCoupons.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCLCoupons.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdCLCoupons.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.grdCLCoupons.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdCLCoupons.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.SystemColors.Control;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook1.Appearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.SystemColors.Control;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook1.ThumbAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.White;
            appearance40.BackColor2 = System.Drawing.Color.White;
            scrollBarLook1.TrackAppearance = appearance40;
            this.grdCLCoupons.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdCLCoupons.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.None;
            this.grdCLCoupons.Font = new System.Drawing.Font("Arial", 8F);
            this.grdCLCoupons.Location = new System.Drawing.Point(401, 150);
            this.grdCLCoupons.Name = "grdCLCoupons";
            this.grdCLCoupons.Size = new System.Drawing.Size(428, 265);
            this.grdCLCoupons.TabIndex = 3;
            this.grdCLCoupons.TabStop = false;
            this.grdCLCoupons.Text = "# of available coupons";
            this.grdCLCoupons.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdCLCoupons.Visible = false;
            // 
            // grdCLPoints
            // 
            this.grdCLPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance41.BackColor = System.Drawing.Color.White;
            appearance41.BackColor2 = System.Drawing.Color.White;
            appearance41.BackColorDisabled = System.Drawing.Color.White;
            appearance41.BackColorDisabled2 = System.Drawing.Color.White;
            appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance41.TextHAlignAsString = "Left";
            this.grdCLPoints.DisplayLayout.Appearance = appearance41;
            ultraGridColumn4.Header.VisiblePosition = 0;
            ultraGridColumn5.Header.VisiblePosition = 1;
            ultraGridColumn6.Header.VisiblePosition = 2;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            appearance42.FontData.BoldAsString = "True";
            ultraGridBand2.Header.Appearance = appearance42;
            this.grdCLPoints.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdCLPoints.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.FontData.BoldAsString = "True";
            appearance43.FontData.SizeInPoints = 9F;
            appearance43.ForeColor = System.Drawing.Color.Black;
            this.grdCLPoints.DisplayLayout.CaptionAppearance = appearance43;
            this.grdCLPoints.DisplayLayout.InterBandSpacing = 10;
            this.grdCLPoints.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCLPoints.DisplayLayout.MaxRowScrollRegions = 1;
            appearance44.BackColor = System.Drawing.Color.White;
            appearance44.BackColor2 = System.Drawing.Color.White;
            this.grdCLPoints.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance44;
            appearance45.BackColor = System.Drawing.Color.White;
            appearance45.BackColor2 = System.Drawing.Color.White;
            appearance45.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.ActiveRowAppearance = appearance45;
            appearance46.BackColor = System.Drawing.Color.White;
            appearance46.BackColor2 = System.Drawing.Color.White;
            appearance46.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.AddRowAppearance = appearance46;
            this.grdCLPoints.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdCLPoints.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCLPoints.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance47.BackColor = System.Drawing.Color.Transparent;
            this.grdCLPoints.DisplayLayout.Override.CardAreaAppearance = appearance47;
            appearance48.BackColor = System.Drawing.Color.White;
            appearance48.BackColor2 = System.Drawing.Color.White;
            appearance48.BackColorDisabled = System.Drawing.Color.White;
            appearance48.BackColorDisabled2 = System.Drawing.Color.White;
            appearance48.BorderColor = System.Drawing.Color.Black;
            appearance48.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdCLPoints.DisplayLayout.Override.CellAppearance = appearance48;
            appearance49.BackColor = System.Drawing.Color.White;
            appearance49.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance49.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance49.BorderColor = System.Drawing.Color.Gray;
            appearance49.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance49.Image = ((object)(resources.GetObject("appearance49.Image")));
            appearance49.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance49.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdCLPoints.DisplayLayout.Override.CellButtonAppearance = appearance49;
            appearance50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance50.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdCLPoints.DisplayLayout.Override.EditCellAppearance = appearance50;
            appearance51.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.FilteredInRowAppearance = appearance51;
            appearance52.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.FilteredOutRowAppearance = appearance52;
            appearance53.BackColor = System.Drawing.Color.White;
            appearance53.BackColor2 = System.Drawing.Color.White;
            appearance53.BackColorDisabled = System.Drawing.Color.White;
            appearance53.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdCLPoints.DisplayLayout.Override.FixedCellAppearance = appearance53;
            appearance54.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance54.BackColor2 = System.Drawing.Color.Beige;
            this.grdCLPoints.DisplayLayout.Override.FixedHeaderAppearance = appearance54;
            appearance55.BackColor = System.Drawing.Color.White;
            appearance55.BackColor2 = System.Drawing.SystemColors.Control;
            appearance55.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance55.FontData.BoldAsString = "True";
            appearance55.FontData.SizeInPoints = 9F;
            appearance55.ForeColor = System.Drawing.Color.Black;
            appearance55.TextHAlignAsString = "Left";
            appearance55.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdCLPoints.DisplayLayout.Override.HeaderAppearance = appearance55;
            this.grdCLPoints.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance56.BackColor = System.Drawing.Color.LightCyan;
            appearance56.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.RowAlternateAppearance = appearance56;
            appearance57.BackColor = System.Drawing.Color.White;
            appearance57.BackColor2 = System.Drawing.Color.White;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance57.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance57.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.RowAppearance = appearance57;
            appearance58.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.RowPreviewAppearance = appearance58;
            appearance59.BackColor = System.Drawing.Color.White;
            appearance59.BackColor2 = System.Drawing.SystemColors.Control;
            appearance59.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance59.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.RowSelectorAppearance = appearance59;
            this.grdCLPoints.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdCLPoints.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance60.BackColor = System.Drawing.Color.Navy;
            appearance60.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdCLPoints.DisplayLayout.Override.SelectedCellAppearance = appearance60;
            appearance61.BackColor = System.Drawing.Color.Navy;
            appearance61.BackColorDisabled = System.Drawing.Color.Navy;
            appearance61.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance61.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance61.BorderColor = System.Drawing.Color.Gray;
            appearance61.ForeColor = System.Drawing.Color.Black;
            this.grdCLPoints.DisplayLayout.Override.SelectedRowAppearance = appearance61;
            this.grdCLPoints.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCLPoints.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCLPoints.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            appearance62.BorderColor = System.Drawing.Color.Gray;
            this.grdCLPoints.DisplayLayout.Override.TemplateAddRowAppearance = appearance62;
            this.grdCLPoints.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdCLPoints.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance63.BackColor = System.Drawing.Color.White;
            appearance63.BackColor2 = System.Drawing.SystemColors.Control;
            appearance63.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            scrollBarLook2.Appearance = appearance63;
            appearance64.BackColor = System.Drawing.Color.White;
            appearance64.BackColor2 = System.Drawing.SystemColors.Control;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance64.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook2.ButtonAppearance = appearance64;
            appearance65.BackColor = System.Drawing.Color.White;
            appearance65.BackColor2 = System.Drawing.SystemColors.Control;
            scrollBarLook2.ThumbAppearance = appearance65;
            appearance66.BackColor = System.Drawing.Color.White;
            appearance66.BackColor2 = System.Drawing.Color.White;
            scrollBarLook2.TrackAppearance = appearance66;
            this.grdCLPoints.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdCLPoints.DisplayLayout.Scrollbars = Infragistics.Win.UltraWinGrid.Scrollbars.None;
            this.grdCLPoints.Font = new System.Drawing.Font("Arial", 8F);
            this.grdCLPoints.Location = new System.Drawing.Point(10, 150);
            this.grdCLPoints.Name = "grdCLPoints";
            this.grdCLPoints.Size = new System.Drawing.Size(385, 265);
            this.grdCLPoints.TabIndex = 2;
            this.grdCLPoints.TabStop = false;
            this.grdCLPoints.Text = "Current Points";
            this.grdCLPoints.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdCLPoints.Visible = false;
            // 
            // frmPOSTransCLCardInput
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(836, 427);
            this.Controls.Add(this.grdCLCoupons);
            this.Controls.Add(this.grdCLPoints);
            this.Controls.Add(this.grpCLCard);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSTransCLCardInput";
            this.ShowInTaskbar = false;
            this.Text = "Customer Loyalty Program Card Information";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPOSTransCLCardInput_FormClosing);
            this.Load += new System.EventHandler(this.frmPOSChangeDue_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPOSPayAuthNo_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmPOSPayAuthNo_KeyUp);
            this.grpCLCard.ResumeLayout(false);
            this.grpCLCard.PerformLayout();
            this.grpCLTransPoints.ResumeLayout(false);
            this.grpCLTransPoints.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransPoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCLCoupons)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCLPoints)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private void frmPOSChangeDue_Load(object sender, System.EventArgs e)
        {
            this.Left = 100;		//( frmMain.getInstance().Width-this.Width)/2;
            this.Top = 100;			//(frmMain.getInstance().Height-this.Height)/2;
            clsUIHelper.setColorSchecme(this);

            try
            {
                clsUIHelper.SetAppearance(this.grdCLPoints);
                clsUIHelper.SetReadonlyRow(this.grdCLPoints);

                clsUIHelper.SetAppearance(this.grdCLCoupons);
                clsUIHelper.SetReadonlyRow(this.grdCLCoupons);

                grpCLTransPoints.Enabled = Configuration.CLoyaltyInfo.DisableAutoPointCalc && inViewMode==false;
                if (inViewMode)
                {
                    btnClose.Text = "&Close";
                }

                if (customerAcctNo > 0)
                {
                    SearchCardByCustomer();
                    customerAcctNo = 0;
                }

                if (txtCardID.Text.Trim().Length == 0)
                {
                    Search();
                }
                this.txtCardID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtCardID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.txtTransPoints.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.txtTransPoints.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                this.ActiveControl = this.txtCardID;
                clsUIHelper.setColorSchecme(this);

                EnableOrDisable();

                allowedCLCardEdit = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID, 0);
                allowedCustomerEdit = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.Customers.ID, 0);
                this.btnIssueNewCard.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID, -998);
                
                if (Configuration.CLoyaltyInfo.ShowCLControlPane==false || this.CLCardRow == null)
                {
                    this.Height = this.grdCLCoupons.Top + 30;
                    this.grdCLCoupons.Visible = false;
                    this.grdCLPoints.Visible = false;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmPOSChangeDue_Load()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            this.txtCardID.Focus();
        }

        public void btnClose_Click(object sender, System.EventArgs e)   //13-Apr-2015 JY Changed private to public
        {
            isCloseButtonClicked = true;  //Sprint-23 - PRIMEPOS-2275 13-Jun-2016 JY Added 
            processCard();
        }

        private void processCard()
        {
            processCard(false);
        }

        private void processCard(bool isCancelled)
        {
            logger.Trace("processCard(bool isCancelled) - " + clsPOSDBConstants.Log_Entering);

            if (inViewMode)
            {
                //selectedCard = null;    //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added //Sprint-23 - PRIMEPOS-2275 05-Aug-2016 JY Commented - After implementation of changes of this This ticket, the "Show Card Input Screen" settings functionality was disturbed, in fact I'm not aware about this settings. Now corrected the behavior of this settings which is used to manually select the CL card by using the "Customer Loyalty" button.
                logger.Trace("processCard(bool isCancelled) - " + clsPOSDBConstants.Log_Exiting);
                this.Close();
                inViewMode = false; //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added 
            }
            else
            {
                if (selectedCard != null && isCancelled==false)
                {
                    clTransPoints = Configuration.convertNullToInt(txtTransPoints.Value);
                    this.DialogResult = DialogResult.OK;
                    isCloseButtonClicked = true;    //Sprint-23 - PRIMEPOS-2275 13-Jun-2016 JY Added 
                    logger.Trace("processCard(bool isCancelled) - " + clsPOSDBConstants.Log_Exiting);
                    this.Close();
                }
                else
                {
                    if (Resources.Message.Display("You have not selected Loyalty Card. Do you want to leave it empty?", "Loyalty Card", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        selectedCard = null;
                        selectedCustomer = null;
                        this.clTransPoints = 0;

                        this.DialogResult = DialogResult.OK;
                        isCloseButtonClicked = true;    //Sprint-23 - PRIMEPOS-2275 13-Jun-2016 JY Added 
                        logger.Trace("processCard(bool isCancelled) - " + clsPOSDBConstants.Log_Exiting);
                        this.Close();
                    }
                    else
                    {
                        logger.Trace("processCard(bool isCancelled) - " + clsPOSDBConstants.Log_Exiting);
                        txtCardID.Focus();
                    }
                }
            }
        }

		private void frmPOSPayAuthNo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if ( e.Alt==true || e.Control==true)
			{
				e.Handled=true;
			}
            else if (e.KeyData == Keys.Escape)
            {
                inViewMode = true; //Sprint-23 - PRIMEPOS-2275 03-Jun-2016 JY Added 
                processCard(true);
                e.Handled = true;
            }
            
		}

        //Added this function to ensure the AUTNO dialog is getting closed.    
        private void frmPOSPayAuthNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (grpCLCard.Enabled == false) return;
            if (e.KeyData == Keys.F2)
            {
                IssueNewCard();
            }
            else if (e.KeyData == Keys.F4)
            {
                SearchCardByCustomer();
            }
            else if (e.KeyData == Keys.F6)
            {
                EditCLCards();
            }
            else if (e.KeyData == Keys.F7)
            {
                EditCustomer();
            }

        }

        private void btnIssueNewCard_Click(object sender, EventArgs e)
        {
            IssueNewCard();
        }

        private void IssueNewCard()
        {
            if (this.btnIssueNewCard.Visible == false)
            {
                return;
            }

            IssueNewCard( SearchCustomer());
        }

        private void IssueNewCard(string customerCode)
        {
            try
            {
                logger.Trace("IssueNewCard(string customerCode) - " + clsPOSDBConstants.Log_Entering);

                if (customerCode.Trim().Length == 0)
                {
                    return;
                }

                #region Sprint-19 - 15-May-2015 JY Added to pop up warning if customer already have active card
                try
                {
                    if (this.txtCardID.Text.Trim() != string.Empty)
                    {
                        CLCards oCLCards = new CLCards();
                        CLCardsData oCLData = null;
                        oCLData = oCLCards.GetByCustomerID(Configuration.convertNullToInt(customerCode));
                        if (oCLData.CLCards.Rows.Count > 0)
                        {
                            Customer oCustomer = new Customer();
                            CustomerData oCustomerData;
                            oCustomerData = oCustomer.Populate(customerCode, true);
                            string strCustName = oCustomerData.Customer[0].CustomerName;
                            string strCards = string.Empty;
                            foreach (CLCardsRow row in oCLData.CLCards.Rows)
                            {
                                if (strCards == string.Empty)
                                    strCards = row.CLCardID.ToString();
                                else
                                    strCards += ",\n" + row.CLCardID.ToString();
                            }
                            if (Resources.Message.Display("Selected customer " + strCustName + " already has these loyalty cards:\n" + strCards + "\nDo you wish to continue?",
                            "Issue New Card", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                            {
                                return;
                            }
                        }
                    }
                }
                catch(Exception Ex)
                {
                    logger.Fatal(Ex, "IssueNewCard()");
                }
                #endregion

                frmCLCards ofrmCLCards = new frmCLCards();
                ofrmCLCards.Initialize(customerCode, this.txtCardID.Text);//Addded by shitaljit for JIRA-357 on jan 15 2013
                ofrmCLCards.ShowDialog(this);
                if (!ofrmCLCards.IsCanceled)
                {
                    this.txtCardID.Text = ofrmCLCards.txtCLCardID.Value.ToString();
                    Search();
                }
                logger.Trace("IssueNewCard(string customerCode) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IssueNewCard()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        
        private string SearchCustomer()
        {
            //frmCustomerSearch oSearch = new frmCustomerSearch(string.Empty);
            frmSearchMain oSearch = new frmSearchMain(string.Empty, true, clsPOSDBConstants.Customer_tbl);  //18-Dec-2017 JY Added new reference
            oSearch.ActiveOnly = 1;
            //Commented by shitaljit shitaljit to let users search PrimeRx Patients too
            //oSearch.chkIncludeRXCust.Visible = false;
            //oSearch.chkIncludeRXCust.Checked = false;
            oSearch.ShowDialog(this);
            if (!oSearch.IsCanceled)
            {
                //Added By shitaljit to add customer to DB if it is a customer from PrimeRx that is not exist in POS currently.
                CustomerRow oCustRow = oSearch.SelectedRow();
                return oCustomer.ImportNewCust(oSearch.SelectedRowID(), oCustRow);
                //End of added By Shitaljit.
                //return oSearch.SelectedRowID();

            }
            else
            {
                return string.Empty;
            }
        }

        private void btnSearchByCustomer_Click(object sender, EventArgs e)
        {
            SearchCardByCustomer();
        }

        private void SearchCardByCustomer()  
        {
            logger.Trace("SearchCardByCustomer() - " + clsPOSDBConstants.Log_Entering);

            //frmCustomerSearch oSearch;
            frmSearchMain oSearch;  //18-Dec-2017 JY Added new reference
            if (customerAcctNo > 0)
            {
                //oSearch = new frmCustomerSearch(customerAcctNo.ToString(), true, true,true,true);
                oSearch = new frmSearchMain(customerAcctNo.ToString(), true, true, true, true, true, clsPOSDBConstants.Customer_tbl); //18-Dec-2017 JY Added new reference
            }
            else
            {
                if (txtCardID.Text.EndsWith("/")) //search by customer contact#
                {
                    //oSearch = new frmCustomerSearch(txtCardID.Text, true, false);
                    //oSearch = new frmCustomerSearch(txtCardID.Text, true, false, true, true);
                    oSearch = new frmSearchMain(txtCardID.Text, true, false, true, true,true, clsPOSDBConstants.Customer_tbl);  //18-Dec-2017 JY Added new reference
                }
                else
                {
                    //oSearch = new frmCustomerSearch(string.Empty, true, false);
                    //oSearch = new frmCustomerSearch(string.Empty, true, false, true, true);
                    oSearch = new frmSearchMain(string.Empty, true, false, true, true, true, clsPOSDBConstants.Customer_tbl); //18-Dec-2017 JY Added new reference
                }
            }
            //oSearch.SearchTable = clsPOSDBConstants.Customer_tbl;   //18-Dec-2017 JY Added new reference
            oSearch.ActiveOnly = 1;
            oSearch.ShowDialog(this);
            if (!oSearch.IsCanceled)
            {
                txtCardID.Text = oSearch.SelectedCLPCardID();
                if (txtCardID.Text.Trim().Length == 0)
                {
                    if (Resources.Message.Display("Selected customer does not have a Loyalty card assigned. \nDo you want to assign a new card?", Configuration.CLoyaltyInfo.ProgramName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //Added By Shitalit to check whether cust is exist in POSSQL if nnot import the cust
                        CustomerRow oCustRow = oSearch.SelectedRow();
                       string strCustCode = oCustomer.ImportNewCust(oSearch.SelectedRowID(), oCustRow);
                       IssueNewCard(strCustCode);
                       // IssueNewCard(oSearch.SelectedRowID());
                    }
                }
            }
            else
            {
                txtCardID.Text = string.Empty;
                selectedCustomer = null;
                selectedCard = null;
            }
            Search();
            txtCardID.Focus();
            logger.Trace("SearchCardByCustomer() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void txtCardID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtCardID.Text.Trim() != "")
                {
                    if (txtCardID.Text.EndsWith("/"))
                    {
                        SearchCardByCustomer();
                    }
                    else
                    {
                        Search();
                    }
                }
                else
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
        }

        private void txtTransPoints_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnClose.Focus();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            logger.Trace("Search() - " + clsPOSDBConstants.Log_Entering);

            this.grdCLCoupons.Text = "# of available coupons :0";
            this.grdCLPoints.Text = "Current Point:0";
            this.lblCustomerName.Text = string.Empty;

            PopulateCustomerAndCard();
            if (selectedCard != null)
            {
                if (Configuration.CLoyaltyInfo.ShowCLControlPane == true)
                {
                    this.Height =465;
                    this.grdCLCoupons.Visible = true;
                    this.grdCLCoupons.Visible = true;
                    this.grdCLPoints.Visible = true;
                }
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
            PopulateCurrentPointsLogGrid();   
            PopulateCouponsStatusGrid();

            EnableOrDisable();

            if (selectedCustomer != null)
            {
                lblCustomerName.Text = selectedCustomer.CustomerFullName;
            }
            logger.Trace("Search() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void PopulateCustomerAndCard()
        {
            logger.Trace("PopulateCustomerAndCard() - " + clsPOSDBConstants.Log_Entering);

            FillCLCardRow();
            if (selectedCard != null)
            {
                CustomerData oCData = (new CustomerSvr()).GetCustomerByID(selectedCard.CustomerID);
                if (oCData != null && oCData.Customer.Count > 0)
                {
                    selectedCustomer = oCData.Customer[0];
                }
                else
                {
                    selectedCustomer = null;
                }
            }
            logger.Trace("PopulateCustomerAndCard() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void FillCLCardRow()
        {
            logger.Trace("FillCLCardRow() - " + clsPOSDBConstants.Log_Entering);
            if (txtCardID.Text.Trim().Length == 0)
            {
                selectedCard = null;
            }
            else
            {
                CLCardsSvr oSvr = new CLCardsSvr();
                CLCardsData oCards = oSvr.GetByCLCardID(Configuration.convertNullToInt64(txtCardID.Text));
                if (oCards == null || oCards.CLCards.Rows.Count == 0)
                {
                    if (Resources.Message.Display("Entered card# is not valid.\nDo you want to assign a new card?", "Invalid CL Card", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        IssueNewCard(SearchCustomer());
                    }
                    else
                    {
                        txtCardID.Text = string.Empty;
                        selectedCard = null;
                        this.lblCustomerName.Text = "";
                        selectedCard = null;
                        selectedCustomer = null;
                    }
                }
                else
                {
                    selectedCard = oCards.CLCards[0];
                    if (selectedCard.IsPrepetual == false && selectedCard.RegisterDate.AddDays(selectedCard.ExpiryDays) < DateTime.Now)
                    {
                        clsUIHelper.ShowErrorMsg("Card is expired.");
                        txtCardID.Text = string.Empty;
                        selectedCard = null;
                    }
                }
            }
            logger.Trace("FillCLCardRow() - " + clsPOSDBConstants.Log_Exiting);
        }

        private void EnableOrDisable()
        {
            btnEditCLCard.Enabled = (selectedCard != null && UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.CustomerLoyaltyCards.ID, -998));
            btnEditCustomer.Enabled = (selectedCustomer != null && UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -998));
        }

        private void EditCustomer()
        {
            if (selectedCustomer == null) return;
            logger.Trace("EditCustomer() - " + clsPOSDBConstants.Log_Entering);
            try
            {
                frmCustomers oCustomer = new frmCustomers();
                oCustomer.Edit(selectedCustomer.CustomerId.ToString());
                oCustomer.ShowDialog(this);
                if (!oCustomer.IsCanceled)
                    Search();
                logger.Trace("EditCustomer() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditCustomer()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void EditCLCards()
        {
            if (selectedCard == null) return;
            logger.Trace("EditCLCards() - " + clsPOSDBConstants.Log_Entering);
            try
            {
                frmCLCards ofrmCLCards = new frmCLCards();
                ofrmCLCards.Edit(selectedCard.CLCardID);
                ofrmCLCards.ShowDialog(this);
                if (!ofrmCLCards.IsCanceled)
                    Search();
                logger.Trace("EditCLCards() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "EditCLCards()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void resizeColumns(UltraGrid grdData)
        {
            foreach (UltraGridColumn oCol in grdData.DisplayLayout.Bands[0].Columns)
            {
                oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }
        }

        private void btnEditCLCard_Click(object sender, EventArgs e)
        {
            EditCLCards();
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            EditCustomer();
        }

        #region Sprint-18 - 2090 10-Oct-2014 JY Commented below code block as ulternate logic implemented
        //private void PopulateCurrentPointsLogGrid()
        //{
        //    if (this.grdCLPoints.Visible==false) return;

        //    Int64 clCardId = -1;
        //    if (selectedCard != null)
        //    {
        //        clCardId = selectedCard.CLCardID;
        //    }

        //    String sql = "SELECT  pt.TransId as TransNo, pt.LoyaltyPoints as Points, pt.TransDate as TransDate " +
        //                ",'T' as RowType From postransaction pt, customer c, cl_cards cld " +
        //                "Where pt.customerid=c.customerid and c.customerid=cld.customerid " +
        //                "and cardid=" + clCardId + " and pt.LoyaltyPoints<>0 " +
        //                 "Union All " +
        //                "Select 0 as TransNo, NewPoints as Points, CreatedOn as TransDate,'A' as RowType  " +
        //                "From cl_PointsAdjustmentLog Where cardid=" + clCardId + " " +
        //                "Union All " +
        //                "SELECT coupon.UsedInTransId as TransNo,-1*coupon.Points as Points,coupon.CreatedOn as TransDate " +
        //                ",'U' as RowType From cl_Coupons coupon , postransaction pt " +
        //                "Where coupon.UsedInTransId=pt.transid and coupon.cardid=" + clCardId + " and coupon.isActive=1 " +
        //                "And coupon.IsCouponUsed=1 " +
        //                "Union All " +
        //                "SELECT coupon.CreatedInTransId as TransNo,-1*coupon.Points as Points,coupon.CreatedOn as TransDate  " +
        //                ",'X' as RowType From cl_Coupons coupon  " +
        //                "Where cardid=" + clCardId + " and coupon.isActive=1 and coupon.IsCouponUsed=0 " +
        //                "And DateAdd(d,ExpiryDays,CreatedOn)<GetDate() order by TransDate";
            
        //    DataSet result = new Search().SearchData(sql);
        //    result.Tables[0].Columns.Add("Balance", typeof(decimal));
        //    result.Tables[0].Columns.Add("CurrentPT", typeof(decimal));
        //    result.Tables[0].Columns.Add("RowCount", typeof(int));
        //    decimal balance = 0;
        //    decimal curPt = 0;
        //    int rowCount = 0;
        //    //Added By Shitaljit on 1/15/2014 for PRIMEPOS-1728 CL points are not calculated properly in grid of Customer Loyalty Control Pane
        //    decimal totalUnusedPoints = 0;
        //    decimal totalUsedPoints = 0;
        //    foreach (DataRow row in result.Tables[0].Rows)
        //    {
        //        if (row["RowType"].ToString() == "A")
        //        {
        //            balance = 0;
        //            curPt = 0;
        //        }

        //        curPt += Configuration.convertNullToDecimal(row["Points"]);
        //        balance += Configuration.convertNullToDecimal(row["Points"]) < 0 ? 0 : Configuration.convertNullToDecimal(row["Points"]);
        //        rowCount += 1;
        //        row["Balance"] = balance;
        //        row["CurrentPT"] = curPt;
        //        row["RowCount"] = rowCount;

        //    }


        //    //Following code section is added By shitaljit 
        //    #region Corrent Total Points
        //    string sSql = @"SELECT  (SELECT ISNULL(SUM(CurrentPoints),0) From CL_Cards WHERE CardID = '" + clCardId + "' AND IsActive =1 " + ") AS CurrentUnusedPoints, " +
        //                  " (SELECT ISNULL(SUM(Points),0) from CL_Coupons WHERE  CardID = '" + clCardId + "'" + " AND isactive =1 " +
        //                  " AND IsCouponUsed <> 1 AND DateAdd(d,ExpiryDays,CreatedOn)>=GetDate() ) AS CurrentUsedPoints";
        //    DataSet dsTotalPoints = new Search().SearchData(sSql);
        //    if (Configuration.isNullOrEmptyDataSet(dsTotalPoints) == false)
        //    {
        //        totalUnusedPoints = Configuration.convertNullToDecimal(dsTotalPoints.Tables[0].Rows[0]["CurrentUnusedPoints"]);
        //        totalUsedPoints = Configuration.convertNullToDecimal(dsTotalPoints.Tables[0].Rows[0]["CurrentUsedPoints"]);
        //    }
        //    #endregion

        //    this.grdCLPoints.DataSource = result;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransNo"].Header.Caption = "Trans #";
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["CurrentPT"].Header.Caption = "Cur.PT";
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["Balance"].Header.Caption = "Balance";     
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Format = "MM/dd/yyyy";
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Header.VisiblePosition = 5;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Header.Caption = "Date";
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowType"].Header.Caption = "Type";
        //   // this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowCount"].Header.Caption = "Rows";
        //    //this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowType"].Hidden = true;
        //    //this.grdCLPoints.Text = "Current Point:" + balance.ToString(); Commented By shitaljit to fixe wrong total points displaying in grid
        //    this.grdCLPoints.Text = "                           Total available points: " + Configuration.convertNullToString(totalUnusedPoints) +
        //                            "\n                           Points from active coupon: " + Configuration.convertNullToString(totalUsedPoints);
        //    this.grdCLPoints.DisplayLayout.Scrollbars = Scrollbars.Vertical;
        //    this.resizeColumns(grdCLPoints);
        //    this.grdCLPoints.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransNo"].SortIndicator = SortIndicator.Disabled;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["Balance"].SortIndicator = SortIndicator.Disabled;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].SortIndicator = SortIndicator.Disabled;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["CurrentPT"].SortIndicator = SortIndicator.Disabled;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowType"].SortIndicator = SortIndicator.Disabled;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowCount"].SortIndicator = SortIndicator.Descending;
        //    this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowCount"].Hidden = true;

        //}
        #endregion

        #region Sprint-18 - 2090 10-Oct-2014 JY Added new logic to load point table
        private void PopulateCurrentPointsLogGrid()
        {
            if (this.grdCLPoints.Visible == false) return;

            logger.Trace("PopulateCurrentPointsLogGrid() - " + clsPOSDBConstants.Log_Entering);

            Int64 clCardId = -1;
            if (selectedCard != null)
            {
                clCardId = selectedCard.CLCardID;

                //Check whether card is old or new. If old then convert data into new logic and then populate elase populate directly
                Boolean bCardStatus = GetCardStatus(clCardId);

                if (!bCardStatus)
                {
                    Boolean bIsConvertedSuccessfully = CovertCLPointsData(clCardId);

                    if (bIsConvertedSuccessfully)
                    {
                        UpdateIsConvertedStatus(clCardId);
                    }
                }
                PopulateCLPointsFromTable(clCardId);
            }
            logger.Trace("PopulateCurrentPointsLogGrid() - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        #region Sprint-18 - 2090 20-Oct-2014 JY Check whether card is old or new. If old then convert data into new logic and then populate elase populate directly
        private bool GetCardStatus(Int64 CardID)
        {
            logger.Trace("GetCardStatus() - " + clsPOSDBConstants.Log_Entering);
            string strSQL = string.Empty, strResult = string.Empty;
            Boolean bCardStatus = false;
            try
            {
                strSQL = "SELECT IsConverted FROM CL_Cards WHERE CardID = " + CardID;
                strResult = new Search().SearchScalar(strSQL);

                if (strResult.ToUpper() == "FALSE" || strResult == "" || strResult == "0")
                    bCardStatus = false;
                else
                    bCardStatus = true;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetCardStatus()");
            }
            logger.Trace("GetCardStatus() - " + clsPOSDBConstants.Log_Exiting);
            return bCardStatus;
        }
        #endregion

        #region Sprint-18 - 2090 21-Oct-2014 JY Update IsConverted flag in cl_cards table
        private Boolean UpdateIsConvertedStatus(Int64 clCardId)
        {
            try
            {
                CLCardsSvr oSvr = new CLCardsSvr();
                return oSvr.UpdateIsConvertedStatus(clCardId);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateIsConvertedStatus()");
                throw (ex);
            }
        }
        #endregion

        #region Sprint-18 - 2090 20-Oct-2014 JY polulate CL points data from CL_TransDetail table
        private void PopulateCLPointsFromTable(Int64 clCardId)
        {
            logger.Trace("PopulateCLPointsFromTable(Int64 clCardId) - " + clsPOSDBConstants.Log_Entering);

            /*String sql = "select a.TransId AS TransNo, a.PointsAcquired AS Points, a.TransDate AS TransDate, " +
                        " CASE WHEN a.ActionType = 'A' THEN 'A' " +
                        " WHEN a.TransId = (SELECT TOP 1 ISNULL(b.CreatedInTransId,-1) From cl_Coupons b Where a.TransId = b.CreatedInTransId AND b.cardid=a.Cardid and b.isActive=1 and b.IsCouponUsed=0 And DateAdd(d,b.ExpiryDays,b.CreatedOn)<GetDate()) THEN 'X' ELSE 'T' END AS RowType, " +
                        " a.CurrentPoints AS Balance, a.RunningTotalPoints AS CurrentPT, 0 AS [RowCount] from CL_TransDetail a " +
                        " Where a.CardId = " + clCardId + 
                        " Union All " +
                        " SELECT coupon.UsedInTransId as TransNo, -1*coupon.Points as Points, coupon.CreatedOn as TransDate,'U' as RowType, 0 AS Balance, 0 AS CurrentPT, 0 AS [RowCount] From cl_Coupons coupon, postransaction pt " +
                        " Where coupon.UsedInTransId=pt.transid and coupon.cardid=" + clCardId + " and coupon.isActive=1 And coupon.IsCouponUsed=1 " +
                        " order by TransDate DESC";*/

            String sql = "select a.TransId AS TransNo, a.PointsAcquired AS [Points], a.TransDate AS TransDate, a.ActionType AS [Type], " +
                        " a.CurrentPoints AS [Bal Pts], a.RunningTotalPoints AS [Tot Pts] from CL_TransDetail a " +
                        " Where a.CardId = " + clCardId +
                        " Union All " +
                        " SELECT coupon.UsedInTransId as TransNo, -1*coupon.Points as PointsAcquired, coupon.CreatedOn as TransDate, 'U' as RowType, 0.00 AS CurrentPoints, 0.00 AS RunningTotalPoints From cl_Coupons coupon , postransaction pt " + 
                        " Where coupon.UsedInTransId=pt.transid and coupon.cardid= " + clCardId + " and coupon.isActive=0 " + 
                        " And coupon.IsCouponUsed=1 " + 
                        " Union All " +
                        " SELECT coupon.CreatedInTransId as TransNo, -1*coupon.Points as PointsAcquired, coupon.CreatedOn as TransDate, 'X' as RowType, 0.00 AS CurrentPoints, 0.00 AS RunningTotalPoints From cl_Coupons coupon " +
                        " Where cardid= " + clCardId + " and coupon.isActive=1 and coupon.IsCouponUsed=0 " + 
                        " And DateAdd(d,ExpiryDays,CreatedOn)<GetDate() " +
                        " order by TransDate DESC";

            DataSet result = new Search().SearchData(sql);

            decimal totalUnusedPoints = 0;
            decimal totalUsedPoints = 0;
            
            #region Corrent Total Points
            string sSql = @"SELECT  (SELECT ISNULL(SUM(CurrentPoints),0.00) From CL_Cards WHERE CardID = '" + clCardId + "' AND IsActive =1 " + ") AS CurrentUnusedPoints, " +
                          " (SELECT ISNULL(SUM(Points),0.00) from CL_Coupons WHERE  CardID = '" + clCardId + "'" + " AND isactive =1 " +
                          " AND IsCouponUsed <> 1 AND DateAdd(d,ExpiryDays,CreatedOn)>=GetDate() ) AS CurrentUsedPoints";
            DataSet dsTotalPoints = new Search().SearchData(sSql);
            if (Configuration.isNullOrEmptyDataSet(dsTotalPoints) == false)
            {
                totalUnusedPoints = Configuration.convertNullToDecimal(dsTotalPoints.Tables[0].Rows[0]["CurrentUnusedPoints"]);
                totalUsedPoints = Configuration.convertNullToDecimal(dsTotalPoints.Tables[0].Rows[0]["CurrentUsedPoints"]);
            }
            #endregion

            this.grdCLPoints.DataSource = result;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransNo"].Header.Caption = "Trans #";
            //this.grdCLPoints.DisplayLayout.Bands[0].Columns["CurrentPT"].Header.Caption = "Cur.PT";
            //this.grdCLPoints.DisplayLayout.Bands[0].Columns["Balance"].Header.Caption = "Balance";
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Format = "MM/dd/yyyy";
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Header.VisiblePosition = 5;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].Header.Caption = "Date";
            //this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowType"].Header.Caption = "Type";
            // this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowCount"].Header.Caption = "Rows";
            //this.grdCLPoints.DisplayLayout.Bands[0].Columns["RowType"].Hidden = true;
            //this.grdCLPoints.Text = "Current Point:" + balance.ToString(); Commented By shitaljit to fixe wrong total points displaying in grid
            //this.grdCLPoints.Text = "                           Total available points: " + Configuration.convertNullToString(totalUnusedPoints) +
            //                        "\n                           Points from active coupon: " + Configuration.convertNullToString(totalUsedPoints);

            this.grdCLPoints.Text = "Type: T-Trans, A-Adjustment, U-Used Coupon, X-Expired Coupon" +
                                    "\nBalance points: " + Configuration.convertNullToString(totalUnusedPoints) + "  And Active coupon points: " + Configuration.convertNullToString(totalUsedPoints);

            this.grdCLPoints.DisplayLayout.Scrollbars = Scrollbars.Vertical;
            this.resizeColumns(grdCLPoints);
            this.grdCLPoints.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransNo"].SortIndicator = SortIndicator.Disabled;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["Points"].SortIndicator = SortIndicator.Disabled;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["TransDate"].SortIndicator = SortIndicator.Disabled;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["Bal Pts"].SortIndicator = SortIndicator.Disabled;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["Type"].SortIndicator = SortIndicator.Disabled;
            this.grdCLPoints.DisplayLayout.Bands[0].Columns["Tot Pts"].SortIndicator = SortIndicator.Disabled;

            logger.Trace("PopulateCLPointsFromTable(Int64 clCardId) - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        #region Sprint-18 - 2090 20-Oct-2014 JY Convert CL Points data and insert into "CL_TRansDetail" table
        private Boolean CovertCLPointsData(Int64 clCardId)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = "SELECT pt.TransId as TransNo, cld.CardId, 0.00 AS CurrentPoints, pt.LoyaltyPoints as PointsAcquired, 0.00 AS RunningTotalPoints, 'T' as RowType, pt.TransDate as TransDate From postransaction pt, customer c, cl_cards cld " +
                    " Where pt.customerid=c.customerid and c.customerid=cld.customerid " +
                    " and cardid= " + clCardId + " and pt.LoyaltyPoints<>0 " + 
                    " Union All " + 
                    " Select 0 as TransNo, CardId, 0.00 AS CurrentPoints, NewPoints as PointsAcquired, 0.00 AS RunningTotalPoints, 'A' as RowType, CreatedOn as TransDate From cl_PointsAdjustmentLog Where cardid= " + clCardId +
                    " Union All " + 
                    " SELECT coupon.UsedInTransId as TransNo, coupon.CardID, 0.00 AS CurrentPoints, -1*coupon.Points as PointsAcquired, 0.00 AS RunningTotalPoints,'U' as RowType, coupon.CreatedOn as TransDate From cl_Coupons coupon , postransaction pt " + 
                    " Where coupon.UsedInTransId=pt.transid and coupon.cardid= " + clCardId + " and coupon.isActive=0 " + 
                    " And coupon.IsCouponUsed=1 " + 
                    " Union All " + 
                    " SELECT coupon.CreatedInTransId as TransNo, coupon.CardId, 0.00 AS CurrentPoints, -1*coupon.Points as PointsAcquired, 0.00 AS RunningTotalPoints,'X' as RowType, coupon.CreatedOn as TransDate From cl_Coupons coupon " +
                    " Where cardid= " + clCardId + " and coupon.isActive=1 and coupon.IsCouponUsed=0 " + 
                    " And DateAdd(d,ExpiryDays,CreatedOn)<GetDate() order by TransDate";

                DataSet result = new Search().SearchData(strSQL);

                decimal RunningTotalPoints = 0;
                decimal CurrentPoints = 0;

                CLTransDetail oBRCLTransDetail = new CLTransDetail();
                CLTransDetailRow oCLTransDetailRow;
                //IDbTransaction oTrans = null;

                //delete records
                oBRCLTransDetail.DeleteRow(clCardId);

                int i=0;
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    i++;
                    if (row["RowType"].ToString() == "A" || row["RowType"].ToString() == "X")   //13-Apr-2015 JY Added row["RowType"].ToString() == "X" to set points to 0 in case of expired coupons
                    {
                        RunningTotalPoints = 0;
                        CurrentPoints = 0;
                    }

                    CurrentPoints = 0;
                    RunningTotalPoints += Configuration.convertNullToDecimal(row["PointsAcquired"]); 

                    if (result.Tables[0].Rows.Count == i)
                    {
                        string sSql = @"SELECT  (SELECT ISNULL(SUM(CurrentPoints),0) From CL_Cards WHERE CardID = '" + clCardId + "' AND IsActive =1 " + ") AS CurrentUnusedPoints, " +
                          " (SELECT ISNULL(SUM(Points),0) from CL_Coupons WHERE  CardID = '" + clCardId + "'" + " AND isactive =1 " +
                          " AND IsCouponUsed <> 1 AND DateAdd(d,ExpiryDays,CreatedOn)>=GetDate() ) AS CurrentUsedPoints";
                        DataSet dsTotalPoints = new Search().SearchData(sSql);
                        if (Configuration.isNullOrEmptyDataSet(dsTotalPoints) == false)
                        {
                            CurrentPoints = Configuration.convertNullToDecimal(dsTotalPoints.Tables[0].Rows[0]["CurrentUnusedPoints"]);
                            RunningTotalPoints = CurrentPoints + Configuration.convertNullToDecimal(dsTotalPoints.Tables[0].Rows[0]["CurrentUsedPoints"]);
                        }
                    }
                    if (RunningTotalPoints < 0) RunningTotalPoints = 0; 

                    row["RunningTotalPoints"] = RunningTotalPoints;
                    row["CurrentPoints"] = CurrentPoints;

                    if (row["RowType"].ToString() == "T" || row["RowType"].ToString() == "A")
                    {
                        CLTransDetailData oCLTransDetailData = new CLTransDetailData();
                        oCLTransDetailRow = oCLTransDetailData.CLTransDetail.AddRow(0, Convert.ToInt32(row["TransNo"]), clCardId, Convert.ToDecimal(row["CurrentPoints"]), Convert.ToDecimal(row["PointsAcquired"]), Convert.ToDecimal(row["RunningTotalPoints"]), row["RowType"].ToString(), Convert.ToDateTime(row["TransDate"]));
                        //insert rows
                        Application.DoEvents();
                        oBRCLTransDetail.Persist(oCLTransDetailData);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion


        private void PopulateCouponsStatusGrid()
        {
            if (this.grdCLCoupons.Visible == false) return;

            logger.Trace("PopulateCouponsStatusGrid() - " + clsPOSDBConstants.Log_Entering);

            Int64 clCardId = -1;
            if (selectedCard != null)
            {
                clCardId = selectedCard.CLCardID;
            }

            //PRIMEPOS-2783 19-Feb-2020 JY modified to display adjustment
            String sql = "SELECT coupon.id as CouponId, coupon.CouponValue," +
                        " Case When coupon.IsCouponUsed = 1 Then coupon.UsedInTransId Else coupon.CreatedInTransId End as TransNo," +
                        " Case When coupon.IsCouponUsed = 1 Then (Select pt.TransDate From PosTransaction pt Where pt.TransId = coupon.UsedInTransId) Else coupon.CreatedOn End as TransDate," +
                        " Case When coupon.IsCouponUsed = 1 Then 'Used' When coupon.isActive = 0 Then 'Cancelled'" +
                        " When DateAdd(d, ExpiryDays, CreatedOn) < GetDate() Then 'Coupon Expired'" +
                        " When IsNUll(coupon.CreatedInTransId, 0) = 0 Then 'Adjustment' Else 'Available' End as RowType From cl_Coupons coupon" +
                        " Where coupon.cardid = " + clCardId + " Order by coupon.id DESC";
            DataSet result = new Search().SearchData(sql);
            int availableCoupons = 0;
            decimal totCouponValue = 0;
            foreach (DataRow row in result.Tables[0].Rows)
            {
                if (row["RowType"].ToString() == "Available" || row["RowType"].ToString() == "Adjustment")
                {
                    availableCoupons++;
                    totCouponValue += Configuration.convertNullToDecimal(row["CouponValue"]);
                }
            }
            this.grdCLCoupons.DataSource = result;

            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["TransNo"].Header.Caption = "Trans #";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["CouponId"].Header.Caption = "Coupon #";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["RowType"].Header.Caption = "Status";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["TransDate"].Format = "MM/dd/yyyy";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["TransDate"].Header.VisiblePosition = 4;
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["TransDate"].Header.Caption = "Date";
            this.grdCLCoupons.DisplayLayout.Bands[0].Columns["CouponValue"].Header.Caption = "Coupon Value";
            this.grdCLCoupons.Text = "                          # of available coupons :" + availableCoupons + 
                                     "\n                          Total coupon value :" + totCouponValue;
            this.grdCLCoupons.DisplayLayout.Scrollbars = Scrollbars.Vertical;
            this.resizeColumns(grdCLCoupons);
            this.grdCLCoupons.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

            logger.Trace("PopulateCouponsStatusGrid() - " + clsPOSDBConstants.Log_Entering);
        }

        private void txtCardID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCardID.Text) == true && string.IsNullOrEmpty(this.lblCustomerName.Text) == true)
            {
                this.Height = this.grdCLCoupons.Top + 30;
                this.grdCLCoupons.Visible = false;
                this.grdCLPoints.Visible = false;
            }
        }

        //Sprint-23 - PRIMEPOS-2275 13-Jun-2016 JY Added to handel close event by X controlbox button
        private void frmPOSTransCLCardInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCloseButtonClicked == true)   //Close button on the screen
            {
            }
            else //Controlbox X button on the form
            {
                selectedCard = null;
            }

        }
    }
}
