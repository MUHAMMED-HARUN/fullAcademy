using MediatR;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Person.Person.Query
{
    class IsExistPersonByQueryHandler : IRequestHandler<IsExistPersonByQuery, TResult<bool>>
    {
        IPersonRepository _personRepository;
        public IsExistPersonByQueryHandler( IPersonRepository personRepository)
        {
            _personRepository= personRepository;
        }
        public async Task<TResult<bool>> Handle(IsExistPersonByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _personRepository.IsExist(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.FailToFindStudent);

            return TResult<bool>.OK(isExist);

        }
    }
}
