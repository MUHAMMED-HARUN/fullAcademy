using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Institute.InstituteClass.Command;
using System.Application.Services.Institute.InstituteClass.Query;
using System.Application.Services.Institute.Program.Command;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    public class InstituteClassController : ControllerBase
    {

        private readonly IMediator _mediator;
        public InstituteClassController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewInstituteClass")]
        public async Task<IActionResult> Add([FromBody] NewInstituteClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateInstituteClass")]
        public async Task<IActionResult> Update([FromBody] UpdateInstituteClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteInstituteClass")]
        public async Task<IActionResult> Delete([FromBody] DeleteInstituteClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetInstituteClassList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllInstituteClassQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetInstituteClassBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetInstituteClassByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistInstituteClassBy")]
        public async Task<IActionResult> IsExistBy(IsExistInstituteClassByQuery query)
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
