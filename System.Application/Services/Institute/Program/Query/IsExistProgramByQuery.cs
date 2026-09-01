using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Institute.Query
{
    public class IsExistProgramByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enProgramType ParamType { get; set; }
    }
}
