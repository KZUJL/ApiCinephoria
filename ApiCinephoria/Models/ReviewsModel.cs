using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ApiCinephoria.Models
{
    public class ReviewsModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  

        [BsonElement("UserId")]
        public int UserId { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; }

        [BsonElement("MovieId")]
        public int MovieId { get; set; }

        [BsonElement("MovieTitle")]
        public string MovieTitle { get; set; }

        [BsonElement("reviews")]
        public int Reviews { get; set; }

        [BsonElement("Comments")]
        public string Comments { get; set; }

        [BsonElement("ReviewsDate")]
        public DateTime ReviewsDate { get; set; }

        [BsonElement("ReviewsValidation")]
        public bool ReviewsValidation { get; set; } = false;
    }
}
