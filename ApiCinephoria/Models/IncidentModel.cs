using System.ComponentModel.DataAnnotations;

namespace ApiCinephoria.Models
{
    public class IncidentModel
    {
        [Key]
        public int IncidentId { get; set; }
        public int CinemaId { get; set; }
        public int RoomId { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
