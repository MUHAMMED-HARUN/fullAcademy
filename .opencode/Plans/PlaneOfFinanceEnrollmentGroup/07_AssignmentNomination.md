# خطة خدمة AssignmentNomination

## 1. العيوب المكتشفة حالياً
- `clsAssignmentNomination.StudentEnrollmentID` و`AssignmentNominationInfo.StudentEnrollmentID` يقيدان الترشيح بعضوية Group.
- `AssignmentNominationService.GetList/GetBy` بلا Read Scope؛ `HasNomination` يحتوي `&&/||` دون أقواس سليمة.
- `NewAssignmentNominationCommandHandler` يغير خصائص `request`، وController لا يظهر عليه تفويض سياسات.

## 2. خطة Backend
- إضافة `StudentAcademicEnrollmentID` إلى `clsAssignmentNomination` وConfiguration/Repository/DTO/Commands/Queries/Mapper/Validators.
- التحقق في `NewAssignmentNominationCommandHandler` من أن الاشتراك فعال وأن `AssignmentScopeInfo.StageInfoID` ضمن AcademicEnrollmentItems؛ لا ترشيح لمادة خارج الخطة.
- تصحيح `HasNomination` ليقيد `(StudentAcademicEnrollmentID, AssignmentScopeID)` مع IsActive/DeletedAt وحالة واضحة.
- إضافة `GetListFilter` وScope extension قبل projection؛ حماية `AssignmentNominationController` بسياسات `Can_Read/Create/Update/Delete_AssignmentNomination`.
- تعديل `GetCanNominateList` في AssignmentScope ليقبل `StudentAcademicEnrollmentID` ويتحقق من الاشتراك.

## 3. خطة منطق Frontend
- تحديث `AssignmentNominationMapper.js`, `AssignmentNominationService.js`, `AssignmentNominationStore.js`, `AssignmentNominationRepositories.js`, `AssignmentNominationSchema.js` إلى معرف الاشتراك.
- تعديل `StudentEnrollmentView.vue` لفتح الترشيح من الاشتراك المرتبط بعضوية Group، لا تمرير group membership باعتبارها حق الدراسة.

## 4. خطة UI/UX ونمط الاختيار
- اشتراك الطالب سياق ثابت؛ AssignmentScope يظهر فقط إذا كان StageInfo الخاص به ضمن الاشتراك.
- Edit Mode يعيد ID الاشتراك والنطاق والاسم عبر `get-for-upsert`; حالات no eligible scope/loading/error واضحة.

## 5. تطبيق توجيهات المستخدم
- الطالب يقرأ ترشيحاته؛ المعلم يرى طلاب مجموعاته المخولين؛ Admin كامل. ApplyReadScope في Infrastructure.
- لا DomainEvent لإنشاء الترشيح أو تغيير حالته.
