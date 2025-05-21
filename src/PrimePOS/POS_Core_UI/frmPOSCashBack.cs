using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPOSDicount.
	/// </summary>
	public class frmPOSCashBack : System.Windows.Forms.Form
    {
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraLabel lblCashBackAmount;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor numDiscAmount;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private GroupBox groupBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private Infragistics.Win.Misc.UltraLabel lblLimit;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

		public frmPOSCashBack()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            lblLimit.Text = "(<=" + Configuration.convertNullToDecimal(Configuration.UserMaxCashbackLimit).ToString() + ")";

            this.numDiscAmount.MaxValue = Configuration.convertNullToDecimal(Configuration.UserMaxCashbackLimit);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSCashBack));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblCashBackAmount = new Infragistics.Win.Misc.UltraLabel();
            this.numDiscAmount = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblLimit = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscAmount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.BorderColor3DBase = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "True";
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnClose.Appearance = appearance1;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(187, 81);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 26);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance2.FontData.BoldAsString = "True";
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnSave.Appearance = appearance2;
            this.btnSave.Location = new System.Drawing.Point(95, 81);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 26);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Ok";
            this.btnSave.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblCashBackAmount
            // 
            appearance3.FontData.BoldAsString = "True";
            this.lblCashBackAmount.Appearance = appearance3;
            this.lblCashBackAmount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashBackAmount.Location = new System.Drawing.Point(10, 21);
            this.lblCashBackAmount.Name = "lblCashBackAmount";
            this.lblCashBackAmount.Size = new System.Drawing.Size(150, 15);
            this.lblCashBackAmount.TabIndex = 22;
            this.lblCashBackAmount.Text = "Cash Back Amount :";
            // 
            // numDiscAmount
            // 
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.SizeInPoints = 8F;
            this.numDiscAmount.Appearance = appearance4;
            this.numDiscAmount.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.numDiscAmount.Location = new System.Drawing.Point(167, 25);
            this.numDiscAmount.MaskInput = "{LOC}nn,nnn,nnn.nn";
            this.numDiscAmount.MaxValue = 999.99D;
            this.numDiscAmount.MinValue = 0D;
            this.numDiscAmount.Name = "numDiscAmount";
            this.numDiscAmount.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numDiscAmount.Size = new System.Drawing.Size(122, 20);
            this.numDiscAmount.TabIndex = 0;
            this.numDiscAmount.ValueChanged += new System.EventHandler(this.numDiscAmount_ValueChanged);
            // 
            // lblTransactionType
            // 
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance5.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance5;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(323, 32);
            this.lblTransactionType.TabIndex = 33;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Cash Back";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblLimit);
            this.groupBox1.Controls.Add(this.lblCashBackAmount);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.numDiscAmount);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(11, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 118);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lblLimit
            // 
            appearance6.FontData.BoldAsString = "True";
            this.lblLimit.Appearance = appearance6;
            this.lblLimit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLimit.Location = new System.Drawing.Point(10, 36);
            this.lblLimit.Name = "lblLimit";
            this.lblLimit.Size = new System.Drawing.Size(150, 15);
            this.lblLimit.TabIndex = 23;
            // 
            // frmPOSCashBack
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(323, 161);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSCashBack";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cash Back";
            this.Load += new System.EventHandler(this.frmPOSDicount_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPOSDicount_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numDiscAmount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmPOSDicount_Load(object sender, System.EventArgs e)
		{
						clsUIHelper.setColorSchecme(this);
			this.numDiscAmount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.numDiscAmount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.Cancel;
			this.Close();
		}

		private void frmPOSDicount_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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
                logger.Fatal(exp, "frmPOSDicount_KeyDown()");
                clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.OK;
			this.Close();
		}

		private void numDiscAmount_ValueChanged(object sender, System.EventArgs e)
		{
			if (Convert.ToDouble( this.numDiscAmount.Value)!=0)
			{
			}
		}
	}
}
