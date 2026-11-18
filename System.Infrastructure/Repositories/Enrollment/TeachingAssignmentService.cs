using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Common;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;
using System.Application.Services.Enrollment.TeachingAssignment.Query;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Enrollment
{
    public sealed class TeachingAssignmentService : ITeachingAssignmentRepository
    {
        private static readonly IGridifyMapper<TeachingAssignmentInfo> TeachingAssignmentFilterMapper =
            new GridifyMapper<TeachingAssignmentInfo>()
                .AddMap(nameof(TeachingAssignmentInfo.TeacherName), info => info.TeacherName)
                .AddMap(nameof(TeachingAssignmentInfo.GroupName), info => info.GroupName)
                .AddMap(nameof(TeachingAssignmentInfo.SubjectName), info => info.SubjectName)
                .AddMap(nameof(TeachingAssignmentInfo.EnrollmentDate), info => info.EnrollmentDate);

        private static readonly IGridifyMapper<TeacherGroupInfo> TeacherGroupFilterMapper =
            new GridifyMapper<TeacherGroupInfo>()
                .AddMap(nameof(TeacherGroupInfo.TeacherName), info => info.TeacherName)
                .AddMap(nameof(TeacherGroupInfo.GroupName), info => info.GroupName)
                .AddMap(nameof(TeacherGroupInfo.SubjectName), info => info.SubjectName)
                .AddMap(nameof(TeacherGroupInfo.EnrollmentDate), info => info.EnrollmentDate);

        private readonly AppDbContext _context;
        private readonly DbSet<clsTeachingAssignment> _set;
        private readonly ICurrentUserService _currentUser;

        public TeachingAssignmentService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = context.Set<clsTeachingAssignment>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsTeachingAssignment entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<TeachingAssignmentInfo?> GetBy(string param, enTeachingAssignmentType paramType)
        {
            var query = _set.AsNoTracking()
                .ApplyTeachingAssignmentReadScope(_currentUser, _context);

            query = paramType switch
            {
                enTeachingAssignmentType.ID => Guid.TryParse(param, out var id)
                    ? query.Where(item => item.Id == id)
                    : query.Where(_ => false),
                enTeachingAssignmentType.TeacherID => Guid.TryParse(param, out var teacherID)
                    ? query.Where(item => item.TeacherID == teacherID)
                    : query.Where(_ => false),
                enTeachingAssignmentType.GroupID => Guid.TryParse(param, out var groupID)
                    ? query.Where(item => item.GroupID == groupID)
                    : query.Where(_ => false),
                enTeachingAssignmentType.SubjectID => Guid.TryParse(param, out var subjectID)
                    ? query.Where(item => item.SubjectID == subjectID)
                    : query.Where(_ => false),
                _ => query.Where(_ => false),
            };

            return await ProjectTeachingAssignment(query)
                .OrderByDescending(info => info.EnrollmentDate)
                .ThenBy(info => info.TeacherName)
                .FirstOrDefaultAsync();
        }

        public async Task<clsTeachingAssignment?> GetEntityBy(string param, enTeachingAssignmentType paramType)
        {
            var query = _set.ApplyTeachingAssignmentReadScope(_currentUser, _context);

            query = paramType switch
            {
                enTeachingAssignmentType.ID => Guid.TryParse(param, out var id)
                    ? query.Where(item => item.Id == id)
                    : query.Where(_ => false),
                enTeachingAssignmentType.TeacherID => Guid.TryParse(param, out var teacherID)
                    ? query.Where(item => item.TeacherID == teacherID)
                    : query.Where(_ => false),
                enTeachingAssignmentType.GroupID => Guid.TryParse(param, out var groupID)
                    ? query.Where(item => item.GroupID == groupID)
                    : query.Where(_ => false),
                enTeachingAssignmentType.SubjectID => Guid.TryParse(param, out var subjectID)
                    ? query.Where(item => item.SubjectID == subjectID)
                    : query.Where(_ => false),
                _ => query.Where(_ => false),
            };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TeachingAssignmentInfo>> GetList()
        {
            var query = _set.AsNoTracking()
                .ApplyTeachingAssignmentReadScope(_currentUser, _context);

            return await ProjectTeachingAssignment(query).ToListAsync();
        }

        public async Task<Paging<TeachingAssignmentInfo>> GetListFilter(GetAllTeachingAssignmentQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking()
                .ApplyTeachingAssignmentReadScope(_currentUser, _context);

            return await ProjectTeachingAssignment(query)
                .GridifyAsync(request, TeachingAssignmentFilterMapper);
        }

        public async Task<Paging<TeacherGroupInfo>> GetTeacherGroups(GetTeacherGroupsQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking()
                .ApplyTeachingAssignmentReadScope(_currentUser, _context);

            return await ProjectTeacherGroup(query)
                .GridifyAsync(request, TeacherGroupFilterMapper);
        }

        public async Task<bool> IsExistBy(string param, enTeachingAssignmentType paramType)
        {
            var query = _set.AsNoTracking()
                .ApplyTeachingAssignmentReadScope(_currentUser, _context);

            return paramType switch
            {
                enTeachingAssignmentType.ID => Guid.TryParse(param, out var id) &&
                    await query.AnyAsync(item => item.Id == id),
                enTeachingAssignmentType.TeacherID => Guid.TryParse(param, out var teacherID) &&
                    await query.AnyAsync(item => item.TeacherID == teacherID),
                enTeachingAssignmentType.GroupID => Guid.TryParse(param, out var groupID) &&
                    await query.AnyAsync(item => item.GroupID == groupID),
                enTeachingAssignmentType.SubjectID => Guid.TryParse(param, out var subjectID) &&
                    await query.AnyAsync(item => item.SubjectID == subjectID),
                _ => false,
            };
        }

        public async Task<bool> IsTeacherEnrolled(
            string teacherID,
            string groupID,
            string subjectID,
            string? excludeTeachingAssignmentID = null)
        {
            if (!Guid.TryParse(teacherID, out var teacherGuid) ||
                !Guid.TryParse(groupID, out var groupGuid) ||
                !Guid.TryParse(subjectID, out var subjectGuid))
                return false;

            var query = _set.AsNoTracking()
                .ApplyTeachingAssignmentReadScope(_currentUser, _context)
                .Where(item =>
                    item.TeacherID == teacherGuid &&
                    item.GroupID == groupGuid &&
                    item.SubjectID == subjectGuid);

            if (Guid.TryParse(excludeTeachingAssignmentID, out var excludedID))
                query = query.Where(item => item.Id != excludedID);

            return await query.AnyAsync();
        }

        public Task<bool> Update(clsTeachingAssignment entity)
        {
            _set.Update(entity);
            return Task.FromResult(true);
        }

        private static IQueryable<TeachingAssignmentInfo> ProjectTeachingAssignment(
            IQueryable<clsTeachingAssignment> query)
        {
            return query.Select(item => new TeachingAssignmentInfo
            {
                EnrollmentID = item.Id.ToString(),
                TeacherID = item.TeacherID.ToString(),
                TeacherName = item.Teacher.Person.FirstName + " " +
                              item.Teacher.Person.FatherName + " " +
                              item.Teacher.Person.LastName,
                GroupID = item.GroupID.ToString(),
                GroupName = item.Group.GroupName,
                SubjectID = item.SubjectID.ToString(),
                SubjectName = item.Subject.Name,
                EnrollmentDate = item.Date,
            });
        }

        private static IQueryable<TeacherGroupInfo> ProjectTeacherGroup(
            IQueryable<clsTeachingAssignment> query)
        {
            return query.Select(item => new TeacherGroupInfo
            {
                EnrollmentID = item.Id.ToString(),
                TeacherID = item.TeacherID.ToString(),
                TeacherName = item.Teacher.Person.FirstName + " " +
                              item.Teacher.Person.FatherName + " " +
                              item.Teacher.Person.LastName,
                GroupID = item.GroupID.ToString(),
                GroupName = item.Group.GroupName,
                SubjectID = item.SubjectID.ToString(),
                SubjectName = item.Subject.Name,
                ClassName = item.Group.InstituteClass.Class.Name,
                ClassLevel = item.Group.InstituteClass.Class.Level,
                EnrollmentDate = item.Date,
            });
        }
    }
}