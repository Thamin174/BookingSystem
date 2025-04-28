using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ten.Services.BookingApplication.Commands.BookItem;
using Ten.Services.BookingApplication.Commands.CancelBooking;
using Ten.Services.BookingApplication.Dtos;

namespace Ten.Services.BookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookItem([FromBody] BookingDto bookingDto)
        {
            var command = new BookItemCommand { BookingDto = bookingDto };
            var bookingReference = await _mediator.Send(command);
            return Ok(new { BookingReference = bookingReference });
        }

        [HttpDelete("cancel/{reference}")]
        public async Task<IActionResult> CancelBooking(Guid reference)
        {
            var command = new CancelBookingCommand { BookingReference = reference };
            var result = await _mediator.Send(command);
            return Ok(new { Success = result });
        }
    }
}
