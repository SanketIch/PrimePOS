using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using POS.Devices;
using NLog;

namespace MMS.Device
{
    class OPOSsig
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        private OPOSSigCapClass OPOSSigCapture = null;
        public OPOSsig()
        {
            OpenSigCap();
        }

        /// <summary>
        /// Open the Device Object SigCap
        /// </summary>
        /// <returns></returns>
        private bool OpenSigCap()
        {
            bool isEnable = false;
            try
            {
                logger.Debug("\t\t\tEntering Enable SigCapture");
                if(OPOSSigCapture == null)
                {
                    OPOSSigCapture = new OPOSSigCapClass();
                    OPOSSigCapture.Open("Verifone Mx");
                    OPOSSigCapture.ClaimDevice(2000);
                    OPOSSigCapture.DeviceEnabled = true;
                    isEnable = true;
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            return isEnable;
        }

        /// <summary>
        /// Enable the sign portion of the pad
        /// </summary>
        /// <param name="cScreen"></param>
        /// <returns></returns>
        public bool SetSigCap(string cScreen)
        {
            int retCode = 0;
            bool isBegin = false;
            try
            {
                retCode = OPOSSigCapture.BeginCapture(cScreen.ToUpper().Trim());
                isBegin = retCode != 0 ? false : true;
                if(isBegin)
                {
                   retCode = OPOSSigCapture.EndCapture();
                   isBegin = retCode != 0 ? false : true;
                }
                logger.Debug("\t\t\tEnable and Disable SigCap: " + isBegin);
                return isBegin;
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.Message.ToString());
            }
           finally
            {
                CloseSigCap();
            }
        }

        /// <summary>
        /// Close the device Object SigCap
        /// </summary>
        /// <returns></returns>
        public bool CloseSigCap()
        {
            int xsuccess = 0;
            bool isClose = false;
            try
            {
                if(OPOSSigCapture != null)
                {
                    OPOSSigCapture.DeviceEnabled = false; // disable the SigCap
                    xsuccess = OPOSSigCapture.ReleaseDevice();
                    isClose = xsuccess != 0 ? false : true;
                    logger.Debug("\t\t\tCloseSigCap Release: " + isClose);
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            return isClose;
        }
    }
}
