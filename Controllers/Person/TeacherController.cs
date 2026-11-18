using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Person.Teacher.Command;
using System.Application.Services.Person.Teacher.Query;

namespace School_Academy.Controllers.Person
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {

        private readonly IMediator _mediator;
        public TeacherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewTeacher")]
        [Authorize(Policy = "Can_Create_Teacher")]
        public async Task<IActionResult> Add([FromBody] NewTeacherCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateTeacher")]
        [Authorize(Policy = "Can_Update_Teacher")]
        public async Task<IActionResult> Update([FromBody] UpdateTeacherCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteTeacher")]
        [Authorize(Policy = "Can_Delete_Teacher")]
        public async Task<IActionResult> Delete([FromBody] DeleteTeacherCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetTeacherList")]
        [Authorize(Policy = "Can_Read_Teacher")]
        public async Task<IActionResult> GetList([FromQuery] GetAllTeacherQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetTeacherBy")]
        [Authorize(Policy = "Can_Read_Teacher")]
        public async Task<IActionResult> GetBy( [FromQuery] GetTeacherByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistTeacherBy")]
        [Authorize(Policy = "Can_Read_Teacher")]
        public async Task<IActionResult> IsExistBy([FromQuery] IsExistTeacherByQuery query)
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
