using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ten.Services.BookingApplication.Commands.UploadMembers
{
    public class UploadMembersCommand : IRequest<bool>
    {
        public IFormFile File { get; set; }
    }
}
