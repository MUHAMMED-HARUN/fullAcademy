# الخدمة 1 — Institute: اقتراحات العنوان في Upsert

## العيوب الحالية

- في `views/Institute/InstituteView.vue` تُحمّل `CityStore` و`DistrictStore` و`NeighborhoodStore` قوائم كاملة، ثم تُمرر إلى `UpsertInstituteComp.vue`.
- تمرير القوائم يجعل `upsertAddressItem.vue` يدخل وضع `legacyOptionsMode` ويعرض قوائم `<select>` بدلاً من البحث الخادمي المستخدم في نموذج الشخص.
- بحث العناوين في `city_service.js` و`District_service.js` و`neighborhood_service.js` يطلب 10 نتائج ويتطلب نص بحث؛ لا يدعم اقتراحات أولية فارغة من خمس نتائج.
- `GenericSearchByFilter.vue` لا يفتح الاقتراحات إلا بعد بحث صريح؛ يلزم دعم اختياري للاقتراحات الأولية من دون تغيير سلوك مستهلكيه الآخرين.
- وضع التعديل يجلب العنوان بواسطة `AddressStore.fetchAddressBy`؛ يجب الحفاظ على تهيئة المعرّفات والأسماء عند فتح النموذج.

## خطة Backend

- لا يظهر احتياج مؤكد إلى Endpoint جديد؛ يمكن إعادة استخدام:
  - `GET /City/GetCityList` مع `CountryID`.
  - `GET /District/GetDistrictListByCityID/{cityId}`.
  - `GET /Neighborhood/GetNeighborhoodListByDistrictID/{districtId}`.
- ضبط `Page=1`, `PageSize=5` وإضافة شرط الاسم عند البحث فقط. للفلاتر النصية تستخدم صيغة Gridify مثل `Name=*قيمة`، بلا `*` في النهاية، مع قيد الأب دائماً.
- التحقق أثناء التنفيذ من كفاية `GridifyAsync` وخرائط الحقول الحالية؛ لا تغيير في Domain أو قاعدة البيانات متوقع لهذه الخطوة.

## خطة منطق Frontend

- تعديل `InstituteView.vue` لإيقاف تحميل قوائم المدن والمناطق والشوارع كاملة، وتمرير حالات النتائج والتحميل والخطأ إلى العنوان.
- إضافة مسار اقتراحات في `city_service.js` و`District_service.js` و`neighborhood_service.js` يستخدم الصفحات المحدودة وقيود الأب، وإتاحة البحث النصي مع قيود Zod الحالية أو تحقق مناسب للطلب الفارغ.
- تمرير تدفق البحث عبر المخازن والمستودعات القائمة: `useCityStore.js`, `useDistrictStore.js`, `useNeighborhoodStore.js`، ثم `city_repositories.js`, `district_repositories.js`, و`neighborhood_repositories.js`.
- حماية الطلبات من الاستجابات القديمة، ومسح نتائج وقيم الأبناء عند تغيير الأب، كما في تدفق `PersonView.vue`.

## خطة UI/UX ونمط الاختيار

- الدولة: `Standard Select`.
- المدينة ثم المنطقة ثم الشارع: اختيارات متدرجة مع بحث خادمي؛ عند اختيار دولة تُعرض أول خمس مدن المطابقة، وعند اختيار مدينة أول خمس مناطق تابعة، وعند اختيار منطقة أول خمسة شوارع تابعة. يظل البحث النصي محصوراً بالأب المختار.
- تطبيق ذلك في `upsertAddressItem.vue` مع خيار اقتراحات أولية صريح في `GenericSearchByFilter.vue`، وعدم عرض اقتراح قبل تحقق شرط الأب.
- في Edit Mode تُهيّأ `countryID`, `cityID`, `districtID`, `neighborhoodID` والأسماء من العنوان المحمّل قبل إتاحة الحفظ. المسح أو تغيير الأب يمسح المعرفات والأسماء والاختيار التابع.
- عرض حالات التحميل والخطأ وعدم وجود نتائج، وعدم اللجوء إلى `GetList()` أو التصفية المحلية للـ Lookup.

## تطبيق التوجيهات والأمان

- يقتصر طلب الاقتراحات على خمسة سجلات، مشروطة بالأب، مع اعتماد نمط العنوان المستخدم في Person.
- حالياً لا تظهر سياسات صلاحيات لهذه المسارات في `clsPermission.cs`، كما أن `InstituteController` وControllers العناوين لا تعرض `[Authorize]`. عند التنفيذ يلزم تضمين صلاحية القراءة المناسبة لحقول العنوان وسياسات Controller، مع مراجعة `Program.cs` و`GetAllPermissionHandler.cs`؛ لا تُعامل حماية الواجهة كبديل لتفويض الخادم.
- يبقى `InstituteSchema.js` وشكل حمولة العنوان دون تغيير ما لم يكشف التحقق من عقد API عن تعارض فعلي.
