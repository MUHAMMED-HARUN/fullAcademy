using MediatR;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Assignment.Result.DTO;

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

    public class GetAllResultQueryHandler : IRequestHandler<GetAllResultQuery, TResult<List<ResultInfo>>>
    {
        private readonly IResultRepository _repo;

        public GetAllResultQueryHandler(IResultRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<ResultInfo>>> Handle(GetAllResultQuery request, CancellationToken cancellationToken)
        {
            var result = await _repo.GetList();
            if (result == null || result.Count == 0)
                return TResult<List<ResultInfo>>.Fail(ApplicationExMsg.NotFound);

            return TResult<List<ResultInfo>>.OK(result);
        }
    }
}
