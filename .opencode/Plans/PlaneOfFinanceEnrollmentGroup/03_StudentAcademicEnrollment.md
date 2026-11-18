# خطة StudentAcademicEnrollment / Enrollment

## 1. العيوب المكتشفة حالياً
- `clsStudentEnrollment` في `System.Domain/Entities/Enrollment/clsStudentEnrollment.cs` يفرض `StudentID` و`GroupID`، فلا يمثل برنامجاً كاملاً عبر عدة مراحل ولا مادة منفردة بلا Group.
- `NewStudentEnrollmentCommand` (`StudentID`, `GroupID`, `Date`) و`EnrollByRangeCommand` ينشئان عضوية Group فقط؛ `GroupView.vue` يتيح تسجيل الطالب مباشرة في Group.
- `StudentStage`, DailyEvaluation وAssignmentNomination تعتمد `StudentEnrollmentID`; لا تستطيع Finance وحدها منح حق اختبار مادة منفردة.

## 2. خطة Backend
- إنشاء `clsStudentAcademicEnrollment` (StudentID, AcademicPlanID/VersionID, EnrollmentDate, Status, Validity) و`clsStudentAcademicEnrollmentItem` كنسخة StageInfo المصرح بها وقت الاشتراك.
- إبقاء `clsStudentEnrollment` كعضوية تشغيلية في Group وإضافة `StudentAcademicEnrollmentID`. يسمح الاشتراك أن يكون بلا Group؛ العضوية تضاف عند وجود شعبة مناسبة.
- إضافة Configurations/Repository/DI و`StudentAcademicEnrollmentController` مع `EnrollStudentAcademicPlanCommand`, `GetListFilter`, `get-for-upsert` وأوامر Cancel/Transfer إذا اعتمدت.
- تحديث `StudentEnrollmentController` و`StudentEnrollmentService` لإنشاء عضوية Group من اشتراك قائم والتحقق من أن `InstituteClassID` للشعبة ضمن AcademicPlan. منع إنشاء شراء من StudentID+Group وحدهما.
- إنشاء الاشتراك ونسخ العناصر ضمن `ITransaction`; إصدار الفاتورة عبر Command صريح لاحق أو Orchestrator معتمد، لا DomainEvent.

## 3. خطة منطق Frontend
- تحديث `StudentEnrollmentStore.js`, `StudentEnrollmentService.js`, `StudentEnrollmentRepositories.js`, Mapper وSchema لتمييز `studentAcademicEnrollmentID` عن `groupEnrollmentID`.
- إنشاء Repository/Mapper/Schema/Service/Store للاشتراك الأكاديمي. شاشة التسجيل تبدأ من AcademicPlan؛ شاشة Group تضيف اشتراكاً قائماً فقط.

## 4. خطة UI/UX ونمط الاختيار
- الطالب: GenericSearchByFilter server-side من شاشة موظف، أو سياق ثابت من ملف الطالب. AcademicPlan عبر Gridify/Nested Combo.
- عضوية Group خطوة منفصلة اختيارية، وتعرض الاشتراكات المؤهلة فقط. Edit Mode يعيد الخطة والإصدار والـ Snapshot والعضويات السابقة.

## 5. تطبيق توجيهات المستخدم
- اشتراك Program/صف/مرحلة/StageInfo مستقل عن Group. طالب واحد قد يملك اشتراكات متعددة، واشتراك Program قد يرتبط بعضويات Groups متعددة بالتتابع.
- لا DomainEvent لإنشاء فاتورة أو عضوية أو حق وصول.
