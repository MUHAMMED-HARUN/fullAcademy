# خطة خدمة Person

**الترتيب:** 6 من 9 — تعتمد على Address وCity؛ وهي مصدر الأشخاص المرتبطين بـStudent وTeacher.

## 1. العيوب المكتشفة

- يوجد endpoint `get-for-upsert/{id:guid}` و`PersonForUpsertDto` يدعمان استعادة IDs العنوان، وهي نقطة قائمة يجب الحفاظ عليها.
- `PersonService.GetListFilter` لا يستبعد المحذوفين كما يفعل GetList، ولا يظهر إعداد Allowlist صريح لـGridify.
- `PersonView.vue` يحمل كل المدن والمناطق والأحياء ثم يفلتر محلياً.
- Domain يستخدم `Gendor` بينما واجهة Vue تستخدم `gender`؛ يلزم تثبيت العقد وعدم تغيير الاسم ضمنياً.
- `PersonController` لا يحمل `[Authorize]`.

## 2. خطة Backend

- مراجعة `PersonController.cs`, `NewPersonCommand.cs`, `UpdatePersonCommand.cs`, `PersonInfo.cs`, `PersonForUpsertDto.cs` والعقود الحالية.
- توحيد `GetAllPersonQuery` و`IPersonRepository` و`PersonService.GetListFilter` حول Projection وGridify allowlist، مع سياسة soft delete متسقة.
- مراجعة `PersonProfile.cs` و`NewPersonCommandValidator.cs` و`UpdatePersonCommandValidator.cs` لقواعد الأسماء والتاريخ وGUID والعنوان، ومطابقتها مع Domain وEF.
- حماية endpoints في WebApi؛ حماية Vue وحدها لا تكفي.

## 3. خطة منطق Frontend

- توحيد `PersonRepositories.js`, `PersonMapper.js`, `PersonSchema.js`, `PersonService.js`, `usePersonStore.js` بين list DTO وupsert DTO.
- مطابقة Zod مع FluentValidation وDomain في الحقول المطلوبة والطول وGUID والتاريخ وملاحظات العنوان.
- إبقاء مرشحي الشخص محصورين بـ`FirstName` و`NationalNumber`، وطلب صفحات محدودة، والتحقق من قيم البحث التي تحتوي رموز Gridify.

## 4. خطة UI/UX ونمط الاختيار

- مراجعة `PersonView.vue`, `PersonList.vue`, `PersonCard.vue`, `upsertPersonAddress.vue`، وتوحيد casing للأحداث وحالات التحميل والأخطاء.
- الجنس **Standard Select**. الميلاد والعنوان **Nested Combo**. Edit Mode يعيد تهيئة IDs والlabels لكل مستوى من DTO، لا من قائمة كاملة.
- عند تغيير Country/City/District تُمسح قيم الأبناء والبطاقة التابعة فوراً.

## 5. Gridify والبوابة

- Contains يكتب `Field=*Value` فقط بلا نجمة نهائية. AND `,`، OR `|`، وCase-insensitive بإضافة `/i` عند الحاجة.
- Allowlist صريحة، وعدم استخدام قائمة كاملة ثم فلترة محلية في lookup.
- Person لا يبدأ قبل مراجعة Address. ينفذ منفرداً، ثم `dotnet build` وESLint ومراجعة المستخدم قبل Student.
