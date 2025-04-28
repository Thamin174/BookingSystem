using FluentValidation;
using Ten.Services.BookingApplication.Commands.UploadInventory;

namespace Ten.Services.BookingApplication.Validators
{
    public class UploadInventoryCommandValidator : AbstractValidator<UploadInventoryCommand>
    {
        public UploadInventoryCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File must not be empty");
        }
    }
}
