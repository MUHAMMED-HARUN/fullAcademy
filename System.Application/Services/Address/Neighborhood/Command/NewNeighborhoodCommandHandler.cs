using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Neighborhood.Command
{
    public class NewNeighborhoodCommandHandler : IRequestHandler<NewNeighborhoodCommand, TResult<string>>
    {
        private readonly INeighborhoodRepository _neighborhoodRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewNeighborhoodCommandHandler(
            INeighborhoodRepository neighborhood,
            IDistrictRepository districtRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _neighborhoodRepository = neighborhood;
            _districtRepository = districtRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewNeighborhoodCommand request, CancellationToken cancellationToken)
        {
            var district = await _districtRepository.GetById(request.DistrictID, enDistrictParamType.ID);
            if (district is null)
                return TResult<string>.Fail(ApplicationExMsg.DistrictNotFound);

            bool IsExist = await _neighborhoodRepository.IsNameExist(request.NeighborhoodName);
            if (IsExist)
                return TResult<string>.Fail(ApplicationExMsg.NeighborhoodIsExist);
            clsNeighborhood Neighborhood = _mapper.Map<NewNeighborhoodCommand,clsNeighborhood>(request);

            Neighborhood.SetCreated(_currentUserService.UserId.ToString());
            string NeighborhoodID = await _neighborhoodRepository.Add(Neighborhood);

            if (string.IsNullOrEmpty(NeighborhoodID))
                return TResult<string>.Fail(ApplicationExMsg.FailAddNeighborhood);

            return TResult<string>.OK(NeighborhoodID);
        }
    }
}
