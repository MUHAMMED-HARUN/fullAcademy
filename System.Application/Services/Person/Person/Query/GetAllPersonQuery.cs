using System.Application.Common.Models;
using System.Application.Services.Person.Person.DTO;

namespace System.Application.Services.Person.Person.Query
{
    public class GetAllPersonQuery : BaseFilterQuery<PersonInfo>
    {
        public GetAllPersonQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
