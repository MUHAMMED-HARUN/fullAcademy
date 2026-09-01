<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم المرحلة:</div>
      <div class="item-value">{{ this.DataReq?.stageName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم المادة:</div>
      <div class="item-value">{{ this.DataReq?.matterName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">من:</div>
      <div class="item-value">{{ this.DataReq?.from }}</div>
    </div>

    <div class="card-item">
      <div class="item-name">الى:</div>
      <div class="item-value">{{ this.DataReq?.to }}</div>
    </div>

    <div class="card-item">
      <div class="item-name">نوع الوحدة:</div>
      <div class="item-value">{{ this.DataReq?.uniteTypeName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">نوع العملية:</div>
      <div class="item-value">{{ this.DataReq?.progressTypeName }}</div>
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
