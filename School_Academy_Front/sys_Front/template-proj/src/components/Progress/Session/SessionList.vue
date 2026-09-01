<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم المادة</th>
          <th>نوع المادة</th>
          <th>المادة الرئيسية</th>
          <th>ملاحظات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="session in Sessions" :key="session.sessionID">
          <td>{{ session.name }}</td>
          <td>{{ session.typeName }}</td>
          <td>{{ session.parentSessionName }}</td>
          <td>{{ session.note }}</td>

          <td>
            <button @click="ShowInfoSession(session)">معلومات</button>
            <button @click="ShowUpdateSession(session)">تحديث</button>
            <button @click="ShowDeleteSession(session)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "SessionList",
  props: {
    Sessions: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateSession", "ShowDeleteSession", "ShowInfoSession"],
  methods: {
    async ShowUpdateSession(Data) {
      this.$emit("ShowUpdateSession", Data);
    },

    async ShowDeleteSession(Data) {
      this.$emit("ShowDeleteSession", Data);
    },
    async ShowInfoSession(Data) {
      this.$emit("ShowInfoSession", Data);
    },
  },
};
</script>
