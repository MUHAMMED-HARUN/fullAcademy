using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Services.Assignment.Result.DTO;
using System.Application.Services.Assignment.Result.Query;
using System.Domain.Entities;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public sealed class ResultService : IResultRepository
    {
        private static readonly IGridifyMapper<ResultInfo> ResultFilterMapper =
            new GridifyMapper<ResultInfo>()
                .AddMap(nameof(ResultInfo.StudentName), info => info.StudentName)
                .AddMap(nameof(ResultInfo.AcademicPlanName), info => info.AcademicPlanName)
                .AddMap(nameof(ResultInfo.AssignmentScopeName), info => info.AssignmentScopeName)
                .AddMap(nameof(ResultInfo.StageInfoMatter), info => info.StageInfoMatter)
                .AddMap(nameof(ResultInfo.Score), info => info.Score);

        private readonly AppDbContext _context;
        private readonly DbSet<clsResult> _set;
        private readonly ICurrentUserService _currentUser;

        public ResultService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = context.Set<clsResult>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsResult entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public Task<bool> Update(clsResult entity)
        {
            _set.Update(entity);
            return Task.FromResult(true);
        }

        public async Task<Paging<ResultInfo>> GetListFilter(GetAllResultQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking()
                .ApplyResultReadScope(_currentUser, _context);
            if (!string.IsNullOrWhiteSpace(request.AssignmentID))
            {
                if (!Guid.TryParse(request.AssignmentID, out var assignmentID) || assignmentID == Guid.Empty)
                    return new Paging<ResultInfo>();
                query = query.Where(result => result.AssignmentID == assignmentID);
            }

            return await ProjectResult(query).GridifyAsync(request, ResultFilterMapper);
        }

        public async Task<ResultInfo?> GetBy(Guid id)
        {
            var query = _set.AsNoTracking()
                .ApplyResultReadScope(_currentUser, _context)
                .Where(result => result.Id == id);
            return await ProjectResult(query).FirstOrDefaultAsync();
        }

        public Task<ResultInfo?> GetForUpsert(Guid id) => GetBy(id);

        public async Task<clsResult?> GetEntityBy(Guid id)
        {
            return await _set.ApplyResultReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(result => result.Id == id);
        }

        public async Task<List<clsResult>> GetListByAssignmentID(Guid assignmentID)
        {
            return await _set.AsNoTracking()
                .ApplyResultReadScope(_currentUser, _context)
                .Where(result => result.AssignmentID == assignmentID)
                .ToListAsync();
        }

        public async Task<bool> HasResult(Guid assignmentID, Guid assignmentScopeInfoID)
        {
            return await _set.AsNoTracking()
                .ApplyResultReadScope(_currentUser, _context)
                .AnyAsync(result => result.AssignmentID == assignmentID &&
                    result.AssignmentScopeInfoID == assignmentScopeInfoID &&
                    result.IsActive && result.DeletedAt == null);
        }

        private static IQueryable<ResultInfo> ProjectResult(IQueryable<clsResult> query)
        {
            return query.Select(result => new ResultInfo
            {
                ResultID = result.Id.ToString(),
                AssignmentID = result.AssignmentID.ToString(),
                StudentAcademicEnrollmentID = result.Assignment.AssignmentNomination.StudentAcademicEnrollmentID.ToString(),
                StudentName = result.Assignment.AssignmentNomination.StudentAcademicEnrollment.Student.Person.FirstName + " " +
                    result.Assignment.AssignmentNomination.StudentAcademicEnrollment.Student.Person.FatherName + " " +
                    result.Assignment.AssignmentNomination.StudentAcademicEnrollment.Student.Person.LastName,
                AcademicPlanName = result.Assignment.AssignmentNomination.StudentAcademicEnrollment.AcademicPlan.Name,
                AssignmentScopeName = result.Assignment.AssignmentNomination.AssignmentScope.Name,
                AssignmentScopeInfoID = result.AssignmentScopeInfoID.ToString(),
                StageInfoMatter = result.Assignment.AssignmentNomination.StudentAcademicEnrollment.Items
                    .Where(item => item.StageInfoID == result.AssignmentScopeInfo.StageInfoID)
                    .Select(item => item.StageName + " — " + item.MatterName)
                    .FirstOrDefault() ?? string.Empty,
                MaxScore = result.AssignmentScopeInfo.MaxScore,
                MinPassScore = result.AssignmentScopeInfo.MinPassScore,
                Score = result.Score,
                Notes = result.Notes,
            });
        }
    }
}
