using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.EntityModels
{
    public class ServicePool
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }


        [Column("type")]
        public int Type { get; set; }

        [Column("kid_pool_option")]
        public int KidPool { get; set; }

        [Column("jacuzzi_option")]
        public int Jacuzzi { get; set; }

        [Column("heater_option")]
        public int Heater { get; set; }

        [Column("light_option")]
        public int Light { get; set; }

        [Column("waterfall_option")]
        public int WaterFall { get; set; }

        [Column("mosaic_option")]
        public int Mosaic { get; set; }

        [Column("selected_samples")]
        public string? SelectedSamples { get; set; }
    }
}
