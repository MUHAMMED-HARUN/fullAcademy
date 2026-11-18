using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Finance.StudentInvoice.Command;
using System.Application.Services.Finance.StudentInvoice.Query;

namespace School_Academy.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentInvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentInvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GenerateInvoicesForEnrollment")]
        [Authorize(Policy = "Can_Create_StudentInvoice")]
        public async Task<IActionResult> GenerateInvoicesForEnrollment([FromBody] GenerateInvoicesForEnrollmentCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpPut("UpdateStudentInvoice")]
        [Authorize(Policy = "Can_Update_StudentInvoice")]
        public async Task<IActionResult> Update([FromBody] UpdateStudentInvoiceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpDelete("DeleteStudentInvoice")]
        [Authorize(Policy = "Can_Delete_StudentInvoice")]
        public async Task<IActionResult> Delete([FromBody] DeleteStudentInvoiceCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetStudentInvoiceListFilter")]
        [Authorize(Policy = "Can_Read_StudentInvoice")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllStudentInvoiceQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetStudentInvoiceBy")]
        [Authorize(Policy = "Can_Read_StudentInvoice")]
        public async Task<IActionResult> GetBy([FromQuery] GetStudentInvoiceByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetByAcademicEnrollment")]
        [Authorize(Policy = "Can_Read_StudentInvoice")]
        public async Task<IActionResult> GetByAcademicEnrollment([FromQuery] GetByAcademicEnrollmentQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("get-for-upsert/{StudentInvoiceID}")]
        [Authorize(Policy = "Can_Read_StudentInvoice")]
        public async Task<IActionResult> GetForUpsert([FromRoute] string StudentInvoiceID)
        {
            var result = await _mediator.Send(new GetStudentInvoiceForUpsertQuery
            {
                StudentInvoiceID = StudentInvoiceID,
            });
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetStudentFinanceSummary")]
        [Authorize(Policy = "Can_Read_StudentInvoice")]
        public async Task<IActionResult> GetStudentFinanceSummary([FromQuery] GetStudentFinanceSummaryQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }
    }
}
