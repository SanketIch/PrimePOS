﻿using MMS.GlobalPayments.Api.Terminals;

namespace MMS.GlobalPayments.Api.Services {
    public class DeviceService {
        public static IDeviceInterface Create(ConnectionConfig config, string configName = "default") {
            ServicesContainer.ConfigureService(config, configName);
            return ServicesContainer.Instance.GetDeviceInterface(configName);
        }
    }
}
