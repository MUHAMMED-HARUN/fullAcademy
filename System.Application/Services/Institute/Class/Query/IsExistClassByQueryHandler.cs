using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Class.Query;

namespace System.Application.Services.Institute.Class.Query
{
    class IsExistClassByQueryHandler : IRequestHandler<IsExistClassByQuery, TResult<bool>>
    {
        IClassRepository _repo;
        public IsExistClassByQueryHandler(IClassRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistClassByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindClass);

            return TResult<bool>.OK(isExist);

        }
    }
}
