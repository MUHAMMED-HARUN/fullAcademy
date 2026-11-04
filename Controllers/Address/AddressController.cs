using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Address.Address.Query;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewAddress")]
        [Authorize(Policy = "Can_Create_Address")]
        public async Task<IActionResult> Add([FromBody] NewAddressCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateAddress")]
        [Authorize(Policy = "Can_Update_Address")]
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
        [Authorize(Policy = "Can_Read_Address")]
        public async Task<IActionResult> GetById([FromQuery] GetAddressByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
   

    }
}
