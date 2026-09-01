using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Class.DTO;
using System.Application.Services.Institute.DTO;

namespace System.Application.Services.Institute.Class.Query
{
    class GetClassByQueryHandler : IRequestHandler<GetClassByQuery, TResult<ClassInfo>>
    {
        IClassRepository _repo;
        public GetClassByQueryHandler(IClassRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<ClassInfo>> Handle(GetClassByQuery request, CancellationToken cancellationToken)
        {
            ClassInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<ClassInfo>.Fail(ApplicationExMsg.failToFindClass);

            return TResult<ClassInfo>.OK(entityInfo);

        }
    }
}
