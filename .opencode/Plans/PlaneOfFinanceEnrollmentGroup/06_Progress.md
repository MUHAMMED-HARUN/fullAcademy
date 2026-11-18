# خطة خدمات StudentStage وDailyEvaluation

## 1. العيوب المكتشفة حالياً
- `clsStudentStage.EnrollmentStudentID` و`clsDailyEvaluation.StudentEnrollmentID` يرتبطان بعضوية Group الحالية، فلا يمثلان مادة منفردة بلا Group.
- `StudentStageService` لا يطبق Read Scope، ويعرض `GroupName` بافتراض وجود Group. `StudentStageController` لا يظهر عليه Authorization.
- `StageInfoScopeExtensions.ApplyStageInfoReadScope` يسمح للطالب بمواد صفه عبر Group، وليس عناصر AcademicPlan snapshot؛ وهذا واسع على اشتراك StageInfo منفرد.
- `DailyEvaluationScopeExtensions` يربط المعلم بـ Group، و`NewDailyEvaluationCommandHandler` يفحص eligibility بواسطة StudentEnrollment فقط.

## 2. خطة Backend
- تحويل `clsStudentStage.EnrollmentStudentID` و`clsDailyEvaluation.StudentEnrollmentID` إلى `StudentAcademicEnrollmentID` بعلاقات EF واضحة؛ يحدد التنفيذ مسار ترحيل FK قبل إزالة القديم.
- استخدام `StudentAcademicEnrollmentItem` كمصدر السماح لـ StageInfo. يجب التحقق أن Stage/StageInfo داخل Snapshot الاشتراك.
- تحديث `IStudentStageRepository`, `StudentStageService`, Queries/DTOs، وRepository/Handlers لـ DailyEvaluation.
- إضافة `ApplyStudentAcademicEnrollmentReadScope` في Infrastructure: الطالب على اشتراكاته وعناصره، المعلم حسب group assignment، Admin شامل.
- إضافة `GetListFilter` للقوائم الكبيرة، وسياسات CRUD، ومجلد `Validations`؛ لا `IgnoreQueryFilters()`.

## 3. خطة منطق Frontend
- تحديث `StudentStageMapper/Service/Store/Repositories/Schema` و`DailyEvaluationMapper/Service/Store/Repositories/Schema` لاستخدام `studentAcademicEnrollmentID`.
- تمرير الاشتراك وStageInfo المصرح به من Store؛ لا تحميل StageInfos للصف ثم فلترتها في الواجهة لاختيار مادة.

## 4. خطة UI/UX ونمط الاختيار
- الخطة الكاملة تعرض مراحلها وموادها الإلزامية؛ StageInfo منفردة تعرض الوحدة نفسها فقط.
- Stage اختيار هرمي محدود؛ StageInfo قائمة تابعة من Endpoint بحسب الاشتراك. Edit Mode يسترجع IDs وlabels وحق الوصول قبل فتح النموذج.

## 5. تطبيق توجيهات المستخدم
- جميع المواد المدرجة في الخطة إلزامية. اشتراك المادة المنفردة يمنح متابعة المادة فقط.
- لا DomainEvent لإنشاء StudentStage أو DailyEvaluation أو تحديث تقدم الاشتراك.
