
namespace POS_Core.BusinessRules  
{
	using System;
	using System.Data;
	using POS_Core.DataAccess;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData;
	using POS_Core.ErrorLogging;

	public class PayOut : IDisposable 
	{

		#region Persist Methods

        
		public  void Persist(PayOutData updates) 
		{
			try 
			{
				using(PayOutSvr dao = new PayOutSvr()) 
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
		#endregion

		#region Get Methods

		public  PayOutData Populate(System.String DeptCode) 
		{
			try 
			{
				using(PayOutSvr dao = new PayOutSvr()) 
				{
					return dao.Populate(DeptCode);
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}
				
		public PayOutData PopulateList(string whereClause) 
		{
			try 
			{
				using(PayOutSvr dao = new PayOutSvr()) 
				{
					return dao.PopulateList(whereClause);
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}

		#endregion


		#region Validation Methods
		public void checkIsValidData(PayOutData updates) 
		{
			PayOutTable table;

			PayOutRow oRow ;
				
			oRow = (PayOutRow)updates.Tables[0].Rows[0];

			table = (PayOutTable)updates.Tables[0].GetChanges(DataRowState.Added);

			if (table == null)
				table= (PayOutTable)updates.Tables[0].GetChanges(DataRowState.Modified);
			else if ((PayOutTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
				table.MergeTable ((PayOutTable)updates.Tables[0].GetChanges(DataRowState.Modified));
		  
			if (table == null)
				table= (PayOutTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
			else if ((PayOutTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
				table.MergeTable( (PayOutTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

			if (table==null) return;

			foreach(PayOutRow row in table.Rows)
			{ 
				if (row.Description.Trim() == "" || row.Description==null)
					ErrorHandler.throwCustomError(POSErrorENUM.PayOut_DescriptionCanNotBeNULL); 
				if (row.Amount==0)
					ErrorHandler.throwCustomError(POSErrorENUM.PayOut_AmountCanNotBeNull); 
			}
		}

		#endregion

		public void Dispose() 
		{
			GC.SuppressFinalize(true);
		}

	}
}
