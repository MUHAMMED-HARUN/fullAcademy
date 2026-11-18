using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Assignment.AssignmentNomination.Command;
using System.Application.Services.Assignment.AssignmentNomination.Query;

namespace School_Academy.Controllers.Assignment
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentNominationController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AssignmentNominationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewAssignmentNomination")]
        [Authorize(Policy = "Can_Create_AssignmentNomination")]
        public async Task<IActionResult> Add([FromBody] NewAssignmentNominationCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateAssignmentNomination")]
        [Authorize(Policy = "Can_Update_AssignmentNomination")]
        public async Task<IActionResult> Update([FromBody] UpdateAssignmentNominationCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteAssignmentNomination")]
        [Authorize(Policy = "Can_Delete_AssignmentNomination")]
        public async Task<IActionResult> Delete([FromBody] DeleteAssignmentNominationCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentNominationListFilter")]
        [Authorize(Policy = "Can_Read_AssignmentNomination")]
        public async Task<IActionResult> GetList([FromQuery] GetAllAssignmentNominationQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentNominationBy")]
        [Authorize(Policy = "Can_Read_AssignmentNomination")]
        public async Task<IActionResult> GetBy([FromQuery] GetAssignmentNominationByQuery query)
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
