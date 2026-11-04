using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.Neighborhood.Command;
using System.Application.Services.Address.Neighborhood.Query;
using System.Domain.Enums.Address;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NeighborhoodController : ControllerBase
    {

        private readonly IMediator _mediator;
        public NeighborhoodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewNeighborhood")]
        [Authorize(Policy = "Can_Create_Neighborhood")]
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
        [Authorize(Policy = "Can_Update_Neighborhood")]
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
        [Authorize(Policy = "Can_Read_Neighborhood")]
        public async Task<IActionResult> GetList([FromQuery] GetAllNeighborhoodQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetNeighborhoodOptions")]
        [Authorize(Policy = "Can_Read_Neighborhood")]
        public async Task<IActionResult> GetOptions()
        {
            var result = await _mediator.Send(new GetNeighborhoodOptionsQuery());
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetNeighborhoodListByDistrictID/{districtId}")]
        [Authorize(Policy = "Can_Read_Neighborhood")]
        public async Task<IActionResult> GetListByDistrictId(
            [FromRoute] Guid districtId,
            [FromQuery] GetAllNeighborhoodQuery query)
        {
            query.DistrictID = districtId;
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetNeighborhoodByID/{param}")]
        [Authorize(Policy = "Can_Read_Neighborhood")]
        public async Task<IActionResult> GetById(string param, [FromQuery] enNeighborhoodParamType paramType = enNeighborhoodParamType.ID)
        {
            var result = await _mediator.Send(new GetNeighborhoodByQuery
            {
                Param = param,
                ParamType = paramType,
            });
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

    }
}
