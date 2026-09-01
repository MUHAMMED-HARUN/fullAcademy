using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.Stage.DTO;
namespace System.Application.Services.Progress.Stage.Query{
    public class GetAllStageQuery : IRequest<TResult<List<DTO.StageInfo>>>, IQuery
    {
    }
}
