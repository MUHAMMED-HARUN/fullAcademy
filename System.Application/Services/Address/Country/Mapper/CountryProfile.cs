
using AutoMapper;
using System.Application.Services.Address.Country.Command;
using System.Domain.Entities;

namespace System.Application.Services.Address.Country.Mapper
{
    public class CountryProfile :Profile
    {
        public CountryProfile()
        {
            CreateMap<NewCountryCommand, clsCountry>()
                  .ConstructUsing(src => new clsCountry(
                 src.CountryName, src.CountryCode
                  )); 

            CreateMap<UpdateCountryCommand, clsCountry>()
                  .ConstructUsing(src => new clsCountry(
                 src.CountryName, src.CountryCode
                  ));
        }
    }
}
