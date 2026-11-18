using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.DTO;
using System.Application.Services.Progress.StageInfo.Query;


namespace System.Application.Services.Progress.StageInfo.Query
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
            StageInfoInfo? entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null)
                return TResult<StageInfoInfo>.Fail("لم يتم العثور على تفاصيل المرحلة");

            return TResult<StageInfoInfo>.OK(entityInfo);

        }
    }
}
