using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCinephoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieTimesController(CinephoriaContext context) : ControllerBase
    {
        private readonly CinephoriaContext _context = context;

        // GET: api/MovieTimes
        [HttpGet]
        public async Task<ActionResult<List<MovieTimesModel>>> GetMovieTimes(
            int? cinemaId = null,
            int? movieId = null,
            int? roomId = null)
        {
            var query = _context.MovieTimes
                .Include(mt => mt.Movie)
                .Include(mt => mt.Cinema)
                .Include(mt => mt.Room)
                .AsQueryable();

            if (cinemaId.HasValue)
            {
                query = query.Where(mt => mt.CinemaId == cinemaId.Value);
            }

            if (movieId.HasValue)
            {
                query = query.Where(mt => mt.MovieId == movieId.Value);
            }

            if (roomId.HasValue)
            {
                query = query.Where(mt => mt.RoomId == roomId.Value);
            }

            var movieTimes = await query.ToListAsync();
            return Ok(movieTimes);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MovieTimesModel>>> GetSeancesByMovieId(int id)
        {
            var movieTimes = await _context.MovieTimes
                .Where(mt => mt.MovieId == id)
                .Include(mt => mt.Movie)
                .Include(mt => mt.Cinema)
                .Include(mt => mt.Room)
                .ToListAsync();

            if (movieTimes == null || movieTimes.Count == 0)
            {
                return NotFound();
            }

            return Ok(movieTimes);
        }
        [HttpGet("ByMovieTimes/{id}")]
        public async Task<ActionResult<IEnumerable<MovieTimesModel>>> GetSeancesByMovieTimesId(int id)
        {
            var movieTimes = await _context.MovieTimes
                .Where(mt => mt.MovieTimesId == id)
                .Include(mt => mt.Movie)
                .Include(mt => mt.Cinema)
                .Include(mt => mt.Room)
                .FirstOrDefaultAsync();

            if (movieTimes == null )
            {
                return NotFound();
            }

            return Ok(movieTimes);
        }

        [HttpPut("{movieTimesId}")]
        public async Task<IActionResult> UpdateMovieTime(int movieTimesId, [FromBody] MovieTimesDto updatedMovieTime)
        {
            if (updatedMovieTime == null || movieTimesId != updatedMovieTime.MovieTimesId)
                return BadRequest("MovieTimesId mismatch.");

            var movieTime = await _context.MovieTimes.FindAsync(movieTimesId);
            if (movieTime == null)
                return NotFound();

            // Mise à jour des propriétés
            movieTime.MovieId = updatedMovieTime.MovieId;
            movieTime.CinemaId = updatedMovieTime.CinemaId;
            movieTime.RoomId = updatedMovieTime.RoomId;
            movieTime.day = updatedMovieTime.day;
            movieTime.StartTime = updatedMovieTime.StartTime;
            movieTime.EndTime = updatedMovieTime.EndTime;
            movieTime.Price = updatedMovieTime.Price;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{movieTimesId}")]
        public async Task<IActionResult> DeleteMovieTime(int movieTimesId)
        {
            var movieTime = await _context.MovieTimes.FindAsync(movieTimesId);
            if (movieTime == null)
                return NotFound();

            _context.MovieTimes.Remove(movieTime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<MovieTimesModel>> CreateMovieTime([FromBody] MovieTimesDto dto)
        {
            if (dto == null)
                return BadRequest();

            var movieTime = new MovieTimesModel
            {
                MovieId = dto.MovieId,
                CinemaId = dto.CinemaId,
                RoomId = dto.RoomId,
                day = dto.day,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Price = dto.Price
            };

            _context.MovieTimes.Add(movieTime);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSeancesByMovieTimesId), new { id = movieTime.MovieTimesId }, movieTime);
        }
    }
}
