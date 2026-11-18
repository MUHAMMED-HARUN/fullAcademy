using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Assignment.Assignment.Query;

namespace School_Academy.Controllers.Assignment
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewAssignment")]
        [Authorize(Policy = "Can_Create_Assignment")]
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
        [Authorize(Policy = "Can_Update_Assignment")]
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
        [Authorize(Policy = "Can_Delete_Assignment")]
        public async Task<IActionResult> Delete([FromBody] DeleteAssignmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentListFilter")]
        [Authorize(Policy = "Can_Read_Assignment")]
        public async Task<IActionResult> GetList([FromQuery] GetAllAssignmentQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentForUpsert")]
        [Authorize(Policy = "Can_Read_Assignment")]
        public async Task<IActionResult> GetForUpsert([FromQuery] GetAssignmentForUpsertQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetAssignmentBy")]
        [Authorize(Policy = "Can_Read_Assignment")]
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
