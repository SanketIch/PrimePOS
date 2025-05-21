using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;
//using POS.UI;
//using POS_Core_UI.Reports.ReportsUI;
namespace POS_Core_UI.Reports.ReportsUI
{
	/// <summary>
	/// Summary description for frmViewTransaction.
	/// </summary>
	public class frmViewTransaction : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel21;
		private Infragistics.Win.Misc.UltraLabel label;
		private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtTransactionNo;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserID;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStationID;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.Misc.UltraButton btnView;
		private Infragistics.Win.Misc.UltraButton btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmViewTransaction()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewTransaction));
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtStationID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTransactionNo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.label = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.txtUserID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtStationID);
            this.groupBox1.Controls.Add(this.txtTransactionNo);
            this.groupBox1.Controls.Add(this.label);
            this.groupBox1.Controls.Add(this.ultraLabel21);
            this.groupBox1.Controls.Add(this.ultraLabel12);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 143);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // txtStationID
            // 
            this.txtStationID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtStationID.Location = new System.Drawing.Point(280, 94);
            this.txtStationID.MaxLength = 20;
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(123, 23);
            this.txtStationID.TabIndex = 2;
            this.txtStationID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtTransactionNo
            // 
            this.txtTransactionNo.AlwaysInEditMode = true;
            this.txtTransactionNo.AutoSize = false;
            this.txtTransactionNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtTransactionNo.FormatString = "";
            this.txtTransactionNo.Location = new System.Drawing.Point(280, 36);
            this.txtTransactionNo.MaxValue = 1410065407;
            this.txtTransactionNo.MinValue = 0;
            this.txtTransactionNo.Name = "txtTransactionNo";
            this.txtTransactionNo.Nullable = true;
            this.txtTransactionNo.NullText = "0";
            this.txtTransactionNo.Size = new System.Drawing.Size(123, 23);
            this.txtTransactionNo.TabIndex = 0;
            this.txtTransactionNo.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtTransactionNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtTransactionNo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // label
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            this.label.Appearance = appearance1;
            this.label.Location = new System.Drawing.Point(18, 28);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(203, 31);
            this.label.TabIndex = 15;
            this.label.Text = "Starting Transaction # <Blank = Last Transaction>";
            // 
            // ultraLabel21
            // 
            appearance2.ForeColor = System.Drawing.Color.White;
            this.ultraLabel21.Appearance = appearance2;
            this.ultraLabel21.AutoSize = true;
            this.ultraLabel21.Location = new System.Drawing.Point(18, 100);
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.Size = new System.Drawing.Size(228, 18);
            this.ultraLabel21.TabIndex = 17;
            this.ultraLabel21.Text = "Station # <Blank = All Station>";
            // 
            // ultraLabel12
            // 
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraLabel12.Appearance = appearance3;
            this.ultraLabel12.AutoSize = true;
            this.ultraLabel12.Location = new System.Drawing.Point(18, 69);
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.Size = new System.Drawing.Size(195, 18);
            this.ultraLabel12.TabIndex = 6;
            this.ultraLabel12.Text = "User ID <Blank = Any User";
            // 
            // txtUserID
            // 
            this.txtUserID.AutoSize = false;
            this.txtUserID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtUserID.Location = new System.Drawing.Point(280, 63);
            this.txtUserID.MaxLength = 20;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(123, 23);
            this.txtUserID.TabIndex = 1;
            this.txtUserID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblTransactionType
            // 
            appearance4.ForeColor = System.Drawing.Color.White;
            appearance4.ForeColorDisabled = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance4;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(11, 16);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(424, 30);
            this.lblTransactionType.TabIndex = 26;
            this.lblTransactionType.Text = "View POS Transactions";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 194);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 57);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            // 
            // btnClose
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            this.btnClose.Appearance = appearance5;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(318, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnView
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            this.btnView.Appearance = appearance6;
            this.btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnView.Location = new System.Drawing.Point(226, 20);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 26);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "&View";
            this.btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // frmViewTransaction
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(454, 268);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmViewTransaction";
            this.Text = "View Transaction";
            this.Load += new System.EventHandler(this.frmViewTransaction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmViewTransaction_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStationID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmViewTransaction_Load(object sender, System.EventArgs e)
		{
			this.txtStationID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtStationID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			this.txtTransactionNo.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtTransactionNo.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtUserID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtUserID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
			
			clsUIHelper.setColorSchecme(this);
		}

		private void btnView_Click(object sender, System.EventArgs e)
		{
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			frmViewTransactionDetail ofrmVTD=new frmViewTransactionDetail(this.txtTransactionNo.Value.ToString().Trim(),this.txtUserID.Text.Trim(),this.txtStationID.Text.Trim(),false);
			ofrmVTD.ShowDialog();
			this.Cursor = System.Windows.Forms.Cursors.Default;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmViewTransaction_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				else if (e.KeyData==Keys.Escape)
					this.Close();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}

	}
}
