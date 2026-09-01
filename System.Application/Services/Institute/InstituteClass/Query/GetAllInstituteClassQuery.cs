using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.Program.DTO;
namespace System.Application.Services.Institute.InstituteClass.Query{
    public class GetAllInstituteClassQuery : IRequest<TResult<List<InstituteClassInfo>>>, IQuery
    {
    }
}
