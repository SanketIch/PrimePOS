using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using POS_Core.CommonData;
//using Resources;
using POS_Core.ErrorLogging;
////using POS.Resources;
using POS_Core.Resources;
using Resources;
using NLog;

namespace POS_Core.DataAccess
{
    /// <summary>
    /// Summary description for PreferenceSvr.
    /// </summary>
    public class PreferenceSvr
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public PreferenceSvr()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void UpdateInterfaceColor(IDbTransaction oTrans, string WinBC, string WinFC, string BtnBC1, string BtnBC2, string BtnFC, int AllowAddItem, int MoveToQtyCol, string ActBC, string ActFC, string HdrBC, string HdrFC, int showNumPad, int showItemPad, string theme) //PrimePOS-2523 Added theme by Farman Ansari on 06-June-2018
        {
            //IDbConnection conn;
            //conn=DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);

            string sSQL = "select userid from Util_Interface_Param where userid='" + POS_Core.Resources.Configuration.UserName + "'";
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = oTrans;
            cmd.Connection = oTrans.Connection;
            string result = "";
            result = (string)cmd.ExecuteScalar();
            sSQL = "";
            //Modify by Atul 2-10-10 Add one Parameter AllowRxPicked in insert and update Query.
            if (result == null)
            {
                sSQL = "INSERT INTO Util_Interface_Param " +
                        " (Window_BackColor,Window_ForeColor, Button_BackColor1, Button_BackColor2, Button_ForeColor " +
                    " , MoveToQtyCol, AllowAddItemInTrans, Active_BackColor, Active_ForeColor, Header_BackColor, Header_ForeColor, UserID,showNumPad,showItemPad,Theme )" +
                    " Values ( " +
                    " '" + WinBC + "', '" + WinFC +
                    "', '" + BtnBC1 + "', '" + BtnBC2 + "', '" + BtnFC +
                    "'," + MoveToQtyCol.ToString() + "," + AllowAddItem.ToString() + "," +
                    " '" + ActBC + "', '" + ActFC + "', '" + HdrBC + "', '" + HdrFC + "', '" + Configuration.UserName + "', " +
                    showNumPad + "," + showItemPad + ",'" + theme + "')"; //PrimePOS-2523 Added theme by Farman Ansari on 06-June-2018
            }
            else
            {
                sSQL = "UPDATE Util_Interface_Param SET Window_BackColor='" + WinBC + "', Window_ForeColor='" + WinFC +
                    "', Button_BackColor1='" + BtnBC1 + "', Button_BackColor2='" + BtnBC2 + "', Button_ForeColor='" + BtnFC +
                    "', MoveToQtyCol=" + MoveToQtyCol.ToString() + ", AllowAddItemInTrans=" + AllowAddItem.ToString() + "," +
                    " Active_BackColor='" + ActBC + "', Active_ForeColor='" + ActFC + "', Header_BackColor='" + HdrBC + "',	Header_ForeColor='" + HdrFC + "', showNumPad=" + showNumPad + " , showItemPad=" + showItemPad +
                    " , Theme = '" + theme + "'" +
                    " where userid='" + Configuration.UserName + "' "; //PrimePOS-2523 Added theme by Farman Ansari on 06-June-2018
            }

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
        }

        public void UpdateDeviceSettings(POSSET oPOSSet)
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;

            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                UpdateDeviceSettings(oTrans, oPOSSet);
                oTrans.Commit();
            }
            catch (Exception exp)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                //ErrorHandler.throwCustomError(POSErrorENUM.Pref_UnableToSave);
                throw (exp);
            }
        }

        public void UpdateDeviceSettings(IDbTransaction oTrans, POSSET oPOSSet)
        {
            //IDbConnection conn;
            //conn=DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);

            string sSQL = "select stationid from Util_POSSet where stationid='" + Configuration.StationID + "'";
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = oTrans;
            cmd.Connection = oTrans.Connection;
            string result = "";
            result = (string)cmd.ExecuteScalar();
            sSQL = "";

            if (result == null)
            {
                sSQL = "INSERT INTO Util_POSSet ( " +
                        " PDP_BAUD " +
                        ", PDP_CLSCD " +
                        ", PDP_CODE" +
                        ", PDP_CUROFF" +
                        ", PDP_DBITS" +
                        ", PD_INTRFCE" +
                        ", PD_LINES" +
                        ", PD_LINELEN" +
                        ", PD_MSG" +
                        ", PDP_PARITY" +
                        ", PD_PORT" +
                        ", PDP_STOPB" +
                        ", CDP_BAUD" +
                        ", CDP_BAUD2" +
                        ", CDP_CODE" +
                        ", CDP_CODE2" +
                        ", CDP_DBITS" +
                        ", CDP_DBITS2" +
                        ", CDP_PARITY" +
                        ", CDP_PARIT2" +
                        ", CD_PORT" +
                        ", CDP_STOPB" +
                        ", CDP_STOPB2" +
                        ", CD_TYPE" +
                        ", RP_Name" +
                        ", USECASHDRW" +
                        ", UsePoleDsp" +
                        ", StationID" +
                        ", LoginBeforeTrans" +
                        ", RoundTaxValue" +
                        ", UseSigPad" +
                        ", SigPadHostAddr" +
                        //Added By Dharmendra(SRT) on Nov-13-08 to add those settings related with Card Payments & Pin Pad
                        ",PaymentProcessor" +
                        ",AvsMode" +
                        ",TxnTimeOut" +
                        ",UsePinPad" +
                        ",PinPadModel" +
                        ",PinPadBaudRate" +
                        ",PinPadPairity" +
                        ",PinPadPortNo" +
                        ",PinPadDataBits" +
                        ",PinPadDispMesg" +
                        ",PinPadKeyEncryptionType" +
                        ",HeartBeatTime" +
                        ",ProcessOnLine" +            //Added By Dharmendra(SRT) on Nov-13-08
                                                      //Add Ended
                        ",ShowAuthorization" +      //Added By Dharmendra(SRT) on May-13-09
                        ",ShowCustomerNotes" +
                        ",FetchUnbilledRx" +       //Added by SRT(Abhishek) on 17 Aug 2009
                        ",CaptureSigForDebit" +       //Added by SRT(Ritesh Parekh) on 20 Aug 2009
                        ",DispSigOnHouseCharge" +   //Added by Manoj 11/21/2011
                        ",AllowPickUpRx" + //Added by Manoj 1/23/2013
                        ",ReceiptPrinterType" +
                        "NoOfReceipt ,"
                         + "NoOfOnHoldTransReceipt  ,"
                         //+ "NoOfCC  ,"
                         //+ "NoOfHCRC  ,"
                         + "NoOfRARC  ,"
                         //+ "NoOfCheckRC ,"
                         + "NoOfGiftReceipt , "
                        //+ "NoOfCashReceipts  ," +
                        + ",HPS_USERNAME" +
                        ",HPS_PASSWORD" +
                        ",ALLOWDUP" +
                        ",PreferReverse" +
                        ",AllowRxPicked" +
                        ",MaxCashLimitForStnCose" +
                        ",IVULottoTerminalID" +
                        ",IVULottoPassword	" +
                        ",IVULottoServerURL" +
                        ",SelectMultipleTaxes" +    //Sprint-19 - 2146 26-Dec-2014 JY Added to select multiple taxes functionality
                        ",WP_SubID" +   //Added by Rohit Nair on May-3-2016 for WorldPay Integration
                        ",ShowRxNotes" +    //PRIMEPOS-2459 03-Apr-2019 JY Added
                        ",ShowPatientNotes" +    //PRIMEPOS-2459 03-Apr-2019 JY Added
                        ",ShowItemNotes" +  //PRIMEPOS-2536 14-May-2019 JY Added
                        ",TerminalID" + //PrimePOS-2491 
                        ",SkipEMVCardSign" +
                         ", AllowManualFirstMiles" + // allow first mile manual transaction - NileshJ - PRIMEPOS-2737  30-Sept-2019
                        ", SkipRxSignature" +    // allow first mile manual transaction - NileshJ - PRIMEPOS-2737  30-Sept-2019
                        ", EnableStoreCredit" + //  NileshJ - PRIMEPOS-2747 - StoreCredit
                        ", UserID" + //  PRIMEPOS-2808 Added by Arvind
                #region PRIMEPOS-2996 22-Sep-2021 JY Added
                        ", ReportPrinter" +
                        ", ReceiptPrinterPaperSource" +
                        ", LabelPrinterPaperSource" +
                        ", ReportPrinterPaperSource" +
                #endregion
                #region PRIMEPOS-3455
                        ", IsSecuredDevice" +
                        ", SecureDeviceModel" +
                        ", SecureDeviceSrNumber" +
                #endregion
                #region PRIMEPOS-3167 07-Nov-2022 JY Commented
                    //", UsePrimePO, HostAddress, ConnectionTimer, PurchaseOrderTimer, PriceUpdateTimer, RemoteURL, ConsiderReturnTrans" +    //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added                     
                    //", UpdateDescription, INSERT11DIGITITEM, IgnoreVendorSequence" +
                #endregion
                    " ) Values ( " +
                        "'" + oPOSSet.PDP_BAUD.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PDP_CLSCD.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PDP_CODE.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PDP_CUROFF.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PDP_DBITS.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PD_INTRFCE.Replace("'", "''") + "'" +
                        "," + oPOSSet.PD_LINES +
                        "," + oPOSSet.PD_LINELEN +
                        ",'" + oPOSSet.PD_MSG.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PDP_PARITY.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PD_PORT.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PDP_STOPB.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_BAUD.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_BAUD2.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_CODE.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_CODE2.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_DBITS.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_DBITS2.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_PARITY.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_PARITY2.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CD_PORT.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_STOPB.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CDP_STOPB2.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.CD_TYPE.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.RP_Name.Replace("'", "''") + "'" +
                        "," + Configuration.convertBoolToInt(oPOSSet.USECASHDRW) +
                        "," + Configuration.convertBoolToInt(oPOSSet.UsePoleDisplay) +
                        ", '" + Configuration.StationID + "'" +
                        "," + Configuration.convertBoolToInt(oPOSSet.LoginBeforeTrans) +
                        "," + Configuration.convertBoolToInt(oPOSSet.RoundTaxValue) +
                        "," + Configuration.convertBoolToInt(oPOSSet.UseSigPad) +
                        ",'" + oPOSSet.SigPadHostAddr.Replace("'", "''") + "'" +
                        //Added By Dharmendra(SRT) on Nov-13-08 to add those settings related with Card Payments & Pin Pad
                        ",'" + oPOSSet.PaymentProcessor.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.AvsMode.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.TxnTimeOut.Replace("'", "''") + "'" +
                        ",'" + Configuration.convertBoolToInt(oPOSSet.UsePinPad) + "'" +
                        ",'" + oPOSSet.OrigPinPadModel.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PinPadBaudRate.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PinPadPairity.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PinPadPortNo.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PinPadDataBits.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PinPadDispMesg.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.PinPadKeyEncryptionType.Replace("'", "''") + "'" +
                        ",'" + oPOSSet.HeartBeatTime.Replace("'", "''") + "'" +
                        ",'" + Configuration.convertBoolToInt(oPOSSet.ProcessOnLine) + "'" + //Added By Dharmendra (SRT) on Nov-13-08
                    "," + Configuration.convertBoolToInt(oPOSSet.ShowAuthorization) +   //Added By Dharmendra (SRT) on May-13-09
                    "," + Configuration.convertBoolToInt(oPOSSet.ShowCustomerNotes) +
                    "," + Configuration.convertNullToInt(oPOSSet.FetchUnbilledRx) +  //Added by SRT(Abhishek) 17 Aug 2009   //PRIMEPOS-2398 04-Jan-2021 JY modified
                    "," + Configuration.convertBoolToInt(oPOSSet.CaptureSigForDebit) + //Added by SRT(Abhishek) 17 Aug 2009
                    "," + Configuration.convertBoolToInt(oPOSSet.DispSigOnHouseCharge) + //Added by Manoj 11/21/2011
                    "," + Configuration.convertBoolToInt(oPOSSet.CaptureSigForEBT) + //Added by Manoj 7/2/2013
                    "," + Configuration.convertBoolToInt(oPOSSet.AllowPickedUpRxToTrans) + //Added by Manoj 1/23/2013
                    "," + Configuration.convertBoolToInt(oPOSSet.SkipF10Sign) + //Added by Manoj 9/26/2013
                    "," + Configuration.convertBoolToInt(oPOSSet.SkipAmountSign) + //Added by Manoj 9/26/2013
                    "," + Configuration.convertNullToInt(oPOSSet.ControlByID) + //Added by Manoj 4/2/2013   //PRIMEPOS-2547 03-Jul-2018 JY Modified
                    "," + Configuration.convertNullToInt(oPOSSet.AskVerificationIdMode) +   //PRIMEPOS-2547 11-Jul-2018 JY Added
                    "," + Configuration.convertBoolToInt(oPOSSet.SkipDelSign) + //Added by Manoj 5/8/2013
                    ",'" + oPOSSet.ReceiptPrinterType.Replace("'", "''") + "'" +
                    //",'" + oPOSSet.NoOfCC + "'" + //Added by Ravindra                         
                    ",'" + oPOSSet.NoOfReceipt + "'" +
                    ",'" + oPOSSet.NoOfOnHoldTransReceipt + "'" +
                    //",'" + oPOSSet.NoOfCC + "'" +
                    //",'" + oPOSSet.NoOfHCRC + "'" +
                    ",'" + oPOSSet.NoOfRARC + "'" +
                    //",'" + oPOSSet.NoOfCheckRC + "'" +
                    ",'" + oPOSSet.NoOfGiftReceipt + "'" +
                    //",'" + oPOSSet.NoOfCashReceipts + "'" +
                    ",'" + oPOSSet.HPS_USERNAME + "'" +
                    ",'" + oPOSSet.HPS_PASSWORD + "'" +
                    ",'" + Configuration.convertNullToBoolean(oPOSSet.ALLOWDUP) + "'" +
                    ",'" + Configuration.convertNullToBoolean(oPOSSet.PreferReverse) + "'" +
                    ",'" + oPOSSet.AllowRxPicked + "'" +
                    ",'" + oPOSSet.MaxCashLimitForStnCose + "'" +
                    ",'" + oPOSSet.IVULottoTerminalID + "'" +
                    ",'" + oPOSSet.IVULottoPassword + "'" +
                    ",'" + oPOSSet.IVULottoServerURL + "'" +
                    "," + Configuration.convertBoolToInt(oPOSSet.SelectMultipleTaxes) + //Sprint-19 - 2146 26-Dec-2014 JY Added to select multiple taxes functionality
                    ",'" + oPOSSet.WP_SubID + "'" + //Added by Rohit Nair on May-3-2016 for WorldPay Integration
                    "," + Configuration.convertBoolToInt(oPOSSet.ShowRxNotes) + //PRIMEPOS-2459 03-Apr-2019 JY Added
                    "," + Configuration.convertBoolToInt(oPOSSet.ShowPatientNotes) + //PRIMEPOS-2459 03-Apr-2019 JY Added
                    "," + Configuration.convertBoolToInt(oPOSSet.ShowItemNotes) +   //PRIMEPOS-2536 14-May-2019 JY Added
                    "," + oPOSSet.TerminalID + "'" + //PrimePOS-2491 
                    "," + Configuration.convertBoolToInt(oPOSSet.SkipEMVCardSign) +
                     ",'" + Configuration.convertBoolToInt(oPOSSet.AllowManualFirstMiles) + "'" + // allow first mile manual transaction - NileshJ - PRIMEPOS-2737 30-Sept-2019
                    ",'" + Configuration.convertBoolToInt(oPOSSet.SkipRxSignature) + "'" +    // allow first mile manual transaction - NileshJ - PRIMEPOS-2737 30-Sept-2019
                    ",'" + Configuration.convertBoolToInt(oPOSSet.EnableStoreCredit) + "'" + // NileshJ - PRIMEPOS-2747 -StoreCredit
                    ",'" + Configuration.UserName + "'" + // PRIMEPOS-2808 Added by Arvind
                #region PRIMEPOS-2996 22-Sep-2021 JY Added
                    ",'" + oPOSSet.ReportPrinter.Replace("'", "''") + "'" +
                    ",'" + oPOSSet.ReceiptPrinterPaperSource.Replace("'", "''") + "'" +
                    ",'" + oPOSSet.LabelPrinterPaperSource.Replace("'", "''") + "'" +
                    ",'" + oPOSSet.ReportPrinterPaperSource.Replace("'", "''") + "'" +
                #endregion
                #region PRIMEPOS-3455
                    "," + Configuration.convertBoolToInt(oPOSSet.IsSecureDevice) +
                    ",'" + oPOSSet.SecureDeviceModel.Replace("'", "''") + "'" +
                    ",'" + oPOSSet.SecureDeviceSrNumber.Replace("'", "''") + "'" +
                #endregion
                #region PRIMEPOS-3167 07-Nov-2022 JY Commented
                    //"," + Configuration.convertBoolToInt(oPOSSet.USePrimePO) + "" +      //Added By Dharmendra (SRT) on Apr-29-09
                    //",'" + oPOSSet.HostAddress.Replace("'", "''") + "'" +
                    //"," + Configuration.convertNullToInt(oPOSSet.ConnectionTimer.ToString()) + "" +
                    //"," + Configuration.convertNullToInt(oPOSSet.PurchaseOrdTimer.ToString()) + "" +
                    //"," + Configuration.convertNullToInt(oPOSSet.PriceUpdateTimer.ToString()) + "" +
                    //"," + oPOSSet.RemoteURL +
                    //"," + Configuration.convertBoolToInt(oPOSSet.ConsiderReturnTrans) + //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added 
                    //"," + Configuration.convertBoolToInt(oPOSSet.UpdateDescription) +
                    //"," + Configuration.convertBoolToInt(oPOSSet.Insert11DigitItem) + ")"; //Added by SRT(Sachin) 23 Feb 2010
                    //"," + Configuration.convertBoolToInt(oPOSSet.IgnoreVendorSequence) +
                #endregion
                    ")";
                //",'"+Configuration.convertNullToBoolean(oPOSSet.PreferReverse)+"'"+")";
                //Added by Prog1
            }
            else
            {
                sSQL = "UPDATE Util_POSSet SET " +
                    " PDP_BAUD= '" + oPOSSet.PDP_BAUD.Replace("'", "''") + "'" +
                    ", PDP_CLSCD= '" + oPOSSet.PDP_CLSCD.Replace("'", "''") + "'" +
                    ", PDP_CODE='" + oPOSSet.PDP_CODE.Replace("'", "''") + "'" +
                    ", PDP_CUROFF='" + oPOSSet.PDP_CUROFF.Replace("'", "''") + "'" +
                    ", PDP_DBITS='" + oPOSSet.PDP_DBITS.Replace("'", "''") + "'" +
                    ", PD_INTRFCE='" + oPOSSet.PD_INTRFCE.Replace("'", "''") + "'" +
                    ", PD_LINES=" + oPOSSet.PD_LINES + "" +
                    ", PD_LINELEN=" + oPOSSet.PD_LINELEN + "" +
                    ", PD_MSG='" + oPOSSet.PD_MSG.Replace("'", "''") + "'" +
                    ", PDP_PARITY='" + oPOSSet.PDP_PARITY.Replace("'", "''") + "'" +
                    ", PD_PORT='" + oPOSSet.PD_PORT.Replace("'", "''") + "'" +
                    ", PDP_STOPB='" + oPOSSet.PDP_STOPB.Replace("'", "''") + "'" +
                    ", CDP_BAUD='" + oPOSSet.CDP_BAUD.Replace("'", "''") + "'" +
                    ", CDP_BAUD2='" + oPOSSet.CDP_BAUD2.Replace("'", "''") + "'" +
                    ", CDP_CODE='" + oPOSSet.CDP_CODE.Replace("'", "''") + "'" +
                    ", CDP_CODE2='" + oPOSSet.CDP_CODE2.Replace("'", "''") + "'" +
                    ", CDP_DBITS='" + oPOSSet.CDP_DBITS.Replace("'", "''") + "'" +
                    ", CDP_DBITS2='" + oPOSSet.CDP_DBITS2.Replace("'", "''") + "'" +
                    ", CDP_PARITY='" + oPOSSet.CDP_PARITY.Replace("'", "''") + "'" +
                    ", CDP_PARIT2='" + oPOSSet.CDP_PARITY2.Replace("'", "''") + "'" +
                    ", CD_PORT='" + oPOSSet.CD_PORT.Replace("'", "''") + "'" +
                    ", CDP_STOPB='" + oPOSSet.CDP_STOPB.Replace("'", "''") + "'" +
                    ", CDP_STOPB2='" + oPOSSet.CDP_STOPB2.Replace("'", "''") + "'" +
                    ", CD_TYPE='" + oPOSSet.CD_TYPE.Replace("'", "''") + "'" +
                    ", RP_Name='" + oPOSSet.RP_Name.Replace("'", "''") + "'" +
                    ", LabelPrinter='" + oPOSSet.LabelPrinter.Replace("'", "''") + "'" +
                    //", RP_CCPrint=" + oPOSSet.RP_CCPrint +
                    ", UsePoleDsp=" + Configuration.convertBoolToInt(oPOSSet.UsePoleDisplay) +
                    ", USECASHDRW=" + Configuration.convertBoolToInt(oPOSSet.USECASHDRW) +
                    ", LoginBeforeTrans=" + Configuration.convertBoolToInt(oPOSSet.LoginBeforeTrans) +
                    ", UseSigPad=" + Configuration.convertBoolToInt(oPOSSet.UseSigPad) +
                    ", SigPadHostAddr='" + oPOSSet.SigPadHostAddr.Replace("'", "''") + "'" +

                    //Added By Dharmendra(SRT) on Nov-13-08 to add those settings related with Card Payments & Pin Pad
                    ",UsePinPad=" + Configuration.convertBoolToInt(oPOSSet.UsePinPad) +
                    ",PinPadModel='" + oPOSSet.OrigPinPadModel.Replace("'", "''") + "'" +
                    ",PinPadBaudRate='" + oPOSSet.PinPadBaudRate.Replace("'", "''") + "'" +
                    ",PinPadPairity='" + oPOSSet.PinPadPairity.Substring(0, 1).Replace("'", "''") + "'" +
                    ",PinPadPortNo='" + oPOSSet.PinPadPortNo.Replace("'", "''") + "'" +
                    ",PinPadDataBits='" + oPOSSet.PinPadDataBits.Replace("'", "''") + "'" +
                    ",PinPadDispMesg='" + oPOSSet.PinPadDispMesg.Replace("'", "''") + "'" +
                    ",PinPadKeyEncryptionType='" + oPOSSet.PinPadKeyEncryptionType.Replace("'", "''") + "'" +                    
                    ",FetchUnbilledRx=" + Configuration.convertNullToInt(oPOSSet.FetchUnbilledRx) + "" +    //PRIMEPOS-2398 04-Jan-2021 JY modified
                                                                                                            //End of Added By SRT(Abhishek) Date: 17-Aug-2009
                                                                                                            //Added By SRT(Ritesh Parekh) Date : 20 Aug 2009
                                                                                                            //", NoOfCC=" + Configuration.convertNullToInt(oPOSSet.NoOfCC) + "" +//added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set    
                    ", NoOfReceipt=" + Configuration.convertNullToInt(oPOSSet.NoOfReceipt) + "" +
                    ", NoOfOnHoldTransReceipt=" + Configuration.convertNullToInt(oPOSSet.NoOfOnHoldTransReceipt) + "" +
                    //", NoOfHCRC=" + Configuration.convertNullToInt(oPOSSet.NoOfHCRC) + "" +
                    ", NoOfRARC=" + Configuration.convertNullToInt(oPOSSet.NoOfRARC) + "" +
                    //", NoOfCheckRC= " + Configuration.convertNullToInt(oPOSSet.NoOfCheckRC) + "" +
                    ", NoOfGiftReceipt= " + Configuration.convertNullToInt(oPOSSet.NoOfGiftReceipt) + "" +
                    //", NoOfCashReceipts= " + Configuration.convertNullToInt(oPOSSet.NoOfCashReceipts) +
                    //Till here added by Ravindra PRIMEPOS-1538   Number of receipts printed to be a station set rather then a global set    
                    ",CaptureSigForDebit=" + Configuration.convertBoolToInt(oPOSSet.CaptureSigForDebit) + "" +
                    ",CaptureSigForEBT=" + Configuration.convertBoolToInt(oPOSSet.CaptureSigForEBT) + "" + //Added by Manoj 7/2/2013
                    ",MaxCATransAmt=" + Configuration.convertNullToInt(oPOSSet.MaxCATransAmt) + "" +    //Added by SRT as a part of improper fix by Naim on 25-Jan-10
                                                                                                        //End of Added By SRT(Ritesh Parekh) Date: 20-Aug-2009
                    ", ReceiptPrinterType='" + oPOSSet.ReceiptPrinterType.Replace("'", "''") + "'" +                    
                    ", ALLOWDUP=" + Configuration.convertBoolToInt(oPOSSet.ALLOWDUP) + "" +
                    ", MaxCashLimitForStnCose=" + Configuration.convertNullToDecimal(oPOSSet.MaxCashLimitForStnCose) + "" + //Added By shitaljit on 1/7/2013 for maximum station close cash limit JIRA-1044
                    ", SelectMultipleTaxes = " + Configuration.convertBoolToInt(oPOSSet.SelectMultipleTaxes) + //Sprint-19 - 2146 26-Dec-2014 JY Added to select multiple taxes functionality
                    ", WP_SubID = '" + oPOSSet.WP_SubID + "'" + //Added by Rohit Nair on May-3-2016 for WorldPay Integration
                    ", ShowItemNotes = " + Configuration.convertBoolToInt(oPOSSet.ShowItemNotes) +   //PRIMEPOS-2536 14-May-2019 JY Added      
                     ", AllowManualFirstMiles ='" + Configuration.convertBoolToInt(oPOSSet.AllowManualFirstMiles) + "'" + // allow first mile manual transaction - NileshJ - PRIMEPOS-2737  30-Sept-2019
                    ", SkipRxSignature ='" + Configuration.convertBoolToInt(oPOSSet.SkipRxSignature) + "'" +    // allow first mile manual transaction - NileshJ - PRIMEPOS-2737 30-Sept-2019                                   
                    ", EnableStoreCredit ='" + Configuration.convertBoolToInt(oPOSSet.EnableStoreCredit) + "'" + // NileshJ PRIMEPOS-2747 StoreCredit
                    ", UserID ='" + Configuration.UserName + "'" + // PRIMEPOS-2808 Added by Arvind
                #region PRIMEPOS-2996 22-Sep-2021 JY Added
                    ", ReportPrinter = '" + oPOSSet.ReportPrinter.Replace("'", "''") + "'" +
                    ", ReceiptPrinterPaperSource = '" + oPOSSet.ReceiptPrinterPaperSource.Replace("'", "''") + "'" +
                    ", LabelPrinterPaperSource = '" + oPOSSet.LabelPrinterPaperSource.Replace("'", "''") + "'" +
                    ", ReportPrinterPaperSource = '" + oPOSSet.ReportPrinterPaperSource.Replace("'", "''") + "'" +
                #endregion
                #region PRIMEPOS-3455
                    ", IsSecuredDevice  = " + Configuration.convertBoolToInt(oPOSSet.IsSecureDevice) +
                    ", SecureDeviceModel = '" + oPOSSet.SecureDeviceModel.Replace("'", "''") + "'" +
                    ", SecureDeviceSrNumber = '" + oPOSSet.SecureDeviceSrNumber.Replace("'", "''") + "'" +
                #endregion
                    //",UpdateDescription=" + Configuration.convertBoolToInt(oPOSSet.UpdateDescription) + ", IgnoreVendorSequence='" + Configuration.convertBoolToInt(oPOSSet.IgnoreVendorSequence) + "'" + //PRIMEPOS-3167 07-Nov-2022 JY Commented
                    "  where StationID='" + Configuration.StationID.Replace("'", "''") + "'";
            }

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
        }

        #region PRIMEPOS-3167 07-Nov-2022 JY Added
        public void UpdatePrimeEDISettings(IDbTransaction oTrans, PrimeEDISetting oPrimeEDISetting)
        {
            string sSQL = "SELECT ID FROM PrimeEDISetting";
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = oTrans;
            cmd.Connection = oTrans.Connection;
            object result = cmd.ExecuteScalar();

            if (result == null)
            {
                sSQL = "INSERT INTO PrimeEDISetting (ID, UsePrimePO, HostAddress, ConnectionTimer, PurchaseOrderTimer, PriceUpdateTimer, RemoteURL, ConsiderReturnTrans, UpdateVendorPrice, UpdateDescription, Insert11DigitItem, IgnoreVendorSequence, DefaultVendor, UseDefaultVendor, AutoPOSeq)" +
                    " Values (1, " + Configuration.convertBoolToInt(oPrimeEDISetting.UsePrimePO) + ",'"
                    + oPrimeEDISetting.HostAddress.Replace("'", "''") + "'," +
                    +Configuration.convertNullToInt(oPrimeEDISetting.ConnectionTimer.ToString()) +
                    "," + Configuration.convertNullToInt(oPrimeEDISetting.PurchaseOrderTimer.ToString()) +
                    "," + Configuration.convertNullToInt(oPrimeEDISetting.PriceUpdateTimer.ToString()) +
                    ",'" + oPrimeEDISetting.RemoteURL.Trim().Replace("'", "''") +
                    "'," + Configuration.convertBoolToInt(oPrimeEDISetting.ConsiderReturnTrans) +
                    "," + Configuration.convertBoolToInt(oPrimeEDISetting.UpdateVendorPrice) +
                    "," + Configuration.convertBoolToInt(oPrimeEDISetting.UpdateDescription) +
                    "," + Configuration.convertBoolToInt(oPrimeEDISetting.Insert11DigitItem) +
                    "," + Configuration.convertBoolToInt(oPrimeEDISetting.IgnoreVendorSequence) +
                    ", '" + oPrimeEDISetting.DefaultVendor.Trim().Replace("'", "''") +
                    "', " + Configuration.convertBoolToInt(oPrimeEDISetting.UseDefaultVendor) +
                    ",'" + oPrimeEDISetting.AutoPOSeq.Trim().Replace("'", "''") + "')";
            }
            else
            {
                sSQL = "UPDATE PrimeEDISetting SET UsePrimePO = " + Configuration.convertBoolToInt(oPrimeEDISetting.UsePrimePO) +
                    ", HostAddress = '" + oPrimeEDISetting.HostAddress.Trim().Replace("'", "''") + "'" +
                    ", ConnectionTimer = " + Configuration.convertNullToInt(oPrimeEDISetting.ConnectionTimer.ToString()) +
                    ", PurchaseOrderTimer = " + Configuration.convertNullToInt(oPrimeEDISetting.PurchaseOrderTimer.ToString()) +
                    ", PriceUpdateTimer = " + Configuration.convertNullToInt(oPrimeEDISetting.PriceUpdateTimer.ToString()) +
                    ", RemoteURL = '" + oPrimeEDISetting.RemoteURL.Trim().Replace("'", "''") + "'" +
                    ", ConsiderReturnTrans = " + Configuration.convertBoolToInt(oPrimeEDISetting.ConsiderReturnTrans) +  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added                                         
                    ", UpdateVendorPrice = " + Configuration.convertBoolToInt(oPrimeEDISetting.UpdateVendorPrice) +
                    ", UpdateDescription = " + Configuration.convertBoolToInt(oPrimeEDISetting.UpdateDescription) +
                    ", Insert11DigitItem = " + Configuration.convertBoolToInt(oPrimeEDISetting.Insert11DigitItem) +
                    ", IgnoreVendorSequence = " + Configuration.convertBoolToInt(oPrimeEDISetting.IgnoreVendorSequence) +
                    ", DefaultVendor = '" + oPrimeEDISetting.DefaultVendor.Trim().Replace("'", "''") + "'" +
                    ", UseDefaultVendor = " + Configuration.convertBoolToInt(oPrimeEDISetting.UseDefaultVendor) +
                    ", AutoPOSeq = '" + oPrimeEDISetting.AutoPOSeq.Trim().Replace("'", "''") + "'";
            }

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
        }
        #endregion

        public void UpdateAppSettings(IDbTransaction oTrans, POSSET oPOSSet)
        {
            //IDbConnection conn;
            //conn=DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);

            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            string sSQL = "UPDATE Util_POSSet SET " +
                    " SingleUserLogin=" + Configuration.convertBoolToInt(oPOSSet.SingleUserLogin) + "" +
                    ", UseRcptForCloseStation=" + Configuration.convertBoolToInt(oPOSSet.UseRcptForCloseStation) +
                    ", UseRcptForEOD=" + Configuration.convertBoolToInt(oPOSSet.UseRcptForEOD) +
                    ", Inactive_Interval=" + oPOSSet.Inactive_Interval.ToString() +
                    ", RestrictConcurrentLogin=" + Configuration.convertBoolToInt(oPOSSet.RestrictConcurrentLogin) +
                    ", RoundTaxValue=" + Configuration.convertBoolToInt(oPOSSet.RoundTaxValue) +
                    ", ApplyPriceValidation=" + Configuration.convertBoolToInt(oPOSSet.ApplyPriceValidation) +
                    ", ApplyGPOvercompItem=" + Configuration.convertBoolToInt(oPOSSet.ApplyGroupPriceOverCompanionItem) +
                    ", ShowCustomerNotes=" + Configuration.convertBoolToInt(oPOSSet.ShowCustomerNotes) +
                    //Added By Dharmendra(SRT) on Nov-13-08 to add those station specific
                    ", DispSigOnHouseCharge =" + Configuration.convertBoolToInt(oPOSSet.DispSigOnHouseCharge) + "" + // Added by Manoj 11/21/2011                    
                    ", SkipF10Sign =" + Configuration.convertBoolToInt(oPOSSet.SkipF10Sign) + "" + //Added by Manoj 9/26/2013                    
                    ", ALLOWZEROAMTTRANSACTION=" + Configuration.convertBoolToInt(oPOSSet.AllowZeroAmtTransaction) + "" +  //Added By SRT(Abhishek D) Date: 12 Feb 2010                    
                    //", RemoteURL='" + oPOSSet.RemoteURL + "'" +   
                    ", HPS_USERNAME='" + oPOSSet.HPS_USERNAME + "'" +
                    ", HPS_PASSWORD='" + oPOSSet.HPS_PASSWORD + "'" +
                    ", PreferReverse=" + Configuration.convertBoolToInt(oPOSSet.PreferReverse) + "" +
                     ", WP_SubID = '" + oPOSSet.WP_SubID + "'" + //Added by Rohit Nair on May-3-2016 for WorldPay Integration
                     ", SkipEMVCardSign=" + Configuration.convertBoolToInt(oPOSSet.SkipEMVCardSign) +
                       ", AllowManualFirstMiles ='" + Configuration.convertBoolToInt(oPOSSet.AllowManualFirstMiles) + "'" + // allow first mile manual transaction - NileshJ - PRIMEPOS-2737 30-Sept-2019
                    ", SkipRxSignature ='" + Configuration.convertBoolToInt(oPOSSet.SkipRxSignature) + "'" +    // allow first mile manual transaction - NileshJ - PRIMEPOS-2737 30-Sept-2019
                    ", EnableStoreCredit ='" + Configuration.convertBoolToInt(oPOSSet.EnableStoreCredit) + "'" + // NileshJ - PRIMEPOS-2737 - StoreCredit
                    ", UserID ='" + Configuration.UserName + "'" + // PRIMEPOS-2808 Added by Arvind
                   " where StationID='" + Configuration.StationID.Replace("'", "''") + "'";
            //Add Ended

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
        }

        public void UpdateCompanyInfo(IDbTransaction oTrans, CompanyInfo oCInfo)
        {
            //IDbConnection conn;
            //conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);

            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            //Original Commented By Amit
            //string sSQL = "UPDATE Util_Company_Info SET " +
            //        " AllowCashBack=" + Configuration.convertBoolToInt(oCInfo.AllowCashBack) + "" +
            //        ", ChargeCashBackInPercent=" + Configuration.convertBoolToInt(oCInfo.ChargeCashBackInPercent);
            //End

            //Following Code is added By shitaljit(QuicSolv) on June
            //Changes are made on UPDATE query added fields.
            string sSQL = "";
            sSQL = "UPDATE Util_Company_Info SET " +
                    " AllowCashBack=" + Configuration.convertBoolToInt(oCInfo.AllowCashBack) + "" +
                    ", ChargeCashBackMode='" + Configuration.convertNullToString(oCInfo.ChargeCashBackMode) + "'" +
                    ",PrintStCloseNo=" + Configuration.convertBoolToInt(oCInfo.PrintStCloseNo) + "" +
                    ",PrintEODNo=" + Configuration.convertBoolToInt(oCInfo.PrintEODNo) + "" +
                    ",AllowMultipleInstanceOfPOS=" + Configuration.convertNullToInt(oCInfo.AllowMultipleInstanceOfPOS) + "" +   //PRIMEPOS-2936 21-Jan-2021 JY Modified
                    ",UseCashManagement = " + Configuration.convertBoolToInt(oCInfo.UseCashManagement) + "" +
                    ",DefInvRetId=" + Configuration.convertNullToInt(oCInfo.DefaultInvReturnID) + "" +
                    ",DefInvRecvId=" + Configuration.convertNullToInt(oCInfo.DefalutInvRecievedID) + "" +
                    ",DEFAULTDEPTID =" + Configuration.convertNullToInt(oCInfo.DefaultDeptId) + "" +
                    ",DefCDStartBalance = " + oCInfo.DefCDStartBalance + "" +
                    ",DaysforWarn=" + oCInfo.DaysForWarn + "" +
                    ",NoOfReceipt=" + Configuration.convertNullToInt(oCInfo.NoOfReceipt) + "" +
                    ",NoOfOnHoldTransReceipt=" + Configuration.convertNullToInt(oCInfo.NoOfOnHoldTransReceipt) + "" +
                    ",NoOfCC=" + Configuration.convertNullToInt(oCInfo.NoOfCC) + "" +
                    ",NoOfHCRC=" + Configuration.convertNullToInt(oCInfo.NoOfHCRC) + "" +
                    ",NoOfRARC=" + Configuration.convertNullToInt(oCInfo.NoOfRARC) + "" +
                    ",TagFSA =" + Configuration.convertBoolToInt(oCInfo.TagFSA) + "" +
                    ",TagTaxable =" + Configuration.convertBoolToInt(oCInfo.TagTaxable) + "" +
                    ",TagMonitored =" + Configuration.convertBoolToInt(oCInfo.TagMonitored) + "" +
                    ",TagEBT =" + Configuration.convertBoolToInt(oCInfo.TagEBT) + "" +
                    ",ByPassPayScreen =" + Configuration.convertBoolToInt(oCInfo.ByPassPayScreen) + "" + //Added by Manoj 9/26/2013
                    ",MergeSign =" + Configuration.convertBoolToInt(oCInfo.MergeSign) + "" + //Added by Manoj 10/01/2013
                    ", NoOfCheckRC= " + Configuration.convertNullToInt(oCInfo.NoOfCheckRC) + "" +//Added By Shitaljit(QuicSolv) on 27 dec 2011
                    ", NoOfGiftReceipt= " + Configuration.convertNullToInt(oCInfo.NoOfGiftReceipt) + "" +//Added By Shitaljit(QuicSolv) 8 jan 2012
                    ", ShowTextPrediction= " + Configuration.convertBoolToInt(oCInfo.ShowTextPrediction) +//Added on 10 Aug for Text Prediction on Item Add
                    ", EnforceComplexPassword= " + Configuration.convertBoolToInt(oCInfo.EnforceComplexPassword) +//Added on 8 Oct 2012 for Password Complexity check logic
                    ", PromptForSellingPriceLessThanCost= " + Configuration.convertBoolToInt(oCInfo.PromptForSellingPriceLessThanCost) +//  //add by Ravindra for on 21 Mrach 2013 // PromptForSellingPriceLessThanCost add By Ravindra
                    ", PRINTRECEIPTFORONHOLDTRANS = '" + Configuration.convertNullToString(oCInfo.PrintReceiptForOnHoldTrans) + "' " +
                    ", PrintReceipt= '" + oCInfo.PrintReceipt + "' " +
                    ", AllowPrintZeroTrans =  " + Configuration.convertBoolToInt(oCInfo.AllowPrintZeroTrans) +
                    ", DoNotOpenDrawerForCCOnlyTrans =  " + Configuration.convertBoolToInt(oCInfo.DoNotOpenDrawerForCCOnlyTrans) +
                    ", DoNotOpenDrawerForChequeTrans =  " + Configuration.convertBoolToInt(oCInfo.DoNotOpenDrawerForChequeTrans) +
                    ", DoNotOpenDrawerForHouseChargeOnlyTrans =  " + Configuration.convertBoolToInt(oCInfo.DoNotOpenDrawerForHouseChargeOnlyTrans) +    //Sprint-19 - 2161 30-Mar-2015 JY Added 
                    ", AllowZeroSellingPrice =  " + Configuration.convertBoolToInt(oCInfo.AllowZeroSellingPrice) +    //Sprint-21 - 2204 26-Jun-2015 JY Added
                    ", RestrictInActiveItem =  " + Configuration.convertBoolToInt(oCInfo.RestrictInActiveItem) +    //Sprint-21 - 2173 10-Jul-2015 JY Added
                    ", PrintReceiptInMultipleLanguage =  " + Configuration.convertBoolToInt(oCInfo.PrintReceiptInMultipleLanguage) +    //Sprint-21 - 1272 25-Aug-2015 JY Added
                    ", ConsiderItemType =  " + Configuration.convertBoolToInt(oCInfo.ConsiderItemType) +    //Sprint-22 16-Dec-2015 JY Added settings to control the ItemType behavior 
                    ", NoOfCashReceipts= " + Configuration.convertNullToInt(oCInfo.NoOfCashReceipts) +
                    ", DefaultTaxableItem ='" + Configuration.convertNullToString(oCInfo.DefaultTaxableItem) + "'" +
                    ", DefaultNonTaxableItem='" + Configuration.convertNullToString(oCInfo.DefaultNonTaxableItem) + "'" +
                    ", OpenDrawerForZeroAmtTrans='" + Configuration.convertBoolToInt(oCInfo.OpenDrawerForZeroAmtTrans) + "'" +//Added By shitaljit on 9/7/2103
                    ", AutoPopulateCLCardDetails='" + Configuration.convertBoolToInt(oCInfo.AutoPopulateCLCardDetails) + "'" +//added By shitaljit on 25July2013 for PrimePOS-436 Loyalty cards could be displayed for Customer during payment
                    ", AllowItemComboPrice =" + Configuration.convertBoolToInt(oCInfo.AllowItemComboPrice) + "" +
                    ", OutGoingEmailSubject ='" + Configuration.convertNullToString(oCInfo.OutGoingEmailSubject) + "'" +
                    ", PromptForZeroCostPrice ='" + Configuration.convertBoolToInt(oCInfo.PromptForZeroCostPrice) + "'" +
                    ", PromptForZeroSellingPrice ='" + Configuration.convertBoolToInt(oCInfo.PromptForZeroSellingPrice) + "'" +
                     ", CurrencySymbol  = @CurrencySymbol " +

            #region Sprint-23 - PRIMEPOS-2244 19-May-2016 JY Added 
            ", PrintStationCloseDateTime = " + Configuration.convertBoolToInt(oCInfo.PrintStationCloseDateTime) +
            ", PrintEODDateTime = " + Configuration.convertBoolToInt(oCInfo.PrintEODDateTime) +
            #endregion

            #region PRIMEPOS-2562 27-Jul-2018 JY Added
            ", EnforceLowerCaseChar = " + Configuration.convertBoolToInt(oCInfo.EnforceLowerCaseChar) +
            ", EnforceUpperCaseChar = " + Configuration.convertBoolToInt(oCInfo.EnforceUpperCaseChar) +
            ", EnforceSpecialChar = " + Configuration.convertBoolToInt(oCInfo.EnforceSpecialChar) +
            ", EnforceNumber = " + Configuration.convertBoolToInt(oCInfo.EnforceNumber) +
            ", PasswordExpirationDays = " + Configuration.convertNullToInt(oCInfo.PasswordExpirationDays) +
            ", PasswordLength = " + Configuration.convertNullToInt(oCInfo.PasswordLength) +
            ", PasswordHistoryCount = " + Configuration.convertNullToInt(oCInfo.PasswordHistoryCount) +
            #endregion

            ", PromptForPartialPayment ='" + Configuration.convertBoolToInt(oCInfo.PromptForPartialPayment) + "'" + //PRIMEPOS-2499 27-Mar-2018 JY Added

            ", UseBiometricDevice = '" + Configuration.convertNullToString(oCInfo.UseBiometricDevice) + "'" +    //PRIMEPOS-2576 23-Aug-2018 JY Added
            ", ShowPaytypeDetails = " + Configuration.convertBoolToInt(oCInfo.ShowPaytypeDetails) +  //PRIMEPOS-2384 29-Oct-2018 JY Added             
            ", AuthenticationMode = " + Configuration.convertNullToInt(oCInfo.AuthenticationMode) +    //PRIMEPOS-2576 23-Aug-2018 JY Added
            ", PromptForItemPriceUpdate = " + Configuration.convertBoolToInt(oCInfo.PromptForItemPriceUpdate) +  //PRIMEPOS-2602 28-Jan-2019 JY Added
            #region S3 Setting - Solutran - Arvind / Nilesh PRIMEPOS-2663
            ", S3Enable = " + Configuration.convertBoolToInt(oCInfo.S3Enable) + // added by arvind
            ", S3Url = '" + Configuration.convertNullToString(oCInfo.S3Url) + "'" +
            ", S3Key = '" + Configuration.convertNullToString(oCInfo.S3Key) + "'" +
            ", S3Merchant = '" + Configuration.convertNullToString(oCInfo.S3Merchant) + "'" +
            #endregion

            ", HidePatientCounseling=" + Configuration.convertBoolToInt(oCInfo.HidePatientCounseling) +
            ", isPrimeDeliveryReconciliation = " + Configuration.convertBoolToInt(oCInfo.isPrimeDeliveryReconciliation) + // PRIMERX-7688 - 23-Sept-2019
            ", PatientCounceling = '" + Configuration.convertNullToString(oCInfo.PatientCounceling) + "'" + //PRIMEPOS-2846 15-May-2020 JY Added
            ", UserID ='" + Configuration.UserName + "'" + // PRIMEPOS-2808 Added by Arvind
            ", SSOIdentifier = " + Configuration.convertNullToInt(oCInfo.SSOIdentifier);  //PRIMEPOS-3484

            SqlParameter CurrencySymbol = new SqlParameter("@" + "CurrencySymbol", System.Data.SqlDbType.NVarChar);
            CurrencySymbol.Value = Configuration.convertNullToString(oCInfo.CurrencySymbol);
            cmd.Parameters.Add(CurrencySymbol);
            //modification End

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
        }

        #region PRIMEPOS-2676 21-May-2019 JY Seperate email settings update logic
        public void UpdateEmailsettings(IDbTransaction oTrans, CompanyInfo oCInfo)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            string sSQL = "UPDATE Util_Company_Info SET " +
                        " UseEmailInvoice ='" + Configuration.convertBoolToInt(oCInfo.UseEmailInvoice) + "'" +
                        ", OutGoingEmailPromptAutomatically  ='" + Configuration.convertBoolToInt(oCInfo.OutGoingEmailPromptAutomatically) + "'" +
                        ", OutGoingEmailServer ='" + Configuration.convertNullToString(oCInfo.OutGoingEmailServer) + "'" +
                        ", OutGoingEmailID ='" + Configuration.convertNullToString(oCInfo.OutGoingEmailID) + "'" +
                        ", OutGoingEmailUserID ='" + Configuration.convertNullToString(oCInfo.OutGoingEmailUserID) + "'" +
                        ", OutGoingEmailPort='" + Configuration.convertNullToString(oCInfo.OutGoingEmailPort) + "'" +
                        ", OutGoingEmailEnableSSL ='" + Configuration.convertBoolToInt(oCInfo.OutGoingEmailEnableSSL) + "'" +
                        ", AutoPopulateCustEmail = " + Configuration.convertBoolToInt(oCInfo.AutoPopulateCustEmail) + //Added By Shitaljit on 6/2/2104 for PRIMEPOS-1804 Auto Populate Email address from customer
                        ", OutGoingEmailPass='" + Configuration.convertNullToString(oCInfo.OutGoingEmailPass) + "'" +
                        ", OutGoingEmailBody  ='" + Configuration.convertNullToString(oCInfo.OutGoingEmailBody) + "'" +
                        ", OutGoingEmailSignature  ='" + Configuration.convertNullToString(oCInfo.OutGoingEmailSignature) + "'" +
                        ", AutoEmailStationCloseReport = " + Configuration.convertBoolToInt(oCInfo.AutoEmailStationCloseReport) +   //Sprint-24 - PRIMEPOS-2363 30-Jan-2017 JY Added
                        ", AutoEmailEODReport = " + Configuration.convertBoolToInt(oCInfo.AutoEmailEODReport) + //Sprint-24 - PRIMEPOS-2363 30-Jan-2017 JY Added
                        ", OwnersEmailId ='" + Configuration.convertNullToString(oCInfo.OwnersEmailId) + "'";  //Sprint-24 - PRIMEPOS-2363 28-Dec-2016 JY Added

            SqlParameter CurrencySymbol = new SqlParameter("@" + "CurrencySymbol", System.Data.SqlDbType.NVarChar);
            CurrencySymbol.Value = Configuration.convertNullToString(oCInfo.CurrencySymbol);
            cmd.Parameters.Add(CurrencySymbol);

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region PRIMEPOS-2739 ADDED BY ARVIND
        public void SaveSettingDetails(IDbTransaction oTrans, SettingDetail oSetting)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            //PRIMEPOS-2774 13-Jan-2020 JY Commented
            //string sSQL = "Update SettingDetail Set FieldValue = case FieldName when 'StrictReturn' then '" +
            //                      Configuration.convertBoolToInt(oSetting.StrictReturn) +                            
            //                    "' end " +
            //                    "where FieldName in('StrictReturn')";

            //PRIMEPOS-2774 13-Jan-2020 JY Added
            string sSQL = "UPDATE SettingDetail SET FieldValue = CASE " +
                            " WHEN FieldName = 'StrictReturn' THEN '" + Configuration.convertBoolToInt(oSetting.StrictReturn) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3FTPURL + "' THEN '" + Configuration.convertNullToString(oSetting.S3FTPURL) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3FTPPort + "' THEN '" + Configuration.convertNullToInt(oSetting.S3FTPPort) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3FTPUserId + "' THEN '" + Configuration.convertNullToString(oSetting.S3FTPUserId) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3FTPPassword + "' THEN '" + Configuration.convertNullToString(oSetting.S3FTPPassword) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3Frequency + "' THEN '" + Configuration.convertNullToInt(oSetting.S3Frequency) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3LastUploadDateOnFTP + "' THEN '" + Configuration.convertNullToString(oSetting.S3LastUploadDateOnFTP) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3FileName + "' THEN '" + Configuration.convertNullToString(oSetting.S3FileName) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3FTPFolderPath + "' THEN '" + Configuration.convertNullToString(oSetting.S3FTPFolderPath) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_S3FTP + "' THEN '" + Configuration.convertBoolToInt(oSetting.S3FTP) + "'" +
                            " WHEN FieldName = '" + "PromtManualEvertec" + "' THEN '" + Configuration.convertNullToString(oSetting.PromptForEvertecManual) + "'" +//PRIMEPOS-2805
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_TagSolutran + "' THEN '" + Configuration.convertBoolToInt(oSetting.TagSolutran) + "'" + //PRIMEPOS-2836 21-Apr-2020 JY Added
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_AutoSearchPrimeRxPatient + "' THEN '" + Configuration.convertBoolToInt(oSetting.AutoSearchPrimeRxPatient) + "'" + //PRIMEPOS-2845 14-May-2020 JY Added
                            " WHEN FieldName = '" + "OnlinePayment" + "' THEN '" + Configuration.convertNullToString(oSetting.OnlinePayment) + "'" +//PRIMEPOS-2841 added by Arvind
                            " WHEN FieldName = 'EnableCustomerEngagement' THEN '" + Configuration.convertBoolToInt(oSetting.EnableCustomerEngagement) + "'" + //PRIMEPOS-2794
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_ProceedROATransWithHCaccNotLinked + "' THEN '" + Configuration.convertBoolToInt(oSetting.ProceedROATransWithHCaccNotLinked) + "'" +   //PRIMEPOS-2570 17-Aug-2020 JY Added
                            " WHEN FieldName = '" + "PayProviderID" + "' THEN '" + Configuration.convertNullToString(oSetting.PayProviderID) + "'" +//PRIMEPOS-2902
                            " WHEN FieldName = 'MaskDrugName' THEN '" + Configuration.convertBoolToInt(oSetting.MaskDrugName) + "'" +//PRIMEPOS-3130
                            #region PRIMEPOS-3370
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NBSEnable + "' THEN '" + Configuration.convertBoolToInt(oSetting.NBSEnable) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NBSUrl + "' THEN '" + Configuration.convertNullToString(oSetting.NBSUrl) + "'" +
                            //" WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NBSToken + "' THEN '" + Configuration.convertNullToString(oSetting.NBSToken) + "'" + //PRIMEPOS-3412-Need-To-Change 
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NBSEntityID + "' THEN '" + Configuration.convertNullToString(oSetting.NBSEntityID) + "'" +
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NBSStoreID + "' THEN '" + Configuration.convertNullToString(oSetting.NBSStoreID) + "'" +
                            //" WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NBSTerminalID + "' THEN '" + Configuration.convertNullToString(oSetting.NBSTerminalID) + "'" +
            #endregion
                            " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_WaiveTransactionFee + "' THEN '" + Configuration.convertBoolToInt(oSetting.WaiveTransactionFee) + "'";  //PRIMEPOS-3234
            if (!string.IsNullOrWhiteSpace(oSetting.PayProviderName))//PRIMEPOS-2902
            {
                sSQL += " WHEN FieldName = '" + "PayProviderName" + "' THEN '" + Configuration.convertNullToString(oSetting.PayProviderName) + "'";
            }
            sSQL += " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_DefaultPaytype + "' THEN '" + Configuration.convertNullToString(oSetting.DefaultPaytype) + "'" + //PRIMEPOS-2512 02-Oct-2020 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_RestrictSignatureLineAndWordingOnReceipt + "' THEN '" + Configuration.convertBoolToInt(oSetting.RestrictSignatureLineAndWordingOnReceipt) + "'" +   //PRIMEPOS-2910 29-Oct-2020 JY Added
                    " WHEN FieldName = '" + "AllowMailOrder" + "' THEN '" + Configuration.convertNullToString(oSetting.AllowMailOrder) + "'" +//PRIMEPOS-2927
                    " WHEN FieldName = '" + "AllowZeroDollarShipCharge" + "' THEN '" + Configuration.convertNullToString(oSetting.AllowZeroShippingCharge) + "'" +//PRIMEPOS-2927
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_RxInsuranceToBeTaxed + "' THEN '" + Configuration.convertNullToString(oSetting.RxInsuranceToBeTaxed) + "'" +    //PRIMEPOS-2924 02-Dec-2020 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_PatientCounselingPrompt + "' THEN '" + Configuration.convertNullToString(oSetting.PatientCounselingPrompt) + "'" +  //PRIMEPOS-2461 01-Mar-2021 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_PrintCompanyLogo + "' THEN '" + Configuration.convertBoolToInt(oSetting.PrintCompanyLogo) + "'" + //PRIMEPOS-2386 26-Feb-2021 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_CompanyLogoFileName + "' THEN '" + Configuration.convertNullToString(oSetting.CompanyLogoFileName) + "'" + //PRIMEPOS-2386 26-Feb-2021 JY Added
                    " WHEN FieldName = '" + "LinkExpiryInMinutes" + "' THEN '" + Configuration.convertNullToString(oSetting.LinkExpriyInMinutes) + "'" +//PRIMEPOS-2915
                    " WHEN FieldName = '" + "OnlineOption" + "' THEN '" + Configuration.convertNullToString(oSetting.OnlineOption) + "'" +//PRIMEPOS-2915
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_SchedularMachine + "' THEN '" + Configuration.convertNullToString(oSetting.SchedularMachine) + "'" +  //PRIMEPOS-2485 19-Mar-2021 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_SchedulerUser + "' THEN '" + Configuration.convertNullToString(oSetting.SchedulerUser) + "'" +  //PRIMEPOS-2485 05-Apr-2021 JY Added
            #region PRIMEPOS-2999 09-Sep-2021 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NPlexURL + "' THEN '" + Configuration.convertNullToString(oSetting.NPlexURL) + "'" +
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NPlexTokenURL + "' THEN '" + Configuration.convertNullToString(oSetting.NPlexTokenURL) + "'" +
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NPlexClientID + "' THEN '" + Configuration.convertNullToString(oSetting.NPlexClientID) + "'" +
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NPlexClientSecret + "' THEN '" + Configuration.convertNullToString(oSetting.NPlexClientSecret) + "'" +
            #endregion
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_SiteID + "' THEN '" + Configuration.convertNullToString(oSetting.SiteId) + "'" +  //PRIMEPOS-2990 
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_LicenseID + "' THEN '" + Configuration.convertNullToString(oSetting.LicenseId) + "'" +
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_DeviceId + "' THEN '" + Configuration.convertNullToString(oSetting.DeviceId) + "'" +
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_DeveloperId + "' THEN '" + Configuration.convertNullToString(oSetting.DeveloperId) + "'" +
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_Username + "' THEN '" + Configuration.convertNullToString(oSetting.Username) + "'" +
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_Password + "' THEN '" + Configuration.convertNullToString(oSetting.Password) + "'" +
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_VersionNumber + "' THEN '" + Configuration.convertNullToString(oSetting.VersionNumber) + "'" +  //
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_ChainCode + "' THEN '" + Configuration.convertNullToString(oSetting.ChainCode) + "'" +  //3001
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_LocationName + "' THEN '" + Configuration.convertNullToString(oSetting.LocationName) + "'" +  //3001
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_TerminalID + "' THEN '" + Configuration.convertNullToString(oSetting.TerminalID) + "'" +  //3001
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_RxTaxPolicy + "' THEN '" + Configuration.convertNullToString(oSetting.RxTaxPolicy) + "'" +  //PRIMEPOS-3053 04-Feb-2022 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_NotifyRefrigeratedMedication + "' THEN '" + Configuration.convertBoolToInt(oSetting.NotifyRefrigeratedMedication) + "'" +   //PRIMEPOS-2651 08-Apr-2022 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_RestrictMultipleClockIn + "' THEN '" + Configuration.convertBoolToInt(oSetting.RestrictMultipleClockIn) + "'" +   //PRIMEPOS-2790 18-Apr-2022 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_TransactionFeeApplicableFor + "' THEN '" + Configuration.convertNullToString(oSetting.TransactionFeeApplicableFor) + "'" +   //PRIMEPOS-3115 11-Jul-2022 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_ResetPwdForceUserToChangePwd + "' THEN '" + Configuration.convertBoolToInt(oSetting.ResetPwdForceUserToChangePwd) + "'" +   //PRIMEPOS-3129 22-Aug-2022 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_PromptToSaveCCToken + "' THEN '" + Configuration.convertNullToString(oSetting.PromptToSaveCCToken) + "'" +  //PRIMEPOS-3145 28-Sep-2022 JY Added
                    " WHEN FieldName = '" + clsPOSDBConstants.SettingDetail_PatientsSubCategories + "' THEN '" + Configuration.convertNullToString(oSetting.PatientsSubCategories) + "'" +    //PRIMEPOS-3157 28-Nov-2022 JY Added
                    " ELSE FieldValue END ";

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();

            UpdateIsHideFlagForDefaultPayType(oTrans, oSetting.DefaultPaytype); //PRIMEPOS-2512 02-Oct-2020 JY Added
        }
        #endregion

        #region PRIMEPOS-2841
        public DataSet GetPrimeRxPayDetails(string fieldName)
        {
            SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM SettingDetail where  SettingID IN (select ID from Setting where Name  = @FieldName)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@FieldName", fieldName);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetPrimeRxPayDetails");
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }

        public void UpdatePrimeRxPayDetails(string fieldName, string fieldValue)
        {
            SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Update SettingDetail set FieldValue = @FieldValue where FieldName=@fieldName";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@FieldValue", fieldValue);
                cmd.Parameters.AddWithValue("@fieldName", fieldName);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdatePrimeRxPayDetails");
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region PRIMEPOS-2676 21-May-2019 JY Seperate Interface settings update logic
        public void UpdateInterfaceSettings(IDbTransaction oTrans, CompanyInfo oCInfo)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            string sSQL = "UPDATE Util_Company_Info SET " +
            #region Sprint-20 26-May-2015 Auto updater JY Added settings for auto updater
                    " AllowAutomaticUpdates = " + Configuration.convertBoolToInt(oCInfo.AllowAutomaticUpdates) +
                    ", AllowRunningUpdates = " + Configuration.convertBoolToInt(oCInfo.AllowRunningUpdates) +
                    ", AutoUpdateServiceAddress ='" + Configuration.convertNullToString(oCInfo.AutoUpdateServiceAddress) + "'" +
                    ", RunningTasksTimerInterval =" + Configuration.convertNullToInt(oCInfo.RunningTasksTimerInterval) +
            #endregion

            #region Sprint-23 - PRIMEPOS-2029 11-Apr-2016 JY Added
                    ", Nplex = " + Configuration.convertBoolToInt(oCInfo.useNplex) +
                    ", nplexStoreId = '" + Configuration.convertNullToString(oCInfo.nplexStoreId) + "'" +
                    ", StoreSiteId = '" + Configuration.convertNullToString(oCInfo.StoreSiteId) + "'" +
                    ", postSaleInd = " + Configuration.convertBoolToInt(oCInfo.postSaleInd) +
            #endregion

            #region PRIMEPOS-2643 05-Sep-2019
                     ",IsEnablePrimeInterface = " + Configuration.convertBoolToInt(oCInfo.PIEnable) +
                    ",PrimeInterfaceUrl = '" + Configuration.convertNullToString(oCInfo.PIURL) + "'" + //added by bhavesh
                    ",PrimeInterfaceUser = '" + Configuration.convertNullToString(oCInfo.PUser) + "'" +
                    ",PrimeInterfacePassword = '" + Configuration.convertNullToString(oCInfo.PPassword) + "'" +
            #endregion

                    ",EnableEPrimeRx = " + Configuration.convertBoolToInt(oCInfo.EnableEPrimeRx) +
                    ",EPrimeRxURL = '" + Configuration.convertNullToString(oCInfo.EPrimeRxURL) + "'" +
                    ",EPrimeRxToken = '" + Configuration.convertNullToString(oCInfo.EPrimeRxToken) + "'" +

            #region Sprint-24 - PRIMEPOS-2344 02-Dec-2016 JY Added
                    ", IIASFTPAddress = '" + Configuration.convertNullToString(oCInfo.IIASFTPAddress) + "'" +
                    ", IIASFTPUserId = '" + Configuration.convertNullToString(oCInfo.IIASFTPUserId) + "'" +
                    ", IIASFTPPassword = '" + Configuration.convertNullToString(oCInfo.IIASFTPPassword) + "'" +
                    ", IIASFileName = '" + Configuration.convertNullToString(oCInfo.IIASFileName) + "'" +
                    ", IIASDownloadInterval =" + Configuration.convertNullToInt(oCInfo.IIASDownloadInterval) +
                    ", IIASFileModifiedDateOnFTP ='" + oCInfo.IIASFileModifiedDateOnFTP + "'" +
            #endregion

            #region Sprint-24 - PRIMEPOS-2344 02-Dec-2016 JY Added
                    ", PSEFTPAddress = '" + Configuration.convertNullToString(oCInfo.PSEFTPAddress) + "'" +
                    ", PSEFTPUserId = '" + Configuration.convertNullToString(oCInfo.PSEFTPUserId) + "'" +
                    ", PSEFTPPassword = '" + Configuration.convertNullToString(oCInfo.PSEFTPPassword) + "'" +
                    ", PSEFileName = '" + Configuration.convertNullToString(oCInfo.PSEFileName) + "'" +
                    ", PSEDownloadInterval =" + Configuration.convertNullToInt(oCInfo.PSEDownloadInterval) +
                    ", PSEFileModifiedDateOnFTP ='" + oCInfo.PSEFileModifiedDateOnFTP + "'" +
                    ", UpdatePSEItem =" + Configuration.convertBoolToInt(oCInfo.UpdatePSEItem);
            #endregion

            SqlParameter CurrencySymbol = new SqlParameter("@" + "CurrencySymbol", System.Data.SqlDbType.NVarChar);
            CurrencySymbol.Value = Configuration.convertNullToString(oCInfo.CurrencySymbol);
            cmd.Parameters.Add(CurrencySymbol);

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();

            UpdateEPrimeRxAppSettings(oCInfo);
        }

        public bool UpdateEPrimeRxAppSettings(CompanyInfo oCInfo)
        {
            bool bSaveSetting = false;
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration("POS.exe");
            System.Configuration.AppSettingsSection App_Section = (System.Configuration.AppSettingsSection)config.GetSection("appSettings");
            System.Configuration.KeyValueConfigurationCollection app_settings = App_Section.Settings;

            string sValue1 = "SQL";
            if (oCInfo.EnableEPrimeRx)
                sValue1 = "WebServiceDB";

            System.Configuration.KeyValueConfigurationElement element1 = app_settings["DBTYPEPHARM"];
            if (element1 != null)
            {
                if (element1.Value != sValue1)
                {
                    bSaveSetting = true;
                    app_settings.Remove("DBTYPEPHARM");
                    element1.Value = sValue1;
                    app_settings.Add(element1);
                }
            }
            else
            {
                bSaveSetting = true;
                element1 = new System.Configuration.KeyValueConfigurationElement("DBTYPEPHARM", sValue1);
                app_settings.Add(element1);
            }

            if (!string.IsNullOrEmpty(oCInfo.EPrimeRxURL))
            {
                System.Configuration.KeyValueConfigurationElement element2 = app_settings["ePrimeRxURL"];
                if (element2 != null)
                {
                    if (element2.Value != oCInfo.EPrimeRxURL)
                    {
                        element2.Value = oCInfo.EPrimeRxURL;
                        app_settings.Remove("ePrimeRxURL");
                        bSaveSetting = true;
                        app_settings.Add(element2);
                    }
                }
                else
                {
                    element2 = new System.Configuration.KeyValueConfigurationElement("ePrimeRxURL", oCInfo.EPrimeRxURL);
                    bSaveSetting = true;
                    app_settings.Add(element2);
                }
            }

            if (!string.IsNullOrWhiteSpace(oCInfo.EPrimeRxToken))
            {
                System.Configuration.KeyValueConfigurationElement element3 = app_settings["ePrimeRxToken"];
                if (element3 != null)
                {
                    if (element3.Value != oCInfo.EPrimeRxToken)
                    {
                        bSaveSetting = true;
                        element3.Value = oCInfo.EPrimeRxToken;
                        app_settings.Remove("ePrimeRxToken");
                        app_settings.Add(element3);
                    }
                }
                else
                {
                    bSaveSetting = true;
                    element3 = new System.Configuration.KeyValueConfigurationElement("ePrimeRxToken", oCInfo.EPrimeRxToken);
                    app_settings.Add(element3);
                }
            }

            if (bSaveSetting)
                config.Save();

            return bSaveSetting;
        }

        #endregion

        #region PRIMEPOS-2676 21-May-2019 JY Seperate IVU Lotto settings update logic
        public void UpdateIVULottoSettings(IDbTransaction oTrans, CompanyInfo oCInfo, POSSET oPOSSet)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            string sSQL = "UPDATE Util_Company_Info SET " +
                    " UseIVULottoProgram ='" + Configuration.convertBoolToInt(oCInfo.UseIVULottoProgram) + "'" +
                    ", IVULottoMerchantID ='" + Configuration.convertNullToString(oCInfo.IVULottoMerchantID) + "'" +
                    ", IVULottoSetUpMode = '" + Configuration.convertNullToString(oCInfo.IVULottoCommunicationMode) + "'";

            SqlParameter CurrencySymbol = new SqlParameter("@" + "CurrencySymbol", System.Data.SqlDbType.NVarChar);
            CurrencySymbol.Value = Configuration.convertNullToString(oCInfo.CurrencySymbol);
            cmd.Parameters.Add(CurrencySymbol);

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();

            sSQL = "select stationid from Util_POSSet where stationid='" + Configuration.StationID + "'";
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            string result = (string)cmd.ExecuteScalar();

            if (result != null)
            {
                sSQL = "UPDATE Util_POSSet SET " +
                    " IVULottoTerminalID = '" + oPOSSet.IVULottoTerminalID + "'" +
                    ", IVULottoServerURL = '" + oPOSSet.IVULottoServerURL + "'" +
                    ", IVULottoPassword = '" + oPOSSet.IVULottoPassword + "'" +
                    "  where StationID='" + Configuration.StationID.Replace("'", "''") + "'";
                cmd.CommandText = sSQL;
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region PRIMEPOS-2676 21-May-2019 JY Messaging options settings update logic
        public void UpdateMessagingOptions(IDbTransaction oTrans, CompanyInfo oCInfo)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            string sSQL = "UPDATE Util_Company_Info SET " +
                    " CardExpAlert = " + Configuration.convertBoolToInt(oCInfo.CardExpAlert) +  //PRIMEPOS-2613 28-Dec-2018 JY Added
                    ", CardExpAlertDays = " + Configuration.convertNullToInt(oCInfo.CardExpAlertDays) + //PRIMEPOS-2613 28-Dec-2018 JY Added
                    ", CardExpEmail = " + Configuration.convertBoolToInt(oCInfo.CardExpEmail) +  //PRIMEPOS-2613 28-Dec-2018 JY Added
                    ", SPEmailFormatId = " + Configuration.convertNullToInt(oCInfo.SPEmailFormatId);    //PRIMEPOS-2613 28-Dec-2018 JY Added

            SqlParameter CurrencySymbol = new SqlParameter("@" + "CurrencySymbol", System.Data.SqlDbType.NVarChar);
            CurrencySymbol.Value = Configuration.convertNullToString(oCInfo.CurrencySymbol);
            cmd.Parameters.Add(CurrencySymbol);

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region PRIMEPOS-2676 21-May-2019 JY Seperate Consent Capture settings update logic
        public void UpdateConsentCapture(IDbTransaction oTrans, CompanyInfo oCInfo)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            string sSQL = "UPDATE Util_Company_Info SET " +
            #region PRIMEPOS-2442 ADDED BY ROHIT NAIR
            " EnableConsentCapture =" + Configuration.convertBoolToInt(oCInfo.EnableConsentCapture) +
            ", SelectedConsentSource = '" + Configuration.convertNullToString(oCInfo.SelectedConsentSource) + "'";
            #endregion

            SqlParameter CurrencySymbol = new SqlParameter("@" + "CurrencySymbol", System.Data.SqlDbType.NVarChar);
            CurrencySymbol.Value = Configuration.convertNullToString(oCInfo.CurrencySymbol);
            cmd.Parameters.Add(CurrencySymbol);

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region PRIMEPOS-2676 21-May-2019 JY Seperate Rx settings update logic
        public void UpdateRxSettings(IDbTransaction oTrans, CompanyInfo oCInfo, POSSET oPOSSet)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            string sSQL = "UPDATE Util_Company_Info SET " +
                    " UsePrimeESC = " + Configuration.convertBoolToInt(oCInfo.UsePrimeESC) +   //PRIMEPOS-2385 14-Mar-2019 JY Added
                    ", ALLOWVERIFIEDRXONLY =" + Configuration.convertNullToInt(oCInfo.AllowVerifiedRXOnly) + "" +   //PRIMEPOS-2593 23-Jun-2020 JY modified
                    ", ConfirmPatient=" + Configuration.convertNullToInt(oCInfo.ConfirmPatient) +   //PRIMEPOS-2317 15-Mar-2019 JY Added
                    ", RestrictIfDOBMismatch = " + Configuration.convertBoolToInt(oCInfo.RestrictIfDOBMismatch) +   //PRIMEPOS-2317 21-Mar-2019 JY Added
                    ", ShowPatientsData = " + Configuration.convertBoolToInt(oCInfo.ShowPatientsData) +   //PRIMEPOS-2317 21-Mar-2019 JY Added
                    ", ALLOWUNPICKEDRX=  " + oCInfo.AllowUnPickedRX + "" + //Added by shitaljit on 19 Mar 2012
                    ", UNPICKEDRXSEARCHDAYS= " + Configuration.convertNullToInt(oCInfo.UnPickedRXSearchDays) + "" + //Added by shitaljit on 19 Mar 2012                    
                    ", AllowMultipleRXRefillsInSameTrans =  " + Configuration.convertBoolToInt(oCInfo.AllowMultipleRXRefillsInSameTrans) + "" + //Added by shitaljit on 23 Mar 2012
                    ", AutoImportCustAtTrans =  " + Configuration.convertNullToInt(oCInfo.AutoImportCustAtTrans) + "" + //Added by shitaljit on 23 April 2012   //PRIMEPOS-2886 25-Sep-2020 JY modified
                    ", UpdatePatientData='" + Configuration.convertNullToString(oCInfo.UpdatePatientData) + "'" +//Added By shitaljit on 19 July 2013. PRIMEPOS-1235 Add Preference to control Updating patient data from PrimeRX during transaction.
                    ", WarnForRXDelivery = " + Configuration.convertBoolToInt(oCInfo.WarnForRXDelivery) +//Added By Shitaljit on 6/2/2104 for PRIMEPOS-1816 Ability to turn on\off delivery prompt
                    ", PreventRxMaxFillDayLimit =  " + Configuration.convertNullToInt(oCInfo.PreventRxMaxFillDayLimit) +
                    ", PreventRxMaxFillDayNotifyAction =  " + Configuration.convertNullToInt(oCInfo.PreventRxMaxFillDayNotifyAction) +
                    ", SearchRxsWithPatientName = " + Configuration.convertBoolToInt(oCInfo.SearchRxsWithPatientName) +  //Sprint-23 - PRIMEPOS-2276 06-Jun-2016 JY Added 
                    ", FetchFamilyRx = " + Configuration.convertBoolToInt(oCInfo.FetchFamilyRx) +   //Sprint-25 - PRIMEPOS-2322 31-Jan-2017 JY Added
                    ", WarnMultiPatientRX='" + Configuration.convertBoolToInt(oCInfo.WarnMultiPatientRX) + "'" +//Added By shitaljit on 3/7/2103
                    ", IgnoreFutureRx = " + Configuration.convertBoolToInt(oCInfo.IgnoreFutureRx) + //PRIMEPOS-2591 25-Oct-2018 JY Added

            #region PrimePOS-2448 Added BY Rohit Nair
                    ", EnableIntakeBatch =" + Configuration.convertBoolToInt(oCInfo.EnableIntakeBatch) +
                    ", IntakeBatchCode = '" + Configuration.convertNullToString(oCInfo.IntakeBatchCode) + "'" +
                    ", IntakeBatchStatus = '" + Configuration.convertNullToString(oCInfo.IntakeBatchStatus) + "'" + //PrimePOS-2518 Jenny Added
                    ", SkipSignatureForInatkeBatch =" + Configuration.convertBoolToInt(oCInfo.SkipSignatureForInatkeBatch) +
                    ", IntakeBatchMarkAsPickedup =" + Configuration.convertBoolToInt(oCInfo.IntakeBatchMarkAsPickedup);
            #endregion

            SqlParameter CurrencySymbol = new SqlParameter("@" + "CurrencySymbol", System.Data.SqlDbType.NVarChar);
            CurrencySymbol.Value = Configuration.convertNullToString(oCInfo.CurrencySymbol);
            cmd.Parameters.Add(CurrencySymbol);

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();

            sSQL = "select stationid from Util_POSSet where stationid='" + Configuration.StationID + "'";
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            string result = (string)cmd.ExecuteScalar();

            if (result != null)
            {
                sSQL = "UPDATE Util_POSSet SET " +
                    " RXCode='" + oPOSSet.RXCode.Replace("'", "''") + "'" +
                    ", PrintRxDescription=" + Configuration.convertBoolToInt(oPOSSet.PrintRXDescription) +
                    ", UsePrimeRX=" + Configuration.convertBoolToInt(oPOSSet.UsePrimeRX) +
                    ", DisableNOPP=" + Configuration.convertBoolToInt(oPOSSet.DisableNOPP) +
                    //Added by SRT(Abhishek) Date : 17 Aug 2009
                    ", FetchUnbilledRx=" + Configuration.convertNullToInt(oPOSSet.FetchUnbilledRx) + "" +  //Added By SRT(Abhishek) Date: 17-Aug-2009   //PRIMEPOS-2398 04-Jan-2021 JY modified
                                                                                                           //End of Added by SRT(Abhishek) Date : 17 Aug 2009
                    ", AllowRxPicked=" + Configuration.convertNullToInt(oPOSSet.AllowRxPicked) + "" +   //PRIMEPOS-2865 15-Jul-2020 JY modified
                    ", AllowPickedUpRx=" + Configuration.convertBoolToInt(oPOSSet.AllowPickedUpRxToTrans) + "" + //Added BY Manoj 1/24/2013
                    ", SkipDeliverySign=" + Configuration.convertBoolToInt(oPOSSet.SkipDelSign) + "" + //Added by Manoj 5/8/2013
                    ", RXInsToIgnoreCopay='" + oPOSSet.RXInsToIgnoreCopay.Replace("'", "''") + "'" +
                    ", ControlByID=" + Configuration.convertNullToInt(oPOSSet.ControlByID) + "" + //Added by Manoj 4/2/2013 //PRIMEPOS-2547 03-Jul-2018 JY Modified
                    ", AskVerificationIdMode =" + Configuration.convertNullToInt(oPOSSet.AskVerificationIdMode) +   //PRIMEPOS-2547 11-Jul-2018 JY Added                                        
                    ", ShowRxNotes = " + Configuration.convertBoolToInt(oPOSSet.ShowRxNotes) +  //PRIMEPOS-2459 03-Apr-2019 JY Added
                    ", ShowPatientNotes = " + Configuration.convertBoolToInt(oPOSSet.ShowPatientNotes) +  //PRIMEPOS-2459 03-Apr-2019 JY Added
                    "  where StationID='" + Configuration.StationID.Replace("'", "''") + "'";
                cmd.CommandText = sSQL;
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region PRIMEPOS-2676 21-May-2019 JY Seperate PrimePO settings update logic //PRIMEPOS-3167 07-Nov-2022 JY Commented
        //public void UpdatePrimePOSettings(IDbTransaction oTrans, CompanyInfo oCInfo, POSSET oPOSSet)
        //{
        //    string sSQL = "select stationid from Util_POSSet where stationid='" + Configuration.StationID + "'";
        //    IDbCommand cmd = DataFactory.CreateCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = sSQL;
        //    cmd.Transaction = oTrans;
        //    cmd.Connection = oTrans.Connection;
        //    string result = "";
        //    result = (string)cmd.ExecuteScalar();

        //    if (result != null)
        //    {
        //        sSQL = "UPDATE Util_POSSet SET " +
        //            " UsePrimePO=" + Configuration.convertBoolToInt(oPOSSet.USePrimePO) + "" +   //Added By Dharmendra on Apr-29-09
        //            ",HostAddress='" + oPOSSet.HostAddress.Replace("'", "''") + "'" +
        //            ",ConnectionTimer=" + Configuration.convertNullToInt(oPOSSet.ConnectionTimer.ToString()) + "" +
        //            ",PurchaseOrderTimer=" + Configuration.convertNullToInt(oPOSSet.PurchaseOrdTimer.ToString()) + "" +
        //            ",PriceUpdateTimer=" + Configuration.convertNullToInt(oPOSSet.PriceUpdateTimer.ToString()) + "" +
        //            ", RemoteURL='" + oPOSSet.RemoteURL + "'" +
        //            ", ConsiderReturnTrans = " + Configuration.convertBoolToInt(oPOSSet.ConsiderReturnTrans) +  //Sprint-27 - PRIMEPOS-2390 28-Sep-2017 JY Added                                         
        //            ", UpdateVendorPrice=" + Configuration.convertBoolToInt(oPOSSet.UpdateVendorPrice) +
        //            ", UpdateDescription=" + Configuration.convertBoolToInt(oPOSSet.UpdateDescription) +//Added By Abhishek Jun-26-2009
        //            ", INSERT11DIGITITEM =" + Configuration.convertBoolToInt(oPOSSet.Insert11DigitItem) + "" +  //Added By SRT(Abhishek D) Date: 23 Feb 2010
        //            ", IgnoreVendorSequence=" + Configuration.convertBoolToInt(oPOSSet.IgnoreVendorSequence) + "" +
        //            ", DEFAULTVENDOR='" + oPOSSet.DefaultVendor.ToString() + "'" +
        //            ", USEDEFAULTVENDOR=" + Configuration.convertBoolToInt(oPOSSet.UseDefaultVendor) +
        //            ", AutoPOSeq='" + oPOSSet.AutoPOSeq.Trim() + "'" +
        //            "  where StationID='" + Configuration.StationID.Replace("'", "''") + "'";
        //        cmd.CommandText = sSQL;
        //        cmd.ExecuteNonQuery();
        //    }
        //}
        #endregion

        #region PRIMEPOS-2676 21-May-2019 JY Seperate Transaction settings update logic
        public void UpdateTransSettings(IDbTransaction oTrans, CompanyInfo oCInfo, POSSET oPOSSet)
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oTrans.Connection;
            cmd.Transaction = oTrans;

            string sSQL = "UPDATE Util_Company_Info SET " +
                    " RestrictFSACardMessage = " + Configuration.convertBoolToInt(oCInfo.RestrictFSACardMessage) +  //PRIMEPOS-2621 17-Dec-2018 JY Added
                    ", EBTAsManualTrans =  " + Configuration.convertBoolToInt(oCInfo.EBTAsManualTrans) + "" + //Added by shitaljit on 23 Mar 2012                    
                    ", SaveCCToken = " + Configuration.convertBoolToInt(oCInfo.SaveCCToken) +  //Sprint-23 - PRIMEPOS-2313 09-Jun-2016 JY Added
                    ", DefaultCustomerTokenValue = " + Configuration.convertBoolToInt(oCInfo.DefaultCustomerTokenValue) +   //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added
                    ", ApplyInvDiscSettingsForCoupon =" + Configuration.convertBoolToInt(oCInfo.ApplyInvDiscSettingsForCoupon) + "" +//Added by Shitaljit for PRIMEPOS-1652 Add preference to manage Promotional coupon discount to abide with discount settings
                    ", AllowDiscountOfItemsOnSale= " + Configuration.convertBoolToInt(oCInfo.AllowDiscountOfItemsOnSale) +
                    ", AllowHundredPerInvDiscount=" + Configuration.convertBoolToInt(oCInfo.AllowHundredPerInvDiscount) + "" +//Added By Shitaljit(QuicSolv) on 7 dec 2011
                    ", InvDiscToDiscountableItemOnly= " + Configuration.convertBoolToInt(oCInfo.InvDiscToDiscountableItemOnly); //Added By Shitaljit(QuicSolv) on 27 dec 2011

            SqlParameter CurrencySymbol = new SqlParameter("@" + "CurrencySymbol", System.Data.SqlDbType.NVarChar);
            CurrencySymbol.Value = Configuration.convertNullToString(oCInfo.CurrencySymbol);
            cmd.Parameters.Add(CurrencySymbol);

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();

            sSQL = "select stationid from Util_POSSet where stationid='" + Configuration.StationID + "'";
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            string result = (string)cmd.ExecuteScalar();

            if (result != null)
            {
                sSQL = "UPDATE Util_POSSet SET " +
                    " ONLINECCT=" + Configuration.convertBoolToInt(oPOSSet.ONLINECCT) + "" +
                    ", MergeCCWithRcpt=" + Configuration.convertBoolToInt(oPOSSet.MergeCCWithRcpt) +
                    ",AvsMode='" + oPOSSet.AvsMode.Replace("'", "''") + "'" +
                    ",ProcessOnLine=" + Configuration.convertBoolToInt(oPOSSet.ProcessOnLine) + //Added By Dharmendra (SRT) Nov-03-08 to make ProcessOnLine configurable                    
                    ",ShowAuthorization=" + Configuration.convertBoolToInt(oPOSSet.ShowAuthorization) + "" +   //Added By Dharmendra on May-13-09
                    ", PreferReverse=" + Configuration.convertBoolToInt(oPOSSet.PreferReverse) + "" +
                    ", procFSAwithXCharge=" + Configuration.convertBoolToInt(oPOSSet.procFSAWithXLINK) +   //Added By SRT(Gaurav) Date: 21-Jul-2009                
                    ", DispSigOnTrans=" + Configuration.convertBoolToInt(oPOSSet.DispSigOnTrans) +
                    ", AllowManualCCTrans=" + Configuration.convertBoolToInt(oPOSSet.AllowManualCCTrans) +
                    ",DispSigOnHouseCharge=" + Configuration.convertBoolToInt(oPOSSet.DispSigOnHouseCharge) + "" + //Added by Manoj 11/21/2011

                    //Added by SRT(Ritesh Parekh) Date : 20 Aug 2009
                    ", CaptureSigForDebit=" + Configuration.convertBoolToInt(oPOSSet.CaptureSigForDebit) + "" +  //Added By SRT(Ritesh Parekh) Date: 20-Aug-2009
                                                                                                                 //End of Added by SRT(Ritesh Parekh) Date : 20 Aug 2009

                    ", CaptureSigForEBT =" + Configuration.convertBoolToInt(oPOSSet.CaptureSigForEBT) + "" + //Added by Manoj 7/2/2013
                    ", ALLOWDUP=" + Configuration.convertBoolToInt(oPOSSet.ALLOWDUP) + "" +
                    ",PaymentProcessor='" + oPOSSet.PaymentProcessor.Replace("'", "''") + "'" +
                    ", HPS_USERNAME='" + oPOSSet.HPS_USERNAME + "'" +
                    ", HPS_PASSWORD='" + oPOSSet.HPS_PASSWORD + "'" +
                    ", TerminalID = '" + oPOSSet.TerminalID + "'" + //PrimePOS-2491 
                    ",TxnTimeOut='" + oPOSSet.TxnTimeOut.Replace("'", "''") + "'" +
                    ",HeartBeatTime='" + oPOSSet.HeartBeatTime.Replace("'", "''") + "'" +
                    ",SkipF10Sign=" + Configuration.convertBoolToInt(oPOSSet.SkipF10Sign) + "" + //Added by Manoj 9/26/2013
                    ",SkipAmountSign=" + Configuration.convertBoolToInt(oPOSSet.SkipAmountSign) + "" + //Added by Manoj 9/26/2013                    
                    ", SkipEMVCardSign=" + Configuration.convertBoolToInt(oPOSSet.SkipEMVCardSign) +
                    ", AllItemDisc=" + oPOSSet.AllItemDisc + //Added By Shitaljit(QuicSolv) 7 Sept 2011                    
                    "  where StationID='" + Configuration.StationID.Replace("'", "''") + "'";

                cmd.CommandText = sSQL;
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        public DataSet PopulateKeyDiscount(string StationID)
        {
            SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);
            DataSet DtKeyDiscount = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select StationID,KeyA,KeyB,KeyC,KeyD,KeyE from Util_POSSET where StationID=@StationID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@StationID", StationID);

            SqlDataAdapter daKeyDis = new SqlDataAdapter(cmd);
            daKeyDis.Fill(DtKeyDiscount);
            daKeyDis.FillSchema(DtKeyDiscount, SchemaType.Source);
            return DtKeyDiscount;
        }

        public void UpdateKeyDiscount(string StationID, Decimal A, Decimal B, Decimal C, Decimal D, Decimal E)
        {
            SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);
            DataSet DtKeyDiscount = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Update Util_POSSET set KeyA =@KeyA ,KeyB=@KeyB , KeyC=@KeyC , KeyD=@KeyD , KeyE=@KeyE where StationID=@StationID";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@StationID", StationID);
            cmd.Parameters.AddWithValue("@KeyA", A);
            cmd.Parameters.AddWithValue("@KeyB", B);
            cmd.Parameters.AddWithValue("@KeyC", C);
            cmd.Parameters.AddWithValue("@KeyD", D);
            cmd.Parameters.AddWithValue("@KeyE", E);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public void UpdateCustomerLoyaltyInfo(IDbTransaction oTrans, CustomerLoyaltyInfo oCLInfo)
        {
            //IDbConnection conn;
            //conn=DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            string sSQL = string.Empty; ;
            //= "select 1 from CL_Setup ";
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //cmd.CommandText = sSQL;
            cmd.Transaction = oTrans;
            cmd.Connection = oTrans.Connection;
            sSQL = @" DELETE FROM CL_SetUp ";
            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();
            /*object result;
            result = cmd.ExecuteScalar();
            sSQL = "";

             *
            if(result == null || Configuration.convertNullToInt(result) == 0)
            {*/
            sSQL = "INSERT INTO CL_Setup ( " +
                    " UseCustomerLoyalty " +
                    ", ProgramName " +
                    ", CardRangeFrom " +
                    ", CardRangeTo " +
                    ", DefaultCardExpiryDays " +
                    ", IsCardPrepetual" +
                    ", PrintCopounWithReceipt " +
                    ", ExcludeItemsOnSale " +
                    ", Message " +
                    ", PrintMsgOnReceipt " +
                    ", RedeemValue " +
                    ", RedeemMethod " +
                    ", ShowDiscountAppliedMsg " +
                    ", AllowMultipleCouponsInTrans " +
                    ", IsTierValueInPercent " +
                    ", ShowCLCardInputOnTrans " +
                    ", PointsCalcMethod " +
                    ", IncludeItemsWithType " +
                    ", ShowCLControlPane " +
                    ", DisableAutoPointCalc " +
                    ", DoNotGenerateCoupons " +
                    ", ExcludeDiscountableItems " +
                    ", ApplyCLOrCusDisc " +
                    ", CreatedOn " +
                    ", SingleCouponPerRewardTier " +
                    ", PrintCLCouponOnlyIfTierIsReached " +
                    ", ApplyDiscountOnlyIfTierIsReached " + //Sprint-25 - PRIMEPOS-2297 21-Feb-2017 JY Added
                " ) Values ( " +
                    "" + Configuration.convertBoolToInt(oCLInfo.UseCustomerLoyalty) + "" +
                    ", '" + oCLInfo.ProgramName + "'" +
                    "," + oCLInfo.CardRangeFrom.ToString() + "" +
                    "," + oCLInfo.CardRangeTo.ToString() + "" +
                    ",'" + oCLInfo.DefaultCardExpiryDays + "'" +
                    "," + Configuration.convertBoolToInt(oCLInfo.IsCardPrepetual) + "" +
                    "," + Configuration.convertBoolToInt(oCLInfo.PrintCLCouponSeparately) +
                    "," + Configuration.convertBoolToInt(oCLInfo.ExcludeItemsOnSale) +
                    ",'" + oCLInfo.Message.Replace("'", "''") + "'" +
                    "," + Configuration.convertBoolToInt(oCLInfo.PrintMsgOnReceipt) +
                    "," + oCLInfo.RedeemValue +
                    "," + oCLInfo.RedeemMethod +
                    "," + Configuration.convertBoolToInt(oCLInfo.ShowDiscountAppliedMsg) +
                    "," + Configuration.convertBoolToInt(oCLInfo.AllowMultipleCouponsInTrans) +
                    "," + Configuration.convertBoolToInt(oCLInfo.IsTierValueInPercent) +
                    "," + Configuration.convertBoolToInt(oCLInfo.ShowCLCardInputOnTrans) +
                    ",'" + oCLInfo.PointsCalcMethod + "'" +
                    ",'" + oCLInfo.IncludeItemsWithType + "'" +
                    "," + Configuration.convertBoolToInt(oCLInfo.ShowCLControlPane) +
                    "," + Configuration.convertBoolToInt(oCLInfo.DisableAutoPointCalc) +
                    "," + Configuration.convertBoolToInt(oCLInfo.DoNotGenerateCoupons) +
                    "," + Configuration.convertBoolToInt(oCLInfo.ExcludeDiscountableItems) +
                    "," + Configuration.convertBoolToInt(oCLInfo.ApplyCLOrCusDisc) +
                    ", getdate()" +
                     "," + Configuration.convertBoolToInt(oCLInfo.SingleCouponPerRewardTier) +
                     "," + Configuration.convertBoolToInt(oCLInfo.PrintCLCouponOnlyIfTierIsReached) +
                     "," + Configuration.convertBoolToInt(oCLInfo.ApplyDiscountOnlyIfTierIsReached) +   //Sprint-25 - PRIMEPOS-2297 21-Feb-2017 JY Added
                    ")"; //Added by Prog1
            /*}
            else
            {
                sSQL = "UPDATE CL_Setup SET " +
                    " UseCustomerLoyalty=" + Configuration.convertBoolToInt(oCLInfo.UseCustomerLoyalty) +
                    ",ProgramName ='" + oCLInfo.ProgramName + "'" +
                    ",CardRangeFrom = " + Configuration.convertNullToInt64(oCLInfo.CardRangeFrom.ToString()) + "" +
                    ",CardRangeTo =" + Configuration.convertNullToInt64(oCLInfo.CardRangeTo.ToString()) + "" +
                    ",DefaultCardExpiryDays =" + oCLInfo.DefaultCardExpiryDays +
                    ",IsCardPrepetual=" + Configuration.convertBoolToInt(oCLInfo.IsCardPrepetual) + "" +
                    ",PrintCopounWithReceipt=" + Configuration.convertBoolToInt(oCLInfo.PrintCopounWithReceipt) +
                    ",ExcludeItemsOnSale=" + Configuration.convertBoolToInt(oCLInfo.ExcludeItemsOnSale) +
                    ",Message='" + oCLInfo.Message.Replace("'", "''") + "'" +
                    ",PrintMsgOnReceipt=" + Configuration.convertBoolToInt(oCLInfo.PrintMsgOnReceipt) +
                    ",RedeemValue=" + oCLInfo.RedeemValue +
                    ",ShowDiscountAppliedMsg=" + Configuration.convertBoolToInt(oCLInfo.ShowDiscountAppliedMsg) +
                    ",AllowMultipleCouponsInTrans=" + Configuration.convertBoolToInt(oCLInfo.AllowMultipleCouponsInTrans) +
                    ",IsTierValueInPercent=" + Configuration.convertBoolToInt(oCLInfo.IsTierValueInPercent) +
                    ",ShowCLCardInputOnTrans=" + Configuration.convertBoolToInt(oCLInfo.ShowCLCardInputOnTrans) +
                    ",IncludeItemsWithType='" + oCLInfo.IncludeItemsWithType + "'" +
                    ",PointsCalcMethod='" + oCLInfo.PointsCalcMethod + "'" +
                    ",RedeemMethod=" + oCLInfo.RedeemMethod +
                    ",ShowCLControlPane=" + Configuration.convertBoolToInt(oCLInfo.ShowCLControlPane) +
                    ",DisableAutoPointCalc=" + Configuration.convertBoolToInt(oCLInfo.DisableAutoPointCalc) +
                    ",DoNotGenerateCoupons=" + Configuration.convertBoolToInt(oCLInfo.DoNotGenerateCoupons) +
                    ",ExcludeDiscountableItems=" + Configuration.convertBoolToInt(oCLInfo.ExcludeDiscountableItems)+
                    ",ExcludeNonDiscountableItems=" + Configuration.convertBoolToInt(oCLInfo.ExcludeNonDiscountableItems);
            }*/

            cmd.CommandText = sSQL;
            cmd.ExecuteNonQuery();

            UpdateExcludeCLData(oTrans, oCLInfo.ExcludeDepts, clsPOSDBConstants.Department_tbl, clsPOSDBConstants.Department_Fld_DeptID, false);
            UpdateExcludeCLData(oTrans, oCLInfo.ExcludeSubDepts, clsPOSDBConstants.SubDepartment_tbl, clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID, false);
            UpdateExcludeCLData(oTrans, oCLInfo.ExcludeItems, clsPOSDBConstants.Item_tbl, clsPOSDBConstants.Item_Fld_ItemID, true);

            UpdateExcludeCLCouponData(oTrans, oCLInfo.ExcludeClCouponItems, clsPOSDBConstants.Item_tbl, clsPOSDBConstants.Item_Fld_ItemID, true);
            UpdateExcludeCLCouponData(oTrans, oCLInfo.ExcludeClCouponDepts, clsPOSDBConstants.Department_tbl, clsPOSDBConstants.Department_Fld_DeptID, false);
            UpdateExcludeCLCouponData(oTrans, oCLInfo.ExcludeClCouponSubDepts, clsPOSDBConstants.SubDepartment_tbl, clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID, false);
        }

        public void UpdateExcludeCLData(IDbTransaction oTrans, ExcludeData excludeData, String tableName, String keyColumn, bool encloseKeyInQuotes)
        {
            if (excludeData.IsDataChanged == false) return;
            string sSQL = "update " + tableName + " set ExcludeFromCL=0 where ExcludeFromCL=1 ";
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = oTrans;
            cmd.Connection = oTrans.Connection;
            object result;
            result = cmd.ExecuteNonQuery();

            List<String> codes = excludeData.Data;
            if (codes.Count > 0)
            {
                int toRange = 0;
                string inClause;

                for (int i = 0; i < codes.Count; i = i + 500)
                {
                    toRange = i + 500;
                    if (toRange > codes.Count)
                    {
                        toRange = codes.Count;
                    }

                    if (encloseKeyInQuotes)
                    {
                        inClause = "'" + String.Join("','", codes.GetRange(i, toRange).ToArray()) + "'";
                    }
                    else
                    {
                        inClause = String.Join(",", codes.GetRange(i, toRange).ToArray());
                    }

                    sSQL = "UPDATE " + tableName +
                            " SET ExcludeFromCL=1 where " + keyColumn + " in (" + inClause + ")";

                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateExcludeCLCouponData(IDbTransaction oTrans, ExcludeData excludeData, String tableName, String keyColumn, bool encloseKeyInQuotes)
        {
            string sSQL = "update " + tableName + " set EXCLUDEFROMCLCouponPay=0 where EXCLUDEFROMCLCouponPay=1 ";
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = oTrans;
            cmd.Connection = oTrans.Connection;
            object result;
            result = cmd.ExecuteNonQuery();

            List<String> codes = excludeData.Data;
            if (codes.Count > 0)
            {
                int toRange = 0;
                string inClause;

                for (int i = 0; i < codes.Count; i = i + 500)
                {
                    toRange = i + 500;
                    if (toRange > codes.Count)
                    {
                        toRange = codes.Count;
                    }

                    if (encloseKeyInQuotes)
                    {
                        inClause = "'" + String.Join("','", codes.GetRange(i, toRange).ToArray()) + "'";
                    }
                    else
                    {
                        inClause = String.Join(",", codes.GetRange(i, toRange).ToArray());
                    }

                    sSQL = "UPDATE " + tableName +
                            " SET EXCLUDEFROMCLCouponPay=1 where " + keyColumn + " in (" + inClause + ")";

                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #region Sprint-20 15-Jun-2015 JY need to save NABP into StoreId
        public void UpdateStoreId(IDbTransaction oTrans, String StoreId)
        {
            string sSQL = "UPDATE Util_Company_Info SET StoreID = '" + StoreId + "'";
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = oTrans;
            cmd.Connection = oTrans.Connection;
            object result;
            result = cmd.ExecuteNonQuery();
        }
        #endregion

        #region PRIMEPOS-2667 12-Apr-2019 JY Added
        public void UpdatePHNPINO(IDbTransaction oTrans, String PHNPINO)
        {
            string sSQL = "UPDATE Util_Company_Info SET PHNPINO = '" + PHNPINO + "'";
            IDbCommand cmd = DataFactory.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sSQL;
            cmd.Transaction = oTrans;
            cmd.Connection = oTrans.Connection;
            object result;
            result = cmd.ExecuteNonQuery();
        }
        #endregion

        #region PRIMEPOS-2227 05-May-2017 JY Added for merchant config settings
        public void UpdateMerchantConfig(IDbTransaction oTrans, MerchantConfig oMerchantConfig)
        {
            string strSQL = "SELECT * FROM MerchantConfig";
            DataSet ds = DataHelper.ExecuteDataset(oTrans, CommandType.Text, strSQL);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if ((Configuration.convertNullToString(oMerchantConfig.User_ID).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["User_ID"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.Merchant).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["Merchant"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.Processor_ID).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["Processor_ID"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.Payment_Server).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["Payment_Server"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.Port_No).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["Port_No"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.Payment_Client).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["Payment_Client"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.Payment_ResultFile).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["Payment_ResultFile"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.Application_Name).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["Application_Name"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.XCClientUITitle).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["XCClientUITitle"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.LicenseID).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["LicenseID"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.SiteID).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["SiteID"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.DeviceID).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["DeviceID"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.URL).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["URL"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.VCBin).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["VCBin"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.MCBin).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["MCBin"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.VantivAccountUrl).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["PaymentAccountAPI"]).Trim().ToUpper())//PRIMEPOS-TOKENURL
                    || (Configuration.convertNullToString(oMerchantConfig.VantivTokenUrl).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["CreditCardAPI"]).Trim().ToUpper())
                    || (Configuration.convertNullToString(oMerchantConfig.VantivReportUrl).Trim().ToUpper() != Configuration.convertNullToString(ds.Tables[0].Rows[0]["VantiveReportApi"]).Trim().ToUpper())//PRIMEPOS-3156
                    )
                {
                    strSQL = "UPDATE MerchantConfig SET " +
                    " User_ID = '" + oMerchantConfig.User_ID.Trim().Replace("'", "''") + "'," +
                    " Merchant = '" + oMerchantConfig.Merchant.Trim().Replace("'", "''") + "'," +
                    " Processor_ID = '" + oMerchantConfig.Processor_ID.Trim().Replace("'", "''") + "'," +
                    " Payment_Server = '" + oMerchantConfig.Payment_Server.Trim().Replace("'", "''") + "'," +
                    " Port_No = '" + oMerchantConfig.Port_No.Trim().Replace("'", "''") + "'," +
                    " Payment_Client = '" + oMerchantConfig.Payment_Client.Trim().Replace("'", "''") + "'," +
                    " Payment_ResultFile = '" + oMerchantConfig.Payment_ResultFile.Trim().Replace("'", "''") + "'," +
                    " Application_Name = '" + oMerchantConfig.Application_Name.Trim().Replace("'", "''") + "'," +
                    " XCClientUITitle = '" + oMerchantConfig.XCClientUITitle.Trim().Replace("'", "''") + "'," +
                    " LicenseID = '" + oMerchantConfig.LicenseID.Trim().Replace("'", "''") + "'," +
                    " SiteID = '" + oMerchantConfig.SiteID.Trim().Replace("'", "''") + "'," +
                    " DeviceID = '" + oMerchantConfig.DeviceID.Trim().Replace("'", "''") + "'," +
                    " URL = '" + oMerchantConfig.URL.Trim().Replace("'", "''") + "'," +
                    " VCBin = '" + oMerchantConfig.VCBin.Trim().Replace("'", "''") + "'," +
                    " MCBin = '" + oMerchantConfig.MCBin.Trim().Replace("'", "''") + "'," +
                    " PaymentAccountAPI = '" + oMerchantConfig.VantivAccountUrl.Trim().Replace("'", "''") + "'," +//PRIMEPOS-TOKENURL
                    " CreditCardAPI = '" + oMerchantConfig.VantivTokenUrl.Trim().Replace("'", "''") + "'," + //PRIMEPOS-3156
                    " VantiveReportApi = '" + oMerchantConfig.VantivReportUrl.Trim().Replace("'", "''") + "'"; //PRIMEPOS-3156

                    IDbCommand cmd = DataFactory.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = strSQL;
                    cmd.Transaction = oTrans;
                    cmd.Connection = oTrans.Connection;
                    int nRowsAffected = cmd.ExecuteNonQuery();
                }
            }
            else
            {
                //PRIMEPOS-2636 ARVIND VANTIV
                strSQL = "INSERT INTO MerchantConfig (User_ID, Merchant, Processor_ID, Payment_Server, Port_No, Payment_Client, Payment_ResultFile, Application_Name, XCClientUITitle, LicenseID, SiteID, DeviceID, URL, VCBin, MCBin) VALUES ('" +
                    oMerchantConfig.User_ID.Replace("'", "''") + "','" +
                    oMerchantConfig.Merchant.Replace("'", "''") + "','" +
                    oMerchantConfig.Processor_ID.Replace("'", "''") + "','" +
                    oMerchantConfig.Payment_Server.Replace("'", "''") + "','" +
                    oMerchantConfig.Port_No.Replace("'", "''") + "','" +
                    oMerchantConfig.Payment_Client.Replace("'", "''") + "','" +
                    oMerchantConfig.Payment_ResultFile.Replace("'", "''") + "','" +
                    oMerchantConfig.Application_Name.Replace("'", "''") + "','" +
                    oMerchantConfig.XCClientUITitle.Replace("'", "''") + "','" +
                    oMerchantConfig.LicenseID.Replace("'", "''") + "','" +
                    oMerchantConfig.SiteID.Replace("'", "''") + "','" +
                    oMerchantConfig.DeviceID.Replace("'", "''") + "','" +
                    oMerchantConfig.URL.Replace("'", "''") + "','" +
                    oMerchantConfig.VCBin.Replace("'", "''") + "','" +
                    oMerchantConfig.MCBin.Replace("'", "''") + "')";

                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSQL;
                cmd.Transaction = oTrans;
                cmd.Connection = oTrans.Connection;
                int nRowsAffected = cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region  Sprint-24 - PRIMEPOS-2344 18-Jan-2017 JY Added to get last updated IIAS file date
        public DateTime? GetIIASFileLastUpdatedDate()
        {
            DateTime? dt = null;
            try
            {
                string sSQL = "select TOP 1 CreateDate from IIAS_items order by CreateDate desc";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue != null)
                {
                    dt = Convert.ToDateTime(objValue);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dt;
        }
        #endregion

        #region Sprint-25 - PRIMEPOS-2379 08-Feb-2017 JY Added
        public DateTime? GetPSEFileLastUpdatedDate()
        {
            DateTime? dt = null;
            try
            {
                string sSQL = "select TOP 1 UpdatedOn from PSE_Items order by UpdatedOn desc";
                object objValue = DataHelper.ExecuteScalar(sSQL);
                if (objValue != null)
                {
                    dt = Convert.ToDateTime(objValue);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dt;
        }
        #endregion

        #region Sprint-26 - PRIMEPOS-2441 21-Jul-2017 JY Added
        public DataTable GetMerchantConfig()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

                    string sSQL = " SELECT * FROM MerchantConfig";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds.Tables[0];
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region PRIMEPOS-2308 15-May-2018 JY Added to load paytype grid to set # of receipts 
        public DataTable GetPayTypesReceipts()
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    string strSQL = "SELECT STATIONID FROM Util_PayTypeReceipts WHERE STATIONID = '" + Configuration.StationID + "'";
                    object obj = DataHelper.ExecuteScalar(Configuration.ConnectionString, CommandType.Text, strSQL);
                    if (obj == null)
                    {
                        #region if PayTypeReceits records not found w.r.t. logged-in station then it might be new station, need to insert all records in it
                        strSQL = "INSERT INTO Util_PayTypeReceipts(STATIONID, PayTypeID, NoOfReceipts)" +
                                  " SELECT DISTINCT a.STATIONID, b.PayTypeID, 0 As NoOfReceipts FROM Util_POSSET a" +
                                  " CROSS JOIN PayType b" +
                                  " WHERE b.PayTypeID NOT IN ('3', '4', '5', '6', '7', 'E', 'B') AND a.STATIONID = '" + Configuration.StationID + "'" +
                                  " UNION" +
                                  " SELECT '" + Configuration.StationID + "' AS STATIONID, '99' AS PayTypeID, 0 As NoOfReceipts " +
                                  " ORDER By a.STATIONID, b.PayTypeID";

                        int RowsAffected = DataHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);

                        if (RowsAffected > 0)
                        {
                            strSQL = "UPDATE a SET a.NoOfReceipts = b.NoOfCC FROM Util_PayTypeReceipts a" +
                                    " INNER JOIN Util_POSSET b ON LTRIM(RTRIM(a.STATIONID)) = LTRIM(RTRIM(b.STATIONID))" +
                                    " WHERE a.PayTypeID = '99' AND b.NoOfCC <> 0 AND a.STATIONID = '" + Configuration.StationID + "';" +
                                    "UPDATE a SET a.NoOfReceipts = b.NoOfCheckRC FROM Util_PayTypeReceipts a" +
                                    " INNER JOIN Util_POSSET b ON LTRIM(RTRIM(a.STATIONID)) = LTRIM(RTRIM(b.STATIONID))" +
                                    " WHERE a.PayTypeID = '2' AND b.NoOfCheckRC <> 0 AND a.STATIONID = '" + Configuration.StationID + "';" +
                                    "UPDATE a SET a.NoOfReceipts = b.NoOfHCRC FROM Util_PayTypeReceipts a" +
                                    " INNER JOIN Util_POSSET b ON LTRIM(RTRIM(a.STATIONID)) = LTRIM(RTRIM(b.STATIONID))" +
                                    " WHERE a.PayTypeID = 'H' AND b.NoOfHCRC <> 0 AND a.STATIONID = '" + Configuration.StationID + "';" +
                                    "UPDATE a SET a.NoOfReceipts = b.NoOfCashReceipts FROM Util_PayTypeReceipts a" +
                                    " INNER JOIN Util_POSSET b ON LTRIM(RTRIM(a.STATIONID)) = LTRIM(RTRIM(b.STATIONID))" +
                                    " WHERE a.PayTypeID = '1' AND b.NoOfCashReceipts <> 0 AND a.STATIONID = '" + Configuration.StationID + "'";

                            RowsAffected = DataHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                        }
                        #endregion
                    }
                    else
                    {
                        //add recently added paytype if any
                        strSQL = "SELECT b.PayTypeID FROM Util_PayTypeReceipts a" +
                            " RIGHT JOIN (SELECT a1.PayTypeID FROM PayType a1 WHERE a1.PayTypeID NOT IN('3', '4', '5', '6', '7', 'E', 'B')" +
                                        " UNION SELECT '99' AS PayTypeID ) b ON a.PayTypeID = b.PayTypeID AND a.STATIONID = '" + Configuration.StationID + "'" +
                             " WHERE a.PayTypeID IS NULL";

                        dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                strSQL = "INSERT INTO Util_PayTypeReceipts(STATIONID,PayTypeID,NoOfReceipts)" +
                                    " SELECT '" + Configuration.StationID + "','" + dt.Rows[i]["PayTypeID"] + "',0";

                                DataHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                            }
                        }
                    }

                    //return the paytype list with no of receipts 
                    strSQL = "SELECT a.STATIONID, a.PayTypeID AS PayTypeID, CASE WHEN a.PayTypeID = '99' THEN 'CC' ELSE b.PayTypeDesc END AS [Description], a.NoOfReceipts AS [# of Receipts] FROM Util_PayTypeReceipts a" +
                        " LEFT JOIN PayType b ON a.PayTypeID = b.PayTypeID" +
                        " WHERE a.STATIONID = '" + Configuration.StationID + "'" +
                        " and a.PayTypeID <> '8' " +//2664
                        " ORDER BY ISNULL(b.CustomPayType,0), 3";   //PRIMEPOS-2940 30-Mar-2021 JY Added
                                                                    //" ORDER BY 3";

                    dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);

                    //update dctPayTypeReceipts as well
                    if (Configuration.dctPayTypeReceipts != null)
                        Configuration.dctPayTypeReceipts.Clear();
                    else
                        Configuration.dctPayTypeReceipts = new Dictionary<string, int>();

                    #region PRIMEPOS-2847 20-May-2020 JY Added - If the paytype is deleted then we need to delete receipt record from Util_PayTypeReceipts table
                    try
                    {
                        string strPayTypeIDs = string.Empty;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (Configuration.convertNullToString(dt.Rows[i]["Description"]) == "")
                                {
                                    if (strPayTypeIDs == string.Empty)
                                    {
                                        strPayTypeIDs = "'" + Configuration.convertNullToString(dt.Rows[i]["PayTypeID"]) + "'";
                                    }
                                    else
                                    {
                                        strPayTypeIDs += ",'" + Configuration.convertNullToString(dt.Rows[i]["PayTypeID"]) + "'";
                                    }
                                }
                            }
                            if (strPayTypeIDs != "")
                            {
                                strSQL = "DELETE FROM Util_PayTypeReceipts WHERE PayTypeID IN (" + strPayTypeIDs + ")";
                                DataHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);

                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (Configuration.convertNullToString(dr["Description"]) == "")
                                        dr.Delete();
                                }
                                dt.AcceptChanges();
                            }
                        }
                    }
                    catch { }
                    #endregion

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Configuration.convertNullToString(dr[2]).Trim() != "")
                                Configuration.dctPayTypeReceipts.Add(Configuration.convertNullToString(dr[1]).Trim(), Configuration.convertNullToInt(dr[3]));
                        }
                    }
                }
                return dt;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region PRIMEPOS-2308 16-May-2018 JY Added logic to update # of receipts against paytype
        public void UpdatePayTypesReceipts(IDbTransaction oTrans, DataTable dtPayTypeReceipts)
        {
            try
            {
                string strSQL = string.Empty;
                DataTable dt = dtPayTypeReceipts.GetChanges();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Configuration.dctPayTypeReceipts[Configuration.convertNullToString(dr[1])] = Configuration.convertNullToInt(dr[3]); //update dctPayTypeReceipts as well

                        if (strSQL == string.Empty)
                            strSQL = "UPDATE Util_PayTypeReceipts SET NoOfReceipts = " + Configuration.convertNullToInt(dr[3]) + " WHERE PayTypeID = '" + Configuration.convertNullToString(dr[1]) + "'";
                        else
                            strSQL += "; UPDATE Util_PayTypeReceipts SET NoOfReceipts = " + Configuration.convertNullToInt(dr[3]) + " WHERE PayTypeID = '" + Configuration.convertNullToString(dr[1]) + "'";
                    }
                    DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, strSQL, (IDbDataParameter[])null);


                }
            }
            catch (Exception Ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                throw (Ex);
            }
        }
        #endregion

        #region PRIMEPOS-2613 26-Dec-2018 JY Added
        public DataTable getMessageFormat(int MessageCatId, int MessageTypeId)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    string strSQL = "SELECT RecID, MessageCode FROM FMessage " +
                                    " WHERE MessageCatId = " + MessageCatId + " AND MessageTypeId =  " + MessageTypeId +
                                    " ORDER BY MessageCode";
                    dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                }
                return dt;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region PRIMEPOS-2842 05-May-2020 JY Added       
        public DataTable GetTriPOSSettings()
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    string strSQL = "SELECT TriPOSAcceptorID, TriPOSAccountID, TriPOSAccountToken, TriPOSLaneID, TriPOSLaneDesc, TriPOSConfigFilePath, TriPOSTerminalId, TriPOSIPLaneID, TriPOSIPLaneDesc, TriPOSIPTerminalId FROM Util_POSSET WHERE STATIONID = '" + Configuration.StationID + "'";

                    dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                }
                return dt;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public void UpdateTriPOSSettings(IDbTransaction oTrans, System.Int64 TriPOSAcceptorID, System.Int64 TriPOSAccountID, string TriPOSAccountToken, int TriPOSLaneID, string TriPOSLaneDesc, string TriPOSConfigFilePath, string TriPOSTerminalId)
        {
            try
            {
                string sSQL = "SELECT STATIONID FROM Util_POSSET WHERE STATIONID = '" + Configuration.StationID + "'";
                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = oTrans;
                cmd.Connection = oTrans.Connection;
                string result = "";
                result = (string)cmd.ExecuteScalar();

                if (result != null)
                {
                    sSQL = "UPDATE Util_POSSet SET " +
                        " TriPOSAcceptorID = " + TriPOSAcceptorID +
                        ", TriPOSAccountID = " + TriPOSAccountID +
                        ", TriPOSAccountToken = '" + TriPOSAccountToken +
                        "', TriPOSLaneID = " + TriPOSLaneID +
                        ", TriPOSLaneDesc = '" + TriPOSLaneDesc +
                        "', TriPOSConfigFilePath = '" + TriPOSConfigFilePath +
                        "', TriPOSTerminalId = '" + TriPOSTerminalId +
                        " ' WHERE STATIONID = '" + Configuration.StationID.Replace("'", "''") + "'";
                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
            }
        }

        #region PRIMEPOS-3024 08-Nov-2021 JY Added
        public void UpdateTriPOSIPLane(IDbTransaction oTrans, int TriPOSLaneID, string TriPOSLaneDesc, string TriPOSTerminalId)
        {
            try
            {
                string sSQL = "SELECT STATIONID FROM Util_POSSET WHERE STATIONID = '" + Configuration.StationID + "'";
                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = oTrans;
                cmd.Connection = oTrans.Connection;
                string result = "";
                result = (string)cmd.ExecuteScalar();

                if (result != null)
                {
                    sSQL = "UPDATE Util_POSSet SET TriPOSIPLaneID = " + TriPOSLaneID + ", TriPOSIPLaneDesc = '" + TriPOSLaneDesc +
                        "', TriPOSIPTerminalId = '" + TriPOSTerminalId + "' WHERE STATIONID = '" + Configuration.StationID.Replace("'", "''") + "'";
                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        public void UpdateTriPOSConfigFilePath(IDbTransaction oTrans, string strConfigFilePath)
        {
            try
            {
                string sSQL = "SELECT TriPOSConfigFilePath FROM Util_POSSET WHERE STATIONID = '" + Configuration.StationID + "'";
                IDbCommand cmd = DataFactory.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Transaction = oTrans;
                cmd.Connection = oTrans.Connection;
                string strDBConfigFilePath = Configuration.convertNullToString(cmd.ExecuteScalar());

                if (strDBConfigFilePath.Trim().ToUpper() != strConfigFilePath.Trim().ToUpper())
                {
                    sSQL = "UPDATE Util_POSSet SET TriPOSConfigFilePath = '" + strConfigFilePath + " ' WHERE STATIONID = '" + Configuration.StationID.Replace("'", "''") + "'";
                    cmd.CommandText = sSQL;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region PRIMEPOS-2512 01-Oct-2020 JY Added
        public DataTable GetPayTypeData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    string strSQL = "SELECT * FROM (SELECT LTRIM(RTRIM(PayTypeID)) AS PayTypeID, PayTypeDesc, CustomPayType, SortOrder FROM PayType " +
                                    " UNION SELECT '-999' AS PayTypeID, ' Let user select every time' AS PayTypeDesc, -1 AS CustomPayType, NULL AS SortOrder) X ORDER BY SortOrder, CustomPayType, PayTypeID";  //PRIMEPOS-2966 20-May-2021 JY Modified

                    dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                }
                return dt;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public void UpdateIsHideFlagForDefaultPayType(IDbTransaction oTrans, string DefaultPaytype)
        {
            try
            {
                string strSQL = "SELECT ISNULL(IsHide,0) AS IsHide FROM PayType WHERE PayTypeID = '" + DefaultPaytype.Trim().Replace("'", "''") + "'";
                object objValue = DataHelper.ExecuteScalar(strSQL);
                if (objValue != null && Configuration.convertNullToBoolean(objValue) == true)
                {
                    IDbCommand cmd = DataFactory.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = oTrans.Connection;
                    cmd.Transaction = oTrans;
                    strSQL = "UPDATE PayType SET IsHide = 0 WHERE PayTypeID = '" + DefaultPaytype.Trim().Replace("'", "''") + "'";
                    cmd.CommandText = strSQL;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception Ex)
            {
            }
        }
        #endregion
    }
}