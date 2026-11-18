using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;
using System.Application.Services.Institute.Group.Query;
using System.Domain.Entities;
using System.Domain.Enums.Institute;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Institute.Institute
{
    public class GroupService : IGroupRepository
    {
        private static readonly IGridifyMapper<GroupInfo> GroupFilterMapper =
            new GridifyMapper<GroupInfo>()
                .AddMap(nameof(GroupInfo.GroupName), group => group.GroupName)
                .AddMap(nameof(GroupInfo.Code), group => group.Code)
                .AddMap(nameof(GroupInfo.ClassName), group => group.ClassName)
                .AddMap(nameof(GroupInfo.InstituteName), group => group.InstituteName);

        private readonly AppDbContext _context;
        private readonly DbSet<clsGroup> _dbSet;
        private readonly ICurrentUserService _currentUser;

        public GroupService(AppDbContext appDbContext, ICurrentUserService currentUser)
        {
            _context = appDbContext;
            _dbSet = _context.Set<clsGroup>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsGroup entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<GroupInfo?> GetBy(string param, enGroupType paramType)
        {
            if (string.IsNullOrWhiteSpace(param))
                return null;

            var query = _dbSet.AsNoTracking()
                .ApplyGroupReadScope(_currentUser, _context);

            query = paramType switch
            {
                enGroupType.ID when Guid.TryParse(param, out var groupID) =>
                    query.Where(group => group.Id == groupID),
                enGroupType.Name => query.Where(group => group.GroupName == param),
                enGroupType.Code => query.Where(group => group.Code == param),
                _ => null,
            };

            if (query == null)
                return null;

            return await query.Select(group => new GroupInfo
            {
                GroupID = group.Id.ToString(),
                GroupName = group.GroupName,
                Code = group.Code,
                InstituteClassID = group.InstituteClassID.ToString(),
                ClassName = group.InstituteClass.Class.Name,
                InstituteName = group.InstituteClass.Institute.Name,
                HasActiveRelationships = _context.StudentEnrollments.Any(enrollment =>
                    enrollment.GroupID == group.Id && enrollment.IsActive) ||
                    _context.TeachingAssignments.Any(assignment =>
                        assignment.GroupID == group.Id && assignment.IsActive),
            }).FirstOrDefaultAsync();
        }

        public async Task<clsGroup?> GetEntityBy(string param, enGroupType paramType)
        {
            if (string.IsNullOrWhiteSpace(param))
                return null;

            var query = _dbSet.ApplyGroupReadScope(_currentUser, _context);
            query = paramType switch
            {
                enGroupType.ID when Guid.TryParse(param, out var groupID) =>
                    query.Where(group => group.Id == groupID),
                enGroupType.Name => query.Where(group => group.GroupName == param),
                enGroupType.Code => query.Where(group => group.Code == param),
                _ => null,
            };

            return query == null ? null : await query.FirstOrDefaultAsync();
        }

        public async Task<List<GroupInfo>> GetList()
        {
            var query = _dbSet.AsNoTracking()
                .ApplyGroupReadScope(_currentUser, _context);

            return await query.Select(group => new GroupInfo
                {
                    GroupID = group.Id.ToString(),
                    GroupName = group.GroupName,
                    Code = group.Code,
                    InstituteClassID = group.InstituteClassID.ToString(),
                    ClassName = group.InstituteClass.Class.Name,
                    InstituteName = group.InstituteClass.Institute.Name,
                    HasActiveRelationships = _context.StudentEnrollments.Any(enrollment =>
                        enrollment.GroupID == group.Id && enrollment.IsActive) ||
                        _context.TeachingAssignments.Any(assignment =>
                            assignment.GroupID == group.Id && assignment.IsActive),
                }).ToListAsync();
        }

        public async Task<Paging<GroupInfo>> GetListFilter(GetAllGroupQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);
            var query = _dbSet.AsNoTracking()
                .ApplyGroupReadScope(_currentUser, _context)
                .Select(group => new GroupInfo
                {
                    GroupID = group.Id.ToString(),
                    GroupName = group.GroupName,
                    Code = group.Code,
                    InstituteClassID = group.InstituteClassID.ToString(),
                    ClassName = group.InstituteClass.Class.Name,
                    InstituteName = group.InstituteClass.Institute.Name,
                    HasActiveRelationships = _context.StudentEnrollments.Any(enrollment =>
                        enrollment.GroupID == group.Id && enrollment.IsActive) ||
                        _context.TeachingAssignments.Any(assignment =>
                            assignment.GroupID == group.Id && assignment.IsActive),
                });

            return await query.GridifyAsync(request, GroupFilterMapper);
        }

        public async Task<bool> IsExistBy(string param, enGroupType paramType)
        {
            if (string.IsNullOrWhiteSpace(param))
                return false;

            var query = _dbSet.AsNoTracking()
                .ApplyGroupReadScope(_currentUser, _context);
            query = paramType switch
            {
                enGroupType.ID when Guid.TryParse(param, out var groupID) =>
                    query.Where(group => group.Id == groupID),
                enGroupType.Name => query.Where(group => group.GroupName == param),
                enGroupType.Code => query.Where(group => group.Code == param),
                _ => null,
            };

            return query != null && await query.AnyAsync();
        }

        public async Task<bool> IsAvalibleName(string name, string InstituteClassID)
        {
            return await _dbSet.AnyAsync(c => c.GroupName == name && c.InstituteClassID == clsUtil.ToGuid(InstituteClassID));
        }

        public async Task<bool> IsAvalibleCode(string code, string InstituteClassID)
        {
            return await _dbSet.AnyAsync(c => c.Code == code && c.InstituteClassID == clsUtil.ToGuid(InstituteClassID));
        }

        public async Task<bool> Update(clsGroup entity)
        {
            _dbSet.Update(entity);
            return true;
        }

        public async Task<Paging<GroupInfo>> GetMyTeachingGroups(GetMyTeachingGroupsQuery request)
        {
            if (!_currentUser.IsAuthenticated || _currentUser.IsAdmin ||
                !string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) ||
                _currentUser.EntityGuid is not Guid teacherID || teacherID == Guid.Empty)
                return new Paging<GroupInfo>();

            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);
            var query = _dbSet.AsNoTracking()
                .ApplyGroupReadScope(_currentUser, _context)
                .Select(group => new GroupInfo
                {
                    GroupID = group.Id.ToString(),
                    GroupName = group.GroupName,
                    Code = group.Code,
                    InstituteClassID = group.InstituteClassID.ToString(),
                    ClassName = group.InstituteClass.Class.Name,
                    InstituteName = group.InstituteClass.Institute.Name,
                    HasActiveRelationships = _context.StudentEnrollments.Any(enrollment =>
                        enrollment.GroupID == group.Id && enrollment.IsActive) ||
                        _context.TeachingAssignments.Any(assignment =>
                            assignment.GroupID == group.Id && assignment.IsActive),
                });

            return await query.GridifyAsync(request, GroupFilterMapper);
        }

        public Task<bool> IsInstituteClassExist(Guid instituteClassID) =>
            instituteClassID == Guid.Empty
                ? Task.FromResult(false)
                : _context.InstituteClasses.AsNoTracking()
                    .AnyAsync(item => item.Id == instituteClassID);

        public async Task<bool> HasActiveRelationships(Guid groupID)
        {
            return await _context.StudentEnrollments.AsNoTracking()
                       .AnyAsync(enrollment => enrollment.GroupID == groupID && enrollment.IsActive) ||
                   await _context.TeachingAssignments.AsNoTracking()
                       .AnyAsync(assignment => assignment.GroupID == groupID && assignment.IsActive);
        }
    }
}
