using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Finance.Payment.Command;
using System.Application.Services.Finance.Payment.Query;

namespace School_Academy.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewPayment")]
        public async Task<IActionResult> Add([FromBody] NewPaymentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpPut("UpdatePayment")]
        public async Task<IActionResult> Update([FromBody] UpdatePaymentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpDelete("DeletePayment")]
        public async Task<IActionResult> Delete([FromBody] DeletePaymentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetPaymentList")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllPaymentQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetPaymentBy")]
        public async Task<IActionResult> GetBy([FromQuery] GetPaymentByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }
    }
}
