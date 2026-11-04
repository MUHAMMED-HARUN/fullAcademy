using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Address.DTO;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;
using System.Domain.Entities;
using System.Domain.Entities.Address;
using System.Domain.Enums;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Address.Command
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, TResult<string>>
    {
        private readonly IAddressRepository _AddressRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateAddressCommandHandler(IAddressRepository addressRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _AddressRepository = addressRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var data = await _AddressRepository.GetBy(request.AddressID, enAddressParamType.ID);
            if (data==null)
                return TResult<string>.Fail(ApplicationExMsg.AddressNotFound);

            clsAddress Address = _mapper.Map<UpdateAddressCommand, clsAddress>(request);

            Address.SetID(clsUtil.ToGuid(request.AddressID));
            Address.SetModified( _currentUserService.UserId.ToString());

            await _AddressRepository.Update(Address);
            return TResult<string>.OK(string.Empty);
        }

   
    }
}
