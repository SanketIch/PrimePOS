using System;
using System.Threading;
using System.Windows.Forms;

namespace PrimeRxPay
{
    public partial class DisplayMessage : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public DisplayMessage()
        {
            InitializeComponent();
        }

        private void DisplayMessage_Load(object sender, EventArgs e)
        {
            timer.Interval = 5000;
            timer.Tick += new EventHandler(FormClose_Timer);
            timer.Start();
        }
        private void FormClose_Timer(object sender, EventArgs e)
        {
            this.Close();
            timer.Dispose();
        }
    }
}
