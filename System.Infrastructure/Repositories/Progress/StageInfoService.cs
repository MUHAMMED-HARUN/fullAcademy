using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.DTO;
using System.Application.Services.Progress.StageInfo.Query;
using System.Domain.Entities;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public sealed class StageInfoService : IStageInfoRepository
    {
        private static readonly IGridifyMapper<StageInfoInfo> StageInfoFilterMapper =
            new GridifyMapper<StageInfoInfo>()
                .AddMap(nameof(StageInfoInfo.StageName), info => info.StageName)
                .AddMap(nameof(StageInfoInfo.MatterName), info => info.MatterName)
                .AddMap(nameof(StageInfoInfo.UniteTypeName), info => info.UniteTypeName)
                .AddMap(nameof(StageInfoInfo.ProgressTypeName), info => info.ProgressTypeName);

        private readonly AppDbContext _context;
        private readonly DbSet<clsStageInfo> _set;
        private readonly ICurrentUserService _currentUser;

        public StageInfoService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = context.Set<clsStageInfo>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsStageInfo entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<StageInfoInfo?> GetBy(string param, enStageInfoParamType paramType)
        {
            if (paramType != enStageInfoParamType.ID)
                return null;

            var query = _set.AsNoTracking().ApplyStageInfoReadScope(_currentUser, _context)
                .Where(info => info.Id == clsUtil.ToGuid(param));
            return await ProjectStageInfo(query).FirstOrDefaultAsync();
        }

        public async Task<clsStageInfo?> GetEntityBy(string param, enStageInfoParamType paramType)
        {
            if (paramType != enStageInfoParamType.ID)
                return null;

            return await _set.ApplyStageInfoReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(info => info.Id == clsUtil.ToGuid(param));
        }

        public async Task<List<StageInfoInfo>> GetList()
        {
            var query = _set.AsNoTracking().ApplyStageInfoReadScope(_currentUser, _context);
            return await ProjectStageInfo(query).ToListAsync();
        }

        public async Task<Paging<StageInfoInfo>> GetListFilter(GetAllStageInfoFilterQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking().ApplyStageInfoReadScope(_currentUser, _context);
            if (Guid.TryParse(request.StageID, out var stageID) && stageID != Guid.Empty)
                query = query.Where(info => info.StageID == stageID);

            return await ProjectStageInfo(query).GridifyAsync(request, StageInfoFilterMapper);
        }

        public async Task<List<StageInfoInfo>> GetStudentStageInfo(string studentAcademicEnrollmentID)
        {
            if (!Guid.TryParse(studentAcademicEnrollmentID, out var enrollmentID) || enrollmentID == Guid.Empty)
                return new List<StageInfoInfo>();

            var enrollment = await _context.StudentAcademicEnrollments.AsNoTracking()
                .Include(enrollment => enrollment.Items)
                .ApplyStudentAcademicEnrollmentReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(enrollment => enrollment.Id == enrollmentID);

            if (enrollment is null)
                return new List<StageInfoInfo>();

            var items = enrollment.Items;

            // For teachers, restrict to the student's current stage only.
            if (!_currentUser.IsAdmin &&
                string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
            {
                var currentStageID = await _context.StudentStages.AsNoTracking()
                    .Where(stage => stage.StudentAcademicEnrollmentID == enrollmentID &&
                                    stage.State == enStageStateType.procecing)
                    .Select(stage => stage.StageID)
                    .FirstOrDefaultAsync();

                if (currentStageID == Guid.Empty)
                    return new List<StageInfoInfo>();

                items = items.Where(item => item.StageID == currentStageID).ToList();
            }

            return items.Select(item => new StageInfoInfo
            {
                StageInfoID = item.StageInfoID.ToString(),
                From = item.From,
                To = item.To,
                StageID = item.StageID.ToString(),
                StageName = item.StageName,
                MatterID = item.MatterID.ToString(),
                MatterName = item.MatterName,
                UniteType = item.UniteType,
                UniteTypeName = item.UniteTypeName,
                ProgressType = item.ProgressType,
                ProgressTypeName = item.ProgressTypeName,
            }).ToList();
        }

        public async Task<bool> IsExistBy(string param, enStageInfoParamType paramType)
        {
            if (paramType != enStageInfoParamType.ID)
                return false;

            return await _set.AsNoTracking()
                .ApplyStageInfoReadScope(_currentUser, _context)
                .AnyAsync(info => info.Id == clsUtil.ToGuid(param));
        }

        public Task<bool> Update(clsStageInfo entity)
        {
            _set.Update(entity);
            return Task.FromResult(true);
        }

        private static IQueryable<StageInfoInfo> ProjectStageInfo(IQueryable<clsStageInfo> query)
        {
            return query.Select(info => new StageInfoInfo
            {
                StageInfoID = info.Id.ToString(),
                From = info.From,
                To = info.To,
                StageID = info.StageID.ToString(),
                StageName = info.Stage.Name,
                MatterID = info.MatterID.ToString(),
                MatterName = info.Matter.Name,
                UniteType = info.UniteType,
                UniteTypeName = info.UniteType == enUniteType.Page ? "صفحة" : "جزء",
                ProgressType = info.ProgressType,
                ProgressTypeName = info.ProgressType == enProgressType.Read
                    ? "قراءة"
                    : info.ProgressType == enProgressType.Save
                        ? "حفظ"
                        : "كتابة",
            });
        }
    }
}
