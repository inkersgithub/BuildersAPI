using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkersCore.Models.EntityModels
{
    [Table("subscription_plan")]
    public class SubscriptionPlan : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(50, ErrorMessage = "Name cannot be greater than 50 characters")]
        public string Name { get; set; }

        [Column("days")]
        public long Days { get; set; }

        [Column("amount")]
        public float Amount { get; set; }
    }
}
