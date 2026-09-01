<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم مرحلة الاختبار:</div>
      <div class="item-value">{{ this.DataReq?.assignmentScopeName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم مادة عنصر المرحلة:</div>
      <div class="item-value">{{ this.DataReq?.stageInfoMatterNameName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">العلامة التامة :</div>
      <div class="item-value">{{ this.DataReq?.maxScore }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الحد الادنى:</div>
      <div class="item-value">{{ this.DataReq?.minPassScore }}</div>
    </div>

    <div class="card-item">
      <div class="item-name">الوزن:</div>
      <div class="item-value">{{ this.DataReq?.weight }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "AssignmentScopeInfoCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
