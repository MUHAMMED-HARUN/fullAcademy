
using FluentValidation;
using System.Application.Services.Authentication.Login.Command;
using System.Application.Services.Authentication.RolePermission.Qurery;

namespace System.Application.Services.Authentication.Login.Validator
{
    public class GetRolePermissionQueryValidator : AbstractValidator<GetRolePermissionQuery>
    {

        public GetRolePermissionQueryValidator()
        {
            RuleFor(x => x.RoleID).NotEmpty().NotNull();
        }
    }
}
