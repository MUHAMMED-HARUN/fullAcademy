using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;

namespace System.Application.Services.Enrollment.TeachingAssignment.Command
{
    public class UpdateTeachingAssignmentCommandHandler : IRequestHandler<UpdateTeachingAssignmentCommand, TResult<string>>
    {
        private readonly ITeachingAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateTeachingAssignmentCommandHandler(ITeachingAssignmentRepository repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateTeachingAssignmentCommand request, CancellationToken cancellationToken)
        {
            clsTeachingAssignment entity = await _repo.GetEntityBy(request.EnrollmentID, enTeachingAssignmentType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.TeachingAssignmentNotFound);

            if ((await _repo.IsTeacherEnrolled(request.TeacherID, request.GroupID, request.SubjectID)) && 
                (entity.TeacherID != clsUtil.ToGuid(request.TeacherID) || entity.GroupID != clsUtil.ToGuid(request.GroupID)
                || entity.SubjectID != clsUtil.ToGuid(request.SubjectID)))
            {
                return TResult<string>.Fail(ApplicationExMsg.TeacherAlreadyEnrolledInGroup);
            }

            _mapper.Map<UpdateTeachingAssignmentCommand, clsTeachingAssignment>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
