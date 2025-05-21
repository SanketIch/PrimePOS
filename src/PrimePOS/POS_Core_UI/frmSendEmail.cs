using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using POS_Core_UI.Reports.Reports;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.Resources;
using POS_Core.LabelHandler.RxLabel;
//using POS_Core_UI.Reports.ReportsUI;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
    public partial class frmSendEmail : Form
    {
        public long TransId = 0;
        public DateTime Tdate = DateTime.Now;
        string CustName = "";
        public string Tomail = "";
        public string EmailSubject = "";
        public static bool IsCanceled = false;
        rptPrintTransaction oRptPrintTrancsaction = null;
        frmPOSChangeDue ofrmPosChangeDue;
        CustomerRow oCustRow = null;
        public bool bPrintDuplicateReceipt = false;   //PRIMEPOS-2900 15-Sep-2020 JY Added

        public frmSendEmail()
        {
            InitializeComponent();
        }
        public frmSendEmail(long TransacID)
        {

            this.TransId = TransacID;
            InitializeComponent();


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region PRIMEPOS-2560 17-Jul-2018 JY Added to validate email id
                if (!ValidateEmail(txtFromMail.Text.Trim()) || !ValidateToEmailIds())
                {
                    clsUIHelper.ShowErrorMsg("Invalid Email Format");
                    return;
                }
                #endregion

                #region PRIMEPOS-2560 17-Jul-2018 JY Commented
                //string words = txtToEmailID.Text;
                //string[] split = words.Split(new Char[] { ',', ';' });

                //foreach (string emailString in split)
                //{
                //    if (this.txtToEmailID.Text.Trim() == "" || !Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
                //    {
                //        clsUIHelper.ShowErrorMsg("Invalid Email Format");
                //        return;

                //    }
                //}
                #endregion

                string emailSubject = txtSubject.Text.Trim();
                string Emailbody = Configuration.CInfo.OutGoingEmailBody.ToString();
                oRptPrintTrancsaction = CreateTransReceiptForEmail(Configuration.convertNullToInt64(txtTransID.Text.Trim()));
                new System.Threading.Thread(delegate()
                {
                    clsReports.EmailReport(oRptPrintTrancsaction, CustName, txtToEmailID.Text.Trim().ToLower(), emailSubject, txtOutEmailBody.Text.Trim(), " INV" + txtTransID.Text.Trim());
                }).Start();

                IsCanceled = false;
            }
            catch { }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            IsCanceled = true;
            this.Close();
        }

        private void frmSendEmail_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
            this.Cursor = Cursors.WaitCursor;
            if (this.TransId > 0)
            {
                CreateTransReceiptForEmail(TransId);
                this.txtCustName.Enabled = false;
                this.txtTransID.Enabled = false;
                this.Cursor = Cursors.Default;
            }
            txtSubject.Text = EmailSubject;
            txtOutEmailBody.Text = Configuration.CInfo.OutGoingEmailBody;
            txtFromMail.Text = Configuration.CInfo.OutGoingEmailID;
            txtToEmailID.Text = "";
            txtCustName.Text = this.CustName;
            txtTransID.Text = this.TransId.ToString();
            //Added By Shitaljit on 6/2/2104 for PRIMEPOS-1804 Auto Populate Email address from customer
            if (Configuration.CInfo.AutoPopulateCustEmail == true && oCustRow != null && string.IsNullOrEmpty(oCustRow.Email) == false)
            {
                this.txtToEmailID.Text = oCustRow.Email.ToLower();   //PRIMEPOS-2903 06-Oct-2020 JY Modified
            }
            //END
            this.Cursor = Cursors.Default;
            IsCanceled = true;
        }
        string strQuery = "";
        private void PopulateChargeAcct(RxLabel oRxlabel, string CustID)
        {

            try
            {
                try
                {
                    clsReports.setCRTextObjectText("sCustomerName", "Customer Name : " + GetCustomerName(CustID), oRptPrintTrancsaction);
                    //oRptPrintTrancsaction.se("sCustomerName", "Customer Name : " + GetCustomerName(CustID));
                }
                catch (Exception)
                {

                    // throw;
                }
                oRptPrintTrancsaction.SetParameterValue("sAccCode", Configuration.convertNullToString(oRxlabel.AccCode));
                oRptPrintTrancsaction.SetParameterValue("sAccName", Configuration.convertNullToString(oRxlabel.AccName));
                oRptPrintTrancsaction.SetParameterValue("AcctAmount", Configuration.convertNullToString(oRxlabel.AccAmount));

                oRptPrintTrancsaction.SetParameterValue("CurrBalance", Configuration.convertNullToString(oRxlabel.AccCurrBalance));

                if (oRxlabel.TransactionTypeCode == 3)
                {
                    oRptPrintTrancsaction.SetParameterValue("sChargeAccType", "Received On Account :");
                    string hcReference = Configuration.convertNullToString(oRxlabel.HCReference);
                    if (hcReference.Length > 25)
                    {
                        oRptPrintTrancsaction.SetParameterValue("HCReference", hcReference.Substring(0, 25) + "..");
                        if (hcReference.Length > 25)
                        {
                            oRptPrintTrancsaction.SetParameterValue("HCReference2", hcReference.Substring(25));
                        }
                        else
                        {
                            oRptPrintTrancsaction.SetParameterValue("HCReference2", "");
                        }
                    }
                    else
                    {
                        oRptPrintTrancsaction.SetParameterValue("HCReference", hcReference);
                        oRptPrintTrancsaction.SetParameterValue("HCReference2", "");
                    }
                }
                else
                {
                    oRptPrintTrancsaction.SetParameterValue("sChargeAccType", "Amount Charged :");
                    oRptPrintTrancsaction.SetParameterValue("HCReference", "");
                    oRptPrintTrancsaction.SetParameterValue("HCReference2", "");
                }

            }
            catch { }
        }
        string CurrentInvoiceNo;
        string strWhere;
        bool misROATrans = false;
        private void ClearSubReportPearameters(RxLabel oRxlabel)
        {

            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Pharmacyname_.ParameterFieldName, POS_Core.Resources.Configuration.CInfo.StoreName);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_PhramecyAddress.ParameterFieldName, oRxlabel.CAddress);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_txtPharmacyCityStateZip.ParameterFieldName, Configuration.convertNullToString(oRxlabel.CCity) + ", " + Configuration.convertNullToString(oRxlabel.CState) + " " + Configuration.convertNullToString(oRxlabel.CZip));  //PRIMEPOS-2560 16-Jul-2018 JY Added
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TelephoneNo.ParameterFieldName, oRxlabel.CTelephone);

            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_MasterCard.ParameterFieldName, "");
            //Added By SRT(Ritesh Parekh) Date : 25-Jul-2009
            //oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_rptViewTransPayment_PayTypeDesc.ParameterFieldName, "Card Type :");
            //End Of Added By SRT(Ritesh Parekh)
            oRptPrintTrancsaction.SetParameterValue("MasterCard", "");
            oRptPrintTrancsaction.SetParameterValue("Charge", "0");
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Merchant.ParameterFieldName, "0");
            oRptPrintTrancsaction.SetParameterValue("Merchant", "0");
            oRptPrintTrancsaction.SetParameterValue("Authority", "0");
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotlaAmount.ParameterFieldName, "0");
            oRptPrintTrancsaction.ReportFooterSection2.SectionFormat.EnableSuppress = true;

        }
        private string GetCustomerName(string CstID)
        {
            Customer oCustomer = new Customer();
            CustomerData oCustdata = new CustomerData();

            try
            {
                oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(CstID));
                if (oCustdata.Tables[0].Rows.Count > 0)
                {
                    oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                    CustName = oCustRow.CustomerName + " " + oCustRow.FirstName;
                    this.Tomail = oCustRow.Email.ToLower();   //PRIMEPOS-2903 06-Oct-2020 JY Modified
                    this.CustName = " " + oCustRow.CustomerName + " " + oCustRow.FirstName;

                    return oCustRow.CustomerName + " " + oCustRow.FirstName;

                }
                return "";
            }

            catch (Exception)
            {
                return "";
                //throw;
            }
        }

        private void PrintPreview(long tranceno)
        {
            try
            {

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                //rptViewTransaction oRptViewTrans= new rptViewTransaction();

                Search oSearch = new Search();
                strCQuery = strQuery + strWhere;
                DataSet ds = oSearch.SearchData(strCQuery);
                //oRptViewTrans.Database.Tables[0].SetDataSource(ds.Tables[0]);
                //Following Commented part UnCommented by Krishna on 17 May 2011
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        CurrentInvoiceNo = tranceno.ToString();
                        //Added By Shitaljit for ROA transaction.
                        if (POS_Core.Resources.Configuration.convertNullToString(ds.Tables[0].Rows[0]["TransType"].ToString().Trim()).Equals("ROA"))
                        {
                            misROATrans = true;
                        }
                    }
                    else
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return;
                    }
                }
                //Comments removed till here by krishna

                DataSet subDs = oSearch.SearchData(strSubQuery + " and PT.TransID=" + CurrentInvoiceNo);
                #region Populate Values
                Customer oCustomer = new Customer();
                CustomerData oCustdata = new CustomerData();
                CustomerRow oCustRow = null;

                oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(ds.Tables[0].Rows[0]["CustomerID"].ToString().Trim()));
                if (oCustdata.Tables[0].Rows.Count > 0)
                {
                    oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];

                }



                int RowIndex = 0;
                DataSet oDataSet = new DataSet();
                oDataSet.Tables.Add("TransDetail");
                oDataSet.Tables[0].Columns.Add("Qty", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("ItemDescription", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("Price", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("Discount", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("TaxCode", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("TaxAmount", Type.GetType("System.String"));
                oDataSet.Tables[0].Columns.Add("ExtendedPrice", Type.GetType("System.String"));

                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    DataRow drTranDetRow = oDataSet.Tables[0].NewRow();
                    drTranDetRow[0] = Configuration.convertNullToString(oRow["Qty"]);
                    drTranDetRow[1] = Configuration.convertNullToString(oRow["Description"]);
                    drTranDetRow[2] = Configuration.convertNullToDecimal(oRow["Price"]).ToString("########0.00");
                    drTranDetRow[3] = Configuration.convertNullToDecimal(oRow["Discount"]).ToString("########0.00");
                    drTranDetRow[4] = Configuration.convertNullToString(oRow["TaxID"]);
                    drTranDetRow[5] = Configuration.convertNullToDecimal(oRow["TaxAmount"]).ToString("########0.00");
                    drTranDetRow[6] = Configuration.convertNullToDecimal(oRow["ExtendedPrice"]).ToString("########0.00");
                    oDataSet.Tables[0].Rows.Add(drTranDetRow);
                    RowIndex++;

                }
                //FillListView(subDs);

                //MaxInvoiceNo = oTHSvr.GetMaxTransId();


                #endregion

                #region Commented Code



                #endregion
                strWhere = "";


            }
            catch (Exception exp)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        private void PrintCC(RxLabel oRxlabel)
        {
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Pharmacyname_.ParameterFieldName, POS_Core.Resources.Configuration.CInfo.StoreName);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_PhramecyAddress.ParameterFieldName, oRxlabel.CAddress);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_txtPharmacyCityStateZip.ParameterFieldName, Configuration.convertNullToString(oRxlabel.CCity) + ", " + Configuration.convertNullToString(oRxlabel.CState) + " " + Configuration.convertNullToString(oRxlabel.CZip));  //PRIMEPOS-2560 16-Jul-2018 JY Added
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TelephoneNo.ParameterFieldName, oRxlabel.CTelephone);
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_MasterCard.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.RefNo.ToString()));
            //Added By SRT(Ritesh Parekh) Date : 25-Jul-2009
            //oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_rptViewTransPayment_PayTypeDesc.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "Card:" : oRxlabel.CCPaymentRow.TransTypeDesc.ToString()+":"));
            //End OF Added By SRT(Ritesh Parekh)
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Charge.ParameterFieldName, Convert.ToString(oRxlabel.TransH(0).TenderedAmount - oRxlabel.TransH(0).TotalPaid));
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Merchant.ParameterFieldName, (oRxlabel.CMerchantNo == null ? "" : oRxlabel.CMerchantNo));
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_Authority.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.AuthNo.ToString()));
            oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotlaAmount.ParameterFieldName, (oRxlabel.CCPaymentRow == null ? "" : oRxlabel.CCPaymentRow.Amount.ToString()));

            oRptPrintTrancsaction.ReportFooterSection2.SectionFormat.EnableSuppress = false;
        }
        //((CrystalDecisions.CrystalReports.Engine.TextObject)oRptViewTrans.ReportDefinition.ReportObjects["txtHeader"]).Text = Configuration.CInfo.StoreName;
        string strCQuery = "";
        string strSubQuery = "";
        public rptPrintTransaction CreateTransReceiptForEmail(long TransID)
        {
            //PRIMEPOS-2560 17-Jul-2018 JY Added CustName
            oRptPrintTrancsaction = new rptPrintTransaction();
            strQuery = " select PT.TransID,PT.TransDate,PT.CustomerID,PT.UserID,PT.StationID,PT.GrossTotal,PT.TotalDiscAmount, PT.TotalTaxAmount, PT.TotalPaid ," +
                           " Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType, " +
                           " PTD.Qty,PTD.ItemID, PTD.ItemDescription as Description, PTD.Price,Tx.TaxCode as TaxID,PTD.TaxAmount,PTD.Discount,PTD.ExtendedPrice,ps.stationname, " +
                           " (CustomerName + ', ' + FIRSTNAME) AS CustName, PT.TotalTransFeeAmt " + //PRIMEPOS-3119 11-Aug-2022 JY Added TotalTransFeeAmt
                           " from postransaction PT left join POSTransactionDetail PTD on PT.TransID=PTD.TransID " +
                           " left join Item I on I.ItemID=PTD.ItemID  " +
                           " left join taxcodes tx on (ptd.taxid=tx.taxid) " +
                           " left join util_POSSet ps on (ps.stationid=pt.stationid) " +
                           " LEFT JOIN Customer C ON c.CustomerID = PT.CustomerID " + 
                           " where 1=1 ";   //and PT.StationID='" + StationID.Trim() + "'";

            //Modified The Sub Query By shitaljit To Dispaly Payments of singgle Paytype togethr in single Row.
            strSubQuery = "SELECT PAY.PAYTYPEDESC AS DESCRIPTION,cast(PT.TransAmt as varchar) as TransAmt, " +
                        " case CHARINDEX('|',isnull(refno,'')) when  0 then refno else '******'+right(rtrim(left(refno,CHARINDEX('|',refno)-1)) ,4) End as RefNo ,PT.TRANSID " +
                // " FROM POSTRANSPAYMENT PT,PAYTYPE PAY where PT.TransTypeCode=Pay.PayTypeID ";
                        " FROM PAYTYPE PAY, (Select RefNo,Transid, TransTypeCode, sum(POSTransPayment.Amount) " +
                        "as TransAmt from POSTRANSPAYMENT  GROUP BY TransTypeCode,Transid,RefNo)  as PT WHERE PT.TransTypeCode = Pay.PayTypeID";


            if (TransID.ToString().Trim() != "" && TransID.ToString().Trim() != "0")
                strWhere = " and PT.TransID=" + TransID.ToString().Trim();

            PrintPreview(TransID);
            int custID = 0;

            try
            {
                try
                {
                    if (TransID < 1)
                    {
                        return null;
                    }

                    TransHeaderData oTransHData;
                    TransHeaderSvr oTransHSvr = new TransHeaderSvr();

                    TransDetailData oTransDData;
                    TransDetailSvr oTransDSvr = new TransDetailSvr();

                    POSTransPaymentData oTransPaymentData;
                    POSTransPaymentSvr oTransPaymentSvr = new POSTransPaymentSvr();

                    //added by atul 07-jan-2011
                    TransDetailRXData oTransRxData;
                    TransDetailRXSvr oTransRxSvr = new TransDetailRXSvr();
                    //End of added by atul 07-jan-2011

                    oTransHData = oTransHSvr.Populate(Configuration.convertNullToInt(TransID.ToString()));
                    if (oTransHData.TransHeader.Rows.Count > 0)
                    {
                        DateTime TransDate = Convert.ToDateTime(oTransHData.TransHeader.Rows[0][clsPOSDBConstants.TransHeader_Fld_TransDate].ToString());
                        this.Tdate = TransDate;
                        custID = Configuration.convertNullToInt(oTransHData.TransHeader.Rows[0][clsPOSDBConstants.TransHeader_Fld_CustomerID].ToString());
                        GetCustomerName(custID.ToString());

                    }
                    oTransDData = oTransDSvr.PopulateData(Configuration.convertNullToInt(TransID.ToString()));

                    oTransPaymentData = oTransPaymentSvr.Populate(Configuration.convertNullToInt(TransID.ToString()));

                    TransDetailTaxSvr oTransDetailTaxSvr = new TransDetailTaxSvr(); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added
                    DataTable dtTransDetailTax = oTransDetailTaxSvr.GetTransDetailTax(Configuration.convertNullToInt(TransID.ToString())); //Sprint-26 - PRIMEPOS-2445 28-Aug-2017 JY Added

                    //added by atul 07-jan-2011
                    RxLabel oRxLabel;
                    if (oTransDData != null && oTransDData.Tables[0].Rows.Count > 0)
                    {
                        //if (oTransDData.TransDetail[0].ItemID.ToString().Trim().ToUpper() == "RX")    //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented as its checking only first item
                        oTransRxData = oTransRxSvr.PopulateData(Configuration.convertNullToInt(TransID.ToString()));    //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added
                        if (oTransRxData != null && oTransRxData.Tables.Count > 0 && oTransRxData.Tables[0].Rows.Count > 0)  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Added condtion to check whether rx item exists in transaction
                        {
                            //oTransRxData = oTransRxSvr.PopulateData(Configuration.convertNullToInt(TransID.ToString()));  //Sprint-23 - PRIMEPOS-2319 24-Jun-2016 JY Commented
                            oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, oTransRxData, ReceiptType.Void, dtTransDetailTax, bPrintDuplicateReceipt);  //PRIMEPOS-2900 15-Sep-2020 JY Added bPrintDuplicateReceipt
                        } //end of added by atul 07-jan-2011
                        else
                        {
                            oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, bPrintDuplicateReceipt); //PRIMEPOS-2900 15-Sep-2020 JY Added bPrintDuplicateReceipt
                        }
                    }
                    else
                    {
                        oRxLabel = new RxLabel(oTransHData, oTransDData, oTransPaymentData, ReceiptType.SalesTransactionReprint, dtTransDetailTax, bPrintDuplicateReceipt); //PRIMEPOS-2900 15-Sep-2020 JY Added bPrintDuplicateReceipt
                    }

                    oRptPrintTrancsaction = new rptPrintTransaction();
                    Search oSearch = new Search();
                    DataSet ds = oSearch.SearchData(strCQuery);


                    ds.Tables[0].Columns.Add("CustomerSignature", typeof(System.Byte[]));
                    int index = 0;
                    DataTable oChargeAccTab = new DataTable();
                    oChargeAccTab.Columns.Add("TransID", System.Type.GetType("System.Int32"));
                    oChargeAccTab.Columns.Add("AcctSignature", typeof(System.Byte[]));

                    oTransPaymentData.Tables[0].Columns.Add("CustomerSignature", typeof(System.Byte[]));
                    oTransPaymentData.Tables[0].Columns.Add("UserID", typeof(System.String));
                    oTransPaymentData.Tables[0].Columns.Add("TransType", typeof(System.String));
                    oTransPaymentData.Tables[0].Columns.Add("RestrictSignatureLineAndWordingOnReceipt", typeof(System.String));    //PRIMEPOS-2910 29-Oct-2020 JY Added

                    //**************
                    Bitmap bit = null;
                    System.IO.MemoryStream oStream = null;
                    //*************

                    foreach (DataRow oRow in ds.Tables[0].Rows)
                    {
                        try
                        {
                            if (oTransPaymentData.Tables[0].Rows[0]["BinarySign"] != System.DBNull.Value) //Added by prashant(SRT) 28-sep-2010
                            {
                                //MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                                //bit = new Bitmap(ms);

                                #region PRIMEPOS-2900 15-Sep-2020 JY Added if part for Vantiv
                                try
                                {
                                    if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") || (bPrintDuplicateReceipt && Configuration.convertNullToString(oTransPaymentData.Tables[0].Rows[0][clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor]).Trim().ToUpper() == "VANTIV"))
                                    {
                                        Byte[] strBinSign = (byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"];
                                        bit = POS_Core.Resources.DelegateHandler.clsCoreUIHelper.ConvertPoints(strBinSign);
                                    }
                                    else
                                    {
                                        MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                                        bit = new Bitmap(ms);
                                    }
                                }
                                catch { }
                                #endregion
                            }
                            else
                            {
                                bit = clsUIHelper.GetSignature(oTransPaymentData.Tables[0].Rows[0]["CustomerSign"].ToString(), POS_Core.Resources.Configuration.CInfo.SigType);
                            }
                            oStream = new System.IO.MemoryStream();
                            if (bit != null)
                            {
                                bit.Save(oStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                oRow["CustomerSignature"] = oStream.ToArray();
                            }
                            oRow["Description"] = oRxLabel.TransD(index).ItemDescription;
                            index++;

                        }
                        catch { }
                    }

                    foreach (DataRow oRow in oTransPaymentData.POSTransPayment.Rows)
                    {
                        oRow["TransType"] = ds.Tables[0].Rows[0]["TransType"].ToString();
                        oRow["UserID"] = ds.Tables[0].Rows[0]["UserID"].ToString();
                        oRow["RestrictSignatureLineAndWordingOnReceipt"] = (Configuration.convertNullToBoolean(Configuration.CSetting.RestrictSignatureLineAndWordingOnReceipt) == false ? "0" : "1");  //PRIMEPOS-2910 29-Oct-2020 JY Added

                        switch (oRow[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString().Trim())
                        {
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "H":
                                if (oRow["BinarySign"] != System.DBNull.Value) //Added by prashant(SRT) 28-sep-2010
                                {
                                    //MemoryStream ms = new MemoryStream((byte[])oTransPaymentData.Tables[0].Rows[0]["BinarySign"]);
                                    //bit = new Bitmap(ms);

                                    #region PRIMEPOS-2900 15-Sep-2020 JY Added if part for Vantiv
                                    try
                                    {
                                        if ((!bPrintDuplicateReceipt && Configuration.CPOSSet.PaymentProcessor.ToUpper() == "VANTIV") || (bPrintDuplicateReceipt && Configuration.convertNullToString(oRow[clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor]).Trim().ToUpper() == "VANTIV"))
                                        {
                                            Byte[] strBinSign = (byte[])oRow["BinarySign"];
                                            bit = POS_Core.Resources.DelegateHandler.clsCoreUIHelper.ConvertPoints(strBinSign);
                                        }
                                        else
                                        {
                                            MemoryStream ms = new MemoryStream((byte[])oRow["BinarySign"]);
                                            bit = new Bitmap(ms);
                                        }
                                    }
                                    catch { }
                                    #endregion
                                }
                                else
                                {
                                    bit = clsUIHelper.GetSignature(oRow[clsPOSDBConstants.POSTransPayment_Fld_CustomerSign].ToString(), POS_Core.Resources.Configuration.CInfo.SigType);
                                }

                                oStream = new System.IO.MemoryStream();
                                if (bit != null)
                                {
                                    bit.Save(oStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    oRow["CustomerSignature"] = oStream.ToArray();
                                }
                                if (oRow[clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode].ToString().Trim() == "H")
                                {
                                    DataRow oCRow = oChargeAccTab.NewRow();
                                    oCRow["TransID"] = oRow[clsPOSDBConstants.POSTransPayment_Fld_TransID].ToString();
                                    if (oStream != null)
                                        oCRow["AcctSignature"] = oStream.ToArray();
                                    oChargeAccTab.Rows.Add(oCRow);
                                }
                                break;
                        }
                    }

                    oRptPrintTrancsaction.ReportOptions.EnableSaveDataWithReport = false;
                    oRptPrintTrancsaction.Database.Tables[0].SetDataSource(ds.Tables[0]);
                    oRptPrintTrancsaction.Database.Tables[1].SetDataSource(oChargeAccTab);
                    oRptPrintTrancsaction.Subreports["rptPaymentDetail"].Database.Tables[0].SetDataSource(oTransPaymentData.Tables[0]);
                    oRptPrintTrancsaction.Subreports["rptViewTransPayment"].Database.Tables[0].SetDataSource(oTransPaymentData.Tables[0]);

                    if (oRxLabel.AccCode.Trim() == "")
                    {
                        oRptPrintTrancsaction.ReportFooterSection3.SectionFormat.EnableSuppress = true;
                    }
                    else
                    {
                        oRptPrintTrancsaction.ReportFooterSection3.SectionFormat.EnableSuppress = false;
                    }

                    PopulateChargeAcct(oRxLabel, custID.ToString());
                    ClearSubReportPearameters(oRxLabel);

                    if (oRxLabel.MergeCCWithRecpt == true)
                    {
                        if (oRxLabel.CCPaymentRow == null || oRxLabel.CCPaymentRow.RefNo.ToString() == "")
                        {
                            ClearSubReportPearameters(oRxLabel);
                        }
                        else
                        {
                            {
                                PrintCC(oRxLabel); //add compies of pos credit card transcation report
                            }
                        }
                    }
                    else
                    {
                        ClearSubReportPearameters(oRxLabel);
                        //oRptPrintTrancsaction.PrintToPrinter(1, false, 1, 1);           
                    }
                    this.EmailSubject = "Invoice : Transaction No:" + TransId + " Purchase Date: " + Tdate.ToString();
                    oRptPrintTrancsaction.SetParameterValue(oRptPrintTrancsaction.Parameter_TotalQuantity.ParameterFieldName, oRxLabel.TotalQty.ToString());
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
            }
            catch { }
            return oRptPrintTrancsaction;
        }

        private void txtToEmailID_Leave(object sender, EventArgs e)
        {

        }

        private void frmSendEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                IsCanceled = true;
                this.Close();

            } if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, true);
                e.Handled = true;
            }
        }

        #region PRIMEPOS-2560 17-Jul-2018 JY Added to validate email id
        private Boolean ValidateToEmailIds()
        {           
            Boolean bStatus = true;
            txtToEmailID.Text = txtToEmailID.Text.Trim().ToLower(); //PRIMEPOS-2903 06-Oct-2020 JY Added
            Application.DoEvents();
            if (txtToEmailID.Text.Trim() != "")
            {
                string[] arrEmailIds = txtToEmailID.Text.Trim().Split(new Char[] { ',', ';' });
                foreach (string emailString in arrEmailIds)
                {
                    if (!ValidateEmail(emailString))
                        return false;
                }
            }
            else
            {
                bStatus = false;
            }

            return bStatus;
        }

        private bool ValidateEmail(string emailAddress)
        {
            //string regexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            string regexPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            Match matches = Regex.Match(emailAddress, regexPattern);
            return matches.Success;
        }
        #endregion
    }
}
