namespace POS_Core_UI
{
    partial class frmItemAdd
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemAdd));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.btnSimpleMode = new Infragistics.Win.Misc.UltraButton();
            this.btnQuickMode = new Infragistics.Win.Misc.UltraButton();
            this.btnAdvanceMode = new Infragistics.Win.Misc.UltraButton();
            this.lblItemId = new Infragistics.Win.Misc.UltraLabel();
            this.lblItemMode = new Infragistics.Win.Misc.UltraLabel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnMMSSearch = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSimpleMode
            // 
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            this.btnSimpleMode.Appearance = appearance1;
            this.btnSimpleMode.Location = new System.Drawing.Point(13, 31);
            this.btnSimpleMode.Name = "btnSimpleMode";
            this.btnSimpleMode.Size = new System.Drawing.Size(109, 35);
            this.btnSimpleMode.TabIndex = 0;
            this.btnSimpleMode.TabStop = false;
            this.btnSimpleMode.Text = "&Simple Mode    <S>";
            this.btnSimpleMode.Click += new System.EventHandler(this.btnSimpleMode_Click);
            this.btnSimpleMode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSimpleMode_KeyDown);
            // 
            // btnQuickMode
            // 
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnQuickMode.Appearance = appearance2;
            this.btnQuickMode.Location = new System.Drawing.Point(129, 31);
            this.btnQuickMode.Name = "btnQuickMode";
            this.btnQuickMode.Size = new System.Drawing.Size(109, 35);
            this.btnQuickMode.TabIndex = 1;
            this.btnQuickMode.TabStop = false;
            this.btnQuickMode.Text = "&Quick Mode      <Q>";
            this.btnQuickMode.Click += new System.EventHandler(this.btnQuickMode_Click);
            // 
            // btnAdvanceMode
            // 
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            this.btnAdvanceMode.Appearance = appearance3;
            this.btnAdvanceMode.Location = new System.Drawing.Point(246, 31);
            this.btnAdvanceMode.Name = "btnAdvanceMode";
            this.btnAdvanceMode.Size = new System.Drawing.Size(109, 35);
            this.btnAdvanceMode.TabIndex = 2;
            this.btnAdvanceMode.TabStop = false;
            this.btnAdvanceMode.Text = "&Advanced Mode <A>";
            this.btnAdvanceMode.Click += new System.EventHandler(this.btnAdvanceMode_Click);
            // 
            // lblItemId
            // 
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            appearance4.BorderColor2 = System.Drawing.Color.Fuchsia;
            appearance4.FontData.Name = "Verdana";
            appearance4.FontData.SizeInPoints = 10F;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.TextHAlignAsString = "Left";
            this.lblItemId.Appearance = appearance4;
            this.lblItemId.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblItemId.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemId.Location = new System.Drawing.Point(6, 11);
            this.lblItemId.Name = "lblItemId";
            this.lblItemId.Size = new System.Drawing.Size(598, 30);
            this.lblItemId.TabIndex = 3;
            this.lblItemId.Text = "Item you entered is not in the System.";
            this.lblItemId.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.lblItemId.Click += new System.EventHandler(this.lblItemId_Click);
            // 
            // lblItemMode
            // 
            appearance5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            appearance5.BorderColor2 = System.Drawing.Color.Fuchsia;
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.Name = "Verdana";
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.lblItemMode.Appearance = appearance5;
            this.lblItemMode.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblItemMode.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemMode.Location = new System.Drawing.Point(6, 45);
            this.lblItemMode.Name = "lblItemMode";
            this.lblItemMode.Size = new System.Drawing.Size(598, 110);
            this.lblItemMode.TabIndex = 5;
            this.lblItemMode.Text = "Item you entered is not in the System.";
            this.lblItemMode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnCancel
            // 
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            this.btnCancel.Appearance = appearance6;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(478, 31);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ultraGroupBox1
            // 
            appearance7.BorderColor = System.Drawing.Color.DarkGray;
            this.ultraGroupBox1.ContentAreaAppearance = appearance7;
            this.ultraGroupBox1.Controls.Add(this.btnMMSSearch);
            this.ultraGroupBox1.Controls.Add(this.btnSimpleMode);
            this.ultraGroupBox1.Controls.Add(this.btnCancel);
            this.ultraGroupBox1.Controls.Add(this.btnAdvanceMode);
            this.ultraGroupBox1.Controls.Add(this.btnQuickMode);
            appearance9.BackColor = System.Drawing.Color.LightSkyBlue;
            appearance9.BorderColor = System.Drawing.Color.DarkGray;
            appearance9.FontData.BoldAsString = "True";
            this.ultraGroupBox1.HeaderAppearance = appearance9;
            this.ultraGroupBox1.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGroupBox1.Location = new System.Drawing.Point(6, 161);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(598, 75);
            this.ultraGroupBox1.TabIndex = 7;
            this.ultraGroupBox1.Text = "Options for Adding New Item";
            // 
            // btnMMSSearch
            // 
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.Black;
            this.btnMMSSearch.Appearance = appearance8;
            this.btnMMSSearch.Location = new System.Drawing.Point(364, 31);
            this.btnMMSSearch.Name = "btnMMSSearch";
            this.btnMMSSearch.Size = new System.Drawing.Size(104, 35);
            this.btnMMSSearch.TabIndex = 3;
            this.btnMMSSearch.TabStop = false;
            this.btnMMSSearch.Text = "&MMS Search <M>";
            this.btnMMSSearch.Click += new System.EventHandler(this.btnMMSSearch_Click);
            // 
            // frmItemAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(611, 241);
            this.ControlBox = false;
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.lblItemMode);
            this.Controls.Add(this.lblItemId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(295, 187);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmItemAdd";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Item Not Found";
            this.Load += new System.EventHandler(this.frmItemAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmItemAdd_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnSimpleMode;
        private Infragistics.Win.Misc.UltraButton btnQuickMode;
        private Infragistics.Win.Misc.UltraButton btnAdvanceMode;
        private Infragistics.Win.Misc.UltraLabel lblItemId;
        private Infragistics.Win.Misc.UltraLabel lblItemMode;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraButton btnMMSSearch;
    }
}