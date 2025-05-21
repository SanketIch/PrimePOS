namespace POS_Core_UI
{
    partial class frmAddNewItem
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddNewItem));
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
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnNextToEditItem = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbItemCode = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtSellingPrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.txtCostPrice = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.lblSellingPrice = new Infragistics.Win.Misc.UltraLabel();
            this.lblCostPrice = new Infragistics.Win.Misc.UltraLabel();
            this.lblVendItemCode = new Infragistics.Win.Misc.UltraLabel();
            this.txtVendItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.cmbVendorCode = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblVendorCode = new Infragistics.Win.Misc.UltraLabel();
            this.lblDescription = new Infragistics.Win.Misc.UltraLabel();
            this.txtItemDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblItemCode = new Infragistics.Win.Misc.UltraLabel();
            this.txtItemCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblMessage = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSellingPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendItemCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVendorCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.Navy;
            appearance1.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(0, 0);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(789, 40);
            this.lblTransactionType.TabIndex = 34;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Add New Item";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnClose
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnClose.Appearance = appearance2;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(638, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(115, 28);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnNextToEditItem
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.btnNextToEditItem.Appearance = appearance3;
            this.btnNextToEditItem.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnNextToEditItem.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextToEditItem.Location = new System.Drawing.Point(517, 20);
            this.btnNextToEditItem.Name = "btnNextToEditItem";
            this.btnNextToEditItem.Size = new System.Drawing.Size(115, 28);
            this.btnNextToEditItem.TabIndex = 7;
            this.btnNextToEditItem.Text = "&Ok";
            this.btnNextToEditItem.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNextToEditItem.Click += new System.EventHandler(this.btnNextToEditItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbItemCode);
            this.groupBox1.Controls.Add(this.txtSellingPrice);
            this.groupBox1.Controls.Add(this.txtCostPrice);
            this.groupBox1.Controls.Add(this.lblSellingPrice);
            this.groupBox1.Controls.Add(this.lblCostPrice);
            this.groupBox1.Controls.Add(this.lblVendItemCode);
            this.groupBox1.Controls.Add(this.txtVendItemCode);
            this.groupBox1.Controls.Add(this.cmbVendorCode);
            this.groupBox1.Controls.Add(this.lblVendorCode);
            this.groupBox1.Controls.Add(this.lblDescription);
            this.groupBox1.Controls.Add(this.txtItemDescription);
            this.groupBox1.Controls.Add(this.lblItemCode);
            this.groupBox1.Controls.Add(this.txtItemCode);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(6, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(775, 176);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add New Item";
            // 
            // cmbItemCode
            // 
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance4.BorderColor3DBase = System.Drawing.Color.Black;
            appearance4.FontData.BoldAsString = "False";
            appearance4.FontData.ItalicAsString = "False";
            appearance4.FontData.StrikeoutAsString = "False";
            appearance4.FontData.UnderlineAsString = "False";
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbItemCode.Appearance = appearance4;
            this.cmbItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.cmbItemCode.ButtonAppearance = appearance5;
            this.cmbItemCode.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbItemCode.Location = new System.Drawing.Point(118, 25);
            this.cmbItemCode.MaxLength = 20;
            this.cmbItemCode.Name = "cmbItemCode";
            this.cmbItemCode.Size = new System.Drawing.Size(132, 23);
            this.cmbItemCode.TabIndex = 1;
            this.cmbItemCode.SelectionChanged += new System.EventHandler(this.cmbItemCode_SelectionChanged);
            this.cmbItemCode.Enter += new System.EventHandler(this.cmbItemCode_Enter);
            this.cmbItemCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddNewItem_KeyDown);
            // 
            // txtSellingPrice
            // 
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.ItalicAsString = "False";
            appearance6.FontData.StrikeoutAsString = "False";
            appearance6.FontData.UnderlineAsString = "False";
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtSellingPrice.Appearance = appearance6;
            this.txtSellingPrice.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtSellingPrice.Location = new System.Drawing.Point(118, 126);
            this.txtSellingPrice.MaskInput = "{LOC}-nn,nnn.nn";
            this.txtSellingPrice.MaxValue = 99999999;
            this.txtSellingPrice.MinValue = -9999999;
            this.txtSellingPrice.Name = "txtSellingPrice";
            this.txtSellingPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtSellingPrice.Size = new System.Drawing.Size(132, 23);
            this.txtSellingPrice.TabIndex = 6;
            this.txtSellingPrice.Enter += new System.EventHandler(this.txtSellingPrice_Enter);
            this.txtSellingPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddNewItem_KeyDown);
            // 
            // txtCostPrice
            // 
            appearance7.FontData.BoldAsString = "False";
            appearance7.FontData.ItalicAsString = "False";
            appearance7.FontData.StrikeoutAsString = "False";
            appearance7.FontData.UnderlineAsString = "False";
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtCostPrice.Appearance = appearance7;
            this.txtCostPrice.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCostPrice.Location = new System.Drawing.Point(416, 126);
            this.txtCostPrice.MaskInput = "{LOC}-nn,nnn.nn";
            this.txtCostPrice.MaxValue = 99999999;
            this.txtCostPrice.MinValue = -9999999;
            this.txtCostPrice.Name = "txtCostPrice";
            this.txtCostPrice.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.txtCostPrice.Size = new System.Drawing.Size(132, 23);
            this.txtCostPrice.TabIndex = 5;
            this.txtCostPrice.Enter += new System.EventHandler(this.txtCostPrice_Enter);
            this.txtCostPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddNewItem_KeyDown);
            // 
            // lblSellingPrice
            // 
            appearance8.FontData.BoldAsString = "False";
            appearance8.ForeColor = System.Drawing.Color.White;
            this.lblSellingPrice.Appearance = appearance8;
            this.lblSellingPrice.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSellingPrice.Location = new System.Drawing.Point(9, 127);
            this.lblSellingPrice.Name = "lblSellingPrice";
            this.lblSellingPrice.Size = new System.Drawing.Size(99, 19);
            this.lblSellingPrice.TabIndex = 69;
            this.lblSellingPrice.Text = "Selling Price";
            this.lblSellingPrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblCostPrice
            // 
            appearance9.FontData.BoldAsString = "False";
            appearance9.ForeColor = System.Drawing.Color.White;
            this.lblCostPrice.Appearance = appearance9;
            this.lblCostPrice.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostPrice.Location = new System.Drawing.Point(272, 127);
            this.lblCostPrice.Name = "lblCostPrice";
            this.lblCostPrice.Size = new System.Drawing.Size(99, 19);
            this.lblCostPrice.TabIndex = 67;
            this.lblCostPrice.Text = "Cost Price";
            this.lblCostPrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblVendItemCode
            // 
            appearance10.FontData.BoldAsString = "False";
            appearance10.ForeColor = System.Drawing.Color.White;
            this.lblVendItemCode.Appearance = appearance10;
            this.lblVendItemCode.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendItemCode.Location = new System.Drawing.Point(272, 76);
            this.lblVendItemCode.Name = "lblVendItemCode";
            this.lblVendItemCode.Size = new System.Drawing.Size(143, 19);
            this.lblVendItemCode.TabIndex = 65;
            this.lblVendItemCode.Text = "Vendor Item Code";
            this.lblVendItemCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtVendItemCode
            // 
            appearance11.FontData.BoldAsString = "False";
            appearance11.FontData.ItalicAsString = "False";
            appearance11.FontData.StrikeoutAsString = "False";
            appearance11.FontData.UnderlineAsString = "False";
            appearance11.ForeColor = System.Drawing.Color.Black;
            appearance11.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtVendItemCode.Appearance = appearance11;
            this.txtVendItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtVendItemCode.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendItemCode.Location = new System.Drawing.Point(416, 76);
            this.txtVendItemCode.MaxLength = 10;
            this.txtVendItemCode.Name = "txtVendItemCode";
            this.txtVendItemCode.Size = new System.Drawing.Size(132, 23);
            this.txtVendItemCode.TabIndex = 4;
            this.txtVendItemCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddNewItem_KeyDown);
            // 
            // cmbVendorCode
            // 
            appearance12.BorderAlpha = Infragistics.Win.Alpha.Opaque;
            appearance12.BorderColor3DBase = System.Drawing.Color.Black;
            appearance12.FontData.BoldAsString = "False";
            appearance12.FontData.ItalicAsString = "False";
            appearance12.FontData.StrikeoutAsString = "False";
            appearance12.FontData.UnderlineAsString = "False";
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.ForeColorDisabled = System.Drawing.Color.Black;
            this.cmbVendorCode.Appearance = appearance12;
            this.cmbVendorCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance13.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.ForeColor = System.Drawing.Color.Black;
            this.cmbVendorCode.ButtonAppearance = appearance13;
            this.cmbVendorCode.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbVendorCode.Location = new System.Drawing.Point(118, 76);
            this.cmbVendorCode.MaxLength = 20;
            this.cmbVendorCode.Name = "cmbVendorCode";
            this.cmbVendorCode.Size = new System.Drawing.Size(132, 23);
            this.cmbVendorCode.TabIndex = 3;
            this.cmbVendorCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddNewItem_KeyDown);
            // 
            // lblVendorCode
            // 
            appearance14.FontData.BoldAsString = "False";
            appearance14.ForeColor = System.Drawing.Color.White;
            this.lblVendorCode.Appearance = appearance14;
            this.lblVendorCode.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendorCode.Location = new System.Drawing.Point(9, 76);
            this.lblVendorCode.Name = "lblVendorCode";
            this.lblVendorCode.Size = new System.Drawing.Size(99, 19);
            this.lblVendorCode.TabIndex = 63;
            this.lblVendorCode.Text = "Vendor Code";
            this.lblVendorCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // lblDescription
            // 
            appearance15.FontData.BoldAsString = "False";
            appearance15.ForeColor = System.Drawing.Color.White;
            this.lblDescription.Appearance = appearance15;
            this.lblDescription.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(272, 29);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(99, 19);
            this.lblDescription.TabIndex = 61;
            this.lblDescription.Text = "Description";
            this.lblDescription.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtItemDescription
            // 
            appearance16.FontData.BoldAsString = "False";
            appearance16.FontData.ItalicAsString = "False";
            appearance16.FontData.StrikeoutAsString = "False";
            appearance16.FontData.UnderlineAsString = "False";
            appearance16.ForeColor = System.Drawing.Color.Black;
            appearance16.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtItemDescription.Appearance = appearance16;
            this.txtItemDescription.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtItemDescription.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemDescription.Location = new System.Drawing.Point(416, 29);
            this.txtItemDescription.MaxLength = 10;
            this.txtItemDescription.Name = "txtItemDescription";
            this.txtItemDescription.Size = new System.Drawing.Size(336, 23);
            this.txtItemDescription.TabIndex = 2;
            this.txtItemDescription.Enter += new System.EventHandler(this.txtItemDescription_Enter);
            this.txtItemDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddNewItem_KeyDown);
            // 
            // lblItemCode
            // 
            appearance17.FontData.BoldAsString = "False";
            appearance17.ForeColor = System.Drawing.Color.White;
            this.lblItemCode.Appearance = appearance17;
            this.lblItemCode.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemCode.Location = new System.Drawing.Point(9, 25);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.Size = new System.Drawing.Size(99, 19);
            this.lblItemCode.TabIndex = 59;
            this.lblItemCode.Text = "Item Code";
            this.lblItemCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtItemCode
            // 
            appearance18.FontData.BoldAsString = "False";
            appearance18.FontData.ItalicAsString = "False";
            appearance18.FontData.StrikeoutAsString = "False";
            appearance18.FontData.UnderlineAsString = "False";
            appearance18.ForeColor = System.Drawing.Color.Black;
            appearance18.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtItemCode.Appearance = appearance18;
            this.txtItemCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtItemCode.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemCode.Location = new System.Drawing.Point(118, 25);
            this.txtItemCode.MaxLength = 13;
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(132, 23);
            this.txtItemCode.TabIndex = 1;
            this.txtItemCode.Enter += new System.EventHandler(this.txtItemCode_Enter);
            this.txtItemCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddNewItem_KeyDown);
            this.txtItemCode.Leave += new System.EventHandler(this.txtItemCode_Leave);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 240);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 50;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnNextToEditItem);
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Location = new System.Drawing.Point(6, 224);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(775, 60);
            this.groupBox2.TabIndex = 51;
            this.groupBox2.TabStop = false;
            // 
            // frmAddNewItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(789, 290);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmAddNewItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddNewItem";
            this.Load += new System.EventHandler(this.frmAddNewItem_Load);
            this.Shown += new System.EventHandler(this.frmAddNewItem_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAddNewItem_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSellingPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendItemCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVendorCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemCode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnNextToEditItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel lblSellingPrice;
        private Infragistics.Win.Misc.UltraLabel lblCostPrice;
        private Infragistics.Win.Misc.UltraLabel lblVendItemCode;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendItemCode;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbVendorCode;
        private Infragistics.Win.Misc.UltraLabel lblVendorCode;
        private Infragistics.Win.Misc.UltraLabel lblDescription;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemDescription;
        private Infragistics.Win.Misc.UltraLabel lblItemCode;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtItemCode;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor txtSellingPrice;
        public Infragistics.Win.UltraWinEditors.UltraNumericEditor txtCostPrice;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbItemCode;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}