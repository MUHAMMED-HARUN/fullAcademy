using Gridify;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Application.Services.Person.Student.DTO;
using System.Application.Services.Progress.DailyEvaluation.DTO;


namespace System.Application.Services.Progress.DailyEvaluation.Query
{
    class GetAllDailyEvaluationQueryHandler : IRequestHandler<GetAllDailyEvaluationQuery, TResult<Paging<DailyEvaluationInfo>>>
    {
        IDailyEvaluationRepository _repo;
        private readonly ICurrentUserService _currentUserService;

        public GetAllDailyEvaluationQueryHandler ( IDailyEvaluationRepository repo,ICurrentUserService currentUserService)
        {
            _repo= repo;
            _currentUserService = currentUserService;
        }
        public async Task<TResult<Paging<DailyEvaluationInfo>>> Handle(GetAllDailyEvaluationQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == Guid.Empty || _currentUserService.EntityId == null)
                return TResult<Paging<DailyEvaluationInfo>>.Fail(ApplicationExMsg.AccessDinede);
            else if (_currentUserService.Role == "Teacher")
                return TResult<Paging<DailyEvaluationInfo>>.OK(await _repo.GetAllFilter(request, _currentUserService.EntityId));

            else if (_currentUserService.Role == "Admin")
                return TResult<Paging<DailyEvaluationInfo>>.OK(await _repo.GetAllFilter(request, null));

            return TResult<Paging<DailyEvaluationInfo>>.Fail(ApplicationExMsg.AccessDinede);

        }
    }
}
