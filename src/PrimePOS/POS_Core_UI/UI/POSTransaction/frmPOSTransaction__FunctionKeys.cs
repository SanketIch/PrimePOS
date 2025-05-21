using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using POS_Core_UI.Layout;
using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core_UI.UI
{
    public partial class frmPOSTransaction
    {
        #region functionKey
        //completed by sandeep
        private void BrowseFuncKey(FuncKeyBrowse Movement)
        {
            try {
                if (Movement == FuncKeyBrowse.Forward) {
                    MaxFuncKeyPosition = MaxFuncKeyPosition + cntFuncKey;
                    MinFuncKeyPosition = MinFuncKeyPosition + cntFuncKey;
                } else if (Movement == FuncKeyBrowse.Backward) {
                    MaxFuncKeyPosition = MaxFuncKeyPosition - cntFuncKey;
                    MinFuncKeyPosition = MinFuncKeyPosition - cntFuncKey;
                } else {
                    MinFuncKeyPosition = 0;
                    MaxFuncKeyPosition = cntFuncKey;
                }
                PopulateFunctionKeys();
                SetFontSize(tblFuncKey, this.Width, this.Height, 1);
                this.txtItemCode.Focus();
            } catch (Exception ex) {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        //added by sandeep to populate function key
        private void PopulateFunctionKeys()
        {
            logger.Trace("PopulateFunctionKeys() - " + clsPOSDBConstants.Log_Entering);

            FunctionKeys oFunctionKeys = new FunctionKeys();
            FunctionKeysData oFunctionKeysData = new FunctionKeysData();

            try {
                oFunctionKeysData = oFunctionKeys.PopulateList(" WHERE MainPosition > '" + MaxFuncKeyPosition + "'");
                if (oFunctionKeysData != null && oFunctionKeysData.FunctionKeys.Rows.Count > 0) {
                    this.btnNext.Enabled = true;
                } else {
                    this.btnNext.Enabled = false;
                }
                oFunctionKeysData = oFunctionKeys.PopulateList(" WHERE MainPosition > '" + MinFuncKeyPosition + "' AND  MainPosition <= '" + MaxFuncKeyPosition + "' ORDER BY  " + clsPOSDBConstants.FunctionKeys_Fld_MainPosition);
                if (MaxFuncKeyPosition > 24) {
                    this.btnPrevious.Enabled = true;
                } else {
                    this.btnPrevious.Enabled = false;
                }

                int index = 0;

                tblFuncKey.Controls.Clear();
                for (index = 1; index <= oFunctionKeysData.FunctionKeys.Rows.Count; index++) {
                    string funcKey = "";
                    string funcKeyName = "";
                    string Operation = string.Empty;

                    FunctionKeysRow oRow = (FunctionKeysRow)oFunctionKeysData.FunctionKeys.Rows[index - 1];

                    bool isNotFuncKey = false;

                    if (string.IsNullOrEmpty(oRow.FunKey)) {
                        funcKey = oRow.KeyId.ToString();
                        isNotFuncKey = true;
                    } else {
                        funcKey = oRow.FunKey;
                    }

                    if (oRow.FunctionType.Equals(clsPOSDBConstants.FunctionKeys_Type_Item) == true && oRow.Description.Trim().Length > 0) {
                        funcKeyName = oRow.Description.Trim();
                    } else if (oRow.FunctionType.Equals(clsPOSDBConstants.FunctionKeys_Type_Parent) == true && string.IsNullOrEmpty(oRow.Operation.Trim()) == false) {
                        funcKeyName = oRow.Operation.Trim();
                    } else
                        funcKeyName = "Not Set";

                    Color backColor = Configuration.ExtractColor(oRow.ButtonBackColor.ToString());
                    Color foreColor = Configuration.ExtractColor(oRow.ButtonForeColor.ToString());

                    TableLayoutPanel keyTableLayout = getFunctionKey(funcKey, funcKeyName, isNotFuncKey, backColor, foreColor);

                    //int controlCount = tblFuncKey.Controls.Count;

                    //if (controlCount >= 20 && controlCount % 5 == 0) {
                    //    tblFuncKey.RowCount = tblFuncKey.RowCount + 1;
                    //    tblFuncKey.RowStyles.Add(new RowStyle(SizeType.Absolute, 0));
                    //}

                    tblFuncKey.Controls.Add(keyTableLayout);
                }
                //Fill Empty Key
                if (oFunctionKeysData.FunctionKeys.Rows.Count < cntFuncKey) {
                    int countPendingKey = cntFuncKey - oFunctionKeysData.FunctionKeys.Rows.Count;
                    for (int cnt = 1; cnt <= countPendingKey; cnt++) {
                        TableLayoutPanel keyTableLayout = getFunctionKey("", "Not Set", true, Configuration.ExtractColor("Color [Transparent]"), Configuration.ExtractColor("Color [White]"));
                        tblFuncKey.Controls.Add(keyTableLayout);
                    }
                }


            } catch (Exception exp) {
                logger.Fatal(exp, "PopulateFunctionKeys()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                exp = null;
            }
            logger.Trace("PopulateFunctionKeys() - " + clsPOSDBConstants.Log_Exiting);
        }
        //completed by sandeep
        private void SetRightClickMenu()
        {
            logger.Trace("SetRightClcikMenu() - " + clsPOSDBConstants.Log_Entering);
            RightClickMenu = new System.Windows.Forms.ContextMenu();
            MenuItem Add = new MenuItem("Assign Item");
            MenuItem Edit = new MenuItem("Edit");
            MenuItem Remove = new MenuItem("Remove");
            RightClickMenu.MenuItems.Add(Add);
            RightClickMenu.MenuItems.Add(Edit);
            RightClickMenu.MenuItems.Add(Remove);
            Add.Click += RightClickMenuClick;
            Edit.Click += RightClickMenuClick;
            Remove.Click += RightClickMenuClick;
            logger.Trace("SetRightClcikMenu() - " + clsPOSDBConstants.Log_Exiting);
        }
        //completed by sandeep
        private void RightClickMenuClick(object sender, EventArgs e)
        {
            try {
                logger.Trace("RightClickMenuClick() - " + clsPOSDBConstants.Log_Entering);
                MenuItem MI = (MenuItem)sender;
                switch (MI.Index) {
                    case 0:
                        FunctionKeys oFunctionKeys = new FunctionKeys();
                        FunctionKeysData oFunctionKeysData = new FunctionKeysData();
                        string whereClause = string.Empty;
                        if (clsUIHelper.isNumeric(CurrentFuncKey) == true) {
                            whereClause = " where KeyId='" + CurrentFuncKey + "'";
                        } else {
                            whereClause = " where FunKey='" + CurrentFuncKey + "'";
                        }
                        FunctionKeysData oFKData = oFunctionKeys.PopulateList(whereClause);
                        if (oFKData.FunctionKeys.Rows.Count > 0) {
                            oFKRow = (FunctionKeysRow)oFKData.FunctionKeys.Rows[0];
                        }
                        FunKeyCommonOperations.AddKeys(oFKRow);
                        break;
                    case 1:
                        FunKeyCommonOperations.EditKeys(CurrentFuncKey);
                        break;
                    case 2:
                        FunKeyCommonOperations.RemoveKey(CurrentFuncKey);
                        break;
                }
                PopulateFunctionKeys();
                SetFontSize(tblFuncKey, this.Width, this.Height, 1);
                this.txtItemCode.Focus();
                logger.Trace("RightClickMenuClick() - " + clsPOSDBConstants.Log_Exiting);
            } catch (Exception exp) {
                logger.Fatal(exp, "RightClickMenuClick()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                exp = null;
            }
        }
        //completed by sandeep
        private void btnFunKeyHome_Click(object sender, EventArgs e)
        {
            BrowseFuncKey(FuncKeyBrowse.Home);
        }
        private void FuncKeyEnter(object sender, EventArgs e)
        {
            ((Infragistics.Win.Misc.UltraButton)sender).Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
        }
        
        //completed by sandeep
        private TableLayoutPanel getFunctionKey(string funcKey, string funcKeyName, bool isNotFuncKey, Color backColor, Color foreColor)
        {

            FontSize = 6F;

            TableLayoutPanel Tablebtn = new TableLayoutPanel();

            Infragistics.Win.Appearance appearancelbl = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearancebtn = new Infragistics.Win.Appearance();

            Infragistics.Win.Misc.UltraButton btn = new Infragistics.Win.Misc.UltraButton();
            Infragistics.Win.Misc.UltraLabel utrLbl = new Infragistics.Win.Misc.UltraLabel();

            int indexOfSign = funcKey.IndexOf('+');
           
            utrLbl.Dock = DockStyle.Fill;
            btn.Size = new System.Drawing.Size(73, 19);
            utrLbl.Size = new System.Drawing.Size(31, 19);

            appearancelbl.BackColor = backColor;
            appearancelbl.ForeColor = foreColor;
            appearancelbl.TextHAlignAsString = "Center";
            appearancelbl.TextVAlignAsString = "Middle";
            utrLbl.Appearance = appearancelbl;
            utrLbl.Margin = new System.Windows.Forms.Padding(1, 1, 0, 1);
            utrLbl.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            utrLbl.Text = isNotFuncKey ? "" : (indexOfSign > 1? funcKey.Insert(indexOfSign+1, "\n"):funcKey);
            utrLbl.Tag = "NOCOLOR";
            utrLbl.MouseDown += new MouseEventHandler(funcKeyClick);
            utrLbl.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

            btn.AcceptsFocus = false;
            btn.Dock = DockStyle.Fill;
            btn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            btn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            appearancebtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            appearancebtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            appearancebtn.TextHAlignAsString = "Center";
            appearancebtn.TextVAlignAsString = "Middle";
            btn.Appearance = appearancebtn;
            btn.Margin = new System.Windows.Forms.Padding(0, 1, 1, 1);
            btn.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btn.Text = funcKeyName;
            btn.MouseDown += new MouseEventHandler(funcKeyClick);
            //btn.Click += new EventHandler(FuncKeyEnter);
           //btn.MouseLeave += new EventHandler(FuncKeyLeave);
            btn.Tag = funcKey;
            btn.TabStop = false;
            btn.UseFlatMode = Infragistics.Win.DefaultableBoolean.True; 
            btn.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            btn.HotTrackAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
           

            Tablebtn.Size = new System.Drawing.Size(108, 19);//108,31
            Tablebtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(193)))), ((int)(((byte)(193)))));
            //Tablebtn.BackColor = Color.Red;
            Tablebtn.ColumnCount = 2;
            Tablebtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            Tablebtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            Tablebtn.RowCount = 1;
            Tablebtn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            Tablebtn.Tag = "FuncKey";
            Tablebtn.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            Tablebtn.Controls.Add(utrLbl, 0, 0);
            Tablebtn.Controls.Add(btn, 1, 0);
            Tablebtn.Dock = System.Windows.Forms.DockStyle.Fill;
            Tablebtn.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            Tablebtn.CellPaint+= new TableLayoutCellPaintEventHandler(this.FunctionKeys_CellPaint);

            return Tablebtn;
        }

        private void FunctionKeys_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var panel = sender as TableLayoutPanel;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;
            using (var pen = new Pen(Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120))))), 0.1f)) {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;

                if (e.Row == (panel.RowCount - 1)) {
                    rectangle.Height -= 1;
                }

                if (e.Column == (panel.ColumnCount - 1)) {
                    rectangle.Width -= 1;
                }

                e.Graphics.DrawRectangle(pen, rectangle);
            }
        }


        //completed by sandeep
        private void btnAddNewFucnKey_Click(object sender, EventArgs e)
        {
            FunKeyCommonOperations.AddKeys(oFKRow);
            this.txtItemCode.Focus();
        }
        //completed by sandeep
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            BrowseFuncKey(FuncKeyBrowse.Backward);
        }
        //completed by sandeep
        private void btnNext_Click(object sender, EventArgs e)
        {
            BrowseFuncKey(FuncKeyBrowse.Forward);
        }


        //added by sandeep to call function key
        private void funcKeyClick(object sender, MouseEventArgs e)
        {
            string funcKey = "";
           // bool isbtn = false;
            //Color btnColor = Color.White;
            if (sender.GetType() == typeof(Infragistics.Win.Misc.UltraLabel)) {
                funcKey = ((Infragistics.Win.Misc.UltraButton)((Infragistics.Win.Misc.UltraLabel)sender).Parent.Controls[1]).Tag.ToString();
            } else {
                //isbtn = true;
               // btnColor = ((Infragistics.Win.Misc.UltraButton)sender).Appearance.BackColor;
                funcKey =  ((Infragistics.Win.Misc.UltraButton)sender).Tag.ToString();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.FunctionKeys.ID) == true) {
                    bool isEnable = string.IsNullOrEmpty(funcKey);
                    CurrentFuncKey = Configuration.convertNullToString(funcKey);
                    RightClickMenu.MenuItems[0].Enabled = isEnable;
                    RightClickMenu.MenuItems[1].Enabled = !isEnable;
                    RightClickMenu.MenuItems[2].Enabled = !isEnable;
                    RightClickMenu.Show((Control)sender, e.Location, LeftRightAlignment.Right);
                }
            } else {
                //if (isbtn) {
                //    ((Infragistics.Win.Misc.UltraButton)sender).Appearance.ForeColor = Color.Black;
                //}
                    ProcessFunctionKeyToolbarSelection(funcKey);
                //if (isbtn) {
                //    ((Infragistics.Win.Misc.UltraButton)sender).Appearance.BackColor = btnColor;
                //}
            }
        }
        //completed by sandeep
        private void ProcessFunctionKeyToolbarSelection(string Key)
        {
            try {
                logger.Trace("ProcessFunctionKeyToolbarSelection() - " + clsPOSDBConstants.Log_Entering);
                if (Key.StartsWith("F")) {
                    txtItemCode.Focus();
                    SendKeys.Send("{" + Key + "}");
                    Application.DoEvents();
                } else {
                    int pos = Key.IndexOf("+", 0);
                    if (pos > 0) {
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

                        txtItemCode.Focus();
                        System.Windows.Forms.SendKeys.Send(leftKey + "{" + rightKey + "}");
                        Application.DoEvents();
                    } else//Added By Shitaljit for keys which dont have a short key associated
                      {
                        string ItemCode = FunKeyCommonOperations.GetITemCode(Key);
                        if (string.IsNullOrEmpty(ItemCode) == false && AddItemsToTransaction(ItemCode, Key) == false) {
                            clsUIHelper.ShowErrorMsg("Invalid item.");
                        } else if (string.IsNullOrEmpty(ItemCode) == true) {
                            frmExtendedFuncKeys ofrmExtendedFuncKeys = new frmExtendedFuncKeys(Key);
                            ofrmExtendedFuncKeys.parentForm = this;
                            if (ofrmExtendedFuncKeys.LoadSubKeys(Key)) {
                                ofrmExtendedFuncKeys.StartPosition = FormStartPosition.CenterParent;
                                ofrmExtendedFuncKeys.ShowDialog();
                            }
                        }
                    }
                }
            } catch (Exception Exp) {
                logger.Trace(Exp, "ProcessFunctionKeyToolbarSelection()");
            }
            logger.Trace("ProcessFunctionKeyToolbarSelection() - " + clsPOSDBConstants.Log_Exiting);
        }

        //completed by sandeep
        public bool AddItemsToTransaction(string sItemCode, string strKey)
        {
            logger.Trace("AddItemsToTransaction() - " + clsPOSDBConstants.Log_Entering);
            bool retValue = false;
            ItemSvr oItemSvr = new ItemSvr();
            ItemData oIData = oItemSvr.Populate(sItemCode);
            if (oIData.Item.Rows.Count > 0) {
                if (string.IsNullOrEmpty(this.txtItemCode.Text) == true) {
                    this.txtItemCode.Text = sItemCode;
                } else if (clsUIHelper.isNumeric(this.txtItemCode.Text)) {
                    this.txtItemCode.Text += "@" + sItemCode;
                } else if (strKey == "Shift+Q" || strKey == "Shift+W") {
                    this.txtItemCode.Text = this.txtItemCode.Text.Substring(0, this.txtItemCode.Text.Length - 1);
                    this.txtItemCode.Text += "@" + sItemCode;
                } else {
                    this.txtItemCode.Text += "@" + sItemCode;
                }
                ItemBox_Validatiang(txtItemCode, new CancelEventArgs());
                retValue = true;
            }
            logger.Trace("AddItemsToTransaction() - " + clsPOSDBConstants.Log_Exiting);
            return retValue;
        }
        //completed by sandeep
        private void btnItemPad_Click(object sender, System.EventArgs e)
        {
            try {
                if (tblFuncKey.Visible) {
                    this.btnItemPad.Text = "Show Item Pad";
                } else {
                    this.btnItemPad.Text = "Hide Item Pad";
                }
                tblFuncKey.Visible = !(tblFuncKey.Visible);
                this.tblFunKeyMenu.Visible = !(this.tblFunKeyMenu.Visible);
                this.txtItemCode.Focus();
            } catch (Exception) { }
        }
        #endregion

    }
}
