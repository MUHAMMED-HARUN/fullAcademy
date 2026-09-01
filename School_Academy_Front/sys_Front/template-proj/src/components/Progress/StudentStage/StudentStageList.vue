<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>اسم المرحلة</th>
          <th>الشعبة</th>
          <th>حالة</th>
          <th>تاريخ التسجيل</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="studentStage in StudentStages"
          :key="studentStage.assignmentScopeInfoID"
        >
          <td>{{ studentStage.studentName }}</td>
          <td>{{ studentStage.stageName }}</td>
          <td>{{ studentStage.groupName }}</td>
          <td>{{ studentStage.studentStageState }}</td>
          <td>{{ studentStage.studentStageDate }}</td>
          <td>
            <button @click="ShowDelete(studentStage)">حذف</button>
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
  },
  emits: ["ShowDelete"],
  methods: {
    async ShowDelete(Data) {
      this.$emit("ShowDelete", Data);
    },
  },
};
</script>
