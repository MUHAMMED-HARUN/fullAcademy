using FluentValidation;
using System.Application.Services.Authentication.RolePermission.Qurery;

namespace System.Application.Services.Authentication.RolePermission.Validations
{
    public sealed class GetRolePermissionQueryValidator : AbstractValidator<GetRolePermissionQuery>
    {
        public GetRolePermissionQueryValidator()
        {
            RuleFor(query => query.RoleID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("معرّف الدور غير صالح");
        }
    }
}
