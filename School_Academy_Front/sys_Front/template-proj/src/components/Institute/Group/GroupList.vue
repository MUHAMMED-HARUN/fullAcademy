<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم المجموعة</th>
          <th>كود المجموعة</th>
          <th>اسم الصف</th>
          <th>اسم المعهد</th>
          <th>العمليات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="group in groups" :key="group.groupID">
          <td>{{ group.groupName }}</td>
          <td>{{ group.code }}</td>
          <td>{{ group.className }}</td>
          <td>{{ group.instituteName }}</td>
          <td>
            <button @click="ShowInfoGroup(group)">معلومات</button>
            <button @click="ShowUpdateGroup(group)">تحديث</button>
            <button @click="ShowDeleteGroup(group)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "GroupList",
  props: {
    groups: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateGroup", "ShowDeleteGroup", "ShowInfoGroup"],
  methods: {
    async ShowUpdateGroup(Data) {
      this.$emit("ShowUpdateGroup", Data);
    },
    async ShowDeleteGroup(Data) {
      this.$emit("ShowDeleteGroup", Data);
    },
    async ShowInfoGroup(Data) {
      this.$emit("ShowInfoGroup", Data);
    },
  },
};
</script>
