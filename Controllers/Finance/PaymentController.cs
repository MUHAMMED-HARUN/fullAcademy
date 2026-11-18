using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Finance.Payment.Command;
using System.Application.Services.Finance.Payment.Query;

namespace School_Academy.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewPayment")]
        [Authorize(Policy = "Can_Record_Payment")]
        public async Task<IActionResult> Add([FromBody] NewPaymentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpPost("ReversePayment")]
        [Authorize(Policy = "Can_Reverse_Payment")]
        public async Task<IActionResult> Reverse([FromBody] ReversePaymentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetPaymentListFilter")]
        [Authorize(Policy = "Can_Read_Payment")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllPaymentQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetPaymentsByInvoice/{StudentInvoiceID}")]
        [Authorize(Policy = "Can_Read_Payment")]
        public async Task<IActionResult> GetPaymentsByInvoice([FromRoute] string StudentInvoiceID)
        {
            var result = await _mediator.Send(new GetPaymentsByInvoiceQuery { StudentInvoiceID = StudentInvoiceID });
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetPaymentBy")]
        [Authorize(Policy = "Can_Read_Payment")]
        public async Task<IActionResult> GetBy([FromQuery] GetPaymentByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }
    }
}
