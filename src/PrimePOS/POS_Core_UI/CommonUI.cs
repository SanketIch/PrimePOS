using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for CommonUI.
	/// </summary>
	public class CommonUI
	{
		public CommonUI()
		{

			//
			// TODO: Add constructor logic here
			//
		}

        #region JY 19-Sep-2014 commented as it is not in use
        //public static void ShowReport(ReportClass pReport)
        //{
        //    clsUIHelper.ReportViewer.rvReportViewer.ReportSource = pReport;

        //    clsUIHelper.ReportViewer.MdiParent = frmMain.getInstance();
        //    clsUIHelper.ReportViewer.WindowState = FormWindowState.Maximized;
        //    clsUIHelper.ReportViewer.rvReportViewer.DisplayGroupTree = false;
        //    clsUIHelper.ReportViewer.Show();
        //}

        //public static void PrintReport(ReportClass pReport)
        //{
        //    clsUIHelper.ReportViewer.rvReportViewer.ReportSource = pReport;
        //    clsUIHelper.ReportViewer.rvReportViewer.PrintReport();
        //}
        #endregion

        private static string getColumnName(String ErrorText)
		{
			int StartIndex = ErrorText.IndexOf("Column '");
			int EndIndex = ErrorText.IndexOf("'",StartIndex+8);

			string columnName = ErrorText.Substring(StartIndex+8,EndIndex-StartIndex-8);
			return columnName;
		}

		public static void checkGridError(Infragistics.Win.UltraWinGrid.UltraGrid grid, Infragistics.Win.UltraWinGrid.ErrorEventArgs e,params string[] MandatoryColumn)
		{
			try
			{
				if (e.ErrorType ==  Infragistics.Win.UltraWinGrid.ErrorType.Data)
				{
					
			//		clsUIHelper.ShowErrorMsg(e.ErrorText);
					

					string columnName = getColumnName(e.DataErrorInfo.ErrorText);
					grid.Focus();

					if (IsColumnInArray(MandatoryColumn,columnName))
					{
						grid.Rows[e.DataErrorInfo.Row.Index].Cells[columnName].Activate();
						grid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
//						grid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextCell);
					}

					//e.Cancel = true;
				}
			}
			catch(Exception )
			{
				grid.Focus();
				e.Cancel = true;
			}
		}
		private static bool IsColumnInArray(string []arr,string key)
		{
			for(int i = 0;i<arr.Length;i++)
			{
				if (arr[i]==key)
					return true;
			}
			return false;
		}

	}
}
