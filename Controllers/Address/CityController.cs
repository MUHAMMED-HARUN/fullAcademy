using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.City.Command;
using System.Application.Services.Address.City.Query;


namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewCity")]
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
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllCityQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetCityByID")]
        public async Task<IActionResult> GetById()
        {
            return Ok();
        }

    }
}
