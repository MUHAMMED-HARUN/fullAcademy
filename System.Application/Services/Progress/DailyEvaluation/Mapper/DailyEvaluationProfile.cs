using AutoMapper;
using System.Application.Common;
using System.Application.Services.Progress.DailyEvaluation.Command;
using System.Domain.Entities;

namespace System.Application.Services.Progress.DailyEvaluation.Mapper
{
    public class DailyEvaluationProfile : Profile
    {
        public DailyEvaluationProfile()
        {
            CreateMap<NewDailyEvaluationCommand, clsDailyEvaluation>()
                  .ConstructUsing(src => new clsDailyEvaluation(clsUtil.ToGuid(src.StudentEnrollmentID),
                  src.Date, 0, src.To, clsUtil.ToGuid(src.StageInfoID), src.LevelType));

            var UpdateMap = CreateMap<UpdateDailyEvaluationCommand, clsDailyEvaluation>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetTo(src.To);
                dest.SetLevelType(src.LevelType);
            }
                );
        }
    }
}
