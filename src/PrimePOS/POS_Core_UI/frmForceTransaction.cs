using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.UI
{
    public partial class frmForceTransaction : Form
    {
        public bool isAllowDuplicate = false;
        public bool isCancel = false;
        public frmForceTransaction()
        {
            InitializeComponent();
        }

        private void btnForce_Click(object sender, EventArgs e)
        {
            isAllowDuplicate = true;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
        }
    }
}
