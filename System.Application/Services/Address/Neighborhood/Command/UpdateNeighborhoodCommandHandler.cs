using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.District.DTO;
using System.Application.Services.Address.Neighborhood.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Neighborhood.Command
{
    public class UpdateNeighborhoodCommandHandler : IRequestHandler<UpdateNeighborhoodCommand, TResult<string>>
    {
        private readonly INeighborhoodRepository _neighborhoodRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateNeighborhoodCommandHandler(INeighborhoodRepository neighborhood, ICurrentUserService currentUserService, IMapper mapper)
        {
            _neighborhoodRepository = neighborhood;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateNeighborhoodCommand request, CancellationToken cancellationToken)
        {
            NeighborhoodInfo data  = await _neighborhoodRepository.GetById(request.NeighborhoodID,enNeighborhoodParamType.ID);
            if (data==null)
                return TResult<string>.Fail(ApplicationExMsg.NeighborhoodNotFound);

            clsNeighborhood Neighborhood = _mapper.Map<UpdateNeighborhoodCommand, clsNeighborhood>(request);

            Neighborhood.SetID(clsUtil.ToGuid(request.NeighborhoodID));
            Neighborhood.SetModified( _currentUserService.UserId.ToString());

            await _neighborhoodRepository.Update(Neighborhood);

            return TResult<string>.OK(string.Empty);

            
        }

   
    }
}
