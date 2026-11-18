using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.Result.DTO;
using Gridify;

namespace System.Application.Services.Assignment.Result.Query
{
    public class GetResultByQueryHandler : IRequestHandler<GetResultByQuery, TResult<ResultInfo>>
    {
        private readonly IResultRepository _repo;

        public GetResultByQueryHandler(IResultRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<ResultInfo>> Handle(GetResultByQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetBy(clsUtil.ToGuid(request.ID));
            if (result == null)
                return TResult<ResultInfo>.Fail(ApplicationExMsg.NotFound);

            return TResult<ResultInfo>.OK(result);
        }
    }

    public sealed class GetResultForUpsertQueryHandler : IRequestHandler<GetResultForUpsertQuery, TResult<ResultInfo>>
    {
        private readonly IResultRepository _repo;

        public GetResultForUpsertQueryHandler(IResultRepository repo) => _repo = repo;

        public async Task<TResult<ResultInfo>> Handle(GetResultForUpsertQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.ResultID, out var resultID) || resultID == Guid.Empty)
                return TResult<ResultInfo>.Fail(ApplicationExMsg.NotFound);

            var result = await _repo.GetForUpsert(resultID);
            return result is null
                ? TResult<ResultInfo>.Fail(ApplicationExMsg.NotFound)
                : TResult<ResultInfo>.OK(result);
        }
    }

    public class GetAllResultQueryHandler : IRequestHandler<GetAllResultQuery, TResult<Paging<ResultInfo>>>
    {
        private readonly IResultRepository _repo;

        public GetAllResultQueryHandler(IResultRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<Paging<ResultInfo>>> Handle(GetAllResultQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<ResultInfo>>.OK(await _repo.GetListFilter(request));
        }
    }
}
