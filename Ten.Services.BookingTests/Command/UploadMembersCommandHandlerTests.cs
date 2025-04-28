using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;
using Ten.Services.BookingApplication.Commands.UploadMembers;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingDomain.Entities;

namespace Ten.Services.BookingTests.Command
{
    public class UploadMembersCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUploadMembersSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Member>>();

            mockRepository.Setup(r => r.AddAsync(It.IsAny<Member>()))
                          .Returns(Task.CompletedTask);

            mockRepository.Setup(r => r.SaveChangesAsync())
                          .Returns(Task.CompletedTask);

            var handler = new UploadMembersCommandHandler(mockRepository.Object);

            var csvContent =
                    @"name,surname,booking_count,date_joined
                    John,Doe,1,01/01/2024
                    Alice,Smith,0,02/02/2024";

            var fileBytes = Encoding.UTF8.GetBytes(csvContent);
            var formFile = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, "file", "members.csv");

            var command = new UploadMembersCommand
            {
                File = formFile
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mockRepository.Verify(r => r.AddAsync(It.IsAny<Member>()), Times.Exactly(2)); // 2 members
            mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenFileIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Member>>();
            var handler = new UploadMembersCommandHandler(mockRepository.Object);

            var command = new UploadMembersCommand
            {
                File = null // No file
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
