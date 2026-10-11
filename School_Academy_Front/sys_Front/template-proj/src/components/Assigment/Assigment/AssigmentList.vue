<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>نطاق الامتحان</th>
          <th>حالة الامتحان</th>
          <th>تاريخ التنفيذ</th>
          <th>نتيجة الامتحان</th>
          <th>نتيجة</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="assig in Assigments" :key="assig.assignmentID">
          <td>{{ assig.studentName }}</td>
          <td>{{ assig.assigmentScopeName }}</td>
          <td>{{ assig.assignmentState }}</td>
          <td>{{ assig.scheduledDate }}</td>
          <td>{{ assig.isPassed }}</td>
          <td>{{ assig.totalScore }}</td>
          <td>
            <button @click="ShowInfo(assig)">معلومات</button>
            <button @click="ShowUpdate(assig)">تحديث</button>
            <button @click="ShowDelete(assig)">حذف</button>
            <button v-if="assig.canAddResult" @click="showAddResult(assig)">
              اضافة نتيجة
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "AssigmentList",
  props: {
    Assigments: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdate", "ShowDelete", "ShowInfo", "ShowAddResult"],
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
    async showAddResult(Data) {
      this.$emit("ShowAddResult", Data);
    },
  },
};

/*
الفكرة هي انه يفتح امر اختبار من الترشيح
وفي قائمة الاختبارات المفتوحة يقوم باضافة نتيحة خاصة بهذا الاختبار

*/
</script>
