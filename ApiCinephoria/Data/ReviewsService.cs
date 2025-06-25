using ApiCinephoria.Models;
using MongoDB.Bson;
using MongoDB.Driver;
namespace ApiCinephoria.Data
{
    public class ReviewsService(IMongoDatabase database)
    {
        private readonly IMongoCollection<ReviewsModel> _reviews = database.GetCollection<ReviewsModel>("reviews");
        public async Task<List<ReviewsModel>> GetAsync() =>
           await _reviews.Find(_ => true).ToListAsync();
        public async Task CreateAsync(ReviewsModel reviews) =>
            await _reviews.InsertOneAsync(reviews);

        public async Task<List<ReviewsModel>> GetFilteredAsync(
            int? userId = null,
            int? movieId = null)
        {
            var filters = new List<FilterDefinition<ReviewsModel>>();

            if (userId.HasValue)
            {
                filters.Add(Builders<ReviewsModel>.Filter.Eq(r => r.UserId, userId.Value));
            }

            if (movieId.HasValue)
            {
                filters.Add(Builders<ReviewsModel>.Filter.Eq(r => r.MovieId, movieId.Value));
            }

            var filter = filters.Count > 0 ? Builders<ReviewsModel>.Filter.And(filters) : Builders<ReviewsModel>.Filter.Empty;

            return await _reviews.Find(filter).ToListAsync();
        }
        public async Task<bool> ValidateReviewAsync(string id)
        {
            var filter = Builders<ReviewsModel>.Filter.Eq(r => r.Id, id);
            var update = Builders<ReviewsModel>.Update.Set(r => r.ReviewsValidation, true);

            var result = await _reviews.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

    }
}
