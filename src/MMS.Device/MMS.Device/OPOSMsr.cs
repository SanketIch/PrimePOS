using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using POS.Devices;
using NLog;

namespace MMS.Device
{
    class OPOSMsr
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        private OPOSMSRClass OposMSR = null;
        private int swipe = 0;
        private bool isSwipe;
        static private ArrayList _cardData = new ArrayList();
        public delegate void MsrEvent(ArrayList ccinfo);
        public event MsrEvent msr = delegate { };

        /// <summary>
        /// Get the Credit Card Data
        /// </summary>
        public ArrayList CardData
        {
            get { return _cardData; }
        }

        /// <summary>
        /// Get or Set if the card is swipe
        /// </summary>
        public bool IsSwipe
        {
            get { return isSwipe; }
            set { isSwipe = value; }
        }

        /// <summary>
        /// This method enable the MSR/swiper on the device.
        /// Device must be OPEN, CLAIM, ENABLE
        /// </summary>
        /// <returns></returns>
        public bool EnableMSR()
        {
            bool isEnable = false;
            int rtCode = 0;
            Constant.isMsrEnbale = false;
            try
            {
                while(!isEnable)
                {
                    try
                    {
                        if(OposMSR == null)
                        {
                            OposMSR = new OPOSMSRClass();
                            rtCode = OposMSR.Open(Constant.deviceName);//open the device
                            if(rtCode == (int) OPOS_Constants.OPOS_SUCCESS)
                            {
                                OposMSR.ClaimDevice(5000);//Claim the device
                                OposMSR.AutoDisable = true;
                                OposMSR.DataEvent += OposMSR_DataEvent;
                                OposMSR.ErrorEvent += OposMSR_ErrorEvent;
                                OposMSR.DeviceEnabled = true;
                                OposMSR.DataEventEnabled = true;
                                isEnable = true;
                                Constant.isMsrEnbale = true;
                                logger.Debug("\t\tEnable MSR Swiper");
                            }
                        }
                        else
                        {
                            if(ReEnableMSR())
                            {
                                break;
                            }
                            else
                            {
                                isEnable = false;
                                OposMSR = null;
                            }
                        }                      
                    }
                    catch(Exception)
                    {
                        ReEnableMSR();
                        isEnable = false;
                    }
                }
                logger.Debug("\t\tExiting MSR Swiper");
            }
            catch(Exception ex)
            {
                logger.Error("Error in EnableMSR \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            return isEnable;
        }

        /// <summary>
        /// MSR(swiper) Error Event.  Activate if MSR encounter error
        /// </summary>
        /// <param name="ResultCode"></param>
        /// <param name="ResultCodeExtended"></param>
        /// <param name="ErrorLocus"></param>
        /// <param name="pErrorResponse"></param>
        void OposMSR_ErrorEvent(int ResultCode, int ResultCodeExtended, int ErrorLocus, ref int pErrorResponse)
        {
            logger.Debug("\t\t*****Entering OPOSMSR ErrorEvent");
            ReEnableMSR();
            logger.Debug("\t\t*****Exiting OPOSMSR ErrorEvent");
        }

        /// <summary>
        /// Disable event if the user click on the cancel button on POS
        /// </summary>
        public void DisableMsr()
        {
            if (OposMSR != null && Constant.isMsrEnbale && !Constant.isMsrRelease)
            {
                OposMSR.DataEvent -= OposMSR_DataEvent;
                OposMSR.DeviceEnabled = false;
                OposMSR.DataEventEnabled = false;
                Constant.isMsrRelease = true;
                Constant.isMsrEnbale = false;
            }
            _cardData = new ArrayList();
            Constant.CCDataSwipe = new ArrayList();
        }
        /// <summary>
        /// Disable the sigPad after use or when the pad is close.
        /// </summary>
        /// <returns></returns>
        public bool ReEnableMSR()
        {
            bool isReEnable = false;
            try
            {
                logger.Debug("\t\tEntering OPOSMSR ReEnableMSR");
                if (!Constant.isMsrRelease && Constant.isMsrEnbale)
                {
                    if (OposMSR != null && OposMSR.Claimed)
                    {
                        OposMSR.DataEvent -= OposMSR_DataEvent;
                        OposMSR.DeviceEnabled = false;
                        OposMSR.DataEventEnabled = false;
                        Constant.isMsrRelease = true;
                        Constant.isMsrEnbale = false;   
                        logger.Debug("\t\tOPOSMSR Release");
                        logger.Debug("\t\tExiting OPOSMSR ReEnableMSR");
                    }
                }

                if (OposMSR != null && OposMSR.Claimed)
                {
                    OposMSR.DeviceEnabled = true;
                    OposMSR.DataEventEnabled = true;
                    OposMSR.AutoDisable = true;
                    OposMSR.ClearInput();
                    OposMSR.DataEvent += OposMSR_DataEvent;
                    OposMSR.ErrorEvent += OposMSR_ErrorEvent;
                    isReEnable = true;
                    Constant.isMsrEnbale = true;
                    Constant.isMsrRelease = false;
                    logger.Debug("\t\tExiting OPOSMSR ReEnableMSR");
                }
                else if (OposMSR == null || OposMSR.Claimed)
                {
                    isReEnable = false;
                    logger.Debug("\t\tExiting OPOSMSR ReEnableMSR OposMSR = NULL and is disable");
                }
             
            }
            catch(Exception ex)
            {
                logger.Error("\t\tError in DisableMSR \n" + ex.ToString());
                throw new Exception(ex.Message.ToString());
            }
            return isReEnable;
        }

        /// <summary>
        /// Event for when the user swipe the card, get the data from card
        /// </summary>
        /// <param name="Status"></param>
        public void OposMSR_DataEvent(int Status)
        {
            OposMSR.DataEvent -= OposMSR_DataEvent;
            object obj = new object();
            string track2 = string.Empty;
            try
            {   
                lock(obj)
                {
                    swipe++;
                    isSwipe = false;
                    if(swipe <= 5 && !Constant.InMSR)
                    {
                        Constant.InMSR = true;
                        if(_cardData == null)
                        {
                            _cardData = new ArrayList();
                        }
                        else
                        {
                            _cardData.Clear();
                        }
                        if(Constant.CCDataSwipe == null)
                        {
                            Constant.CCDataSwipe = new ArrayList();
                        }
                        else
                        {
                            Constant.CCDataSwipe.Clear();
                        }
                        OposMSR.DataEvent -= OposMSR_DataEvent;
                        OposMSR.DataEventEnabled = false;
                        OposMSR.DeviceEnabled = false;
                        track2 = OposMSR.Track2Data; //get the Data from Device
                        if(track2.Trim() != string.Empty)
                        {
                            logger.Debug("\t\tOPOSMSR ->Begin");
                            _cardData.Add(OposMSR.AccountNumber);
                            logger.Debug("\t\tOPOSMSR AccountNumber is Empty?: " + string.IsNullOrEmpty(OposMSR.AccountNumber));
                            _cardData.Add(OposMSR.ExpirationDate);
                            logger.Debug("\t\tOPOSMSR ExpirationDate is Empty?: " + string.IsNullOrEmpty(OposMSR.ExpirationDate));
                            _cardData.Add(OposMSR.FirstName);
                            logger.Debug("\t\tOPOSMSR FirstName is Empty?: " + string.IsNullOrEmpty(OposMSR.FirstName));
                            _cardData.Add(OposMSR.Surname);
                            logger.Debug("\t\tOPOSMSR Surname is Empty?: " + string.IsNullOrEmpty(OposMSR.Surname));
                            _cardData.Add(OposMSR.Track1Data);
                            logger.Debug("\t\tOPOSMSR Track1Data is Empty?: " + string.IsNullOrEmpty(OposMSR.Track1Data));
                            _cardData.Add(OposMSR.Track2Data);
                            logger.Debug("\t\tOPOSMSR Track2Data is Empty?: " + string.IsNullOrEmpty(OposMSR.Track2Data));
                            _cardData.Add(OposMSR.Track3Data);
                            logger.Debug("\t\tOPOSMSR Track3Data is Empty?: " + string.IsNullOrEmpty(OposMSR.Track3Data));
                            swipe = 0;
                            logger.Debug("\t\tOPOSMSR Data count: " + _cardData.Count);
                            Constant.CCDataSwipe = _cardData;
                            logger.Debug("\t\tOPOSMSR ccData count: " + Constant.CCDataSwipe.Count);

                            MsrEvent EventMsr = null;
                            EventMsr = msr;
                            if(EventMsr != null)
                            {
                                Constant.IsMsrEvent = true;
                                EventMsr(_cardData); //send to the event
                            }
                            else
                            {
                                Constant.IsMsrNotOK = true;
                                logger.Debug("\t\tOPOSMSR msr event is NULL");
                            }

                        }
                        else
                        {
                            logger.Debug("\t\tOPOSMSR TRACK 2 Data is empty <<<--");
                        } 

                    }
                    else
                    {
                        logger.Debug("\t\tOPOSMSR MAX Swipe 5+");
                    }
                }
            }
            catch(Exception ex) { logger.Error("\t\tError in OPOSMSR DataEvent \n" + ex.ToString()); }
            finally
            {
                logger.Debug("\t\tOPOSMSR DataEvent was fired, swipe = "+swipe);
                Constant.InMSR = false;
                Constant.isMsrRelease = true;
                Constant.isMsrEnbale = false;
                swipe = 0;
                if(Constant.IsMsrNotOK)
                {
                    Constant.IsMsrNotOK = false;
                    EnableMSR();
                }
            }
        }
    }
}
