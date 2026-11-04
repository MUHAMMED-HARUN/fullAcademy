<template>
  <div class="table-responsive" dir="rtl">
    <table class="data-table">
      <thead>
        <tr>
          <th>اسم المادة</th>
          <th>نوع المادة</th>
          <th>المادة الرئيسية</th>
          <th>ملاحظات</th>
          <th>الإجراءات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="session in sessions" :key="session.sessionID">
          <td>{{ session.name }}</td>
          <td>{{ session.typeName }}</td>
          <td>{{ session.parentSessionName }}</td>
          <td>{{ session.note }}</td>

          <td>
            <button
              v-if="canCreate && session.type === 0"
              type="button"
              class="btn btn-sm btn-primary"
              @click="$emit('add-child', session)"
            >
              إضافة ابن
            </button>
            <button
              v-if="canRead"
              type="button"
              class="btn btn-sm btn-outline-secondary"
              @click="$emit('show-info', session)"
            >
              معلومات
            </button>
            <button
              v-if="canUpdate"
              type="button"
              class="btn btn-sm btn-outline-primary"
              @click="$emit('show-update', session)"
            >
              تحديث
            </button>
            <button
              v-if="canDelete"
              type="button"
              class="btn btn-sm btn-outline-danger"
              @click="$emit('show-delete', session)"
            >
              حذف
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "SessionList",
  props: {
    sessions: {
      type: Array,
      required: true,
    },
    canRead: { type: Boolean, default: false },
    canCreate: { type: Boolean, default: false },
    canUpdate: { type: Boolean, default: false },
    canDelete: { type: Boolean, default: false },
  },
  emits: ["show-update", "show-delete", "show-info", "add-child"],
};
</script>
