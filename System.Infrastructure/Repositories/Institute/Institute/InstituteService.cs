using Microsoft.EntityFrameworkCore;
using Gridify;
using Gridify.EntityFramework;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Services.Institute.DTO;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Person.Student.DTO;
using System.Domain.Entities.Institue;
using System.Domain.Entities.Person;
using System.Domain.Enums.Institute;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Institute.Institute
{
    public class InstituteService : IInstituteRepository
    {
        private static readonly IGridifyMapper<InstituteLookupInfo> InstituteLookupMapper =
            new GridifyMapper<InstituteLookupInfo>()
                .AddMap(nameof(InstituteLookupInfo.Name), institute => institute.Name);

        private readonly AppDbContext _context;
        private readonly DbSet<clsInstitute> _dbSet;

        public InstituteService(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet = _context.Set<clsInstitute>();
        }

        public async Task<string> Add(clsInstitute entity)
        {
          await _dbSet.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<InstituteInfo> GetBy(
            string param,
            enInstituteType paramType,
            InstituteReadScope readScope)
        {
            if (paramType == enInstituteType.ID)
                return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                    .Where(i => i.Id == clsUtil.ToGuid(param)).Select(i =>
                new InstituteInfo
                {
                    InstituteID = i.Id.ToString(),
                    InstituteName = i.Name,
                    InstituteNote = i.Notes,
                    InstituteAddress = i.Address.Country.CountryName + " " + i.Address.City.CityName + " " + i.Address.District.DistrictName + " " + i.Address.Neighborhood.NeighborhoodName + " " + i.Address.Notes,
                }).FirstOrDefaultAsync();
            else
                return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                    .Where(i => i.Name== param).Select(i =>
 new InstituteInfo
 {
     InstituteID = i.Id.ToString(),
     InstituteName = i.Name,
     InstituteNote = i.Notes,
     InstituteAddress = i.Address.Country.CountryName + " " + i.Address.City.CityName + " " + i.Address.District.DistrictName + " " + i.Address.Neighborhood.NeighborhoodName + " " + i.Address.Notes,
 }).FirstOrDefaultAsync();
        }

        public async Task<clsInstitute> GetEntityBy(string param, enInstituteType paramType)
        {
            if (paramType == enInstituteType.ID)
                return await _dbSet.Where(i => i.Id == clsUtil.ToGuid(param)).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(i => i.Name == param).FirstOrDefaultAsync();
  
        }

        public async Task<List<InstituteInfo>> GetList(InstituteReadScope readScope)
        {
            return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                .Where(i => i.DeletedAt == null)
                .Select(i =>
                        new InstituteInfo
                        {
                            InstituteID = i.Id.ToString(),
                            InstituteName = i.Name,
                            InstituteNote = i.Notes,
                            InstituteAddress = i.Address.Country.CountryName + " " + i.Address.City.CityName + " " + i.Address.District.DistrictName + " " + i.Address.Neighborhood.NeighborhoodName + " " + i.Address.Notes,
                         }).ToListAsync();
        }

        public async Task<Paging<InstituteLookupInfo>> GetListFilter(
            GetInstituteListFilterQuery query,
            InstituteReadScope readScope)
        {
            return await ApplyReadScope(_dbSet.AsNoTracking(), readScope)
                .Select(institute => new InstituteLookupInfo
                {
                    InstituteID = institute.Id.ToString(),
                    Name = institute.Name,
                })
                .GridifyAsync(query, InstituteLookupMapper);
        }

        public async Task<bool> IsExistBy(string param, enInstituteType paramType)
        {
            if (paramType == enInstituteType.ID)
                return await _dbSet.AnyAsync(i => i.Id == clsUtil.ToGuid(param));
            else
                return await _dbSet.AnyAsync(i => i.Name == param);
        }

        public async Task<bool> IsExistBy(
            string param,
            enInstituteType paramType,
            InstituteReadScope readScope)
        {
            var query = ApplyReadScope(_dbSet.AsNoTracking(), readScope);
            return paramType == enInstituteType.ID
                ? await query.AnyAsync(institute => institute.Id == clsUtil.ToGuid(param))
                : await query.AnyAsync(institute => institute.Name == param);
        }

        private IQueryable<clsInstitute> ApplyReadScope(
            IQueryable<clsInstitute> query,
            InstituteReadScope readScope)
        {
            if (readScope.IsAdmin)
                return query;

            if (readScope.TeacherID.HasValue && readScope.TeacherID.Value != Guid.Empty)
            {
                var teacherId = readScope.TeacherID.Value;
                return query.Where(institute => _context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherId &&
                    assignment.Group.InstituteClass.InstituteID == institute.Id));
            }

            if (readScope.StudentID.HasValue && readScope.StudentID.Value != Guid.Empty)
            {
                var studentId = readScope.StudentID.Value;
                return query.Where(institute => _context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == studentId &&
                    enrollment.Group.InstituteClass.InstituteID == institute.Id));
            }

            return query.Where(_ => false);
        }

        public async Task<bool> Update(clsInstitute entity)
        {
           _dbSet.Update(entity);
            return true;
        }
    }
}
