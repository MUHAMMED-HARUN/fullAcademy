using MediatR;
using System.Application.Abstractions.AcademicPlan;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Command
{
    public class DeleteAcademicPlanCommandHandler : IRequestHandler<DeleteAcademicPlanCommand, TResult<string>>
    {
        private readonly IAcademicPlanRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAcademicPlanCommandHandler(
            IAcademicPlanRepository repo,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteAcademicPlanCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(clsUtil.ToGuid(request.AcademicPlanID));
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            if (entity.Status != enAcademicPlanStatus.Draft)
                return TResult<string>.Fail(ApplicationExMsg.AcademicPlanDeleteOnlyDraft);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
