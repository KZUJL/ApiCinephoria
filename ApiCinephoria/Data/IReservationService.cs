using ApiCinephoria.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCinephoria.Data
{
    public interface IReservationService
    {
        Task<List<ReservationModel>> GetAsync();
        Task<ReservationModel> GetAsync(string id);
        Task CreateAsync(ReservationModel reservation);
        Task UpdateAsync(string id, ReservationModel updatedReservation);
        Task DeleteAsync(string id);
        Task<List<ReservationModel>> GetFilteredAsync(
            int? userId = null,
            int? cinemaId = null,
            int? movieId = null,
            DateTime? reservationDate = null,
            DateTime? reservationTime = null,
            int? seatId = null);
    }
}
