using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.StageInfo.Query;
using System.Application.Services.Progress.StageInfo.Command;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    public class StageInfoController : ControllerBase
    {

        private readonly IMediator _mediator;
        public StageInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewStageInfo")]
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

        [HttpGet("GetStageInfoBy")]
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
        public async Task<IActionResult> GetStudentStageInfo([FromQuery] GetStudentStageInfoQuery query)
        {

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistStageInfoeBy")]
        public async Task<IActionResult> IsExistBy(IsExistStageInfoByQuery query)
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
