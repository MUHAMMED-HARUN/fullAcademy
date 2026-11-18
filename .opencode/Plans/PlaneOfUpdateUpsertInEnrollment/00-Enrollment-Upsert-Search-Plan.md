# خطة: استبدال البحث القديم بـ GenericSearchByFilter في Enrollment Upsert

> الحالة: **خطة فقط (Read-Only)** — لم يُعدَّل أي كود. التنفيذ يبدأ بالخدمة رقم (1) فقط بعد موافقة المستخدم.

## النطاق

- المكوّن المستهدف: `UpsertStudentEnrollmentComp.vue` (تسجيل الطالب في مجموعة، وضع الإضافة والتعديل).
- المستهلكون: `StudentEnrollmentView.vue`، `StudentView.vue`، `GroupView.vue`.
- خارج النطاق حالياً: `TeachingAssignment/UpsertTeachingAssignmentComp.vue` (يحوي بحثاً قديماً مشابهاً للمعلم والمجموعة والمادة). يُقترح كخدمة اختيارية لاحقة بعد موافقة المستخدم.

---

## أولاً: ترتيب تنفيذ الخدمات (Dependency Order)

1. **الخدمة 1 — Group Lookup (مستقلة):** Backend (تأمين `GetGroupList`) + Store/Service/Repository للبحث المستقل عن المجموعات. لا تعتمد على أي خدمة أخرى.
2. **الخدمة 2 — Student Lookup (مستقلة):** Store/Service/Repository للبحث المستقل عن الطلاب عبر `GetStudentList` (Gridify) مع تأمين الصلاحية. لا تعتمد على الخدمة 1.
3. **الخدمة 3 — Upsert Component + Views (مركّبة):** تستهلك الخدمتين 1 و2؛ تستبدل البحث القديم داخل `UpsertStudentEnrollmentComp.vue` وتعيد توصيل `StudentEnrollmentView.vue` و`StudentView.vue` و`GroupView.vue`، وتفصل مسار الـ Hydration عن مسار البحث.

---

## ثانياً: بطاقات الخدمات التفصيلية

### الخدمة 1 — Group Lookup

**1. العيوب المكتشفة حالياً**

- `src/store/Institute/GroupStore.js` → `fetchGroups` (السطور 26–47) يكتب على الحالة المشتركة `groups` و`page` و`totalCount`، ويستدعيه أيضاً `GroupView` وشاشات أخرى؛ البحث في الإنرولمنت يلوّث قائمة المجموعات الأساسية.
- `fetchGroups` لا يستخدم `try/finally` ولا يضبط `loading = true` في البداية (السطور 27–45).
- `StudentEnrollmentView.vue` (السطور 422–431) يستدعي `GroupStore.fetchGroups({Filter})` ثم `GroupStore.fetchGroupBy({Param: val, ParamType: type})` بنفس قيمة نوع البحث؛ قيم `type` في الواجهة (`0=ID, 1=GroupName, 2=Code`) لا تطابق بالضرورة `ParamType` في `GetGroupBy` — يجب التحقق قبل التنفيذ.
- `StudentEnrollmentView.vue` **لا يمرر** `:group-results` إلى `UpsertStudentEnrollmentComp` (السطور 39–47)، بينما `StudentView.vue` (السطر 72) يمرره؛ أي أن نتائج البحث عن المجموعة لا تظهر في شاشة التسجيل الرئيسية.
- Backend: `Controllers/Institute/GroupController.cs` السطر 58–59 `GetGroupList` **بدون `[Authorize]`** ولا Policy، بينما بقية الـ Controllers تستخدم `Can_Read_<Entity>`. نفس الأمر ينطبق على `GetGroupsByTeacherID` و`GetGroupBy` (السطور 71 و85).
- الـ Gridify للمجموعات: `System.Application/Services/Institute/Group/Query/GetAllGroupQuery.cs` يرث `BaseFilterQuery<GroupInfo>` ✔، ويحتاج التحقق من `GetAllGroupQueryHandler.cs` أنه يستخدم `GridifyAsync` بعد تطبيق النطاق الأمني.

**2. خطة الـ Backend**

- `Controllers/Institute/GroupController.cs`: إضافة `[Authorize]` على مستوى الـ Controller (إن لم يكن موجوداً) و`[Authorize(Policy = "Can_Read_Group")]` على `GetList` و`GetBy` و`IsExistBy`. أما `GetGroupsByTeacherID` فيُبقى بصلاحية قراءة مناسبة بعد مراجعة من يستدعيه.
- التحقق من وجود `Can_Read_Group` في `clsPermission` وتسجيله في `Program.cs` عبر `GetAllPermissionHandler`. إن لم يوجد يُضاف إلى `clsPermission` (يحتاج موافقة لأنه تغيير إعداد مشترك).
- `GetAllGroupQueryHandler`: التأكد من `GridifyAsync` على النطاق المصفّى (`ApplyReadScope`) — لا تغيير متوقع في Domain أو قاعدة البيانات.
- لا تغيير على شكل `GroupInfo` أو DTO ما لم يكشف التحقق عن تعارض.

**3. خطة منطق الـ Frontend**

- `src/repositories/Institute/` (ملف `GroupRepositories` الحالي): الإبقاء على `getList(params)` لاستخدامه بحقل Gridify فقط، ولا استخدامه بدون `Filter`.
- `src/Service/Institute/GroupService.js`: إضافة `searchForCandidates({ filterType, value })` بنفس نمط `PersonService.searchForCandidates` (السطور 55–67): تحقق Zod، ثم `Page: 1, PageSize: 10`، و`Filter` بصيغة:
  - `GroupName=*قيمة` (filterType=1)
  - `Code=*قيمة` (filterType=2)
  - `GroupID=قيمة` (filterType=0، Equals بدون `*`)
  - بلا `*` في النهاية، والقيمة مُقلَّمة `trim()`.
- `src/store/Institute/GroupStore.js`: إضافة حالات مستقلة `groupSearchResults`, `groupSearchLoading`, `groupSearchError`, `groupSearchRequestId` وأكشن `searchGroupCandidates(criteria)` و`resetGroupCandidateSearch()` بنمط `usePersonStore.searchPersonCandidates` (السطور 111–161) مع حماية الاستجابات القديمة و`finally`. **لا يكتب** على `groups`.
- `src/validations/Institute/` (إن وجد Schema للمجموعة): إضافة `GroupSearchSchema` بحقول `filterType` (0|1|2) و`value` (نص غير فارغ بعد `trim`).
- Mapper: لا تغيير متوقع؛ النتائج تُعاد كـ camelCase بالفعل من الـ Mapper الحالي (`groupID`, `groupName`, `code`) — يُتحقق قبل التنفيذ.

**4. خطة الـ UI/UX ونمط الاختيار**

- النمط: **GenericSearchByFilter** (وليس Standard Select، لأن عدد المجموعات كبير).
- لا يُستخدم `SearchFilterCombo` مباشرة؛ الاكتفاء بالمكوّن الأساسي `GenericSearchByFilter` أو الإبقاء على `SearchFilterCombo` كغلاف فقط بعد التأكد من تمرير `results` و`loading` و`error` بشكل صحيح.
- Edit Mode: تهيئة `groupID` و`groupName` (من `entity.groupName`) و`selectedGroupData` قبل إتاحة الحفظ — الحالي لا يهيّئ `groupNameValue` في `fillSelectedEnrollment` (السطور 182–197) فتظهر الخانة فارغة.

**5. تطبيق توجيهات المستخدم الخاصة**

- المستخدم طلب استبدال البحث القديم بالجديد؛ الحل: إلغاء الاعتماد على `fetchGroups({Filter})` + `GroupList` المشتركة في البحث، واستخدام `searchGroupCandidates` المعزول.

---

### الخدمة 2 — Student Lookup

**1. العيوب المكتشفة حالياً**

- `UpsertStudentEnrollmentComp.vue` السطور 7–16: بحث قديم يدوي بـ `<select>` + `<input>` + زر، ويصدر `onSearchStudent(type, value)` بدون عرض قائمة نتائج.
- `StudentEnrollmentView.vue` (السطور 415–420) و`StudentView.vue` (السطور 352–357): `onSearchStudent` يستدعي `StudentStore.fetchStudentBy({Param, ParamType})` — بحث بمعرّف/رقم وطني واحد فقط (تطابق دقيق، لا اقتراحات)، ويكتب على `StudentStore.selectedStudent`.
- نفس الدالة تُستخدم لـ **Hydration في وضع التعديل** (`OnUpsertEnrollment` السطور 372–382 تستدعي `onSearchStudent(1, studentID)`)؛ فصل الاثنين مطلوب.
- `StudentStore.fetchStudents` (يُستخدم في `StudentView` كقائمة جدول) يكتب على `students` و`totalCount`؛ لا يجب إعادة استخدامه للاقتراحات.
- `GetStudentList` في `Controllers/Person/StudentController.cs` السطر 61 **بدون Policy** (فقط `[Authorize]` على مستوى الـ Controller)، بينما `GetStudentBy` (السطر 75) كذلك؛ لا يوجد `Can_Read_Student` مربوط.
- `GetAllStudentQuery : BaseFilterQuery<StudentInfo>` (`System.Application/Services/Person/Student/Query/GetAllStudentQuery.cs`) ✔ جاهز لـ Gridify؛ يجب التحقق من حقول `StudentInfo` المتاحة للفلترة (الاسم، الرقم الوطني، المعرّف) وأن الـ Handler يستخدم `GridifyAsync`.
- يوجد نمط مرجعي للبحث الجديد: `UpsertStudentComp.vue` (السطور 33–48) يستخدم `GenericSearchByFilter` مع `PersonStore.searchPersonCandidates` — هذا هو الشكل المطلوب تكراره.

**2. خطة الـ Backend**

- `Controllers/Person/StudentController.cs`: إضافة `[Authorize(Policy = "Can_Read_Student")]` على `GetStudentList` و`GetStudentBy` و`IsExistStudentBy`، بعد التأكد من وجود `Can_Read_Student` في `clsPermission`.
- ملاحظة أمنية (إلزامية وفق `backend-security-scoping`): `GetStudentList` لمستخدم `Teacher` يجب أن يُقيَّد بطلاب مجموعاته عبر `clsStudentEnrollment`/`clsTeachingAssignment`، ولمستخدم `Student` بسجله الشخصي فقط. هذا يتطلب `ApplyReadScope` داخل الـ Repository قبل `GridifyAsync`، وهو تغيير أمني قد يتجاوز نطاق هذه الخدمة — **يحتاج قرار المستخدم** (إما ضمن الخدمة 2 أو كخدمة منفصلة).
- لا تغيير على الـ DTO أو الـ Domain.

**3. خطة منطق الـ Frontend**

- `src/repositories/Person/` (ملف `StudentRepositories` أو المكافئ الحالي): الإبقاء على `getList(params)` ← `GET /Student/GetStudentList`.
- `src/Service/Person/` (`StudentService` الحالي): إضافة `searchForCandidates({ filterType, value })` بنفس نمط `PersonService.searchForCandidates`:
  - `filterType` مسموح: `FirstName`, `NationalNumber` (مطابق لـ `studentFilterSchema` في `StudentView.vue`) — لا يُسمح بغيرهما (Allowlist).
  - `Page: 1, PageSize: 10`، `Filter: \`${filterType}=*${value}\`` بلا `*` في النهاية.
  - Zod: `StudentSearchSchema` في `src/validations/Person/` مع `validate1`.
- `src/store/Person/useStudentStore.js`: إضافة `studentSearchResults`, `studentSearchLoading`, `studentSearchError`, `studentSearchRequestId` و`searchStudentCandidates(criteria)` و`resetStudentCandidateSearch()` بنمط `usePersonStore` (السطور 111–161). **لا يكتب** على `students` أو `selectedStudent`.
- Hydration في وضع التعديل يبقى على `fetchStudentBy({Param, ParamType: 1})` (تفاصيل بالمعرّف، ليس بحثاً) ويكتب على `selectedStudent` فقط.
- Mapper: التحقق من أن `StudentMapper` يعيد `studentID` و`firstName` و`fatherName` و`lastName` بصيغة camelCase للعرض في الـ Chip.

**4. خطة الـ UI/UX ونمط الاختيار**

- النمط: **GenericSearchByFilter** مع `filterTypes`: `FirstName` (الاسم) و`NationalNumber` (الرقم الوطني)، بنفس قيم `UpsertStudentComp`.
- `id-field="studentID"` و`name-field="displayName"` (أو حقل مكوّن من الاسم الثلاثي كما في `personNameOf`).
- الطالب متعدد الاختيار (`selectedStudents` array): عند الاختيار يُضاف العنصر للقائمة ويُفرَّغ حقل البحث؛ الإزالة بزر "إزالة" الحالي (السطور 26–32) يبقى.
- منع التكرار: إذا كان الطالب موجوداً في `selectedStudents` تُعرض رسالة واضحة ولا يُضاف مرة ثانية (الكود الحالي يتحقق منه في `handleStudent` السطور 198–209).
- Edit Mode: `selectedStudents` تُهيّأ بعنصر واحد من `entity` مع الاسم (`studentName`)، ولا يمكن إضافة طلاب آخرين في وضع التعديل (تقييد صريح بإخفاء البحث).
- حالات: `loading` (زر "جاري البحث")، `error` (من `studentSearchError`)، `no results` (`emptyMessage`)، ومسح الاختيار عند تغيير النص.
- التحقق من `ValidationError :messages="errors?.studentIDs"` مع الحفاظ على المفتاح الحالي المستخدم في الـ Backend.

**5. تطبيق توجيهات المستخدم الخاصة**

- استبدال البحث القديم (`select + input + button` ونداء `fetchStudentBy` للبحث) بـ `GenericSearchByFilter` المرتبط بـ Gridify؛ `fetchStudentBy` يبقى للـ Hydration فقط.

---

### الخدمة 3 — Upsert Component + Views (مركّبة)

**1. العيوب المكتشفة حالياً**

- `UpsertStudentEnrollmentComp.vue`:
  - البحث عن الطالب (السطور 6–16) والإشارة `onSearchStudent` (السطور 156–162) والـ `studentSearchType`/`enStudentSearchType` (السطور 80، 117–119) قديمة.
  - `fillSelectedEnrollment` (السطور 182–197) لا يهيّئ `groupNameValue` ولا `selectedGroupData`، فتظهر خانة المجموعة فارغة في وضع التعديل.
  - `onSubmit` (السطور 141–155) في وضع التعديل يرسل `studentID` من `formData` لكن `selectedStudents` قد تكون فارغة؛ يلزم ضمان المعرّف.
  - `emits` (السطر 132) تعلن `onSearchStudent` و`onSearchGroup` ولا تعلن `select`/`clear` الجديدة.
- `StudentEnrollmentView.vue`:
  - السطور 39–47: لا تمرر `:group-results`.
  - السطور 372–389 (`OnUpsertEnrollment`): تستخدم دوال البحث كـ Hydration.
  - السطور 415–432 (`onSearchStudent`/`onSearchGroup`): تكتب على الـ Stores المشتركة، وتستدعي `fetchGroupBy` بنوع البحث مباشرة.
- `StudentView.vue`:
  - السطور 67–77 و352–364 و358–364: نفس المشكلة؛ `onSearchGroupEnrollment` يكتب على `GroupStore.groups`.
  - السطر 327: `GroupStore.fetchGroups()` بدون فلتر عند فتح التسجيل، يبقي قائمة المجموعات الكاملة في الحالة المشتركة (يُفضَّل إزالته بعد الانتقال للبحث الجديد).
- `GroupView.vue` (السطور 30–35، 170–190، 226): يستخدم نفس المكوّن؛ لم يُفحص سطر أحداث البحث بعد — **يجب التحقق أثناء التنفيذ**.

**2. خطة الـ Backend**

- لا تغيير على Endpoints التسجيل: `POST /StudentEnrollment/NewStudentEnrollment`، `POST /StudentEnrollment/EnrollByRange`، `PUT /StudentEnrollment/UpdateStudentEnrollment`، `GET /StudentEnrollment/GetStudentEnrollmentBy` (تبقى كما هي).
- التحقق من تطابق مفاتيح `studentIDs` و`groupID` و`date` مع `EnrollByRange` و`enrollmentID` مع `UpdateStudentEnrollmentCommand`.

**3. خطة منطق الـ Frontend**

- `StudentEnrollmentView.vue`:
  - تمرير `:group-results` من `GroupStore.groupSearchResults` (بدلاً من `GroupStore.groups`) مع `:loading` و`:error` لكل بحث.
  - إضافة handlers جديدة: `onSearchStudentCandidates(payload)` و`onSearchGroupCandidates(payload)` تستدعي الأكشنات المعزولة من الخدمتين 1 و2، وتمرير `payload` كما هو بعد تحويله (`filterType`, `value`).
  - `OnUpsertEnrollment(e)`: استبدال `onSearchStudent`/`onSearchGroup` بدالتي Hydration منفصلتين: `hydrateStudentForEdit(studentID)` تستدعي `fetchStudentBy({Param, ParamType: 1})`، و`hydrateGroupForEdit(groupID)` تستدعي `GroupStore.fetchGroupBy({Param, ParamType: 0})` (ID فقط).
  - إزالة `fetchGroupBy` بنوع البحث الجديد، وإزالة `fetchGroups()` العامة عند فتح النموذج.
- `StudentView.vue`: تطبيق نفس الاستبدال (`onSearchStudentEnrollment` → `searchStudentCandidates`، `onSearchGroupEnrollment` → `searchGroupCandidates`)، وإزالة `GroupStore.fetchGroups()` في `OnEnrollStudent` (السطر 327).
- `GroupView.vue`: التحقق من أحداث البحث الحالية وتطبيق الاستبدال نفسه إن كانت موجودة.
- Repositories/Services/Stores: لا تعديل على `StudentEnrollmentRepositories.js` ولا `StudentEnrollmentService.js` ولا `StudentEnrollmentStore.js` في هذه الخدمة؛ الحفظ يبقى كما هو.
- تحديث `StudentEnrollmentSchema` فقط إذا كشف التحقق عن اختلاف في أسماء الحقول.

**4. خطة الـ UI/UX ونمط الاختيار**

- `UpsertStudentEnrollmentComp.vue`:
  - الطالب: `GenericSearchByFilter` (متعدد الاختيار عبر قائمة `selectedStudents`، مع منع التكرار) — وفق الخدمة 2.
  - المجموعة: `GenericSearchByFilter` (أو `SearchFilterCombo`) بـ `filterTypes` مطابقة لـ `enGroupSearchType` (0=ID، 1=GroupName، 2=Code)، `v-model` لـ `formData.groupID`، `v-model:name-value` لـ `groupNameValue`، وصيغ `@search`/`@select`/`@clear` — وفق الخدمة 1.
  - عند `@clear` للمجموعة: إخفاء `GroupCard` ومسح `selectedGroupData` و`groupID` معاً.
  - عند `@clear` للطالب: إفراغ `selectedStudents` بالكامل إذا كان في وضع التعديل فقط (تأكيد من المستخدم قبل التنفيذ).
  - Edit Mode: تهيئة `groupNameValue` من `entity.groupName` و`selectedGroupData` من `GroupStore.selectedGroup` (بعد `fetchGroupBy`)، وتهيئة `selectedStudents` من `StudentStore.selectedStudent`.
  - الحفاظ على هوية `Main.css` (`--primary-color`) وعلى RTL، واستخدام `frontend-design-rtl-system` عند التعديل.

**5. تطبيق توجيهات المستخدم الخاصة**

- "استبدال البحث القديم بالبحث الجديد" = كل مسارات `onSearchStudent`/`onSearchGroup` القديمة (وكذلك `select + input + button`) تُستبدل بـ `GenericSearchByFilter` وأكشنات `search*Candidates` المعزولة؛ `fetchStudentBy` و`fetchGroupBy` يبقيان للتحميل بالمعرّف فقط.

---

## ثالثاً: مخاطر ونقاط تحتاج قراراً قبل التنفيذ

1. **تأمين `GetGroupList` و`GetStudentList`:** إضافة Policies تستلزم وجود `Can_Read_Group` و`Can_Read_Student` في `clsPermission` (تغيير إعداد مشترك — يحتاج موافقة).
2. **نطاق البيانات للمعلم والطالب:** `GetStudentList` مفتوح لأي مستخدم مصادق بعد الإضافة؛ تقييد النطاق (`ApplyReadScope`) مطلوب وفق `backend-security-scoping`. هل يُضاف ضمن الخدمة 2 أم يُعالج كخدمة أمنية منفصلة؟
3. **`TeachingAssignment/UpsertTeachingAssignmentComp.vue`:** يحتوي بحثاً قديماً لنفس النمط بالإضافة إلى `alert(data.sessionID)` في `handleSubject` (السطر 193) وهو تصحيح مؤقت يجب إزالته. هل يُضم إلى هذه الخطة كخدمة 4؟
4. **تضارب `ParamType` في `StudentEnrollmentView.onSearchGroup`:** يجب التحقق من قيم `GetGroupBy` قبل الاعتماد على قيم `filterType`.
5. **`GroupStore.fetchGroups` بدون `try/finally`:** لم يُعدَّل في هذه الخطة إلا عند الحاجة لمسار البحث المعزول؛ يُقترح إصلاحه ضمن الخدمة 1 فقط لأن البحث الجديد سيعتمد على أكشن منفصل.
6. **لم يُفحص بعد:** `GroupView.vue` (أحداث البحث)، `GetAllGroupQueryHandler.cs` و`GetAllStudentQueryHandler`/`StudentInfo` للتأكد من `GridifyAsync` وحقول الفلترة، و`src/repositories/Institute/` و`src/Service/Institute/GroupService.js` بالتفصيل. تُفحص قبل بدء كل خدمة.

## رابعاً: التحقق المطلوب بعد كل خدمة

- `dotnet build` لمشروع الـ Backend المتأثر.
- `ESLint` لملفات الواجهة المعدّلة.
- مراجعة `git diff` للتأكد من أن التغيير محصور في الخدمة المنفذة.
- اختبار يدوي: بحث بالاسم/الرقم الوطني/الكود، اختيار، مسح، فشل الشبكة، صفر نتائج، فتح وضع التعديل وعرض الاسم الصحيح قبل الحفظ.

## خامساً: شرط التوقف

بعد مراجعة هذه الخطة، يُنفَّذ **الخدمة رقم (1) فقط** عند إعطاء الإشارة من المستخدم، مع الإجابة عن النقاط 1–4 في القسم الثالث.
