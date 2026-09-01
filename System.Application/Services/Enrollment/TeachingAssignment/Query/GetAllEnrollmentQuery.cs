using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Common.Models;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;

namespace System.Application.Services.Enrollment.TeachingAssignment.Query
{
    public class GetAllTeachingAssignmentQuery : BaseFilterQuery<TeachingAssignmentInfo>
    {
    }
}
