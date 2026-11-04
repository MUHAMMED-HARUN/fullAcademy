# Student Feature — Improvement Plan

> **الهدف:** تحسين ميزة Student على نمط Person، وتحسين تدفق enrollStudent (التسجيل) ليستخدم `UpsertStudentEnrollmentComp` مع تعبئة بيانات Student، واستخدام `SearchFilterCombo` (GenericSearchByFilter) للبحث عن Group مع endpoint `GetAllGroup` (pagination/filter).

> **المراجع:** `.ai-rules` (00-architecture, 01-theme, 02-controls, 03-modals, 04-forms, 05-notifications, 06-pinia, 07-backend).

---

## 1. الوضع الحالي (As-Is)

- `Student` frontend (Options API) lacks Person-level UX: no `GenericComboActions`, no row selection, no unified filter/pagination contract consistent with Person.
- `UpsertStudentEnrollmentComp` uses manual select+input for Student/Group search; Group search depends on `GroupStore.groups` (no pagination/filter) and `GroupStore.fetchGroupBy` (single).
- Backend `GetAllGroupQuery` is a simple `IQuery` returning `List<GroupInfo>` (no Gridify pagination/filter). `GroupController.GetGroupList` does not accept query params.
- `GroupRepository`/`GroupService` (frontend) do not support filter/paging like Person.

## 2. الهدف (To-Be)

1. **Student (frontend)** becomes on Person pattern: filter schema + combo actions + selection + pagination + card/info popup.
2. **enrollStudent action** uses `UpsertStudentEnrollmentComp`, auto-filling Student data (selected person → student) and Group via `SearchFilterCombo`.
3. **Group search** in `UpsertStudentEnrollmentComp` uses `SearchFilterCombo` (`GenericSearchByFilter`) instead of manual inputs.
4. **Backend** creates `GetAllGroup` endpoint inheriting `BaseFilterQuery<GroupInfo>` (pagination/filter via Gridify), following `GetAllPersonQuery` pattern.
5. **Group search** in `SearchFilterCombo` calls `GetAllGroup` (frontend `GroupStore.fetchGroups(filter)`).

> ملاحظة: النص الأصلي ذكر `BaseFilterQuery<StudentInfo>`؛ للـ Group DTO الصحيح هو `GroupInfo`. إذا تطلب التحقق حرفياً `StudentInfo`، فعدّل DTO وفقاً لـ GroupInfo (لا تخلط).

---

## 3. Backend Changes

### 3.1 Query
**File:** `System.Application/Services/Institute/Group/Query/GetAllGroupQuery.cs`
```csharp
using System.Application.Common.Models;
using System.Application.Services.Institute.Group.DTO;

namespace System.Application.Services.Institute.Group.Query
{
    public class GetAllGroupQuery : BaseFilterQuery<GroupInfo> { }
}
```

### 3.2 Handler
**File:** `System.Application/Services/Institute/Group/Query/GetAllGroupQueryHandler.cs`
```csharp
using Gridify;
using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Group.DTO;

namespace System.Application.Services.Institute.Group.Query
{
    class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, TResult<Paging<GroupInfo>>>
    {
        private readonly IGroupRepository _repo;
        public GetAllGroupQueryHandler(IGroupRepository repo) { _repo = repo; }

        public async Task<TResult<Paging<GroupInfo>>> Handle(GetAllGroupQuery request, CancellationToken ct)
        {
            return TResult<Paging<GroupInfo>>.OK(await _repo.GetListFilter(request));
        }
    }
}
```

### 3.3 Repository Abstraction
**File:** `System.Application/Abstractions/Institute/GroupRepository.cs` (interface `IGroupRepository`)
Add:
```csharp
using Gridify;
// ...
Task<Paging<GroupInfo>> GetListFilter(GetAllGroupQuery request);
```

### 3.4 Repository Implementation
**File:** `System.Infrastructure/Repositories/Institute/Institute/GroupService.cs`
Add:
```csharp
public async Task<Paging<GroupInfo>> GetListFilter(GetAllGroupQuery request)
{
    return await _dbSet
        .Where(i => i.DeletedAt == null)
        .Select(i => new GroupInfo
        {
            GroupID = i.Id.ToString(),
            GroupName = i.GroupName,
            Code = i.Code,
            InstituteClassID = i.InstituteClassID.ToString(),
            ClassName = i.InstituteClass.Class.Name,
            InstituteName = i.InstituteClass.Institute.Name
        })
        .GridifyAsync(request);
}
```
(Requires `using Gridify;` and `using System.Application.Services.Institute.Group.Query;`.)

### 3.5 Controller
**File:** `Controllers/Institute/GroupController.cs`
Update `GetGroupList`:
```csharp
[HttpGet("GetGroupList")]
public async Task<IActionResult> GetList([FromQuery] GetAllGroupQuery query)
{
    var result = await _mediator.Send(query);
    if (!result.IsSuccess) return BadRequest(new { errors = result.Error });
    return Ok(result);
}
```

### 3.6 DI/Registration
No new registration needed (IGroupRepository already registered). Verify `InfrastructureServiceRegistration`.

---

## 4. Frontend Changes

### 4.1 GroupRepository (frontend)
**File:** `src/repositories/Institute/GroupRepositories.js`
- `getList(filter)` already passes `{ params: filter }`. Keep; ensure called with filter object.

### 4.2 GroupService (frontend)
**File:** `src/Service/Institute/GroupService.js`
Update `getAll(filter)` to map Paging like PersonService:
```js
async getAll(filter) {
  const rees = await Group_repositories.getList(filter);
  if (rees.success) {
    const pagingObj = rees.data || {};
    const rawList = pagingObj.Data;
    const totalCount = pagingObj.Count ?? 0;
    const dto = GroupMapper.mapListApiToGroup(rawList);
    return new TResult({ items: dto, count: totalCount }, rees.success, rees.errors);
  }
  return new TResult(null, false, rees.errors);
}
```
(Ensure GroupMapper has `mapListApiToGroup`; otherwise add.)

### 4.3 GroupStore (frontend)
**File:** `src/store/Institute/GroupStore.js`
Add paging state + `fetchGroups(filter)` supporting Page/PageSize/Filter like PersonStore:
```js
state: () => ({
  groups: [],
  selectedGroup: null,
  loading: false,
  error: null,
  page: 1,
  pageSize: 10,
  totalCount: 0,
}),
getters: { totalPages: (s) => Math.ceil(s.totalCount / s.pageSize) || 1 },
actions: {
  async fetchGroups(filter = {}) {
    this.loading = true; this.error = null;
    if (filter.Page !== undefined) this.page = filter.Page;
    if (filter.PageSize !== undefined) this.pageSize = filter.PageSize;
    const queryParams = { Page: this.page, PageSize: this.pageSize, ...filter };
    const result = await Group_service.getAll(queryParams);
    if (result.success) {
      this.groups = result.data?.items || [];
      this.totalCount = result.data?.count || 0;
    } else {
      this.error = result.errors;
    }
    this.loading = false;
    return result;
  },
  async setPage(newPage) { if (newPage >= 1 && newPage <= this.totalPages) { this.page = newPage; return await this.fetchGroups(); } },
  async setPageSize(newSize) { this.pageSize = newSize; this.page = 1; return await this.fetchGroups(); },
  // keep fetchGroupBy, GetGroupsByTeacherID, CRUD...
}
```

### 4.4 Student on Person pattern (frontend)
**Files:**
- `src/views/Person/StudentView.vue`: add `GenericComboActions`, selection state (`selectedStudentIds`), toggle handlers, actions list (info/update/delete) similar to PersonView; use `StudentStore.fetchStudents`.
- `src/components/Person/Student/StudentList.vue`: add checkbox column, emit toggle-select-all/toggle-select-row, action buttons.
- `src/components/Person/Student/UpsertStudentComp.vue`: optionally replace person search with `GenericSearchByFilter` (idField personID/nameField firstName). Keep existing fallback.

### 4.5 enrollStudent flow (frontend)
**File:** `src/views/Enrollment/StudentEnrollmentView.vue`
- `OnUpsertEnrollment(null)` already clears selectedStudent/Group. Keep.
- Pass `:group-results="GroupStore.groups"` to `UpsertStudentEnrollmentComp`.
- Handle `@on-search-group` by calling `GroupStore.fetchGroups({ Filter: buildGroupFilter(type, val) })` instead of (or in addition to) `fetchGroupBy`.
- `onSearchStudent` already fills `StudentStore.selectedStudent`; pass as `:student-data`. Ensure `UpsertStudentEnrollmentComp` fills `formData.studentID` (already via watch). Add explicit fill of any additional Student fields if needed.

**File:** `src/components/Enrollment/StudentEnrollment/UpsertStudentEnrollmentComp.vue`
- Replace Group search block (select+input+button) with:
```vue
<GenericSearchByFilter
  :filter-types="groupFilterTypes"
  :results="groupResults"
  id-field="groupID"
  name-field="groupName"
  @search="onSearchGroup"
  @select="onSelectGroup"
/>
```
- Add props: `groupResults: Array`.
- Add `groupFilterTypes` mapping GroupName/Code/ID.
- `onSearchGroup(payload)` emits `onSearchGroup(payload.filterType, payload.value)` (or builds filter and calls parent).
- `onSelectGroup(item)` sets `formData.groupID = item.groupID`, `showGroupCard = true`, emits update of groupData.
- Modify `handleStudent` to fill `formData.studentID = data.studentID || data.id` and show card; also fill any student display fields (name, national number) if available.
- Keep date input and submit.

### 4.6 SearchFilterCombo naming
If project prefers component named `SearchFilterCombo`, either:
- Rename `GenericSearchByFilter.vue` → `SearchFilterCombo.vue` and update imports, OR
- Create `SearchFilterCombo.vue` as a thin wrapper/alias importing GenericSearchByFilter.
Recommend alias to avoid breaking existing usages.

---

## 5. File Checklist

Backend:
- [ ] `System.Application/Services/Institute/Group/Query/GetAllGroupQuery.cs`
- [ ] `System.Application/Services/Institute/Group/Query/GetAllGroupQueryHandler.cs`
- [ ] `System.Application/Abstractions/Institute/GroupRepository.cs` (IGroupRepository)
- [ ] `System.Infrastructure/Repositories/Institute/Institute/GroupService.cs`
- [ ] `Controllers/Institute/GroupController.cs`

Frontend:
- [ ] `src/repositories/Institute/GroupRepositories.js`
- [ ] `src/Service/Institute/GroupService.js`
- [ ] `src/store/Institute/GroupStore.js`
- [ ] `src/Mapper/Institute/GroupMapper.js` (ensure mapListApiToGroup)
- [ ] `src/components/general/SearchFilterCombo.vue` (alias or rename)
- [ ] `src/components/Enrollment/StudentEnrollment/UpsertStudentEnrollmentComp.vue`
- [ ] `src/views/Enrollment/StudentEnrollmentView.vue`
- [ ] `src/views/Person/StudentView.vue`
- [ ] `src/components/Person/Student/StudentList.vue`
- [ ] `src/components/Person/Student/UpsertStudentComp.vue` (optional)

---

## 6. Phases

1. Backend: GetAllGroupQuery + handler + repo + controller.
2. Frontend Group: repository/service/store paging.
3. Frontend Enrollment: SearchFilterCombo for Group + fill Student data.
4. Frontend Student: Person-pattern refactor (filter/combo actions/selection).
5. Build/test: `dotnet build`, `npm run serve/build`, smoke test enrollment.

## 7. Risks / Notes

- Gridify field names must match DTO properties (`GroupName`, `Code`, etc.).
- `BaseFilterQuery<T>` returns `Paging<T>`; update all consumers expecting `List<GroupInfo>`.
- GroupStore.groups now holds mapped GroupInfo (not raw). Ensure `UpsertTeachingAssignmentComp` and others still work (they may expect GroupInfo). Verify.
- Avoid breaking `GetGroupsByTeacherID` (returns `List<clsGroup>` mapped separately).
- Keep soft-delete global filter; add `Where(DeletedAt == null)` in GetListFilter for safety (Gridify may include deleted if not filtered).

---

## 8. Acceptance Criteria

- `GET /api/Group/GetGroupList?Page=1&PageSize=10&Filter=GroupName=*x*` returns `{ Data, Count }`.
- In enrollment form, searching Group uses SearchFilterCombo and displays popup for multiple matches; selecting sets `groupID`.
- Student data auto-fills when a student is found (card + ID).
- Student view has filter/combo actions/selection/pagination consistent with Person.
