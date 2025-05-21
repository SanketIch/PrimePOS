using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

namespace POS_Core.CommonData.Rows
{
    public class BaseRow : DataRow, INotifyPropertyChanged
    {

        private DataTable table;

        // Constructor
        internal BaseRow(DataRowBuilder rb) : base(rb)
        {
            this.table = this.Table;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
