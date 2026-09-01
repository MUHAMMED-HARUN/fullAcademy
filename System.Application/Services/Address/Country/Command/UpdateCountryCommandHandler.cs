using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Country.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Country.Command
{
    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, TResult<string>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateCountryCommandHandler(ICountryRepository countryRepository,ICurrentUserService currentUserService,IMapper mapper)
        {
            _countryRepository = countryRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            CountryInfo data  = await _countryRepository.GetById(request.CountryID, enCountryParamType.ID);
            if (data==null)
                return TResult<string>.Fail(ApplicationExMsg.CountryNotFound);

            clsCountry Country = _mapper.Map<UpdateCountryCommand, clsCountry>(request);

            Country.SetID(clsUtil.ToGuid(request.CountryID));
            Country.SetModified( _currentUserService.UserId.ToString());

            await _countryRepository.Update(Country);


            return TResult<string>.OK(string.Empty);

            
        }

   
    }
}
