using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.Class.DTO;
namespace System.Application.Services.Institute.Class.Query{
    public class GetAllClassQuery : IRequest<TResult<List<ClassInfo>>>, IQuery
    {
    }
}
