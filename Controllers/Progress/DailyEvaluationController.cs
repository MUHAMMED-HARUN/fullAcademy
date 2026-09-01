using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.DailyEvaluation.Query;
using System.Application.Services.Progress.DailyEvaluation.Command;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    public class DailyEvaluationController : ControllerBase
    {

        private readonly IMediator _mediator;
        public DailyEvaluationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewDailyEvaluation")]
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

        public async Task<IActionResult> GetlastDailyEvaluation([FromQuery] GetlastDailyEvaluationQuery query)
        {

              var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistDailyEvaluationeBy")]
        public async Task<IActionResult> IsExistBy(IsExistDailyEvaluationByQuery query)
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
