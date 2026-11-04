using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.Stage.DTO;
using System.Application.Services.Progress.Stage.Query;
using StageInfoDto = System.Application.Services.Progress.Stage.DTO.StageInfo;


namespace System.Application.Services.Progress.Stage.Query
{
    class GetStageByQueryHandler : IRequestHandler<GetStageByQuery, TResult<StageInfoDto>>
    {
        IStageRepository _repo;
        public GetStageByQueryHandler( IStageRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<StageInfoDto>> Handle(GetStageByQuery request, CancellationToken cancellationToken)
        {
            StageInfoDto? entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
                return TResult<StageInfoDto>.Fail("لم يتم العثور على المرحلة");

            return TResult<StageInfoDto>.OK(entityInfo);

        }
    }
}
