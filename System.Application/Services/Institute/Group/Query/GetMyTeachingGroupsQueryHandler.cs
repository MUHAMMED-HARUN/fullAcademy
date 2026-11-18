using Gridify;
using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;

namespace System.Application.Services.Institute.Group.Query
{
    public sealed class GetMyTeachingGroupsQueryHandler
        : IRequestHandler<GetMyTeachingGroupsQuery, TResult<Paging<GroupInfo>>>
    {
        private readonly IGroupRepository _repository;

        public GetMyTeachingGroupsQueryHandler(IGroupRepository repository) => _repository = repository;

        public async Task<TResult<Paging<GroupInfo>>> Handle(
            GetMyTeachingGroupsQuery request,
            CancellationToken cancellationToken)
        {
            return TResult<Paging<GroupInfo>>.OK(await _repository.GetMyTeachingGroups(request));
        }
    }
}
