using System;
using System.Collections;
namespace POS_Core.CommonData
{
    /// <summary>
    /// Summary description for EndOFDayData.
    /// </summary>
    public class EndOFDayData
    {
        private System.Decimal m_TotalSale = 0;
        private System.Decimal m_TotalReturn = 0;
        private System.Decimal m_TotalDiscount = 0;
        private System.Decimal m_NetSale = 0;
        private System.Decimal m_SalesTax = 0;
        private System.Decimal m_TotalCash = 0;
        private System.Decimal m_ReceiveOnAccount = 0;
        private System.Decimal m_GrandTotal = 0;
        private System.Decimal m_PayTypeTotal = 0;
        //private System.Decimal m_TotalCashPT = 0;
        private System.Decimal m_Payout = 0;
        private string m_EODID = "";
        //private System.Decimal m_NetCash = 0;
        private colEndOfDayDetail m_Details = new colEndOfDayDetail();
        private colDepartments m_Departments = new colDepartments();
        private string m_CloseDate = string.Empty;  //PRIMEPOS-2480 26-Jun-2020 JY Added
        private System.Decimal m_TransFee = 0;  //PRIMEPOS-3118 03-Aug-2022 JY Added

        public EndOFDayData()
        {
        }

        public System.Decimal NetCash
        {
            get
            {
                return this.TotalCashPT - m_Payout;
            }
        }

        public System.Decimal Payout
        {
            get
            {
                return m_Payout;
            }
            set { this.m_Payout = value; }
        }

        public System.String EODID
        {
            get
            {
                return m_EODID;
            }
            set { this.m_EODID = value; }
        }

        public System.Decimal Total
        {
            get
            {
                return this.Details.TotalCash;
            }
        }

        public System.Decimal TotalCashPT
        {
            get
            {
                return GetCash();
            }
        }

        public System.Decimal GetCash()
        {
            System.Decimal CashAmount = 0;
            for (int i = 0; i < this.Details.Count; i++)
            {
                if (this.Details[i].PayTypeName == "Cash")
                    CashAmount = this.Details[i].Amount;
            }
            return CashAmount;
        }

        public System.Decimal PayTypeTotal
        {
            get
            {
                return m_PayTypeTotal;
            }
            set { this.m_PayTypeTotal = value; }
        }

        public System.Decimal GrandTotal
        {
            get
            {
                return TotalCash + m_ReceiveOnAccount + m_TransFee; //PRIMEPOS-3118 03-Aug-2022 JY Added m_TransFee
            }
            set { this.m_GrandTotal = value; }
        }

        public System.Decimal ReceiveOnAccount
        {
            get
            {
                return m_ReceiveOnAccount;
            }
            set { this.m_ReceiveOnAccount = value; }
        }

        public System.Decimal TotalCash
        {
            get
            {
                return this.NetSale + m_SalesTax;
            }
            set { this.m_TotalCash = value; }
        }

        public System.Decimal SalesTax
        {
            get
            {
                return m_SalesTax;
            }
            set { this.m_SalesTax = value; }
        }

        public System.Decimal NetSale
        {
            get
            {
                return m_TotalSale - m_TotalDiscount + m_TotalReturn;
            }
            set { this.m_NetSale = value; }
        }

        public System.Decimal TotalDiscount
        {
            get
            {
                return m_TotalDiscount;
            }
            set { this.m_TotalDiscount = value; }
        }

        public System.Decimal TotalReturn
        {
            get
            {
                return m_TotalReturn;
            }
            set { this.m_TotalReturn = value; }
        }


        public System.Decimal TotalSale
        {
            get
            {
                return m_TotalSale;
            }
            set { this.m_TotalSale = value; }
        }

        public colEndOfDayDetail Details
        {
            get
            {
                return m_Details;
            }
        }
        public colDepartments Departments
        {
            get
            {
                return m_Departments;
            }
        }

        #region PRIMEPOS-2480 26-Jun-2020 JY Added
        public string CloseDate
        {
            get
            {
                return m_CloseDate;
            }
            set { this.m_CloseDate = value; }
        }
        #endregion

        //PRIMEPOS-3118 03-Aug-2022 JY Added
        public System.Decimal TransFee
        {
            get
            {
                return m_TransFee;
            }
            set { this.m_TransFee = value; }
        }
    }

    public class colEndOfDayDetail
    {

        ArrayList m_oCol = new ArrayList();

        public int Count
        {
            get
            {
                return this.m_oCol.Count;
            }
        }

        public System.Decimal TotalCash
        {
            get
            {
                return CountTotalCash();
            }
        }

        private System.Decimal CountTotalCash()
        {
            System.Decimal totalAmount = 0;
            for (int i = 0; i < m_oCol.Count; i++)
            {
                totalAmount = totalAmount + this[i].Amount;
            }
            return totalAmount;
        }

        public void Clear()
        {
            m_oCol.Clear();
        }

        public int Add(EndOFDayDetail Value)
        {
            return m_oCol.Add(Value);
        }


        public EndOFDayDetail this[int index]
        {
            get
            {
                return (EndOFDayDetail)m_oCol[index];
            }
        }
    }
    public class colDepartments
    {

        ArrayList m_oCol = new ArrayList();

        public int Count
        {
            get
            {
                return this.m_oCol.Count;
            }
        }


        public void Clear()
        {
            m_oCol.Clear();
        }

        public int Add(Departments Value)
        {
            return m_oCol.Add(Value);
        }


        public Departments this[int index]
        {
            get
            {
                return (Departments)m_oCol[index];
            }
        }
    }

    public class EndOFDayDetail
    {

        private string m_PayTypeName = "";
        private System.Decimal m_Amount = 0;

        public System.String PayTypeName
        {
            get
            {
                return m_PayTypeName;
            }
            set { this.m_PayTypeName = value; }
        }
        public System.Decimal Amount
        {
            get
            {
                return m_Amount;
            }
            set { this.m_Amount = value; }
        }

    }
    public class Departments
    {

        private string m_DepartmentId = "";
        private string m_DepartmentName = "";
        private System.Decimal m_Sales = 0;
        private System.Decimal m_Tax = 0;
        private System.Decimal m_Discount = 0;

        public System.String DepartmentId
        {
            get
            {
                return m_DepartmentId;
            }
            set { this.m_DepartmentId = value; }
        }

        public System.String DepartmentName
        {
            get
            {
                return m_DepartmentName;
            }
            set { this.m_DepartmentName = value; }
        }

        public System.Decimal Sales
        {
            get
            {
                return m_Sales;
            }
            set
            {
                try
                {
                    this.m_Sales = value;
                }
                catch (Exception)
                {
                    this.m_Tax = 0;
                }
            }
        }

        public System.Decimal Tax
        {
            get
            {
                return m_Tax;
            }
            set
            {
                try
                {
                    this.m_Tax = value;
                }
                catch (Exception)
                {
                    this.m_Tax = 0;
                }
            }
        }

        public System.Decimal Discount
        {
            get
            {
                return m_Discount;
            }
            set
            {
                try
                {
                    this.m_Discount = value;
                }
                catch (Exception)
                { this.m_Discount = 0; }
            }
        }        
    }
}
