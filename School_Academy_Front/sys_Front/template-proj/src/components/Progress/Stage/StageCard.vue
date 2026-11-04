<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم المرحلة:</div>
      <div class="item-value">{{ this.DataReq?.name }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم الصف:</div>
      <div class="item-value">{{ this.DataReq?.className }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">المرحلة السابقة:</div>
      <div class="item-value">{{ this.DataReq?.prevStageName || "—" }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">الملاحظات:</div>
      <div class="item-value">{{ this.DataReq?.note }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "StageCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
