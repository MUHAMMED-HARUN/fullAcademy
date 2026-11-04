# خطة خدمة Student

**الترتيب:** 7 من 9 — تعتمد على Person من خلال `PersonID`.

## 1. العيوب المكتشفة

- `StudentView.vue` يستخدم GenericSearch لربط Person عبر Store/Service وطلب محدود، وهو النمط الصحيح مبدئياً.
- يلزم التحقق من تزامن `personID` واسم الشخص و`selectedPerson/Card` عند المسح والفشل وتبديل النص.
- `StudentController` محمي بـ`[Authorize]`، بينما Person وTeacher ليسا محميين حالياً.
- لا يظهر Allowlist صريح لفلاتر Gridify في مستودع الطلاب.

## 2. خطة Backend

- مراجعة `StudentController.cs`, `GetAllStudentQuery.cs`, `StudentInfo.cs`, `IStudentRepository`, `StudentService` لعقود القائمة والبحث والحذف.
- إضافة Allowlist لحقول Projection، وتوحيد soft-delete behavior، والتحقق من وجود Person وعدم ربطه بطالب آخر.
- مراجعة Validators أو إضافتها ضمن `System.Application/Services/Person/Student/Validations/`، ومراجعة علاقة الواحد لواحد في `StudentConfiguration.cs`.

## 3. خطة منطق Frontend

- مواءمة `StudentRepositories.js`, `StudentMapper.js`, `StudentSchema.js`, `StudentService.js`, `useStudentStore.js` مع `StudentID/PersonID` وDTO الفعلي.
- مراجعة موضع DELETE body، ومعالجة Paging/Count، وربط lookup Person بـ`PersonService.searchForCandidates` دون تحميل القائمة.

## 4. خطة UI/UX ونمط الاختيار

- مراجعة `StudentView.vue`, `StudentList.vue`, `StudentCard.vue`, `UpsertStudentComp.vue`.
- Person يبقى **GenericSearchByFilter** مع ID + label + object متزامنة. Edit Mode يجلب سجل الطالب والشخص المرتبط، ويعرض البطاقة قبل الحفظ.
- إظهار loading/error/no-results؛ لا اختيار تلقائياً لأن نتيجة الصفحة واحدة فقط.

## 5. Gridify والبوابة

- Contains = `Field=*Value` دون نجمة ختامية؛ AND `,` وOR `|`، مع Allowlist وPageSize محدودين.
- لا GetList كاملة أو client-side filtering لتغذية GenericSearch.
- التنفيذ بعد Person ومراجعته؛ يوقف العمل بعد Student حتى مراجعة الخدمة وموافقة المستخدم قبل Teacher.
