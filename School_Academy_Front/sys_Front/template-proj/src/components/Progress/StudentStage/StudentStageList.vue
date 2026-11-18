<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>اسم المرحلة</th>
          <th>الخطة الأكاديمية</th>
          <th>حالة</th>
          <th>تاريخ التسجيل</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="studentStage in StudentStages"
          :key="studentStage.studentStageID"
        >
          <td>{{ studentStage.studentName }}</td>
          <td>{{ studentStage.stageName }}</td>
          <td>{{ studentStage.academicPlanName }}</td>
          <td>{{ studentStage.studentStageState }}</td>
          <td>{{ studentStage.studentStageDate }}</td>
          <td>
            <button
              v-if="canUpdate"
              @click="$emit('show-update', studentStage)"
            >
              تحديث
            </button>
            <button v-if="canDelete" @click="ShowDelete(studentStage)">
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
  name: "StudentStageList",
  props: {
    StudentStages: {
      type: Array,
      required: true,
    },
    canDelete: { type: Boolean, default: false },
    canUpdate: { type: Boolean, default: false },
  },
  emits: ["ShowDelete", "show-update"],
  methods: {
    async ShowDelete(Data) {
      this.$emit("ShowDelete", Data);
    },
  },
};
</script>
