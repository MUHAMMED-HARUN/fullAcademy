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
    
    public class NewDailyEvaluationCommandHandler : IRequestHandler<NewDailyEvaluationCommand, TResult<string>>
    {
        private readonly IDailyEvaluationRepository _repo;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public NewDailyEvaluationCommandHandler(IDailyEvaluationRepository repo, ICurrentUserService currentUser,
            IMapper mapper, IStudentAcademicEnrollmentRepository academicEnrollmentRepository)
        {
            _repo = repo;
            _currentUserService = currentUser;
           _mapper = mapper;
            _academicEnrollmentRepository = academicEnrollmentRepository;
        }
        public async Task<TResult<string>> Handle(NewDailyEvaluationCommand request, CancellationToken cancellationToken)
        {
            request.Date = request.Date.Date + DateTime.Now.TimeOfDay;

            if (!Guid.TryParse(request.StudentAcademicEnrollmentID, out var academicEnrollmentID) ||
                academicEnrollmentID == Guid.Empty ||
                !Guid.TryParse(request.StageInfoID, out var stageInfoID) || stageInfoID == Guid.Empty)
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            var academicEnrollment = await _academicEnrollmentRepository.GetBy(academicEnrollmentID);
            if (academicEnrollment is null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);

            var attendanceDate = DateOnly.FromDateTime(request.Date);
            if (academicEnrollment.Status != System.Domain.Enums.Enrollment.enStudentAcademicEnrollmentStatus.Active ||
                attendanceDate < academicEnrollment.ValidFrom ||
                (academicEnrollment.ValidTo.HasValue && attendanceDate > academicEnrollment.ValidTo.Value))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);

            var stageInfo = academicEnrollment.Items.FirstOrDefault(item =>
                clsUtil.ToGuid( item.StageInfoID)== clsUtil.ToGuid(request.StageInfoID));
            if (stageInfo is null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageInfoNotIncluded);

            var lastDailyEvaluation = await _repo.GetLastDailyEvaluation(
                request.StudentAcademicEnrollmentID,
                request.StageInfoID);
            var from = lastDailyEvaluation is null
                ? stageInfo.From
                : (((int)lastDailyEvaluation.LevelType)>((int)enLevelType.Bad)) ?lastDailyEvaluation.To + 1: lastDailyEvaluation.From;
            if (request.To < from || request.To > stageInfo.To)
                return TResult<string>.Fail(ApplicationExMsg.DailyEvaluationRangeIsOutOFRange);
          
            clsDailyEvaluation entity = _mapper.Map<NewDailyEvaluationCommand, clsDailyEvaluation>(request);
            entity.SetFrom(from);
            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("المستخدم الحالي غير صالح");

            entity.SetCreated(userID.ToString());
            entity.SetFrom(from);

            string entityID = await _repo.Add(entity);

            if (string.IsNullOrEmpty(entityID))
                return TResult<string>.Fail(ApplicationExMsg.FailAdd);

            return TResult<string>.OK(entityID);
        }
    }
}
