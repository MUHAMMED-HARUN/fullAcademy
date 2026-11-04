<template>
  <div class="table-responsive" dir="rtl">
    <table class="data-table">
      <thead>
        <tr>
          <th>اسم المرحلة</th>
          <th>الصف</th>
          <th>المرحلة السابقة</th>
          <th>ملاحظات</th>
          <th>الإجراءات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="stage in stages" :key="stage.stageID">
          <td>{{ stage.name }}</td>
          <td>{{ stage.className }}</td>
          <td>{{ stage.prevStageName || "—" }}</td>
          <td>{{ stage.note }}</td>

          <td>
            <button
              v-if="canCreate"
              type="button"
              class="btn btn-sm btn-primary"
              @click="$emit('add-stage-info', stage)"
            >
              إضافة تفاصيل
            </button>
            <button
              v-if="canRead"
              type="button"
              class="btn btn-sm btn-outline-secondary"
              @click="$emit('show-info', stage)"
            >
              معلومات
            </button>
            <button
              v-if="canUpdate"
              type="button"
              class="btn btn-sm btn-outline-primary"
              @click="$emit('show-update', stage)"
            >
              تحديث
            </button>
            <button
              v-if="canDelete"
              type="button"
              class="btn btn-sm btn-outline-danger"
              @click="$emit('show-delete', stage)"
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
  name: "StageList",
  props: {
    stages: {
      type: Array,
      required: true,
    },
    canRead: { type: Boolean, default: false },
    canCreate: { type: Boolean, default: false },
    canUpdate: { type: Boolean, default: false },
    canDelete: { type: Boolean, default: false },
  },
  emits: ["show-update", "show-delete", "show-info", "add-stage-info"],
};
</script>
