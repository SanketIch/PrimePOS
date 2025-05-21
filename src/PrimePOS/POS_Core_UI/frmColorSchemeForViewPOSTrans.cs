using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData;
using Infragistics.Win.UltraWinGrid;
//using POS_Core.DataAccess;
using NLog;
using POS_Core.Resources;
using POS_Core_UI.UI;

namespace POS_Core_UI
{
    public partial class frmColorSchemeForViewPOSTrans : Form
    {
        #region Declaration 
        ColorSchemeForViewPOSTrans oClrScmForPOSTrans = new ColorSchemeForViewPOSTrans();
        ColorSchemeForViewPOSTransData oClrScmForPOSTransData = new ColorSchemeForViewPOSTransData();
        ColorSchemeForViewPOSTransRow oClrScmForPOSTransRow = null;
        ColorSchemeForViewPOSTransTable oClrScmForPOSTransTable = new ColorSchemeForViewPOSTransTable();
        bool m_FromError =false;
        bool isAddMode = false;
        bool isEditMode = false;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        #endregion 

        public frmColorSchemeForViewPOSTrans()
        {
            InitializeComponent();
            SetNew();
        }

        private void Save()
        {
            try
            {
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Entering);

                oClrScmForPOSTransRow.FromAmount = Configuration.convertNullToDecimal(numFromAmount.Value);
                oClrScmForPOSTransRow.ToAmount = Configuration.convertNullToDecimal(numToAmount.Value);
                oClrScmForPOSTransRow.ForeColor =Configuration.convertNullToString(cpForeColor.Value);
                oClrScmForPOSTransRow.BackColor =Configuration.convertNullToString(cpBackColor.Value);

                oClrScmForPOSTrans.Persist(oClrScmForPOSTransData);
                GetData();
                Clear();
                this.grColorScemeDetails.Enabled = false;
                ApplyGrigFormat();
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void ApplyGrigFormat()
        {
            try
            {
                grdDetail.DisplayLayout.Override.DefaultRowHeight = 40;
                grdDetail.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
                grdDetail.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
                grdDetail.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
                grdDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
                grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor].CellActivation = Activation.NoEdit;
                grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor].CellActivation = Activation.NoEdit; 

                if (grdDetail.DisplayLayout.Bands[0].Columns.Contains(clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID))
                {
                    grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID].Hidden = true;
                }
                if (grdDetail.DisplayLayout.Bands[0].Columns.Contains(clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_UserID))
                {
                    grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_UserID].Hidden = true;
                }
                grdDetail.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ApplyGrigFormat()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GetData()
        {
            try
            {
                oClrScmForPOSTransData = oClrScmForPOSTrans.PopulateList("");
                grdDetail.DataSource = oClrScmForPOSTransData;
                sbMain.Panels[0].Text = "Record(s) Count = " + grdDetail.Rows.Count;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetData()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            
        }
        private void ValidateAmountRange(decimal FromAmount, decimal ToAmount)
        {
            logger.Trace("ValidateAmountRange(decimal FromAmount, decimal ToAmount) - " + clsPOSDBConstants.Log_Entering);
            if (grdDetail.Selected.Rows.Count == 0 || m_FromError == true)
            {
                return;
            }
            string whereclause = "";
             Int32 Id = 0;
             if (isEditMode == true)
             {
                 Id = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID].Value);
                 whereclause = " WHERE ID <> " + Id;
             }
            ColorSchemeForViewPOSTransData ds = oClrScmForPOSTrans.PopulateList(whereclause);
            foreach (ColorSchemeForViewPOSTransRow oRow in ds.ColorSchemeForViewPOSTrans.Rows)
            {
                if ((FromAmount >= ToAmount && ToAmount > 0) || (FromAmount > 0 && FromAmount >= oRow.FromAmount && FromAmount <= oRow.ToAmount) ||
                    (ToAmount > 0 && ToAmount <= oRow.ToAmount && ToAmount >= oRow.FromAmount))
                {
                    throw new Exception("Amount range is already defined.");
                }
            }
            logger.Trace("ValidateAmountRange(decimal FromAmount, decimal ToAmount) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void Clear()
        {
            this.numFromAmount.Value = 0;
            this.numToAmount.Value = 0;
            this.cpBackColor.Value = cpBackColor.DefaultColor;
            this.cpForeColor.Value = cpForeColor.DefaultColor;
            isEditMode = false;
            isAddMode = false;
            m_FromError = false;
        }

        private void SetNew()
        {
            oClrScmForPOSTransData = new ColorSchemeForViewPOSTransData();
            Clear();
            oClrScmForPOSTransRow = oClrScmForPOSTransData.ColorSchemeForViewPOSTrans.AddRow(0, 0, 0, "", "");
        }

        private void frmColorSchemeForViewPOSTrans_Load(object sender, EventArgs e)
        {
            this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numFromAmount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numFromAmount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.numToAmount.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numToAmount.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cpForeColor.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cpForeColor.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.cpBackColor.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.cpBackColor.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.SetKeyActionMappings(this.grdDetail);
            clsUIHelper.SetAppearance(this.grdDetail);
            clsUIHelper.setColorSchecme(this);
            GetData();
            ApplyGrigFormat();
        }

        private void grdDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.grdDetail.ActiveCell == null) return;

            if (e.KeyData == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);

            }
        }

        private void grdDetail_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            m_FromError = true;
        }

        private void grdDetail_Validated(object sender, EventArgs e)
        {
            grdDetail.PerformAction(UltraGridAction.LastCellInGrid);
        }

      

        private void btnNewNote_Click(object sender, EventArgs e)
        {
            SetNew();
            this.grColorScemeDetails.Enabled = true;
            this.numFromAmount.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }
       
        private void Add()
        {
            try
            {
                logger.Trace("Add() - " + clsPOSDBConstants.Log_Entering);
                if (grdDetail.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(this.grdDetail.Rows[grdDetail.Rows.Count - 1].Cells[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount].Value.ToString()))
                    {
                        this.grdDetail.ActiveCell = this.grdDetail.Rows[grdDetail.Rows.Count - 1].Cells[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount];
                        return;
                    }
                }
                isAddMode = true;
                this.grdDetail.DisplayLayout.Bands[0].AddNew();
                grdDetail.Update();
                this.grdDetail.Focus();
                this.grdDetail.ActiveCell = this.grdDetail.Rows[grdDetail.Rows.Count - 1].Cells[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount];
                logger.Trace("Add() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Add()");
                this.grdDetail.ActiveCell = this.grdDetail.Rows[grdDetail.Rows.Count - 1].Cells[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount];
            }
        }
        private void Edit()
        {
            try
            {
                logger.Trace("Edit() - " + clsPOSDBConstants.Log_Entering);

                if (grdDetail.Selected.Rows.Count == 0)
                {
                    return;
                }
                isEditMode = true;
                frmPOSTransaction ofrmPOSTrns = new frmPOSTransaction();
                Int32 Id = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID].Value);
                if (Id > 0)
                {
                    oClrScmForPOSTransData = oClrScmForPOSTrans.Populate(Id);
                    if(oClrScmForPOSTransData != null)
                    {
                        if(oClrScmForPOSTransData.ColorSchemeForViewPOSTrans.Rows.Count > 0)
                        {
                            oClrScmForPOSTransRow = (ColorSchemeForViewPOSTransRow)oClrScmForPOSTransData.ColorSchemeForViewPOSTrans.Rows[0];
                            this.numFromAmount.Value = oClrScmForPOSTransRow.FromAmount;
                            this.numToAmount.Value = oClrScmForPOSTransRow.ToAmount;
                            this.cpForeColor.Value = Configuration.ExtractColor(oClrScmForPOSTransRow.ForeColor);
                            this.cpBackColor.Value = Configuration.ExtractColor(oClrScmForPOSTransRow.BackColor);
                            this.grColorScemeDetails.Enabled = true;
                            this.numFromAmount.Focus();
                        }
                    }
                }
                logger.Trace("Edit() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Edit()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void Delete()
        {
            try
            {
                logger.Trace("Delete() - " + clsPOSDBConstants.Log_Entering);

                if (grdDetail.Selected.Rows.Count == 0)
                {
                    return;
                }
                Int32 Id = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID].Value);
                if (Id > 0)
                {
                    oClrScmForPOSTransData = oClrScmForPOSTrans.Populate(Id);
                    if (oClrScmForPOSTransData != null)
                    {
                        if (oClrScmForPOSTransData.ColorSchemeForViewPOSTrans.Rows.Count > 0)
                        {
                            if (Resources.Message.Display("This action will delete selected record. Are your sure?", "Color Scheme For View POS Trans.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                oClrScmForPOSTransData.ColorSchemeForViewPOSTrans[0].Delete();
                                oClrScmForPOSTrans.Persist(oClrScmForPOSTransData);
                                Clear();
                                GetData();
                            }
                        }
                    }
                }
                logger.Trace("Delete() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "Delete()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void frmColorSchemeForViewPOSTrans_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F1)
            {
                SetNew();
                grColorScemeDetails.Enabled = true;
                this.numFromAmount.Focus();
            }
            if (e.KeyData == Keys.F3)
            {
                Edit();
            }
            if (e.KeyData == Keys.F11)
            {
                Delete();
            }
        }

       
       
        private void FocusToCell(string cellName)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell;
            try
            {
                oCurrentRow = this.grdDetail.ActiveRow;
                oCurrentCell = oCurrentRow.Cells[cellName];
                this.grdDetail.ActiveCell = oCurrentCell;
                oCurrentCell.Selected = true;
                oCurrentCell.Activate();
                this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                m_FromError = true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "FocusToCell(string cellName)");
            }
        }

        private void NumBox_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal ToAmount = Configuration.convertNullToDecimal(this.numToAmount.Value);
                decimal FromAmount = Configuration.convertNullToDecimal(this.numFromAmount.Value);
                ValidateAmountRange(FromAmount, ToAmount);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "NumBox_Leave(object sender, EventArgs e)");
                m_FromError = true;
                clsUIHelper.ShowErrorMsg(Ex.Message);
                this.numFromAmount.Focus();
            }
        }
        private void ValidateColor(Color ForeColor , Color BackColor)
        {
            logger.Trace("ValidateColor(Color ForeColor , Color BackColor) - " + clsPOSDBConstants.Log_Entering);
            if (grdDetail.Selected.Rows.Count == 0 || m_FromError == true)
            {
                return;
            }
            string whereclause = "";
            Int32 Id = 0;
            if (isEditMode == true)
            {
                Id = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID].Value);
                whereclause = " WHERE ID <> " + Id;
            }
            ColorSchemeForViewPOSTransData ds = oClrScmForPOSTrans.PopulateList(whereclause);
            foreach (ColorSchemeForViewPOSTransRow oRow in ds.ColorSchemeForViewPOSTrans.Rows)
            {
                Color RowForeColor = Configuration.ExtractColor(oRow.ForeColor);
                Color RowBackColor = Configuration.ExtractColor(oRow.BackColor);

                if (RowForeColor == ForeColor || RowForeColor== BackColor || RowBackColor == ForeColor || RowBackColor == BackColor)
                {
                    throw new Exception("Color is already used in other amount range.");
                }
            }
            logger.Trace("ValidateColor(Color ForeColor , Color BackColor) - " + clsPOSDBConstants.Log_Exiting);
        }
        private void cpColor_Leave(object sender, EventArgs e)
        {
            Infragistics.Win.UltraWinEditors.UltraColorPicker ColorBox = null;
            try
            {
                ColorBox = (Infragistics.Win.UltraWinEditors.UltraColorPicker)sender;
                Color ForeColor = Configuration.ExtractColor(this.cpForeColor.Value.ToString());
                Color BackColor = Configuration.ExtractColor(this.cpBackColor.Value.ToString());
                ValidateColor(ForeColor, BackColor);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "cpColor_Leave(object sender, EventArgs e)");
                m_FromError = true;
                clsUIHelper.ShowErrorMsg(Ex.Message);
                ColorBox.Focus();
            }
        }
}
}