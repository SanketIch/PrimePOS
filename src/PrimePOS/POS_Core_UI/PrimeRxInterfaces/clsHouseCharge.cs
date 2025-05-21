using System;
using System.Data;
using MMSBClass;
using MMSChargeAccount;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.Resources;
using POS_Core.CommonData.Rows;
using POS_Core.Resources.PaymentHandler;
using POS_Core.TransType;
using POS_Core;
using POS_Core.Data_Tier;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for clsHouseCharge.
	/// </summary>
	public class clsHouseCharge
	{
		private clsHouseCharge()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet getHouseChargeInfo(string searchCode,string SearchName)
		{
			DataSet oDS=null;
            if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
            {
                ContAccount oSearch = new ContAccount();
                if (searchCode.Trim() != "")
                {
                    oSearch.GetAccountByCode(searchCode, out oDS);
                }
                else
                {
                    oSearch.GetAccountByName(SearchName, out oDS);
                }
            }
			return oDS;
		}

        public static bool getHCConfirmation(string strCode, out string strName, Decimal amount, out string strSignature, out string strSigType, out string OverrideHousechargeLimitUser)
		{
			strName="";
            strSignature="";
            strSigType = "";
            bool capturesig = false; // Added by Manoj 11/22/2011
            OverrideHousechargeLimitUser = string.Empty;    //PRIMEPOS-2402 09-Jul-2021 JY Added

            if (strCode.Trim()=="")
			{
				return false;
			}
			else
			{
				frmHouseChargeConfirmation oHCInfo=new frmHouseChargeConfirmation(strCode.Trim(),amount);
                if (oHCInfo.ShowDialog() == DialogResult.OK)
                {
                    OverrideHousechargeLimitUser = oHCInfo.OverrideHousechargeLimitUser;
                    //if (CaptureSignarure(amount, out strSignature) == true)
                    if (Configuration.CPOSSet.DispSigOnHouseCharge)// Added by Manoj 11/22/2011
                    {
                        if (CaptureSignarure(amount, out strSignature, out strSigType /*Added RiteshMx*/) == true)
                        {
                            strName = oHCInfo.lblName.Text.ToString();
                            //return true;
                            capturesig = true;
                        }
                        else
                        {
                            // return false;
                            capturesig = false;
                        }
                        return capturesig;
                    }
                    else
                    {
                        strName = oHCInfo.lblName.Text.ToString();
                        return true;
                    }
                }
				else
				{
					return false;
				}
			}
		}

        private static bool CaptureSignarure(decimal Amount, out string strSignature, out string strSigType /*RiteshMx*/)
        {
            bool retVal = true;
            strSignature = string.Empty;
            strSigType = string.Empty;
            if (Configuration.CPOSSet.UseSigPad == true)
            {
                string strMessage = "Amount charged = " + Configuration.CInfo.CurrencySymbol + Amount.ToString("##,###,##0.00");
                if (SigPadUtil.DefaultInstance.CaptureSignature(strMessage) == true)
                {
                    strSignature = SigPadUtil.DefaultInstance.CustomerSignature;
                    strSigType = SigPadUtil.DefaultInstance.SigType;//RIteshMx
                    if (Configuration.CPOSSet.DispSigOnTrans == true)
                    {
                        //Modified By Dharmendra SRT on Jan-31-09 again checking whether sigpad is true 
                        if (Configuration.CPOSSet.UseSigPad == true)
                        {
                            SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                        }
                        //Modified Till Here Jan-31-09
                        // Mantis Id : 0000119 Modified By Dharmendra (SRT) on Dec-02-08 passed an extra parameter true
                        frmViewSignature ofrm = new frmViewSignature(strSignature, strSigType, true); //RIteshMx
                        ofrm.SetMsgDetails("Validating Signature...");
                        ofrm.ShowDialog();
                        //Modified By Dharmendra (SRT) on Dec-15-08  to reinitialize the signature variables
                        if (ofrm.IsSignatureRejected == true)
                        {
                            ofrm = null;
                            strSignature = "";
                            SigPadUtil.DefaultInstance.CustomerSignature = null;
                            retVal = false;
                            CaptureSignarure(Amount, out strSignature, out strSigType);
                        }
                        else
                        {
                            retVal = true;
                        }
                        //End Modified
                        // Mantis Id : 0000119  Modified till here
                    }
                }
                else
                {
                    DialogResult dgRes = MessageBox.Show("Unable to obtain Customer Signature\nEither Signature Pad is Not Connected Properly or Customer Refused to Sign\nDo You wish to Proceed with this Transaction?", "Problem Obtaining Signature", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                    if (dgRes == DialogResult.Yes)
                        retVal = true;
                    else
                        retVal = false;
                }
                /*else
                {
                    CancelTransaction();
                    retVal = false;
                }*/
            }
            return retVal;
        }

		public static bool GetReceiveOnAccount(string strCode,out string strName,out System.Decimal Amount, out DataTable dtChargeAccount, Decimal AmountToReturn, int CustomerID, ref int nPatientNo)
		{
			strName="";
			Amount=0;
            dtChargeAccount = null;
			if (strCode.Trim()=="")
			{
				return false;
			}
			else
			{
                frmHouseChargePayment oHCInfo = new frmHouseChargePayment(strCode.Trim(), AmountToReturn, CustomerID);
				if (oHCInfo.ShowDialog()==DialogResult.OK)
				{
					strName=oHCInfo.lblName.Text.ToString();
					Amount=Convert.ToDecimal( oHCInfo.txtAmount.Value);
                    dtChargeAccount = oHCInfo.ChargeAccountRecord;
                    nPatientNo = oHCInfo.PatientNo; //PRIMEPOS-2570 02-Jul-2020 JY Added
                    return true;
				}
				else
				{
					return false;
				}
			}
		}

        public static void postHouseCharge(TransDetailData oTransDData, POSTransPaymentData oTransPData, POSTransactionType TransType, System.Decimal InvDiscPerc, POS_Core.CommonData.Rows.POSTransPaymentRow oHRow, System.Int32 lTransID)
        {
            #region HC
            //System.Collections.ArrayList oList = new System.Collections.ArrayList();
            //string accountNo = oHRow[clsPOSDBConstants.POSTransPayment_Fld_RefNo].ToString();
            //accountNo = accountNo.Substring(0, accountNo.IndexOf("\\"));
            //AcctTran oAccTran = new AcctTran();
            //string TType = string.Empty;
            //if (TransType == POSTransactionType.Sales)
            //{
            //    TType = "C";
            //}
            //else
            //{
            //    TType = "R";
            //}
            //if (oTransPData.POSTransPayment.Rows.Count > 0)
            //{
            //    foreach (POS_Core.CommonData.Rows.POSTransPaymentRow oRow in oTransPData.POSTransPayment.Rows)
            //    {
            //        if(oRow.TransTypeCode.Trim() == "H")
            //        {
            //            oAccTran = new AcctTran();
            //            oAccTran.ACCT_NO = long.Parse(accountNo);
            //            oAccTran.TRAN_DATE = System.DateTime.Now.ToString();
            //            oAccTran.TRANS_TYPE = TType;
            //            oAccTran.REFERENCE = oRow.TransTypeDesc + "\\" + oRow.RefNo;
            //            oAccTran.TRAN_AMT = oRow.Amount;
            //            oAccTran.POSStoreID = Configuration.CInfo.StoreID;
            //            oAccTran.POSTransID = lTransID;
            //            oList.Add(oAccTran);
            //        }
            //    }
            //    ContAcctTran oCAT = new ContAcctTran();
            //    AcctTran[] oTrans = (AcctTran[])oList.ToArray(typeof(AcctTran));
            //    oCAT.SaveAcctTrans(oTrans);
            //}
            #endregion
            System.Collections.ArrayList oList=new System.Collections.ArrayList();
            //AcctTran [] oTrans=new AcctTran[oTransDData.TransDetail.Rows.Count];
            AcctTran oAccTran;

            string accountNo=oHRow[clsPOSDBConstants.POSTransPayment_Fld_RefNo].ToString();
            accountNo = accountNo.Substring(0, accountNo.IndexOf("\\"));
            POS_Core.CommonData.Rows.TransDetailRow oDRow;
            System.Decimal taxAmount=0;
            string TType;
            if(TransType == POSTransactionType.Sales)
            {
                TType = "C";
            }
            else
            {
                TType = "R";
            }

            for(int i=0; i < oTransDData.TransDetail.Rows.Count; i++)
            {
                oDRow = (POS_Core.CommonData.Rows.TransDetailRow) oTransDData.TransDetail.Rows[i];
                taxAmount += oDRow.TaxAmount;
                oAccTran = new AcctTran();
                oAccTran.ACCT_NO = long.Parse(accountNo);
                oAccTran.TRANS_TYPE = TType;
                oAccTran.TRAN_DATE = System.DateTime.Now.ToString();
                if(Configuration.CInfo.PostDescOnlyInHC == true)
                {
                    oAccTran.REFERENCE = oDRow.ItemDescription;
                }
                else
                {
                    if(Configuration.CInfo.PostRXNumberOnlyInHC == true)
                    {
                        string sRXNo = oDRow.ItemDescription;
                        if(oDRow.ItemDescription.IndexOf("-", 0) > 0)
                        {
                            sRXNo = oDRow.ItemDescription.Substring(0, oDRow.ItemDescription.IndexOf("-", 0));
                        }
                        oAccTran.REFERENCE = oDRow.ItemID + "\\" + sRXNo;
                    }
                    else
                    {
                        oAccTran.REFERENCE = oDRow.ItemID + "\\" + oDRow.ItemDescription;
                    }
                }

                //if (POS_Core.Resources.Configuration.CPOSSet.AllItemDisc==true)//Commented By Shitaljit on 7 September
                if(POS_Core.Resources.Configuration.CPOSSet.AllItemDisc == "1")//Added By Shitaljit on 7 September   //Apply invoice discount to all the items (irrespective of the current individual item disc) (Default settings) 
                    oAccTran.TRAN_AMT = Convert.ToDecimal(oDRow.ExtendedPrice - oDRow.Discount - (InvDiscPerc / 100 * (oDRow.ExtendedPrice - oDRow.Discount)));
                //else if (POS_Core.Resources.Configuration.CPOSSet.AllItemDisc=="false" && oDRow.Discount==0)//Commented By Shitaljit on 7 September
                else if(POS_Core.Resources.Configuration.CPOSSet.AllItemDisc == "0" && oDRow.Discount == 0)//Added By Shitaljit on 7 September   //Apply invoice discount to only items which are not individually discounted 
                {
                    oAccTran.TRAN_AMT = Convert.ToDecimal(oDRow.ExtendedPrice - (InvDiscPerc / 100 * (oDRow.ExtendedPrice)));
                }
                //Added By Shitaljit on 7 September
                else if(POS_Core.Resources.Configuration.CPOSSet.AllItemDisc == "2")//Added By Shitaljit on 7 September  //Remove individual item disc and apply the invoice disc to the transaction net amount 
                {
                    //oDRow.Discount = 0;
                    oAccTran.TRAN_AMT = Convert.ToDecimal(oDRow.ExtendedPrice - (InvDiscPerc / 100 * (oDRow.ExtendedPrice)));
                }
                else if(POS_Core.Resources.Configuration.CPOSSet.AllItemDisc == "3")//Added By Shitaljit on 7 September   //Do not allow invoice disc when individual item disc are present in the transaction 
                {
                    //PRIMEPOS-2505 23-Apr-2018 JY Added
                    if (oDRow.Discount != 0)    
                        oAccTran.TRAN_AMT = Convert.ToDecimal(oDRow.ExtendedPrice - oDRow.Discount);                    
                    else
                        oAccTran.TRAN_AMT = Convert.ToDecimal(oDRow.ExtendedPrice - (InvDiscPerc / 100 * (oDRow.ExtendedPrice)));
                }
                //END of added by shitaljit on 7 September
                else
                {
                    oAccTran.TRAN_AMT = Convert.ToDecimal(oDRow.ExtendedPrice - oDRow.Discount);
                }

                if(oAccTran.TRAN_AMT < 0 && TType == "C")
                {
                    oAccTran.TRANS_TYPE = "R";
                }
                else if(oAccTran.TRAN_AMT > 0 && TType == "R")
                {
                    oAccTran.TRANS_TYPE = "C";
                }
                oAccTran.TRAN_AMT = Math.Round(Math.Abs(oAccTran.TRAN_AMT), 2);
                oAccTran.POSStoreID = Configuration.CInfo.StoreID;
                oAccTran.POSTransID = lTransID;
                
                #region Sprint-21 - PRIMEPOS-2261 12-Feb-2016 JY Added logic to commit RxNo and nRefillNo info in ACCTTRAN table
                try
                {
                    if (oDRow.ItemID.Trim().ToUpper() == "RX")
                    {
                        String[] splitArray = oDRow.ItemDescription.Split('-');

                        if (splitArray.Length > 0)
                        {
                            oAccTran.RXNO = Configuration.convertNullToInt64(splitArray[0].Trim());
                            if (oAccTran.RXNO == 0) oAccTran.RXNO = -1;
                        }
                        if (splitArray.Length > 1)
                        {
                            try
                            {
                                oAccTran.REFILL_NO = Int32.Parse(splitArray[1].Trim());
                            }
                            catch { oAccTran.REFILL_NO = -1; }
                        }
                    }
                }
                catch{ }
                #endregion

                oList.Add(oAccTran);
            }

            if(taxAmount != 0)
            {
                oAccTran = new AcctTran();
                oAccTran.ACCT_NO = long.Parse(accountNo);
                oAccTran.TRAN_DATE = System.DateTime.Now.ToString();
                oAccTran.TRANS_TYPE = TType;
                if(Convert.ToDecimal(taxAmount) < 0 && TType == "C")
                {
                    oAccTran.TRANS_TYPE = "R";
                }
                else if(Convert.ToDecimal(taxAmount) > 0 && TType == "R")
                {
                    oAccTran.TRANS_TYPE = "C";
                }

                //oAccTran.TRANS_TYPE=TType;
                oAccTran.REFERENCE = "Tax";
                oAccTran.TRAN_AMT = Math.Round(Math.Abs(Convert.ToDecimal(taxAmount)), 2);
                oAccTran.POSStoreID = Configuration.CInfo.StoreID;
                oAccTran.POSTransID = lTransID;
                oList.Add(oAccTran);
            }

            if(oTransPData.POSTransPayment.Rows.Count > 1)
            {
                foreach(POS_Core.CommonData.Rows.POSTransPaymentRow oRow in oTransPData.POSTransPayment.Rows)
                {
                    if(oRow.TransTypeCode.Trim() != "H")
                    {
                        oAccTran = new AcctTran();
                        oAccTran.ACCT_NO = long.Parse(accountNo);
                        oAccTran.TRAN_DATE = System.DateTime.Now.ToString();
                        oAccTran.TRANS_TYPE = "P";
                        oAccTran.REFERENCE = oRow.TransTypeCode + "\\" + oRow.TransTypeDesc + "\\" + oRow.RefNo;
                        oAccTran.TRAN_AMT = oRow.Amount;
                        oAccTran.POSStoreID = Configuration.CInfo.StoreID;
                        oAccTran.POSTransID = lTransID;
                        oList.Add(oAccTran);
                    }
                }
            }
            ContAcctTran oCAT=new ContAcctTran();
            AcctTran [] oTrans=(AcctTran[]) oList.ToArray(typeof(AcctTran));
            oCAT.SaveAcctTrans(oTrans);
        }

        public static void AdjustOverPayment(ref POSTransPaymentData oTransPData)
        {
            try
            {
                POS_Core.CommonData.Rows.POSTransPaymentRow oDRow = null;
                DataRow[] dr = oTransPData.POSTransPayment.Select(clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " = '1' AND " + clsPOSDBConstants.POSTransPayment_Fld_Amount + " < 0 ");
                if (dr.Length > 0)
                {
                    oDRow = (POS_Core.CommonData.Rows.POSTransPaymentRow)dr[0];
                    decimal overPaidAmount = Math.Abs(oDRow.Amount);
                    oTransPData.POSTransPayment.Rows.Remove(oDRow);
                    oTransPData.POSTransPayment.AcceptChanges();
                    foreach (POSTransPaymentRow oRow in oTransPData.POSTransPayment.Rows)
                    {
                        if (oRow.TransTypeCode.Trim() == "2" && oRow.Amount > overPaidAmount)
                        {
                            oRow.Amount = oRow.Amount - overPaidAmount;
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        public static void postROA(System.Int64 AccountNo, POSTransPaymentData oTransPData, POSTransactionType TransType)
		{
           // AdjustOverPayment(ref oTransPData); 
			AcctTran [] oTrans=new AcctTran[oTransPData.POSTransPayment.Rows.Count];
			
			POS_Core.CommonData.Rows.POSTransPaymentRow oDRow;
			string TType;
            string sRefference = string.Empty;
           
			for(int i=0;i<oTransPData.POSTransPayment.Rows.Count;i++)
			{
				oDRow=(POS_Core.CommonData.Rows.POSTransPaymentRow)oTransPData.POSTransPayment.Rows[i];
                if (TransType == POSTransactionType.Sales)
                {
                    TType = "P";
                    sRefference = oDRow.TransTypeDesc.ToString();
                }
                else
                {
                    TType = "C";
                    sRefference = oDRow.TransTypeDesc.ToString() + "\\Returned ROA";
                }
				oTrans[i]=new AcctTran();
				oTrans[i].ACCT_NO=long.Parse( AccountNo.ToString());
				oTrans[i].TRANS_TYPE=TType;

				oTrans[i].TRAN_DATE=System.DateTime.Now.ToString();
                if (string.IsNullOrEmpty(frmHouseChargePayment.ROARefference) == false)
                {
                    oTrans[i].REFERENCE = sRefference + "\\" + frmHouseChargePayment.ROARefference;
                }
                else 
                {
                    oTrans[i].REFERENCE = sRefference ;
                }
				oTrans[i].TRAN_AMT=Math.Round( Math.Abs( Convert.ToDecimal(oDRow.Amount)),2);
                oTrans[i].POSTransID = long.Parse(oDRow.TransID.ToString()); //added by atul 11-feb-2011
			}
			ContAcctTran oCAT=new ContAcctTran();
			oCAT.SaveAcctTrans(oTrans);
		}

        public static HouseChargeAccount GetAccountInformation(string AccountCode)
        {
            HouseChargeAccount oHouseChargeAccount = new HouseChargeAccount();

            if (AccountCode.Trim() != "")
            {
                ContAccount oAcct = new ContAccount();
                DataSet oDS = new DataSet();
                //oAcct.GetAccountByCode(AccountCode, out oDS);
                oAcct.GetAccountByCode(AccountCode, out oDS, true); //PRIMEPOS-2888 28-Aug-2020 JY Added third parameter as "true" to get the exact HouseCharge record
                if (oDS != null)
                {
                    if (oDS.Tables[0].Rows.Count != 0)
                    {
                        oHouseChargeAccount.AccountCode = AccountCode;
                        oHouseChargeAccount.AccountName = oDS.Tables[0].Rows[0]["acct_Name"].ToString();
                        oHouseChargeAccount.AccountAddress1 = oDS.Tables[0].Rows[0]["ADDRESS1"].ToString();
                        oHouseChargeAccount.AccountAddress2 = oDS.Tables[0].Rows[0]["ADDRESS2"].ToString();
                        oHouseChargeAccount.City = oDS.Tables[0].Rows[0]["city"].ToString();
                        oHouseChargeAccount.State = oDS.Tables[0].Rows[0]["state"].ToString();
                        oHouseChargeAccount.ZipCode = oDS.Tables[0].Rows[0]["zip"].ToString();
                        oHouseChargeAccount.PhoneNo = oDS.Tables[0].Rows[0]["PHONE_NO"].ToString();
                        oHouseChargeAccount.Comment = oDS.Tables[0].Rows[0]["COMMENT"].ToString();
                        oHouseChargeAccount.MobileNo = oDS.Tables[0].Rows[0]["MOBILENO"].ToString();

                        oHouseChargeAccount.CurrentBalance = oAcct.GetAccountBalance(Convert.ToInt32(AccountCode));
                    }
                }
            }

            return oHouseChargeAccount;
        }

        //Added By Rohit Nair on 01/17/2017 for PRIMEPOS-2368
        /// <summary>
        /// Gets the Prime Rx Patient Information For the patient the House charge Account Belongs to.
        /// </summary>
        /// <param name="Acctno"></param>
        /// <returns></returns>
        public static DataSet GetPatientByChargeAccountNumber(long Acctno)
        {
            DataSet oDsPatient = null;

            ContAccount oAcct = new ContAccount();
            long rec = oAcct.GetPatientByAccntNum(Acctno,out oDsPatient);

            return oDsPatient;
        }

	}

    //public class HouseChargeAccount
    //{
    //    private string sAccountCode;
    //    private string sAccountName;
    //    private string sAccountAddr1;
    //    private string sAccountAddr2;
    //    private string sCity;
    //    private string sState;
    //    private string sZipCode;
    //    private string sPhoneNo;
    //    private string sMobileNo;
    //    private string sComment;
    //    private decimal dCurrentBalance = 0;


    //    public string AccountCode
    //    {
    //        get { return sAccountCode; }
    //        set { sAccountCode = value; }
    //    }

    //    public string AccountName
    //    {
    //        get { return sAccountName; }
    //        set { sAccountName = value; }
    //    }

    //    public string AccountAddress1
    //    {
    //        get { return sAccountAddr1; }
    //        set { sAccountAddr1 = value; }
    //    }

    //    public string AccountAddress2
    //    {
    //        get { return sAccountAddr2; }
    //        set { sAccountAddr2 = value; }
    //    }

    //    public string City
    //    {
    //        get { return sCity; }
    //        set { sCity = value; }
    //    }

    //    public string State
    //    {
    //        get { return sState; }
    //        set { sState = value; }
    //    }

    //    public string ZipCode
    //    {
    //        get { return sZipCode; }
    //        set { sZipCode = value; }
    //    }

    //    public string PhoneNo
    //    {
    //        get { return sPhoneNo; }
    //        set { sPhoneNo = value; }
    //    }

    //    public string MobileNo
    //    {
    //        get { return sMobileNo; }
    //        set { sMobileNo = value; }
    //    }

    //    public string Comment
    //    {
    //        get { return sComment; }
    //        set { sComment = value; }
    //    }

    //    public decimal CurrentBalance
    //    {
    //        get { return dCurrentBalance; }
    //        set { dCurrentBalance = value; }
    //    }



    //}
}

