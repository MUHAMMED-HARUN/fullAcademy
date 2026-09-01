using Gridify;
using System.Application.Services.Person.Student.DTO;
using System.Application.Services.Person.Student.Query;
using System.Application.Services.Progress.DailyEvaluation.DTO;
using System.Application.Services.Progress.DailyEvaluation.Query;
using System.Application.Services.Progress.Stage.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Progress
{
    public interface IDailyEvaluationRepository
    {
        Task<string> Add(clsDailyEvaluation entity);// return ID as String
        Task<bool> Update(clsDailyEvaluation entity);
        Task<List<DailyEvaluationInfo>> GetList();
        Task<Paging<DailyEvaluationInfo>> GetAllFilter(GetAllDailyEvaluationQuery requset,string TeacherID);

        Task<DailyEvaluationInfo> GetBy(string param, enDailyEvaluationParamType paramType);
        Task<DailyEvaluationInfo> GetLastDailyEvaluationInfo(string enrollmentStudentIDs,string stageInfoID);
        Task<clsDailyEvaluation> GetEntityBy(string param, enDailyEvaluationParamType paramType);
        Task<clsDailyEvaluation> GetLastDailyEvaluation(string enrollmentStudentIDs);
        Task<clsDailyEvaluation> GetLastDailyEvaluation(string enrollmentStudentID,string outDailyEvaluationID);
        Task<bool> IsExistBy(string param, enDailyEvaluationParamType paramType);
    }
}
