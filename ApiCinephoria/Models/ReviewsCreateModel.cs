namespace ApiCinephoria.Models
{
    public class ReviewsCreateModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int MovieId { get; set; }      
        public string MovieTitle { get; set; }
        public int Reviews { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewsDate { get; set; }
        
    }
}
