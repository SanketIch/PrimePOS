using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_Core_UI.UI
{
    public partial class frmAlert : Form
    {
        public frmAlert()
        {
            InitializeComponent();
        }

        public enum enmAction
        {
            wait,
            start,
            close
        }

        public enum enmType
        {
            Success,
            Warning,
            Error,
            Info
        }
        private frmAlert.enmAction action;

        private int x, y;

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch(this.action)
            {
                case enmAction.wait:
                    timer1.Interval = 7500;
                    action = enmAction.close;
                    break;
                case frmAlert.enmAction.start:
                    this.timer1.Interval = 1;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {
                        if (this.Opacity == 1.0)
                        {
                            action = frmAlert.enmAction.wait;
                        }
                    }
                    break;
                case enmAction.close:
                    timer1.Interval = 1;
                    this.Opacity -= 0.1;

                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            action = enmAction.close;
        }

        private void Form_Alert_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(lblMsg.Text);
        }

        public void showAlert(string msg, enmType type)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            for (int i = 1; i < 10; i++)
            {
                fname = "alert" + i.ToString();
                frmAlert frm = (frmAlert)Application.OpenForms[fname];

                if (frm == null)
                {
                    this.Name = fname;
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i - 5 * i;
                    this.Location = new Point(this.x, this.y);
                    break;

                }

            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;

            switch(type)
            {
                case enmType.Success:
                    this.pictureBox1.Image = Properties.Resources.Star; //PRIMEPOS-3207 need to change 
                    this.BackColor = Color.SeaGreen;
                    break;
                //case enmType.Error:
                //    this.pictureBox1.Image = Resources.error;
                //    this.BackColor = Color.DarkRed;
                //    break;
                //case enmType.Info:
                //    this.pictureBox1.Image = Resources.info;
                //    this.BackColor = Color.RoyalBlue;
                //    break;
                //case enmType.Warning:
                //    this.pictureBox1.Image = Resources.warning;
                //    this.BackColor = Color.DarkOrange;
                //    break;
            }


            this.lblMsg.Text = msg;

            this.Show();
            this.action = enmAction.start;
            this.timer1.Interval = 1; 
            this.timer1.Start();
        }

        #region PRIMEPOS-3292
        frmUrlView frmUrl;
        public string LaunchUrl { get; set; }
        public NotifyIcon notifyIcon;
        private void frmAlert_Click(object sender, System.EventArgs e)
        {
            if (frmUrl == null)
                frmUrl = new frmUrlView(this.LaunchUrl);
            else
                frmUrl._url = this.LaunchUrl;

            notifyIcon.Visible = false;

            frmUrl.Show();
            frmUrl.Focus();
        }
        #endregion
    }
}
