using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.FeeStructure.Command
{
    public class UpdateFeeStructureCommandHandler : IRequestHandler<UpdateFeeStructureCommand, TResult<string>>
    {
        private readonly IFeeStructureRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateFeeStructureCommandHandler(
            IFeeStructureRepository repo,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(UpdateFeeStructureCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetEntityBy(request.FeeStructureID, enFeeStructureParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.NotFound);

            if (await _repo.IsExistBy(request.Name, enFeeStructureParamType.Name)
                && entity.Name != request.Name)
                return TResult<string>.Fail(ApplicationExMsg.FeeStructureExistByName);

            _mapper.Map<UpdateFeeStructureCommand, Domain.Entities.Finance.clsFeeStructure>(request, entity);
            entity.SetModified(_currentUserService.UserId.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
