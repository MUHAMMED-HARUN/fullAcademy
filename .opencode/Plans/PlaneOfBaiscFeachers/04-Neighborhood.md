# خطة خدمة Neighborhood

**الترتيب:** 4 من 9 — تعتمد على District عبر `DistrictID`.

## 1. العيوب المكتشفة

- `NeighborhoodController.GetNeighborhoodByID/{param}` لا ينفذ lookup، ولا يوجد حالياً endpoint لقائمة الأحياء/الشوارع حسب `DistrictID`.
- `NeighborhoodView.vue` يفلتر ويرقم محلياً على قائمة كاملة.
- `UpsertNeighborhoodComp.vue` يجلب جميع المناطق ثم يستخدم `Array.filter` لتغذية lookup.
- `neighborhoodMapper.js` يتعامل مع `NeighborhoodID` في مسار و`ID` في مسار آخر.
- `NeighborhoodProfile.cs` يربط `UpdateNeighborhoodCommand` بـ`clsDistrict` بدلاً من `clsNeighborhood`.

## 2. خطة Backend

- تحويل `GetAllNeighborhoodQuery.cs` إلى `BaseFilterQuery<NeighborhoodInfo>`، وإضافة `GetListFilter` في `INeighborhoodRepository` و`NeighborhoodService` عبر `GridifyAsync` وAllowlist.
- تفعيل GetBy ID وإضافة lookup حسب District ID بعقد واضح.
- مراجعة `NeighborhoodInfo.cs`, `NeighborhoodProfile.cs`, `NewNeighborhoodCommandValidator.cs`, `UpdateNeighborhoodCommandValidator.cs`, `NeighborhoodConfiguration.cs`.
- إعادة التحقق من قواعد التحقق وتسميات مجلدات Validators وفق `Validations`.

## 3. خطة منطق Frontend

- توحيد DTO في `neighborhoodMapper.js` وإصلاح المسارات ووسائط GET في `neighborhood_repositories.js`.
- تحديث `neighborhood_service.js` و`useNeighborhoodStore.js` لاستعلامات Gridify والفلترة حسب District، وإزالة جلب القائمة الكاملة للبحث.

## 4. خطة UI/UX ونمط الاختيار

- `NeighborhoodView.vue` يستخدم قائمة مرقمة ومفلترة خادمياً.
- اختيار الشارع/الحي **Nested Combo**: Country→City→District→Neighborhood، أو endpoint بحث خادمي مرتبط بالأب. تغيير أي أب يصفر الأبناء.
- Edit Mode يهيئ المعرفات والأسماء لكل مستوى قبل الحفظ، مع حالات loading/error/no results.

## 5. Gridify والبوابة

- Contains `Field=*Value` دون نجمة ختامية؛ AND `,` وOR `|`؛ `/i` عند الحاجة.
- الحقول Allowlist، ولا يُستخدم GetList ثم client-side filtering للـlookup.
- يبدأ التنفيذ بعد اكتمال ومراجعة District، ثم يتوقف قبل Address إلى حين موافقة المستخدم.
