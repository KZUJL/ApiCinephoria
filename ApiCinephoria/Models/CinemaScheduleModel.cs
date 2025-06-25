using System.ComponentModel.DataAnnotations;

namespace ApiCinephoria.Models
{
    public class CinemaScheduleModel
    {
        [Key]
        public int ScheduleId { get; set; }
        public int CinemaId { get; set; }
        public string Jour { get; set; }
        public TimeSpan? Heure_ouverture { get; set; }
        public TimeSpan? Heure_fermeture { get; set; }
    }
}
