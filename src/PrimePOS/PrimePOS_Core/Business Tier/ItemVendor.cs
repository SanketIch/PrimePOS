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

	// ItemVendors Business Rules Class  

	public class ItemVendor : IDisposable 
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// A method for inserting and updating ItemVendor data.

		public  void Persist(ItemVendorData updates) 
		{
			
			try 
			{	

				checkIsValidData(updates);
				using(ItemVendorSvr dao = new ItemVendorSvr()) 
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
				logger.Fatal(ex, "Persist(ItemVendorData updates) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
			}
		}			
		#endregion

		#region Get Methods

		// Fills a DataSet with all ItemVendors based on a condition.
		public ItemVendorData PopulateList(string whereClause) 
		{
			try 
			{
				using(ItemVendorSvr dao = new ItemVendorSvr()) 
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
				//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}

		#endregion        

		#region Validation Methods
		// Validate a ItemVendor.  This would be the place to put field validations.
		public void checkIsValidData(ItemVendorData updates) 
		{
			ItemVendorTable table;

			ItemVendorRow oRow ;
				
			oRow = (ItemVendorRow)updates.Tables[0].Rows[0];

			table = (ItemVendorTable)updates.Tables[0].GetChanges(DataRowState.Added);

			if (table == null)
				table= (ItemVendorTable)updates.Tables[0].GetChanges(DataRowState.Modified);
			else if ((ItemVendorTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
				table.MergeTable ((ItemVendorTable)updates.Tables[0].GetChanges(DataRowState.Modified));

			if (table == null)
				table= (ItemVendorTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
			else if ((ItemVendorTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
				table.MergeTable( (ItemVendorTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

			if (table==null) return;

			foreach(ItemVendorRow row in table.Rows)
			{ 
				if (row.ItemID.Trim() == "" )
					ErrorHandler.throwCustomError(POSErrorENUM.ItemVendor_ItemIDCanNotBeNULL); 
			//	if (row.VenorCostPrice == 0)
			//		ErrorHandler.throwCustomError(POSErrorENUM.ItemVendor_VenorCostPriceCanNotBeNull); 
				if (row.VendorID == 0)
					ErrorHandler.throwCustomError(POSErrorENUM.ItemVendor_VendorIDCanNotBeNull); 
				if (row.VendorItemID.Trim() == "")
					ErrorHandler.throwCustomError(POSErrorENUM.ItemVendor_VendorItemIDCanNotBeNull); 
			}
		}

		public void Validate_VendorID(string strValue)
		{
			if (strValue.Trim()=="" || strValue==null )
			{
				ErrorHandler.throwCustomError(POSErrorENUM.ItemVendor_VendorIDCanNotBeNull); 
			}
		}

		public void Validate_VendorItemID(string strValue)
		{
			if (strValue.Trim()=="" || strValue==null )
			{
				ErrorHandler.throwCustomError(POSErrorENUM.ItemVendor_VendorItemIDCanNotBeNull); 
			}
		}

		public void Validate_Price(string strValue)
		{
			if (strValue.Trim()=="" || strValue==null )
			{
				ErrorHandler.throwCustomError(POSErrorENUM.ItemVendor_VenorCostPriceCanNotBeNull); 
			}
		}

		public ItemVendorData Populate(System.String VendorItemID) 
		{
			try 
			{
				using(ItemVendorSvr dao = new ItemVendorSvr()) 
				{
					return dao.Populate(VendorItemID);
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
				logger.Fatal(ex, "Populate(System.String VendorItemID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}

		public void checkIsValidPrimaryKey(ItemVendorData updates) 
		{	
			ItemVendorTable table = (ItemVendorTable)updates.Tables[clsPOSDBConstants.ItemVendor_tbl];
			foreach(ItemVendorRow row in table.Rows)
			{ 
				if (this.Populate (row.VendorItemID).Tables[clsPOSDBConstants.ItemVendor_tbl].Rows.Count != 0)
				{ 
					throw new Exception ("Primary key violation for ItemVendor ");
				}		
			} 
		}	

		// Check whether an attempted delete is valid for ItemVendor
		public void checkIsValidDelete(ItemVendorData updates)
		{ 
		}	
		#endregion

        #region Sprint-21 - 2207 13-Aug-2015 JY Added
        public void UpdateItemVendor(int nItemDetailID, string strVendorItemID)
        {
            try
            {
                using (ItemVendorSvr dao = new ItemVendorSvr())
                {
                    dao.UpdateItemVendor(nItemDetailID, strVendorItemID);
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
				logger.Fatal(ex, "UpdateItemVendor(int nItemDetailID, string strVendorItemID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
			}
        }
        #endregion

        public void Dispose() 
		{
			GC.SuppressFinalize(true);
		}

	}
}
