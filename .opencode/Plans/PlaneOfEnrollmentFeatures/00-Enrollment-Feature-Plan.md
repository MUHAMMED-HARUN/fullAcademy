# خطة تحسين StudentEnrollment وTeacherEnrollment

> **تخطيط فقط:** لا تتضمن هذه الوثيقة تنفيذاً أو تعديلاً للكود أو قاعدة البيانات. لا تُطبق migrations دون موافقة منفصلة.

## 1. النطاق والمصطلحات

- لا يوجد كيان أو خدمة باسم `TeacherEnrollment` في الشيفرة الحالية. المقابل الموجود هو **TeachingAssignment** (`clsTeachingAssignment`) الذي يربط المعلم بالمجموعة والمادة؛ تستخدم الوثيقة «TeacherEnrollment» عند الإشارة إلى هذه الخدمة فقط.
- تشمل الخطة خدمتي `TeachingAssignment` و`StudentEnrollment` في Domain وApplication وInfrastructure وWebApi وFrontend، مع مراجعة عقود البحث المستخدمة لاختيارات الطالب والمعلم والمجموعة والمادة.
- تُحافظ واجهة StudentEnrollment على الأعلام الحالية `CanTrack` و`CanNominate` و`CanAddToStage` وعلى تدفقات الحضور والمتابعة والمرحلة، دون توسيع العمل إلى خدمات تلك الميزات.

## 2. ترتيب التنفيذ والاعتماديات

1. **TeachingAssignment (TeacherEnrollment):** يحدد مجموعات المعلمين المسندة إليهم، وهي مصدر النطاق الأمني الذي تعتمد عليه قراءة المعلم لتسجيلات الطلاب.
2. **StudentEnrollment:** يعتمد على الطالب والمجموعة، ويستخدم تكليف المعلم لتقييد تسجيلاته المرئية. يدعم التسجيل الفردي والجماعي.

### بوابة التنفيذ

- عند بدء التنفيذ، تنفذ خدمة واحدة فقط في كل مرحلة، Backend وFrontend معاً.
- يتوقف العمل بعد الخدمة الأولى لمراجعة المستخدم والتحقق من `dotnet build` وESLint قبل الانتقال للخدمة الثانية.
- لا تُنشأ أو تُطبق migrations. أي حاجة إلى migration أو تعديل عقد API تتطلب موافقة منفصلة قبل التنفيذ.

## 3. بطاقة الخدمة الأولى — TeachingAssignment

### 3.1 العيوب المكتشفة

- `Controllers/Enrollment/TeachingAssignmentController.cs` لا يضع `[Authorize]` ولا يربط نقاط النهاية بسياسات قراءة/إنشاء/تعديل/حذف.
- `System.Infrastructure/Repositories/Enrollment/TeachingAssignmentService.cs` يحتوي `IsTeacherEnrolled` الذي يقارن `i.Id` بـ`TeacherID` بدلاً من `i.TeacherID`؛ يوجد بالفعل قيد فريد على `(TeacherID, GroupID, SubjectID)` في `System.Infrastructure/Configurations/Enrollment/TeachingAssignmentConfiguration.cs`.
- عدم اتساق المعرّفات: DTO وأوامر الإنشاء/التحديث تستخدم `EnrollmentID`، بينما `DeleteTeachingAssignmentCommand` ينتظر `TeachingAssignmentID`، و`TeachingAssignmentService.js` يرسل `EnrollmentID` للحذف.
- أسماء DTO/الملفات غير متطابقة: `DTO/TeachingAssignment.cs` يعرّف `TeacherGroupInfo`، و`DTO/TeacherGroupInfo.cs` يعرّف `TeachingAssignmentInfo`.
- الحقل `CalssName` في `TeacherGroupInfo` مكتوب خطأ، ويُستخدم في `TeachingAssignmentMapper.js` و`TeachingGroupList.vue`.
- Validators موجودة تحت `System.Application/Services/Enrollment/TeachingAssignment/Validation/` بدلاً من مجلد `Validations` المعتمد.
- `clsTeachingAssignment.SetDate` في `System.Domain/Entities/Enrollment/clsTeachingAssignment.cs` يعيّن تاريخاً افتراضياً ثم يعيد إسناد القيمة الافتراضية نفسها.
- قائمة الإدارة تعتمد على `GetTeachingAssignmentList`، وقائمة المعلم على `GetTeacherGroups`، لكن `TeachingAssignmentStore.js` لا يعرّف `page` و`pageSize` و`totalCount` التي تستخدمها الـ View.
- فلاتر `TeachingAssignmentView.vue` تعرف حقل `EnrollmentDate` مرتين بالمفتاح نفسه؛ لا يمكن ضبط حدّي التاريخ بصورة مستقلة.
- `UpsertTeachingAssignmentComp.vue` يستخدم بحث نصي أحادي النتيجة للمعلم والمجموعة والمادة، ولا يقدم حالات Lookup مكتملة؛ كما يتضمن `alert` عند اختيار المادة.
- بطاقة `TeachingAssignment` ووظائف التعديل لا تضمن استعادة الكيانات المرتبطة وبطاقاتها قبل إظهار النموذج. قائمة المعلم `TeacherRole/TeachingGroupList.vue` تفتقد حالات التحميل والخطأ والفراغ.

### 3.2 خطة Backend

- مراجعة `System.Domain/Entities/Enrollment/clsTeachingAssignment.cs` و`System.Infrastructure/Configurations/Enrollment/TeachingAssignmentConfiguration.cs`؛ الحفاظ على القيد الفريد الحالي دون migration.
- تحديث `Controllers/Enrollment/TeachingAssignmentController.cs` لتطبيق `[Authorize]` وسياسات:
  - `Can_Read_TeachingAssignment`: `GetTeachingAssignmentList` و`GetTeacherGroups` و`GetTeachingAssignmentBy` و`IsExistTeachingAssignmentBy`.
  - `Can_Create_TeachingAssignment`: `NewTeachingAssignment`.
  - `Can_Update_TeachingAssignment`: `UpdateTeachingAssignment`.
  - `Can_Delete_TeachingAssignment`: `DeleteTeachingAssignment`.
- إضافة `TeachingAssignmentScopeExtensions.cs` تحت `System.Infrastructure/Extensions/Security/`: المدير يرى جميع السجلات؛ المعلم يرى تكليفاته فقط؛ غير ذلك لا يرى صفوفاً. تطبق الدالة على `IQueryable<clsTeachingAssignment>` قبل الإسقاط في القائمة والتفاصيل والوجود.
- تحديث `ITeachingAssignmentRepository` في `System.Application/Abstractions/Enrollment/TeachingAssignmentRepository.cs` و`TeachingAssignmentService.cs`:
  - إصلاح شرط التكرار والتحقق من GUIDs والكيانات المرتبطة ونوع `clsSession` المستخدم كمادة.
  - إضافة حد أقصى للصفحة وGridify Mapper بقائمة حقول مسموحة لـ`TeacherName` و`GroupName` و`SubjectName` و`EnrollmentDate`.
  - الحفاظ على `GetTeacherGroups` للمعلم، مع اشتقاق معرّف المعلم من `ICurrentUserService` بدلاً من قبول هوية من العميل.
  - جعل `GetBy` و`GetEntityBy` مقيدين بالنطاق، وإرجاع IDs والبيانات المطلوبة لوضع التعديل.
- تحديث أوامر وQueries الخدمة في `System.Application/Services/Enrollment/TeachingAssignment/Command/` و`Query/`، ومنها `GetAllEnrollmentQuery.cs` و`GetTeacherGroupsQuery.cs`، وإضافة/تصحيح Validators تحت `Validations/`.
- توحيد أسماء `TeachingAssignmentInfo` و`TeacherGroupInfo` و`TeachingAssignmentID` داخلياً. إصلاح `CalssName` إلى `ClassName` مع مراجعة توافق JSON مع `TeachingAssignmentMapper.js` و`TeachingGroupList.vue`.
- إضافة الصلاحيات القياسية في `System.Domain/Entities/Permissions/clsPermission.cs`، ومنح المعلم صلاحية القراءة افتراضياً. المدير يستفيد من تجاوز صلاحيات `IsAdmin`.
- **قرار مطلوب قبل التنفيذ:** عقد الحذف الحالي متعارض بين `TeachingAssignmentID` و`EnrollmentID`. يوصى بتوحيده مع دعم توافق مؤقت أو بتحديث الطرفين بعد موافقة صريحة؛ لا يُغيّر العقد دون مراجعة.

### 3.3 خطة منطق Frontend

- مواءمة الملفات:
  - `src/repositories/Enrollment/TeachingAssignmentRepositories.js`
  - `src/Mapper/Enrollment/TeachingAssignmentMapper.js`
  - `src/validations/Enrollment/TeachingAssignmentSchema.js`
  - `src/Service/Enrollment/TeachingAssignmentService.js`
  - `src/store/Enrollment/TeachingAssignmentStore.js`
- تثبيت شكل Paging وفلتر/ترتيب محفوظين في Pinia، وتحويل Gridify fields وIDs/labels صراحةً في Mapper، واستخدام `try/finally` لحالات التحميل.
- فصل مصدر بيانات المدير (`GetTeachingAssignmentList`) عن قائمة المعلم (`GetTeacherGroups`) دون تمرير الدور كآلية أمان من المتصفح.
- إصلاح الحذف Query/Body وعقد `TeachingAssignmentID` بالتنسيق المعتمد، وتحديث القائمة بعد نجاح CRUD.
- في وضع التعديل، تحميل السجل المرتبط وتكوين قيم Teacher/Group/Subject المختارة قبل إظهار النموذج.

### 3.4 خطة UI/UX ونمط الاختيار

- تحديث `src/views/Enrollment/TeachingAssignmentView.vue` و:
  - `src/components/Enrollment/TeachingAssignment/TeachingAssignmentList.vue`
  - `src/components/Enrollment/TeachingAssignment/TeachingGroupList.vue`
  - `src/components/Enrollment/TeachingAssignment/UpsertTeachingAssignmentComp.vue`
  - `src/components/Enrollment/TeachingAssignment/TeachingAssignmentCard.vue`
- ضبط إظهار أفعال الإدارة وفق الصلاحيات. تظل قائمة المعلم للقراءة وعرض مجموعاته المسندة فقط.
- الحقول الكبيرة تستخدم `GenericSearchByFilter` خادمياً ومرقّماً، ولا تستخدم `GetList()` كاملاً أو فلترة محلية:
  - **Teacher:** `GetTeacherList`، مع Allowlist مثل `FirstName` و`LastName` و`NationalNumber`.
  - **Group:** `GetGroupList`، مع Allowlist مثل `GroupName` و`Code`.
  - **Subject:** `GetSessionListFilter`، مقيّد إلى نوع `Subject` وبحث مسموح على `SessionName`.
  - **Date:** حقل تاريخ قياسي.
- كل اختيار يزامن ID والاسم والعنصر المختار والبطاقة. البحث الجديد أو المسح أو الخطأ/النتيجة الفارغة يمسح الاختيار السابق والبطاقة.
- Edit Mode يعيد تهيئة Teacher وGroup وSubject وIDs قبل تمكين الحفظ. تعرض القائمة والبطاقات والنموذج RTL حالات تحميل/خطأ/فراغ واضحة.

### 3.5 توجيهات التنفيذ الخاصة

- الخدمة الأولى فقط في التنفيذ؛ بعدها توقف للمراجعة ونجاح البناء وESLint.
- لا migration للقيد الفريد لأنه موجود في Configuration حالياً.
- لا تغيّر عقد `EnrollmentID`/`TeachingAssignmentID` أو `ClassName`/`CalssName` قبل مراجعة توافقهما.

## 4. بطاقة الخدمة الثانية — StudentEnrollment

### 4.1 العيوب المكتشفة

- `Controllers/Enrollment/StudentEnrollmentController.cs` لا يفرض المصادقة أو سياسات الصلاحيات على التسجيل الفردي والجماعي والتعديل والحذف والقراءة.
- `GetAllStudentEnrollmentQueryHandler` في `System.Application/Services/Enrollment/StudentEnrollment/Query/GetAllStudentEnrollmentQueryHandler.cs` يقرر الوصول يدوياً من `Role`، ويسمح حالياً بالمدير والمعلم فقط؛ لا يوجد نطاق قراءة للطالب لسجلاته الشخصية.
- `System.Infrastructure/Repositories/Enrollment/StudentEnrollmentService.cs` يستعلم من `vw_StudentEnrollmentInfo` دون نطاق أمني مركزي. كما أن `GetBy` و`GetEntityBy` و`GetList` و`IsExistBy` و`IsStudentEnrolled` لا تطبق نطاق ملكية مركزي.
- `GetGroupStudentss` يقبل `TeacherID` من طبقة Application ويطبّق التصفية داخلياً، بدلاً من تطبيق Scope مركزي على كيان `clsStudentEnrollment` قبل الإسقاط.
- `EnrollByRangeCommandHandler` لا يملك Validator خاصاً، ولا يتحقق من كل GUIDs أو النطاق أو تاريخ التسجيل؛ يتجاوز التكرارات وقد يعيد نجاحاً جزئياً دون تفصيل النتائج.
- Validators في `System.Application/Services/Enrollment/StudentEnrollment/Validation/` تستخدم مجلد `Validation`، وتحتاج قواعد GUID واضحة ومطابقة لـZod.
- `StudentEnrollmentView.vue` يعرّف حدّي التاريخ بالمفتاح `EnrollmentDate` نفسه، مع أن DTO `StudentEnrollmentInfoView` يعرض `Date`. كما توجد مسارات فلترة قديمة موازية وبارامترات `filter/page` مختلفة الحالة.
- `UpsertStudentEnrollmentComp.vue` يبحث عن الطالب بعملية GetBy مفردة، بينما يتيح عدة طلاب في قائمة محلية؛ بحث المجموعة يخلط بين نتائج القائمة وجلب GetBy ولا يمرر بوضوح loading/error إلى `SearchFilterCombo`.
- إجراءات التسجيل والتحديث والحذف في `StudentEnrollmentView.vue` ليست كلها مقيدة بصلاحيات. تحديد الصفوف قد يبقى قديماً عند تغيير الصفحة أو الفلتر.
- `vw_StudentEnrollmentInfo` في `System.Infrastructure/Persistence/Views/Class1.cs` يوفّر أعلام `CanTrack` و`CanNominate` و`CanAddToStage` اللازمة للواجهة؛ يجب الحفاظ عليها عند إصلاح نطاق القراءة.
- لا يوجد قيد فريد ظاهر على `(StudentID, GroupID)` في `StudentEnrollmentConfiguration.cs`؛ فحص التكرار الحالي على مستوى التطبيق لا يمنع سباق الطلبات.

### 4.2 خطة Backend

- تحديث `Controllers/Enrollment/StudentEnrollmentController.cs` بسياسة `[Authorize]`، وربط:
  - القراءة (`GetStudentEnrollmentList`, `GetStudentEnrollmentBy`, `IsExistStudentEnrollmentBy`) بـ`Can_Read_StudentEnrollment`.
  - الإنشاء الفردي و`EnrollByRange` بـ`Can_Create_StudentEnrollment`.
  - التعديل بـ`Can_Update_StudentEnrollment`.
  - الحذف بـ`Can_Delete_StudentEnrollment`.
- إنشاء `StudentEnrollmentScopeExtensions.cs` تحت `System.Infrastructure/Extensions/Security/`:
  - المدير: كامل الاستعلام.
  - المعلم: تسجيلات المجموعات التي لديه تكليف فعال بها في TeachingAssignment.
  - الطالب: تسجيلات `StudentID` المطابقة له فقط.
  - مستخدم غير مصادق أو بلا نطاق معروف: `Where(false)`.
- تحديث `IStudentEnrollmentRepository` في `System.Application/Abstractions/Enrollment/StudentEnrollmentRepository.cs` و`StudentEnrollmentService.cs`:
  - إزالة اعتماد نطاق الوصول على `TeacherID` الوارد من Handler.
  - تطبيق Scope على `_dbSet.AsNoTracking()` قبل الإسقاط في القائمة والقراءة والوجود، وتطبيقه على الكيان المتعقب في مسارات التعديل/الحذف.
  - للحفاظ على أعلام `CanTrack` و`CanNominate` و`CanAddToStage` في `vw_StudentEnrollmentInfo`، تُقيّد نتيجة العرض بمعرّفات مستخرجة من استعلام كيان التسجيل المقيّد أمنياً؛ لا يُعدّل تعريف قاعدة البيانات ضمن هذه الخطة.
  - قصر `GetBy` على استخدام ID واضح في وضع التعديل؛ لا تعتمد الواجهة على `StudentID` أو `GroupID` لإرجاع سجل اعتباطي.
- تحديث Queries وHandlers في `System.Application/Services/Enrollment/StudentEnrollment/Query/` لإزالة فحوص الدور اليدوية ونقل نطاق الصفوف إلى Infrastructure.
- إضافة Validator لـ`EnrollByRangeCommand`، وتقوية `NewStudentEnrollmentCommandValidation` و`UpdateStudentEnrollmentCommandValidation` للتحقق من GUIDs والتاريخ وقائمة IDs غير الفارغة.
- التأكد من وجود الطالب والمجموعة وصحة الوصول عند الإنشاء/التعديل؛ والتحقق من التكرار قبل الإضافة الجماعية مع نتيجة مفهومة لكل عنصر أو نتيجة واضحة للعملية.
- نقل مجلد التحقق إلى `System.Application/Services/Enrollment/StudentEnrollment/Validations/` وتوحيد namespaces.
- إضافة صلاحيات `StudentEnrollment` القياسية في `clsPermission.cs`: المدير CRUD؛ المعلم والطالب قراءة فقط ضمن النطاق. لا يُمنح المعلم أو الطالب إنشاء/تعديل التسجيل افتراضياً.
- **قرار قاعدة بيانات منفصل:** يمكن دراسة قيد فريد لـ`StudentID + GroupID` لمنع التكرار المتزامن، لكن إضافته تحتاج migration وموافقة منفصلة.

### 4.3 خطة منطق Frontend

- مواءمة:
  - `src/repositories/Enrollment/StudentEnrollmentRepositories.js`
  - `src/Mapper/Enrollment/StudentEnrollmentMapper.js`
  - `src/validations/Enrollment/StudentEnrollmentSchema.js`
  - `src/Service/Enrollment/StudentEnrollmentService.js`
  - `src/store/Enrollment/StudentEnrollmentStore.js`
- توحيد Query keys إلى PascalCase في الطلب، مع نموذج واجهة camelCase وMapper واضح. اعتماد `Date` في الحقول التي تطابق DTO، وPaging `Data/Count` أو `data/count` وفق TResult الفعلي.
- حفظ `Filter` و`OrderBy` و`Page/PageSize` في Options Store، وإعادة تحميل الصفحة الحالية بعد النجاح، ومسح الخطأ القديم، وضمان إنهاء Loading باستخدام `try/finally`.
- توحيد تدفق الإنشاء الفردي والجماعي مع `EnrollByRange`; لا تُرسل طلبات متعددة من View ولا تُخفى نتائج العناصر المتجاوزة/الفاشلة.
- الحفاظ على استجابة الـ DTO التي تحتوي `CanTrack` و`CanNominate` و`CanAddToStage`، وعدم استنتاجها أو حسابها محلياً.

### 4.4 خطة UI/UX ونمط الاختيار

- تحديث `src/views/Enrollment/StudentEnrollmentView.vue` و:
  - `src/components/Enrollment/StudentEnrollment/StudentEnrollmentList.vue`
  - `src/components/Enrollment/StudentEnrollment/UpsertStudentEnrollmentComp.vue`
  - `src/components/Enrollment/StudentEnrollment/StudentEnrollmentCard.vue`
- **Student:** `GenericSearchByFilter` عبر `GetStudentList` المرقّم، وتُحوّل النتيجة إلى عرض اسم واضح، مع دعم الإضافة إلى قائمة الطلاب المختارين وإزالة كل طالب صراحةً. لا يستخدم `GetList()` كاملاً ولا `Array.filter` لتغذية البحث.
- **Group:** `GenericSearchByFilter` عبر `GetGroupList` المرقّم. تاريخ التسجيل حقل تاريخ قياسي.
- مزامنة `studentID/groupID` والاسم والعنصر المختار والبطاقة. بدء بحث جديد أو مسح الاختيار أو حدوث خطأ/نتيجة فارغة يمسح ID والاسم والبطاقة معاً.
- Edit Mode يعيد تحميل التسجيل بواسطة `EnrollmentID` ثم يهيئ الطالب والمجموعة والبطاقات قبل إظهار النموذج. يقيّد تعديل تسجيل واحد على طالب واحد؛ التسجيل الجماعي مسار إنشاء فقط.
- معالجة التاريخ: استخدام مفتاحين مستقلين `DateFrom` و`DateTo` مربوطين بخاصية `Date` المسموحة، أو الاكتفاء بحد تاريخ واحد؛ لا يُكرر المفتاح نفسه في `GenericFilterComp`.
- إظهار التسجيل/التعديل/الحذف وفق الصلاحيات؛ الاحتفاظ بإجراءات attendance وtrack وstage الحالية مع بوابات الصلاحيات الخاصة بكل ميزة. تنظيف التحديد عند تغيير الصفحة أو الفلتر.
- الحفاظ على RTL وهوية `Main.css`، وإضافة حالات Empty/Loading/Error ورسائل التحقق قرب الحقول.

### 4.5 توجيهات التنفيذ الخاصة

- StudentEnrollment هي الخدمة الثانية، ولا تبدأ قبل مراجعة TeachingAssignment واكتمال بنائه واختبار ESLint.
- الحفاظ على أعلام الرؤية القادمة من `vw_StudentEnrollmentInfo` دون تعديل view أو قاعدة البيانات.
- لا migration للقيد الفريد المحتمل دون موافقة مستقلة.

## 5. ملف الخطة

- تحتوي هذه الوثيقة ترتيب الاعتماديات وبطاقتي الخدمتين وبوابات التنفيذ المرحلي.
- لم تُنشأ ملفات إضافية للخدمتين؛ يمكن فصل البطاقات إلى ملفات مستقلة إذا طُلب ذلك.
