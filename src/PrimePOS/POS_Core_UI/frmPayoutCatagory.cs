using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmPayoutCatagory : Form
    {
        #region Declaration
        public bool IsCanceled = false;
        private PayOutCatData oPayOutCatData = new PayOutCatData();
        private PayOutCatRow oPayOutCatRow;
        private PayOutCat oPayOutCat = new PayOutCat();
        #endregion
        bool GridVisibleFlag = false;
        public frmPayoutCatagory()
        {
            InitializeComponent();
            clsUIHelper.setColorSchecme(this);
        }
        private bool IsEditing = false;
        private bool Save()
        {
            try
            {
                if (this.txtName.Text.Trim() == "")
                {
                    this.txtName.Focus();
                    throw (new Exception("Please type Payout category Name."));
                }
                if (IsEditing)
                {
                    oPayOutCatRow.PayoutCatType = this.txtName.Text.Trim();
                    oPayOutCatRow.UserId = Configuration.UserName;
                    oPayOutCatRow.DefaultDescription = this.txtDefaultDescription.Text.Trim();
                    //DataRowState st = oPayOutCatRow.RowState;
                    oPayOutCat.Persist(oPayOutCatData);
                    //return true;
                }
                if (oPayOutCatRow.ID == 0)
                {
                    PayOutCatData oPayOutCatDatatemp = new PayOutCatData();
                    PayOutCatSvr oPayOutCatSvrtemp = new PayOutCatSvr();
                    oPayOutCatDatatemp = oPayOutCatSvrtemp.PopulateList("where " + clsPOSDBConstants.PayOutCat_Fld_PayoutType + " = '" + oPayOutCatRow.PayoutCatType + "' And " + clsPOSDBConstants.PayOutCat_Fld_UserId + " = '" + oPayOutCatRow.UserId + "'");
                    if (oPayOutCatDatatemp.PayOutCat.Rows.Count > 0 && !IsEditing)
                    {
                        clsUIHelper.ShowErrorMsg("Pay Out Catagory with same Name already Exist");
                        return false;
                    }
                    else if (oPayOutCatDatatemp.PayOutCat.Rows.Count > 0)
                    {
                        oPayOutCatRow.ID = Configuration.convertNullToInt(oPayOutCatDatatemp.PayOutCat[0][clsPOSDBConstants.payoutCat_Fld_Id].ToString());
                    }
                    else if (!IsEditing)
                    {
                        oPayOutCatRow.PayoutCatType = this.txtName.Text.Trim();
                        oPayOutCatRow.UserId = Configuration.UserName;
                        oPayOutCatRow.DefaultDescription = this.txtDefaultDescription.Text.Trim();
                        //DataRowState st = oPayOutCatRow.RowState;
                        oPayOutCat.Persist(oPayOutCatData);                        
                        oPayOutCatDatatemp = oPayOutCatSvrtemp.PopulateList("where " + clsPOSDBConstants.PayOutCat_Fld_PayoutType + " = '" + oPayOutCatRow.PayoutCatType + "' And " + clsPOSDBConstants.PayOutCat_Fld_UserId + " = '" + oPayOutCatRow.UserId + "'");
                        oPayOutCatRow.ID = Configuration.convertNullToInt(oPayOutCatDatatemp.PayOutCat[0][clsPOSDBConstants.payoutCat_Fld_Id].ToString());
                        //return true;
                    }
                }
                List<string> userList = GetSelectedUser();
                Util_UserOptionDetailRightsData oUtil_UserOptionDetailRightsData;
                //Util_UserOptionDetailRightsRow oUtil_UserOptionDetailRightsRow;
                DataRow oUtil_UserOptionDetailRightsRow;
                Util_UserOptionDetailRights oUtil_UserOptionDetailRights;
                Util_UserOptionDetailRightsSvr oUtil_UserOptionDetailRightsSvr = new Util_UserOptionDetailRightsSvr();
                //Util_UserOptionDetailRightsSvr 
                

                oUtil_UserOptionDetailRightsData = oUtil_UserOptionDetailRightsSvr.Populate(" where " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId + " = " + oPayOutCatRow.ID);
                if (oUtil_UserOptionDetailRightsData.Tables[0] != null && oUtil_UserOptionDetailRightsData.Tables[0].Rows.Count > 0 && IsEditing)
                {
                    foreach (DataRow item in oUtil_UserOptionDetailRightsData.Tables[0].Rows)
                    {
                        oUtil_UserOptionDetailRightsSvr.DeleteRow(Configuration.convertNullToInt64(item[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID].ToString()));
                    }
                }
                if (userList != null && userList.Count > 0)
                {

                    foreach (string UserID in userList)
                    {


                        oUtil_UserOptionDetailRightsData = new Util_UserOptionDetailRightsData();
                        oUtil_UserOptionDetailRights = new Util_UserOptionDetailRights();


                        oUtil_UserOptionDetailRightsData = new Util_UserOptionDetailRightsData();
                        oUtil_UserOptionDetailRightsRow = oUtil_UserOptionDetailRightsData.Util_UserOptionDetailRights.AddRow(0, "", 0, 0, 0, false, 0);
                        //oUtil_UserOptionDetailRightsRow = oUtil_UserOptionDetailRightsData._Util_UserOptionDetailRightsT.AddRow(0, "",0);
                        oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID] = UserID;
                        oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId] = oPayOutCatRow.ID;
                        oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID] = UserPriviliges.Screens.Payout.ID;
                        oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID] = UserPriviliges.Modules.POSTransaction.ID;
                        oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId] = UserPriviliges.Permissions.EditPayout.ID;
                        oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed] = true;

                        //oUtil_UserOptionDetailRightsData=oUtil_UserOptionDetailRights.po
                        oUtil_UserOptionDetailRights.Persist(oUtil_UserOptionDetailRightsData);


                    }
                }
                else
                {
                    oUtil_UserOptionDetailRightsData = new Util_UserOptionDetailRightsData();
                    oUtil_UserOptionDetailRights = new Util_UserOptionDetailRights();
                    oUtil_UserOptionDetailRightsRow = oUtil_UserOptionDetailRightsData.Util_UserOptionDetailRights.AddRow(0, "", 0, 0, 0, true, 0);
                    //oUtil_UserOptionDetailRightsRow = oUtil_UserOptionDetailRightsData._Util_UserOptionDetailRightsT.AddRow(0, "", 0);
                    oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID] = "All";
                    oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId] = oPayOutCatRow.ID;
                    oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID] = UserPriviliges.Screens.Payout.ID;
                    oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID] = UserPriviliges.Modules.POSTransaction.ID;
                    oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId] = UserPriviliges.Permissions.EditPayout.ID;
                    oUtil_UserOptionDetailRightsRow[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed] = true;
                    oUtil_UserOptionDetailRights.Persist(oUtil_UserOptionDetailRightsData);
                }
                return true;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
        }

        public List<String> GetSelectedUser()
        {
            List<String> userlist = new List<string>();
            try
            {

                for (int i = 0; i < dataGridList.Rows.Count; i++)
                {

                    if (Configuration.convertNullToBoolean(dataGridList.Rows[i].Cells[0].Value) == true)
                    {
                        userlist.Add(dataGridList.Rows[i].Cells[dataGridList.Columns["UserId"].Index].Value.ToString());
                    }
                }
            }
            catch { }
            return userlist;

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
                if (oPayOutCatRow == null)
                    return;
                Infragistics.Win.UltraWinEditors.UltraTextEditor txtEditor = (Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
                switch (txtEditor.Name)
                {
                    case "txtName":
                        oPayOutCatRow.PayoutCatType = txtName.Text;
                        break;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }


        private void Display()
        {
            txtID.Text = oPayOutCatRow.ID.ToString();
            txtName.Text = oPayOutCatRow.PayoutCatType;
            txtDefaultDescription.Text = Configuration.convertNullToString(oPayOutCatRow.DefaultDescription);
        }

        public void Edit(Int32 iID)
        {
            try
            {
                
                oPayOutCatData = oPayOutCat.Populate(iID);
                oPayOutCatRow = oPayOutCatData.PayOutCat.GetRowByID(iID.ToString());
                this.Text = "Edit Payout Category";
                this.lblTransactionType.Text = this.Text;
                if (oPayOutCatRow != null)
                    Display();
                fillUserist();
                IsEditing = true;
               
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public void SetNew()
        {
            oPayOutCat = new PayOutCat();
            oPayOutCatData = new PayOutCatData();
            this.Text = "Add Payout Category";
            this.lblTransactionType.Text = this.Text;
            Clear();
            oPayOutCatRow = oPayOutCatData.PayOutCat.AddRow(0, "");
            fillUserist();
        }

        private void Clear()
        {
            txtName.Text = "";
            txtID.Text = "";
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            try
            {
                txtID.Text = "";
                SetNew();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        private void frmInvTransType_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmPayoutCatagory_Load(object sender, EventArgs e)
        {
            this.txtID.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtID.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtName.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtName.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            
            this.txtDefaultDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDefaultDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtDefaultDescription.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtDefaultDescription.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            //UserList_Expand(false);
            this.txtSelectedUser.Enabled = false;
            //this.txtSelectedUser.tol
            UserList_Expand(true);            
            IsCanceled = true;            
            //clsUIHelper.setColorSchecme(this);
            UserList_Expand(false);
        }
        public void SetUser()
        {
            Util_UserOptionDetailRightsData oUtil_UserOptionDetailRightsData;
            Util_UserOptionDetailRightsSvr oUtil_UserOptionDetailRightsSvr = new Util_UserOptionDetailRightsSvr();
            if (oPayOutCatRow.ID > 0)
            {
                oUtil_UserOptionDetailRightsData = oUtil_UserOptionDetailRightsSvr.Populate(" where " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId + "=" + oPayOutCatRow.ID);
                if (oUtil_UserOptionDetailRightsData.Tables[0] != null && oUtil_UserOptionDetailRightsData.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow item in oUtil_UserOptionDetailRightsData.Tables[0].Rows)
                    {
                        if (item[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID].ToString() != "All")
                        {
                            string tempUserID = item[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID].ToString();
                            for (int i = 0; i < dataGridList.Rows.Count; i++)
                            {
                                string tuser = Configuration.convertNullToString(dataGridList.Rows[i].Cells[2].Value).ToString();

                                if (tuser.Trim() == tempUserID.Trim())
                                {
                                    dataGridList.Rows[i].Cells[0].Value = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        bool isExpandBefore = false;
        private void UserList_Expand(bool Value)
        {
            if (Value == true)
            {
                if (grpVendorList.Visible == false)
                {
                    dataGridList.Visible = true;
                    grpVendorList.Visible = true;
                    dataGridList.Height = 125;
                    grpVendorList.Height = dataGridList.Height +10;
                    GridVisibleFlag = true;
                   int yVal=dataGridList.Height+10;
                   int xVal = chkSelectAll.Location.X;
                   chkSelectAll.Location = new Point(xVal, yVal);
                   grpVendorList.Focus();

                   if (!isExpandBefore)
                   {
                       SetUser();
                       isExpandBefore = true;
                   } 
                   //txtSelectedUser.Visible = false;
                   //grpVendorList.fr = true;
                   

                    //dataGridList.Focus();
                    //dataGridList.Rows[1].Selected = true;
                }
            }
            else
            {
                if (grpVendorList.Visible == true)
                {
                    dataGridList.Visible = false;
                    grpVendorList.Visible = false;
                    GridVisibleFlag = false;
                   // txtSelectedUser.Visible = true;
                    //butVendorList.Focus();
                }
            }
            
            List<string> lstUser = GetSelectedUser();
            txtSelectedUser.Text = "";
            foreach (string item in lstUser)
            {
                txtSelectedUser.Text += item + ",";
            }
            
        }
        POS_Core.BusinessRules.User user = new POS_Core.BusinessRules.User();
        UserData udata = new UserData();
        UserSvr UserrSvr = new UserSvr();
        private void butUserList_Click(object sender, EventArgs e)
        {
            string test=udata.User[0][0].ToString();
            if (grpVendorList.Visible == false)
                UserList_Expand(true);
            else
                UserList_Expand(false);
        }
        private void fillUserist()
        {

            //UserData usedata = new UserData();
            string whereClause = " ORDER BY UserID";
            udata = user.PopulateList(whereClause);


            dataGridList.DataSource = udata.User;
            for (int i = 1; i <= udata.User.Columns.Count; i++)
            {
                dataGridList.Columns[i].ReadOnly = true;

                if (dataGridList.Columns[i].Name != "UserID")
                    dataGridList.Columns[i].Visible = false;
                else
                {
                    dataGridList.Columns[i].Width = dataGridList.Width - dataGridList.Columns[0].Width - 20;
                    dataGridList.Columns[i].Name = "UserID";
                }

            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int rowsCount = dataGridList.Rows.Count;
            if (rowsCount > 0)
            {
                for (int i = 0; i < rowsCount; i++)
                {

                    if (chkSelectAll.Checked == true)
                    {

                        dataGridList.Rows[i].Cells[0].Value = true;
                        chkSelectAll.Text = "Unselect All";
                    }
                    else
                    {
                        dataGridList.Rows[i].Cells[0].Value = false;
                        chkSelectAll.Text = "Select All";
                    }
                }


            }
        }

        private void dataGridList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Down)
            {
                dataGridList.Rows[0].Selected = true;
            }
            try
            {
                List<string> lstUser = GetSelectedUser();
                txtSelectedUser.Text = "";
                foreach (string item in lstUser)
                {
                    txtSelectedUser.Text += item + ",";
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void dataGridList_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> lstUser = GetSelectedUser();
                txtSelectedUser.Text = "";
                foreach (string item in lstUser)
                {
                    txtSelectedUser.Text += item + ",";
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void frmPayoutCatagory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.F5)
            {
                string test = udata.User[0][0].ToString();
                if (grpVendorList.Visible == false)
                    UserList_Expand(true);
                else
                    UserList_Expand(false);
            }
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {                
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                e.Handled = true;
            }
        }


    }
}