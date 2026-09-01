using AutoMapper;
using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.Result.Command
{
    public class NewResultCommandHandler : IRequestHandler<NewResultCommand, TResult<string>>
    {
        private readonly IResultRepository _repo;
        private readonly IAssignmentScopeInfoRepository _AssignmentScopeInfo;
        private readonly IAssignmentRepository _Assignment;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewResultCommandHandler(IResultRepository repo,
            IAssignmentScopeInfoRepository assignmentScopeInfo,
            IAssignmentRepository assignment,
            ICurrentUserService currentUserService, 
            IMapper mapper)
        {
            _repo = repo;
            _AssignmentScopeInfo = assignmentScopeInfo;
            _Assignment = assignment;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewResultCommand request, CancellationToken cancellationToken)
        {

            bool  hasReasult= await _repo.HasResult(clsUtil.ToGuid(request.AssignmentID),
                clsUtil.ToGuid(request.AssignmentScopeInfoID));
           var scopeInfo= await _AssignmentScopeInfo.GetBy(clsUtil.ToGuid(request.AssignmentScopeInfoID));
            if (scopeInfo == null || hasReasult) 
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);



            if(request.Score>scopeInfo.MaxScore|| request.Score<0)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            clsResult entity = _mapper.Map<NewResultCommand, clsResult>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
