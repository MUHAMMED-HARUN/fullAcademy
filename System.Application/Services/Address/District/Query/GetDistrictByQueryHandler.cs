using MediatR;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.District.DTO;

namespace System.Application.Services.Address.District.Query
{
    public sealed class GetDistrictByQueryHandler
        : IRequestHandler<GetDistrictByQuery, TResult<DistrictInfo>>
    {
        private readonly IDistrictRepository _districtRepository;

        public GetDistrictByQueryHandler(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }

        public async Task<TResult<DistrictInfo>> Handle(
            GetDistrictByQuery request,
            CancellationToken cancellationToken)
        {
            var district = await _districtRepository.GetById(request.Param, request.ParamType);
            return district is null
                ? TResult<DistrictInfo>.Fail(ApplicationExMsg.DistrictNotFound)
                : TResult<DistrictInfo>.OK(district);
        }
    }
}
