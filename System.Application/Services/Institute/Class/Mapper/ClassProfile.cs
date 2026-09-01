using AutoMapper;
using System.Application.Common;
using System.Application.Services.Institute.Class.Command;
using System.Domain.Entities;

namespace System.Application.Services.Institute.Class.Mapper
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<NewClassCommand, clsClass>()
                  .ConstructUsing(src => new clsClass(src.ClassName, src.ClassLevel, src.ClassNotes,clsUtil.ToGuid( src.ProgramID))
                  );

            var UpdateMap = CreateMap<UpdateClassCommand, clsClass>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetName(src.ClassName);
                dest.SetNote(src.ClassNotes);
                dest.SetLevel(src.ClassLevel);
                dest.SetProgramID(clsUtil.ToGuid(src.ProgramID));
            }
                );
        }
    }
}
