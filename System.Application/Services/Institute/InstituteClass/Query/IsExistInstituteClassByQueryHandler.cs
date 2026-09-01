using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    class IsExistInstituteClassByQueryHandler : IRequestHandler<IsExistInstituteClassByQuery, TResult<bool>>
    {
        IInstituteClassRepository _repo;
        public IsExistInstituteClassByQueryHandler(IInstituteClassRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistInstituteClassByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindProgram);

            return TResult<bool>.OK(isExist);

        }
    }
}
