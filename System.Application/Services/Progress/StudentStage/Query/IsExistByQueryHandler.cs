using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;

namespace System.Application.Services.Progress.StudentStage.Query
{
    class IsExistStudentStageByQueryHandler : IRequestHandler<IsExistStudentStageByQuery, TResult<bool>>
    {
        IStudentStageRepository _repo;
        public IsExistStudentStageByQueryHandler(   IStudentStageRepository repo)
        {
           _repo = repo;
        }
        public async Task<TResult<bool>> Handle(IsExistStudentStageByQuery request, CancellationToken cancellationToken)
        {
            bool isExist = await _repo.IsExistBy(request.Param, request.ParamType);
            if (!isExist) 
            return TResult<bool>.Fail(ApplicationExMsg.failToFindStudentStage);

            return TResult<bool>.OK(isExist);

        }
    }
}
