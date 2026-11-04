using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Finance.FeeStructure.Command;
using System.Application.Services.Finance.FeeStructure.Query;

namespace School_Academy.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeStructureController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FeeStructureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewFeeStructure")]
        public async Task<IActionResult> Add([FromBody] NewFeeStructureCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpPut("UpdateFeeStructure")]
        public async Task<IActionResult> Update([FromBody] UpdateFeeStructureCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpDelete("DeleteFeeStructure")]
        public async Task<IActionResult> Delete([FromBody] DeleteFeeStructureCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetFeeStructureList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllFeeStructureQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetFeeStructureBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetFeeStructureByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }
    }
}
