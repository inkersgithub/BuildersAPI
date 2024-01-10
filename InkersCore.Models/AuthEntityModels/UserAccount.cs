using InkersCore.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkersCore.Models.AuthEntityModels
{
    [Table("user_account")]
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [MaxLength(50, ErrorMessage = "Name cannot be greater than 50 characters")]
        public string Name { get; set; }

        [Column("phone")]
        [MaxLength(14, ErrorMessage = "Name cannot be greater than 14 characters")]
        public string Phone { get; set; }

        [Column("email")]
        [MaxLength(80, ErrorMessage = "Name cannot be greater than 80 characters")]
        public string Email { get; set; }

        [Column("password")]
        [MaxLength(256, ErrorMessage = "Name cannot be greater than 256 characters")]
        public string Password { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Timestamp]
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [ForeignKey("created_by")]
        public UserAccount? CreatedBy { get; set; }

        [Timestamp]
        [Column("last_updated_time")]
        public DateTime LastUpdatedTime { get; set; }

        [ForeignKey("last_updated_by")]
        public UserAccount? LastUpdatedBy { get; set; }

        public UserAccount()
        {
            IsDeleted = false;
            IsActive = true;
            CreatedTime = DateTimeHandler.GetDateTime();
            LastUpdatedTime = DateTimeHandler.GetDateTime();
        }
    }
}
