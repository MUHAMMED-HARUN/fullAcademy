using MediatR;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Services.Progress.StudentStage.DTO;


namespace System.Application.Services.Progress.StudentStage.Query
{
    class GetAllStudentStageQueryHandler : IRequestHandler<GetAllStudentStageQuery, TResult<List<StudentStageInfo>>>
    {
        IStudentStageRepository _repo;
        public GetAllStudentStageQueryHandler ( IStudentStageRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<StudentStageInfo>>> Handle(GetAllStudentStageQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<StudentStageInfo>>.OK(await _repo.GetList());
        }
    }
}
