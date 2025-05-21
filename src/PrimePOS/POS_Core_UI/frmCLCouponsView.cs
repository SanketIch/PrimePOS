using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win;
using System.Data;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmCLCouponsView.
	/// </summary>
	public class frmCLCouponsView : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHistory;
		private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private IContainer components;
        private Infragistics.Win.Misc.UltraButton btnReject;
        private Infragistics.Win.Misc.UltraButton btnOk;

        private Int64 _CLCardID;

        public frmCLCouponsView(Int64 CLCardId)
        {
            _CLCardID = CLCardId;
            InitializeComponent();
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Amount");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Reference");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCLCouponsView));
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.grdHistory = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnReject = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdHistory
            // 
            this.grdHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance28.BackColor = System.Drawing.Color.White;
            appearance28.BackColor2 = System.Drawing.Color.White;
            appearance28.BackColorDisabled = System.Drawing.Color.White;
            appearance28.BackColorDisabled2 = System.Drawing.Color.White;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdHistory.DisplayLayout.Appearance = appearance28;
            ultraGridBand1.UseRowLayout = true;
            this.grdHistory.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdHistory.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistory.DisplayLayout.InterBandSpacing = 10;
            this.grdHistory.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHistory.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BackColor2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance29;
            appearance30.BackColor = System.Drawing.Color.White;
            appearance30.BackColor2 = System.Drawing.Color.White;
            appearance30.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.White;
            appearance31.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.AddRowAppearance = appearance31;
            this.grdHistory.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdHistory.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdHistory.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdHistory.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdHistory.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance32.BackColor = System.Drawing.Color.Transparent;
            this.grdHistory.DisplayLayout.Override.CardAreaAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.White;
            appearance33.BackColor2 = System.Drawing.Color.White;
            appearance33.BackColorDisabled = System.Drawing.Color.White;
            appearance33.BackColorDisabled2 = System.Drawing.Color.White;
            appearance33.BorderColor = System.Drawing.Color.Black;
            appearance33.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdHistory.DisplayLayout.Override.CellAppearance = appearance33;
            appearance34.BackColor = System.Drawing.Color.White;
            appearance34.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance34.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance34.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance34.BorderColor = System.Drawing.Color.Gray;
            appearance34.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance34.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance34.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdHistory.DisplayLayout.Override.CellButtonAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdHistory.DisplayLayout.Override.EditCellAppearance = appearance35;
            appearance36.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredInRowAppearance = appearance36;
            appearance37.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.FilteredOutRowAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.White;
            appearance38.BackColor2 = System.Drawing.Color.White;
            appearance38.BackColorDisabled = System.Drawing.Color.White;
            appearance38.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.FixedCellAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance39.BackColor2 = System.Drawing.Color.Beige;
            this.grdHistory.DisplayLayout.Override.FixedHeaderAppearance = appearance39;
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance40.FontData.BoldAsString = "True";
            appearance40.FontData.SizeInPoints = 10F;
            appearance40.ForeColor = System.Drawing.Color.White;
            appearance40.TextHAlignAsString = "Left";
            appearance40.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdHistory.DisplayLayout.Override.HeaderAppearance = appearance40;
            this.grdHistory.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdHistory.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance41.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAlternateAppearance = appearance41;
            appearance42.BackColor = System.Drawing.Color.White;
            appearance42.BackColor2 = System.Drawing.Color.White;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance42.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance42.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowAppearance = appearance42;
            appearance43.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowPreviewAppearance = appearance43;
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance44.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance44.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.RowSelectorAppearance = appearance44;
            this.grdHistory.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdHistory.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdHistory.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance45.BackColor = System.Drawing.Color.Navy;
            appearance45.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdHistory.DisplayLayout.Override.SelectedCellAppearance = appearance45;
            appearance46.BackColor = System.Drawing.Color.Navy;
            appearance46.BackColorDisabled = System.Drawing.Color.Navy;
            appearance46.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance46.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance46.BorderColor = System.Drawing.Color.Gray;
            appearance46.ForeColor = System.Drawing.Color.White;
            this.grdHistory.DisplayLayout.Override.SelectedRowAppearance = appearance46;
            this.grdHistory.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdHistory.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance47.BorderColor = System.Drawing.Color.Gray;
            this.grdHistory.DisplayLayout.Override.TemplateAddRowAppearance = appearance47;
            this.grdHistory.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdHistory.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance48.BackColor = System.Drawing.Color.White;
            appearance48.BackColor2 = System.Drawing.Color.Gainsboro;
            appearance48.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance48.BackHatchStyle = Infragistics.Win.BackHatchStyle.Horizontal;
            appearance48.BorderColor = System.Drawing.Color.WhiteSmoke;
            appearance48.BorderColor3DBase = System.Drawing.Color.WhiteSmoke;
            appearance48.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            scrollBarLook1.Appearance = appearance48;
            appearance49.BackColor = System.Drawing.Color.LightGray;
            appearance49.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            scrollBarLook1.ButtonAppearance = appearance49;
            appearance50.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            scrollBarLook1.ThumbAppearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.Gainsboro;
            appearance51.BackColor2 = System.Drawing.Color.WhiteSmoke;
            appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance51.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance51.BorderAlpha = Infragistics.Win.Alpha.UseAlphaLevel;
            appearance51.BorderColor = System.Drawing.Color.White;
            appearance51.BorderColor3DBase = System.Drawing.Color.Gainsboro;
            appearance51.ImageBackgroundOrigin = Infragistics.Win.ImageBackgroundOrigin.Form;
            scrollBarLook1.TrackAppearance = appearance51;
            this.grdHistory.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdHistory.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdHistory.Location = new System.Drawing.Point(14, 24);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.Size = new System.Drawing.Size(362, 312);
            this.grdHistory.TabIndex = 0;
            this.grdHistory.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdHistory.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2});
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance26.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance26;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(22, 15);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(388, 38);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Coupons View";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnReject);
            this.groupBox2.Controls.Add(this.btnOk);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(22, 410);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(388, 54);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnReject
            // 
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.White;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            this.btnReject.Appearance = appearance8;
            this.btnReject.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnReject.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReject.Location = new System.Drawing.Point(292, 17);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(88, 28);
            this.btnReject.TabIndex = 74;
            this.btnReject.Text = "&Cancel";
            this.btnReject.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnReject.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance9.FontData.BoldAsString = "True";
            appearance9.ForeColor = System.Drawing.Color.White;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            this.btnOk.Appearance = appearance9;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.Location = new System.Drawing.Point(197, 17);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 28);
            this.btnOk.TabIndex = 73;
            this.btnOk.Text = "&Select";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.grdHistory);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(22, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(388, 348);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // frmCLCouponsView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(422, 479);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCLCouponsView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Coupons View";
            this.Load += new System.EventHandler(this.frmCLCouponsView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCLCouponsView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmCLCouponsView_Load(object sender, System.EventArgs e)
		{
			try
			{
				Display();
				clsUIHelper.SetReadonlyRow(this.grdHistory);
				clsUIHelper.SetAppearance(this.grdHistory);
				clsUIHelper.setColorSchecme(this);
                this.grdHistory.Focus();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

        private void Display()
        {

            String strQuery = "Select " + clsPOSDBConstants.CLCoupons_Fld_ID + " as [Coupon #]," +
                clsPOSDBConstants.CLCoupons_Fld_Points + " as [Points]," +
                clsPOSDBConstants.CLCoupons_Fld_CouponValue + " as [Discount]" +
                " From " + clsPOSDBConstants.CLCoupons_tbl +
                " Where CardID=" + _CLCardID.ToString() + " And (IsCouponUsed=0 And IsActive=1 And DateAdd(d,ExpiryDays,CreatedOn)>GetDate()) ";
            Search oSearch = new Search();
            DataSet ods = oSearch.SearchData(strQuery);
            this.grdHistory.DataSource = ods;
            this.grdHistory.Refresh();
            ApplyGrigFormat();
            resizeColumns();
        }

        private void resizeColumns()
        {
            try
            {
                foreach (UltraGridBand oBand in grdHistory.DisplayLayout.Bands)
                {
                    foreach (UltraGridColumn oCol in oBand.Columns)
                    {
                        oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true) + 10;
                        if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                        {
                            oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void ApplyGrigFormat()
        {
            clsUIHelper.SetReadonlyRow(this.grdHistory);
            clsUIHelper.SetAppearance(this.grdHistory);
            clsUIHelper.setColorSchecme(this);
            grdHistory.DisplayLayout.Bands[0].Columns["Discount"].Format = "#######0.00";
            this.grdHistory.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
        }

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void frmCLCouponsView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
                else if (e.KeyData == Keys.Escape)
                {
                    btnClose_Click(btnReject, new EventArgs());
                }
				
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public Int64 GetSelectedCouponID()
        {
            if (grdHistory.ActiveRow!=null)
            {
                return Configuration.convertNullToInt64(grdHistory.ActiveRow.Cells["Coupon #"].Value);
            }
            return 0;
        }
	}
}
