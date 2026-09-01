using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.Session.Query;
using System.Application.Services.Progress.Session.Command;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        private readonly IMediator _mediator;
        public SessionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewSession")]
        public async Task<IActionResult> Add([FromBody] NewSessionCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateSession")]
        public async Task<IActionResult> Update([FromBody] UpdateSessionCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteSession")]
        public async Task<IActionResult> Delete([FromBody] DeleteSessionCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetSessionList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllSessionQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetSessionBy")]
        public async Task<IActionResult> GetBy([FromQuery]GetSessionByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistSessioneBy")]
        public async Task<IActionResult> IsExistBy(IsExistSessionByQuery query)
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
