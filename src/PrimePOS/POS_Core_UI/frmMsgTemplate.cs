using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS_Core_UI.Layout;
using NLog;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
    public partial class frmMsgTemplate : frmMasterLayout
    {
        #region Decalaration
        private ILogger logger = LogManager.GetCurrentClassLogger();
        public bool IsCanceled = true;
        public int MessageCategoryId = 0;
        public int MessageTypeId = 0;
        private bool isInEditMode;
        public bool AddStatus = false;

        private MsgTemplate oMsgTemplate = new MsgTemplate();
        private MsgTemplateData oMsgTemplateData = new MsgTemplateData();
        private MsgTemplateRow oMsgTemplateRow;
        #endregion

        public frmMsgTemplate()
        {
            InitializeComponent();
            try
            {
                setChildControlProperties(this);
                clsUIHelper.SetHeader(this, this.Name);
                PopulateMessageCategory();
                Initialize();
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "frmMsgTemplate()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }

        private void Initialize()
        {
            oMsgTemplate = new MsgTemplate();
            oMsgTemplateData = new MsgTemplateData();
            this.Text = "Add Message Template";
            this.lblTransactionType.Text = "Add Message Template";
            Clear();
            oMsgTemplateRow = oMsgTemplateData.MsgTemplate.AddRow(0, "", "", "", MessageCategoryId, MessageTypeId);
        }

        private void Clear()
        {
            txtMessageTitle.Text = "";
            cboMessageCategory.SelectedIndex = -1;
            txtMessageSubject.Text = "";
            this.opnMessageType.Items[0].CheckState = CheckState.Checked;
            txtMessage.Text = "";
        }

        private void PopulateMessageCategory()
        {
            this.cboMessageCategory.DataSource = Enum.GetValues(typeof(eMessageCategory)).Cast<eMessageCategory>()
                                            .Where(p => p != eMessageCategory.None).ToArray<eMessageCategory>();
        }

        private void frmMsgTemplate_Load(object sender, EventArgs e)
        {
            logger.Trace("frmMsgTemplate_Load() - " + clsPOSDBConstants.Log_Entering);

            this.txtMessageTitle.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtMessageTitle.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtMessageSubject.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtMessageSubject.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.txtMessage.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.txtMessage.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            IsCanceled = true;

            cboMessageCategory.Value = MessageCategoryId;
            if (MessageTypeId == 0) //Email
                this.opnMessageType.Items[0].CheckState = CheckState.Checked;

            this.ActiveControl = this.txtMessageTitle;
            logger.Trace("frmMsgTemplate_Load() - " + clsPOSDBConstants.Log_Exiting);
        }

        public bool Edit(int RecID)
        {
            bool bStatus = true;
            try
            {
                isInEditMode = true;
                this.txtMessageTitle.Enabled = false;
                this.cboMessageCategory.Enabled = false;
                this.opnMessageType.Enabled = false;
                oMsgTemplateData = oMsgTemplate.Populate(RecID);
                oMsgTemplateRow = oMsgTemplateData.MsgTemplate.GetRowByID(RecID);
                if (oMsgTemplateData != null && oMsgTemplateData.MsgTemplate != null && oMsgTemplateData.MsgTemplate.Rows.Count > 0)
                {
                    oMsgTemplateRow = (MsgTemplateRow)oMsgTemplateData.MsgTemplate.Rows[0];
                    this.Text = "Add Message Template";
                    this.lblTransactionType.Text = "Add Message Template";
                    if (oMsgTemplateRow != null)
                        Display();
                }
                else
                {
                    Resources.Message.Display("Record not found","PrimePOS",MessageBoxButtons.OK);
                    IsCanceled = false;
                    this.Close();
                    bStatus = false;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            return bStatus;
        }

        private void Display()
        {
            this.txtMessageTitle.Text = oMsgTemplateRow.MessageCode;
            this.txtMessageSubject.Text = oMsgTemplateRow.MessageSub;
            this.txtMessage.Text = oMsgTemplateRow.Message;
            cboMessageCategory.Value = MessageCategoryId;
            if (MessageTypeId == 0) //Email
                this.opnMessageType.Items[0].CheckState = CheckState.Checked;
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            AssignValues();
            if (!ValidateFields()) return;

            if (Save())
            {
                if (isInEditMode == false)
                    AddStatus = true;

                IsCanceled = false;
                this.Close();
            }
        }

        private void AssignValues()
        {
            oMsgTemplateRow.MessageCode = this.txtMessageTitle.Text;
            oMsgTemplateRow.MessageSub = this.txtMessageSubject.Text;
            oMsgTemplateRow.Message = this.txtMessage.Text;

            if (string.IsNullOrEmpty(cboMessageCategory.Text) == false)
                oMsgTemplateRow.MessageCatId = (int)(cboMessageCategory.Value);
            else
                oMsgTemplateRow.MessageCatId = -1;

            oMsgTemplateRow.MessageTypeId = Configuration.convertNullToInt(opnMessageType.Value);
        }

        private bool ValidateFields()
        {
            Boolean nStatus = true;
            try
            {
                logger.Trace("ValidateFields() - " + clsPOSDBConstants.Log_Entering);

                if (string.IsNullOrWhiteSpace(txtMessageTitle.Text))
                {
                    errorProvider.SetIconPadding(txtMessageTitle, 2);
                    errorProvider.SetError(txtMessageTitle, "Message title cannot be blank.");
                    nStatus = false;
                }
                if (string.IsNullOrWhiteSpace(txtMessage.Text))
                {
                    errorProvider.SetIconPadding(txtMessage, 2);
                    errorProvider.SetError(txtMessage, "Message cannot be blank.");
                    nStatus = false;
                }
                if(cboMessageCategory.SelectedIndex == -1)
                {
                    errorProvider.SetIconPadding(cboMessageCategory, 2);
                    errorProvider.SetError(cboMessageCategory, "Please select Message Category");
                    nStatus = false;
                }
                logger.Trace("ValidateFields() - " + clsPOSDBConstants.Log_Exiting);
                return nStatus;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ValidateFields()");
                return false;
            }
        }

        private bool Save()
        {
            try
            {
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Entering);
                oMsgTemplate.Persist(oMsgTemplateData);
                logger.Trace("Save() - " + clsPOSDBConstants.Log_Exiting);
                return true;
            }
            catch (POSExceptions exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.ErrMessage);
                //switch (exp.ErrNumber)
                //{
                //    case (long)POSErrorENUM.Customer_CodeCanNotBeNULL:
                //        txtCustomerCode.Focus();
                //        break;
                //}
                return false;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "Save()");
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            IsCanceled = true;
            this.Close();
        }

        
    }
}
