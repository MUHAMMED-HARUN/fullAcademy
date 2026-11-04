using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.DTO;
using System.Application.Services.Person.Person.DTO;

namespace System.Application.Services.Institute.Institute.Query
{
    class GetInstituteByQueryHandler : IRequestHandler<GetInstituteByQuery, TResult<InstituteInfo>>
    {
        IInstituteRepository _repo;
        ICurrentUserService _currentUser;
        public GetInstituteByQueryHandler(IInstituteRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }
        public async Task<TResult<InstituteInfo>> Handle(GetInstituteByQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<InstituteInfo>.Fail("غير مصرح بقراءة بيانات المعاهد");

            InstituteInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType, readScope);
            if (entityInfo == null) 
            return TResult<InstituteInfo>.Fail(ApplicationExMsg.failToFindProgram);

            return TResult<InstituteInfo>.OK(entityInfo);

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
