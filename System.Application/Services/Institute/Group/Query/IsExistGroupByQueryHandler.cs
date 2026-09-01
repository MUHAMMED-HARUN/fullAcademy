using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;

namespace System.Application.Services.Institute.Group.Query
{
    public class IsExistGroupByQueryHandler : IRequestHandler<IsExistGroupByQuery, TResult<bool>>
    {
        private readonly IGroupRepository _repo;

        public IsExistGroupByQueryHandler(IGroupRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<bool>> Handle(IsExistGroupByQuery request, CancellationToken cancellationToken)
        {
            return TResult<bool>.OK(await _repo.IsExistBy(request.Param, request.ParamType));
        }
    }
}
