using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice.IscForms
{
    public class PadPatCouncel:iForm
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
        public bool HideCouncel
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
        /// <summary>
        /// CTOR for Rxlist and Paitient confirmation form
        /// </summary>
        /// <param name="defaultAck">set True to Default the patent councelling to 'YES' and False for 'NO'</param>
        /// <param name="bHideCouncel">true to hide councelling section on the form and false to show councelling section</param>
        public PadPatCouncel(bool defaultAck, bool bHideCouncel=false)
        {
            if (defaultAck)
            {
                this.FormName = "PADRXLISTY";
                
            }
            else
            {
                this.FormName = "PADRXLISTN";
            }
            if (bHideCouncel)
                this.FormName = "PADRXLISTNC";
            this.FormItems = new List<FormElement>();
            this.HasLineDisplay = true;
            this.LineItemData = new List<string>();
            this.HasSigBox = false;
            this.HideCouncel = bHideCouncel;
        }

        #endregion

        public void SetPatientName(string PatientName)
        {
            FormElement patientNameElement;
            patientNameElement.ElementName = "RxPatientName";
            patientNameElement.ElementData = PatientName;


            this.FormItems.Add(patientNameElement);
        }

        public void SetRxCount(string Rxcount)
        {
            FormElement patientAddressElement;
            patientAddressElement.ElementName = "RxCount";
            patientAddressElement.ElementData = Rxcount;

            this.FormItems.Add(patientAddressElement);
        }

        public void SetCouncellingText(string Councelling)
        {
            if (HideCouncel)
                return;
            FormElement patientAddressElement;
            patientAddressElement.ElementName = "LblCouncelling";
            patientAddressElement.ElementData = Councelling;

            this.FormItems.Add(patientAddressElement);
        }

        public void SetRxCopay(string RxCopay)
        {
            FormElement patientAddressElement;
            patientAddressElement.ElementName = "RxCopay";
            patientAddressElement.ElementData = RxCopay;

            this.FormItems.Add(patientAddressElement);
        }

        public void AddToLineDisplay(string lineitemData)
        {
            this.LineItemData.Add(lineitemData);
        }
    }
}
