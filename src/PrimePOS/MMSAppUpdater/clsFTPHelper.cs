using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Ftp;
using System.IO;
using System.Data;

namespace MMSAppUpdater
{
    internal class clsFTPHelper
    {
        
        private FtpClient ftp;
        private string m_sFTPPassword = "";
        private string m_sFTPServer = "";
        private string m_sFTPUserName = "";
        private string m_sPassword = "";
        private string m_sUserName = "";
      
        public string FTPServer
        {
            get { return m_sFTPServer; }
            set { m_sFTPServer = value.Trim(); }
        }

        public string Password
        {
            get { return m_sPassword; }
            set { m_sPassword = value.Trim(); }
        }


        public FtpClient FTPLogin()
        {
            this.ftp.Connect(this.FTPServer);
            this.ftp.Login(this.UserName, this.Password);
            return ftp;
        }
        
        public void LogOut()
        {
            this.ftp.Disconnect();
        }
        public string UserName
        {
            get { return m_sUserName; }
            set { m_sUserName = value.Trim(); }
        }
        public void DownLoadFolder(string sSourceFolder, string sDestinationFolder, clsLogService log)
        {

            if (ftp.Connected)
            {
                this.CopyFilesFromFTP(sSourceFolder, sDestinationFolder, log, true);
            }
            
        }
        
        public clsFTPHelper(MMSAppUpdater.MMSUpdate.MmsUPdateService MmsUpdateService)
        {
            Xceed.Ftp.Licenser.LicenseKey = "FTN71-TWB3A-8DTB5-CJKA";   //PRIMEPOS-3170 02-Dec-2022 JY Modified key, old key was FTN32A7BE7JWKKY04AA;
            string Key = "MMS010B3";
            Encryption oEncryption = new Encryption(Encryption.SymmProvEnum.RC2);
            DataSet oFtpSetting = new DataSet();
            oFtpSetting = MmsUpdateService.GetFtpCredentials();
            if (oFtpSetting.Tables[0].Rows.Count > 0)
            {
                this.m_sFTPServer = oEncryption.Decrypting(oFtpSetting.Tables[0].Rows[0]["FtpAddress"].ToString(), Key).Trim();
                this.m_sUserName = oEncryption.Decrypting(oFtpSetting.Tables[0].Rows[0]["UserName"].ToString(), Key).Trim();
                this.m_sPassword = oEncryption.Decrypting(oFtpSetting.Tables[0].Rows[0]["Password"].ToString(), Key).Trim();
               

                ftp = new FtpClient();
                ftp.Connect(this.FTPServer);
                ftp.Login(this.UserName, this.Password);
            }
            else
            {
                
                throw new Exception("Failed to Login FTP \nFTP Setting does not exits");
            }
        }
        private void DownloadFileRecursively(string source, string destination, clsLogService log, bool append)
        {            
            String NextFolder = String.Empty;
            String CreateFolder = String.Empty;
            String FileName = String.Empty;
            this.ftp.ChangeCurrentFolder(source);
            FtpItemInfoList list = this.ftp.GetFolderContents();            
            foreach (FtpItemInfo item in list)
            {
                try
                {
                    if (item.Type == FtpItemType.Folder)
                    {
                        if (item.Name == "." || item.Name == "..")
                            continue;
                        NextFolder = String.Format("{0}",item.Name);
                        CreateFolder = String.Format("{0}\\{1}", destination, item.Name);
                        if (!Directory.Exists(CreateFolder))
                        {
                            Directory.CreateDirectory(CreateFolder);
                        }
                        DownloadFileRecursively(NextFolder, CreateFolder, log, append);
                        ftp.ChangeToParentFolder();
                    }
                    else if (item.Type == FtpItemType.File)
                    {
                        FileName = String.Format("{0}\\{1}",destination, item.Name);
                        ftp.ReceiveFile(item.Name, FileName);
                    }
                }
                catch (Exception ex)
                {
                    if (append)
                    {
                        log.AddError("DownloadFileRecursively - Failed to copy file " + source + " due to error - " + ex.Message);
                    }
                }
            }
        }
        private void CopyFilesFromFTP(string source,string destination, clsLogService log, bool append)
        {
          
            try
            {
                
                ftp.MultipleFileTransferError += new MultipleFileTransferErrorEventHandler(ftp_MultipleFileTransferError);
                ftp.ReceivingFile += new TransferringFileEventHandler(ftp_ReceivingFile);
                DownloadFileRecursively(source, destination, log, append);
                //this.ftp.ChangeCurrentFolder(source);
                //FtpItemInfoList list = this.ftp.GetFolderContents();
                
                //this.ftp.ReceiveMultipleFiles(String.Empty, destination, true, true);
                if (append)
                {
                    log.AddMessage(String.Format("File {0} successfully copied to {1} ", source, destination));
                }

            }
            catch (Exception ex)
            {
                if (append)
                {
                    log.AddError("CopyFilesFromFTP - Failed to copy file " + source + " due to error - " + ex.Message);
                }

            }
            finally
            {
              this.ftp.ChangeCurrentFolder("//"); // set the ftp folder back to ftp main folder
            }
          
        }

        void ftp_ReceivingFile(object sender, TransferringFileEventArgs e)
        {
            
        }

        void ftp_MultipleFileTransferError(object sender, MultipleFileTransferErrorEventArgs e)
        {
            
        }
    }
}
