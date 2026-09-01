<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم الصف:</div>
      <div class="item-value">{{ this.DataReq?.name }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">مستوى الصف:</div>
      <div class="item-value">{{ this.DataReq?.level }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">المشروع:</div>
      <div class="item-value">{{ this.DataReq?.programName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">ملاحظات الصف:</div>
      <div class="item-value">{{ this.DataReq?.notes }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "ClassCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
