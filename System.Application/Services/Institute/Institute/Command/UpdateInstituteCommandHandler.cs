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
using System.Domain.Enums.Address;
using System.Domain.Enums.Institute;


namespace System.Application.Services.Institute.Institute.Command
{
    public class UpdateInstituteCommandHandler : IRequestHandler<UpdateInstituteCommand, TResult<string>>
    {
        private readonly IInstituteRepository _instituteRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public UpdateInstituteCommandHandler(IInstituteRepository instituteRepository, ICurrentUserService currentUser,
            IMapper mapper, IAddressRepository addressRepository)
        {
            _instituteRepo = instituteRepository;
            _currentUserService = currentUser;
            _mapper = mapper;
            _addressRepository = addressRepository;
        }
        public async Task<TResult<string>> Handle(UpdateInstituteCommand request, CancellationToken cancellationToken)
        {
          clsInstitute Institute = await  _instituteRepo.GetEntityBy(request.InstituteID, enInstituteType.ID);
            clsAddress address = await _addressRepository.GetEntityBy(request.Address.AddressID, enAddressParamType.ID);
            
            if(Institute == null && address ==null)
                return TResult<string>.Fail(ApplicationExMsg.failUpdateInstitute);

            if ((await _instituteRepo.IsExistBy(request.InstituteName, enInstituteType.Name)) 
                && Institute.Name!=request.InstituteName)
                return TResult<string>.Fail(ApplicationExMsg.InstituteExistByName);

            address = _mapper.Map<UpdateAddressCommand, clsAddress>(request.Address, address);
            address.SetID(clsUtil.ToGuid(request.Address.AddressID));
            address.SetModified(_currentUserService.UserId.ToString());

            Institute = _mapper.Map<UpdateInstituteCommand, clsInstitute>(request, Institute);
            Institute.SetModified(_currentUserService.UserId.ToString());

            await _addressRepository.Update(address);

            await _instituteRepo.Update(Institute);

           ;

            return TResult<string>.OK(string.Empty);
        }
    }
}
