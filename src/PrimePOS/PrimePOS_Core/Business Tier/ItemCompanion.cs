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

	// ItemCompanions Business Rules Class  

	public class ItemCompanion : IDisposable 
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods
		// A method for inserting and updating ItemCompanion data.
		public  void Persist(ItemCompanionData updates) 
		{
			
			try 
			{	
			
				checkIsValidData(updates);
				using(ItemCompanionSvr dao = new ItemCompanionSvr()) 
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
				logger.Fatal(ex, "Persist(ItemCompanionData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
			}
		}			
		#endregion

		#region Get Methods

		// Fills a DataSet with all ItemCompanions based on a condition.
		public ItemCompanionData PopulateList(string whereClause) 
		{
			try 
			{
				using(ItemCompanionSvr dao = new ItemCompanionSvr()) 
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
				//ErrorHandler.throwException(ex,"","");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}

		#endregion

        

		#region Validation Methods
		// Validate a ItemCompanion.  This would be the place to put field validations.
		public void checkIsValidData(ItemCompanionData updates) 
		{
			ItemCompanionTable table;

			ItemCompanionRow oRow ;
				
			oRow = (ItemCompanionRow)updates.Tables[0].Rows[0];

			table = (ItemCompanionTable)updates.Tables[0].GetChanges(DataRowState.Added);

			if (table == null)
				table= (ItemCompanionTable)updates.Tables[0].GetChanges(DataRowState.Modified);
			else if ((ItemCompanionTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
				table.MergeTable ((ItemCompanionTable)updates.Tables[0].GetChanges(DataRowState.Modified));

			if (table == null)
				table= (ItemCompanionTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
			else if ((ItemCompanionTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
				table.MergeTable( (ItemCompanionTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

			if (table==null) return;

			foreach(ItemCompanionRow row in table.Rows)
			{ 
				if (row.CompanionItemID.Trim() == "")  
					ErrorHandler.throwCustomError(POSErrorENUM.ItemCompanion_CompanionItemIDCanNotBeNULL); 
				if (row.ItemID.Trim() == "" )
					ErrorHandler.throwCustomError(POSErrorENUM.ItemCompanion_ItemIDCanNotBeNull);
                //if (row.Amount == 0 && row.Percentage==0 )    //PRIMEPOS-2923 13-Nov-2020 JY commented
                //	ErrorHandler.throwCustomError(POSErrorENUM.ItemCompanion_AmountCanNotBeNull);   //PRIMEPOS-2923 13-Nov-2020 JY commented 
            }
        }

		public ItemCompanionData Populate(System.String CompanionItemID) 
		{
			try 
			{
				using(ItemCompanionSvr dao = new ItemCompanionSvr()) 
				{
					return dao.Populate(CompanionItemID);
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
				logger.Fatal(ex, "Populate(System.String CompanionItemID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}

		public void checkIsValidPrimaryKey(ItemCompanionData updates) 
		{	
			ItemCompanionTable table = (ItemCompanionTable)updates.Tables[clsPOSDBConstants.ItemCompanion_tbl];
			foreach(ItemCompanionRow row in table.Rows)
			{ 
				if (this.Populate (row.CompanionItemID).Tables[clsPOSDBConstants.ItemCompanion_tbl].Rows.Count != 0)
				{ 
					throw new Exception ("Primary key violation for ItemCompanion ");
				}		
			} 
		}	

		// Check whether an attempted delete is valid for ItemCompanion
		public void checkIsValidDelete(ItemCompanionData updates)
		{ 
		}	
		#endregion

		public void Dispose() 
		{
			GC.SuppressFinalize(true);
		}

	}
}
