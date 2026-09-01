using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.StudentStage.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public class StudentStageService : IStudentStageRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsStudentStage> _set;
        private static string GetState(enStageStateType state)
        {
            switch (state)
            {
                case enStageStateType.canceled:
                    return "تم الإلغاء";
                case enStageStateType.completed:
                    return "مكتمل";
                case enStageStateType.failed:
                    return "فشل";
                case enStageStateType.procecing:
                    return "قيد المعالجة";

            }
            return "فشل";
        }

        public StudentStageService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsStudentStage>();
        }
        public async Task<string> Add(clsStudentStage entity)
        {
          await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<StudentStageInfo> GetBy(string param, enStudentStageParamType paramType)
        {
 
                return await _set.Where(x => x.Id == clsUtil.ToGuid(param)).
                             Select(x => new StudentStageInfo
                             {
                                 StudentStageID = x.Id.ToString(),
                                 StageID = x.StageID.ToString(),
                                 StageName = x.Stage.Name,
                                 EnrollmentStudentID = x.EnrollmentStudentID.ToString(),
                                 StudentName = x.StudentEnrollment.Student.Person.FirstName + " " + x.StudentEnrollment.Student.Person.FatherName + " " + x.StudentEnrollment.Student.Person.LastName,
                                 GroupName= x.StudentEnrollment.Group.GroupName,
                                 StudentStageDate = x.Date,
                                 StudentStageState = GetState(x.State)
                             }
                            ).FirstOrDefaultAsync();
        }

        public async Task<clsStudentStage> GetEntityBy(string param, enStudentStageParamType paramType)
        {
      return await _set.Where(x => x.Id == clsUtil.ToGuid(param))
                            .FirstOrDefaultAsync();

        
        }

        public async Task<List<StudentStageInfo>> GetList()
        {
            return await _set.Select(x => new StudentStageInfo
            {
                StudentStageID = x.Id.ToString(),
                StageID = x.StageID.ToString(),
                StageName = x.Stage.Name,
                EnrollmentStudentID = x.EnrollmentStudentID.ToString(),
                StudentName = x.StudentEnrollment.Student.Person.FirstName + " " + x.StudentEnrollment.Student.Person.FatherName + " " + x.StudentEnrollment.Student.Person.LastName,
                GroupName = x.StudentEnrollment.Group.GroupName,
                StudentStageDate = x.Date,
                StudentStageState = GetState(x.State)
            }
                            ).ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enStudentStageParamType paramType)
        {
            return await _set.AnyAsync(x => x.Id == clsUtil.ToGuid(param));
        }

        public async Task<bool> Update(clsStudentStage entity)
        {
            _set.Update(entity);
            return true;
        }

        public async Task<clsStudentStage> GetEntityBy(string enrollmentStudentID, string stageID, enStageStateType state)
        {
            return await _set.Where(x => x.EnrollmentStudentID == clsUtil.ToGuid(enrollmentStudentID)&&
                x.StageID == clsUtil.ToGuid(stageID)&&
                x.State==state)
                                  .FirstOrDefaultAsync();
        }
    }
}
