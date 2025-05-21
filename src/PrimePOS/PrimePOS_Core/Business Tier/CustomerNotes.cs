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
	

	public class CustomerNotes : IDisposable 
	{

		#region Persist Methods

        
		public  void Persist(CustomerNotesData updates) 
		{
			try 
			{
				using(CustomerNotesSvr dao = new CustomerNotesSvr()) 
				{
					//checkIsValidData(updates);
					dao.Persist(updates);
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}			

		public  void DeleteRow(System.Int32 CurrentID) 
		{
			System.Data.IDbConnection oConn=null;
			try 
			{
				oConn=DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
				CustomerNotesSvr oSvr=new CustomerNotesSvr();
				oSvr.DeleteRow(CurrentID);
			}
			catch(Exception ex) 
			{
				throw(ex);
			}
		}

		#endregion

		#region Get Methods

		public  CustomerNotesData Populate(System.Int32 iCustomerID,bool activeOnly) 
		{
			try 
			{
				using(CustomerNotesSvr dao = new CustomerNotesSvr()) 
				{
					return dao.Populate(iCustomerID,activeOnly);
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}

        public CustomerNotesData PopulateByID(System.Int32 iID)
        {
            try
            {
                using (CustomerNotesSvr dao = new CustomerNotesSvr())
                {
                    return dao.PopulateByID(iID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
		
		#endregion


		
		public void Dispose() 
		{
			GC.SuppressFinalize(true);
		}

	}
}
