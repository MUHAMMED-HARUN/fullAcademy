
using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Entities;

namespace System.Application.Services.Institute.Group.Query
{
    public class GetGroupsByTeacherIDQuery :IRequest<TResult<List<clsGroup>>>, IQuery
    {
        public string TeacherID { get; set; }
    }
}
