# خطة خدمة Address

**الترتيب:** 5 من 9 — يجمع Country/City/District/Neighborhood، ويُستخدم داخل Person.

## 1. العيوب المكتشفة

- `address_repositories.js` يستدعي `GetAddressList`، لكن `AddressController` لا يعرّف هذا endpoint.
- `AddressController.UpdateAddress` معلن POST بينما Repository يستخدم PUT.
- تنفيذ المستودع اسمه `enericService` في `System.Infrastructure/Repositories/Address/AddressService.cs`.
- `AddressService.GetBy` و`GetEntityBy` يقرآن `AddressID.Value` دون ضمان وجود قيمة أو سجل.
- توجد مكونات Upsert متداخلة/قديمة، وPersonView يحمل كل القوائم الجغرافية ثم يفلتر محلياً.

## 2. خطة Backend

- تثبيت العقود الضرورية: `NewAddress`, `UpdateAddress`, `GetAddressBy` بحسب `Param/ParamType` (ID أو PersonID).
- توحيد Update HTTP verb بين `AddressController` وRepository، ومعالجة عدم وجود عنوان بإرجاع NotFound/نتيجة فشل واضحة بدلاً من nullable dereference.
- مراجعة `IAddressRepository`, `AddressService`, `AddressInfo.cs`, `AddressProfile.cs`, `NewAddressCommandValidator.cs`, `UpdateAddressCommandValidator.cs`, `AddressConfiguration.cs`.
- لا تُضاف قائمة Address كاملة ما لم توجد شاشة قائمة مستقلة؛ تُزال النداءات غير المستخدمة أو ينشأ endpoint عند ثبوت الحاجة فقط.

## 3. خطة منطق Frontend

- إزالة/تصحيح `GetAddressList` غير المطابق في `address_repositories.js`, `address_service.js`, `useAddreessStore.js`.
- توحيد حقول `AddressID/CountryID/CityID/DistrictID/NeighborhoodID/Notes` عبر `addressMapper.js` و`addressSchema.js`.
- توحيد `TResult` وأخطاء create/update/get، وعدم تغيير عقد Controller بمعزل عن Repository.

## 4. خطة UI/UX ونمط الاختيار

- توحيد `UpsertAddressComp.vue` و`upsertAddressItem.vue` في مكوّن مسؤول واحد.
- العنوان يستخدم **Nested Combo** Country→City→District→Neighborhood، مع تصفير معرّفات الأبناء عند تغيير الأب، وحالات loading/error/empty لكل مستوى.
- Place of birth يستخدم Country→City. تحرير Person يستعيد العنوان والـIDs والlabels من DTO كامل قبل تمكين الحفظ.

## 5. Gridify والبوابة

- لا يُحمّل GenericSearch من GetList كاملة ولا يُفلتر محلياً. Lookup كبير يُطلب server-side مع Parent ID وPageSize محدود.
- أي Contains في Gridify هو `Field=*Value` بلا `*` في النهاية؛ AND `,` وOR `|`، و`/i` عند الحاجة.
- التنفيذ لا يبدأ قبل إكمال Neighborhood ومراجعته؛ الخدمة الحالية Address لا تحتاج قائمة Gridify عامة ما لم يتأكد وجود شاشة قائمة.
