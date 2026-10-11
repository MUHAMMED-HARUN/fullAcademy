using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Services.Assignment.Assignment.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public class AssignmentService : IAssignmentRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAssignment> _set;

        public AssignmentService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsAssignment>();
        }

        public async Task<string> Add(clsAssignment entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsAssignment entity)
        {
            _set.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<List<AssignmentInfo>> GetList()
        {
            return await _set.Select(x => new AssignmentInfo
            {
                AssignmentID = x.Id.ToString(),
                AssignmentNominationID = x.AssignmentNominationID.ToString(),
                ScheduledDate = x.ScheduledDate.ToString(),
                AssignmentState = x.AssignmentState.ToString(),
                IsPassed = x.IsPassed,
                AssigmentScopeName = x.AssignmentNomination.AssignmentScope.Name,
                StudentName = x.AssignmentNomination.StudentEnrollment.Student.Person.FirstName + " " +
                 x.AssignmentNomination.StudentEnrollment.Student.Person.FatherName + " " +
                  x.AssignmentNomination.StudentEnrollment.Student.Person.LastName,
                AssignmentScopeID = x.AssignmentNomination.AssignmentScopeID.ToString(),
                CanAddResult =
                // ÇáÍÇáÉ ÇáÃæáì: áÇ ÊæÌÏ Ãí äÊíÌÉ ãÓÌáÉ áåÐÇ ÇáÇãÊÍÇä äåÇÆíÇð
                !_context.Results.Any(r => r.AssignmentID == x.Id && r.IsActive)
                ||
                // ÇáÍÇáÉ ÇáËÇäíÉ: ÇáÇãÊÍÇä äÔØ¡ æáã íÊã ÅÏÎÇá äÊÇÆÌ áßá ÇáäØÇÞÇÊ ÇáÝÑÚíÉ ÇáãÊÇÍÉ ÈÚÏ
                (
                    x.IsActive &&
                    _context.Results.Count(r => r.AssignmentID == x.Id && r.IsActive)
                    <
                    _context.AssignmentScopeInfos.Count(a => a.AssignmentScopeID == x.AssignmentNomination.AssignmentScopeID && a.IsActive)
                )
                , TotalScore = x.TotalScore
            }).ToListAsync();
      

        }

        public async Task<AssignmentInfo> GetBy(Guid id)
        {
            return await _set.Select(x => new AssignmentInfo
            {
                AssignmentID = x.Id.ToString(),
                AssignmentNominationID = x.AssignmentNominationID.ToString(),
                ScheduledDate = x.ScheduledDate.ToString(),
                AssignmentState = x.AssignmentState.ToString(),
                IsPassed = x.IsPassed,
                AssigmentScopeName = x.AssignmentNomination.AssignmentScope.Name,
                StudentName = x.AssignmentNomination.StudentEnrollment.Student.Person.FirstName + " " +
                 x.AssignmentNomination.StudentEnrollment.Student.Person.FatherName + " " +
                  x.AssignmentNomination.StudentEnrollment.Student.Person.LastName,
                AssignmentScopeID = x.AssignmentNomination.AssignmentScopeID.ToString()
            }).FirstOrDefaultAsync();
        }

        public async Task<clsAssignment> GetEntityBy(Guid id)
        {
            return await _set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistBy(Guid id)
        {
            return await _set.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> HasAssignment(Guid NominationID)
        {
            return await _set.AnyAsync(x => x.AssignmentNominationID == NominationID&&
            x.AssignmentState==enAssignmentType.inProgress ||
            x.AssignmentState==enAssignmentType.Completed);
        }
    }
}
