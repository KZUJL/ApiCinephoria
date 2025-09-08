using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCinephoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController :ControllerBase
    {
        private readonly CinephoriaContext _context;
        public IncidentController(CinephoriaContext context)
        {
            _context = context;            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncidentModel>>> GetIncident()
        {
            var incidents = await _context.Incidents.ToListAsync();
            return Ok(incidents);
        }
        [HttpGet("with-details")]
        public async Task<ActionResult<IEnumerable<object>>> GetIncidentWithDetails([FromQuery] int? locationId = null)
        {
            var query = from i in _context.Incidents
                        join c in _context.Cinemas on i.CinemaId equals c.CinemaId
                        join r in _context.Rooms on i.RoomId equals r.RoomId
                        join l in _context.Locations on i.LocationId equals l.LocationId
                        select new
                        {
                            i.IncidentId,
                            i.Date,
                            i.Description,
                            CinemaName = c.Name,
                            RoomName = r.Name,
                            LocationName = l.Name,
                            i.LocationId
                        };

            if (locationId.HasValue)
            {
                query = query.Where(x => x.LocationId == locationId.Value);
            }

            var incidents = await query.ToListAsync();

            return Ok(incidents);
        }

        [HttpPost]
        public async Task<ActionResult<IncidentModel>> PostIncident(IncidentModel incident)
        {
            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncident), new { id = incident.IncidentId }, incident);
        }

        // PUT : api/Incident/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncident(int id, IncidentModel incident)
        {
            if (id != incident.IncidentId)
                return BadRequest("L'ID fourni ne correspond pas à l'incident.");

            _context.Entry(incident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Incidents.Any(e => e.IncidentId == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE : api/Incident/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
                return NotFound();

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
