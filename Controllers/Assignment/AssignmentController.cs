using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Assignment.Assignment.Query;

namespace School_Academy.Controllers.Assignment
{

    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewAssignment")]
        public async Task<IActionResult> Add([FromBody] NewAssignmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateAssignment")]
        public async Task<IActionResult> Update([FromBody] UpdateAssignmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteAssignment")]
        public async Task<IActionResult> Delete([FromBody] DeleteAssignmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllAssignmentQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetAssignmentByQuery query)
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
