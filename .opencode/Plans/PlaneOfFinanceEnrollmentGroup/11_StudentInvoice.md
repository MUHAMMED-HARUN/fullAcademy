# خطة خدمة StudentInvoice

## 1. العيوب المكتشفة حالياً
- `clsStudentInvoice.StudentID` لا يحدد الاشتراك/الخطة. `NewStudentInvoiceCommand` يقبل StudentID وFeeStructureID والمبالغ من العميل.
- `StudentInvoiceService.GetBy(StudentID)` يرجع `FirstOrDefault` رغم وجود عدة فواتير؛ `GetList` غير مرقم وغير مقيّد.
- `StudentInvoiceView.vue` يختار الطالب والرسم بلا AcademicEnrollment ويجري بحثاً محلياً. تحديث الحالة والمتبقي من العميل قد يفسد Ledger.
- الفواتير الحالية قد لا يمكن ربطها بتسجيل محدد تلقائياً عند تعدد تسجيلات الطالب.

## 2. خطة Backend
- إضافة `StudentAcademicEnrollmentID` إلى `clsStudentInvoice`, `StudentInvoiceConfiguration`, Commands و`StudentInvoiceInfo`. StudentID يستنتج من الاشتراك أو يبقى مؤقتاً مع consistency validation.
- إضافة `GenerateInvoicesForEnrollmentCommand` صريح: تحميل الاشتراك والخطة والتعرفة، تحقق الأهلية، إنشاء فاتورة بمبلغ/خطة Snapshot. منع التكرار بمعرف الاشتراك والتعرفة وفترة الفوترة.
- إضافة `GetByAcademicEnrollment`, `GetListFilter`, `GetStudentFinanceSummary`; إصلاح GetBy الطالب إذا كانت النتيجة المفترضة قائمة.
- تحديث `IStudentInvoiceRepository`, `StudentInvoiceService`, Handlers وValidators في مجلد `Validations`; `GenerateInvoices` يستخدم `ITransaction`.
- عدم قبول OriginalAmount/RemainingAmount/Status من العميل. منع حذف/تعديل فاتورة ذات دفعات إلا بإجراء إلغاء موثق. جرد الفواتير القديمة قبل Migration.

## 3. خطة منطق Frontend
- تحديث `StudentInvoiceMapper.js`, `StudentInvoiceService.js`, `StudentInvoiceStore.js`, `StudentInvoiceRepositories.js`, `StudentInvoiceSchema.js`.
- الإنشاء يرسل StudentAcademicEnrollmentID وFeeStructure/period المطلوبين فقط؛ API يعيد المبلغ والحالة المحسوبة.
- تحويل StudentInvoiceView من فلترة قائمة كاملة إلى Gridify مع Page/PageSize/Filter.

## 4. خطة UI/UX ونمط الاختيار
- بدء إصدار الفاتورة من الاشتراك المحدد، لا اختيار طالب ورسوم عشوائية. لوحة مالية عامة تختار اشتراكاً عبر GenericSearchByFilter/Gridify.
- `get-for-upsert` يعيد EnrollmentID والاسم والخطة والإصدار والفاتورة. الحالة والمتبقي للعرض لا للتعديل.
- عرض إجمالي/مدفوع/متبقٍ/استحقاق، وحالات تحميل/خطأ/لا توجد فواتير.

## 5. تطبيق توجيهات المستخدم
- الاشتراك الأكاديمي هو سبب الفاتورة، لا Group. اشتراك StageInfo منفرد له فاتورة وحق محدود بتلك المادة.
- الطالب يقرأ فواتيره ذاتياً؛ لا DomainEvent لإنشاء فاتورة أو متابعة دين.
