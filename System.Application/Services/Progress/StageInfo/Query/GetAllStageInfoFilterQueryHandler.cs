using Gridify;
using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.DTO;

namespace System.Application.Services.Progress.StageInfo.Query
{
    public sealed class GetAllStageInfoFilterQueryHandler
        : IRequestHandler<GetAllStageInfoFilterQuery, TResult<Paging<StageInfoInfo>>>
    {
        private readonly IStageInfoRepository _repository;

        public GetAllStageInfoFilterQueryHandler(IStageInfoRepository repository)
        {
            _repository = repository;
        }

        public async Task<TResult<Paging<StageInfoInfo>>> Handle(
            GetAllStageInfoFilterQuery request,
            CancellationToken cancellationToken)
        {
            return TResult<Paging<StageInfoInfo>>.OK(
                await _repository.GetListFilter(request));
        }
    }
}
