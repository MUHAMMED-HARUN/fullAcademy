using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class AssignmentScopeInfoController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AssignmentScopeInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewAssignmentScopeInfo")]
        [Authorize(Policy = "Can_Create_AssignmentScopeInfo")]
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
        [Authorize(Policy = "Can_Update_AssignmentScopeInfo")]
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
        [Authorize(Policy = "Can_Delete_AssignmentScopeInfo")]
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
        [Authorize(Policy = "Can_Read_AssignmentScopeInfo")]
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
        [Authorize(Policy = "Can_Read_AssignmentScopeInfo")]
        public async Task<IActionResult> GetBy([FromQuery] GetAssignmentScopeInfoByQuery query)
        {

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("fetchAssignmentScopeinfosForAssigment")]
        [Authorize(Policy = "Can_Read_AssignmentScopeInfo")]
        public async Task<IActionResult> fetchAssignmentScopeinfosForAssigment([FromQuery] GetByAssigmentIDQuery query)
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
