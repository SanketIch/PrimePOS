using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;

namespace POS_Core_UI
{
    public partial class frmCSItems : Form
    {
        private bool isCancel = false;
        private int iQty = 0;
        private bool isLoaded = false;
        DataSet dsPacketInfo = null;
        private int iOldQty = 0;
        private int iSoldItemQty = 0;

        public bool IsCancel
        {
            get { return isCancel; }        
        }

        public int Qty
        {
            get { return iQty; }
            set { iQty = value; }
        }

        public frmCSItems(string ItemID,int soldItemQty)
        {
            InitializeComponent();
            dsPacketInfo = new DataSet();
            GetData(ItemID);
            iSoldItemQty = soldItemQty;
        }

        private void GetData(string itemId)
        {
            Item item = new Item();
            dsPacketInfo  = item.GetPacketInfo(itemId);            
        }       

        private void btnClose_Click(object sender, EventArgs e)
        {
            isCancel = true;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }       

        private void frmCSItems_Load(object sender, EventArgs e)
        {
            int pckSize=0;
            if(dsPacketInfo != null &&  dsPacketInfo.Tables.Count>0 &&  dsPacketInfo.Tables[0].Rows.Count >0)
            {
                txtItemCode.Text = dsPacketInfo.Tables[0].Rows[0][clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                txtItemDescription.Text = dsPacketInfo.Tables[0].Rows[0][clsPOSDBConstants.POHeader_Fld_Description].ToString();
                txtPacketSize.Text = dsPacketInfo.Tables[0].Rows[0][clsPOSDBConstants.PODetail_Fld_PackSize].ToString();
                txtPacketQuantity.Text = dsPacketInfo.Tables[0].Rows[0][clsPOSDBConstants.PODetail_Fld_PackQuant].ToString();
                iOldQty = Qty;
                txtPacketToOrder.Value = Qty;
                if(txtPacketSize.Text!=string.Empty)
                {
                    pckSize = Convert.ToInt32(txtPacketSize.Text.Trim());
                }
                int QtyHand=(pckSize * Qty) + Convert.ToInt32(dsPacketInfo.Tables[0].Rows[0][clsPOSDBConstants.PODetail_Fld_QtyInHand].ToString().Trim());
                txtQtyInHand.Text = QtyHand.ToString();
                int unitToOrd = (pckSize * Qty);
                txtUnitQntOrd.Text = unitToOrd.ToString();
                int ItemSold = iSoldItemQty;
                int pckQty = 0;
                int itemQty = 0;
                if (ItemSold>0)
                {
                    if (Convert.ToInt32(txtPacketSize.Text) > 0)
                    {
                        pckQty = ItemSold / Convert.ToInt32(txtPacketSize.Text);
                        itemQty = ItemSold % Convert.ToInt32(txtPacketSize.Text);
                        txtPacketQuantitySold.Text = pckQty.ToString();
                        txtItemQuantitySold.Text = itemQty.ToString();
                    }
                }
                
            }
        }
       
        private void txtPacketToOrder_ValueChanged(object sender, EventArgs e)
        {
            int iQtyInHand = 0;
            int unitToOrd = 0;
            iQtyInHand = MMSUtil.UtilFunc.ValorZeroI(txtQtyInHand.Text);
            iQty = MMSUtil.UtilFunc.ValorZeroI(txtPacketToOrder.Value.ToString());
            unitToOrd = iQty * MMSUtil.UtilFunc.ValorZeroI(txtPacketSize.Text);
            if (iQty > iOldQty)
            {
                iQtyInHand = iQtyInHand + MMSUtil.UtilFunc.ValorZeroI(txtPacketSize.Text);
                txtQtyInHand.Text = iQtyInHand.ToString();       
            }
            else if (iQty < iOldQty)
            {
                iQtyInHand = iQtyInHand - MMSUtil.UtilFunc.ValorZeroI(txtPacketSize.Text);
                txtQtyInHand.Text = iQtyInHand.ToString();  
            }
            txtUnitQntOrd.Text = unitToOrd.ToString();
            iOldQty = iQty; 
        }

        private void ultraLabel1_Click(object sender, EventArgs e)
        {

        }

        private void ultraTextEditor1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ultraLabel4_Click(object sender, EventArgs e)
        {

        }

        private void ultraLabel5_Click(object sender, EventArgs e)
        {

        }
    }
}