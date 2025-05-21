//Author:Shitaljit 
//Created Date 27 May 2013
//This form is responsible to show the child keys of a parent function Key/Touch Key
//Known Bugs/Issesw=NONE


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.BusinessRules;
using NLog;
using POS_Core_UI.UI;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmExtendedFuncKeys : Form
    {
        private string ParentKey = string.Empty;
        public frmPOSTransaction parentForm = null;
        string ItemCode = string.Empty;
        string CurrentKey = string.Empty;
        FunctionKeysRow oFKRow = null;
        ContextMenu RightClickMenu = new ContextMenu();
        private int MaxFuncKeyPosition = 40;
        private int MinFuncKeyPosition = 0;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public frmExtendedFuncKeys()
        {
            InitializeComponent();
            SetRightClcikMenu();
        }
        public frmExtendedFuncKeys(string sParentKey)
        {
            InitializeComponent();
            SetRightClcikMenu();
            ParentKey = sParentKey;
        }

       
        public frmPOSTransaction ParentForm
        {
            get { return parentForm; }
            set { parentForm = value; }
        }


       
        public void FillEmptyCells(int NoOfEmptyRows)
        {
            for (int RowIndex = NoOfEmptyRows; RowIndex <= 40; RowIndex++)
            {
                string funcKeyName = "";
                string Operation = string.Empty;
                string panelKey = string.Empty;
                Color c = new Color();
                funcKeyName += "Not Set";
                if (RowIndex <= 8)
                {
                    formatPanel(this.stbFunKeys1.Panels.Add(panelKey, funcKeyName), this.stbFunKeys1);
                    try {
                        this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.BackColor = c;
                        this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                        this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                        this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.ForeColor = c;
                    } catch (Exception Ex) {
                        logger.Fatal(Ex, "FillEmptyCells(int NoOfEmptyRows)");
                    }
                }
                else if (RowIndex <= 16)
                {
                    formatPanel(this.stbFunKeys2.Panels.Add(panelKey, funcKeyName), this.stbFunKeys2);
                    try {
                        this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.BackColor = c;
                        this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                        this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                        this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.ForeColor = c;
                    } catch (Exception Ex) {
                        logger.Fatal(Ex, "FillEmptyCells(int NoOfEmptyRows)");
                    }
                }
                else if (RowIndex <= 24)
                {
                    formatPanel(this.stbFunKeys3.Panels.Add(panelKey, funcKeyName), this.stbFunKeys3);
                    try {
                        this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.BackColor = c;
                        this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                        this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                        this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.ForeColor = c;
                    } catch (Exception Ex) {
                        logger.Fatal(Ex, "FillEmptyCells(int NoOfEmptyRows)");
                    }
                }
                else if (RowIndex <= 32)
                {
                    formatPanel(this.stbFunKeys4.Panels.Add(panelKey, funcKeyName), this.stbFunKeys4);
                    try {
                        this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.BackColor = c;
                        this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                        this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                        this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.ForeColor = c;
                    } catch (Exception Ex) {
                        logger.Fatal(Ex, "FillEmptyCells(int NoOfEmptyRows)");
                    }
                }
                else if (RowIndex <= 40)
                {
                    formatPanel(this.stbFunKeys5.Panels.Add(panelKey, funcKeyName), this.stbFunKeys5);
                    try {
                        this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.BackColor = c;
                        this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                        this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                        this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.ForeColor = c;
                    } catch (Exception Ex) {
                        logger.Fatal(Ex, "FillEmptyCells(int NoOfEmptyRows)");
                    }
                }
            }
        }
        private void formatPanel(Infragistics.Win.UltraWinStatusBar.UltraStatusPanel pnl, Infragistics.Win.UltraWinStatusBar.UltraStatusBar stb)
        {
            pnl.WrapText = Infragistics.Win.DefaultableBoolean.True;
            pnl.Style = Infragistics.Win.UltraWinStatusBar.PanelStyle.Button;
            pnl.Width = stb.Width / 8;
            pnl.MinWidth = stb.Width / 8;
        }

        /// <summary>
        /// To Pupulate the child keys.
        /// </summary>
        /// <param name="sParentKey"></param>
        /// <returns></returns>
        public bool LoadSubKeys(string sParentKey)
        {
            bool RetVal = false;

            FunctionKeys oFunctionKeys = new FunctionKeys();
            FunctionKeysData oFunctionKeysData = new FunctionKeysData();
             FunctionKeysRow oRow  = null;
            try
            {
                oFunctionKeysData = oFunctionKeys.PopulateList(" WHERE SubPosition >= '" + MaxFuncKeyPosition + "'"+" AND FunKey='" + sParentKey + "'");
                if (oFunctionKeysData != null && oFunctionKeysData.FunctionKeys.Rows.Count > 0)
                {
                    this.btnNext.Enabled = true;
                }
                else
                {
                    this.btnNext.Enabled = false;
                }
                oFunctionKeysData = oFunctionKeys.PopulateList(" WHERE SubPosition >= '" + MinFuncKeyPosition + "' AND  SubPosition <= '" + MaxFuncKeyPosition + "'" + " AND FunKey='" + sParentKey + "'");
                if (MaxFuncKeyPosition > 40)
                {
                    this.btnPrevious.Enabled = true;
                }
                else
                {
                    this.btnPrevious.Enabled = false;
                }
                FunctionKeysData oFKData = null;
                if (clsUIHelper.isNumeric(sParentKey) == true)
                {
                    oFKData = oFunctionKeys.PopulateList(" where KeyId='" + sParentKey + "'");
                }
                else
                {
                    oFKData = oFunctionKeys.PopulateList(" where FunKey='" + sParentKey + "'");
                }
                if (oFKData.FunctionKeys.Rows.Count == 0)
                {
                    return false;
                }
                oFKRow = (FunctionKeysRow)oFKData.FunctionKeys.Rows[0];
                this.Text = "Sub Keys For [" + oFKRow.Operation.Trim()+"]";
                oFunctionKeysData = oFunctionKeys.PopulateList(" WHERE Parent = '" + oFKRow.KeyId + "' ORDER BY  " + clsPOSDBConstants.FunctionKeys_Fld_SubPosition);
                btnNext.Enabled = btnPrevious.Enabled = (oFunctionKeysData.FunctionKeys.Rows.Count > 40);
                this.stbFunKeys1.Panels.Clear();
                this.stbFunKeys2.Panels.Clear();
                this.stbFunKeys3.Panels.Clear();
                this.stbFunKeys4.Panels.Clear();
                this.stbFunKeys5.Panels.Clear();

                string funcKeyName = "";
                string Operation = string.Empty;
                string panelKey = string.Empty;
                for (int index = 1; index <= oFunctionKeysData.FunctionKeys.Rows.Count; index++)
                {
                    oRow = (FunctionKeysRow)oFunctionKeysData.FunctionKeys.Rows[index - 1];

                    funcKeyName = (string.IsNullOrEmpty(oRow.FunKey) ? string.Empty : oRow.FunKey + "=");
                    panelKey = string.IsNullOrEmpty(oRow.FunKey) ? oRow.KeyId.ToString() : oRow.FunKey;
                    if (oRow.FunctionType.Equals(clsPOSDBConstants.FunctionKeys_Type_Item) == true && getItemName(oRow.Operation).Trim().Length > 0)
                    {
                        funcKeyName += getItemName(oRow.Operation).Trim();
                    }
                    else if (oRow.FunctionType.Equals(clsPOSDBConstants.FunctionKeys_Type_Parent) == true && string.IsNullOrEmpty(oRow.Operation.Trim()) == false)
                    {
                        funcKeyName += oRow.Operation.Trim();
                    }
                    else
                        funcKeyName += "Not Set";

                    if (index <= 8)
                    {
                        formatPanel(this.stbFunKeys1.Panels.Add(panelKey, funcKeyName), this.stbFunKeys1);
                        try
                        {
                            if (oRow.ButtonBackColor != "" && oRow.ButtonForeColor != "" && oRow.ButtonBackColor.ToUpper() != "Color [Transparent]".ToUpper() && oRow.ButtonForeColor.ToUpper() != "Color [Transparent]".ToUpper())
                            {
                                Color c = Configuration.ExtractColor(oRow.ButtonBackColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.BackColor = c;
                                    this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                                    this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;

                                }
                                c = Configuration.ExtractColor(oRow.ButtonForeColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys1.Panels[this.stbFunKeys1.Panels.Count - 1].Appearance.ForeColor = c;
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Fatal(Ex, "LoadSubKeys(string sParentKey)");
                        }
                    }
                    else if (index <= 16)
                    {
                        formatPanel(this.stbFunKeys2.Panels.Add(panelKey, funcKeyName), this.stbFunKeys2);
                        try
                        {
                            if (oRow.ButtonBackColor != "" && oRow.ButtonForeColor != "" && oRow.ButtonBackColor.ToUpper() != "Color [Transparent]".ToUpper() && oRow.ButtonForeColor.ToUpper() != "Color [Transparent]".ToUpper())
                            {
                                Color c = Configuration.ExtractColor(oRow.ButtonBackColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.BackColor = c;
                                    this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                                    this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                                }

                                c = Configuration.ExtractColor(oRow.ButtonForeColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys2.Panels[this.stbFunKeys2.Panels.Count - 1].Appearance.ForeColor = c;
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Fatal(Ex, "LoadSubKeys(string sParentKey)");
                        }
                    }
                    else if (index <= 24)
                    {
                        formatPanel(this.stbFunKeys3.Panels.Add(panelKey, funcKeyName), this.stbFunKeys3);
                        try
                        {
                            if (oRow.ButtonBackColor != "" && oRow.ButtonForeColor != "" && oRow.ButtonBackColor.ToUpper() != "Color [Transparent]".ToUpper() && oRow.ButtonForeColor.ToUpper() != "Color [Transparent]".ToUpper())
                            {
                                Color c = Configuration.ExtractColor(oRow.ButtonBackColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.BackColor = c;
                                    this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                                    this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                                }

                                c = Configuration.ExtractColor(oRow.ButtonForeColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys3.Panels[this.stbFunKeys3.Panels.Count - 1].Appearance.ForeColor = c;
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Fatal(Ex, "LoadSubKeys(string sParentKey)");
                        }
                    }
                    else if (index <= 32)
                    {
                        //PRIMEPOS-2467 30-Apr-2018 JY corrected the stbFunKeys4 reference
                        formatPanel(this.stbFunKeys4.Panels.Add(panelKey, funcKeyName), this.stbFunKeys4);
                        try
                        {
                            if (oRow.ButtonBackColor != "" && oRow.ButtonForeColor != "" && oRow.ButtonBackColor.ToUpper() != "Color [Transparent]".ToUpper() && oRow.ButtonForeColor.ToUpper() != "Color [Transparent]".ToUpper())
                            {
                                Color c = Configuration.ExtractColor(oRow.ButtonBackColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.BackColor = c;
                                    this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                                    this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                                }

                                c = Configuration.ExtractColor(oRow.ButtonForeColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys4.Panels[this.stbFunKeys4.Panels.Count - 1].Appearance.ForeColor = c;
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Fatal(Ex, "LoadSubKeys(string sParentKey)");
                        }
                    }
                    else if (index <= 40)
                    {
                        //PRIMEPOS-2467 30-Apr-2018 JY corrected the stbFunKeys5 reference
                        formatPanel(this.stbFunKeys5.Panels.Add(panelKey, funcKeyName), this.stbFunKeys5);
                        try
                        {
                            if (oRow.ButtonBackColor != "" && oRow.ButtonForeColor != "" && oRow.ButtonBackColor.ToUpper() != "Color [Transparent]".ToUpper() && oRow.ButtonForeColor.ToUpper() != "Color [Transparent]".ToUpper())
                            {
                                Color c = Configuration.ExtractColor(oRow.ButtonBackColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.BackColor = c;
                                    this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].BorderStyle = Infragistics.Win.UIElementBorderStyle.Raised;
                                    this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.BorderColor = Color.Maroon;
                                }

                                c = Configuration.ExtractColor(oRow.ButtonForeColor.ToString());
                                if (c != Color.Empty)
                                {
                                    this.stbFunKeys5.Panels[this.stbFunKeys5.Panels.Count - 1].Appearance.ForeColor = c;
                                }
                            }
                        }
                        catch (Exception Ex)
                        {
                            logger.Fatal(Ex, "LoadSubKeys(string sParentKey)");
                        }
                    }
                }
                if (oFunctionKeysData != null && oFunctionKeysData.FunctionKeys.Rows.Count < 40)
                {
                    FillEmptyCells(oFunctionKeysData.FunctionKeys.Rows.Count);
                }
                RetVal = true;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "LoadSubKeys(string sParentKey)");
                clsUIHelper.ShowErrorMsg(exp.Message);
                exp = null;
            }
            return RetVal;
        }

        private string getItemName(string itemId)
        {
            try
            {
                Item oItem = new Item();
                ItemData oItemData;
                ItemRow oItemRow;

                oItemData = oItem.Populate(itemId);
                if (oItemData.Tables[0].Rows.Count > 0)
                    oItemRow = oItemData.Item[0];
                else
                    return "";

                if (Configuration.convertNullToString(oItemRow.Description) == "")  //PRIMEPOS-2467 30-Apr-2018 JY Also handled one more scenario as - we might have blank item description, in that case, that item could not be loaded on "Sub Keys" screen, here, we can display ItemID instead of the blank item description.
                    return oItemRow.ItemID;
                else
                    return oItemRow.Description;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "getItemName(string itemId)");
                return "";
            }
        }

        private void ProcessFunctionKeyToolbarSelection(string Key)
        {
            try
            {
                if (Key.StartsWith("F"))
                {
                    SendKeys.Send("{" + Key + "}");
                }
                else
                {
                    int pos = Key.IndexOf("+", 0);
                    if (pos > 0)
                    {
                        string leftKey = Key.Substring(0, pos);
                        string rightKey = Key.Substring(pos + 1);
                        if (leftKey.ToUpper() == "CTRL")
                            leftKey = "^";
                        else if (leftKey.ToUpper() == "ALT")
                            leftKey = "%";
                        else if (leftKey.ToUpper() == "SHIFT")
                            leftKey = "+";
                        else
                            leftKey = "";

                        System.Windows.Forms.SendKeys.Send(leftKey + "{" + rightKey + "}");
                        Application.DoEvents();
                    }
                    else
                    {
                        AddItemToTransaction(Key);
                    }

                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ProcessFunctionKeyToolbarSelection(string Key)");
            }
        }

        private void FunctionKeysClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelEventArgs e)
        {
            ProcessFunctionKeyToolbarSelection(e.Panel.Key);
        }
        private void AddItemToTransaction(string FKey)
        {
            FunctionKeys oFKeys = new FunctionKeys();
            ItemCode = string.Empty;
            string whereClause = string.Empty;
            try
            {
                ItemCode = FunKeyCommonOperations.GetITemCode(FKey);
                if (ParentForm.AddItemsToTransaction(ItemCode, FKey) == false)
                {
                    ItemCode = string.Empty;
                    return;
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "AddItemToTransaction(string FKey)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void frmExtendedFuncKeys_KeyUp(object sender, KeyEventArgs e)
        {
            String FKey;
            ItemCode = string.Empty;
            try
            {
                FKey = "";
                if (e.Control == true)
                {
                    FKey = "Ctrl+" + e.KeyData.ToString().Substring(0, e.KeyData.ToString().IndexOf(","));
                }
                else if (e.Shift == true)
                {
                    FKey = "Shift+" + e.KeyData.ToString().Substring(0, e.KeyData.ToString().IndexOf(","));
                }
                else
                {
                    FKey += e.KeyData.ToString();
                }
                AddItemToTransaction(FKey);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "frmExtendedFuncKeys_KeyUp(object sender, KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void frmExtendedFuncKeys_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.PageDown && e.Shift == false && e.Control == false && e.Alt == false && this.btnNext.Enabled == true)
            {
                BrowseFuncKey(FuncKeyBrowse.Forward);
            }
            else if (e.KeyData == Keys.PageUp && e.Shift == false && e.Control == false && e.Alt == false && this.btnPrevious.Enabled == true)
            {
                BrowseFuncKey(FuncKeyBrowse.Backward);
            }
            else if (e.KeyData == Keys.F1 && e.Shift == false && e.Control == false && e.Alt == false)
            {
                BrowseFuncKey(FuncKeyBrowse.Home);
            }
            else if (e.KeyData == Keys.F2 && e.Shift == false && e.Control == false && e.Alt == false)
            {
                FunKeyCommonOperations.AddKeys(oFKRow);
            }
        }

        private void frmExtendedFuncKeys_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
        }

        private void FunctionKeyRightClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                bool isEnable = string.IsNullOrEmpty(e.Panel.Key);
                CurrentKey = Configuration.convertNullToString(e.Panel.Key);
                RightClickMenu.MenuItems[0].Enabled = isEnable;
                RightClickMenu.MenuItems[1].Enabled = !isEnable;
                RightClickMenu.MenuItems[2].Enabled = !isEnable;
                RightClickMenu.Show((Control)sender, e.Location, LeftRightAlignment.Right);
            }
        }

        private void SetRightClcikMenu()
        {
            MenuItem Add = new MenuItem("Assign Item");
            MenuItem Edit = new MenuItem("Edit");
            MenuItem Remove = new MenuItem("Remove");
            RightClickMenu.MenuItems.Add(Add);
            RightClickMenu.MenuItems.Add(Edit);
            RightClickMenu.MenuItems.Add(Remove);
            Add.Click += RightClickMenuClick;
            Edit.Click += RightClickMenuClick;
            Remove.Click += RightClickMenuClick;
        }
       
       
        private void btnAddKeys_Click(object sender, EventArgs e)
        {
            FunKeyCommonOperations.AddKeys(oFKRow);
        }

        private  void RightClickMenuClick(object sender, EventArgs e)
        {
            try
            {
                MenuItem MI = (MenuItem)sender;
                switch (MI.Index)
                {
                    case 0:
                        FunKeyCommonOperations.AddKeys(oFKRow);
                        break;
                    case 1:
                        FunKeyCommonOperations.EditKeys(CurrentKey);
                        break;
                    case 2:
                        FunKeyCommonOperations.RemoveKey(CurrentKey);
                        break;
                }
                LoadSubKeys(ParentKey);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "RightClickMenuClick(object sender, EventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
                exp = null;
            }
        }

        private void BrowseFuncKey(FuncKeyBrowse Movement)
        {
            try
            {
                if (Movement == FuncKeyBrowse.Forward)
                {
                    MaxFuncKeyPosition = MaxFuncKeyPosition + 40;
                    MinFuncKeyPosition = MinFuncKeyPosition + 40;
                }
                else if (Movement == FuncKeyBrowse.Backward)
                {
                    MaxFuncKeyPosition = MaxFuncKeyPosition - 40;
                    MinFuncKeyPosition = MinFuncKeyPosition - 40;
                }
                else
                {
                    MinFuncKeyPosition = 0;
                    MaxFuncKeyPosition = 40;
                }
                LoadSubKeys(ParentKey);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "BrowseFuncKey(FuncKeyBrowse Movement)");
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            BrowseFuncKey(FuncKeyBrowse.Backward);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            BrowseFuncKey(FuncKeyBrowse.Forward);
        }

        private void btnFunKeyHome_Click(object sender, EventArgs e)
        {
            BrowseFuncKey(FuncKeyBrowse.Home);
        }
    }
}
