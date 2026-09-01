using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.Neighborhood.Command;
using System.Application.Services.Address.Neighborhood.Query;
using System.Domain.Enums.Address;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    public class NeighborhoodController : ControllerBase
    {

        private readonly IMediator _mediator;
        public NeighborhoodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewNeighborhood")]
        public async Task<IActionResult> Add([FromBody] NewNeighborhoodCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateNeighborhood")]
        public async Task<IActionResult> Update([FromBody] UpdateNeighborhoodCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetNeighborhoodList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllNeighborhoodQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetNeighborhoodByID/{param}")]
        public async Task<IActionResult> GetById(string param, [FromQuery] enNeighborhoodParamType paramType = enNeighborhoodParamType.ID)
        {
            
            return Ok();
        }

    }
}
