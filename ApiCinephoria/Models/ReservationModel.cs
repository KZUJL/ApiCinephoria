using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ApiCinephoria.Models
{
    public class ReservationModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // MongoDB gère automatiquement cet ID

        [BsonElement("UserId")]
        public int UserId { get; set; }

        [BsonElement("MovieId")]
        public int MovieId { get; set; }

        [BsonElement("MovieTitle")]
        public string MovieTitle { get; set; }

        [BsonElement("CinemaId")]
        public int CinemaId { get; set; }

        [BsonElement("CinemaName")]
        public string CinemaName { get; set; }

        [BsonElement("SeatId")]
        public int SeatId { get; set; }

        [BsonElement("SeatName")]
        public string SeatName { get; set; }

        [BsonElement("RoomId")]
        public int RoomId { get; set; }

        [BsonElement("RoomName")]
        public string RoomName { get; set; }

        [BsonElement("ReservationDate")]
        public DateTime ReservationDate { get; set; }

        [BsonElement("ReservationTime")]
        public DateTime ReservationTime { get; set; }
    }
}
