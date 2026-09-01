using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.Stage.Query
{
    public class IsExistStageByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enStageParamType ParamType { get; set; }
    }
}
