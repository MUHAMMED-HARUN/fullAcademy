# خطة خدمة Result

## 1. العيوب المكتشفة حالياً
- `clsResult` يطلق `ResultCreatedEvent`، و`ResultCreatedEventHandler` يحسب النتيجة النهائية ويعدل Assignment بعد الحفظ.
- `UpdateResultCommandHandler` يضيف بدلاً من تحديث. `ResultService.GetList()` غير مفلتر/غير مرقم، و`ResultMapper.js` يستخدم casing مخالفاً لـ `StageInfoMatter`.
- لا يتحقق Repository من أن StageInfo ضمن AcademicEnrollmentItems.

## 2. خطة Backend
- إزالة `ResultCreatedEvent` وHandler بعد نقل الحساب إلى Workflow Application صريح. وضع سياسة حساب نقية في Domain (مثلاً `AssignmentScoringPolicy`) بلا وصول إلى Repository.
- تحديث New/Update/Delete Handlers لتحميل Assignment والاشتراك والـ StageInfo، التحقق من الأهلية، وحفظ النتيجة وتحديث حالة الاختبار ضمن `ITransaction` عند تعدد الكيانات.
- حسم عتبة النجاح (50 الحالية أو `MinPassScore`) ومتى يكتمل الاشتراك؛ لا يعتمد تغيير حالة ضمني.
- إصلاح `IResultRepository`, `ResultService`, `ResultInfo`, Validators في `Validations`; إضافة Gridify/Scope وسياسات `ResultController`.

## 3. خطة منطق Frontend
- تحديث `ResultMapper.js`, `ResultService.js`, `ResultStore.js`, `ResultRepositories.js`, `ResultSchema.js` لتطابق `StageInfoMatter` وAcademicEnrollmentID.
- إكمال update الحالي وإعادة تحميل نتيجة الاختبار/الملخص المرقم بعد النجاح.

## 4. خطة UI/UX ونمط الاختيار
- وحدات النتيجة تأتي من الاختبار المحدد؛ في StageInfo منفردة تظهر مادته فقط. لا فلترة محلية من القائمة الكاملة.
- Edit Mode يجلب النتيجة الحالية وID/label حتى لو كانت الوحدة مستبعدة من قائمة الإنشاء؛ عرض MaxScore ورسالة تحقق ملاصقة.

## 5. تطبيق توجيهات المستخدم
- لا DomainEvent للحساب أو إكمال الاختبار. السياسة الحسابية نقية والتنسيق صريح في Application.
- لا تكشف نتائج مواد غير داخلة في اشتراك الطالب.
