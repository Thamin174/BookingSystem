using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ten.Services.BookingApplication.Commands.UploadInventory;
using Ten.Services.BookingApplication.Commands.UploadMembers;

namespace Ten.Services.BookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("members")]
        public async Task<IActionResult> UploadMembers([FromForm] UploadMembersCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Success = result });
        }

        [HttpPost("inventory")]
        public async Task<IActionResult> UploadInventory([FromForm] UploadInventoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Success = result });
        }
    }
}
