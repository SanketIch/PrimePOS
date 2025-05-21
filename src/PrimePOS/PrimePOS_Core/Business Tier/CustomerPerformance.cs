using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using POS.Resources;

namespace POS_Core.BusinessRules
{
    #region  Declaration
    
    #endregion
    public class CustomerPerformance
    {
        public DataSet getCustPerformData(int custmerID, DateTime FromDate, DateTime ToDate)
        {
            DataSet dsCustData = new DataSet();
            try
            {
                CustomerPerformanceSvr custSvr = new CustomerPerformanceSvr();
                dsCustData = custSvr.getCustPerformData(custmerID, FromDate,ToDate);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return dsCustData;
        }
    }


  
}
