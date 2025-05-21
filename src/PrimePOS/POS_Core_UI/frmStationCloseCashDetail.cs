//-----------------------------------------------------------------------------------------------------------
//Added By Shitaljit(QuicSolv) on 16 June 2011
//For Station Close Cash Entry Summary.
//-----------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
    public partial class frmStationCloseCashDetail : Form
    {
        StationClose oStationClose = new StationClose();
        int StationCloseID;
        public frmStationCloseCashDetail(int StCloseID)
        {
            StationCloseID = StCloseID;
            InitializeComponent();
        }

        private void Display()
        {
			/*Date 27/01/2014
			 * Modified By Shitaljit
			 * for dynamic currency 
			 */
			// old code
			//lblStationCloseDetail.Text = "           Station Close#:  " + StationCloseID.ToString() + "       Station#: " + Configuration.StationName + "         USER:   " + Configuration.UserName.Trim();
			//lblTotalCashEnter.Text = frmStationCloseCash.GrandTotal.ToString("$######0.00");
			//lblTotalCC.Text = frmStationCloseCash.CCAmount.ToString("$######0.00");
			//lblTotalCheck.Text = frmStationCloseCash.CheckAmount.ToString("$######0.00");
			//lblTotalCoupons.Text = frmStationCloseCash.CouponTotal.ToString("$######0.00");
			//lblTotalEBT.Text = frmStationCloseCash.EBTAmount.ToString("$######0.00");
			//lblTotalPayout.Text = frmStationCloseCash.PayoutAmount.ToString("$######0.00");
			//lblTotalROA.Text = frmStationCloseCash.ROAAmount.ToString("$######0.00");
            
			//new code
            lblStationCloseDetail.Text = "           Station Close#:  " + StationCloseID.ToString() + "       Station#: " + Configuration.StationName + "         USER:   " + Configuration.UserName.Trim();
            lblTotalCashEnter.Text = frmStationCloseCash.GrandTotal.ToString(Configuration.CInfo.CurrencySymbol.ToString()+"######0.00");
			lblTotalCC.Text = frmStationCloseCash.CCAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "######0.00");
			lblTotalCheck.Text = frmStationCloseCash.CheckAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "######0.00");
			lblTotalCoupons.Text = frmStationCloseCash.CouponTotal.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "######0.00");
			lblTotalEBT.Text = frmStationCloseCash.EBTAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "######0.00");
			lblTotalPayout.Text = frmStationCloseCash.PayoutAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "######0.00");
			lblTotalROA.Text = frmStationCloseCash.ROAAmount.ToString(Configuration.CInfo.CurrencySymbol.ToString() + "######0.00");
            
        }

        private void frmStationCloseCashDetail_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            Display();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            frmStationCloseCash.FlagCloseStationForm = false;
            this.Close();
        }
    }
}