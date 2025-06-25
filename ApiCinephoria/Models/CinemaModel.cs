using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCinephoria.Models
{
    public class CinemaModel
    {
        [Key]
        public int CinemaId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }       
        public string City { get; set; }
        public string? PhoneNumber { get; set; }

        public List<CinemaScheduleModel> Schedules { get; set; }
      
    }
}
