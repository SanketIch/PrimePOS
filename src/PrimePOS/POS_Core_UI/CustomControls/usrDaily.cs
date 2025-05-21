using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class usrDaily : UserControl, IScheduledControl
    {
        public usrDaily()
        {
            InitializeComponent();
            this.txtRecur.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtRecur.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
        }

        public void SetFocusControl()
        {
            txtRecur.Focus();
        }

        public Object getObject()
        {
            return this.txtRecur.Value;
        }

        public void SetObject(Object obj)
        {
            this.txtRecur.Value = (string)obj.ToString();
        }

        public Control getControl()
        {
            return this;
        }

        public void save(int itaskiD)
        {
        }

        public bool checkValidation()
        {
            int iDays = Convert.ToInt32(txtRecur.Value);
            if (iDays < 1 && iDays > 999)
            {
                errorProvider.SetError(txtRecur, "Days Can’t Be Less Than 0 Or Greater Than 999."); 
                return false;
            }
            return true;
        }
    }
}
