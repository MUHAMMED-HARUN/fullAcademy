using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;
using System.Application.Services.Institute.Group.Query;
using System.Domain.Entities;
using System.Domain.Enums.Institute;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Institute.Institute
{
    public class GroupService : IGroupRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsGroup> _dbSet;

        public GroupService(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet = _context.Set<clsGroup>();
        }

        public async Task<string> Add(clsGroup entity)
        {
            await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<GroupInfo> GetBy(string param, enGroupType paramType)
        {
            if (paramType == enGroupType.ID)
            {
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param))
                    .Select(i => new GroupInfo
                    {
                        GroupID = i.Id.ToString(),
                        GroupName = i.GroupName,
                        Code = i.Code,
                        InstituteClassID = i.InstituteClassID.ToString(),
                        ClassName = i.InstituteClass.Class.Name,
                        InstituteName = i.InstituteClass.Institute.Name
                    }).FirstOrDefaultAsync();
            }
            else if (paramType == enGroupType.Name)
            {
                return await _dbSet.Where(i => i.GroupName == param)
                    .Select(i => new GroupInfo
                    {
                        GroupID = i.Id.ToString(),
                        GroupName = i.GroupName,
                        Code = i.Code,
                        InstituteClassID = i.InstituteClassID.ToString(),
                        ClassName = i.InstituteClass.Class.Name,
                        InstituteName = i.InstituteClass.Institute.Name
                    }).FirstOrDefaultAsync();
            }
            else // Code
            {
                return await _dbSet.Where(i => i.Code == param)
                    .Select(i => new GroupInfo
                    {
                        GroupID = i.Id.ToString(),
                        GroupName = i.GroupName,
                        Code = i.Code,
                        InstituteClassID = i.InstituteClassID.ToString(),
                        ClassName = i.InstituteClass.Class.Name,
                        InstituteName = i.InstituteClass.Institute.Name
                    }).FirstOrDefaultAsync();
            }
        }

        public async Task<clsGroup> GetEntityBy(string param, enGroupType paramType)
        {
            if (paramType == enGroupType.ID)
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).FirstOrDefaultAsync();
            else if (paramType == enGroupType.Name)
                return await _dbSet.Where(i => i.GroupName == param).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(i => i.Code == param).FirstOrDefaultAsync();
        }

        public async Task<List<GroupInfo>> GetList()
        {
            return await _dbSet.Where(i => i.DeletedAt == null)
                .Select(i => new GroupInfo
                {
                    GroupID = i.Id.ToString(),
                    GroupName = i.GroupName,
                    Code = i.Code,
                    InstituteClassID = i.InstituteClassID.ToString(),
                    ClassName = i.InstituteClass.Class.Name,
                    InstituteName = i.InstituteClass.Institute.Name
                }).ToListAsync();
        }

        public async Task<Paging<GroupInfo>> GetListFilter(GetAllGroupQuery request)
        {


            return await _dbSet.IgnoreQueryFilters() .Select(i => new GroupInfo
                {
                    GroupID = i.Id.ToString(),
                    GroupName = i.GroupName,
                    Code = i.Code,
                    InstituteClassID = i.InstituteClassID.ToString(),
                    ClassName = i.InstituteClass.Class.Name,
                    InstituteName = i.InstituteClass.Institute.Name
                }).GridifyAsync(request);
        }

        public async Task<bool> IsExistBy(string param, enGroupType paramType)
        {
            if (paramType == enGroupType.ID)
                return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
            else if (paramType == enGroupType.Name)
                return await _dbSet.AnyAsync(i => i.GroupName == param);
            else
                return await _dbSet.AnyAsync(i => i.Code == param);
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

        public async Task<List<clsGroup>> GetGroupByTeacherID(string TeacherID)
        {
            return await _dbSet
                .Join(_context.TeachingAssignments,
            group => group.Id,
            TeacherAssig => TeacherAssig.GroupID,
            (group, TeacherAssig) => new { group, TeacherAssig })

                .Where(i => i.group.DeletedAt == null && i.TeacherAssig.TeacherID == clsUtil.ToGuid(TeacherID))
                .Select(i => i.group)
                .ToListAsync();
        }
    }
}
