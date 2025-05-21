using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice.IscForms
{
    public class PadPatSelect:iForm
    {
        #region Properties
        public List<FormElement> FormItems
        {
            get; set;

        }

        public string FormName
        {
            set; get;
        }

        public bool HasLineDisplay
        {
            set; get;
        }

        public bool HasSigBox
        {
            set; get;
        }

        public List<string> LineItemData
        {
            set; get;
        }

        public string SigBoxName
        {

            set; get;
        }

        #endregion

        public PadPatSelect()
        {
            this.FormName = "PadPatSel";
            this.FormItems = new List<FormElement>();
            this.HasLineDisplay = false;
            this.HasSigBox = false;
        }
    }
}
