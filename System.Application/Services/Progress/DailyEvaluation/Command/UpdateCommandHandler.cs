using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.DailyEvaluation.Command
{
    public class UpdateDailyEvaluationCommandHandler : IRequestHandler<UpdateDailyEvaluationCommand, TResult<string>>
    {
        private readonly IDailyEvaluationRepository _repo;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public UpdateDailyEvaluationCommandHandler(IDailyEvaluationRepository repo, ICurrentUserService currentUser,
            IMapper mapper, IStudentAcademicEnrollmentRepository academicEnrollmentRepository)
        {
            _repo = repo;
            _currentUserService = currentUser;
            _mapper = mapper;
            _academicEnrollmentRepository = academicEnrollmentRepository;
        }
        public async Task<TResult<string>> Handle(UpdateDailyEvaluationCommand request, CancellationToken cancellationToken)
        {

            var entity = await _repo.GetEntityBy(
                request.DailyEvaluationID,
                enDailyEvaluationParamType.ID);
            if (entity is null)
                return TResult<string>.Fail(ApplicationExMsg.failToFindDailyEvaluation);

            var academicEnrollment = await _academicEnrollmentRepository.GetBy(entity.StudentAcademicEnrollmentID);
            if (academicEnrollment is null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);
            var stageInfo = academicEnrollment.Items.FirstOrDefault(item =>
                item.StageInfoID == entity.StageInfoID.ToString());
            if (stageInfo is null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageInfoNotIncluded);

            var previousEvaluation = await _repo.GetLastDailyEvaluation(
                entity.StudentAcademicEnrollmentID.ToString(),
                entity.StageInfoID.ToString(),
                entity.Id.ToString());
            if (previousEvaluation is not null && previousEvaluation.To >= entity.To)
                return TResult<string>.Fail("يمكن تعديل آخر متابعة فقط للحفاظ على تسلسل التقدم");

            var minimumTo = previousEvaluation is null
                ? stageInfo.From
                : previousEvaluation.To + 1;
            if (request.To < minimumTo || request.To > stageInfo.To)
                return TResult<string>.Fail(ApplicationExMsg.DailyEvaluationRangeIsOutOFRange);

            _mapper.Map<UpdateDailyEvaluationCommand, clsDailyEvaluation>(request, entity);
            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("المستخدم الحالي غير صالح");

            entity.SetModified(userID.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
