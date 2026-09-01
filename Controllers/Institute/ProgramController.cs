using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Institute.Program.Command;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {

        private readonly IMediator _mediator;
        public ProgramController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewProgram")]
        public async Task<IActionResult> Add([FromBody] NewProgramCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateProgram")]
        public async Task<IActionResult> Update([FromBody] UpdateProgramCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteProgram")]
        public async Task<IActionResult> Delete([FromBody] DeleteProgramCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetProgramList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllProgramQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetProgramBy")]
        public async Task<IActionResult> GetBy([FromQuery]GetProgramByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistProgrameBy")]
        public async Task<IActionResult> IsExistBy(IsExistProgramByQuery query)
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
