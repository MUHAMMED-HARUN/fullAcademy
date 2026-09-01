<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم المادة:</div>
      <div class="item-value">{{ this.DataReq?.name }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">نوع المادة:</div>
      <div class="item-value">{{ this.DataReq?.typeName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">المادة الرئيسية:</div>
      <div class="item-value">{{ this.DataReq?.parentSessionName }}</div>
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
