using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using POS_Core.ErrorLogging;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using System.Collections.Generic;
//using POS_Core.DataAccess;
using System.Text.RegularExpressions;
using POS_Core.DataAccess;
using Infragistics.Win;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmVendor : System.Windows.Forms.Form
	{
		public bool IsCanceled = true;
		private VendorRow oVendorRow;
        private POS_Core.BusinessRules.Vendor oBRVendor = new POS_Core.BusinessRules.Vendor();
		private VendorData oVendData = new VendorData();
        MMS.PROCESSOR.MMSDictionary<string, string> dict = null;
        DataSet datasetVendorID = new DataSet();
        private bool isEdit = false;   


		#region Declaration
		private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel24;
		private Infragistics.Win.Misc.UltraLabel ultraLabel22;
		private Infragistics.Win.Misc.UltraLabel ultraLabel23;
		private Infragistics.Win.Misc.UltraLabel ultraLabel25;
		private Infragistics.Win.Misc.UltraLabel ultraLabel26;
		private Infragistics.Win.Misc.UltraLabel ultraLabel27;
		private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		private Infragistics.Win.Misc.UltraLabel ultraLabel2;
		private Infragistics.Win.Misc.UltraLabel ultraLabel3;
		private Infragistics.Win.Misc.UltraLabel ultraLabel4;
		private Infragistics.Win.Misc.UltraLabel ultraLabel5;
		private Infragistics.Win.Misc.UltraLabel ultraLabel6;
		private Infragistics.Win.Misc.UltraLabel lblTransactionType;
		private System.Windows.Forms.CheckBox chkIsActive;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEmailAddr;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWebAddress;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAddress1;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendorName;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtState;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCity;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAddress2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit txtZipCode;
		private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit txtPhoneOff;
		private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit txtFaxNo;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVendorCode;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.Misc.UltraButton btnClose;
		private Infragistics.Win.Misc.UltraButton btnSave;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private Infragistics.Win.Misc.UltraLabel ultraLabel55;
		private Infragistics.Win.Misc.UltraLabel ultraLabel39;
		private Infragistics.Win.Misc.UltraLabel ultraLabel40;
		private Infragistics.Win.Misc.UltraLabel ultraLabel41;
		private Infragistics.Win.Misc.UltraLabel ultraLabel42;
		private Infragistics.Win.Misc.UltraLabel ultraLabel43;
		private Infragistics.Win.Misc.UltraLabel ultraLabel44;
		private Infragistics.Win.Misc.UltraLabel ultraLabel45;
		private Infragistics.Win.Misc.UltraLabel ultraLabel47;
		private Infragistics.Win.Misc.UltraLabel ultraLabel48;
		private Infragistics.Win.Misc.UltraLabel ultraLabel49;
		private Infragistics.Win.Misc.UltraLabel ultraLabel50;
		private Infragistics.Win.Misc.UltraLabel ultraLabel51;
		private Infragistics.Win.Misc.UltraLabel ultraLabel52;
		private Infragistics.Win.Misc.UltraLabel ultraLabel53;
		private Infragistics.Win.Misc.UltraLabel ultraLabel54;
		private Infragistics.Win.Misc.UltraLabel ultraLabel8;
		private Infragistics.Win.Misc.UltraLabel ultraLabel56;
		private Infragistics.Win.Misc.UltraLabel ultraLabel57;
		private Infragistics.Win.Misc.UltraLabel ultraLabel58;
		private Infragistics.Win.Misc.UltraLabel ultraLabel59;
		private Infragistics.Win.Misc.UltraLabel ultraLabel9;
		private Infragistics.Win.Misc.UltraLabel ultraLabel10;
		private Infragistics.Win.Misc.UltraLabel ultraLabel11;
		private Infragistics.Win.Misc.UltraLabel ultraLabel46;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tbX12;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPERName;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPerContactFunctionCode;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPERCommNumQualifier;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAMTAmountQualifier;
		private Infragistics.Win.Misc.UltraLabel ultraLabel60;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPERCommNum;
		private Infragistics.Win.Misc.UltraLabel ultraLabel7;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtIEA_Interchange_Control_No;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFTPURL;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtApp_Sender_Code;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFTPLogin;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtISA_ID_Qualifier_Sender;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPO_Type;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtISA_Interchange_SenderID;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtID_Code_Qualifier_SE;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtApp_Receiver_Code;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtISA_ID_Qualifier_Receiver;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFTPPassword;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFTPPort;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtISA_Interchange_ReceiverID;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtID_Code_Qualifier_By;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cboStandardType;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtISA_Interchange_Control_No;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtIdentification_Code_SE;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtVersion;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtProduct_Qualifier;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtISA_Acknowledgement_Request;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tbFTP;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtOutboundDir;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtInboundDir;
		private Infragistics.Win.Misc.UltraLabel ultraLabel16;
		private Infragistics.Win.Misc.UltraLabel ultraLabel17;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSegmentSepfix;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtElementSepfix;
		private Infragistics.Win.Misc.UltraLabel ultraLabel14;
		private Infragistics.Win.Misc.UltraLabel ultraLabel15;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtPriceFileFormatfix;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAckFileFormatfix;
		private Infragistics.Win.Misc.UltraLabel ultraLabel12;
		private Infragistics.Win.Misc.UltraLabel ultraLabel13;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSubElementSep;
		private Infragistics.Win.Misc.UltraLabel ultraLabel18;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cboItemType;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cboPriceType;
		private Infragistics.Win.Misc.UltraLabel ultraLabel21;
		private Infragistics.Win.Misc.UltraLabel ultraLabel28;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cboISA_Test_Indicator;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton optUseUPCCode;
        private System.Windows.Forms.RadioButton optUseVendorItemCode;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtIdentification_Code_By;
		private Infragistics.Win.Misc.UltraLabel ultraLabel34;
		private Infragistics.Win.Misc.UltraLabel ultraLabel20;
		private Infragistics.Win.UltraWinEditors.UltraTextEditor txtISA_Interchange_Control_Version_No;
		private Infragistics.Win.Misc.UltraLabel ultraLabel29;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor cboEncryptionType;
        private Label lblPrimePoVendor;
        private CheckBox chkIsEPO;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboVendorList;
        private Infragistics.Win.Misc.UltraLabel ultraLabel30;
        private Infragistics.Win.Misc.UltraLabel lblCostQualifier;
        private CheckBox chkUpdatePrice;
        private Infragistics.Win.Misc.UltraLabel lblTimeToOrder;
        private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit maskedEditTimeToOrder;
        private CheckBox chkIsAutoClose;
        private Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit txtCellNo;
        private CheckBox chkSendVendorCostPrice;
        private CheckBox chkprocess810;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboCostQualifier;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboPriceQualifier;
        private Infragistics.Win.Misc.UltraButton btnVendorNote;
        private CheckBox chkAckPriceUpdate;
        private CheckBox chkSalePriceUpdate;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboSalePriceQualifie;
        private Infragistics.Win.Misc.UltraLabel ultraLabel19;
        private CheckBox chkReduceSPWithPriceUpd;
		private System.ComponentModel.IContainer components;
        #endregion

        private Infragistics.Win.Misc.UltraPanel pnlClose;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraPanel pnlSave;
        private Infragistics.Win.Misc.UltraLabel lblSave;
        private Infragistics.Win.Misc.UltraPanel pnlVendorNote;
        private Infragistics.Win.Misc.UltraLabel lblVendorNote;
        private bool bSave; //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 

		public frmVendor()
		{
			InitializeComponent();
			try
			{
                bSave = false;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
				Initialize();
                CmbVendorList_Load();
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg( exp.Message);
			}
		}
		public void Initialize()
		{
			SetNew();
		}

		private void SetNew()
		{
			oBRVendor = new POS_Core.BusinessRules.Vendor();
			oVendData = new VendorData();
            cboSalePriceQualifie.SelectedIndex = 0; //Manoj 3-17-2015
			Clear();
			this.chkIsActive.Checked=true;
            this.chkIsAutoClose.Checked = true;
            oVendorRow = oVendData.Vendor.AddRow(0, "", "", "", "", "", "", "", "", "", "", "", "", true, true, "", true, "", "", "", true, false, false,false,false,true,"");
		}

		public void Edit(string VendorCode)
		{
			try
			{
                cboPriceQualifier.SelectedIndex = -1;   //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                cboCostQualifier.SelectedIndex = -1;    //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                isEdit = true;
				txtVendorCode.Enabled = false;
				oVendData = oBRVendor.Populate(VendorCode.Trim());
				oVendorRow = (VendorRow) oVendData.Vendor.Rows[0];
                pnlVendorNote.Visible = UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.Notes.ID, UserPriviliges.Screens.VendorNotes.ID);//Added by Krishna on 10 October 2011
				
				if (oVendorRow!= null ) 
                {
                    chkAckPriceUpdate.CheckedChanged -= new System.EventHandler(this.chkIs_CheckedChanged);
                    chkprocess810.CheckedChanged -= new System.EventHandler(this.chkIs_CheckedChanged);
                    chkIsAutoClose.CheckedChanged -= new System.EventHandler(this.chkIs_CheckedChanged);
                    chkIsActive.CheckedChanged -= new System.EventHandler(this.chkIs_CheckedChanged);
                    chkSalePriceUpdate.CheckedChanged -= new System.EventHandler(this.chkIs_CheckedChanged);

					Display();

                    chkAckPriceUpdate.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
                    chkprocess810.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
                    chkIsAutoClose.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
                    chkIsActive.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
                    chkSalePriceUpdate.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
                }
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}

		}
		private void Clear()
		{
            isEdit = false;
			txtVendorName.Text = String.Empty;
			txtWebAddress.Text = String.Empty;
			txtZipCode.Text = String.Empty;
			txtAddress1.Text = String.Empty;
			txtAddress2.Text = String.Empty;
			txtCellNo.Text = String.Empty;
			txtCity.Text = String.Empty;
			txtEmailAddr.Text = String.Empty;
			txtFaxNo.Text = String.Empty;
			txtPhoneOff.Text = String.Empty;
			txtState.Text = String.Empty;
			chkIsActive.Checked = false;
            chkIsAutoClose.Checked = false;
            chkprocess810.Checked = false;
            //AckPriceUpdate Added by Ravindra on 20 Feb 2013
            chkAckPriceUpdate.Checked = false;
            chkSalePriceUpdate.Checked = false; //12-Nov-2014 JY added new field IsSalePriceUpdate
            chkReduceSPWithPriceUpd.Checked = true; //Sprint-21 - 2208 24-Jul-2015 JY Added

			//this.cboX12CommType.SelectedIndex=0;
            this.cboPriceQualifier.SelectedIndex = 0;
            this.cboCostQualifier.SelectedIndex = 6;

            chkIsEPO.Checked = false;
            //
            chkSendVendorCostPrice.Checked = false;

			this.txtFTPURL.Text=String.Empty;
			this.txtFTPLogin.Text=String.Empty;
			this.txtFTPPassword.Text=String.Empty;
			this.txtFTPPort.Text=String.Empty; 
			this.txtISA_ID_Qualifier_Sender.Text=String.Empty; 
			this.txtISA_ID_Qualifier_Receiver.Text=String.Empty; 
			this.txtISA_Interchange_SenderID.Text=String.Empty; 
			this.txtISA_Interchange_ReceiverID.Text=String.Empty;
			this.txtIEA_Interchange_Control_No.Text=String.Empty;
			this.cboISA_Test_Indicator.SelectedIndex=0;
			this.txtApp_Sender_Code.Text=String.Empty; 
			this.txtApp_Receiver_Code.Text=String.Empty; 
			this.txtPO_Type.Text=String.Empty; 
			this.txtID_Code_Qualifier_By.Text=String.Empty; 
			this.txtIdentification_Code_By.Text=String.Empty; 
			this.txtID_Code_Qualifier_SE.Text=String.Empty; 
			this.txtIdentification_Code_SE.Text=String.Empty;
			this.txtProduct_Qualifier.Text=String.Empty; 
			this.txtISA_Interchange_Control_No.Text=String.Empty; 
			this.txtISA_Acknowledgement_Request.Text=String.Empty;
			this.txtVersion.Text=String.Empty;

			this.txtPERCommNumQualifier.Text=String.Empty;
			this.txtPerContactFunctionCode.Text=String.Empty;
			this.txtPERName.Text=String.Empty;
			this.txtPERCommNum.Text=String.Empty;
			this.txtAMTAmountQualifier.Text=String.Empty;

			this.txtAckFileFormatfix.Text=String.Empty;
			this.txtPriceFileFormatfix.Text=String.Empty;
			this.txtElementSepfix.Text=String.Empty;
			this.txtSubElementSep.Text=String.Empty;
			this.txtSegmentSepfix.Text=String.Empty;

			this.txtInboundDir.Text=String.Empty;
			this.txtOutboundDir.Text=String.Empty;
			
			//EnableDisableFTP(cboX12CommType.SelectedIndex>0);
			if (oVendData != null) oVendData.Vendor.Rows.Clear();
		}
	
		private void Display()
		{
			txtVendorCode.Text = oVendorRow.Vendorcode.ToString();
			txtVendorName.Text = oVendorRow.Vendorname;
			txtWebAddress.Text = oVendorRow.Url;
			txtZipCode.Text = oVendorRow.Zip;
			txtAddress1.Text = oVendorRow.Address1;
			txtAddress2.Text = oVendorRow.Address2;
			txtCellNo.Text = oVendorRow.Cellno;
			txtCity.Text = oVendorRow.City;
			txtEmailAddr.Text = oVendorRow.Email;
			txtFaxNo.Text = oVendorRow.Faxno;
			txtPhoneOff.Text = oVendorRow.Telephoneno;
			txtState.Text = oVendorRow.State;
			chkIsActive.Checked=oVendorRow.IsActive;
            chkprocess810.Checked = oVendorRow.Process810;
            //Added by Prashant(SRT) Date:1-06-2009
            chkIsAutoClose.Checked = oVendorRow.IsAutoClose;
            //End of Added by Prashant(SRT) Date:1-06-2009

            chkIsEPO.Checked = oVendorRow.USEVICForEPO;

            //
            chkSendVendorCostPrice.Checked = oVendorRow.SendVendCostPrice;
            //Added by Atul Joshi on 29-10-2010
            chkprocess810.Checked = oVendorRow.Process810;
            //AckPriceUpdate added by Arvindra on 20 Feb 2013
            chkAckPriceUpdate.Checked = oVendorRow.AckPriceUpdate;
            chkSalePriceUpdate.Checked = oVendorRow.SalePriceUpdate;  //12-Nov-2014 JY added new field IsSalePriceUpdate
            chkReduceSPWithPriceUpd.Checked = oVendorRow.ReduceSellingPrice;    //Sprint-21 - 2208 24-Jul-2015 JY Added

            chkUpdatePrice.Checked = oVendorRow.UpdatePrice;
            
            //fields  added by SRT(Abhishek) Date : 01/15/2009 
            cboPriceQualifier.Text = oVendorRow.PriceQualifier; //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY un-commented
            cboCostQualifier.Text = oVendorRow.CostQualifier;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY un-commented
            //Added By Shitaljit to set vendor Cost & Price Qualifier to default value. JIRA-586
            //SetPriceQualifier();  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Commented
            //SetCostQualifier();   //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Commented
            SetSaleRiceQualifier();
            //Added by Prashant(SRT) Date:1-06-2009
             maskedEditTimeToOrder.Text = oVendorRow.TimeToOrder;
             //End of Added by Prashant(SRT) Date:1-06-2009

             if (datasetVendorID.Tables.Count > 0 && datasetVendorID.Tables[0].Rows.Count > 0)
             {
                 this.cboVendorList.Enabled = true;
                 DataRow[] row = datasetVendorID.Tables[0].Select(clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode +" ='" + oVendorRow.PrimePOVendorCode.ToString() + "'");
                 if (row.Length > 0)
                 {
                     bool isExist = this.cboVendorList.IsItemInList(row[0][clsPOSDBConstants.Vendor_Fld_VendorName].ToString());
                     this.cboVendorList.Value = Convert.ToString(row[0][clsPOSDBConstants.Vendor_Fld_VendorName].ToString());
                 }
             }
             else
             {
                 VendorData ovd = new VendorData();
                 VendorSvr oVsvr = new VendorSvr();
                 ovd = oVsvr.Populate(Configuration.convertNullToInt( oVendorRow.PrimePOVendorCode));
                 if (ovd != null && ovd.Tables[0].Rows.Count > 0)
                 {

                 }
                 else
                 {
                     this.cboVendorList.Value = "None";
                 }
                this.cboVendorList.Enabled = false;
             } 
      
           if(oVendorRow.USEVICForEPO==true)
			{
			   this.optUseVendorItemCode.Checked=true;
			   this.optUseUPCCode.Checked=false;
            }
			else
			{
				this.optUseVendorItemCode.Checked=false;
				this.optUseUPCCode.Checked=true;
			}
           if (bSave)
           {
               bSave = false;
               Resources.Message.Display("The default qualifiers has been set, please save the record to preserve the values into database.", "Vendor", MessageBoxButtons.OK,MessageBoxIcon.Information);
           }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVendor));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
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
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem13 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem14 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem15 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem16 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem17 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem18 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem19 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem20 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem21 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem22 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem23 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem24 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem25 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem26 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem27 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem28 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem29 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem30 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem31 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem32 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem33 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem49 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem61 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem50 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem51 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem52 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem53 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem59 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem54 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem55 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem60 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem56 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem80 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem57 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem62 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem79 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem58 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem63 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem81 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem82 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem34 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem35 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem36 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem37 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem38 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem39 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem40 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem41 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem42 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem43 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem44 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem45 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem46 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem47 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem48 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem64 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem76 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem65 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem66 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem67 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem68 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem74 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem69 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem70 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem75 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem71 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem84 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem72 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem77 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem83 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem73 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem78 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Vendor Code");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Vendor Name");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Address 1");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Address 2");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("City");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("State");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Zip");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Telephone No.");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Fax No.");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn10 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Cell No.");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn11 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("URL");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn12 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Email");
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            this.tbFTP = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.txtISA_Interchange_Control_Version_No = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel20 = new Infragistics.Win.Misc.UltraLabel();
            this.cboISA_Test_Indicator = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.txtIEA_Interchange_Control_No = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtFTPURL = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtFTPLogin = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtISA_ID_Qualifier_Sender = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtPO_Type = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtISA_Interchange_SenderID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtID_Code_Qualifier_SE = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel39 = new Infragistics.Win.Misc.UltraLabel();
            this.txtApp_Receiver_Code = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtISA_ID_Qualifier_Receiver = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtFTPPassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel40 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel41 = new Infragistics.Win.Misc.UltraLabel();
            this.txtFTPPort = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel42 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel43 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel44 = new Infragistics.Win.Misc.UltraLabel();
            this.txtISA_Interchange_ReceiverID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtID_Code_Qualifier_By = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel45 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel47 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel48 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel49 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel50 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel51 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel52 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel53 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel54 = new Infragistics.Win.Misc.UltraLabel();
            this.cboStandardType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtApp_Sender_Code = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.txtIdentification_Code_By = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel34 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPERCommNum = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel60 = new Infragistics.Win.Misc.UltraLabel();
            this.txtISA_Interchange_Control_No = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtIdentification_Code_SE = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.txtVersion = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.txtProduct_Qualifier = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel46 = new Infragistics.Win.Misc.UltraLabel();
            this.txtAMTAmountQualifier = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel59 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPERCommNumQualifier = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel58 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPerContactFunctionCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel57 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPERName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel56 = new Infragistics.Win.Misc.UltraLabel();
            this.txtISA_Acknowledgement_Request = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel55 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraLabel29 = new Infragistics.Win.Misc.UltraLabel();
            this.cboEncryptionType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.optUseVendorItemCode = new System.Windows.Forms.RadioButton();
            this.optUseUPCCode = new System.Windows.Forms.RadioButton();
            this.ultraLabel28 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel21 = new Infragistics.Win.Misc.UltraLabel();
            this.cboItemType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboPriceType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtSubElementSep = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel18 = new Infragistics.Win.Misc.UltraLabel();
            this.txtOutboundDir = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtInboundDir = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel16 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel17 = new Infragistics.Win.Misc.UltraLabel();
            this.txtSegmentSepfix = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtElementSepfix = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel14 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel15 = new Infragistics.Win.Misc.UltraLabel();
            this.txtPriceFileFormatfix = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtAckFileFormatfix = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel12 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel13 = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkReduceSPWithPriceUpd = new System.Windows.Forms.CheckBox();
            this.cboSalePriceQualifie = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel19 = new Infragistics.Win.Misc.UltraLabel();
            this.chkSalePriceUpdate = new System.Windows.Forms.CheckBox();
            this.chkAckPriceUpdate = new System.Windows.Forms.CheckBox();
            this.cboCostQualifier = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cboPriceQualifier = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chkprocess810 = new System.Windows.Forms.CheckBox();
            this.chkSendVendorCostPrice = new System.Windows.Forms.CheckBox();
            this.txtCellNo = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.chkIsAutoClose = new System.Windows.Forms.CheckBox();
            this.maskedEditTimeToOrder = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.lblTimeToOrder = new Infragistics.Win.Misc.UltraLabel();
            this.chkUpdatePrice = new System.Windows.Forms.CheckBox();
            this.lblCostQualifier = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel30 = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsEPO = new System.Windows.Forms.CheckBox();
            this.cboVendorList = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblPrimePoVendor = new System.Windows.Forms.Label();
            this.txtState = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtEmailAddr = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtVendorCode = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtFaxNo = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.txtPhoneOff = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.txtZipCode = new Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel25 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtVendorName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel27 = new Infragistics.Win.Misc.UltraLabel();
            this.txtAddress1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel24 = new Infragistics.Win.Misc.UltraLabel();
            this.txtAddress2 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel22 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.txtWebAddress = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.txtCity = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel23 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel26 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlVendorNote = new Infragistics.Win.Misc.UltraPanel();
            this.btnVendorNote = new Infragistics.Win.Misc.UltraButton();
            this.lblVendorNote = new Infragistics.Win.Misc.UltraLabel();
            this.pnlSave = new Infragistics.Win.Misc.UltraPanel();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblSave = new Infragistics.Win.Misc.UltraLabel();
            this.pnlClose = new Infragistics.Win.Misc.UltraPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tbX12 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.tbFTP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Interchange_Control_Version_No)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboISA_Test_Indicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIEA_Interchange_Control_No)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPURL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_ID_Qualifier_Sender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPO_Type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Interchange_SenderID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Code_Qualifier_SE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApp_Receiver_Code)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_ID_Qualifier_Receiver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Interchange_ReceiverID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Code_Qualifier_By)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStandardType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApp_Sender_Code)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIdentification_Code_By)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPERCommNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Interchange_Control_No)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIdentification_Code_SE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProduct_Qualifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAMTAmountQualifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPERCommNumQualifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPerContactFunctionCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPERName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Acknowledgement_Request)).BeginInit();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboEncryptionType)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboItemType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPriceType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubElementSep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutboundDir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInboundDir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSegmentSepfix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementSepfix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPriceFileFormatfix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAckFileFormatfix)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSalePriceQualifie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCostQualifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPriceQualifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVendorList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAddr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWebAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlVendorNote.ClientArea.SuspendLayout();
            this.pnlVendorNote.SuspendLayout();
            this.pnlSave.ClientArea.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlClose.ClientArea.SuspendLayout();
            this.pnlClose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbX12)).BeginInit();
            this.tbX12.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbFTP
            // 
            this.tbFTP.Controls.Add(this.txtISA_Interchange_Control_Version_No);
            this.tbFTP.Controls.Add(this.ultraLabel20);
            this.tbFTP.Controls.Add(this.cboISA_Test_Indicator);
            this.tbFTP.Controls.Add(this.ultraLabel8);
            this.tbFTP.Controls.Add(this.txtIEA_Interchange_Control_No);
            this.tbFTP.Controls.Add(this.txtFTPURL);
            this.tbFTP.Controls.Add(this.txtFTPLogin);
            this.tbFTP.Controls.Add(this.txtISA_ID_Qualifier_Sender);
            this.tbFTP.Controls.Add(this.txtPO_Type);
            this.tbFTP.Controls.Add(this.txtISA_Interchange_SenderID);
            this.tbFTP.Controls.Add(this.txtID_Code_Qualifier_SE);
            this.tbFTP.Controls.Add(this.ultraLabel39);
            this.tbFTP.Controls.Add(this.txtApp_Receiver_Code);
            this.tbFTP.Controls.Add(this.txtISA_ID_Qualifier_Receiver);
            this.tbFTP.Controls.Add(this.txtFTPPassword);
            this.tbFTP.Controls.Add(this.ultraLabel40);
            this.tbFTP.Controls.Add(this.ultraLabel41);
            this.tbFTP.Controls.Add(this.txtFTPPort);
            this.tbFTP.Controls.Add(this.ultraLabel42);
            this.tbFTP.Controls.Add(this.ultraLabel43);
            this.tbFTP.Controls.Add(this.ultraLabel44);
            this.tbFTP.Controls.Add(this.txtISA_Interchange_ReceiverID);
            this.tbFTP.Controls.Add(this.txtID_Code_Qualifier_By);
            this.tbFTP.Controls.Add(this.ultraLabel45);
            this.tbFTP.Controls.Add(this.ultraLabel47);
            this.tbFTP.Controls.Add(this.ultraLabel48);
            this.tbFTP.Controls.Add(this.ultraLabel49);
            this.tbFTP.Controls.Add(this.ultraLabel50);
            this.tbFTP.Controls.Add(this.ultraLabel51);
            this.tbFTP.Controls.Add(this.ultraLabel52);
            this.tbFTP.Controls.Add(this.ultraLabel53);
            this.tbFTP.Controls.Add(this.ultraLabel54);
            this.tbFTP.Controls.Add(this.cboStandardType);
            this.tbFTP.Controls.Add(this.txtApp_Sender_Code);
            resources.ApplyResources(this.tbFTP, "tbFTP");
            this.tbFTP.Name = "tbFTP";
            // 
            // txtISA_Interchange_Control_Version_No
            // 
            this.txtISA_Interchange_Control_Version_No.AcceptsTab = true;
            appearance1.FontData.BoldAsString = resources.GetString("resource.BoldAsString");
            appearance1.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString");
            appearance1.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints")));
            appearance1.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString");
            appearance1.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString");
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtISA_Interchange_Control_Version_No.Appearance = appearance1;
            this.txtISA_Interchange_Control_Version_No.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtISA_Interchange_Control_Version_No, "txtISA_Interchange_Control_Version_No");
            this.txtISA_Interchange_Control_Version_No.MaxLength = 50;
            this.txtISA_Interchange_Control_Version_No.Name = "txtISA_Interchange_Control_Version_No";
            this.txtISA_Interchange_Control_Version_No.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtISA_Interchange_Control_Version_No.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel20
            // 
            appearance2.FontData.BoldAsString = resources.GetString("resource.BoldAsString1");
            appearance2.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString1");
            appearance2.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints1")));
            appearance2.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString1");
            appearance2.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString1");
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel20.Appearance = appearance2;
            resources.ApplyResources(this.ultraLabel20, "ultraLabel20");
            this.ultraLabel20.Name = "ultraLabel20";
            this.ultraLabel20.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboISA_Test_Indicator
            // 
            this.cboISA_Test_Indicator.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboISA_Test_Indicator.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.cboISA_Test_Indicator.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "P";
            resources.ApplyResources(valueListItem1, "valueListItem1");
            valueListItem1.ForceApplyResources = "";
            valueListItem2.DataValue = "T";
            resources.ApplyResources(valueListItem2, "valueListItem2");
            valueListItem2.ForceApplyResources = "";
            this.cboISA_Test_Indicator.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.cboISA_Test_Indicator.LimitToList = true;
            resources.ApplyResources(this.cboISA_Test_Indicator, "cboISA_Test_Indicator");
            this.cboISA_Test_Indicator.Name = "cboISA_Test_Indicator";
            // 
            // ultraLabel8
            // 
            appearance3.FontData.BoldAsString = resources.GetString("resource.BoldAsString2");
            appearance3.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString2");
            appearance3.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString2");
            appearance3.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString2");
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel8.Appearance = appearance3;
            resources.ApplyResources(this.ultraLabel8, "ultraLabel8");
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtIEA_Interchange_Control_No
            // 
            this.txtIEA_Interchange_Control_No.AcceptsTab = true;
            appearance4.FontData.BoldAsString = resources.GetString("resource.BoldAsString3");
            appearance4.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString3");
            appearance4.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints2")));
            appearance4.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString3");
            appearance4.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString3");
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtIEA_Interchange_Control_No.Appearance = appearance4;
            this.txtIEA_Interchange_Control_No.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtIEA_Interchange_Control_No, "txtIEA_Interchange_Control_No");
            this.txtIEA_Interchange_Control_No.MaxLength = 50;
            this.txtIEA_Interchange_Control_No.Name = "txtIEA_Interchange_Control_No";
            this.txtIEA_Interchange_Control_No.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtIEA_Interchange_Control_No.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtFTPURL
            // 
            appearance5.FontData.BoldAsString = resources.GetString("resource.BoldAsString4");
            appearance5.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString4");
            appearance5.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString4");
            appearance5.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString4");
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtFTPURL.Appearance = appearance5;
            this.txtFTPURL.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtFTPURL, "txtFTPURL");
            this.txtFTPURL.MaxLength = 20;
            this.txtFTPURL.Name = "txtFTPURL";
            this.toolTip1.SetToolTip(this.txtFTPURL, resources.GetString("txtFTPURL.ToolTip"));
            this.txtFTPURL.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtFTPURL.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtFTPLogin
            // 
            this.txtFTPLogin.AcceptsTab = true;
            appearance6.FontData.BoldAsString = resources.GetString("resource.BoldAsString5");
            appearance6.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString5");
            appearance6.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints3")));
            appearance6.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString5");
            appearance6.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString5");
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtFTPLogin.Appearance = appearance6;
            this.txtFTPLogin.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtFTPLogin, "txtFTPLogin");
            this.txtFTPLogin.MaxLength = 50;
            this.txtFTPLogin.Name = "txtFTPLogin";
            this.txtFTPLogin.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtFTPLogin.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtISA_ID_Qualifier_Sender
            // 
            this.txtISA_ID_Qualifier_Sender.AcceptsTab = true;
            appearance7.FontData.BoldAsString = resources.GetString("resource.BoldAsString6");
            appearance7.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString6");
            appearance7.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString6");
            appearance7.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString6");
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtISA_ID_Qualifier_Sender.Appearance = appearance7;
            this.txtISA_ID_Qualifier_Sender.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtISA_ID_Qualifier_Sender, "txtISA_ID_Qualifier_Sender");
            this.txtISA_ID_Qualifier_Sender.MaxLength = 50;
            this.txtISA_ID_Qualifier_Sender.Name = "txtISA_ID_Qualifier_Sender";
            this.txtISA_ID_Qualifier_Sender.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtISA_ID_Qualifier_Sender.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtPO_Type
            // 
            this.txtPO_Type.AcceptsTab = true;
            appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString7");
            appearance8.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString7");
            appearance8.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints4")));
            appearance8.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString7");
            appearance8.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString7");
            appearance8.ForeColor = System.Drawing.Color.Black;
            appearance8.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPO_Type.Appearance = appearance8;
            this.txtPO_Type.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtPO_Type, "txtPO_Type");
            this.txtPO_Type.MaxLength = 50;
            this.txtPO_Type.Name = "txtPO_Type";
            this.txtPO_Type.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPO_Type.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtISA_Interchange_SenderID
            // 
            this.txtISA_Interchange_SenderID.AcceptsTab = true;
            appearance9.FontData.BoldAsString = resources.GetString("resource.BoldAsString8");
            appearance9.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString8");
            appearance9.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString8");
            appearance9.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString8");
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtISA_Interchange_SenderID.Appearance = appearance9;
            this.txtISA_Interchange_SenderID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtISA_Interchange_SenderID, "txtISA_Interchange_SenderID");
            this.txtISA_Interchange_SenderID.MaxLength = 50;
            this.txtISA_Interchange_SenderID.Name = "txtISA_Interchange_SenderID";
            this.txtISA_Interchange_SenderID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtISA_Interchange_SenderID.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtID_Code_Qualifier_SE
            // 
            this.txtID_Code_Qualifier_SE.AcceptsTab = true;
            appearance10.FontData.BoldAsString = resources.GetString("resource.BoldAsString9");
            appearance10.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString9");
            appearance10.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString9");
            appearance10.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString9");
            appearance10.ForeColor = System.Drawing.Color.Black;
            appearance10.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtID_Code_Qualifier_SE.Appearance = appearance10;
            this.txtID_Code_Qualifier_SE.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtID_Code_Qualifier_SE, "txtID_Code_Qualifier_SE");
            this.txtID_Code_Qualifier_SE.MaxLength = 50;
            this.txtID_Code_Qualifier_SE.Name = "txtID_Code_Qualifier_SE";
            this.txtID_Code_Qualifier_SE.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtID_Code_Qualifier_SE.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel39
            // 
            appearance11.FontData.BoldAsString = resources.GetString("resource.BoldAsString10");
            appearance11.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString10");
            appearance11.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints5")));
            appearance11.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString10");
            appearance11.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString10");
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel39.Appearance = appearance11;
            resources.ApplyResources(this.ultraLabel39, "ultraLabel39");
            this.ultraLabel39.Name = "ultraLabel39";
            this.ultraLabel39.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtApp_Receiver_Code
            // 
            this.txtApp_Receiver_Code.AcceptsTab = true;
            appearance12.FontData.BoldAsString = resources.GetString("resource.BoldAsString11");
            appearance12.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString11");
            appearance12.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints6")));
            appearance12.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString11");
            appearance12.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString11");
            appearance12.ForeColor = System.Drawing.Color.Black;
            appearance12.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtApp_Receiver_Code.Appearance = appearance12;
            this.txtApp_Receiver_Code.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtApp_Receiver_Code, "txtApp_Receiver_Code");
            this.txtApp_Receiver_Code.MaxLength = 12;
            this.txtApp_Receiver_Code.Name = "txtApp_Receiver_Code";
            this.txtApp_Receiver_Code.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtApp_Receiver_Code.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtISA_ID_Qualifier_Receiver
            // 
            this.txtISA_ID_Qualifier_Receiver.AcceptsTab = true;
            appearance13.FontData.BoldAsString = resources.GetString("resource.BoldAsString12");
            appearance13.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString12");
            appearance13.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString12");
            appearance13.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString12");
            appearance13.ForeColor = System.Drawing.Color.Black;
            appearance13.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtISA_ID_Qualifier_Receiver.Appearance = appearance13;
            this.txtISA_ID_Qualifier_Receiver.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtISA_ID_Qualifier_Receiver, "txtISA_ID_Qualifier_Receiver");
            this.txtISA_ID_Qualifier_Receiver.MaxLength = 50;
            this.txtISA_ID_Qualifier_Receiver.Name = "txtISA_ID_Qualifier_Receiver";
            this.txtISA_ID_Qualifier_Receiver.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtISA_ID_Qualifier_Receiver.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtFTPPassword
            // 
            this.txtFTPPassword.AcceptsTab = true;
            appearance14.FontData.BoldAsString = resources.GetString("resource.BoldAsString13");
            appearance14.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString13");
            appearance14.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString13");
            appearance14.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString13");
            appearance14.ForeColor = System.Drawing.Color.Black;
            appearance14.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtFTPPassword.Appearance = appearance14;
            this.txtFTPPassword.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtFTPPassword, "txtFTPPassword");
            this.txtFTPPassword.MaxLength = 50;
            this.txtFTPPassword.Name = "txtFTPPassword";
            this.txtFTPPassword.PasswordChar = '*';
            this.txtFTPPassword.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtFTPPassword.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel40
            // 
            appearance15.FontData.BoldAsString = resources.GetString("resource.BoldAsString14");
            appearance15.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString14");
            appearance15.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints7")));
            appearance15.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString14");
            appearance15.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString14");
            appearance15.ForeColor = System.Drawing.Color.White;
            appearance15.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel40.Appearance = appearance15;
            resources.ApplyResources(this.ultraLabel40, "ultraLabel40");
            this.ultraLabel40.Name = "ultraLabel40";
            this.ultraLabel40.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel41
            // 
            appearance16.FontData.BoldAsString = resources.GetString("resource.BoldAsString15");
            appearance16.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString15");
            appearance16.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString15");
            appearance16.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString15");
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel41.Appearance = appearance16;
            resources.ApplyResources(this.ultraLabel41, "ultraLabel41");
            this.ultraLabel41.Name = "ultraLabel41";
            this.ultraLabel41.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtFTPPort
            // 
            this.txtFTPPort.AcceptsTab = true;
            appearance17.FontData.BoldAsString = resources.GetString("resource.BoldAsString16");
            appearance17.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString16");
            appearance17.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints8")));
            appearance17.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString16");
            appearance17.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString16");
            appearance17.ForeColor = System.Drawing.Color.Black;
            appearance17.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtFTPPort.Appearance = appearance17;
            this.txtFTPPort.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtFTPPort, "txtFTPPort");
            this.txtFTPPort.MaxLength = 5;
            this.txtFTPPort.Name = "txtFTPPort";
            this.toolTip1.SetToolTip(this.txtFTPPort, resources.GetString("txtFTPPort.ToolTip"));
            this.txtFTPPort.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtFTPPort.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel42
            // 
            appearance18.FontData.BoldAsString = resources.GetString("resource.BoldAsString17");
            appearance18.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString17");
            appearance18.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString17");
            appearance18.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString17");
            appearance18.ForeColor = System.Drawing.Color.White;
            appearance18.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel42.Appearance = appearance18;
            resources.ApplyResources(this.ultraLabel42, "ultraLabel42");
            this.ultraLabel42.Name = "ultraLabel42";
            this.ultraLabel42.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel43
            // 
            appearance19.FontData.BoldAsString = resources.GetString("resource.BoldAsString18");
            appearance19.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString18");
            appearance19.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints9")));
            appearance19.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString18");
            appearance19.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString18");
            appearance19.ForeColor = System.Drawing.Color.White;
            appearance19.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel43.Appearance = appearance19;
            resources.ApplyResources(this.ultraLabel43, "ultraLabel43");
            this.ultraLabel43.Name = "ultraLabel43";
            this.ultraLabel43.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel44
            // 
            appearance20.FontData.BoldAsString = resources.GetString("resource.BoldAsString19");
            appearance20.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString19");
            appearance20.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString19");
            appearance20.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString19");
            appearance20.ForeColor = System.Drawing.Color.White;
            appearance20.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel44.Appearance = appearance20;
            resources.ApplyResources(this.ultraLabel44, "ultraLabel44");
            this.ultraLabel44.Name = "ultraLabel44";
            this.ultraLabel44.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtISA_Interchange_ReceiverID
            // 
            this.txtISA_Interchange_ReceiverID.AcceptsTab = true;
            appearance21.FontData.BoldAsString = resources.GetString("resource.BoldAsString20");
            appearance21.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString20");
            appearance21.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints10")));
            appearance21.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString20");
            appearance21.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString20");
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtISA_Interchange_ReceiverID.Appearance = appearance21;
            this.txtISA_Interchange_ReceiverID.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtISA_Interchange_ReceiverID, "txtISA_Interchange_ReceiverID");
            this.txtISA_Interchange_ReceiverID.MaxLength = 50;
            this.txtISA_Interchange_ReceiverID.Name = "txtISA_Interchange_ReceiverID";
            this.txtISA_Interchange_ReceiverID.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtISA_Interchange_ReceiverID.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtID_Code_Qualifier_By
            // 
            this.txtID_Code_Qualifier_By.AcceptsTab = true;
            appearance22.FontData.BoldAsString = resources.GetString("resource.BoldAsString21");
            appearance22.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString21");
            appearance22.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString21");
            appearance22.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString21");
            appearance22.ForeColor = System.Drawing.Color.Black;
            appearance22.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtID_Code_Qualifier_By.Appearance = appearance22;
            this.txtID_Code_Qualifier_By.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtID_Code_Qualifier_By, "txtID_Code_Qualifier_By");
            this.txtID_Code_Qualifier_By.MaxLength = 50;
            this.txtID_Code_Qualifier_By.Name = "txtID_Code_Qualifier_By";
            this.txtID_Code_Qualifier_By.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtID_Code_Qualifier_By.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel45
            // 
            appearance23.FontData.BoldAsString = resources.GetString("resource.BoldAsString22");
            appearance23.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString22");
            appearance23.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints11")));
            appearance23.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString22");
            appearance23.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString22");
            appearance23.ForeColor = System.Drawing.Color.White;
            appearance23.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel45.Appearance = appearance23;
            resources.ApplyResources(this.ultraLabel45, "ultraLabel45");
            this.ultraLabel45.Name = "ultraLabel45";
            this.ultraLabel45.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel47
            // 
            appearance24.FontData.BoldAsString = resources.GetString("resource.BoldAsString23");
            appearance24.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString23");
            appearance24.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints12")));
            appearance24.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString23");
            appearance24.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString23");
            appearance24.ForeColor = System.Drawing.Color.White;
            appearance24.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel47.Appearance = appearance24;
            resources.ApplyResources(this.ultraLabel47, "ultraLabel47");
            this.ultraLabel47.Name = "ultraLabel47";
            this.ultraLabel47.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel48
            // 
            appearance25.FontData.BoldAsString = resources.GetString("resource.BoldAsString24");
            appearance25.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString24");
            appearance25.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints13")));
            appearance25.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString24");
            appearance25.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString24");
            appearance25.ForeColor = System.Drawing.Color.White;
            appearance25.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel48.Appearance = appearance25;
            resources.ApplyResources(this.ultraLabel48, "ultraLabel48");
            this.ultraLabel48.Name = "ultraLabel48";
            this.ultraLabel48.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel49
            // 
            appearance26.FontData.BoldAsString = resources.GetString("resource.BoldAsString25");
            appearance26.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString25");
            appearance26.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString25");
            appearance26.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString25");
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel49.Appearance = appearance26;
            resources.ApplyResources(this.ultraLabel49, "ultraLabel49");
            this.ultraLabel49.Name = "ultraLabel49";
            this.ultraLabel49.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel50
            // 
            appearance27.FontData.BoldAsString = resources.GetString("resource.BoldAsString26");
            appearance27.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString26");
            appearance27.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString26");
            appearance27.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString26");
            appearance27.ForeColor = System.Drawing.Color.White;
            appearance27.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel50.Appearance = appearance27;
            resources.ApplyResources(this.ultraLabel50, "ultraLabel50");
            this.ultraLabel50.Name = "ultraLabel50";
            this.ultraLabel50.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel51
            // 
            appearance28.FontData.BoldAsString = resources.GetString("resource.BoldAsString27");
            appearance28.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString27");
            appearance28.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints14")));
            appearance28.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString27");
            appearance28.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString27");
            appearance28.ForeColor = System.Drawing.Color.White;
            appearance28.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel51.Appearance = appearance28;
            resources.ApplyResources(this.ultraLabel51, "ultraLabel51");
            this.ultraLabel51.Name = "ultraLabel51";
            this.ultraLabel51.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel52
            // 
            appearance29.FontData.BoldAsString = resources.GetString("resource.BoldAsString28");
            appearance29.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString28");
            appearance29.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints15")));
            appearance29.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString28");
            appearance29.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString28");
            appearance29.ForeColor = System.Drawing.Color.White;
            appearance29.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel52.Appearance = appearance29;
            resources.ApplyResources(this.ultraLabel52, "ultraLabel52");
            this.ultraLabel52.Name = "ultraLabel52";
            this.ultraLabel52.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel53
            // 
            appearance30.FontData.BoldAsString = resources.GetString("resource.BoldAsString29");
            appearance30.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString29");
            appearance30.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString29");
            appearance30.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString29");
            appearance30.ForeColor = System.Drawing.Color.White;
            appearance30.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel53.Appearance = appearance30;
            resources.ApplyResources(this.ultraLabel53, "ultraLabel53");
            this.ultraLabel53.Name = "ultraLabel53";
            this.ultraLabel53.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel54
            // 
            appearance31.FontData.BoldAsString = resources.GetString("resource.BoldAsString30");
            appearance31.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString30");
            appearance31.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString30");
            appearance31.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString30");
            appearance31.ForeColor = System.Drawing.Color.White;
            appearance31.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel54.Appearance = appearance31;
            resources.ApplyResources(this.ultraLabel54, "ultraLabel54");
            this.ultraLabel54.Name = "ultraLabel54";
            this.ultraLabel54.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboStandardType
            // 
            this.cboStandardType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboStandardType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.cboStandardType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem3.DataValue = "0";
            valueListItem4.DataValue = "1";
            valueListItem5.DataValue = "2";
            this.cboStandardType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4,
            valueListItem5});
            this.cboStandardType.LimitToList = true;
            resources.ApplyResources(this.cboStandardType, "cboStandardType");
            this.cboStandardType.Name = "cboStandardType";
            // 
            // txtApp_Sender_Code
            // 
            this.txtApp_Sender_Code.AcceptsTab = true;
            appearance32.FontData.BoldAsString = resources.GetString("resource.BoldAsString31");
            appearance32.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString31");
            appearance32.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString31");
            appearance32.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString31");
            appearance32.ForeColor = System.Drawing.Color.Black;
            appearance32.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtApp_Sender_Code.Appearance = appearance32;
            this.txtApp_Sender_Code.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtApp_Sender_Code, "txtApp_Sender_Code");
            this.txtApp_Sender_Code.MaxLength = 12;
            this.txtApp_Sender_Code.Name = "txtApp_Sender_Code";
            this.txtApp_Sender_Code.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtApp_Sender_Code.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.txtIdentification_Code_By);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel34);
            this.ultraTabPageControl2.Controls.Add(this.txtPERCommNum);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel60);
            this.ultraTabPageControl2.Controls.Add(this.txtISA_Interchange_Control_No);
            this.ultraTabPageControl2.Controls.Add(this.txtIdentification_Code_SE);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel9);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel10);
            this.ultraTabPageControl2.Controls.Add(this.txtVersion);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel11);
            this.ultraTabPageControl2.Controls.Add(this.txtProduct_Qualifier);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel46);
            this.ultraTabPageControl2.Controls.Add(this.txtAMTAmountQualifier);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel59);
            this.ultraTabPageControl2.Controls.Add(this.txtPERCommNumQualifier);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel58);
            this.ultraTabPageControl2.Controls.Add(this.txtPerContactFunctionCode);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel57);
            this.ultraTabPageControl2.Controls.Add(this.txtPERName);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel56);
            this.ultraTabPageControl2.Controls.Add(this.txtISA_Acknowledgement_Request);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel55);
            resources.ApplyResources(this.ultraTabPageControl2, "ultraTabPageControl2");
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            // 
            // txtIdentification_Code_By
            // 
            this.txtIdentification_Code_By.AcceptsTab = true;
            appearance33.FontData.BoldAsString = resources.GetString("resource.BoldAsString32");
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtIdentification_Code_By.Appearance = appearance33;
            this.txtIdentification_Code_By.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtIdentification_Code_By, "txtIdentification_Code_By");
            this.txtIdentification_Code_By.MaxLength = 50;
            this.txtIdentification_Code_By.Name = "txtIdentification_Code_By";
            this.txtIdentification_Code_By.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtIdentification_Code_By.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel34
            // 
            appearance34.FontData.BoldAsString = resources.GetString("resource.BoldAsString33");
            appearance34.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString32");
            appearance34.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints16")));
            appearance34.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString32");
            appearance34.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString32");
            appearance34.ForeColor = System.Drawing.Color.White;
            appearance34.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel34.Appearance = appearance34;
            resources.ApplyResources(this.ultraLabel34, "ultraLabel34");
            this.ultraLabel34.Name = "ultraLabel34";
            this.ultraLabel34.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtPERCommNum
            // 
            this.txtPERCommNum.AcceptsTab = true;
            appearance35.FontData.BoldAsString = resources.GetString("resource.BoldAsString34");
            appearance35.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString33");
            appearance35.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString33");
            appearance35.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString33");
            appearance35.ForeColor = System.Drawing.Color.Black;
            appearance35.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPERCommNum.Appearance = appearance35;
            this.txtPERCommNum.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtPERCommNum, "txtPERCommNum");
            this.txtPERCommNum.MaxLength = 50;
            this.txtPERCommNum.Name = "txtPERCommNum";
            this.txtPERCommNum.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPERCommNum.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel60
            // 
            appearance36.FontData.BoldAsString = resources.GetString("resource.BoldAsString35");
            appearance36.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString34");
            appearance36.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString34");
            appearance36.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString34");
            appearance36.ForeColor = System.Drawing.Color.White;
            appearance36.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel60.Appearance = appearance36;
            resources.ApplyResources(this.ultraLabel60, "ultraLabel60");
            this.ultraLabel60.Name = "ultraLabel60";
            this.ultraLabel60.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtISA_Interchange_Control_No
            // 
            this.txtISA_Interchange_Control_No.AcceptsTab = true;
            appearance37.FontData.BoldAsString = resources.GetString("resource.BoldAsString36");
            appearance37.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString35");
            appearance37.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString35");
            appearance37.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString35");
            appearance37.ForeColor = System.Drawing.Color.Black;
            appearance37.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtISA_Interchange_Control_No.Appearance = appearance37;
            this.txtISA_Interchange_Control_No.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtISA_Interchange_Control_No, "txtISA_Interchange_Control_No");
            this.txtISA_Interchange_Control_No.MaxLength = 50;
            this.txtISA_Interchange_Control_No.Name = "txtISA_Interchange_Control_No";
            this.txtISA_Interchange_Control_No.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtISA_Interchange_Control_No.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtIdentification_Code_SE
            // 
            this.txtIdentification_Code_SE.AcceptsTab = true;
            appearance38.FontData.BoldAsString = resources.GetString("resource.BoldAsString37");
            appearance38.ForeColor = System.Drawing.Color.Black;
            appearance38.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtIdentification_Code_SE.Appearance = appearance38;
            this.txtIdentification_Code_SE.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtIdentification_Code_SE, "txtIdentification_Code_SE");
            this.txtIdentification_Code_SE.MaxLength = 50;
            this.txtIdentification_Code_SE.Name = "txtIdentification_Code_SE";
            this.txtIdentification_Code_SE.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtIdentification_Code_SE.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel9
            // 
            appearance39.FontData.BoldAsString = resources.GetString("resource.BoldAsString38");
            appearance39.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString36");
            appearance39.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints17")));
            appearance39.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString36");
            appearance39.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString36");
            appearance39.ForeColor = System.Drawing.Color.White;
            appearance39.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel9.Appearance = appearance39;
            resources.ApplyResources(this.ultraLabel9, "ultraLabel9");
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel10
            // 
            appearance40.FontData.BoldAsString = resources.GetString("resource.BoldAsString39");
            appearance40.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString37");
            appearance40.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString37");
            appearance40.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString37");
            appearance40.ForeColor = System.Drawing.Color.White;
            appearance40.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel10.Appearance = appearance40;
            resources.ApplyResources(this.ultraLabel10, "ultraLabel10");
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtVersion
            // 
            this.txtVersion.AcceptsTab = true;
            appearance41.FontData.BoldAsString = resources.GetString("resource.BoldAsString40");
            appearance41.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString38");
            appearance41.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints18")));
            appearance41.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString38");
            appearance41.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString38");
            appearance41.ForeColor = System.Drawing.Color.Black;
            appearance41.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtVersion.Appearance = appearance41;
            this.txtVersion.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtVersion, "txtVersion");
            this.txtVersion.MaxLength = 50;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtVersion.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel11
            // 
            appearance42.FontData.BoldAsString = resources.GetString("resource.BoldAsString41");
            appearance42.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString39");
            appearance42.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString39");
            appearance42.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString39");
            appearance42.ForeColor = System.Drawing.Color.White;
            appearance42.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel11.Appearance = appearance42;
            resources.ApplyResources(this.ultraLabel11, "ultraLabel11");
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtProduct_Qualifier
            // 
            this.txtProduct_Qualifier.AcceptsTab = true;
            appearance43.FontData.BoldAsString = resources.GetString("resource.BoldAsString42");
            appearance43.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString40");
            appearance43.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString40");
            appearance43.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString40");
            appearance43.ForeColor = System.Drawing.Color.Black;
            appearance43.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtProduct_Qualifier.Appearance = appearance43;
            this.txtProduct_Qualifier.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtProduct_Qualifier, "txtProduct_Qualifier");
            this.txtProduct_Qualifier.MaxLength = 50;
            this.txtProduct_Qualifier.Name = "txtProduct_Qualifier";
            this.txtProduct_Qualifier.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtProduct_Qualifier.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel46
            // 
            appearance44.FontData.BoldAsString = resources.GetString("resource.BoldAsString43");
            appearance44.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString41");
            appearance44.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString41");
            appearance44.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString41");
            appearance44.ForeColor = System.Drawing.Color.White;
            appearance44.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel46.Appearance = appearance44;
            resources.ApplyResources(this.ultraLabel46, "ultraLabel46");
            this.ultraLabel46.Name = "ultraLabel46";
            this.ultraLabel46.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtAMTAmountQualifier
            // 
            this.txtAMTAmountQualifier.AcceptsTab = true;
            appearance45.FontData.BoldAsString = resources.GetString("resource.BoldAsString44");
            appearance45.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString42");
            appearance45.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString42");
            appearance45.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString42");
            appearance45.ForeColor = System.Drawing.Color.Black;
            appearance45.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtAMTAmountQualifier.Appearance = appearance45;
            this.txtAMTAmountQualifier.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtAMTAmountQualifier, "txtAMTAmountQualifier");
            this.txtAMTAmountQualifier.MaxLength = 50;
            this.txtAMTAmountQualifier.Name = "txtAMTAmountQualifier";
            this.txtAMTAmountQualifier.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtAMTAmountQualifier.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel59
            // 
            appearance46.FontData.BoldAsString = resources.GetString("resource.BoldAsString45");
            appearance46.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString43");
            appearance46.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString43");
            appearance46.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString43");
            appearance46.ForeColor = System.Drawing.Color.White;
            appearance46.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel59.Appearance = appearance46;
            resources.ApplyResources(this.ultraLabel59, "ultraLabel59");
            this.ultraLabel59.Name = "ultraLabel59";
            this.ultraLabel59.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtPERCommNumQualifier
            // 
            this.txtPERCommNumQualifier.AcceptsTab = true;
            appearance47.FontData.BoldAsString = resources.GetString("resource.BoldAsString46");
            appearance47.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString44");
            appearance47.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString44");
            appearance47.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString44");
            appearance47.ForeColor = System.Drawing.Color.Black;
            appearance47.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPERCommNumQualifier.Appearance = appearance47;
            this.txtPERCommNumQualifier.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtPERCommNumQualifier, "txtPERCommNumQualifier");
            this.txtPERCommNumQualifier.MaxLength = 50;
            this.txtPERCommNumQualifier.Name = "txtPERCommNumQualifier";
            this.txtPERCommNumQualifier.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPERCommNumQualifier.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel58
            // 
            appearance48.FontData.BoldAsString = resources.GetString("resource.BoldAsString47");
            appearance48.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString45");
            appearance48.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString45");
            appearance48.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString45");
            appearance48.ForeColor = System.Drawing.Color.White;
            appearance48.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel58.Appearance = appearance48;
            resources.ApplyResources(this.ultraLabel58, "ultraLabel58");
            this.ultraLabel58.Name = "ultraLabel58";
            this.ultraLabel58.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtPerContactFunctionCode
            // 
            this.txtPerContactFunctionCode.AcceptsTab = true;
            appearance49.FontData.BoldAsString = resources.GetString("resource.BoldAsString48");
            appearance49.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString46");
            appearance49.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString46");
            appearance49.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString46");
            appearance49.ForeColor = System.Drawing.Color.Black;
            appearance49.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPerContactFunctionCode.Appearance = appearance49;
            this.txtPerContactFunctionCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtPerContactFunctionCode, "txtPerContactFunctionCode");
            this.txtPerContactFunctionCode.MaxLength = 50;
            this.txtPerContactFunctionCode.Name = "txtPerContactFunctionCode";
            this.txtPerContactFunctionCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPerContactFunctionCode.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel57
            // 
            appearance50.FontData.BoldAsString = resources.GetString("resource.BoldAsString49");
            appearance50.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString47");
            appearance50.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString47");
            appearance50.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString47");
            appearance50.ForeColor = System.Drawing.Color.White;
            appearance50.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel57.Appearance = appearance50;
            resources.ApplyResources(this.ultraLabel57, "ultraLabel57");
            this.ultraLabel57.Name = "ultraLabel57";
            this.ultraLabel57.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtPERName
            // 
            this.txtPERName.AcceptsTab = true;
            appearance51.FontData.BoldAsString = resources.GetString("resource.BoldAsString50");
            appearance51.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString48");
            appearance51.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString48");
            appearance51.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString48");
            appearance51.ForeColor = System.Drawing.Color.Black;
            appearance51.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPERName.Appearance = appearance51;
            this.txtPERName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtPERName, "txtPERName");
            this.txtPERName.MaxLength = 50;
            this.txtPERName.Name = "txtPERName";
            this.txtPERName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPERName.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel56
            // 
            appearance52.FontData.BoldAsString = resources.GetString("resource.BoldAsString51");
            appearance52.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString49");
            appearance52.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString49");
            appearance52.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString49");
            appearance52.ForeColor = System.Drawing.Color.White;
            appearance52.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel56.Appearance = appearance52;
            resources.ApplyResources(this.ultraLabel56, "ultraLabel56");
            this.ultraLabel56.Name = "ultraLabel56";
            this.ultraLabel56.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtISA_Acknowledgement_Request
            // 
            this.txtISA_Acknowledgement_Request.AcceptsTab = true;
            appearance53.FontData.BoldAsString = resources.GetString("resource.BoldAsString52");
            appearance53.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString50");
            appearance53.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString50");
            appearance53.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString50");
            appearance53.ForeColor = System.Drawing.Color.Black;
            appearance53.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtISA_Acknowledgement_Request.Appearance = appearance53;
            this.txtISA_Acknowledgement_Request.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtISA_Acknowledgement_Request, "txtISA_Acknowledgement_Request");
            this.txtISA_Acknowledgement_Request.MaxLength = 50;
            this.txtISA_Acknowledgement_Request.Name = "txtISA_Acknowledgement_Request";
            this.txtISA_Acknowledgement_Request.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtISA_Acknowledgement_Request.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel55
            // 
            appearance54.FontData.BoldAsString = resources.GetString("resource.BoldAsString53");
            appearance54.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString51");
            appearance54.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString51");
            appearance54.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString51");
            appearance54.ForeColor = System.Drawing.Color.White;
            appearance54.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel55.Appearance = appearance54;
            resources.ApplyResources(this.ultraLabel55, "ultraLabel55");
            this.ultraLabel55.Name = "ultraLabel55";
            this.ultraLabel55.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel29);
            this.ultraTabPageControl1.Controls.Add(this.cboEncryptionType);
            this.ultraTabPageControl1.Controls.Add(this.groupBox3);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel28);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel21);
            this.ultraTabPageControl1.Controls.Add(this.cboItemType);
            this.ultraTabPageControl1.Controls.Add(this.cboPriceType);
            this.ultraTabPageControl1.Controls.Add(this.txtSubElementSep);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel18);
            this.ultraTabPageControl1.Controls.Add(this.txtOutboundDir);
            this.ultraTabPageControl1.Controls.Add(this.txtInboundDir);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel16);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel17);
            this.ultraTabPageControl1.Controls.Add(this.txtSegmentSepfix);
            this.ultraTabPageControl1.Controls.Add(this.txtElementSepfix);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel14);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel15);
            this.ultraTabPageControl1.Controls.Add(this.txtPriceFileFormatfix);
            this.ultraTabPageControl1.Controls.Add(this.txtAckFileFormatfix);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel12);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel13);
            resources.ApplyResources(this.ultraTabPageControl1, "ultraTabPageControl1");
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            // 
            // ultraLabel29
            // 
            appearance55.FontData.BoldAsString = resources.GetString("resource.BoldAsString54");
            appearance55.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString52");
            appearance55.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString52");
            appearance55.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString52");
            appearance55.ForeColor = System.Drawing.Color.White;
            appearance55.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel29.Appearance = appearance55;
            resources.ApplyResources(this.ultraLabel29, "ultraLabel29");
            this.ultraLabel29.Name = "ultraLabel29";
            this.ultraLabel29.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboEncryptionType
            // 
            this.cboEncryptionType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboEncryptionType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.cboEncryptionType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem6.DataValue = "0";
            valueListItem7.DataValue = "1";
            this.cboEncryptionType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem6,
            valueListItem7});
            this.cboEncryptionType.LimitToList = true;
            resources.ApplyResources(this.cboEncryptionType, "cboEncryptionType");
            this.cboEncryptionType.Name = "cboEncryptionType";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.optUseVendorItemCode);
            this.groupBox3.Controls.Add(this.optUseUPCCode);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // optUseVendorItemCode
            // 
            resources.ApplyResources(this.optUseVendorItemCode, "optUseVendorItemCode");
            this.optUseVendorItemCode.Name = "optUseVendorItemCode";
            // 
            // optUseUPCCode
            // 
            this.optUseUPCCode.Checked = true;
            resources.ApplyResources(this.optUseUPCCode, "optUseUPCCode");
            this.optUseUPCCode.Name = "optUseUPCCode";
            this.optUseUPCCode.TabStop = true;
            // 
            // ultraLabel28
            // 
            appearance56.FontData.BoldAsString = resources.GetString("resource.BoldAsString55");
            appearance56.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString53");
            appearance56.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString53");
            appearance56.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString53");
            appearance56.ForeColor = System.Drawing.Color.White;
            appearance56.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel28.Appearance = appearance56;
            resources.ApplyResources(this.ultraLabel28, "ultraLabel28");
            this.ultraLabel28.Name = "ultraLabel28";
            this.ultraLabel28.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel21
            // 
            appearance57.FontData.BoldAsString = resources.GetString("resource.BoldAsString56");
            appearance57.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString54");
            appearance57.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString54");
            appearance57.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString54");
            appearance57.ForeColor = System.Drawing.Color.White;
            appearance57.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel21.Appearance = appearance57;
            resources.ApplyResources(this.ultraLabel21, "ultraLabel21");
            this.ultraLabel21.Name = "ultraLabel21";
            this.ultraLabel21.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // cboItemType
            // 
            this.cboItemType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboItemType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.cboItemType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem8.DataValue = "IN";
            valueListItem9.DataValue = "N1";
            valueListItem10.DataValue = "N2";
            valueListItem11.DataValue = "N3";
            valueListItem12.DataValue = "N4";
            valueListItem13.DataValue = "NH";
            valueListItem14.DataValue = "RP";
            valueListItem15.DataValue = "UA";
            valueListItem16.DataValue = "UG";
            valueListItem17.DataValue = "UH";
            valueListItem18.DataValue = "UI";
            valueListItem19.DataValue = "UN";
            valueListItem20.DataValue = "UP";
            valueListItem21.DataValue = "VN";
            this.cboItemType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem8,
            valueListItem9,
            valueListItem10,
            valueListItem11,
            valueListItem12,
            valueListItem13,
            valueListItem14,
            valueListItem15,
            valueListItem16,
            valueListItem17,
            valueListItem18,
            valueListItem19,
            valueListItem20,
            valueListItem21});
            resources.ApplyResources(this.cboItemType, "cboItemType");
            this.cboItemType.Name = "cboItemType";
            // 
            // cboPriceType
            // 
            this.cboPriceType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cboPriceType.ButtonStyle = Infragistics.Win.UIElementButtonStyle.OfficeXPToolbarButton;
            this.cboPriceType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem22.DataValue = "AWP";
            valueListItem23.DataValue = "CAT";
            valueListItem24.DataValue = "CON";
            valueListItem25.DataValue = "DAP";
            valueListItem26.DataValue = "FUL";
            valueListItem27.DataValue = "MSR";
            valueListItem28.DataValue = "PRO";
            valueListItem29.DataValue = "RES";
            valueListItem30.DataValue = "ZN1";
            valueListItem31.DataValue = "ZN2";
            valueListItem32.DataValue = "ZN3";
            valueListItem33.DataValue = "ZN4";
            this.cboPriceType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem22,
            valueListItem23,
            valueListItem24,
            valueListItem25,
            valueListItem26,
            valueListItem27,
            valueListItem28,
            valueListItem29,
            valueListItem30,
            valueListItem31,
            valueListItem32,
            valueListItem33});
            resources.ApplyResources(this.cboPriceType, "cboPriceType");
            this.cboPriceType.Name = "cboPriceType";
            // 
            // txtSubElementSep
            // 
            this.txtSubElementSep.AcceptsTab = true;
            appearance58.FontData.BoldAsString = resources.GetString("resource.BoldAsString57");
            appearance58.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString55");
            appearance58.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString55");
            appearance58.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString55");
            appearance58.ForeColor = System.Drawing.Color.Black;
            appearance58.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtSubElementSep.Appearance = appearance58;
            this.txtSubElementSep.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtSubElementSep, "txtSubElementSep");
            this.txtSubElementSep.MaxLength = 50;
            this.txtSubElementSep.Name = "txtSubElementSep";
            this.txtSubElementSep.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtSubElementSep.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel18
            // 
            appearance59.FontData.BoldAsString = resources.GetString("resource.BoldAsString58");
            appearance59.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString56");
            appearance59.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString56");
            appearance59.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString56");
            appearance59.ForeColor = System.Drawing.Color.White;
            appearance59.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel18.Appearance = appearance59;
            resources.ApplyResources(this.ultraLabel18, "ultraLabel18");
            this.ultraLabel18.Name = "ultraLabel18";
            this.ultraLabel18.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtOutboundDir
            // 
            this.txtOutboundDir.AcceptsTab = true;
            appearance60.FontData.BoldAsString = resources.GetString("resource.BoldAsString59");
            appearance60.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString57");
            appearance60.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString57");
            appearance60.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString57");
            appearance60.ForeColor = System.Drawing.Color.Black;
            appearance60.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtOutboundDir.Appearance = appearance60;
            this.txtOutboundDir.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtOutboundDir, "txtOutboundDir");
            this.txtOutboundDir.MaxLength = 50;
            this.txtOutboundDir.Name = "txtOutboundDir";
            this.txtOutboundDir.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtOutboundDir.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtInboundDir
            // 
            this.txtInboundDir.AcceptsTab = true;
            appearance61.FontData.BoldAsString = resources.GetString("resource.BoldAsString60");
            appearance61.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString58");
            appearance61.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString58");
            appearance61.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString58");
            appearance61.ForeColor = System.Drawing.Color.Black;
            appearance61.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtInboundDir.Appearance = appearance61;
            this.txtInboundDir.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtInboundDir, "txtInboundDir");
            this.txtInboundDir.MaxLength = 50;
            this.txtInboundDir.Name = "txtInboundDir";
            this.txtInboundDir.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtInboundDir.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel16
            // 
            appearance62.FontData.BoldAsString = resources.GetString("resource.BoldAsString61");
            appearance62.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString59");
            appearance62.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString59");
            appearance62.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString59");
            appearance62.ForeColor = System.Drawing.Color.White;
            appearance62.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel16.Appearance = appearance62;
            resources.ApplyResources(this.ultraLabel16, "ultraLabel16");
            this.ultraLabel16.Name = "ultraLabel16";
            this.ultraLabel16.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel17
            // 
            appearance63.FontData.BoldAsString = resources.GetString("resource.BoldAsString62");
            appearance63.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString60");
            appearance63.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString60");
            appearance63.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString60");
            appearance63.ForeColor = System.Drawing.Color.White;
            appearance63.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel17.Appearance = appearance63;
            resources.ApplyResources(this.ultraLabel17, "ultraLabel17");
            this.ultraLabel17.Name = "ultraLabel17";
            this.ultraLabel17.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtSegmentSepfix
            // 
            this.txtSegmentSepfix.AcceptsTab = true;
            appearance64.FontData.BoldAsString = resources.GetString("resource.BoldAsString63");
            appearance64.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString61");
            appearance64.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString61");
            appearance64.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString61");
            appearance64.ForeColor = System.Drawing.Color.Black;
            appearance64.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtSegmentSepfix.Appearance = appearance64;
            this.txtSegmentSepfix.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtSegmentSepfix, "txtSegmentSepfix");
            this.txtSegmentSepfix.MaxLength = 50;
            this.txtSegmentSepfix.Name = "txtSegmentSepfix";
            this.txtSegmentSepfix.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtSegmentSepfix.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtElementSepfix
            // 
            this.txtElementSepfix.AcceptsTab = true;
            appearance65.FontData.BoldAsString = resources.GetString("resource.BoldAsString64");
            appearance65.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString62");
            appearance65.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString62");
            appearance65.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString62");
            appearance65.ForeColor = System.Drawing.Color.Black;
            appearance65.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtElementSepfix.Appearance = appearance65;
            this.txtElementSepfix.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtElementSepfix, "txtElementSepfix");
            this.txtElementSepfix.MaxLength = 50;
            this.txtElementSepfix.Name = "txtElementSepfix";
            this.txtElementSepfix.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtElementSepfix.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel14
            // 
            appearance66.FontData.BoldAsString = resources.GetString("resource.BoldAsString65");
            appearance66.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString63");
            appearance66.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString63");
            appearance66.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString63");
            appearance66.ForeColor = System.Drawing.Color.White;
            appearance66.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel14.Appearance = appearance66;
            resources.ApplyResources(this.ultraLabel14, "ultraLabel14");
            this.ultraLabel14.Name = "ultraLabel14";
            this.ultraLabel14.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel15
            // 
            appearance67.FontData.BoldAsString = resources.GetString("resource.BoldAsString66");
            appearance67.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString64");
            appearance67.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString64");
            appearance67.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString64");
            appearance67.ForeColor = System.Drawing.Color.White;
            appearance67.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel15.Appearance = appearance67;
            resources.ApplyResources(this.ultraLabel15, "ultraLabel15");
            this.ultraLabel15.Name = "ultraLabel15";
            this.ultraLabel15.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtPriceFileFormatfix
            // 
            this.txtPriceFileFormatfix.AcceptsTab = true;
            appearance68.FontData.BoldAsString = resources.GetString("resource.BoldAsString67");
            appearance68.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString65");
            appearance68.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString65");
            appearance68.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString65");
            appearance68.ForeColor = System.Drawing.Color.Black;
            appearance68.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPriceFileFormatfix.Appearance = appearance68;
            this.txtPriceFileFormatfix.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtPriceFileFormatfix, "txtPriceFileFormatfix");
            this.txtPriceFileFormatfix.MaxLength = 50;
            this.txtPriceFileFormatfix.Name = "txtPriceFileFormatfix";
            this.txtPriceFileFormatfix.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPriceFileFormatfix.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtAckFileFormatfix
            // 
            this.txtAckFileFormatfix.AcceptsTab = true;
            appearance69.FontData.BoldAsString = resources.GetString("resource.BoldAsString68");
            appearance69.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString66");
            appearance69.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString66");
            appearance69.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString66");
            appearance69.ForeColor = System.Drawing.Color.Black;
            appearance69.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtAckFileFormatfix.Appearance = appearance69;
            this.txtAckFileFormatfix.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtAckFileFormatfix, "txtAckFileFormatfix");
            this.txtAckFileFormatfix.MaxLength = 50;
            this.txtAckFileFormatfix.Name = "txtAckFileFormatfix";
            this.txtAckFileFormatfix.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtAckFileFormatfix.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel12
            // 
            appearance70.FontData.BoldAsString = resources.GetString("resource.BoldAsString69");
            appearance70.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString67");
            appearance70.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString67");
            appearance70.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString67");
            appearance70.ForeColor = System.Drawing.Color.White;
            appearance70.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel12.Appearance = appearance70;
            resources.ApplyResources(this.ultraLabel12, "ultraLabel12");
            this.ultraLabel12.Name = "ultraLabel12";
            this.ultraLabel12.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel13
            // 
            appearance71.FontData.BoldAsString = resources.GetString("resource.BoldAsString70");
            appearance71.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString68");
            appearance71.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString68");
            appearance71.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString68");
            appearance71.ForeColor = System.Drawing.Color.White;
            appearance71.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel13.Appearance = appearance71;
            resources.ApplyResources(this.ultraLabel13, "ultraLabel13");
            this.ultraLabel13.Name = "ultraLabel13";
            this.ultraLabel13.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkReduceSPWithPriceUpd);
            this.groupBox1.Controls.Add(this.cboSalePriceQualifie);
            this.groupBox1.Controls.Add(this.ultraLabel19);
            this.groupBox1.Controls.Add(this.chkSalePriceUpdate);
            this.groupBox1.Controls.Add(this.chkAckPriceUpdate);
            this.groupBox1.Controls.Add(this.cboCostQualifier);
            this.groupBox1.Controls.Add(this.cboPriceQualifier);
            this.groupBox1.Controls.Add(this.chkprocess810);
            this.groupBox1.Controls.Add(this.chkSendVendorCostPrice);
            this.groupBox1.Controls.Add(this.txtCellNo);
            this.groupBox1.Controls.Add(this.chkIsAutoClose);
            this.groupBox1.Controls.Add(this.maskedEditTimeToOrder);
            this.groupBox1.Controls.Add(this.lblTimeToOrder);
            this.groupBox1.Controls.Add(this.chkUpdatePrice);
            this.groupBox1.Controls.Add(this.lblCostQualifier);
            this.groupBox1.Controls.Add(this.ultraLabel30);
            this.groupBox1.Controls.Add(this.chkIsEPO);
            this.groupBox1.Controls.Add(this.cboVendorList);
            this.groupBox1.Controls.Add(this.lblPrimePoVendor);
            this.groupBox1.Controls.Add(this.txtState);
            this.groupBox1.Controls.Add(this.txtEmailAddr);
            this.groupBox1.Controls.Add(this.txtVendorCode);
            this.groupBox1.Controls.Add(this.txtFaxNo);
            this.groupBox1.Controls.Add(this.txtPhoneOff);
            this.groupBox1.Controls.Add(this.txtZipCode);
            this.groupBox1.Controls.Add(this.ultraLabel6);
            this.groupBox1.Controls.Add(this.ultraLabel25);
            this.groupBox1.Controls.Add(this.ultraLabel2);
            this.groupBox1.Controls.Add(this.ultraLabel1);
            this.groupBox1.Controls.Add(this.txtVendorName);
            this.groupBox1.Controls.Add(this.ultraLabel27);
            this.groupBox1.Controls.Add(this.txtAddress1);
            this.groupBox1.Controls.Add(this.ultraLabel24);
            this.groupBox1.Controls.Add(this.txtAddress2);
            this.groupBox1.Controls.Add(this.ultraLabel22);
            this.groupBox1.Controls.Add(this.ultraLabel4);
            this.groupBox1.Controls.Add(this.chkIsActive);
            this.groupBox1.Controls.Add(this.ultraLabel5);
            this.groupBox1.Controls.Add(this.txtWebAddress);
            this.groupBox1.Controls.Add(this.ultraLabel3);
            this.groupBox1.Controls.Add(this.txtCity);
            this.groupBox1.Controls.Add(this.ultraLabel23);
            this.groupBox1.Controls.Add(this.ultraLabel26);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkReduceSPWithPriceUpd
            // 
            resources.ApplyResources(this.chkReduceSPWithPriceUpd, "chkReduceSPWithPriceUpd");
            this.chkReduceSPWithPriceUpd.Checked = true;
            this.chkReduceSPWithPriceUpd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReduceSPWithPriceUpd.Name = "chkReduceSPWithPriceUpd";
            this.chkReduceSPWithPriceUpd.UseVisualStyleBackColor = true;
            // 
            // cboSalePriceQualifie
            // 
            resources.ApplyResources(this.cboSalePriceQualifie, "cboSalePriceQualifie");
            this.cboSalePriceQualifie.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem49.DataValue = "0";
            resources.ApplyResources(valueListItem49, "valueListItem49");
            valueListItem49.ForceApplyResources = "";
            valueListItem61.DataValue = "13";
            resources.ApplyResources(valueListItem61, "valueListItem61");
            valueListItem61.ForceApplyResources = "";
            valueListItem50.DataValue = "1";
            resources.ApplyResources(valueListItem50, "valueListItem50");
            valueListItem50.ForceApplyResources = "";
            valueListItem51.DataValue = "2";
            resources.ApplyResources(valueListItem51, "valueListItem51");
            valueListItem51.ForceApplyResources = "";
            valueListItem52.DataValue = "4";
            resources.ApplyResources(valueListItem52, "valueListItem52");
            valueListItem52.ForceApplyResources = "";
            valueListItem53.DataValue = "5";
            resources.ApplyResources(valueListItem53, "valueListItem53");
            valueListItem53.ForceApplyResources = "";
            valueListItem59.DataValue = "11";
            resources.ApplyResources(valueListItem59, "valueListItem59");
            valueListItem59.ForceApplyResources = "";
            valueListItem54.DataValue = "6";
            resources.ApplyResources(valueListItem54, "valueListItem54");
            valueListItem54.ForceApplyResources = "";
            valueListItem55.DataValue = "7";
            resources.ApplyResources(valueListItem55, "valueListItem55");
            valueListItem55.ForceApplyResources = "";
            valueListItem60.DataValue = "12";
            resources.ApplyResources(valueListItem60, "valueListItem60");
            valueListItem60.ForceApplyResources = "";
            valueListItem56.DataValue = "8";
            resources.ApplyResources(valueListItem56, "valueListItem56");
            valueListItem56.ForceApplyResources = "";
            valueListItem80.DataValue = "17";
            resources.ApplyResources(valueListItem80, "valueListItem80");
            valueListItem80.ForceApplyResources = "";
            valueListItem57.DataValue = "9";
            resources.ApplyResources(valueListItem57, "valueListItem57");
            valueListItem57.ForceApplyResources = "";
            valueListItem62.DataValue = "14";
            resources.ApplyResources(valueListItem62, "valueListItem62");
            valueListItem62.ForceApplyResources = "";
            valueListItem79.DataValue = "16";
            resources.ApplyResources(valueListItem79, "valueListItem79");
            valueListItem79.ForceApplyResources = "";
            valueListItem58.DataValue = "10";
            resources.ApplyResources(valueListItem58, "valueListItem58");
            valueListItem58.ForceApplyResources = "";
            valueListItem63.DataValue = "15";
            resources.ApplyResources(valueListItem63, "valueListItem63");
            valueListItem63.ForceApplyResources = "";
            this.cboSalePriceQualifie.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem49,
            valueListItem61,
            valueListItem50,
            valueListItem51,
            valueListItem52,
            valueListItem53,
            valueListItem59,
            valueListItem54,
            valueListItem55,
            valueListItem60,
            valueListItem56,
            valueListItem80,
            valueListItem57,
            valueListItem62,
            valueListItem79,
            valueListItem58,
            valueListItem63});
            this.cboSalePriceQualifie.LimitToList = true;
            this.cboSalePriceQualifie.Name = "cboSalePriceQualifie";
            this.cboSalePriceQualifie.SelectionChanged += new System.EventHandler(this.cboSalePriceQualifie_SelectionChanged);
            // 
            // ultraLabel19
            // 
            resources.ApplyResources(this.ultraLabel19, "ultraLabel19");
            this.ultraLabel19.Name = "ultraLabel19";
            // 
            // chkSalePriceUpdate
            // 
            resources.ApplyResources(this.chkSalePriceUpdate, "chkSalePriceUpdate");
            this.chkSalePriceUpdate.Checked = true;
            this.chkSalePriceUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSalePriceUpdate.Name = "chkSalePriceUpdate";
            this.chkSalePriceUpdate.UseVisualStyleBackColor = true;
            this.chkSalePriceUpdate.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
            // 
            // chkAckPriceUpdate
            // 
            resources.ApplyResources(this.chkAckPriceUpdate, "chkAckPriceUpdate");
            this.chkAckPriceUpdate.Checked = true;
            this.chkAckPriceUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAckPriceUpdate.Name = "chkAckPriceUpdate";
            this.chkAckPriceUpdate.UseVisualStyleBackColor = true;
            this.chkAckPriceUpdate.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
            // 
            // cboCostQualifier
            // 
            resources.ApplyResources(this.cboCostQualifier, "cboCostQualifier");
            this.cboCostQualifier.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem81.DataValue = "16";
            resources.ApplyResources(valueListItem81, "valueListItem81");
            valueListItem81.ForceApplyResources = "";
            valueListItem82.DataValue = "17";
            resources.ApplyResources(valueListItem82, "valueListItem82");
            valueListItem82.ForceApplyResources = "";
            valueListItem34.DataValue = "0";
            resources.ApplyResources(valueListItem34, "valueListItem34");
            valueListItem34.ForceApplyResources = "";
            valueListItem35.DataValue = "13";
            resources.ApplyResources(valueListItem35, "valueListItem35");
            valueListItem35.ForceApplyResources = "";
            valueListItem36.DataValue = "1";
            resources.ApplyResources(valueListItem36, "valueListItem36");
            valueListItem36.ForceApplyResources = "";
            valueListItem37.DataValue = "2";
            resources.ApplyResources(valueListItem37, "valueListItem37");
            valueListItem37.ForceApplyResources = "";
            valueListItem38.DataValue = "3";
            resources.ApplyResources(valueListItem38, "valueListItem38");
            valueListItem38.ForceApplyResources = "";
            valueListItem39.DataValue = "4";
            resources.ApplyResources(valueListItem39, "valueListItem39");
            valueListItem39.ForceApplyResources = "";
            valueListItem40.DataValue = "11";
            resources.ApplyResources(valueListItem40, "valueListItem40");
            valueListItem40.ForceApplyResources = "";
            valueListItem41.DataValue = "5";
            resources.ApplyResources(valueListItem41, "valueListItem41");
            valueListItem41.ForceApplyResources = "";
            valueListItem42.DataValue = "6";
            resources.ApplyResources(valueListItem42, "valueListItem42");
            valueListItem42.ForceApplyResources = "";
            valueListItem43.DataValue = "12";
            resources.ApplyResources(valueListItem43, "valueListItem43");
            valueListItem43.ForceApplyResources = "";
            valueListItem44.DataValue = "7";
            resources.ApplyResources(valueListItem44, "valueListItem44");
            valueListItem44.ForceApplyResources = "";
            valueListItem45.DataValue = "8";
            resources.ApplyResources(valueListItem45, "valueListItem45");
            valueListItem45.ForceApplyResources = "";
            valueListItem46.DataValue = "14";
            resources.ApplyResources(valueListItem46, "valueListItem46");
            valueListItem46.ForceApplyResources = "";
            valueListItem47.DataValue = "10";
            resources.ApplyResources(valueListItem47, "valueListItem47");
            valueListItem47.ForceApplyResources = "";
            valueListItem48.DataValue = "15";
            resources.ApplyResources(valueListItem48, "valueListItem48");
            valueListItem48.ForceApplyResources = "";
            this.cboCostQualifier.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem81,
            valueListItem82,
            valueListItem34,
            valueListItem35,
            valueListItem36,
            valueListItem37,
            valueListItem38,
            valueListItem39,
            valueListItem40,
            valueListItem41,
            valueListItem42,
            valueListItem43,
            valueListItem44,
            valueListItem45,
            valueListItem46,
            valueListItem47,
            valueListItem48});
            this.cboCostQualifier.LimitToList = true;
            this.cboCostQualifier.Name = "cboCostQualifier";
            this.cboCostQualifier.SortStyle = Infragistics.Win.ValueListSortStyle.Ascending;
            this.cboCostQualifier.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboCostQualifier.SelectionChanged += new System.EventHandler(this.cboCostQualifier_SelectionChanged);
            // 
            // cboPriceQualifier
            // 
            resources.ApplyResources(this.cboPriceQualifier, "cboPriceQualifier");
            this.cboPriceQualifier.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem64.DataValue = "0";
            resources.ApplyResources(valueListItem64, "valueListItem64");
            valueListItem64.ForceApplyResources = "";
            valueListItem76.DataValue = "13";
            resources.ApplyResources(valueListItem76, "valueListItem76");
            valueListItem76.ForceApplyResources = "";
            valueListItem65.DataValue = "1";
            resources.ApplyResources(valueListItem65, "valueListItem65");
            valueListItem65.ForceApplyResources = "";
            valueListItem66.DataValue = "2";
            resources.ApplyResources(valueListItem66, "valueListItem66");
            valueListItem66.ForceApplyResources = "";
            valueListItem67.DataValue = "4";
            resources.ApplyResources(valueListItem67, "valueListItem67");
            valueListItem67.ForceApplyResources = "";
            valueListItem68.DataValue = "5";
            resources.ApplyResources(valueListItem68, "valueListItem68");
            valueListItem68.ForceApplyResources = "";
            valueListItem74.DataValue = "11";
            resources.ApplyResources(valueListItem74, "valueListItem74");
            valueListItem74.ForceApplyResources = "";
            valueListItem69.DataValue = "6";
            resources.ApplyResources(valueListItem69, "valueListItem69");
            valueListItem69.ForceApplyResources = "";
            valueListItem70.DataValue = "7";
            resources.ApplyResources(valueListItem70, "valueListItem70");
            valueListItem70.ForceApplyResources = "";
            valueListItem75.DataValue = "12";
            resources.ApplyResources(valueListItem75, "valueListItem75");
            valueListItem75.ForceApplyResources = "";
            valueListItem71.DataValue = "8";
            resources.ApplyResources(valueListItem71, "valueListItem71");
            valueListItem71.ForceApplyResources = "";
            valueListItem84.DataValue = "17";
            resources.ApplyResources(valueListItem84, "valueListItem84");
            valueListItem84.ForceApplyResources = "";
            valueListItem72.DataValue = "9";
            resources.ApplyResources(valueListItem72, "valueListItem72");
            valueListItem72.ForceApplyResources = "";
            valueListItem77.DataValue = "14";
            resources.ApplyResources(valueListItem77, "valueListItem77");
            valueListItem77.ForceApplyResources = "";
            valueListItem83.DataValue = "16";
            resources.ApplyResources(valueListItem83, "valueListItem83");
            valueListItem83.ForceApplyResources = "";
            valueListItem73.DataValue = "10";
            resources.ApplyResources(valueListItem73, "valueListItem73");
            valueListItem73.ForceApplyResources = "";
            valueListItem78.DataValue = "15";
            resources.ApplyResources(valueListItem78, "valueListItem78");
            valueListItem78.ForceApplyResources = "";
            this.cboPriceQualifier.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem64,
            valueListItem76,
            valueListItem65,
            valueListItem66,
            valueListItem67,
            valueListItem68,
            valueListItem74,
            valueListItem69,
            valueListItem70,
            valueListItem75,
            valueListItem71,
            valueListItem84,
            valueListItem72,
            valueListItem77,
            valueListItem83,
            valueListItem73,
            valueListItem78});
            this.cboPriceQualifier.Name = "cboPriceQualifier";
            this.cboPriceQualifier.SelectionChanged += new System.EventHandler(this.cboPriceQualifier_SelectionChanged);
            // 
            // chkprocess810
            // 
            resources.ApplyResources(this.chkprocess810, "chkprocess810");
            this.chkprocess810.ForeColor = System.Drawing.Color.White;
            this.chkprocess810.Name = "chkprocess810";
            this.chkprocess810.UseVisualStyleBackColor = true;
            this.chkprocess810.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
            // 
            // chkSendVendorCostPrice
            // 
            resources.ApplyResources(this.chkSendVendorCostPrice, "chkSendVendorCostPrice");
            this.chkSendVendorCostPrice.Checked = true;
            this.chkSendVendorCostPrice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSendVendorCostPrice.Name = "chkSendVendorCostPrice";
            this.chkSendVendorCostPrice.UseVisualStyleBackColor = true;
            // 
            // txtCellNo
            // 
            appearance72.FontData.BoldAsString = resources.GetString("resource.BoldAsString71");
            appearance72.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString69");
            appearance72.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString69");
            appearance72.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString69");
            appearance72.ForeColor = System.Drawing.Color.Black;
            appearance72.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtCellNo.Appearance = appearance72;
            this.txtCellNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtCellNo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.UseSpecifiedMask;
            this.txtCellNo.InputMask = "(999)999-9999";
            resources.ApplyResources(this.txtCellNo, "txtCellNo");
            this.txtCellNo.Name = "txtCellNo";
            this.txtCellNo.NonAutoSizeHeight = 21;
            this.txtCellNo.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtCellNo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCellNo.Validated += new System.EventHandler(this.MaskEditBoxes_Validate);
            // 
            // chkIsAutoClose
            // 
            resources.ApplyResources(this.chkIsAutoClose, "chkIsAutoClose");
            this.chkIsAutoClose.Checked = true;
            this.chkIsAutoClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsAutoClose.ForeColor = System.Drawing.Color.White;
            this.chkIsAutoClose.Name = "chkIsAutoClose";
            this.chkIsAutoClose.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
            // 
            // maskedEditTimeToOrder
            // 
            appearance73.FontData.BoldAsString = resources.GetString("resource.BoldAsString72");
            appearance73.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString70");
            appearance73.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString70");
            appearance73.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString70");
            appearance73.ForeColor = System.Drawing.Color.Black;
            appearance73.ForeColorDisabled = System.Drawing.Color.Black;
            this.maskedEditTimeToOrder.Appearance = appearance73;
            this.maskedEditTimeToOrder.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.maskedEditTimeToOrder.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Time;
            this.maskedEditTimeToOrder.InputMask = "{time}";
            resources.ApplyResources(this.maskedEditTimeToOrder, "maskedEditTimeToOrder");
            this.maskedEditTimeToOrder.Name = "maskedEditTimeToOrder";
            this.maskedEditTimeToOrder.NonAutoSizeHeight = 21;
            this.maskedEditTimeToOrder.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.maskedEditTimeToOrder.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.maskedEditTimeToOrder.Validated += new System.EventHandler(this.MaskEditBoxes_Validate);
            // 
            // lblTimeToOrder
            // 
            resources.ApplyResources(this.lblTimeToOrder, "lblTimeToOrder");
            this.lblTimeToOrder.Name = "lblTimeToOrder";
            // 
            // chkUpdatePrice
            // 
            resources.ApplyResources(this.chkUpdatePrice, "chkUpdatePrice");
            this.chkUpdatePrice.Checked = true;
            this.chkUpdatePrice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdatePrice.Name = "chkUpdatePrice";
            this.chkUpdatePrice.UseVisualStyleBackColor = true;
            // 
            // lblCostQualifier
            // 
            resources.ApplyResources(this.lblCostQualifier, "lblCostQualifier");
            this.lblCostQualifier.Name = "lblCostQualifier";
            this.lblCostQualifier.Click += new System.EventHandler(this.lblCostQualifier_Click);
            // 
            // ultraLabel30
            // 
            resources.ApplyResources(this.ultraLabel30, "ultraLabel30");
            this.ultraLabel30.Name = "ultraLabel30";
            // 
            // chkIsEPO
            // 
            resources.ApplyResources(this.chkIsEPO, "chkIsEPO");
            this.chkIsEPO.Checked = true;
            this.chkIsEPO.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsEPO.ForeColor = System.Drawing.Color.White;
            this.chkIsEPO.Name = "chkIsEPO";
            // 
            // cboVendorList
            // 
            this.cboVendorList.DisplayMember = "NONE";
            this.cboVendorList.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cboVendorList.LimitToList = true;
            resources.ApplyResources(this.cboVendorList, "cboVendorList");
            this.cboVendorList.Name = "cboVendorList";
            this.cboVendorList.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cboVendorList.SelectionChanged += new System.EventHandler(this.cboVendorList_SelectionChanged);
            // 
            // lblPrimePoVendor
            // 
            resources.ApplyResources(this.lblPrimePoVendor, "lblPrimePoVendor");
            this.lblPrimePoVendor.Name = "lblPrimePoVendor";
            // 
            // txtState
            // 
            this.txtState.AcceptsTab = true;
            appearance74.FontData.BoldAsString = resources.GetString("resource.BoldAsString73");
            appearance74.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString71");
            appearance74.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString71");
            appearance74.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString71");
            appearance74.ForeColor = System.Drawing.Color.Black;
            appearance74.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtState.Appearance = appearance74;
            this.txtState.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtState, "txtState");
            this.txtState.MaxLength = 50;
            this.txtState.Name = "txtState";
            this.txtState.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtState.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtEmailAddr
            // 
            this.txtEmailAddr.AcceptsTab = true;
            appearance75.FontData.BoldAsString = resources.GetString("resource.BoldAsString74");
            appearance75.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString72");
            appearance75.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString72");
            appearance75.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString72");
            appearance75.ForeColor = System.Drawing.Color.Black;
            appearance75.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtEmailAddr.Appearance = appearance75;
            this.txtEmailAddr.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtEmailAddr.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            resources.ApplyResources(this.txtEmailAddr, "txtEmailAddr");
            this.txtEmailAddr.MaxLength = 100;
            this.txtEmailAddr.Name = "txtEmailAddr";
            this.txtEmailAddr.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtEmailAddr.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // txtVendorCode
            // 
            appearance76.FontData.BoldAsString = resources.GetString("resource.BoldAsString75");
            appearance76.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString73");
            appearance76.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString73");
            appearance76.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString73");
            appearance76.ForeColor = System.Drawing.Color.Black;
            appearance76.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtVendorCode.Appearance = appearance76;
            this.txtVendorCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtVendorCode, "txtVendorCode");
            this.txtVendorCode.MaxLength = 20;
            this.txtVendorCode.Name = "txtVendorCode";
            this.toolTip1.SetToolTip(this.txtVendorCode, resources.GetString("txtVendorCode.ToolTip"));
            this.txtVendorCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtFaxNo
            // 
            appearance77.FontData.BoldAsString = resources.GetString("resource.BoldAsString76");
            appearance77.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString74");
            appearance77.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString74");
            appearance77.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString74");
            appearance77.ForeColor = System.Drawing.Color.Black;
            appearance77.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtFaxNo.Appearance = appearance77;
            this.txtFaxNo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtFaxNo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.UseSpecifiedMask;
            this.txtFaxNo.InputMask = "(999)999-9999";
            resources.ApplyResources(this.txtFaxNo, "txtFaxNo");
            this.txtFaxNo.Name = "txtFaxNo";
            this.txtFaxNo.NonAutoSizeHeight = 21;
            this.txtFaxNo.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtFaxNo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtFaxNo.Validated += new System.EventHandler(this.MaskEditBoxes_Validate);
            // 
            // txtPhoneOff
            // 
            appearance78.FontData.BoldAsString = resources.GetString("resource.BoldAsString77");
            appearance78.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString75");
            appearance78.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString75");
            appearance78.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString75");
            appearance78.ForeColor = System.Drawing.Color.Black;
            appearance78.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtPhoneOff.Appearance = appearance78;
            this.txtPhoneOff.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtPhoneOff.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.UseSpecifiedMask;
            this.txtPhoneOff.InputMask = "(999)999-9999";
            resources.ApplyResources(this.txtPhoneOff, "txtPhoneOff");
            this.txtPhoneOff.Name = "txtPhoneOff";
            this.txtPhoneOff.NonAutoSizeHeight = 21;
            this.txtPhoneOff.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtPhoneOff.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtPhoneOff.Validated += new System.EventHandler(this.MaskEditBoxes_Validate);
            // 
            // txtZipCode
            // 
            appearance79.FontData.BoldAsString = resources.GetString("resource.BoldAsString78");
            appearance79.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString76");
            appearance79.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString76");
            appearance79.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString76");
            appearance79.ForeColor = System.Drawing.Color.Black;
            appearance79.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtZipCode.Appearance = appearance79;
            this.txtZipCode.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtZipCode.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.UseSpecifiedMask;
            this.txtZipCode.InputMask = "9999999999";
            resources.ApplyResources(this.txtZipCode, "txtZipCode");
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.NonAutoSizeHeight = 21;
            this.txtZipCode.TabNavigation = Infragistics.Win.UltraWinMaskedEdit.MaskedEditTabNavigation.NextControl;
            this.txtZipCode.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtZipCode.Validated += new System.EventHandler(this.MaskEditBoxes_Validate);
            // 
            // ultraLabel6
            // 
            appearance80.FontData.BoldAsString = resources.GetString("resource.BoldAsString79");
            appearance80.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString77");
            appearance80.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString77");
            appearance80.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString77");
            appearance80.ForeColor = System.Drawing.Color.White;
            appearance80.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel6.Appearance = appearance80;
            resources.ApplyResources(this.ultraLabel6, "ultraLabel6");
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel25
            // 
            appearance81.FontData.BoldAsString = resources.GetString("resource.BoldAsString80");
            appearance81.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString78");
            appearance81.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString78");
            appearance81.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString78");
            appearance81.ForeColor = System.Drawing.Color.White;
            appearance81.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel25.Appearance = appearance81;
            resources.ApplyResources(this.ultraLabel25, "ultraLabel25");
            this.ultraLabel25.Name = "ultraLabel25";
            this.ultraLabel25.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel2
            // 
            appearance82.FontData.BoldAsString = resources.GetString("resource.BoldAsString81");
            appearance82.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString79");
            appearance82.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString79");
            appearance82.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString79");
            appearance82.ForeColor = System.Drawing.Color.White;
            appearance82.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel2.Appearance = appearance82;
            resources.ApplyResources(this.ultraLabel2, "ultraLabel2");
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel1
            // 
            appearance83.FontData.BoldAsString = resources.GetString("resource.BoldAsString82");
            appearance83.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString80");
            appearance83.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString80");
            appearance83.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString80");
            appearance83.ForeColor = System.Drawing.Color.White;
            appearance83.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance83;
            resources.ApplyResources(this.ultraLabel1, "ultraLabel1");
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtVendorName
            // 
            this.txtVendorName.AcceptsTab = true;
            appearance84.FontData.BoldAsString = resources.GetString("resource.BoldAsString83");
            appearance84.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString81");
            appearance84.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString81");
            appearance84.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString81");
            appearance84.ForeColor = System.Drawing.Color.Black;
            appearance84.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtVendorName.Appearance = appearance84;
            this.txtVendorName.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtVendorName, "txtVendorName");
            this.txtVendorName.MaxLength = 50;
            this.txtVendorName.Name = "txtVendorName";
            this.toolTip1.SetToolTip(this.txtVendorName, resources.GetString("txtVendorName.ToolTip"));
            this.txtVendorName.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtVendorName.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel27
            // 
            appearance85.FontData.BoldAsString = resources.GetString("resource.BoldAsString84");
            appearance85.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString82");
            appearance85.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString82");
            appearance85.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString82");
            appearance85.ForeColor = System.Drawing.Color.White;
            appearance85.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel27.Appearance = appearance85;
            resources.ApplyResources(this.ultraLabel27, "ultraLabel27");
            this.ultraLabel27.Name = "ultraLabel27";
            this.ultraLabel27.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtAddress1
            // 
            this.txtAddress1.AcceptsTab = true;
            appearance86.FontData.BoldAsString = resources.GetString("resource.BoldAsString85");
            appearance86.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString83");
            appearance86.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString83");
            appearance86.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString83");
            appearance86.ForeColor = System.Drawing.Color.Black;
            appearance86.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtAddress1.Appearance = appearance86;
            this.txtAddress1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtAddress1, "txtAddress1");
            this.txtAddress1.MaxLength = 200;
            this.txtAddress1.Name = "txtAddress1";
            this.txtAddress1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtAddress1.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel24
            // 
            appearance87.FontData.BoldAsString = resources.GetString("resource.BoldAsString86");
            appearance87.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString84");
            appearance87.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString84");
            appearance87.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString84");
            appearance87.ForeColor = System.Drawing.Color.White;
            appearance87.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel24.Appearance = appearance87;
            resources.ApplyResources(this.ultraLabel24, "ultraLabel24");
            this.ultraLabel24.Name = "ultraLabel24";
            this.ultraLabel24.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtAddress2
            // 
            this.txtAddress2.AcceptsTab = true;
            appearance88.FontData.BoldAsString = resources.GetString("resource.BoldAsString87");
            appearance88.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString85");
            appearance88.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString85");
            appearance88.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString85");
            appearance88.ForeColor = System.Drawing.Color.Black;
            appearance88.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtAddress2.Appearance = appearance88;
            this.txtAddress2.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtAddress2, "txtAddress2");
            this.txtAddress2.MaxLength = 200;
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtAddress2.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel22
            // 
            appearance89.FontData.BoldAsString = resources.GetString("resource.BoldAsString88");
            appearance89.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString86");
            appearance89.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString86");
            appearance89.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString86");
            appearance89.ForeColor = System.Drawing.Color.White;
            appearance89.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel22.Appearance = appearance89;
            resources.ApplyResources(this.ultraLabel22, "ultraLabel22");
            this.ultraLabel22.Name = "ultraLabel22";
            this.ultraLabel22.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel4
            // 
            appearance90.FontData.BoldAsString = resources.GetString("resource.BoldAsString89");
            appearance90.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString87");
            appearance90.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString87");
            appearance90.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString87");
            appearance90.ForeColor = System.Drawing.Color.White;
            appearance90.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel4.Appearance = appearance90;
            resources.ApplyResources(this.ultraLabel4, "ultraLabel4");
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // chkIsActive
            // 
            resources.ApplyResources(this.chkIsActive, "chkIsActive");
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.ForeColor = System.Drawing.Color.White;
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.CheckedChanged += new System.EventHandler(this.chkIs_CheckedChanged);
            // 
            // ultraLabel5
            // 
            appearance91.FontData.BoldAsString = resources.GetString("resource.BoldAsString90");
            appearance91.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString88");
            appearance91.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString88");
            appearance91.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString88");
            appearance91.ForeColor = System.Drawing.Color.White;
            appearance91.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel5.Appearance = appearance91;
            resources.ApplyResources(this.ultraLabel5, "ultraLabel5");
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtWebAddress
            // 
            this.txtWebAddress.AcceptsTab = true;
            appearance92.FontData.BoldAsString = resources.GetString("resource.BoldAsString91");
            appearance92.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString89");
            appearance92.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString89");
            appearance92.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString89");
            appearance92.ForeColor = System.Drawing.Color.Black;
            appearance92.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtWebAddress.Appearance = appearance92;
            this.txtWebAddress.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtWebAddress, "txtWebAddress");
            this.txtWebAddress.MaxLength = 100;
            this.txtWebAddress.Name = "txtWebAddress";
            this.txtWebAddress.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtWebAddress.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel3
            // 
            appearance93.FontData.BoldAsString = resources.GetString("resource.BoldAsString92");
            appearance93.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString90");
            appearance93.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString90");
            appearance93.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString90");
            appearance93.ForeColor = System.Drawing.Color.White;
            appearance93.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel3.Appearance = appearance93;
            resources.ApplyResources(this.ultraLabel3, "ultraLabel3");
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // txtCity
            // 
            this.txtCity.AcceptsTab = true;
            appearance94.FontData.BoldAsString = resources.GetString("resource.BoldAsString93");
            appearance94.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString91");
            appearance94.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString91");
            appearance94.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString91");
            appearance94.ForeColor = System.Drawing.Color.Black;
            appearance94.ForeColorDisabled = System.Drawing.Color.Black;
            this.txtCity.Appearance = appearance94;
            this.txtCity.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            resources.ApplyResources(this.txtCity, "txtCity");
            this.txtCity.MaxLength = 50;
            this.txtCity.Name = "txtCity";
            this.txtCity.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.txtCity.Validated += new System.EventHandler(this.txtBoxs_Validate);
            // 
            // ultraLabel23
            // 
            appearance95.FontData.BoldAsString = resources.GetString("resource.BoldAsString94");
            appearance95.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString92");
            appearance95.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString92");
            appearance95.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString92");
            appearance95.ForeColor = System.Drawing.Color.White;
            appearance95.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel23.Appearance = appearance95;
            resources.ApplyResources(this.ultraLabel23, "ultraLabel23");
            this.ultraLabel23.Name = "ultraLabel23";
            this.ultraLabel23.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraLabel26
            // 
            appearance96.FontData.BoldAsString = resources.GetString("resource.BoldAsString95");
            appearance96.FontData.ItalicAsString = resources.GetString("resource.ItalicAsString93");
            appearance96.FontData.StrikeoutAsString = resources.GetString("resource.StrikeoutAsString93");
            appearance96.FontData.UnderlineAsString = resources.GetString("resource.UnderlineAsString93");
            appearance96.ForeColor = System.Drawing.Color.White;
            appearance96.ForeColorDisabled = System.Drawing.Color.Black;
            this.ultraLabel26.Appearance = appearance96;
            resources.ApplyResources(this.ultraLabel26, "ultraLabel26");
            this.ultraLabel26.Name = "ultraLabel26";
            this.ultraLabel26.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraDataSource1
            // 
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4,
            ultraDataColumn5,
            ultraDataColumn6,
            ultraDataColumn7,
            ultraDataColumn8,
            ultraDataColumn9,
            ultraDataColumn10,
            ultraDataColumn11,
            ultraDataColumn12});
            // 
            // lblTransactionType
            // 
            appearance97.ForeColor = System.Drawing.Color.White;
            appearance97.ForeColorDisabled = System.Drawing.Color.Navy;
            this.lblTransactionType.Appearance = appearance97;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblTransactionType, "lblTransactionType");
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pnlVendorNote);
            this.groupBox2.Controls.Add(this.pnlSave);
            this.groupBox2.Controls.Add(this.pnlClose);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // pnlVendorNote
            // 
            // 
            // pnlVendorNote.ClientArea
            // 
            this.pnlVendorNote.ClientArea.Controls.Add(this.btnVendorNote);
            this.pnlVendorNote.ClientArea.Controls.Add(this.lblVendorNote);
            resources.ApplyResources(this.pnlVendorNote, "pnlVendorNote");
            this.pnlVendorNote.Name = "pnlVendorNote";
            // 
            // btnVendorNote
            // 
            appearance98.BackColor = System.Drawing.Color.White;
            appearance98.FontData.BoldAsString = resources.GetString("resource.BoldAsString96");
            appearance98.ForeColor = System.Drawing.Color.Black;
            this.btnVendorNote.Appearance = appearance98;
            this.btnVendorNote.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            resources.ApplyResources(this.btnVendorNote, "btnVendorNote");
            this.btnVendorNote.Name = "btnVendorNote";
            this.btnVendorNote.TabStop = false;
            this.btnVendorNote.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnVendorNote.Click += new System.EventHandler(this.btnVendorNote_Click);
            // 
            // lblVendorNote
            // 
            appearance99.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance99.FontData.BoldAsString = resources.GetString("resource.BoldAsString97");
            appearance99.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(appearance99, "appearance99");
            this.lblVendorNote.Appearance = appearance99;
            resources.ApplyResources(this.lblVendorNote, "lblVendorNote");
            this.lblVendorNote.Name = "lblVendorNote";
            this.lblVendorNote.Tag = "NOCOLOR";
            this.lblVendorNote.Click += new System.EventHandler(this.btnVendorNote_Click);
            // 
            // pnlSave
            // 
            // 
            // pnlSave.ClientArea
            // 
            this.pnlSave.ClientArea.Controls.Add(this.btnSave);
            this.pnlSave.ClientArea.Controls.Add(this.lblSave);
            resources.ApplyResources(this.pnlSave, "pnlSave");
            this.pnlSave.Name = "pnlSave";
            // 
            // btnSave
            // 
            appearance100.BackColor = System.Drawing.Color.White;
            appearance100.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance = appearance100;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSave
            // 
            appearance101.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance101.FontData.BoldAsString = resources.GetString("resource.BoldAsString98");
            appearance101.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(appearance101, "appearance101");
            this.lblSave.Appearance = appearance101;
            resources.ApplyResources(this.lblSave, "lblSave");
            this.lblSave.Name = "lblSave";
            this.lblSave.Tag = "NOCOLOR";
            this.lblSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlClose
            // 
            // 
            // pnlClose.ClientArea
            // 
            this.pnlClose.ClientArea.Controls.Add(this.btnClose);
            this.pnlClose.ClientArea.Controls.Add(this.lblClose);
            resources.ApplyResources(this.pnlClose, "pnlClose");
            this.pnlClose.Name = "pnlClose";
            // 
            // btnClose
            // 
            appearance102.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance102.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance102.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance102;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.Tag = "";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblClose
            // 
            appearance103.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance103.FontData.BoldAsString = resources.GetString("resource.BoldAsString99");
            appearance103.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(appearance103, "appearance103");
            this.lblClose.Appearance = appearance103;
            resources.ApplyResources(this.lblClose, "lblClose");
            this.lblClose.Name = "lblClose";
            this.lblClose.Tag = "NOCOLOR";
            this.lblClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbX12
            // 
            appearance104.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance104.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance104.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance104.BorderColor = System.Drawing.Color.Navy;
            appearance104.BorderColor3DBase = System.Drawing.Color.Navy;
            this.tbX12.ClientAreaAppearance = appearance104;
            this.tbX12.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tbX12.Controls.Add(this.tbFTP);
            this.tbX12.Controls.Add(this.ultraTabPageControl2);
            this.tbX12.Controls.Add(this.ultraTabPageControl1);
            resources.ApplyResources(this.tbX12, "tbX12");
            this.tbX12.MinTabWidth = 80;
            this.tbX12.Name = "tbX12";
            this.tbX12.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tbX12.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPage2003;
            appearance105.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance105.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance105.FontData.BoldAsString = resources.GetString("resource.BoldAsString100");
            appearance105.ForeColor = System.Drawing.Color.White;
            ultraTab1.Appearance = appearance105;
            ultraTab1.TabPage = this.tbFTP;
            resources.ApplyResources(ultraTab1, "ultraTab1");
            ultraTab1.ForceApplyResources = "";
            appearance106.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            appearance106.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance106.FontData.BoldAsString = resources.GetString("resource.BoldAsString101");
            appearance106.ForeColor = System.Drawing.Color.White;
            ultraTab2.Appearance = appearance106;
            ultraTab2.TabPage = this.ultraTabPageControl2;
            resources.ApplyResources(ultraTab2, "ultraTab2");
            ultraTab2.ForceApplyResources = "";
            appearance107.FontData.BoldAsString = resources.GetString("resource.BoldAsString102");
            ultraTab3.Appearance = appearance107;
            ultraTab3.TabPage = this.ultraTabPageControl1;
            resources.ApplyResources(ultraTab3, "ultraTab3");
            ultraTab3.ForceApplyResources = "";
            this.tbX12.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            this.tbX12.TabsPerRow = 2;
            this.tbX12.TabStop = false;
            this.tbX12.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage1
            // 
            resources.ApplyResources(this.ultraTabSharedControlsPage1, "ultraTabSharedControlsPage1");
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            // 
            // ultraLabel7
            // 
            this.ultraLabel7.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.ultraLabel7.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.ultraLabel7, "ultraLabel7");
            this.ultraLabel7.Name = "ultraLabel7";
            // 
            // frmVendor
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTransactionType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbX12);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmVendor";
            this.Activated += new System.EventHandler(this.frmVendor_Activated);
            this.Load += new System.EventHandler(this.frmVendor_Load);
            this.Shown += new System.EventHandler(this.frmVendor_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmVendor_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmVendor_KeyUp);
            this.tbFTP.ResumeLayout(false);
            this.tbFTP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Interchange_Control_Version_No)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboISA_Test_Indicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIEA_Interchange_Control_No)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPURL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_ID_Qualifier_Sender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPO_Type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Interchange_SenderID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Code_Qualifier_SE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApp_Receiver_Code)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_ID_Qualifier_Receiver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFTPPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Interchange_ReceiverID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID_Code_Qualifier_By)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStandardType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtApp_Sender_Code)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIdentification_Code_By)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPERCommNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Interchange_Control_No)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIdentification_Code_SE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProduct_Qualifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAMTAmountQualifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPERCommNumQualifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPerContactFunctionCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPERName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtISA_Acknowledgement_Request)).EndInit();
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboEncryptionType)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboItemType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPriceType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSubElementSep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutboundDir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInboundDir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSegmentSepfix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtElementSepfix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPriceFileFormatfix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAckFileFormatfix)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSalePriceQualifie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboCostQualifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPriceQualifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboVendorList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmailAddr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVendorName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAddress2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWebAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.pnlVendorNote.ClientArea.ResumeLayout(false);
            this.pnlVendorNote.ResumeLayout(false);
            this.pnlSave.ClientArea.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.pnlClose.ClientArea.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbX12)).EndInit();
            this.tbX12.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		
		private void txtBoxs_Validate(object sender, System.EventArgs e)
		{
			try
			{
				if (oVendorRow == null) 
					return ;
				
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor =  (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
			
                switch(txtEditor.Name)
				{
					case "txtVendorCode":
						oVendorRow.Vendorcode = txtVendorCode.Text;
						break;
					case "txtVendorName":
						oVendorRow.Vendorname = txtVendorName.Text;
						break;
					case "txtWebAddress":
						oVendorRow.Url = txtWebAddress.Text;
						break;
					case "txtAddress1":
						oVendorRow.Address1 = txtAddress1.Text;
						break;
					case "txtAddress2":
						oVendorRow.Address2 = txtAddress2.Text;
						break;
					case "txtCellNo":
						oVendorRow.Cellno = txtCellNo.Text;
						break;
					case "txtCity":
						oVendorRow.City = txtCity.Text;
						break;
                    case "txtEmailAddr":
                        {
                            string strRegularexp = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                            Regex rex = new Regex(strRegularexp);
                            if (rex.IsMatch(txtEmailAddr.Text))
                            {
                                oVendorRow.Email = txtEmailAddr.Text;
                            }
                            else
                            {
                                clsUIHelper.ShowErrorMsg("E-Mail Address is not Valid.");
                                txtEmailAddr.Focus();
                            }
                        }
                        
                        break;
					case "txtState":
						oVendorRow.State = txtState.Text;
						break;                   
                }
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}
		private void MaskEditBoxes_Validate(object sender, System.EventArgs e)
		{
			try
			{
				if (oVendorRow == null) 
					return ;
				Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit txtEditor =  (Infragistics.Win.UltraWinMaskedEdit.UltraMaskedEdit)sender;
				switch(txtEditor.Name)
				{
					case "txtZipCode":
						oVendorRow.Zip =  txtZipCode.Text;
						break;
					case "txtFaxNo":
						oVendorRow.Faxno = txtFaxNo.Text;
						break;
					case "txtPhoneOff":
						oVendorRow.Telephoneno= this.txtPhoneOff.Text;
						break;
                    case "maskedEditTimeToOrder":
                        oVendorRow.TimeToOrder = this.maskedEditTimeToOrder.Text;
                        break;
				}
			}
			catch(Exception exp)
			{
                clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmVendor_Load(object sender, System.EventArgs e)
		{
			this.txtAddress1.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtAddress1.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtAddress2.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtAddress2.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            //this.txtCellNo.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            //this.txtCellNo.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtCity.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtCity.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtVendorCode.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtVendorCode.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtVendorName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtVendorName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtEmailAddr.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtEmailAddr.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtFaxNo.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtFaxNo.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtPhoneOff.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtPhoneOff.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtState.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtState.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtWebAddress.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtWebAddress.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

			this.txtZipCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
			this.txtZipCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            #region
            //
//			this.txtFTPURL.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtFTPURL.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtFTPLogin.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtFTPLogin.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtFTPPassword.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtFTPPassword.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtFTPPort.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtFTPPort.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtISA_ID_Qualifier_Sender.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtISA_ID_Qualifier_Sender.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtISA_ID_Qualifier_Receiver.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtISA_ID_Qualifier_Receiver.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtISA_Interchange_SenderID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtISA_Interchange_SenderID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtISA_Interchange_ReceiverID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtISA_Interchange_ReceiverID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtIEA_Interchange_Control_No.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtIEA_Interchange_Control_No.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.cboISA_Test_Indicator.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.cboISA_Test_Indicator.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtApp_Sender_Code.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtApp_Sender_Code.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtApp_Receiver_Code.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtApp_Receiver_Code.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtPO_Type.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtPO_Type.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtID_Code_Qualifier_By.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtID_Code_Qualifier_By.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtIdentification_Code_By.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtIdentification_Code_By.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtID_Code_Qualifier_SE.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtID_Code_Qualifier_SE.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtIdentification_Code_SE.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtIdentification_Code_SE.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtProduct_Qualifier.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtProduct_Qualifier.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtISA_Interchange_Control_No.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtISA_Interchange_Control_No.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//			
//			this.txtPERCommNumQualifier.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtPERCommNumQualifier.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtPerContactFunctionCode.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtPerContactFunctionCode.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtPERName.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtPERName.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtPERCommNum.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtPERCommNum.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtAMTAmountQualifier.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtAMTAmountQualifier.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
//
//			this.txtVersion.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
//			this.txtVersion.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            #endregion

            cboVendorList.SelectionChangeCommitted += new EventHandler(cboVendorList_SelectionChangeCommitted);
            cboPriceQualifier.SelectionChangeCommitted += new EventHandler(cboPriceQualifier_SelectionChangeCommitted);
            cboCostQualifier.SelectionChangeCommitted += new EventHandler(cboCostQualifier_SelectionChangeCommitted);
            cboSalePriceQualifie.SelectionChangeCommitted += new EventHandler(cboSalePriceQualifie_SelectionChangeCommitted);

            foreach (Control ctrl in this.tbX12.Tabs[0].TabPage.Controls)
			{
				if(ctrl.GetType() ==typeof( Infragistics.Win.UltraWinEditors.UltraTextEditor))
				{
					Infragistics.Win.UltraWinEditors.UltraTextEditor oTxt= (Infragistics.Win.UltraWinEditors.UltraTextEditor)ctrl;
					oTxt.Enter+=  new System.EventHandler(clsUIHelper.AfterEnterEditMode);
					oTxt.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
                }
			}

			foreach (Control ctrl in this.tbX12.Tabs[1].TabPage.Controls)
			{
				if (ctrl.GetType() ==typeof( Infragistics.Win.UltraWinEditors.UltraTextEditor))
				{
					Infragistics.Win.UltraWinEditors.UltraTextEditor oTxt= (Infragistics.Win.UltraWinEditors.UltraTextEditor)ctrl;
					oTxt.Enter+=  new System.EventHandler(clsUIHelper.AfterEnterEditMode);
					oTxt.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
				}
			}

			foreach (Control ctrl in this.tbX12.Tabs[2].TabPage.Controls)
			{
				if (ctrl.GetType() ==typeof( Infragistics.Win.UltraWinEditors.UltraTextEditor))
				{
					Infragistics.Win.UltraWinEditors.UltraTextEditor oTxt= (Infragistics.Win.UltraWinEditors.UltraTextEditor)ctrl;
					oTxt.Enter+=  new System.EventHandler(clsUIHelper.AfterEnterEditMode);
					oTxt.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
				}
			}
			this.StartPosition = FormStartPosition.CenterScreen;
			IsCanceled = true;
			clsUIHelper.setColorSchecme(this);
            btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            //this.cboVendorList.Enabled = true;
        }

        void cboSalePriceQualifie_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string priceQualifier = (string)cboSalePriceQualifie.SelectedItem.DisplayText;
            oVendorRow.SalePriceQualifier = priceQualifier;
        }

        void cboPriceQualifier_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string priceQualifier = (string)cboPriceQualifier.SelectedItem.DisplayText;
            oVendorRow.PriceQualifier = priceQualifier;
        }

        void cboVendorList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string selectedvalue = (string)cboVendorList.SelectedItem.DataValue;

                if (dict.ContainsKey(selectedvalue) || selectedvalue == clsPOSDBConstants.NONE)
                {
                    if (selectedvalue == clsPOSDBConstants.NONE)
                    {
                        oVendorRow.PrimePOVendorCode = string.Empty;
                        chkIsEPO.Checked = false;
                    }
                    else
                    {
                        oVendorRow.PrimePOVendorCode = dict[selectedvalue].ToString();
                        chkIsEPO.Checked = true;
                        SetPriceQualifier();
                        SetCostQualifier();
                    }
                }
                else
                {
                    chkIsEPO.Checked = false;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", ""); 
            } 
        }        
        
		private bool Save()
		{
            string vendorName = string.Empty; 
			try
			{				    
                oVendorRow.Vendorcode = txtVendorCode.Text;
                if (cboPriceQualifier.SelectedItem != null)
                {
                    oVendorRow.PriceQualifier = cboPriceQualifier.SelectedItem.DisplayText;
                }
                if (cboCostQualifier.SelectedItem != null)
                {
                    oVendorRow.CostQualifier = cboCostQualifier.SelectedItem.DisplayText;
                }
                if (cboPriceQualifier.SelectedItem != null)
                {
                    oVendorRow.SalePriceQualifier = cboSalePriceQualifie.SelectedItem.DisplayText;
                }
               

                if(chkUpdatePrice.CheckState == CheckState.Unchecked)
                    oVendorRow.UpdatePrice = false;
                else
                    oVendorRow.UpdatePrice = true;

                if (cboVendorList.SelectedItem != null)
                {
                    vendorName = cboVendorList.SelectedItem.DisplayText;
                }
              
                if(chkIsEPO.Checked==true)
				{
				 	oVendorRow.USEVICForEPO = true;
				}
				else
				{
					oVendorRow.USEVICForEPO=false;
				}

                if (chkSendVendorCostPrice.Checked == true)
                {
                    oVendorRow.SendVendCostPrice = true;
                }
                else
                {
                    oVendorRow.SendVendCostPrice = false;
                }


                //Added by Atul Joshi on 29-10-2010
                if (chkprocess810.Checked == true)
                {
                    oVendorRow.Process810 = true;
                }
                else
                {
                    oVendorRow.Process810 = false;
                }
                //AckPriceUpdate Added by Ravindra on 20 Feb 2013
                if (chkAckPriceUpdate.Checked == true)
                {
                    oVendorRow.AckPriceUpdate = true;
                }
                else
                {
                    oVendorRow.AckPriceUpdate = false;
                }
                #region 12-Nov-2014 JY added new field IsSalePriceUpdate
                if (chkSalePriceUpdate.Checked == true)
                {
                    oVendorRow.SalePriceUpdate = true;
                }
                else
                {
                    oVendorRow.SalePriceUpdate = false;
                }
                #endregion
                #region Sprint-21 - 2208 24-Jul-2015 JY Added
                if (chkReduceSPWithPriceUpd.Checked == true)
                {
                    oVendorRow.ReduceSellingPrice = true;
                }
                else
                {
                    oVendorRow.ReduceSellingPrice = false;
                }
                #endregion

                oBRVendor.Persist(oVendData);
				SetNew();
				txtVendorCode.Text = String.Empty;
				this.txtVendorCode.Focus();
				return true;

			}
         catch(POSExceptions exp)
			{
			    clsUIHelper.ShowErrorMsg(exp.ErrMessage);
				switch (exp.ErrNumber)
				{
					case (long)POSErrorENUM.Vendor_DuplicateCode:
						txtVendorCode.Focus();
						break;
					case (long)POSErrorENUM.Vendor_NameCanNotBeNULL:
						txtVendorName.Focus();
						break;
					case (long)POSErrorENUM.Vendor_PrimaryKeyVoilation:
						txtVendorCode.Focus();
						break;
					case (long)POSErrorENUM.Vendor_Address1CanNotBeNULL:
						txtAddress1.Focus();
						break;
					case (long)POSErrorENUM.Vendor_CityCanNotBeNull:
						txtCity.Focus();
						break;
					case (long)POSErrorENUM.Vendor_StateCannotBeNull:
						txtState.Focus();
						break;
				}
				return false;
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

		private void Search()
		{
		  try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
					Edit(strCode);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void SearchItem()
		{
			try
			{
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Vendor_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Vendor_tbl; //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
				if (!oSearch.IsCanceled)
				{
					string strCode=oSearch.SelectedRowID();
					if (strCode == "") 
						return;
					
					Edit(strCode);
				}
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void txtVendorCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			Search();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			Search();
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			try
			{
				txtVendorCode.Text = String.Empty;
				SetNew();
				this.txtVendorCode.Focus();
                
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			IsCanceled = true;
			this.Close();
		}

		private void chkIs_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (oVendorRow == null)
					return;
				oVendorRow.IsActive = chkIsActive.Checked;
                oVendorRow.IsAutoClose = chkIsAutoClose.Checked;
                oVendorRow.Process810 = chkprocess810.Checked;
                oVendorRow.AckPriceUpdate = chkAckPriceUpdate.Checked;
                oVendorRow.SalePriceUpdate = chkSalePriceUpdate.Checked;    //12-Nov-2014 JY added new field IsSalePriceUpdate
                oVendorRow.ReduceSellingPrice = chkReduceSPWithPriceUpd.Checked;    //Sprint-21 - 2208 24-Jul-2015 JY Added
			}
			catch(POSExceptions exp)
			{
				clsUIHelper.ShowErrorMsg(exp.ErrMessage);
			}
		}

		private void frmVendor_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyData==System.Windows.Forms.Keys.F4)
				{
					if ( txtVendorCode.ContainsFocus)
						this.Search();
				}
				else if (e.KeyCode==System.Windows.Forms.Keys.D1 && e.Control==true)
				{
					this.tbX12.Focus();
					this.tbX12.ActiveTab=this.tbX12.Tabs[0];
					this.tbX12.Tabs[0].EnsureTabInView();
				}
				else if (e.KeyCode==System.Windows.Forms.Keys.D2 && e.Control==true)
				{
					this.tbX12.ActiveTab=this.tbX12.Tabs[1];
					this.tbX12.Focus();
					this.tbX12.Tabs[1].EnsureTabInView();
				}
                //Added By Shitaljit(QuicSolv) 0n 11 oct 2011 
                else if (e.KeyData == Keys.F6 && pnlVendorNote.Visible == true)
                {
                    this.btnVendorNote_Click(null,null);
                }
                //End of Added By Shitaljit(QuicSolv) 0n 11 oct 2011 
			}
			catch(Exception exp)
			{
				clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}

		private void frmVendor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private void frmVendor_Activated(object sender, System.EventArgs e)
		{
			clsUIHelper.CurrentForm=this;
		}

		private void ultraLabel14_Click(object sender, System.EventArgs e)
		{
		
		}
		
		private void EnableDisableFTP(bool enable)
		{
			this.txtFTPURL.Enabled=enable;
			this.txtFTPLogin.Enabled=enable;
			this.txtFTPPassword.Enabled=enable;
			this.txtFTPPort.Enabled=enable;
			this.txtISA_ID_Qualifier_Sender.Enabled=enable;
			this.txtISA_ID_Qualifier_Receiver.Enabled=enable;
			this.txtISA_Interchange_SenderID.Enabled=enable;
			this.txtISA_Interchange_ReceiverID.Enabled=enable;
			this.txtIEA_Interchange_Control_No.Enabled=enable;
			this.cboISA_Test_Indicator.Enabled=enable;
			this.txtApp_Sender_Code.Enabled=enable;
			this.txtApp_Receiver_Code.Enabled=enable;
			this.txtPO_Type.Enabled=enable;
			this.txtID_Code_Qualifier_By.Enabled=enable;
			this.txtIdentification_Code_By.Enabled=enable;
			this.txtID_Code_Qualifier_SE.Enabled=enable;
			this.txtIdentification_Code_SE.Enabled=enable;
			this.txtProduct_Qualifier.Enabled=enable;
			this.txtISA_Interchange_Control_No.Enabled=enable;
			this.txtISA_Acknowledgement_Request.Enabled=enable;
			this.txtVersion.Enabled=enable;

			this.txtPERCommNum.Enabled=enable;
			this.txtPERCommNumQualifier.Enabled=enable;
			this.txtPerContactFunctionCode.Enabled=enable;
			this.txtPERName.Enabled=enable;
			this.cboStandardType.Enabled=enable;
			this.txtAMTAmountQualifier.Enabled=enable;

			this.txtAckFileFormatfix.Enabled=enable;
			this.txtPriceFileFormatfix.Enabled=enable;
			this.txtElementSepfix.Enabled=enable;
			this.txtSegmentSepfix.Enabled=enable;

			this.txtInboundDir.Enabled=enable;
			this.txtOutboundDir.Enabled=enable;

			this.txtSubElementSep.Enabled=enable;
		}
	
        private void CmbVendorList_Load()
        {
            DataTable dataTable = new DataTable();
            DataColumn oCol1 = dataTable.Columns.Add(clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode);
            DataColumn oCol2 = dataTable.Columns.Add(clsPOSDBConstants.Vendor_Fld_VendorName);
            try
            {
                dict = new MMS.PROCESSOR.MMSDictionary<string, string>();
                dict = Resources.PrimePOUtil.DefaultInstance.GetVendors();

                if (dict != null)
                {
                    MMS.PROCESSOR.MMSDictionary<string, string>.KeyCollection keyColl = dict.Keys;
                    cboVendorList.Items.Clear();
                    cboVendorList.Enabled = true;
                    cboVendorList.Items.Add(clsPOSDBConstants.NONE, clsPOSDBConstants.NONE);
                    cboVendorList.SelectedIndex = 0;

                    int count = 1;
                    foreach (string key in keyColl)
                    {
                        cboVendorList.Items.Add(key);
                        DataRow row = dataTable.NewRow();
                        row[clsPOSDBConstants.Vendor_Fld_VendorName] = key;
                        row[clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode] = dict[key];
                        dataTable.Rows.Add(row);
                        count++;
                    }
                   datasetVendorID.Tables.Add(dataTable);
                }                
            }
            catch(POSExceptions ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            catch(OtherExceptions ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }                
            catch(Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }

        private void cboCostQualifier_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string costQualifier = (string)cboCostQualifier.SelectedItem.DisplayText;
                oVendorRow.CostQualifier = costQualifier;
            }
            catch (POSExceptions ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString()); 
            }
            catch(Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }

        private void cboVendorList_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void lblCostQualifier_Click(object sender, EventArgs e)
        {

        }
       
        #region Set Default Cost Qualifiers
        private void SetSaleRiceQualifier()
        {
            try
            {
                if (oVendorRow.SalePriceQualifier != null && oVendorRow.SalePriceQualifier.Trim() != "")
                {
                    foreach (ValueListItem item in cboSalePriceQualifie.Items)
                    {
                        if (item.DisplayText.Trim() == oVendorRow.SalePriceQualifier.Trim())
                        {
                            cboSalePriceQualifie.SelectedItem = item;
                        }
                    }
                }
            }
            catch (Exception)
            {
               
            }
            
        }
        /// <summary>
        /// Added By Shitaljit to set vendor Cost Qualifier to default value. JIRA-586
        /// </summary>
        private void SetCostQualifier()
        {
            switch (oVendorRow.PrimePOVendorCode.ToUpper().Trim())
            {
                case "HDSMITH":
                case "KINRAY":
                case "NCMUTUAL":
                case "RDC":
                case "MCKESSON":
                case "BELLCO":
                case "SMITH":
                case "ANDA":
                case "VALUEDRUGCO":
                    this.cboCostQualifier.Text = "NET";
                    oVendorRow.CostQualifier = "NET";
                    break;
                case "CARDINAL":
                    this.cboCostQualifier.Text = "INV"; //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY replaced "MSR" by "INV" 
                    oVendorRow.CostQualifier = "INV";   //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY replaced "MSR" by "INV" 
                    break;
                case "AMERISOURCEBERGER":
                    this.cboCostQualifier.Text = "CON"; //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY replaced "RTL" by "CON" 
                    oVendorRow.CostQualifier = "CON";   //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY replaced "RTL" by "CON" 
                    break;
            }
        }


        /// <summary>
        /// Added By Shitaljit to set vendor Price Qualifier to default value. JIRA-586
        /// </summary>
        private void SetPriceQualifier()
        {
            switch (oVendorRow.PrimePOVendorCode.ToUpper().Trim())
            {
                case "HDSMITH":
                //case "CARDINAL":  //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY Commented
                case "RDC":
                case "SMITH":
                case "ANDA":
                case "VALUEDRUGCO":
                    this.cboPriceQualifier.Text = "RTL";
                    oVendorRow.PriceQualifier = "RTL";
                    break;
                
                //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY Added "MSR" for cardinal, previously it was "RTL"
                case "CARDINAL":
                    this.cboPriceQualifier.Text = "MSR";
                    oVendorRow.PriceQualifier = "MSR";
                    break;

                case "KINRAY":
                case "BELLCO":
                    this.cboPriceQualifier.Text = "RES";
                    oVendorRow.PriceQualifier = "RES";
                    break;

                case "AMERISOURCEBERGER":
                    this.cboPriceQualifier.Text = "RTL";    //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY replaced "MSR" by "RTL" 
                    oVendorRow.PriceQualifier = "RTL";  //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY replaced "MSR" by "RTL" 
                    break;

                case "MCKESSON":
                    this.cboPriceQualifier.Text = "RESM";
                    oVendorRow.PriceQualifier = "RESM";
                    break;

                case "NCMUTUAL":
                    this.cboPriceQualifier.Text = "DAP";
                    oVendorRow.PriceQualifier = "DAP";
                    break;
            }
        }
        #endregion
        #region Price Qualifier Change..
        /// <summary>
        /// Added By Shitaljit to warn user if they are changing the vendor Cost Qualifier from default value.
        /// PRIMEPOS-JIRA-586
        /// </summary>

        private void WarnForCostQualifierChange()
        {
            switch (oVendorRow.PrimePOVendorCode.ToUpper().Trim())
            {
                case "HDSMITH":
                case "KINRAY":
                case "NCMUTUAL":
                case "RDC":
                case "MCKESSON":
                case "BELLCO":
                case "SMITH":
                case "ANDA":
                case "VALUEDRUGCO":
                    if (cboCostQualifier.Text.Trim().Equals("NET") == false)
                    {
                        if (Resources.Message.Display("The default Cost Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'NET'.\n" + "Do you want to change it to '" + cboCostQualifier.Text.Trim() + "'?", "Cost Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            this.cboCostQualifier.Text = "NET";
                            oVendorRow.CostQualifier = "NET";
                            bSave = true;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                        }
                    }
                    break;
                case "CARDINAL":
                    if (cboCostQualifier.Text.Trim().Equals("INV") == false)
                    {
                        if (Resources.Message.Display("The default Cost Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'INV'.\n" + "Do you want to change it to '" + cboCostQualifier.Text.Trim() + "'?", "Cost Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            this.cboCostQualifier.Text = "INV";
                            oVendorRow.CostQualifier = "INV";
                            bSave = true;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                        }
                    }
                    break;
                case "AMERISOURCEBERGER":
                    if (cboCostQualifier.Text.Trim().Equals("CON") == false)
                    {
                        if (Resources.Message.Display("The default Cost Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'CON'.\n" + "Do you want to change it to '" + cboCostQualifier.Text.Trim() + "'?", "Cost Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            this.cboCostQualifier.Text = "CON";
                            oVendorRow.CostQualifier = "CON";
                            bSave = true;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Added By Shitaljit to warn user if they are changing the vendor Price Qualifier from default value.
        /// PRIMEPOS-JIRA-586
        /// </summary>
        private void WarnForPriceQualifierChange()
        {
            switch (oVendorRow.PrimePOVendorCode.ToUpper().Trim())
            {
                case "HDSMITH":
                //case "CARDINAL":  //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY Commented 
                case "RDC":
                case "SMITH":
                case "ANDA":
                case "VALUEDRUGCO":
                case "AMERISOURCEBERGER":   //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY Added as changed qualifier from "MSR" to "RTL"
                    if (cboPriceQualifier.Text.Trim().Equals("RTL") == false)
                    {
                        if (Resources.Message.Display("The default Price Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'RTL'.\n" + "Do you want to change it to '" + cboPriceQualifier.Text.Trim() + "'?", "Price Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            this.cboPriceQualifier.Text = "RTL";
                            oVendorRow.PriceQualifier = "RTL";
                            bSave = true;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                        }
                    }
                    break;

                //Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY Added 
                case "CARDINAL":
                    if (cboPriceQualifier.Text.Trim().Equals("MSR") == false)
                    {
                        if (Resources.Message.Display("The default Price Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'MSR'.\n" + "Do you want to change it to '" + cboPriceQualifier.Text.Trim() + "'?", "Price Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            this.cboPriceQualifier.Text = "MSR";
                            oVendorRow.PriceQualifier = "MSR";
                            bSave = true;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                        }
                    }
                    break;

                case "KINRAY":
                case "BELLCO":

                    if (cboPriceQualifier.Text.Trim().Equals("RES") == false)
                    {
                        if (Resources.Message.Display("The default Price Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'RES'.\n" + "Do you want to change it to '" + cboPriceQualifier.Text.Trim() + "'?", "Price Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            this.cboPriceQualifier.Text = "RES";
                            oVendorRow.PriceQualifier = "RES";
                            bSave = true;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                        }
                    }
                    break;
                #region Sprint-22 - PRIMEPOS-2249 03-Dec-2015 JY Commented as replaced "MSR" by "RTL"
                //case "AMERISOURCEBERGER":
                //    if (cboPriceQualifier.Text.Trim().Equals("MSR") == false)   
                //    {
                //        if (Resources.Message.Display("The default Price Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'MSR'\n." + "Do you want to change it to '" + cboPriceQualifier.Text.Trim() + "'?", "Price Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                //        {
                //            this.cboPriceQualifier.Text = "MSR";
                //            oVendorRow.PriceQualifier = "MSR";
                //        }
                //    }
                //    break;
                #endregion

                case "MCKESSON":
                    if (cboPriceQualifier.Text.Trim().Equals("RESM") == false)
                    {
                        if (Resources.Message.Display("The default Price Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'RESM'.\n" + "Do you want to change it to '" + cboPriceQualifier.Text.Trim() + "'?", "Price Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            this.cboPriceQualifier.Text = "RESM";
                            oVendorRow.PriceQualifier = "RESM";
                            bSave = true;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                        }
                    }
                    break;

                case "NCMUTUAL":
                    if (cboPriceQualifier.Text.Trim().Equals("DAP") == false)
                    {
                        if (Resources.Message.Display("The default Price Qualifier for " + oVendorRow.Vendorcode.Trim() + " is 'DAP'.\n" + "Do you want to change it to '" + cboPriceQualifier.Text.Trim() + "'?", "Price Qualifier", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            this.cboPriceQualifier.Text = "DAP";
                            oVendorRow.PriceQualifier = "DAP";
                            bSave = true;  //Sprint-22 - PRIMEPOS-2249 04-Dec-2015 JY Added 
                        }
                    }
                    break;
            }
        }
        #endregion  
        private void frmVendor_Shown(object sender, EventArgs e)
        {
            if (txtVendorCode.Enabled == true)
            {
                txtVendorCode.Focus();
            }
        }
        //Added By Shitaljit(QuicSolv) 0n 11 oct 2011 
        private void btnVendorNote_Click(object sender, EventArgs e)
        {
            frmCustomerNotes oFrmCustNotes = new frmCustomerNotes(this.oVendorRow.VendorId.ToString(), clsPOSDBConstants.Vendor_tbl, clsEntityType.VendorNote);
            oFrmCustNotes.ShowDialog();
        }

        private void cboCostQualifier_SelectionChanged(object sender, EventArgs e)
        {
            if (this.cboVendorList.Enabled == true && isEdit == true)
            {
                WarnForCostQualifierChange();
            }
        }
        private void cboPriceQualifier_SelectionChanged(object sender, EventArgs e)
        {
            if (this.cboVendorList.Enabled == true && isEdit == true)
            {
                WarnForPriceQualifierChange();
            }
        }

        private void cboSalePriceQualifie_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //oVendorRow.SalePriceQualifier = cboSalePriceQualifie.Text;
            }
            catch (Exception)
            {
            }
        }

       
        //END of Added By Shitaljit(QuicSolv) 0n 11 oct 2011                    
	}
}
