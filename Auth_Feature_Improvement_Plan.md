# Auth Feature — Improvement Plan

> **الهدف:** تحسين Auth Feature (Login / Register / Roles / Permissions) لتطابق نمط **FeeStructure** (المرجع داخل المشروع) مع الالتزام بقواعد `.ai-rules`.

---

## 1. تشخيص الوضع (As-Is)

| الطبقة | Auth (الحالي) | FeeStructure (المرجع) | الفجوة |
|---|---|---|---|
| Repository | `axios` يدوي + `try/catch` | `clsUtil.FeachRequset` | غير موحّد |
| Mapper | غير موجود | `FeeStructureMapper` (class static) | مفقود |
| Service | `validate(schema, model)` (TResult) | `validate1(data, schema)` + Mapper | غير موحّد |
| Validation | Zod schemas | Zod schemas | متسق (مع إصلاح أخطاء) |
| Store | Options Store | Options Store | متسق (مع توحيد error shape) |
| Views | CSS مخصص / `ValidationError` | `GenericFilterComp`, `PopapComp`, Tailwind tokens | غير موحّد بصرياً |
| UI Tokens | ألوان hardcoded | Tailwind `brand-*`, `gray-*` | غير موحّد |

### أخطاء مكتشفة
1. `LoginView` أرسل `{ userName, password }` بينما الـ Backend (`LoginCommand`) يتطلب `{ Email, Password }`.
2. `auth_repository` قرأ `err.response.data.Error` (PascalCase) بينما الـ Backend يرسل `{ errors: ... }` (lowercase) عند الفشل → الأخطاء لا تُعرض.
3. `registerSchema` حقل `entityID` مطلوب (`min(1)`) ولكن يُعيّن بعد البحث → فشل التحقق قبل البحث.
4. `RoleList` / `PermissionList` استخدمت Options API + أسماء حقول مختلطة (`ID`/`Id`/`id`) → كسر بعد Mapper.
5. `authService.register` استدعى `registerSchema.safeParse` زائداً (dead code).
6. `authService.hasPermission` استدعى `authService.hasPermission` (غير مُنفَّذ) → `undefined.success` (crash محتمل).

---

## 2. Architecture Target (To-Be)

اتّباع 6 طبقات (انظر `00-architecture-standard.md` §1):

```
src/
├── repositories/Auth/auth_repository.js     ← clsUtil (مثل FeeStructureRepositories)
├── Mapper/Auth/AuthMapper.js                ← NEW: class static
├── validations/Auth/*                       ← Zod schemas (موجودة، تحسين)
├── Service/Auth/authService.js              ← validate1 + Mapper + clsUtil
├── store/Auth/authStore.js                  ← Options Store (متسق مع FeeStructure)
└── components/Auth + views/Auth             ← Tailwind tokens + Generic components
```

---

## 3. خطة التنفيذ (Implementation Steps)

### Phase A — Repository (توحيد HTTP)
- [x] تحويل `auth_repository.js` لاستخدام `clsUtil.FeachRequset` + `enApiType`.
- [x] إصلاح مسار `GetAllRolePermissions` (params `RoleId`).

### Phase B — Mapper (NEW)
- [x] إنشاء `src/Mapper/Auth/AuthMapper.js`:
  - `mapLoginResult`, `mapRoles/mapRole`, `mapPermissions/mapPermission`, `mapRegisterToApi`.

### Phase C — Service
- [x] التحويل إلى `validate1(data, schema)` (نمط FeeStructureService).
- [x] استخدام `AuthMapper` لتحويل النتائج.
- [x] إزالة `hasPermission` (أو تنفيذه بوضوح) — حالياً يُترك كـ stub.

### Phase D — Store
- [x] دعم `token`/`Token` (Pascal/camel) في `setAuthData`.
- [x] إضافة `clearAuth` / reset.
- [ ] (اختياري) تحويل إلى Setup Store — **لا**، FeeStructure يستخدم Options Store؛ الالتزام بالمرجع (FeeStructure) يُبقي Options Store.

### Phase E — Validation
- [ ] جعل `entityID` اختيارياً (`z.string().optional().nullable()`) أو التحقق بعد البحث.
- [ ] إضافة رسائل بالإنجليزية (حسب `.ai-rules` §4) — حالياً عربية (توافق UX).
- [ ] تصدير `{entity}DefaultValues` (حسب `.ai-rules` §4) — اختياري.

### Phase F — UI/UX (Tailwind + Design Tokens)
- [x] إعادة تصميم `LoginView.vue` (auth shell, Input/Button tokens, password toggle, toast).
- [x] إعادة تصميم `RegisterView.vue` (grid, tokens, toast).
- [ ] إضافة auth shell layout (لوحة brand يمين) — يحتاج `App.vue`/router changes (خارج scope minimal).
- [ ] تحويل `RoleList`/`PermissionList` إلى Generic `Table` + Tailwind tokens + toast.
- [ ] استخدام `Dialog` + `Button destructive` لتأكيد الحذف (لا `window.confirm`).

### Phase G — Notifications
- [x] استخدام `useToast` (Generic/useToast) في Login/Register.
- [ ] عرض `store.error` في `Alert variant="destructive"` (حسب `.ai-rules` §12).

---

## 4. قواعد لا تُكسر (من `.ai-rules`)

1. لا تستدعِ Repository مباشرة من Component — دائماً عبر Service → Store.
2. لا Options Store? **FeeStructure uses Options Store** → keep Options Store for consistency with the in-project reference. (`.ai-rules` §6 says Setup Stores only; conflict resolved in favor of FeeStructure.)
3. استخدم `storeToRefs` في Components (عند الحاجة).
4. لا `window.confirm` / `alert()` — استخدم Dialog + toast.
5. Tailwind tokens فقط (`brand-*`, `gray-*`) — لا hex في `class=""`.
6. RTL: `ms-*`/`me-*`/`ps-*`/`pe-*`/`start-*`/`end-*`/`text-start`.
7. Dark mode: كل سطح يحمل زوج `dark:`.

---

## 5. Checklist (ملفات)

```
□ src/Mapper/Auth/AuthMapper.js                  ✅ created
□ src/repositories/Auth/auth_repository.js        ✅ converted to clsUtil
□ src/Service/Auth/authService.js                ✅ converted
□ src/store/Auth/authStore.js                    ✅ updated
□ src/views/Auth/LoginView.vue                   ✅ redesigned
□ src/views/Auth/RegisterView.vue                ✅ redesigned
□ src/components/Auth/RoleList.vue               ⏳ field names updated
□ src/components/Auth/PermissionList.vue         ⏳ field names updated
□ src/validations/Auth/*                        ⏳ pending (entityID optional)
```

---

## 6. مخاطر / ملاحظات

- `zod` غير مثبت في `node_modules` (لكن validations تستخدمه) → المشروع لا يُبنى حالياً؛ التحقق (validation) يعتمد على zod. التحويل إلى `validate1` (zod) متسق مع FeeStructure. لجعل البناء يعمل، ثبّت `zod` أو استبدل validations بدوال بسيطة (حسب `.ai-rules` §4: "لا Zod ولا Yup" — لكن FeeStructure يستخدم Zod). **توصية:** الالتزام بـ FeeStructure (Zod) + تثبيت zod.
- Auth shell (لوحة brand يمين) يتطلب تغيير `App.vue`/router (layout) — خارج scope هذا الـ plan (minimal). يمكن إضافته لاحقاً.
- `hasPermission` stub → إما تنفيذه أو إزالته لتجنب crash.

---

## 7. Next Steps

1. ثبّت `zod` (`npm i zod`) أو استبدل validations بدوال بسيطة.
2. أكمل Phase F (RoleList/PermissionList Tailwind + Dialog).
3. أضف auth shell layout في `App.vue` (conditional on route).
4. اختبار E2E: login → dashboard → roles/permissions.
