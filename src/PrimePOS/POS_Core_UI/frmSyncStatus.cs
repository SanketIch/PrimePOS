using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Net.Mail;
using System.Net;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.Resources;

namespace POS_Core_UI
{
    /// <summary>
    /// Sync status
    /// </summary>
    public class frmSyncStatus : System.Windows.Forms.Form
    {
        private BackgroundWorker bw = new BackgroundWorker();
        private int lblIndex = 1;
        public string FaxId = "";
        Thread tr = null;
        private bool m_FirstTime = true;
        private bool mClosed = false;
        private Infragistics.Win.Misc.UltraButton cmdOk;
        private Infragistics.Win.Misc.UltraGroupBox gbLine;
        private Infragistics.Win.Misc.UltraLabel lbl8;
        private Infragistics.Win.Misc.UltraLabel lbl7;
        private Infragistics.Win.Misc.UltraLabel lbl6;
        private Infragistics.Win.Misc.UltraLabel lbl5;
        private Infragistics.Win.Misc.UltraLabel lbl4;
        private Infragistics.Win.Misc.UltraLabel lbl3;
        private Infragistics.Win.Misc.UltraLabel lbl2;
        private Infragistics.Win.Misc.UltraLabel lbl1;
        private System.Windows.Forms.Timer tmrLights;
        private Infragistics.Win.Misc.UltraLabel lbl9;
        private Infragistics.Win.Misc.UltraLabel lblStatusDescription;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTask;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtTaskName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel lblPages;
        private Infragistics.Win.Misc.UltraLabel lblResolution;
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar pbStatus;
        private Infragistics.Win.Misc.UltraButton txtMinimize;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer tmrShowStatus;
        public DataTable dt = new DataTable();
        public ScheduledTasksData oScheduledTasksData = new ScheduledTasksData();

        public frmSyncStatus()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_DoWork);
            this.bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            this.cmdOk = new Infragistics.Win.Misc.UltraButton();
            this.gbLine = new Infragistics.Win.Misc.UltraGroupBox();
            this.lblStatusDescription = new Infragistics.Win.Misc.UltraLabel();
            this.lbl9 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl8 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl7 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl6 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl5 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl4 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl3 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl2 = new Infragistics.Win.Misc.UltraLabel();
            this.lbl1 = new Infragistics.Win.Misc.UltraLabel();
            this.tmrLights = new System.Windows.Forms.Timer(this.components);
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtTask = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTaskName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblPages = new Infragistics.Win.Misc.UltraLabel();
            this.lblResolution = new Infragistics.Win.Misc.UltraLabel();
            this.pbStatus = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.txtMinimize = new Infragistics.Win.Misc.UltraButton();
            this.tmrShowStatus = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gbLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaskName)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdOk.Location = new System.Drawing.Point(386, 207);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(88, 32);
            this.cmdOk.TabIndex = 1;
            this.cmdOk.Text = "Close";
            this.cmdOk.Visible = false;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // gbLine
            // 
            this.gbLine.BackColorInternal = System.Drawing.Color.DimGray;
            this.gbLine.Location = new System.Drawing.Point(0, 132);
            this.gbLine.Name = "gbLine";
            this.gbLine.Size = new System.Drawing.Size(491, 5);
            this.gbLine.TabIndex = 4;
            // 
            // lblStatusDescription
            // 
            appearance1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblStatusDescription.Appearance = appearance1;
            this.lblStatusDescription.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusDescription.Location = new System.Drawing.Point(18, 148);
            this.lblStatusDescription.Name = "lblStatusDescription";
            this.lblStatusDescription.Size = new System.Drawing.Size(405, 16);
            this.lblStatusDescription.TabIndex = 5;
            this.lblStatusDescription.Text = "Waiting Response...";
            this.lblStatusDescription.TextChanged += new System.EventHandler(this.lblStatusDescription_TextChanged);
            // 
            // lbl9
            // 
            appearance2.BorderColor = System.Drawing.Color.Gray;
            this.lbl9.Appearance = appearance2;
            this.lbl9.BackColorInternal = System.Drawing.Color.DarkSeaGreen;
            this.lbl9.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl9.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl9.Location = new System.Drawing.Point(313, 224);
            this.lbl9.Name = "lbl9";
            this.lbl9.Size = new System.Drawing.Size(16, 8);
            this.lbl9.TabIndex = 32;
            // 
            // lbl8
            // 
            appearance3.BorderColor = System.Drawing.Color.Gray;
            this.lbl8.Appearance = appearance3;
            this.lbl8.BackColorInternal = System.Drawing.Color.DarkSeaGreen;
            this.lbl8.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl8.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl8.Location = new System.Drawing.Point(284, 224);
            this.lbl8.Name = "lbl8";
            this.lbl8.Size = new System.Drawing.Size(16, 8);
            this.lbl8.TabIndex = 31;
            // 
            // lbl7
            // 
            appearance4.BorderColor = System.Drawing.Color.Gray;
            this.lbl7.Appearance = appearance4;
            this.lbl7.BackColorInternal = System.Drawing.Color.DarkSeaGreen;
            this.lbl7.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl7.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl7.Location = new System.Drawing.Point(255, 224);
            this.lbl7.Name = "lbl7";
            this.lbl7.Size = new System.Drawing.Size(16, 8);
            this.lbl7.TabIndex = 30;
            // 
            // lbl6
            // 
            appearance5.BorderColor = System.Drawing.Color.Gray;
            this.lbl6.Appearance = appearance5;
            this.lbl6.BackColorInternal = System.Drawing.Color.DarkSeaGreen;
            this.lbl6.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl6.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl6.Location = new System.Drawing.Point(226, 224);
            this.lbl6.Name = "lbl6";
            this.lbl6.Size = new System.Drawing.Size(16, 8);
            this.lbl6.TabIndex = 29;
            // 
            // lbl5
            // 
            appearance6.BorderColor = System.Drawing.Color.Gray;
            this.lbl5.Appearance = appearance6;
            this.lbl5.BackColorInternal = System.Drawing.Color.Yellow;
            this.lbl5.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl5.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl5.Location = new System.Drawing.Point(197, 224);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(16, 8);
            this.lbl5.TabIndex = 28;
            // 
            // lbl4
            // 
            appearance7.BorderColor = System.Drawing.Color.Gray;
            this.lbl4.Appearance = appearance7;
            this.lbl4.BackColorInternal = System.Drawing.Color.DarkSeaGreen;
            this.lbl4.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl4.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl4.Location = new System.Drawing.Point(168, 224);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(16, 8);
            this.lbl4.TabIndex = 27;
            // 
            // lbl3
            // 
            appearance8.BorderColor = System.Drawing.Color.Gray;
            this.lbl3.Appearance = appearance8;
            this.lbl3.BackColorInternal = System.Drawing.Color.DarkSeaGreen;
            this.lbl3.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl3.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl3.Location = new System.Drawing.Point(139, 224);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(16, 8);
            this.lbl3.TabIndex = 26;
            // 
            // lbl2
            // 
            appearance9.BorderColor = System.Drawing.Color.Gray;
            this.lbl2.Appearance = appearance9;
            this.lbl2.BackColorInternal = System.Drawing.Color.DarkSeaGreen;
            this.lbl2.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl2.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl2.Location = new System.Drawing.Point(110, 224);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(16, 8);
            this.lbl2.TabIndex = 25;
            // 
            // lbl1
            // 
            appearance10.BorderColor = System.Drawing.Color.Gray;
            this.lbl1.Appearance = appearance10;
            this.lbl1.BackColorInternal = System.Drawing.Color.DarkSeaGreen;
            this.lbl1.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl1.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lbl1.Location = new System.Drawing.Point(81, 224);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(16, 8);
            this.lbl1.TabIndex = 24;
            // 
            // tmrLights
            // 
            this.tmrLights.Interval = 100;
            this.tmrLights.Tick += new System.EventHandler(this.tmrLights_Tick);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.txtTask);
            this.ultraGroupBox1.Controls.Add(this.txtDescription);
            this.ultraGroupBox1.Controls.Add(this.txtTaskName);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel3);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel2);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Location = new System.Drawing.Point(9, 7);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(466, 114);
            this.ultraGroupBox1.TabIndex = 36;
            this.ultraGroupBox1.Text = "Task Initials";
            // 
            // txtTask
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            this.txtTask.Appearance = appearance11;
            this.txtTask.BackColor = System.Drawing.Color.Transparent;
            this.txtTask.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtTask.Location = new System.Drawing.Point(152, 75);
            this.txtTask.Name = "txtTask";
            this.txtTask.Size = new System.Drawing.Size(300, 18);
            this.txtTask.TabIndex = 41;
            this.txtTask.TabStop = false;
            this.txtTask.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtDescription
            // 
            appearance12.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescription.Appearance = appearance12;
            this.txtDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtDescription.Location = new System.Drawing.Point(152, 48);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(300, 18);
            this.txtDescription.TabIndex = 40;
            this.txtDescription.TabStop = false;
            this.txtDescription.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtTaskName
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Control;
            this.txtTaskName.Appearance = appearance13;
            this.txtTaskName.BackColor = System.Drawing.SystemColors.Control;
            this.txtTaskName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtTaskName.Location = new System.Drawing.Point(152, 21);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(300, 18);
            this.txtTaskName.TabIndex = 39;
            this.txtTaskName.TabStop = false;
            this.txtTaskName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Location = new System.Drawing.Point(24, 78);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(110, 16);
            this.ultraLabel3.TabIndex = 38;
            this.ultraLabel3.Text = "Task :";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Location = new System.Drawing.Point(24, 50);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(118, 19);
            this.ultraLabel2.TabIndex = 37;
            this.ultraLabel2.Text = "Description :";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(24, 23);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(110, 19);
            this.ultraLabel1.TabIndex = 36;
            this.ultraLabel1.Text = "Task Name :";
            // 
            // lblPages
            // 
            appearance14.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblPages.Appearance = appearance14;
            this.lblPages.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPages.Location = new System.Drawing.Point(18, 172);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(223, 16);
            this.lblPages.TabIndex = 37;
            // 
            // lblResolution
            // 
            appearance15.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblResolution.Appearance = appearance15;
            this.lblResolution.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResolution.Location = new System.Drawing.Point(18, 196);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(223, 16);
            this.lblResolution.TabIndex = 38;
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(232, 176);
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(245, 16);
            this.pbStatus.Style = Infragistics.Win.UltraWinProgressBar.ProgressBarStyle.Segmented;
            this.pbStatus.TabIndex = 39;
            this.pbStatus.Text = "[Formatted]";
            this.pbStatus.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // txtMinimize
            // 
            this.txtMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMinimize.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.txtMinimize.Font = new System.Drawing.Font("Verdana", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinimize.Location = new System.Drawing.Point(347, 207);
            this.txtMinimize.Name = "txtMinimize";
            this.txtMinimize.Size = new System.Drawing.Size(36, 32);
            this.txtMinimize.TabIndex = 40;
            this.txtMinimize.Text = "-";
            this.txtMinimize.Visible = false;
            this.txtMinimize.Click += new System.EventHandler(this.txtMinimize_Click);
            // 
            // tmrShowStatus
            // 
            this.tmrShowStatus.Interval = 100;
            this.tmrShowStatus.Tick += new System.EventHandler(this.tmrShowStatus_Tick);
            // 
            // frmSyncStatus
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 14);
            this.CancelButton = this.cmdOk;
            this.ClientSize = new System.Drawing.Size(485, 246);
            this.ControlBox = false;
            this.Controls.Add(this.txtMinimize);
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.lblResolution);
            this.Controls.Add(this.lblPages);
            this.Controls.Add(this.lbl9);
            this.Controls.Add(this.lbl8);
            this.Controls.Add(this.lbl7);
            this.Controls.Add(this.lbl6);
            this.Controls.Add(this.lbl5);
            this.Controls.Add(this.lbl4);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.lblStatusDescription);
            this.Controls.Add(this.gbLine);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.cmdOk);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmSyncStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sync Status";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmSyncStatus_Closing);
            this.Load += new System.EventHandler(this.frmSyncStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaskName)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void frmSyncStatus_Load(object sender, System.EventArgs e)
        {
            try
            {
                tmrLights.Start();
                tmrShowStatus.Start();
                bw.RunWorkerAsync();
            }
            catch (ThreadAbortException exp)
            {
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void tmrLights_Tick(object sender, System.EventArgs e)
        {
            try
            {
                lbl1.BackColor = ((lblIndex + 1 == 1) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);
                lbl2.BackColor = ((lblIndex + 1 == 2) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);
                lbl3.BackColor = ((lblIndex + 1 == 3) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);
                lbl4.BackColor = ((lblIndex + 1 == 4) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);
                lbl5.BackColor = ((lblIndex + 1 == 5) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);
                lbl6.BackColor = ((lblIndex + 1 == 6) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);
                lbl7.BackColor = ((lblIndex + 1 == 7) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);
                lbl8.BackColor = ((lblIndex + 1 == 8) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);
                lbl9.BackColor = ((lblIndex + 1 == 9) ? System.Drawing.Color.Yellow : System.Drawing.Color.DarkSeaGreen);

                lblIndex = lblIndex + 1;
                lblIndex = (lblIndex % 9);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ToString());
            }
        }

        public void showStatus()
        {
            IScheduledControl oPerformTaskBaseControl = null;
            DataTable tempdt = new DataTable();
            ManageScheduledTask.DeleteAllTask();
            this.pbStatus.Minimum = 0;
            this.pbStatus.Maximum = oScheduledTasksData.ScheduledTasks.Rows.Count;
            ScheduledTasks oScheduledTasks = new ScheduledTasks();
            foreach (ScheduledTasksRow dr in oScheduledTasksData.ScheduledTasks.Rows)
            {
                txtTaskName.Text = Configuration.convertNullToString(dr.TaskName);
                txtDescription.Text = Configuration.convertNullToString(dr.TaskDescription);
                txtTask.Text = oScheduledTasks.GetTask(dr.TaskId);
                Thread.Sleep(5000);

                switch (dr.PerformTask)
                {
                    case 0:
                        oPerformTaskBaseControl = new usrDaily();
                        break;
                    //case 1:
                    //    oPerformTaskBaseControl = new usrWeekly();
                    //    break;
                    //case 2:
                    //    oPerformTaskBaseControl = new usrMonthly();
                    //    break;
                    case 3:
                        oPerformTaskBaseControl = null;
                        break;
                }

                if (oPerformTaskBaseControl != null)
                    if (oPerformTaskBaseControl.GetType().Name == "usrDaily")
                        oPerformTaskBaseControl.SetObject(dr.RepeatTask);
                    else
                        oPerformTaskBaseControl.SetObject(dr.ScheduledTasksID);

                ManageScheduledTask.addModifyScheuled(dr, oPerformTaskBaseControl);

                if (this.pbStatus.InvokeRequired)
                    Invoke(new addPrograssValuedelegate(addPrograssValue), this.pbStatus.Value);
                else
                    this.pbStatus.Value = this.pbStatus.Value + 1;
            }
        }

        private void addPrograssValue(int iValue)
        {
            this.pbStatus.Value = iValue + 1;
        }

        private delegate void addPrograssValuedelegate(int ivalue);

        private void cmdOk_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (ThreadAbortException exp)
            {
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ToString());
            }
        }

        private void frmSyncStatus_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                tmrLights.Stop();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.ToString());
            }
        }

        private void txtMinimize_Click(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void lblStatusDescription_TextChanged(object sender, System.EventArgs e)
        {
            this.Text = lblStatusDescription.Text;
        }

        private void tmrShowStatus_Tick(object sender, System.EventArgs e)
        {
            try
            {
                tmrShowStatus.Stop();
            }
            catch (Exception exp)
            {
                //clsUIHelper.ShowErrorMsg(exp.ToString());
            }
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            showStatus();
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception exp)
            {
            }
        }
    }
}
