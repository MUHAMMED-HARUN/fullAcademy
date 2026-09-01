
using AutoMapper;
using System.Application.Common;
using System.Application.Services.Address.Neighborhood.Command;
using System.Domain.Entities;

namespace System.Application.Services.Address.Neighborhood.Mapper
{
    public class NeighborhoodProfile : Profile
    {
        public NeighborhoodProfile()
        {
            CreateMap<NewNeighborhoodCommand, clsNeighborhood>()
                  .ConstructUsing(src => new clsNeighborhood(
                 clsUtil.ToGuid(src.DistrictID), src.NeighborhoodName
                  )); 

            CreateMap<UpdateNeighborhoodCommand, clsDistrict>()
                  .ConstructUsing(src => new clsDistrict(
                 clsUtil.ToGuid(src.DistrictID), src.NeighborhoodName
                  ));
        }
    }
}
