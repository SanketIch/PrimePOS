using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmCopyMMSOpts : Form
    {
        private ItemData oItemData = new ItemData();
        private DataSet dsMMS = new DataSet();
        public bool bUpdate = false;
        public frmCopyMMSOpts()
        {
            InitializeComponent();
        }

        public frmCopyMMSOpts(DataSet ds, ItemData objItemData)
        {
            InitializeComponent();
            oItemData = objItemData;
            dsMMS = ds;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {            
            if (chkDescription.Checked == true && txtDescMMS.Text.Trim() != txtDescCurr.Text.Trim())
            {
                oItemData.Item[0].Description = dsMMS.Tables[0].Rows[0]["Description"].ToString();
                bUpdate = true;
            }
            if (chkProductCode.Checked == true && txtProductCodeMMS.Text.Trim() != txtProductCodeCurr.Text.Trim())
            {
                oItemData.Item[0].ProductCode = dsMMS.Tables[0].Rows[0]["ProductCode"].ToString();
                bUpdate = true;
            }
            if (chkSeasonCode.Checked == true && txtSeasonCodeMMS.Text.Trim() != txtSeasonCodeCurr.Text.Trim())
            {
                oItemData.Item[0].SeasonCode = dsMMS.Tables[0].Rows[0]["SeasonCode"].ToString();
                bUpdate = true;
            }
            if (chkManufacturer.Checked == true && txtManufacturerMMS.Text.Trim() != txtManufacturerCurr.Text.Trim())
            {
                oItemData.Item[0].ManufacturerName = dsMMS.Tables[0].Rows[0]["ManufacturerName"].ToString();
                bUpdate = true;
            }
            //if (chkSellingPrice.Checked == true && Configuration.convertNullToDecimal(numSellingPriceMMS.Value) != Configuration.convertNullToDecimal(numSellingPriceCurr.Value))
            //{
            //    oItemData.Item[0].SellingPrice = Configuration.convertNullToDecimal(dsMMS.Tables[0].Rows[0]["SellingPrice"]);
            //    bUpdate = true;
            //}
            if (chkPacketSize.Checked == true && txtPacketSizeMMS.Text.Trim() != txtPacketSizeCurr.Text.Trim())
            {
                oItemData.Item[0].PckSize = dsMMS.Tables[0].Rows[0]["PCKSIZE"].ToString();
                bUpdate = true;
            }
            if (chkPacketQty.Checked == true && txtPacketQtyMMS.Text.Trim() != txtPacketQtyCurr.Text.Trim())
            {
                oItemData.Item[0].PckQty = dsMMS.Tables[0].Rows[0]["PCKQTY"].ToString();
                bUpdate = true;
            }
            if (chkPacketUnit.Checked == true && txtPacketUnitMMS.Text.Trim() != txtPacketUnitCurr.Text.Trim())
            {
                oItemData.Item[0].PckUnit = dsMMS.Tables[0].Rows[0]["PCKUNIT"].ToString();
                bUpdate = true;
            }

            if (bUpdate == true)
            {
                ItemSvr oItemSvr = new ItemSvr();
                oItemSvr.Persist(oItemData);
                Resources.Message.Display("Data updated successfully", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void frmCopyMMSOpts_Shown(object sender, EventArgs e)
        {
            txtDescMMS.Text = dsMMS.Tables[0].Rows[0]["Description"].ToString();
            txtProductCodeMMS.Text = dsMMS.Tables[0].Rows[0]["ProductCode"].ToString();
            txtSeasonCodeMMS.Text = dsMMS.Tables[0].Rows[0]["SeasonCode"].ToString();
            txtManufacturerMMS.Text = dsMMS.Tables[0].Rows[0]["ManufacturerName"].ToString();
            //numSellingPriceMMS.Value = Configuration.convertNullToDecimal(dsMMS.Tables[0].Rows[0]["SellingPrice"]);
            txtPacketSizeMMS.Text = dsMMS.Tables[0].Rows[0]["PCKSIZE"].ToString();
            txtPacketQtyMMS.Text = dsMMS.Tables[0].Rows[0]["PCKQTY"].ToString();
            txtPacketUnitMMS.Text = dsMMS.Tables[0].Rows[0]["PCKUNIT"].ToString();

            txtDescCurr.Text = oItemData.Item[0].Description;
            txtProductCodeCurr.Text = oItemData.Item[0].ProductCode;
            txtSeasonCodeCurr.Text = oItemData.Item[0].SeasonCode;
            txtManufacturerCurr.Text = oItemData.Item[0].ManufacturerName;
            //numSellingPriceCurr.Value = Configuration.convertNullToDecimal(oItemData.Item[0].SellingPrice);
            txtPacketSizeCurr.Text = oItemData.Item[0].PckSize;
            txtPacketQtyCurr.Text = oItemData.Item[0].PckQty;
            txtPacketUnitCurr.Text = oItemData.Item[0].PckUnit;
        }
    }
}
