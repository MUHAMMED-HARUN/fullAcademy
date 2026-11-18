using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Attendance.Attendance.Command;
using System.Application.Services.Attendance.Attendance.Query;

namespace School_Academy.Controllers.Attendance
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendancesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendancesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewAttendance")]
        // Batch upsert requires Create; the handler additionally checks Update when an existing row changes.
        [Authorize(Policy = "Can_Create_Attendance")]
        public async Task<IActionResult> Add([FromBody] NewAttendanceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetGroupAttendance")]
        [Authorize(Policy = "Can_Read_Attendance")]
        public async Task<IActionResult> GetGroupAttendance([FromQuery] GetGroupAttendanceQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

    }
}
