using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using NLog;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPOSChangeDue.
	/// </summary>
	public class frmPOSGetDebitCard : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private CbDbf.CCbDbf oCCDbf;
		private System.Threading.Thread oThread;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmPOSGetDebitCard(CbDbf.CCbDbf oDbf)
		{
			InitializeComponent();
			this.oCCDbf=oDbf;
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOSGetDebitCard));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 114);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnClose
            // 
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(172, 70);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ultraLabel1
            // 
            appearance2.FontData.Name = "Arial";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(47, 30);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(370, 26);
            this.ultraLabel1.TabIndex = 5;
            this.ultraLabel1.Text = "Please Enter Your Pin Code";
            // 
            // frmPOSGetDebitCard
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(484, 131);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOSGetDebitCard";
            this.Load += new System.EventHandler(this.frmPOSGetDebitCard_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmPOSGetDebitCard_Load(object sender, System.EventArgs e)
		{
            logger.Trace("frmPOSGetDebitCard_Load(object sender, System.EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Entering);
            this.Left=100;		//( frmMain.getInstance().Width-this.Width)/2;
			this.Top=100;			//(frmMain.getInstance().Height-this.Height)/2;
			clsUIHelper.setColorSchecme(this);
			this.Show();
			oThread=new System.Threading.Thread(new System.Threading.ThreadStart(checkResponse));
			oThread.Start();
            logger.Trace("frmPOSGetDebitCard_Load(object sender, System.EventArgs e) - " + POS_Core.CommonData.clsPOSDBConstants.Log_Exiting);
        }

		private void checkResponse()
		{
			String sClaimStat= oCCDbf.GetValString("CLAIMSTAT");
						
			while(sClaimStat.Trim()=="P")
			{
				System.Threading.Thread.Sleep(1000);
				oCCDbf.db.refreshRecord();
				sClaimStat= oCCDbf.GetValString("CLAIMSTAT");
			}
			this.DialogResult=DialogResult.OK;
			this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
