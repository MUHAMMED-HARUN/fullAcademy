using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Assignment.AssignmentNomination.Command;
using System.Application.Services.Assignment.AssignmentNomination.Query;

namespace School_Academy.Controllers.Assignment
{

    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentNominationController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AssignmentNominationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewAssignmentNomination")]
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
        public async Task<IActionResult> Delete([FromBody] DeleteAssignmentNominationCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentNominationList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllAssignmentNominationQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentNominationBy")]
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
