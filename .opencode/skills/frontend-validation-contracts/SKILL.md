---
name: frontend-validation-contracts
description: Define feature Zod schemas and use validate1 in frontend Services while matching required and optional fields, constraints, and error keys to backend FluentValidation and domain rules.
---

# تحقق المدخلات وعقودها

استخدم هذه المهارة عند إضافة أو تعديل Zod schema أو validation في Frontend، أو عند مواءمة التحقق مع Application.

## قواعد إلزامية

- ضع المخطط في `src/validations/{Feature}`، واتبِع تصديراً واسم ملف واضحين ومتسقين داخل الميزة.
- استخدم Zod، ثم `validate1(data, schema)` من Service قبل الإرسال عند عمليات الإنشاء والتحديث. اعرض أخطاء الحقول من نتيجة التحقق بدلاً من إرسال بيانات غير صالحة.
- طابق الحقول المطلوبة والاختيارية وأنواعها والحدود ورسائل الأخطاء مع FluentValidation وCommand/DTO والـ Domain invariants. لا تجعل حقلاً مطلوباً في طرف واختيارياً في الآخر دون سبب موثق.
- دقق في أثر Zod modifiers: `optional()` يعني أن القيمة قد تكون غائبة، وليس أنها مطلوبة مع رسالة خطأ. استخدم القاعدة التي تحقق المقصود فعلاً.
- استخدم أسماء الحقول الداخلية camelCase، ثم اترك Mapper يحولها إلى أسماء Backend PascalCase. يجب أن تكون مفاتيح الأخطاء المستخدمة في UI متوافقة مع نموذج الواجهة.
- تحقق الواجهة يحسن تجربة الاستخدام ولا يستبدل FluentValidation أو Domain rules على الخادم.
- لا تضف تحققاً ثانياً متعارضاً في View عند وجود Zod في Service. `useFormValidation.js` موجود لكنه ليس مسار التحقق المستخدم في الميزات المفحوصة؛ لا تعتمده بديلاً دون توحيد معمارية التحقق أولاً.
- رسائل الخطأ المقدمة إلى المكونات يجب أن تكون متوافقة مع شكل `errors` الفعلي (نص أو خريطة حقول)، ولا تفترض أن جميع endpoints تعيد envelope واحداً قبل فحصها.

## سير العمل

1. اقرأ Validator وCommand/DTO وMapper وSchema الحالية قبل تغيير أي قاعدة.
2. اكتب جدولاً مختصراً للحقول المطلوبة/الاختيارية وأنواعها على الطرفين، ثم طبّق Zod.
3. اربط `validate1` بـ Service وتأكد من إعادة أخطاء مفهومة للـ Store والواجهة.
4. أضف/حدّث اختباراً للمخطط إن كان ضمن نطاق المشروع، أو اذكر ما لم يُختبر.

## التحقق قبل الإنهاء

- لم تُنسخ حالة مثل `classID` اختياري في Zod وإجباري في FluentValidation.
- بيانات صالحة للواجهة مقبولة في العقد الخلفي، والبيانات غير الصالحة تُرفض في الطرفين.
