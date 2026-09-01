using MediatR;
using System.Application.Common;
using System.Application.Services.Assignment.Result.DTO;

namespace System.Application.Services.Assignment.Result.Query
{
    public class GetResultByQuery : IRequest<TResult<ResultInfo>>
    {
        public string ID { get; set; }
    }

    public class GetAllResultQuery : IRequest<TResult<List<ResultInfo>>>
    {
    }
}
