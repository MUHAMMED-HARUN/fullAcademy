using MediatR;
using System;
using System.Application.Abstractions;
using System.Application.Common;
using System.Collections.Generic;

namespace System.Application.Services.Enrollment.StudentEnrollment.Command
{
    public class EnrollByRangeCommand : ICommand, IRequest<TResult<List<string>>>
    {
        public List<string> StudentAcademicEnrollmentIDs { get; set; } = new();
        public string GroupID { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
    }
}
