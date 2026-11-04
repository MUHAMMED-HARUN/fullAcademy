using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.InstituteClass.DTO;
using System.Application.Services.Institute.Program.DTO;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    class GetInstituteClassByQueryHandler : IRequestHandler<GetInstituteClassByQuery, TResult<InstituteClassInfo>>
    {
        IInstituteClassRepository _repo;
        ICurrentUserService _currentUser;
        public GetInstituteClassByQueryHandler(IInstituteClassRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }
        public async Task<TResult<InstituteClassInfo>> Handle(GetInstituteClassByQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<InstituteClassInfo>.Fail("غير مصرح بقراءة ربط الصفوف بالمعاهد");

            InstituteClassInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType, readScope);
            if (entityInfo == null) 
            return TResult<InstituteClassInfo>.Fail(ApplicationExMsg.failToFindInstituteClass);

            return TResult<InstituteClassInfo>.OK(entityInfo);

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
