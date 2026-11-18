# StageInfo — Teacher Scoping & Current Stage

## 1. العيوب المكتشفة حالياً
- `StageInfoService.GetStudentStageInfo` يعيد **جميع** عناصر `StudentAcademicEnrollment.Items` (جميع المراحل/المواد في الخطة) حتى للمعلم.
- although `ApplyStudentAcademicEnrollmentReadScope` يقيّد `StudentAcademicEnrollment` (المعلم يرى طلاب مجموعاته فقط)، لا يوجد تقييد إضافي لـ `StageInfo` items بمرحلة الطالب الحالية.
- للمعلم، كل مراحل الخطة الأكاديمية تُعرض، بدل مرحلة الطالب الحالية (`enStageStateType.procecing`).
- `ICurrentUserService.EntityType/EntityGuid` متوفّران ويُستخدمان في scopes، لكن `GetStudentStageInfo` لا يفحص `IsAdmin` قبل `EntityType` (خطر فخ seed value 0).

## 2. خطة Backend
- الملف: `System.Infrastructure/Repositories/Progress/StageInfoService.cs`
  - تعديل `GetStudentStageInfo(string studentAcademicEnrollmentID)`:
    - إرجاع قائمة فارغة إذا `studentAcademicEnrollmentID` غير GUID/فارغ.
    - تطبيق `ApplyStudentAcademicEnrollmentReadScope(_currentUser, _context)` (موجود).
    - إذا `currentUser.IsAdmin`: إرجاع جميع items (السلوك الحالي).
    - إذا `EntityType == "Teacher"`:
      - تحديد مرحلة الطالب الحالية من `_context.StudentStages` مع `ApplyStudentStageReadScope(_currentUser, _context)` و `State == enStageStateType.procecing` للـ `studentAcademicEnrollmentID`.
      - إذا وُجدت مرحلة حالية: تصفية `enrollment.Items` إلى `item.StageID == currentStageId`.
      - إذا لم تُوجد: إرجاع قائمة فارغة (أو جميع items? قرار: فارغة، لأن المطلوب مرحلة حالية).
    - إذا `EntityType == "Student"`: إرجاع جميع items (سلوك حالي) (أو تقييد لمرحلة حالية؟ المطلوب للمعلم فقط).
    - Fallback: `query.Where(_ => false)` equivalent (empty).
  - Need check `IsAdmin` before `EntityType`.
- الملف: `System.Application/Abstractions/Progress/StageInfoRepository.cs` (no signature change; keep `GetStudentStageInfo`).
- الملف: `System.Application/Services/Progress/StageInfo/Query/GetStudentStageInfoQuery.cs` (no change; keep `StudentAcademicEnrollmentID`).
- الملف: `System.Infrastructure/Extensions/Security/StudentStageScopeExtensions.cs` (reuse; ensure teacher scope uses academic enrollments).
- Enum: `System.Domain/Enums/Progress/enProgressParamType.cs` (`enStageStateType.procecing`).

## 3. Frontend (no change required by this plan, but verify)
- `StageInfoStore.getStudentStageInfo` already calls endpoint; no changes.
- DailyEvaluation/StudentStage flows will receive fewer items for teachers (current stage only), which is intended.

## 4. UI/UX
- No UI changes; lists become shorter for teachers.

## 5. توجيهات المستخدم
- "teacher fetches only own data": enforced via `ApplyStudentAcademicEnrollmentReadScope` + `ApplyStudentStageReadScope`.
- "teacher fetches current stage records": filter items to `State == procecing`.
- Admin unaffected.
- Note: property typo `procecing` must be used as-is.
