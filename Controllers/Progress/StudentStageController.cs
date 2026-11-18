using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.StudentStage.Query;
using System.Application.Services.Progress.StudentStage.Command;

namespace School_Academy.Controllers.Progress
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentStageController : ControllerBase
    {

        private readonly IMediator _mediator;
        public StudentStageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewStudentStage")]
        [Authorize(Policy = "Can_Create_StudentStage")]
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
        [Authorize(Policy = "Can_Update_StudentStage")]
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
        [Authorize(Policy = "Can_Delete_StudentStage")]
        public async Task<IActionResult> Delete([FromBody] DeleteStudentStageCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStudentStageListFilter")]
        [Authorize(Policy = "Can_Read_StudentStage")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllStudentStageQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStudentStageBy")]
        [Authorize(Policy = "Can_Read_StudentStage")]
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
        [Authorize(Policy = "Can_Read_StudentStage")]
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
