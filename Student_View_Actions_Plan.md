# Student View — Actions Combo Plan

> **الهدف:** نقل الإجراءات الموجودة في صفوف `StudentList` إلى `GenericComboActions` (على نمط Person)، وإضافة إجراء `UpsertStudentEnrollmentComp` (تسجيل طالب) إلى إجراءات Student View.

---

## 1. الوضع الحالي (As-Is)

- `StudentList.vue` يحتوي أزرار per-row: معلومات / تحديث / حذف.
- `StudentView.vue` لا يستخدم `GenericComboActions` ولا حالة تحديد (selection).
- لا يوجد إجراء لفتح `UpsertStudentEnrollmentComp` من Student View.

## 2. الهدف (To-Be)

1. إزالة/إبقاء أزرار الصف (اختياري) ونقل الإجراءات إلى `GenericComboActions`.
2. إضافة حالة تحديد (`selectedStudentIds`) + `selectedStudents` computed.
3. إضافة إجراءات:
   - `info` (معلومات)
   - `update` (تحديث)
   - `delete` (حذف)
   - `enrollStudent` (تسجيل طالب في مجموعة) → يفتح `UpsertStudentEnrollmentComp`
4. عند `enrollStudent`: تعبئة Student Data من الطالب المحدد + السماح باختيار Group عبر SearchFilterCombo.

---

## 3. Frontend Changes

### 3.1 StudentView.vue
- استيراد `GenericComboActions` و `UpsertStudentEnrollmentComp` و `PopapComp`.
- إضافة state:
  - `selectedStudentIds: []`
  - `showUpsertEnrollment: false`
- computed:
  - `selectedStudents` (filter StudentStore.students by selectedStudentIds)
- `actions` list:
  ```js
  [
    { key: 'info', label: 'معلومات', minSelection: 1, maxSelection: 1 },
    { key: 'update', label: 'تحديث', minSelection: 1, maxSelection: 1 },
    { key: 'delete', label: 'حذف', minSelection: 1, class: 'bg-red-500 hover:bg-red-600' },
    { key: 'enrollStudent', label: 'تسجيل طالب', minSelection: 1, maxSelection: 1, class: 'bg-brand-500 hover:bg-brand-600' },
  ]
  ```
- `handleComboAction(actionKey, selectedItems)`:
  - `info` → `onShowInfo(singleItem)`
  - `update` → `OnUpsertPerson(singleItem)`
  - `delete` → `OnDelete(singleItem)`
  - `enrollStudent` → `OnEnrollStudent(singleItem)`
- `OnEnrollStudent(student)`:
  - `StudentStore.selectedStudent = student`
  - `GroupStore.selectedGroup = null`
  - `showUpsertEnrollment = true`
- Popup:
  ```vue
  <PopapComp v-if="showUpsertEnrollment" @close="onpopaCloseEnrollment">
    <UpsertStudentEnrollmentComp
      :errors="StudentEnrollmentStore.error"
      :student-data="StudentStore.selectedStudent"
      :group-data="GroupStore.selectedGroup"
      :group-results="GroupStore.groups"
      @on-submit="OnSubmitEnrollment"
      @on-search-student="onSearchStudent"
      @on-search-group="onSearchGroup"
    />
  </PopapComp>
  ```
- `onToggleSelectAll`, `onToggleSelectRow` (already partially) keep.
- `onSearchStudent`, `onSearchGroup` handlers (reuse from StudentEnrollmentView or add).

### 3.2 StudentList.vue
- إبقاء checkbox column + emits (already).
- Optional: إخفاء أزرار per-row أو الإبقاء. Plan: إخفاء per-row actions để unify into combo (أو الإبقاء كاختصار). التوصية: إبقاء "معلومات/تحديث" كأزرار ghost (اختياري) + combo للعمليات المجمعة. لتبسيط: الإبقاء.

### 3.3 Imports/Stores
- `useStudentEnrollmentStore`, `useGroupStore` in StudentView.
- `OnSubmitEnrollment` calls `StudentEnrollmentStore.addEnrollment(e)` then refresh.

---

## 4. File Checklist

- [ ] `src/views/Person/StudentView.vue`
- [ ] `src/components/Person/Student/StudentList.vue` (already updated)
- [ ] Reuse `src/components/Enrollment/StudentEnrollment/UpsertStudentEnrollmentComp.vue`
- [ ] Reuse `src/components/general/GenericComboActions.vue`
- [ ] Reuse `src/components/general/PopapComp.vue`

## 5. Phases

1. Add selection + combo actions to StudentView.
2. Add enrollStudent action + popup wiring.
3. Reuse onSearchStudent/onSearchGroup handlers.
4. Build & smoke test.

## 6. Risks

- `StudentStore.selectedStudent` shape must match `StudentCard` expectations (studentID/firstName/etc.) — already used in enrollment view.
- Ensure `GroupStore.groups` populated for SearchFilterCombo (call fetchGroups on open).
- Avoid duplicate handlers: reuse `onSearchGroup` from StudentEnrollmentView pattern.

## 7. Acceptance Criteria

- Selecting one student enables combo actions.
- Clicking "تسجيل طالب" opens `UpsertStudentEnrollmentComp` with Student Data filled.
- Group search uses SearchFilterCombo + GetAllGroup.
- Submitting enrollment refreshes enrollment list / shows toast.