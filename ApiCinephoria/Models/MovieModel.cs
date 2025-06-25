using System.ComponentModel.DataAnnotations;

namespace ApiCinephoria.Models
{
    public class MovieModel
    {
        [Key]
        public int MovieId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        public string Cast { get; set; }    
        public string SourcePoster { get; set; }
        public string SourceTrailer { get; set; }
        public DateTime AvailableDate { get; set; }
        public string MinimumAge { get; set; }
        public bool Isfavorite { get; set; }
    }
}
