<template>
  <div>
    <div class="card-header"></div>
    <div class="card-item">
      <div class="item-name">اسم الطالب:</div>
      <div class="item-value">{{ this.DataReq?.studentName }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">اسم الشعبة:</div>
      <div class="item-value">{{ this.DataReq?.groupName }}</div>
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
      <div class="item-name">التاريخ:</div>
      <div class="item-value">{{ this.DataReq?.date }}</div>
    </div>
    <div class="card-item">
      <div class="item-name">مستوى التقييم:</div>
      <div class="item-value">{{ this.DataReq?.levelType }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "DailyEvaluationCard",
  props: {
    DataReq: {
      type: Object,
      required: true,
    },
  },
};
</script>
