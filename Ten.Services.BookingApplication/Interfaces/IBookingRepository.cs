using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ten.Services.BookingDomain.Entities;

namespace Ten.Services.BookingApplication.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<int> GetBookingCountForMemberAsync(int memberId);
        Task<Booking> GetBookingByReferenceAsync(Guid reference);
    }
}
