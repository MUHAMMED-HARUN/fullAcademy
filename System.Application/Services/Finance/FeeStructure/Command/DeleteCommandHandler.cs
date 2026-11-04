using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.FeeStructure.Command
{
    public class DeleteFeeStructureCommandHandler : IRequestHandler<DeleteFeeStructureCommand, TResult<string>>
    {
        private readonly IFeeStructureRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public DeleteFeeStructureCommandHandler(
            IFeeStructureRepository repo,
            ICurrentUserService currentUserService)
        {
            _repo = repo;
            _currentUserService = currentUserService;
        }

        public async Task<TResult<string>> Handle(DeleteFeeStructureCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(request.FeeStructureID, enFeeStructureParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            entity.SetDeleted(_currentUserService.UserId.ToString());
            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
