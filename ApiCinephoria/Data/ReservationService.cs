using ApiCinephoria.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiCinephoria.Data
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<ReservationModel> _reservations;

        public ReservationService(IMongoDatabase database)
        {
            _reservations = database.GetCollection<ReservationModel>("Reservations");

            // Index UNIQUE pour empêcher double réservation
            var indexKeys = Builders<ReservationModel>.IndexKeys
                .Ascending(r => r.SeatId)
                .Ascending(r => r.MovieId)
                .Ascending(r => r.ReservationTime);

            var indexOptions = new CreateIndexOptions
            {
                Unique = true
            };

            var indexModel =
                new CreateIndexModel<ReservationModel>(indexKeys, indexOptions);

            _reservations.Indexes.CreateOne(indexModel);
        }

        public async Task<List<ReservationModel>> GetAsync() =>
            await _reservations.Find(_ => true).ToListAsync();

        public async Task<ReservationModel> GetAsync(string id) =>
            await _reservations.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ReservationModel reservation)
        {
            try
            {
                await _reservations.InsertOneAsync(reservation);
            }
            catch (MongoWriteException ex)
                when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                // Cas où le siège vient d’être réservé par quelqu’un d’autre
                throw new Exception("Ce siège vient d’être réservé.");
            }
        }

        public async Task UpdateAsync(string id, ReservationModel updatedReservation) =>
            await _reservations.ReplaceOneAsync(x => x.Id == id, updatedReservation);

        public async Task DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                throw new ArgumentException("ID invalide", nameof(id));

            await _reservations.DeleteOneAsync(x => x.Id == objectId.ToString());
        }

        public async Task<List<ReservationModel>> GetFilteredAsync(
            int? userId = null,
            int? cinemaId = null,
            int? movieId = null,
            DateTime? reservationDate = null,
            DateTime? reservationTime = null,
            int? seatId = null)
        {
            var filter = Builders<ReservationModel>.Filter.Empty;

            if (userId.HasValue)
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.UserId, userId.Value);

            if (cinemaId.HasValue)
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.CinemaId, cinemaId.Value);

            if (movieId.HasValue)
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.MovieId, movieId.Value);

            if (reservationDate.HasValue)
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.ReservationDate, reservationDate.Value);

            if (reservationTime.HasValue)
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.ReservationTime, reservationTime.Value);

            if (seatId.HasValue)
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.SeatId, seatId.Value);

            return await _reservations.Find(filter).ToListAsync();
        }
    }
}
