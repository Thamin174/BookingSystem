using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ten.Services.BookingApplication.Commands.CancelBooking
{
    public class CancelBookingCommand : IRequest<bool>
    {
        public Guid BookingReference { get; set; }
    }
}
