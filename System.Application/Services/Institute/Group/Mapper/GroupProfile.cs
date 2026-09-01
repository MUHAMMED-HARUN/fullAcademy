using AutoMapper;
using System.Application.Common;
using System.Application.Services.Institute.Group.Command;
using System.Domain.Entities;

namespace System.Application.Services.Institute.Group.Mapper
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<NewGroupCommand, clsGroup>()
                  .ConstructUsing(src => new clsGroup(clsUtil.ToGuid(src.InstituteClassID), src.GroupName, src.Code)
                  );

            var UpdateMap = CreateMap<UpdateGroupCommand, clsGroup>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetInstituteClassID(clsUtil.ToGuid(src.InstituteClassID));
                dest.SetGroupName(src.GroupName);
                dest.SetCode(src.Code);
            }
                );
        }
    }
}
