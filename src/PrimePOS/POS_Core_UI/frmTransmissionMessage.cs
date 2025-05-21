using Infragistics.Win.UltraWinGrid;
using System;
using System.Data;
using System.Windows.Forms;
using POS_Core.CommonData;
namespace POS_Core_UI
{
    public partial class frmTransmissionMessage : Form
    {
        //Add Shortcut key for Add Cards
        #region Variable declaration
        public bool IsCanceled = false;
        #endregion
        public frmTransmissionMessage()
        {
            InitializeComponent();
        }
        public frmTransmissionMessage(string SendMessage, string RecieveMessage)
        {
            InitializeComponent();
            DisplayMessage(SendMessage, RecieveMessage);
        }

        #region Control Events

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            IsCanceled = true;
        }

        #endregion

        #region Form Events

        private void frmAddSolutranCards_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region Method
        private void DisplayMessage(string SendMessage, string RecieveMessage)
        {
            txtSendMessage.Text = SendMessage;
            txtRecieveMessage.Text = RecieveMessage;
        }
        #endregion

        private void frmTransmissionMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}
