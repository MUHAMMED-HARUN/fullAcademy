using MediatR;
using System.Application.Abstractions.AcademicPlan;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.AcademicPlan;
using System.Domain.Enums.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Command
{
    public class UpdateAcademicPlanCommandHandler : IRequestHandler<UpdateAcademicPlanCommand, TResult<string>>
    {
        private readonly IAcademicPlanRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public UpdateAcademicPlanCommandHandler(
            IAcademicPlanRepository repo,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(UpdateAcademicPlanCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(clsUtil.ToGuid(request.AcademicPlanID));
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            if (entity.Status == enAcademicPlanStatus.Archived)
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanNotEditable);

            var scope = await _repo.GetScopeAsync(entity.ScopeType, entity.ScopeID);
            if (scope == null)
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanScopeNotFound);

            if (scope.StageInfoIDs.Count == 0)
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanScopeHasNoStageInfo);

            var userID = _currentUserService.UserId.ToString();

            if (entity.Status == enAcademicPlanStatus.Draft)
            {
                entity.SetDetails(request.Name, request.Notes, request.EffectiveFrom, request.EffectiveTo);
                entity.SetItems(scope.StageInfoIDs);
                entity.SetModified(userID);

                await _repo.Update(entity);
                return TResult<string>.OK(entity.Id.ToString());
            }

            clsAcademicPlan nextVersion = entity.CreateNextVersion(userID);
            nextVersion.SetDetails(request.Name, request.Notes, request.EffectiveFrom, request.EffectiveTo);
            nextVersion.SetItems(scope.StageInfoIDs);

            string nextVersionID = await _repo.Add(nextVersion);
            if (string.IsNullOrEmpty(nextVersionID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(nextVersionID);
        }
    }
}
