# خطة خدمة City

**الترتيب:** 2 من 9 — تعتمد على Country وتُستخدم في بيانات الميلاد والعنوان.

## 1. العيوب المكتشفة

- `Controllers/Address/CityController.cs`: `GetCityByID` يعيد `Ok()` فقط ولا يستقبل معرّفاً.
- `src/repositories/Address/city_repositories.js`: مسار GetBy يتضمن backtick حرفياً ووسيط GET غير موضوع في `params`.
- `CityView.vue` يحمل القائمة كاملة ويستخدم `applyClientQuery` للفلترة والترقيم محلياً.
- `UpsertCityComp.vue` يجلب جميع الدول ثم ينفذ `Array.filter` محلياً لتغذية البحث.
- `citySchema.js` يعرّف `cityCode` بينما نموذج الإدخال و`cityMapper.js` يستخدمان `code`.
- Validator إنشاء المدينة موجود باسم `NewCountryCommandValidator.cs` ومجلد `Validation`.

## 2. خطة Backend

- تحويل `GetAllCityQuery.cs` إلى `BaseFilterQuery<CityInfo>` وإضافة `GetListFilter` في `ICityRepository` و`CityService` باستخدام `GridifyAsync` وAllowlist.
- تفعيل `GetCityByID` في `CityController` بوسيط متفق عليه، وإتاحة استعلام المدن حسب `CountryID` (فلتر query أو endpoint واضح).
- مراجعة `CityInfo.cs`, `CityProfile.cs`, `NewCityCommandValidator.cs`, `UpdateCityCommandValidator.cs`, `CityConfiguration.cs` وتوحيد قواعد `CityCode` الاختيارية مع Domain/EF.

## 3. خطة منطق Frontend

- إصلاح `city_repositories.js`، وتوحيد `code/cityCode` في `cityMapper.js`, `citySchema.js`, `city_service.js`.
- تحديث `useCityStore.js` لدعم Gridify والصفحات وطلبات المدن المقيدة بـ`CountryID` بدلاً من تحميل الكل.
- فصل DTO القائمة عن DTO الاختيار فقط عند الحاجة، والمحافظة على casing موحد.

## 4. خطة UI/UX ونمط الاختيار

- تحويل `CityView.vue` إلى قائمة server-filtered ذات pagination وحالات تحميل/خطأ.
- Country في نموذج المدينة **Standard Select**. تسلسل Country→City في نماذج العنوان **Nested Combo**، ويُمسح `CityID` عند تغيير الدولة.
- في Edit Mode تهيئة Country والمدينة المختارة من البيانات المحملة قبل إتاحة الحفظ.

## 5. Gridify والبوابة

- Contains هو `Field=*Value` بلا نجمة نهائية؛ AND `,` وOR `|`، و`/i` عند الحاجة.
- لا `GetList()` كاملة ولا فلترة محلية لتغذية lookup كبير؛ استخدام Allowlist وPage/PageSize محدودين.
- لا يبدأ التنفيذ قبل اكتمال Country ومراجعتها. تنفذ City وحدها في مرحلتها، ثم build وESLint ومراجعة المستخدم قبل District.
