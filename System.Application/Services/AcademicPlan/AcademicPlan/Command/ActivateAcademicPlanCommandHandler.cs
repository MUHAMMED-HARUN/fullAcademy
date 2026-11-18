using MediatR;
using System.Application.Abstractions.AcademicPlan;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Command
{
    public class ActivateAcademicPlanCommandHandler : IRequestHandler<ActivateAcademicPlanCommand, TResult<string>>
    {
        private readonly IAcademicPlanRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public ActivateAcademicPlanCommandHandler(
            IAcademicPlanRepository repo,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(ActivateAcademicPlanCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(clsUtil.ToGuid(request.AcademicPlanID));
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            if (entity.Status != enAcademicPlanStatus.Draft)
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanActivateOnlyDraft);

            var userID = _currentUserService.UserId.ToString();

            var previousActivePlans = await _repo.GetActiveByScopeAsync(entity.ScopeType, entity.ScopeID, entity.Id);
            foreach (var previousPlan in previousActivePlans)
                previousPlan.Archive(userID);

            entity.Activate(userID);

            await _repo.Update(entity);
            return TResult<string>.OK(entity.Id.ToString());
        }
    }
}
