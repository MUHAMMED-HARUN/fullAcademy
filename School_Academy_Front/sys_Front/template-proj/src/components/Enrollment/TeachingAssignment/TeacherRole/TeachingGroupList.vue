<template>
  <div>
    <div v-for="enroll in enrollments" :key="enroll.enrollmentID">
      <div>
        <label>المادة</label>
        <label>{{ enroll.subjectName }}</label>
      </div>
      <div>
        <label>المجموعة</label>
        <label>{{ enroll.groupName }}</label>
      </div>
      <div>
        <label>الصف</label>
        <label>{{ enroll.calssName }}</label>
      </div>
      <div>
        <label>المستوى</label>
        <label>{{ enroll.classLevel }}</label>
      </div>
      <div>
        <button @click.prevent.stop="ShowStudentGroup(enroll)">
          عرض الطلاب
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "TeachingGroupList",
  props: {
    enrollments: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowStudentGroup"],
  methods: {
    async ShowStudentGroup(Data) {
      this.$emit("ShowStudentGroup", Data);
    },
  },
};
</script>
