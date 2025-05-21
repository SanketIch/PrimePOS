using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
//using POS_Core_UI.DataAccess;
using POS_Core.Resources;
using POS_Core_UI.UI;
using POS_Core.ErrorLogging;
//using POS_Core.TaxType;
using POS_Core.Resources.DelegateHandler;
using POS_Core.LabelHandler;

namespace POS_Core_UI.Reports.ReportsUI
{
    public class BarcodeLabel
    {
        protected BarcodeLabelDataCollection oBarcodeDataColl;

        string sLabelFile = "ItemBarCodeLabel.js";
        private System.Drawing.Font CFont;
        private System.Drawing.Brush CBrush;

        private int iLabelWidth = 250;
        private int iLabelHeight = 200;

        private int loopCounter = 0;

        private System.Drawing.Printing.PrintDocument oPrintDocument;

        public int LabelWidth
        {
            set
            {
                iLabelWidth = value;
                oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
            }
        }

        public int LabelHeight
        {
            set
            {
                iLabelHeight = value;
                oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
            }
        }

        public PrintDocument PrintDocument
        {
            get { return oPrintDocument; }
        }

        public BarcodeLabel(BarcodeLabelDataCollection oBarcodeColl)
        {
            this.oBarcodeDataColl = oBarcodeColl;
            // TODO: Add constructor logic here
            //
            InitClass();
        }

        public BarcodeLabelData CurrentBarcodeLabelData
        {
            get
            {
                try
                {
                    return this.oBarcodeDataColl[loopCounter];
                }
                catch
                { return null; }
            }
        }

        private Graphics printerHandle = null;

        private void InitClass()
        {
            this.CFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
            this.CBrush = new System.Drawing.SolidBrush(Color.Black);
        }

        public void Print()
        {
            // this routine prints the dot matrix label

            if (System.IO.File.Exists(sLabelFile))
            {
                bool bSuccess = false;
                if (Configuration.CPOSSet.LabelPrinter.Trim() == "")
                {
                    return;
                }

                try
                {
                    oPrintDocument = new PrintDocument();
                    oPrintDocument.DefaultPageSettings.PrinterSettings.PrinterName = Configuration.CPOSSet.LabelPrinter;
                    #region PRIMEPOS-2996 23-Sep-2021 JY Added
                    try
                    {
                        if (Configuration.CPOSSet.LabelPrinterPaperSource.Trim() != "")
                        {
                            System.Drawing.Printing.PrinterSettings oPrinterSettings = new System.Drawing.Printing.PrinterSettings();
                            foreach (System.Drawing.Printing.PaperSource ps in oPrintDocument.PrinterSettings.PaperSources)
                            {
                                if (ps.SourceName.Trim().ToUpper() == Configuration.CPOSSet.LabelPrinterPaperSource.Trim().ToUpper())
                                {
                                    oPrintDocument.DefaultPageSettings.PaperSource = ps;
                                    break;
                                }
                            }
                        }
                    }
                    catch { }
                    #endregion
                    //oPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom Paper Size", iLabelWidth, iLabelHeight);
                    //oPrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                    oPrintDocument.PrintPage += new PrintPageEventHandler(oPrintDocument_PrintPage);

                    /*PrintPreviewDialog pd = new PrintPreviewDialog();
                    pd.Document = oPrintDocument;

                    pd.ShowDialog();
                    return;
                    */

                    loopCounter = 0;
                    for (; loopCounter < oBarcodeDataColl.Count; loopCounter++)
                    {
                        oPrintDocument.Print();
                    }
                    //}
                    bSuccess = true;
                }
                catch (Exception e)
                {
                    clsCoreUIHelper.ShowBtnIconMsg(e.Message.ToString(), "ERROR Printing Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void oPrintDocument_PrintPage(object sender, PrintPageEventArgs e) // NileshJ - Change access modifier private to public
        {
            printerHandle = e.Graphics;
            ScriptableLabel _lbl = new ScriptableLabel(this, sLabelFile);
            _lbl.PrintCLabel();   // this will actually send the label printing lines
        }

        public void SetFont(string fontName, long fontSize, bool bold)
        {
            if (bold == true)
            {
                this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Bold);
            }
            else
            {
                this.CFont = new System.Drawing.Font(fontName, fontSize, FontStyle.Regular);
            }
        }

        public void SetFont(string fontName, long fontSize)
        {
            SetFont(fontName, fontSize, false);
        }

        public void PrintLine(string printData, int iLeft, int iTop)
        {
            this.printerHandle.DrawString(printData, CFont, CBrush, iLeft, iTop, new StringFormat());
        }

        public void PrintImage(Image oImage, int iLeft, int iTop, int iWidth, int iHeight)
        {
            this.printerHandle.DrawImage(oImage, iLeft, iTop, iWidth, iHeight);
        }
    }

    //public class BarcodeLabelDataCollection : System.Collections.Generic.List<BarcodeLabelData>
    //{
    //}

  
}
