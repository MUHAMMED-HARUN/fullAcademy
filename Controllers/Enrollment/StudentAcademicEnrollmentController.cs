using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Command;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Query;

namespace School_Academy.Controllers.Enrollment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public sealed class StudentAcademicEnrollmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentAcademicEnrollmentController(IMediator mediator) => _mediator = mediator;

        [HttpPost("EnrollStudentAcademicPlan")]
        [Authorize(Policy = "Can_Create_StudentAcademicEnrollment")]
        public async Task<IActionResult> EnrollStudentAcademicPlan([FromBody] EnrollStudentAcademicPlanCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetStudentAcademicEnrollmentListFilter")]
        [Authorize(Policy = "Can_Read_StudentAcademicEnrollment")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllStudentAcademicEnrollmentFilterQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetStudentAcademicEnrollmentBy")]
        [Authorize(Policy = "Can_Read_StudentAcademicEnrollment")]
        public async Task<IActionResult> GetBy([FromQuery] GetStudentAcademicEnrollmentByQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("get-for-upsert/{StudentAcademicEnrollmentID}")]
        [Authorize(Policy = "Can_Read_StudentAcademicEnrollment")]
        public async Task<IActionResult> GetForUpsert([FromRoute] string StudentAcademicEnrollmentID)
        {
            var result = await _mediator.Send(new GetStudentAcademicEnrollmentByQuery
            {
                StudentAcademicEnrollmentID = StudentAcademicEnrollmentID,
            });
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }
    }
}
