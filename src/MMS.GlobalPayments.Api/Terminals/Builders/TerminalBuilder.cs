﻿using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Terminals.Abstractions;

namespace MMS.GlobalPayments.Api.Terminals.Builders {
    public abstract class TerminalBuilder<T> : TransactionBuilder<ITerminalResponse> where T : TerminalBuilder<T> {
        internal PaymentMethodType PaymentMethodType { get; set; }
        internal int ReferenceNumber { get; set; }
        internal int EcrId { get; set; }

        public T WithPaymentMethodType(PaymentMethodType value) {
            PaymentMethodType = value;
            return this as T;
        }
        public T WithReferenceNumber(int value) {
            ReferenceNumber = value;
            return this as T;
        }
        public T WithRequestId(int value) {
            ReferenceNumber = value;
            return this as T;
        }

        internal TerminalBuilder(TransactionType type, PaymentMethodType paymentType) : base(type) {
            PaymentMethodType = paymentType;
        }

        public abstract byte[] Serialize(string configName = "default");
    }
}
