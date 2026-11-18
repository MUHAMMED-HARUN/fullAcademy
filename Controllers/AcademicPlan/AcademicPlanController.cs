using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.AcademicPlan.AcademicPlan.Command;
using System.Application.Services.AcademicPlan.AcademicPlan.Query;

namespace School_Academy.Controllers.AcademicPlan
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AcademicPlanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AcademicPlanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("NewAcademicPlan")]
        [Authorize(Policy = "Can_Create_AcademicPlan")]
        public async Task<IActionResult> Add([FromBody] NewAcademicPlanCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpPut("UpdateAcademicPlan")]
        [Authorize(Policy = "Can_Update_AcademicPlan")]
        public async Task<IActionResult> Update([FromBody] UpdateAcademicPlanCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpPut("ActivateAcademicPlan")]
        [Authorize(Policy = "Can_Update_AcademicPlan")]
        public async Task<IActionResult> Activate([FromBody] ActivateAcademicPlanCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpDelete("DeleteAcademicPlan")]
        [Authorize(Policy = "Can_Delete_AcademicPlan")]
        public async Task<IActionResult> Delete([FromBody] DeleteAcademicPlanCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetAcademicPlanListFilter")]
        [Authorize(Policy = "Can_Read_AcademicPlan")]
        public async Task<IActionResult> GetListFilter([FromQuery] GetAllAcademicPlanFilterQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("GetAcademicPlanBy")]
        [Authorize(Policy = "Can_Read_AcademicPlan")]
        public async Task<IActionResult> GetBy([FromQuery] GetAcademicPlanByQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }

        [HttpGet("get-for-upsert/{AcademicPlanID}")]
        [Authorize(Policy = "Can_Read_AcademicPlan")]
        public async Task<IActionResult> GetForUpsert([FromRoute] string AcademicPlanID)
        {
            var result = await _mediator.Send(new GetAcademicPlanByQuery { AcademicPlanID = AcademicPlanID });

            if (!result.IsSuccess)
                return BadRequest(new { errors = result.Error });

            return Ok(result);
        }
    }
}
