using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ten.Services.BookingApplication.Commands.UploadInventory
{
    public class UploadInventoryCommand : IRequest<bool>
    {
        public IFormFile File { get; set; }
    }
}
