using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  FirstMile;

namespace FirstMileTest.ViewModel
{
    public abstract class BaseViewModel
    {
        #region Closing Request Event
        public event EventHandler ClosingRequest;

        protected void OnClosingRequest()
        {
            if (this.ClosingRequest != null)
            {
                this.ClosingRequest(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Hiding Request Event
        public event EventHandler HidingRequest;
        protected void OnHidingRequest()
        {
            if (this.HidingRequest != null)
            {
                this.HidingRequest(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Show Request Event
        public event EventHandler ShowRequest;
        protected void onShowRequest()
        {
            if (this.ShowRequest != null)
            {
                this.ShowRequest(this, EventArgs.Empty);
            }
        }
        #endregion
        public BaseViewModel()
        {
            GatewaySettings.AccountId = "MPTST";//WYWVM- "MPTST" -      "EMVTS"          WYWVM
            GatewaySettings.SubId = "MMRCH";// MICRO - "BILL0" MMRCH    "ROHIT"         WLISO
            GatewaySettings.MerchantPin = "1234567890";//1234567890                        mmsdemo
        }
    }
}
