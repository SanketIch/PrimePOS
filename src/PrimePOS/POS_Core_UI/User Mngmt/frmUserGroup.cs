using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using POS_Core.ErrorLogging;
using System.Data.SqlClient;
using System.Data.OleDb;
using POS_Core.CommonData;
using POS_Core.Resources;

using Resources;
using POS_Core.UserManagement;

namespace POS_Core_UI.UserManagement
{
    public partial class frmUserGroup : Form
    {
        private SaveModeENUM mSaveMode;
        public bool IsCanceled = false;
        private string mUserId = string.Empty;

        public frmUserGroup()
        {
            InitializeComponent();
        }
        public void SetNew()
        {
            mSaveMode = SaveModeENUM.Create;
            Clear();
        }
        private void Clear()
        {
            txtGroupID.Text = "";
            txtGroupName.Text = "";
        }
        private void trvPermissions_AfterSelect(object sender, Infragistics.Win.UltraWinTree.SelectEventArgs e)
        {

        }
        private void checkAll(bool check)
        {
            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode oCtrl in this.trvPermissions.Nodes)
            {
                if (check == true)
                    oCtrl.CheckedState = CheckState.Checked;
                else
                    oCtrl.CheckedState = CheckState.Unchecked;
            }
        }

        private void btnAllowAll_Click(object sender, EventArgs e)
        {
            checkAll(true);
        }

        private void btnDisAllowAll_Click(object sender, EventArgs e)
        {
            checkAll(false);
        }

        private void trvPermissions_AfterCheck(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)
        {
            this.trvPermissions.AfterCheck -= new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);
            Infragistics.Win.UltraWinTree.UltraTreeNode oNode = null;
            if (e.TreeNode.CheckedState == CheckState.Checked)
            {
                oNode = e.TreeNode.Parent;
                while (oNode != null)
                {
                    oNode.CheckedState = e.TreeNode.CheckedState;
                    oNode = oNode.Parent;
                }
                oNode = null;
            }
            this.trvPermissions.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);

            foreach (Infragistics.Win.UltraWinTree.UltraTreeNode oNodeC in e.TreeNode.Nodes)
            {
                oNodeC.CheckedState = e.TreeNode.CheckedState;
            }
        }

        private void BuildTreeView()
        {
            try
            {
                DataSet oDS = new DataSet();
                sm_Modules o = new sm_Modules();
                oDS.Tables.Add(o.Modules);

                sm_Screens oScreens = new sm_Screens();
                oDS.Tables.Add(oScreens.Screens);

                sm_Permissions oPerm = new sm_Permissions();
                oDS.Tables.Add(oPerm.Permissions);

                DataRelation drel = new DataRelation("rl_mod_scr", oDS.Tables["Modules"].Columns["ModuleID"], oDS.Tables["Screens"].Columns["ModuleID"]);
                oDS.Relations.Add(drel);

                DataRelation drelPerm = new DataRelation("rl_scr_perm", oDS.Tables["Screens"].Columns["ScID"], oDS.Tables["Permissions"].Columns["ScreenID"]);
                oDS.Relations.Add(drelPerm);

                Infragistics.Win.UltraWinTree.UltraTreeNode oACNode;

                foreach (DataRow dr in oDS.Tables["Modules"].Rows)
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode oNode;
                    oNode = this.trvPermissions.Nodes.Add(dr["ModuleID"].ToString(), dr["ModuleName"].ToString());
                    oNode.Tag = dr["ModuleID"].ToString();
                    foreach (DataRow drScreens in dr.GetChildRows(drel))
                    {
                        Infragistics.Win.UltraWinTree.UltraTreeNode oScNode = oNode.Nodes.Add("sc-" + drScreens["ScID"].ToString(), drScreens["ScName"].ToString());
                        oScNode.Tag = drScreens["ScID"].ToString();
                        if (bool.Parse(drScreens["EntryScreen"].ToString()) == true)
                        {
                            oACNode = oScNode.Nodes.Add(oScNode.Key + "-999", "Add");
                            oACNode.Tag = -999;
                            oACNode = oScNode.Nodes.Add(oScNode.Key + "-998", "Change");
                            oACNode.Tag = -998;
                        }
                        foreach (DataRow drPerm in drScreens.GetChildRows(drelPerm))
                        {
                            Infragistics.Win.UltraWinTree.UltraTreeNode oPermNode = oScNode.Nodes.Add("pm-" + drPerm["pmID"].ToString(), drPerm["PmName"].ToString());
                            oPermNode.Tag = drPerm["pmID"].ToString();
                            if (bool.Parse(drPerm["EntryScreen"].ToString()) == true)
                            {
                                oACNode = oPermNode.Nodes.Add(oPermNode.Key + "-999", "Add");
                                oACNode.Tag = -999;
                                oPermNode.Nodes.Add(oPermNode.Key + "-998", "Change");
                                oACNode.Tag = -998;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Save() == true)
            {

                ErrorHandler.SaveLog((int)LogENUM.UserRights_Change, this.txtGroupID.Text, "Success", "User rights changed successfully");
                IsCanceled = false;
                this.Close();
            }
            else
            {
                if (this.txtGroupID.Text != null)
                    ErrorHandler.SaveLog((int)LogENUM.UserRights_Change, this.txtGroupID.Text, "Fail", "User rights change not saved");
                this.txtGroupID.Focus();
                IsCanceled = true;
            }
        }
        private bool Save()
        {
            if (ValidateFields() == false)
                return false;

            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();

                bool retValue = false;

                if (mSaveMode == SaveModeENUM.Create)
                {
                    retValue = SaveNewUser();
                    if (retValue && this.txtGroupID.Text != null)
                        ErrorHandler.SaveLog((int)LogENUM.Create_User, this.txtGroupID.Text, "Success", "New user created");

                }
                else
                {
                    retValue = SaveEditUser();
                    if (retValue && this.txtGroupID.Text != null)
                        ErrorHandler.SaveLog((int)LogENUM.Update_User, this.txtGroupID.Text, "Success", "User information updated");

                }

                if (retValue == true)
                {
                    mSaveMode = SaveModeENUM.Modify;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (System.Data.SqlClient.SqlException exp)
            {
                if (exp.Number == 2627)
                    clsUIHelper.ShowErrorMsg(" User group id already exist");
                else
                    clsUIHelper.ShowErrorMsg(exp.Message);

                tr.Rollback();
                conn.Close();
                return false;
            }
            catch (Exception exp)
            {

                clsUIHelper.ShowErrorMsg(exp.Message);
                tr.Rollback();
                conn.Close();
                return false;
            }

        }
        private bool SaveNewUser()
        {
            if (ValidateFields() == false)
                return false;

            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();

                string sSQL = "";
              
                sSQL = "INSERT INTO " + clsPOSDBConstants.Users_tbl + "( " +
                        clsPOSDBConstants.Users_Fld_UserID +
                        " , " + clsPOSDBConstants.Users_Fld_fName +
                         " , " + clsPOSDBConstants.Users_Fld_UserType +
                         " , " + clsPOSDBConstants.Users_Fld_ModifiedBy + //PRIMEPOS-2577 14-Aug-2018 JY Added
                         " ) " +
                        " VALUES( " +
                        "'" + txtGroupID.Text.Replace("'", "''") + "'" +
                         ",'" + txtGroupName.Text.Replace("'", "''") + "'" +
                        " ,'G'" +
                        ", '" + Configuration.UserName.Trim().Replace("'", "''") + "'" +    //PRIMEPOS-2577 14-Aug-2018 JY Added
                        ")";
                conn.ConnectionString = Configuration.ConnectionString;

                conn.Open();
                tr = conn.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "delete from util_userrights where userid='" + this.txtGroupID.Text + "'";
                cmd.ExecuteNonQuery();


                for (int i = 0; i < this.trvPermissions.Nodes.Count; i++)
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode oNode = this.trvPermissions.Nodes[i];
                    sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                    sSQL += " values ('" + this.txtGroupID.Text + "'," +
                        Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + ",null,null," + Configuration.convertBoolToInt(oNode.CheckedState == CheckState.Checked).ToString() + ")";
                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                    for (int j = 0; j < oNode.Nodes.Count; j++)
                    {
                        Infragistics.Win.UltraWinTree.UltraTreeNode oNode1 = oNode.Nodes[j];
                        sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                        sSQL += " values ('" + this.txtGroupID.Text + "'," +
                            Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + "," +
                            Configuration.convertNullToInt(oNode1.Tag.ToString()).ToString() + ",null," + Configuration.convertBoolToInt(oNode1.CheckedState == CheckState.Checked).ToString() + ")";
                        cmd.CommandText = sSQL;
                        cmd.ExecuteNonQuery();
                        for (int k = 0; k < oNode1.Nodes.Count; k++)
                        {
                            Infragistics.Win.UltraWinTree.UltraTreeNode oNode2 = oNode1.Nodes[k];
                            sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                            sSQL += " values ('" + this.txtGroupID.Text + "'," +
                                Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + "," +
                                Configuration.convertNullToInt(oNode1.Tag.ToString()).ToString() + "," +
                                Configuration.convertNullToInt(oNode2.Tag.ToString()).ToString() + "," + Configuration.convertBoolToInt(oNode2.CheckedState == CheckState.Checked).ToString() + ")";
                            cmd.CommandText = sSQL;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                tr.Commit();
                conn.Close();
                mSaveMode = SaveModeENUM.Modify;

                return true;
            }
            catch (System.Data.SqlClient.SqlException exp)
            {
                if (exp.Number == 2627)
                    clsUIHelper.ShowErrorMsg(" User group code already exist.");
                else
                    clsUIHelper.ShowErrorMsg(exp.Message);

                tr.Rollback();
                conn.Close();
                return false;
            }
            catch (Exception exp)
            {

                clsUIHelper.ShowErrorMsg(exp.Message);
                tr.Rollback();
                conn.Close();
                return false;
            }

        }

        private bool ValidateFields()
        {
            try
            {
                string strErrorMSG = string.Empty;
                if (txtGroupID.Text.Trim() == "")
                {
                    strErrorMSG= "Group code can not be blank.";
                }
                if (txtGroupName.Text.Trim() == "")
                {
                    strErrorMSG += "\nGroup name can not be blank.";
                   
                }
                if (string.IsNullOrEmpty(strErrorMSG) == false)
                {
                    clsUIHelper.ShowErrorMsg(strErrorMSG);
                    if (txtGroupID.Text.Trim() == "")
                    {
                        txtGroupID.Focus();
                    }
                    else if (txtGroupName.Text.Trim() == "")
                    {
                        txtGroupName.Focus();
                    }
                    return false;
                }
                return true;
            }

            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
        }

        private bool SaveEditUser()
        {
            if (ValidateFields() == false)
                return false;

            IDbTransaction tr = null;
            IDbConnection conn = DataFactory.CreateConnection();
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();

                string sSQL = "";

                sSQL = "UPDATE " + clsPOSDBConstants.Users_tbl + " SET "
                    + clsPOSDBConstants.Users_Fld_Password + " = '" + "''" + "'" +
                    " , " + clsPOSDBConstants.Users_Fld_fName + " = '" + txtGroupName.Text.Replace("'", "''") + "'" +
                    " , " + clsPOSDBConstants.Users_Fld_ModifiedBy + " = '" + Configuration.UserName.Trim().Replace("'", "''") + "'" +   //PRIMEPOS-2577 14-Aug-2018 JY Added
                    " WHERE " +
                    clsPOSDBConstants.Users_Fld_UserID + " = '" + this.txtGroupID.Text.Replace("'", "''") + "'";

                conn.ConnectionString = Configuration.ConnectionString;

                conn.Open();
                tr = conn.BeginTransaction();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = tr;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "delete from util_userrights where userid='" + this.txtGroupID.Text + "'";
                cmd.ExecuteNonQuery();

                for (int i = 0; i < this.trvPermissions.Nodes.Count; i++)
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode oNode = this.trvPermissions.Nodes[i];
                    sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                    sSQL += " values ('" + this.txtGroupID.Text + "'," +
                        Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + ",null,null," + Configuration.convertBoolToInt(oNode.CheckedState == CheckState.Checked).ToString() + ")";
                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                    for (int j = 0; j < oNode.Nodes.Count; j++)
                    {
                        Infragistics.Win.UltraWinTree.UltraTreeNode oNode1 = oNode.Nodes[j];
                        sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                        sSQL += " values ('" + this.txtGroupID.Text + "'," +
                            Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + "," +
                            Configuration.convertNullToInt(oNode1.Tag.ToString()).ToString() + ",null," + Configuration.convertBoolToInt(oNode1.CheckedState == CheckState.Checked).ToString() + ")";
                        cmd.CommandText = sSQL;
                        cmd.ExecuteNonQuery();
                        for (int k = 0; k < oNode1.Nodes.Count; k++)
                        {
                            Infragistics.Win.UltraWinTree.UltraTreeNode oNode2 = oNode1.Nodes[k];
                            sSQL = " insert into Util_UserRights (UserID,ModuleID,ScreenID,PermissionID,isAllowed) ";
                            sSQL += " values ('" + this.txtGroupID.Text + "'," +
                                Configuration.convertNullToInt(oNode.Tag.ToString()).ToString() + "," +
                                Configuration.convertNullToInt(oNode1.Tag.ToString()).ToString() + "," +
                                Configuration.convertNullToInt(oNode2.Tag.ToString()).ToString() + "," + Configuration.convertBoolToInt(oNode2.CheckedState == CheckState.Checked).ToString() + ")";
                            cmd.CommandText = sSQL;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                tr.Commit();
                conn.Close();
                mSaveMode = SaveModeENUM.Modify;
                return true;
            }
            catch (System.Data.SqlClient.SqlException exp)
            {
                if (exp.Number == 2627)
                    clsUIHelper.ShowErrorMsg(" User group code already exist.");
                else
                    clsUIHelper.ShowErrorMsg(exp.Message);

                tr.Rollback();
                conn.Close();
                return false;
            }
            catch (Exception exp)
            {

                clsUIHelper.ShowErrorMsg(exp.Message);
                tr.Rollback();
                conn.Close();
                return false;
            }

        }

        public void Edit(string UserId)
        {
            mSaveMode = SaveModeENUM.Modify;
            txtGroupID.Enabled = false;
            mUserId = UserId;
            //			Display(UserId);
        }

        private void Display(string pUserId)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            IDataReader reader;
            string sSQL = "";

            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = Configuration.ConnectionString;

            conn.Open();

            try
            {
                sSQL = String.Concat("SELECT * FROM "
                    , clsPOSDBConstants.Users_tbl
                    , " WHERE "
                    , clsPOSDBConstants.Users_Fld_UserID, " = '", pUserId, "'");

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                reader = cmd.ExecuteReader();

                reader.Read();


                txtGroupID.Text = reader.GetString(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_UserID));
                txtGroupName.Text = reader.GetValue(reader.GetOrdinal(clsPOSDBConstants.Users_Fld_fName)).ToString();
                this.trvPermissions.AfterCheck -= new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);
                for (int i = 0; i < this.trvPermissions.Nodes.Count; i++)
                {
                    Infragistics.Win.UltraWinTree.UltraTreeNode oNode = this.trvPermissions.Nodes[i];
                    if (UserPriviliges.IsUserHasPriviliges(Configuration.convertNullToInt(oNode.Tag.ToString()), this.txtGroupID.Text))
                    {
                        oNode.CheckedState = CheckState.Checked;
                    }
                    else
                    {
                        oNode.CheckedState = CheckState.Unchecked;
                    }
                    for (int j = 0; j < oNode.Nodes.Count; j++)
                    {
                        Infragistics.Win.UltraWinTree.UltraTreeNode oNode1 = oNode.Nodes[j];
                        if (UserPriviliges.IsUserHasPriviliges(Configuration.convertNullToInt(oNode.Tag.ToString()), Configuration.convertNullToInt(oNode1.Tag.ToString()), this.txtGroupID.Text))
                        {
                            oNode1.CheckedState = CheckState.Checked;
                        }
                        else
                        {
                            oNode1.CheckedState = CheckState.Unchecked;
                        }
                        for (int k = 0; k < oNode1.Nodes.Count; k++)
                        {
                            Infragistics.Win.UltraWinTree.UltraTreeNode oNode2 = oNode1.Nodes[k];
                            if (UserPriviliges.IsUserHasPriviliges(Configuration.convertNullToInt(oNode.Tag.ToString()), Configuration.convertNullToInt(oNode1.Tag.ToString()), Configuration.convertNullToInt(oNode2.Tag.ToString()), this.txtGroupID.Text))
                            {
                                oNode2.CheckedState = CheckState.Checked;
                            }
                            else
                            {
                                oNode2.CheckedState = CheckState.Unchecked;
                            }
                        }
                    }
                }
                this.trvPermissions.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.trvPermissions_AfterCheck);
                conn.Close();
            }
            catch (NullReferenceException)
            {
                conn.Close();
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                conn.Close();
            }
        }

        private void frmUserGroup_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            BuildTreeView();
            if (mSaveMode == SaveModeENUM.Modify)
            {
                Display(mUserId);
            }
            this.txtGroupID.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtGroupID.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            this.txtGroupName.Enter += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtGroupName.Leave += new System.EventHandler(clsUIHelper.AfterExitEditMode);
        }

        private void frmUserGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
            }
        }
    }
}
