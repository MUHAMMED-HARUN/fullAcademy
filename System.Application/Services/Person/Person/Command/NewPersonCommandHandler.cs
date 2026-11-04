using AutoMapper;
using MediatR;
using System;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Address.City.Command;
using System.Collections.Generic;
using System.Domain.Entities;
using System.Domain.Entities.Address;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace System.Application.Services.Person.Person.Command
{
    public class NewPersonCommandHandler : IRequestHandler<NewPersonCommand, TResult<string>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public NewPersonCommandHandler(IPersonRepository personRepository ,ICurrentUserService currentUser,
            IMapper mapper,IAddressRepository addressRepository)
        {
            _personRepository = personRepository;
            _currentUserService = currentUser;
           _mapper = mapper;
            _addressRepository = addressRepository;
        }
        public async Task<TResult<string>> Handle(NewPersonCommand request, CancellationToken cancellationToken)
        {
            if ((await _personRepository.IsExist(request.NationalNumber, enPersonParamType.NationalNumber)) == true)
                return TResult<string>.Fail(ApplicationExMsg.PersonNationalNumberExists);

         clsAddress address=   _mapper.Map<NewAddressCommand, clsAddress>(request.Address);
            address.SetID(Guid.NewGuid());
            address.SetCreated(_currentUserService.UserId.ToString());

            clsPerson entity = _mapper.Map<NewPersonCommand, clsPerson>(request);
            entity.SetCreated(_currentUserService.UserId.ToString());
            entity.SetLivingPlaceID(address.Id);

            await _addressRepository.Add(address);

            string entityID = await _personRepository.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
