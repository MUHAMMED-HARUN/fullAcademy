# خطة خدمة Assignment

## 1. العيوب المكتشفة حالياً
- `clsAssignment` يعتمد على AssignmentNomination دون تحقق مباشر أن StageInfo المختبرة ضمن اشتراك الطالب.
- `AssignmentController` بلا Policies ظاهرة. `AssignmentService.GetBy(Guid id)` لا يضيف Where للـ ID، و`HasAssignment` يحتاج أقواساً وشروط soft-delete دقيقة.
- `NewAssignmentCommandHandler` يحقن `IMediator` غير مستخدم، وتحديث Assignment في Handler الحالي يضيف كياناً جديداً.
- الإنشاء والحذف يطلقان `AssigmentCreatedEvent` و`AssigmentDeletedEvent` لتنفيذ منطق أعمال.

## 2. خطة Backend
- إزالة DomainEvent الخاص بـ Assignment من `clsAssignment`؛ نقل transitions إلى `NewAssignmentCommandHandler` و`DeleteAssignmentCommandHandler` صراحةً.
- التحقق أن الترشيح مرتبط باشتراك فعال وScopeInfo مسموح؛ عمليات إنشاء/حذف متعددة الكيانات تستخدم `ITransaction`، وتحذف/تلغي التوابع حسب قرار موثق.
- إصلاح `AssignmentService.GetBy`, `HasAssignment`, `IAssignmentRepository`؛ إضافة `GetListFilter` مع `ApplyReadScope` قبل DTO وبدون `IgnoreQueryFilters()`.
- تأمين `AssignmentController` بسياسات CRUD؛ Teacher scope عبر group assignments، والطالب scope عبر AcademicEnrollmentItems.
- إزالة Event types/handlers الخاصة بالخدمة بعد نقل المنطق. لا حذف لـ AppDbContext أو TransactionBehavior العامين.

## 3. خطة منطق Frontend
- تحديث `AssignmentRepositories.js`, `AssigmentMapper.js`, `AssignmentService.js`, `AssignmentStore.js`, `AssignmentSchema.js` لعقود الاشتراك/الترشيح الجديدة.
- إزالة `IsExist...` إذا لا يقابله Endpoint، وإكمال update الفعلي أو إخفاء إجراء UI غير المدعوم.
- تحديث `AssignmentView.vue` و`AssignmentNominationView.vue` بحيث يبدأ الاختبار من الترشيح المحدد.

## 4. خطة UI/UX ونمط الاختيار
- لا اختيار طالب/شعبة يدوياً عند بدء الاختبار؛ الترشيح سياق ثابت يحمل StudentAcademicEnrollmentID وScope.
- Edit Mode يستخدم `GetAssignmentForUpsert` لإعادة جميع IDs. تظهر الحالة/النتيجة/الأزرار حسب صلاحيات المستخدم.
- القوائم الكبيرة مرقمة خادمياً، مع loading/error/empty states.

## 5. تطبيق توجيهات المستخدم
- لا DomainEvent منطقي؛ Handler صريح وITransaction عند تغيير عدة كيانات.
- اشتراك StageInfo منفرد لا يصل لاختبارات ScopeInfos خارج تلك المادة.
