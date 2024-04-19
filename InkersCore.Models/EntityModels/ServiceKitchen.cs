using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.EntityModels
{
    public class ServiceKitchen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("layout")]
        public int Layout { get; set; }

        [Column("shape")]
        public int Shape { get; set; }

        [Column("mode")]
        public int Mode { get; set; }

        [Column("selected_samples")]
        public string? SelectedSamples { get; set; }
    }
}
