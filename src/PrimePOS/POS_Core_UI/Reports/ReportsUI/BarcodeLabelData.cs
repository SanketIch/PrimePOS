using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using POS_Core.Resources;
using POS_Core_UI.UI;
using POS_Core.ErrorLogging;
////using POS_Core.TaxType;
namespace POS_Core.LabelHandler
{
    public class BarcodeLabelData
    {
        public BarcodeLabelData()
        {
        }

        public string ItemCode;
        public string ItemName;
        public decimal SellingPrice;
        public Image BarCode;
        public string ItemCode2;
        public bool IsItemFSA;
        public string Location;
        public int FineLineCode;
        public string VendorCode; //Added by SRT (Abhishek D ) Date : 04 April 2010
        public string VendorID;
        public string PckSize;
        public string PckQty;
        public string PckUnit;
        public ItemRow Item;
        //Added by Amit Date 
        public string VendorItemID;
        public Image VendorItemIDBarCode;
        public string DesireQuantity;//Added By Shitaljit on 10 April 2013 for PRIMEPOS-776
        //From Here added by Ravindra
        public string ManufacturerName;
        public string AvgPrice;
        public string ProductCode;  //SKU Code
        public string OnSalePrice;
        #region Sprint-21 - 08-Sep-2015 JY Added sale price related objects
        public bool IsOnSale;
        public int SaleLimitQty;
        public DateTime SaleStartDate;
        public DateTime SaleEndDate;
        #endregion
    }
}
