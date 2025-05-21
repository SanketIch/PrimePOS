using System;
using System.Collections.Generic;
using System.Text;
using MMSComponentInfo;

namespace POS_Core.Resources
{
    public  class MMSComponentInfoUtil
    {
        private  System.ComponentModel.BackgroundWorker backGroundWorker;
        SearchComponents oSearchComponents;

        public MMSComponentInfoUtil()
        {
            backGroundWorker = new System.ComponentModel.BackgroundWorker();
            backGroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(backGroundWorker_DoWork);
        }

        public  void SearchAndUpdateComponentInfo()
        {
            backGroundWorker.RunWorkerAsync();
            
        }

        private void backGroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            LogHandler oLogHandler;
            

            oLogHandler = new LogHandler(AddToLog);
            oSearchComponents = new SearchComponents(oLogHandler, GetApplicationList());

            oSearchComponents.Search();
            
            SaveResultInDB();
        }

        private void SaveResultInDB()
        {
            POS_Core.DataAccess.MMSComponentInfoSvr componentSvr = new POS_Core.DataAccess.MMSComponentInfoSvr();
            componentSvr.Persist(oSearchComponents.ApplicationList);
        }

        private PrimeApplicationList GetApplicationList()
        {

            PrimeApplicationList oPrimeAppList = new PrimeApplicationList();
            PrimeApplication oApp = new PrimeApplication("PrimeRX", "PHW.EXE");
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oApp.ProcessName = "PHW";
            oApp.SearchPathList.Add(@"f:\phwin");
            oApp.SearchPathList.Add(@"c:\phwin");
            oApp.SearchPathList.Add(@"d:\phwin");
            oPrimeAppList.Add(oApp);

            oApp = new PrimeApplication("Trans Claim", "TransclmM.exe");
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oApp.SearchAllLocations = true;
            oApp.ProcessName = "TRANSCLMM";
            oApp.SearchPathList.Add(@"f:\transclmm");
            oApp.SearchPathList.Add(@"c:\transclmm");
            oPrimeAppList.Add(oApp);

            oApp = new PrimeApplication("PrimeESC", "Padhost.exe");
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oApp.SearchAllLocations = true;
            oApp.ProcessName = "PADHOST";
            oApp.SearchPathList.Add(@"c:\padhost");
            oPrimeAppList.Add(oApp);

            oApp = new PrimeApplication("UpdateW", "Updatew.exe");
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oApp.ProcessName = "UPDATEW";
            oApp.SearchPathList.Add(@"f:\phwin");
            oApp.SearchPathList.Add(@"c:\phwin");
            oApp.SearchPathList.Add(@"d:\phwin");
            oPrimeAppList.Add(oApp);

            oApp = new PrimeApplication("PrimeUpdate", "AppStart.exe");
            oApp.ProcessName = "APPSTART";
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oApp.SearchPathList.Add(@"c:\PrimeUpdate");
            oPrimeAppList.Add(oApp);

            oApp = new PrimeApplication("PrimeDelivery", "PrimeDelivery.exe");
            oApp.ProcessName = "PRIMEDELIVERY";
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oApp.SearchPathList.Add(@"f:\phwin");
            oApp.SearchPathList.Add(@"c:\phwin");
            oApp.SearchPathList.Add(@"d:\phwin");
            oPrimeAppList.Add(oApp);

            oApp = new PrimeApplication("DispenseInterface", "DispenseInterface.exe");
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oApp.ProcessName = "DISPENSEINTERFACE";
            oApp.SearchPathList.Add(@"c:\DispenseInterface");
            oPrimeAppList.Add(oApp);

            oApp = new PrimeApplication("FaxServer", "FaxServer.exe");
            oApp.ProcessName = "FAXSERVER";
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oPrimeAppList.Add(oApp);

            oApp = new PrimeApplication("MMSERXCOMM", "MMSERXCOMM.exe");
            oApp.ProcessName = "MMSERXCOMM";
            oApp.CheckLocalProcesses = true;
            oApp.CheckNetworkProcesses = false;
            oPrimeAppList.Add(oApp);

            return oPrimeAppList;
        }

        private void AddToLog(String logMessage)
        { }

        ~MMSComponentInfoUtil()
        {
            backGroundWorker.Dispose();
        }
    }
}
