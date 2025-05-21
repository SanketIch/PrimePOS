// ----------------------------------------------------------------
// Library: Business Tier
//  Author: Adeel Shehzad.
// Company: D-P-S. (www.d-p-s.com)
//
// ----------------------------------------------------------------

namespace POS_Core.BusinessRules  
{
	using System;
	using System.Data;
	using POS_Core.DataAccess;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData;
	using POS_Core.ErrorLogging;
	using NLog;

	// SubDepartments Business Rules Class  
	public class SubDepartment : IDisposable 
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods
		// A method for inserting and updating SubDepartment data.

		public  void Persist(SubDepartmentData updates) 
		{
			
			try 
			{	

				checkIsValidData(updates);
				using(SubDepartmentSvr dao = new SubDepartmentSvr()) 
				{
					dao.Persist(updates);
				}
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch(Exception ex) 
			{
				logger.Fatal(ex, "Persist(SubDepartmentData updates) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
			}
		}			
		#endregion

		#region Get Methods

		// Fills a DataSet with all SubDepartments based on a condition.
		public SubDepartmentData PopulateList(string whereClause) 
		{
			try 
			{
				using(SubDepartmentSvr dao = new SubDepartmentSvr()) 
				{
					return dao.PopulateList(whereClause);
				}
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch(Exception ex) 
			{
				logger.Fatal(ex, "PopulateList(string whereClause) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

        public DataSet GetCLExcludedSubDeptData()
        {
            try
            {
                using (SubDepartmentSvr dao = new SubDepartmentSvr())
                {
                    return dao.PopulateListWithIdName(" where ExcludeFromCL=1");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public DataSet GetCLCouponExcludedSubDeptData()
        {
            try
            {
                using (SubDepartmentSvr dao = new SubDepartmentSvr())
                {
                    return dao.PopulateListWithIdName(" where ExcludeFromCL=1 and EXCLUDEFROMCLCouponPay=1 ");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion        

		#region Validation Methods
		// Validate a SubDepartment.  This would be the place to put field validations.
		public void checkIsValidData(SubDepartmentData updates) 
		{
			SubDepartmentTable table;

			SubDepartmentRow oRow ;
				
			oRow = (SubDepartmentRow)updates.Tables[0].Rows[0];

			table = (SubDepartmentTable)updates.Tables[0].GetChanges(DataRowState.Added);

			if (table == null)
				table= (SubDepartmentTable)updates.Tables[0].GetChanges(DataRowState.Modified);
			else if ((SubDepartmentTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
				table.MergeTable ((SubDepartmentTable)updates.Tables[0].GetChanges(DataRowState.Modified));

			if (table == null)
				table= (SubDepartmentTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
			else if ((SubDepartmentTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
				table.MergeTable( (SubDepartmentTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

			if (table==null) return;

		}

		public SubDepartmentData Populate(System.Int32 departmentID) 
		{
			try 
			{
				using(SubDepartmentSvr dao = new SubDepartmentSvr()) 
				{
					return dao.Populate(departmentID);
				}
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch(Exception ex) 
			{
				logger.Fatal(ex, "Populate(System.Int32 departmentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

#endregion

		public void Dispose() 
		{
			GC.SuppressFinalize(true);
		}

	}
}
