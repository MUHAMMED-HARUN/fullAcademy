using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.Assignment.Command;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Command
{
    public class UpdateAssignmentScopeInfoCommandHandler : IRequestHandler<UpdateAssignmentScopeInfoCommand, TResult<string>>
    {
        private readonly IAssignmentScopeInfoRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateAssignmentScopeInfoCommandHandler(IAssignmentScopeInfoRepository repo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateAssignmentScopeInfoCommand request, CancellationToken cancellationToken)
        {
            if (request.MaxScore <= 0 || request.MinPassScore < 0 ||
         request.MaxScore < request.MinPassScore ||
         request.Weight < 0)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            clsAssignmentScopeinfo entity = _mapper.Map<UpdateAssignmentScopeInfoCommand, clsAssignmentScopeinfo>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
