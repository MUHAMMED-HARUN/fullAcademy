using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.StudentStage.Query;
using System.Application.Services.Progress.StudentStage.Command;

namespace School_Academy.Controllers.Progress
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentStageController : ControllerBase
    {

        private readonly IMediator _mediator;
        public StudentStageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewStudentStage")]
        public async Task<IActionResult> Add([FromBody] NewStudentStageCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateStudentStage")]
        public async Task<IActionResult> Update([FromBody] UpdateStudentStageCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteStudentStage")]
        public async Task<IActionResult> Delete([FromBody] DeleteStudentStageCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStudentStageList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllStudentStageQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStudentStageBy")]
        public async Task<IActionResult> GetBy([FromQuery]GetStudentStageByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistStudentStageeBy")]
        public async Task<IActionResult> IsExistBy(IsExistStudentStageByQuery query)
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
