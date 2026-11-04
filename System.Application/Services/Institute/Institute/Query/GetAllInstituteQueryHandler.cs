using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.DTO;
using System.Application.Services.Institute.Institute.Query;

namespace System.Application.Services.Institute.Institute.Query
{
    class GetAllInstituteQueryHandler : IRequestHandler<GetAllInstituteQuery, TResult<List<InstituteInfo>>>
    {
        IInstituteRepository _repo;
        ICurrentUserService _currentUser;
        public GetAllInstituteQueryHandler(IInstituteRepository repo, ICurrentUserService currentUser)
        {
            _repo= repo;
            _currentUser = currentUser;
        }
        public async Task<TResult<List<InstituteInfo>>> Handle(GetAllInstituteQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<List<InstituteInfo>>.Fail("غير مصرح بقراءة بيانات المعاهد");

            return TResult<List<InstituteInfo>>.OK(await _repo.GetList(readScope));
        }

        private InstituteReadScope? CreateReadScope()
        {
            if (!_currentUser.IsAuthenticated)
                return null;

            if (_currentUser.IsAdmin)
                return new InstituteReadScope { IsAdmin = true };

            if (!_currentUser.HasPermission("Can_Read_Institute"))
                return null;

            if (string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid teacherId && teacherId != Guid.Empty)
            {
                return new InstituteReadScope { TeacherID = teacherId };
            }

            if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid studentId && studentId != Guid.Empty)
            {
                return new InstituteReadScope { StudentID = studentId };
            }

            return null;
        }
    }
}
