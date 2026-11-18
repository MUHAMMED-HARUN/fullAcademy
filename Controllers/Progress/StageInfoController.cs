using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.StageInfo.Query;
using System.Application.Services.Progress.StageInfo.Command;

namespace School_Academy.Controllers.Progress
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StageInfoController : ControllerBase
    {

        private readonly IMediator _mediator;
        public StageInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewStageInfo")]
        [Authorize(Policy = "Can_Create_StageInfo")]
        public async Task<IActionResult> Add([FromBody] NewStageInfoCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateStageInfo")]
        [Authorize(Policy = "Can_Update_StageInfo")]
        public async Task<IActionResult> Update([FromBody] UpdateStageInfoCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteStageInfo")]
        [Authorize(Policy = "Can_Delete_StageInfo")]
        public async Task<IActionResult> Delete([FromBody] DeleteStageInfoCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStageInfoList")]
        [Authorize(Policy = "Can_Read_StageInfo")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllStageInfoQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStageInfoListFilter")]
        [Authorize(Policy = "Can_Read_StageInfo")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllStageInfoFilterQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetStageInfoBy")]
        [Authorize(Policy = "Can_Read_StageInfo")]
        public async Task<IActionResult> GetBy([FromQuery]GetStageInfoByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStudentStageInfo")]
        [Authorize(Policy = "Can_Read_StageInfo")]
        public async Task<IActionResult> GetStudentStageInfo([FromQuery] GetStudentStageInfoQuery query)
        {

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistStageInfoBy")]
        [Authorize(Policy = "Can_Read_StageInfo")]
        public async Task<IActionResult> IsExistBy([FromQuery] IsExistStageInfoByQuery query)
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
