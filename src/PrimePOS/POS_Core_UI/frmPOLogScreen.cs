using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using System.Data;
using Infragistics.Win.UltraWinGrid;
using POS_Core_UI.Resources;

namespace POS_Core_UI
{
    public class frmPOLogScreen : Form
    {
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.Misc.UltraButton btnPurge;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdPoLog;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor lastDaysNumericEditor;
        private MessageLogData msgLogData = null;

        public frmPOLogScreen()
        {
            InitializeComponent();

            LoadLogData();
            ClsUpdatePOStatus.UpdateLogStatus += new ClsUpdatePOStatus.UpdateLogData(PrimePOUtil_UpdateLogStatus);
            this.Hide();
        }

        void PrimePOUtil_UpdateLogStatus()
        {
            try
            {
                Application.DoEvents();
                LoadLogData();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());    
            }  
        }

        private void LoadLogData()
        {
            try
            {
                if (ClsUpdatePOStatus.UpdateStatusInst.GetLogData != null)
                    this.grdPoLog.DataSource = ClsUpdatePOStatus.UpdateStatusInst.GetLogData;

                this.grdPoLog.DisplayLayout.Bands[0].Columns[0].SortIndicator = SortIndicator.Descending;
                this.grdPoLog.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.Descending;
            }
            catch(Exception ex) {}
        }

        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOLogScreen));
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.grdPoLog = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.btnPurge = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lastDaysNumericEditor = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            ((System.ComponentModel.ISupportInitialize)(this.grdPoLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lastDaysNumericEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // grdPoLog
            // 
            this.grdPoLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BackColorDisabled = System.Drawing.Color.White;
            appearance4.BackColorDisabled2 = System.Drawing.Color.White;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance4.BorderColor = System.Drawing.Color.White;
            appearance4.TextHAlignAsString = "Center";
            this.grdPoLog.DisplayLayout.Appearance = appearance4;
            this.grdPoLog.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.grdPoLog.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPoLog.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance1.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.BorderColor = System.Drawing.SystemColors.Window;
            this.grdPoLog.DisplayLayout.GroupByBox.Appearance = appearance1;
            appearance2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdPoLog.DisplayLayout.GroupByBox.BandLabelAppearance = appearance2;
            this.grdPoLog.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPoLog.DisplayLayout.GroupByBox.Hidden = true;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdPoLog.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdPoLog.DisplayLayout.Override.ActiveCellAppearance = appearance12;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdPoLog.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.grdPoLog.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdPoLog.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdPoLog.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdPoLog.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.grdPoLog.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdPoLog.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdPoLog.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.grdPoLog.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance5.BorderColor = System.Drawing.Color.Silver;
            appearance5.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdPoLog.DisplayLayout.Override.CellAppearance = appearance5;
            this.grdPoLog.DisplayLayout.Override.CellPadding = 0;
            this.grdPoLog.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdPoLog.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance11.TextHAlignAsString = "Left";
            this.grdPoLog.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grdPoLog.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdPoLog.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            this.grdPoLog.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdPoLog.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance8.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdPoLog.DisplayLayout.Override.TemplateAddRowAppearance = appearance8;
            this.grdPoLog.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdPoLog.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdPoLog.Location = new System.Drawing.Point(16, 42);
            this.grdPoLog.Name = "grdPoLog";
            this.grdPoLog.Size = new System.Drawing.Size(621, 169);
            this.grdPoLog.TabIndex = 0;
            // 
            // btnClose
            // 
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance16.FontData.BoldAsString = "True";
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
            this.btnClose.Appearance = appearance16;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(526, 217);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(111, 24);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close(Esc)";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance14.FontData.BoldAsString = "True";
            appearance14.ForeColor = System.Drawing.Color.White;
            appearance14.Image = ((object)(resources.GetObject("appearance14.Image")));
            this.btnSearch.Appearance = appearance14;
            this.btnSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(212, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(87, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPurge
            // 
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.Image = ((object)(resources.GetObject("appearance15.Image")));
            this.btnPurge.Appearance = appearance15;
            this.btnPurge.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnPurge.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPurge.Location = new System.Drawing.Point(315, 12);
            this.btnPurge.Name = "btnPurge";
            this.btnPurge.Size = new System.Drawing.Size(87, 24);
            this.btnPurge.TabIndex = 3;
            this.btnPurge.Text = "Purge";
            this.btnPurge.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPurge.Click += new System.EventHandler(this.btnPurge_Click);
            // 
            // ultraLabel1
            // 
            appearance13.ForeColor = System.Drawing.Color.White;
            this.ultraLabel1.Appearance = appearance13;
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(16, 13);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(91, 21);
            this.ultraLabel1.TabIndex = 5;
            this.ultraLabel1.Text = "Last Days :";
            // 
            // lastDaysNumericEditor
            // 
            this.lastDaysNumericEditor.Location = new System.Drawing.Point(97, 12);
            this.lastDaysNumericEditor.MaxValue = 99;
            this.lastDaysNumericEditor.MinValue = 0;
            this.lastDaysNumericEditor.Name = "lastDaysNumericEditor";
            this.lastDaysNumericEditor.PromptChar = ' ';
            this.lastDaysNumericEditor.Size = new System.Drawing.Size(100, 21);
            this.lastDaysNumericEditor.TabIndex = 6;
            this.lastDaysNumericEditor.Value = 7;
            // 
            // frmPOLogScreen
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(654, 253);
            this.Controls.Add(this.lastDaysNumericEditor);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.btnPurge);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdPoLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPOLogScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PO Log Message";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPOLogScreen_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPOLogScreen_KeyDown);
            this.Load += new System.EventHandler(this.frmPOLogScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdPoLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lastDaysNumericEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void frmPOLogScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                this.FindForm().Hide();
                frmMain.FromPanel = false;
            }
        }

        private void frmPOLogScreen_KeyDown(object sender, KeyEventArgs e)
        {
		    if(e.KeyData == Keys.Escape)
				this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex){}
        }
        private void PurgeLogData(String retentionInDays)
        {
            MessageLog msgLog = null;
            MessageLogRow [] logRowList=null;
            DateTime retentionTime;
            String filterExpression = String.Empty; 
            try
            {
             
                    msgLog = new MessageLog();
                    filterExpression = " Where LogDate < '" + DateTime.Now + "'";
                    msgLogData = msgLog.PopulateList(filterExpression);  
                    retentionTime = DateTime.Now.AddDays(-Convert.ToDouble(retentionInDays));
                    filterExpression = "LogDate < '" + retentionTime + "'";
                    logRowList = (MessageLogRow[])msgLogData.Tables[0].Select(filterExpression);
                    foreach (MessageLogRow msgLogRow in logRowList)
                    {                       
                        msgLogRow.Delete(); 
                    }             
                  
                    msgLog.Persist(msgLogData);
                    msgLogData.AcceptChanges();
                    grdPoLog.DataSource = msgLogData;
                    grdPoLog.Refresh();             
            
            }
            catch (Exception ex)
            {
            }
        }
        private void UptoDateLogData()
        {
            MessageLog msgLog = null;
            MessageLogData msgLogUptoDate = null;
            String strNoOfDays = String.Empty;
            DateTime uptoDate;
            try
            {
                strNoOfDays = lastDaysNumericEditor.Value.ToString ();  
                uptoDate = DateTime.Now.AddDays(-Convert.ToDouble(strNoOfDays));

                //Changes by Atul on 12-10-2010 
                uptoDate = DateTime.Now.AddDays(-Convert.ToDouble(strNoOfDays) - 1);
                uptoDate = uptoDate.Date;

                msgLog = new MessageLog();
                msgLogUptoDate = msgLog.Populate(uptoDate);
                msgLogData = msgLogUptoDate;
                this.grdPoLog.DataSource = msgLogUptoDate;
                this.grdPoLog.DisplayLayout.Bands[0].Columns[0].SortIndicator = SortIndicator.Descending;
                this.grdPoLog.DisplayLayout.Bands[0].Columns[1].SortIndicator = SortIndicator.Descending;
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }
        }
        private void frmPOLogScreen_Load(object sender, EventArgs e)
        {
            try
            {

                UptoDateLogData();
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }
        }
        #region commented
        //private bool IsValidate(String noOfDays)
        //{
        //    bool isInt = false;
        //    bool isValid = false;
        //    char[] noOfDaysInChar = null;
        //    int days;
        //    try
        //    {
        //        noOfDaysInChar = noOfDays.ToCharArray();
        //        isInt = Int32.TryParse(noOfDays, out days);
        //        if (noOfDaysInChar.Length > 2)
        //        {
        //            if (isInt)
        //                clsUIHelper.ShowErrorMsg(clsPOSDBConstants.LogErrorMessage1);
        //            else
        //                clsUIHelper.ShowErrorMsg(clsPOSDBConstants.LogErrorMessage2);
        //            isValid = false;
        //        }
        //        else
        //        {
        //            if (isInt)
        //            {
        //                isValid = true;
        //            }
        //            else
        //            {
        //                clsUIHelper.ShowErrorMsg(clsPOSDBConstants.LogErrorMessage2);
        //                isValid = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return isValid;
        //}
        #endregion
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                grdPoLog.DataSource = null;
                UptoDateLogData();
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }
        }

        private void btnPurge_Click(object sender, EventArgs e)
        {
            PurgeLogData(lastDaysNumericEditor.Value.ToString());
        }
    }
}
