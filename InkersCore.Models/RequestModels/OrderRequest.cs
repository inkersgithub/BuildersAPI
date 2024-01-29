namespace InkersCore.Models.RequestModels
{
    public class OrderRequest : CommonRequest
    {
        public long CompanyId { get; set; }
        public long ServiceId { get; set; }
        public long? CustomerId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? RemarksCustomer { get; set; }
    }
}
