using System;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;


namespace POS_Core.CommonData.Rows
{
   public class FileHistoryRow : DataRow 
    {

        private FileHistoryTable table;

		// Constructor
        internal FileHistoryRow(DataRowBuilder rb):base(rb) 
		{
            this.table = (FileHistoryTable)this.Table;
		}
		
        #region Public Properties

        // Public Property FileID
		public System.Int64 FileID
		{
			get { 
				try {
                    return (System.Int64)this[this.table.FileID];
				}
					catch{
                        return 0; 
				}
			} 
			set {

                 this[this.table.FileID] = value;
            }
		}

        // Public Property LastUpdateDate
		public System.DateTime LastUpdateDate
		{
			get 
			{ 
				try 
				{
                    return (System.DateTime)this[this.table.LastUpdateDate];
				}
				catch
				{ 
					return System.DateTime.MinValue; 
				}
			}
            set { this[this.table.LastUpdateDate] = value; }
        }

       public bool SynchronizedCentrally
       {
           get
           {
               return (bool) this[this.table.SynchronizedCentrally];
           }
           set
           {
               this[this.table.SynchronizedCentrally] = value;
           }
       }
        #endregion
    }
}
