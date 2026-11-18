using Gridify;
using MediatR;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.InstituteClass.DTO;
using System.Application.Services.Institute.InstituteClass.Query;
using System.Application.Services.Institute.Program.DTO;

namespace System.Application.Services.Institute.InstituteClass.Query
{
    public sealed class GetInstituteClassListFilterQueryHandler
        : IRequestHandler<GetInstituteClassListFilterQuery, TResult<Paging<InstituteClassInfo>>>
    {
        private readonly IInstituteClassRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public GetInstituteClassListFilterQueryHandler(
            IInstituteClassRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<TResult<Paging<InstituteClassInfo>>> Handle(
            GetInstituteClassListFilterQuery request,
            CancellationToken cancellationToken)
        {
            var scope = InstituteClassReadScopeFactory.Create(_currentUser);
            if (scope == null)
                return TResult<Paging<InstituteClassInfo>>.Fail("غير مصرح بقراءة ربط الصفوف بالمعاهد");

            return TResult<Paging<InstituteClassInfo>>.OK(
                await _repository.GetListFilter(request, scope));
        }
    }
}
