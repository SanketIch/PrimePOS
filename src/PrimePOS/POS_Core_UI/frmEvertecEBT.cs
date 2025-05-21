using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmEvertecEBT : Form
    {
        public bool IsCash = false;
        public bool IsFood = false;
        public frmEvertecEBT()
        {
            InitializeComponent();
        }

        private void frmEvertecEBT_Load(object sender, EventArgs e)
        {
            
        }

        private void btnFood_Click(object sender, EventArgs e)
        {
            IsFood = true;
            this.Close();
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            IsCash = true;
            this.Close();
        }
    }
}
