using MediatR;
using System.Application.Abstractions.AcademicPlan;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.AcademicPlan.AcademicPlan.DTO;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Query
{
    public class GetAcademicPlanByQueryHandler : IRequestHandler<GetAcademicPlanByQuery, TResult<AcademicPlanInfo>>
    {
        private readonly IAcademicPlanRepository _repo;

        public GetAcademicPlanByQueryHandler(IAcademicPlanRepository repo)
        {
            _repo = repo;
        }

        public async Task<TResult<AcademicPlanInfo>> Handle(GetAcademicPlanByQuery request, CancellationToken cancellationToken)
        {
            var info = await _repo.GetBy(clsUtil.ToGuid(request.AcademicPlanID));
            if (info == null)
                return TResult<AcademicPlanInfo>.Fail(ApplicationExMsg.NotFound);

            return TResult<AcademicPlanInfo>.OK(info);
        }
    }
}
