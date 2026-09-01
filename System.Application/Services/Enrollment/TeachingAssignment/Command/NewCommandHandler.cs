using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;

namespace System.Application.Services.Enrollment.TeachingAssignment.Command
{
    public class NewTeachingAssignmentCommandHandler : IRequestHandler<NewTeachingAssignmentCommand, TResult<string>>
    {
        private readonly ITeachingAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewTeachingAssignmentCommandHandler(ITeachingAssignmentRepository repo, ICurrentUserService currentUser, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewTeachingAssignmentCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.IsTeacherEnrolled(request.TeacherID, request.GroupID,request.SubjectID))
                return TResult<string>.Fail(ApplicationExMsg.TeacherAlreadyEnrolledInGroup);

            clsTeachingAssignment entity = _mapper.Map<NewTeachingAssignmentCommand, clsTeachingAssignment>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
