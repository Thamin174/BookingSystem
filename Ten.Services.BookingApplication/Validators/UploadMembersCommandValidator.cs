using FluentValidation;
using Ten.Services.BookingApplication.Commands.UploadMembers;

namespace Ten.Services.BookingApplication.Validators
{
    public class UploadMembersCommandValidator : AbstractValidator<UploadMembersCommand>
    {
        public UploadMembersCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File must not be empty");
        }
    }
}
