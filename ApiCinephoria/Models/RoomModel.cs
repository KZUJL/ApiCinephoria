using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiCinephoria.Models
{
    public class RoomModel
    {
        [Key]
        public int RoomId { get; set; }
        public int CinemaId { get; set; }
        public string Name { get; set; }
        public string Quality { get; set; }
        public int SeatsNumber { get; set; }

        
        [ForeignKey("CinemaId")]
        public CinemaModel Cinema { get; set; }

        public ICollection<SeatsModel> Seats { get; set; }      

    }
}
