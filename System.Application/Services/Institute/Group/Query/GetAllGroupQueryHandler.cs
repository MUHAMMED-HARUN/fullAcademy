using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;

namespace System.Application.Services.Institute.Group.Query
{
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, TResult<List<GroupInfo>>>
    {
        private readonly IGroupRepository _repo;

        public GetAllGroupQueryHandler(IGroupRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<List<GroupInfo>>> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<GroupInfo>>.OK(await _repo.GetList());
        }
    }
}
