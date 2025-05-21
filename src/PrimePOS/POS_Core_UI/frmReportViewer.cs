using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmReportViewer.
	/// </summary>
	public class frmReportViewer : System.Windows.Forms.Form
	{
		public CrystalDecisions.Windows.Forms.CrystalReportViewer rvReportViewer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private static frmReportViewer defaultInstance;
       

		public frmReportViewer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
            defaultInstance = this;
		}

        public static frmReportViewer DefaultInstance
        {
            get
            {
                //if (defaultInstance == null)
                //{
                //    defaultInstance = new frmReportViewer();
                //}
                return defaultInstance;
            }
            set
            {
                defaultInstance = value;
            }
        }
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (defaultInstance != null)
                {
                    defaultInstance.Dispose();
                }
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
            this.rvReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // rvReportViewer
            // 
            this.rvReportViewer.ActiveViewIndex = -1;
            this.rvReportViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rvReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rvReportViewer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rvReportViewer.Location = new System.Drawing.Point(5, 11);
            this.rvReportViewer.Name = "rvReportViewer";
            this.rvReportViewer.SelectionFormula = "";
            this.rvReportViewer.Size = new System.Drawing.Size(370, 262);
            this.rvReportViewer.TabIndex = 0;
            this.rvReportViewer.ViewTimeSelectionFormula = "";
            // 
            // frmReportViewer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(378, 273);
            this.Controls.Add(this.rvReportViewer);
            this.Name = "frmReportViewer";
            this.Shown += new System.EventHandler(this.frmReportViewer_Shown);
            this.Activated += new System.EventHandler(this.frmReportViewer_Activated);
            this.Load += new System.EventHandler(this.frmReportViewer_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmReportViewer_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);
		}

		private void frmReportViewer_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

        private void frmReportViewer_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
        }
	}
}
