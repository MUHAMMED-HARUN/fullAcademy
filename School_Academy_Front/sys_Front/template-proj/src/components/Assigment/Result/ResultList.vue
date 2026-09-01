<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>نطاق الاختبار</th>
          <th>المادة</th>
          <th>النتيجة</th>
          <th>ملاحظات</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="res in Results" :key="res.assignmentScopeInfoID">
          <td>{{ res.studentName }}</td>
          <td>{{ res.assignmentScopeName }}</td>
          <td>{{ res.stageInfoMatter }}</td>
          <td>{{ res.score }}</td>
          <td>{{ res.notes }}</td>
          <td>
            <button @click="ShowUpdate(res)">تحديث</button>
            <button @click="ShowDelete(res)">حذف</button>
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
    Results: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdate", "ShowDelete"],
  methods: {
    async ShowUpdate(Data) {
      this.$emit("ShowUpdate", Data);
    },

    async ShowDelete(Data) {
      this.$emit("ShowDelete", Data);
    },
  },
};
</script>
