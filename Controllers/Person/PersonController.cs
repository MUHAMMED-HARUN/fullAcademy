using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Person.Person.Command;
using System.Application.Services.Person.Person.Query;

namespace School_Academy.Controllers.Person
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> Delete([FromBody] DeletePersonCommand command)
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

        [HttpGet("get-for-upsert/{id:guid}")]
        public async Task<IActionResult> GetForUpsert([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetPersonForUpsertQuery { PersonID = id });

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("IsExistPersonBy")]
        public async Task<IActionResult> IsExistBy([FromQuery] IsExistPersonByQuery query)
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
