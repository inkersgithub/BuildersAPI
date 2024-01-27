namespace InkersCore.Models.RequestModels
{
    public class SubscriptionFetchRequest: CommonRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
}
