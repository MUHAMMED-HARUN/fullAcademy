# خطة خدمة Country

**الترتيب:** 1 من 9 — أصل التسلسل الجغرافي، وتعتمد عليه City وبقية خيارات العنوان.

## 1. العيوب المكتشفة

- `Controllers/Address/CountryController.cs`: `GetCountryByID` يعيد `Ok()` دون وسيط أو استعلام.
- `src/repositories/Address/country_repository.js`: مسار GET يتضمن backtick حرفياً، ووسيط GET لا يرسل كـ`params`.
- التحديث غير متطابق: Repository يرسل PUT بينما Controller يعرّف `UpdateCountry` كـPOST.
- `country_service.js` يبني حقولاً مثل `countryID/countryName/countryCode` بينما النموذج وSchema يستخدمان `ID/Name/Code`، ولا يوجد `countryMapper.js`.
- مخطط `countrySchema` يستخدم `id` بينما نموذج `UpsertCountryComp.vue` يستخدم `ID`، كما أن رمز الدولة اختياري في Zod لكنه مطلوب في Domain/FluentValidation.

## 2. خطة Backend

- توحيد العقود في `CountryController.cs` لمسارات `GetCountryList`, `GetCountryByID`, `NewCountry`, `UpdateCountry`، ومطابقة HTTP verb مع Repository.
- تحويل `System.Application/Services/Address/Country/Query/GetAllCountryQuery.cs` إلى `BaseFilterQuery<CountryInfo>`.
- إضافة `GetListFilter` إلى `System.Application/Abstractions/Address/ICountryRepository.cs` وتنفيذه في `System.Infrastructure/Repositories/Address/CountryService.cs` باستخدام `GridifyAsync` وAllowlist.
- مراجعة `CountryInfo.cs` و`CountryProfile.cs` و`NewCountryCommandValidator.cs` و`UpdateCountryCommandValidator.cs` و`CountryConfiguration.cs` لتوحيد الحقول والطول والإلزام.
- إبقاء قائمة الدول كاملة فقط لخيار Select إن بقي حجمها صغيراً؛ قائمة الإدارة تكون مرقمة ومفلترة خادمياً.

## 3. خطة منطق Frontend

- إصلاح `src/repositories/Address/country_repository.js` لمسارات GET ووسائطه وفعل التحديث.
- إضافة `src/Mapper/Address/countryMapper.js` لتوحيد `ID/Name/Code` مع `id/name/code`.
- توحيد عقد `countrySchema.js` و`country_service.js` و`useCountryStore.js`، بما في ذلك `TResult` ومفاتيح الأخطاء وحالات loading.
- إضافة paging/filter للائحة الإدارية إن اعتمدت واجهة Gridify لها.

## 4. خطة UI/UX ونمط الاختيار

- تحديث `CountryView.vue`, `CountryList.vue`, `UpsertCountryComp.vue` لتتطابق أسماء الحقول وتعرض loading/error/empty.
- اختيار Country في النماذج هو **Standard Select** لأن القائمة محدودة نسبياً؛ لا يستخدم بحثاً محلياً داخل `GenericSearchByFilter`.

## 5. Gridify والبوابة

- Contains يكتب `Field=*Value` فقط، ويُمنع `Field=*Value*`. AND بالفاصلة `,` وOR بالرمز `|`، مع `/i` عند الحاجة.
- الحقول مسموحة عبر Allowlist، ولا تُمرر أسماء حقول من المستخدم.
- بعد الموافقة تنفذ Country وحدها كخدمة رقم 1؛ لا يبدأ العمل على City قبل مراجعة Country ونجاح `dotnet build` وESLint وموافقة المستخدم.
