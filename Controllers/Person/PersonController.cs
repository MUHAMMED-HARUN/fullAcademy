using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.Neighborhood.Command;
using System.Application.Services.Address.Neighborhood.Query;
using System.Application.Services.Person.Person.Command;
using System.Application.Services.Person.Person.Query;
using System.Domain.Enums.Address;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {

        private readonly IMediator _mediator;
        public PersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewPerson")]
        public async Task<IActionResult> Add([FromBody] NewPersonCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdatePerson")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeletePerson")]
        public async Task<IActionResult> Delete( DeletePersonCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetPersonList")]
        public async Task<IActionResult> GetList([FromQuery] GetAllPersonQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetPersonBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetPersonByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistPersonBy")]
        public async Task<IActionResult> IsExistBy(IsExistPersonByQuery query)
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
