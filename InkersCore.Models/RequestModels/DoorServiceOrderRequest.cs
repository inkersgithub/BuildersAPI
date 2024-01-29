using InkersCore.Models.EntityModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace InkersCore.Models.RequestModels
{
    public class DoorServiceOrderRequest : OrderRequest
    {
        public int PropertyType { get; set; }
        public int Category { get; set; }
        public int Type { get; set; }
        public int Count { get; set; }
    }
}
