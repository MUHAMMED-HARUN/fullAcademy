using AutoMapper;
using MediatR;
using System.Application.Abstractions.AcademicPlan;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.AcademicPlan;
using System.Domain.Enums.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Command
{
    public class NewAcademicPlanCommandHandler : IRequestHandler<NewAcademicPlanCommand, TResult<string>>
    {
        private readonly IAcademicPlanRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewAcademicPlanCommandHandler(
            IAcademicPlanRepository repo,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewAcademicPlanCommand request, CancellationToken cancellationToken)
        {
            var providerInstituteID = clsUtil.ToGuid(request.ProviderInstituteID);
            if (!await _repo.IsInstituteExist(providerInstituteID))
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanProviderInstituteNotFound);

            var scopeID = ResolveScopeID(request);
            var scope = await _repo.GetScopeAsync(request.ScopeType, scopeID);
            if (scope == null)
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanScopeNotFound);

            if (scope.ProviderInstituteID is Guid scopeProviderID && scopeProviderID != providerInstituteID)
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanProviderMismatch);

            if (scope.StageInfoIDs.Count == 0)
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanScopeHasNoStageInfo);

            clsAcademicPlan entity = _mapper.Map<NewAcademicPlanCommand, clsAcademicPlan>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());
            entity.SetItems(scope.StageInfoIDs);

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }

        private static Guid ResolveScopeID(NewAcademicPlanCommand request) => request.ScopeType switch
        {
            enAcademicPlanScopeType.Program => clsUtil.ToGuid(request.ProgramID),
            enAcademicPlanScopeType.InstituteClass => clsUtil.ToGuid(request.InstituteClassID),
            enAcademicPlanScopeType.Stage => clsUtil.ToGuid(request.StageID),
            enAcademicPlanScopeType.StageInfo => clsUtil.ToGuid(request.StageInfoID),
            _ => Guid.Empty
        };
    }
}
