using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using POS.BusinessTier;
using POS_Core.BusinessTier;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.Resources;
//using POS_Core.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class frmLanguageTranslation : Form
    {
        #region Declaration
        public bool IsCancel = false;
        int iLanguageId = 0;
        string sDescription = string.Empty;
        string sToLanguage = string.Empty;
        Translator oTranslator = new Translator();
        DataTable dtLanguage = new DataTable();
        string sEnglishDescription = string.Empty;
        string sItemID = string.Empty;
        public frmItemSecondaryDescription parentForm = null;
        #endregion
        public frmLanguageTranslation()
        {
            InitializeComponent();
        }

        public int LanguageId
        {
            get { return iLanguageId; }
            set { iLanguageId = value; }
        }
        public string ItemID
        {
            get { return sItemID; }
            set { sItemID = value; }
        }
        public string Description
        {
            get { return sDescription; }
            set { sDescription = value; }
        }
        public string EnglishDescription
        {
            get { return sEnglishDescription; }
            set { sEnglishDescription = value; }
        }
        public string ToLanguage
        {
            get { return sToLanguage; }
            set { sToLanguage = value; }
        }
        public void SetNew()
        {
            this.txtFrom.Text = this.EnglishDescription;
            this.txtTo.Text = "";
        }

        public frmItemSecondaryDescription ParentForm
        {
            get { return parentForm; }
            set { parentForm = value; }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            IsCancel = true;
            this.Close();
        }
        private void LoadLanguage()
        {
            try
            {
                cmbFromLanguage.Items.Clear();
                cmbToLanguage.Items.Clear();
                Language oLanguage = new Language();
                dtLanguage = oLanguage.PopulateList();
                if (Configuration.isNullOrEmptyDataTable(dtLanguage) == true)
                {
                    clsUIHelper.ShowErrorMsg("Fail to populate languages.");
                    return;
                }
                foreach (DataRow oRow in dtLanguage.Rows)
                {
                    cmbFromLanguage.Items.Add(Configuration.convertNullToInt(oRow[clsPOSDBConstants.Language_Fld_ID]), Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Name]) + "(" + Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Code]) + ")");
                    cmbToLanguage.Items.Add(Configuration.convertNullToInt(oRow[clsPOSDBConstants.Language_Fld_ID]), Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Name]) + "(" + Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Code]) + ")");
                }
            }
            catch (Exception Ex)
            {

                clsUIHelper.ShowErrorMsg(Ex.Message);
            }

        }
        private void frmLanguageTranslation_Load(object sender, EventArgs e)
        {
            this.txtFrom.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtFrom.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtTo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtTo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cmbToLanguage.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbToLanguage.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cmbFromLanguage.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cmbFromLanguage.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.setColorSchecme(this);
            if (string.IsNullOrEmpty(this.txtTo.Text) == true)
            {
                LoadLanguage();
            }
            this.btnOK.Enabled = false;
            //this.cmbFromLanguage.SelectedIndex = 0; //PRIMEPOS-3164 01-Nov-2022 JY Commented
            cmbFromLanguage.SelectedIndex = SetLanguageCombo("1", cmbFromLanguage);  //PRIMEPOS-3164 01-Nov-2022 JY Added
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Validate() == true && Configuration.convertNullToInt(this.cmbToLanguage.Value) > 0)
            {
                ItemDescription oItemDescription = new ItemDescription();
                ItemDescriptionData oItemDescriptionData = oItemDescription.PopulateList("  ItemID = '" + ItemID + "' AND LanguageId = '" + Configuration.convertNullToInt(this.cmbToLanguage.Value) + "'");
                DataRow[] dr = ParentForm.oItemDescriptionData.ItemDescription.Select(" LanguageId = '" + Configuration.convertNullToInt(this.cmbToLanguage.Value) + "'");
                if (Configuration.isNullOrEmptyDataSet(oItemDescriptionData) == true && dr.Length == 0 && Description.Equals(this.txtTo.Text) == false)
                {
                    sDescription = this.txtTo.Text.Trim();
                    iLanguageId = Configuration.convertNullToInt(this.cmbToLanguage.Value);
                    IsCancel = false;
                    this.Close();
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Selected language description already present.");
                    this.IsCancel = true;
                }
            }
            else
            {
                this.IsCancel = true;
            }
        }

        private async System.Threading.Tasks.Task TranslateAsync()  //PRIMEPOS-3164 01-Nov-2022 JY Modified
        {
            string sToLanguageCode = string.Empty;
            string sFromLanguageCode = string.Empty;
            try
            {
                foreach (DataRow oRow in dtLanguage.Rows)
                {
                    if (Configuration.convertNullToInt(oRow[clsPOSDBConstants.Language_Fld_ID]) == Configuration.convertNullToInt(this.cmbFromLanguage.Value))
                    {
                        sFromLanguageCode = Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Code]);
                    }
                    if (Configuration.convertNullToInt(oRow[clsPOSDBConstants.Language_Fld_ID]) == Configuration.convertNullToInt(this.cmbToLanguage.Value))
                    {
                        sToLanguageCode = Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Code]);
                        sToLanguage = Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Name]);
                    }
                }
                string sTranslatedWord = await oTranslator.Translate(this.txtFrom.Text.Trim(), sFromLanguageCode, sToLanguageCode); //PRIMEPOS-3164 01-Nov-2022 JY Modified
                this.txtTo.Text = sTranslatedWord;
                this.btnOK.Enabled = true;
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private bool Validate()
        {
            string strErrorMSG = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(this.cmbFromLanguage.Text) == true)
                {
                    strErrorMSG += "Please select from language.\n";
                }
                if (string.IsNullOrEmpty(this.cmbToLanguage.Text) == true)
                {
                    strErrorMSG += "Please select to language.\n";
                }
                if (string.IsNullOrEmpty(this.cmbToLanguage.Text) == true)
                {
                    strErrorMSG += "Please use translation to get secondary description.\n";
                }
                if (string.IsNullOrEmpty(this.txtTo.Text) == true)
                {
                    strErrorMSG += "Please use translation to get secondary description.\n";
                }
                if (string.IsNullOrEmpty(strErrorMSG) == false)
                {
                    clsUIHelper.ShowErrorMsg(strErrorMSG);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
                return false;
            }
        }
        private void btnTranslate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(this.cmbToLanguage.Text) == true)
            {
                clsUIHelper.ShowErrorMsg("Please select to language.");
                return;
            }
            TranslateAsync();   //PRIMEPOS-3164 01-Nov-2022 JY Modified
            this.Cursor = Cursors.Default;
        }

        public void Edit(string SeconderyDescription, int LanguageId)
        {
            try
            {
                LoadLanguage();
                this.txtFrom.Text = EnglishDescription;
                this.txtTo.Text = SeconderyDescription;                
                //this.cmbFromLanguage.SelectedIndex = 0;
                //this.cmbToLanguage.SelectedIndex = LanguageId - 1;                
                cmbToLanguage.SelectedIndex = SetLanguageCombo(LanguageId.ToString(), cmbToLanguage);    //PRIMEPOS-3164 01-Nov-2022 JY Added
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        #region PRIMEPOS-3164 01-Nov-2022 JY Added
        private int SetLanguageCombo(string dataValue, UltraComboEditor cmbFromLanguage)
        {
            int SelectedIndex = 0;
            foreach (ValueListItem vlItem in cmbFromLanguage.Items)
            {                
                if (vlItem.DataValue.ToString() == dataValue)
                {
                    break;
                }
                SelectedIndex++;
            }
            return SelectedIndex;
        }
        #endregion

        private void cmbToLanguage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.btnOK.Enabled = false;
        }
    }
}
