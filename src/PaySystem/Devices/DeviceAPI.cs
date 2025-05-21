/*
 * Author: Manoj Kumar
 * Description: Implementation of Ingenico isc series.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace EDevice
{
    /// <summary>
    /// Instantiate the class base on device name
    /// </summary>
    public class DeviceAPI
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private IEDevice DEVICE = null;
        CommSettings.DeviceName Device_Name = new CommSettings.DeviceName();
        public DeviceAPI(CommSettings.DeviceName deviceName)
        {
            logger.Trace("In DeviceAPI()");
            this.Device_Name = deviceName; 
        }

        /// <summary>
        /// Interface to the Device driver
        /// </summary>
        /// <returns></returns>
        public IEDevice ISCDEVICE(FormProperties.FormSettings fs)
        {
            logger.Trace("In IEDevice ISCDEVICE(FormProperties.FormSettings fs)");
            if (DEVICE == null)
            {
                if (this.Device_Name == CommSettings.DeviceName.Ingenico_isc480 || this.Device_Name == CommSettings.DeviceName.Ingenico_isc250)
                {
                    DEVICE = (IEDevice)new ISCDevices(fs);
                }
            }
            return DEVICE;
        }
     
    }
}
