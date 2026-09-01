
using AutoMapper;
using System.Application.Common;
using System.Application.Services.Address.Address.Command;
using System.Application.Services.Address.City.Command;
using System.Application.Services.Address.Country.Command;
using System.Domain.Entities;
using System.Domain.Entities.Address;

namespace System.Application.Services.Address.Address.Mapper
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<NewAddressCommand, clsAddress>()
                  .ConstructUsing(src => new clsAddress(
                 clsUtil.ToGuid(src.CountryID),
                 clsUtil.ToGuid(src.CityID),
                 clsUtil.ToGuid(src.DistrictID),
                 clsUtil.ToGuid(src.NeighborhoodID),
                 src.Notes
                  ));

          var UpdateMap=  CreateMap<UpdateAddressCommand, clsAddress>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap.AfterMap((src, dest) => {
                 dest.SetCountryID(clsUtil.ToGuid(src.CountryID));
                 dest.SetCityID(clsUtil.ToGuid(src.CityID));
                 dest.SetDistrictID(clsUtil.ToGuid(src.DistrictID));
                 dest.SetNeighborhoodID(clsUtil.ToGuid(src.NeighborhoodID));
                 dest.SetNotes(src.Notes);
             });
         
            
        }
    }
}
