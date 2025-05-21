using POS.BusinessTier;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
//using POS_Core.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POS_Core.DataAccess;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;
using POS_Core.BusinessTier;

namespace POS_Core_UI
{
    public partial class frmOtherLanguageDesc : Form
    {
        #region Declaration
        DataTable dtLanguage = new DataTable();
        string sToLanguage = string.Empty;
        Translator oTranslator = new Translator();
        frmLanguageTranslation ofrmLangTranslator = new frmLanguageTranslation();
        #endregion

        public frmOtherLanguageDesc()
        {
            InitializeComponent();
        }

        private void frmOtherLanguageDesc_Load(object sender, EventArgs e)
        {
            try
            {
                this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
                this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

                clsUIHelper.SetAppearance(this.grdDetail);
                clsUIHelper.SetKeyActionMappings(this.grdDetail);
                clsUIHelper.setColorSchecme(this);
                this.grdDetail.DisplayLayout.Bands[0].Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                LoadLanguage();
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void ApplyGridFormate()
        {
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.DescInEnglish_Fld_Id].Hidden = true;
            this.grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.DescInEnglish_Fld_ColumnName].Hidden = true;
        }

        private void LoadLanguage()
        {
            try
            {
                cmbLanguage.Items.Clear();
                Language oLanguage = new Language();
                dtLanguage = oLanguage.PopulateList();
                if (Configuration.isNullOrEmptyDataTable(dtLanguage) == true)
                {
                    clsUIHelper.ShowErrorMsg("Fail to populate languages.");
                    return;
                }
                foreach (DataRow oRow in dtLanguage.Rows)
                {
                    cmbLanguage.Items.Add(Configuration.convertNullToInt(oRow[clsPOSDBConstants.Language_Fld_ID]), Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Name]) + "(" + Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Code]) + ")");
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
                    DataSet ds = null;
                    string strSQL = string.Empty;
                    using (OtherLanguageDescSvr dao = new OtherLanguageDescSvr())
                    {
                        ds = dao.PopulateOtherLanguageDesc(Convert.ToInt64(this.cmbLanguage.Value));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //update record
                            strSQL = "UPDATE OtherLanguageDesc SET ";
                            for (int i = 0; i < this.grdDetail.Rows.Count; i++)
                            {
                                strSQL += this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_ColumnName].Value.ToString() + " = N'" + this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Translation].Value.ToString().Replace("'","''") + "',";
                            }
                            strSQL = strSQL.Substring(0, strSQL.Length - 1) + " WHERE LanguageId = " + Convert.ToInt64(this.cmbLanguage.Value); 
                        }
                        else
                        {
                            //Insert record
                            strSQL = "INSERT INTO OtherLanguageDesc(LanguageId";
                            for (int i = 0; i < this.grdDetail.Rows.Count; i++)
                            {
                                strSQL += "," + this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_ColumnName].Value.ToString();
                            }
                            strSQL += ") VALUES(" + Convert.ToInt64(this.cmbLanguage.Value);
                            for (int i = 0; i < this.grdDetail.Rows.Count; i++)
                            {
                                strSQL += ",N'" + this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Translation].Value.ToString().Replace("'", "''") + "'";
                            }
                            strSQL += ")";
                        }
                        dao.Persist(strSQL);
                    }
                    RetVal = true;
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("No data found.");
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
                    clsUIHelper.ShowSuccessMsg("Translated data saved successfully.", "Save");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.cmbLanguage.Text) == true)
                {
                    clsUIHelper.ShowErrorMsg("Please select translation language.");
                    return;
                }
                LoadGrid();
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void LoadGrid()
        {
             DataSet ds = null;
             using (OtherLanguageDescSvr dao = new OtherLanguageDescSvr())
             {
                 ds = dao.PopulateDescInEnglish();
                 if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                 {
                     for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                     {
                         if (this.grdDetail.Rows.Count < ds.Tables[0].Rows.Count)
                             this.grdDetail.Rows.Band.AddNew();

                         this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Id].Value = Convert.ToInt32(ds.Tables[0].Rows[i][clsPOSDBConstants.DescInEnglish_Fld_Id].ToString());
                         this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_ColumnName].Value = Configuration.convertNullToString(ds.Tables[0].Rows[i][clsPOSDBConstants.DescInEnglish_Fld_ColumnName]);
                         this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Description].Value = Configuration.convertNullToString(ds.Tables[0].Rows[i][clsPOSDBConstants.DescInEnglish_Fld_Description]); 
                     }

                     //fetch the translation from database and load
                     ds = dao.PopulateOtherLanguageDesc(Convert.ToInt64(this.cmbLanguage.Value));
                     if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                     {
                         for (int i = 0; i < this.grdDetail.Rows.Count; i++)
                         {
                             for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                             {
                                 if (this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_ColumnName].Value.ToString().Trim().ToUpper() == ds.Tables[0].Columns[j].ColumnName.ToString().Trim().ToUpper())
                                 {
                                     this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Translation].Value = Configuration.convertNullToString(ds.Tables[0].Rows[0][j]);
                                     break;
                                 }
                             }
                         }
                     }
                     else 
                     {
                         for (int i = 0; i < this.grdDetail.Rows.Count; i++)
                            this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Translation].Value = "";
                     }
                 }
             }
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.cmbLanguage.Text) == true)
                {
                    clsUIHelper.ShowErrorMsg("Please select translation language.");
                    return;
                }
                if (this.grdDetail.Rows.Count == 0)
                {
                    clsUIHelper.ShowErrorMsg("No data found for translation.");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                TranslateAsync();   //PRIMEPOS-3164 01-Nov-2022 JY Modified
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private async System.Threading.Tasks.Task TranslateAsync()
        {
            string sToLanguageCode = string.Empty;
            string sFromLanguageCode = "en";    //English 
            try
            {
                foreach (DataRow oRow in dtLanguage.Rows)
                {
                    if (Configuration.convertNullToInt(oRow[clsPOSDBConstants.Language_Fld_ID]) == Configuration.convertNullToInt(this.cmbLanguage.Value))
                    {
                        sToLanguageCode = Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Code]);
                        sToLanguage = Configuration.convertNullToString(oRow[clsPOSDBConstants.Language_Fld_Name]);
                        break;
                    }
                }

                for (int i = 0; i < this.grdDetail.Rows.Count; i++)
                {
                    //this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Translation].Value = oTranslator.Translate(this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Description].Value.ToString().Trim(), sFromLanguageCode, sToLanguageCode);
                    this.grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Translation].Value = await oTranslator.Translate(grdDetail.Rows[i].Cells[clsPOSDBConstants.DescInEnglish_Fld_Description].Value.ToString().Trim(), sFromLanguageCode, sToLanguageCode); //PRIMEPOS-3164 01-Nov-2022 JY Modified
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void grdDetail_Enter(object sender, EventArgs e)
        {
            try
            {
                this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void cmbLanguage_SelectionChanged(object sender, EventArgs e)
        {
            if (cmbLanguage.SelectedIndex != -1)
                btnSearch_Click(sender, e);
        }
    }
}
