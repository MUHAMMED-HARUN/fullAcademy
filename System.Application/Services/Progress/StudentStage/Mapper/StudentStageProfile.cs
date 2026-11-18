using AutoMapper;
using System.Application.Common;
using System.Application.Services.Progress.StudentStage.Command;
using System.Domain.Entities;

namespace System.Application.Services.Progress.StudentStage.Mapper
{
    public class StudentStageProfile : Profile
    {
        public StudentStageProfile()
        {
            CreateMap<NewStudentStageCommand, clsStudentStage>()
                  .ConstructUsing(src => new clsStudentStage(src.StudentStageState,
                  clsUtil.ToGuid(src.StudentAcademicEnrollmentID), clsUtil.ToGuid(src.StageID)
                  , src.StudentStageDate)
                  );

            var UpdateMap = CreateMap<UpdateStudentStageCommand, clsStudentStage>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetState(src.StudentStageState);
                dest.SetDate(src.StudentStageDate);
            }
                );
        }
    }
}
