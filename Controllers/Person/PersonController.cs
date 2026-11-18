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
        [Authorize(Policy = "Can_Create_Person")]
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
        [Authorize(Policy = "Can_Update_Person")]
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
        [Authorize(Policy = "Can_Delete_Person")]
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
        [Authorize(Policy = "Can_Read_Person")]
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
        [Authorize(Policy = "Can_Read_Person")]
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
        [Authorize(Policy = "Can_Read_Person")]
        public async Task<IActionResult> GetForUpsert([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetPersonForUpsertQuery { PersonID = id });

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("IsExistPersonBy")]
        [Authorize(Policy = "Can_Read_Person")]
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
