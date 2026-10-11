using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Assignment.Result.Command;
using System.Application.Services.Assignment.Result.Query;

namespace School_Academy.Controllers.Assignment
{

    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {

        private readonly IMediator _mediator;
        public ResultController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewResult")]
        public async Task<IActionResult> Add([FromBody] NewResultCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateResult")]
        public async Task<IActionResult> Update([FromBody] UpdateResultCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteResult")]
        public async Task<IActionResult> Delete([FromBody] DeleteResultCommand command)
        {
                var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetResultList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllResultQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetResultBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetResultByQuery query)
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
