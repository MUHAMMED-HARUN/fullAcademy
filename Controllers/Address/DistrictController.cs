using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Address.District.Command;
using System.Application.Services.Address.District.Query;
using System.Domain.Enums.Address;

namespace School_Academy.Controllers.Address
{

    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {

        private readonly IMediator _mediator;
        public DistrictController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewDistrict")]
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
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllDistrictQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetDistrictListByCityID/{cityId}")]
        public async Task<IActionResult> GetListByCityId(string cityId)
        {
            

            return Ok();
        }

        [HttpGet("GetDistrictByID/{param}")]
        public async Task<IActionResult> GetById(string param, [FromQuery] enDistrictParamType paramType = enDistrictParamType.ID)
        {
            return Ok();
        }

    }
}
