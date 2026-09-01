using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Progress.StudentStage.DTO;
using System.Application.Services.Progress.StudentStage.Query;


namespace System.Application.Services.Progress.StudentStage.Query
{
    class GetStudentStageByQueryHandler : IRequestHandler<GetStudentStageByQuery, TResult<StudentStageInfo>>
    {
        IStudentStageRepository _repo;
        public GetStudentStageByQueryHandler( IStudentStageRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<StudentStageInfo>> Handle(GetStudentStageByQuery request, CancellationToken cancellationToken)
        {
            StudentStageInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<StudentStageInfo>.Fail(ApplicationExMsg.failToFindStudentStage);

            return TResult<StudentStageInfo>.OK(entityInfo);

        }
    }
}
