using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;

namespace System.Application.Services.Progress.Stage.Query
{
    class IsExistStageByQueryHandler : IRequestHandler<IsExistStageByQuery, TResult<bool>>
    {
        IStageRepository _repo;
        public IsExistStageByQueryHandler(   IStageRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistStageByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            return TResult<bool>.OK(isExist);

        }
    }
}
