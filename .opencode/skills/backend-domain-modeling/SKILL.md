---
name: backend-domain-modeling
description: Create or change School_Academy domain entities, enums, domain events, and domain exceptions while preserving invariants and matching namespaces to feature paths.
---

# نمذجة Domain في School_Academy

استخدم هذه المهارة عند إضافة أو تعديل كيان أو Enum أو Domain Event أو استثناء مجال في `System.Domain`.

## قواعد إلزامية

- افحص كياناً قريباً، `clsBaseClass`، والـ Enums والأحداث والاستثناءات ذات الصلة قبل التعديل. اتبع سلوك المجال، لا أخطاء المسارات أو الـ namespaces القديمة.
- الكيان يبدأ اسمه بـ `cls` ويرث من `clsBaseClass`. استخدم setters خاصة، ومنشئاً محمياً لـ EF Core، ومنشئاً يفرض invariants المجال، وطرقاً صريحة لتعديل الحالة.
- الـ Enum يبدأ اسمه بـ `en` ويوضع في `Enums/{Feature}`.
- اجعل الـ namespace مطابقاً للمسار. مثال: `Entities/Progress/clsStage.cs` يقابله `System.Domain.Entities.Progress`، لا namespace عام أو تابع لميزة أخرى.
- ضع Domain Events الخاصة بالميزة في مجلدها، وطبّق عقد `IDomainEvent` الحالي. لا تنشر الحدث مباشرة من الكيان؛ أضفه إلى مجموعة أحداث `clsBaseClass` ودع مسار Unit of Work ينشره.
- استخدم `DomainException` وكتالوج رسائل Domain القائم عند انتهاك قاعدة مجال. لا تضع تحقق واجهة المستخدم أو تفاصيل HTTP/EF في Domain.
- حافظ على خصائص العلاقات وأنواع المعرفات متوافقة مع الكيان وConfiguration المرتبطين؛ لا تضف مرجعاً أو حدثاً افتراضياً بلا حاجة مجال واضحة.

## سير العمل

1. حدّد الميزة والكيانات والعلاقات والـ enums/الأحداث المستخدمة فعلياً.
2. اكتب قواعد invariants أولاً، ثم طبّقها في constructor وطرق الكيان.
3. طابق namespace مع مجلد الملف، ثم افحص كل استدعاء/مرجع للعضو الذي غيّرته.
4. عند إضافة Event، تحقق من جهة النشر والـ Handler في الطبقات الأخرى باستخدام المهارات المناسبة؛ لا توسّع نطاق التعديل تلقائياً.

## التحقق قبل الإنهاء

- لا setters عامة لحقول الكيان ولا منطق persistence داخل Domain.
- كل اسم ومسار وnamespace ومرجع علاقة متطابق، ولا يعيد العمل تناقضاً مثل `Stage` السابق.
- اذكر أي تغيير عقدي أو حالة لا يمكن التحقق منها؛ لا تخفِ عدم اليقين ولا تعدّل ملفات خارج نطاق الطلب.
