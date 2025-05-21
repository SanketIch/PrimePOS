using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.Reports;
//using POS.UI;
using POS_Core.CommonData;

namespace POS_Core_UI.Reports.ReportsUI
{
    class clsYearlySalesComparison
    {
        public clsYearlySalesComparison() { }

        public void ShowReport()
        {
            try
            {
                //PRIMEPOS-2767 04-Dec-2019 JY corrected the @CurrDt logic which getting failed in every december month, it is used to get first day of next month 

                rptYearlySalesComparison oRpt = new rptYearlySalesComparison();
                string sSQL = @" DECLARE @CurrDt datetime
                                    Set @CurrDt = CAST(LEFT(DATEADD(m,1,GETDATE()-DAY(GETDATE())+1),11) AS datetime)
                                    Select Case when Month(@CurrDt)>=6 Then
                                    Case when Month(@CurrDt) > Year_Months.Month_Num Then Year_Months.Month_Num Else -1*(Month(@CurrDt)+(Month(@CurrDt)-Year_Months.Month_Num)) End
                                    Else
                                    Case when Month(@CurrDt) > Year_Months.Month_Num Then Year_Months.Month_Num Else -1*(12+(Month(@CurrDt)-Year_Months.Month_Num)) End
                                    End AS MonthSort 
                                    ,Year_Months.Month_Name, Year_Months.[Year], 
                                    Case When TempTable.Year_Name is Null Then 
                                    Cast(Case When Year_Months.Month_Num>Month(@CurrDt) Then  Year_Months.[Year]+1 Else  Year_Months.[Year] End as Varchar) + '/' + 
                                    Cast(Case When Year_Months.Month_Num<=Month(@CurrDt) Then  Year_Months.[Year]-1 Else  Year_Months.[Year] End as varchar) 
                                    Else TempTable.Year_Name End as Year_Name
                                    , ISNULL(Sum(NetAmount),0) as NetAmount, Year_Months.Month_Num
                                    From (SELECT DATENAME(month, PT.TransDate) AS 'Month_Name'
                                    , Case When PT.TransDate > @CurrDt-365 Then Cast(Year(@CurrDt) as Varchar)+ '/' + Cast(Year(@CurrDt-365) as Varchar)
                                    When PT.TransDate > @CurrDt-730 And PT.TransDate < @CurrDt-365 Then Cast(Year(@CurrDt-365) as Varchar)+ '/' + Cast(Year(@CurrDt-730) as Varchar)
                                    When PT.TransDate > @CurrDt-1095 And PT.TransDate < @CurrDt-730 Then Cast(Year(@CurrDt-730) as Varchar)+ '/' + Cast(Year(@CurrDt-1095) as Varchar)
                                    Else '' End as Year_Name ,DATEPart(MM, PT.TransDate) AS 'Month_Num' ,DATENAME(Year, PT.TransDate) AS 'Year',
                                    case TransType when 3 then ISNULL(PT.TotalPaid,0) else  ISNULL(PT.GrossTotal,0) + ISNULL(PT.TotalTaxAmount,0) - ISNULL(PT.TotalDiscAmount,0) end as NetAmount,    
                                    Case TransType when 1 Then 'Sale' when 2 Then 'Return' when 3 then 'ROA' end as TransType 
                                    From postransaction PT where transdate>@CurrDt-1095 ) as TempTable
                                    Right Outer Join ( SELECT * FROM (
                                    Select 'January' as Month_Name, 1 as Month_Num
                                    Union Select 'February' as Month_Name, 2 as Month_Num
                                    Union Select 'March' as Month_Name, 3 as Month_Num
                                    Union Select 'April' as Month_Name, 4 as Month_Num
                                    Union Select 'May' as Month_Name, 5 as Month_Num
                                    Union Select 'June' as Month_Name, 6 as Month_Num
                                    Union Select 'July' as Month_Name, 7 as Month_Num
                                    Union Select 'August' as Month_Name, 8 as Month_Num
                                    Union Select 'September' as Month_Name, 9 as Month_Num
                                    Union Select 'October' as Month_Name, 10 as Month_Num
                                    Union Select 'November' as Month_Name, 11  as Month_Num
                                    Union Select 'December' as Month_Name, 12  as Month_Num
                                    ) AS MonthName,
                                    (Select Year(TransDate) as [Year] from postransaction PT  where transdate>@CurrDt-1095 Group by Year(TransDate) ) as YearName) as Year_Months
                                    On Year_Months.Month_Num=TempTable.Month_Num And Year_Months.[Year]=TempTable.[Year]
                                    Where Cast(Year_Months.[Year] as Varchar)+ Right('00'+Cast(Year_Months.Month_Num as Varchar),2) <=Cast(Year(@CurrDt) as Varchar)+ Right('00'+Cast(Month(@CurrDt) as Varchar),2) 
                                    And Cast(Year_Months.[Year] as Varchar)+ Right('00'+Cast(Year_Months.Month_Num as Varchar),2) >Cast(Year(@CurrDt-1095) as Varchar)+ Right('00'+Cast(Month(@CurrDt-1095) as Varchar),2) 
                                    Group by Year_Months.[Year] ,TempTable.Month_Name, Year_Months.Month_Num,TempTable.Year_Name,Year_Months.Month_Name
                                    Order by  MonthSort Desc,Year_Months.[Year] Desc, Year_Months.Month_Num Desc";

                clsReports.Preview(false, sSQL, oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

    }
}
