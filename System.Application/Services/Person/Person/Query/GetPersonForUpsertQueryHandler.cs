using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Person.Person.DTO;

namespace System.Application.Services.Person.Person.Query
{
    public sealed class GetPersonForUpsertQueryHandler
        : IRequestHandler<GetPersonForUpsertQuery, TResult<PersonForUpsertDto>>
    {
        private readonly IPersonRepository _personRepository;

        public GetPersonForUpsertQueryHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<TResult<PersonForUpsertDto>> Handle(
            GetPersonForUpsertQuery request,
            CancellationToken cancellationToken)
        {
            if (request.PersonID == Guid.Empty)
                return TResult<PersonForUpsertDto>.Fail(ApplicationExMsg.NotFound);

            var person = await _personRepository.GetForUpsert(request.PersonID);
            return person is null
                ? TResult<PersonForUpsertDto>.Fail(ApplicationExMsg.NotFound)
                : TResult<PersonForUpsertDto>.OK(person);
        }
    }
}
