# خطة خدمة Group

## 1. العيوب المكتشفة حالياً
- `clsGroup` يحتوي `InstituteClassID`, `GroupName`, `Code` فقط؛ مناسب لتعريف شعبة، لكنه لا يميز الفترة/السعة إن كانت مطلوبة.
- `GroupService.GetListFilter` في `System.Infrastructure/Repositories/Institute/Institute/GroupService.cs` يستخدم `IgnoreQueryFilters()` ولا يطبق Scope.
- `GetGroupsByTeacherIDQuery` و`GroupService.GetGroupByTeacherID` يثقان بـ TeacherID من العميل. `GroupController` بلا `[Authorize]` ظاهر.
- `UpsertGroupComp.vue` يستدعي `fetchInstituteClasses()` ثم يفلتر محلياً لـ GenericSearch. `GroupView.vue` ينشئ StudentEnrollment مباشرة داخل Group.

## 2. خطة Backend
- إبقاء `clsGroup` مجموعة تدريس/حضور لا AcademicPlan ولا FeeStructure؛ لا ربط مباشر للرسوم بالمجموعة.
- إضافة `GroupScopeExtensions` للمدير/المعلم/عضويات الطلاب، وتطبيقه قبل Gridify دون `IgnoreQueryFilters()`.
- استبدال TeacherID العميل في `GetGroupsByTeacherIDQuery` بـ `ICurrentUserService`، أو السماح للمدير فقط بتحديد Teacher مع تحقق صلاحية. تحديث `IGroupRepository`, `GroupService`, Handler.
- ربط إنشاء عضوية Group بـ `StudentAcademicEnrollmentID` والتحقق من أن InstituteClass للشعبة ضمن الخطة. منع حذف Group ذات عضويات نشطة أو TeachingAssignments؛ لا حذف تاريخي متسلسل.
- إضافة سياسات CRUD في `GroupController`. إضافة Capacity/AcademicPeriodID فقط بعد قرار صريح حول الفترة والسعة.

## 3. خطة منطق Frontend
- تحديث `GroupMapper.js`, `GroupService.js`, `GroupStore.js`, `GroupRepositories.js`, `GroupSchema.js`.
- إنشاء InstituteClass Lookup مرقم، وعدم استدعاء `fetchInstituteClasses()` كاملاً داخل `UpsertGroupComp.vue`.
- فصل إضافة Group عن عملية شراء AcademicPlan وعن إلحاق اشتراك قائم بها.

## 4. خطة UI/UX ونمط الاختيار
- تحديث `GroupView.vue`, `GroupList.vue`, `GroupCard.vue`, `UpsertGroupComp.vue`.
- InstituteClass: GenericSearchByFilter server-side مع ID/name/object متزامنة. العضويات: بحث بالاشتراكات المؤهلة فقط.
- Edit Mode يسترجع InstituteClassID والاسم. منع تبديل الصف عند وجود memberships/teaching assignments إلا بعملية نقل معتمدة.

## 5. تطبيق توجيهات المستخدم
- Group تضم عضويات كثيرة، لكنها ليست الخطة أو الفاتورة. اشتراك برنامج قد يمر بعدة Groups عبر الزمن.
- اشتراك StageInfo منفرد لا يُجبر على Group.
