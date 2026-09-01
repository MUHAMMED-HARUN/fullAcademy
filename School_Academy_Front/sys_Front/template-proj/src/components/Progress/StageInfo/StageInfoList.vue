<template>
  <div>
    <table>
      <thead>
        <tr>
          <th>اسم المرحلة</th>
          <th>اسم المادة</th>
          <th>من</th>
          <th>الى</th>
          <th>نوع الوحدة</th>
          <th>نوع العملية</th>
          <th>الاجرائات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="stageinfo in StageInfos" :key="stageinfo.stageInfoID">
          <td>{{ stageinfo.stageName }}</td>
          <td>{{ stageinfo.matterName }}</td>
          <td>{{ stageinfo.from }}</td>
          <td>{{ stageinfo.to }}</td>
          <td>{{ stageinfo.uniteTypeName }}</td>
          <td>{{ stageinfo.progressTypeName }}</td>
          <td>
            <button @click="ShowInfoStageInfo(stageinfo)">معلومات</button>
            <button @click="ShowUpdateStageInfo(stageinfo)">تحديث</button>
            <button @click="ShowDeleteStageInfo(stageinfo)">حذف</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "StageInfoList",
  props: {
    StageInfos: {
      type: Array,
      required: true,
    },
  },
  emits: ["ShowUpdateStageInfo", "ShowDeleteStageInfo", "ShowInfoStageInfo"],
  methods: {
    async ShowUpdateStageInfo(Data) {
      this.$emit("ShowUpdateStageInfo", Data);
    },

    async ShowDeleteStageInfo(Data) {
      this.$emit("ShowDeleteStageInfo", Data);
    },
    async ShowInfoStageInfo(Data) {
      this.$emit("ShowInfoStageInfo", Data);
    },
  },
};
</script>
