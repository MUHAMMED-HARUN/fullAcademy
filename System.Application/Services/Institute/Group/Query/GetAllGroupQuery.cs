using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;

namespace System.Application.Services.Institute.Group.Query
{
    public class GetAllGroupQuery : IRequest<TResult<List<GroupInfo>>>, IQuery
    {
    }
}
