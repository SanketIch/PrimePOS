﻿using System.Collections.Generic;
using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.Builders {
    public class RecurringBuilder<TResult> : TransactionBuilder<TResult> where TResult : class {
        internal string Key { get; set; }
        internal string OrderId { get; set; }
        internal IRecurringEntity Entity { get; set; }
        internal Dictionary<string, string> SearchCriteria { get; set; }

        internal RecurringBuilder<TResult> AddSearchCriteria(string key, string value) {
            SearchCriteria.Add(key, value);
            return this;
        }

        internal RecurringBuilder(TransactionType type, IRecurringEntity entity = null) : base(type) {
            SearchCriteria = new Dictionary<string, string>();
            if (entity != null) {
                Entity = entity;
                Key = entity.Key;
            }
        }

        /// <summary>
        /// Executes the builder against the gateway.
        /// </summary>
        /// <returns>TResult</returns>
        public override TResult Execute(string configName = "default") {
            base.Execute(configName);

            var client = ServicesContainer.Instance.GetRecurringClient(configName);
            return client.ProcessRecurring(this);
        }

        protected override void SetupValidations() {
            Validations.For(TransactionType.Edit | TransactionType.Delete | TransactionType.Fetch).Check(() => Key).IsNotNull();
            Validations.For(TransactionType.Search).Check(() => SearchCriteria).IsNotNull();
        }
    }
}
