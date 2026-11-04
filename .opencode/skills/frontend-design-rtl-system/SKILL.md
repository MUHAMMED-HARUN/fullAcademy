---
name: frontend-design-rtl-system
description: Apply the School_Academy Main.css visual identity with Tailwind layout utilities, Arabic RTL support, responsive tables and forms, validation feedback, and loading states without conflicting design tokens.
---

# نظام التصميم وRTL

استخدم هذه المهارة عند بناء أو تعديل تنسيق واجهات Frontend، بما يشمل الهوية البصرية والتجاوب والنماذج والجداول.

اقرأ `frontend-selection-patterns` عند تصميم قوائم الاختيار ونتائج البحث.

## مرجع الهوية البصرية الإلزامي

- المرجع الأساسي هو `src/css/Main.css`: متغير اللون الأساسي `--primary-color: #1e3a8a`، ألوان الحالات، خطوط Cairo وOutfit، ومقاييس التباعد والحواف والظلال.
- استخدم Tailwind CSS أساساً للتخطيط السريع وFlex/Grid والمسافات ونقاط التجاوب. يجب ألا تستبدل أصناف Tailwind الهوية اللونية المعتمدة أو تناقض متغيرات Main.css.
- لا تستخدم قيم `brand-*` الحالية من `tailwind.config.js` كلون هوية بديل؛ درجتها مثل `brand-500` تختلف عن اللون الأساسي في Main.css. استخدم متغيرات/أصناف الهوية الموجودة، أو اطلب توحيداً صريحاً إذا لزم تعديل tokens.
- استخدم Cairo/Outfit والـ CSS variables بدلاً من تعريف palette أو spacing أو typography موازٍ داخل كل مكون.
- حافظ على RTL عبر خصائص CSS المنطقية وأصناف Tailwind المنطقية (`start/end`, `ps/pe`, `text-start`) عند دعمها. لا تعكس الأيقونات أو ترتيب العناصر آلياً دون فحص معناها.
- اضبط `lang="ar"` و`dir="rtl"` على عنصر `<html>` في `public/index.html`، ولا تفرض في `App.vue` خطاً أو لوناً على `#app` يتغلب على `Main.css`؛ يجب أن تسود `Cairo` و`Outfit` ومتغيرات الهوية.
- فضّل utility classes للتخطيط والتجاوب، و`Main.css` للهوية والقواعد العامة. لا تضف selectors عامة قوية أو `!important` جديدة لمجرد تجاوز تنسيق مكون؛ استخدم scoped style ضيقاً عند وجود حاجة خاصة.

## نماذج وجداول وحالات واجهة

- نسّق الحقول عبر قواعد `.form-group` وحقول الإدخال والأزرار الموجودة، والجداول ضمن تخطيط متجاوب، مع المحافظة على وضوح labels والمسافات.
- اربط أخطاء التحقق قرب الحقل بصرياً، مع نص قابل للقراءة وتباين مناسب؛ لا تعتمد على اللون وحده للدلالة على الخطأ.
- ميّز حالات loading/disabled وempty/error/success، وحافظ على تباين focus-visible وعدم إزالة مؤشر لوحة المفاتيح.
- نتائج GenericSearch قائمة تفاعلية: وفر `role="combobox/listbox/option"`، وحركة `ArrowUp/ArrowDown` والاختيار بـ `Enter` والإغلاق بـ `Escape`، ومؤشراً مرئياً للخيار النشط. حافظ على `start/end` في RTL.
- راجع اختلاف الواجهة القديمة المعتمدة على Main.css والواجهة التي تستخدم Tailwind، ولا تخلط نظامي تصميم في المكون الواحد بلا حاجة.
- لا تضف مكتبة UI أو خطوطاً أو ألواناً خارجية دون طلب؛ fonts المستوردة حالياً من Google Fonts جزء من الإعداد القائم.

## سير العمل

1. افحص Main.css وTailwind config وPostCSS ومكوناً قريباً من نفس نوع الواجهة.
2. استخدم tokens الهوية، ثم أضف utilities للتخطيط والتجاوب فقط.
3. راجع RTL والمقاسات الصغيرة والـ hover/focus/disabled وحالات التحقق.

## التحقق قبل الإنهاء

- الهوية البصرية تتبع `Main.css` ولا تستحدث primary color أو font family منافساً.
- الواجهة متجاوبة وRTL، مع تباين وحالات focus وخطأ وتحميل واضحة.
- لا توجد قواعد global أو `!important` غير لازمة ولا تعارض غير مقصود مع Tailwind.
