using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using System.Data;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using NLog;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmNumericPad2.
	/// </summary>
	public class frmNumericPad : System.Windows.Forms.Form
	{
		private FunctionKeys oFunctionKeys = new FunctionKeys();
		private FunctionKeysData oFunctionKeysData = new FunctionKeysData();
        private System.ComponentModel.IContainer components;

        private bool canClose = false;
		private bool mouse_is_down=false; 
		private Point mouse_pos=new Point(0,0);
		private Point mouse_pos2=new Point(0,0);

        private POSNumericKeyPad.NumericKeyPad nkp = new POSNumericKeyPad.NumericKeyPad();

        private TableLayoutPanel tableLayoutPanel10;
        private Infragistics.Win.Misc.UltraButton btn1;
        private Infragistics.Win.Misc.UltraButton btn2;
        private Infragistics.Win.Misc.UltraButton btn3;
        private Infragistics.Win.Misc.UltraButton btn4;
        private Infragistics.Win.Misc.UltraButton btn5;
        private Infragistics.Win.Misc.UltraButton btn6;
        private Infragistics.Win.Misc.UltraButton btn7;
        private Infragistics.Win.Misc.UltraButton btn8;
        private Infragistics.Win.Misc.UltraButton btn9;
        private Infragistics.Win.Misc.UltraButton btn0;
        private Infragistics.Win.Misc.UltraButton btnnokta;
        private Infragistics.Win.Misc.UltraButton btnPlusMinus;
        private Infragistics.Win.Misc.UltraButton btClear;
        private Infragistics.Win.Misc.UltraButton btnenter;
        private Infragistics.Win.Misc.UltraButton btnbackspace;
        private Infragistics.Win.Misc.UltraGroupBox gbItemPad;

        private IntPtr mhWindParent;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private string plusMinusSign = "-";
        public frmNumericPad(IntPtr hWindParent)
		{
            mhWindParent = hWindParent;
            InitializeComponent();
            //clsUIHelper.SetParentWindow(this.Handle, hWindParent);
        }
        public void AttachParent(IntPtr hWindParent)
        {
            //if (hWindParent != null)
            //    clsUIHelper.SetParentWindow(this.Handle, hWindParent);
            //else
            //    clsUIHelper.SetParentWindow(this.Handle, new IntPtr(0));
        }
        public void RemoveParent()
        {
            //clsUIHelper.SetParentWindow(this.Handle, new IntPtr(0));

        }
      
        protected override void Dispose( bool disposing )
		{
            try {
                if (disposing) {
                    if (components != null) {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
            } catch (Exception) { }
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance("1");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance("2");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance("3");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance("4");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance("5");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance("6");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance("7");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance("8");
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance("9");
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance("0");
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance(".");
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance("-");
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance("Delete");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance("Enter");
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance("Backspace");
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            this.gbItemPad = new Infragistics.Win.Misc.UltraGroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.btn1 = new Infragistics.Win.Misc.UltraButton();
            this.btn2 = new Infragistics.Win.Misc.UltraButton();
            this.btn3 = new Infragistics.Win.Misc.UltraButton();
            this.btn4 = new Infragistics.Win.Misc.UltraButton();
            this.btn5 = new Infragistics.Win.Misc.UltraButton();
            this.btn6 = new Infragistics.Win.Misc.UltraButton();
            this.btn7 = new Infragistics.Win.Misc.UltraButton();
            this.btn8 = new Infragistics.Win.Misc.UltraButton();
            this.btn9 = new Infragistics.Win.Misc.UltraButton();
            this.btn0 = new Infragistics.Win.Misc.UltraButton();
            this.btnnokta = new Infragistics.Win.Misc.UltraButton();
            this.btnPlusMinus = new Infragistics.Win.Misc.UltraButton();
            this.btClear = new Infragistics.Win.Misc.UltraButton();
            this.btnenter = new Infragistics.Win.Misc.UltraButton();
            this.btnbackspace = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.gbItemPad)).BeginInit();
            this.gbItemPad.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbItemPad
            // 
            this.gbItemPad.BackColorInternal = System.Drawing.SystemColors.InactiveCaption;
            this.gbItemPad.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
            this.gbItemPad.Controls.Add(this.tableLayoutPanel10);
            this.gbItemPad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbItemPad.Location = new System.Drawing.Point(0, 0);
            this.gbItemPad.Name = "gbItemPad";
            this.gbItemPad.Size = new System.Drawing.Size(162, 213);
            this.gbItemPad.TabIndex = 0;
            this.gbItemPad.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2000;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tableLayoutPanel10.ColumnCount = 3;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel10.Controls.Add(this.btn1, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.btn2, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.btn3, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.btn4, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.btn5, 1, 1);
            this.tableLayoutPanel10.Controls.Add(this.btn6, 2, 1);
            this.tableLayoutPanel10.Controls.Add(this.btn7, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.btn8, 1, 2);
            this.tableLayoutPanel10.Controls.Add(this.btn9, 2, 2);
            this.tableLayoutPanel10.Controls.Add(this.btn0, 0, 3);
            this.tableLayoutPanel10.Controls.Add(this.btnnokta, 1, 3);
            this.tableLayoutPanel10.Controls.Add(this.btnPlusMinus, 2, 3);
            this.tableLayoutPanel10.Controls.Add(this.btClear, 0, 4);
            this.tableLayoutPanel10.Controls.Add(this.btnenter, 1, 4);
            this.tableLayoutPanel10.Controls.Add(this.btnbackspace, 2, 4);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 5;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(162, 213);
            this.tableLayoutPanel10.TabIndex = 151;
            this.tableLayoutPanel10.Tag = "NOCOLOR";
            // 
            // btn1
            // 
            this.btn1.AcceptsFocus = false;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn1.Appearance = appearance1;
            this.btn1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn1.HotTrackAppearance = appearance2;
            this.btn1.Location = new System.Drawing.Point(3, 3);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(47, 36);
            this.btn1.TabIndex = 136;
            this.btn1.TabStop = false;
            this.btn1.Tag = "";
            this.btn1.Text = "1";
            this.btn1.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn1.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn2
            // 
            this.btn2.AcceptsFocus = false;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn2.Appearance = appearance3;
            this.btn2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn2.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn2.HotTrackAppearance = appearance4;
            this.btn2.Location = new System.Drawing.Point(56, 3);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(48, 36);
            this.btn2.TabIndex = 137;
            this.btn2.TabStop = false;
            this.btn2.Tag = "";
            this.btn2.Text = "2";
            this.btn2.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn2.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn3
            // 
            this.btn3.AcceptsFocus = false;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn3.Appearance = appearance5;
            this.btn3.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn3.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn3.HotTrackAppearance = appearance6;
            this.btn3.Location = new System.Drawing.Point(110, 3);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(49, 36);
            this.btn3.TabIndex = 138;
            this.btn3.TabStop = false;
            this.btn3.Tag = "";
            this.btn3.Text = "3";
            this.btn3.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn3.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn4
            // 
            this.btn4.AcceptsFocus = false;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn4.Appearance = appearance7;
            this.btn4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn4.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn4.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance8.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn4.HotTrackAppearance = appearance8;
            this.btn4.Location = new System.Drawing.Point(3, 45);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(47, 36);
            this.btn4.TabIndex = 139;
            this.btn4.TabStop = false;
            this.btn4.Tag = "";
            this.btn4.Text = "4";
            this.btn4.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn4.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn5
            // 
            this.btn5.AcceptsFocus = false;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn5.Appearance = appearance9;
            this.btn5.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn5.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn5.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn5.HotTrackAppearance = appearance10;
            this.btn5.Location = new System.Drawing.Point(56, 45);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(48, 36);
            this.btn5.TabIndex = 140;
            this.btn5.TabStop = false;
            this.btn5.Tag = "";
            this.btn5.Text = "5";
            this.btn5.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn5.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn6
            // 
            this.btn6.AcceptsFocus = false;
            appearance11.BackColor = System.Drawing.Color.White;
            appearance11.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn6.Appearance = appearance11;
            this.btn6.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn6.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn6.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn6.HotTrackAppearance = appearance12;
            this.btn6.Location = new System.Drawing.Point(110, 45);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(49, 36);
            this.btn6.TabIndex = 141;
            this.btn6.TabStop = false;
            this.btn6.Tag = "";
            this.btn6.Text = "6";
            this.btn6.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn6.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn6.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn7
            // 
            this.btn7.AcceptsFocus = false;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn7.Appearance = appearance13;
            this.btn7.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn7.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn7.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance14.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn7.HotTrackAppearance = appearance14;
            this.btn7.Location = new System.Drawing.Point(3, 87);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(47, 36);
            this.btn7.TabIndex = 142;
            this.btn7.TabStop = false;
            this.btn7.Tag = "";
            this.btn7.Text = "7";
            this.btn7.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn7.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn7.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn8
            // 
            this.btn8.AcceptsFocus = false;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn8.Appearance = appearance15;
            this.btn8.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn8.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn8.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn8.HotTrackAppearance = appearance16;
            this.btn8.Location = new System.Drawing.Point(56, 87);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(48, 36);
            this.btn8.TabIndex = 143;
            this.btn8.TabStop = false;
            this.btn8.Tag = "";
            this.btn8.Text = "8";
            this.btn8.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn8.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn8.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn9
            // 
            this.btn9.AcceptsFocus = false;
            appearance17.BackColor = System.Drawing.Color.White;
            appearance17.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn9.Appearance = appearance17;
            this.btn9.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn9.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn9.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn9.HotTrackAppearance = appearance18;
            this.btn9.Location = new System.Drawing.Point(110, 87);
            this.btn9.Name = "btn9";
            this.btn9.ShowFocusRect = false;
            this.btn9.Size = new System.Drawing.Size(49, 36);
            this.btn9.TabIndex = 144;
            this.btn9.TabStop = false;
            this.btn9.Tag = "";
            this.btn9.Text = "9";
            this.btn9.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn9.UseMnemonic = false;
            this.btn9.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn9.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn0
            // 
            this.btn0.AcceptsFocus = false;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btn0.Appearance = appearance19;
            this.btn0.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btn0.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btn0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn0.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn0.HotTrackAppearance = appearance20;
            this.btn0.Location = new System.Drawing.Point(3, 129);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(47, 36);
            this.btn0.TabIndex = 145;
            this.btn0.TabStop = false;
            this.btn0.Tag = "";
            this.btn0.Text = "0";
            this.btn0.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btn0.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btn0.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnnokta
            // 
            this.btnnokta.AcceptsFocus = false;
            appearance21.BackColor = System.Drawing.Color.White;
            appearance21.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btnnokta.Appearance = appearance21;
            this.btnnokta.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnnokta.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnnokta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnnokta.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance22.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnnokta.HotTrackAppearance = appearance22;
            this.btnnokta.Location = new System.Drawing.Point(56, 129);
            this.btnnokta.Name = "btnnokta";
            this.btnnokta.Size = new System.Drawing.Size(48, 36);
            this.btnnokta.TabIndex = 146;
            this.btnnokta.TabStop = false;
            this.btnnokta.Tag = "";
            this.btnnokta.Text = ".";
            this.btnnokta.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnnokta.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnnokta.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnPlusMinus
            // 
            this.btnPlusMinus.AcceptsFocus = false;
            appearance23.BackColor = System.Drawing.Color.White;
            appearance23.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btnPlusMinus.Appearance = appearance23;
            this.btnPlusMinus.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnPlusMinus.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnPlusMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlusMinus.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPlusMinus.HotTrackAppearance = appearance24;
            this.btnPlusMinus.Location = new System.Drawing.Point(110, 129);
            this.btnPlusMinus.Name = "btnPlusMinus";
            this.btnPlusMinus.Size = new System.Drawing.Size(49, 36);
            this.btnPlusMinus.TabIndex = 147;
            this.btnPlusMinus.TabStop = false;
            this.btnPlusMinus.Tag = "";
            this.btnPlusMinus.Text = "+/-";
            this.btnPlusMinus.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnPlusMinus.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnPlusMinus.Click += new System.EventHandler(this.btn_Click);
            // 
            // btClear
            // 
            this.btClear.AcceptsFocus = false;
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btClear.Appearance = appearance25;
            this.btClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btClear.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btClear.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance26.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btClear.HotTrackAppearance = appearance26;
            this.btClear.Location = new System.Drawing.Point(3, 171);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(47, 39);
            this.btClear.TabIndex = 148;
            this.btClear.TabStop = false;
            this.btClear.Tag = "";
            this.btClear.Text = "C";
            this.btClear.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btClear.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnenter
            // 
            this.btnenter.AcceptsFocus = false;
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btnenter.Appearance = appearance27;
            this.btnenter.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnenter.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnenter.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance28.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnenter.HotTrackAppearance = appearance28;
            this.btnenter.Location = new System.Drawing.Point(56, 171);
            this.btnenter.Name = "btnenter";
            this.btnenter.Size = new System.Drawing.Size(48, 39);
            this.btnenter.TabIndex = 149;
            this.btnenter.TabStop = false;
            this.btnenter.Tag = "";
            this.btnenter.Text = "Enter";
            this.btnenter.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnenter.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnenter.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnbackspace
            // 
            this.btnbackspace.AcceptsFocus = false;
            appearance29.BackColor = System.Drawing.Color.White;
            appearance29.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            appearance29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            this.btnbackspace.Appearance = appearance29;
            this.btnbackspace.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnbackspace.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnbackspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnbackspace.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnbackspace.HotTrackAppearance = appearance30;
            this.btnbackspace.Location = new System.Drawing.Point(110, 171);
            this.btnbackspace.Name = "btnbackspace";
            this.btnbackspace.Size = new System.Drawing.Size(49, 39);
            this.btnbackspace.TabIndex = 150;
            this.btnbackspace.TabStop = false;
            this.btnbackspace.Tag = "";
            this.btnbackspace.Text = "Back";
            this.btnbackspace.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.btnbackspace.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnbackspace.Click += new System.EventHandler(this.btn_Click);
            // 
            // frmNumericPad
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(162, 213);
            this.Controls.Add(this.gbItemPad);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNumericPad";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmNumericPad_Activated);
            this.Deactivate += new System.EventHandler(this.frmNumericPad_Deactivate);
            this.Load += new System.EventHandler(this.frmNumericPad_Load);
            this.Shown += new System.EventHandler(this.frmNumericPad_Shown);
            this.LocationChanged += new System.EventHandler(this.frmNumericPad_LocationChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmNumericPad_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNumericPad_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmNumericPad_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmNumericPad_KeyUp);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmNumericPad_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.gbItemPad)).EndInit();
            this.gbItemPad.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        #endregion

        private void frmNumericPad_Load(object sender, System.EventArgs e)
        {
            try {
               // logger.Trace("frmNumericPad_Load() - " + clsPOSDBConstants.Log_Entering);
                //this.Location = new Point(1065, 2);
                //this.Size = new Size(296, 482);
                ////clsUIHelper.setColorSchecme(this);
                //FormLocation frmLoc = Configuration.GetPadSetting(1);
                //this.Left = 1065; //frmLoc.Left-8;
                //this.Top = 2; //frmLoc.Top-20;
                //this.Width = frmLoc.Width;
                //this.Height = frmLoc.Height;
                //clsUIHelper.SETACTIVEWINDOW(new IntPtr(99999999));
               // logger.Trace("frmNumericPad_Load() - " + clsPOSDBConstants.Log_Exiting);
            } catch (Exception) { }
        }
        private void frmNumericPad_Activated(object sender, System.EventArgs e)
        {
            //timer1.Enabled = true;
        }

        private void frmNumericPad_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //clsUIHelper.SETACTIVEWINDOW(new IntPtr(99999999));
        }

        private void frmNumericPad_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //clsUIHelper.SETACTIVEWINDOW(new IntPtr(99999999));
        }

        private void frmNumericPad_LocationChanged(object sender, System.EventArgs e)
        {
            //clsUIHelper.SETACTIVEWINDOW(new IntPtr((long)99999999));
        }

        private void frmNumericPad_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

        }

        private void frmNumericPad_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        private void frmNumericPad_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
           // if (e.KeyData == Keys.Escape)
                //clsUIHelper.SETACTIVEWINDOW(new IntPtr(99999999));
            e.Handled = true;
        }

        private void frmNumericPad_Deactivate(object sender, EventArgs e)
        {

        }

        private void frmNumericPad_Shown(object sender, EventArgs e)
        {
           // frmMain.getInstance().Activate();

        }


        private void btn_Click(object sender, System.EventArgs e)
        {
            //clsUIHelper.SETACTIVEWINDOW(mhWindParent);
            Infragistics.Win.Misc.UltraButton btn = (Infragistics.Win.Misc.UltraButton)sender;
            //nkp.buttonClick(btn.Name);
            if (btn.Appearance.Key == plusMinusSign) {
                SendKeys.Send(plusMinusSign);
                if (plusMinusSign == "+=") {
                    plusMinusSign = "-";
                } else {
                    plusMinusSign = "+=";
                }
                btn.Appearance.Key = plusMinusSign;
            } else if (btn.Appearance.Key == "Delete") {
                SendKeys.Send("{c}");
                SendKeys.Send("^+{A}{BACKSPACE}");
                //SendKeys.Send("{" + btn.Appearance.Key + "}");
            } else {
                SendKeys.Send("{" + btn.Appearance.Key + "}");
            }

        }


        public void closeForm()
        {
            canClose = true;
            this.Close();
        }

    }
}
