using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ten.Services.BookingDomain.Exceptions
{
    public class MaxBookingLimitReachedException : Exception
    {
        public MaxBookingLimitReachedException()
            : base("Member has already reached the maximum booking limit (2 bookings).")
        {
        }
    }
}
