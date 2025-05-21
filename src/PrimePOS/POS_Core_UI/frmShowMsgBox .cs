using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.CommonData;

namespace POS_Core_UI
{
    public partial class frmShowMsgBox : Form
    {
        private string saveAndCloseOrder = string.Empty;

        /// <summary>
        /// Author: Ritesh Parekh
        /// Date: 19
        /// </summary>
        public bool IsCancelled
        {
            get
            {
                if (saveAndCloseOrder == clsPOSDBConstants.PO_Cancel)
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
        }
        public frmShowMsgBox()
        {
            InitializeComponent();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            saveAndCloseOrder = clsPOSDBConstants.PO_Save;
            
            this.Close(); 
        }
        public string SaveOrCloseOrder
        {
            get
            {
                return this.saveAndCloseOrder; 
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            saveAndCloseOrder = clsPOSDBConstants.PO_Cancel;
             this.Close(); 
        }
        private void btnSaveOrClose_Click(object sender, EventArgs e)
        {
            saveAndCloseOrder = clsPOSDBConstants.PO_SaveClose;
            this.Close(); 
        }
        private void frmShowMsgBox_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
        }
        private void btnDiscardChanges_Click(object sender, EventArgs e)
        {
            saveAndCloseOrder = clsPOSDBConstants.PO_DiscardChanges;
            this.Close(); 
        }

        private void frmShowMsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.S:
                    {
                        if (e.Control)
                        {
                            btSave_Click(this, new EventArgs());
                        }
                        break;
                    }
                case System.Windows.Forms.Keys.U:
                    {
                        if (e.Control)
                        {
                            btnSaveOrClose_Click(this, new EventArgs());
                        }
                        break;
                    }
                case System.Windows.Forms.Keys.D:
                    {
                        if (e.Control)
                        {
                            btnDiscardChanges_Click(this, new EventArgs());
                        }
                        break;
                    }
            }
        }

    }
}