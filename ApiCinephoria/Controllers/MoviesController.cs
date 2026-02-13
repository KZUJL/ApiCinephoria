using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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

            var movies = await _context.Movies.ToListAsync();

            // date la plus récente en base
            DateTime maxDate = movies.Max(m => m.AvailableDate);

            // nombre de semaines de décalage
            int weeksOffset = (today - maxDate).Days / 7;

            DateTime startDate = today.AddDays(-7);
            DateTime endDate = today.AddDays(7);

            var result = movies
                .Select(m => new MovieModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Poster = m.Poster,
                    AvailableDate = m.AvailableDate.AddDays(weeksOffset * 7)
                })
                .Where(m => m.AvailableDate >= startDate &&
                            m.AvailableDate <= endDate)
                .OrderBy(m => m.AvailableDate)
                .ToList();

            return result;
        }


        [HttpGet("soon-available")]
        public async Task<ActionResult<IEnumerable<MovieModel>>> GetMoviesAvailableAfterToday()
        {
            DateTime today = DateTime.Today;

            var movies = await _context.Movies.ToListAsync();

            DateTime maxDate = movies.Max(m => m.AvailableDate);
            int weeksOffset = (today - maxDate).Days / 7;

            DateTime startDate = today.AddDays(7);
            DateTime endDate = today.AddDays(28);

            var result = movies
                .Select(m => new MovieModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Poster = m.Poster,
                    AvailableDate = m.AvailableDate.AddDays(weeksOffset * 7)
                })
                .Where(m => m.AvailableDate > startDate &&
                            m.AvailableDate <= endDate)
                .OrderBy(m => m.AvailableDate)
                .ToList();

            return result;
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
