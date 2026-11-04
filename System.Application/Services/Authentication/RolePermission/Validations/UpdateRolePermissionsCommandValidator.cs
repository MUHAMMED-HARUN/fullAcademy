using FluentValidation;
using System.Application.Services.Authentication.RolePermission.Command;
using System.Domain.Entities.Permissions;

namespace System.Application.Services.Authentication.RolePermission.Validations
{
    public sealed class RolePermissionChangeDtoValidator : AbstractValidator<RolePermissionChangeDto>
    {
        public RolePermissionChangeDtoValidator()
        {
            RuleFor(change => change.RoleId)
                .Must(BeGuid).WithMessage("معرّف الدور غير صالح");
            RuleFor(change => change.PermissionName)
                .Must(permissionName => clsPermission.AllPermissions()
                    .Any(permission => permission.Name == permissionName))
                .WithMessage("الصلاحية غير معروفة");
        }

        private static bool BeGuid(string? value) =>
            Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty;
    }

    public sealed class UpdateRolePermissionsCommandValidator
        : AbstractValidator<UpdateRolePermissionsCommand>
    {
        public UpdateRolePermissionsCommandValidator()
        {
            RuleFor(command => command.Changes)
                .NotNull().NotEmpty()
                .WithMessage("يجب إرسال صلاحية واحدة على الأقل للتعديل");
            RuleForEach(command => command.Changes)
                .SetValidator(new RolePermissionChangeDtoValidator());
        }
    }
}
