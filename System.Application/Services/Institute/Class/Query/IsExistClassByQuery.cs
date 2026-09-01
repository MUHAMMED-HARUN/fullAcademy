

using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Class.Query
{
    public class IsExistClassByQuery : IRequest<TResult <bool>>, IQuery
    {
        public string Param { get; set; }
        public enClassType ParamType { get; set; }
    }
}
