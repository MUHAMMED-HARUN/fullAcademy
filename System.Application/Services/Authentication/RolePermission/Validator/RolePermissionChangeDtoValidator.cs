
using FluentValidation;
using System.Application.Services.Authentication.Login.Command;
using System.Application.Services.Authentication.RolePermission.Command;
using System.Application.Services.Authentication.RolePermission.Qurery;

namespace System.Application.Services.Authentication.Login.Validator
{
    public class RolePermissionChangeDtoValidator : AbstractValidator<RolePermissionChangeDto>
    {
        public RolePermissionChangeDtoValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("معرف الدور (RoleId) مطلوب ولا يمكن أن يكون فارغاً.");

            RuleFor(x => x.PermissionName)
                .NotEmpty().WithMessage("اسم الصلاحية (PermissionName) مطلوب ولا يمكن أن يكون فارغاً.");
        }
    }
    public class UpdateRolePermissionsCommandValidator : AbstractValidator<UpdateRolePermissionsCommand>
    {
        public UpdateRolePermissionsCommandValidator()
        {
            // 1. فحص القائمة الرئيسية
            RuleFor(x => x.Changes)
                .NotNull().WithMessage("قائمة التغييرات لا يمكن أن تكون null.")
                .NotEmpty().WithMessage("يجب إرسال عنصر واحد على الأقل داخل قائمة التغييرات.");

            // 2. تطبيق الفحص على كل عنصر محتوى داخل القائمة
            RuleForEach(x => x.Changes)
                .SetValidator(new RolePermissionChangeDtoValidator());
        }
    }
}
