using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.InstituteClass.DTO;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    class IsExistInstituteClassByQueryHandler : IRequestHandler<IsExistInstituteClassByQuery, TResult<bool>>
    {
        IInstituteClassRepository _repo;
        ICurrentUserService _currentUser;
        public IsExistInstituteClassByQueryHandler(IInstituteClassRepository repo, ICurrentUserService currentUser)
        {
           _repo = repo;
           _currentUser = currentUser;
        }
        public async Task<TResult<bool>> Handle(IsExistInstituteClassByQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<bool>.Fail("غير مصرح بقراءة ربط الصفوف بالمعاهد");

            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType, readScope);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindProgram);

            return TResult<bool>.OK(isExist);

        }

        private InstituteClassReadScope? CreateReadScope()
        {
            if (!_currentUser.IsAuthenticated)
                return null;
            if (_currentUser.IsAdmin)
                return new InstituteClassReadScope { IsAdmin = true };
            if (!_currentUser.HasPermission("Can_Read_InstituteClass"))
                return null;
            if (string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
                return new InstituteClassReadScope { TeacherID = teacherID };
            if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
                return new InstituteClassReadScope { StudentID = studentID };
            return null;
        }
    }
}
