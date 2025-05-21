using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using POS.UI;
using POS_Core.CommonData;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmSSalesByVendor : Form
    {
        public frmSSalesByVendor()
        {
            InitializeComponent();
        }

        private void SearchVendor()
        {
            //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
            frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
            oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
            oSearch.ShowDialog();
            if (oSearch.IsCanceled) return;
            txtVendor.Text = oSearch.SelectedRowID();
        }
        private void txtVendor_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchVendor();
        }

        private void frmSSalesByVendor_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtVendor.ContainsFocus)
                    {
                        SearchVendor();
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
    }
}
