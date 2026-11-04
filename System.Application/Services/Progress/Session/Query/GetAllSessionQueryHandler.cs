using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.Session.DTO;
using System.Application.Services.Progress.Session.Query;

namespace System.Application.Services.Progress.Session.Query
{
    class GetAllSessionQueryHandler : IRequestHandler<GetAllSessionQuery, TResult<List<SessionInfo>>>
    {
        private readonly ISessionRepository _repo;
        public GetAllSessionQueryHandler ( ISessionRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<SessionInfo>>> Handle(GetAllSessionQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<SessionInfo>>.OK(await _repo.GetList());
        }
    }
}
