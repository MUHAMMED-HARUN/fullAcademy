using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.Session.DTO;
using System.Application.Services.Progress.Session.Query;


namespace System.Application.Services.Progress.Session.Query
{
    public class GetSessionByQueryHandler : IRequestHandler<GetSessionByQuery, TResult<SessionInfo>>
    {
        private readonly ISessionRepository _repo;
        public GetSessionByQueryHandler( ISessionRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<SessionInfo>> Handle(GetSessionByQuery request, CancellationToken cancellationToken)
        {
            SessionInfo? entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null)
                return TResult<SessionInfo>.Fail("لم يتم العثور على السجل");

            return TResult<SessionInfo>.OK(entityInfo);

        }
    }
}
