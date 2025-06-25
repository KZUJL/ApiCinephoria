namespace ApiCinephoria.Models
{
    public class RoomDto
    {
        public int RoomId { get; set; }
        public int CinemaId { get; set; }
        public string Name { get; set; }
        public string Quality { get; set; }
        public int SeatsNumber { get; set; }
    }
}
