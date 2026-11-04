using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.Country.Command;
using System.Application.Services.Address.Country.Query;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountryController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewCountry")]
        [Authorize(Policy = "Can_Create_Country")]
        public async Task<IActionResult> Add([FromBody] NewCountryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateCountry")]
        [Authorize(Policy = "Can_Update_Country")]
        public async Task<IActionResult> Update([FromBody] UpdateCountryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetCountryList")]
        [Authorize(Policy = "Can_Read_Country")]
        public async Task<IActionResult> GetList([FromQuery] GetAllCountryQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetCountryOptions")]
        [Authorize(Policy = "Can_Read_Country")]
        public async Task<IActionResult> GetOptions()
        {
            var result = await _mediator.Send(new GetCountryOptionsQuery());

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetCountryByID")]
        [Authorize(Policy = "Can_Read_Country")]
        public async Task<IActionResult> GetById([FromQuery] GetCountryByQuery query)
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
