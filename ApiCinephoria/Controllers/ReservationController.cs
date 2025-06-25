using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ApiCinephoria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController(IReservationService reservationService) : ControllerBase
    {
        private readonly IReservationService _reservationService = reservationService;

        [HttpGet]
        public async Task<ActionResult<List<ReservationModel>>> Get(
            int? cinemaId = null,
            int? userId =null,
            int? movieId = null,
            DateTime? reservationDate = null,
            DateTime? reservationTime = null,
            int? seatId = null)
        {
            var reservations = await _reservationService.GetFilteredAsync(userId, cinemaId, movieId, reservationDate, reservationTime, seatId);


            //if (reservations == null || reservations.Count == 0)
            //{
            //    return NotFound();
            //}

            return reservations;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationModel>> Get(string id)
        {
            var reservation = await _reservationService.GetAsync(id);

            if (reservation is null)
            {
                return NotFound();
            }

            return reservation;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReservationCreateModel reservation)
        {
           
            var reservationModel = new ReservationModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = reservation.UserId,
                MovieId = reservation.MovieId,
                MovieTitle = reservation.MovieTitle,
                CinemaId = reservation.CinemaId,
                CinemaName = reservation.CinemaName,
                SeatId = reservation.SeatId,
                SeatName = reservation.SeatName,
                RoomId = reservation.RoomId,
                RoomName = reservation.RoomName,
                ReservationDate = reservation.ReservationDate,  
                ReservationTime = reservation.ReservationTime
            };

            await _reservationService.CreateAsync(reservationModel);

            return CreatedAtAction(nameof(Get), new { id = reservationModel.Id }, reservationModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ReservationModel updatedReservation)
        {
            var reservation = await _reservationService.GetAsync(id);

            if (reservation is null)
            {
                return NotFound();
            }

            updatedReservation.Id = reservation.Id;

            await _reservationService.UpdateAsync(id, updatedReservation);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))  // Vérifier si l'ID est valide
            {
                return BadRequest("ID invalide");
            }

            var reservation = await _reservationService.GetAsync(objectId.ToString());

            if (reservation is null)
            {
                return NotFound();
            }

            await _reservationService.DeleteAsync(objectId.ToString());

            return NoContent();
        }
    }
}
