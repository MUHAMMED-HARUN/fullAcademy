<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>المرحلة الاختبار</th>
          <th>المادة عنصر الاختبار</th>
          <th>العلامة التامة</th>
          <th>الحد الادنى</th>
          <th>الوزن</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="assigScopeInfo in AssigmentScopeInfos"
          :key="assigScopeInfo.assignmentScopeInfoID"
        >
          <td>{{ assigScopeInfo.assignmentScopeName }}</td>
          <td>{{ assigScopeInfo.stageInfoMatterNameName }}</td>
          <td>{{ assigScopeInfo.maxScore }}</td>
          <td>{{ assigScopeInfo.minPassScore }}</td>
          <td>{{ assigScopeInfo.weight }}</td>
          <td>
            <button @click="ShowInfo(assigScopeInfo)">معلومات</button>
            <button @click="ShowUpdate(assigScopeInfo)">تحديث</button>
            <button @click="ShowDelete(assigScopeInfo)">حذف</button>
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
    AssigmentScopeInfos: {
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
