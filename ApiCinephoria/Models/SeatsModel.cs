using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCinephoria.Models
{
    public class SeatsModel
    {
        [Key]
        public int LocationId { get; set; }
        public int RoomId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int RowLocation { get; set; }
        public int ColumnLocation { get; set; }

        [JsonIgnore]
        public RoomModel? Room { get; set; }
    }
}
