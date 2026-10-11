<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>مرحلة الاختبار</th>
          <th>التاريخ</th>
          <th>حالة الترشيح</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="assigNomin in AssignmentNominations"
          :key="assigNomin.assignmentScopeInfoID"
        >
          <td>{{ assigNomin.studentName }}</td>
          <td>{{ assigNomin.assignmentScopeName }}</td>
          <td>{{ assigNomin.date }}</td>
          <td>{{ assigNomin.nominationState }}</td>
          <td>
            <button @click="ShowInfo(assigNomin)">معلومات</button>
            <button @click="ShowUpdate(assigNomin)">تحديث</button>
            <button @click="ShowDelete(assigNomin)">حذف</button>
            <button
              v-if="assigNomin.canStartAssigment"
              @click="ShowNewAssignment(assigNomin)"
            >
              ابدا الاختبار
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "AssigmentScopeInfoList",
  props: {
    AssignmentNominations: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdate", "ShowDelete", "ShowInfo", "ShowNewAssignment"],
  methods: {
    async ShowUpdate(Data) {
      this.$emit("ShowUpdate", Data);
    },

    async ShowDelete(Data) {
      this.$emit("ShowDelete", Data);
    },
    async ShowInfo(Data) {
      this.$emit("ShowInfo", Data);
    },
    async ShowNewAssignment(Data) {
      this.$emit("ShowNewAssignment", Data);
    },
  },
};
</script>
