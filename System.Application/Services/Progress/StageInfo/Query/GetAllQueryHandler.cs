using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.DTO;


namespace System.Application.Services.Progress.StageInfo.Query
{
    class GetAllStageInfoQueryHandler : IRequestHandler<GetAllStageInfoQuery, TResult<List<StageInfoInfo>>>
    {
        IStageInfoRepository _repo;
        public GetAllStageInfoQueryHandler ( IStageInfoRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<StageInfoInfo>>> Handle(GetAllStageInfoQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<StageInfoInfo>>.OK(await _repo.GetList());
        }
    }
}
