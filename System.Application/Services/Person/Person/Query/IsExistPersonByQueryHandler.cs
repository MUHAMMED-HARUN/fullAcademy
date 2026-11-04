using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;

namespace System.Application.Services.Person.Person.Query
{
    public sealed class IsExistPersonByQueryHandler : IRequestHandler<IsExistPersonByQuery, TResult<bool>>
    {
        IPersonRepository _personRepository;
        public IsExistPersonByQueryHandler( IPersonRepository personRepository)
        {
            _personRepository= personRepository;
        }
        public async Task<TResult<bool>> Handle(IsExistPersonByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _personRepository.IsExist(request.Param, request.ParamType);
            return TResult<bool>.OK(isExist);

        }
    }
}
