# خطة خدمة Auth

**الترتيب:** 9 من 9 — تعتمد على Identity والأدوار، ويرتبط التسجيل بـPerson/Student/Teacher عبر نوع الكيان ومعرّفه.

## 1. العيوب المكتشفة

- `AuthController.Login` يعيد `Ok(result)` دون معالجة HTTP واضحة للفشل، بينما `LoginCommandHandler` يرمي exception عند بيانات اعتماد غير صحيحة.
- يوجد Validatorان لـLogin (`LoginCommandValidator` و`LoginValidator`) بقواعد غير موحدة. FluentValidation يطلب كلمة مرور 8 أحرف وحرفاً كبيراً، بينما Zod والواجهة يطلبان 6.
- `RegisterCommandValidator` لا يتحقق من `RoleID`, `UserType`, `EntityID`، و`RegisterCommandHandler` يتجاهل نتيجة `SetUserRole`.
- `AuthDTOInfo` لا يعيد الدور/الكيان/الصلاحيات التي يتوقع بعضها `AuthMapper`؛ `authStore.permissions` لا تُملأ.
- `UpdateRolePermissions` بلا Authorization والـpolicy معلّق. منطق `RoleList.vue` لإسناد `isDelete` يبدو معكوساً، و`PermissionList.vue` لا يحسب تغييرات الإضافة والحذف بشكل موثوق.
- Validators موزعة تحت `Validator` و`Validator` namespaces غير مناسبة، و`GetRolePermissionQueryValidator` ضمن namespace Login.
- التسجيل العام يسمح للعميل باختيار Role وUserType وEntityID؛ يلزم تحديد سياسة أمان لهذا العقد.

## 2. خطة Backend

- مراجعة `Controllers/AuthController.cs` لمسارات login/register/permissions/roles/role-permissions ومطابقة status codes وpayloads.
- توحيد Login validator وإزالة الازدواج بين `Login/Command/LoginValidator.cs` و`Login/Validator/LoginCommandValidator.cs`.
- تقوية `RegisterCommandValidator.cs` لقواعد الدور ونوع الكيان والمعرّف، ومراجعة `RegisterCommandHandler.cs` لضمان فشل/تراجع العملية عند فشل تعيين الدور.
- مراجعة `UpdateRolePermissionsCommandHandler.cs` وvalidators الخاصة به، والتحقق من نتائج `AddClaims/DeleteClaims` وعدم إعلان نجاح جزئي.
- توحيد مخرجات `LoginDto`, `AuthDTOInfo`, `LoginProfile` مع مصدر واحد للـrole/entity/permissions (DTO أو JWT claims)، وحماية endpoints الإدارية على الخادم.
- نقل/توحيد Validators المتأثرة إلى مجلدات `Validations` وفق معيار المشروع. مراجعة `ApplicationUser`, `ApplicationRole`, `IdentityService`, `JwtTokenGeneratorService` وإعدادات JWT/policies في `Program.cs`.

## 3. خطة منطق Frontend

- توحيد `auth_repository.js`, `AuthMapper.js`, `authService.js`, `authStore.js` حول عقد Login/Register/Roles/Permissions، وتعبئة permissions فعلياً.
- مواءمة `login.js`, `registerSchema.js`, `UpdateRolePermissionsSchema.js`, `getAllRolePermissionsSchema.js` مع FluentValidation، بما فيها ConfirmPassword وحقول نوع الكيان إذا بقيت ضمن العقد.
- إصلاح حساب فرق الصلاحيات بحيث ترسل عمليات الإضافة والحذف الفعلية فقط، والتحقق من نتيجة التحديث قبل إغلاق الواجهة.
- تحديد عقد موحد لحقول الدور/الكيان وعدم الاعتماد على احتمالات casing متعددة لإخفاء اختلاف DTO.

## 4. خطة UI/UX ونمط الاختيار

- مراجعة `LoginView.vue`, `RegisterView.vue`, `UpsertUserComp.vue`, `RoleList.vue`, `PermissionList.vue`.
- Role اختيار **Standard Select**. ربط الحساب بكيان كبير يستخدم GenericSearch خادمي عند الحاجة؛ يزامن ID والاسم والبطاقة، ويمسحها معاً عند التغيير أو الخطأ.
- توحيد التحقق المرئي ورسائل الخادم والتحميل، وتحسين RTL والهوية البصرية وفق `Main.css`.
- Frontend route guards تحسين UX فقط؛ ليست بديلاً عن Authorization في API.

## 5. Gridify والبوابة

- إن وُجد lookup كبير لـPerson/Student/Teacher، يجب أن يكون Gridify server-side مع Allowlist وPageSize محدود. Contains بصيغة `Field=*Value` دون نجمة ختامية؛ AND `,` وOR `|` و`/i` عند الحاجة.
- لا يبدأ Auth قبل اكتمال الخدمات السابقة ومراجعتها. يتطلب أي تغيير لعقد التسجيل العام أو سياسات Authorization قراراً صريحاً قبل التنفيذ.
