# Class / Enrollment / Payment — Improvement Plan

> **الهدف:** تحسين Class Feature + إضافة GlobalSearchByFilter + enrollment attendance status + payment injection + pagination/filter endpoint.
> **الالتزام:** `.ai-rules` (architecture, theme, controls, modals, forms, notifications, pinia).

---

## 1. ملخص المطلوب

### A. Enrollment Student View
- جلب حالة الحضور/الغياب للطالب (`AttendanceStatus`).
- لا يمكن عمل متابعة يومية (Daily Evaluation) إذا كان الطالب غائباً أو لديه إذن (`Absent`/`Excused`).
- عرض Badge حالة (Present/Excused/Absent) في القائمة.

### B. GenericSearchByFilter Component (✅ تم)
- `src/components/general/GenericSearchByFilter.vue`
- يتكون من: `FilterTypeSelect` + `TextInput` + `SearchButton`.
- عند نتائج متعددة → `popapList` (popup) لاختيار سجل.
- عند نتيجة واحدة → تعبئة `id` + `name` مباشرة.
- **الاستخدام في Class:** البحث عن `programID` (تم تطبيقه في `UpserClassComp.vue`).

### C. Backend — New Endpoint (pagination + filter)
- إنشاء `GetClassListPagination` (أو `GetStudentListPagination withFilter`) on pattern of `GetStudentList`.
- Query params: `filter`, `orderBy`, `page`, `pageSize` (Gridify).
- Repository/Service/Store: تحديث لدعم pagination + filter (مثل FeeStructure).

### D. DB Rebuild
- إعادة إنشاء migration (drop + recreate) بعد تعديل entities (Attendance, Payment).
- `dotnet ef migrations add` + `database update`.

### E. Payment Injection (Infrastructure)
- إضافة `IPaymentService` / `PaymentRepository` injection في `InfrastructureServiceRegistration`.
- إضافة `Payment` entity + configuration + migration.

### F. Institute & Address completion
- تطبيق نمط Program على Institute (✅ تم) + Class (✅ GenericSearch).
- Address: تطبيق نفس النمط (Tailwind + GenericComboActions + search).

---

## 2. خطة التنفيذ (Phases)

### Phase 1 — Backend
- [ ] إضافة `GetClassListPagination` endpoint في `ClassController`.
- [ ] إضافة `GetAllClassQuery` with filter/pagination params.
- [ ] Payment injection (`InfrastructureServiceRegistration`).
- [ ] Migration rebuild (Attendance + Payment entities).
- [ ] Attendance status endpoint for student (for enrollment view).

### Phase 2 — Frontend Architecture (Class)
- [x] `GenericSearchByFilter.vue` (✅ created).
- [x] `UpserClassComp.vue` uses it for `programID` (✅).
- [ ] `ClassStore` → pagination state (page/pageSize/totalCount) + filter.
- [ ] `ClassView` → `GenericFilterComp` + `PaginationComp` + `GenericComboActions`.
- [ ] `ClassList` → Tailwind table (like ProgramList).

### Phase 3 — Enrollment Student View
- [ ] Add attendance status fetch in `StudentEnrollmentStore`/view.
- [ ] Disable daily evaluation action when status is `Absent`/`Excused`.
- [ ] Badge for status (success/warning/error).

### Phase 4 — Institute & Address
- [x] Institute on Program pattern (✅).
- [ ] Address views on same pattern (Tailwind + combo actions + search).

---

## 3. قواعد (من `.ai-rules`)
1. لا Repository مباشرة من Component — عبر Service → Store.
2. Setup/Options Store (FeeStructure uses Options Store → keep consistent).
3. `storeToRefs` للحالة التفاعلية.
4. `withBusy` pattern (أو manual loading/error كما في FeeStructure).
5. Tailwind tokens فقط (`brand-*`, `gray-*`), لا hex.
6. RTL: `ms-*`/`me-*`/`ps-*`/`pe-*`/`start-*`/`end-*`/`text-start`.
7. Dark mode pairs على كل سطح.
8. لا `window.confirm`/`alert()` — Dialog + toast.
9. Pagination + filter via `applyClientQuery` (client) أو server-side (Gridify).

---

## 4. Checklist (ملفات)

```
□ src/components/general/GenericSearchByFilter.vue   ✅ created
□ src/components/Institute/Class/UpserClassComp.vue  ✅ uses GenericSearchByFilter
□ src/views/Institute/ClassView.vue                  ⏳ add pagination/filter/combo actions
□ src/components/Institute/Class/ClassList.vue       ⏳ Tailwind table + selection
□ src/store/Institute/ClassStore.js                  ⏳ pagination state
□ src/views/Enrollment/StudentEnrollmentView.vue     ⏳ attendance status
□ Backend: ClassController.GetClassListPagination    ⏳
□ Backend: Payment injection                         ⏳
□ Backend: migration rebuild                         ⏳
□ Backend: Attendance status endpoint                 ⏳
```

---

## 5. مخاطر
- Backend migration rebuild يُفقد بيانات dev — acceptable for dev.
- Payment injection يتطلب `IPaymentService` registration.
- GenericSearchByFilter يعتمد على `results` prop من parent — parent must fetch list.
- ClassView currently uses `fetchClasses()` (no pagination) — adding pagination changes store contract.

---

## 6. Next Steps (أولوية)
1. Backend: إضافة pagination endpoint (Class + Student).
2. Frontend: ClassView + ClassList + ClassStore pagination/filter.
3. Frontend: Enrollment attendance status.
4. Frontend: Address on Program pattern.
5. DB migration rebuild + Payment injection.