namespace Vantiv.RequestModels
{
    public class ReversalRequest
    {
        public string laneId { get; set; }
        public string transactionAmount { get; set; }
        public string cardHolderPresentCode { get; set; }
        public string clerkNumber { get; set; }
        public Configuration configuration { get; set; }
        public string convenienceFeeAmount { get; set; }
        public string ebtType { get; set; }
        public string referenceNumber { get; set; }
        public string shiftId { get; set; }
        public StoreCard storeCard { get; set; }
        public string ticketNumber { get; set; }
        public string type { get; set; }
    }    
}
