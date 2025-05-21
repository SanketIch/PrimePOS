// ----------------------------------------------------------------
// ----------------------------------------------------------------

   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

       public class FunctionKeysRow : DataRow
       {
           private FunctionKeysTable table;

           /// <summary>
           /// Constructor
           /// </summary>
           internal FunctionKeysRow(DataRowBuilder rb)
               : base(rb)
           {
               this.table = (FunctionKeysTable)this.Table;
           }
           #region Public Properties

           /// <summary>
           /// Public Property FunKey
           /// </summary>
           public System.Int32 KeyId
           {
               get
               {
                   try
                   {
                       return (System.Int32)this[this.table.KeyId];
                   }
                   catch
                   {
                       return 0;
                   }
               }
               set { this[this.table.FunKey] = value; }
           }

           public System.String FunKey
           {
               get
               {
                   try
                   {
                       return (System.String)this[this.table.FunKey];
                   }
                   catch
                   {
                       return System.String.Empty;
                   }
               }
               set { this[this.table.FunKey] = value; }
           }
           /// <summary>
           /// Public Property Operation
           /// </summary>
           public System.String Operation
           {
               get
               {
                   try
                   {
                       return this[this.table.Operation].ToString();
                   }
                   catch
                   {
                       return System.String.Empty;
                   }
               }
               set { this[this.table.Operation] = value; }
           }

           public System.String ButtonBackColor
           {
               get
               {
                   try
                   {
                       if (this[this.table.ButtonBackColor] == null || this[this.table.ButtonBackColor] == "")
                       {
                           return System.String.Empty;
                       }
                       else
                       {
                           return this[this.table.ButtonBackColor].ToString();
                       }
                   }
                   catch
                   {
                       return System.String.Empty;
                   }
               }
               set { this[this.table.ButtonBackColor] = value; }
           }

           public System.String ButtonForeColor
           {
               get
               {
                   try
                   {
                       return this[this.table.ButtonForeColor].ToString();
                   }
                   catch
                   {
                       return System.String.Empty;
                   }
               }
               set { this[this.table.ButtonForeColor] = value; }
           }
           //Added By Shitaljit on 24 May 2013 for JIRA-932 Assign unlimited touch kyes
           public System.Int32 Parent
           {
               get
               {
                   try
                   {
                       return (System.Int32)this[this.table.Parent];
                   }
                   catch
                   {
                       return 0;
                   }
               }
               set { this[this.table.Parent] = value; }
           }

           public System.String FunctionType
           {
               get
               {
                   try
                   {
                       return this[this.table.FunctionType].ToString();
                   }
                   catch
                   {
                       return System.String.Empty;
                   }
               }
               set { this[this.table.FunctionType] = value; }
           }

           public System.Int32 MainPosition
           {
               get
               {
                   try
                   {
                       return (System.Int32)this[this.table.MainPosition];
                   }
                   catch
                   {
                       return 0;
                   }
               }
               set { this[this.table.MainPosition] = value; }
           }
           public System.Int32 SubPosition
           {
               get
               {
                   try
                   {
                       return (System.Int32)this[this.table.SubPosition];
                   }
                   catch
                   {
                       return 0;
                   }
               }
               set { this[this.table.SubPosition] = value; }
           }

           public System.String Description
           {
               get
               {
                   try
                   {
                       return this[this.table.Description].ToString();
                   }
                   catch
                   {
                       return System.String.Empty;
                   }
               }
               set { this[this.table.Description] = value; }
           }
           //END
           #endregion // Properties
       }
	public class FunctionKeysRowChangeEvent : EventArgs {
		private FunctionKeysRow eventRow;  
		private DataRowAction eventAction;

		public FunctionKeysRowChangeEvent(FunctionKeysRow row, DataRowAction action) {
			this.eventRow = row;
			this.eventAction = action;
		}

		public FunctionKeysRow Row {
			get { return this.eventRow; }
		}

	public DataRowAction Action { 
		get { return this.eventAction; }
	}
}
}
