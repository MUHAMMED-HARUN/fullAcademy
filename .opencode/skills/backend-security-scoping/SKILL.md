---
name: backend-security-scoping
description: Enforce authentication, standard permission generation/activation per entity, role-permission matrices, and ownership-based query scoping (ABAC/RBAC) across WebApi, Application, Infrastructure, and Frontend in School_Academy.
---

# الأمان، توليد وتوزيع الصلاحيات، ونطاق ملكية البيانات (ABAC & Query Scoping)

استخدم هذه المهارة إلزامياً عند التخطيط أو بناء أو تحسين أي ميزة (Feature/Service) في المشروع. لا تترك أي ميزة جديدة أو معدلة بدون تعريف وتفعيل صلاحياتها ونطاق ملكية بياناتها.

## 1. عقد الهوية (`ICurrentUserService`) والـ JWT Claims

التوكن يصدر Claims مخصصة بالأسماء التالية حرفياً: `UserId`, `UserName`, `Email`, `EntityId`, `EntityType`, `Role`, `Permission`.

يجب أن توفر `ICurrentUserService` (وتنفيذها `CurrentUserService`) الخصائص والدوال التالية:
- `Guid? UserId` و `Guid? EntityGuid => clsUtil.ToGuid(EntityId)`
- `string? EntityType` (`"Admin"`, `"Teacher"`, `"Student"`)
- `List<string> Roles` و `List<string> Permissions` (تُقرأ من `FindAll("Permission")`)
- `bool IsAdmin => Roles.Contains("Admin")`
- `bool HasPermission(string permission) => IsAdmin || Permissions.Contains(permission)`

**قاعدة ذهبية لتفادي فخ الـ Seed:** افحص دائماً `currentUser.IsAdmin` (أو `Roles.Contains("Admin")`) **قبل** فحص `EntityType`، لأن `enUserType` يبدأ بـ `Teacher = 0` كقيمة افتراضية.

## 2. القاعدة الإلزامية لتوليد وتفعيل الصلاحيات مع كل ميزة (Permission Lifecycle)

عند العمل على أي كيان (`<Entity>`) في أي ميزة (سواء كانت جديدة أو قيد التحسين)، يجب التحقق من وجود صلاحياته وإنشاؤها وتفعيلها عبر الخطوات الأربع التالية:

### أ) التسمية القياسية في `clsPermission`:
لكل كيان، يجب أن تتوفر الصلاحيات القياسية التالية في `clsPermission` (مصنفة حسب `Type = "<ModuleOrEntity>"`):
- `Can_Read_<Entity>`: لعرض القوائم (`GetList`, `GetListFilter`) والتفاصيل (`GetBy`, `get-for-upsert`).
- `Can_Create_<Entity>`: لإنشاء سجل جديد (`New/Create`).
- `Can_Update_<Entity>`: لتعديل سجل قائم (`Update`).
- `Can_Delete_<Entity>`: لحذف سجل (`Delete`).
- **صلاحيات إجرائية خاصة (عند الحاجة):** مثل `Can_Register_User`, `Can_SetPermission_User`, `Can_Take_Attendance`, `Can_Submit_Exam`, `Can_Grade_Exam`.

### ب) التسجيل التلقائي في Policies والـ Seed:
- تأكد من أن `GetAllPermissionHandler` / `clsPermission` يرجع الصلاحيات الجديدة لكي يقوم `Program.cs` بتسجيلها تلقائياً كـ `Authorization Policies`.
- يُمنع استخدام صيغ متضاربة في `ModelBuilderExtensions` (مثل `Permissions.Users.View`)؛ يجب أن يقرأ الـ Seed من ثوابت `clsPermission` مباشرة.

### ج) التفعيل على مستوى الـ Controller (`WebApi`):
- ضع `[Authorize]` على مستوى الـ Controller بالكامل (باستثناء `Login` في `AuthController`).
- اربط كل عملية بالـ Policy المقابلة لها:
  - نقاط القراءة (`GetList`, `GetListFilter`, `GetBy`, `get-for-upsert`): `[Authorize(Policy = "Can_Read_<Entity>")]`.
  - نقطة الإنشاء (`New`/`Create`): `[Authorize(Policy = "Can_Create_<Entity>")]`.
  - نقطة التعديل (`Update`): `[Authorize(Policy = "Can_Update_<Entity>")]`.
  - نقطة الحذف (`Delete`): `[Authorize(Policy = "Can_Delete_<Entity>")]`.

### د) التفعيل في الواجهة الأمامية (`Frontend`):
- استخرج مطالبات `Permission` و `Role` و `EntityId` و `EntityType` من الـ JWT في `authStore`.
- استخدم `authStore.hasPermission('Can_Create_<Entity>')`، `Can_Update_<Entity>`، `Can_Delete_<Entity>` لإظهار أو إخفاء أزرار الإضافة والتعديل والحذف وعناصر القائمة الجانبية.

## 3. المصفوفة القياسية لتوزيع الصلاحيات حسب الكيان والدور (Role-Permission Matrix)

وزّع الصلاحيات افتراضياً على الأدوار وفق الجدول التالي (مع تقييد القراءة والتعديل فعلياً في قاعدة البيانات عبر `ApplyReadScope`):

1. **كيانات الهوية (`User`, `Role`, `Permission`):**
   - `Admin`: كامل الصلاحيات (`Can_Read_User`, `Can_Register_User`, `Can_Update_User`, `Can_Delete_User`, `Can_SetPermission_User`).
   - `Teacher` / `Student`: بدون صلاحيات إدارة مستخدمين (فقط ملفه الشخصي).
2. **البيانات المرجعية والعناوين (`Country`, `City`, `District`, `Neighborhood`, `Address`, `Stage`):**
   - `Admin`: كامل الصلاحيات (`Read, Create, Update, Delete`).
   - `Teacher` / `Student`: `Can_Read_*` فقط (لعرض العناوين والاختيار عند الحاجة).
3. **الهيكل الأكاديمي (`Institute`, `Program`, `Class`, `InstituteClass`, `Group`, `Subject`):**
   - `Admin`: كامل الصلاحيات (`Read, Create, Update, Delete`).
   - `Teacher`: `Can_Read_*` (مقيدة برمجياً بالمجموعات والصفوف والمعاهد المسندة إليه عبر `clsTeachingAssignment`).
   - `Student`: `Can_Read_*` (مقيدة برمجياً بالمجموعات والصفوف والمعاهد المسجل فيها عبر `clsStudentEnrollment`).
4. **الأشخاص (`Person`, `Student`, `Teacher`):**
   - `Admin`: كامل الصلاحيات (`Read, Create, Update, Delete`).
   - `Teacher`: `Can_Read_Teacher` (نفسه)، `Can_Read_Student` و `Can_Read_Person` (مقيدة بطلاب مجموعاته ونفسه فقط).
   - `Student`: `Can_Read_Student` و `Can_Read_Person` (مقيدة بسجله الشخصي فقط).
5. **التسجيل والتكليف والحضور (`StudentEnrollment`, `TeachingAssignment`, `Attendance`):**
   - `Admin`: كامل الصلاحيات (`Read, Create, Update, Delete`).
   - `Teacher`: `Can_Read_TeachingAssignment` (تكليفاته)، `Can_Read_StudentEnrollment` (طلاب مجموعاته)، و `Can_Read_Attendance`, `Can_Create_Attendance`, `Can_Update_Attendance` (رصد وتعديل حضور طلاب مجموعاته فقط).
   - `Student`: `Can_Read_StudentEnrollment` و `Can_Read_Attendance` (لسجلاته الشخصية فقط).
6. **المالية (`Finance` / `Payment` / `Fee`):**
   - `Admin`: كامل الصلاحيات (`Read, Create, Update, Delete`).
   - `Teacher`: لا يملك وصولاً لبيانات الطلاب المالية.
   - `Student`: `Can_Read_*` (لمدفوعاته وأقساطه الخاصة فقط).

## 4. عزل نطاق البيانات (Query Scoping Extensions)

الصلاحية (`Can_Read_<Entity>`) تسمح بدخول الـ Endpoint، لكن **نطاق الملكية (`ApplyReadScope`)** يحدد أي صفوف (Rows) يحق للمستخدم رؤيتها:

1. **لا تضع شروط الأدوار والملكية داخل الـ Handler ولا تكررها يدوياً في دوال الـ Repository.**
2. أنشئ دوال امتداد مركزية `IQueryable<TEntity>` في `System.Infrastructure/Extensions/Security/` (مثل `InstituteScopeExtensions`, `StudentScopeExtensions`).
3. **ترتيب الاستعلام الإلزامي داخل الـ Repository (`GetListFilter` و `GetBy`):**
   - ابدأ بـ `_dbSet.AsNoTracking()`.
   - **يُمنع استخدام `.IgnoreQueryFilters()`** في استعلامات العرض والبحث حفاظاً على فلتر الحذف الناعم (`DeletedAt == null`).
   - طبّق دالة النطاق الأمني `.ApplyReadScope(_currentUser)` **على الكيان الأصلي قبل `.Select(...)`**.
   - نفّذ الإسقاط `.Select(x => new Dto { ... })`.
   - طبّق `.GridifyAsync(request)` في النهاية على النطاق المصفّى أمنياً.

## 5. مسارات العلاقات الفعلية في `System.Domain` لبناء الـ Scope

التزم بخصائص التنقل الحقيقية في المشروع:
- **الإدارة (`currentUser.IsAdmin`):** يرجع `query` كما هو دون تقييد.
- **المعلم (`EntityType == "Teacher"` و `EntityGuid != Guid.Empty`):**
  - يرتبط بالمجموعات عبر **`clsTeachingAssignment`** (`TeacherID`, `GroupID`, `SubjectID`).
  - نطاقه في `clsGroup`: المجموعات الموجودة في `clsTeachingAssignment` حيث `TeacherID == currentUser.EntityGuid`.
  - نطاقه في `clsInstituteClass` و `clsInstitute`: عبر `clsTeachingAssignment -> Group -> InstituteClass -> Institute`.
  - نطاقه في `clsStudent`: الطلاب المسجلون عبر **`clsStudentEnrollment`** في مجموعة يدرّسها هذا المعلم.
- **الطالب (`EntityType == "Student"` و `EntityGuid != Guid.Empty`):**
  - في `clsStudent`: `s.Id == currentUser.EntityGuid`.
  - في `clsGroup` / `clsInstituteClass` / `clsInstitute`: ما يرتبط بتسجيلاته في **`clsStudentEnrollment`** (`StudentID == currentUser.EntityGuid`).
- **الحالة الافتراضية (Fallback):** إذا لم يكن المستخدم مصادقاً أو لم يطابق أي نطاق مسموح، أرجع `query.Where(_ => false)`.
