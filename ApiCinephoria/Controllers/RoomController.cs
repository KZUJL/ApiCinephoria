using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCinephoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController(CinephoriaContext context) : ControllerBase
    {
        private readonly CinephoriaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<RoomModel>>> GetRooms( int? roomId = null, int? cinemaId = null)
        {
            var query = _context.Rooms
                .Include(r => r.Seats)
                .Include(r => r.Cinema)
                .AsQueryable();

            if (roomId.HasValue)
            {
                query = query.Where(r => r.RoomId == roomId.Value);
            }
            if (cinemaId.HasValue)
            {
                query = query.Where(r => r.CinemaId == cinemaId.Value);
            }

            var roomsWithSeats = await query.ToListAsync();

            return Ok(roomsWithSeats);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            // On récupère la room
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound();

            // Vérifie si la salle est encore utilisée dans des séances
            bool hasMovieTimes = await _context.MovieTimes.AnyAsync(mt => mt.RoomId == id);
            if (hasMovieTimes)
                return BadRequest("Impossible de supprimer la salle : elle est encore utilisée par des projections.");

            // On récupère les sièges liés à cette room
            var seats = await _context.Locations
                .Where(s => s.RoomId == id)
                .ToListAsync();

            // On les supprime
            _context.Locations.RemoveRange(seats);

            // On supprime la room
            _context.Rooms.Remove(room);

            // On sauvegarde les changements
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<RoomModel>> CreateRoom([FromBody] RoomDto dto)
        {
            if (dto == null)
                return BadRequest();

            var room = new RoomModel
            {
                // RoomId n'est pas affecté ici, il sera généré par la base
                Name = dto.Name,
                Quality = dto.Quality,
                SeatsNumber = dto.SeatsNumber,
                CinemaId = dto.CinemaId
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            // Retourne la ressource créée avec son URI
            return CreatedAtAction(nameof(GetRooms), new { roomId = room.RoomId }, room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, [FromBody] RoomDto dto)
        {
            if (id != dto.RoomId)
            {
                return BadRequest("Room ID mismatch.");
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            // Mise à jour des propriétés
            room.Name = dto.Name;
            room.Quality = dto.Quality;
            room.SeatsNumber = dto.SeatsNumber;
            room.CinemaId = dto.CinemaId;

            await _context.SaveChangesAsync();

            return NoContent();
        }




    }
}
