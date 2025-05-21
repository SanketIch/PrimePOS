#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

#endregion

namespace POS_Core.Resources
{
	public class ProgressPanel : UserControl
	{
		public ProgressPanel()
		{
			InitializeComponent();
        }
        #region Events
        private void pnlProgress_Resize(object sender, EventArgs e)
		{
		
		}
		private void ProgressPanel_Resize(object sender, EventArgs e)
		{
        }
        #endregion
        #region Methods
        public void Start()
		{
			this.Visible = true;
			this.BringToFront();
			this.activityBar.Enabled = true;
            this.activityBar.Start();
            
            
		}
		public void Stop()
		{
            //while (activityBar.Value < activityBar.Maximum) ;
			this.activityBar.Enabled = false;
            this.Visible = false;
            this.SendToBack();
            this.activityBar.Stop();
        }
        #endregion
        #region Properties
        public string Title
		{
			get { return lblProgressMessage.Text; }
			set { lblProgressMessage.Text = value; }
        }
        #endregion

        private System.ComponentModel.IContainer components = null;
        public Label lblProgressMessage;
		private POS_Core_UI.UI.ActivityBar activityBar;
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblProgressMessage = new System.Windows.Forms.Label();
            this.activityBar = new POS_Core_UI.UI.ActivityBar();
            this.SuspendLayout();
            // 
            // lblProgressMessage
            // 
            this.lblProgressMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgressMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressMessage.Location = new System.Drawing.Point(3, 0);
            this.lblProgressMessage.Name = "lblProgressMessage";
            this.lblProgressMessage.Size = new System.Drawing.Size(458, 29);
            this.lblProgressMessage.TabIndex = 20;
            this.lblProgressMessage.Text = "Loading...";
            this.lblProgressMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // activityBar
            // 
            this.activityBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.activityBar.BackColor = System.Drawing.Color.Transparent;
            this.activityBar.BarColor = System.Drawing.Color.Orange;
            this.activityBar.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.activityBar.HighlightColor = System.Drawing.Color.White;
            this.activityBar.Location = new System.Drawing.Point(3, 37);
            this.activityBar.Name = "activityBar";
            this.activityBar.Size = new System.Drawing.Size(452, 22);
            this.activityBar.TabIndex = 21;
            // 
            // ProgressPanel
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.activityBar);
            this.Controls.Add(this.lblProgressMessage);
            this.Name = "ProgressPanel";
            this.Size = new System.Drawing.Size(458, 63);
            this.Resize += new System.EventHandler(this.ProgressPanel_Resize);
            this.ResumeLayout(false);

		}

		#endregion

		

    }
}
