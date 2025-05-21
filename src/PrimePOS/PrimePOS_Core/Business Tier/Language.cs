using POS.DataTier;
using POS_Core.ErrorLogging;
using System;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using NLog;

namespace POS_Core.BusinessTier
{
    public class Language : IDisposable
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public Language(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }

        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public Language()
        {
            
        }

        /// <summary>
        /// Fills a DataSet with all Notess based on a condition.
        /// </summary>
        /// <param name="whereClause">SQL WHERE CLUASE type condition.</param>
        /// <returns>returns Typed DataSet (NotesData).</returns>
        /// 
        public DataTable PopulateList()
        {
            try
            {
                using (LanguageSvr dao = new LanguageSvr())
                {
                    return dao.PopulateList();
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
                logger.Fatal(ex, "PopulateList()");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
