using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiCinephoria;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

public class ReservationControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ReservationControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private static StringContent SerializeToJson(object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    [Fact]
    public async Task PostPutDeleteReservation_Sequence_ShouldSucceed()
    {
        // 1. POST - Création
        var reservationToCreate = new ReservationCreateModel
        {
            UserId = 1,
            MovieId = 1,
            MovieTitle = "Film Test Initial",
            CinemaId = 1,
            CinemaName = "Cinéma Test",
            SeatId = 1,
            SeatName = "A1",
            RoomId = 1,
            RoomName = "Salle 1",
            ReservationDate = System.DateTime.Today,
            ReservationTime = System.DateTime.Now
        };
        var postContent = SerializeToJson(reservationToCreate);
        var postResponse = await _client.PostAsync("/api/reservation", postContent);
        postResponse.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, postResponse.StatusCode);
        var postBody = await postResponse.Content.ReadAsStringAsync();
        var createdReservation = JsonConvert.DeserializeObject<ReservationModel>(postBody);
        Assert.NotNull(createdReservation);
        Assert.False(string.IsNullOrEmpty(createdReservation.Id));

        // 2. PUT - Mise à jour
        var updatedReservation = new ReservationModel
        {
            Id = createdReservation.Id,
            UserId = createdReservation.UserId,
            MovieId = 2, // modification
            MovieTitle = "Film Test Modifié",
            CinemaId = createdReservation.CinemaId,
            CinemaName = createdReservation.CinemaName,
            SeatId = createdReservation.SeatId,
            SeatName = createdReservation.SeatName,
            RoomId = createdReservation.RoomId,
            RoomName = createdReservation.RoomName,
            ReservationDate = createdReservation.ReservationDate,
            ReservationTime = createdReservation.ReservationTime
        };
        var putContent = SerializeToJson(updatedReservation);
        var putResponse = await _client.PutAsync($"/api/reservation/{createdReservation.Id}", putContent);
        putResponse.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.NoContent, putResponse.StatusCode);

        // 3. GET - Vérification de la mise à jour
        var getResponse = await _client.GetAsync($"/api/reservation/{createdReservation.Id}");
        getResponse.EnsureSuccessStatusCode();
        var getBody = await getResponse.Content.ReadAsStringAsync();
        var fetchedReservation = JsonConvert.DeserializeObject<ReservationModel>(getBody);
        Assert.NotNull(fetchedReservation);
        Assert.Equal(updatedReservation.MovieId, fetchedReservation.MovieId);
        Assert.Equal(updatedReservation.MovieTitle, fetchedReservation.MovieTitle);

        // 4. DELETE - Suppression
        var deleteResponse = await _client.DeleteAsync($"/api/reservation/{createdReservation.Id}");
        deleteResponse.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }


    [Fact]
    public async Task GetReservations_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("/api/reservation");
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(body));
    }

    [Fact]
    public async Task PostReservation_WithMissingFields_ShouldReturnBadRequest()
    {
        var reservation = new { UserId = 1 }; // Manque plein de champs obligatoires

        var content = SerializeToJson(reservation);

        var response = await _client.PostAsync("/api/reservation", content);

        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
