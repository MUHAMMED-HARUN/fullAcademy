using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Application.Services.Progress.DailyEvaluation.DTO;
using System.Application.Services.Progress.DailyEvaluation.Query;
using System.Domain.Entities;
using System.Domain.Enums.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Progress
{
    public class DailyEvaluationService : IDailyEvaluationRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsDailyEvaluation> _set;
      
        public DailyEvaluationService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsDailyEvaluation>();
        }
        private static string GetLevelType(enLevelType levelType)
        {
            switch (levelType)
            {
                case enLevelType.VeryBad:
                    return"سيء جدا";
                case enLevelType.Bad:
                    return "سيء";
                case enLevelType.Good:
                    return "جبد";
                case enLevelType.VeryGood:
                    return"جيد جدا";
                case enLevelType.Excilent:
                    return "ممتاز";
            }
            return "سيئ خطا";
        }
        public async Task<string> Add(clsDailyEvaluation entity)
        {
          await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<DailyEvaluationInfo> GetBy(string param, enDailyEvaluationParamType paramType)
        {
            if (paramType == enDailyEvaluationParamType.ID)

                return await _set.Where(x => x.Id == clsUtil.ToGuid(param)).
                             Select(x => new DailyEvaluationInfo
                             {
                                 DailyEvaluationID = x.Id.ToString(),
                                 StudentEnrollmentID = x.StudentEnrollmentID.ToString(),
                                 StudentName = x.StudentEnrollment.Student.Person.FirstName + " " +
                              x.StudentEnrollment.Student.Person.FatherName + " " + x.StudentEnrollment.Student.Person.LastName,
                                 StageInfoID = x.StageInfoID.ToString(),
                                 Date = x.Date,
                                 From = x.From,
                                 To = x.To,
                                 GroupID = x.StudentEnrollment.GroupID.ToString(),
                                 GroupName = x.StudentEnrollment.Group.GroupName,
                                 LevelType = GetLevelType(x.LevelType)
                             }
                            ).FirstOrDefaultAsync();

            else
    return await _set.Where(x => x.StudentEnrollmentID == clsUtil.ToGuid( param)).
                                  Select(x => new DailyEvaluationInfo
                                  {
                                      DailyEvaluationID = x.Id.ToString(),
                                      StudentEnrollmentID = x.StudentEnrollmentID.ToString(),
                                      StudentName = x.StudentEnrollment.Student.Person.FirstName + " " +
                              x.StudentEnrollment.Student.Person.FatherName + " " + x.StudentEnrollment.Student.Person.LastName,
                                      StageInfoID = x.StageInfoID.ToString(),
                                      Date = x.Date,
                                      From = x.From,
                                      To = x.To,
                                      GroupID = x.StudentEnrollment.GroupID.ToString(),
                                      GroupName = x.StudentEnrollment.Group.GroupName,
                                      LevelType = GetLevelType(x.LevelType)
                                  }
                            ).FirstOrDefaultAsync();
        }

        public async Task<clsDailyEvaluation> GetEntityBy(string param, enDailyEvaluationParamType paramType)
        {
            if (paramType == enDailyEvaluationParamType.ID)

                return await _set.Where(x => x.Id == clsUtil.ToGuid(param))
                            .FirstOrDefaultAsync();

            else
                return await _set.Where(x => x.StudentEnrollmentID == clsUtil.ToGuid( param)).
                                         FirstOrDefaultAsync();
        }

        public async Task<List<DailyEvaluationInfo>> GetList()
        {
            return await _set.Select(x => new DailyEvaluationInfo
            {
                DailyEvaluationID = x.Id.ToString(),
                StudentEnrollmentID = x.StudentEnrollmentID.ToString(),
                StudentName = x.StudentEnrollment.Student.Person.FirstName + " " +
                              x.StudentEnrollment.Student.Person.FatherName + " " + x.StudentEnrollment.Student.Person.LastName,
                StageInfoID = x.StageInfoID.ToString(),
                Date = x.Date,
                From = x.From,
                To = x.To,
                GroupID = x.StudentEnrollment.GroupID.ToString(),
                GroupName = x.StudentEnrollment.Group.GroupName,
                LevelType = GetLevelType(x.LevelType)
            }
                            ).ToListAsync();
        }

        public async Task<bool> IsExistBy(string param, enDailyEvaluationParamType paramType)
        {
            if (paramType == enDailyEvaluationParamType.ID)

                return await _set.AnyAsync(x => x.Id == clsUtil.ToGuid(param))
                             ;

            else
                return await _set.AnyAsync(x => x.StudentEnrollmentID == clsUtil.ToGuid( param))
                                        ;
        }

        public async Task<bool> Update(clsDailyEvaluation entity)
        {
            _set.Update(entity);
            return true;
        }

        public async Task<DailyEvaluationInfo> GetLastDailyEvaluationInfo(string enrollmentStudentIDs ,string stageInfoID)
        {
         return await   _set.Where(x=>x.StudentEnrollmentID==clsUtil.ToGuid(enrollmentStudentIDs)
         && x.StageInfoID==clsUtil.ToGuid( stageInfoID)).OrderByDescending(x=>x.To).Select(x => new DailyEvaluationInfo
            {
                DailyEvaluationID = x.Id.ToString(),
                StudentEnrollmentID = x.StudentEnrollmentID.ToString(),
                StudentName = x.StudentEnrollment.Student.Person.FirstName + " " +
       x.StudentEnrollment.Student.Person.FatherName + " " + x.StudentEnrollment.Student.Person.LastName,
                StageInfoID = x.StageInfoID.ToString(),
                Date = x.Date,
                From = x.From,
                To = x.To,
                GroupID = x.StudentEnrollment.GroupID.ToString(),
                GroupName = x.StudentEnrollment.Group.GroupName,
                LevelType = GetLevelType(x.LevelType)
            }
     ).FirstOrDefaultAsync();
        }

        public async Task<clsDailyEvaluation> GetLastDailyEvaluation(string enrollmentStudentIDs)
        {
           return await _set.Where(x => x.StudentEnrollmentID == clsUtil.ToGuid(enrollmentStudentIDs)&& 
           x.LevelType>enLevelType.Bad).OrderByDescending(x => x.To)
.FirstOrDefaultAsync();
        }

        public async Task<clsDailyEvaluation> GetLastDailyEvaluation(string enrollmentStudentID, string outDailyEvaluationID)
        {
           return await _set.Where(x => x.StudentEnrollmentID == clsUtil.ToGuid(enrollmentStudentID)
            && x.StudentEnrollmentID!= clsUtil.ToGuid( outDailyEvaluationID)).OrderByDescending(x => x.To)
  .FirstOrDefaultAsync();
        }

        public async Task<Paging<DailyEvaluationInfo>> GetAllFilter(GetAllDailyEvaluationQuery requset, string TeacherID)
        {

                        var query = _set
            .Join(_context.StudentEnrollments,
            dayEv => dayEv.StudentEnrollmentID,
            studentEnroll => studentEnroll.Id,
            (dayEv, studentEnroll) => new { dayEv, studentEnroll })
            .Join(_context.TeachingAssignments,
            x => x.studentEnroll.GroupID,
            teachingAssig => teachingAssig.GroupID,
            (x, teachingAssig) => new { x.studentEnroll, x.dayEv, teachingAssig });


            if (!string.IsNullOrEmpty(TeacherID))
            {
                var teacherGuid = clsUtil.ToGuid(TeacherID);
                query = query.Where(x => x.teachingAssig.TeacherID == teacherGuid);
            }
            var resultQuery = query.Select(x => new DailyEvaluationInfo
            {
                DailyEvaluationID = x.dayEv.Id.ToString(),
                StudentEnrollmentID = x.dayEv.StudentEnrollmentID.ToString(),
                StudentName = x.dayEv.StudentEnrollment.Student.Person.FirstName + " " +
       x.dayEv.StudentEnrollment.Student.Person.FatherName + " " + x.dayEv.StudentEnrollment.Student.Person.LastName,
                StageInfoID = x.dayEv.StageInfoID.ToString(),
                Date = x.dayEv.Date,
                From = x.dayEv.From,
                To = x.dayEv.To,
                GroupID = x.dayEv.StudentEnrollment.GroupID.ToString(),
                GroupName = x.dayEv.StudentEnrollment.Group.GroupName,
                LevelType = x.dayEv.LevelType == enLevelType.VeryBad ? "سيء جدا" :
                x.dayEv.LevelType == enLevelType.Bad ? "سيء" :
                x.dayEv.LevelType == enLevelType.Good ? "جيد" :
                x.dayEv.LevelType == enLevelType.VeryGood ? "جيد جدا" :
                x.dayEv.LevelType == enLevelType.Excilent ? "ممتاز" : "غير معروف"
            }
     );

            // 4. تطبيق التصفية والترقيم (Pagination) باستخدام Gridify
            return await resultQuery.GridifyAsync(requset);


        }
    }
}
