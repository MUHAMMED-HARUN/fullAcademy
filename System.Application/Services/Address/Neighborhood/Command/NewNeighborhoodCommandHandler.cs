using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;

namespace System.Application.Services.Address.Neighborhood.Command
{
    public class NewNeighborhoodCommandHandler : IRequestHandler<NewNeighborhoodCommand, TResult<string>>
    {
        private readonly INeighborhoodRepository _neighborhoodRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewNeighborhoodCommandHandler(INeighborhoodRepository neighborhood,ICurrentUserService currentUserService,IMapper mapper)
        {
            _neighborhoodRepository = neighborhood;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewNeighborhoodCommand request, CancellationToken cancellationToken)
        {
            bool IsExist = await _neighborhoodRepository.IsExist(request.NeighborhoodName);
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
