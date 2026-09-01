using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Person.Teacher.Command;
using System.Application.Services.Person.Teacher.Query;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {

        private readonly IMediator _mediator;
        public TeacherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewTeacher")]
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
        public async Task<IActionResult> IsExistBy(IsExistTeacherByQuery query)
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
