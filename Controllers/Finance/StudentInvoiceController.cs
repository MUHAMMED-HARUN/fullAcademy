using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Finance.StudentInvoice.Command;
using System.Application.Services.Finance.StudentInvoice.Query;

namespace School_Academy.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentInvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentInvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewStudentInvoice")]
        public async Task<IActionResult> Add([FromBody] NewStudentInvoiceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpPut("UpdateStudentInvoice")]
        public async Task<IActionResult> Update([FromBody] UpdateStudentInvoiceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpDelete("DeleteStudentInvoice")]
        public async Task<IActionResult> Delete([FromBody] DeleteStudentInvoiceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetStudentInvoiceList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllStudentInvoiceQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetStudentInvoiceBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetStudentInvoiceByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }
    }
}
