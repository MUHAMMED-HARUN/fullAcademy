# خطة خدمة FeeStructure

## 1. العيوب المكتشفة حالياً
- `clsFeeStructure` و`NewFeeStructureCommand` يقبلان Program/Class/Stage/StageInfo كحقول nullable منفصلة بلا تحقق يمنع الأهداف المتعارضة.
- `FeeStructureView.vue` يستخدم `applyClientQuery` على القائمة الكاملة؛ `UpsertFeeStructureComp.vue` يفلتر StageInfos محلياً ويتيح أهدافاً متعددة معاً.
- `FeeStructureController` بلا Authorization ظاهر، ولا توجد صلاحيات Finance في `clsPermission`.

## 2. خطة Backend
- ربط `clsFeeStructure` بـ `AcademicPlanID`؛ تبقى `FeeType` وAmount/terms معلومات مالية لا تحدد النطاق الدراسي.
- تحديث `IFeeStructureRepository`, `FeeStructureService`, `FeeStructureInfo`, `FeeStructureConfiguration`, Commands/Mapper و`FeeStructureCommandValidation` في `Validations`.
- إضافة `GetListFilter` Gridify و`GetByAcademicPlan`; حماية `FeeStructureController` بسياسات CRUD. التعرفة المستخدمة تحفظ كـ Snapshot على الفاتورة.
- تغيير السعر لخطة مستخدمة ينشئ تعرفة/إصداراً جديداً أو صلاحية زمنية؛ لا يعدل تاريخ الفواتير السابقة.

## 3. خطة منطق Frontend
- تحديث `FeeStructureMapper.js`, `FeeStructureService.js`, `FeeStructureStore.js`, `FeeStructureRepositories.js`, `FeeStructureSchema.js` لاستخدام AcademicPlanID.
- نقل البحث والفرز والترقيم من applyClientQuery إلى Gridify، ودعم البحث حسب الخطة والنوع والحالة.

## 4. خطة UI/UX ونمط الاختيار
- اختيار AcademicPlan من Lookup مفلتر؛ GenericSearchByFilter عند كبر القائمة. FeeType والحالة Standard Select.
- عرض ProviderInstitute والنطاق وإصدار الخطة كبيانات قراءة. Edit Mode يستعيد IDs وlabels ولا يسمح بتغيير هدف تعرفة مستخدمة.

## 5. تطبيق توجيهات المستخدم
- يمكن تسعير خطة Program أو InstituteClass أو Stage أو StageInfo منفردة. لا رسوم اختيارية داخل برنامج مواده إلزامية.
- لا DomainEvent لتطبيق السعر أو نسخه إلى الفاتورة.
