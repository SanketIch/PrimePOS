//using POS_Core.DataAccess;
using POS_Core.Resources;

namespace POS_Core_UI
{
    partial class frmCashCount
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("R", 0);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCashCount));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.ultbtn7 = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn8 = new Infragistics.Win.Misc.UltraButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ulttxtCurname = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultxtCurrencyNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultbtnBK = new Infragistics.Win.Misc.UltraButton();
            this.ultbtnEnt = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn0 = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn3 = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn1 = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn2 = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn6 = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn4 = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn5 = new Infragistics.Win.Misc.UltraButton();
            this.ultbtn9 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton13 = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ultraButton20 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton18 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton16 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton25 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton23 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton21 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton19 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton17 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton15 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton14 = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grdCashDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.utxtTotalAmount = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultbtnclear = new Infragistics.Win.Misc.UltraButton();
            this.ultbtnok = new Infragistics.Win.Misc.UltraButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulttxtCurname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultxtCurrencyNo)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCashDetail)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utxtTotalAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultbtn7
            // 
            this.ultbtn7.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn7.Location = new System.Drawing.Point(9, 234);
            this.ultbtn7.Name = "ultbtn7";
            this.ultbtn7.Size = new System.Drawing.Size(65, 49);
            this.ultbtn7.TabIndex = 2;
            this.ultbtn7.Text = "7";
            this.ultbtn7.Click += new System.EventHandler(this.Counter);
            // 
            // ultbtn8
            // 
            this.ultbtn8.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn8.Location = new System.Drawing.Point(86, 234);
            this.ultbtn8.Name = "ultbtn8";
            this.ultbtn8.Size = new System.Drawing.Size(65, 49);
            this.ultbtn8.TabIndex = 3;
            this.ultbtn8.Text = "8";
            this.ultbtn8.Click += new System.EventHandler(this.Counter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ulttxtCurname);
            this.groupBox1.Controls.Add(this.ultxtCurrencyNo);
            this.groupBox1.Controls.Add(this.ultbtnBK);
            this.groupBox1.Controls.Add(this.ultbtnEnt);
            this.groupBox1.Controls.Add(this.ultbtn0);
            this.groupBox1.Controls.Add(this.ultbtn3);
            this.groupBox1.Controls.Add(this.ultbtn1);
            this.groupBox1.Controls.Add(this.ultbtn2);
            this.groupBox1.Controls.Add(this.ultbtn6);
            this.groupBox1.Controls.Add(this.ultbtn4);
            this.groupBox1.Controls.Add(this.ultbtn5);
            this.groupBox1.Controls.Add(this.ultbtn9);
            this.groupBox1.Controls.Add(this.ultbtn7);
            this.groupBox1.Controls.Add(this.ultbtn8);
            this.groupBox1.Location = new System.Drawing.Point(581, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 408);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // ulttxtCurname
            // 
            this.ulttxtCurname.Enabled = false;
            this.ulttxtCurname.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ulttxtCurname.Location = new System.Drawing.Point(8, 20);
            this.ulttxtCurname.Name = "ulttxtCurname";
            this.ulttxtCurname.Size = new System.Drawing.Size(221, 33);
            this.ulttxtCurname.TabIndex = 13;
            this.ulttxtCurname.Text = "Currency";
            // 
            // ultxtCurrencyNo
            // 
            this.ultxtCurrencyNo.Enabled = false;
            this.ultxtCurrencyNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultxtCurrencyNo.Location = new System.Drawing.Point(9, 68);
            this.ultxtCurrencyNo.Name = "ultxtCurrencyNo";
            this.ultxtCurrencyNo.Size = new System.Drawing.Size(220, 33);
            this.ultxtCurrencyNo.TabIndex = 12;
            this.ultxtCurrencyNo.Text = "Currency Count";
            // 
            // ultbtnBK
            // 
            this.ultbtnBK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtnBK.Location = new System.Drawing.Point(86, 290);
            this.ultbtnBK.Name = "ultbtnBK";
            this.ultbtnBK.Size = new System.Drawing.Size(143, 49);
            this.ultbtnBK.TabIndex = 6;
            this.ultbtnBK.Text = "Backspace";
            this.ultbtnBK.Click += new System.EventHandler(this.ultbtnBK_Click);
            // 
            // ultbtnEnt
            // 
            this.ultbtnEnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtnEnt.Location = new System.Drawing.Point(9, 346);
            this.ultbtnEnt.Name = "ultbtnEnt";
            this.ultbtnEnt.Size = new System.Drawing.Size(220, 49);
            this.ultbtnEnt.TabIndex = 7;
            this.ultbtnEnt.Text = "Enter";
            this.ultbtnEnt.Click += new System.EventHandler(this.ultbtnEnt_Click);
            // 
            // ultbtn0
            // 
            this.ultbtn0.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn0.Location = new System.Drawing.Point(9, 290);
            this.ultbtn0.Name = "ultbtn0";
            this.ultbtn0.Size = new System.Drawing.Size(65, 49);
            this.ultbtn0.TabIndex = 5;
            this.ultbtn0.Text = "0";
            this.ultbtn0.Click += new System.EventHandler(this.Counter);
            // 
            // ultbtn3
            // 
            this.ultbtn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn3.Location = new System.Drawing.Point(164, 122);
            this.ultbtn3.Name = "ultbtn3";
            this.ultbtn3.Size = new System.Drawing.Size(65, 49);
            this.ultbtn3.TabIndex = 12;
            this.ultbtn3.Text = "3";
            this.ultbtn3.Click += new System.EventHandler(this.Counter);
            // 
            // ultbtn1
            // 
            this.ultbtn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn1.Location = new System.Drawing.Point(9, 122);
            this.ultbtn1.Name = "ultbtn1";
            this.ultbtn1.Size = new System.Drawing.Size(65, 49);
            this.ultbtn1.TabIndex = 10;
            this.ultbtn1.Text = "1";
            this.ultbtn1.Click += new System.EventHandler(this.Counter);
            // 
            // ultbtn2
            // 
            this.ultbtn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn2.Location = new System.Drawing.Point(86, 122);
            this.ultbtn2.Name = "ultbtn2";
            this.ultbtn2.Size = new System.Drawing.Size(65, 49);
            this.ultbtn2.TabIndex = 11;
            this.ultbtn2.Text = "2";
            this.ultbtn2.Click += new System.EventHandler(this.Counter);
            // 
            // ultbtn6
            // 
            this.ultbtn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn6.Location = new System.Drawing.Point(164, 178);
            this.ultbtn6.Name = "ultbtn6";
            this.ultbtn6.Size = new System.Drawing.Size(65, 49);
            this.ultbtn6.TabIndex = 1;
            this.ultbtn6.Text = "6";
            this.ultbtn6.Click += new System.EventHandler(this.Counter);
            // 
            // ultbtn4
            // 
            this.ultbtn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn4.Location = new System.Drawing.Point(9, 178);
            this.ultbtn4.Name = "ultbtn4";
            this.ultbtn4.Size = new System.Drawing.Size(65, 49);
            this.ultbtn4.TabIndex = 13;
            this.ultbtn4.Text = "4";
            this.ultbtn4.Click += new System.EventHandler(this.Counter);
            // 
            // ultbtn5
            // 
            this.ultbtn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn5.Location = new System.Drawing.Point(86, 178);
            this.ultbtn5.Name = "ultbtn5";
            this.ultbtn5.Size = new System.Drawing.Size(65, 49);
            this.ultbtn5.TabIndex = 0;
            this.ultbtn5.Text = "5";
            this.ultbtn5.Click += new System.EventHandler(this.Counter);
            // 
            // ultbtn9
            // 
            this.ultbtn9.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtn9.Location = new System.Drawing.Point(164, 234);
            this.ultbtn9.Name = "ultbtn9";
            this.ultbtn9.Size = new System.Drawing.Size(65, 49);
            this.ultbtn9.TabIndex = 4;
            this.ultbtn9.Text = "9";
            this.ultbtn9.Click += new System.EventHandler(this.Counter);
            // 
            // ultraButton13
            // 
            this.ultraButton13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton13.Location = new System.Drawing.Point(14, 32);
            this.ultraButton13.Name = "ultraButton13";
            this.ultraButton13.Size = new System.Drawing.Size(90, 40);
            this.ultraButton13.TabIndex = 1;
            this.ultraButton13.Text = "Penny";
            this.ultraButton13.Click += new System.EventHandler(this.CurrencyName);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ultraButton20);
            this.groupBox2.Controls.Add(this.ultraButton18);
            this.groupBox2.Controls.Add(this.ultraButton16);
            this.groupBox2.Controls.Add(this.ultraButton13);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(4, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 143);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coins";
            // 
            // ultraButton20
            // 
            this.ultraButton20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton20.Location = new System.Drawing.Point(121, 82);
            this.ultraButton20.Name = "ultraButton20";
            this.ultraButton20.Size = new System.Drawing.Size(90, 40);
            this.ultraButton20.TabIndex = 4;
            this.ultraButton20.Text = "Quarter";
            this.ultraButton20.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton18
            // 
            this.ultraButton18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton18.Location = new System.Drawing.Point(121, 32);
            this.ultraButton18.Name = "ultraButton18";
            this.ultraButton18.Size = new System.Drawing.Size(90, 40);
            this.ultraButton18.TabIndex = 2;
            this.ultraButton18.Text = "Dime";
            this.ultraButton18.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton16
            // 
            this.ultraButton16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton16.Location = new System.Drawing.Point(14, 82);
            this.ultraButton16.Name = "ultraButton16";
            this.ultraButton16.Size = new System.Drawing.Size(90, 40);
            this.ultraButton16.TabIndex = 3;
            this.ultraButton16.Text = "Nickel";
            this.ultraButton16.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton1.Location = new System.Drawing.Point(141, 208);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(70, 40);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = Configuration.CInfo.CurrencySymbol +  " 1000 ";
            this.ultraButton1.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton25
            // 
            this.ultraButton25.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton25.Location = new System.Drawing.Point(141, 153);
            this.ultraButton25.Name = "ultraButton25";
            this.ultraButton25.Size = new System.Drawing.Size(70, 40);
            this.ultraButton25.TabIndex = 8;
			this.ultraButton25.Text = Configuration.CInfo.CurrencySymbol + " 100 ";
            this.ultraButton25.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton23
            // 
            this.ultraButton23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton23.Location = new System.Drawing.Point(141, 43);
            this.ultraButton23.Name = "ultraButton23";
            this.ultraButton23.Size = new System.Drawing.Size(70, 40);
            this.ultraButton23.TabIndex = 5;
			this.ultraButton23.Text = Configuration.CInfo.CurrencySymbol + " 20";
            this.ultraButton23.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton21
            // 
            this.ultraButton21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton21.Location = new System.Drawing.Point(141, 98);
            this.ultraButton21.Name = "ultraButton21";
            this.ultraButton21.Size = new System.Drawing.Size(70, 40);
            this.ultraButton21.TabIndex = 6;
			this.ultraButton21.Text = Configuration.CInfo.CurrencySymbol + " 50 ";
            this.ultraButton21.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton19
            // 
            this.ultraButton19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton19.Location = new System.Drawing.Point(17, 208);
            this.ultraButton19.Name = "ultraButton19";
            this.ultraButton19.Size = new System.Drawing.Size(70, 40);
            this.ultraButton19.TabIndex = 3;
			this.ultraButton19.Text = Configuration.CInfo.CurrencySymbol + " 10";
            this.ultraButton19.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton17
            // 
            this.ultraButton17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton17.Location = new System.Drawing.Point(17, 153);
            this.ultraButton17.Name = "ultraButton17";
            this.ultraButton17.Size = new System.Drawing.Size(70, 40);
            this.ultraButton17.TabIndex = 2;
			this.ultraButton17.Text = Configuration.CInfo.CurrencySymbol + " 5 ";
            this.ultraButton17.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton15
            // 
            this.ultraButton15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton15.Location = new System.Drawing.Point(17, 98);
            this.ultraButton15.Name = "ultraButton15";
            this.ultraButton15.Size = new System.Drawing.Size(70, 40);
            this.ultraButton15.TabIndex = 1;
			this.ultraButton15.Text = Configuration.CInfo.CurrencySymbol + " 2";
            this.ultraButton15.Click += new System.EventHandler(this.CurrencyName);
            // 
            // ultraButton14
            // 
            this.ultraButton14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton14.Location = new System.Drawing.Point(17, 43);
            this.ultraButton14.Name = "ultraButton14";
            this.ultraButton14.Size = new System.Drawing.Size(70, 40);
            this.ultraButton14.TabIndex = 0;
			this.ultraButton14.Text = Configuration.CInfo.CurrencySymbol + " 1 ";
            this.ultraButton14.Click += new System.EventHandler(this.CurrencyName);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.grdCashDetail);
            this.groupBox3.Location = new System.Drawing.Point(226, 36);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 408);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // grdCashDetail
            // 
            ultraGridColumn1.AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None;
            ultraGridColumn1.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            ultraGridColumn1.CellAppearance = appearance1;
            appearance2.Image = global::POS_Core_UI.Properties.Resources.close2;
            appearance2.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            ultraGridColumn1.CellButtonAppearance = appearance2;
            ultraGridColumn1.DataType = typeof(System.Drawing.Bitmap);
            ultraGridColumn1.Header.Caption = "Remove";
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(62, 0);
            ultraGridColumn1.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1});
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Center";
            ultraGridBand1.Header.Appearance = appearance3;
            ultraGridBand1.Header.Caption = "Coin/Bill\r\n";
            ultraGridBand1.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdCashDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCashDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdCashDetail.Location = new System.Drawing.Point(6, 12);
            this.grdCashDetail.Name = "grdCashDetail";
            this.grdCashDetail.Size = new System.Drawing.Size(343, 396);
            this.grdCashDetail.TabIndex = 0;
            this.grdCashDetail.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCashDetail_CellChange);
            this.grdCashDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCashDetail_ClickCellButton);
            this.grdCashDetail.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdCashDetail_AfterSelectChange);
            this.grdCashDetail.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this.grdCashDetail_DoubleClickRow);
            this.grdCashDetail.Click += new System.EventHandler(this.grdCashDetail_Click);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Tai Le", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(367, 9);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(135, 21);
            this.ultraLabel1.TabIndex = 10;
            this.ultraLabel1.Text = "Cash Count";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ultraLabel1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(818, 36);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // utxtTotalAmount
            // 
            appearance4.TextHAlignAsString = "Right";
            this.utxtTotalAmount.Appearance = appearance4;
            this.utxtTotalAmount.Enabled = false;
            this.utxtTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.utxtTotalAmount.Location = new System.Drawing.Point(469, 14);
            this.utxtTotalAmount.Name = "utxtTotalAmount";
            this.utxtTotalAmount.Size = new System.Drawing.Size(102, 24);
            this.utxtTotalAmount.TabIndex = 14;
            this.utxtTotalAmount.Text = "Total Amount";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel4.Location = new System.Drawing.Point(357, 18);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(88, 17);
            this.ultraLabel4.TabIndex = 15;
            this.ultraLabel4.Text = "Total Amount";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.ultraLabel5);
            this.ultraGroupBox1.Controls.Add(this.ultraLabel4);
            this.ultraGroupBox1.Controls.Add(this.utxtTotalAmount);
            this.ultraGroupBox1.Location = new System.Drawing.Point(4, 444);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(583, 57);
            this.ultraGroupBox1.TabIndex = 16;
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.AutoSize = true;
            this.ultraLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel5.Location = new System.Drawing.Point(452, 19);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(10, 14);
            this.ultraLabel5.TabIndex = 16;
            this.ultraLabel5.Text = Configuration.CInfo.CurrencySymbol.ToString() ;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.ultbtnclear);
            this.ultraGroupBox2.Controls.Add(this.ultbtnok);
            this.ultraGroupBox2.Location = new System.Drawing.Point(581, 444);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(235, 57);
            this.ultraGroupBox2.TabIndex = 17;
            // 
            // ultbtnclear
            // 
            this.ultbtnclear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtnclear.Location = new System.Drawing.Point(152, 9);
            this.ultbtnclear.Name = "ultbtnclear";
            this.ultbtnclear.Size = new System.Drawing.Size(77, 34);
            this.ultbtnclear.TabIndex = 1;
            this.ultbtnclear.Text = "Clear";
            this.ultbtnclear.Click += new System.EventHandler(this.ultbtnclear_Click);
            // 
            // ultbtnok
            // 
            this.ultbtnok.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultbtnok.Location = new System.Drawing.Point(26, 10);
            this.ultbtnok.Name = "ultbtnok";
            this.ultbtnok.Size = new System.Drawing.Size(77, 34);
            this.ultbtnok.TabIndex = 0;
            this.ultbtnok.Text = "OK";
            this.ultbtnok.Click += new System.EventHandler(this.ultbtnok_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.ultraButton14);
            this.groupBox7.Controls.Add(this.ultraButton1);
            this.groupBox7.Controls.Add(this.ultraButton15);
            this.groupBox7.Controls.Add(this.ultraButton25);
            this.groupBox7.Controls.Add(this.ultraButton17);
            this.groupBox7.Controls.Add(this.ultraButton23);
            this.groupBox7.Controls.Add(this.ultraButton21);
            this.groupBox7.Controls.Add(this.ultraButton19);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.Location = new System.Drawing.Point(4, 179);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(222, 265);
            this.groupBox7.TabIndex = 22;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Bills";
            // 
            // frmCashCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 502);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCashCount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cash Count";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCashCount_FormClosed);
            this.Load += new System.EventHandler(this.frmCashCount_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCashCount_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulttxtCurname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultxtCurrencyNo)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCashDetail)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utxtTotalAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultbtn7;
        private Infragistics.Win.Misc.UltraButton ultbtn8;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraButton ultbtn9;
        private Infragistics.Win.Misc.UltraButton ultbtn0;
        private Infragistics.Win.Misc.UltraButton ultbtn3;
        private Infragistics.Win.Misc.UltraButton ultbtn1;
        private Infragistics.Win.Misc.UltraButton ultbtn2;
        private Infragistics.Win.Misc.UltraButton ultbtn6;
        private Infragistics.Win.Misc.UltraButton ultbtn4;
        private Infragistics.Win.Misc.UltraButton ultbtn5;
        private Infragistics.Win.Misc.UltraButton ultbtnBK;
        private Infragistics.Win.Misc.UltraButton ultbtnEnt;
        private Infragistics.Win.Misc.UltraButton ultraButton13;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.Misc.UltraButton ultraButton15;
        private Infragistics.Win.Misc.UltraButton ultraButton16;
        private Infragistics.Win.Misc.UltraButton ultraButton14;
        private Infragistics.Win.Misc.UltraButton ultraButton25;
        private Infragistics.Win.Misc.UltraButton ultraButton23;
        private Infragistics.Win.Misc.UltraButton ultraButton21;
        private Infragistics.Win.Misc.UltraButton ultraButton19;
        private Infragistics.Win.Misc.UltraButton ultraButton20;
        private Infragistics.Win.Misc.UltraButton ultraButton17;
        private Infragistics.Win.Misc.UltraButton ultraButton18;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCashDetail;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ulttxtCurname;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraButton ultbtnok;
        private Infragistics.Win.Misc.UltraButton ultbtnclear;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor utxtTotalAmount;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultxtCurrencyNo;
        private System.Windows.Forms.GroupBox groupBox7;
    }
}