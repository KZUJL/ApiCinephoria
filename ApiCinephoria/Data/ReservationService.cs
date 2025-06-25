using ApiCinephoria.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApiCinephoria.Data
{
    public class ReservationService(IMongoDatabase database) : IReservationService
    {
        private readonly IMongoCollection<ReservationModel> _reservations = database.GetCollection<ReservationModel>("Reservations");

        public async Task<List<ReservationModel>> GetAsync() =>
            await _reservations.Find(_ => true).ToListAsync();

        public async Task<ReservationModel> GetAsync(string id) =>
            await _reservations.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ReservationModel reservation) =>
            await _reservations.InsertOneAsync(reservation);

        public async Task UpdateAsync(string id, ReservationModel updatedReservation) =>
            await _reservations.ReplaceOneAsync(x => x.Id == id, updatedReservation);

        public async Task DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("ID invalide", nameof(id));
            }

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
            {
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.CinemaId, cinemaId);
            }

            if (movieId.HasValue)
            {
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.MovieId, movieId);
            }

            if (reservationDate.HasValue)
            {
                filter &= Builders<ReservationModel>.Filter.Eq(r => r.ReservationDate, reservationDate.Value);
            }

            if (reservationTime.HasValue)
            {
                filter &= Builders<ReservationModel>.Filter.Eq("ReservationTime", reservationTime);
            }

            return await _reservations.Find(filter).ToListAsync();
        }
    }
}
