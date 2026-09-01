using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.StageInfo.Query
{
    public class IsExistStageInfoByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enStageInfoParamType ParamType { get; set; }
    }
}
