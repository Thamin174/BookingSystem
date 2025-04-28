using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ten.Services.BookingAPI.Controllers;
using Ten.Services.BookingApplication.Commands.UploadInventory;
using Ten.Services.BookingApplication.Commands.UploadMembers;

namespace Ten.Services.BookingTests.Controllers
{
    public class UploadControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UploadController _sut;

        public UploadControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _sut = new UploadController(_mediatorMock.Object);
        }

        [Fact]
        public async Task UploadMembers_ShouldReturnOk_WhenUploadIsSuccessful()
        {
            // Arrange
            var uploadCommand = new UploadMembersCommand
            {
                File = new FormFile(Stream.Null, 0, 0, "file", "members.csv")
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UploadMembersCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _sut.UploadMembers(uploadCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value!;
        }

        [Fact]
        public async Task UploadInventory_ShouldReturnOk_WhenUploadIsSuccessful()
        {
            // Arrange
            var uploadCommand = new UploadInventoryCommand
            {
                File = new FormFile(Stream.Null, 0, 0, "file", "inventory.csv")
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UploadInventoryCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _sut.UploadInventory(uploadCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic response = okResult.Value!;
        }
    }
}
