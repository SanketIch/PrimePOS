namespace Vantiv.RequestModels
{
    public class StrictReturn
    {
        public string laneId { get; set; }
        public string transactionAmount { get; set; }
        public string clerkNumber { get; set; }
        public Configuration configuration { get; set; }
        public string convenienceFeeAmount { get; set; }
        public string ebtType { get; set; }
        public string referenceNumber { get; set; }
        public string shiftId { get; set; }
        public string ticketNumber { get; set; }
    }    
}
