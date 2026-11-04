using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Class.DTO;
using System.Application.Services.Institute.DTO;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Class.Query
{
    class GetClassByQueryHandler : IRequestHandler<GetClassByQuery, TResult<ClassInfo>>
    {
        IClassRepository _repo;
        ICurrentUserService _currentUser;
        public GetClassByQueryHandler(IClassRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }
        public async Task<TResult<ClassInfo>> Handle(GetClassByQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<ClassInfo>.Fail("غير مصرح بقراءة الصفوف");

            ClassInfo entityInfo = await _repo.GetBy(request.Param, (enClassType)request.ParamType, readScope);
            if (entityInfo == null) 
            return TResult<ClassInfo>.Fail(ApplicationExMsg.failToFindClass);

            return TResult<ClassInfo>.OK(entityInfo);

        }

        private ClassReadScope? CreateReadScope()
        {
            if (!_currentUser.IsAuthenticated)
                return null;
            if (_currentUser.IsAdmin)
                return new ClassReadScope { IsAdmin = true };
            if (!_currentUser.HasPermission("Can_Read_Class"))
                return null;
            if (string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
                return new ClassReadScope { TeacherID = teacherID };
            if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
                return new ClassReadScope { StudentID = studentID };
            return null;
        }
    }
}
