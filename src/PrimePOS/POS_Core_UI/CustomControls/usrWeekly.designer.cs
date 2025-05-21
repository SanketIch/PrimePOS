namespace POS_Core_UI
{
    partial class usrWeekly
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
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtRecur = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.pnl = new Infragistics.Win.Misc.UltraPanel();
            this.lblError = new Infragistics.Win.Misc.UltraLabel();
            this.chkSaturday = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkFriday = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkThursday = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkWednesday = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkTuesday = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkMonday = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkSunday = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtRecur)).BeginInit();
            this.pnl.ClientArea.SuspendLayout();
            this.pnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSaturday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFriday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkThursday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWednesday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTuesday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMonday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSunday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ultraLabel1.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(16, 20);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(78, 13);
            this.ultraLabel1.TabIndex = 19;
            this.ultraLabel1.Text = "Re&cur Every:";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ultraLabel4.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(143, 19);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(95, 13);
            this.ultraLabel4.TabIndex = 18;
            this.ultraLabel4.Text = "Weeks On:";
            // 
            // txtRecur
            // 
            this.txtRecur.Location = new System.Drawing.Point(98, 16);
            this.txtRecur.MaskInput = "nnnnnnn";
            this.txtRecur.MaxValue = 52;
            this.txtRecur.MinValue = 1;
            this.txtRecur.Name = "txtRecur";
            this.txtRecur.PromptChar = ' ';
            this.txtRecur.Size = new System.Drawing.Size(41, 21);
            this.txtRecur.TabIndex = 27;
            this.txtRecur.Value = 1;
            this.txtRecur.Validating += new System.ComponentModel.CancelEventHandler(this.txtRecur_Validating);
            // 
            // pnl
            // 
            this.pnl.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            // 
            // pnl.ClientArea
            // 
            this.pnl.ClientArea.Controls.Add(this.lblError);
            this.pnl.ClientArea.Controls.Add(this.chkSaturday);
            this.pnl.ClientArea.Controls.Add(this.chkFriday);
            this.pnl.ClientArea.Controls.Add(this.chkThursday);
            this.pnl.ClientArea.Controls.Add(this.chkWednesday);
            this.pnl.ClientArea.Controls.Add(this.chkTuesday);
            this.pnl.ClientArea.Controls.Add(this.chkMonday);
            this.pnl.ClientArea.Controls.Add(this.chkSunday);
            this.pnl.Location = new System.Drawing.Point(16, 39);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(350, 81);
            this.pnl.TabIndex = 28;
            // 
            // lblError
            // 
            this.lblError.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblError.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.Location = new System.Drawing.Point(8, 3);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(11, 13);
            this.lblError.TabIndex = 35;
            // 
            // chkSaturday
            // 
            this.chkSaturday.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkSaturday.Location = new System.Drawing.Point(177, 43);
            this.chkSaturday.Name = "chkSaturday";
            this.chkSaturday.Size = new System.Drawing.Size(79, 20);
            this.chkSaturday.TabIndex = 33;
            this.chkSaturday.Text = "Saturday:";
            // 
            // chkFriday
            // 
            this.chkFriday.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkFriday.Location = new System.Drawing.Point(94, 43);
            this.chkFriday.Name = "chkFriday";
            this.chkFriday.Size = new System.Drawing.Size(79, 20);
            this.chkFriday.TabIndex = 32;
            this.chkFriday.Text = "Friday:";
            // 
            // chkThursday
            // 
            this.chkThursday.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkThursday.Location = new System.Drawing.Point(8, 43);
            this.chkThursday.Name = "chkThursday";
            this.chkThursday.Size = new System.Drawing.Size(79, 20);
            this.chkThursday.TabIndex = 31;
            this.chkThursday.Text = "Thursday:";
            // 
            // chkWednesday
            // 
            this.chkWednesday.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkWednesday.Location = new System.Drawing.Point(255, 17);
            this.chkWednesday.Name = "chkWednesday";
            this.chkWednesday.Size = new System.Drawing.Size(87, 20);
            this.chkWednesday.TabIndex = 30;
            this.chkWednesday.Text = "Wednesday:";
            // 
            // chkTuesday
            // 
            this.chkTuesday.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTuesday.Location = new System.Drawing.Point(177, 17);
            this.chkTuesday.Name = "chkTuesday";
            this.chkTuesday.Size = new System.Drawing.Size(79, 20);
            this.chkTuesday.TabIndex = 29;
            this.chkTuesday.Text = "Tuesday:";
            // 
            // chkMonday
            // 
            this.chkMonday.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkMonday.Location = new System.Drawing.Point(94, 17);
            this.chkMonday.Name = "chkMonday";
            this.chkMonday.Size = new System.Drawing.Size(79, 20);
            this.chkMonday.TabIndex = 28;
            this.chkMonday.Text = "Monday:";
            // 
            // chkSunday
            // 
            this.chkSunday.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkSunday.Checked = true;
            this.chkSunday.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSunday.Location = new System.Drawing.Point(8, 17);
            this.chkSunday.Name = "chkSunday";
            this.chkSunday.Size = new System.Drawing.Size(79, 20);
            this.chkSunday.TabIndex = 27;
            this.chkSunday.Text = "Sunday:";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // usrWeekly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnl);
            this.Controls.Add(this.txtRecur);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.ultraLabel4);
            this.Name = "usrWeekly";
            this.Size = new System.Drawing.Size(369, 123);
            this.Enter += new System.EventHandler(this.usrWeekly_Enter);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.usrWeekly_Validating);
            ((System.ComponentModel.ISupportInitialize)(this.txtRecur)).EndInit();
            this.pnl.ClientArea.ResumeLayout(false);
            this.pnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkSaturday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFriday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkThursday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWednesday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTuesday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMonday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSunday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor txtRecur;
        private Infragistics.Win.Misc.UltraPanel pnl;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSaturday;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkFriday;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkThursday;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkWednesday;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkTuesday;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkMonday;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkSunday;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Infragistics.Win.Misc.UltraLabel lblError;
    }
}
