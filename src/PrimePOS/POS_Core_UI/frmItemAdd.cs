using System;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Win.CalcEngine;
using Infragistics.Win.UltraWinCalcManager;
//using POS_Core.DataAccess;
using System.Data;
using System.Reflection;
using POS_Core.UserManagement;
using System.Threading;
using PharmData;
using POS_Core.DataAccess;
using POS_Core.Resources;
//using POS_Core_UI.Reports.ReportsUI;

namespace POS_Core_UI
{
    public partial class frmItemAdd : Form
    {
        string name;
        string itemid;
        Decimal Amount;
        public bool bttnState = false;
        private MMSSearch.Service objService = new MMSSearch.Service(); //PRIMEPOS-2671 19-Apr-2019 JY Added

        public frmItemAdd()
        {
            InitializeComponent();
        }
        public frmItemAdd(string ID, string sAmount)
        {
            InitializeComponent();
            this.ItemID = ID;
            Amount = Configuration.convertNullToDecimal(sAmount);
        }
        #region Properties
        public string btnName
        {
            get { return name; }
            set { name = value; }
        }

        public string ItemID
        {
            get { return itemid; }
            set { itemid = value; }
        }
        #endregion


        //public void DoAction(string btnAction)
        //{
        //    switch (btnAction)
        //    {
        //        case "SimpleMode":   //clsUIHelper.ShowInfoMsg("Simple Mode Selected");
        //                             this.Close();
        //                             break;
                               
        //        case "QuickMode":    //clsUIHelper.ShowInfoMsg("Quick Mode Selected");
        //                             this.Close();
        //                             break;
                 
        //        case "AdvancedMode": clsUIHelper.ShowInfoMsg("Advanced Mode Selected");
        //                             this.Close();
        //                             break;


        //    }
        //}

        private void frmItemAdd_Load(object sender, EventArgs e)
        {
            btnSimpleMode.Focus();
            //Added by Shitaljit(QuicSolv) on May 2011
            if (bttnState)
            {
                btnSimpleMode.Enabled = false;  //PRIMEPOS-2488 22-Jun-2020 JY Added
                btnQuickMode.Enabled = false;
                btnAdvanceMode.Enabled = false;
                btnMMSSearch.Enabled = false;   //PRIMEPOS-2488 22-Jun-2020 JY Added
            }
            //Till here Added By Shitaljit(QuicSolv)
            lblItemId.Text = "Opps! Item#  " + ItemID + "  - Not Found in the System ";
            //lblItemAdd.Text= "Do you wish to Add this Item?";
            //lblItemMode.Text= "Please Select one of the options to Add this Item or Select Cancel if you do not wish to add this item";
            //Added By shitaljit for JIRA-385 on !6May 2013
            lblItemMode.Text = "To add this item number to the database, please use one of the options at the bottom of this screen.";
            if (Amount > 0)
            {
                lblItemMode.Text += "\nIf you were trying to ring up amount "+Configuration.CInfo.CurrencySymbol+ Amount+", please press \"T\" to ring up as default Taxable Item or \"N\" for default Non-Taxable item.";
            }
            //End Of Added By Shitaljit
            clsUIHelper.setColorSchecme(this);
            btnSimpleMode.Select();
        }


        private void btnSimpleMode_Click(object sender, EventArgs e)
        {
            btnName = "SimpleMode";
            this.Close();
            //DoAction(btnName);
        }

        private void btnQuickMode_Click(object sender, EventArgs e)
        {
            btnName = "QuickMode";
            this.Close();
            //DoAction(btnName);
        }

        private void btnAdvanceMode_Click(object sender, EventArgs e)
        {
            btnName = "AdvancedMode";
            this.Close();
            //DoAction(btnName);
        }

        #region PRIMEPOS-2671 24-Apr-2019 JY Added
        private void btnMMSSearch_Click(object sender, EventArgs e)
        {
            btnName = "MMSSearch";
            this.Close();
        }
        #endregion

        private void frmItemAdd_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            { 
                case Keys.S:
                    btnSimpleMode_Click(null, null);
                    break;
                case Keys.Q:
                    btnQuickMode_Click(null, null);
                    break;
                case Keys.A:
                    btnAdvanceMode_Click(null, null);
                    break;
                case Keys.Escape:
                    btnName = "Escape";
                    this.Close();
                    break;
                //Added By shitaljit for JIRA-385 on !6May 2013
                case Keys.T:
                    if (string.IsNullOrEmpty(Configuration.CInfo.DefaultTaxableItem) == false)
                    {
                        ItemID = Configuration.CInfo.DefaultTaxableItem;
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg("Default Taxable item is not defined in preference, please define the item.");
                    }
                     this.Close();
                    break;
                case Keys.N:
                    if (string.IsNullOrEmpty(Configuration.CInfo.DefaultNonTaxableItem) == false)
                    {
                        ItemID = Configuration.CInfo.DefaultNonTaxableItem;
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg("Default Non-Taxable item is not defined in preference, please define the item.");
                    }
                    this.Close();
                    break;
                //End 
                #region PRIMEPOS-2671 24-Apr-2019 JY Added
                case Keys.M:
                    btnMMSSearch_Click(null, null);
                    break;
                default:
                    break;
                    #endregion
            }
        }

        private void lblItemId_Click(object sender, EventArgs e)
        {

        }

        private void btnSimpleMode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && btnSimpleMode.ContainsFocus == true)
            {
                btnSimpleMode_Click(null, null);
            }
            else
            {
                frmItemAdd_KeyUp(null, e);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnName = "Escape";
            this.Close();
        }        
    }
}