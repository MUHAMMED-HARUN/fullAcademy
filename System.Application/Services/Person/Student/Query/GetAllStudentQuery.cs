
using System.Application.Common.Models;
using System.Application.Services.Person.Student.DTO;

namespace System.Application.Services.Person.Student.Query
{
    public class GetAllStudentQuery : BaseFilterQuery<StudentInfo>
    {
        public GetAllStudentQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
