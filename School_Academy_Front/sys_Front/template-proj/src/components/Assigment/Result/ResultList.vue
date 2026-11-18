<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الطالب</th>
          <th>نطاق الاختبار</th>
          <th>المادة</th>
          <th>النتيجة</th>
          <th>الحد الأعلى</th>
          <th>ملاحظات</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="res in Results" :key="res.resultID">
          <td>{{ res.studentName }}</td>
          <td>{{ res.assignmentScopeName }}</td>
          <td>{{ res.stageInfoMatter }}</td>
          <td>{{ res.score }}</td>
          <td>{{ res.maxScore }}</td>
          <td>{{ res.notes }}</td>
          <td>
            <button v-if="canUpdate" @click="ShowUpdate(res)">تحديث</button>
            <button v-if="canDelete" @click="ShowDelete(res)">حذف</button>
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
    canUpdate: { type: Boolean, default: false },
    canDelete: { type: Boolean, default: false },
  },
  emits: ["show-update", "show-delete"],
  methods: {
    async ShowUpdate(Data) {
      this.$emit("show-update", Data);
    },

    async ShowDelete(Data) {
      this.$emit("show-delete", Data);
    },
  },
};
</script>
