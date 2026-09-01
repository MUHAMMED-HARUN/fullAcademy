using AutoMapper;
using System.Application.Common;
using System.Application.Services.Institute.InstituteClass.Command;
using System.Application.Services.Institute.Program.Command;
using System.Domain.Entities;

namespace System.Application.Services.Institute.InstituteClass.Mapper
{
    public class InstituteClassProfile : Profile
    {
        public InstituteClassProfile()
        {
            CreateMap<NewInstituteClassCommand, clsInstituteClass>()
                  .ConstructUsing(src => new clsInstituteClass(clsUtil.ToGuid( src.InstituteID), clsUtil.ToGuid( src.ClassID))
                  );

            var UpdateMap = CreateMap<UpdateInstituteClassCommand, clsInstituteClass>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetClassID(clsUtil.ToGuid(src.ClassID));
                dest.SetInstituteID(clsUtil.ToGuid(src.InstituteID));
            }
                );
        }
    }
}
