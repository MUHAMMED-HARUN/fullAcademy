using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Services.Progress.StudentStage.DTO;
using System.Application.Services.Progress.StudentStage.Query;
using System.Domain.Entities;
using System.Domain.Enums.Person;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public sealed class StudentStageService : IStudentStageRepository
    {
        private static readonly IGridifyMapper<StudentStageInfo> StudentStageFilterMapper =
            new GridifyMapper<StudentStageInfo>()
                .AddMap(nameof(StudentStageInfo.StudentName), item => item.StudentName)
                .AddMap(nameof(StudentStageInfo.AcademicPlanName), item => item.AcademicPlanName)
                .AddMap(nameof(StudentStageInfo.StageName), item => item.StageName)
                .AddMap(nameof(StudentStageInfo.StudentStageState), item => item.StudentStageState)
                .AddMap(nameof(StudentStageInfo.StudentStageDate), item => item.StudentStageDate);

        private readonly AppDbContext _context;
        private readonly DbSet<clsStudentStage> _set;
        private readonly ICurrentUserService _currentUser;

        public StudentStageService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = context.Set<clsStudentStage>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsStudentStage entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<StudentStageInfo?> GetBy(string param, enStudentStageParamType paramType)
        {
            if (paramType != enStudentStageParamType.ID || !Guid.TryParse(param, out var id) || id == Guid.Empty)
                return null;

            return await ProjectStudentStages(
                    _set.AsNoTracking().ApplyStudentStageReadScope(_currentUser, _context)
                        .Where(item => item.Id == id))
                .FirstOrDefaultAsync();
        }

        public async Task<clsStudentStage?> GetEntityBy(string param, enStudentStageParamType paramType)
        {
            if (paramType != enStudentStageParamType.ID || !Guid.TryParse(param, out var id) || id == Guid.Empty)
                return null;

            return await _set.ApplyStudentStageReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<clsStudentStage?> GetEntityBy(
            string studentAcademicEnrollmentID,
            string stageID,
            enStageStateType state)
        {
            if (!Guid.TryParse(studentAcademicEnrollmentID, out var enrollmentID) || enrollmentID == Guid.Empty ||
                !Guid.TryParse(stageID, out var requestedStageID) || requestedStageID == Guid.Empty)
                return null;

            return await _set.ApplyStudentStageReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(item =>
                    item.StudentAcademicEnrollmentID == enrollmentID &&
                    item.StageID == requestedStageID &&
                    item.State == state);
        }

        public async Task<Guid?> GetPreviousStageID(Guid stageID)
        {
            if (stageID == Guid.Empty)
                return null;

            return await _context.Stages.AsNoTracking()
                .Where(stage => stage.Id == stageID)
                .Select(stage => stage.PrevStageID)
                .FirstOrDefaultAsync();
        }

        public async Task<Paging<StudentStageInfo>> GetListFilter(GetAllStudentStageQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking()
                .ApplyStudentStageReadScope(_currentUser, _context);
            if (!string.IsNullOrWhiteSpace(request.StudentAcademicEnrollmentID))
            {
                if (!Guid.TryParse(request.StudentAcademicEnrollmentID, out var enrollmentID) || enrollmentID == Guid.Empty)
                    return new Paging<StudentStageInfo>();
                query = query.Where(item => item.StudentAcademicEnrollmentID == enrollmentID);
            }

            return await ProjectStudentStages(query)
                .GridifyAsync(request, StudentStageFilterMapper);
        }

        public async Task<bool> IsExistBy(string param, enStudentStageParamType paramType)
        {
            if (paramType != enStudentStageParamType.ID || !Guid.TryParse(param, out var id) || id == Guid.Empty)
                return false;

            return await _set.AsNoTracking()
                .ApplyStudentStageReadScope(_currentUser, _context)
                .AnyAsync(item => item.Id == id);
        }

        public Task<bool> Update(clsStudentStage entity)
        {
            _set.Update(entity);
            return Task.FromResult(true);
        }

        private static IQueryable<StudentStageInfo> ProjectStudentStages(IQueryable<clsStudentStage> query)
        {
            return query.Select(item => new StudentStageInfo
            {
                StudentStageID = item.Id.ToString(),
                StudentAcademicEnrollmentID = item.StudentAcademicEnrollmentID.ToString(),
                StudentName = item.StudentAcademicEnrollment.Student.Person.FirstName + " " +
                              item.StudentAcademicEnrollment.Student.Person.FatherName + " " +
                              item.StudentAcademicEnrollment.Student.Person.LastName,
                AcademicPlanName = item.StudentAcademicEnrollment.AcademicPlan.Name,
                StageID = item.StageID.ToString(),
                StageName = item.StudentAcademicEnrollment.Items
                    .Where(snapshot => snapshot.StageID == item.StageID)
                    .Select(snapshot => snapshot.StageName)
                    .FirstOrDefault() ?? string.Empty,
                StudentStageState = item.State == enStageStateType.canceled ? "تم الإلغاء" :
                    item.State == enStageStateType.completed ? "مكتمل" :
                    item.State == enStageStateType.failed ? "فشل" :
                    item.State == enStageStateType.procecing ? "قيد المعالجة" : "غير معروف",
                StudentStageStateValue = (int)item.State,
                StudentStageDate = item.Date,
            });
        }
    }
}
