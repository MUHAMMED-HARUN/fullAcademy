---
name: frontend-selection-patterns
description: Choose and implement the correct School_Academy selection pattern—Standard Select, cascading combos, or server-filtered Gridify search—with synchronized IDs, labels, selected records, edit hydration, and empty/error states.
---

# أنماط الاختيار والربط

حمّل هذه المهارة قبل بناء أي حقل Lookup أو Select أو ربط كيان داخل Frontend. لا تختَر المكوّن بناءً على شكله فقط؛ صنّف مصدر البيانات أولاً بحسب حجمه وعلاقاته.

## اختيار النمط

### 1. Standard Select

استخدم `<select>` القياسي عندما تكون الخيارات قليلة ومحدودة ومستقرة أو مستقلة، مثل Enums والجنس والحالات والأدوار. حمّل الخيارات الصغيرة مرة واحدة، واجعل قيمة النموذج هي المعرّف أو القيمة الفعلية لا label المعروض. استخدم `:value` صريحاً للقيم غير النصية مثل Boolean وEnums الرقمية.

لا تستخدم Standard Select لعرض قائمة كيانات كبيرة كاملة؛ إذا صار عدد الخيارات غير محدود أو يتطلب بحثاً عاماً، انتقل إلى `GenericSearchByFilter` مع Gridify.

### 2. Nested / Cascading Combo

استخدم هذا النمط للبيانات ذات علاقة أب/ابن واضحة، مثل `Country → City → District → Neighborhood`.

- اختيار الأب يحدّث معرّفه، ويصفّي خيارات الابن، ويمسح معرّفات وقيم كل المستويات التابعة كي لا يبقى تركيب غير صالح.
- استعمل خصائص منطقية للأبناء؛ لا تربط حقولاً مختلفة في قائمة خيارات واحدة إذا كانت لها علاقات مستقلة.
- في Edit Mode أعد تحميل/تهيئة المعرّفات والـ labels لكل مستوى قبل إظهار النموذج. لا تعتمد على أن الخيارات وصلت بالصدفة قبل فتحه.
- إذا كانت بيانات المستوى كبيرة، اطلب المستوى الأدنى حسب `ParentID` من Endpoint مناسب؛ لا تحمّل جميع المدن أو المناطق تلقائياً ثم تصفّيها في المتصفح.
- اعرض loading/error/empty للحقل التابع، ولا تمسح قيمة صحيحة عند تحديث بيانات غير مرتبطة به.

### 3. GenericSearchByFilter

استخدمه حصراً للكيانات المستقلة كبيرة العدد التي تحتاج البحث، مثل البحث عن Person لربطه بـ Student أو Teacher، أو عن Institute/Class عند الحاجة.

- المكوّن **واجهة بحث فقط**: يستقبل `results` وحقول ID واسم العرض، ويصدر حدث بحث وحدث اختيار. لا يتصل بـ Axios ولا يحمّل البيانات بنفسه.
- مصدر `results` يجب أن يكون طلباً مرّ عبر Store/Service/Repository إلى Endpoint مفلتر ومرقّم بـ Gridify. يمنع منعاً باتاً استدعاء `GetList()` كاملاً ثم تنفيذ `Array.filter` محلياً لاقتراحات GenericSearch.
- حدّد حقول Gridify المسموح البحث فيها بقائمة Allowlist؛ مرّر نوع البحث والقيمة إلى Service ليبني filter آمن، مع `Page` و`PageSize` محدودين.
- عند الاختيار، زامن الحالة الثلاثية: `selectedId` في النموذج، `displayName` في حقل العرض، و`selectedObject` المستخدم للبطاقة/التفاصيل. يجب أن تصدر الأحداث بقيم camelCase في Vue، ويحوّل Mapper/Repository الأسماء إلى عقد Backend.
- المسح أو بدء بحث جديد أو فشل الطلب أو خلو النتائج يمسح الثلاثة معاً ويخفي البطاقة فوراً. لا تترك ID أو Card سابقاً بعد فشل البحث أو تبديل النص.
- لا تختَر نتيجة تلقائياً لأن `results.length === 1` فقط؛ قد تكون هذه نتيجة واحدة في صفحة من نتائج أكثر. الاختيار يتم صراحةً، أو لا يُفعّل auto-select إلا إذا أكد العقد أن `TotalCount === 1` وأن النتيجة فريدة.
- تجاهل الاستجابات القديمة إذا سبقها طلب بحث أحدث، واستخدم `try/finally` لإعادة loading إلى حالته الطبيعية.
- اعرض حالات `loading` و`error` و`no results`، ووفّر تنقلاً بلوحة المفاتيح (`ArrowUp/ArrowDown/Enter/Escape`) وسمات `combobox/listbox/option`.

## قواعد صياغة فلاتر Gridify الإلزامية (Gridify Filtering Syntax)

عند بناء نص `Filter` لـ Gridify، استخدم المعاملات القياسية التالية فقط:

- **Contains (يحتوي على):** `FieldName=*Value`؛ يُمنع منعاً باتاً وضع `*` في نهاية القيمة مثل `FieldName=*Value*`.
- **StartsWith (يبدأ بـ):** `FieldName^Value`.
- **EndsWith (ينتهي بـ):** `FieldName$Value`.
- **Equals (يساوي):** `FieldName=Value`.
- **Not Equals (لا يساوي):** `FieldName!=Value`.
- **Case-Insensitive (تجاهل حالة الأحرف عند الحاجة):** أضف `/i` في النهاية، مثل `FieldName=*Value/i`.
- **الدمج المنطقي:** استخدم `,` للربط بـ (AND) و`|` للربط بـ (OR).

## عقد المكوّن

عند استخدام `GenericSearchByFilter`، وثّق ما يلي في المستهلك:

1. اسم الحقل ومصدر Gridify له.
2. قيمة ID في DTO وقيمة label التي تعرض للمستخدم.
3. مسار البحث/مسح الاختيار/الاختيار الفعلي، ومن يملك `selectedObject`.
4. طريقة تهيئة ID وlabel والـ object عند فتح Edit Mode.
5. سلوك empty/error/loading وما يحدث للبطاقة عند كل حالة.

استخدم `v-model` لعقد المعرّف والاسم، وحدثاً واضحاً مثل `select` و`clear` للعنصر المختار. لا تجعل Card يعتمد على prop قديم مستقل عن العنصر الذي اختاره المستخدم.

## التحقق

- لا `GetList()` كامل ولا client-side filtering في مسار GenericSearch.
- مسح/فشل/صفر نتائج لا يترك أي ID أو label أو preview قديم.
- Edit Mode يعرض record الصحيح قبل تمكين الحفظ.
- الاختيار قابل للاستخدام في RTL بلوحة المفاتيح مع تباين وهوية `Main.css`.
