using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Institute.Institute.Query
{
    class IsExistInstituteByQueryHandler : IRequestHandler<IsExistInstituteByQuery, TResult<bool>>
    {
        IInstituteRepository _repo;
        public IsExistInstituteByQueryHandler(   IInstituteRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistInstituteByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindProgram);

            return TResult<bool>.OK(isExist);

        }
    }
}
