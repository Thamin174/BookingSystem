using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ten.Services.BookingApplication.Commands.BookItem;

namespace Ten.Services.BookingApplication.Validators
{
    public class BookItemCommandValidator : AbstractValidator<BookItemCommand>
    {
        public BookItemCommandValidator()
        {
            RuleFor(x => x.BookingDto.MemberId)
                .GreaterThan(0).WithMessage("Invalid Member ID");

            RuleFor(x => x.BookingDto.InventoryItemId)
                .GreaterThan(0).WithMessage("Invalid Inventory Item ID");
        }
    }
}
