

using System.Application.Common.Models;
using System.Application.Services.Address.District.DTO;

namespace System.Application.Services.Address.District.Query
{
    public class GetAllDistrictQuery : BaseFilterQuery<DistrictInfo>
    {
        public Guid? CityID { get; set; }

        public GetAllDistrictQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
