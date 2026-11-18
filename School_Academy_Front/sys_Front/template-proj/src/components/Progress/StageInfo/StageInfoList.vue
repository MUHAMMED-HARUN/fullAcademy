<template>
  <div class="table-responsive" dir="rtl">
    <table class="data-table">
      <thead>
        <tr>
          <th>اسم المرحلة</th>
          <th>اسم المادة</th>
          <th>من</th>
          <th>الى</th>
          <th>نوع الوحدة</th>
          <th>نوع العملية</th>
          <th>الإجراءات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="stageinfo in stageInfos" :key="stageinfo.stageInfoID">
          <td>{{ stageinfo.stageName }}</td>
          <td>{{ stageinfo.matterName }}</td>
          <td>{{ stageinfo.from }}</td>
          <td>{{ stageinfo.to }}</td>
          <td>{{ stageinfo.uniteTypeName }}</td>
          <td>{{ stageinfo.progressTypeName }}</td>
          <td>
            <button
              v-if="canRead"
              type="button"
              class="btn btn-sm btn-outline-secondary"
              @click="$emit('show-info', stageinfo)"
            >
              معلومات
            </button>
            <button
              v-if="canUpdate"
              type="button"
              class="btn btn-sm btn-outline-primary"
              @click="$emit('show-update', stageinfo)"
            >
              تحديث
            </button>
            <button
              v-if="canDelete"
              type="button"
              class="btn btn-sm btn-outline-danger"
              @click="$emit('show-delete', stageinfo)"
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
  name: "StageInfoList",
  props: {
    stageInfos: {
      type: Array,
      required: true,
    },
    canRead: { type: Boolean, default: false },
    canUpdate: { type: Boolean, default: false },
    canDelete: { type: Boolean, default: false },
  },
  emits: ["show-update", "show-delete", "show-info"],
};
</script>
