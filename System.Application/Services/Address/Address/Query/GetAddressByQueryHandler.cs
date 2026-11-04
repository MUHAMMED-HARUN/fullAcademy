using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Repository;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Address.Address.DTO;
using System.Application.Services.Address.Address.Query;
using System.Application.Services.Institute.DTO;
using System.Domain.Enums.Address;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Address.Address.Query
{
    public sealed class GetAddressByQueryHandler : IRequestHandler<GetAddressByQuery, TResult<AddressInfo>>
    {
        IAddressRepository _repo;
        IInstituteRepository _instituteRepository;
        ICurrentUserService _currentUser;
        public GetAddressByQueryHandler(
            IAddressRepository repo,
            IInstituteRepository instituteRepository,
            ICurrentUserService currentUser)
        {
            _repo = repo;
            _instituteRepository = instituteRepository;
            _currentUser = currentUser;
        }

        public async Task<TResult<AddressInfo>> Handle(GetAddressByQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return TResult<AddressInfo>.Fail("غير مصرح بقراءة بيانات العنوان");

            if (!_currentUser.IsAdmin)
            {
                if (!_currentUser.HasPermission("Can_Read_Address"))
                    return TResult<AddressInfo>.Fail("غير مصرح بقراءة بيانات العنوان");

                if (request.ParamType == enAddressParamType.PersonID)
                {
                    var isTeacherOrStudent =
                        string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase);
                    if (!isTeacherOrStudent ||
                        !Guid.TryParse(request.Param, out var personId) ||
                        !_currentUser.EntityGuid.HasValue ||
                        personId != _currentUser.EntityGuid.Value)
                    {
                        return TResult<AddressInfo>.Fail(ApplicationExMsg.AddressNotFound);
                    }
                }
                else if (request.ParamType == enAddressParamType.InstituteID)
                {
                    var readScope = CreateInstituteReadScope();
                    if (readScope is null ||
                        !await _instituteRepository.IsExistBy(
                            request.Param,
                            enInstituteType.ID,
                            readScope))
                    {
                        return TResult<AddressInfo>.Fail(ApplicationExMsg.AddressNotFound);
                    }
                }
                else
                {
                    return TResult<AddressInfo>.Fail(ApplicationExMsg.AddressNotFound);
                }
            }

            var address = await _repo.GetBy(request.Param, request.ParamType);
            return address is null
                ? TResult<AddressInfo>.Fail(ApplicationExMsg.AddressNotFound)
                : TResult<AddressInfo>.OK(address);
        }

        private InstituteReadScope? CreateInstituteReadScope()
        {
            if (!_currentUser.HasPermission("Can_Read_Institute"))
                return null;

            if (string.Equals(_currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid teacherId && teacherId != Guid.Empty)
            {
                return new InstituteReadScope { TeacherID = teacherId };
            }

            if (string.Equals(_currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                _currentUser.EntityGuid is Guid studentId && studentId != Guid.Empty)
            {
                return new InstituteReadScope { StudentID = studentId };
            }

            return null;
        }
    }
}
