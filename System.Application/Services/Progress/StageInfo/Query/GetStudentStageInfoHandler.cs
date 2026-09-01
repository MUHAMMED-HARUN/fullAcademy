using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.DTO;


namespace System.Application.Services.Progress.StageInfo.Query
{
    class GetStudentStageInfoQueryHandler : IRequestHandler<GetStudentStageInfoQuery, TResult<List<StageInfoInfo>>>
    {
        IStageInfoRepository _repo;
        public GetStudentStageInfoQueryHandler( IStageInfoRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<StageInfoInfo>>> Handle(GetStudentStageInfoQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<StageInfoInfo>>.OK(await _repo.GetStudentStageInfo(request.enrollmentID));
        }
    }
}
