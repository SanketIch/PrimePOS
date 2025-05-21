using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using POS.Devices;
using NLog;

namespace MMS.Device
{
    /// <summary>
    /// This is the PinPad service object.  All PinPad functionality is about in this class
    /// </summary>
    /// <Author>Author: Manoj Kumar</Author>
    
    class OPOSPinPad
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        #region Variables
        private OPOSPINPadClass OPOSPin = null;
        public delegate void PinEvent(ArrayList PinInfo);
        public event PinEvent PinBlock = delegate { };
        private int Status = 0;
        #endregion

        public OPOSPinPad()
        {
            EnablePinPad();
        }
        public void DisablePinPad()
        {
            if (!Constant.isPinRelease && Constant.isPinEnable)
            {     
                logger.Debug("\t\t\tPOS send message to cancel Pin Entry");
                ReleasePin();
            }
        }

        #region Enable PINPAD
        /// <summary>
        /// Enable the PinPad, must OPEN, CLAIM, ENABLE
        /// </summary>
        /// <returns></returns>
        private bool EnablePinPad()
        {
            int rtCode = 0;
            bool isEnable = false;
            try
            {
                logger.Debug("\t\t\tEntering PinPad");
                Constant.isPinEnable = false;
                if(OPOSPin == null)
                {
                    OPOSPin = new OPOSPINPadClass();
                    rtCode = OPOSPin.Open(Constant.deviceName);
                    logger.Debug("\t\t\tIs PinPad Open?: " + rtCode);
                    rtCode = OPOSPin.ClaimDevice(5000);
                    logger.Debug("\t\t\tIs PinPad Claimed?: " + rtCode);
                    OPOSPin.ClearInput();
                    OPOSPin.DeviceEnabled = true;
                    OPOSPin.DataEventEnabled = true;
                    OPOSPin.AccountNumber = Constant.ccinfo.ToString().Trim();
                    OPOSPin.BeginEFTTransaction("DUKPT", 1);
                    OPOSPin.EnablePINEntry();
                    OPOSPin.DataEvent += OPOSPin_DataEvent; //event to capture the pin
                    OPOSPin.DirectIOEvent += OPOSPin_DirectIOEvent;
                    Constant.isPinEnable = true;
                    logger.Debug("\t\t\tEnablePinPad OPOSPINPAD");
                }
                logger.Debug("\t\t\tExiting PinPad: " + Constant.isPinEnable);
                isEnable = Constant.isPinEnable;
            }
            catch(Exception ex)
            {
                logger.Debug("Error in EnablePINPAD OPOSPINPAD \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            return isEnable;
        }

        void OPOSPin_DirectIOEvent(int EventNumber, ref int pData, ref string pString)
        {
            logger.Debug("\t\t\tDirect IO Event");
            ReleasePin();
            EnablePinPad();
        }
        #endregion


        #region Release PINPAD
        /// <summary>
        /// Release the PinPad when finish
        /// </summary>
        public void ReleasePin()
        {
            int rtCode = 0;
            try
            {
                logger.Debug("\t\t\tEntering ReleasePinPad");
                Constant.isPinRelease = false;
                if(OPOSPin != null && OPOSPin.Claimed)
                {
                    rtCode = OPOSPin.EndEFTTransaction(0);
                    OPOSPin.DeviceEnabled = false; // disable the pinpad device 
                    OPOSPin.DataEventEnabled = false;
                    OPOSPin.DataEvent -= OPOSPin_DataEvent; //event to capture the pin
                    OPOSPin = null;
                    Constant.isPinRelease = true;
                    Constant.isPinEnable = false;
                    logger.Debug("\t\t\tPinPad Release: "+ rtCode);
                }
                logger.Debug("\t\t\tReleasePinPad");
            }
            catch(Exception ex)
            {
                logger.Error("Error in ReleasePin OPOSPINPAD \n"+ ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                OPOSPin = null;
                if(Status != 2)
                {
                    Constant.ccinfo = string.Empty;
                }
            }
        }
        #endregion
        /// <summary>
        /// Event for when Pin is entered
        /// </summary>
        /// <param name="Status"></param>
        void OPOSPin_DataEvent(int Status)
        {
            string security = string.Empty;
            string pin = string.Empty;
            string pinCode = string.Empty;
            this.Status = Status;

            if(Status == 1)
                OPOSPin.DataEvent -= OPOSPin_DataEvent; //remove event only if the pinpad is successful
            else
            {
                logger.Debug("\t\t\tPinPad Data Event Entered. User click Cancel");
                OPOSPin.DataEvent -= OPOSPin_DataEvent;
                ReleasePin();
                EnablePinPad();
                return;
            }

            ArrayList pinBlockCode = new ArrayList();
            try
            {           
                 logger.Debug("\t\t\tPinPad Data Event Entered");
                 pin = OPOSPin.EncryptedPIN; //Encrypt the PIN
                 security = OPOSPin.AdditionalSecurityInformation; //security for Pin
                 pinCode = pin.Trim() + security.Trim(); //combile PIN + Security
                 pinBlockCode.Add("PINBLOCK");
                 pinBlockCode.Add(pinCode);
                 PinEvent EventPin = null;
                 EventPin = PinBlock;
                 if(EventPin != null)
                 {
                     Constant.IsPinEvent = true;
                     EventPin(pinBlockCode);
                     logger.Debug("\t\t\tPinBlock sent");
                 }
                 else
                 {
                     logger.Debug("\t\t\tPinBlock event is NULL");                 
                 }
                 logger.Debug("\t\t\tPinPad Data Event Exited");
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                logger.Debug("\t\t\tOPOSPIN DataEvent was fired");
                ReleasePin();//release the PINPAD if status is 1
                Constant.ccinfo = string.Empty;
            }
        }
    }
}
