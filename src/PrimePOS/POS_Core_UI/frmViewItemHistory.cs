using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
//using POS.UI;
using POS_Core.ErrorLogging;

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmViewHistory : Form
    {
        #region PrivateConstant
        private const string QUANTITYANALYSIS = "Quantity Analysis";
        private const string PROFITANALYSIS = "Profit Analysis";
        private const string ITEMORDEREDPERMONTH = "Item Ordered Per Vendor";
        private const string ORDER_MONTH  = "Order Month";
        
        private const string QUANTITY_ORDERED = "QtyOrdered";
        private const string QUANTITY_SOLD = "QtySold";

        private const string AVERAGECOST_PRICE = "AverageCostPrice";
        private const string AVERAGESELLING_PRICE = "AverageSellingPrice";
        #endregion

        private string upcCode = string.Empty;
        private string description = string.Empty;
        Item item = null;
        ItemData itemData = null;

        public bool CanShowItemHistory
        {
            get
            {
                if (itemData != null && itemData.Tables.Count > 0 && itemData.Tables[0].Rows.Count > 0)
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
        }

        public frmViewHistory(string itemCode)
        {
            InitializeComponent();
            item = new Item();
            itemData = item.Populate(itemCode);
            if (itemData != null && itemData.Tables.Count > 0 && itemData.Tables[0].Rows.Count > 0)
            {
                description = itemData.Item[0].Description;
            }
            upcCode = itemCode;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewHistory_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);
        }

        private void QuantityOrderedPerVendorPerMonth()
        {
            DataSet ds = new DataSet();
            Search search = new Search();
            DataSet dsForVendor = new DataSet();
           
            try
            {
                string queryForVendor = "Select VendorId,VendorName from Vendor";
                dsForVendor = search.SearchData(queryForVendor);
                DataTable dataTable = CreateTable(dsForVendor);
                ds.Merge(dataTable);
                ds = SetOrderMonth(ds);

                foreach (DataRow rw in dsForVendor.Tables[0].Rows)
                {
                    string vendorName = (rw["VendorName"].ToString()).Substring(0,3);

                    string queryToFetchOrderCount = "Select Convert(VarChar(3),DateName(Month,OrderDate)) + ' ' + Convert(VarChar(4), Year(OrderDate)) as OrderMonth , SUM(PurchaseOrderDetail.Qty) as QtyOrdered , Vendor.VendorName " +
                                         " from PurchaseOrder ,PurchaseOrderDetail,Vendor,Item where Item.ItemID = PurchaseOrderDetail.ItemID AND PurchaseOrder.OrderID = PurchaseOrderDetail.OrderID " +
                                         " And PurchaseOrder.VendorID = Vendor.VendorID AND Item.ItemID ='" + upcCode + "' AND OrderDate Between Convert(varchar(20),GetDate()-365,101) AND Convert(varchar(20),GetDate(),101) AND Vendor.VendorName = '" + vendorName + "' group by Convert(VarChar(3),DateName(Month,OrderDate)) + ' ' + Convert(VarChar(4), Year(OrderDate)),Vendor.VendorName ";

                    DataSet poData = search.SearchData(queryToFetchOrderCount);
                
                    if(poData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in poData.Tables[0].Rows)
                        {
                            DataRow[] dsData = ds.Tables[0].Select("[Order Month]='" + row[0].ToString() + "'");
                            if (dsData.Length > 0)
                                dsData[0][vendorName] = row["QtyOrdered"].ToString();
                            else
                            {
                                DataRow newDsRow = ds.Tables[0].NewRow();
                                newDsRow[vendorName] = row["QtyOrdered"].ToString();
                                newDsRow["Order Month"] = row["OrderMonth"].ToString();
                                ds.Tables[0].Rows.Add(newDsRow);
                            }
                        }
                    }
                }

                if(description != string.Empty)
                  ultraChartItmOrdPerVend.TitleTop.Text = "Item - " + description + " ( " + upcCode + " )";
                else
                  ultraChartItmOrdPerVend.TitleTop.Text = "Item - "+ upcCode ; 

                ultraChartItmOrdPerVend.Axis.X.RangeMin = 0;
                ultraChartItmOrdPerVend.Axis.X.RangeMax = 40;

                ultraChartItmOrdPerVend.Axis.Y.RangeMin = 0;                
                double maxValue = GetMaxValueQuantVendMonth(ds);
                ultraChartItmOrdPerVend.Axis.Y.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;
                
                if(maxValue != 0.00)
                   ultraChartItmOrdPerVend.Axis.Y.RangeMax = maxValue;                    
                else
                   ultraChartItmOrdPerVend.Axis.Y.RangeMax = 10.00;
                
                ultraChartItmOrdPerVend.DataSource = ds;
                ultraChartItmOrdPerVend.DataBind();
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }            
        }

        private double GetMaxValueQuantVendMonth(DataSet ds)
        {
            int count = 1;
            double maxQuantOrded = 0.00;
            double maxValue = 0.00;

            try
            {
                int columnCount=ds.Tables[0].Columns.Count;
            
                for(count = 1; count < ds.Tables[0].Columns.Count; count++)
                {
                   string coulomnName = ds.Tables[0].Columns[count].Caption.ToString();
                   DataRow[] rowQuantOrd =  ds.Tables[0].Select(coulomnName+"=MAX("+coulomnName+")");
                   maxQuantOrded = Convert.ToDouble(rowQuantOrd[0].ItemArray[count].ToString());

                   if(maxQuantOrded > maxValue)
                      maxValue = maxQuantOrded;
                   else if (maxQuantOrded == maxValue)
                      maxValue = maxQuantOrded;                  
                }
            }
            catch (Exception ex)
            {
             POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
            return maxValue;
        }

        private DataTable CreateTable(DataSet dsForVendor)
        {
            DataTable oTable = new DataTable();
            DataColumn oCol1 = oTable.Columns.Add("Order Month");

            if (dsForVendor.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsForVendor.Tables[0].Rows)
                {
                    DataColumn col = oTable.Columns.Add((row["VendorName"].ToString()).Substring(0,3));
                    col.DataType = System.Type.GetType("System.Int32");
                }
            }
            return oTable;
        }

        private void QuantityAnalysis()
        {
            DataSet ds = new DataSet();
            Search search = new Search();
            
            DataSet dsForItemSold = new DataSet();
            DataSet dsForItemOrdered = new DataSet();

            try
            {
                DataTable oTable = new DataTable();
                oTable.Columns.Add("Order Month");
                oTable.Columns.Add(QUANTITY_ORDERED,typeof(System.Int32));
                oTable.Columns.Add(QUANTITY_SOLD,typeof(System.Int32));
                ds.Merge(oTable);
                ds = SetOrderMonth(ds);

                string queryForItemOrdered = "Select  Item.ItemID , SUM(PurchaseOrderDetail.Qty) as QtyOrdered ,Convert(VarChar(3),DateName(Month,OrderDate)) + ' ' + Convert(VarChar(4), Year(OrderDate)) as POSTransactionDetailDate from PurchaseOrderDetail,Item ,PurchaseOrder " +
                                             " where Item.ItemID = PurchaseOrderDetail.ItemID AND PurchaseOrder.OrderID = PurchaseOrderDetail.OrderID AND Item.ItemID  ='" + upcCode + "' AND OrderDate Between Convert(varchar(20),GetDate()-365,101) AND Convert(varchar(20),GetDate(),101) group by Item.ItemID ,Convert(VarChar(3),DateName(Month,OrderDate)) + ' ' + Convert(VarChar(4), Year(OrderDate)) order by  Convert(VarChar(3),DateName(Month,OrderDate)) + ' ' + Convert(VarChar(4), Year(OrderDate)) Asc";
          
                dsForItemOrdered = search.SearchData(queryForItemOrdered);
                foreach (DataRow row in dsForItemOrdered.Tables[0].Rows)
                {
                    foreach (DataRow dsRow in ds.Tables[0].Rows)
                    {
                        if(dsRow[ORDER_MONTH].ToString() == row["POSTransactionDetailDate"].ToString())
                        {
                            dsRow[QUANTITY_ORDERED] = row[QUANTITY_ORDERED].ToString();
                        }
                    }
                }

                string queryForItemSold = "Select Item.ItemID ,SUM(POSTransactionDetail.Qty) as QtySold ,Convert(VarChar(3),DateName(Month,TransDate)) + ' ' + Convert(VarChar(4), Year(TransDate)) as TransactionDetailDate from Item,POSTransactionDetail , POSTransaction " +
                                          "where Item.ItemID = POSTransactionDetail.ItemID AND POSTransaction.TransID = POSTransactionDetail.TransID AND Item.ItemID ='" + upcCode + "' AND  TransDate Between Convert(varchar(20),GetDate()-365,101) AND  Convert(varchar(20),GetDate(),101) group by Item.ItemID ,Convert(VarChar(3),DateName(Month,TransDate)) + ' ' + Convert(VarChar(4), Year(TransDate)) order by Convert(VarChar(3),DateName(Month,TransDate)) + ' ' + Convert(VarChar(4), Year(TransDate))  Desc";
               
                dsForItemSold = search.SearchData(queryForItemSold);
               
                foreach (DataRow row in dsForItemSold.Tables[0].Rows)
                {
                    foreach (DataRow dsRow in ds.Tables[0].Rows)
                    {
                        if (dsRow[ORDER_MONTH].ToString() == row["TransactionDetailDate"].ToString())
                        {
                            dsRow[QUANTITY_SOLD] = row[QUANTITY_SOLD].ToString();
                        }
                    }
                }                
             
                if (description != string.Empty)
                    ultraChartForQuantity.TitleTop.Text = "Quantity Analysis - " + description  +" ( " + upcCode + " )";
                else
                    ultraChartForQuantity.TitleTop.Text = "Quantity Analysis - " + upcCode; 

                ultraChartForQuantity.Axis.X.RangeMin = 0;
                ultraChartForQuantity.Axis.X.RangeMax = 13;

                ultraChartForQuantity.Axis.Y.RangeMin = 0;
                ultraChartForQuantity.Axis.Y.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;

                double maxValue = GetMaxValueForQuantAnalysis(ds);
                if (maxValue != 0.00)
                {
                    ultraChartForQuantity.Axis.Y.RangeMax = maxValue;
                    ultraChartForQuantity.Axis.Y.NumericAxisType = Infragistics.UltraChart.Shared.Styles.NumericAxisType.Linear;
                }
                else
                    ultraChartForQuantity.Axis.Y.RangeMax = 10.00;

                ultraChartForQuantity.DataSource = ds;                
                ultraChartForQuantity.DataBind();              
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            } 
        }

        private double GetMaxValueForQuantAnalysis(DataSet ds)
        {
            double maxQuantOrded = 0.00;
            double maxQuantSold = 0.00;
            double maxValue = 0.00;

            try
            {
                DataRow[] rowQuantOrd = ds.Tables[0].Select("QtyOrdered=MAX(QtyOrdered)");
                maxQuantOrded = Convert.ToDouble(rowQuantOrd[0].ItemArray[1].ToString());
                DataRow[] rowQuantSold = ds.Tables[0].Select("QtySold=MAX(QtySold)");
                maxQuantSold = Convert.ToDouble(rowQuantSold[0].ItemArray[2].ToString());

                if (maxQuantOrded > maxQuantSold)
                     maxValue = maxQuantOrded;
                else if (maxQuantSold > maxQuantOrded)
                     maxValue = maxQuantSold;
                else if (maxQuantOrded == maxQuantSold)
                     maxValue = maxQuantSold;
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");                
            }
            return maxValue;
        }

        private void ProfitAnalysisPerMonth()
        {
           DataSet ds = new DataSet();
           Search search = new Search();

           DataSet dsForAverageSellingPrice = new DataSet();
           DataSet dsForAverageCostPrice = new DataSet();
            
           try
           {
               DataTable oTable = new DataTable();
               oTable.Columns.Add("Order Month");
               oTable.Columns.Add(AVERAGECOST_PRICE, typeof(System.Decimal));
               oTable.Columns.Add(AVERAGESELLING_PRICE, typeof(System.Decimal));
               ds.Merge(oTable);
               ds = SetOrderMonth(ds);
               string queryForAverajeCostPrice = "Select  Item.ItemID , SUM(PurchaseOrderDetail.Qty*PurchaseOrderDetail.cost)/SUM(PurchaseOrderDetail.Qty) as  AverageCostPrice , Convert(VarChar(3),DateName(Month,OrderDate)) + ' ' + Convert(VarChar(4), Year(OrderDate)) as POSTransactionDetailDate from PurchaseOrderDetail,Item,PurchaseOrder " +
                                            "where Item.ItemID = PurchaseOrderDetail.ItemID AND PurchaseOrder.OrderID = PurchaseOrderDetail.OrderID AND Item.ItemID ='" + upcCode + "' AND OrderDate Between Convert(varchar(20),GetDate()-365,101) AND Convert(varchar(20),GetDate(),101) group by Item.ItemID ,Convert(VarChar(3),DateName(Month,OrderDate)) + ' ' + Convert(VarChar(4), Year(OrderDate)) order by  Convert(VarChar(3),DateName(Month,OrderDate)) + ' ' + Convert(VarChar(4), Year(OrderDate)) Asc";

               dsForAverageCostPrice = search.SearchData(queryForAverajeCostPrice);
               foreach (DataRow row in dsForAverageCostPrice.Tables[0].Rows)
               {
                   foreach (DataRow dsRow in ds.Tables[0].Rows)
                   {
                       if (dsRow[ORDER_MONTH].ToString() == row["POSTransactionDetailDate"].ToString())
                       {
                           dsRow[AVERAGECOST_PRICE] = row[AVERAGECOST_PRICE].ToString();
                       }
                   }
               }
               string queryForAverajeSellingPrice = "Select Item.ItemID ,SUM(POSTransactionDetail.Qty * POSTransactionDetail.Price)/SUM(POSTransactionDetail.Qty) as AverageSellingPrice ,Convert(VarChar(3),DateName(Month,TransDate)) + ' ' + Convert(VarChar(4), Year(TransDate)) as TransactionDetailMonth from Item,POSTransactionDetail , POSTransaction " +
                                         "where Item.ItemID = POSTransactionDetail.ItemID AND POSTransaction.TransID = POSTransactionDetail.TransID AND Item.ItemID ='" + upcCode + "' AND   TransDate Between Convert(varchar(20),GetDate()-365,101) AND  Convert(varchar(20),GetDate(),101) group by Item.ItemID ,Convert(VarChar(3),DateName(Month,TransDate)) + ' ' + Convert(VarChar(4), Year(TransDate)) order by Convert(VarChar(3),DateName(Month,TransDate)) + ' ' + Convert(VarChar(4), Year(TransDate)) Asc";
               dsForAverageSellingPrice = search.SearchData(queryForAverajeSellingPrice);
               foreach (DataRow row in dsForAverageSellingPrice.Tables[0].Rows)
               {
                   foreach (DataRow dsRow in ds.Tables[0].Rows)
                   {
                       if (dsRow[ORDER_MONTH].ToString() == row["TransactionDetailMonth"].ToString())
                       {
                           dsRow[AVERAGESELLING_PRICE] = row[AVERAGESELLING_PRICE].ToString();
                       }
                   }
               }               
              

               if (description != string.Empty)
                   ultraChartPriceHistory.TitleTop.Text = "Profit Analysis - " + description + " ( " + upcCode + " )";
               else
                   ultraChartPriceHistory.TitleTop.Text = "Profit Analysis - " + upcCode;


               ultraChartPriceHistory.Axis.X.RangeMin = 0;
               ultraChartPriceHistory.Axis.X.RangeMax = 13;

               ultraChartPriceHistory.Axis.Y.RangeMin = 0;
               ultraChartPriceHistory.Axis.Y.RangeType = Infragistics.UltraChart.Shared.Styles.AxisRangeType.Custom;

               double maxValue = GetMaxValueProfitAnalysis(ds);
               if (maxValue != 0.00)
               {
                   ultraChartPriceHistory.Axis.Y.RangeMax = maxValue;
                   ultraChartPriceHistory.Axis.Y.NumericAxisType = Infragistics.UltraChart.Shared.Styles.NumericAxisType.Linear;
               }
               else
                   ultraChartPriceHistory.Axis.Y.RangeMax =10.00;

               ultraChartPriceHistory.DataSource = ds;
               ultraChartPriceHistory.DataBind();
           }
           catch (Exception ex)
           {
              POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");            
           }
        }

        private double GetMaxValueProfitAnalysis(DataSet ds)
        {
            double maxAvgCostPrice = 0.00;
            double maxAvgSellingPrice = 0.00;
            double maxValue = 0.00;

            try
            {
                DataRow[] rowQuantOrd = ds.Tables[0].Select("AverageCostPrice=MAX(AverageCostPrice)");
                maxAvgCostPrice = Convert.ToDouble(rowQuantOrd[0].ItemArray[1].ToString());
                DataRow[] rowQuantSold = ds.Tables[0].Select("AverageSellingPrice=MAX(AverageSellingPrice)");
                maxAvgSellingPrice = Convert.ToDouble(rowQuantSold[0].ItemArray[2].ToString());

                if (maxAvgCostPrice > maxAvgSellingPrice)
                    maxValue = maxAvgCostPrice;
                else if (maxAvgSellingPrice > maxAvgCostPrice)
                    maxValue = maxAvgSellingPrice;
                else if (maxAvgCostPrice == maxAvgSellingPrice)
                    maxValue = maxAvgCostPrice;
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
            return maxValue;
        }
        
        private void ultraTab_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (e.Tab.Text == QUANTITYANALYSIS)
                { 
                    QuantityAnalysis();
                }
                else if (e.Tab.Text == PROFITANALYSIS)
                {
                    ProfitAnalysisPerMonth();    
                }
                else if (e.Tab.Text == ITEMORDEREDPERMONTH)
                {
                    QuantityOrderedPerVendorPerMonth();
                }
            }
            catch(Exception ex)
            {
              POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
        }

        private void ultraChartForQuantity_InvalidDataReceived(object sender, Infragistics.UltraChart.Shared.Events.ChartDataInvalidEventArgs e)
        {
 
          try
           {
             e.Text = "No Data Available";
             e.LabelStyle.FontSizeBestFit = false;
             e.LabelStyle.FontColor = Color.Red;
             e.LabelStyle.Font = new Font("arial",12,FontStyle.Regular);
             e.LabelStyle.HorizontalAlign = StringAlignment.Center;
             e.LabelStyle.VerticalAlign = StringAlignment.Center;              
           }
           catch (Exception ex)
           {
             POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
           }
        }

        private void ultraChartPriceHistory_InvalidDataReceived(object sender, Infragistics.UltraChart.Shared.Events.ChartDataInvalidEventArgs e)
        {

            try
            {
                e.Text = "No Data Available";
                e.LabelStyle.FontColor = Color.Red;
                e.LabelStyle.FontSizeBestFit = false;
                e.LabelStyle.Font = new Font("arial", 12, FontStyle.Regular);
                e.LabelStyle.HorizontalAlign = StringAlignment.Center;
                e.LabelStyle.VerticalAlign = StringAlignment.Center;
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
        }

        private void ultraChartItmOrdPerVend_InvalidDataReceived(object sender, Infragistics.UltraChart.Shared.Events.ChartDataInvalidEventArgs e)
        {
            try
            {
                e.Text = "No Data Available";
                e.LabelStyle.FontColor = Color.Red;
                e.LabelStyle.FontSizeBestFit = false;
                e.LabelStyle.Font = new Font("arial", 12 , FontStyle.Regular);
                e.LabelStyle.HorizontalAlign = StringAlignment.Center;
                e.LabelStyle.VerticalAlign = StringAlignment.Center;
            }
            catch (Exception ex)
            {
               POS_Core.ErrorLogging.ErrorHandler.logException(ex, "", "");
            }
        }


        private DataSet SetOrderMonth(DataSet ds)
        {
            int count = 0;
            Search search = new Search();
            DataSet tempDs = new DataSet();

            try
            {
                tempDs = ds.Clone();
                for(count = 0; count <= 12; count++)
                {
                    DataRow row = tempDs.Tables[0].NewRow();
                    DataSet dsOrderMonth = search.SearchData("Select Convert(VarChar(3),DateName(Month, GetDate()-" + 30 * count + ")) + ' ' + Convert(VarChar(4),Year(GetDate()-" + 30 * count + ")) as [Order Month] order by Convert(VarChar(3),DateName(Month, GetDate()-" + 30 * count + ")) + ' ' + Convert(VarChar(4),Year(GetDate()-" + 30 * count + ")) Desc");
                    row[ORDER_MONTH] = dsOrderMonth.Tables[0].Rows[0]["Order Month"].ToString();
                    tempDs.Tables[0].Rows.Add(row);
                }
                count = 0;
                if(tempDs.Tables[0].Rows.Count >0)
                {
                    int tempDsRowCount = 0;
                    tempDsRowCount = tempDs.Tables[0].Rows.Count;
                    for(count = 0; count < tempDs.Tables[0].Rows.Count;count++)
                    {
                        DataRow row = tempDs.Tables[0].Rows[tempDsRowCount-1];
                        ds.Tables[0].ImportRow(row);
                        tempDsRowCount--;
                    }
                }
            }
            catch (Exception ex)
            {
               ErrorHandler.logException(ex, "", "");
            }
            return ds;
        }
    }
}