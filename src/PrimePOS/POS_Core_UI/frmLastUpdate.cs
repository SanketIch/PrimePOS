using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using System.IO;
using System.Collections;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
using System.Timers;

namespace POS_Core_UI
{
   
    public partial class frmLastUpdate : Form
    {
        #region private 
        Dictionary<String, FileInfo> vendorFileDict = new Dictionary<string, FileInfo>();
        DataSet vendorDs = new DataSet();
        DataSet itemInfoDataset = new DataSet();
        private string vendorCode = string.Empty;
        FileInfo[] rgFiles = null;
        #endregion

        #region constant
        private const string LASTUPDATE = "_LastUpdate.txt";
        private const string ITEMNUMBER = "Item Number";
        private const string ITEMDESCRIPTION = "Item Description";        
        private const string OLDCOSTPRICE = "Old Cost Price";
        private const string NEWCOSTPRICE  = "New Cost Price";
        private const string OLDSELLINGPRICE = "Old Selling Price";
        private const string NEWSELLINGPRICE = "New Selling Price";
        private const string ISDISCONTINUED = "IsDiscotinued";
        private const int ITEMCOUNT = 8;
        #endregion

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        int nDisplayItemCost = 0;
        System.Timers.Timer tmrBlinking;
        private long iBlinkCnt = 0;
        #endregion

        public frmLastUpdate()
        {
            InitializeComponent();
            LoadVendorList();        
        }

        
        private void LoadVendorList()
        {
            try
            {
                itemInfoDataset = new DataSet();
                cboVendorList.Items.Clear();

                POS_Core.BusinessRules.Vendor vendor = new POS_Core.BusinessRules.Vendor();
                VendorData vendData = vendor.PopulateList(" Where IsActive ='1'");

                string filePath = GetInOutDirectory();
                string fileName = string.Empty;

                DirectoryInfo di = new DirectoryInfo(filePath + "\\");

                if (vendData.Tables.Count > 0 && vendData.Tables[0].Rows.Count > 0)
                {
                    //Added by Abhishek(SRT) -- 07/09/2009
                    //Added to sort the data by vendor 
                    DataRow[] vendorRows = vendData.Tables[0].Select("", "VendorCode ASC");
                    //End of Added by Abhishek(SRT) 

                    foreach (VendorRow vendRow in vendorRows)
                    {
                        cboVendorList.Items.Add(vendRow.Vendorcode);
                    }
                }

                cboVendorList.SelectedIndex = 0;
                fileName = cboVendorList.SelectedItem.DisplayText + LASTUPDATE;
                rgFiles = di.GetFiles(fileName, SearchOption.TopDirectoryOnly);
                if (rgFiles.Length > 0)
                    lblTimeStamp.Text = rgFiles[0].LastWriteTime.ToString();
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }
        }
   

        private string GetInOutDirectory()
        {
            return Application.StartupPath;
        }


        private FileInfo[] GetFilesList(string path, string reqPatterns)
        {
            
            FileInfo[] rgFiles = null;

            try
            {
                //get the path from local
                DirectoryInfo di = new DirectoryInfo(path+"\\");

                // get files from local path of required pattern 
                rgFiles = di.GetFiles("*" + reqPatterns,SearchOption.TopDirectoryOnly);         
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }           
            return rgFiles;
        }
        
     
        private void btnRefersh_Click(object sender, EventArgs e)
        {
           
            itemInfoDataset = null;
            itemInfoDataset = new DataSet();
            grdLastFileDetails.DataSource = null;
            LoadVendorList();
        }

        private void btnShowLastUpdate_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            DataColumn oCol1 = dataTable.Columns.Add(ITEMNUMBER);
            DataColumn oCol2 = dataTable.Columns.Add(ITEMDESCRIPTION);

            DataColumn oCol3 = dataTable.Columns.Add(OLDCOSTPRICE);
            DataColumn oCol4 = dataTable.Columns.Add(NEWCOSTPRICE);

            DataColumn oCol5 = dataTable.Columns.Add(OLDSELLINGPRICE);
            DataColumn oCol6 = dataTable.Columns.Add(NEWSELLINGPRICE);

            DataColumn oCol7 = dataTable.Columns.Add(ISDISCONTINUED);

            try
            {
                System.IO.StreamReader fs = new StreamReader(rgFiles[0].FullName);
                string fileLine = string.Empty;

                while(((fileLine = fs.ReadLine()) != null))
                {
                    string[] split = fileLine.Split(new char[] { '|' });
                    DataRow row = dataTable.NewRow();

                    if (split.Length == ITEMCOUNT)
                    {
                        row[ITEMNUMBER] = split[0].ToString();
                        row[ITEMDESCRIPTION] = split[1].ToString();
                        row[OLDCOSTPRICE] = split[2];
                        row[NEWCOSTPRICE] = split[3];
                        row[OLDSELLINGPRICE] = split[4];
                        row[NEWSELLINGPRICE] = split[5];
                        row[ISDISCONTINUED] = split[6];
                        dataTable.Rows.Add(row);
                    }                   
                }
                itemInfoDataset.Merge(dataTable);
                grdLastFileDetails.DataSource = itemInfoDataset;

                #region PRIMEPOS-2464 06-Mar-2020 JY Added
                if (nDisplayItemCost == 0)
                {
                    grdLastFileDetails.DisplayLayout.Bands[0].Columns[OLDCOSTPRICE].Hidden = true;
                    grdLastFileDetails.DisplayLayout.Bands[0].Columns[NEWCOSTPRICE].Hidden = true;
                }
                else
                {
                    grdLastFileDetails.DisplayLayout.Bands[0].Columns[OLDCOSTPRICE].Hidden = false;
                    grdLastFileDetails.DisplayLayout.Bands[0].Columns[NEWCOSTPRICE].Hidden = false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }

        }

        private void cboVendorList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string filePath = GetInOutDirectory();
            string fileName = string.Empty;
            DirectoryInfo di = new DirectoryInfo(filePath + "\\");

            try
            {
                itemInfoDataset = null;
                itemInfoDataset = new DataSet();
                grdLastFileDetails.DataSource = null;
                vendorCode = cboVendorList.SelectedItem.DisplayText;
                fileName = vendorCode + LASTUPDATE;
                rgFiles = di.GetFiles(fileName, SearchOption.TopDirectoryOnly);
                lblTimeStamp.Text = rgFiles[0].LastWriteTime.ToString();
            }
            catch (Exception ex)
            {
                ErrorHandler.logException(ex, "", "");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLastUpdate_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);

            #region PRIMEPOS-2464 10-Mar-2020 JY Added
            nDisplayItemCost = Configuration.convertBoolToInt(UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID));   //PRIMEPOS-2464 09-Mar-2020 JY Added
            if (nDisplayItemCost == 0)
            {
                lblMessage.Visible = true;
                tmrBlinking = new System.Timers.Timer();
                tmrBlinking.Interval = 1000;//1 seconds
                tmrBlinking.Elapsed += new ElapsedEventHandler(tmrBlinkingTimedEvent);
                tmrBlinking.Enabled = true;
            }
            else
            {
                this.Height -= 15;
            }
            #endregion
        }

        #region PRIMEPOS-2464 10-Mar-2020 JY Added
        public void tmrBlinkingTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                iBlinkCnt++;
                if (iBlinkCnt % 4 == 0)
                    lblMessage.Appearance.ForeColor = Color.Transparent;
                else
                    lblMessage.Appearance.ForeColor = Color.Red;
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
    }
}