using System;
using System.Collections;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using Microsoft.Win32;

using POS_Core_UI.Reports.Reports;
//using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using POS_Core.CommonData;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Text;
using System.Collections.Generic;
using Resources;
using POS_Core.Resources;

namespace POS_Core_UI.Reports.ReportsUI
{
    /// <summary>
    /// Summary description for clsReports.
    /// </summary>
    public class clsReports
    {
        private static frmReportViewer oRptViewer = new frmReportViewer();
        public static DataSet DStoExport = null;//Added By Krishna on 9 June 2011
        public clsReports()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //Added by SRT(Abhishek) Date : 07 Aug 09
        // Added to chek if PrintPreview is called from the 
        //cretenew purchase order form
        public static bool isCreteNewPreView = false;

        public static bool CreateNewPreView
        {
            set { isCreteNewPreView = value; }
            get { return isCreteNewPreView; }
        }
        //end of added by (SRT)Abhishek.

        /// <summary>
        /// Sets the currency Symbol for the current application thread and also
        /// changes the systems currency symbol to the symbol set by the user in application in 
        /// configuration settings.
        /// </summary>
        public static void SetCurrencySymbol()
        {
            var currencySymbol = Configuration.CInfo.CurrencySymbol.ToString();
            const string currencySymbolRegistryPath = @"HKEY_CURRENT_USER\Control Panel\International";
            const string currencySymbolRegistryName = "sCurrency";
            try
            {
                Registry.SetValue(currencySymbolRegistryPath, currencySymbolRegistryName, currencySymbol,
                                  RegistryValueKind.String);
            }
            catch (ArgumentException)
            {
            }

            var currentCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            currentCulture.NumberFormat.CurrencySymbol = currencySymbol;

            System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        //Added by Krishna on 22 June 2011
        public static ArrayList dsToExportList = new ArrayList();
        //Till here added by Krishna on 22 June 2011
        public static void setCRTextObjectText(string ObjectKey, string TextToSet, ReportClass oRpt)
        {
            try
            {
                //                dsToExportList.Clear();
                CrystalDecisions.CrystalReports.Engine.TextObject txt = (CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects[ObjectKey];
                txt.Text = TextToSet;
                string title = ObjectKey;
                dsToExportList.Add(title);
                string value = TextToSet;
                dsToExportList.Add(value);
            }
            catch (Exception ex)
            {
            }
        }

        public static bool SetRepParam(ReportClass oRepDoc, string ParamName, string ParamValue)
        {
            CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition pDef;
            pDef = oRepDoc.DataDefinition.ParameterFields[ParamName];

            CrystalDecisions.Shared.ParameterValues pVals = new CrystalDecisions.Shared.ParameterValues();
            CrystalDecisions.Shared.ParameterDiscreteValue pVal = new CrystalDecisions.Shared.ParameterDiscreteValue();

            pVal.Value = ParamValue; //AppGlobal.gPharmacy.sPharmacyName;
            pVals.Add(pVal);
            pDef.ApplyCurrentValues(pVals);
            return true;

        }

        public static void Preview(bool PrintIt, string sSQL, ReportClass oRpt, bool bCalledFromScheduler = false)  //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
        {
            try
            {
                SetCurrencySymbol();
                Logs.Logger("Start Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                //Added by krishna on 17 August 2011
                ReportDocument subRptObj;
                //Till here added by krishna on 17 August 2011
                //CrystalDecisions.CrystalReports.Engine.TextObject txtObj;

                frmMain.getInstance().Cursor = Cursors.WaitCursor;
                DataSet ds = clsReports.GetReportSource(sSQL);
                //Follwing Code added by Krishna on 9 June 2011
                DStoExport = ds;
                //Till here added by Krishna on 9 June 2011
                oRpt.SetDatabaseLogon(Configuration.SQLUserID, Configuration.SQLUserPassword); //Added as a work around by Abhishek.
                oRpt.SetDataSource(ds.Tables[0]);
                // oRpt.Subreports[0].Database.Tables   
                try
                {
                    if (oRpt.ReportDefinition.ReportObjects["txtHeader"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    //Added By Shitaljit(QuicSolv)on 13 May 2011
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    //Modified By Shitaljit(QuicSolv) on 18 July 2011
                    //Added State, City and Zip on the Report Header
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    if (oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                    //Till here aded By Shitaljit.
                    string CurSymbol = Configuration.CInfo.CurrencySymbol.ToString();
                    oRpt.SetParameterValue("Currency", CurSymbol);    //Added by Ravindrto set Global currency setting 
                    Logs.Logger("End Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                }
                catch (Exception ex) { Logs.Logger("Exception Report Preview" + oRpt.Name + "Exception:" + ex.Message + " Print =" + PrintIt.ToString()); }

                //Added by krishna on 17 August 2011
                for (int i = 0; i < oRpt.Subreports.Count; i++)
                {
                    try
                    {
                        subRptObj = oRpt.Subreports[i];

                        if (subRptObj.ReportDefinition.ReportObjects["txtHeader"] != null)
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)subRptObj.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                        //Added By Shitaljit(QuicSolv)on 13 May 2011
                        if (subRptObj.ReportDefinition.ReportObjects["txtPharmacyAddress"] != null)
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)subRptObj.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                        //Modified By Shitaljit(QuicSolv) on 18 July 2011
                        //Added State, City and Zip on the Report Header
                        if (subRptObj.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"] != null)
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)subRptObj.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                        if (subRptObj.ReportDefinition.ReportObjects["txtTelephoneNo"] != null)
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)subRptObj.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                        string CurSymbol = Configuration.CInfo.CurrencySymbol.ToString();
                        oRpt.SetParameterValue("Currency", CurSymbol);
                        //Till here aded By Shitaljit.
                        Logs.Logger("End Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                    }
                    catch (Exception ex) { Logs.Logger("Exception Report Preview" + oRpt.Name + "Exception:" + ex.Message + " Print =" + PrintIt.ToString()); }
                }
                //Till here added by krishna on 17 August 2011

                //	oRpt.Database.Tables[0]=ds.Tables[0];  
                if (PrintIt)    
                    clsReports.PrintReport(oRpt);
                else
                    if (!bCalledFromScheduler)  clsReports.ShowReport(oRpt);    //PRIMEPOS-2485 02-Apr-2021 JY Added

                frmMain.getInstance().Cursor = Cursors.Default;
            }
            catch (Exception exp)
            {
                frmMain.getInstance().Cursor = Cursors.Default;
                Resources.Message.Display(exp.Message, "Reports", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Preview(bool PrintIt, string sSQL, string sumSQL, ReportClass oRpt, int iType, bool bCalledFromScheduler = false)    //PRIMEPOS-2485 22-Mar-2021 JY Added bCalledFromScheduler
        {
            try
            {
                SetCurrencySymbol();
                Logs.Logger("Start Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                //Added by krishna on 17 August 2011
                ReportDocument subRptObj;
                //Till here added by krishna on 17 August 2011
                //CrystalDecisions.CrystalReports.Engine.TextObject txtObj;

                frmMain.getInstance().Cursor = Cursors.WaitCursor;
                DataSet ds = clsReports.GetReportSource(sSQL);
                DataSet ds1 = clsReports.GetReportSource(sumSQL);   //JY
                //Follwing Code added by Krishna on 9 June 2011
                DStoExport = ds;
                //Till here added by Krishna on 9 June 2011
                oRpt.SetDatabaseLogon(Configuration.SQLUserID, Configuration.SQLUserPassword); //Added as a work around by Abhishek.
                                                                                               //oRpt.SetDataSource(ds.Tables[0]);

                oRpt.Database.Tables[0].SetDataSource(ds.Tables[0]);
                oRpt.Database.Tables[1].SetDataSource(ds1.Tables[0]);


                // oRpt.Subreports[0].Database.Tables   
                try
                {
                    if (oRpt.ReportDefinition.ReportObjects["txtHeader"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    //Added By Shitaljit(QuicSolv)on 13 May 2011
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    //Modified By Shitaljit(QuicSolv) on 18 July 2011
                    //Added State, City and Zip on the Report Header
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                    if (oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                    //Till here aded By Shitaljit.
                    string CurSymbol = Configuration.CInfo.CurrencySymbol.ToString();
                    oRpt.SetParameterValue("Currency", CurSymbol);    //Added by Ravindrto set Global currency setting 
                    try
                    {
                        string strCompanyLogoPath = POS_Core.Resources.Configuration.GetCompanyLogoPath(oRpt);    //PRIMEPOS-2386 02-Mar-2021 JY Added
                        oRpt.SetParameterValue("CompanyLogoPath", strCompanyLogoPath);   //PRIMEPOS-2386 02-Mar-2021 JY Added
                    }
                    catch { }
                    Logs.Logger("End Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                }
                catch (Exception ex) { Logs.Logger("Exception Report Preview" + oRpt.Name + "Exception:" + ex.Message + " Print =" + PrintIt.ToString()); }

                //Added by krishna on 17 August 2011
                for (int i = 0; i < oRpt.Subreports.Count; i++)
                {
                    try
                    {
                        subRptObj = oRpt.Subreports[i];

                        if (subRptObj.ReportDefinition.ReportObjects["txtHeader"] != null)
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)subRptObj.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                        //Added By Shitaljit(QuicSolv)on 13 May 2011
                        if (subRptObj.ReportDefinition.ReportObjects["txtPharmacyAddress"] != null)
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)subRptObj.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                        //Modified By Shitaljit(QuicSolv) on 18 July 2011
                        //Added State, City and Zip on the Report Header
                        if (subRptObj.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"] != null)
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)subRptObj.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + " " + Configuration.CInfo.Zip;
                        if (subRptObj.ReportDefinition.ReportObjects["txtTelephoneNo"] != null)
                            ((CrystalDecisions.CrystalReports.Engine.TextObject)subRptObj.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                        string CurSymbol = Configuration.CInfo.CurrencySymbol.ToString();
                        oRpt.SetParameterValue("Currency", CurSymbol);
                        //Till here aded By Shitaljit.
                        Logs.Logger("End Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                    }
                    catch (Exception ex) { Logs.Logger("Exception Report Preview" + oRpt.Name + "Exception:" + ex.Message + " Print =" + PrintIt.ToString()); }
                }
                //Till here added by krishna on 17 August 2011

                //	oRpt.Database.Tables[0]=ds.Tables[0];  
                if (PrintIt)    
                    clsReports.PrintReport(oRpt);
                else
                    if (!bCalledFromScheduler)  clsReports.ShowReport(oRpt);    //PRIMEPOS-2485 22-Mar-2021 JY Added

                frmMain.getInstance().Cursor = Cursors.Default;
            }
            catch (Exception exp)
            {
                frmMain.getInstance().Cursor = Cursors.Default;
                Resources.Message.Display(exp.Message, "Reports", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public static void Preview(bool PrintIt, string mainReportQL, string subReportQL, ReportClass oRpt)
        {
            DataSet mainReortds = null;
            DataSet subReortds = null;
            ParameterField paramfield = null;
            try
            {
                SetCurrencySymbol();
                Logs.Logger("Start Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                frmMain.getInstance().Cursor = Cursors.WaitCursor;
                mainReortds = clsReports.GetReportSource(mainReportQL);
                //Follwing Code added by Krishna on 9 June 2011
                DStoExport = mainReortds;
                //Till here added by Krishna on 9 June 2011
                //subReortds = clsReports.GetReportSource(subReportQL);
                paramfield = new ParameterField();

                oRpt.SetDatabaseLogon(Configuration.SQLUserID, Configuration.SQLUserPassword);
                oRpt.SetDataSource(mainReortds.Tables[0]);
                //oRpt.Subreports[0].SetParameterValue("OrderNO", 
                //oRpt.Subreports[0].Database.Tables[0].SetDataSource(subReortds);    
                try
                {
                    if (oRpt.ReportDefinition.ReportObjects["txtHeader"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    //Added By Shitaljit(QuicSolv)on 13 May 2011
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    //Modified By Shitaljit(QuicSolv) on 18 July 2011
                    //Added State, City and Zip on the Report Header
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + "  " + Configuration.CInfo.Zip;
                    if (oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                    //Till here aded By Shitaljit.
                    string CurSymbol = Configuration.CInfo.CurrencySymbol.ToString();
                    oRpt.SetParameterValue("Currency", CurSymbol);    //Added by Ravindrto set Global currency setting 
                    //"currencysymbol"
                    Logs.Logger("End Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                }
                catch (Exception ex) { Logs.Logger("Exception Report Preview" + oRpt.Name + "Exception:" + ex.Message + " Print =" + PrintIt.ToString()); }
                if (PrintIt)
                    clsReports.PrintReport(oRpt);
                else
                    clsReports.ShowReport(oRpt);

                frmMain.getInstance().Cursor = Cursors.Default;
            }
            catch (Exception exp)
            {
                frmMain.getInstance().Cursor = Cursors.Default;
                Resources.Message.Display(exp.Message, "Reports", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void Preview(bool PrintIt, ReportClass oRpt, bool bCalledFromScheduler = false)  //PRIMEPOS-2485 02-Apr-2021 JY Added bCalledFromScheduler
        {
            try
            {
                SetCurrencySymbol();
                Logs.Logger("Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                frmMain.getInstance().Cursor = Cursors.WaitCursor;
                oRpt.SetDatabaseLogon(Configuration.SQLUserID, Configuration.SQLUserPassword);
                try
                {
                    if (oRpt.ReportDefinition.ReportObjects["txtHeader"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    //Added By Shitaljit(QuicSolv)on 13 May 2011
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    //Modified By Shitaljit(QuicSolv) on 18 July 2011
                    //Added State, City and Zip on the Report Header
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + "  " + Configuration.CInfo.Zip;
                    if (oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                    //Till here aded By Shitaljit.
                    string CurSymbol = Configuration.CInfo.CurrencySymbol.ToString();
                    oRpt.SetParameterValue("Currency", CurSymbol);    //Added by Ravindrto set Global currency setting 
                    Logs.Logger("End Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                }
                catch (Exception ex) { Logs.Logger("Exception Report Preview" + oRpt.Name + "Exception:" + ex.Message + " Print =" + PrintIt.ToString()); }
                if (PrintIt)    
                    clsReports.PrintReport(oRpt);
                else
                    if (!bCalledFromScheduler) clsReports.ShowReport(oRpt);    //PRIMEPOS-2485 02-Apr-2021 JY Added

                frmMain.getInstance().Cursor = Cursors.Default;
            }
            catch (Exception exp)
            {
                frmMain.getInstance().Cursor = Cursors.Default;
                Resources.Message.Display(exp.Message, "Reports", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //-------------------------------------------------------------------------------------------------------
        //Follwing Code added by Krishna on 15 June 2011
        public static void SetReportHeader(ReportClass oRpt, DataSet oDsToExport)
        {
            DStoExport = oDsToExport;
            if (oRpt.ReportDefinition.ReportObjects["txtHeader"] != null)
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;

            if (oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"] != null)
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
            //Modified By Shitaljit(QuicSolv) on 18 July 2011
            //Added State, City and Zip on the Report Header
            if (oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"] != null)
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + "  " + Configuration.CInfo.Zip;
            if (oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"] != null)
                ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
        }
        //Till here added by Krishna on 15 June 2011
        //-------------------------------------------------------------------------------------------------------

        public static void Preview(bool PrintIt, System.Data.DataSet reportData, ReportClass oRpt)
        {
            try
            {
                SetCurrencySymbol();
                //CrystalDecisions.CrystalReports.Engine.TextObject txtObj;
                Logs.Logger("Start Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                frmMain.getInstance().Cursor = Cursors.WaitCursor;
                DataSet ds = reportData.Copy();
                oRpt.SetDataSource(ds.Tables[0]);
                //Follwing Code added by Krishna on 9 June 2011
                DStoExport = ds;
                //Till here added by Krishna on 9 June 2011
                try
                {
                    if (oRpt.ReportDefinition.ReportObjects["txtHeader"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
                    //Added By Shitaljit(QuicSolv)on 13 May 2011
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyAddress"]).Text = Configuration.CInfo.Address;
                    //Modified By Shitaljit(QuicSolv) on 18 July 2011
                    //Added State, City and Zip on the Report Header
                    if (oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtPharmacyCityStateZip"]).Text = Configuration.CInfo.City + ", " + Configuration.CInfo.State + "  " + Configuration.CInfo.Zip;
                    if (oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"] != null)
                        ((CrystalDecisions.CrystalReports.Engine.TextObject)oRpt.ReportDefinition.ReportObjects["txtTelephoneNo"]).Text = Configuration.CInfo.Telephone;
                    //Till here aded By Shitaljit.

                    string CurSymbol = Configuration.CInfo.CurrencySymbol.ToString();
                    oRpt.SetParameterValue("Currency", CurSymbol);    //Added by Ravindrto set Global currency setting
                    Logs.Logger("End Report Preview" + oRpt.Name + " Print =" + PrintIt.ToString());
                }
                catch (Exception ex) { Logs.Logger("Exception Report Preview" + oRpt.Name + "Exception:" + ex.Message + " Print =" + PrintIt.ToString()); }
                if (PrintIt)
                    clsReports.PrintReport(oRpt);
                else
                    clsReports.ShowReport(oRpt);

                frmMain.getInstance().Cursor = Cursors.Default;

            }
            catch (Exception exp)
            {
                frmMain.getInstance().Cursor = Cursors.Default;
                Resources.Message.Display(exp.Message, "Reports", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                frmMain.getInstance().Cursor = Cursors.Default;
            }
        }

        //public static string ExportRptToPdf(clsReports.)
        //{
        //    try
        //    {
        //        if (clsReports.DStoExport == null)
        //            return "";
        //        SaveFileDialog saveFileDialog = new SaveFileDialog();
        //        saveFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
        //        saveFileDialog.RestoreDirectory = true;
        //        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            System.Data.DataSet DStoExport = clsReports.DStoExport;

        //            ReportDocument cryRpt = new ReportDocument();
        //            cryRpt = (ReportDocument)rvReportViewer.ReportSource;
        //            string sFileName = string.Empty;
        //            string Path = System.Windows.Forms.Application.ExecutablePath;
        //            System.IO.FileInfo FI = new System.IO.FileInfo(Path);
        //            sFileName = saveFileDialog.FileName + ".pdf";
        //            cryRpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, sFileName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsUIHelper.ShowErrorMsg(ex.Message);
        //    }
        //}


        public static DataSet GetReportSource(string sSQL)
        {
            IDbCommand cmd = DataFactory.CreateCommand();

            DataSet ds = new DataSet();
            IDataAdapter da = DataFactory.CreateDataAdapter();
            IDbConnection conn = DataFactory.CreateConnection();

            conn.ConnectionString = Configuration.ConnectionString;
            if (conn.ConnectionString.EndsWith(";") == false)
            {
                conn.ConnectionString += ";";
            }
            conn.ConnectionString += "Connect Timeout=60";

            conn.Open();

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.CommandTimeout = 1200;
                cmd.Connection = conn;

                SqlDataAdapter sqlDa = (SqlDataAdapter)da;
                sqlDa.SelectCommand = (SqlCommand)cmd;
                da.Fill(ds);

                conn.Close();
                return ds;
            }
            catch (NullReferenceException)
            {
                conn.Close();
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                conn.Close();
                throw (exp);
            }
            return null;
        }

        public static frmReportViewer ReportViewer
        {
            get
            {
                if (oRptViewer.IsDisposed)
                    oRptViewer = new frmReportViewer();
                return oRptViewer;
            }
        }

        public static void ShowReport(ReportClass pReport)
        {
            ShowReport(pReport, frmMain.getInstance());
        }

        public static void ShowReport(ReportClass pReport, Form oForm)
        {
            try
            {
                frmReportViewer oRpt = new frmReportViewer();

                try
                {
                    pReport.SetParameterValue("Currency", POS_Core.Resources.Configuration.CInfo.CurrencySymbol.ToString());
                    SetCurrencySymbol();
                }
                catch (Exception Ex)
                {
                    POS_Core.ErrorLogging.Logs.Logger(Ex.StackTrace);
                }
                // Modification Ends
                oRpt.rvReportViewer.ReportSource = pReport;
                //Added by SRT(Abhishek) Date : 07 Aug 09
                if (!clsReports.CreateNewPreView)
                    oRpt.MdiParent = oForm;
                //End of Added by SRT(Abhishek) Date : 07 Aug 09
                oRpt.WindowState = FormWindowState.Maximized;

                oRpt.rvReportViewer.DisplayGroupTree = false;
                oRpt.TopMost = true;
                oRpt.Show();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void PrintReport(ReportClass pReport)
        {
            try
            {
                SetCurrencySymbol();
                pReport.SetParameterValue("Currency", POS_Core.Resources.Configuration.CInfo.CurrencySymbol.ToString());
            }
            catch (Exception Ex)
            {
                POS_Core.ErrorLogging.Logs.Logger(Ex.StackTrace);
            }
            ReportViewer.rvReportViewer.ReportSource = pReport;
            System.Windows.Forms.PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            pd.AllowSomePages = true;
            pd.AllowSelection = false;
            pd.ShowNetwork = true;

            #region PRIMEPOS-2996 23-Sep-2021 JY Added
            string strReportPrinter = string.Empty;
            try
            {
                if (Configuration.CPOSSet.ReportPrinter.Trim() != "")
                {
                    Configuration.SetReportPrinter(ref strReportPrinter);
                    if (strReportPrinter != "")
                        pd.PrinterSettings.PrinterName = strReportPrinter;
                }
                if (Configuration.CPOSSet.ReportPrinterPaperSource.Trim() != "")
                {
                    System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                    foreach (System.Drawing.Printing.PaperSource ps in pd.PrinterSettings.PaperSources)
                    {
                        if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.ReportPrinterPaperSource.Trim().ToUpper())
                        {
                            pReport.PrintOptions.CustomPaperSource = ps;
                            break;
                        }
                    }
                }
            }
            catch { }
            #endregion

            if (pd.ShowDialog() != DialogResult.Cancel)
            {
                if (pd.PrinterSettings.IsValid == true)
                {
                    try
                    {
                        pReport.PrintOptions.PrinterName = pd.PrinterSettings.PrinterName;	 //PrinterSel.cbPrinter.Text;
                        pReport.PrintToPrinter(pd.PrinterSettings.Copies, pd.PrinterSettings.Collate, pd.PrinterSettings.FromPage, pd.PrinterSettings.ToPage);
                        //oRpt.SetParameterValue("Currency",Configuration.CInfo.CurrencySymbol);    //Added by Ravindrto set Global currency setting 
                    }
                    catch (PrintException)
                    { }
                }
                else
                {
                    throw (new Exception("Invalid printer name " + pd.PrinterSettings.PrinterName));
                }
            }
        }

        public static bool EmailReport(ReportClass reportName, string CustName, string toMail, string subject, string body, string fileName)
        {
            //public static bool sendImvoiceMAil(string CustName,string toMail, string trancID, DateTime trancedate)

            try
            {

                Logs.Logger("Start sending email TO" + toMail);
                Regex expression = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                string words = toMail;

                string[] split = words.Split(new Char[] { ',', ';' });
                //Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Configuration.CInfo.OutGoingEmailServer);

                if (expression.IsMatch(Configuration.CInfo.OutGoingEmailID))
                {
                    mail.From = new MailAddress(Configuration.CInfo.OutGoingEmailID);
                }
                else
                {
                    clsUIHelper.ShowErrorMsg("Invalid Email Format OutGoingEmailID");
                    return false;
                }
                foreach (string emailString in split)
                {

                    if (expression.IsMatch(emailString))
                    {
                        mail.To.Add(emailString);
                    }
                    else
                    {
                        clsUIHelper.ShowErrorMsg("Invalid Email Format");
                        return false;
                    }
                }

                SmtpServer.Port = Configuration.CInfo.OutGoingEmailPort;
                SmtpServer.EnableSsl = Configuration.CInfo.OutGoingEmailEnableSSL;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Configuration.CInfo.OutGoingEmailUserID, Configuration.CInfo.OutGoingEmailPass);

                StringBuilder mailBody = new StringBuilder();

                mailBody.AppendFormat("<br> Dear " + CustName + "<br />");
                mailBody.AppendFormat("<p>" + body + "</p>");
                mailBody.AppendFormat("" + Configuration.CInfo.OutGoingEmailSignature + "");

                mail.Subject = subject;//"Invoice:" + trancID + ",Purchase Date:" + trancedate;
                string htmlBody = mailBody.ToString();
                mail.Body = htmlBody;
                mail.IsBodyHtml = true;
                if (reportName != null)
                {
                    mail.Attachments.Add(new Attachment(reportName.ExportToStream(ExportFormatType.PortableDocFormat), fileName + ".pdf"));
                }
                // mail.Attachments.Add(attachment);

                SmtpServer.Send(mail);
                Logs.Logger("Email sent successfully TO" + toMail);
                return true;
            }
            catch (Exception ex)
            {
                Logs.Logger("Unable to send Email  successfully TO" + toMail + Environment.NewLine + ex.Message);
                clsUIHelper.ShowErrorMsg("Unable to Send Email" + Environment.NewLine + ex.Message);
                return false;
            }


        }

        //Sprint-24 - PRIMEPOS-2363 26-Dec-2016 JY Added
        public static bool EmailReport(List<ReportClass> lstReportClass, string toMail, string subject, string body, string fileName, Boolean bShowMessage = true, bool bCalledFromScheduler = false)   //PRIMEPOS-3039 16-Dec-2021 JY Added
        {
            //PRIMEPOS-2613 03-Jan-2019 JY Added bShowMessage
            try
            {
                Logs.Logger("Start sending email to recipient(s) " + toMail);
                Regex expression = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                string words = toMail.ToLower();

                string[] split = words.Split(new Char[] { ',', ';' });
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Configuration.CInfo.OutGoingEmailServer);

                if (expression.IsMatch(Configuration.CInfo.OutGoingEmailID.ToLower()))
                {
                    mail.From = new MailAddress(Configuration.CInfo.OutGoingEmailID.ToLower());
                }
                else
                {
                    if (!bCalledFromScheduler)
                    {
                        if (bShowMessage == true)
                            clsUIHelper.ShowErrorMsg("Invalid sender email id");
                        return false;
                    }
                }
              
                foreach (string emailString in split)
                {
                    if (expression.IsMatch(emailString.Trim()))
                    {
                        if (mail.From == null && bCalledFromScheduler)
                            mail.From = new MailAddress(emailString);
                        mail.To.Add(emailString);
                    }
                    else
                    {
                        if (bShowMessage == true)
                            clsUIHelper.ShowErrorMsg("Invalid recipient email id, please update owners email id on Email Settings screen");
                        return false;
                    }
                }

                SmtpServer.Port = Configuration.CInfo.OutGoingEmailPort;
                SmtpServer.EnableSsl = Configuration.CInfo.OutGoingEmailEnableSSL;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Configuration.CInfo.OutGoingEmailUserID, Configuration.CInfo.OutGoingEmailPass);

                StringBuilder mailBody = new StringBuilder();

                mailBody.AppendFormat("<p>" + body + "</p>");
                //mailBody.AppendFormat("" + Configuration.CInfo.OutGoingEmailSignature + "");

                mail.Subject = subject;
                string htmlBody = mailBody.ToString();
                mail.Body = htmlBody;
                mail.IsBodyHtml = true;
                int i = 1;
                try
                {
                    foreach (ReportClass rptFile in lstReportClass)
                    {
                        if (rptFile != null)
                        {
                            mail.Attachments.Add(new Attachment(rptFile.ExportToStream(ExportFormatType.PortableDocFormat), fileName + i.ToString() + ".pdf"));
                            i++;
                        }
                    }
                }
                catch { }

                SmtpServer.Send(mail);
                Logs.Logger("Email sent successfully to " + toMail);

                if (bShowMessage == true)
                    clsUIHelper.ShowSuccessMsg("Email sent successfully.", "Email...");
                return true;
            }
            catch (Exception ex)
            {
                Logs.Logger("Unable to send email successfully to recipient(s) " + toMail + Environment.NewLine + ex.Message);
                if (bShowMessage == true)
                    clsUIHelper.ShowErrorMsg("Unable to Send email" + Environment.NewLine + ex.Message);
                return false;
            }
        }

        public static bool EmailReport(string subject, string body, string fileName, string toMail)
        {
            try
            {
                Logs.Logger("Start sending email to: " + toMail);
                Regex expression = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                string words = toMail;

                string[] split = words.Split(new Char[] { ',', ';' });
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Configuration.CInfo.OutGoingEmailServer);

                //PRIMEPOS-3042 10-Jan-2022 JY Added
                if (expression.IsMatch(Configuration.CInfo.OutGoingEmailID.ToLower()))
                {
                    mail.From = new MailAddress(Configuration.CInfo.OutGoingEmailID.ToLower());
                }

                foreach (string emailString in split)
                {
                    if (expression.IsMatch(emailString))
                    {
                        if (mail.From == null)
                            mail.From = new MailAddress(emailString);
                        mail.To.Add(emailString);
                    }
                    //else
                    //{
                    //    clsUIHelper.ShowErrorMsg("Invalid Email Format");
                    //    return false;
                    //}
                }

                SmtpServer.Port = Configuration.CInfo.OutGoingEmailPort;
                SmtpServer.EnableSsl = Configuration.CInfo.OutGoingEmailEnableSSL;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Configuration.CInfo.OutGoingEmailUserID, Configuration.CInfo.OutGoingEmailPass);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network; //PRIMEPOS-3042 10-Jan-2022 JY Added
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (fileName != string.Empty)
                    mail.Attachments.Add(new Attachment(fileName));

                SmtpServer.Send(mail);
                Logs.Logger("Email sent successfully to: " + toMail);
                return true;
            }
            catch (Exception ex)
            {
                Logs.Logger("Unable to send email to: " + toMail + Environment.NewLine + ex.Message);
                clsUIHelper.ShowErrorMsg("Unable to send email: " + Environment.NewLine + ex.Message);
                return false;
            }
        }
    }
}