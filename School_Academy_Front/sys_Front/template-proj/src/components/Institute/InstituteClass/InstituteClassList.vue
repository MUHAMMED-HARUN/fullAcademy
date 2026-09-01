<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الصف</th>
          <th>مستوى الصف</th>
          <th>المعهد</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="InstituteClass in instituteClasses"
          :key="InstituteClass.instituteClassID"
        >
          <td>{{ InstituteClass.className }}</td>
          <td>{{ InstituteClass.classLevel }}</td>
          <td>{{ InstituteClass.instituteName }}</td>
          <td>
            <button @click="ShowInfoInstituteClass(InstituteClass)">
              معلومات
            </button>
            <button @click="ShowDeleteInstituteClass(InstituteClass)">
              حذف
            </button>
            <button @click="ShowAddGroup(InstituteClass)">انشاء مجموعة</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "InstituteClassList",
  props: {
    instituteClasses: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowInfoInstituteClass", "ShowDeleteInstituteClass", "ShowAddGroup"],
  methods: {
    async ShowDeleteInstituteClass(Data) {
      this.$emit("ShowDeleteInstituteClass", Data);
    },
    async ShowInfoInstituteClass(Data) {
      this.$emit("ShowInfoInstituteClass", Data);
    },
    async ShowAddGroup(Data) {
      this.$emit("ShowAddGroup", Data);
    },
  },
};
</script>
