using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.District.Command;
using System.Application.Services.Address.District.Query;
using System.Domain.Enums.Address;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DistrictController : ControllerBase
    {

        private readonly IMediator _mediator;
        public DistrictController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewDistrict")]
        [Authorize(Policy = "Can_Create_District")]
        public async Task<IActionResult> Add([FromBody] NewDistrictCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateDistrict")]
        [Authorize(Policy = "Can_Update_District")]
        public async Task<IActionResult> Update([FromBody] UpdateDistrictCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetDistrictList")]
        [Authorize(Policy = "Can_Read_District")]
        public async Task<IActionResult> GetList([FromQuery] GetAllDistrictQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetDistrictOptions")]
        [Authorize(Policy = "Can_Read_District")]
        public async Task<IActionResult> GetOptions()
        {
            var result = await _mediator.Send(new GetDistrictOptionsQuery());
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetDistrictListByCityID/{cityId}")]
        [Authorize(Policy = "Can_Read_District")]
        public async Task<IActionResult> GetListByCityId(
            [FromRoute] Guid cityId,
            [FromQuery] GetAllDistrictQuery query)
        {
            query.CityID = cityId;
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetDistrictByID/{param}")]
        [Authorize(Policy = "Can_Read_District")]
        public async Task<IActionResult> GetById(string param, [FromQuery] enDistrictParamType paramType = enDistrictParamType.ID)
        {
            var result = await _mediator.Send(new GetDistrictByQuery
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
