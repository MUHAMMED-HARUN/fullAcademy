using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;

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
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindStage);

            return TResult<bool>.OK(isExist);

        }
    }
}
