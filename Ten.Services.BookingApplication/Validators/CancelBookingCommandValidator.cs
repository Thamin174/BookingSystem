using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ten.Services.BookingApplication.Commands.CancelBooking;

namespace Ten.Services.BookingApplication.Validators
{
    public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
    {
        public CancelBookingCommandValidator()
        {
            RuleFor(x => x.BookingReference)
                .NotEmpty().WithMessage("Booking Reference cannot be empty");
        }
    }
}
