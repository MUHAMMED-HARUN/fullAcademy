using Gridify;
using System.Application.Services.Person.Person.DTO;
using System.Application.Services.Person.Person.Query;
using System.Domain.Entities.Person;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Person
{
    public interface IPersonRepository
    {
        Task<string> Add(clsPerson entity);// return CountryID as String
        Task<bool> Update(clsPerson entity);
        Task<List<PersonInfo>> GetList();
        Task<Paging<PersonInfo>> GetListFilter(GetAllPersonQuery request);
        Task<bool> IsExist(string param, enPersonParamType paramType);
        Task<PersonInfo> GetBy(string param, enPersonParamType paramType);
        Task<clsPerson> GetEntityBy(string param, enPersonParamType paramType);
    }
}
