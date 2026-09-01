using Gridify;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class GetTeacherGroupsQueryHandler : IRequestHandler<GetTeacherGroupsQuery, TResult<Paging<TeacherGroupInfo>>>
    {
        private readonly ITeachingAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        public GetTeacherGroupsQueryHandler(ITeachingAssignmentRepository repo,ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<Paging<TeacherGroupInfo>>> Handle(GetTeacherGroupsQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == Guid.Empty || _currentUserService.EntityId == null || _currentUserService.Role != "Teacher")
                return TResult<Paging<TeacherGroupInfo>>.Fail(ApplicationExMsg.AccessDinede);

            return  TResult<Paging<TeacherGroupInfo>>.OK(await _repo.GetTeacherGroups(request,_currentUserService.EntityId.ToString()));
             
        }
    }
}
