using System;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinMaskedEdit;
namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmWarningMessages.
	/// </summary>
	public class frmWarningMessages : System.Windows.Forms.Form
	{
        
		private WarningMessagesData oWarningMsgsHData ;
		private WarningMessagesRow oWarningMsgHRow;
		private WarningMessages oWarningMsgs= new  WarningMessages();

		private WarningMessagesDetailData oWarningMsgDData;
        
        //private System.Data.DataView oWarningMsgDeptView;
        //private System.Data.DataView oWarningMsgItemView;

		private bool m_exceptionAccoured = false;

        private Infragistics.Win.Misc.UltraLabel ultraLabel21;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWarningMessage;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
		private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ToolTip toolTip1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
		public  Infragistics.Win.Misc.UltraButton btnAddItem;
        public Infragistics.Win.Misc.UltraButton btnDeleteItem;
		private System.ComponentModel.IContainer components;
        private POHeaderData poHeadData = null;
        private PODetailData poDetailData = null;
        private String inventoryWay = String.Empty;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkIsActive;
        private GroupBox groupBox4;
        public Infragistics.Win.Misc.UltraButton btnDeleteDept;
        private UltraGrid grdDetailDept;
        public Infragistics.Win.Misc.UltraButton btnAddDept;
        public bool IsCanceled = true;


		public frmWarningMessages()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WarningDetailID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WarningID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RefObjectID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RefObjectType");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWarningMessages));
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WarningDetailID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("WarningID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RefObjectID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RefObjectType");
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook2 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.txtWarningMessage = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIsActive = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDeleteItem = new Infragistics.Win.Misc.UltraButton();
            this.btnAddItem = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.grdDetailDept = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnDeleteDept = new Infragistics.Win.Misc.UltraButton();
            this.btnAddDept = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtWarningMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailDept)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel21
            // 
            appearance42.FontData.BoldAsString = "False";
            appearance42.ForeColor = System.Drawing.Color.White;
            appearance42.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel21.Appearance = appearance42;
            this.ultraLabel21.Location = new System.Drawing.Point(16, 60);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(80, 17);
            this.ultraLabel21.TabIndex = 4;
            this.ultraLabel21.Text = "Is Active";
            this.ultraLabel21.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtWarningMessage
            // 
            this.txtWarningMessage.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtWarningMessage.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWarningMessage.Location = new System.Drawing.Point(154, 25);
            this.txtWarningMessage.Name = "txtWarningMessage";
            this.txtWarningMessage.Size = new System.Drawing.Size(463, 23);
            this.txtWarningMessage.TabIndex = 1;
            // 
            // ultraLabel12
            // 
            appearance5.FontData.BoldAsString = "False";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel12.Appearance = appearance5;
            this.ultraLabel12.Location = new System.Drawing.Point(16, 29);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(129, 19);
            this.ultraLabel12.TabIndex = 1;
            this.ultraLabel12.Text = "Warning Message";
            this.ultraLabel12.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // grdDetail
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackColorDisabled = System.Drawing.Color.White;
            appearance1.BackColorDisabled2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance1;
            this.grdDetail.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(176, 0);
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.Width = 123;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            ultraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridBand1.UseRowLayout = true;
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance4;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.Color.White;
            appearance30.BackColorDisabled = System.Drawing.Color.White;
            appearance30.BackColorDisabled2 = System.Drawing.Color.White;
            appearance30.BorderColor = System.Drawing.Color.Black;
            appearance30.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance30;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance34.BorderColor = System.Drawing.Color.Gray;
            appearance34.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance34.Image = ((object)(resources.GetObject("appearance34.Image")));
            appearance34.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance34.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance35;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance36;
            appearance37.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance37;
            appearance39.BackColor = System.Drawing.Color.White;
            appearance39.BackColor2 = System.Drawing.Color.White;
            appearance39.BackColorDisabled = System.Drawing.Color.White;
            appearance39.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance39;
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance43.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance43;
            appearance64.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance64.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance64.FontData.BoldAsString = "True";
            appearance64.FontData.SizeInPoints = 10F;
            appearance64.ForeColor = System.Drawing.Color.White;
            appearance64.TextHAlignAsString = "Left";
            appearance64.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance64;
            appearance65.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance65;
            appearance66.BackColor = System.Drawing.Color.White;
            appearance66.BackColor2 = System.Drawing.Color.White;
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance66.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance66.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance66;
            appearance67.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance67;
            appearance70.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance70.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance70.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance70.BorderColor = System.Drawing.Color.Gray;
            appearance70.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance70;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance71.BackColor = System.Drawing.Color.Navy;
            appearance71.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance71;
            appearance72.BackColor = System.Drawing.Color.Navy;
            appearance72.BackColorDisabled = System.Drawing.Color.Navy;
            appearance72.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance72.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance72.BorderColor = System.Drawing.Color.Gray;
            appearance72.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance72;
            appearance73.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance73;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance74.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance74;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(6, 23);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(293, 256);
            this.grdDetail.TabIndex = 4;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmWarningMessages_KeyUp);
            this.grdDetail.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdDetail_BeforeRowUpdate);
            this.grdDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmWarningMessages_KeyDown);
            this.grdDetail.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.ValidateRow);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance31.FontData.BoldAsString = "True";
            appearance31.ForeColor = System.Drawing.Color.White;
            appearance31.Image = ((object)(resources.GetObject("appearance31.Image")));
            this.btnClose.Appearance = appearance31;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(513, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 26);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance32.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance32.FontData.BoldAsString = "True";
            appearance32.ForeColor = System.Drawing.Color.White;
            appearance32.Image = ((object)(resources.GetObject("appearance32.Image")));
            this.btnSave.Appearance = appearance32;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSave.Location = new System.Drawing.Point(417, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance33.ForeColor = System.Drawing.Color.White;
            appearance33.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance33.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance33;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(18, 8);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(617, 40);
            this.lblTransactionType.TabIndex = 21;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Warning Messages";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkIsActive);
            this.groupBox1.Controls.Add(this.ultraLabel7);
            this.groupBox1.Controls.Add(this.txtWarningMessage);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(625, 91);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // chkIsActive
            // 
            appearance17.FontData.BoldAsString = "False";
            this.chkIsActive.Appearance = appearance17;
            this.chkIsActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsActive.Location = new System.Drawing.Point(154, 59);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(16, 19);
            this.chkIsActive.TabIndex = 2;
            this.chkIsActive.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.chkIsActive.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel7
            // 
            appearance38.ForeColor = System.Drawing.Color.White;
            appearance38.TextHAlignAsString = "Center";
            appearance38.TextVAlignAsString = "Middle";
            this.ultraLabel7.Appearance = appearance38;
            this.ultraLabel7.AutoSize = true;
            this.ultraLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel7.Location = new System.Drawing.Point(137, 33);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(11, 15);
            this.ultraLabel7.TabIndex = 37;
            this.ultraLabel7.Text = "*";
            this.ultraLabel7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnDeleteItem);
            this.groupBox2.Controls.Add(this.btnAddItem);
            this.groupBox2.Controls.Add(this.grdDetail);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(9, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(313, 320);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Link Items";
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.AcceptsFocus = false;
            appearance68.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance68.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance68.FontData.BoldAsString = "True";
            appearance68.ForeColor = System.Drawing.Color.White;
            appearance68.Image = ((object)(resources.GetObject("appearance68.Image")));
            this.btnDeleteItem.Appearance = appearance68;
            this.btnDeleteItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnDeleteItem.Location = new System.Drawing.Point(8, 286);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(144, 26);
            this.btnDeleteItem.TabIndex = 15;
            this.btnDeleteItem.TabStop = false;
            this.btnDeleteItem.Text = "Del. Item (F3)";
            this.btnDeleteItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.AcceptsFocus = false;
            appearance69.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance69.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance69.FontData.BoldAsString = "True";
            appearance69.ForeColor = System.Drawing.Color.White;
            appearance69.Image = ((object)(resources.GetObject("appearance69.Image")));
            this.btnAddItem.Appearance = appearance69;
            this.btnAddItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnAddItem.Location = new System.Drawing.Point(156, 286);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(144, 26);
            this.btnAddItem.TabIndex = 13;
            this.btnAddItem.TabStop = false;
            this.btnAddItem.Text = "Add Item (F2)";
            this.btnAddItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(10, 469);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(625, 56);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.grdDetailDept);
            this.groupBox4.Controls.Add(this.btnDeleteDept);
            this.groupBox4.Controls.Add(this.btnAddDept);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(328, 147);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(307, 320);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Link Departments";
            // 
            // grdDetailDept
            // 
            appearance75.BackColor = System.Drawing.Color.White;
            appearance75.BackColor2 = System.Drawing.Color.White;
            appearance75.BackColorDisabled = System.Drawing.Color.White;
            appearance75.BackColorDisabled2 = System.Drawing.Color.White;
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetailDept.DisplayLayout.Appearance = appearance75;
            this.grdDetailDept.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn6.Header.VisiblePosition = 0;
            ultraGridColumn7.Header.VisiblePosition = 1;
            ultraGridColumn8.Header.VisiblePosition = 2;
            ultraGridColumn9.Header.VisiblePosition = 3;
            ultraGridColumn9.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn9.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(176, 0);
            ultraGridColumn9.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn9.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn9.Width = 123;
            ultraGridColumn10.Header.VisiblePosition = 4;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10});
            ultraGridBand2.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            ultraGridBand2.UseRowLayout = true;
            this.grdDetailDept.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdDetailDept.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetailDept.DisplayLayout.InterBandSpacing = 10;
            appearance76.BackColor = System.Drawing.Color.White;
            appearance76.BackColor2 = System.Drawing.Color.White;
            this.grdDetailDept.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance76;
            appearance77.BackColor = System.Drawing.Color.White;
            appearance77.BackColor2 = System.Drawing.Color.White;
            appearance77.BorderColor = System.Drawing.Color.Gray;
            this.grdDetailDept.DisplayLayout.Override.ActiveRowAppearance = appearance77;
            appearance78.BackColor = System.Drawing.Color.White;
            appearance78.BackColor2 = System.Drawing.Color.White;
            appearance78.BorderColor = System.Drawing.Color.Gray;
            this.grdDetailDept.DisplayLayout.Override.AddRowAppearance = appearance78;
            this.grdDetailDept.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdDetailDept.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDetailDept.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            appearance79.BackColor = System.Drawing.Color.Transparent;
            this.grdDetailDept.DisplayLayout.Override.CardAreaAppearance = appearance79;
            appearance80.BackColor = System.Drawing.Color.White;
            appearance80.BackColor2 = System.Drawing.Color.White;
            appearance80.BackColorDisabled = System.Drawing.Color.White;
            appearance80.BackColorDisabled2 = System.Drawing.Color.White;
            appearance80.BorderColor = System.Drawing.Color.Black;
            appearance80.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetailDept.DisplayLayout.Override.CellAppearance = appearance80;
            appearance81.BackColor = System.Drawing.Color.White;
            appearance81.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance81.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance81.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance81.BorderColor = System.Drawing.Color.Gray;
            appearance81.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance81.Image = ((object)(resources.GetObject("appearance81.Image")));
            appearance81.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance81.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDetailDept.DisplayLayout.Override.CellButtonAppearance = appearance81;
            appearance82.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance82.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetailDept.DisplayLayout.Override.EditCellAppearance = appearance82;
            appearance83.BorderColor = System.Drawing.Color.Gray;
            this.grdDetailDept.DisplayLayout.Override.FilteredInRowAppearance = appearance83;
            appearance84.BorderColor = System.Drawing.Color.Gray;
            this.grdDetailDept.DisplayLayout.Override.FilteredOutRowAppearance = appearance84;
            appearance85.BackColor = System.Drawing.Color.White;
            appearance85.BackColor2 = System.Drawing.Color.White;
            appearance85.BackColorDisabled = System.Drawing.Color.White;
            appearance85.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetailDept.DisplayLayout.Override.FixedCellAppearance = appearance85;
            appearance86.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance86.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetailDept.DisplayLayout.Override.FixedHeaderAppearance = appearance86;
            appearance87.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance87.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance87.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance87.FontData.BoldAsString = "True";
            appearance87.FontData.SizeInPoints = 10F;
            appearance87.ForeColor = System.Drawing.Color.White;
            appearance87.TextHAlignAsString = "Left";
            appearance87.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetailDept.DisplayLayout.Override.HeaderAppearance = appearance87;
            appearance88.BorderColor = System.Drawing.Color.Gray;
            this.grdDetailDept.DisplayLayout.Override.RowAlternateAppearance = appearance88;
            appearance89.BackColor = System.Drawing.Color.White;
            appearance89.BackColor2 = System.Drawing.Color.White;
            appearance89.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance89.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance89.BorderColor = System.Drawing.Color.Gray;
            this.grdDetailDept.DisplayLayout.Override.RowAppearance = appearance89;
            appearance90.BorderColor = System.Drawing.Color.Gray;
            this.grdDetailDept.DisplayLayout.Override.RowPreviewAppearance = appearance90;
            appearance91.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance91.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance91.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance91.BorderColor = System.Drawing.Color.Gray;
            appearance91.ForeColor = System.Drawing.Color.White;
            this.grdDetailDept.DisplayLayout.Override.RowSelectorAppearance = appearance91;
            this.grdDetailDept.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetailDept.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance92.BackColor = System.Drawing.Color.Navy;
            appearance92.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetailDept.DisplayLayout.Override.SelectedCellAppearance = appearance92;
            appearance93.BackColor = System.Drawing.Color.Navy;
            appearance93.BackColorDisabled = System.Drawing.Color.Navy;
            appearance93.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance93.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance93.BorderColor = System.Drawing.Color.Gray;
            appearance93.ForeColor = System.Drawing.Color.White;
            this.grdDetailDept.DisplayLayout.Override.SelectedRowAppearance = appearance93;
            appearance94.BorderColor = System.Drawing.Color.Gray;
            this.grdDetailDept.DisplayLayout.Override.TemplateAddRowAppearance = appearance94;
            this.grdDetailDept.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetailDept.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance95.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook2.TrackAppearance = appearance95;
            this.grdDetailDept.DisplayLayout.ScrollBarLook = scrollBarLook2;
            this.grdDetailDept.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetailDept.Location = new System.Drawing.Point(7, 23);
            this.grdDetailDept.Name = "grdDetailDept";
            this.grdDetailDept.Size = new System.Drawing.Size(293, 256);
            this.grdDetailDept.TabIndex = 6;
            this.grdDetailDept.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnDeleteDept
            // 
            this.btnDeleteDept.AcceptsFocus = false;
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance40.FontData.BoldAsString = "True";
            appearance40.ForeColor = System.Drawing.Color.White;
            appearance40.Image = ((object)(resources.GetObject("appearance40.Image")));
            this.btnDeleteDept.Appearance = appearance40;
            this.btnDeleteDept.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnDeleteDept.Location = new System.Drawing.Point(7, 286);
            this.btnDeleteDept.Name = "btnDeleteDept";
            this.btnDeleteDept.Size = new System.Drawing.Size(144, 26);
            this.btnDeleteDept.TabIndex = 15;
            this.btnDeleteDept.TabStop = false;
            this.btnDeleteDept.Text = "Del. Dept (F5)";
            this.btnDeleteDept.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDeleteDept.Click += new System.EventHandler(this.btnDeleteDept_Click);
            // 
            // btnAddDept
            // 
            this.btnAddDept.AcceptsFocus = false;
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance41.FontData.BoldAsString = "True";
            appearance41.ForeColor = System.Drawing.Color.White;
            appearance41.Image = ((object)(resources.GetObject("appearance41.Image")));
            this.btnAddDept.Appearance = appearance41;
            this.btnAddDept.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnAddDept.Location = new System.Drawing.Point(156, 286);
            this.btnAddDept.Name = "btnAddDept";
            this.btnAddDept.Size = new System.Drawing.Size(144, 26);
            this.btnAddDept.TabIndex = 13;
            this.btnAddDept.TabStop = false;
            this.btnAddDept.Text = "Add Dept (F4)";
            this.btnAddDept.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAddDept.Click += new System.EventHandler(this.btnAddDept_Click);
            // 
            // frmWarningMessages
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(647, 543);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmWarningMessages";
            this.Text = "Warning Messages";
            this.Resize += new System.EventHandler(this.frmWarningMessages_Resize);
            this.Activated += new System.EventHandler(this.frmWarningMessages_Activated);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmWarningMessages_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmWarningMessages_KeyDown);
            this.Load += new System.EventHandler(this.frmWarningMessages_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtWarningMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetailDept)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        public void Initialize()
        {
            SetNew();
        }

		private void ApplyItemGrigFormat()
		{
			clsUIHelper.SetAppearance(this.grdDetail);
		
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID].Hidden = true;
			this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID].Hidden = true;

            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectDescription].Header.Caption = "Item Description";
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectDescription].CellActivation = Activation.NoEdit;

		}

        private void ApplyDeptGrigFormat()
        {
            clsUIHelper.SetAppearance(this.grdDetail);

            this.grdDetailDept.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID].Hidden = true;
            this.grdDetailDept.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID].Hidden = true;
            this.grdDetailDept.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType].Hidden = true;
            this.grdDetailDept.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID].Hidden = true;

            this.grdDetailDept.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectDescription].Header.Caption = "Department Description";
            this.grdDetailDept.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectDescription].CellActivation = Activation.NoEdit;

        }

		private void frmWarningMessages_Load(object sender, System.EventArgs e)
		{
			try
			{
				clsUIHelper.SetKeyActionMappings(this.grdDetail);
				
				//SetNew();
				this.txtWarningMessage.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
				this.txtWarningMessage.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);


                //this.ApplyGrigFormat();

				clsUIHelper.setColorSchecme(this);

			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                if (txtWarningMessage.Text.Trim().Length == 0)
                {
                    clsUIHelper.ShowErrorMsg("Warning Message cannot be empty.");
                    txtWarningMessage.Focus();
                }
                else
                {
                    oWarningMsgHRow.WarningMessage = this.txtWarningMessage.Text;
                    oWarningMsgHRow.IsActive = chkIsActive.Checked;

                    oWarningMsgs.Persist(oWarningMsgsHData, oWarningMsgDData);
                    IsCanceled = false;
                    this.Close();
                }
                
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void Display()
        {
            txtWarningMessage.Text = oWarningMsgHRow.WarningMessage;
            chkIsActive.Checked = oWarningMsgHRow.IsActive;

            if (oWarningMsgDData == null)
            {
                oWarningMsgDData = new WarningMessagesDetailData();
            }

            BindItemGrid();
            BindDeptGrid();
        }

		private void Clear()
		{
			this.txtWarningMessage.Text = String.Empty;
			this.chkIsActive.Checked= true;
		}
		
		private void SetNew()
		{
			oWarningMsgsHData = new  WarningMessagesData();
			Clear();
			oWarningMsgHRow=oWarningMsgsHData.WarningMessages.AddRow(0,"",true);

			oWarningMsgDData=new WarningMessagesDetailData();
            BindItemGrid();

            BindDeptGrid(); 


			this.txtWarningMessage.Focus();
		}

		private void FKEdit(string code,string senderName)
		{
            if (senderName==clsPOSDBConstants.Item_tbl)
			{
				#region Items
				try
				{
					Item oItem=new  Item();
					ItemData oItemData;
					ItemRow oItemRow=null;
					oItemData = oItem.Populate(code);
					oItemRow = oItemData.Item[0];
					if (oItemRow!=null)
					{
                        WarningMessagesDetailRow oRow = this.oWarningMsgDData.WarningMessagesDetail.NewWarningMessagesDetailRow();
                        oRow.WarningMessagesID = this.oWarningMsgHRow.WarningMessageID;
                        oRow.RefObjectType = "I";
                        oRow.RefObjectID = oItemRow.ItemID;
                        oRow.RefObjectDescription = oItemRow.Description;

                        this.oWarningMsgDData.WarningMessagesDetail.AddRow(oRow,true);

                        BindItemGrid();
					}
				}
				catch(System.IndexOutOfRangeException )
				{
					this.grdDetail.ActiveCell.Value=String.Empty;
					this.grdDetail.ActiveRow.Cells["Description"].Value=String.Empty;
				}
				catch(Exception exp)
				{
					clsUIHelper.ShowErrorMsg(exp.Message);
				}
				#endregion
			}
            else if (senderName == clsPOSDBConstants.Department_tbl)
            {
                #region Departments
                try
                {
                    Department oDept= new  Department();
                    DepartmentData oDeptData;
                    DepartmentRow oDeptRow = null;
                    oDeptData = oDept.Populate(code);
                    oDeptRow = oDeptData.Department[0];
                    if (oDeptRow != null)
                    {
                        WarningMessagesDetailRow oRow = this.oWarningMsgDData.WarningMessagesDetail.NewWarningMessagesDetailRow();
                        oRow.WarningMessagesID = this.oWarningMsgHRow.WarningMessageID;
                        oRow.RefObjectType = "D";
                        oRow.RefObjectID = oDeptRow.DeptID.ToString();
                        oRow.RefObjectDescription = oDeptRow.DeptName;

                        this.oWarningMsgDData.WarningMessagesDetail.AddRow(oRow,true);

                        BindDeptGrid();
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    this.grdDetailDept.ActiveCell.Value = String.Empty;
                    this.grdDetailDept.ActiveRow.Cells["Description"].Value = String.Empty;
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
                #endregion
            }
		}

        private void BindItemGrid()
        {
            System.Data.DataView oWarningMsgItemView = new System.Data.DataView(this.oWarningMsgDData.WarningMessagesDetail);
             oWarningMsgItemView.RowFilter = "RefObjectType='I'";
             this.grdDetail.DataSource = oWarningMsgItemView;
             this.grdDetail.Refresh();
             ApplyItemGrigFormat();
        }

        private void BindDeptGrid()
        {
            System.Data.DataView oWarningMsgDeptView = new System.Data.DataView(this.oWarningMsgDData.WarningMessagesDetail);
            oWarningMsgDeptView.RowFilter = "RefObjectType='D'";
            this.grdDetailDept.DataSource = oWarningMsgDeptView;
            this.grdDetailDept.Refresh();
            ApplyDeptGrigFormat();
        }

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            IsCanceled = false;
			this.Close();
		}

		private void SearchItem()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
					
					FKEdit(strCode,clsPOSDBConstants.Item_tbl);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

        private void SearchDepartment()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Department_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Department_tbl; //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    FKEdit(strCode, clsPOSDBConstants.Department_tbl);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

		private void ValidateRow(object sender,System.ComponentModel.CancelEventArgs e)
		{
			UltraGridRow oCurrentRow;
			UltraGridCell oCurrentCell;
			oCurrentRow=this.grdDetail.ActiveRow;		
			oCurrentCell=null;
			bool blnCellChanged;
			blnCellChanged=false;
			
			foreach (UltraGridCell oCell in oCurrentRow.Cells)
			{
				if (oCell.DataChanged==true || oCell.Text.Trim()!="")
				{
					blnCellChanged=true;
					break;
				}
			}

			if (blnCellChanged==false)
			{
				return;
			}
			try
			{
				/*oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_ItemID];
				oWarningMsgs.Validate_ItemID(oCurrentCell.Text.ToString());
				
				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_QTY];
				oWarningMsgs.Validate_Qty(oCurrentCell.Text.ToString());
				
				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_Cost];
				oWarningMsgs.Validate_Cost(oCurrentCell.Text.ToString());

				oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_SalePrice];
				oWarningMsgs.Validate_SalePrice(oCurrentCell.Text.ToString());
                */
			} 
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				if (oCurrentCell!=null)
				{
					m_exceptionAccoured = true;
					e.Cancel=true;
					this.grdDetail.ActiveCell=oCurrentCell;
					this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
					this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
					
				}
			}	
		}

        public void Edit(string WarningMessageID)
        {
            try
            {
                int iWarningMessageID = POS_Core.Resources.Configuration.convertNullToInt(WarningMessageID.Trim());

                oWarningMsgsHData=oWarningMsgs.Populate(iWarningMessageID);

                oWarningMsgDData=oWarningMsgs.PopulateDetail(iWarningMessageID);

                if (oWarningMsgsHData != null)
                {
                    oWarningMsgHRow = oWarningMsgsHData.WarningMessages[0];
                    Display();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			SetNew();
		}

		private void frmWarningMessages_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmWarningMessages_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
                    SearchDepartment();
				}
                else if (e.KeyData == System.Windows.Forms.Keys.F5)
                {
                    btnDeleteDept_Click(btnDeleteDept, new EventArgs());
                }
				else if (e.KeyData==System.Windows.Forms.Keys.F2)
				{
					SearchItem();

				}
				else if (e.KeyData==System.Windows.Forms.Keys.F3)
				{
                    btnDeleteItem_Click(btnDeleteItem, new EventArgs());
				}
				e.Handled=true;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

		private void frmWarningMessages_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;            
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
		}

		private void grdDetail_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{
			UltraGridRow oCurrentRow;
			UltraGridCell oCurrentCell;
			oCurrentRow=e.Row;		
			oCurrentCell=null;
			
			try
			{
				//oCurrentCell=oCurrentRow.Cells[clsPOSDBConstants.InvRecvDetail_Fld_ItemID];
				//oWarningMsgs.Validate_ItemID(oCurrentCell.Text.ToString());
				
			} 
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
				if (oCurrentCell!=null)
				{
					this.grdDetail.ActiveCell=oCurrentCell;
					this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
					this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
				}
				e.Cancel=true;
			}	
		
		}

		private void btnAddItem_Click(object sender, System.EventArgs e)
		{
			SearchItem();
		}

		private void btnDeleteItem_Click(object sender, System.EventArgs e)
		{
			if (this.grdDetail.ActiveRow!=null)
			{
				this.grdDetail.ActiveRow.Delete(true);
			}
		}

        private void frmWarningMessages_Resize(object sender, EventArgs e)
        {
            this.Left = clsPOSDBConstants.formLeft;
            this.Top = clsPOSDBConstants.formLeft;
        }

        private void btnDeleteDept_Click(object sender, EventArgs e)
        {
            if (this.grdDetailDept.ActiveRow != null)
            {
                this.grdDetailDept.ActiveRow.Delete(true);
            }
        }

        private void btnAddDept_Click(object sender, EventArgs e)
        {
            SearchDepartment();
        }
	}
}
