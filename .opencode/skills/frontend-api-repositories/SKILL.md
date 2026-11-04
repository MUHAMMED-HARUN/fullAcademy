---
name: frontend-api-repositories
description: Create feature API repositories in the Vue frontend using clsUtil.FeachRequset and keep HTTP verbs, routes, query parameters, payload fields, and responses aligned with WebApi controllers.
---

# API Repositories في Frontend

استخدم هذه المهارة عند إضافة أو تعديل وصول ميزة إلى Backend داخل `src/repositories/{Feature}`.

لقواعد اختيار النمط اقرأ `frontend-selection-patterns`؛ Repository الخاص بـ GenericSearch يجب أن يستدعي Endpoint Gridify مفلتر/مرقّم، لا Endpoint قائمة كاملة.

## قواعد إلزامية

- اقرأ Controller وCommands/Queries المقابلة قبل كتابة Repository. لا تستنتج أسماء المسارات أو حقول payload من أسماء الواجهة وحدها.
- استخدم Axios عبر `src/Generic/clsUtil.js` و`src/api/axios.js`؛ استدعِ `clsUtil.FeachRequset` مع `enApiType` المناسبة، ولا تتصل بـ Axios مباشرة من View أو Store أو Service.
- اتبع شكل repository القائم: class للميزة وdefault singleton export، مع أسماء methods متسقة مع Service المقابل.
- طابق `baseUrl` والـ route وHTTP verb حرفياً مع `[Route]` و`[Http...]` في Controller. دقق في spelling وحالة الأحرف وأسماء مثل `IsExist...`.
- طابق كل payload والـ query parameter مع DTO/Command/Query على الخادم. لا تستخدم معرّفاً يخص ميزة أخرى؛ مثال: حذف Stage يحتاج `StageID` لا `EnrollmentID`.
- استخدم صيغة Axios المناسبة: query params ضمن config لطلبات GET، وراجع سلوك `clsUtil` لإرسال جسم DELETE (`{ data: payload }`). لا تمرر config كأنه جسم الطلب.
- أعد نتيجة المساعد كما هي إلى Service؛ المساعد يحول الاستجابة إلى `TResult` ويعالج أخطاء `errors` و`Error`. لا تخترع response envelope جديداً داخل كل Repository.
- لا تغيّر `FeachRequset` أو عنوان الخادم أو بنية الاستجابة العامة ضمن إضافة feature عادية؛ التغييرات المشتركة تحتاج نطاقاً وموافقة صريحة.
- يحظر استخدام `GetList()` ثم `Array.filter` في `Service` أو `View` لتغذية `GenericSearchByFilter`. مرّر `Page` و`PageSize` وfilter مبنياً من حقول مسموحة إلى Endpoint `GetListFilter`، وتحقق من DTO الذي يحتوي ID واسم العرض. لتهيئة Edit Mode استخدم `GetById`/`get-for-upsert` عند الحاجة بدلاً من تحميل القائمة.

## قواعد صياغة فلاتر Gridify الإلزامية (Gridify Filtering Syntax)

عند بناء نص `Filter` لـ Gridify، استخدم المعاملات القياسية التالية فقط:

- **Contains (يحتوي على):** `FieldName=*Value`؛ يُمنع منعاً باتاً وضع `*` في نهاية القيمة مثل `FieldName=*Value*`.
- **StartsWith (يبدأ بـ):** `FieldName^Value`.
- **EndsWith (ينتهي بـ):** `FieldName$Value`.
- **Equals (يساوي):** `FieldName=Value`.
- **Not Equals (لا يساوي):** `FieldName!=Value`.
- **Case-Insensitive (تجاهل حالة الأحرف عند الحاجة):** أضف `/i` في النهاية، مثل `FieldName=*Value/i`.
- **الدمج المنطقي:** استخدم `,` للربط بـ (AND) و`|` للربط بـ (OR).

## سير العمل

1. دوّن route والـ verb ومصدر كل parameter ونموذج الطلب والنجاح والفشل من Backend.
2. أنشئ Repository في مجلد الميزة الصحيح، وطبّق methods ذات الأسماء الواضحة والمتسقة.
3. قارن كل call بـ Controller، ثم تحقق أن Service وMapper يستخدمان الحقول نفسها.

## التحقق قبل الإنهاء

- كل route والـ verb والـ payload متطابق مع endpoint، لا مجرد route مشابه.
- GET params وDELETE body يمرران في الموضع الصحيح، والـ Repository لا يحتوي منطق UI أو مجال.
