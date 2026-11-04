using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.Session.DTO;
namespace System.Application.Services.Progress.Session.Query{
    public class GetAllSessionQuery : IRequest<TResult<List<SessionInfo>>>, IQuery
    {
    }
}
