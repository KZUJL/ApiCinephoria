using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCinephoria.Data;

namespace ApiCinephoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController(CinephoriaContext context) : ControllerBase
    {
        private readonly CinephoriaContext _context = context;


        [HttpGet]
        public async Task<ActionResult<List<SeatsModel>>> GetSeats(int? locationId = null)
        {
            var query = _context.Locations             
                .AsQueryable();

            if (locationId.HasValue)
            {
                query = query.Where(l => l.LocationId == locationId.Value);
            }

            var seats = await query.ToListAsync();
            return Ok(seats);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<SeatsModel>>> GetSeatsByRoomId(int id)
        {
            var seats = await _context.Locations
                .Where(mt => mt.RoomId == id)                
                .ToListAsync();

            if (seats == null || seats.Count == 0)
            {
                return NotFound();
            }

            return Ok(seats);
        }
        [HttpPost]
        public async Task<ActionResult<SeatsModel>> CreateSeat([FromBody] SeatsModel seat)
        {
            if (seat == null)
                return BadRequest();

            _context.Locations.Add(seat);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSeats), new { locationId = seat.LocationId }, seat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(int id, [FromBody] SeatsModel updatedSeat)
        {
            if (updatedSeat == null || id != updatedSeat.LocationId)
                return BadRequest();

            var existingSeat = await _context.Locations.FindAsync(id);
            if (existingSeat == null)
                return NotFound();

            existingSeat.Name = updatedSeat.Name;
            existingSeat.Type = updatedSeat.Type;
            existingSeat.RowLocation = updatedSeat.RowLocation;
            existingSeat.ColumnLocation = updatedSeat.ColumnLocation;
            existingSeat.RoomId = updatedSeat.RoomId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var seat = await _context.Locations.FindAsync(id);
            if (seat == null)
                return NotFound();

            _context.Locations.Remove(seat);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
