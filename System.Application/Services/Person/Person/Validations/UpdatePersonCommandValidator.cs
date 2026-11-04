using FluentValidation;
using System.Application.Services.Address.Address.Validations;
using System.Application.Services.Person.Person.Command;

namespace System.Application.Services.Person.Person.Validations
{
    public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidator()
        {
            RuleFor(x => x.PersonID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("Person ID must be a valid, non-empty ID");
            RuleFor(x => x.FirstName).Must(HasText).WithMessage("الاسم الأول مطلوب").MaximumLength(60).WithMessage("الاسم الأول طويل جداً");
            RuleFor(x => x.FatherName).Must(HasText).WithMessage("اسم الأب مطلوب").MaximumLength(60).WithMessage("اسم الأب طويل جداً");
            RuleFor(x => x.LastName).Must(HasText).WithMessage("الكنية مطلوبة").MaximumLength(60).WithMessage("الكنية طويلة جداً");
            RuleFor(x => x.MotherName).Must(HasText).WithMessage("اسم الأم مطلوبة").MaximumLength(60).WithMessage("اسم الأم طويلة جداً");
            RuleFor(x => x.MotherLastName).Must(HasText).WithMessage("كنية الأم مطلوبة").MaximumLength(60).WithMessage("كنية الأم طويلة جداً");
            RuleFor(x => x.NationalNumber).Must(HasText).WithMessage("الرقم الوطني مطلوب");
            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("تاريخ الولادة مطلوب")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("تاريخ الولادة لا يمكن أن يكون في المستقبل");
            RuleFor(x => x.PlaceOfBirthID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("Place of birth must be a valid, non-empty ID");
            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new UpdateAddressCommandValidator());
        }

        private static bool HasText(string? value) => !string.IsNullOrWhiteSpace(value);
    }
}
