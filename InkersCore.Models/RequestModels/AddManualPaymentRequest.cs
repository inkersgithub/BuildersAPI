namespace InkersCore.Models.RequestModels
{
    public class AddManualPaymentRequest: CommonRequest
    {
        public string Phone { get; set; }
        public long SubscriptionPlanId { get; set; }
        public string Reference { get; set; }
    }
}
