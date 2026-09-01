using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Enrollment.StudentEnrollment.Command;
using System.Application.Services.Enrollment.StudentEnrollment.Query;

namespace School_Academy.Controllers.Enrollment
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentEnrollmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentEnrollmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewStudentEnrollment")]
        public async Task<IActionResult> Add([FromBody] NewStudentEnrollmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateStudentEnrollment")]
        public async Task<IActionResult> Update([FromBody] UpdateStudentEnrollmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteStudentEnrollment")]
        public async Task<IActionResult> Delete([FromBody] DeleteStudentEnrollmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStudentEnrollmentList")]
        public async Task<IActionResult> GetList([FromQuery] GetAllStudentEnrollmentQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetStudentEnrollmentBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetStudentEnrollmentByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("IsExistStudentEnrollmentBy")]
        public async Task<IActionResult> IsExistBy([FromQuery] IsExistStudentEnrollmentByQuery query)
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
