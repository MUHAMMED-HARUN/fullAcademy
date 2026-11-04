---
name: backend-cqrs-feature
description: Build a complete School_Academy Application feature slice with repository contracts, MediatR commands and queries, handlers, DTOs, AutoMapper profiles, and FluentValidation.
---

# شريحة CQRS في Application

استخدم هذه المهارة لبناء أو تعديل ميزة داخل `System.Application`، بما يشمل عقد Repository وطلبات MediatR ومعالجاتها والتحقق والخرائط.

## قواعد إلزامية

- استخدم مهارة `backend-security-scoping` إلزامياً عند التخطيط أو تنفيذ أو تحسين أي ميزة، لتعريف صلاحياتها ونطاق ملكية بياناتها بالتنسيق مع الطبقات المتأثرة.
- افحص مجلد الميزة الأقرب، وعقد repository والتنفيذ في Infrastructure قبل تحديد الطلبات. أسماء الملفات والـ namespaces يجب أن تطابق المسار الفعلي للميزة والخدمة.
- ضع الطلبات والـ Handlers والـ DTO والـ Mapper والتحقق ضمن `Services/{Feature}/{Service}/`، وسمِّ مجلد التحقق **`Validations` دائماً**. لا تستخدم `Validation` أو `Validator` أو تهجئات بديلة.
- عرّف عقد المستودع باسم `I*Repository` داخل `Abstractions/{Feature}`، واجعل التنفيذ Infrastructure من نوع `*Service`. نسّق العقد مع مهارة `backend-infrastructure-persistence` عند الحاجة.
- Command يطبّق `ICommand` و`IRequest<TResult<T>>`، وQuery يطبّق `IQuery` و`IRequest<TResult<T>>`. استخدم `ITransaction` فقط للعمل الذي يحتاج معاملة صريحة، بعد مراجعة `TransactionBehavior`.
- أعد `TResult<T>` من الطلبات، مستخدماً `OK` و`Fail`، ولا تخلط نتيجة خام أو `Result` غير generic بعقد من نوع `TResult<T>`.
- اجعل Handler مسؤولاً عن orchestration: حقن الواجهات عبر constructor، استدعاء Repository، وتطبيق سلوك المجال عبر الكيان. لا تنفذ EF Core مباشرة في Handler.
- عرّف DTOs للعقود والقراءات. استخدم AutoMapper `Profile` للخرائط المتكررة؛ وإذا كانت القراءة تُسقط DTO داخل Repository، فلا تنشئ مسار mapping مكرراً بلا داعٍ. طابق كل اسم حقل بين Command وDTO والكيان والـ API.
- اكتب Validators بـ FluentValidation وسجّلها ضمن Assembly الذي يمسحه `ApplicationServiceRegistration`. `ValidationBehavior` الحالي يستدعي `Validate` بشكل متزامن؛ لا تضف قواعد async قبل التأكد من دعم `ValidateAsync` في الـ behavior.
- استخدم صيغة أسماء متسقة داخل الميزة: `New/Update/Delete...Command` و`...Handler` و`Get...Query`، مع اتباع المفردات المطلوبة في العقد القائم دون خلط أسماء مرادفة داخل الميزة.
- إذا احتاجت الواجهة Lookup عاماً لكيان كبير، فأنشئ Query مفلتر/مرقّم بدلاً من توسيع `GetList` غير المرقمة: `GetAll{Entity}FilterQuery : BaseFilterQuery<LookupDto>` مع `IQuery` ونتيجة `TResult<Paging<LookupDto>>`. اجعل DTO للاختيار يحتوي ID وDisplay Name والحد الأدنى اللازم فقط، ووفّر Query منفصلاً للتهيئة التفصيلية عند Edit Mode.

## سير العمل

1. افحص Domain والواجهة البرمجية والعقد المقابل في Frontend لتحديد الحقول المطلوبة والـ routes وسلوك النتيجة.
2. حدّد Repository methods ثم الطلبات والـ DTOs والـ Handlers والـ Validators والـ Profiles.
3. تأكد من تسجيل التجميع والسلوك في Application DI؛ لا تضف تسجيلات متكررة لكل Handler.
4. راجع توافق كل الحقول والمسارات باستخدام مهارات Infrastructure وWebApi وFrontend عند العمل عبر الطبقات.

## التحقق قبل الإنهاء

- مجلد التحقق اسمه `Validations` وnamespace كل ملف يطابق مجلده.
- علامات `ICommand`/`IQuery`/`ITransaction` ملائمة لمسار الحفظ، وكل طلب يعيد `TResult<T>`.
- قواعد FluentValidation متوافقة مع قواعد Zod والـ payload، ولا يعيد التغيير تناقضاً من مثال Stage.
