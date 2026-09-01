<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم المشروع</th>
          <th>ملاحظات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="program in programs" :key="program.programID">
          <td>{{ program.name }}</td>
          <td>{{ program.note }}</td>

          <td>
            <button @click="ShowInfoProgram(program)">معلومات</button>
            <button @click="ShowUpdateProgram(program)">تحديث</button>
            <button @click="ShowDeleteProgram(program)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "ProgramList",
  props: {
    programs: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateProgram", "ShowDeleteProgram", "ShowInfoProgram"],
  methods: {
    async ShowUpdateProgram(Data) {
      this.$emit("ShowUpdateProgram", Data);
    },

    async ShowDeleteProgram(Data) {
      this.$emit("ShowDeleteProgram", Data);
    },
    async ShowInfoProgram(Data) {
      this.$emit("ShowInfoProgram", Data);
    },
  },
};
</script>
