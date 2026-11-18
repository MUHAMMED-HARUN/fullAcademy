using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Finance.FeeStructure.Command;
using System.Application.Services.Finance.FeeStructure.Query;

namespace School_Academy.Controllers.Finance
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeeStructureController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FeeStructureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewFeeStructure")]
        [Authorize(Policy = "Can_Create_FeeStructure")]
        public async Task<IActionResult> Add([FromBody] NewFeeStructureCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpPut("UpdateFeeStructure")]
        [Authorize(Policy = "Can_Update_FeeStructure")]
        public async Task<IActionResult> Update([FromBody] UpdateFeeStructureCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpDelete("DeleteFeeStructure")]
        [Authorize(Policy = "Can_Delete_FeeStructure")]
        public async Task<IActionResult> Delete([FromBody] DeleteFeeStructureCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetFeeStructureList")]
        [Authorize(Policy = "Can_Read_FeeStructure")]
        public async Task<IActionResult> GetList()
        {
            var query = new GetAllFeeStructureQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetFeeStructureListFilter")]
        [Authorize(Policy = "Can_Read_FeeStructure")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllFeeStructureFilterQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetFeeStructuresByAcademicPlan")]
        [Authorize(Policy = "Can_Read_FeeStructure")]
        public async Task<IActionResult> GetByAcademicPlan([FromQuery] string AcademicPlanID)
        {
            var result = await _mediator.Send(new GetFeeStructureByAcademicPlanQuery { AcademicPlanID = AcademicPlanID });
            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });
            return Ok(result);
        }

        [HttpGet("GetFeeStructureBy")]
        [Authorize(Policy = "Can_Read_FeeStructure")]
        public async Task<IActionResult> GetBy([FromQuery] GetFeeStructureByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }
    }
}
