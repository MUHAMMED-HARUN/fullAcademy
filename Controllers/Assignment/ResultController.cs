using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Assignment.Result.Command;
using System.Application.Services.Assignment.Result.Query;

namespace School_Academy.Controllers.Assignment
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultController : ControllerBase
    {

        private readonly IMediator _mediator;
        public ResultController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewResult")]
        [Authorize(Policy = "Can_Create_Result")]
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
        [Authorize(Policy = "Can_Update_Result")]
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
        [Authorize(Policy = "Can_Delete_Result")]
        public async Task<IActionResult> Delete([FromBody] DeleteResultCommand command)
        {
                var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetResultListFilter")]
        [Authorize(Policy = "Can_Read_Result")]
        public async Task<IActionResult> GetList([FromQuery] GetAllResultQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetResultForUpsert")]
        [Authorize(Policy = "Can_Read_Result")]
        public async Task<IActionResult> GetForUpsert([FromQuery] GetResultForUpsertQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetResultBy")]
        [Authorize(Policy = "Can_Read_Result")]
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
