namespace POS_Core_UI
{
    public partial class usrCustomDateTime
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            this.cmbDateOptions = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.ndDays = new System.Windows.Forms.NumericUpDown();
            this.lblDays = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDateOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndDays)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbDateOptions
            // 
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Right;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.cmbDateOptions.Appearance = appearance1;
            this.cmbDateOptions.DropDownListWidth = 120;
            this.cmbDateOptions.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "1";
            valueListItem1.DisplayText = "Today\'s Date";
            appearance2.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Right;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            valueListItem2.Appearance = appearance2;
            valueListItem2.DataValue = "2";
            valueListItem2.DisplayText = "Today\'s Date +";
            appearance3.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Right;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            valueListItem3.Appearance = appearance3;
            valueListItem3.DataValue = "3";
            valueListItem3.DisplayText = "Today\'s Date -";
            valueListItem4.DataValue = "4";
            valueListItem4.DisplayText = "Fixed Date";
            this.cmbDateOptions.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2,
            valueListItem3,
            valueListItem4});
            this.cmbDateOptions.Location = new System.Drawing.Point(1, 2);
            this.cmbDateOptions.Name = "cmbDateOptions";
            this.cmbDateOptions.ShowOverflowIndicator = true;
            this.cmbDateOptions.Size = new System.Drawing.Size(108, 21);
            this.cmbDateOptions.TabIndex = 0;
            this.cmbDateOptions.Tag = "Custom Date Option";
            this.cmbDateOptions.ValueChanged += new System.EventHandler(this.cmbDateOptions_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(111, 2);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(76, 20);
            this.dtpDate.TabIndex = 2;
            this.dtpDate.Tag = "Custom Date Time";
            this.dtpDate.Visible = false;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // ndDays
            // 
            this.ndDays.Location = new System.Drawing.Point(111, 2);
            this.ndDays.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.ndDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndDays.Name = "ndDays";
            this.ndDays.Size = new System.Drawing.Size(46, 20);
            this.ndDays.TabIndex = 3;
            this.ndDays.Tag = "Custom nd Days";
            this.ndDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ndDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndDays.Visible = false;
            this.ndDays.ValueChanged += new System.EventHandler(this.ndDays_ValueChanged);
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = true;
            this.lblDays.Location = new System.Drawing.Point(159, 5);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(31, 13);
            this.lblDays.TabIndex = 4;
            this.lblDays.Text = "Days";
            this.lblDays.Visible = false;
            // 
            // usrCustomDateTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.lblDays);
            this.Controls.Add(this.ndDays);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDateOptions);
            this.Name = "usrCustomDateTime";
            this.Size = new System.Drawing.Size(197, 26);
            this.Load += new System.EventHandler(this.UserControl2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbDateOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ndDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbDateOptions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.NumericUpDown ndDays;
        private System.Windows.Forms.Label lblDays;
    }
}
