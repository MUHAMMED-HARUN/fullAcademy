# خطة خدمة Payment

## 1. العيوب المكتشفة حالياً
- Payment مربوط بـ StudentInvoiceID، لكن `NewPaymentCommandHandler` وUpdate/Delete يغيرون الدفع والفاتورة من دون معاملة معلنة للتنافس على الرصيد.
- `PaymentController` بلا Authorization ظاهر. `PaymentService.GetList()` يعيد كل الدفعات، و`PaymentView.vue` يفلتر محلياً ويستنتج invoiceLabel من قائمة فواتير كاملة.
- الحذف يعكس `ReversePayment` ثم soft-delete؛ لا توجد حركة عكس مستقلة موثقة.

## 2. خطة Backend
- التحقق من نطاق الفاتورة/الاشتراك قبل العرض أو الدفع. تحويل Create/Update/Reverse إلى `ITransaction` لمنع سباق دفعتين يتجاوز المتبقي.
- اعتماد Recommendation: الدفعة Immutable؛ التصحيح بـ `ReversePaymentCommand` مع السبب والمستخدم والتاريخ، لا حذف صامت.
- تحديث `IPaymentRepository`, `PaymentService`, Commands و`PaymentCommandValidation`; إضافة `GetListFilter`, `GetPaymentsByInvoice` وScope.
- حماية `PaymentController` بصلاحيات `Can_Read_Payment`, `Can_Record_Payment`, `Can_Reverse_Payment`.

## 3. خطة منطق Frontend
- تحديث `PaymentMapper.js`, `PaymentService.js`, `PaymentStore.js`, `PaymentRepositories.js`, `PaymentSchema.js` ومطابقة route/verb/payload.
- استخدام القوائم المرقمة؛ بعد الدفع إعادة جلب ملخص الاشتراك والفاتورة بدلاً من الحساب المحلي.

## 4. خطة UI/UX ونمط الاختيار
- عند فتح الدفع من فاتورة تكون الفاتورة سياقاً ثابتاً؛ خارج ذلك اختيار الفواتير المستحقة بـ GenericSearchByFilter وEndpoint Gridify.
- المبلغ لا يتجاوز المتبقي؛ PaymentMethod Standard Select. Edit Mode يعرض الفاتورة والمرجع، والتصحيح عبر شاشة عكس موثقة.

## 5. تطبيق توجيهات المستخدم
- Payment يتبع Invoice ثم StudentAcademicEnrollment. الطالب يقرأ السجل فقط؛ الدفع والعكس للمدير/موظف مخول.
- لا DomainEvent لتحريك الرصيد أو حالة الفاتورة؛ كل انتقال صريح ضمن Application transaction.
