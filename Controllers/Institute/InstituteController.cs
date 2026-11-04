using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Institute.Query;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InstituteController : ControllerBase
    {

        private readonly IMediator _mediator;
        public InstituteController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewInstitute")]
        [Authorize(Policy = "Can_Create_Institute")]
        public async Task<IActionResult> Add([FromBody] NewInstituteCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpPut("UpdateInstitute")]
        [Authorize(Policy = "Can_Update_Institute")]
        public async Task<IActionResult> Update([FromBody] UpdateInstituteCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteInstitute")]
        [Authorize(Policy = "Can_Delete_Institute")]
        public async Task<IActionResult> Delete([FromBody] DeleteInstituteCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetInstituteList")]
        [Authorize(Policy = "Can_Read_Institute")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllInstituteQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }

        [HttpGet("GetInstituteListFilter")]
        [Authorize(Policy = "Can_Read_Institute")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetInstituteListFilterQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetInstituteBy")]
        [Authorize(Policy = "Can_Read_Institute")]
        public async Task<IActionResult> GetBy([FromQuery] GetInstituteByQuery query )
        {
                   
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("IsExistInstituteBy")]
        [Authorize(Policy = "Can_Read_Institute")]
        public async Task<IActionResult> IsExistBy(IsExistInstituteByQuery query)
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
