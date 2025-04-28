using FluentAssertions;
using Moq;
using Ten.Services.BookingApplication.Commands.BookItem;
using Ten.Services.BookingApplication.Dtos;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingDomain.Entities;
using Ten.Services.BookingDomain.Exceptions;

namespace Ten.Services.BookingTests.Application
{
    public class BookingTests
    {
        private readonly Mock<IRepository<Member>> _memberRepositoryMock;
        private readonly Mock<IRepository<InventoryItem>> _inventoryRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;

        public BookingTests()
        {
            _memberRepositoryMock = new Mock<IRepository<Member>>();
            _inventoryRepositoryMock = new Mock<IRepository<InventoryItem>>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
        }

        [Fact]
        public async Task BookItem_Should_Create_Booking_When_Valid()
        {
            // Arrange
            var member = new Member { Id = 1, BookingCount = 1 };
            var inventory = new InventoryItem { Id = 1, RemainingCount = 5 };
            var handler = new BookItemCommandHandler(_memberRepositoryMock.Object, _inventoryRepositoryMock.Object, _bookingRepositoryMock.Object);

            _memberRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(member);
            _inventoryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(inventory);

            var command = new BookItemCommand
            {
                BookingDto = new BookingDto
                {
                    MemberId = 1,
                    InventoryItemId = 1
                }
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _bookingRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Once);
        }

        [Fact]
        public async Task BookItem_Should_Throw_When_Max_Booking_Limit_Reached()
        {
            // Arrange
            var member = new Member { Id = 1, BookingCount = 2 };
            var inventory = new InventoryItem { Id = 1, RemainingCount = 5 };
            var handler = new BookItemCommandHandler(_memberRepositoryMock.Object, _inventoryRepositoryMock.Object, _bookingRepositoryMock.Object);

            _memberRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(member);
            _inventoryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(inventory);

            var command = new BookItemCommand
            {
                BookingDto = new BookingDto
                {
                    MemberId = 1,
                    InventoryItemId = 1
                }
            };

            // Act
            Func<Task> act = async () => { await handler.Handle(command, CancellationToken.None); };

            // Assert
            await act.Should().ThrowAsync<MaxBookingLimitReachedException>();
        }
    }
}
