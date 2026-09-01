using AutoMapper;
using System.Application.Common;
using System.Application.Services.Institute.Institute.Command;
using System.Domain.Entities.Institue;

namespace System.Application.Services.Person.Person.Mapper
{
    public class InstituteProfile : Profile
    {
        public InstituteProfile()
        {
            CreateMap<NewInstituteCommand, clsInstitute>()
                  .ConstructUsing(src => new clsInstitute(src.InstituteName, Guid.NewGuid(), src.InstituteNotes)
                  );

            var UpdateMap = CreateMap<UpdateInstituteCommand, clsInstitute>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetName(src.InstituteName);
                dest.SetNotes(src.InstituteNotes);
            }
                );
        }
    }
}
