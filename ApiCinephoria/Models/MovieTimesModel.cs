using System.ComponentModel.DataAnnotations;

namespace ApiCinephoria.Models
{
    public class MovieTimesModel
    {
        [Key]
        public int MovieTimesId { get; set; }
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        public int RoomId { get; set; }
        public DateTime day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal Price { get; set; }
        public MovieModel Movie { get; set; }
        public CinemaModel Cinema { get; set; }
        public RoomModel Room { get; set; }
    }
}
