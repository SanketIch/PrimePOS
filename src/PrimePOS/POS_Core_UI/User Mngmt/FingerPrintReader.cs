using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using DPUruNet;

using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI.UserManagement
{
    public class FingerPrintReader
    {
        private const int DPFJ_PROBABILITY_ONE = 0x7fffffff;
        private const int DP_IDENTIFY_THRESHOLD_SCORE = DPFJ_PROBABILITY_ONE * 1 / 100000;

        private ReaderCollection phListReaders;
        private Reader currentReader = null;
        private string currentReaderName = string.Empty;
        private DataTable fpTable = null;
        public bool enrollmentFlag = false;
        public int captureCount = 0;

        private Action<fpReaderAction, object> fpSendMessage = null;
        public frmLogin senderOA = null;
        public frmEnrollFingerPrint senderER = null;
        List<Fmd> preEnrollmentFmds;

        public FingerPrintReader()
        {

        }

        public FingerPrintReader(Action<fpReaderAction, object> callback)
        {
            fpSendMessage = callback;
        }

        public bool FingerPrintReaderConnect()
        {
            bool rtnCode = false;

            try
            {
                phListReaders = ReaderCollection.GetReaders();
            }
            catch (Exception exp)
            {
                return rtnCode;
            }
            if (phListReaders.Count >= 1)
            {
                currentReader = phListReaders[0]; //pick the first one as default
                currentReaderName = currentReader.Description.Name;

                using (Tracer tracer = new Tracer("FingerPrintReader::FingerPrintReaderConnect"))
                {
                    Constants.ResultCode rstCode = Constants.ResultCode.DP_DEVICE_FAILURE;

                    rstCode = currentReader.Open(Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);
                    if (rstCode == Constants.ResultCode.DP_SUCCESS)
                    {
                        if (!enrollmentFlag)
                        {
                            currentReader.On_Captured += new Reader.CaptureCallback(OnCapturedLoginProcess);
                        }
                        else
                        {
                            currentReader.On_Captured += new Reader.CaptureCallback(OnCapturedEnrollment);
                            preEnrollmentFmds = new List<Fmd>();
                        }

                        if (CaptureFingerprintAsync())
                            rtnCode = true;
                    }
                }
            }
           
            return rtnCode;
        }

        public void FingerprintReaderResetData()
        {
            if (fpTable != null)
                fpTable.Dispose();
            fpTable = null;
        } 

        private bool CaptureFingerprintAsync()
        {
            using (Tracer tracer = new Tracer("FingerPrintReader::CaptureFingerprintAsync"))
            {
                try
                {
                    GetReaderStatus();

                    Constants.ResultCode captureResult = currentReader.CaptureAsync(Constants.Formats.Fid.ANSI, Constants.CaptureProcessing.DP_IMG_PROC_DEFAULT, currentReader.Capabilities.Resolutions[0]);
                    if (captureResult != Constants.ResultCode.DP_SUCCESS)
                    {
                        throw new Exception("" + captureResult);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("FingerPrintReader Error:  " + ex.Message);
                    return false;
                }
            }
        }

        private void GetReaderStatus()
        {
            using (Tracer tracer = new Tracer("FingerPrintReader::GetReaderStatus"))
            {
                Constants.ResultCode result = currentReader.GetStatus();

                if ((result != Constants.ResultCode.DP_SUCCESS))
                {
                    throw new Exception("" + result);
                }

                if ((currentReader.Status.Status == Constants.ReaderStatuses.DP_STATUS_BUSY))
                {
                    Thread.Sleep(50);
                }
                else if ((currentReader.Status.Status == Constants.ReaderStatuses.DP_STATUS_NEED_CALIBRATION))
                {
                    currentReader.Calibrate();
                }
                else if ((currentReader.Status.Status != Constants.ReaderStatuses.DP_STATUS_READY))
                {
                    throw new Exception("FingerPrintReader Status - " + currentReader.Status.Status);
                }
            }
        }

        public void DisconnectCloseReader()
        {
            if (!enrollmentFlag)
                CancelCaptureAndCloseReader(this.OnCapturedLoginProcess);
            else
                CancelCaptureAndCloseReader(this.OnCapturedEnrollment);
        }

        public void MatchFingerPrintToUser(object objFinger, out string UserID)
        {
            Fmd capturedFinger = (Fmd)objFinger;
            UserID = string.Empty;
            DBUser oDBUser = new DBUser();
            int fivWidth = capturedFinger.Width;
            int fivHeight = capturedFinger.Height;

            if (fpTable == null)
                fpTable = oDBUser.LoadAllFingerPrint();

            if (fpTable != null && fpTable.Rows.Count >= 1)
            {
                List<Fmd> preEnrollmentFmds = new List<Fmd>();

                foreach (DataRow dr in fpTable.Rows)
                {
                    Fmd enrolledFingerprint = Fmd.DeserializeXml((string)dr["Fingerprint"]);
                    preEnrollmentFmds.Add(enrolledFingerprint);
                }

                IdentifyResult identifyResult = Comparison.Identify(capturedFinger, 0, preEnrollmentFmds, DP_IDENTIFY_THRESHOLD_SCORE, 2);
                if (identifyResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception(identifyResult.ResultCode.ToString());
                }

                if (identifyResult.Indexes.Length > 0)
                {
                    int tblIndex = identifyResult.Indexes[0][0];
                    UserID = fpTable.Rows[tblIndex]["UserID"].ToString().Trim();
                }

                preEnrollmentFmds.Clear();
            }
        }

        private void CancelCaptureAndCloseReader(Reader.CaptureCallback OnCaptured)
        {
            using (Tracer tracer = new Tracer("FingerPrintReader::CancelCaptureAndCloseReader"))
            {
                if (currentReader != null)
                {
                    currentReader.CancelCapture();

                    // Dispose of reader handle and unhook reader events.
                    currentReader.Dispose();
                    currentReader = null;
                }
            }
        }

        private void OnCapturedLoginProcess(CaptureResult captureResult)
        {
            try
            {
                if (!CheckFingerprintCaptureResult(captureResult))
                    return;

                captureCount++;

                DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);

                if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    return;

                Fmd capturedFinger = resultConversion.Data;

                if (senderOA != null)
                {
                    senderOA.SendMessage(fpReaderAction.SendFingerprint, capturedFinger);
                }
                else if(fpSendMessage != null)
                {
                    fpSendMessage(fpReaderAction.SendFingerprint, capturedFinger);
                }
            }
            catch (Exception ex)
            {
                string captureError = "FingerPrintReader Error:  " + ex.Message;
                if (senderOA != null)
                    senderOA.SendMessage(fpReaderAction.SendMessage, captureError);
                else if (fpSendMessage != null)
                    fpSendMessage(fpReaderAction.SendMessage, captureError);
            }
        }

        private void OnCapturedEnrollment(CaptureResult captureResult)
        {
            try
            {
                if (!CheckFingerprintCaptureResult(captureResult))
                    return;

                captureCount++;

                foreach (Fid.Fiv fiv in captureResult.Data.Views)
                {
                    senderER.SendMessage(fpReaderAction.SendBitmap, CreateBitmap(fiv.RawImage, fiv.Width, fiv.Height));
                }

                DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);

                if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception(resultConversion.ResultCode.ToString());
                }

                preEnrollmentFmds.Add(resultConversion.Data);

                if (captureCount >= 4)
                {
                    DataResult<Fmd> resultEnrollment = DPUruNet.Enrollment.CreateEnrollmentFmd(Constants.Formats.Fmd.ANSI, preEnrollmentFmds);

                    if (resultEnrollment.ResultCode == Constants.ResultCode.DP_SUCCESS)
                    {
                        senderER.SendMessage(fpReaderAction.SendMessage, "Fingerprint was captured successfully. Please click on Enroll button to save it.");
                        Fmd enrollmentFinger = resultEnrollment.Data;
                        senderER.SendMessage(fpReaderAction.SendFMD, enrollmentFinger);
                        preEnrollmentFmds.Clear();
                        captureCount = 0;
                        return;
                    }
                    else if (resultEnrollment.ResultCode == Constants.ResultCode.DP_ENROLLMENT_INVALID_SET)
                    {
                        senderER.SendMessage(fpReaderAction.SendMessage, "Failed to capture fingerprint. Please try again.");
                        preEnrollmentFmds.Clear();
                        captureCount = 0;
                        return;
                    }
                }
                int displayCount = 4 - captureCount;
                string msgStr = "Now place the same finger on the reader ";
                if (displayCount > 0)
                    msgStr += displayCount.ToString() + " more time(s).";
                else
                    msgStr += "again.";
                senderER.SendMessage(fpReaderAction.SendMessage, msgStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("FingerPrintReader Error:  " + ex.Message);
            }

        }

        private bool CheckFingerprintCaptureResult(CaptureResult captureResult)
        {
            using (Tracer tracer = new Tracer("FingerPrintReader::CheckFingerprintCaptureResult"))
            {
                if (captureResult.Data == null || captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        throw new Exception(captureResult.ResultCode.ToString());
                    }

                    // Send message if quality shows fake finger
                    if ((captureResult.Quality != Constants.CaptureQuality.DP_QUALITY_CANCELED))
                    {
                        throw new Exception("Fingerprint Quality - " + captureResult.Quality);
                    }
                    return false;
                }

                return true;
            }
        }

        private Bitmap CreateBitmap(byte[] bytes, int width, int height)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3];

            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                rgbBytes[(i * 3)] = bytes[i];
                rgbBytes[(i * 3) + 1] = bytes[i];
                rgbBytes[(i * 3) + 2] = bytes[i];
            }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            for (int i = 0; i <= bmp.Height - 1; i++)
            {
                IntPtr p = new IntPtr(data.Scan0.ToInt64() + data.Stride * i);
                System.Runtime.InteropServices.Marshal.Copy(rgbBytes, i * bmp.Width * 3, p, bmp.Width * 3);
            }

            bmp.UnlockBits(data);

            return bmp;
        }
    }
}
