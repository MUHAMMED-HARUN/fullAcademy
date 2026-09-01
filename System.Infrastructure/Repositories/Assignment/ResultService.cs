using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Services.Assignment.Result.DTO;
using System.Domain.Entities;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public class ResultService : IResultRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsResult> _set;

        public ResultService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsResult>();
        }

        public async Task<string> Add(clsResult entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsResult entity)
        {
            _set.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<List<ResultInfo>> GetList()
        {
            return await _set.Where(x=>x.IsActive==true).Select(x => new ResultInfo
            {
                ResultID = x.Id.ToString(),
                AssignmentID = x.AssignmentID.ToString(),
                AssignmentScopeName= x.Assignment.AssignmentNomination.AssignmentScope.Name,
                StageInfoMatter=x.AssignmentScopeInfo.StageInfo.Matter.Name,
                AssignmentScopeInfoID = x.AssignmentScopeInfoID.ToString(),
                Score = x.Score,
                Notes = x.Notes,
                StudentName=
                x.Assignment.AssignmentNomination.StudentEnrollment.Student.Person.FatherName + " " +
                x.Assignment.AssignmentNomination.StudentEnrollment.Student.Person.FatherName + " " +
                x.Assignment.AssignmentNomination.StudentEnrollment.Student.Person.LastName + " " 


            }).ToListAsync();
        }

        public async Task<ResultInfo> GetBy(Guid id)
        {
            return await _set.Where(x => x.Id == id)
                .Select(x => new ResultInfo
                {
                    ResultID = x.Id.ToString(),
                    AssignmentID = x.AssignmentID.ToString(),
                    AssignmentScopeName = x.Assignment.AssignmentNomination.AssignmentScope.Name,
                    StageInfoMatter = x.AssignmentScopeInfo.StageInfo.Matter.Name,
                    AssignmentScopeInfoID = x.AssignmentScopeInfoID.ToString(),
                    Score = x.Score,
                    Notes = x.Notes,
                    StudentName =
                x.Assignment.AssignmentNomination.StudentEnrollment.Student.Person.FatherName + " " +
                x.Assignment.AssignmentNomination.StudentEnrollment.Student.Person.FatherName + " " +
                x.Assignment.AssignmentNomination.StudentEnrollment.Student.Person.LastName + " "
                })
                .FirstOrDefaultAsync();
        }

        public async Task<clsResult> GetEntityBy(Guid id)
        {
            return await _set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistBy(Guid id)
        {
            return await _set.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> HasResult(Guid AssigmentID, Guid AssigmentScopeInfoID)
        {
            return await _set.AnyAsync(x => x.AssignmentID == AssigmentID && x.AssignmentScopeInfoID == AssigmentScopeInfoID);
        }

        public async Task<List<clsResult>> GetList(Guid[] scopeInfos)
        {
            return await _set.Where(x => scopeInfos.Contains(x.AssignmentScopeInfoID)).ToListAsync();
        }

        public async Task<List<clsResult>> GetListByAssignmentID(Guid AssignmentID)
        {
         return await _set.Where(x=> x.AssignmentID == AssignmentID).ToListAsync();
        }
    }
}
