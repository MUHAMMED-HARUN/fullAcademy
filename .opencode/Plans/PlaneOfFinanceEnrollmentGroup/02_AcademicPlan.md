# خطة AcademicPlan (خدمة جديدة)

## 1. العيوب المكتشفة حالياً
- لا يوجد كيان يمثل عرضاً أكاديمياً قابلاً للشراء أو نسخة ثابتة من محتواه.
- الهيكل الحالي موزع: `clsProgram` لا يملك Institute FK، `clsClass.ProgramID`، `clsInstituteClass` يربط المدرسة بالصف، `clsStage.ClassID`، و`clsStageInfo.StageID/MatterID`.
- `FeeStructure` يحمل أهدافاً أكاديمية مباشرة ويخلط المنتج الدراسي بالسعر.

## 2. خطة Backend
- إنشاء `System.Domain/Entities/AcademicPlan/clsAcademicPlan.cs` و`clsAcademicPlanItem.cs`. رأس الخطة: `Name`, `ProviderInstituteID`, `ScopeType` (Program/InstituteClass/Stage/StageInfo), FK typed للنطاق، `Version`, `Status`, `EffectiveFrom/To`.
- عناصر الخطة تحفظ `StageInfoID` الإلزامية كـ Snapshot. خطة Program/Class/Stage تتضمن المواد التابعة، وخطة StageInfo المنفردة عنصرها مادة واحدة.
- لا تستخدم `TargetID` polymorphic بلا FK. تحقق Domain/FluentValidation من توافق ScopeType وFK؛ إنشاء `IAcademicPlanRepository`, Queries/Commands, `AcademicPlanInfo`, Mapper Profile، ومجلد `Validations`.
- إضافة `AcademicPlanConfiguration`, `AcademicPlanService`, تسجيل DI، و`AcademicPlanController` بسياسات `Can_*_AcademicPlan`. إضافة `GetAcademicPlanListFilter` بـ Gridify و`GetAcademicPlanBy/get-for-upsert` يعيد IDs والمحتوى.

## 3. خطة منطق Frontend
- إنشاء `src/repositories/AcademicPlan/AcademicPlanRepositories.js`, `src/Mapper/AcademicPlan/AcademicPlanMapper.js`, `src/validations/AcademicPlan/AcademicPlanSchema.js`, `src/Service/AcademicPlan/AcademicPlanService.js`, و`src/store/AcademicPlan/AcademicPlanStore.js` بنمط Pinia Options.
- إضافة `src/router/Links/AcademicPlanLinks.js` وربطه في `src/router/index.js`.
- عقد DTO يعيد `AcademicPlanID`, `ScopeType`, المدرسة، النطاق، الإصدار وعناصر StageInfo.

## 4. خطة UI/UX ونمط الاختيار
- إنشاء View/List/Card/Upsert في `src/views/AcademicPlan/` و`src/components/AcademicPlan/AcademicPlan/`.
- `ScopeType` Standard Select، ثم Nested Combo للمدرسة → InstituteClass → الهدف، أو Class → Stage → StageInfo. البحث عن مؤسسة/خطة كبيرة عبر GenericSearchByFilter + Gridify.
- العناصر الإلزامية للعرض فقط. Edit Mode يحمل الإصدار والعناصر؛ إذا استُخدمت الخطة لا يعدّل Snapshot بل ينشئ إصداراً جديداً.

## 5. تطبيق توجيهات المستخدم
- مواد Program/Class/Stage إلزامية. StageInfo واحدة مسموحة كخطة مستقلة لإتاحة الدراسة/الاختبار والشهادة.
- لا DomainEvent لبناء الخطة أو Snapshot المحتوى؛ التنفيذ صريح عبر Command.
