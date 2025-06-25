namespace ApiCinephoria.Models
{
    public class ReservationCreateModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public int CinemaId { get; set; }
        public string CinemaName { get; set; }
        public int SeatId { get; set; }
        public string SeatName { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ReservationTime { get; set; }
    }
}
