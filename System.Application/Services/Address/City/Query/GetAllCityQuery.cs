

using System.Application.Common.Models;
using System.Application.Services.Address.City.DTO;

namespace System.Application.Services.Address.City.Query
{
    public class GetAllCityQuery : BaseFilterQuery<CityInfo>
    {
        public GetAllCityQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
