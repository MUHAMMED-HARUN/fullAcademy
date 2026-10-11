using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentNomination.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Persistence;
using System.Threading.Tasks.Dataflow;

namespace System.Infrastructure.Repositories.Assignment
{
    public class AssignmentNominationService : IAssignmentNominationRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAssignmentNomination> _set;

        public AssignmentNominationService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsAssignmentNomination>();
        }

        public async Task<string> Add(clsAssignmentNomination entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsAssignmentNomination entity)
        {
            _set.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<List<AssignmentNominationInfo>> GetList()
        {
            return await _set.Select(x => new AssignmentNominationInfo
            {
                AssignmentNominationID = x.Id.ToString(),
                StudentEnrollmentID = x.StudentEnrollmentID.ToString(),
                AssignmentScopeID = x.AssignmentScopeID.ToString(),
                Date = x.Date.ToString(),
              AssignmentScopeName = x.AssignmentScope.Name,
              StudentName=x.StudentEnrollment.Student.Person.FirstName + " "+
              x.StudentEnrollment.Student.Person.FatherName+ " " +
              x.StudentEnrollment.Student.Person.LastName+ " " ,
                NominationState = x.NominationState.ToString()
               ,CanStartAssigment = !_context.Assignments.Any(a=>a.AssignmentNominationID==x.Id &&
               (a.IsActive==true && a.AssignmentState!=enAssignmentType.Canceled))
            }).ToListAsync();
    
        }

        public async Task<AssignmentNominationInfo> GetBy(Guid id)
        {
            return await _set.Where(x => x.Id == id)
                .Select(x => new AssignmentNominationInfo
                {
                    AssignmentNominationID = x.Id.ToString(),
                    StudentEnrollmentID = x.StudentEnrollmentID.ToString(),
                    AssignmentScopeID = x.AssignmentScopeID.ToString(),
                    Date = x.Date.ToString(),
                    AssignmentScopeName = x.AssignmentScope.Name,
                    StudentName = x.StudentEnrollment.Student.Person.FirstName + " " +
              x.StudentEnrollment.Student.Person.FatherName + " " +
              x.StudentEnrollment.Student.Person.LastName + " ",
                    NominationState = x.NominationState.ToString()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<clsAssignmentNomination> GetEntityBy(Guid id)
        {
            return await _set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistBy(Guid id)
        {
            return await _set.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> HasNomination(Guid StudentEnrollmentID, Guid AssignmentScopeID)
        {
          return await  _set.AnyAsync(x => x.StudentEnrollmentID == StudentEnrollmentID && 
          x.AssignmentScopeID == AssignmentScopeID &&
          x.NominationState == Domain.Enums.Enrollment.enAssignmentType.Completed ||
          x.NominationState == Domain.Enums.Enrollment.enAssignmentType.inProgress&&
          x.IsActive==true &&
          x.DeletedAt==null);
        }

        public async Task<clsAssignmentNomination> GetEntityByAssigmetID(Guid AssigmetID)
        {
            var query = _set.Join(_context.Assignments,
                 assigNomini => assigNomini.Id,
                 assig => assig.AssignmentNominationID,
                 (assigNomini, assig) => new { assigNomini, assig });

          return await  query.Where(x => x.assig.Id == AssigmetID).Select(x=>x.assigNomini).FirstOrDefaultAsync();
        }
    }
}
