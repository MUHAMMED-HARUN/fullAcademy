using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;

namespace System.Application.Services.Assignment.AssignmentNomination.Command
{
    public class NewAssignmentNominationCommandHandler : IRequestHandler<NewAssignmentNominationCommand, TResult<string>>
    {
        private readonly IAssignmentNominationRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewAssignmentNominationCommandHandler(IAssignmentNominationRepository repo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewAssignmentNominationCommand request, CancellationToken cancellationToken)
        {
            bool hasNomination = await _repo.HasNomination(clsUtil.ToGuid(request.StudentEnrollmentID), clsUtil.ToGuid(request.AssignmentScopeID));
            if (hasNomination)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            request.NominatedByID = _currentUserService.EntityId;
            request.NominatedByType = _currentUserService.Role == "Teacher" ? enUserType.Teacher : enUserType.Admin;
            request.NominationState = enAssignmentType.inProgress;

            clsAssignmentNomination entity = _mapper.Map<NewAssignmentNominationCommand, clsAssignmentNomination>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
