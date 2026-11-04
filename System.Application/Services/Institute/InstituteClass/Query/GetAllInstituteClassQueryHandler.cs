using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.InstituteClass.DTO;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Institute.Program.DTO;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    class GetAllInstituteClassQueryHandler : IRequestHandler<GetAllInstituteClassQuery, TResult<List<InstituteClassInfo>>>
    {
        IInstituteClassRepository _repo;
        ICurrentUserService _currentUser;
        public GetAllInstituteClassQueryHandler(IInstituteClassRepository repo, ICurrentUserService currentUser)
        {
            _repo= repo;
            _currentUser = currentUser;
        }
        public async Task<TResult<List<InstituteClassInfo>>> Handle(GetAllInstituteClassQuery request, CancellationToken cancellationToken)
        {
            var readScope = CreateReadScope();
            if (readScope is null)
                return TResult<List<InstituteClassInfo>>.Fail("غير مصرح بقراءة ربط الصفوف بالمعاهد");

            return TResult<List<InstituteClassInfo>>.OK(await _repo.GetList(readScope));
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
