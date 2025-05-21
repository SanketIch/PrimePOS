using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using Infragistics.Win.UltraWinGrid;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using NLog;
using Resources;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmPayOut.
	/// </summary>
    public class frmFunctionKeys : System.Windows.Forms.Form
    {
        private FunctionKeys oFunctionKeys = new FunctionKeys();
        private FunctionKeysData oFunctionKeysData = new FunctionKeysData();
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.Misc.UltraLabel lblTransactionType;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        //		private static frmfunctionKeySelection oFunctionKeySelection=new frmfunctionKeySelection();
        private Infragistics.Win.UltraWinGrid.UltraGrid grdDetail;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cpColor;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbParent;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbFuncType;
        private IContainer components;
        private bool isAddKey = false;
        private bool isCallFromTrans = false;
        int PrevSubPosValue = 0;
        public Infragistics.Win.Misc.UltraButton btnDelete;
        public Infragistics.Win.Misc.UltraButton btnOk;
        int PrevMainPosValue = 0;
        bool m_ExceptionOccured = false;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbForeColor;
        private Infragistics.Win.Misc.UltraPanel pnlClose;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraPanel pnlSave;
        private Infragistics.Win.Misc.UltraLabel lblSave;
        private Infragistics.Win.Misc.UltraPanel pblOk;
        private Infragistics.Win.Misc.UltraLabel lblOk;
        private Infragistics.Win.Misc.UltraPanel pnlDelete;
        private Infragistics.Win.Misc.UltraLabel lblDelete;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private string sAssignItemError = "Please assign an item to the operation failed.";  //PRIMEPOS-2584 24-Sep-2018 JY Added 
        private string sTypeValueError = "Please type vlaue of operation failed.";  //PRIMEPOS-2584 24-Sep-2018 JY Added 

        public frmFunctionKeys()
        {
            //
            // Required for Windows Form Designer support
            //
            try
            {
                InitializeComponent();
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmFunctionKeys()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        //		public static frmfunctionKeySelection functionKeySelection
        //		{
        //			get
        //			{
        //				if (oFunctionKeySelection.IsDisposed)
        //					oFunctionKeySelection = new frmfunctionKeySelection();
        //				return oFunctionKeySelection;
        //			}
        //		}

        private void ApplyGrigFormat()
        {
            grdDetail.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
            grdDetail.DisplayLayout.Override.CellClickAction = CellClickAction.EditAndSelectText;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_KeyId].Hidden = true;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Hidden = false;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Header.Caption = "Function\nKey";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Header.SetVisiblePosition(1, false);
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunKey].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunKey].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnMouseEnter;

            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].SortIndicator = SortIndicator.None;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].CellActivation = Activation.AllowEdit;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].CellActivation = Activation.AllowEdit;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].CellActivation = Activation.AllowEdit;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Header.Caption = "Function\nType";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Header.Caption = "Indv.\nMenu Pos";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Header.Caption = "Parent\nMenu Pos";
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Parent].Width = 120;
            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Width = 60;
            this.grdDetail.UpdateMode = UpdateMode.OnCellChange;
            this.grdDetail.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Single;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void grdDetail_BeforeRowUpdate(object sender, CancelableRowEventArgs e)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell = null;
            try
            {
                if (m_ExceptionOccured == true)
                {
                    m_ExceptionOccured = false;
                    return;
                }
                if (grdDetail.ActiveRow == null )
                {
                    return;
                }
                if (this.grdDetail.ActiveCell == null)
                {
                    return;
                }
                if (this.grdDetail.ActiveCell == null)
                {
                    return;
                }
                if (this.grdDetail.ActiveCell.Column.Key != clsPOSDBConstants.FunctionKeys_Fld_Operation)
                {
                    return;
                }
                oCurrentRow = grdDetail.ActiveRow;
                oCurrentCell = null;

                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation];
                grdDetail.ActiveCell = oCurrentCell;
                if (string.IsNullOrEmpty(oCurrentCell.Text.ToString()) && string.IsNullOrEmpty(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Text))
                {

                    string strErrorMesage = "";
                    if (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value.ToString() == "I")
                    {
                        strErrorMesage = sAssignItemError;
                    }
                    else
                    {
                        strErrorMesage = sTypeValueError;
                    }
                    throw new Exception(strErrorMesage);
                }
            }
            catch (Exception exp)
            {
                if (exp.Message.ToString().ToUpper().Trim() != sAssignItemError.Trim().ToUpper() && exp.Message.ToString().ToUpper().Trim() != sTypeValueError.Trim().ToUpper())    //PRIMEPOS-2584 24-Sep-2018 JY Added 
                {
                    logger.Fatal(exp, "grdDetail_BeforeRowUpdate(object sender, CancelableRowEventArgs e)");
                }
                clsUIHelper.ShowErrorMsg(exp.Message);
                if (oCurrentCell != null)
                {
                    this.grdDetail.ActiveCell = oCurrentCell;
                    this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
                CancelEventArgs eC= new CancelEventArgs();
                eC.Cancel = true;
            }
        }

        /// <summary>
        /// Author: Shitaljit
        /// To vidate the data to be save
        /// </summary>
        private bool ValidateData()
        {
            bool RetVal = false;
            int Index = 0;
            try
            {
                foreach (UltraGridRow row in grdDetail.Rows)
                {

                    if (string.IsNullOrEmpty(row.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Value.ToString()) == true &&
                        string.IsNullOrEmpty(row.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Text) == true)
                    {
                        this.grdDetail.ActiveRow = row;
                        RetVal = false;
                        string strErrorMesage = "";
                        if (row.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value.ToString() == "I")
                        {
                            strErrorMesage = sAssignItemError;
                        }
                        else
                        {
                            strErrorMesage = sTypeValueError;
                        }
                        throw new Exception(strErrorMesage);
                    }
                   
                    oFunctionKeysData.FunctionKeys.Rows[Index][clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor] = row.Cells[clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor].Value.ToString();
                    oFunctionKeysData.FunctionKeys.Rows[Index][clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor] = row.Cells[clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor].Value.ToString();
                    if (Index == grdDetail.Rows.Count - 1)
                    {
                        string sSQL = " UPDATE   " + clsPOSDBConstants.FunctionKeys_tbl +
                          " SET  " + clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor + " = '" + row.Cells[clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor].Value.ToString() + "'" +
                          ", " + clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor + " = '" +row.Cells[clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor].Value.ToString()+"'" +
                          " WHERE  MainPosition='" + row.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value.ToString() + "'";
                        DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                    }
                    Index++;
                }
               
                RetVal = true;
            }
            catch (Exception exp)
            {
                if (exp.Message.ToString().ToUpper().Trim() != sAssignItemError.Trim().ToUpper() && exp.Message.ToString().ToUpper().Trim() != sTypeValueError.Trim().ToUpper())    //PRIMEPOS-2584 24-Sep-2018 JY Added 
                {                
                    logger.Fatal(exp, "ValidateData()");
                }
                clsUIHelper.ShowErrorMsg(exp.Message);
                m_ExceptionOccured = true;
                isAddKey = true;
                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activation = Activation.AllowEdit;
                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activate();
                this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
               
            }
            return RetVal;
        }
        private void Save()
        {
            try
            {
                if (ValidateData())
                {
                    grdDetail.Update();
                    grdDetail.UpdateData();
                    oFunctionKeysData.AcceptChanges();
                    oFunctionKeys.Persist(oFunctionKeysData);
                    this.Close();
                }
                else if (m_ExceptionOccured == true)
                {
                    m_ExceptionOccured = false;
                    this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activation = Activation.AllowEdit;
                    this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activate();
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Key");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Operation");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Key");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Operation", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ButtonBackColor");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ButtonForeColor");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FunctionType", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Parent", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("MainPosition", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SubPosition", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 4);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFunctionKeys));
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            this.cpColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.cmbForeColor = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbFuncType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.cmbParent = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTransactionType = new Infragistics.Win.Misc.UltraLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdDetail = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnOk = new Infragistics.Win.Misc.UltraButton();
            this.pnlClose = new Infragistics.Win.Misc.UltraPanel();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.pnlSave = new Infragistics.Win.Misc.UltraPanel();
            this.lblSave = new Infragistics.Win.Misc.UltraLabel();
            this.pnlDelete = new Infragistics.Win.Misc.UltraPanel();
            this.lblDelete = new Infragistics.Win.Misc.UltraLabel();
            this.pblOk = new Infragistics.Win.Misc.UltraPanel();
            this.lblOk = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.cpColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbForeColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFuncType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbParent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnlClose.ClientArea.SuspendLayout();
            this.pnlClose.SuspendLayout();
            this.pnlSave.ClientArea.SuspendLayout();
            this.pnlSave.SuspendLayout();
            this.pnlDelete.ClientArea.SuspendLayout();
            this.pnlDelete.SuspendLayout();
            this.pblOk.ClientArea.SuspendLayout();
            this.pblOk.SuspendLayout();
            this.SuspendLayout();
            // 
            // cpColor
            // 
            this.cpColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsXPCommandButton;
            this.cpColor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.OfficeXP;
            this.cpColor.Location = new System.Drawing.Point(46, 20);
            this.cpColor.Name = "cpColor";
            this.cpColor.Size = new System.Drawing.Size(144, 25);
            this.cpColor.TabIndex = 30;
            this.cpColor.Text = "Control";
            this.cpColor.Visible = false;
            this.cpColor.ColorChanged += new System.EventHandler(this.cpColor_ColorChanged);
            // 
            // cmbForeColor
            // 
            this.cmbForeColor.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem3.DataValue = "Color [Black]";
            valueListItem3.DisplayText = "Black";
            valueListItem4.DataValue = "Color [White]";
            valueListItem4.DisplayText = "White";
            this.cmbForeColor.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.cmbForeColor.Location = new System.Drawing.Point(110, 15);
            this.cmbForeColor.Name = "cmbForeColor";
            this.cmbForeColor.Size = new System.Drawing.Size(130, 25);
            this.cmbForeColor.TabIndex = 90;
            this.cmbForeColor.Visible = false;
            // 
            // cmbFuncType
            // 
            this.cmbFuncType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "I";
            valueListItem1.DisplayText = "Item";
            valueListItem2.DataValue = "P";
            valueListItem2.DisplayText = "Parent";
            this.cmbFuncType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.cmbFuncType.Location = new System.Drawing.Point(196, 29);
            this.cmbFuncType.Name = "cmbFuncType";
            this.cmbFuncType.Size = new System.Drawing.Size(130, 25);
            this.cmbFuncType.TabIndex = 3;
            this.cmbFuncType.Visible = false;
            // 
            // cmbParent
            // 
            this.cmbParent.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbParent.Location = new System.Drawing.Point(271, 15);
            this.cmbParent.Name = "cmbParent";
            this.cmbParent.Size = new System.Drawing.Size(130, 25);
            this.cmbParent.TabIndex = 32;
            this.cmbParent.Visible = false;
            // 
            // ultraDataSource2
            // 
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2});
            this.ultraDataSource2.Rows.AddRange(new object[] {
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F1"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F2"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F3"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F4"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F5"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F6"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F7"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F8"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F9"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F10"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F11"))}),
            new Infragistics.Win.UltraWinDataSource.UltraDataRow(new object[] {
                        ((object)("Key")),
                        ((object)("F12"))})});
            // 
            // btnClose
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(62)))), ((int)(((byte)(76)))));
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Appearance = appearance1;
            this.btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(50, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Close";
            this.btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance = appearance2;
            this.btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Location = new System.Drawing.Point(50, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(70, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTransactionType
            // 
            this.lblTransactionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.ForeColorDisabled = System.Drawing.Color.White;
            appearance3.TextHAlignAsString = "Center";
            this.lblTransactionType.Appearance = appearance3;
            this.lblTransactionType.BackColorInternal = System.Drawing.Color.Transparent;
            this.lblTransactionType.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionType.Location = new System.Drawing.Point(18, 7);
            this.lblTransactionType.Name = "lblTransactionType";
            this.lblTransactionType.Size = new System.Drawing.Size(967, 38);
            this.lblTransactionType.TabIndex = 25;
            this.lblTransactionType.Tag = "Header";
            this.lblTransactionType.Text = "Function Keys";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdDetail);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(18, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(967, 424);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add / Edit / Delete Function Keys";
            // 
            // grdDetail
            // 
            this.grdDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.BackColor2 = System.Drawing.Color.White;
            appearance4.BackColorDisabled = System.Drawing.Color.White;
            appearance4.BackColorDisabled2 = System.Drawing.Color.White;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdDetail.DisplayLayout.Appearance = appearance4;
            this.grdDetail.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            ultraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn1.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn1.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn1.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn1.TabStop = false;
            ultraGridColumn1.Width = 118;
            ultraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.True;
            ultraGridColumn2.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(103, 0);
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn2.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
            ultraGridColumn2.Width = 235;
            ultraGridColumn3.EditorComponent = this.cpColor;
            ultraGridColumn3.Header.Caption = "Back Color";
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.RowLayoutColumnInfo.OriginX = 14;
            ultraGridColumn3.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn3.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn3.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn3.Width = 140;
            ultraGridColumn4.EditorComponent = this.cmbForeColor;
            ultraGridColumn4.Header.Caption = "Fore Color";
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 16;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.Width = 140;
            ultraGridColumn5.EditorComponent = this.cmbFuncType;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn5.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn5.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn5.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn6.EditorComponent = this.cmbParent;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn6.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn6.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn6.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.MaskInput = "nnnnnnnnn";
            ultraGridColumn7.RowLayoutColumnInfo.OriginX = 10;
            ultraGridColumn7.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn7.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn7.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn8.Header.VisiblePosition = 7;
            ultraGridColumn8.MaskInput = "nnnnnnnnn";
            ultraGridColumn8.RowLayoutColumnInfo.OriginX = 12;
            ultraGridColumn8.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn8.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn8.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn9.Header.VisiblePosition = 8;
            ultraGridColumn9.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn9.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn9.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn9.RowLayoutColumnInfo.SpanY = 2;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9});
            ultraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout;
            this.grdDetail.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDetail.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdDetail.DisplayLayout.InterBandSpacing = 10;
            this.grdDetail.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDetail.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.BackColor2 = System.Drawing.Color.White;
            appearance6.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            appearance7.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.AddRowAppearance = appearance7;
            this.grdDetail.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.grdDetail.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdDetail.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDetail.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.grdDetail.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.BackColor2 = System.Drawing.Color.White;
            appearance9.BackColorDisabled = System.Drawing.Color.White;
            appearance9.BackColorDisabled2 = System.Drawing.Color.White;
            appearance9.BorderColor = System.Drawing.Color.Black;
            appearance9.BorderColor3DBase = System.Drawing.Color.Black;
            this.grdDetail.DisplayLayout.Override.CellAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.White;
            appearance10.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(230)))));
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance10.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.BorderColor = System.Drawing.Color.Gray;
            appearance10.BorderColor3DBase = System.Drawing.Color.Gray;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            appearance10.ImageBackgroundAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered;
            this.grdDetail.DisplayLayout.Override.CellButtonAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdDetail.DisplayLayout.Override.EditCellAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredInRowAppearance = appearance12;
            appearance13.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.FilteredOutRowAppearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.White;
            appearance14.BackColor2 = System.Drawing.Color.White;
            appearance14.BackColorDisabled = System.Drawing.Color.White;
            appearance14.BackColorDisabled2 = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.FixedCellAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance15.BackColor2 = System.Drawing.Color.Beige;
            this.grdDetail.DisplayLayout.Override.FixedHeaderAppearance = appearance15;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance16.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance16.FontData.BoldAsString = "True";
            appearance16.FontData.SizeInPoints = 10F;
            appearance16.ForeColor = System.Drawing.Color.White;
            appearance16.TextHAlignAsString = "Left";
            appearance16.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdDetail.DisplayLayout.Override.HeaderAppearance = appearance16;
            this.grdDetail.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdDetail.DisplayLayout.Override.MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Never;
            appearance17.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAlternateAppearance = appearance17;
            appearance18.BackColor = System.Drawing.Color.White;
            appearance18.BackColor2 = System.Drawing.Color.White;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance18.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance18.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowAppearance = appearance18;
            appearance19.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.RowPreviewAppearance = appearance19;
            appearance20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(62)))), ((int)(((byte)(176)))));
            appearance20.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(129)))), ((int)(((byte)(247)))));
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance20.BorderColor = System.Drawing.Color.Gray;
            appearance20.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.RowSelectorAppearance = appearance20;
            this.grdDetail.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdDetail.DisplayLayout.Override.RowSizingArea = Infragistics.Win.UltraWinGrid.RowSizingArea.EntireRow;
            this.grdDetail.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance21.BackColor = System.Drawing.Color.Navy;
            appearance21.BackColorAlpha = Infragistics.Win.Alpha.Opaque;
            this.grdDetail.DisplayLayout.Override.SelectedCellAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.Navy;
            appearance22.BackColorDisabled = System.Drawing.Color.Navy;
            appearance22.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance22.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            appearance22.BorderColor = System.Drawing.Color.Gray;
            appearance22.ForeColor = System.Drawing.Color.White;
            this.grdDetail.DisplayLayout.Override.SelectedRowAppearance = appearance22;
            this.grdDetail.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdDetail.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance23.BorderColor = System.Drawing.Color.Gray;
            this.grdDetail.DisplayLayout.Override.TemplateAddRowAppearance = appearance23;
            this.grdDetail.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdDetail.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance24.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            scrollBarLook1.ButtonAppearance = appearance24;
            this.grdDetail.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdDetail.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grdDetail.Location = new System.Drawing.Point(13, 22);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.Size = new System.Drawing.Size(945, 391);
            this.grdDetail.TabIndex = 2;
            this.grdDetail.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdDetail.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdDetail.AfterCellActivate += new System.EventHandler(this.grdDetail_AfterCellActivate);
            this.grdDetail.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_AfterCellUpdate);
            this.grdDetail.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdDetail_InitializeRow);
            this.grdDetail.AfterEnterEditMode += new System.EventHandler(this.grdDetail_AfterEnterEditMode);
            this.grdDetail.AfterRowActivate += new System.EventHandler(this.grdDetail_AfterRowActivate);
            this.grdDetail.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdDetail_AfterRowInsert);
            this.grdDetail.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdDetail_AfterRowUpdate);
            this.grdDetail.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.grdDetail_BeforeRowUpdate);
            this.grdDetail.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdDetail_ClickCellButton);
            this.grdDetail.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.grdDetail_BeforeCellDeactivate);
            this.grdDetail.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.grdDetail_BeforeExitEditMode);
            this.grdDetail.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.grdDetail_BeforeRowDeactivate);
            this.grdDetail.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdDetail_BeforeCellUpdate);
            this.grdDetail.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdDetail_Error);
            this.grdDetail.Enter += new System.EventHandler(this.grdDetail_Enter);
            this.grdDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdDetail_KeyDown);
            this.grdDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.grdDetail_KeyPress);
            this.grdDetail.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmFunctionKeys_KeyUp);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.pblOk);
            this.groupBox2.Controls.Add(this.pnlDelete);
            this.groupBox2.Controls.Add(this.pnlSave);
            this.groupBox2.Controls.Add(this.pnlClose);
            this.groupBox2.Controls.Add(this.cmbForeColor);
            this.groupBox2.Controls.Add(this.cmbParent);
            this.groupBox2.Controls.Add(this.cmbFuncType);
            this.groupBox2.Controls.Add(this.cpColor);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(18, 478);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(967, 54);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnDelete
            // 
            appearance27.BackColor = System.Drawing.Color.White;
            appearance27.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Appearance = appearance27;
            this.btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(30, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 30);
            this.btnDelete.TabIndex = 89;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnOk
            // 
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.ForeColor = System.Drawing.Color.Black;
            this.btnOk.Appearance = appearance25;
            this.btnOk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Location = new System.Drawing.Point(30, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 30);
            this.btnOk.TabIndex = 88;
            this.btnOk.Text = "&Add";
            this.btnOk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlClose
            // 
            // 
            // pnlClose.ClientArea
            // 
            this.pnlClose.ClientArea.Controls.Add(this.btnClose);
            this.pnlClose.ClientArea.Controls.Add(this.lblClose);
            this.pnlClose.Location = new System.Drawing.Point(838, 16);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Size = new System.Drawing.Size(120, 30);
            this.pnlClose.TabIndex = 91;
            // 
            // lblClose
            // 
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance30.FontData.BoldAsString = "True";
            appearance30.ForeColor = System.Drawing.Color.White;
            appearance30.TextHAlignAsString = "Center";
            appearance30.TextVAlignAsString = "Middle";
            this.lblClose.Appearance = appearance30;
            this.lblClose.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblClose.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblClose.Location = new System.Drawing.Point(0, 0);
            this.lblClose.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(50, 30);
            this.lblClose.TabIndex = 0;
            this.lblClose.Tag = "NOCOLOR";
            this.lblClose.Text = "Alt + C";
            this.lblClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlSave
            // 
            // 
            // pnlSave.ClientArea
            // 
            this.pnlSave.ClientArea.Controls.Add(this.btnSave);
            this.pnlSave.ClientArea.Controls.Add(this.lblSave);
            this.pnlSave.Location = new System.Drawing.Point(711, 16);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(120, 30);
            this.pnlSave.TabIndex = 92;
            // 
            // lblSave
            // 
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance29.FontData.BoldAsString = "True";
            appearance29.ForeColor = System.Drawing.Color.White;
            appearance29.TextHAlignAsString = "Center";
            appearance29.TextVAlignAsString = "Middle";
            this.lblSave.Appearance = appearance29;
            this.lblSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSave.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSave.Location = new System.Drawing.Point(0, 0);
            this.lblSave.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(50, 30);
            this.lblSave.TabIndex = 0;
            this.lblSave.Tag = "NOCOLOR";
            this.lblSave.Text = "Alt + S";
            this.lblSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlDelete
            // 
            // 
            // pnlDelete.ClientArea
            // 
            this.pnlDelete.ClientArea.Controls.Add(this.btnDelete);
            this.pnlDelete.ClientArea.Controls.Add(this.lblDelete);
            this.pnlDelete.Location = new System.Drawing.Point(584, 16);
            this.pnlDelete.Name = "pnlDelete";
            this.pnlDelete.Size = new System.Drawing.Size(120, 30);
            this.pnlDelete.TabIndex = 93;
            this.pnlDelete.Visible = false;
            // 
            // lblDelete
            // 
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance28.FontData.BoldAsString = "True";
            appearance28.ForeColor = System.Drawing.Color.White;
            appearance28.TextHAlignAsString = "Center";
            appearance28.TextVAlignAsString = "Middle";
            this.lblDelete.Appearance = appearance28;
            this.lblDelete.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDelete.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDelete.Location = new System.Drawing.Point(0, 0);
            this.lblDelete.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(30, 30);
            this.lblDelete.TabIndex = 0;
            this.lblDelete.Tag = "NOCOLOR";
            this.lblDelete.Text = "F11";
            this.lblDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pblOk
            // 
            // 
            // pblOk.ClientArea
            // 
            this.pblOk.ClientArea.Controls.Add(this.btnOk);
            this.pblOk.ClientArea.Controls.Add(this.lblOk);
            this.pblOk.Location = new System.Drawing.Point(457, 16);
            this.pblOk.Name = "pblOk";
            this.pblOk.Size = new System.Drawing.Size(120, 30);
            this.pblOk.TabIndex = 94;
            this.pblOk.Visible = false;
            // 
            // lblOk
            // 
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(138)))), ((int)(((byte)(31)))));
            appearance26.FontData.BoldAsString = "True";
            appearance26.ForeColor = System.Drawing.Color.White;
            appearance26.TextHAlignAsString = "Center";
            appearance26.TextVAlignAsString = "Middle";
            this.lblOk.Appearance = appearance26;
            this.lblOk.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblOk.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblOk.Location = new System.Drawing.Point(0, 0);
            this.lblOk.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblOk.Name = "lblOk";
            this.lblOk.Size = new System.Drawing.Size(30, 30);
            this.lblOk.TabIndex = 0;
            this.lblOk.Tag = "NOCOLOR";
            this.lblOk.Text = "F2";
            this.lblOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmFunctionKeys
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(108)))), ((int)(((byte)(172)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1001, 540);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTransactionType);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmFunctionKeys";
            this.Text = "Function Keys";
            this.Activated += new System.EventHandler(this.frmFunctionKeys_Activated);
            this.Load += new System.EventHandler(this.frmFunctionKeys_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmFunctionKeys_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmFunctionKeys_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmFunctionKeys_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.cpColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbForeColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFuncType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbParent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnlClose.ClientArea.ResumeLayout(false);
            this.pnlClose.ResumeLayout(false);
            this.pnlSave.ClientArea.ResumeLayout(false);
            this.pnlSave.ResumeLayout(false);
            this.pnlDelete.ClientArea.ResumeLayout(false);
            this.pnlDelete.ResumeLayout(false);
            this.pblOk.ClientArea.ResumeLayout(false);
            this.pblOk.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        private void SetParentValue()
        {
            for (int RowIndex = 0; RowIndex < oFunctionKeysData.FunctionKeys.Rows.Count; RowIndex++)
            {
                if (oFunctionKeysData.FunctionKeys.Rows[RowIndex][clsPOSDBConstants.FunctionKeys_Fld_FunctionType].ToString().Equals(clsPOSDBConstants.FunctionKeys_Type_Item))
                {
                    oFunctionKeysData.FunctionKeys.Rows[RowIndex][clsPOSDBConstants.FunctionKeys_Fld_FunctionType] = "I";
                }
                else
                {
                    oFunctionKeysData.FunctionKeys.Rows[RowIndex][clsPOSDBConstants.FunctionKeys_Fld_FunctionType] = "P";
                }
            }
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SetParentValue();
            Save();
        }

        private void frmFunctionKeys_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
           

        }

        private void LoadData()
        {
            try
            {
                LoadParents();
                oFunctionKeysData = oFunctionKeys.PopulateList(" ORDER BY  " + clsPOSDBConstants.FunctionKeys_Fld_MainPosition);
                grdDetail.DataSource = oFunctionKeysData;
                grdDetail.Refresh();
                ApplyGrigFormat();
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "LoadData()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }

        }
        private void frmFunctionKeys_Load(object sender, System.EventArgs e)
        {
            this.grdDetail.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.grdDetail.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            clsUIHelper.SetKeyActionMappings(this.grdDetail);
            clsUIHelper.SetAppearance(this.grdDetail);
            clsUIHelper.setColorSchecme(this);
            btnClose.Appearance.BackColor = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            btnClose.Appearance.BackColor2 = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            if (isAddKey == false)
            {
                LoadData();
            }
        }

        private void grdDetail_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            if (this.grdDetail.ActiveCell == null) return;

            if (e.KeyData == Keys.Enter)
            {
                if (this.grdDetail.ContainsFocus == false)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (this.grdDetail.ActiveRow.Index == grdDetail.Rows.Count - 1 && this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor)
                {
                    grdDetail.PerformAction(UltraGridAction.LastCellInGrid);
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            else if (this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value.ToString() == "I" && e.KeyData == System.Windows.Forms.Keys.F4)
            {
                SearchItem();
            }
        }


        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void grdDetail_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (this.grdDetail.ActiveCell != null && this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_SubPosition ||
                   this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_MainPosition)
                {
                    e.Handled = false;
                    return;
                }
                if (this.grdDetail.ActiveCell != null && (this.grdDetail.ActiveCell.Column.Key.Equals(clsPOSDBConstants.FunctionKeys_Fld_Operation) == false ||
                   this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Text.ToString().Equals(clsPOSDBConstants.FunctionKeys_Fld_Parent) == false))
                {
                    e.Handled = true;
                }
            }
            catch { }
        }

        private bool isValidKey(Char chr)
        {
            return true;
        }
        private bool IsFunctionKey(Char chr)
        {
            return true;
        }
        private bool IsAlphabet(Char chr)
        {
            return true;
        }
        private bool IsCtrlKeys(Char chr)
        {
            return true;
        }
        private bool ValidateKey(string ItemID, int ParentID)
        {
            try 
            {
                FunctionKeysData OData = oFunctionKeys.PopulateList(" WHERE Operation = '" + ItemID + "' AND Parent = " + ParentID);
                if (OData != null && OData.FunctionKeys.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "ValidateKey(string ItemID, int ParentID)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
                return false;
            }
        }

        private void SearchItem()
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog(this);
                if (oSearch.IsCanceled) return;
                int ParentID = Configuration.convertNullToInt(grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Value);
                if (ValidateKey(oSearch.SelectedRowID(), ParentID) == true)
                {
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value = oSearch.SelectedRowID();
                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.Item_Fld_Description].Value = FunKeyCommonOperations.getItemName(oSearch.SelectedRowID());
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Item# " + oSearch.SelectedRowID() + " is already assinged to parent\" " + grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Text + "\".");
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "SearchItem()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        private void grdDetail_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {

            if (grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_FunKey)
            {
                //				functionKeySelection.ShowDialog();
                //				grdDetail.ActiveCell.Value = functionKeySelection.Key1 ;
                //				if (functionKeySelection.Key2.Trim()!= "") grdDetail.ActiveCell.Value = functionKeySelection.Key1 + "+" + functionKeySelection.Key2;
            }
            else if (grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_Operation)
            {
                SearchItem();
            }

        }

        private void grdDetail_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            grdDetail.Focus();
        }

        private void frmFunctionKeys_Activated(object sender, System.EventArgs e)
        {
            clsUIHelper.CurrentForm = this;
        }

        private void grdDetail_Enter(object sender, System.EventArgs e)
        {
            try
            {
                if (this.grdDetail.Rows.Count > 0 && isAddKey == false)
                {

                    this.grdDetail.Rows[0].Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activation = Activation.AllowEdit;
                    this.grdDetail.Rows[0].Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activate();
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
                else
                {
                    if (isCallFromTrans == true)
                    {
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activate();
                    }
                    else
                    {
                        this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Activate();
                    }
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
            catch (Exception)
            { }
        }

        private void cpColor_ColorChanged(object sender, System.EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, System.EventArgs e)
        {

        }
        private void ChangePosition(int CurrValue, string Key)
        {
            int nextPos = 0;
            try
            {
                switch (Key)
                {
                    case "Sub":
                        int parentID = Configuration.convertNullToInt(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Value);
                        if (CurrValue == 0)
                        {
                            nextPos = oFunctionKeys.GetNextKeyPosition(clsPOSDBConstants.FunctionKeys_Fld_SubPosition, parentID);
                            grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Value = nextPos;
                        }
                        else if(PrevSubPosValue > 0)
                        {
                            int index = 0;
                            foreach (UltraGridRow oRow in grdDetail.Rows)
                            {
                                if (Configuration.convertNullToInt(oRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Value) == CurrValue && oRow.Index != grdDetail.ActiveRow.Index)
                                {
                                    grdDetail.Rows[index].Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Value = PrevSubPosValue;
                                    grdDetail.Update();
                                    break;
                                }
                                index++;
                            }
                        }
                        break;
                    case "Main":
                        if (CurrValue == 0)
                        {
                            nextPos = oFunctionKeys.GetNextKeyPosition(clsPOSDBConstants.FunctionKeys_Fld_MainPosition, 0);
                            grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value = nextPos;
                        }
                        else if(PrevMainPosValue > 0)
                        {
                            int index = 0;
                            foreach (UltraGridRow oRow in grdDetail.Rows)
                            {
                                if (Configuration.convertNullToInt(oRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value) == CurrValue && oRow.Index != grdDetail.ActiveRow.Index)
                                {
                                    grdDetail.Rows[index].Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value = PrevMainPosValue;
                                    grdDetail.Update();
                                    break;
                                }
                                index++;
                            }
                        }
                        break;

                }
                grdDetail.Refresh();
                PrevMainPosValue = 0;
                PrevSubPosValue = 0;
            }
            catch { }
        }
        private void grdDetail_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                e.Row.Height = 25;
                switch (e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value.ToString())
                {
                    case "I": e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value = clsPOSDBConstants.FunctionKeys_Type_Item;
                        e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Activation = Activation.Disabled;
                        break;
                    case "P": e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value = clsPOSDBConstants.FunctionKeys_Type_Parent;
                        e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Activation = Activation.Disabled;
                        e.Row.Cells[clsPOSDBConstants.Item_Fld_Description].Activation = Activation.Disabled;
                        e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Activation = Activation.Disabled;
                        break;
                }
                e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Activation = Activation.AllowEdit;
                e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].ToolTipText = "1-24 In Page 1\n25-48 in Page 2\n49-72 in Page 3 and so on..";
                e.Row.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].ToolTipText = "1-40 In Page 1\n41-80 in Page 2\n81-120 in Page 3 and so on..";
            }
            catch { }
        }

        private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            if (this.grdDetail.ActiveRow != null)
            {
                if (grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_Operation)
                {
                    //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl, "--##--", "");
                    frmSearchMain oSearch = new frmSearchMain(clsPOSDBConstants.Item_tbl, "--##--", "", true);  //20-Dec-2017 JY Added new reference
                    oSearch.ShowDialog(this);
                    if (oSearch.IsCanceled) return;

                    grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value = oSearch.SelectedRowID();
                }
            }
        }

        //Added By Shitaljit on 24 May 2013 for JIRA-932 Assign unlimited touch kyes

        private void LoadParents()
        {
            cmbParent.Items.Clear();
            DataTable dtParents = oFunctionKeys.PopulateParents();
            foreach (DataRow dr in dtParents.Rows)
            {
                cmbParent.Items.Add(Configuration.convertNullToInt(dr[clsPOSDBConstants.FunctionKeys_Fld_KeyId].ToString()), Configuration.convertNullToString(dr[clsPOSDBConstants.FunctionKeys_Fld_Operation]));
            }
        }

        public void AddKey(FunctionKeysRow oParentRow)
        {
            logger.Trace("AddKey(FunctionKeysRow oParentRow) - " + clsPOSDBConstants.Log_Entering);
            try
            {                
                LoadData();
                ApplyGrigFormat();
                isAddKey = true;
                this.grdDetail.Rows.Band.AddNew();
                grdDetail.Update();
                this.grdDetail.PerformAction(UltraGridAction.LastRowInGrid);
                this.grdDetail.ActiveRow = this.grdDetail.Rows[this.grdDetail.Rows.Count-1];
                ChangePosition(0, "Main");
                if (oParentRow != null)
                {
                    this.grdDetail.Rows[this.grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Value = oParentRow.KeyId;
                    isCallFromTrans = true;
                    ChangePosition(0, "Sub");
                    this.grdDetail.Rows[this.grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value = clsPOSDBConstants.FunctionKeys_Type_Item;
                }
                else
                {
                    this.grdDetail.Rows[this.grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value = clsPOSDBConstants.FunctionKeys_Type_Parent;
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "AddKey(FunctionKeysRow oParentRow)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            logger.Trace("AddKey(FunctionKeysRow oParentRow) - " + clsPOSDBConstants.Log_Exiting);
        }

        public void EditKey(FunctionKeysRow oParentRow)
        {
            logger.Trace("EditKey(FunctionKeysRow oParentRow) - " + clsPOSDBConstants.Log_Entering);
            try
            {
                LoadData();
                ApplyGrigFormat();
                isAddKey = true;
                int RowIndex = 0;
                foreach (UltraGridRow row in grdDetail.Rows)
                {
                    if (POS_Core.Resources.Configuration.convertNullToInt(row.Cells[clsPOSDBConstants.FunctionKeys_Fld_KeyId].Value) == oParentRow.KeyId)
                    {
                        grdDetail.ActiveRow = grdDetail.Rows[RowIndex];
                        break;
                    }
                    RowIndex++;
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "EditKey(FunctionKeysRow oParentRow)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            logger.Trace("EditKey(FunctionKeysRow oParentRow) - " + clsPOSDBConstants.Log_Exiting);
        }

        private void grdDetail_AfterRowInsert(object sender, RowEventArgs e)
        {
            try
            {
                ApplyGrigFormat();
                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Activation = Activation.AllowEdit;
                this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activation = Activation.AllowEdit;
                if (isCallFromTrans == true)
                {
                    this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Activate();
                }
                else
                {
                    this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Activate();
                }
                this.grdDetail.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                this.grdDetail.Refresh();
                m_ExceptionOccured = false;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "grdDetail_AfterRowInsert(object sender, RowEventArgs e)");
            }
        }


        private void grdDetail_BeforeExitEditMode(object sender, BeforeExitEditModeEventArgs e)
        {
           
        }

        private void grdDetail_AfterRowActivate(object sender, EventArgs e)
        {
           
        }

        private void grdDetail_AfterRowUpdate(object sender, RowEventArgs e)
        {
            try
            {
                grdDetail.Update();
                if (string.IsNullOrEmpty(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value.ToString()))
                {
                    return;
                }
                oFunctionKeys.Persist(oFunctionKeysData);
                LoadParents();
                grdDetail.Refresh();

            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "grdDetail_AfterRowUpdate(object sender, RowEventArgs e)");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void grdDetail_AfterCellActivate(object sender, EventArgs e)
        {
            try
            {
                if (grdDetail.ActiveRow == null)
                {
                    return;
                }
                    PrevMainPosValue = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value);
                    PrevSubPosValue = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Value);
                
                if (this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value.ToString() == "P")
                {
                    grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                    grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
                    grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].CellActivation = Activation.AllowEdit;
                }
                else
                {
                    grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                    grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
                    grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].CellActivation = Activation.AllowEdit;
                }
            }
            catch { }
        }

        private void grdDetail_AfterEnterEditMode(object sender, EventArgs e)
        {
           
        }

        private void grdDetail_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (grdDetail.ActiveRow == null && this.grdDetail.ActiveCell == null)
                {
                    return;
                }
                else if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_SubPosition)
                {
                    if (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Activation == Activation.AllowEdit)
                    {
                        int curval = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Value);
                        ChangePosition(curval, "Sub");
                    }
                }
                else if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_MainPosition)
                {
                    if (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Activation == Activation.AllowEdit)
                    {
                        int curval = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value);
                        ChangePosition(curval, "Main");
                    }
                }
                else if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.Item_Fld_Description)
                {
                    if (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Activation == Activation.AllowEdit)
                    {
                        int curval = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Value);
                        ChangePosition(curval, "Main");
                    }
                }
                else if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_Parent)
                {
                    if (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Activation == Activation.AllowEdit)
                    {
                        int curval = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Value);
                        ChangePosition(curval, "Sub");
                    }
                    if (this.grdDetail.ActiveCell.DataChanged == true)
                    {
                        string ItemId = Configuration.convertNullToString(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value);
                        if (string.IsNullOrEmpty(ItemId) == false && grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].DataChanged == true)
                        {
                            int ParentID = Configuration.convertNullToInt(grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Value);
                            if (ValidateKey(ItemId, ParentID) == false)
                            {
                                clsUIHelper.ShowErrorMsg("Item# " + ItemId + " is already assinged to parent\" " + grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Text + "\".");
                            }
                        }
                    }
                }
                else if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_FunctionType)
                {
                    switch (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value.ToString())
                    {
                        case "I":
                            grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Activation = Activation.AllowEdit;
                            grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Activation = Activation.AllowEdit;
                            if (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Activation == Activation.AllowEdit)
                            {
                                int curval = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value);
                                ChangePosition(curval, "Main");
                                curval = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value);
                                ChangePosition(curval, "Main");
                                grdDetail.Update();
                                grdDetail.Refresh();
                                grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                                grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
                                grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].CellActivation = Activation.AllowEdit;
                            }
                            string ItemId = Configuration.convertNullToString(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value);
                            if (string.IsNullOrEmpty(ItemId) == false)
                            {
                                int ParentID = Configuration.convertNullToInt(grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Value);
                                if (ValidateKey(ItemId, ParentID) == false)
                                {

                                    clsUIHelper.ShowErrorMsg("Item# " + ItemId + " is already assinged to parent\" " + grdDetail.Rows[grdDetail.ActiveRow.Index].Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Text + "\".");
                                }
                            }
                            this.grdDetail.ActiveCell = this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent];
                            this.grdDetail.ActiveCell.Activate();
                            this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                            break;
                        case "P":
                            grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Activation = Activation.Disabled;
                            grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Activation = Activation.Disabled;
                            grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Activation = Activation.Disabled;
                            if (Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Value) > 0)
                            {
                                grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Parent].Value = DBNull.Value;
                                grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value = DBNull.Value;
                                grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_SubPosition].Value = DBNull.Value;
                                grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value = DBNull.Value;
                            }
                            if (Configuration.convertNullToString(grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Value) != "")
                            {
                                grdDetail.ActiveRow.Cells[clsPOSDBConstants.Item_Fld_Description].Value = DBNull.Value;
                                grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value = DBNull.Value;
                            }
                            if (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Activation == Activation.AllowEdit)
                            {
                                int curval = Configuration.convertNullToInt(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_MainPosition].Value);
                                ChangePosition(curval, "Main");
                                grdDetail.Update();
                                grdDetail.Refresh();
                            }
                            grdDetail.Update();
                            grdDetail.Refresh();
                            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
                            grdDetail.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.FunctionKeys_Fld_Operation].CellActivation = Activation.AllowEdit;
                            this.grdDetail.ActiveCell = this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation];
                            this.grdDetail.ActiveCell.Activate();
                            this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                            break;
                    }
                }
                if (string.IsNullOrEmpty(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation].Value.ToString()) == false)
                {
                    this.grdDetail.UpdateData();
                    oFunctionKeysData.AcceptChanges();
                    grdDetail.Update();
                    oFunctionKeys.Persist(oFunctionKeysData);
                    LoadParents();
                    grdDetail.Refresh();
                }
                oFunctionKeysData.AcceptChanges();
            }
            catch { }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            AddKey(null);
            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Activation = Activation.AllowEdit;
            this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Activate();
            this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
        }

        private void DeleteRow()
        {
             string KeyId  = string.Empty;
            try 
            {
                if (this.grdDetail.Selected.Rows.Count == 0)
                {
                    clsUIHelper.ShowErrorMsg("Please select the record you want to delete.");
                    return;
                }
                else if(string.IsNullOrEmpty(grdDetail.Selected.Rows[0].Cells[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Text) == false)
                {
                    KeyId = grdDetail.Selected.Rows[0].Cells[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Text;
                }
                else
                {
                    KeyId = grdDetail.Selected.Rows[0].Cells[clsPOSDBConstants.FunctionKeys_Fld_KeyId].Text;
                }
                FunKeyCommonOperations.RemoveKey(KeyId);
                clsUIHelper.ShowOKMsg("Record deleted successfully.");
                LoadData();
            }
            catch(Exception Ex) 
            {
                logger.Fatal(Ex, "DeleteRow()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRow();
        }

        private void grdDetail_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
           
        }

        private void frmFunctionKeys_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void frmFunctionKeys_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F11 && e.Shift == false && e.Control == false && e.Alt == false)
                {
                    DeleteRow();
                }
                else if (e.KeyCode == Keys.F2 && e.Shift == false && e.Control == false && e.Alt == false)
                {
                    AddKey(null);
                    this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Activation = Activation.AllowEdit;
                    this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Activate();
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "frmFunctionKeys_KeyDown(object sender, KeyEventArgs e)");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }

        }

        private void grdDetail_BeforeRowDeactivate(object sender, CancelEventArgs e)
        {
            
        }

        private void grdDetail_BeforeCellDeactivate(object sender, CancelEventArgs e)
        {
            UltraGridRow oCurrentRow;
            UltraGridCell oCurrentCell = null;
            string strErrorMesage = string.Empty;
            try
            {
                if (m_ExceptionOccured == true)
                {
                    return;
                }
                if (grdDetail.ActiveRow == null || this.grdDetail.ActiveCell == null)
                {
                    return;
                }
                oCurrentRow = grdDetail.ActiveRow;
                oCurrentCell = null;
               
                if (this.grdDetail.ActiveCell.Column.Key != clsPOSDBConstants.FunctionKeys_Fld_Operation)
                {
                    return;
                }
                if (string.IsNullOrEmpty(this.grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Value.ToString()) == false)
                {
                    return;
                }
                oCurrentCell = oCurrentRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_Operation];
                if (grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunctionType].Value.ToString() == "I")
                {
                    strErrorMesage = sAssignItemError;
                }
                else
                {
                    strErrorMesage = sTypeValueError;
                }
                if (this.grdDetail.ActiveCell.Column.Key == clsPOSDBConstants.FunctionKeys_Fld_Operation && oCurrentCell.Value.ToString() == "")
                {
                    throw new Exception(strErrorMesage);
                    
                }
                if (oCurrentCell.DataChanged == false)
                {
                    return;
                }
                grdDetail.ActiveCell = oCurrentCell;
                if (string.IsNullOrEmpty(oCurrentCell.Text.ToString()) && string.IsNullOrEmpty(grdDetail.ActiveRow.Cells[clsPOSDBConstants.FunctionKeys_Fld_FunKey].Text))
                {
                    throw new Exception(strErrorMesage);
                }
            }
            catch (Exception exp)
            {
                if (exp.Message.ToString().ToUpper().Trim() != sAssignItemError.Trim().ToUpper() && exp.Message.ToString().ToUpper().Trim() != sTypeValueError.Trim().ToUpper())    //PRIMEPOS-2584 24-Sep-2018 JY Added 
                {
                    logger.Fatal(exp, "grdDetail_BeforeCellDeactivate(object sender, CancelEventArgs e)");
                }
                clsUIHelper.ShowErrorMsg(exp.Message);
                if (oCurrentCell != null)
                {
                    this.grdDetail.ActiveCell = oCurrentCell;
                    this.grdDetail.PerformAction(UltraGridAction.ActivateCell);
                    this.grdDetail.PerformAction(UltraGridAction.EnterEditMode);
                }
                e.Cancel = true;
            }
        }

    }
}
