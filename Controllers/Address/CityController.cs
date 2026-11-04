using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.City.Command;
using System.Application.Services.Address.City.Query;


namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CityController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewCity")]
        [Authorize(Policy = "Can_Create_City")]
        public async Task<IActionResult> Add([FromBody] NewCityCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateCity")]
        [Authorize(Policy = "Can_Update_City")]
        public async Task<IActionResult> Update([FromBody] UpdateCityCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetCityList")]
        [Authorize(Policy = "Can_Read_City")]
        public async Task<IActionResult> GetList([FromQuery] GetAllCityQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetCityOptions")]
        [Authorize(Policy = "Can_Read_City")]
        public async Task<IActionResult> GetOptions()
        {
            var result = await _mediator.Send(new GetCityOptionsQuery());

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetCityByID")]
        [Authorize(Policy = "Can_Read_City")]
        public async Task<IActionResult> GetById([FromQuery] GetCityByQuery query)
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
