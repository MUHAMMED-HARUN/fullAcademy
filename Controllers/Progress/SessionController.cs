using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Progress.Session.Query;
using System.Application.Services.Progress.Session.Command;

namespace School_Academy.Controllers.Progress
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SessionController : ControllerBase
    {

        private readonly IMediator _mediator;
        public SessionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewSession")]
        [Authorize(Policy = "Can_Create_Session")]
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
        [Authorize(Policy = "Can_Update_Session")]
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
        [Authorize(Policy = "Can_Delete_Session")]
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
        [Authorize(Policy = "Can_Read_Session")]
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

        [HttpGet("GetSessionListFilter")]
        [Authorize(Policy = "Can_Read_Session")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllSessionFilterQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetSessionBy")]
        [Authorize(Policy = "Can_Read_Session")]
        public async Task<IActionResult> GetBy([FromQuery]GetSessionByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistSessionBy")]
        [Authorize(Policy = "Can_Read_Session")]
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
