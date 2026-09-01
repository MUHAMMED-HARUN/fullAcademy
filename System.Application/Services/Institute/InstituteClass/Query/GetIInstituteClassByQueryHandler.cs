using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.Program.DTO;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    class GetInstituteClassByQueryHandler : IRequestHandler<GetInstituteClassByQuery, TResult<InstituteClassInfo>>
    {
        IInstituteClassRepository _repo;
        public GetInstituteClassByQueryHandler(IInstituteClassRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<InstituteClassInfo>> Handle(GetInstituteClassByQuery request, CancellationToken cancellationToken)
        {
            InstituteClassInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<InstituteClassInfo>.Fail(ApplicationExMsg.failToFindInstituteClass);

            return TResult<InstituteClassInfo>.OK(entityInfo);

        }
    }
}
