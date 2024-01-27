using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.EntityModels
{

    [Table("subscription_transactions")]
    public class SubscriptionTransaction : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [ForeignKey("company_id")]
        public Company Company { get; set; }

        [ForeignKey("subscription_id")]
        public SubscriptionPlan SubscriptionPlan { get; set; }

        [ForeignKey("start_date")]
        public DateTime StartDate { get; set; }

        [ForeignKey("end_date")]
        public DateTime EndDate { get; set; }

        [Column("transaction_status")]
        public SubscriptionTransactionStatus TransactionStatus { get; set; }

        [Column("reference")]
        [MaxLength(200, ErrorMessage = "Maximum 200 characters")]
        public string Reference { get; set; }

    }

    public enum SubscriptionTransactionStatus
    {
        Started = 1,
        Processing = 2,
        Pending = 3,
        Failed = 4,
        Success = 5
    }
}
