---
name: frontend-feature-data-flow
description: Implement the School_Academy frontend feature Mapper, Service, and Pinia Options Store so data flows consistently from Views through clsUtil repositories and back as TResult state.
---

# تدفق بيانات الميزة

استخدم هذه المهارة لإنشاء أو تعديل الثلاثية `Mapper` و`Service` و`Pinia Store` في الميزة.

عند بناء أي Selector، حمّل `frontend-selection-patterns` لتحديد النمط ومصدر البيانات قبل تعديل الثلاثية.

## قواعد إلزامية

- احفظ التدفق بهذا الترتيب دون تجاوزه: `View → Pinia Store → Service → Repository (clsUtil.FeachRequset) → Axios`، والعودة عبر Repository وService وStore إلى View.
- استخدم Mapper صريحاً بين نموذج الواجهة camelCase وعقد Backend PascalCase. طبّق mapping منفصلاً للـ upsert والقراءات/القوائم عندما تختلف أشكالها، وتحقق من جميع أسماء الحقول.
- اجعل Service واجهة منطق الميزة: يتحقق عبر `validate1` عند الحاجة، يحوّل البيانات عبر Mapper، يستدعي Repository، ويعيد `TResult` موحداً. لا تجعل View أو Store يستدعي Repository مباشرة.
- احتفظ بـ **Pinia Options Store** (`defineStore` مع `state/getters/actions`) كما في المشروع. لا تحوّل المخزن إلى Composition Store.
- إدارة حالة الميزة تشمل ما يلزم من `loading` و`error` وبيانات القائمة/العنصر المحدد. استدعِ Service من Actions، حدّث الحالة بعد النجاح، واحتفظ بنتيجة العملية للـ View.
- أعد `loading` إلى false حتى عند فشل/رمي الطلب (استخدم `try/finally` عند الحاجة)، وامسح الأخطاء القديمة عند بدء العملية. لا تخفِ خطأ API أو خطأ validation.
- اتبع أسلوب تصدير الـ singleton المستخدم في Service وMapper عند ملاءمته، ولا تنقل منطق العرض أو إدارة popup إلى Service.
- قبل إضافة Service/Store، تحقق من وجود ملف قائم بأسماء مختلفة في الميزة لتجنب إنشاء تنفيذ مكرر.
- لحقل Lookup كبير، أضف تدفق نتائج مستقل من Store إلى Service ثم Repository إلى Endpoint Gridify مفلتر/مرقّم؛ لا تستدعِ `GetList()` لتصفية النتائج في المتصفح. احتفظ بـ ID وlabel والعنصر المحدد متزامنة، وامسحها مع البطاقة عند المسح أو البحث الجديد أو الفشل أو عدم وجود نتائج.
- في Edit Mode، حمّل العنصر المرتبط عبر `GetById` أو `get-for-upsert`، ثم هيئ ID وlabel والـ object قبل عرض النموذج؛ لا تعتمد على `GetList` أو قائمة حالية لإعادة بناء علاقة السجل.

## سير العمل

1. افحص Repository المقابل وعقد Backend وSchemas وMapper وService وStore لميزة قريبة.
2. ثبت نموذج الواجهة camelCase، ثم خرائط التحويل، ثم عمليات Service، ثم state/actions.
3. اربط الـ View بالـ Store فقط، وراجع مسار التحميل والنجاح والفشل والتحديث بعد التغيير.

## التحقق قبل الإنهاء

- كل خاصية camelCase تتحول إلى الخاصية الصحيحة في API والعكس.
- لا يوجد اتصال HTTP خارج Repository، ولا منطق تحقق أو mapping متكرر في View.
- الحالات والنتائج متوافقة مع `TResult` المستخدم فعلياً.
