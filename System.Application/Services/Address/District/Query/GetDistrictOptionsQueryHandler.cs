using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Services.Address.District.DTO;

namespace System.Application.Services.Address.District.Query
{
    public sealed class GetDistrictOptionsQueryHandler
        : IRequestHandler<GetDistrictOptionsQuery, TResult<List<DistrictInfo>>>
    {
        private readonly IDistrictRepository _districtRepository;

        public GetDistrictOptionsQueryHandler(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public async Task<TResult<List<DistrictInfo>>> Handle(
            GetDistrictOptionsQuery request,
            CancellationToken cancellationToken)
        {
            return TResult<List<DistrictInfo>>.OK(await _districtRepository.GetList());
        }
    }
}
