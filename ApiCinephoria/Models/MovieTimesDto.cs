namespace ApiCinephoria.Models
{
    public class MovieTimesDto
    {
        public int MovieTimesId { get; set; }
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        public int RoomId { get; set; }
        public DateTime day { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal Price { get; set; }
    }
}
