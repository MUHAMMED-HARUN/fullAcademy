using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Class.Command;
using System.Application.Services.Institute.Class.Query;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Institute.Query;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {

        private readonly IMediator _mediator;
        public ClassController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewClass")]
        public async Task<IActionResult> Add([FromBody] NewClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateClass")]
        public async Task<IActionResult> Update([FromBody] UpdateClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteClass")]
        public async Task<IActionResult> Delete([FromBody] DeleteClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetClassList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllClassQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetClassBy")]
        public async Task<IActionResult> GetBy(GetClassByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistClassBy")]
        public async Task<IActionResult> IsExistBy(IsExistClassByQuery query)
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
