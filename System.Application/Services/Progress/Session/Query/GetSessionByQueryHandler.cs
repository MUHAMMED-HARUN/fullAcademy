using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Program.DTO;
using System.Application.Services.Progress.Session.Query;


namespace System.Application.Services.Institute.Institute.Query
{
    class GetSessionByQueryHandler : IRequestHandler<GetSessionByQuery, TResult<SessionInfo>>
    {
        ISessionRepository _repo;
        public GetSessionByQueryHandler( ISessionRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<SessionInfo>> Handle(GetSessionByQuery request, CancellationToken cancellationToken)
        {
            SessionInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<SessionInfo>.Fail(ApplicationExMsg.failToFindSession);

            return TResult<SessionInfo>.OK(entityInfo);

        }
    }
}
