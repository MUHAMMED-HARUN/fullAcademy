using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;

namespace System.Application.Services.Progress.Session.Query
{
    class IsExistSessionByQueryHandler : IRequestHandler<IsExistSessionByQuery, TResult<bool>>
    {
        private readonly ISessionRepository _repo;
        public IsExistSessionByQueryHandler(   ISessionRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistSessionByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            return TResult<bool>.OK(isExist);

        }
    }
}
