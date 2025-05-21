using System;
using System.Windows.Forms;

namespace POS_Core_UI.UI
{
    public partial class frmNotify : Form
    {
        public frmNotify()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipText = "Click here to see the details";
            notifyIcon1.BalloonTipTitle = "Sample Title";
            notifyIcon1.Text = "Sample text";
            notifyIcon1.ShowBalloonTip(100);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            MessageBox.Show("Hello Everyone");
        }
    }
}
