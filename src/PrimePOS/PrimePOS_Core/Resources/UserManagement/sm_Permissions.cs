using System.Data;
using System.Reflection;

namespace POS_Core.UserManagement
{
    /// <summary>
    /// Summary description for sm_Permissions.
    /// </summary>
    /// max id = 40
    public class sm_Permissions
    {
        private DataTable dt;

        public sm_Permissions()
        {
            //
            // TODO: Add constructor logic here
            //

            //			dt.Rows.Add(new object[] {1,"POS Transaction"});
            //			dt.Rows.Add(new object[] {2,"Inventory Management"});
            //			dt.Rows.Add(new object[] {3,"Administrative Functions"});
            //			dt.Rows.Add(new object[] {4,"Application Settings"});
        }

        public DataTable Permissions
        {
            get
            {
                dt = new DataTable("Permissions");

                dt.Columns.Add("PmID", typeof(int));
                dt.Columns.Add("PmName", typeof(string));
                dt.Columns.Add("ScreenID", typeof(int));
                dt.Columns.Add("EntryScreen", typeof(bool));
                dt.Columns.Add("SortOrder", typeof(int));

                foreach(PropertyInfo pInfo in this.GetType().GetProperties())
                {
                    if(pInfo.PropertyType == typeof(PermissionStruct))
                    {
                        PermissionStruct obj=null;
                        obj = (PermissionStruct) pInfo.GetValue(this, null);
                        dt.Rows.Add(new object[] { obj.ID, obj.Name, obj.ScreenID, obj.EntryScreen, obj.SortOrder });
                    }
                }
                dt.DefaultView.Sort = "PmName asc";
                dt = dt.DefaultView.ToTable();
                return this.dt;
            }
        }

        #region 1-permissions for postransaction

        public PermissionStruct POSTransaction
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 1;
                obj.Name = "Allow POS Transaction";
                obj.ScreenID = 1;
                obj.SortOrder = 1;
                return obj;
            }
        }

        public PermissionStruct ReturnTransaction
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 2;
                obj.Name = "Allow Return Transaction";
                obj.ScreenID = 1;
                obj.SortOrder = 2;
                return obj;
            }
        }

        public PermissionStruct NoSaleTrans
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 4;
                obj.Name = "Allow No Sale Trans";
                obj.ScreenID = 1;
                obj.SortOrder = 3;
                return obj;
            }
        }

        public PermissionStruct OnHoldTrans
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 5;
                obj.Name = "Allow Trans On-Hold";
                obj.ScreenID = 1;
                obj.SortOrder = 4;
                return obj;
            }
        }

        public PermissionStruct DeleteTrans
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 6;
                obj.Name = "Allow Transaction Deletion";
                obj.ScreenID = 1;
                obj.SortOrder = 5;
                return obj;
            }
        }

        public PermissionStruct ROA
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 7;
                obj.Name = "Receive On Account";
                obj.ScreenID = 1;
                obj.SortOrder = 6;
                return obj;
            }
        }

        public PermissionStruct DeleteItemfromPOSTrans
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 37;
                obj.Name = "Delete Item";
                obj.ScreenID = 1;
                obj.SortOrder = 7;
                return obj;
            }
        }

        public PermissionStruct DiscOverridefromPOSTrans
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 36;
                obj.Name = "Discount Override";
                obj.ScreenID = 1;
                obj.SortOrder = 8;
                return obj;
            }
        }

        public PermissionStruct PriceOverridefromPOSTrans
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 38;
                obj.Name = "Price Override";
                obj.ScreenID = 1;
                obj.SortOrder = 9;
                return obj;
            }
        }

        public PermissionStruct TaxOverride
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 68;
                obj.Name = "Tax Override For OTC";
                obj.ScreenID = 1;
                obj.SortOrder = 68;
                return obj;
            }
        }
        public PermissionStruct TaxOverrideAll
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 69;
                obj.Name = "Tax Override All For OTC";
                obj.ScreenID = 1;
                obj.SortOrder = 69;
                return obj;
            }
        }

        /// <summary>
        /// Author: Shitaljit added on 5 April 2013
        /// Security for Price Override Less Than Cost Price PRIMEPOS-669 Add Override option to apply item price less than cost
        /// </summary>
        public PermissionStruct PriceOverrideLessThanCostPrice
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 64;
                obj.Name = "Price Override Less Than Cost Price";
                obj.ScreenID = 1;
                obj.SortOrder = 64;
                return obj;
            }
        }

        //PRIMEPOS-3015 26-Oct-2021 JY Commented
        //public PermissionStruct DuplicateChargePosting
        //{
        //    get
        //    {
        //        PermissionStruct obj = new PermissionStruct();
        //        obj.ID = 66;
        //        obj.Name = "Allow Duplicate Charge Posting";
        //        obj.ScreenID = 1;
        //        obj.SortOrder = 66;
        //        return obj;
        //    }
        //}

        public PermissionStruct PriceOverrideForRXItemsfromPOSTrans
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 56;
                obj.Name = "Price Override RX Items";
                obj.ScreenID = 1;
                obj.SortOrder = 10;
                return obj;
            }
        }

        public PermissionStruct OverrideHouseChargeLimit
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 40;
                obj.Name = "Override HouseCharge Limit";
                obj.ScreenID = 1;
                obj.SortOrder = 40;
                return obj;
            }
        }

        public PermissionStruct AllowCashback
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 52;
                obj.Name = "Allow Cashback";
                obj.ScreenID = 1;
                obj.SortOrder = 52;
                return obj;
            }
        }

        //Added By shitaljit(QuicSolv) on 21 Oct 2011

        public PermissionStruct EditPayout
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 58;
                obj.Name = "Edit";
                obj.ScreenID = 2;
                obj.SortOrder = 58;
                return obj;
            }
        }

        public PermissionStruct DeletePayout
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 59;
                obj.Name = "Delete";
                obj.ScreenID = 2;
                obj.SortOrder = 59;
                return obj;
            }
        }

        //Added By shitaljit to control coupon payment by user rights
        public PermissionStruct MakeCouponPayment
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 62;
                obj.Name = "Make Coupon Payment";
                obj.ScreenID = 1;
                obj.SortOrder = 62;
                return obj;
            }
        }

        public PermissionStruct DeleteTransOnHold
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 63;
                obj.Name = "Allow Trans On-Hold Delete";
                obj.ScreenID = 1;
                obj.SortOrder = 63;
                return obj;
            }
        }

        //END of Added By shitaljit(QuicSolv) on 21 Oct 2011
        /// <summary>
        /// Author: Shitaljit added on 5 April 2013
        /// Security for Price Override Less Than Cost Price PRIMEPOS-669 Add Override option to apply item price less than cost
        /// </summary>
        public PermissionStruct OverrideMaxStationCloseCashLimit
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 65;
                obj.Name = "Override Max Station Close Cash Limit";
                obj.ScreenID = 1;
                obj.SortOrder = 65;
                return obj;
            }
        }

        /// <summary>
        /// Author: Manoj Kumar
        /// Description: Skip signature for F10 Transaction.
        /// </summary>
        public PermissionStruct F10SignSkip
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 67;
                obj.Name = "Skip Signature For F10 Transactions";
                obj.ScreenID = 1;
                obj.SortOrder = 67;
                return obj;
            }
        }

        /// <summary>
        /// Author: Sprint-23 - PRIMEPOS-2280 10-May-2016 JY Added 
        /// Description: "Create New Coupon" settings to control create new coupon behavior on transaction screen
        /// </summary>
        public PermissionStruct CreateNewCoupon
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 72;
                obj.Name = "Create New Coupon";
                obj.ScreenID = 1;
                obj.SortOrder = 72;
                return obj;
            }
        }

        /// <summary>
        /// Author: Sprint-24 - PRIMEPOS-2290 24-Oct-2016 JY Added
        /// Description: "Allow House Charge Payments" settings to control house charge payment
        /// </summary>
        public PermissionStruct AllowHouseChargePaytype
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 73;
                obj.Name = "Allow House Charge Payments";
                obj.ScreenID = 1;
                obj.SortOrder = 73;
                return obj;
            }
        }

        #endregion 1-permissions for postransaction

        #region 4-permissions for Inventory Recieved
        public PermissionStruct InventoryReceived   //PRIMEPOS-3141 27-Oct-2022 JY Added
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 91;
                obj.Name = "Inventory Received";
                obj.ScreenID = 4;
                obj.SortOrder = 91;
                return obj;
            }
        }

        public PermissionStruct ViewInventory
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 8;
                obj.Name = "Inventory Received History";    //PRIMEPOS-2824 25-Mar-2020 JY modified
                obj.ScreenID = 4;
                obj.SortOrder = 8;
                return obj;
            }
        }

        #endregion 4-permissions for Inventory Recieved

        #region 6-ItemFile
        public PermissionStruct PriceUpdate
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 9;
                obj.Name = "Price Update";
                obj.ScreenID = 6;
                obj.SortOrder = 9;
                return obj;
            }
        }
        #region PRIMEPOS-2464 04-Mar-2020 JY Added
        public PermissionStruct DisplayItemCost
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 81;
                obj.Name = "Display Item Cost";
                obj.ScreenID = 6;
                obj.SortOrder = 81;
                return obj;
            }
        }

        public PermissionStruct EditItemCost
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 82;
                obj.Name = "Edit Item Cost";
                obj.ScreenID = 6;
                obj.SortOrder = 82;
                return obj;
            }
        }
        #endregion
        #endregion 6-ItemFile

        #region 8-PurchaseOrder

        public PermissionStruct POAckProcess
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 20;
                obj.Name = "PO Ack. Process";
                obj.ScreenID = 8;
                obj.SortOrder = 20;
                return obj;
            }
        }

        public PermissionStruct POHistory
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 10;
                obj.Name = "View PO History";
                obj.ScreenID = 8;
                obj.SortOrder = 10;
                return obj;
            }
        }

        #endregion 8-PurchaseOrder

        #region 9- InvReports

        public PermissionStruct InvItemFileListing
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 11;
                obj.Name = "Item File Listing";
                obj.ScreenID = 9;
                obj.SortOrder = 11;
                return obj;
            }
        }

        public PermissionStruct InvVendFileListing
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 12;
                obj.Name = "Vendor File Listing";
                obj.ScreenID = 9;
                obj.SortOrder = 12;
                return obj;
            }
        }

        public PermissionStruct InvInvStatusRep
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 13;
                obj.Name = "Inventory Status Report";
                obj.ScreenID = 9;
                obj.SortOrder = 13;
                return obj;
            }
        }

        public PermissionStruct InvInventoryReceived
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 14;
                obj.Name = "Inventory Received";
                obj.ScreenID = 9;
                obj.SortOrder = 14;
                return obj;
            }
        }

        public PermissionStruct InvItemsReorder
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 15;
                obj.Name = "Items Re-Order";
                obj.ScreenID = 9;
                obj.SortOrder = 15;
                return obj;
            }
        }

        public PermissionStruct InvPurchaseOrder
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 16;
                obj.Name = "Purchase Order";
                obj.ScreenID = 9;
                obj.SortOrder = 16;
                return obj;
            }
        }

        public PermissionStruct InvPhysicalInvSheet
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 17;
                obj.Name = "Physical Inventory Sheet";
                obj.ScreenID = 9;
                obj.SortOrder = 17;
                return obj;
            }
        }

        public PermissionStruct InvPhysicalInvHistory
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 18;
                obj.Name = "Physical Inventory History";
                obj.ScreenID = 9;
                obj.SortOrder = 18;
                return obj;
            }
        }

        public PermissionStruct InvPhysicalInvChangeSellingPrice
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 60;
                obj.Name = "Change Selling Price";
                obj.ScreenID = 5;
                obj.SortOrder = 60;
                return obj;
            }
        }

        public PermissionStruct InvPhysicalInvChangeCostPrice
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 61;
                obj.Name = "Change Cost Price";
                obj.ScreenID = 5;
                obj.SortOrder = 61;
                return obj;
            }
        }

        #region Sprint-21 - 2206 11-Mar-2016 JY Added
        public PermissionStruct InvPhysicalInvChangeExpDate
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 71;
                obj.Name = "Change Exp. Date";
                obj.ScreenID = 5;
                obj.SortOrder = 71;
                return obj;
            }
        }
        #endregion

        public PermissionStruct InvItemLabelReport
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 19;
                obj.Name = "Item Label Report";
                obj.ScreenID = 9;
                obj.SortOrder = 19;
                return obj;
            }
        }

        public PermissionStruct IIASItemFileListing
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 43;
                obj.Name = "IIAS Item File Listing";
                obj.ScreenID = 9;
                obj.SortOrder = 43;
                return obj;
            }
        }

        #endregion 9- InvReports

        #region 13- StationClose
        public PermissionStruct ProcessStClose
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 21;
                obj.Name = "Process";
                obj.ScreenID = 13;
                obj.SortOrder = 21;
                return obj;
            }
        }

        public PermissionStruct ViewStationClose
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 22;
                obj.Name = "View History";
                obj.ScreenID = 13;
                obj.SortOrder = 22;
                return obj;
            }
        }

        public PermissionStruct UnClStView
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 35;
                obj.Name = "Unclosed Station view";
                obj.ScreenID = 13;
                obj.SortOrder = 35;
                return obj;
            }
        }

        //Sprint-19 - 2165 19-Mar-2015 JY Added setting to set cash difference visibility
        public PermissionStruct ViewCashDifference
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 70;
                obj.Name = "View Cash Difference";
                obj.ScreenID = 13;
                obj.SortOrder = 70;
                return obj;
            }
        }
        #endregion 13- StationClose

        #region 14- End of day

        public PermissionStruct ProcessEOD
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 23;
                obj.Name = "Process";
                obj.ScreenID = 14;
                obj.SortOrder = 23;
                return obj;
            }
        }

        public PermissionStruct ViewEOD
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 24;
                obj.Name = "View History";
                obj.ScreenID = 14;
                obj.SortOrder = 24;
                return obj;
            }
        }

        #endregion 14- End of day

        #region 17- Admin Reports

        public PermissionStruct TransactionDetail   //PRIMEPOS-3136 30-Aug-2022 JY Added
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 25;
                obj.Name = "Transaction Detail";
                obj.ScreenID = 17;
                obj.SortOrder = 25;
                return obj;
            }
        }

        public PermissionStruct SumSaleByU
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 26;
                obj.Name = "Sales Summary By User";
                obj.ScreenID = 17;
                obj.SortOrder = 26;
                return obj;
            }
        }

        public PermissionStruct SumSaleByI
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 27;
                obj.Name = "Sales Report By Item";  //Sprint-21 - PRIMEPOS-2278 19-Feb-2016 JY Renamed report from "Sales Summary by Item" to "Sales Report By Item" as we introduced summary and detail view.
                obj.ScreenID = 17;
                obj.SortOrder = 27;
                return obj;
            }
        }

        public PermissionStruct SaleByDept
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 28;
                obj.Name = "Sales By Department";
                obj.ScreenID = 17;
                obj.SortOrder = 28;
                return obj;
            }
        }

        public PermissionStruct SaleByPayment
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 29;
                obj.Name = "Sales By Payment";
                obj.ScreenID = 17;
                obj.SortOrder = 29;
                return obj;
            }
        }

        public PermissionStruct StationCloseSum
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 30;
                obj.Name = "Station Close Summary";
                obj.ScreenID = 17;
                obj.SortOrder = 30;
                return obj;
            }
        }

        public PermissionStruct SalesTaxSum
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 31;
                obj.Name = "Sales Tax Summary";
                obj.ScreenID = 17;
                obj.SortOrder = 31;
                return obj;
            }
        }

        public PermissionStruct SalesTaxControl
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 51;
                obj.Name = "Sales Tax Control";
                obj.ScreenID = 17;
                obj.SortOrder = 51;
                return obj;
            }
        }

        public PermissionStruct PriceOverridden
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 55;
                obj.Name = "Price Overridden Report";
                obj.ScreenID = 17;
                obj.SortOrder = 55;
                return obj;
            }
        }

        public PermissionStruct TopSellingProd
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 32;
                obj.Name = "Top Selling Products";
                obj.ScreenID = 17;
                obj.SortOrder = 32;
                return obj;
            }
        }

        public PermissionStruct ProductivityRep
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 33;
                obj.Name = "Hourly Trans. / Productivity Report";
                obj.ScreenID = 17;
                obj.SortOrder = 33;
                return obj;
            }
        }

        public PermissionStruct PayoutDetails
        {
            get
            {
                PermissionStruct obj=new PermissionStruct();
                obj.ID = 34;
                obj.Name = "Payout Details";
                obj.ScreenID = 17;
                obj.SortOrder = 34;
                return obj;
            }
        }

        public PermissionStruct CostAnalysis
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 39;
                obj.Name = "Cost Analysis";
                obj.ScreenID = 17;
                obj.SortOrder = 39;
                return obj;
            }
        }

        public PermissionStruct IIASTransSummary
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 44;
                obj.Name = "IIAS Trans Summary";
                obj.ScreenID = 17;
                obj.SortOrder = 44;
                return obj;
            }
        }

        public PermissionStruct IIASPaymentTransaction
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 45;
                obj.Name = "IIAS Transaction By Payment";
                obj.ScreenID = 17;
                obj.SortOrder = 45;
                return obj;
            }
        }

        public PermissionStruct SalesByCustomer
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 46;
                obj.Name = "Sales By Customer";
                obj.ScreenID = 17;
                obj.SortOrder = 46;
                return obj;
            }
        }

        public PermissionStruct CustomerList
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 47;
                obj.Name = "Customer List";
                obj.ScreenID = 17;
                obj.SortOrder = 47;
                return obj;
            }
        }

        public PermissionStruct SalesByItemMonitoringCategory
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 48;
                obj.Name = "Sales By Item Monitoring Category";
                obj.ScreenID = 17;
                obj.SortOrder = 48;
                return obj;
            }
        }

        public PermissionStruct DeliveryListReport
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 49;
                obj.Name = "Delivery List";
                obj.ScreenID = 17;
                obj.SortOrder = 49;
                return obj;
            }
        }

        public PermissionStruct InsuranceDetialReport
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 50;
                obj.Name = "Insurance Detail";
                obj.ScreenID = 17;
                obj.SortOrder = 50;
                return obj;
            }
        }

        public PermissionStruct SalesComparisonReport
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 53;
                obj.Name = "Sales Comparison";
                obj.ScreenID = 17;
                obj.SortOrder = 53;
                return obj;
            }
        }

        public PermissionStruct SalesComparisonByDeptReport
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 54;
                obj.Name = "Sales Comparison By Department";
                obj.ScreenID = 17;
                obj.SortOrder = 54;
                return obj;
            }
        }

        //Following Code added by Krishna on 23 June 2011
        public PermissionStruct StnCloseCash
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 57;
                obj.Name = "Station Close Cash";
                obj.ScreenID = 17;
                obj.SortOrder = 57;
                return obj;
            }
        }
        //Till here added by Krishna on 23 June 2011

        public PermissionStruct TransactionFeeReport
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 90;
                obj.Name = "Transaction Fee Report";
                obj.ScreenID = 17;
                obj.SortOrder = 90;
                return obj;
            }
        }

        #region PRIMEPOS-3360
        public PermissionStruct PseudoephedrineSalesLogs
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 92;
                obj.Name = "Pseudoephedrine Sales Logs";
                obj.ScreenID = 17;
                obj.SortOrder = 92;
                return obj;
            }
        }
        #endregion
        #endregion 17- Admin Reports

        #region 24- Timesheet - Clock - In / Out User
        public PermissionStruct AllowManualTimeInOut
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 41;
                obj.Name = "Allow Manual TimeIn/Out";
                obj.ScreenID = 24;
                obj.SortOrder = 41;
                return obj;
            }
        }

        public PermissionStruct AllowForLoggedInUserOnly
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 42;
                obj.Name = "Logged In User Only";
                obj.ScreenID = 24;
                obj.SortOrder = 42;
                return obj;
            }
        }
        #endregion 24- Timesheet

        #region Sprint-25 - PRIMEPOS-2253 23-Mar-2017 JY Added: 26- Timesheet - Create Timesheet
        public PermissionStruct EditProcessedTimesheet
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 75;
                obj.Name = "Edit Processed Timesheet";
                obj.ScreenID = 26;
                obj.SortOrder = 75;
                return obj;
            }
        }
        #endregion

        #region Sprint-26 - PRIMEPOS-2416 04-Jul-2017 JY Added
        public PermissionStruct QuantityOverride
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 76;
                obj.Name = "Quantity Override";
                obj.ScreenID = 1;
                obj.SortOrder = 10;
                return obj;
            }
        }
        #endregion

        #region Sprint-26 - PRIMEPOS-2383 08-Aug-2017 JY Added
        public PermissionStruct StandAloneReturn
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 77;
                obj.Name = "Stand alone Return";
                obj.ScreenID = 1;
                obj.SortOrder = 11;
                return obj;
            }
        }
        #endregion

        #region PRIMEPOS-2510 26-Apr-2018 JY Added tax override permissons for Rx  
        public PermissionStruct TaxOverrideForRx
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 78;
                obj.Name = "Tax Override For RX";
                obj.ScreenID = 1;
                obj.SortOrder = 78;
                return obj;
            }
        }
        public PermissionStruct TaxOverrideAllForRx
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 79;
                obj.Name = "Tax Override All For RX";
                obj.ScreenID = 1;
                obj.SortOrder = 79;
                return obj;
            }
        }
        #endregion

        #region PRIMEPOS-2539 06-Jul-2018 JY Added Allow Check Payment
        public PermissionStruct AllowCheckPayment
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 80;
                obj.Name = "Allow Check Payment";
                obj.ScreenID = 1;
                obj.SortOrder = 80;
                return obj;
            }
        }
        #endregion

        #region PRIMEPOS-2484 04-Jun-2020 JY Added
        public PermissionStruct CLPSettings
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 83;
                obj.Name = "CLP Settings";
                obj.ScreenID = 81;
                obj.SortOrder = 83;
                return obj;
            }
        }

        public PermissionStruct DeviceSettings
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 84;
                obj.Name = "Device Settings";
                obj.ScreenID = 81;
                obj.SortOrder = 84;
                return obj;
            }
        }

        public PermissionStruct TransSettings
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 85;
                obj.Name = "Transaction Settings";
                obj.ScreenID = 81;
                obj.SortOrder = 85;
                return obj;
            }
        }

        public PermissionStruct RxSettings
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 86;
                obj.Name = "RX Settings";
                obj.ScreenID = 81;
                obj.SortOrder = 86;
                return obj;
            }
        }

        public PermissionStruct PrimePOSettings
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 87;
                obj.Name = "PrimePO Settings";
                obj.ScreenID = 81;
                obj.SortOrder = 87;
                return obj;
            }
        }

        public PermissionStruct AppSettings
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 88;
                obj.Name = "Application Settings";
                obj.ScreenID = 81;
                obj.SortOrder = 88;
                return obj;
            }
        }
        #endregion

        #region PRIMEPOS-2896
        public PermissionStruct AllowTokenizationFromCustomerFile
        {
            get
            {
                PermissionStruct obj = new PermissionStruct();
                obj.ID = 89;
                obj.Name = "Allow Tokenizing Card from Customer File";
                obj.ScreenID = 1;
                obj.SortOrder = 89;
                return obj;
            }
        }
        #endregion
        //#region PRIMEPOS-2808 - Audit Trail
        //#region PRIMEPOS-2811 - Sub Ticket
        //public PermissionStruct ViewAuditLog
        //{
        //    get
        //    {
        //        PermissionStruct obj = new PermissionStruct();
        //        obj.ID = 90;
        //        obj.Name = "View Audit Trail";
        //        obj.ScreenID = 82;
        //        obj.SortOrder = 90;
        //        return obj;
        //    }
        //}
        //#endregion
        //#region PRIMEPOS-2812 - Sub Ticket
        //public PermissionStruct ViewNoSaleTransaction
        //{
        //    get
        //    {
        //        PermissionStruct obj = new PermissionStruct();
        //        obj.ID = 91;
        //        obj.Name = "View NoSale Transaction";
        //        obj.ScreenID = 83;
        //        obj.SortOrder = 91;
        //        return obj;
        //    }
        //}
        //#endregion
        //#endregion
        #region PRIMEPOS-2808 - Audit Trail
        #region PRIMEPOS-2811 - Sub Ticket
        //public PermissionStruct ViewAuditLog
        //{
        //    get
        //    {
        //        PermissionStruct obj = new PermissionStruct();
        //        obj.ID = 90;
        //        obj.Name = "View Audit Trail";
        //        obj.ScreenID = 82;
        //        obj.SortOrder = 90;
        //        return obj;
        //    }
        //}
        #endregion
        #region PRIMEPOS-2812 - Sub Ticket
        //public PermissionStruct ViewNoSaleTransaction
        //{
        //    get
        //    {
        //        PermissionStruct obj = new PermissionStruct();
        //        obj.ID = 91;
        //        obj.Name = "View NoSale Transaction";
        //        obj.ScreenID = 83;
        //        obj.SortOrder = 91;
        //        return obj;
        //    }
        //}
        #endregion
        #endregion
    }

    public class PermissionStruct
    {
        public PermissionStruct() { }

        public int ID;
        public string Name;
        public int ScreenID;
        public bool EntryScreen=false;
        public int SortOrder;
    }
}