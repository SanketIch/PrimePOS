using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice.IscForms
{
    public class PadPatSign:iForm
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

        #region Constructor

        public PadPatSign()
        {
            this.FormName = "PADPATSIGN";
            this.FormItems = new List<FormElement>();
            this.HasLineDisplay = false;
            this.HasSigBox = true;
            this.SigBoxName = "sig";
        }

        #endregion


        public void SetPatientName(string PatientName)
        {
            FormElement patientNameElement;
            patientNameElement.ElementName = "lblPatientName";
            patientNameElement.ElementData = PatientName;


            this.FormItems.Add(patientNameElement);
        }
    }
}
