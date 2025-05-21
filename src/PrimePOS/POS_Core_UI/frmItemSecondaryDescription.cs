using POS.BusinessTier;
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
using Infragistics.Win.UltraWinGrid;

namespace POS_Core_UI
{
    public partial class frmItemSecondaryDescription : Form
    {
        #region Declaration
        public bool IsCancel = false;
        ItemDescription oItemDescription = new ItemDescription();
        ItemDescriptionRow oItemDescriptionRow = null;
        public ItemDescriptionData oItemDescriptionData = new ItemDescriptionData();
        private string sItemID= string.Empty;
        private string sDescription = string.Empty;
        frmLanguageTranslation ofrmLangTranslator = new frmLanguageTranslation();
        #endregion
        public frmItemSecondaryDescription()
        {
            InitializeComponent();
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

        private void Add()
        {
            try
            {
                ofrmLangTranslator.EnglishDescription = Description;
                ofrmLangTranslator.ItemID = ItemID;
                ofrmLangTranslator.SetNew();
                ofrmLangTranslator.parentForm = this;
                ofrmLangTranslator.ShowDialog();
                if (ofrmLangTranslator.IsCancel == false)
                {
                    grdDetail.BeginUpdate();
                    this.grdDetail.Rows.Band.AddNew();
                    this.grdDetail.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                    this.grdDetail.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.LastRowInGrid);
                    oItemDescriptionRow.ID = oItemDescriptionData.ItemDescription.GetNextID();
                    oItemDescriptionRow.LanguageId = ofrmLangTranslator.LanguageId;
                    oItemDescriptionRow.Description = ofrmLangTranslator.Description;
                    oItemDescriptionRow.UserID = Configuration.UserName;
                    oItemDescriptionRow.ItemID = ItemID;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemDescription_Fld_ID].Value = oItemDescriptionRow.ID;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemDescription_Fld_Description].Value = oItemDescriptionRow.Description;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemDescription_Fld_LanguageId].Value = oItemDescriptionRow.LanguageId;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.Language_tbl].Value = ofrmLangTranslator.ToLanguage;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemDescription_Fld_ItemID].Value = ItemID;
                    ApplyGridFormate();
                    grdDetail.UpdateData();
                    grdDetail.EndUpdate();
                }

            }
            catch (Exception Ex)
            {

                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void SetNew()
        {
            oItemDescriptionRow = oItemDescriptionData.ItemDescription.AddRow(0, "", "", 0, "");
           
        }

        private void frmItemSecondaryDescription_Load(object sender, EventArgs e)
        {
            this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.SetAppearance(this.grdDetail);
            clsUIHelper.SetKeyActionMappings(this.grdDetail);
            clsUIHelper.setColorSchecme(this);
            this.grdDetail.DisplayLayout.Bands[0].Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.lblItemDetail.Text = "Item Code:   " + ItemID + "      Description:   " + Description;
            SetNew();
            LoadDescriptions();            
        }
        private void ApplyGridFormate()
        {
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemDescription_Fld_ID].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemDescription_Fld_UserID].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemDescription_Fld_LanguageId].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ItemDescription_Fld_ItemID].Hidden = true;
        }
        private void LoadDescriptions()
        {
            try
            {
                oItemDescriptionData.Tables.Clear();
                oItemDescriptionData = oItemDescription.PopulateList("  ItemID = '" + ItemID + "'");
                this.grdDetail.DataSource = oItemDescriptionData;
                ApplyGridFormate();
                if (this.grdDetail.Rows.Count > 0)
                {
                    this.grdDetail.Rows[0].Selected = true;
                    grdDetail.ActiveRow = grdDetail.Rows[0];    //PRIMEPOS-3164 01-Nov-2022 JY Added
                    //this.grdDetail.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;  //PRIMEPOS-3164 01-Nov-2022 JY Added
                }                
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        private bool Save()
        {
            bool RetVal = false;
            try
            {
                if (grdDetail.Rows.Count > 0)
                {
                    oItemDescription.Persist(oItemDescriptionData);
                    RetVal = true;
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Please add secondary descriptions.");
                    RetVal = false;
                }
            }
            catch (Exception Ex)
            {
                RetVal = false;
                throw Ex;
            }
            return RetVal;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Save())
                {
                    clsUIHelper.ShowSuccessMsg("Secondary description(s) saved successfully.", "Save");
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Delete()
        {
            try
            {
                grdDetail.BeginUpdate();
                this.grdDetail.ActiveRow.Delete(false);
                grdDetail.EndUpdate();
                grdDetail.Update();
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdDetail.Rows.Count == 0)
            {
                return;
            }
            if (grdDetail.Selected.Rows.Count == 0)
            {
                clsUIHelper.ShowErrorMsg("Please select a row to delete.");
                return;
            }
            if (Resources.Message.Display("Are you sure, you want to delete selected description?", "Delete secondary Description", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Delete();
            }
        }
        private void Edit()
        {
            try
            {
                int LanguageId = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemDescription_Fld_LanguageId].Value);
                string ssecondaryDescription = Configuration.convertNullToString(grdDetail.ActiveRow.Cells[clsPOSDBConstants.ItemDescription_Fld_Description].Value);
                ofrmLangTranslator.EnglishDescription = Description;
                ofrmLangTranslator.Edit(ssecondaryDescription,LanguageId);
                ofrmLangTranslator.parentForm = this;
                ofrmLangTranslator.ShowDialog();
                if (ofrmLangTranslator.IsCancel == false)
                {
                    grdDetail.BeginUpdate();
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemDescription_Fld_Description].Value = ofrmLangTranslator.Description;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.ItemDescription_Fld_LanguageId].Value = ofrmLangTranslator.LanguageId;
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.Language_tbl].Value = ofrmLangTranslator.ToLanguage;
                    grdDetail.UpdateData();
                    grdDetail.EndUpdate();
                }
            }
            catch (Exception Ex)
            {
                
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdDetail.Rows.Count == 0)
            {
                return;
            }
            if (grdDetail.ActiveRow == null)
            {
                clsUIHelper.ShowErrorMsg("Please select a row to edit.");
                return;
            }
            Edit();
        }
    }
}
