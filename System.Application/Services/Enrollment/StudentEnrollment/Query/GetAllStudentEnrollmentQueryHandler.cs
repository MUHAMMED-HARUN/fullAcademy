using Gridify;
using MediatR;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Common.Models;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;

namespace System.Application.Services.Enrollment.StudentEnrollment.Query
{
    public class GetAllStudentEnrollmentQueryHandler : IRequestHandler<GetAllStudentEnrollmentQuery, TResult<Paging<StudentEnrollmentInfoView>>>
    {
        private readonly IStudentEnrollmentRepository _repo;

        public GetAllStudentEnrollmentQueryHandler(IStudentEnrollmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<Paging<StudentEnrollmentInfoView>>> Handle(GetAllStudentEnrollmentQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<StudentEnrollmentInfoView>>.OK(await _repo.GetGroupStudentss(request, null));
        }
    }
}
