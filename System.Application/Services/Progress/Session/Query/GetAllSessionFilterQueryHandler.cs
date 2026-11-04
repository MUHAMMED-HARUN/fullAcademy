using Gridify;
using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.Session.DTO;

namespace System.Application.Services.Progress.Session.Query
{
    public sealed class GetAllSessionFilterQueryHandler
        : IRequestHandler<GetAllSessionFilterQuery, TResult<Paging<SessionLookupInfo>>>
    {
        private readonly ISessionRepository _repository;

        public GetAllSessionFilterQueryHandler(ISessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<TResult<Paging<SessionLookupInfo>>> Handle(
            GetAllSessionFilterQuery request,
            CancellationToken cancellationToken)
        {
            return TResult<Paging<SessionLookupInfo>>.OK(
                await _repository.GetListFilter(request));
        }
    }
}
