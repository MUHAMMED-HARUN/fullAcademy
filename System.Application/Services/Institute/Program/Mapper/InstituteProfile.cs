using AutoMapper;
using System.Application.Services.Institute.Institute.Command;
using System.Application.Services.Institute.Program.Command;
using System.Domain.Entities;
using System.Domain.Entities.Institue;

namespace System.Application.Services.Person.Person.Mapper
{
    public class ProgramProfile : Profile
    {
        public ProgramProfile()
        {
            CreateMap<NewProgramCommand, clsProgram>()
                  .ConstructUsing(src => new clsProgram(src.ProgramName, src.ProgramNote)
                  );

            var UpdateMap = CreateMap<UpdateProgramCommand, clsProgram>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetName(src.ProgramName);
                dest.SetNotes(src.ProgramNote);
            }
                );
        }
    }
}
