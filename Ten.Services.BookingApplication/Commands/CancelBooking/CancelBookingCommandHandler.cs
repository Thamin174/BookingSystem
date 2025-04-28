using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingDomain.Entities;
using Ten.Services.BookingDomain.Exceptions;

namespace Ten.Services.BookingApplication.Commands.CancelBooking
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, bool>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<InventoryItem> _inventoryRepository;

        public CancelBookingCommandHandler(
            IBookingRepository bookingRepository,
            IRepository<Member> memberRepository,
            IRepository<InventoryItem> inventoryRepository)
        {
            _bookingRepository = bookingRepository;
            _memberRepository = memberRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByReferenceAsync(request.BookingReference)
                         ?? throw new NotFoundException("Booking not found");

            var member = await _memberRepository.GetByIdAsync(booking.MemberId)
                         ?? throw new NotFoundException("Member not found");

            var inventoryItem = await _inventoryRepository.GetByIdAsync(booking.InventoryItemId)
                             ?? throw new NotFoundException("Inventory item not found");

            _bookingRepository.Delete(booking);

            member.BookingCount -= 1;
            inventoryItem.RemainingCount += 1;

            _memberRepository.Update(member);
            _inventoryRepository.Update(inventoryItem);

            await _bookingRepository.SaveChangesAsync();
            await _memberRepository.SaveChangesAsync();
            await _inventoryRepository.SaveChangesAsync();

            return true;
        }
    }
}
