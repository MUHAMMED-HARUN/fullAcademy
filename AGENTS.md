# School_Academy — قواعد الوكيل وفهرس المهارات

## مسارات المشاريع

- **Root & WebApi:** `D:\Project\Real Proj\School_Academy`
- **Domain:** `D:\Project\Real Proj\School_Academy\System.Domain`
- **Application:** `D:\Project\Real Proj\School_Academy\System.Application`
- **Infrastructure:** `D:\Project\Real Proj\School_Academy\System.Infrastructure`
- **Frontend:** `D:\Project\Real Proj\School_Academy\School_Academy_Front\sys_Front\template-proj`

## استخدام المهارات قبل التنفيذ

قبل كتابة أو تعديل أي كود، حدّد الطبقة والميزة، ثم حمّل المهارة أو المهارات المناسبة من `.opencode/skills/` بواسطة أداة `skill`، واقرأ الملفات والعقود القائمة ذات الصلة. إذا امتد العمل عبر طبقات، حمّل مهارات كل طبقة متأثرة، ولا تعتمد على مهارة واحدة لتغيير عقد كامل.

### فهرس المهارات

**Backend**

- `backend-domain-modeling` — Domain entities وEnums والأحداث والاستثناءات.
- `backend-infrastructure-persistence` — EF Core و`AppDbContext` وRepositories وDI وUnit of Work.
- `backend-cqrs-feature` — عقود المستودعات وCQRS وDTO وAutoMapper وFluentValidation.
- `backend-webapi-contracts` — Controllers وRoutes وHTTP contracts وSwagger وauthorization.

**Frontend: المنطق والهيكلية**

- `frontend-api-repositories` — Axios client وRepositories وعقود endpoints.
- `frontend-feature-data-flow` — Mapper وService وPinia Options Store.
- `frontend-validation-contracts` — Zod و`validate1` وتطابق قواعد Backend.
- `frontend-routing-auth` — Vue Router وFeature links وAuth Store والحماية الأمامية.

**Frontend: UI والتصميم**

- `frontend-selection-patterns` — اختيار Standard Select أو Cascading Combo أو Gridify GenericSearch وربط الحالة الثلاثية.
- `frontend-vue-feature-ui` — Views ومكونات الميزة باستخدام `<script setup>`.
- `frontend-design-rtl-system` — `Main.css` وTailwind والهوية والتجاوب وRTL والنماذج.

## قواعد معمارية إلزامية

- لا تنسخ أخطاء تاريخية. طابق دائماً مسار الملف مع namespace، والـ Controller route مع Repository URL، وأسماء DTO/Command/Mapper/schema والـ payload على الطرفين.
- قبل تغيير عقد API، تتبع البيانات بين Controller وApplication وRepository في Backend، ثم Mapper وService وRepository وView في Frontend. حدّث الطرفين المتأثرين فقط إذا كان ذلك ضمن طلب المستخدم.
- معيار Domain: أسماء الكيانات تبدأ بـ `cls` وترث `clsBaseClass`، والـ Enums تبدأ بـ `en`.
- معيار Infrastructure: استخدم `AppDbContext`، وعقد `I*Repository` في Application، وتنفيذ Repository باسم `*Service` في Infrastructure، وسجّله في `InfrastructureServiceRegistration`.
- اسم مجلد التحقق في خدمات Backend الجديدة هو `Validations` دائماً. أعد `TResult<T>`، واستخدم علامات `ICommand`/`IQuery`/`ITransaction` وفق `TransactionBehavior`.
- معيار تدفق Frontend هو `View → Pinia Store → Service → Repository (clsUtil.FeachRequset) → Axios`. استخدم Mapper للتحويل بين camelCase وPascalCase، وZod مع `validate1` في Service.
- المكونات الجديدة تستخدم Vue 3 `<script setup>`؛ حافظ على Pinia Options Store.
- قبل بناء أي Selector، حمّل `frontend-selection-patterns`. يمنع تغذية GenericSearch من `GetList()` كاملة أو فلترة النتائج محلياً؛ يجب أن يأتي Lookup الكبير من Gridify server-side، وأن تبقى ID والاسم والعنصر المختار/البطاقة متزامنة.
- مرجع الهوية البصرية هو متغيرات `src/css/Main.css` (ومنها `#1e3a8a` وCairo وOutfit). استخدم Tailwind للتخطيط والتجاوب دون استبدال الهوية أو خلق تعارض معها، وحافظ على RTL.
- لا تعدّل ملف كود أو إعداد خارج نطاق طلب المستخدم، ولا تنفذ إعادة هيكلة عامة أثناء إضافة ميزة. اسأل قبل التغيير العقدي أو المهاجرات أو تغيير إعدادات مشتركة غير مطلوبة.
- بعد التنفيذ افحص diff للتأكد أن التغيير محصور، وطابق العقود، وشغّل الاختبارات أو البناء المناسب عند توفره. اذكر ما لم يتم اختباره وأي تناقض قائم لم يُعالج.
