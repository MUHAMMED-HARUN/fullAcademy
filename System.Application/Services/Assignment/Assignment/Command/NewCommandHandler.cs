using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class NewAssignmentCommandHandler : IRequestHandler<NewAssignmentCommand, TResult<string>>
    {
        private readonly IAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator; // 👈 حقن الـ Mediator لنشر الحدث

        public NewAssignmentCommandHandler(
            IAssignmentRepository repo,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IMediator mediator)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<TResult<string>> Handle(NewAssignmentCommand request, CancellationToken cancellationToken)
        {

        bool hasAssignment=await  _repo.HasAssignment(clsUtil.ToGuid(request.AssignmentNominationID));
            if(hasAssignment)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            request.ScheduledDate = DateOnly.FromDateTime(DateTime.Now).ToString();
            request.CommitteeID = _currentUserService.EntityId;
            request.CommitteeType = _currentUserService.Role == "Teacher" ? enCommitteeType.Teacher : enCommitteeType.User;
            request.AssignmentState = enAssignmentType.inProgress;

            clsAssignment entity = _mapper.Map<NewAssignmentCommand, clsAssignment>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);
          

            return TResult<string>.OK(entityID);
        }
    }
}
