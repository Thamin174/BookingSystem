using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;
using Ten.Services.BookingApplication.Commands.UploadInventory;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingDomain.Entities;

namespace Ten.Services.BookingTests.Command
{
    public class UploadInventoryCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUploadInventorySuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<InventoryItem>>();

            mockRepository.Setup(r => r.AddAsync(It.IsAny<InventoryItem>()))
                          .Returns(Task.CompletedTask);

            mockRepository.Setup(r => r.SaveChangesAsync())
                          .Returns(Task.CompletedTask);

            var handler = new UploadInventoryCommandHandler(mockRepository.Object);

            var csvContent =
                    @"title,description,remaining_count,expiration_date
                    Bali,Beautiful trip to Bali,5,19/11/2030
                    Paris,Trip to Paris,3,21/11/2030";

            var fileBytes = Encoding.UTF8.GetBytes(csvContent);
            var formFile = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, "file", "inventory.csv");

            var command = new UploadInventoryCommand
            {
                File = formFile
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mockRepository.Verify(r => r.AddAsync(It.IsAny<InventoryItem>()), Times.Exactly(2)); // Two records
            mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenFileIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<InventoryItem>>();
            var handler = new UploadInventoryCommandHandler(mockRepository.Object);

            var command = new UploadInventoryCommand
            {
                File = null // No file
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
