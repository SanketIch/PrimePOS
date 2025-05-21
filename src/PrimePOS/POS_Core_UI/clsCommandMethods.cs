using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
//using SecurityManager;
using CrystalDecisions.CrystalReports.Engine;
//using MessagingLib;


namespace POS_Core_UI
{
    /// <summary>
    /// Summary description for clsCommandMethods.
    /// </summary>
    public class clsCommandMethods
    {
   //     private bool IsTracingEnabled;

   //     public DateTime CurrDate
   //     {
   //         get { return AppGlobal.objSysSettings.TDATE; }
   //     }

   //     public clsCommandMethods()
   //     {

   //     }
   //     public string clsTest()
   //     {
   //         return "sadfa";
   //     }
   //     ContScanDocument oCont;


   //     public void AddImageInDMS(string imgPath)
   //     {
   //         try
   //         {
   //             dsElecDocument oDsDoc = new dsElecDocument();
   //             dsElecDocument.DM_DocumentRow oDocRow;
   //             ContScanDocument oCont = new ContScanDocument();
   //             oDsDoc.DM_Document.Clear();
   //             imgPath = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + @"\" + imgPath;
   //             //imgPath = @"D:\mainimage.jpg";
   //             //if (HaveReadWrites(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache)))
   //             //{
   //             //    //dosomething
   //             //    //GetDirectoyReadRights(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
   //             //}
   //             System.Drawing.Image img = System.Drawing.Image.FromFile(imgPath);
   //             byte[] imgbytes = AppGlobal.imageToByteArray(img);
   //             oDocRow = oDsDoc.DM_Document.AddDM_DocumentRow("", "", "", "", 1, 1, imgbytes, DateTime.Now, AppGlobal.gPhUser.PH_INIT, null, null);
   //             if (oCont.Save(oDsDoc))
   //             {
   //                 frmScanDocument frmscandoc = new frmScanDocument();
   //                 frmscandoc.CMDImage = img;
   //                 frmscandoc.SaveImageToDisk(oDocRow.DocumentId);
   //             }
   //         }
   //         catch (Exception exc)
   //         {
   //             ErrorHandler.ShowMessage(null, exc, false);
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }

   //     public void AddRefillsDue(DateTime StartScanDate, DateTime DateFrom, DateTime DateTo, string FacilityCd, string RefillReminderCriteria)
   //     {
   //         try
   //         {
   //             FrmRefQue oFrmRefQue = new FrmRefQue();
   //             oFrmRefQue.CalledFromCommLine = true;
   //             oFrmRefQue.dStartScan = StartScanDate;
   //             oFrmRefQue.dFromDate = DateFrom;
   //             oFrmRefQue.dToDate = DateTo;
   //             oFrmRefQue.sFacility = FacilityCd;
   //             oFrmRefQue.sRefillReminderCriteria = RefillReminderCriteria;
   //             oFrmRefQue.ShowInTaskbar = false;

   //             oFrmRefQue.ShowDialog();
   //         }
   //         catch (Exception exc)
   //         {
   //             ErrorHandler.ShowMessage(null, exc, false);
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }
   //     public void PrintDailyLogReport(DateTime DateFrom, DateTime DateTo, string FacilityCd, string ReportType)
   //     {
   //         PrintDailyLogReport(DateFrom, DateTo, FacilityCd, ReportType, "", false, false, false, false);
   //     }
   //     public void PrintDailyLogReport(DateTime DateFrom, DateTime DateTo, string FacilityCd, string ReportType, string PatientCat, bool sortBy, bool bchkFacilityOnly, bool bckhRefillsonly, bool bcbxControlRep)
   //     {
   //         try
   //         {
   //             FrmDailyLog oDailylog = new FrmDailyLog();
   //             oDailylog.calledFromCommLine = true;
   //             oDailylog.customControl.dtFromDate.Value = DateFrom;
   //             oDailylog.customControl.dtToDate.Value = DateTo;
   //             oDailylog.customControl.cbFacility.Text = FacilityCd;
   //             oDailylog.customControl.cbPatientCat.Text = PatientCat;
   //             if (sortBy == true)
   //                 oDailylog.customControl.optSortByDateTimeFilled.Checked = true;
   //             else
   //                 oDailylog.customControl.optSortByDateTimeFilled.Checked = false;
   //             if (bchkFacilityOnly == true)
   //                 oDailylog.customControl.chkFacilityOnly.Checked = true;
   //             if (bckhRefillsonly == true)
   //                 oDailylog.customControl.ckhRefillsonly.Checked = true;
   //             if (bcbxControlRep == true)
   //                 oDailylog.customControl.cbxControlRep.Checked = true;

   //             switch (ReportType.ToUpper())
   //             {
   //                 case "C":
   //                     oDailylog.customControl.rbCompact.Checked = true;
   //                     break;
   //                 case "D":
   //                     oDailylog.customControl.rbDetail.Checked = true;
   //                     break;
   //                 case "S":
   //                     oDailylog.customControl.rbSummary.Checked = true;
   //                     break;
   //                 default:
   //                     oDailylog.customControl.rbCompact.Checked = true;
   //                     break;
   //             }


   //             oDailylog.Opacity = 0;
   //             oDailylog.ShowInTaskbar = false;
   //             oDailylog.Show();
   //             if (!oDailylog.IsDisposed && oDailylog != null) oDailylog.Close();
   //         }
   //         catch (Exception exc)
   //         {
   //             ErrorHandler.ShowMessage(null, exc, false);
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }
   //     //

   //     public void ScheduledTaskExecute(int iScheduledTasksID, int TaskID)
   //     {

   //         ContScheduledTasks oContScheduledTasks = new ContScheduledTasks();
   //         ScheduledTasksLog oScheduledTasksLog = new ScheduledTasksLog();
   //         ContScheduledTasksLog oContScheduledTasksLog = new ContScheduledTasksLog();
   //         ScheduledTasks oScheduledTasks = null;
   //         string sBody = string.Empty;
   //         string sTaskStatus = string.Empty;
   //         string filePath = string.Empty;
   //         string sNoOfRecordAffect = string.Empty;
   //         try
   //         {
   //             DataSet ds = new DataSet();
   //             oContScheduledTasks.GetScheduledTasks(out ds, iScheduledTasksID);
   //             oScheduledTasks = new ScheduledTasks(ds);
   //             // Updated By M. Ali for primerx-1775  
   //             oScheduledTasksLog.NewRec = true;
   //             oScheduledTasksLog.ScheduledTasksLogID = 0;
   //             oScheduledTasksLog.StartDate = DateTime.Now;
   //             oScheduledTasksLog.StartTime = DateTime.Now;
   //             oScheduledTasksLog.EndTime = DateTime.Now;
   //             oScheduledTasksLog.TaskStatus = "In Progress...";
   //             oScheduledTasksLog.Task = iScheduledTasksID;
   //             oScheduledTasksLog.TaskDescription = "";

   //             // changes by adeel 2/21/2014 
   //             // if Scheduler machine is not set then set this machine as scheduler machine 
   //             // and synch all db tasks to windows tasks

   //             if (AppGlobal.objSysSettings.SchedularMachine.Trim() == "")
   //             {

   //                 ContSysSet oSysSet = new ContSysSet();
   //                 AppGlobal.objSysSettings.SchedularMachine = Environment.MachineName;
   //                 oSysSet.Save(AppGlobal.objSysSettings);


   //                 frmScheduledTasksSearch frmSchedular = new frmScheduledTasksSearch();
   //                 frmSchedular.SyncScheduleTasks();
   //             }

   //             if (AppGlobal.objSysSettings.SchedularMachine.Trim() != Environment.MachineName)
   //             {
   //                 oScheduledTasksLog.TaskStatus = "In Complete";
   //                 oScheduledTasksLog.TaskDescription = "Computer name Is not matching with schedular configured machine.";
   //                 oContScheduledTasksLog.Save(oScheduledTasksLog);
   //                 return;
   //             }

   //             ICommandLIneTaskControl obj = null;
   //             Type type = Type.GetType(ScheduledTasks.getTask(TaskID), false, false);
   //             obj = (ICommandLIneTaskControl)System.Activator.CreateInstance(type);
   //             oContScheduledTasksLog.Save(oScheduledTasksLog);

   //             obj.RunTask(iScheduledTasksID, ref filePath, Convert.ToBoolean(oScheduledTasks.SendPrint), ref sNoOfRecordAffect);

   //             if (obj.GetType().Name == "FrmRefsDueRep")
   //             {
   //                 FrmRefQue oFrmRefQue = new FrmRefQue();
   //                 oFrmRefQue.ShowInTaskbar = false;
   //                 oFrmRefQue.Opacity = 0.0;
   //                 oFrmRefQue.oFrmRefsDueRep = (FrmRefsDueRep)obj;
   //                 oFrmRefQue.CalledFromCommLine = true;
   //                 oFrmRefQue.CalledFromWinScheduled = true;
   //                 oFrmRefQue.ShowDialog();
   //                 sNoOfRecordAffect = oFrmRefQue.sNoOfRecordMessage;
   //             }
   //             else if (obj.GetType().Name == "FrmFiledRxRep")
   //             {
   //                 FrmRefQue oFrmRefQue = new FrmRefQue();
   //                 oFrmRefQue.ShowInTaskbar = false;
   //                 oFrmRefQue.Opacity = 0.0;
   //                 oFrmRefQue.oFrmFiledRxRep = (FrmFiledRxRep)obj;
   //                 oFrmRefQue.CalledFromCommLine = true;
   //                 oFrmRefQue.ShowDialog();
   //                 sNoOfRecordAffect = oFrmRefQue.sNoOfRecordMessage;
   //             }


   //             sBody = "<b> Task Name: </b>";
   //             sBody = sBody + " " + oScheduledTasks.TaskName;
   //             sBody = sBody + "<br />";

   //             sBody = sBody + "<b> Task Description: </b>";
   //             sBody = sBody + " " + oScheduledTasks.TaskDescription;
   //             sBody = sBody + "<br />";


   //             oScheduledTasksLog.EndTime = DateTime.Now;
   //             oScheduledTasksLog.TaskStatus = "Complete";
   //             oScheduledTasksLog.TaskDescription = "";
   //             oContScheduledTasksLog.Save(oScheduledTasksLog);

   //             sTaskStatus = "<b> Task Status: </b>";
   //             sTaskStatus = sTaskStatus + " " + oScheduledTasksLog.TaskStatus;
   //             sTaskStatus = sTaskStatus + "<br />";

   //             if (sNoOfRecordAffect.Trim() != string.Empty)
   //                 sTaskStatus = sTaskStatus + sNoOfRecordAffect + "<br />";

   //             //ScheduledTasks.SendTaskEmail("Task Status", "Task Status", oScheduledTasks.EmailAddress, filePath);
   //             if (oScheduledTasks.SendEmail == 1)
   //                 AppGlobal.SendEmail(AppGlobal.objSysSettings.SCHDEmailSubject, sBody + sTaskStatus + "<br />" + AppGlobal.objSysSettings.SCHDEmailBody, filePath, oScheduledTasks.EmailAddress);
   //             if (File.Exists(filePath))
   //             {
   //                 try
   //                 {
   //                     System.Threading.Thread.Sleep(5000);
   //                     File.Delete(filePath);
   //                 }
   //                 catch (IOException ie)
   //                 {
   //                     System.Threading.Thread.Sleep(60 * 1000);
   //                 }
   //             }

   //         }
   //         catch (Exception exc)
   //         {
   //             oScheduledTasksLog.EndTime = DateTime.Now;
   //             oScheduledTasksLog.TaskStatus = "Incomplete";
   //             oScheduledTasksLog.TaskDescription = exc.Message;
   //             oContScheduledTasksLog.Save(oScheduledTasksLog);

   //             sTaskStatus = "<b> Task Status: </b>";
   //             sTaskStatus = sTaskStatus + " " + oScheduledTasksLog.TaskStatus;
   //             sTaskStatus = sTaskStatus + "</br>";

   //             if (oScheduledTasks.SendEmail == 1)
   //                 AppGlobal.SendEmail(AppGlobal.objSysSettings.SCHDEmailSubject, sBody + sTaskStatus + "<br />" + AppGlobal.objSysSettings.SCHDEmailBody, filePath, oScheduledTasks.EmailAddress);
   //             if (File.Exists(filePath))
   //             {
   //                 try
   //                 {
   //                     System.Threading.Thread.Sleep(5000);
   //                     File.Delete(filePath);
   //                 }
   //                 catch (IOException ie)
   //                 {
   //                     System.Threading.Thread.Sleep(60 * 1000);
   //                 }
   //             }
   //             ErrorHandler.ShowMessage(null, exc, false);
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //             if (File.Exists(filePath))
   //             {
   //                 try
   //                 {
   //                     File.Delete(filePath);
   //                 }
   //                 catch
   //                 {

   //                 }
   //             }
   //         }

   //     }
   //     //
   //     //
   //     public void SchPrintDailyLog(int TaskID)
   //     {
   //         try
   //         {

   //             DateTime dtFromDate = DateTime.Now;
   //             DateTime dtToDate = DateTime.Now;


   //             String ReportType = string.Empty;
   //             string scbFacility = string.Empty;
   //             string sPatCat = "";

   //             bool sortBy = false;
   //             bool bchkFacilityOnly = false;
   //             bool breportType = false;
   //             bool bckhRefillsonly = false;
   //             bool bcbxControlRep = false;

   //             DataSet oControlds = new DataSet();
   //             DataSet oFormControls = new DataSet();
   //             ContScheduledTasksControls oContScheduledTasksControls = new ContScheduledTasksControls();
   //             oContScheduledTasksControls.GetScheduledTasksControlsList(out oControlds, TaskID);
   //             oFormControls = oContScheduledTasksControls.GetControlList(TaskID);
   //             FrmDailyLog oFrmDailyLog = new FrmDailyLog();

   //             foreach (DataRow dr in oFormControls.Tables[0].Rows)
   //             {
   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.dtFromDate.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.dtFromDate.Name + " ' "))
   //                     {
   //                         dtFromDate = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
   //                     }

   //                 }

   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.dtToDate.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.dtToDate.Name + "' "))
   //                     {
   //                         dtToDate = DateTime.Now.AddDays(Convert.ToDouble(odr["ControlsValue"].ToString().Trim()) * -1);
   //                     }

   //                 }

   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.cbFacility.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.cbFacility.Name + "' "))
   //                     {
   //                         scbFacility = odr["ControlsValue"].ToString();
   //                     }

   //                 }
   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.cbPatientCat.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.cbPatientCat.Name + "' "))
   //                     {
   //                         sPatCat = odr["ControlsValue"].ToString();
   //                     }

   //                 }

   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.optSortByDateTimeFilled.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.optSortByDateTimeFilled.Name + "' "))
   //                     {
   //                         if (Convert.ToBoolean(odr["ControlsValue"].ToString().Trim()) == true)
   //                         {
   //                             sortBy = true;
   //                         }
   //                     }

   //                 }
   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.chkFacilityOnly.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.chkFacilityOnly.Name + "' "))
   //                     {
   //                         bchkFacilityOnly = Convert.ToBoolean(odr["ControlsValue"].ToString().Trim());
   //                     }
   //                 }
   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.rbSummary.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.rbSummary.Name + "' "))
   //                     {
   //                         breportType = Convert.ToBoolean(odr["ControlsValue"].ToString().Trim());
   //                         if (breportType == true)
   //                             ReportType = "S";
   //                     }
   //                 }
   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.rbCompact.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.rbCompact.Name + "' "))
   //                     {
   //                         breportType = Convert.ToBoolean(odr["ControlsValue"].ToString().Trim());
   //                         if (breportType == true)
   //                             ReportType = "C";
   //                     }
   //                 }
   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.rbDetail.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.rbDetail.Name + "' "))
   //                     {
   //                         breportType = Convert.ToBoolean(odr["ControlsValue"].ToString().Trim());
   //                         if (breportType == true)
   //                             ReportType = "D";
   //                     }
   //                 }
   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.ckhRefillsonly.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.ckhRefillsonly.Name + "' "))
   //                     {
   //                         bckhRefillsonly = Convert.ToBoolean(odr["ControlsValue"].ToString().Trim());
   //                     }
   //                 }
   //                 if (dr["ControlsName"].ToString().Trim() == oFrmDailyLog.customControl.cbxControlRep.Name)
   //                 {
   //                     foreach (DataRow odr in oControlds.Tables[0].Select("ControlsName = '" + oFrmDailyLog.customControl.cbxControlRep.Name + "' "))
   //                     {
   //                         bcbxControlRep = Convert.ToBoolean(odr["ControlsValue"].ToString().Trim());
   //                     }
   //                 }

   //             }


   //             PrintDailyLogReport(dtFromDate, dtToDate, scbFacility, ReportType, sPatCat, sortBy, bchkFacilityOnly, bckhRefillsonly, bcbxControlRep);

   //         }
   //         catch (Exception exc)
   //         {
   //             ErrorHandler.ShowMessage(null, exc, false);
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }

   //     }


   //     //naim 14Sep2006
   //     //This method will be used by clscommandlineoptions and it will generate control file.
   //     public void GenerateControlFile()
   //     {

   //         try
   //         {
   //             //AppGlobal.
   //             DateTime sDate = DateTime.MinValue;
   //             DateTime eDate = DateTime.MinValue;

   //             object oSDate = null;
   //             object oEDate = null;

   //             MMSUtil.ReadIni.IniFileName = System.Windows.Forms.Application.StartupPath + "\\CommLineOptions.ini";
   //             string sSDate = MMSUtil.ReadIni.GetIniSetting("CTRLSDATE=");
   //             string sEDate = MMSUtil.ReadIni.GetIniSetting("CTRLEDATE=");

   //             try
   //             {
   //                 if (sSDate.Trim() != "" && sEDate.Trim() != "")
   //                 {
   //                     sDate = DateTime.Parse(sSDate);
   //                     oSDate = DateTime.Parse(sSDate);
   //                     eDate = DateTime.Parse(sEDate);
   //                     oEDate = DateTime.Parse(sEDate);
   //                 }
   //             }
   //             catch { }

   //             if (oSDate == null || oEDate == null)
   //             {
   //                 int month = System.DateTime.Now.Month;
   //                 int year = System.DateTime.Now.Year;

   //                 if (month == 1)
   //                 {
   //                     month = 12;
   //                     year = year - 1;
   //                 }
   //                 else
   //                 {
   //                     month = month - 1;
   //                 }

   //                 sDate = new DateTime(year, month, 1);
   //                 eDate = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59, 999);
   //             }
   //             GenerateControlFile(sDate, eDate);
   //         }
   //         catch (Exception exc)
   //         {
   //             ErrorHandler.ShowMessage(null, exc, false);
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }



   //     //naim 14Sep2006
   //     //This method will be used by clscommandlineoptions and it will generate control file.
   //     private void GenerateControlFile(DateTime sFromDate, DateTime sToDate)
   //     {
   //         if (System.IO.File.Exists(AppGlobal.sControlErrorFile) == true)
   //         {
   //             System.IO.File.Delete(AppGlobal.sControlErrorFile);
   //         }
   //         if (System.IO.File.Exists(ControlRxFile.GetControlRxFileName()))
   //         {
   //             System.IO.File.Delete(ControlRxFile.GetControlRxFileName());
   //         }
   //         FrmControlRx oFrmControlRx = null;
   //         //FrmErrorDisp oFrmErrorDisp = new FrmErrorDisp();
   //         try
   //         {
   //             oFrmControlRx = new FrmControlRx();
   //             oFrmControlRx.sFromDate = sFromDate.Date.ToString(); ;
   //             oFrmControlRx.sToDate = sToDate.Date.ToString();
   //             oFrmControlRx.Opacity = 0;
   //             oFrmControlRx.CalledFromCommandLine = true;
   //             oFrmControlRx.CreateControlRx();
   //         }
   //         catch (Exception e)
   //         {
   //             //oFrmErrorDisp.GroupDisplay = "Control Rx Errors";
   //             //oFrmErrorDisp.FormTitle = "Control Rx Errors";
   //             //oFrmErrorDisp.ShowDialog();

   //         }
   //         finally
   //         {
   //             oFrmControlRx.Close();
   //             oFrmControlRx.Dispose();

   //         }





   //         /* Existing Code Written By Naim*/
   //         /*
			//DataSet odsCntRx=null;
			//ControlRxCreator oCntRxCreator=new ControlRxCreator();
			////FrmRxPicker oFrmRxPicker=new FrmRxPicker();			
			//oCntRxCreator.Nabp = AppGlobal.objPharmacy.sNABP;			

			//if(sFromDate.ToString() != "" && sToDate.ToString() != "")
			//{
			//	ControlRxSelector oCntRxSelector=new ControlRxSelector();
			//	odsCntRx=oCntRxSelector.GetControlData(Convert.ToDateTime(sFromDate),Convert.ToDateTime(sToDate),-1,ControlRxSelector.SearchOptions.RxDates,"");
			//}
	
			////oFrmRxPicker.ContinueButton = true;
			////oFrmRxPicker.RxData = odsCntRx;						
			////oFrmRxPicker.ShowDialog();					
			////this.Refresh();
			
			

			//if(odsCntRx != null && odsCntRx.Tables[0].Rows.Count > 0)
			//{
			//	oCntRxCreator.RxData=odsCntRx.Tables[0];
			//	oCntRxCreator.Process();
				
			//	string NYDHSUBHNO="";
			//	if (oCntRxCreator.TotalErrCount>0 || oCntRxCreator.TotalCreated>0)
			//	{
			//		//Saving the creation record to submission history file
			//		Nydhsubh oNydhsubh=new Nydhsubh();
			//		ContNydhsubh oContNydhsubh=new ContNydhsubh();
			//		oNydhsubh.CR_DATE = Convert.ToDateTime(System.DateTime.Today.ToShortDateString());
			//		oNydhsubh.FROM_DATE = Convert.ToDateTime(sFromDate);
			//		oNydhsubh.TO_DATE = Convert.ToDateTime(sToDate);
			//		oNydhsubh.RX_COUNT = oCntRxCreator.TotalCreated;
			//		NYDHSUBHNO=oContNydhsubh.Save(oNydhsubh);
			//	}

			//	FileGenerator oFileGen=new FileGenerator();

			//	if(oCntRxCreator.TotalErrCount > 0)
			//	{						
			//		DataSetStringGenerator odsStrGen=new DataSetStringGenerator();
					
			//		oFileGen.FileName = AppGlobal.sControlErrorFile;
			//		oFileGen.CreateNew = true;
			//		oFileGen.pFlushType = FileGenerator.FlushType.StringFlush;										
					
					
			//		odsStrGen.StringData = oCntRxCreator.RxData;
			//		odsStrGen.FilterInfo = "RXSTATUS = 'E'";
			//		odsStrGen.ColumnInfo = "RXERROR";
			//		odsStrGen.RowSep="\r\n-------------------------------------------------------------\r\n";
					
			//		oFileGen.FileData = NYDHSUBHNO + "\r\n" + odsStrGen.GetString();										
			//		oFileGen.FlushData();
					
			//		try
   //                 //{
   //                 //    System.Diagnostics.Process.Start("NOTEPAD.EXE", AppGlobal.sControlErrorFile); //System.Diagnostics.Process.Start(
   //                 //}

   //                 //catch(Exception e)
   //                 //{
   //                 //    oFrmErrorDisp.GroupDisplay = "Control Rx Errors";		 																		
   //                 //    oFrmErrorDisp.FormTitle = "Control Rx Errors";														
   //                 //    oFrmErrorDisp.ShowDialog(); 																									

   //                 //}
				
				
			//	}								
				
			//	if(oCntRxCreator.TotalCreated > 0)
			//	{
			//		string sControlFile=ControlRxFile.GetControlRxFileName();
			//		FileGenerator oFGen=new FileGenerator();
			//		oFGen.ColumnInfo = "RXREC";
			//		oFGen.CreateNew = true;
			//		oFGen.DataFilter = "RXSTATUS = 'C'";
			//		oFGen.FileDataSet = odsCntRx;
			//		oFGen.FileName = sControlFile;
			//		oFGen.pFlushType = FileGenerator.FlushType.DataSetFlush;
			//		oFGen.FlushData();
			//	}
			

			//}
   //          */
   //     }

   //     //naim 08Mar2007
   //     public void REPRICELIST()
   //     {
   //         try
   //         {
   //             ContPriceList oCont = new ContPriceList();
   //             oCont.RepriceList();
   //         }
   //         catch (Exception exc)
   //         {
   //             ErrorHandler.ShowMessage(null, exc, false);
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }
   //     // Muhamamd Saqib feb 3 2009 
   //     public void ProcessProfitWatch(string sFilePath)
   //     {
   //         FrmBatchRR oFrmBatchRR = new FrmBatchRR(true, sFilePath);
   //         try
   //         {

   //             AppGlobal.IniFile = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\phw01.ini";
   //             if (System.IO.File.Exists(AppGlobal.IniFile))
   //             {
   //                 oFrmBatchRR.btnAddFromFile_Click(null, null);
   //                 oFrmBatchRR.btnCreate_Click(null, null);
   //                 int batchId = oFrmBatchRR.ibatchId;
   //                 oFrmBatchRR.cmdProcess_Click(null, null);
   //                 try
   //                 {
   //                     FrmProfitWatchResubmissionReport oFrmWacthSubmession = new FrmProfitWatchResubmissionReport();
   //                     oFrmWacthSubmession.txtBatch.Tag = batchId.ToString();// oFrmBatchRR.BatchId.ToString();
   //                     string Report = oFrmWacthSubmession.ExportPDFReport();
   //                     AppGlobal.SendProfitwatchEmail("Profit Watch Report", "Please see Attached Report File", Report, "");
   //                     if (File.Exists(Report))
   //                         File.Delete(Report);
   //                 }
   //                 catch (Exception exp)
   //                 {

   //                 }
   //             }
   //             else
   //                 throw new Exception("No Phw01.ini Found");

   //         }
   //         catch (Exception ex)
   //         {
   //             ErrorHandler.WriteProfitWatchErr(ex);
   //             ErrorHandler.ShowMessage(oFrmBatchRR, ex, false);
   //         }
   //         finally
   //         {
   //             oFrmBatchRR.Dispose();
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }

   //     // Muhammad Saqib august 12 2009
   //     public void PrintAuditInsPayReport(string sReportOption, DateTime sFromDate, DateTime sToDate)
   //     {
   //         FrmRRQuery oFrmRR = new FrmRRQuery(FrmQueryMods.Report);
   //         oFrmRR.PrintReport(sReportOption, sFromDate, sToDate);
   //     }

   //     /// <summary>
   //     /// IMPLEMENTED BY: Muhammad Shahid
   //     /// DATE: 08-Apr-2011
   //     /// PURPOSE: Method to be executed when calling from WCFRefillMyRxs Service.
   //     /// LOGIC: Get all Rxs which are due for refill from DB. Generate a DataSet containing all information as which Rxs were 
   //     ///          for refill or not. 

   //     /// LAST MODIFIED: 08-Apr-2011
   //     /// LAST MODIFIED BY: Muhammad Shahid
   //     /// </summary>
   //     public DataSet GetRxRefillStatus(string sNABPNo, string sCSVRxs, out string sErrorMessage)
   //     {
   //         sErrorMessage = "";
   //         try
   //         {

   //             ContRxRefQue oContRefQue = new ContRxRefQue();
   //             ContClaims cClm = new ContClaims();
   //             DataSet dsRxs = cClm.ValidateNABPandRxNo(sNABPNo, sCSVRxs);
   //             long lRxNo;
   //             //############################################################
   //             //Modified by Muhammad Shahid to check Queue Rxs
   //             DataSet dsQueuedRx;
   //             dsQueuedRx = oContRefQue.GetQueRecs("-1");
   //             //############################################################
   //             //Loop to set Refill Status of valid Rxs
   //             foreach (DataRow dr in dsRxs.Tables[0].Select("STATUS=''"))
   //             {
   //                 int DaysLeft;
   //                 DateTime DueDate;
   //                 decimal QtyLeft;
   //                 string RxStatus = "";

   //                 if (Int64.TryParse(dr["RXNO"].ToString(), out lRxNo))
   //                 {
   //                     try
   //                     {
   //                         RxStatus = cClm.IsRxRefillable(lRxNo, out DaysLeft, out DueDate, out QtyLeft).ToString();
   //                         //############################################################
   //                         //Confirm this Rx is not already queued for refill
   //                         if (dsQueuedRx != null && dsQueuedRx.Tables.Count > 0 && dsQueuedRx.Tables[0].Select("RXNO = '" + lRxNo.ToString() + "'").Length > 0)
   //                             RxStatus = "Specified Rx is already queued for refill";
   //                         switch (RxStatus)
   //                         {
   //                             case ("OkToRefill"):
   //                                 dr["STATUS"] = "OK";
   //                                 break;
   //                             case ("NoQuantity"):
   //                                 dr["STATUS"] = "No Refill";
   //                                 break;
   //                             case ("FiledRx"):
   //                                 dr["STATUS"] = "Rx doesn't exists";
   //                                 break;

   //                             case ("ControlNotRefillable"):
   //                                 dr["STATUS"] = "Control Not Refillable";
   //                                 break;

   //                             case ("EarlyforRefill"):
   //                                 dr["STATUS"] = "Early for refill";
   //                                 break;

   //                             case ("RefillExpired"):
   //                                 dr["STATUS"] = "Refill Expired";
   //                                 break;

   //                             case ("RxNotFound"):
   //                                 dr["STATUS"] = "Rx doesn't exists";
   //                                 break;
   //                             case ("DiscontinuedRx"):
   //                                 dr["STATUS"] = "Discontinued";
   //                                 break;
   //                             case ("TransferredOut"):
   //                                 dr["STATUS"] = "Rx doesn't exists";
   //                                 break;
   //                             case ("DontKnow"):
   //                                 dr["STATUS"] = "Rx doesn't exists";
   //                                 break;

   //                             default:
   //                                 dr["STATUS"] = RxStatus;
   //                                 break;
   //                         }
   //                     }
   //                     catch (Exception oIEx)
   //                     {
   //                         ErrorHandler.ShowMessage(null, oIEx, false);
   //                     }
   //                 }
   //                 else
   //                 {
   //                     dr["STATUS"] = "Specified RxNo is invalid";
   //                 }
   //             }

   //             return dsRxs;
   //         }
   //         catch (Exception oEx)
   //         {
   //             sErrorMessage = oEx.ToString();
   //             //As this method will be called from command line so just write to error log file
   //             //without showing popup message i.e. set bShowMessage parameter to false
   //             return null;
   //             //ErrorHandler.ShowMessage(null, oEx, false);
   //         }
   //     }

   //     /// <summary>
   //     /// IMPLEMENTED BY: Khurram Jalil
   //     /// DATE: 12-Nov-2010
   //     /// PURPOSE: Method to be executed when Phw.exe is compiled as a DLL and included in WCFRefillMyRxClient for performing Rx Refills.
   //     /// LOGIC: Get all Rxs which are due for refill from DB, if specified Rxs exists in resultset then
   //     ///         queue them for refill.
   //     /// LAST MODIFIED: 23-Mar-2012
   //     /// LAST MODIFIED BY: Kashif Abbasi
   //     /// </summary>
   //     public DataSet PerformRxRefill(string sNABPNo, DataSet dsRxsReq, out string sErrorMessage)
   //     {
   //         sErrorMessage = "";
   //         try
   //         {
   //             ContClaims cClm = new ContClaims();
   //             //ErrorHandler.ShowMessage(null, new Exception("Total Rxs req = " + dsRxsReq.Tables[0].Rows.Count), false);
   //             string sCSVRxs = GetCSRxs(dsRxsReq, out sErrorMessage);

   //             ErrorHandler.ShowMessage(null, new Exception("Rx requested for refill = " + sCSVRxs), false);
   //             DataSet dsRxs = cClm.ValidateNABPandRxNo(sNABPNo, sCSVRxs);

   //             //ErrorHandler.ShowMessage(null, new Exception("Total Rxs req = "+dsRxs.Tables[0].Rows.Count), false);

   //             long lRxNo;
   //             //change for comments by Muhammad Shahid
   //             //dsRxs.Tables[0].Columns.Add("Remark", typeof(string));
   //             //string[] strActionID = Comments.Split(',');
   //             //int i = 0;
   //             //foreach (DataRow dr in dsRxs.Tables[0].Rows)
   //             //{
   //             //    dr["Remark"] = strActionID[i];
   //             //    i++;
   //             //}
   //             //string Comments, it's parameter change it after demo
   //             //-------------------------------------

   //             //############################################################
   //             //Modified by Muhammd Shahid date on 28042011 
   //             //Not giving proper Rx Refill Status so modifying the method call

   //             //Loop to set Refill Status of valid Rxs
   //             foreach (DataRow dr in dsRxs.Tables[0].Select("STATUS=''"))
   //             {
   //                 int DaysLeft;
   //                 DateTime DueDate;
   //                 decimal QtyLeft;

   //                 if (Int64.TryParse(dr["RXNO"].ToString(), out lRxNo))
   //                 {
   //                     try
   //                     {
   //                         dr["STATUS"] = cClm.IsRxRefillable(lRxNo, out DaysLeft, out DueDate, out QtyLeft).ToString();
   //                     }
   //                     catch (Exception oIEx)
   //                     {
   //                         ErrorHandler.ShowMessage(null, oIEx, false);
   //                     }
   //                 }
   //                 else
   //                 {
   //                     dr["STATUS"] = "Specified RxNo is invalid";
   //                 }
   //             }

   //             //############################################################
   //             //Loop to add Rxs due for Refill into the Refill Queue
   //             ContClaims oContClaims = new ContClaims();
   //             ContRxRefQue oContRefQue = new ContRxRefQue();
   //             ContPatient oContPat = new ContPatient();
   //             ContPrescrib oContPrs = new ContPrescrib();
   //             ContDrug oContDrg = new ContDrug();

   //             DataSet dsRxToQueue, dsQueuedRx;
   //             dsQueuedRx = oContRefQue.GetQueRecs("-1");
   //             DataSet oDsQueueRec = oContRefQue.GetRxRefQue("-1");
   //             //Modified by Muhammad Shahid dated on 29042011 to queue Rxs having status of 'OkToRefill' and 'EarlyForRefill'
   //             foreach (DataRow dr in dsRxs.Tables[0].Select("STATUS='" + RxRefillStatus.OkToRefill.ToString() + "' OR STATUS='" + RxRefillStatus.EarlyforRefill.ToString() + "' OR STATUS='" + RxRefillStatus.ControlNotRefillable.ToString() + "' OR STATUS='" + RxRefillStatus.DiscontinuedRx.ToString() + "'OR STATUS='" + RxRefillStatus.RefillExpired.ToString() + "'OR STATUS='" + RxRefillStatus.NoQuantity.ToString() + "'"))
   //             {
   //                 if (Int64.TryParse(dr["RXNO"].ToString(), out lRxNo))
   //                 {
   //                     try
   //                     {
   //                         oContClaims.GetRxSelection(lRxNo.ToString(), "", out dsRxToQueue, true, "");

   //                         if (dsQueuedRx != null && dsQueuedRx.Tables.Count > 0 && dsQueuedRx.Tables[0].Select("RXNO = '" + lRxNo.ToString() + "'").Length > 0)
   //                             dr["STATUS"] = "Specified Rx is already queued for refill";
   //                         else if (dsRxToQueue != null && dsRxToQueue.Tables.Count > 0 && dsRxToQueue.Tables[0].Rows.Count > 0)
   //                         {
   //                             if (!oContPat.RecExist(dsRxToQueue.Tables[0].Rows[0]["PATIENTNO"].ToString()))
   //                                 dr["STATUS"] = "Patient for specified Rx does not exist";
   //                             else if (!oContPrs.RecExist(dsRxToQueue.Tables[0].Rows[0]["PRESNO"].ToString()))
   //                                 dr["STATUS"] = "Prescriber for specified Rx does not exist";
   //                             else if (!oContDrg.ValidDrugNdc(dsRxToQueue.Tables[0].Rows[0]["NDC"].ToString()))
   //                                 dr["STATUS"] = "Drug NDC for specified Rx is invalid";
   //                             else
   //                             {
   //                                 //in below if-else we decide the Rx is verified or not by comparing the Patient sent Detail and actual Rx Detail
   //                                 DataSet dsActualPatInfo = oContClaims.GetPatInfoByRx(lRxNo);
   //                                 if (dsActualPatInfo != null && dsActualPatInfo.Tables.Count > 0)
   //                                 {
   //                                     DataRow[] drWebPatInfo = dsRxsReq.Tables[0].Select("RXNO = '" + lRxNo.ToString() + "'");
   //                                     if (dsActualPatInfo.Tables[0].Rows.Count > 0 && drWebPatInfo != null)
   //                                     {
   //                                         DateTime dtDOBWeb = DateTime.MinValue;
   //                                         DateTime.TryParse(drWebPatInfo[0]["DOB"].ToString().Trim(), out dtDOBWeb);
   //                                         DateTime dtDOBAct = DateTime.MinValue;
   //                                         DateTime.TryParse(dsActualPatInfo.Tables[0].Rows[0]["DOB"].ToString().Trim(), out dtDOBAct);
   //                                         ErrorHandler.ShowMessage(null, new Exception(lRxNo.ToString() + "," + dtDOBWeb.ToString() + "$$" + dtDOBAct.ToString() + "," + drWebPatInfo[0]["LName"].ToString().Trim().ToUpper() + "$$" + dsActualPatInfo.Tables[0].Rows[0]["LName"].ToString().Trim() + "," + drWebPatInfo[0]["FName"].ToString().Trim().ToUpper() + "$$" + dsActualPatInfo.Tables[0].Rows[0]["FName"].ToString().Trim()), false);
   //                                         if (drWebPatInfo[0]["LName"].ToString().Trim().ToUpper().Equals(dsActualPatInfo.Tables[0].Rows[0]["LName"].ToString().Trim().ToUpper()) && drWebPatInfo[0]["FName"].ToString().Trim().ToUpper().Equals(dsActualPatInfo.Tables[0].Rows[0]["FName"].ToString().Trim().ToUpper()) && dtDOBWeb == dtDOBAct)
   //                                         {
   //                                             DataRow oQueueRow = oDsQueueRec.Tables[0].NewRow();

   //                                             oQueueRow["RXNO"] = lRxNo.ToString();
   //                                             oQueueRow["DATE_QUED"] = System.DateTime.Now;

   //                                             //#######################################################
   //                                             //Enhanced by Khurram Jalil on 22-Aug-2011
   //                                             //As this method will be called by WCFRefillMyRxClient
   //                                             //so hardcode "PHARMACIST" & "SENTBYPROG" as "MMS" & "FR" respectively
   //                                             /*
   //                                             if (AppGlobal.gPhUser == null)
   //                                                 oQueueRow["PHARMACIST"] = "MMS";
   //                                             else
   //                                                 oQueueRow["PHARMACIST"] = AppGlobal.gPhUser.PH_INIT;
   //                                             oQueueRow["SENTBYPROG"] = "PH";
   //                                             */

   //                                             oQueueRow["PHARMACIST"] = "MMS";
   //                                             oQueueRow["SENTBYPROG"] = "FR";
   //                                             //#######################################################

   //                                             if (dsRxToQueue.Tables[0].Columns.Contains("BILLINS"))
   //                                                 oQueueRow["INS"] = dsRxToQueue.Tables[0].Rows[0]["BILLINS"].ToString();
   //                                             else
   //                                                 oQueueRow["INS"] = dsRxToQueue.Tables[0].Rows[0]["PATTYPE"].ToString();

   //                                             //oQueueRow["ActionID"] = dsRxs.Tables[0].Rows[0]["ActionID"].ToString();
   //                                             oDsQueueRec.Tables[0].Rows.Add(oQueueRow);

   //                                             //Commented out by Khurram Jalil on 14-Dec-2010
   //                                             //Don't accept changes on dataset otherwise these changes will not be saved to DB
   //                                             //oDsQueueRec.AcceptChanges();

   //                                             dr["STATUS"] = dr["STATUS"] + ",Rx successfully queued for refill";
   //                                         }
   //                                         else
   //                                         {
   //                                             dr["STATUS"] = dr["STATUS"] + ",Pat UnVerified";
   //                                         }
   //                                     }
   //                                     else
   //                                         dr["STATUS"] = "unable to get details for specified rx";
   //                                 }
   //                                 else
   //                                 {
   //                                     dr["STATUS"] = "unable to get details for specified rx";
   //                                 }


   //                             }
   //                         }
   //                         else
   //                             dr["STATUS"] = "Unable to get details for specified Rx";
   //                     }
   //                     catch (Exception oIEx)
   //                     {
   //                         ErrorHandler.ShowMessage(null, oIEx, false);
   //                         dr["STATUS"] = oIEx.Message;
   //                     }
   //                 }
   //                 else
   //                     dr["STATUS"] = "Specified RxNo is invalid";
   //             }

   //             ////Call the save method on dataset to queue all Rxs in 1 go
   //             //oContRefQue.SaveDS(oDsQueueRec);
   //             oContRefQue.InsertInRxRefQ(oDsQueueRec);
   //             //Commented out by Khurram Jalil on 14-Dec-2010 after completing debug testing
   //             //bool bSaved = oContRefQue.SaveDS(oDsQueueRec);
   //             //ErrorHandler.ShowMessage(null, new Exception("Refill queued records = "+oDsQueueRec.Tables[0].Rows.Count+" and save status="+bSaved.ToString()), false);
   //             //############################################################
   //             //dsRxs.WriteXml(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\RequestedRefills_" + sFileName + ".xml");

   //             //Loop to check Refill Status of valid Rxs and update comments of Specified Rx
   //             //foreach (DataRow dr in dsRxs.Tables[0].Select("STATUS='Rx successfully queued for refill'"))
   //             //{
   //             //    oContRefQue.UpdateField(dr["Remark"].ToString(), "REMARK", dr["RXNO"].ToString());
   //             //}

   //             //ErrorHandler.ShowMessage(null, new Exception("Total Rxs req = " + dsRxsReq.Tables[0].Rows.Count+" and Total Validated Rxs = "+dsRxs.Tables[0].Rows.Count), false);
   //             if (dsRxs != null && dsRxs.Tables.Count > 0
   //                 && dsRxsReq != null && dsRxsReq.Tables.Count > 0)
   //             {
   //                 foreach (DataRow dr in dsRxsReq.Tables[0].Rows)
   //                 {
   //                     long xRxNo = 0;
   //                     string sFName = "", sLName = "", sPhone = "", sMobile = "", sEmail = "", sStatus = "";
   //                     DateTime dtDOB = DateTime.MinValue;
   //                     //===============added by Kashif Abbasi on 12-Dec-2012==================
   //                     string sCallBackNo, sDeliveryMethod, sPickUpTime, sDeliveryAddress, sComments, sUserStatus;
   //                     DateTime dtPickUpDate = DateTime.MinValue;
   //                     bool bRefillConfirmation, bRefillReminder;
   //                     //=======================================================================
   //                     DataRow[] drCol = dsRxs.Tables[0].Select("RXNO = '" + dr["RxNo"].ToString() + "'");
   //                     if (drCol != null && drCol.Length > 0)
   //                         sStatus = drCol[0]["STATUS"].ToString();

   //                     long.TryParse(dr["RxNo"].ToString(), out xRxNo);
   //                     sFName = dr["FName"].ToString();
   //                     sLName = dr["LName"].ToString();
   //                     sPhone = dr["Phone"].ToString();
   //                     sMobile = dr["Mobile"].ToString();
   //                     sEmail = dr["Email"].ToString();
   //                     DateTime.TryParse(dr["DOB"].ToString(), out dtDOB);

   //                     //===============added by Kashif Abbasi on 12-Dec-2012==================

   //                     sCallBackNo = dr["CallBackNo"].ToString();
   //                     sDeliveryMethod = dr["DeliveryMethod"].ToString();
   //                     DateTime.TryParse(dr["PickUpDate"].ToString(), out dtPickUpDate);
   //                     sPickUpTime = dr["PickUpTime"].ToString();
   //                     sDeliveryAddress = dr["DeliveryAddress"].ToString();
   //                     bRefillConfirmation = Convert.ToBoolean(dr["RefillConfirmation"].ToString());
   //                     bRefillReminder = Convert.ToBoolean(dr["RefillReminder"].ToString());
   //                     sComments = dr["Comments"].ToString();
   //                     sUserStatus = dr["UserStatus"].ToString();

   //                     //=======================================================================
   //                     //=================ADDED BY KASHIF ABBASI ON 30-Jan-2012================
   //                     long RxRefQID = 0;
   //                     char RxVerified = 'U';// RxVerified = 'U' mean it is unverified, and it will consider as verified(RxVerified = 'V' when RxRefQID != 0)
   //                     if (oDsQueueRec.Tables[0].Rows.Count > 0)
   //                     {
   //                         DataRow[] drRxRefQs = oDsQueueRec.Tables[0].Select("RXNO = '" + xRxNo.ToString() + "'");
   //                         if (drRxRefQs != null && drRxRefQs.Length > 0)
   //                         {
   //                             long.TryParse(drRxRefQs[0]["ID"].ToString(), out RxRefQID);
   //                             RxVerified = 'V';
   //                         }
   //                     }

   //                     //=================END ADDED BY KASHIF ABBASI ON 30-Jan-2012================

   //                     //ErrorHandler.ShowMessage(null, new Exception(xRxNo.ToString() + " $$ " + sFName + " $$ " + sLName + " $$ " + dtDOB.ToString() +" $$ " + sPhone + " $$ " + sMobile + " $$ " + sEmail + " $$ " + sStatus+ " $$  callback=" + sCallBackNo+ " $$ DeliverMethod:" +sDeliveryMethod+ " $$ pickUp:" +dtPickUpDate.ToString() ), false);

   //                     ContFMRPatRxRequest cfmr = new ContFMRPatRxRequest();

   //                     cfmr.Insert(xRxNo, sFName, sLName, dtDOB, sPhone, sMobile, sEmail, sStatus, sCallBackNo, sDeliveryMethod, dtPickUpDate, sPickUpTime, sDeliveryAddress, bRefillConfirmation, bRefillReminder, sComments, sUserStatus, RxRefQID, RxVerified);
   //                 }
   //             }
   //             return dsRxs;
   //         }
   //         catch (Exception oEx)
   //         {
   //             sErrorMessage = oEx.ToString();
   //             //As this method will be called from command line so just write to error log file
   //             //without showing popup message i.e. set bShowMessage parameter to false                
   //             ErrorHandler.ShowMessage(null, oEx, false);
   //             return null;
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }

   //     //Engineered by Khurram Jalil on 18-Aug-2011
   //     private string GetCSRxs(DataSet dsRxsReq, out string sErrorMessage)
   //     {
   //         sErrorMessage = "";
   //         StringBuilder sbRxs = new StringBuilder();
   //         try
   //         {
   //             if (dsRxsReq != null && dsRxsReq.Tables.Count > 0)
   //             {
   //                 foreach (DataRow dr in dsRxsReq.Tables[0].Rows)
   //                 {
   //                     sbRxs.Append(dr["RxNo"].ToString() + ",");
   //                 }
   //             }
   //         }
   //         catch (Exception oEx)
   //         {
   //             sErrorMessage = oEx.ToString();
   //             ErrorHandler.ShowMessage(null, oEx, false);
   //         }

   //         if (sbRxs.ToString().Trim().Length > 1)
   //             return sbRxs.ToString().Substring(0, sbRxs.Length - 1);
   //         else
   //             return "";
   //     }
   //     //Above code commented by Muhammad Shahid
   //     //public DataSet PerformRxRefill(string sNABPNo, string sCSVRxs)
   //     //{
   //     //    DataSet dsRxs = new DataSet();
   //     //    try
   //     //    {
   //     //        ContClaims cClm = new ContClaims();
   //     //        dsRxs = cClm.ValidateNABPandRxNo(sNABPNo, sCSVRxs);
   //     //        long lRxNo;

   //     //        //############################################################
   //     //        //Loop to set Refill Status of valid Rxs
   //     //        foreach (DataRow dr in dsRxs.Tables[0].Select("STATUS=''"))
   //     //        {
   //     //            int DaysLeft;
   //     //            DateTime DueDate;
   //     //            decimal QtyLeft;

   //     //            if (Int64.TryParse(dr["RXNO"].ToString(), out lRxNo))
   //     //            {
   //     //                try
   //     //                {
   //     //                    dr["STATUS"] = cClm.IsRxRefillable(lRxNo, out DaysLeft, out DueDate, out QtyLeft).ToString();
   //     //                }
   //     //                catch (Exception oIEx)
   //     //                {
   //     //                    ErrorHandler.ShowMessage(null, oIEx, false);
   //     //                }
   //     //            }
   //     //            else
   //     //            {
   //     //                dr["STATUS"] = "Specified RxNo is invalid";
   //     //            }
   //     //        }
   //     //        //############################################################
   //     //        //Loop to add Rxs due for Refill into the Refill Queue
   //     //        ContClaims oContClaims = new ContClaims();
   //     //        ContRxRefQue oContRefQue = new ContRxRefQue();
   //     //        ContPatient oContPat = new ContPatient();
   //     //        ContPrescrib oContPrs = new ContPrescrib();
   //     //        ContDrug oContDrg = new ContDrug();

   //     //        DataSet dsRxToQueue, dsQueuedRx;
   //     //        dsQueuedRx = oContRefQue.GetQueRecs("-1");
   //     //        DataSet oDsQueueRec = oContRefQue.GetRxRefQue("-1");

   //     //        foreach (DataRow dr in dsRxs.Tables[0].Select("STATUS='" + RxRefillStatus.OkToRefill.ToString() + "'"))
   //     //        {
   //     //            if (Int64.TryParse(dr["RXNO"].ToString(), out lRxNo))
   //     //            {
   //     //                try
   //     //                {
   //     //                    oContClaims.GetRxSelection(lRxNo.ToString(), "", out dsRxToQueue, true, "");

   //     //                    if (dsQueuedRx != null && dsQueuedRx.Tables.Count > 0 && dsQueuedRx.Tables[0].Select("RXNO = '" + lRxNo.ToString() + "'").Length > 0)
   //     //                        dr["STATUS"] = "Specified Rx is already queued for refill";
   //     //                    else if (dsRxToQueue != null && dsRxToQueue.Tables.Count > 0 && dsRxToQueue.Tables[0].Rows.Count > 0)
   //     //                    {
   //     //                        if (!oContPat.RecExist(dsRxToQueue.Tables[0].Rows[0]["PATIENTNO"].ToString()))
   //     //                            dr["STATUS"] = "Patient for specified Rx does not exist";
   //     //                        else if (!oContPrs.RecExist(dsRxToQueue.Tables[0].Rows[0]["PRESNO"].ToString()))
   //     //                            dr["STATUS"] = "Prescriber for specified Rx does not exist";
   //     //                        else if (!oContDrg.ValidDrugNdc(dsRxToQueue.Tables[0].Rows[0]["NDC"].ToString()))
   //     //                            dr["STATUS"] = "Drug NDC for specified Rx is invalid";
   //     //                        else
   //     //                        {
   //     //                            DataRow oQueueRow = oDsQueueRec.Tables[0].NewRow();

   //     //                            oQueueRow["RXNO"] = lRxNo.ToString();
   //     //                            oQueueRow["DATE_QUED"] = System.DateTime.Now;
   //     //                            if (AppGlobal.gPhUser == null)
   //     //                                oQueueRow["PHARMACIST"] = "MMS";
   //     //                            else
   //     //                                oQueueRow["PHARMACIST"] = AppGlobal.gPhUser.PH_INIT;
   //     //                            oQueueRow["SENTBYPROG"] = "PH";
   //     //                            oQueueRow["INS"] = dsRxToQueue.Tables[0].Rows[0]["PATTYPE"].ToString();

   //     //                            oDsQueueRec.Tables[0].Rows.Add(oQueueRow);

   //     //                            //Commented out by Khurram Jalil on 14-Dec-2010
   //     //                            //Don't accept changes on dataset otherwise these changes will not be saved to DB
   //     //                            //oDsQueueRec.AcceptChanges();

   //     //                            dr["STATUS"] = "Rx successfully queued for refill";
   //     //                        }
   //     //                    }
   //     //                    else
   //     //                        dr["STATUS"] = "Unable to get details for specified Rx";
   //     //                }
   //     //                catch (Exception oIEx)
   //     //                {
   //     //                    ErrorHandler.ShowMessage(null, oIEx, false);
   //     //                    dr["STATUS"] = oIEx.Message;
   //     //                }
   //     //            }
   //     //            else
   //     //                dr["STATUS"] = "Specified RxNo is invalid";
   //     //        }

   //     //        //Call the save method on dataset to queue all Rxs in 1 go
   //     //        oContRefQue.SaveDS(oDsQueueRec);

   //     //        //Commented out by Khurram Jalil on 14-Dec-2010 after completing debug testing
   //     //        //bool bSaved = oContRefQue.SaveDS(oDsQueueRec);
   //     //        //ErrorHandler.ShowMessage(null, new Exception("Refill queued records = "+oDsQueueRec.Tables[0].Rows.Count+" and save status="+bSaved.ToString()), false);
   //     //        //############################################################
   //     //        //dsRxs.WriteXml(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\RequestedRefills_" + sFileName + ".xml");
   //     //    }
   //     //    catch (Exception oEx)
   //     //    {
   //     //        //As this method will be called from command line so just write to error log file
   //     //        //without showing popup message i.e. set bShowMessage parameter to false
   //     //        ErrorHandler.ShowMessage(null, oEx, false);

   //     //    }
   //     //    return dsRxs;
   //     //}



   //     /// <summary>
   //     /// IMPLEMENTED BY: Shoaib Ahmed
   //     /// DATE: 30-Dec-2010
   //     /// PURPOSE: Method to be executed by Window Service for Sending Dose Alert.
   //     /// LOGIC: Get all Rxs which are due for Dose from DB, Get detailed Info of these Rxs.
   //     /// send SMS/Email or Both to Patients of these Rxs 
   //     /// CAUTION: Don't change name of this method otherwise DoseAlert window service will not be able to use it
   //     /// LAST MODIFIED: 30-Dec-2010
   //     /// LAST MODIFIED BY: Shoaib Ahmed
   //     /// </summary>
   //     public void SendDoseAlert()
   //     {
   //         try
   //         {
   //             if (IsTracingEnabled)
   //             {
   //                 ErrorHandler.ShowMessage(null, new Exception("Send Dose Alert Started"), false);
   //             }
   //             ContMessagingCom oCMsgCom = new ContMessagingCom();
   //             MessagingCom.AddDoseAlertMessageInQueue();
   //             //MessagingCom.SendDoseAlertMessage(oCMsgCom.GetDoseAlertsDue());
   //             if (IsTracingEnabled)
   //             {
   //                 ErrorHandler.ShowMessage(null, new Exception("Send Dose Alert Ended"), false);
   //             }
   //         }
   //         catch (Exception oEx)
   //         {
   //             //As this method will be called from command line so just write to error log file
   //             //without showing popup message i.e. set bShowMessage parameter to false
   //             ErrorHandler.ShowMessage(null, oEx, false);
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }

   //     /// <summary>
   //     /// IMPLEMENTED BY: Shoaib Ahmed
   //     /// DATE: 02-Nov-2012
   //     /// PURPOSE: Method to be executed from command line or by PrimeRx Schedular for
   //     /// Cleaning up TrackPatient Data which is more than a year old.
   //     /// LOGIC:  
   //     /// CAUTION: Don't change name of this method otherwise PrimeRx Schedular will not be able to use it
   //     /// LAST MODIFIED: 09-Nov-2012
   //     /// LAST MODIFIED BY: Shoaib Ahmed
   //     /// </summary>
   //     public bool CleanupData(int TaskId, ref string filePath, bool bSendPrint, ref string sNoOfRecordAffect)
   //     {
   //         try
   //         {
   //             if (IsTracingEnabled)
   //             {
   //                 ErrorHandler.ShowMessage(null, new Exception("Cleanup Data Started"), false);
   //             }
   //             //ContPatient oContPatient= new ContPatient();
   //             //oContPatient.CleanupTrackPatientData();
   //             ContGenericTableAccess oContGenericTableAccess = new ContGenericTableAccess();
   //             return oContGenericTableAccess.CleanupData(TaskId, ref filePath, bSendPrint, ref sNoOfRecordAffect);
   //             if (IsTracingEnabled)
   //             {
   //                 ErrorHandler.ShowMessage(null, new Exception("Cleanup Data Ended"), false);
   //             }
   //         }
   //         catch (Exception oEx)
   //         {
   //             //As this method will be called from command line so just write to error log file
   //             //without showing popup message i.e. set bShowMessage parameter to false
   //             ErrorHandler.ShowMessage(null, oEx, false);
   //             return false;
   //         }
   //         finally
   //         {
   //             AppMain.bApplicationClosed = true;
   //         }
   //     }
    }
}
