using Gridify;
using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Services.Person.Person.DTO;

namespace System.Application.Services.Person.Person.Query
{
    public class GetAllPersonQueryHandler : IRequestHandler<GetAllPersonQuery, TResult<Paging<PersonInfo>>>
    {
        IPersonRepository _personRepository;
        public GetAllPersonQueryHandler( IPersonRepository personRepository)
        {
            _personRepository= personRepository;
        }
        public async Task<TResult<Paging<PersonInfo>>> Handle(GetAllPersonQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<PersonInfo>>.OK(await _personRepository.GetListFilter(request));
        }
    }
}
