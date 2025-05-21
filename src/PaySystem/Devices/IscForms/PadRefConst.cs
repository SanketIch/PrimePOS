using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDevice.IscForms
{
    // PRIMEPOS-2868 Added new class for Default or other consent
    public class PadRefConst : iForm
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

        public PadRefConst(int numOptions = 4)
        {
            this.FormName = "PADREFCONST";
            if (numOptions == 2)
                this.FormName = "PADREFCONS2";
            else if (numOptions == 3)
                this.FormName = "PADREFCONS3";
            this.FormItems = new List<FormElement>();
            this.HasLineDisplay = false;
            this.HasSigBox = false;
        }

        public void SetTitle(string Title)
        {
            FormElement FormTitle;
            FormTitle.ElementName = "lblConsTitle";
            FormTitle.ElementData = Title;
            this.FormItems.Add(FormTitle);
        }
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

        public void SetConsentText(string ConsentText)
        {
            FormElement ConsentElement;
            ConsentElement.ElementName = "lblConsent";
            ConsentElement.ElementData = ConsentText;
            this.FormItems.Add(ConsentElement);
        }
        #region PRIMEPOS-3192
        public void SetDrugNameText(string DrugNameText)
        {
            FormElement ConsentElement;
            ConsentElement.ElementName = "lblDrugName";
            ConsentElement.ElementData = DrugNameText;
            this.FormItems.Add(ConsentElement);
        }
        #endregion

        public void SetConsentRadioButton1Text(string RadioButton1Text)
        {
            FormElement rdbBtn1Text;
            rdbBtn1Text.ElementName = "rbtnRep1";
            rdbBtn1Text.ElementData = RadioButton1Text;
            this.FormItems.Add(rdbBtn1Text);
        }

        public void SetConsentRadioButton2Text(string RadioButton2Text)
        {
            FormElement rdbBtn2Text;
            rdbBtn2Text.ElementName = "rbtnRep2";
            rdbBtn2Text.ElementData = RadioButton2Text;
            this.FormItems.Add(rdbBtn2Text);
        }

        public void SetConsentRadioButton3Text(string RadioButton3Text)
        {
            FormElement rdbBtn3Text;
            rdbBtn3Text.ElementName = "rbtnRep3";
            rdbBtn3Text.ElementData = RadioButton3Text;
            this.FormItems.Add(rdbBtn3Text);
        }
        public void SetConsentRadioButton4Text(string RadioButton4Text)
        {
            FormElement rdbBtn4Text;
            rdbBtn4Text.ElementName = "rbtnRep4";
            rdbBtn4Text.ElementData = RadioButton4Text;
            this.FormItems.Add(rdbBtn4Text);
        }
        public void SetConsentButtonText1(string Button1Text)
        {
            FormElement btn1Text;
            btn1Text.ElementName = "BUTTONTEXT1";
            btn1Text.ElementData = Button1Text;
            this.FormItems.Add(btn1Text);
        }
        public void SetConsentButtonText2(string Button2Text)
        {
            FormElement btn2Text;
            btn2Text.ElementName = "BUTTONTEXT2";
            btn2Text.ElementData = Button2Text;
            this.FormItems.Add(btn2Text);
        }
        public void SetConsentButtonText3(string Button3Text)
        {
            FormElement btn3Text;
            btn3Text.ElementName = "BUTTONTEXT3";
            btn3Text.ElementData = Button3Text;
            this.FormItems.Add(btn3Text);

        }
        public void SetConsentRefuseButtonText4(string Button4Text)
        {
            FormElement btn4Text;
            btn4Text.ElementName = "BUTTONTEXT4";
            btn4Text.ElementData = Button4Text;
            this.FormItems.Add(btn4Text);
        }
    }
}
