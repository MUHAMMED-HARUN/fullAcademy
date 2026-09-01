using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Institute.Query;

namespace School_Academy.Controllers.Institute
{

    [Route("api/[controller]")]
    [ApiController]
    public class InstituteController : ControllerBase
    {

        private readonly IMediator _mediator;
        public InstituteController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("NewInstitute")]
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

        [HttpGet("GetInstituteBy")]
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
