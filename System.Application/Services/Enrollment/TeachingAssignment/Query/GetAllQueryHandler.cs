using Gridify;
using MediatR;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class GetAllTeachingAssignmentQueryHandler : IRequestHandler<GetAllTeachingAssignmentQuery, TResult<Paging<TeachingAssignmentInfo>>>
    {
        private readonly ITeachingAssignmentRepository _repo;

        public GetAllTeachingAssignmentQueryHandler(ITeachingAssignmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<Paging<TeachingAssignmentInfo>>> Handle(GetAllTeachingAssignmentQuery request, CancellationToken cancellationToken)
        {
            return TResult < Paging < TeachingAssignmentInfo >>.OK(await _repo.GetListFilter(request));
        }
    }
}
