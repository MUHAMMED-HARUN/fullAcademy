using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Progress.Stage.DTO;


namespace System.Application.Services.Progress.Stage.Query
{
    class GetAllStageQueryHandler : IRequestHandler<GetAllStageQuery, TResult<List<DTO.StageInfo>>>
    {
        IStageRepository _repo;
        public GetAllStageQueryHandler ( IStageRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<DTO.StageInfo>>> Handle(GetAllStageQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<DTO.StageInfo>>.OK(await _repo.GetList());
        }
    }
}
