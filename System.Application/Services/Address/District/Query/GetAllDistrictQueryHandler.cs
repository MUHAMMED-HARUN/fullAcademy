using Gridify;
using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;

namespace System.Application.Services.Address.District.Query
{
    public class GetAllDistrictQueryHandler : IRequestHandler<GetAllDistrictQuery, TResult<Paging<DistrictInfo>>>
    {
        IDistrictRepository _districtRepository;
        public GetAllDistrictQueryHandler(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }
        public async Task<TResult<Paging<DistrictInfo>>> Handle(GetAllDistrictQuery request, CancellationToken cancellationToken)
        {
            return TResult<Paging<DistrictInfo>>.OK(await _districtRepository.GetListFilter(request));
        }
    }
}
