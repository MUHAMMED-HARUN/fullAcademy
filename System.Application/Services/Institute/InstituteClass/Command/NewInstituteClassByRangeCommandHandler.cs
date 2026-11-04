using AutoMapper;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Class.DTO;
using System.Application.Services.Institute.InstituteClass.DTO;
using System.Application.Services.Institute.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.InstituteClass.Command
{
    public class NewInstituteClassByRangeCommandHandler
        : IRequestHandler<NewInstituteClassByRangeCommand, TResult<InstituteClassBulkResult>>
    {
        private readonly IInstituteClassRepository _instituteClassRepository;
        private readonly IClassRepository _classRepository;
        private readonly IInstituteRepository _instituteRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public NewInstituteClassByRangeCommandHandler(
            IInstituteClassRepository instituteClassRepository,
            IClassRepository classRepository,
            IInstituteRepository instituteRepository,
            ICurrentUserService currentUser,
            IMapper mapper)
        {
            _instituteClassRepository = instituteClassRepository;
            _classRepository = classRepository;
            _instituteRepository = instituteRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<TResult<InstituteClassBulkResult>> Handle(
            NewInstituteClassByRangeCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return TResult<InstituteClassBulkResult>.Fail("يجب تسجيل الدخول لإضافة الصفوف إلى المعهد");

            if (!_currentUser.IsAdmin && !_currentUser.HasPermission("Can_Create_InstituteClass"))
                return TResult<InstituteClassBulkResult>.Fail("ليس لديك صلاحية ربط الصفوف بالمعهد");

            var instituteReadScope = CreateInstituteReadScope();
            var classReadScope = CreateClassReadScope();
            if (instituteReadScope is null || classReadScope is null)
                return TResult<InstituteClassBulkResult>.Fail("لا يمكن ربط صفوف خارج نطاق المستخدم");

            if (_currentUser.UserId is not Guid userId || userId == Guid.Empty)
                return TResult<InstituteClassBulkResult>.Fail("تعذر تحديد المستخدم الحالي");

            if (!Guid.TryParse(request.InstituteID, out var instituteID) || instituteID == Guid.Empty)
                return TResult<InstituteClassBulkResult>.Fail("معرّف المعهد غير صالح");

            var classIDs = new List<Guid>();
            foreach (var classID in request.ClassIDs ?? new List<string>())
            {
                if (!Guid.TryParse(classID, out var parsedClassID) || parsedClassID == Guid.Empty)
                    return TResult<InstituteClassBulkResult>.Fail("أحد معرّفات الصفوف غير صالح");

                if (!classIDs.Contains(parsedClassID))
                    classIDs.Add(parsedClassID);
            }

            if (classIDs.Count == 0)
                return TResult<InstituteClassBulkResult>.Fail("يجب اختيار صف واحد على الأقل");

            if (!await _instituteRepository.IsExistBy(
                    instituteID.ToString(),
                    enInstituteType.ID,
                    instituteReadScope))
                return TResult<InstituteClassBulkResult>.Fail("المعهد المحدد غير موجود");

            var existingClassIDs = await _classRepository.GetExistingClassIDs(classIDs, classReadScope);
            var existingClassIDSet = existingClassIDs.ToHashSet();
            var missingClassIDs = classIDs.Where(classID => !existingClassIDSet.Contains(classID)).ToList();
            if (missingClassIDs.Count > 0)
                return TResult<InstituteClassBulkResult>.Fail("بعض الصفوف المحددة غير موجودة أو محذوفة");

            var alreadyLinkedClassIDs = await _instituteClassRepository
                .GetAlreadyLinkedClassIDs(instituteID, classIDs);
            var alreadyLinkedSet = alreadyLinkedClassIDs.ToHashSet();
            var added = 0;

            foreach (var classID in classIDs.Where(id => !alreadyLinkedSet.Contains(id)))
            {
                var command = new NewInstituteClassCommand
                {
                    ClassID = classID.ToString(),
                    InstituteID = instituteID.ToString(),
                };
                var entity = _mapper.Map<NewInstituteClassCommand, clsInstituteClass>(command);
                entity.SetCreated(userId.ToString());

                if (!string.IsNullOrWhiteSpace(await _instituteClassRepository.Add(entity)))
                    added++;
            }

            return TResult<InstituteClassBulkResult>.OK(new InstituteClassBulkResult
            {
                AddedCount = added,
                SkippedAlreadyLinkedClassIDs = classIDs
                    .Where(alreadyLinkedSet.Contains)
                    .Select(id => id.ToString())
                    .ToList(),
            });
        }

        private InstituteReadScope? CreateInstituteReadScope()
        {
            if (_currentUser.IsAdmin)
                return new InstituteReadScope { IsAdmin = true };
            if (!_currentUser.HasPermission("Can_Read_Institute"))
                return null;
            if (string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
                return new InstituteReadScope { TeacherID = teacherID };
            if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
                return new InstituteReadScope { StudentID = studentID };
            return null;
        }

        private ClassReadScope? CreateClassReadScope()
        {
            if (_currentUser.IsAdmin)
                return new ClassReadScope { IsAdmin = true };
            if (!_currentUser.HasPermission("Can_Read_Class"))
                return null;
            if (string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
                return new ClassReadScope { TeacherID = teacherID };
            if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
                return new ClassReadScope { StudentID = studentID };
            return null;
        }
    }
}
