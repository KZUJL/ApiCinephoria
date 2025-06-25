using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCinephoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CinemaController(CinephoriaContext context) : ControllerBase
    {
        private readonly CinephoriaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CinemaModel>>> GetCinemas()
        {
            var cinemas = await _context.Cinemas
                .Include(c => c.Schedules) 
                .ToListAsync();

            return Ok(cinemas);
        }

    }
}
