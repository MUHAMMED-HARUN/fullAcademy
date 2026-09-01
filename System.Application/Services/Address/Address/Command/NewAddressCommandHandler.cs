using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Entities.Address;
using System.Domain.Entities.Permissions;

namespace System.Application.Services.Address.Address.Command
{
    public class NewAddressCommandHandler : IRequestHandler<NewAddressCommand, TResult<string>>
    {
        private readonly IAddressRepository _AddressRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewAddressCommandHandler(IAddressRepository addressRepository,ICurrentUserService currentUserService,IMapper mapper)
        {
            _AddressRepository = addressRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewAddressCommand request, CancellationToken cancellationToken)
        {
            clsAddress Adderss = _mapper.Map<NewAddressCommand,clsAddress>(request);

            Adderss.SetCreated(_currentUserService.UserId.ToString());
            string AddressID = await _AddressRepository.Add(Adderss);

            if (string.IsNullOrEmpty(AddressID))
                return TResult<string>.Fail(ApplicationExMsg.FailAddAddress);

            return TResult<string>.OK(AddressID);
        }
    }
}
