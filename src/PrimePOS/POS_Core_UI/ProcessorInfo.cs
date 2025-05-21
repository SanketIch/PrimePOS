using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace POS_Core_UI
{
    public class ProcessorInfo
    {
        #region constants
        private const String MERCH_NUM = "MERCH_NUM";
        private const String PROCESSOR_ID = "PROCESSOR_ID";
        private const String LICENSE_ID = "LICENSEID";
        private const String SITE_ID = "SITEID";
        private const String DEVICE_ID = "DEVICEID";
        #endregion

        private String MerchantNumber = String.Empty;
        private String ProcessorId = String.Empty;
        private string applicationName = String.Empty;
        private String LicenseID = String.Empty;
        private String SiteID = String.Empty;
        private String DeviceID = String.Empty;

        public ProcessorInfo(String procInfo)
        {
            XmlToKeys processorInfo = new XmlToKeys();
            Dictionary<String, String> keys = new Dictionary<String, String>();
            processorInfo.GetFields(procInfo, "", ref keys, false);
            String value = String.Empty;
            if (keys.TryGetValue(MERCH_NUM, out value))
                MerchantNumber = value.Trim();
            if (keys.TryGetValue(PROCESSOR_ID, out value))
                ProcessorId = value.Trim();
            if (keys.TryGetValue(LICENSE_ID, out value))
                LicenseID = value.Trim();
            if (keys.TryGetValue(SITE_ID, out value))
                SiteID = value.Trim();
            if (keys.TryGetValue(DEVICE_ID, out value))
                DeviceID = value.Trim();
            value = String.Empty;
            keys.Clear();
            keys = null;
            processorInfo = null;
        }

        public string MERCHNUM
        {
            //Property for error
            get
            {
                return MerchantNumber;
            }
        }
        public string PROCESSORID
        {
            //Property for error
            get
            {
                return ProcessorId;
            }
        }

        public string LICENSEID
        {
            get
            {
                return LicenseID;
            }
        }
        public string SITEID
        {
            get
            {
                return SiteID;
            }
        }
        public string DEVICEID
        {
            get
            {
                return DeviceID;
            }
        }
    }
}
