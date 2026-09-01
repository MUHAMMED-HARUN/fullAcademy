using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Group.Command;
using System.Application.Services.Institute.Group.Query;

namespace School_Academy.Controllers.Institute
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewGroup")]
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
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllGroupQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetGroupsByTeacherID")]
        public async Task<IActionResult> GetGroupsByTeacherID([FromQuery] GetGroupsByTeacherIDQuery query)
        {
        
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetGroupBy")]
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
