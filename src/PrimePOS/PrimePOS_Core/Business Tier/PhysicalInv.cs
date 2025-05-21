using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
//using POS.Resources;
using Resources;
using POS_Core.ErrorLogging;
namespace POS_Core.BusinessRules  
{
	

	public class PhysicalInv : IDisposable 
	{

		#region Persist Methods

        
		public  void Persist(PhysicalInvData updates) 
		{
			try 
			{
				using(PhysicalInvSvr dao = new PhysicalInvSvr()) 
				{
					checkIsValidData(updates);
					dao.Persist(updates);
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}			

		public  void ProcessData() 
		{
			System.Data.IDbConnection oConn=null;
			System.Data.IDbTransaction oTrans=null;
			try 
			{
				oConn=DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
				oTrans=oConn.BeginTransaction();
				PhysicalInvSvr oPISvr=new PhysicalInvSvr();
				oPISvr.ProcessData(oTrans);
				oTrans.Commit();
			}
			catch(Exception ex) 
			{
				if (oTrans!=null)
				oTrans.Rollback();
				throw(ex);
			}
		}

		public  void DeleteRow(System.Int32 CurrentID) 
		{
			System.Data.IDbConnection oConn=null;
			try 
			{
				oConn=DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
				PhysicalInvSvr oPISvr=new PhysicalInvSvr();
				oPISvr.DeleteRow(CurrentID);
			}
			catch(Exception ex) 
			{
				throw(ex);
			}
		}

		#endregion

		#region Get Methods
		public  PhysicalInvData Populate(System.Int32 isProcessed) 
		{
			try 
			{
				using(PhysicalInvSvr dao = new PhysicalInvSvr()) 
				{
					return dao.Populate(isProcessed);
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}
        				
		public PhysicalInvData PopulateList(string whereClause) 
		{
			try 
			{
				using(PhysicalInvSvr dao = new PhysicalInvSvr()) 
				{
					return dao.PopulateList(whereClause);
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}

        #region PRIMEPOS-2396 11-Jun-2018 JY Added logic to get last inventory updated quantity
        public System.Int32 GetLastInvUpdatedQty(string ItemId)
        {
            try
            {
                using (PhysicalInvSvr dao = new PhysicalInvSvr())
                {
                    return dao.GetLastInvUpdatedQty(ItemId);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #endregion

        #region Validation Methods
        public void checkIsValidData(PhysicalInvData updates) 
		{
			PhysicalInvTable table;

			PhysicalInvRow oRow ;
				
			oRow = (PhysicalInvRow)updates.Tables[0].Rows[0];

			table = (PhysicalInvTable)updates.Tables[0].GetChanges(DataRowState.Added);

			if (table == null)
				table= (PhysicalInvTable)updates.Tables[0].GetChanges(DataRowState.Modified);
			else if ((PhysicalInvTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
				table.MergeTable ((PhysicalInvTable)updates.Tables[0].GetChanges(DataRowState.Modified));
		  
			if (table == null)
				table= (PhysicalInvTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
			else if ((PhysicalInvTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
				table.MergeTable( (PhysicalInvTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

			if (table==null) return;

			foreach(PhysicalInvRow row in table.Rows)
			{ 
				if (row.ItemCode.Trim() == "" || row.Description==null)
					ErrorHandler.throwCustomError(POSErrorENUM.Item_CodeCanNotBeNULL); 
			}
		}

        public void Validate_ItemCode(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_ItemCodeCanNotNull);
            }
            else
            {
                ItemData oID = new ItemData();
                Item oI = new Item();
                oID = oI.Populate(strValue);
                if (oID == null)
                {
                    throw (new Exception("Invalid Item code"));
                }
                else if (oID.Item.Rows.Count == 0)
                {
                    throw (new Exception("Invalid Item code"));
                }
            }
        }

		//PRIMEPOS-3040 21-Dec-2021 JY Commented
		//public void Validate_Qty(string strValue)
		//{
		//    if (strValue == null || strValue.Trim() == "")
		//    {
		//        ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_QTyCanNotNull);
		//    }
		//}

		public void Validate_Cost(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_CostCanNotBeNull);
            }
        }

        #endregion

        public void Dispose() 
		{
			GC.SuppressFinalize(true);
		}

	}
}
