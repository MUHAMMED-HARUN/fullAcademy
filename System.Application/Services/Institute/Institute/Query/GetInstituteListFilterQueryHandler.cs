using Gridify;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.DTO;

namespace System.Application.Services.Institute.Institute.Query
{
    public sealed class GetInstituteListFilterQueryHandler
        : IRequestHandler<GetInstituteListFilterQuery, TResult<Paging<InstituteLookupInfo>>>
    {
        private readonly IInstituteRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public GetInstituteListFilterQueryHandler(
            IInstituteRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<TResult<Paging<InstituteLookupInfo>>> Handle(
            GetInstituteListFilterQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return TResult<Paging<InstituteLookupInfo>>.Fail("يجب تسجيل الدخول للبحث عن المعاهد");

            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<Paging<InstituteLookupInfo>>.Fail("غير مصرح بقراءة بيانات المعاهد");

            var search = request.Search.Trim();
            if (search.Length == 0 || search.Length > 100 || ContainsGridifyTokens(search))
                return TResult<Paging<InstituteLookupInfo>>.Fail("قيمة البحث غير صالحة");

            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize <= 0 ? 5 : request.PageSize, 1, 5);
            request.Filter = $"Name=*{search}";
            request.OrderBy = nameof(InstituteLookupInfo.Name);

            return TResult<Paging<InstituteLookupInfo>>.OK(
                await _repository.GetListFilter(request, readScope));
        }

        private InstituteReadScope? CreateReadScope()
        {
            if (_currentUser.IsAdmin)
                return new InstituteReadScope { IsAdmin = true };

            if (!_currentUser.HasPermission("Can_Read_Institute"))
                return null;

            if (string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
            {
                return new InstituteReadScope { TeacherID = teacherID };
            }

            if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return new InstituteReadScope { StudentID = studentID };
            }

            return null;
        }

        private static bool ContainsGridifyTokens(string value) =>
            value.IndexOfAny([',', '|', '=', '*', '^', '$', '!', '(', ')', '/']) >= 0;
    }
}
