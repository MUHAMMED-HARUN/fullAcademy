<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم المشروع:</div>
      <div class="item-value">{{ this.DataReq?.name }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الملاحظات:</div>
      <div class="item-value">{{ this.DataReq?.note }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "ProgramCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
