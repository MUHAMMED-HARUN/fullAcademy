

using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public class StageInfoService : IStageInfoRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsStageInfo> _set;
        private readonly DbSet<clsStudentStage> _studentStage;

        public StageInfoService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsStageInfo>();
            _studentStage = _context.Set<clsStudentStage>();
        }
       private static string GetProgressType(enProgressType type)
        {
            switch(type)
            {
                case enProgressType.Save:
                    return "حفظ";
                case enProgressType.Write:
                    return "كتابة";
                case enProgressType.Read:
                    return "قرائة";
            }
            return "خطاء";
        }
        private static string GetUniteTypeType(enUniteType type)
        {
            switch (type)
            {
                case enUniteType.Unite:
                    return "جزء";
                case enUniteType.Page:
                    return "صفحة";
            }
            return "خطا";
        }
        public async Task<string> Add(clsStageInfo entity)
        {
          await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<StageInfoInfo> GetBy(string param, enStageInfoParamType paramType)
        {
                return await _set.Where(x => x.Id == clsUtil.ToGuid(param)).
                             Select(x => new StageInfoInfo
                             {
                              StageInfoID = x.Id.ToString(),
                              From = x.From,
                              To = x.To,
                              MatterID = x.MatterID.ToString(),
                              MatterName  =x.Matter.Name,
                              ProgressTypeName= GetProgressType(x.ProgressType),
                              ProgressType= (enProgressType)x.ProgressType,
                              StageID = x.StageID.ToString(),
                              StageName = x.Stage.Name,
                              UniteTypeName =GetUniteTypeType(x.UniteType),
                              UniteType =(enUniteType)x.UniteType
                             }
                            ).FirstOrDefaultAsync();

        }

        public async Task<clsStageInfo> GetEntityBy(string param, enStageInfoParamType paramType)
        {
                return await _set.Where(x => x.Id == clsUtil.ToGuid(param))
                            .FirstOrDefaultAsync();
        }

        public async Task<List<StageInfoInfo>> GetList()
        {
            return await _set.Select(x => new StageInfoInfo
            {
                StageInfoID = x.Id.ToString(),
                From = x.From,
                To = x.To,
                MatterID = x.MatterID.ToString(),
                MatterName = x.Matter.Name,
                ProgressTypeName = GetProgressType(x.ProgressType),
                StageID = x.StageID.ToString(),
                StageName = x.Stage.Name,
                UniteTypeName = GetUniteTypeType(x.UniteType)
            }
                            ).ToListAsync();
        }

        public async Task<List<StageInfoInfo>> GetStudentStageInfo(string StudentEnrollmentID)
        {
         return await   _set.Join(_context.Stages,
                stageinfo => stageinfo.StageID,
                stage => stage.Id,
                (stageinfo, stage) =>new { stageinfo , stage })
                .Join(_context.StudentStages,
                x=>x.stage.Id,
                studentStage=> studentStage.StageID,
                (x, studentStage) =>new {x.stageinfo,x.stage, studentStage })
                    .Where(x=>x.studentStage.EnrollmentStudentID==clsUtil.ToGuid(StudentEnrollmentID))
                .Select(x =>  new StageInfoInfo
            {
                StageInfoID = x.stageinfo.Id.ToString(),
                From = x.stageinfo.From,
                To = x.stageinfo.To,
                MatterID = x.stageinfo.MatterID.ToString(),
                MatterName = x.stageinfo.Matter.Name,
                ProgressTypeName = GetProgressType(x.stageinfo.ProgressType),
                StageID = x.stageinfo.StageID.ToString(),
                StageName = x.stageinfo.Stage.Name,
                UniteTypeName = GetUniteTypeType(x.stageinfo.UniteType)
            }
                                       ).ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enStageInfoParamType paramType)
        {
                return await _set.AnyAsync(x => x.Id == clsUtil.ToGuid(param)) ;
        }

        public async Task<bool> Update(clsStageInfo entity)
        {
            _set.Update(entity);
            return true;
        }
    }
}
