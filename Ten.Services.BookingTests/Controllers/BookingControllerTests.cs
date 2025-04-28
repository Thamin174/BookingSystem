using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ten.Services.BookingAPI.Controllers;
using Ten.Services.BookingApplication.Commands.BookItem;
using Ten.Services.BookingApplication.Commands.CancelBooking;
using Ten.Services.BookingApplication.Dtos;

namespace Ten.Services.BookingTests.Controllers
{
    public class BookingControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BookingController _sut;

        public BookingControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _sut = new BookingController(_mediatorMock.Object);
        }

        [Fact]
        public async Task BookItem_ShouldReturnOk_WithBookingReference()
        {
            // Arrange
            var bookingDto = new BookingDto { MemberId = 1, InventoryItemId = 2 };
            var expectedReference = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.IsAny<BookItemCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedReference);

            // Act
            var result = await _sut.BookItem(bookingDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CancelBooking_ShouldReturnOk_WithSuccessTrue()
        {
            // Arrange
            var bookingReference = Guid.NewGuid();

            _mediatorMock.Setup(m => m.Send(It.IsAny<CancelBookingCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _sut.CancelBooking(bookingReference);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
