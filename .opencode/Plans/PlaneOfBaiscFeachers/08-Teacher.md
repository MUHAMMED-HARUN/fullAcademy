# خطة خدمة Teacher

**الترتيب:** 8 من 9 — تعتمد على Person، ومستقلة عن Student بعد اكتمال Person.

## 1. العيوب المكتشفة

- نمط البحث عن Person في Teacher مناسب مبدئياً، لكن يجب تدقيق الحالة الثلاثية عند المسح والفشل والتحرير.
- `TeacherController` لا يحمل `[Authorize]`، بعكس `StudentController`.
- لا يظهر Allowlist Gridify صريح لقائمة المعلمين.
- `TeacherMapper` يقبل عدة احتمالات casing، ما قد يخفي عدم تطابق العقد.

## 2. خطة Backend

- مراجعة `TeacherController.cs`, `GetAllTeacherQuery.cs`, `TeacherInfo.cs`, `ITeacherRepository`, `TeacherService` وإضافة Allowlist وsoft-delete behavior متسق.
- مراجعة Validators للأوامر والتحقق من وجود Person وعدم تكرار الربط.
- توحيد حماية API مع Student، ومراجعة علاقة الواحد لواحد في `TeacherConfiguration.cs`.

## 3. خطة منطق Frontend

- مواءمة `TeacherRepositories.js`, `TeacherMapper.js`, `TeacherSchema.js`, `TeacherService.js`, `useTeacherStore.js` مع DTO موحد وحقلي `TeacherID/PersonID`.
- معالجة نتائج Paging وأخطاء CRUD، وعدم الاعتماد على casing بدائل كثيرة.

## 4. خطة UI/UX ونمط الاختيار

- مراجعة `TeacherView.vue`, `TeacherList.vue`, `TeacherCard.vue`, `UpsertTeachertComp.vue`.
- Person lookup يبقى **GenericSearchByFilter**. في Edit Mode يُجلب الشخص المرتبط وتُعرض البطاقة، ويؤدي المسح أو فشل البحث إلى تصفير ID والاسم والعنصر المختار.
- توفير loading/error/no-results، وعدم الانتقاء التلقائي لنتيجة وحيدة من صفحة.

## 5. Gridify والبوابة

- Contains = `Field=*Value` بلا نجمة نهائية، AND `,` وOR `|`، وAllowlist لحقول DTO فقط.
- لا استخدام GetList كاملة أو filtering محلي للبحث عن Person.
- يبدأ التنفيذ بعد مراجعة Student؛ لا يبدأ Auth قبل مراجعة Teacher وبناء ناجح وESLint وموافقة المستخدم.
