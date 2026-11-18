using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Abstractions.Progress;
using System.Application.Common;
using System.Application.Exceptions;
using System.Domain.Entities;
using System.Domain.Enums.Person;


namespace System.Application.Services.Progress.StudentStage.Command
{
    public class UpdateStudentStageCommandHandler : IRequestHandler<UpdateStudentStageCommand, TResult<string>>
    {
        private readonly IStudentStageRepository _repo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStudentAcademicEnrollmentRepository _academicEnrollmentRepository;
        private readonly IMapper _mapper;
        public UpdateStudentStageCommandHandler(IStudentStageRepository repo, IStudentAcademicEnrollmentRepository academicEnrollmentRepository, ICurrentUserService currentUser,
            IMapper mapper)
        {
            _repo = repo;
            _academicEnrollmentRepository = academicEnrollmentRepository;
            _currentUserService = currentUser;
            _mapper = mapper;
        }
        public async Task<TResult<string>> Handle(UpdateStudentStageCommand request, CancellationToken cancellationToken)
        {
          clsStudentStage? entity = await  _repo.GetEntityBy(request.StudentStageID, enStudentStageParamType.ID);
            if (entity == null)
                return TResult<string>.Fail( ApplicationExMsg.StudentStageNotFound);

            var academicEnrollment = await _academicEnrollmentRepository.GetBy(entity.StudentAcademicEnrollmentID);
            if (academicEnrollment == null)
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotFound);
            var targetDate = request.StudentStageDate == default
                ? DateOnly.FromDateTime(DateTime.Today)
                : request.StudentStageDate;
            if (academicEnrollment.Status != System.Domain.Enums.Enrollment.enStudentAcademicEnrollmentStatus.Active ||
                targetDate < academicEnrollment.ValidFrom ||
                (academicEnrollment.ValidTo.HasValue && targetDate > academicEnrollment.ValidTo.Value))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentNotActive);
            if (!academicEnrollment.Items.Any(item => item.StageID == entity.StageID.ToString()))
                return TResult<string>.Fail(ApplicationExMsg.StudentAcademicEnrollmentStageNotIncluded);

            if (_currentUserService.UserId is not Guid userID)
                return TResult<string>.Fail("المستخدم الحالي غير صالح");

           _mapper.Map<UpdateStudentStageCommand, clsStudentStage>(request, entity);
             entity.SetDate(targetDate);
             entity.SetModified(userID.ToString());

            await _repo.Update(entity);

            return TResult<string>.OK(string.Empty);
        }
    }
}
