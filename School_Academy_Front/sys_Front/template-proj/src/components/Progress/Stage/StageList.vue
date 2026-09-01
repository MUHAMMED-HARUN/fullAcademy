<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم المرحلة</th>
          <th>المشروع</th>
          <th>المرحلة السابقة</th>
          <th>ملاحظات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="stage in Stages" :key="stage.stageID">
          <td>{{ stage.name }}</td>
          <td>{{ stage.programName }}</td>
          <td>{{ stage.prevStageName }}</td>
          <td>{{ stage.note }}</td>

          <td>
            <button @click="ShowInfoStage(stage)">معلومات</button>
            <button @click="ShowUpdateStage(stage)">تحديث</button>
            <button @click="ShowDeleteStage(stage)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "StageList",
  props: {
    Stages: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateStage", "ShowDeleteStage", "ShowInfoStage"],
  methods: {
    async ShowUpdateStage(Data) {
      this.$emit("ShowUpdateStage", Data);
    },

    async ShowDeleteStage(Data) {
      this.$emit("ShowDeleteStage", Data);
    },
    async ShowInfoStage(Data) {
      this.$emit("ShowInfoStage", Data);
    },
  },
};
</script>
