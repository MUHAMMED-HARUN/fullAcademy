<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم المرحلة الاختبار:</div>
      <div class="item-value">{{ this.DataReq?.name }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم المرحلة:</div>
      <div class="item-value">{{ this.DataReq?.stageName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">المرحلة السابقة :</div>
      <div class="item-value">{{ this.DataReq?.prevAssignmentScopeName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الملاحظات:</div>
      <div class="item-value">{{ this.DataReq?.note }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "AssignmentScopeCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
