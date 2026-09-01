using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Services.Progress.DailyEvaluation.Command
{
    public class DeleteDailyEvaluationCommandHandler : IRequestHandler<DeleteDailyEvaluationCommand, TResult<string>>
    {
        private readonly IDailyEvaluationRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public DeleteDailyEvaluationCommandHandler(IDailyEvaluationRepository repo ,ICurrentUserService currentUser,IMapper mapper)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(DeleteDailyEvaluationCommand request, CancellationToken cancellationToken)
        {
            clsDailyEvaluation entity = await _repo.GetEntityBy(request.DailyEvaluationID, enDailyEvaluationParamType.ID);
            if (entity == null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindDailyEvaluation);

            entity.SetDeleted(_currentUserService.UserId.ToString());
           await _repo.Update(entity);

            return TResult<string>.OK(string .Empty);
        }
    }
}
