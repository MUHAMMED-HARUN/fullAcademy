<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم مرحلة الاختبار</th>
          <th>المرحلة</th>
          <th>المرحلة السابقة</th>
          <th>ملاحظات</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="assigScope in AssignmentScopes"
          :key="assigScope.assignmentScopeID"
        >
          <td>{{ assigScope.name }}</td>
          <td>{{ assigScope.stageName }}</td>
          <td>{{ assigScope.prevAssignmentScopeName }}</td>
          <td>{{ assigScope.notes }}</td>

          <td>
            <button @click="ShowInfo(assigScope)">معلومات</button>
            <button @click="ShowUpdate(assigScope)">تحديث</button>
            <button @click="ShowDelete(assigScope)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "AssignmentScopeList",
  props: {
    AssignmentScopes: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdate", "ShowDelete", "ShowInfo"],
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
  },
};
</script>
