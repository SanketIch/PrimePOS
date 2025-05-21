﻿using MMS.GlobalPayments.Api.Builders;
using MMS.GlobalPayments.Api.Entities;
using MMS.GlobalPayments.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.GlobalPayments.Api.PaymentMethods
{
    public class BankPayment : IPaymentMethod
    {
        /// <summary>
        /// Merchant/Individual Name.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        ///  Financial institution account number.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// A  SORT   Code   is a number code, which is used by British and Irish banks.
        /// These codes have six digits, and they are divided into three different pairs, such as 12-34-56.
        /// </summary>
        public string SortCode { get; set; }

        /// <summary>
        /// The International Bank Account Number
        /// </summary>
        public string Iban { get; set; }

        public PaymentMethodType PaymentMethodType { get { return PaymentMethodType.BankPayment; } }
        
        public string ReturnUrl { get; set; }
    
        public string StatusUpdateUrl { get; set; }
       
        public BankPaymentType? BankPaymentType { get; set; }

        /// <summary>
        /// This is a mandatory request used to initiate an Open Banking transaction,
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public BankPaymentBuilder Charge(decimal? amount) {
            return (new BankPaymentBuilder(TransactionType.Sale, this))
                .WithModifier(TransactionModifier.BankPayment)
                .WithAmount(amount);
        }

    }
}
