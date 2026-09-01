

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Address.City.DTO;
using System.Application.Services.Person.Person.DTO;
using System.Domain.Enums.Person;

namespace System.Application.Services.Person.Person.Query
{
    public class GetPersonByQuery : IRequest<TResult<PersonInfo>>, IQuery
    {
        public string Param { get; set; }
        public enPersonParamType ParamType { get; set; }
    }
}
