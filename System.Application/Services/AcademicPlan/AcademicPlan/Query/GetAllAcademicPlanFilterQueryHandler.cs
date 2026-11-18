using Gridify;
using MediatR;
using System.Application.Abstractions.AcademicPlan;
using System.Application.Common;
using System.Application.Services.AcademicPlan.AcademicPlan.DTO;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Query
{
    public sealed class GetAllAcademicPlanFilterQueryHandler
        : IRequestHandler<GetAllAcademicPlanFilterQuery, TResult<Paging<AcademicPlanLookupInfo>>>
    {
        private readonly IAcademicPlanRepository _repository;

        public GetAllAcademicPlanFilterQueryHandler(IAcademicPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<TResult<Paging<AcademicPlanLookupInfo>>> Handle(
            GetAllAcademicPlanFilterQuery request,
            CancellationToken cancellationToken)
        {
            return TResult<Paging<AcademicPlanLookupInfo>>.OK(
                await _repository.GetListFilter(request));
        }
    }
}
