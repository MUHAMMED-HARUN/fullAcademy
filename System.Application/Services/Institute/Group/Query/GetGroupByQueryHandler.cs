using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Group.DTO;

namespace System.Application.Services.Institute.Group.Query
{
    public class GetGroupByQueryHandler : IRequestHandler<GetGroupByQuery, TResult<GroupInfo>>
    {
        private readonly IGroupRepository _repo;

        public GetGroupByQueryHandler(IGroupRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<GroupInfo>> Handle(GetGroupByQuery request, CancellationToken cancellationToken)
        {
            GroupInfo info = await _repo.GetBy(request.Param, request.ParamType);
            if (info == null)
                return TResult<GroupInfo>.Fail(ApplicationExMsg.GroupNotFound);

            return TResult<GroupInfo>.OK(info);
        }
    }
}
