<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم المعلم</th>
          <th>اسم المجموعة</th>
          <th>اسم المادة</th>
          <th>تاريخ التسجيل</th>
          <th>العمليات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="enroll in enrollments" :key="enroll.enrollmentID">
          <td>{{ enroll.teacherName }}</td>
          <td>{{ enroll.groupName }}</td>
          <td>{{ enroll.subjectName }}</td>
          <td>{{ enroll.date }}</td>
          <td>
            <button @click="ShowInfoEnrollment(enroll)">معلومات</button>
            <button @click="ShowUpdateEnrollment(enroll)">تحديث</button>
            <button @click="ShowDeleteEnrollment(enroll)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "TeachingAssignmentList",
  props: {
    enrollments: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateEnrollment", "ShowDeleteEnrollment", "ShowInfoEnrollment"],
  methods: {
    async ShowUpdateEnrollment(Data) {
      this.$emit("ShowUpdateEnrollment", Data);
    },
    async ShowDeleteEnrollment(Data) {
      this.$emit("ShowDeleteEnrollment", Data);
    },
    async ShowInfoEnrollment(Data) {
      this.$emit("ShowInfoEnrollment", Data);
    },
  },
};
</script>
