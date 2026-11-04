using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Person.Person.DTO;

namespace System.Application.Services.Person.Person.Query
{
    public sealed class GetPersonForUpsertQuery : IRequest<TResult<PersonForUpsertDto>>, IQuery
    {
        public Guid PersonID { get; init; }
    }
}
