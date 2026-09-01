using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Address.Address.Query;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewAddress")]
        public async Task<IActionResult> Add(NewAddressCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPost("UpdateAddress")]
        public async Task<IActionResult> Update([FromBody] UpdateAddressCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("GetAddressBy")]
        public async Task<IActionResult> GetById([FromQuery] GetAddressByQuery Query)
        {
            var result = await _mediator.Send(Query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
   

    }
}
