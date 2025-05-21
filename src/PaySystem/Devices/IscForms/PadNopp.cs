using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice.IscForms
{
    public class PadNopp : iForm
    {
        /*#region Private Field
        bool bHasLineDisplay;

        #endregion*/

        #region Properties
        public List<FormElement> FormItems
        {
            get; set;
           
        }

        public string FormName
        {
            set;get;
        }

        public bool HasLineDisplay
        {
            set;get;
        }

        public bool HasSigBox
        {
            set;get;
        }

        public List<string> LineItemData
        {
            set;get;
        }

        public string SigBoxName
        {
            
            set;get;
        }

        #endregion

        #region Constructor
        public PadNopp()
        {
            this.FormName = "PADNOPP";
            this.FormItems = new List<FormElement>();
            this.HasLineDisplay = false;
            this.HasSigBox = false;
        }

        #endregion


        public void SetPatientName(string PatientName)
        {
            FormElement patientNameElement;
            patientNameElement.ElementName = "lblPatientName";
            patientNameElement.ElementData = PatientName;


            this.FormItems.Add(patientNameElement);
        }

        public void SetPatientAddress(string PatientAddress)
        {
            FormElement patientAddressElement;
            patientAddressElement.ElementName = "lblAddress";
            patientAddressElement.ElementData = PatientAddress;

            this.FormItems.Add(patientAddressElement);
        }

        public void SetPatientHippaText(string HippaText)
        {
            FormElement PatientHippaElement;
            PatientHippaElement.ElementName = "lblHippa";
            PatientHippaElement.ElementData = HippaText;

            this.FormItems.Add(PatientHippaElement);

        }

    }
}
