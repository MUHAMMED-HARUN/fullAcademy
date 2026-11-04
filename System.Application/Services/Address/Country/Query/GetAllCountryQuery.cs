

using System.Application.Common.Models;
using System.Application.Services.Address.Country.DTO;

namespace System.Application.Services.Address.Country.Query
{
    public class GetAllCountryQuery : BaseFilterQuery<CountryInfo>
    {
        public GetAllCountryQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
