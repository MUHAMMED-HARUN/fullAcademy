using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.DTO;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.StageInfo.Query
{
    public class GetStageInfoByQuery : IRequest<TResult<StageInfoInfo>>, IQuery
    {
        public string Param { get; set; }
        public enStageInfoParamType ParamType { get; set; }
    }
}
