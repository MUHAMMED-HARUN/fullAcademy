using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.DailyEvaluation.Query;
using System.Application.Services.Progress.DailyEvaluation.Command;

namespace School_Academy.Controllers.Progress
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DailyEvaluationController : ControllerBase
    {

        private readonly IMediator _mediator;
        public DailyEvaluationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewDailyEvaluation")]
        [Authorize(Policy = "Can_Create_DailyEvaluation")]
        public async Task<IActionResult> Add([FromBody] NewDailyEvaluationCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateDailyEvaluation")]
        [Authorize(Policy = "Can_Update_DailyEvaluation")]
        public async Task<IActionResult> Update([FromBody] UpdateDailyEvaluationCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteDailyEvaluation")]
        [Authorize(Policy = "Can_Delete_DailyEvaluation")]
        public async Task<IActionResult> Delete([FromBody] DeleteDailyEvaluationCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetDailyEvaluationList")]
        [Authorize(Policy = "Can_Read_DailyEvaluation")]
        public async Task<IActionResult> GetList([FromQuery] GetAllDailyEvaluationQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetDailyEvaluationBy")]
        [Authorize(Policy = "Can_Read_DailyEvaluation")]
        public async Task<IActionResult> GetBy([FromQuery]GetDailyEvaluationByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("GetlastDailyEvaluation")]
        [Authorize(Policy = "Can_Read_DailyEvaluation")]

        public async Task<IActionResult> GetlastDailyEvaluation([FromQuery] GetlastDailyEvaluationQuery query)
        {

              var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistDailyEvaluationBy")]
        [HttpGet("IsExistDailyEvaluationeBy")]
        [Authorize(Policy = "Can_Read_DailyEvaluation")]
        public async Task<IActionResult> IsExistBy([FromQuery] IsExistDailyEvaluationByQuery query)
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
