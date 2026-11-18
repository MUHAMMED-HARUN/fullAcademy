using Gridify;
using System.Application.Services.Progress.DailyEvaluation.DTO;
using System.Application.Services.Progress.DailyEvaluation.Query;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Progress
{
    public interface IDailyEvaluationRepository
    {
        Task<string> Add(clsDailyEvaluation entity);// return ID as String
        Task<bool> Update(clsDailyEvaluation entity);
        Task<List<DailyEvaluationInfo>> GetList();
        Task<Paging<DailyEvaluationInfo>> GetAllFilter(GetAllDailyEvaluationQuery request);

        Task<DailyEvaluationInfo?> GetBy(string param, enDailyEvaluationParamType paramType);
        Task<DailyEvaluationInfo?> GetLastDailyEvaluationInfo(string studentAcademicEnrollmentID, string stageInfoID);
        Task<clsDailyEvaluation?> GetEntityBy(string param, enDailyEvaluationParamType paramType);
        Task<clsDailyEvaluation?> GetLastDailyEvaluation(
            string studentAcademicEnrollmentID,
            string stageInfoID,
            string? excludeDailyEvaluationID = null);
        Task<bool> IsExistBy(string param, enDailyEvaluationParamType paramType);
    }
}
