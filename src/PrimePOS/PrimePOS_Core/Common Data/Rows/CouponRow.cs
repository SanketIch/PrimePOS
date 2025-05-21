

namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
   public class CouponRow : DataRow 
    {
       private CouponTable table;
       internal CouponRow(DataRowBuilder rb): base(rb) 
		{
			this.table = (CouponTable)this.Table;
		}
       #region Public Properties

       public System.Int32 CouponID
       {
           get
           {
               try
               {
                   return (System.Int32)this[this.table.CouponID];
               }
               catch
               {
                   return 0;
               }
           }
         
           set { this[this.table.CouponID] = value; }
       }

       public System.String CouponCode
       {
           get
           {
               try
               {
                   return (System.String)this[this.table.CouponCode];
               }
               catch
               {
                   return "";
               }
           }

           set { this[this.table.CouponCode] = value; }
       }
       public System.String UserID
       {
           get
           {
               try
               {
                   return (System.String)this[this.table.UserID];
               }
               catch
               {
                   return "";
               }
           }

           set { this[this.table.UserID] = value; }
       }

       public System.DateTime StartDate
       {
           get
           {
               try
               {
                   return (System.DateTime)this[this.table.StartDate];
               }
               catch
               {
                   return DateTime.MinValue;
               }
           }

           set { this[this.table.StartDate] = value; }
       }
       public System.DateTime EndDate
       {
           get
           {
               try
               {
                   return (System.DateTime)this[this.table.EndDate];
               }
               catch
               {
                   return DateTime.MinValue;
               }
           }

           set { this[this.table.EndDate] = value; }
       }
       public System.Decimal DiscountPerc
       {
           get
           {
               try
               {
                   return (System.Decimal)this[this.table.DiscountPerc];
               }
               catch
               {
                   return 0;
               }
           }

           set { this[this.table.DiscountPerc] = value; }
       }


       public System.Boolean IsCouponInPercent
       {
           get
           {
               try
               {
                   return (System.Boolean)this[this.table.IsCouponInPercent];
               }
               catch
               {
                   return false;
               }
           }

           set { this[this.table.IsCouponInPercent] = value; }
       }

       public System.DateTime CreatedDate
       {
           get
           {
               try
               {
                   return (System.DateTime)this[this.table.CreatedDate];
               }
               catch
               {
                   return DateTime.MinValue;
               }
           }

           set { this[this.table.CreatedDate] = value; }
       }

       //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added
       public System.String CouponDesc
       {
           get
           {
               try
               {
                   return (System.String)this[this.table.CouponDesc];
               }
               catch
               {
                   return "";
               }
           }

           set { this[this.table.CouponDesc] = value; }
       }

       #endregion Public Properties
    }
}
