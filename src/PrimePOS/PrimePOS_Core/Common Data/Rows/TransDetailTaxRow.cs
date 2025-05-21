
//using POS.Resources;

namespace POS_Core.CommonData.Rows
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    public class TransDetailTaxRow : DataRow
    {
        private TransDetailTaxTable table;

        internal TransDetailTaxRow(DataRowBuilder rb)
            : base(rb)
        {
            this.table = (TransDetailTaxTable)this.Table;
        }


        #region Public Properties
        public System.Int32 ItemRow
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.ItemRow];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.ItemRow] = value;
            }
        }
        public System.Int32 TransDetailTaxID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransDetailTaxID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.TransDetailTaxID] = value;
            }
        }

        public System.Int32 TransDetailID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransDetailID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                this[this.table.TransDetailID] = value;
            }
        }

        public System.Int32 TransID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TransID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TransID] = value; }
        }

        public System.Decimal TaxAmount
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.TaxAmount];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                decimal oldVal = this.TaxAmount;
                try
                {

                    //this[this.table.TaxAmount] = value;   //Sprint-21 21-Sep-2015 JY Commented  
                    #region PRIMEPOS-2640 13-Feb-2019 JY Added
                    if (Configuration.CPOSSet.RoundTaxValue == false)
                        this[this.table.TaxAmount] = Math.Floor(value * 10000) / 10000;
                    else
                        this[this.table.TaxAmount] = Math.Round(value, 4, MidpointRounding.AwayFromZero);
                    #endregion

                    #region PRIMEPOS-2640 13-Feb-2019 JY commented
                    ////this[this.table.TaxAmount] = value;   //Sprint-21 21-Sep-2015 JY Commented  
                    //#region Sprint-21 21-Sep-2015 JY Added to rounf TaxAmount up-to 2 decimals
                    //if (Configuration.CPOSSet.RoundTaxValue == false)
                    //    this[this.table.TaxAmount] = Math.Floor(value * 100) / 100;
                    //else
                    //    this[this.table.TaxAmount] = Math.Round(value, 2);
                    //#endregion
                    #endregion
                }
                catch (Exception exp)
                {
                    this[this.table.TaxAmount] = oldVal;
                    throw (exp);
                }
            }
        }

        public System.Decimal TaxPercent
        {
            get
            {
                try
                {
                    return (System.Decimal)this[this.table.TaxPercent];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                decimal oldVal = this.TaxPercent;
                try
                {

                    this[this.table.TaxPercent] = value;
                }
                catch (Exception exp)
                {
                    this[this.table.TaxPercent] = oldVal;
                    throw (exp);
                }
            }
        }

        public System.String ItemID
        {
            get
            {
                try
                {
                    return (System.String)this[this.table.ItemID];
                }
                catch
                {
                    return "";
                }
            }
            set { this[this.table.ItemID] = value; }
        }


        public System.Int32 TaxID
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.TaxID];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.TaxID] = value; }
        }
        #endregion      

        public void Copy(TransDetailTaxRow oCopyTo)
        {
            oCopyTo.TransDetailTaxID = this.TransDetailTaxID;
            oCopyTo.ItemID = this.ItemID;
            oCopyTo.TaxAmount = this.TaxAmount;
            oCopyTo.TaxID = this.TaxID;
            oCopyTo.TransDetailID = this.TransDetailID;
            oCopyTo.TransID = this.TransID;
            oCopyTo.TaxPercent = this.TaxPercent;

        }

    }
}
