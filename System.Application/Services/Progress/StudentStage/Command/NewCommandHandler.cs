using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Domain.Enums.Person;



namespace System.Application.Services.Progress.StudentStage.Command
{
    public class NewStudentStageCommandHandler : IRequestHandler<NewStudentStageCommand, TResult<string>>
    {
        private readonly IStudentStageRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly IMapper _mapper;
        public NewStudentStageCommandHandler(IStudentStageRepository repo, IStudentAcademicEnrollmentRepository academicEnrollmentRepository, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _academicEnrollmentRepository = academicEnrollmentRepository;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(NewStudentStageCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.StudentAcademicEnrollmentID, out var academicEnrollmentID) || academicEnrollmentID == Guid.Empty ||
                !Guid.TryParse(request.StageID, out var stageID) || stageID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var targetDate = request.StudentStageDate == default
                ? DateOnly.FromDateTime(DateTime.Today)
                : request.StudentStageDate;
            var academicEnrollment = await _academicEnrollmentRepository.GetBy(academicEnrollmentID);
            if (academicEnrollment == null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);
            if (academicEnrollment.Status != enStudentAcademicEnrollmentStatus.Active ||
                targetDate < academicEnrollment.ValidFrom ||
                (academicEnrollment.ValidTo.HasValue && targetDate > academicEnrollment.ValidTo.Value))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);
            if (!academicEnrollment.Items.Any(item => Guid.TryParse(item.StageID, out var snapshotStageID) && snapshotStageID == stageID))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageNotIncluded);

            var previousStageID = await _repo.GetPreviousStageID(stageID);
            if (previousStageID is Guid previousID && previousID != Guid.Empty)
            {
                if (!academicEnrollment.Items.Any(item =>
                        Guid.TryParse(item.StageID, out var snapshotStageID) && snapshotStageID == previousID))
                    return TResult<string>.Fail(ApplicationExMsg.StudentStage_PreviousStageNotIncluded);

                var previousStage = await _repo.GetEntityBy(
                    academicEnrollmentID.ToString(), previousID.ToString(), enStageStateType.completed);
                if (previousStage == null)
                    return TResult<string>.Fail(ApplicationExMsg.StudentStage_PreviousStageNotCompleted);
            }

            clsStudentStage? studentStage = await _repo.GetEntityBy(
                academicEnrollmentID.ToString(), request.StageID, enStageStateType.procecing);
            if (studentStage != null)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            clsStudentStage entity = _mapper.Map<NewStudentStageCommand, clsStudentStage>(request);
            entity.SetDate(targetDate);
            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("المستخدم الحالي غير صالح");
            entity.SetCreated(userID.ToString());


            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
