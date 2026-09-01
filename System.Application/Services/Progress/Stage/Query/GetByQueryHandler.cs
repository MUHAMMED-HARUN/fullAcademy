using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Progress.Stage.DTO;
using System.Application.Services.Progress.Stage.Query;


namespace System.Application.Services.Institute.Institute.Query
{
    class GetStageByQueryHandler : IRequestHandler<GetStageByQuery, TResult<StageInfo>>
    {
        IStageRepository _repo;
        public GetStageByQueryHandler( IStageRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<StageInfo>> Handle(GetStageByQuery request, CancellationToken cancellationToken)
        {
            StageInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<StageInfo>.Fail(ApplicationExMsg.failToFindStage);

            return TResult<StageInfo>.OK(entityInfo);

        }
    }
}
