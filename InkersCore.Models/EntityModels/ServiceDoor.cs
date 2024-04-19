using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkersCore.Models.EntityModels
{
    [Table("service_door")]
    public class ServiceDoor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("property_type")]
        public int PropertyType { get; set; }

        [Column("category")]
        public int Category { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("count")]
        public int Count { get; set; }

        [Column("selected_samples")]
        public string? SelectedSamples { get; set; }
    }
}
