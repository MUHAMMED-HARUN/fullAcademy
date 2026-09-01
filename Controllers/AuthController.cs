using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Application.Services.Authentication.Login.Command;
using System.Application.Services.Authentication.Permission.Query;
using System.Application.Services.Authentication.Register.Command;
using System.Application.Services.Authentication.Role.Qurery;
using System.Application.Services.Authentication.RolePermission.Command;
using System.Application.Services.Authentication.RolePermission.Qurery;

namespace School_Academy.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("GetPermissions")]
        public async Task<IActionResult> Permissions()
        {
            GetAllPermission query = new GetAllPermission();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);
        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetRoles()
        {
            GetRolesQuery query = new GetRolesQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);

        }
        [HttpGet("GetAllRolePermissions")]
        public async Task<IActionResult> GetRoles([FromQuery] string RoleId)
        {
            GetRolePermissionQuery query = new GetRolePermissionQuery();
            query.RoleID = RoleId;

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);

        }
        [HttpPost("UpdateRolePermissions")]
        //[Authorize(Policy = "Can_SetPermission_User")]
        public async Task<IActionResult> UpdateRolePermissions( UpdateRolePermissionsCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { errors = result.Error });
            }

            return Ok(result);

        }
    }
}
