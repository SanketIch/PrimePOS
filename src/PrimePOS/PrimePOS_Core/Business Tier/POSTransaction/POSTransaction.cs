using System;
using System.Data;
using System.Windows.Forms;
using MMSChargeAccount;
using PharmData;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
////using POS.Resources;
using System.Collections.Generic;
using System.Linq;
using NLog;
using MMS.Device;
using MMS.Device.Global;
using Resources;
using System.Collections;
using System.Drawing;
using POS_Core.Resources;
using POS_Core.Resources.DelegateHandler;
using POS_Core.TransType;
using POS_Core.CommonClass;
using POS_Core_UI;
using POS_Core.Resources.PaymentHandler;
using System.Text;
using PossqlData;

namespace POS_Core.BusinessRules
{


    public partial class POSTransaction : IDisposable
    {

        #region declaration
        public TransHeaderData oTransHData { get; set; }
        public TransHeaderRow oTransHRow { get; set; }
        public TransDetailData oTransDData { get; set; }
        public TransDetailRow oTDRow { get; set; }
        public TransDetailRXData oTransDRXData { get; set; }
        public RXHeaderList oRXHeaderList { get; set; }
        public CustomerRow oCustomerRow { get; set; }
        public POSTransPaymentData oPOSTransPaymentData { get; set; }
        public TransDetailTaxData oTDTaxData { get; set; }
        public TaxCodesData oTaxCodesData { get; set; }
        public ItemMonitorCategoryDetailRow oIMCDetailRow { get; set; }
        public POSTransSignLogData oTransSignLogData { get; set; }
        public ItemMonitorCategoryRow oIMCategoryRow { get; set; }
        public ArrayList strOTCItemDescriptions { get; set; }
        public DepartmentRow oDeptRow { get; set; }
        public bool isBySignPresent { get; set; }
        public bool isByDLPresent { get; set; }
        public bool isByBoth { get; set; }
        public bool isAgeLimit { get; set; }

        public string monitorItemOverriddenBy { get; set; } //PRIMEPOS-3166
        public bool showOverrideBtn { get; set; }//PRIMEPOS-3166N
        public bool skipMoveNext { get; set; }
        public bool IsItemExist { get; set; }
        public string unbilledRx { get; set; }
        public int countUnBilledRx { get; set; }
        public int isUnBilledRx { get; set; }   //PRIMEPOS-2398 04-Jan-2021 JY modified
        public string strDiscountPolicy { get; set; }
        public string OTCSignDataText { get; set; }
        public bool ItemAlreadyProcess { get; set; }


        //Added By Shitaljit(QuicSolv) on 16 May 2011
        private PccCardInfo CustomerCardInfo;
        //Till Here added By Shitaljit(QuicSolv)

        //Added by Manoj 1/23/2013
        private static bool AllowESCRxPickedUp;
        private bool IsPOSAllowRX;
        public static bool isRxConsentHave;//PRIMEPOS-3192
        private string PickupRx;
        public bool IsPriceChange { get; set; }
        public int LineRow { get; set; }
        public bool IsInvoiceDisc { get; set; }
        /// <summary>
        /// Get or Set the Row that is click
        /// </summary>
        public int ClickRow { get; set; } //Added by Manoj 7/29/2015
        //End Here
        public POSTransactionType CurrentTransactionType;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        #region PRIMEPOS-2761 - NileshJ
        DataTable dtOldRxData = new DataTable();
        public bool isRxTxnStatus = false;
        public DataSet dsOrgRxData = new DataSet();
        RxTransactionData oRxTransactionData = new RxTransactionData();
        RxTransactionDataRow oRxTransactionDataRow = null;
        RxTransactionDataSvr oRxTransDataSvr = new RxTransactionDataSvr();
        InsSigTransSvr oInsSigTransSvr = new InsSigTransSvr();
        #endregion
        ConsentTansmissionLogSvr oConsentTansmissionLogSvr = new ConsentTansmissionLogSvr();//PRIMEPOS-2866,PRIMEPOS-2871
        #endregion
        clsCoreHouseCharge clsHouseCharge = new clsCoreHouseCharge();
        public StoreCreditData oStoreCreditData = new StoreCreditData();   //PRIMEPOS-2938 28-Jan-2021 JY Added
        private PictureBox picSignature; //PRIMEPOS-3055
        public POSTransaction()
        {
            skipMoveNext = false;
            IsItemExist = false;
            isUnBilledRx = 0;   //PRIMEPOS-2398 04-Jan-2021 JY modified
            isBySignPresent = false;
            isByDLPresent = false;
            isByBoth = false;
            isAgeLimit = false;
            unbilledRx = string.Empty;
            strDiscountPolicy = string.Empty;
            OTCSignDataText = "";
            countUnBilledRx = 0;
            oTDRow = null;
            oTransHData = new TransHeaderData();
            oTransHRow = oTransHData.TransHeader.AddRow(0, System.DateTime.Now, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            oTransDData = new TransDetailData();
            oTransDRXData = new TransDetailRXData();
            oPOSTransPaymentData = new POSTransPaymentData();
            oTDTaxData = new TransDetailTaxData();
            oRXHeaderList = new RXHeaderList();
            oTaxCodesData = new TaxCodesData();
            oTransSignLogData = new POSTransSignLogData();
            strOTCItemDescriptions = new ArrayList();
            oDeptRow = null;


        }

        #region Persist Methods

        public void Persist(TransHeaderData oTransHData, TransDetailData oTransDData, POSTransPaymentData oTransPData, Int32 onHoldTransID,
            System.Decimal InvDiscPerc, bool isReceiveOnAccount, RXHeaderList oRXInfoList, POSTransPayment_CCLogList oPOSTransPayment_CCLogList,
            TransDetailRXData oRXDetailData, CLCardsRow oCLCardsRow, POSTransSignLogData oTransSignLogData, DataTable RxData, TransDetailTaxData oTDTaxData,
            ref bool isCLTierreached, ref decimal CLCouponValue, string pseTrxId, bool bItemMonitorInTrans, List<OnholdRxs> lstOnHoldRxs,
            bool isBatchDelivery = false, string strOverrideMaxStationCloseCashLimit = "", string strMaxTransactionAmountUser = "", string strMaxReturnTransactionAmountUser = "", string strInvDiscOverrideUser = "", string strMaxDiscountLimitOverrideUser = "", DataTable selectedRxDt = null) //PRIMEPOS-2639 27-Mar-2019 JY Added lstOnHoldRxs // PRIMERX-7688 - NileshJ - Added isBatchDelivery 23-Sept-2019   //PRIMEPOS-2402 12-Jul-2021 JY Added strOverrideMaxStationCloseCashLimit, strMaxTransactionAmountUser, strMaxReturnTransactionAmountUser, strInvDiscOverrideUser, strMaxDiscountLimitOverrideUser //PRIMEPOS-3192 Added selectedRxDt
        {
            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()" + oTransHData.TransHeader[0].TransType.ToString(), clsPOSDBConstants.Log_Entering);
            logger.Trace("Persist() - " + clsPOSDBConstants.Log_Entering);
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            decimal oldClValue = 0;
            decimal newClValue = 0;
            int oldClId = 0, NewClId = 0;   //Sprint-25 - PRIMEPOS-2297 28-Feb-2017 JY  Added to fix the issue with the CL Tier Reached logic. If we generated same no of coupons that we have available in CL_Coupons tables, it will not hit the IsClTierReached flag as "oldClValue" and "newClValue" both will be equal.

            try
            {
                Customer oCustomer = new Customer();
                CustomerData oCustomerData = oCustomer.GetCustomerByID(oTransHData.TransHeader[0].CustomerID);
                bool updateLoyaltyData = false;
                decimal transactionLoyaltyPoints = 0;

                if (oCLCardsRow != null && (oTransHData.TransHeader[0].TransType == 1 || oTransHData.TransHeader[0].TransType == 2) && Configuration.CLoyaltyInfo.UseCustomerLoyalty == true)
                {
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Calculate CL Points Started");
                    logger.Trace("Persist() - Calculate CL Points Started");
                    updateLoyaltyData = true;

                    if (oCustomerData.Customer[0].UseForCustomerLoyalty == false)
                    {
                        oTransHData.TransHeader[0].LoyaltyPoints = transactionLoyaltyPoints = 0;
                    }
                    else if (Configuration.CLoyaltyInfo.DisableAutoPointCalc == false)
                    {
                        CLCards oCLCard = new CLCards();
                        //Added By Shitaljit to Calcualte points after Coupon Discount
                        DataRow[] oRow = oTransPData.POSTransPayment.Select(clsPOSDBConstants.POSTransPayment_Fld_CLCouponID + " > 0");
                        if (oRow.Length == 0)
                        {
                            transactionLoyaltyPoints = oCLCard.CalculatePoints(oTransHData.TransHeader[0], oTransDData);
                        }
                        else
                        {
                            transactionLoyaltyPoints = oCLCard.CalculatePointsAfterCouponDiscount(oTransHData.TransHeader[0], oTransDData, Configuration.convertNullToDecimal(oRow[0][clsPOSDBConstants.POSTransPayment_Fld_Amount]));
                        }
                        oTransHData.TransHeader[0].LoyaltyPoints = transactionLoyaltyPoints;
                    }
                    else
                    {
                        transactionLoyaltyPoints = oTransHData.TransHeader[0].LoyaltyPoints;
                    }

                    CLCouponsSvr clcoup = new CLCouponsSvr();
                    //CLCouponsData clData = clcoup.GetByCLCardID(oCLCardsRow.CLCardID);    //Sprint-19 - 01-Apr-2015 JY Commented
                    CLCouponsData clData = clcoup.GetByCLCardID(oCLCardsRow.CLCardID, "PosTrans");  //Sprint-19 - 01-Apr-2015 JY Added 
                    try
                    {
                        if (clData.CLCoupons.Rows.Count > 0)
                        {
                            oldClValue = Configuration.convertNullToDecimal(clData.CLCoupons.Rows[0][clsPOSDBConstants.CLCoupons_Fld_CouponValue]);
                            oldClId = Configuration.convertNullToInt(clData.CLCoupons.Rows[0][clsPOSDBConstants.CLCoupons_Fld_ID]); //Sprint-25 - PRIMEPOS-2297 28-Feb-2017 JY Added     
                        }
                    }
                    catch (Exception)
                    {
                    }

                    //clData.CLCoupons.CouponValue.va
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Calculate CL Points Done");
                    logger.Trace("Persist() - Calculate CL Points Done");
                    //}
                }

                TransHeaderSvr oTransHeaderSvr = new TransHeaderSvr();
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();
                POSTransPaymentSvr oTransPaymentSvr = new POSTransPaymentSvr();

                #region Sprint-23 - PRIMEPOS-2029 20-Apr-2016 JY Added logic to save item monitoring transaction details
                ItemMonitorTransDetailData oItemMonitorTransDetailData = new ItemMonitorTransDetailData();
                try
                {
                    logger.Trace("Persist() - Added logic to save item monitoring transaction details");
                    if (bItemMonitorInTrans == true) //if monitoring item present in trans
                    {
                        int cnt = 1;
                        bool bPSEItemInTable = false;
                        //bool bMonitoringItemStatus = false; //Sprint-25 24-Feb-2017 JY Added
                        List<int> lstTransDetailIds = new List<int>();   //PRIMEPOS-3082 01-Apr-2022 JY Added
                        if (Configuration.CInfo.useNplex == true)
                        {
                            Item oItem = new Item();
                            foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                            {
                                DataTable dtPSE_Items = oItem.IsPSEItemData(oRow.ItemID);
                                if (dtPSE_Items != null && dtPSE_Items.Rows.Count > 0)
                                {
                                    bPSEItemInTable = true;
                                    oItemMonitorTransDetailData.ItemMonitorTransDetail.AddRow(cnt++, oRow.TransDetailID, Configuration.PSE_Items_Monitoring_Category, Configuration.PSE_Items_UOM, Convert.ToDecimal(dtPSE_Items.Rows[0]["ProductGrams"]), Configuration.PSE_Items_SentToNplex, Configuration.CInfo.postSaleInd, Configuration.convertNullToInt64(pseTrxId));
                                    lstTransDetailIds.Add(oRow.TransDetailID);  //PRIMEPOS-3082 01-Apr-2022 JY Added
                                }
                                #region Sprint-25 24-Feb-2017 JY Added logic to add only monitoring items (non-sudafed) in ItemMonitorTransDetail log
                                else
                                {
                                    DataSet dt = oTransDetailSvr.GetItemMonitoringdetails(oRow.ItemID);
                                    if (dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                                    {
                                        //bMonitoringItemStatus = true;
                                        for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                                        {
                                            oItemMonitorTransDetailData.ItemMonitorTransDetail.AddRow(cnt++, oRow.TransDetailID, Configuration.convertNullToInt(dt.Tables[0].Rows[i]["ItemMonCatID"]), dt.Tables[0].Rows[i]["UOM"].ToString(), Configuration.convertNullToDecimal(dt.Tables[0].Rows[i]["UnitsPerPackage"]), false, false, 0);
                                            lstTransDetailIds.Add(oRow.TransDetailID);  //PRIMEPOS-3082 01-Apr-2022 JY Added
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                        if (bPSEItemInTable == false)
                        {
                            foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                            {
                                bool isExist = lstTransDetailIds.Contains(Configuration.convertNullToInt(oRow.TransDetailID));  //PRIMEPOS-3082 01-Apr-2022 JY Added
                                if (!isExist)
                                {
                                    Boolean? isSudafed = oTransDetailSvr.IsSudaFedItem(oRow.ItemID);
                                    if (isSudafed != null)
                                    {
                                        DataSet dt = oTransDetailSvr.GetItemMonitoringdetails(oRow.ItemID);
                                        if (dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                                            {
                                                bool bNplex = false;
                                                if ((Configuration.convertNullToBoolean(dt.Tables[0].Rows[i]["ePSE"]) == true) && (Configuration.CInfo.useNplex == true) && Configuration.convertNullToInt64(pseTrxId) != 0)
                                                    bNplex = true;
                                                oItemMonitorTransDetailData.ItemMonitorTransDetail.AddRow(cnt++, oRow.TransDetailID, Configuration.convertNullToInt(dt.Tables[0].Rows[i]["ItemMonCatID"]), dt.Tables[0].Rows[i]["UOM"].ToString(), Configuration.convertNullToDecimal(dt.Tables[0].Rows[i]["UnitsPerPackage"]), bNplex, Configuration.CInfo.postSaleInd, Configuration.convertNullToInt64(pseTrxId));
                                            }
                                        }
                                    }
                                    #region Sprint-25 24-Feb-2017 JY Added logic to add only monitoring items (non-sudafed) in ItemMonitorTransDetail log
                                    else
                                    {
                                        //if (bMonitoringItemStatus == false)
                                        //{
                                        DataSet dt = oTransDetailSvr.GetItemMonitoringdetails(oRow.ItemID);
                                        if (dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                                            {
                                                oItemMonitorTransDetailData.ItemMonitorTransDetail.AddRow(cnt++, oRow.TransDetailID, Configuration.convertNullToInt(dt.Tables[0].Rows[i]["ItemMonCatID"]), dt.Tables[0].Rows[i]["UOM"].ToString(), Configuration.convertNullToDecimal(dt.Tables[0].Rows[i]["UnitsPerPackage"]), false, false, 0);
                                            }
                                        }
                                        //}
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    logger.Trace("Persist() - save item monitoring transaction details done");
                }
                catch (Exception Ex1)
                {
                    logger.Fatal(Ex1, "Persist()");
                    throw (Ex1);
                }
                #endregion

                //checkIsValidData(oTransHData,oTransDData,oTransPData);
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize connection Object with : "+POS.Resources.Configuration.ConnectionString);
                //logger.Trace("Persist() - Initialize connection Object with : " + Configuration.ConnectionString);
                logger.Trace("Persist() - Initialize connection Object with : " + Configuration.convertNullToString(Configuration.ServerName) + " - " + Configuration.convertNullToString(Configuration.DatabaseName));
                oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed creating connection Object with : " + POS.Resources.Configuration.ConnectionString);
                //logger.Trace("Persist() - Completed creating connection Object with : " + Configuration.ConnectionString);
                logger.Trace("Persist() - Completed creating connection Object with : " + Configuration.convertNullToString(Configuration.ServerName) + " - " + Configuration.convertNullToString(Configuration.DatabaseName));

                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Begin Transaction ");
                logger.Trace("Persist() - Begin Transaction");
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                System.Int32 TransID = 0;

                if (onHoldTransID > 0)
                {
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize DeleteOnHoldRow Started ");
                    logger.Trace("Persist() - Initialize DeleteOnHoldRow Started");
                    oTransHeaderSvr.DeleteOnHoldRows(oTrans, onHoldTransID);
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed DeleteOnHoldRow  ");
                    logger.Trace("Persist() - Completed DeleteOnHoldRow");
                }
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize Writing data to POSTransaction Table ");

                #region PRIMEPOS-2639 27-Mar-2019 JY Added logic to delete scanned on hold rx data
                if (lstOnHoldRxs.Count > 0)
                {
                    oTransHeaderSvr.DeleteOnHoldRows(oTrans, lstOnHoldRxs);
                }
                #endregion

                logger.Trace("Persist() - Initialize Writing data to POSTransaction Table");
                oTransHData.TransHeader[0].AllowRxPicked = Configuration.AllowRxPicked; //PRIMEPOS-2865 16-Jul-2020 JY Added
                oTransHData.TransHeader[0].RxTaxPolicyID = Configuration.convertNullToInt(Configuration.CSetting.RxTaxPolicy.Trim()); //PRIMEPOS-3053 08-Feb-2021 JY Added
                oTransHData.TransHeader[0].TotalTransFeeAmt = CalculateTotalTransFeeAmt(oTransPData);  //PRIMEPOS-3117 11-Jul-2022 JY Added
                oTransHeaderSvr.Persist(oTransHData, oTrans, ref TransID);
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Completed Writing data to POSTransaction Table  ");
                logger.Trace("Persist() - Completed Writing data to POSTransaction Table");
                if (TransID > 0)
                {
                    if (isReceiveOnAccount == false)
                    {
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize Writing data to POSTransactionDetail Table ");
                        logger.Trace("Persist() - Initialize Writing data to POSTransactionDetail Table");
                        oTransDetailSvr.Persist(oTransDData, oTrans, TransID, oRXDetailData, oTDTaxData, oItemMonitorTransDetailData, oTransSignLogData);
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed Writing data to POSTransactionDetail Table Completed ");
                        logger.Trace("Persist() - Completed Writing data to POSTransactionDetail Table Completed");

                        //Added By shitaljit 
                        //Run this logic iff transaction has RX items.
                        if (Configuration.isNullOrEmptyDataSet(oRXDetailData) == false)
                        {
                            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize Writing data to POSTransactionRXDetail Table ");
                            logger.Trace("Persist() - Initialize Writing data to POSTransactionRXDetail Table");
                            TransDetailRXSvr oSvr = new TransDetailRXSvr();

                            // added by atul 07-jan-2011
                            for (int i = 0; i < oRXDetailData.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < oRXInfoList.Count; j++)
                                {
                                    for (int k = 0; k < oRXInfoList[j].RXDetails.Count; k++)
                                    {
                                        if (oRXDetailData.Tables[0].Rows[i][clsPOSDBConstants.TransDetailRX_Fld_RXNo].ToString().Trim() == oRXInfoList[j].RXDetails[k].RXNo.ToString().Trim())
                                        {
                                            oRXDetailData.Tables[0].Rows[i][clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq] = oRXInfoList[j].CounselingRequest.ToString();
                                        }
                                    }
                                }
                            }
                            //End of added by atul 07-jan-2011

                            //Added by Manoj 1/7/2015
                            foreach (DataRow detRow in oTransDData.Tables[0].Rows)
                            {
                                if (detRow["ITEMID"].ToString().ToUpper().Trim() != "RX")
                                {
                                    continue;
                                }
                                //foreach (DataRow rxDet in oRXDetailData.Tables[0].Rows)
                                //{
                                //    if (Convert.ToInt32(rxDet["RXDetailID"]) == Convert.ToInt32(detRow["TransDetailID"]) && detRow["ItemDescription"].ToString().Contains(rxDet["RXNO"].ToString()))
                                //    {
                                //        rxDet["ReturnTransDetailID"] = Configuration.convertNullToInt(detRow["ReturnTransDetailID"]);
                                //    }
                                //}
                                #region PRIMEPOS-3012 07-Oct-2021 JY Added                                
                                bool bRecordFound = false;
                                foreach (DataRow rxDet in oRXDetailData.Tables[0].Rows)
                                {
                                    if (Configuration.convertNullToInt(rxDet["ReturnTransDetailID"]) == 0 && Convert.ToInt32(rxDet["RXDetailID"]) == Convert.ToInt32(detRow["TransDetailID"]) && detRow["ItemDescription"].ToString().Contains(rxDet["RXNO"].ToString()))
                                    {
                                        rxDet["ReturnTransDetailID"] = Configuration.convertNullToInt(detRow["ReturnTransDetailID"]);
                                        bRecordFound = true;
                                        break;
                                    }
                                }
                                try
                                {
                                    if (!bRecordFound)
                                    {
                                        PharmBL oPharmBL = new PharmBL();
                                        string ItemDesc = Configuration.convertNullToString(detRow["ItemDescription"]);
                                        int nSecondOccurance = ItemDesc.IndexOf('-', ItemDesc.IndexOf('-') + 1);
                                        string sRxNo = "";
                                        if (nSecondOccurance != -1)
                                            sRxNo = ItemDesc.Substring(0, nSecondOccurance);
                                        if (sRxNo != "")
                                        {
                                            string[] arr = sRxNo.Split('-');
                                            string strRxNo = arr[0].Trim();
                                            string strnRefill = arr[1].Trim();
                                            DataTable oRxInfo = oPharmBL.GetRxsWithStatus(strRxNo, strnRefill, "");
                                            if (oRxInfo != null && oRxInfo.Rows.Count > 0)
                                            {
                                                DataTable oTable = oPharmBL.GetPatient(oRxInfo.Rows[0]["PATIENTNO"].ToString());
                                                string ezcap = oTable.Rows[0]["EZCAP"].ToString();
                                                RXDetail oRXDetail = BuildRxDetail(oRxInfo.Rows[0]);
                                                string CounselingRequest = "";
                                                bool bFound = false;
                                                for (int j = 0; j < oRXInfoList.Count; j++)
                                                {
                                                    for (int k = 0; k < oRXInfoList[j].RXDetails.Count; k++)
                                                    {
                                                        if (strRxNo == oRXInfoList[j].RXDetails[k].RXNo.ToString().Trim())
                                                        {
                                                            CounselingRequest = Configuration.convertNullToString(oRXInfoList[j].CounselingRequest);
                                                            bFound = true;
                                                            break;
                                                        }
                                                    }
                                                    if (bFound == true)
                                                        break;
                                                }

                                                oTransDRXData.TransDetailRX.AddRow(Configuration.convertNullToInt(detRow["TransDetailID"]),
                                                Convert.ToDateTime(oRXDetail.RxDate), oRXDetail.RXNo, oRxInfo.Rows[0]["NDC"].ToString(),
                                                Configuration.convertNullToInt64(oRxInfo.Rows[0]["PATIENTNO"]), oRxInfo.Rows[0]["BILLTYPE"].ToString(),
                                                oRxInfo.Rows[0]["PATTYPE"].ToString(), (int)oRXDetail.RefillNo,
                                                CounselingRequest, ezcap, Configuration.convertNullToString(oRxInfo.Rows[0]["DELIVERY"]), (int)oRXDetail.PartialFillNo);//PRIMEPOS-3008 30-Sep-2021 JY Added Delivery
                                            }
                                        }
                                    }
                                }
                                catch (Exception Ex1)
                                {
                                }
                                #endregion
                            }
                            oSvr.Persist(oRXDetailData, oTrans);
                            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed Writing data to POSTransactionRXDetail Table ");
                            logger.Trace("Persist() - Completed Writing data to POSTransactionRXDetail Table");
                        }
                    }
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize Writing data to POSTransPayment Table ");
                    logger.Trace("Persist() - Initialize Writing data to POSTransPayment Table");
                    oTransPaymentSvr.Persist(oTransPData, oTrans, TransID);
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed Writing data to POSTransPayment Table ");
                    logger.Trace("Persist() - Completed Writing data to POSTransPayment Table");
                    bool couponUsedInPayment = false;

                    if (oTransPData.POSTransPayment.Rows.Count > 0)
                    {

                        foreach (POSTransPaymentRow orow in oTransPData.POSTransPayment.Rows)
                        {
                            if (orow[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString().Trim() == "H")
                            {
                                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize posting house charge  ");
                                logger.Trace("Persist() - Initialize posting house charge");
                                if (oTransHData.TransHeader[0].TransType == 1)
                                    clsCoreHouseCharge.postHouseCharge(oTransDData, oTransPData, POSTransactionType.Sales, InvDiscPerc, orow, TransID);
                                else
                                    clsCoreHouseCharge.postHouseCharge(oTransDData, oTransPData, POSTransactionType.SalesReturn, InvDiscPerc, orow, TransID);
                                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed Posting house charge");
                                logger.Trace("Persist() - Completed Posting house charge");
                                break;
                            }
                            else if (orow.CLCouponID > 0)
                            {
                                couponUsedInPayment = true;
                            }
                        }
                    }
                    #region PRIMEPOS-2761 - Commented
                    //if (oRXInfoList.Count > 0)
                    //{
                    //    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize Updating RX picked up Details");
                    //    logger.Trace("Persist() - Initialize Updating RX picked up Details");
                    //    if (oTransHData.TransHeader[0].TransType == 2)
                    //    {
                    //        UpdateRXData(oRXInfoList, true, oTransHData.TransHeader[0].IsDelivery, oTrans, oTransDData.TransDetail[0].TransID, isBatchDelivery);// PRIMERX-7688 - NileshJ - BatchDelivery 23-Sept-2019
                    //    }
                    //    else
                    //    {
                    //        UpdateRXData(oRXInfoList, false, oTransHData.TransHeader[0].IsDelivery, oTrans, oTransDData.TransDetail[0].TransID, isBatchDelivery);// PRIMERX-7688 - NileshJ - BatchDelivery 23-Sept-2019
                    //    }
                    //    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Completed Updating RX picked up Details");
                    //    logger.Trace("Persist() - Completed Updating RX picked up Details");
                    //}
                    #endregion
                    if (isReceiveOnAccount == true)
                    {
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize postROA");
                        logger.Trace("Persist() - Initialize postROA");
                        if (oTransHData.TransHeader[0].TotalPaid < 0)
                        {
                            clsCoreHouseCharge.postROA(oTransHData.TransHeader[0].Acc_No, oTransPData, POSTransactionType.SalesReturn);
                        }
                        else
                        {
                            clsCoreHouseCharge.postROA(oTransHData.TransHeader[0].Acc_No, oTransPData, POSTransactionType.Sales);
                        }
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Completed postROA");
                        logger.Trace("Persist() - Completed postROA");
                    }

                    if (oPOSTransPayment_CCLogList != null)
                    {
                        oPOSTransPayment_CCLogList.RemoveAll();
                    }

                    if (updateLoyaltyData == true && (transactionLoyaltyPoints != 0 || couponUsedInPayment))
                    {
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize ProcessPointsInTiers()");
                        logger.Trace("Persist() - Initialize ProcessPointsInTiers()");
                        CLPointsRewardTier oCLPointsRewardTier = new CLPointsRewardTier();
                        //oCLPointsRewardTier.ProcessPointsInTiers(transactionLoyaltyPoints, TransID, oCLCardsRow.CLCardID, oTrans);    //Sprint-18 - 2090 10-Oct-2014 JY Commented
                        oCLPointsRewardTier.ProcessPointsInTiers(transactionLoyaltyPoints, TransID, oCLCardsRow.CLCardID, (System.DateTime)oTransHData.Tables[0].Rows[0]["TransDate"], "T", (System.Int32)oTransHData.Tables[0].Rows[0]["TransType"], oTrans);  //Sprint-18 - 2090 10-Oct-2014 JY Added transdate parameter
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Completed ProcessPointsInTiers()");
                        logger.Trace("Persist() - Completed ProcessPointsInTiers()");
                    }
                    //Added by shitaljit on 3 May 2012
                    //to save POSTransSingLog details
                    if (oTransSignLogData != null)
                    {
                        if (oTransSignLogData.Tables[0].Rows.Count > 0)
                        {
                            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize Saving Signature()");
                            logger.Trace("Persist() - Initialize Saving Signature()");
                            POSTransSignLog oPOSTransSignLog = new POSTransSignLog();
                            oPOSTransSignLog.Persist(oTransSignLogData, TransID, oTrans);
                            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Completed Saving Signature()");
                            logger.Trace("Persist() - Completed Saving Signature()");
                        }
                    }
                    //if (this.CurrentTransactionType == POSTransactionType.Sales && Configuration.isNullOrEmptyDataSet(oTDTaxData) == false)   //Sprint-18 - 2142 10-Dec-2014 JY commented as tax computed for sales trans only, it should be for return trans as well
                    if (Configuration.isNullOrEmptyDataSet(oTDTaxData) == false)    //Sprint-18 - 2142 10-Dec-2014 JY Added  
                    {
                        TransDetailTaxSvr oTDTaxSvr = new TransDetailTaxSvr();
                        oTDTaxSvr.Persist(oTDTaxData, oTrans, TransID);
                    }
                    /* Added by Manoj for the Drug Class */
                    if (RxData != null && RxData.Rows.Count > 0)
                    {
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize Saving InsertRxPickUpLog() About to call PharmSQL");
                        logger.Trace("Persist() - Initialize Saving InsertRxPickUpLog() About to call PharmSQL");
                        PharmBL oPBL = new PharmBL();
                        oPBL.InsertRxPickUpLog(RxData);
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize Saving InsertRxPickUpLog(), PharmSQL Successful");
                        logger.Trace("Persist() - Initialize Saving InsertRxPickUpLog(), PharmSQL Successful");
                    }

                    #region Sprint-23 - PRIMEPOS-2029 20-Apr-2016 JY Added logic to save item monitoring transaction details
                    if (oItemMonitorTransDetailData.ItemMonitorTransDetail.Rows.Count > 0)
                    {
                        ItemMonitorTransDetailSvr oItemMonitorTransDetailSvr = new ItemMonitorTransDetailSvr();
                        oItemMonitorTransDetailSvr.Persist(oItemMonitorTransDetailData, oTrans);
                    }
                    #endregion

                    #region PRIMEPOS-2938 28-Jan-2021 JY Added to save store credit
                    try
                    {
                        if (Configuration.CPOSSet.EnableStoreCredit && oTransHData.TransHeader[0].CustomerID.ToString().Trim() != "")
                        {
                            logger.Trace("Persist() - Start - Added logic to save store credit data");
                            StoreCredit oStoreCredit = new StoreCredit();
                            StoreCreditRow oStoreCreditRow;
                            StoreCreditDetailsData oStoreCreditDetailsData = new StoreCreditDetailsData();
                            StoreCreditDetailsRow oStoreCreditDetailsRow;
                            StoreCreditDetails oStoreCreditDetails = new StoreCreditDetails();
                            DataSet dsAlreadyStoreCreditCustomer = new DataSet();
                            int StoreCreditID = 0;

                            var CreditAmount = oTransPData.POSTransPayment.AsEnumerable().Where(x => x.Field<string>("TransTypeCode").Trim().ToUpper() == "X").Select(p => p.Field<decimal>("Amount")).FirstOrDefault();
                            if (this.CurrentTransactionType == POSTransactionType.Sales)
                                CreditAmount = Math.Abs(CreditAmount);
                            else if (this.CurrentTransactionType == POSTransactionType.SalesReturn)
                                CreditAmount = Math.Abs(CreditAmount) * -1;

                            if (CreditAmount != 0)
                            {
                                if (oStoreCreditData.Tables[0].Rows.Count > 0)
                                {
                                    dsAlreadyStoreCreditCustomer = oStoreCredit.GetByCustomerID(oTransHData.TransHeader[0].CustomerID, oTrans);
                                    if (dsAlreadyStoreCreditCustomer != null && dsAlreadyStoreCreditCustomer.Tables.Count > 0 && dsAlreadyStoreCreditCustomer.Tables[0].Rows.Count > 0)
                                    {
                                        oStoreCredit.UpdateCreditAmount(oStoreCreditData, oTrans);
                                        StoreCreditID = Convert.ToInt32(dsAlreadyStoreCreditCustomer.Tables[0].Rows[0]["StoreCreditID"].ToString());
                                    }
                                    else
                                    {
                                        oStoreCreditRow = oStoreCreditData.StoreCredit.AddRow(0, oTransHData.TransHeader[0].CustomerID, Convert.ToDecimal(oStoreCreditData.Tables[0].Rows[0]["CreditAmt"]), DateTime.Now, Configuration.UserName, 0);
                                        oStoreCredit.Persist(oStoreCreditData, oTrans);
                                        oStoreCreditData = oStoreCredit.GetByCustomerID(oTransHData.TransHeader[0].CustomerID, oTrans);
                                        StoreCreditID = Convert.ToInt32(oStoreCreditData.Tables[0].Rows[0]["StoreCreditID"].ToString());
                                    }
                                    if (oStoreCreditDetailsData != null) oStoreCreditDetailsData.StoreCreditDetails.Rows.Clear();
                                    oStoreCreditDetailsRow = oStoreCreditDetailsData.StoreCreditDetails.AddRow(0, StoreCreditID, oTransHData.TransHeader[0].CustomerID, TransID, Convert.ToDecimal(CreditAmount) * -1, DateTime.Now, Configuration.UserName);
                                    oStoreCreditDetails.Persist(oStoreCreditDetailsData);
                                }
                                else
                                {
                                    oStoreCreditData = new StoreCreditData();
                                    if (oStoreCreditData != null)
                                    {
                                        oStoreCreditData.StoreCredit.Rows.Clear();
                                    }
                                    dsAlreadyStoreCreditCustomer = oStoreCredit.GetByCustomerID(oTransHData.TransHeader[0].CustomerID, oTrans);
                                    if (dsAlreadyStoreCreditCustomer != null && dsAlreadyStoreCreditCustomer.Tables.Count > 0 && dsAlreadyStoreCreditCustomer.Tables[0].Rows.Count > 0)
                                    {
                                        decimal NewCreditAmount = 0;
                                        if (this.CurrentTransactionType == POSTransactionType.Sales)
                                        {
                                            NewCreditAmount = Configuration.convertNullToDecimal(dsAlreadyStoreCreditCustomer.Tables[0].Rows[0]["CreditAmt"]) - Math.Abs(CreditAmount);
                                        }
                                        else if (this.CurrentTransactionType == POSTransactionType.SalesReturn)
                                        {
                                            NewCreditAmount = Configuration.convertNullToDecimal(dsAlreadyStoreCreditCustomer.Tables[0].Rows[0]["CreditAmt"]) + Math.Abs(CreditAmount);
                                        }
                                        oStoreCreditRow = oStoreCreditData.StoreCredit.AddRow(0, oTransHData.TransHeader[0].CustomerID, NewCreditAmount, DateTime.Now, Configuration.UserName, 0);
                                        oStoreCredit.UpdateCreditAmount(oStoreCreditData, oTrans);
                                        StoreCreditID = Convert.ToInt32(dsAlreadyStoreCreditCustomer.Tables[0].Rows[0]["StoreCreditID"].ToString());
                                    }
                                    else
                                    {
                                        oStoreCreditRow = oStoreCreditData.StoreCredit.AddRow(0, oTransHData.TransHeader[0].CustomerID, Math.Abs(CreditAmount), DateTime.Now, Configuration.UserName, 0);
                                        oStoreCredit.Persist(oStoreCreditData, oTrans);
                                        oStoreCreditData = oStoreCredit.GetByCustomerID(oTransHData.TransHeader[0].CustomerID, oTrans);
                                        StoreCreditID = Convert.ToInt32(oStoreCreditData.Tables[0].Rows[0]["StoreCreditID"].ToString());
                                    }
                                    if (oStoreCreditDetailsData != null) oStoreCreditDetailsData.StoreCreditDetails.Rows.Clear();
                                    oStoreCreditDetailsRow = oStoreCreditDetailsData.StoreCreditDetails.AddRow(0, StoreCreditID, oTransHData.TransHeader[0].CustomerID, TransID, Convert.ToDecimal(CreditAmount) * -1, DateTime.Now, Configuration.UserName);
                                    oStoreCreditDetails.Persist(oStoreCreditDetailsData);
                                }
                            }
                            logger.Trace("Persist() - End - Added logic to save store credit data");
                        }
                    }
                    catch (Exception Ex)
                    {
                        logger.Fatal(Ex, "Persist() - Save store credit record");
                    }
                    #endregion

                    oTransHData.TransHeader[0].TransID = TransID;
                    oTransHData.AcceptChanges();
                    oTransDData.AcceptChanges();
                    oTransPData.AcceptChanges();
                    #region  PRIMEPOS-2761 - NileshJ
                    if (oRXInfoList != null)
                    {
                        dsOrgRxData = FetchRxDetails(oRXInfoList);

                        if (oRXInfoList.Count > 0)
                        {
                            bool RxtxnSuccess = false;
                            logger.Trace("Persist() - Initialize Updating RX picked up Details");
                            if (oTransHData.TransHeader[0].TransType == 2) // Return
                            {
                                RxtxnSuccess = UpdateRxTransDataLocally(oRXInfoList, true, oTransHData.TransHeader[0].IsDelivery, oTrans, oTransDData.TransDetail[0].TransID, isBatchDelivery, selectedRxDt); //PRIMEPOS-3192 Added selectedRxDt
                            }
                            else // Sale
                            {
                                RxtxnSuccess = UpdateRxTransDataLocally(oRXInfoList, false, oTransHData.TransHeader[0].IsDelivery, oTrans, oTransDData.TransDetail[0].TransID, isBatchDelivery, selectedRxDt); //PRIMEPOS-3192 Added selectedRxDt
                            }
                            logger.Trace("Persist() - Completed Updating RX picked up Details");
                            if (!RxtxnSuccess)
                            {
                                RxTransRollback(dsOrgRxData);
                                logger.Trace("RxTransRollback(DataSet dsOriginalRxData) - " + clsPOSDBConstants.Log_Exiting);
                                //UpdateCcTransmissionLog(oTransPData , true);//IsReversal
                                return;
                            }
                        }
                    }
                    #endregion

                    #region PRIMEPOS-2402 12-Jul-2021 JY Added
                    //for Override Max Station Close Cash Limit
                    try
                    {
                        if (strOverrideMaxStationCloseCashLimit != "")
                        {
                            oTransDetailSvr.InsertOverrideDetails(3, TransID, 0, "", "", strOverrideMaxStationCloseCashLimit, oTrans);
                        }
                    }
                    catch (Exception Exp)
                    {
                        logger.Fatal(Exp, "Persist() - Update Override Max Station Close Cash Limit");
                    }

                    //for Max. Transaction Amount
                    try
                    {
                        if (strMaxTransactionAmountUser != "")
                        {
                            object MaxTransAmountOverrideUser = DataHelper.ExecuteScalar("SELECT MaxTransactionLimit FROM Users  WHERE UserID = '" + strMaxTransactionAmountUser + "'");
                            oTransDetailSvr.InsertOverrideDetails(13, TransID, 0, Configuration.UserMaxTransactionLimit.ToString(), Configuration.convertNullToString(MaxTransAmountOverrideUser), strMaxTransactionAmountUser, oTrans);
                        }
                    }
                    catch (Exception Exp)
                    {
                        logger.Fatal(Exp, "Persist() - Update Max. Transaction Amount");
                    }

                    //for Max. Return Transaction Amount
                    try
                    {
                        if (strMaxReturnTransactionAmountUser != "")
                        {
                            object MaxReturnTransAmountOverrideUser = DataHelper.ExecuteScalar("SELECT MaxReturnTransLimit FROM Users WHERE UserID = '" + strMaxReturnTransactionAmountUser + "'");
                            oTransDetailSvr.InsertOverrideDetails(14, TransID, 0, Configuration.UserMaxReturnTransLimit.ToString(), Configuration.convertNullToString(MaxReturnTransAmountOverrideUser), strMaxReturnTransactionAmountUser, oTrans);
                        }
                    }
                    catch (Exception Exp)
                    {
                        logger.Fatal(Exp, "Persist() - Update Max. Transaction Amount");
                    }

                    //1 Invoice Discount Override
                    try
                    {
                        if (strInvDiscOverrideUser != "" && oTransHData.TransHeader[0].InvoiceDiscount > 0)
                        {
                            oTransDetailSvr.InsertOverrideDetails(1, TransID, 0, "0", oTransHData.TransHeader[0].InvoiceDiscount.ToString(), strInvDiscOverrideUser, oTrans);
                        }
                    }
                    catch (Exception Exp)
                    {
                        logger.Fatal(Exp, "Persist() - Invoice Discount Override");
                    }
                    // 12	Max. Discount Limit
                    try
                    {
                        if (strMaxDiscountLimitOverrideUser != "" && oTransHData.TransHeader[0].InvoiceDiscount > 0)
                        {
                            object MaxDiscountLimitForOverriddenUser = DataHelper.ExecuteScalar("SELECT MaxDiscountLimit FROM Users  WHERE UserID = '" + strMaxDiscountLimitOverrideUser + "'");
                            oTransDetailSvr.InsertOverrideDetails(12, TransID, 0, Configuration.UserMaxDiscountLimit.ToString(), Configuration.convertNullToString(MaxDiscountLimitForOverriddenUser), strMaxDiscountLimitOverrideUser, oTrans);
                        }
                    }
                    catch (Exception Exp)
                    {
                        logger.Fatal(Exp, "Persist() - Max. Discount Limit");
                    }
                    #endregion

                    oTrans.Commit();

                    #region PRIMEPOS-2572 15-Jun-2020 JY moved the return block outside and added else part for sale transaction
                    try
                    {
                        if (Configuration.CInfo.useNplex == true)
                        {
                            #region Sprint-23 - PRIMEPOS-2029 15-Apr-2016 JY Added logic to return nplex
                            logger.Trace("Persist() - Added logic to return nplex");
                            bool bPSEItemInTable = false;
                            if (this.CurrentTransactionType == POSTransactionType.SalesReturn)
                            {
                                Boolean ePSE = false;
                                Item oItem = new Item();
                                #region Sprint-25 - PRIMEPOS-2380 14-Feb-2017 JY Added logic to check Item exists in PSE_Items table
                                foreach (DataRow row in oTransDData.Tables[0].Rows)
                                {
                                    DataTable dtPSE_Items = oItem.IsPSEItemData(row["ITEMID"].ToString());
                                    if (dtPSE_Items != null && dtPSE_Items.Rows.Count > 0)
                                    {
                                        ePSE = true;
                                        bPSEItemInTable = true;
                                        break;
                                    }
                                }
                                #endregion

                                if (bPSEItemInTable == false)
                                {
                                    foreach (DataRow row in oTransDData.Tables[0].Rows)
                                    {
                                        Boolean? result = oTransDetailSvr.IsSudaFedItem(row["ITEMID"].ToString());

                                        if (result == true)
                                        {
                                            ItemData oIData = oItem.Populate(row["ITEMID"].ToString());
                                            if (oIData != null && oIData.Item != null)
                                            {
                                                if (oIData.Item.Rows.Count > 0)
                                                {
                                                    if (oIData.Item[0].isOTCItem == true)
                                                    {
                                                        ePSE = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (ePSE == true)
                                {
                                    NplexBL oNplex = new NplexBL();
                                    NplexBL.PatientInfo PatInfo = new NplexBL.PatientInfo();
                                    ItemData oIData = null;
                                    oNplex.MethItem.Clear();

                                    //get customer
                                    if (oCustomerData.Customer.Count > 0)
                                    {
                                        PatInfo.ID = oCustomerData.Customer[0].DriveLicNo;
                                        try
                                        {
                                            PatInfo.ID = oTransSignLogData.POSTransSignLog[0].CustomerIDDetail.Substring(0, oTransSignLogData.POSTransSignLog[0].CustomerIDDetail.IndexOf('^'));
                                        }
                                        catch
                                        {
                                            PatInfo.ID = Configuration.AuthorizationNo; //12-Sep-2016 JY Added to preserve ID 
                                        }
                                        if (oTransSignLogData.POSTransSignLog.Count > 0)
                                        {
                                            PatInfo.IDType = GetIDType(oTransSignLogData.POSTransSignLog[0].CustomerIDType);
                                        }
                                        if (PatInfo.IDType.ToString().Trim() == string.Empty)
                                            PatInfo.IDType = NplexBL.PatientInfo.IDTypes.DL_ID;

                                        PatInfo.IDIssuingAgency = Configuration.convertNullToString(oCustomerData.Customer[0].State);
                                        PatInfo.LastName = Configuration.convertNullToString(oCustomerData.Customer[0].CustomerName);
                                        PatInfo.FirstName = Configuration.convertNullToString(oCustomerData.Customer[0].FirstName);

                                        #region PRIMEPOS-2729 06-Sep-2019 JY Added
                                        DateTime result = DateTime.Now;
                                        if (Configuration.convertNullToString(Configuration.strDOB) != "")
                                        {
                                            String format = "MMddyyyy";
                                            try
                                            {
                                                result = DateTime.ParseExact(Configuration.strDOB, format, System.Globalization.CultureInfo.InvariantCulture);
                                            }
                                            catch (Exception Ex1) { }
                                            PatInfo.DateOfBirth = result.ToShortDateString();
                                        }
                                        else
                                        {
                                            if (Configuration.convertNullToString(oCustomerData.Customer[0].DateOfBirth) != "")
                                                PatInfo.DateOfBirth = Convert.ToDateTime(oCustomerData.Customer[0].DateOfBirth).ToShortDateString();
                                        }
                                        #endregion

                                        PatInfo.Address1 = Configuration.convertNullToString(oCustomerData.Customer[0].Address1);
                                        PatInfo.PostalCode = Configuration.convertNullToString(oCustomerData.Customer[0].Zip);
                                        PatInfo.State = Configuration.convertNullToString(oCustomerData.Customer[0].State);    //required for PO
                                        PatInfo.Address2 = Configuration.convertNullToString(oCustomerData.Customer[0].Address2);    //not mandatory
                                        PatInfo.City = Configuration.convertNullToString(oCustomerData.Customer[0].City);    //not mandatory
                                    }

                                    //get item
                                    if (bPSEItemInTable == true)
                                    {
                                        foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                                        {
                                            DataTable dtPSE_Items = oItem.IsPSEItemData(oRow.ItemID);
                                            if (dtPSE_Items != null && dtPSE_Items.Rows.Count > 0)
                                            {
                                                oIData = oItem.Populate(oRow.ItemID.Trim());
                                                ProductType item = new ProductType();

                                                item.upc = oIData.Item[0].ItemID;
                                                item.name = oIData.Item[0].Description;
                                                item.grams = Convert.ToSingle(dtPSE_Items.Rows[0]["ProductGrams"]);
                                                if ((int)dtPSE_Items.Rows[0]["ProductPillCnt"] > 0)
                                                {
                                                    item.pills = (int)dtPSE_Items.Rows[0]["ProductPillCnt"];
                                                }
                                                item.boxCount = Math.Abs(oRow.QTY);
                                                oNplex.MethItem.Add(item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                                        {
                                            Boolean? isSudafed = oTransDetailSvr.IsSudaFedItem(oRow.ItemID);
                                            if (isSudafed == true)
                                            {
                                                oIData = oItem.Populate(oRow.ItemID.Trim());
                                                ProductType item = new ProductType();

                                                item.upc = oIData.Item[0].ItemID;
                                                item.name = oIData.Item[0].Description;
                                                item.grams = (float)oTransDetailSvr.GetSudafedUnits(oRow.ItemID);
                                                item.boxCount = Math.Abs(oRow.QTY);
                                                oNplex.MethItem.Add(item);
                                            }
                                        }
                                    }

                                    //doreturn
                                    string strErrorMsg = string.Empty;
                                    ReturnResponseType retResponse = oNplex.DoReturn(PatInfo, ref strErrorMsg);    //PRIMEPOS-2999 10-Sep-2021 JY Added strErrorMsg
                                    if (retResponse.trxStatus != null && retResponse.trxStatus.resultCode == 0)
                                    {
                                        //return succedd - LOG THE MESSAGE
                                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist-Purchase return success", clsPOSDBConstants.Log_Success);
                                        logger.Trace("Persist() - nPlex Purchase return " + " - " + clsPOSDBConstants.Log_Success);
                                    }
                                    else
                                    {
                                        //return failed
                                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist-Purchase return error" + retResponse.trxStatus.errorMsg, clsPOSDBConstants.Log_Exception_Occured);
                                        if (retResponse.trxStatus != null && Configuration.convertNullToString(retResponse.trxStatus.errorMsg) != "")
                                            logger.Trace("Persist() - nPlex Purchase return " + " - " + retResponse.trxStatus.errorMsg + " - " + clsPOSDBConstants.Log_Exception_Occured);
                                        else
                                            logger.Trace("Persist() - nPlex Purchase return " + " - " + strErrorMsg + " - " + clsPOSDBConstants.Log_Exception_Occured);
                                    }
                                }
                                logger.Trace("Persist() - logic to return nplex done");
                            }
                            #endregion
                            else if (this.CurrentTransactionType == POSTransactionType.Sales)
                            {
                                try
                                {
                                    if (pseTrxId.Trim() != "")
                                    {
                                        logger.Trace("Persist() - delete NplexRecovery record start");
                                        NplexBL oNplexBL = new NplexBL();
                                        oNplexBL.DeleteNplexRecovery(pseTrxId);
                                        logger.Trace("Persist() - delete NplexRecovery record end");
                                    }
                                }
                                catch (Exception Ex)
                                {
                                    logger.Fatal(Ex, "Persist() - delete NplexRecovery record");
                                }
                            }
                        }
                    }
                    catch (Exception Ex1)
                    {
                        logger.Fatal(Ex1, "Persist() - nplex");
                        //throw (Ex1);
                    }
                    #endregion

                    if (oCLCardsRow != null && (oTransHData.TransHeader[0].TransType == 1 || oTransHData.TransHeader[0].TransType == 2) && Configuration.CLoyaltyInfo.UseCustomerLoyalty == true)
                    {
                        CLCouponsSvr clcoup = new CLCouponsSvr();
                        //CLCouponsData clData = clcoup.GetByCLCardID(oCLCardsRow.CLCardID);    //Sprint-19 - 01-Apr-2015 JY Commented
                        CLCouponsData clData = clcoup.GetByCLCardID(oCLCardsRow.CLCardID, "PosTrans");  //Sprint-19 - 01-Apr-2015 JY Added 
                        try
                        {
                            if (clData.CLCoupons.Rows.Count > 0)
                            {
                                newClValue = Configuration.convertNullToDecimal(clData.CLCoupons.Rows[0][clsPOSDBConstants.CLCoupons_Fld_CouponValue]);
                                NewClId = Configuration.convertNullToInt(clData.CLCoupons.Rows[0][clsPOSDBConstants.CLCoupons_Fld_ID]); //Sprint-25 - PRIMEPOS-2297 28-Feb-2017 JY Added
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    #region Sprint-25 - PRIMEPOS-2297 28-Feb-2017 JY Added
                    if (NewClId != oldClId && newClValue > 0)
                    {
                        isCLTierreached = true;
                    }
                    #endregion

                    #region Sprint-25 - PRIMEPOS-2297 28-Feb-2017 JY Commented
                    //if (newClValue>0 && newClValue != oldClValue)
                    //{
                    //    isCLTierreached=true;
                    //}
                    #endregion
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Commit Transaction ");
                    logger.Trace("Persist() - Commit Transaction");
                }
                else
                {
                    ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_UnableToSaveData);
                }
                CLCouponValue = newClValue; //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist() - Catch block Start");  //PRIMEPOS-2848 21-May-2020 JY Added
                #region PRIMEPOS-2761 - NileshJ
                RxTransRollback(dsOrgRxData);
                logger.Trace("RxTransRollback(DataSet dsOriginalRxData) - " + clsPOSDBConstants.Log_Exiting);
                #endregion

                if (oTrans != null)
                    oTrans.Rollback();

                #region Sprint-23 - PRIMEPOS-2029 06-Apr-2016 JY Added to return to nplex
                if (pseTrxId != string.Empty)
                {
                    NplexBL oNplex = new NplexBL();
                    VoidResponseType voidResponse = oNplex.DoVoid(pseTrxId);
                    if (voidResponse.trxStatus.resultCode == 0)
                    {
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist-Void request success", clsPOSDBConstants.Log_Success);
                        logger.Trace("Persist() - nPlex Void request success for Purchase TransID: " + pseTrxId);
                    }
                    else
                    {
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist-Void request error-" + voidResponse.trxStatus.errorMsg, clsPOSDBConstants.Log_Exception_Occured);
                        logger.Trace("Persist() - nPlex Void request error for Purchase TransID: " + pseTrxId + " - " + voidResponse.trxStatus.errorMsg);
                    }
                }
                #endregion
                #region PRIMEPOS-2761
                if (oPOSTransPaymentData.Tables[0].Rows.Count > 0)
                {
                    reverseCCTransaction();
                    UpdateCcTransmissionLog(oTransPData, true);//IsReversal
                }
                #endregion
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Rollback Transaction ");
                logger.Fatal(ex, "Persist() - Catch block End");
                throw (ex);
            }
            #region PRIMEPOS-2761
            try
            {
                logger.Trace("Persist() - RxTrans-Start");
                if (oRXDetailData.Tables.Count > 0)
                {
                    if (oRXDetailData.Tables[0].Rows.Count > 0)
                    {

                        //if (clsCoreUIHelper.DisplayYesNo("Are your sure, you want to Close ?", "Close Application Temp Screen", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    //Application.Exit();
                        //    Environment.Exit(1);
                        //}
                        //else
                        //{
                        DataSet dsRxTransLog = new DataSet();
                        DataTable dtRxPrescConsentLog = new DataTable(); //PRIMEPOS-3192
                        logger.Trace("Persist() - RxTransactionPopulate(oTransDData.TransDetail[0].TransID) - Start");
                        dsRxTransLog = RxTransactionPopulate(oTransDData.TransDetail[0].TransID);
                        logger.Trace("Persist() - RxTransactionPopulate(oTransDData.TransDetail[0].TransID) - End");

                        logger.Trace("Persist() - RxPrescConsentPopulate() - Start");
                        dtRxPrescConsentLog = RxPrescConsentPopulate(); //PRIMEPOS-3192
                        logger.Trace("Persist() - RxPrescConsentPopulate() - End");

                        logger.Trace("Persist() - bool isSuccess = UpdatePrimeRXData(dsRxTransLog); - Start");
                        if (dsRxTransLog != null && dsRxTransLog.Tables.Count > 0 && RxData != null)
                            dsRxTransLog.Tables.Add(RxData);
                        bool isSuccess = UpdatePrimeRXData(dsRxTransLog, oRXDetailData.TransDetailRX, oTransHData.TransHeader[0].IsDelivery, lstOnHoldRxs, dtRxPrescConsentLog);    //PRIMEPOS-3008 30-Sep-2021 JY Added oRXDetailData.TransDetailRX, IsDelivery, lstOnHoldRxs //PRIMEPOS-3192 Added dtRxPrescConsentLog
                        logger.Trace("Persist() - bool isSuccess = UpdatePrimeRXData(dsRxTransLog); - End");
                        //}
                    }
                }
                if (oTransPData.POSTransPayment.Rows.Count > 0)
                {
                    logger.Trace("Persist() -  UpdateCcTransmissionLog(oTransPData) - Start");
                    UpdateCcTransmissionLog(oTransPData);
                    logger.Trace("Persist() -  UpdateCcTransmissionLog(oTransPData) - End");
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex.StackTrace, "Sync");
                logger.Fatal(ex, "Sync");
            }
            #endregion
            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("Persist() - " + clsPOSDBConstants.Log_Exiting);
        }

        //2915 
        public void Persist(TransHeaderData oTransHData, TransDetailData oTransDData, POSTransPaymentData oTransPData, Int32 onHoldTransID,
            bool isReceiveOnAccount, RXHeaderList oRXInfoList,
            TransDetailRXData oRXDetailData, TransDetailTaxData oTDTaxData)
        {
            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()" + oTransHData.TransHeader[0].TransType.ToString(), clsPOSDBConstants.Log_Entering);
            logger.Trace("Persist() - " + clsPOSDBConstants.Log_Entering);
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;

            try
            {
                Customer oCustomer = new Customer();
                CustomerData oCustomerData = oCustomer.GetCustomerByID(oTransHData.TransHeader[0].CustomerID);

                TransHeaderSvr oTransHeaderSvr = new TransHeaderSvr();
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();
                POSTransPaymentSvr oTransPaymentSvr = new POSTransPaymentSvr();

                #region Sprint-23 - PRIMEPOS-2029 15-Apr-2016 JY Added logic to return nplex
                try
                {
                    logger.Trace("Persist() - Added logic to return nplex");
                    bool bPSEItemInTable = false;
                    if ((this.CurrentTransactionType == POSTransactionType.SalesReturn) && (Configuration.CInfo.useNplex == true))
                    {
                        Boolean ePSE = false;
                        Item oItem = new Item();
                        #region Sprint-25 - PRIMEPOS-2380 14-Feb-2017 JY Added logic to check Item exists in PSE_Items table
                        foreach (DataRow row in oTransDData.Tables[0].Rows)
                        {
                            DataTable dtPSE_Items = oItem.IsPSEItemData(row["ITEMID"].ToString());
                            if (dtPSE_Items != null && dtPSE_Items.Rows.Count > 0)
                            {
                                ePSE = true;
                                bPSEItemInTable = true;
                                break;
                            }
                        }
                        #endregion

                        if (bPSEItemInTable == false)
                        {
                            foreach (DataRow row in oTransDData.Tables[0].Rows)
                            {
                                Boolean? result = oTransDetailSvr.IsSudaFedItem(row["ITEMID"].ToString());

                                if (result == true)
                                {
                                    ItemData oIData = oItem.Populate(row["ITEMID"].ToString());
                                    if (oIData != null && oIData.Item != null)
                                    {
                                        if (oIData.Item.Rows.Count > 0)
                                        {
                                            if (oIData.Item[0].isOTCItem == true)
                                            {
                                                ePSE = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (ePSE == true)
                        {
                            NplexBL oNplex = new NplexBL();
                            NplexBL.PatientInfo PatInfo = new NplexBL.PatientInfo();
                            ItemData oIData = null;
                            oNplex.MethItem.Clear();

                            //get customer
                            if (oCustomerData.Customer.Count > 0)
                            {
                                PatInfo.ID = oCustomerData.Customer[0].DriveLicNo;
                                try
                                {
                                    PatInfo.ID = oTransSignLogData.POSTransSignLog[0].CustomerIDDetail.Substring(0, oTransSignLogData.POSTransSignLog[0].CustomerIDDetail.IndexOf('^'));
                                }
                                catch
                                {
                                    PatInfo.ID = Configuration.AuthorizationNo; //12-Sep-2016 JY Added to preserve ID 
                                }
                                if (oTransSignLogData.POSTransSignLog.Count > 0)
                                {
                                    PatInfo.IDType = GetIDType(oTransSignLogData.POSTransSignLog[0].CustomerIDType);
                                }
                                if (PatInfo.IDType.ToString().Trim() == string.Empty)
                                    PatInfo.IDType = NplexBL.PatientInfo.IDTypes.DL_ID;

                                PatInfo.IDIssuingAgency = Configuration.convertNullToString(oCustomerData.Customer[0].State);
                                PatInfo.LastName = Configuration.convertNullToString(oCustomerData.Customer[0].CustomerName);
                                PatInfo.FirstName = Configuration.convertNullToString(oCustomerData.Customer[0].FirstName);

                                #region PRIMEPOS-2729 06-Sep-2019 JY Added
                                DateTime result = DateTime.Now;
                                if (Configuration.convertNullToString(Configuration.strDOB) != "")
                                {
                                    String format = "MMddyyyy";
                                    try
                                    {
                                        result = DateTime.ParseExact(Configuration.strDOB, format, System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch (Exception Ex1) { }
                                    PatInfo.DateOfBirth = result.ToShortDateString();
                                }
                                else
                                {
                                    if (Configuration.convertNullToString(oCustomerData.Customer[0].DateOfBirth) != "")
                                        PatInfo.DateOfBirth = Convert.ToDateTime(oCustomerData.Customer[0].DateOfBirth).ToShortDateString();
                                }
                                #endregion

                                PatInfo.Address1 = Configuration.convertNullToString(oCustomerData.Customer[0].Address1);
                                PatInfo.PostalCode = Configuration.convertNullToString(oCustomerData.Customer[0].Zip);
                                PatInfo.State = Configuration.convertNullToString(oCustomerData.Customer[0].State);    //required for PO
                                PatInfo.Address2 = Configuration.convertNullToString(oCustomerData.Customer[0].Address2);    //not mandatory
                                PatInfo.City = Configuration.convertNullToString(oCustomerData.Customer[0].City);    //not mandatory
                            }

                            //get item
                            if (bPSEItemInTable == true)
                            {
                                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                                {
                                    DataTable dtPSE_Items = oItem.IsPSEItemData(oRow.ItemID);
                                    if (dtPSE_Items != null && dtPSE_Items.Rows.Count > 0)
                                    {
                                        oIData = oItem.Populate(oRow.ItemID.Trim());
                                        ProductType item = new ProductType();

                                        item.upc = oIData.Item[0].ItemID;
                                        item.name = oIData.Item[0].Description;
                                        item.grams = Convert.ToSingle(dtPSE_Items.Rows[0]["ProductGrams"]);
                                        if ((int)dtPSE_Items.Rows[0]["ProductPillCnt"] > 0)
                                        {
                                            item.pills = (int)dtPSE_Items.Rows[0]["ProductPillCnt"];
                                        }
                                        item.boxCount = Math.Abs(oRow.QTY);
                                        oNplex.MethItem.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                                {
                                    Boolean? isSudafed = oTransDetailSvr.IsSudaFedItem(oRow.ItemID);
                                    if (isSudafed == true)
                                    {
                                        oIData = oItem.Populate(oRow.ItemID.Trim());
                                        ProductType item = new ProductType();

                                        item.upc = oIData.Item[0].ItemID;
                                        item.name = oIData.Item[0].Description;
                                        item.grams = (float)oTransDetailSvr.GetSudafedUnits(oRow.ItemID);
                                        item.boxCount = Math.Abs(oRow.QTY);
                                        oNplex.MethItem.Add(item);
                                    }
                                }
                            }

                            //doreturn
                            string strErrorMsg = string.Empty;
                            ReturnResponseType retResponse = oNplex.DoReturn(PatInfo, ref strErrorMsg); //PRIMEPOS-2999 10-Sep-2021 JY Added
                            if (retResponse.trxStatus != null && retResponse.trxStatus.resultCode == 0)
                            {
                                //return succedd - LOG THE MESSAGE
                                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist-Purchase return success", clsPOSDBConstants.Log_Success);
                                logger.Trace("Persist() - Purchase return " + " - " + clsPOSDBConstants.Log_Success);
                            }
                            else
                            {
                                //return failed
                                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist-Purchase return error" + retResponse.trxStatus.errorMsg, clsPOSDBConstants.Log_Exception_Occured);
                                if (retResponse.trxStatus != null && Configuration.convertNullToString(retResponse.trxStatus.errorMsg) != "")
                                    logger.Trace("Persist() - Purchase return" + " - " + retResponse.trxStatus.errorMsg + " - " + clsPOSDBConstants.Log_Exception_Occured);
                                else
                                    logger.Trace("Persist() - Purchase return" + " - " + strErrorMsg + " - " + clsPOSDBConstants.Log_Exception_Occured);
                            }
                        }
                    }
                    logger.Trace("Persist() - logic to return nplex done");
                }
                catch (Exception Ex1)
                {
                    logger.Fatal(Ex1, "Persist() - Added logic to return nplex");
                    throw (Ex1);
                }
                #endregion

                #region Sprint-23 - PRIMEPOS-2029 20-Apr-2016 JY Added logic to save item monitoring transaction details
                ItemMonitorTransDetailData oItemMonitorTransDetailData = new ItemMonitorTransDetailData();
                #endregion

                logger.Trace("Persist() - Initialize connection Object with : " + Configuration.convertNullToString(Configuration.ServerName) + " - " + Configuration.convertNullToString(Configuration.DatabaseName));
                oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
                logger.Trace("Persist() - Completed creating connection Object with : " + Configuration.convertNullToString(Configuration.ServerName) + " - " + Configuration.convertNullToString(Configuration.DatabaseName));

                logger.Trace("Persist() - Begin Transaction");
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                System.Int32 TransID = 0;

                if (onHoldTransID > 0)
                {
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize DeleteOnHoldRow Started ");
                    logger.Trace("Persist() - Initialize DeleteOnHoldRow Started");
                    oTransHeaderSvr.DeleteOnHoldRows(oTrans, onHoldTransID);
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed DeleteOnHoldRow  ");
                    logger.Trace("Persist() - Completed DeleteOnHoldRow");
                }
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize Writing data to POSTransaction Table ");

                #region PRIMEPOS-2639 27-Mar-2019 JY Added logic to delete scanned on hold rx data
                //if (lstOnHoldRxs.Count > 0)
                //{
                //    oTransHeaderSvr.DeleteOnHoldRows(oTrans, lstOnHoldRxs);
                //}
                #endregion

                logger.Trace("Persist() - Initialize Writing data to POSTransaction Table");
                oTransHeaderSvr.Persist(oTransHData, oTrans, ref TransID);
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Completed Writing data to POSTransaction Table  ");
                logger.Trace("Persist() - Completed Writing data to POSTransaction Table");
                if (TransID > 0)
                {
                    if (isReceiveOnAccount == false)
                    {
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize Writing data to POSTransactionDetail Table ");
                        logger.Trace("Persist() - Initialize Writing data to POSTransactionDetail Table");
                        oTransDetailSvr.Persist(oTransDData, oTrans, TransID, oRXDetailData, oTDTaxData, oItemMonitorTransDetailData, oTransSignLogData);
                        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed Writing data to POSTransactionDetail Table Completed ");
                        logger.Trace("Persist() - Completed Writing data to POSTransactionDetail Table Completed");

                        //Added By shitaljit 
                        //Run this logic iff transaction has RX items.
                        if (Configuration.isNullOrEmptyDataSet(oRXDetailData) == false)
                        {
                            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize Writing data to POSTransactionRXDetail Table ");
                            logger.Trace("Persist() - Initialize Writing data to POSTransactionRXDetail Table");
                            TransDetailRXSvr oSvr = new TransDetailRXSvr();

                            // added by atul 07-jan-2011
                            for (int i = 0; i < oRXDetailData.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < oRXInfoList.Count; j++)
                                {
                                    for (int k = 0; k < oRXInfoList[j].RXDetails.Count; k++)
                                    {
                                        if (oRXDetailData.Tables[0].Rows[i][clsPOSDBConstants.TransDetailRX_Fld_RXNo].ToString().Trim() == oRXInfoList[j].RXDetails[k].RXNo.ToString().Trim())
                                        {
                                            oRXDetailData.Tables[0].Rows[i][clsPOSDBConstants.TransDetailRX_Fld_CounsellingReq] = oRXInfoList[j].CounselingRequest.ToString();
                                        }
                                    }
                                }
                            }
                            //End of added by atul 07-jan-2011

                            //Added by Manoj 1/7/2015
                            foreach (DataRow detRow in oTransDData.Tables[0].Rows)
                            {
                                if (detRow["ITEMID"].ToString().ToUpper().Trim() != "RX")
                                {
                                    continue;
                                }
                                foreach (DataRow rxDet in oRXDetailData.Tables[0].Rows)
                                {
                                    if (Convert.ToInt32(rxDet["RXDetailID"]) == Convert.ToInt32(detRow["TransDetailID"]) && detRow["ItemDescription"].ToString().Contains(rxDet["RXNO"].ToString()))
                                    {
                                        rxDet["ReturnTransDetailID"] = Configuration.convertNullToInt(detRow["ReturnTransDetailID"]);
                                    }
                                }
                            }

                            oSvr.Persist(oRXDetailData, oTrans);
                            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed Writing data to POSTransactionRXDetail Table ");
                            logger.Trace("Persist() - Completed Writing data to POSTransactionRXDetail Table");
                        }
                    }
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Initialize Writing data to POSTransPayment Table ");
                    logger.Trace("Persist() - Initialize Writing data to POSTransPayment Table");
                    //oTransPaymentSvr.Persist(oTransPData, oTrans, TransID);
                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", "Completed Writing data to POSTransPayment Table ");
                    logger.Trace("Persist() - Completed Writing data to POSTransPayment Table");
                    //bool couponUsedInPayment = false;

                    if (oTransSignLogData != null)
                    {
                        if (oTransSignLogData.Tables[0].Rows.Count > 0)
                        {
                            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Initialize Saving Signature()");
                            logger.Trace("Persist() - Initialize Saving Signature()");
                            POSTransSignLog oPOSTransSignLog = new POSTransSignLog();
                            oPOSTransSignLog.Persist(oTransSignLogData, TransID, oTrans);
                            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Completed Saving Signature()");
                            logger.Trace("Persist() - Completed Saving Signature()");
                        }
                    }
                    //if (this.CurrentTransactionType == POSTransactionType.Sales && Configuration.isNullOrEmptyDataSet(oTDTaxData) == false)   //Sprint-18 - 2142 10-Dec-2014 JY commented as tax computed for sales trans only, it should be for return trans as well
                    if (Configuration.isNullOrEmptyDataSet(oTDTaxData) == false)    //Sprint-18 - 2142 10-Dec-2014 JY Added  
                    {
                        TransDetailTaxSvr oTDTaxSvr = new TransDetailTaxSvr();
                        oTDTaxSvr.Persist(oTDTaxData, oTrans, TransID);
                    }
                    if (Configuration.isNullOrEmptyDataSet(oTransPData) == false)//2915
                    {
                        oTransPaymentSvr.Persist(oTransPData, oTrans, TransID);
                    }

                    oTransHData.TransHeader[0].TransID = TransID;
                    oTransHData.AcceptChanges();
                    oTransDData.AcceptChanges();
                    oTransPData.AcceptChanges();
                    #region  PRIMEPOS-2761 - NileshJ
                    if (oRXInfoList != null)
                    {
                        dsOrgRxData = FetchRxDetails(oRXInfoList);

                        if (oRXInfoList.Count > 0)
                        {
                            bool RxtxnSuccess = false;
                            logger.Trace("Persist() - Initialize Updating RX picked up Details");
                            //if (oTransHData.TransHeader[0].TransType == 2) // Return
                            //{
                            //    RxtxnSuccess = UpdateRxTransDataLocally(oRXInfoList, true, oTransHData.TransHeader[0].IsDelivery, oTrans, oTransDData.TransDetail[0].TransID, isBatchDelivery);
                            //}
                            //else // Sale
                            //{
                            //    RxtxnSuccess = UpdateRxTransDataLocally(oRXInfoList, false, oTransHData.TransHeader[0].IsDelivery, oTrans, oTransDData.TransDetail[0].TransID, isBatchDelivery);
                            //}
                            logger.Trace("Persist() - Completed Updating RX picked up Details");
                            if (!RxtxnSuccess)
                            {
                                RxTransRollback(dsOrgRxData);
                                logger.Trace("RxTransRollback(DataSet dsOriginalRxData) - " + clsPOSDBConstants.Log_Exiting);
                                //UpdateCcTransmissionLog(oTransPData , true);//IsReversal
                                return;
                            }
                        }
                    }


                    #endregion
                    oTrans.Commit();

                    logger.Trace("Persist() - Commit Transaction");
                }
                else
                {
                    ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_UnableToSaveData);
                }
                //CLCouponValue = newClValue; //Sprint-18 - 2039 12-Jan-2015 JY Added to preserve active coupon value
            }
            catch (Exception ex)
            {
                #region PRIMEPOS-2761 - NileshJ
                RxTransRollback(dsOrgRxData);
                logger.Trace("RxTransRollback(DataSet dsOriginalRxData) - " + clsPOSDBConstants.Log_Exiting);
                #endregion

                if (oTrans != null)
                    oTrans.Rollback();

                #region Sprint-23 - PRIMEPOS-2029 06-Apr-2016 JY Added to return to nplex
                //if (pseTrxId != string.Empty)
                //{
                //    Business_Tier.NplexBL oNplex = new Business_Tier.NplexBL();
                //    VoidResponseType voidResponse = oNplex.DoVoid(pseTrxId);
                //    if (voidResponse.trxStatus.resultCode == 0)
                //    {
                //        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist-Void request success", clsPOSDBConstants.Log_Success);
                //        logger.Trace("Persist() - Persist-Void request success");
                //    }
                //    else
                //    {
                //        //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist-Void request error-" + voidResponse.trxStatus.errorMsg, clsPOSDBConstants.Log_Exception_Occured);
                //        logger.Trace("Persist() - Void request error - " + voidResponse.trxStatus.errorMsg);
                //    }
                //}
                #endregion
                #region PRIMEPOS-2761
                //if (oPOSTransPaymentData.Tables[0].Rows.Count > 0)
                //{
                //    reverseCCTransaction();
                //    UpdateCcTransmissionLog(oTransPData, true);//IsReversal
                //}
                #endregion
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", " Rollback Transaction ");
                logger.Fatal(ex, "Persist()");
                throw (ex);
            }
            #region PRIMEPOS-2761
            try
            {
                logger.Trace("Persist() - RxTrans-Start");
                if (oRXDetailData.Tables.Count > 0)
                {
                    if (oRXDetailData.Tables[0].Rows.Count > 0)
                    {

                        //if (clsCoreUIHelper.DisplayYesNo("Are your sure, you want to Close ?", "Close Application Temp Screen", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    //Application.Exit();
                        //    Environment.Exit(1);
                        //}
                        //else
                        //{
                        DataSet dsRxTransLog = new DataSet();
                        DataTable dtRxPrescConsentLog = new DataTable(); //PRIMEPOS-3192
                        logger.Trace("Persist() - RxTransactionPopulate(oTransDData.TransDetail[0].TransID) - Start");
                        dsRxTransLog = RxTransactionPopulate(oTransDData.TransDetail[0].TransID);
                        logger.Trace("Persist() - RxTransactionPopulate(oTransDData.TransDetail[0].TransID) - End");

                        logger.Trace("Persist() - RxPrescConsentPopulate() - Start");
                        dtRxPrescConsentLog = RxPrescConsentPopulate(); //PRIMEPOS-3192
                        logger.Trace("Persist() - RxPrescConsentPopulate() - End");

                        logger.Trace("Persist() - bool isSuccess = UpdatePrimeRXData(dsRxTransLog); - Start");
                        bool isSuccess = UpdatePrimeRXData(dsRxTransLog, oRXDetailData.TransDetailRX, oTransHData.TransHeader[0].IsDelivery, null, dtRxPrescConsentLog);    //PRIMEPOS-3008 30-Sep-2021 JY Added oTransDData.TransDetail, IsDelivery //PRIMEPOS-3192 Added dtRxPrescConsentLog
                        logger.Trace("Persist() - bool isSuccess = UpdatePrimeRXData(dsRxTransLog); - End");
                        //}
                    }
                }
                //if (oTransPData.POSTransPayment.Rows.Count > 0)
                //{
                //    logger.Trace("Persist() -  UpdateCcTransmissionLog(oTransPData) - Start");
                //    UpdateCcTransmissionLog(oTransPData);
                //    logger.Trace("Persist() -  UpdateCcTransmissionLog(oTransPData) - End");
                //}

            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace, "Sync");
            }
            #endregion
            //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "Persist()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("Persist() - " + clsPOSDBConstants.Log_Exiting);
        }

        //Added by SRT (Sachin) Date : 06 March 2010
        //Added TransID parameter by shitaljit on 3 arpil 2012
        public bool RxIsOnHold(string RXNumber, string PatientNo, out string StationID, out string UserID, out string TransID)
        {
            System.Data.IDbTransaction oTrans = null;
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                TransDetailRXSvr oSvr = new TransDetailRXSvr();
                return (oSvr.RxIsOnHold(RXNumber, PatientNo, out StationID, out UserID, out TransID, oTrans));
            }
            catch (Exception ex)
            {
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "RxIsOnHold()", ex.ToString());
                logger.Fatal(ex, "RxIsOnHold(string RXNumber, string PatientNo, out string StationID, out string UserID, out string TransID)");
                if (oTrans != null)
                    oTrans.Rollback();
                throw (ex);
            }
        }
        //End of Added by SRT (Sachin) Date : 06 March 2010

        #region Sprint-21 04-Feb-2016 JY added code to fix - SCENARIO: If you put some rxs for a patient on hold. Then if you enter a different rx for the same patient on the pos the unpicked rx search feature has some problem.
        public bool RxIsOnHold(string PatientNo, out DataTable dtRxOnHold)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);

                TransDetailRXSvr oSvr = new TransDetailRXSvr();
                return (oSvr.RxIsOnHold(PatientNo, out dtRxOnHold, oConn));
            }
            catch (Exception ex)
            {
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "RxIsOnHold()", ex.ToString());
                logger.Fatal(ex, "RxIsOnHold(string PatientNo, out DataTable dtRxOnHold)");
                throw (ex);
            }
        }
        #endregion

        #region PRIMEPOS-3248
        public bool RxIsOnHoldForPrimeRxPayment(string PatientNo, out DataTable dtRxOnHoldForPrimeRxPayment)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);

                TransDetailRXSvr oSvr = new TransDetailRXSvr();
                return (oSvr.RxIsOnHoldForPrimeRxPayment(PatientNo, out dtRxOnHoldForPrimeRxPayment, oConn));
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "RxIsOnHoldForPrimeRxPayment(string PatientNo, out DataTable dtRxOnHold)");
                throw (ex);
            }
        }
        #endregion

        public void PutOnHold(TransHeaderData oTransHData, TransDetailData oTransDData, TransDetailRXData oRXDetailData, TransDetailTaxData oTDTaxData, POSTransPaymentData oPOSTransPaymentData = null)//2915
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            try
            {
                TransHeaderSvr oTransHeaderSvr = new TransHeaderSvr();
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();

                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction();

                System.Int32 TransID = 0;
                oTransHeaderSvr.PutOnHold(oTransHData, oTrans, ref TransID);
                if (TransID > 0)
                {
                    oTransDetailSvr.PutOnHold(oTransDData, oTrans, TransID, oRXDetailData, oTDTaxData);
                    TransDetailRXSvr oSvr = new TransDetailRXSvr();
                    if (oPOSTransPaymentData != null)//2915
                    {
                        POSTransPaymentSvr oTansPayment_Onholdsvr = new POSTransPaymentSvr();
                        oTansPayment_Onholdsvr.PutOnHold(oPOSTransPaymentData, oTrans, TransID);
                    }
                    //oSvr.Persist(oRXDetailData,oTrans); //cmt by SRT (Abhishek D) Date 22 March 2010
                    oTransHData.TransHeader[0].TransID = TransID;
                    oTransHData.AcceptChanges();
                    oTrans.Commit();
                }
                else
                {
                    ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_UnableToSaveData);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PutOnHold()");
                if (oTrans != null)
                    oTrans.Rollback();
                throw (ex);
            }
        }
        #region Validation for Maximum Limit % Discount user can give
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool ValidateUserDicountLimit(decimal DicsValueToVerify, out decimal InvDicsValueToVerify)
        {
            InvDicsValueToVerify = 0;
            bool RetVal = false;

            try
            {
                //Added By shitaljit on 2/11/2014 for JIRA PRIMEPOS-1810
                if (Configuration.IsDiscOverridefromPOSTrans == true)
                {
                    return true;
                }//END
                // UserManagement.clsLogin oLogin = new POS_Core.UserManagement.clsLogin();
                string sUserID = "";
                if (DicsValueToVerify > Configuration.UserMaxDiscountLimit)
                {
                    // frmPOSTransaction.InvDicsValueToVerify = DicsValueToVerify;
                    if (clsCoreLogin.loginForPreviliges(clsPOSDBConstants.UserMaxDiscountLimit, "", out sUserID, "Security Override For User Discount Limit") == false)
                    {
                        RetVal = false;
                    }
                    else
                    {
                        RetVal = true;
                    }
                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception Ex)
            {
                RetVal = false;
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "ValidateUserDiscountLimit()", Ex.ToString());
                logger.Fatal(Ex, "ValidateUserDiscountLimit()");
                throw Ex;
            }
            return RetVal;
        }
        #endregion Validation for Maximum Limit % Discount user can give
        public void RemoveOnHoldTrans(int TransID, bool IsDeletePayOnHold = true)//PRIMEPOS-2915
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            try
            {
                TransHeaderSvr oTransHeaderSvr = new TransHeaderSvr();
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();

                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                oTransHeaderSvr.DeleteOnHoldRows(oTrans, TransID, IsDeletePayOnHold);//PRIMEPOS-2915
                oTrans.Commit();
                oTrans.Dispose();
            }
            catch (Exception ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "RemoveOnHoldTrans()", ex.ToString());
                logger.Fatal(ex, "RemoveOnHoldTrans()");
                throw (ex);
            }
        }

        //Added by Manoj 1/23/2013
        public bool AllowPickedUpRX(string IsPosPickup, string IsPickUp, string sRXNo, string nRefill, out bool isAlreadyProcessedInPOS, int iPartialFillNo = 0)
        {
            bool isAllow = false;
            isAlreadyProcessedInPOS = POSTransaction.IsRXAlreadyProcessedInPOS(sRXNo, nRefill, iPartialFillNo);
            #region Comented code 


            //if (Configuration.CPOSSet.AllowPickedUpRxToTrans && IsPosPickup.ToUpper().Trim() == "Y" && isAlreadyProcessedInPOS == true)
            //{
            //    isAllow = false;
            //}
            //else if(Configuration.CPOSSet.AllowPickedUpRxToTrans && IsPickUp.ToUpper().Trim() == "Y" && (IsPosPickup.ToUpper().Trim() == "N" || String.IsNullOrEmpty(IsPosPickup.Trim())))
            //{
            //    isAllow = true;
            //}
            //else if((IsPickUp.ToUpper().Trim() == "N" || string.IsNullOrEmpty(IsPickUp.Trim())) && (IsPosPickup.ToUpper().Trim() == "N" || string.IsNullOrEmpty(IsPosPickup.Trim())))
            //{
            //    isAllow = false;
            //    isPosAllow = true;
            //}
            //else if(!Configuration.CPOSSet.AllowPickedUpRxToTrans && IsPosPickup.ToUpper().Trim() == "Y")
            //{
            //    isPosAllow = true;
            //    isAllow = false;
            //}
            //else if(!Configuration.CPOSSet.AllowPickedUpRxToTrans && IsPickUp.ToUpper().Trim() == "Y")
            //{
            //    isPosAllow = false;
            //    isAllow = false;
            //}
            //else
            //{
            //    isAllow = true;
            //}
            #endregion
            PickupRx = IsPickUp.Trim();
            AllowESCRxPickedUp = isAllow = (!isAlreadyProcessedInPOS && Configuration.CPOSSet.AllowPickedUpRxToTrans);
            IsPOSAllowRX = ((Configuration.AllowRxPicked > 0) && IsPosPickup.ToUpper().Trim() != "Y") ? true : false;   //PRIMEPOS-2865 16-Jul-2020 JY modified
            return isAllow;
        }

        /// <summary>
        /// Author: Shitaljit
        /// Last Updated Date: 2/27/2014
        /// To check whether RX is already preocess in POS.
        /// This is to block PH manually changing the Pickup and Puck up POS to reprocess RX in POS.
        /// </summary>
        /// <param name="oRxInfo"></param>
        /// <returns></returns>
        public static bool IsRXAlreadyProcessedInPOS(string sRXNo, string nRefill, int iPartialFillNo = 0)
        {
            bool RetVal = false;
            try
            {
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "PickedUpRxTransDetails()", clsPOSDBConstants.Log_Entering);
                logger.Trace("IsRXAlreadyProcessedInPOS() - " + clsPOSDBConstants.Log_Entering);
                string PaymentType = string.Empty;
                TransDetailRXSvr oSvr = new TransDetailRXSvr();
                DataSet DsPOSTransactionRXDetail = null;
                //int RefillNo = 0;- not be used
                //string sPickupPOS = string.Empty; - not be used
                //string sPickupRX = string.Empty;  - not be used
                //PharmBL oPharmBL = new PharmBL(); no need to go to PrimeRx db get RefillNo which just retrieved from there
                DataTable oRxInfo = null;
                //ErrorLogging.Logs.Logger("frmPPOSTransaction", "ISrXAlreadyProcessedInPOS()", "About to call PharmSQL");
                /*logger.Trace("IsRXAlreadyProcessedInPOS() - About to call PharmSQL");
                if (iPartialFillNo == 0)
                    oRxInfo = oPharmBL.GetRxsWithStatus(sRXNo, nRefill, ""); // no partial fill
                else
                    oRxInfo = oPharmBL.GetRxsWithStatus(sRXNo, nRefill, "", iPartialFillNo); */
                //ErrorLogging.Logs.Logger("frmPPOSTransaction", "ISrXAlreadyProcessedInPOS()", "Call PharmSQL Successful");
                //logger.Trace("IsRXAlreadyProcessedInPOS() - Call PharmSQL Successful");
                if (Configuration.isNullOrEmptyDataTable(oRxInfo) == true)
                {
                    //RefillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["nrefill"]);
                    //sPickupPOS = Configuration.convertNullToString(oRxInfo.Rows[0]["PickupPOS"]); - not be used
                    //sPickupRX = Configuration.convertNullToString(oRxInfo.Rows[0]["Pickedup"]);   - not be used
                    string sSQL = @"select rxno,nrefill,pt.transid,transtype,PartialFillNo from postransaction pt inner join POSTransactionDetail ptd
                                    on pt.TransID = ptd.transid inner join POSTransactionRXDetail prx 
                                    on prx.TransDetailID = ptd.TransDetailID  ";
                    if (iPartialFillNo == 0)
                        sSQL += " WHERE RXNO = " + sRXNo + " AND NREFILL = " + nRefill + " order by TransDate DESC";
                    else
                        sSQL += " WHERE RXNO = " + sRXNo + " AND NREFILL = " + nRefill + " AND prx.PartialFillNo=" + iPartialFillNo.ToString() + " order by TransDate DESC";
                    SearchSvr oSerSvr = new SearchSvr();

                    DsPOSTransactionRXDetail = oSerSvr.Search(sSQL);
                    TransDetailSvr oTransDetailSvr = new TransDetailSvr();
                    string sTransType = string.Empty;
                    if (Configuration.isNullOrEmptyDataSet(DsPOSTransactionRXDetail) == false)
                    {
                        sTransType = Configuration.convertNullToString(DsPOSTransactionRXDetail.Tables[0].Rows[0]["transtype"]);
                        if (sTransType.Trim().Equals("2") == false)
                        {
                            RetVal = true;
                        }

                    }
                    #region Commented Old Logic


                    //for (RowIndex = DsPOSTransactionRXDetail.Tables[0].Rows.Count - 1; RowIndex >= 0; RowIndex--)
                    //{
                    //    DsPOSTransactionDetail = oTransDetailSvr.PopulateTransDetailId(Convert.ToInt32(DsPOSTransactionRXDetail.Tables[0].Rows[RowIndex]["TransDetailID"]));
                    //    if (Configuration.isNullOrEmptyDataSet(DsPOSTransactionDetail) == false)
                    //    {
                    //        if (Configuration.convertNullToInt(DsPOSTransactionDetail.Tables[0].Rows[0][clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId]) == 0 && TransRetDetId != -1 &&
                    //            TransRetDetId !=Configuration.convertNullToInt(DsPOSTransactionDetail.Tables[0].Rows[0][clsPOSDBConstants.TransDetail_Fld_TransDetailID]) &&
                    //            Configuration.convertNullToInt(DsPOSTransactionRXDetail.Tables[0].Rows[RowIndex][clsPOSDBConstants.TransDetailRX_Fld_NRefill]) == Configuration.convertNullToInt(nRefill))
                    //        {
                    //            RetVal = true;
                    //            break;
                    //        }
                    //        else if (Configuration.convertNullToInt(DsPOSTransactionRXDetail.Tables[0].Rows[RowIndex][clsPOSDBConstants.TransDetailRX_Fld_NRefill])
                    //            == Configuration.convertNullToInt(nRefill) &&
                    //            Configuration.convertNullToInt(DsPOSTransactionDetail.Tables[0].Rows[0][clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId]) > 0)
                    //        {
                    //            TransRetDetId = Configuration.convertNullToInt(DsPOSTransactionDetail.Tables[0].Rows[0][clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId]);
                    //        }
                    //        else
                    //        {
                    //            TransRetDetId = -1;
                    //        }
                    //    }
                    //}
                    #endregion
                }
                logger.Trace("IsRXAlreadyProcessedInPOS() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "IsRxAlreadyProcessedInPOS()", Ex.ToString());
                logger.Fatal(Ex, "IsRXAlreadyProcessedInPOS()");
                throw Ex;
            }
            return RetVal;
        }

        //private void UpdateRXData(RXHeaderList ORXInfoList, bool isReturn, bool markAsDelivered, IDbTransaction oTrans, int TransId, bool isBatchDelivery = false) // PRIMERX-7688 - NileshJ - Added isBatchDelivery 23-Sept-2019
        //{
        //    string Pickedup = "";//Added By Rohit Nair on Dec 19 2016 for PrimePOS-2366
        //    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", clsPOSDBConstants.Log_Entering);
        //    logger.Trace("UpdateRXData() - " + clsPOSDBConstants.Log_Entering);
        //    if (ORXInfoList.Count == 0) return;

        //    if (Configuration.CPOSSet.UsePrimeRX == true)
        //    {
        //        string isDelivered = null;
        //        if (markAsDelivered == true)
        //        {
        //            isDelivered = "D";
        //        }
        //        DataTable dt = new DataTable();
        //        PharmBL oPBL = new PharmBL();
        //        foreach (RXHeader oRXHeader in ORXInfoList)
        //        {
        //            #region PRIMEPOS-2442 ADDED BY ROHIT NAIR
        //            //if (isReturn == false && oRXHeader.PatConsent != null && oRXHeader.PatConsent.IsConsentSkip == false)   //Sprint-26 - PRIMEPOS-2442 17-Aug-2017 JY added return flag in condition to bypass consent logic for return
        //            //{
        //            //    try
        //            //    {
        //            //        oPBL.SavePatientConsent(Convert.ToInt32(oRXHeader.PatientNo), oRXHeader.PatConsent.ConsentTextID, oRXHeader.PatConsent.ConsentTypeID, oRXHeader.PatConsent.ConsentStatusID, oRXHeader.PatConsent.ConsentCaptureDate,
        //            //           oRXHeader.PatConsent.ConsentEffectiveDate, oRXHeader.PatConsent.ConsentEndDate, oRXHeader.PatConsent.PatConsentRelationID, oRXHeader.PatConsent.SigneeName, oRXHeader.PatConsent.SignatureData);
        //            //    }
        //            //    catch (Exception ex)
        //            //    {
        //            //        string message = "An error Occured while trying to save Patient Consnet Data " + ex.Message;
        //            //        logger.Fatal(ex, message);
        //            //    }
        //            //}
        //            #endregion

        //            string rxData = "";
        //            foreach (RXDetail objRXInfo in oRXHeader.RXDetails)
        //            {
        //                dt = oPBL.GetRxsWithStatus(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), "");
        //                IsPOSAllowRX = ((Configuration.AllowRxPicked > 0) && dt.Rows[0]["Pickedup"].ToString() != "Y") ? true : false;  //PRIMEPOS-2865 16-Jul-2020 JY modified
        //                if (isReturn == true)
        //                {
        //                    //Prog1 06Sep2009 Change to support specific refillno
        //                    string sError = "";
        //                    //Added By Rohit Nair on Dec 19-2016 for PrimePOS-2366
        //                    Pickedup = dt.Rows[0]["Pickedup"].ToString();

        //                    //oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), null, "N", DateTime.Now, "N", out sError);
        //                    //oPBL.MarkCopayPaid(objRXInfo.RXNo.ToString(), 'N');//Added by shitaljit to Mark CopayPaid to N in case of return transaction.
        //                    //Added by Manoj 1/23/2013
        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Initialize Writing into PharmSQL for MarkDelivery() About to Call PharmSQL ");
        //                    logger.Trace("UpdateRXData() - Initialize Writing into PharmSQL for MarkDelivery() About to Call PharmSQL");

        //                    if (!oRXHeader.IsIntakeBatch || (oRXHeader.IsIntakeBatch && Configuration.CInfo.IntakeBatchMarkAsPickedup))////PrimePOS-2448 Added BY Rohit Nair
        //                    {
        //                        if (!IsPOSAllowRX)
        //                        {
        //                            oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), isDelivered, "N", DateTime.MinValue, "N", out sError, isBatchDelivery);// PRIMERX-7688 - Added isBatchDelivery - NileshJ 23-Sept-2019
        //                        }
        //                        else
        //                        {
        //                            oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), null, Pickedup, DateTime.Now, "N", out sError, isBatchDelivery);// PRIMERX-7688 - Added isBatchDelivery - NileshJ 23-Sept-2019 //PickupRx
        //                        }
        //                    }
        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Completed Writing into PharmSQL for MarkDelivery(), PharmSQL Successful ");
        //                    logger.Trace("UpdateRXData() - Completed Writing into PharmSQL for MarkDelivery(), PharmSQL Successful");

        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Initialize Writing into PharmSQL for MarkCopayPaid() About to call PharmSQL ");
        //                    logger.Trace("UpdateRXData() - Initialize Writing into PharmSQL for MarkCopayPaid() About to call PharmSQL");
        //                    oPBL.MarkCopayPaid(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), 'N');//Added by shitaljit to Mark CopayPaid to N in case of return transaction.
        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Completed Writing into PharmSQL for MarkDelivery(), Call Successful ");
        //                    logger.Trace("UpdateRXData() - Completed Writing into PharmSQL for MarkDelivery(), Call Successful");
        //                }
        //                else
        //                {
        //                    //Prog1 06Sep2009 Change to support specific refillno
        //                    string sError = "";
        //                    //Changed by prog1 28May2009 changes revertedback to old implementation
        //                    //oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), null, "Y", DateTime.Now, "Y", out sError);

        //                    //Added by Manoj 1/23/2013
        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Initialize Writing into PharmSQL for MarkDelivery() ");
        //                    logger.Trace("UpdateRXData() - Initialize Writing into PharmSQL for MarkDelivery()");

        //                    if (!oRXHeader.IsIntakeBatch || (oRXHeader.IsIntakeBatch && Configuration.CInfo.IntakeBatchMarkAsPickedup))//PrimePOS-2448 Added BY Rohit Nair
        //                    {
        //                        if ((Configuration.AllowRxPicked > 0) && IsPOSAllowRX)  //PRIMEPOS-2865 16-Jul-2020 JY modified
        //                        {
        //                            oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), isDelivered, "Y", DateTime.Now, "Y", out sError, isBatchDelivery); // PRIMERX-7688 - NileshJ - Added isBatchDelivery 23-Sept-2019
        //                        }
        //                        else if ((Configuration.AllowRxPicked > 0) && !IsPOSAllowRX)    //PRIMEPOS-2865 16-Jul-2020 JY modified
        //                        {
        //                            //added by Rohit Nair on Dec 19-2016 for PrimePOS-2366
        //                            Pickedup = (string.IsNullOrWhiteSpace(dt.Rows[0]["Pickedup"].ToString()) ? "Y" : dt.Rows[0]["Pickedup"].ToString());
        //                            DateTime pickupdate;
        //                            if (Pickedup.Trim().ToUpper().Equals("Y"))
        //                            {
        //                                try
        //                                {
        //                                    if (dt.Rows[0]["PICKUPDATE"] != DBNull.Value && (Convert.ToDateTime(dt.Rows[0]["PICKUPDATE"].ToString()) >= Convert.ToDateTime(dt.Rows[0]["DATEF"].ToString())))
        //                                    {
        //                                        pickupdate = Convert.ToDateTime(dt.Rows[0]["PICKUPDATE"].ToString());
        //                                    }
        //                                    else
        //                                    {
        //                                        pickupdate = DateTime.Now;
        //                                    }
        //                                }
        //                                catch (Exception expn)
        //                                {
        //                                    pickupdate = DateTime.Now;
        //                                }
        //                                oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), isDelivered, Pickedup, pickupdate, "N", out sError, isBatchDelivery); // PRIMERX-7688 - NileshJ - Added isBatchDelivery 23-Sept-2019
        //                            }
        //                            else
        //                            {
        //                                oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), isDelivered, Pickedup, DateTime.MinValue, "N", out sError, isBatchDelivery); // PRIMERX-7688 - NileshJ - Added isBatchDelivery 23-Sept-2019
        //                            }

        //                        }
        //                        else
        //                        {
        //                            //added by Rohit Nair on Dec 19-2016 for PrimePOS-2366
        //                            Pickedup = dt.Rows[0]["Pickedup"].ToString();
        //                            oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), isDelivered, PickupRx, DateTime.MinValue, "Y", out sError, isBatchDelivery); // PRIMERX-7688 - NileshJ - Added isBatchDelivery 23-Sept-2019
        //                        }
        //                    }


        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Completed Writing into PharmSQL for MarkDelivery() ");
        //                    logger.Trace("UpdateRXData() - Completed Writing into PharmSQL for MarkDelivery() ");

        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Initialize Writing into PharmSQL for MarkCopayPaid() ");
        //                    logger.Trace("UpdateRXData() - Initialize Writing into PharmSQL for MarkCopayPaid()");
        //                    oPBL.MarkCopayPaid(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), 'Y');//Added by shitaljit to Mark CopayPaid to Y in case of sale transaction.
        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Completed Writing into PharmSQL for MarkDelivery()  ");
        //                    logger.Trace("UpdateRXData() - Completed Writing into PharmSQL for MarkDelivery()");

        //                    //Chages By Atul on 02-10-2010 again change by Atul on 12-01-2010
        //                    //if (Configuration.AllowRxPicked)
        //                    //    oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), isDelivered, "Y", DateTime.Now, "Y", out sError);
        //                    //else
        //                    //    oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), null, "N", DateTime.MinValue, "N", out sError);

        //                    /*//Added & Modified by SRT on May-27-09
        //                    //If Rx Approval signature is fetehced from HHP & Payment is done through POS
        //                    // then will update both the fields Pickedup & Pickuppos with the value 'Y' in claims table
        //                    //in the database either PharmSql or BedStuy whichever is pointed.
        //                    if (Configuration.CPOSSet.UseSigPad == true)
        //                    {
        //                        oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), DateTime.Now, "Y");
        //                    }
        //                    else
        //                    {
        //                        //If Rx Approval signature is not fetehced from HHP & Payment is done through POS
        //                        // then will update only the field Pickuppos with the value 'Y' and the value in Pickedup field
        //                        // will remain same as previous value in claims table in the database either PharmSql or BedStuy whichever is pointed.
        //                        //The users who don't have HHPInterface then can use PrimeESC to approve rxs
        //                        DataTable oRxInfo;
        //                        oRxInfo = oPBL.GetRxs(objRXInfo.RXNo.ToString());
        //                        oPBL.MarkDelivery(objRXInfo.RXNo.ToString(), oRxInfo.Rows[0]["Pickedup"].ToString(), DateTime.Now, "Y");
        //                    }
        //                    //Added & Modified Till Here May-27-09*/
        //                }

        //                if (rxData == "")
        //                {
        //                    rxData = objRXInfo.RXNo.ToString() + "-" + objRXInfo.RefillNo;
        //                }
        //                else
        //                {
        //                    rxData += "," + objRXInfo.RXNo.ToString() + "-" + objRXInfo.RefillNo;
        //                }
        //            }
        //            //SaveInsSigBase for saving rx signature
        //            //SavePrivayAck for saving NOPP signature
        //            //if (oRXHeader.NOPPSignature.Trim() != "")
        //            if (oRXHeader.NOPPStatus != null)
        //            {
        //                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Initialize Writing into PharmSQL forSavePrivacyAck() ");
        //                logger.Trace("UpdateRXData() - Initialize Writing into PharmSQL forSavePrivacyAck()");
        //                if ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper()) || Configuration.CPOSSet.IsTouchScreen)    //Sprint-23 - PRIMEPOS-2321 01-Aug-2016 JY Added if clause//3002
        //                    oPBL.SavePrivacyAck(long.Parse(oRXHeader.PatientNo), DateTime.Now, oRXHeader.NOPPStatus, oRXHeader.PrivacyText, oRXHeader.NOPPSignature, "M", oRXHeader.NoppBinarySign);
        //                else
        //                    oPBL.SavePrivacyAck(long.Parse(oRXHeader.PatientNo), DateTime.Now, oRXHeader.NOPPStatus, oRXHeader.PrivacyText, oRXHeader.NOPPSignature, SigPadUtil.DefaultInstance.SigType, oRXHeader.NoppBinarySign);
        //                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Completed Writing into PharmSQL for SavePrivacyAck()");
        //                logger.Trace("UpdateRXData() - Completed Writing into PharmSQL for SavePrivacyAck()");

        //                try
        //                {
        //                    oPBL.SavePatientAck(long.Parse(oRXHeader.PatientNo), oRXHeader.NOPPStatus, DateTime.Now);
        //                }
        //                catch (Exception expn)
        //                {
        //                    string message = string.Format("An error Occured while trying to Update Patient SigAck for Patient {0}  {1}", oRXHeader.PatientNo.ToString(), expn.ToString());
        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", message);
        //                    logger.Trace("UpdateRXData() - " + message);
        //                }


        //            }

        //            #region logic to save data in INSSIGTRANS and TRANSDET table
        //            long sigTransID = 0;
        //            if (oRXHeader.RXSignature.Trim() != "" || oRXHeader.bBinarySign != null)
        //            {
        //                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Initialize Writing into PharmSQL SaveInsSigTrans() ");
        //                logger.Trace("UpdateRXData() - Initialize Writing into PharmSQL SaveInsSigTrans()");
        //                if ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper())  || Configuration.CPOSSet.IsTouchScreen)    //Sprint-23 - PRIMEPOS-2321 01-Aug-2016 JY Added if clause//3002
        //                {
        //                    #region PRIMEPOS -2339 04-Aug-2016 JY Added logic to save InsSigData in POS
        //                    bool bPrivacyAck = false;
        //                    InsSigTransSvr oInsSigTransSvr = new InsSigTransSvr();
        //                    InsSigTransData oInsSigTransData = new InsSigTransData();
        //                    if (oRXHeader.NOPPStatus != null) //then we have PrivacyAck data
        //                    {
        //                        bPrivacyAck = true;
        //                        oInsSigTransData.InsSigTrans.AddRow(0, TransId, int.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, oRXHeader.RXSignature, oRXHeader.CounselingRequest, "M", oRXHeader.bBinarySign,
        //                            oRXHeader.NOPPStatus, oRXHeader.PrivacyText, oRXHeader.NOPPSignature, "M", oRXHeader.NoppBinarySign);
        //                    }
        //                    else
        //                    {
        //                        oInsSigTransData.InsSigTrans.AddRow(0, TransId, int.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, oRXHeader.RXSignature, oRXHeader.CounselingRequest, "M", oRXHeader.bBinarySign);
        //                    }
        //                    oInsSigTransSvr.Persist(oInsSigTransData, bPrivacyAck, oTrans);
        //                    #endregion
        //                    sigTransID = oPBL.SaveInsSigTrans(DateTime.Now, long.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, oRXHeader.RXSignature, oRXHeader.CounselingRequest, "M", oRXHeader.bBinarySign);
        //                }
        //                else
        //                {
        //                    #region PRIMEPOS -2339 04-Aug-2016 JY Added logic to save InsSigData in POS
        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Initialize Writing into POS InsSigTrans table");
        //                    logger.Trace("UpdateRXData() - Initialize Writing into POS InsSigTrans table");
        //                    bool bPrivacyAck = false;
        //                    InsSigTransSvr oInsSigTransSvr = new InsSigTransSvr();
        //                    InsSigTransData oInsSigTransData = new InsSigTransData();
        //                    if (oRXHeader.NOPPStatus != null) //then we have PrivacyAck data
        //                    {
        //                        bPrivacyAck = true;
        //                        oInsSigTransData.InsSigTrans.AddRow(0, TransId, int.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, oRXHeader.RXSignature, oRXHeader.CounselingRequest, SigPadUtil.DefaultInstance.SigType, oRXHeader.bBinarySign,
        //                                                    oRXHeader.NOPPStatus, oRXHeader.PrivacyText, oRXHeader.NOPPSignature, SigPadUtil.DefaultInstance.SigType, oRXHeader.NoppBinarySign);
        //                    }
        //                    else
        //                    {
        //                        oInsSigTransData.InsSigTrans.AddRow(0, TransId, int.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, oRXHeader.RXSignature, oRXHeader.CounselingRequest, SigPadUtil.DefaultInstance.SigType, oRXHeader.bBinarySign);
        //                    }
        //                    oInsSigTransSvr.Persist(oInsSigTransData, bPrivacyAck, oTrans);
        //                    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Completed Writing into POS InsSigTrans table");
        //                    logger.Trace("UpdateRXData() - Completed Writing into POS InsSigTrans table");
        //                    #endregion

        //                    sigTransID = oPBL.SaveInsSigTrans(DateTime.Now, long.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, oRXHeader.RXSignature, oRXHeader.CounselingRequest, SigPadUtil.DefaultInstance.SigType, oRXHeader.bBinarySign);
        //                }
        //                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Completed Writing into PharmSQL for SaveInsSigTrans()");
        //                logger.Trace("UpdateRXData() - Completed Writing into PharmSQL for SaveInsSigTrans()");
        //            }
        //            if (sigTransID > 0)
        //            {
        //                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Initialize Writing into PharmSQL .SaveTransDet() ");
        //                logger.Trace("UpdateRXData() - Initialize Writing into PharmSQL .SaveTransDet()");
        //                foreach (RXDetail objRXInfo in oRXHeader.RXDetails)
        //                {
        //                    oPBL.SaveTransDet(sigTransID, objRXInfo.RXNo, objRXInfo.RefillNo);
        //                }
        //                //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", "Completed Writing into PharmSQL for .SaveTransDet() ");
        //                logger.Trace("UpdateRXData() - Completed Writing into PharmSQL for .SaveTransDet()");
        //            }
        //            #endregion
        //        }
        //    }
        //    //Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "UpdateRXData()", clsPOSDBConstants.Log_Exiting);
        //    //logger.Trace("UpdateRXData() - " + clsPOSDBConstants.Log_Exiting);
        //    logger.Trace("UpdateRXData() - " + clsPOSDBConstants.Log_Exiting);
        //}

        #endregion Persist Methods

        #region Validation Methods
        public void checkIsValidData(TransHeaderData updates, TransDetailData oTransDData, POSTransPaymentData oTransPData)
        {
            TransHeaderTable table;

            TransHeaderRow oRow;

            oRow = (TransHeaderRow)updates.Tables[0].Rows[0];

            table = (TransHeaderTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (TransHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((TransHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((TransHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (TransHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((TransHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((TransHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (TransHeaderRow row in table.Rows)
            {
                if (row.CustomerID == 0 || row.CustomerID.ToString() == null)
                    ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_CustomerIDCanNotNull);
                if (row.TransDate.ToString() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_DateIsInvalid);
            }

            if (oTransDData.TransDetail.Rows.Count == 0)
                ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_DetailIsMisssing);
            if (oTransPData.POSTransPayment.Rows.Count == 0)
                ErrorHandler.throwCustomError(POSErrorENUM.TransPayment_DetailIsMisssing);
        }

        public void Validate_Customer(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_CustomerIDCanNotNull);
            }
        }

        public void Validate_ItemDescription(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.Item_DescriptionCanNotBeNULL);
            }
        }

        public void Validate_ItemID(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_ItemCodeCanNotNull);
            }
        }

        public void Validate_Qty(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.TransDetail_QTYCanNotNull);
            }
        }

        public bool IsTransAlreadyReturned(int iTransID)
        {
            if (iTransID != 0)
            {
                TransHeaderSvr oSvr = new TransHeaderSvr();
                return oSvr.IsTransAlreadyReturned(iTransID);
            }
            else
            {
                return false;
            }
        }

        public System.Decimal CheckGroupPricing(System.String ItemID, System.Int32 QTY, System.Decimal itemPrice, out int nonGroupQty, System.Decimal OrignalPrice)    //PRIMEPOS-3098 20-Jun-2022 JY Added OrignalPrice
        {
            decimal extendedPrice = -1;
            nonGroupQty = 0;

            ItemGroupPrice oIGPrice = new ItemGroupPrice();
            ItemGroupPriceData oIGPData;
            oIGPData = oIGPrice.PopulateList(string.Concat(clsPOSDBConstants.ItemGroupPrice_Fld_ItemID, "='", ItemID,
                "' and ", clsPOSDBConstants.ItemGroupPrice_Fld_Qty, "<=", QTY,
                " And IsNull(", clsPOSDBConstants.ItemGroupPrice_Fld_StartDate, ",cast(convert(varchar,getdate(),110) as smalldatetime))<=cast(GetDate() as smalldatetime) And IsNull(",
                clsPOSDBConstants.ItemGroupPrice_Fld_EndDate, ",cast(convert(varchar,getdate(),110) as smalldatetime))>=cast(convert(varchar,getdate(),110) as smalldatetime)",
                " order by ", clsPOSDBConstants.ItemGroupPrice_Fld_Qty, " Desc"));
            if (oIGPData.ItemGroupPrice.Rows.Count > 0)
            {
                ItemGroupPriceRow oIGPRow = (ItemGroupPriceRow)oIGPData.ItemGroupPrice.Rows[0];
                int quotient = 0;
                int remainder = 0;
                quotient = Math.DivRem(QTY, oIGPRow.Qty, out remainder);
                extendedPrice = quotient * oIGPRow.SalePrice;
                oTDRow.Discount = CalculateDiscount(ItemID, quotient, oIGPRow.SalePrice);   //PRIMEPOS-3098 20-Jun-2022 JY Added
                oTDRow.ItemGroupPrice = true;    //PRIMEPOS-3098 20-Jun-2022 JY Added

                if (remainder > 0)
                {
                    decimal returnVal = CheckGroupPricing(ItemID, remainder, itemPrice, out nonGroupQty, OrignalPrice); //PRIMEPOS-3098 20-Jun-2022 JY Added OrignalPrice
                    if (returnVal == -1)
                    {
                        extendedPrice += remainder * OrignalPrice;
                        nonGroupQty = remainder;
                        oTDRow.Discount += CalculateDiscount(ItemID, remainder, OrignalPrice);   //PRIMEPOS-3098 20-Jun-2022 JY Added
                    }
                    else
                    {
                        extendedPrice += returnVal;
                    }
                }
            }
            else
            {
                nonGroupQty = QTY;
            }

            return extendedPrice;
        }

        /*		public void Validate_Cost(string strValue)
                {
                    if (strValue.Trim()=="" || strValue==null )
                    {
                        ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_CostCanNotBeNull);
                    }
                }

                public void Validate_SalePrice(string strValue)
                {
                    if (strValue.Trim()=="" || strValue==null )
                    {
                        ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_SalePriceCanNotBeNull);
                    }
                }
                */

        #endregion Validation Methods

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        /// <summary>
        /// Added By Shitaljit(QuicSolv) on 18 May 2011
        ///if there are any credit card details for the selected patient
        /// </summary>
        /// <param name="PatientNo"></param>
        /// <returns></returns>
        public DataTable PopulatePatientAccount(string PatientNo)
        {
            DataTable dtPatientPayPref = new DataTable();
            PharmBL oPhBl = new PharmBL();
            try
            {
                //ErrorLogging.Logs.Logger("POSTransaction", "PopulatePatientAccount()", " About to call PharmSQL");
                logger.Trace("PopulatePatientAccount() - About to call PharmSQL");
                dtPatientPayPref = oPhBl.GetPatientPayPref(PatientNo, "CC");
                //ErrorLogging.Logs.Logger("POSTransaction", "PopulatePatientAccount()", " Call PharmSQL successful");
                logger.Trace("PopulatePatientAccount() - Call PharmSQL successful");
            }
            catch { }
            return dtPatientPayPref;
        }
        //Added By Shitaljit(QuicSolv) on 17 May 2011
        public DataTable PatientInfo(TransDetailRXData oTRxDetailData)
        {
            try
            {
                using (TransDetailRXData dao = new TransDetailRXData())
                {
                    return dao.PatientInfo(oTRxDetailData);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PatientInfo(TransDetailRXData oTRxDetailData)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        /// <summary>
        ///  Added By Shitaljit(QuicSolv) on 5 Nov 2012
        /// Gets Credt Card Detaials from PatPayPref in PhamSql for all Patients belonging to provided AccNo.
        /// </summary>
        /// <param name="sAccNo"></param>
        /// <returns></returns>
        public DataTable GetPatientPayPrefByAccNo(string sAccNo)
        {
            DataTable dtPatientPayPref = new DataTable();
            if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
            {
                PharmBL oPhBl = new PharmBL();
                //ErrorLogging.Logs.Logger("POSTransaction", "GetPatientPayPrefByAccNo()", " About to call PharmSQL");
                logger.Trace("GetPatientPayPrefByAccNo() - About to call PharmSQL");
                dtPatientPayPref = oPhBl.GetPatientPayPrefByAccNo(sAccNo, "CC");
                //ErrorLogging.Logs.Logger("POSTransaction", "GetPatientPayPrefByAccNo()", " Call PharmSQL Successful");
                logger.Trace("GetPatientPayPrefByAccNo() - Call PharmSQL Successful");
            }
            return dtPatientPayPref;
        }
        /// <summary>
        /// PRIMEPOS-3103 Gets Credt Card Detaials from PatPayPref in PhamSql for all Patients belonging to provided PatientNo
        /// </summary>
        /// <param name="sPatientNo"></param>
        /// <returns></returns>
        public DataTable GetPatientPayPrefByPatientNo(string sPatientNo)
        {
            DataTable dtPatientPayPref = new DataTable();
            if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
            {
                PharmBL oPhBl = new PharmBL();
                //ErrorLogging.Logs.Logger("POSTransaction", "GetPatientPayPrefByAccNo()", " About to call PharmSQL");
                logger.Trace("GetPatientPayPrefByPatientNo() - About to call PharmSQL");
                dtPatientPayPref = oPhBl.GetPatientPayPrefByPatientNo(sPatientNo, "CC");
                //ErrorLogging.Logs.Logger("POSTransaction", "GetPatientPayPrefByAccNo()", " Call PharmSQL Successful");
                logger.Trace("GetPatientPayPrefByPatientNo() - Call PharmSQL Successful");
            }
            return dtPatientPayPref;
        }

        /// <summary>
        /// Author:Shitaljit 
        /// To get Item TAX Codes data accroding to tax policy define for the item.
        /// </summary>
        /// <param name="sItemID"></param>
        /// <returns></returns>
        public TaxCodesData GetItemTaxData(string sItemID)
        {
            //PRIMEPOS-2500 03-Apr-2018 JY did some changes
            Item oItem = new Item();
            ItemData oITemData = new ItemData();
            ItemRow oItemRow = null;
            TaxCodesData oTaxCodesData = new TaxCodesData();
            ItemTax oTaxCodes = new ItemTax();
            try
            {
                oITemData = oItem.Populate(sItemID);

                if (Configuration.isNullOrEmptyDataSet(oITemData) == true)
                {
                    return oTaxCodesData;
                }
                oItemRow = oITemData.Item[0];

                if (oItemRow.TaxPolicy == "O" || oItemRow.TaxPolicy == String.Empty)
                {
                    bool isDeptTaxable = false;
                    Department oDepartment = new Department();
                    if (Configuration.convertNullToInt(oItemRow.DepartmentID) > 0)
                        isDeptTaxable = oDepartment.IsTaxable(oItemRow.DepartmentID);

                    if (isDeptTaxable == true)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(Configuration.convertNullToString(oItemRow.DepartmentID), EntityType.Department);
                        //if department is taxable but no tax record found then we need to consider item tax policy
                        if (Configuration.isNullOrEmptyDataSet(oTaxCodesData) == true && oItemRow.isTaxable == true)
                        {
                            oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                        }
                    }
                    else if (oItemRow.isTaxable == true)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                    }
                }
                else if (oItemRow.TaxPolicy == "D")
                {
                    bool isDeptTaxable = false;
                    Department oDepartment = new Department();
                    if (Configuration.convertNullToInt(oItemRow.DepartmentID) > 0)
                        isDeptTaxable = oDepartment.IsTaxable(oItemRow.DepartmentID);

                    if (isDeptTaxable == true)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(Configuration.convertNullToString(oItemRow.DepartmentID), EntityType.Department);
                    }
                }
                else if (oItemRow.TaxPolicy == "I")
                {
                    if (oItemRow.isTaxable == true)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetItemTaxData()");
                throw Ex;
            }
            return oTaxCodesData;
        }

        public void SetRowTrans(TransDetailRow oRow)
        {
            SetRowTrans(oRow, true);
        }

        public void SetRowTrans(TransDetailRow oRow, bool isSale)
        {
            if (this.CurrentTransactionType == POSTransactionType.Sales && isSale == true)
            {
                oRow.isSalesTransaction = true;
            }
            else
            {
                oRow.isSalesTransaction = false;
            }
        }

        //Sprint-23 - PRIMEPOS-2029 25-Mar-2016 JY Added
        public NplexBL.PatientInfo.IDTypes GetIDType(string idtype)
        {
            NplexBL.PatientInfo.IDTypes Return = NplexBL.PatientInfo.IDTypes.DL_ID;

            switch (idtype)
            {
                case "DL":
                    Return = NplexBL.PatientInfo.IDTypes.DL_ID;
                    break;
                case "UP":
                    Return = NplexBL.PatientInfo.IDTypes.PASSPORT;
                    break;
                case "MC":
                    Return = NplexBL.PatientInfo.IDTypes.MILITARY_ID;
                    break;
                case "SI":
                    Return = NplexBL.PatientInfo.IDTypes.INSTITUTION;
                    break;
                case "ALIEN":
                    Return = NplexBL.PatientInfo.IDTypes.ALIEN;
                    break;
                case "STATE_ID":
                    Return = NplexBL.PatientInfo.IDTypes.STATE_ID;
                    break;
                case "RC":
                case "FP":
                case "EA":
                case "VC":
                case "TD":
                    Return = NplexBL.PatientInfo.IDTypes.OTHER;
                    break;
                default:
                    break;
            }
            return Return;
        }

        #region PRIMEPOS-2442 ADDED BY ROHIT NAIR
        public static bool HasActiveConsent(RXHeader oheader)
        {
            bool hasConsent = false;
            PharmBL oPBL = new PharmBL();
            int patno = Convert.ToInt32(oheader.PatientNo);
            DataTable otable = oPBL.GetLastPatientConsent(patno, Configuration.CInfo.SelectedConsentSource.ToUpper().Trim());

            if (otable != null && otable.Rows.Count > 0)
            {
                try
                {
                    //Added by Rohit Nair on 08/29/2017 to prevent Consent capture if there is already an existing consent
                    if (otable.Rows[0]["ConsentEndDate"] != null && !string.IsNullOrEmpty(otable.Rows[0]["ConsentEndDate"].ToString()))
                    {
                        if (Convert.ToDateTime(otable.Rows[0]["ConsentEndDate"].ToString()) > DateTime.Now)
                        {
                            hasConsent = true;
                        }
                    }
                    else
                    {
                        hasConsent = true;
                    }

                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "An error occured while trying to convert value " + otable.Rows[0]["ConsentEndDate"].ToString());
                }

            }
            return hasConsent;

        }
        #endregion

        //PRIMEPOS-CONSENT SAJID DHUKKA //PRIMEPOS-2866,PRIMEPOS-2871
        public static List<PatientConsent> GetActiveConsentList(RXHeader oheader)
        {
            List<PatientConsent> patientConsentList = new List<PatientConsent>();
            try
            {
                CompanyInfo oCInfo = new Resources.CompanyInfo();
                PharmBL oPBL = new PharmBL();
                DataTable otable = new DataTable();
                int patno = Convert.ToInt32(oheader.PatientNo);
                if (Configuration.CInfo.ConsentSourceActiveList == null)
                {
                    otable = new DataTable();
                    otable = oPBL.PopulateConsentSource();
                    foreach (DataRow row in otable.Rows)
                    {
                        if (oCInfo.ConsentSourceActiveList == null)
                        {
                            oCInfo.ConsentSourceActiveList = new Dictionary<int, string>();
                        }
                        oCInfo.ConsentSourceActiveList.Add(int.Parse(row["ID"].ToString()), row["Name"].ToString());
                    }
                    Configuration.CInfo.ConsentSourceActiveList = oCInfo.ConsentSourceActiveList;
                }
                otable = new DataTable();
                bool isConsentExpired, isConsentHave;
                otable = oPBL.GetActivePatientConsent(patno, Configuration.CInfo.ConsentSourceActiveList, out isConsentExpired, out isConsentHave); // This is for value have but expired
                isRxConsentHave = isConsentHave; //PRIMEPOS-3192

                if (otable?.Rows.Count > 0)  //PRIMEPOS-3192
                {
                    if (isConsentExpired)
                    {
                        bool isAutoRefillAlredyinPatientList = false; //PRIMEPOS - 3287
                        foreach (DataRow row in otable.Rows)
                        {
                            //patientConsentList.Add(new PatientConsent { IsConsentSkip = false, ConsentEndDate = DateTime.Parse(row["ConsentEndDate"].ToString()), ConsentCaptureDate = DateTime.Parse(row["ConsentCaptureDate"].ToString()), ConsentSourceID = int.Parse(row["ConsentSourceId"].ToString()) });
                            #region PRIMEPOS-3287
                            string TempConsentSourceName = string.Empty;
                            DataTable dt = oPBL.PopulateConsentNameBasedOnID(Convert.ToString(row["ConsentSourceId"]));
                            if (dt.Rows.Count > 0)
                            {
                                TempConsentSourceName = dt.Rows[0]["Name"].ToString();
                            }
                            if (!string.IsNullOrEmpty(TempConsentSourceName) && TempConsentSourceName.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_AUTO_REFILL.ToUpper())
                            {
                                isAutoRefillAlredyinPatientList = true;
                            }
                            #endregion
                            patientConsentList.Add(new PatientConsent { IsConsentSkip = false, ConsentCaptureDate = DateTime.Parse(row["ConsentCaptureDate"].ToString()), ConsentSourceID = int.Parse(row["ConsentSourceId"].ToString()),ConsentSourceName = TempConsentSourceName }); //PRIMEPOS-3287 modified
                        }
                        #region PRIMEPOS-3287
                        if (!isAutoRefillAlredyinPatientList)
                        {
                            if (Configuration.CInfo.ConsentSourceActiveList?.Count > 0)
                            {
                                foreach (var item in Configuration.CInfo.ConsentSourceActiveList)
                                {
                                    if (item.Value.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_AUTO_REFILL.ToUpper())
                                    {
                                        bool isPresciptionActive = oPBL.isPrescriptionConsentActive(item.Key, Constants.CONSENT_TYPE_CODE_PRESCRIPTION_AUTO_REFILL);//PRIMEPOS-3192N
                                        if (isPresciptionActive)
                                        {
                                            patientConsentList.Add(new PatientConsent { IsConsentSkip = false, ConsentSourceID = item.Key, ConsentSourceName = item.Value });
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                else
                { // If record is not there in patient consent
                    if (!isConsentHave)
                    {
                        if (Configuration.CInfo.ConsentSourceActiveList?.Count > 0)
                        {
                            foreach (var item in Configuration.CInfo.ConsentSourceActiveList) // if value not there in configuration
                            {
                                patientConsentList.Add(new PatientConsent { IsConsentSkip = false, ConsentSourceID = item.Key, ConsentSourceName = item.Value });
                            }
                        }
                    }
                    else //PRIMEPOS-3192
                    {
                        if (Configuration.CInfo.ConsentSourceActiveList?.Count > 0)
                        {
                            foreach (var item in Configuration.CInfo.ConsentSourceActiveList)
                            {
                                if (item.Value.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_AUTO_REFILL.ToUpper())
                                {
                                    bool isPresciptionActive = oPBL.isPrescriptionConsentActive(item.Key, Constants.CONSENT_TYPE_CODE_PRESCRIPTION_AUTO_REFILL);//PRIMEPOS-3192N
                                    if (isPresciptionActive)//PRIMEPOS-3192N
                                    {
                                        patientConsentList.Add(new PatientConsent { IsConsentSkip = false, ConsentSourceID = item.Key, ConsentSourceName = item.Value });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "An error occured while trying GetActiveConsentList " + oheader.PatientNo);
            }
            return patientConsentList;
        }

        public DataSet CheckROATransForReturn(string transID)
        {
            DataSet DsROA;
            try
            {
                TransHeaderSvr oTHsvr = new TransHeaderSvr();
                DsROA = oTHsvr.PopulateROATransForReturn(Convert.ToInt32(transID));
                bool bThrowExp = false; //PRIMEPOS-2586 11-Sep-2018 JY Added
                if (DsROA == null)
                {
                    bThrowExp = true;
                    //throw (new Exception("This transaction is already returned."));
                }
                else if (DsROA.Tables[0].Rows.Count == 0)
                {
                    bThrowExp = true;
                    //throw (new Exception("This transaction is already returned."));
                }
                #region PRIMEPOS-2586 11-Sep-2018 JY Added
                if (bThrowExp == true)
                {
                    var ex = new Exception(string.Format("{0} - {1}", Configuration.sTransAlreadyReturned, Configuration.iTransAlreadyReturned));
                    ex.Data.Add(Configuration.iTransAlreadyReturned, Configuration.sTransAlreadyReturned);
                    throw ex;
                }
                #endregion
            }
            catch (Exception exp)
            {
                #region PRIMEPOS-2751 04-Nov-2019 JY Added
                string statusMessage = string.Empty;
                bool bLogException = true;
                if (exp.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in exp.Data)
                    {
                        if (de.Key.ToString() == Configuration.iTransAlreadyReturned.ToString())
                        {
                            statusMessage = exp.Data[de.Key].ToString();
                            bLogException = false;
                            break;
                        }
                    }
                }
                else //PRIMEPOS-2844 13-May-2020 JY Added
                {
                    try
                    {
                        if (((POSExceptions)exp).ErrNumber.ToString() == Configuration.iTransAlreadyReturned.ToString())
                        {
                            statusMessage = ((POSExceptions)exp).ErrMessage;
                            bLogException = false;
                        }
                    }
                    catch { }
                }

                if (bLogException == true)
                {
                    logger.Fatal(exp, "CheckROATransForReturn()");
                }
                #endregion

                throw exp;
            }
            return DsROA;
        }

        public bool IsROATrans(DataSet transHeader)
        {

            bool isROATrans = false;
            try
            {
                if (transHeader != null)
                {
                    if (transHeader.Tables[0].Rows.Count > 0)
                    {
                        if (Configuration.convertNullToString(transHeader.Tables[0].Rows[0]["TransType"]).Equals("3") == true)
                        {
                            isROATrans = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransactionByTransactionId()");
                throw exp;
            }
            return isROATrans;
        }

        public void ExtractRXInfoFromDescription(string sDescription, out long RXNo, out int refillNo, out int partialFillNo)  //PRIMEPOS-2515 17-Oct-2018 JY changed RxNo datatype from int to long
        {
            RXNo = 0;
            refillNo = -1;
            partialFillNo = 0;
            String[] splitArray = sDescription.Split('-');

            if (splitArray.Length > 0)
            {
                RXNo = Configuration.convertNullToInt64(splitArray[0].Trim());
            }
            if (splitArray.Length > 1)
            {
                try
                {
                    refillNo = Int32.Parse(splitArray[1].Trim());
                }
                catch
                {
                    refillNo = -1;
                }
            }
            if (splitArray.Length > 2)
            {
                try
                {
                    partialFillNo = Int32.Parse(splitArray[2].Trim());
                }
                catch
                {
                    partialFillNo = 0;
                }
            }
        }
        public string ExtractRXInfoFromDescription(string sDescription)
        {
            logger.Trace("ExtractRXInfoFromDescription() - " + clsPOSDBConstants.Log_Entering);
            long RXNo;  //PRIMEPOS-2515 17-Oct-2018 JY changed RxNo datatype from int to long
            int refillNo;
            int partialFillNo;
            ExtractRXInfoFromDescription(sDescription, out RXNo, out refillNo, out partialFillNo);

            if (refillNo >= 0)
            {
                logger.Trace("ExtractRXInfoFromDescription() - " + clsPOSDBConstants.Log_Exiting + "1");
                if (partialFillNo > 0)
                    return RXNo.ToString() + "-" + refillNo.ToString() + "-" + partialFillNo.ToString();
                else
                    return RXNo.ToString() + "-" + refillNo.ToString();
            }
            else
            {
                logger.Trace("ExtractRXInfoFromDescription() - " + clsPOSDBConstants.Log_Exiting + "2");
                return RXNo.ToString();
            }
        }

        #region TransSVR
        public TransHeaderData GetTransactionHeaderByTransactionId(string sTransId)
        {
            TransHeaderData oHData;
            try
            {
                TransHeaderSvr oTHsvr = new TransHeaderSvr();
                oHData = oTHsvr.Populate(Convert.ToInt32(sTransId));

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransactionByTransactionId()");
                throw exp;
            }
            return oHData;
        }

        public TransDetailData PopulateTransactionDetailData(int transId)
        {
            TransDetailData oTransDetData;
            try
            {
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();
                oTransDetData = oTransDetailSvr.PopulateData(transId);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateTransactionData()");
                throw exp;
            }
            return oTransDetData;
        }
        public DataSet PopulateTransactionDetData(int transId)
        {
            DataSet oTransDetData;
            try
            {
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();
                oTransDetData = oTransDetailSvr.Populate(transId);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateTransactionDetData()");
                throw exp;
            }
            return oTransDetData;
        }

        public DataSet PopulateTransactionList(string condn)
        {
            DataSet oTransDetData;
            try
            {
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();
                oTransDetData = oTransDetailSvr.PopulateList(condn);
                oTransDetData.Tables[0].Columns.Add("Picked", typeof(System.Boolean));
                oTransDetData.Tables[0].Columns.Add("RowIndex", typeof(System.Int16));
                int Index = 0;
                for (int k = 0; k < oTransDetData.Tables[0].Rows.Count; k++)
                {
                    oTransDetData.Tables[0].Rows[k]["Picked"] = 0;
                    oTransDetData.Tables[0].Rows[k]["RowIndex"] = Index;
                    Index++;
                }


            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateTransactionList()");
                throw exp;
            }
            return oTransDetData;
        }

        public DataSet PopulateTransactionByTransactionId(string sTransId, bool isCallofRetTrans)
        {
            DataSet oTDData;
            try
            {
                TransDetailSvr oTDsvr = new TransDetailSvr();
                oTDData = oTDsvr.Populate(Configuration.convertNullToInt(sTransId), isCallofRetTrans);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransactionByTransactionId()");
                throw exp;
            }
            return oTDData;
        }

        #endregion TransSVR

        #region SigPad

        public void GetBytesForPatientSignature(string strSignature, out byte[] btarr)
        {
            btarr = null;
            try
            {

                Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                string errorMsg = string.Empty;
                SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                if (SigPadUtil.DefaultInstance.isISC)
                {
                    byte[] iscsig = Convert.FromBase64String(strSignature);
                    sigDisp.DrawSignatureMX(iscsig, ref bmp, out errorMsg);
                }
                else if (SigPadUtil.DefaultInstance.isEvertec || SigPadUtil.DefaultInstance.isElavon)//2664//2943
                {
                    byte[] iscsig = Encoding.Default.GetBytes(strSignature);
                    sigDisp.DrawSignatureMXCrop(iscsig, ref bmp, out errorMsg, new Size(0, 0));
                }
                else
                {
                    sigDisp.DrawSignature(strSignature, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);
                }
                ImageConverter converter = new ImageConverter();
                btarr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                if (SigPadUtil.DefaultInstance.isVantiv)
                {
                    //btarr = System.Text.Encoding.Default.GetBytes(SigPadUtil.DefaultInstance.CustomerSignature);
                    #region Start PRIMEPOS-3055
                    if (!Configuration.CPOSSet.DispSigOnTrans)
                    {
                        picSignature = new PictureBox();
                        byte[] bytes = Encoding.Default.GetBytes(SigPadUtil.DefaultInstance.CustomerSignature);
                        SigDiplay.Signature oSigDisplay = new SigDiplay.Signature();
                        oSigDisplay.SetFormat("PointsLittleEndian");
                        oSigDisplay.SetData(bytes);
                        Bitmap oBmpSig = oSigDisplay.GetSignatureBitmap(80);//PRIMEPOS-3063
                        picSignature.SizeMode = PictureBoxSizeMode.StretchImage;
                        picSignature.Image = oBmpSig;
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        picSignature.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                        btarr = Encoding.Default.GetBytes(Encoding.Default.GetString(ms.ToArray()));
                    }
                    else
                    {
                        btarr = System.Text.Encoding.Default.GetBytes(SigPadUtil.DefaultInstance.CustomerSignature);
                    }
                    #endregion End PRIMEPOS-3055
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetBytesForPatientSignature()");
                throw exp;
            }
        }

        public void GetSigTypeForOTCItem(string strSignature, string sigType, ref byte[] SignBinary, ref signatureType oSignatureType)
        {

            OTCSignDataText = "";
            //SignBinary = null;
            byte[] btarr = null;
            //oSignatureType = new signatureType();
            try
            {

                if (sigType != clsPOSDBConstants.BINARYIMAGE)
                {
                    OTCSignDataText = strSignature;
                    #region Sprint-24 - PRIMEPOS-2332 22-Aug-2016 JY Added
                    try
                    {
                        oSignatureType.mimeType = @"image/MemoryBmp";
                        oSignatureType.Value = strSignature;
                    }
                    catch (Exception Ex1)
                    {
                        //it is optional paramter, so no need to capture exception
                    }
                    #endregion
                }
                else
                {
                    Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    string errorMsg = string.Empty;
                    SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                    //sigDisp.DrawSignature(strSignature, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);    //03-Aug-2016 JY Commented

                    #region 03-Aug-2016 JY Change the logic to save signature as the signature saved by previous logic was not shoing on report
                    if (SigPadUtil.DefaultInstance.isISC)
                    {
                        byte[] iscsig = Convert.FromBase64String(strSignature);
                        sigDisp.DrawSignatureMX(iscsig, ref bmp, out errorMsg);
                    }
                    else if (SigPadUtil.DefaultInstance.isPAX)
                    {
                        sigDisp.DrawSignaturePAX(strSignature, ref bmp, out errorMsg);
                    }
                    else if (SigPadUtil.DefaultInstance.isElavon || SigPadUtil.DefaultInstance.isEvertec)//2943
                    {
                        sigDisp.DrawSignatureMX(Encoding.Default.GetBytes(strSignature), ref bmp, out errorMsg);
                    }
                    else
                    {
                        sigDisp.DrawSignature(strSignature, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);
                    }
                    #endregion

                    ImageConverter converter = new ImageConverter();
                    btarr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                    SignBinary = btarr;

                    #region Sprint-24 - PRIMEPOS-2332 22-Aug-2016 JY Added
                    try
                    {
                        oSignatureType.mimeType = @"image/" + GetImageFormat(bmp.RawFormat);
                        oSignatureType.Value = Convert.ToBase64String(SignBinary);
                    }
                    catch (Exception Ex1)
                    {
                        //it is optional paramter, so no need to capture exception
                    }
                    #endregion
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetSigType()");
                throw exp;
            }

        }

        public void GetRXHeaderForSiganture(string sigType, string strSignature, string patientCounceling, ref RXHeader oRXHeader)
        {
            try
            {

                if (sigType != clsPOSDBConstants.BINARYIMAGE)
                    oRXHeader.RXSignature = strSignature;
                else
                {
                    Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    //Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    string errorMsg = string.Empty;
                    SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                    if (SigPadUtil.DefaultInstance.isISC)
                    {
                        byte[] iscsig = Convert.FromBase64String(strSignature);
                        sigDisp.DrawSignatureMX(iscsig, ref bmp, out errorMsg);
                    }
                    if (SigPadUtil.DefaultInstance.isEvertec || SigPadUtil.DefaultInstance.isElavon)//2664//2943
                    {
                        byte[] iscsig = Encoding.Default.GetBytes(strSignature);
                        sigDisp.DrawSignatureMX(iscsig, ref bmp, out errorMsg);
                    }
                    else
                    {
                        sigDisp.DrawSignature(strSignature, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);
                    }
                    ImageConverter converter = new ImageConverter();
                    byte[] btarr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                    oRXHeader.bBinarySign = btarr;
                }
                oRXHeader.CounselingRequest = patientCounceling; //we have to get cousling req from device

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetRXHeaderForSiganture()");
                throw exp;
            }
        }

        public bool IsSignatureValid(string strSigString, string _sigType, out bool rphVerified, out bool isVerifySignature, out byte[] btarr)
        {


            rphVerified = false;
            bool ret = false;
            btarr = null;
            isVerifySignature = false;
            try
            {
                if (Configuration.CPOSSet.UseSigPad == true)
                {
                    SigPadUtil.DefaultInstance.ShowCustomScreen("Validating Signature. Please wait...");
                }

                if (_sigType != clsPOSDBConstants.BINARYIMAGE)
                {
                    ret = true;
                    //TODO 
                    //
                }
                //else if (SigPadUtil.DefaultInstance.isEvertec)
                //{
                //    btarr = Encoding.GetEncoding(1252).GetBytes(strSigString);
                //    ret = true;
                //}
                else
                {
                    Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    //Bitmap bmp = new Bitmap(335, 245, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    string errorMsg = string.Empty;
                    SigDiplay.SigDisplay sigDisp = new SigDiplay.SigDisplay();
                    if (SigPadUtil.DefaultInstance.isISC)
                    {
                        byte[] iscsig = Convert.FromBase64String(strSigString);
                        sigDisp.DrawSignatureMX(iscsig, ref bmp, out errorMsg);
                    }
                    else if (SigPadUtil.DefaultInstance.isPAX)
                    {
                        sigDisp.DrawSignaturePAX(strSigString, ref bmp, out errorMsg);
                    }
                    //else if (SigPadUtil.DefaultInstance.isEvertec)//PRIMEPOS-2664
                    //{
                    //    btarr = Encoding.Default.GetBytes(strSigString);
                    //}
                    else
                    {
                        sigDisp.DrawSignature(strSigString, ref bmp, out errorMsg, clsPOSDBConstants.BINARYIMAGE);
                    }
                    if (SigPadUtil.DefaultInstance.IsVF && !string.IsNullOrWhiteSpace(strSigString) && !SigPadUtil.DefaultInstance.isISC)//PRIMEPOS-2867
                    {
                        foreach (RXHeader oHeader in oRXHeaderList)
                        {
                            if (oHeader.IsConsentRequired)
                            {
                                foreach (PatientConsent item in oHeader.PatConsent)
                                {
                                    if (!item.IsConsentSkip)
                                    {
                                        item.SignatureData = System.Text.Encoding.Default.GetBytes(strSigString);
                                    }
                                }
                            }
                            oHeader.bBinarySign = System.Text.Encoding.Default.GetBytes(strSigString);
                        }
                    }
                    ImageConverter converter = new ImageConverter();
                    //if (!SigPadUtil.DefaultInstance.isEvertec)
                    btarr = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                    if (btarr != null && btarr.Length > 1)
                    {
                        switch (btarr[0])
                        {
                            #region PNG
                            case 0x89:
                                {
                                    int limit = 0;
                                    if (SigPadUtil.DefaultInstance.isISC)
                                    {
                                        limit = 700;
                                    }
                                    else if (SigPadUtil.DefaultInstance.isEvertec)//2664
                                    {
                                        limit = 500;
                                    }
                                    else if (SigPadUtil.DefaultInstance.isElavon)//2943
                                    {
                                        limit = 200;
                                    }
                                    else if (SigPadUtil.DefaultInstance.isVantiv)//PRIMEPOS-3063
                                    {
                                        limit = 1100;
                                    }
                                    else
                                    {
                                        limit = 1700;
                                    }
                                    if (btarr.Length > limit)
                                    {
                                        ret = true;
                                    }
                                    else
                                    {
                                        isVerifySignature = true;
                                    }
                                }
                                break;
                            #endregion
                            #region BMP
                            case 0x42:
                                {
                                    if (btarr.Length > 35000)
                                    {
                                        ret = true;
                                    }
                                    else
                                    {
                                        isVerifySignature = true;
                                    }
                                }
                                break;
                            #endregion
                            #region JPG
                            case 0xFF:
                                {
                                    if (btarr.Length > 1200)
                                    {
                                        ret = true;
                                    }
                                    else
                                    {
                                        isVerifySignature = true;
                                    }
                                }
                                break;
                                #endregion
                        }
                    }

                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IsSignatureValid()");
                throw exp;
            }

            return ret;
        }

        public string GetImageFormat(System.Drawing.Imaging.ImageFormat img)
        {
            if (img.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                return System.Drawing.Imaging.ImageFormat.Jpeg.ToString();
            if (img.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                return System.Drawing.Imaging.ImageFormat.Bmp.ToString();
            if (img.Equals(System.Drawing.Imaging.ImageFormat.Png))
                return System.Drawing.Imaging.ImageFormat.Png.ToString();
            if (img.Equals(System.Drawing.Imaging.ImageFormat.Emf))
                return System.Drawing.Imaging.ImageFormat.Emf.ToString();
            if (img.Equals(System.Drawing.Imaging.ImageFormat.Exif))
                return System.Drawing.Imaging.ImageFormat.Exif.ToString();
            if (img.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                return System.Drawing.Imaging.ImageFormat.Gif.ToString();
            if (img.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                return System.Drawing.Imaging.ImageFormat.Icon.ToString();
            if (img.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
                return System.Drawing.Imaging.ImageFormat.MemoryBmp.ToString();
            if (img.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                return System.Drawing.Imaging.ImageFormat.Tiff.ToString();
            else
                return System.Drawing.Imaging.ImageFormat.Wmf.ToString();
        }

        #endregion

        #region PharmBL

        public string GetErrorMsgForBatchInformation(string bStatus, long lbatchCode)
        {
            string strErrorMsg = "";
            switch (bStatus)
            {
                case "0"://2927
                    strErrorMsg = string.Format("Unable Process Intake Batch Due To IsReadyForPayment field #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Call Patient");
                    break;
                case "01":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Call Patient");
                    break;
                case "02":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Manage Copay ");
                    break;
                case "03":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Exception");
                    break;
                case "IP":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch Is In Progress");
                    break;
                case "SF":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch Already Send For Fulfilment");
                    break;
                case "RF":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch Ready For FulFillment");
                    break;
                case "RJF":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch has been Rejected From Fulfilment");
                    break;
                case "CF":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch Has been Cancelled from Fulfilment");
                    break;
                case "PF":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch Has Been partially Fullfilled");
                    break;
                case "CPF":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch has been completley fulfilled");
                    break;
                case "PSH":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch Has been Shipped Partially");
                    break;
                case "SH":
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} PrimeRx Returned the Following Message {1}", lbatchCode, "Batch Already Shipped");
                    break;
                case "NF": //PRIMEPOS-3251
                    strErrorMsg = string.Format("Batch #{0} {1}", lbatchCode, "Not Found");
                    break;

                default:
                    strErrorMsg = string.Format("Unable Process Intake Batch #{0} Please check the batch status in PrimeRX", lbatchCode);
                    break;
            }
            return strErrorMsg;

        }

        public RXHeader CheckRXAlreadyInTrans(DataTable oRxInfo)
        {
            RXHeader oRXHeader = null;
            try
            {

                if (oRXHeaderList.Count > 0)
                {
                    oRXHeader = oRXHeaderList.FindByPatient(oRxInfo.Rows[0]["PatientNo"].ToString());
                }
                if (oRXHeader != null && oRXHeader.RXDetails != null)
                {
                    DateTime MinRxPickUpDate = DateTime.Today.Date.Subtract(new System.TimeSpan(Configuration.CInfo.UnPickedRXSearchDays, 0, 0, 0));
                    DateTime RxPickedUpDate = Convert.ToDateTime(Configuration.convertNullToString(oRxInfo.Rows[0]["DATEF"]));
                    int iPartialFillNo_Claim = 0;
                    if (oRxInfo.Columns.Contains("PartialFillNo"))
                        iPartialFillNo_Claim = Configuration.convertNullToInt(oRxInfo.Rows[0]["PartialFillNo"]);
                    foreach (TransDetailRXRow oRow in oTransDRXData.TransDetailRX.Rows)
                    {
                        string sRXNo = oRow.RXNo.ToString();
                        string sRefill = oRow.NRefill.ToString();
                        string sPartialFillNo = oRow.PartialFillNo.ToString();
                        foreach (RXDetail oDetail in oRXHeader.RXDetails.FindByRXNo(Configuration.convertNullToInt64(sRXNo), Configuration.convertNullToInt(sRefill), Configuration.convertNullToInt(sPartialFillNo)))
                        {
                            // Comment out by Manoj from the IF. Not correct message (|| RxPickedUpDate.Date < MinRxPickUpDate.Date)
                            if ((oDetail.RXNo.ToString() == oRxInfo.Rows[0]["RXNo"].ToString() && Configuration.CInfo.AllowMultipleRXRefillsInSameTrans == false && Configuration.CInfo.AllowUnPickedRX == "0")
                             || (oDetail.RXNo.ToString() == oRxInfo.Rows[0]["RXNo"].ToString() && oDetail.RefillNo.ToString() == oRxInfo.Rows[0]["nrefill"].ToString() && oDetail.PartialFillNo.ToString() == iPartialFillNo_Claim.ToString()))
                            {
                                //throw (new Exception("Cannot scan same RX in same transaction."));
                                #region PRIMEPOS-2586 24-Sep-2018 JY Added
                                var ex = new Exception(string.Format("{0} - {1}", Configuration.sCanNotScanSameRx, Configuration.iCanNotScanSameRx));
                                ex.Data.Add(Configuration.iCanNotScanSameRx, Configuration.sCanNotScanSameRx);
                                throw ex;
                                #endregion                                
                            }
                            else if (RxPickedUpDate.Date < MinRxPickUpDate.Date)
                            {
                                //throw (new Exception(oDetail.RXNo.ToString() + "RX is older than " + Configuration.CInfo.UnPickedRXSearchDays));                                
                                #region PRIMEPOS-2586 24-Sep-2018 JY Added
                                string sRxIsOlderThan = oDetail.RXNo.ToString() + "RX is older than " + Configuration.CInfo.UnPickedRXSearchDays;
                                var ex = new Exception(string.Format("{0} - {1}", sRxIsOlderThan, Configuration.iRxIsOlderThan));
                                ex.Data.Add(Configuration.iRxIsOlderThan, sRxIsOlderThan);
                                throw ex;
                                #endregion                                
                            }
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                #region PRIMEPOS-2586 24-Sep-2018 JY Added
                string statusMessage = string.Empty;
                bool bLogException = true;

                if (exp.Data.Count > 0)
                {
                    foreach (DictionaryEntry de in exp.Data)
                    {
                        if (de.Key.ToString() == Configuration.iCanNotScanSameRx.ToString() || de.Key.ToString() == Configuration.iRxIsOlderThan.ToString())
                        {
                            statusMessage = exp.Data[de.Key].ToString();
                            bLogException = false;
                            break;
                        }
                    }
                }
                else //PRIMEPOS-2844 13-May-2020 JY Added
                {
                    try
                    {
                        if (((POSExceptions)exp).ErrNumber.ToString() == Configuration.iCanNotScanSameRx.ToString() || ((POSExceptions)exp).ErrNumber.ToString() == Configuration.iRxIsOlderThan.ToString())
                        {
                            statusMessage = ((POSExceptions)exp).ErrMessage;
                            bLogException = false;
                        }
                    }
                    catch { }
                }

                if (bLogException == true)
                {
                    logger.Fatal(exp, "CheckRXAlreadyInTrans()");
                }
                #endregion

                throw exp;
            }
            return oRXHeader;

        }

        #region PRIMEPOS-3248
        public bool HasOnHoldRXForPrimeRxPayment(DataTable dtRxOnHold, DataTable oRxInfo, string nRefill)
        {
            bool flag = false;
            foreach (DataRow oRow in dtRxOnHold.Rows)
            {
                if ((oRxInfo.Rows[0]["RXNO"].ToString() == oRow["RXNO"].ToString() && nRefill == "") || (oRxInfo.Rows[0]["RXNO"].ToString() == oRow["RXNO"].ToString() && nRefill == oRow["NRefill"].ToString()))
                    flag = true;
            }
            return flag;
        }
        #endregion

        public bool HasOnHoldRX(DataTable dtRxOnHold, DataTable oRxInfo, string nRefill, out string strRxNoTransNo)
        {
            bool flag = false;
            strRxNoTransNo = string.Empty;
            int count = 1;
            foreach (DataRow oRow in dtRxOnHold.Rows)
            {
                //if the searched rx and onhold rx are same then throw message as exception else popup message and show list of unpicked rx
                if ((oRxInfo.Rows[0]["RXNO"].ToString() == oRow["RXNO"].ToString() && nRefill == "") || (oRxInfo.Rows[0]["RXNO"].ToString() == oRow["RXNO"].ToString() && nRefill == oRow["NRefill"].ToString()))
                    flag = true;

                if (strRxNoTransNo == string.Empty)
                {
                    strRxNoTransNo += "RX# " + oRow["RXNO"].ToString() + "-" + oRow["NRefill"].ToString() + ": Trans# " + oRow["TransID"].ToString();
                }
                else
                {
                    count++;
                    if (count % 4 == 1)
                        strRxNoTransNo += "," + Environment.NewLine + "RX# " + oRow["RXNO"].ToString() + "-" + oRow["NRefill"].ToString() + ": Trans# " + oRow["TransID"].ToString();
                    else
                        strRxNoTransNo += ", RX# " + oRow["RXNO"].ToString() + "-" + oRow["NRefill"].ToString() + ": Trans# " + oRow["TransID"].ToString();
                }
            }
            return flag;
        }
        public RXHeader InsertRXHeader(DataTable oRxInfo)
        {
            RXHeader oRXHeader = new RXHeader();
            PharmBL oPharmBL = new PharmBL();
            try
            {
                oRXHeader.PatientNo = oRxInfo.Rows[0]["PatientNo"].ToString();
                oRXHeader.InsType = oRxInfo.Rows[0]["BillType"].ToString();
                oRXHeader.RXSignature = "";
                oRXHeader.NOPPSignature = "";
                oRXHeader.bBinarySign = null;
                oRXHeader.FamilyID = Configuration.convertNullToString(oRxInfo.Rows[0]["FamilyID"]);    //Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added

                //PRIMEPOS-2981 28-Jun-2021 JY Added logic to handle NULL
                DataTable oTable = oPharmBL.GetPatient(Configuration.convertNullToString(oRXHeader.PatientNo));
                if (oTable.Rows.Count > 0)
                {
                    oRXHeader.PatientName = Configuration.convertNullToString(oTable.Rows[0]["Lname"]).Trim() + ", " + Configuration.convertNullToString(oTable.Rows[0]["Fname"]).Trim();
                    oRXHeader.PatientAddr = Configuration.convertNullToString(oTable.Rows[0]["addrstr"]).Trim();
                }
                oTable = new DataTable();
                oTable = oPharmBL.GetPrivackAck(Configuration.convertNullToString(oRXHeader.PatientNo));
                if (oTable.Rows.Count > 0)
                {
                    if (Configuration.convertNullToString(oTable.Rows[0]["PatAccept"]).Trim().ToUpper() == "N")
                    {
                        oRXHeader.isNOPPSignRequired = true;
                    }
                    else
                    {
                        try
                        {
                            if (DateTime.Parse(oTable.Rows[0]["datesigned"].ToString()).AddMonths(Configuration.CInfo.PrivacyExpiry) > DateTime.Now)
                            {
                                oRXHeader.isNOPPSignRequired = false;
                            }
                            else
                            {
                                oRXHeader.isNOPPSignRequired = true;
                            }
                        }
                        catch
                        {
                            oRXHeader.isNOPPSignRequired = true;
                        }
                    }
                }
                else
                {
                    oRXHeader.isNOPPSignRequired = true;
                }
                oRXHeaderList.Add(oRXHeader);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "InsertRXHeader()");
                throw exp;
            }
            return oRXHeader;
        }

        public bool IsRXInBatch(RXHeader oheader)
        {
            bool result = false;
            PharmBL oPharmBL = new PharmBL();
            if (oheader != null && oheader.RXDetails != null && oheader.RXDetails.Count > 0)
            {
                try
                {
                    long rxno = oheader.RXDetails[0].RXNo;
                    int nrefill = oheader.RXDetails[0].RefillNo;

                    long Batchno = oPharmBL.GetBatchIDFromRxno(rxno, nrefill);

                    if (Batchno > 0)
                    {
                        DataTable oBatchInfo = oPharmBL.GetBatchStatusfromView(Batchno.ToString());

                        if (oBatchInfo != null && oBatchInfo.Rows.Count > 0)
                        {
                            //PrimePOS-2518 Jenny
                            //if (oBatchInfo.Rows[0]["OFSbatchStatus"].ToString().Equals(Configuration.CInfo.IntakeBatchStatus, StringComparison.OrdinalIgnoreCase))
                            if (Convert.ToBoolean(oBatchInfo.Rows[0]["IsReadyforPayment"]))//Arvind 2925
                            {
                                result = true;
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "IsRXInBatch()");
                    //logger.Error(ex);
                }
            }
            return result;
        }

        public void DeleteRxOnValidating(DataTable RxWithValidClass, DataTable DrugClassInfoCapture, bool isAnyUnPickRx, ref bool isSearchUnPickCancel) //PRIMEPOS-2547 23-Jul-2018 JY Added DrugClassInfoCapture parameter
        {
            if (oRXHeaderList != null && oRXHeaderList.Count > 0)
            {
                if (isSearchUnPickCancel || !isAnyUnPickRx) //Added by Manoj 3/31/2014 - If the user cancel or does not select any unpick RX
                {
                    int rxCount = oRXHeaderList.Count;
                    oRXHeaderList.RemoveAt(rxCount - 1);
                }
                TransDetailRXRow transDetailRxRow = null;
                foreach (RXHeader oInfo in oRXHeaderList)
                {
                    for (int Index = 0; Index < oTransDRXData.TransDetailRX.Rows.Count; Index++)
                    {
                        try
                        {
                            transDetailRxRow = oTransDRXData.TransDetailRX.GetRowByID(Index + 1);
                            RXDetail oRxDetail = oInfo.RXDetails.FindByRXRefillNotInTrans(transDetailRxRow.RXNo, transDetailRxRow.NRefill);
                            if (oRxDetail != null)
                            {
                                //oInfo.RXDetails.Remove(oRxDetail); //Remove because Rx was missing signature when multiple refills are in trans (Manoj) 3/14/2016 
                                //Following if is added by shitaljit to resolved object reffreence not set error while deleting RX item. PRIMEPOS - 801
                                if (RxWithValidClass != null)
                                {
                                    if (RxWithValidClass.Rows.Count > 0)
                                    {
                                        DataRow[] rx = RxWithValidClass.Select("RxNo= " + oRxDetail.RXNo + " AND RefillNo= " + oRxDetail.RefillNo);
                                        foreach (var data in rx)
                                        {
                                            data.Delete();
                                        }
                                    }
                                }
                                #region PRIMEPOS-2547 23-Jul-2018 JY Added                            
                                try
                                {
                                    if (DrugClassInfoCapture != null)
                                    {
                                        if (DrugClassInfoCapture.Rows.Count > 0)
                                        {
                                            DataRow[] rx = DrugClassInfoCapture.Select("RxNo= " + oRxDetail.RXNo + " AND RefillNo= " + oRxDetail.RefillNo);
                                            foreach (var data in rx)
                                            {
                                                data.Delete();
                                            }
                                        }
                                    }
                                }
                                catch { }
                                #endregion
                            }
                        }
                        catch { }
                    }
                }
                isSearchUnPickCancel = false;
            }
        }

        public int GetTransDetailID(string sRxNo, string sRefillNo)
        {
            int transDetID = 0;
            int prevTransDetID = 0;
            DataSet DsPOSTransactionRXDetail = null;
            using (var oSvr = new TransDetailRXSvr())
            {
                DsPOSTransactionRXDetail = oSvr.GetSaleTransDetailWithNoReturnLinked(sRxNo, sRefillNo);  //Sprint-26 - PRIMEPOS-2418 02-Aug-2017 JY Added to get sale transaction having no return transaction linked
                if (DsPOSTransactionRXDetail != null && DsPOSTransactionRXDetail.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow rxpos in DsPOSTransactionRXDetail.Tables[0].Rows)
                    {
                        if (Configuration.convertNullToInt(rxpos["TRANSDETAILID"].ToString()) > prevTransDetID)
                        {
                            prevTransDetID = Configuration.convertNullToInt(rxpos["TRANSDETAILID"].ToString());
                            transDetID = Configuration.convertNullToInt(rxpos["TRANSDETAILID"].ToString());
                        }
                    }
                }
            }
            return transDetID;
        }

        public bool PickedUpRxTransDetails(DataTable oRxInfo, bool isAllowPrimeESC, out bool isExceptionThrown, out bool shouldFetchRX, out string strRxException, out long transId)
        {
            logger.Trace("PickedUpRxTransDetails() - " + clsPOSDBConstants.Log_Entering);
            shouldFetchRX = false;
            strRxException = "";
            transId = 0;
            string strDate = Configuration.convertNullToString(oRxInfo.Rows[0]["PICKUPDATE"].ToString());
            isExceptionThrown = false;
            if (isUnBilledRx == 1 || isUnBilledRx == 2)   //PRIMEPOS-2398 04-Jan-2021 JY modified
            {
                countUnBilledRx--;
            }
            string PaymentType = string.Empty;
            TransDetailRXSvr oSvr = new TransDetailRXSvr();
            DataSet DsPOSTransactionRXDetail = null;
            TransHeaderSvr oTransHSvr;
            TransHeaderData oTransHData = null;
            POSTransPaymentSvr oTransPaySvr = null;
            DataSet DsPOSTransactionDetail = null;
            DataSet oPayTypeData;
            DataSet dsTransPayData;
            PayType oPayType;
            int RowIndex = 0;
            bool isNotProcessInPOS = true;
            int RefillNo = 0;
            string sPickupPOS = string.Empty;
            string sPickupRX = string.Empty;
            int TransRetDetId = 0;
            if (oRxInfo != null && oRxInfo.Rows.Count > 0)
            {
                RefillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["nrefill"]);
                sPickupPOS = Configuration.convertNullToString(oRxInfo.Rows[0]["PickupPOS"]);
                sPickupRX = Configuration.convertNullToString(oRxInfo.Rows[0]["Pickedup"]);
                DsPOSTransactionRXDetail = oSvr.PopulateList(" WHERE RXNO = " + oRxInfo.Rows[0]["Rxno"].ToString() + " AND NREFILL = " + RefillNo);
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();
                if (DsPOSTransactionRXDetail != null && DsPOSTransactionRXDetail.Tables.Count > 0)
                {
                    if (DsPOSTransactionRXDetail.Tables[0].Rows.Count > 0)
                    {
                        for (RowIndex = DsPOSTransactionRXDetail.Tables[0].Rows.Count - 1; RowIndex >= 0; RowIndex--)
                        {
                            DsPOSTransactionDetail = oTransDetailSvr.PopulateTransDetailId(Convert.ToInt32(DsPOSTransactionRXDetail.Tables[0].Rows[RowIndex]["TransDetailID"]));
                            if (Configuration.convertNullToInt(DsPOSTransactionDetail.Tables[0].Rows[0][clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId]) == 0 && TransRetDetId != -1 &&
                                    TransRetDetId != Configuration.convertNullToInt(DsPOSTransactionDetail.Tables[0].Rows[0][clsPOSDBConstants.TransDetail_Fld_TransDetailID]) &&
                                    Configuration.convertNullToInt(DsPOSTransactionRXDetail.Tables[0].Rows[RowIndex][clsPOSDBConstants.TransDetailRX_Fld_NRefill]) == Configuration.convertNullToInt(RefillNo))
                            {
                                isNotProcessInPOS = false;
                                break;
                            }
                            else if (Configuration.convertNullToInt(DsPOSTransactionRXDetail.Tables[0].Rows[RowIndex][clsPOSDBConstants.TransDetailRX_Fld_NRefill])
                                == Configuration.convertNullToInt(RefillNo) &&
                                Configuration.convertNullToInt(DsPOSTransactionDetail.Tables[0].Rows[0][clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId]) > 0)
                            {
                                TransRetDetId = Configuration.convertNullToInt(DsPOSTransactionDetail.Tables[0].Rows[0][clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId]);
                            }
                            else
                            {
                                TransRetDetId = -1;
                            }
                        }

                        if (isNotProcessInPOS == true && (sPickupRX.Equals("Y") == false || isAllowPrimeESC == true))
                        {
                            return true;
                        }
                        oTransHSvr = new TransHeaderSvr();
                        if (DsPOSTransactionDetail != null && DsPOSTransactionDetail.Tables.Count > 0)
                        {
                            if (DsPOSTransactionDetail.Tables[0].Rows.Count > 0)
                            {
                                oTransHData = oTransHSvr.Populate(Convert.ToInt32(DsPOSTransactionDetail.Tables[0].Rows[0]["TransID"]));
                                oPayType = new PayType();

                                if (oTransHData != null && oTransHData.Tables.Count > 0)
                                {
                                    if (oTransHData.Tables[0].Rows.Count > 0)
                                    {
                                        oTransPaySvr = new POSTransPaymentSvr();
                                        dsTransPayData = oTransPaySvr.Populate(Convert.ToInt32(oTransHData.TransHeader[0].TransID));
                                        for (int count = 0; count < dsTransPayData.Tables[0].Rows.Count; count++)
                                        {
                                            oPayTypeData = oPayType.PopulateList(" PayTypeID = '" + dsTransPayData.Tables[0].Rows[count]["TransTypeCode"].ToString() + "'");

                                            if (oPayTypeData.Tables[0].Rows.Count > 0)
                                            {
                                                PaymentType += oPayTypeData.Tables[0].Rows[0]["PayTypeDesc"].ToString() + " , ";
                                            }
                                        }
                                        if (PaymentType.EndsWith(" , "))
                                        {
                                            int index = PaymentType.LastIndexOf(" , ");
                                            PaymentType = PaymentType.Remove(index);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Configuration.isNullOrEmptyDataSet(DsPOSTransactionRXDetail) == true && (sPickupRX.Equals("Y") == false || isAllowPrimeESC == true))
            {
                return true;
            }
            else if (Configuration.isNullOrEmptyDataSet(DsPOSTransactionRXDetail) == true && sPickupRX.Equals("Y") == true)
            {
                logger.Trace("PickedUpRxTransDetails() - Picked up Rx is not allowed.");
                throw (new Exception("Picked up Rx is not allowed."));
            }

            strRxException = "Rx is already picked up on " + strDate;

            RXException rxException;
            //Added By Shitaljit(QuicSolv) on 23 August 2011
            if (Configuration.isNullOrEmptyDataSet(oTransHData) == true)
            {
                strRxException += "\nTransaction ID : " + "Not Available" + "\n" +
               "User : " + "Not Available" + "\n" + "Payment Type :" + "Not Available" + "\n" + "Station ID :" + "Not Available";
                transId = 0;
                isExceptionThrown = true;
                rxException = new RXException(strRxException);
                throw (rxException);
            }
            else if (oTransHData.Tables.Count > 0 && oTransHData.Tables[0].Rows.Count > 0)
            {
                strRxException += "\nTransaction ID : " + oTransHData.TransHeader[0].TransID + "\n" +
                "User : " + oTransHData.TransHeader[0].UserID.ToString() + "\n" + "Payment Type :" + PaymentType + "\n" + "Station ID :" + oTransHData.TransHeader[0].StationID.ToString();
                transId = oTransHData.TransHeader[0].TransID;
                //Move the code inside the if to eliminate the object ref not set error. while ringign up RX item
                //PriemPOS-801 in 17 April 2013
                rxException = new RXException(strRxException);
                rxException.Source = "RXPICKEDUPMSG";
                rxException.TransID = transId;
                isExceptionThrown = true;
                if (isNotProcessInPOS == false && Configuration.CInfo.AllowUnPickedRX.Equals("0") == true)
                {
                    rxException.DisplayYesNoButonOnly = true;
                    //logger.Error(rxException, "PickedUpRxTransDetails()");    //PRIMEPOS-2844 13-May-2020 JY Commented
                    throw (rxException);
                }
                else
                {
                    shouldFetchRX = true;
                }
            }
            logger.Trace("PickedUpRxTransDetails() - " + clsPOSDBConstants.Log_Exiting);
            return false;
        }

        #region PrimePOS-2448 Added BY Rohit Nair
        //public bool CheckRxPickupDetailPOS(DataRow dr, out bool isReturn)
        //{
        //    bool isPickedupPOS = true;
        //    bool notYetReturn = false; ;
        //    string TransID = string.Empty;
        //    DataSet DsPOSTransactionRXDetail = null;
        //    DataSet DsPOSTransactionDetail = null;
        //    long RxNo = 0;
        //    int RefillNo = 0;
        //    string sPickupPOS = string.Empty;
        //    string sPickupRX = string.Empty;

        //    if (dr != null)
        //    {
        //        RxNo = Configuration.convertNullToInt64(dr["Rxno"]);
        //        RefillNo = Configuration.convertNullToInt(dr["nrefill"]);

        //        sPickupPOS = Configuration.convertNullToString(dr["PickupPOS"]);
        //        sPickupRX = Configuration.convertNullToString(dr["Pickedup"]);

        //        using (var oSvr = new TransDetailRXSvr())
        //        {
        //            DsPOSTransactionRXDetail = oSvr.PopulateList(" WHERE RXNO = " + RxNo + " AND NREFILL = " + RefillNo);
        //            if (DsPOSTransactionRXDetail != null && DsPOSTransactionRXDetail.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow rxpos in DsPOSTransactionRXDetail.Tables[0].Rows)
        //                {
        //                    if (dr["RXNO"].ToString() == rxpos["RXNO"].ToString() && dr["NREFILL"].ToString() == rxpos["NREFILL"].ToString()
        //                        && !string.IsNullOrEmpty(rxpos["RETURNTRANSDETAILID"].ToString()) && Convert.ToInt32(rxpos["RETURNTRANSDETAILID"]) > 0)
        //                    {
        //                        isPickedupPOS = false;
        //                    }
        //                    else
        //                    {
        //                        isPickedupPOS = true;
        //                        notYetReturn = true;
        //                        TransID = rxpos["TRANSDETAILID"].ToString();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                isPickedupPOS = false;
        //            }
        //            if (CurrentTransactionType == POSTransactionType.SalesReturn && isPickedupPOS)
        //            {
        //                using (var oTransDetailSvr = new TransDetailSvr())
        //                {
        //                    DsPOSTransactionDetail = oTransDetailSvr.PopulateTransDetailId(Convert.ToInt32(Convert.ToInt32(TransID)));
        //                    foreach (DataRow drTransDetail in DsPOSTransactionDetail.Tables[0].Rows)
        //                    {
        //                        if (Configuration.convertNullToInt(drTransDetail["RETURNTRANSDETAILID"].ToString()) == 0 && CurrentTransactionType == POSTransactionType.SalesReturn)
        //                        {
        //                            if (oTransHRow != null)
        //                                oTransHRow.ReturnTransID = Configuration.convertNullToInt(drTransDetail["TRANSID"].ToString());
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    isReturn = notYetReturn;
        //    return isPickedupPOS;
        //}

        //public bool CheckRxPickupDetailPOS(DataRow dr, out bool isReturn)
        public void CheckRxPickupDetailPOS(DataRow dr, out bool bAlreadyPickedUp, out bool bAlreadyReturned, out bool bNeverUsed)
        {
            bAlreadyPickedUp = false;
            bAlreadyReturned = false;
            bNeverUsed = false;

            try
            {
                if (dr != null)
                {
                    long RxNo = Configuration.convertNullToInt64(dr["Rxno"]);
                    int RefillNo = Configuration.convertNullToInt(dr["nrefill"]);

                    using (var oSvr = new TransDetailRXSvr())
                    {
                        DataTable dtPOSTransactionRXDetail = null;
                        dtPOSTransactionRXDetail = oSvr.GetPOSTransactionRXDetailRecord(RxNo, RefillNo);
                        if (dtPOSTransactionRXDetail != null && dtPOSTransactionRXDetail.Rows.Count > 0)
                        {
                            if (Configuration.convertNullToInt(dtPOSTransactionRXDetail.Rows[0]["TransType"]) == 1) //last transaction was sale
                            {
                                if (CurrentTransactionType == POSTransactionType.Sales)
                                {
                                    bAlreadyPickedUp = true;
                                    //clsCoreUIHelper.ShowErrorMsg("Rx#: " + RxNo + "-" + RefillNo + " is already picked up.");
                                }
                                else
                                {
                                    oTransHRow.ReturnTransID = Configuration.convertNullToInt(dtPOSTransactionRXDetail.Rows[0]["TransID"]);
                                }
                            }
                            else
                            {
                                if (CurrentTransactionType == POSTransactionType.SalesReturn)
                                {
                                    bAlreadyReturned = true;
                                    //clsCoreUIHelper.ShowErrorMsg("Rx#: " + RxNo + "-" + RefillNo + " is already returned, so please scan another Rx.");
                                }
                            }
                        }
                        else
                        {
                            bNeverUsed = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CheckRxPickupDetailPOS(DataRow dr, out bool bAlreadyPickedUp, out bool bAlreadyReturned, out bool bNeverUsed)");
                throw exp;
            }
        }
        #endregion

        #region PRIMEPOS-2699 05-Aug-2019 JY Commented
        //public bool CheckRxPickupDetailPOS(DataTable oRxInfo, out bool isReturn)
        //{
        //    bool isPickedupPOS = true;
        //    bool notYetReturn = false; ;
        //    string TransID = string.Empty;
        //    DataSet DsPOSTransactionRXDetail = null;
        //    DataSet DsPOSTransactionDetail = null;
        //    long RxNo = 0;
        //    int RefillNo = 0;
        //    string sPickupPOS = string.Empty;
        //    string sPickupRX = string.Empty;

        //    try
        //    {

        //        if (oRxInfo != null && oRxInfo.Rows.Count > 0)
        //        {
        //            RxNo = Configuration.convertNullToInt64(oRxInfo.Rows[0]["Rxno"]);
        //            RefillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["nrefill"]);
        //            sPickupPOS = Configuration.convertNullToString(oRxInfo.Rows[0]["PickupPOS"]);
        //            sPickupRX = Configuration.convertNullToString(oRxInfo.Rows[0]["Pickedup"]);

        //            using (var oSvr = new TransDetailRXSvr())
        //            {
        //                DsPOSTransactionRXDetail = oSvr.PopulateList(" WHERE RXNO = " + RxNo + " AND NREFILL = " + RefillNo);
        //                if (DsPOSTransactionRXDetail != null && DsPOSTransactionRXDetail.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (DataRow rxinfo in oRxInfo.Rows)
        //                    {
        //                        foreach (DataRow rxpos in DsPOSTransactionRXDetail.Tables[0].Rows)
        //                        {
        //                            if (rxinfo["RXNO"].ToString() == rxpos["RXNO"].ToString() && rxinfo["NREFILL"].ToString() == rxpos["NREFILL"].ToString()
        //                                && !string.IsNullOrEmpty(rxpos["RETURNTRANSDETAILID"].ToString()) && Convert.ToInt32(rxpos["RETURNTRANSDETAILID"]) > 0)
        //                            {
        //                                isPickedupPOS = false;
        //                            }
        //                            else
        //                            {
        //                                isPickedupPOS = true;
        //                                notYetReturn = true;
        //                                TransID = rxpos["TRANSDETAILID"].ToString();
        //                            }
        //                        }
        //                    }
        //                    if (isPickedupPOS && !notYetReturn && CurrentTransactionType != POSTransactionType.SalesReturn)
        //                    {
        //                        clsCoreUIHelper.ShowErrorMsg("Rx#: " + RxNo + "-" + RefillNo + " was already picked up.");
        //                    }
        //                }
        //                else
        //                {
        //                    isPickedupPOS = false;
        //                }

        //                if (CurrentTransactionType == POSTransactionType.SalesReturn && isPickedupPOS)
        //                {
        //                    using (var oTransDetailSvr = new TransDetailSvr())
        //                    {
        //                        DsPOSTransactionDetail = oTransDetailSvr.PopulateTransDetailId(Convert.ToInt32(Convert.ToInt32(TransID)));
        //                        foreach (DataRow dr in DsPOSTransactionDetail.Tables[0].Rows)
        //                        {
        //                            if (POS_Core.Resources.Configuration.convertNullToInt(dr["RETURNTRANSDETAILID"].ToString()) == 0 && CurrentTransactionType == POSTransactionType.SalesReturn)
        //                            {
        //                                oTransHRow.ReturnTransID = POS_Core.Resources.Configuration.convertNullToInt(dr["TRANSID"].ToString());
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        isReturn = notYetReturn;

        //    }
        //    catch (Exception exp)
        //    {
        //        logger.Fatal(exp, "CheckRxPickupDetailPOS()");
        //        throw exp;
        //    }
        //    return isPickedupPOS;
        //}
        #endregion

        #region PRIMEPOS-2699 05-Aug-2019 JY Added
        public void CheckRxPickupDetailPOS(DataTable oRxInfo, out bool bAlreadyPickedUp, out bool bAlreadyReturned, out bool bNeverUsed)
        {
            bAlreadyPickedUp = false;
            bAlreadyReturned = false;
            bNeverUsed = false;

            try
            {
                if (oRxInfo != null && oRxInfo.Rows.Count > 0)
                {
                    long RxNo = Configuration.convertNullToInt64(oRxInfo.Rows[0]["Rxno"]);
                    int RefillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["nrefill"]);
                    int iPartialFillNo = 0;
                    if (oRxInfo.Columns.Contains("PartialFillNo"))
                        iPartialFillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["PartialFillNo"]);

                    using (var oTransDetailRXSvr = new TransDetailRXSvr())
                    {
                        DataTable dtPOSTransactionRXDetail = null;
                        dtPOSTransactionRXDetail = oTransDetailRXSvr.GetPOSTransactionRXDetailRecord(RxNo, RefillNo, iPartialFillNo);
                        if (dtPOSTransactionRXDetail != null && dtPOSTransactionRXDetail.Rows.Count > 0)
                        {
                            if (Configuration.convertNullToInt(dtPOSTransactionRXDetail.Rows[0]["TransType"]) == 1) //last transaction was sale
                            {
                                if (CurrentTransactionType == POSTransactionType.Sales)
                                {
                                    bAlreadyPickedUp = true;
                                    //clsCoreUIHelper.ShowErrorMsg("Rx#: " + RxNo + "-" + RefillNo + " is already picked up.");
                                }
                                else
                                {
                                    oTransHRow.ReturnTransID = Configuration.convertNullToInt(dtPOSTransactionRXDetail.Rows[0]["TransID"]);
                                }
                            }
                            else
                            {
                                if (CurrentTransactionType == POSTransactionType.SalesReturn)
                                {
                                    bAlreadyReturned = true;
                                    //clsCoreUIHelper.ShowErrorMsg("Rx#: " + RxNo + "-" + RefillNo + " is already returned, so please scan another Rx.");
                                }
                            }
                        }
                        else
                        {
                            bNeverUsed = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CheckRxPickupDetailPOS(DataTable oRxInfo, out bool bAlreadyPickedUp, out bool bAlreadyReturned,  out bool bNeverUsed)");
                throw exp;
            }
        }

        public void ReturnedRxTransDetails(DataTable oRxInfo, out string strRxException, out long transId)
        {
            logger.Trace("ReturnedRxTransDetails(DataTable oRxInfo, out string strRxException) - " + clsPOSDBConstants.Log_Entering);
            strRxException = "";
            transId = 0;

            try
            {
                if (oRxInfo != null && oRxInfo.Rows.Count > 0)
                {
                    long RxNo = Configuration.convertNullToInt64(oRxInfo.Rows[0]["Rxno"]);
                    int RefillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["nrefill"]);
                    using (var oTransDetailRXSvr = new TransDetailRXSvr())
                    {
                        DataTable dt = oTransDetailRXSvr.GetReturnedRxTransDetails(RxNo, RefillNo);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string strDate = string.Empty;
                            transId = Configuration.convertNullToInt64(dt.Rows[0]["TransID"]);
                            try
                            {
                                if (dt.Rows[0]["TransDate"] != null)
                                {
                                    strDate = Convert.ToDateTime(dt.Rows[0]["TransDate"]).ToString();
                                }
                            }
                            catch { }

                            strRxException = RxNo + "-" + RefillNo + ": Rx is already returned on " + strDate;
                            strRxException += "\nTransaction ID : " + transId.ToString() + "\n" +
                                            "User : " + Configuration.convertNullToString(dt.Rows[0]["UserID"]) + "\n" +
                                            "Payment Type : " + Configuration.convertNullToString(dt.Rows[0]["PayTypes"]) + "\n" +
                                            "Station ID : " + Configuration.convertNullToString(dt.Rows[0]["StationID"]);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ReturnedRxTransDetails(DataTable oRxInfo, out string strRxException)");
                throw exp;
            }

            logger.Trace("ReturnedRxTransDetails(DataTable oRxInfo, out string strRxException) - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion

        public DataTable GetRxWithStatus(string ItemCode, string nRefill)
        {
            DataTable oRxInfo = new DataTable();
            try
            {
                isUnBilledRx = 0;   //PRIMEPOS-2398 04-Jan-2021 JY modified
                PharmBL oPharmBL = new PharmBL();
                if (nRefill.Length > 0)
                {
                    oRxInfo = oPharmBL.GetRxsWithStatus(ItemCode, nRefill, "");
                }
                else
                {
                    oRxInfo = oPharmBL.GetRxsWithStatus(ItemCode, "");
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetRxWithStatus()");
                throw exp;
            }
            return oRxInfo;
        }

        //public DataTable GetRxWithStatus(string ItemCode, string nRefill, bool isOnHoldTrans, ref bool isAlreadyProcessRx, ref bool isNotReturned)    //PRIMEPOS-2699 05-Aug-2019 JY Commented
        public DataTable GetRxWithStatus(string ItemCode, string nRefill, bool isOnHoldTrans, ref bool bAlreadyPickedUp, ref bool bAlreadyReturned, ref bool bNeverUsed, string sPartialFillNo)   //PRIMEPOS-2699 05-Aug-2019 JY Added few parameters
        {
            DataTable oRxInfo = new DataTable();
            try
            {
                isUnBilledRx = 0;   //PRIMEPOS-2398 04-Jan-2021 JY modified
                PharmBL oPharmBL = new PharmBL();
                if (nRefill.Length > 0)
                {
                    if (string.IsNullOrWhiteSpace(sPartialFillNo))
                        oRxInfo = oPharmBL.GetRxsWithStatus(ItemCode, nRefill, "");
                    else
                    {
                        int iPartialFillNo = Configuration.convertNullToInt(sPartialFillNo);
                        oRxInfo = oPharmBL.GetRxsWithStatus(ItemCode, nRefill, "", iPartialFillNo);
                    }
                }
                else
                {
                    oRxInfo = oPharmBL.GetRxsWithStatus(ItemCode, "");
                }
                if (oRxInfo.Rows.Count > 0 && !isOnHoldTrans)
                {
                    #region PRIMEPOS-2398 04-Jan-2021 JY Commented
                    //if (oRxInfo.Rows[0]["Status"].ToString() == "U" && Configuration.CPOSSet.FetchUnbilledRx == false)
                    //{
                    //    isUnBilledRx = true;
                    //}
                    //else
                    //{
                    //    isUnBilledRx = false;
                    //}
                    #endregion
                    //if (oRxInfo.Rows[0]["Status"].ToString() == "U") isUnBilledRx = Configuration.CPOSSet.FetchUnbilledRx;   //PRIMEPOS-2398 04-Jan-2021 JY Added

                    CheckRxPickupDetailPOS(oRxInfo, out bAlreadyPickedUp, out bAlreadyReturned, out bNeverUsed);
                }
                if (oRxInfo.Rows.Count > 0 && oRxInfo.Rows[0]["Status"].ToString() == "U") isUnBilledRx = Configuration.CPOSSet.FetchUnbilledRx;   //PRIMEPOS-2398 04-Jan-2021 JY Added //PRIMEPOS-3251-Added condition of rows count
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetRxWithStatus()");
                throw exp;
            }
            return oRxInfo;
        }

        public DataTable GetRxInfo(string itemDescription)
        {
            DataTable oRxInfo = new DataTable();
            try
            {
                PharmBL oPharmBL = new PharmBL();
                long RXNo = 0;  //PRIMEPOS-2515 17-Oct-2018 JY changed RxNo datatype from int to long
                int refillNo = -1;
                int partialFillNo = 0;
                ExtractRXInfoFromDescription(itemDescription, out RXNo, out refillNo, out partialFillNo);
                //Following code is modified by sendUnbilled from false to true Shitaljit(QuicSolv) on 9 Nov 2011
                if (refillNo == -1)
                {
                    oRxInfo = oPharmBL.GetRxs(RXNo.ToString(), true);
                }
                else
                {
                    oRxInfo = oPharmBL.GetRxs(RXNo.ToString(), refillNo.ToString(), true);
                }
                if (oRxInfo.Rows.Count == 0)
                {
                    if (refillNo == -1)
                    {
                        oRxInfo = oPharmBL.GetRxs(RXNo.ToString(), false);
                    }
                    else
                    {
                        oRxInfo = oPharmBL.GetRxs(RXNo.ToString(), refillNo.ToString(), false);
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetRxInfo()");
                throw exp;
            }
            return oRxInfo;
        }

        public void RemoveRX(DataTable RxWithValidClass, DataTable DrugClassInfoCapture, string transDetailId, List<OnholdRxs> lstOnHoldRxs)  //PRIMEPOS-2547 23-Jul-2018 JY Added DrugClassInfoCapture parameter
        {
            try
            {
                TransDetailRXRow transDetailRxRow = oTransDRXData.TransDetailRX.GetRowByID(Convert.ToInt32(transDetailId));
                if (transDetailRxRow != null)
                {
                    foreach (RXHeader oInfo in oRXHeaderList)
                    {
                        RXDetail oRxDetail = oInfo.RXDetails.FindByRX(transDetailRxRow.RXNo, transDetailRxRow.NRefill);

                        //Following if is added By Shitaljit on 27 august 2012
                        //In case of multiple patient if the deleting RX is not of first patient we get oRxDetail = null and while trying to execute
                        //RemoveRxFromUnbilledRxStr(oRxDetail.RXNo.ToString()) it throws error and code get executed so the
                        //Actual RX is not get delete it remains store in the list.

                        //Following if is Added By shitaljit fo pRIMEPOS- 801 for
                        //oBject Refference No Set exception while adding RX item in transaction on 18 Arpil 2013
                        if (oRxDetail != null)
                        {
                            oInfo.RXDetails.Remove(oRxDetail);
                            //Following if is added by shitaljit to resolved object reffreence not set error while deleting RX item. PRIMEPOS - 801
                            if (RxWithValidClass != null)
                            {
                                if (RxWithValidClass.Rows.Count > 0)
                                {
                                    DataRow[] rx = RxWithValidClass.Select("RxNo= " + oRxDetail.RXNo + " AND RefillNo= " + oRxDetail.RefillNo);
                                    foreach (var data in rx)
                                    {
                                        data.Delete();
                                    }
                                }
                            }

                            #region PRIMEPOS-2639 29-Mar-2019 JY Added
                            try
                            {
                                if (lstOnHoldRxs.Count > 0)
                                {
                                    foreach (OnholdRxs objOnholdRxs in lstOnHoldRxs)
                                    {
                                        if (objOnholdRxs.RxNo == oRxDetail.RXNo && objOnholdRxs.NRefill == oRxDetail.RefillNo)
                                        {
                                            lstOnHoldRxs.Remove(objOnholdRxs);
                                            break;
                                        }
                                    }
                                }
                            }
                            catch { }
                            #endregion

                            #region PRIMEPOS-2547 23-Jul-2018 JY Added
                            try
                            {
                                if (DrugClassInfoCapture != null)
                                {
                                    if (DrugClassInfoCapture.Rows.Count > 0)
                                    {
                                        DataRow[] rx = DrugClassInfoCapture.Select("RxNo= " + oRxDetail.RXNo + " AND RefillNo= " + oRxDetail.RefillNo);
                                        foreach (var data in rx)
                                        {
                                            data.Delete();
                                        }
                                    }
                                }
                            }
                            catch { }
                            #endregion

                            //Following if is Added By shitaljit fo pRIMEPOS- 801 for
                            //oBject Refference No Set exception while adding RX item in transaction on 18 Arpil 2013
                            if (oInfo != null)
                            {
                                if (oInfo.RXDetails.Count == 0)
                                {
                                    oRXHeaderList.Remove(oInfo);
                                }
                            }
                            RemoveRxFromUnbilledRxStr(oRxDetail.RXNo.ToString());
                            oTransDRXData.TransDetailRX.Rows.Remove(transDetailRxRow);
                            break;
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "RemoveRX()");
                throw exp;
            }
        }

        public void RemoveRxFromUnbilledRxStr(string rxNo)
        {
            logger.Trace("RemoveRxFromUnbilledRxStr() - " + clsPOSDBConstants.Log_Entering);
            //Added by SRT(Abhishek) Date : 24 Aug 2009
            //Added this logic to remove RxNo from the string which will be populated when click on Payment button
            if (unbilledRx.Trim() != string.Empty)
            {
                int count = 0;
                string[] unBilledRxs = unbilledRx.Split(new char[] { ',' });
                unbilledRx = string.Empty;
                foreach (string rx in unBilledRxs)
                {
                    if (rx.Trim() != rxNo.Trim())
                    {
                        if (count == 0)
                            unbilledRx += rx;
                        else
                            unbilledRx += "," + rx;
                        count++;
                    }
                    else
                        countUnBilledRx--;
                }
            }
            //End of Added by SRT(Abhishek)
            logger.Trace("RemoveRxFromUnbilledRxStr() - " + clsPOSDBConstants.Log_Exiting);
        }

        public long GetBatchIDFromRxno(long rxno, int nrefill)
        {

            long lbatchNo;
            try
            {
                PharmBL oPharmBL = new PharmBL();
                lbatchNo = oPharmBL.GetBatchIDFromRxno(rxno, nrefill);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransactionByTransactionId()");
                throw exp;
            }
            return lbatchNo;
        }

        public bool CheckUnpickedRxLocally(DataTable oRxInfo, RXHeader oRXHeader = null)
        {
            bool retVal = false;
            try
            {
                DataSet DsPOSTransactionRXDetail = null;
                TransDetailRXSvr oSvr = new TransDetailRXSvr();

                List<DataRow> oRxInfoPick = new List<DataRow>();
                List<DataRow> FinalRxInfo = new List<DataRow>();

                string prevRxNo = string.Empty;
                string pRxNoRefill = string.Empty;

                foreach (DataRow dr in oRxInfo.Rows)
                {

                    if (oRXHeader != null)
                    {
                        if (oRXHeader.RXDetails != null && oRXHeader.RXDetails.Count > 0 && oRXHeader.RXDetails[0].RXNo == Convert.ToInt64(dr["RXNO"].ToString()))
                        {
                            oRxInfoPick.Add(dr);
                            continue;
                        }
                    }

                    if (oRxInfo.Columns.Contains("PartialFillNo"))
                        DsPOSTransactionRXDetail = oSvr.PopulateList(" WHERE RXNO = " + dr["RXNO"].ToString() + " AND NREFILL = " + dr["NREFILL"].ToString() + " AND PartialFillNo = " + dr["PartialFillNo"].ToString());
                    else
                        DsPOSTransactionRXDetail = oSvr.PopulateList(" WHERE RXNO = " + dr["RXNO"].ToString() + " AND NREFILL = " + dr["NREFILL"].ToString());
                    foreach (DataRow ds in DsPOSTransactionRXDetail.Tables[0].Rows)
                    {
                        if (dr["RXNO"].ToString() == ds["RXNO"].ToString() && dr["NREFILL"].ToString() == ds["NREFILL"].ToString())
                        {
                            if (ds["RXNO"].ToString() == prevRxNo && ds["NREFILL"].ToString() == pRxNoRefill && Configuration.convertNullToInt(ds["ReturnTransDetailID"].ToString()) != 0)
                            {
                                if (oRxInfoPick.Contains(dr))
                                {
                                    oRxInfoPick.Remove(dr);
                                }
                                continue;
                            }
                            prevRxNo = ds["RXNO"].ToString();
                            pRxNoRefill = ds["NREFILL"].ToString();
                            oRxInfoPick.Add(dr);
                        }
                    }
                }

                if (oRxInfoPick != null && oRxInfoPick.Count > 0)
                {
                    FinalRxInfo = oRxInfoPick.Distinct().ToList();
                }

                if (!Configuration.CInfo.AllowMultipleRXRefillsInSameTrans)
                {
                    foreach (var dr in FinalRxInfo)
                    {
                        oRxInfo.Rows.Remove(dr);
                    }
                }

                if (oRxInfo == null || oRxInfo.Rows.Count == 0)
                {
                    retVal = true;
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CheckUnpickedRxLocally()");
                throw exp;
            }
            return retVal;
        }

        public DataTable GetRxNotes(string sRxNo)
        {
            DataTable dtRxNotes = null;
            try
            {
                PharmBL oPharmBL = new PharmBL();
                dtRxNotes = oPharmBL.GetRxNotes(sRxNo);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetRxNotes()");
                throw exp;
            }
            return dtRxNotes;
        }
        public DataTable GetBatchStatusfromView(string batchCode)
        {
            DataTable oBatchInfo = null;
            try
            {
                PharmBL oPharmBL = new PharmBL();
                oBatchInfo = oPharmBL.GetBatchStatusfromView(batchCode);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetBatchStatusfromView()");
                throw exp;
            }
            return oBatchInfo;
        }
        public DataTable GetPatientNotes(string sPatNo)
        {
            DataTable dtPatNote = null;
            try
            {
                PharmBL oPharmBL = new PharmBL();
                dtPatNote = oPharmBL.GetPatientNotes(sPatNo);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetPatientNotes()");
                throw exp;
            }
            return dtPatNote;
        }

        public DataTable GetPatient(string sPatNo)
        {
            DataTable dtPatient = null;
            try
            {
                PharmBL oPharmBL = new PharmBL();
                dtPatient = oPharmBL.GetPatient(sPatNo);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetPatient()");
                throw exp;
            }
            return dtPatient;
        }

        public void isValidDrugClassRx(string rxno, string refill, string Class, ref DataTable RxWithValidClass, string PartialFillNo)
        {
            string DrugClass = "";    //PRIMEPOS-2547 24-Jul-2018 JY Added
            switch (Class.Trim())
            {
                case "2":
                case "3":
                case "4":
                case "5":
                    #region PRIMEPOS-2547 24-Jul-2018 JY Commented
                    //{
                    //    if (RxWithValidClass != null)
                    //    {
                    //        RxWithValidClass.Rows.Add(rxno, refill);
                    //    }
                    //    else
                    //    {
                    //        RxWithValidClass = new DataTable();
                    //        RxWithValidClass.Columns.Add("RxNo", typeof(string));
                    //        RxWithValidClass.Columns.Add("RefillNo", typeof(string));
                    //        RxWithValidClass.Rows.Add(rxno, refill);
                    //    }
                    //}
                    #endregion
                    DrugClass = Class.Trim();
                    break;
            }
            #region PRIMEPOS-2547 24-Jul-2018 JY Added
            if ((Configuration.CPOSSet.ControlByID == 1 && DrugClass != "") || (Configuration.CPOSSet.ControlByID == 2))
            {
                if (RxWithValidClass != null)
                {
                    RxWithValidClass.Rows.Add(rxno, refill, Class.Trim(), PartialFillNo);
                }
                else
                {
                    RxWithValidClass = new DataTable();
                    RxWithValidClass.Columns.Add("RxNo", typeof(string));
                    RxWithValidClass.Columns.Add("RefillNo", typeof(string));
                    RxWithValidClass.Columns.Add("DrugClass", typeof(string));
                    RxWithValidClass.Columns.Add("PartialFillNo", typeof(string));
                    RxWithValidClass.Rows.Add(rxno, refill, DrugClass, PartialFillNo);
                }
            }
            #endregion
        }

        public bool InsToIgnoreCopay(string InsCode, string billtype, decimal copayAmount)
        {
            logger.Trace("InsToIgnoreCopay() - " + clsPOSDBConstants.Log_Entering);
            bool retVal = false;
            if (Configuration.CPOSSet.RXInsToIgnoreCopay.Trim().Length > 0)
            {
                string[] sIns = Configuration.CPOSSet.RXInsToIgnoreCopay.Split(char.Parse(","));
                for (int i = 0; i < sIns.Length; i++)
                {
                    //Prog1 21Apr2010
                    //Changed logic here because a minimum price parameter is introduced

                    string insName = sIns[i];
                    decimal minimumPriceToIgnore = 0;

                    if (sIns[i].IndexOf("/") > 0)
                    {
                        insName = sIns[i].Split('/')[0];
                        minimumPriceToIgnore = Configuration.convertNullToDecimal(sIns[i].Split('/')[1].Trim());
                    }

                    if ((insName.Trim().ToUpper() == InsCode.Trim().ToUpper()) || (insName.Trim().ToUpper() == "ALL"))
                    {
                        if (billtype.ToUpper().Trim() == "I")
                        {
                            if (minimumPriceToIgnore == 0 || copayAmount <= minimumPriceToIgnore)
                            {
                                retVal = true;
                            }
                        }
                        break;
                    }
                }
            }
            logger.Trace("InsToIgnoreCopay() - " + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        public void SetTransactionRowForUnPickedRx(DataRow oRXRow, int defaultQty)
        {

            oTDRow = oTransDData.TransDetail.AddRow(clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, defaultQty, 0, 0, 0, 0, 0, "RX", "");

            oTDRow.Category = "F";

            if (InsToIgnoreCopay(oRXRow["Pattype"].ToString(), oRXRow["billtype"].ToString(), Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString())) == false)
            {
                if (CurrentTransactionType == POSTransactionType.SalesReturn)
                {
                    oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString()) * -1;
                }
                else
                {
                    oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(oRXRow["PatAmt"].ToString());
                }
                oTDRow.Price = oTDRow.ExtendedPrice;
            }
            else
            {
                oTDRow.ExtendedPrice = 0;
            }
            oTDRow.ItemDescription = oRXRow["RXNo"].ToString() + "-" + oRXRow["nRefill"].ToString() + "-" + oRXRow["DETDRGNAME"].ToString();
            oTDRow.ItemDescriptionMasked = MaskDrugName(oTDRow.ItemID.ToString(), oTDRow.ItemDescription.ToString());  //PRIMEPOS-3130
            oTDRow.QTY = defaultQty;

            SetRowTrans(oTDRow);

            if (Configuration.CInfo.CheckRXItemsForIIAS == true)
            {
                Item oItem = new Item();
                oTDRow.IsIIAS = oItem.IsIIASItem(oRXRow["ndc"].ToString());
            }
            else
            {
                oTDRow.IsIIAS = true;
            }

            //To Identify whether this row was Rx or not.
            oTDRow.IsRxItem = true;
        }

        public RXHeader SetRxHeaderForUnPickedRx(DataRow oRXRow, out RXDetail oRXDetail)
        {

            RXHeader oRXHeader = null;
            PharmBL oPharmBL = new PharmBL();
            try
            {

                if (oRXHeaderList.Count > 0)
                {
                    oRXHeader = oRXHeaderList.FindByPatient(oRXRow["PatientNo"].ToString());
                }
                if (oRXHeader == null)
                {
                    oRXHeader = new RXHeader();
                    oRXHeader.PatientNo = oRXRow["PatientNo"].ToString();
                    oRXHeader.InsType = oRXRow["BillType"].ToString();
                    oRXHeader.RXSignature = "";
                    oRXHeader.NOPPSignature = "";
                    oRXHeader.bBinarySign = null;
                    oRXHeader.PatientState = "";
                    oRXHeader.FamilyID = Configuration.convertNullToString(oRXRow["FamilyID"]);    //Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added

                    //PRIMEPOS-2981 28-Jun-2021 JY Added logic to handle NULL
                    DataTable oTable = oPharmBL.GetPatient(Configuration.convertNullToString(oRXHeader.PatientNo));
                    if (oTable.Rows.Count > 0)
                    {
                        oRXHeader.PatientName = Configuration.convertNullToString(oTable.Rows[0]["Lname"]).Trim() + ", " + Configuration.convertNullToString(oTable.Rows[0]["Fname"]).Trim();
                        oRXHeader.PatientAddr = Configuration.convertNullToString(oTable.Rows[0]["addrstr"]).Trim();
                        oRXHeader.PatientState = Configuration.convertNullToString(oTable.Rows[0]["addrst"]).Trim();
                    }

                    oTable = new DataTable();
                    oTable = oPharmBL.GetPrivackAck(Configuration.convertNullToString(oRXHeader.PatientNo));
                    if (oTable.Rows.Count > 0)
                    {
                        if (Configuration.convertNullToString(oTable.Rows[0]["PatAccept"]).Trim().ToUpper() == "N")
                        {
                            oRXHeader.isNOPPSignRequired = true;
                        }
                        else
                        {
                            try
                            {
                                if (DateTime.Parse(oTable.Rows[0]["datesigned"].ToString()).AddMonths(Configuration.CInfo.PrivacyExpiry) > DateTime.Now)
                                {
                                    oRXHeader.isNOPPSignRequired = false;
                                }
                                else
                                {
                                    oRXHeader.isNOPPSignRequired = true;
                                }
                            }
                            catch
                            {
                                oRXHeader.isNOPPSignRequired = true;
                            }
                        }
                    }
                    else
                    {
                        oRXHeader.isNOPPSignRequired = true;
                    }
                    oRXHeaderList.Add(oRXHeader);
                }

                oRXDetail = new RXDetail();
                oRXDetail.RXNo = Convert.ToInt64(oRXRow["RXNo"].ToString());
                oRXDetail.RefillNo = Convert.ToInt16(oRXRow["nrefill"].ToString());
                oRXDetail.DrugName = oRXRow["drgName"].ToString();
                if (oRXRow.Table.Columns.Contains("PartialFillNo"))
                    oRXDetail.PartialFillNo = Configuration.convertNullToShort(oRXRow["PartialFillNo"]);
                if (oRXRow["datef"].ToString().Length >= 10)
                {
                    oRXDetail.RxDate = Convert.ToDateTime(oRXRow["datef"]).ToShortDateString();
                }
                else
                {
                    oRXDetail.RxDate = "";
                }
                oRXHeader.RXDetails.Add(oRXDetail);


            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SetRxHeaderForUnPickedRx()");
                throw exp;
            }
            return oRXHeader;
        }


        #endregion

        #region Customer
        public void SaveToken()
        {
            try
            {
                if (oCustomerRow.AccountNumber > 0 && Configuration.CInfo.SaveCCToken == true)  //Sprint-23 - PRIMEPOS-2315 21-Jun-2016 JY Added SaveCCToken condition
                {
                    bool persist = false;
                    CCCustomerTokInfoData ocustomerTokData = new CCCustomerTokInfoData();
                    CCCustomerTokInfo tokinfo = new CCCustomerTokInfo();
                    ocustomerTokData = tokinfo.GetTokenByCustomerandProcessor(oCustomerRow.CustomerId);

                    if (oPOSTransPaymentData != null && oPOSTransPaymentData.Tables.Count > 0 && oPOSTransPaymentData.Tables[0].Rows.Count > 0)
                    {
                        int EntryID = 0;//PRIMEPOS-3189
                        foreach (POSTransPaymentRow opayRow in oPOSTransPaymentData.Tables[0].Rows)
                        {
                            if (opayRow.Tokenize)   //PRIMEPOS-3145 28-Sep-2022 JY Added if condition
                            {
                                if (opayRow.ProfiledID.Length > 0 && opayRow.TransTypeDesc.Length > 0)//&& opayRow.pay
                                {
                                    string last4 = string.Empty;
                                    if (opayRow.PaymentProcessor == "WORLDPAY")
                                    {
                                        string[] temp;
                                        string[] delimiter = new string[] { "|" };
                                        temp = opayRow.RefNo.Split(delimiter, StringSplitOptions.None);
                                        if (temp.Length > 0)
                                        {
                                            last4 = temp[0].Replace("*", string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        last4 = opayRow.CCName.Replace("*", string.Empty);
                                    }

                                    bool cardExists = false;
                                    if (ocustomerTokData.Tables.Count > 0 && ocustomerTokData.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (CCCustomerTokInfoRow otokRow in ocustomerTokData.CCCustomerTokInfo.Rows)//ocustomerTokData.Tables[0].Rows
                                        {
                                            if (otokRow.Last4 == last4 && otokRow.CardType == opayRow.TransTypeDesc)
                                            {
                                                cardExists = true;
                                                otokRow.ProfiledID = opayRow.ProfiledID;
                                                otokRow.EntryType = opayRow.EntryMethod;
                                                otokRow.TokenDate = opayRow.TransDate; //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added
                                                persist = true;
                                            }
                                        }
                                    }
                                    if (!cardExists)
                                    {
                                        CCCustomerTokInfoRow orow = (CCCustomerTokInfoRow)ocustomerTokData.CCCustomerTokInfo.NewRow();
                                        CCCustomerTokInfoTable otable = new CCCustomerTokInfoTable();
                                        orow.EntryID = EntryID++;//PRIMEPOS-3189
                                        //if (oCustomerRow.SaveCardProfile == true) //PRIMEPOS-3145 28-Sep-2022 JY Commented
                                        //{
                                        orow.CustomerID = oCustomerRow.CustomerId;
                                        orow.CardType = opayRow.TransTypeDesc;
                                        orow.Last4 = last4;
                                        orow.ProfiledID = opayRow.ProfiledID;
                                        orow.Processor = opayRow.PaymentProcessor;
                                        orow.EntryType = opayRow.EntryMethod;
                                        orow.TokenDate = opayRow.TransDate;
                                        orow.ExpDate = ExtractCreditCardExpiryDate(opayRow.RefNo);  //PRIMEPOS-2612 04-Dec-2018 JY Added
                                        ocustomerTokData.CCCustomerTokInfo.AddRow(orow, true);
                                        persist = true;
                                        //}
                                    }
                                }
                            }
                        }
                    }

                    if (persist)
                    {
                        tokinfo.Persist(ocustomerTokData);
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SaveToken()");
                throw exp;
            }
        }

        #region PRIMEPOS-2570 08-Jun-2020 JY Commented
        //public CustomerData GetChargeCustomer(DataTable ChargeAccount)
        //{
        //    CustomerData chargeCustomer = null;
        //    try
        //    {

        //        if (ChargeAccount != null && ChargeAccount.Rows.Count > 0)
        //        {
        //            long acctno = 0;
        //            try
        //            {
        //                acctno = Convert.ToInt64(ChargeAccount.Rows[0]["ACCT_NO"]);
        //            }
        //            catch (Exception ex1)
        //            {
        //                acctno = -1;
        //            }

        //            DataSet oDsPat = null;
        //            if (acctno > 0)
        //            {
        //                oDsPat = clsCoreHouseCharge.GetPatientByChargeAccountNumber(acctno);
        //            }

        //            if (oDsPat != null && oDsPat.Tables.Count > 0 && oDsPat.Tables[0].Rows.Count > 0)
        //            {
        //                int patno = 0;
        //                try
        //                {
        //                    patno = Convert.ToInt32(oDsPat.Tables[0].Rows[0]["PATIENTNO"]);
        //                }
        //                catch (Exception ex1)
        //                {
        //                    patno = -1;
        //                }
        //                if (patno > 0)
        //                {
        //                    using (CustomerSvr ocstmsvr = new CustomerSvr())
        //                    {
        //                        chargeCustomer = ocstmsvr.GetCustomerByPatientNo(patno);
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        logger.Fatal(exp, "GetChargeCustomer()");
        //        throw exp;
        //    }
        //    return chargeCustomer;
        //}
        #endregion

        #region PRIMEPOS-2570 08-Jun-2020 JY Added
        public CustomerData GetChargeCustomer(int PatientNo)
        {
            CustomerData chargeCustomer = null;
            try
            {
                using (CustomerSvr ocstmsvr = new CustomerSvr())
                {
                    chargeCustomer = ocstmsvr.GetCustomerByPatientNo(PatientNo);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetChargeCustomer()");
                throw exp;
            }
            return chargeCustomer;
        }
        #endregion

        public string GetCustomerId(string condn)
        {
            string strCode = "";
            try
            {
                Customer oCustomer = new Customer();
                CustomerData oData = new CustomerData();
                oData = oCustomer.PopulateList(condn);
                if (oData.Customer.Rows.Count > 0)
                {
                    CustomerRow oCustRow = (CustomerRow)oData.Customer.Rows[0];
                    strCode = oCustRow.CustomerId.ToString();

                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetCustomerId()");
                throw exp;
            }
            return strCode;

        }
        public string GetToolKeyDescForCustomer(string key)
        {
            string result = "";

            switch (key)
            {
                case "C":
                    result = "F6=>Save Card Profile";
                    break;
                case "N":
                    result = "F11=>Customer Notes";
                    break;
                case "H":
                    result = "F10=>Trans History";
                    break;
                case "S":
                    result = "F4=>search Customer";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        public string GetCustomerId(string sCode, bool bByAcctNo)
        {
            CustomerData oCustdata = new CustomerData();
            CustomerRow oCustRow = null;
            string strCode = "";
            try
            {
                if (bByAcctNo == true)
                {
                    oCustdata = PopulateCustomer(sCode);
                }
                else
                {
                    oCustdata = GetCustomerByID(Configuration.convertNullToInt(sCode));
                }
                if (oCustdata.Tables[0].Rows.Count > 0)
                {
                    oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                    strCode = oCustRow.CustomerId.ToString();
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetCustomer()");
                throw Ex;
            }
            return strCode;
        }

        public void PersistCustomer(CustomerData oData, bool ignoreValidation)
        {
            try
            {
                Customer oCustomer = new Customer();
                oCustomer.Persist(oData, ignoreValidation);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PersistCustomer()");
                throw exp;
            }
        }
        public CustomerData CreateCustomerDSFromPatientDS(DataSet oDs, bool newPatientsOnly)
        {
            CustomerData oData = null;
            try
            {
                Customer oCustomer = new Customer();
                oData = oCustomer.CreateCustomerDSFromPatientDS(oDs, newPatientsOnly);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CreateCustomerDSFromPatientDS()");
                throw exp;
            }
            return oData;
        }

        public DataSet IsCustomerChangedInPrimeRX(CustomerData oData, int tempPatNo, out bool isCustomerChanged)
        {
            isCustomerChanged = false;
            CustomerRow orow = oData.Customer[0];
            DataSet oDs = null;
            try
            {
                if (orow.PatientNo > 0 && Configuration.CPOSSet.UsePrimeRX == true)
                {
                    if (orow.PatientNo != tempPatNo)
                    {
                        MMSChargeAccount.ContAccount oAcct = new MMSChargeAccount.ContAccount();
                        oAcct.GetPatientByCode(oData.Customer[0].PatientNo, out oDs);
                        if (oDs.Tables[0].Rows.Count > 0)
                        {
                            Customer oCustomer = new Customer();
                            if (oCustomer.IsCustomerChangedInPharmacy(oData.Customer[0], oDs.Tables[0].Rows[0]) == true)
                            {
                                isCustomerChanged = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IsCustomerChangedInPrimeRX()");
                throw exp;
            }
            return oDs;
        }

        public CustomerData GetCustomerByContactNumber(string sCustContactnumber)
        {
            CustomerData oData = null;
            try
            {
                Customer oCustomer = new Customer();
                oData = oCustomer.PopulateListByCotactNumber(sCustContactnumber);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetCustomerByContactNumber()");
                throw exp;
            }
            return oData;
        }

        public CustomerData GetCustomerByID(int customerID)
        {
            CustomerData oData = null;
            try
            {
                Customer oCustomer = new Customer();
                oData = oCustomer.GetCustomerByID(customerID);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetCustomerByID()");
                throw exp;
            }
            return oData;
        }
        public CustomerNotesData GetCustomerNotesByCustId(int customerID, bool activeOnly)
        {
            CustomerNotesData oData = null;
            try
            {
                CustomerNotes oCNotes = new CustomerNotes();
                oData = oCNotes.Populate(customerID, activeOnly);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetCustomerNotesByCustId()");
                throw exp;
            }
            return oData;
        }

        public CustomerData PopulateCustomer(string code)
        {
            CustomerData oData = null;
            try
            {
                Customer oCustomer = new Customer();
                oData = oCustomer.Populate(code);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateCustomer()");
                throw exp;
            }
            return oData;
        }

        public CustomerData PopulateCustomer(System.String Customercode, bool isActive)
        {
            CustomerData oCustomerData;
            try
            {
                Customer oCustomer = new Customer();
                oCustomerData = oCustomer.Populate("-1", true);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateCustomer()");
                throw exp;
            }
            return oCustomerData;
        }

        public CustomerData GetCustomerByPatientNo(int patientNo)
        {
            CustomerData oData = null;
            try
            {
                Customer oCustomer = new Customer();
                oData = oCustomer.GetCustomerByPatientNo(patientNo);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetCustomerByPatientNo()");
                throw exp;
            }
            return oData;
        }

        public CustomerData PopulateCustomerList(string whereCondition)
        {
            CustomerData oData = null;
            try
            {
                Customer oCustomer = new Customer();
                oData = oCustomer.PopulateList(whereCondition);

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateList()");
                throw exp;
            }
            return oData;
        }

        public bool CheckAgeLimit(int AgeLimit, string dob, int cmbVerificationIndex, bool isfrmPOSPayAuthNo, ref bool reDisplayID)
        {
            bool isUnder = false;
            string aMonth, aDay, aYear, Dob;
            try
            {
                if (!string.IsNullOrEmpty(dob))
                {
                    // POS_Core.Business_Tier.DL = new Business_Tier.DL()
                    DateTime today = DateTime.Today;
                    string cDate = today.Year.ToString() + string.Format("{0:00}", today.Month) + string.Format("{0:00}", today.Day);

                    string dobchk = dob.Substring(0, 2); //use to check if the Year is first or the month

                    if (Convert.ToInt32(dobchk) < 13) // if month first reconstruct the string to the YYYYMMDD format
                    {
                        aMonth = dob.Substring(0, 2); //Month
                        aDay = dob.Substring(2, 2); //Day
                        aYear = dob.Substring(4, 4); //Year
                        Dob = aYear + string.Format("{0:00}", aMonth) + string.Format("{0:00}", aDay); //DOB with Year, Month and Day format
                    }
                    else
                    {
                        Dob = dob; //Year is first
                    }

                    Int64 age = (Convert.ToInt64(cDate) - Convert.ToInt64(Dob)) / (10000); //Get the age up to today so if they are 1 day before 21 it will show 20
                    if (age < AgeLimit)
                    {
                        isUnder = true; //If below the Age Limit
                    }

                    if (age <= 13 && cmbVerificationIndex == 0)
                    {
                        clsCoreUIHelper.ShowErrorMsg("Please enter a valid Date Of Birth \nthat is on the Driver License.");
                        reDisplayID = true;
                    }
                    else
                    {
                        reDisplayID = false;
                    }
                }
                else
                {
                    if (!isfrmPOSPayAuthNo)
                    {
                        if (cmbVerificationIndex == 0)
                        {
                            clsCoreUIHelper.ShowErrorMsg("Please enter a valid Date Of Birth.");
                        }
                        else
                        {
                            clsCoreUIHelper.ShowErrorMsg("The ID type is not a valid scan format. \nPlease select Driver License as ID Type.");
                        }
                        reDisplayID = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsCoreUIHelper.ShowErrorMsg(ex.Message);
            }
            return isUnder;
        }

        #endregion Customer

        #region Item

        public void ValidateItemQTY(string qty, bool isItemInfoChanged, bool isQtyChange, bool isEditRow, out int oldQty, ref decimal GroupPrice, bool bApplyDiscount = true)
        {

            oldQty = oTDRow.QTY;

            try
            {


                Validate_Qty(qty);
                //Added by Ravindra for Sale limit 22 March 2013

                TransDetailRow TempRow = oTDRow;

                int quantity = GetExistingQuantity(TempRow, oTransDData);
                ItemData oItemData;
                ItemRow oItemRow = null;
                oItemData = PopulateItem(TempRow.ItemID);
                if (!isQtyChange)
                {
                    this.oTDRow.QTY = Convert.ToInt32(qty);
                    if (bApplyDiscount) //PRIMEPOS-2514 09-May-2018 JY Added bApplyDiscount to control changed discount behavior when we change it through "Change Item Info" option
                        oTDRow.Discount = CalculateDiscount(oTDRow.ItemID, oTDRow.QTY, oTDRow.Price);   //Sprint-21 - PRIMEPOS-225 11-Feb-2016 JY line discount is not considered when we change the item price/discount of item by "Change Item Info".
                }

                if (oItemData.Item.Rows.Count > 0)
                {
                    oItemRow = oItemData.Item[0];

                    if (!(quantity == 0 && this.oTDRow.QTY < oItemRow.SaleLimitQty))
                    {
                        #region Sprint-27 - PRIMEPOS-2413 15-Sep-2017 JY Added
                        if (oItemData.Item.Rows.Count > 0 &&
                           oItemRow.isOnSale && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)
                        {
                            int iSalelimitQty = Configuration.convertNullToInt(oItemRow.SaleLimitQty);
                            if (Configuration.CInfo.GroupTransItems == true)
                            {
                                if (oTDRow.QTY <= iSalelimitQty || iSalelimitQty == 0) //apply sale price for all quantity
                                {
                                    oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oItemRow.OnSalePrice, 2);
                                    oTDRow.IsPriceChanged = true;
                                    oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                }
                                else
                                {
                                    decimal FinalPrice = ((iSalelimitQty * Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero) + ((oTDRow.QTY - iSalelimitQty) * oTDRow.OrignalPrice))) / oTDRow.QTY;    //Sprint-27 - PRIMEPOS-2413 06-Nov-2017 JY Added to get exact price of item
                                    oTDRow.NonComboUnitPrice = oTDRow.Price = FinalPrice;
                                    oTDRow.IsPriceChanged = true;
                                    oTDRow.IsOnSale = false; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                }
                            }
                            else
                            {
                                int existingQty = 0;
                                if (!isEditRow)
                                {
                                    existingQty = GetExistingQuantity(oTDRow, oTransDData) + oTDRow.QTY;
                                    if (existingQty <= iSalelimitQty || iSalelimitQty == 0) //apply sale price for all quantity
                                    {
                                        oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero);
                                        oTDRow.IsPriceChanged = true;
                                        oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                    }
                                    else
                                    {
                                        oTDRow.IsOnSale = false; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                        int tempExistingQuantity = GetExistingQuantity(oTDRow, oTransDData);
                                        if (tempExistingQuantity < iSalelimitQty)
                                        {
                                            decimal TempPrice = (((iSalelimitQty - tempExistingQuantity) * Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero) + ((oTDRow.QTY - (iSalelimitQty - tempExistingQuantity)) * oTDRow.OrignalPrice))) / oTDRow.QTY;   //Sprint-27 - PRIMEPOS-2413 06-Nov-2017 JY Added to get exact price of item
                                            oTDRow.NonComboUnitPrice = oTDRow.Price = TempPrice;
                                            oTDRow.IsPriceChanged = true;
                                        }
                                        else
                                        {
                                            oTDRow.NonComboUnitPrice = oTDRow.Price = oTDRow.OrignalPrice;
                                            oTDRow.IsPriceChanged = true;
                                        }
                                    }
                                }
                                else
                                {
                                    existingQty = GetExistingQuantity(oTDRow, oTransDData);
                                    if (existingQty <= iSalelimitQty || iSalelimitQty == 0) //apply sale price for all quantity
                                    {
                                        oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero);
                                        oTDRow.ExtendedPrice = oTDRow.QTY * oTDRow.Price;
                                        oTDRow.IsPriceChanged = true;
                                        oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                    }
                                    else
                                    {
                                        int ProcessedQty = 0;
                                        //process current row
                                        if (ProcessedQty + oTDRow.QTY <= iSalelimitQty || iSalelimitQty == 0)
                                        {
                                            oTDRow.NonComboUnitPrice = oTDRow.Price = Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero);
                                            oTDRow.ExtendedPrice = oTDRow.QTY * oTDRow.Price;
                                            oTDRow.IsOnSale = true;    //PRIMEPOS-2907 13-Oct-2020 JY Added
                                        }
                                        else
                                        {
                                            if (ProcessedQty < iSalelimitQty)
                                            {
                                                //we need to apply sale price for few quantity
                                                decimal TempPrice = (((iSalelimitQty - ProcessedQty) * Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero) + ((oTDRow.QTY - (iSalelimitQty - ProcessedQty)) * oTDRow.OrignalPrice))) / oTDRow.QTY;   //Sprint-27 - PRIMEPOS-2413 06-Nov-2017 JY Added to get exact price of item
                                                oTDRow.NonComboUnitPrice = oTDRow.Price = TempPrice;
                                                oTDRow.ExtendedPrice = oTDRow.QTY * oTDRow.Price;
                                                oTDRow.IsOnSale = false; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                            }
                                            else
                                            {
                                                oTDRow.NonComboUnitPrice = oTDRow.Price = oTDRow.OrignalPrice;
                                                oTDRow.ExtendedPrice = oTDRow.QTY * oTDRow.Price;
                                                oTDRow.IsOnSale = false; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                            }
                                        }
                                        ProcessedQty += oTDRow.QTY;

                                        //process other rows
                                        foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                                        {
                                            if (oTDRow.ItemID == oRow.ItemID && oTDRow.TransDetailID != oRow.TransDetailID)
                                            {
                                                if (ProcessedQty + oRow.QTY <= iSalelimitQty || iSalelimitQty == 0)
                                                {
                                                    oRow.NonComboUnitPrice = oRow.Price = Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero);
                                                    oRow.ExtendedPrice = oRow.QTY * oRow.Price;
                                                    oTDRow.IsOnSale = true; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                                }
                                                else
                                                {
                                                    oTDRow.IsOnSale = false; //PRIMEPOS-2907 13-Oct-2020 JY Added
                                                    if (ProcessedQty < iSalelimitQty)
                                                    {
                                                        //we need to apply sale price for few quantity
                                                        decimal TempPrice = (((iSalelimitQty - ProcessedQty) * Math.Round(oItemRow.OnSalePrice, 2, MidpointRounding.AwayFromZero) + ((oRow.QTY - (iSalelimitQty - ProcessedQty)) * oRow.OrignalPrice))) / oRow.QTY; //Sprint-27 - PRIMEPOS-2413 06-Nov-2017 JY Added to get exact price of item
                                                        oRow.NonComboUnitPrice = oRow.Price = TempPrice;
                                                        oRow.ExtendedPrice = oRow.QTY * oRow.Price;
                                                    }
                                                    else
                                                    {
                                                        oRow.NonComboUnitPrice = oRow.Price = oRow.OrignalPrice;
                                                        oRow.ExtendedPrice = oRow.QTY * oRow.Price;
                                                    }
                                                }
                                                ProcessedQty += oRow.QTY;
                                                string sTaxCodes1 = string.Empty;
                                                if (IsItemTaxableForTrasaction(oTDTaxData, oRow.ItemID, out sTaxCodes1, oRow.TransDetailID) == true)
                                                {
                                                    //EditTax(oRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl);
                                                    string tempItedId = oRow.ItemID;
                                                    TaxCodes oTaxCodes = new TaxCodes();
                                                    TaxCodesData oTaxCodesData;
                                                    oTaxCodesData = oTaxCodes.PopulateList(" WHERE TaxID IN " + sTaxCodes1);
                                                    CalculateTax(oRow, oTaxCodesData, oTDTaxData);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                //Till here Add  by Ravindra for Sale Limit
                //if (oTransactionType==POSTransactionType.SalesReturn)
                //	this.oTDRow.TaxAmount=-1*this.oTDRow.TaxAmount;

                CalcExtdPrice(this.oTDRow);
                //Commented By Amit Date 27May 2011 (To calculate tax after discount is applied follwing lines added after discount calculation)
                //if (oTDRow.TaxCode.Trim() != "")
                //    FKEdit(oTDRow.TaxCode, clsPOSDBConstants.TaxCodes_tbl, false);
                //Itemdictionary.Add(
                int nonGroupQty = 0;

                if (oTDRow.Discount > 0)
                {
                    //CheckGroupPricing(oTDRow.ItemID, oldQty, oTDRow.Price, out nonGroupQty);  //PRIMEPOS-3098 20-Jun-2022 JY Commented
                    CheckGroupPricing(oTDRow.ItemID, this.oTDRow.QTY, oTDRow.Price, out nonGroupQty, oTDRow.OrignalPrice);   //PRIMEPOS-3098 20-Jun-2022 JY Added oTDRow.OrignalPrice
                    if (nonGroupQty == 0 && CalculateDiscount(oTDRow.ItemID, nonGroupQty, oTDRow.Price) == oTDRow.Discount) //PRIMEPOS-2514 08-May-2018 JY discount should be removed for group pricing, also the older code removed discount for all item when we edited item info 
                    {
                        oTDRow.Discount = 0;
                    }
                }

                if (Configuration.CInfo.GroupTransItems == true)
                {
                    if (bApplyDiscount) //PRIMEPOS-2514 21-May-2018 JY Added bApplyDiscount to control changed discount behavior when we change it through "Change Item Info" option
                        oTDRow.Discount = CalculateDiscount(oTDRow.ItemID, oTDRow.QTY, oTDRow.Price);   //Sprint-21 - PRIMEPOS-225 11-Feb-2016 JY line discount is not considered when we change the item price/discount of item by "Change Item Info".
                }

                nonGroupQty = 0;
                GroupPrice = CheckGroupPricing(oTDRow.ItemID, Convert.ToInt32(qty), oTDRow.Price, out nonGroupQty, oTDRow.OrignalPrice);    //PRIMEPOS-3098 20-Jun-2022 JY Added OrignalPrice
                if (oTDRow.Discount == 0 && nonGroupQty > 0 && isItemInfoChanged == false)
                {
                    oTDRow.Discount = CalculateDiscount(oTDRow.ItemID, nonGroupQty, oTDRow.Price);
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "ValidateItemQTY()");
                throw exp;
            }
        }

        public void SetItemCategory(TransDetailRow oTDRow)
        {
            string strCat = "";
            ItemRow oIRow = null;
            try
            {
                oIRow = checkOTCItems(oTDRow);
                if (oTDRow.IsIIAS == true)
                {
                    strCat = "F";
                }

                if (oTDRow.IsEBTItem == true)
                {
                    strCat += strCat == "" ? "E" : ",E";
                }

                if (oIRow != null)
                {
                    oTDRow.IsMonitored = true;
                    strCat += strCat == "" ? "M" : ",M";
                }

                if (strCat != "")
                {
                    oTDRow.Category = strCat;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsItemActive()
        {
            bool bReturn = true;
            try
            {
                Item oItem = new Item();
                ItemData oItemData = oItem.Populate(oTDRow.ItemID.Trim());

                if (oItemData.Item.Rows.Count > 0)
                {
                    bReturn = (oItemData.Item[0].IsActive == true ? true : false);
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "IsItemActive()");
                bReturn = true;
            }
            return bReturn;
        }

        public bool IsItemExists()
        {
            try
            {
                logger.Trace("IsItemExists() - " + clsPOSDBConstants.Log_Entering);
                Item oItem = new Item();

                //Modified by SRT(Abhishek) Date : 07 Aug 2009
                ItemData oItemData = oItem.Populate(oTDRow.ItemID.Trim());
                //End of Modified by SRT(Abhishek) Date : 07 Aug 2009

                if (oItemData.Item.Rows.Count > 0)
                {
                    logger.Trace("IsItemExists() - " + clsPOSDBConstants.Log_Exiting + "1");
                    return true;
                }
                else
                {
                    logger.Trace("IsItemExists() - " + clsPOSDBConstants.Log_Exiting + "2");
                    return false;
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "IsItemExists()");
                return false;
            }
        }

        public ItemData PopulateItemList(string condn)
        {
            ItemData oItemData = new ItemData();
            try
            {
                ItemSvr itemSvr = new ItemSvr();
                oItemData = itemSvr.PopulateList(condn);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateItemList()");
                throw exp;
            }
            return oItemData;
        }

        public string GetToolTipForGridItem(string strCat)
        {
            string[] cat = strCat.Split(',');
            string ToolTip = "";

            foreach (string s in cat)
            {
                if (s == "F")
                {
                    ToolTip = "F - FSA Item\n";
                }
                else if (s == "E")
                {
                    ToolTip += "E - EBT Item\n";
                }
                else if (s == "M")
                {
                    ToolTip += "M - Monitored Item";
                }
            }
            return ToolTip;

        }

        public string GetWarningMessage(string itemId)
        {
            string strMessage = "";
            try
            {
                WarningMessagesData oData = null;
                WarningMessages oMsgs = new WarningMessages();
                oData = oMsgs.GetByItemID(itemId);
                if (oData != null)
                {
                    if (oData.WarningMessages.Rows.Count > 0)
                    {

                        foreach (WarningMessagesRow oRow in oData.WarningMessages.Rows)
                        {
                            if (oRow.IsActive == true)
                            {
                                if (strMessage.Trim().Length > 0)
                                {
                                    strMessage += Environment.NewLine;
                                }

                                strMessage += oRow.WarningMessage;
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetWarningMessage()");
                throw exp;
            }
            return strMessage;
        }

        public void ValidateOTCItems(string customerTag)
        {
            int customerID = Configuration.convertNullToInt(customerTag);
            ItemSvr itemSvr = new ItemSvr();
            Boolean? bIsSudafed = false;
            TransDetailSvr oTransDetailSvr = new TransDetailSvr();

            foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
            {
                #region Sprint-18 - 2133 13-Nov-2014 JY Added to check sudafed item present in the list
                if (bIsSudafed == false)
                {
                    Boolean? result = oTransDetailSvr.IsSudaFedItem(oRow.ItemID);
                    if (result != null)
                    {
                        bIsSudafed = true;
                    }
                    if (bIsSudafed == true) break;
                }
                #endregion   
            }
            #region Sprint-18 - 2133 13-Nov-2014 JY Added to check sudafed item present in the list
            if (bIsSudafed == true)
                itemSvr.IsQtyValid(customerID, oTransDData.TransDetail);
            #endregion
        }

        public ItemMonitorCategoryRow GetItemMonitoringCategoryRow(int itemMonCatID)
        {
            ItemMonitorCategoryData oIMCData = null;
            ItemMonitorCategoryRow oIMCRow = null;
            try
            {
                ItemMonitorCategory oIMCategory = new ItemMonitorCategory();
                oIMCData = oIMCategory.Populate(itemMonCatID);
                if (oIMCData != null && oIMCData.Tables[0].Rows.Count > 0)
                {
                    oIMCRow = (ItemMonitorCategoryRow)oIMCData.ItemMonitorCategory.Rows[0];
                }
            }
            catch (Exception Ex)
            {
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
            }
            return oIMCRow;
        }
        public ItemMonitorCategoryDetailRow GetItemMonitoringDetails(string itemID)
        {
            ItemMonitorCategoryDetailData oIMCDData = null;
            ItemMonitorCategoryDetailRow oIMCDRow = null;
            try
            {
                ItemMonitorCategoryDetail oIMCDetail = new ItemMonitorCategoryDetail();
                oIMCDData = oIMCDetail.Populate(itemID);
                if (oIMCDData != null && oIMCDData.Tables[0].Rows.Count > 0)
                {
                    oIMCDRow = (ItemMonitorCategoryDetailRow)oIMCDData.Tables[0].Rows[0];
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetItemMonitoringDetails()");
                clsCoreUIHelper.ShowErrorMsg(Ex.Message);
            }
            return oIMCDRow;
        }

        public bool SkipForDelivery()
        {
            bool shouldskip = false;
            if (Configuration.CPOSSet.SkipDelSign && oTransHRow.IsDelivery)
            {
                shouldskip = true;
            }
            return shouldskip;
        }

        public ItemData GetAllItems()
        {
            ItemData oItemData = null;

            try
            {
                string itemIDs = string.Empty;
                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    if (itemIDs.Length > 0)
                    {
                        itemIDs += ",";
                    }
                    itemIDs += "'" + oRow.ItemID.Replace("'", "''") + "'";
                }
                if (itemIDs.Length > 0)
                {
                    ItemSvr itemSvr = new ItemSvr();
                    oItemData = itemSvr.PopulateList(" where itemid in (" + itemIDs + ")");
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetAllItems()");
                throw exp;
            }
            return oItemData;
        }

        public ItemRow checkOTCItems()
        {
            ItemRow OTCItem = null;
            try
            {

                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    OTCItem = checkOTCItems(oRow);

                    if (OTCItem != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "checkOTCItems()");
                throw exp;
            }

            return OTCItem;
        }
        public ItemRow checkOTCItems(TransDetailRow oRow)
        {
            ItemRow OTCItem = null;

            try
            {
                Item oItem = new Item();
                //Chnaged by SRT(Abhishek) 12 Aug 2009
                ItemData oIData = oItem.Populate(oRow.ItemID.Trim());

                //Chnaged by SRT(Abhishek) 12 Aug 2009
                if (oIData != null && oIData.Item != null)
                {
                    //Added by SRT(Abhishek) Date : 25 Aug 2009
                    if (oIData.Item.Rows.Count > 0)
                    {
                        if (oIData.Item[0].isOTCItem == true)
                        {
                            OTCItem = oIData.Item[0];
                        }
                    }
                    //End of Added by SRT(Abhishek) Date : 25 Aug 2009
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "checkOTCItems()");
                throw exp;
            }


            return OTCItem;
        }

        public DataTable checkPSEItems()
        {
            DataTable dt = new DataTable();
            try
            {
                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    dt = checkPSEItems(oRow);
                    if (dt != null && dt.Rows.Count > 0)
                        break;
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "checkPSEItems()");
                throw exp;
            }
            return dt;
        }
        public DataTable checkPSEItems(TransDetailRow oRow)
        {
            DataTable dt;
            try
            {
                Item oItem = new Item();
                dt = oItem.IsPSEItemData(oRow.ItemID.Trim());
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "checkPSEItems()");
                throw exp;
            }

            return dt;
        }

        public int GetTransIndex(TransDetailData TransData, int TransItemDetailID)
        {
            /* By Manoj 11/14/2013 This is to get the correct index ofd each item to be updated. */
            int index = 0;
            try
            {
                if (Configuration.isNullOrEmptyDataSet(TransData) == false)
                {
                    //This will give one DataRow only. We are looking for a specific TransItemDetailID
                    DataRow[] TransItemRow = TransData.TransDetail.Select(" TransDetailID ='" + TransItemDetailID + "'");
                    foreach (DataRow dr in TransItemRow)
                    {
                        index = dr.Table.Rows.IndexOf(dr); //index of the DataRow
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTransIndex()");
                throw new Exception(ex.ToString());
            }
            return index;
        }

        public ItemRow GetItemRowBySKUCode(string code)
        {
            ItemRow oItemRow;
            try
            {
                Item oItem = new Item();
                oItemRow = oItem.FindItemBySKUCode(code);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetItemRowBySKUCode()");
                throw exp;
            }
            return oItemRow;
        }

        public bool CheckForMatchingItem(string code, ref ItemRow oItemRow)
        {
            bool isExists = false;
            try
            {
                bool IsMatchingItemExist = false;
                ItemSvr oItemSvr = new ItemSvr();
                string sItemID = oItemSvr.LookForMatchingItem(code, out IsMatchingItemExist);
                if (string.IsNullOrEmpty(sItemID) == false)
                {
                    GetItemRowByItemId(sItemID, ref oItemRow);
                    if (oItemRow != null)
                    {
                        isExists = true;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "LookForMatchingItem()");
                throw exp;
            }

            return isExists;
        }

        public void GetItemRowByItemId(string sItemID, ref ItemRow oItemRow)
        {

            try
            {
                Item oItem = new Item();
                ItemData oItemData;
                oItemData = oItem.Populate(sItemID);
                if (Configuration.isNullOrEmptyDataSet(oItemData) == false)
                {
                    oItemRow = oItemData.Item[0];
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "getItemRowByItemId()");
                throw exp;
            }
        }

        public POSTransSignLogData AddPSEItemIntoTransSignLog()
        {
            POSTransSignLogData oTempTransSignLogData = new POSTransSignLogData();
            try
            {

                int mRow_Index = 0;
                int RowIndex = 0;

                strOTCItemDescriptions.Clear();
                ItemData oItemData = GetAllItems();
                if (oItemData.Item.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    Item oItem = new Item();
                    foreach (ItemRow oRow in oItemData.Item.Rows)
                    {
                        dt = oItem.IsPSEItemData(oRow.ItemID.Trim());
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //get TransDetailId means grid row no of this itemrow
                            int tmpTransDetailId = 0;
                            foreach (TransDetailRow tmpRow in oTransDData.TransDetail.Rows)
                            {
                                if (oRow.ItemID == tmpRow.ItemID)
                                {
                                    tmpTransDetailId = tmpRow.TransDetailID;
                                    break;
                                }
                            }
                            oTempTransSignLogData.POSTransSignLog.AddRow(mRow_Index, 0, POSSignContext.ItemMonitoring, "", null, "", clsVerificationBy.ByBoth, "", tmpTransDetailId);
                            mRow_Index++;

                            isByBoth = true;
                            strOTCItemDescriptions.Insert(RowIndex, oRow.Description.Trim());
                            RowIndex++;
                            isAgeLimit = true;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddPSEItemIntoTransSignLog()");
                throw exp;
            }
            return oTempTransSignLogData;
        }

        public ItemData PopulateItem(string sItemID)
        {
            ItemData oItemData = new ItemData();
            try
            {
                Item oItem = new Item();
                oItemData = oItem.Populate(sItemID);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateItem()");
                throw exp;
            }
            return oItemData;
        }
        public void PersistItem(ItemData oItemData)
        {
            try
            {
                Item oItem = new Item();
                oItem.Persist(oItemData);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PersistItem()");
                throw exp;
            }
        }
        public bool IsIIASItem(string sItemID)
        {
            bool isIIAS = false;
            try
            {
                Item oItem = new Item();
                isIIAS = oItem.IsIIASItem(sItemID);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IsIIASItem()");
                throw exp;
            }
            return isIIAS;
        }
        public NotesData PopulateItemNotes(string whereClause)
        {
            NotesData oNotesData = new NotesData();
            try
            {
                Notes oNotes = new Notes();
                oNotesData = oNotes.PopulateList(whereClause);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateItem()");
                throw exp;
            }
            return oNotesData;
        }
        #endregion

        #region transactiom

        public void SetTransDetails(ItemRow oItemRow, int DefaultQTY, string CouponItemDesc, Int64 CouponID, ref int QTY, ref bool isQtyChange, ref int ExtraQty, ref bool isDeptTaxable, out DepartmentData oDeptData) //PRIMEPOS-2034 19-Mar-2018 JY Added CouponId parameter
        {
            try
            {
                TransDetailData oTData = new TransDetailData();
                if (!isQtyChange)
                {
                    oTDRow = oTData.TransDetail.AddRow(clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, oItemRow[clsPOSDBConstants.Item_Fld_ItemID].ToString(), "");
                }
                else
                {
                    oTDRow = oTData.TransDetail.AddRow(clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, oItemRow[clsPOSDBConstants.Item_Fld_ItemID].ToString(), "");
                    QTY = ExtraQty;
                    ExtraQty = 0;
                    isQtyChange = false;
                }
                SetRowTrans(oTDRow);

                //check if item is IIAS or not
                if (oTDRow.ItemID.ToUpper().Trim() == Configuration.CPOSSet.RXCode.ToUpper().Trim())
                {
                    oTDRow.IsIIAS = true;
                    oTDRow.IsRxItem = true;
                }
                else
                {
                    Item oItem = new Item();
                    oTDRow.IsIIAS = oItem.IsIIASItem(oItemRow.ItemID);
                }

                oTDRow.IsEBTItem = oItemRow.IsEBTItem;  //Sprint-21 - PRIMEPOS-2258 12-Feb-2016 JY Added to assign correct EBT

                isDeptTaxable = false;
                bool isDeptDiscount = false;
                oDeptData = new DepartmentData();
                if (oItemRow.DepartmentID != 0)
                {
                    bool bTaxableDept = false;
                    bool bDiscountDept = true;
                    Department oDept = new Department();
                    oDeptData = new DepartmentData();
                    oDeptData = oDept.Populate(oItemRow.DepartmentID);
                    if (oDeptData == null || oDeptData.Department.Rows.Count == 0)
                    {
                        if (Configuration.CInfo.DefaultDeptId > 0)
                        {
                            oItemRow.DepartmentID = Configuration.CInfo.DefaultDeptId;
                            oDeptData = oDept.Populate(oItemRow.DepartmentID);
                        }
                    }
                    if (oDeptData != null && oDeptData.Department.Rows.Count > 0)
                        bTaxableDept = oDeptData.Department[0].IsTaxable;

                    isDeptTaxable = bTaxableDept; // oDeptData.Department[0].IsTaxable;
                    isDeptDiscount = bDiscountDept;  // true; //oDeptData.Department[0].IsDiscountable;
                }

                if (QTY == 0)
                {
                    oTDRow.QTY = DefaultQTY;
                    QTY = DefaultQTY;
                }
                else
                {
                    oTDRow.QTY = QTY;
                }

                if (oItemRow.ItemID.Trim().ToUpper() == Configuration.CouponItemCode.Trim().ToUpper())    //Sprint-23 - PRIMEPOS-2279 19-Mar-2016 JY Added if
                {
                    oTDRow.ItemDescription = CouponItemDesc;
                    oTDRow.CouponID = CouponID;
                }
                else
                {
                    oTDRow.ItemDescription = oItemRow.Description;
                }

                //Added by SRT(Abhishek) Date : 05/09/2009
                oTDRow.ItemCost = Math.Round(oItemRow.LastCostPrice, 2);
                //End of Added by SRT(Abhishek) Date : 05/09/2009
                oTDRow.OrignalPrice = oItemRow.SellingPrice;
                oTDRow.IsNonRefundable = Configuration.convertNullToBoolean(oItemRow.IsNonRefundable);      //PRIMEPOS-2592 05-Nov-2018 JY Added 
                #region  Added for Solutran - PRIMEPOS-2663 - NileshJ - 05-July-2019
                oTDRow.S3TransID = oItemRow.S3TransID;
                oTDRow.S3PurAmount = oItemRow.S3PurAmount;
                oTDRow.S3TaxAmount = oItemRow.S3TaxAmount;
                oTDRow.S3DiscountAmount = oItemRow.S3DiscountAmount;
                #endregion
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SetTransDetails()");
                throw exp;
            }
        }

        public void GetTransDetailTaxForCopyTrans(DataRow dr, int transId, ref TransDetailTaxData TmpTaxData, ref int row)
        {
            try
            {

                if (!ItemAlreadyProcess)
                {
                    oTDRow = oTransDData.TransDetail.AddRow(clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, "", "");
                    SetRowTrans(oTDRow);


                    oTDRow.ItemID = dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString();
                    oTDRow.QTY = Convert.ToInt32(dr[clsPOSDBConstants.TransDetail_Fld_QTY].ToString());
                    oTDRow.ItemDescription = dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString();
                    oTDRow.ItemDescriptionMasked = MaskDrugName(oTDRow.ItemID,oTDRow.ItemDescription);  //PRIMEPOS-3130
                    oTDRow.NonComboUnitPrice = oTDRow.Price = Convert.ToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_Price].ToString());
                    oTDRow.Discount = Convert.ToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_Discount].ToString());
                    oTDRow.ExtendedPrice = Convert.ToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].ToString());
                    oTDRow.ItemCost = Convert.ToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_ItemCost].ToString());
                    //Added by Shitaljit(QuicSolv) on 30 March 2012
                    oTDRow.IsNonRefundable = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsNonRefundable]); //PRIMEPOS-2592 06-Nov-2018 JY Added 
                    if (CurrentTransactionType == POSTransactionType.SalesReturn)   // SOLUTRAN PRIMEPOS-2663-FAILED WHEN STRICT RETURN IS ENABLED // Resolved SAJID DHUKKA
                    {
                        oTDRow.S3TransID = Configuration.convertNullToInt64(dr[clsPOSDBConstants.TransDetail_Fld_S3TransID]);//PRIMPOS-3265
                        oTDRow.S3PurAmount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3PurAmount]);
                        oTDRow.S3TaxAmount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3TaxAmount]);
                        oTDRow.S3DiscountAmount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount]);
                    }
                    else
                    {
                        oTDRow.S3TransID = 0;
                        oTDRow.S3PurAmount = 0;
                        oTDRow.S3TaxAmount = 0;
                        oTDRow.S3DiscountAmount = 0;
                    }
                    #region Tag Item Category.

                    oTDRow.IsEBTItem = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsEBT].ToString());//Added by Shitaljit(QuicSolv) on 6 Sept 2011
                    oTDRow.IsIIAS = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsIIAS].ToString());

                    SetItemCategory(oTDRow);
                }
                else
                {
                    ItemAlreadyProcess = false;
                }
                #endregion Tag Item Category.

                //if(Convert.ToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString()) > 0) //Sprint-21 - 2169 20-Jul-2015 JY Commented
                if (Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString()) != 0)  //Sprint-21 - 2169 20-Jul-2015 JY Added to consider tax while copying return trans item in sale trans
                {
                    ItemTax oItemTax = new ItemTax();
                    TransDetailTaxSvr oTDTaxSvr = new TransDetailTaxSvr();
                    oTDTaxData = oTDTaxSvr.PopulateData(transId);
                    oTDRow.TaxAmount = Convert.ToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString());
                }

                //Added by Manoj 7/29/2015 - Fix issue with tax returns.  
                if (oTDTaxData != null && oTDTaxData.TransDetailTax.Count > 0)
                {
                    foreach (TransDetailTaxRow TDR in oTDTaxData.TransDetailTax)
                    {
                        if (Convert.ToInt32(dr["TRANSDETAILID"].ToString()) == TDR.TransDetailID) //Compare each item to the TransDetailID in tax
                        {
                            //TDR.TaxAmount = TDR.TaxAmount * (-1); //Sprint-21 01-Feb-2016 JY Commented and added below code
                            #region Sprint-21 01-Feb-2016 JY Added to resolve the issue - Tax not being considered when sale is performed using the historical transaction. It punches -ve tax for sale
                            if (CurrentTransactionType == POSTransactionType.SalesReturn)
                                TDR.TaxAmount = TDR.TaxAmount * (-1);
                            else
                                TDR.TaxAmount = TDR.TaxAmount;
                            #endregion
                            TDR.ItemRow = row;
                            TmpTaxData.TransDetailTax.ImportRow(TDR);
                            row++;

                            #region PRIMEPOS-2924 09-Dec-2020 JY Added logic to update TaxCode in TransDetail
                            TaxCodesData oTaxCodesData = new TaxCodesData();
                            using (var dao = new TaxCodesSvr())
                            {
                                oTaxCodesData = dao.PopulateList(" WHERE " + clsPOSDBConstants.TaxCodes_Fld_TaxID + " = '" + TDR.TaxID + "'");
                            }
                            if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                            {
                                if (oTDRow.TaxCode == "")
                                    oTDRow.TaxCode = Configuration.convertNullToString(oTaxCodesData.TaxCodes.Rows[0]["TaxCode"]);
                                else
                                    oTDRow.TaxCode += "," + Configuration.convertNullToString(oTaxCodesData.TaxCodes.Rows[0]["TaxCode"]);
                            }
                            #endregion
                        }
                    }
                }
                if (oTDRow != null && ((CurrentTransactionType == POSTransactionType.Sales && oTDRow.QTY < 0) || (CurrentTransactionType == POSTransactionType.SalesReturn && oTDRow.QTY > 0)))
                {
                    oTDRow.QTY *= -1;
                    oTDRow.Discount *= -1;
                    oTDRow.TaxAmount *= -1;
                    oTDRow.ExtendedPrice *= -1;
                    oTDRow.InvoiceDiscount *= -1;   //PRIMEPOS-2768 03-Jan-2020 JY Added
                }


            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransDetailTaxForCopyTrans()");
                throw exp;
            }

        }

        public void GetTransDetailTaxForCopyViewTrans(DataRow dr, int transId, ref TransDetailTaxData TmpTaxData, ref int row)
        {
            try
            {
                if (!ItemAlreadyProcess)
                {
                    oTDRow = oTransDData.TransDetail.AddRow(clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, "", "");
                    SetRowTrans(oTDRow);

                    oTDRow.ItemID = dr[clsPOSDBConstants.TransDetail_Fld_ItemID].ToString();
                    oTDRow.QTY = Configuration.convertNullToInt(dr[clsPOSDBConstants.TransDetail_Fld_QTY].ToString());
                    oTDRow.ItemDescription = dr[clsPOSDBConstants.TransDetail_Fld_ItemDescription].ToString();
                    //oTDRow.NonComboUnitPrice = oTDRow.Price = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_Price].ToString()); //PRIMEPOS-2743 05-Nov-2019 JY Commented
                    oTDRow.OrignalPrice = oTDRow.NonComboUnitPrice = oTDRow.Price = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_Price].ToString()); //PRIMEPOS-2743 05-Nov-2019 JY Added     
                    oTDRow.Discount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_Discount].ToString());
                    oTDRow.ExtendedPrice = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].ToString());
                    oTDRow.ItemCost = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_ItemCost].ToString());
                    oTDRow.IsEBTItem = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsEBT].ToString());
                    oTDRow.IsIIAS = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsIIAS].ToString());
                    oTDRow.IsNonRefundable = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsNonRefundable].ToString());  //PRIMEPOS-2592 06-Nov-2018 JY Added 
                    #region  Added for Solutran - PRIMEPOS-2663 - NileshJ - 05-July-2019
                    if (CurrentTransactionType == POSTransactionType.SalesReturn)   //PRIMEPOS-2836 15-Apr-2020 JY Added if condition and else part
                    {
                        oTDRow.S3TransID = Configuration.convertNullToInt64(dr[clsPOSDBConstants.TransDetail_Fld_S3TransID]);//PRIMPOS-3265
                        oTDRow.S3PurAmount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3PurAmount]);
                        oTDRow.S3TaxAmount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3TaxAmount]);
                        oTDRow.S3DiscountAmount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount]);
                    }
                    else
                    {
                        oTDRow.S3TransID = 0;
                        oTDRow.S3PurAmount = 0;
                        oTDRow.S3TaxAmount = 0;
                        oTDRow.S3DiscountAmount = 0;
                    }
                    #endregion
                    oTDRow.InvoiceDiscount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount].ToString());  //PRIMEPOS-2768 03-Jan-2020 JY Added
                    //oTDRow.IsOnSale = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.TransDetail_Fld_IsOnSale].ToString());    //PRIMEPOS-2907 13-Oct-2020 JY intentionally added and commented because when we copy transaction it will copy the same transaction as historical, so no need to copy IsOnSale flag.
                    if (CurrentTransactionType == POSTransactionType.SalesReturn)
                    {
                        if (Configuration.convertNullToInt(dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID]) != 0)
                        {
                            oTDRow.ReturnTransDetailId = Configuration.convertNullToInt(dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID]);
                        }
                    }
                    oTDRow.ItemDescriptionMasked = MaskDrugName(oTDRow.ItemID, oTDRow.ItemDescription);  //PRIMEPOS-3423
                    SetItemCategory(oTDRow);
                }
                else
                {
                    ItemAlreadyProcess = false;
                }

                //if (Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString()) != 0)  //Sprint-21 - 2169 20-Jul-2015 JY Added to consider tax while copying return trans item in sale trans
                {//Commented if condition as for "9999T - taxable" item have tax amount 0
                    TransDetailTaxSvr oTDTaxSvr = new TransDetailTaxSvr();
                    oTDTaxData = oTDTaxSvr.PopulateData(transId);
                    #region PRIMEPOS-2501 04-Apr-2018 JY Added logic to update Taxcode in transdetail so that it would displayed on copied record
                    if (oTDTaxData != null && oTDTaxData.Tables.Count > 0 && oTDTaxData.TransDetailTax.Rows.Count > 0)
                    {
                        TaxCodes oTaxCodes = new TaxCodes();
                        List<int> lstTaxIds = oTDTaxData.TransDetailTax.AsEnumerable().Where(r => r.TransDetailID == Configuration.convertNullToInt(dr[clsPOSDBConstants.TransDetail_Fld_TransDetailID])).Select(r => r.Field<int>("TaxId")).ToList();
                        if (lstTaxIds.Count > 0)
                        {
                            oTDRow.TaxAmount = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.TransDetail_Fld_TaxAmount].ToString());
                            string strTaxIds = "(" + String.Join(",", lstTaxIds) + ")";
                            TaxCodesData oTaxCodesData = oTaxCodes.PopulateList(" WHERE TaxID IN " + strTaxIds);
                            List<string> lstTaxCodes = oTaxCodesData.TaxCodes.AsEnumerable().Select(r => r.Field<string>("TaxCode")).ToList().OrderBy(t => t).ToList();
                            string strTaxCodes = String.Join(",", lstTaxCodes);
                            oTDRow.TaxCode = strTaxCodes;
                        }
                    }
                    #endregion
                }

                //Added by Manoj 7/29/2015 - Fix issue with tax returns.  
                if (oTDTaxData != null && oTDTaxData.TransDetailTax.Count > 0)
                {
                    #region PRIMEPOS-2500 05-Apr-2018 JY added logic to filter specific tax records and apply changes on it
                    oTDTaxData.TransDetailTax.DefaultView.RowFilter = "TRANSDETAILID = " + Convert.ToInt32(dr["TRANSDETAILID"].ToString());
                    DataTable dt = (oTDTaxData.TransDetailTax.DefaultView).ToTable();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TransDetailTaxData ds = new TransDetailTaxData();
                        ds.TransDetailTax.MergeTable(dt);

                        foreach (TransDetailTaxRow TDR in ds.TransDetailTax)
                        {
                            if (CurrentTransactionType == POSTransactionType.SalesReturn)
                                TDR.TaxAmount = TDR.TaxAmount * (-1);
                            else
                                TDR.TaxAmount = Math.Abs(TDR.TaxAmount);
                            TDR.ItemRow = row;
                            TDR.TransDetailID = oTDRow.TransDetailID;
                            TDR.TransID = 0;
                            TmpTaxData.TransDetailTax.ImportRow(TDR);
                        }
                        row += 1;
                    }
                    #endregion

                    #region PRIMEPOS-2500 05-Apr-2018 JY Commented
                    //int? previousTransDetailId = null;
                    //foreach (TransDetailTaxRow TDR in oTDTaxData.TransDetailTax)
                    //{
                    //    if (Convert.ToInt32(dr["TRANSDETAILID"].ToString()) == TDR.TransDetailID) //Compare each item to the TransDetailID in tax
                    //    {
                    //        //TDR.TaxAmount = TDR.TaxAmount * (-1); //Sprint-21 01-Feb-2016 JY Commented and added below code
                    //        #region Sprint-21 01-Feb-2016 JY Added to resolve the issue - Tax not being considered when sale is performed using the historical transaction. It punches -ve tax for sale
                    //        if (CurrentTransactionType == POSTransactionType.SalesReturn)
                    //            TDR.TaxAmount = TDR.TaxAmount * (-1);
                    //        else
                    //            TDR.TaxAmount = TDR.TaxAmount;
                    //        #endregion
                    //        if (previousTransDetailId == null || previousTransDetailId == TDR.TransDetailID)
                    //        {
                    //            TDR.ItemRow = row;
                    //            previousTransDetailId = TDR.TransDetailID;
                    //        }
                    //        else
                    //        {
                    //            TDR.ItemRow = row + 1;
                    //            previousTransDetailId = TDR.TransDetailID;
                    //        }
                    //        TmpTaxData.TransDetailTax.ImportRow(TDR);
                    //    }
                    //}
                    #endregion
                }
                if (oTDRow != null && ((CurrentTransactionType == POSTransactionType.Sales && oTDRow.QTY < 0) || (CurrentTransactionType == POSTransactionType.SalesReturn && oTDRow.QTY > 0)))
                {
                    oTDRow.QTY *= -1;
                    oTDRow.Discount *= -1;
                    oTDRow.TaxAmount *= -1;
                    oTDRow.ExtendedPrice *= -1;
                    oTDRow.InvoiceDiscount *= -1;   //PRIMEPOS-2768 03-Jan-2020 JY Added
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransDetailTaxForCopyViewTrans()");
                throw exp;
            }
        }

        public void AddTransactionDetailData(string itemCode)
        {
            try
            {
                TransDetailData oTData = new TransDetailData();
                oTDRow = oTData.TransDetail.AddRow(clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, itemCode, "");
                SetRowTrans(oTDRow);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddTransactionDetailData()");
                throw exp;
            }

        }

        public TransDetailRow AddCompanionItem(ItemCompanionRow OICompRow, int qty, string unitPrice, out TransDetailRow oExistingRow)
        {

            ItemData oItemData;
            ItemRow oItemRow = null;
            oExistingRow = null;
            TransDetailRow oTDCompRow = null;
            try
            {
                oItemData = PopulateItem(OICompRow.CompanionItemID);
                if (oItemData.Item.Rows.Count > 0)
                    oItemRow = oItemData.Item[0];
                else
                    oItemRow = null;

                if (oItemRow != null)
                {
                    //check if companion item already exist

                    oTDCompRow = (TransDetailRow)oTransDData.TransDetail.NewRow(); //this.oTransDData.TransDetail.AddRow(clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, "", "");

                    oTDCompRow.ItemID = oItemRow.ItemID;
                    oTDCompRow.QTY = qty;
                    oTDCompRow.ItemCost = oItemRow.LastCostPrice;
                    oTDCompRow.ItemDescription = oItemRow.Description;
                    if (OICompRow.Amount != 0)
                    {
                        oTDCompRow.NonComboUnitPrice = oTDCompRow.Price = OICompRow.Amount;
                    }
                    else
                    {
                        if (OICompRow.Percentage != 0)
                        {
                            oTDCompRow.NonComboUnitPrice = oTDCompRow.Price = OICompRow.Percentage / 100 * Convert.ToDecimal(unitPrice);
                        }
                        else
                        {
                            oTDCompRow.NonComboUnitPrice = oTDCompRow.Price = 0;
                        }
                    }
                    CalcExtdPrice(oTDCompRow);

                    RecaclulateTransDetailItem(oTDCompRow, oItemRow, oTDTaxData);

                    oTDCompRow.IsCompanionItem = true;
                    if (Configuration.CInfo.PromptForSellingPriceLessThanCost)
                    {
                    }
                    if (Configuration.CInfo.GroupTransItems == true)
                    {
                        oExistingRow = GetExistingRow(oTDCompRow, true);
                    }

                    oTDCompRow.TransDetailID = clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID);

                    if (oExistingRow != null)
                    {
                        oExistingRow.QTY += oTDCompRow.QTY;
                        oTDCompRow = oExistingRow;
                        RecaclulateTransDetailItem(oTDCompRow, oItemRow, oTDTaxData);
                    }
                    else
                    {
                        oTDCompRow.Discount = 0;
                        oTransDData.TransDetail.Rows.Add(oTDCompRow.ItemArray);
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddCompanionItem()");
                throw exp;
            }
            return oTDCompRow;

        }

        public void AddPOSSignTransLog(POSTransSignLogData oTempTransSignLogData, byte[] OTCSignDataBinary, string authorizationNo, string dob, string verificationId, bool isCanceled)
        {
            try
            {
                foreach (POSTransSignLogRow oRow in oTempTransSignLogData.POSTransSignLog.Rows)
                {
                    /*Comment by Manoj 9/20/2013 - All IF was commented because it use to only get ID type for the first item only
                    Now the new way will get the requested ID type for each item.*/

                    //if(oIMCategoryRow.VerificationBy == clsVerificationBy.ByBoth)
                    if (clsVerificationBy.ByBoth == oRow.CustomerIDType.Trim())
                    {
                        if (authorizationNo.Length > 41) authorizationNo = authorizationNo.Substring(0, 41); //PRIMEPOS-2959 28-Apr-2021 JY Added
                        oRow.CustomerIDDetail = authorizationNo + "^" + dob;
                        oRow.SignDataText = OTCSignDataText;
                        oRow.SignDataBinary = OTCSignDataBinary;
                    }

                    //if(oIMCategoryRow.VerificationBy == clsVerificationBy.ByID)
                    if (clsVerificationBy.ByID == oRow.CustomerIDType.Trim())
                    {
                        if (authorizationNo.Length > 41) authorizationNo = authorizationNo.Substring(0, 41); //PRIMEPOS-2959 28-Apr-2021 JY Added
                        oRow.CustomerIDDetail = authorizationNo + "^" + dob;
                        oRow.SignDataText = "";
                        oRow.SignDataBinary = null;
                    }

                    //if(oIMCategoryRow.VerificationBy == clsVerificationBy.BySignature)
                    if (clsVerificationBy.BySignature == oRow.CustomerIDType.Trim())
                    {
                        oRow.SignDataText = OTCSignDataText;
                        oRow.SignDataBinary = OTCSignDataBinary;
                    }

                    if (isCanceled == false && (clsVerificationBy.ByBoth == oRow.CustomerIDType.Trim() || clsVerificationBy.ByID == oRow.CustomerIDType.Trim()))
                    {
                        oRow.CustomerIDType = verificationId;
                    }
                    else
                    {
                        oRow.CustomerIDType = "";
                    }
                    oTransSignLogData.POSTransSignLog.ImportRow(oRow);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "AddPOSSignTransLog()");
                throw exp;
            }

        }

        public POSTransSignLogData GetPOSSignTransLog()
        {
            int RowIndex = 0;
            int ItemMnoCatID = 0;
            int mRow_Index = 0;
            strOTCItemDescriptions.Clear();
            POSTransSignLogData oTempTransSignLogData = new POSTransSignLogData();
            try
            {

                ItemData oItemData = GetAllItems();
                if (oItemData.Item.Rows.Count > 0)
                {
                    foreach (ItemRow oRow in oItemData.Item.Rows)
                    {
                        oIMCDetailRow = GetItemMonitoringDetails(oRow.ItemID.Trim());
                        if (oIMCDetailRow != null)
                        {
                            oIMCategoryRow = GetItemMonitoringCategoryRow(oIMCDetailRow.ItemMonCatID);
                            if (oIMCategoryRow.VerificationBy != string.Empty)
                            {
                                if (ItemMnoCatID == 0 || ItemMnoCatID != oIMCDetailRow.ItemMonCatID)
                                {
                                    //get TransDetailId means grid row no of this itemrow
                                    int tmpTransDetailId = 0;
                                    foreach (TransDetailRow tmpRow in oTransDData.TransDetail.Rows)
                                    {
                                        if (tmpRow.ItemID == oRow.ItemID)
                                        {
                                            tmpTransDetailId = tmpRow.TransDetailID;
                                            break;
                                        }
                                    }

                                    oTempTransSignLogData.POSTransSignLog.AddRow(mRow_Index, 0, POSSignContext.ItemMonitoring, oIMCategoryRow.ID.ToString(), null, "", oIMCategoryRow.VerificationBy, "", tmpTransDetailId);
                                    mRow_Index++;
                                }
                                ItemMnoCatID = oIMCDetailRow.ItemMonCatID;

                                if (oIMCategoryRow.VerificationBy == clsVerificationBy.ByBoth)
                                {
                                    isByBoth = true;
                                }
                                if (oIMCategoryRow.VerificationBy == clsVerificationBy.ByID)
                                {
                                    isByDLPresent = true;
                                }
                                if (oIMCategoryRow.VerificationBy == clsVerificationBy.BySignature)
                                {
                                    isBySignPresent = true;
                                }
                                if (oIMCategoryRow.VerificationBy == clsVerificationBy.ByBoth || oIMCategoryRow.VerificationBy == clsVerificationBy.BySignature)
                                {
                                    strOTCItemDescriptions.Insert(RowIndex, oRow.Description.Trim());
                                    RowIndex++;
                                }
                                if (oIMCategoryRow.IsAgeLimit && oIMCategoryRow.AgeLimit > 0)
                                {
                                    isAgeLimit = true; //For Age by Manoj on 7/16/2013
                                }
                                if (oIMCategoryRow.canOverrideMonitorItem) //PRIMEPOS-3166
                                {
                                    showOverrideBtn = true;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetPOSSignTransLog()");
                throw exp;
            }
            return oTempTransSignLogData;
        }

        #region PRIMEPOS-3166
        public void OverrideTempSaving(string overrideUserId)
        {
            int ItemMnoCatID = 0;
            ItemData oItemData = GetAllItems();
            if (oItemData.Item.Rows.Count > 0)
            {
                foreach (ItemRow oRow in oItemData.Item.Rows) //Multiple item 
                {
                    oIMCDetailRow = GetItemMonitoringDetails(oRow.ItemID.Trim()); //item wise monitor detail
                    if (oIMCDetailRow != null)
                    {
                        oIMCategoryRow = GetItemMonitoringCategoryRow(oIMCDetailRow.ItemMonCatID); //Item Monitor category
                        if (!string.IsNullOrWhiteSpace(oIMCategoryRow.VerificationBy) && oIMCategoryRow.canOverrideMonitorItem)
                        {
                            if (ItemMnoCatID == 0 || ItemMnoCatID != oIMCDetailRow.ItemMonCatID)
                            {
                                foreach (TransDetailRow tmpRow in oTransDData.TransDetail.Rows)
                                {
                                    if (tmpRow.ItemID == oRow.ItemID)
                                    {
                                        tmpRow.MonitorItemOverrideUser = overrideUserId;
                                        tmpRow.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        public void CheckCompanionItemRow(ItemCompanionRow OICompRow, ItemRow oItemRow, int qty, string unitPrice)
        {

            //check if companion item already exist
            TransDetailRow oTDCompRow;
            oTDCompRow = (TransDetailRow)oTransDData.TransDetail.NewRow(); //this.oTransDData.TransDetail.AddRow(clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID), 0, 0, 0, 0, 0, 0, 0, "", "");

            oTDCompRow.ItemID = oItemRow.ItemID;
            oTDCompRow.QTY = qty;
            oTDCompRow.ItemCost = oItemRow.LastCostPrice;
            oTDCompRow.ItemDescription = oItemRow.Description;
            if (OICompRow.Amount != 0)
            {
                oTDCompRow.NonComboUnitPrice = oTDCompRow.Price = OICompRow.Amount;
            }
            else
            {
                if (OICompRow.Percentage != 0)
                {
                    oTDCompRow.NonComboUnitPrice = oTDCompRow.Price = OICompRow.Percentage / 100 * Convert.ToDecimal(unitPrice);
                }
                else
                {
                    oTDCompRow.NonComboUnitPrice = oTDCompRow.Price = 0;
                }
            }
            CalcExtdPrice(oTDCompRow);

            RecaclulateTransDetailItem(oTDCompRow, oItemRow, oTDTaxData);

            oTDCompRow.IsCompanionItem = true;

            TransDetailRow oExistingRow = null;
            if (Configuration.CInfo.PromptForSellingPriceLessThanCost)
            {
            }
            if (Configuration.CInfo.GroupTransItems == true)
            {
                oExistingRow = GetExistingRow(oTDCompRow, true);
            }

            oTDCompRow.TransDetailID = clsCoreUIHelper.GetNextNumber(oTransDData, clsPOSDBConstants.TransDetail_Fld_TransDetailID);

            if (oExistingRow != null)
            {
                //oQty = oExistingRow.QTY;
                oExistingRow.QTY += oTDCompRow.QTY;
                oTDCompRow = oExistingRow;
                RecaclulateTransDetailItem(oTDCompRow, oItemRow, oTDTaxData);
            }
            else
            {
                oTDCompRow.Discount = 0;
                oTransDData.TransDetail.Rows.Add(oTDCompRow.ItemArray);
            }

        }

        public DataSet PopulateROATransForReturn(string transId)
        {
            DataSet DsROA;
            try
            {
                TransHeaderSvr oHSvr = new TransHeaderSvr();
                DsROA = oHSvr.PopulateROATransForReturn(Convert.ToInt32(transId));
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateROATransForReturn()");
                throw exp;
            }
            return DsROA;
        }

        public void BuildRxHeader(DataRow oRxInfo, ref RXHeader oRXHeader, ref bool bNewRxHeader, string batchCode = null)
        {
            try
            {
                if (oRXHeaderList.Count > 0)
                {
                    oRXHeader = oRXHeaderList.FindByPatient(oRxInfo["PatientNo"].ToString());
                    if (batchCode != null)
                    {
                        if (oRXHeader != null && !oRXHeader.IsIntakeBatch)
                        {
                            oRXHeader.IsIntakeBatch = true;
                            oRXHeader.BatchID = Convert.ToInt64(batchCode);
                        }
                    }
                }
                if (oRXHeader == null)
                {
                    bNewRxHeader = true;
                    oRXHeader = new RXHeader();
                    oRXHeader.PatientNo = oRxInfo["PatientNo"].ToString();
                    oRXHeader.InsType = oRxInfo["BillType"].ToString();
                    oRXHeader.RXSignature = "";
                    oRXHeader.NOPPSignature = "";
                    oRXHeader.bBinarySign = null;
                    oRXHeader.PatientState = "";
                    oRXHeader.FamilyID = Configuration.convertNullToString(oRxInfo["FamilyID"]);    //Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added

                    PharmBL oPharmBL = new PharmBL();
                    //PRIMEPOS-2981 28-Jun-2021 JY Added logic to handle NULL
                    DataTable oTable = oPharmBL.GetPatient(Configuration.convertNullToString(oRXHeader.PatientNo));
                    if (oTable.Rows.Count > 0)
                    {
                        oRXHeader.PatientName = Configuration.convertNullToString(oTable.Rows[0]["Lname"]).Trim() + ", " + Configuration.convertNullToString(oTable.Rows[0]["Fname"]).Trim();
                        oRXHeader.PatientAddr = Configuration.convertNullToString(oTable.Rows[0]["addrstr"]).Trim();
                        oRXHeader.PatientState = Configuration.convertNullToString(oTable.Rows[0]["addrst"]).Trim();
                    }
                    oRXHeader.TblPatient = oTable;

                    oTable = new DataTable();
                    oTable = oPharmBL.GetPrivackAck(Configuration.convertNullToString(oRXHeader.PatientNo));
                    if (oTable.Rows.Count > 0)
                    {
                        if (Configuration.convertNullToString(oTable.Rows[0]["PatAccept"]).Trim().ToUpper() == "N")
                        {
                            oRXHeader.isNOPPSignRequired = true;
                        }
                        else
                        {
                            try
                            {
                                if (DateTime.Parse(oTable.Rows[0]["datesigned"].ToString()).AddMonths(Configuration.CInfo.PrivacyExpiry) > DateTime.Now)
                                {
                                    oRXHeader.isNOPPSignRequired = false;
                                }
                                else
                                {
                                    oRXHeader.isNOPPSignRequired = true;
                                }
                            }
                            catch
                            {
                                oRXHeader.isNOPPSignRequired = true;
                            }
                        }
                    }
                    else
                    {
                        oRXHeader.isNOPPSignRequired = true;
                    }
                    oRXHeaderList.Add(oRXHeader);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "BuildRxHeader()");
                throw exp;
            }
        }

        public void InsertTransRxData(DataTable dsRxInfo, RXDetail oRXDetail, int gridCount, int rowCount, ref RXHeader oRXHeader, out DataTable oTable) //PRIMEPOS-3446 Added rowCount parameter
        {
            DataRow[] oRxInfo = dsRxInfo.Select("RXNO = " + oRXDetail.RXNo + " AND NREFILL = " + oRXDetail.RefillNo); //PRIMEPOS-2694 20-Jun-2019 JY Added
            if (dsRxInfo.Columns.Contains("PartialFillNo"))
                oRxInfo = dsRxInfo.Select("RXNO = " + oRXDetail.RXNo + " AND NREFILL = " + oRXDetail.RefillNo + " AND PartialFillNo = " + oRXDetail.PartialFillNo);
            oTable = null;
            if (oTDRow != null) //Add by SRT (Abhidhek D) date : 6 March 2010
            {
                if (oRXHeader.TblPatient != null && oRXHeader.PatientNo.Trim() == oRxInfo[0]["PATIENTNO"].ToString())
                    oTable = oRXHeader.TblPatient;
                else
                {
                    PharmBL oPharmBL = new PharmBL();
                    oTable = oPharmBL.GetPatient(oRxInfo[0]["PATIENTNO"].ToString());//added by atul 07-jan-2011
                }
                string ezcap = oTable.Rows[0]["EZCAP"].ToString();
                // added 2 parameters counsellingReq & ezcap by atul 07-jan-2011
                if (oRXHeader.CounselingRequest == null)
                    oRXHeader.CounselingRequest = "";

                //End of added by Krishna
                #region PRIMEPOS-3271
                if (oTransDRXData.TransDetailRX != null && oTransDRXData.TransDetailRX.Rows.Count > 0 && rowCount == 1) //PRIMEPOS-3446
                {
                    oTransDRXData.TransDetailRX.Rows.Clear();
                }
                //PRIMEPOS-3446 Commented below logic
                //if (oTransDRXData.TransDetailRX != null && oTransDRXData.TransDetailRX.Rows.Count > 0)
                //{
                //    for (int i = oTransDRXData.TransDetailRX.Rows.Count - 1; i >= 0; i--)
                //    {
                //        DataRow dr = oTransDRXData.TransDetailRX.Rows[i];
                //        if (Convert.ToInt64(dr["RXNo"]) == oRXDetail.RXNo && Convert.ToInt16(dr["NRefill"]) == oRXDetail.RefillNo
                //            && Convert.ToString(dr["DrugNDC"]) == oRxInfo[0]["NDC"].ToString())
                //            dr.Delete();
                //    }
                //    oTransDRXData.TransDetailRX.AcceptChanges();
                //}
                #endregion

                oTransDRXData.TransDetailRX.AddRow(oTDRow.TransDetailID, Convert.ToDateTime(oRXDetail.RxDate), oRXDetail.RXNo, oRxInfo[0]["NDC"].ToString(), Configuration.convertNullToInt64(oRxInfo[0]["PATIENTNO"]), oRxInfo[0]["BILLTYPE"].ToString(), oRxInfo[0]["PATTYPE"].ToString(), (int)oRXDetail.RefillNo, oRXHeader.CounselingRequest, ezcap, Configuration.convertNullToString(oRxInfo[0]["DELIVERY"]), (int)oRXDetail.PartialFillNo);  //Sprint-25 - PRIMEPOS-2322 02-Feb-2017 JY Added logic to update correct PatientNo w.r.t. Rx //PRIMEPOS-3008 30-Sep-2021 JY Added Delivery

                ezcap = string.Empty;//end of added by atul 07-jan-2011
            }
        }

        public void InsertTransRxData(DataTable dsRxInfo, RXDetail oRXDetail, int gridCount, bool bNewRxHEader, ref RXHeader oRXHeaderNew, ref RXHeader oRXHeader, out DataTable oTable)
        {
            DataRow[] oRxInfo = dsRxInfo.Select("RXNO = " + oRXDetail.RXNo + " AND NREFILL = " + oRXDetail.RefillNo); //PRIMEPOS-2694 20-Jun-2019 JY Added
            if (dsRxInfo.Columns.Contains("PartialFillNo"))
                oRxInfo = dsRxInfo.Select("RXNO = " + oRXDetail.RXNo + " AND NREFILL = " + oRXDetail.RefillNo + " AND PartialFillNo = " + oRXDetail.PartialFillNo);
            oTable = null;
            if (oTDRow != null) //Add by SRT (Abhidhek D) date : 6 March 2010
            {
                if (oRXHeader.TblPatient != null && oRXHeader.PatientNo.Trim() == oRxInfo[0]["PATIENTNO"].ToString())
                    oTable = oRXHeader.TblPatient;
                else
                {
                    PharmBL oPharmBL = new PharmBL();
                    oTable = oPharmBL.GetPatient(oRxInfo[0]["PATIENTNO"].ToString());//added by atul 07-jan-2011
                }
                string ezcap = oTable.Rows[0]["EZCAP"].ToString();
                // added 2 parameters counsellingReq & ezcap by atul 07-jan-2011
                #region PRIMEPOS-2469 By Rohit Nair
                if (bNewRxHEader == true)
                {
                    if (oRXHeaderNew.CounselingRequest == null)
                    {
                        oRXHeaderNew.CounselingRequest = "";
                    }
                }
                else
                {
                    if (oRXHeader.CounselingRequest == null)
                    {
                        oRXHeader.CounselingRequest = "";
                    }
                }
                #endregion
                //End of added by Krishna
                if (gridCount == 0 && oTransDRXData.TransDetailRX.Rows.Count > 0)
                {
                    oTransDRXData.TransDetailRX.Rows.Clear();
                }

                oTransDRXData.TransDetailRX.AddRow(oTDRow.TransDetailID, Convert.ToDateTime(oRXDetail.RxDate), oRXDetail.RXNo, oRxInfo[0]["NDC"].ToString(), Configuration.convertNullToInt64(oRxInfo[0]["PATIENTNO"]), oRxInfo[0]["BILLTYPE"].ToString(), oRxInfo[0]["PATTYPE"].ToString(), (int)oRXDetail.RefillNo, oRXHeader.CounselingRequest, ezcap, Configuration.convertNullToString(oRxInfo[0]["DELIVERY"]), (int)oRXDetail.PartialFillNo);  //Sprint-25 - PRIMEPOS-2322 02-Feb-2017 JY Added logic to update correct PatientNo w.r.t. Rx //Added By Shitaljit(QuicSolv)on 17 May 2011    //PRIMEPOS-3008 30-Sep-2021 JY Added Delivery
                ezcap = string.Empty;//end of added by atul 07-jan-2011
            }
        }

        public RXDetail BuildRxDetail(DataRow oRxInfo)
        {
            RXDetail oRXDetail = new RXDetail();

            try
            {
                oRXDetail.RXNo = Convert.ToInt64(oRxInfo["RXNo"].ToString());
                oRXDetail.RefillNo = Convert.ToInt16(oRxInfo["nrefill"].ToString());
                oRXDetail.DrugName = oRxInfo["drgName"].ToString();
                if (oRxInfo.Table.Columns.Contains("PartialFillNo"))
                    oRXDetail.PartialFillNo = Configuration.convertNullToShort(oRxInfo["PartialFillNo"]);
                else
                    oRXDetail.PartialFillNo = 0;
                if (oRxInfo["datef"].ToString().Length >= 10)
                {
                    oRXDetail.RxDate = Convert.ToDateTime(oRxInfo["datef"].ToString()).ToShortDateString();
                }
                else
                {
                    oRXDetail.RxDate = "";
                }

                DataTable dt = new DataTable("Claims");
                dt = oRxInfo.Table.Clone();
                dt.ImportRow(oRxInfo);
                oRXDetail.TblClaims = dt;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "BuildRxDetail()");
                throw exp;
            }
            return oRXDetail;
        }

        public TransDetailRow GetExistingRow(TransDetailRow oNewRow, bool isCompanionItem)
        {
            logger.Trace("GetExistingRow() - " + clsPOSDBConstants.Log_Entering);
            TransDetailRow oTDRow = null;

            if (oNewRow.ItemID != "RX")
            {
                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    if (oRow.ItemID == oNewRow.ItemID &&
                        oRow.Price == oNewRow.Price &&
                        oRow.IsCompanionItem == isCompanionItem &&
                        oRow.ItemDescription == oNewRow.ItemDescription &&
                        oRow.TaxID == oNewRow.TaxID &&
                        oRow.TransDetailID != oNewRow.TransDetailID
                        )
                    {
                        oTDRow = oRow;
                        break;
                    }
                    if (oRow.ItemID == oNewRow.ItemID &&
                        oRow.OrignalPrice == oNewRow.Price &&
                        oRow.IsCompanionItem == isCompanionItem &&
                        oRow.ItemDescription == oNewRow.ItemDescription &&
                        oRow.TaxID == oNewRow.TaxID &&
                        oRow.TransDetailID != oNewRow.TransDetailID
                        ) //PRIMEPOS-3443
                    {
                        oTDRow = oRow;
                        break;
                    }
                    else if (oRow.ItemID == oNewRow.ItemID &&
                        oRow.IsCompanionItem == isCompanionItem &&
                        oRow.ItemDescription == oNewRow.ItemDescription &&
                        oRow.TaxID == oNewRow.TaxID &&
                        oRow.TransDetailID != oNewRow.TransDetailID &&
                        oRow.ItemComboPricingID > 0
                        )
                    {
                        oTDRow = oRow;
                        break;
                    }
                    #region Sprint-27 - PRIMEPOS-2413 18-Sep-2017 JY Added 
                    else if (oRow.ItemID == oNewRow.ItemID &&
                        oRow.IsCompanionItem == isCompanionItem &&
                        oRow.ItemDescription == oNewRow.ItemDescription &&
                        oRow.TaxID == oNewRow.TaxID &&
                        oRow.TransDetailID != oNewRow.TransDetailID
                        )
                    {
                        Item oTempItem = new Item();
                        ItemData oTempItemData = oTempItem.Populate(oRow.ItemID);
                        if (oTempItemData.Item.Rows.Count > 0)
                        {
                            ItemRow oItemRow = oTempItemData.Item[0];
                            if (oItemRow.isOnSale && oItemRow.SaleStartDate != DBNull.Value && oItemRow.SaleEndDate != DBNull.Value && DateTime.Now.Date >= Convert.ToDateTime(oItemRow.SaleStartDate).Date && DateTime.Now.Date <= Convert.ToDateTime(oItemRow.SaleEndDate).Date)
                            {
                                oTDRow = oRow;
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
            logger.Trace("GetExistingRow() - " + clsPOSDBConstants.Log_Exiting);
            return oTDRow;
        }

        public TransHeaderData GetOnHoldTransData(int TransID, out TransDetailData oHoldTransDData, bool IsCustomerDriven = false)//2915
        {
            TransHeaderData oHoldTransHData;
            oHoldTransDData = null;
            try
            {
                TransHeaderSvr oHoldTransHSvr = new TransHeaderSvr();
                TransDetailSvr oHoldTransDSvr = new TransDetailSvr();
                TransDetailTaxSvr oTDTaxSvr = new TransDetailTaxSvr();
                TaxCodes oTaxCodes = new TaxCodes();
                TaxCodesData oTaxCodeTada = new TaxCodesData();
                int iRow = 1;
                oHoldTransHData = oHoldTransHSvr.GetOnHoldData(TransID);
                oHoldTransDData = oHoldTransDSvr.GetOnHoldTransDetail(TransID);
                oTDTaxData = oTDTaxSvr.GetOnHoldTransDetail(TransID);
                foreach (TransDetailTaxRow tdr in oTDTaxData.TransDetailTax)
                {
                    if (!string.IsNullOrWhiteSpace(oTDTaxData.TransDetailTax.Rows[oTDTaxData.TransDetailTax.Rows.Count - 1]["ItemRow"].ToString()))
                        iRow += Convert.ToInt32(oTDTaxData.TransDetailTax.Rows[oTDTaxData.TransDetailTax.Rows.Count - 1]["ItemRow"].ToString());
                    tdr.ItemRow = iRow;
                    iRow++;
                }
                foreach (TransDetailRow oRow in oHoldTransDData.TransDetail.Rows)
                {
                    string sTaxIds = "";
                    oRow.ItemDescriptionMasked = MaskDrugName(oRow.ItemID,oRow.ItemDescription);  //PRIMEPOS-3130
                    if (IsItemTaxableForTrasaction(oTDTaxData, oRow.ItemID, out sTaxIds, oRow.TransDetailID) == true)
                    {
                        oTaxCodesData = oTaxCodes.PopulateList(" WHERE TaxID IN " + sTaxIds);
                        if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                        {
                            if (oTaxCodesData.TaxCodes.Rows[0][clsPOSDBConstants.TaxCodes_Fld_TaxCode].ToString().Trim().ToUpper() == clsPOSDBConstants.TaxCodes_Fld_RxTax.ToUpper())
                            {
                                oRow.TaxCode = string.Empty;
                                foreach (TaxCodesRow tRow in oTaxCodesData.TaxCodes)
                                {
                                    oRow.TaxCode += tRow.TaxCode + ",";
                                }
                                if (oRow.TaxCode.EndsWith(","))
                                {
                                    oRow.TaxCode = oRow.TaxCode.Substring(0, oRow.TaxCode.Length - 1);
                                }
                            }
                            else
                                CalculateTaxOnHold(oRow, oTaxCodesData, oTDTaxData);
                        }
                    }
                }
                //Added By shoitaljit(QuicSolv) on 8 nov 2011
                int RowIndex = 0;
                int TransDetailID = 1;
                if (!IsCustomerDriven)//2915
                {
                    for (RowIndex = 0; RowIndex < oHoldTransDData.TransDetail.Rows.Count; RowIndex++)
                    {
                        oHoldTransDData.TransDetail[RowIndex].TransDetailID = TransDetailID;
                        TransDetailID++;
                    }
                }
                //END of Added By shoitaljit(QuicSolv) on 8 nov 2011

                oTransHData.TransHeader[0].IsDelivery = oHoldTransHData.TransHeader[0].IsDelivery;
                oTransHData.TransHeader[0].WasonHold = oHoldTransHData.TransHeader[0].WasonHold;   //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
                oTransHData.TransHeader[0].DeliverySigSkipped = oHoldTransHData.TransHeader[0].DeliverySigSkipped; //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
                oTransHData.TransHeader[0].DeliveryAddress = oHoldTransHData.TransHeader[0].DeliveryAddress;

                //2915
                oTransHData.TransHeader[0].IsCustomerDriven = oHoldTransHData.TransHeader[0].IsCustomerDriven;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetOnHoldTransData()");
                throw exp;
            }
            return oHoldTransHData;
        }

        public TransDetailRow GetTransactionDetatilRow(string transId, int index)
        {
            TransDetailRow TempRow = null;
            try
            {

                if (transId.Trim() == "")
                {
                    TempRow = oTransDData.TransDetail[index];
                    SetRowTrans(TempRow);
                }
                else
                {
                    DataRow[] dr = (oTransDData.TransDetail.Select(clsPOSDBConstants.TransDetail_Fld_TransDetailID + "=" + transId));
                    TempRow = oTransDData.TransDetail.NewTransDetailRow();
                    TempRow.ItemArray = ((TransDetailRow)dr[0]).ItemArray;
                    SetRowTrans(TempRow);
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransactionDetatilRow()");
                throw exp;
            }
            return TempRow;
        }

        public void GetTransDetailRow(string transDetailID, int index)
        {
            if (transDetailID.Trim() == "")
            {
                oTDRow = oTransDData.TransDetail[index];
                SetRowTrans(oTDRow);
            }
            else
            {
                DataRow[] dr = (oTransDData.TransDetail.Select(clsPOSDBConstants.TransDetail_Fld_TransDetailID + "=" + transDetailID));
                oTDRow = (TransDetailRow)dr[0];
                SetRowTrans(oTDRow);
            }
        }

        public TransHeaderData GetTransactionHeader(bool isOnHoldTrans, bool isCallofRetTrans, int onHoldTransID, int transId)
        {
            TransHeaderSvr oHSvr = new TransHeaderSvr();
            TransHeaderData oHData = null;
            try
            {
                if (isOnHoldTrans == false && isCallofRetTrans == true)
                {
                    oHData = oHSvr.Populate(transId);
                }
                else if (isOnHoldTrans == true)
                {
                    oHData = oHSvr.GetOnHoldData(onHoldTransID);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransactionHeader()");
                throw exp;
            }

            return oHData;

        }

        public bool CheckReturnTransactionId()
        {
            bool isReturn = false;
            foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
            {
                if (oRow.ReturnTransDetailId > 0)
                {
                    isReturn = true;
                    break;
                }
            }
            return isReturn;
        }

        public void SetDescForTransData()
        {

            foreach (TransDetailRow oRow in oTransDData.Tables[0].Rows)
            {
                string strCat = "";
                if (oRow.IsIIAS == true && Configuration.CInfo.TagFSA == true)
                {
                    strCat = "F";
                }
                //Added oRow.TaxAmount > 0 by shitaljit to Tag item as taxable only if there is Tax amount charged
                //Sprint-22 - PRIMEPOS-1704 04-Dec-2015 JY removed oRow.TaxID > 0 condition as this is PosTransactionDetail.TaxId and now we are not using it
                if (oRow.TaxAmount > 0 && Configuration.CInfo.TagTaxable == true)
                {
                    strCat += "T";
                }

                if (oRow.IsMonitored == true && Configuration.CInfo.TagMonitored == true)
                {
                    strCat += "M";
                }
                if (oRow.IsEBTItem == true && Configuration.CInfo.TagEBT == true)
                {
                    strCat += "E";
                }
                if (Configuration.CSetting.TagSolutran == true && Configuration.convertNullToInt(oRow.S3TransID) > 0)   //PRIMEPOS-2836 21-Apr-2020 JY Added
                {
                    strCat += "S";
                }

                if (strCat != "")
                    oRow.ItemDescription = "(" + strCat + ") " + oRow.ItemDescription;
            }

        }

        public decimal GetChangeDueFromPaymentTrans(string changeDue)
        {
            decimal dChangeDue = 0;
            try
            {

                if (CurrentTransactionType == POSTransactionType.SalesReturn)
                {
                    foreach (POSTransPaymentRow oPayRow in oPOSTransPaymentData.POSTransPayment.Rows)
                    {
                        if (oPayRow.TransTypeCode.Trim() == "1")
                        {
                            dChangeDue += oPayRow.Amount;
                        }
                    }
                }
                else
                {
                    dChangeDue = Convert.ToDecimal(changeDue);
                }

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetChangeDueFromPaymentTrans()");
                throw exp;
            }
            return dChangeDue;
        }

        public void CaptureDrugInformation(DataTable RxWithValidClass, DataTable DrugClassInfoCapture)  //PRIMEPOS-2547 23-Jul-2018 JY Changed datatype of DrugClassInfoCapture
        {
            //PRIMEPOS-2683 25-Jun-2019 JY did few changes related to exception handelling
            try
            {
                //if (Configuration.CPOSSet.ControlByID && !SkipForDelivery() && RxWithValidClass != null && RxWithValidClass.Rows.Count > 0 && CurrentTransactionType == POSTransactionType.Sales) //PRIMEPOS-2547 03-Jul-2018 JY Commented
                if (Configuration.CPOSSet.ControlByID > 0 && !SkipForDelivery() && RxWithValidClass != null && RxWithValidClass.Rows.Count > 0 && CurrentTransactionType == POSTransactionType.Sales)   //PRIMEPOS-2547 03-Jul-2018 JY Added
                {
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Capturing drug information");
                    if (RxWithValidClass.Columns.Count < 5)
                    {
                        if (!RxWithValidClass.Columns.Contains("PICKUPDATE"))
                            RxWithValidClass.Columns.Add("PICKUPDATE", typeof(DateTime));

                        if (!RxWithValidClass.Columns.Contains("IDTYPE"))
                            RxWithValidClass.Columns.Add("IDTYPE", typeof(string));

                        if (!RxWithValidClass.Columns.Contains("RELATION"))
                            RxWithValidClass.Columns.Add("RELATION", typeof(string));

                        if (!RxWithValidClass.Columns.Contains("IDNUM"))
                            RxWithValidClass.Columns.Add("IDNUM", typeof(string));

                        if (!RxWithValidClass.Columns.Contains("STATE"))
                            RxWithValidClass.Columns.Add("STATE", typeof(string));

                        if (!RxWithValidClass.Columns.Contains("LASTNAME"))
                            RxWithValidClass.Columns.Add("LASTNAME", typeof(string));

                        if (!RxWithValidClass.Columns.Contains("FIRSTNAME"))
                            RxWithValidClass.Columns.Add("FIRSTNAME", typeof(string));
                        if (!RxWithValidClass.Columns.Contains("DriversLicenseExpDate"))
                            RxWithValidClass.Columns.Add("DriversLicenseExpDate", typeof(DateTime));    //PRIMEPOS-3065 10-Mar-2022 JY Added
                    }

                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - DrugClassInfoCapture");

                    if (DrugClassInfoCapture != null && DrugClassInfoCapture.Rows.Count > 0)
                    {
                        DataRow[] tempRow = DrugClassInfoCapture.Select("1 = 1");
                        foreach (DataRow row in RxWithValidClass.Rows)
                        {
                            try
                            {
                                //PRIMEPOS-2547 23-Jul-2018 JY modified code
                                if (Configuration.CPOSSet.AskVerificationIdMode == 1)
                                {
                                    tempRow = DrugClassInfoCapture.Select("RxNo Like '" + Configuration.convertNullToInt64(row["RxNo"]).ToString() + "' AND RefillNo = '" + Configuration.convertNullToInt(row["RefillNo"]).ToString() + "' AND PartialFillNo = '" + Configuration.convertNullToInt(row["PartialFillNo"]).ToString() + "'");
                                }

                                row["PICKUPDATE"] = DateTime.Now;
                                row["IDTYPE"] = Configuration.convertNullToString(tempRow[0]["IDTYPE"]).Trim();
                                row["RELATION"] = Configuration.convertNullToString(tempRow[0]["RELATION"]).Trim();
                                row["IDNUM"] = Configuration.convertNullToString(tempRow[0]["IDNUM"]).Trim();
                                row["STATE"] = Configuration.convertNullToString(tempRow[0]["STATE"]).Trim();
                                row["LASTNAME"] = Configuration.convertNullToString(tempRow[0]["LASTNAME"]).Trim();
                                row["FIRSTNAME"] = Configuration.convertNullToString(tempRow[0]["FIRSTNAME"]).Trim();
                                #region PRIMEPOS-3065 10-Mar-2022 JY Added
                                try
                                {
                                    if (tempRow[0]["DriversLicenseExpDate"] != null && Convert.ToDateTime(tempRow[0]["DriversLicenseExpDate"]) != DateTime.MinValue)
                                        row["DriversLicenseExpDate"] = tempRow[0]["DriversLicenseExpDate"];
                                }
                                catch { }
                                #endregion
                            }
                            catch (Exception Ex)
                            {
                                logger.Fatal(Ex, "CaptureDrugInformation() - Capturing drug information failed for any parameter");
                            }
                        }
                    }
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Capturing drug information completed");
                }
                else if (RxWithValidClass != null && RxWithValidClass.Rows.Count > 0)
                {
                    RxWithValidClass.Clear();
                    logger.Trace(clsPOSDBConstants.Log_Module_Transaction + " btnSave - Done Clearing Captured drug information");
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "CaptureDrugInformation()");
                //ErrorHandler.logException(exp, "", ""); //09-Dec-2019 JY Added ErrorLog 
                //throw exp;    //23-Dec-2019 JY - no need to throw the exception as we are already recording it for our reference
            }
        }

        public string GetTransactionDetailString(int transId)
        {
            string result = "";
            try
            {

                TransDetailData oTransDetData = PopulateTransactionDetailData(transId);
                System.Collections.Generic.List<string> lstDetailId = new System.Collections.Generic.List<string>();
                foreach (TransDetailRow row in oTransDetData.Tables[0].Rows)
                {
                    lstDetailId.Add(row[0].ToString());
                }
                result = string.Join(",", lstDetailId.ToArray());
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransactionDetailString()");
                throw exp;
            }
            return result;
        }

        public void GetDrugClassInfoCapture(string verificationId, string relation, ref string IDTYPE, ref string RELATION)
        {
            switch (verificationId.Trim())
            {
                case "Driver's License":
                    IDTYPE = "06";
                    break;
                case "Military ID":
                    IDTYPE = "01";
                    break;
                case "US Passport":
                    IDTYPE = "05";
                    break;
                case "Permanent Resident Card":
                    IDTYPE = "04";
                    break;
                case "Social Security Card":
                    IDTYPE = "07";
                    break;
                case "State Issued ID":
                    IDTYPE = "02";
                    break;
                case "Native American Tribal Documents(ID)":
                    IDTYPE = "08";
                    break;
                case "Unique System ID":
                    IDTYPE = "03";
                    break;
                case "Other":
                    IDTYPE = "09";
                    break;
                default:
                    IDTYPE = "00";
                    break;
            }

            switch (relation.Trim())
            {
                case "Patient":
                    RELATION = "01";
                    break;
                case "Parent/Legal Guardian":
                    RELATION = "02";
                    break;
                case "Spouse":
                    RELATION = "03";
                    break;
                case "Caregiver": //PRIMEPOS-3267
                    RELATION = "04";
                    break;
                case "Other":
                    RELATION = "05";
                    break;
                default:
                    IDTYPE = "00";
                    break;
            }
        }

        public bool? CapturePatientConsent(List<PatientConsent> patientList, string patientName, string rxs) //PRIMEPOS-CONSENT SAJID DHUKKA //PRIMEPOS-2866,PRIMEPOS-2871 //PRIMEPOS-3192 Added rxs
        {
            logger.Trace("CapturePatientConsent");
            bool? retVal = null;
            // Need to check distinct

            PharmBL oPBl = new PharmBL();
            foreach (PatientConsent consent in patientList)
            {
                if (!consent.IsConsentSkip)
                {
                    try
                    {
                        DataSet dsActiveConsentDetails = new DataSet();
                        int patientNo = 0;
                        bool isPrescriptionAutoRefillActive = false; //PRIMEPOS-3269
                        int.TryParse(oRXHeaderList[0].PatientNo, out patientNo);
                        dsActiveConsentDetails = GetActiveConsentBySourceId(consent.ConsentSourceID, patientNo.ToString(), oPBl);
                        if (consent.ConsentSourceName != null && consent.ConsentSourceName.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_AUTO_REFILL.ToUpper())//PRIMEPOS-3192N //PRIMEPOS-3287
                        {
                            string consentText = dsActiveConsentDetails.Tables["ConsentTextVersion"].Rows[0].Field<string>("ConsentText");
                            string pharmacyName = Configuration.CInfo.StoreName;
                            consentText = consentText.Replace("_", pharmacyName);
                            string updatedConsentText = consentText + Environment.NewLine + rxs;
                            int indexValueConsentText = dsActiveConsentDetails.Tables["ConsentTextVersion"].Columns["ConsentText"].Ordinal;
                            dsActiveConsentDetails.Tables["ConsentTextVersion"].Rows[0].SetField<string>(indexValueConsentText, updatedConsentText);
                            #region PRIMEPOS-3269
                            isPrescriptionAutoRefillActive = oPBl.isPrescriptionConsentActive(consent.ConsentSourceID, Constants.CONSENT_TYPE_CODE_PRESCRIPTION_AUTO_REFILL);
                            #endregion
                        }
                        DataTable dt = new DataTable();
                        dt = oPBl.PopulateConsentNameBasedOnID(consent.ConsentSourceID.ToString());
                        if (dt.Rows.Count > 0)
                        {
                            consent.ConsentSourceName = dt.Rows[0]["Name"].ToString();
                        }
                        PatientConsent oConsent = null;
                        if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected())
                        {
                            retVal = SigPadUtil.DefaultInstance.CapturePatientConsent(consent.ConsentSourceName, out oConsent, dsActiveConsentDetails);
                        }
                        //PRIMEPOS-CONSENT SAJID DHUKKA //PRIMEPOS-2866,PRIMEPOS-2871
                        if (retVal.HasValue && retVal.Value && oConsent!=null) //PRIMEPOS-3442
                        {
                            if (consent.ConsentSourceName.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_HEALTHIX.ToUpper())
                            {
                                //case MMS.Device.Global.Constants.CONSENT_SOURCE_HEALTHIX:
                                consent.IsConsentSkip = oConsent.IsConsentSkip;
                                consent.ConsentTypeCode = oConsent.ConsentTypeCode;
                                consent.ConsentCaptureDate = DateTime.Now;
                                consent.ConsentEffectiveDate = DateTime.Now;
                                consent.ConsentStatusCode = oConsent.ConsentStatusCode;
                                consent.PatConsentRelationShipDescription = oConsent.PatConsentRelationShipDescription;
                                consent.ConsentTextID = oPBl.GetConsentTextID(consent.ConsentSourceID, 1);
                                consent.ConsentTypeID = oPBl.GetConsentTypeID(consent.ConsentSourceID, oConsent.ConsentTypeCode);//PRIMEPOS-3120
                                consent.ConsentStatusID = oPBl.GetConsentStatusID(consent.ConsentSourceID, oConsent.ConsentStatusCode);//PRIMEPOS-3120
                                consent.PatConsentRelationID = oPBl.GetConsentRelationShipID(consent.ConsentSourceID, oConsent.PatConsentRelationShipDescription);//PRIMEPOS-3120
                                consent.ConsentEndDate = DateTime.Now.AddDays(oPBl.GetConsentValidityPeriod(consent.ConsentSourceID, consent.ConsentStatusID) + 1).AddSeconds(-1);//PRIMEPOS-3120
                                consent.SigneeName = Convert.ToString(oConsent.PatConsentRelationShipDescription).ToLower() == MMS.Device.Global.Constants.CONSENT_RELATIONSHIP_SELF.ToLower() ? patientName : "";//PRIMEPOS-3120
                            }
                            //break;
                            //case MMS.Device.Global.Constants.CONSENT_SOURCE_RxAuto_Refill: //PRIMEPOS-CONSENT SAJID DHUKKA
                            else
                            {
                                if (!oConsent.IsConsentSkip)
                                {
                                    consent.ConsentTypeCode = oConsent.ConsentTypeCode;
                                    consent.ConsentTextID = oConsent.ConsentTextID;
                                    if (isPrescriptionAutoRefillActive) //PRIMEPOS-3269
                                    {
                                        consent.ConsentTypeID = oPBl.GetConsentTypeID(consent.ConsentSourceID, Constants.CONSENT_TYPE_CODE_PRESCRIPTION_AUTO_REFILL);
                                    }
                                    else
                                    {
                                        consent.ConsentTypeID = oPBl.GetConsentTypeID(consent.ConsentSourceID, "");//PRIMEPOS-3120
                                    }
                                    consent.ConsentStatusID = oConsent.ConsentStatusID;
                                    consent.ConsentStatusCode = oConsent.ConsentStatusCode;
                                    consent.PatConsentRelationID = oConsent.PatConsentRelationID;
                                    consent.ConsentCaptureDate = DateTime.Now;
                                    consent.ConsentEffectiveDate = DateTime.Now;
                                    consent.ConsentEndDate = DateTime.Today.AddDays(oPBl.GetConsentValidityPeriod(consent.ConsentSourceID, consent.ConsentStatusID) + 1).AddSeconds(-1);//PRIMEPOS-3120
                                    consent.SigneeName = Convert.ToString(oConsent.PatConsentRelationShipDescription).ToLower() == MMS.Device.Global.Constants.CONSENT_RELATIONSHIP_SELF.ToLower() ? patientName : "";//PRIMEPOS-3120
                                }
                                consent.IsConsentSkip = oConsent.IsConsentSkip;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = "An Error occured while Fetching Pat Consent Values" + ex.Message;
                        logger.Fatal(ex, message);
                    }
                }
            }

            foreach (RXHeader oRXHeader in oRXHeaderList)
            {
                logger.Trace("oRXHeader.IsConsentRequired = " + oRXHeader.IsConsentRequired);
                if (oRXHeader.IsConsentRequired)
                {
                    oRXHeader.PatConsent = patientList;
                }
            }
            logger.Trace("CapturePatientConsent() - " + clsPOSDBConstants.Log_Exiting);
            return retVal;
        }

        public bool IsConsentRequired(out List<PatientConsent> patientListout)//PRIMEPOS-2866,PRIMEPOS-2871
        {
            bool isConsentRequired = false;
            try
            {
                IEnumerable<PatientConsent> patientConsenttempout = new List<PatientConsent>();//PRIMEPOS-2866,PRIMEPOS-2871
                if (Configuration.CInfo.EnableConsentCapture /*&& !string.IsNullOrWhiteSpace(Configuration.CInfo.SelectedConsentSource)*/ && !SkipForDelivery())//Modified By Rohit Nair on 08/29/2017 for skip for delivery//PRIMEPOS-2866 Arvind removed one check of SelectedConsentSource
                {
                    List<PatientConsent> patientConsentTemp = new List<PatientConsent>();
                    foreach (RXHeader oRXHeader in oRXHeaderList)
                    {
                        //Modified By Rohit Nair for PrimePOS-2448
                        //if (!(oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch) && POSTransaction.HasActiveConsent(oRXHeader)) // PRIMEPOS-CONSENT SAJID DHUKKA

                        if (oRXHeader.IsIntakeBatch && Configuration.CInfo.SkipSignatureForInatkeBatch)//PRIMEPOS-2866,PRIMEPOS-2871
                        {
                            logger.Trace("oRXHeader.IsConsentRequired()=false");
                            oRXHeader.IsConsentRequired = false;
                        }
                        else
                        {
                            logger.Trace("oRXHeader.IsConsentRequired()=true");
                            oRXHeader.IsConsentRequired = true;
                            isConsentRequired = true;
                            oRXHeader.PatConsent = POSTransaction.GetActiveConsentList(oRXHeader);//PRIMEPOS-2866,PRIMEPOS-2871
                            //if (oRXHeader.PatConsent?.Count == 0)//PRIMEPOS-3192
                            //{
                            //    isConsentRequired = false;
                            //    oRXHeader.IsConsentRequired = false;
                            //}
                            patientConsentTemp.AddRange(oRXHeader.PatConsent);//PRIMEPOS-2866,PRIMEPOS-2871 //PRIMEPOS-2950 29-Mar-2021 JY Moved inside the else
                        }
                    }
                    patientConsenttempout = patientConsentTemp.GroupBy(o => new { o.ConsentSourceID }).Select(o => o.FirstOrDefault());//PRIMEPOS-2866,PRIMEPOS-2871
                }
                patientListout = patientConsenttempout.ToList();//PRIMEPOS-2866,PRIMEPOS-2871
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "IsConsentRequired()");
                throw exp;
            }
            return isConsentRequired;
        }

        //PRIMEPOS-CONSENT SAJID DHUKKA//PRIMEPOS-2866,PRIMEPOS-2871
        public DataSet GetActiveConsentBySourceId(int sourceId, string patientNo, PharmBL oPharmBL)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                dt.TableName = "Patient";
                dt = oPharmBL.GetPatient(patientNo);
                ds.Tables.Add(dt.Copy());
                dt = new DataTable();
                dt.TableName = "ConsentText";
                dt = oPharmBL.GetConsentTextById(sourceId);
                ds.Tables.Add(dt.Copy());
                dt = new DataTable();
                dt.TableName = "ConsentRelationShip";
                dt = oPharmBL.GetConsentRelationshipById(sourceId);
                ds.Tables.Add(dt.Copy());
                dt.TableName = "ConsentStatus";
                dt = oPharmBL.GetActiveConsentStatusById(sourceId);
                ds.Tables.Add(dt.Copy());
                dt.TableName = "ConsentType";
                dt = oPharmBL.GetActiveConsentTypeById(sourceId);
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, string.Concat("GetActiveConsentBySourceId()", "PatientNo=", patientNo, "SourceId=", sourceId));
                if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
                    throw exp;
            }
            return ds;
        }

        public DataTable GetTransDetailTaxOnHold(System.Int32 TransId)
        {
            DataTable dtTransDetailTax = null;
            try
            {
                TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTaxOnHold(Configuration.convertNullToInt(((TransHeaderRow)oTransHData.TransHeader.Rows[0]).TransID)); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added

            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetTransDetailTaxOnHold()");
                throw exp;
            }
            return dtTransDetailTax;
        }

        public CustomerRow GetCustomerRowFromRxHeaderList()
        {
            CustomerRow oCRow = null;
            Customer oCustomer = new Customer();
            CustomerData oCustdata = new CustomerData();
            try
            {
                string patients = "";
                int patientNo = 0;
                foreach (RXHeader oRXHeader in oRXHeaderList)
                {
                    patientNo = Configuration.convertNullToInt(oRXHeader.PatientNo);
                    clsCorePrimeRXHelper.ImportPatientAsCustomer(patientNo);
                    patients += patientNo + ",";
                }
                patients = patients.Substring(0, patients.Length - 1);
                //Added By Shitaljit on 9 May 2013 for PRIMEPOS 408 wrong customer is save for on Hold Trans.
                oTransHRow.DeliveryAddress = clsCorePrimeRXHelper.GetPatientDeliveryAddress(patients);
                oCustdata = oCustomer.GetCustomerByPatientNo(patientNo);
                if (oCustdata.Tables[0].Rows.Count > 0)
                {
                    oCRow = (CustomerRow)oCustdata.Customer.Rows[0];
                }


            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetCustomerRowFromRxHeaderList()");
                throw exp;
            }
            return oCRow;
        }

        public int GetExistingQuantity(TransDetailRow oNewRow, TransDetailData oTransDData)
        {
            logger.Trace("GetExistingQuantity() - " + clsPOSDBConstants.Log_Entering);
            int existingQty = 0;

            if (oNewRow.ItemID != "RX")
            {
                foreach (TransDetailRow oRow in oTransDData.TransDetail.Rows)
                {
                    if (oRow.ItemID == oNewRow.ItemID &&
                        oRow.ItemDescription == oNewRow.ItemDescription
                        )
                    {
                        existingQty += oRow.QTY;
                    }
                }
            }
            logger.Trace("GetExistingQuantity() - " + clsPOSDBConstants.Log_Exiting);
            return existingQty;
        }

        public void SeperateItem(ref System.String ItemID, ref System.Int32 QTY, ref System.Decimal SalePrice, out DataSet dsItem)
        {
            try
            {
                dsItem = null;
                int index = 0;
                index = ItemID.IndexOf("/", 1);
                skipMoveNext = false;
                if (index > 0)
                {
                    System.String str = ItemID.Substring(0, index);
                    ItemID = ItemID.Substring(index + 1);
                    if (clsCoreUIHelper.isNumeric(str) == true)
                    {
                        QTY = Configuration.convertNullToInt(str);
                        if (CurrentTransactionType == POSTransactionType.SalesReturn && Math.Abs(QTY) < 1)
                            QTY = 0;
                        else if (CurrentTransactionType == POSTransactionType.Sales && QTY < 1)
                            QTY = 0;
                        skipMoveNext = true;
                    }
                    else
                        QTY = 0;
                }
                if (CurrentTransactionType == POSTransactionType.SalesReturn && QTY > 0)
                {
                    QTY = -QTY;
                }
                if (ItemID.Trim() != "")    //PRIMEPOS-2830 10-Apr-2020 JY Added condition to avoid exception
                {
                    index = 0;
                    index = ItemID.IndexOf("@", 1);
                    if (index > 0)
                    {
                        System.String str = ItemID.Substring(0, index);
                        ItemID = ItemID.Substring(index + 1);
                        if (clsCoreUIHelper.isNumeric(str) == true)
                        {
                            if (str.IndexOf(".") > -1)
                            {
                                SalePrice = Configuration.convertNullToDecimal(str);
                            }
                            else
                            {
                                SalePrice = Configuration.convertNullToDecimal(str) / 100;
                            }
                            if (SalePrice < 0 && ItemID.ToUpper().Equals(Configuration.CouponItemCode) == false)
                                SalePrice = 0;
                            skipMoveNext = true;
                        }
                        else
                            SalePrice = 0;
                    }
                }
                if (ItemID.Contains("@") == true)
                {
                    index = 0;
                    index = ItemID.IndexOf("@", 0);
                    if (index == 0)
                    {
                        ItemID = ItemID.Substring(index + 1);
                    }
                }
                if (ItemID.ToUpper().StartsWith("V") == true)
                {
                    index = 0;
                    index = ItemID.IndexOf("\\", 0);
                    if (index == 1)
                    {
                        string ItemvedorID = ItemID.Substring(index + 1);
                        ItemVendor oIV = new ItemVendor();
                        ItemVendorData oIVData = oIV.Populate(ItemvedorID);
                        if (oIVData != null)
                        {
                            if (oIVData.ItemVendor.Rows.Count == 1)
                            {
                                ItemVendorRow oIVRow = oIVData.ItemVendor[0];
                                ItemID = oIVRow.ItemID;
                            }
                            else
                            {
                                ItemSvr itemSvr = new ItemSvr();
                                bool isVendRequired = false;
                                bool isItemVendorRequired = true;
                                string WhereClause = "WHERE IV." + clsPOSDBConstants.ItemVendor_Fld_VendorItemID + "= '" + ItemvedorID + "'";
                                dsItem = itemSvr.PopulateAdvSearch(WhereClause, ref isVendRequired, ref isItemVendorRequired);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SeperateItem()");
                throw exp;
            }
        }

        #endregion

        #region Department

        public DepartmentData PopulateDepartment(string deptCode)
        {
            DepartmentData oDeptData = new DepartmentData();
            try
            {
                Department oDept = new Department();
                oDeptData = oDept.Populate(deptCode);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "PopulateDepartment()");
                throw exp;
            }


            return oDeptData;
        }

        #endregion

        public RXHeader getPatientFromRxHeaderList(RXHeaderList oRXHeaderList, string sRXNo)
        {
            RXHeader oPatient = null;
            try
            {
                foreach (RXHeader oHeader in oRXHeaderList)
                {
                    foreach (RXDetail oDetail in oHeader.RXDetails)
                    {
                        if (oDetail.RXNo.ToString() == sRXNo)
                        {
                            oPatient = oHeader;
                            break;
                        }
                    }

                    if (oPatient != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "getPatientFromRxHeaderList()");
                throw exp;
            }
            return oPatient;

        }

        #region PRIMEPOS-2034 08-Mar-2018 JY Added to check coupon status
        public Boolean IsCouponExpired(Int64 CouponID)
        {
            try
            {
                Coupon oCoupon = new Coupon();
                CouponData oCouponData = oCoupon.Populate(CouponID);
                if (!Configuration.isNullOrEmptyDataSet(oCouponData))
                {
                    CouponRow oCouponRow = (CouponRow)oCouponData.Coupon.Rows[0];
                    DateTime EndDate;
                    DateTime.TryParse(oCouponRow.EndDate.ToShortDateString(), out EndDate);
                    DateTime currentdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    if (EndDate < currentdate)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "IsCouponExpired()");
                return false;
            }
        }
        #endregion


        #region Solutran - PRIMEPOS-2663
        public DataSet GetTransPaymentDetail(String TransId, String TranstypeCode)
        {
            System.Data.IDbConnection oConn = null;
            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            POSTransPaymentSvr oPaymentSvr = new POSTransPaymentSvr();

            return oPaymentSvr.GetTransPaymentDetail(TransId, oConn, TranstypeCode);
        }
        #endregion
        #region REVERSAL - PRIMEPOS-2738
        public DataSet GetPOSPaymentDetail(String TransId, String TranstypeCode)
        {
            System.Data.IDbConnection oConn = null;
            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            POSTransPaymentSvr oPaymentSvr = new POSTransPaymentSvr();

            return oPaymentSvr.GetPOSPaymentDetail(TransId, oConn, TranstypeCode);
        }
        public DataSet GetPaymentDetail(String TransId, String TranstypeCode, DataSet oOrigPayTransData, bool isPrimeRxPay)
        {
            System.Data.IDbConnection oConn = null;
            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            POSTransPaymentSvr oPaymentSvr = new POSTransPaymentSvr();

            return oPaymentSvr.GetPaymentDetails(TransId, oConn, TranstypeCode, oOrigPayTransData, isPrimeRxPay);
        }
        public void SetReversedAmountDetails(DataSet oOrigPayTransData)
        {
            POSTransPaymentSvr oPaymentSvr = new POSTransPaymentSvr();
            oPaymentSvr.SetReversedAmountDetails(oOrigPayTransData);
        }
        #endregion
        #region BatchDelivery - NileshJ - PRIMERX-7688 - 23-Sept-2019
        // Check Rx Hold and Paid Transaction 
        public static string CheckOnHoldOrAlreadyPaid(DataRow row, out DataTable dtRxOnHold, out DataSet dsTransRxAlreadyPaid)
        {
            string status = string.Empty;
            dsTransRxAlreadyPaid = null;
            try
            {
                TransDetailRXSvr oSvr = new TransDetailRXSvr();
                if ((oSvr.RxIsOnHoldForDelivery(row["PatientNo"].ToString(), row["RxNo"].ToString(), out dtRxOnHold)))
                {
                    status = "Hold";
                }
                else if (POSTransaction.CheckAlreadyPaidForDelivery(row["RxNo"].ToString(), row["RefillNo"].ToString(), out dsTransRxAlreadyPaid) == true)
                {
                    status = "Paid";
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "CheckOnHoldOrAlreadyPaid(DataRow row, out DataTable dtRxOnHold , out DataSet dsTransRxPaid)");
                throw (ex);
            }
            return status;
        }

        public static bool CheckAlreadyPaidForDelivery(string sRXNo, string nRefill, out DataSet DsPOSTransRXDetail)
        {
            bool RetVal = false;
            DsPOSTransRXDetail = null;
            try
            {
                logger.Trace("CheckAlreadyPaidForDelivery() - " + clsPOSDBConstants.Log_Entering);
                DataSet DsPOSTransactionRXDetail = null;
                int RefillNo = 0;
                string sPickupPOS = string.Empty;
                string sPickupRX = string.Empty;
                PharmBL oPharmBL = new PharmBL();
                DataTable oRxInfo = new DataTable();
                logger.Trace("CheckAlreadyPaidForDelivery() - About to call PharmSQL");
                oRxInfo = oPharmBL.GetRxsWithStatus(sRXNo, nRefill, "");
                logger.Trace("CheckAlreadyPaidForDelivery() - Call PharmSQL Successful");
                if (Configuration.isNullOrEmptyDataTable(oRxInfo) == false)
                {
                    RefillNo = Configuration.convertNullToInt(oRxInfo.Rows[0]["nrefill"]);
                    sPickupPOS = Configuration.convertNullToString(oRxInfo.Rows[0]["PickupPOS"]);
                    sPickupRX = Configuration.convertNullToString(oRxInfo.Rows[0]["Pickedup"]);
                    string sSQL = @"select rxno,nrefill,pt.transid,transtype,ptd.Price as paidamount from postransaction pt inner join POSTransactionDetail ptd
                                    on pt.TransID = ptd.transid inner join POSTransactionRXDetail prx 
                                    on prx.TransDetailID = ptd.TransDetailID  ";
                    sSQL += " WHERE RXNO = " + oRxInfo.Rows[0]["Rxno"].ToString() + " AND NREFILL = " + RefillNo + " order by TransDate DESC";
                    SearchSvr oSerSvr = new SearchSvr();

                    DsPOSTransactionRXDetail = oSerSvr.Search(sSQL);
                    string sTransType = string.Empty;
                    if (Configuration.isNullOrEmptyDataSet(DsPOSTransactionRXDetail) == false)
                    {
                        DsPOSTransRXDetail = DsPOSTransactionRXDetail;
                        sTransType = Configuration.convertNullToString(DsPOSTransactionRXDetail.Tables[0].Rows[0]["transtype"]);
                        if (sTransType.Trim().Equals("2") == false)
                        {
                            RetVal = true;
                        }
                    }
                }
                logger.Trace("CheckAlreadyPaidForDelivery() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CheckAlreadyPaidForDelivery()");
                if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
                    throw Ex;
            }
            return RetVal;
        }
        #endregion

        #region PRIMEPOS-2760 18-Nov-2019 JY Added
        public bool RxExistsInPrimeRxDb(out string strMessage)
        {
            bool bStatus = false;
            try
            {
                logger.Trace("RxExistsInPrimeRxDb(out string strMessage) - " + clsPOSDBConstants.Log_Entering);
                strMessage = string.Empty;
                PharmBL oPharmBL = new PharmBL();
                foreach (TransDetailRXRow rxRow in oTransDRXData.TransDetailRX.Rows)
                {
                    bool bTemp = oPharmBL.RxExistsInPrimeRxDb(rxRow.RXNo.ToString(), rxRow.NRefill.ToString(), rxRow.PartialFillNo.ToString());

                    if (!bTemp)
                    {
                        if (strMessage == string.Empty)
                            strMessage = rxRow.RXNo.ToString() + "-" + rxRow.NRefill.ToString();
                        else
                            strMessage += ", " + rxRow.RXNo.ToString() + "-" + rxRow.NRefill.ToString();
                    }
                }

                if (strMessage != "")
                {
                    strMessage = strMessage + Environment.NewLine + "Rx(s) are not present in the PrimeRx database," + Environment.NewLine + "please remove item(s) from this transaction and then proceed.";
                    bStatus = true;
                }
                else
                    bStatus = false;

                logger.Trace("RxExistsInPrimeRxDb(out string strMessage) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                bStatus = false;
                logger.Fatal(exp, "RxExistsInPrimeRxDb(out string strMessage)");
                throw exp;
            }
            return bStatus;
        }
        #endregion
        #region PRIMEPOS-2761
        private bool RxTransRollback(DataSet dsOriginalRxData)
        {
            logger.Trace("RxTransRollback(DataSet dsOriginalRxData) - " + clsPOSDBConstants.Log_Entering);
            PharmBL oPBL = new PharmBL();
            return oPBL.RollbackRxUpdate(dsOriginalRxData);
        }

        private DataSet FetchRxDetails(RXHeaderList ORXInfoList)
        {
            PharmBL oPBL = new PharmBL();
            DataSet dsOrgData = new DataSet();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("RxNo");
            dt.Columns.Add("Nrefill");
            DataRow dr;
            foreach (RXHeader oRXHeader in ORXInfoList)
            {
                foreach (RXDetail objRXInfo in oRXHeader.RXDetails)
                {
                    if (objRXInfo.TblClaims != null && oPBL.ConnectedTo_ePrimeRx())
                    {
                        DataSet dsClaims = new DataSet();
                        dsClaims.Tables.Add(objRXInfo.TblClaims.Copy());
                        ds = dsClaims;
                    }
                    else
                    {
                        ds = oPBL.FetchRxDetails(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), objRXInfo.PartialFillNo.ToString());
                        if (ds != null && ds.Tables.Count > 0)
                            objRXInfo.TblClaims = ds.Tables[0].Copy();
                    }
                    if (dsOrgData.Tables.Count == 0)
                    {
                        dsOrgData = ds;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dsOrgData.Tables[0].Merge(ds.Tables["Claims"]);
                        }
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            dsOrgData.Tables[1].Merge(ds.Tables[1]);
                        }
                        if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                        {
                            dsOrgData.Tables[2].Merge(ds.Tables[2]);
                        }
                    }
                }
            }

            return dsOrgData;
        }

        //public bool UpdatePrimeRXData(DataSet dsRxTransmissionData)

        //{
        //    try
        //    {
        //        string sError = "";
        //        long sigTransID = 0;
        //        PharmBL oPBL = new PharmBL();
        //        DataTable dtRxTransMissionData = new DataTable();
        //        DataSet dsSigTransData = new DataSet();
        //        DataTable dtSigTransData = new DataTable();
        //        if (dsRxTransmissionData.Tables.Count > 0)
        //        {
        //            if (dsRxTransmissionData.Tables[0].Rows.Count > 0)
        //            {
        //                dtRxTransMissionData = dsRxTransmissionData.Tables[0];
        //                if (dtRxTransMissionData.Rows[0]["ConsentTextID"].ToString() != "" &&
        //                    dtRxTransMissionData.Rows[0]["ConsentTextID"].ToString() != null &&
        //                    Convert.ToDateTime(dtRxTransMissionData.Rows[0]["ConsentCaptureDate"]).Year.ToString() != "1753")
        //                {
        //                    oPBL.SavePatientConsent(Convert.ToInt32(dtRxTransMissionData.Rows[0]["PatientID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[0]["ConsentTextID"].ToString()),
        //                         Convert.ToInt32(dtRxTransMissionData.Rows[0]["ConsentTypeID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[0]["ConsentStatusID"].ToString()),
        //                         Convert.ToDateTime(dtRxTransMissionData.Rows[0]["ConsentCaptureDate"].ToString()),
        //                         Convert.ToDateTime(dtRxTransMissionData.Rows[0]["ConsentEffectiveDate"].ToString()), Convert.ToDateTime(dtRxTransMissionData.Rows[0]["ConsentEndDate"].ToString()),
        //                         Convert.ToInt32(dtRxTransMissionData.Rows[0]["RelationID"].ToString()),
        //                         dtRxTransMissionData.Rows[0]["SigneeName"].ToString(),
        //                         //Encoding.ASCII.GetBytes(dtRxTransMissionData.Rows[i]["SignatureData"].ToString()));
        //                         (byte[])dtRxTransMissionData.Rows[0]["SignatureData"]);
        //                }

        //                DataTable dtprivacyAck = dtRxTransMissionData.AsEnumerable().GroupBy(row => row.Field<int>("PatientID")).Select(group => group.First()).CopyToDataTable();

        //                for (int count = 0; count < dtprivacyAck.Rows.Count; count++)
        //                {
        //                    if (!string.IsNullOrWhiteSpace(dtprivacyAck.Rows[count]["PackPATACCEPT"].ToString()) && dtprivacyAck.Rows[count]["PackPATACCEPT"].ToString() != "0")
        //                    {
        //                        logger.Trace("UpdatePrimeRXData() - Initialize Writing into PharmSQL for SavePrivacyAck()");
        //                        if (Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.IsTouchScreen)

        //                        {
        //                            oPBL.SavePrivacyAck(long.Parse(dtprivacyAck.Rows[count]["PatientID"].ToString()), DateTime.Now, dtprivacyAck.Rows[count]["PackPATACCEPT"].ToString(), dtprivacyAck.Rows[count]["PackPrivacyText"].ToString(), dtprivacyAck.Rows[count]["PackPRIVACYSIG"].ToString(), dtprivacyAck.Rows[count]["PackSIGTYPE"].ToString(), (byte[])dtprivacyAck.Rows[count]["PackBinarySign"]);// Encoding.ASCII.GetBytes(dtRxTransMissionData.Rows[i]["PackBinarySign"].ToString()));
        //                        }
        //                        else
        //                        {
        //                            oPBL.SavePrivacyAck(long.Parse(dtprivacyAck.Rows[count]["PatientID"].ToString()), DateTime.Now, dtprivacyAck.Rows[count]["PackPATACCEPT"].ToString(), dtprivacyAck.Rows[count]["PackPrivacyText"].ToString(), dtprivacyAck.Rows[count]["PackPRIVACYSIG"].ToString(), dtprivacyAck.Rows[count]["PackSIGTYPE"].ToString(), (byte[])dtprivacyAck.Rows[count]["PackBinarySign"]);// Encoding.ASCII.GetBytes(dtRxTransMissionData.Rows[i]["PackBinarySign"].ToString()));
        //                        }
        //                        //if (!isRxTxnStatus)
        //                        //{
        //                        //    return isRxTxnStatus;
        //                        //}

        //                        logger.Trace("UpdatePrimeRXData() - Completed Writing into PharmSQL for SavePrivacyAck()");
        //                        try
        //                        {
        //                            oPBL.SavePatientAck(long.Parse(dtprivacyAck.Rows[count]["PatientID"].ToString()), dtprivacyAck.Rows[count]["PackPATACCEPT"].ToString(), DateTime.Now);
        //                        }
        //                        catch (Exception expn)
        //                        {
        //                            isRxTxnStatus = false;
        //                            string message = string.Format("An error Occured while trying to Update Patient SigAck for Patient {0}  {1}", dtprivacyAck.Rows[count]["PatientID"].ToString(), expn.ToString());

        //                            logger.Trace("UpdatePrimeRXData() - " + message);
        //                        }
        //                    }
        //                }
        //                for (int i = 0; i < dtRxTransMissionData.Rows.Count; i++)
        //                {
        //                    oPBL.MarkDelivery(dtRxTransMissionData.Rows[i]["RxNo"].ToString(), dtRxTransMissionData.Rows[i]["Nrefill"].ToString(), "", dtRxTransMissionData.Rows[i]["PickedUp"].ToString(), Convert.ToDateTime(dtRxTransMissionData.Rows[i]["PickUpDate"].ToString()), dtRxTransMissionData.Rows[i]["PickUpPOS"].ToString(), out sError, Convert.ToBoolean(dtRxTransMissionData.Rows[i]["IsDelivery"]));

        //                    oPBL.MarkCopayPaid(dtRxTransMissionData.Rows[i]["RxNo"].ToString(), dtRxTransMissionData.Rows[i]["Nrefill"].ToString(), Convert.ToBoolean(dtRxTransMissionData.Rows[i]["CopayPaid"].ToString()) == true ? 'Y' : 'N');
        //                }


        //                dsSigTransData = oInsSigTransSvr.Populate(Convert.ToInt32(dtRxTransMissionData.Rows[0]["TransID"].ToString()));
        //                if (dsSigTransData.Tables.Count > 0)
        //                {
        //                    dtSigTransData = dsSigTransData.Tables[0];
        //                    if (dtSigTransData.Rows.Count > 0)
        //                    {
        //                        for (int insigCount = 0; insigCount < dtSigTransData.Rows.Count; insigCount++)
        //                        {
        //                            sigTransID = oPBL.SaveInsSigTrans(DateTime.Now, long.Parse(dtSigTransData.Rows[insigCount]["PatientNo"].ToString()), dtSigTransData.Rows[insigCount]["InsType"].ToString(), dtSigTransData.Rows[insigCount]["TransData"].ToString(),
        //                                dtSigTransData.Rows[insigCount]["TransSigData"].ToString(), dtSigTransData.Rows[insigCount]["CounselingReq"].ToString(), dtSigTransData.Rows[insigCount]["SigType"].ToString(), (byte[])dtSigTransData.Rows[insigCount]["BinarySign"]);// Encoding.ASCII.GetBytes(dtSigTransData.Rows[i]["BinarySign"].ToString()));                                    
        //                            if (sigTransID > 0)
        //                            {
        //                                for (int transdetCount = 0; transdetCount < dtRxTransMissionData.Rows.Count; transdetCount++)
        //                                {
        //                                    if (dtSigTransData.Rows[insigCount]["PatientNo"].ToString() == dtRxTransMissionData.Rows[transdetCount]["PatientID"].ToString())
        //                                        oPBL.SaveTransDet(sigTransID, Convert.ToInt64(dtRxTransMissionData.Rows[transdetCount]["RxNo"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[transdetCount]["Nrefill"].ToString()));
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                bool isRXSync = SynchronizeWithPrimeRx(Convert.ToInt32(dtRxTransMissionData.Rows[0]["TransID"].ToString()));
        //                isRxTxnStatus = isRXSync;




        //                //dsSigTransData = oInsSigTransSvr.Populate(Convert.ToInt32(dtRxTransMissionData.Rows[0]["TransID"].ToString()));
        //                //if (dsSigTransData.Tables.Count > 0)
        //                //{
        //                //    dtSigTransData = dsSigTransData.Tables[0];
        //                //    if (dtSigTransData.Rows.Count > 0)
        //                //    {
        //                //        for (int j = 0; j < dtSigTransData.Rows.Count; j++)
        //                //            sigTransID = oPBL.SaveInsSigTrans(DateTime.Now, long.Parse(dtSigTransData.Rows[j]["PatientNo"].ToString()), dtSigTransData.Rows[j]["InsType"].ToString(), dtSigTransData.Rows[j]["TransData"].ToString(),
        //                //                dtSigTransData.Rows[j]["TransSigData"].ToString(), dtSigTransData.Rows[j]["CounselingReq"].ToString(), dtSigTransData.Rows[j]["SigType"].ToString(), (byte[])dtSigTransData.Rows[j]["BinarySign"]);// Encoding.ASCII.GetBytes(dtSigTransData.Rows[j]["BinarySign"].ToString()));
        //                //    }
        //                //}

        //                //bool isRXSync = SynchronizeWithPrimeRx(Convert.ToInt32(dtRxTransMissionData.Rows[0]["TransID"].ToString()));
        //                //isRxTxnStatus = isRXSync;

        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //        isRxTxnStatus = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "UpdatePrimeRXData(DataSet dsRxTransmissionData)");
        //        throw;
        //    }

        //    return isRxTxnStatus;
        //}

        public bool UpdatePrimeRXData(DataSet dsRxTransmissionData, TransDetailRXTable TransDetailRX = null, Boolean IsDelivery = false, List<OnholdRxs> lstOnHoldRxs = null, DataTable dtSelectedRx = null)   //PRIMEPOS-3008 30-Sep-2021 JY Added TransDetailRX, IsDelivery //PRIMEPOS-3192 Added dtSelectedRx
        {
            PharmBL oPBL = new PharmBL();
            if (oPBL.ConnectedTo_ePrimeRx())
            {
                return Update_ePrimeRxData(oPBL, dsRxTransmissionData);
            }

            try
            {
                List<Int64> uniquePatient = new List<Int64>(); //PRIMEPOS-3276
                string sError = "";
                DataTable dtRxTransMissionData = new DataTable();
                bool bUpdateStatus = false; //PRIMEPOS-2916 23-Oct-2020 JY Added

                if (dsRxTransmissionData.Tables.Count > 0)
                {
                    if (dsRxTransmissionData.Tables[0].Rows.Count > 0)
                    {
                        dtRxTransMissionData = dsRxTransmissionData.Tables[0];
                        for (int i = 0; i < dtRxTransMissionData.Rows.Count; i++)
                        {
                            if (dtRxTransMissionData.Rows[i]["ConsentTextID"].ToString() != "" && Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentTextID"]) != 0) //PRIMEPOS-3290
                            {
                                if (!Convert.ToBoolean(dtRxTransMissionData.Rows[i]["IsConsentSkip"])) //PRIMEPOS-2866,PRIMEPOS-2871
                                {
                                    if (Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentSourceID"]) == 1)  //PRIMEPOS-2866,PRIMEPOS-2871
                                    {

                                        if (Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentCaptureDate"].ToString()) != Convert.ToDateTime("01/01/1753 00:00:00"))
                                        {
                                            bool isConsentActive = oPBL.isConsentActiveForPatient(Convert.ToInt32(dtRxTransMissionData.Rows[i]["PatientID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentSourceID"].ToString())); //PRIMEPOS-3276
                                            if (isConsentActive) //PRIMEPOS-3276
                                            {
                                                DateTime endDate = Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentEndDate"].ToString()) == Convert.ToDateTime("01/01/1753 00:00:00") ? DateTime.MinValue : Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentEndDate"].ToString());
                                                oPBL.SavePatientConsent(Convert.ToInt32(dtRxTransMissionData.Rows[i]["PatientID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentTextID"].ToString()),
                                                Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentTypeID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentStatusID"].ToString()),
                                                Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentCaptureDate"].ToString()),
                                                Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentEffectiveDate"].ToString()),
                                                endDate,
                                                Convert.ToInt32(dtRxTransMissionData.Rows[i]["RelationID"].ToString()),
                                                dtRxTransMissionData.Rows[i]["SigneeName"].ToString(),
                                                //Encoding.ASCII.GetBytes(dtRxTransMissionData.Rows[i]["SignatureData"].ToString()));
                                                (byte[])dtRxTransMissionData.Rows[i]["SignatureData"]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DateTime endDate = Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentEndDate"].ToString()) == Convert.ToDateTime("01/01/1753 00:00:00") ? DateTime.MinValue : Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentEndDate"].ToString());
                                        /*
                                        oPBL.SavePatientConsent(Convert.ToInt32(dtRxTransMissionData.Rows[i]["PatientID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentTextID"].ToString()),
                                        Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentTypeID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentStatusID"].ToString()),
                                        Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentCaptureDate"].ToString()),
                                        Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentEffectiveDate"].ToString()),
                                        endDate,
                                        Convert.ToInt32(dtRxTransMissionData.Rows[i]["RelationID"].ToString()),
                                        dtRxTransMissionData.Rows[i]["SigneeName"].ToString(),
                                        //Encoding.ASCII.GetBytes(dtRxTransMissionData.Rows[i]["SignatureData"].ToString()));
                                        (byte[])dtRxTransMissionData.Rows[i]["SignatureData"], int.Parse(dtRxTransMissionData.Rows[i]["ConsentSourceID"].ToString()));
                                        */ //PRIMEPOS-3192 commented for get id to save patientPrescription
                                        #region PRIMEPOS-3192
                                        try
                                        {
                                            int idOfNoConsent = oPBL.GetConsentStatusID(3, "N");
                                            long patientConsentID = oPBL.SaveAndGetIDPatientConsent(Convert.ToInt32(dtRxTransMissionData.Rows[i]["PatientID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentTextID"].ToString()),
                                            Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentTypeID"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentStatusID"].ToString()),
                                            Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentCaptureDate"].ToString()),
                                            Convert.ToDateTime(dtRxTransMissionData.Rows[i]["ConsentEffectiveDate"].ToString()),
                                            endDate,
                                            Convert.ToInt32(dtRxTransMissionData.Rows[i]["RelationID"].ToString()),
                                            dtRxTransMissionData.Rows[i]["SigneeName"].ToString(),
                                            //Encoding.ASCII.GetBytes(dtRxTransMissionData.Rows[i]["SignatureData"].ToString()));
                                            (byte[])dtRxTransMissionData.Rows[i]["SignatureData"], int.Parse(dtRxTransMissionData.Rows[i]["ConsentSourceID"].ToString()));

                                            if (Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentSourceID"]) == 3)  //PRIMEPOS-3287
                                            {
                                                if (dtSelectedRx != null && dtSelectedRx.Rows.Count > 0)
                                                {
                                                    for (int j = 0; j < dtSelectedRx.Rows.Count; j++)
                                                    {
                                                        if (Convert.ToString(dtRxTransMissionData.Rows[i]["PatientID"]).Equals(Convert.ToString(dtSelectedRx.Rows[j]["PatientNo"])))
                                                        {
                                                            if (dtSelectedRx.Rows[j]["IsNewRx"].ToString().ToLower().Trim().Contains("false"))
                                                            {
                                                                oPBL.UpdatePatientConsentSignature(Convert.ToInt64(dtSelectedRx.Rows[j]["PatientConsentID"]), Convert.ToInt64(dtSelectedRx.Rows[j]["PatientNo"]), (byte[])dtRxTransMissionData.Rows[i]["SignatureData"]);
                                                                SynchronizeWithPrescriptionConsent(Convert.ToString(dtSelectedRx.Rows[j]["RxNo"]));
                                                            }
                                                            else if (dtSelectedRx.Rows[j]["IsNewRx"].ToString().ToLower().Trim().Contains("true"))
                                                            {

                                                                string isExist = oPBL.checkPatientPrescriptionRecord(Convert.ToString(dtSelectedRx.Rows[j]["RxNo"]));
                                                                if (isExist != "1")
                                                                {
                                                                    if (dtSelectedRx.Rows[j]["IsChecked"].ToString().ToLower().Trim().Contains("true"))
                                                                    {
                                                                        oPBL.SavePatientPrescriptionConsent(patientConsentID, Convert.ToInt64(dtSelectedRx.Rows[j]["RxNo"]), Convert.ToString(dtSelectedRx.Rows[j]["DrugNDC"]), Convert.ToInt32(dtRxTransMissionData.Rows[i]["ConsentStatusID"]), Convert.ToInt32(dtSelectedRx.Rows[j]["SpecificProductId"]));
                                                                    }
                                                                    else
                                                                    {
                                                                        oPBL.SavePatientPrescriptionConsent(patientConsentID, Convert.ToInt64(dtSelectedRx.Rows[j]["RxNo"]), Convert.ToString(dtSelectedRx.Rows[j]["DrugNDC"]), idOfNoConsent, Convert.ToInt32(dtSelectedRx.Rows[j]["SpecificProductId"]));
                                                                    }
                                                                }
                                                                SynchronizeWithPrescriptionConsent(Convert.ToString(dtSelectedRx.Rows[j]["RxNo"]));

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex, "POSTransaction==>UpdatePrimeRXData(): An Exception Occured while insert/update PatientPrecription and patientConsent");
                                        }
                                        #endregion
                                    }
                                }
                            }

                            //PRIMEPOS-2866,PRIMEPOS-2871
                            if (Convert.ToInt32(dtRxTransMissionData.Rows[i]["RxTransNo"].ToString()) < 0)
                                continue;

                            #region PRIMEPOS-3008 30-Sep-2021 JY Added
                            string sDelivery = null;
                            if (IsDelivery || lstOnHoldRxs != null)
                            {
                                try
                                {
                                    if (TransDetailRX != null && TransDetailRX.Rows.Count > 0)
                                    {
                                        DataRow[] oTransDetailRXRow = TransDetailRX.Select("RXNo = " + dtRxTransMissionData.Rows[i]["RxNo"].ToString() + " AND NRefill = " + dtRxTransMissionData.Rows[i]["Nrefill"].ToString());
                                        if (oTransDetailRXRow.Length > 0)
                                        {
                                            if (Configuration.convertNullToString(oTransDetailRXRow[0]["DELIVERY"]) == "")
                                            {
                                                if (IsDelivery)
                                                    sDelivery = "D";
                                                else if (lstOnHoldRxs != null && lstOnHoldRxs.Count > 0)
                                                {
                                                    foreach (OnholdRxs objOnholdRxs in lstOnHoldRxs)
                                                    {
                                                        if (objOnholdRxs.RxNo.ToString() == dtRxTransMissionData.Rows[i]["RxNo"].ToString() && objOnholdRxs.NRefill.ToString() == dtRxTransMissionData.Rows[i]["Nrefill"].ToString())
                                                        {
                                                            sDelivery = "D";
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception Ex)
                                { }
                            }
                            #endregion

                            oPBL.MarkDelivery(dtRxTransMissionData.Rows[i]["RxNo"].ToString(), dtRxTransMissionData.Rows[i]["Nrefill"].ToString(), sDelivery, dtRxTransMissionData.Rows[i]["PickedUp"].ToString(), Convert.ToDateTime(dtRxTransMissionData.Rows[i]["PickUpDate"].ToString()), dtRxTransMissionData.Rows[i]["PickUpPOS"].ToString(), out sError, Convert.ToBoolean(dtRxTransMissionData.Rows[i]["IsDelivery"]));

                            //oPBL.MarkCopayPaid(dtRxTransMissionData.Rows[i]["RxNo"].ToString(), dtRxTransMissionData.Rows[i]["Nrefill"].ToString(), dtRxTransMissionData.Rows[i]["CopayPaid"].ToString()[i]);

                            oPBL.MarkCopayPaid(dtRxTransMissionData.Rows[i]["RxNo"].ToString(), dtRxTransMissionData.Rows[i]["Nrefill"].ToString(), Convert.ToBoolean(dtRxTransMissionData.Rows[i]["CopayPaid"].ToString()) == true ? 'Y' : 'N');

                            if (dtRxTransMissionData.Rows[i]["PackPATACCEPT"].ToString() != "0")
                            {
                                if (!string.IsNullOrWhiteSpace(dtRxTransMissionData.Rows[i]["PackPATACCEPT"].ToString()))//2943 changes
                                {
                                    if (!uniquePatient.Contains(Convert.ToInt64(dtRxTransMissionData.Rows[i]["PatientID"])))//PRMEPOS-3276
                                    {
                                        logger.Trace("UpdatePrimeRXData() - Initialize Writing into PharmSQL for SavePrivacyAck()");
                                        if ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") || Configuration.CPOSSet.IsTouchScreen)//3002 //PRIMEPOS-3231N

                                        {
                                            oPBL.SavePrivacyAck(long.Parse(dtRxTransMissionData.Rows[i]["PatientID"].ToString()), DateTime.Now, dtRxTransMissionData.Rows[i]["PackPATACCEPT"].ToString(), dtRxTransMissionData.Rows[i]["PackPrivacyText"].ToString(), dtRxTransMissionData.Rows[i]["PackPRIVACYSIG"].ToString(), dtRxTransMissionData.Rows[i]["PackSIGTYPE"].ToString(), dtRxTransMissionData.Rows[i]["PackBinarySign"] != null ? (byte[])dtRxTransMissionData.Rows[i]["PackBinarySign"] : null);// Encoding.ASCII.GetBytes(dtRxTransMissionData.Rows[i]["PackBinarySign"].ToString()));//PRIMEPOS-2866,PRIMEPOS-2871
                                        }
                                        else
                                        {
                                            oPBL.SavePrivacyAck(long.Parse(dtRxTransMissionData.Rows[i]["PatientID"].ToString()), DateTime.Now, dtRxTransMissionData.Rows[i]["PackPATACCEPT"].ToString(), dtRxTransMissionData.Rows[i]["PackPrivacyText"].ToString(), dtRxTransMissionData.Rows[i]["PackPRIVACYSIG"].ToString(), dtRxTransMissionData.Rows[i]["PackSIGTYPE"].ToString(), !string.IsNullOrEmpty(dtRxTransMissionData.Rows[i]["PackBinarySign"].ToString()) ? (byte[])dtRxTransMissionData.Rows[i]["PackBinarySign"] : null);// Encoding.ASCII.GetBytes(dtRxTransMissionData.Rows[i]["PackBinarySign"].ToString()));//PRIMEPOS-2866,PRIMEPOS-2871
                                        }
                                        //if (!isRxTxnStatus)
                                        //{
                                        // return isRxTxnStatus;
                                        //}

                                        logger.Trace("UpdatePrimeRXData() - Completed Writing into PharmSQL for SavePrivacyAck()");

                                        try
                                        {
                                            oPBL.SavePatientAck(long.Parse(dtRxTransMissionData.Rows[i]["PatientID"].ToString()), dtRxTransMissionData.Rows[i]["PackPATACCEPT"].ToString(), DateTime.Now);
                                        }
                                        catch (Exception expn)
                                        {
                                            isRxTxnStatus = false;
                                            string message = string.Format("An error Occured while trying to Update Patient SigAck for Patient {0} {1}", dtRxTransMissionData.Rows[i]["PatientID"].ToString(), expn.ToString());

                                            logger.Trace("UpdatePrimeRXData() - " + message);
                                        }
                                        uniquePatient.Add(Convert.ToInt64(dtRxTransMissionData.Rows[i]["PatientID"])); //PRIMEPOS-3276
                                    }
                                }
                            }

                            #region Commented
                            //dsSigTransData = oInsSigTransSvr.Populate(Convert.ToInt32(dtRxTransMissionData.Rows[i]["TransID"].ToString()));
                            //if (dsSigTransData.Tables.Count > 0)
                            //{
                            //    dtSigTransData = dsSigTransData.Tables[0];
                            //    sigTransID = 0; //PRIMEPOS-2866,PRIMEPOS-2871
                            //    if (dtSigTransData.Rows.Count > 0)
                            //    {
                            //        sigTransID = oPBL.SaveInsSigTrans(DateTime.Now, long.Parse(dtSigTransData.Rows[i]["PatientNo"].ToString()), dtSigTransData.Rows[i]["InsType"].ToString(), dtSigTransData.Rows[i]["TransData"].ToString(),
                            //        dtSigTransData.Rows[i]["TransSigData"].ToString(), dtSigTransData.Rows[i]["CounselingReq"].ToString(), dtSigTransData.Rows[i]["SigType"].ToString(), (byte[])dtSigTransData.Rows[i]["BinarySign"]);// Encoding.ASCII.GetBytes(dtSigTransData.Rows[i]["BinarySign"].ToString()));
                            //    }
                            //}
                            //if (sigTransID > 0)
                            //{
                            //    oPBL.SaveTransDet(sigTransID, Convert.ToInt64(dtRxTransMissionData.Rows[i]["RxNo"].ToString()), Convert.ToInt32(dtRxTransMissionData.Rows[i]["Nrefill"].ToString()));
                            //}
                            #endregion

                            #region PRIMEPOS-2916 23-Oct-2020 JY Added
                            DataSet dsSigTransData = new DataSet();
                            if (bUpdateStatus == false)
                            {
                                bUpdateStatus = true;
                                dsSigTransData = oInsSigTransSvr.Populate(Convert.ToInt32(dtRxTransMissionData.Rows[i]["TransID"].ToString()));
                                if (dsSigTransData != null && dsSigTransData.Tables.Count > 0)
                                {
                                    DataTable dtSigTransData = dsSigTransData.Tables[0];
                                    if (dtSigTransData.Rows.Count > 0)
                                    {
                                        bool isSuccess = true;//PRIMEPOS-3211
                                        //byte[] tBinarySign = null;//PRIMEPOS-3239 //PRIMEPOS-3286 commented
                                        for (int j = 0; j < dtSigTransData.Rows.Count; j++)
                                        {
                                            if ((dtSigTransData.Rows[j]["BinarySign"]).GetType().FullName.Equals("System.DBNull"))
                                            {
                                                continue; //PRIMEPOS-3352
                                                //tBinarySign = (byte[])dtSigTransData.Rows[j]["BinarySign"];
                                            }
                                            //else
                                            //{
                                            //    tBinarySign = null;
                                            //}

                                            //long sigTransID = oPBL.SaveInsSigTrans(DateTime.Now, long.Parse(dtSigTransData.Rows[j]["PatientNo"].ToString()), dtSigTransData.Rows[j]["InsType"].ToString(), dtSigTransData.Rows[j]["TransData"].ToString(),
                                            //dtSigTransData.Rows[j]["TransSigData"].ToString(), dtSigTransData.Rows[j]["CounselingReq"].ToString(), dtSigTransData.Rows[j]["SigType"].ToString(), tBinarySign);

                                            long sigTransID = oPBL.SaveInsSigTrans(DateTime.Now, long.Parse(dtSigTransData.Rows[j]["PatientNo"].ToString()), dtSigTransData.Rows[j]["InsType"].ToString(), dtSigTransData.Rows[j]["TransData"].ToString(),
                                            dtSigTransData.Rows[j]["TransSigData"].ToString(), dtSigTransData.Rows[j]["CounselingReq"].ToString(), dtSigTransData.Rows[j]["SigType"].ToString(), (byte[])dtSigTransData.Rows[j]["BinarySign"]);

                                            if (sigTransID > 0)
                                            {
                                                string strTransData = Configuration.convertNullToString(dtSigTransData.Rows[j]["TransData"]);
                                                if (strTransData != "")
                                                {
                                                    string[] arrRxNo = strTransData.Split(',');
                                                    if (arrRxNo.Length > 0)
                                                    {
                                                        for (int k = 0; k < arrRxNo.Length; k++)
                                                        {
                                                            string[] arrRxNoRefillNo = arrRxNo[k].Split('-');
                                                            if (arrRxNoRefillNo.Length > 0)
                                                            {
                                                                try
                                                                {
                                                                    oPBL.SaveTransDet(sigTransID, Convert.ToInt64(arrRxNoRefillNo[0]), Convert.ToInt32(arrRxNoRefillNo[1]));
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    logger.Error(ex, "UpdatePrimeRXData->SaveTransDet: An Error Occured");
                                                                    isSuccess = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (isSuccess)//PRIMEPOS-3211
                                        {
                                            SynchronizeWithInsSigTrans(Convert.ToInt64(dtSigTransData.Rows[i]["TransID"])); 
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        bool isRXSync = false;
                        try
                        {
                            //PRIMEPOS-2976 15-Jun-2021 JY moved outside the for loop
                            isRXSync = SynchronizeWithPrimeRx(Convert.ToInt32(dtRxTransMissionData.Rows[0]["TransID"].ToString()));
                            // PRIMEPOS-CONSENT SAJID DHUKKA //PRIMEPOS-2866,PRIMEPOS-2871
                            ConsentSynchronizeWithPrimeRx();
                            isRxTxnStatus = isRXSync;
                        }
                        catch (Exception Ex1)
                        {
                            logger.Error(Ex1.Message, "UpdatePrimeRXData(DataSet dsRxTransmissionData) - SynchronizeWithPrimeRx, ConsentSynchronizeWithPrimeRx");
                        }
                    }
                }
                else
                {
                    return false;
                }
                isRxTxnStatus = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "UpdatePrimeRXData(DataSet dsRxTransmissionData)");
                throw;
            }

            return isRxTxnStatus;
        }

        public bool SynchronizeWithPrimeRx(int TransID)
        {
            //logger.Trace("UpdateRXSync(int TransID) - " + clsPOSDBConstants.Log_Entering);
            //System.Data.IDbConnection oConn = null;
            //System.Data.IDbTransaction oTrans = null;
            //oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
            //oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
            //try
            //{
            //    oRxTransDataSvr.UpdateRXSync(TransID, Configuration.UserName, oTrans);
            //    oTrans.Commit();
            //    logger.Trace("UpdateRXSync(int TransID) - " + clsPOSDBConstants.Log_Exiting);
            //}
            //catch (Exception Ex)
            //{
            //    logger.Fatal(Ex.Message, "UpdateRXSync(int TransID)");
            //    throw Ex;
            //}

            #region PRIMEPOS-2976 15-Jun-2021 JY Added
            logger.Trace("SynchronizeWithPrimeRx(int TransID) - " + clsPOSDBConstants.Log_Entering);
            try
            {
                oRxTransDataSvr.UpdateRXSync(TransID);
                logger.Trace("SynchronizeWithPrimeRx(int TransID) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex.Message, "SynchronizeWithPrimeRx(int TransID)");
            }
            #endregion
            return true;
        }

        //PRIMEPOS-3192
        public bool SynchronizeWithPrescriptionConsent(string RxNo)
        {
            logger.Trace("SynchronizeWithPrescriptionConsent() - " + clsPOSDBConstants.Log_Entering);
            try
            {
                oConsentTansmissionLogSvr.UpdatePrescriptionConsentRXSync(RxNo);
                logger.Trace("SynchronizeWithPrescriptionConsent() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex.Message, "SynchronizeWithPrescriptionConsent()");
            }
            return true;
        }

        #region PRIMEPOS-3211
        public bool SynchronizeWithInsSigTrans(long TransID)
        {
            logger.Trace("SynchronizeWithInsSigTrans() - " + clsPOSDBConstants.Log_Entering);
            //string strSQL = string.Empty;
            try
            {
                oInsSigTransSvr.UpdateIsVerified(TransID);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex.Message, "SynchronizeWithInsSigTrans()");
            }
            return true;
        }
        #endregion

        // PRIMEPOS-CONSENT SAJID DHUKKA //PRIMEPOS-2866,PRIMEPOS-2871
        public bool ConsentSynchronizeWithPrimeRx()
        {
            logger.Trace("ConsentSynchronizeWithPrimeRx() - " + clsPOSDBConstants.Log_Entering);
            //System.Data.IDbConnection oConn = null;
            //System.Data.IDbTransaction oTrans = null;
            //oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
            //oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);
            //try
            //{
            //    oConsentTansmissionLogSvr.UpdateConsentRXSync(oTrans);
            //    oTrans.Commit();
            //    logger.Trace("UpdateConsentRXSync(oTrans) - " + clsPOSDBConstants.Log_Exiting);
            //}
            //catch (Exception Ex)
            //{
            //    logger.Fatal(Ex.Message, "ConsentSynchronizeWithPrimeRx()");
            //    throw Ex;
            //}

            #region PRIMEPOS-2976 15-Jun-2021 JY Added
            try
            {
                oConsentTansmissionLogSvr.UpdateConsentRXSync();
                logger.Trace("ConsentSynchronizeWithPrimeRx() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ConsentSynchronizeWithPrimeRx()");
            }
            #endregion

            return true;
        }

        public DataSet RxTransactionPopulate(int TransId)
        {
            return oRxTransDataSvr.Populate(TransId);
        }

        public DataSet ConsentRecoverPopulate() //PRIMEPOS-2866,PRIMEPOS-2871
        {
            return oConsentTansmissionLogSvr.Populate();
        }

        public DataTable RxPrescConsentPopulate() //PRIMEPOS-3192
        {
            return oConsentTansmissionLogSvr.RxPrescConsentPopulate();
        }

        public PccCardInfo getPccRecoveryInfo(DataRow oRow)
        {
            PccCardInfo oPci = new PccCardInfo();
            oPci.PaymentProcessor = oRow["PaymentProcessor"].ToString().Trim();
            //oPci.IsFSATransaction = oRow["IsIIASPayment"].ToString().Trim();
            if (oPci.PaymentProcessor != "WORLDPAY")
            {
                oPci.TransactionID = oRow["HostTransID"].ToString().Trim();// TransactionID
            }
            else
            {
                if (oRow["HostTransID"].ToString() != "")
                {
                    string[] ProcessorTransID = oRow["HostTransID"].ToString().Trim().Split('|');
                    oPci.OrderID = ProcessorTransID[0].ToString();// Order Id
                    oPci.TransactionID = ProcessorTransID[1].ToString(); // TransactionID
                }
                if (oRow["TicketNo"].ToString() != "")
                {
                    oPci.TicketNo = oRow["TicketNo"].ToString().Trim();

                }
            }
            oPci.transAmount = oRow["TransAmount"].ToString().Trim();
            if (oPci.PaymentProcessor == "EVERTEC" || oPci.PaymentProcessor == "VANTIV" || oPci.PaymentProcessor == "PRIMERXPAY" || oPci.PaymentProcessor == "ELAVON")//primepos-2841//2943
            {
                if (oRow["TransType"].ToString().Trim().Contains("SALE"))
                {
                    oPci.Transtype = "Sales";
                }
                else if (oRow["TransType"].ToString().Trim().Contains("RETURN"))
                {
                    oPci.Transtype = "SalesReturn";
                }
            }
            else if (oPci.PaymentProcessor == "HPS")
            {
                if (oRow["TransType"].ToString().Trim().Contains("CreditSale"))
                {
                    oPci.Transtype = "Sales";
                }
                else
                {
                    oPci.Transtype = "SalesReturn";
                }
            }
            else if (oPci.PaymentProcessor == "HPSPAX")
            {

                if (oRow["TransType"].ToString().Trim().Contains("RETURN"))
                {
                    oPci.Transtype = "SalesReturn";
                }
                else
                {
                    oPci.Transtype = "Sales";
                }
            }
            else if (oPci.PaymentProcessor == "XLINK" || oPci.PaymentProcessor == "EDGE_EXPRESS")
            {

                if (oRow["TransType"].ToString().Trim().Contains("Purchase"))
                {
                    oPci.Transtype = "Sales";
                }
                else
                {
                    oPci.Transtype = "Sales";
                }
            }
            else
            {
                oPci.Transtype = oRow["TransType"].ToString().Trim();
            }

            //oPci.Transtype = oRow["TransType"].ToString().Trim();
            // oPci.TerminalRefNumber = oRow["TerminalRefNumber"].ToString().Trim();

            if (oPci.PaymentProcessor == "EVERTEC" || oPci.PaymentProcessor == "HPS" || oPci.PaymentProcessor == "VANTIV" || oPci.PaymentProcessor == "XLINK" || oPci.PaymentProcessor == "ELAVON")//2943
            {
                oPci.tRoutId = oRow["HostTransID"].ToString().Trim();
            }
            else
            {
                oPci.tRoutId = oRow["TerminalRefNumber"].ToString().Trim();
            }

            return oPci;
        }

        public void UpdateCcTransmissionLog(POSTransPaymentData oTransPData, bool IsReversed = false)
        {
            logger.Trace("UpdateCcTransmissionLog(POSTransPaymentData oTransPData, bool IsReversed = false) - " + clsPOSDBConstants.Log_Entering);
            try
            {
                DataSet dsPaidData = new DataSet();
                dsPaidData = GetTransPaymentID(oTransPData.POSTransPayment.Rows[0]["TransID"].ToString());
                if (IsReversed)
                {
                    using (var db = new Possql())
                    {
                        CCTransmission_Log cclog = new CCTransmission_Log();
                        for (int i = 0; i < oTransPData.POSTransPayment.Rows.Count; i++)
                        {
                            string strTicket = oTransPData.POSTransPayment.Rows[i]["TicketNumber"].ToString();
                            cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == strTicket).SingleOrDefault();
                            if (cclog != null)  //PRIMEPOS-2848 21-May-2020 JY Added if condition
                            {
                                cclog.IsReversed = true;
                                //cclog.TransmissionStatus = "Rejected";
                                db.CCTransmission_Logs.Attach(cclog);
                                db.Entry(cclog).Property(p => p.IsReversed).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < dsPaidData.Tables[0].Rows.Count; j++)
                    {
                        using (var db = new Possql())
                        {
                            CCTransmission_Log cclog = new CCTransmission_Log();

                            for (int i = 0; i < oTransPData.POSTransPayment.Rows.Count; i++)
                            {
                                if (dsPaidData.Tables[0].Rows[j]["TicketNumber"].ToString() == oTransPData.POSTransPayment.Rows[i]["TicketNumber"].ToString())
                                {
                                    string strTicket = oTransPData.POSTransPayment.Rows[i]["TicketNumber"].ToString();
                                    if (oTransPData.POSTransPayment.Rows[i]["PaymentProcessor"].ToString() == "HPSPAX" || oTransPData.POSTransPayment.Rows[i]["PaymentProcessor"].ToString() == "VANTIV" || Convert.ToString(oTransPData.POSTransPayment.Rows[i]["PaymentProcessor"]) == "PRIMERXPAY" || Convert.ToString(oTransPData.POSTransPayment.Rows[i]["PaymentProcessor"]) == "NB_VANTIV") //PRIMEPOS-3382 //PRIMEPOS-3407 //PRIMEPOS-3482
                                    {
                                        cclog = db.CCTransmission_Logs.OrderByDescending(r => r.TransNo).Where(w => w.TicketNo == strTicket).Take(1).SingleOrDefault();
                                    }
                                    else
                                    {
                                        cclog = db.CCTransmission_Logs.Where(w => w.TicketNo == strTicket).SingleOrDefault();
                                    }
                                    if (cclog != null)
                                    {
                                        cclog.POSTransID = oTransPData.POSTransPayment.Rows[i]["TransID"].ToString();
                                        cclog.AmtApproved = Convert.ToDecimal(oTransPData.POSTransPayment.Rows[i]["Amount"].ToString());
                                        cclog.POSPaymentID = dsPaidData.Tables[0].Rows[j]["TransPayID"].ToString();
                                        //cclog.TransmissionStatus = "Completed";
                                        db.CCTransmission_Logs.Attach(cclog);
                                        db.Entry(cclog).Property(p => p.POSTransID).IsModified = true;
                                        db.Entry(cclog).Property(p => p.AmtApproved).IsModified = true;
                                        db.Entry(cclog).Property(p => p.POSPaymentID).IsModified = true;

                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "UpdateCcTransmissionLog(POSTransPaymentData oTransPData, bool IsReversed = false)");
                throw;
            }
            logger.Trace("UpdateCcTransmissionLog(POSTransPaymentData oTransPData, bool IsReversed = false) - " + clsPOSDBConstants.Log_Exiting);
        }

        public DataSet GetTransPaymentID(String TransId)
        {
            System.Data.IDbConnection oConn = null;
            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            POSTransPaymentSvr oPaymentSvr = new POSTransPaymentSvr();

            return oPaymentSvr.GetTransPaymentID(TransId, oConn);
        }

        public DataSet getCcTransmissionLog(DateTime dtFrom, DateTime dtTo, string status = "", string stationID = "", string user = "")
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                TransDetailSvr trDetSvr = new TransDetailSvr();
                dsCcdata = trDetSvr.GetCcTransmissionLog(dtFrom, dtTo, status, stationID, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }

        public DataSet getStatus()
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                TransDetailSvr trDetSvr = new TransDetailSvr();
                dsCcdata = trDetSvr.GetStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }

        public DataSet getStation()
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                TransDetailSvr trDetSvr = new TransDetailSvr();
                dsCcdata = trDetSvr.GetStation();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }

        public DataSet getUser()
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                TransDetailSvr trDetSvr = new TransDetailSvr();
                dsCcdata = trDetSvr.GetUser();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }

        public bool UpdateRxTransDataLocally(RXHeaderList ORXInfoList, bool isReturn, bool markAsDelivered, IDbTransaction oTrans, int TransId, bool isBatchDelivery, DataTable dtSelectedRx) //PRIMEPOS-3192 Added dtSelectedRx 
        {
            logger.Trace("UpdateRxTransDataLocally(RXHeaderList ORXInfoList, bool isReturn, bool markAsDelivered, IDbTransaction oTrans, int TransId, bool isBatchDelivery) - " + clsPOSDBConstants.Log_Entering);
            string Pickedup = "";
            bool isPOSRxTransUpdate = false;
            DataTable dtData = new DataTable();
            PharmBL oPBL = new PharmBL();

            try
            {
                string isDelivered = null;
                if (markAsDelivered == true)
                {
                    isDelivered = "D";
                }

                foreach (RXHeader oRXHeader in ORXInfoList)
                {
                    string rxData = "";
                    #region oRxTransactionData block
                    try
                    {
                        foreach (RXDetail objRXInfo in oRXHeader.RXDetails)
                        {
                            oRxTransactionDataRow = (RxTransactionDataRow)oRxTransactionData.RxTransaction.NewRxTransactionDataRow();
                            if (objRXInfo.TblClaims != null && oPBL.ConnectedTo_ePrimeRx())
                                dtData = objRXInfo.TblClaims;
                            else
                                dtData = oPBL.GetRxsWithStatus(objRXInfo.RXNo.ToString(), objRXInfo.RefillNo.ToString(), "");
                            IsPOSAllowRX = ((Configuration.AllowRxPicked > 0) && dtData.Rows[0]["Pickedup"].ToString() != "Y") ? true : false;  //PRIMEPOS-2865 16-Jul-2020 JY modified

                            oRxTransactionDataRow.StationID = Configuration.StationID;
                            oRxTransactionDataRow.UserID = Configuration.UserName;
                            oRxTransactionDataRow.RxNo = objRXInfo.RXNo;
                            oRxTransactionDataRow.Nrefill = Convert.ToInt32(objRXInfo.RefillNo);
                            oRxTransactionDataRow.PartialFillNo = Convert.ToInt32(objRXInfo.PartialFillNo);
                            oRxTransactionDataRow.PatientID = Convert.ToInt32(oRXHeader.PatientNo);
                            oRxTransactionDataRow.TransDate = DateTime.Now;
                            oRxTransactionDataRow.DeliveryStatus = isDelivered;
                            if (isReturn == true)
                            {
                                if (!oRXHeader.IsIntakeBatch || (oRXHeader.IsIntakeBatch && Configuration.CInfo.IntakeBatchMarkAsPickedup))
                                {
                                    if (!IsPOSAllowRX)
                                    {
                                        oRxTransactionDataRow.PickedUp = "N";
                                        //oRxTransactionDataRow.PickUpDate = DateTime.MinValue; //PRIMEPOS-2914 22-Oct-2020 JY Commented
                                        oRxTransactionDataRow.PickUpPOS = "N";
                                    }
                                    else
                                    {
                                        oRxTransactionDataRow.PickedUp = dtData.Rows[0]["Pickedup"].ToString();
                                        //oRxTransactionDataRow.PickUpDate = DateTime.MinValue; //PRIMEPOS-2914 22-Oct-2020 JY Commented
                                        oRxTransactionDataRow.PickUpPOS = "N";
                                    }
                                }
                                oRxTransactionDataRow.CopayPaid = false;
                            }
                            else
                            {
                                if (!oRXHeader.IsIntakeBatch || (oRXHeader.IsIntakeBatch && Configuration.CInfo.IntakeBatchMarkAsPickedup))
                                {
                                    if ((Configuration.AllowRxPicked > 0) && IsPOSAllowRX)  //PRIMEPOS-2865 16-Jul-2020 JY modified
                                    {
                                        if (markAsDelivered == true && Configuration.AllowRxPicked == 2)    //PRIMEPOS-2865 16-Jul-2020 JY Added if part
                                            oRxTransactionDataRow.PickedUp = "N";
                                        else
                                            oRxTransactionDataRow.PickedUp = "Y";

                                        oRxTransactionDataRow.PickUpDate = DateTime.Now;
                                        oRxTransactionDataRow.PickUpPOS = "Y";
                                    }
                                    else if ((Configuration.AllowRxPicked > 0) && !IsPOSAllowRX)    //PRIMEPOS-2865 16-Jul-2020 JY modified
                                    {
                                        Pickedup = (string.IsNullOrWhiteSpace(dtData.Rows[0]["Pickedup"].ToString()) ? "Y" : dtData.Rows[0]["Pickedup"].ToString());
                                        DateTime pickupdate;
                                        if (Pickedup.Trim().ToUpper().Equals("Y"))
                                        {
                                            try
                                            {
                                                if (dtData.Rows[0]["PICKUPDATE"] != DBNull.Value && (Convert.ToDateTime(dtData.Rows[0]["PICKUPDATE"].ToString()) >= Convert.ToDateTime(dtData.Rows[0]["DATEF"].ToString())))
                                                {
                                                    pickupdate = Convert.ToDateTime(dtData.Rows[0]["PICKUPDATE"].ToString());
                                                }
                                                else
                                                {
                                                    pickupdate = DateTime.Now;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                pickupdate = DateTime.Now;
                                            }
                                            if (markAsDelivered == true && Configuration.AllowRxPicked == 2 && dtData.Rows[0]["Pickedup"].ToString().ToUpper() != "Y")    //PRIMEPOS-2865 16-Jul-2020 JY Added if part
                                                oRxTransactionDataRow.PickedUp = "N";
                                            else
                                                oRxTransactionDataRow.PickedUp = Pickedup;

                                            oRxTransactionDataRow.PickUpDate = pickupdate;
                                            oRxTransactionDataRow.PickUpPOS = "N";
                                        }
                                        else
                                        {
                                            oRxTransactionDataRow.PickedUp = Pickedup;
                                            //oRxTransactionDataRow.PickUpDate = DateTime.MinValue; //PRIMEPOS-2914 22-Oct-2020 JY Commented
                                            oRxTransactionDataRow.PickUpPOS = "N";
                                        }
                                    }
                                    else
                                    {
                                        Pickedup = dtData.Rows[0]["Pickedup"].ToString();
                                        if (markAsDelivered == true && Configuration.AllowRxPicked == 2 && Pickedup.ToUpper() != "Y")    //PRIMEPOS-2865 16-Jul-2020 JY Added if part
                                            oRxTransactionDataRow.PickedUp = "N";
                                        else
                                            oRxTransactionDataRow.PickedUp = Pickedup;

                                        //oRxTransactionDataRow.PickUpDate = DateTime.MinValue; //PRIMEPOS-2914 22-Oct-2020 JY Commented
                                        oRxTransactionDataRow.PickUpPOS = "Y";
                                    }
                                }
                                oRxTransactionDataRow.CopayPaid = true;
                            }

                            oRxTransactionDataRow.IsDelivery = isBatchDelivery;// isBatchDelivery;

                            //PRIMEPOS-2866,PRIMEPOS-2871 Consent
                            try
                            {
                                PatientConsent healthixConsent = new PatientConsent();
                                if (oRXHeader.PatConsent != null)
                                {
                                    healthixConsent = oRXHeader.PatConsent.Where(a => a.ConsentSourceName.ToUpper() == MMS.Device.Global.Constants.CONSENT_SOURCE_HEALTHIX.ToUpper()).FirstOrDefault();
                                    if (isReturn == false && healthixConsent != null && !healthixConsent.IsConsentSkip)
                                    {
                                        oRxTransactionDataRow.ConsentTextID = healthixConsent.ConsentTextID;
                                        oRxTransactionDataRow.ConsentTypeID = healthixConsent.ConsentTypeID;
                                        oRxTransactionDataRow.ConsentStatusID = healthixConsent.ConsentStatusID;
                                        oRxTransactionDataRow.ConsentCaptureDate = healthixConsent.ConsentCaptureDate;
                                        oRxTransactionDataRow.ConsentEffectiveDate = healthixConsent.ConsentEffectiveDate;
                                        oRxTransactionDataRow.ConsentEndDate = healthixConsent.ConsentEndDate;
                                        oRxTransactionDataRow.PatConsentRelationID = healthixConsent.PatConsentRelationID;
                                        oRxTransactionDataRow.SigneeName = healthixConsent.SigneeName;
                                        oRxTransactionDataRow.SignatureData = healthixConsent.SignatureData;
                                    }
                                    #region PRIMEPOS-2866
                                    if (healthixConsent != null)
                                    {
                                        oRxTransactionDataRow.IsConsentSkip = healthixConsent.IsConsentSkip; // PRIMEPOS-2866
                                    }
                                    #endregion
                                }
                            }
                            catch (Exception Ex)
                            {
                                logger.Fatal(Ex, "UpdateRxTransDataLocally(RXHeaderList ORXInfoList, bool isReturn, bool markAsDelivered, IDbTransaction oTrans, int TransId, bool isBatchDelivery) - PatientConsent healthixConsent block");
                            }

                            if (rxData == "")
                            {
                                rxData = objRXInfo.RXNo.ToString() + "-" + objRXInfo.RefillNo;
                                if (objRXInfo.PartialFillNo > 0)
                                    rxData += "-" + objRXInfo.PartialFillNo;
                            }
                            else
                            {
                                rxData += "," + objRXInfo.RXNo.ToString() + "-" + objRXInfo.RefillNo;
                                if (objRXInfo.PartialFillNo > 0)
                                    rxData += "-" + objRXInfo.PartialFillNo;
                            }

                            if (oRXHeader.NOPPStatus != null)
                            {
                                if ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") || Configuration.CPOSSet.IsTouchScreen)//3002 //PRIMEPOS-3231N
                                {
                                    oRxTransactionDataRow.PackDATESIGNED = DateTime.Now;
                                    oRxTransactionDataRow.PackPATACCEPT = oRXHeader.NOPPStatus;
                                    oRxTransactionDataRow.PackPRIVACYTEXT = oRXHeader.PrivacyText;
                                    oRxTransactionDataRow.PackPRIVACYSIG = oRXHeader.NOPPSignature;
                                    oRxTransactionDataRow.PackSIGTYPE = "M";
                                    oRxTransactionDataRow.PackBinarySign = oRXHeader.NoppBinarySign;
                                }
                                else
                                {
                                    oRxTransactionDataRow.PackDATESIGNED = DateTime.Now;
                                    oRxTransactionDataRow.PackPATACCEPT = oRXHeader.NOPPStatus;
                                    oRxTransactionDataRow.PackPRIVACYTEXT = oRXHeader.PrivacyText;
                                    oRxTransactionDataRow.PackPRIVACYSIG = oRXHeader.NOPPSignature;
                                    oRxTransactionDataRow.PackSIGTYPE = SigPadUtil.DefaultInstance.SigType;
                                    oRxTransactionDataRow.PackBinarySign = oRXHeader.NoppBinarySign;
                                }
                            }
                            oRxTransactionData.RxTransaction.AddRow(oRxTransactionDataRow);
                            oRxTransDataSvr.Persist(oRxTransactionData, oTrans, TransId);
                            oRxTransactionData.Clear();
                        }
                    }
                    catch (Exception Ex)
                    {
                        logger.Fatal(Ex, "UpdateRxTransDataLocally(RXHeaderList ORXInfoList, bool isReturn, bool markAsDelivered, IDbTransaction oTrans, int TransId, bool isBatchDelivery) - oRxTransactionData block");
                    }
                    #endregion oRxTransactionData

                    #region ConsentTransmissionData
                    // PRIMEPOS-CONSENT SAJID DHUKKA // //PRIMEPOS-2866,PRIMEPOS-2871 // Need to check and save as a locally
                    try
                    {
                        ConsentTransmissionData oConsentTransmissionData = new ConsentTransmissionData();
                        if (oRXHeader.PatConsent != null)
                        {
                            foreach (PatientConsent patConsent in oRXHeader.PatConsent)
                            {
                                if (!patConsent.IsConsentSkip)
                                {
                                    ConsentTransmissionDataRow oConsentTransmissionDataRow = null;
                                    oConsentTransmissionDataRow = (ConsentTransmissionDataRow)oConsentTransmissionData.ConsentTransmission.NewConsentTransmissionDataRow();
                                    oConsentTransmissionDataRow.ConsentSourceID = patConsent.ConsentSourceID;
                                    oConsentTransmissionDataRow.ConsentTypeId = patConsent.ConsentTypeID;
                                    oConsentTransmissionDataRow.ConsentTextId = patConsent.ConsentTextID;
                                    oConsentTransmissionDataRow.ConsentStatusId = patConsent.ConsentStatusID;
                                    oConsentTransmissionDataRow.ConsentRelationId = patConsent.PatConsentRelationID;
                                    oRxTransactionDataRow.ConsentEffectiveDate = patConsent.ConsentEffectiveDate;//PRIMEPOS-ARVIND CONSENT
                                    int patientNo = 0;
                                    int.TryParse(oRXHeader.PatientNo, out patientNo);
                                    oConsentTransmissionDataRow.PatientNo = patientNo;
                                    oConsentTransmissionDataRow.ConsentCaptureDate = patConsent.ConsentCaptureDate;
                                    oConsentTransmissionDataRow.ConsentExpiryDate = patConsent.ConsentEndDate;
                                    oConsentTransmissionDataRow.SigneeName = patConsent.SigneeName;
                                    oConsentTransmissionDataRow.SignatureData = patConsent.SignatureData;
                                    oConsentTransmissionDataRow.RxNo = oRXHeader.RXDetails[0].RXNo;
                                    oConsentTransmissionDataRow.Nrefill = oRXHeader.RXDetails[0].RefillNo;
                                    oConsentTransmissionData.ConsentTransmission.AddRow(oConsentTransmissionDataRow);
                                    oConsentTansmissionLogSvr.Persist(oConsentTransmissionData, oTrans, 0);
                                    oConsentTransmissionData.ConsentTransmission.Rows.Clear();
                                }
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        logger.Fatal(Ex, "UpdateRxTransDataLocally(RXHeaderList ORXInfoList, bool isReturn, bool markAsDelivered, IDbTransaction oTrans, int TransId, bool isBatchDelivery) - ConsentTransmissionData block");
                    }
                    #endregion

                    if (dtSelectedRx != null && dtSelectedRx.Rows.Count > 0)//PRIMEPOS-3192
                    {
                        oConsentTansmissionLogSvr.PersistPrescription(dtSelectedRx, oTrans);
                        dtSelectedRx.Clear();
                    }

                    #region InSigTrans
                    try
                    {
                        //long sigTransID = 0;
                        if (oRXHeader.RXSignature.Trim() != "" || oRXHeader.bBinarySign != null) //PRIMEPOS-3286 uncommented this
                        {
                            if (this.CurrentTransactionType == POSTransactionType.Sales)    //PRIMEPOS-3177 27-Dec-2022 JY Added
                            {
                                using (InsSigTransSvr oInsSigTransSvr = new InsSigTransSvr())
                                {
                                    InsSigTransData oInsSigTransData = new InsSigTransData();
                                    //if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected()) //PRIMEPOS-3217 //PRIMEPOS-3290 commented
                                    //{
                                        if ((Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "Windows Tablet".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_ISMP_WITHTOUCHSCREEN".Trim().ToUpper() || Configuration.CPOSSet.PinPadModel.Trim().ToUpper() == "VANTIV_LINK_2500") || Configuration.CPOSSet.IsTouchScreen)//3002 //PRIMEPOS-3231N
                                        {
                                            bool bPrivacyAck = false;
                                            if (oRXHeader.NOPPStatus != null)
                                            {
                                                bPrivacyAck = true;
                                                oInsSigTransData.InsSigTrans.AddRow(0, TransId, int.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, Configuration.convertNullToString(oRXHeader.RXSignature), oRXHeader.CounselingRequest, "M", oRXHeader.bBinarySign,
                                                    oRXHeader.NOPPStatus, oRXHeader.PrivacyText, oRXHeader.NOPPSignature, "M", (oRXHeader.NoppBinarySign != null ? oRXHeader.NoppBinarySign : null));//PRIMEPOS-3286
                                            }
                                            else
                                            {
                                                oInsSigTransData.InsSigTrans.AddRow(0, TransId, int.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, Configuration.convertNullToString(oRXHeader.RXSignature), oRXHeader.CounselingRequest, "M", oRXHeader.bBinarySign);//PRIMEPOS-3286
                                            }
                                            oInsSigTransSvr.Persist(oInsSigTransData, bPrivacyAck, oTrans);
                                        }
                                        else
                                        {
                                            if (Configuration.CPOSSet.UseSigPad == true && SigPadUtil.DefaultInstance.isConnected()) //PRIMEPOS-3290
                                            {
                                                bool bPrivacyAck = false;
                                                if (oRXHeader.NOPPStatus != null)
                                                {
                                                    bPrivacyAck = true;
                                                    oInsSigTransData.InsSigTrans.AddRow(0, TransId, int.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, Configuration.convertNullToString(oRXHeader.RXSignature), oRXHeader.CounselingRequest, SigPadUtil.DefaultInstance.SigType, oRXHeader.bBinarySign,
                                                                                oRXHeader.NOPPStatus, oRXHeader.PrivacyText, oRXHeader.NOPPSignature, SigPadUtil.DefaultInstance.SigType, (oRXHeader.NoppBinarySign != null ? oRXHeader.NoppBinarySign : null));//PRIMEPOS-3286
                                                }
                                                else
                                                {
                                                    oInsSigTransData.InsSigTrans.AddRow(0, TransId, int.Parse(oRXHeader.PatientNo), oRXHeader.InsType, rxData, Configuration.convertNullToString(oRXHeader.RXSignature), oRXHeader.CounselingRequest, SigPadUtil.DefaultInstance.SigType, oRXHeader.bBinarySign); //PRIMEPOS-3286
                                                }
                                                oInsSigTransSvr.Persist(oInsSigTransData, bPrivacyAck, oTrans);
                                            }
                                        }
                                    //}//PRIMEPOS-3217
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isPOSRxTransUpdate = false;
                        logger.Fatal(ex, "UpdateRxTransDataLocally(RXHeaderList ORXInfoList, bool isReturn, bool markAsDelivered, IDbTransaction oTrans, int TransId, bool isBatchDelivery) - InsSigTrans block");
                        throw ex;
                    }
                    #endregion
                }
                isPOSRxTransUpdate = true;
                logger.Trace("UpdateRxTransDataLocally(RXHeaderList ORXInfoList, bool isReturn, bool markAsDelivered, IDbTransaction oTrans, int TransId, bool isBatchDelivery) - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception ex)
            {
                isPOSRxTransUpdate = false;
                throw ex;
            }
            return isPOSRxTransUpdate;
        }

        public DataTable GetRxTransmission(DateTime dtFrom, DateTime dtTo)
        {
            return oRxTransDataSvr.GetRxTransDetail(dtFrom, dtTo);
        }

        public PccCardInfo getPccInfo(DataRow oRow)
        {
            logger.Trace("getPccInfo(DataRow oRow) - " + clsPOSDBConstants.Log_Entering);
            PccCardInfo oPci = new PccCardInfo();
            oPci.PaymentProcessor = oRow["PaymentProcessor"].ToString().Trim();
            oPci.IsFSATransaction = oRow["IsIIASPayment"].ToString().Trim();
            if (oPci.PaymentProcessor != "WORLDPAY")
            {
                oPci.tRoutId = oRow["ProcessorTransID"].ToString().Trim();
                oPci.TransactionID = oPci.tRoutId;
            }
            else //if (oPci.PaymentProcessor == "WORLDPAY")
            {
                string[] ProcessorTransID = oRow["ProcessorTransID"].ToString().Trim().Split(':');
                oPci.OrderID = ProcessorTransID[1].ToString();// Order Id
                oPci.TransactionID = ProcessorTransID[0].ToString(); // TransactionID
            }
            oPci.transAmount = oRow["Amount"].ToString().Trim();
            logger.Trace("getPccInfo(DataRow oRow) - " + clsPOSDBConstants.Log_Exiting);
            return oPci;
        }

        /// <summary>
        /// Reverse Transaction for all payment processor when server error is coming
        /// </summary>
        public void reverseCCTransaction()
        {
            try
            {
                logger.Trace("reverseCCTransaction() - " + clsPOSDBConstants.Log_Entering);
                for (int i = 0; i < oPOSTransPaymentData.Tables[0].Rows.Count; i++)
                {
                    string strPaymentType = oPOSTransPaymentData.Tables[0].Rows[i]["TransTypeCode"].ToString().Trim();

                    switch (strPaymentType)
                    {
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "-99":
                        case "E":
                            PccCardInfo pccCardInfo = getPccInfo(oPOSTransPaymentData.Tables[0].Rows[i]);

                            //if (oPOSTransPaymentData.Tables[0].Rows[i]["PaymentProcessor"].ToString().Trim() == "HPSPAX" || oPOSTransPaymentData.Tables[0].Rows[i]["PaymentProcessor"].ToString().Trim() == "HPS")
                            if (pccCardInfo.PaymentProcessor != "WORLDPAY")
                            {
                                string ticketNum = Configuration.StationID + clsCoreUIHelper.GetRandomNo().ToString();
                                if (this.CurrentTransactionType == POSTransactionType.Sales)
                                {
                                    logger.Trace("Invoking PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSales(ticketNum, ref pccCardInfo) - " + clsPOSDBConstants.Log_Entering);
                                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSales(ticketNum, ref pccCardInfo);
                                    logger.Trace("Invoking PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSales(ticketNum, ref pccCardInfo) - " + clsPOSDBConstants.Log_Exiting);
                                }
                                else if (this.CurrentTransactionType == POSTransactionType.SalesReturn)
                                {
                                    logger.Trace("Invoking PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSalesReturn(ticketNum, ref pccCardInfo) - " + clsPOSDBConstants.Log_Entering);
                                    PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSalesReturn(ticketNum, ref pccCardInfo);
                                    logger.Trace("Invoking PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnCreditCardSalesReturn(ticketNum, ref pccCardInfo) - " + clsPOSDBConstants.Log_Exiting);
                                }
                            }
                            else // if (pccCardInfo.PaymentProcessor == "WORLDPAY")
                            {
                                logger.Trace("Invoking PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnWP(FirstMile.TransactionType.Void, ref pccCardInfo) - " + clsPOSDBConstants.Log_Entering);
                                PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnWP(FirstMile.TransactionType.Void, ref pccCardInfo);
                                logger.Trace("Invoking Ended PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).PerformVoidOnWP(FirstMile.TransactionType.Void, ref pccCardInfo) - " + clsPOSDBConstants.Log_Exiting);
                            }

                            String responseStatus = PccPaymentSvr.GetProcessorInstance(pccCardInfo.PaymentProcessor).ResponseStatus;
                            if (responseStatus.ToUpper().Trim() == "SUCCESS")
                            {
                                logger.Trace("Forced void for Transactionid " + pccCardInfo.TransactionID + " successful");
                            }
                            else
                            {
                                logger.Trace("Forced void for Transactionid " + pccCardInfo.TransactionID + " unsuccessful");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "reverseCCTransaction()");
            }
            logger.Trace("reverseCCTransaction() - " + clsPOSDBConstants.Log_Exiting);
        }
        #endregion
        #region PRIMEPOS-2841
        public DataSet GetPharmacyNPI()
        {
            System.Data.IDbConnection oConn = null;
            oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            POSTransPaymentSvr oPaymentSvr = new POSTransPaymentSvr();

            return oPaymentSvr.GetPharmacyNPI(oConn);
        }
        #endregion
        #region Customer Engagement Details PRIMEPOS-2794 SAJID

        //Customer Engagement Details PRIMEPOS-2794 SAJID DHUKKA
        public int CalculateStarRating(int patientNo)
        {
            DataSet ds = new DataSet();
            int iStarRating = 0;
            if (patientNo > 0)
            {
                try
                {
                    PharmBL oPBL = new PharmBL();
                    ds = oPBL.GetPatMedAdherence(patientNo);
                    double MPRValue = 0;
                    double PDCValue = 0;
                    int counter = 0;
                    double AvgMPRValue = 0, AvgPDCValue = 0;
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow oDr in ds.Tables[0].Rows)
                        {
                            if (MMSUtil.UtilFunc.ValorZeroD(oDr["PDCValue"].ToString()) > 1)
                            {
                                MPRValue += MMSUtil.UtilFunc.ValorZeroD(oDr["MPRValue"].ToString());
                                PDCValue += MMSUtil.UtilFunc.ValorZeroD(oDr["PDCValue"].ToString());
                                counter += 1;
                            }
                        }
                        if (MPRValue > 0 && counter > 0)
                            AvgMPRValue = MPRValue / counter;

                        if (PDCValue > 0 && counter > 0)
                            AvgPDCValue = PDCValue / counter;

                        if (AvgMPRValue >= 80)
                        {
                            iStarRating = 5;
                        }
                        else if (AvgMPRValue >= 70)
                        {
                            iStarRating = 4;
                        }
                        else if (AvgMPRValue >= 60)
                        {
                            iStarRating = 3;
                        }
                        else if (AvgMPRValue >= 50)
                        {
                            iStarRating = 2;
                        }
                        else if (AvgMPRValue >= 30)
                        {
                            iStarRating = 1;
                        }
                        else if (AvgMPRValue < 30)
                        {
                            iStarRating = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "CalculateStarRating(int patientNo)");
                    if (Configuration.CPOSSet.UsePrimeRX)   //PRIMEPOS-3106 13-Jul-2022 JY Added if condition
                        throw (ex);
                }
            }
            return iStarRating;
        }

        public string GetLastTransactionByCustId(int customerId)
        {
            TransHeaderSvr oTHsvr = new TransHeaderSvr();
            return oTHsvr.PopulateROALastTransaction(customerId);
        }
        #endregion

        #region PRIMEPOS-2886 24-Aug-2020 JY Added
        public CustomerData GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)
        {
            CustomerData oData = null;
            try
            {
                Customer oCustomer = new Customer();
                oData = oCustomer.GetCustomerByMultiplePatientsPatameters(dsPatient, ref LevelId);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)");
                throw exp;
            }
            return oData;
        }
        #endregion

        //PRIMEPOS-2927
        public DataTable AddShippingItem(string TransID)
        {
            using (var oSvr = new TransDetailRXSvr())
            {
                DataTable dtPOSTransactionRXDetail = null;
                dtPOSTransactionRXDetail = oSvr.GetShippingRecord(TransID);

                return dtPOSTransactionRXDetail;
            }
        }

        public DataSet GetTaxCodeData()
        {
            try
            {
                System.Data.IDbConnection oConn = null;
                oConn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);

                string sSQL = sSQL = String.Concat("SELECT  ", clsPOSDBConstants.TaxCodes_Fld_Amount, ","
                    , clsPOSDBConstants.TaxCodes_Fld_TaxID, ","
                    , clsPOSDBConstants.TaxCodes_Fld_TaxType, ","
                    , clsPOSDBConstants.TaxCodes_Fld_Description, ","
                    , clsPOSDBConstants.TaxCodes_Fld_TaxCode
                    , " FROM "
                    , clsPOSDBConstants.TaxCodes_tbl);

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(oConn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetTaxCodeData()");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetTaxCodeData()");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTaxCodeData()");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public bool Update_ePrimeRxData(PharmBL oPBL, DataSet dsRxTransmissionData)
        {
            if (dsRxTransmissionData == null || dsRxTransmissionData.Tables.Count == 0 || dsRxTransmissionData.Tables[0].Rows.Count == 0)
                return isRxTxnStatus;
            try
            {
                DataTable dtSigTransData = null;
                DataTable dtRxData = null;
                DataTable dtRxTransMissionData = dsRxTransmissionData.Tables[0];
                int iTransID = Configuration.convertNullToInt(dtRxTransMissionData.Rows[0]["TransID"].ToString());
                DataSet dsSigTransData = oInsSigTransSvr.Populate(iTransID);

                if (dsSigTransData != null && dsSigTransData.Tables.Count > 0)
                    dtSigTransData = dsSigTransData.Tables[0];

                if (dsRxTransmissionData.Tables.Count > 1)
                    dtRxData = dsRxTransmissionData.Tables[1];
                bool isRXSync = false;
                isRXSync = oPBL.SaveDetails_ePrimeRx(dtRxTransMissionData, dtSigTransData, dtRxData);
                if (isRXSync)
                {
                    try
                    {
                        isRXSync = SynchronizeWithPrimeRx(iTransID);
                        ConsentSynchronizeWithPrimeRx();
                    }
                    catch (Exception exp)
                    {
                        logger.Error(exp.Message, "Update_ePrimeRxData() - calling SynchronizeWithPrimeRx, ConsentSynchronizeWithPrimeRx");
                    }
                }
                isRxTxnStatus = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, "Update_ePrimeRxData()");
                isRxTxnStatus = false;
                throw;
            }
            return isRxTxnStatus;
        }

        #region PRIMEPOS-3044 26-Jan-2021 JY Added
        public bool CheckRxAlreadyPickedup(POSTransactionType TransType, TransDetailRXTable oTransDetailRX, ref ArrayList alRxNos)
        {
            bool bReturn = false;
            try
            {
                TransDetailRXSvr oTransDetailRXSvr = new TransDetailRXSvr();
                DataTable dtPOSTransactionRXDetail = null;
                foreach (TransDetailRXRow rxRow in oTransDetailRX)
                {
                    dtPOSTransactionRXDetail = oTransDetailRXSvr.GetPOSTransactionRXDetailRecord(rxRow.RXNo, rxRow.NRefill, rxRow.PartialFillNo);
                    if (dtPOSTransactionRXDetail != null && dtPOSTransactionRXDetail.Rows.Count > 0)
                    {
                        if (Configuration.convertNullToInt(dtPOSTransactionRXDetail.Rows[0]["TransType"]) == 1) //last transaction was sale
                        {
                            if (TransType == POSTransactionType.Sales)
                            {
                                alRxNos.Add(rxRow.RXNo + "-" + rxRow.NRefill);
                                bReturn = true;
                            }
                        }
                        else
                        {
                            if (CurrentTransactionType == POSTransactionType.SalesReturn)
                            {
                                alRxNos.Add(rxRow.RXNo + "-" + rxRow.NRefill);
                                bReturn = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "CheckRxAlreadyPickedup()");
                bReturn = false;
            }
            return bReturn;
        }
        #endregion

        #region PRIMEPOS-3103
        public void CheckPatientTokenAndCardData(string sPatientNo, int sCustomerID, ref int tokenCount, ref int cardInfoCount)
        {
            tokenCount = 0;
            cardInfoCount = 0;
            if (sPatientNo != "0")
            {
                DataTable dtPatPayPref = GetPatientPayPrefByPatientNo(sPatientNo);
                if (dtPatPayPref != null)
                {
                    cardInfoCount = dtPatPayPref.Rows.Count;
                }
            }
            PharmBL oPhBl = new PharmBL();
            CCCustomerTokInfo tokinfo = new CCCustomerTokInfo();
            if (sCustomerID > 0)
            {
                CCCustomerTokInfoData oCustomerCCProfileTokenData = tokinfo.GetTokenByCustomerandProcessor(sCustomerID);
                if (oCustomerCCProfileTokenData != null && oCustomerCCProfileTokenData.Tables.Count > 0)
                {
                    tokenCount = oCustomerCCProfileTokenData.Tables[0].Rows.Count;
                }
            }
        }
        #endregion

        #region PRIMEPOS-3117 11-Jul-2022 JY Added
        public decimal CalculateTotalTransFeeAmt(POSTransPaymentData oTransPData)
        {
            decimal TotalTransFeeAmt = 0;
            try
            {
                foreach (POSTransPaymentRow row in oTransPData.POSTransPayment.Rows)
                {
                    TotalTransFeeAmt += row.TransFeeAmt;
                }
            }
            catch { }
            return Math.Round(TotalTransFeeAmt, 2);
        }
        #endregion

        #region PRIMEPOS-3130
        public string MaskDrugName(string itemID, string itemDescription)
        {
            try
            {
                if (Configuration.CSetting.MaskDrugName == true)
                {
                    if (itemID.ToLower() == "rx")
                    {
                        string[] arr = itemDescription.Split('-');
                        string itemCode = arr[0];
                        string refillNo = arr[1];
                        //string itemName = arr[2];
                        string itemName = string.Join("-", arr.Skip(2));
                        if (itemName != null && itemName.Length > 0)
                        {
                            int length = itemName.Length;

                            if (length > 3)
                            {
                                string hidden = new string('*', length - 3);
                                itemName = itemName.Substring(0, 3) + hidden;
                                itemDescription = itemCode + "-" + refillNo + "-" + itemName;
                            }
                            if (length == 2)
                            {
                                itemName = itemName.Substring(0, 1) + "*";
                                itemDescription = itemCode + "-" + refillNo + "-" + itemName;
                            }
                            return itemDescription;
                        }
                        else
                            return itemDescription;
                    }
                    else
                        return itemDescription;
                }
                else
                    return itemDescription;
            }
            catch (Exception ex)
            {
                return itemDescription;
            }
        }
        #endregion
    }

    public enum TransactionStToolbars
    {
        strTBTerminal = 1,
        strTBTerminalEntery = 2,
        strTBEditItem = 3
    }

    public enum FuncKeyBrowse
    {
        Forward = 1,
        Backward = 2,
        Home = 3
    }

    public class PrintReciepForOnHoldTransaction
    {
        public const string No = "0";
        public const string Yes = "1";
        public const string Verify = "2";
    }

    public class POSSignContext
    {
        public const string ItemMonitoring = "M";
    }

    #region Sprint-25 - PRIMEPOS-2322 03-Feb-2017 JY Added
    public class InsSigTransDetail
    {
        string _PATIENTNO;
        string _INSTYPE;
        string _TRANSDATA;

        public string PATIENTNO
        {
            get { return _PATIENTNO; }
            set { _PATIENTNO = value; }
        }

        public string INSTYPE
        {
            get { return _INSTYPE; }
            set { _INSTYPE = value; }
        }

        public string TRANSDATA
        {
            get { return _TRANSDATA; }
            set { _TRANSDATA = value; }
        }
    }
    #endregion

    public class OnholdRxs
    {
        public long RxNo;
        public int NRefill;
    }
}