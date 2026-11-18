using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.DailyEvaluation.DTO;
using System.Application.Services.Progress.DailyEvaluation.Query;
using System.Domain.Entities;
using System.Domain.Enums.Person;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public sealed class DailyEvaluationService : IDailyEvaluationRepository
    {
        private static readonly IGridifyMapper<DailyEvaluationInfo> DailyEvaluationFilterMapper =
            new GridifyMapper<DailyEvaluationInfo>()
                .AddMap(nameof(DailyEvaluationInfo.StudentName), info => info.StudentName)
                .AddMap(nameof(DailyEvaluationInfo.AcademicPlanName), info => info.AcademicPlanName)
                .AddMap(nameof(DailyEvaluationInfo.StageInfoName), info => info.StageInfoName)
                .AddMap(nameof(DailyEvaluationInfo.Date), info => info.Date)
                .AddMap(nameof(DailyEvaluationInfo.LevelType), info => info.LevelType);

        private readonly AppDbContext _context;
        private readonly DbSet<clsDailyEvaluation> _set;
        private readonly ICurrentUserService _currentUser;

        public DailyEvaluationService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = context.Set<clsDailyEvaluation>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsDailyEvaluation entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<DailyEvaluationInfo?> GetBy(string param, enDailyEvaluationParamType paramType)
        {
            var query = _set.AsNoTracking().ApplyDailyEvaluationReadScope(_currentUser, _context);
            if (paramType == enDailyEvaluationParamType.ID && Guid.TryParse(param, out var evaluationID))
                query = query.Where(evaluation => evaluation.Id == evaluationID);
            else if (paramType == enDailyEvaluationParamType.StudentAcademicEnrollment && Guid.TryParse(param, out var enrollmentID))
                query = query.Where(evaluation => evaluation.StudentAcademicEnrollmentID == enrollmentID);
            else
                return null;

            return await ProjectDailyEvaluation(query)
                .OrderByDescending(info => info.Date)
                .ThenByDescending(info => info.To)
                .FirstOrDefaultAsync();
        }

        public async Task<clsDailyEvaluation?> GetEntityBy(string param, enDailyEvaluationParamType paramType)
        {
            var query = _set.ApplyDailyEvaluationReadScope(_currentUser, _context);
            if (paramType != enDailyEvaluationParamType.ID ||
                !Guid.TryParse(param, out var evaluationID))
                return null;

            return await query.FirstOrDefaultAsync(evaluation => evaluation.Id == evaluationID);
        }

        public async Task<List<DailyEvaluationInfo>> GetList()
        {
            var query = _set.AsNoTracking().ApplyDailyEvaluationReadScope(_currentUser, _context);
            return await ProjectDailyEvaluation(query).ToListAsync();
        }

        public async Task<Paging<DailyEvaluationInfo>> GetAllFilter(GetAllDailyEvaluationQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking().ApplyDailyEvaluationReadScope(_currentUser, _context);
            if (!string.IsNullOrWhiteSpace(request.StudentAcademicEnrollmentID))
            {
                if (!Guid.TryParse(request.StudentAcademicEnrollmentID, out var enrollmentID) || enrollmentID == Guid.Empty)
                    return new Paging<DailyEvaluationInfo>();
                query = query.Where(evaluation => evaluation.StudentAcademicEnrollmentID == enrollmentID);
            }
            return await ProjectDailyEvaluation(query)
                .GridifyAsync(request, DailyEvaluationFilterMapper);
        }

        public async Task<DailyEvaluationInfo?> GetLastDailyEvaluationInfo(
            string studentAcademicEnrollmentID,
            string stageInfoID)
        {
            if (!Guid.TryParse(studentAcademicEnrollmentID, out var enrollmentGuid) ||
                !Guid.TryParse(stageInfoID, out var stageInfoGuid))
                return null;

            var query = _set.AsNoTracking()
                .ApplyDailyEvaluationReadScope(_currentUser, _context)
                .Where(evaluation => evaluation.StudentAcademicEnrollmentID == enrollmentGuid &&
                                     evaluation.StageInfoID == stageInfoGuid);

            return await ProjectDailyEvaluation(query)
                .OrderByDescending(info => info.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<clsDailyEvaluation?> GetLastDailyEvaluation(
            string studentAcademicEnrollmentID,
            string stageInfoID,
            string? excludeDailyEvaluationID = null)
        {
            if (!Guid.TryParse(studentAcademicEnrollmentID, out var enrollmentGuid) ||
                !Guid.TryParse(stageInfoID, out var stageInfoGuid))
                return null;

            var query = _set.AsNoTracking()
                .ApplyDailyEvaluationReadScope(_currentUser, _context)
                .Where(evaluation => evaluation.StudentAcademicEnrollmentID == enrollmentGuid &&
                                     evaluation.StageInfoID == stageInfoGuid);

            if (Guid.TryParse(excludeDailyEvaluationID, out var excludedID))
                query = query.Where(evaluation => evaluation.Id != excludedID);

            return await query.OrderByDescending(evaluation => evaluation.To)
                .ThenByDescending(evaluation => evaluation.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistBy(string param, enDailyEvaluationParamType paramType)
        {
            var query = _set.AsNoTracking().ApplyDailyEvaluationReadScope(_currentUser, _context);
            if (paramType == enDailyEvaluationParamType.ID && Guid.TryParse(param, out var evaluationID))
                return await query.AnyAsync(evaluation => evaluation.Id == evaluationID);
            if (paramType == enDailyEvaluationParamType.StudentAcademicEnrollment && Guid.TryParse(param, out var enrollmentID))
                return await query.AnyAsync(evaluation => evaluation.StudentAcademicEnrollmentID == enrollmentID);
            return false;
        }

        public Task<bool> Update(clsDailyEvaluation entity)
        {
            _set.Update(entity);
            return Task.FromResult(true);
        }

        private static IQueryable<DailyEvaluationInfo> ProjectDailyEvaluation(
            IQueryable<clsDailyEvaluation> query)
        {
            return query.Select(evaluation => new DailyEvaluationInfo
            {
                DailyEvaluationID = evaluation.Id.ToString(),
                StudentAcademicEnrollmentID = evaluation.StudentAcademicEnrollmentID.ToString(),
                StudentName = evaluation.StudentAcademicEnrollment.Student.Person.FirstName + " " +
                              evaluation.StudentAcademicEnrollment.Student.Person.FatherName + " " +
                              evaluation.StudentAcademicEnrollment.Student.Person.LastName,
                AcademicPlanName = evaluation.StudentAcademicEnrollment.AcademicPlan.Name,
                StageInfoID = evaluation.StageInfoID.ToString(),
                StageInfoName = evaluation.StudentAcademicEnrollment.Items
                    .Where(item => item.StageInfoID == evaluation.StageInfoID)
                    .Select(item => item.StageName + " — " + item.MatterName)
                    .FirstOrDefault() ?? string.Empty,
                Date = evaluation.Date,
                From = evaluation.From,
                To = evaluation.To,
                LevelType = evaluation.LevelType == enLevelType.VeryBad ? "سيء جدا" :
                    evaluation.LevelType == enLevelType.Bad ? "سيء" :
                    evaluation.LevelType == enLevelType.Good ? "جيد" :
                    evaluation.LevelType == enLevelType.VeryGood ? "جيد جدا" :
                    evaluation.LevelType == enLevelType.Excilent ? "ممتاز" : "غير معروف",
            });
        }
    }
}
