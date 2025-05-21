using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinEditors;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmInvTransType.
	/// </summary>
	public class frmInvTransType : System.Windows.Forms.Form
	{
		public bool IsCanceled = true;
		private InvTransTypeData oInvTransTypeData = new  InvTransTypeData();
		private InvTransTypeRow oInvTransTypeRow ;
		private InvTransType oInvTransType = new InvTransType();
		private Infragistics.Win.Misc.UltraLabel ultraLabel18;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtName;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtID;
		private System.Windows.Forms.RadioButton optDebit;
		private System.Windows.Forms.RadioButton optCredit;
		private System.ComponentModel.IContainer components;

		public void Initialize()
		{
			SetNew();
		}

		public frmInvTransType()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			try
			{
				Initialize();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmInvTransType));
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			this.ultraLabel18 = new Infragistics.Win.Misc.UltraLabel();
			this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
			this.txtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
			this.txtID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
			this.btnClose = new Infragistics.Win.Misc.UltraButton();
			this.btnSave = new Infragistics.Win.Misc.UltraButton();
			this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.optCredit = new System.Windows.Forms.RadioButton();
			this.optDebit = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtID)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// ultraLabel18
			// 
			appearance1.ForeColor = System.Drawing.Color.White;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Right;
			this.ultraLabel18.Appearance = appearance1;
			this.ultraLabel18.Location = new System.Drawing.Point(8, 88);
			this.ultraLabel18.Name = "ultraLabel18";
			this.ultraLabel18.Size = new System.Drawing.Size(170, 18);
			this.ultraLabel18.TabIndex = 15;
			this.ultraLabel18.Text = "Transaction Effect";
			this.ultraLabel18.WrapText = false;
			// 
			// ultraLabel11
			// 
			appearance2.ForeColor = System.Drawing.Color.White;
			appearance2.TextHAlign = Infragistics.Win.HAlign.Right;
			this.ultraLabel11.Appearance = appearance2;
			this.ultraLabel11.Location = new System.Drawing.Point(8, 58);
			this.ultraLabel11.Name = "ultraLabel11";
			this.ultraLabel11.Size = new System.Drawing.Size(170, 18);
			this.ultraLabel11.TabIndex = 3;
			this.ultraLabel11.Text = "Description";
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.txtName.Location = new System.Drawing.Point(188, 52);
			this.txtName.MaxLength = 50;
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(372, 23);
			this.txtName.SupportThemes = false;
			this.txtName.TabIndex = 4;
			this.txtName.Validated += new System.EventHandler(this.txtBoxs_Validate);
			this.txtName.ValueChanged += new System.EventHandler(this.txtName_ValueChanged);
			// 
			// ultraLabel14
			// 
			appearance3.ForeColor = System.Drawing.Color.White;
			appearance3.TextHAlign = Infragistics.Win.HAlign.Right;
			this.ultraLabel14.Appearance = appearance3;
			this.ultraLabel14.Location = new System.Drawing.Point(8, 26);
			this.ultraLabel14.Name = "ultraLabel14";
			this.ultraLabel14.Size = new System.Drawing.Size(170, 18);
			this.ultraLabel14.TabIndex = 1;
			this.ultraLabel14.Text = "Transaction Type Code";
			// 
			// txtID
			// 
			this.txtID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.txtID.Enabled = false;
			this.txtID.Location = new System.Drawing.Point(188, 24);
			this.txtID.MaxLength = 20;
			this.txtID.Name = "txtID";
			this.txtID.Size = new System.Drawing.Size(98, 23);
			this.txtID.SupportThemes = false;
			this.txtID.TabIndex = 2;
			this.toolTip1.SetToolTip(this.txtID, "Press F4 To Search");
			this.txtID.Validated += new System.EventHandler(this.txtBoxs_Validate);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			appearance4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance4.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance4.FontData.BoldAsString = "True";
			appearance4.ForeColor = System.Drawing.Color.White;
			appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
			this.btnClose.Appearance = appearance4;
			this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(488, 20);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(85, 26);
			this.btnClose.SupportThemes = false;
			this.btnClose.TabIndex = 18;
			this.btnClose.Text = "&Cancel";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			appearance5.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(52)), ((System.Byte)(62)), ((System.Byte)(176)));
			appearance5.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(65)), ((System.Byte)(129)), ((System.Byte)(247)));
			appearance5.FontData.BoldAsString = "True";
			appearance5.ForeColor = System.Drawing.Color.White;
			appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
			this.btnSave.Appearance = appearance5;
			this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
			this.btnSave.Location = new System.Drawing.Point(396, 20);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(85, 26);
			this.btnSave.SupportThemes = false;
			this.btnSave.TabIndex = 17;
			this.btnSave.Text = "&Ok";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lblTransactionType
			// 
			this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			appearance6.ForeColor = System.Drawing.Color.White;
			appearance6.ForeColorDisabled = System.Drawing.Color.White;
			appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
			this.lblTransactionType.Appearance = appearance6;
			this.lblTransactionType.BackColor = System.Drawing.Color.Transparent;
			this.lblTransactionType.Font = new System.Drawing.Font("Arial", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblTransactionType.Location = new System.Drawing.Point(10, 10);
			this.lblTransactionType.Name = "lblTransactionType";
			this.lblTransactionType.Size = new System.Drawing.Size(580, 40);
			this.lblTransactionType.TabIndex = 23;
			this.lblTransactionType.Tag = "Header";
			this.lblTransactionType.Text = "Inventory Transaction Type";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.optCredit);
			this.groupBox1.Controls.Add(this.optDebit);
			this.groupBox1.Controls.Add(this.ultraLabel11);
			this.groupBox1.Controls.Add(this.ultraLabel18);
			this.groupBox1.Controls.Add(this.ultraLabel14);
			this.groupBox1.Controls.Add(this.txtID);
			this.groupBox1.Controls.Add(this.txtName);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(10, 52);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(580, 130);
			this.groupBox1.TabIndex = 24;
			this.groupBox1.TabStop = false;
			// 
			// optCredit
			// 
			this.optCredit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.optCredit.ForeColor = System.Drawing.Color.White;
			this.optCredit.Location = new System.Drawing.Point(336, 88);
			this.optCredit.Name = "optCredit";
			this.optCredit.Size = new System.Drawing.Size(194, 18);
			this.optCredit.TabIndex = 17;
			this.optCredit.Text = "Remove From Inventory";
			// 
			// optDebit
			// 
			this.optDebit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.optDebit.ForeColor = System.Drawing.Color.White;
			this.optDebit.Location = new System.Drawing.Point(188, 88);
			this.optDebit.Name = "optDebit";
			this.optDebit.Size = new System.Drawing.Size(152, 18);
			this.optDebit.TabIndex = 16;
			this.optDebit.Text = "Add To Inventory";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnClose);
			this.groupBox2.Controls.Add(this.btnSave);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(10, 186);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(580, 59);
			this.groupBox2.TabIndex = 25;
			this.groupBox2.TabStop = false;
			// 
			// frmInvTransType
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(32)), ((System.Byte)(108)), ((System.Byte)(172)));
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(604, 263);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblTransactionType);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "frmInvTransType";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Inventory Transaction Type";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmInvTransType_KeyDown);
			this.Load += new System.EventHandler(this.frmInvTransType_Load);
			this.Activated += new System.EventHandler(this.frmInvTransType_Activated);
			((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtID)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		private bool Save()
		{
			try
			{
				if (this.txtName.Text.Trim()=="")
				{
					this.txtName.Focus();
					throw(new Exception("Please type Trans Type Name."));
				}
				
				oInvTransTypeRow.TypeName=this.txtName.Text.Trim();
				oInvTransTypeRow.UserID= POS_Core.Resources.Configuration.UserName;
				if (optCredit.Checked==true)
				{
					oInvTransTypeRow.TransType=1;
				}
				else
				{
					oInvTransTypeRow.TransType=0;
				}
			
				oInvTransType.Persist(oInvTransTypeData);
				return true;
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
				return false;
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if (Save())
			{
				IsCanceled = false;
				this.Close();
			}
		}

		private void txtBoxs_Validate(object sender, System.EventArgs e)
		{
			try
			{
				if (oInvTransTypeRow == null) 
					return ;
				Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
				switch(txtEditor.Name)
				{
					case "txtName":
						oInvTransTypeRow.TypeName= txtName.Text;
						break;
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}

		}

		private void SearchTaxCode()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.InvTransType_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.InvTransType_tbl;    //20-Dec-2017 JY Added
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
					
					Display();
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void Display()
		{
			txtID.Text = oInvTransTypeRow.ID.ToString();
			txtName.Text = oInvTransTypeRow.TypeName;
			if (oInvTransTypeRow.TransType==1)
			{
				this.optCredit.Checked=true;
			}
			else
			{
				this.optDebit.Checked=true;
			}
		}

		public void Edit(Int32 iID)
		{
			try
			{
				oInvTransTypeData = oInvTransType.Populate(iID);
				oInvTransTypeRow = oInvTransTypeData.InvTransType.GetRowByID(iID);
				this.Text="Edit Inventory Transaction Type";
				this.lblTransactionType.Text=this.Text;
				if (oInvTransTypeRow!= null ) 
					Display();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void SetNew()
		{
			oInvTransType = new  InvTransType();
			oInvTransTypeData = new  InvTransTypeData();
			this.Text="Add to Inventory Transaction Type";
			this.lblTransactionType.Text=this.Text;
			Clear();
			oInvTransTypeRow = oInvTransTypeData.InvTransType.AddRow(0,"",0,"");
		}

		private void Clear()
		{
			txtName.Text = "";
			txtID.Text = "";
			optDebit.Checked=true;
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			try
			{
				txtID.Text = "";
				SetNew();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			IsCanceled = true;
			this.Close();
		}

		private void frmInvTransType_Load(object sender, System.EventArgs e)
		{
			this.txtID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);			
			this.txtID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);			
			this.txtName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			IsCanceled = true;
			clsUIHelper.setColorSchecme(this);

		}
		private void frmInvTransType_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		
			try
			{
				if(e.KeyData==System.Windows.Forms.Keys.Enter)
				{
					this.SelectNextControl(this.ActiveControl,true,true,true,true);
				}
				
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}	
		}

		private void frmInvTransType_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

		private void txtName_ValueChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
