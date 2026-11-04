---
name: backend-webapi-contracts
description: Add School_Academy ASP.NET Core controllers and stable API contracts with IMediator, matching routes, payloads, authorization, serialization, and Swagger to the frontend.
---

# عقود Web API

استخدم هذه المهارة عند إضافة Controller أو تعديل endpoint أو عقد HTTP في مشروع WebApi الجذري.

## قواعد إلزامية

- استخدم مهارة `backend-security-scoping` إلزامياً لكل ميزة أو endpoint جديد أو معدل لتحديد claims والصلاحيات وسياسات `Authorize` المقابلة للعمليات.
- راجع Controller وCommand/Query وFrontend Repository المقابلين قبل تغيير أي endpoint.
- ضع Controller في `Controllers/{Feature}`، واجعل namespace مطابقاً لمسار المجلد (`School_Academy.Controllers.{Feature}`). لا تكرر اختلاف Progress/Institute الموجود في بعض الملفات.
- استخدم `IMediator` لإرسال Commands وQueries إلى Application؛ لا تضع منطق المجال أو الاستعلام المباشر في Controller.
- عرّف route وHTTP verb واسماء/أنواع الحقول بوضوح وثبات. طابقها حرفياً مع Repository والـ Mapper، بما يشمل الأحرف الكبيرة والصغيرة ومعرّفات مثل `StageID`، وتجنب أخطاء الإملاء أو route غير متطابق.
- حدّد مصدر الطلب صراحةً (`[FromBody]` أو `[FromQuery]`) وراجع serializer الفعلي. `Program.cs` يضبط `PropertyNamingPolicy = null`، ونتيجة `TResult<T>` تتضمن `Value` و`IsSuccess` و`Error` بأسماء PascalCase.
- حافظ على شكل نجاح/فشل موحد ومتوافق مع مساعد Axios/`clsUtil.FeachRequset`. لا تعرض Entity داخلياً بدلاً من DTO أو تغيّر شكل الأخطاء من طرف واحد.
- افحص `Program.cs` قبل الاعتماد على المصادقة أو authorization policy أو CORS أو middleware. وجود `GlobalExceptionMiddleware.cs` وحده لا يعني أنه مسجل في request pipeline.
- أضف endpoint إلى Swagger من خلال إعدادات المشروع القائمة، ولا تضف حزمة توثيق موازية.
- لأي بحث GenericSearch عن كيان كبير، استقبل Query مفلتر عبر `[FromQuery]` وأعد `TResult<Paging<LookupDto>>` من Endpoint `GetListFilter`، مع تطابق كامل لأسماء Gridify fields وIDs وlabels مع Repository/Mapper. لا تعرض Endpoint غير مرقم بديلاً عن فلترة الخادم.
- وفّر `GetById` أو `get-for-upsert/{id}` لإعادة تهيئة ID والـ label والعلاقات في Edit Mode؛ لا تعتمد على نتائج بحث سابقة أو payload من العميل لإعادة بناء العلاقات.
- مهام الواجهة لا تثبت صلاحية المستخدم؛ تحقق من authorization في WebApi، ولا تعتمد على Router guard وحده.

## سير العمل

1. تتبع endpoint من Controller إلى الطلب وDTO ثم Repository، ومن Frontend Repository إلى الـ View.
2. ثبت verb وroute ومصادر الـ binding وشكل الاستجابة والأخطاء.
3. أضف Controller action بأقل مسؤولية ممكنة، ثم اختبر route/payload مع العميل المقابل أو وثّق ما لم يُختبر.

## التحقق قبل الإنهاء

- لا route مكرراً أو غير مطابق للواجهة، ولا حقل payload مختلف بين الطرفين.
- الـ Controller يمرر إلى MediatR، والـ authorization ومعالجة الأخطاء تستند إلى التسجيل الفعلي.
- أي تعديل عقدي موثق ومصحوب بتحديث الطرفين ضمن النطاق المصرح.
