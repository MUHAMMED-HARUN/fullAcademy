using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Group.Command;
using System.Application.Services.Institute.Group.Query;

namespace School_Academy.Controllers.Institute
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewGroup")]
        [Authorize(Policy = "Can_Create_Group")]
        public async Task<IActionResult> Add([FromBody] NewGroupCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateGroup")]
        [Authorize(Policy = "Can_Update_Group")]
        public async Task<IActionResult> Update([FromBody] UpdateGroupCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteGroup")]
        [Authorize(Policy = "Can_Delete_Group")]
        public async Task<IActionResult> Delete([FromBody] DeleteGroupCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetGroupList")]
        [Authorize(Policy = "Can_Read_Group")]
        public async Task<IActionResult> GetList([FromQuery] GetAllGroupQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetMyTeachingGroups")]
        [Authorize(Policy = "Can_Read_Group")]
        public async Task<IActionResult> GetMyTeachingGroups([FromQuery] GetMyTeachingGroupsQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetGroupBy")]
        [Authorize(Policy = "Can_Read_Group")]
        public async Task<IActionResult> GetBy([FromQuery] GetGroupByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("IsExistGroupBy")]
        [Authorize(Policy = "Can_Read_Group")]
        public async Task<IActionResult> IsExistBy([FromQuery] IsExistGroupByQuery query)
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
