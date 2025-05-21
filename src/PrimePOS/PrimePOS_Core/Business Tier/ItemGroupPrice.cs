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

	// ItemGroupPrices Business Rules Class  

	public class ItemGroupPrice : IDisposable 
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// A method for inserting and updating ItemGroupPrice data.

		public  void Persist(ItemGroupPriceData updates) 
		{
			
			try 
			{	

				checkIsValidData(updates);
				using(ItemGroupPriceSvr dao = new ItemGroupPriceSvr()) 
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
				logger.Fatal(ex, "Persist(ItemGroupPriceData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
			}
		}			
		#endregion

		#region Get Methods

		// Fills a DataSet with all ItemGroupPrices based on a condition.
		public ItemGroupPriceData PopulateList(string whereClause) 
		{
			try 
			{
				using(ItemGroupPriceSvr dao = new ItemGroupPriceSvr()) 
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
				logger.Fatal(ex, "PopulateList(string whereClause)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}

		#endregion

        

		#region Validation Methods
		// Validate a ItemGroupPrice.  This would be the place to put field validations.
		public void checkIsValidData(ItemGroupPriceData updates) 
		{
			ItemGroupPriceTable table;

			ItemGroupPriceRow oRow ;
				
			oRow = (ItemGroupPriceRow)updates.Tables[0].Rows[0];

			table = (ItemGroupPriceTable)updates.Tables[0].GetChanges(DataRowState.Added);

			if (table == null)
				table= (ItemGroupPriceTable)updates.Tables[0].GetChanges(DataRowState.Modified);
			else if ((ItemGroupPriceTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
				table.MergeTable ((ItemGroupPriceTable)updates.Tables[0].GetChanges(DataRowState.Modified));

			if (table == null)
				table= (ItemGroupPriceTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
			else if ((ItemGroupPriceTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
				table.MergeTable( (ItemGroupPriceTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

			if (table==null) return;

			foreach(ItemGroupPriceRow row in table.Rows)
			{ 
				if (row.ItemID.Trim() == "" )
					ErrorHandler.throwCustomError(POSErrorENUM.ItemGroupPrice_ItemIDCanNotBeNULL); 
				if (row.SalePrice == 0)
					ErrorHandler.throwCustomError(POSErrorENUM.ItemGroupPrice_SalePriceCanNotBeNull); 
				//if (row.Cost == 0)
				//	ErrorHandler.throwCustomError(POSErrorENUM.ItemGroupPrice_CostCanNotBeNull); 
				if (row.Qty == 0)
					ErrorHandler.throwCustomError(POSErrorENUM.ItemGroupPrice_QtyCanNotBeNull); 
			}
		}

		public ItemGroupPriceData Populate(System.Int32 GroupPriceID) 
		{
			try 
			{
				using(ItemGroupPriceSvr dao = new ItemGroupPriceSvr()) 
				{
					return dao.Populate(GroupPriceID);
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
				logger.Fatal(ex, "Populate(System.Int32 GroupPriceID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}

		public void checkIsValidPrimaryKey(ItemGroupPriceData updates) 
		{	
			ItemGroupPriceTable table = (ItemGroupPriceTable)updates.Tables[clsPOSDBConstants.ItemGroupPrice_tbl];
			foreach(ItemGroupPriceRow row in table.Rows)
			{ 
				if (this.Populate (row.GroupPriceID).Tables[clsPOSDBConstants.ItemGroupPrice_tbl].Rows.Count != 0)
				{ 
					throw new Exception ("Primary key violation for ItemGroupPrice ");
				}		
			} 
		}	

		// Check whether an attempted delete is valid for ItemGroupPrice
		public void checkIsValidDelete(ItemGroupPriceData updates)
		{ 
		}	
		#endregion

		public void Dispose() 
		{
			GC.SuppressFinalize(true);
		}

	}
}
