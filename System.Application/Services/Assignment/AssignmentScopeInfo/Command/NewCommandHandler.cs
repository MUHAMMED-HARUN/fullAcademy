using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Command
{
    public class NewAssignmentScopeInfoCommandHandler : IRequestHandler<NewAssignmentScopeInfoCommand, TResult<string>>
    {
        private readonly IAssignmentScopeInfoRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewAssignmentScopeInfoCommandHandler(IAssignmentScopeInfoRepository repo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewAssignmentScopeInfoCommand request, CancellationToken cancellationToken)
        {
            bool HasAssignmentScopeInfo = await _repo.HasAssignmentScopeInfo(clsUtil.ToGuid(request.StageInfoID), clsUtil.ToGuid(request.AssignmentScopeID));

            if(HasAssignmentScopeInfo)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);


            if (request.MaxScore <= 0 || request.MinPassScore < 0 ||
                request.MaxScore < request.MinPassScore ||
                request.Weight < 0)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            clsAssignmentScopeinfo entity = _mapper.Map<NewAssignmentScopeInfoCommand, clsAssignmentScopeinfo>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
