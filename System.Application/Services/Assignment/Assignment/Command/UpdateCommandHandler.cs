using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.Assignment.Command
{
    public class UpdateAssignmentCommandHandler : IRequestHandler<UpdateAssignmentCommand, TResult<string>>
    {
        private readonly IAssignmentRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateAssignmentCommandHandler(IAssignmentRepository repo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            clsAssignment entity = _mapper.Map<UpdateAssignmentCommand, clsAssignment>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
