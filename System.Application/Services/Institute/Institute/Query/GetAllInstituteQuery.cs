using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.DTO;
namespace System.Application.Services.Institute.Institute.Query{
    public class GetAllInstituteQuery : IRequest<TResult<List<InstituteInfo>>>, IQuery
    {
    }
}
