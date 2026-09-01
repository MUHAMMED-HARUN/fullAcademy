using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class DeleteStudentEnrollmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string EnrollmentID { get; set; }
    }
}
