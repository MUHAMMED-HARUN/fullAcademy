using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.DTO;
namespace System.Application.Services.Progress.StageInfo.Query{
    public class GetStudentStageInfoQuery : IRequest<TResult<List<StageInfoInfo>>>, IQuery
    {
        public string enrollmentID { get; set; }
    }
}
