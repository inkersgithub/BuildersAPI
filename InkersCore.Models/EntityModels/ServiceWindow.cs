using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.EntityModels
{
    public class ServiceWindow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("window_count")]
        public int WindowCount { get; set; }

        [Column("selected_samples")]
        public string? SelectedSamples { get; set; }
    }
}
