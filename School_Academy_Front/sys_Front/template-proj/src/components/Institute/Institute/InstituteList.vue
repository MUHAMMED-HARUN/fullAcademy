<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم المعهد</th>
          <th>عنوان المعهد</th>
          <th>ملاحظات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="institute in institutes" :key="institute.instituteID">
          <td>{{ institute.name }}</td>
          <td>{{ institute.address }}</td>
          <td>{{ institute.note }}</td>

          <td>
            <button @click="ShowInfoInstitutes(institute)">معلومات</button>
            <button @click="ShowUpdateInstitutes(institute)">تحديث</button>
            <button @click="ShowDeleteInstitutes(institute)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "InstituteList",
  props: {
    institutes: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateInstitutes", "ShowDeleteInstitutes", "ShowInfoInstitutes"],
  methods: {
    async ShowUpdateInstitutes(Data) {
      this.$emit("ShowUpdateInstitutes", Data);
    },

    async ShowDeleteInstitutes(Data) {
      this.$emit("ShowDeleteInstitutes", Data);
    },
    async ShowInfoInstitutes(Data) {
      this.$emit("ShowInfoInstitutes", Data);
    },
  },
};
</script>
