using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Institute.InstituteClass.Command;
using System.Application.Services.Institute.InstituteClass.Query;
using System.Application.Services.Institute.Program.Command;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InstituteClassController : ControllerBase
    {

        private readonly IMediator _mediator;
        public InstituteClassController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewInstituteClass")]
        [Authorize(Policy = "Can_Create_InstituteClass")]
        public async Task<IActionResult> Add([FromBody] NewInstituteClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPost("NewInstituteClassByRange")]
        [Authorize(Policy = "Can_Create_InstituteClass")]
        public async Task<IActionResult> AddByRange([FromBody] NewInstituteClassByRangeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateInstituteClass")]
        [Authorize(Policy = "Can_Update_InstituteClass")]
        public async Task<IActionResult> Update([FromBody] UpdateInstituteClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteInstituteClass")]
        [Authorize(Policy = "Can_Delete_InstituteClass")]
        public async Task<IActionResult> Delete([FromBody] DeleteInstituteClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetInstituteClassList")]
        [Authorize(Policy = "Can_Read_InstituteClass")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllInstituteClassQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetInstituteClassBy")]
        [Authorize(Policy = "Can_Read_InstituteClass")]
        public async Task<IActionResult> GetBy([FromQuery] GetInstituteClassByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistInstituteClassBy")]
        [Authorize(Policy = "Can_Read_InstituteClass")]
        public async Task<IActionResult> IsExistBy(IsExistInstituteClassByQuery query)
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
