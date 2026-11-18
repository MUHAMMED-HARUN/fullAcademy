using System.Application.Common.Models;
using System.Application.Services.Progress.StudentStage.DTO;

namespace System.Application.Services.Progress.StudentStage.Query
{
    public sealed class GetAllStudentStageQuery : BaseFilterQuery<StudentStageInfo>
    {
        public string? StudentAcademicEnrollmentID { get; set; }

        public GetAllStudentStageQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
