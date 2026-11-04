# الخدمة 2 — Class إلى Institute: ربط عدة صفوف

## العيوب الحالية

- الإجراء موجود بالفعل: `ClassView.vue` يعرّف `addByRange` ويفتح `UpsertInstituteClassByRangeComp.vue`؛ الخطة تطوير الإجراء لا إضافة إجراء مكرر.
- `POST /InstituteClass/NewInstituteClassByRange` يعالج الربط في `NewInstituteClassByRangeCommandHandler.cs` ويتجاوز الصف المرتبط، لكنه يعيد عدد المضاف فقط؛ الواجهة تعرض نجاحاً عاماً ولا تبيّن ما تم تجاوزه.
- واجهة الدفعة تختار المعهد بعد تحديد الصفوف، فلا يستطيع المستخدم معرفة الصفوف المرتبطة مسبقاً قبل الحفظ.
- يوجد فهرس فريد `(InstituteID, ClassID)` في `InstituteClassConfiguration.cs`، وهو حماية إضافية، لكن التحقق الحالي يتم صفاً بعد صف.
- `ClassView.vue` في مسار الربط الفردي يخفي `showUpsert` بدلاً من نافذة الربط `showAddToInstitue` بعد النجاح؛ يجب تصحيح إغلاق النافذة المعنية ضمن تنفيذ هذا التدفق.

## خطة Backend

- إعادة استخدام `InstituteClassController` وEndpoint `POST /NewInstituteClassByRange`؛ لا حاجة مبدئية إلى Endpoint جديد.
- تحديث `NewInstituteClassByRangeCommand` وHandler لإزالة تكرار `ClassIDs` والتحقق من المعرفات، وإجراء فحص دفعي للروابط الموجودة عبر عقد `IInstituteClassRepository` وتنفيذه في `InstituteClassService` بدلاً من استعلام منفصل لكل صف.
- تحسين نتيجة الأمر لإرجاع عدد المضاف ومعرّفات الصفوف المتجاوزة بسبب وجودها مسبقاً، ثم مواءمة `TResult` وعقد الواجهة معاً.
- تحديث `NewInstituteClassByRangeCommandValidation` للتحقق من المعهد والقائمة والمعرفات. ملف التحقق الحالي داخل مجلد `Validation`؛ أي Validator جديد يجب أن يلتزم بمجلد `Validations` المعتمد.
- الحفاظ على الفهرس الفريد الحالي، وعدم اقتراح migration إلا إذا تغيّرت قاعدة التعامل مع سجل ربط محذوف soft-delete؛ هذه النقطة تحتاج قراراً قبل تغيير قاعدة البيانات.

## خطة منطق Frontend

- تعديل `InstituteClassRepository.js`, `InstituteClassService.js`, و`InstituteClassStore.js` لمواءمة نتيجة الدفعة الجديدة، ثم تحديث `InstituteClassMapper.js` أو إضافة Mapper خاص بالنتيجة إذا استدعى شكل العقد ذلك.
- إضافة Zod schema لحمولة الدفعة، والتحقق من `instituteID` و`classIDs` والمعرفات الفريدة بدلاً من تجاوز التحقق في مسار `createByRange`.
- تحديث `ClassView.vue` و`UpsertInstituteClassByRangeComp.vue` لعرض الصفوف التي تم تجاوزها، وعدد الصفوف المضافة، ومعالجة الفشل، وإغلاق النافذة الصحيحة وإعادة تحديث البيانات.
- يبقى فحص الخادم هو الحماية النهائية، حتى لو سبقته الواجهة، لتغطية الإرسال المتكرر أو التزامن.

## خطة UI/UX ونمط الاختيار

- المعهد: `GenericSearchByFilter` مع بحث خادمي؛ لا تحميل قائمة المعاهد كاملة لغرض البحث.
- الصفوف المختارة من شاشة Class تبقى كما هي في الإجراء القائم. بعد اختيار المعهد، تعرض الواجهة نتائج الإرسال بوضوح: المضاف، والمتجاوز لأنه مرتبط مسبقاً. لا يتم إرسال IDs مكررة.
- إذا كان المطلوب إخفاء الصف المرتبط قبل الحفظ بدلاً من إظهار التجاوز بعده، فحينها يلزم Endpoint فحص/قائمة متاحة؛ لم يُضف إلى الخطة الأساسية لأن الفحص النهائي الحالي موجود في الخادم، لكن نتيجته تحتاج أن تصبح مرئية للمستخدم.
- الحفاظ على حالات loading/error والقائمة الفارغة، وتحديث بطاقات أو أسماء الصفوف المحددة عند اختيارها.

## تطبيق التوجيهات والأمان

- إعادة استخدام إجراء Class الحالي لربط عدة صفوف بمعهد، مع الإبقاء على فحص التكرار في Backend والفهرس.
- يلزم مراجعة `ClassController` و`InstituteClassController` و`clsPermission.cs` و`Program.cs` و`GetAllPermissionHandler.cs` و`UpdateRolePermissionsCommandValidator.cs`؛ حالياً كتالوج الصلاحيات الظاهر يقتصر على User ولا تظهر سياسات على Controller الربط. الخطة تضيف صلاحيات القراءة/الإنشاء المناسبة، وتقيّد إنشاء الروابط بالدور المصرح، مع ضبط Role-Permission matrix وتفعيل صلاحيات الواجهة في `ClassView.vue` دون اعتبارها تفويضاً خادمياً.
