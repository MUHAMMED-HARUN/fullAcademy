using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Attendance.Attendance.Command;

namespace School_Academy.Controllers.Attendance
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewAttendance")]
        public async Task<IActionResult> Add([FromBody] NewAttendanceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

    }
}
