using MediatR;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingDomain.Entities;
using Ten.Services.BookingDomain.Exceptions;


namespace Ten.Services.BookingApplication.Commands.BookItem
{
    public class BookItemCommandHandler : IRequestHandler<BookItemCommand, Guid>
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<InventoryItem> _inventoryRepository;
        private readonly IBookingRepository _bookingRepository;

        public BookItemCommandHandler(
            IRepository<Member> memberRepository,
            IRepository<InventoryItem> inventoryRepository,
            IBookingRepository bookingRepository)
        {
            _memberRepository = memberRepository;
            _inventoryRepository = inventoryRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<Guid> Handle(BookItemCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(request.BookingDto.MemberId)
                         ?? throw new NotFoundException("Member not found");

            var inventoryItem = await _inventoryRepository.GetByIdAsync(request.BookingDto.InventoryItemId)
                             ?? throw new NotFoundException("Inventory item not found");

            if (member.BookingCount >= 2)
                throw new MaxBookingLimitReachedException();

            if (inventoryItem.RemainingCount <= 0)
                throw new InventoryDepletedException();

            var booking = new Booking
            {
                MemberId = member.Id,
                InventoryItemId = inventoryItem.Id
            };

            await _bookingRepository.AddAsync(booking);

            member.BookingCount += 1;
            inventoryItem.RemainingCount -= 1;

            _memberRepository.Update(member);
            _inventoryRepository.Update(inventoryItem);

            await _bookingRepository.SaveChangesAsync();
            await _memberRepository.SaveChangesAsync();
            await _inventoryRepository.SaveChangesAsync();

            return booking.BookingReference;
        }
    }
}
