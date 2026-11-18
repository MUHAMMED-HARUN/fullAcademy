using System.Application.Abstractions;
using System.Application.Common.Models;
using System.Application.Services.Progress.DailyEvaluation.DTO;
namespace System.Application.Services.Progress.DailyEvaluation.Query{
     public class GetAllDailyEvaluationQuery : BaseFilterQuery<DailyEvaluationInfo>, IQuery
     {
         public string? StudentAcademicEnrollmentID { get; set; }

         public GetAllDailyEvaluationQuery()
         {
             Page = 1;
             PageSize = 10;
         }
     }
}
