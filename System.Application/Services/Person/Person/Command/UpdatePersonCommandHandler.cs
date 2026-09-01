using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Person.Person.DTO;
using System.Domain.Entities.Address;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;


namespace System.Application.Services.Person.Person.Command
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, TResult<string>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        public UpdatePersonCommandHandler(IPersonRepository personRepository, ICurrentUserService currentUser,
            IMapper mapper, IAddressRepository addressRepository)
        {
            _personRepository = personRepository;
            _currentUserService = currentUser;
            _mapper = mapper;
            _addressRepository = addressRepository;
        }
        public async Task<TResult<string>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
          clsPerson person= await  _personRepository.GetEntityBy(request.PersonID, enPersonParamType.ID);
            if(person==null)
                return TResult<string>.Fail(ApplicationExMsg.FailAddPerson);
            if ((await _personRepository.IsExist(request.NationalNumber, enPersonParamType.NationalNumber)) 
                && person.NationalNumber!=request.NationalNumber)
                return TResult<string>.Fail(ApplicationExMsg.PersonIsAlreadyStudent);

            clsAddress address = _mapper.Map<UpdateAddressCommand, clsAddress>(request.Address);
            address.SetID(clsUtil.ToGuid(request.Address.AddressID));
            address.SetModified(_currentUserService.UserId.ToString());

            clsPerson entity = _mapper.Map<UpdatePersonCommand, clsPerson>(request);
            entity.SetID(person.Id);
            entity.SetModified(_currentUserService.UserId.ToString());
            entity.SetLivingPlaceID(address.Id);

            await _addressRepository.Update(address);

            await _personRepository.Update(entity);

           ;

            return TResult<string>.OK(string.Empty);
        }
    }
}
