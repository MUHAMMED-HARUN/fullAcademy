using AutoMapper;
using MediatR;
using System;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;

namespace System.Application.Services.Person.Person.Command
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, TResult<string>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public DeletePersonCommandHandler(IPersonRepository personRepository ,ICurrentUserService currentUser,IMapper mapper)
        {
            _personRepository = personRepository;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            clsPerson entity = await _personRepository.GetEntityBy(request.PersonID, enPersonParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.FailToFindStudent);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _personRepository.Update(entity);

           

            return TResult<string>.OK(string .Empty);
        }
    }
}
