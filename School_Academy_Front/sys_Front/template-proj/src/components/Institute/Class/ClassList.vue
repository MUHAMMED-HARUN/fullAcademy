<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم الصف</th>
          <th>مستوى الصف</th>
          <th>المشروع</th>
          <th>ملاحظات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="cls in classes" :key="cls.classID">
          <td>{{ cls.name }}</td>
          <td>{{ cls.level }}</td>
          <td>{{ cls.programName }}</td>
          <td>{{ cls.notes }}</td>

          <td>
            <button @click="ShowInfoClass(cls)">معلومات</button>
            <button @click="ShowUpdateClass(cls)">تحديث</button>
            <button @click="ShowDeleteClass(cls)">حذف</button>
            <button @click="ShowAddToInstitueClass(cls)">اضافة الى معهد</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "ClassList",
  props: {
    classes: {
      type: Array,
      required: true,
    },
  },
  emits: [
    "ShowUpdateClass",
    "ShowDeleteClass",
    "ShowInfoClass",
    "ShowAddToInstitueClass",
  ],
  methods: {
    async ShowUpdateClass(Data) {
      this.$emit("ShowUpdateClass", Data);
    },

    async ShowDeleteClass(Data) {
      this.$emit("ShowDeleteClass", Data);
    },
    async ShowInfoClass(Data) {
      this.$emit("ShowInfoClass", Data);
    },
    async ShowAddToInstitueClass(Data) {
      this.$emit("ShowAddToInstitueClass", Data);
    },
  },
};
</script>
