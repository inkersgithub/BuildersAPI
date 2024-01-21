namespace InkersCore.Models.RequestModels
{
    public class AddUpdateCustomerRequest
    {
        public long? CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public long? UpdatedById { get; set; }
    }
}
