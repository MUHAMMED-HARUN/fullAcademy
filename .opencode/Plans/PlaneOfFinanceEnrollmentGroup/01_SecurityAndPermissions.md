# خطة SecurityAndPermissions

## 1. العيوب المكتشفة حالياً
- `System.Domain/Entities/Permissions/clsPermission.cs` لا يسجل صلاحيات AcademicPlan أو StudentAcademicEnrollment أو Finance.
- `GroupController`, `StudentStageController`, `AttendancesController` وControllers المالية لا تظهر عليها سياسات `[Authorize]` في العقود المفحوصة؛ `StudentEnrollmentController` وحده عليه `[Authorize]` وسياسات CRUD.
- `GroupService.GetListFilter` يستخدم `IgnoreQueryFilters()`، و`GetGroupsByTeacherID` يقبل TeacherID من العميل. لا توجد Scopes للفواتير والاشتراكات الجديدة.

## 2. خطة Backend
- في `clsPermission.AllPermissions()` تسجيل CRUD القياسي للكيانات `AcademicPlan`, `StudentAcademicEnrollment`, `StudentEnrollment`, `Group`, `StudentStage`, `DailyEvaluation`, `AssignmentNomination`, `Assignment`, `Result`, `FeeStructure`, `StudentInvoice`, `Payment`, `Attendance`.
- إضافة صلاحيات إجرائية عند اعتمادها: `Can_Enroll_StudentAcademicPlan`, `Can_Generate_StudentInvoice`, `Can_Record_Payment`, `Can_FollowUp_StudentFinance`.
- تفعيل `[Authorize]` وسياسة كل Action في Controllers. إضافة Scopes مركزية في `System.Infrastructure/Extensions/Security/` للـ AcademicPlan/Enrollment/Group/Assignment/Finance.
- ترتيب الاستعلام: `AsNoTracking()` ثم Scope على الكيان الأصلي ثم projection ثم `GridifyAsync`; ممنوع `IgnoreQueryFilters()`.
- فحص `IsAdmin` أولاً. الطالب يقرأ اشتراكاته وفواتيره ومدفوعاته؛ المعلم يرى مجموعاته وطلابها بلا Finance. دور FinanceOfficer غير موجود حالياً ولا يضاف دون قرار.

## 3. خطة منطق Frontend
- استخدام `useAuthStore().hasPermission()` في `src/store/Auth/authStore.js` لإظهار الإجراءات المناسبة.
- تحديث `FinanceLinks.js` وروابط AcademicPlan/Enrollment، مع بقاء التفويض الخادمي الأساس.
- لا تخفي Services أخطاء 403 أو تستنتج صلاحية من route metadata.

## 4. خطة UI/UX ونمط الاختيار
- الطالب يرى صفوفه المالية فقط، والمعلم لا يرى صفحات Finance؛ عرض 403 وحالة التحميل بوضوح.
- Lookups الكبيرة للطالب/الاشتراك/الفاتورة تستخدم Gridify server-side، لا GetList كاملة.

## 5. تطبيق توجيهات المستخدم
- لا DomainEvent. الصلاحية تحدد دخول Endpoint، وScope يحدد الصفوف. لا تعديل لـ `AppDbContext` أو `TransactionBehavior` المشترك.
