using MediatR;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Services.Finance.FeeStructure.DTO;

namespace System.Application.Services.Finance.FeeStructure.Query
{
    public sealed class GetFeeStructureByAcademicPlanQueryHandler
        : IRequestHandler<GetFeeStructureByAcademicPlanQuery, TResult<List<FeeStructureInfo>>>
    {
        private readonly IFeeStructureRepository _repository;
        public GetFeeStructureByAcademicPlanQueryHandler(IFeeStructureRepository repository) => _repository = repository;

        public async Task<TResult<List<FeeStructureInfo>>> Handle(GetFeeStructureByAcademicPlanQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.AcademicPlanID, out var academicPlanID) || academicPlanID == Guid.Empty)
                return TResult<List<FeeStructureInfo>>.Fail("AcademicPlanID is invalid.");
            return TResult<List<FeeStructureInfo>>.OK(await _repository.GetByAcademicPlan(academicPlanID));
        }
    }
}
