using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiCinephoria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(ReviewsService reviewsService) : ControllerBase
    {
        private readonly ReviewsService _reviewsService = reviewsService;

        [HttpGet]
        public async Task<ActionResult<List<ReviewsModel>>> Get(
         int? movieId = null,
         int? userId = null)
            {
                var reviews = await _reviewsService.GetFilteredAsync(userId, movieId);

                
                return Ok(reviews);
            }
        [HttpGet("validated")]
        public async Task<ActionResult<List<ReviewsModel>>> GetValidatedReviews(
         int? movieId = null,
         int? userId = null)
            {
                var reviews = await _reviewsService.GetFilteredAsync(userId, movieId);
                var validatedReviews = reviews.Where(r => r.ReviewsValidation).ToList();
                return Ok(validatedReviews);
            }

        [HttpGet("average")]
        public async Task<ActionResult<object>> GetAverageReview([FromQuery] int movieId)
        {
            var reviews = await _reviewsService.GetFilteredAsync(null, movieId);

            var validatedReviews = reviews.Where(r => r.ReviewsValidation).ToList();

            if (validatedReviews == null || validatedReviews.Count == 0)
            {
                return Ok(new { movieId, averageReview = 0.0 });
            }

            double average = validatedReviews.Average(r => r.Reviews);

            return Ok(new { movieId, averageReview = average });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReviewsCreateModel reviews)
        {
            

            var reviewsModel = new ReviewsModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = reviews.UserId,
                UserName = reviews.UserName,
                MovieId = reviews.MovieId,
                MovieTitle = reviews.MovieTitle,
                Reviews = reviews.Reviews,
                Comments = reviews.Comments,
                ReviewsDate = reviews.ReviewsDate,
                ReviewsValidation = false

            };

            await _reviewsService.CreateAsync(reviewsModel);

            return CreatedAtAction(nameof(Get), new { id = reviewsModel.Id }, reviewsModel);
        }

        [HttpPut("validate/{id}")]
        public async Task<IActionResult> ValidateReview(string id)
        {
            var success = await _reviewsService.ValidateReviewAsync(id);

            if (!success)
                return NotFound();

            return Ok(new { message = "Review validated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var reviews = await _reviewsService.GetFilteredAsync();
            var reviewToDelete = reviews.FirstOrDefault(r => r.Id == id);

            if (reviewToDelete == null)
                return NotFound();

            var collection = _reviewsService.GetType()
                .GetField("_reviews", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_reviewsService) as IMongoCollection<ReviewsModel>;

            if (collection == null)
                return StatusCode(500, "Erreur d'accès à la collection MongoDB.");

            var result = await collection.DeleteOneAsync(r => r.Id == id);

            if (result.DeletedCount == 0)
                return NotFound();

            return NoContent();
        }

    }
}
