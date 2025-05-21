using System.Collections.Generic;
namespace POS_Core_UI
{
    partial class usrDateRangeParams
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cusdtToDate = new POS_Core_UI.usrCustomDateTime();
            this.cusdtFromDate = new POS_Core_UI.usrCustomDateTime();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // dtFromDate
            // 
            this.dtFromDate.DateTime = new System.DateTime(2008, 4, 1, 0, 0, 0, 0);
            this.dtFromDate.Location = new System.Drawing.Point(128, 35);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(100, 21);
            this.dtFromDate.TabIndex = 28;
            this.dtFromDate.Tag = "From Date";
            this.dtFromDate.Value = new System.DateTime(2008, 4, 1, 0, 0, 0, 0);
            // 
            // dtToDate
            // 
            this.dtToDate.DateTime = new System.DateTime(2008, 4, 1, 0, 0, 0, 0);
            this.dtToDate.Location = new System.Drawing.Point(390, 35);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(96, 21);
            this.dtToDate.TabIndex = 31;
            this.dtToDate.Tag = "To Date";
            this.dtToDate.Value = new System.DateTime(2008, 4, 1, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(36, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 29;
            this.label1.Text = "From Fill Date";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(322, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 42;
            this.label2.Text = "To Fill Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cusdtToDate
            // 
            this.cusdtToDate.AutoSize = true;
            this.cusdtToDate.DateSelected = "0";
            this.cusdtToDate.Location = new System.Drawing.Point(392, 33);
            this.cusdtToDate.Name = "cusdtToDate";
            this.cusdtToDate.Size = new System.Drawing.Size(183, 26);
            this.cusdtToDate.TabIndex = 31;
            this.cusdtToDate.Tag = "To Fill Date";
            this.cusdtToDate.Visible = false;
            this.cusdtToDate.Validating += new System.ComponentModel.CancelEventHandler(this.cusdtToDate_Validating);
            // 
            // cusdtFromDate
            // 
            this.cusdtFromDate.AutoSize = true;
            this.cusdtFromDate.DateSelected = "0";
            this.cusdtFromDate.Location = new System.Drawing.Point(127, 33);
            this.cusdtFromDate.Name = "cusdtFromDate";
            this.cusdtFromDate.Size = new System.Drawing.Size(183, 26);
            this.cusdtFromDate.TabIndex = 28;
            this.cusdtFromDate.Tag = "From Fill Date";
            this.cusdtFromDate.Visible = false;
            this.cusdtFromDate.Validating += new System.ComponentModel.CancelEventHandler(this.cusdtFromDate_Validating);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // usrDateRangeParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtToDate);
            this.Controls.Add(this.cusdtToDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtFromDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cusdtFromDate);
            this.Name = "usrDateRangeParams";
            this.Size = new System.Drawing.Size(760, 124);
            this.Load += new System.EventHandler(this.usrDailyLogReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate;
        public Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate;
        private System.Windows.Forms.Label label1;        
        public bool calledFromCommLine = false;
        public usrCustomDateTime cusdtFromDate;
        public usrCustomDateTime cusdtToDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
