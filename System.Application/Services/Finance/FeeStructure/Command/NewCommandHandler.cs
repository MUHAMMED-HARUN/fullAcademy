using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Finance;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;

namespace System.Application.Services.Finance.FeeStructure.Command
{
    public class NewFeeStructureCommandHandler : IRequestHandler<NewFeeStructureCommand, TResult<string>>
    {
        private readonly IFeeStructureRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public NewFeeStructureCommandHandler(
            IFeeStructureRepository repo,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<TResult<string>> Handle(NewFeeStructureCommand request, CancellationToken cancellationToken)
        {
            if (await _repo.IsExistBy(request.Name, enFeeStructureParamType.Name))
                return TResult<string>.Fail(ApplicationExMsg.FeeStructureExistByName);

            clsFeeStructure entity = _mapper.Map<NewFeeStructureCommand, clsFeeStructure>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
