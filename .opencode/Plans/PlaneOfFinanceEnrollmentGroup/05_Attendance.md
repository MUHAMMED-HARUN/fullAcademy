# خطة خدمة Attendance

## 1. العيوب المكتشفة حالياً
- `StudentEnrollmentView.vue` يرسل `EnrollmentID` كـ `AttendeeID` و`GroupID` كسياق؛ يجب تثبيت أن الحضور يتعلق بعضوية Group لا اشتراك الطالب المالي.
- `AttendancesController` بلا `[Authorize]` أو Policy ظاهرة. `clsAttendance.AttendeeID` polymorphic ولا يحمل FK صريحاً لعضوية Group.
- `NewAttendanceCommandHandler` يحدث الحضور اليومي بالـ AttendeeID مفتاحاً فقط، وقد يدمج سجلات سياقات إذا كان للعضوية أكثر من سياق.

## 2. خطة Backend
- إبقاء الحضور حصراً لعضوية Group النشطة (`StudentEnrollment`)، لا لـ `StudentAcademicEnrollment` المجرد أو مادة شهادة بلا Group.
- توثيق عقد `NewAttendanceCommand.Attendances`: `EntityID` هو عضوية المجموعة، و`ContextID` هي Group. التحقق أن المعلم مكلف بالمجموعة أو Admin.
- إضافة Policies `Can_Read_Attendance`, `Can_Create_Attendance`, `Can_Update_Attendance` إلى `AttendancesController` وتحديد سياسة الطلب المجمع.
- تحسين مفتاح upsert ليشمل عضوية المجموعة + التاريخ + السياق، وإضافة نطاق مركزي قبل إدخال/تعديل Attendance.

## 3. خطة منطق Frontend
- تحديث `AttendanceMapper.js`, `AttendanceService.js`, `AttendanceStore.js`, `AttendanceRepositories.js`, `AttendanceSchema.js` إذا تغيرت هوية العضوية.
- تعديل `StudentEnrollmentView.vue` لإرسال Group membership ID فقط؛ لا تظهر عملية Attendance لاشتراك StageInfo منفرد.

## 4. خطة UI/UX ونمط الاختيار
- الحضور يبدأ من Group المحددة وقائمة عضوياتها؛ لا اختيار طالب عام خارج المجموعة.
- تحميل/empty/error حسب الشعبة واليوم. Edit Mode يثبت Group والسياق الصحيح.

## 5. تطبيق توجيهات المستخدم
- Attendance وظيفة للشعبة فقط. شراء الخطة أو تسديدها لا يسجل حضوراً ولا يخلق Group.
- لا DomainEvent؛ التسجيل أمر Application صريح.
