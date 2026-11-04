using Gridify;
using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;

namespace System.Application.Services.Institute.Group.Query
{
    class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, TResult<Paging<GroupInfo>>>
    {
        private readonly IGroupRepository _repo;

        public GetAllGroupQueryHandler(IGroupRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<Paging<GroupInfo>>> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<GroupInfo>>.OK(await _repo.GetListFilter(request));
        }
    }
}