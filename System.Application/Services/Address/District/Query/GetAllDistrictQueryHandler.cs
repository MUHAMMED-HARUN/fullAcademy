using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.Country.DTO;
using System.Application.Services.Address.District.DTO;

namespace System.Application.Services.Address.District.Query
{
    class GetAllDistrictQueryHandler : IRequestHandler<GetAllDistrictQuery, TResult<List<DistrictInfo>>>
    {
        IDistrictRepository _districtRepository;
        public GetAllDistrictQueryHandler(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }
        public async Task<TResult<List<DistrictInfo>>> Handle(GetAllDistrictQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<DistrictInfo>>.OK(await _districtRepository.GetList());
        }
    }
}
