
using FluentValidation;
using System.Application.Services.Authentication.Register.Command;
using System.Domain.Enums.Person;

namespace System.Application.Services.Authentication.Register.Validations
{
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().EmailAddress()
                .WithMessage("البريد الإلكتروني غير صالح");
            RuleFor(command => command.UserName)
                .NotEmpty().WithMessage("اسم المستخدم مطلوب")
                .MinimumLength(4).WithMessage("اسم المستخدم يجب ألا يقل عن 4 أحرف")
                .MaximumLength(256).WithMessage("اسم المستخدم طويل جداً");
            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة")
                .MinimumLength(8).WithMessage("كلمة المرور يجب ألا تقل عن 8 أحرف")
                .Matches("[A-Z]").WithMessage("كلمة المرور يجب أن تحتوي حرفاً كبيراً")
                .Matches("[a-z]").WithMessage("كلمة المرور يجب أن تحتوي حرفاً صغيراً")
                .Matches("[0-9]").WithMessage("كلمة المرور يجب أن تحتوي رقماً")
                .Matches("[^a-zA-Z0-9]").WithMessage("كلمة المرور يجب أن تحتوي رمزاً خاصاً");
            RuleFor(command => command.RoleID)
                .Must(BeGuid).WithMessage("معرّف الدور غير صالح");
            RuleFor(command => command.UserType)
                .IsInEnum().WithMessage("نوع المستخدم غير صالح");
            RuleFor(command => command.EntityID)
                .Must(BeGuid).WithMessage("معرّف الكيان غير صالح");
        }

        private static bool BeGuid(string? value) =>
            Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty;
    }
}
