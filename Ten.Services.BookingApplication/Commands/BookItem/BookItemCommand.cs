using MediatR;
using Ten.Services.BookingApplication.Dtos;

namespace Ten.Services.BookingApplication.Commands.BookItem
{
    public class BookItemCommand : IRequest<Guid>
    {
        public BookingDto BookingDto { get; set; }
    }
}
