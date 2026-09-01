using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.Result.Command
{
    public class UpdateResultCommandHandler : IRequestHandler<UpdateResultCommand, TResult<string>>
    {
        private readonly IResultRepository _repo;
        private readonly IAssignmentScopeInfoRepository _AssignmentScopeInfo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateResultCommandHandler(IResultRepository repo, IAssignmentScopeInfoRepository assignmentScopeInfo, ICurrentUserService currentUserService, IMapper mapper)
        {
            _repo = repo;
            _AssignmentScopeInfo = assignmentScopeInfo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateResultCommand request, CancellationToken cancellationToken)
        {
            var scopeInfo = await _AssignmentScopeInfo.GetBy(clsUtil.ToGuid(request.AssignmentScopeInfoID));
            if (scopeInfo == null)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            if (request.Score > scopeInfo.MaxScore || request.Score < 0)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            clsResult entity = _mapper.Map<UpdateResultCommand, clsResult>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
