using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkersCore.Models.EntityModels
{
    [Table("order")]
    public class Order : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("contact_name")]
        [MaxLength(80, ErrorMessage = "Name cannot be greater than 80 characters")]
        public string ContactName { get; set; }

        [Column("contact_phone")]
        [MaxLength(12, ErrorMessage = "Phone cannot be greater than 12 characters")]
        public string ContactPhone { get; set; }

        [ForeignKey("compnay_id")] 
        public Company Company { get; set; }

        [ForeignKey("customer_id")]
        public Customer Customer { get; set; }

        [Column("order_status")]
        public OrderStatus OrderStatus { get; set; }

        [Column("address")]
        [MaxLength(255, ErrorMessage = "Address cannot be greater than 255 characters")]
        public string Address { get; set; }

        [ForeignKey("service_id")]
        public Service Service { get; set; }

        #region ServiceMappings

        [ForeignKey("service_door_id")]
        public ServiceDoor? ServiceDoor { get; set; }

        #endregion

        [Column("remarks_customer")]
        [MaxLength(255, ErrorMessage = "Remarks cannot be greater than 255 characters")]
        public string? RemarksCustomer { get; set; }

        [Column("remarks_company")]
        [MaxLength(255, ErrorMessage = "Remarks cannot be greater than 255 characters")]
        public string? RemarksCompnay { get; set; }

        [Column("remarks_admin")]
        [MaxLength(255, ErrorMessage = "Remarks cannot be greater than 255 characters")]
        public string? RemarksAdmin { get; set; }

        [Column("amount")]
        public float Amount { get; set; }

        [Column("estimated_days")]
        public int? EstimatedDays { get; set; }

        [Column("company_contact_person_name")]
        [MaxLength(80, ErrorMessage = "Contact Person Name cannot be greater than 80 characters")]
        public string? CompanyContactPersonName { get; set; }

        [Column("company_contact_person_phone")]
        [MaxLength(14, ErrorMessage = "Contact Person Phone cannot be greater than 14 characters")]
        public string? CompanyContactPersonPhone { get; set; }
    }

    public enum OrderStatus
    {
        Requested = 1,
        CompanyApproved = 2,
        CompanyDeclined = 3,
        AdminApproved = 4,
        AdminDeclined = 5,
        WorkStarted = 6,
        WorkHold = 7,
        WorkRejected = 8,
        WorkCompleted = 9,
    }
}
