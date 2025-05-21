
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Entities.Billing;
using MMS.GlobalPayments.Api.Entities.Enums;
using MMS.GlobalPayments.Api.PaymentMethods;
using System.Collections.Generic;

namespace MMS.GlobalPayments.Api.Builders {
    public class BillingBuilder : TransactionBuilder<BillingResponse> {
        internal BillingBuilder(TransactionType transactionType) : base (transactionType) {
            BillingLoadType = BillingLoadType.None;
        }

        internal IEnumerable<Bill> Bills { get; set; }
        internal BillingLoadType BillingLoadType { get; set; }
        internal HostedPaymentData HostedPaymentData { get; set; }
        internal string OrderID { get; set; }
        internal bool CommitBills { get; private set; }
        internal bool ClearBills { get; private set; }
        internal Customer Customer { get; set; }
        internal decimal? Amount { get; set; }

        public BillingBuilder WithOrderID(string orderID) {
            OrderID = orderID;
            return this;
        }

        internal BillingBuilder CommitPreloadedBills() {
            CommitBills = true;
            return this;
        }

        internal BillingBuilder ClearPreloadedBills() {
            ClearBills = true;
            return this;
        }

        public BillingBuilder WithBillingLoadType(BillingLoadType billingLoadType) {
            BillingLoadType = billingLoadType;
            return this;
        }

        /// <summary>
        /// Sets the bills used in the action
        /// </summary>
        /// <param name="bills"></param>
        /// <returns></returns>
        public BillingBuilder WithBills(IEnumerable<Bill> bills) {
            Bills = bills;
            return this;
        }

        /// <summary>
        /// Sets the hosted payment data used in the action
        /// </summary>
        /// <param name="bills"></param>
        /// <returns></returns>
        public BillingBuilder WithHostedPaymentData(HostedPaymentData value) {
            HostedPaymentData = value;
            return this;
        }

        /// <summary>
        /// Sets the request's payment method.
        /// </summary>
        /// <param name="value">The payment method</param>
        /// <returns>AuthorizationBuilder</returns>
        public BillingBuilder WithPaymentMethod(IPaymentMethod value) {
            PaymentMethod = value;
            if (value is EBTCardData && ((EBTCardData)value).SerialNumber != null) {
                TransactionModifier = TransactionModifier.Voucher;
            }

            if (value is CreditCardData && ((CreditCardData)value).MobileType != null) {
                TransactionModifier = TransactionModifier.EncryptedMobile;
            }

            return this;
        }

        /// <summary>
        /// Sets the customer, where applicable
        /// </summary>
        /// <param name="value">The transaction's customer</param>
        /// <returns>AuthorizationBuilder</returns>
        public BillingBuilder WithCustomer(Customer value) {
            Customer = value;
            return this;
        }

        /// <summary>
        /// Sets the transaction's amount
        /// </summary>
        /// <param name="value">The amount</param>
        /// <returns>AuthorizationBuilder</returns>
        public BillingBuilder WithAmount(decimal? value) {
            Amount = value;
            return this;
        }

        /// <summary>
        /// Executes the builder against the gateway.
        /// </summary>
        /// <returns>TResult</returns>
        public override BillingResponse Execute(string configName = "default") {
            base.Execute(configName);

            var client = ServicesContainer.Instance.GetBillingClient(configName);
            return client.ProcessBillingRequest(this);
        }
    }
}
