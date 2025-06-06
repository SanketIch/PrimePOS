﻿using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Gateways;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.Services {
    public class GpApiService {
        public static AccessTokenInfo GenerateTransactionKey(GpApiConfig gpApiConfig) {
            GpApiConnector connector = new GpApiConnector(gpApiConfig);

            if (string.IsNullOrEmpty(connector.ServiceUrl))
            {
                if (gpApiConfig.Environment.Equals(Entities.Environment.TEST))
                    connector.ServiceUrl = ServiceEndpoints.GP_API_TEST;
                else
                    connector.ServiceUrl = ServiceEndpoints.GP_API_PRODUCTION;
            }

            var data = connector.GetAccessToken();

            return new AccessTokenInfo {
                Token = data.Token,
                DataAccountName = data.DataAccountName,
                DisputeManagementAccountName = data.DisputeManagementAccountName,
                TokenizationAccountName = data.TokenizationAccountName,
                TransactionProcessingAccountName = data.TransactionProcessingAccountName,
            };
        }
    }
}
