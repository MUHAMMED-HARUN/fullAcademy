using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Person.Person.DTO;

namespace System.Application.Services.Person.Person.Query
{
    public sealed class GetPersonByQueryHandler : IRequestHandler<GetPersonByQuery, TResult<PersonInfo>>
    {
        IPersonRepository _personRepository;
        public GetPersonByQueryHandler( IPersonRepository personRepository)
        {
            _personRepository= personRepository;
        }
        public async Task<TResult<PersonInfo>> Handle(GetPersonByQuery request, CancellationToken cancellationToken)
        {
            var personInfo = await _personRepository.GetBy(request.Param, request.ParamType);
            if (personInfo == null) 
            return TResult<PersonInfo>.Fail(ApplicationExMsg.NotFound);

            return TResult<PersonInfo>.OK(personInfo);

        }
    }
}
