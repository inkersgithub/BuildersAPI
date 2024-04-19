using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InkersCore.Models.EntityModels
{
    public class ServicePlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("budget")]
        public long Budget { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("occupants")]
        public int Occupants { get; set; }

        [Column("levels")]
        public int Levels { get; set; }

        [Column("basement")]
        public int Basement { get; set; }

        [Column("master_room_count")]
        public int MasterRoomCount { get; set; }

        [Column("room_count")]
        public int RoomCount { get; set; }

        [Column("bedroom_level")]
        public int BedroomLevel { get; set; }

        [Column("living_room_count")]
        public int LivingRoomCount { get; set; }

        [Column("house_style")]
        public int HouseStyle { get; set; }

        [Column("ac_type")]
        public int AcType { get; set; }

        [Column("pool")]
        public int Pool { get; set; }

        [Column("kitchen")]
        public int Kitchen { get; set; }

        [Column("pantry")]
        public int Pantry { get; set; }

        [Column("garage")]
        public int Garage { get; set; }

        [Column("selected_samples")]
        public string? SelectedSamples { get; set; }
    }
}
