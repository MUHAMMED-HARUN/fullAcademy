<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>الخطة الأكاديمية</th>
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
          <td>{{ assig.academicPlanName }}</td>
          <td>{{ assig.assigmentScopeName }}</td>
          <td>{{ assig.assignmentState }}</td>
          <td>{{ assig.scheduledDate }}</td>
          <td>{{ assig.isPassed }}</td>
          <td>{{ assig.totalScore }}</td>
          <td>
            <button v-if="canRead" @click="ShowInfo(assig)">معلومات</button>
            <button v-if="canUpdate" @click="ShowUpdate(assig)">تحديث</button>
            <button v-if="canDelete" @click="ShowDelete(assig)">حذف</button>
            <button
              v-if="canAddResult && assig.canAddResult"
              @click="showAddResult(assig)"
            >
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
    canRead: { type: Boolean, default: false },
    canUpdate: { type: Boolean, default: false },
    canDelete: { type: Boolean, default: false },
    canAddResult: { type: Boolean, default: false },
  },
  emits: ["show-update", "show-delete", "show-info", "show-add-result"],
  methods: {
    async ShowUpdate(Data) {
      this.$emit("show-update", Data);
    },

    async ShowDelete(Data) {
      this.$emit("show-delete", Data);
    },
    async ShowInfo(Data) {
      this.$emit("show-info", Data);
    },
    async showAddResult(Data) {
      this.$emit("show-add-result", Data);
    },
  },
};

/*
الفكرة هي انه يفتح امر اختبار من الترشيح
وفي قائمة الاختبارات المفتوحة يقوم باضافة نتيحة خاصة بهذا الاختبار

*/
</script>
