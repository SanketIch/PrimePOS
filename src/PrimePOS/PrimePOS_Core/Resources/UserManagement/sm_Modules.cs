using System;
using System.Data;
using System.Reflection;

namespace POS_Core.UserManagement
{
	/// <summary>
	/// Summary description for sm_Modules.
	/// </summary>
	public class sm_Modules
	{
		private DataTable dt;
		public sm_Modules()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DataTable Modules
		{
			get { 
				dt=new DataTable("Modules");

				dt.Columns.Add("ModuleID",typeof(int));
				dt.Columns.Add("ModuleName",typeof(string));
				dt.Columns.Add("SortOrder",typeof(int));
				
				foreach  (PropertyInfo pInfo in this.GetType().GetProperties())
				{
					if (pInfo.PropertyType==typeof(ModuleStruct) )
					{
						ModuleStruct obj=null;
						obj=(ModuleStruct)pInfo.GetValue(this,null);
						dt.Rows.Add(new object[] {obj.ID,obj.Name,obj.SortOrder});
					}
				}

//				dt.Rows.Add(new object[] {2,"Inventory Management"});
//				dt.Rows.Add(new object[] {3,"Administrative Functions"});
//				dt.Rows.Add(new object[] {4,"Application Settings"});
				return this.dt; }
		}

		

		public ModuleStruct POSTransaction
		{
			get 
			{
				ModuleStruct obj=new ModuleStruct();
				obj.ID=1;
				obj.Name="POS Transaction";
				obj.SortOrder=1;
				return obj;
			}
		}
		public ModuleStruct InventoryMgmt
		{
			get 
			{
				ModuleStruct obj=new ModuleStruct();
				obj.ID=2;
				obj.Name="Inventory Management";
				obj.SortOrder=2;
				return obj;
			}
		}
		public ModuleStruct AdminFunc
		{
			get 
			{
				ModuleStruct obj=new ModuleStruct();
				obj.ID=3;
				obj.Name="Administrative Functions";
				obj.SortOrder=3;
				return obj;
			}
		}

		public ModuleStruct AppSettings
		{
			get 
			{
				ModuleStruct obj=new ModuleStruct();
				obj.ID=4;
				obj.Name="Application"; //PRIMEPOS-2484 04-Jun-2020 JY renamed
                obj.SortOrder=4;
				return obj;
			}
		}

        public ModuleStruct Timesheet
        {
            get
            {
                ModuleStruct obj = new ModuleStruct();
                obj.ID = 5;
                obj.Name = "Timesheet";
                obj.SortOrder = 5;
                return obj;
            }
        }
        //Following Code added by Krishna on 11 October 2011
        public ModuleStruct Notes
        {
            get
            {
                ModuleStruct obj = new ModuleStruct();
                obj.ID = 6;
                obj.Name = "Notes";
                obj.SortOrder = 6;
                return obj;
            }
        }
        //Till here Added by Krishna on 11 October 2011

	}
		public class ModuleStruct
		{
			public ModuleStruct(){}
			public int ID ;
			public string Name ;
			public int SortOrder;
		}
	}
