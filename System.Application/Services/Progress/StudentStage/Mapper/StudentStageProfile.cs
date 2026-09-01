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
                  clsUtil.ToGuid(src.EnrollmentStudentID), clsUtil.ToGuid(src.StageID)
                  , src.StudentStageDate)
                  );

            var UpdateMap = CreateMap<UpdateStudentStageCommand, clsStudentStage>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetID(clsUtil.ToGuid(src.StudentStageID));
                dest.SetEnrollmentStudentID(clsUtil.ToGuid(src.EnrollmentStudentID));
                dest.SetStageID(clsUtil.ToGuid(src.StageID));
                dest.SetDate(src.StudentStageDate);
                dest.SetDate(src.StudentStageDate);
            }
                );
        }
    }
}
