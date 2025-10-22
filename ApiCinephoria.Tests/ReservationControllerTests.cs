using ApiCinephoria.Controllers;
using ApiCinephoria.Models;
using ApiCinephoria.Data;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace ApiCinephoria.Tests.Unit
{
    public class ReservationControllerTests
    {
        private readonly Mock<IReservationService> _mockService;
        private readonly ReservationController _controller;


        // Exemple d'ID valide MongoDB
        private const string ValidObjectId = "64a8d7e6f5e4c3d2b1a0f123";
        public ReservationControllerTests()
        {
            _mockService = new Mock<IReservationService>();
            _controller = new ReservationController(_mockService.Object);
        }

        [Fact]
        public async Task GetReservations_ShouldReturnOk_WithList()
        {
            // Arrange
            var reservations = new List<ReservationModel>
            {
                new ReservationModel { Id = "1", MovieTitle = "Test Movie 1" },
                new ReservationModel { Id = "2", MovieTitle = "Test Movie 2" }
            };
            _mockService.Setup(s => s.GetFilteredAsync(null, null, null, null, null, null))
                        .ReturnsAsync(reservations);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<ReservationModel>>>(result);
            var returnValue = Assert.IsType<List<ReservationModel>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetReservationById_ShouldReturnOk_WithReservation()
        {
            // Arrange
            var reservation = new ReservationModel { Id = "123", MovieTitle = "Test Movie" };
            _mockService.Setup(s => s.GetAsync("123")).ReturnsAsync(reservation);

            // Act
            var result = await _controller.Get("123");

            // Assert
            var okResult = Assert.IsType<ActionResult<ReservationModel>>(result);
            Assert.NotNull(okResult.Value);
            Assert.Equal("123", okResult.Value.Id);
        }

        [Fact]
        public async Task GetReservationById_ShouldReturnNotFound_WhenMissing()
        {
            // Arrange
            _mockService.Setup(s => s.GetAsync("missing")).ReturnsAsync((ReservationModel?)null);

            // Act
            var result = await _controller.Get("missing");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostReservation_ShouldReturnCreatedAtAction_AndCallCreateAsync()
        {
            // Arrange
            _mockService.Setup(s => s.CreateAsync(It.IsAny<ReservationModel>())).Returns(Task.CompletedTask).Verifiable();

            var reservation = new ReservationCreateModel
            {
                UserId = 1,
                MovieId = 1,
                MovieTitle = "Film Test",
                CinemaId = 1,
                CinemaName = "Cinéma Test",
                SeatId = 1,
                SeatName = "A1",
                RoomId = 1,
                RoomName = "Salle 1",
                ReservationDate = DateTime.Today,
                ReservationTime = DateTime.Now
            };

            // Act
            var result = await _controller.Post(reservation);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdResult.ActionName);

            var createdReservation = Assert.IsType<ReservationModel>(createdResult.Value);
            Assert.Equal(reservation.MovieTitle, createdReservation.MovieTitle);

            _mockService.Verify(s => s.CreateAsync(It.IsAny<ReservationModel>()), Times.Once);
        }

        [Fact]
        public async Task PutReservation_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            var existingReservation = new ReservationModel { Id = "abc" };
            _mockService.Setup(s => s.GetAsync("abc")).ReturnsAsync(existingReservation);
            _mockService.Setup(s => s.UpdateAsync("abc", It.IsAny<ReservationModel>())).Returns(Task.CompletedTask);

            var updatedReservation = new ReservationModel
            {
                Id = "abc",
                MovieTitle = "Updated Title"
            };

            // Act
            var result = await _controller.Update("abc", updatedReservation);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.UpdateAsync("abc", updatedReservation), Times.Once);
        }

        [Fact]
        public async Task PutReservation_ShouldReturnNotFound_WhenReservationDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetAsync("nonexistent")).ReturnsAsync((ReservationModel?)null);

            var updatedReservation = new ReservationModel { Id = "nonexistent" };

            // Act
            var result = await _controller.Update("nonexistent", updatedReservation);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task DeleteReservation_ShouldReturnNoContent_WhenSuccess()
        {
            // Arrange
            var reservation = new ReservationModel { Id = ValidObjectId };
            _mockService.Setup(s => s.GetAsync(ValidObjectId)).ReturnsAsync(reservation);
            _mockService.Setup(s => s.DeleteAsync(ValidObjectId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(ValidObjectId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.DeleteAsync(ValidObjectId), Times.Once);
        }

        [Fact]
        public async Task DeleteReservation_ShouldReturnNotFound_WhenReservationMissing()
        {
            // Arrange
            _mockService.Setup(s => s.GetAsync(ValidObjectId)).ReturnsAsync((ReservationModel?)null);

            // Act
            var result = await _controller.Delete(ValidObjectId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteReservation_ShouldReturnBadRequest_WhenIdInvalid()
        {
            // Act
            var result = await _controller.Delete("invalid-id");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID invalide", badRequestResult.Value);
        }
    }
}
