using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.Country.Command;
using System.Application.Services.Address.Country.Query;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewCountry")]
        public async Task<IActionResult> Add([FromBody] NewCountryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPost("UpdateCountry")]
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
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllCountryQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetCountryByID")]
        public async Task<IActionResult> GetById()
        {
            return Ok();
        }


    }
}
