using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Assignment.AssignmentScope.Command;
using System.Application.Services.Assignment.AssignmentScope.Query;

namespace School_Academy.Controllers.Assignment
{

    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentScopeController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AssignmentScopeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewAssignmentScope")]
        public async Task<IActionResult> Add([FromBody] NewAssignmentScopeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateAssignmentScope")]
        public async Task<IActionResult> Update([FromBody] UpdateAssignmentScopeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteAssignmentScope")]
        public async Task<IActionResult> Delete([FromBody] DeleteAssignmentScopeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentScopeList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllAssignmentScopeQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("GetCanNominateList")]
        public async Task<IActionResult> GetCanNominateList([FromQuery] GetCanNominateQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentScopeBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetAssignmentScopeByQuery query)
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
