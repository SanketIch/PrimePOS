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
    public partial class frmEvertecManual : Form
    {
        public bool isCancel = false;
        public bool isManual = false;
        public bool isSwipe = false;    
        public frmEvertecManual()
        {
            InitializeComponent();
            this.btnSwipe.Select() ;
        }

        private void btnSwipe_Click(object sender, EventArgs e)
        {
            isSwipe = true;
            this.Close();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            isManual = true;
            this.Close();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();

        }
    }
}
