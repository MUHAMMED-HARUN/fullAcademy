using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Enrollment.TeachingAssignment.Command;
using System.Application.Services.Enrollment.TeachingAssignment.Query;

namespace School_Academy.Controllers.Enrollment
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachingAssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeachingAssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewTeachingAssignment")]
        public async Task<IActionResult> Add([FromBody] NewTeachingAssignmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateTeachingAssignment")]
        public async Task<IActionResult> Update([FromBody] UpdateTeachingAssignmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteTeachingAssignment")]
        public async Task<IActionResult> Delete([FromBody] DeleteTeachingAssignmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetTeachingAssignmentList")]
        public async Task<IActionResult> GetList([FromQuery] GetAllTeachingAssignmentQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("GetTeacherGroups")]
        public async Task<IActionResult> GetTeacherGroupList([FromQuery] GetTeacherGroupsQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetTeachingAssignmentBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetTeachingAssignmentByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("IsExistTeachingAssignmentBy")]
        public async Task<IActionResult> IsExistBy([FromQuery] IsExistTeachingAssignmentByQuery query)
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
