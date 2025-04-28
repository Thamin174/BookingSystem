using Microsoft.EntityFrameworkCore;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingDomain.Entities;
using Ten.Services.BookingInfrastructure.Data;

namespace Ten.Services.BookingInfrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetBookingCountForMemberAsync(int memberId)
        {
            return await _context.Bookings.CountAsync(b => b.MemberId == memberId);
        }

        public async Task<Booking> GetBookingByReferenceAsync(Guid reference)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.BookingReference == reference);
        }
    }
}
