using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Class.DTO;

namespace System.Application.Services.Institute.Class.Query
{
    class GetAllClassQueryHandler : IRequestHandler<GetAllClassQuery, TResult<List<ClassInfo>>>
    {
        IClassRepository _repo;
        ICurrentUserService _currentUser;
        public GetAllClassQueryHandler(IClassRepository repo, ICurrentUserService currentUser)
        {
            _repo= repo;
            _currentUser = currentUser;
        }
        public async Task<TResult<List<ClassInfo>>> Handle(GetAllClassQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<List<ClassInfo>>.Fail("غير مصرح بقراءة الصفوف");

            return TResult<List<ClassInfo>>.OK(await _repo.GetList(readScope));
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
            {
                return new ClassReadScope { TeacherID = teacherID };
            }

            if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return new ClassReadScope { StudentID = studentID };
            }

            return null;
        }
    }
}
