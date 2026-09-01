using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Progress.StageInfo.DTO;
using System.Application.Services.Progress.StageInfo.Query;


namespace System.Application.Services.Institute.Institute.Query
{
    class GetStageInfoByQueryHandler : IRequestHandler<GetStageInfoByQuery, TResult<StageInfoInfo>>
    {
        IStageInfoRepository _repo;
        public GetStageInfoByQueryHandler( IStageInfoRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<StageInfoInfo>> Handle(GetStageInfoByQuery request, CancellationToken cancellationToken)
        {
            StageInfoInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<StageInfoInfo>.Fail(ApplicationExMsg.failToFindStageInfo);

            return TResult<StageInfoInfo>.OK(entityInfo);

        }
    }
}
