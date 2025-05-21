using POS_Core.CommonData;
using POS_Core.Resources;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

using Infragistics.Win.UltraWinGrid;
using POS_Core.CommonData.Rows;
using System.Windows.Forms;
using POS_Core.Resources.DelegateHandler;
using POS_Core.Resources;
using POS_Core.TransType;

namespace POS_Core.BusinessRules
{
    public partial class POSPayTypeList
    {
        #region Moved from Screeen and declare variable

        public Decimal totalAmount;
        public Decimal totalTaxAmt;
        public Decimal totalEBTAmount;
        public Decimal totalEBTItesTaxAmount;
        public Decimal totalIIASAmount;
        public Decimal totalIIASRxAmount;
        public Decimal totalIIASNonRxAmount;
        public Decimal totalIIASAmountPaid = 0;
        public Decimal totalNonIIASAmountPaid = 0;
        public Decimal totalIIASRxAmountPaid = 0;
        public Decimal totalIIASNonRxAmountPaid = 0;
        public bool isROA;
        public bool CancelTransaction;
        public Decimal tenderedAmount;
        public POSTransactionType oTransactionType;
        public string sSigPadTransID = "";
        public string oPayTpes;
        public String sTransactionType = string.Empty;
        public bool allowCouponPayment = false;
        public decimal ClCouponAmount = 0;
        public bool _Tokenize = false;
        public Decimal ChangeDue;
        public decimal maxClCouponAmount = 0;
        public static Decimal TransactionAmount = 0; //Sprint-25 - PRIMEPOS-2411 21-Apr-2017 JY Added 
        public bool bIsDelivery = false;
        public int iCustomerID = 0;

        public decimal amountBalanceDue = 0; // Add this for refector
        public decimal amountCashBack = 0;
        public decimal amountPaid = 0;

        public string LastKey;
        public bool is_F10 = false; // Add this to the Variable section by Manoj 8/23/2011
        public decimal TransFeeAmt = 0; //PRIMEPOS-3117 11-Jul-2022 JY Added
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #endregion Moved from Screeen and declare variable

        #region   Added for Solutran - PRIMEPOS-2663 - NileshJ
        public string S3TransID = "0";
        public decimal S3PurAmount = 0;
        public decimal S3TaxAmount = 0;
        public decimal S3DiscAmount = 0;
        #endregion

        #region BatchDelivery - NileshJ - PRIMERX-7688
        public bool IsBatchDelivery = false;
        public decimal BatchDelTotalPaidAmount = 0;
        #endregion

        #region Constructor
        public POSPayTypeList()
        {
            oPOSTransPayment_CCLogList = new POSTransPayment_CCLogList();//Added by Prog1
            oPOSTransPaymentData = new POSTransPaymentData();
            oPOSTransPaymentDataPaid = new POSTransPaymentData();
            oRXHeaderList = new RXHeaderList(); //Added by Naim

        }

        #endregion Constructor

        #region Properties

        public POSTransPayment_CCLogList oPOSTransPayment_CCLogList { get; set; }
        public POSTransPayment_CCLogList POSTransPaymentList_CCLogList
        {
            get { return oPOSTransPayment_CCLogList; }
        }
        public POSTransPaymentData oPOSTransPaymentData { get; set; }
        public POSTransPaymentData oPOSTransPaymentDataPaid { get; set; }

        public PccCardInfo CustomerCardInfo;

        public CLCardsRow oCLCardRow = null;

        public CustomerRow oCurrentCustRow = null;

        public RXHeaderList oRXHeaderList { get; set; }

        public RXHeaderList RXHeaderList
        {
            get { return oRXHeaderList; }
            set { oRXHeaderList = value; }
        }

        public int CustomerID
        {
            get { return iCustomerID; }
            set { iCustomerID = value; }
        }

        #region StoreCredit PRIMEPOS-2747 - NileshJ - 20-Nov-2019
        public StoreCreditData oStoreCreditData = null;
        public bool IsStoreCredit = false;
        #endregion

        #endregion Properties

        #region Methods
        public void VerifyHousechargeAcc(ref string sHousechargeAccountNo)
        {
            try
            {
                if (sHousechargeAccountNo != "" && sHousechargeAccountNo != "0")
                {
                    MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                    DataSet oDS = new DataSet();
                    //oAcct.GetAccountByCode(sHousechargeAccountNo, out oDS);
                    oAcct.GetAccountByCode(sHousechargeAccountNo, out oDS, true);   //PRIMEPOS-2888 28-Aug-2020 JY Added third parameter as "true" to get the exact HouseCharge record
                    if (oDS == null || oDS.Tables[0].Rows.Count == 0)
                    {
                        clsCoreUIHelper.ShowBtnIconMsg("Assigned Account# : " + sHousechargeAccountNo.ToString() + "  Does Not Exist", "Charge Account Not Found", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
                        sHousechargeAccountNo = "";
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "VerifyHouseChargeAcc()");
                throw exp;
            }
        }


        public bool CheckMaxTenderAmt(Decimal dCashPayment, ref bool bTenderedAmountOverrideCancel, ref string strMaxTenderedAmountOverrideUser) //PRIMEPOS-2619 16-Nov-2018 JY modified
        {
            try
            {
                //UserManagement.clsLogin oLogin = new POS_Core.UserManagement.clsLogin();
                POSPayTypeList.TransactionAmount = dCashPayment;
                string sUserID = string.Empty;
                if (clsCoreLogin.loginForPreviliges(clsPOSDBConstants.UserMaxTenderedAmountLimit, "", out sUserID, "Security Override For Maximum Tendered Amount Limit") == false)// NileshJ - Add Delegate
                {
                    POSPayTypeList.TransactionAmount = 0;
                    bTenderedAmountOverrideCancel = true;
                    return false;
                }
                else //PRIMEPOS-2402 20-Jul-2021 JY Added
                {
                    strMaxTenderedAmountOverrideUser = sUserID;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CheckMaxTenderAmt()");
                throw exp;
            }
            return true;

        }

        public void CheckTotalPaidAmount(Decimal totalPaid, ref bool retVal)
        {
            try
            {
                if (totalPaid > (this.totalAmount + TransFeeAmt + this.amountCashBack)) //PRIMEPOS-3117 11-Jul-2022 JY modified
                {
                    if (totalPaid - (this.totalAmount + TransFeeAmt + this.amountCashBack) >= 1000) //PRIMEPOS-3117 11-Jul-2022 JY modified
                    {
                        retVal = true;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CheckTotalPaidAmount()");
                throw exp;
            }
        }


        public void RemoveZeroTrans()
        {
            try
            {
                DataRow oRow = null;
                if (oPOSTransPaymentDataPaid != null && oPOSTransPaymentDataPaid?.Tables.Count > 0)
                {
                    for (int i = 0; i < oPOSTransPaymentDataPaid.Tables[0].Rows.Count; i++)
                    {
                        oRow = oPOSTransPaymentDataPaid.Tables[0].Rows[i];
                        if (Configuration.CPOSSet.AllowZeroAmtTransaction == false)
                        {
                            if (Convert.ToDecimal(oRow[clsPOSDBConstants.POSTransPayment_Fld_Amount]) == 0)
                            {
                                oPOSTransPaymentDataPaid.Tables[0].Rows.RemoveAt(i);
                                i = i - 1;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "RemoveZeroTrans()");
                throw exp;
            }

        }


        // Added By Farman Ansari for get house charge account number
        public string GetHouseChargeAccount()
        {
            string sHousechargeAccountNo = string.Empty;
            logger.Trace("GetHouseChargeAccount() - " + clsPOSDBConstants.Log_Entering);
            if (oRXHeaderList.Count == 0 && iCustomerID > 0)
            {
                using (Customer oCustomer = new Customer())
                {
                    using (CustomerData oCustomerData = oCustomer.PopulateList(" Where CustomerID=" + iCustomerID.ToString()))
                    {
                        if (oCustomerData.Customer.Rows.Count > 0)
                        {
                            if (Configuration.CPOSSet.UsePrimeRX && oCustomerData.Customer[0].PatientNo > 0)    //PRIMEPOS-3106 13-Jul-2022 JY modified
                            {
                                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                                DataTable oDataTablePat = oPharmBL.GetPatient(oCustomerData.Customer[0].PatientNo.ToString());
                                if (oDataTablePat.Rows.Count > 0 && oDataTablePat.Rows[0]["acct_no"].ToString().Trim().Length > 0)
                                {
                                    sHousechargeAccountNo = oDataTablePat.Rows[0]["acct_no"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            else if (oRXHeaderList.Count > 0)
            {
                DataTable oDataTablePat = oRXHeaderList[0].TblPatient;
                if (oDataTablePat == null || oDataTablePat.Rows.Count == 0)
                {
                    PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                    oDataTablePat = oPharmBL.GetPatient(oRXHeaderList[0].PatientNo);
                }
                if (oDataTablePat.Rows.Count > 0 && oDataTablePat.Rows[0]["acct_no"].ToString().Trim().Length > 0)
                {
                    sHousechargeAccountNo = oDataTablePat.Rows[0]["acct_no"].ToString();
                }
            }

            logger.Trace("GetHouseChargeAccount() - " + clsPOSDBConstants.Log_Exiting);
            return sHousechargeAccountNo;
        }
        // end


        //  Added by Farman for screen Amount Display
        public string FormatPoleDisplayText(Decimal totalAmount, Decimal txtAmtCashBack, Decimal totalTaxAmt)
        {
            String strPoleData = String.Empty;
            String strText = "Total Due:";
            String strDue = Convert.ToDecimal(totalAmount + Convert.ToDecimal(txtAmtCashBack)).ToString(Configuration.CInfo.CurrencySymbol + "##########0.00").ToString();
            strPoleData = strText.PadRight(Configuration.CPOSSet.PD_LINELEN - 9) + " " + strDue.PadLeft(8);
            if (Configuration.CPOSSet.PD_LINES > 1)
            {
                strText = "Tax: ";
                strPoleData += strText.PadRight(Configuration.CPOSSet.PD_LINELEN - 7) + " " + totalTaxAmt.ToString(Configuration.CInfo.CurrencySymbol + "##########0.00");
            }
            return strPoleData;
        }
        // End

        public string GetPayType(string paymentType)
        {
            logger.Trace("GetPayType() - " + clsPOSDBConstants.Log_Entering);
            string returnValue = string.Empty;
            switch (paymentType)
            {
                case "1":
                    returnValue = PayTypes.Cash;
                    break;
                case "2":
                    returnValue = PayTypes.Cheque;
                    break;
                case "3":
                case "4":
                case "5":
                case "6":
                case "-99":
                case "DISOVER":
                    returnValue = PayTypes.CreditCard;
                    break;
                case "E":
                    returnValue = PayTypes.EBT;
                    break;
                case "7":
                    returnValue = PayTypes.DebitCard;
                    break;
                default:
                    PayType oPayType = new PayType();
                    DataSet dsPpyType = oPayType.Populate(paymentType);
                    if (dsPpyType != null)
                    {
                        if (dsPpyType.Tables[0].Rows.Count > 0)
                        {
                            returnValue = Configuration.convertNullToString(dsPpyType.Tables[0].Rows[0][clsPOSDBConstants.PayType_Fld_PayType].ToString());
                        }
                    }
                    break;
            }
            logger.Trace("GetPayType() - " + clsPOSDBConstants.Log_Exiting);
            return returnValue;
        }

        public bool WarnForOverPayment(string ActiveRowTranTypeCode, decimal CurrentPaytypeAmount, decimal TotaldueAmount, POSTransactionType oTransactionType, string oPayTpes)
        {
            bool RetVal = false;
            try
            {
                if (oTransactionType == POSTransactionType.Sales)
                {
                    oPayTpes = ActiveRowTranTypeCode;
                    if (oPayTpes == PayTypes.Cheque || oPayTpes == PayTypes.CreditCard || oPayTpes == PayTypes.DebitCard)
                    {
                        RetVal = (CurrentPaytypeAmount > TotaldueAmount);
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "WarnForOverPayment()");
                throw Ex;
            }
            return RetVal;
        }

        public string GetCustomerName(ref CLCouponsRow oCLCouponsRow)
        {

            #region 10-Apr-2015 JY Added code to get customer name

            CLCardsData oCLCardsData1 = new CLCardsData();
            CLCards oBRCLCards1 = new CLCards();
            CLCardsRow oCLCardsRow1;
            oCLCardsData1 = oBRCLCards1.GetByCLCardID(oCLCouponsRow.CLCardID);
            oCLCardsRow1 = oCLCardsData1.CLCards[0];

            Customer oCustomer1 = new Customer();
            CustomerData oCustomerData1;
            oCustomerData1 = oCustomer1.Populate(oCLCardsData1.CLCards[0].CustomerID.ToString(), true);
            string strCustName = oCustomerData1.Customer[0].CustomerName;
            clsCoreUIHelper.ShowErrorMsg("Coupon does not belong to selected customer. \nSelected coupon belongs to: \nCustomer Name: " + strCustName + "\nCard ID: " + oCLCouponsRow.CLCardID.ToString());  //10-Apr-2015 JY Added changed message
            return strCustName;


            #endregion
        }

        public void GetAllDeliveryAddress(Customer oCustomer, CustomerData oCustdata, CustomerRow oCustRow, ref string strCode)
        {
            try
            {
                if (oCustRow != null)
                {
                    oCustdata.Tables[0].ImportRow(oCustRow);
                    oCustomer.Persist(oCustdata, true);
                    oCustdata = oCustomer.GetCustomerByPatientNo(oCustRow.PatientNo);
                    if (oCustdata.Tables[0].Rows.Count > 0)
                    {
                        oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                        strCode = oCustRow.CustomerId.ToString();
                    }
                    oCurrentCustRow = oCustRow;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetAllDeliveryAddress()");
                throw exp;
            }

        }

        public decimal GetIIASRxAmount(Decimal totalNonIIASAmount)
        {

            logger.Trace("GetIIASRxAmount() - " + clsPOSDBConstants.Log_Entering);
            decimal returnAmount = 0;
            decimal TotalIIASAmountRemain = GetIIASAmount(totalNonIIASAmount);
            decimal TotalIIASAmountActPaid = this.totalIIASAmount - TotalIIASAmountRemain;
            //Check for if IIASRXAmount is not 'zero'
            //Check for Total IIAS Amount Paid,
            //Check for
            try
            {
                if (totalIIASRxAmount != 0)
                {
                    if (this.totalIIASAmount > this.totalIIASAmountPaid)
                    {
                        if (TotalIIASAmountActPaid < this.totalIIASRxAmount)
                        {
                            returnAmount = this.totalIIASRxAmount - TotalIIASAmountActPaid;
                        }
                        else if (TotalIIASAmountActPaid >= this.totalIIASRxAmount)
                        {
                            returnAmount = 0;
                        }
                    }
                }
            }

            catch (Exception exp)
            {
                logger.Fatal(exp, "GetIIASRxAmount()");
                throw exp;
            }
            logger.Trace("GetIIASRxAmount() - " + clsPOSDBConstants.Log_Exiting);
            return returnAmount;
        }

        public decimal GetIIASAmount(Decimal totalNonIIASAmount)
        {
            logger.Trace("GetIIASAmount() - " + clsPOSDBConstants.Log_Entering);
            decimal returnAmount = 0;
            try
            {
                if (this.totalIIASAmount != 0)
                {
                    if (this.totalIIASAmount > totalIIASAmountPaid)
                    {
                        if (totalNonIIASAmount == 0)
                        {
                            returnAmount = this.totalIIASAmount - this.totalIIASAmountPaid;
                        }
                        else if (this.totalNonIIASAmountPaid > totalNonIIASAmount)
                        {
                            returnAmount = this.totalIIASAmount - (this.totalNonIIASAmountPaid - totalNonIIASAmount);
                        }
                        else
                        {
                            //Modified by Dharmendra SRT on Jan-28-09
                            //substracted  totalIIASAmountPaid from totalIIASAmount
                            returnAmount = this.totalIIASAmount - this.totalIIASAmountPaid;
                            //Modified Till Here Jan-28-09
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetIIASAmount()");
                throw exp;
            }
            logger.Trace("GetIIASAmount() - " + clsPOSDBConstants.Log_Exiting);
            return returnAmount;
        }

        public void ComputeIIASAmounts(Decimal totalNonIIASAmount)
        {
            try
            {
                Decimal totalRxPending;
                totalRxPending = this.GetIIASRxAmount(totalNonIIASAmount);
                this.totalIIASRxAmountPaid = this.totalIIASRxAmount - totalRxPending;
                this.totalIIASNonRxAmountPaid = this.totalIIASAmountPaid - this.totalIIASRxAmountPaid;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ComputeIIASAmounts()");
                throw exp;
            }
        }
        public void RemoveEBTTaxAndGetTotals(bool removeEBTTax, ref Decimal totalPaid, ref Decimal CashbackAmount)
        {
            try
            {
                foreach (POSTransPaymentRow oRow in this.oPOSTransPaymentDataPaid.POSTransPayment.Rows)
                {
                    if (oRow.Amount > 0 && this.totalEBTAmount > 0 && this.oPayTpes == PayTypes.EBT && this.totalEBTItesTaxAmount > 0)
                    {
                        //Added ny shitaljit on 26Dec2013 for Remove Tax from EBT items.
                        if (removeEBTTax == true)
                        {
                            this.totalAmount = this.totalAmount - this.totalEBTItesTaxAmount;
                            this.totalTaxAmt = this.totalTaxAmt - this.totalEBTItesTaxAmount;
                            //this.txtAmtTotal.Text = this.totalAmount.ToString("##########0.00"); //Data Binding ??
                            this.totalEBTItesTaxAmount = 0;
                        }
                    }
                    if (oRow.TransTypeCode.Trim() != "B")
                    {
                        totalPaid += oRow.Amount;
                        if (oRow.IsIIASPayment == true)
                        {
                            this.totalIIASAmountPaid += oRow.Amount;
                        }
                        else
                        {
                            this.totalNonIIASAmountPaid += oRow.Amount;
                        }
                    }

                    if (oRow.TransTypeCode.Trim() == "B")
                    {
                        CashbackAmount = Math.Abs(oRow.Amount);
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "RemoveEBTTaxAndGetTotal()");
                throw exp;
            }
        }

        public void GetTotalPaidAmount(ref Decimal totalPaid, ref Decimal dCashPayment)
        {
            try
            {
                foreach (POSTransPaymentRow oRow in oPOSTransPaymentDataPaid.POSTransPayment.Rows)
                {
                    totalPaid += oRow.Amount;
                    if (oRow.TransTypeCode.Trim() == "1") dCashPayment = oRow.Amount; //Sprint-25 - PRIMEPOS-2411 24-Apr-2017 JY Added 
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTotalPaidAmount()");
                throw exp;
            }


        }

        public decimal GetPendingEBTAmount(Decimal totalNonIIASAmount)
        {
            logger.Trace("GetPendingEBTAmount() - " + clsPOSDBConstants.Log_Entering);

            decimal paidUsingEBT = 0;
            foreach (POSTransPaymentRow oRow in this.oPOSTransPaymentDataPaid.POSTransPayment.Rows)
            {
                try
                {
                    if (oRow.TransTypeCode.Trim() == "E")
                    {
                        paidUsingEBT += oRow.Amount;
                    }
                }
                catch (Exception exp)
                {
                    logger.Fatal(exp, "GetPendingEBTAmount()");
                    throw exp;
                }
            }

            decimal returnValue = 0;
            try
            {

                if (paidUsingEBT == this.totalEBTAmount)
                {
                    returnValue = 0;
                }
                else if (paidUsingEBT == 0)
                {
                    if ((this.amountBalanceDue > this.totalEBTAmount))
                    {
                        //Comment out by Manoj 7/24/2014. Ebt Calculating wrong when you have a discount and tax.
                        //returnValue = totalEBTAmount;  
                        returnValue = this.totalEBTAmount;
                    }
                    else if (Configuration.CPOSSet.PaymentProcessor != "WORLDPAY" && this.totalEBTAmount > 0 && totalNonIIASAmount < 1)
                    {
                        returnValue = this.totalEBTAmount - Math.Abs(totalNonIIASAmount);
                    }
                    else
                    {
                        returnValue = this.amountBalanceDue;
                    }
                }
                else
                {
                    decimal remainingEBTAmount = this.totalEBTAmount - paidUsingEBT;
                    if ((this.amountBalanceDue > remainingEBTAmount))
                    {
                        returnValue = remainingEBTAmount;
                    }
                    else
                    {
                        returnValue = this.amountBalanceDue;
                    }
                }
                //ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction_Paymnet, "GetPendingEBTAmount()", clsPOSDBConstants.Log_Exiting);
                logger.Trace("GetPendingEBTAmount() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetPendingEBTAmount()");
                throw exp;
            }
            return returnValue;
        }

        public void AllowCouponPayment()
        {
            try
            {
                string sUserID = Configuration.UserName;
                if (this.allowCouponPayment == false && UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.MakeCouponPayment.ID, UserPriviliges.Permissions.MakeCouponPayment.Name, out sUserID) == false)
                {
                    return;
                }
                else
                { this.allowCouponPayment = true; }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AllowCouponPayment()");
                throw exp;
            }
        }

        public void ProcessDebitCard()
        {
            try
            {
                if (oPayTpes == PayTypes.DebitCard)
                {
                    //PRIMEPOS-2664 ADDED BY ARVIND FOR EVERTEC
                    //Added by Arvind PRIMEPOS-2636
                    if (Configuration.CPOSSet.UsePinPad == false && Configuration.CPOSSet.PaymentProcessor != "XLINK" && Configuration.CPOSSet.PaymentProcessor != "HPS"
                        && Configuration.CPOSSet.PaymentProcessor.ToUpper() != "WORLDPAY" && Configuration.CPOSSet.PaymentProcessor.ToUpper() != "HPSPAX" && Configuration.CPOSSet.PaymentProcessor.ToUpper() != "EVERTEC" && Configuration.CPOSSet.PaymentProcessor.ToUpper() != "VANTIV" && Configuration.CPOSSet.PaymentProcessor.ToUpper() != "ELAVON") // HPSPAX added by Suraj//2943
                    {
                        oPayTpes = string.Empty;
                        clsCoreUIHelper.ShowBtnErrorMsg("Debit option is currently not supported.\r\n Please try some other payment option.", "Payment Failure", MessageBoxButtons.OK);
                        LastKey = "Esc";//Added by SRT (3-Nov-08).
                        return;
                    }

                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ProcessDebitCard()");
                throw exp;
            }
        }

        // Added by Farman Ansari 21/12/2017 for ProcessCouponID
        public bool IsValidCoupon(Int64 clCouponID)
        {
            try
            {
                int scannedCouponsCount = 0;
                foreach (POSTransPaymentRow oRow in oPOSTransPaymentDataPaid.POSTransPayment.Rows)
                {
                    if (oRow.CLCouponID == clCouponID)
                    {
                        clsCoreUIHelper.ShowErrorMsg("Coupon ID is already used in current transaction.");
                        return false;
                    }
                    else if (oRow.CLCouponID > 0)
                    {
                        scannedCouponsCount++;
                    }
                }

                if (scannedCouponsCount > 0)
                {
                    clsCoreUIHelper.ShowErrorMsg("Multiple coupons are not allowed in one transaction.");
                    return false;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IsValidCoupon()");
                throw exp;
            }
            return true;
        }

        public CLCouponsRow ApplyCoupon(Int64 clCouponID, Decimal amount, ref decimal calculatedCouponValue, ref decimal remainingclamount)
        {
            CLCouponsRow oCLCouponsRow = null;
            CLCoupons oCLCoupons = new CLCoupons();
            CLCouponsData oCLCouponsData = oCLCoupons.Populate(clCouponID);
            try
            {
                if (oCLCouponsData.CLCoupons.Rows.Count > 0)
                {
                    oCLCouponsRow = oCLCouponsData.CLCoupons[0];
                    if (oCLCardRow != null && oCLCouponsRow.CLCardID != oCLCardRow.CLCardID)
                    {
                        #region 10-Apr-2015 JY Added code to get customer name
                        try
                        {
                            // Added by Farman Ansari for Get CustomerName
                            GetCustomerName(ref oCLCouponsRow);
                            // End
                        }
                        catch
                        {
                            //If error occur then display original message
                            clsCoreUIHelper.ShowErrorMsg("Coupon does not belong to current customer.");
                        }
                        #endregion
                    }
                    else if (oCLCouponsRow.CreatedOn.AddDays(oCLCouponsRow.ExpiryDays) >= DateTime.Now.Date)
                    {
                        if (oCLCouponsRow.IsCouponUsed == false)
                        {
                            calculatedCouponValue = oCLCoupons.CalculateAndGetCouponValue(oCLCouponsRow, amount);
                            remainingclamount = maxClCouponAmount - ClCouponAmount;
                            if (remainingclamount > 0)
                            {

                                if (calculatedCouponValue > amount)
                                {
                                    calculatedCouponValue = amount;
                                }
                                if (calculatedCouponValue > remainingclamount)
                                {
                                    calculatedCouponValue = remainingclamount;
                                }

                                ClCouponAmount += calculatedCouponValue;
                            }

                            else
                            {
                                if (maxClCouponAmount > 0)
                                {
                                    clsCoreUIHelper.ShowErrorMsg(" Max Cl Coupon amount use for this transaction is: " + maxClCouponAmount);
                                }
                                else
                                {
                                    clsCoreUIHelper.ShowErrorMsg(" Cl Coupon is not allowed for this transaction is: " + maxClCouponAmount);
                                }
                            }
                            // oCLCouponsRow = null;
                            // this.grdPayment.ActiveRow.Cells[clsPOSDBConstants.POSTransPayment_Fld_RefNo].Value = string.Empty;
                        }
                        else
                        {
                            clsCoreUIHelper.ShowErrorMsg("Coupon already consumed.");
                        }
                    }
                    else
                    {
                        clsCoreUIHelper.ShowErrorMsg("Coupon already expired.");
                    }
                }
                else
                {
                    clsCoreUIHelper.ShowErrorMsg("Invalid coupon scanned.");
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ApplyCoupon()");
                throw exp;
            }
            return oCLCouponsRow;


        }

        public Boolean HouseInfoUserPreviliges()    //PRIMEPOS-2557 06-Jul-2018 JY changed return type    
        {
            #region Sprint-24 - PRIMEPOS-2290 24-Oct-2016 JY Added
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.AllowHouseChargePaytype.ID) == false)
            {
                //UserManagement.clsLogin oLogin = new POS_Core.UserManagement.clsLogin();
                string sUserID = string.Empty;
                if (clsCoreLogin.loginForPreviliges(clsPOSDBConstants.AllowHouseChargePaytype, "", out sUserID, "Security Override For Allow House Charge Payments") == false)
                {
                    return false;
                }
            }
            #endregion
            return true;
        }

        public void FetchHouseInfoCustomer(ref string sHousechargeAccountNo, ref string acctID, ref string strPatientName)
        {
            PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
            DataTable oDataTablePat = null;
            if (this.oRXHeaderList.Count == 0 && this.iCustomerID > 0)
            {
                Customer oCustomer = new Customer();
                CustomerData oCustomerData = oCustomer.PopulateList(" Where CustomerID=" + iCustomerID.ToString());
                if (oCustomerData.Customer.Rows.Count > 0)
                {
                    #region Sprint-24 - PRIMEPOS-2277 03-Nov-2016 JY Added    
                    if (Configuration.CPOSSet.UsePrimeRX && oCustomerData.Customer[0].PatientNo > 0)    //PRIMEPOS-3106 13-Jul-2022 JY modified
                    {
                        oPharmBL = new PharmData.PharmBL();
                        oDataTablePat = oPharmBL.GetPatient(oCustomerData.Customer[0].PatientNo.ToString());
                        if (oDataTablePat.Rows.Count > 0 && oDataTablePat.Rows[0]["acct_no"].ToString().Trim().Length > 0)
                        {
                            sHousechargeAccountNo = oDataTablePat.Rows[0]["acct_no"].ToString();
                        }
                    }
                    #endregion

                    if (oCustomerData.Customer[0].HouseChargeAcctID > 0)
                        acctID = oCustomerData.Customer[0].HouseChargeAcctID.ToString();
                }
            }
            else if (this.oRXHeaderList.Count > 0)
            {
                strPatientName = this.oRXHeaderList[0].PatientName;
                logger.Trace("SearchHouseCHargeInfo() - About to Call PHARMSQL");
                oDataTablePat = oPharmBL.GetPatient(this.oRXHeaderList[0].PatientNo);
                logger.Trace("SearchHouseCHargeInfo() - Successfully Call PHARMSQL");
                if (oDataTablePat.Rows.Count > 0 && oDataTablePat.Rows[0]["acct_no"].ToString().Trim().Length > 0)
                {
                    sHousechargeAccountNo = oDataTablePat.Rows[0]["acct_no"].ToString();
                }
            }
        }

        public string F10KeyImportCustomer(Customer oCustomer, CustomerData oCustData, CustomerRow oCustRow)
        {
            #region  Import Customer if not exist and if AutoImportCustAtTrans is true
            string returnCustomerName = string.Empty;
            oCustData = oCustomer.GetCustomerByPatientNo(Configuration.convertNullToInt(RXHeaderList[0].PatientNo));
            try
            {
                if ((oCustData == null || oCustData.Tables[0].Rows.Count == 0) && (Configuration.CInfo.AutoImportCustAtTrans == 1 || Configuration.CInfo.AutoImportCustAtTrans == 2))   //PRIMEPOS-2886 25-Sep-2020 JY Modified
                {
                    DataTable dtPatientPayPref = new DataTable();
                    PharmData.PharmBL oPhBl = new PharmData.PharmBL();
                    //ErrorLogging.Logs.Logger(this.Text, "frmPOSPayTypesList_KeyDown() - About to Call PHARMSQL", "");
                    logger.Trace("GetDeliveryAddress() - GetPatient() called");
                    dtPatientPayPref = oPhBl.GetPatient(RXHeaderList[0].PatientNo.ToString());
                    //ErrorLogging.Logs.Logger(this.Text, "frmPOSPayTypesList_KeyDown() - Successfully Call PHARMSQL", "");
                    logger.Trace("GetDeliveryAddress() - GetPatient() completed");
                    if (Configuration.isNullOrEmptyDataTable(dtPatientPayPref) == false)
                    {
                        DataSet oDs = new DataSet();
                        DataTable dtPatdata = dtPatientPayPref.Clone();
                        dtPatdata = dtPatientPayPref.Copy();
                        oDs.Tables.Add(dtPatdata);
                        CustomerData oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(oDs, true);
                        if (oCustomerData != null)
                        {
                            oCustData.Tables[0].ImportRow(oCustRow);
                            oCustomer.Persist(oCustomerData);
                            oCustData = oCustomer.GetCustomerByPatientNo(oCustRow.PatientNo);
                            if (oCustData != null && oCustData.Tables[0].Rows.Count > 0)
                            {
                                oCurrentCustRow = oCustData.Customer[0];
                                returnCustomerName = oCurrentCustRow.CustomerFullName;
                                //this.lblCustomerName.Text = oCurrentCustRow.CustomerFullName;
                            }
                        }
                    }
                }
                else if (oCustData != null && oCustData.Tables[0].Rows.Count > 0)
                {
                    oCurrentCustRow = oCustData.Customer[0];
                    returnCustomerName = oCurrentCustRow.CustomerFullName;
                    // this.lblCustomerName.Text = oCurrentCustRow.CustomerFullName;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "F10KeyImportCustomer()");
                if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 22-Jun-2022 JY Added
                    throw exp;
            }
            return returnCustomerName;
            #endregion
        }

        public void FetchAutoCLDiscount(CLCoupons oCLCoupons, CLCouponsData oCLCouponsData, ref decimal paidAmount)
        {
            try
            {
                decimal calculatedCouponValue = oCLCoupons.CalculateAndGetCouponValue(oCLCouponsData.CLCoupons[0], paidAmount);

                if (paidAmount > calculatedCouponValue)
                {
                    paidAmount = calculatedCouponValue;
                }

                if (Configuration.CLoyaltyInfo.ApplyDiscountOnlyIfTierIsReached == true && paidAmount < 1) return;  //Sprint-25 - PRIMEPOS-2297 28-Feb-2017 JY Added logic to restrict consumtion of fractional (< 0) coupon

                POSTransPaymentRow oRowPaid = (POSTransPaymentRow)oPOSTransPaymentDataPaid.POSTransPayment.NewPOSTransPaymentRow();

                oRowPaid.TransTypeCode = "C";
                oRowPaid.TransTypeDesc = "Coupon";

                oRowPaid.TransID = 0;
                oRowPaid.TransDate = DateTime.Now;
                //oRowPaid.RefNo = "@" + oCLCouponsData.CLCoupons[0].ID.ToString() + "@";
                oRowPaid.HC_Posted = false;
                oRowPaid.CustomerSign = string.Empty;
                oRowPaid.CCTransNo = string.Empty;
                oRowPaid.CCName = string.Empty;
                oRowPaid.AuthNo = string.Empty;
                oRowPaid.Amount = paidAmount;
                oRowPaid.IsIIASPayment = false;
                //Added By SRT(Gaurav) Date : 21-07-2009
                //Code added to initialize Payment Processor. Need to update as development goes onword.
                oRowPaid.PaymentProcessor = clsPOSDBConstants.NOPROCESSOR;
                //End Of Added By SRT(Gaurav)
                oRowPaid.IsManual = "";   //Sprint-19 - 2139 06-Jan-2015 JY Added
                oRowPaid.CLCouponID = oCLCouponsData.CLCoupons[0].ID;
                oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRowPaid);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "FetchAutoCLDiscount()");
                throw exp;
            }
        }

        public bool CheckUserPriviligesCashBack()   //PRIMEPOS-2741 25-Sep-2019 JY Added return type
        {
            string sUserID = Configuration.UserName;
            return (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.AllowCashback.ID, UserPriviliges.Permissions.AllowCashback.Name, out sUserID));
        }

        public POSTransPaymentData PopulatePayTypeData()
        {
            oPOSTransPaymentData = new POSTransPaymentData();
            PayType oPayType = new PayType();
            DataSet oPayTypeData;
            //oPayTypeData = oPayType.PopulateList("(IsHide IS NULL OR IsHide = 0)");   //PRIMEPOS-2675 17-Apr-2019 JY Commented
            oPayTypeData = oPayType.PopulateList(""); //PRIMEPOS-2675 17-Apr-2019 JY Added
            POSTransPaymentRow oPaymentRow;
            DataRow oDRow;
            try
            {
                for (int i = 0; i < oPayTypeData.Tables[0].Rows.Count; i++)
                {
                    oDRow = oPayTypeData.Tables[0].Rows[i];

                    if (isROA)
                    {
                        //if (oDRow[clsPOSDBConstants.PayType_Fld_PayTypeID].ToString().Trim() == "H" || oDRow[clsPOSDBConstants.PayType_Fld_PayTypeID].ToString().Trim() == "C") continue;   //PRIMEPOS-2763 27-Nov-2019 JY restrict "Coupon" for ROA
                        if (oDRow[clsPOSDBConstants.PayType_Fld_PayTypeID].ToString().Trim() == "H") continue;  //PRIMEPOS-2434 04-May-2021 JY Added
                    }

                    oPaymentRow = oPOSTransPaymentData.POSTransPayment.AddRow(i, oDRow[clsPOSDBConstants.PayType_Fld_PayTypeID].ToString(), oDRow[clsPOSDBConstants.PayType_Fld_PayTypeDescription].ToString(), "", 0, false, System.DateTime.Now, 0, "", "", "", "", clsPOSDBConstants.NOPROCESSOR, null,//2943
                                  "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", false, "", "", System.DateTime.Now, false,0); //Sprint-19 - 2139 06-Jan-2015 JY Added IsManual parameter value // PRIMEPOS-2664 - Evertec//PRIMEPOS-2636 ADDED PARAMETERS/PRIMEPOS-2793 ADDED FOR VANTIV//2990    //PRIMEPOS-3117 11-Jul-2022 JY Added TransFeeAmt parameter
                    //PRIMEPOS-2664 AND PRIMEPOS-2786 ADDED PARAMTERS
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "RemoveEBTTaxAndGetTotal()");
                throw exp;
            }
            return oPOSTransPaymentData;
        }

        public void CashBackForOverPay()
        {
            try
            {
                if ((amountPaid - (totalAmount + TransFeeAmt + amountCashBack)) > 0)    //PRIMEPOS-3117 11-Jul-2022 JY modified
                {
                    #region PRIMEPOS-3023 23-Nov-2021 JY Added
                    POSTransPaymentRow cashRow = null;
                    foreach (POSTransPaymentRow oCRow in oPOSTransPaymentData.POSTransPayment.Rows)
                    {
                        if (oCRow.TransTypeCode.Trim() == "1")
                        {
                            cashRow = oCRow;
                            break;
                        }
                    }
                    #endregion

                    bool isCashPaymentExist = false;
                    foreach (POSTransPaymentRow oCRow in oPOSTransPaymentDataPaid.POSTransPayment.Rows)
                    {
                        //if (oCRow.TransTypeCode == oPOSTransPaymentData.POSTransPayment[0].TransTypeCode) //PRIMEPOS-3023 23-Nov-2021 JY Commented
                        if (oCRow.TransTypeCode.Trim() == cashRow.TransTypeCode.Trim()) //PRIMEPOS-3023 23-Nov-2021 JY Added
                        {
                            //Fix by Manoj for split cash payment. 7/9/2015
                            oCRow.Amount = oCRow.Amount - (amountPaid - (totalAmount + TransFeeAmt + amountCashBack));  //PRIMEPOS-3117 11-Jul-2022 JY modified
                            ChangeDue = amountPaid - (totalAmount + TransFeeAmt + amountCashBack);  //PRIMEPOS-3117 11-Jul-2022 JY modified
                            isCashPaymentExist = true;
                            break;
                        }
                    }

                    if (isCashPaymentExist == false)
                    {
                        int TransPayID = 1;
                        foreach (POSTransPaymentRow oCRow in oPOSTransPaymentDataPaid.POSTransPayment.Rows)
                        {
                            if (oCRow.TransPayID >= TransPayID)
                            {
                                TransPayID = oCRow.TransPayID + 1;
                            }
                        }
                        POSTransPaymentRow oRowPaid = (POSTransPaymentRow)oPOSTransPaymentDataPaid.Tables[0].NewRow();
                        oRowPaid.TransTypeCode = cashRow.TransTypeCode; //oPOSTransPaymentData.POSTransPayment[0].TransTypeCode;
                        oRowPaid.TransTypeDesc = cashRow.TransTypeDesc; //oPOSTransPaymentData.POSTransPayment[0].TransTypeDesc;
                        oRowPaid.TransPayID = TransPayID;
                        oRowPaid.TransID = cashRow.TransID; //oPOSTransPaymentData.POSTransPayment[0].TransID;
                        oRowPaid.TransDate = cashRow.TransDate; //oPOSTransPaymentData.POSTransPayment[0].TransDate;
                        oRowPaid.RefNo = "Cashback for overpay";
                        oRowPaid.HC_Posted = cashRow.HC_Posted; //oPOSTransPaymentData.POSTransPayment[0].HC_Posted;
                        oRowPaid.CustomerSign = "";
                        oRowPaid.BinarySign = null;
                        oRowPaid.SigType = "";
                        oRowPaid.CCTransNo = "";
                        oRowPaid.CCName = "";
                        oRowPaid.AuthNo = "";
                        oRowPaid.Amount = (totalAmount + TransFeeAmt  + amountCashBack) - amountPaid;   //PRIMEPOS-3117 11-Jul-2022 JY modified
                        oRowPaid.IsIIASPayment = false;
                        oRowPaid.ProcessorTransID = cashRow.ProcessorTransID;   //oPOSTransPaymentData.POSTransPayment[0].ProcessorTransID;
                        oPOSTransPaymentDataPaid.POSTransPayment.AddRow(oRowPaid);

                        ChangeDue = amountPaid - (totalAmount + TransFeeAmt + amountCashBack);  //PRIMEPOS-3117 11-Jul-2022 JY modified
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "RemoveEBTTaxAndGetTotal()");
                throw exp;
            }
        }

        #region PRIMEPOS-2539 09-Jul-2018 JY Added
        public Boolean AllowCheckPaymentPriviliges()
        {
            string sUserID = string.Empty;
            bool bStatus = UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.AllowCheckPayment.ID, UserPriviliges.Permissions.AllowCheckPayment.Name, out sUserID);
            return bStatus;
        }
        #endregion

        #endregion Methods

    }
    public class F10TransType
    {
        public const string ROA = "ROA";
        public const string RXTrans = "RX";
        public const string NonRXTrans = "NONRX";
    }
}
