using FluentValidation;
using System.Application.Services.Institute.Institute.Query;

namespace System.Application.Services.Institute.Institute.Validations
{
    public sealed class GetInstituteListFilterQueryValidator
        : AbstractValidator<GetInstituteListFilterQuery>
    {
        public GetInstituteListFilterQueryValidator()
        {
            RuleFor(query => query.Search)
                .NotEmpty()
                .MaximumLength(100)
                .Must(BeSafeSearchText)
                .WithMessage("قيمة البحث غير صالحة");
        }

        private static bool BeSafeSearchText(string? value) =>
            !string.IsNullOrWhiteSpace(value) &&
            !value.Any(character => ",|=*^$!()/".Contains(character));
    }
}
