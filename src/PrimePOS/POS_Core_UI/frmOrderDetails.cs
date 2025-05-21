using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmOrderDetails : Form
    {
        private String orderDescription = String.Empty;       
        public frmOrderDetails()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
        public String OrderDescription
        {
            get
            {
                return this.orderDescription; 
            }
        }     
        private void btnOK_Click(object sender, EventArgs e)
        {
            orderDescription = this.txtOrdDescription.Text.Trim();
            this.Close(); 
        }
        private void frmOrderDetails_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);  
        }
        private void frmOrderDetails_Shown(object sender, EventArgs e)
        {
            this.txtOrdDescription.Focus(); 
        }
        private void txtOrdDescription_KeyDown(object sender, KeyEventArgs e)
        {
            System.EventArgs arg = new EventArgs();
            if (e.KeyCode == Keys.Enter)
            {          
                this.btnOK_Click(this.btnOK, arg); 
            }
        }
    }
}