using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MMSAppUpdater
{
    internal partial class frmNewUpdatesPopup : Form
    {
        private bool m_bUpdatesAvail = false;
        ContNewUpdatesPopup oContPopUp = null;
       
        Form ofrm = null;
        public frmNewUpdatesPopup(clsUpdateManager oManagerObject, Form oParentForm, bool bUpdatesAvail)
        {
            m_bUpdatesAvail = bUpdatesAvail;
            InitializeComponent();
            ofrm = oParentForm;

            oContPopUp = new ContNewUpdatesPopup(oManagerObject);
        }

        public void AddRoundRect(GraphicsPath gp, float x, float y, float width, float height, float radius)
        {
            gp.AddLine(x + radius, y, x + width - (radius * 2), y); // Line
            gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90); // Corner
            gp.AddLine(x + width, y + radius, x + width, y + height - (radius * 2)); // Line
            gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90); // Corner
            gp.AddLine(x + width - (radius * 2), y + height, x + radius, y + height); // Line
            gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90); // Corner
            gp.AddLine(x, y + height - (radius * 2), x, y + radius); // Line
            gp.AddArc(x, y, radius * 2, radius * 2, 180, 90); // Corner
        }

        private void frmNewUpdatesPopup_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath shape = new System.Drawing.Drawing2D.GraphicsPath();
            AddRoundRect(shape,0, 0, this.Width-20, this.Height-20,4);
            shape.AddPolygon(new Point[] { new Point(10, this.Height - 20),new Point(30, this.Height - 20), new Point(5, this.Height ) });

            this.Region = new System.Drawing.Region(shape);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNewUpdatesPopup_Load(object sender, EventArgs e)
        {
            this.Location = new Point(ofrm.Width-this.Width, ofrm.Height-(this.Height+10));
            //txtUpdates.Text= oContPopUp.GetUpdatedString().Trim();
            timer1.Start();
        }
       
        private void frmNewUpdatesPopup_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
         
            if (m_bUpdatesAvail)
            {
                oContPopUp.LoadFrmUpdate(ofrm);
            }
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            frmNewUpdatesPopup_MouseClick(null, null);
        }

        private void txtUpdates_Click(object sender, EventArgs e)
        {
            frmNewUpdatesPopup_MouseClick(null, null);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            frmNewUpdatesPopup_MouseClick(null, null);
        }

        private void txtUpdates_ValueChanged(object sender, EventArgs e)
        {
        }

        private void txtUpdates_MouseClick(object sender, MouseEventArgs e)
        {
            frmNewUpdatesPopup_MouseClick(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                for (int i = 0; i < 300; i++)
                {
                    this.Opacity--;
                    System.Threading.Thread.Sleep(10);
                }
                this.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void label4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNewUpdatesPopup_MouseClick(null, null);
        }
    }
}