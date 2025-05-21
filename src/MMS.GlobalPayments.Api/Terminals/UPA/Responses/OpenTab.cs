using MMS.GlobalPayments.Api.Entities;

namespace MMS.GlobalPayments.Api.Terminals.UPA

{
    public class OpenTab {
        public decimal AuthorizedAmount { get; set; }
        public string CardType { get; set; }
        public string MaskedPan { get; set; }
        public string TransactionId { get; set; }
    }
}
