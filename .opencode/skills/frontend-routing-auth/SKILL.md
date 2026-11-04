---
name: frontend-routing-auth
description: Add feature routes to router/Links and router/index.js, applying the existing Vue Router and Pinia Auth Store patterns without treating frontend guards as backend authorization.
---

# التوجيه والمصادقة في Frontend

استخدم هذه المهارة عند إضافة route أو روابط ميزة أو حماية صفحة أو ربط واجهة المصادقة.

## قواعد إلزامية

- ضع routes الخاصة بالميزة في `src/router/Links/{Feature...}.js`، وصدّر مصفوفة routes، ثم اربطها في `src/router/index.js`.
- طابق أسماء المكونات ومسارات الملفات وأسماء routes؛ لا تشير إلى ملف view غير موجود أو route مختلفة عن مسار الاستخدام.
- استخدم `meta.requiresAuth` للصفحات المحمية بحسب النمط القائم، وراجع `beforeEach` و`useAuthStore` قبل تغيير السلوك.
- استخدم Auth Store المبني على Pinia Options Store لعمليات الدخول والخروج وبيانات الجلسة؛ لا تكرر قراءة token أو decoding داخل عدة Views.
- Router guard في المتصفح لتحسين تجربة المستخدم فقط. يجب أن تبقى صلاحيات API والتحقق من JWT والسياسات مسؤولية WebApi.
- إذا أضيف redirect بعد login، تحقق أن المسار الوجهة مسجل، ولا تغيّر تدفق الصلاحيات أو storage ضمن إضافة route عادية.
- استعمل أسماء routes وpaths متسقة، ولا تخلط روابط الحروف الكبيرة والصغيرة أو تهجئات الميزة بين ملف Links والـ View.

## سير العمل

1. افحص `router/index.js` وملف Links المقابل وAuth Store والـ Views قبل الإضافة.
2. أضف route في المجموعة الصحيحة وسجّل المجموعة في Router الرئيسي.
3. راجع metadata وredirect ومسار مكون View، وتحقق من حماية Backend بصورة مستقلة.

## التحقق قبل الإنهاء

- route قابل للحل إلى View موجودة ومجموعة Links مربوطة فعلياً.
- `requiresAuth` يعمل عبر Auth Store، دون ادعاء أن الحماية الأمامية بديل عن API authorization.
