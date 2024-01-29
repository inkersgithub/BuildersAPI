namespace InkersCore.Models.RequestModels
{
    public class OrderListFilterReqest : CommonRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? Status { get; set; }
        public bool IsAdmin { get; set; }
        public long?  CompanyId { get; set; }
        public long? CustomerId { get; set; }
    }
}
