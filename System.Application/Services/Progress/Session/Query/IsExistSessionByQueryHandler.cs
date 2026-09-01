using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Progress.Session.Query
{
    class IsExistSessionByQueryHandler : IRequestHandler<IsExistSessionByQuery, TResult<bool>>
    {
        ISessionRepository _repo;
        public IsExistSessionByQueryHandler(   ISessionRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistSessionByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindSession);

            return TResult<bool>.OK(isExist);

        }
    }
}
