using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Network.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Services {
    public class AdminService {
        public AdminService(NetworkGatewayConfig config, string configName = "default") {
            ServicesContainer.ConfigureService(config, configName);
        }

        public AuthorizationBuilder POSSiteConfig(RecordDataEntry configData) {
            return new AuthorizationBuilder(TransactionType.SiteConfig).WithPOSSiteConfigRecord(configData);            
        }

        public AuthorizationBuilder TimeRequest() {
            return new AuthorizationBuilder(TransactionType.TimeRequest);
        }
    }
}
