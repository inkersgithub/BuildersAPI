using InkersCore.Models.EntityModels;

namespace InkersCore.Models.RequestModels
{
    public class OrderRequestCommon : CommonRequest
    {
        public long CompanyId { get; set; }
        public long ServiceId { get; set; }
        public long? CustomerId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? RemarksCustomer { get; set; }

        public ServiceDoor? ServiceDoor { get; set; }
        public ServiceWindow? ServiceWindow { get; set; }
        public ServiceKitchen? ServiceKitchen { get; set; }
        public ServicePool? ServicePool { get; set; }
        public ServicePlan? ServicePlan { get; set; }
    }
}
