using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Address.Command;
using System.Domain.Entities.Address;
using System.Domain.Entities.Institue;
using System.Domain.Enums.Institute;


namespace System.Application.Services.Institute.Institute.Command
{
    public class NewInstituteCommandHandler : IRequestHandler<NewInstituteCommand, TResult<string>>
    {
        private readonly IInstituteRepository _instituteRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public NewInstituteCommandHandler(IInstituteRepository instituteRepository, ICurrentUserService currentUser,
            IMapper mapper,IAddressRepository addressRepository)
        {
            _instituteRepo = instituteRepository;
            _currentUserService = currentUser;
           _mapper = mapper;
            _addressRepository = addressRepository;
        }
        public async Task<TResult<string>> Handle(NewInstituteCommand request, CancellationToken cancellationToken)
        {
            if ((await _instituteRepo.IsExistBy(request.InstituteName, enInstituteType.Name)) == true)
                return TResult<string>.Fail(ApplicationExMsg.failToFindInstitue);

         clsAddress address=   _mapper.Map<NewAddressCommand, clsAddress>(request.Address);
            address.SetID(Guid.NewGuid());
            address.SetCreated(_currentUserService.UserId.ToString());

            clsInstitute entity = _mapper.Map<NewInstituteCommand, clsInstitute>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());
            entity.SetAddressID(address.Id);

            await _addressRepository.Add(address);

            string entityID = await _instituteRepo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
