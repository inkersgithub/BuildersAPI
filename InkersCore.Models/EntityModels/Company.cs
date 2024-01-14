using InkersCore.Models.AuthEntityModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkersCore.Models.EntityModels
{
    [Table("company")]
    public class Company : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(80, ErrorMessage = "Name cannot be greater than 80 characters")]
        public string Name { get; set; }

        [Column("phone")]
        [MaxLength(14, ErrorMessage = "Phone cannot be greater than 14 characters")]
        public string Phone { get; set; }

        [Column("email")]
        [MaxLength(80, ErrorMessage = "Email cannot be greater than 80 characters")]
        public string Email { get; set; }

        [Column("address1")]
        [MaxLength(50, ErrorMessage = "Address1 cannot be greater than 50 characters")]
        public string Address1 { get; set; }

        [Column("address2")]
        [MaxLength(50, ErrorMessage = "Address2 cannot be greater than 50 characters")]
        public string Address2 { get; set; }

        [Column("address3")]
        [MaxLength(50, ErrorMessage = "Address3 cannot be greater than 50 characters")]
        public string Address3 { get; set; }

        [Column("zipcode")]
        [MaxLength(8, ErrorMessage = "Zipcode cannot be greater than 8 characters")]
        public string ZipCode { get; set; }

        [ForeignKey("useraccount_id")]
        public UserAccount? UserAccount { get; set; }

        [Column("is_approved")]
        public int IsApproved { get; set; }

        [NotMapped]
        public long[] RequestedServiceIds { get; set; }

        public Company()
        {
            IsApproved = 0;
        }
    }
}
