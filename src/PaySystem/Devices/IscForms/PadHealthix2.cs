using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice.IscForms
{
    public class PadHealthix2:iForm
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

        public PadHealthix2()
        {
            this.FormName = "PadTerm2";
            this.FormItems = new List<FormElement>();
            this.HasLineDisplay = false;
            this.HasSigBox = false;
        }

        public void SetPharmacyName(string strPharmacyName)
        {
            FormElement pharmacyNameElement;
            pharmacyNameElement.ElementName = "PharmacyName";
            pharmacyNameElement.ElementData = strPharmacyName+" Only";

            this.FormItems.Add(pharmacyNameElement);
        }
    }
}
