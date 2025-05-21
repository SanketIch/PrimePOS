using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Gateways;
using System;

namespace MMS.GlobalPayments.Api.Builders {
    public class ResubmitBuilder : TransactionBuilder<Transaction> {
        public string TransactionToken { get; set; }        
        public ResubmitBuilder WithTransactionToken(string token) {
            TransactionToken = token;
            return this;
        }
        public ResubmitBuilder(TransactionType type) : base(type, null) {
            
        }
        public override Transaction Execute(string configName) {
            base.Execute(configName);

            IPaymentGateway client = ServicesContainer.Instance.GetClient(configName);
            if(client is NWSConnector) {
                return ((NWSConnector)client).ResubmitTransaction(this);
            }
            else {
                throw new UnsupportedTransactionException("Resubmissions are not allowed for the currently configured gateway.");
            }
        }

        public new void SetupValidations() {

        }
    }
}
