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

	// ItemMonitorCategoryDetails Business Rules Class  

	public class ItemMonitorCategoryDetail : IDisposable 
	{
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // A method for inserting and updating ItemMonitorCategoryDetail data.

        public  void Persist(ItemMonitorCategoryDetailData updates) 
		{
			
			try 
			{	
			
				using(ItemMonitorCategoryDetailSvr dao = new ItemMonitorCategoryDetailSvr()) 
				{
					dao.Persist(updates);
				}
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "Persist(ItemMonitorCategoryDetailData updates)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Persist(ItemMonitorCategoryDetailData updates)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Persist(ItemMonitorCategoryDetailData updates)");
                //ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
			}
		}			
		#endregion

		#region Get Methods

		// Fills a DataSet with all ItemMonitorCategoryDetails based on a condition.
		public ItemMonitorCategoryDetailData PopulateList(string whereClause) 
		{
			try 
			{
				using(ItemMonitorCategoryDetailSvr dao = new ItemMonitorCategoryDetailSvr()) 
				{
					return dao.PopulateList(whereClause);
				}
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "PopulateList(string whereClause)");
                //ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}

		#endregion

		#region Validation Methods

		public ItemMonitorCategoryDetailData Populate(System.String ItemID) 
		{
			try 
			{
				using(ItemMonitorCategoryDetailSvr dao = new ItemMonitorCategoryDetailSvr()) 
				{
					return dao.Populate(ItemID);
				}
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.String ItemID)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.String ItemID)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Populate(System.String ItemID)");
                //ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
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
