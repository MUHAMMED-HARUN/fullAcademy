using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Progress.StageInfo.Query
{
    class IsExistStageInfoByQueryHandler : IRequestHandler<IsExistStageInfoByQuery, TResult<bool>>
    {
        IStageInfoRepository _repo;
        public IsExistStageInfoByQueryHandler(   IStageInfoRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistStageInfoByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindStageInfo);

            return TResult<bool>.OK(isExist);

        }
    }
}
