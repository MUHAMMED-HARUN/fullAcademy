# خطة تحليل وتحسين Assignment Features

> الحالة: **خطة فقط (Read-Only)** — لم يُعدَّل أي ملف. التنفيذ يبدأ بالخدمة رقم (1) فقط بعد موافقة المستخدم.

## القيد الحاكم للمستخدم

- **لا يُنفَّذ أي منطق عبر Domain Events** في وحدة Assignment: لا `AddDomainEvent` داخل الكيانات، ولا `INotificationHandler` يغيّر حالة الكيانات الأخرى.
- **فصل المسؤوليات:**
  - **Domain:** الحالة والقواعد (Invariants) وانتقالات الحالة كدوال صريحة على الكيان، وسياسة الحساب كدالة نقية بلا وصول للمستودعات.
  - **Application:** Command/Query Handlers تنسّق صراحةً: تحميل → استدعاء دوال الـ Domain → حفظ عبر `IUnitOfWork` داخل `ITransaction`.
  - **Infrastructure:** مستودعات واستعلامات فقط، بلا أي قواعد أعمال أو تغيير حالة خفي.
  - **WebApi:** توجيه وتفويض (`[Authorize(Policy=...)]`) وتحويل النتيجة فقط.
  - **Frontend:** `View → Pinia Store → Service → Repository (clsUtil.FeachRequset)` مع Mapper وZod و`validate1`.

---

## أولاً: الوضع الحالي (ملخص التحليل)

### 1. المنطق المنفَّذ عبر Domain Events حالياً (يجب إزالته)

| الحدث | المصدر (Domain) | المعالج (Application) | المنطق المخفي |
|---|---|---|---|
| `AssigmentCreatedEvent` | `clsAssignment` (الـ constructor) | `Assignment/Event/AssignmentCreatedEventHandler.cs` | يغيّر حالة `clsAssignmentNomination` إلى `Completed` عند إنشاء الاختبار |
| `AssigmentDeletedEvent` | `clsAssignment.SetDeleted` | `Assignment/Event/AssignmentDeletedEventHandler.cs` | يلغي الترشيح (`Canceled`) ويحذف النتائج المرتبطة بالاختبار |
| `ResultCreatedEvent` | `clsResult` (الـ constructor) | `Result/Event/ResultCreatedEventHandler.cs` | يحسب النسبة المرجّحة، ويحدد النجاح (`>= 50`)، ويكتب `TotalScore` و`IsPassed` و`Completed` على الاختبار |

ملفات الأحداث: `System.Domain/Entities/Assignment/Events/AssignmentEvent.cs`, `AssignmentDeletedEvent.cs`, `ResultEvent.cs`.

### 2. مشكلة تنفيذ الأحداث نفسها (دليل على هشاشة النمط)

- `AppDbContext.SaveChangesAsync` (السطور 89–108) ينشر الأحداث **داخل** الحفظ لأوامر `ICommand`.
- `TransactionBehavior` (السطور 40–53) يمسح الأحداث ثم يحفظ ثم ينشرها **بعد** `SaveChangesAsync`؛ أي تعديل يقوم به المعالج بعد الحفظ (مثل تحديث `Assignment` في `ResultCreatedEventHandler`) **لا يُحفظ** لأن لا يوجد `SaveChanges` لاحق. هذا يعني أن `NewAssignmentCommand` (وهو `ITransaction`) قد لا يحفظ نتائج المعالجات. يحتاج تأكيداً بالاختبار قبل الإزالة، لكنه سبب كافٍ للإزالة.
- النتيجة: سلوك مختلف بين `ICommand` و`ITransaction` للمنطق نفسه.

### 3. أخطاء Backend مؤكدة من قراءة الكود

**أ) تحديث يُنفَّذ كإضافة (`_repo.Add`) — ينشئ سجلاً جديداً بدل التعديل:**
- `Assignment/Command/UpdateCommandHandler.cs` السطر 29
- `Result/Command/UpdateCommandHandler.cs` السطر 40 (وكذلك `UpdateResultCommandHandler` يبني `clsResult` جديداً بالـ Mapper دون تحميل السجل الأصلي)
- `AssignmentNomination/Command/UpdateCommandHandler.cs` السطر 29
- `AssignmentScope/Command/UpdateCommandHandler.cs` السطر 30
- `AssignmentScopeInfo/Command/UpdateCommandHandler.cs` السطر 35

**ب) خطأ أولوية المنطق (Operator Precedence) في الاستعلامات:**
- `AssignmentService.HasAssignment` (السطور 92–97): `A && B || C` بدون أقواس؛ فيعيد `true` لأي اختبار `Completed` في النظام بغض النظر عن الترشيح. وبالتالي يمنع إنشاء اختبار لترشيح آخر.
- `AssignmentNominationService.HasNomination` (السطور 82–90): الفرع الثاني `inProgress && IsActive && DeletedAt==null` غير مقيّد بالطالب ولا بالنطاق؛ أي ترشيح قيد التنفيذ في النظام يمنع كل الترشيحات الجديدة.

**ج) حالة الترشيح تُكتب عند إنشاء الاختبار لا عند اكتماله:**
- سلوك حالي: إنشاء `Assignment` → الترشيح `Completed` فوراً. هذا غير منطقي (الاكتمال يجب أن يكون بعد النتيجة). يحتاج قراراً (انظر القسم الخامس).

**د) تسمية/Namespace منسوخ بخطأ:**
- `AssignmentNomination/Command/DeleteCommandHandler.cs` في مسار `Services/Assignment/AssignmentNomination` لكن namespace هو `System.Application.Services.Assignment.Assignment.Command` (يخالف قاعدة مطابقة المسار مع namespace في AGENTS).
- `Assignment/Event/*` namespace `...Assignment.EventHandlers` بينما المسار `Assignment/Event` (سيُحذف أصلاً).

**هـ) `ResultCreatedEventHandler` يعدّل `scopeInfoResults` في الذاكرة:** يضيف `new clsResult(...)` إلى قائمة لا تُحفظ، ويحسب النتيجة على بيانات غير متسقة؛ هذا منطق حساب مخفي داخل معالج حدث.

**و) عتبة النجاح ثابتة (`>= 50`)** في المعالج، بينما كيان `clsAssignmentScopeinfo` يحتوي `MinPassScore` غير مستخدم في الحساب. يحتاج قراراً (انظر القسم الخامس).

**ز) الأمان:**
- `AssignmentController`، `ResultController`، `AssignmentScopeController`، `AssignmentScopeInfoController`، `AssignmentNominationController` **بدون `[Authorize]`** ولا Policies (تحقق بالبحث عن `[Authorize` داخل `Controllers/Assignment` فلم يظهر أي نتيجة).
- `GetAssignmentList` و`GetResultList` تعيد قائمة كاملة دون `Gridify` ولا تقييد بالنطاق.
- لا توجد صلاحيات `Can_*_Assignment` / `Can_*_Result` مربوطة في Controllers (على عكس `StudentEnrollmentController` الذي يستخدم `Can_Create_StudentEnrollment` وغيرها).

**ح) جودة الكود:**
- `AssignmentService.cs` يحتوي تعليقات بترميز تالف (Mojibake)؛ يُعالج عند التنظيف.
- `clsAssignmentScope.cs` يحتوي `using` مكرراً.
- `NewAssignmentCommandHandler` يحقن `IMediator` دون استخدام، ويعدّل خصائص الأمر (`request.ScheduledDate`, `request.CommitteeID`) بدل بناء الكيان بقيم محسوبة صريحة.
- `UpdateResultCommandHandler` يستخدم `_repo.Add` مع `SetCreated` (انظر أ).

### 4. أخطاء Frontend مؤكدة

**Assignment:**
- `views/Assigment/AssignmentView.vue`:
  - `OnDelete` بلا `await` ولا تأكيد `confirm` (قارن مع `StudentEnrollmentView.OnDelete`).
  - `AddResultSubmit` يعيد `return` عند وجود `resultID` دون تنفيذ أي تعديل؛ لا يوجد مسار تحديث نتيجة فعلي.
  - `AssignmentScopeinfoData` مُعرَّف كـ `[]` ويُمرَّر إلى `UpsertResultComp` المعرّف بنوع `Object` (عدم تطابق نوع).
- `store/Assignment/AssignmentStore.js`:
  - أسماء أكشنات غير دقيقة: `fetchStagetBy` و`updateStage` (نسخ من Stage) و`useassignmentstore`.
  - `DeleteAssignment` يستدعي `fetchassignments` بعد النجاح؛ مقبول، لكن لا يوجد تعامل مع حالة `saving`.
- `Service/Assignment/AssignmentService.js`:
  - `getBy` يحتوي `console.log(rees)` (تنظيف مطلوب).
  - `update` يتحقق بـ `AssignmentSchema` الذي لا يغطي `committeeID` ولا `scheduledDate` (لكن العقد الحالي يعتمد على الخادم لتعيينها).
- `repositories/Assigment/AssignmentRepositories.js`:
  - `Delete(param)` يمرّر `param` مباشرة كبيانات طلب لـ `DELETE`؛ يجب مطابقته لنمط `StudentEnrollmentRepositories.Delete` (`{ data: {...} }`) بعد التحقق من تنفيذ `clsUtil.FeachRequset`.
  - رسالة الخطأ في `add`/`update`/`getList` تحمل نصوص لا تطابق الكيان (مثل "طلب التسجيل").
- `Mapper/Assigment/AssigmentMapper.js`:
  - اسم الكلاس منسوخ: `export default class StageMapper`.
  - `mapUpsertToApi` يرسل `CommitteeID` و`CommitteeType` من العميل رغم أن الخادم يحسبهما؛ يُفضّل إزالتهما من العقد أو توثيق أن الخادم يتجاهلهما.
  - اسم الخاصية `assigmentScopeName` (تكرار إملائي) مقابل `assigmentScopeName` في عقود أخرى — يحتاج توحيداً في Mapper فقط.
- `components/Assigment/Assigment/AssigmentList.vue`: الأزرار `ShowInfo` و`ShowUpdate` تُصدر أحداثاً لا يستمع لها `AssignmentView`.
- `views/Assigment/AssignmentNominationView.vue` يستخدم `UpsertAssigmentComp` لبدء الاختبار من ترشيح محدد؛ بعد فحص الـ View تبيّن أن هذا الاستخدام متسق مع تدفق `OnStartAssigment`، وليس دليلاً وحده على استخدام مكوّن خاطئ. لا يُستبدل بمكوّن الترشيح إلا إذا تغيّر نطاق الشاشة.

**Assignment Nomination:**
- `components/Assigment/AssignmentNomination/UpsertAssignmentNominationComp.vue`:
  - `alert(this.studentEnrollmentID)` في `fillSelectedAssignmentNomination` (السطر 100) — تصحيح debug يجب إزالته.
  - الشرط `studentEnrollmentID !== null || !== ""` دائماً `true` (السطور 96–99).
  - `AssigmentScopesData` مُعرَّف `Object` ويُمرَّر `Array`.
  - `ValidationError :messages="errors?.date"` يُستخدم فوق اسم الطالب (السطر 8) بخطأ في الربط.
  - اسم المكوّن `UpsertStageInfoComp` منسوخ.
  - `formData.date` قيمة ابتدائية `""` ثم `entity.date || 0` (نوع غير متسق).
- `StudentEnrollmentView.vue` (السطور 76–82) يمرر `AssignmentScopeStore.AssignmentScopes` و`studentEnrollmentID`؛ يجب التحقق من أن الـ Store يحمّل القائمة قبل فتح النافذة.

**Result:**
- `components/Assigment/Result/UpsertResultComp.vue`:
  - `AssigmentScopeInfossData` مُعرَّف `Object` بينما يُمرَّر `Array`.
  - `fillSelectedResult` الشرط `asignmentID !== null || asignmentID !== ""` دائماً `true` (السطور 109–111).
  - عرض الخيارات باسم `stageInfoMatterNameName` — يجب التحقق من الـ Mapper أنه يعيد هذه الخاصية فعلاً.
  - يستخدم `name: "UpsertStageInfoComp"` منسوخ.
  - لا يوجد `v-model` ولا حالة `saving`.
- فُحصت `views/Assigment/ResultView.vue` و`store/Assignment/ResultStore.js` و`Service/Assignment/ResultService.js` و`repositories/Assigment/ResultRepositories.js` و`Mapper/Assigment/ResultMapper.js`. شاشة التعديل تبني خياراتها من قائمة النتائج المحلية بدلاً من استدعاء Lookup موثوق، و`ResultMapper` يقرأ `DataReq.stageInfoMatter` بحروف صغيرة رغم أن DTO يعرّف `StageInfoMatter`.

---

## ثانياً: ترتيب تنفيذ الخدمات (Dependency Order)

1. **الخدمة 1 — AssignmentScope (الكتالوج الأساسي):** لا تعتمد على أي خدمة Assignment أخرى. تشمل إصلاح `Update` وتوحيد الـ Controller والصلاحيات.
2. **الخدمة 2 — AssignmentScopeInfo (وحدات الاختبار والأوزان):** تعتمد على AssignmentScope (FK) وStage/StageInfo. تشمل إصلاح `Update` وتقييد `GetListForAssigmet`.
3. **الخدمة 3 — AssignmentNomination (الترشيح):** تعتمد على StudentEnrollment وAssignmentScope. تشمل إصلاح `HasNomination` وتحديد آلة حالة الترشيح صراحةً وإزالة الاعتماد على حدث الإنشاء.
4. **الخدمة 4 — Assignment (الاختبار نفسه):** تعتمد على Nomination. تشمل إزالة `AssigmentCreatedEvent` و`AssigmentDeletedEvent` واستبدالهما بتنسيق صريح في Handlers، وإصلاح `HasAssignment` و`Update`.
5. **الخدمة 5 — Result (النتائج والحساب):** تعتمد على Assignment وAssignmentScopeInfo. تشمل إزالة `ResultCreatedEvent`، ونقل الحساب إلى سياسة Domain نقية، وتنسيق إكمال الاختبار صراحةً عبر خدمة Application.

> تنبيه: الخدمة 4 والخدمة 5 مترابطتان في الحساب؛ يُنفَّذ الحساب الصريح في الخدمة 5 مع إبقاء Handler الاختبار كما هو بعد الخدمة 4، ولا يُنقل منطق الحساب إلى الخدمة 4.

---

## ثالثاً: بطاقات الخدمات التفصيلية

### الخدمة 1 — AssignmentScope

**1. العيوب المكتشفة**
- `AssignmentScope/Command/UpdateCommandHandler.cs` السطر 30: `_repo.Add` بدل التحديث.
- `clsAssignmentScope.cs`: `using` مكرر (السطور 1–4).
- `AssignmentScopeController`: بدون `[Authorize]` ولا Policies.
- `AssignmentScopeService.GetList()` بدون Gridify ولا نطاق.
- `GetCanNominateQuery` يعتمد على `vw_StudentAvailableNominationScopes` (View) — يجب التحقق من أنه يقيّد الطالب بتسجيلاته فقط.
- Frontend: `AssignmentScopeRepositories copy.js` اسمه يوحي بأنه نسخة، لكنه مستخدم فعلياً من `AssignmentScopeService.js`. لا يُحذف؛ يُعاد تسميته إلى `AssignmentScopeRepositories.js` مع تحديث الاستيراد ضمن تنفيذ الخدمة.

**2. خطة الـ Backend**
- إصلاح `UpdateAssignmentScopeCommandHandler`: تحميل `GetEntityBy(id)`، ثم `SetName/SetNotes/SetStageID/SetPrevAssignmentScopeID`، ثم `Update`. بلا `Add`.
- `AssignmentScopeController`: `[Authorize]` على مستوى Controller، و`Can_Create_AssignmentScope` / `Can_Update_AssignmentScope` / `Can_Delete_AssignmentScope` / `Can_Read_AssignmentScope` على الإجراءات.
- تعريف الصلاحيات في `clsPermission` (تغيير إعداد مشترك — يحتاج موافقة).
- `GetAllAssignmentScopeQuery`: إضافة `BaseFilterQuery` + `GridifyAsync` إن كان الحجم كبيراً؛ يُحدد بالتحقق.
- `GetCanNominateQuery`: التحقق من أن الـ View مقيّد بالطالب (`EnrollmentID`) وأن الصلاحية `Can_Read_AssignmentScope` أو صلاحية الترشيح مطبّقة.
- نقل `prevAssignmentScopeID` إلى تحقق: لا يشير إلى نفسه (يُضاف في `clsAssignmentScope` كـ Invariant).

**3. خطة منطق الـ Frontend**
- `repositories/Assigment/AssignmentScopeRepositories.js`: التحقق من عناوين الـ endpoints مقابل Controller.
- `Service/Assignment/AssignmentScopeService.js` و`validations/Assignment/AssignmentScopeSchema.js`: مطابقة الحقول `name`, `notes`, `stageID`, `prevAssignmentScopeID` مع Domain Exceptions.
- `store/Assignment/AssignmentScopeStore.js`: التحقق من تحميل القائمة مرة واحدة لا بعد فتح كل نافذة (يُستخدم في `StudentEnrollmentView` وفي `AssignmentScopeView`).
- `Mapper/Assigment/AssignmentScopeMapper.js`: توحيد الأسماء camelCase.

**4. خطة الـ UI/UX**
- `UpsertAssignmentScopeComp.vue`: `Stage` عبر Standard Select إن كانت المراحل قليلة (مستقرة)، أو `GenericSearchByFilter` إن كبرت. `prevAssignmentScopeID` عبر Standard Select مفلتر بنفس `StageID` (Cascading خفيف) مع منع اختيار النفس.
- Edit Mode: تهيئة `stageID` قبل فتح النموذج.

**5. تطبيق توجيهات المستخدم**
- لا Domain Events. الدالة `SetDeleted` تبقى كما هي (من `clsBaseClass`) دون إضافة أي حدث.

---

### الخدمة 2 — AssignmentScopeInfo

**1. العيوب المكتشفة**
- `AssignmentScopeInfo/Command/UpdateCommandHandler.cs` السطر 35: `_repo.Add` بدل التحديث.
- `AssignmentScopeInfoService.GetListForAssigmet` (السطور 49–72): منطق "الاختبارات المتبقية" داخل Repository ويعيد `StageInfoMatterNameName` بدون `AssignmentScopeName`؛ يخلط مسؤولية الاستعلام بقاعدة الأعمال.
- `GetListForAssigmet` يستخدم `_context.Assignments` ويستنتج `ScopeID` من الاختبار مباشرة؛ يجب نقله إلى Query Handler يستخدم `GetByAssigmentIDQuery`.
- `clsAssignmentScopeinfo.SetMinPassScore` يتحقق من `MaxScore` **قبل** تعيينه في الـ constructor (ترتيب استدعاء `SetMaxScore` ثم `SetMinPassScore` صحيح في الـ constructor، لكن `SetMaxScore` لا يحقق `MinPassScore <= MaxScore` عند تغييره لاحقاً — Invariant ناقص).
- `Weight` من نوع `float` مع فحص `< 0` فقط؛ لا حد أعلى ولا مجموع أوزان لكل `AssignmentScopeID` (يجب أن يكون مجموع الأوزان قابلاً للتحقق؛ قرار مطلوب).
- Controller بدون `[Authorize]`.

**2. خطة الـ Backend**
- إصلاح `UpdateAssignmentScopeInfoCommandHandler` بالتحميل ثم التحديث.
- إضافة Invariant في Domain: `SetMaxScore` يتحقق من `MinPassScore <= MaxScore` عند التعديل (داخل الكيان فقط).
- إضافة `GetAssignmentScopeInfoForAssignmentQuery` في Application يحسب المتبقي باستخدام `Results` عبر Repository مخصص، بدل منطق داخل `GetListForAssigmet`.
- `AssignmentScopeInfoController`: `[Authorize]` + Policies (`Can_Read_AssignmentScopeInfo` للقراءة، `Can_Create/Update/Delete`).
- التحقق من `HasAssignmentScopeInfo` في `NewCommandHandler` لمنع التكرار (موجود في Repository، يجب التأكد من استخدامه).

**3. خطة منطق الـ Frontend**
- `repositories/Assigment/AssignmentScopeinfoRepositories.js` ↔ `AssignmentScopeInfoController` (مطابقة الروابط).
- `Service/Assignment/AssignmentScopeInfoService.js`: `fetchAssignmentScopeinfosForAssigment` تُحفظ مع Mapper.
- `validations/Assignment/AssignmentScopeinfoSchema.js`: قيود `maxScore >= minPassScore >= 0`، `weight >= 0`، `stageInfoID` مطلوب.
- `store/Assignment/AssignmentScopeInfoStore.js`: فصل الحالة العامة (قائمة الكل) عن قائمة الاختبار المحدد.

**4. خطة الـ UI/UX**
- `UpsertAssignmentScopeInfoComp.vue`: `AssignmentScope` عبر Standard Select (قليل) ، `StageInfo` عبر Cascading Combo مرتبط بالمرحلة (Stage → StageInfo) مع مسح الاختيار التابع عند تغيير الأب.
- عرض `maxScore` و`minPassScore` كحقول رقمية مع `ValidationError` مطابقة للـ Invariant.

**5. تطبيق توجيهات المستخدم**
- لا Domain Events. قاعدة `minPassScore <= maxScore` تُفرض داخل الكيان بدالة صريحة.

---

### الخدمة 3 — AssignmentNomination

**1. العيوب المكتشفة**
- `AssignmentNominationService.HasNomination` (السطور 82–90): خطأ أولوية المنطق (انظر أولاً.3.ب).
- `AssignmentNomination/Command/UpdateCommandHandler.cs` السطر 29: `_repo.Add`.
- `AssignmentNomination/Command/DeleteCommandHandler.cs`: namespace غير مطابق للمسار.
- حالة الترشيح تُكتب من معالج حدث الإنشاء (`Completed`) — يجب إزالته.
- `NewCommandHandler` يعدّل `request.NominatedByID`, `request.NominatedByType`, `request.NominationState` (تعديل مدخلات الأمر).
- `GetCanNominateQuery` في Scope يقرر إمكانية الترشيح؛ القاعدة مزدوجة (الـ View + HasNomination) وقد تتعارض.
- Controller بدون `[Authorize]`.
- Frontend: `UpsertAssignmentNominationComp.vue` (انظر أولاً.4)، و`AssignmentNominationView.vue` يستخدم مكوّن الاختبار بدل الترشيح (تحقق).

**2. خطة الـ Backend**
- إصلاح `HasNomination`: `StudentEnrollmentID == X && AssignmentScopeID == Y && NominationState != Canceled && IsActive && DeletedAt == null` (قاعدة واحدة واضحة، بأقواس صريحة).
- إصلاح `UpdateAssignmentNominationCommandHandler` بالتحميل ثم التحديث، مع منع تغيير `StudentEnrollmentID`/`AssignmentScopeID` إذا كان هناك اختبار مرتبط (Invariant في Domain).
- توحيد namespace الملف مع مساره.
- آلة حالة الترشيح صراحةً في Domain:
  - `inProgress` عند الإنشاء.
  - `Canceled` بأمر صريح عند إلغاء الترشيح أو حذف الاختبار (ينفذه Handler الاختبار صراحةً، لا حدث).
  - `Completed` عند اكتمال الاختبار (ينفذه تسلسل الخدمة 5 صراحةً).
  - دوال: `MarkInProgress()`, `Cancel()`, `Complete()` على `clsAssignmentNomination`.
- `NewCommandHandler`: بناء الكيان من الأمر دون تعديل خصائص الأمر (تحويل `NominatedByID` عبر دالة صريحة في الكيان أو Mapper فقط).
- `[Authorize]` + `Can_Create_AssignmentNomination` / `Can_Read_AssignmentNomination` / `Can_Update_...` / `Can_Delete_...`.
- نطاق البيانات: المعلم يرى ترشيحات طلاب مجموعاته فقط (عبر `clsTeachingAssignment` و`clsStudentEnrollment`)، والطالب يرى ترشيحاته فقط؛ الإدارة كاملة. يُطبّق بدالة امتداد `AssignmentNominationScopeExtensions` في `System.Infrastructure/Extensions/Security/` قبل `GridifyAsync`.

**3. خطة منطق الـ Frontend**
- `UpsertAssignmentNominationComp.vue`: إزالة `alert`، تصحيح شروط `fillSelectedAssignmentNomination`، تغيير نوع `AssigmentScopesData` إلى `Array`، تصحيح الـ `ValidationError`، تنظيف `date`.
- `AssignmentNominationView.vue`: تصحيح الاستيراد إلى `UpsertAssignmentNominationComp` (إن كان مكوّن الاختبار مستخدماً بالخطأ).
- `Service/Assignment/AssignmentNominationService.js` و`Schema`: `studentEnrollmentID` و`assignmentScopeID` و`date` مطابقة لـ `NewAssignmentNominationCommand`.
- `store/Assignment/AssignmentNominationStore.js`: التحقق من تفريغ `selectedAssignmentNomination` عند الإغلاق.

**4. خطة الـ UI/UX ونمط الاختيار**
- نطاق الاختبار (`AssignmentScope`) Standard Select (قائمة محدودة ومستقرة).
- الطالب: مُهيأ من `StudentEnrollment` (لا يُغيَّر داخل النموذج) — بطاقة للعرض فقط.
- التاريخ: `date` بقيمة ابتدائية صريحة `YYYY-MM-DD`.
- حالات: تحميل النطاقات، خطأ التحميل، لا توجد نطاقات متاحة (الطالب لا يملك نطاقات قابلة للترشيح).

**5. تطبيق توجيهات المستخدم**
- حالة الترشيح تنتقل صراحةً عبر دوال الكيان من Handlers؛ لا حدث، ولا تغيير من معالج آخر.

---

### الخدمة 4 — Assignment (الاختبار)

**1. العيوب المكتشفة**
- `clsAssignment` (الـ constructor السطر 32، و`SetDeleted` السطر 77): يضيفان `AddDomainEvent` — **يُزال**.
- `Assignment/Event/AssignmentCreatedEventHandler.cs` و`AssignmentDeletedEventHandler.cs`: منطق مخفي — **يُحذف** بعد نقل السلوك.
- `Domain/Entities/Assignment/Events/AssignmentEvent.cs` و`AssignmentDeletedEvent.cs`: **تُحذف**.
- `AssignmentService.HasAssignment` (السطور 92–97): خطأ أولوية المنطق.
- `Assignment/Command/UpdateCommandHandler.cs` السطر 29: `_repo.Add`.
- `NewAssignmentCommandHandler`: يعدّل خصائص الأمر ويحقن `IMediator` دون استخدام.
- `DeleteAssignmentCommandHandler`: لا يلغي الترشيح ولا يحذف النتائج (كان ذلك في الحدث المحذوف).
- `AssignmentService.GetList()` يستعلم بشرط `IsActive` ضمني دون تحقق صريح من `DeletedAt`، ويحتوي `CanAddResult` بتعقيد مكرر.
- `AssignmentController`: بدون `[Authorize]`، وبدون Gridify أو Paging.
- `GetAllAssignmentQuery` يعيد `TResult<List<...>>` بدون Paging.

**2. خطة الـ Backend (فصل المسؤوليات)**
- **Domain:**
  - `clsAssignment`: إزالة `AddDomainEvent` من constructor و`SetDeleted`.
  - إضافة دوال انتقال صريحة: `Complete(double totalScore, bool isPassed)`، `Cancel()`؛ تُستدعى من Application عند الحاجة فقط.
  - Invariant: لا يُنشأ `clsAssignment` لترشيح غير `inProgress` أو مع ترشيح به اختبار نشط (يُتحقق عبر Application لأن الفحص يحتاج استعلاماً).
- **Application (Handlers):**
  - `NewAssignmentCommandHandler` (ITransaction): تحقق `HasAssignment(nominationID)` مُصحَّح → إنشاء `clsAssignment` بقيم محسوبة صريحة → إن لزم تحديث الترشيح (وفق قرار القسم الخامس) عبر `nomination.MarkInProgress()` أو تركه كما هو → حفظ داخل المعاملة نفسها.
  - `DeleteAssignmentCommandHandler` (ITransaction، وليس ICommand لأنه يعدّل عدة كيانات): تحميل الاختبار → `Cancel`/`SetDeleted` → تحميل الترشيح → `nomination.Cancel()` → تحميل النتائج `GetListByAssignmentID` → `SetDeleted` لكل نتيجة → حفظ واحد داخل المعاملة.
  - `UpdateAssignmentCommandHandler`: تحميل بـ `GetEntityBy` ثم تحديث بالـ Mapper (`_mapper.Map(request, entity)`) ثم `Update`. بلا `Add`.
- **Infrastructure:**
  - `AssignmentService.HasAssignment`: `x.AssignmentNominationID == NominationID && (x.AssignmentState == inProgress || x.AssignmentState == Completed) && x.DeletedAt == null` بأقواس صريحة.
  - `GetList()` بـ `GridifyAsync` بعد `ApplyReadScope`، وإزالة الـ subquery المكرر لـ `CanAddResult` إلى دالة استعلام مستقلة.
- **WebApi:**
  - `[Authorize]` على Controller + `Can_Create_Assignment` / `Can_Update_Assignment` / `Can_Delete_Assignment` / `Can_Read_Assignment`.
  - `GetAssignmentList` يتحول إلى `GetAssignmentListFilter` بـ `BaseFilterQuery<AssignmentInfo>` (تغيير عقد — يحتاج موافقة).
- **حذف الأحداث:** حذف ملفات `Events` وHandlers الخاصة بـ Assignment، وإزالة `using` المرتبطة.

**3. خطة منطق الـ Frontend**
- `AssignmentView.vue`:
  - `OnDelete` مع `confirm` و`await`.
  - فصل `OnAddResult` (فتح نموذج النتيجة) عن `AddResultSubmit` (الحفظ).
  - إصلاح تمرير `AssignmentScopeinfoData` كـ `Array` وتوحيد نوع الخاصية في `UpsertResultComp`.
- `AssignmentStore.js`: إعادة تسمية `fetchStagetBy` → `fetchAssignmentBy`، و`updateStage` → `updateAssignment`، و`useassignmentstore` → `useAssignmentStore` (يتطلب تحديث كل الاستيرادات — بعد موافقة)، وإضافة `saving` مستقل.
- `AssignmentService.js`: إزالة `console.log`.
- `AssignmentRepositories.js`: تصحيح `Delete` ليرسل `{ data: {...} }` مطابقاً لنمط الـ Repository الآخر بعد التحقق من `clsUtil.FeachRequset`؛ توحيد رسائل الخطأ.
- `AssigmentMapper.js`: إعادة تسمية الكلاس إلى `AssignmentMapper`، وتوحيد أسماء الحقول، وإزالة `CommitteeID`/`CommitteeType` من payload الإنشاء إن لم يحتجهما الخادم (يتطلب تأكيد عقد).
- `AssignmentSchema.js`: إضافة `scheduledDate` و`committeeID` كحقول اختيارية حسب العقد، وتوثيق القيود.
- `AssigmentList.vue`: ربط الأحداث `ShowInfo`/`ShowUpdate` بمعالجات حقيقية في `AssignmentView` أو إزالتها إن لم تكن مطلوبة.
- `AssignmentNominationView.vue`: التحقق من استخدام `UpsertAssigmentComp` (انظر أولاً.4).

**4. خطة الـ UI/UX**
- القائمة: حالة الاختبار `assignmentState` بعرض عربي (inProgress/Completed/Canceled)، و`isPassed` بشارة (نجح/رسب/لم يحسب بعد) بدل قيمة `true/false` الخام، و`totalScore` مع رقمين عشريين.
- زر "اضافة نتيجة" يظهر فقط عند `canAddResult === true` (موجود) مع توضيح سبب عدم الإتاحة إن لم يظهر (اختيارياً).
- حالات: تحميل، خطأ، قائمة فارغة (تعمل مع Paging بعد الخدمة).
- حذف: نافذة تأكيد صريحة تبين أن الترشيح سيُلغى وأن النتائج ستُحذف.

**5. تطبيق توجيهات المستخدم**
- الإلغاء والحذف والإكمال تُنفَّذ **صراحةً** في Handler واحد داخل معاملة واحدة، بلا أي حدث. هذا يحافظ على فصل المسؤوليات (كل Handler يملك حالة الاستدعاء وتسلسله)، ويلغي الآثار الجانبية الخفية.

---

### الخدمة 5 — Result (النتائج والحساب)

**1. العيوب المكتشفة**
- `clsResult` constructor (السطر 27): `AddDomainEvent(new ResultCreatedEvent(...))` — **يُزال**.
- `Result/Event/ResultCreatedEventHandler.cs`: منطق حساب وإكمال مخفٍ — **يُحذف** بعد نقله.
- `Domain/Entities/Assignment/Events/ResultEvent.cs`: **يُحذف**.
- `ResultCreatedEventHandler` يضيف `clsResult` جديداً إلى قائمة في الذاكرة دون حفظ، ويعتمد على `TryGetValue` لكل نتيجة.
- عتبة النجاح `50.0m` ثابتة، و`MinPassScore` غير مستخدم (قرار مطلوب، انظر القسم الخامس).
- `UpdateResultCommandHandler`: `_repo.Add` مع بناء كيان جديد؛ يجب التحميل والتحديث.
- `NewResultCommandHandler` يتحقق من `scopeInfo.MaxScore` بينما `clsResult.SetScore` يتحقق من `< 0` فقط؛ قاعدة النطاق الأعلى يجب أن تكون Invariant داخل Domain (`score <= maxScore`) أو تنتقل إلى سياسة Application موحدة.
- `ResultController` بدون `[Authorize]`؛ و`GetResultList` يعيد كل النتائج النشطة دون نطاق.
- `ResultService.GetList(Guid[] scopeInfos)` يحمّل كيانات كاملة (`ToListAsync`) — مقبول لكنه يستدعي `Add` أو `Update` على كيانات قادمة من الاستعلام دون `AsTracking` واضح.

**2. خطة الـ Backend (فصل المسؤوليات)**
- **Domain (نقي):**
  - `clsResult`: إزالة `AddDomainEvent`. إضافة Invariant `Score <= MaxScore` عبر تمرير `maxScore` إلى دالة `SetScore` أو عبر سياسة.
  - **سياسة حساب نقية** `AssignmentScoringPolicy` في `System.Domain/Services/Assignment/` (static أو service بلا تبعيات): تستقبل قائمة `(score, maxScore, weight, minPassScore)` وتعيد `(totalPercentage, isPassed)`. لا تصل إلى أي Repository.
  - دالة `clsAssignment.Complete(totalPercentage, isPassed)` تكتب الحالة (انظر الخدمة 4).
- **Application (تنسيق صريح):**
  - خدمة تنسيق `AssignmentEvaluationService` في `System.Application/Services/Assignment/Result/` (أو `Abstractions/Assignment` للعقد): تُستدعى صراحةً من `NewResultCommandHandler` و`UpdateResultCommandHandler` و`DeleteResultCommandHandler`. تقوم بـ:
    1. تحميل نتائج الاختبار النشطة عبر `IResultRepository`.
    2. تحميل نطاقات المعلومات عبر `IAssignmentScopeInfoRepository`.
    3. استدعاء `AssignmentScoringPolicy`.
    4. تحميل الاختبار وتنفيذ `Complete(...)`.
  - تُنفَّذ داخل نفس `ITransaction` ويتم `SaveChanges` مرة واحدة في نهاية الـ Handler (لا بعد الحفظ).
  - `NewResultCommand` و`UpdateResultCommand` و`DeleteResultCommand` تتحول إلى `ITransaction` (لأنها تعدّل النتيجة والاختبار معاً).
- **Infrastructure:**
  - `ResultService.GetList(Guid[])` و`GetListByAssignmentID`: قراءة فقط، بلا تعديل.
  - `UpdateResultCommandHandler`: إصلاح `Add` → تحميل + `Update`.
- **WebApi:**
  - `[Authorize]` + `Can_Create_Result` / `Can_Update_Result` / `Can_Delete_Result` / `Can_Read_Result`.
  - نطاق: المعلم يسجّل نتائج اختبارات تحت لجنته فقط (`CommitteeID == EntityGuid`)، والطالب يقرأ نتائجه فقط.

**3. خطة منطق الـ Frontend**
- `ResultStore.js` و`ResultView.vue` و`UpsertResultComp.vue`:
  - إضافة مسار تحديث النتيجة فعلياً (`resultID`).
  - تصحيح `fillSelectedResult` (الشرط الحالي دائماً `true`).
  - توحيد نوع `AssigmentScopeInfossData` إلى `Array`.
  - إعادة تسمية `name: "UpsertStageInfoComp"` إلى `UpsertResultComp`.
  - إضافة حالة `saving` وعرض الخطأ من الـ Store.
  - التحقق من وجود `ResultRepositories` و`ResultService` و`ResultMapper` والاتساق مع `ResultInfo`.
- Schema: `validations/Assignment/ResultSchema.js` (إن لم يوجد) بحقول `assignmentScopeInfoID` (مطلوب)، `score` (رقم `>= 0`)، `notes` (اختياري).

**4. خطة الـ UI/UX**
- اختيار `AssignmentScopeInfo`: Standard Select (قائمة محدودة لاختبار واحد)، يعرض المادة والوزن والدرجة القصوى.
- الدرجة: حقل رقمي مع `min=0` و`max=maxScore` المحدد من الاختيار، مع رسالة خطأ واضحة قبل الإرسال.
- بعد الحفظ: عرض النسبة المحدثة والنجاح/الرسوب للاختبار.
- حالات: تحميل نطاقات الاختبار، لا توجد نطاقات متبقية (تم إدخال كل النتائج)، خطأ الحفظ.

**5. تطبيق توجيهات المستخدم**
- لا Domain Events. الحساب في سياسة نقية، والتنسيق في Handler صريح، والحفظ مرة واحدة داخل المعاملة.

---

## رابعاً: إزالة البنية القديمة للأحداث (خطوة مشتركة)

- حذف: `System.Domain/Entities/Assignment/Events/*`، `System.Application/Services/Assignment/Assignment/Event/*`، `System.Application/Services/Assignment/Result/Event/*`.
- الحقول: `clsBaseClass` (`_domainEvents`, `AddDomainEvent`, `RemoveDomainEvent`, `ClearDomainEvents`) و`IDomainEvent` و`AppDbContext.SaveChangesAsync` (ينشر الأحداث) و`TransactionBehavior` (ينشرها) و`UnitOfWork.GetEntitiesWithDomainEvents`.
- **قرار مطلوب:** حالياً فقط وحدة Assignment تستخدم `AddDomainEvent` (تم التحقق بالبحث عن `AddDomainEvent` وكل `DomainEvent`). لذلك حذف البنية المشتركة آمن من ناحية الاستخدام، لكنه **تغيير على إعدادات/بنية مشتركة** (`clsBaseClass`, `AppDbContext`, `TransactionBehavior`, `UnitOfWork`) ويتطلب موافقة صريحة قبل التنفيذ. يُقترح تنفيذه كخطوة أخيرة بعد الخدمات 1–5.

## خامساً: أسئلة تحتاج قراراً قبل التنفيذ

1. **آلة حالة الترشيح:** هل يصبح الترشيح `Completed` عند اكتمال الاختبار (نتيجة مكتملة) — الاقتراح الموصى به — أم يبقى `inProgress` حتى تُقفل النتيجة؟
2. **عتبة النجاح:** هل النجاح = `TotalScore >= 50` (الحالي)، أم يُعتمد `MinPassScore` لكل نطاق معلومات (كل وحدة لها حد أدنى مستقل)، أم قاعدة مختلطة؟
3. **الحد الأعلى للأوزان:** هل يُفرض مجموع أوزان الوحدات لكل نطاق (مثلاً 100%) أم يبقى الحساب نسبياً؟
4. **حذف الاختبار:** عند حذف اختبار، هل تُحذف النتائج (الحالي) أم تُحفظ كتاريخ؟
5. **الصلاحيات:** تأكيد مصفوفة المعلم/الطالب/الإدارة للوحدة (Assignment, Result, Nomination, Scope, ScopeInfo) وإضافتها إلى `clsPermission`.
6. **`GetAssignmentList`** و`GetResultList`: هل يُحوَّلان إلى Gridify مع Paging الآن (تغيير عقد) أم بعد استقرار الخدمات؟
7. **حذف البنية المشتركة للأحداث** (القسم الرابع): موافقة على تنفيذها في النهاية؟
8. **`AssignmentNominationView.vue`** يستخدم `UpsertAssigmentComp`: هل هو خطأ مقصود أم خطأ نسخ؟

## سادساً: تحقق لم يُنفَّذ بعد (يُفحص قبل بدء كل خدمة)

- `clsUtil.FeachRequset` لمعرفة شكل بيانات `DELETE`.
- `Result` Mapper و`ResultRepositories.js` و`ResultService.js` و`ResultStore.js` و`ResultView.vue` (تم فحصها؛ انظر أخطاء Frontend وملاحظات الخدمة 5 أعلاه).
- `AssignmentScopeInfoMapper` للتأكد من `stageInfoMatterNameName`.
- `GetCanNominateQuery` و`vw_StudentAvailableNominationScopes` لتأكيد قيد الطالب.
- `AssignmentScopeStore` و`AssignmentNominationStore` (تحميل القوائم قبل فتح النوافذ).
- Gridify لكل Handler قائمة في وحدة Assignment، مع حصر الفلاتر المسموح بها وتطبيق نطاق الملكية قبل الإسقاط إلى DTO.

## سابعاً: التحقق المطلوب بعد كل خدمة

- `dotnet build` للمشروع المتأثر.
- `ESLint` للملفات الأمامية المعدّلة.
- اختبار وحدة للـ Domain (السياسة والـ Invariants) قبل تعديل Handlers.
- اختبار تكامل للمعاملات: إنشاء اختبار → حفظ مرة واحدة، حذف اختبار → إلغاء الترشيح وحذف النتائج داخل معاملة واحدة، إضافة نتيجة → تحديث الاختبار.
- مراجعة `git diff` للتأكد من أن التغيير محصور في الخدمة المنفذة.

## ثامناً: شرط التوقف

بعد مراجعة هذه الخطة والإجابة عن الأسئلة في القسم الخامس، يُنفَّذ **الخدمة رقم (1) فقط** عند إعطاء الإشارة من المستخدم، مع التوقف للمراجعة قبل الانتقال للخدمة التالية.

---

## ملحق مراجعة التحقق

تمت مراجعة ملفات الواجهة التي كانت معلّمة سابقاً بأنها تحتاج فحصاً، وتُعتمد التوضيحات التالية عند التنفيذ:

- `AssignmentNominationView.vue` يستخدم `UpsertAssigmentComp.vue` لبدء `Assignment` من ترشيح؛ هذا متسق مع تدفق الشاشة الحالي، وليس خطأً مؤكداً لاستبداله بمكوّن الترشيح.
- `AssignmentScopeRepositories copy.js` مستخدم فعلياً من `AssignmentScopeService.js`؛ المطلوب إعادة تسميته وتحديث الاستيراد، لا حذفه كملف غير مستخدم.
- `AssignmentService.GetBy(Guid id)` لا يفلتر بالـ ID قبل `FirstOrDefaultAsync`؛ إصلاحه شرط قبل الاعتماد على Endpoint التفاصيل أو Edit Mode.
- فُحصت `ResultView.vue` و`ResultStore.js` و`ResultService.js` و`ResultRepositories.js` و`ResultMapper.js`. خيارات تعديل النتيجة مصدرها حالياً القائمة المحلية، كما أن الـ Mapper يقرأ `stageInfoMatter` بدلاً من `StageInfoMatter`.
- مستودعات الواجهة تعرف `IsExist...` methods لا تقابلها Actions ظاهرة في Controllers الحالية؛ يجب مطابقة كل استدعاء بالعقد الخلفي أو إزالة الاستدعاء غير المستخدم ضمن الخدمة المعنية.
- يجب فحص `clsUtil.FeachRequset` قبل تعديل صيغة جسم `DELETE`؛ لا يُفترض شكل Axios من دون التحقق من تنفيذ المساعد.
- يُحظر منطق DomainEvent داخل Assignment كما طلب المستخدم. إزالة أحداث الميزة ومعالجاتها لا تعني حذف Dispatcher أو تعديل `AppDbContext` و`TransactionBehavior` على مستوى المشروع؛ ذلك تغيير مشترك منفصل يحتاج موافقة.
