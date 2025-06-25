using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ApiCinephoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController(CinephoriaContext context) : ControllerBase
    {
        private readonly CinephoriaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieModel>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<MovieModel>> CreateMovie(MovieModel movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovies), new { id = movie.MovieId }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieModel movie)
        {
            if (id != movie.MovieId)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<MovieModel>>> GetMoviesByAvailableDate()
        {
            DateTime today = DateTime.Today;
            int daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)today.DayOfWeek + 7) % 7;
            DateTime nextWednesday = today.AddDays(daysUntilWednesday);
            DateTime previousWednesday = nextWednesday.AddDays(-7);

            return await _context.Movies
                .Where(m => m.AvailableDate >= previousWednesday && m.AvailableDate <= nextWednesday)
                .ToListAsync();
        }

        [HttpGet("soon-available")]
        public async Task<ActionResult<IEnumerable<MovieModel>>> GetMoviesAvailableAfterToday()
        {
            DateTime today = DateTime.Today;

            return await _context.Movies
                .Where(m => m.AvailableDate > today)
                .ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieModel>> GetMovieById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();  
            }

            return movie; 
        }


        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
