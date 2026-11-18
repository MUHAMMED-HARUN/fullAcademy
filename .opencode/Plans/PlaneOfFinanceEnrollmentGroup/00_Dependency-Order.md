# ترتيب الاعتماديات — Academic Plans, Enrollment, Finance, Group

## القرار المعماري
- المواد داخل البرنامج/الصف/المرحلة إلزامية؛ لا اختيار اختياري لمادة ضمن الخطة.
- `AcademicPlan` هو المنتج الدراسي: Program أو صف مدرسة (`InstituteClass`) أو Stage أو مادة واحدة (`StageInfo`). `StudentAcademicEnrollment` يمنح حق الوصول ولا يساوي عضوية Group.
- `Group` تبقى شعبة تعليمية، وعضوية الطالب فيها سجل منفصل. اشتراك برنامج قد يرتبط بعضويات شعب متعددة عبر الزمن؛ لا تنشأ Group وهمية للمادة المنفردة.
- `StudentInvoice` يرتبط بالاشتراك الأكاديمي، لا بـ `StudentID` وحده. يمنع استخدام DomainEvent لتنفيذ سير العمل.

## ترتيب التنفيذ
1. **SecurityAndPermissions** — اعتماد الصلاحيات والنطاقات قبل فتح Endpoints جديدة.
2. **AcademicPlan** — تعريف المنتج ومحتواه الإلزامي.
3. **StudentAcademicEnrollment** — اشتراك الطالب ونسخة المحتوى؛ يعتمد على AcademicPlan.
4. **Group** — ربط العضويات بالاشتراك دون تحويل Group إلى خطة مالية.
5. **Attendance** — إبقاء الحضور تابعاً لعضوية Group.
6. **Progress** — ربط StudentStage وDailyEvaluation بمحتوى الاشتراك.
7. **AssignmentNomination** — نقل مرجع الطالب إلى الاشتراك الأكاديمي.
8. **Assignment** — ضبط إنشاء الاختبارات ضمن نطاق الاشتراك بلا أحداث منطقية.
9. **Result** — نتيجة StageInfo المسموحة.
10. **FeeStructure** — ربط التعرفة بالخطة.
11. **StudentInvoice** — الفاتورة من اشتراك مؤهل، مع منع التكرار.
12. **Payment** — الدفعات والعكس والمتابعة.

## الرسم المستهدف
```text
AcademicPlan 1 ── N AcademicPlanItem (StageInfoID, نسخة المحتوى)
Student 1 ── N StudentAcademicEnrollment N ── 1 AcademicPlan
StudentAcademicEnrollment 1 ── N StudentEnrollment (عضوية Group)
Group 1 ── N StudentEnrollment
AcademicPlan 1 ── N FeeStructure
StudentAcademicEnrollment 1 ── N StudentInvoice 1 ── N Payment
```
`Program → Class → Stage → StageInfo` مصدر المواد؛ الصف المدرسي الفعلي يتحدد عبر `InstituteClass`.

## بوابة التنفيذ
يبدأ التنفيذ بالخدمة 01 فقط بعد مراجعة هذه الخطط. تتوقف كل مرحلة لمراجعة المستخدم ونجاح `dotnet build` و`ESLint`. أي Migration أو تغيير عقد مشترك يحتاج موافقة مسبقة.
