using POS_Core.BusinessRules;
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
    public partial class frmPaytoutCatFuncKeys : Form
    {
        public string SelectedKey = "";
        public frmPaytoutCatFuncKeys()
        {
            InitializeComponent();
        }


        public void FillEmptyCells(int NoOfEmptyRows)
        {
            for (int RowIndex = NoOfEmptyRows; RowIndex <= 40; RowIndex++)
            {
                string funcKeyName = "";
                string Operation = string.Empty;
                string panelKey = string.Empty;
                funcKeyName += "Not Defined";
                if (RowIndex <= 8)
                {
                    formatPanel(this.stbFunKeys1.Panels.Add(panelKey, funcKeyName), this.stbFunKeys1);
                    try
                    {
                        this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Visible = false;

                    }
                    catch (Exception) { }
                }
                else if (RowIndex <= 16)
                {
                    formatPanel(this.stbFunKeys2.Panels.Add(panelKey, funcKeyName), this.stbFunKeys2);
                    try
                    {
                        this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Visible = false;
                    }
                    catch (Exception Ex) { }
                }
                else if (RowIndex <= 24)
                {
                    formatPanel(this.stbFunKeys3.Panels.Add(panelKey, funcKeyName), this.stbFunKeys3);
                    try
                    {
                        this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Visible = false;
                    }
                    catch (Exception) { }
                }
                else if (RowIndex <= 32)
                {
                    formatPanel(this.stbFunKeys4.Panels.Add(panelKey, funcKeyName), this.stbFunKeys4);
                    try
                    {
                        this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Visible = false;
                    }
                    catch (Exception) { }
                }
                else if (RowIndex <= 40)
                {
                    formatPanel(this.stbFunKeys5.Panels.Add(panelKey, funcKeyName), this.stbFunKeys5);
                    try
                    {
                        this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Visible = false;
                    }
                    catch (Exception) { }
                }
            }
        }
        
        
        /// <summary>
        /// To Pupulate payout categories keys.
        /// </summary>
        /// <param name="sParentKey"></param>
        /// <returns></returns>
        public bool LoadSubKeys()
        {
            bool RetVal = false;

            PayOutCat oPayOutCat = new PayOutCat();
            PayOutCatData oPayOutCatData = new PayOutCatData();
            PayOutCatRow oRow = null;
            try
            {
                oPayOutCatData = oPayOutCat.PopulateList("");
                if (Configuration.isNullOrEmptyDataSet(oPayOutCatData) == true)
                {
                    return RetVal;
                }
                if (Configuration.isNullOrEmptyDataSet(oPayOutCatData) == false && oPayOutCatData.PayOutCat.Rows.Count > 40)
                {
                    this.btnNext.Enabled = true;
                }
                else
                {
                    this.btnNext.Enabled = false;
                }

                this.stbFunKeys1.Panels.Clear();
                this.stbFunKeys2.Panels.Clear();
                this.stbFunKeys3.Panels.Clear();
                this.stbFunKeys4.Panels.Clear();
                this.stbFunKeys5.Panels.Clear();

                string funcKeyName = "";
                string Operation = string.Empty;
                string panelKey = string.Empty;
                for (int index = 1; index <= oPayOutCatData.PayOutCat.Rows.Count; index++)
                {
                    oRow = (PayOutCatRow)oPayOutCatData.PayOutCat.Rows[index - 1];

                    funcKeyName = (string.IsNullOrEmpty(oRow.PayoutCatType) ? string.Empty : oRow.PayoutCatType );
                    panelKey = string.IsNullOrEmpty(oRow.ID.ToString()) ? oRow.ID.ToString() : oRow.ID.ToString();

                    if (index <= 8)
                    {
                       formatPanel(this.stbFunKeys1.Panels.Add(panelKey, funcKeyName), this.stbFunKeys1);
                        try
                        {
                            Color c = Color.AliceBlue;
                            this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.BackColor = c;
                            this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                            this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;

                            c = Color.Black;

                            this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.ForeColor = c;
                        }
                        catch (Exception) { }
                    }
                    else if (index <= 16)
                    {
                        formatPanel(this.stbFunKeys2.Panels.Add(panelKey, funcKeyName), this.stbFunKeys2);
                        try
                        {

                            Color c = Color.AliceBlue;
                            this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.BackColor = c;
                            this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                            this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;

                            c = Color.Black;

                            this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.ForeColor = c;
                        }
                        catch (Exception) { }
                    }
                    else if (index <= 24)
                    {
                        formatPanel(this.stbFunKeys3.Panels.Add(panelKey, funcKeyName), this.stbFunKeys3);
                        try
                        {
                            Color c = Color.AliceBlue;
                            this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.BackColor = c;
                            this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                            this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;

                            c = Color.Black;

                            this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.ForeColor = c;
                        }
                        catch (Exception) { }
                    }
                    else if (index <= 32)
                    {
                        formatPanel(this.stbFunKeys3.Panels.Add(panelKey, funcKeyName), this.stbFunKeys4);
                        try
                        {
                            Color c = Color.AliceBlue;
                            this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.BackColor = c;
                            this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                            this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;

                            c = Color.Black;

                            this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.ForeColor = c;
                        }
                        catch (Exception) { }
                    }
                    else if (index <= 40)
                    {
                        formatPanel(this.stbFunKeys3.Panels.Add(panelKey, funcKeyName), this.stbFunKeys5);
                        try
                        {
                            Color c = Color.AliceBlue;
                            this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.BackColor = c;
                            this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                            this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;

                            c = Color.Black;

                            this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.ForeColor = c;
                        }
                        catch (Exception) { }
                    }
                }
                if (oPayOutCatData != null && oPayOutCatData.PayOutCat.Rows.Count < 40)
                {
                    FillEmptyCells(oPayOutCatData.PayOutCat.Rows.Count);
                }
                RetVal = true;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                exp = null;
            }
            return RetVal;
        }


        private void formatPanel(Infragistics.Win.UltraWinStatusBar.UltraStatusPanel pnl, Infragistics.Win.UltraWinStatusBar.UltraStatusBar stb)
        {
            pnl.WrapText = Infragistics.Win.DefaultableBoolean.True;
            pnl.Style = Infragistics.Win.UltraWinStatusBar.PanelStyle.Button;
            pnl.Width = stb.Width / 8;
            pnl.MinWidth = stb.Width / 8;
        }


        private void frmPaytoutCatFuncKeys_Load(object sender, EventArgs e)
        {
            try
            {
                clsUIHelper.setColorSchecme(this);
                LoadSubKeys();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmPaytoutCatFuncKeys_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void stbFunKeys1_ButtonClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelEventArgs e)
        {
            ProcessFunctionKeyToolbarSelection(e.Panel.Key);
        }

        private void ProcessFunctionKeyToolbarSelection(string Key)
        {
            try
            {
                SelectedKey = Key;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception)
            { }
        }
    }
}
