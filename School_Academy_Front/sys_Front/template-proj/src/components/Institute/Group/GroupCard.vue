<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم المجموعة:</div>
      <div class="item-value">{{ this.DataReq?.groupName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">كود المجموعة:</div>
      <div class="item-value">{{ this.DataReq?.code }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم الصف:</div>
      <div class="item-value">{{ this.DataReq?.className }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم المعهد:</div>
      <div class="item-value">{{ this.DataReq?.instituteName }}</div>
    </div>
  </div>
</template>

<script>
export default {
  name: "GroupCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
