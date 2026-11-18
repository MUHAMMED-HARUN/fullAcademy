using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.Enrollment.TeachingAssignment.Command
{
    public class DeleteTeachingAssignmentCommand : ICommand, IRequest<TResult<string>>
    {
        public string TeachingAssignmentID { get; set; } = string.Empty;

        // Compatibility with the existing frontend payload until all deployed clients align.
        public string EnrollmentID { get; set; } = string.Empty;

        public string ResolveID() =>
            string.IsNullOrWhiteSpace(TeachingAssignmentID)
                ? EnrollmentID
                : TeachingAssignmentID;
    }
}
