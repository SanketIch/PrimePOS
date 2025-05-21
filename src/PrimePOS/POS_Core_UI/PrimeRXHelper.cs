using System;
using System.Collections.Generic;
using System.Text;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using System.Data;
//using POS_Core.DataAccess;
using System.Windows.Forms;
using POS_Core.CommonData.Rows;
using MMSChargeAccount;
using PharmData;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public class PrimeRXHelper
    {
        public static bool ImportPatientAsCustomer(int patientNo)
        {
            Customer oCustomer=new Customer();
            MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
            DataSet dsPatient = new DataSet();
            
            oAcct.GetPatientByCode(Configuration.convertNullToInt(patientNo), out dsPatient);
            if (dsPatient.Tables[0].Rows.Count > 0)
            {
                CustomerData oCustomerData;
                oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(dsPatient, false);
                oCustomer.Persist(oCustomerData, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetPatientDeliveryAddress(string patienNos)
        {
            string strReturn = string.Empty;    //PRIMEPOS-3106 22-Jun-2022 JY Added
            DataSet dsAddress = new DataSet();
            DataSet dsPatient = new DataSet();
            DataSet dsChargeAcct = new DataSet();
            DataTable dtFacility = new DataTable();
            MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
            PharmBL opharmBl = new PharmBL();                            //Added By Rohit Nair For PRIMEPOS-2372
            string sAddress = string.Empty;
            string sName = string.Empty;
            try //PRIMEPOS-3106 22-Jun-2022 JY Added try catch block
            {
                // dsAddress = oAcct.GetPatientsDeliveryAddr(patienNos);    //commented out By Rohit Nair For PRIMEPOS-2372
                dsAddress = opharmBl.GetPatientsDeliveryAddr(patienNos);    //Added By Rohit Nair For PRIMEPOS-2372
                if (Configuration.isNullOrEmptyDataSet(dsAddress) == true)
                {
                    strReturn = string.Empty;
                }
                dsAddress.Tables[0].Columns.Add("Source", typeof(string));
                dsAddress.Tables[0].Rows[0]["Source"] = "P";
                for (int Index = 1; Index < dsAddress.Tables[0].Rows.Count; Index++)
                {
                    dsAddress.Tables[0].Rows[Index]["Source"] = "D";
                }
                #region To get more Address
                oAcct.GetPatientByCode(Configuration.convertNullToInt(patienNos), out dsPatient);
                if (Configuration.isNullOrEmptyDataSet(dsPatient) == false)
                {
                    sName = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["LNAME"]).Trim() + " " +
                                   Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["FNAME"]).Trim();
                    if (dsPatient.Tables[0].Rows[0]["ACCT_NO"].ToString().Trim() != string.Empty)   //PRIMEPOS-2205 02-Aug-2016 JY Added logic to resolve the issue related to returning multiple addresses   
                    {
                        oAcct.GetAccountByCode(dsPatient.Tables[0].Rows[0]["ACCT_NO"].ToString(), out dsChargeAcct, true);   //Sprint-21 - 2205 01-Jul-2015 JY Added third parameter as "true" to get the exact HouseCharge record (single Record)
                        if (Configuration.isNullOrEmptyDataSet(dsChargeAcct) == false)
                        {
                            foreach (DataRow oRow in dsChargeAcct.Tables[0].Rows)
                            {
                                sAddress = Configuration.convertNullToString(oRow["ADDRESS1"]).Trim() + " " +
                                          Configuration.convertNullToString(oRow["ADDRESS2"]).Trim() + " " +
                                          Configuration.convertNullToString(oRow["CITY"]).Trim() + " " +
                                          Configuration.convertNullToString(oRow["STATE"]).Trim() + " " +
                                          Configuration.convertNullToString(oRow["ZIP"]).Trim();
                                //following if is added by Shitaljit to check null adress
                                if (string.IsNullOrEmpty(sAddress) == false)
                                {
                                    dsAddress.Tables[0].NewRow();
                                    dsAddress.Tables[0].Rows.Add(sName, sAddress, "H");
                                }
                            }
                        }
                    }
                }
                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                //ADDRESS1	ADDRESS2	CITY	STATE	ZIP
                POS_Core.ErrorLogging.Logs.Logger("PrimeRxHelper", "GetPatientDeliveryAddress() - About to Call PHARMSQL", "");
                dtFacility = oPharmBL.GetFacility(dsPatient.Tables[0].Rows[0]["FacilityCD"].ToString());
                POS_Core.ErrorLogging.Logs.Logger("PrimeRxHelper", "GetPatientDeliveryAddress() - Successfully Call PHARMSQL", "");
                if (Configuration.isNullOrEmptyDataTable(dtFacility) == false)
                {
                    foreach (DataRow oRow in dtFacility.Rows)
                    {

                        sAddress = Configuration.convertNullToString(oRow["ADDRESS1"]).Trim() + " " +
                                   Configuration.convertNullToString(oRow["ADDRESS2"]).Trim() + " " +
                                   Configuration.convertNullToString(oRow["CITY"]).Trim() + " " +
                                   Configuration.convertNullToString(oRow["STATE"]).Trim() + " " +
                                   Configuration.convertNullToString(oRow["ZIP"]).Trim();
                        //following if is added by Shitaljit to check null adress
                        if (string.IsNullOrEmpty(sAddress) == false)
                        {
                            dsAddress.Tables[0].NewRow();
                            dsAddress.Tables[0].Rows.Add(sName, sAddress, "F");
                        }
                    }
                }
                Customer ocust = new Customer();
                CustomerData oCustData = ocust.GetCustomerByPatientNo(Configuration.convertNullToInt(patienNos));
                if (Configuration.isNullOrEmptyDataSet(oCustData) == false)
                {
                    POS_Core.CommonData.Rows.CustomerRow ocustRow = (POS_Core.CommonData.Rows.CustomerRow)oCustData.Customer.Rows[0];
                    sAddress = ocustRow.GetAddress();
                    dsAddress.Tables[0].NewRow();
                    dsAddress.Tables[0].Rows.Add(sName, sAddress, "C");
                }
                #endregion
                if (dsAddress.Tables[0].Rows.Count == 0)
                {
                    strReturn = string.Empty;
                }
                else
                {
                    frmDeliveryAddress ofrm = new frmDeliveryAddress(dsAddress);
                    if (ofrm.ShowDialog() != DialogResult.Cancel)
                    {
                        strReturn = ofrm.GetAddress();
                    }
                }
            }
            catch { }
            return strReturn;
        }

        /// <summary>
        /// Author: Shitaljit 
        /// Added to get RXs which is marked for delivery 
        /// </summary>
        /// <param name="dsRXDetails"></param>
        /// <returns></returns>
        public static string GetListOfRxMarkedForDelivery(TransDetailRXData dsRXDetails)
        {
            //DataTable dtRxInfo = new DataTable();      - no need to get Delivery flag from PrimeRx db, it is available
            //PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
            string sStatus = string.Empty;
            string[] arr = { "", ""};
            string sRXList = string.Empty;
            ListViewItem oItem;
            frmRxMarkedForDelivery oRXforDelivery = new frmRxMarkedForDelivery();
            try
            {
                foreach (TransDetailRXRow oRow in dsRXDetails.TransDetailRX.Rows)
                {
                    string sRXNo = Configuration.convertNullToString(oRow.RXNo);
                    string sRefillNo = Configuration.convertNullToString(oRow.NRefill);
                    string sPartialFillNo = Configuration.convertNullToString(oRow.PartialFillNo);
                    if (sRXList.Contains(sRXNo) == true)
                    {
                        continue;
                    }
					/*Date 27/01/2014
			 * Modified By Shitaljit
			 * for dynamic currency 
			 */
					// old code
                    //sRXList += sRXNo + "$";
					// new code
					sRXList += sRXNo + Configuration.CInfo.CurrencySymbol.ToString();

                    // POS_Core.ErrorLogging.Logs.Logger("PrimeRxHelper", "GetListOfRxMarkedForDelivery() - About to Call PHARMSQL", "");
                    // dtRxInfo = oPharmBL.GetRxForDelivery(sRXNo, out  sStatus, sRefillNo, sPartialFillNo);
                    // POS_Core.ErrorLogging.Logs.Logger("PrimeRxHelper", "GetListOfRxMarkedForDelivery() - Successfully Call PHARMSQL", "");
                    if ( oRow.Delivery.Equals("D") == true)
                    {
                        arr[0] = sRXNo;
                        arr[1] = sRefillNo;
                        oItem = new ListViewItem(arr);
                        oRXforDelivery.lvItemList.Items.Add(oItem);
                    }
                }
                if (oRXforDelivery.lvItemList.Items.Count > 0)
                {
                    oRXforDelivery.ShowDialog();
                }

            }
            catch (Exception Ex)
            {
                return string.Empty;
                throw Ex;
            }
            return oRXforDelivery.SelectedAction;
        }

        public static string VarifyChargeAlreadyPosted(TransDetailRXData oTransRXDData, TransDetailData oTransDData, ref DataSet oDS)   //PRIMEPOS-3015 26-Oct-2021 JY Added oDS
        {
            ContAcctTran oAcctTran = new ContAcctTran();
            string sRXNo = string.Empty;
            //string SSQL = "SELECT * FROM ACCTTRAN WHERE TRANS_TYPE = 'C' AND ";   //PRIMEPOS-2855 27-May-2020 JY Commented
            string SSQL = "SELECT * FROM (SELECT ROW_NUMBER() OVER(PARTITION BY RXNO, REFILL_NO ORDER BY TRANSNO DESC) rNum, * FROM ACCTTRAN";     //PRIMEPOS-2855 27-May-2020 JY Added
            string sRefill = string.Empty;
            oDS = new DataSet();
            string[] arr = { "", "", "", "" };
            frmRxMarkedForDelivery oRXforDelivery = new frmRxMarkedForDelivery();
            ListViewItem oItem;
            string RetVal = string.Empty;
            try
            {
                foreach (TransDetailRXRow oRow in oTransRXDData.TransDetailRX.Rows)
                {
                    try
                    {
                        TransDetailRow oTransDDRow = oTransDData.TransDetail.GetRowByID(oRow.TransDetailID);
                        if (oTransDDRow == null || oTransDDRow.ExtendedPrice > 0)
                        {
                            sRXNo += " (RXNO = '" + oRow.RXNo + "' AND REFILL_NO = '" + oRow.NRefill + "') OR";
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                if (string.IsNullOrEmpty(sRXNo) == true)
                {
                    return RetVal;
                }
                if (sRXNo.EndsWith("OR"))
                {
                    sRXNo = sRXNo.Substring(0, sRXNo.Length - 2);
                }
                //SSQL += sRXNo;    //PRIMEPOS-2855 27-May-2020 JY Commented
                SSQL += " WHERE " + sRXNo + ") a WHERE rNum = 1 AND Trans_Type = 'C'";  //PRIMEPOS-2855 27-May-2020 JY Added
                try
                {
                    oAcctTran.GetRecs(SSQL, out oDS);
                }
                catch
                {
                    oDS = null;
                }
                if (Configuration.isNullOrEmptyDataSet(oDS) == true)
                {
                    return RetVal;
                }
                oRXforDelivery.lvItemList.Columns.Add("Trans Date");
                oRXforDelivery.lvItemList.Columns.Add("Trans Amt");
                oRXforDelivery.lvItemList.Columns[0].Width = 70;
                oRXforDelivery.lvItemList.Columns[1].Width = 50;
                oRXforDelivery.lvItemList.Columns[2].Width = 150;
                oRXforDelivery.lvItemList.Columns[3].Width = 80;

                //oRXforDelivery.lblMSG.Text = "Above RX(s) charge is already posted in PrimeRX.\nPlease make the copay amount to "+Configuration.CInfo.CurrencySymbol.ToString()
                //	+"0 or remove the RX(s) from transaction."; //PRIMEPOS-3015 26-Oct-2021 JY Commented
                oRXforDelivery.lblMSG.Text = "Above Rx(s) charge is already posted in PrimeRx. " + Environment.NewLine +
                    "Click on \"Yes\" to make the copay amount to " + Configuration.CInfo.CurrencySymbol.ToString() + "0 and proceed." + Environment.NewLine +
                    "Click on \"No\" to remove the Rx(s) manually from the transaction.";    //PRIMEPOS-3015 26-Oct-2021 JY Added

                oRXforDelivery.Text = "Duplicate Charge Posting";
                foreach (DataRow oRow in oDS.Tables[0].Rows)
                {
                    arr[0] = Configuration.convertNullToString(oRow["RXNO"]).Trim();
                    arr[1] = Configuration.convertNullToString(oRow["REFILL_NO"]).Trim();
                    string TransDate = Configuration.convertNullToString(oRow["TRAN_DATE"]).Trim();
                    arr[2] = TransDate.Substring(0, 10);
                    arr[3] = Configuration.convertNullToDecimal(oRow["TRAN_AMT"]).ToString("#########0.00").Trim();
                    oItem = new ListViewItem(arr);
                    oRXforDelivery.lvItemList.Items.Add(oItem);
                }
                oRXforDelivery.ShowDialog();
                RetVal = oRXforDelivery.SelectedAction;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return RetVal;
        }
    }
}
