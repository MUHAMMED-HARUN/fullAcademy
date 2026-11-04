# خطة تحسين Progress Feature

> وثيقة تخطيط فقط. لا تتضمن تنفيذ كود أو تغييرات قاعدة بيانات.

## الهدف والنطاق

- تحسين خدمات Progress الموجودة: `Stage` و`Session` و`StageInfo` و`StudentStage` و`DailyEvaluation`.
- اختيار نمط Select مناسب لحجم وطبيعة البيانات، مع منع تحميل القوائم الكبيرة كاملة ثم تصفيتها في المتصفح.
- توفير إجراءات واضحة من جداول الآباء لإضافة السجلات التابعة.
- تضمين التحقق من الصلاحيات ونطاق ملكية البيانات في كل خدمة.
- لا تُقترح migrations في هذه الخطة؛ العلاقات المطلوبة ممثلة في الكيانات الحالية. أي حاجة إلى تغيير قاعدة البيانات تتطلب مراجعة وموافقة منفصلتين.

## ترتيب التنفيذ (Dependency Order)

1. **Session** — مستقلة داخل Progress، وتوفر بيانات الأب/المحتوى التي يعتمد عليها `StageInfo`.
2. **Stage** — مستقلة داخل Progress، وتعتمد على `Class` الموجودة، وتحتاجها خدمتا `StageInfo` و`StudentStage`.
3. **StageInfo** — تعتمد على `Stage` و`Session`، وتوفر خيارات المحتوى للتقييم اليومي.
4. **StudentStage** — تعتمد على `Stage` و`StudentEnrollment`.
5. **DailyEvaluation** — تعتمد على التسجيل، و`StageInfo`، والتحقق من آخر متابعة ومدى التقدم.

## توصية أنماط الاختيار

- `SessionType` و`UniteType` و`ProgressType` و`LevelType` وحالة `StudentStage`: **Standard Select**؛ كلها قيم Enum محدودة.
- `Class` و`Stage` و`Session` في البحث العام أو القوائم كبيرة الحجم: **GenericSearchByFilter** عبر Gridify خادمي وAllowlist لحقول البحث. لا يستخدم `GetList()` الكامل ولا تصفية محلية.
- اختيار الأب من جدول الأب: لا يُطلب من المستخدم اختياره مجدداً؛ يمرر معرّف الأب إلى نموذج الابن ويعرض اسمه للقراءة فقط.
- `StageInfo` داخل نموذج التقييم: **Standard Select** فقط للقائمة المؤهلة والمحمّلة خادمياً حسب التسجيل المحدد.
- `Stage` داخل إضافة `StudentStage`: **Standard Select** فقط إذا كانت النتائج قليلة ومحددة خادمياً حسب سياق الصف/المجموعة.
- بحث `GenericSearchByFilter` يزامن ID والاسم والعنصر المختار، ويمسحها عند المسح أو بدء بحث جديد أو الفشل أو عدم وجود نتائج. يستخدم Gridify بصيغة Contains مثل `Field=*Value` دون `*` في نهاية القيمة.

## إجراءات الابن المطلوبة في جداول الآباء

1. **Session → Session فرعي:** إجراء في صف الجلسة/الموضوع المؤهل. يفتح نموذج الإضافة مع `ParentSessionID` مثبتاً، ويعرض نوع الأب والابن المناسبين.
2. **Stage → StageInfo:** إجراء في صف المرحلة يفتح نموذج StageInfo مع `StageID` مثبتاً؛ ويمكن عرض/تحميل تفاصيل المرحلة التابعة بحسب هذا السياق.
3. **StudentEnrollment → StudentStage وDailyEvaluation:** عمود Actions في صف التسجيل يتيح إضافة مرحلة للطالب أو متابعة يومية، مع تمرير `EnrollmentStudentID` وبيانات الطالب/المجموعة للنموذج.
4. إعادة استخدام النماذج والـ Stores الموجودة عند الإمكان، وتوحيد التنفيذ بين أزرار الصف وإجراءات التحديد الجماعي الموجودة. إظهار الأزرار في الواجهة لا يغني عن صلاحيات Backend.

## بطاقات الخدمات

### الخدمة 1 — Session

1. **العيوب المكتشفة**
   - `Controllers/Progress/SessionController.cs` يستخدم namespace تابعاً لـ `Controllers.Institute` بدلاً من `Controllers.Progress`.
   - عقد المستودع وDTO موجودان تحت namespaces خاصة بـ Institute؛ منها `System.Application.Abstractions.Institute` و`System.Application.Services.Institute.Program.DTO`.
   - `UpsertSessionComp.vue` يختار الأب من قائمة محملة بالكامل ومفلترة محلياً في `SessionView.vue`.
   - route التحقق مختلف: Controller يعرّف `IsExistSessioneBy` بينما Repository يستدعي `IsExistSessionBy`.
   - حذف Session في `SessionService.js` يرسل `EnrollmentID`، كما أن زر الإضافة في `SessionView.vue` يحمل نصاً غير متعلق بالميزة.
2. **خطة Backend**
   - مراجعة `SessionController.cs` وواجهة `ISessionRepository` في `System.Application/Abstractions/Progress/` و`SessionService.cs` في Infrastructure.
   - وضع `SessionInfo` تحت `System.Application/Services/Progress/Session/DTO/` ومواءمة namespace والاعتمادات.
   - دعم قائمة/بحث خادمي مفلتر ومرقم للـ Sessions المؤهلة كآباء، مع Gridify وAllowlist.
   - التحقق من توافق نوع الأب والابن و`ParentSessionID` عند الإنشاء والتعديل.
   - إضافة صلاحيات Session إلى `clsPermission` وربط Policies بنقاط Controller.
3. **خطة Frontend Logic**
   - مراجعة `repositories/Progress/SessionRepositories.js` و`Mapper/Progress/SessionMapper.js` و`validations/Progress/SessionSchema.js`.
   - تعديل `Service/Progress/SessionService.js` و`store/Progress/SessionStore.js` لتدفق البحث الخادمي، والتحقق، والتحميل والأخطاء.
   - تصحيح عقد الحذف وتوحيد النوع الرقمي للـ Enum وحقول `ParentSessionID`.
4. **خطة UI/UX ونمط الاختيار**
   - تحديث `components/Progress/Session/SessionList.vue` لإضافة إجراء الابن في صف الأب المؤهل.
   - تحديث `UpsertSessionComp.vue` و`views/Progress/SessionView.vue`: في سياق الإضافة من الأب يكون الأب مثبتاً للقراءة فقط؛ في الإضافة العامة يكون البحث الخادمي بـ GenericSearchByFilter.
   - `SessionType` Standard Select؛ تهيئة بيانات الأب والـ labels عند Edit Mode قبل إظهار النموذج.
5. **توجيه المستخدم الخاص**
   - زر «إضافة ابن» في جدول Session هو المسار الأساسي لإنشاء Session تابعة، ويظهر وفق `Can_Create_Session`.

### الخدمة 2 — Stage

1. **العيوب المكتشفة**
   - `clsStage.cs` موجود في مجلد Progress لكن namespace عام `System.Domain.Entities`.
   - البحث عن Class في `UpsertStageComp.vue` يعتمد على إدخال نوع بحث ونص، وليس مكون بحث واختيار متزامن.
   - نموذج المرحلة يعرض مراحل في حقل اسمه «الصف السابق»، بينما `PrevClassID` و`PrevStage` وDTO `PrevClassName` لا تتسق تسمياتها بوضوح.
   - `StageList.vue` لا يعرّف عنواناً واضحاً لعمود الإجراءات ولا يوفّر إجراء إضافة `StageInfo`.
   - `StageSchema.js` يجعل الملاحظة إلزامية رغم أن Notes اختيارية في الكيان، وتسمية/مجلدات Validation غير موحدة.
2. **خطة Backend**
   - مراجعة `StageController.cs` وملف عقد `IStageRepository` الحالي (اسمه يتضمن مسافة زائدة) و`StageService.cs` و`DTO/StageInfo.cs`.
   - إضافة بحث Stage مفلتر ومرقم يدعم قيد Class عند استخدامه كـ Lookup.
   - توحيد معنى وهوية المرحلة السابقة في DTO والعقد، والتحقق من وجود الصف والمرحلة السابقة.
   - مراجعة namespaces بما يطابق مسارات Progress دون توسيع التغيير خارج الملفات والمراجع التابعة لهذه الخدمة.
   - إضافة صلاحيات Stage وربطها بنطاق الصفوف والمجموعات المسموح بها.
3. **خطة Frontend Logic**
   - مواءمة `StageRepositories.js` و`StageMapper.js` و`StageSchema.js` و`StageService.js` و`StageStore.js` مع عقد البحث والمرحلة السابقة.
   - إزالة الاعتماد على تحميل كل المراحل في Lookup كبير، والتحقق من أن حقول Zod تطابق قواعد Backend.
4. **خطة UI/UX ونمط الاختيار**
   - تحديث `StageList.vue` لإظهار Actions واضحة، ومنها «إضافة تفاصيل مرحلة» مع تمرير StageID.
   - Class: GenericSearchByFilter مع Gridify عند كبر القائمة. المرحلة السابقة: Select محدود ومفلتر خادمياً إن كانت نتائجه قليلة؛ وإلا GenericSearchByFilter.
   - في Edit Mode تهيئة Class والمرحلة السابقة والـ labels والبطاقات قبل تمكين الحفظ.
5. **توجيه المستخدم الخاص**
   - إضافة StageInfo من جدول Stage تملأ `StageID` تلقائياً، دون مطالبة المستخدم بإعادة اختيار الأب، وتخضع لصلاحية `Can_Create_StageInfo`.

### الخدمة 3 — StageInfo

1. **العيوب المكتشفة**
   - `StageInfoController.cs` في مسار Progress لكن namespace تابع لـ Institute، وواجهة المستودع كذلك في namespace Institute.
   - اختيار Stage وMatter في `UpsertStageInfoComp.vue` بحث نصي أحادي النتيجة؛ ولا يضمن مسار البحث/الفشل إزالة ID والبطاقة القديمة معاً.
   - `StageInfoView.vue` يحمل القائمة العامة، ولا يقيّد نتائجها بالمرحلة عند الوصول من جدول الأب.
   - اختلاف route التحقق بين Controller وRepository: `IsExistStageInfoeBy` مقابل `IsExistStageInfoBy`.
   - عقد `GetStudentStageInfo` وDTO يحتاج مراجعة لتوحيد الحقول والـ Enums، وZod يجعل معرفات Stage وMatter اختيارية.
2. **خطة Backend**
   - مراجعة `StageInfoController.cs` و`StageInfoRepository.cs` و`StageInfoService.cs` و`StageInfoInfo.cs`.
   - إضافة فلترة حسب `StageID` للتابعين وبحث مفلتر للمادة، مع Gridify عند الحاجة.
   - مراجعة `GetStudentStageInfoQuery` وHandler للتأكد أن النتائج مناسبة للتسجيل المحدد، وإرجاع IDs والحقول التي تحتاجها الواجهة.
   - التحقق من وجود Stage وSession وصحة `From/To` وتوافق نوع المادة.
   - إضافة صلاحيات StageInfo ونطاق القراءة المناسب قبل إسقاط DTO.
3. **خطة Frontend Logic**
   - تحديث `StageInfoRepositories.js` و`StageInfoMapper.js` و`StageInfoSchema.js` و`StageInfoService.js` و`StageInfoStore.js`.
   - مطابقة أسماء routes وحقول DTO، والتحقق من معرفات العلاقات وقيم Enums وحدود From/To.
   - إصلاح تهيئة Edit Mode ليجلب Stage وMatter المحددين قبل عرض النموذج، وعدم الاعتماد على Store قديم.
4. **خطة UI/UX ونمط الاختيار**
   - إضافة StageInfo من جدول Stage مع Stage مثبت؛ وعند الإضافة العامة تكون Stage وMatter عبر GenericSearchByFilter خادمي وAllowlist.
   - `UniteType` و`ProgressType` Standard Select.
   - مسح ID والاسم والبطاقة عند مسح البحث أو فشله، مع حالات loading/error/no results ودعم لوحة المفاتيح وARIA.
5. **توجيه المستخدم الخاص**
   - الجدول الأب Stage هو نقطة بدء إضافة الابن؛ شاشة StageInfo العامة تبقى متاحة، لكن لا تتجاوز سياق الأب أو الصلاحيات.

### الخدمة 4 — StudentStage

1. **العيوب المكتشفة**
   - `StudentStageList.vue` يستخدم `assignmentScopeInfoID` كمفتاح بدلاً من `studentStageID`، ولا يعرض تحديثاً.
   - شرط تهيئة EnrollmentID في `UpsertStudentStageComp.vue` يستخدم OR، فيتحقق دائماً تقريباً.
   - نموذج StudentStage موجود داخل `StudentEnrollmentView.vue`، بينما `StudentStageView.vue` يملك سياق تسجيل غير مضمون ويستورد مخازن غير لازمة بحسب الاستعمال الحالي.
   - `StudentStageService.cs` يعرض القائمة دون نطاق مركزي، كما لا يقيّد القراءة بالتسجيل.
   - مجلد وnamespace التحقق الحاليان لا يتبعان معيار `Validations` بشكل متسق، والتحقق من العلاقة/النطاق يحتاج مراجعة.
2. **خطة Backend**
   - مراجعة `StudentStageController.cs` و`StudentStageRepository.cs` و`StudentStageService.cs` وDTO `StudentStageInfo`.
   - توفير استعلامات قائمة مفلترة بالتسجيل و/أو المرحلة وتطبيق النطاق الأمني على الكيان الأصلي قبل الإسقاط.
   - مراجعة `NewStudentStageCommand` وHandler وUpdate Handler والتحقق من تسجيل الطالب، صلاحية Stage، منع التكرار، الحالة والتاريخ.
   - إصلاح route التحقق `IsExistStudentStageeBy` مقابل Repository.
   - تطبيق Admin CRUD، وTeacher ضمن المجموعات المسندة، وقراءة الطالب لسجله فقط، عبر Policies وscope.
3. **خطة Frontend Logic**
   - تحديث `StudentStageRepositories.js` و`StudentStageMapper.js` و`StudentStageSchema.js` و`StudentStageService.js` و`StudentStageStore.js`.
   - إضافة جلب تابع حسب Enrollment، وتحديث النتائج بعد الإنشاء/التعديل/الحذف، وتوحيد حقول الحالة والتاريخ مع العقد.
   - مراجعة `StudentStageView.vue` وتقليل الاعتماد على أي Store غير متعلق بالخدمة.
4. **خطة UI/UX ونمط الاختيار**
   - إضافة إجراء «إضافة مرحلة» في صف Enrollment يمرر EnrollmentID واسم الطالب كقيم ثابتة.
   - Stage: Select محدود فقط بعد جلب المراحل المناسبة للسياق خادمياً؛ لا تحميل قائمة عامة ولا تصفية محلية.
   - تصحيح مفتاح `StudentStageList.vue`، ودعم empty/loading/error وتأكيد الحذف، وإظهار التعديل وفق الصلاحية.
   - في Edit Mode استعادة التسجيل والمرحلة المختارين قبل السماح بالحفظ.
5. **توجيه المستخدم الخاص**
   - جدول Enrollment هو الأب لهذه الخدمة. يعاد استخدام نافذة الإضافة الموجودة مع تمرير سياق الصف مباشرة بدلاً من اختيار Enrollment يدوياً.

### الخدمة 5 — DailyEvaluation

1. **العيوب المكتشفة**
   - `DailyEvaluationController.cs` يستخدم namespace Institute، وroute التحقق مختلف عن Repository: `IsExistDailyEvaluationeBy` مقابل `IsExistDailyEvaluationBy`.
   - `DailyEvaluationService.GetAllFilter` ينفذ Join مع TeachingAssignments قد يكرر الصف، ولا يطبق نطاقاً مركزياً على الكيان قبل الإسقاط.
   - `GetAllDailyEvaluationQueryHandler` يحتاج مراجعة فحص UserId nullable والاعتماد على الدور، لتوحيد authorization وscope.
   - `DailyEvaluationView.vue` يرفض إنشاء سجل جديد في `OnSubmitUpsert`، مع أن سياق Enrollment يتضمن مسار إنشاء منفصلاً.
   - `StudentEnrollmentView.vue` يستدعي آخر تقييم دون StageInfo في موضع، كما تتباين أسماء الحالة `selectedDailyEvaluation` و`selectedEvaluation`.
   - `DailyEvaluationSchema.js` يجعل معرفات Enrollment وStageInfo اختيارية رغم أنها مطلوبة، وأنواع بعض props في Upsert غير مناسبة للـ IDs والـ labels.
2. **خطة Backend**
   - مراجعة `DailyEvaluationController.cs` و`DailyEvaluationRepository.cs` و`DailyEvaluationService.cs` و`DailyEvaluationInfo.cs`.
   - الإبقاء على Gridify للقائمة مع Allowlist، وتطبيق `ApplyReadScope` على `clsDailyEvaluation` قبل الإسقاط، وتفادي تكرار الصفوف.
   - مراجعة `GetAllDailyEvaluationQueryHandler` وQuery آخر تقييم لإسناد النطاق للمستودع: Admin كامل، Teacher ضمن تكليفاته ومجموعاته، Student سجلاته الشخصية فقط.
   - تقوية `NewDailyEvaluationCommandHandler` للتحقق من تسجيل الطالب وارتباطه بـ StageInfo والنطاق المسموح، وصحة From/To والتسلسل.
   - إضافة صلاحيات DailyEvaluation وربطها بالـ Controller، ومعالجة route التحقق المختلف.
3. **خطة Frontend Logic**
   - تحديث `DailyEvaluationRepositories.js` و`DailyEvaluationMapper.js` و`DailyEvaluationSchema.js` و`DailyEvaluationService.js` و`DailyEvaluationStore.js`.
   - توحيد `selectedEvaluation` وبارامترات استرجاع آخر سجل، ومطابقة Paging/DTO وحقول Enrollment وStageInfo.
   - تمكين إنشاء السجل في `DailyEvaluationView.vue`، وتحديث القائمة/الحالة بعد نجاح الإنشاء أو التعديل أو الحذف.
4. **خطة UI/UX ونمط الاختيار**
   - إضافة إجراء «إضافة متابعة يومية» في صف Enrollment، مع الطالب والمجموعة وEnrollmentID ثابتة في النموذج.
   - StageInfo: Standard Select من قائمة محدودة مؤهلة لهذا Enrollment وتُجلب خادمياً؛ LevelType Standard Select.
   - إظهار empty/loading/error، وتعطيل الحفظ عند عدم وجود خيارات مؤهلة أو اكتمال المدى، ومنع بقاء بيانات سجل سابق عند تبديل Enrollment.
5. **توجيه المستخدم الخاص**
   - `StudentEnrollmentList.vue` هو الجدول الأب المطلوب لإجراءات إضافة Stage وDailyEvaluation. تندمج الإجراءات مع التدفق القائم في `StudentEnrollmentView.vue`، ولا تعتمد على تحديد الصف وحده.

## المجلدات المشتركة ذات الصلة

- الصلاحيات: `System.Domain/Entities/Permissions/clsPermission.cs`، وسياسات Controllers، و`School_Academy_Front/sys_Front/template-proj/src/store/Auth/authStore.js`.
- نطاقات الأمان: `System.Infrastructure/Extensions/Security/`؛ تخطيط نطاق مركزي لكيانات Progress، دون شروط ملكية مكررة في Handlers.
- تسجيل المستودعات: `System.Infrastructure/InfrastructureServiceRegistration.cs`، بعد التحقق مما هو مسجل حالياً.
- الروابط: `School_Academy_Front/sys_Front/template-proj/src/router/Links/Progress.js`.
- إجراءات Enrollment: `School_Academy_Front/sys_Front/template-proj/src/components/Enrollment/StudentEnrollment/StudentEnrollmentList.vue` و`src/views/Enrollment/StudentEnrollmentView.vue`.
- لا حاجة مخططة إلى تعديل إعداد مشترك أو قاعدة بيانات؛ أي تغيير من هذا النوع يحتاج مراجعة وموافقة مستقلة.

## بوابة التنفيذ المرحلي

- عند الموافقة، ينفذ **Session فقط** في المرحلة الأولى.
- بعده يتوقف العمل لمراجعة المستخدم ونجاح `dotnet build` و`ESLint` قبل الانتقال إلى Stage.
- يستمر الترتيب خدمة واحدة في كل مرحلة، ولا تُنفذ بقية الخدمات دفعة واحدة.
- هذه الوثيقة تخطيط فقط، ولا تمثل تفويضاً لبدء التنفيذ.
