<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم المعهد:</div>
      <div class="item-value">{{ this.DataReq?.instituteName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم الصف:</div>
      <div class="item-value">{{ this.DataReq?.className }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">مستوى الصف:</div>
      <div class="item-value">{{ this.DataReq?.classLevel }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "InstituteClassCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
