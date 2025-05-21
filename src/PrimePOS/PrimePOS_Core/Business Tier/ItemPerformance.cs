using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using POS_Core.DataAccess;

namespace POS_Core.BusinessRules
{
    public class ItemPerformance
    {
        public DataSet getItemPerformData(string ItemID, DateTime FromDate, DateTime ToDate)
        {
            DataSet dsItemData = new DataSet();
            try
            {
                ItemPerformanceSvr itemSvr = new ItemPerformanceSvr();
                dsItemData = itemSvr.getItemPerformData(ItemID, FromDate, ToDate);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return dsItemData;
        }
    }
}
