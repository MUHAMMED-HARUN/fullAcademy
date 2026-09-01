using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Assignment.AssignmentScopeInfo.Command;
using System.Application.Services.Assignment.AssignmentScopeInfo.Query;
using System.Application.Services.Institute.Program.Command;

namespace School_Academy.Controllers.Assignment
{

    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentScopeInfoController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AssignmentScopeInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewAssignmentScopeInfo")]
        public async Task<IActionResult> Add([FromBody] NewAssignmentScopeInfoCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateAssignmentScopeInfo")]
        public async Task<IActionResult> Update([FromBody] UpdateAssignmentScopeInfoCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteAssignmentScopeInfo")]
        public async Task<IActionResult> Delete([FromBody] DeleteAssignmentScopeInfoCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentScopeInfoList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllAssignmentScopeInfoQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetAssignmentScopeInfoBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetAssignmentScopeInfoByQuery query)
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
