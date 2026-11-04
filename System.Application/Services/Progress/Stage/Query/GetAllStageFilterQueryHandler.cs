using Gridify;
using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.Stage.DTO;

namespace System.Application.Services.Progress.Stage.Query
{
    public sealed class GetAllStageFilterQueryHandler
        : IRequestHandler<GetAllStageFilterQuery, TResult<Paging<StageLookupInfo>>>
    {
        private readonly IStageRepository _repository;

        public GetAllStageFilterQueryHandler(IStageRepository repository)
        {
            _repository = repository;
        }

        public async Task<TResult<Paging<StageLookupInfo>>> Handle(
            GetAllStageFilterQuery request,
            CancellationToken cancellationToken)
        {
            return TResult<Paging<StageLookupInfo>>.OK(
                await _repository.GetListFilter(request));
        }
    }
}
