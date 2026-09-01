<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>اسم المجموعة</th>
          <th>تاريخ التسجيل</th>
          <th>العمليات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="enroll in enrollments" :key="enroll.enrollmentID">
          <td>{{ enroll.studentName }}</td>
          <td>{{ enroll.groupName }}</td>
          <td>{{ enroll.date }}</td>
          <td>
            <button
              v-if="enroll.canNominate"
              @click="ShowNewAssigmentNomination(enroll)"
            >
              اضافة ترشيح
            </button>
            <button
              v-if="enroll.canTrack"
              @click="ShowNewDailyEvaluation(enroll)"
            >
              اضافة متابعة
            </button>
            <button
              v-if="enroll.canAddToStage"
              @click="ShowNewStudentStage(enroll)"
            >
              اضافة الى مرحلة
            </button>
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
  data() {
    return {};
  },
  name: "StudentEnrollmentList",
  props: {
    enrollments: {
      type: Array,
      required: true,
    },
  },
  emits: [
    "ShowUpdateEnrollment",
    "ShowNewDailyEvaluation",
    "ShowDeleteEnrollment",
    "ShowInfoEnrollment",
    "ShowNewAssigmentNomination",
    "ShowNewStudentStage",
  ],
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
    async ShowNewDailyEvaluation(Data) {
      this.$emit("ShowNewDailyEvaluation", Data);
    },
    async ShowNewAssigmentNomination(Data) {
      this.$emit("ShowNewAssigmentNomination", Data);
    },
    async ShowNewStudentStage(Data) {
      this.$emit("ShowNewStudentStage", Data);
    },
  },
};
</script>
