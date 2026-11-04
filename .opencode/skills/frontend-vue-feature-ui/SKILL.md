---
name: frontend-vue-feature-ui
description: Build School_Academy Vue feature Views and List, Card, Upsert, and Popup components using Vue 3 script setup, props and emits, Pinia Options Stores, and the existing feature folders.
---

# واجهات Vue للميزات

استخدم هذه المهارة لإنشاء أو تعديل مكونات الميزة وViews وتجميعها.

قبل إضافة أي حقل اختيار، اقرأ مهارة `frontend-selection-patterns` واختر Standard Select أو Nested/Cascading Combo أو GenericSearchByFilter وفق حجم البيانات وعلاقاتها.

## قواعد إلزامية

- ضع View في `src/views/{Feature}` والمكونات الخاصة بها في `src/components/{Feature}/{Entity}`، واستخدم مجلدات `components/general` و`components/GlobalComp` عند ملاءمة مكون مشترك.
- اكتب كل **مكون Vue جديد** بأسلوب Vue 3 `<script setup>` واستخدم `defineProps` و`defineEmits` للواجهات؛ لا تنشئ مكوناً جديداً بصيغة Options API.
- احتفظ بـ Pinia **Options Store** كما هو معتمد. View تقرأ الحالة وتستدعي Actions، والمكونات تعرض بيانات وتتواصل عبر props/events، ولا تنفذ طلبات API مباشرة.
- افصل مسؤوليات العرض: `List` للقائمة والإجراءات، `Card` للتفاصيل، `Upsert` للنموذج، و`Popup` لإطار الحوار عند الحاجة. لا تنشئ كل هذه المكونات إذا لم تتطلبها الميزة.
- عرّف props وemits بأسماء واضحة ومتطابقة مع المستهلك. استخدم أسماء أحداث موحدة ولا تخلط kebab-case وPascalCase عشوائياً.
- في أي Lookup، أبقِ ID والاسم المعروض والعنصر المختار/بطاقة المعاينة متزامنة؛ المسح أو تبديل البحث أو الخطأ أو خلو النتائج يخفي البطاقة ويمسح الاختيار بالكامل. لا تضبط `showCard = true` عند عدم العثور.
- استخدم `GenericSearchByFilter` للكيانات الكبيرة فقط مع نتائج من Gridify server-side؛ لا تربطه بـ `GetList()` كاملة أو تصفية `Array.filter` محلية. راجع `frontend-selection-patterns` لقواعد العقد والأحداث.
- الواجهة عربية وتستخدم RTL؛ استخدم خصائص واتجاهات منطقية مثل `start/end` و`ps/pe`، ولا تثبت layout إلى اليمين/اليسار بطريقة تكسر اتجاه النص.
- اعرض حالة التحميل والأخطاء والنتيجة الفارغة، واربط أخطاء الحقول بالمكون العام الملائم. لا تخفِ رد الفعل عند فشل Action.
- حافظ على مكونات الواجهة صغيرة وقابلة لإعادة الاستخدام، ولا تكرر منطق بيانات Store أو قواعد Zod داخل component.

## سير العمل

1. افحص View ومكونات الميزة ومكوناً مشتركاً شبيهاً قبل التعديل.
2. حدد state التي تأتي من Store والـ props/events بين الأب والابن.
3. ابنِ المكونات الجديدة بـ `<script setup>`، ثم اربطها في View بأقل قدر من المنطق.
4. افحص الشاشة على أحجام مختلفة واتجاه RTL، وراجع استخدام مكونات ورسائل الأخطاء المشتركة.

## التحقق قبل الإنهاء

- المكونات الجديدة تستخدم `<script setup>`، والـ Store يبقى Options Store.
- لا API calls من مكونات العرض، والـ props/events وأسماء الحقول متطابقة.
- تدفق التحميل والخطأ والنجاح والـ RTL قابل للاستخدام بلوحة المفاتيح والشاشات الصغيرة.
