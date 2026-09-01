using Gridify;
using MediatR;
using System.Application.Abstractions;

namespace System.Application.Common.Models
{
    // كلاس أساسي يرث من GridifyQuery ويعيد Paging<TResult>
    public class BaseFilterQuery<TResult> : GridifyQuery, IRequest<TResult< Paging<TResult>>>,IQuery
    {
    }
}