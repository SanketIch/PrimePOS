namespace POS_Core_UI.UserManagement
{
    partial class frmUserGroup
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.txtGroupID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.trvPermissions = new Infragistics.Win.UltraWinTree.UltraTree();
            this.ultraGroupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDisAllowAll = new Infragistics.Win.Misc.UltraButton();
            this.btnAllowAll = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvPermissions)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ultraGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTransactionType
            // 
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance1;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(15, 5);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(379, 30);
            this.lblTransactionType.TabIndex = 45;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "User Group";
            // 
            // txtGroupName
            // 
            this.txtGroupName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGroupName.Location = new System.Drawing.Point(113, 48);
            this.txtGroupName.MaxLength = 50;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(259, 20);
            this.txtGroupName.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Group Name*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGroupID
            // 
            this.txtGroupID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGroupID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtGroupID.Location = new System.Drawing.Point(113, 22);
            this.txtGroupID.MaxLength = 10;
            this.txtGroupID.Name = "txtGroupID";
            this.txtGroupID.Size = new System.Drawing.Size(259, 20);
            this.txtGroupID.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(12, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 20);
            this.label8.TabIndex = 2;
            this.label8.Text = "Group Code*";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trvPermissions
            // 
            this.trvPermissions.AllowAutoDragScrolling = false;
            this.trvPermissions.Location = new System.Drawing.Point(15, 29);
            this.trvPermissions.Name = "trvPermissions";
            _override1.LabelEdit = Infragistics.Win.DefaultableBoolean.False;
            _override1.NodeDoubleClickAction = Infragistics.Win.UltraWinTree.NodeDoubleClickAction.ToggleExpansion;
            _override1.NodeStyle = Infragistics.Win.UltraWinTree.NodeStyle.CheckBox;
            _override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.None;
            this.trvPermissions.Override = _override1;
            this.trvPermissions.Size = new System.Drawing.Size(357, 196);
            this.trvPermissions.TabIndex = 17;
            this.trvPermissions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.trvPermissions.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);
            this.trvPermissions.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.trvPermissions_AfterSelect);
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.txtGroupName);
            this.ultraGroupBox2.Controls.Add(this.label1);
            this.ultraGroupBox2.Controls.Add(this.txtGroupID);
            this.ultraGroupBox2.Controls.Add(this.label8);
            this.ultraGroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ultraGroupBox2.ForeColor = System.Drawing.Color.White;
            this.ultraGroupBox2.Location = new System.Drawing.Point(10, 43);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(384, 81);
            this.ultraGroupBox2.TabIndex = 43;
            this.ultraGroupBox2.TabStop = false;
            this.ultraGroupBox2.Text = "User Group Information";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trvPermissions);
            this.groupBox1.Controls.Add(this.btnDisAllowAll);
            this.groupBox1.Controls.Add(this.btnAllowAll);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(10, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 233);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rights";
            // 
            // btnDisAllowAll
            // 
            this.btnDisAllowAll.Location = new System.Drawing.Point(146, 0);
            this.btnDisAllowAll.Name = "btnDisAllowAll";
            this.btnDisAllowAll.Size = new System.Drawing.Size(106, 22);
            this.btnDisAllowAll.TabIndex = 16;
            this.btnDisAllowAll.Text = "Disallow All";
            this.btnDisAllowAll.Click += new System.EventHandler(this.btnDisAllowAll_Click);
            // 
            // btnAllowAll
            // 
            this.btnAllowAll.Location = new System.Drawing.Point(50, 0);
            this.btnAllowAll.Name = "btnAllowAll";
            this.btnAllowAll.Size = new System.Drawing.Size(88, 22);
            this.btnAllowAll.TabIndex = 15;
            this.btnAllowAll.Text = "Allow All";
            this.btnAllowAll.Click += new System.EventHandler(this.btnAllowAll_Click);
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.btnCancel);
            this.ultraGroupBox3.Controls.Add(this.btnOk);
            this.ultraGroupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ultraGroupBox3.ForeColor = System.Drawing.Color.White;
            this.ultraGroupBox3.Location = new System.Drawing.Point(10, 371);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(384, 54);
            this.ultraGroupBox3.TabIndex = 46;
            this.ultraGroupBox3.TabStop = false;
            // 
            // btnCancel
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance2.FontData.BoldAsString = "True";
            this.btnCancel.Appearance = appearance2;
            this.btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(294, 19);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 26);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // btnOk
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance3.FontData.BoldAsString = "True";
            this.btnOk.Appearance = appearance3;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.btnOk.Location = new System.Drawing.Point(208, 19);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(78, 26);
            this.btnOk.TabIndex = 19;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmUserGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(407, 433);
            this.Controls.Add(this.ultraGroupBox3);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "frmUserGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Group";
            this.Load += new System.EventHandler(this.frmUserGroup_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUserGroup_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trvPermissions)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ultraGroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private System.Windows.Forms.TextBox txtGroupID;
        private System.Windows.Forms.Label label8;
        private Infragistics.Win.UltraWinTree.UltraTree trvPermissions;
        private System.Windows.Forms.GroupBox ultraGroupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraButton btnDisAllowAll;
        private Infragistics.Win.Misc.UltraButton btnAllowAll;
        private System.Windows.Forms.GroupBox ultraGroupBox3;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnOk;
    }
}