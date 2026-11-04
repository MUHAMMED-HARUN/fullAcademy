using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.DTO;

namespace System.Application.Services.Institute.Institute.Query
{
    class IsExistInstituteByQueryHandler : IRequestHandler<IsExistInstituteByQuery, TResult<bool>>
    {
        IInstituteRepository _repo;
        ICurrentUserService _currentUser;
        public IsExistInstituteByQueryHandler(IInstituteRepository repo, ICurrentUserService currentUser)
        {
           _repo = repo;
           _currentUser = currentUser;
        }
        public async Task<TResult<bool>> Handle(IsExistInstituteByQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<bool>.Fail("غير مصرح بقراءة بيانات المعاهد");

            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType, readScope);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindProgram);

            return TResult<bool>.OK(isExist);

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
