using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.Stage.Query;
using System.Application.Services.Progress.Stage.Command;

namespace School_Academy.Controllers.Progress
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StageController : ControllerBase
    {

        private readonly IMediator _mediator;
        public StageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewStage")]
        [Authorize(Policy = "Can_Create_Stage")]
        public async Task<IActionResult> Add([FromBody] NewStageCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateStage")]
        [Authorize(Policy = "Can_Update_Stage")]
        public async Task<IActionResult> Update([FromBody] UpdateStageCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteStage")]
        [Authorize(Policy = "Can_Delete_Stage")]
        public async Task<IActionResult> Delete([FromBody] DeleteStageCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStageList")]
        [Authorize(Policy = "Can_Read_Stage")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllStageQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStageListFilter")]
        [Authorize(Policy = "Can_Read_Stage")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllStageFilterQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetStageBy")]
        [Authorize(Policy = "Can_Read_Stage")]
        public async Task<IActionResult> GetBy([FromQuery]GetStageByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistStageBy")]
        [Authorize(Policy = "Can_Read_Stage")]
        public async Task<IActionResult> IsExistBy([FromQuery] IsExistStageByQuery query)
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
