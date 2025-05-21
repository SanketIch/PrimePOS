using MMS.GlobalPayments.Api.Gateways;
using System;

namespace MMS.GlobalPayments.Api {
    public class BoardingConfig : Configuration {
        public string Portal { get; set; }

        internal override void ConfigureContainer(ConfiguredServices services) {
            services.BoardingConnector = new OnlineBoardingConnector {
                Portal = Portal
            };
        }
    }
}
