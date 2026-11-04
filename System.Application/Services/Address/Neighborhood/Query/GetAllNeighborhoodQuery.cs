

using System.Application.Common.Models;
using System.Application.Services.Address.Neighborhood.DTO;
using System.Application.Services.Authentication.Role.DTO;

namespace System.Application.Services.Address.Neighborhood.Query
{
    public class GetAllNeighborhoodQuery : BaseFilterQuery<NeighborhoodInfo>
    {
        public Guid? DistrictID { get; set; }

        public GetAllNeighborhoodQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
