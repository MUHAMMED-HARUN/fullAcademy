# خطة خدمة District

**الترتيب:** 3 من 9 — تعتمد على City عبر `CityID`.

## 1. العيوب المكتشفة

- `DistrictController.GetDistrictListByCityID/{cityId}` و`GetDistrictByID/{param}` يعيدان `Ok()` دون استعلام.
- `DistrictView.vue` يفلتر ويرقم محلياً بعد جلب القائمة كاملة.
- `UpsertDistrictComp.vue` يجلب المدن كاملة ثم يستخدم `Array.filter` محلياً للبحث.
- `districtMapper.js` يقرأ `DistrictID` في DTO الفردي و`ID` في DTO القائمة؛ الشكل غير موحد.
- ملفات التحقق تستخدم مجلد `Validation` بدلاً من معيار `Validations`.

## 2. خطة Backend

- تحويل `GetAllDistrictQuery.cs` إلى `BaseFilterQuery<DistrictInfo>` وإضافة `GetListFilter` في `IDistrictRepository` و`DistrictService` مع Allowlist و`GridifyAsync`.
- تنفيذ `GetDistrictListByCityID/{cityId}` والتحقق من GUID، وتفعيل `GetDistrictByID/{param}`.
- مراجعة `DistrictInfo.cs`, `DistrictProfile.cs`, `NewDistrictCommandValidator.cs`, `UpdateDistrictCommandValidator.cs`, `DistrictConfiguration.cs` لضمان تطابق DTO والعلاقة والقيود.
- الحفاظ على الفلترة الافتراضية وعدم استخدام `IgnoreQueryFilters()`.

## 3. خطة منطق Frontend

- توحيد `ID/DistrictID` داخل `districtMapper.js`، وتصحيح المسارات والـparams في `district_repositories.js`.
- تحديث `District_service.js` و`useDistrictStore.js` لتمرير `CityID` وGridify paging/filter إلى الخادم.
- تثبيت أسماء حالة الاختيار وأفعال Store، ومعالجة أخطاء وفشل lookup دون الاحتفاظ بنتيجة قديمة.

## 4. خطة UI/UX ونمط الاختيار

- تحويل `DistrictView.vue` إلى server-side filter/paging.
- اختيار المدينة يكون **Nested Combo** يبدأ بالدولة ثم المدينة عند الحاجة؛ تغيير الدولة يصفر المدينة.
- تحميل اختيار Edit Mode من DTO موثوق يحتوي City ID والاسم، وإظهار loading/error/empty.

## 5. Gridify والبوابة

- Contains: `Field=*Value` حصراً، دون نجمة ختامية؛ AND `,`، OR `|`، و`/i` عند الحاجة.
- منع تغذية GenericSearch من القائمة كاملة أو تصفيتها محلياً؛ Allowlist وPageSize محدودان.
- يبدأ التنفيذ بعد مراجعة City بنجاح `dotnet build` وESLint وموافقة المستخدم.
