using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Class.DTO;
using System.Application.Services.Institute.Class.Query;

namespace System.Application.Services.Institute.Class.Query
{
    class IsExistClassByQueryHandler : IRequestHandler<IsExistClassByQuery, TResult<bool>>
    {
        IClassRepository _repo;
        ICurrentUserService _currentUser;
        public IsExistClassByQueryHandler(IClassRepository repo, ICurrentUserService currentUser)
        {
           _repo = repo;
           _currentUser = currentUser;
        }
        public async Task<TResult<bool>> Handle(IsExistClassByQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<bool>.Fail("غير مصرح بقراءة الصفوف");

            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType, readScope);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindClass);

            return TResult<bool>.OK(isExist);

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
