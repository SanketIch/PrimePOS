using System;
using System.Data;
using System.Reflection;

namespace POS_Core.UserManagement
{
	/// <summary>
	/// Summary description for sm_Screens.
	/// </summary>
	public class sm_Screens
	{
		private DataTable dt;
		public sm_Screens()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public DataTable Screens
		{
			get 
			{ 
				dt=new DataTable("Screens");

				dt.Columns.Add("ScID",typeof(int));
				dt.Columns.Add("ScName",typeof(string));
				dt.Columns.Add("ModuleID",typeof(int));
				dt.Columns.Add("EntryScreen",typeof(bool));
				dt.Columns.Add("SortOrder",typeof(int));

				foreach  (PropertyInfo pInfo in this.GetType().GetProperties())
				{
					if (pInfo.PropertyType==typeof(ScreensStruct) )
					{
						ScreensStruct obj=null;
						obj=(ScreensStruct)pInfo.GetValue(this,null);
						dt.Rows.Add(new object[] {obj.ID,obj.Name,obj.ModuleID,obj.EntryScreen,obj.SortOrder});
					}
				}
                dt.DefaultView.Sort = "ScName asc";
                dt = dt.DefaultView.ToTable();
                return this.dt;
            }
		}
        
		#region 1-POS Transaction
		public ScreensStruct POSTransaction
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=1;
				obj.Name="Allow POS Transaction";
				obj.ModuleID=1;
				obj.EntryScreen=false;
				obj.SortOrder=1;
				return obj;
			}
		}
        
		public ScreensStruct Payout
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=2;
				obj.Name="Allow Payout";
				obj.ModuleID=1;
				obj.EntryScreen=false;
				obj.SortOrder=2;
				return obj;
			}
		}
		public ScreensStruct Customers
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=3;
				obj.Name="Allow Customers";
				obj.ModuleID=1;
				obj.EntryScreen=true;
				obj.SortOrder=3;
				return obj;
			}
		}
        
		#endregion

		#region 2- Inventory Management
		public ScreensStruct InventoryRecvd
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=4;
				obj.Name= "Inventory Received"; //PRIMEPOS-2824 25-Mar-2020 JY modified
                obj.ModuleID=2;
				obj.EntryScreen=false;
				obj.SortOrder=4;
				
				return obj;
			}
		}

		public ScreensStruct InvTransType
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=22;
				obj.Name="Inventory Transaction Type";
				obj.ModuleID=2;
				obj.EntryScreen=true;
				obj.SortOrder=5;
				
				return obj;
			}
		}

		public ScreensStruct PhysicalInventory
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=5;
				obj.Name="Physical Inventory";
				obj.ModuleID=2;
				obj.SortOrder=5;
				return obj;
			}
		}
		
		public ScreensStruct ItemFile
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=6;
				obj.Name="Item File";
				obj.ModuleID=2;
				obj.SortOrder=6;
				obj.EntryScreen=true;
				return obj;
			}
		}
		
		public ScreensStruct VendorFile
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=7;
				obj.Name="Vendor File";
				obj.ModuleID=2;
				obj.SortOrder=7;
				obj.EntryScreen=true;
				return obj;
			}
		}
		
		public ScreensStruct PurchaseOrder
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=8;
				obj.Name="Purchase Order";
				obj.ModuleID=2;
				obj.EntryScreen=true;
				obj.SortOrder=8;
				return obj;
			}
		}
		
		public ScreensStruct InvReports
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=9;
				obj.Name="Reports";
				obj.ModuleID=2;
				obj.EntryScreen=false;
				obj.SortOrder=9;
				return obj;
			}
		}

        public ScreensStruct MinimumItemPrice
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=23;
                obj.Name = "Minimum Item Price";
				obj.ModuleID=2;
				obj.EntryScreen=false;
				obj.SortOrder=10;
				return obj;
			}
		}

        public ScreensStruct WarningMessages
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 27;
                obj.Name = "Warning Messages";
                obj.ModuleID = 2;
                obj.SortOrder = 27;
                obj.EntryScreen = true;
                return obj;
            }
        }
        #region PRIMEPOS-3419
        public ScreensStruct AllowInventoryMenu
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 87;
                obj.Name = "Allow Inventory Menu";
                obj.ModuleID = 2;
                obj.SortOrder = 87;
                obj.EntryScreen = false;
                return obj;
            }
        }
        #endregion
        #endregion

        #region 3- Administrative Functions

        public ScreensStruct ManageUsers
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=20;
				obj.Name="Manage Users";
				obj.ModuleID=3;
				obj.SortOrder=20;
				return obj;
			}
		}
        
        //Added by shitaljit for manage payment types
        public ScreensStruct ManagePayType
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 69;
                obj.Name = "Manage Payment Types";
                obj.ModuleID = 3;
                obj.SortOrder = 69;
                return obj;
            }
        }

       
		public ScreensStruct Department
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=10;
				obj.Name="Department File";
				obj.ModuleID=3;
				obj.EntryScreen=true;
				obj.SortOrder=10;
				return obj;
			}
		}
		
		public ScreensStruct TaxCodes
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=11;
				obj.Name="Tax Codes";
				obj.ModuleID=3;
				obj.EntryScreen=true;
				obj.SortOrder=11;
				return obj;
			}
		}
		
		public ScreensStruct FunctionKeys
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=12;
				obj.Name="Function Keys";
				obj.ModuleID=3;
				obj.SortOrder=12;
				return obj;
			}
		}
		
		public ScreensStruct StationClose
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=13;
				obj.Name="Station Close";
				obj.ModuleID=3;
				obj.SortOrder=13;
				return obj;
			}
		}
		
		public ScreensStruct EndOfDay
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=14;
				obj.Name="End Of Day";
				obj.ModuleID=3;
				obj.SortOrder=14;
				return obj;
			}
		}
		
		public ScreensStruct ViewPOSTrans
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=15;
				obj.Name="View POS Transaction";
				obj.ModuleID=3;
				obj.SortOrder=15;
				return obj;
			}
		}
		
		public ScreensStruct MyStore
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=16;
				obj.Name="View My Store";
				obj.ModuleID=3;
				obj.SortOrder=16;
				return obj;
			}
		}
		
		public ScreensStruct AdminReports
		{
			get 
			{
				ScreensStruct obj=new ScreensStruct();
				obj.ID=17;
				obj.Name="Reports";
				obj.ModuleID=3;
				obj.SortOrder=17;
				return obj;
			}
		}

        public ScreensStruct PointsRewardTier
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 29;
                obj.Name = "Points Reward Tier";
                obj.ModuleID = 3;
                obj.SortOrder = 29;
                obj.EntryScreen = true;
                return obj;
            }
        }
        public ScreensStruct CustomerLoyaltyCards
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 30;
                obj.Name = "Customer Loyalty Cards";
                obj.ModuleID = 3;
                obj.SortOrder = 30;
                obj.EntryScreen = true;
                return obj;
            }
        }
        #endregion

        #region 4 - Preferences
        //public ScreensStruct AppSettings
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 19;
        //        obj.Name = "Application Settings";
        //        obj.ModuleID = 4;
        //        obj.SortOrder = 19;
        //        return obj;
        //    }
        //}

        public ScreensStruct Preferences
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 81;
                obj.Name = "Preferences";
                obj.ModuleID = 4;
                obj.SortOrder = 81;
                return obj;
            }
        }

        #region PRIMEPOS-2484 04-Jun-2020 JY Added
        public ScreensStruct IIASFileList
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 79;
                obj.Name = "IIAS File List";
                obj.ModuleID = 4;
                obj.SortOrder = 79;
                return obj;
            }
        }

        //Sprint-25 - PRIMEPOS-2379 14-Mar-2017 JY Added
        public ScreensStruct PSEItemList
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 74;
                obj.Name = "PSE Item List";
                obj.ModuleID = 4;
                obj.SortOrder = 74;
                //obj.EntryScreen = true;
                return obj;
            }
        }

        #region PRIMEPOS-3166
        public ScreensStruct OverrideMonitorItem
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 86;
                obj.Name = "Allow Override Monitor Item";
                obj.ModuleID = 4;
                obj.SortOrder = 86;
                //obj.EntryScreen = true;
                return obj;
            }
        }
        #endregion

        //Added by shitaljit on 5 April 2013 for
        //PRIMEPOS-382 Add a new security manager option that will allow/disallow access to the change password menu option
        public ScreensStruct ChangeLoginUserPassword
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 70;
                obj.Name = "Change Login User Password";
                obj.ModuleID = 4;
                obj.SortOrder = 70;
                return obj;
            }
        }

        public ScreensStruct LockStation
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 80;
                obj.Name = "Lock Station";
                obj.ModuleID = 4;
                obj.SortOrder = 80;
                return obj;
            }
        }
        #endregion

        //public ScreensStruct OverridePermissions
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 21;
        //        obj.Name = "Override Actions";
        //        obj.ModuleID = 4;
        //        obj.SortOrder = 21;
        //        return obj;
        //    }
        //}

        //ADDED PRASHANT 7 JUN 2010
        //public ScreensStruct XChargeSettings
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 28;
        //        obj.Name = "XCharge Settings";
        //        obj.ModuleID = 4;
        //        obj.SortOrder = 28;
        //        return obj;
        //    }
        //}
        //END ADDED PRASHANT 7 JUN 2010        

        #region PRIMEPOS-2484 04-Jun-2020 JY Commented
        //public ScreensStruct CLPSettings
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 78;
        //        obj.Name = "CLP Settings";
        //        obj.ModuleID = 4;
        //        obj.SortOrder = 78;
        //        return obj;
        //    }
        //}
        //public ScreensStruct DeviceSettings
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 18;
        //        obj.Name = "Device Settings";
        //        obj.ModuleID = 4;
        //        obj.SortOrder = 18;
        //        return obj;
        //    }
        //}
        #region PRIMEPOS-2676 20-May-2019 JY Added
        //public ScreensStruct TransSettings
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 75;
        //        obj.Name = "Transaction Settings";
        //        obj.ModuleID = 4;
        //        obj.SortOrder = 75;
        //        return obj;
        //    }
        //}
        //public ScreensStruct RxSettings
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 76;
        //        obj.Name = "RX Settings";
        //        obj.ModuleID = 4;
        //        obj.SortOrder = 76;
        //        return obj;
        //    }
        //}
        //public ScreensStruct PrimePOSettings
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 77;
        //        obj.Name = "PrimePO Settings";
        //        obj.ModuleID = 4;
        //        obj.SortOrder = 77;
        //        return obj;
        //    }
        //}
        #endregion
        #endregion


        //END ADDED PRASHANT 7 JUN 2010
        public ScreensStruct DeleteVendor
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 56;
                obj.Name = "Delete Vendor";
                obj.ModuleID = 3;
                obj.SortOrder = 56;
                return obj;
            }
        }
        public ScreensStruct DeleteUser
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 57;
                obj.Name = "Delete User";
                obj.ModuleID = 3;
                obj.SortOrder = 57;
                return obj;
            }
        }
        public ScreensStruct DeleteDepartment
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 58;
                obj.Name = "Delete Department";
                obj.ModuleID = 3;
                obj.SortOrder = 58;
                return obj;
            }
        }
        public ScreensStruct DeleteTaxCode
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 59;
                obj.Name = "Delete Tax Code";
                obj.ModuleID = 3;
                obj.SortOrder = 59;
                return obj;
            }
        }
        public ScreensStruct DeleteItem
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 60;
                obj.Name = "Delete Item";
                obj.ModuleID = 3;
                obj.SortOrder = 60;
                return obj;
            }
        }
        public ScreensStruct DeleteCustomer
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 61;
                obj.Name = "Delete Customer";
                obj.ModuleID = 3;
                obj.SortOrder = 61;
                return obj;
            }
        }

        //Sprint-23 - PRIMEPOS-2316 16-Jun-2016 JY Added
        public ScreensStruct DeleteCreditCardProfiles
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 73;
                obj.Name = "Delete Credit Card Profiles";
                obj.ModuleID = 3;
                obj.SortOrder = 73;
                return obj;
            }
        }

        #endregion

        #region 5- Timesheet
        public ScreensStruct Timesheet
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 24;
                obj.Name = "Clock - In / Out User"; //Sprint-25 - PRIMEPOS-2253 23-Mar-2017 JY - renaming "Timesheet" to "Clock - In / Out User"
                obj.ModuleID = 5;
                obj.SortOrder = 24;
                return obj;
            }
        }

        public ScreensStruct TimesheetReport
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 25;
                obj.Name = "Timesheet Report For All User";
                obj.ModuleID = 5;
                obj.SortOrder = 25;
                return obj;
            }
        }

        //Added by shitaljit for PRIMEPOS-1265 Ability to view\print logged in users own clock-in\clock-out times
        public ScreensStruct TimesheetReportForLoginUser
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 71;
                obj.Name = "Timesheet Report For Login User";
                obj.ModuleID = 5;
                obj.SortOrder = 71;
                return obj;
            }
        }

        public ScreensStruct TimesheetCreate
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 26;
                obj.Name = "Create Timesheet";
                obj.ModuleID = 5;
                obj.SortOrder = 26;
                return obj;
            }
        }

        #region PRIMEPOS-189 05-Aug-2021 JY Added
        public ScreensStruct DisplayHourlyRate
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 84;
                obj.Name = "Display Hourly Rate";
                obj.ModuleID = 5;
                obj.SortOrder = 84;
                return obj;
            }
        }
        public ScreensStruct EditHourlyRate
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 85;
                obj.Name = "Edit Hourly Rate";
                obj.ModuleID = 5;
                obj.SortOrder = 85;
                return obj;
            }
        }
        #endregion
        #endregion

        #region 6-Notes
        public ScreensStruct ManageNotes
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 62;
                obj.Name = "Manage Notes";
                obj.ModuleID = 6;
                obj.SortOrder = 62;
                return obj;
            }
        }

        //Added By Shitaljit(QuicSolv) on 11 oct 2011
        public ScreensStruct SystemNotes
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 67;
                obj.Name = "System Notes";
                obj.ModuleID = 6;
                obj.SortOrder = 67;
                return obj;
            }
        }
        public ScreensStruct CustomerNotes
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 68;
                obj.Name = "Customer Notes";
                obj.ModuleID = 6;
                obj.SortOrder = 68;
                return obj;
            }
        }
        //END of Added By Shitaljit(QuicSolv) on 11 oct 2011
        public ScreensStruct ItemNotes
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 63;
                obj.Name = "Item Notes";
                obj.ModuleID = 6;
                obj.SortOrder = 63;
                return obj;
            }
        }

        public ScreensStruct DepartmentNotes
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 64;
                obj.Name = "Department Notes";
                obj.ModuleID = 6;
                obj.SortOrder = 64;
                return obj;
            }
        }

        public ScreensStruct VendorNotes
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 65;
                obj.Name = "Vendor Notes";
                obj.ModuleID = 6;
                obj.SortOrder = 65;
                return obj;
            }
        }

        public ScreensStruct UserNotes
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 66;
                obj.Name = "User Notes";
                obj.ModuleID = 6;
                obj.SortOrder = 66;
                return obj;
            }
        }
        #endregion

        //#region PRIMEPOS-2808 - AuditTrail
        //#region PRIMEPOS-2811 - Sub Ticket
        //public ScreensStruct ViewAuditLog
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 82;
        //        obj.Name = "View Audit Trail";
        //        obj.ModuleID = 3;
        //        obj.SortOrder = 82;
        //        return obj;
        //    }
        //}
        //#endregion
        //#region PRIMEPOS-2812 - Sub Ticket
        //public ScreensStruct ViewNoSaleTransaction
        //{
        //    get
        //    {
        //        ScreensStruct obj = new ScreensStruct();
        //        obj.ID = 83;
        //        obj.Name = "View No Sale Transaction";
        //        obj.ModuleID = 3;
        //        obj.SortOrder = 83;
        //        return obj;
        //    }
        //}
        //#endregion 
        //#endregion
        #region PRIMEPOS-2808 - AuditTrail
        #region PRIMEPOS-2811 - Sub Ticket
        public ScreensStruct ViewAuditLog
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 82;
                obj.Name = "View Audit Trail";
                obj.ModuleID = 3;
                obj.SortOrder = 82;
                return obj;
            }
        }
        #endregion
        #region PRIMEPOS-2812 - Sub Ticket
        public ScreensStruct ViewNoSaleTransaction
        {
            get
            {
                ScreensStruct obj = new ScreensStruct();
                obj.ID = 83;
                obj.Name = "View No Sale Transaction";
                obj.ModuleID = 3;
                obj.SortOrder = 83;
                return obj;
            }
        }
        #endregion 
        #endregion
    }

    public class ScreensStruct
	{
		public ScreensStruct(){}
		public int ID ;
		public string Name ;
		public int ModuleID;
		public bool EntryScreen=false;
		public int SortOrder;
	}
}
