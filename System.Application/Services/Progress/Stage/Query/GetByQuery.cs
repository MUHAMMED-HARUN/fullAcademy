using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.Stage.DTO;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.Stage.Query
{
    public class GetStageByQuery : IRequest<TResult<DTO.StageInfo>>, IQuery
    {
        public string Param { get; set; }
        public enStageParamType ParamType { get; set; }
    }
}
