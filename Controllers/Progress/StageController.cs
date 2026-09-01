using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.Stage.Query;
using System.Application.Services.Progress.Stage.Command;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    public class StageController : ControllerBase
    {

        private readonly IMediator _mediator;
        public StageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewStage")]
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

        [HttpGet("GetStageBy")]
        public async Task<IActionResult> GetBy([FromQuery]GetStageByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistStageeBy")]
        public async Task<IActionResult> IsExistBy(IsExistStageByQuery query)
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
