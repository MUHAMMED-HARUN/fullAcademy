using Gridify;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;
using System.Application.Services.Person.Student.DTO;

namespace System.Application.Services.Enrollment.StudentEnrollment.Query
{
    public class GetAllStudentEnrollmentQueryHandler : IRequestHandler<GetAllStudentEnrollmentQuery, TResult<Paging<StudentEnrollmentInfoView>>>
    {
        private readonly IStudentEnrollmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public GetAllStudentEnrollmentQueryHandler(IStudentEnrollmentRepository repo,ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<Paging<StudentEnrollmentInfoView>>> Handle(GetAllStudentEnrollmentQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == Guid.Empty || _currentUserService.EntityId == null)
                return TResult<Paging<StudentEnrollmentInfoView>>.Fail(ApplicationExMsg.AccessDinede);
            else if(_currentUserService.Role == "Teacher")
                return TResult<Paging<StudentEnrollmentInfoView>>.OK(await _repo.GetGroupStudentss(request, _currentUserService.EntityId));
            else if(_currentUserService.Role == "Admin")
                return TResult<Paging<StudentEnrollmentInfoView>>.OK(await _repo.GetGroupStudentss(request, null));


            return TResult<Paging<StudentEnrollmentInfoView>>.Fail(ApplicationExMsg.AccessDinede);


        }
    }
}
